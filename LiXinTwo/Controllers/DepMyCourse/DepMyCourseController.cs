using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinInterface.DepCourseLearn;
using LiXinInterface.DepCourseManage;
using LiXinInterface.DepLeaveApproval;
using LiXinInterface.DeptSurvey;
using LiXinInterface.SystemManage;
using LiXinModels.DepCourseLearn;
using LiXinCommon;
using System.Text.RegularExpressions;
using LiXinModels.Survey;
using LiXinModels.DepCourseManage;
using LiXinInterface.Examination;
using LiXinModels.User;
using LiXinInterface.User;
using LiXinInterface;
using LiXinInterface.CourseLearn;
using LiXinModels;


namespace LiXinControllers.DeptMyCourse
{
    public partial class DepMyCourseController : BaseController
    {
        protected IDep_Course DepCourseBL;
        protected IDepCourseAdvice DepAdvice;
        protected IDep_CourseOrder Dep_CourseOrder;
        protected IDeptSurveyExampaper ISurveyExampaperBL;
        protected IDepSurveyReplyAnswer ISurveyReplyAnswerBL;
        protected IDep_Attendce ClAttendceBL;
        protected IDep_CoursePaper IDep_CoursePaperBL;
        protected IDep_CourseResource IDep_CourseResourseBL;
        protected IExamination IExaminationrBL;
        protected IExampaper IExampaperBL;
        protected ISys_Leader leaderBL;
        protected IDepLeaveApproval Approval;
        protected IUser userBL;
        protected ISys_TrainGrade sys_TrainBL;
        protected ICourseOrder courseOrderBL;
        protected IDep_CpaLearnStatus depCpaLearn;

        public DepMyCourseController(IDep_Course _DepCourseBL, IDepCourseAdvice _DepAdvice, IDep_CourseOrder _Dep_CourseOrder,
            IDeptSurveyExampaper _ISurveyExampaperBL, IDepSurveyReplyAnswer _ISurveyReplyAnswerBL, IDep_CoursePaper _IDep_CoursePaperBL,
            IDep_Attendce _ClAttendceBL, IExamination _IExaminationrBL, IExampaper _IExampaperBL, IDep_CourseResource _IDep_CourseResourseBL, ISys_Leader _leaderBL, IDepLeaveApproval approval, IUser _userBL, ISys_TrainGrade _sys_TrainBL, ICourseOrder _courseOrderBL, IDep_CpaLearnStatus _depCpaLearn)
        {
            DepCourseBL = _DepCourseBL;
            DepAdvice = _DepAdvice;
            Dep_CourseOrder = _Dep_CourseOrder;
            ISurveyExampaperBL = _ISurveyExampaperBL;
            ISurveyReplyAnswerBL = _ISurveyReplyAnswerBL;
            ClAttendceBL = _ClAttendceBL;
            IDep_CoursePaperBL = _IDep_CoursePaperBL;
            IExaminationrBL = _IExaminationrBL;
            IExampaperBL = _IExampaperBL;
            IDep_CourseResourseBL = _IDep_CourseResourseBL;
            leaderBL = _leaderBL;
            Approval = approval;
            userBL = _userBL;
            sys_TrainBL = _sys_TrainBL;
            courseOrderBL = _courseOrderBL;
            depCpaLearn = _depCpaLearn;
        }

        #region 页面呈现
        public ActionResult MyCourseList()
        {
            ViewBag.DepOrBranch = CurrentUser.SubordinateSubstation.Contains("上海");
            return View();
        }

        public ActionResult MyCourse(int courseid, int tp = 0)
        {
            //var Course = DepCourseBL.GetCo_Course(courseid, CurrentUser.UserId);
            var Course = Dep_CourseOrder.GetCourseById(courseid, CurrentUser.UserId);

            var courseorder = Dep_CourseOrder.GetCo_CourseMainPaperByCourseIdAndUserId(courseid, CurrentUser.UserId);
            ViewBag.courseorder = courseorder;

            ViewBag.tp = tp;
            ViewBag.Course = Course;

            return View();
        }

