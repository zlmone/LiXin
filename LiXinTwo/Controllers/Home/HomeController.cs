using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinBLL;
using LiXinBLL.SystemManage;
using LiXinCommon;
using LiXinInterface;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinInterface.DepCourseLearn;
using LiXinInterface.DepCourseManage;
using LiXinInterface.DepLeaveApproval;
using LiXinInterface.DepTranManage;
using LiXinInterface.Home;
using LiXinInterface.NewClassManage;
using LiXinInterface.NewCourseManage;
using LiXinInterface.SystemManage;
using LiXinInterface.User;
using LiXinModels.CourseManage;
using LiXinModels.DepTranAttendce;
using LiXinModels.NewQueryStatistics;
using LiXinModels.SystemManage;
using LiXinModels.User;
using LiXinModels;
using System.Text.RegularExpressions;
using LiXinInterface.ClassRoom;
using System.IO;
using System.Web;
using LiXinInterface.PlanManage;

namespace LiXinControllers.Home
{
    public class HomeController : BaseController
    {
        protected ICo_Course CourseBL;
        protected IUpdateRecord Record;
        protected ICourseOrder CourseOrderBL;
        protected IUser UserBL;
        protected ICl_Attendce IAttendce;
        protected ISys_Notes noteBL;
        protected INewGroupUser NewGroupUserBl;
        protected INewClassRoom NewClassRoomBl;
        protected INew_CourseRoomRule CourseRoomRule;
        protected INewUser nuserBL;
        protected IDepCourseAdvice DepCourseAdvice;
        protected IDepLeaveApproval Approval;
        protected IHome HomeBL;
        protected IDepMyScore DepMyScore;
        protected IMyScore ScoreBL;
        protected IDep_CourseOrder DepCourseOrderBL;
        protected IDepTran_DepartLeaderConfig DepartLeaderConfigBL;
        protected IDepTran_CourseOrder DepTranCourseOrder;
        protected IDept_Notes DepNoteBL;
        protected IDepTran_Attendce DepTranAttBL;
        protected IDep_Attendce DepAttBL;
        protected ITr_YearPlan Iyear;
        public IFree_UserOtherApply userApplyBL;
        public HomeController(ICo_Course courseBL, ICourseOrder courseOrderBL, IUpdateRecord record, IUser userBL, ICl_Attendce iAttendce,
            ISys_Notes _noteBL, INewGroupUser newGroupUserBl, INew_CourseRoomRule courseRoomRule, INewUser _nuserBL,
            IDepCourseAdvice depCourseAdvice, IDepLeaveApproval approval, IHome homeBL, IDepMyScore depMyScore,
            IMyScore scoreBL, IDep_CourseOrder depCourseOrderBL, IDepTran_DepartLeaderConfig departLeaderConfigBL,
            IDepTran_CourseOrder depTranCourseOrder, IDept_Notes depNoteBL, IDepTran_Attendce depTranAttBL,
            IDep_Attendce depAttBL, INewClassRoom newClassRoomBl, ITr_YearPlan _Iyear, IFree_UserOtherApply _userApplyBL)
        {
            CourseBL = courseBL;
            CourseOrderBL = courseOrderBL;
            Record = record;
            UserBL = userBL;
            IAttendce = iAttendce;
            noteBL = _noteBL;
            NewGroupUserBl = newGroupUserBl;
            CourseRoomRule = courseRoomRule;
            nuserBL = _nuserBL;
            DepCourseAdvice = depCourseAdvice;
            Approval = approval;
            HomeBL = homeBL;
            DepMyScore = depMyScore;
            ScoreBL = scoreBL;
            DepCourseOrderBL = depCourseOrderBL;
            DepartLeaderConfigBL = departLeaderConfigBL;
            DepTranCourseOrder = depTranCourseOrder;
            DepNoteBL = depNoteBL;
            DepTranAttBL = depTranAttBL;
            DepAttBL = depAttBL;
            NewClassRoomBl = newClassRoomBl;
            Iyear = _Iyear;
            userApplyBL = _userApplyBL;
        }

        #region 页面呈现

        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// 帮助文档页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ResourceMain()
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
        /// 我的部门分所培训首页
        /// </summary>
        /// <returns></returns>
        public ActionResult MyDepIndex()
        {
            Session["moduleName"] = "MyDepTrain";
            //我的部门学时
            ViewBag.MyScore = DepCourseAdvice.GetMyTotalScore(CurrentUser.UserId,DateTime.Now.Year);
            //获取全年考试的在线配置
            var config = Approval.GetConfig(CurrentUser.DeptId, 5);
            ViewBag.ExamNumber = config != null ? config.ConfigValue : "--";
            //考试通过的次数
            ViewBag.PassNum = HomeBL.GetMyPassExamCount(CurrentUser.UserId, DateTime.Now.Year);

            ViewData["model"] = CurrentUser;
            ViewBag.isMan = CurrentUser.SubordinateSubstation.Contains("上海") ? 1 : 0;
            return View();
        }

