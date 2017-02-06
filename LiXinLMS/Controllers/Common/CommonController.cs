using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LiXinCommon;
using LiXinModels.User;
using LiXinBLL.SystemManage;
using LiXinModels;
using LiXinBLL;

namespace LiXinControllers
{
    /// <summary>
    ///     通过模块
    /// </summary>
    public class CommonController : BaseController
    {
        public ActionResult RightError(string backUrl)
        {
            ViewBag.backUrl = backUrl;
            return View();
        }


        public ActionResult SiteMapLink(string linkName)
        {
            var str = "";
            GetRightPath(AllRights, linkName, ref str);

            ViewBag.CnTitle = str;//CodeHelper.GetNavigateString(linkName, new CultureInfo("zh-CN")) ?? linkName;
            //ViewBag.UsTitle = CodeHelper.GetNavigateString(linkName, new CultureInfo("en-US")) ?? linkName;
            return View();
        }

        /// <summary>
        ///     获取当前请求的页面的菜单
        /// </summary>
        /// <param name="urs">权限集合</param>
        /// <param name="rstr">权限名称</param>
        /// <param name="path">路径</param>
        /// <param name="flag">层级标志</param>
        /// <returns></returns>
        private void GetRightPath(List<Sys_Right> urs, string rstr,ref string path,int flag=0)
        {
            if (flag == 0)
            {
                path = (CodeHelper.GetNavigateString(rstr, new CultureInfo("zh-CN")) ?? rstr) + path;
            }else
            {
                path = (CodeHelper.GetNavigateString(rstr, new CultureInfo("zh-CN")) ?? rstr) + string.Format(" > {0}",flag==1?"</span>":"") + path;
            }
            flag++;
            var obj = urs.FirstOrDefault(p => p.RightName == rstr);
            if (obj != null)
            {
                var fr = urs.FirstOrDefault(p => p.RightId == obj.ParentId);
                if (fr != null)
                    GetRightPath(urs, fr.RightName, ref path, flag);
                else
                    path = "<span>" + path;
            }
        }

