using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinInterface.Report_CPA;
using LiXinBLL.Report_CPA;
using LiXinModels.Report_CPA;
using LiXinCommon;
using System.Data;
using LiXinModels.SystemManage;

namespace LiXinControllers
{
    public partial class Report_CPAController : BaseController
    {
        private static List<ReportCPA> AllcpaList = new List<ReportCPA>();
        private static List<ReportCPADetails> AllDetailscpaList = new List<ReportCPADetails>();
        private static List<string> zhouqilist = new List<string>();
        #region 呈现
        /// <summary>
        /// 执业CPA继续教育学时统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportCPAList()
        {
            zhouqilist = CPABL.CPAStartAndEndStr(AllSystemConfigs.Where(p => p.ConfigType == 8).FirstOrDefault());
            ViewBag.zhouqi = zhouqilist;
            //ViewBag.otherConfig = freeConfigBL.GetModel(" and Year=" + DateTime.Now.Year);
            //ViewBag.freeConfig = freeConfigBL.GetFree_ApplyConfig(" and Year=" + DateTime.Now.Year);
            return View();
        }
        #endregion

        #region 学时统计
        /// <summary>
        /// 学时统计
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCPAList(string DeptName = "", int start = -1, string RealName = "", int isJoinDaode = -1,
             int OtherID = -1, int FreeID = -1, int isOtherOrg = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " SumScore desc")
        {
            try
            {
                string where = " 1=1";
                string timeWhere = " 1=1";
                string freeWhere = " 1=1";
                if (!string.IsNullOrEmpty(DeptName))
                {
                    where += " and DeptName like '%" + DeptName.ReplaceSql() + "%'";
                }
                if (start != -1)
                {
                    timeWhere += " and YearPlan=" + start;
                }

                if (!string.IsNullOrEmpty(RealName))
                {
                    where += " and Realname like '%" + RealName.ReplaceSql() + "%'";
                }
                if (isJoinDaode != -1)
                {

                    if (isJoinDaode == 1)
                    {
                        where += " and isJoinDaode>0";
                    }
                    else
                    {
                        where += " and isJoinDaode=0";
                    }
                   
                }



                //部门自学最大学时
                var DeptMaxScoreConfig = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 14).ConfigValue;
                var deptMaxScore = DeptMaxScoreConfig.Split(';')[2].Split(',')[0].StringToInt32();

                AllcpaList = CPABL.GetCPAList(start, OtherID, FreeID, AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 8), deptMaxScore, where, timeWhere);
                if (isOtherOrg != -1)
                {
                    if (isOtherOrg == 1)
                    {
                        AllcpaList = AllcpaList.Where(p => p.OtherProjectScore > 0).ToList();
                    }
                    else
                    {
                        AllcpaList = AllcpaList.Where(p => p.OtherProjectScore == 0).ToList();
                    }
                }

