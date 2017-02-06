using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Retech.Cache;
using LiXinCommon;
using LiXinInterface.User;
using LiXinModels.User;
using LiXinControllers;
using LiXinLanguage.Login;
using LiXinBLL.User;
using LiXinInterface.SystemManage;
using LiXinControllers.Filter;
using System.Configuration;
namespace LiXinControllers.Login
{
    public class LoginController : BaseController
    {
        protected IRight rightBL;
        protected IUser userBL;
        protected ISys_Log logBL;

        public LoginController(IUser _userBL, IRight _rightBL, ISys_Log _logBL)
        {
            userBL = _userBL;
            rightBL = _rightBL;
            logBL = _logBL;
        }

        /// <summary>
        ///     Login
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Session.Clear();
            string backUrl = "";
            if (Session["backUrl"] != null)
            {
                backUrl = Session["backUrl"].ToString();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["backUrl"]))
            {
                backUrl = Request.QueryString["backUrl"];
            }
            ViewBag.backUrl = backUrl;

            #region verification code

            string loginRequireValCode = LoginConfig.LoginConfigurations.IsShowLoginVaildateCode ? "true" : "false";
            ViewBag.loginReqVCode = loginRequireValCode;

            #endregion

            #region remember password

            ViewBag.userName = "";
            ViewBag.password = "";
            ViewBag.remember = 0;
            HttpCookie httpCookie = Request.Cookies["LiXin_User"];
            if (httpCookie != null)
            {
                ViewBag.remember = 1;
                ViewBag.userName = HttpUtility.UrlDecode(httpCookie["username"]);
                ViewBag.password = BaseEncode.DecodingForString(httpCookie["password"]);
            }

            #endregion

