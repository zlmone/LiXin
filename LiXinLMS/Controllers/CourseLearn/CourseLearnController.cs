using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinInterface.CourseLearn;
using LiXinCommon;
using LiXinInterface.SystemManage;
using LiXinModels.CourseLearn;
using LiXinModels.User;
using LiXinModels;
using System.Data;
using LiXinInterface.CourseManage;
using LiXinInterface.Survey;
using LiXinModels.CourseManage;
using System.Text.RegularExpressions;
using LiXinInterface.Examination;
using LiXinInterface.User;
using System.Xml;
using System.Collections;

namespace LiXinControllers
{
    public partial class CourseLearnController : BaseController
    {
        protected ICourseOrder courseOrderBL;
        protected ICo_Course courseBL;
        protected ISurveyQuestion surveyQuestionBL;
        protected ICo_CourseResource courseResourceBL;
        protected ICo_CoursePaper coursePaperBL;
        protected IExamination examinationBL;
        protected IExampaper exampaperBL;
        protected IUser userBL;
        protected ISurveyExampaper surveyExampaperBL;
        protected ISys_Group _sys_GroupBL;
        protected ICl_CpaLearnStatus _cpaLearnStatusBL;
        protected IDepartment deptBL;
        protected ISys_Leader leaderBL;

        public CourseLearnController(ICourseOrder _courseOrderBL, ICo_Course _courseBL, ISurveyQuestion _surveyQuestionBL, ICo_CourseResource _courseResourceBL, ICo_CoursePaper _coursePaperBL, IExamination _examinationBL, IExampaper _exampaperBL, IUser _userBL, ISurveyExampaper _surveyExampaperBL, ISys_Group sys_GroupBL, ICl_CpaLearnStatus cpaLearnStatusBL, IDepartment _deptBL, ISys_Leader _leaderBL)
        {
            courseOrderBL = _courseOrderBL;
            courseBL = _courseBL;
            surveyQuestionBL = _surveyQuestionBL;
            courseResourceBL = _courseResourceBL;
            coursePaperBL = _coursePaperBL;
            examinationBL = _examinationBL;
            exampaperBL = _exampaperBL;
            userBL = _userBL;
            surveyExampaperBL = _surveyExampaperBL;
            _sys_GroupBL = sys_GroupBL;
            _cpaLearnStatusBL = cpaLearnStatusBL;
            deptBL = _deptBL;
            leaderBL = _leaderBL;
        }

        #region 我的课程预订(学员)

        public ActionResult MyCourseSubscribe(string p, string cp, int showflag = 0)
        {
            ViewBag.CPA = CurrentUser.CPA;
            ViewBag.showflag = showflag;

            ViewBag.page = 1;
            ViewBag.course = "请输入搜索内容";
            ViewBag.cType = 99;
            ViewBag.sType = 99;
            ViewBag.iscpa = 99;
            if (p != null && p != "" && p == "1")
            {
                if (Session["mcstpage"] != null)
                {
                    string sess = Session["mcstpage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);

                    ViewBag.page = att[0];
                    ViewBag.course = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.must = att[2];
                    ViewBag.apply = att[3];
                    ViewBag.cType = att[4];
                    ViewBag.sType = att[5];
                    ViewBag.iscpa = att[6];
                    ViewBag.start = att[7];
                    ViewBag.end = att[8];
                }
            }

            ViewBag.cpage = 1;
            ViewBag.ccourse = "请输入搜索内容";
            ViewBag.csType = 99;
            if (cp != null && cp != "" && cp == "1")
            {
                if (Session["ccstpage"] != null)
                {
                    string csess = Session["ccstpage"].ToString();
                    string[] catt = Regex.Split(csess, "㉿", RegexOptions.IgnoreCase);

                    ViewBag.cpage = catt[0];
                    ViewBag.ccourse = catt[1] == "" ? "请输入搜索内容" : catt[1];
                    ViewBag.csType = catt[2];
                    ViewBag.cstart = catt[3];
                    ViewBag.cend = catt[4];
                }
            }
            return View();
        }

        /// <summary>
        /// CPA课程报名详情
        /// </summary>
        public ActionResult CpaCourseApplyDetail(int id)
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.Id = id;
            return View();
        }

        public ActionResult CourseDetail(int id)
        {
            var coursepaperstatus = 1;
            var model = courseOrderBL.GetCourseById(id, CurrentUser.UserId);
            var course = courseBL.GetCo_Course(id);
            if (!string.IsNullOrWhiteSpace(model.SurveyPaperId))
            {
                var arr = model.SurveyPaperId.Split(';');
                if (!string.IsNullOrWhiteSpace(arr[0]))
                    model.AfterCourseAssess = surveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[0].Split(',')[1]));
                if (!string.IsNullOrWhiteSpace(arr[1]))
                    model.AfterCourseTeacherExam =
                        surveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[1].Split(',')[1]));
            }
            model.CourseResourselist = courseResourceBL.GetList(id);
            var paper = coursePaperBL.GetCo_CourseMainPaper(id);
            if (paper != null && course.IsTest == 1)
            {
                var exampapger = exampaperBL.GetExampaper(paper.PaperId);
                if (exampapger != null)
                {
                    paper.PaperName = exampapger.ExampaperTitle;
                    paper.TotalScore = exampapger.ExampaperScore;
                    //coursepaperstatus = exampapger.Status;
                }
            }
            model.CoursePaper = paper;
            ViewBag.model = model;
            ViewBag.coursepaperstatus = course.IsTest;

            //ViewBag.IsOpenSubInt=CurrentUser.IsMain==item.IsOpenSub?0:1,
            ViewBag.inopenlevel = course.OpenLevel == null ? 0 : course.OpenLevel.Contains(CurrentUser.TrainGrade) ? 1 : 0;
            ViewBag.noopenlevelandfensuo = course.OpenLevel == null ? 0 : course.IsOpenSub == CurrentUser.IsMain ? 1 : 0;
            ViewBag.IsMain = CurrentUser.IsMain;

            var isbool = true;

            //是否对本人所在群组开放和是否对本人培训级别进行开放
            var list = _sys_GroupBL.GetGroupList(CurrentUser.UserId, 0).Select(p => p.GroupId).ToList();
            var ismygrade = string.IsNullOrEmpty(model.OpenLevel) || ("," + model.OpenLevel + ",").IndexOf("," + CurrentUser.TrainGrade + ",", System.StringComparison.Ordinal) >= 0; //是否对我的级别开放

            var ismydept = string.IsNullOrEmpty(model.OpenDepartObject);
            var ismydept1 = !string.IsNullOrEmpty(model.OpenDepartObject) && ("," + model.OpenDepartObject + ",").IndexOf("," + CurrentUser.DeptId + ",", System.StringComparison.Ordinal) >= 0; //是否对我的部门开放
            var aflag = string.IsNullOrEmpty(model.OpenGroupObject);
            var aflag1 = !string.IsNullOrEmpty(model.OpenGroupObject) && list.Any(i => ("," + model.OpenGroupObject + ",").IndexOf("," + i + ",", System.StringComparison.Ordinal) >= 0); //我是否在开放的群组

            if (!(ismygrade && ((ismydept && aflag) || ismydept1 || aflag1)))
            {
                isbool = false;
            }
            //是否已预订过
            if (isbool)
            {
                var list1 = _cpaLearnStatusBL.GetCpaCourse(string.Format(" CourseID={0} and UserID={1} ", model.Id, CurrentUser.UserId));
                isbool = list1.Count == 0;
            }


            ViewBag.Predit = isbool;

            return View();
        }

        public JsonResult GetMyCourseSubscribeList(string course, string must, string apply, int courseType, int subscribeType, int iscpa, DateTime? start, DateTime? end, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {

            if (Session["mcstpage"] != null)
            {
                Session.Remove("mcstpage");
            }
            Session["mcstpage"] = pageIndex + "㉿" + course + "㉿" + must + "㉿" + apply + "㉿" + courseType + "㉿" + subscribeType + "㉿" + iscpa + "㉿" + start + "㉿" + end;
            int totalCount = 0;

            string IsOpenSub = "";
            string openlevel = "";

            string findnum = " 1=1";

            if (CurrentUser.IsMain == 1)
            {
                IsOpenSub = " or IsOpenSub=1";
                //openlevel = "or Co_Course.OpenLevel = '' or Co_Course.OpenLevel is null";
            }


            string where = string.Format(@" Co_Course.IsDelete = 0 
and Co_Course.Publishflag = 1 
and Co_Course.CourseFrom = 2 
and Co_Course.Way = 1
and ((
        (
			(select count(*) from Sys_User where UserId={0} and TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)))>0  
			 {4}
		) 
		and 
		(
			(
				
				(Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null) and (Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null )
			) 
			or 
				(select count(*) from Sys_User where UserId={0} and deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject)))>0  
			or
			(select count(*) from Sys_GroupUser where UserId={0} and groupid in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))>0
		) 
    ) or (Cl_CourseOrder.UserId = {0} and  Cl_CourseOrder.orderstatus > 0) {3}) 
