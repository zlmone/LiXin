using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LiXinBLL.AttendceManage;
using LiXinCommon;
using LiXinControllers;
using LiXinInterface.AttendceManage;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinInterface.Examination;
using LiXinModels.CourseLearn;
using LiXinModels.Examination.DBModel;
using LiXinModels.Examination.ShowModel;
using LiXinInterface.NewCourseManage;
using LiXinInterface.DepTranManage;
using LiXinModels.DepTranManage;
using LiXinInterface.DepCourseManage;
using LiXinInterface.DepCourseLearn;
using LiXinModels.DepCourseLearn;
using LiXinModels;
using System.Text.RegularExpressions;

namespace LiXinExam.Controllers
{
    public class ExamTestController : BaseController
    {
        protected IExamTest ExamTestBL;
        protected IExamination ExaminationBL;
        protected IExampaper ExampaperBL;
        protected ICo_CoursePaper ICoCoursePaperBL;
        protected ICo_Course ICourseBl;
        protected ICourseOrder CourseOrderBL;

        protected ICl_CpaLearnStatus ICpaLearnStatusBL;
        protected ICl_LearnVideoInfor IClLearnVideoInforBL;

        protected IAttendce IAttendceBL;

        protected INew_Course inewcourseBl;
        protected INew_CoursePaper inewcoursepaperBl;
        protected INew_CourseOrder inewcourseorderBl;


        protected IDepTran_CourseOrder IDeptran_courseorder;
        protected IDepTran_DepartLeaderConfig iIDepTran_DepartLeaderConfigBL;

        protected IDep_CourseOrder _IDep_courseOrder;
        protected IDep_CoursePaper _IDep_coursePaper;
        protected IDep_Course _IDep_Course;
        protected IDep_CpaLearnStatus depCpaLearn;


        public ExamTestController(IExamTest examTestBL, IExamination examinationBL, IExampaper exampaperBL, ICo_CoursePaper _ICoCoursePaperBL,
            ICo_Course _ICourseBl, ICourseOrder _CourseOrderBL, ICl_CpaLearnStatus _ICpaLearnStatusBL,
            ICl_LearnVideoInfor _IClLearnVideoInforBL, AttendceBL _IAttendceBL, INew_Course _inewcourseBl, INew_CoursePaper _inewcoursepaperBl,
            INew_CourseOrder _inewcourseorderBl, IDepTran_DepartLeaderConfig _iIDepTran_DepartLeaderConfigBL,
            IDepTran_CourseOrder _IDeptran_courseorder, IDep_CourseOrder IDep_courseOrder, IDep_CoursePaper IDep_coursePaper, IDep_Course IDep_Course, IDep_CpaLearnStatus _depCpaLearn)
        {
            ExamTestBL = examTestBL;
            ExaminationBL = examinationBL;
            ExampaperBL = exampaperBL;
            ICoCoursePaperBL = _ICoCoursePaperBL;
            ICourseBl = _ICourseBl;
            CourseOrderBL = _CourseOrderBL;
            ICpaLearnStatusBL = _ICpaLearnStatusBL;
            IClLearnVideoInforBL = _IClLearnVideoInforBL;
            IAttendceBL = _IAttendceBL;
            inewcourseBl = _inewcourseBl;
            inewcoursepaperBl = _inewcoursepaperBl;
            inewcourseorderBl = _inewcourseorderBl;
            iIDepTran_DepartLeaderConfigBL = _iIDepTran_DepartLeaderConfigBL;
            IDeptran_courseorder = _IDeptran_courseorder;
            _IDep_courseOrder = IDep_courseOrder;
            _IDep_coursePaper = IDep_coursePaper;
            _IDep_Course = IDep_Course;
            depCpaLearn = _depCpaLearn;
        }

        #region 页面呈现

        /// <summary>
        ///     我的考试列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MyExaminationList()
        {
            return View();
        }

        /// <summary>
        ///     考试页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamTestOnline()
        {
            return View();
        }

        /// <summary>
        ///     答卷详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ExamTestDetail(int euID, int type = 0)
        {
            var exam = ExamTestBL.GetExamBaseInforShow(euID, type);
            ViewBag.PercentFlag = exam.PercentScore;
            ViewBag.ExamName = exam.ExamTitle;
            ViewBag.ExamScoreStatus = exam.ResultFlag;
            ViewBag.ExanUser = ExamTestBL.GetExamUser(euID);
            ViewBag.QuestionList = ExamTestBL.GetMyExaminationQuestionList(euID);
            return View();
        }

        /// <summary>
        /// 部门分所自学-答卷详情页面 type=5
        /// </summary>
        /// <param name="euID">考试人员ID</param>
        /// <param name="type">试卷分类5：部门分所</param>
        /// <returns></returns>
        public ActionResult DepExamTestDetail(int euID, int type = 0)
        {
            var exam = ExamTestBL.GetExamBaseInforShow(euID, type);
            ViewBag.PercentFlag = exam.PercentScore;
            ViewBag.ExamName = exam.ExamTitle;
            ViewBag.ExamScoreStatus = exam.ResultFlag;
            ViewBag.ExanUser = ExamTestBL.GetExamUser(euID);
            ViewBag.QuestionList = ExamTestBL.GetMyExaminationQuestionList(euID);
            return View();
        }

        #endregion

        #region 方法

