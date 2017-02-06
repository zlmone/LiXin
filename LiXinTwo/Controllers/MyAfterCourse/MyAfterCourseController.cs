using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinInterface.Survey;

namespace LiXinControllers.MyAfterCourse
{
    public class MyAfterCourseController : BaseController
    {

        protected ICl_Attendce IAttendce;
        protected ICo_Course CoCourseBL;
        protected ICourseOrder CourseOrderBL;
        protected ISurveyQuestion ISurveyQuestionBL;

        public MyAfterCourseController(ICl_Attendce _IAttendce, ICo_Course _CoCourseBL, ICourseOrder _CourseOrderBL, ISurveyQuestion _ISurveyQuestionBL)
        {
            IAttendce = _IAttendce;
            CoCourseBL = _CoCourseBL;
            CourseOrderBL = _CourseOrderBL;
            ISurveyQuestionBL = _ISurveyQuestionBL;
        }

        #region 呈现页面
        
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult AfterCourse(int courseid,string backurl)
        {
            ViewBag.backurl = backurl;
            var course = CoCourseBL.GetCo_Course(courseid);
            ViewBag.course = course;
            ViewBag.TitleName = "我的评估详情";
            var CourseOrder = CourseOrderBL.Get(courseid);
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
            var course = CoCourseBL.GetCo_Course(courseid);
            ViewBag.course = course;

            var CourseOrder = CourseOrderBL.Get(courseid);
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
            var course = CoCourseBL.GetCo_Course(courseid);
            ViewBag.course = course;

            var CourseOrder = CourseOrderBL.Get(courseid);
            ViewBag.CourseOrder = CourseOrder;


            var qaaList = ISurveyQuestionBL.GetSurvey_QuestionAndAnswerByExampaperID(Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]));
            if (qaaList!=null)
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
        
        public JsonResult GetMyAfterCourseList(string CourseName,string type,string StartTime,string EndTime, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and Co_Course.CourseName like '%" + CourseName.ReplaceSql() + "%'";
            }
            if (!string.IsNullOrEmpty(type))
            {
                if (type != "0")
                {
                    sql += "and Co_Course.Way=" + type;
                }
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql += " and Co_Course.StartTime>='" + StartTime + "'";
;           }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " and Co_Course.EndTime<='" + EndTime + "'";
            }
            int count = 0;
            var list = IAttendce.GetMyAfterCourseList(out count, CurrentUser.UserId,CurrentUser.TrainGrade,CurrentUser.DeptId, sql, "1=1",(pageIndex - 1) * pageSize + 1, pageSize);
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
                 sql += " and Co_Course.CourseName like '%" + CourseName.ReplaceSql() + "%'";
            }
            if (!string.IsNullOrEmpty(type))
            {
                if (type != "0")
                {
                   sql += "and Co_Course.Way=" + type;
                }
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                  sql += " and Co_Course.StartTime>='" + StartTime + "'";
                
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " and Co_Course.EndTime<='" + EndTime + "'";
            }

            int count = 0;
            //var list = IAttendce.GetMyAfterCourseList(out count, CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId,sql, (pageIndex - 1) * pageSize + 1, pageSize).FindAll(p => p.num!=0);
            var list = IAttendce.GetMyAfterCourseList(out count, CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId, sql,"ALLnum>0", (pageIndex - 1) * pageSize + 1, pageSize);//.FindAll(p => p.ALLnum != 0);


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
                sql += " and Co_Course.CourseName like '%" + CourseName.ReplaceSql() + "%'";
            }
            if (!string.IsNullOrEmpty(type))
            {
                if (type != "0")
                {
                    sql += "and Co_Course.Way=" + type;
                }
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql += " and Co_Course.StartTime>='" + StartTime + "'";
                
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " and Co_Course.EndTime<='" + EndTime + "'";
            }

            int count = 0;
            var list = IAttendce.GetMyAfterCourseList(out count,CurrentUser.UserId,CurrentUser.TrainGrade,CurrentUser.DeptId, sql,"ALLnum=0", (pageIndex - 1) * pageSize + 1, pageSize);//.FindAll(p => p.ALLnum==0);
            return Json(new
            {
                dataList = list,
                recordCount = count
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