and (Co_Course.Sort like '%{1}%' or {1} = 99)
and (Co_Course.IsCPA = {2} or {2} = 99)
", CurrentUser.UserId, courseType, iscpa, IsOpenSub, openlevel);

            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(must))
            {
                if (!must.Contains(","))
                {
                    where += string.Format(" and Co_Course.IsMust in ({0})", Convert.ToInt32(must));
                }
            }
            if (!string.IsNullOrWhiteSpace(apply))
            {
                //if (!apply.Contains(","))
                //{
                //    if (apply == "1")
                //        where += " and (Co_Course.IsOrder = 1 or Co_Course.IsOrder = 3 )";
                //    if (apply == "0")
                //        where += " and (Co_Course.IsOrder = 0 or Co_Course.IsOrder = 2 )";
                //}
                //可以预定   StopOrderFlag 0：是：1：否    IsOrder 0:无；1：预定；2：指定；3：两者都有
                if (apply == "1")
                {
                    where += " and (getdate() < StartTime) and Co_Course.IsOrder in(1,3) and Co_Course.StopOrderFlag=0";
                }
                if (apply == "2")
                {
                    where += "and ( (cl_courseorder.userid is null or Cl_CourseOrder.orderstatus = 0)and Co_Course.StartTime < '" + DateTime.Now + "' or  Co_Course.IsOrder in(0,2) or Co_Course.StopOrderFlag=1)";
                }
                //if (apply == "3")
                //{
                //    where += "and Co_Course.StartTime >'" + DateTime.Now +"'";// and Co_Course.IsOrder in(1,3)";
                //    findnum = "NumberLimited<=HasEntered";
                //}

            }
            if (start.HasValue)
                where += string.Format(" and Co_Course.StartTime >= '{0}'", start.Value);
            if (end.HasValue)
                where += string.Format(" and Co_Course.EndTime <= '{0}'", end.Value.AddDays(1));
            switch (subscribeType)
            {
                case 0://退订成功
                    //where += " and Cl_CourseOrder.orderstatus = 0 ";
                    where += " and Cl_CourseOrder.orderstatus = 0 and StopOrderFlag=0 and Co_Course.StartTime>='" + DateTime.Now + "'";
                    break;
                case 1://预订成功[排队中]
                    //where += " and cl_courseorder.IsAppoint = 0 and Cl_CourseOrder.orderstatus = 1 and (Cl_CourseOrder.IsLeave = 0 or (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1) or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime]))";
                    //where += " and cl_courseorder.orderstatus = 2";
                    where += " and cl_courseorder.orderstatus = 2 and StopOrderFlag=0";
                    break;
                case 2://请假成功
                    //where += " and Cl_CourseOrder.isleave = 1 and  Cl_CourseOrder.ApprovalFlag = 1 and Cl_CourseOrder.[ApprovalDateTime] <= Cl_CourseOrder.[ApprovalLimitTime]";
                    where += " and Cl_CourseOrder.isleave = 1 and  Cl_CourseOrder.ApprovalFlag = 1 and Cl_CourseOrder.[ApprovalDateTime] <= Cl_CourseOrder.[ApprovalLimitTime] and StopOrderFlag=0";
                    break;
                case 3://待预订
                    //where += string.Format(" and cl_courseorder.userid is null and Co_Course.StartTime >= '{0}'", DateTime.Now);
                    where += string.Format(" and cl_courseorder.userid is null and Co_Course.StartTime >= '{0}' and  Co_Course.StopOrderFlag=0", DateTime.Now);
                    break;
                case 4://部门指定 
                    //where += " and cl_courseorder.IsAppoint = 1 and Cl_CourseOrder.orderstatus = 1 and (Cl_CourseOrder.IsLeave = 0 or (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1) or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime]))";
                    where += " and cl_courseorder.IsAppoint = 1 and Cl_CourseOrder.orderstatus = 1 and (Cl_CourseOrder.IsLeave = 0 or (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1) or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])) and StopOrderFlag=0";
                    break;
                case 5://总所指定 
                    //where += " and cl_courseorder.IsAppoint = 2 and Cl_CourseOrder.orderstatus = 1 and (Cl_CourseOrder.IsLeave = 0 or (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1) or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime]))";
                    where += " and cl_courseorder.IsAppoint = 2 and Cl_CourseOrder.orderstatus = 1 and (Cl_CourseOrder.IsLeave = 0 or (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1) or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])) and StopOrderFlag=0";
                    break;
                case 6://补预订 
                    //where += " and cl_courseorder.orderstatus = 3 ";
                    where += " and cl_courseorder.orderstatus = 3 and StopOrderFlag=0";
                    break;
                case 7://已过期 
                    //where += string.Format(" and cl_courseorder.userid is null and Co_Course.StartTime < '{0}'", DateTime.Now);
                    where += string.Format(" and (cl_courseorder.userid is null or Cl_CourseOrder.orderstatus = 0 ) and Co_Course.StartTime < '{0}' and StopOrderFlag=0 ", DateTime.Now);
                    break;
                case 8://关闭预定
                    where += " and Co_Course.StopOrderFlag=1 ";// and Co_Course.StartTime >'" + DateTime.Now + "'";//and Co_Course.IsOrder in(0,1,2,3)
                    break;
                default:
                    where += " and (Cl_CourseOrder.orderstatus = 0 or  cl_courseorder.userid is null)";
                    break;
            }


            var list = courseOrderBL.GetMyCourseList(out totalCount, CurrentUser.UserId, where, findnum, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);

            ////显示排队中
            if (apply == "3")
            {
                list = list.Where(p => p.HasEntered >= p.NumberLimited && p.StartTime > DateTime.Now && p.StopOrderFlag == 0).ToList();
                totalCount = list.Count();
            }


            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    Id = item.Id,
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
                    MyDoStatus = item.MyDoStatus,
                    MyStatusStr = item.MyStatusStr,
                    YearPlan = item.YearPlan,
                    SortStr = item.SortStr,
                    IsCPA = item.IsCPA == 0 ? "否" : "是",
                    CourseTimes = item.CourseTimesOrder + "/" + item.CourseTimes,
                    IsOpenSubInt = CurrentUser.IsMain == item.IsOpenSub ? 0 : 1,
                    inopenlevel = item.OpenLevel == null ? 0 : item.OpenLevel.Contains(CurrentUser.TrainGrade) ? 1 : 0,
                    noopenlevelandfensuo = item.OpenLevel == null ? 0 : item.IsOpenSub == CurrentUser.IsMain ? 1 : 0,
                    IsMain = CurrentUser.IsMain,
                    IsOrder = item.IsOrder,
                    StopOrderFlag = item.StopOrderFlag,
                    IsTest=item.IsTest,
                    IsOrNoOrLine = FIsOrNoOrLine(item.DeptHasEntered, item.SuccessEntered, item.HasEntered, item.NumberLimited, item.StartTime, item.StopDucueFlag, item.Way, item.StopOrderFlag, item.IsOrder)

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

        public string FIsOrNoOrLine(int DeptHasEntered, int SuccessEntered, int HasEntered, int NumberLimited, DateTime StartTime, int StopDucueFlag, int Way, int StopOrderFlag, int IsOrder)
        {
            if (IsOrder == 0 || IsOrder == 2)
            {
                return "否";
            }
            if (StopOrderFlag == 1)
            {
                return "否";
            }
            if (StartTime < DateTime.Now)
                return "否";

            if (NumberLimited > HasEntered)
            {
                if (StartTime.AddHours(AllSystemConfigs.Find(p => p.ConfigType == 21).ConfigValue.GetDouble() * -1) > DateTime.Now)
                {
                    if (DeptHasEntered > 0)
                    {
                        if (StopDucueFlag == 0)
                            return "是"; //可报名，需排队 
                        else
                            return "否";//不可报名，排队已关闭
                    }
                }
                else
                {
                    if (SuccessEntered <= 0 && NumberLimited <= (HasEntered + DeptHasEntered))
                    {
                        if (StopDucueFlag == 0)
                            return "是"; //可报名，需排队 排队中
                        else
                            return "否"; //不可报名，排队已关闭
                    }
                }
                return "是";//可直接报名
            }
            else
            {
                if (Way == 3)
                    return "否";
                if (StopDucueFlag == 0)
                    return "排队中"; //可报名，需排队 
                else
                    return "否"; //不可报名，排队已关闭

                return "是";//可直接报名

            }
        }

        public JsonResult GetMyCPACourseSubscribeList(string course, int subscribeType, DateTime? start, DateTime? end, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {
            if (Session["ccstpage"] != null)
            {
                Session.Remove("ccstpage");
            }
            Session["ccstpage"] = pageIndex + "㉿" + course + "㉿" + subscribeType + "㉿" + start + "㉿" + end;
            int totalCount = 0;
            string where = string.Format(@" Co_Course.IsDelete = 0 
and Co_Course.Publishflag = 1 
and Co_Course.CourseFrom = 2 
and Co_Course.Way = 3
and (
    ((select count(*) from Sys_User where UserId={0} and TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)))>0  
            or Co_Course.OpenLevel = '' or Co_Course.OpenLevel is null ) 
    and 
    ((select count(*) from Sys_GroupUser where UserId={0} and groupid in 
        (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))>0
	    or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null) 
    )
", CurrentUser.UserId);
            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
            if (start.HasValue)
                where += string.Format(" and Co_Course.StartTime >= '{0}'", start.Value);
            if (end.HasValue)
                where += string.Format(" and Co_Course.EndTime <= '{0}'", end.Value.AddDays(1));
            switch (subscribeType)
            {
                case 0://待预订
                    where += string.Format(" and Cl_CpaLearnStatus.userid is null and Co_Course.StartTime >= '{0}'", DateTime.Now);
                    break;
                case 1://预订成功
                    where += " and Cl_CpaLearnStatus.userid > 0 ";
                    break;
                case 2://已过期
                    where += string.Format(" and Cl_CpaLearnStatus.userid is null and Co_Course.StartTime < '{0}'", DateTime.Now);
                    break;
                default:
                    where += " and Cl_CpaLearnStatus.userid is null ";
                    break;
            }
            var list = courseOrderBL.GetMyCourseCPAList(out totalCount, CurrentUser.UserId, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    Id = item.Id,
                    CourseName = item.CourseName.HtmlXssEncode(),
                    CourseLength = item.CourseLength,
                    TrainDays = item.TrainDays,
                    CourseTime = item.CourseTime,
                    TeacherName = item.CpaTeacher.HtmlXssEncode(),
                    CourseAddress = item.CourseAddress.HtmlXssEncode(),
                    CourseObjectMemo = item.CourseObjectMemo.HtmlXssEncode(),
                    Memo = item.Memo.HtmlXssEncode(),
                    MyDoStatus = item.MyStatus > 0 ? 1 : (item.StartTime > DateTime.Now ? 0 : 1),
                    //MyStatusStr = item.MyStatus > 0 ? "预订成功" : (item.StartTime > DateTime.Now ? "待预订" : "已过期"),
                    MyStatusStr = item.MyStatus == 0 ? "待预订" :"已过期"
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

        public JsonResult Subscribe(int courseId)
        {
            int page = 1;
            if (Session["mcstpage"] != null)
            {
                string sess = Session["mcstpage"].ToString();
                string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                page = Convert.ToInt32(att[0]);
            }

            var error = "";
            int num = 0;
            var flag = courseOrderBL.GetCanSignup(out num, courseId, CurrentUser.UserId,
                                                AllSystemConfigs.Find(p => p.ConfigType == 21).ConfigValue.GetDouble());
            switch (flag)
            {
                case 0:
                    error = "不可预订！";
                    break;
                case 1:
                    error = "预订成功！";
                    break;
                case 2:
                    error = "预订人数已超过场地最大容纳数，将进入排队状态，是否继续报名？";
                    //error = string.Format("可预订，但需排队，目前排队 {0}人，是否继续？", num);
                    break;
                case 3:
                    error = "预订失败！";
                    break;
                case 4:
                    error = "不可预订，排队已关闭！";
                    break;
                case 5:
                    error = "不可预订，报名已关闭！";
                    break;
                default:
                    break;
            }
            return Json(new
            {
                result = flag,
                indexpage = page,
                content = error
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubscribeCPA(int courseId)
        {
            int page = 1;
            if (Session["ccstpage"] != null)
            {
                string sess = Session["ccstpage"].ToString();
                string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                page = Convert.ToInt32(att[0]);
            }
            var error = "";
            var flag = courseOrderBL.GetCanSignupCPA(courseId, CurrentUser.UserId);
            if (flag == 0)
                error = "不可预订！";
            if (flag == 1)
                error = "预订成功！";
            if (flag == 2)
                error = "不可预订，人数已满！";
            if (flag == 3)
                error = "预订失败！";
            return Json(new
            {
                result = flag,
                indexpage = page,
                content = error
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubscribeLine(int courseId)
        {
            try
            {
                var course = courseBL.GetCo_Course(courseId);
                courseOrderBL.Add(new Cl_CourseOrder
                {
                    CourseId = courseId,
                    UserId = CurrentUser.UserId,
                    OrderTime = DateTime.Now,
                    OrderStatus = 2,
                    OrderEndTime = course.StartTime.AddHours(-1 * AllSystemConfigs.Find(p => p.ConfigType == 21).ConfigValue.GetDouble()),
                    IsAppoint = 0,
                    CourseStartTime = course.StartTime,
                    CourseEndTime = course.EndTime,
                    CourseName = course.CourseName,
                    FtriggerFlag = 0
                });
                return Json(new
                {
                    result = 1,
                    content = "预订成功！"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "预订失败！"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Unsubscribe(int id, int year)
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

                //DateTime startTime = DateTime.Now;
                //DateTime endTime = DateTime.Now;
                //GetYearStartAndEnd(out startTime, out endTime, year);
                //var flag = courseOrderBL.UpdateStatus(id, CurrentUser.UserId, startTime, endTime, AllSystemConfigs.Find(p => p.ConfigType == 19).ConfigValue.GetInt32());
                int num = -1;
                var flag = courseOrderBL.UpdateStatus(out num, id, CurrentUser.UserId, year, AllSystemConfigs.Find(p => p.ConfigType == 19).ConfigValue.GetInt32());
                var error = "退订成功！";
                if (flag == 1 && num > -1)
                    error = "你全年课程退订次数还剩" + num + "次！确定继续退订吗？";
                else if (flag == 1)
                    flag = 4;
                if (flag == 2)
                    error = "你全年课程退订次数已经用完，不可退订！";
                else if (flag == 3)
                    error = "退订失败！";

                return Json(new
                {
                    result = flag,
                    content = error,
                    indexpage = page
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "退订失败！"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UnsubscribeLine(int id)
        {
            try
            {
                var model = courseOrderBL.Get(id);
                if (model == null)
                    return Json(0, JsonRequestBehavior.AllowGet);
                model.OrderStatus = 0;
                model.ApprovalDateTime = DateTime.Now;
                model.IsLeave = 0;
                model.ApprovalLimitTime = DateTime.Parse("2000-01-01");
                model.LeaveTime = DateTime.Parse("2000-01-01");
                model.Reson = "";
                model.LeaveUserID = "";
                model.Name = "";
                model.OrderTimes = model.OrderTimes + 1;
                courseOrderBL.Update(model);

                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

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
                var courseId = courseOrderBL.Get(id).CourseId.Value;
                var course = courseOrderBL.GetCourseById(courseId);
                var leaveMemo = AllSystemConfigs.Find(p => p.ConfigType == 31).ConfigValue;
                var str = memo.Split('♣');
                //var leader = userBL.GetUserByHrID(CurrentUser.LeaderID);
                var leader = leaderBL.GetLeaderIdByUserId(CurrentUser.UserId);
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
                var timespan = AllSystemConfigs.Find(p => p.ConfigType == 22).ConfigValue.GetDouble();
                courseOrderBL.UpdateLeave(id, newStr, leader.JobNum,
                                            timespan, CurrentUser.JobNum, CurrentUser.Realname);

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
                    memo = c
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
                    var leader = leaderBL.GetLeaderIdByUserId(CurrentUser.UserId);
                    if (leader != null)
                    {
                        if (!string.IsNullOrWhiteSpace(leader.Realname))
                        {
                            //if (!string.IsNullOrWhiteSpace(leader.MobileNum))
                            //    messList.Add(new KeyValuePair<string, string>(leader.MobileNum, memo));
                            if (!string.IsNullOrWhiteSpace(leader.Email))
                                emailList.Add(new KeyValuePair<string, string>(leader.Email, memo));
                        }
                        if (messList.Count > 0)
                            SendMessage(messList);
                        //if (emailList.Count > 0)
                        //    SendEmail(emailList, "您的下属有人请假了，快来审批吧！");
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
            var course = courseBL.GetCo_Course(id);
            var leaveMemo = AllSystemConfigs.Find(p => p.ConfigType == 31).ConfigValue;
            Regex regex = new Regex(@"\{[\d]{1,5}\}");
            string newStr = regex.Replace(leaveMemo, new MatchEvaluator(delegate(Match match)
            {
                //return match.Value.Replace("{", "<input class='Box Raster_10' type='text' id='txtLeaveMemo").Replace("}", "' />");
                return "<input class='Box span30' maxlength='50' type='text' />";
            }));
            if (newStr.Contains("{Leader}"))//领导
            {
                //var leader = userBL.GetUserByHrID(CurrentUser.LeaderID);
                var leader = leaderBL.GetLeaderIdByUserId(CurrentUser.UserId);
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
            return Json(newStr, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 课程预订(管理员)

        public ActionResult CourseSubscribe(string p, string cp)
        {
            ViewBag.page = 1;
            ViewBag.cstea = "请输入搜索内容";
            ViewBag.csname = "请输入搜索内容";
            ViewBag.csstate = 99;

            if (p != null && p != "" && p == "1")
            {
                if (Session["copage"] != null)
                {
                    string sess = Session["copage"].ToString();
                    string[] cs = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = cs[0];
                    ViewBag.cstea = cs[2] == "" ? "请输入搜索内容" : cs[2];
                    ViewBag.csname = cs[1] == "" ? "请输入搜索内容" : cs[1];
                    ViewBag.csstate = cs[3];
                }
            }

            ViewBag.cpage = 1;
            ViewBag.ccsname = "请输入搜索内容";
            ViewBag.ccstea = "请输入搜索内容";
            if (cp != null && cp != "" && cp == "1")
            {
                if (Session["cpapage"] != null)
                {
                    string csess = Session["cpapage"].ToString();
                    string[] ccs = Regex.Split(csess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.cpage = ccs[0];
                    ViewBag.ccsname = ccs[1] == "" ? "请输入搜索内容" : ccs[1];
                    ViewBag.ccstea = ccs[2] == "" ? "请输入搜索内容" : ccs[2];
                }
            }
            return View();
        }

        //详情
        public ActionResult CourseSubscribeDetail(int id)
        {
            ViewBag.model = courseOrderBL.GetCourseById(id);
            return View();
        }

        /// <summary>
        /// 查看CPA课程预定详情
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseCPASubscribeDetail()
        {
            return View();
        }

        //报名详情
        public ActionResult CourseApplyDetail(int id)
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.Id = id;
            return View();
        }

        public JsonResult GetCourseSubscribeList(string course, string teaName, int courseOrder, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {
            if (Session["copage"] != null)
            {
                Session.Remove("copage");
            }
            Session["copage"] = pageIndex + "㉿" + course + "㉿" + teaName + "㉿" + courseOrder;
            int totalCount = 0;
            string where = string.Format(@" Co_Course.IsDelete = 0 
                                            and Co_Course.Publishflag = 1 
                                            and Co_Course.Way = 1 
                                            and Co_Course.CourseFrom = 2 
                                            and (Co_Course.IsOrder = {0} or {0} = 99)
                                            ", courseOrder);

            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(teaName))
                where += string.Format(" and Sys_User.RealName like '%{0}%'", teaName.ReplaceSql());
            var list = courseOrderBL.GetCourseSubscribeList(out totalCount, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
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
                    HasEntered = item.HasEntered,
                    NumberLimited = item.NumberLimited,
                    StopDucueFlag = item.StopDucueFlag,
                    StopOrderFlag = item.StopOrderFlag,
                    OrderStr = item.OrderStr,
                    IsOrder = item.IsOrder,
                    OpenLevel = item.OpenLevel == null ? "无级别" : item.OpenLevel
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
        /// 获取CPA课程预定的信息
        /// </summary>
        /// <param name="cpaname">课程名称查询条件</param>
        /// <param name="teaName">CPA讲师查询条件</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">当前的页号</param>
        /// <param name="jsRenderSortField">排序方式</param>
        /// <returns></returns>
        public JsonResult GetCpaCourseSubscribeList(string cpaname, string teaName, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {
            if (Session["cpapage"] != null)
            {
                Session.Remove("cpapage");
            }
            Session["cpapage"] = pageIndex + "㉿" + cpaname + "㉿" + teaName;

            var totalCount = 0;
            var where = string.Format(@" Co_Course.IsDelete = 0 and Co_Course.Publishflag = 1 and Co_Course.Way = 3 and Co_Course.CourseFrom = 2 ");

            if (!string.IsNullOrWhiteSpace(cpaname))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", cpaname.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(teaName))
                where += string.Format(" and Co_Course.CpaTeacher like '%{0}%'", teaName.ReplaceSql());
            var list = courseOrderBL.GetCpaCourseSubscribeList(out totalCount, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
            var newlist = new List<object>();
            foreach (var item in list)
            {
                newlist.Add(new
                {
                    item.Id,
                    CourseName = item.CourseName.HtmlXssEncode(),
                    CourseTime = item.StartTime.ToString("yyyy-MM-dd HH:mm") + " -- " + item.EndTime.ToString("yyyy-MM-dd HH:mm"),
                    TeacherName = item.CpaTeacher.HtmlXssEncode(),
                    item.HasEntered,
                    item.NumberLimited,
                    OpenLevel = item.OpenLevel.Replace(",", "，")
                });
            }

            return Json(new
            {
                result = 1,
                dataList = newlist,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DucueFlag(int id, int status)
        {
            try
            {
                courseOrderBL.UpdateDucueFlag(id, status);
                string page = "1";
                if (Session["copage"] != null)
                {
                    string sess = Session["copage"].ToString();
                    string[] cs = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    page = cs[0];
                }
                return Json(new
                {
                    result = 1,
                    content = status == 1 ? "关闭成功！" : "开启成功！",
                    indexpage = page
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = status == 1 ? "关闭失败！" : "开启失败！"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult OrderFlag(int id, int status)
        {
            try
            {
                courseOrderBL.UpdateOrderFlag(id, status);
                string page = "1";
                if (Session["copage"] != null)
                {
                    string sess = Session["copage"].ToString();
                    string[] cs = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    page = cs[0];
                }
                return Json(new
                {
                    result = 1,
                    content = status == 1 ? "关闭成功！" : "开启成功！",
                    indexpage = page
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = status == 1 ? "关闭失败！" : "开启失败！"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDrawSubscribe(int courseId)
        {
            var pie = new ChartModel();
            var column = new ChartModel();
            var columnList = courseOrderBL.GetDeptSubscribe(courseId);
            //var pieList = courseOrderBL.GetTrainGradeSubscribe(courseId);
            var pieList = courseOrderBL.GetSuccessTrainGradeSubscribe(courseId); //报名成功的 
            var pieSer = new List<pieSeries>();
            var columnSer = new List<Series>();

            #region 柱形图
            var nameList = new List<string>();
            var columnSer1 = new List<Double>();
            var columnSer2 = new List<Double>();
            foreach (var item in columnList)
            {
                columnSer1.Add(item.AllCount);
                columnSer2.Add(item.SubscribeCount);
                nameList.Add(item.DeptName);
            }
            columnSer.Add(new Series
            {
                data = columnSer1,
                name = "目标人数"
            });
            columnSer.Add(new Series
            {
                data = columnSer2,
                name = "已报名人数"
            });

            column.DivID = "showcolumn";
            column.xAxis = nameList;
            column.title = "";
            column.series = columnSer;
            #endregion

            #region 扇形图
            foreach (var item in pieList)
            {
                pieSer.Add(new pieSeries
                {
                    name = item.Name + " (" + item.SubscribeCount + "人)",
                    y = item.SubscribeCount
                });
            }
            pie.DivID = "showpie";
            pie.title = "培训级别报名率";
            pie.pieseries = pieSer;
            #endregion

            return Json(new
            {
                pie = pie,
                column = column
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取CPA预定培训级别的饼图
        /// </summary>
        /// <param name="id">课程ID</param>
        /// <returns></returns>
        public JsonResult GetDrawCPASubscribe(int id)
        {
            var pie = new ChartModel();
            var pieList = courseOrderBL.GetTrainGradeCPASubscribe(id);
            var pieSer = new List<pieSeries>();
            foreach (var item in pieList)
            {
                pieSer.Add(new pieSeries
                {
                    name = item.Name,
                    y = item.SubscribeCount
                });
            }
            pie.DivID = "showpie";
            pie.title = "培训级别报名率";
            pie.pieseries = pieSer;

            return Json(new
            {
                pie = pie
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCourseSubscribeUserList(int courseId, string apply, string realName = "", string deptName = "", string IsAppoint = "", int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "OrderTime desc")
        {
            int totalCount = 0;
            string where = string.Format(@"  sys_user.IsDelete = 0 AND co_course.ID = {0}
and (((sys_user.userid in (select userid from sys_groupuser where groupid in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
	or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null
)
and (sys_user.deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject))
	or Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null
)) 
or cl_courseorder.IsAppoint is not null)
and (sys_user.TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) or cl_courseorder.IsAppoint is not null)
", courseId);
            if (apply == "1")
                where += @" and (cl_courseorder.orderstatus is not null and cl_courseorder.orderstatus > 0 
                            and (Cl_CourseOrder.IsLeave = 0 
                                or (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1)
                                or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                                    and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])) )";
            if (apply == "0")
                where += @" and (cl_courseorder.orderstatus is null or cl_courseorder.orderstatus = 0 
                                or (cl_courseorder.orderstatus = 1 and Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                                    and Cl_CourseOrder.[ApprovalDateTime] <= Cl_CourseOrder.[ApprovalLimitTime]))";
            if (IsAppoint != "10")
            {
                if (IsAppoint == "0")
                {
                    where += " and Cl_CourseOrder.IsAppoint=" + IsAppoint + " and Cl_CourseOrder.OrderStatus=1";
                }
                if (IsAppoint == "1")
                {
                    where += " and Cl_CourseOrder.IsAppoint=" + IsAppoint;
                }
                if (IsAppoint == "2")
                {
                    where += " and Cl_CourseOrder.IsAppoint=" + IsAppoint + "  and Cl_CourseOrder.OrderStatus in(1,2)";
                }

                if (IsAppoint == "3")
                {
                    where += " and Cl_CourseOrder.OrderStatus=3";
                }
            }

            if (!string.IsNullOrWhiteSpace(deptName))
                where += string.Format(" and Sys_User.DeptPath like '%{0}%'", deptName.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(realName))
                where += string.Format(" and Sys_User.RealName like '%{0}%'", realName.ReplaceSql());
            var list = courseOrderBL.GetCourseSubscribeUserList(out totalCount, courseId, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    IsApply = item.IsApply,
                    UserId = item.UserId,
                    JobNum = item.JobNum,
                    Realname = item.Realname,
                    SexStr = item.SexStr,
                    DeptName = item.DeptPath,
                    TrainGrade = item.TrainGrade,
                    ApplyStr = item.ApplyStr,
                    StopOrderFlag = item.ApplyPropertiesStr,
                    ApplyPropertiesStr = item.ApplyPropertiesStr,
                    OrderTimeStr = item.OrderTimeStr

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
        /// CPA课程预定人员信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCPACourseSubscribeUserList(int id, string pname = "", string dname = "",int flag=-1, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "UserId desc")
        {
            var totalCount = 0;
            var where = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(dname))
                where += string.Format(" and Sys_User.DeptPath like '%{0}%'", dname.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(pname))
                where += string.Format(" and Sys_User.RealName like '%{0}%'", pname.ReplaceSql());
            if (flag != -1)
            {
                where += string.Format(" and Cl_CpaLearnStatus.CpaFlag={0}", flag);
            }
            var list = courseOrderBL.GetCPACourseSubscribeUserList(out totalCount, id, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);

            var newlist = new List<object>();
            foreach (var item in list)
            {
                newlist.Add(new
                                {
                                    item.UserId,
                                    item.Realname,
                                    item.SexStr,
                                    DeptName = item.DeptPath,
                                    item.TrainGrade,
                                    item.OrderTimeStr
                                });
            }

            return Json(new
            {
                result = 1,
                dataList = newlist,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }


        public void ExportCourseSubscribeUserList(int courseId, string apply, string realName = "", string deptName = "")
        {
            int totalCount = 0;
            string where = string.Format(@"   sys_user.IsDelete = 0 AND co_course.ID = {0}
and (((sys_user.userid in (select userid from sys_groupuser where groupid in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
	or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null
)
and (sys_user.deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject))
	or Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null
)) 
or cl_courseorder.IsAppoint is not null)
and (sys_user.TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) or cl_courseorder.IsAppoint is not null)
", courseId);
            if (apply == "1")
                where += " and (cl_courseorder.orderstatus is not null and cl_courseorder.orderstatus <> 0)";
            if (apply == "0")
                where += " and (cl_courseorder.orderstatus is null or cl_courseorder.orderstatus = 0)";
            if (!string.IsNullOrWhiteSpace(deptName))
                where += string.Format(" and Sys_User.DeptPath like '%{0}%'", deptName.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(realName))
                where += string.Format(" and Sys_User.RealName like '%{0}%'", realName.ReplaceSql());
            var list = courseOrderBL.GetCourseSubscribeUserList(out totalCount, courseId, where);

            var dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("部门/分所", typeof(string));
            dt.Columns.Add("培训级别", typeof(string));
            dt.Columns.Add("性别", typeof(string));
            dt.Columns.Add("是否报名", typeof(string));
            dt.Columns.Add("报名性质", typeof(string));
            dt.Columns.Add("报名时间", typeof(string));
            var count = 1;
            foreach (Sys_User item in list)
            {
                dt.Rows.Add(count++, item.UserId, item.Realname, item.DeptPath, item.TrainGrade, item.SexStr, item.ApplyStr,
                            item.ApplyPropertiesStr, item.OrderTimeStr);
            }
            var dtList = new List<DataTable>();
            string strFileName = "课程报名情况";
            dtList.Add(dt);
            var export = new Spreadsheet();
            export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, strFileName);
        }

        /// <summary>
        /// 导出CPA课程人员信息
        /// </summary>
        public void ExportCPACourseSubscribeUserList(int id, string apply, string pname = "", string dname = "")
        {
            var totalCount = 0;
            var where = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(dname))
                where += string.Format(" and Sys_User.DeptPath like '%{0}%'", dname.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(pname))
                where += string.Format(" and Sys_User.RealName like '%{0}%'", pname.ReplaceSql());
            var list = courseOrderBL.GetCPACourseSubscribeUserList(out totalCount, id, where);

            var dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("性别", typeof(string));
            dt.Columns.Add("部门/分所", typeof(string));
            dt.Columns.Add("培训级别", typeof(string));
            var count = 1;
            foreach (var item in list)
            {
                dt.Rows.Add(count++, item.Realname, item.SexStr, item.DeptPath, item.TrainGrade);
            }
            var dtList = new List<DataTable>();
            const string strFileName = "CPA课程报名情况";
            dtList.Add(dt);
            var export = new Spreadsheet();
            export.ExportChart(new List<ChartModel>(), dtList, HttpContext, strFileName);
        }


        public JsonResult RemindUserSubscribe(string userIds, int courseId)
        {
            try
            {
                var content = GetFormworkContent(2);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var course = courseOrderBL.GetCourseById(courseId);
                    var userList = userBL.GetList(" sys_user.userid in (select id from dbo.F_SplitIDs('" + userIds + "')) and sys_user.isteacher< 2 and sys_user.isdelete = 0 and (sys_user.realname is not null or sys_user.realname <> '')");
                    var messList = new List<KeyValuePair<string, string>>();
                    var emailList = new List<KeyValuePair<string, string>>();
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(userList[i].Realname))
                        {
                            var c = string.Format(content,
                                                userList[i].Realname,
                                                course.TeacherName,
                                                course.CourseName,
                                                course.StartTime.ToString("yyyy-MM-dd HH:mm"));
                            if (!string.IsNullOrWhiteSpace(userList[i].MobileNum))
                                messList.Add(new KeyValuePair<string, string>(userList[i].MobileNum, c));
                            if (!string.IsNullOrWhiteSpace(userList[i].Email))
                                emailList.Add(new KeyValuePair<string, string>(userList[i].Email, c));
                        }
                    }
                    if (messList.Count > 0)
                        SendMessage(messList);
                    if (emailList.Count > 0)
                        SendEmail(emailList, "课程即将开始，速来报名！");
                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult NoRemindUserSubscribe(string userIds, int courseId)
        {
            try
            {
                var content = GetFormworkContent(2);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var course = courseOrderBL.GetCourseById(courseId);
                    var userList = userBL.GetList(" sys_user.userid in (select id from dbo.F_SplitIDs('" + userIds + "')) and sys_user.isteacher< 2 and sys_user.isdelete = 0 and (sys_user.realname is not null or sys_user.realname <> '')");
                    var messList = new List<KeyValuePair<string, string>>();
                    //var emailList = new List<KeyValuePair<string, string>>();
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(userList[i].Realname))
                        {
                            var c = string.Format(content,
                                                userList[i].Realname,
                                                course.TeacherName,
                                                course.CourseName,
                                                course.StartTime.ToString("yyyy-MM-dd HH:mm"));
                            if (!string.IsNullOrWhiteSpace(userList[i].MobileNum))
                                messList.Add(new KeyValuePair<string, string>(userList[i].MobileNum, c));
                            //if (!string.IsNullOrWhiteSpace(userList[i].Email))
                            //    emailList.Add(new KeyValuePair<string, string>(userList[i].Email, c));
                        }
                    }
                    if (messList.Count > 0)
                        SendMessage(messList);
                    //if (emailList.Count > 0)
                    //   SendEmail(emailList, "课程即将开始，速来报名！");
                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCourseDeptSubscribe(int courseId, string deptName, int? start, int? end, int pageSize = 10, int pageIndex = 1)
        {
            var dataList = new List<NameSubscribeCount>();
            var list = courseOrderBL.GetDeptSubscribe(courseId);
            //if (!string.IsNullOrWhiteSpace(deptName))
            //list = list.Where(p => p.DeptName.Contains(deptName)).ToList();
            //list = list.Where(p => p.DeptName.Contains(deptName)).ToList();              
            if (start.HasValue)
                list = list.Where(p => p.Percent >= start).ToList();
            if (end.HasValue)
                list = list.Where(p => p.Percent <= end).ToList();

            int i = 0;
            int all = 0;
            int sub = 0;
            string dep = "立信集团";
            //list = list.Skip(9).Take(2).ToList();
            foreach (var item in list)
            {
                i++;
                all += item.AllCount;
                sub += item.SubscribeCount;

                var name = "";
                var allname = "";
                int a = item.ParentDeptId;

                Sys_Department n = new Sys_Department();
                bool b = true;
                Stack arr = new Stack();
                arr.Push("" + item.DeptName + "/");

                while (b)
                {
                    n = list.Where(p => p.DepartmentId == a).FirstOrDefault();
                    //n = list.Where(p => p.DepartmentId == item.ParentDeptId).FirstOrDefault();  //找出父及
                    //n = deptBL.Get(item.ParentDeptId);
                    if (n != null)
                    {
                        //allname += n.DeptName+"/"  ;
                        arr.Push("" + n.DeptName + "/");
                        a = n.ParentDeptId;
                        //item.ParentDeptId = n.ParentDeptId;
                    }
                    else
                    {
                        //name = dep + "/" + allname;
                        b = false;
                        break;
                    }
                    if (n.ParentDeptId == 0)
                    {
                        b = false;
                        break;
                    }

                }
                int sun = arr.Count;
                for (int z = 0; z < sun; z++)
                {
                    name += arr.Pop();
                }

                dataList.Add(new NameSubscribeCount
                {
                    num = i,
                    AllCount = item.AllCount,
                    SubscribeCount = item.SubscribeCount,
                    Name = name.Substring(0, name.LastIndexOf('/'))
                });
            }

            if (!string.IsNullOrWhiteSpace(deptName))
                dataList = dataList.Where(p => p.Name.Contains(deptName)).ToList();


            int totalcount = dataList.Count;
            dataList = dataList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                result = 1,
                dataList,
                recordCount = totalcount,
                all,
                sub,
                per = all == 0 ? 0 : sub * 100 / all
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCourseTrainGradeSubscribe(int courseId, string trainGrade, int? start, int? end, int pageSize = 10, int pageIndex = 1)
        {
            var list = courseOrderBL.GetTrainGradeSubscribe(courseId).OrderBy(p => p.Name).ToList();
            if (!string.IsNullOrWhiteSpace(trainGrade))
                list = list.Where(p => trainGrade.Contains(p.Name)).ToList();
            if (start.HasValue)
                list = list.Where(p => p.Percent >= start).ToList();
            if (end.HasValue)
                list = list.Where(p => p.Percent <= end).ToList();
            int i = 0;
            int all = 0;
            int sub = 0;
            foreach (var item in list)
            {
                i++;
                all += item.AllCount;
                sub += item.SubscribeCount;
                item.num = i;
            }

            int totalcount = list.Count;
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = totalcount,
                all,
                sub,
                per = all == 0 ? 0 : sub * 100 / all
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// CPA课程报名培训级别明细
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCpaCourseTrainGradeSubscribe(int courseId, string trainGrade, int? start, int? end, int pageSize = 10, int pageIndex = 1)
        {
            var list = courseOrderBL.GetCpaTrainGradeSubscribe(courseId).OrderBy(p => p.Name).ToList();
            if (!string.IsNullOrWhiteSpace(trainGrade))
                list = list.Where(p => trainGrade.Contains(p.Name)).ToList();
            if (start.HasValue)
                list = list.Where(p => p.Percent >= start).ToList();
            if (end.HasValue)
                list = list.Where(p => p.Percent <= end).ToList();
            int i = 0, all = 0, sub = 0;
            foreach (var item in list)
            {
                i++;
                all += item.AllCount;
                sub += item.SubscribeCount;
                item.num = i;
            }

            var totalcount = list.Count;
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = totalcount,
                all,
                sub,
                per = all == 0 ? 0 : sub * 100 / all
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 课程预订状态查询（部门负责人）

        public ActionResult CourseDeptSubscribe(string p)
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
                if (Session["cdspage"] != null)
                {
                    string sess = Session["cdspage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.course = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.teaname = att[2] == "" ? "请输入搜索内容" : att[2];
                    ViewBag.cOrder = att[3];
                    ViewBag.isOrder = att[4];
                    ViewBag.sltStatus = att[5];
                }
            }
            return View();
        }

        //详情
        public ActionResult CourseDeptSubscribeDetail(int id)
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.model = courseOrderBL.GetCourseById(id);
            return View();
        }

        public JsonResult GetCourseDeptSubscribeList(string course, string teaName, int courseOrder, int isorder, int sltStatus = 99, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {
            if (Session["cdspage"] != null)
            {
                Session.Remove("cdspage");
            }
            Session["cdspage"] = pageIndex + "㉿" + course + "㉿" + teaName + "㉿" + courseOrder + "㉿" + isorder + "㉿" + sltStatus;
            var leaderId = CurrentUser.JobNum;
            var deptId = CurrentUser.DeptId;
            if (CurrentUser.TrainMaster == 1 && CurrentUser.LeaderID != "")
            {
                leaderId = CurrentUser.LeaderID;
                var le = userBL.GetUserByHrID(leaderId);
                if (le != null)
                    deptId = le.DeptId;
            }
            int totalCount = 0;
            string where = string.Format(@" Co_Course.IsDelete = 0 
                                            and Co_Course.Publishflag = 1 
                                            and Co_Course.CourseFrom = 2 
                                            and Co_Course.Way = 1 
                                            and (Co_Course.IsOrder = {0} or {0} = 99)
                                            and (','+Co_Course.OpenDepartObject+',' like '%,{1},%' 
                                                    or Co_Course.OpenDepartObject = '' or Co_Course.OpenDepartObject is null )
                                            ", courseOrder, deptId);

            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(teaName))
                where += string.Format(" and Sys_User.RealName like '%{0}%'", teaName.ReplaceSql());
            if (isorder == 1)
                where += " and (Co_Course.IsOrder = 1 or Co_Course.IsOrder  = 3)";
            else if (isorder == 0)
                where += " and (Co_Course.IsOrder = 0 or Co_Course.IsOrder  = 2 or Co_Course.IsOrder  = 3)";

            if (sltStatus == 0)
                where += string.Format(" and (Co_Course.StartTime >'{0}')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            else if (sltStatus == 1)
                where += string.Format(" and (Co_Course.StartTime <= '{0}' and Co_Course.EndTime>='{0}')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            else if (sltStatus == 2)
                where += string.Format(" and (Co_Course.EndTime<'{0}')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var list = courseOrderBL.GetCourseDeptSubscribeList(out totalCount, leaderId, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
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
                    AssignUserCount = item.AssignUserCount
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

        public JsonResult GetCourseDeptSubscribeUserList(int courseId, string realName, string traingrade, int apply, int appoint, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "UserId desc")
        {
            var leaderId = CurrentUser.JobNum;
            if (CurrentUser.TrainMaster == 1 && CurrentUser.LeaderID != "")
                leaderId = CurrentUser.LeaderID;
            int totalCount = 0;
            string where = string.Format(@"  sys_user.IsDelete = 0 AND co_course.ID = {0}
and ((sys_user.userid in (select userid from sys_group where groupid in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
	or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null
)
or (sys_user.deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject))
	or Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null
)) 
and Sys_User.LeaderID = '{1}'
and (sys_user.TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) or cl_courseorder.IsAppoint is not null)
and (cl_courseorder.IsAppoint = {2} or {2} = 99 {4})
{3}
", courseId, leaderId, appoint, appoint == 0 ? "and cl_courseorder.orderstatus <> 0" : "", (appoint == 3 ? "or cl_courseorder.orderstatus=3 " : "")
);
            if (apply == 1)//已报名
                where += " and ((cl_courseorder.orderstatus = 1 and cl_courseorder.IsLeave = 0) or (cl_courseorder.orderstatus = 1 and cl_courseorder.IsLeave = 1 and cl_courseorder.ApprovalFlag <> 1) or cl_courseorder.orderstatus = 3 or cl_courseorder.orderstatus = 2)";
            if (apply == 0)//未报名
                where += " and (cl_courseorder.orderstatus is null)";
            if (apply == 2)//已请假
                where += " and (cl_courseorder.orderstatus = 1 and cl_courseorder.IsLeave = 1 and cl_courseorder.ApprovalFlag = 1)";
            if (apply == 3)//已退订
                where += " and (cl_courseorder.orderstatus = 0 )";
            if (!string.IsNullOrWhiteSpace(traingrade))
                where += string.Format(" and ('{0}' like '%'+sys_user.traingrade+'%' and (sys_user.traingrade is not null and sys_user.traingrade <> ''))", traingrade.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(realName))
                where += string.Format(" and Sys_User.RealName like '%{0}%'", realName.ReplaceSql());
            var list = courseOrderBL.GetCourseSubscribeUserList(out totalCount, courseId, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    IsApply = item.IsApply,
                    UserId = item.UserId,
                    JobNum = item.JobNum,
                    Realname = item.Realname,
                    SexStr = item.SexStr,
                    TrainGrade = item.TrainGrade,
                    ApplyStr = item.CourseOrderStr,
                    ApplyPropertiesStr = item.CourseOrderStr != "已退订" ? item.ApplyPropertiesStr : "——"
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

        #endregion

        #region 修改本课程中的排队状态
        /// <summary>
        /// 修改本课程中的排队状态
        /// </summary>
        public JsonResult UpdateCourseOrderStatusByCourseId(int id)
        {
            try
            {
                var list = courseOrderBL.UpdateCourseOrderStatusByCourseId(id);
                var content = GetFormworkContent(5);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var messList = new List<KeyValuePair<string, string>>();
                    var emailList = new List<KeyValuePair<string, string>>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(list[i].realname))
                        {
                            var c = string.Format(content,
                                                list[i].realname,
                                                list[i].OrderTime.Value.ToString("yyyy-MM-dd HH:mm"),
                                                list[i].Course_Name,
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                            if (!string.IsNullOrWhiteSpace(list[i].userMobileNum))
                                messList.Add(new KeyValuePair<string, string>(list[i].userMobileNum, c));
                            if (!string.IsNullOrWhiteSpace(list[i].userEmail))
                                emailList.Add(new KeyValuePair<string, string>(list[i].userEmail, c));
                        }
                    }
                    if (messList.Count > 0)
                        SendMessage(messList);
                    if (emailList.Count > 0)
                        SendEmail(emailList, "排队中的课程，报名成功了！");
                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        /// <summary>
        /// 获取集中授课中报名的人员
        /// </summary>
        public JsonResult GetCourseOrderName(int courseID,string realName="",string deptName="", int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalCount = 0;
                string where = " 1=1";
                if (!string.IsNullOrWhiteSpace(deptName))
                    where += string.Format(" and su.DeptPath like '%{0}%'", deptName.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(realName))
                    where += string.Format(" and su.RealName like '%{0}%'", realName.ReplaceSql());
                var list = courseOrderBL.GetCourseOrderName(out totalCount, courseID,where, pageSize, pageIndex);
                return Json(new
                      {
                          result = 1,
                          dataList = list,
                          recordCount = totalCount
                      }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Sys_User>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