                var totalcount = AllcpaList.Count();
                AllcpaList = AllcpaList.SortListByField(jsRenderSortField);
                var cpalist = AllcpaList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new
                {
                    result = 1,
                    dataList = cpalist,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<ReportCPA>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导出视频课程汇总统计(总所)
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void OutCPAList(int start, string zhouqiStr)
        {
            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("部门", typeof(string));
            outTable.Columns.Add("登录名", typeof(string));
            outTable.Columns.Add("姓名", typeof(string));
            outTable.Columns.Add("CPA编号", typeof(string));
            outTable.Columns.Add("所内折算CPA学时", typeof(string));
            outTable.Columns.Add("注协课程CPA学时", typeof(string));
            outTable.Columns.Add("是否参加职业道德培训", typeof(string));
            outTable.Columns.Add("其他培训项目", typeof(string));
            outTable.Columns.Add("其他培训学时", typeof(string));
            outTable.Columns.Add("可免培训项目", typeof(string));
            outTable.Columns.Add("可免培训学时", typeof(string));
            outTable.Columns.Add("其他有组织形式学时", typeof(string));
            outTable.Columns.Add("考试通过次数", typeof(string));
            outTable.Columns.Add("CPA学时合计", typeof(string));
            outTable.Columns.Add("培训周期学时合计", typeof(string));
            var index = 1;
            var zhouqi = "（周期：" + zhouqiStr + ")";
            foreach (var v in AllcpaList)
            {
                outTable.Rows.Add(index, v.DeptName, v.Username, v.Realname, "'" + v.CPANo, v.sumZheScore, v.sumCPAScore, v.isJoinDaodeStr,
                    v.OtherProject, v.OtherScore, v.FreeProject, v.FreeScore, v.OtherProjectScore, v.passNumber, v.SumScore, v.SumZhouqiScore);
                index++;
            }
            new Spreadsheet().Template(start + "年执业CPA继续教育学时统计" + zhouqi, null, outTable, HttpContext, "执业CPA继续教育学时统计", "执业CPA继续教育学时统计");
        }
        #endregion

        #region 学时明细
        /// <summary>
        /// 执业CPA继续教育学时明细
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCPADetailsList(string DeptName = "", int start = -1, string RealName = "",
            string courseName = "", string CPANO = "", int courseType = -1, int way = 999, int pageSize = 3, int pageIndex = 1, string jsRenderSortField = "SumScore asc")
        {
            try
            {
                string where = " 1=1";
                var yearwhere = " 1=1";
                #region 条件
                if (!string.IsNullOrEmpty(DeptName))
                {
                    where += " and DeptName like '%" + DeptName.ReplaceSql() + "%'";
                }
                if (start != -1)
                {
                    yearwhere += " and (YearPlan=" + start + " OR  YearPlan IS NULL)";
                }
                if (!string.IsNullOrEmpty(RealName))
                {
                    where += " and Realname like '%" + RealName.ReplaceSql() + "%'";
                }
                //if (!string.IsNullOrEmpty(courseName))
                //{
                //    where += " and CourseName like '%" + courseName.ReplaceSql() + "%'";
                //}
                if (!string.IsNullOrEmpty(CPANO))
                {
                    where += " and CPANo like '%" + CPANO.ReplaceSql() + "%'";
                }
                //if (courseType != -1)
                //{
                //    where += " and courseType=" + courseType;
                //}
                //if (way != 999)
                //{
                //    where += " and Way=" + way;
                //}
                #endregion

                //部门自学最大学时
                var DeptMaxScoreConfig = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 14).ConfigValue;
                var deptMaxScore = DeptMaxScoreConfig.Split(';')[2].Split(',')[0].StringToInt32();

                AllDetailscpaList = CPABL.GetCPADetailsList(start, deptMaxScore, where, yearwhere);
                #region 查询
                var listShow = new List<ReportCPADetails>();

                // ReportDetailsList.ForEach(p => p.CPADetailsList = p.CPADetailsList.Where(t => t.Way == way).ToList());
                if (way != 999 || courseType != -1 || !string.IsNullOrEmpty(courseName))
                {
                    foreach (var item in AllDetailscpaList)
                    {
                        item.CPADetailsList = item.CPADetailsList
                            .Where(p => ((way == 999) || p.Way == way)
                                  && (courseType == -1 || p.courseType == courseType)
                                  && (string.IsNullOrEmpty(courseName) || p.CourseName.ToLower().Contains(courseName.ToLower()))
                                  ).ToList();
                        item.rowspan = item.CPADetailsList.Count();

                    }
                    foreach (var item in AllDetailscpaList)
                    {
                        if (item.CPADetailsList.Count() > 0)
                        {
                            listShow.Add(item);
                        }
                    }


                    AllDetailscpaList = listShow;
                }


                #endregion
                var totalcount = AllDetailscpaList.Count();
                if (jsRenderSortField.Contains("SumScore") || jsRenderSortField.Contains("Realname") || jsRenderSortField.Contains("DeptName"))
                {
                    AllDetailscpaList = AllDetailscpaList.SortListByField(jsRenderSortField);
                }
                else
                {
                    foreach (var item in AllDetailscpaList)
                    {
                        item.CPADetailsList = item.CPADetailsList.SortListByField(jsRenderSortField);
                    }

                    // AllDetailscpaList.ForEach(p => p.CPADetailsList.SortListByField(jsRenderSortField));
                }
                var cpalist = AllDetailscpaList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return Json(new
                {
                    result = 1,
                    dataList = cpalist,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<ReportCPADetails>(),
                    recordCount = 0,
                    ex = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导出执业CPA继续教育学时明细
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void OutCPADetailsList(int start)
        {
            var zhouqi = "（周期：" + zhouqilist.FirstOrDefault() + ")";
            var index = 0;
            // for (var i = 0; i < AllDetailscpaList; i++)
            StringBuilder html = new StringBuilder();
            html.AppendFormat(@"
<table><tr ><td colspan='12' style='text-align:center'>{1}年执业CPA继续教育学时明细(周期：{0})<td></tr> <table>
<table  class='tb' cellspacing='0' cellpadding='0' border='0'><thead>
                <tr>
                    <th>序号</th>
                    <th>部门</th>
                    <th>登录名</th>
                    <th>姓名</th>
                    <th>CPA编号</th>
                    <th>课程类型</th>
                    <th>培训形式</th>
                    <th>课程名称</th>
                    <th>培训开始日期</th>
                    <th>培训结束日期</th>
                    <th>学时</th>
                    <th>总学时</th>
                </tr>
            </thead>
            <tbody>", zhouqilist.FirstOrDefault(), start);
            foreach (var v in AllDetailscpaList)
            {
                index++;
                var span = v.rowspan;
                for (var j = 0; j < v.CPADetailsList.Count; j++)
                // foreach(var item in v.CPADetailsList)
                {
                    html.Append("<tr  class='child'>");
                    if (j == 0)
                    {
                        html.Append("<td rowspan=" + span + ">" + index + "</td>");
                        html.Append("<td rowspan=" + span + "><div class='ovh tl span15' title='" + v.DeptName + "'  style='cursor:pointer'>" + v.DeptName + "</div></td>");
                        html.Append("<td rowspan=" + span + ">" + v.Username + "</td>");
                        html.Append("<td rowspan=" + span + ">" + v.Realname + "</td>");
                        html.Append("<td rowspan=" + span + " style='vnd.ms-excel.numberformat:@'>" + v.CPANo + "</td>");
                        html.Append(" <td>" + v.CPADetailsList[j].courseTypeStr + "</td>");
                        html.Append(" <td>" + v.CPADetailsList[j].WayStr + "</td>");
                        html.Append(" <td><div class='ovh tl span15' title='" + v.CPADetailsList[j].CourseName + "'  style='cursor:pointer'>" + v.CPADetailsList[j].CourseName + "</div></td>");
                        html.Append(" <td>" + v.CPADetailsList[j].StartTimeStr + "</td>");
                        html.Append(" <td>" + v.CPADetailsList[j].EndTimeStr + "</td>");
                        html.Append(" <td>" + v.CPADetailsList[j].singleScore + "</td>");
                        html.Append(" <td rowspan=" + span + ">" + v.SumScore + "</td>");
                    }
                    else
                    {
                        html.Append(" <td>" + v.CPADetailsList[j].courseTypeStr + "</td>");
                        html.Append(" <td>" + v.CPADetailsList[j].WayStr + "</td>");
                        html.Append(" <td><div class='ovh tl span15' title='" + v.CPADetailsList[j].CourseName + "'  style='cursor:pointer'>" + v.CPADetailsList[j].CourseName + "</div></td>");
                        html.Append(" <td>" + v.CPADetailsList[j].StartTimeStr + "</td>");
                        html.Append(" <td>" + v.CPADetailsList[j].EndTimeStr + "</td>");
                        html.Append(" <td>" + v.CPADetailsList[j].singleScore + "</td>");
                    }
                    html.Append("</tr>");
                }
            }
            html.Append("</tbody></table>");
            new Spreadsheet().ExportExcel_Html(html.ToString(), "ReportForCPADetails", HttpContext);

        }
        #endregion

        #region 绑定其他形式、免修
        /// <summary>
        /// 其他形式
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public JsonResult GetOtherItemList(int year)
        {
            try
            {
                var OtherList = freeConfigBL.GetFreeOtherList_New(" year=" + year);
                return Json(new { dataList = OtherList }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { dataList = new List<Free_OtherApplyConfig>() }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 免修
        /// </summary>
        public JsonResult GetFreeItemList(int year)
        {
            try
            {
                int totalCount = 0;
                var FreeList = freeConfigBL.GetFreeApplyList(out totalCount, " year=" + year);
                return Json(new { dataList = FreeList }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { dataList = new List<Free_ApplyConfig>() }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion


    }
}
