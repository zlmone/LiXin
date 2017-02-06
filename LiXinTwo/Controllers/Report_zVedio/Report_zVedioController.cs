using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinInterface.Report_Vedio;
using LiXinModels.Report_zVedio;
using LiXinCommon;
using System.Data;
using LiXinInterface.Report_fVedio;
using LiXinModels;
using System.Diagnostics;
using LiXinModels.Report_Vedio;
using System.Text.RegularExpressions;

namespace LiXinControllers
{
    public partial class Report_zVedioController : BaseController
    {
        private IReport_Vedio vedioBL;
        private static IReport_OnLineTest iIReport_OnLineTestBL;
        private static List<CourseJoin> CourseJoinList;
        private static ChartModel joinpie = new ChartModel();//学员人数分布
        private static ChartModel joinzong = new ChartModel();//各月份学习情况分布
        private static ChartModel joinfeng = new ChartModel();//分所员工学习分布
        private static ChartModel joinmonth = new ChartModel();//总所各部门学习人数分布
        private static List<CourseJoin> sumList = new List<CourseJoin>();//总所滴人  为了图标的排序
        private static List<CourseJoin> fengList = new List<CourseJoin>();//总所滴人  为了图标的排序
        public Report_zVedioController(IReport_Vedio _vedioBL, IReport_OnLineTest _iIReport_OnLineTestBL)
        {
            vedioBL = _vedioBL;
            iIReport_OnLineTestBL = _iIReport_OnLineTestBL;
        }

        #region  呈现
        /// <summary>
        /// 总所视频课程
        /// </summary>
        /// <param name="type">0 汇总  1单个</param>
        /// <returns></returns>
        public ActionResult VedioLearnList(string cp, int tabIndex = 0)
        {
            ViewBag.zpageInex = 1;
            ViewBag.fpageIndex = 1;
            ViewBag.Attname = "请输入搜索内容";
            //汇总
            if (!string.IsNullOrEmpty(cp))
            {
                ViewBag.type = 0;
                if (tabIndex == 0)
                {
                    if (Session["zvedio_all"] != null)
                    {
                        string sess = Session["zvedio_all"].ToString();
                        string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                        ViewBag.zpageInex = att[0];
                        ViewBag.courseName = att[1] == "" ? "请输入搜索内容" : att[1];
                        ViewBag.start = att[2];
                        ViewBag.end = att[3];
                    }

                }
                else
                {
                    //单个
                    if (Session["zvedio_single"] != null)
                    {
                        // pageIndex + "㉿" + courseName + "㉿" + startyear + "㉿" + endyear + "㉿" + teacher + "㉿" + isMust + "㉿" + isTest + "㉿" + IsCPA 
                        //+ "㉿" + StartTime + "㉿" + EndTime + "㉿" + OpenObject;
                        string sess = Session["zvedio_single"].ToString();
                        string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                        ViewBag.fpageIndex = att[0];
                        ViewBag.fcourseName = att[1] == "" ? "请输入搜索内容" : att[1]; ;
                        ViewBag.startyear = att[2];
                        ViewBag.endyear = att[3];
                        ViewBag.teacher = att[4] == "" ? "请输入搜索内容" : att[4];
                        ViewBag.isMust = att[5];
                        ViewBag.isTest = att[6];
                        ViewBag.IsCPA = att[7];
                        ViewBag.StartTime = att[8];
                        ViewBag.EndTime = att[9];
                        ViewBag.OpenObject = att[10] == "" ? "请输入搜索内容" : att[10];
                    }

                }
            }
            else
            {
                ViewBag.type = 1;
            }
            return View();
        }

        /// <summary>
        /// 视频课程详情
        /// </summary>
        /// <returns></returns>
        public ActionResult VedioLearnDetail(int courseID, int type = 0)
        {
            ViewBag.courseID = courseID;
            ViewBag.type = type;
            return View();
        }

