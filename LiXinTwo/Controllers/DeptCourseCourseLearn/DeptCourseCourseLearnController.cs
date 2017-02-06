using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.DepTranManage;
using LiXinInterface.CourseManage;
using LiXinInterface.Survey;
using LiXinInterface.Examination;
using LiXinInterface.CourseLearn;
using System.Text.RegularExpressions;
using LiXinModels.DepTranManage;
using LiXinModels.CourseManage;
using LiXinModels.DepTranAttendce;
using LiXinInterface.DepCourseManage;

namespace LiXinControllers
{
    public partial class DeptCourseCourseLearnController : BaseController
    {
        protected IDepTran_CourseOrder iDepTran_CourseOrder;
        protected IDepTran_CourseOpen iDepTran_CourseOpen;
        //protected ICourseOrder courseOrderBL;
        protected ICo_Course courseBL;
        protected ISurveyExampaper ISurveyExampaperBL;
        protected ICo_CoursePaper ICo_CoursePaperBL;
        protected ICo_CourseResource ICoCourseResourceBL;
        protected IDepTran_DepartLeaderConfig iIDepTran_DepartLeaderConfigBL;
        protected IDepTran_Attendce IDepTran_Attendce;
        protected IExampaper exampaperBL;
        protected IExamination IExaminationrBL;
        protected IDepMyScore IDepMyScore;
        protected IDepTran_Attendce DTAttendceBl;
        protected IExamTest ExamTest;
        public DeptCourseCourseLearnController(IDepTran_CourseOrder _iDepTran_CourseOrder, ICo_Course _courseBL,IDepTran_Attendce _DTAttendceBl,
            ISurveyExampaper _ISurveyExampaperBL, ICo_CoursePaper _ICo_CoursePaperBL, IExampaper _IExampaperBL,
            ICo_CourseResource _ICoCourseResourceBL, IDepTran_DepartLeaderConfig _iIDepTran_DepartLeaderConfigBL,
            IExamination _IExaminationrBL, IDepTran_Attendce _IDepTran_Attendce, IDepMyScore _IDepMyScore, IExamTest examTest, IDepTran_CourseOpen _iDepTran_CourseOpen)
        {
            iDepTran_CourseOrder = _iDepTran_CourseOrder;
            courseBL = _courseBL;
            ISurveyExampaperBL = _ISurveyExampaperBL;
            ICo_CoursePaperBL = _ICo_CoursePaperBL;
            exampaperBL = _IExampaperBL;
            ICoCourseResourceBL = _ICoCourseResourceBL;
            iIDepTran_DepartLeaderConfigBL = _iIDepTran_DepartLeaderConfigBL;
            IExaminationrBL = _IExaminationrBL;
            IDepTran_Attendce = _IDepTran_Attendce;
            DTAttendceBl = _DTAttendceBl;
            IDepMyScore = _IDepMyScore;
            ExamTest = examTest;
            iDepTran_CourseOpen = _iDepTran_CourseOpen;
        }

        #region 呈现

     


        public ActionResult MyCourseSubscribe()
        {
            return View();
        }

        public ActionResult CourseDetail(int id)
        {
            var Course = courseBL.GetCo_Course(id, CurrentUser.UserId);       
            ViewBag.Course = Course;
            int deptid = iIDepTran_DepartLeaderConfigBL.GetDepartSettingDepartSetID(CurrentUser.UserId);
            ViewBag.orderstatus = iDepTran_CourseOrder.GetYuDing(id, deptid);
            return View();
        }

