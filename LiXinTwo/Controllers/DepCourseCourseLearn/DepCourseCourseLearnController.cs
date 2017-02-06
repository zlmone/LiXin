using LiXinInterface.DepCourseManage;
using LiXinInterface.DeptSurvey;
using LiXinInterface.Examination;
using LiXinInterface.User;
using LiXinModels.DepCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinModels.User;
using System.Text.RegularExpressions;
using LiXinModels;
using System.Data;
using LiXinModels.CourseLearn;
using System.Collections;
using LiXinControllers.Filter;

namespace LiXinControllers
{
    public partial class DepCourseCourseLearnController : BaseController
    {
        private IDep_Course IdepcourseBL;
        private IDep_CourseResource IDepCourseResourceBL;
        private IDep_CoursePaper IDep_CoursePaperBL;
        private IDep_CourseOrder IDep_CourseOrderBL;
        private IDeptSurveyExampaper ISurveyExampaperBL;
        protected IExampaper exampaperBL;
        protected IExamination IExaminationrBL;
        protected IUser userBL;
        protected IDepartment IDepartmentBL;

        public DepCourseCourseLearnController(IDep_Course _IdepcourseBL, IDeptSurveyExampaper _ISurveyExampaperBL,
            IDep_CourseResource _IDepCourseResourceBL, IDep_CoursePaper _IDep_CoursePaperBL,
            IExampaper _exampaperBL, IExamination _IExaminationrBL, IDepartment _IDepartmentBL, IDep_CourseOrder _IDep_CourseOrderBL, IUser _userBL)
        {
            IdepcourseBL = _IdepcourseBL;
            ISurveyExampaperBL = _ISurveyExampaperBL;
            IDepCourseResourceBL = _IDepCourseResourceBL;
            IDep_CoursePaperBL = _IDep_CoursePaperBL;
            exampaperBL = _exampaperBL;
            IExaminationrBL = _IExaminationrBL;
            IDepartmentBL = _IDepartmentBL;
            IDep_CourseOrderBL = _IDep_CourseOrderBL;
            userBL = _userBL;
        }

        #region 页面呈现
      

        public ActionResult MyCourseSubscribe()
        {
            //List<Sys_Department> aa = IDepartmentBL.GetTreeDeptParent(CurrentUser.DeptId.ToString());
            //ViewBag.DeptList = IDepartmentBL.GetTreeDeptParent(CurrentUser.DeptId.ToString());
            ViewBag.DepOrBranch = CurrentUser.SubordinateSubstation.Contains("上海");
            return View();
        }

        public ActionResult CourseDetail(int id)
        {
            var Course = IdepcourseBL.GetCo_Course(id, CurrentUser.UserId);
            ViewBag.Course = Course;

            ViewBag.orderstatus = IDep_CourseOrderBL.GetYuDing(id);
            return View();
        }

        public ActionResult AfterCourse(int id)
        {

            //ViewBag.backurl = backurl;

            var courselist = IdepcourseBL.GetCo_CourseAllList(id);

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

            var Course = IdepcourseBL.GetCo_CourseAllList(id);
            //var Course = CourseOrderBL.GetCourseById(id, CurrentUser.UserId);
            ViewBag.Course = Course;

            var CourseResourse = IDepCourseResourceBL.GetList(id);
            ViewBag.CourseResourse = CourseResourse;
            return View();
        }

        public ActionResult OnlineCourse(int id)
        {

            Dep_Coursepaper CoCoursePaper = IDep_CoursePaperBL.GetCo_CourseMainPaper(id);
            var course = IdepcourseBL.GetCo_CourseAllList(id);

            if (CoCoursePaper != null && course.IsTest == 1)
            {
                var exap = IExaminationrBL.GetExamination(CoCoursePaper.PaperId);
                ViewBag.exap = exap;
                // var examsendstudent = IExaminationrBL.GetExamSendStudent(exap.PaperID);
                var examsendstudent = IExaminationrBL.GetSingletbExamSendStudentByCourseIdAndUserId(id,
                                                                                                    CurrentUser.UserId, 5);
                var exampapger = exampaperBL.GetExampaper(CoCoursePaper.PaperId);
                ViewBag.exampapger = exampapger;
                ViewBag.examsendstudent = examsendstudent;
            }
            ViewBag.Co_CoursePaper = CoCoursePaper;
            ViewBag.courseid = id;
            ViewBag.course = course;

            return View();
        }

        /// <summary>
        /// 课程预定查询
        /// </summary>
        /// <param name="p"></param>
        /// <param name="cp"></param>
        /// <returns></returns>
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


            ViewBag.DeptidList = GetAllSubDepartments();

