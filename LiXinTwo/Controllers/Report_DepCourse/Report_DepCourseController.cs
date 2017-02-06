using LiXinInterface.Report_Together;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinModels;
using LiXinInterface.DepCourseManage;
using LiXinInterface;
using System.Reflection;
using LiXinModels.DepCourseManage;
using System.Text.RegularExpressions;

namespace LiXinControllers
{
    public class Report_DepCourseController : BaseController
    {
        private static ChartModel joinpie = new ChartModel();//各部门培训进度
        private static ChartModel joinfeng = new ChartModel();//关于计划开设课程学时数分布图
        private static ChartModel joinzong = new ChartModel();//计划开设课程学时数分布图
        private static ChartModel joinshiji = new ChartModel();//实际开课

        protected ISys_TrainGrade sys_TrainBL;

        private IReport_DepCourse iReport_DepCourse;
        private IDep_Course iDep_Course;
        private static List<Report_DepCourse> ListDepCourse = new List<Report_DepCourse>();
        public Report_DepCourseController(IReport_DepCourse _iReport_DepCourse, IDep_Course _iDep_Course, ISys_TrainGrade _sys_TrainBL)
        {
            iReport_DepCourse = _iReport_DepCourse;
            iDep_Course = _iDep_Course;
            sys_TrainBL = _sys_TrainBL;
        }

