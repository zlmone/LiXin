using LiXinInterface.Report_fVedio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using System.Data;
using LiXinModels.Report_Vedio;
using LiXinModels;
using LiXinInterface.Report_Vedio;

namespace LiXinControllers
{
    public partial class Report_fVedioController : BaseController
    {
        private static IReport_OnLineTest iIReport_OnLineTestBL;
        private static IReport ReportBL;
        private IReport_Vedio vedioBL;

        public Report_fVedioController(IReport_OnLineTest _iIReport_OnLineTestBL, IReport _ReportBL, IReport_Vedio _vedioBL)
        {
            iIReport_OnLineTestBL = _iIReport_OnLineTestBL;
            ReportBL = _ReportBL;
            vedioBL = _vedioBL;
        }

        #region 呈现
        public ActionResult Report_fOnLineTest(int courseid)
        {
            ViewBag.courseid = courseid;
            return View();
        }

        public ActionResult Repot_fCanYu(int courseid)
        {
            ViewBag.courseid = courseid;

            return View();
        }



        public ActionResult Report_Detail(int courseid, string deptid, string backurl = "", int type = 0)
        {
            ViewBag.courseid = courseid;
            ViewBag.deptid = deptid;
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.backurl = backurl == "" ? "" : backurl.TrimStart('[').TrimEnd(']');
            ViewBag.type = type;
            return View();
        }

        public ActionResult FReport_Detail(int courseid, string deptid, string backurl = "", int type = 0)
        {
            ViewBag.courseid = courseid;
            ViewBag.deptid = deptid;
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.backurl = backurl == "" ? "" : backurl.TrimStart('[').TrimEnd(']');
            ViewBag.type = type;
            return View("Report_Detail");
        }

        public ActionResult Report_fOnLineTestDetail(int courseid)
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.courseid = courseid;
            //int limit = 0;
            //var deptlist = GetAllSubDepartments(CurrentUser.DeptId.ToString());
            //string a = "";
            //for (int i = 0; i < deptlist.Count; i++)
            //{
            //    a += deptlist[i].DepartmentId + ",";
            //}
            //var list = iIReport_OnLineTestBL.GetOnLineTestDetail(out limit, courseid, "  (DeptId in (" + a.Substring(0, a.LastIndexOf(',')) + "))", 1, int.MaxValue, "order by DeptName desc");
            //int ALLLearnTimes = list.Sum(p => p.LearnTimes);
            //ViewBag.ALLLearnTimes = ALLLearnTimes;
            //ViewBag.ALLVedioTimes = list.Sum(p => p.VedioTime);
            //ViewBag.ALLExamNum = list.Sum(p =>Convert.ToInt32(p.ExamNum));

            //ViewBag.ALLExamScore = ReportCommon.CalculateMedian(list.Select(p => p.ExamScore).ToList());
            //ViewBag.ALLGetLength = ReportCommon.CalculateMedian(list.Select(p => p.GetLength).ToList());
            return View();
        }
        #endregion


        #region  点击部门跳转到子页面
        public JsonResult GetReport_Detail(int courseid, string deptid, string DeptName, string RealName, string trainGrade, 
            string sltStatus, string sltcpa,int isFree=-1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " TrainGrade desc")
        {
            int limit = 0;
            string sql = " UserId in (SELECT UserId FROM dbo.View_CheckUser)";
            //var deptlist = GetAllSubDepartments(CurrentUser.DeptId.ToString());
            //string a = "";
            //for (int i = 0; i < deptlist.Count; i++)
            //{
            //    a += deptlist[i].DepartmentId + ",";
            //}
            string a = string.Join(",", GetAllSubLeardDepartments());

            if (!string.IsNullOrEmpty(deptid))
            {
                sql += " and (DeptId in  (" + deptid + "))";
            }

            if (!string.IsNullOrEmpty(DeptName))
            {
                sql += " and deptname like '%" + DeptName.ReplaceSql() + "%'";
            }

            if (!string.IsNullOrEmpty(RealName))
            {
                sql += " and Realname like '%" + RealName.ReplaceSql() + "%'";
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

            var list = iIReport_OnLineTestBL.GetOnLineTestDetail(out limit, courseid, sql, (pageIndex - 1) * pageSize + 1, pageSize, "order by " + jsRenderSortField);

            //<option value="1">未参与</option>
            //<option value="2">考试中</option>
            //<option value="6">已通过</option>
            //<option value="4">未通过</option>

            //if (!string.IsNullOrEmpty(sltStatus) && sltStatus != "99")
            //{
            //    list = list.SortListByField(jsRenderSortField).Where(p => p.ExamStatus == sltStatus).ToList();
            //}
            //else
            //{
            //    list = list.SortListByField(jsRenderSortField);
            //}
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
                list = list.SortListByField(jsRenderSortField);
            }


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
            limit = list.Count();

            string ALLExamScore = ReportCommon.CalculateMedian(list.Select(p => p.ExamScore).ToList());
            string ALLGetLength = ReportCommon.CalculateMedian(list.Select(p => p.GetLength).ToList());



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
        /// 倒出总所人员明细
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
        public void GetfOnlineTestDetailForReport(int courseid, string deptid, string DeptName, string RealName, string trainGrade, string sltStatus, string sltcpa, int pageSize = 10, int pageIndex = 1)
        {
            int limit = 0;
            string sql = " UserId in (SELECT UserId FROM dbo.View_CheckUser)";
            //var deptlist = GetAllSubDepartments(CurrentUser.DeptId.ToString());
            //string a = "";
            //for (int i = 0; i < deptlist.Count; i++)
            //{
            //    a += deptlist[i].DepartmentId + ",";
            //}
            string a = string.Join(",", GetAllSubLeardDepartments());
            if (!string.IsNullOrEmpty(deptid))
            {
                sql += " and (DeptId  in  (" + deptid + "))";
            }

            if (!string.IsNullOrEmpty(DeptName))
            {
                sql += " and deptname like '%" + DeptName.ReplaceSql() + "%'";
            }

            if (!string.IsNullOrEmpty(RealName))
            {
                sql += " and Realname like '%" + RealName.ReplaceSql() + "%'";
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
            var list = iIReport_OnLineTestBL.GetOnLineTestDetail(out limit, courseid, sql, pageIndex, pageSize, "order by UserId desc");

            if (!string.IsNullOrEmpty(sltStatus) && sltStatus != "99")
            {
                //list = list.SortListByField(jsRenderSortField).Where(p => p.ExamStatus == Convert.ToInt32(sltStatus)).ToList();
                //list = list.Where(p => p.ExamStatus == Convert.ToInt32(sltStatus)).ToList();
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

            foreach (Report_OnLineTest item in list)
            {
                dt.Rows.Add(count++, item.DeptName, item.Realname, item.Username, item.TrainGrade, item.CPA, "'" + item.CPANo, item.LearnTimes,
                    Math.Round(item.VedioTime / 60, 0), item.StrExamStatus, item.ExamNum, item.ExamScore, item.ExamStatus == "0" ? "N/A" : item.GetLength.ToString());
            }

            var dtList = new List<DataTable>();
            string strFileName = "分所学习人员明细";
            dtList.Add(dt);
            var export = new Spreadsheet();
            export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, strFileName);

        }

        #endregion

    }
}
