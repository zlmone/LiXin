using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinControllers;
using LiXinInterface.Examination;
using LiXinModels;
using LiXinModels.Examination;
using LiXinModels.Examination.ShowModel;
using System.Diagnostics;

namespace LiXinExam.Controllers
{
    public class ExamReportController : BaseController
    {
        private readonly IExamReport _examReportBL;

        public ExamReportController(IExamReport examReportBL)
        {
            _examReportBL = examReportBL;
        }

        #region 考试参与通过率

        public ActionResult JoinAndPassExam()
        {
            return View();
        }

        public JsonResult GetJoinAndPassExam(string ExamName, string StartTime, string EndTime, int pageSize = 20,
                                             int pageIndex = 1)
        {

            List<RExamination> result = _examReportBL.GetJoinAndPassExamReport(ExamName, StartTime, EndTime);

            //result = result.Where(p => p.ExaminationTitle.Contains(ExamName) && p.ExamBeginTime > StartTime.StringToDate(0) && p.ExamBeginTime < EndTime.StringToDate(1)).ToList();
            //stop.Stop();
            //var time1 = stop.ElapsedMilliseconds;


            IOrderedEnumerable<RExamination> JoinResult = result.OrderByDescending(p => p.JoinRate);
            IOrderedEnumerable<RExamination> PassResult = result.OrderByDescending(p => p.PassRate);

            int totalCount = result.Count;
            result = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var charArray = new List<Series>();
            var passSeries = new Series
                {
                    name = "通过率",
                    data = new List<double>()
                };
            var joinSeries = new Series
                {
                    name = "参与率",
                    data = new List<double>()
                };
            var xAxis = new List<String>();



            foreach (RExamination item in result)
            {
                //xAxis.Add(item.ExaminationTitle.Length > 10 ? item.ExaminationTitle.Substring(0, 10) + "..." : item.ExaminationTitle);
                xAxis.Add(item.ExaminationTitle);
                passSeries.data.Add(item.PassRate);
                joinSeries.data.Add(item.JoinRate);
            }

            charArray.Add(joinSeries);
            charArray.Add(passSeries);
            var res = new ChartModel
                {
                    DivID = "JoinAndPassExamChart",
                    xAxis = xAxis,
                    series = charArray,
                    charttype = Charttype.Line,
                    yText = "参与率及通过率（%）",
                    title = "参与及通过情况统计"
                };
            return Json(new
                {
                    result = 1,
                    dataList = result,
                    recordCount = totalCount,
                    MaxPass = PassResult.FirstOrDefault(),
                    MinPass = PassResult.LastOrDefault(),
                    MaxJoin = JoinResult.FirstOrDefault(),
                    MinJoin = JoinResult.LastOrDefault(),
                    res
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportJoinAndPassExamChart(string ExamName, string StartTime, string EndTime)
        {
            List<RExamination> result = _examReportBL.GetJoinAndPassExamReport(ExamName, StartTime, EndTime);

            var charArray = new List<Series>();
            var passSeries = new Series
                {
                    name = "通过率",
                    data = new List<double>()
                };
            var joinSeries = new Series
                {
                    name = "参与率",
                    data = new List<double>()
                };
            var xAxis = new List<String>();

            var table = new DataTable();
            table.Columns.Add("考试名称");
            table.Columns.Add("考试时长（分钟）");
            table.Columns.Add("总分");
            table.Columns.Add("安排人数");
            table.Columns.Add("参与人数");
            table.Columns.Add("参与率");
            table.Columns.Add("通过人数");
            table.Columns.Add("通过率");

            foreach (RExamination item in result)
            {
                xAxis.Add(item.ExaminationTitle);
                passSeries.data.Add(item.PassRate);
                joinSeries.data.Add(item.JoinRate);
                table.Rows.Add(new object[]
                    {
                        item.ExaminationTitle,
                        item.ExamLength,
                        item.ExamPaperTotalScore,
                        item.TotalPerson,
                        item.JoinPerson,
                        item.JoinRate + "%",
                        item.PassPerson,
                        item.PassRate + "%"
                    });
            }

            charArray.Add(joinSeries);
            charArray.Add(passSeries);
            var res = new ChartModel
                {
                    DivID = "JoinAndPassExamChart",
                    xAxis = xAxis,
                    series = charArray,
                    charttype = Charttype.Line,
                    yText = "参与率及通过率（%）",
                    title = "参与及通过情况统计"
                };

            new Spreadsheet().ExportChart(new List<ChartModel> { res }, new List<DataTable> { table }, HttpContext);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 题型正确率统计  (只统计参加考试的，每个人的正确率的平均值)

        public ActionResult QuestionCorrectReport()
        {
            return View();
        }

        public JsonResult GetQuestionCorrectReport(string ExamName, string StartTime, string EndTime, int pageSize = 20,
                                                   int pageIndex = 1)
        {
            List<RExamination> result = _examReportBL.GetQuestionCorrectReport(ExamName, StartTime, EndTime);

            RExamination MaxRate = result.OrderByDescending(p => p.QuesTypeCorrectMaxRate).FirstOrDefault();
            RExamination MinRate = result.OrderBy(p => p.QuesTypeCorrectMinRate).FirstOrDefault();
            IOrderedEnumerable<RExamination> ave = result.OrderByDescending(p => p.QuesTypeCorrectAverageRate);

            int totalCount = result.Count;
            result = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var itemArray = new object[result.Count()];
            int n = 0;
            var charArray = new List<Series>();
            foreach (RExamination item in result)
            {
                var temp = new
                    {
                        item._id,
                        item.ExaminationTitle,
                        item.ExamLength,
                        ExamBeginTime = item.ExamBeginTime.ToLocalTime().ToString(),
                        ExamEndTime = item.ExamEndTime.ToLocalTime().ToString(),
                        item.TestTimes,
                        item.PassScore,
                        item.PercentFlag,
                        item.ExamPaperTotalScore,
                        danxuan =
                            item.QuesTypeCorrectRate[QuestionType.单选题.ToString()] == -1
                                ? "--"
                                : item.QuesTypeCorrectRate[QuestionType.单选题.ToString()].ToString() + "%",
                        duoxuan =
                            item.QuesTypeCorrectRate[QuestionType.多选题.ToString()] == -1
                                ? "--"
                                : item.QuesTypeCorrectRate[QuestionType.多选题.ToString()].ToString() + "%",
                        panduan =
                            item.QuesTypeCorrectRate[QuestionType.判断题.ToString()] == -1
                                ? "--"
                                : item.QuesTypeCorrectRate[QuestionType.判断题.ToString()].ToString() + "%",
                        qingjing =
                            item.QuesTypeCorrectRate[QuestionType.情景题.ToString()] == -1
                                ? "--"
                                : item.QuesTypeCorrectRate[QuestionType.情景题.ToString()].ToString() + "%",
                        tiankong =
                            item.QuesTypeCorrectRate[QuestionType.填空题.ToString()] == -1
                                ? "--"
                                : item.QuesTypeCorrectRate[QuestionType.填空题.ToString()].ToString() + "%",
                        wenda =
                            item.QuesTypeCorrectRate[QuestionType.问答题.ToString()] == -1
                                ? "--"
                                : item.QuesTypeCorrectRate[QuestionType.问答题.ToString()].ToString() + "%"
                    };
                itemArray[n] = temp;
                n++;

                charArray.Add(new Series
                    {
                        name = item.ExaminationTitle,
                        data = new List<double>
                            {
                                item.QuesTypeCorrectRate["单选题"] == -1 ? 0 : item.QuesTypeCorrectRate["单选题"],
                                item.QuesTypeCorrectRate["多选题"] == -1 ? 0 : item.QuesTypeCorrectRate["多选题"],
                                item.QuesTypeCorrectRate["判断题"] == -1 ? 0 : item.QuesTypeCorrectRate["判断题"],
                                item.QuesTypeCorrectRate["情景题"] == -1 ? 0 : item.QuesTypeCorrectRate["情景题"],
                                item.QuesTypeCorrectRate["填空题"] == -1 ? 0 : item.QuesTypeCorrectRate["填空题"],
                                item.QuesTypeCorrectRate["问答题"] == -1 ? 0 : item.QuesTypeCorrectRate["问答题"]
                            }
                    });
            }

            var xAxis = new List<String>
                {
                    "单选题",
                    "多选题",
                    "判断题",
                    "情景题",
                    "填空题",
                    "问答题"
                };
            var res = new ChartModel
                {
                    DivID = "QuestionCorrectChart",
                    xAxis = xAxis,
                    series = charArray,
                    charttype = Charttype.Line,
                    yText = "题型正确率（%）",
                    title = "题型正确率统计"
                };

            return Json(new
                {
                    result = 1,
                    dataList = itemArray.ToList(),
                    recordCount = totalCount,
                    MaxRate,
                    MinRate,
                    MaxAve = ave.FirstOrDefault(),
                    MinAve = ave.LastOrDefault(),
                    res
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportQuestionCorrectChart(string ExamName, string StartTime, string EndTime)
        {
            List<RExamination> result = _examReportBL.GetQuestionCorrectReport(ExamName, StartTime, EndTime);

            var charArray = new List<Series>();
            var xAxis = new List<String>
                {
                    "单选题",
                    "多选题",
                    "判断题",
                    "情景题",
                    "填空题",
                    "问答题"
                };

            var table = new DataTable();
            table.Columns.Add("考试名称");
            table.Columns.Add("考试时长（分钟）");
            table.Columns.Add("总分");
            table.Columns.Add("单选题");
            table.Columns.Add("多选题");
            table.Columns.Add("判断题");
            table.Columns.Add("情景题");
            table.Columns.Add("填空题");
            table.Columns.Add("问答题");

            foreach (RExamination item in result)
            {
                charArray.Add(new Series
                    {
                        name = item.ExaminationTitle,
                        data = new List<double>
                            {
                                item.QuesTypeCorrectRate["单选题"] == -1 ? 0 : item.QuesTypeCorrectRate["单选题"],
                                item.QuesTypeCorrectRate["多选题"] == -1 ? 0 : item.QuesTypeCorrectRate["多选题"],
                                item.QuesTypeCorrectRate["判断题"] == -1 ? 0 : item.QuesTypeCorrectRate["判断题"],
                                item.QuesTypeCorrectRate["情景题"] == -1 ? 0 : item.QuesTypeCorrectRate["情景题"],
                                item.QuesTypeCorrectRate["填空题"] == -1 ? 0 : item.QuesTypeCorrectRate["填空题"],
                                item.QuesTypeCorrectRate["问答题"] == -1 ? 0 : item.QuesTypeCorrectRate["问答题"]
                            }
                    });

                table.Rows.Add(new object[]
                    {
                        item.ExaminationTitle,
                        item.ExamLength,
                        item.ExamPaperTotalScore,
                        item.QuesTypeCorrectRate["单选题"] == -1 ? "--" : item.QuesTypeCorrectRate["单选题"] + "%",
                        item.QuesTypeCorrectRate["多选题"] == -1 ? "--" : item.QuesTypeCorrectRate["多选题"] + "%",
                        item.QuesTypeCorrectRate["判断题"] == -1 ? "--" : item.QuesTypeCorrectRate["判断题"] + "%",
                        item.QuesTypeCorrectRate["情景题"] == -1 ? "--" : item.QuesTypeCorrectRate["情景题"] + "%",
                        item.QuesTypeCorrectRate["填空题"] == -1 ? "--" : item.QuesTypeCorrectRate["填空题"] + "%",
                        item.QuesTypeCorrectRate["问答题"] == -1 ? "--" : item.QuesTypeCorrectRate["问答题"] + "%"
                    });
            }

            var res = new ChartModel
                {
                    DivID = "QuestionCorrectChart",
                    xAxis = xAxis,
                    series = charArray,
                    charttype = Charttype.Line,
                    yText = "题型正确率（%）",
                    title = "题型正确率统计"
                };

            new Spreadsheet().ExportChart(new List<ChartModel> { res }, new List<DataTable> { table }, HttpContext);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 题库正确率

        public ActionResult QuestionSortCorrectReport()
        {
            return View();
        }

        public JsonResult GetQuestionSortCorrectReport(string ExamName, string StartTime, string EndTime,
                                                       int pageSize = 20, int pageIndex = 1)
        {
            List<string> questionSort;
            List<RExamination> result = _examReportBL.GetQuestionSortCorrectReport(ExamName, StartTime, EndTime,
                                                                                   out questionSort);

            RExamination MaxRate = result.OrderByDescending(p => p.QuesSortCorrectMaxRate).FirstOrDefault();
            RExamination MinRate = result.OrderBy(p => p.QuesSortCorrectMinRate).FirstOrDefault();
            IOrderedEnumerable<RExamination> ave = result.OrderByDescending(p => p.QuesSortCorrectAverageRate);

            int totalCount = result.Count;
            result = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var charArray = new List<Series>();
            var xAxis = new List<String>();
            var head = new StringBuilder
                (@"<tr>
                            <th class='tl'>考试名称 [ 题库名: 正确率 ]</th>
                            <th class='Raster_8'>考试时长(分钟)</th>
                            <th class='Raster_5'>总分</th>");
            foreach (string sort in questionSort)
            {
                //head.Append("<th>" + sort + "</th>");
                //xAxis.Add(sort.Length > 10 ? sort.Substring(0, 10) + "..." : sort);
                xAxis.Add(sort);
            }
            head.Append(@"</tr>");

            var itemArray = new object[result.Count()];
            int n = 0;
            foreach (RExamination item in result)
            {
                var body = new StringBuilder(@"<tr>

                    <td title='" + item.ExaminationTitle + @"'><p class='c33'><strong>"
                                 + item.ExaminationTitle + @"</strong></p><p class='Info'>");


                var series = new Series
                    {
                        name = item.ExaminationTitle,
                        data = new List<double>()
                    };

                bool falg = false;

                foreach (string sort in questionSort)
                {
                    if (item.QuesSortCorrectRate[sort] != -1)
                    {
                        body.Append(sort + ": " + item.QuesSortCorrectRate[sort] + "%" + "<span>|</span>");
                        falg = true;
                    }
                    series.data.Add(item.QuesSortCorrectRate[sort] == -1 ? 0 : item.QuesSortCorrectRate[sort]);
                }

                body = falg ? body.Remove(body.Length - 14, 14) : body;

                body.Append("</p></td><td title='" + item.ExamLength + @"' class='tc'>" + item.ExamLength + @"</td>
                    <td title='" + (item.PercentFlag == 1 ? item.ExamPaperTotalScore : 100) + @"'class='tc'>"
                                 + (item.PercentFlag == 1 ? item.ExamPaperTotalScore : 100) + @"</td></tr>");


                //body.Append("</tr>");
                var temp = new
                    {
                        body = body.ToString()
                    };

                itemArray[n] = temp;
                n++;

                charArray.Add(series);
            }
            var res = new ChartModel
                {
                    DivID = "QuestionSortCorrectChart",
                    xAxis = xAxis,
                    series = charArray,
                    charttype = Charttype.Line,
                    yText = "题库正确率（%）",
                    title = "题库正确率统计"
                };
            return Json(new
                {
                    result = 1,
                    dataList = itemArray.ToList(),
                    recordCount = totalCount,
                    MaxRate,
                    MinRate,
                    MaxAve = ave.FirstOrDefault(),
                    MinAve = ave.LastOrDefault(),
                    head = head.ToString(),
                    res
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportQuestionSortCorrectChart(string ExamName, string StartTime, string EndTime)
        {
            List<string> questionSort;
            List<RExamination> result = _examReportBL.GetQuestionSortCorrectReport(ExamName, StartTime, EndTime,
                                                                                   out questionSort);
            var charArray = new List<Series>();
            var xAxis = new List<String>();
            var table = new DataTable();
            table.Columns.Add("考试名称");
            table.Columns.Add("考试时长（分钟）");
            table.Columns.Add("总分");
            foreach (string sort in questionSort)
            {
                xAxis.Add(sort);
                table.Columns.Add(sort);
            }
            foreach (RExamination item in result)
            {
                var series = new Series
                    {
                        name = item.ExaminationTitle,
                        data = new List<double>()
                    };
                var row = new List<object> { item.ExaminationTitle, item.ExamLength, item.ExamPaperTotalScore };

                foreach (string sort in questionSort)
                {
                    series.data.Add(item.QuesSortCorrectRate[sort] == -1 ? 0 : item.QuesSortCorrectRate[sort]);
                    row.Add(item.QuesSortCorrectRate[sort] == -1 ? "--" : item.QuesSortCorrectRate[sort] + "%");
                }
                charArray.Add(series);

                table.Rows.Add(row.ToArray());
            }
            var res = new ChartModel
                {
                    DivID = "QuestionSortCorrectChart",
                    xAxis = xAxis,
                    series = charArray,
                    charttype = Charttype.Line,
                    yText = "题库正确率（%）",
                    title = "题库正确率统计"
                };
            new Spreadsheet().ExportChart(new List<ChartModel> { res }, new List<DataTable> { table }, HttpContext);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 成绩分布

        public ActionResult ExaminationRecordsDistributionReport()
        {
            return View();
        }

        public JsonResult GetExaminationRecordsDistributionReport(string ExamName, string StartTime, string EndTime,
                                                                  int pageSize = 20, int pageIndex = 1)
        {
            int charId = 0;
            List<string> distribution;
            List<RExamination> result = _examReportBL.GetExaminationRecordsDistribution(ExamName, StartTime, EndTime,
                                                                                        out distribution);

            int totalCount = result.Count;
            result = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var head = new StringBuilder
                (@"<tr>
                            <th class='tl'>考试名称</th>
                            <th class='Raster_8'>考试时长(分钟)</th>
                            <th class='Raster_5'>总分</th>");
            foreach (string sort in distribution)
            {
                head.Append("<th class='Raster_3'>" + sort + "</th>");
            }
            head.Append("<th class='Raster_3'>操作</th>");
            head.Append(@"</tr>");

            var itemArray = new object[result.Count()];
            int n = 0;
            foreach (RExamination item in result)
            {
                if (charId == 0)
                    charId = item._id;
                var body = new StringBuilder(@"<tr>
                    <td title='" + item.ExaminationTitle + @"' class='c33'><strong>" + item.ExaminationTitle +
                                             @"</strong></td>
                    <td title='" + item.ExamLength + @"' class='tc'>" + item.ExamLength + @"</td>
                    <td title='" + (item.PercentFlag == 1 ? item.ExamPaperTotalScore : 100) + @"' class='tc'>"
                                 + (item.PercentFlag == 1 ? item.ExamPaperTotalScore : 100) + @"</td>");

                foreach (string d in distribution)
                {
                    body.Append("<td class='tc'>" + item.RecordsDistribution[d] + "</td>");
                }
                body.Append("<td class='tc'><a onclick='DrawChart(" + item._id + ")'>查看</a></td>");
                body.Append("</tr>");
                var temp = new
                    {
                        body = body.ToString()
                    };

                itemArray[n] = temp;
                n++;
            }

            return Json(new
                {
                    result = 1,
                    dataList = itemArray.ToList(),
                    recordCount = totalCount,
                    head = head.ToString(),
                    charId
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExaminationRecordsDistribution(int id)
        {
            List<string> distribution;
            RExamination result = _examReportBL.GetExaminationRecordsDistribution(id, out distribution);

            var itemArray = new List<pieSeries>();

            foreach (string d in distribution)
            {
                itemArray.Add(new pieSeries
                    {
                        name = d,
                        y = result.RecordsDistribution[d]
                    });
            }

            var res = new ChartModel
                {
                    DivID = "RecordsDistributionChart",
                    charttype = Charttype.Pie,
                    title = result == null ? "" : result.ExaminationTitle + "成绩分布统计",
                    pieseries = itemArray
                };

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportRecordsDistributionChart(string ExamName, string StartTime, string EndTime, int charId)
        {
            List<string> distribution;
            List<RExamination> result = _examReportBL.GetExaminationRecordsDistribution(ExamName, StartTime, EndTime,
                                                                                        out distribution);
            RExamination instance = result.FirstOrDefault(p => p._id == charId);

            var itemArray = new List<pieSeries>();
            var table = new DataTable();
            table.Columns.Add("考试名称");
            table.Columns.Add("考试时长（分钟）");
            table.Columns.Add("总分");
            foreach (string d in distribution)
            {
                table.Columns.Add(d);

                itemArray.Add(new pieSeries { name = d, y = instance.RecordsDistribution[d] });

            }

            foreach (RExamination item in result)
            {
                var row = new List<object> { item.ExaminationTitle, item.ExamLength, item.ExamPaperTotalScore };

                foreach (string d in distribution)
                {
                    row.Add(item.RecordsDistribution[d]);
                }

                table.Rows.Add(row.ToArray());
            }

            var res = new ChartModel
                {
                    DivID = "RecordsDistributionChart",
                    charttype = Charttype.Pie,
                    title = instance.ExaminationTitle + "成绩分布统计",
                    pieseries = itemArray
                };
            new Spreadsheet().ExportChart(new List<ChartModel> { res }, new List<DataTable> { table }, HttpContext);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 页面呈现

        /// <summary>
        ///     考试评估首页
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportIndex()
        {
            return View();
        }

        #endregion

        #region 成绩排名与答卷 (浩浩专用)

        public ActionResult GradeRank()
        {
            ViewData["listPost"] = AllPosts;
            ViewData["listDept"] = AllDepartments;
            return View();
        }

        /// <summary>
        ///     成绩与排名统计
        /// </summary>
        /// <param name="examname">考试姓名</param>
        /// <param name="deptID">部门</param>
        /// <param name="postID">岗位</param>
        /// <param name="jobnum">工号</param>
        /// <param name="realname">姓名</param>
        /// <param name="isPass">是否通过</param>
        /// <param name="pageSize">每页显示的数目</param>
        /// <param name="pageIndex">页数</param>
        /// <returns></returns>
        public JsonResult GetGradeRank(string examname = "", string deptcode = "", string postcode = "", string jobnum = "",
                                       string realname = "", int isPass = -1, int pageSize = 20, int pageIndex = 1,
                                       string startDate = "", string endDate = "")
        {
            List<ExamGradeRank> result = _examReportBL.GetGradeRank();
            var newlist = result.OrderBy(p => p.Rank).GroupBy(p => p.examinationTitle);
            var newResult = new List<ExamGradeRank>();
            foreach (var item in newlist)
            {
                foreach (var model in item)
                {
                    newResult.Add(model);
                }
            }

            //根据条件筛选
            newResult = newResult.Where(p => p.examinationTitle.Contains(examname)
                                       && p.deptcode.Contains(deptcode)
                                       && p.postcode.Contains(postcode)
                                       && p.jobnum.ToString().Contains(jobnum)
                                       && p.realname.Contains(realname)
                                       && (isPass == -1 || p.IsPass == isPass)).ToList();
            if (startDate != "")
            {
                    newResult = newResult.Where(p => p.EndDate >= Convert.ToDateTime(startDate)
                                           && p.EndDate < Convert.ToDateTime(endDate).AddDays(1)).ToList();
                
            }

            int totalCount = newResult.Count;
            newResult = newResult.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
                {
                    result = 1,
                    dataList = newResult,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExportGrandRank(string examname = "", string deptcode = "", string postcode = "", string jobnum = "",
                                          string realname = "", int isPass = -1, string startDate = "",
                                          string endDate = "")
        {
            List<ExamGradeRank> result = _examReportBL.GetGradeRank();
            //根据条件筛选
            result = result.Where(p => p.examinationTitle.Contains(examname)
                                       && p.deptcode.Contains(deptcode)
                                       && p.postcode.Contains(postcode)
                                       && p.jobnum.ToString().Contains(jobnum)
                                       && p.realname.Contains(realname)
                                       && (isPass == -1 || p.IsPass == isPass)).ToList();

            var table = new DataTable();
            table.Columns.Add("考试名称");
            table.Columns.Add("工号");
            table.Columns.Add("姓名");
            table.Columns.Add("岗位");
            table.Columns.Add("部门");
            table.Columns.Add("得分");
            table.Columns.Add("已答题数");
            table.Columns.Add("答对题数");
            table.Columns.Add("未答题数");
            table.Columns.Add("答错题数");
            table.Columns.Add("正确率");
            table.Columns.Add("错误率");
            table.Columns.Add("是否通过");
            table.Columns.Add("排名");
            foreach (ExamGradeRank item in result)
            {
                table.Rows.Add(new object[]
                    {
                        item.examinationTitle,
                        item.jobnum,
                        item.realname,
                        item.deptcode,
                        item.postcode,
                        item.sumSocre,
                        item.hasAnswerNumber,
                        item.CorrectAnswerNumber,
                        item.NotAnswerNumber,
                        item.WrongAnswerNumber,
                        item.CorrectRate,
                        item.WrongRate,
                        item.passState,
                        item.Rank
                    });
            }

            new Spreadsheet().ExportChart(new List<ChartModel>(), new List<DataTable> { table }, HttpContext);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}