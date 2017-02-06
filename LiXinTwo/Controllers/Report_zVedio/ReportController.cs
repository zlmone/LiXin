using LiXinCommon;
using LiXinInterface.Report_fVedio;
using LiXinModels;
using LiXinModels.Report_Vedio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace LiXinControllers
{
    public partial class Report_zVedioController : BaseController
    {
        public static ChartModel testpie = new ChartModel();
        public static ChartModel testcolumn = new ChartModel();
        public ActionResult Report_zOnLineTestDetail(int courseid)
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.courseid = courseid;
            return View();
        }

        public ActionResult Report_zOnLineTest(int courseid)
        {
            ViewBag.courseid = courseid;
            return View();
        }


        #region 总所学习人员明细-execl

        public JsonResult GetzOnlineTestDetail(int courseid, string DeptName, string RealName, string trainGrade, string sltStatus, string sltcpa, string StartTime, string EndTime,
            int isFree=-1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " DeptId,TrainGrade,VedioTime asc", int type = 0)
        {
            int limit = 0;
            string sql = "";
            if (type == 0)
            {
                sql = "   UserId in (SELECT UserId FROM dbo.View_CheckUser)";
            }
            else
            {
                string a = string.Join(",", GetAllSubLeardDepartments());

                sql += " DeptId in (" + a + ")  and  UserId in (SELECT UserId FROM dbo.View_CheckUser)";

            }

            if (!string.IsNullOrEmpty(DeptName))
            {
                sql += " and deptname like '%" + DeptName + "%'";
            }

            if (!string.IsNullOrEmpty(RealName))
            {
                sql += " and Realname like '%" + RealName + "%'";
            }

            if (!string.IsNullOrEmpty(trainGrade))
            {
                //sql += " and TrainGrade like '%" + trainGrade + "%'";
                sql += "And (SELECT count(*) FROM  dbo.F_SplitIDs(trainGrade)  WHERE ID  IN (SELECT ID FROM dbo.F_SplitIDs('" + trainGrade + "')) )>0";
            }

            if (!string.IsNullOrEmpty(sltcpa) && sltcpa != "99")
            {
                if (sltcpa == "1")
                { sql += " and CPA='是'"; }
                else
                {
                    sql += " and CPA='否'";
                }
            }

            if (!string.IsNullOrEmpty(StartTime))
            {
                sql += " and  t.LastUpdateTime>='" + StartTime + "'";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " and  t.LastUpdateTime<='" + EndTime + "'";
            }


            var list = iIReport_OnLineTestBL.GetOnLineTestDetail(out limit, courseid, sql, jsRenderSortField: " order by " + jsRenderSortField);

            if (!string.IsNullOrEmpty(sltStatus) && sltStatus != "99")
            {
                //list = list.SortListByField(jsRenderSortField).Where(p => p.ExamStatus == Convert.ToInt32(sltStatus)).ToList();
                if (sltStatus == "1")
                {
                    list = list.Where(p => p.IsPass == 0 && p.ExamStatus == "0").ToList();
                }
                if (sltStatus == "2")
                {
                    list = list.Where(p => p.IsPass == 0 && p.ExamStatus == "1").ToList();
                }
                if (sltStatus == "3")
                {
                    list = list.Where(p => p.IsPass == 1 && p.ExamStatus == "2").ToList();
                }
                if (sltStatus == "4")
                {
                    list = list.Where(p => p.IsPass == 0 && p.ExamStatus == "2").ToList();
                }
            }
           
            // list = list.SortListByField(jsRenderSortField);


            int ALLLearnTimes = list.Sum(p => p.LearnTimes);
            //ViewBag.ALLLearnTimes = ALLLearnTimes;
            decimal ALLVedioTimes = Math.Round(list.Sum(p => p.VedioTime) / 60, 0);
            string ALLExamNum = "";

            if (list.Where(p => p.ExamNum == "N/A").ToList().Count() == list.Count())
            {
                ALLExamNum = "N/A";
            }
            else
            {
                ALLExamNum = list.Where(p => p.ExamNum != "N/A").Sum(p => Convert.ToInt32(p.ExamNum)).ToString();
            }
            //ViewBag.ALLExamNum = list.Sum(p =>Convert.ToInt32(p.ExamNum));

            if (isFree != -1)
            {
                list = list.Where(p => p.IsFree == isFree).ToList();
            }

            string ALLExamScore = ReportCommon.CalculateMedian(list.Select(p => p.ExamScore).ToList());
            string ALLGetLength = ReportCommon.CalculateMedian(list.Select(p => p.GetLength).ToList());

          
            limit = list.Count();

            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                recordCount = limit,
                dataList = list,
                ALLLearnTimes = ALLLearnTimes,
                ALLVedioTimes = ALLVedioTimes,
                ALLExamNum = ALLExamNum,
                ALLExamScore = ALLExamScore,
                ALLGetLength = ALLGetLength
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 总所人员明细-execl
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="DeptName"></param>
        /// <param name="RealName"></param>
        /// <param name="trainGrade"></param>
        /// <param name="sltStatus"></param>
        /// <param name="sltcpa"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="jsRenderSortField"></param>
        public void GetzOnlineTestDetailForReport(int courseid, string DeptName, string RealName, string trainGrade, string sltStatus, string sltcpa, string StartTime, string EndTime,int isFree=-1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " Sys_User.UserId desc", int type = 0)
        {
            #region 数据
            int limit = 0;
            string sql = "";
            if (type == 0)
            {
                sql = "   UserId in (SELECT UserId FROM dbo.View_CheckUser)";
            }
            else
            {
                string a = string.Join(",", GetAllSubLeardDepartments());

                sql += " DeptId in (" + a + ")  and  UserId in (SELECT UserId FROM dbo.View_CheckUser)";

            }

            if (!string.IsNullOrEmpty(DeptName))
            {
                sql += " and deptname like '%" + DeptName + "%'";
            }

            if (!string.IsNullOrEmpty(RealName))
            {
                sql += " and Realname like '%" + RealName + "%'";
            }

            if (!string.IsNullOrEmpty(trainGrade))
            {
                //sql += " and TrainGrade like '%" + trainGrade + "%'";
                sql += "And (SELECT count(*) FROM  dbo.F_SplitIDs(trainGrade)  WHERE ID  IN (SELECT ID FROM dbo.F_SplitIDs('" + trainGrade + "')) )>0";
            }

            if (!string.IsNullOrEmpty(sltcpa) && sltcpa != "99")
            {
                if (sltcpa == "1")
                { sql += " and CPA='是'"; }
                else
                {
                    sql += " and CPA='否'";
                }
            }

            if (!string.IsNullOrEmpty(StartTime))
            {
                sql += " and  t.LastUpdateTime>='" + StartTime + "'";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                sql += " and  t.LastUpdateTime<='" + EndTime + "'";
            }

            var list = iIReport_OnLineTestBL.GetOnLineTestDetail(out limit, courseid, sql, pageIndex, pageSize, "  order by DeptId,TrainGrade,VedioTime asc");

            if (!string.IsNullOrEmpty(sltStatus) && sltStatus != "99")
            {
                if (sltStatus == "1")
                {
                    list = list.Where(p => p.IsPass == 0 && p.ExamStatus == "0").ToList();
                }
                if (sltStatus == "2")
                {
                    list = list.Where(p => p.IsPass == 0 && p.ExamStatus == "1").ToList();
                }
                if (sltStatus == "3")
                {
                    list = list.Where(p => p.IsPass == 1 && p.ExamStatus == "2").ToList();
                }
                if (sltStatus == "4")
                {
                    list = list.Where(p => p.IsPass == 0 && p.ExamStatus == "2").ToList();
                }
            }
            else
            {
                //list = list.SortListByField(jsRenderSortField);

            }

            if (isFree != -1)
            {
                list = list.Where(p => p.IsFree == isFree).ToList();
            }

            var dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("部门/分所", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("登录名", typeof(string));
            dt.Columns.Add("培训级别", typeof(string));
            dt.Columns.Add("是否CPA", typeof(string));
            dt.Columns.Add("CPA编号", typeof(string));
            dt.Columns.Add("在线观看次数", typeof(string));
            dt.Columns.Add("在线观看时间（分钟）", typeof(string));
            dt.Columns.Add("考试状态", typeof(string));
            dt.Columns.Add("考试次数", typeof(string));
            dt.Columns.Add("考试成绩", typeof(string));
            dt.Columns.Add("获取学时", typeof(string));

            var count = 1;
            foreach (Report_OnLineTest item in list)
            {
                dt.Rows.Add(count++, item.DeptName, item.Realname, item.Username, item.TrainGrade, item.CPA, "'" + item.CPANo, item.LearnTimes,
                            Math.Round(item.VedioTime / 60, 0), item.StrExamStatus, item.ExamNum, item.ExamScore, item.ExamStatus == "0" ? "N/A" : item.GetLength.ToString());
            }

            string ALLExamNum = "";

            if (list.Where(p => p.ExamNum == "N/A").ToList().Count() == list.Count())
            {
                ALLExamNum = "N/A";
            }
            else
            {
                ALLExamNum = list.Where(p => p.ExamNum != "N/A").Sum(p => Convert.ToInt32(p.ExamNum)).ToString();
            }

            dt.Rows.Add("合计", "--", "--", "--", "--", "--", list.Sum(p => p.LearnTimes),
                            Math.Round(list.Sum(p => p.VedioTime) / 60, 0), "--", ALLExamNum, ReportCommon.CalculateMedian(list.Select(p => p.ExamScore).ToList()), ReportCommon.CalculateMedian(list.Select(p => p.GetLength).ToList()));
            #endregion

            //var dtList = new List<DataTable>();
            //string strFileName = "总所学习人员明细";
            //dtList.Add(dt);
            //var export = new Spreadsheet();
            //export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, strFileName);
            var value = type == 0 ? "全所" : "分所";
            SheetModel model = new SheetModel();

            model.SheetName = "视频课程学习人员明细" + value;
            model.space = 2;

            var datalist = new List<DataModel>();

            var data = new DataModel();
            data.datatype = Datatype.DataTable;
            data.Dataseries = GetCourseTable(courseid, type);
            datalist.Add(data);

            var data1 = new DataModel();
            data1.datatype = Datatype.DataTable;
            data1.Dataseries = dt;
            datalist.Add(data1);

            model.DataModels = datalist;

            var sheetmodelList = new List<SheetModel>();
            sheetmodelList.Add(model);
            new Spreadsheet().ExportExcel(sheetmodelList, HttpContext, "视频课程学习人员明细" + value);

        }


        #endregion

        #region 总所在线测试情况-页面数据
        /// <summary>
        /// 总所
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="jsRenderSortField"></param>
        /// <param name="type">0 总所  1分所</param>
        /// <returns></returns>
        public JsonResult GetfOnlineTest(int courseid, string CourseName, string StartTime, string EndTime, string jsRenderSortField = "JoinPateDouble desc", int type = 0)
        {
            string where = "";
            if (type == 0)
            {
                //                where = @" DeptId in(
                //		 select sys_user.deptid from dbo.Cl_CpaLearnStatus
                //		left join sys_user on Cl_CpaLearnStatus.userid=sys_user.userid
                //      where courseid=" + courseid + @" group by sys_user.deptid
                //     ) and    DeptId in (SELECT DeptID FROM dbo.View_CheckUser)";
                where = @" syu.UserId in (SELECT UserId FROM dbo.View_CheckUser)";
            }
            else
            {

                string a = string.Join(",", GetAllSubLeardDepartments());
                where += "( DeptId in (" + a + "))";

                if (!string.IsNullOrEmpty(CourseName))
                {
                    where += " and deptname like '%" + CourseName.ReplaceSql() + "%'";
                }

                where += " and  syu.UserId in (SELECT UserId FROM dbo.View_CheckUser)";
            }

            if (!string.IsNullOrEmpty(CourseName))
            {
                where += " and deptname like '%" + CourseName + "%'";
            }

            //string time=" 1=1";
            if (!string.IsNullOrEmpty(StartTime))
            {
                where += " and  ccs.LastUpdateTime>='" + StartTime + "'";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                where += " and  ccs.LastUpdateTime<='" + EndTime + "'";
            }

            //总所
            var list = iIReport_OnLineTestBL.GetOnLineTest(courseid, where);
            //            var newlist = list;
            //            if (type == 1)
            //            {
            //                string where1 = @"DeptId in(
            //		 select sys_user.deptid from dbo.Cl_CpaLearnStatus
            //		left join sys_user on Cl_CpaLearnStatus.userid=sys_user.userid
            //      where courseid=" + courseid + @" group by sys_user.deptid
            //     ) and     DeptId in (SELECT DeptID FROM dbo.View_CheckUser)";
            //                newlist = iIReport_OnLineTestBL.GetOnLineTest(courseid, where1);
            //            }

            StringBuilder sb = new StringBuilder();

            if (list.Count() != 0)
            {
                sb.Append("<tr>");
                sb.Append("<td></td>");
                sb.Append("<td>全所合计（" + list.Count() + "个）</td>");
                if (list.Sum(p => p.YingCanJia) != 0)
                {
                    sb.Append("<td>" + (Convert.ToDecimal(list.Sum(p => p.OnLineTestNum)) / Convert.ToDecimal(list.Sum(p => p.YingCanJia))).ToString("p") + "</td>");
                }
                else
                {
                    sb.Append("<td>0</td>");
                }
                sb.Append("<td>" + list.Sum(p => p.OnLineTestNum) + "</td>");
                sb.Append("<td>" + list.Sum(p => p.YingCanJia) + "</td>");
                sb.Append("<td>" + list.Sum(p => p.IsPassNum) + "</td>");
                if (list.Sum(p => p.OnLineTestNum) != 0)
                {
                    sb.Append("<td>" + (Convert.ToDecimal(list.Sum(p => p.IsPassNum)) / Convert.ToDecimal(list.Sum(p => p.OnLineTestNum))).ToString("p") + "</td>");
                }
                else
                {
                    sb.Append("<td>0.00%</td>");
                }
                sb.Append("<td>" + ReportCommon.CalculateMedian(list.Where(p => p.OnLineTestNum > 0).Select(p => p.AvgAnswerTimeMin).ToList()) + "</td>");//
                sb.Append("<td>" + ReportCommon.CalculateMedian(list.Where(p => p.OnLineTestNum > 0).Select(p => p.AvgAnswerNum).ToList()) + "</td>");
                sb.Append("<td>" + ReportCommon.CalculateMedian(list.Where(p => p.OnLineTestNum > 0).Select(p => p.Avg).ToList()) + "</td>");
                sb.Append("<td></td>");
                sb.Append("</tr>");
            }
            else
            {
                sb.Append("<tr><td></td><td>全所合计（0个）</td><td>0.00%</td><td>0</td><td>0</td><td>0</td><td>0.00%</td><td>0</td><td>0</td><td>0</td><td></td></tr>");
                sb.Append("<tr class='odd'><td colspan='11'><div class='tc c38 line_h30'>暂无数据</div></td></tr>");

            }

            var zongsuo = list.Where(p => p.DeptName.Contains("上海")).ToList().SortListByField(jsRenderSortField);

            if (zongsuo.Count() != 0)
            {
                sb.Append("<tr>");
                sb.Append("<td></td>");
                sb.Append("<td>总所合计（" + zongsuo.Count() + "个）</td>");
                if (zongsuo.Sum(p => p.YingCanJia) != 0)
                {
                    sb.Append("<td>" + (Convert.ToDecimal(zongsuo.Sum(p => p.OnLineTestNum)) / Convert.ToDecimal(zongsuo.Sum(p => p.YingCanJia))).ToString("p") + "</td>");
                }
                else
                {
                    sb.Append("<td>0</td>");
                }
                sb.Append("<td>" + zongsuo.Sum(p => p.OnLineTestNum) + "</td>");
                sb.Append("<td>" + zongsuo.Sum(p => p.YingCanJia) + "</td>");
                sb.Append("<td>" + zongsuo.Sum(p => p.IsPassNum) + "</td>");
                //sb.Append("<td>" + (Convert.ToDecimal(zongsuo.Sum(p => p.IsPassNum)) / Convert.ToDecimal(zongsuo.Sum(p => p.OnLineTestNum))).ToString("p") + "</td>");
                if (zongsuo.Sum(p => p.OnLineTestNum) != 0)
                {
                    sb.Append("<td>" + (Convert.ToDecimal(zongsuo.Sum(p => p.IsPassNum)) / Convert.ToDecimal(zongsuo.Sum(p => p.OnLineTestNum))).ToString("p") + "</td>");
                }
                else
                {
                    sb.Append("<td>0.00%</td>");
                }
                sb.Append("<td>" + ReportCommon.CalculateMedian(zongsuo.Where(p => p.OnLineTestNum > 0).Select(p => p.AvgAnswerTimeMin).ToList()) + "</td>");
                sb.Append("<td>" + ReportCommon.CalculateMedian(zongsuo.Where(p => p.OnLineTestNum > 0).Select(p => p.AvgAnswerNum).ToList()) + "</td>");
                sb.Append("<td>" + ReportCommon.CalculateMedian(zongsuo.Where(p => p.OnLineTestNum > 0).Select(p => p.Avg).ToList()) + "</td>");
                sb.Append("<td><a class='buttonhide' onclick='showHidetest(1,this)'><a></td>");
                sb.Append("</tr>");

                int zongsuonum = 1;
                foreach (var item in zongsuo)
                {
                    sb.Append("<tr class='reporttest_1' style='display:none'>");
                    sb.Append("<td>" + zongsuonum++ + "</td>");
                    sb.Append("<td><a onclick=\"viewTestDetail('" + item.DeptIDs + "')\"' style=\"cursor:pointer\">" + item.DeptTwoName + "</a></td>");
                    sb.Append("<td>" + item.JoinPate + "</td>");
                    sb.Append("<td>" + item.OnLineTestNum + "</td>");
                    sb.Append("<td>" + item.YingCanJia + "</td>");
                    sb.Append("<td>" + item.IsPassNum + "</td>");
                    //sb.Append("<td>" + (Convert.ToDecimal(item.IsPassNum) / Convert.ToDecimal(item.OnLineTestNum)).ToString("p") + "</td>");
                    sb.Append("<td>" + item.ExamPassPate + "</td>");
                    //if (item.OnLineTestNum != 0)
                    //{
                    //    sb.Append("<td>" + (Convert.ToDecimal(item.IsPassNum) / Convert.ToDecimal(item.OnLineTestNum)).ToString("p") + "</td>");
                    //}
                    //else
                    //{
                    //    sb.Append("<td>0.00%</td>");
                    //}
                    sb.Append("<td>" + (item.OnLineTestNum == 0 ? "N/A" : item.AvgAnswerTimeMin.ToString()) + "</td>");
                    sb.Append("<td>" + (item.OnLineTestNum == 0 ? "N/A" : item.AvgAnswerNum) + "</td>");
                    sb.Append("<td>" + (item.OnLineTestNum == 0 ? "N/A" : item.Avg) + "</td>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                }
            }



            var fensuo = list.Where(p => p.DeptName.Contains("分所")).ToList().SortListByField(jsRenderSortField);

            if (fensuo.Count() != 0)
            {
                sb.Append("<tr>");
                sb.Append("<td></td>");
                sb.Append("<td>分所合计（" + fensuo.Count() + "个）</td>");
                if (fensuo.Sum(p => p.YingCanJia) != 0)
                {
                    sb.Append("<td>" + (Convert.ToDecimal(fensuo.Sum(p => p.OnLineTestNum)) / Convert.ToDecimal(fensuo.Sum(p => p.YingCanJia))).ToString("p") + "</td>");
                }
                else
                {
                    sb.Append("<td>0</td>");
                }
                sb.Append("<td>" + fensuo.Sum(p => p.OnLineTestNum) + "</td>");
                sb.Append("<td>" + fensuo.Sum(p => p.YingCanJia) + "</td>");
                sb.Append("<td>" + fensuo.Sum(p => p.IsPassNum) + "</td>");
                //sb.Append("<td>" + (Convert.ToDecimal(fensuo.Sum(p => p.IsPassNum)) / Convert.ToDecimal(fensuo.Sum(p => p.OnLineTestNum))).ToString("p") + "</td>");
                if (fensuo.Sum(p => p.OnLineTestNum) != 0)
                {
                    sb.Append("<td>" + (Convert.ToDecimal(fensuo.Sum(p => p.IsPassNum)) / Convert.ToDecimal(fensuo.Sum(p => p.OnLineTestNum))).ToString("p") + "</td>");
                }
                else
                {
                    sb.Append("<td>0.00%</td>");
                }
                sb.Append("<td>" + ReportCommon.CalculateMedian(fensuo.Where(p => p.OnLineTestNum > 0).Select(p => p.AvgAnswerTimeMin).ToList()) + "</td>");
                sb.Append("<td>" + ReportCommon.CalculateMedian(fensuo.Where(p => p.OnLineTestNum > 0).Select(p => p.AvgAnswerNum).ToList()) + "</td>");
                sb.Append("<td>" + ReportCommon.CalculateMedian(fensuo.Where(p => p.OnLineTestNum > 0).Select(p => p.Avg).ToList()) + "</td>");
                sb.Append("<td><a class='buttonhide' onclick='showHidetest(0,this)'><a></td>");
                sb.Append("</tr>");

                int fensuonum = 1;
                foreach (var item in fensuo)
                {
                    sb.Append("<tr class='reporttest_0' style='display:none'>");
                    sb.Append("<td>" + fensuonum++ + "</td>");
                    sb.Append("<td><a onclick=\"viewTestDetail('" + item.DeptIDs + "')\" style=\"cursor:pointer\">" + item.DeptTwoName + "</a></td>");
                    sb.Append("<td>" + (item.YingCanJia == 0 ? "0.00%" : (Convert.ToDecimal(item.OnLineTestNum) / Convert.ToDecimal(item.YingCanJia)).ToString("p")) + "</td>");
                    sb.Append("<td>" + item.OnLineTestNum + "</td>");
                    sb.Append("<td>" + item.YingCanJia + "</td>");
                    sb.Append("<td>" + item.IsPassNum + "</td>");
                    //sb.Append("<td>" + (Convert.ToDecimal(item.IsPassNum) / Convert.ToDecimal(item.OnLineTestNum)).ToString("p") + "</td>");
                    if (item.OnLineTestNum != 0)
                    {
                        sb.Append("<td>" + (item.OnLineTestNum == 0 ? "0.00%" : (Convert.ToDecimal(item.IsPassNum) / Convert.ToDecimal(item.OnLineTestNum)).ToString("p")) + "</td>");
                    }
                    else
                    {
                        sb.Append("<td>0.00%</td>");
                    }
                    sb.Append("<td>" + (item.OnLineTestNum == 0 ? "N/A" : item.AvgAnswerTimeMin.ToString()) + "</td>");
                    sb.Append("<td>" + (item.OnLineTestNum == 0 ? "N/A" : item.AvgAnswerNum) + "</td>");
                    sb.Append("<td>" + (item.OnLineTestNum == 0 ? "N/A" : item.Avg) + "</td>");
                    sb.Append("<td></td>");
                    sb.Append("</tr>");
                }
            }
            //if (zongsuo.Count == 0 && fensuo.Count == 0)
            //{

            //    sb.Append("<tr class='odd'><td colspan='11'><div class='tc c38 line_h30'>暂无数据</div></td></tr>");

            //}
            var columnList = zongsuo;
            //var pieList = courseOrderBL.GetTrainGradeSubscribe(courseId);
            var pieList = fensuo; //报名成功的 
            var pieSer = new List<pieSeries>();
            var columnSer = new List<Series>();

            #region 柱形图
            var nameList = new List<string>();
            var columnSer1 = new List<Double>();
            //var columnSer2 = new List<Double>();
            foreach (var item in columnList)
            {
                columnSer1.Add(item.IsPassNum);
                nameList.Add(item.DeptTwoName);
            }
            columnSer.Add(new Series
            {
                data = columnSer1,
                name = "测试通过人数"
            });
            //columnSer.Add(new Series
            //{
            //    data = columnSer2,
            //    name = "已报名人数"
            //});

            testcolumn.DivID = "showcolumn";
            testcolumn.xAxis = nameList;
            testcolumn.title = "总所各部门通过测试人数分布";
            testcolumn.series = columnSer;
            testcolumn.charttype = Charttype.Column;
            #endregion

            #region 扇形图
            foreach (var item in pieList)
            {

                pieSer.Add(new pieSeries
                {
                    name = item.DeptTwoName,
                    y = item.IsPassNum
                });
            }
            testpie.DivID = "showpie";
            testpie.title = "分所人员通过测试人数分布图";
            testpie.pieseries = pieSer;
            testpie.reportFormat = 1;
            testpie.charttype = Charttype.Pie;
            #endregion

            return Json(new
            {
                dataList = sb.ToString(),
                pie = testpie,
                column = testcolumn,
                zongsuo = zongsuo.Count(),
                fensuo = fensuo.Count()
            }, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region 总所在线测试情况-execl
        public void GetzOnlineTestForReport(int courseid, string CourseName, string StartTime, string EndTime, int type = 0)
        {
            #region 数据


            string where = "";
            if (type == 0)
            {
              
                where = @" syu.UserId in (SELECT UserId FROM dbo.View_CheckUser)";
            }
            else
            {

                string a = string.Join(",", GetAllSubLeardDepartments());
                where += "( DeptId in (" + a + "))";

                if (!string.IsNullOrEmpty(CourseName))
                {
                    where += " and deptname like '%" + CourseName.ReplaceSql() + "%'";
                }

                where += " and  syu.UserId in (SELECT UserId FROM dbo.View_CheckUser)";
            }

            if (!string.IsNullOrEmpty(StartTime))
            {
                where += " and  ccs.LastUpdateTime>='" + StartTime + "'";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                where += " and  ccs.LastUpdateTime<='" + EndTime + "'";
            }

            if (!string.IsNullOrEmpty(CourseName))
            {
                where += " and deptname like '%" + CourseName + "%'";
            }
            where += " and DeptID in (SELECT DeptID FROM dbo.View_CheckUser)";
            //总所
            var list = iIReport_OnLineTestBL.GetOnLineTest(courseid, where);

            var dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("对象名称", typeof(string));
            dt.Columns.Add("测试参与率", typeof(string));
            dt.Columns.Add("已经参加人数", typeof(string));
            dt.Columns.Add("应参加人数", typeof(string));
            dt.Columns.Add("测试通过人数", typeof(string));
            dt.Columns.Add("测试通过率", typeof(string));
            dt.Columns.Add("平均答题时间(分钟)", typeof(string));
            dt.Columns.Add("平均答题次数", typeof(string));
            dt.Columns.Add("平均得分", typeof(string));


            if (list.Count() != 0)
            {
                dt.Rows.Add(""
                    , "全所合计（" + list.Count() + "）"
                    , list.Sum(p => p.YingCanJia) == 0 ? "0" : (Convert.ToDecimal(list.Sum(p => p.OnLineTestNum)) / Convert.ToDecimal(list.Sum(p => p.YingCanJia))).ToString("p")
                    , list.Sum(p => p.OnLineTestNum)
                    , list.Sum(p => p.YingCanJia)
                    , list.Sum(p => p.IsPassNum)
                    , list.Sum(p => p.OnLineTestNum) == 0 ? "0" : (Convert.ToDecimal(list.Sum(p => p.IsPassNum)) / Convert.ToDecimal(list.Sum(p => p.OnLineTestNum))).ToString("p")
                    , ReportCommon.CalculateMedian(list.Where(p => p.OnLineTestNum > 0).Select(p => p.AvgAnswerTimeMin).ToList())
                    , ReportCommon.CalculateMedian(list.Where(p => p.OnLineTestNum > 0).Select(p => p.AvgAnswerNum).ToList())
                    , ReportCommon.CalculateMedian(list.Where(p => p.OnLineTestNum > 0).Select(p => p.Avg).ToList()));
            }
            else
            {
                dt.Rows.Add("", "全所合计（0）", "0.00%", 0, 0, 0, "0.00%", 0, 0, 0);
            }

            var zongsuo = list.Where(p => p.DeptName.Contains("上海")).ToList();

            if (zongsuo.Count() != 0)
            {
                dt.Rows.Add(""
                    , "总所合计（" + zongsuo.Count() + "）"
                    , zongsuo.Sum(p => p.YingCanJia) == 0 ? "0" : (Convert.ToDecimal(zongsuo.Sum(p => p.OnLineTestNum)) / Convert.ToDecimal(zongsuo.Sum(p => p.YingCanJia))).ToString("p")
                    , zongsuo.Sum(p => p.OnLineTestNum)
                    , zongsuo.Sum(p => p.YingCanJia)
                    , zongsuo.Sum(p => p.IsPassNum)
                    , zongsuo.Sum(p => p.OnLineTestNum) == 0 ? "0" : (Convert.ToDecimal(zongsuo.Sum(p => p.IsPassNum)) / Convert.ToDecimal(zongsuo.Sum(p => p.OnLineTestNum))).ToString("p")
                    , ReportCommon.CalculateMedian(zongsuo.Where(p => p.OnLineTestNum > 0).Select(p => p.AvgAnswerTimeMin).ToList())
                    , ReportCommon.CalculateMedian(zongsuo.Where(p => p.OnLineTestNum > 0).Select(p => p.AvgAnswerNum).ToList())
                    , ReportCommon.CalculateMedian(zongsuo.Where(p => p.OnLineTestNum > 0).Select(p => p.Avg).ToList()));
                int count = 1;
                foreach (var item in zongsuo)
                {
                    dt.Rows.Add(count++
                   , item.DeptTwoName
                   , item.YingCanJia == 0 ? "0" : (Convert.ToDecimal(item.OnLineTestNum) / Convert.ToDecimal(item.YingCanJia)).ToString("p")
                   , item.OnLineTestNum
                   , item.YingCanJia
                   , item.IsPassNum
                   , item.OnLineTestNum == 0 ? "0" : (Convert.ToDecimal(item.IsPassNum) / Convert.ToDecimal(item.OnLineTestNum)).ToString("p")
                   , (item.OnLineTestNum == 0 ? "N/A" : item.AvgAnswerTimeMin.ToString())
                   , (item.OnLineTestNum == 0 ? "N/A" : item.AvgAnswerNum)
                   , (item.OnLineTestNum == 0 ? "N/A" : item.Avg));
                }
            }

            var fensuo = list.Where(p => p.DeptName.Contains("分所")).ToList();
            if (fensuo.Count() != 0)
            {
                dt.Rows.Add(""
                                   , "分所合计（" + fensuo.Count() + "）"
                                   , fensuo.Sum(p => p.YingCanJia) == 0 ? "0" : (Convert.ToDecimal(fensuo.Sum(p => p.OnLineTestNum)) / Convert.ToDecimal(fensuo.Sum(p => p.YingCanJia))).ToString("p")
                                   , fensuo.Sum(p => p.OnLineTestNum)
                                   , fensuo.Sum(p => p.YingCanJia)
                                   , fensuo.Sum(p => p.IsPassNum)
                                   , fensuo.Sum(p => p.OnLineTestNum) == 0 ? "0" : (Convert.ToDecimal(fensuo.Sum(p => p.IsPassNum)) / Convert.ToDecimal(fensuo.Sum(p => p.OnLineTestNum))).ToString("p")
                                   , ReportCommon.CalculateMedian(fensuo.Where(p => p.OnLineTestNum > 0).Select(p => p.AvgAnswerTimeMin).ToList())
                                   , ReportCommon.CalculateMedian(fensuo.Where(p => p.OnLineTestNum > 0).Select(p => p.AvgAnswerNum).ToList())
                                   , ReportCommon.CalculateMedian(fensuo.Where(p => p.OnLineTestNum > 0).Select(p => p.Avg).ToList()));
                int count = 1;
                foreach (var item in fensuo)
                {
                    dt.Rows.Add(count++
                   , item.DeptTwoName
                   , item.YingCanJia == 0 ? "0" : (Convert.ToDecimal(item.OnLineTestNum) / Convert.ToDecimal(item.YingCanJia)).ToString("p")
                   , item.OnLineTestNum
                   , item.YingCanJia
                   , item.IsPassNum
                   , item.OnLineTestNum == 0 ? "0" : (Convert.ToDecimal(item.IsPassNum) / Convert.ToDecimal(item.OnLineTestNum)).ToString("p")
                   , (item.OnLineTestNum == 0 ? "N/A" : item.AvgAnswerTimeMin.ToString())
                   , (item.OnLineTestNum == 0 ? "N/A" : item.AvgAnswerNum)
                   , (item.OnLineTestNum == 0 ? "N/A" : item.Avg));
                }
            }
            #endregion

            //var dtList = new List<DataTable>();
            //string strFileName = "总所在线测试情况";
            //dtList.Add(dt);
            //var listchart = new List<LiXinModels.ChartModel>();



            //var export = new Spreadsheet();
            //export.ExportChart(listchart, dtList, HttpContext, strFileName);

            var value = type == 0 ? "全所" : "分所";
            SheetModel model = new SheetModel();

            model.SheetName = "视频课程在线测试情况" + value;
            model.space = 2;

            var datalist = new List<DataModel>();

            var data = new DataModel();
            data.datatype = Datatype.DataTable;
            data.Dataseries = GetCourseTable(courseid, type);
            datalist.Add(data);

            var data1 = new DataModel();
            data1.datatype = Datatype.DataTable;
            data1.Dataseries = dt;
            datalist.Add(data1);

            if (type == 0)
            {
                if (fensuo.Count > 0)
                {
                    //listchart.Add(testpie);
                    datalist.Add(Spreadsheet.ChangChartModelToDataModel(testpie));
                }
                if (zongsuo.Count > 0)
                {
                    datalist.Add(Spreadsheet.ChangChartModelToDataModel(testcolumn));
                }
            }

            model.DataModels = datalist;

            var sheetmodelList = new List<SheetModel>();
            sheetmodelList.Add(model);
            new Spreadsheet().ExportExcel(sheetmodelList, HttpContext, "视频课程在线测试情况" + value);
        }

        #endregion




    }
}
