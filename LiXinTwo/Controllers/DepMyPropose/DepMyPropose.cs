using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinInterface.DepCourseLearn;
using LiXinInterface.DepCourseManage;

namespace LiXinControllers.DepMyPropose
{
    public partial class DepMyProposeController : BaseController
    {
        protected IDepCourseAdvice IDep_Attendce;
        protected IDep_Course DepCourseBL;
        protected IDep_CourseOrder DepCourseOrder;

        public DepMyProposeController(IDepCourseAdvice _IDep_Attendce, IDep_Course _DepCourseBL, IDep_CourseOrder _DepCourseOrder)
        {
            IDep_Attendce = _IDep_Attendce;
            DepCourseBL = _DepCourseBL;
            DepCourseOrder = _DepCourseOrder;
        }

        #region 页面呈现

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult Mypropose(int courseid, string backurl)
        {
            var course = DepCourseBL.GetCo_CourseAllList(courseid);
            ViewBag.course = course;
            ViewBag.backurl = backurl;
            var CourseOrder = DepCourseOrder.Get(courseid);
            ViewBag.CourseOrder = CourseOrder;
            return View();
        }

        public ActionResult MyproposeList()
        {
            return View();
        }

        public ActionResult NoPropose()
        {
            return View();
        }

        public ActionResult NoProposeList()
        {
            return View();
        }

        #endregion

        #region 所有建议
        public JsonResult GetMyproposeList(string CourseName, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and  Dep_Course.CourseName LIKE '%" + CourseName + "%'";
            }
            int count = 0;
            var list = IDep_Attendce.GetMyproposeList(out count, CurrentUser.UserId, sql, "1=1", (pageIndex - 1) * pageSize + 1, pageSize);

            //list =list.Skip(((pageIndex - 1)*pageSize)).Take(pageSize).ToList();

            return Json(new
            {
                dataList = list,
                recordCount = count

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 已提交的建议
        /// <summary>
        /// 已提交的建议
        /// </summary>
        /// <param name="CourseName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetMyproposePassList(string CourseName, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and  Dep_Course.CourseName LIKE '%" + CourseName + "%'";
            }
            //sql += " and AdviceCount=0";
            int count = 0;
            var list = IDep_Attendce.GetMyproposeList(out count, CurrentUser.UserId, sql, "AdviceCount>0", (pageIndex - 1) * pageSize + 1, pageSize).FindAll(p => p.AdviceCount != 0).ToList();



            return Json(new
            {
                dataList = list,
                recordCount = list.Count

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 未提交的建议
        /// <summary>
        /// 未提交的建议
        /// </summary>
        /// <param name="CourseName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetMyproposeNoPassList(string CourseName, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and  Dep_Course.CourseName LIKE '%" + CourseName + "%'";
            }

            //sql += " and AdviceCount>0";
            int count = 0;
            var list = IDep_Attendce.GetMyproposeList(out count, CurrentUser.UserId, sql, "AdviceCount=0", (pageIndex - 1) * pageSize + 1, pageSize).FindAll(p => p.AdviceCount == 0).ToList();


            return Json(new
            {
                dataList = list,
                recordCount = count

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
