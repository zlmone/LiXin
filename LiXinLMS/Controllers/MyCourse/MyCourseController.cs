﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.AttendceManage;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinInterface.Examination;
using LiXinInterface.Survey;
using LiXinInterface.User;
using LiXinModels.CourseLearn;
using LiXinModels.Examination.DBModel;
using LiXinModels.Survey;
using LiXinModels.CourseManage;
using LiXinModels.User;
using Retech.LearningAPI.Core;
using System.Text.RegularExpressions;

namespace LiXinControllers
{
    public partial class MyCourseController : BaseController
    {
        protected ICo_Course CoCourseBL;
        protected ICourseOrder CourseOrderBL;
        protected ICl_CourseAdvice ClCourseAdviceBL;
        protected ICl_Attendce ClAttendceBL;
        protected ISurveyExampaper ISurveyExampaperBL;
        protected ISurveyQuestion ISurveyQuestionBL;
        protected ISurveyQuestionAnswer ISurveyQuestionAnswerBL;
        protected ISurveyReplyAnswer ISurveyReplyAnswerBL;
        protected ICo_CourseResource ICoCourseResourceBL;
        protected ICo_CoursePaper ICo_CoursePaperBL;

        protected ICl_LearnVideoInfor IClLearnVideoInforBL;
        protected ICl_CpaLearnStatus ICpaLearnStatusBL;
        protected IExamination IExaminationrBL;
        protected IExampaper IExampaperBL;
        protected IUser IUserBL;
        protected IAttendce IAttendceBL;

        public MyCourseController(ICo_Course _CoCourseBL, ICourseOrder _CourseOrderBL,
                                  ICl_CourseAdvice _ClCourseAdviceBL, ICl_Attendce _ClAttendceBL,
                                  ISurveyExampaper _ISurveyExampaperBL, ISurveyQuestion _ISurveyQuestionBL,
                                  ISurveyQuestionAnswer _ISurveyQuestionAnswerBL,
                                  ISurveyReplyAnswer _ISurveyReplyAnswerBL, ICo_CourseResource _ICoCourseResourceBL,
                                  ICo_CoursePaper _ICo_CoursePaperBL, IExamination _IExaminationrBL,
                                  IExampaper _IExampaperBL, ICl_CpaLearnStatus _ICpaLearnStatusBL,
                                  ICl_LearnVideoInfor _IClLearnVideoInforBL, IUser _IUserBL,
            IAttendce _IAttendceBL)
        {
            CoCourseBL = _CoCourseBL;
            CourseOrderBL = _CourseOrderBL;
            ClCourseAdviceBL = _ClCourseAdviceBL;
            ClAttendceBL = _ClAttendceBL;
            ISurveyExampaperBL = _ISurveyExampaperBL;
            ISurveyQuestionBL = _ISurveyQuestionBL;
            ISurveyQuestionAnswerBL = _ISurveyQuestionAnswerBL;
            ISurveyReplyAnswerBL = _ISurveyReplyAnswerBL;
            ICoCourseResourceBL = _ICoCourseResourceBL;
            ICo_CoursePaperBL = _ICo_CoursePaperBL;
            IExaminationrBL = _IExaminationrBL;
            IExampaperBL = _IExampaperBL;
            ICpaLearnStatusBL = _ICpaLearnStatusBL;
            IClLearnVideoInforBL = _IClLearnVideoInforBL;
            IUserBL = _IUserBL;
            IAttendceBL = _IAttendceBL;
        }

