using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Web.Mvc;
using LiXinModels.User;
using System.Text.RegularExpressions;
using LiXinCommon;
using LiXinLanguage.UserManage;
using LiXinControllers.Filter;
using LiXinModels;
using LiXinModels.PlanManage;

namespace LiXinControllers.User
{
    public partial class UserManageController : BaseController
    {
        private LiXinSync.SyncInfo syncUserInfo = new LiXinSync.SyncInfo();

        [SystemLog("账户管理", LogLevel.Info)]
        public ActionResult UserManage()
        {
            ViewBag.RoleList = roleBL.GetAllRoles().Where(p => p.IsDelete == 0).ToList();
            ViewBag.TrainGrade = AllTrainGrade;

            ViewBag.UserTypeList = userBL.GetUserType();

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobNum">工号</param>
        /// <param name="realName">姓名</param>
        /// <param name="post">岗位</param>
        /// <param name="dept">部门</param>
        /// <param name="sex">性别  【99：全部；0：男；1：女】</param>
        /// <param name="status">状态  【99：全部；0：正常；1：冻结】</param>
        /// <param name="email">邮箱</param>
        /// <param name="deptId">部门ID</param>
        /// <param name="postId">岗位ID</param>
        /// <param name="NotShowHasPost">是否显示有岗位的  【0：显示；1：不显示；默认为0】</param>
        /// <param name="NotShowHasDept">是否显示有部门的  【0：显示；1：不显示；默认为0】</param>
        /// <param name="flag">
        /// 0：本部门、本岗位
        /// 1：包含下级部门、本岗位
        /// 2：本部门、包含下级岗位
        /// 3：本部门、本岗位、包含下级部门、下级岗位
        /// </param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetUserList(string jobNum, string realName, string post, string dept, int sex, int status, string email, int deptId = 0, int postId = 0, int NotShowHasPost = 0, int NotShowHasDept = 0, int flag = 0, int pageSize = 20, int pageIndex = 1)
        {
            int totalCount = 0;
            var deptIds = "0";
            var postIds = "0";
            if (flag == 0)
            {
                deptIds = deptId.ToString();
                postIds = postId.ToString();
            }
            else if (flag == 1)//根据部门Id获取该部门及其子部门的ID的集合
            {
                if (deptId != 0)
                {
                    var deptList = new List<int>();
                    GetChildDeptIds(deptId, deptList);
                    deptIds = deptList.Aggregate("", (current, item) => current + (item + ",")) + deptId;
                }
            }
            else if (flag == 2)//根据岗位Id获取该部门及其子岗位的ID的集合
            {
                if (postId != 0)
                {
                    var postList = new List<int>();
                    GetChildPostIds(postId, postList);
                    postIds = postList.Aggregate("", (current, item) => current + (item + ",")) + postId;
                }
            }
            else if (flag == 3)//根据部门Id，岗位Id获取该部门及其子部门，子岗位的ID的集合
            {
                if (deptId != 0)
                {
                    var deptList = new List<int>();
                    GetChildDeptIds(deptId, deptList);
                    deptIds = deptList.Aggregate("", (current, item) => current + (item + ",")) + deptId;
                }
                if (postId != 0)
                {
                    var postList = new List<int>();
                    GetChildPostIds(postId, postList);
                    postIds = postList.Aggregate("", (current, item) => current + (item + ",")) + postId;
                }
            }
            var list = userBL.GetList(out totalCount, deptIds, postIds, dept.ReplaceSql(), post.ReplaceSql(), email.ReplaceSql(), jobNum.ReplaceSql(), realName.ReplaceSql(), sex, status, NotShowHasPost, NotShowHasDept, (pageIndex - 1) * pageSize, pageSize);
            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobNum">工号</param>
        /// <param name="realName">姓名</param>
        /// <param name="post">岗位</param>
        /// <param name="dept">部门</param>
        /// <param name="sex">性别  【99：全部；0：男；1：女】</param>
        /// <param name="status">状态  【99：全部；0：正常；1：冻结】</param>
        /// <param name="email">邮箱</param>
        /// <param name="deptId">部门ID</param>
        /// <param name="postId">岗位ID</param>
        /// <param name="NotShowHasPost">是否显示有岗位的  【0：显示；1：不显示；默认为0】</param>
        /// <param name="NotShowHasDept">是否显示有部门的  【0：显示；1：不显示；默认为0】</param>
        /// <param name="flag">
        /// 0：本部门、本岗位
        /// 1：包含下级部门、本岗位
        /// 2：本部门、包含下级岗位
        /// 3：本部门、本岗位、包含下级部门、下级岗位
        /// </param>
        public void ExportUserList(string jobNum, string realName, string post, string dept, int sex, int status, string email, int deptId = 0, int postId = 0, int flag = 0)
        {
            int totalCount = 0;
            var deptIds = "0";
            var postIds = "0";
            if (flag == 1)
            {
                if (deptId != 0)
                {
                    var deptList = new List<int>();
                    GetChildDeptIds(deptId, deptList);
                    deptIds = deptList.Aggregate("", (current, item) => current + (item + ",")) + deptId;
                }
            }
            else if (flag == 2)
            {
                if (postId != 0)
                {
                    var postList = new List<int>();
                    GetChildPostIds(postId, postList);
                    postIds = postList.Aggregate("", (current, item) => current + (item + ",")) + postId;
                }
            }
            else if (flag == 3)
            {
                if (deptId != 0)
                {
                    var deptList = new List<int>();
                    GetChildDeptIds(deptId, deptList);
                    deptIds = deptList.Aggregate("", (current, item) => current + (item + ",")) + deptId;
                }
                if (postId != 0)
                {
                    var postList = new List<int>();
                    GetChildPostIds(postId, postList);
                    postIds = postList.Aggregate("", (current, item) => current + (item + ",")) + postId;
                }
            }
            var list = userBL.GetList(out totalCount, deptIds, postIds, dept.ReplaceSql(), post.ReplaceSql(), email.ReplaceSql(), jobNum.ReplaceSql(), realName.ReplaceSql(), sex, status);

            var dt = new DataTable();
            dt.Columns.Add("工号", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("性别", typeof(string));
            dt.Columns.Add("部门", typeof(string));
            dt.Columns.Add("岗位", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("状态", typeof(string));
            foreach (Sys_User item in list)
            {
                dt.Rows.Add(item.JobNum, item.Realname, item.SexStr, item.DeptCodeStr, item.PostCodeStr, item.Email,
                            item.StatusStr);
            }
            var dtList = new List<DataTable>();
            string strFileName = "用户数据";
            dtList.Add(dt);
            var export = new Spreadsheet();
            export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, strFileName);
        }

        /// <summary>
        /// 获取用户列表（立信新的）
        /// </summary>
        /// <param name="jobNum">工号</param>
        /// <param name="realName">姓名</param>
        /// <param name="dept">部门</param>
        /// <param name="sex">性别  【99：全部；0：男；1：女】</param>
        /// <param name="status">状态  【99：全部；0：正常；1：冻结】</param>
        /// <param name="trainGrade">培训级别</param>
        /// <param name="deptId">部门ID</param>
        /// <param name="flag">
        /// 0：本部门
        /// 1：包含下级部门
        /// </param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="IsNew">0所有人  1不是新进员工 2新进员工</param>
        /// <returns></returns>
        public JsonResult GetUserInfoList(string jobNum, string realName, string email, string deptName, int sex, int status, string trainGrade, int cpa = 99, string usertype = "99", int roleId = 0, int deptId = 0, int flag = 0, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Sys_User.UserId desc", int IsNew = 0, string selectflag = "")
        {
            int totalCount = 0;
            var deptIds = deptId.ToString();
            if (flag == 1)//根据部门Id获取该部门及其子部门的ID的集合
            {
                if (deptId != 0)
                {
                    var deptList = new List<int>();
                    GetChildDeptIds(deptId, deptList);
                    deptIds = deptList.Aggregate("", (current, item) => current + (item + ",")) + deptId;
                }
            }
            string where = " Sys_User.IsDelete = 0 and Sys_User.IsTeacher < 2";
            if (selectflag == "vedio")
            {
                where += " And charindex('上海',Sys_User.SubordinateSubstation)=0 ";
            }
            if (deptIds != "0")
                where += string.Format(" and Sys_User.DeptId in ({0})", deptIds);
            if (!string.IsNullOrWhiteSpace(deptName))
                where += string.Format(" and Sys_User.DeptPath like '%{0}%' ", deptName.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(email))
                where += string.Format(" and Sys_User.Email like '%{0}%' ", email.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(jobNum))
                where += string.Format(" and Sys_User.JobNum like '%{0}%' ", jobNum.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(realName))
                where += string.Format(" and Sys_User.Realname like '%{0}%' ", realName.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(trainGrade) && trainGrade != "99")
                where += string.Format(" and ('{0}' like '%'+ Sys_User.TrainGrade +'%' and (Sys_User.TrainGrade is not null and Sys_User.TrainGrade <> '')) ", trainGrade);
            if (cpa != 99)
                where += string.Format(" and Sys_User.CPA = '{0}' ", cpa == 1 ? "是" : "否");
            if (usertype != "99")
            {
                where += string.Format(" and Sys_User.UserType = '{0}' ", usertype.Trim());
            }
            if (sex != 99)
                where += " and Sys_User.Sex = " + sex;
            if (roleId != 0)
                where += string.Format(" and Sys_User.UserId in (select distinct userid from Sys_UserRole where roleid = {0}) ", roleId);
            if (status != 99)
            {
                if (status == 1)
                    where += string.Format(" and ((Sys_User.status = 1 and Sys_User.FreezeTime is  null) or (Sys_User.status = 1 and Sys_User.FreezeTime is not null and Sys_User.FreezeTime > '{0}') or Sys_User.status = 2) ", DateTime.Now);
                else if (status == 0)
                    where += string.Format(" and ((Sys_User.Status = 0) or (Sys_User.Status = 1 and Sys_User.FreezeTime is not null and Sys_User.FreezeTime < '{0}')) ", DateTime.Now);
            }
            if (IsNew != 0)
            {
                if (IsNew == 1)
                {
                    where += " and (Sys_User.IsNew=0 or Sys_User.IsNew is null)";
                }
                else
                {
                    where += " and Sys_User.IsNew=1";
                }
            }
            var list = userBL.GetList(out totalCount, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);

            int n = 0;
            var itemArray = new object[list.Count()];
            foreach (var item in list)
            {
                var temp = new
                {
                    JobNum = item.JobNum,
                    Realname = item.Realname,
                    SexStr = item.SexStr,
                    DeptName = item.DeptPath,
                    TrainGrade = item.TrainGrade,
                    Telephone = item.Telephone,
                    Email = item.Email,
                    RoleName = item.RoleName.HtmlXssEncode(),
                    StatusStr = item.StatusStr,
                    UserId = item.UserId,
                    DeptId = item.DeptId,
                    PayGrade = string.IsNullOrWhiteSpace(item.PayGrade) ? "--" : item.PayGrade,
                    CPA = item.CPA,
                    Status = item.Status
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        public void ExportDirtyUserList()
        {
            //1:名称为空；
            //2:名称重名；
            //3:培训级别不为（E A C B D T N）；
            //4:培训级别为空；
            //4:域账号为空（即登录名为空）；
            int totalCount = 0;
            var dt = new DataTable();
            dt.Columns.Add("HRID", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("域账号", typeof(string));
            dt.Columns.Add("部门", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("手机", typeof(string));
            dt.Columns.Add("培训级别", typeof(string));
            dt.Columns.Add("错误类别", typeof(string));

            var list = new List<Sys_User>();
            //1
            list = userBL.GetList(out totalCount, @"(Sys_User.RealName = '' or Sys_User.RealName is null) ");
            foreach (Sys_User item in list)
            {
                dt.Rows.Add(item.JobNum, item.Realname, item.LoginId, item.DeptPath, item.Email, item.MobileNum, item.TrainGrade, "姓名为空");
            }
            //2
            list = userBL.GetList(out totalCount, @"(Sys_User.Realname in (select Realname from (select Realname,COUNT(0) as cc from Sys_User where Realname <> '' and realname is not null group by Realname) tt where tt.cc>1))", 1, 100000, "order by sys_user.realname");
            foreach (Sys_User item in list)
            {
                dt.Rows.Add(item.JobNum, item.Realname, item.LoginId, item.DeptPath, item.Email, item.MobileNum, item.TrainGrade, "姓名重名");
            }
            //3,4
            list = userBL.GetList(out totalCount, @"(Sys_User.TrainGrade not in (select Grade from TrainGrade)) and (Sys_User.RealName <> '' and Sys_User.RealName is not null) and (Sys_User.Realname not in (select Realname from (select Realname,COUNT(0) as cc from Sys_User where Realname <> '' and realname is not null group by Realname) tt where tt.cc>1))");
            foreach (Sys_User item in list)
            {
                dt.Rows.Add(item.JobNum, item.Realname, item.LoginId, item.DeptPath, item.Email, item.MobileNum, item.TrainGrade, "培训级别有误");
            }
            //5
            list = userBL.GetList(out totalCount, @"(Sys_User.LoginId = '' or Sys_User.LoginId is null) and (Sys_User.TrainGrade in (select Grade from TrainGrade)) and (Sys_User.RealName <> '' and Sys_User.RealName is not null) and (Sys_User.Realname not in (select Realname from (select Realname,COUNT(0) as cc from Sys_User where Realname <> '' and realname is not null group by Realname) tt where tt.cc>1))");
            foreach (Sys_User item in list)
            {
                dt.Rows.Add(item.JobNum, item.Realname, item.LoginId, item.DeptPath, item.Email, item.MobileNum, item.TrainGrade, "域账号为空");
            }

            var dtList = new List<DataTable>();
            string strFileName = "用户错误数据";
            dtList.Add(dt);
            var export = new Spreadsheet();
            export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, strFileName);
        }



        [SystemLog("编辑账户", LogLevel.Info)]
        public ActionResult UserEdit(int id = 0)
        {
            var model = new Sys_User
            {
                UserId = id,
                JobNum = "",
                Email = "",
                Realname = "",
                DeptCode = "",
                PostCode = ""
            };
            if (id != 0)
                model = userBL.Get(id);
            ViewBag.model = model;
            return View();
        }

        public JsonResult UpdateUserPwd(int id)
        {
            try
            {
                Sys_User model = userBL.Get(id);
                model.Password = BaseEncode.GetMd5Str("123456");
                userBL.Update(model);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateUserStatus(int userId)
        {
            try
            {
                Sys_User model = userBL.Get(userId);
                model.Status = (model.Status + 1) % 2;
                userBL.Update(model);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [SystemLog("删除账户", LogLevel.Info)]
        public JsonResult DeleteUser(string userIds)
        {
            try
            {
                userBL.Delete(userIds);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult SaveUser(int userId, string jobNum, string realName, string email, int sex, int deptId,
                                   string deptName, int postId, string postName)
        {
            try
            {
                if (userBL.Exists(jobNum.ReplaceSingleSql(), jobNum.ReplaceSingleSql(), userId))
                {
                    return Json(new
                    {
                        result = 0,
                        content = "工号重复！"
                    }, JsonRequestBehavior.AllowGet);
                }
                var model = new Sys_User();
                if (userId != 0)
                    model = userBL.Get(userId);
                model.JobNum = jobNum;
                model.Username = jobNum;
                model.Password = BaseEncode.GetMd5Str("123456");
                model.Realname = realName;
                model.Ename = realName;
                model.Email = email;
                model.Sex = sex;
                model.PostId = -1;
                model.DeptId = -1;
                var postTemp = AllPosts.Find(p => p.PostId == postId);
                if (postTemp != null)
                {
                    model.PostId = postId;
                    model.PostCode = postTemp.PostCode;
                    model.PostName = postTemp.PostName;
                    //model.PostLevel = postTemp.PostLevel;
                }
                var deptTemp = AllDepartments.Find(p => p.DepartmentId == deptId);
                if (deptTemp != null)
                {
                    model.DeptId = deptId;
                    model.DeptCode = deptTemp.DeptCode;
                    model.DeptName = deptTemp.DeptName;
                }
                if (userId == 0)
                    userBL.Add(model);
                else
                    userBL.Update(model);

                return Json(new
                {
                    result = 1,
                    content = "保存成功！"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "保存失败！"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddRoleToUser(string userIds, string roleIds)
        {
            try
            {
                userBL.AddRoleToUser(userIds, roleIds);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SetUserStatus(int id)
        {
            ViewBag.userId = id;
            return View();
        }

        public JsonResult CloseUser(int userId, DateTime? time)
        {
            int uid = CurrentUser.UserId;
            if (uid == userId)
                return Json(new
                {
                    result = 0,
                    content = UserManageLanguage.UserManage_CannotNoFreeze
                },
                            JsonRequestBehavior.AllowGet);
            if (!time.HasValue)
            {
                //永久锁定
                userBL.SetUserStatus(userId.ToString(CultureInfo.InvariantCulture), 1, null);
            }
            else
            {
                if (time.Value < DateTime.Now)
                {
                    return Json(new
                    {
                        result = 0,
                        content = UserManageLanguage.UserManage_FreezeTimeLessThanNowTime
                    },
                                JsonRequestBehavior.AllowGet);
                }
                userBL.SetUserStatus(userId.ToString(CultureInfo.InvariantCulture), 1, time);
            }
            return Json(new
            {
                result = 1,
                content = "保存成功"
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OpenUser(int id)
        {
            try
            {
                Sys_User user = userBL.Get(id);
                user.Status = 0;
                user.PasswordFailureTime = null;
                user.PasswordFailureCount = 0;
                user.FreezeTime = null;
                userBL.Update(user);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UserBaseInfo()
        {
            ViewBag.User = CurrentUser;
            return View();
        }

        public ActionResult UserDetail(int id)
        {
            ViewBag.User = userBL.Get(id);
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult SaveUserBaseInfo(string idcard, string mobnum, string email, string memo, string photoUrl)
        {
            try
            {
                Sys_User user = CurrentUser;

                user.IdCardNo = idcard;
                user.MobileNum = mobnum;
                user.Email = email;
                user.Memo = memo;
                user.PhotoUrl = photoUrl;
                bool result = userBL.Update(user);
                if (result)
                    CurrentUser = user;
                return Json(new
                    {
                        result = result ? 1 : 0,
                        content = result ? "保存成功" : "保存失败"
                    }, JsonRequestBehavior.DenyGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "保存失败"
                }, JsonRequestBehavior.DenyGet);
            }
        }

        #region 批量导入

        public JsonResult ImportUsers(string fileName)
        {
            string errorStr = "";
            try
            {
                string userTempID = ""; //记行号
                string userReId = ""; //记工号
                string userNumberId = "";//记不为数字的行号
                bool type = true;
                List<Sys_User> listUser = GetExcelUserContent(HttpContext.Server.MapPath(ConfigurationManager.AppSettings["UFUserFile"]) + "\\" + fileName, ref userTempID, ref userReId, ref errorStr, ref type, ref userNumberId);
                if (type)
                {
                    if (userTempID != "")
                        errorStr = errorStr + "<br /> Excel中第 " + userTempID + " 条数据，必填项为空，没有添加成功！";
                    if (userReId != "")
                        errorStr = errorStr + "<br /> Excel中工号为：" + userReId + " 的数据工号出现重复，同步第一个！";
                    if (userNumberId != "")
                        errorStr = errorStr + "<br /> Excel中工号为：" + userNumberId + " 不正确，工号只能由6-20位字母、数字和下划线组成，跳过这些数据的添加！";
                    userTempID = "";
                    var chongfu = "";
                    InsertUserList(listUser, ref userTempID, ref chongfu);
                    if (chongfu != "")
                        errorStr = errorStr + "<br /> Excel中工号为：" + chongfu + " 的数据已存在于系统中，不用再次添加！";
                    if (userTempID != "")
                        errorStr = errorStr + "<br /> Excel中工号为：" + userTempID + " 的数据存在异常，没有添加成功！";
                    return Json(new
                    {
                        result = 1,
                        content = errorStr
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new
                    {
                        result = 0,
                        content = errorStr
                    }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = errorStr
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///     获取用户excel表中的数据
        /// </summary>
        /// <param name="excelPath">Excel路径</param>
        /// <param name="errorStr">错误信息</param>
        /// <returns>返回User数据列表</returns>
        private List<Sys_User> GetExcelUserContent(string excelPath, ref string userTempID, ref string userReId, ref string errorStr, ref bool type, ref string userNumberId)
        {
            var listUser = new List<Sys_User>();
            var err = "";
            try
            {
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelPath + ";Extended Properties=Excel 8.0;";
                var tmpcn = new OleDbConnection(strConn);
                tmpcn.Open(); //打开文件，读取里面的内容
                int flag = 0;
                try
                {
                    //打开文件，读取里面的工作表
                    DataTable dt = tmpcn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    foreach (DataRow drr in dt.Rows)
                    {
                        string strExcelTableName = drr["TABLE_NAME"].ToString(); //EXCEL中的工作表名
                        if (strExcelTableName == "模板$")
                        {
                            flag = 1;
                            var tmpda = new OleDbDataAdapter("select * from [" + strExcelTableName + "]", tmpcn);
                            var ds = new DataSet();
                            tmpda.Fill(ds);

                            bool formworkflag = true;

                            #region 判断Excel是否正确

                            if (ds.Tables[0].Columns.Count == 13)
                            {
                                if (!ds.Tables[0].Columns[0].ToString().Contains("工号"))
                                {
                                    formworkflag = false;
                                }
                                if (!ds.Tables[0].Columns[1].ToString().Contains("中文名"))
                                {
                                    formworkflag = false;
                                }
                                if (!ds.Tables[0].Columns[2].ToString().Contains("性别"))
                                {
                                    formworkflag = false;
                                }
                                if (!ds.Tables[0].Columns[3].ToString().Contains("出生日期"))
                                {
                                    formworkflag = false;
                                }
                                if (!ds.Tables[0].Columns[4].ToString().Contains("电子邮件"))
                                {
                                    formworkflag = false;
                                }
                                if (!ds.Tables[0].Columns[5].ToString().Contains("电话"))
                                {
                                    formworkflag = false;
                                }
                                if (!ds.Tables[0].Columns[6].ToString().Contains("传真"))
                                {
                                    formworkflag = false;
                                }
                                if (!ds.Tables[0].Columns[7].ToString().Contains("职称"))
                                {
                                    formworkflag = false;
                                }
                                if (!ds.Tables[0].Columns[8].ToString().Contains("加入公司日期"))
                                {
                                    formworkflag = false;
                                }
                                if (!ds.Tables[0].Columns[9].ToString().Contains("部门代码"))
                                {
                                    formworkflag = false;
                                }
                                if (!ds.Tables[0].Columns[10].ToString().Contains("职务代码"))
                                {
                                    formworkflag = false;
                                }
                                if (!ds.Tables[0].Columns[11].ToString().Contains("英文名"))
                                {
                                    formworkflag = false;
                                }
                                if (!ds.Tables[0].Columns[12].ToString().Contains("状态"))
                                {
                                    formworkflag = false;
                                }
                            }
                            else
                            {
                                formworkflag = false;
                            }

                            #endregion

                            if (formworkflag)
                            {
                                //Excel正确
                                int i = 1;
                                var strUser = new List<string>();
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    if (!(string.IsNullOrWhiteSpace(dr[0].ToString()) || string.IsNullOrWhiteSpace(dr[1].ToString()) || string.IsNullOrWhiteSpace(dr[12].ToString())))
                                    {
                                        if (Regex.IsMatch(dr[0].ToString(), "^[a-zA-Z0-9_]{6,20}$"))
                                        {
                                            var result = VerifyUser(dr[0].ToString(), dr[4].ToString());
                                            if (result == 0)
                                            {
                                                if (!strUser.Contains(dr[0].ToString()))
                                                {
                                                    var model = new Sys_User();
                                                    model.JobNum = dr[0].ToString();
                                                    model.Username = model.JobNum;
                                                    model.Password = BaseEncode.GetMd5Str("123456");
                                                    model.Realname = dr[1].ToString();
                                                    model.Sex = 0;
                                                    if (dr[2].ToString() == "女")
                                                        model.Sex = 1;
                                                    model.Email = dr[4].ToString();
                                                    model.Telephone = dr[5].ToString();
                                                    model.JobTitle = dr[7].ToString();
                                                    if (!string.IsNullOrWhiteSpace(dr[8].ToString()))
                                                        model.JoinDate = dr[8].StringToDate(2);
                                                    model.DeptCode = dr[9].ToString();
                                                    model.PostCode = dr[10].ToString();
                                                    if (string.IsNullOrWhiteSpace(dr[11].ToString()))
                                                        model.Ename = model.Realname;
                                                    else
                                                        model.Ename = dr[11].ToString();
                                                    model.Status = 0;
                                                    if (dr[12].ToString().ToUpper() == "冻结")
                                                        model.Status = 1;
                                                    listUser.Add(model);
                                                    strUser.Add(model.Username);
                                                }
                                                else
                                                {
                                                    if (userReId != "")
                                                        userReId = userReId + "," + dr[0];
                                                    else
                                                        userReId = dr[0].ToString();
                                                }
                                            }
                                            else
                                            {
                                                if (err != "")
                                                    err = err + "," + i;
                                                else
                                                    err = i.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (userNumberId != "")
                                                userNumberId = userNumberId + "," + dr[0];
                                            else
                                                userNumberId = dr[0].ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (userTempID != "")
                                            userTempID = userTempID + "," + i;
                                        else
                                            userTempID = i.ToString();
                                    }
                                    i++;

                                }
                            }
                            else
                            {
                                errorStr = errorStr + "<br /> 用户Excel不正确，请联系管理员以便获得帮助！";
                                type = false;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorStr = errorStr + "<br />" + ex.Message;
                    type = false;
                }
                finally
                {
                    tmpcn.Close();
                }
                if (flag == 0)
                {
                    errorStr = errorStr + "<br />用户Excel不正确，请联系管理员以便获得帮助！";
                    type = false;
                }
            }
            catch (Exception e)
            {
                errorStr = errorStr + "<br />" + e.Message;
                type = false;
            }
            if (type && (!string.IsNullOrWhiteSpace(err)))
                errorStr = errorStr + "<br /> Excel中第 " + err + " 条数据，工号、邮箱存在异常，没有添加成功！";
            return listUser;
        }

        private void InsertUserList(List<Sys_User> listUser, ref string userTempID, ref string chongfu)
        {
            List<Sys_User> listUserSql = userBL.GetList(" Sys_User.IsDelete = 0 ");
            foreach (Sys_User item in listUser)
            {
                try
                {
                    Sys_User model = listUserSql.Find(p => p.JobNum == item.JobNum);
                    if (model != null)
                    {
                        #region 系统已存在用户，做修改操作    但是被无知的人说，这个不需要；那这个就被邪恶的我干掉了。。。不要怪我哦

                        //model.Realname = item.Realname;
                        //model.Sex = item.Sex;
                        //model.Birthday = item.Birthday;
                        //model.Email = item.Email;
                        //model.Telephone = item.Telephone;
                        //model.Fax = item.Fax;
                        //model.JobTitle = item.JobTitle;
                        //model.Ename = item.Ename;

                        //Sys_Department dep = AllDepartments.Find(p => p.DeptCode == item.DeptCode);
                        //if (dep != null)
                        //{
                        //    item.DeptCode = dep.DeptCode;
                        //    item.DeptName = dep.DeptName;
                        //    item.DeptId = dep.DepartmentId;
                        //}
                        //else
                        //{
                        //    item.DeptCode = "";
                        //    item.DeptName = "";
                        //    item.DeptId = -1;
                        //}

                        //Sys_Post post = AllPosts.Find(p => p.PostCode == item.PostCode);
                        //if (post != null)
                        //{
                        //    item.PostCode = post.PostCode;
                        //    item.PostName = post.PostName;
                        //    item.PostLevel = post.PostLevel;
                        //    item.PostId = post.PostId;
                        //}
                        //else
                        //{
                        //    item.PostLevel = null;
                        //    item.PostName = "";
                        //    item.PostCode = "";
                        //    item.PostId = -1;
                        //}

                        //model.Status = item.Status;
                        //model.FreezeTime = null;

                        //userBL.Update(model);

                        #endregion

                        if (chongfu != "")
                            chongfu = chongfu + "," + item.Username;
                        else
                            chongfu = item.Username;
                    }
                    else
                    {
                        if (item.Status < 2)
                        {
                            Sys_Department dep = AllDepartments.Find(p => p.DeptCode == item.DeptCode);
                            if (dep != null)
                            {
                                item.DeptCode = dep.DeptCode;
                                item.DeptName = dep.DeptName;
                                item.DeptId = dep.DepartmentId;
                            }
                            else
                            {
                                item.DeptCode = "";
                                item.DeptName = "";
                                item.DeptId = -1;
                            }

                            Sys_Post post = AllPosts.Find(p => p.PostCode == item.PostCode);
                            if (post != null)
                            {
                                item.PostCode = post.PostCode;
                                item.PostName = post.PostName;
                                //item.PostLevel = post.PostLevel;
                                item.PostId = post.PostId;
                            }
                            else
                            {
                                item.PostLevel = null;
                                item.PostName = "";
                                item.PostCode = "";
                                item.PostId = -1;
                            }

                            userBL.Add(item);
                        }
                    }
                }
                catch
                {
                    if (userTempID != "")
                        userTempID = userTempID + "," + item.Username;
                    else
                        userTempID = item.Username;
                }
            }
        }

        /// <summary>
        /// 验证用户名与邮箱是否正确
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <returns>0：正常；1：工号异常；2：邮箱异常；3：两者都异常</returns>
        private int VerifyUser(string userName, string email)
        {
            var a = Regex.IsMatch(userName, "^\\w{6,20}$");
            var b = true;
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (email.Length <= 50)
                    b = Regex.IsMatch(email, "^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{2,4})+$");
                else
                    b = false;
            }
            if (a && b)
                return 0;
            else if (b)
                return 1;
            else if (a)
                return 2;
            else
                return 3;
        }

        #endregion

        /// <summary>
        /// 同步指纹库信息
        /// </summary>
        /// <returns></returns>
        public JsonResult SyncUserFinger()
        {
            return Json(syncUserInfo.SyncUserFinger() ? 1 : 0, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 同步原培训系统指纹库信息
        /// </summary>
        /// <returns></returns>
        public JsonResult SyncTrainUserFinger()
        {
            return Json(syncUserInfo.SyncTrainUserFinger() ? 1 : 0, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 同步指纹库信息
        /// </summary>
        /// <returns></returns>
        public JsonResult SyncUser()
        {
            try
            {
                syncUserInfo.Sync(1);

                RefreshDepartmentCache();
                RefreshPostCache();
                RefreshTrainGrade();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }



    }
    public class testScore
    {
        public int id { get; set; }

        public string realName { get; set; }

        public string DepName { get; set; }

        public string TrainGrade { get; set; }

        public decimal userscore { get; set; }

        public string CPAscore { get; set; }
    }
}