using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.User;
using LiXinLanguage;
using LiXinModels.User;
using LiXinControllers.Filter;
namespace LiXinControllers
{
    public class TeacherManageController : BaseController
    {
        protected IUser userBL;
        public TeacherManageController(IUser _userBL)
        {
            userBL = _userBL;
        }
        #region 呈现
        public ActionResult TeacherIndex()
        {
            return View();
        }

        public ActionResult AddTeacher(int userID = 0)
        {
            ViewBag.userID = userID;
            var user = new Sys_User();
            if (userID > 0)
            {
                user = userBL.Get(userID);
            }
            return View(user);
        }
        public ActionResult TeacherDetails(int userID = 0)
        {
            ViewBag.userID = userID;
            var user = new Sys_User();
            if (userID > 0)
            {
                user = userBL.Get(userID);
            }
            return View(user);
        }
        #endregion

        #region 讲师管理
        /// <summary>
        /// 获取讲师列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllTeacher(string RealName = "", int isTeacher = 0, string PayGrade = "", int deptid = 0, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalCount = 0;
                // var list = userBL.GetAllTeacher(out totalCount, CodeHelper.ReplaceSql(RealName), isTeacher, CodeHelper.ReplaceSql(PayGrade), pageSize, (pageIndex - 1) * pageSize + 1);
                var list = userBL.GetAllTeacher(out totalCount);
                var teacherIDs = string.Join(",", list.Where(p => p.IsTeacher == 1).Select(p => p.UserId));
                if (isTeacher != 0)
                {
                    list = list.Where(p => p.IsTeacher == isTeacher).ToList();
                }
                if (!string.IsNullOrEmpty(PayGrade))
                {
                    list = list.Where(p => p.PayGrade == null ? 1 == 2 : p.PayGrade.ToLower().Contains(PayGrade.ToLower())).ToList();
                }

                if (deptid == 0)
                {
                    list = list.Where(p => p.DeptId == CurrentUser.DeptId).ToList();
                }


                list = list.Where(p => p.Realname.ToLower().Contains(RealName.ToLower())).ToList();
                totalCount = list.Count;
                list = list.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

                foreach (var item in list)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.PayGrade = item.PayGrade == "" ? "--" : item.PayGrade.HtmlXssEncode();
                }
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalCount,
                    teacherIDs = teacherIDs
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Sys_User>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 添加内部讲师
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Filter.SystemLog("编辑内部讲师", LogLevel.Info)]
        public JsonResult AddInnerTeacher(string userID)
        {
            try
            {
                userBL.InsertInnerTeacher(userID);
                return Json(new
                {
                    result = 1,
                    content = "添加成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "添加失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 添加外聘讲师
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Filter.SystemLog("编辑外聘讲师", LogLevel.Info)]
        public JsonResult AddOuterTeacher(Sys_User user)
        {
            try
            {

                if (user.UserId == 0)
                {
                    user.Realname = user.Realname.Trim();
                    userBL.InsertOuterTeacher(user);
                }
                else
                {
                    user.JobNum = "";
                    user.Password = "";
                    user.Username = "";
                    user.IsTeacher = 2;
                    userBL.Update(user);
                }
                return Json(new
                {
                    result = 1,
                    content = "添加成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "添加失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除讲师
        /// </summary>
        /// <param name="userID">讲师ID</param>
        /// <param name="isteacher">1：内部讲师 2：外部讲师</param>
        /// <returns></returns>
        [Filter.SystemLog("删除讲师", LogLevel.Info)]
        public JsonResult DeleteTeacher(int userID, int isteacher)
        {
            try
            {
                userBL.DeleteTeacher(userID, isteacher);
                return Json(new
                {
                    result = 1,
                    content = "添加成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "删除失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion



        #region 2013-09-22  新添部门选择讲师 可以选择本部门下的所有人员当作讲师
        public JsonResult GetTeacherInfoList(string jobNum, string realName, string email, string deptName, int sex, int status, string trainGrade, int deptid = 1, string usertype = "99", int roleId = 0, int deptId = 0, int flag = 0, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Sys_User.UserId desc", int IsNew = 0, string selectflag = "")
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
            //if (deptIds != "0")
            //    where += string.Format(" and Sys_User.DeptId in ({0})", deptIds);
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
            if (deptid == 1)
            {
                where += string.Format(" and deptid={0} ", CurrentUser.DeptId);
            }
            else
            {
                where += string.Format(" and deptid!={0} ", CurrentUser.DeptId);
            }
            //where += string.Format(" and Sys_User.CPA = '{0}' ", cpa == 1 ? "是" : "否");

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
            where += " and UserType in ('在职','聘用','特批','离职人员')";
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

        private void GetChildDeptIds(int parentId, List<int> list)
        {
            foreach (Sys_Department dept in AllDepartments.Where(p => p.ParentDeptId == parentId))
            {
                list.Add(dept.DepartmentId);
                GetChildDeptIds(dept.DepartmentId, list);
            }
        }
        #endregion
    }
}