        public ActionResult CPACourseList(string p)
        {
            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.Attstate = 3;
            if (p != null && p != "" && p == "1")
            {
                if (Session["fcpage"] != null)
                {
                    string sess = Session["fcpage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Attname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.Attstate = att[2];
                    ViewBag.AttstartTime = att[3];
                    ViewBag.AttendTime = att[4];
                }
            }
            return View();
        }

        #region 视频课程呈现

        public ActionResult VideoCourseList(string p)
        {
            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.Attmust = 2;
            ViewBag.Attstate = 3;
           // var a = GetDepartments;
            if (p != null && p != "" && p == "1")
            {
                if (Session["vcpage"] != null)
                {
                    string sess = Session["vcpage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);

                    ViewBag.page = att[0];
                    ViewBag.Attname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.Attmust = att[2];
                    ViewBag.Attstate = att[3];
                }
            }
            return View();
        }

        public ActionResult VideoAfterCourse(int id, string backurl)
        {
            ViewBag.backurl = Url.Content(backurl);
            ViewBag.id = id;
            return View();
        }

        public ActionResult VideoCourse(int id)
        {
            var courselist = CoCourseBL.GetVideoCo_CourseById(id);
            ViewBag.courselist = courselist;
            ViewBag.courseid = id;
            return View();
        }

        /// <summary>
        /// 视频课程详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult VideoCourseMain(int id)
        {
            var courselist = CoCourseBL.GetVideoCo_CourseById(id);
            ViewBag.courselist = courselist;

            var courseResource = ICoCourseResourceBL.GetVedioList(id, CurrentUser.UserId);
            ViewBag.courseResource = courseResource;


            var CourseVideoResourse = ICoCourseResourceBL.GetList(id);
            ViewBag.CourseVideoResourse = CourseVideoResourse;
           

            ViewBag.id = id;
            return View();
        }

        public ActionResult VideoCourseOnLineTest(int id)
        {
            Co_CoursePaper CoCoursePaper = ICo_CoursePaperBL.GetCo_CourseMainPaper(id);
            var course = CoCourseBL.GetCo_Course(id);
            if (CoCoursePaper != null && course.IsTest == 1)
            {
                var exap = IExaminationrBL.GetExamination(CoCoursePaper.PaperId);
                ViewBag.exap = exap;
                // var examsendstudent = IExaminationrBL.GetExamSendStudent(exap.PaperID);
                var examsendstudent = IExaminationrBL.GetSingletbExamSendStudentByCourseIdAndUserId(id,
                                                                                                    CurrentUser.UserId, 2);
                var exampapger = IExampaperBL.GetExampaper(CoCoursePaper.PaperId);
                ViewBag.exampapger = exampapger;
                ViewBag.examsendstudent = examsendstudent;
            }
            ViewBag.Co_CoursePaper = CoCoursePaper;
            ViewBag.courseid = id;
            ViewBag.course = course;

            return View();
        }

        #endregion

        #region 集中课程呈现

        /// <summary>
        /// 课后评估
        /// </summary>
        /// <returns></returns>
        /// 
        /// 
        public ActionResult AfterCourse(int id, string backurl)
        {
            ViewBag.backurl = backurl;
            ViewBag.id = id;
            //ViewBag.SurveyPaperId = SurveyPaperId;
            return View();
        }
        /// <summary>
        /// 课后评估单独页面 其他地方也需要用到!!!!!!!!!!
        /// </summary>
        /// <param name="SurveyPaperId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AfterCourseList(int SurveyPaperId, int id)
        {
            var qaaList = ISurveyQuestionBL.GetSurvey_QuestionAndAnswerByExampaperID(SurveyPaperId);
            if (qaaList != null)
            {
                ViewBag.qaaList = qaaList;
            }
            ViewBag.id = id;
            ViewBag.SurveyPaperId = SurveyPaperId;
            return View();
        }

        /// <summary>
        /// 新的课后页面方法 只有星级题目 原AfterCourseList暂时不用
        /// </summary>
        /// <param name="SurveyPaperId"></param>
        /// <returns></returns>
        public ActionResult AfterCourseNewList(int id, string backurl)
        {
            ViewBag.backurl = backurl;

            var courselist = CoCourseBL.GetVideoCo_CourseById(id);

            if (!string.IsNullOrEmpty(courselist.SurveyPaperId) && courselist.IsPing == 1)
            {
                var arr = courselist.SurveyPaperId.Split(';');
                if (arr[0] != "")
                {
                    var examPaper = ISurveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[0].Split(',')[1]));
                    if (examPaper != null)
                    {
                        ViewBag.examPaper = examPaper;
                    }
                    else
                    {
                        ViewBag.examPaper = null;
                    }
                }
                if (arr[1] != "")
                {
                    var TeacherexamPaper = ISurveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[1].Split(',')[1]));
                    if (TeacherexamPaper != null)
                    {
                        ViewBag.TeacherexamPaper = TeacherexamPaper;
                    }
                    else
                    {
                        ViewBag.TeacherexamPaper = null;
                    }
                }
                ViewBag.id = id;
                ViewBag.SurveyPaperId = courselist.SurveyPaperId;
            }
            return View();
        }


        /// <summary>
        /// 考勤详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Attendance(int id)
        {
            var Cl_Attendce = ClAttendceBL.GetCl_Attendce(id, CurrentUser.UserId);
            var cocurse = CoCourseBL.GetCo_Course(id);
            if (Cl_Attendce != null)
            {
                ViewBag.Cl_Attendce = Cl_Attendce;
                ViewBag.cocurse = cocurse;
            }
            else {
                ViewBag.Cl_Attendce = null;
            }
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 课前建议
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassPro(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 课程详情
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseMain(int id)
        {
            var Course = CoCourseBL.GetCo_Course(id);
            //var Course = CourseOrderBL.GetCourseById(id, CurrentUser.UserId);
            ViewBag.Course = Course;

            var CourseResourse = ICoCourseResourceBL.GetList(id);
            ViewBag.CourseResourse = CourseResourse;

            return View();
        }

        /// <summary>
        /// 详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCourse(int id)
        {
            var Course = CourseOrderBL.GetCourseById(id, CurrentUser.UserId);
            //var Course = CoCourseBL.GetCo_Course(id, CurrentUser.UserId);
            ViewBag.Course = Course;
            return View();
        }

        /// <summary>
        /// 课程列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCourseList(string p)
        {
            ViewBag.CPA = CurrentUser.CPA;

            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.Atttea = "请输入搜索内容";
            ViewBag.Attmust = 2;
            ViewBag.Attstate = 0;
            ViewBag.Attsort = 0;
            ViewBag.AttType = 99;
            if (p != null && p != "" && p == "1")
            {
                if (Session["clpage"] != null)
                {
                    string sess = Session["clpage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Atttea = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.Attname = att[2] == "" ? "请输入搜索内容" : att[2];
                    ViewBag.Attmust = att[3];
                    ViewBag.Attstate = att[4];
                    ViewBag.Attsort = att[5];
                    ViewBag.AttType = att[6];
                    ViewBag.AttstartTime = att[7];
                    ViewBag.AttendTime = att[8];
                }
            }
            return View();
        }

        /// <summary>
        /// 在线测试
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineTest(int id)
        {

            Co_CoursePaper CoCoursePaper = ICo_CoursePaperBL.GetCo_CourseMainPaper(id);
            var course = CoCourseBL.GetCo_Course(id);

            if (CoCoursePaper != null && course.IsTest == 1)
            {
                var exap = IExaminationrBL.GetExamination(CoCoursePaper.PaperId);
                ViewBag.exap = exap;
                // var examsendstudent = IExaminationrBL.GetExamSendStudent(exap.PaperID);
                var examsendstudent = IExaminationrBL.GetSingletbExamSendStudentByCourseIdAndUserId(id,
                                                                                                    CurrentUser.UserId, 1);
                var exampapger = IExampaperBL.GetExampaper(CoCoursePaper.PaperId);
                ViewBag.exampapger = exampapger;
                ViewBag.examsendstudent = examsendstudent;
            }
            ViewBag.Co_CoursePaper = CoCoursePaper;
            ViewBag.courseid = id;
            ViewBag.course = course;

            int leavestatus = GetUserForCourseStatus(id) ? 1 : 0;
            ViewBag.leavestatus = leavestatus;

            return View();
        }

        #endregion

        #region 集中课程列表
        public JsonResult CourseList(string teachername, string CourseName, string IsMust, string LearnStatus, string Sort, string subscribeType, string StartTime,string EndTime,int pageSize = 10, int pageIndex = 1)
        {
            if (Session["clpage"] != null)
            {
                Session.Remove("clpage");
            }
            Session["clpage"] = pageIndex + "㉿" + teachername + "㉿" + CourseName + "㉿" + IsMust + "㉿" + LearnStatus + "㉿" + Sort + "㉿" + subscribeType + "㉿" + StartTime + "㉿" + EndTime;
            string sql = " 1=1";

            if (!string.IsNullOrEmpty(teachername))
            {
                sql += " and Sys_User.Realname like '%" + teachername.ReplaceSql() + "%'";
            }
            if (!string.IsNullOrEmpty(CourseName))
            {
                if (CourseName != "请输入你要找的课程名称")
                {
                    sql += " and Co_Course.CourseName like '%" + CourseName.ReplaceSql() + "%'";
                }
            }
            if (!string.IsNullOrEmpty(IsMust))
            {
                if (IsMust != "2")
                {
                    sql += " and Co_Course.IsMust=" + IsMust;
                }
            }
            if (!string.IsNullOrEmpty(LearnStatus))
            {
                /*<option value="0">未开始</option>
                 <option value="1">进行中</option>
                 <option value="2">已完成</option>*/
                if (LearnStatus != "3")
                {
                    if (LearnStatus == "0")
                    {
                        sql += " and (getdate() < StartTime)";
                    }
                    else if (LearnStatus == "1")
                    {
                        sql += " and (getdate()>=StartTime and getdate()<=EndTime)";
                    }
                    else
                    {
                        sql += " and (getdate() >=EndTime)";
                    }
                }
            }

            if (!string.IsNullOrEmpty(Sort) && Sort != "0")
            {
                //sql += " and Co_Course.Sort='"+Sort+"'";
                sql += " and charindex('" + Sort + "',Co_Course.Sort)>0";
            }
            switch (subscribeType)
            {
                case "0"://退订成功
                    sql += " and Cl_CourseOrder.orderstatus = 0 ";
                    break;
                case "1"://预订成功
                    sql += " and cl_courseorder.IsAppoint = 0 and Cl_CourseOrder.orderstatus in(1,2) and (Cl_CourseOrder.IsLeave = 0 or (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1) or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime]))";
                    break;
                case "2"://请假成功
                    sql += " and Cl_CourseOrder.isleave = 1 and  Cl_CourseOrder.ApprovalFlag = 1 and Cl_CourseOrder.[ApprovalDateTime] <= Cl_CourseOrder.[ApprovalLimitTime]";
                    break;
                case "3"://待预订
                    sql += string.Format(" and cl_courseorder.userid is null and Co_Course.StartTime >= '{0}'", DateTime.Now);
                    break;
                case "4"://部门指定 
                    sql += " and cl_courseorder.IsAppoint = 1 and Cl_CourseOrder.orderstatus = 1 and (Cl_CourseOrder.IsLeave = 0 or (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1) or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime]))";
                    break;
                case "5"://总所指定 
                    sql += " and cl_courseorder.IsAppoint = 2 and Cl_CourseOrder.orderstatus = 1 and (Cl_CourseOrder.IsLeave = 0 or (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1) or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime]))";
                    break;
                case "6"://补预订 
                    sql += " and cl_courseorder.orderstatus = 3 ";
                    break;
                case "7"://已过期 
                    sql += string.Format(" and cl_courseorder.userid is null and Co_Course.StartTime < '{0}'", DateTime.Now);
                    break;
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                sql += " and Co_Course.StartTime>='" + StartTime + "'";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " and Co_Course.StartTime<='" + EndTime + "'";
            }
            //第一次进来过滤掉已结束的，在搜索时候在所有状态下搜索
            //if (first == 0)
            //{
            //    sql += " and getdate()<=StartTime";
            //}
            var litme = 0;
            var courseorder = CourseOrderBL.GetMyCourseListCourse(out litme, CurrentUser.UserId, sql,
                                                                  (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
                {
                    result = 1,
                    dataList = courseorder,
                    recordCount = litme
                }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  课前建议
        public JsonResult OnLoadCl_CourseAdvice(int id)
        {
            int leavestatus = GetUserForCourseStatus(id) ? 1 : 0;
            string html = "";

            string otherhtml = "";
            if (leavestatus == 1)
            {
                List<Cl_CourseAdvice> clCourseAdvicesList = ClCourseAdviceBL.GetList("  Cl_CourseAdvice.CourseId=" + id);

                foreach (var clCourseAdvice in clCourseAdvicesList)
                {
                    if (clCourseAdvice.UserId == CurrentUser.UserId)
                    {
                        html += "<div id=" + clCourseAdvice.Id + " class='list'>";
                        html += "   <div class='list-head'><span>" + clCourseAdvice.userRealname
                                + "</span><img src='" + Url.Content(clCourseAdvice.userPhotoUrlStr) + "' /></div>";
                        html += "   <div class='list-info'>";
                        html += "       <i></i>";
                        html += "       <div class='list-type'><strong>课前建议</strong>" + clCourseAdvice.AdviceTime +
                                "</div>";
                        html += "       <div class='list-con'>" + clCourseAdvice.AdviceContent + "</div>";
                        if (clCourseAdvice.AnserContent != "")
                        {
                            html += "       <div class='list-te'>";
                            html += "          <div class='te-con'><strong>讲师<span class='c_col'>" +
                                    clCourseAdvice.TeacherName + "</span>的反馈内容：</strong>" + clCourseAdvice.AnserContent +
                                    "</div>";
                            html += "          <div class='time'>时间：" + clCourseAdvice.AnserTime + "</div>";
                            html += "       </div>";
                        }
                        html += "   </div>";
                        html += "</div>";
                    }
                    else
                    {
                        if (clCourseAdvice.VisibleFlag == 2)
                        {
                            otherhtml += "<div id=" + clCourseAdvice.Id + " class='list'>";
                            otherhtml += "   <div class='list-head'><span>" + clCourseAdvice.userRealname
                                         + "</span><img src='" + Url.Content(clCourseAdvice.userPhotoUrlStr) +
                                         "' /></div>";
                            otherhtml += "   <div class='list-info'>";
                            otherhtml += "       <i></i>";
                            otherhtml += "       <div class='list-type'><strong>课前建议</strong>" +
                                         clCourseAdvice.AdviceTime + "</div>";
                            otherhtml += "       <div class='list-con'>" + clCourseAdvice.AdviceContent + "</div>";
                            if (clCourseAdvice.AnserContent != "")
                            {
                                otherhtml += "       <div class='list-te'>";
                                otherhtml += "          <div class='te-con'><strong>讲师<span class='c_col'>" +
                                             clCourseAdvice.TeacherName + "</span>的反馈内容：</strong>" +
                                             clCourseAdvice.AnserContent + "</div>";
                                otherhtml += "          <div class='time'>时间：" + clCourseAdvice.AnserTime + "</div>";
                                otherhtml += "       </div>";
                            }
                            otherhtml += "    </div>";
                            otherhtml += "</div>";
                        }
                    }
                }
            }
            return Json(new
                {
                    content = html,
                    other = otherhtml,
                    leavestatus = leavestatus

                }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加课前建议
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="AdviceContent"></param>
        /// <returns></returns>
        public JsonResult SubmitCl_CourseAdvice(int CourseId, string AdviceContent)
        {
            var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 29);


            var course = CoCourseBL.GetCo_Course(CourseId);

            DateTime datetime = DateTime.Now;

            //if (course.PreAdviceConfigTime != 0)
            //{
            DateTime starttime = course.StartTime.AddHours(-Convert.ToDouble(course.PreAdviceConfigTime));

            if (datetime > starttime)
            {
                return Json(
                    new { result = 1, content = "课前开放" + course.PreAdviceConfigTime + "小时，允许提建议" },
                    JsonRequestBehavior.AllowGet);
            }
            //}
            //else
            //{
            //    DateTime starttime = course.StartTime.AddHours(-Convert.ToDouble(Sys_ParamConfig.ConfigValue.Split(';')[0]));
            //    if (datetime > starttime)
            //    {
            //        return Json(
            //            new { result = 1, content = "课前开放" + Sys_ParamConfig.ConfigValue.Split(';')[0] + "小时，允许提建议" },
            //            JsonRequestBehavior.AllowGet);
            //    }
            //}




            Cl_CourseAdvice model = new Cl_CourseAdvice();
            model.CourseId = CourseId;
            model.AdviceContent = AdviceContent;
            model.AdviceTime = DateTime.Now;
            model.UserId = CurrentUser.UserId;
            model.VisibleFlag = 0;  //0：都不可见；1：仅提出人；2：全部
            model.IsDelete = 0;
            ClCourseAdviceBL.SubmitCl_CourseAdvice(model);
            //var user = IUserBL.Get(CurrentUser.UserId);//如果登录时没头像,然后上传头像后时时查询头像

            return Json(new
                {
                    result = 0,
                    username = CurrentUser.Realname,
                    photourl = Url.Content(CurrentUser.PhotoUrlStr),
                    adviceTime = model.AdviceTime.ToString(),
                    id = model.Id,
                    content = "提交成功"
                }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 回复课前建议
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CourseId"></param>
        /// <param name="AnserContent"></param>
        /// <returns></returns>
        public JsonResult ReplyCl_CourseAdvice(int id, int CourseId, string AnserContent, int VisibleFlag)
        {
            Cl_CourseAdvice model = new Cl_CourseAdvice();
            model.Id = id;
            model.CourseId = CourseId;
            model.AnserContent = AnserContent;
            model.AnserTime = DateTime.Now;
            model.VisibleFlag = VisibleFlag;
            model.AnserUserId = CurrentUser.UserId;

            if (ClCourseAdviceBL.ReplyCl_CourseAdvice(model))
            {
                string html = "";
                html += "<div class='list-te'>";
                html += "<div class='te-con'><strong  class='c_col'>讲师<span>" + CurrentUser.Realname + "</span>的反馈内容：</strong>" + model.AnserContent + "</div>";
                html += "<div class='time'>时间：" + model.AnserTime + "</div>";
                html += "</div>";

                return Json(new
                    {
                        result = 0,
                        innerhtml = html,
                        content = "回复成功"
                    }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                    {
                        result = 0,
                        content = "回复失败"
                    }, JsonRequestBehavior.AllowGet);

            }
        }

        #endregion

        #region 课后评估
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tt">前面获取的评估答案和对应的字段</param>
        /// <param name="courseId"></param>
        /// <param name="type">type:1是提交0是保存</param>
        /// <param name="num">num:1指只有一个评估,num:2指2个评估都有</param>
        /// <returns></returns>
        public JsonResult FInsertSurvey_ReplyAnswer(string tt, int courseId, int type, int ExampaperID,int num)
        {
            var course = CoCourseBL.GetCo_Course(courseId);
            //var xueshi = AllSystemConfigs.Find(p => p.ConfigType == 24);//读取参数配置表 各部分比例
            var xueshi = course.CourseLengthDistribute;
            //way=1是集中   2是视频
            if (course.Way == 1)
            {
                if (type == 0)
                {
                    //先删除后在保存
                    ISurveyReplyAnswerBL.DeleteByCourseIDAndUserId(courseId, CurrentUser.UserId, ExampaperID);
                    string[] arr = tt.Split('♥');
                    Survey_ReplyAnswer surveyReply = null;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        surveyReply = new Survey_ReplyAnswer();
                        surveyReply.SourceFrom = 0;
                        surveyReply.ObjectID = courseId;
                        surveyReply.ExampaperID = Convert.ToInt32(arr[i].Split('♣')[1]);
                        surveyReply.QuestionID = Convert.ToInt32(arr[i].Split('♣')[0]);
                        surveyReply.SubjectiveAnswer = arr[i].Split('♣')[2];
                        surveyReply.QuestionReply = arr[i].Split('♣')[3];
                        surveyReply.UserID = CurrentUser.UserId;
                        surveyReply.Status = Convert.ToInt32(type);
                        ISurveyReplyAnswerBL.InsertSurvey_ReplyAnswer(surveyReply);
                    }
                    return Json(new { result = 0, content = "保存成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (course.AttFlag !=0)
                    { 
                        //先判断是否有考勤
                        var ttt = ClAttendceBL.GetCl_Attendce(courseId, CurrentUser.UserId);
                        if (ttt != null)
                        {
                            if (course.AttFlag == 1 && (ttt.StartTime == Convert.ToDateTime("2050-1-1") || ttt.StartTime == DateTime.MinValue))
                            {
                                return Json(new { result = 1, content = "请先上课考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                            }

                            if (course.AttFlag == 2 && (ttt.EndTime == Convert.ToDateTime("2000-1-1") || ttt.EndTime == DateTime.MinValue))
                            {
                                return Json(new { result = 1, content = "请先下课考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                            }

                            if (course.AttFlag == 3 && (ttt == null || (ttt.StartTime == Convert.ToDateTime("2050-1-1") && ttt.EndTime == Convert.ToDateTime("2000-1-1"))))
                            {
                                return Json(new { result = 1, content = "请先考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { result = 1, content = "请先考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 29);
                    DateTime datetime = DateTime.Now;

                    //判断是否有评估 有就提示先评估 没有就直接进入考试
                    if (course.IsPing == 1)
                    {
                        DateTime endtime = course.EndTime.AddHours(Convert.ToDouble(course.AfterEvlutionConfigTime));
                        if (datetime > endtime &&datetime< course.EndTime)
                        {
                            return
                                Json(
                                    new
                                        {
                                            result = 1,
                                            //content = "课程结束后" + course.AfterEvlutionConfigTime + "小时,允许提交评估"
                                            //content = "评估时间已到期"
                                            content="课程结束后，允许提交评估"
                                        },
                                    JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (ISurveyReplyAnswerBL.GetStatus(courseId, CurrentUser.UserId, 0, ExampaperID))
                    {
                        ISurveyReplyAnswerBL.DeleteByCourseIDAndUserId(courseId, CurrentUser.UserId, ExampaperID);
                    }
                    if (num == 1)
                    {
                        //判断是否提交多次 有数据则跳出 
                        if (ISurveyReplyAnswerBL.GetList(" ObjectID=" + course.Id + " and UserID=" + CurrentUser.UserId + " and [ExampaperID]=" + ExampaperID + " and Status=1").Count > 0)
                        {
                            return Json(new { result = 1, content = "评估已经提交！" }, JsonRequestBehavior.AllowGet);
                        }

                        string[] arr = tt.Split('♥');
                        Survey_ReplyAnswer surveyReply = null;
                        for (int i = 0; i < arr.Length; i++)
                        {
                            surveyReply = new Survey_ReplyAnswer();
                            surveyReply.SourceFrom = 0;
                            surveyReply.ObjectID = courseId;
                            surveyReply.ExampaperID = Convert.ToInt32(arr[i].Split('♣')[1]);
                            surveyReply.QuestionID = Convert.ToInt32(arr[i].Split('♣')[0]);
                            surveyReply.SubjectiveAnswer = arr[i].Split('♣')[2];
                            surveyReply.QuestionReply = arr[i].Split('♣')[3];
                            surveyReply.UserID = CurrentUser.UserId;
                            surveyReply.Status = Convert.ToInt32(type);
                            ISurveyReplyAnswerBL.InsertSurvey_ReplyAnswer(surveyReply);
                        }
                    }
                    else
                    {
                        string[] arrlist = tt.Split('✚');//用✚隔开课程和讲师评估

                        for (int z = 0; z < arrlist.Length; z++)
                        {
                            if (arrlist[z] != "")
                            {
                                //判断是否提交多次 有数据则跳出 
                                if (ISurveyReplyAnswerBL.GetList(" ObjectID=" + course.Id + " and UserID=" + CurrentUser.UserId + " and [ExampaperID]="
                                    + arrlist[z].Split('♥')[z].Split('♣')[1] + " and Status=1").Count > 0)
                                {
                                    return Json(new { result = 1, content = "评估已经提交！" }, JsonRequestBehavior.AllowGet);
                                }

                                string[] arr = arrlist[z].Split('♥');
                                Survey_ReplyAnswer surveyReply = null;
                                for (int i = 0; i < arr.Length; i++)
                                {
                                    surveyReply = new Survey_ReplyAnswer();
                                    surveyReply.SourceFrom = 0;
                                    surveyReply.ObjectID = courseId;
                                    surveyReply.ExampaperID = Convert.ToInt32(arr[i].Split('♣')[1]);
                                    surveyReply.QuestionID = Convert.ToInt32(arr[i].Split('♣')[0]);
                                    surveyReply.SubjectiveAnswer = arr[i].Split('♣')[2];
                                    surveyReply.QuestionReply = arr[i].Split('♣')[3];
                                    surveyReply.UserID = CurrentUser.UserId;
                                    surveyReply.Status = Convert.ToInt32(type);
                                    ISurveyReplyAnswerBL.InsertSurvey_ReplyAnswer(surveyReply);
                                }
                            }
                        }


                    }
                    var courseorder = CourseOrderBL.GetCourseById(courseId, CurrentUser.UserId);

                    //判断他是否是第一次学习课程,如果是第一次则记录分数,第2次的话就不做任何操作
                    if (IAttendceBL.ExistAtts(courseId, CurrentUser.UserId))
                    {
                        //判断是否有考试，如果没有则记录学时.   
                        if (course.IsTest == 0 && course.IsPing == 1)
                        {                            

                            //如果只有考勤和评估没有考试 提交评估把 评估和考试所占比列都加上去
                            double afterlenght = (double)Convert.ToInt32(xueshi.Split(';')[1]) / 100 +
                                               (double)Convert.ToInt32(xueshi.Split(';')[2]) / 100;

                            decimal forlenght = course.CourseLength * Convert.ToDecimal(afterlenght) + courseorder.GetScore;

                            CourseOrderBL.UpdateGetScore(course.Id, CurrentUser.UserId, forlenght, 1, 2);

                            //在判断下是否准入aCPA 有则转
                            #region 转入cpa课程 
                            if (course.IsCPA == 1)
                            {
                                //ICpaLearnStatusBL

                                Cl_CpaLearnStatus cls = ICpaLearnStatusBL.GetCl_CpaLearnStatusByCourseId(course.Id, CurrentUser.UserId);
                                if (cls == null)
                                {
                                    cls = new Cl_CpaLearnStatus();
                                    cls.CourseID = course.Id;
                                    cls.UserID = CurrentUser.UserId;
                                    cls.IsAttFlag = 0;
                                    cls.IsPass = 1;
                                    //cls.IsAttFlag = 1;
                                    cls.Progress = 0;
                                    cls.LearnTimes = 0;
                                    if (course.IsMust == 1)
                                    {
                                        cls.GetLength = forlenght*Convert.ToDecimal(0.5);
                                    }
                                    if (course.IsMust == 0)
                                    {
                                        cls.GetLength = forlenght;
                                    }
                                    cls.CpaFlag = 2;
                                    cls.GradeStatus = 1;
                                    ICpaLearnStatusBL.SubscribeCPALearnStatus(cls);
                                }
                                else 
                                {
                                    cls.IsAttFlag = 0;
                                    cls.IsPass = 1;
                                    cls.Progress = 0;
                                    cls.LearnTimes = 0;
                                    //cls.GetLength = GetScore + CPAmodel.GetLength;
                                    if (course.IsMust == 1)
                                    {
                                        cls.GetLength = forlenght * Convert.ToDecimal(0.5);
                                    }
                                    if (course.IsMust == 0)
                                    {
                                        cls.GetLength = forlenght;
                                    }
                                    cls.CpaFlag = 2;
                                    cls.GradeStatus = 1;
                                    ICpaLearnStatusBL.UpdateCPALearnStatusByModel(cls);

                                }
                            #endregion

                            }
                        }
                        else //如果有考试则记录 评估所占的比例分数
                        {                            
                            
                            decimal forlenght = (course.CourseLength * Convert.ToDecimal(xueshi.Split(';')[2]) / 100) + courseorder.GetScore;

                            CourseOrderBL.UpdateGetScore(course.Id, CurrentUser.UserId, forlenght, 2, 1);//状态不变

                        }
                    }
                    return Json(new { result = 0, content = "提交成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            else //视频课程开始
            {
                //type是保存1是提交
                if (type == 0)
                {
                    ISurveyReplyAnswerBL.DeleteByCourseIDAndUserId(courseId, CurrentUser.UserId, ExampaperID);
                    string[] arr = tt.Split('♥');
                    Survey_ReplyAnswer surveyReply = null;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        surveyReply = new Survey_ReplyAnswer();
                        surveyReply.SourceFrom = 0;
                        surveyReply.ObjectID = courseId;
                        surveyReply.ExampaperID = Convert.ToInt32(arr[i].Split('♣')[1]);
                        surveyReply.QuestionID = Convert.ToInt32(arr[i].Split('♣')[0]);
                        surveyReply.SubjectiveAnswer = arr[i].Split('♣')[2];
                        surveyReply.QuestionReply = arr[i].Split('♣')[3];
                        surveyReply.UserID = CurrentUser.UserId;
                        surveyReply.Status = Convert.ToInt32(type);
                        ISurveyReplyAnswerBL.InsertSurvey_ReplyAnswer(surveyReply);
                    }
                    return Json(new { result = 0, content = "保存成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var vedio = ICpaLearnStatusBL.GetCl_CpaLearnStatusByCourseId(courseId, CurrentUser.UserId);

                    //查不到学习状态就是没学
                    if (vedio == null)
                    {
                        return Json(new { result = 1, content = "请先学习课程资源" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var courseResource = ICoCourseResourceBL.GetVedioList(course.Id, CurrentUser.UserId);

                        if (courseResource.Count != 0)
                        {
                            if (courseResource[0].ResourceType == 4)
                            {
                                foreach (var clLearnVideoInfor in courseResource)
                                {
                                    if (clLearnVideoInfor.Progress < Convert.ToDecimal(course.ReturnTimes))
                                    {
                                        return Json(new { result = 1, content = "视频还未达到一定进度" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }

                        DateTime now = DateTime.Now;

                        if (now > course.EndTime)
                        {
                            return Json(new { result = 1, content = "课程已结束不能进行评估" }, JsonRequestBehavior.AllowGet);
                        }

                        if (ISurveyReplyAnswerBL.GetStatus(courseId, CurrentUser.UserId, 0, ExampaperID))
                        {
                            ISurveyReplyAnswerBL.DeleteByCourseIDAndUserId(courseId, CurrentUser.UserId, ExampaperID);
                        }

                        if (num == 1)
                        {
                            //判断是否提交多次 有数据则跳出 
                            if (ISurveyReplyAnswerBL.GetList(" ObjectID=" + course.Id + " and UserID=" + CurrentUser.UserId + " and [ExampaperID]=" + ExampaperID + " and Status=1").Count > 0)
                            {
                                return Json(new { result = 1, content = "评估已经提交！" }, JsonRequestBehavior.AllowGet);
                            }

                            string[] arr = tt.Split('♥');
                            Survey_ReplyAnswer surveyReply = null;
                            for (int i = 0; i < arr.Length; i++)
                            {
                                surveyReply = new Survey_ReplyAnswer();
                                surveyReply.SourceFrom = 0;
                                surveyReply.ObjectID = courseId;
                                surveyReply.ExampaperID = Convert.ToInt32(arr[i].Split('♣')[1]);
                                surveyReply.QuestionID = Convert.ToInt32(arr[i].Split('♣')[0]);
                                surveyReply.SubjectiveAnswer = arr[i].Split('♣')[2];
                                surveyReply.QuestionReply = arr[i].Split('♣')[3];
                                surveyReply.UserID = CurrentUser.UserId;
                                surveyReply.Status = Convert.ToInt32(type);
                                ISurveyReplyAnswerBL.InsertSurvey_ReplyAnswer(surveyReply);
                            }
                        }
                        else
                        {
                            string[] arrlist = tt.Split('✚');//用✚隔开课程和讲师评估

                            for (int z = 0; z < arrlist.Length; z++)
                            {
                                if (arrlist[z] != "")
                                {
                                    //判断是否提交多次 有数据则跳出 
                                    if (ISurveyReplyAnswerBL.GetList(" ObjectID=" + course.Id + " and UserID=" + CurrentUser.UserId + " and [ExampaperID]="
                                        + arrlist[z].Split('♥')[z].Split('♣')[1] + " and Status=1").Count > 0)
                                    {
                                        return Json(new { result = 1, content = "评估已经提交！" }, JsonRequestBehavior.AllowGet);
                                    }

                                    string[] arr = arrlist[z].Split('♥');
                                    Survey_ReplyAnswer surveyReply = null;
                                    for (int i = 0; i < arr.Length; i++)
                                    {
                                        surveyReply = new Survey_ReplyAnswer();
                                        surveyReply.SourceFrom = 0;
                                        surveyReply.ObjectID = courseId;
                                        surveyReply.ExampaperID = Convert.ToInt32(arr[i].Split('♣')[1]);
                                        surveyReply.QuestionID = Convert.ToInt32(arr[i].Split('♣')[0]);
                                        surveyReply.SubjectiveAnswer = arr[i].Split('♣')[2];
                                        surveyReply.QuestionReply = arr[i].Split('♣')[3];
                                        surveyReply.UserID = CurrentUser.UserId;
                                        surveyReply.Status = Convert.ToInt32(type);
                                        ISurveyReplyAnswerBL.InsertSurvey_ReplyAnswer(surveyReply);
                                    }
                                }
                            }
                        }
      
                            //没有考试 有评估 
                            if (course.IsTest == 0 && course.IsPing == 1)
                            {
                                //修改学时 没有考试的情况下 评估后得    得分满
                                ICpaLearnStatusBL.UpdateGetCl_CpaLearnStatusByCourseId(course.Id, CurrentUser.UserId,
                                                                                       course.CourseLength, 1);

                                if (course.IsCPA == 1)
                                { 
                                    Cl_CpaLearnStatus cls = new Cl_CpaLearnStatus();
                                    cls.CourseID = course.Id;
                                    cls.UserID = CurrentUser.UserId;
                                    cls.IsAttFlag = 0;
                                    cls.Progress = 0;
                                    cls.LearnTimes = 0;
                                    if (course.IsMust == 1)
                                    {
                                        cls.GetLength = course.CourseLength*Convert.ToDecimal(0.5);
                                    }
                                    if (course.IsMust == 0)
                                    {
                                        cls.GetLength = course.CourseLength;
                                    }
                                    
                                    cls.CpaFlag = 2;
                                    cls.GradeStatus = 1;
                                    cls.IsPass = 1;

                                    ICpaLearnStatusBL.SubscribeCPALearnStatus(cls);
                                }
                               
                            }
                            return Json(new { result = 0, content = "提交成功" }, JsonRequestBehavior.AllowGet);                       
                    }
                }
            }
        }


        #region

        /// <summary>
        /// 视频课程的课后评估
        /// </summary>
        /// <param name="tt"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public JsonResult FInsertVedioSurvey_ReplyAnswer(string tt, int courseId, int type)
        {
            if (type == 0)
            {
                ISurveyReplyAnswerBL.DeleteByCourseIDAndUserId(courseId, CurrentUser.UserId, 0);
                string[] arr = tt.Split(';');
                Survey_ReplyAnswer surveyReply = null;
                for (int i = 0; i < arr.Length; i++)
                {
                    surveyReply = new Survey_ReplyAnswer();
                    surveyReply.SourceFrom = 0;
                    surveyReply.ObjectID = courseId;
                    surveyReply.ExampaperID = Convert.ToInt32(arr[i].Split('.')[0]);
                    surveyReply.QuestionID = Convert.ToInt32(arr[i].Split('.')[1]);
                    if (Convert.ToInt32(arr[i].Split('.')[2]) < 2)
                    {
                        surveyReply.ObjectiveAnswer = arr[i].Split('.')[3];
                    }
                    else
                    {
                        surveyReply.SubjectiveAnswer = arr[i].Split('.')[3].ReplaceSql();
                    }
                    surveyReply.UserID = CurrentUser.UserId;
                    surveyReply.Status = Convert.ToInt32(type);
                    ISurveyReplyAnswerBL.InsertSurvey_ReplyAnswer(surveyReply);
                }
                return Json(new { result = 0, content = "保存成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var vedio = ICpaLearnStatusBL.GetCl_CpaLearnStatusByCourseId(courseId, CurrentUser.UserId);
                //查不到学习状态就是没学
                if (vedio == null)
                {
                    return Json(new { result = 1, content = "请先学习课程资源." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var vediostatus = IClLearnVideoInforBL.GetCl_LearnVideoInforList(vedio.Id);

                    var course = CoCourseBL.GetCo_Course(courseId);

                    var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 25);//读取参数配置 观看课程时长达到视频总时长的

                    foreach (Cl_LearnVideoInfor clLearnVideoInfor in vediostatus)
                    {
                        if (clLearnVideoInfor.Progress < Convert.ToDecimal(Sys_ParamConfig.ConfigValue))
                        {
                            return Json(new { result = 1, content = "视频还未达到一定进度" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    //添加课后评估
                    string[] arr = tt.Split(';');
                    Survey_ReplyAnswer surveyReply = null;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        surveyReply = new Survey_ReplyAnswer();
                        surveyReply.SourceFrom = 0;
                        surveyReply.ObjectID = courseId;
                        surveyReply.ExampaperID = Convert.ToInt32(arr[i].Split('.')[0]);
                        surveyReply.QuestionID = Convert.ToInt32(arr[i].Split('.')[1]);
                        if (Convert.ToInt32(arr[i].Split('.')[2]) < 2)
                        {
                            surveyReply.ObjectiveAnswer = arr[i].Split('.')[3];
                        }
                        else
                        {
                            surveyReply.SubjectiveAnswer = arr[i].Split('.')[3];
                        }
                        surveyReply.UserID = CurrentUser.UserId;
                        surveyReply.Status = Convert.ToInt32(1);
                        ISurveyReplyAnswerBL.InsertSurvey_ReplyAnswer(surveyReply);
                    }

                    //没有考试 只有评估
                    if (course.IsTest == 0 && course.IsPing == 1)
                    {


                        //修改学时
                        ICpaLearnStatusBL.UpdateGetCl_CpaLearnStatusByCourseId(course.Id, CurrentUser.UserId,
                                                                               course.CourseLength, 1);
                    }
                    return Json(new { result = 0, content = "提交成功" }, JsonRequestBehavior.AllowGet);
                }

            }
        }
        #endregion

        public bool GetUserForCourseStatus(int CourseId)
        {
            var model = CourseOrderBL.GetCourseById(CourseId, CurrentUser.UserId);
            if (model.MyStatus == 6 || model.MyStatus == 2)
                return false;
            return true;
        }


        public JsonResult FBindSurvey_ReplyAnswer(int Courseid, string SurveyPaperId)
        {
            string html = "";
            string teacherhtml = "";
            int status = 0;
            int teacherstatus = 0;
            //为1时，可以操作；为0时，请假成功，不可操作
            int leavestatus = GetUserForCourseStatus(Courseid) ? 1 : 0;
            if (leavestatus == 1)
            {
                if (SurveyPaperId != "")
                {
                    if (SurveyPaperId.Split(';')[0] != "")
                    {
                        var ReplyAnswer =
                   ISurveyReplyAnswerBL.GetList(" ObjectID=" + Courseid + " and ExampaperID=" + SurveyPaperId.Split(';')[0].Split(',')[1] +
                                                " and UserID=" + CurrentUser.UserId);
                        if (ReplyAnswer.Count != 0)
                        {

                            foreach (Survey_ReplyAnswer surveyReplyAnswer in ReplyAnswer)
                            {
                                //html += surveyReplyAnswer.SubjectiveAnswer + surveyReplyAnswer.ObjectiveAnswer + ";";
                                html += surveyReplyAnswer.QuestionID + "♣" + surveyReplyAnswer.SubjectiveAnswer + "♣" +
                                        surveyReplyAnswer.QuestionReply + "♥";
                                status = surveyReplyAnswer.Status;
                            }

                            html = html.Substring(0, html.LastIndexOf('♥'));
                            //return Json(new { result = 1, content = html, status = status }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (SurveyPaperId.Split(';')[1] != "")//绑定对讲师的评估
                    {
                        var ReplyAnswer =
                  ISurveyReplyAnswerBL.GetList(" ObjectID=" + Courseid + " and ExampaperID=" + SurveyPaperId.Split(';')[1].Split(',')[1] +
                                               " and UserID=" + CurrentUser.UserId);
                        if (ReplyAnswer.Count != 0)
                        {

                            foreach (Survey_ReplyAnswer surveyReplyAnswer in ReplyAnswer)
                            {
                                //html += surveyReplyAnswer.SubjectiveAnswer + surveyReplyAnswer.ObjectiveAnswer + ";";
                                teacherhtml += surveyReplyAnswer.QuestionID + "♣" + surveyReplyAnswer.SubjectiveAnswer + "♣" +
                                        surveyReplyAnswer.QuestionReply + "♥";
                                teacherstatus = surveyReplyAnswer.Status;
                            }

                            teacherhtml = teacherhtml.Substring(0, teacherhtml.LastIndexOf('♥'));
                            //return Json(new { result = 1, content = html, status = status }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
            }
            return Json(new { result = 1, leavestatus = leavestatus, content = html, teachercontent = teacherhtml, status = status, teacherstatus = teacherstatus }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region CPA课程列表

        public JsonResult FCPACourseList(string CourseName, string LearnStatus, string StartTime,string EndTime, int pageSize = 10, int pageIndex = 1)
        {
            if (Session["fcpage"] != null)
            {
                Session.Remove("fcpage");
            }
            Session["fcpage"] = pageIndex + "㉿" + CourseName + "㉿" + LearnStatus + "㉿" + StartTime + "㉿" + EndTime;
            string sql = " ";

            if (!string.IsNullOrEmpty(CourseName))
            {
                if (CourseName != "请输入你要找的课程名称")
                {
                    sql += " and Co_Course.CourseName like '%" + CourseName.ReplaceSql() + "%'";
                }
            }

            if (!string.IsNullOrEmpty(LearnStatus))
            {
                //<option value="0">未开始</option>
                //<option value="1">进行中</option>
                //<option value="2">已完成</option>
                if (LearnStatus != "3")
                {
                    if (LearnStatus == "0")
                    {
                        sql += " and (getdate() < StartTime)";
                    }
                    else if (LearnStatus == "1")
                    {
                        sql += " and (getdate()>=StartTime and getdate()<=EndTime)";
                    }
                    else
                    {
                        sql += " and (getdate() >=EndTime)";
                    }
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
            //if (!string.IsNullOrEmpty(PassStatus))
            //{
            //    if (PassStatus != "3")
            //    {
            //        sql += " and IsPass='" + PassStatus + "'";
            //    }
            //}


            var litme = 0;
            var courseorder = CoCourseBL.GetCPACourseList(out litme, sql, CurrentUser.UserId,
                                                          (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
                {
                    //result = 1,
                    dataList = courseorder,
                    recordCount = litme
                }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 视频课程

        /// <summary>
        /// 视频列表
        /// </summary>
        /// <param name="CourseName"></param>
        /// <param name="IsMust"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public JsonResult FVideoCourseList(string CourseName, string IsMust, int pageSize = 20,int pageIndex = 1, string status = "3")
        {
            if (Session["vcpage"] != null)
            {
                Session.Remove("vcpage");
            }
            Session["vcpage"] = pageIndex + "㉿" + CourseName + "㉿" + IsMust + "㉿" + status;
            string sql = " 1=1";

            if (!string.IsNullOrEmpty(CourseName))
            {
                if (CourseName != "请输入你要找的课程名称")
                {
                    sql += " and CourseName like '%" + CourseName.ReplaceSql() + "%'";
                }
            }
            if (!string.IsNullOrEmpty(IsMust))
            {
                if (IsMust != "2")
                {
                    sql += " and IsMust=" + IsMust;
                }
            }
            if (!string.IsNullOrEmpty(status))
            {
                /*<option value="0">未开始</option>
                 <option value="1">进行中</option>
                 <option value="2">已完成</option>*/
                if (status != "3")
                {
                    if (status == "0")
                    {
                        sql += " and (getdate() < StartTime)";
                    }
                    else if (status == "1")
                    {
                        sql += " and (getdate()>=StartTime and getdate()<=EndTime)";
                    }
                    else
                    {
                        sql += " and (getdate() >=EndTime)";
                    }
                }
            }
            //if (!string.IsNullOrEmpty(PassStatus))
            //{
            //    if (PassStatus != "3")
            //    {
            //        sql += " and Cl_CpaLearnStatus.IsPass='" + PassStatus + "'";
            //    }
            //}

            string IsOpenSub = "";

            if (CurrentUser.IsMain == 1)
            {
                IsOpenSub = " or IsOpenSub=1";
            }

            var litme = 0;
            var courseorder = CoCourseBL.GetVideoCourseList(out litme, sql, CurrentUser.TrainGrade, CurrentUser.UserId, CurrentUser.DeptId, IsOpenSub,
                                                            (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
                {
                    //result = 1,
                    dataList = courseorder,
                    recordCount = litme
                }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 进入考试

        /// <summary>
        /// 进入考试
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <param name="ExamPaperId">试卷ID</param>
        /// <param name="SourceType">1:集中课程 2:视频课程</param>
        /// <returns></returns>
        public JsonResult FGetInto(int courseid, int ExamPaperId, int SourceType)
        {
            var course = CoCourseBL.GetCo_Course(courseid);
            var coursepaper = ICo_CoursePaperBL.GetCo_CourseMainPaper(courseid);//查找关联试卷表

            if (SourceType == 1)
            {
                if (course.AttFlag != 0)
                { 
                    var ttt = ClAttendceBL.GetCl_Attendce(courseid, CurrentUser.UserId);
                    if (ttt != null)
                    {
                        if (course.AttFlag == 1 && (ttt.StartTime == Convert.ToDateTime("2050-1-1") || ttt.StartTime == DateTime.MinValue))
                        {
                            return Json(new { result = 0, message = "请先上课考勤后才能进行在线测试" }, JsonRequestBehavior.AllowGet);
                        }

                        if (course.AttFlag == 2 && (ttt.EndTime == Convert.ToDateTime("2000-1-1") || ttt.EndTime == DateTime.MinValue))
                        {
                            return Json(new { result = 0, message = "请先下课考勤后才能进行在线测试" }, JsonRequestBehavior.AllowGet);
                        }

                        if (course.AttFlag == 3 && (ttt == null || (ttt.StartTime == Convert.ToDateTime("2050-1-1") && ttt.EndTime == Convert.ToDateTime("2000-1-1"))))
                        {
                            return Json(new { result = 0, message = "请先考勤后才能进行在线测试" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { result = 0, message = "请先考勤后才能进行在线测试" }, JsonRequestBehavior.AllowGet);
                    }
                    //if (ttt == null)
                    //{
                    //    return Json(new { result = 0, message = "请先考勤后，再进行考试" }, JsonRequestBehavior.AllowGet);
                    //}
                }
            }

            if (SourceType == 2)
            {
                var vedio = ICpaLearnStatusBL.GetCl_CpaLearnStatusByCourseId(courseid, CurrentUser.UserId);

                DateTime now = DateTime.Now;

                if (now > course.EndTime)
                {
                    return Json(new { result = 0, message = "课程已结束不能进行考试" }, JsonRequestBehavior.AllowGet);
                }

                //查不到学习状态就是没学
                if (vedio == null)
                {
                    return Json(new { result = 0, message = "请先学习课程资源" }, JsonRequestBehavior.AllowGet);
                }


                //var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 25);

                var courseResource = ICoCourseResourceBL.GetVedioList(course.Id, CurrentUser.UserId);

                if (courseResource.Count != 0)
                {
                    if (courseResource[0].ResourceType == 4)
                    {
                        foreach (var clLearnVideoInfor in courseResource)
                        {
                            if (clLearnVideoInfor.Progress < Convert.ToDecimal(course.ReturnTimes))
                            {
                                return Json(new { result = 0, message = "视频还未达到一定进度" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
            }

            //如果课程有课后评估则判断 没有则直接进入考试
            if (course.IsPing == 1)
            {
                if (course.SurveyPaperId != "")
                {
                    var arr = course.SurveyPaperId.Split(';');
                    if (arr[0] != "")
                    {
                        if (!ISurveyReplyAnswerBL.GetStatus(courseid, CurrentUser.UserId, 1, Convert.ToInt32(arr[0].Split(',')[1])))
                        {
                            return Json(new { result = 0, message = "请先提交课后评估，再进行考试" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (arr[1] != "")
                    {
                        if (!ISurveyReplyAnswerBL.GetStatus(courseid, CurrentUser.UserId, 1, Convert.ToInt32(arr[1].Split(',')[1])))
                        {
                            return Json(new { result = 0, message = "请先提交讲师评估，再进行考试" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            DateTime dateTime = DateTime.Now;
            DateTime endtime = course.EndTime.AddHours(Convert.ToDouble(coursepaper.Hour));//根据参数配置的 单位:分钟

            if (dateTime > endtime)
            {
                //return Json(new { result = 0, message = "课程结束后" + coursepaper.Hour + "小时后,才能进行考试" }, JsonRequestBehavior.AllowGet);
                return Json(new { result = 0, message = "考试已经过期" }, JsonRequestBehavior.AllowGet);
            }
            if (SourceType == 1)
            {
                if (dateTime < course.EndTime)
                {
                    return Json(new { result = 0, message = "课程还未结束，无法进行考试" }, JsonRequestBehavior.AllowGet);
                }
            }

            var Student = IExaminationrBL.GetSingletbExamSendStudentByCourseIdAndUserId(courseid, CurrentUser.UserId, SourceType);

            if (Student == null)
            {
                tbExamSendStudent model = new tbExamSendStudent();
                model.RelationID = courseid; //存放课程ID
                model.SourceType = SourceType; //区分0;1
                model.ExamPaperID = ExamPaperId;
                model.UserID = CurrentUser.UserId;
                model.DoExamStatus = 0;
                model.LastUpdateTime = DateTime.Now;
                model.SubmitTime = DateTime.Now;
                model.TestTimes = 0;
                model.IsPass = 0;
                model.Status = 0;
                model.CoursePaperId = coursepaper.id;//添加
                model.StudentAnswerList = new List<ReStudentExamAnswer>();

                int t = IExaminationrBL.InserttbExamSendStudent(model);

                return Json(new { result = 1, euId = model._id }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Student.RelationID != courseid)
                {

                    IExaminationrBL.DeleteExamSendStudentWithByCourseIdAndUserId(courseid, CurrentUser.UserId, SourceType);

                    tbExamSendStudent model = new tbExamSendStudent();
                    model.RelationID = courseid; //存放课程ID
                    model.SourceType = SourceType; //区分0;1
                    model.ExamPaperID = ExamPaperId;
                    model.UserID = CurrentUser.UserId;
                    model.DoExamStatus = 0;
                    model.LastUpdateTime = DateTime.Now;
                    model.SubmitTime = DateTime.Now;
                    model.TestTimes = 0;
                    model.IsPass = 0;
                    model.Status = 0;
                    model.CoursePaperId = coursepaper.id;
                    model.StudentAnswerList = new List<ReStudentExamAnswer>();

                    int t = IExaminationrBL.InserttbExamSendStudent(model);

                    return Json(new { result = 1, euId = model._id }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { result = 1, euId = Student._id }, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region 视频方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LearnVideoInforId"></param>
        /// <param name="allallduration">视频总时长</param>
        /// <param name="Co_CourseResourceId"></param>
        /// <param name="tt">视频当前播放进度</param>
        /// <returns></returns>
        public JsonResult FUpdateProgress(int LearnId, string allallduration, string tt, int ResourseId)
        {
            //算出进度 百分比
            double Progress = Convert.ToDouble(tt) / Convert.ToDouble(allallduration) * 100;

            //找出CPA/视频课程学习状态对应该课程和学员ID 找出学习记录ID
            var CpaLearnStatus = ICpaLearnStatusBL.GetCl_CpaLearnStatusByLearnId(LearnId, CurrentUser.UserId);
            //根据课程ID找出课程信息
            var course = CoCourseBL.GetCo_Course(CpaLearnStatus.CourseID);

            //根据当前播放视频的ID和对应学习记录ID找出视频课程学习详细记录
            var learn = IClLearnVideoInforBL.Get(LearnId, ResourseId);

            //当前视频进度(默认值是0)  
            if (learn.Progress < Convert.ToDecimal(Progress))
            {
                //修改视频进度 播放进度
                IClLearnVideoInforBL.UpdateProgress(Convert.ToDecimal(Progress), LearnId, ResourseId);
            }
            //查找改视频下的所有记录集合
            var courseResource = ICoCourseResourceBL.GetVedioList(CpaLearnStatus.CourseID, CurrentUser.UserId);
            //var hege = courseResource.Where(p => p.Progress >= Convert.ToDecimal(Sys_ParamConfig.ConfigValue)).ToList();
            //找出大于规定进度的视频记录
            var hege = courseResource.Where(p => p.Progress >= Convert.ToDecimal(course.ReturnTimes)).ToList();
            if (courseResource.Count == hege.Count)
            {

                //var CpaLearnStatus = ICpaLearnStatusBL.GetCl_CpaLearnStatusByLearnId(LearnId, CurrentUser.UserId);
                //var course = CoCourseBL.GetCo_Course(CpaLearnStatus.CourseID);
                if (course.IsPing == 0 && course.IsTest == 0)
                {
                    if (CpaLearnStatus.Progress == 0)
                    {
                        ICpaLearnStatusBL.UpdateGetCl_CpaLearnStatusByCourseId(CpaLearnStatus.CourseID, CurrentUser.UserId, course.CourseLength, 1);

                        if (course.IsCPA == 1)
                        {
                            Cl_CpaLearnStatus cls = new Cl_CpaLearnStatus();
                            cls.CourseID = course.Id;
                            cls.UserID = CurrentUser.UserId;
                            cls.IsAttFlag = 0;
                            cls.Progress = 0;
                            cls.LearnTimes = 0;
                            cls.GetLength = course.CourseLength;
                            cls.CpaFlag = 2;
                            cls.GradeStatus = 1;
                            cls.IsPass = 1;

                            ICpaLearnStatusBL.SubscribeCPALearnStatus(cls);
                        }
                    }
                    
                }
            }


            return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 点击学习添加视频/CPA这门课程的学校状态

        /// <summary>
        /// 点击学习添加视频/CPA这门课程的学校状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult LearnVideoInforAdd(int courseid, int coursecourseid, string url, int ResourceType)
        {

            var learn = ICpaLearnStatusBL.GetCl_CpaLearnStatusByCourseId(courseid, CurrentUser.UserId);
            //if (!ICpaLearnStatusBL.IsImport(courseid, CurrentUser.UserId, 0))
            var course = CoCourseBL.GetCo_Course(courseid);

            DateTime now = DateTime.Now;

            if (now < course.StartTime)
            {
                return Json(new { result = 0, content = "课程还未开始无法学习视频" }, JsonRequestBehavior.AllowGet);
            }

            if (now > course.EndTime)
            {
                return Json(new { result = 0, content = "课程已结束无法学习视频" }, JsonRequestBehavior.AllowGet);
            }
            //第一次学 运行的方法
            if (learn == null)
            {
                Cl_CpaLearnStatus model = new Cl_CpaLearnStatus();
                model.CourseID = courseid;
                model.UserID = CurrentUser.UserId;
                model.IsAttFlag = 0;
                model.IsPass = 0;
                model.Progress = 0;
                model.LearnTimes = 0;
                model.GetLength = 0;
                model.CpaFlag = 0;

                ICpaLearnStatusBL.SubscribeCPALearnStatus(model);

                Cl_LearnVideoInfor cl = new Cl_LearnVideoInfor();
                cl.LearnId = model.Id;
                if (ResourceType == 3)
                {
                    cl.Progress = 100;//暂时用于记录是否记录学时 当0的时候 需要在修改学时 如果是1的话 第2次点视频就不获取学时
                }
                else
                {
                    cl.Progress = 0;
                }
                cl.ResourseId = coursecourseid;
                cl.AttendId = 0;
                cl.LearnTimes = 1;

                IClLearnVideoInforBL.SubscribeVedio(cl);

                //3：scorm;4;视频 
                if (ResourceType == 4)
                {
                    if (course.IsPing == 0 && course.IsTest == 0)
                    {
                        //ICpaLearnStatusBL.UpdateGetCl_CpaLearnStatusByCourseId(course.Id, CurrentUser.UserId,course.CourseLength, 1);

                        //if (course.IsCPA == 1)
                        //{
                        //    //ICpaLearnStatusBL
                        //    Cl_CpaLearnStatus cls = new Cl_CpaLearnStatus();
                        //    cls.CourseID = course.Id;
                        //    cls.UserID = CurrentUser.UserId;
                        //    cls.IsAttFlag = 0;
                        //    //cls.IsAttFlag = 1;
                        //    cls.Progress = 0;
                        //    cls.LearnTimes = 0;
                        //    cls.GetLength = course.CourseLength;
                        //    cls.CpaFlag = 2;
                        //    cls.GradeStatus = 1;
                        //    ICpaLearnStatusBL.SubscribeCPALearnStatus(cls);
                        //}
                    }

                    return Json(new
                    {
                        result = 1,
                        learnurl = ConfigurationManager.AppSettings["uploadServiceUrl"] + url + "&LearnId=" + model.Id + "&ResourseId=" + coursecourseid + "&UserId=" + CurrentUser.UserId

                    }, JsonRequestBehavior.AllowGet);
                }
                else //scome课件
                {
                    var CourseResour = ICoCourseResourceBL.GetCo_CourseResourceByCourseId(courseid);

                    string urlstr = "";
                    //if (cw.Type == 0)
                    //{
                    //int packId = cw.PackId;
                    int packId = CourseResour.PackId;
                    int attemptId = new ScormLearn().LearnAttemptItemId(packId);
                    Sys_User username = IUserBL.Get(CurrentUser.UserId);

                    urlstr = ConfigurationManager.AppSettings["scormPlay"] + attemptId + "&username=" +
                             Server.UrlEncode(username.Realname);// +"&roomid=" + id;

                    //当视频课程下面没有评估和考试的时候 直接记录总分
                    if (course.IsPing == 0 && course.IsTest == 0)
                    {
                        //修改状态
                        ICpaLearnStatusBL.UpdateGetCl_CpaLearnStatusByCourseId(course.Id, CurrentUser.UserId,
                                                                               course.CourseLength, 1);

                        if (course.IsCPA == 1)
                        {
                            //ICpaLearnStatusBL
                            Cl_CpaLearnStatus cls = new Cl_CpaLearnStatus();
                            cls.CourseID = course.Id;
                            cls.UserID = CurrentUser.UserId;
                            cls.IsAttFlag = 0;
                            //cls.IsAttFlag = 1;
                            cls.Progress = 0;
                            cls.LearnTimes = 0;
                            if (course.IsMust == 1)
                            {
                                cls.GetLength = course.CourseLength* Convert.ToDecimal(0.5);
                            }
                            if (course.IsMust == 0)
                            {
                                cls.GetLength = course.CourseLength;
                            }
                            
                            cls.CpaFlag = 2;
                            cls.GradeStatus = 1;

                            ICpaLearnStatusBL.SubscribeCPALearnStatus(cls);
                        }
                        //ICpaLearnStatusBL.UpdateProgress(course.Id, CurrentUser.UserId);
                        //ICpaLearnStatusBL.UpdateGetCl_CpaLearnStatusByCourseId();
                    }

                    return Json(new
                    {
                        URL = urlstr
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //根据学习视频ID和对应视频资源ID 查找是否已学过
                if (!IClLearnVideoInforBL.IsImport(learn.Id, coursecourseid))
                {
                    Cl_LearnVideoInfor cl = new Cl_LearnVideoInfor();
                    cl.LearnId = learn.Id;
                    cl.Progress = 0;
                    cl.ResourseId = coursecourseid;
                    cl.AttendId = 0;
                    cl.LearnTimes = 1;

                    IClLearnVideoInforBL.SubscribeVedio(cl);
                }
                else
                {
                    IClLearnVideoInforBL.UpdateLearnTimes(learn.Id, coursecourseid);
                }

                if (ResourceType == 4)
                {
                    return Json(new
                    {
                        result = 1,
                        learnurl = ConfigurationManager.AppSettings["uploadServiceUrl"] + url + "&LearnId=" + learn.Id + "&ResourseId=" + coursecourseid + "&UserId=" + CurrentUser.UserId

                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var CourseResour = ICoCourseResourceBL.GetCo_CourseResourceByCourseId(courseid);

                    string urlstr = "";
                    //if (cw.Type == 0)
                    //{
                    //int packId = cw.PackId;
                    int packId = CourseResour.PackId;
                    int attemptId = new ScormLearn().LearnAttemptItemId(packId);
                    Sys_User username = IUserBL.Get(CurrentUser.UserId);

                    urlstr = ConfigurationManager.AppSettings["scormPlay"] + attemptId + "&username=" +
                             Server.UrlEncode(username.Realname);// +"&roomid=" + id;

                    return Json(new
                    {
                        URL = urlstr
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion


        #region 6.21新加2个方法 
        public JsonResult fsumbitAftercourse(int courseid)
        {
            var course = CoCourseBL.GetCo_Course(courseid);
            if (course.IsPing == 1)
            {
                if (course.AttFlag != 0)
                {
                    var ttt = ClAttendceBL.GetCl_Attendce(courseid, CurrentUser.UserId);
                    if (ttt != null)
                    {
                        if (course.AttFlag == 1 && (ttt.StartTime == Convert.ToDateTime("2050-1-1") || ttt.StartTime == DateTime.MinValue))
                        {
                            return Json(new { result = 1, content = "请先上课考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                        }

                        if (course.AttFlag == 2 && (ttt.EndTime == Convert.ToDateTime("2000-1-1") || ttt.EndTime == DateTime.MinValue))
                        {
                            return Json(new { result = 1, content = "请先下课考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                        }

                        if (course.AttFlag == 3 && (ttt == null || (ttt.StartTime == Convert.ToDateTime("2050-1-1") && ttt.EndTime == Convert.ToDateTime("2000-1-1"))))
                        {
                            return Json(new { result = 1, content = "请先考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { result = 1, content = "请先考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            return Json(new
            {
                result = 0
            }, JsonRequestBehavior.AllowGet);
        }

       

        #endregion


    }

}