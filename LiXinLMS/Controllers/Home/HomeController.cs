using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinBLL.SystemManage;
using LiXinCommon;
using LiXinInterface;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinInterface.SystemManage;
using LiXinInterface.User;
using LiXinModels.CourseManage;
using LiXinModels.SystemManage;
using LiXinModels.User;
using LiXinModels;
using System.Text.RegularExpressions;

namespace LiXinControllers.Home
{
    public class HomeController:BaseController
    {
        protected ICo_Course CourseBL;
        protected IUpdateRecord Record;
        protected ICourseOrder CourseOrderBL;
        protected IUser UserBL;
        protected ICl_Attendce IAttendce;
        protected ISys_Notes noteBL;

        public HomeController(ICo_Course courseBL, ICourseOrder courseOrderBL, IUpdateRecord record, IUser userBL, ICl_Attendce iAttendce, ISys_Notes _noteBL)
        {
            CourseBL = courseBL;
            CourseOrderBL = courseOrderBL;
            Record = record;
            UserBL = userBL;
            IAttendce = iAttendce;
            noteBL = _noteBL;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 系统管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemManageIndex()
        {
            ViewBag.UserRight = UserRights;
            return View();
        }

        /// <summary>
        /// 我的培训首页
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult MyTrainIndex()
        {
            var uid = CurrentUser == null ? 0 : CurrentUser.UserId;
            var usermodel = InitSuper;
            //所内目标学时
            string result = "0";
            //CPA目标学时
            string CPAresult = "0";
            //个人总学时（除CPA）
            decimal userscore = 0;
            //全年补预定次数
            int ordercount = 0;

            if (uid != 0)
            {
                //CPA目标学时
                Sys_ParamConfig cpazq1 = AllSystemConfigs.Where(p => p.ConfigType == 16).FirstOrDefault();
                CPAresult = cpazq1.ConfigValue;

                //计算个人学时
                Sys_ParamConfig cpazq2 = AllSystemConfigs.Where(p => p.ConfigType == 14).FirstOrDefault();
                userscore = UserBL.GetUserScore(uid, cpazq2);

                usermodel = UserBL.GetmodelCAP(uid);
                Sys_ParamConfig cpazq = AllSystemConfigs.Where(p => p.ConfigType == 13).FirstOrDefault();
                string mianstr = cpazq.ConfigValue + ";";
                string substr = "(?<=" + usermodel.TrainGrade + "-).*?(?=;)";
                //string substr = "(?<=B-).*?(?=;)";
                result = Regex.Match(mianstr, substr).Value;

                

                //全年补预定次数
                Sys_ParamConfig cpazq3 = AllSystemConfigs.Where(p => p.ConfigType == 20).FirstOrDefault();
                ordercount = Convert.ToInt32(cpazq3.ConfigValue);
                if (usermodel.ordertimes > ordercount)
                {
                    usermodel.ordertimes = ordercount;
                }
            }
            ViewData["model"] = usermodel;
            ViewBag.PayGrade = CurrentUser.PayGrade;

            var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 33);
            //读参数配置获取全年在线测试完成次数
            ViewBag.ALLOnLineTestNum = Sys_ParamConfig.ConfigValue;

            ViewBag.PassOnLineTestNum = IAttendce.GetMyExamListPassCount(CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId, " ((way=1 and PassStatus=1) or (way=2 and IsPass=1))");
            ViewBag.CourseLe = result;
            ViewBag.UserScore = userscore;
            ViewBag.CPACourseLe = CPAresult;
            ViewBag.OrderCount = ordercount;

            return View();
        }

        /// <summary>
        /// 版本更新日志呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateRecord()
        {
            return View();
        }

