using LiXinCommon;
using LiXinControllers.Filter;
using LiXinModels;
using LiXinModels.DepCourseManage;
using LiXinModels.DepPlanManage;
using LiXinModels.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace LiXinControllers.DeptPlanManage
{
    public partial class DeptPlanManageController : BaseController
    {
        #region 呈现
        /// <summary>
        /// 月度管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Monthplan()
        {
            ViewBag.DeptID = GetAllSubDepartments().FirstOrDefault().DepartmentId;
            ViewBag.TrainMaster = CurrentUser.TrainMaster;
            return View();
        }

        /// <summary>
        /// 月度分解
        /// </summary>
        /// <param name="id"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns> 
        public ActionResult AddMonthPlan(int id, int deptID, string year = "", string month = "")
        {
            ViewBag.id = id;
            ViewBag.planyear = year;
            ViewBag.planmonth = month;
            ViewBag.deptID = deptID;
            var where = " PublishFlag=1 AND DeptId =" + deptID;
            ViewBag.year = Iyear.GetYear(where);
            ViewBag.trainGrade = AllTrainGrade;
            return View();
        }

        /// <summary>
        /// 月度分解  添加课程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddCourse(int id,int DeptID)
        {
            ViewBag.id = id;
            ViewBag.year = Request.QueryString["year"] ?? DateTime.Now.Year.ToString();
            ViewBag.month = Request.QueryString["month"] ?? DateTime.Now.Month.ToString();
            ViewBag.trainGrade = AllTrainGrade;
            ViewBag.ClassRoomList = roomBL.GetRoomList("  IsDelete=0 and DeptId=" + DeptID);
            return View();
        }

        public ActionResult MonthPlanDetail(int id, int deptID)
        {
            int totalcount = 0;
            ViewBag.id = id;

            ViewBag.monplan = monthBL.GetMonthPlan(out totalcount, DeptID: deptID, where: " tm.Id=" + id).FirstOrDefault();
            ViewBag.trainGrade = AllTrainGrade;
            return View();
        }

        /// <summary>
        /// 年--月 选择框
        /// </summary>
        /// <param name="viewType"> 0:  年度   月度   1： 年月下旬  默认为1</param>
        /// <param name="dataType">0： 年---今年往后推一共5年   1：取年度计划中的年 默认为0</param>
        /// <param name="onclick"> 年度下拉框,以及月度下拉框中的事件  默认为"" </param>
        /// <returns></returns>
        public ActionResult YearPlanMonthSelect(int viewType = 1, int dataType = 0, string onclick = "")
        {
            if (dataType == 0)
            {
                ViewBag.year = DateTime.Now.Year;
            }
            else
            {
                var where = string.Format(@" PublishFlag=1 AND (DeptId IN (SELECT ID FROM dbo.f_GetDeptChildByDeptID({0})) OR
Id in (SELECT YearId FROM Dep_LinkDepart WHERE DeptId={0} ))", CurrentUser.DeptId);
                ViewBag.year = Iyear.GetYear(where);

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
            //根据人员SubordinateSubstation字段区分是 开设人员是分所还是部门  有上海是属于部门  没有上海则是分所

            var deptlist = GetAllSubDepartments();


            ViewBag.DepOrBranch = Convert.ToInt32(CurrentUser.SubordinateSubstation.Contains("上海"));

            if (dataType == 0)
            {
                ViewBag.year = DateTime.Now.Year;
            }
            else
            {
                //ViewBag.year = monthBL.GetYearOfPlan(CurrentUser.DeptId);
                //ViewBag.year = Iyear.GetAllYear(CurrentUser.DeptId, " PublishFlag=1 AND IsDelete=0 and IsOpen=1 ").Select(t => t.Year);
                ViewBag.year = Iyear.GetAllYear(deptlist[0].DepartmentId, " PublishFlag=1 AND IsDelete=0  ").Select(t => t.Year);
            }

            ViewBag.DeptidList = GetAllSubDepartments();
            ViewBag.viewType = viewType;
            ViewBag.dataType = dataType;
            ViewBag.onclick = onclick;
            return View();
        }


        /// <summary>
        /// 年--月 选择框
        /// </summary>
        /// <param name="viewType"> 0:  年度   月度   1： 年月下旬  默认为1</param>
        /// <param name="dataType">0： 年---今年往后推一共5年   1：取年度计划中的年 默认为0</param>
        /// <param name="onclick"> 年度下拉框,以及月度下拉框中的事件  默认为"" </param>
        /// <returns></returns>
        public ActionResult YearMonthSelect(int viewType = 1, int dataType = 0, string onclick = "")
        {
            if (dataType == 0)
            {
                ViewBag.year = DateTime.Now.Year;
            }
            else
            {
                var where = string.Format(@" PublishFlag=1 AND (DeptId IN (SELECT ID FROM dbo.f_GetDeptChildByDeptID({0})) OR
Id in (SELECT YearId FROM Dep_LinkDepart WHERE DeptId={0} ))", CurrentUser.DeptId);
                ViewBag.year = Iyear.GetYear(where);

            }
            ViewBag.viewType = viewType;
            ViewBag.dataType = dataType;
            ViewBag.onclick = onclick;
            return View();
        }

        /// <summary>
        /// 月度差异对比
        /// </summary>
        /// <returns></returns>
        public ActionResult MonthPlanCompare(int deptID)
        {
            var id = Request.QueryString["id"].StringToInt32();
            ViewBag.monthPlan = monthBL.GetPlanCourseCount(id, deptID);
            ViewBag.year = Request.QueryString["year"].StringToInt32();
            ViewBag.month = Request.QueryString["month"];
            ViewBag.DeptID = deptID;
            ViewBag.id = id;

            return View();
        }

        /// <summary>  
        /// 差异对比详情页面
        /// </summary>
        /// <param name="type">0，新增 1，删除 2未变</param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult CompareDetail(int DeptID, int type = 0, int year = 0, string month = "", int id = 0)
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
            ViewBag.DeptID = DeptID;
            return View();
        }


        /// <summary>
        /// 差异对比详情页面
        /// </summary>
        public ActionResult CompareDetailsForUpdate(int DeptID, int type = 0, int year = 0, string month = "", int id = 0)
        {
            ViewBag.type = type;
            ViewBag.year = year;
            ViewBag.month = month;
            ViewBag.id = id;
            ViewBag.DeptID = DeptID;
            return View();
        }
        #endregion

        #region 月度大纲管理
        /// <summary>
        /// 获取计划的基本信息
        /// </summary>
        public JsonResult GetMonthplan(int myDeptID, int deptID = 0, int year = -1, string month = "-1", int publish = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " month asc")
        {
            try
            {
                int totalCount = 0;
                var where = " 1=1";
                var deptwhere = " 1=1";
                if (deptID != 0)
                {
                    deptwhere += " AND ID=" + deptID;
                }
                if (year != -1)
                {
                    where += " and Year=" + year;
                }
                if (month != "-1")
                {
                    where += " and Month='" + month + "'";
                }
                if (publish != -1)
                {
                    where += " and PublishFlag=" + publish;
                }

                var monthList = monthBL.GetMonthPlan(out totalCount, CurrentUser.TrainMaster == 0 ? CurrentUser.DeptId : deptID, pageIndex, pageSize, where, deptwhere, jsRenderSortField);

                monthList.ForEach(p =>
                {
                    if (p.DeptId == myDeptID)
                    {
                        p.isMy = 1;
                    }
                    else
                    {
                        p.isMy = 0;
                    }
                });
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
                    dataList = new List<Dep_MonthPlan>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Filter.SystemLog("发布月度计划", LogLevel.Info)]
        public JsonResult publish(int id)
        {
            try
            {
                monthBL.UpdateMonthPlan(id, " PublishFlag=1");
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
        [Filter.SystemLog("删除月度计划", LogLevel.Info)]
        public JsonResult Delete(int id)
        {
            try
            {

                monthBL.DeleteMonPlanAndCourse(id);
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

        #region 月度大纲分解
        /// <summary>
        /// 获取月度计划的课程基本信息
        /// </summary>
        public JsonResult GetMonthplanCourse(int DeptID, int id = -1, int type = 0, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " cc.month desc")
        {
            try
            {
                int totalCount = 0;
                var where = string.Format(" ty.DeptId={0}", DeptID);
                if (id != -1)
                {
                    where += " AND ty.Id=" + id;
                }

                if (type == 0)
                {
                    pageSize = int.MaxValue;
                }

                var monthList = monthBL.GetMonthCourseDetails(out totalCount, pageIndex, pageSize, where, jsRenderSortField);

                var courseIds = string.Join<int>(",", monthList.Where(p => p.SurveyPaperId.StringToInt32() != 0).Select(p => p.SurveyPaperId.StringToInt32()));
                foreach (var item in monthList)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.PayGrade = item.PayGrade.HtmlXssEncode();
                }
                return Json(new
                {
                    result = 1,
                    dataList = monthList,
                    recordCount = totalCount,
                    courseIds = courseIds
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
        /// 获取年度计划的课程基本信息
        /// </summary>
        public JsonResult GetYearplanCourse(int DeptID, int year = -1, string month = "", string openLevel = "", string courseName = "", int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalCount = 0;
                var where = " 1=1 and ty.DeptId=" + DeptID;
                if (!string.IsNullOrEmpty(openLevel))
                {
                    // where += string.Format(" And cc.OpenLevel LIKE '%{0}%'", openlevel);
                    where += string.Format(" And (SELECT count(*) FROM  dbo.F_SplitIDs(OpenLevel)  WHERE ID  IN (SELECT ID FROM dbo.F_SplitIDs('{0}')) )>0", openLevel);
                }
                if (!string.IsNullOrEmpty(month))
                {
                    where += string.Format(" AND charindex(cc.Month,'{0}')>0", month);
                }
                if (!string.IsNullOrEmpty(courseName))
                {
                    where += string.Format(" AND cc.CourseName LIKE '%{0}%'", courseName);
                }
                if (year != -1)
                {
                    where += " AND ty.Year=" + year;
                }

                var yearList = monthBL.GetYearCourseDetails(out totalCount, pageIndex, pageSize, where);

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
                    dataList = new List<Dep_Course>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 添加月度计划，月计划和课程的关联
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("添加月度计划，月计划和课程的关联", LogLevel.Info)]
        public JsonResult AddMonthPlanAndCourse()
        {
            try
            {
                var monPlan = Request.Form["monPlan"];
                dynamic plandata = Newtonsoft.Json.JsonConvert.DeserializeObject(monPlan);

                var plan = new Dep_MonthPlan()
                {
                    Id = plandata.id,
                    Year = plandata.year,
                    Month = plandata.month,
                    UserId = CurrentUser.UserId,
                    LastUpdateTime = DateTime.Now,
                    DeptId = plandata.deptID
                };

                var monthPlanID = plan.Id;
                var dic = new Dictionary<int, int>();

                //添加新课程之后的ID
                var newAddCourseID = "";
                var publish = 0;

                if (!monthBL.IsExistMonplan(plan.Year, plan.Month, monthPlanID, plan.DeptId))
                {
                    if (monthPlanID == 0)
                    {
                        monthBL.InsertDep_MonthPlan(plan);
                        monthPlanID = plan.Id;
                    }
                    else
                    {
                        var Smonth = monthBL.Get(monthPlanID);
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
                            Dep_Course course = new Dep_Course()
                            {
                                Code = plandata.newCourse[i].Code,
                                CourseName = plandata.newCourse[i].CourseName,
                                PreCourseTime = plandata.newCourse[i].PreCourseTime,
                                OpenLevel = plandata.newCourse[i].OpenLevel,
                                IsMust = plandata.newCourse[i].IsMust,
                                Teacher = plandata.newCourse[i].teacher,
                                SurveyPaperId = plandata.newCourse[i].id,
                                Year = plandata.year,
                                Month = plandata.month,
                                Publishflag = publish,
                                Day = 0,
                                CourseFrom = 1,
                                CourseLength = plandata.newCourse[i].CourseLength,
                                IsCPA = 0,
                                RoomId = plandata.newCourse[i].roomID,
                                IsSystem = plandata.newCourse[i].IsSystem,
                                IsYearPlan = 0
                            };
                            course.CourseName = course.CourseName.HtmlDecode();
                            monthBL.InsertDept_Course(course);

                            if (course.Id > 0 && monthPlanID > 0)
                            {
                                var model = new Dep_MonthPlanCourse()
                                {
                                    MonthId = monthPlanID,
                                    CourseId = course.Id,
                                    IsSystem = plandata.newCourse[i].IsSystem
                                };
                                monthBL.InsertDep_MonthPlanCourse(model);
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
         [Filter.SystemLog("删除计划中的课程，添加之前先全部删除", LogLevel.Info)]
        public JsonResult DeleteMonPlanCourse(int id, string courseID)
        {
            try
            {
                monthBL.DeleteMonPlanCourse(id, courseID);
                monthBL.DeleteCourse(courseID);
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
        public JsonResult GetMonthplanDetailsCourse(string name, string Teaname, string openLevel, string isMust, string isSystem, string Order, int id = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " cc.month desc")
        {
            try
            {

                // var where = " ty.DeptId=" + CurrentUser.DeptId;
                var where = "1=1";
                if (id != -1)
                {
                    where += " AND ty.Id=" + id;
                }
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

                var yearList = monthBL.GetMonthCourseDetails(out totalCount, pageIndex, pageSize, where, jsRenderSortField);
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
                    dataList = new List<Dep_MonthPlan>(),
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
            // var where = string.Format(" ty.DeptId in (SELECT ID FROM  f_GetDeptChildByDeptID({0}))", CurrentUser.DeptId);
            var where = " 1=1";
            if (id != -1)
            {
                where += " AND ty.Id=" + id;
            }
            List<LiXinModels.CourseShow> yearList = monthBL.GetMonthCourseDetails(out totalCount, where: where);
            DataTable outTable = new DataTable();
            outTable.Columns.Add("课程名称", typeof(string));
            outTable.Columns.Add("预计开课时间", typeof(string));
            outTable.Columns.Add("开放级别", typeof(string));
            outTable.Columns.Add("学时", typeof(string));
            outTable.Columns.Add("地点", typeof(string));
            outTable.Columns.Add("授课讲师", typeof(string));
            outTable.Columns.Add("必修/选修", typeof(string));
            outTable.Columns.Add("框架内/外", typeof(string));
            foreach (var v in yearList)
            {
                outTable.Rows.Add(v.CourseName, v.PreCourseTimeStr, v.OpenLevel, v.CourseLength, v.RoomNamestr, v.Realname,
                                  v.IsMustStr, v.IsSystemStr);
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
        public JsonResult GetMonPlanCompare(int year, int DeptID, string month, int pageSize = 10, int pageIndex = 1)
        {
            try
            {

                var list = monthBL.GetMonPlanCompare(year, month, DeptID).OrderBy(p => p.DeleteOrNew.StringToInt32()).ToList();
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
        public JsonResult GetCompareDetail(int deptID, int year, string month, string type, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                var list = monthBL.GetMonPlanCompare(year, month, deptID).Where(p => p.DeleteOrNew == type).ToList();
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
        public JsonResult GetCompareDetailUpdate(int deptID, int year, string month, string type, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                var list = monthBL.GetMonPlanCompare(year, month, deptID).Where(p => p.DeleteOrNew == "0").ToList();
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
        public JsonResult GetMonthByYear(int year, int IsPublishMonth = 0, int DepOrBranch = 0, int flag = 0, int depitd = 0)
        {
            // bool DepOrBranch= CurrentUser.SubordinateSubstation.Contains("上海");

            string configvalue = AllSystemConfigs.Where(p => p.ConfigType == 45).FirstOrDefault().ConfigValue;
            int linmit = 0;
            string html = "";
            string idlist = "";
            var itemArray = new object[] { };
            try
            {
                var monthList = new List<string>();
                if (IsPublishMonth == 0)
                {
                    monthList = monthBL.GetMonthOfConfig(AllSystemConfigs.Where(p => p.ConfigType == 7).FirstOrDefault(), year);
                }
                if (IsPublishMonth == 1)
                {
                    int total = 0;
                    //var where = " tm.DeptId=" + CurrentUser.DeptId + " and tm.PublishFlag=1 ";
                    var where = " tm.DeptId=" + depitd + " and tm.PublishFlag=1 ";
                    if (year != -1)
                    {
                        where += " and Year=" + year;
                    }


                    //var ar = Iyear.GetAllYear(CurrentUser.DeptId, " PublishFlag=1 AND IsDelete=0 ").Select(t => t.Year);
                    //configvalue 1：是 0：否
                    //DepOrBranch 1:部门 0：分所
                    if (DepOrBranch == 1)
                    {
                        if (configvalue.Split(';')[0] == "1")
                        {
                            if (monthList.Count() == 0)
                            {
                                //找出年度课程
                                //monthList = Iyear.GetLinkYearPlanList(CurrentUser.DeptId, 0, 100, "year=" + year);

                                //string strwhere = string.Format("  CourseFrom=0 and Dep_Course.Id in( select CourseId from [Dep_YearPlanCourse] where Yearid in (select Id from dbo.Dep_YearPlan where DeptId=" + CurrentUser.DeptId + " and PublishFlag=1 and yearplan={0})) ", year, 1);
                                string strwhere = string.Format(@"   CourseFrom=0 and (Dep_Course.Id in( select CourseId from [Dep_YearPlanCourse] where Yearid in
     (select Id from dbo.Dep_YearPlan where DeptId=" + depitd + @" and PublishFlag=1 and yearplan={0}))
     or Dep_Course.Id in(  
     select Courseid from Dep_YearPlanCourse where yearid=(
  select Id from Dep_YearPlan where [id] in(
   select YearId from Dep_LinkDepart where deptid=" + depitd + @" and ApprovalFlag=1) and [year]={0})
   )
     )", year, 1);

                                int limt = 0;
                                List<Dep_Course> listCourse = Idep_courseBL.GetCourseCommonList(out limt, strwhere, 0, int.MaxValue);
                                int n = 0;
                                itemArray = new object[listCourse.Count()];
                                foreach (var item in listCourse)
                                {
                                    var temp = new
                                    {
                                        Id = item.Id,
                                        Code = item.Code,
                                        CourseName = item.CourseName,
                                        OpenLevel = item.OpenLevel,
                                        Year = item.Year,
                                        Month = item.Month,
                                        StartTime = item.Day,
                                        EndTime = item.OpenLevel,
                                        IsMust = item.IsMust,
                                        TeacherName = item.TeacherName,
                                        Teacher = item.Teacher,
                                        PreCourseTime = item.PreCourseTime.ToString("yyyy-MM-dd HH:mm"),
                                        CourseLength = item.CourseLength,
                                        IsCPA = item.IsCPA,
                                        RoomId = item.RoomId,
                                        IsSystem = item.IsSystem,
                                        IsYearPlan = item.IsYearPlan
                                    };
                                    itemArray[n] = temp;
                                    n++;
                                }

                            }
                        }
                        else
                        {
                            string beiwhere = string.Format(@" tm.[year]=(
		select [year] from Dep_YearPlan where id=
		(
		 select yearid from Dep_LinkDepart where DeptId={0} and ApprovalFlag=1
		)
	 ) and tm.deptid=
		(
			 select deptid from Dep_YearPlan where id =
			 (
			 select yearid from Dep_LinkDepart where DeptId={0} and ApprovalFlag=1
			 )
		)", depitd);
                            //monthList = monthBL.GetMonthPlan(out total,depitd, where: where).Select(i => i.Month).ToList();

                            monthList = monthBL.GetMonthPlanForMaoJiaYuan(out total, depitd, where: where, beiwhere: beiwhere).Select(i => i.Month).ToList();
                        }
                    }
                    else
                    {
                        if (configvalue.Split(';')[1] == "1")
                        {
                            if (monthList.Count() == 0)
                            {
                                //找出年度课程
                                //monthList = Iyear.GetLinkYearPlanList(CurrentUser.DeptId, 0, 100, "year=" + year);

                                //string strwhere = string.Format("  CourseFrom=0 and Dep_Course.Id in( select CourseId from [Dep_YearPlanCourse] where Yearid in (select Id from dbo.Dep_YearPlan where DeptId=" + CurrentUser.DeptId + " and PublishFlag=1 and yearplan={0})) ", year, 1);
                                string strwhere = string.Format(@"   CourseFrom=0 and (Dep_Course.Id in( select CourseId from [Dep_YearPlanCourse] where Yearid in
     (select Id from dbo.Dep_YearPlan where DeptId=" + depitd + @" and PublishFlag=1 and yearplan={0}))
     or Dep_Course.Id in(  
     select Courseid from Dep_YearPlanCourse where yearid=(
  select Id from Dep_YearPlan where [id] in(
   select YearId from Dep_LinkDepart where deptid=" + depitd + @" and ApprovalFlag=1) and [year]={0})
   )
     )", year, 1);

                                int limt = 0;
                                List<Dep_Course> listCourse = Idep_courseBL.GetCourseCommonList(out limt, strwhere, 0, int.MaxValue);
                                int n = 0;
                                itemArray = new object[listCourse.Count()];
                                foreach (var item in listCourse)
                                {
                                    var temp = new
                                    {
                                        Id = item.Id,
                                        Code = item.Code,
                                        CourseName = item.CourseName,
                                        OpenLevel = item.OpenLevel,
                                        Year = item.Year,
                                        Month = item.Month,
                                        StartTime = item.Day,
                                        EndTime = item.OpenLevel,
                                        IsMust = item.IsMust,
                                        TeacherName = item.TeacherName,
                                        Teacher = item.Teacher,
                                        PreCourseTime = item.PreCourseTime.ToString("yyyy-MM-dd HH:mm"),
                                        CourseLength = item.CourseLength,
                                        IsCPA = item.IsCPA,
                                        RoomId = item.RoomId,
                                        IsSystem = item.IsSystem,
                                        IsYearPlan = item.IsYearPlan
                                    };
                                    itemArray[n] = temp;
                                    n++;
                                }

                            }
                        }
                        else
                        {
                            monthList = monthBL.GetMonthPlan(out total, depitd, where: where).Select(i => i.Month).ToList();
                        }

                    }

                    //if (DepOrBranch == 1)
                    //{
                        if (flag == 0)//0是 还没有组织结构
                        {
                            html += "<div class='seled-list' ><h4 id='sp_chooseDept'>已选组织结构：</h4><ul>";
                            //List<Dep_LinkDepart> list = Iyear.GetALLLinkDepart();
                            //List<Sys_Department> list = Iyear.GetALLLinkDepart(CurrentUser.DeptId, year);

                            List<Sys_Department> list = Iyear.GetALLLinkDepart(depitd, year);

                            foreach (var item in list)
                            {
                                html += "<li id=div_DeptItem_" + item.DepartmentId + " name='year-seled-li'><span title=" + item.DeptName + ">" + item.DeptName + "</span>";
                                html += "<input  type='button' class='btn btn-cancel' name='btn' title='移除' value='X' onclick=fnYearRemoveDeptItem(\'div_DeptItem_" + item.DepartmentId + "\'," + item.DepartmentId + ") />";
                                idlist += item.DepartmentId + ",";
                            }

                            html += "</ul><div class='mt10'><input type='button' class='btn btn-co' id='btnGoOnAddDept' onclick=fnGoOnAddDept() value='继续添加组织结构' /></div></div>";
                        }
                        else//已经有组织结构了
                        {
                            //List<Sys_Department> list = Iyear.GetALLLinkDepart(CurrentUser.DeptId, year);
                            List<Sys_Department> list = Iyear.GetALLLinkDepart(depitd, year);

                            foreach (var item in list)
                            {
                                html += "<li id=div_DeptItem_" + item.DepartmentId + " name='year-seled-li'><span title=" + item.DeptName + ">" + item.DeptName + "</span>";
                                html += "<input  type='button' class='btn btn-cancel' name='btn' title='移除' value='X' onclick=fnYearRemoveDeptItem(\'div_DeptItem_" + item.DepartmentId + "\'," + item.DepartmentId + ") />";
                                idlist += item.DepartmentId + ",";
                            }
                        }
                    //}
                }
                return Json(new
                {
                    result = 1,
                    monthList = monthList,
                    dataList = itemArray,
                    html = html,
                    idlist = idlist.Length == 0 ? "" : idlist.Substring(0, idlist.LastIndexOf(','))
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


        public JsonResult GetDeptParamConfig(int deptid)
        {

            List<int> deptlist = Iyear.GetAllYear(deptid, " PublishFlag=1 AND IsDelete=0 and deptid=" + deptid + " ").Select(t => t.Year).ToList();

            string html = "";

            foreach (var item in deptlist)
            {
                html += " <option value=" + item + ">" + item + "</option>";
            }


            var RefHour = DepConfig(deptid, 3) == "" ? "999" : DepConfig(deptid, 3).Split(';')[0];
            //考试时长
            //ViewBag.RefLength = AllSystemConfigs.Find(s => s.ConfigType == 26).ConfigValue.Split(';')[1];
            var RefLength = DepConfig(deptid, 3) == "" ? "999" : DepConfig(deptid, 3).Split(';')[1];

            var MaxTestTimes = DepConfig(deptid, 4);

            var RefPreAdviceConfigTime = DepConfig(deptid, 2) == "" ? "0" : DepConfig(deptid, 2).Split(';')[0];

            //课后评估时间内
            //ViewBag.RefAfterEvlutionConfigTime = AllSystemConfigs.Find(s => s.ConfigType == 29).ConfigValue.Split(';')[1];
            var RefAfterEvlutionConfigTime = DepConfig(deptid, 2) == "" ? "0" : DepConfig(deptid, 2).Split(';')[1];

            List<Dep_ClassRoom> list = roomBL.GetRoomList(" DeptId=" + deptid + " and IsDelete=0 ");

            string roomhtml = "";

            foreach (var item in list)
            {
                roomhtml += "<option num='" + item.RoomNumber + "' id='sel_room_" + item.Id + "' value='" + item.Id + "'>" + item.RoomName + "</option>";
            }

            return Json(new
            {
                deptlist = html,
                roomhtml = roomhtml,
                RefHour = RefHour,
                RefLength = RefLength,
                MaxTestTimes = MaxTestTimes,
                RefPreAdviceConfigTime = RefPreAdviceConfigTime,
                RefAfterEvlutionConfigTime = RefAfterEvlutionConfigTime
            }, JsonRequestBehavior.AllowGet);



        }



    }
}