        /// <summary>
        /// 获取我的模块
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyModule()
        {
            var module1 = UserRights.Where(p => p.ModuleName != null && p.ModuleName.Trim().ToLower() == "MyTrain".ToLower());
            var module2 = UserRights.Where(p => p.ModuleName != null && p.ModuleName.Trim().ToLower() == "TrainManage".ToLower());
            var module3 = UserRights.Where(p => p.ModuleName != null && p.ModuleName.Trim().ToLower() == "nodo".ToLower());
            var module4 = UserRights.Where(p => p.ModuleName != null && p.ModuleName.Trim().ToLower() == "SystemManage".ToLower());
            //var module5 = UserRights.Where(p => p.ModuleName != null && p.ModuleName.Trim().ToLower() == "Announcement".ToLower());
            var module6 = UserRights.Where(p => p.ModuleName != null && p.ModuleName.Trim().ToLower() == "FAQ".ToLower());

            var list = new List<object>();
            if (module1.Any())
                list.Add(new
                {
                    name = "我的培训",
                    module = "MyTrain",
                    ename = "My Training",
                    order = list.Count + 1
                });
            if (module2.Any())
                list.Add(new
                {
                    name = "培训管理",
                    module = "TrainManage",
                    ename = "Training Management",
                    order = list.Count + 1
                });
            if (module3.Any())
                list.Add(new
                {
                    name = "部门/分所管理",
                    module = "nodo",
                    ename = "Department / Office Management",
                    order = list.Count + 1
                });
            if (module4.Any())
                list.Add(new
                {
                    name = "系统管理",
                    module = "SystemManage",
                    ename = "System Management",
                    order = list.Count + 1
                });
            //if (module5.Any())
            //    list.Add(new
            //    {
            //        name = "系统公告",
            //        module = "Announcement",
            //        ename = "The System Notification",
            //        order = list.Count + 1
            //    });
            if (module6.Any())
                list.Add(new
                {
                    name = "培训政策及FAQ",
                    module = "FAQ",
                    ename = "Department / Office Management",
                    order = list.Count + 1
                });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MenuHtml()
        {
            if (UserRights.Count == 0)
            {
                ViewBag.MenuStr = "";
                return View();
            }

            string rurl = Request.Url.AbsolutePath.Remove(0, 1); //当前请求的页面url
            string[] urls = rurl.Split('/');
            rurl = urls[0] + "/" + urls[1];
            var rt = GetSubMenu(UserRights, rurl, 0);

            var htmlMenu = new StringBuilder("");
            var rootRight = UserRights.First(p => p.ParentId == 0);
            var tempUserRights = UserRights.Where(p => p.ParentId == rootRight.RightId && p.RightType == 0 && (p.ModuleName == null ? "" : p.ModuleName).ToLower() == Session["moduleName"].ToString().ToLower()).OrderBy(p => p.ShowOrder);
            foreach (var right in tempUserRights)
            {
                string path = right.Path.IndexOf("/", System.StringComparison.Ordinal) >= 0 ? right.Path : "#";
                string title = CodeHelper.GetNavigateString(right.RightName);
                if (string.IsNullOrEmpty(title))
                    title = right.RightName;
                if (path!="#" && !path.StartsWith("/"))
                {
                    path = "/" + path;
                }
                var subRight = UserRights.Where(p => p.ParentId == right.RightId && p.RightType == 0);
                htmlMenu.AppendFormat(
                    "<li name='{0}'><a class='" + ((rt != null && rt.Path.ToLower() == right.Path.ToLower()) ? "On" : "") + "' href='{3}'>{1}</a>{2}{4}",
                    right.RightName, 
                    title, 
                    (subRight.Any() ? "<i></i>" : ""), 
                    (subRight.Any() ? "#" : path),
                    (subRight.Any()?"<ul>":""));
                foreach (var c in subRight.OrderByDescending(p => p.ShowOrder))
                {
                    var p = c.Path;
                    if (!p.StartsWith("/"))
                    {
                        p = "/" + p;
                    }
                    var t = CodeHelper.GetNavigateString(c.RightName);
                    if (string.IsNullOrEmpty(t))
                    {
                        t = c.RightName;
                    }
                    htmlMenu.AppendFormat("<li><a href='{0}'><span>{1}</span></a></li>", p, t);
                }
                htmlMenu.Append(string.Format("{0}</li>", (subRight.Any() ? "</ul>" : "")));
            }

            ViewBag.MenuStr = htmlMenu.ToString();
            return View();
        }

        /// <summary>
        /// 菜单跳转连接
        /// </summary>
        public ActionResult RedirectUrl(string blockName)
        {
            Session["moduleName"] = blockName;
            var redirectUrl = "";
            switch (blockName)
            {
                case "MyTrain":
                    redirectUrl = "/Home/MyTrainIndex";
                    break;
                case "TrainManage":
                    redirectUrl = "/PlanManage/Index";
                    break;
                case "nodo":
                    redirectUrl = "";
                    break;
                case "SystemManage":
                    redirectUrl = "/Home/SystemManageIndex";
                    break;
                case "Announcement":
                    redirectUrl = "/SystemManage/NoteListShow";
                    break;
                case "FAQ":
                    redirectUrl = "/SystemManage/FAQ_NoteListShow";
                    break;
                case "Person":
                    redirectUrl = "/UserManage/UserInfoIndex";
                    break;
                default:
                    redirectUrl = "";
                    break;
            }
            if (redirectUrl == "")
                return View("ErroMessage");
            Response.Redirect(redirectUrl);
            return View("");
            //var right = UserRights.FirstOrDefault(p => p.RightName == blockName);
            //if (right == null) { return View("ErroMessage"); }
            //else
            //{
            //    var r = UserRights.Where(p => p.ParentId == right.RightId && p.RightType == 0).OrderBy(p => p.ShowOrder).First();
            //    Response.Redirect("/" + r.Path);
            //    return View("");
            //}
        }

        /// <summary>
        ///     获取当前请求的页面的菜单
        /// </summary>
        /// <param name="urs">权限集合</param>
        /// <param name="url">当前请求的url</param>
        /// <param name="flag">0：总（一级）菜单；1：子（二级）菜单</param>
        /// <returns></returns>
        private Sys_Right GetSubMenu(List<Sys_Right> urs, string url, int flag)
        {
            Sys_Right obj = urs.FirstOrDefault(p => p.Path.ToLower() == url.ToLower());
            if (obj != null)
            {
                Session["moduleName"] = string.IsNullOrEmpty(obj.ModuleName)
                                            ? Session["moduleName"].ToString()
                                            : obj.ModuleName;
                Sys_Right fr = urs.FirstOrDefault(p => p.RightId == obj.ParentId);
                if (obj.RightType == 0 &&
                    (flag == 1 || (flag == 0 && (fr != null && fr.ParentId == 0))))
                    return obj;
                Sys_Right sysRight = urs.FirstOrDefault(p => p.RightId == obj.ParentId);
                if (sysRight != null)
                    return GetSubMenu(urs, sysRight.Path, flag);
            }
            return null;
        }

        /// <summary>
        /// 出错提示
        /// </summary>
        /// <returns></returns>
        public ActionResult ErroMessage()
        {
            return View();
        }

        /// <summary>
        /// 文件上传调用的方法
        /// </summary>
        /// <param name="FileData"></param>
        /// <param name="folder"></param>
        /// <param name="detailFlag">detailFlag : 0 :返回上传后的文件名 1: 返回 "原始文件名,Guid文件名,文件大小"</param>
        /// <returns></returns>
        public ContentResult UploadFileAction(HttpPostedFileBase FileData, string folder, int detailFlag = 0)
        {
            string filename = "";
            string resultName = "";
            if (null != FileData)
            {
                try
                {
                    filename = Path.GetFileName(FileData.FileName); //获得文件名
                    string fullPathname = Path.Combine(folder, filename);
                    //文件后缀名
                    string suffix = FileData.FileName.Substring(FileData.FileName.LastIndexOf(".") + 1).ToLower();
                    resultName = Guid.NewGuid() + "." + suffix;
                    saveFile(FileData, folder, resultName);
                }
                catch (Exception ex)
                {
                    resultName = ex.ToString();
                }
            }
            if (detailFlag == 1)
            {
                resultName = FileData.FileName + "|" + resultName + "|" + FileData.ContentLength / 1024;
            }
            return Content(resultName);
        }

        private bool saveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
        {
            bool result = false;
            string a = HttpContext.Server.MapPath(filepath);
            if (!Directory.Exists(a))
            {
                Directory.CreateDirectory(a);
            }
            try
            {
                postedFile.SaveAs((a + "\\" + saveName));
                result = true;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            return result;
        }

        /// <summary>
        /// 获取所有培训级别
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllTrainGrade()
        {
            return Json(AllTrainGrade, JsonRequestBehavior.AllowGet);
        }


       
    }
}