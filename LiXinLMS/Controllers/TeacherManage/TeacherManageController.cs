using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.User;
using LiXinLanguage;
using LiXinModels.User;
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
        public JsonResult GetAllTeacher(string RealName = "", int isTeacher = 0, string PayGrade = "", int pageSize = 10, int pageIndex = 1)
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
    }
}
