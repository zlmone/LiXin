using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;
using System.Web.Mvc;
using LiXinInterface.Report_Together;
using LiXinModels.Report_Together;
using System.Data;
using System.Text.RegularExpressions;
using System.Diagnostics;
using LiXinModels;

namespace LiXinControllers
{
    public partial class Report_TogetherController : BaseController
    {
        private static List<ReportTogether> AllList;
        private static List<ReportTogether> fenList;
        private static List<ReportTogether> zongList;
        private static List<ReportTogether> CourseInfoList;
        private static List<SingleTogtherSurvey> SingleTogtherSurveyList;
        private static List<SingleTogtherSurvey> SinglezongList;
        private static List<SingleTogtherSurvey> SinglefengList;
        private static bool IsIncourseDate;

        #region 呈现
        public ActionResult CourseList()
        {
            return View();
        }
        public ActionResult CourseJoin()
        {
            return View();
        }

        /// <summary>
        /// 所有课程的综合统计
        /// </summary>
        /// <param name="type">0 正常加载  1返回 要记住滴</param>
        /// <returns></returns>
        public ActionResult CourseAll(int type = 0)
        {
            ViewBag.type = type;
            ViewBag.pageInex = 1;
            ViewBag.allTraGrade = AllTrainGrade;
            ViewBag.year = DateTime.Now.Year;
            ViewBag.LevelType = 0;

            if (type == 1)
            {
                if (Session["together_CourseAll"] != null)
                {
                    // = pageIndex + "㉿" + year + "㉿" + courseName + "㉿" + teacherName + "㉿" + IsMUst + "㉿" + IsCPA + "㉿" + IsTest+ payGrade;
                    //+LevelType + "㉿" + openLevel;
                    string sess = Session["together_CourseAll"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.pageInex = att[0];
                    ViewBag.year = att[1] == "" ? DateTime.Now.Year.ToString() : att[1];
                    ViewBag.courseName = att[2] == "" ? "请输入搜索内容" : att[2];
                    ViewBag.teacherName = att[3] == "" ? "请输入搜索内容" : att[3];
                    ViewBag.IsMUst = att[4];
                    ViewBag.IsCPA = att[5];
                    ViewBag.IsTest = att[6];
                    ViewBag.payGrade = att[7];
                    ViewBag.LevelType = att[8];
                    ViewBag.openLevel = att[9];
                }
            }
            return View();
        }



        public ActionResult SingleTogether(int courseID)
        {
            ViewBag.courseID = courseID;
            return View();
        }

        public ActionResult SingleTogetherSurvey(int courseID, int cp = 0)
        {
            ViewBag.courseID = courseID;

            ViewBag.deptid = "";
            ViewBag.StartTime = "";
            ViewBag.EndTime = "";
            ViewBag.cp = cp;
            if (cp == 1)
            {
                if (Session["together_TogtherSurvey"] != null)
                {
                    string sess = Session["together_TogtherSurvey"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.deptid = att[0];
                    ViewBag.StartTime = att[1];
                    ViewBag.EndTime = att[2];
                }
            }
            return View();
        }
        #endregion

        #region 所有课程的参与、贡献度情况
        /// <summary>
        /// 所有课程的参与、贡献度情况
        /// </summary>
        public JsonResult GetTogetherList(int year, string deptIDs = "", string jsRenderSortField = " JoinRate desc")
        {
            try
            {
                var freeConfig = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == year).FirstOrDefault();
                AllList = TogetherBL.GetTogetherList(year, freeConfig, deptIDs);


                fenList = AllList.Where(p => p.IsMain == 1).ToList().SortListByField(jsRenderSortField);
                zongList = AllList.Where(p => p.IsMain == 0).ToList().SortListByField(jsRenderSortField);

                var all = GetObjectForCPA(AllList, 0);
                var zong = GetObjectForCPA(zongList, 1);
                var fen = GetObjectForCPA(fenList, 2);


                return Json(new
                {
                    result = 1,
                    all = all,
                    zong = zong,
                    fen = fen,
                    fenList = fenList,
                    zongList = zongList
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<ReportTogether>()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 取出所有课程的参与、贡献度情况的集合 全部 总所 分所
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        public dynamic GetObjectForCPA(List<ReportTogether> List, int type = 0)
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

                var allscore = 0;
                //var UserScore = List.FirstOrDefault().UserScoreList;
                //foreach (var item in allscore)
                //{
                //    UserScore.AddRange(item);
                //}
                var allSum = new
                {
                    allNumer = string.Format(title, List.Count()),
                    allApplyRateStr = ReportCommon.CalculateMedianDouble(List.Select(p => p.ApplyRate).ToList(), 4).ToString("p"),
                    allJoinRateStr = ReportCommon.CalculateMedianDouble(List.Select(p => p.JoinRate).ToList(), 4).ToString("p"),
                    allGetScore = ReportCommon.CalculateMedianDouble(List.Select(p => p.GetScore).ToList(), 4).ToString("0.00"),
                    allSurveyRateStr = ReportCommon.CalculateMedianDouble(List.Select(p => p.SurveyRate).ToList(), 4).ToString("p"),
                    allExamJoinRateStr = ReportCommon.CalculateMedianDouble(List.Select(p => p.ExamJoinRate).ToList(), 4).ToString("p"),
                    allExamPassRateStr = ReportCommon.CalculateMedianDouble(List.Select(p => p.ExamPassRate).ToList(), 4).ToString("p"),
                    type = type,
                };
                return allSum;
            }
            else
            {
                var allSum = new
                {
                    allNumer = "全所合计0个",
                    allApplyRateStr = "0%",
                    allJoinRateStr = "0%",
                    allGetScore = 0,
                    allSurveyRateStr = "0%",
                    allExamJoinRateStr = "0%",
                    allExamPassRateStr = "0%"
                };
                return allSum;
            }
        }

        /// <summary>
        /// 所有课程的参与、贡献度情况
        /// </summary>
        public void outTogetherJoinScore(int start)
        {

            var list = AllList;
            var all = GetObjectForCPA(AllList, 0);

            //总所合计
            var sum = GetObjectForCPA(zongList, 1);

            //分所合计
            var fen = GetObjectForCPA(fenList, 2);

            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("部门/分所", typeof(string));
            outTable.Columns.Add("课程报名率", typeof(string));
            outTable.Columns.Add("课程参与率", typeof(string));
            outTable.Columns.Add("人均获取学时", typeof(string));
            outTable.Columns.Add("课程评估参与率", typeof(string));
            outTable.Columns.Add("测试参与率", typeof(string));
            outTable.Columns.Add("测试通过率", typeof(string));
            var index = 1;
            outTable.Rows.Add("", all.allNumer, all.allApplyRateStr, all.allJoinRateStr, all.allGetScore, all.allSurveyRateStr, all.allExamJoinRateStr, all.allExamPassRateStr);
            if (zongList.Count > 0)
            {
                outTable.Rows.Add("", sum.allNumer, sum.allApplyRateStr, sum.allJoinRateStr, sum.allGetScore, sum.allSurveyRateStr, sum.allExamJoinRateStr, sum.allExamPassRateStr);
                foreach (var item in zongList)
                {
                    outTable.Rows.Add(index, item.DeptName, item.ApplyRateStr, item.JoinRateStr, item.GetScoreStr, item.SurveyRateStr, item.ExamJoinRateStr, item.ExamPassRateStr);
                    index++;
                }
            }
            if (fenList.Count > 0)
            {
                outTable.Rows.Add("", fen.allNumer, fen.allApplyRateStr, fen.allJoinRateStr, fen.allGetScore, fen.allSurveyRateStr, fen.allExamJoinRateStr, fen.allExamPassRateStr);
                foreach (var item in fenList)
                {
                    outTable.Rows.Add(index, item.DeptName, item.ApplyRateStr, item.JoinRateStr, item.GetScoreStr, item.SurveyRateStr, item.ExamJoinRateStr, item.ExamPassRateStr);
                    index++;
                }
            }

            var title = start + "年全年课程参与、贡献度情况表";
            new Spreadsheet().Template(title, null, outTable, HttpContext, title, title);

        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        public void Refresh(int year=-1)
        {
            RefreshGetTogetherUser(year);
            RefreshGetCourseNumber();

        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        public void RefreshCourseInfo()
        {
            RefreshGetCacheCourseInfoList();
        }
        #endregion

        #region 所有课程的综合统计
        /// <summary>
        /// 所有课程的综合统计
        /// </summary>
        public JsonResult GetCourseInfoList(int year, string courseName = "", string teacherName = "", int IsMUst = -1, int IsCPA = -1, int IsTest = -1, string payGrade = "",
            string openLevel = "", int IsDelete = 0, int pageSize = 3, int pageIndex = 1, string jsRenderSortField = " Survey_AllScoreDouble desc", int LevelType = 0)
        {
            try
            {

                if (Session["together_CourseAll"] != null)
                {
                    Session.Remove("together_CourseAll");
                }
                Session["together_CourseAll"] = pageIndex + "㉿" + year + "㉿" + courseName + "㉿" + teacherName + "㉿" + IsMUst + "㉿" + IsCPA + "㉿" + IsTest
                    + "㉿" + payGrade + "㉿" + LevelType + "㉿" + openLevel;

                string where = "1=1";

                var freeConfig = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == year).FirstOrDefault();

                CourseInfoList = TogetherBL.GetCourseInfoList(year,freeConfig, AllTrainGrade, courseName, teacherName, IsMUst, IsCPA, IsTest, payGrade, openLevel, IsDelete, LevelType);
                CourseInfoList = CourseInfoList.SortListByField(jsRenderSortField);

                var allreallyApply = CourseInfoList.Sum(p => p.reallyApply);
                var allreallyJoin = CourseInfoList.Sum(p => p.reallyJoin);
                var allsurveyNumber = CourseInfoList.Where(p => p.surveyNumber >= 0).Sum(p => p.surveyNumber);
                var allNOSurvey = CourseInfoList.Where(p => p.NOSurvey >= 0).Sum(p => p.NOSurvey);
                var allSurvey_AllScore = ReportCommon.CalculateMedianDouble(CourseInfoList.Where(p => p.Survey_AllScoreDouble >= 0).Select(p => p.Survey_AllScoreDouble).ToList());
                var allSurvey_CourseScore = ReportCommon.CalculateMedianDouble(CourseInfoList.Where(p => p.Survey_CourseScoreDouble >= 0).Select(p => p.Survey_CourseScoreDouble).ToList());
                var allSurvey_TeacherScore = ReportCommon.CalculateMedianDouble(CourseInfoList.Where(p => p.Survey_TeacherScoreDouble >= 0).Select(p => p.Survey_TeacherScoreDouble).ToList());

                var totalcount = CourseInfoList.Count();

                var list = CourseInfoList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalcount,
                    allreally = allreallyApply + "/" + allreallyJoin,
                    allsurvey = allsurveyNumber + "/" + allNOSurvey,
                    allSurvey_AllScore = allSurvey_AllScore.ToString("0.00"),
                    allSurvey_CourseScore = allSurvey_CourseScore.ToString("0.00"),
                    allSurvey_TeacherScore = allSurvey_TeacherScore.ToString("0.00")
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<ReportTogether>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 所有课程的综合统计
        /// </summary>
        public void outCourseInfoList(int start)
        {

            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("课程名称", typeof(string));
            outTable.Columns.Add("课程属性", typeof(string));
            outTable.Columns.Add("开课时间", typeof(string));
            outTable.Columns.Add("开课对象", typeof(string));
            outTable.Columns.Add("讲师", typeof(string));
            outTable.Columns.Add("薪酬级别/职务", typeof(string));
            outTable.Columns.Add("报名人数", typeof(string));
            outTable.Columns.Add("实际参与人数", typeof(string));
            outTable.Columns.Add("参与评估人数", typeof(string));
            outTable.Columns.Add("未参与评估人数", typeof(string));
            outTable.Columns.Add("综合评分（均值）", typeof(string));
            outTable.Columns.Add("课程评分（均值）", typeof(string));
            outTable.Columns.Add("讲师评分（均值）", typeof(string));

            var allreallyApply = CourseInfoList.Sum(p => p.reallyApply);
            var allreallyJoin = CourseInfoList.Sum(p => p.reallyJoin); ;
            var allsurveyNumber = CourseInfoList.Where(p => p.surveyNumber >= 0).Sum(p => p.surveyNumber); ;
            var allNOSurvey = CourseInfoList.Where(p => p.NOSurvey >= 0).Sum(p => p.NOSurvey); ;
            var allSurvey_AllScore = ReportCommon.CalculateMedianDouble(CourseInfoList.Where(p => p.Survey_AllScoreDouble >= 0).Select(p => p.Survey_AllScoreDouble).ToList());
            var allSurvey_CourseScore = ReportCommon.CalculateMedianDouble(CourseInfoList.Where(p => p.Survey_CourseScoreDouble >= 0).Select(p => p.Survey_CourseScoreDouble).ToList()); ;
            var allSurvey_TeacherScore = ReportCommon.CalculateMedianDouble(CourseInfoList.Where(p => p.Survey_TeacherScoreDouble >= 0).Select(p => p.Survey_TeacherScoreDouble).ToList()); ;

            outTable.Rows.Add("合计", "--", "--", "--", "--", "--", "--", allreallyApply, allreallyJoin, allsurveyNumber,
                allNOSurvey, allSurvey_AllScore.ToString("0.00"), allSurvey_CourseScore.ToString("0.00"), allSurvey_TeacherScore.ToString("0.00"));

            var index = 1;
            foreach (var item in CourseInfoList)
            {
                //  //单独为预定的时候
                //  if (item.IsOrder == 2)
                //  {
                //      var list = AllTrainGrade.Where(p => string.IsNullOrEmpty(item.OpenLevel) ? true : !item.OpenLevel.Contains(p)).ToList();
                //      var OpenLevel = list.Count() == 0 ? "" : string.Join(",", list);
                //      outTable.Rows.Add(index, item.courseName, item.IsMustStr, item.CourseTime, OpenLevel, item.teacher, item.PayGrade, item.reallyApply, item.reallyJoin, (item.surveyNumber == -1 ? "N/A" : item.surveyNumber.ToString()),
                //(item.NOSurvey == -1 ? "N/A" : item.NOSurvey.ToString()), item.Survey_AllScore, item.Survey_CourseScore, item.Survey_TeacherScore);
                //  }
                //  else
                //  {
                outTable.Rows.Add(index, item.courseName, item.IsMustStr, item.CourseTime, item.OpenLevel, item.teacher, item.PayGrade, item.reallyApply, item.reallyJoin, (item.surveyNumber == -1 ? "N/A" : item.surveyNumber.ToString()),
               (item.NOSurvey == -1 ? "N/A" : item.NOSurvey.ToString()), item.Survey_AllScore, item.Survey_CourseScore, item.Survey_TeacherScore);
                //}
                index++;
            }
            var title = start + "年所有课程的综合统计";
            new Spreadsheet().Template(title, null, outTable, HttpContext, title, title);
        }
        #endregion

        #region 员工单门课程评估、测试情况表
        /// <summary>
        /// 员工单门课程评估、测试情况表
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="SurveyPaperId"></param>
        /// <param name="isPing"></param>
        /// <param name="isTest"></param>
        /// <param name="deptids"></param>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <returns></returns>
        public JsonResult GetSingleTogtherSurvey(int courseID, string SurveyPaperId, int isPing, int isTest, string courseStarttime,
            string deptids = "", string sDate = "", string eDate = "", string jsRenderSortField = " SurveyJoinRate desc")
        {
            try
            {
                if (Session["together_TogtherSurvey"] != null)
                {
                    Session.Remove("together_TogtherSurvey");
                }
                Session["together_TogtherSurvey"] = deptids + "㉿" + sDate + "㉿" + eDate;

                //是否在课程结束时间之前,为 true的时候 考勤人数等都为N/A
                IsIncourseDate = false;
                if (!string.IsNullOrEmpty(eDate))
                {
                    if (Convert.ToDateTime(courseStarttime) > Convert.ToDateTime(eDate))
                    {
                        IsIncourseDate = true;
                    }
                }

                SingleTogtherSurveyList = TogetherBL.GetSingleTogtherSurvey(courseID, SurveyPaperId, isPing, isTest, courseStarttime, deptids, sDate, eDate);
                var all = GetTogtherSurvey(SingleTogtherSurveyList, isPing, isTest, IsIncourseDate, 0);

                SinglezongList = SingleTogtherSurveyList.Where(p => p.IsMain == 1).ToList().SortListByField(jsRenderSortField);
                var zong = GetTogtherSurvey(SinglezongList, isPing, isTest, IsIncourseDate, 1);
                SinglefengList = SingleTogtherSurveyList.Where(p => p.IsMain == 0).ToList().SortListByField(jsRenderSortField); ;
                var feng = GetTogtherSurvey(SinglefengList, isPing, isTest, IsIncourseDate, 2);
                return Json(new
                {
                    result = 1,
                    all = all,
                    zong = zong,
                    zongList = SinglezongList,
                    fengList = SinglefengList,
                    feng = feng
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    zlist = new List<SingleTogtherSurvey>()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 取出员工单门课程评估、测试情况表
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        public SingleTogtherSurvey GetTogtherSurvey(List<SingleTogtherSurvey> List, int isPing, int isTest, bool IsIncourseDate, int type = 0)
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
                var allSum = new SingleTogtherSurvey();
                allSum.DeptName = string.Format(title, List.Count);
                allSum.HaveJoin = List.Where(p => p.HaveJoin >= 0).Sum(p => p.HaveJoin);
                if (isPing == 0 || IsIncourseDate)
                {
                    allSum.HaveJoin = -1;
                    allSum.SurveyNumber = -1;
                    allSum.SurveyJoinRate = -1;
                    allSum.Survey_CourseScoreDouble = -1;
                    allSum.Survey_TeacherScoreDouble = -1;
                }
                else
                {

                    allSum.SurveyNumber = List.Where(p => p.SurveyNumber >= 0).Sum(p => p.SurveyNumber);
                    allSum.SurveyJoinRate = ReportCommon.CalculateMedianDouble(List.Where(p => p.SurveyJoinRate >= 0).Select(p => p.SurveyJoinRate).ToList(), 4);
                    allSum.Survey_CourseScoreDouble = List[0].SumSurvey_CourseScore;
                    allSum.Survey_TeacherScoreDouble = List[0].SumSurvey_TeacherScore;

                }

                if (isTest == 0 || IsIncourseDate)
                {
                    allSum.ExamJoin = -1;
                    allSum.ExamHaveJoin = -1;
                    allSum.ExamJoinRate = -1;
                    allSum.ExamPassRate = -1;
                    allSum.AnswerTime = -1;
                    allSum.avgAnswerTimes = -1;
                    allSum.avgExamScore = -1;
                }
                else
                {
                    allSum.ExamHaveJoin = List.Where(p => p.ExamHaveJoin >= 0).Sum(p => p.ExamHaveJoin);
                    allSum.ExamJoin = List.Where(p => p.ExamJoin >= 0).Sum(p => p.ExamJoin);
                    allSum.ExamJoinRate = ReportCommon.CalculateMedianDouble(List.Where(p => p.ExamJoinRate >= 0).Select(p => p.ExamJoinRate).ToList(), 4);
                    allSum.ExamPassRate = ReportCommon.CalculateMedianDouble(List.Where(p => p.ExamPassRate >= 0).Select(p => p.ExamPassRate).ToList(), 4);
                    allSum.AnswerTime = ReportCommon.CalculateMedianDouble(List.Where(p => p.AnswerTime >= 0).Select(p => p.AnswerTime).ToList(), 4);
                    allSum.avgAnswerTimes = ReportCommon.CalculateMedianDouble(List.Where(p => p.avgAnswerTimes >= 0).Select(p => p.avgAnswerTimes).ToList(), 4);
                    allSum.avgExamScore = ReportCommon.CalculateMedianDouble(List.Where(p => p.avgExamScore >= 0).Select(p => p.avgExamScore).ToList(), 4);
                }
                allSum.type = type;

                return allSum;
            }
            else
            {
                var allSum = new SingleTogtherSurvey();
                allSum.DeptName = "全所合计0个";
                return allSum;
            }
        }

        /// <summary>
        /// 所有课程的参与、贡献度情况
        /// </summary>
        public void outTogetherSurveyScore(int isPing, int isTest)
        {
            var index = 1;
            var list = SingleTogtherSurveyList;
            var all = GetTogtherSurvey(SingleTogtherSurveyList, isPing, isTest, IsIncourseDate, 0);
            var zong = GetTogtherSurvey(SinglezongList, isPing, isTest, IsIncourseDate, 1);
            var feng = GetTogtherSurvey(SinglefengList, isPing, isTest, IsIncourseDate, 2);
            StringBuilder html = new StringBuilder();
            html.Append(@"
<table><tr ><td colspan='12' style='text-align:center'>各部门/分所单门课程评估、测试情况表<td></tr> <table>
<table  class='tb' cellspacing='0' cellpadding='0' border='0'><thead>
               <tr>
                <th></th>
                <th></th>
                <th colspan='5' style='background-color: gray'>评估</th>
                <th colspan='7'>测试</th>
            </tr>
            <tr>
                <th>序号</th>
                <th>部门/分所</th>
                <th>应参加评估人数</th>
                <th>实际参加评估人数</th>
                <th>评估参与率</th>
                <th>课程评估均分</th>
                <th>讲师评估均分</th>
                <th>测试应参与人数</th>
                <th>测试实际参与人数</th>
                <th>测试参与率</th>
                <th>测试通过率</th>
                <th>平均答题时间</th>
                <th>平均答题次数</th>
                <th>平均得分</th>
            </tr>
            </thead>
            <tbody>");
            html.Append(surveyHtml(all, 0));
            if (SinglezongList.Any())
            {
                html.Append(surveyHtml(zong, 0));
                foreach (var item in SinglezongList)
                {
                    html.Append(surveyHtml(item, index));
                    index++;
                }
            }
            if (SinglefengList.Any())
            {
                html.Append(surveyHtml(feng, 0));
                foreach (var item in SinglefengList)
                {
                    html.Append(surveyHtml(item, index));
                    index++;
                }
            }
            html.Append("</tbody></table>");
            new Spreadsheet().ExportExcel_Html(html.ToString(), "SingleTogetherSurvey", HttpContext);
        }

        /// <summary>
        /// 返回需要的html
        /// </summary>
        /// <param name="model"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public StringBuilder surveyHtml(SingleTogtherSurvey model, int index)
        {
            var newstr = new StringBuilder();
            newstr.Append("<tr>");
            if (index > 0)
            {
                newstr.Append("<td>" + index + "</td>");
            }
            else
            {
                newstr.Append("<td></td>");
            }
            newstr.Append("<td>" + model.DeptName + "</td>");
            newstr.Append("<td>" + model.HaveJoinStr + "</td>");
            newstr.Append("<td>" + model.SurveyNumberStr + "</td>");
            newstr.Append("<td>" + model.SurveyJoinRateStr + "</td>");
            newstr.Append("<td>" + model.Survey_CourseScore + "</td>");
            newstr.Append("<td>" + model.Survey_TeacherScore + "</td>");
            newstr.Append("<td>" + model.ExamHaveJoinStr + "</td>");
            newstr.Append("<td>" + model.ExamJoinStr + "</td>");
            newstr.Append("<td>" + model.ExamJoinRateStr + "</td>");
            newstr.Append("<td>" + model.ExamPassRateStr + "</td>");
            newstr.Append("<td>" + model.AnswerTimeStr + "</td>");
            newstr.Append("<td>" + model.avgAnswerTimesStr + "</td>");
            newstr.Append("<td>" + model.avgExamScoreStr + "</td>");
            newstr.Append("</tr>");
            return newstr;
        }
        #endregion

    }
}