            return View();
        }

        #region 登录验证

        public ActionResult LoginVerify(string username, string password, string code, string backUrl, int rememberPwd)
        {
            #region 准备登录成功后，返回的URL

            if (Session["backUrl"] != null)
                backUrl = Session["backUrl"].ToString();
            if (string.IsNullOrEmpty(backUrl) || backUrl == "/" || (!string.IsNullOrEmpty(backUrl) && backUrl.ToLower().IndexOf("indexpage") >= 0))
            {
                backUrl = "/Home/Index";
            }
            if (!backUrl.StartsWith("http://"))
            {
                if (!backUrl.StartsWith("/"))
                    backUrl = "/" + backUrl;
            }
            backUrl = Server.HtmlDecode(backUrl);

            #endregion

            #region 验证是否有空项

            if (string.IsNullOrEmpty(username))
                return LoginJson(2, LoginLanguage.Login_ValidateUserName);
            if (string.IsNullOrEmpty(password))
                return LoginJson(3, LoginLanguage.Login_ValidatePassword);

            #endregion

            #region 验证验证码

            if (LoginConfig.LoginConfigurations.IsShowLoginVaildateCode)
            {
                try
                {
                    string realCode = Session["CheckCode"].ToString();
                    if (string.IsNullOrEmpty(code))
                        return LoginJson(1, LoginLanguage.Login_ValidateCode);
                    if (realCode.ToUpper() != code.ToUpper())
                        return LoginJson(1, LoginLanguage.Login_ValidateCodeTrue);
                }
                catch
                {
                    return LoginJson(1, LoginLanguage.Login_ValidateCodeTrue);
                }
            }

            #endregion

            return CheckLogin(username, password, code, backUrl, rememberPwd);
        }

        private JsonResult CheckLogin(string username, string password, string code, string backUrl, int rememberPwd)
        {
            try
            {
                #region 登录验证

                #region 判断超级管理员

                if (username.Equals(LoginConfig.LoginConfigurations.SuperAdmin, StringComparison.OrdinalIgnoreCase))
                {
                    if (password.Equals(LoginConfig.LoginConfigurations.SuperPassword))
                    {
                        InitSuperAdmin();
                        LoginLog("超级管理员用户登录", 0, HttpContext);
                        return Json(new { result = 0, url = backUrl }, JsonRequestBehavior.DenyGet);
                    }
                }

                #endregion

                #region 判断用户名与密码是否匹配

                Sys_User user = userBL.GetUserByName(username);
                if (user == null)
                    return LoginJson(2, LoginLanguage.Login_CheckUserName);
                if (user.IsDelete == 1)
                    return LoginJson(2, LoginLanguage.Login_NotFindUserName);
                if (!user.Password.Equals(BaseEncode.GetMd5Str(password)))
                    return CheckPwdCount(user);

                #endregion

                #region 判断用户的状态是否正常

                //异常状况，需要判断锁定时间
                if (user.Status == 1)
                {
                    if (!user.FreezeTime.HasValue || user.FreezeTime > DateTime.Now)
                        return LoginJson(5, LoginLanguage.Login_FreezeAccount);
                    if (user.FreezeTime.HasValue && user.FreezeTime < DateTime.Now)
                    {
                        user.Status = 0;
                        user.FreezeTime = null;
                    }
                }

                #endregion

                #endregion

                #region 登录验证通过

                //TODO:大并发时有问题
                //SoleUser(user);
                if (rememberPwd == 1)
                    RememberPwd(username, password);
                //记录最后一次登录时间
                user.LastLoginTime = DateTime.Now;
                user.PasswordFailureCount = 0;
                user.PasswordFailureTime = null;
                userBL.Update(user);
                //初始化用户权限
                UserRights = rightBL.GetRightByUserId(user.UserId);
                CurrentUser = user;
                FormsAuthentication.SetAuthCookie(user.Username, true);

                LoginLog("用户登录", CurrentUser.UserId, HttpContext);

                #endregion

                return Json(new { result = 0, url = backUrl }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = 4, url = backUrl,message=ex.Message+ex.StackTrace }, JsonRequestBehavior.AllowGet);
            }
        }

        private void LoginLog(string logContent, int userId, HttpContextBase context)
        {
            string requestUrl = "";
            int indexMark = context.Request.RawUrl.IndexOf("/", 1);
            if (indexMark > 0)
            {
                requestUrl = context.Request.RawUrl.Substring(1, indexMark - 1);
            }
            var log = new LiXinModels.SystemManage.Sys_Log()
            {
                ClientIP = LiXinCommon.CodeHelper.GetClientIp(context.Request),
                LogTime = DateTime.Now,
                LogType = 1,
                LogContent = logContent,
                ModuleName = requestUrl,
                RequestUrl = context.Request.RawUrl,
                CurrentUserId = userId
            };
            logBL.Add(log);
        }

        /// <summary>
        ///     验证密码的错误次数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private JsonResult CheckPwdCount(Sys_User user)
        {
            DateTime today = DateTime.Now.AddDays(1);
            if (user.PasswordFailureTime.HasValue)
            {
                if (user.PasswordFailureTime.Value.Date == DateTime.Now.Date)
                {
                    if (user.PasswordFailureCount >= LoginConfig.LoginConfigurations.AllowFailurePasswordCount)
                    {
                        return LoginJson(3, string.Format(LoginLanguage.Login_ErrorPassword, user.PasswordFailureCount));
                    }
                    user.PasswordFailureCount++;
                    if (user.PasswordFailureCount >= LoginConfig.LoginConfigurations.AllowFailurePasswordCount)
                    {
                        //TODO:数据库操作
                        userBL.SetUserStatus(user.UserId.ToString(CultureInfo.InvariantCulture), 1, today);
                        return LoginJson(3, string.Format(LoginLanguage.Login_AccountsLocked, user.PasswordFailureCount));
                    }
                    else
                    {
                        userBL.UpdateUserPwdStatus(user.UserId.ToString(CultureInfo.InvariantCulture),
                                                   user.PasswordFailureCount, DateTime.Now);
                    }
                }
                else
                {
                    user.PasswordFailureCount = 1;
                    userBL.UpdateUserPwdStatus(user.UserId.ToString(CultureInfo.InvariantCulture),
                                               user.PasswordFailureCount, DateTime.Now);
                }
            }
            else
            {
                user.PasswordFailureCount = 1;
                userBL.UpdateUserPwdStatus(user.UserId.ToString(CultureInfo.InvariantCulture), user.PasswordFailureCount,
                                           DateTime.Now);
            }
            return LoginJson(3, string.Format(LoginLanguage.Login_CheckPassword, user.PasswordFailureCount));
        }

        /// <summary>
        ///     登录的唯一性
        /// </summary>
        /// <param name="user"></param>
        private void SoleUser(Sys_User user)
        {
            Dictionary<string, int> dict = cacheManager.Get("exists_sessions",
                                                            () => { return new Dictionary<string, int>(); });
            if (dict.ContainsKey(Session.SessionID))
            {
                dict.Remove(Session.SessionID);
                dict.Add(Session.SessionID, user.UserId);
            }
            else
            {
                //替换键
                KeyValuePair<string, int> kv = dict.FirstOrDefault(p => p.Value == user.UserId);
                if (kv.Key != null)
                {
                    dict.Remove(kv.Key);
                }
                dict.Add(Session.SessionID, user.UserId);
            }
            cacheManager.Remove("exists_sessions");
            cacheManager.Get("exists_sessions", () => { return dict; });
        }

        /// <summary>
        ///     记住密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        private void RememberPwd(string username, string password)
        {
            if (Request.Form["rememberPwd"] == "1")
            {
                var newCookie = new HttpCookie("LiXin_User");
                newCookie.Values["username"] = HttpUtility.UrlEncode(username);
                newCookie.Values["password"] = BaseEncode.EncodingForString(password); //加密后的密码放到COOKIE内
                newCookie.Expires = DateTime.Now.AddDays(15);
                Response.AppendCookie(newCookie);
            }
            else
            {
                HttpCookie cookie = Request.Cookies["LiXin_User"];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-2);
                    Response.Cookies.Set(cookie);
                }
            }
        }

        private void InitSuperAdmin()
        {
            UserRights = AllRights;
            CurrentUser = InitSuper;
        }


        private JsonResult LoginJson(int flag, string errorString)
        {
            return Json(new
                {
                    result = flag,
                    content = errorString
                }, JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region 退出

        public ActionResult LoginOut()
        {
            try
            {
                //RemoveSSO();
                //LoginLog("用户退出", CurrentUser.UserId, HttpContext);
                Session.Clear();
                FormsAuthentication.SignOut();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private void RemoveSSO()
        {
            Dictionary<string, int> dict = cacheManager.Get("exists_sessions",
                                                            () => { return new Dictionary<string, int>(); });
            if (dict != null)
            {
                dict.Remove(Session.SessionID);
                cacheManager.Remove("exists_sessions");
                cacheManager.Get("exists_sessions", () => { return dict; });
            }
        }

        #endregion

        #region 生成验证码

        public void GetVCode()
        {
            string vcode;
            Bitmap basemap = CodeHelper.GetValidateCodeMap(out vcode);
            Session["CheckCode"] = vcode;
            basemap.Save(Response.OutputStream, ImageFormat.Gif);
            Response.End();
        }

        #endregion

        #region 验证验证码

        public ActionResult CheckVCode(string code)
        {
            try
            {
                if (!LoginConfig.LoginConfigurations.IsShowLoginVaildateCode)
                    return Json(1, JsonRequestBehavior.AllowGet);
                //屏蔽验证码
                if (Session["CheckCode"].ToString().ToUpper() == code.ToUpper())
                    return Json(1, JsonRequestBehavior.AllowGet);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CheckCode(string code)
        {
            if (Session["CheckCode"].ToString() == code)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        #endregion

        /// <summary>
        /// 单点登录
        /// </summary>
        /// <param name="loginid">用户名</param>
        /// <param name="token">用户名+当天时间（yyyyMMdd）+systemid 经过MD5加密</param>
        public void SSO(string loginid, string token)
        {
            string loginFrom = ConfigurationManager.AppSettings["loginFrom"];
            string loginFailUrl = ConfigurationManager.AppSettings["loginFailUrl"];
            try
            {
                var test= BaseEncode.GetMd5Str(loginid + DateTime.Now.ToString("yyyyMMdd") + loginFrom);
                if (token.ToLower() == test.ToLower())
                {
                    Sys_User user = userBL.GetUserByLoginId(loginid);
                    if (user != null)
                    {
                        //记录最后一次登录时间
                        user.LastLoginTime = DateTime.Now;
                        user.PasswordFailureCount = 0;
                        user.PasswordFailureTime = null;
                        userBL.Update(user);
                        //初始化用户权限
                        UserRights = rightBL.GetRightByUserId(user.UserId);
                        CurrentUser = user;
                        FormsAuthentication.SetAuthCookie(user.Username, true);
                        LoginLog("用户登录", CurrentUser.UserId, HttpContext);
                        Response.Redirect("/Home/Index", true);
                    }
                    else
                    {
                        Response.Redirect(loginFailUrl, true);
                    }
                }
                else
                {
                    Response.Redirect(loginFailUrl, true);
                }
            }
            catch
            {
                Response.Redirect(loginFailUrl, true);
            }
        }
    }
}