        /// <summary>
        /// 获取我的培训首页的三种课程
        /// </summary>
        /// <param name="type">0:集中授课；1：视频；2：CPA</param>
        /// <returns></returns>
        public JsonResult GetMyTrainIndexCourse(int type=0)
        {
            var isOpenSub = "";
            if (CurrentUser.IsMain == 1)
            {
                isOpenSub = " or IsOpenSub=1";
            }
            var totalCount = 0;
            var sqlwhere = "";

            switch(type)
            {
                case 0:
                    sqlwhere = string.Format(@" Co_Course.Way=1 and Co_Course.Publishflag=1 and StartTime>=getdate() and (
        ((
			(select count(*) from Sys_User where UserId={0} and TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)))>0  
			--or Co_Course.OpenLevel = '' or Co_Course.OpenLevel is null 
		) 
		and 
		(
			((Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null) and (Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null )) 
			or 
            (select count(*) from Sys_User where UserId={0} and deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject)))>0  
			or
			(select count(*) from Sys_GroupUser where UserId={0} and groupid in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))>0
		)) or Co_Course.Id in (select CourseId from Cl_CourseOrder where Cl_CourseOrder.UserId = {0} and  Cl_CourseOrder.orderstatus > 0) 
   {1} )", CurrentUser.UserId,isOpenSub);
                    break;
                case 1:
                    sqlwhere = string.Format(@" Co_Course.Way=2 and Co_Course.Publishflag=1 and EndTime>=getdate() 
        and (
        (
			(select count(*) from Sys_User where UserId={0} and TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)))>0  
			--or Co_Course.OpenLevel = '' or Co_Course.OpenLevel is null 
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
		)  {1}
    )", CurrentUser.UserId, isOpenSub);
                    break;
                case 2:
                    sqlwhere = string.Format(@" Co_Course.Way=3 and Co_Course.Publishflag=1 and StartTime>=getdate()
and (
        (
			(select count(*) from Sys_User where UserId={0} and TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)))>0  
			or Co_Course.OpenLevel = '' or Co_Course.OpenLevel is null 
		) 
		and 
		(
			(Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null)
			or
			(select count(*) from Sys_GroupUser where UserId={0} and groupid in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))>0
		) 
    )", CurrentUser.UserId);
                    break;
            }
            var list = CourseBL.GetCourseCommonList(out totalCount, sqlwhere, 0, 8, " order by Co_Course.StartTime desc ");
            var newlist = new List<object>();
            list.ForEach(p=> newlist.Add(new
                                             {
                                                 id=p.Id,
                                                 name = p.CourseName.HtmlXssEncode(),
                                                 date=p.StartTime.ToString("yyyy-MM-dd HH:mm"),
                                                 picdate=p.StartTime.ToString("yyyy.MM.dd HH:mm")+"-"+p.EndTime.ToString("yyyy.MM.dd HH:mm"),
                                                 length = p.CourseLength,
                                                 teacher = p.TeacherStr
                                             }));
            return Json(newlist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取我的请假申请
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyLeaveList()
        {
            var totalCount = 0;
            var sqlwhere = string.Format(" Co_Course.IsDelete = 0  and Cl_CourseOrder.userid = {0} and Co_Course.publishflag =1 and Cl_CourseOrder.isleave = 1 ", CurrentUser.UserId);
            const string orderwhere = " order by Cl_CourseOrder.LeaveTime desc ";
            var list= CourseOrderBL.GetMyLeaveList(out totalCount, sqlwhere, 0, 4, orderwhere);
            var newlist = new List<object>();
            list.ForEach(p=> newlist.Add(new
                                             {
                                                 name=p.CourseName,
                                                 date=p.LeaveTimeStr,
                                                 status=p.ApprovalStatusStr,
                                                 flag=0,
                                                 url=0
                                             }));
            return Json(newlist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取我的补预定申请
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyMakeUpList()
        {
            var totalCount = 0;
            var sqlwhere = string.Format(@" Co_Course.IsDelete = 0 and Cl_MakeUpOrder.userid = {0} and Co_Course.publishflag =1 ", CurrentUser.UserId);
            const string orderwhere = " order by Cl_MakeUpOrder.LeaveTime desc ";
            var list = CourseOrderBL.GetMyComplementResList(out totalCount, sqlwhere, 0, 4, orderwhere);
            var newlist = new List<object>();
            list.ForEach(p => newlist.Add(new
            {
                name = p.Course_Name,
                date = p.LeaveTimeStr,
                status = p.ApprovalStatusStr,
                flag=0
            }));
            return Json(newlist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取我的逾时申请
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyTimeOutList()
        {
            var totalCount = 0;
            var sqlwhere = string.Format(@" Co_Course.IsDelete = 0 and Cl_TimeOutOrder.userid = {0} and Co_Course.publishflag =1 ", CurrentUser.UserId);
            const string orderwhere = " order by Cl_TimeOutOrder.OutTime desc ";
            var list = CourseOrderBL.GetMyTimeOutList(out totalCount, sqlwhere, 0, 4, orderwhere);
            var newlist = new List<object>();
            list.ForEach(p => newlist.Add(new
            {
                name = p.Course_Name,
                date = p.LeaveTimeStr,
                status = p.ApprovalStatusStr,
                flag=0,
                url=1
            }));
            return Json(newlist, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取某月的任务安排
        /// </summary>
        /// <param name="tID">课程ID</param>
        /// <param name="month">月</param>
        /// <param name="addMonth">所加的月数</param>
        /// <param name="year">年</param>
        /// <returns></returns>
        public JsonResult GetCalendarTask(string year = "", string month = "", int addMonth = 0)
        {
            year = year == "" ? DateTime.Now.Year.ToString() : year;
            month = month == "" ? DateTime.Now.Month.ToString() : month;
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            //所查询的月的1号
            var quMonth = Convert.ToDateTime(year + "-" + month + "-1").AddMonths(addMonth);

            var strwhere = string.Format(" {0} ", string.Format("(b.StartTime between '{0}' and '{1}' )",
                                                       quMonth.ToString("yyyy-MM-dd"),
                                                       quMonth.AddMonths(1).ToString("yyyy-MM-dd")));
            var listCourse = CourseOrderBL.GetMyCourseShedule(CurrentUser.UserId,strwhere);
            
            //初始化日历集合
            var calendarTask = new List<CalendarTask>();
            InitCalendarTaskCollection(ref calendarTask, quMonth);
            //加载当前月
            for (var i = 0; ; i++)
            {
                var newdate = quMonth.AddDays(i);
                if (newdate.Month != quMonth.Month)
                    break;
                var morningstr = "";
                var afterstr = "";
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
                    Bg = newdate == DateTime.Now.Date ? 3 : 1,
                    MoringStr = morningstr.Trim(),
                    AfterStr=afterstr.Trim(),
                    TaskTotal = listCourse.Count(c => c.StartTime.Date <= newdate && newdate<c.EndTime.Date.AddDays(1))
                });
            }
            //加载后续的几个日子
            AddNextMonthDays(ref calendarTask, quMonth);

            return Json(new { dataList = calendarTask, year = quMonth.Year, month = quMonth.Month },
                        JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新版本更新的查看
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateUserRecord(string version)
        {
            var userID = CurrentUser.UserId;
            var flag = 0;

            var userrecord = Record.GetUserRecord(userID);
            if (userrecord != null)
            {
                if (userrecord.Version.Trim() == version.Trim())
                    flag = 1;
            }
            else
                userrecord = new UpdateRecord();

            userrecord.UserID = userID;
            if (userrecord._id == 0)
            {
                userrecord.Version = version.Trim();
                Record.InsertKey(userrecord);
            }
            else
            {
                if (userrecord.Version.Trim() != version.Trim())
                    userrecord.Version = version.Trim();
                    Record.ModifyKey(userrecord);
            }

            //0：自动弹出；1：不弹出
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        #region 首页广告切换

        /// <summary>
        /// 获取我的培训首页的三种课程
        /// </summary>
        /// <returns></returns>
        public JsonResult GetIndexAdList()
        {
            var isOpenSub = "";
            if (CurrentUser.IsMain == 1)
            {
                isOpenSub = " or IsOpenSub=1";
            }
            var totalCount = 0;
            var sqlwhere = "";
            sqlwhere = string.Format(@" Co_Course.AdFlag=1 and  Co_Course.Way=1 and Co_Course.Publishflag=1 and StartTime>=getdate() and (
        ((
			(select count(*) from Sys_User where UserId={0} and TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)))>0
		) 
		and 
		(
			((Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null) and (Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null )) 
			or 
            (select count(*) from Sys_User where UserId={0} and deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject)))>0  
			or
			(select count(*) from Sys_GroupUser where UserId={0} and groupid in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))>0
		)) or Co_Course.Id in (select CourseId from Cl_CourseOrder where Cl_CourseOrder.UserId = {0} and  Cl_CourseOrder.orderstatus > 0) 
   {1} )", CurrentUser.UserId, isOpenSub);

            //课程列表
            var colist = CourseBL.GetCourseCommonList(out totalCount, sqlwhere, 0, 5, " order by Co_Course.StartTime desc ");
            var list = colist.Select(co => new TempAd
                                               {
                                                   ID = co.Id, Name = co.CourseName, StartDate = co.StartTime, EndDate = co.EndTime, Memo = co.Memo, Teacher = co.TeacherName, Type = 0
                                               }).ToList();
            //公告列表
            int notetotalCount;
            var notelist = noteBL.GetListNote(out notetotalCount, 1, 5, string.Format(@" and sn.PublishFlag={0} AND sn.type={1} and sn.AdFlag={2} ", 1, 0, 1), " order by PublishTime desc");
            list.AddRange(notelist.Select(note => new TempAd
                                                      {
                                                          ID = note.NoteId, Name = note.NoteTitle, StartDate = Convert.ToDateTime(note.publishtime), EndDate = DateTime.Now, Memo = note.NoHtmlNoteContent, Teacher = "", Type = 1
                                                      }));


            var newlist = new List<object>();
            list.OrderByDescending(p=>p.StartDate).Take(5).ToList().ForEach(p => newlist.Add(new
            {
                id = p.ID,
                name = p.Name,
                date = p.StartDate.ToString("yyyy-MM-dd HH:mm"),
                picdate = p.StartDate.ToString("yyyy.MM.dd HH:mm") + "-" + p.EndDate.ToString("yyyy.MM.dd HH:mm"),
                memo = (string.IsNullOrEmpty(p.Memo) ? "" : (p.Memo.Length > 50 ? (p.Memo.Substring(0, 50) + "…") : p.Memo)).HtmlXssEncode(),
                teacher = p.Teacher,
                type=p.Type
            }));
            return Json(newlist, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }

    /// <summary>
    /// 学员首页图片切换
    /// </summary>
    public class TempAd
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Memo { set; get; }
        public string Teacher { set; get; }
        public int Type { set; get; }
    }
}
