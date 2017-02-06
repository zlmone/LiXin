using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.DepCourseManage;
using LiXinModels.DepCourseLearn;
using LiXinInterface.DepCourseLearn;
using LiXinInterface.DeptSurvey;
using LiXinModels.DepCourseManage;

namespace LiXinControllers
{
    public class DepTeacherCourseController : BaseController
    {
        private IDep_CourseOrder CourseOrderBL;
        private IDep_Course courseBL;
        private IDep_CourseResource CourseResourceBL;
        private IDepCourseAdvice CourseAdviceBL;
        private IDeptSurveyExampaper surveyBL;
        private IDepSurveyReplyAnswer ReplyAnswerBL;
        public DepTeacherCourseController(IDep_CourseOrder _CourseOrderBL, IDep_Course _courseBL, IDep_CourseResource _CourseResourceBL, IDepCourseAdvice _CourseAdviceBL, IDeptSurveyExampaper _surveyBL, IDepSurveyReplyAnswer _ReplyAnswerBL)
        {
            CourseOrderBL = _CourseOrderBL;
            courseBL = _courseBL;
            CourseResourceBL = _CourseResourceBL;
            CourseAdviceBL = _CourseAdviceBL;
            surveyBL = _surveyBL;
            ReplyAnswerBL = _ReplyAnswerBL;
        }
        #region 呈现
        /// <summary>
        /// 我的授课课程
        /// </summary>
        /// <returns></returns>
        public ActionResult TeacherCourseList()
        {
            ViewBag.trainGrade = AllTrainGrade;
            return View();
        }

        /// <summary>
        /// 课程详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CourseMain(int id)
        {
            var Course = courseBL.GetCo_Course(id);
            ViewBag.Course = Course;

            var CourseResourse = CourseResourceBL.GetList(id);

            ViewBag.CourseResourse = CourseResourse;

            ViewBag.id = id;


            return View();
        }

        /// <summary>
        /// 课程详情公共页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="showFlag"></param>
        /// <returns></returns>
        public ActionResult TeacherCourse(int id, int showFlag = 0)
        {
            var Course = courseBL.GetCo_Course(id);
            //还需在获取讲师信息

            ViewBag.Course = Course;

            var CourseOrder = CourseOrderBL.Get(Course.Id);
            ViewBag.CourseOrder = CourseOrder;
            ViewBag.showFlag = showFlag;

            return View();
        }

        /// <summary>
        /// 课前建议
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ClassPro(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 考勤详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Attendance(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 课后评估
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AfterCourseNew(int id)
        {
            var course = courseBL.GetCo_Course(id);

            //if (course.SurveyPaperId != "" && course.SurveyPaperId !=null)
            if (!string.IsNullOrEmpty(course.SurveyPaperId))
            {
                if (course.SurveyPaperId.Split(';')[0] != "")
                {
                    var afterquestion = surveyBL.GetSurvey_QuestionByExampaperID(Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]));
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
                    var afterteacherquestion = surveyBL.GetSurvey_QuestionByExampaperID(Convert.ToInt32(course.SurveyPaperId.Split(';')[1].Split(',')[1]));
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

        /// <summary>
        /// 在线测试
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineTest(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UploadResource(int id)
        {
            ViewBag.id = id;
            return View();
        }
        #endregion


        #region 绑定讲师课程列表

        public JsonResult FTeacherCourseList(string CourseName, int pageSize = 10, int pageIndex = 1, string openLevel = "", int status = -1, int sort = -1,
            string start = "", string end = "", string jsRenderSortField=" StartTime desc")
        {
            int number = 0;

            #region 查询条件
            string sql = " dc.teacher=" + CurrentUser.UserId + "  and CourseFrom=2";
            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and dc.CourseName like '%" + CourseName.ReplaceSql() + "%'";
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

            var teacherlist = CourseOrderBL.GetCourseSubscribeList(out number, pageIndex, pageSize, sql, jsRenderSortField);

            return Json(new
            {
                result = 1,
                dataList = teacherlist,
                recordCount = number
            }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 我的授课课程详情
        /// <summary>
        /// 我的课程(讲师)详细信息下学员列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTeacherCourseUserList(int courseid, int pageIndex = 1, int pageSize = 10)
        {
            int Count = 0;
            //  var list = CourseOrderBL.GetTeacherCourseUserList(out Count, courseid, (pageIndex - 1) * pageSize + 1, pageSize);
            var list = CourseOrderBL.GetCourseUserListByTeacher(out Count, courseid, pageIndex, pageSize);
            return Json(new
            {
                recordCount = Count,
                dataList = list

            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 课前建议
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult OnLoadCl_CourseAdvice(int id)
        {
            List<Dep_CourseAdvice> clCourseAdvicesList = CourseAdviceBL.GetList(" Dep_CourseAdvice.CourseId=" + id);
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
            var model = new Dep_CourseAdvice
                            {
                                Id = id,
                                CourseId = CourseId,
                                AnserContent = AnserContent,
                                AnserTime = DateTime.Now,
                                VisibleFlag = VisibleFlag,
                                AnserUserId = CurrentUser.UserId
                            };

            if (!CourseAdviceBL.ReplyCl_CourseAdvice(model))
            {
                return Json(new
                                {
                                    result = 0,
                                    content = "反馈失败"
                                }, JsonRequestBehavior.AllowGet);
            }
            var html = "";
            html += "<div class='list-te'>";
            html += "<p><strong class='c_col'>讲师<span>" + CurrentUser.Realname + "</span>反馈内容:</strong>" +
                    model.AnserContent + "</p>";
            html += "<p class='time'>时间:" + model.AnserTime + "</p>";
            html += "</div>";

            return Json(new
                            {
                                result = 0,
                                innerhtml = html,
                                content = "反馈成功"
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

        /// <summary>
        /// 在线测试
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetTeacherOnLineTest(int courseid, int pageSize = 10, int pageIndex = 1)
        {

            var litme = 0;
            var courseorder = CourseOrderBL.GetCourseTeacherOnLineTest(out litme,courseid, pageIndex, pageSize);
            litme = courseorder.Count();
            courseorder = courseorder.Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();


            return Json(new
            {
                result = 1,
                dataList = courseorder,
                recordCount = litme
            }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 讲师上传资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult SubmitAddCourseResource(Dep_CourseResource model)
        {
            model.Flag = 1;
            return Json(new
            {
                result = CourseResourceBL.AddCourseResource(model) ? "1" : "0"
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 我要删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult FDeleteCourseResource(int id)
        {
            return Json(new
            {
                result = CourseResourceBL.DeleteCourseResource(id) ? "1" : "0"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 点击问题把答案绑定出来
        /// <summary>
        /// 点击问题把答案绑定出来
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="ExampaperID"></param>
        /// <param name="questiontype"></param>
        /// <param name="QuestionID"></param>
        /// <returns></returns>
        public JsonResult FindAnswer(int courseid, int ExampaperID, int questiontype, int QuestionID)
        {
            var list = ReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + courseid + " and ExampaperID=" + ExampaperID + " and QuestionID=" + QuestionID + " and Dep_Survey_ReplyAnswer.Status=1");
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

        /// <summary>
        /// 点击问题把答案绑定出来
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="ExampaperID"></param>
        /// <param name="questiontype"></param>
        /// <param name="QuestionID"></param>
        /// <returns></returns>
        public string FindAnswerString(int courseid, int ExampaperID, int questiontype, int QuestionID)
        {
            var list = ReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + courseid + " and ExampaperID=" + ExampaperID + " and QuestionID=" + QuestionID + " and Dep_Survey_ReplyAnswer.Status=1");
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
    }
}