            return View();
        }

        //详情
        public ActionResult CourseSubscribeDetail(int id)
        {
            int totalcount = 0;
            ViewBag.model = IDep_CourseOrderBL.GetCourseSubscribeList(out totalcount, where: " id=" + id).FirstOrDefault();
            return View();
        }

        //报名详情
        public ActionResult CourseApplyDetail(int id)
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.Id = id;
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
            //if (deptid == 0)
            //{
            //    deptid = CurrentUser.DeptId;
            //}

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
                where += string.Format(" and aa.EndTime <= '{0}'", end.Value);

            if (courseType != 99)
            {
                //where += string.Format(" and aa.Sort= '{0}'", courseType);
                where += " and charindex('" + courseType + "',aa.Sort)>0";
            }

            if (subscribeType != 99)
            {
                if (subscribeType == 1)
                {
                    where += string.Format(" and  aa.StartTime >= '{0}' and  aa.StopOrderFlag=0", DateTime.Now);
                }
                else if (subscribeType == 2)
                {
                    where += string.Format(" and (aa.StartTime < '{0}' and aa.StopOrderFlag=0) ", DateTime.Now);
                }
                else if (subscribeType == 8)
                {
                    where += " and aa.StopOrderFlag=1 ";
                }
            }

            //var list = iDepTran_CourseOrder.GetCourseList(CurrentUser.UserId," Co_Course.id in (select CourseId from DepTran_CourseOpen where DepartId=" + CurrentUser.DeptId + ")");
            int limit = 0;

            var list = IDep_CourseOrderBL.GetDepCourseList(out limit, CurrentUser.UserId, 0, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);

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
                    MyStatusStr = item.MyStatusStr,
                    MyStatus = item.MyStatus,
                    //YearPlan = item.YearPlan,
                    SortStr = item.SortStr,
                    //IsCPA = item.IsCPA == 0 ? "否" : "是",
                    CourseTimes = item.CourseTimesOrder + "/" + item.CourseTimes,
                    Yuding = IDep_CourseOrderBL.GetYuDing(item.CourseId),
                    //IsOpenSubInt = CurrentUser.IsMain == item.IsOpenSub ? 0 : 1,
                    //inopenlevel = item.OpenLevel == null ? 0 : item.OpenLevel.Contains(CurrentUser.TrainGrade) ? 1 : 0,
                    //noopenlevelandfensuo = item.OpenLevel == null ? 0 : item.IsOpenSub == CurrentUser.IsMain ? 1 : 0,
                    //IsMain = CurrentUser.IsMain,
                    //IsOrder = item.IsOrder,
                    StopOrderFlag = item.StopOrderFlag,
                    IsTest = item.IsTest
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

            var error = "";
            int num = 0;
            //var flag = courseOrderBL.GetCanSignup(out num, courseId, CurrentUser.UserId,AllSystemConfigs.Find(p => p.ConfigType == 21).ConfigValue.GetDouble());

            var flag = IDep_CourseOrderBL.GetYuDing(courseId);

            switch (flag)
            {
                case 0:
                    error = "预定失败！";
                    break;
                case 3:
                    error = "预订成功！";
                    break;
                //case 3:
                //    error = "预订失败！";
                //    break;
                case 1:
                    error = "预定失败，排队已关闭！";
                    break;
                case 2:
                    error = "预定失败，报名已关闭！";
                    break;
                default:
                    break;
            }

