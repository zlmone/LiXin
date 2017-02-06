using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinModels;
using Newtonsoft.Json;
using LiXinModels.CourseManage;
using LiXinCommon;
using LiXinModels.PlanManage;
using System.Data;
using System.Web;

namespace LiXinControllers
{
    public partial class PlanManageController
    {

        #region 呈现
        //public void testMessage()
        //{
        //    var messList = new List<KeyValuePair<string, string>>();
        //    messList.Add(new KeyValuePair<string, string>("15906194763", "测试"));
        //    SendMessage(messList);
        //}
        public ActionResult Monthplan()
        {
            //testMessage();
            ViewBag.year = Imonth.GetYearOfPlan();
            return View();
        }

        public ActionResult AddMonthPlan(int id, string year = "", string month = "")
        {
            //  testMessage();

            ViewBag.id = id;
            ViewBag.planyear = year;
            ViewBag.planmonth = month;
            ViewBag.year = Imonth.GetYearOfPlan().OrderBy(p => p).ToList();
            ViewBag.trainGrade = AllTrainGrade;
            return View();
        }
        public ActionResult AddCourse(int id)
        {
            ViewBag.id = id;
            ViewBag.year = Request.QueryString["year"] ?? DateTime.Now.Year.ToString();
            ViewBag.month = Request.QueryString["month"] ?? DateTime.Now.Month.ToString();
            ViewBag.trainGrade = AllTrainGrade;
            ViewBag.ClassRoomList = classRoomBL.GetRoomList();
            return View();
        }

        public ActionResult MonthPlanDetail(int id)
        {
            int totalcount = 0;
            ViewBag.id = id;
            ViewBag.monplan = Imonth.GetMonthPlan(out totalcount, 1, int.MaxValue, -1, "-1", -1, " and Id=" + ViewBag.id)[0];
            ViewBag.trainGrade = trainBL.GetAllTrainGrade();
            return View();
        }

        public ActionResult MonthPlanCompare()
        {
            var id = Request.QueryString["id"].StringToInt32();
            ViewBag.monthPlan = Imonth.GetPlanCourseCount(id);
            ViewBag.year = Request.QueryString["year"].StringToInt32();
            ViewBag.month = Request.QueryString["month"];
            ViewBag.id = id;

            return View();
        }



        public ActionResult YearMonthSelect(int viewType = 1, int dataType = 0, string onclick = "")
        {
            if (dataType == 0)
            {
                ViewBag.year = DateTime.Now.Year;
            }
            else
            {
                ViewBag.year = Imonth.GetYearOfPlan().OrderBy(p => p).ToList();
            }
            ViewBag.viewType = viewType;
            ViewBag.dataType = dataType;
            ViewBag.onclick = onclick;
            return View();
        }

        public ActionResult YearPlanMonthSelect(int viewType = 1, int dataType = 0, string onclick = "")
        {
            if (dataType == 0)
            {
                ViewBag.year = DateTime.Now.Year;
            }
            else
            {
                ViewBag.year = Imonth.GetYearOfPlan();
            }
            ViewBag.viewType = viewType;
            ViewBag.dataType = dataType;
            ViewBag.onclick = onclick;
            return View();
        }

        /// <summary>
        ///  集中授课开设 专属 ,内有自定义事件~~ (*^__^*) ……    调用 年月 部分视图页面  拷贝 YearMonthSelect 
        /// </summary>
        /// <param name="viewType"></param>
        /// <param name="dataType"></param>
        /// <param name="onclick"></param>
        /// <returns></returns>
        public ActionResult YearMonthSelectForCourse(int viewType = 1, int dataType = 0, string onclick = "")
        {
            if (dataType == 0)
            {
                ViewBag.year = DateTime.Now.Year;
            }
            else
            {
                ViewBag.year = Imonth.GetYearOfPlan();
            }
            ViewBag.viewType = viewType;
            ViewBag.dataType = dataType;
            ViewBag.onclick = onclick;
            return View();
        }
        /// <summary>
        /// 差异对比详情页面
        /// </summary>
        /// <param name="type">0，新增 1，删除 2未变</param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult CompareDetail(int type = 0, int year = 0, string month = "", int id = 0)
        {
            ViewBag.type = type;
            ViewBag.year = year;
            ViewBag.month = month;
            var head = "";
            switch (type)
            {
                case 0:
                    head = "未变的课程";
                    break;
                case 1:
                    head = "删除的课程";
                    break;
                case 2:
                    head = "新增的课程";
                    break;
            }
            ViewBag.head = head;
            ViewBag.id = id;
            return View();
        }

