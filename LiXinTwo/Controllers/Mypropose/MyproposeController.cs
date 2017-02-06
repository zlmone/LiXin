using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;

namespace LiXinControllers
{
    public partial class MyproposeController : BaseController
    {

        protected ICl_Attendce IAttendce;
        protected ICo_Course CoCourseBL;
        protected ICourseOrder CourseOrderBL;

        public MyproposeController(ICl_Attendce _IAttendce, ICo_Course _CoCourseBL, ICourseOrder _CourseOrderBL)
        {
            IAttendce = _IAttendce;
            CoCourseBL = _CoCourseBL;
            CourseOrderBL = _CourseOrderBL;
        }

        #region 页面呈现

        
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult Mypropose(int courseid,string backurl)
        {
            var course = CoCourseBL.GetCo_Course(courseid);
            ViewBag.course = course;
            ViewBag.backurl = backurl;
            var CourseOrder = CourseOrderBL.Get(courseid);
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

        public ActionResult AfterCourse()
        {
            return View();
        }

        public ActionResult AfterCourseList()
        {
            return View();
        }

        public ActionResult AfterCourseMain()
        {
            return View();
        }

        public ActionResult NoAfterCourseList()
        {
            return View();
        }
       
        #endregion

        #region 所有的建议
        /// <summary>
        /// 所有的建议
        /// </summary>
        /// <param name="CourseName"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetMyproposeList(string CourseName, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and  Co_Course.CourseName LIKE '%" + CourseName + "%'";
            }
            int count = 0;
            var list = IAttendce.GetMyproposeList(out count,CurrentUser.UserId, sql,"1=1", (pageIndex - 1) * pageSize + 1, pageSize);

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
                sql += " and  Co_Course.CourseName LIKE '%" + CourseName + "%'";
            }
            //sql += " and AdviceCount=0";
            int count = 0;
            var list = IAttendce.GetMyproposeList(out count, CurrentUser.UserId, sql, "AdviceCount>0", (pageIndex - 1) * pageSize + 1, pageSize).FindAll(p => p.AdviceCount != 0).ToList();

           

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
                sql += " and  Co_Course.CourseName LIKE '%" + CourseName + "%'";
            }

            //sql += " and AdviceCount>0";
            int count = 0;
            var list = IAttendce.GetMyproposeList(out count, CurrentUser.UserId, sql, "AdviceCount=0", (pageIndex - 1) * pageSize + 1, pageSize).FindAll(p => p.AdviceCount == 0).ToList();

           
            return Json(new
            {
                dataList = list,
                recordCount = count

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