            if (flag == 3)
            {
                //int deptid = iIDepTran_DepartLeaderConfigBL.GetDepartSettingDepartSetID(CurrentUser.UserId);
                var list = IDep_CourseOrderBL.GetDep_CourseOrderByCourseIdAndUserId(courseId, CurrentUser.UserId);
                if (list == null)
                {
                    Dep_CourseOrder model = new Dep_CourseOrder();

                    model.CourseId = courseId;
                    model.UserId = CurrentUser.UserId;
                    model.OrderTime = DateTime.Now;
                    model.OrderStatus = 1;
                    model.LearnStatus = 0;
                    model.GetScore = 0;
                    model.PassStatus = "0";
                    model.AttScore = 0;
                    model.EvlutionScore = 0;
                    model.ExamScore = 0;
                    model.DepartSetId = CurrentUser.DeptId;
                    model.IsLeave = 0;

                    //model.Reason = "";
                    //model.LeaveTime = DateTime.MinValue;
                    //model.ApprovalFlag = 0;
                    //model.ApprovalTime=DateTime.MinValue;
                    //model.ApprovalUserId = 0;
                    model.OrderTimes = 0;


                    //model.AttScore = 0;
                    //model.CourseId = courseId;
                    //model.GetScore = 0;
                    model.IsAppoint = 0;
                    //model.LearnStatus = 0;
                    //model.OrderStatus = 1;
                    //model.OrderTime = DateTime.Now;
                    //model.PassStatus = "0";
                    //model.UserId = CurrentUser.UserId;
                    //model.EvlutionScore = 0;
                    //model.ExamScore = 0;
                    ////model.DepartSetId = deptid;
                    //model.OrderTimes = 0;

                    IDep_CourseOrderBL.InsertDep_CourseOrder(model);

                    //var attendce = IDep_CourseOrderBL.GetDepAttendce(courseId, CurrentUser.UserId);
                    //if (attendce == null)
                    //{
                    //    Dep_CourseOrder deptran = new Dep_CourseOrder();
                    //    deptran.UserId = CurrentUser.UserId;
                    //    deptran.CourseId = courseId;
                    //    deptran.StartTime = DateTime.Now;
                    //    deptran.EndTime = DateTime.Now;
                    //    deptran.Status = 0;
                    //    deptran.ApprovalFlag = 0;
                    //    deptran.Reason = "";
                    //    deptran.DepartSetId = deptid;

                    //    IDepTran_Attendce.AddDepAttend(deptran);
                    //}
                }
                else
                {
                    Dep_CourseOrder model = new Dep_CourseOrder();

                    model.Id = list.Id;
                    model.CourseId = courseId;
                    model.UserId = CurrentUser.UserId;
                    model.OrderTime = DateTime.Now;
                    model.OrderStatus = 1;
                    model.LearnStatus = 0;
                    model.GetScore = 0;
                    model.PassStatus = "0";
                    model.AttScore = 0;
                    model.EvlutionScore = 0;
                    model.ExamScore = 0;
                    model.DepartSetId = CurrentUser.DeptId;
                    model.IsLeave = 0;
                    //model.Reason = "";
                    //model.LeaveTime = DateTime.MinValue;
                    //model.ApprovalFlag = 0;
                    //model.ApprovalTime = DateTime.MinValue;
                    //model.ApprovalUserId = 0;
                    model.OrderTimes = list.OrderTimes;

                    //model.Id = list.Id;
                    //model.AttScore = 0;
                    //model.CourseId = courseId;
                    //model.GetScore = 0;
                    model.IsAppoint = 0;
                    //model.LearnStatus = 0;
                    //model.OrderStatus = 1;
                    //model.OrderTime = DateTime.Now;
                    //model.PassStatus = "0";
                    //model.UserId = CurrentUser.UserId;
                    //model.EvlutionScore = 0;
                    //model.ExamScore = 0;
                    //model.OrderTimes = list.OrderTimes;

                    IDep_CourseOrderBL.UpdateDep_CourseOrder(model);
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

        #region 课程预定查询
        /// <summary>
        /// 课程预定查询
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCourseSubscribeList(string course, string teaName, int courseOrder, int deptid, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {
            try
            {
                int totalCount = 0;
                //var where = " dc.DeptId IN (SELECT ID FROM f_GetDeptChildByDeptID(" + CurrentUser.DeptId + "))";
                var where = " 1=1";
                if (!string.IsNullOrWhiteSpace(course))
                    where += string.Format(" and dc.CourseName like '%{0}%'", course.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(teaName))
                    where += string.Format(" and su.RealName like '%{0}%'", teaName.ReplaceSql());
                if (courseOrder != 99)
                    where += string.Format(" and dc.IsOrder={0} ", courseOrder);
                if (deptid != 0)
                {
                    where += " and dc.DeptId IN (SELECT ID FROM f_GetDeptChildByDeptID(" + deptid + "))";
                }
              
                var list = IDep_CourseOrderBL.GetCourseSubscribeList(out totalCount, pageIndex, pageSize, where, jsRenderSortField);
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
                    dataList = new List<Dep_Course>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 报名开启关闭
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status">1关闭  0开启</param>
        /// <returns></returns>
        [Filter.SystemLog("报名开启关闭", LogLevel.Info)]
        public JsonResult OrderFlag(int id, int status)
        {
            try
            {
                IDep_CourseOrderBL.UpdateOrderFlag(id, status);
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

        /// <summary>
        /// 获取报名成功的人员，根据培训级别分
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public JsonResult GetDrawSubscribe(int courseId)
        {
            var pie = new ChartModel();
            var column = new ChartModel();
            var pieList = IDep_CourseOrderBL.GetSuccessTrainGradeSubscribe(CurrentUser.DeptId, courseId); //报名成功的 
            var pieSer = new List<pieSeries>();
            var columnSer = new List<Series>();

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
        /// 课程预定查询中能管理的所有人员
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCourseSubscribeUserList(int courseId, string apply, string realName = "", string deptName = "", string IsAppoint = "", int pageSize = 20, int pageIndex = 1, string jsRenderSortField = " UserId")
        {
            try
            {
                int totalcount = 0;
                var where = " 1=1";
                if (apply != "-1")
                {
                    if (apply == "0")
                    {
                        where += @" and ((OrderStatus=0 or OrderStatus is null) or OrderStatus=2 
                        or (orderstatus = 1 and isleave = 1 and approvalflag=1))";
                    }
                    else
                    {
                        where += @" and  ((orderstatus = 1 and isleave = 0 )
				   or (orderstatus = 1 and isleave = 1 and approvalflag in (0,2))) ";
                    }
                }
                if (IsAppoint != "-1")
                {
                    if (IsAppoint == "0")
                    {
                        where += " and (IsAppoint=0 or IsAppoint is null)";
                    }
                    else
                    {
                        where += " and IsAppoint=" + IsAppoint;
                    }
                }
                if (!string.IsNullOrWhiteSpace(deptName))
                    where += string.Format(" and deptName like '%{0}%'", deptName.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(realName))
                    where += string.Format(" and RealName like '%{0}%'", realName.ReplaceSql());
                var list = IDep_CourseOrderBL.GetCourseSubscribeUserList(out totalcount, courseId, pageIndex, pageSize, where, jsRenderSortField);
                var newList = new List<object>();
                list.ForEach(p =>newList.Add(new {
                        p.UserId,
                        p.Realname,
                        p.SexStr,
                        p.DeptName,
                        p.TrainGrade,
                        p.OrderStatusStr,
                        p.DepAppointStr,
                        p.OrderTimeStr}
                        ));
                return Json(new
                {
                    result = 1,
                    dataList = newList,
                    recordCount = totalcount
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

        /// <summary>
        /// 导出课程预定查询中能管理的所有人员
        /// </summary>
        /// <returns></returns>
        public void ExportCourseSubscribeUserList(int courseId, string apply, string realName = "", string deptName = "", string IsAppoint = "-1")
        {
            int totalcount = 0;
            var where = " 1=1";
            if (apply != "-1")
            {
                if (apply == "0")
                {
                    where += " and (OrderStatus=0 or OrderStatus is null)";
                }
                else
                {
                    where += " and OrderStatus=1";
                }
            }
            if (IsAppoint != "-1")
            {
                if (IsAppoint == "0")
                {
                    where += " and (IsAppoint=0 or IsAppoint=-1)";
                }
                else
                {
                    where += " and IsAppoint=1";
                }
            }
            if (!string.IsNullOrWhiteSpace(deptName))
                where += string.Format(" and deptName like '%{0}%'", deptName.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(realName))
                where += string.Format(" and RealName like '%{0}%'", realName.ReplaceSql());
            var list = IDep_CourseOrderBL.GetCourseSubscribeUserList(out totalcount, courseId, where: where);
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
                dt.Rows.Add(count++, item.UserId, item.Realname, item.DeptPath, item.TrainGrade, item.SexStr, item.OrderStatusStr,
                            item.DepAppointStr, item.OrderTimeStr);
            }
            var dtList = new List<DataTable>();
            string strFileName = "课程报名情况";
            dtList.Add(dt);
            var export = new Spreadsheet();
            export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, strFileName);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public JsonResult RemindUserSubscribe(string userIds, int courseId)
        {
            try
            {
                var content = GetFormworkContent(2);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var course = IDep_CourseOrderBL.GetCourseById(courseId);
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
                                messList.Add(new KeyValuePair<string, string>(userList[i].MobileNum, c.Replace("教育培训部",CurrentUser.DeptName)));
                            if (!string.IsNullOrWhiteSpace(userList[i].Email))
                                emailList.Add(new KeyValuePair<string, string>(userList[i].Email, c.Replace("教育培训部", CurrentUser.DeptName)));
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

        /// <summary>
        /// 部门报名详情
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCourseDeptSubscribe(int courseId, string deptName, int? start, int? end, int pageSize = 10, int pageIndex = 1)
        {
            var dataList = new List<NameSubscribeCount>();
            var list = IDep_CourseOrderBL.GetDeptSubscribe(courseId);
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

        /// <summary>
        /// 培训级别报名详情
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCourseTrainGradeSubscribe(int courseId, string trainGrade, int? start, int? end, int pageSize = 10, int pageIndex = 1)
        {
            var list = IDep_CourseOrderBL.GetTrainGradeSubscribe(courseId).OrderBy(p => p.Name).ToList();
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

        #endregion
    }
}