        #region 呈现
        public ActionResult Report_Complete(int cp = 0)
        {
            //Session["Report_Complete"] = deptid + "㉿"  + PlanType + "㉿" + iscomplete + "㉿" + year;
            ViewBag.deptid = "";
            ViewBag.year = DateTime.Now;
            ViewBag.cp = cp;
            if (cp == 1)
            {
                if (Session["Report_Complete"] != null)
                {
                    string sess = Session["Report_Complete"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.deptid = att[0];
                    ViewBag.PlanType = att[1];
                    ViewBag.iscomplete = att[2];
                    ViewBag.year = att[3];
                }
            }
            return View();
        }

        public ActionResult SelectDept(int year, int ReportType)
        {
            ViewBag.year = year;
            ViewBag.ReportType = ReportType;
            return View();
        }

        public ActionResult Report_CompleteDetail(int cp = 0, string deptid = "", int year = -1, int show = 0)
        {
            ViewBag.deptid = deptid;
            ViewBag.pageInex = 1;
            ViewBag.year = year == -1 ? DateTime.Now.Year : year;
            ViewBag.show = show;
            ///为1 返回记住
            if (cp == 1)
            {
                if (Session["Report_CompleteDetail"] != null)
                {
                    string sess = Session["Report_CompleteDetail"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.deptid = att[0];
                    ViewBag.realname = att[1] == "" ? "" : att[1];
                    ViewBag.cpa = att[2];
                    ViewBag.year = att[3];
                    ViewBag.pageIndex = att[4];
                }
            }
            return View();
        }

        public ActionResult Report_DepCourseALL(int type = 0, int deptid = 0)
        {
            ViewBag.type = type;
            ViewBag.deptid = deptid;
            return View();
        }

        public ActionResult fReport_Complete(int cp = 0)
        {
            ViewBag.deptid = "";
            ViewBag.year = DateTime.Now;
            ViewBag.cp = cp;
            if (cp == 1)
            {
                if (Session["fReport_Complete"] != null)
                {
                    string sess = Session["fReport_Complete"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.deptid = att[0];
                    ViewBag.PlanType = att[1];
                    ViewBag.iscomplete = att[2];
                    ViewBag.year = att[3];
                }
            }
            return View();
        }

        public ActionResult fReport_CompleteDetail(int cp = 0, string deptid = "", int year = -1, int show = 0)
        {
            ViewBag.deptid = deptid;
            ViewBag.pageInex = 1;
            ViewBag.year = year == -1 ? DateTime.Now.Year : year;
            ViewBag.show = show;
            ///为1 返回记住
            if (cp == 1)
            {
                if (Session["Report_fCompleteDetail"] != null)
                {
                    string sess = Session["Report_fCompleteDetail"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.deptid = att[0];
                    ViewBag.realname = att[1] == "" ? "" : att[1];
                    ViewBag.cpa = att[2];
                    ViewBag.year = att[3];
                    ViewBag.pageIndex = att[4];
                }
            }
            return View();
        }

        public ActionResult fReport_DepCourseALL(int type = 0)
        {
            ViewBag.type = type;
            return View();
        }
        /// <summary>
        /// 师资情况
        /// </summary>
        /// <returns></returns>
        public ActionResult Report_Teachers(int cp = 0)
        {
            //培训级别
            ViewBag.trainGrade = sys_TrainBL.GetAllTrainGrade();
            ViewBag.pageIndex = 1;
            ViewBag.year = DateTime.Now;
            ViewBag.cp = cp;
            if (cp == 1)
            {
                if (Session["Report_Teachers"] != null)
                {
                    string sess = Session["Report_Teachers"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.realname = att[0];
                    ViewBag.deptid = att[1];
                    ViewBag.OpenLevel = att[2];
                    ViewBag.cpa = att[3];
                    ViewBag.PayGrade = att[4];
                    ViewBag.IsTeacher = att[5];
                    ViewBag.year = att[6];
                    ViewBag.pageIndex = att[7];
                }
            }
            return View();
        }

        /// <summary>
        /// 选择薪酬级别
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectPayGrade()
        {
            return View();
        }
        #endregion

        #region 完成情况
        /// <summary>
        /// 完成后情况
        /// </summary>
        /// <param name="deptname"></param>
        /// <param name="PlanType"></param>
        /// <param name="iscomplete"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public JsonResult GetReport_DepCourseList(int deptype, string deptid, string deptname, int PlanType, string iscomplete, int year = 2013, string jsRenderSortField = " IsCompleteStr asc")
        {
            //部门自学最大学时
            var DeptMaxScoreConfig = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 14).ConfigValue;
            var deptMaxScore = DeptMaxScoreConfig.Split(';')[2].Split(',')[0].StringToInt32();

            if (Session["Report_Complete"] != null)
            {
                Session.Remove("Report_Complete");
            }
            Session["Report_Complete"] = deptid + "㉿" + PlanType + "㉿" + iscomplete + "㉿" + year;

            if (Session["fReport_Complete"] != null)
            {
                Session.Remove("fReport_Complete");
            }
            Session["fReport_Complete"] = deptid + "㉿" + PlanType + "㉿" + iscomplete + "㉿" + year;

            #region 查询参数
            var config = AllSystemConfigs.Find(p => p.ConfigType == 56).ConfigValue;

            string sql = " where 1=1 ";
            string depwhere = " 1=1";
            if (!string.IsNullOrEmpty(deptid))
            {
                sql += "  and View_CheckUser.deptid in (" + deptid + ")";
                depwhere += " and  syu.DeptId in (" + deptid + ")";
            }

            if (deptype == 1)
            {
                string a = string.Join(",", GetAllSubLeardDepartments());
                sql += " and ( View_CheckUser.DeptId in (" + a + "))";
            }

            int zhi = Convert.ToInt32(AllSystemConfigs.Find(p => p.ConfigType == 14).ConfigValue.Split(';')[2].Split(',')[0]);

            ListDepCourse = iReport_DepCourse.GetReport_Complete(zhi, year, sql, depwhere, deptMaxScore);

            if (jsRenderSortField.Contains(","))
            {
                ListDepCourse = ListDepCourse.OrderBy(p => p.IsCompleteStr).ThenBy(p => p.IsYearPlan_length).ToList();
            }
            else
            {
                ListDepCourse = ListDepCourse.SortListByField(jsRenderSortField);
            }



            if (PlanType != 0 && PlanType != 5)
            {
                if (PlanType == 1)
                {
                    ListDepCourse = ListDepCourse.Where(p => p.PlanStr == "单体上报").ToList();
                }
                else if (PlanType == 2)
                {
                    ListDepCourse = ListDepCourse.Where(p => p.PlanStr == "联合上报").ToList();
                }
                else if (PlanType == 3)
                {
                    ListDepCourse = ListDepCourse.Where(p => p.PlanStr.Contains("被联合上报")).ToList();
                }
                else
                {
                    ListDepCourse = ListDepCourse.Where(p => p.PlanStr == "N/A").ToList();
                }
            }

            if (!string.IsNullOrEmpty(iscomplete) && iscomplete != "2")
            {
                if (iscomplete == "0")
                {
                    ListDepCourse = ListDepCourse.Where(p => p.IsCompleteStr == "是").ToList();
                }
                else
                {
                    ListDepCourse = ListDepCourse.Where(p => p.IsCompleteStr == "否").ToList();
                }
            }
            #endregion

            #region 拼接html
            StringBuilder sb = new StringBuilder();

            var zongsuo = ListDepCourse.Where(p => p.Dep_DeptName.Contains("上海")).ToList();

            if (ListDepCourse.Count == 0)
            {
                sb.Append("<tr class='odd'><td colspan='15'><div class='tc c38 line_h30'>暂无数据</div></td></tr>");
            }
            if (zongsuo.Count() != 0)
            {
                var sumAvg_Qq = ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.Avg_Qq).ToList(), 4).ToString("p");
                var Avg_Cq = ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.Avg_Cq).ToList(), 4).ToString("p");
                var Avg_Cd = ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.Avg_Cd).ToList(), 4).ToString("p");
                sb.Append("<tr >");
                sb.Append("<td></td>");
                sb.Append("<td>总所合计（" + zongsuo.Count() + "个）</td>");
                sb.Append("<td>--</td>");
                sb.Append("<td>--</td>");
                sb.Append("<td>--</td>");
                sb.Append("<td>" + zongsuo.Sum(p => p.Dep_Course_Commencement) + "（" + zongsuo.Sum(p => p.Dep_Course_Commencement_length).ToString("0.00") + "）</td>");
                sb.Append("<td  style='text-align:left'>" + zongsuo.Sum(p => p.IsYearPlanNei) + "(" + zongsuo.Sum(p => p.IsYearPlanNei_length).ToString("0.00") + ")/<br>" + zongsuo.Sum(p => p.IsYearPlanWai) + "(" + zongsuo.Sum(p => p.IsYearPlanWai_length).ToString("0.00") + ")</td>");
                sb.Append("<td  style='text-align:left'>" + zongsuo.Sum(p => p.IsSystemNei) + "(" + zongsuo.Sum(p => p.IsSystemNei_length).ToString("0.00") + ")/<br>" + zongsuo.Sum(p => p.IsSystemWai) + "(" + zongsuo.Sum(p => p.IsSystemWai_length).ToString("0.00") + ")</td>");
                sb.Append("<td>" + zongsuo.Sum(p => p.IsOpenSub) + "(" + zongsuo.Sum(p => p.IsOpenSub_length) + ")</td>");
                sb.Append("<td>" + zongsuo.Sum(p => p.UploadAttend_Num) + "</td>");
                sb.Append("<td>" + zongsuo.Sum(p => p.ApprovalPass) + "/" + zongsuo.Sum(p => p.ApprovalNoPass) + "</td>");
                sb.Append("<td>" + zongsuo.Sum(p => p.UploadRes_Num) + "</td>");
                sb.Append("<td>" + ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.Avg_GetLength).ToList(), 2) + "</td>");
                sb.Append("<td style='text-align:left'>" + Avg_Cq + "/<br>" + Avg_Cd + "/<br>" + sumAvg_Qq + "</td>");
                if (deptype == 1)
                {
                    sb.Append("<td><a class='buttonshow btnshow'  onclick='showHide(1,this)'><a></td>");
                }
                else
                {
                    sb.Append("<td><a class='buttonhide btnshow'  onclick='showHide(1,this)'><a></td>");
                }
                sb.Append("</tr>");
            }

            int zongsuonum = 1;
            foreach (var item in zongsuo)
            {
                var isYearStatus = item.PlanStr.Contains("被联合上报") ? 1 : 0;
                if (deptype == 1)
                {
                    sb.Append("<tr  class='report_1' >");
                }
                else
                {
                    sb.Append("<tr  class='report_1' style='display:none'>");
                }

                sb.Append("<td>" + zongsuonum + "</td>");
                sb.Append("<td style='text-align: left'><a onclick=\"ViewDepOpenCourse('" + item.DeptId + "'," + isYearStatus + ",0)\" style=\"cursor:pointer\">" + item.Dep_DeptName.Replace("审计事业部/上海总部/", "") + "</a></td>");
                sb.Append("<td>" + item.IsCompleteStr + "</td>");
                sb.Append("<td style='text-align:left'>" + item.PlanStr + "</td>");
                sb.Append("<td>" + item.PlanTime + "</td>");
                sb.Append("<td>" + item.Dep_Course_Commencement + "(" + item.Dep_Course_Commencement_length.ToString("0.00") + ")</td>");
                sb.Append("<td  style='text-align:left'>" + item.SumYearPlanNei + "(" + item.SumYearPlanNei_length.ToString("0.00") + ")/<br>" + item.SumYearPlanWai + "(" + item.SumYearPlanWai_length.ToString("0.00") + ")</td>");
                sb.Append("<td  style='text-align:left'>" + item.SumSystemNei + "(" + item.SumSystemNei_length.ToString("0.00") + ")/<br>" + item.SumSystemWai + "(" + item.SumSystemWai_length.ToString("0.00") + ")</td>");
                sb.Append("<td>" + item.IsOpenSub + "(" + item.IsOpenSub_length + ")</td>");
                sb.Append("<td>" + item.UploadAttend_Num + "</td>");
                sb.Append("<td>" + item.ApprovalPass + "/" + item.ApprovalNoPass + "</td>");
                sb.Append("<td>" + item.UploadRes_Num + "</td>");
                sb.Append("<td>" + item.Avg_GetLengthStr + "</td>");
                sb.Append("<td  style='text-align:left'>" + item.AvgStr + "</td>");
                sb.Append("<td><a onclick=\"ToCompletionDetails('" + item.DeptId + "')\"  class=\"icon iview\" title=\"查看详情\"></a></td>");
                sb.Append("</tr>");
                zongsuonum++;
            }
            //分所
            var fensuo = ListDepCourse.Where(p => p.Dep_DeptName.Contains("分所")).OrderBy(p => p.Dep_TwoDeptName).ToList();

            if (fensuo.Count() != 0)
            {
                var sumAvg_Qq = ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.Avg_Qq).ToList(), 4).ToString("p");
                var Avg_Cq = ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.Avg_Cq).ToList(), 4).ToString("p");
                var Avg_Cd = ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.Avg_Cd).ToList(), 4).ToString("p");
                sb.Append("<tr >");
                sb.Append("<td></td>");
                sb.Append("<td>分所合计（" + fensuo.Select(p => p.Dep_TwoDeptName).Distinct().Count() + "个）</td>");
                sb.Append("<td>--</td>");
                sb.Append("<td>--</td>");
                sb.Append("<td>--</td>");
                sb.Append("<td>" + fensuo.Sum(p => p.Dep_Course_Commencement) + "（" + fensuo.Sum(p => p.Dep_Course_Commencement_length).ToString("0.00") + "）</td>");
                sb.Append("<td  style='text-align:left'>" + fensuo.Sum(p => p.IsYearPlanNei) + "(" + fensuo.Sum(p => p.IsYearPlanNei_length).ToString("0.00") + ")/<br>" + fensuo.Sum(p => p.IsYearPlanWai) + "(" + fensuo.Sum(p => p.IsYearPlanWai_length).ToString("0.00") + ")</td>");
                sb.Append("<td  style='text-align:left'>" + fensuo.Sum(p => p.IsSystemNei) + "(" + fensuo.Sum(p => p.IsSystemNei_length).ToString("0.00") + ")/<br>" + fensuo.Sum(p => p.IsSystemWai) + "(" + fensuo.Sum(p => p.IsSystemWai_length).ToString("0.00") + ")</td>");
                sb.Append("<td>" + fensuo.Sum(p => p.IsOpenSub) + "(" + fensuo.Sum(p => p.IsOpenSub_length) + ")</td>");
                sb.Append("<td>" + fensuo.Sum(p => p.UploadAttend_Num) + "</td>");
                sb.Append("<td>" + fensuo.Sum(p => p.ApprovalPass) + "/" + fensuo.Sum(p => p.ApprovalNoPass) + "</td>");
                sb.Append("<td>" + fensuo.Sum(p => p.UploadRes_Num) + "</td>");
                sb.Append("<td>" + Math.Round(fensuo.Sum(p => p.Avg_GetLength) / Convert.ToDecimal(fensuo.Count()), 2, MidpointRounding.AwayFromZero).ToString("0.00") + "</td>");
                sb.Append("<td  style='text-align:left'>" + Avg_Cq + "/<br>" + Avg_Cd + "/<br>" + sumAvg_Qq + "</td>");

                //	buttonshow  buttonhide
                if (deptype == 1)
                {
                    sb.Append("<td><a class='buttonshow btnshow'  onclick='showHide(2,this)'><a></td>");
                }
                else
                {
                    sb.Append("<td><a class='buttonhide btnshow'  onclick='showHide(2,this)'><a></td>");
                }
                sb.Append("</tr>");
            }


            int fensuonum = 1;
            var number = 1;
            var listName = new List<string>();

            foreach (var item in fensuo)
            {
                var newfensuo = fensuo.Where(p => p.Dep_TwoDeptName == item.Dep_TwoDeptName).ToList();
                if (deptype == 1)
                {
                    sb.Append("<tr  class='report_0' >");
                }
                else
                {
                    sb.Append("<tr  class='report_0' style='display:none'>");
                }
                if (!listName.Contains(item.Dep_TwoDeptName))
                {
                    listName.Add(item.Dep_TwoDeptName);
                    number = 1;
                    var sumAvg_Qq = ReportCommon.CalculateMedianDouble(newfensuo.Select(p => p.Avg_Qq).ToList(), 4).ToString("p");
                    var Avg_Cq = ReportCommon.CalculateMedianDouble(newfensuo.Select(p => p.Avg_Cq).ToList(), 4).ToString("p");
                    var Avg_Cd = ReportCommon.CalculateMedianDouble(newfensuo.Select(p => p.Avg_Cd).ToList(), 4).ToString("p");
                    sb.Append("<td>" + fensuonum + "</td>");
                    sb.Append("<td style='text-align: left'>" + item.Dep_TwoDeptName + "</td>");
                    sb.Append("<td>--</td>");
                    sb.Append("<td>--</td>");
                    sb.Append("<td>--</td>");
                    sb.Append("<td>" + newfensuo.Sum(p => p.Dep_Course_Commencement) + "（" + newfensuo.Sum(p => p.Dep_Course_Commencement_length).ToString("0.00") + "）</td>");
                    sb.Append("<td  style='text-align:left'>" + newfensuo.Sum(p => p.IsYearPlanNei) + "(" + newfensuo.Sum(p => p.IsYearPlanNei_length).ToString("0.00") + ")/<br>" + newfensuo.Sum(p => p.IsYearPlanWai) + "(" + newfensuo.Sum(p => p.IsYearPlanWai_length).ToString("0.00") + ")</td>");
                    sb.Append("<td  style='text-align:left'>" + newfensuo.Sum(p => p.IsSystemNei) + "(" + newfensuo.Sum(p => p.IsSystemNei_length).ToString("0.00") + ")/<br>" + newfensuo.Sum(p => p.IsSystemWai) + "(" + newfensuo.Sum(p => p.IsSystemWai_length).ToString("0.00") + ")</td>");
                    sb.Append("<td>" + newfensuo.Sum(p => p.IsOpenSub) + "(" + newfensuo.Sum(p => p.IsOpenSub_length) + ")</td>");
                    sb.Append("<td>" + newfensuo.Sum(p => p.UploadAttend_Num) + "</td>");
                    sb.Append("<td>" + newfensuo.Sum(p => p.ApprovalPass) + "/" + newfensuo.Sum(p => p.ApprovalNoPass) + "</td>");
                    sb.Append("<td>" + newfensuo.Sum(p => p.UploadRes_Num) + "</td>");
                    sb.Append("<td>" + Math.Round(newfensuo.Sum(p => p.Avg_GetLength) / Convert.ToDecimal(newfensuo.Count()), 2, MidpointRounding.AwayFromZero).ToString("0.00") + "</td>");
                    sb.Append("<td  style='text-align:left'>" + Avg_Cq + "/<br>" + Avg_Cd + "/<br>" + sumAvg_Qq + "</td>");
                    sb.Append("<td><a class=\"buttonhide btnshow\"  onclick=\"showfenHide('" + item.Dep_TwoDeptName + "',this)\"><a></td>");
                    fensuonum++;
                }
                sb.Append("</tr>");

                if (jsRenderSortField.Contains(","))
                {
                    newfensuo = newfensuo.OrderBy(p => p.IsCompleteStr).ThenBy(p => p.IsYearPlan_length).ToList();
                }
                else
                {
                    newfensuo = newfensuo.SortListByField(jsRenderSortField);
                }
                var NameList = new List<string>();
                foreach (var model in newfensuo)
                {
                    if (!NameList.Contains(item.DeptName))
                    {
                        NameList.Add(item.DeptName);
                        var isYearStatus = item.PlanStr.Contains("被联合上报") ? 1 : 0;

                        sb.Append("<tr  class='report_" + item.Dep_TwoDeptName + "' style='display:none'>");
                        sb.Append("<td>" + number + "</td>");
                        sb.Append("<td><a onclick=\"ViewDepOpenCourse('" + item.DeptId + "'," + isYearStatus + ",1)\" style=\"cursor:pointer\">" + item.DeptName + "</a></td>");
                        sb.Append("<td>" + item.IsCompleteStr + "</td>");
                        sb.Append("<td  style='text-align:left'>" + item.PlanStr + "</td>");
                        sb.Append("<td>" + item.PlanTime + "</td>");
                        sb.Append("<td>" + item.Dep_Course_Commencement + "(" + item.Dep_Course_Commencement_length.ToString("0.00") + ")</td>");
                        sb.Append("<td  style='text-align:left'>" + item.SumYearPlanNei + "(" + item.SumYearPlanNei_length.ToString("0.00") + ")/<br>" + item.SumYearPlanWai + "(" + item.SumYearPlanWai_length.ToString("0.00") + ")</td>");
                        sb.Append("<td  style='text-align:left'>" + item.SumSystemNei + "(" + item.SumSystemNei_length.ToString("0.00") + ")/<br>" + item.SumSystemWai + "(" + item.SumSystemWai_length.ToString("0.00") + ")</td>"); sb.Append("<td>" + item.IsOpenSub + "(" + item.IsOpenSub_length + ")</td>");
                        sb.Append("<td>" + item.UploadAttend_Num + "</td>");
                        sb.Append("<td>" + item.ApprovalPass + "/" + item.ApprovalNoPass + "</td>");
                        sb.Append("<td>" + item.UploadRes_Num + "</td>");
                        sb.Append("<td>" + item.Avg_GetLengthStr + "</td>");
                        sb.Append("<td  style='text-align:left'>" + item.AvgStr + "</td>");
                        sb.Append("<td><a onclick=\"ToCompletionDetails('" + item.DeptId + "')\" class=\"icon iview\" title=\"查看详情\">详情</a></td>");
                        sb.Append("</tr>");
                        number++;
                    }
                }

            }

            #endregion

            #region 导出饼状图


            var arrconfig = config == "" ? ("16,17:20,21:30,31").Split(',') : config.Split(',');

            if (deptype == 0)
            {
                #region 总所各部门培训进度
                var pieSer = new List<pieSeries>();
                pieSer.Add(new pieSeries { name = "已完成", y = zongsuo.Where(p => p.IsComplete_length >= zhi).Count() });
                pieSer.Add(new pieSeries { name = "未开展", y = Convert.ToDouble(zongsuo.Where(p => p.IsComplete_length == 0).Count()) });
                pieSer.Add(new pieSeries { name = "开展中", y = Convert.ToDouble(zongsuo.Where(p => p.IsComplete_length > 0 && p.IsComplete_length < 16).Count()) });
                joinpie.DivID = "showpie";
                joinpie.title = "总所各部门培训进度";
                joinpie.pieseries = pieSer;
                joinpie.charttype = Charttype.Pie;
                joinpie.reportFormat = 1;
                #endregion


                var deptList = zongsuo.Select(p => p.DeptId);
                string deptIDWhere = " 1=1";

                deptIDWhere += " AND DeptId IN (" + (deptList.Any() ? string.Join(",", deptList) : "0") + ")";


                var list_shiji = iReport_DepCourse.GetDep_Course_DeptNumFor_ShiJi(year, deptIDWhere);
                var list_YearPlan = iReport_DepCourse.GetDep_Course_DeptNumFor_YearPlan(year, deptIDWhere);

                //总所实际开设课程学时
                var list_shiji_length = iReport_DepCourse.GetDep_Course_DeptNumFor_ShiJi_Length(year, deptIDWhere);
                //总所计划开设课程学时数分布图
                var list_YearPlan_length = iReport_DepCourse.GetDep_Course_DeptNumFor_YearPlan_Length(year, deptIDWhere);

                #region 总所计划开设课程学时数分布图
                var fengSer = new List<pieSeries>();
                foreach (var item in arrconfig)
                {
                    if (item.Split(':')[1] != "-1")
                    {
                        fengSer.Add(new pieSeries { name = item.Split(':')[0] + "〈=" + "X〈=" + item.Split(':')[1], y = Convert.ToDouble(list_YearPlan_length.Where(p => p.CourseLength >= Convert.ToDecimal(item.Split(':')[0]) && p.CourseLength <= Convert.ToDecimal(item.Split(':')[1])).Count()) });
                    }
                    else
                    {
                        fengSer.Add(new pieSeries { name = "X>=" + item.Split(':')[0], y = Convert.ToDouble(list_YearPlan_length.Where(p => p.CourseLength >= Convert.ToDecimal(item.Split(':')[0])).Count()) });
                    }
                }
                joinfeng.DivID = "showfeng";
                joinfeng.title = "总所计划开设课程学时数分布图";
                joinfeng.pieseries = fengSer;
                joinfeng.charttype = Charttype.Pie;
                joinfeng.reportFormat = 1;
                joinfeng.PerName = "学时：";
                joinfeng.PerY = "部门：";
                #endregion

                #region 总所实际开设课程学时数分布图
                var shijiSer = new List<pieSeries>();
                foreach (var item in arrconfig)
                {
                    if (item.Split(':')[1] != "-1")
                    {
                        shijiSer.Add(new pieSeries { name = item.Split(':')[0] + "〈=" + "X〈=" + item.Split(':')[1], y = Convert.ToDouble(list_shiji_length.Where(p => p.CourseLength >= Convert.ToDecimal(item.Split(':')[0]) && p.CourseLength <= Convert.ToDecimal(item.Split(':')[1])).Count()) });
                    }
                    else
                    {
                        shijiSer.Add(new pieSeries { name = "X>=" + item.Split(':')[0], y = Convert.ToDouble(list_shiji_length.Where(p => p.CourseLength >= Convert.ToDecimal(item.Split(':')[0])).Count()) });
                    }
                }
                joinshiji.DivID = "showshiji";
                joinshiji.title = "总所实际开设课程学时数分布图";
                joinshiji.pieseries = shijiSer;
                joinshiji.charttype = Charttype.Pie;
                joinshiji.reportFormat = 1;
                joinshiji.PerName = "学时：";
                joinshiji.PerY = "部门：";
                #endregion


                #region 总所各月份学习情况分布图
                //用 CourseFrom   0：年度计划；1：月度计划；2：课程管理  
                //年度的计划开课
                var passSeries = new Series
                {
                    name = "计划开课部门数",
                    data = new List<double>()
                };

                //开课
                var zongsuopassSeries = new Series
                {
                    name = "实际开课部门数",
                    data = new List<double>()
                };

                //var courselist = iDep_Course.GetDep_CourseList(year);

                var monthSer = new List<Series>();

                var xAxis = new List<String>();

                int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                for (int i = 0; i < arr.Length; i++)
                {

                    zongsuopassSeries.data.Add(list_shiji.Where(p => Convert.ToInt32(p.month) == arr[i]).Distinct(new PropertyComparer<Report_DepCourse>("DeptId")).Count());
                    passSeries.data.Add(list_YearPlan.Where(p => Convert.ToInt32(p.monthstr.Split('-')[1]) == arr[i]).Count());

                    xAxis.Add(arr[i].ToString() + "月");
                }
                monthSer.Add(passSeries);
                monthSer.Add(zongsuopassSeries);
                joinzong.DivID = "showmonth";
                joinzong.xAxis = xAxis;
                joinzong.charttype = Charttype.Line;
                joinzong.title = "总所各月份学习情况分布图";
                joinzong.series = monthSer;
                #endregion
            }
            #endregion

            return Json(new
            {
                list = sb.ToString(),
                pie = joinpie,
                feng = joinfeng,
                month = joinzong,
                shiji = joinshiji
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 完成情况报表
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptname"></param>
        /// <param name="PlanType"></param>
        /// <param name="iscomplete"></param>
        /// <param name="year"></param>
        public void GetReport_DepCourseListForReport(int deptype, string deptid, string deptname, int PlanType, string iscomplete, int year = 2013)
        {


            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("部门/分所", typeof(string));
            outTable.Columns.Add("是否已完成目标学时", typeof(string));
            outTable.Columns.Add("计划上报类型", typeof(string));
            outTable.Columns.Add("计划上报时间", typeof(string));
            outTable.Columns.Add("计划开设课程数（学时）", typeof(string));
            outTable.Columns.Add("实际开设计划内课程数（学时）", typeof(string));
            outTable.Columns.Add("实际开设计划外课程数（学时）", typeof(string));
            outTable.Columns.Add("框架内课程数（学时）", typeof(string));
            outTable.Columns.Add("框架外课程数（学时）", typeof(string));
            outTable.Columns.Add("开放至全所课程数（学时）", typeof(string));
            outTable.Columns.Add("已上传考勤课程数", typeof(string));
            outTable.Columns.Add("审批通过", typeof(string));
            outTable.Columns.Add("退回课程数", typeof(string));
            outTable.Columns.Add("讲义上传课程数", typeof(string));
            outTable.Columns.Add("人均获取学时", typeof(string));
            outTable.Columns.Add("平均出勤率", typeof(string));
            outTable.Columns.Add("迟到率", typeof(string));
            outTable.Columns.Add("缺勤率", typeof(string));

            var zongsuo = ListDepCourse.Where(p => p.Dep_DeptName.Contains("上海")).ToList();

            if (zongsuo.Count() != 0)
            {
                var sumAvg_Qq = ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.Avg_Qq).ToList(), 4).ToString("p");
                var Avg_Cq = ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.Avg_Cq).ToList(), 4).ToString("p");
                var Avg_Cd = ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.Avg_Cd).ToList(), 4).ToString("p");
                outTable.Rows.Add("", "总所合计（" + zongsuo.Count() + "个）", "--", "--", "--",
                     zongsuo.Sum(p => p.Dep_Course_Commencement) + "（" + zongsuo.Sum(p => p.Dep_Course_Commencement_length).ToString("0.00") + "）",
                     zongsuo.Sum(p => p.IsYearPlanNei) + "(" + zongsuo.Sum(p => p.IsYearPlanNei_length).ToString("0.00") + ")", zongsuo.Sum(p => p.IsYearPlanWai) + "(" + zongsuo.Sum(p => p.IsYearPlanWai_length).ToString("0.00") + ")",
                     zongsuo.Sum(p => p.IsSystemNei) + "(" + zongsuo.Sum(p => p.IsSystemNei_length).ToString("0.00") + ")", zongsuo.Sum(p => p.IsSystemWai) + "(" + zongsuo.Sum(p => p.IsSystemWai_length).ToString("0.00") + ")",
                     zongsuo.Sum(p => p.IsOpenSub) + "(" + zongsuo.Sum(p => p.IsOpenSub_length) + ")",
                      zongsuo.Sum(p => p.UploadAttend_Num),
                      zongsuo.Sum(p => p.ApprovalPass), zongsuo.Sum(p => p.ApprovalNoPass),
                     zongsuo.Sum(p => p.UploadRes_Num),
                     ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.Avg_GetLength).ToList(), 2), Avg_Cq, Avg_Cd, sumAvg_Qq);
            }
            int zongsuonum = 1;
            //sb.Append("<td  style='text-align:left'>" + item.SumYearPlanNei + "(" + item.SumYearPlanNei_length + ")/<br>" + item.SumYearPlanWai + "(" + item.SumYearPlanWai_length + ")</td>");
            //sb.Append("<td  style='text-align:left'>" + item.SumSystemNei + "(" + item.SumSystemNei_length + ")/<br>" + item.SumSystemWai + "(" + item.SumSystemWai_length + ")</td>");
            foreach (var item in zongsuo)
            {
                outTable.Rows.Add(zongsuonum, item.Dep_DeptName.Replace("审计事业部/上海总部/", ""), item.IsCompleteStr, item.PlanStr, item.PlanTime,
                item.Dep_Course_Commencement + "(" + item.Dep_Course_Commencement_length.ToString("0.00") + ")",
              item.SumYearPlanNei + "(" + item.SumYearPlanNei_length.ToString("0.00") + ")", item.SumYearPlanWai + "(" + item.SumYearPlanWai_length.ToString("0.00") + ")",
                item.SumSystemNei + "(" + item.SumSystemNei_length.ToString("0.00") + ")", item.SumSystemWai + "(" + item.SumSystemWai_length.ToString("0.00") + ")",
                item.IsOpenSub + "(" + item.IsOpenSub_length + ")",
                 item.UploadAttend_Num,
                 item.ApprovalPass, item.ApprovalNoPass,
                item.UploadRes_Num,
                item.Avg_GetLength, item.Avg_Cq.ToString("p"), item.Avg_Cd.ToString("p"), item.Avg_Qq.ToString("p"));
                zongsuonum++;
            }

            var fensuo = ListDepCourse.Where(p => p.Dep_DeptName.Contains("分所")).ToList();

            if (fensuo.Count() != 0)
            {

                var sumAvg_Qq = ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.Avg_Qq).ToList(), 4).ToString("p");
                var Avg_Cq = ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.Avg_Cq).ToList(), 4).ToString("p");
                var Avg_Cd = ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.Avg_Cd).ToList(), 4).ToString("p");
                outTable.Rows.Add("", "分所合计（" + fensuo.Count() + "个）", "--", "--", "--",
                     fensuo.Sum(p => p.Dep_Course_Commencement) + "（" + fensuo.Sum(p => p.Dep_Course_Commencement_length).ToString("0.00") + "）",
                     fensuo.Sum(p => p.IsYearPlanNei) + "(" + fensuo.Sum(p => p.IsYearPlanNei_length).ToString("0.00") + ")", fensuo.Sum(p => p.IsYearPlanWai) + "(" + fensuo.Sum(p => p.IsYearPlanWai_length).ToString("0.00") + ")",
                     fensuo.Sum(p => p.IsSystemNei) + "(" + fensuo.Sum(p => p.IsSystemNei_length).ToString("0.00") + ")", fensuo.Sum(p => p.IsSystemWai) + "(" + fensuo.Sum(p => p.IsSystemWai_length).ToString("0.00") + ")",
                     fensuo.Sum(p => p.IsOpenSub) + "(" + fensuo.Sum(p => p.IsOpenSub_length) + ")",
                      fensuo.Sum(p => p.UploadAttend_Num),
                      fensuo.Sum(p => p.ApprovalPass), fensuo.Sum(p => p.ApprovalNoPass),
                     fensuo.Sum(p => p.UploadRes_Num),
                     ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.Avg_GetLength).ToList(), 2), Avg_Cq, Avg_Cd, sumAvg_Qq);
            }

            int fensuonum = 1;
            foreach (var item in fensuo)
            {
                outTable.Rows.Add(zongsuonum, item.Dep_DeptName, item.IsCompleteStr, item.PlanStr, item.PlanTime,
                item.Dep_Course_Commencement + "(" + item.Dep_Course_Commencement_length.ToString("0.00") + ")",
                item.SumYearPlanNei + "(" + item.SumYearPlanNei_length.ToString("0.00") + ")", item.SumYearPlanWai + "(" + item.SumYearPlanWai_length.ToString("0.00") + ")",
                item.SumSystemNei + "(" + item.SumSystemNei_length.ToString("0.00") + ")", item.SumSystemWai + "(" + item.SumSystemWai_length.ToString("0.00") + ")", item.IsOpenSub + "(" + item.IsOpenSub_length + ")",
                 item.UploadAttend_Num,
                 item.ApprovalPass, item.ApprovalNoPass,
                item.UploadRes_Num,
                item.Avg_GetLength, item.Avg_Cq.ToString("p"), item.Avg_Cd.ToString("p"), item.Avg_Qq.ToString("p"));
                fensuonum++;
            }


            if (deptype == 0)
            {
                SheetModel model = new SheetModel();

                model.SheetName = "各部门或分所自学完成情况汇总表";// +value;
                model.space = 2;
                var datalist = new List<DataModel>();

                var data1 = new DataModel();
                data1.datatype = Datatype.DataTable;
                data1.Dataseries = outTable;

                datalist.Add(data1);

                datalist.Add(Spreadsheet.ChangChartModelToDataModel(joinpie));
                datalist.Add(Spreadsheet.ChangChartModelToDataModel(joinfeng));
                datalist.Add(Spreadsheet.ChangChartModelToDataModel(joinshiji));
                datalist.Add(Spreadsheet.ChangChartModelToDataModel(joinzong));
                model.DataModels = datalist;

                var sheetmodelList = new List<SheetModel>();
                sheetmodelList.Add(model);
                new Spreadsheet().ExportExcel(sheetmodelList, HttpContext, year + "年各部门或分所自学完成情况汇总表");
                //new Spreadsheet().Template("分所自学培训完成情况", null, outTable, HttpContext, "分所自学培训完成情况", "分所自学培训完成情况");
            }
            else
            {
                new Spreadsheet().Template(year + "年各部门或分所自学完成情况汇总表", null, outTable, HttpContext, year + "年各部门或分所自学完成情况汇总表", year + "年各部门或分所自学完成情况汇总表");
            }

        }

        /// <summary>
        /// 完成情况报表
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult CourseJoinChart(int deptype, string deptid, string deptname, int PlanType, string iscomplete, int year = 2013)
        {
            #region 查询条件
            string sql = " where 1=1 ";

            if (!string.IsNullOrEmpty(deptid))
            {
                sql += "  and View_CheckUser.deptid in (" + deptid + ")";
            }


            if (deptype == 1)
            {
                string a = string.Join(",", GetAllSubLeardDepartments());
                sql += " and ( View_CheckUser.DeptId in (" + a + "))";
            }

            int zhi = Convert.ToInt32(AllSystemConfigs.Find(p => p.ConfigType == 14).ConfigValue.Split(';')[2].Split(',')[0]);

            var list = iReport_DepCourse.GetReport_Complete(zhi, year, sql);

            if (PlanType != 0 && PlanType != 3)
            {
                if (PlanType == 1)
                {
                    list = list.Where(p => p.PlanStr == "单体上报").ToList();
                }
                else
                {
                    list = list.Where(p => p.PlanStr == "联合上报").ToList();
                }
            }

            if (!string.IsNullOrEmpty(iscomplete))
            {
                if (iscomplete == "0")
                {
                    list = list.Where(p => p.IsCompleteStr == "是").ToList();
                }
                else
                {
                    list = list.Where(p => p.IsCompleteStr == "否").ToList();
                }
            }
            #endregion

            var pieSer = new List<pieSeries>();
            //int a = list.Where(p => p.IsComplete_length >= zhi).Count();
            //pieSer.Add(new pieSeries { name = "已完成", y = Math.Round(Convert.ToDouble(list.Where(p => p.IsComplete_length >= zhi).Count()) / Convert.ToDouble(list.Count()),2) });
            //pieSer.Add(new pieSeries { name = "未开展", y = Math.Round(Convert.ToDouble(list.Where(p => p.IsComplete_length == 0).Count()) / Convert.ToDouble(list.Count()),2) });
            //pieSer.Add(new pieSeries { name = "开展中", y = Math.Round(Convert.ToDouble(list.Where(p => p.IsComplete_length > 0 && p.IsComplete_length < 16).Count()) / Convert.ToDouble(list.Count()), 2) });
            pieSer.Add(new pieSeries { name = "已完成", y = list.Where(p => p.IsComplete_length >= zhi).Count() });
            pieSer.Add(new pieSeries { name = "未开展", y = Convert.ToDouble(list.Where(p => p.IsComplete_length == 0).Count()) });
            pieSer.Add(new pieSeries { name = "开展中", y = Convert.ToDouble(list.Where(p => p.IsComplete_length > 0 && p.IsComplete_length < 16).Count()) });
            joinpie.DivID = "showpie";
            joinpie.title = "各部门培训进度";
            joinpie.pieseries = pieSer;
            joinpie.charttype = Charttype.Pie;
            joinpie.reportFormat = 1;


            var fengSer = new List<pieSeries>();
            //fengSer.Add(new pieSeries { name = "小于16学时", y = Math.Round(Convert.ToDouble(list.Where(p => p.Dep_Course_Commencement_length < 16).Count()) / Convert.ToDouble(list.Count()),2) });
            //fengSer.Add(new pieSeries { name = "17-20学时", y = Math.Round(Convert.ToDouble(list.Where(p => p.Dep_Course_Commencement_length >= 17 && p.Dep_Course_Commencement_length <= 20).Count()) / Convert.ToDouble(list.Count()),2) });
            //fengSer.Add(new pieSeries { name = "21-30学时", y = Math.Round(Convert.ToDouble(list.Where(p => p.Dep_Course_Commencement_length > 20 && p.Dep_Course_Commencement_length <= 30).Count()) / Convert.ToDouble(list.Count()),2) });
            //fengSer.Add(new pieSeries { name = "大于30学时", y = Math.Round(Convert.ToDouble(list.Where(p => p.Dep_Course_Commencement_length > 30).Count()) / Convert.ToDouble(list.Count()), 2) });
            fengSer.Add(new pieSeries { name = "小于16学时", y = Convert.ToDouble(list.Where(p => p.Dep_Course_Commencement_length < 16).Count()) });
            fengSer.Add(new pieSeries { name = "17-20学时", y = Convert.ToDouble(list.Where(p => p.Dep_Course_Commencement_length >= 17 && p.Dep_Course_Commencement_length <= 20).Count()) });
            fengSer.Add(new pieSeries { name = "21-30学时", y = Convert.ToDouble(list.Where(p => p.Dep_Course_Commencement_length > 20 && p.Dep_Course_Commencement_length <= 30).Count()) });
            fengSer.Add(new pieSeries { name = "大于30学时", y = Convert.ToDouble(list.Where(p => p.Dep_Course_Commencement_length > 30).Count()) });
            joinfeng.DivID = "showfeng";
            joinfeng.title = "计划开设课程学时数分布图";
            joinfeng.pieseries = fengSer;
            joinfeng.charttype = Charttype.Pie;
            joinfeng.reportFormat = 1;

            //用 CourseFrom   0：年度计划；1：月度计划；2：课程管理  
            //年度的计划开课
            var passSeries = new Series
            {
                name = "计划开课",
                data = new List<double>()
            };

            //开课
            var zongsuopassSeries = new Series
            {
                name = "实际开课",
                data = new List<double>()
            };

            var courselist = iDep_Course.GetDep_CourseList(year);

            var monthSer = new List<Series>();

            var xAxis = new List<String>();

            int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            for (int i = 0; i < arr.Length; i++)
            {
                passSeries.data.Add(courselist.Where(p => p.CourseFrom == 0 && p.YearPlan == year && Convert.ToInt32(p.Month.Split('-')[1]) == arr[i]).Count());
                //zongsuopassSeries.data.Add(courselist.Where(p => p.CourseFrom == 2 && p.YearPlan == year && Convert.ToInt32(p.Month.Split('-')[1]) == arr[i]).Count());
                zongsuopassSeries.data.Add(courselist.Where(p => p.CourseFrom == 2 && p.YearPlan == year && Convert.ToInt32(p.StartTime.ToString("yyyy-MM-dd").Split('-')[1]) == arr[i]).Count());
                xAxis.Add(arr[i].ToString() + "月");
            }
            monthSer.Add(passSeries);
            monthSer.Add(zongsuopassSeries);
            joinzong.DivID = "showmonth";
            joinzong.xAxis = xAxis;
            joinzong.charttype = Charttype.Line;
            joinzong.title = "各月份学习情况分布图";
            joinzong.series = monthSer;


            return Json(new
            {
                pie = joinpie,
                feng = joinfeng,
                month = joinzong

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 完成情况明细
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptype">0：总所 1：分所</param>
        /// <param name="deptid"></param>
        /// <param name="realname"></param>
        /// <param name="cpa"></param>
        /// <param name="year"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        public JsonResult GetReport_CompleteDetail(int deptype, string deptid, string realname, string cpa, int year = 2013, int isFree = -1, int pageIndex = 1, int pageSize = 15, string jsRenderSortField = "")
        {
            if (Session["Report_CompleteDetail"] != null)
            {
                Session.Remove("Report_CompleteDetail");
            }
            Session["Report_CompleteDetail"] = deptid + "㉿" + realname + "㉿" + cpa + "㉿" + year + "㉿" + pageIndex;

            if (Session["Report_fCompleteDetail"] != null)
            {
                Session.Remove("Report_fCompleteDetail");
            }
            Session["Report_fCompleteDetail"] = deptid + "㉿" + realname + "㉿" + cpa + "㉿" + year + "㉿" + pageIndex;
            string sql = " and 1=1";

            if (deptype == 1)
            {
                string a = string.Join(",", GetAllSubLeardDepartments());
                sql += " and  sys_user.deptid  in (" + a + ")";
            }

            if (!string.IsNullOrEmpty(deptid) && deptid != "0")
            {
                sql += " and sys_user.deptid in (" + deptid + ")";
            }

            if (!string.IsNullOrEmpty(realname))
            {
                sql += " and realname like '%" + realname + "%'";
            }

            if (!string.IsNullOrEmpty(cpa) && cpa != "2")
            {
                if (cpa == "0")
                {
                    sql += " and cpa='是'";
                }
                else
                {
                    sql += " and cpa='否'";
                }
            }

            sql += " and yearplan=" + year;

            //部门自学最大学时
            var DeptMaxScoreConfig = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 14).ConfigValue;
            var deptMaxScore = DeptMaxScoreConfig.Split(';')[2].Split(',')[0].StringToInt32();


            var config = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == year).FirstOrDefault();
            var list = iReport_DepCourse.GetReport_CompleteDetail(sql, config, year);

            if (isFree != -1)
            {
                list = list.Where(p => p.IsFree == isFree).ToList();
            }
            foreach (var item in list)
            {
                if (item.GetLength > deptMaxScore && deptMaxScore != -1)
                {
                    item.GetLength = deptMaxScore;
                }
            }


            if (!string.IsNullOrEmpty(jsRenderSortField) && !jsRenderSortField.Contains(","))
            {
                list = list.SortListByField(jsRenderSortField);
            }
            else
            {
                list = list.OrderBy(p => p.DeptId).ThenBy(p => p.GetLength)
                    .ToList();
            }


            int limit = list.Count();

            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                recordCount = limit,
                dataList = list

            }, JsonRequestBehavior.AllowGet);
        }


        public void getReport_CompleteDetailForReport(int deptype, string deptid, string realname, string cpa, int year = 2013,int isFree=-1, int pageIndex = 1, int pageSize = 15)
        {
            string sql = " and 1=1";

            if (deptype == 1)
            {
                string a = string.Join(",", GetAllSubLeardDepartments());
                sql += " and  sys_user.deptid  in (" + a + ")";
            }

            if (!string.IsNullOrEmpty(deptid) && deptid != "0")
            {
                sql += " and sys_user.deptid in(" + deptid + ")";
            }

            if (!string.IsNullOrEmpty(realname))
            {
                sql += " and realname like '%" + realname + "%'";
            }

            if (!string.IsNullOrEmpty(cpa) && cpa != "2")
            {
                if (cpa == "0")
                {
                    sql += " and cpa='是'";
                }
                else
                {
                    sql += " and cpa='否'";
                }
            }
            sql += " and yearplan=" + year;

            //部门自学最大学时
            var DeptMaxScoreConfig = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 14).ConfigValue;
            var deptMaxScore = DeptMaxScoreConfig.Split(';')[2].Split(',')[0].StringToInt32();

            var config = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == year).FirstOrDefault();
            var list = iReport_DepCourse.GetReport_CompleteDetail(sql, config, year);

            if (isFree != -1)
            {
                list = list.Where(p => p.IsFree == isFree).ToList();
            }
                
            foreach (var item in list)
            {
                if (item.GetLength > deptMaxScore && deptMaxScore != -1)
                {
                    item.GetLength = deptMaxScore;
                }
            }

            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("部门/分所", typeof(string));
            outTable.Columns.Add("账号", typeof(string));
            outTable.Columns.Add("姓名", typeof(string));
            outTable.Columns.Add("是否CPA", typeof(string));
            outTable.Columns.Add("参与课程数", typeof(string));
            outTable.Columns.Add("已获取部门自学学时", typeof(string));
            outTable.Columns.Add("请假次数", typeof(string));
            outTable.Columns.Add("退订次数", typeof(string));
            outTable.Columns.Add("迟到次数", typeof(string));
            outTable.Columns.Add("缺勤次数", typeof(string));
            outTable.Columns.Add("课前建议次数", typeof(string));
            outTable.Columns.Add("课后评估次数", typeof(string));
            outTable.Columns.Add("在线测试通过次数", typeof(string));
            outTable.Columns.Add("参与需求调研次数", typeof(string));
            var index = 1;
            foreach (var v in list)
            {
                outTable.Rows.Add(index, v.Dep_DeptName, v.Username, v.RealName, v.IsCPAStr, v.CanYu, v.GetLength,
                     v.Qj_Num, v.Td_Num, v.Cd_Num, v.Qq_Num, v.Advice_Num, v.AfterAnswer_Num, v.OnlineTest_Num, v.Research_Num);
                index++;
            }
            new Spreadsheet().Template(year + "年部门或分所自学员工学习情况明细表", null, outTable, HttpContext, year + "年部门或分所自学员工学习情况明细表", "部门或分所自学员工学习情况明细表");


        }
        #endregion

        #region 暂时没用
        public JsonResult GetSelectDept(int year, int ReportType = 0, int pageIndex = 1, int pageSize = 15)
        {

            var list = iReport_DepCourse.GetReport_Complete(year, " 1=1");

            string deptodlist = "";
            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    deptodlist += item.DeptId + ",";
                }
            }
            int limit = list.Count();
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();


            return Json(new
            {
                dataList = list,
                recordCount = limit,
                deptodlist = deptodlist != "" ? deptodlist.Substring(0, deptodlist.LastIndexOf(',')) : ""
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 师资列表
        public ActionResult CourseSurveyTeacher(int teacher)
        {
            ViewBag.teacher = teacher;
            ViewBag.show = 0;
            return View();
        }


        /// <summary>
        /// 师资列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetReport_Teachers(string realname, string deptid, string OpenLevel, string cpa, string PayGrade, string IsTeacher, string year = "2013", int isFree = -1, int pageIndex = 1, int pageSize = 10, string jsRenderSortField = " Dep_Survey_ReplyAnswer desc")
        {

            if (Session["Report_Teachers"] != null)
            {
                Session.Remove("Report_Teachers");
            }
            Session["Report_Teachers"] = realname + "㉿" + deptid + "㉿" + OpenLevel + "㉿" + cpa + "㉿" + PayGrade
                                         + "㉿" + IsTeacher + "㉿" + year + "㉿" + pageIndex;

            string sql = "";
            if (!string.IsNullOrEmpty(realname))
            {
                sql += " and realname like '%" + realname + "%'";
            }

            if (!string.IsNullOrEmpty(year))
            {
                sql += " and yearplan=" + year;
            }
            if (!string.IsNullOrEmpty(deptid))
            {
                sql += " and sys_user.deptid in(" + deptid + ")";
            }
            if (!string.IsNullOrEmpty(OpenLevel))
            {
                sql += @" and TrainGrade in (" + OpenLevel + ")";
            }
            if (!string.IsNullOrEmpty(cpa) && cpa != "2")
            {
                if (cpa == "0")
                    sql += " and sys_user.CPA='是'";
                else
                    sql += " and sys_user.CPA='否'";
            }
            if (!string.IsNullOrEmpty(PayGrade))
            {
                sql += @" and  PayGrade IN (" + PayGrade + ")";
            }
            if (!string.IsNullOrEmpty(IsTeacher) && IsTeacher != "3" && IsTeacher != "1")
            {
                sql += " and sys_User.IsTeacher=" + IsTeacher;
            }

            var config = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == year.StringToInt32()).FirstOrDefault();
            var list = iReport_DepCourse.GetReport_Teachers(config,sql, Convert.ToInt32(year));
            if (isFree != -1)
            {
                list = list.Where(p => p.IsFree == isFree).ToList();
            }

            list = list.SortListByField(jsRenderSortField);
            int limit = list.Count();
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                dataList = list,
                recordCount = limit

            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出师资列表
        /// </summary>
        /// <returns></returns>
        public void GetReport_TeachersForReport(string realname, string deptid, string OpenLevel, string cpa, string PayGrade, string IsTeacher, string year = "2013", int isFree = -1, string jsRenderSortField = " UserId desc")
        {
            string sql = "";
            if (!string.IsNullOrEmpty(realname))
            {
                sql += " and realname like '%" + realname + "%'";
            }

            if (!string.IsNullOrEmpty(year))
            {
                sql += " and yearplan=" + year;
            }
            if (!string.IsNullOrEmpty(deptid))
            {
                sql += " and sys_user.deptid in(" + deptid + ")";
            }
            if (!string.IsNullOrEmpty(OpenLevel))
            {
                sql += @" and TrainGrade in (" + OpenLevel + ")";
            }
            if (!string.IsNullOrEmpty(cpa) && cpa != "2")
            {
                if (cpa == "0")
                    sql += " and sys_user.CPA='是'";
                else
                    sql += " and sys_user.CPA='否'";
            }
            if (!string.IsNullOrEmpty(PayGrade))
            {
                sql += @" and  PayGrade IN (" + PayGrade + ")";
            }
            if (!string.IsNullOrEmpty(IsTeacher) && IsTeacher != "3" && IsTeacher != "1")
            {
                sql += " and sys_User.IsTeacher=" + IsTeacher;
            }

            var config = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == year.StringToInt32()).FirstOrDefault();
            var list = iReport_DepCourse.GetReport_Teachers(config,sql, Convert.ToInt32(year));

            if (isFree != -1)
            {
                list = list.Where(p => p.IsFree == isFree).ToList();
            }

            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("姓名", typeof(string));
            outTable.Columns.Add("部门/分所/公司/机构", typeof(string));
            outTable.Columns.Add("薪酬级别/职务", typeof(string));
            outTable.Columns.Add("培训级别", typeof(string));
            outTable.Columns.Add("是否CPA", typeof(string));
            outTable.Columns.Add("授课次数", typeof(string));
            outTable.Columns.Add("授课累计学时", typeof(string));
            outTable.Columns.Add("全年课程评估讲师评分均值", typeof(string));

            int i = 1;
            foreach (var item in list)
            {
                outTable.Rows.Add(i, item.RealName, item.Dep_DeptName, item.PayGrade, item.TrainGrade, item.IsCPA, item.courseTeacherCount, item.All_CourseLength, item.Dep_Survey_ReplyAnswer);
                ++i;
            }

            new Spreadsheet().Template(year + "年部门或分所自学涉及师资情况", null, outTable, HttpContext, year + "年部门或分所自学涉及师资情况", year + "年部门或分所自学涉及师资情况");
        }

        /// <summary>
        /// 讲师上课的课程
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepCourseSurvey(int teacher, int year, string courseName = "", string StartTime = "", string EndTime = "", string IsMust = "", int pageIndex = 1, int pageSize = 10, string jsRenderSortField = " Survey_TeacherScoreDouble desc")
        {
            try
            {
                var where = " 1=1 and YearPlan=" + year;

                if (!string.IsNullOrEmpty(courseName))
                {
                    where += " and CourseName like '%" + courseName.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(StartTime))
                {
                    where += " and  StartTime>='" + StartTime + "'";
                }
                if (!string.IsNullOrEmpty(EndTime))
                {
                    where += " and  EndTime<='" + EndTime + "'";
                }
                if (!string.IsNullOrEmpty(IsMust))
                {
                    where += " and  IsMust=" + IsMust;
                }
                var list = iReport_DepCourse.GetDepCourseSurvey(teacher, where);
                list = list.SortListByField(jsRenderSortField);
                var totalCount = list.Count;
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new
                {
                    recordCount = totalCount,
                    dataList = list
                }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new
                {
                    recordCount = 0,
                    dataList = new List<Dep_Course>()
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region
        public JsonResult GetSelectPayGrade(string deptname, int pageIndex = 1, int pageSize = 15)
        {
            string sql = "";

            if (!string.IsNullOrEmpty(deptname))
            {
                sql += " and paygrade like '%" + deptname + "%'";
            }


            var list = iReport_DepCourse.GetPayGrade(sql);

            //string deptodlist = "";
            //if (list.Count != 0)
            //{
            //    foreach (var item in list)
            //    {
            //        deptodlist += item.deptid + ",";
            //    }
            //}
            int limit = list.Count();
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                dataList = list,
                recordCount = limit
                //deptodlist = deptodlist != "" ? deptodlist.Substring(0, deptodlist.LastIndexOf(',')) : ""
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }



    public class PropertyComparer<T> : IEqualityComparer<T>
    {
        private PropertyInfo _PropertyInfo;

        /// <summary>
        /// 通过propertyName 获取PropertyInfo对象        /// </summary>
        /// <param name="propertyName"></param>
        public PropertyComparer(string propertyName)
        {
            _PropertyInfo = typeof(T).GetProperty(propertyName,
            BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            if (_PropertyInfo == null)
            {
                throw new ArgumentException(string.Format("{0} is not a property of type {1}.",
                    propertyName, typeof(T)));
            }
        }

        #region IEqualityComparer<T> Members

        public bool Equals(T x, T y)
        {
            object xValue = _PropertyInfo.GetValue(x, null);
            object yValue = _PropertyInfo.GetValue(y, null);

            if (xValue == null)
                return yValue == null;

            return xValue.Equals(yValue);
        }

        public int GetHashCode(T obj)
        {
            object propertyValue = _PropertyInfo.GetValue(obj, null);

            if (propertyValue == null)
                return 0;
            else
                return propertyValue.GetHashCode();
        }

        #endregion
    }
}