        public ActionResult Attendance(int courseid)
        {
            var Cl_Attendce = ClAttendceBL.GetDepAttendce(courseid, CurrentUser.UserId);

            //var Cl_Attendce = ClAttendceBL.GetCl_Attendce(courseid, CurrentUser.UserId);
            var cocurse = DepCourseBL.GetCo_Course(courseid);
            if (Cl_Attendce != null)
            {
                ViewBag.Cl_Attendce = Cl_Attendce;
                ViewBag.cocurse = cocurse;
            }
            else
            {
                ViewBag.Cl_Attendce = null;
            }
            ViewBag.Id = courseid;
            return View();
        }

        public ActionResult CourseMain(int courseid)
        {
            var Course = DepCourseBL.GetCo_Course(courseid, CurrentUser.UserId);
            ViewBag.Course = Course;
            var CourseResourse = IDep_CourseResourseBL.GetList(courseid);
            ViewBag.CourseResourse = CourseResourse;
            return View();
        }

        public ActionResult ClassPro(int courseid)
        {
            ViewBag.Id = courseid;
            return View();
        }

        public ActionResult OnlineTest(int courseid)
        {

            Dep_Coursepaper CoCoursePaper = IDep_CoursePaperBL.GetCo_CourseMainPaper(courseid);
            var course = DepCourseBL.GetCo_Course(courseid);

            if (CoCoursePaper != null && course.IsTest == 1)
            {
                var exap = IExaminationrBL.GetExamination(CoCoursePaper.PaperId);
                ViewBag.exap = exap;
                // var examsendstudent = IExaminationrBL.GetExamSendStudent(exap.PaperID);
                var examsendstudent = IExaminationrBL.GetSingletbExamSendStudentByCourseIdAndUserId(courseid,
                                                                                                    CurrentUser.UserId, 5);
                var exampapger = IExampaperBL.GetExampaper(CoCoursePaper.PaperId);
                ViewBag.exampapger = exampapger;
                ViewBag.examsendstudent = examsendstudent;
            }
            ViewBag.Co_CoursePaper = CoCoursePaper;
            ViewBag.courseid = courseid;
            ViewBag.course = course;
            return View();
        }



        #endregion

        #region 刘天琛用
        /// <summary>
        /// 添加课前建议
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="AdviceContent"></param>
        /// <returns></returns>
        public JsonResult SubmitCl_CourseAdvice(int CourseId, string AdviceContent)
        {
            var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 29);
            var course = DepCourseBL.GetCo_CourseAllList(CourseId);

            DateTime datetime = DateTime.Now;
            DateTime starttime = course.StartTime.AddHours(-Convert.ToDouble(course.PreAdviceConfigTime));

            if (datetime > starttime)
            {
                return Json(
                    new { result = 1, content = "课前开放" + course.PreAdviceConfigTime + "小时，允许提建议" },
                    JsonRequestBehavior.AllowGet);
            }

            Dep_CourseAdvice model = new Dep_CourseAdvice();
            model.CourseId = CourseId;
            model.AdviceContent = AdviceContent;
            model.AdviceTime = DateTime.Now;
            model.UserId = CurrentUser.UserId;
            model.VisibleFlag = 0;  //0：都不可见；1：仅提出人；2：全部
            model.IsDelete = 0;
            DepAdvice.SubmitCl_CourseAdvice(model);
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

