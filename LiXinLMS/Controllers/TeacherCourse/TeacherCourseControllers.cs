using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinInterface.Examination;
using LiXinInterface.Survey;
using LiXinInterface.User;
using LiXinModels.CourseLearn;
using LiXinModels.CourseManage;
using LiXinModels.Survey;

namespace LiXinControllers
{
    public partial class TeacherCourseController : BaseController
    {
        protected ICourseOrder CourseOrderBL;
        protected ICo_Course CoCourseBL;
        protected ICl_Attendce ClAttendceBL;
        protected ICl_CourseAdvice ClCourseAdviceBL;
        protected ISurveyQuestion ISurveyQuestionBL;
        protected ISurveyQuestionAnswer ISurveyQuestionAnswerBL;
        protected ISurveyReplyAnswer ISurveyReplyAnswerBL;
        protected ICo_CourseResource ICoCourseResourceBL;
        protected IUser IuserBL;
        protected IExamination IExaminationBL;


        public TeacherCourseController(ICourseOrder _CourseOrderBL, ICo_Course _CoCourseBL, ICl_Attendce _ClAttendceBL, ICl_CourseAdvice _ClCourseAdviceBL, ISurveyQuestion _ISurveyQuestionBL, ISurveyQuestionAnswer _ISurveyQuestionAnswerBL, ISurveyReplyAnswer _ISurveyReplyAnswerBL, ICo_CourseResource _ICoCourseResourceBL, IUser _IuserBL, IExamination _IExaminationBL)
        {
            CourseOrderBL = _CourseOrderBL;
            CoCourseBL = _CoCourseBL;
            ClAttendceBL = _ClAttendceBL;
            ClCourseAdviceBL = _ClCourseAdviceBL;
            ISurveyQuestionBL = _ISurveyQuestionBL;
            ISurveyQuestionAnswerBL = _ISurveyQuestionAnswerBL;
            ISurveyReplyAnswerBL = _ISurveyReplyAnswerBL;
            ICoCourseResourceBL = _ICoCourseResourceBL;
            IuserBL = _IuserBL;
            IExaminationBL = _IExaminationBL;
        }

        #region 展示页面

        public ActionResult TeacherCourseList()
        {
            ViewBag.trainGrade = AllTrainGrade;
            return View();
        }

        public ActionResult TeacherCourse(int id, int showFlag = 0)
        {
            var Course = CoCourseBL.GetCo_Course(id);
            //还需在获取讲师信息

            ViewBag.Course = Course;

            var CourseOrder = CourseOrderBL.Get(Course.Id);
            ViewBag.CourseOrder = CourseOrder;
            ViewBag.showFlag = showFlag;

            return View();
        }

        public ActionResult CourseMain(int id)
        {
            var Course = CoCourseBL.GetCo_Course(id);
            ViewBag.Course = Course;

            var CourseResourse = ICoCourseResourceBL.GetList(id);

            ViewBag.CourseResourse = CourseResourse;

            ViewBag.id = id;


            return View();
        }

        public ActionResult AfterCourse(int SurveyPaperId, int id)
        {
            var questionlist = ISurveyQuestionBL.GetSurvey_QuestionByExampaperID(SurveyPaperId);
            ViewBag.id = id;
            ViewBag.SurveyPaperId = SurveyPaperId;
            ViewBag.questionlist = questionlist;

            return View();
        }

