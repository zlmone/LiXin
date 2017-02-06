using LiXinInterface.CourseManage;
using LiXinInterface.Examination;
using LiXinInterface.NewCourseManage;
using LiXinInterface.Survey;
using LiXinInterface.User;
using LiXinModels.CourseManage;
using LiXinModels.Examination.DBModel;
using LiXinModels.NewCourseManage;
using LiXinModels.Survey;
using LiXinModels.User;
using Retech.LearningAPI.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace LiXinControllers
{
    public class NewMyCourseController : BaseController
    {
        protected ICo_Course _courseBL;
        protected INew_Course _newcourseBL;
        protected INew_CourseFiles _newcoursefiles;
        protected INew_CourseAdvice _inew_courseadvice;
        protected INew_CourseOrder _inew_courseorderbl;
        protected INew_CoursePaper _INew_CoursePaperBL;
        protected IExamination _IExaminationrBL;
        protected IExampaper _IExampaperBL;
        protected INew_CourseRoomRule _inew_courseroomrule;
        protected ISurveyExampaper _iSurveyExampaperBL;
        protected ISurveyReplyAnswer ISurveyReplyAnswerBL;
        protected INew_Attendce inew_attendce;
        protected INew_CourseOrderDetail inew_courseorderdetail;
        protected INew_CpaLearnStatus inew_cpalearnstatus;
        protected INew_LearnVideoInfor inew_learbvideoinfor;
        protected IUser iuser;


        public NewMyCourseController(ICo_Course courseBL, INew_Course newcourseBL, INew_CourseFiles newcoursefiles,
            INew_CourseAdvice inew_courseadvice, INew_CourseOrder inew_courseorderbl, INew_CoursePaper INew_CoursePaperBL,
            IExamination IExaminationrBL, IExampaper IExampaperBL, INew_CourseRoomRule inew_courseroomrule, ISurveyExampaper iSurveyExampaperBL,
            ISurveyReplyAnswer _ISurveyReplyAnswerBL, INew_Attendce _inew_attendce, INew_CourseOrderDetail _inew_courseorderdetail,
            INew_CpaLearnStatus _inew_cpalearnstatus, INew_LearnVideoInfor _inew_learbvideoinfor, IUser _iuser
            )
        {
            _courseBL = courseBL;
            _newcourseBL = newcourseBL;
            _newcoursefiles = newcoursefiles;
            _inew_courseadvice = inew_courseadvice;
            _inew_courseorderbl = inew_courseorderbl;
            _INew_CoursePaperBL = INew_CoursePaperBL;
            _IExaminationrBL = IExaminationrBL;
            _IExampaperBL = IExampaperBL;
            _inew_courseroomrule = inew_courseroomrule;
            _iSurveyExampaperBL = iSurveyExampaperBL;
            ISurveyReplyAnswerBL = _ISurveyReplyAnswerBL;
            inew_attendce = _inew_attendce;
            inew_courseorderdetail = _inew_courseorderdetail;
            inew_cpalearnstatus = _inew_cpalearnstatus;
            inew_learbvideoinfor = _inew_learbvideoinfor;
            iuser = _iuser;
        
        }

        #region 页面呈现
        /// <summary>
        /// 日历页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Calendar()
        {
            return View();
        }
        /// <summary>
        /// 总页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseList(string p="0")
        {
            ViewBag.page = p;
            return View();
        }
        /// <summary>
        /// 我的课程列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCourseList(string tp)
        {

            ViewBag.page = 1;
            ViewBag.coursename = "请输入搜索内容";
            ViewBag.roomname = "请输入搜索内容";
            ViewBag.stat = 3;
            //ViewBag.starttime = 2;
            //ViewBag.endtime = 3;
            // var a = GetDepartments;
            if (tp != null && tp != "" && tp == "1")
            {
                if (Session["NewMyCourseList"] != null)
                {
                    string sess = Session["NewMyCourseList"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);

                    ViewBag.page = att[0];
                    ViewBag.coursename = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.roomname = att[2] == "" ? "请输入搜索内容" : att[2];
                    ViewBag.starttime = att[3];
                    ViewBag.endtime = att[4];
                    ViewBag.stat = att[5];
                }
            }
            return View();
        }
        /// <summary>
        /// 详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCourse(int courseid,int tp=0)
        {
            ViewBag.course = _newcourseBL.GetCourseByCourseRoomRule(courseid);
            ViewBag.tp = tp;
            return View();
        }
        /// <summary>
        /// 课后评估
        /// </summary>
        /// <returns></returns>
        public ActionResult AfterCourse(int courseid)
        {
            var list = _inew_courseroomrule.GetNew_CourseRoomRuleListByCourseId(courseid, "  New_CourseRoomRule.Id in (select SubCourseID from New_CourseOrderDetail where CourseId=" + courseid + " and UserId=" + CurrentUser.UserId + " and IsLeave=0)");

            List<Survey_Exampaper> Survey_Exampaperlist = new List<Survey_Exampaper>();
            string pinglist = "";

            if (list.Count != 0)
            {
                //foreach (var item in list)
                //{
                //    if (item.PingId != 0)
                //    {
                //        //var tt = _iSurveyExampaperBL.GetSurveyExampaper(item.PingId).Questions;
                //        Survey_Exampaperlist.Add(_iSurveyExampaperBL.GetSurveyExampaper(item.PingId));
                //        pinglist += item.PingId + ",";
                //    }
                //}
                ////所有讲师评估集合
                //ViewBag.Survey_Exampaperlist = Survey_Exampaperlist;                
                ViewBag.list = list;

                ViewBag.pingid = list[0].PingId;

                

                ViewBag.subid = list[0].Id;//获取第一个规则ID
               
            }
            
            if (pinglist != "")
            {
                ViewBag.pinglist = pinglist.Substring(0, pinglist.LastIndexOf(','));
            }
            else
            {
                ViewBag.pinglist = "0";
            }
            ViewBag.courseid = courseid;

            return View();
        }

        public ActionResult BindExampaper(int pingid, int subid, string backurl="")
        {
            if (pingid != 0)
            {
                ViewBag.backurl = backurl;
                //var tt = _iSurveyExampaperBL.GetSurveyExampaper(item.PingId).Questions;
                ViewBag.Survey_Exampaperlist=_iSurveyExampaperBL.GetSurveyExampaper(pingid);
                ViewBag.pingid = pingid;
                ViewBag.subid = subid;
            }

            return View();
        }


        /// <summary>
        /// 考勤页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Attendance(int courseid)
        {
            ViewBag.courseid = courseid;
            return View();
        }
        /// <summary>
        /// 课前建议
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassPro(int courseid)
        {
            ViewBag.courseid = courseid;
            return View();
        }
        /// <summary>
        /// 在线测试
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineTest(int courseid)
        {
            New_CoursePaper newcoursepaper = _INew_CoursePaperBL.GetSingleCoursePaper(courseid);
            var course = _newcourseBL.GetSingleCourse(courseid);
            if (newcoursepaper != null)
            {
                var exap = _IExaminationrBL.GetExamination(newcoursepaper.PaperId);
                ViewBag.exap = exap;
                // var examsendstudent = IExaminationrBL.GetExamSendStudent(exap.PaperID);
                var examsendstudent = _IExaminationrBL.GetSingletbExamSendStudentByCourseIdAndUserId(courseid,
                                                                                                    CurrentUser.UserId, 3);
                var exampapger = _IExampaperBL.GetExampaper(newcoursepaper.PaperId);
                ViewBag.exampapger = exampapger;
                ViewBag.examsendstudent = examsendstudent;
            }
            ViewBag.New_CoursePaper = newcoursepaper;
            ViewBag.courseid = courseid;
            ViewBag.course = course;


            return View();
        }
        /// <summary>
        /// 课程资源和教室图
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseMain(int courseid)
        {
            //课程附件
            var list = _newcoursefiles.GetCourseResourceList(courseid);
            ViewBag.CourseResourceList = list;

            var New_CourseOrderDetaillist = inew_courseorderdetail.GetNew_CourseOrderDetailSeatDetail(courseid.ToString() ,CurrentUser.UserId);

            ViewBag.New_CourseOrderDetaillist = New_CourseOrderDetaillist;

            //课程信息
            ViewBag.Memo = _newcourseBL.GetSingleCourse(courseid).Memo;

            return View();
        }

        #endregion

        #region 自学课程页面呈现
        public ActionResult StudyMyCourse(int courseid,int tp=0)
        {
            ViewBag.course = _newcourseBL.GetCourseByCourseRoomRule(courseid);
            ViewBag.tp = tp;
            return View();
        }

        public ActionResult StudyCourseMain(int courseid)
        {
            var list = _newcoursefiles.GetCourseResourceList(courseid);
            ViewBag.CourseResourceList = list;

            var New_CourseOrderDetaillist = inew_courseorderdetail.GetNew_CourseOrderDetailSeatDetail(courseid.ToString(), CurrentUser.UserId);

            ViewBag.New_CourseOrderDetaillist = New_CourseOrderDetaillist;


            var courseResource = _newcoursefiles.GetVedioList(courseid, CurrentUser.UserId);
            ViewBag.courseResource = courseResource;


            ViewBag.courseid = courseid;

            return View();
        }

        public ActionResult StudyAfterCourse(int courseid)
        {

            var course = _newcourseBL.GetSingleCourse(courseid);

            if (course.IsPingCourse != 0)
            {
                var Survey_Exampaperlist = _iSurveyExampaperBL.GetSurveyExampaper(course.IsPingCourse);
                ViewBag.Survey_Exampaperlist = Survey_Exampaperlist;
            }
            ViewBag.IsPingCourse = course.IsPingCourse;
            ViewBag.courseid = courseid;

            return View();
        }

        public ActionResult StudyOnlineTest(int courseid)
        {
            var course = _newcourseBL.GetSingleCourse(courseid);

            if (course.IsTest == 1)
            { 
                New_CoursePaper newcoursepaper = _INew_CoursePaperBL.GetSingleCoursePaper(courseid);
            
                if (newcoursepaper != null)
                {
                    var exap = _IExaminationrBL.GetExamination(newcoursepaper.PaperId);
                    ViewBag.exap = exap;
                    // var examsendstudent = IExaminationrBL.GetExamSendStudent(exap.PaperID);
                    var examsendstudent = _IExaminationrBL.GetSingletbExamSendStudentByCourseIdAndUserId(courseid,
                                                                                                        CurrentUser.UserId, 3);
                    var exampapger = _IExampaperBL.GetExampaper(newcoursepaper.PaperId);
                    ViewBag.exampapger = exampapger;
                    ViewBag.examsendstudent = examsendstudent;
                }
                ViewBag.New_CoursePaper = newcoursepaper;
                ViewBag.courseid = courseid;
                ViewBag.course = course;
            }
            return View();
        }

        public ActionResult StudyCourseList()
        {
            return View();
        }

        #endregion

        #region 日历呈现
        public JsonResult GetCalendarTask(string year = "", string month = "", int addMonth = 0)
        {
            year = year == "" ? DateTime.Now.Year.ToString() : year;
            month = month == "" ? DateTime.Now.Month.ToString() : month;

            int total = 0;
            DateTime quMonth = Convert.ToDateTime(year + "-" + month + "-1").AddMonths(addMonth);
            string _year = quMonth.Year.ToString();
            string _month = quMonth.Month.ToString();
            if (_month.Length == 1)
            {
                _month = "0" + _month;
            }
            List<New_CourseOrder> listCourse = _inew_courseorderbl.GetStudentLearnDetal(CurrentUser.UserId, " 1=1");

            //初始化日历集合
            var calendarTask = new List<CalendarTask>();
            InitCalendarTaskCollection(ref calendarTask, quMonth);

            
            //加载当前月
            for (int i = 0; ; i++)
            {
                var morningstr = "";
                var afterstr = "";
                DateTime newdate = quMonth.AddDays(i);
                if (newdate.Month != quMonth.Month)
                    break;
                listCourse.Where(c => c.StartTime.Date >= newdate && c.StartTime.Date < newdate.AddDays(1)).ToList().
                    ForEach(p =>
                    {
                        if (p.StartTime >= newdate && p.StartTime < newdate.AddHours(12))
                        {
                            if (!string.IsNullOrEmpty(p.CourseName))
                            {
                                morningstr += " 《" + p.CourseName + "》 ";
                            }
                        }
                        if (p.StartTime >= newdate.AddHours(12) && p.StartTime < newdate.AddDays(1))
                        {
                            if (!string.IsNullOrEmpty(p.CourseName))
                            {
                                afterstr += " 《" + p.CourseName + "》 ";
                            }
                        }
                    });
                calendarTask.Add(new CalendarTask
                {
                    Year = newdate.Year,
                    Month = newdate.Month,
                    Day = newdate.Day,
                    Bg = (newdate.Day == DateTime.Now.Day && newdate.Month == DateTime.Now.Month) ? 3 : 1,
                    MoringStr = morningstr.Trim(),
                    AfterStr = afterstr.Trim(),
                    TaskTotal = listCourse.Where(c => c.StartTime.Day == newdate.Day).Count()
                });
            }
            //加载后续的几个日子
            AddNextMonthDays(ref calendarTask, quMonth);

            return Json(new
            {
                dataList = calendarTask,
                year = quMonth.Year,
                month = quMonth.Month
            },
                        JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取当天课程
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public JsonResult GetCourseByDay(DateTime starttime,DateTime endtime)
        {
            var a = (0 / 100) * 2; 
            //starttime = DateTime.Now;
            //endtime = starttime.AddDays(1);
            int total = 0;         

            var list = _inew_courseorderbl.GetStudentLearnDetal(CurrentUser.UserId, " New_CourseRoomRule.StartTime>='" + starttime + "' and New_CourseRoomRule.StartTime<='" + endtime + "'");

            return Json(new
            {
                dataList = list
            },  JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 我的课程列表
        public JsonResult GetNewMyCourseList(string coursename, string room, string starttime, string endtime, string coursestatus, string jsRenderSortField = " order by New_Course.StartTime ", int pageSize = 10, int pageIndex = 1)
        {

            if (Session["NewMyCourseList"] != null)
            {
                Session.Remove("NewMyCourseList");
            }
            Session["NewMyCourseList"] = pageIndex + "㉿" + coursename + "㉿" + room + "㉿" + starttime + "㉿" + endtime + "㉿" + coursestatus;

            string sql = " 1=1";
            string strsql = " where 1=1";
            int limit = 0;
            if (!string.IsNullOrEmpty(coursename))
            {
                sql += " and New_Course.CourseName like '%" + coursename + "%'";
            }

            if (!string.IsNullOrEmpty(room)&&room!="0")
            {
                strsql += " and roomname like '%" + room + "%'";
            }
     
            if (!string.IsNullOrEmpty(coursestatus) && coursestatus != "3")
            {
                if (coursestatus == "0")
                {
                    sql += "and StartTime>getdate()";
                }
                if (coursestatus == "1")
                {
                    sql += " and (StartTime<=getdate() and EndTime>=getdate())  ";
                }
                if (coursestatus == "2")
                {
                    sql += " and EndTime<getdate()";
                }
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                sql += " and StartTime>='" + starttime + "'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                sql += " and EndTime<='" + endtime + "'";
            }

            var list = _inew_courseorderbl.GetNew_CourseOrderListByStudent(CurrentUser.UserId, sql, strsql, int.MaxValue, pageSize, "order by " + jsRenderSortField);

            limit = list.Count();

            list = list.Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();

            return Json(new
            {
                dataList = list,
                recordCount = limit
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 我对老师的评价
        public JsonResult GetMyToTeacherCommentList(string coursename, string teacher, string starttime, string endtime, int pageSize = 10, int pageIndex = 1)
        {
            return Json(new
            {

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 老师对我的评价

        public JsonResult GetTeacherToMyCommentList(string coursename, string teacher, string starttime, string endtime, int pageSize = 10, int pageIndex = 1)
        {
            return Json(new
            {

            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 课前建议
        /// <summary>
        /// 添加课前建议
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="AdviceContent"></param>
        /// <returns></returns>
        public JsonResult SubmitCl_CourseAdvice(int CourseId, string AdviceContent)
        {           
            //var courseorder = inew_courseorderdetail.GetSingleNew_CourseOrderDetail(CourseId, CurrentUser.UserId);

            DateTime datetime = DateTime.Now;

            var course = _newcourseBL.GetSingleCourse(CourseId);
            var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 29);

            DateTime starttime = course.StartTime.AddHours(-Convert.ToDouble(Sys_ParamConfig.ConfigValue.Split(';')[0]));

            if (datetime > starttime)
            {
                return Json(
                    new { result = 1, content = "课前开放" + Sys_ParamConfig.ConfigValue.Split(';')[0] + "小时，允许提建议" },
                    JsonRequestBehavior.AllowGet);
            }
            
            New_CourseAdvice model = new New_CourseAdvice();
            model.CourseId = CourseId;
            model.AdviceContent = AdviceContent;
            model.AdviceTime = DateTime.Now;
            model.UserId = CurrentUser.UserId;
            model.SubCourseID = 0;//暂时没用
            model.VisibleFlag = 0;  //0：都不可见；1：仅提出人；2：全部
            model.IsDelete = 0;
            _inew_courseadvice.SubmitCl_CourseAdvice(model);
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


        public JsonResult OnLoadNew_CourseAdvice(int id)
        {            
            string html = "";

            string otherhtml = "";
            
            List<New_CourseAdvice> clCourseAdvicesList = _inew_courseadvice.GetList("  New_CourseAdvice.CourseId=" + id);

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
            
            return Json(new
            {
                content = html,
                other = otherhtml                

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
            New_CourseAdvice model = new New_CourseAdvice();
            model.Id = id;
            model.CourseId = CourseId;
            model.AnserContent = AnserContent;
            model.AnserTime = DateTime.Now;
            model.VisibleFlag = VisibleFlag;
            model.AnserUserId = CurrentUser.UserId;

            if (_inew_courseadvice.ReplyCl_CourseAdvice(model))
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

        #region 进入考试
        /// <summary>
        /// 进入考试
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <param name="ExamPaperId">试卷ID</param>
        /// <param name="SourceType">1:集中课程 2:视频课程 3：新进人员考试</param>
        /// <returns></returns>
        public JsonResult FGetInto(int courseid, int ExamPaperId, int SourceType=3)
        {
            var course = _newcourseBL.GetSingleCourse(courseid);
            var coursepaper = _INew_CoursePaperBL.GetSingleCoursePaper(courseid);//查找关联试卷表

            DateTime now = DateTime.Now;
                     
            if (course.Way == 1)
            {
                var list = inew_courseorderdetail.GetCourseOrderDetailIsSingleOrMore(courseid,CurrentUser.UserId);

                if (list.Where(p => p.IsLeave == 0).Count() == list.Where(p => p.IsLeave == 1).Count())
                {
                    //if (list[0].IsLeave == 1)
                    //{
                        return Json(new { result = 0, message = "你已请假，不能进行考试" }, JsonRequestBehavior.AllowGet);
                    //}
                }

                if (now < course.EndTime)
                {
                    return Json(new { result = 0, message = "课程还没结束，不能进行考试" }, JsonRequestBehavior.AllowGet);
                }

                //var attendce = inew_attendce.GetList(" CourseId=" + courseid + " and UserId=" + CurrentUser.UserId);

                //if (attendce.Count() == 0)
                //{
                //    return Json(new { result = 0, message = "请先考勤后才能进行考试" }, JsonRequestBehavior.AllowGet);
                //}

            }
            else
            {
                var vedio = inew_cpalearnstatus.GetCl_CpaLearnStatusByCourseId(courseid, CurrentUser.UserId);

                if (now > course.EndTime)
                {
                    return Json(new { result = 0, message = "课程已结束，不能进行考试" }, JsonRequestBehavior.AllowGet);
                }

                if (vedio == null)
                {
                    return Json(new { result = 0, message = "请先学习课程资源" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var courseResource = _newcoursefiles.GetVedioList(courseid, CurrentUser.UserId);

                    if (courseResource.Count != 0)
                    {
                        if (courseResource[0].Type == 4)
                        {
                            foreach (var clLearnVideoInfor in courseResource)
                            {
                                if (clLearnVideoInfor.Progress < Convert.ToDecimal(course.VideoLowLength))
                                {
                                    return Json(new { result = 0, message = "视频还未达到一定进度" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                    }
                }

            }
         
            var Student = _IExaminationrBL.GetSingletbExamSendStudentByCourseIdAndUserId(courseid, CurrentUser.UserId, SourceType);

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
                model.CoursePaperId = coursepaper.Id;//添加
                model.StudentAnswerList = new List<ReStudentExamAnswer>();

                int t = _IExaminationrBL.InserttbExamSendStudent(model);

                return Json(new { result = 1, euId = model._id }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (Student.RelationID != courseid)
                {

                    _IExaminationrBL.DeleteExamSendStudentWithByCourseIdAndUserId(courseid, CurrentUser.UserId, SourceType);

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
                    model.CoursePaperId = coursepaper.Id;
                    model.StudentAnswerList = new List<ReStudentExamAnswer>();

                    int t = _IExaminationrBL.InserttbExamSendStudent(model);

                    return Json(new { result = 1, euId = model._id }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { result = 1, euId = Student._id }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 添加评估
        /// <summary>
        /// 添加评估
        /// </summary>
        /// <param name="tt"></param>
        /// <param name="courseId"></param>
        /// <param name="type"></param>
        /// <param name="ExampaperID"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public JsonResult FInsertSurvey_ReplyAnswer(string tt, int courseId, string ExampaperID, int courseroomrunleid=0)
        {
            var course = _newcourseBL.GetSingleCourse(courseId);

            //var attendce = inew_attendce.GetList(" CourseId=" + courseId + " and UserId=" + CurrentUser.UserId);

            var attendce = inew_attendce.GetNew_Attendce(courseId, CurrentUser.UserId, courseroomrunleid);

            var rule = _inew_courseroomrule.Get(" id=" + courseroomrunleid);

            if (rule != null)
            { 
                if (rule.IsAttFlag==3)
                {
                    //if(attendce.StartTime.Year==2050)
                }
                else if (rule.IsAttFlag == 1)
                {
                    if (attendce == null || (attendce.StartTime.Year == 2050 || attendce.StartTime.Year == 1))
                    {
                        return Json(new { result = 1, content = "请先考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (rule.IsAttFlag == 2)
                {
                    if (attendce == null || (attendce.EndTime.Year == 2050 || attendce.EndTime.Year == 1))
                    {
                        return Json(new { result = 1, content = "请先考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (rule.IsAttFlag == 0)
                {

                    if (attendce==null ||(attendce.StartTime.Year == 2050 || attendce.EndTime.Year == 1))
                    {
                        return Json(new { result = 1, content = "请先考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            //if (attendce.Count() == 0)
            //{
            //    return Json(new { result = 1, content = "请先考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
            //}

            DateTime datetime = DateTime.Now;

            DateTime endtime = rule.EndTime;
            if (datetime < endtime)
            {
                return
                    Json(
                        new
                        {
                            result = 1,                            
                            content = "课程结束后，才能提交评估"
                        },
                        JsonRequestBehavior.AllowGet);
            }

            DateTime dd = endtime.AddHours((double)rule.AfterEvlutionConfigTime);

            if (datetime > dd)
            {
                return
                                   Json(
                                       new
                                       {
                                           result = 1,
                                           content = "提交评估时间已过期"
                                       },
                                       JsonRequestBehavior.AllowGet);
            }


                   
                    //if (ISurveyReplyAnswerBL.GetStatus(courseId, CurrentUser.UserId, 0, ExampaperID))
                    //{
                    //    ISurveyReplyAnswerBL.DeleteByCourseIDAndUserId(courseId, CurrentUser.UserId, ExampaperID);
                    //}
                    
                    ////判断是否提交多次 有数据则跳出 
                    if (ISurveyReplyAnswerBL.GetList(" ObjectID=" + course.Id + " and UserID=" + CurrentUser.UserId + " and [ExampaperID]=" +
                        ExampaperID + " and Status=1 and CourseRoomRuleId=" + courseroomrunleid).Count > 0)
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
                        surveyReply.ExampaperID = Convert.ToInt32(arr[i].Split('♣')[0]);
                        surveyReply.QuestionID = Convert.ToInt32(arr[i].Split('♣')[1]);
                        surveyReply.SubjectiveAnswer = arr[i].Split('♣')[2];
                        surveyReply.QuestionReply = arr[i].Split('♣')[3];
                        surveyReply.UserID = CurrentUser.UserId;
                        surveyReply.Status = Convert.ToInt32(1);


                        surveyReply.CourseRoomRuleId = courseroomrunleid;
                        ISurveyReplyAnswerBL.InsertSurvey_ReplyAnswer(surveyReply);
                    }                
                    return Json(new { result = 0, content = "提交成功" }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult FBindAllSurvey_ReplyAnswer(int Courseid, string SurveyPaperId, int courseroomrunleid=0)
        {
            string html = "";
            string teacherhtml = "";
            int status = 0;
            int teacherstatus = 0;

            //if (SurveyPaperId != "")
            if (!string.IsNullOrEmpty(SurveyPaperId) && SurveyPaperId!="0")
            {
                string[] arr=SurveyPaperId.Split(',');
                for (int i = 0; i < arr.Length; i++)
			    {

                    var ReplyAnswer =
           ISurveyReplyAnswerBL.GetList(" ObjectID=" + Courseid + " and ExampaperID=" + arr[i] +
                                        " and UserID=" + CurrentUser.UserId + " and CourseRoomRuleId=" + courseroomrunleid);
                    if (ReplyAnswer.Count != 0)
                    {

                        foreach (Survey_ReplyAnswer surveyReplyAnswer in ReplyAnswer)
                        {
                            html += surveyReplyAnswer.QuestionID + "♣" + surveyReplyAnswer.SubjectiveAnswer + "♣" +
                                            surveyReplyAnswer.QuestionReply + "♥";

                        }
                    }
                    
			    }
                if (html != "")
                {
                    html = html.Substring(0, html.LastIndexOf("♥"));
                }
                return Json(new { result = 1, content = html }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = 1, content = html }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyNew_Attend(int courseid, int pageSize = 10, int pageIndex = 1)
        {
            int lit = 0;
            var list = inew_attendce.GetNewAttendList(courseid, out lit, pageIndex, pageSize, "asc", " na.userid=" + CurrentUser.UserId);
            //var list = inew_attendce.GetNewAttendUserList(courseid, out lit, pageIndex, pageSize, "asc", " na.userid=" + CurrentUser.UserId);
            int n = 0;
            var itemArray = new object[list.Count()];

            foreach (var item in list)
            {
                var temp = new
                {
                    CourseType = item.Type == 0 ? "集中授课" : "分组带教",
                    CourseTime = item.CoTimeStr,
                    UserNum = item.NumberID,
                    TeacherName=item.teachername,
                    UserRealName = item.Realname,
                    AttendceStartTime = item.StartTimeStr,
                    AttendceEndTime = item.EndTimeStr,
                    AttStatusStr = item.AttStatusStr

                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                dataList = itemArray,
                recordCount = lit
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 自学课程
        public JsonResult GetStudyMyCourseList(string coursename,  string starttime, string endtime,string coursestatus, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " 1=1";
            string strsql = " where 1=1";
            int limit = 0;
            if (!string.IsNullOrEmpty(coursename))
            {
                sql += " and New_Course.CourseName like '%" + coursename + "%'";
            }            

            if (!string.IsNullOrEmpty(coursestatus) && coursestatus != "3")
            {
                if (coursestatus == "0")
                {
                    sql += "and StartTime>getdate()";
                }
                if (coursestatus == "1")
                {
                    sql += " and (StartTime<=getdate() and EndTime>=getdate())  ";
                }
                if (coursestatus == "2")
                {
                    sql += " and EndTime<getdate()";
                }
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                sql += " and StartTime>='" + starttime + "'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                sql += " and EndTime<='" + endtime + "'";
            }

            var list = _newcourseBL.GetStudyList(out limit, CurrentUser.UserId, sql, (pageIndex - 1) * pageSize + 1, pageSize);


            return Json(new
            {
                dataList = list,
                recordCount = limit
            }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 视频学时
        public JsonResult LearnVideoInforAdd(int courseid, int coursecourseid, string url, int ResourceType)
        {

            //var learn = ICpaLearnStatusBL.GetCl_CpaLearnStatusByCourseId(courseid, CurrentUser.UserId);
            var learn = inew_cpalearnstatus.GetCl_CpaLearnStatusByCourseId(courseid,CurrentUser.UserId);
            
            var course = _newcourseBL.GetSingleCourse(courseid);

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
                New_CpaLearnStatus model = new New_CpaLearnStatus();
                model.CourseID = courseid;
                model.UserID = CurrentUser.UserId;
                model.IsAttFlag = 0;
                model.IsPass = 0;
                model.Progress = 0;
                model.LearnTimes = 0;
                model.GetLength = 0;
                model.CpaFlag = 0;

                inew_cpalearnstatus.SubscribeCPALearnStatus(model);

                New_LearnVideoInfor cl = new New_LearnVideoInfor();
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

                inew_learbvideoinfor.SubscribeVedio(cl);                

                //3：scorm;4;视频 
                if (ResourceType == 4)
                {
                    
                    return Json(new
                    {
                        result = 1,
                        learnurl = ConfigurationManager.AppSettings["uploadServiceUrl"] + url + "&LearnId=" + model.Id + "&ResourseId=" + coursecourseid + "&UserId=" + CurrentUser.UserId+"&type=1"

                    }, JsonRequestBehavior.AllowGet);
                }
                else //scome课件
                {

                    var CourseResour = _newcoursefiles.GetSingleCourseFile(courseid);

                    string urlstr = "";
                    //if (cw.Type == 0)
                    //{
                    //int packId = cw.PackId;
                    int packId = CourseResour.PackId;
                    int attemptId = new ScormLearn().LearnAttemptItemId(packId);
                    Sys_User username = iuser.Get(CurrentUser.UserId);

                    urlstr = ConfigurationManager.AppSettings["scormPlay"] + attemptId + "&username=" +
                             Server.UrlEncode(username.Realname);// +"&roomid=" + id;

                    return Json(new
                    {
                        URL = urlstr
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //根据学习视频ID和对应视频资源ID 查找是否已学过

                if (!inew_learbvideoinfor.IsImport(learn.Id, coursecourseid))
                {
                    New_LearnVideoInfor cl = new New_LearnVideoInfor();
                    cl.LearnId = learn.Id;
                    cl.Progress = 0;
                    cl.ResourseId = coursecourseid;
                    cl.AttendId = 0;
                    cl.LearnTimes = 1;

                    inew_learbvideoinfor.SubscribeVedio(cl);
                }
                else
                {
                    inew_learbvideoinfor.UpdateLearnTimes(learn.Id, coursecourseid);
                }

                if (ResourceType == 4)
                {
                    return Json(new
                    {
                        result = 1,
                        learnurl = ConfigurationManager.AppSettings["uploadServiceUrl"] + url + "&LearnId=" + learn.Id + "&ResourseId=" + coursecourseid + "&UserId=" + CurrentUser.UserId + "&type=1"

                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var CourseResour = _newcoursefiles.GetSingleCourseFile(courseid);

                    string urlstr = "";
                    //if (cw.Type == 0)
                    //{
                    //int packId = cw.PackId;
                    int packId = CourseResour.PackId;
                    int attemptId = new ScormLearn().LearnAttemptItemId(packId);
                    Sys_User username = iuser.Get(CurrentUser.UserId);

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

        #region 视频提交评估
        public JsonResult FBindSurvey_ReplyAnswer(string tt, int courseId, string ExampaperID)
        {
            var course = _newcourseBL.GetSingleCourse(courseId);

            var vedio = inew_cpalearnstatus.GetCl_CpaLearnStatusByCourseId(courseId, CurrentUser.UserId);

            DateTime datetime = DateTime.Now;           
           

            if (vedio == null)
            {
                return Json(new { result = 1, content = "请先学习课程资源" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var courseResource = _newcoursefiles.GetVedioList(courseId, CurrentUser.UserId);

                if (courseResource.Count != 0)
                {
                    if (courseResource[0].Type == 4)
                    {
                        foreach (var clLearnVideoInfor in courseResource)
                        {
                            if (clLearnVideoInfor.Progress < Convert.ToDecimal(course.VideoLowLength))
                            {
                                return Json(new { result = 1, content = "视频还未达到一定进度" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
            }
            DateTime endtime = course.EndTime;
            if (datetime > endtime)
            {
                return
                    Json(
                        new
                        {
                            result = 1,
                            content = "课程已结束，不能提交评估"
                        },
                        JsonRequestBehavior.AllowGet);
            }

            //DateTime endtime = course.EndTime;
            //if (datetime > endtime.AddHours(course.AfterEvlutionConfigTime))
            //{
            //    return
            //        Json(
            //            new
            //            {
            //                result = 1,
            //                content = "课程已结束，不能提交评估"
            //            },
            //            JsonRequestBehavior.AllowGet);
            //}

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
                surveyReply.ExampaperID = Convert.ToInt32(arr[i].Split('♣')[0]);
                surveyReply.QuestionID = Convert.ToInt32(arr[i].Split('♣')[1]);
                surveyReply.SubjectiveAnswer = arr[i].Split('♣')[2];
                surveyReply.QuestionReply = arr[i].Split('♣')[3];
                surveyReply.UserID = CurrentUser.UserId;
                surveyReply.Status = Convert.ToInt32(1);
                ISurveyReplyAnswerBL.InsertSurvey_ReplyAnswer(surveyReply);
            }
            return Json(new { result = 0, content = "提交成功" }, JsonRequestBehavior.AllowGet);

        }
        #endregion


        #region 下载资源
        public void LoadPrincipleFile(string path, string name,int id)
        {
            //if (System.IO.File.Exists(Server.MapPath(pathurl + path)))
            if (System.IO.File.Exists(Server.MapPath(path)))
            {
                if (_newcoursefiles.UpdateLoadTimes(id))
                { 
                var filePath = Server.MapPath(path); //路径 
                //以字符流的形式下载文件
                var fs = new FileStream(filePath, FileMode.Open);
                var bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.ContentType = "application/octet-stream";
                //通知浏览器下载文件而不是打开             

                //Response.AddHeader("Content-Disposition",
                //                       "attachment;  filename=" + HttpUtility.UrlEncode(name, Encoding.UTF8));

                var downloadfilename = name;

                if (Request.UserAgent.ToLower().IndexOf("msie") > -1)
                {
                    downloadfilename = HttpUtility.UrlPathEncode(downloadfilename);
                }

                if (Request.UserAgent.ToLower().IndexOf("firefox") > -1)
                {
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + downloadfilename + "\"");
                }

                else
                {
                    //Response.AddHeader("Content-Disposition", "attachment;filename=" + downloadfilename);
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(name, Encoding.UTF8));
                }

                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
                }
            }
            else
            {
                Response.Write("此附件已不存在");
            }
        }
        #endregion

    }
}
