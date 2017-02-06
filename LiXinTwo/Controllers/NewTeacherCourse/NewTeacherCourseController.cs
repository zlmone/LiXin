using LiXinInterface.Examination;
using LiXinInterface.NewCourseManage;
using LiXinInterface.Survey;
using LiXinModels.CourseManage;
using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace LiXinControllers
{
    public class NewTeacherCourseController : BaseController
    {

        //protected INew_CourseAdvice _inew_courseadvice;

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
        protected ISurveyQuestion ISurveyQuestionBL;
        protected INew_Attendce inew_attendce;




        public NewTeacherCourseController(INew_Course newcourseBL, INew_CourseFiles newcoursefiles,
            INew_CourseAdvice inew_courseadvice, INew_CourseOrder inew_courseorderbl, INew_CoursePaper INew_CoursePaperBL,
            IExamination IExaminationrBL, IExampaper IExampaperBL, INew_CourseRoomRule inew_courseroomrule, ISurveyExampaper iSurveyExampaperBL,
            ISurveyReplyAnswer _ISurveyReplyAnswerBL, ISurveyQuestion _ISurveyQuestionBL, INew_Attendce _inew_attendce
            )
        {
           
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
            ISurveyQuestionBL = _ISurveyQuestionBL;
            inew_attendce = _inew_attendce;
        
        }


        #region 页面呈现

        /// <summary>
        /// 课程ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UploadResource(int id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult AfterCourseNew(int courseid, int PingId, int sub)
        {

            var afterquestion = ISurveyQuestionBL.GetSurvey_QuestionByExampaperID(PingId);
            ViewBag.afterquestion = afterquestion;
            ViewBag.sub = sub;

            //加载第一个问题的答案
            var firsthtml = "";
            if (afterquestion.Count != 0)
            {
                firsthtml = FindAnswerString(courseid, afterquestion[0].ExampaperID, afterquestion[0].QuestionType, afterquestion[0].QuestionID,sub);
            }

            ViewBag.firsthtml = firsthtml;
            ViewBag.courseid = courseid;
            return View();
        }



        public ActionResult AfterCourse(int courseid)
        {
            var course = _newcourseBL.GetSingleCourse(courseid);
            ViewBag.IsGroupTeach = course.IsGroupTeach;//找到标签


            var list = _inew_courseroomrule.GetNew_CourseRoomRuleListByCourseId(courseid, " TeacherId=" + CurrentUser.UserId);
            ViewBag.list = list;
            
            ViewBag.courseid = courseid;
            ViewBag.pingid = list[0].PingId;
            ViewBag.sub = list[0].Id;

            //if (course.IsGroupTeach == 1 || course.IsGroupTeach==3)
            //{
            //    var afterquestion = ISurveyQuestionBL.GetSurvey_QuestionByExampaperID(list.Where(p=>p.Type==0).Select(p=>p.PingId).FirstOrDefault());
            //    ViewBag.afterquestion = afterquestion;

            //    //加载第一个问题的答案
            //    var firsthtml = "";
            //    if (afterquestion.Count != 0)
            //    {
            //        firsthtml = FindAnswerString(courseid, afterquestion[0].ExampaperID, afterquestion[0].QuestionType, afterquestion[0].QuestionID);
            //    }

            //    ViewBag.firsthtml = firsthtml;
            //    ViewBag.pingid = afterquestion[0].ExampaperID;

            //}

            //if (course.IsGroupTeach == 2 || course.IsGroupTeach == 3)
            //{
            //    var fenzuafterquestion = ISurveyQuestionBL.GetSurvey_QuestionByExampaperID(list.Where(p => p.Type == 1).Select(p => p.PingId).FirstOrDefault());
            //    ViewBag.fenzuafterquestion = fenzuafterquestion;

            //    //加载第一个问题的答案
            //    var fenzufirsthtml = "";
            //    if (fenzuafterquestion.Count != 0)
            //    {
            //        fenzufirsthtml = FindAnswerString(courseid, fenzuafterquestion[0].ExampaperID, fenzuafterquestion[0].QuestionType, fenzuafterquestion[0].QuestionID);
            //    }

            //    ViewBag.fenzufirsthtml = fenzufirsthtml;

            //}




            return View();
        }

        public ActionResult Attendance(int courseid)
        {
            ViewBag.courseid = courseid;
            return View();
        }

        public ActionResult Calendar()
        {
            return View();
        }

        public ActionResult ClassPro(int courseid)
        {
            ViewBag.courseid = courseid;
            return View();
        }

        public ActionResult CourseList()
        {
            return View();
        }

        public ActionResult CourseMain(int courseid)
        {
            var Course = _newcourseBL.GetSingleCourse(courseid);
            ViewBag.Course = Course;

            var CourseResourse = _newcoursefiles.GetCourseResourceList(courseid);

            ViewBag.CourseResourse = CourseResourse;

            ViewBag.New_CourseRoomRule = _inew_courseorderbl.GetTeachertoLearnDetal(CurrentUser.UserId, " courseid="+courseid);


            ViewBag.id = courseid;
            ViewBag.UserID = CurrentUser.UserId;
            return View();
        }

        public ActionResult MyToTeacherComment()
        {
            return View();
        }

        public ActionResult MyToTeacherCommentList()
        {
            return View();
        }

        public ActionResult OnlineTest(int courseid)
        {
            ViewBag.Id = courseid;
            return View();
        }

        public ActionResult TeacherCourse(int courseid,int tp=0)
        {
            ViewBag.course = _newcourseBL.GetCourseByCourseRoomRule(courseid);
            ViewBag.tp = tp;
            return View();
        }

        public ActionResult TeacherCourseList()
        {
            return View();
        }

        public ActionResult TeacherToMyComment()
        {
            return View();
        }

        public ActionResult TeacherToMyCommentList()
        {
            return View();
        }

        #endregion

        #region 讲师端日历呈现
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
            string strwhere = string.Format(" Publishflag=1 AND  CourseFrom=1 AND Month='{0}' AND  CHARINDEX('{1}',OpenLevel)>0 ", _year.Trim() + "-" + _month.Trim(), CurrentUser.TrainGrade);
            //List<Co_Course> listCourse = _courseBL.GetCourseCommonList(out total, strwhere, 0, int.MaxValue);

            //List<New_CourseOrder> listCourse = _inew_courseorderbl.GetNew_CourseOrderListByTeacher(CurrentUser.UserId, " 1=1", " where 1=1", int.MaxValue, int.MaxValue, "ORDER BY New_Course.Id DESC"); ;
            List<New_CourseOrder> listCourse = _inew_courseorderbl.GetTeachertoLearnDetal(CurrentUser.UserId, " 1=1");
            
            //所查询的月的1号



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
                    MoringStr=morningstr,
                    AfterStr=afterstr,
                    Bg = (newdate.Day == DateTime.Now.Day && newdate.Month == DateTime.Now.Month) ? 3 : 1,
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
        public JsonResult GetCourseByDay(DateTime starttime, DateTime endtime)
        {
            var a = (0 / 100) * 2;
            //starttime = DateTime.Now;
            //endtime = starttime.AddDays(1);
            int total = 0;
       
            //var list = _inew_courseorderbl.GetNew_CourseOrderListByTeacher(CurrentUser.UserId, " StartTime>='" + starttime + "' and StartTime<='" + endtime + "'", " where 1=1", int.MaxValue, int.MaxValue, "ORDER BY New_Course.Id DESC");
            var list = _inew_courseorderbl.GetTeachertoLearnDetal(CurrentUser.UserId, " New_CourseRoomRule.StartTime>='" + starttime + "' and New_CourseRoomRule.StartTime<='" + endtime + "'");

            return Json(new
            {
                dataList = list
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 课前建议
        public JsonResult OnLoadCl_CourseAdvice(int id)
        {
            //List<New_CourseAdvice> clCourseAdvicesList = _inew_courseadvice.GetList(" New_CourseAdvice.CourseId=" + id + " and New_CourseRoomRule.TeacherId="+CurrentUser.UserId);
            List<New_CourseAdvice> clCourseAdvicesList = _inew_courseadvice.GetList(" New_CourseAdvice.CourseId=" + id );//+ " and New_CourseRoomRule.TeacherId=" + CurrentUser.UserId);
            
            string html = "";
            if (clCourseAdvicesList.Count > 0)
            { 
                foreach (var clCourseAdvice in clCourseAdvicesList)
                {
                    html += "<div id=" + clCourseAdvice.Id + " class='list'>";
                    html += "<div class='list-head'><span>" + clCourseAdvice.userRealname
                            + "</span><img src='" + Url.Content(clCourseAdvice.userPhotoUrlStr) + "' title='" + clCourseAdvice.userRealname + "' /></div>";
                    html += "   <div class='list-info'><i></i>";
                    html += "       <div class='list-type'><strong>课前建议：</strong>" + clCourseAdvice.AdviceTime + "";

                    if (clCourseAdvice.AnserContent == "")
                    {
                        html += "       <a class='fr btn bt-co p_Reply' onclick='FReply(this)'  type='1'>反馈</a>";
                    }
                    html += "</div>";
                    html += "       <div class='list-con'>" + clCourseAdvice.AdviceContent + "</div>";

                    if (clCourseAdvice.AnserContent != "")
                    {
                        html += "<div class='list-te'>";
                        html += "<p><strong class='c_col'>讲师<span>" + clCourseAdvice.TeacherName + "</span>的反馈内容：</strong>" + clCourseAdvice.AnserContent + "</p>";
                        html += "<p class='time'>时间:" + clCourseAdvice.AnserTime + "</p>";
                        html += "</div>";
                    }
                    else
                    {
                        html += "<div style='display:none'  id='Reply_div' class='pro-back'><table class='tab-Form'>";
                        html += "   <tr><td class='Tit'>请选择分享范围：</td><td><div class='sel'><input type='radio' name='tt' value='1' /><label>仅提出人</label><input type='radio' name='tt' value='2' /><label>全员</label></div></td></tr>";
                        html += "   <tr><td class='Tit'></td><td><input type=\"text\" id=\"txt_AnswerContent\" name=\"content\"  class='all80' maxlength='200'/></td></tr>";
                        html += "   <tr><td class='Tit'></td><td><input type=\"button\" value='提交'  class='btn btn-co' onclick='FAnswer(this)'/></td></tr>";
                        html += "</table></div>";
                    }

                    html += "   </div></div>";
                }
            }
            return Json(new
            {
                content = html

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 在线测试

        public JsonResult GetTeacherOnLineTest(int courseid, int pageSize = 10, int pageIndex = 1)
        {

            var litme = 0;
            //var courseorder = null;// CourseOrderBL.GetTeacherOnLineTest(out litme, courseid, CurrentUser.UserId, (pageIndex - 1) * pageSize + 1, pageSize);

            var coursepaper = _INew_CoursePaperBL.GetSingleCoursePaper(courseid);

            if (coursepaper.PaperId != 0)
            { 
                var courseorder = _inew_courseorderbl.GetTeacherOnLineTest(out litme, courseid, CurrentUser.UserId, (pageIndex - 1) * pageSize + 1, pageSize).Distinct();

                return Json(new
                {
                    result = 1,
                    dataList = courseorder,
                    recordCount = litme
                }, JsonRequestBehavior.AllowGet);
            }

             return Json(new
                {
                    result = 0,
                    dataList = "",
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 课后评估
        //public JsonResult GetPingList(int pingid,int courseid)
        //{

        //    string html = "";
        //    string firsthtml = "";
        //    if (pingid != 0)
        //    { 
        //        var afterquestion = ISurveyQuestionBL.GetSurvey_QuestionByExampaperID(pingid);
        //        int i = 1;
        //        if (afterquestion != null)
        //        { 
        //            foreach (var item in afterquestion)
        //            {
        //              //html += "";
        //              html += "  <div class='question_div' onclick='fliclick(this)' QuestionId='" + item.QuestionID + "' ExampaperID='" + item.ExampaperID + "' questiontype='" + item.QuestionType + "'>";
        //              html += "                  <span>"+ i++ +" .</span>";
        //              html += "                  <strong class='ovh' title='" + item.QuestionContent + "'>" + item.QuestionContent + "</strong>";
        //              html += "                  <i></i>";
        //              html += "              </div>";
        //            }
        //            firsthtml = FindAnswerString(courseid, afterquestion[0].ExampaperID, afterquestion[0].QuestionType, afterquestion[0].QuestionID);

        //        }
        //    }

        //    return Json(new
        //    {
        //        html = html,
        //        firsthtml = firsthtml

        //    }, JsonRequestBehavior.AllowGet);
        //}



        public string FindAnswerString(int courseid, int ExampaperID, int questiontype, int QuestionID,int sub)
        {
            var list = ISurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + courseid + " and ExampaperID=" + ExampaperID + " and QuestionID=" + QuestionID + " and Survey_ReplyAnswer.Status=1 and CourseRoomRuleId="+sub);
            string html = "";
            int i = 1;
            foreach (var surveyReplyAnswer in list)
            {
                if (questiontype == 2)
                {
                    html += "<p>" + i + ".<strong class='c_col'>回答：</strong>" + surveyReplyAnswer.SubjectiveAnswer + "<span>（[" + surveyReplyAnswer.DeptName + "] " + surveyReplyAnswer.Realname + "）</span>" + "</p>";
                }
                else
                {
                    html += "<p>" + i + ".<strong>评分：" + surveyReplyAnswer.SubjectiveAnswer + "</strong><strong class='c_col'>理由：</strong>" + surveyReplyAnswer.QuestionReply + "<span>（[" + surveyReplyAnswer.DeptName + "] " + surveyReplyAnswer.Realname + "）</span>" + "</p>";
                }
                i++;
            }

            return html;

        }

        public JsonResult FindAnswer(int courseid, int ExampaperID, int questiontype, int QuestionID,int sub)
        {
            var list = ISurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + courseid + " and ExampaperID=" + ExampaperID + " and QuestionID=" + QuestionID + " and Survey_ReplyAnswer.Status=1 and CourseRoomRuleId=" + sub);
            string html = "";
            int i = 1;
            foreach (var surveyReplyAnswer in list)
            {
                if (questiontype == 2)
                {
                    html += "<p>" + i + ".<strong class='c_col'>回答：</strong>" + surveyReplyAnswer.SubjectiveAnswer + "<span>（[" + surveyReplyAnswer.DeptName + "] " + surveyReplyAnswer.Realname + "）</span>" + "</p>";
                }
                else
                {
                    html += "<p>" + i + ".<strong>评分：" + surveyReplyAnswer.SubjectiveAnswer + "</strong><strong class='c_col'>理由：</strong>" + surveyReplyAnswer.QuestionReply + "<span>（[" + surveyReplyAnswer.DeptName + "] " + surveyReplyAnswer.Realname + "）</span>" + "</p>";
                }
                i++;
            }

            return Json(new
            {

                List = html

            }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 考勤
        public JsonResult GetNew_Attend(int courseid, int pageSize = 10, int pageIndex = 1)
        {

            int lit=0;
            var list = inew_attendce.GetNewAttendUserList(courseid, out lit, pageIndex, pageSize, "asc", " ncrr.TeacherId="+CurrentUser.UserId);
            //var list = inew_attendce.GetNewAttendList(courseid, out lit, pageIndex, pageSize, "asc", " ncrr.TeacherId=" + CurrentUser.UserId);
            int n = 0;
            var itemArray = new object[list.Count()];

            foreach (var item in list)
            {
                var temp = new
                {
                    CourseType=item.Type==0?"集中授课":"分组带教",
                    CourseTime = item.CoTimeStr,
                    UserNum=item.NumberID,
                    UserRealName=item.Realname,
                    AttendceStartTime=item.StartTimeStr,
                    AttendceEndTime=item.EndTimeStr,
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

        #region 讲师列表
        public JsonResult GetTeacherList(string coursename, string room, string starttime, string endtime,string coursestatus, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " 1=1";
            string strsql = " where 1=1";
            int limit = 0;
            if (!string.IsNullOrEmpty(coursename))
            {
                sql += " and New_Course.CourseName like '%" + coursename + "%'";
            }
            if (!string.IsNullOrEmpty(room))
            {
                strsql+=" and roomname like '%"+room+"%'";
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
            List<New_CourseOrder> list = null;
            //var list = _inew_courseorderbl.GetNew_CourseOrderListByTeacher(out limit, CurrentUser.UserId, sql, (pageIndex - 1) * pageSize + 1, pageSize, "ORDER BY New_Course.Id DESC");
            
            
            list = _inew_courseorderbl.GetNew_CourseOrderListByTeacher(CurrentUser.UserId, sql,strsql, int.MaxValue, pageSize, "ORDER BY New_Course.Id DESC");
            
          

            limit = list.Count();

            list = list.Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();



            return Json(new
            {
                dataList = list,
                recordCount = limit
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 讲师上传资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult SubmitAddCourseResource(New_CourseFiles model)
        {
            model.Flag = CurrentUser.UserId;
            model.CreateDate = DateTime.Now;
            _newcoursefiles.AddCourseFiles(model);
            return Json(new
            {
                result = model.Id > 0 ? "1" : "0"
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FDeleteCourseResource(int id)
        {
            return Json(new
            {
                result = _newcoursefiles.DeleteCourseFiles(id.ToString()) ? "1" : "0"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