        public ActionResult AfterCourseNew(int id)
        {
            var course = CoCourseBL.GetCo_Course(id);

            //if (course.SurveyPaperId != "" && course.SurveyPaperId !=null)
            if (!string.IsNullOrEmpty(course.SurveyPaperId))
            {
                if (course.SurveyPaperId.Split(';')[0] != "")
                {
                    var afterquestion = ISurveyQuestionBL.GetSurvey_QuestionByExampaperID(Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]));
                    //var afterquestion = ISurveyQuestionBL.GetSurvey_QuestionByUserID(id, Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]), CurrentUser.UserId);
                    ViewBag.afterquestion = afterquestion;

                    //加载第一个问题的答案
                    var firsthtml = "";
                    if (afterquestion.Count != 0)
                    {
                        firsthtml = FindAnswerString(id, afterquestion[0].ExampaperID, afterquestion[0].QuestionType, afterquestion[0].QuestionID);
                    }

                    ViewBag.firsthtml = firsthtml;

                }
                else
                {
                    ViewBag.afterquestion = null;
                    ViewBag.firsthtml = null;
                }
                if (course.SurveyPaperId.Split(';')[1] != "")
                {
                    var afterteacherquestion = ISurveyQuestionBL.GetSurvey_QuestionByExampaperID(Convert.ToInt32(course.SurveyPaperId.Split(';')[1].Split(',')[1]));
                    //var afterteacherquestion = ISurveyQuestionBL.GetSurvey_QuestionByUserID(id, Convert.ToInt32(course.SurveyPaperId.Split(';')[1].Split(',')[1]), CurrentUser.UserId);
                    ViewBag.afterteacherquestion = afterteacherquestion;
                    var firstafterteacherhtml = "";
                    if (afterteacherquestion.Count != 0)
                    {
                         firstafterteacherhtml = FindAnswerString(id, afterteacherquestion[0].ExampaperID,
                                                                     afterteacherquestion[0].QuestionType,
                                                                     afterteacherquestion[0].QuestionID);
                    }
                    ViewBag.firstafterteacherhtml = firstafterteacherhtml;

                }
                else
                {
                    ViewBag.afterteacherquestion = null;
                }
            }

            ViewBag.id = id;
            ViewBag.SurveyPaperId = course.SurveyPaperId;

