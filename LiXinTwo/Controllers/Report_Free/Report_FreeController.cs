﻿using LiXinModels;
using System;
﻿using LiXinInterface;
using LiXinInterface.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using System.Data;
using LiXinInterface.MyApproval;
using LiXinModels.SystemManage;

namespace LiXinControllers
{
    public partial class Report_FreeController : BaseController
    {
        private static List<ReportFreeDetails> FreeDetailsList;
        private static List<ReportFreeDetails> otherFreeDetailsList;
        protected IFreeConfig freeConfigBL;
        protected IReport_Free reportFreeBL;
        protected IFree_OtherFroms free_OtherFromsBL;
        protected IFree_UserApplyOtherForm free_UserApplyOtherFormBL;
        protected IFree_UserOtherApply free_UserApplyBL;
        protected IFree freeBL;
        public Report_FreeController(IFreeConfig _freeConfigBL, IReport_Free _reportFreeBL, IFree_OtherFroms _free_OtherFromsBL, IFree_UserApplyOtherForm _free_UserApplyOtherFormBL,
            IFree_UserOtherApply _free_UserApplyBL, IFree _freeBL)
        {
            freeConfigBL = _freeConfigBL;
            reportFreeBL = _reportFreeBL;
            free_OtherFromsBL = _free_OtherFromsBL;
            free_UserApplyOtherFormBL = _free_UserApplyOtherFormBL;
            free_UserApplyBL = _free_UserApplyBL;
            freeBL = _freeBL;
        }



        public ActionResult ReportFreeIndex()
        {
            return View();
        }

        #region 授课人
        public ActionResult ReportOtherMain()
        {

            return View();
        }

        /// <summary>
        /// 授课人
        /// </summary>
        public ActionResult ReportTeacher()
        {
            ViewBag.yearList = freeBL.GetYearList();
            ViewBag.allTrainGrade = AllTrainGrade;
            return View();
        }



        public ActionResult SelectOtherItem()
        {
            return View();
        }