        /// <summary>
        ///     获取我的考试
        /// </summary>
        /// <param name="examName">考试名称</param>
        /// <param name="type">0：未开始；1：进行中；2：已结束；3：已过期</param>
        /// <param name="passflag">当type==2时，0：全部；1：通过；2：不通过</param>
        /// <param name="pageSize">每页显示的个数</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns></returns>
        public JsonResult GetMyExamTestList(string examName = "", int type = 0, int passflag = 0, int pageSize = 20,
                                            int pageIndex = 1)
        {
            //获取我的考试信息
            List<tbExamSendStudent> myTotalExamList = ExamTestBL.GetMyExamList(Session["userID"].StringToInt32());
            //相关的考试
            List<tbExamination> examlist = ExaminationBL.GetAllExamination(myTotalExamList.Select(p => p.RelationID));
            //相关的试卷
            List<tbExampaper> paperList = ExampaperBL.GetAllExampaper(examlist.Select(p => p.PaperID));

            //筛选后的我的考试
            var myExamListShow = new List<ExamTestShow>();
            myTotalExamList.ForEach(p =>
                {
                    var exam = examlist.FirstOrDefault(pa => pa._id == p.RelationID);
                    if (exam != null && exam.PublishedFlag == 1)
                    {
                        tbExampaper paper = paperList.FirstOrDefault(pa => pa._id == exam.PaperID);
                        if (paper != null)
                        {
                            var toscore = paper.ExamType == 0 ? paper.QuestionList.Sum(pc => pc.QScore) : paper.ExampaperScore;
                            if (toscore > 0)
                            {
                                myExamListShow.Add(new ExamTestShow
                                                       {
                                                           ExamUserID = p._id,
                                                           ExampaperID = exam.PaperID,
                                                           ExampaperSortID = paper.ExamSortID,
                                                           ExamTitle = exam.ExaminationTitle,
                                                           EndTime = exam.ExamEndTime.ToLocalTime(),
                                                           ExamLength = exam.ExamLength,
                                                           ExamTestTimes = p.TestTimes + "/" + exam.TestTimes,
                                                           ExamScore =
                                                               exam.PercentFlag == 0
                                                                   ? (exam.PassScore + "/100")
                                                                   : (Convert.ToInt32(toscore * exam.PassScore / 100) + "/" + toscore),
                                                           PassFlag = exam.PublishResult == 1 ? p.IsPass : 3,
                                                           //未发布成绩时3
                                                           ScoreOrder = exam.PublishResult == 1 ? "-" : "--",
                                                           //未发布成绩时"--",否则"-"
                                                           StartTime = exam.ExamBeginTime.ToLocalTime(),
                                                           DoExamStatus = p.DoExamStatus,
                                                           ExamID = p.RelationID,
                                                           GetScore =
                                                               exam.PublishResult == 1
                                                                   ? (exam.PercentFlag == 0
                                                                          ? (p.StudentAnswerList.Sum(pa => pa.GetScore) *
                                                                             100 / toscore + "")
                                                                          : (p.StudentAnswerList.Sum(pa => pa.GetScore) +
                                                                             ""))
                                                                   : "--",
                                                           pFlag = passflag
                                                       });
                            }
                        }
                    }
                });
            myExamListShow = myExamListShow.Where(p => p.ExamTitle.Contains(examName) && type == p.examStatus).ToList();

            IEnumerable<ExamTestShow> myExam =
                myExamListShow.OrderByDescending(p => p.ExamUserID).Skip((pageIndex - 1) * pageSize).Take(pageSize);


            //所有的参考人员
            var allExamUser = new List<tbExamSendStudent>();
            if (type == 1 || type == 2)
            {
                IEnumerable<int> examLists = myExam.ToList().Select(p => p.ExamID);
                allExamUser = ExamTestBL.GetExamSendStudentList(examLists);
            }

            foreach (ExamTestShow exam in myExam)
            {
                if (type == 0 || type == 3)
                    exam.ScoreOrder = "--";
                else
                {
                    int score =
                        allExamUser.FirstOrDefault(p => p._id == exam.ExamUserID).StudentAnswerList.Sum(p => p.GetScore);
                    int order =
                        allExamUser.Count(
                            p => p.RelationID == exam.ExamID && p.StudentAnswerList.Sum(pa => pa.GetScore) > score);
                    exam.ScoreOrder = exam.ScoreOrder == "-" ? (order + 1).ToString() : "--";
                }
            }

            return Json(new { result = 1, dataList = myExam.ToList(), recordCount = myExamListShow.Count },
                        JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     判断是否可以进入考试
        /// </summary>
        /// <param name="euID">考试人员ID</param>
        /// <param name="flag">0:列表进入；1：真正进入</param>
        /// <returns></returns>
        public JsonResult JudgeCanExamTest(int euID, int flag)
        {
            tbExamSendStudent eu = ExamTestBL.GetExamUser(euID);
            //当SourceType不等于0的时候 表示是课程下的考试
            //if (eu.SourceType == 0)
            //{ 
            if (eu.SourceType == 0)
            {
                tbExamination exam = ExaminationBL.GetExamination(eu.RelationID);
                if (eu.TestTimes >= exam.TestTimes && exam.TestTimes >= 0)
                    return Json(new { result = 0, message = "进入次数已达到上限！" }, JsonRequestBehavior.AllowGet);
                if (exam.ExamEndTime.ToLocalTime() <= DateTime.Now)
                    return Json(new { result = 0, message = "本场考试已经结束，不可进入！" }, JsonRequestBehavior.AllowGet);
                eu.TestTimes += 1;
                if (flag == 1)
                    ExamTestBL.SaveExamUser(eu);
            }
            //}
            else
            {
                if (eu.SourceType == 1 || eu.SourceType == 2 || eu.SourceType == 4)
                {
                    var coursepaper = ICoCoursePaperBL.GetCo_CourseMainPaper(eu.RelationID);
                    if (eu.TestTimes >= coursepaper.TestTimes && coursepaper.TestTimes >= 0)
                    {
                        return Json(new { result = 0, message = "进入次数已达到上限！" }, JsonRequestBehavior.AllowGet);
                    }
                    eu.TestTimes += 1;
                    if (flag == 1)
                    {
                        eu.TestTime = DateTime.Now;
                        eu.DoExamStatus = 1;
                        ExamTestBL.SaveExamUser(eu);
                    }
                }
                if (eu.SourceType == 3)
                {
                    var coursepaper = inewcoursepaperBl.GetSingleCoursePaper(eu.RelationID);
                    if (eu.TestTimes >= coursepaper.TestTimes && coursepaper.TestTimes >= 0)
                    {
                        return Json(new { result = 0, message = "进入次数已达到上限！" }, JsonRequestBehavior.AllowGet);
                    }
                    var course = inewcourseBl.GetSingleCourse(coursepaper.CourseId);
                    //if (course.EndTime> DateTime.Now)
                    //    return Json(new { result = 0, message = "课程还未结束，不可进入！" }, JsonRequestBehavior.AllowGet);
                    if (course.EndTime.AddHours(coursepaper.Hour).ToLocalTime() <= DateTime.Now)
                        return Json(new { result = 0, message = "本场考试已经结束，不可进入！" }, JsonRequestBehavior.AllowGet);

                    eu.TestTimes += 1;
                    if (flag == 1)
                    {
                        eu.DoExamStatus = 1;
                        eu.TestTime = DateTime.Now;
                        ExamTestBL.SaveExamUser(eu);
                    }
                }
                if (eu.SourceType == 5)
                {
                    var coursepaper = _IDep_coursePaper.GetCo_CourseMainPaper(eu.RelationID);

                    if (coursepaper.TestTimes != -1)
                    {
                        if (eu.TestTimes >= coursepaper.TestTimes && coursepaper.TestTimes >= 0)
                        {
                            return Json(new { result = 0, message = "进入次数已达到上限！" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var course = _IDep_Course.GetCo_Course(coursepaper.CourseId);
                    //if (course.EndTime> DateTime.Now)
                    //    return Json(new { result = 0, message = "课程还未结束，不可进入！" }, JsonRequestBehavior.AllowGet);
                    if (course.EndTime.AddHours(coursepaper.Hour).ToLocalTime() <= DateTime.Now)
                        return Json(new { result = 0, message = "本场考试已经结束，不可进入！" }, JsonRequestBehavior.AllowGet);

                    eu.TestTimes += 1;
                    if (flag == 1)
                    {
                        eu.TestTime = DateTime.Now;
                        eu.DoExamStatus = 1;
                        ExamTestBL.SaveExamUser(eu);
                    }
                }
            }
            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        ///     根据考试人员ID，获取考试基本信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetExaminationInformation(int examUserID, int SourceType = 0)
        {

            return Json(new { result = 1, data = ExamTestBL.GetExamBaseInforShow(examUserID, SourceType) },
                        JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        ///     根据考试人员ID，获取试卷的基本信息
        /// </summary>
        /// <param name="examUserID"></param>
        /// <returns></returns>
        public JsonResult GetExampaperInformation(int examUserID, int SourceType = 0)
        {
            return Json(new { result = 1, data = ExamTestBL.GetExampaperBaseInforShow(examUserID, SourceType) },
                        JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     获取服务器的考试倒计时的时间差
        /// </summary>
        /// <param name="euID">考生考试ID</param>
        /// <returns></returns>
        public JsonResult GetSericeTimeDifference(int euID, int SourceType = 0)
        {
            if (SourceType == 0)
            {
                DateTime startTime =
                    ExaminationBL.GetExamination(ExamTestBL.GetExamUser(euID).RelationID).ExamBeginTime.ToLocalTime();

                if (DateTime.Now >= startTime)
                    return Json(new { result = 1, timeLength = 0 }, JsonRequestBehavior.AllowGet);
                var ts1 = new TimeSpan(DateTime.Now.Ticks);
                var ts2 = new TimeSpan(startTime.Ticks);
                TimeSpan ts = ts2.Subtract(ts1).Duration();
                return Json(new { result = 1, timeLength = (int)ts.TotalSeconds }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var tt = ICourseBl.GetCo_Course((ExamTestBL.GetExamUser(euID).RelationID));
                // var coursepaper = ICoCoursePaperBL.GetCo_CourseMainPaper(tt.Id);//查找关联试卷表


                //if (coursepaper.Length == 0)
                //{
                //var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 26);
                //DateTime startTime = tt.StartTime.AddHours(Sys_ParamConfig.ConfigValue.StringToInt32());
                ////DateTime startTime = tt.EndTime.AddHours(coursepaper.Length);
                //if (DateTime.Now <= startTime)  //if (DateTime.Now >= startTime)
                return Json(new { result = 1, timeLength = 0 }, JsonRequestBehavior.AllowGet);
                //var ts1 = new TimeSpan(DateTime.Now.Ticks);
                //var ts2 = new TimeSpan(startTime.Ticks);
                //TimeSpan ts = ts2.Subtract(ts1).Duration();
                //return Json(new { result = 1, timeLength = (int)ts.TotalSeconds }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    DateTime startTime = tt.EndTime.AddHours(1);
                //    if (DateTime.Now >= startTime)  //if (DateTime.Now >= startTime)
                //        return Json(new { result = 1, timeLength = 0 }, JsonRequestBehavior.AllowGet);
                //    var ts1 = new TimeSpan(DateTime.Now.Ticks);
                //    var ts2 = new TimeSpan(startTime.Ticks);
                //    TimeSpan ts = ts2.Subtract(ts1).Duration();
                //    return Json(new { result = 1, timeLength = (int)ts.TotalSeconds }, JsonRequestBehavior.AllowGet);
                //}




            }
        }

        /// <summary>
        ///     保存学员答案
        /// </summary>
        /// <param name="form">答案</param>
        /// <param name="euid">学员考试ID</param>
        /// <param name="submitType">1:暂存；2：提交</param>
        /// <param name="pecent">是否百分制（0：是；1:否）</param>
        /// <param name="passScore">及格线</param>
        /// 0为考试试卷1为课程下试卷
        /// <returns></returns>
        public JsonResult SubmitStudentAnswer(FormCollection form, int euid = 0, int submitType = 1, int pecent = 0,
                                              int passScore = 0, int courseType = 0)
        {
            string answer = form["userAnswer"]; //答案
            string quScore = form["questionScore"]; //试题分值
            string quOrder = form["questionOrder"]; //试题题序
            if (answer == "" || quScore == "" || quOrder == "")
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);

            //tbExamSendStudent examUser = ExamTestBL.GetExamUser(euid);

            tbExamSendStudent examUser = null;
            tbExamSendStudent student = null;

            if (courseType == 0)
            {
                examUser = ExamTestBL.GetExamUser(euid);
            }
            else
            {
                //var  student= ExamTestBL.GetExamUser(euid);
                student = ExamTestBL.GetExamUser(euid);
                examUser = ExaminationBL.GetSingletbExamSendStudentByCourseIdAndUserId(student.RelationID, CurrentUser.UserId, student.SourceType);
                //if (student.SourceType == 1)
                //{ 
                //    examUser = ExaminationBL.GetSingletbExamSendStudentByCourseIdAndUserId(student.RelationID, CurrentUser.UserId,1);
                //}
                //else
                //{
                //    examUser = ExaminationBL.GetSingletbExamSendStudentByCourseIdAndUserId(student.RelationID, CurrentUser.UserId, 2);
                //}

            }
            var oldPaperScore = examUser.StudentAnswerList;//.Sum(p => p.GetScore);       

            var pass = 0;//0没通过，1通过
            string nopassnumber = "0";

            var dicAnswer = new Dictionary<int, string>();
            answer.Split(new[] { "**!!!**" }, StringSplitOptions.None).ToList().ForEach(p =>
                {
                    string[] arr = p.Split(new[] { "!!***!!" }, StringSplitOptions.None);
                    dicAnswer.Add(arr[0].StringToInt32(), arr[1]);
                });

            var dicScore = new Dictionary<int, int>();
            quScore.Split(';').ToList().ForEach(p =>
                {
                    string[] arr = p.Split(',');
                    dicScore.Add(arr[0].StringToInt32(), arr[1].StringToInt32());
                });

            var dicOrder = new Dictionary<int, int>();
            quOrder.Split(';').ToList().ForEach(p =>
                {
                    string[] arr = p.Split(',');
                    dicOrder.Add(arr[0].StringToInt32(), arr[1].StringToInt32());
                });

            List<tbQuestion> qulist = ExamTestBL.GetQuestionList(dicAnswer.Keys.ToList());
            examUser.StudentAnswerList = new List<ReStudentExamAnswer>();

            //int NowSum = 0;//记录当前分数

            //循环学员答案
            foreach (var o in dicAnswer)
            {
                tbQuestion qu = qulist.FirstOrDefault(p => p._id == o.Key); //试题
                if (qu != null)
                {
                    int score = submitType == 2 ? GetScore(qu, o.Value, dicScore[o.Key]) : 0;
                    examUser.StudentAnswerList.Add(new ReStudentExamAnswer
                        {
                            DoneFlag = o.Value == "" ? 0 : 1,
                            Evlution = "",
                            GetScore = score,
                            Answer = o.Value,
                            Qid = o.Key,
                            QType = qu.QuestionType,
                            Order = dicOrder[o.Key],
                            Score = dicScore[o.Key]
                        });
                    // NowSum += score;
                }
            }
            if (submitType == 2)
            {
                examUser.DoExamStatus = 2;
                int totalScore = dicScore.Values.Sum(); //总分
                examUser.PaperScore = pecent == 1 ? passScore : totalScore * passScore / 100; //考试通过的基线
                int userScore = examUser.StudentAnswerList.Sum(p => p.GetScore); //考生得分
                if (pecent == 0)
                {
                    examUser.IsPass = userScore * 100 / totalScore >= passScore ? 1 : 0;
                }
                else
                {
                    examUser.IsPass = userScore >= passScore ? 1 : 0;
                }
            }

            //添加每次考试的学习时间 用逗号分开 后面用中位数 2013-11-20 毛佳源
            //string answertime = (DateTime.Now - examUser.TestTime.ToLocalTime()).ToString("p");

            TimeSpan sp = DateTime.Now - examUser.TestTime.ToLocalTime();

            examUser.AnswerTime = Convert.ToInt32(sp.TotalSeconds).ToString();
            //if (examUser.AnswerTime == "")
            //{ examUser.AnswerTime = sp.Seconds.ToString(); }
            //else
            //{
            //    examUser.AnswerTime = examUser.AnswerTime + "," + sp.Seconds.ToString();
            //}


            //当1的时候是课程下的考试
            if (examUser.SourceType == 1)
            {
                #region 一期：集中授课考试
                //var student = ExamTestBL.GetExamUser(euid);
                //查找这门课程对应的通过线和考试次数
                var CoCoursePaper = ICoCoursePaperBL.GetCo_CourseMainPaper(student.RelationID);
                //查找课程信息 用于查找该课程的学时
                var course = ICourseBl.GetCo_Course(student.RelationID);
                //查找试卷总分
                var exampaper = ExampaperBL.GetExampaper(CoCoursePaper.PaperId);


                if (IAttendceBL.ExistAtts(course.Id, CurrentUser.UserId))
                {
                    //获取这个人预定信息 因为集中课程学时是走一步算一步 获取已经获得的学时 
                    var courseorder = CourseOrderBL.GetCourseById(student.RelationID, CurrentUser.UserId);

                    //如果当前考试分数大于上一次考的分数则修改, 如果小则还是记录以前的分数
                    int fenshju = examUser.StudentAnswerList.Sum(p => p.GetScore);
                    //int olderfenshuju=
                    if (fenshju > oldPaperScore.Sum(p => p.GetScore))
                    {
                        //examUser.PaperScore = fenshju;
                        ExamTestBL.SaveExamUser(examUser);
                    }
                    else
                    {
                        if (oldPaperScore.Count != 0)  // 如果第一次没过就记录当前选项
                        {
                            examUser.StudentAnswerList = oldPaperScore;
                        }
                        ExamTestBL.SaveExamUser(examUser);
                    }

                    //var xueshi = AllSystemConfigs.Find(p => p.ConfigType == 24);
                    var xueshi = course.CourseLengthDistribute;
                    var aa = examUser.StudentAnswerList.Sum(p => p.GetScore);
                    var tongguofen = (exampaper.ExampaperScore * CoCoursePaper.LevelScore) / 100;
                    //var aa = ((double)examUser.StudentAnswerList.Sum(p => p.GetScore) / (double)exampaper.ExampaperScore)*100;

                    //考试没有超过通线 记录课后评估
                    //if ((int)aa < CoCoursePaper.LevelScore)
                    nopassnumber = (Convert.ToInt32(CoCoursePaper.TestTimes) - student.TestTimes).ToString();
                    if ((int)aa < (int)tongguofen)
                    {
                        pass = 0;
                        //考试不过则不做任何操作
                        if (examUser.Number == 0)
                        {
                            //double afterlenght = (double) Convert.ToInt32(xueshi.ConfigValue.Split(';')[0])/100 +
                            //                        (double) Convert.ToInt32(xueshi.ConfigValue.Split(';')[2])/100;

                            ////获取考试不过的百分比
                            //decimal forlenght = course.CourseLength*Convert.ToDecimal(afterlenght);

                            ////加上已得分数
                            //decimal tt = courseorder.GetScore  + forlenght;
                            ////PassStatus;1：通过；2：不通过；LearnStatus: 1：进行中；2：已完成
                            //CourseOrderBL.UpdateGetScore(course.Id, CurrentUser.UserId, tt, 2, 1);
                        }
                    }
                    else //考试通过
                    {
                        pass = 1;
                        //当Number为1的时候 则不在记录学时
                        if (examUser.Number == 0)
                        {
                            //只有考勤和考试 没有评估 考试通过后则把考试和评估的部分都加上去
                            if (course.IsPing == 0 && course.IsTest == 1)
                            {
                                double afterlenght = (double)Convert.ToInt32(xueshi.Split(';')[1]) / 100 +
                                                (double)Convert.ToInt32(xueshi.Split(';')[2]) / 100;
                                decimal fortestlenght = (Convert.ToDecimal(afterlenght) * course.CourseLength) + courseorder.GetScore;

                                #region 折算CPA学时
                                if (course.IsCPA == 1)
                                {

                                    Cl_CpaLearnStatus cls = ICpaLearnStatusBL.GetCl_CpaLearnStatusByCourseId(course.Id, CurrentUser.UserId);
                                    if (cls == null)
                                    {
                                        cls = new Cl_CpaLearnStatus();
                                        cls.CourseID = course.Id;
                                        cls.UserID = CurrentUser.UserId;
                                        cls.IsAttFlag = 0;
                                        cls.Progress = 0;
                                        cls.LearnTimes = 0;
                                        //if (course.IsMust == 1)
                                        //{
                                        //    cls.GetLength = fortestlenght * Convert.ToDecimal(0.5);
                                        //}
                                        //if (course.IsMust == 0)
                                        //{
                                        //    cls.GetLength = fortestlenght;
                                        //}
                                        cls.GetLength = fortestlenght;
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
                                        //cls.GetLength = fortestlenght;
                                        //if (course.IsMust == 1)
                                        //{
                                        //    cls.GetLength = fortestlenght * Convert.ToDecimal(0.5);
                                        //}
                                        //if (course.IsMust == 0)
                                        //{
                                        //    cls.GetLength = fortestlenght;
                                        //}
                                        cls.GetLength = fortestlenght;
                                        cls.CpaFlag = 2;
                                        cls.GradeStatus = 1;
                                        ICpaLearnStatusBL.UpdateCPALearnStatusByModel(cls);

                                    }

                                }
                                #endregion

                                CourseOrderBL.UpdateGetScore(course.Id, CurrentUser.UserId, fortestlenght, 1, 2);

                                //考试通过后就把Number标志位改成1 以后在考试则不记学时
                                ExaminationBL.UpdateNumber(student._id, 1);
                            }
                            else //有考勤 评估 考试。考试通过则把考试那部分加上去
                            {
                                //考过后 已得到的分数+考试所占的比例
                                decimal fortestlenght = (course.CourseLength * Convert.ToDecimal((double)Convert.ToInt32(xueshi.Split(';')[1]) / 100)) + courseorder.GetScore;
                                CourseOrderBL.UpdateGetScore(course.Id, CurrentUser.UserId, fortestlenght, 1, 2);

                                //考试通过后就把Number标志位改成1 以后在考试则不记学时
                                ExaminationBL.UpdateNumber(student._id, 1);

                                #region 折算CPA学时
                                if (course.IsCPA == 1)
                                {

                                    Cl_CpaLearnStatus cls = ICpaLearnStatusBL.GetCl_CpaLearnStatusByCourseId(course.Id, CurrentUser.UserId);
                                    if (cls == null)
                                    {
                                        cls = new Cl_CpaLearnStatus();
                                        cls.CourseID = course.Id;
                                        cls.UserID = CurrentUser.UserId;
                                        cls.IsAttFlag = 0;
                                        cls.Progress = 0;
                                        cls.LearnTimes = 0;
                                        if (course.IsMust == 1)
                                        {
                                            cls.GetLength = fortestlenght * Convert.ToDecimal(0.5);
                                        }
                                        if (course.IsMust == 0)
                                        {
                                            cls.GetLength = fortestlenght;
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
                                        //cls.GetLength = fortestlenght;
                                        if (course.IsMust == 1)
                                        {
                                            cls.GetLength = fortestlenght * Convert.ToDecimal(0.5);
                                        }
                                        if (course.IsMust == 0)
                                        {
                                            cls.GetLength = fortestlenght;
                                        }
                                        cls.CpaFlag = 2;
                                        cls.GradeStatus = 1;
                                        ICpaLearnStatusBL.UpdateCPALearnStatusByModel(cls);

                                    }

                                }
                                #endregion
                            }
                        }
                    }

                    //如果当前考试次数等于考试总次数后 改变考试通过LearnStatus
                    if (CoCoursePaper.TestTimes == examUser.TestTimes)
                    {
                        CourseOrderBL.UpdateLearnStatus(course.Id, CurrentUser.UserId, 2);
                    }

                }
                #endregion
            }
            else if (examUser.SourceType == 2)//2为视频下的考试
            {
                #region 一期：视频课程考试
                //var student = ExamTestBL.GetExamUser(euid);
                var CoCoursePaper = ICoCoursePaperBL.GetCo_CourseMainPaper(student.RelationID);
                var course = ICourseBl.GetCo_Course(student.RelationID);
                var exampaper = ExampaperBL.GetExampaper(CoCoursePaper.PaperId);

                //var courseorder = CourseOrderBL.GetCourseById(student.RelationID);

                //如果当前考试分数大于上一次考的分数则修改, 如果小则还是记录以前的分数
                if (examUser.StudentAnswerList.Sum(p => p.GetScore) > oldPaperScore.Sum(p => p.GetScore))
                {
                    ExamTestBL.SaveExamUser(examUser);
                }
                else
                {
                    if (oldPaperScore.Count != 0)
                    {
                        examUser.StudentAnswerList = oldPaperScore;
                    }
                    ExamTestBL.SaveExamUser(examUser);
                }

                var aa = examUser.StudentAnswerList.Sum(p => p.GetScore);
                var tongguofen = (exampaper.ExampaperScore * CoCoursePaper.LevelScore) / 100;
                //var aa = ((double)examUser.StudentAnswerList.Sum(p => p.GetScore) / (double)exampaper.ExampaperScore) * 100;

                nopassnumber = (Convert.ToInt32(CoCoursePaper.TestTimes) - student.TestTimes).ToString();
                //考试没有超过通线 记录课后评估
                if (aa < tongguofen)
                {
                    pass = 0;
                    //考试没过的话
                    if (student.TestTimes == 1)
                    {
                        ICpaLearnStatusBL.UpdateGetCl_CpaLearnStatusByCourseId(course.Id, CurrentUser.UserId, 0, 0);
                    }
                    //关键 如果考试次数达到考试次数还没过 则清空他学习记录 重新在读                    
                }
                else
                {
                    pass = 1;
                    if (examUser.Number == 0)
                    {

                        ICpaLearnStatusBL.UpdateGetCl_CpaLearnStatusByCourseId(course.Id, CurrentUser.UserId,
                                                                                course.CourseLength, 1);
                        examUser.Number = 1;
                        ExaminationBL.UpdateNumber(student._id, 1);

                        #region 折算cpa学时

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
                                cls.GetLength = course.CourseLength * (Convert.ToDecimal(0.5));
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
                        #endregion
                    }
                }

                //通过了之后 达到考试次数就不在清数据..如果达到次数都没过就清次数
                if (examUser.Number == 0 && examUser.TestTimes == CoCoursePaper.TestTimes)
                {
                    var examUserSecond = ExaminationBL.GetSingletbExamSendStudentByCourseIdAndUserId(student.RelationID, CurrentUser.UserId, 2);
                    if (examUser.TestTimes == CoCoursePaper.TestTimes && aa < CoCoursePaper.LevelScore)
                    {

                        var cpa = ICpaLearnStatusBL.GetCl_CpaLearnStatusByCourseId(examUserSecond.RelationID,
                                                                                   CurrentUser.UserId);
                        ICpaLearnStatusBL.DeleteLearn(cpa.Id);
                        ExaminationBL.DeleteExamSendStudentWithByCourseIdAndUserId(course.Id, CurrentUser.UserId, 2);

                    }
                }
                #endregion
            }
            else if (examUser.SourceType == 3)//新进学员考试
            {
                #region 新进学员考试
                //var student = ExamTestBL.GetExamUser(euid);
                //查找这门课程对应的通过线和考试次数
                var CoCoursePaper = inewcoursepaperBl.GetSingleCoursePaper(student.RelationID);
                //查找课程信息 用于查找该课程的学时
                var course = inewcourseBl.GetSingleCourse(student.RelationID);
                //查找试卷总分
                var exampaper = ExampaperBL.GetExampaper(CoCoursePaper.PaperId);

                //获取这个人预定信息 因为集中课程学时是走一步算一步 获取已经获得的学时 
                //var courseorder = CourseOrderBL.GetCourseById(student.RelationID, CurrentUser.UserId);

                //如果当前考试分数大于上一次考的分数则修改, 如果小则还是记录以前的分数
                int fenshju = examUser.StudentAnswerList.Sum(p => p.GetScore);
                //int olderfenshuju=
                if (fenshju > oldPaperScore.Sum(p => p.GetScore))
                {
                    //examUser.PaperScore = fenshju;
                    ExamTestBL.SaveExamUser(examUser);
                }
                else
                {
                    if (oldPaperScore.Count != 0)  // 如果第一次没过就记录当前选项
                    {
                        examUser.StudentAnswerList = oldPaperScore;
                    }
                    ExamTestBL.SaveExamUser(examUser);
                }
                var aa = examUser.StudentAnswerList.Sum(p => p.GetScore);
                var tongguofen = (exampaper.ExampaperScore * CoCoursePaper.LevelScore) / 100;

                //考试没有超过通线 记录课后评估
                //if ((int)aa < CoCoursePaper.LevelScore)
                //if (CoCoursePaper.TestTimes == -1)
                //{
                //    nopassnumber = "无限";
                //}
                //else
                //{
                nopassnumber = (Convert.ToInt32(CoCoursePaper.TestTimes) - student.TestTimes).ToString();
                //}
                if ((int)aa < (int)tongguofen)
                {
                    pass = 0;
                    //考试不过则不做任何操作
                    if (examUser.Number == 0)
                    {

                    }
                }
                else //考试通过
                {
                    pass = 2;
                    //当Number为1的时候 则不在记录
                    if (examUser.Number == 0)
                    {
                        inewcourseorderBl.UpdateLearnStatus(course.Id, CurrentUser.UserId, 2);
                        //考试通过后就把Number标志位改成1 以后在考试则不记学时
                        ExaminationBL.UpdateNumber(student._id, 1);
                    }
                }
                //如果当前考试次数等于考试总次数后 改变考试通过LearnStatus
                if (CoCoursePaper.TestTimes == examUser.TestTimes)
                {
                    CourseOrderBL.UpdateLearnStatus(course.Id, CurrentUser.UserId, 3);
                }
                #endregion
            }
            else if (examUser.SourceType == 4) //部门分所-课程转播考试记录学分
            {
                #region 视频转播：视频转播考试
                //查找课程信息 用于查找该课程的学时
                var course = ICourseBl.GetCo_Course(student.RelationID);

                var courseorder = IDeptran_courseorder.GetCo_CourseMainPaperByCourseIdAndUserId(student.RelationID, CurrentUser.UserId);
                int deptid = iIDepTran_DepartLeaderConfigBL.GetDepartSettingDepartSetID(CurrentUser.UserId);

                //if (courseorder == null)
                //{

                //    DepTran_CourseOrder model = new DepTran_CourseOrder();

                //    model.AttScore = 0;
                //    model.CourseId = student.RelationID;
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

                //    IDeptran_courseorder.AddDepTran_CourseOrder(model);
                //}

                courseorder = IDeptran_courseorder.GetCo_CourseMainPaperByCourseIdAndUserId(student.RelationID, CurrentUser.UserId);
                //var student = ExamTestBL.GetExamUser(euid);
                //查找这门课程对应的通过线和考试次数
                var CoCoursePaper = ICoCoursePaperBL.GetCo_CourseMainPaper(student.RelationID);

                //查找试卷总分
                var exampaper = ExampaperBL.GetExampaper(CoCoursePaper.PaperId);

                //考勤 评估 考试 比例
                var xueshi = course.CourseLengthDistribute;

                //如果当前考试分数大于上一次考的分数则修改, 如果小则还是记录以前的分数
                int fenshju = examUser.StudentAnswerList.Sum(p => p.GetScore);
                //int olderfenshuju=
                if (fenshju > oldPaperScore.Sum(p => p.GetScore))
                {
                    //examUser.PaperScore = fenshju;
                    ExamTestBL.SaveExamUser(examUser);
                }
                else
                {
                    if (oldPaperScore.Count != 0)  // 如果第一次没过就记录当前选项
                    {
                        examUser.StudentAnswerList = oldPaperScore;
                    }
                    ExamTestBL.SaveExamUser(examUser);
                }
                var aa = examUser.StudentAnswerList.Sum(p => p.GetScore);
                var tongguofen = (exampaper.ExampaperScore * CoCoursePaper.LevelScore) / 100;

                //考试没有超过通线 记录课后评估
                //if ((int)aa < CoCoursePaper.LevelScore)
                nopassnumber = (Convert.ToInt32(CoCoursePaper.TestTimes) - student.TestTimes).ToString();
                if ((int)aa < (int)tongguofen)
                {
                    pass = 0;
                    //考试不过则不做任何操作
                    if (examUser.Number == 0)
                    {

                    }
                }
                else //考试通过
                {
                    pass = 2;
                    //当Number为1的时候 则不在记录
                    if (examUser.Number == 0)
                    {

                        //只有考勤和考试 没有评估 考试通过后则把考试和评估的部分都加上去
                        if (course.IsPing == 0 && course.IsTest == 1)
                        {
                            if (courseorder.OrderStatus == 1)
                            {
                                double afterlenght = (double)Convert.ToInt32(xueshi.Split(';')[1]) / 100 +
                                                (double)Convert.ToInt32(xueshi.Split(';')[2]) / 100;
                                decimal fortestlenght = (Convert.ToDecimal(afterlenght) * course.CourseLength);

                                IDeptran_courseorder.UpdateWhere(" set ExamScore=" + fortestlenght + ",PassStatus=1 where CourseId=" + course.Id + " and UserId=" + CurrentUser.UserId);
                            }
                            //CourseOrderBL.UpdateGetScore(course.Id, CurrentUser.UserId, fortestlenght, 1, 2);

                            //考试通过后就把Number标志位改成1 以后在考试则不记学时
                            ExaminationBL.UpdateNumber(student._id, 1);
                        }
                        else //有考勤 评估 考试。考试通过则把考试那部分加上去
                        {
                            if (courseorder.OrderStatus == 1)
                            {
                                //考过后 已得到的分数+考试所占的比例
                                decimal fortestlenght = (course.CourseLength * Convert.ToDecimal((double)Convert.ToInt32(xueshi.Split(';')[1]) / 100));


                                IDeptran_courseorder.UpdateWhere(" set ExamScore=" + fortestlenght + ",PassStatus=1 where CourseId=" + course.Id + " and UserId=" + CurrentUser.UserId);
                                //CourseOrderBL.UpdateGetScore(course.Id, CurrentUser.UserId, fortestlenght, 1, 2);
                            }
                            //考试通过后就把Number标志位改成1 以后在考试则不记学时
                            ExaminationBL.UpdateNumber(student._id, 1);


                        }
                    }
                }
                //如果当前考试次数等于考试总次数后 改变考试通过LearnStatus
                if (CoCoursePaper.TestTimes == examUser.TestTimes)
                {
                    CourseOrderBL.UpdateLearnStatus(course.Id, CurrentUser.UserId, 3);
                }
                #endregion
            }
            else if (examUser.SourceType == 5)
            {
                #region 部门分所:部门分所考试
                //var student = ExamTestBL.GetExamUser(euid);
                //查找这门课程对应的通过线和考试次数
                var CoCoursePaper = _IDep_coursePaper.GetCo_CourseMainPaper(student.RelationID);
                //查找课程信息 用于查找该课程的学时
                var course = _IDep_Course.GetCo_Course(student.RelationID);
                //查找试卷总分
                var exampaper = ExampaperBL.GetExampaper(CoCoursePaper.PaperId);


                //获取这个人预定信息 因为集中课程学时是走一步算一步 获取已经获得的学时 
                var courseorder = _IDep_courseOrder.GetCourseById(student.RelationID, CurrentUser.UserId);

                //如果当前考试分数大于上一次考的分数则修改, 如果小则还是记录以前的分数
                int fenshju = examUser.StudentAnswerList.Sum(p => p.GetScore);
                //int olderfenshuju=
                if (fenshju > oldPaperScore.Sum(p => p.GetScore))
                {
                    //examUser.PaperScore = fenshju;
                    ExamTestBL.SaveExamUser(examUser);
                }
                else
                {
                    if (oldPaperScore.Count != 0)  // 如果第一次没过就记录当前选项
                    {
                        examUser.StudentAnswerList = oldPaperScore;
                    }
                    ExamTestBL.SaveExamUser(examUser);
                }

                //var xueshi = AllSystemConfigs.Find(p => p.ConfigType == 24);
                var xueshi = course.CourseLengthDistribute;
                var aa = examUser.StudentAnswerList.Sum(p => p.GetScore);
                var tongguofen = (exampaper.ExampaperScore * CoCoursePaper.LevelScore) / 100;
                //var aa = ((double)examUser.StudentAnswerList.Sum(p => p.GetScore) / (double)exampaper.ExampaperScore)*100;

                //考试没有超过通线 记录课后评估
                //if ((int)aa < CoCoursePaper.LevelScore)
                if (CoCoursePaper.TestTimes == -1)
                {
                    nopassnumber = "无限";
                }
                else
                {
                    nopassnumber = (Convert.ToInt32(CoCoursePaper.TestTimes) - student.TestTimes).ToString();
                }
                if ((int)aa < (int)tongguofen)
                {
                    pass = 0;
                    //考试不过则不做任何操作
                    if (examUser.Number == 0)
                    {

                    }
                }
                else //考试通过
                {
                    //begin  全部折算CPA信息--by gecc (2013-12-5)
                    var depCPAlist = depCpaLearn.GetDepCPALearn();
                    var depCoursemodel = _IDep_courseOrder.GetCo_CourseMainPaperByCourseIdAndUserId(course.Id, CurrentUser.UserId);
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


                    pass = 1;
                    //当Number为1的时候 则不在记录学时
                    if (examUser.Number == 0)
                    {
                        //只有考勤和考试 没有评估 考试通过后则把考试和评估的部分都加上去
                        if (course.IsPing == 0 && course.IsTest == 1)
                        {
                            double afterlenght = (double)Convert.ToInt32(xueshi.Split(';')[1]) / 100 +
                                            (double)Convert.ToInt32(xueshi.Split(';')[2]) / 100;
                            decimal fortestlenght = (Convert.ToDecimal(afterlenght) * course.CourseLength);

                            _IDep_courseOrder.UpdateWhere(" set ExamScore=" + fortestlenght + ",PassStatus=1 where CourseId=" + course.Id + " and UserId=" + CurrentUser.UserId);

                            //CourseOrderBL.UpdateGetScore(course.Id, CurrentUser.UserId, fortestlenght, 1, 2);

                            //考试通过后就把Number标志位改成1 以后在考试则不记学时
                            ExaminationBL.UpdateNumber(student._id, 1);

                            #region 折算CPA--by gecc (2013-12-5)

                            //判断是否是CPA人员 and 是否开放CPA折算
                            if (cpatemp == true && CurrentUser.IsCPA == 1)
                            {
                                var depCPA = depCPAlist.Where(p => p.CourseID == course.Id && p.UserID == CurrentUser.UserId).FirstOrDefault();
                                if (depCPA != null)
                                {
                                    depCPA.GetLength = depCPA.GetLength + fortestlenght;
                                    depCpaLearn.UpdateDepCPALearn(depCPA);
                                }
                                else
                                {
                                    depCpaLearn.InsertDepCPALearn(new Dep_CpaLearnStatus
                                    {
                                        CourseID = course.Id,
                                        UserID = CurrentUser.UserId,
                                        CpaFlag = 0,
                                        GetLength = fortestlenght,
                                        DepartSetId = depCoursemodel.DepartSetId
                                    });
                                }
                            }

                            #endregion Model
                        }
                        else //有考勤 评估 考试。考试通过则把考试那部分加上去
                        {
                            //考过后 已得到的分数+考试所占的比例
                            decimal fortestlenght = (course.CourseLength * Convert.ToDecimal((double)Convert.ToInt32(xueshi.Split(';')[1]) / 100));
                            //CourseOrderBL.UpdateGetScore(course.Id, CurrentUser.UserId, fortestlenght, 1, 2);

                            _IDep_courseOrder.UpdateWhere(" set ExamScore=" + fortestlenght + ",PassStatus=1 where CourseId=" + course.Id + " and UserId=" + CurrentUser.UserId);


                            //考试通过后就把Number标志位改成1 以后在考试则不记学时
                            ExaminationBL.UpdateNumber(student._id, 1);

                            #region 折算CPA--by gecc (2013-12-5)

                            //判断是否是CPA人员 and 是否开放CPA折算
                            if (cpatemp == true && CurrentUser.IsCPA == 1)
                            {
                                var depCPA = depCPAlist.Where(p => p.CourseID == course.Id && p.UserID == CurrentUser.UserId).FirstOrDefault();
                                if (depCPA != null)
                                {
                                    depCPA.GetLength = depCPA.GetLength + fortestlenght;
                                    depCpaLearn.UpdateDepCPALearn(depCPA);
                                }
                                else
                                {
                                    depCpaLearn.InsertDepCPALearn(new Dep_CpaLearnStatus
                                    {
                                        CourseID = course.Id,
                                        UserID = CurrentUser.UserId,
                                        CpaFlag = 0,
                                        GetLength = fortestlenght,
                                        DepartSetId = depCoursemodel.DepartSetId
                                    });
                                }
                            }

                            #endregion Model

                        }
                    }


                    //如果当前考试次数等于考试总次数后 改变考试通过LearnStatus
                    if (CoCoursePaper.TestTimes == examUser.TestTimes)
                    {
                        CourseOrderBL.UpdateLearnStatus(course.Id, CurrentUser.UserId, 2);
                    }

                }
                #endregion
            }
            else
            {
                ExamTestBL.SaveExamUser(examUser);
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = 0, courseid = examUser.RelationID, way = examUser.SourceType, pass = pass, nopassnumber = nopassnumber }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        ///     单题得分
        /// </summary>
        /// <param name="qu">试题信息</param>
        /// <param name="userAnswer">学员答案</param>
        /// <param name="score">本题分值</param>
        /// <returns></returns>
        public int GetScore(tbQuestion qu, string userAnswer, int score)
        {

            userAnswer = string.Join(",", userAnswer.Split(',').OrderBy(p => p).ToList());
            int getScore = 0; //是否得分（0：未得分；1：得分）
            switch (qu.QuestionType)
            {
                case 1: //问答
                    {
                        //正确答案关键字
                        string[] rightKeys = qu.QuestionAnswerKeys.Split(' ');
                        int count = 0; //答对个数
                        rightKeys.ToList().ForEach(p => { count += userAnswer.Contains(p) ? 1 : 0; });
                        getScore = score * count / rightKeys.Count();
                    }
                    break;
                case 2: //单选
                case 3: //多选
                    {
                        string rightAnswer = "";
                        qu.QuestionAnswer.ForEach(
                            p =>
                            {
                                rightAnswer += p.AnswerFlag == 1
                                                   ? (rightAnswer == "" ? (p.Order + "") : ("," + p.Order))
                                                   : "";
                            });
                        getScore = userAnswer == rightAnswer ? score : 0;
                    }
                    break;
                case 4: //判断
                    {
                        string rightAnswer = qu.QuestionAnswer[0].Answer;
                        getScore = userAnswer == rightAnswer ? score : 0;
                    }
                    break;
                case 5: //填空
                    {
                        //正确答案关键字
                        string[] rightKeys = qu.QuestionAnswerKeys.Split(' ');
                        //学员答案关键字
                        string[] uanswer = userAnswer.Split(new[] { "##**##" }, StringSplitOptions.None);
                        int count = 0; //答对个数
                        for (int i = 0; i < rightKeys.Count(); i++)
                        {
                            if (uanswer.Count() > i && rightKeys[i] == uanswer[i])
                                count++;
                        }
                        getScore = score * count / rightKeys.Count();
                    }
                    break;
                case 6: //情景
                    {
                        switch (qu.QuestionAnswer[0].AnswerType)
                        {
                            case 0:
                                {
                                    //正确答案关键字
                                    string[] rightKeys = qu.QuestionAnswerKeys.Split(' ');
                                    int count = 0; //答对个数
                                    rightKeys.ToList().ForEach(p => { count += userAnswer.Contains(p) ? 1 : 0; });
                                    getScore = score * count / rightKeys.Count();
                                }
                                break;
                            case 1:
                            case 2:
                                {
                                    string rightAnswer = "";
                                    qu.QuestionAnswer.ForEach(
                                        p =>
                                        {
                                            rightAnswer += p.AnswerFlag == 1
                                                               ? (rightAnswer == ""
                                                                      ? (p.Order + "")
                                                                      : ("," + p.Order))
                                                               : "";
                                        });
                                    getScore = userAnswer == rightAnswer ? score : 0;
                                }
                                break;
                        }
                    }
                    break;
            }
            return getScore;
        }

        #endregion
    }
}