            return View();
        }

        public ActionResult ALLCourseList()
        {
            ViewBag.trainGrade = AllTrainGrade;
            return View();
        }

        public ActionResult Attendance(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult ClassPro(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 在线测试
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineTest(int id)
        {
            // var Examlist = IExaminationBL.GetExamSendStudentListWithCourseId(id);
            //ViewBag.Examlist = Examlist;

            //var courseOrder = CourseOrderBL.GetTeacherCourseUserList(CourseId);
            //var courseOrder = CourseOrderBL.GetTeacherOnLineTest(id);
            //ViewBag.courseOrder = courseOrder;
            ViewBag.Id = id;
            return View();
        }

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
        #endregion

        public JsonResult GetTeacherOnLineTest(int courseid, int pageSize = 10, int pageIndex = 1)
        {

            var litme = 0;
            var courseorder = CourseOrderBL.GetTeacherOnLineTest(out litme, courseid, CurrentUser.UserId,
                                                                  (pageIndex - 1) * pageSize + 1, pageSize);


            return Json(new
            {
                result = 1,
                dataList = courseorder,
                recordCount = litme
            }, JsonRequestBehavior.AllowGet);

        }

        #region 点击问题把答案绑定出来

        public JsonResult FindAnswer(int courseid, int ExampaperID, int questiontype, int QuestionID)
        {
            var list = ISurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + courseid + " and ExampaperID=" + ExampaperID + " and QuestionID=" + QuestionID + " and Survey_ReplyAnswer.Status=1");
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

        public string FindAnswerString(int courseid, int ExampaperID, int questiontype, int QuestionID)
        {
            var list = ISurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + courseid + " and ExampaperID=" + ExampaperID + " and QuestionID=" + QuestionID + " and Survey_ReplyAnswer.Status=1");
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
        #endregion



        #region 点击题目绑定出答案和选择人数比例

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="SurveyPaperId"></param>
        /// <param name="AnswerId">该回答的ID</param>
        /// <returns></returns>
        public JsonResult FbindSurvey_QuestionAnswer(int CourseId, int SurveyPaperId, int QuestionId)
        {
            //获取题目列表
            var questionlist = ISurveyQuestionAnswerBL.GetSurvey_QuestionAnswerByQuestionID(QuestionId);

            //获取该题目的点击数
            int allcount = ISurveyReplyAnswerBL.GetSurvey_ReplyAnswerCountBYbjectID(CourseId, SurveyPaperId, QuestionId);

            string html = "";
            if (questionlist.Count != 0)
            {
                foreach (Survey_QuestionAnswer c in questionlist)
                {
                    //获取单个题目的点击数
                    int count = ISurveyReplyAnswerBL.GetSingleSurvey_ReplyAnswerCount(CourseId, SurveyPaperId, c.QuestionID, c.AnswerId, 0);


                    html += "<div><span>" + c.ShowOrder + "</span><span>" + c.AnswerContent + "</span><span>" + (int)(((double)count / allcount) * 100) + "</span></div>";//"." + c.AnswerContent + 
                }
            }
            else
            {
                var zhuguanti = ISurveyReplyAnswerBL.GetList(" ObjectID=" + CourseId + "and ExampaperID=" + SurveyPaperId + " and QuestionID=" + QuestionId);

                foreach (Survey_ReplyAnswer surveyReplyAnswer in zhuguanti)
                {
                    html += "<div><span>" + IuserBL.Get(surveyReplyAnswer.UserID).Realname + "回答:</span><span>" + surveyReplyAnswer.SubjectiveAnswer + "</span></div>";
                }
            }
            return Json(new
            {
                content = html
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 绑定讲师课程列表

        public JsonResult FTeacherCourseList(string CourseName, int pageSize = 10, int pageIndex = 1, string openLevel = "", int status = -1, int sort = -1,
            string start = "", string end = "")
        {
            int number = 0;

            #region 查询条件
            string sql = " and CourseFrom=2";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and Co_Course.CourseName like '%" + CourseName.ReplaceSql() + "%'";
            }
            if (sort != -1)
            {
                sql += " and (SELECT  count(*)  FROM dbo.F_SplitIDs(sort) WHERE ID=" + sort + ")>0";
            }
            if (!string.IsNullOrEmpty(openLevel))
            {
                sql += @" and (SELECT count(*) FROM dbo.F_SplitIDs(OpenLevel) 
WHERE ID IN (SELECT ID FROM dbo.F_SplitIDs('" + openLevel + "')))>0";
            }

            if (!string.IsNullOrEmpty(start))
            {
                sql += string.Format(" AND StartTime>='{0}'", start);
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql += string.Format(" AND EndTime<='{0}'", Convert.ToDateTime(end).AddDays(1).AddSeconds(-1));
            }


            switch (status)
            {
                case 1: //预定中
                    sql += " and getdate()< StartTime";
                    break;
                case 2://进行中
                    sql += " and getdate()> StartTime and  getdate() < EndTime";
                    break;
                case 3://已结束
                    sql += " and  getdate() > EndTime";
                    break;
                default:
                    break;
            }
            #endregion

            var teacherlist = CourseOrderBL.GetListByTeacher(out number, CurrentUser.UserId, sql, (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                result = 1,
                dataList = teacherlist,
                recordCount = number
            }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 绑定所有讲师课程列表

        public JsonResult FALLTeacherCourseList(string CourseName, int pageSize = 10, int pageIndex = 1, string openLevel = "", int status = -1, int sort = -1,
            string start = "", string end = "")
        {
            int number = 0;

            #region 查询条件
            string sql = " and CourseFrom=2";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and Co_Course.CourseName like '%" + CourseName.ReplaceSql() + "%'";
            }
            if (sort != -1)
            {
                sql += " and (SELECT  count(*)  FROM dbo.F_SplitIDs(sort) WHERE ID=" + sort + ")>0";
            }
            if (!string.IsNullOrEmpty(openLevel))
            {
                sql += @" and (SELECT count(*) FROM dbo.F_SplitIDs(OpenLevel) 
WHERE ID IN (SELECT ID FROM dbo.F_SplitIDs('" + openLevel + "')))>0";
            }

            if (!string.IsNullOrEmpty(start))
            {
                sql += string.Format(" AND StartTime>='{0}'", start);
            }
            if (!string.IsNullOrEmpty(end))
            {
                sql += string.Format(" AND EndTime<='{0}'", Convert.ToDateTime(end).AddDays(1).AddSeconds(-1));
            }


            switch (status)
            {
                case 1: //预定中
                    sql += " and getdate()< StartTime";
                    break;
                case 2://进行中
                    sql += " and getdate()> StartTime and  getdate() < EndTime";
                    break;
                case 3://已结束
                    sql += " and  getdate() > EndTime";
                    break;
                default:
                    break;
            }
            #endregion

            var teacherlist = CourseOrderBL.GetListByAllTeacher(out number,sql, (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                result = 1,
                dataList = teacherlist,
                recordCount = number
            }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region  课前建议
        public JsonResult OnLoadCl_CourseAdvice(int id)
        {
            List<Cl_CourseAdvice> clCourseAdvicesList = ClCourseAdviceBL.GetList(" Cl_CourseAdvice.CourseId=" + id);
            string html = "";
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

            return Json(new
            {
                content = html

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

            if (ClCourseAdviceBL.ReplyCl_CourseAdvice(model))
            {
                string html = "";
                html += "<div>";
                html += "<p><strong class='c_col'>讲师:<span>" + CurrentUser.Realname + "</span>反馈内容:</strong>" + model.AnserContent + "</p>";
                html += "<p>时间:" + model.AnserTime + "</p>";
                html += "</div>";

                return Json(new
                {
                    result = 0,
                    innerhtml = html,
                    content = "反馈成功"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    result = 0,
                    content = "反馈失败"
                }, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult FFindAnswer(int Courseid, int ExampaperID, int QuestionID)
        {
            var list = ISurveyReplyAnswerBL.GetList(" ObjectID=" + Courseid + " and ExampaperID=" + ExampaperID + " and QuestionID=" + QuestionID);

            string html = "";

            foreach (var mm in list)
            {
                html += mm.SubjectiveAnswer + mm.ObjectiveAnswer + ";";
            }

            html = html.Substring(0, html.LastIndexOf(';'));
            return Json(new
            {
                result = 0,
                content = "html"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion



        public JsonResult FBindReplyAnswer(int userid, int CourseId, int SurveyPaperId)
        {
            var list =
                ISurveyReplyAnswerBL.GetList(" UserID=" + userid + " and ObjectID=" + CourseId + " and ExampaperID=" +
                                             SurveyPaperId);
            string html = "";
            foreach (var surveyReplyAnswer in list)
            {

                html += surveyReplyAnswer.QuestionID + "-" + surveyReplyAnswer.SubjectiveAnswer + "-" +
                        surveyReplyAnswer.ObjectiveAnswer + "|";
            }

            return Json(new
            {

                List = html.Substring(0, html.LastIndexOf('|'))

            }, JsonRequestBehavior.AllowGet);
        }


        #region

        /// <summary>
        /// 绑定这门课程下的学员
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public JsonResult FTeacherList(int courseid, int pageIndex = 1, int pageSize = 20)
        {
            int Count = 0;
            var teacherlist = CourseOrderBL.GetTeacherCourse(out Count, courseid, (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                result = 1,
                dataList = teacherlist,
                recordCount = Count

            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 我的课程(讲师) 下考勤学员列表
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult FUserrAttendceList(int courseId, int pageIndex = 1, int pageSize = 20)
        {
            int Count = 0;
            //var list = ClAttendceBL.GetListByTeacher(out Count, courseId, pageIndex, pageSize);
            var teacherlist = CourseOrderBL.GetTeacherCourseAttendceList(out Count, courseId, (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                dataList = teacherlist,
                recordCount = Count

            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        /// <summary>
        /// 我的课程(讲师)详细信息下学员列表
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetTeacherCourseUserList(int courseid, int pageIndex = 1, int pageSize = 10)
        {
            int Count = 0;
            var list = CourseOrderBL.GetTeacherCourseUserList(out Count, courseid, (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                recordCount = Count,
                dataList = list

            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 讲师上传资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult SubmitAddCourseResource(Co_CourseResource model)
        {
            model.Flag = 1;
            return Json(new
            {
                result = ICoCourseResourceBL.AddCourseResource(model) ? "1" : "0"
            }, JsonRequestBehavior.AllowGet);
        }


        #region 我要删除

        public JsonResult FDeleteCourseResource(int id)
        {
            return Json(new
            {
                result = ICoCourseResourceBL.DeleteCourseResource(id) ? "1" : "0"
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