        public ActionResult CompareDetailsForUpdate(int type = 0, int year = 0, string month = "", int id = 0)
        {
            ViewBag.type = type;
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.id = id;
            return View();
        }
        #endregion

        #region 月度计划管理
        /// <summary>
        /// 获取计划的基本信息
        /// </summary>
        public JsonResult GetMonthplan(int year = -1, string month = "-1", int publish = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " month asc")
        {
            try
            {
                int totalCount = 0;

                var monthList = Imonth.GetMonthPlan(out totalCount, pageIndex, pageSize, year, CodeHelper.ReplaceSql(month), publish, jsRenderSortField: jsRenderSortField);

                return Json(new
                {
                    result = 1,
                    dataList = monthList,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Tr_MonthPlan>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult publish(int id)
        {
            try
            {
                Imonth.PublishAndCourse(id);
                return Json(new
                {
                    result = 1,
                    content = "发布成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "发布失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Delete(int id)
        {
            try
            {
                Imonth.Delete(id);
                return Json(new
                {
                    result = 1,
                    content = "删除成功"
                }, JsonRequestBehavior.AllowGet);
            }

            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "删除失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 月度计划分解
        /// <summary>
        /// 获取月度计划的课程基本信息
        /// </summary>
        public JsonResult GetMonthplanCourse(int id = -1, int type = 0, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " cc.month desc")
        {
            try
            {
                int totalCount = 0;
                if (type == 0)
                {
                    pageSize = int.MaxValue;
                }
                var yearList = Imonth.GetMonthCourseDetails(out totalCount, pageIndex, pageSize, id, jsRenderSortField);
                var courseIds = string.Join<int>(",", yearList.Where(p => p.SurveyPaperId.StringToInt32() != 0).Select(p => p.SurveyPaperId.StringToInt32()));
                foreach (var item in yearList)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.PayGrade = item.PayGrade.HtmlXssEncode();
                }
                return Json(new
                {
                    result = 1,
                    dataList = yearList,
                    recordCount = totalCount,
                    courseIds = courseIds
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Tr_MonthPlan>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 获取年度计划的课程基本信息
        /// </summary>
        public JsonResult GetYearplanCourse(int year = -1, string month = "", string openLevel = "", string courseName = "", int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalCount = 0;
                var yearList = Imonth.GetYearCourseDetails(out totalCount, pageIndex, pageSize, openLevel, month, year, CodeHelper.ReplaceSql(courseName));
                foreach (var item in yearList)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.PayGrade = item.PayGrade.HtmlXssEncode();
                    item.Code = item.Code.HtmlXssEncode();
                }
                return Json(new
                {
                    result = 1,
                    dataList = yearList,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<CourseShow>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 添加月度计划，月计划和课程的关联
        /// </summary>
        /// <returns></returns>
        public JsonResult AddMonthPlanAndCourse()
        {
            try
            {
                var monPlan = Request.Form["monPlan"];
                dynamic plandata = Newtonsoft.Json.JsonConvert.DeserializeObject(monPlan);

                var plan = new Tr_MonthPlan()
                 {
                     Id = plandata.id,
                     Year = plandata.year,
                     Month = plandata.month,
                     UserId = CurrentUser.UserId
                 };

                var monthPlanID = plan.Id;
                var dic = new Dictionary<int, int>();

                //添加新课程之后的ID
                var newAddCourseID = "";
                var publish = 0;

                if (!Imonth.IsExistMonplan(plan.Year, plan.Month, monthPlanID))
                {
                    if (monthPlanID == 0)
                    {
                        Imonth.InsertTr_MonthPlan(plan);
                        monthPlanID = plan.Id;
                    }
                    else
                    {
                        var Smonth = Imonth.Get(monthPlanID);
                        publish = Smonth == null ? 0 : Smonth.PublishFlag;
                        string courses = plandata.deleteCourse;
                        //如果是修改，需要添加新的课程的话，先把课程全部删了，我厉害吧
                        if (!string.IsNullOrEmpty(courses))
                        {
                            DeleteMonPlanCourse(monthPlanID, courses);
                        }
                    }

                    #region 添加课程 课程与计划的关联
                    if (plandata.newCourse.Count > 0)
                    {
                        for (int i = 0; i < plandata.newCourse.Count; i++)
                        {
                            Co_Course course = new Co_Course()
                            {
                                Code = plandata.newCourse[i].Code,
                                CourseName = plandata.newCourse[i].CourseName,
                                PreCourseTime = plandata.newCourse[i].PreCourseTime,
                                OpenLevel = plandata.newCourse[i].OpenLevel,
                                IsMust = plandata.newCourse[i].IsMust,
                                Way = plandata.newCourse[i].Way,
                                Teacher = plandata.newCourse[i].teacher,
                                SurveyPaperId = plandata.newCourse[i].id,
                                Year = plandata.year,
                                Month = plandata.month,
                                Publishflag = publish,
                                Day = 0,
                                CourseFrom = 1,
                                CourseLength = plandata.newCourse[i].CourseLength,
                                IsCPA = plandata.newCourse[i].IsCPA,
                                RoomId = plandata.newCourse[i].roomID,
                                IsSystem = plandata.newCourse[i].IsSystem,
                                IsYearPlan = 0
                            };
                            course.CourseName = course.CourseName.HtmlDecode();
                            Imonth.InsertCo_Course(course);

                            if (course.Id > 0 && monthPlanID > 0)
                            {
                                var model = new Tr_MonthPlanCourse()
                                {
                                    MonthId = monthPlanID,
                                    CourseId = course.Id,
                                    IsSystem = plandata.newCourse[i].IsSystem
                                };
                                Imonth.InsertTr_MonthPlanCourse(model);
                            }

                        }
                    }
                    #endregion

                    return Json(new
                   {
                       result = 1,
                       monthPlanID = monthPlanID,
                       content = "添加成功"
                   }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        result = 0,
                        content = plan.Year + "年" + plan.Month + "：此月度计划已经存在，请选择其他的时间"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "添加失败"
                }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 删除计划中的课程，添加之前先全部删除
        /// </summary>
        public JsonResult DeleteMonPlanCourse(int id, string courseID)
        {
            try
            {
                Imonth.DeleteMonPlanCourse(id, courseID);
                Imonth.DeleteCourse(courseID);
                return Json(new
                {
                    result = 1,
                    content = "删除成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "删除失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region 月度计划详情
        /// <summary>
        /// 获取月度计划的课程基本信息
        /// </summary>
        public JsonResult GetMonthplanDetailsCourse(string name, string Teaname, string Way, string openLevel, string isMust, string isSystem, string Order, int id = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " cc.month desc")
        {
            try
            {
                string where = "1=1";
                int totalCount = 0;
                if (!string.IsNullOrEmpty(openLevel))
                {
                    // where += string.Format(" And cc.OpenLevel LIKE '%{0}%'", openlevel);
                    where += string.Format(" And (SELECT count(*) FROM  dbo.F_SplitIDs(OpenLevel)  WHERE ID  IN (SELECT ID FROM dbo.F_SplitIDs('{0}')) )>0", openLevel);
                }
                if (!string.IsNullOrEmpty(isMust))
                {
                    where += string.Format(" and cc.IsMust ={0}", Convert.ToInt32(isMust));
                }
                if (!string.IsNullOrEmpty(isSystem))
                {
                    if (Convert.ToInt32(isSystem) == 0)
                    {
                        where += string.Format(" and tp.IsSystem=0 ");
                    }
                    else
                    {
                        where += string.Format(" and tp.IsSystem>0 ");
                    }
                }
                if (!string.IsNullOrEmpty(name))
                {
                    where += string.Format(" and cc.CourseName LIKE '%{0}%'", name.ReplaceSql());
                }
                if (!string.IsNullOrEmpty(Teaname))
                {
                    where += string.Format(" and su.Realname LIKE '%{0}%'", Teaname.ReplaceSql());
                }
                if (Way != "-1")
                {
                    where += string.Format(" and cc.Way={0}", Convert.ToInt32(Way));
                }

                var yearList = Imonth.GetMonthCourseDetails(out totalCount, pageIndex, pageSize, id, jsRenderSortField, where);
                var courseIds = string.Join<int>(",", yearList.Where(p => p.SurveyPaperId.StringToInt32() != 0).Select(p => p.SurveyPaperId.StringToInt32()));
                foreach (var item in yearList)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.PayGrade = item.PayGrade.HtmlXssEncode();
                }
                return Json(new
                {
                    result = 1,
                    dataList = yearList,
                    recordCount = totalCount,
                    courseIds = courseIds
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Tr_MonthPlan>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 导出年度计划课程信息
        /// </summary>
        public void OutMonthCourse(int id, string month)
        {
            int totalCount = 0;
            List<CourseShow> yearList = Imonth.GetMonthCourseDetails(out totalCount, 1, int.MaxValue, id);
            DataTable outTable = new DataTable();
            outTable.Columns.Add("课程名称", typeof(string));
            outTable.Columns.Add("预计开课时间", typeof(string));
            outTable.Columns.Add("开放级别", typeof(string));
            outTable.Columns.Add("培训形式", typeof(string));
            outTable.Columns.Add("学时", typeof(string));
            outTable.Columns.Add("是否可折算CPA学时", typeof(string));
            outTable.Columns.Add("地点", typeof(string));
            outTable.Columns.Add("授课讲师", typeof(string));
            outTable.Columns.Add("讲师薪酬", typeof(string));
            outTable.Columns.Add("必修/选修", typeof(string));
            outTable.Columns.Add("框架内/外", typeof(string));
            foreach (var v in yearList)
            {
                outTable.Rows.Add(v.CourseName, v.PreCourseTimeStr, v.OpenLevel, v.WayStr, v.CourseLength, v.IsCPAstr, v.RoomNamestr, v.Realname,
                                  v.PayGrade, v.IsMustStr, v.IsSystemStr);
            }
            new Spreadsheet().Template(month.Replace("-", "年") + "的月度计划课程安排", null, outTable, HttpContext, "月度计划课程", "月度计划");
        }
        #endregion

        #region 月度差异对比
        /// <summary>
        /// 月度差异对比
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetMonPlanCompare(int year, string month, int pageSize = 10, int pageIndex = 1)
        {
            try
            {

                var list = Imonth.GetMonPlanCompare(year, month).OrderBy(p => p.DeleteOrNew.StringToInt32()).ToList();
                int totalCount = list.Count;

                #region 柱壮图

                var res = new ChartModel();
                //X轴
                List<String> xAxis = new List<String>()
                {
                    "新增",
                    "未变",
                    "删除"
                };
                //数据
                List<Series> itemArray = new List<Series>();

                var List = new List<double>();

                List.Add((double)list.Count(p => p.DeleteOrNew == "2"));
                List.Add((double)list.Count(p => p.DeleteOrNew == "0"));
                List.Add((double)list.Count(p => p.DeleteOrNew == "1"));

                Series Add = new Series
                {
                    name = "",
                    data = List
                };

                itemArray.Add(Add);
                res.DivID = "showchar";
                res.title = "月度计划差异对比";
                res.xAxis = xAxis;
                res.series = itemArray;
                //图下面的小方块，因为此处只有一种颜色，所以false，可以不传或true就能显示
                //res.legend = false;
                #endregion
                list = list.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalCount,
                    res = res
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<CourseShow>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 差异对比详情
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCompareDetail(int year, string month, string type, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                var list = Imonth.GetMonPlanCompare(year, month).Where(p => p.DeleteOrNew == type).ToList();
                var totalcount = list.Count;
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(new
                {
                    result = 0,
                    dataList = new List<CourseShow>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 未变的差异对比
        /// </summary>
        /// <returns></returns>
        /// <param name="type">1开发级别更改  2授课讲师更改  3其他</param>
        public JsonResult GetCompareDetailUpdate(int year, string month, string type, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                var list = Imonth.GetMonPlanCompare(year, month).Where(p => p.DeleteOrNew == "0").ToList();
                var courseList = new List<CourseShow>();
                switch (type)
                {
                    case "1":
                        courseList = list.Where(p => p.OpenLevel != p.OpenLevelUpdate).ToList();
                        break;
                    case "2":
                        courseList = list.Where(p => p.Realname != p.teacherUpdate).ToList();
                        break;
                    case "3":
                        courseList = list.Where(p => p.OpenLevel == p.OpenLevelUpdate && p.Realname == p.teacherUpdate).ToList();
                        break;
                }
                foreach (var item in courseList)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.PayGrade = item.PayGrade.HtmlXssEncode();
                }

                var totalcount = courseList.Count;
                courseList = courseList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new
                {
                    result = 1,
                    dataList = courseList,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(new
                {
                    result = 0,
                    dataList = new List<CourseShow>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 公共
        /// <summary>
        /// 获取配置当中的年度设定
        /// </summary>
        /// <param name="year"></param>
        /// <param name="IsPublishMonth">0: 读取配置中的月份 1 读取已经发布的月份</param>
        /// <returns>月份  形如 2013-5</returns>
        public JsonResult GetMonthByYear(int year, int IsPublishMonth = 0)
        {
            try
            {
                var monthList = new List<string>();
                if (IsPublishMonth == 0)
                {
                    monthList = Imonth.GetMonthOfConfig(AllSystemConfigs.Where(p => p.ConfigType == 7).FirstOrDefault(), year);
                }
                if (IsPublishMonth == 1)
                {
                    int total = 0;
                    monthList = Imonth.GetMonthPlan(out total, 1, int.MaxValue, year, "-1", 1, "").Select(i => i.Month).ToList();
                }
                return Json(new
                {
                    result = 1,
                    monthList = monthList
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    monthList = new List<string>()
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}
