using LiXinCommon;
using LiXinInterface;
using LiXinInterface.AttendceManage;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinInterface.DepCourseManage;
using LiXinInterface.DepTranManage;
using LiXinInterface.Examination;
using LiXinInterface.Survey;
using LiXinModels.CourseManage;
using LiXinModels.DepTranManage;
using LiXinModels.Examination.DBModel;
using LiXinModels.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace LiXinControllers
{
    public partial class DepTranMyCourseController : BaseController
    {

        protected ICo_Course CoCourseBL;
        protected ICl_Attendce ClAttendceBL;
        protected ICourseOrder CourseOrderBL;
        protected ISurveyExampaper ISurveyExampaperBL;
        protected ICo_CourseResource ICoCourseResourceBL;
        protected ICo_CoursePaper ICo_CoursePaperBL;
        protected IExamination IExaminationrBL;
        protected IExampaper IExampaperBL;
        private IMyScore scoreBL;
        protected IDepTran_CourseOrder iDepTran_CourseOrder;
        protected ISurveyReplyAnswer ISurveyReplyAnswerBL;
        protected IDepTran_DepartLeaderConfig iIDepTran_DepartLeaderConfigBL;
        protected IAttendce IAttendceBL;
        protected IDepTran_Attendce IDepTran_AttendceBL;
        protected IExamTest ExamTest;
        protected IDepMyScore IDepMyScore;
        public DepTranMyCourseController(ICo_Course _CoCourseBL, ISurveyExampaper _ISurveyExampaperBL,
            ICl_Attendce _ClAttendceBL, ICo_CourseResource _ICoCourseResourceBL, ICourseOrder _CourseOrderBL,
            ICo_CoursePaper _ICo_CoursePaperBL, IExamination _IExaminationrBL, IExampaper _IExampaperBL,
            IDepTran_CourseOrder _iDepTran_CourseOrder, IMyScore _scoreBL, ISurveyReplyAnswer _ISurveyReplyAnswerBL,
            IAttendce _IAttendceBL, IDepTran_DepartLeaderConfig _iIDepTran_DepartLeaderConfigBL, IDepTran_Attendce _IDepTran_AttendceBL,
            IExamTest examTest, IDepMyScore _IDepMyScore)
        {

            CoCourseBL = _CoCourseBL;
            ISurveyExampaperBL = _ISurveyExampaperBL;
            ClAttendceBL = _ClAttendceBL;
            ICoCourseResourceBL = _ICoCourseResourceBL;
            CourseOrderBL = _CourseOrderBL;
            ICo_CoursePaperBL = _ICo_CoursePaperBL;
            IExaminationrBL = _IExaminationrBL;
            IExampaperBL = _IExampaperBL;
            iDepTran_CourseOrder = _iDepTran_CourseOrder;
            scoreBL = _scoreBL;
            ISurveyReplyAnswerBL = _ISurveyReplyAnswerBL;
            IAttendceBL = _IAttendceBL;
            iIDepTran_DepartLeaderConfigBL = _iIDepTran_DepartLeaderConfigBL;
            IDepTran_AttendceBL = _IDepTran_AttendceBL;
            ExamTest = examTest;
            IDepMyScore = _IDepMyScore;
        }


        public ActionResult AfterCourse(int courseid, int backFrom = 0)
        {

            var courselist = CoCourseBL.GetVideoCo_CourseById(courseid);

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
                ViewBag.id = courseid;
                ViewBag.SurveyPaperId = courselist.SurveyPaperId;
            }
            ViewBag.backFrom = backFrom;
            return View();
          
        }

        public ActionResult Attendance(int courseid, int backFrom = 0)
        {

            var Cl_Attendce = IDepTran_AttendceBL.GetDepAttendce(courseid, CurrentUser.UserId);
            
            //var Cl_Attendce = ClAttendceBL.GetCl_Attendce(courseid, CurrentUser.UserId);
            var cocurse = CoCourseBL.GetCo_Course(courseid);
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
            ViewBag.backFrom = backFrom;
            return View();
        }

        public ActionResult ClassPro(int courseid, int backFrom = 0)
        {
            ViewBag.Id = courseid;
            ViewBag.backFrom = backFrom;
            return View();
        }

        public ActionResult CourseList()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="backFrom">返回的页面0-MyCourseList 我的已预订课程，1-DepTrainMyBroadcastCourse/MyBroadcastList 转播课程</param>
        /// <returns></returns>
        public ActionResult CourseMain(int courseid, int backFrom = 0)
        {
            var config = iIDepTran_DepartLeaderConfigBL.GetDepartCourseLimitNumber(CurrentUser.UserId,courseid);
            var course = CoCourseBL.GetCo_Course(courseid);   
            if(config.Count>0)
            {
                course.NumberLimited = config[0].LimitNumber;
            }
            ViewBag.Course = course;
            var courseResourse = ICoCourseResourceBL.GetList(courseid);
            ViewBag.CourseResourse = courseResourse;
            ViewBag.backFrom = backFrom;
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="backFrom">返回的页面0-MyCourseList 我的已预订课程，1-DepTrainMyBroadcastCourse/MyBroadcastList 转播课程</param>
        /// <returns></returns>
        public ActionResult MyCourse(int courseid,int backFrom=0,int tp=0)
        {
            var Course = iDepTran_CourseOrder.GetCourseById(courseid, CurrentUser.UserId);

            var courseorder = iDepTran_CourseOrder.GetCo_CourseMainPaperByCourseIdAndUserId(courseid, CurrentUser.UserId);
            ViewBag.courseorder = courseorder;

            ViewBag.tp = tp;
            ViewBag.Course = Course;
            ViewBag.backFrom = backFrom;
            return View();
        }

        public ActionResult MyCourseList()
        {
            return View();
        }


        public ActionResult OnlineTest(int courseid, int backFrom = 0)
        {

            Co_CoursePaper CoCoursePaper = ICo_CoursePaperBL.GetCo_CourseMainPaper(courseid);
            var course = CoCourseBL.GetCo_Course(courseid);

            if (CoCoursePaper != null && course.IsTest == 1)
            {
                var exap = IExaminationrBL.GetExamination(CoCoursePaper.PaperId);
                ViewBag.exap = exap;
                // var examsendstudent = IExaminationrBL.GetExamSendStudent(exap.PaperID);
                var examsendstudent = IExaminationrBL.GetSingletbExamSendStudentByCourseIdAndUserId(courseid,
                                                                                                    CurrentUser.UserId, 4);
                var exampapger = IExampaperBL.GetExampaper(CoCoursePaper.PaperId);
                ViewBag.exampapger = exampapger;
                ViewBag.examsendstudent = examsendstudent;
            }
            ViewBag.Co_CoursePaper = CoCoursePaper;
            ViewBag.courseid = courseid;
            ViewBag.course = course;

            //int leavestatus = GetUserForCourseStatus(id) ? 1 : 0;
            //ViewBag.leavestatus = leavestatus;
            ViewBag.backFrom = backFrom;
            return View();
        }

        public JsonResult DepTranMCourseList(string teachername, string CourseName, string IsMust, string LearnStatus, string Sort, int ApprovalFlag, string StartTime, string EndTime, int pageSize = 10, int pageIndex = 1)
        {
            int deptid = iIDepTran_DepartLeaderConfigBL.GetDepartSettingDepartSetID(CurrentUser.UserId);
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

            if (!string.IsNullOrEmpty(StartTime))
            {
                sql += " AND StartTime  >='" + StartTime + "' ";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " AND EndTime <= '"+EndTime+"' ";
            }

            if (Sort != "0")
            {
                //sql += string.Format(" and Co_Course.Sort= '{0}'", Sort);
                sql += " and charindex('" + Sort + "',Co_Course.Sort)>0";
                
            }

            //if (ApprovalFlag != 3)
            //{
                
            //        sql += string.Format(" and  DepTran_CourseOpen.ApprovalFlag={0}", ApprovalFlag);
    
            //}

            int litme = 0;

            var courseorder = iDepTran_CourseOrder.GetMyCourseOrderList(out litme, CurrentUser.UserId, deptid,sql, (pageIndex - 1) * pageSize + 1, pageSize, "");

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

            int order = iDepTran_CourseOrder.GetAllCourseOrderTimes(CurrentUser.UserId);
            
            int ordertimes = AllSystemConfigs.Find(p => p.ConfigType == 41).ConfigValue.GetInt32();

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


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UnsubscribeLine(int id)
        {
            


            if (id != 0)
            {
                iDepTran_CourseOrder.UpdateOrderStatus(id, CurrentUser.UserId, 0);
               
            }

            return Json(new
            {
                result = 1
               
            }, JsonRequestBehavior.AllowGet);
           
           
        }



        #region 课后评估
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tt">前面获取的评估答案和对应的字段</param>
        /// <param name="courseId"></param>
        /// <param name="type">type:1是提交0是保存</param>
        /// <param name="num">num:1指只有一个评估,num:2指2个评估都有</param>
        /// <returns></returns>
        public JsonResult FInsertSurvey_ReplyAnswer(string tt, int courseId, int type, int ExampaperID, int num, int back=0)
        {
            var course = CoCourseBL.GetCo_Course(courseId);

            var att = IDepTran_AttendceBL.GetDepAttendce(courseId, CurrentUser.UserId);

            //back为1的时候 是从转播视频里来的
            if (back == 1)
            {         
                if (att == null)
                {
                    return Json(new { result = 1, content = "请先考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                }
                #region
                //var list = iDepTran_CourseOrder.GetCo_CourseMainPaperByCourseIdAndUserId(courseId, CurrentUser.UserId);
                //int deptid = iIDepTran_DepartLeaderConfigBL.GetDepartSettingDepartSetID(CurrentUser.UserId);
                //if (list == null)
                //{
                    
                //    DepTran_CourseOrder model = new DepTran_CourseOrder();

                //    model.AttScore = 0;
                //    model.CourseId = courseId;
                //    model.GetScore = 0;
                //    model.IsAppoint = 0;
                //    model.LearnStatus = 0;
                //    model.OrderStatus = 3;
                //    model.OrderTime = DateTime.Now;
                //    model.PassStatus = 0;
                //    model.UserId = CurrentUser.UserId;
                //    model.EvlutionScore = 0;
                //    model.ExamScore = 0;
                //    model.DepartSetId = deptid;
                //    model.OrderTimes = 0;

                //    iDepTran_CourseOrder.AddDepTran_CourseOrder(model);
                //}
                #endregion
            }
            

            //var xueshi = AllSystemConfigs.Find(p => p.ConfigType == 24);//读取参数配置表 各部分比例
            var xueshi = course.CourseLengthDistribute;
            //way=1是集中   2是视频
            
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
                    //var att = IDepTran_AttendceBL.GetDepAttendce(courseId,CurrentUser.UserId);

                    if (att == null || att.Status == 0 || att.Status == 2)
                    {
                        return Json(new { result = 1, content = "请先考勤后才能提交课后评估" }, JsonRequestBehavior.AllowGet);
                    }

                    var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 29);
                    DateTime datetime = DateTime.Now;

                    //判断是否有评估 有就提示先评估 没有就直接进入考试
                    if (course.IsPing == 1)
                    {
                        DateTime endtime = course.EndTime.AddHours(Convert.ToDouble(course.AfterEvlutionConfigTime));

                        if (datetime < course.EndTime)
                        {
                            return  Json(new{result = 1, content = "课程结束后，允许提交评估" }, JsonRequestBehavior.AllowGet);
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
                    //var courseorder = CourseOrderBL.GetCourseById(courseId, CurrentUser.UserId);
                    var courseorder = iDepTran_CourseOrder.GetCo_CourseMainPaperByCourseIdAndUserId(courseId, CurrentUser.UserId);
                    //判断他是否是第一次学习课程,如果是第一次则记录分数,第2次的话就不做任何操作
                    //if (IAttendceBL.ExistAtts(courseId, CurrentUser.UserId))
                    //{
                        //判断是否有考试，如果没有则记录学时.   
                        if (course.IsTest == 0 && course.IsPing == 1)
                        {

                            //如果只有考勤和评估没有考试 提交评估把 评估和考试所占比列都加上去

                            if (courseorder.OrderStatus == 1)
                            {
                                double afterlenght = (double)Convert.ToInt32(xueshi.Split(';')[1]) / 100 +
                                                   (double)Convert.ToInt32(xueshi.Split(';')[2]) / 100;

                                decimal forlenght = course.CourseLength * Convert.ToDecimal(afterlenght);// +courseorder.GetScore;


                                iDepTran_CourseOrder.UpdateWhere(" set EvlutionScore=" + forlenght + ",PassStatus=1 where UserId=" + CurrentUser.UserId + " and CourseId=" + courseId);
                            }
                            //CourseOrderBL.UpdateGetScore(course.Id, CurrentUser.UserId, forlenght, 1, 2);
                                                      
                            
                        }
                        else //如果有考试则记录 评估所占的比例分数
                        {
                            if (courseorder.OrderStatus == 1)
                            {
                                decimal forlenght = (course.CourseLength * Convert.ToDecimal(xueshi.Split(';')[2]) / 100);// +courseorder.GetScore;

                                iDepTran_CourseOrder.UpdateWhere(" set EvlutionScore=" + forlenght + " where UserId=" + CurrentUser.UserId + " and CourseId=" + courseId);
                            }
                            //CourseOrderBL.UpdateGetScore(course.Id, CurrentUser.UserId, forlenght, 2, 1);//状态不变

                        }
                    //}
                    return Json(new { result = 0, content = "提交成功" }, JsonRequestBehavior.AllowGet);
                }
            }
        



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



            var att = IDepTran_AttendceBL.GetDepAttendce(courseid, CurrentUser.UserId);

            if (att == null||att.Status == 0 || att.Status == 2)
            {
                return Json(new { result = 0, message = "请先考勤后才能进行考试" }, JsonRequestBehavior.AllowGet);
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
    }
}