        public ActionResult AfterCourse(int id)
        {

            //ViewBag.backurl = backurl;

            var courselist = courseBL.GetCo_Course(id);

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

        public ActionResult CourseMain(int id)
        {

            //var config = iIDepTran_DepartLeaderConfigBL.GetDepartCourseLimitNumber(CurrentUser.UserId);
            //var course = courseBL.GetCo_Course(id);
            //if (config.Count > 0)
            //{
            //    course.NumberLimited = config[0].LimitNumber;
            //}
            int deptid = iIDepTran_DepartLeaderConfigBL.GetDepartSettingDepartSetID(CurrentUser.UserId);
            var course = iDepTran_CourseOrder.GetCo_CourseLimitNumber(id, deptid);

            ViewBag.Course = course;

            var CourseResourse = ICoCourseResourceBL.GetList(id);
            ViewBag.CourseResourse = CourseResourse;
            return View();
        }

        public ActionResult OnlineCourse(int id)
        {

            Co_CoursePaper CoCoursePaper = ICo_CoursePaperBL.GetCo_CourseMainPaper(id);
            var course = courseBL.GetCo_Course(id);

            if (CoCoursePaper != null && course.IsTest == 1)
            {
                var exap = IExaminationrBL.GetExamination(CoCoursePaper.PaperId);
                ViewBag.exap = exap;
                // var examsendstudent = IExaminationrBL.GetExamSendStudent(exap.PaperID);
                var examsendstudent = IExaminationrBL.GetSingletbExamSendStudentByCourseIdAndUserId(id,
                                                                                                    CurrentUser.UserId, 1);
                var exampapger = exampaperBL.GetExampaper(CoCoursePaper.PaperId);
                ViewBag.exampapger = exampapger;
                ViewBag.examsendstudent = examsendstudent;
            }
            ViewBag.Co_CoursePaper = CoCoursePaper;
            ViewBag.courseid = id;
            ViewBag.course = course;

            return View();
        }


        #endregion

        #region 获取预定列表
        /// <summary>
        /// 获取预定列表
        /// </summary>
        /// <param name="course">课程名</param>
        /// <param name="must">选修：1，必修：0</param>
        /// <param name="apply">查看预定状态 是：1，否：0</param>
        /// <param name="courseType">课程类别 内部培训：1，社会招聘：2，新进员工：3，实习生：4</param>
        /// <param name="subscribeType">预定状态</param>        
        /// <param name="start">课程开始时间</param>
        /// <param name="end">课程结束时间</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        public JsonResult GetMyCourseSubscribeList(string course, string must, string apply, int courseType, int subscribeType, DateTime? start, DateTime? end, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {

            int deptid = iIDepTran_DepartLeaderConfigBL.GetDepartSettingDepartSetID(CurrentUser.UserId);
            string where = " 1=1";
            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and aa.CourseName like '%{0}%'", course.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(must))
            {
                if (!must.Contains(","))
                {
                    where += string.Format(" and aa.IsMust in ({0})", Convert.ToInt32(must));
                }
            }

            //if (!string.IsNullOrWhiteSpace(apply))
            //{
            //    //if (!apply.Contains(","))
            //    //{
            //    //    if (apply == "1")
            //    //        where += " and (Co_Course.IsOrder = 1 or Co_Course.IsOrder = 3 )";
            //    //    if (apply == "0")
            //    //        where += " and (Co_Course.IsOrder = 0 or Co_Course.IsOrder = 2 )";
            //    //}
            //    //可以预定   StopOrderFlag 0：是：1：否    IsOrder 0:无；1：预定；2：指定；3：两者都有
            //    if (apply == "1")
            //    {
            //        where += " and (getdate() < StartTime) and Co_Course.IsOrder in(1,3) and Co_Course.StopOrderFlag=0";
            //    }
            //    if (apply == "2")
            //    {
            //        where += "and ( (cl_courseorder.userid is null or Cl_CourseOrder.orderstatus = 0)and Co_Course.StartTime < '" + DateTime.Now + "' or  Co_Course.IsOrder in(0,2) or Co_Course.StopOrderFlag=1)";
            //    }
            //    //if (apply == "3")
            //    //{
            //    //    where += "and Co_Course.StartTime >'" + DateTime.Now +"'";// and Co_Course.IsOrder in(1,3)";
            //    //    findnum = "NumberLimited<=HasEntered";
            //    //}

            //}


            if (start.HasValue)
                where += string.Format(" and aa.StartTime >= '{0}'", start.Value);
            if (end.HasValue)
                where += string.Format(" and aa.EndTime <= '{0}'", end.Value.AddDays(1));

            if (courseType != 99)
            {
                //where += string.Format(" and aa.Sort= '{0}'", courseType);
                where += " and charindex('" + courseType + "',aa.Sort)>0";
            }

            //if (ApprovalFlag != 0)
            //{
            //    if (ApprovalFlag == 0)
            //    {
            //        where += string.Format(" and  aa.StartTime >= '{0}' and  aa.StopOrderFlag=0", ApprovalFlag);
            //    }
            //    else if (ApprovalFlag==1)
            //    {
            //        where += string.Format(" and (aa.StartTime < '{0}' ) ", ApprovalFlag);
            //    }
            //    else if (ApprovalFlag == 2)
            //    {
            //        where += string.Format("", ApprovalFlag);
            //    }
            //}
                

            //var list = iDepTran_CourseOrder.GetCourseList(CurrentUser.UserId," Co_Course.id in (select CourseId from DepTran_CourseOpen where DepartId=" + CurrentUser.DeptId + ")");
            int limit=0;

            var list = iDepTran_CourseOrder.GetNewCourseList(out limit, CurrentUser.UserId, deptid, where, (pageIndex - 1) * pageSize, pageSize);

            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    Id = item.CourseId,                    
                    courseOrderId = item.courseOrderId,
                    CourseName = item.CourseName.HtmlXssEncode(),
                    CourseLength = item.CourseLength,
                    IsMustStr = item.IsMustStr,
                    CourseTime = item.CourseTime,
                    TeacherName = item.TeacherName.HtmlXssEncode(),
                    RoomName = item.RoomName.HtmlXssEncode(),
                    HasEntered = item.HasEntered,
                    NumberLimited = item.NumberLimited,
                    OrderStr = item.OrderStr,
                    //MyDoStatus = item.MyDoStatus,
                    //MyStatusStr = item.MyStatusStr,
                    //YearPlan = item.YearPlan,
                    SortStr = item.SortStr,
                    //IsCPA = item.IsCPA == 0 ? "否" : "是",
                    CourseTimes = item.CourseTimesOrder + "/" + item.CourseTimes,
                    Yuding = iDepTran_CourseOrder.GetYuDing(item.CourseId, deptid),
                    //IsOpenSubInt = CurrentUser.IsMain == item.IsOpenSub ? 0 : 1,
                    //inopenlevel = item.OpenLevel == null ? 0 : item.OpenLevel.Contains(CurrentUser.TrainGrade) ? 1 : 0,
                    //noopenlevelandfensuo = item.OpenLevel == null ? 0 : item.IsOpenSub == CurrentUser.IsMain ? 1 : 0,
                    //IsMain = CurrentUser.IsMain,
                    //IsOrder = item.IsOrder,
                    StopOrderFlag = item.StopOrderFlag
                    //IsTest = item.IsTest,
                    //IsOrNoOrLine = FIsOrNoOrLine(item.DeptHasEntered, item.SuccessEntered, item.HasEntered, item.NumberLimited, item.StartTime, item.StopDucueFlag, item.Way, item.StopOrderFlag, item.IsOrder)

                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = limit
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 预定按钮
        public JsonResult Subscribe(int courseId)
        {
            int page = 1;
            if (Session["mcstpage"] != null)
            {
                string sess = Session["mcstpage"].ToString();
                string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                page = Convert.ToInt32(att[0]);
            }
            int deptid = iIDepTran_DepartLeaderConfigBL.GetDepartSettingDepartSetID(CurrentUser.UserId);
            var error = "";
            int num = 0;
            //var flag = courseOrderBL.GetCanSignup(out num, courseId, CurrentUser.UserId,AllSystemConfigs.Find(p => p.ConfigType == 21).ConfigValue.GetDouble());

            var flag = iDepTran_CourseOrder.GetYuDing(courseId, deptid);
            
            switch (flag)
            {
                case 0:
                    error = "预订失败！";
                    break;
                case 3:
                    error = "预订成功！";
                    break;              
                //case 3:
                //    error = "预订失败！";
                //    break;
                case 1:
                    error = "预订失败，排队已关闭！";
                    break;
                case 2:
                    error = "预订失败，报名已关闭！";
                    break;
                default:
                    break;
            }

            if (flag == 3)
            {
                //int deptid = iIDepTran_DepartLeaderConfigBL.GetDepartSettingDepartSetID(CurrentUser.UserId);
                var list= iDepTran_CourseOrder.GetCo_CourseMainPaperByCourseIdAndUserId(courseId, CurrentUser.UserId);
                if (list == null)
                {
                    DepTran_CourseOrder model = new DepTran_CourseOrder();

                    model.AttScore = 0;
                    model.CourseId = courseId;
                    model.GetScore = 0;
                    model.IsAppoint = 0;
                    model.LearnStatus = 0;
                    model.OrderStatus = 1;
                    model.OrderTime = DateTime.Now;
                    model.PassStatus = 0;
                    model.UserId = CurrentUser.UserId;
                    model.EvlutionScore = 0;
                    model.ExamScore = 0;
                    model.DepartSetId = deptid;
                    model.OrderTimes = 0;

                    iDepTran_CourseOrder.AddDepTran_CourseOrder(model);


                    var attendce = IDepTran_Attendce.GetDepAttendce(courseId, CurrentUser.UserId);

                    if (attendce == null)
                    {
                        DepTran_Attendce deptran = new DepTran_Attendce();
                        deptran.UserId = CurrentUser.UserId;
                        deptran.CourseId = courseId;
                        deptran.StartTime = DateTime.Now;
                        deptran.EndTime = DateTime.Now;
                        deptran.Status = 0;
                        deptran.ApprovalFlag = 0;
                        deptran.Reason = "";
                        deptran.DepartSetId = deptid;
                        
                        IDepTran_Attendce.AddDepAttend(deptran);
                    }
                }
                else
                {
                    DepTran_CourseOrder model = new DepTran_CourseOrder();
                    model.Id = list.Id;
                    model.AttScore = 0;
                    model.CourseId = courseId;
                    model.GetScore = 0;
                    model.IsAppoint = 0;
                    model.LearnStatus = 0;
                    model.OrderStatus = 1;
                    model.OrderTime = DateTime.Now;
                    model.PassStatus = 0;
                    model.UserId = CurrentUser.UserId;
                    model.EvlutionScore = 0;
                    model.DepartSetId = deptid;
                    model.ExamScore = 0;
                    model.OrderTimes = list.OrderTimes;

                    iDepTran_CourseOrder.UpdateDepTran_CourseOrder(model);
                }

            }
            return Json(new
            {
                result = flag,
                indexpage = page,
                content = error
            }, JsonRequestBehavior.AllowGet);
        }
#endregion

    }
}