        /// <summary>
        /// 部门分所培训管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult DepManageIndex()
        {
            var totalCount = 0;

            var deptIDs = string.Join(",", GetAllSubDepartments().Select(p => p.DepartmentId));
            //待考勤的课程
            var waitCourseList = new List<TempAd>();
            //1、待考勤的转播课程
            var uid = CurrentUser == null ? 0 : CurrentUser.UserId;
            var depConfiglist = DepartLeaderConfigBL.GetDepartSettingByUserId(uid);
            if (depConfiglist.Count > 0)
            {
                var depID = depConfiglist.First().Id;
                var deptattendceList = DepTranAttBL.GetDepAttendceList(string.Join(",", depID.ToString()), out totalCount, 1, 4, "StartTime DESC", string.Format(" temp.AttFlag =0 and temp.ApprovalFlag =0 and temp.StartTime<getdate() "));
                deptattendceList.ForEach(p => waitCourseList.Add(new TempAd { Teacher = p.TeacherStr, ID = p.CourseID, DeptId = depID, EndDate = p.EndTime, StartDate = p.StartTime, Name = p.CourseName, Type = 1, Way = 0 }));
            }
            //2、带考勤的部门分所课程
            var depcourse = DepAttBL.GetDepAttendceList(deptIDs, out totalCount, 1, 4, "StartTime DESC", string.Format(" temp.AttFlag =0 and temp.ApprovalFlag =0 and temp.StartTime<getdate()"));
            depcourse.ForEach(p => waitCourseList.Add(new TempAd { Teacher = p.TeacherStr, ID = p.CourseID, DeptId = CurrentUser.DeptId, EndDate = p.EndTime, StartDate = p.StartTime, Name = p.CourseName, Type = 2, Way = p.Way }));

            //待审批的请假
            var config = Approval.GetConfig(CurrentUser.DeptId, 7);
            var hour = config == null ? 999 : Convert.ToDecimal(config.ConfigValue);
            var sqlWhere = string.Format(" and o.ApprovalFlag=0 and dateadd(minute,{0},o.leavetime)>getdate() ", Convert.ToInt32(hour * 60));
            var deptId = string.Join(",", GetAllSubDepartments().Select(p => p.DepartmentId));
            if (!string.IsNullOrEmpty(deptId))
            {
                sqlWhere += string.Format(" and u.DeptId in ({0}) ", deptId);
            }
            var timeoutApprovalList = Approval.GetDepLeaveApprovalList(1, 4, " o.LeaveTime desc ", sqlWhere, Convert.ToInt32((hour * 60)));

            //课程预约情况
            var courseOrderList = new List<TempCourse>();
            //1、部门分所
            var where = CurrentUser.TrainMaster == 0 ? " dc.DeptId IN (SELECT ID FROM f_GetDeptChildByDeptID(" + CurrentUser.DeptId + "))"
                        : string.Format(" dc.DeptId in ({0})", deptIDs);
            var list = DepCourseOrderBL.GetCourseSubscribeList(out totalCount, 1, 6, where, " StartTime desc ");
            list.ForEach(p => courseOrderList.Add(new TempCourse { CourseName = p.CourseName, StartTime = p.StartTime, EndTime = p.EndTime, Flag = 2, NumberLimited = p.NumberLimited, SuccessEntered = p.HasEntered }));

            ViewBag.waitCourseList = waitCourseList.OrderByDescending(p => p.StartDate).Take(4).ToList();
            ViewBag.timeoutApprovalList = timeoutApprovalList;
            ViewBag.courseOrderList = courseOrderList;
            ViewBag.UserRights = UserRights;
            ViewBag.isMan = CurrentUser.SubordinateSubstation.Contains("上海") ? 1 : 0;
            return View();
        }

        /// <summary>
        /// 我的培训首页
        /// </summary>
        /// <returns></returns>
        public ActionResult MyTrainIndex()
        {
            Session["moduleName"] = "MyTrain";
            var uid = CurrentUser == null ? 0 : CurrentUser.UserId;
            var usermodel = InitSuper;
            var year = DateTime.Now.Year;
            var yearlist = Iyear.GetAllYear(" Year=" + year);
            if (yearlist.Count() == 0)
            {
                year = DateTime.Now.Year - 1;
            }
            //所内目标学时
            var result = "0";
            //CPA目标学时
            var cpAresult = "0";
            //个人总学时（除CPA）
            decimal userscore = 0;
            //全年补预定次数
            var ordercount = 0;


            decimal FreetScore = 0;
            decimal FreeCPAScore = 0;
            if (uid != 0)
            {
                //CPA目标学时
                var cpazq1 = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 16);
                cpAresult = cpazq1.ConfigValue == "" ? "0" : cpazq1.ConfigValue;

                //计算个人学时
                var cpazq2 = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 14);
                userscore = UserBL.GetUserScore(uid, year, cpazq2);

