using LiXinInterface.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace LiXinControllers
{
    public partial class NewMyproposeController : BaseController
    {
        protected INew_CourseOrder inew_courseorderbl;
        protected INew_Course newcourseBL;
        protected INew_Attendce IAttendce;
        protected INew_CourseRoomRule CourseRoomRule;
        
        public NewMyproposeController(INew_CourseOrder _inew_courseorderbl, INew_Course _newcourseBL, INew_Attendce _IAttendce,INew_CourseRoomRule courseRoomRule)
        {
            inew_courseorderbl = _inew_courseorderbl;
            newcourseBL = _newcourseBL;
            IAttendce = _IAttendce;
            CourseRoomRule = courseRoomRule;
        }

        public ActionResult MyproposeMain(string p)
        {
            ViewBag.page = 1;
            ViewBag.coursename = "请输入课程名称";          
            if (p != null && p != "" && p == "1")
            {
                if (Session["MyproposeMainSession"] != null)
                {
                    string sess = Session["MyproposeMainSession"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.coursename = att[1] == "" ? "请输入课程名称" : att[1];                
                  
                }
            }
            return View();
        }

        public ActionResult MyproposeList(string p)
        {
            ViewBag.page = 1;
            ViewBag.coursename = "请输入课程名称";
            if (p != null && p != "" && p == "1")
            {
                if (Session["MyproposeListSession"] != null)
                {
                    string sess = Session["MyproposeListSession"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.coursename = att[1] == "" ? "请输入课程名称" : att[1];

                }
            }
            return View();
        }
              
        public ActionResult NoProposeList(string p)
        {
            ViewBag.page = 1;
            ViewBag.coursename = "请输入课程名称";
            if (p != null && p != "" && p == "1")
            {
                if (Session["NoProposeListSession"] != null)
                {
                    string sess = Session["NoProposeListSession"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.coursename = att[1] == "" ? "请输入课程名称" : att[1];

                }
            }
            return View();
        }

        public ActionResult Mypropose(int courseid ,string backurl="")
        {
          
            ViewBag.backurl = backurl+"?p=1";
            ViewBag.course = newcourseBL.GetCourseByCourseRoomRule(courseid);
            return View();
        }

        /// <summary>
        /// 课程评估详情
        /// </summary>
        /// <param name="courseid">分组带教ID或集中授课ID</param>
        /// <param name="backurl">返回页面</param>
        /// <returns></returns>
        public ActionResult AfterCourse(int courseid, string backurl = "")
        {
            ViewBag.backurl = backurl+"?p=1";
            ViewBag.course = CourseRoomRule.GetNewCourseRoomRule(courseid);
            return View();
        }

        public ActionResult AfterCourseList(string p)
        {
            ViewBag.page = 1;
            ViewBag.coursename = "请输入课程名称";
            if (p != null && p != "" && p == "1")
            {
                if (Session["AfterCourseListSession"] != null)
                {
                    string sess = Session["AfterCourseListSession"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.coursename = att[1] == "" ? "请输入课程名称" : att[1];
                    ViewBag.starttime = att[2];
                    ViewBag.endtime = att[3];
                }
            }
            return View();
        }

        public ActionResult AfterCourseMain(string p)
        {
            ViewBag.page = 1;
            ViewBag.coursename = "请输入课程名称";
            if (p != null && p != "" && p == "1")
            {
                if (Session["AfterCourseMainSession"] != null)
                {
                    string sess = Session["AfterCourseMainSession"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.coursename = att[1] == "" ? "请输入课程名称" : att[1];
                    ViewBag.starttime = att[2];
                    ViewBag.endtime = att[3];
                }
            }
            return View();
        }

        public ActionResult NoAfterCourseList(string p)
        {
            ViewBag.page = 1;
            ViewBag.coursename = "请输入课程名称";
            if (p != null && p != "" && p == "1")
            {
                if (Session["NoAfterCourseListSession"] != null)
                {
                    string sess = Session["NoAfterCourseListSession"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.coursename = att[1] == "" ? "请输入课程名称" : att[1];
                    ViewBag.starttime = att[2];
                    ViewBag.endtime = att[3];
                }
            }
            return View();
        }

        #region 课前建议
        public JsonResult GetMyproposeList(string CourseName, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " order by dd.StartTime ")
        {
            if (Session["MyproposeMainSession"] != null)
            {
                Session.Remove("MyproposeMainSession");
            }
            Session["MyproposeMainSession"] = pageIndex + "㉿" + CourseName;

            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and  New_Course.CourseName LIKE '%" + CourseName + "%'";
            }
            int count = 0;

            var list = inew_courseorderbl.GetGetMyproposeList(out count, CurrentUser.UserId, sql, " 1=1", "order by " + jsRenderSortField, (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                dataList = list,
                recordCount = count

            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPassMyproposeList(string CourseName, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " order by dd.StartTime ")
        {


            if (Session["MyproposeListSession"] != null)
            {
                Session.Remove("MyproposeListSession");
            }
            Session["MyproposeListSession"] = pageIndex + "㉿" + CourseName;

            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and  New_Course.CourseName LIKE '%" + CourseName + "%'";
            }
            int count = 0;

            var list = inew_courseorderbl.GetGetMyproposeList(out count, CurrentUser.UserId, sql, "AdviceCount>0", "order by " + jsRenderSortField, (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                dataList = list,
                recordCount = count

            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetNOPassMyproposeList(string CourseName, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " order by dd.StartTime ")
        {
            if (Session["NoProposeListSession"] != null)
            {
                Session.Remove("NoProposeListSession");
            }
            Session["NoProposeListSession"] = pageIndex + "㉿" + CourseName;

            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and  New_Course.CourseName LIKE '%" + CourseName + "%'";
            }
            int count = 0;

            var list = inew_courseorderbl.GetGetMyproposeList(out count, CurrentUser.UserId, sql, "AdviceCount=0", "order by " + jsRenderSortField, (pageIndex - 1) * pageSize + 1, pageSize);
 
            return Json(new
            {
                dataList = list,
                recordCount = count

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 课后评估
        public JsonResult GetAfterCourseMain(string CourseName, string starttime, string endtime, int pageSize = 10, int pageIndex = 1, string jsRenderSortField="")
        {
            if (Session["AfterCourseMainSession"] != null)
            {
                Session.Remove("AfterCourseMainSession");
            }
            Session["AfterCourseMainSession"] = pageIndex + "㉿" + CourseName + "㉿" + starttime + "㉿" + endtime;

            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and  aa.CourseName LIKE '%" + CourseName + "%'";
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                sql += " and  aa.starttime >= '" + starttime + "'";
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sql += " and  aa.endtime <= '" + endtime + "'";
            }

            int count = 0;

            var list = inew_courseorderbl.GetAfterCourseList(out count, CurrentUser.UserId, sql, " 1=1","order by "+ jsRenderSortField,(pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                dataList = list,
                recordCount = count

            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPassAfterCourseMain(string CourseName, string starttime, string endtime, int pageSize = 10, int pageIndex = 1, string jsRenderSortField="")
        {
            if (Session["AfterCourseListSession"] != null)
            {
                Session.Remove("AfterCourseListSession");
            }
            Session["AfterCourseListSession"] = pageIndex + "㉿" + CourseName + "㉿" + starttime + "㉿" + endtime;

            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and  aa.CourseName LIKE '%" + CourseName + "%'";
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                sql += " and  aa.starttime >= '" + starttime + "'";
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sql += " and  aa.endtime <= '" + endtime + "'";
            }

            int count = 0;

            var list = inew_courseorderbl.GetAfterCourseList(out count, CurrentUser.UserId, sql, " cc>0", "order by "+ jsRenderSortField,(pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                dataList = list,
                recordCount = count

            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNoPassAfterCourseMain(string CourseName, string starttime, string endtime, int pageSize = 10, int pageIndex = 1, string jsRenderSortField="")
        {
            if (Session["NoAfterCourseListSession"] != null)
            {
                Session.Remove("NoAfterCourseListSession");
            }
            Session["NoAfterCourseListSession"] = pageIndex + "㉿" + CourseName + "㉿" + starttime + "㉿" + endtime;

            string sql = " 1=1";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and  aa.CourseName LIKE '%" + CourseName + "%'";
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                sql += " and  aa.starttime >= '" + starttime + "'";
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sql += " and  aa.endtime <= '" + endtime + "'";
            }

            int count = 0;

            var list = inew_courseorderbl.GetAfterCourseList(out count, CurrentUser.UserId, sql, " cc=0", "order by "+jsRenderSortField,(pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                dataList = list,
                recordCount = count

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