        /// <summary>
        /// 获取授课人
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCPATeacherList(int year, string realName = "", string Class = "", string CPANO = "", string TrainGrade = "", string deptids = ""
            , int typeForm = -1, int cpa = 1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " ConvertScore asc")
        {
            try
            {
                var where = "1=1";

                var teacherWhere = "  1=1";

                #region 条件
                if (year != 0)
                {
                    where += " And Year=" + year;
                    // teacherWhere += " And Year=" + year;
                }
                if (!string.IsNullOrEmpty(Class))
                {
                    where += " and ClassName like '%" + Class.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(realName))
                {
                    where += "  AND Realname LIKE '%" + realName.ReplaceSql() + "%'";
                }

                if (!string.IsNullOrEmpty(CPANO))
                {
                    where += "  AND CPANo LIKE '%" + CPANO.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(TrainGrade))
                {
                    where += " AND charindex(syu.TrainGrade,'" + TrainGrade + "')>0";
                }
                if (!string.IsNullOrEmpty(deptids))
                {
                    where += " and DeptId in (" + deptids + ")";
                }
                if (typeForm != -1)
                {
                    where += " AND typeForm=" + typeForm;
                }

                #endregion

                var totalCount = 0;
                var list = reportFreeBL.GetCPATeacherList(where, cpa).SortListByField(jsRenderSortField);
                var newlist = new List<ReportFreeDetails>();
                foreach (var item in list)
                {
                    if (!jsRenderSortField.Contains("DeptId") && !jsRenderSortField.Contains("totaltScore"))
                    {
                        item.FreeDetailsList = item.FreeDetailsList.SortListByField(jsRenderSortField);
                    }

                }

                totalCount = list.Count;
                FreeDetailsList = list.SortListByField(jsRenderSortField);
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return Json(new { result = 1, recordCount = totalCount, dataList = list }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, recordCount = 0, dataList = new List<ReportFreeDetails>() }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// 导出授课人信息
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void OutCPADetailsList(int cpa)
        {
            var index = 0;
            // for (var i = 0; i < AllDetailscpaList; i++)
            var title = cpa == 0 ? "我所执业CPA其他形式-授课人" : "我所非CPA其他形式-授课人";
            StringBuilder html = new StringBuilder();
            html.AppendFormat(@"
<table><tr ><td colspan='8' style='text-align:center'>{0}<td></tr> <table>
<table  class='tb' cellspacing='0' cellpadding='0' border='0'><thead>
                <tr>
                    <th>序号</th>
                    <th>姓名</th>
                    <th>登陆名</th>
                    <th>部门</th>
                    <th>培训级别</th>
                    {1}
                    <th>培训班名称</th>
                    <th>授课学时</th>
                    <th>{2}</th>
                    <th>学时合计</th>
                </tr>
            </thead>
            <tbody>", title, cpa == 0 ? "<th>CPA编号</th>" : "", cpa == 0 ? "可折算CPA学时" : "可折算所内学时");
            foreach (var v in FreeDetailsList)
            {
                index++;
                var span = v.rowspan;
                for (var j = 0; j < v.FreeDetailsList.Count; j++)
                {

                    html.Append("<tr  class='child'>");
                    if (j == 0)
                    {
                        html.Append("<td rowspan=" + span + ">" + index + "</td>");
                        html.Append("<td rowspan=" + span + "><div class='ovh tl span15' title='" + v.Realname + "'>" + v.Realname + "</div></td>");
                        html.Append("<td rowspan=" + span + ">" + v.Username + "</td>");
                        html.Append("<td rowspan=" + span + ">" + v.DeptName + "</td>");
                        html.Append("<td rowspan=" + span + ">" + v.TrainGrade + "</td>");
                        if (cpa == 0)
                        {
                            html.Append("<td rowspan=" + span + "  style='vnd.ms-excel.numberformat:@'>" + v.CPANo + "</td>");
                        }
                        html.Append(" <td>" + v.FreeDetailsList[j].ClassName + "</td>");
                        html.Append(" <td>" + v.FreeDetailsList[j].ConvertScore + "</td>");
                        if (cpa == 0)
                        {
                            html.Append(" <td>" + v.FreeDetailsList[j].GetCPAScore + "</td>");
                            html.Append("<td rowspan=" + span + ">" + v.totalCPAScore + "</td>");
                        }
                        else
                        {
                            html.Append(" <td>" + v.FreeDetailsList[j].GettScore + "</td>");
                            html.Append("<td rowspan=" + span + ">" + v.totaltScore + "</td>");
                        }
                    }
                    else
                    {
                        html.Append(" <td>" + v.FreeDetailsList[j].ClassName + "</td>");
                        html.Append(" <td>" + v.FreeDetailsList[j].ConvertScore + "</td>");
                        if (cpa == 0)
                        {
                            html.Append(" <td>" + v.FreeDetailsList[j].GetCPAScore + "</td>");
                        }
                        else
                        {
                            html.Append(" <td>" + v.FreeDetailsList[j].GettScore + "</td>");
                        }
                    }
                    html.Append("</tr>");
                }
            }
            html.Append("</tbody></table>");
            new Spreadsheet().ExportExcel_Html(html.ToString(), title, HttpContext);
        }

        #endregion

        #region 其他形式
        public ActionResult ReportOther()
        {
            ViewBag.allTrainGrade = AllTrainGrade;
            ViewBag.yearList = freeBL.GetYearList();
            return View();
        }

        public JsonResult Free_OtherFromAddList(string othername, int year = 0, int pageSize = 10, int pageIndex = 1, int type = 0)
        {
            int litme = 0;
            string sql = " ApplyType=1 and year=" + (year == 0 ? DateTime.Now.Year : year);
            if (!string.IsNullOrEmpty(othername))
            {
                sql += "AND ApplyContent LIKE '%" + othername.ReplaceSql() + "%'";
            }

            var list = freeConfigBL.GetFreeOtherList(out litme, where: sql);

            var model = new Free_OtherApplyConfig();
            model.ApplyContent = "课后评估奖励学时";
            model.ID = 0;
            list.Insert(0, model);

            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = litme
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取其他形式
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOtherCPAList(int year, string realName = "", string Class = "", string CPANO = "", string TrainGrade = "", string deptids = ""
            , int typeForm = -1, int cpa = 1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " DeptId asc")
        {
            try
            {
                var where = "1=1";

                #region 条件
                if (year != 0)
                {
                    where += " And Year=" + year;
                }
                if (!string.IsNullOrEmpty(realName))
                {
                    where += "  AND Realname LIKE '%" + realName.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(Class))
                {
                    var array = Class.Split(',');
                    if (array.Contains("0"))
                    {

                        where += " AND (typeForm=2";

                        if (array.Length > 1)
                        {
                            where += " or ApplyID in (" + Class + ")";
                        }
                        where += ")";
                    }
                    else
                    {
                        where += " and ApplyID in (" + Class + ")";
                    }

                }

                if (!string.IsNullOrEmpty(CPANO))
                {
                    where += "  AND CPANo LIKE '%" + CPANO.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(TrainGrade))
                {
                    where += " AND charindex(TrainGrade,'" + TrainGrade + "')>0";
                }
                if (!string.IsNullOrEmpty(deptids))
                {
                    where += " and DeptId in (" + deptids + ")";
                }
                if (typeForm != -1)
                {
                    where += " AND typeForm=" + typeForm;
                   
                }
                if (cpa != -1)
                {
                    //where += " AND syu.CPA=" + (cpa == 0 ? "'是'" : "'否'");
                    if (cpa == 0)
                    {
                        where += "  and CPA='是' and (ConvertType=0 or  ConvertType=2)";
                    }
                    else
                    {
                        where += " and (ConvertType=1 or  ConvertType=2)";
                    }
                }
                #endregion

                //自动折算
                // var OutApplyList = free_UserApplyBL.GetOutUserApply(surveyWhere + " and fuo.ApplyUserID=" + CurrentUser.UserId);

                var totalCount = 0;
                otherFreeDetailsList = reportFreeBL.GetOtherList(where: where, jsRenderSortField: jsRenderSortField);
                var list = otherFreeDetailsList.Skip((pageIndex - 1) * pageSize).Take(pageSize);

                totalCount = list.Count() == 0 ? 0 : list.FirstOrDefault().totalcount;
                return Json(new { result = 1, recordCount = totalCount, dataList = list }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, recordCount = 0, dataList = new List<ReportFreeDetails>() }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导出其他形式
        /// </summary>
        public void OutOtherCPAList(int cpa = 1, int pageSize = 10, int pageIndex = 1)
        {
            var where = "1=1";


            var title = cpa == 0 ? "我所执业CPA其他形式" : "我所非CPA其他形式";

            System.Data.DataTable outTable = new System.Data.DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("姓名", typeof(string));
            outTable.Columns.Add("登录名", typeof(string));
            outTable.Columns.Add("部门", typeof(string));
            outTable.Columns.Add("培训级别", typeof(string));
            if (cpa == 0)
            {
                outTable.Columns.Add("CPA编号", typeof(string));
            }
            outTable.Columns.Add("申请内容", typeof(string));
            if (cpa == 0)
            {
                outTable.Columns.Add("申请CPA学时", typeof(string));
            }
            else
            {
                outTable.Columns.Add("申请所内学时", typeof(string));
            }

            var index = 1;
            foreach (var v in otherFreeDetailsList)
            {
                // outTable.Rows.Add(index, v.DeptName, v.Username, v.Realname, "'" + v.CPANo, v.sumZheScore, v.sumCPAScore, v.isJoinDaodeStr, "", "", "", "", "", v.passNumber, v.SumScore, v.SumZhouqiScore);
                if (cpa == 0)
                {
                    outTable.Rows.Add(index, v.Realname, v.Username, v.DeptName, v.TrainGrade, "'" + v.CPANo, v.ClassNameStr, v.GetCPAScore);
                }
                else
                {
                    outTable.Rows.Add(index, v.Realname, v.Username, v.DeptName, v.TrainGrade, v.ClassNameStr, v.GettScore);
                }
                index++;
            }
            new Spreadsheet().Template(title, null, outTable, HttpContext, title, title);
        }
        #endregion
    }
}