                usermodel = UserBL.GetmodelCAP(uid, year);
                var cpazq = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 13);
                var mianstr = cpazq.ConfigValue + ";";
                var substr = "(?<=" + usermodel.TrainGrade + "-).*?(?=;)";
                //string substr = "(?<=B-).*?(?=;)";
                result = Regex.Match(mianstr, substr).Value == "" ? "0" : Regex.Match(mianstr, substr).Value;



                //全年补预定次数
                var cpazq3 = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 20);
                ordercount = Convert.ToInt32(cpazq3.ConfigValue);
                if (usermodel.ordertimes > ordercount)
                {
                    usermodel.ordertimes = ordercount;
                }


                #region 计算CPA有效学时
                //判断 0:部门or 1:分所
                int Isdep = CurrentUser == null ? 0 : (CurrentUser.SubordinateSubstation.Contains("上海") ? 0 : 1);
                List<Co_CourseShow> MyCPACourse = UserBL.GetMyCPACourse(uid, year, "DESC");

                //var sumLength = MyCPACourse.Sum(p => p.GetLength);//一二期全部CPA学时
                //一期CPA有效学时（即一期全部CPA学时）
                var coLength = MyCPACourse.Where(p => p.CPAForm == 0).Sum(pp => pp.GetLength);
                //二期计算有效学时
                string confValue = AllSystemConfigs.Where(p => p.ConfigType == 14).FirstOrDefault().ConfigValue;
                var deplength = MyCPACourse.Where(p => p.CPAForm == 1).Sum(pp => pp.GetLength);
                //部门学时上限
                int depValue = Convert.ToInt32(confValue.Split(';')[2].Split(',')[0]);
                //分所学时上限
                int deptValue = Convert.ToInt32(confValue.Split(';')[3].Split(',')[0]);
                if (Isdep == 1 && deptValue != -1 && deplength > deptValue)
                {
                    deplength = deptValue;
                }
                if (Isdep == 0 && depValue != -1 && deplength > depValue)
                {
                    deplength = depValue;
                }
                usermodel.CPAScore = coLength + deplength;
                #endregion

                #region 其他形式、免修
                var userFreeList = userApplyBL.GetAllMyScore(year.ToString(), CurrentUser.UserId);
                userscore = userscore + userFreeList.Sum(p => p.GettScore);
                FreetScore = userFreeList.Where(p => p.ApplyType==2).Sum(p => p.GettScore);
                usermodel.CPAScore = usermodel.CPAScore + userFreeList.Sum(p => p.GetCPAScore);
                FreeCPAScore = userFreeList.Where(p => p.ApplyType == 2).Sum(p => p.GetCPAScore);

                var freeConfig = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == year).FirstOrDefault();
                if (!(freeConfig == null || freeConfig.ConfigValue == ""))
                {
                    var configvalue = freeConfig.ConfigValue.Split(';');
                    var tDate = year + "-" + configvalue[0].Split(',')[0];
                    var tScore = configvalue[0].Split(',')[1];
                    var CPADate = year + "-" + configvalue[1].Split(',')[0];
                    var CPAScore = configvalue[1].Split(',')[1];

                    if (CurrentUser.JoinDate != null)
                    {
                        if (CurrentUser.JoinDate >= Convert.ToDateTime(tDate))
                        {
                            userscore = userscore + Convert.ToDecimal(tScore);
                            FreetScore = FreetScore + Convert.ToDecimal(tScore);
                        }
                    }
                    if (CurrentUser.CPACreateDate != null)
                    {
                        if (CurrentUser.CPACreateDate >= Convert.ToDateTime(CPADate))
                        {
                            usermodel.CPAScore = usermodel.CPAScore + Convert.ToDecimal(CPAScore);
                            FreeCPAScore = FreeCPAScore + Convert.ToDecimal(CPAScore);
                        }
                    }
                }
                #endregion
            }
            ViewData["model"] = usermodel;
            ViewBag.PayGrade = CurrentUser.PayGrade;

            var sysParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 33);
            //读参数配置获取全年在线测试完成次数
            ViewBag.ALLOnLineTestNum = sysParamConfig.ConfigValue;

            ViewBag.PassOnLineTestNum = IAttendce.GetMyExamListPassCount(CurrentUser.UserId, CurrentUser.TrainGrade, CurrentUser.DeptId, " ((way=1 and PassStatus=1) or (way=2 and IsPass=1)) and YearPlan=" + DateTime.Now.Year);
            ViewBag.CourseLe = result;
            ViewBag.UserScore = userscore;
            ViewBag.CPACourseLe = cpAresult;
            ViewBag.OrderCount = ordercount;


            ViewBag.FreetScore = FreetScore;
            ViewBag.FreeCPAScore = FreeCPAScore;
            return View();
        }

        /// <summary>
        /// 我的培训首页(新进员工)
        /// </summary>
        /// <returns></returns>
        public ActionResult NewMyTrainIndex()
        {
            Session["moduleName"] = "NewMyTrain";
            int newCount = 0;
            var showList = GetShowScoreList();
            nuserBL.GetNewUserList(out newCount);
            ViewBag.TotalPerson = newCount;
            var newShowList = showList.Where(p => p.UserID == CurrentUser.UserId).ToList();
            var showUser = newShowList.Count > 0
                               ? newShowList[0]
                               : new ShowScore();
            var score = showUser;
            var uid = CurrentUser == null ? 0 : CurrentUser.UserId;
            var usermodel = InitSuper;
            if (uid > 0)
            {
                usermodel = CurrentUser;
            }
            ViewData["model"] = usermodel;
            ViewBag.Score = score;
            ViewBag.TotalScore = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 34).ConfigValue.Split(',')[0];
            ViewBag.MyClass = NewGroupUserBl.GetClassInfoByUserID(CurrentUser.UserId);
            //我的座位
            //var roomrule = CourseRoomRule.GetRoomRuleForSeat(string.Format(" b.PublicFlag=1 ")).Where(p => p.SeatDetail != "");
            var roomrule = NewClassRoomBl.GetRoomList().Where(p => p.DisSeat != "");
            //集中授课座位
            var bigseatdetail = "";
            if (roomrule.Count(p => p.SeatType == 0 && p.IsDelete == 0) > 0)
            {
                foreach (var list in roomrule.Where(p => p.SeatType == 0 && p.IsDelete == 0).OrderByDescending(p => p.Id))
                {
                    foreach (var str in list.DisSeat.Split(':').Where(str => IsNum(str.Split(',')[2]) ? Convert.ToInt32(str.Split(',')[2]) == CurrentUser.UserId : 1 == 1))
                    {
                        if (IsNum(str.Split(',')[0]) && IsNum(str.Split(',')[1]))
                        {
                            bigseatdetail = (Convert.ToInt32(str.Split(',')[0]) + 1) + "排" + (Convert.ToInt32(str.Split(',')[1]) + 1) + "座";
                        }
                        break;
                    }
                    if (bigseatdetail != "")
                        break;
                }
            }
            ViewBag.BigSeat = bigseatdetail;

            //分组带教座位
            var smallseatdetail = "";
            if (roomrule.Count(p => p.SeatType == 1 && p.IsDelete == 0) > 0)
            {
                foreach (var list in roomrule.Where(p => p.SeatType == 1 && p.IsDelete == 0).OrderByDescending(p => p.Id))
                {
                    if ((list.DisSeat.Split(':').Where(str => str.Split(',')[3].StringToInt32() == CurrentUser.UserId)).Any())
                    {
                        foreach (var str in list.DisSeat.Split(':').Where(str => Convert.ToInt32(str.Split(',')[3]) == CurrentUser.UserId))
                        {
                            var last = list.DisSeat.Split(':').Last();
                            int col = Convert.ToInt32(last.Split(',')[1]) + 1;//最大列数
                            int zhuohao = ((Convert.ToInt32(str.Split(',')[0]) * col) + Convert.ToInt32(str.Split(',')[1]) + 1);//桌号
                            smallseatdetail = zhuohao + "桌" + (Convert.ToInt32(str.Split(',')[2]) + 1) + "座";
                            break;
                        }
                        if (smallseatdetail != "")
                            break;
                    }

                }
            }
            ViewBag.SmallSeat = smallseatdetail;

            //考试座位
            #region 旧
            //var examroomrule = CourseRoomRule.GetRoomRuleForSeat(string.Format(" b.PublicFlag=0 and b.CourseName like '考试模板' ")).Where(p => p.SeatDetail != "");
            //var examSeatDetail = "";
            //var examRoom = "";
            //foreach (var list in examroomrule)
            //{
            //    foreach (var str in list.SeatDetail.Split(':').Where(str => Convert.ToInt32(str.Split(',')[2]) == CurrentUser.UserId))
            //    {
            //        examSeatDetail = (Convert.ToInt32(str.Split(',')[0]) + 1) + "排" + (Convert.ToInt32(str.Split(',')[1]) + 1) + "座";
            //        break;
            //    }
            //    if (examSeatDetail == "")
            //        continue;
            //    examRoom = list.RoomName;
            //    break;
            //}
            //ViewBag.ExamSeatDetail = examSeatDetail;
            //ViewBag.ExamRoom = examRoom;
            #endregion

            #region 新
            var examseatdetail = "";
            var examroomName = "";
            if (roomrule.Count(p => p.SeatType == 2 && p.IsDelete == 0) > 0)
            {
                foreach (var list in roomrule.Where(p => p.SeatType == 2 && p.IsDelete == 0).OrderByDescending(p => p.Id))
                {
                    foreach (var str in list.DisSeat.Split(':').Where(str => Convert.ToInt32(str.Split(',')[2]) == CurrentUser.UserId))
                    {
                        examseatdetail = (Convert.ToInt32(str.Split(',')[0]) + 1) + "排" + (Convert.ToInt32(str.Split(',')[1]) + 1) + "座";
                        examroomName = list.RoomName;
                        break;
                    }
                    if (examseatdetail != "")
                        break;
                }
            }
            ViewBag.ExamSeat = examroomName + "，" + examseatdetail;
            #endregion

            return View(score);
        }


        /// <summary>
        /// 版本更新日志呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateRecord()
        {
            return View();
        }

        #endregion

        #region 部门分所学员首页

        /// <summary>
        /// 获取我的考勤
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyAttendnce()
        {
            var totalcount = 0;
            //我的部门自学考勤信息
            var depAtt = DepMyScore.GetCourseShow(out totalcount, CurrentUser.UserId, " 1=1 ", 1, 4);
            //我的视频转播考勤信息
            var tranAtt = ScoreBL.GetCourseShow(out totalcount, CurrentUser.UserId, " 1=1 ", 1, 4);
            depAtt.AddRange(tranAtt);
            var newList = new List<object>();
            depAtt.OrderByDescending(p => p.EndTime).Take(4).ToList().ForEach(p => newList.Add(new
            {
                name = p.CourseName,
                date = "",
                url = 1,
                flag = 1,
                status = p.AttdenceFlag
            }));
            return Json(newList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取首页最新开课
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyDepNewCourse(int type = 0)
        {
            var newList = new List<object>();
            var totalCount = 0;
            if (type == 0)
            {
                //部门自学
                var list = DepCourseOrderBL.GetDepCourseList(out totalCount, CurrentUser.UserId, 0, " StartTime>getdate() ", 0, 8, orderWhere: " 1=1 ");
                list.ForEach(p => newList.Add(new { id = p.CourseId, isorder = p.IsOrder, isOrderStatus = p.isOrderStatus, name = p.CourseName, date = p.StartTime.ToString("yyyy-MM-dd HH:mm") }));
            }
            else
            {
                var deptid = DepartLeaderConfigBL.GetDepartSettingDepartSetID(CurrentUser.UserId);
                //视频转播
                var list = DepTranCourseOrder.GetNewCourseList(out totalCount, CurrentUser.UserId, deptid, " StartTime>getdate() ", 0, 8);
                list.ForEach(p => newList.Add(new { id = p.CourseId, name = p.CourseName, date = p.StartTime.ToString("yyyy-MM-dd HH:mm") }));
            }
            return Json(newList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取某月的任务安排(部门分所课程表)
        /// </summary>
        /// <param name="month">月</param>
        /// <param name="addMonth">所加的月数</param>
        /// <param name="year">年</param>
        /// <returns></returns>
        public JsonResult GetMyDepCalendarTask(string year = "", string month = "", int addMonth = 0)
        {
            year = year == "" ? DateTime.Now.Year.ToString(CultureInfo.InvariantCulture) : year;
            month = month == "" ? DateTime.Now.Month.ToString(CultureInfo.InvariantCulture) : month;
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            //所查询的月的1号
            var quMonth = Convert.ToDateTime(year + "-" + month + "-1").AddMonths(addMonth);

            //部门分所课程
            var totalCount = 0;
            var strwhere = string.Format(" {0} ", string.Format(" (Dep_Course.StartTime between '{0}' and '{1}' )", quMonth.ToString("yyyy-MM-dd"), quMonth.AddMonths(1).ToString("yyyy-MM-dd")));
            var depList = DepCourseOrderBL.GetMyCourseOrderList(out totalCount, CurrentUser.UserId, strwhere, 1, int.MaxValue, "");
            //视频转播课程
            strwhere = string.Format(" {0} ", string.Format(" (Co_Course.StartTime between '{0}' and '{1}' )", quMonth.ToString("yyyy-MM-dd"), quMonth.AddMonths(1).ToString("yyyy-MM-dd")));
            var deptid = DepartLeaderConfigBL.GetDepartSettingDepartSetID(CurrentUser.UserId);
            var tranList = DepTranCourseOrder.GetMyCourseOrderList(out totalCount, CurrentUser.UserId, deptid, strwhere, 1, int.MaxValue, "");
            var listCourse = new List<TempCourse>();
            depList.ForEach(p => listCourse.Add(new TempCourse { CourseName = p.CourseName, StartTime = p.StartTime, EndTime = p.EndTime, Flag = 2 }));
            tranList.ForEach(p => listCourse.Add(new TempCourse { CourseName = p.CourseName, StartTime = p.StartTime, EndTime = p.EndTime, Flag = 1 }));

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
                var mdep = 0;//上午部门分所课程数
                var adep = 0;//下午部门分所课程数
                var mtran = 0;//上午转播课程数
                var atran = 0;//下午转播课程数
                listCourse.Where(c => c.StartTime.Date >= newdate && c.StartTime.Date < newdate.AddDays(1)).ToList().
                    ForEach(p =>
                    {
                        //上午
                        if (p.StartTime >= newdate && p.StartTime < newdate.AddHours(12))
                        {
                            if (!string.IsNullOrEmpty(p.CourseName))
                            {
                                morningstr += " 《" + p.CourseName + "》 ";
                                if (p.Flag == 1)
                                    mtran++;
                                else
                                    mdep++;
                            }
                        }
                        //下午
                        if (p.StartTime >= newdate.AddHours(12) && p.StartTime < newdate.AddDays(1))
                        {
                            if (!string.IsNullOrEmpty(p.CourseName))
                            {
                                afterstr += " 《" + p.CourseName + "》 ";
                                if (p.Flag == 1)
                                    atran++;
                                else
                                    adep++;
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
                    AfterStr = afterstr.Trim(),
                    MFlag = mdep > 0 ? 2 : 1,
                    AFlag = adep > 0 ? 2 : 1,
                    TaskTotal = listCourse.Count(c => c.StartTime.Date <= newdate && newdate < c.EndTime.Date.AddDays(1))
                });
            }
            //加载后续的几个日子
            AddNextMonthDays(ref calendarTask, quMonth);

            return Json(new { dataList = calendarTask, year = quMonth.Year, month = quMonth.Month },
                        JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取首页图片切换
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepIndexAdList()
        {
            var totalCount = 0;
            var colist = new List<TempAd>();
            //部门自学
            var depList = DepCourseOrderBL.GetDepCourseList(out totalCount, CurrentUser.UserId, 0, " StartTime>getdate() and AdFlag=1 ", 0, 5);
            depList.ForEach(co => colist.Add(new TempAd { ID = co.CourseId, Name = co.CourseName, StartDate = co.StartTime, EndDate = co.EndTime, Memo = co.Memo.NoHtml(), Teacher = co.TeacherName, Type = 0 }));//部门自学

            var deptid = DepartLeaderConfigBL.GetDepartSettingDepartSetID(CurrentUser.UserId);
            //视频转播
            var tranList = DepTranCourseOrder.GetNewCourseList(out totalCount, CurrentUser.UserId, deptid, " StartTime>getdate() and AdFlag=1 ", 0, 5);
            tranList.ForEach(co => colist.Add(new TempAd { ID = co.CourseId, Name = co.CourseName, StartDate = co.StartTime, EndDate = co.EndTime, Memo = co.Memo.NoHtml(), Teacher = co.TeacherName, Type = 1 }));//视频转播


            //公告列表
            int notetotalCount;
            var notelist = DepNoteBL.GetListNote(out notetotalCount, 1, 5, string.Format(@" and sn.PublishFlag={0} AND sn.type={1} and sn.AdFlag={2} and sn.DeptId in (select ID from dbo.f_GetDeptParentByDeptID({3}))  ", 1, 0, 1, CurrentUser.DeptId), " order by PublishTime desc");
            colist.AddRange(notelist.Select(note => new TempAd { ID = note.NoteId, Name = note.NoteTitle, StartDate = Convert.ToDateTime(note.publishtime), EndDate = DateTime.Now, Memo = note.NoHtmlNoteContent, Teacher = "", Type = 2 }));//公告

            var newlist = new List<object>();
            colist.OrderByDescending(p => p.StartDate).Take(5).ToList().ForEach(p => newlist.Add(new
            {
                id = p.ID,
                name = p.Name,
                date = p.StartDate.ToString("yyyy-MM-dd HH:mm"),
                picdate = p.StartDate.ToString("yyyy.MM.dd HH:mm") + "-" + p.EndDate.ToString("yyyy.MM.dd HH:mm"),
                memo = (string.IsNullOrEmpty(p.Memo) ? "" : (p.Memo.Length > 50 ? (p.Memo.Substring(0, 50) + "…") : p.Memo)).HtmlXssEncode(),
                teacher = p.Teacher,
                type = p.Type
            }));
            return Json(newlist, JsonRequestBehavior.AllowGet);
        }

        #endregion


        /// <summary>
        /// 获取我的培训首页的三种课程
        /// </summary>
        /// <param name="type">0:集中授课；1：视频；2：CPA</param>
        /// <returns></returns>
        public JsonResult GetMyTrainIndexCourse(int type = 0)
        {
            var isOpenSub = "";
            if (CurrentUser.IsMain == 1)
            {
                isOpenSub = " or IsOpenSub=1";
            }
            var totalCount = 0;
            var sqlwhere = "";

            switch (type)
            {
                case 0:
                    sqlwhere = string.Format(@" Co_Course.Way=1 and Co_Course.Publishflag=1 and StartTime>=getdate() and (
        ((
			(select count(*) from Sys_User where UserId={0} and TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)))>0  
			--or Co_Course.OpenLevel = '' or Co_Course.OpenLevel is null 
            and isorder in(1,3)
		) 
		and 
		(
			((Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null) and (Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null )) 
			or 
            (select count(*) from Sys_User where UserId={0} and deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject)))>0  
			or
			(select count(*) from Sys_GroupUser where UserId={0} and groupid in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))>0
		)) or Co_Course.Id in (select CourseId from Cl_CourseOrder where Cl_CourseOrder.UserId = {0} and  Cl_CourseOrder.orderstatus > 0) 
        or (IsOrder in(2,3) and charindex('{0}',co_course.OpenPerson)>0 and StartTime>=getdate())
   {1} )  ", CurrentUser.UserId, isOpenSub);
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

            List<Co_Course> list = new List<Co_Course>();
            if (type == 1 || type == 2)
            {
                list = CourseBL.GetCourseCommonList(out totalCount, sqlwhere, 0, 8, " order by Co_Course.StartTime desc ");
            }
            if (type == 0)
            {
                list = CourseBL.GetCourseCommonListByJiZhong(out totalCount, CurrentUser.UserId, sqlwhere, 0, 8, " order by Co_Course.StartTime desc ");
            }
            var newlist = new List<object>();
            list.ForEach(p => newlist.Add(new
                                             {
                                                 id = p.Id,
                                                 name = p.CourseName.HtmlXssEncode(),
                                                 date = p.StartTime.ToString("yyyy-MM-dd HH:mm"),
                                                 picdate = p.StartTime.ToString("yyyy.MM.dd HH:mm") + "-" + p.EndTime.ToString("yyyy.MM.dd HH:mm"),
                                                 length = p.CourseLength,
                                                 teacher = p.TeacherStr,
                                                 isornoorder = p.isornoorder
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
            var list = CourseOrderBL.GetMyLeaveList(out totalCount, sqlwhere, 0, 4, orderwhere);
            var newlist = new List<object>();
            list.ForEach(p => newlist.Add(new
                                             {
                                                 name = p.CourseName,
                                                 date = p.LeaveTimeStr,
                                                 status = p.ApprovalStatusStr,
                                                 flag = 0,
                                                 url = 0
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
                flag = 0
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
                flag = 0,
                url = 1
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
            var listCourse = CourseOrderBL.GetMyCourseShedule(CurrentUser.UserId, strwhere);

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
                    AfterStr = afterstr.Trim(),
                    TaskTotal = listCourse.Count(c => c.StartTime.Date <= newdate && newdate < c.EndTime.Date.AddDays(1))
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
            sqlwhere = string.Format(@" Co_Course.AdFlag in (1,2) and  Co_Course.Way=1 and Co_Course.Publishflag=1 and StartTime>=getdate() and (
        ((
			(select count(*) from Sys_User where UserId={0} and TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)))>0
            and isorder in(1,3)
		) 
		and 
		(
			((Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null) and (Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null )) 
			or 
            (select count(*) from Sys_User where UserId={0} and deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject)))>0  
			or
			(select count(*) from Sys_GroupUser where UserId={0} and groupid in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))>0
		)) or Co_Course.Id in (select CourseId from Cl_CourseOrder where Cl_CourseOrder.UserId = {0} and  Cl_CourseOrder.orderstatus > 0) 
        or (IsOrder in(2,3) and charindex('{0}',co_course.OpenPerson)>0 and StartTime>=getdate())
   {1} )", CurrentUser.UserId, isOpenSub);

            //课程列表
            var colist = CourseBL.GetCourseCommonList(out totalCount, sqlwhere, 0, 5, " order by Co_Course.StartTime desc ");
            var list = colist.Select(co => new TempAd
                                               {
                                                   ID = co.Id,
                                                   Name = co.CourseName,
                                                   StartDate = co.StartTime,
                                                   EndDate = co.EndTime,
                                                   Memo = co.Memo.NoHtml(),
                                                   Teacher = co.TeacherName,
                                                   Type = 0,
                                                   AdFlag = co.AdFlag,
                                                   room = co.RoomName,
                                                   isTopShow = 0,
                                                   TopTime = DateTime.MinValue
                                               }).ToList();
            //公告列表
            int notetotalCount;
            var notelist = noteBL.GetTop5Note(CurrentUser.UserId, CurrentUser.TrainGrade);



            // var notelist = noteBL.GetListNote(out notetotalCount, 1, 5, string.Format(@" and sn.PublishFlag={0} AND sn.type={1} and sn.AdFlag={2} ", 1, 0, 1), " order by PublishTime desc");
            list.AddRange(notelist.Select(note => new TempAd
                                                      {
                                                          ID = note.NoteId,
                                                          Name = note.NoteTitle,
                                                          StartDate = Convert.ToDateTime(note.publishtime),
                                                          EndDate = DateTime.Now,
                                                          Memo = string.IsNullOrEmpty(note.ContentDesc) ? note.NoHtmlNoteContent : note.ContentDesc,
                                                          ContentDesc = string.IsNullOrEmpty(note.ContentDesc) ? "" : note.ContentDesc,
                                                          Teacher = "",
                                                          Type = 1,
                                                          BackUrlName = string.IsNullOrEmpty(note.BackUrlName) ? "1.jpg" : note.BackUrlName,
                                                          isTopShow = note.isTopShow,
                                                          TopTime = note.TopTime
                                                      }));

            var imageList = noteBL.GetImageSingle(" defalutType>0");
            var courseUrlList = imageList.Where(p => p.defalutType == 2);
            var courseUrl = courseUrlList.Count() == 0 ? "0.jpg" : courseUrlList.FirstOrDefault().RealName;

            var newlist = new List<object>();
            list.OrderByDescending(p => p.isTopShow).ThenByDescending(p => p.TopTime).ThenByDescending(p => p.StartDate).Take(5).ToList().ToList().ForEach(p => newlist.Add(new
            {
                id = p.ID,
                name = p.Name,
                date = p.StartDate.ToString("yyyy-MM-dd HH:mm"),
                picdate = p.StartDate.ToString("yyyy.MM.dd HH:mm") + "-" + p.EndDate.ToString("yyyy.MM.dd HH:mm"),
                memo = (string.IsNullOrEmpty(p.Memo) ? "" : (p.Memo.Length > 50 ? (p.Memo.Substring(0, 50) + "…") : p.Memo)).HtmlXssEncode(),
                teacher = p.Teacher,
                type = p.Type,
                ContentDesc = p.ContentDesc,
                AdFlag = p.AdFlag,
                NoteUrl = p.BackUrlName,
                courseUrl = courseUrl,
                room = p.room
            }));
            // newlist=newlist.
            return Json(newlist, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取我的培训首页（新进员工）
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNewIndexAdList()
        {
            //公告列表
            int notetotalCount;
            var notelist = noteBL.GetListNote(out notetotalCount, 1, 5, string.Format(@" and sn.PublishFlag={0} AND sn.type={1} and sn.DepTrainFlag={2} ", 1, 0, 1), " order by PublishTime desc");

            var newlist = new List<object>();
            notelist.ForEach(p => newlist.Add(new
            {
                id = p.NoteId,
                name = p.NoteTitle,
                date = p.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                picdate = "",
                memo = string.IsNullOrEmpty(p.NoteContent) ? "" : (p.NoteContent.NoHtml().Length > 50 ? (p.NoteContent.NoHtml().Substring(0, 50) + "…") : p.NoteContent.NoHtml()),
                teacher = "",
                type = 1
            }));
            return Json(newlist, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 下载资源
        public void LoadPrincipleFile(string path, string name)
        {

            if (System.IO.File.Exists(Server.MapPath(path)))
            {
                //if (_newcoursefiles.UpdateLoadTimes(id))
                //{
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
                   // downloadfilename = ToHexString(downloadfilename);
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
                //}
            }
            else
            {
                Response.Write("此附件已不存在");
            }
        }

        public JsonResult IsExistFile(string path)
        {
            if (System.IO.File.Exists(Server.MapPath(path)))
            {
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public bool IsNum(string str)
        {
            try
            {
                Convert.ToInt32(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region 解决下载的乱码问题
        /// <summary>  
        /// 为字符串中的非英文字符编码  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        public string ToHexString(string s)
        {
            char[] chars = s.ToCharArray();
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < chars.Length; index++)
            {
                bool needToEncode = NeedToEncode(chars[index]);
                if (needToEncode)
                {
                    string encodedString = ToHexString(chars[index]);
                    builder.Append(encodedString);
                }
                else
                {
                    builder.Append(chars[index]);
                }
            }
            return builder.ToString();
        }

        /// <summary>  
        ///指定 一个字符是否应该被编码  
        /// </summary>  
        /// <param name="chr"></param>  
        /// <returns></returns>  
        private bool NeedToEncode(char chr)
        {
            string reservedChars = "$-_.+!*'(),@=&";
            if (chr > 127)
                return true;
            if (char.IsLetterOrDigit(chr) || reservedChars.IndexOf(chr) >= 0)
                return false;
            return true;
        }

        /// <summary>  
        /// 为非英文字符串编码  
        /// </summary>  
        /// <param name="chr"></param>  
        /// <returns></returns>  
        private string ToHexString(char chr)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(chr.ToString());
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < encodedBytes.Length; index++)
            {
                builder.AppendFormat("%{0}", Convert.ToString(encodedBytes[index], 16));
            }
            return builder.ToString();
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
        public int DeptId { set; get; }
        public string ContentDesc { get; set; }
        public string BackUrlName { get; set; }
        /// <summary>
        /// 0:视频转播；1:自学课程；2：开放至全所；3：过时的 
        /// </summary>
        public int Way { set; get; }
        public int AdFlag { get; set; }
        public string room { get; set; }
        /// <summary>
        /// 是否置顶 1是 0否
        /// </summary>
        public int isTopShow { get; set; }

        /// <summary>
        /// 设置置顶的时间
        /// </summary>
        public DateTime TopTime { get; set; }
    }

    public class TempCourse
    {
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { set; get; }
        /// <summary>
        /// 课程开始时间
        /// </summary>
        public DateTime StartTime { set; get; }
        /// <summary>
        /// 课程结束时间
        /// </summary>
        public DateTime EndTime { set; get; }
        /// <summary>
        /// 1:视频转播；2：部门分所
        /// </summary>
        public int Flag { set; get; }
        /// <summary>
        /// 已报名人数
        /// </summary>
        public int SuccessEntered { set; get; }
        /// <summary>
        /// 允许人数
        /// </summary>
        public int NumberLimited { set; get; }
    }
}
