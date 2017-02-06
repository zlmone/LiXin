using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinBLL.DepCourseLearn;
using LiXinBLL.DepCourseManage;
using LiXinCommon;
using LiXinInterface.DepCourseLearn;
using LiXinInterface.DepCourseManage;
using LiXinInterface.DeptSurvey;

namespace LiXinControllers.DepMyAfterCourse
{
    public partial class DepMyAfterCourseController : BaseController
    {

        protected IDepCourseAdvice Dep_AttendceBl = new DepCourseAdviceBl();
        protected IDep_Course DepCourseBL;
        protected IDep_CourseOrder DepCourseOrderBl;
        protected IDeptSurveyExampaper ISurveyQuestionBL;

        public DepMyAfterCourseController(IDep_Course _DepCourseBL,IDep_CourseOrder _DepCourseOrder, IDeptSurveyExampaper _ISurveyQuestionBL)
        {
            DepCourseBL = _DepCourseBL;
            DepCourseOrderBl = _DepCourseOrder;
            ISurveyQuestionBL = _ISurveyQuestionBL;
        }

        #region 呈现页面

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult AfterCourse(int courseid, string backurl)
        {
            ViewBag.backurl = backurl;
            var course = DepCourseBL.GetCo_CourseAllList(courseid);
            ViewBag.course = course;
            ViewBag.TitleName = "我的评估详情";
            var CourseOrder = DepCourseOrderBl.Get(courseid);
            ViewBag.CourseOrder = CourseOrder;
            ViewBag.id = courseid;

            return View();
        }

        /// <summary>
        /// 评估详情
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public ActionResult AfterCourseDetail(int courseid, string backurl)
        {
            var course = DepCourseBL.GetCo_CourseAllList(courseid);
            ViewBag.course = course;

            var CourseOrder = DepCourseOrderBl.Get(courseid);
            ViewBag.CourseOrder = CourseOrder;
            ViewBag.id = courseid;
            ViewBag.backurl = backurl;
            ViewBag.TitleName = "评估详情";
            return View("AfterCourse");

        }

        public ActionResult AfterCourseList()
        {
            return View();
        }

        public ActionResult NoAfterCourse(int courseid)
        {
            var course = DepCourseBL.GetCo_CourseAllList(courseid);
            ViewBag.course = course;

            var CourseOrder = DepCourseOrderBl.Get(courseid);
            ViewBag.CourseOrder = CourseOrder;


            var qaaList = ISurveyQuestionBL.GetSurvey_QuestionAndAnswerByExampaperID(Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]));
            if (qaaList != null)
            {
                ViewBag.qaaList = qaaList;
            }
            ViewBag.id = courseid;

            return View();

        }

        public ActionResult NoAfterCourseList()
        {
            return View();
        }
        #endregion

        #region 课后评估总列表

        public JsonResult GetMyAfterCourseList(string CourseName, string StartTime, string EndTime, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and Dep_Course.CourseName like '%" + CourseName.ReplaceSql() + "%'";
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql += " and Dep_Course.StartTime>='" + StartTime + "'";
                ;
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " and Dep_Course.EndTime<='" + EndTime + "'";
            }
            int count = 0;
            var list = Dep_AttendceBl.GetMyAfterCourseList(out count, CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId, sql, "1=1", (pageIndex - 1) * pageSize + 1, pageSize);
            return Json(new
            {
                dataList = list,
                recordCount = count
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 已提交的课后评估

        public JsonResult GetPassMyAfterCourseList(string CourseName, string type, string StartTime, string EndTime, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and Dep_Course.CourseName like '%" + CourseName.ReplaceSql() + "%'";
            }
            if (!string.IsNullOrEmpty(type))
            {
                if (type != "0")
                {
                    sql += "and Dep_Course.Way=" + type;
                }
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql += " and Dep_Course.StartTime>='" + StartTime + "'";

            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " and Dep_Course.EndTime<='" + EndTime + "'";
            }

            int count = 0;
            //var list = IAttendce.GetMyAfterCourseList(out count, CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId,sql, (pageIndex - 1) * pageSize + 1, pageSize).FindAll(p => p.num!=0);
            var list = Dep_AttendceBl.GetMyAfterCourseList(out count, CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId, sql, "ALLnum>0", (pageIndex - 1) * pageSize + 1, pageSize);//.FindAll(p => p.ALLnum != 0);


            return Json(new
            {
                dataList = list,
                recordCount = count
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 没有提交的课程评估

        public JsonResult GetNoPassMyAfterCourseList(string CourseName, string type, string StartTime, string EndTime, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and Dep_Course.CourseName like '%" + CourseName.ReplaceSql() + "%'";
            }
            if (!string.IsNullOrEmpty(type))
            {
                if (type != "0")
                {
                    sql += "and Dep_Course.Way=" + type;
                }
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql += " and Dep_Course.StartTime>='" + StartTime + "'";

            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " and Dep_Course.EndTime<='" + EndTime + "'";
            }

            int count = 0;
            var list = Dep_AttendceBl.GetMyAfterCourseList(out count, CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId, sql, "ALLnum=0", (pageIndex - 1) * pageSize + 1, pageSize);//.FindAll(p => p.ALLnum==0);
            return Json(new
            {
                dataList = list,
                recordCount = count
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