        /// <summary>
        /// 课程基本信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public ActionResult CourseBaseInfo(int courseID)
        {
            var model = vedioBL.GetVedioLearnSingle(" CourseID=" + courseID).FirstOrDefault();

            return View(model);
        }


        public ActionResult CourseJoinDetails(int courseID)
        {
            ViewBag.courseID = courseID;
            return View();
        }
        #endregion

        #region 汇总
        /// <summary>
        /// 视频课程汇总统计
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVedioList(string courseName = "", string start = "", string end = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " ReallyNumber desc")
        {
            try
            {
                if (Session["zvedio_all"] != null)
                {
                    Session.Remove("zvedio_all");
                }
                Session["zvedio_all"] = pageIndex + "㉿" + courseName + "㉿" + start + "㉿" + end;
                var where = " 1=1";
                if (!string.IsNullOrEmpty(start))
                {
                    where += " and YearPlan>=" + start;
                }
                if (!string.IsNullOrEmpty(end))
                {
                    where += " and YearPlan<=" + end;
                }
                if (!string.IsNullOrEmpty(courseName))
                {
                    where += " and CourseName like '%" + courseName.ReplaceSql() + "%'";
                }
                var list = vedioBL.GetVedioLearn(where);

                list = list.SortListByField(jsRenderSortField);
                var totalcount = list.Count;
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<CourseVedioLearn>(),
                    recordCount = 0,
                    ex = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导出视频课程汇总统计(总所)
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void OutVedioList(string courseName = "", string start = "", string end = "")
        {
            var where = " 1=1";
            if (!string.IsNullOrEmpty(start))
            {
                where += " and YearPlan>=" + start;
            }
            if (!string.IsNullOrEmpty(end))
            {
                where += " and YearPlan<=" + end;
            }
            if (!string.IsNullOrEmpty(courseName))
            {
                where += " and CourseName like '%" + courseName.ReplaceSql() + "%'";
            }
            var list = vedioBL.GetVedioLearn(where);
            list = list.SortListByField(" ReallyNumber desc");
            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("课程名称", typeof(string));
            outTable.Columns.Add("参与学习人数", typeof(string));
            outTable.Columns.Add("开放总人数", typeof(string));
            outTable.Columns.Add("学习人次", typeof(string));
            outTable.Columns.Add("参与率", typeof(string));
            outTable.Columns.Add("在线测试通过率", typeof(string));
            outTable.Columns.Add("课后评估综合平均分", typeof(string));
            outTable.Columns.Add("课程评估平均分", typeof(string));
            outTable.Columns.Add("教师评估平均分", typeof(string));
            var index = 1;
            foreach (var v in list)
            {
                outTable.Rows.Add(index, v.CourseName, v.LearnNumber, v.Number, v.ReallyNumber, v.JoinRate, v.PassRate, v.Survey_AllScore, v.Survey_CourseScore, v.Survey_TeacherScore);
                index++;
            }
            new Spreadsheet().Template("视频课程汇总统计(全所)", null, outTable, HttpContext, "视频课程汇总统计(全所)", "视频课程汇总统计(全所)");
        }
        #endregion

        #region 单个
        /// <summary>
        /// 视频课程单门课程统计
        /// </summary>
        /// <param name="startyear">开始年度</param>
        /// <param name="endyear">结束年度</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="teacher">讲师</param>
        /// <param name="isMust">是否必修</param>
        /// <param name="isTest">是否有在线测试</param>
        /// <param name="IsCPA">是否折算CPA</param>
        /// <param name="StartTime">开课时间 开始</param>
        /// <param name="EndTime">开课时间 结束</param>
        /// <param name="OpenObject">开放对象</param>
        /// <returns></returns>
        public JsonResult GetVedioLearnSingle(string startyear = "", string endyear = "", string courseName = "", string teacher = "", int isMust = -1,
          int isTest = -1, int IsCPA = -1, string StartTime = "", string EndTime = "", string OpenObject = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " StartTime desc")
        {
            try
            {
                if (Session["zvedio_single"] != null)
                {
                    Session.Remove("zvedio_single");
                }
                Session["zvedio_single"] = pageIndex + "㉿" + courseName + "㉿" + startyear + "㉿" + endyear + "㉿" + teacher + "㉿" + isMust
                    + "㉿" + isTest + "㉿" + IsCPA + "㉿" + StartTime + "㉿" + EndTime + "㉿" + OpenObject;

                string where = " 1=1";
                #region 条件
                if (!string.IsNullOrEmpty(startyear))
                {
                    where += " And YearPlan>=" + startyear;
                }
                if (!string.IsNullOrEmpty(endyear))
                {
                    where += " And YearPlan<=" + endyear;
                }
                if (!string.IsNullOrEmpty(courseName))
                {
                    where += " And CourseName like '%" + courseName.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(teacher))
                {
                    where += " And teacher like '%" + teacher.ReplaceSql() + "%'";
                }
                if (isMust != -1)
                {
                    where += " And isMust=" + isMust;
                }
                if (isTest != -1)
                {
                    where += " And isTest=" + isTest;
                }
                if (IsCPA != -1)
                {
                    where += " And IsCPA=" + IsCPA;
                }
                if (!string.IsNullOrEmpty(StartTime))
                {
                    where += " And  StartTime<=" + StartTime;
                }
                if (!string.IsNullOrEmpty(EndTime))
                {
                    where += " And EndTime>=" + EndTime;
                }
                if (!string.IsNullOrEmpty(OpenObject))
                {
                    where += " And openObject like '%" + OpenObject.ReplaceSql() + "%'";
                }
                #endregion
                int totalcount = 0;
                // var aa = cacheManager.Get<dynamic>("Vedio_Number");
                var list = vedioBL.GetVedioLearnSingle(where);
                totalcount = list.Count;
                list = list.SortListByField(jsRenderSortField);
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
                    dataList = new List<VedioLearnSingle>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///导出视频课程单门课程统计
        /// </summary>
        public void OutVedioSingleList(string startyear = "", string endyear = "", string courseName = "", string teacher = "", int isMust = -1,
          int isTest = -1, int IsCPA = -1, string StartTime = "", string EndTime = "", string OpenObject = "")
        {
            string where = " 1=1";
            #region 条件
            if (!string.IsNullOrEmpty(startyear))
            {
                where += " And YearPlan>=" + startyear;
            }
            if (!string.IsNullOrEmpty(endyear))
            {
                where += " And YearPlan<=" + endyear;
            }
            if (!string.IsNullOrEmpty(courseName))
            {
                where += " And CourseName like '%" + courseName.ReplaceSql() + "%'";
            }
            if (!string.IsNullOrEmpty(teacher))
            {
                where += " And teacher like '%" + teacher.ReplaceSql() + "%'";
            }
            if (isMust != -1)
            {
                where += " And isMust=" + isMust;
            }
            if (isTest != -1)
            {
                where += " And isTest=" + isTest;
            }
            if (IsCPA != -1)
            {
                where += " And IsCPA=" + IsCPA;
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                where += " And  StartTime>=" + StartTime;
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                where += " And EndTime<=" + EndTime;
            }
            if (!string.IsNullOrEmpty(OpenObject))
            {
                where += " And openObject like '" + OpenObject.ReplaceSql() + "'";
            }
            #endregion
            int totalcount = 0;
            // var aa = cacheManager.Get<dynamic>("Vedio_Number");
            var list = vedioBL.GetVedioLearnSingle(where);
            list = list.SortListByField(" StartTime desc");
            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("课程名称", typeof(string));
            outTable.Columns.Add("开放时间", typeof(string));
            outTable.Columns.Add("讲师", typeof(string));
            outTable.Columns.Add("学时", typeof(string));
            outTable.Columns.Add("选/必修", typeof(string));
            outTable.Columns.Add("开放对象", typeof(string));
            outTable.Columns.Add("是否折算CPA学时", typeof(string));
            outTable.Columns.Add("是否有在线测试", typeof(string));
            outTable.Columns.Add("已参加人数", typeof(string));
            outTable.Columns.Add("开放总人数", typeof(string));
            var index = 1;
            foreach (var v in list)
            {
                outTable.Rows.Add(index, v.CourseName, v.CourseTime, v.Teacher, v.CourseLength, v.IsMustStr, v.openObjectStr,
                    v.IsCPAStr, v.IsTestStr,
                    v.LearnNumber, v.sumNumber);
                index++;
            }
            new Spreadsheet().Template("视频课程单门课程统计(总所)", null, outTable, HttpContext, "视频课程单门课程统计(总所)", "视频课程单门课程统计(总所)");
        }
        #endregion

        #region 参与情况
        /// <summary>
        /// 参与情况
        /// </summary>
        /// <param name="DeptName"></param>
        /// <param name="type">0 分所  1总所</param>
        /// <returns></returns>
        public JsonResult GetCourseJoinList(int courseID, string StartTime, string EndTime, string DeptName = "", string jsRenderSortField = " JoinRateDouble desc", int type = 0)
        {
            try
            {
                string where = " 1=1";
                string querytime = " 1=1";
                if (!string.IsNullOrEmpty(DeptName))
                {
                    where += "  and DeptName like '%" + DeptName.ReplaceSql() + "%'";
                }

                if (!string.IsNullOrEmpty(StartTime))
                {
                    querytime += " and  ccs.LastUpdateTime>='" + StartTime + "'";
                }
                if (!string.IsNullOrEmpty(EndTime))
                {
                    querytime += " and  ccs.LastUpdateTime<='" + EndTime + "'";
                }

                if (type == 0)
                {
                    CourseJoinList = vedioBL.GetCourseJoinList(courseID, where, querytime: querytime);
                }
                else
                {
                    CourseJoinList = vedioBL.GetCourseJoinList(courseID, where, 1, string.Join(",", GetAllSubLeardDepartments()), querytime);
                }


                //全所合计
                var all = GetObjectForVedio(CourseJoinList, 0);

                //总所列表
                sumList = CourseJoinList.Where(p => p.IsMain == 1).ToList().SortListByField(jsRenderSortField);

                //分所列表
                fengList = CourseJoinList.Where(p => p.IsMain == 0).ToList().SortListByField(jsRenderSortField);

                //总所合计
                var sum = GetObjectForVedio(sumList, 1);

                //分所合计
                var feng = GetObjectForVedio(fengList, 2);

                return Json(new
                {
                    result = 1,
                    all = all,
                    sum = sum,
                    feng = feng,
                    sumList = sumList,
                    fengList = fengList
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 取出参与情况的集合 全部 总所 分所
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        public dynamic GetObjectForVedio(List<CourseJoin> List, int type = 0)
        {
            var title = "";
            switch (type)
            {
                case 0:
                    title = "全所合计({0})个";
                    break;
                case 1:
                    title = "总所合计({0})个";
                    break;
                case 2:
                    title = "分所合计({0})个";
                    break;
            }
            if (List.Count > 0)
            {

                var allJoinNumber = List.Where(p => p.JoinNumber > 0).Sum(p => p.JoinNumber);
                var allSumNumber = List.Where(p => p.JoinNumber > 0).Sum(p => p.SumNumber);
                var allSumLookTime = List.Where(p => p.JoinNumber > 0).Sum(p => p.SumLookTime);
                var allSum = new
                {
                    allNumer = string.Format(title, List.Select(p => p.DeptID).Distinct().Count()),
                    allJoinNumber = allJoinNumber,//已参加
                    allSumNumber = allSumNumber,//应参加
                    //参与率
                    allJoinRate = allSumNumber == 0 ? "0.00%" :
                    Math.Round(Convert.ToDouble(allJoinNumber) / Convert.ToDouble(allSumNumber), 4, MidpointRounding.AwayFromZero).ToString("P"),
                    //人次
                    allLearnNumber = List.Where(p => p.JoinNumber > 0).Sum(p => p.LearnNumber),
                    //累计观看时间
                    allSumLookTime = allSumLookTime.ToString(),
                    //平均观看时间
                    allAvgLookTime = ReportCommon.CalculateMedianDouble(List.Select(p => p.AvgLookTime).ToList()).ToString(),
                    //人均观看次数
                    allLookNumbr = ReportCommon.CalculateMedianDouble(List.Select(p => p.LookNumbr).ToList()).ToString(),
                    type = type
                };
                return allSum;
            }
            else
            {

                var allSum = new
                    {
                        allNumer = "全所合计0个",
                        allJoinNumber = 0,
                        allSumNumber = 0,
                        allJoinRate = "0.00%",
                        allLearnNumber = 0,
                        allSumLookTime = "N/A",
                        allAvgLookTime = "N/A",
                        allLookNumbr = "N/A"
                    };

                return allSum;
            }
        }

        /// <summary>
        /// 参与情况 图标
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public JsonResult CourseJoinChart(int courseID, int type = 0)
        {
            var newList = CourseJoinList;


            var pieSer = new List<pieSeries>();
            var zongSer = new List<Series>();
            var fengSer = new List<pieSeries>();
            var monthSer = new List<Series>();
            // var fengList = newList.Where(p => p.IsMain == 0);
            // var zongList = newList.Where(p => p.IsMain == 1);
            #region 学员人数分布
            pieSer.Add(new pieSeries { name = "总所", y = sumList.Sum(p => p.JoinNumber) });
            pieSer.Add(new pieSeries { name = "分所", y = fengList.Sum(p => p.JoinNumber) });
            joinpie.DivID = "showpie";
            joinpie.title = "学员人数分布";
            joinpie.pieseries = pieSer;
            joinpie.charttype = Charttype.Pie;
            joinpie.reportFormat = 1;
            #endregion

            #region 分所员工学习分布
            foreach (var item in fengList)
            {

                fengSer.Add(new pieSeries { name = item.DeptPath, y = item.JoinNumber });
            }
            joinfeng.DivID = "showfeng";
            joinfeng.title = "分所员工学习分布";
            joinfeng.pieseries = fengSer;
            joinfeng.charttype = Charttype.Pie;
            joinfeng.reportFormat = 1;
            #endregion

            #region 各月份学习情况分布
            //var monthList = new List<Report_OnLineTest>();
            //if (type == 0)
            //{
            //    monthList = iIReport_OnLineTestBL.GetMonthByCourseId(courseID).OrderByDescending(p => p.monthstr).ToList();
            //}
            //else
            //{
            //    monthList = iIReport_OnLineTestBL.GetMonthDeptByCourseId(courseID, string.Join(",", GetAllSubDepartments().Select(p => p.DepartmentId))).OrderByDescending(p => p.monthstr).ToList();
            //}

            //var zongsuo_month = monthList.Where(p => p.DeptName.Contains("上海")).ToList();
            //var fensuo_month = monthList.Where(p => !p.DeptName.Contains("上海")).ToList();

            //var xAxis = new List<String>();
            //var passSeries = new Series
            //{
            //    name = "全所人次",
            //    data = new List<double>()
            //};

            //var zongsuopassSeries = new Series
            //{
            //    name = "总所人次",
            //    data = new List<double>()
            //};

            //var fenpassSeries = new Series
            //{
            //    name = "分所人次",
            //    data = new List<double>()
            //};

            //var ii = monthList.Select(p => p.monthstr).Distinct().OrderBy(p => p).ToList();

            //foreach (var item in ii)
            //{

            //    passSeries.data.Add(monthList.Where(p => p.monthstr == item).Sum(p=>p.LearnTimes));

            //    zongsuopassSeries.data.Add(zongsuo_month.Where(p => p.monthstr == item).Sum(p => p.LearnTimes));

            //    fenpassSeries.data.Add(fensuo_month.Where(p => p.monthstr == item).Sum(p => p.LearnTimes));

            //    xAxis.Add(item.ToString()+"月");
            //}
            //monthSer.Add(passSeries);
            //monthSer.Add(zongsuopassSeries);
            //monthSer.Add(fenpassSeries);

            //joinmonth.DivID = "showmonth";
            ////month.xAxis = monthnameList;
            //joinmonth.xAxis = xAxis;

            //joinmonth.charttype = Charttype.Line;
            //joinmonth.title = "各月份学习情况分布图";
            //joinmonth.series = monthSer;
            #endregion

            #region 总所各部门学习人数分布
            var allxAxis = new List<String>();
            var allcolumn = new List<Double>();
            foreach (var item in sumList)
            {

                allcolumn.Add(item.JoinNumber);
                allxAxis.Add(item.DeptPath);
            }
            zongSer.Add(new Series
            {
                data = allcolumn,
                name = "学习人数"
            });
            joinzong.DivID = "showall";
            joinzong.title = "总所各部门学习人数分布";
            joinzong.series = zongSer;
            joinzong.xAxis = allxAxis;
            joinzong.charttype = Charttype.Column;
            joinzong.reportFormat = 1;
            #endregion

            return Json(new
           {
               pie = joinpie,
               feng = joinfeng,
               // month = joinmonth,
               zong = joinzong
           }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出参与情况
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="type">0 总所 1分所</param>
        public void OutCourseJoinList(int courseID, string StartTime, string EndTime, string where = " 1=1", int type = 0)
        {

            var list = CourseJoinList;


            if (!string.IsNullOrEmpty(StartTime))
            {
                where += " and  t.LastUpdateTime>='" + StartTime + "'";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                where += " and  t.LastUpdateTime<='" + EndTime + "'";
            }

            //全所合计
            var all = GetObjectForVedio(list, 0);

            //总所列表
            //var sumList = list.Where(p => p.IsMain == 1).ToList();

            //分所列表
            // var fengList = list.Where(p => p.IsMain == 0).ToList();

            //总所合计
            var sum = GetObjectForVedio(sumList, 1);

            //分所合计
            var feng = GetObjectForVedio(fengList, 2);

            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("对象名称", typeof(string));
            outTable.Columns.Add("课程总参与率", typeof(string));
            outTable.Columns.Add("已参加人数", typeof(string));
            outTable.Columns.Add("应参加人数", typeof(string));
            outTable.Columns.Add("学习人次", typeof(string));
            outTable.Columns.Add("累计观看时间（分钟）", typeof(string));
            outTable.Columns.Add("平均观看时间（分钟）", typeof(string));
            outTable.Columns.Add("人均观看次数", typeof(string));
            var index = 1;
            outTable.Rows.Add("", all.allNumer, all.allJoinRate, all.allJoinNumber, all.allSumNumber, all.allLearnNumber, all.allSumLookTime, all.allAvgLookTime, all.allLookNumbr);
            if (sumList.Count > 0)
            {
                outTable.Rows.Add("", sum.allNumer, sum.allJoinRate, sum.allJoinNumber, sum.allSumNumber, sum.allLearnNumber, sum.allSumLookTime, sum.allAvgLookTime, sum.allLookNumbr);
                foreach (var v in sumList)
                {
                    outTable.Rows.Add(index, v.DeptName, v.JoinRate, v.JoinNumber, v.SumNumber, v.LearnNumber, v.SumLookTime, v.AvgLookTime, v.LookNumbr);
                    index++;
                }
            }
            if (fengList.Count > 0)
            {
                outTable.Rows.Add("", feng.allNumer, feng.allJoinRate, feng.allJoinNumber, feng.allSumNumber, feng.allLearnNumber, feng.allSumLookTime, feng.allAvgLookTime, feng.allLookNumbr);
                foreach (var v in fengList)
                {
                    outTable.Rows.Add(index, v.DeptName, v.JoinRate, v.JoinNumber, v.SumNumber, v.LearnNumber, v.SumLookTime, v.AvgLookTime, v.LookNumbr);
                    index++;
                }
            }

            var Listcolumn = new List<ChartModel>();

            var value = type == 0 ? "全所" : "分所";
            SheetModel model = new SheetModel();

            model.SheetName = "视频课程参与情况" + value;
            model.space = 2;

            var datalist = new List<DataModel>();

            var data = new DataModel();
            data.datatype = Datatype.DataTable;
            data.Dataseries = GetCourseTable(courseID, type);
            datalist.Add(data);

            var data1 = new DataModel();
            data1.datatype = Datatype.DataTable;
            data1.Dataseries = outTable;
            datalist.Add(data1);


            if (type == 0)
            {
                if (fengList.Count > 0 || sumList.Count > 0)
                {

                    datalist.Add(Spreadsheet.ChangChartModelToDataModel(joinpie));
                }

                if (fengList.Count > 0)
                {
                    datalist.Add(Spreadsheet.ChangChartModelToDataModel(joinfeng));

                }
                //if (fengList.Count > 0 || sumList.Count > 0)
                //{
                //    Listcolumn.Add(joinmonth);
                //}
                if (sumList.Count > 0)
                {
                    datalist.Add(Spreadsheet.ChangChartModelToDataModel(joinzong));
                }
            }
            model.DataModels = datalist;

            var sheetmodelList = new List<SheetModel>();
            sheetmodelList.Add(model);
            new Spreadsheet().ExportExcel(sheetmodelList, HttpContext, "视频课程参与情况" + value);
            //
            ////基础课程信息
            //// DataRow dr = new DataRow();
            //DataTable DataTable1 = new DataTable(); 
            //var listdt = new List<DataTable>();
            //listdt.Add(outTable);
            //new Spreadsheet().ExportChart(Listcolumn, listdt, HttpContext, "视频课程参与情况" + value, "视频课程参与情况"+value);


        }
        #endregion

        #region 公共
        public DataTable GetCourseTable(int courseID, int type)
        {
            var model = vedioBL.GetVedioLearnSingle(" CourseID=" + courseID).FirstOrDefault();
            DataTable dt = new DataTable();
            dt.Columns.Add("课程名称", typeof(string));
            dt.Columns.Add("开放时间", typeof(string));
            if (type == 0)
            {
                dt.Columns.Add("已参加人数", typeof(string));
                dt.Columns.Add("开放总人数", typeof(string));
            }
            dt.Columns.Add("学时", typeof(string));
            dt.Columns.Add("讲师", typeof(string));
            dt.Columns.Add("选/必修", typeof(string));
            dt.Columns.Add("开放对象", typeof(string));
            dt.Columns.Add("是否折算CPA学时", typeof(string));
            dt.Columns.Add("是否有在线测试", typeof(string));
            if (type == 0)
            {
                dt.Rows.Add(model.CourseName, model.CourseTime, model.LearnNumber, model.sumNumber, model.CourseLength, model.Teacher, model.IsMustStr, model.openObjectStr, model.IsCPAStr, model.IsTestStr);
            }
            else
            {
                dt.Rows.Add(model.CourseName, model.CourseTime, model.CourseLength, model.Teacher, model.IsMustStr, model.openObjectStr, model.IsCPAStr, model.IsTestStr);
            }
            return dt;

        }
        #endregion



    }
}