        public JsonResult OnLoadCl_CourseAdvice(int id)
        {
            int leavestatus = GetUserForCourseStatus(id) ? 1 : 0;
            string html = "";

            string otherhtml = "";
            if (leavestatus == 1)
            {
                List<Dep_CourseAdvice> clCourseAdvicesList = DepAdvice.GetList("  Dep_CourseAdvice.CourseId=" + id);

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

        public bool GetUserForCourseStatus(int CourseId)
        {
            var model = Dep_CourseOrder.GetCourseById(CourseId, CurrentUser.UserId);
            if (model.MyStatus == 6 || model.MyStatus == 2)
                return false;
            return true;
        }

        /// <summary>
        /// 新的课后页面方法 只有星级题目 原AfterCourseList暂时不用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="backurl"></param>
        /// <returns></returns>
        public ActionResult AfterCourseNewList(int id, string backurl)
        {
            ViewBag.backurl = backurl;

            var courselist = DepCourseBL.GetVideoCo_CourseById(id);

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
        /// 
        /// </summary>
        /// <param name="tt">前面获取的评估答案和对应的字段</param>
        /// <param name="courseId"></param>
        /// <param name="type">type:1是提交0是保存</param>
        /// <param name="num">num:1指只有一个评估,num:2指2个评估都有</param>
        /// <returns></returns>
        public JsonResult FInsertSurvey_ReplyAnswer(string tt, int courseId, int type, int ExampaperID, int num)
        {
            var course = DepCourseBL.GetCo_Course(courseId);
            //var xueshi = AllSystemConfigs.Find(p => p.ConfigType == 24);//读取参数配置表 各部分比例
            var xueshi = course.CourseLengthDistribute;
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
                    surveyReply.DeptID = CurrentUser.DeptId;
                    ISurveyReplyAnswerBL.InsertSurvey_ReplyAnswer(surveyReply);
                }
                return Json(new { result = 0, content = "保存成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var att = ClAttendceBL.GetDepAttendce(courseId, CurrentUser.UserId);

                //begin  全部折算CPA信息--by gecc (2013-12-5)
                var depCPAlist = depCpaLearn.GetDepCPALearn();
                //读取CPA配置
                Sys_ParamConfig cpazq = AllSystemConfigs.Where(p => p.ConfigType == 55).FirstOrDefault();
                string[] cpaType = Regex.Split(cpazq.ConfigValue, ",", RegexOptions.IgnoreCase);
                bool cpatemp = false;
                if (CurrentUser.SubordinateSubstation.Contains("上海"))
                {
                    //部门是否开放
                    if (cpaType[0] == "1")
                    {
                        cpatemp = true;
                    }
                }
                else
                {
                    //分所是否开放
                    if (cpaType[1] == "1")
                    {
                        cpatemp = true;
                    }
                }
                //end

                if (att == null || att.Status == 0 || att.Status == 2)
                {
                    return Json(new { result = 0, content = "请先考勤后才能进行考试" }, JsonRequestBehavior.AllowGet);
                }
                //else
                //
                //    if (att.Status == 0 && att.Status == 3)
                //        return Json(new { result = 0, content = "请先考勤后才能进行考试" }, JsonRequestBehavior.AllowGet);
                //}
                //if (course.AttFlag != 0)
                //{
                //先判断是否有考勤
                //var ttt = ClAttendceBL.GetCl_Attendce(courseId, CurrentUser.UserId);
                //if (ttt != null)
                //{
                //    if (course.AttFlag == 1 && (ttt.StartTime == Convert.ToDateTime("2050-1-1") || ttt.StartTime == DateTime.MinValue))
                //    {
                //        return Json(new { result = 1, content = "请先上课考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                //    }

                //    if (course.AttFlag == 2 && (ttt.EndTime == Convert.ToDateTime("2000-1-1") || ttt.EndTime == DateTime.MinValue))
                //    {
                //        return Json(new { result = 1, content = "请先下课考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                //    }

                //    if (course.AttFlag == 3 && (ttt == null || (ttt.StartTime == Convert.ToDateTime("2050-1-1") && ttt.EndTime == Convert.ToDateTime("2000-1-1"))))
                //    {
                //        return Json(new { result = 1, content = "请先考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                //    }
                //}
                //else
                //{
                //    return Json(new { result = 1, content = "请先考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                //}
                //}
                var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 29);
                DateTime datetime = DateTime.Now;

                //判断是否有评估 有就提示先评估 没有就直接进入考试
                if (course.IsPing == 1)
                {
                    DateTime endtime = course.EndTime.AddHours(Convert.ToDouble(course.AfterEvlutionConfigTime));
                    if (datetime < course.EndTime)
                    {
                        return Json(new { result = 1, content = "课程结束后，允许提交评估" }, JsonRequestBehavior.AllowGet);
                    }
                    if (datetime > endtime)
                    {
                        return Json(new { result = 1, content = "提交评估时间已经超出规定时间" }, JsonRequestBehavior.AllowGet);
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
                                + arrlist[z].Split('♥')[0].Split('♣')[1] + " and Status=1").Count > 0)
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
                //var courseorder = Dep_CourseOrder.GetCourseById(courseId, CurrentUser.UserId);
                var courseorder = Dep_CourseOrder.GetCo_CourseMainPaperByCourseIdAndUserId(courseId, CurrentUser.UserId);
                //判断是否有考试，如果没有则记录学时.   
                if (course.IsTest == 0 && course.IsPing == 1)
                {

                    //如果只有考勤和评估没有考试 提交评估把 评估和考试所占比列都加上去

                    if (courseorder.OrderStatus == 1)
                    {
                        double afterlenght = (double)Convert.ToInt32(xueshi.Split(';')[1]) / 100 +
                                           (double)Convert.ToInt32(xueshi.Split(';')[2]) / 100;

                        decimal forlenght = course.CourseLength * Convert.ToDecimal(afterlenght) + courseorder.GetScore;

                        Dep_CourseOrder.UpdateWhere(" set EvlutionScore=" + forlenght + ",PassStatus=1 where UserId=" + CurrentUser.UserId + " and CourseId=" + courseId);

                        #region 折算CPA--by gecc (2013-12-5)

                        //判断是否是CPA人员 and 是否开放CPA折算
                        if (cpatemp==true && CurrentUser.IsCPA == 1)
                        {
                            var depCPA = depCPAlist.Where(p => p.CourseID == courseId && p.UserID == CurrentUser.UserId).FirstOrDefault();
                            if (depCPA != null)
                            {
                                depCPA.GetLength = depCPA.GetLength + forlenght;
                                depCpaLearn.UpdateDepCPALearn(depCPA);
                            }
                            else
                            {
                                depCpaLearn.InsertDepCPALearn(new Dep_CpaLearnStatus
                                {
                                    CourseID = courseId,
                                    UserID = CurrentUser.UserId,
                                    CpaFlag = 0,
                                    GetLength = forlenght,
                                    DepartSetId = courseorder.DepartSetId
                                });
                            }
                        }

                        #endregion Model
                    }

                }
                else //如果有考试则记录 评估所占的比例分数
                {

                    decimal forlenght = (course.CourseLength * Convert.ToDecimal(xueshi.Split(';')[2]) / 100) + courseorder.GetScore;

                    Dep_CourseOrder.UpdateWhere(" set EvlutionScore=" + forlenght + " where UserId=" + CurrentUser.UserId + " and CourseId=" + courseId);

                    #region 折算CPA--by gecc (2013-12-5)

                    //判断是否是CPA人员 and 是否开放CPA折算
                    if (cpatemp == true && CurrentUser.IsCPA == 1)
                    {
                        var depCPA = depCPAlist.Where(p => p.CourseID == courseId && p.UserID == CurrentUser.UserId).FirstOrDefault();
                        if (depCPA != null)
                        {
                            depCPA.GetLength = depCPA.GetLength + forlenght;
                            depCpaLearn.UpdateDepCPALearn(depCPA);
                        }
                        else
                        {
                            depCpaLearn.InsertDepCPALearn(new Dep_CpaLearnStatus
                            {
                                CourseID = courseId,
                                UserID = CurrentUser.UserId,
                                CpaFlag = 0,
                                GetLength = forlenght,
                                DepartSetId = courseorder.DepartSetId
                            });
                        }
                    }

                    #endregion Model
                }
                return Json(new { result = 0, content = "提交成功" }, JsonRequestBehavior.AllowGet);
            }
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


        public JsonResult FBindSurvey_ReplyAnswerAndUserid(int Courseid, int userid, string SurveyPaperId)
        {
            string html = "";
            string teacherhtml = "";
            int status = 0;
            int teacherstatus = 0;
            if (SurveyPaperId != "")
            {
                if (SurveyPaperId.Split(';')[0] != "")
                {
                    var ReplyAnswer =
               ISurveyReplyAnswerBL.GetList(" ObjectID=" + Courseid + " and ExampaperID=" + SurveyPaperId.Split(';')[0].Split(',')[1] +
                                            " and UserID=" + userid + " and Status=1");
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
                                           " and UserID=" + userid + " and Status=1");
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
            return Json(new { result = 1, content = html, teachercontent = teacherhtml, status = status, teacherstatus = teacherstatus }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 毛佳源用

        public JsonResult DepMCourseList(string teachername, string CourseName, string IsMust, string LearnStatus, string Sort, string subscribeType, string StartTime, string EndTime, int pageSize = 10, int pageIndex = 1)
        {

            string sql = " 1=1";
            if (!string.IsNullOrEmpty(teachername))
            {
                sql += " and Sys_User.Realname like '%" + teachername.ReplaceSql() + "%'";
            }
            if (!string.IsNullOrEmpty(CourseName))
            {
                if (CourseName != "请输入你要找的课程名称")
                {
                    sql += " and Dep_Course.CourseName like '%" + CourseName.ReplaceSql() + "%'";
                }
            }
            if (!string.IsNullOrEmpty(IsMust))
            {
                if (IsMust != "2")
                {
                    sql += " and Dep_Course.IsMust=" + IsMust;
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
                        sql += " and (getdate() < Dep_Course.StartTime)";
                    }
                    else if (LearnStatus == "1")
                    {
                        sql += " and (getdate()>=Dep_Course.StartTime and getdate()<=Dep_Course.EndTime)";
                    }
                    else
                    {
                        sql += " and (getdate() >=Dep_Course.EndTime)";
                    }
                }
            }

            if (!string.IsNullOrEmpty(StartTime))
            {
                sql += " AND Dep_Course.StartTime  >='" + StartTime + "' ";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " AND Dep_Course.EndTime <= '" + EndTime + "' ";
            }

            if (Sort != "0")
            {
                //sql += string.Format(" and Co_Course.Sort= '{0}'", Sort);
                sql += " and charindex('" + Sort + "',Dep_Course.Sort)>0";

            }


            int litme = 0;
            //如果该部门没有配置请假时限 就给他个默认值
            //var hour = Convert.ToDecimal(DepConfig(CurrentUser.DeptId, 7) == "" ? "24" : DepConfig(CurrentUser.DeptId, 7));

            var courseorder = Dep_CourseOrder.GetMyCourseOrderList(out litme, CurrentUser.UserId, sql, (pageIndex - 1) * pageSize + 1, pageSize, "");

            return Json(new
            {
                result = 1,
                dataList = courseorder,
                recordCount = litme
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Unsubscribe(int id)
        {
            //var courseorder = iDepTran_CourseOrder.GetCo_CourseMainPaperByCourseIdAndUserId(id, CurrentUser.UserId);

            int order = Dep_CourseOrder.GetAllCourseOrderTimes(CurrentUser.UserId,DateTime.Now.Year);

            //int ordertimes = AllSystemConfigs.Find(p => p.ConfigType == 47).ConfigValue.GetInt32();

            if (DepConfig(CurrentUser.DeptId, 6) != "")
            {

                int ordertimes = Convert.ToInt32(DepConfig(CurrentUser.DeptId, 6));

                var error = "退订成功！";
                int flag = 1;

                if (ordertimes == -1)
                {
                    error = "全年课程退订不限次数！确定继续退订吗？";
                    flag = 1;
                }

                if (order < ordertimes)
                {
                    error = "你全年课程退订次数还剩" + (ordertimes - order) + "次！确定继续退订吗？";
                    flag = 1;
                }
                if (order == ordertimes)
                {
                    error = "你全年课程退订次数已经用完，不可退订！";
                    flag = 2;
                }

                return Json(new
                {
                    result = flag,
                    content = error
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    result = 2,
                    content = "本部门没有设置参数配置，不可退订！"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UnsubscribeLine(int id)
        {
            if (id != 0)
            {
                //iDepTran_CourseOrder.UpdateOrderStatus(id, CurrentUser.UserId, 0);
                Dep_CourseOrder.UpdateOrderStatus(id, CurrentUser.UserId, 0);
            }
            return Json(new
            {
                result = 1
            }, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 请假
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        public JsonResult Leave(int id, string memo)
        {
            try
            {
                int page = 1;
                if (Session["clpage"] != null)
                {
                    string sess = Session["clpage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    page = Convert.ToInt32(att[0]);
                }
                var courseId = Dep_CourseOrder.Get(id).CourseId;
                var course = Dep_CourseOrder.GetCourseById(courseId);
                var leaveMemo = AllSystemConfigs.Find(p => p.ConfigType == 31).ConfigValue;
                var str = memo.Split('♣');
                //var leader = userBL.GetUserByHrID(CurrentUser.LeaderID);
                //var leader = leaderBL.GetLeaderIdByUserId(CurrentUser.UserId);
                //获取领导
                var leader = leaderBL.GetLeaderIdByUserId(CurrentUser.UserId) ?? new Sys_User();
                //var leader ;
                var i = 0;
                Regex regex = new Regex(@"\{[\d]{1,5}\}");
                string newStr = regex.Replace(leaveMemo, new MatchEvaluator(delegate(Match match)
                {
                    i++;
                    return str[i];
                }));
                if (newStr.Contains("{Leader}"))//领导
                {
                    if (leader != null)
                        newStr = newStr.Replace("{Leader}", leader.Realname);
                    else
                        newStr = newStr.Replace("{Leader}", "<input class='Box span4' maxlength='50' type='text' />");
                }
                if (newStr.Contains("{CourseDate}"))//课程开始日期
                {
                    newStr = newStr.Replace("{CourseDate}", course.StartTime.ToString("yyyy年MM月dd日"));
                }
                if (newStr.Contains("{CourseName}"))//课程名称
                {
                    newStr = newStr.Replace("{CourseName}", course.CourseName);
                }
                if (newStr.Contains("{Myself}"))//自己【请假人】
                {
                    newStr = newStr.Replace("{Myself}", CurrentUser.Realname);
                }
                if (newStr.Contains("{LeaveDate}"))//请假日期
                {
                    newStr = newStr.Replace("{LeaveDate}", DateTime.Now.ToString("yyyy年MM月dd日"));
                }
                //var timespan = AllSystemConfigs.Find(p => p.ConfigType == 22).ConfigValue.GetDouble();

                double timespan = DepConfig(CurrentUser.DeptId, 7).GetDouble();

                //Dep_CourseOrder.UpdateLeave(id, newStr, leader.JobNum,timespan, CurrentUser.JobNum, CurrentUser.Realname);


                //获取配置
                var config = Approval.GetConfig(CurrentUser.DeptId, 7);
                var hour = config == null ? 999 : Convert.ToDecimal(config.ConfigValue);

                Dep_CourseOrder.UpdateLeave(id, newStr, courseId, CurrentUser.UserId, leader, (double)hour * 60);


                var content = GetFormworkContent(3);
                var c = string.Format(content,
                                                    leader.Realname,
                                                    CurrentUser.Realname,
                                                    str[1].NoHtml(),
                                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                                                    course.CourseName,
                                                    DateTime.Now.AddHours(timespan).ToString("yyyy-MM-dd HH:mm"));
                return Json(new
                {
                    result = 1,
                    content = "请假成功，等待审批！",
                    indexpage = page,
                    memo = c.Replace("教育培训部", CurrentUser.DeptName)
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "请假失败！"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SendLeaveMessage(string memo)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(memo))
                {
                    var messList = new List<KeyValuePair<string, string>>();
                    var emailList = new List<KeyValuePair<string, string>>();
                    //var leader = userBL.GetUserByHrID(CurrentUser.LeaderID);
                    // var leader = leaderBL.GetLeaderIdByUserId(CurrentUser.UserId);
                    //获取领导
                    var leader = leaderBL.GetLeaderIdByUserId(CurrentUser.UserId);
                    //var leader;
                    if (leader != null)
                    {
                        if (!string.IsNullOrWhiteSpace(leader.Realname))
                        {
                            {
                               // memo = leader.Realname + "," + memo;
                                if (!string.IsNullOrWhiteSpace(leader.Email))
                                    emailList.Add(new KeyValuePair<string, string>(leader.Email, memo));
                            }
                        }
                     
                        if (emailList.Count > 0)
                            SendEmail(emailList, "您的下属有人请假了，快来审批吧！");
                    }
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSystemLeaveMemo(int id)
        {
            var course = DepCourseBL.GetCo_Course(id);
            var leaveMemo = AllSystemConfigs.Find(p => p.ConfigType == 31).ConfigValue;
            Regex regex = new Regex(@"\{[\d]{1,5}\}");
            string newStr = regex.Replace(leaveMemo, new MatchEvaluator(delegate(Match match)
            {
                //return match.Value.Replace("{", "<input class='Box Raster_10' type='text' id='txtLeaveMemo").Replace("}", "' />");
                return "<input class='Box span30' maxlength='50' type='text' />";
            }));
            //if (newStr.Contains("{Leader}"))//领导
            //{
            //var leader = userBL.GetUserByHrID(CurrentUser.LeaderID);
            //var leader = leaderBL.GetLeaderIdByUserId(CurrentUser.UserId);
            var leader = leaderBL.GetLeaderIdByUserId(CurrentUser.UserId) ?? new Sys_User();
            if (leader != null)
                newStr = newStr.Replace("{Leader}", leader.Realname);
            else
                newStr = newStr.Replace("{Leader}", "<input class='Box span4' maxlength='50' type='text' />");


            if (newStr.Contains("{CourseDate}"))//课程开始日期
            {
                newStr = newStr.Replace("{CourseDate}", course.StartTime.ToString("yyyy年MM月dd日"));
            }
            if (newStr.Contains("{CourseName}"))//课程名称
            {
                newStr = newStr.Replace("{CourseName}", course.CourseName);
            }
            if (newStr.Contains("{Myself}"))//自己【请假人】
            {
                newStr = newStr.Replace("{Myself}", CurrentUser.Realname);
            }
            if (newStr.Contains("{LeaveDate}"))//请假日期
            {
                newStr = newStr.Replace("{LeaveDate}", DateTime.Now.ToString("yyyy年MM月dd日"));
            }
            return Json(newStr.Replace("教育培训部", CurrentUser.DeptName), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 我开放的课程指定查询
        /// <summary>
        /// 我开放的课程指定查询
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public ActionResult MyOpenCourse(string p)
        {
            ViewBag.currentDeptId = CurrentUser.DeptId;
            ViewBag.page = 1;
            ViewBag.course = "请输入搜索内容";
            ViewBag.teaname = "请输入搜索内容";
            ViewBag.cOrder = 99;
            ViewBag.isOrder = 99;
            ViewBag.sltStatus = 0;
            if (p != null && p != "" && p == "1")
            {
                if (Session["MyOpenCourse"] != null)
                {
                    string sess = Session["MyOpenCourse"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.course = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.teaname = att[2] == "" ? "请输入搜索内容" : att[2];
                    ViewBag.sltStatus = att[3];
                }
            }
            ViewBag.DeptList = GetAllSubDepartments();
            return View();
        }


        public ActionResult CourseAssignUserList(int courseId, int deptCourseID)
        {

            ViewBag.trainGrade = sys_TrainBL.GetAllTrainGrade();
            ViewBag.CourseId = courseId;
            ViewBag.deptCourseID = deptCourseID;
            return View();
        }

        public ActionResult CourseDropAssignUserList(int courseId, int deptCourseID)
        {
            ViewBag.trainGrade = sys_TrainBL.GetAllTrainGrade();
            ViewBag.CourseId = courseId;
            ViewBag.deptCourseID = deptCourseID;
            return View();
        }


        //详情
        public ActionResult MyOpenCourseDetail(int id, int deptCourseID)
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.model = courseOrderBL.GetCourseById(id);
            ViewBag.CourseId = id;
            ViewBag.deptCourseID = deptCourseID;
            return View();
        }

        /// <summary>
        /// 我开放的课程指定查询
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyOpenCourse(string course, string teaName, int DeptID, int sltStatus = 99, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Co_Course.Id desc")
        {
            if (Session["MyOpenCourse"] != null)
            {
                Session.Remove("MyOpenCourse");
            }
            Session["MyOpenCourse"] = pageIndex + "㉿" + course + "㉿" + teaName + "㉿" + sltStatus;

            int totalCount = 0;
            string where = " 1=1";

            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(teaName))
                where += string.Format(" and Sys_User.RealName like '%{0}%'", teaName.ReplaceSql());

            if (sltStatus == 0)
                where += string.Format(" and (Co_Course.StartTime >'{0}')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            else if (sltStatus == 1)
                where += string.Format(" and (Co_Course.StartTime <= '{0}' and Co_Course.EndTime>='{0}')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            else if (sltStatus == 2)
                where += string.Format(" and (Co_Course.EndTime<'{0}')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var list = DepCourseBL.GetMyOpenCourse(out totalCount, DeptID, where, pageIndex, pageSize, jsRenderSortField);
            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    Id = item.Id,
                    CourseName = item.CourseName.HtmlXssEncode(),
                    IsMustStr = item.IsMustStr,
                    CourseTime = item.CourseTime,
                    TeacherName = item.TeacherName.HtmlXssEncode(),
                    DeptHasEntered = item.DeptHasEntered,
                    HasEntered = item.HasEntered,
                    NumberLimited = item.NumberLimited,
                    StopDucueFlag = item.StopDucueFlag,
                    StopOrderFlag = item.StopOrderFlag,
                    OrderStr = item.OrderStr,
                    IsOrder = item.IsOrder,
                    CourseState = item.CourseState,
                    StatusShow = item.StatusShow,
                    AssignUserCount = item.AssignUserCount,
                    deptCourseID = item.deptCourseID
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取可指定人员的列表 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCourseAssignUserList(int courseId, int deptCourseID, string userName, string deptname = "", string trainGrade = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " DeptName desc")
        {
            try
            {

                string strWhere = " 1=1 ";

                strWhere += " AND IsDelete=0  AND IsTeacher<2 AND Status = 0";
                if (!string.IsNullOrEmpty(userName))
                {
                    strWhere += " AND realName like '%" + userName.ReplaceSql() + "%' ";
                }
                if (!string.IsNullOrEmpty(deptname))
                {
                    strWhere += " AND deptpath like '%" + deptname.ReplaceSql() + "%' ";
                }
                if (string.IsNullOrWhiteSpace(jsRenderSortField))
                {
                    jsRenderSortField = "deptpath desc,trainGrade asc";
                }
                if (!string.IsNullOrEmpty(trainGrade))
                {
                    string m_trainGrade = "";
                    var str = trainGrade.Split(',');
                    foreach (var item in str)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            m_trainGrade += "'" + item + "',";
                        }
                    }
                    if (m_trainGrade.Length > 0)
                    {
                        m_trainGrade = m_trainGrade.Substring(0, m_trainGrade.Length - 1);
                    }
                    strWhere += " AND trainGrade in (" + m_trainGrade + ") ";
                }
                // strWhere += " And UserType in ('在职','见习','特批','聘用')";
                int m_totalCount = 0;
                var listUser = DepCourseBL.GetCanOrderList(out m_totalCount, courseId, deptCourseID, strWhere, pageIndex, pageSize, jsRenderSortField);

                int n = 0;
                var itemArray = new object[listUser.Count()];
                foreach (var item in listUser)
                {
                    var temp = new
                    {
                        UserId = item.UserId,
                        Realname = item.Realname.HtmlXssEncode(),
                        TrainGrade = item.TrainGrade,
                        Telephone = item.MobileNum,
                        Emailstr = item.Emailstr.HtmlXssEncode(),
                        deptpath = item.DeptPath
                    };
                    itemArray[n] = temp;
                    n++;
                }
                return Json(new
                {
                    result = 0,
                    dataList = itemArray,
                    recordCount = m_totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new Array[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取可取消指定人员的列表 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDropList(int courseId, int deptCourseID, string userName, string deptname = "", string trainGrade = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = "")
        {
            try
            {

                string strWhere = " 1=1 ";

                strWhere += " AND IsDelete=0  AND IsTeacher<2 AND Status = 0";
                if (!string.IsNullOrEmpty(userName))
                {
                    strWhere += " AND realName like '%" + userName.ReplaceSql() + "%' ";
                }
                if (!string.IsNullOrEmpty(deptname))
                {
                    strWhere += " AND deptpath like '%" + deptname.ReplaceSql() + "%' ";
                }
                if (string.IsNullOrWhiteSpace(jsRenderSortField))
                {
                    jsRenderSortField = "deptpath desc,trainGrade asc";
                }
                if (!string.IsNullOrEmpty(trainGrade))
                {
                    string m_trainGrade = "";
                    var str = trainGrade.Split(',');
                    foreach (var item in str)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            m_trainGrade += "'" + item + "',";
                        }
                    }
                    if (m_trainGrade.Length > 0)
                    {
                        m_trainGrade = m_trainGrade.Substring(0, m_trainGrade.Length - 1);
                    }
                    strWhere += " AND trainGrade in (" + m_trainGrade + ") ";
                }
                //strWhere += " And UserType in ('在职','见习','特批','聘用')";
                int m_totalCount = 0;
                var listUser = DepCourseBL.GetDropList(out m_totalCount, courseId, deptCourseID, strWhere, pageIndex, pageSize, jsRenderSortField);

                int n = 0;
                var itemArray = new object[listUser.Count()];
                foreach (var item in listUser)
                {
                    var temp = new
                    {
                        UserId = item.UserId,
                        Realname = item.Realname.HtmlXssEncode(),
                        TrainGrade = item.TrainGrade,
                        Telephone = item.MobileNum,
                        Emailstr = item.Emailstr.HtmlXssEncode(),
                        deptpath = item.DeptPath
                    };
                    itemArray[n] = temp;
                    n++;
                }
                return Json(new
                {
                    result = 0,
                    dataList = itemArray,
                    recordCount = m_totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new Array[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion
    }
}
