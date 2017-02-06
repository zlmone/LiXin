using LiXinDataAccess;
using LiXinDataAccess.Examination;
using LiXinInterface;
using LiXinModels.Examination.DBModel;
using LiXinModels.Report_Together;
using LiXinModels.Report_Vedio;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Retech.Cache;
using Retech.Core.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;
using System.Diagnostics;
using LiXinInterface.Report_Together;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using LiXinModels;

namespace LiXinBLL.Report_Together
{
    public class ReportTogetherBL : IReportTogether
    {
        private Report_TogetherDB togethetDB;
        protected ICacheManager cacheManager = new MemoryCacheManager();
        private ExaminationDB examDB;
        private Report_FreeDB freeDB;
        public ReportTogetherBL()
        {
            togethetDB = new Report_TogetherDB();
            examDB = new ExaminationDB();
            freeDB = new Report_FreeDB();
        }

        #region 所有课程的参与、贡献度情况

        /// <summary>
        /// 所有课程的参与、贡献度情况
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportTogether> GetTogetherList(int year, Sys_ParamConfig config, string deptIDs = "")
        {
            //评估的数据
            // var surveyList = togethetDB.GetSurveyUserCount();

            var newList = new List<ReportTogether>();
            var FreeList = new List<int>();
            #region 免修
            FreeList = freeDB.GetFreeUserList("  Year=" + year);
            if (!(config == null || config.ConfigValue == ""))
            {
                var configvalue = config.ConfigValue.Split(';');
                var tDate = year + "-" + configvalue[0].Split(',')[0];
                var CPADate = year + "-" + configvalue[1].Split(',')[0];
                var userList = freeDB.GetUserByDate(tDate, CPADate);
                FreeList.AddRange(userList.Select(p => p.ApplyUserID).Distinct());
            }
            var freeWhere = FreeList.Any() ? "  syu.UserId NOT IN (" + string.Join(",", FreeList) + ")" : " 1=1";
            #endregion

            #region 总所 集中授课

            var zwhere = " 1=1 ";

            if (!string.IsNullOrEmpty(deptIDs))
            {
                zwhere += " AND  syu.DeptId IN (" + deptIDs + ")";
            }
            //人员所有的学时
            var userscore = togethetDB.GetUserScore(year, freeWhere);

            //所有课程的属性
            var TogetherList = togethetDB.GetTogetherList(zwhere, freeWhere).Where(p => p.YearPlan == year && p.IsDelete == 0);

            var TogetherNumber = cacheManager.Get("together_number" + year, 1440, () => { return (new ReportTogetherBL()).GetTogetherUser(year); });
            var TogetherPass = cacheManager.Get("together_examnumber", 1440, () => { return (new ReportTogetherBL()).GetCourseNumber(); });
            //var passlist = examDB.GetAllList<tbExamSendStudent>(Query.In("SourceType", new BsonArray(new List<int>() { 4 })), 1, 10, "UserID", "RelationID", "Number");
            TogetherPass = TogetherPass.Where(p => p.YearPlan == year).ToList();
            var deptList = TogetherList.Select(p => p.DeptName).Distinct();

            //每门课程的应报名人数
            foreach (var item in TogetherList)
            {
                var list = TogetherNumber.Where(p => p.courseID == item.CourseId && p.DeptID == item.DeptId&&!FreeList.Contains(p.userID));
                item.haveApply = list.Count();
            }

            var dic = new Dictionary<string, decimal>();
            //按部门名字组装中位数
            foreach (var DeptName in userscore.Select(p => p.DeptName).Distinct())
            {
                var single = userscore.Where(p => p.DeptName == DeptName);
                var score = Convert.ToDecimal(ReportCommon.CalculateMedianDouble(single.Select(p => p.GetScore).ToList()));
                dic.Add(DeptName, score);
            }

            //组装数据
            foreach (var DeptName in deptList)
            {
                var model = new ReportTogether();
                var single = TogetherList.Where(p => p.DeptName == DeptName);

                if (single.Count() > 0)
                {
                    var deptIDList = single.Select(p => p.DeptId).Distinct();
                    //var deptID = single.FirstOrDefault().DeptId;
                    // var singlescore = userscore.Where(p => deptIDList.Contains(p.DeptId));
                    var pass = TogetherPass.Where(p => deptIDList.Contains(p.DeptId));
                    model.DeptName = single.FirstOrDefault().DeptName;
                    model.haveApply = single.Sum(p => p.haveApply);
                    model.reallyApply = single.Sum(p => p.reallyApply);
                    model.reallyJoin = single.Sum(p => p.reallyJoin);
                    model.HaveJoin = model.reallyJoin;
                    model.ExamJoin = pass.Any() ? pass.Sum(p => p.ExamJoin) : 0;
                    model.ExamPass = pass.Any() ? pass.Sum(p => p.ExamPass) : 0;
                    model.UserScoreList = userscore.Select(p => p.GetScore).ToList();
                    model.GetScore = dic.Keys.Contains(DeptName) ? dic[DeptName] : 0;
                    model.surveyNumber = single.Sum(p => p.surveyNumber);
                    model.SumsurveyNumber = single.Where(p => p.IsPing == 1).Sum(p => p.reallyJoin);
                    model.SumExamJoin = single.Where(p => p.IsTest == 1).Sum(p => p.reallyJoin);
                    model.IsMain = 0;
                    newList.Add(model);
                }
            }
            #endregion

            #region 分所 只算转播
            var fwhere = "1=1";
            if (year != -1)
            {
                fwhere += " and cc.YearPlan=" + year;
            }
            if (!string.IsNullOrEmpty(deptIDs))
            {
                fwhere += " AND syu.DeptId IN (" + deptIDs + ")";
            }
            var rtList = togethetDB.GetDepTranCourse(fwhere, freeWhere);

            var courseInfoList = rtList.Select(p => new { p.CourseId, p.haveApply, p.HaveJoin, p.reallyApply, p.reallyJoin, p.DepartSetId, p.surveyNumber, p.IsPing, p.IsTest }).Distinct();
            var userscoreList = rtList.Select(p => new { p.UserId, p.DeptId, p.GetSumScore }).Distinct();

            var userIDList = new List<ReportTogether>();
            foreach (var item in userscoreList)
            {
                if (userIDList.Where(p => p.UserId == item.UserId).Count() == 0)
                {
                    var model = new ReportTogether();
                    // userIDList.Add(item.UserId);
                    var single = userscoreList.Where(p => p.UserId == item.UserId);
                    model.GetSumScore = single.Sum(p => p.GetSumScore);
                    model.UserId = item.UserId;
                    model.DeptId = item.DeptId;
                    userIDList.Add(model);
                }
            }

            var fennewlistr = new List<string>();


            //组装数据
            foreach (var item in rtList)
            {
                if (fennewlistr.Count(p => p == item.DeptName) == 0)
                {
                    fennewlistr.Add(item.DeptName);
                    var model = new ReportTogether();

                    var single = rtList.Where(p => p.DeptName == item.DeptName);

                    var courseList = courseInfoList.Where(p => single.Select(pp => pp.DepartSetId).Contains(p.DepartSetId) && single.Select(pp => pp.CourseId).Contains(p.CourseId));
                    var userList = single.Select(p => p.UserId);
                    var deptIDList = single.Select(p => p.DeptId).Distinct();
                    var courseIDList = courseList.Select(p => p.CourseId);
                    var pass = TogetherPass.Where(p => deptIDList.Contains(p.DeptId));
                    var users = userIDList.Where(p => deptIDList.Contains(p.DeptId));
                    model.DeptName = item.DeptName;
                    model.DeptIds = string.Join(",", deptIDList);
                    model.haveApply = courseList.Sum(p => p.haveApply);
                    model.reallyApply = courseList.Sum(p => p.reallyApply);
                    model.reallyJoin = courseList.Sum(p => p.reallyJoin);
                    model.HaveJoin = model.reallyJoin;
                    model.ExamJoin = pass.Any() ? pass.Sum(p => p.ExamJoin) : 0;
                    model.ExamPass = pass.Any() ? pass.Sum(p => p.ExamPass) : 0;
                    model.GetScore = Convert.ToDecimal(ReportCommon.CalculateMedianDouble(users.Select(p => p.GetSumScore).ToList()));
                    model.IsOrder = 1;
                    model.IsMain = 1;
                    model.UserScoreList = rtList.Select(p => p.GetSumScore).ToList();
                    model.surveyNumber = courseList.Sum(p => p.surveyNumber);
                    model.SumsurveyNumber = courseList.Where(p => p.IsPing == 1).Sum(p => p.reallyJoin);
                    model.SumExamJoin = courseList.Where(p => p.IsTest == 1).Sum(p => p.reallyJoin);
                    newList.Add(model);
                }
            }
            #endregion

            return newList.ToList();
        }
        #endregion

        #region 所有课程的综合统计
        /// <summary>
        /// 所有课程的综合统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportTogether> GetCourseInfoList(int year, Sys_ParamConfig config, List<string> allTrainGrade, string courseName = "", string teacherName = "", int IsMUst = -1, int IsCPA = -1
            , int IsTest = -1, string payGrade = "", string openLevel = "", int IsDelete = 0, int LevelType = 0)
        {

            var newList = new List<ReportTogether>();
            var courseList = cacheManager.Get("together_CourseInfoList", 1440, () => { return (new ReportTogetherBL()).GetCacheCourseInfoList(); });
            var RtcourseList = togethetDB.GetRTCourseInfoList("  YearPlan=" +year);

            foreach (var item in courseList)
            {
                var single = RtcourseList.Where(p => p.CourseId == item.CourseId);
                var model = new ReportTogether();
                model.courseName = item.courseName;
                model.CourseId = item.CourseId;
                model.IsMust = item.IsMust;
                model.StartTime = item.StartTime;
                model.EndTime = item.EndTime;
                model.Realname = item.Realname;
                model.PayGrade = item.PayGrade;
                model.IsDelete = item.IsDelete;
                model.OpenLevel = item.OpenLevel;
                model.YearPlan = item.YearPlan;
                model.PayGrade = item.PayGrade;
                model.IsCPA = item.IsCPA;
                model.IsTest = item.IsTest;
                model.IsMust = item.IsMust;
                model.surveyNumber = item.surveyNumber;
                model.IsPing = item.IsPing;
                model.SurveyPaperId = item.SurveyPaperId;
                model.reallyApply = item.reallyApply;
                model.reallyJoin = item.reallyJoin;
                model.HaveJoin = item.HaveJoin;
                model.IsOrder = item.IsOrder;
                if (single.Any())
                {
                    model.reallyApply = item.reallyApply + single.FirstOrDefault().reallyApply;
                    model.reallyJoin = item.reallyJoin + single.FirstOrDefault().reallyJoin;
                    model.HaveJoin = item.HaveJoin + single.FirstOrDefault().HaveJoin;
                }
                newList.Add(model);
            }

            foreach (var item in RtcourseList)
            {
                var single = newList.Where(p => p.CourseId == item.CourseId);
                if (!single.Any())
                {
                    newList.Add(item);
                }
            }

            if (IsDelete == 0)
            {
                newList = newList.Where(p => p.reallyJoin > 0).ToList();
            }
            if (IsDelete == -1)
            {
                newList = newList.Where(p => (p.reallyJoin > 0 && p.IsDelete == 0) || p.IsDelete == 1).ToList();
            }

            foreach (var item in newList)
            {
                //单独为预定的时候
                if (item.IsOrder == 2)
                {
                    var list = allTrainGrade.Where(p => string.IsNullOrEmpty(item.OpenLevel) ? true : !item.OpenLevel.Contains(p)).ToList();
                    item.OpenLevel = list.Count() == 0 ? "" : string.Join(",", list);
                }
            }
            #region 根据条件进行筛选 sql中太慢
            if (IsDelete != -1)
            {
                newList = newList.Where(p => p.IsDelete == IsDelete).ToList();
            }

            if (!string.IsNullOrEmpty(openLevel))
            {
                var LevelList = openLevel.Trim(',').Split(',').ToList();
                if (LevelType == 0)
                {
                    newList = newList.Where(p => (string.IsNullOrEmpty(p.OpenLevel) ? false :
                       p.OpenLevel.Split(',').ToList().Where(t => LevelList.Contains(t)).Count() > 0)).ToList();
                }
                else
                {
                    newList = newList.Where(p => (string.IsNullOrEmpty(p.OpenLevel) ? false :
                   p.OpenLevel.Split(',').ToList().Where(t => LevelList.Contains(t)).Count() == LevelList.Count()
                   && p.OpenLevel.Split(',').Count() == LevelList.Count())).ToList();
                }

            }
            if (year != -1)
            {
                newList = newList.Where(p => p.YearPlan == year).ToList();
            }
            if (!string.IsNullOrEmpty(courseName))
            {
                newList = newList.Where(p => p.courseName.ToLower().Contains(courseName.Trim().ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(teacherName))
            {
                newList = newList.Where(p => p.teacher.ToLower().Contains(teacherName.Trim().ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(payGrade))
            {
                newList = newList.Where(p => payGrade.Split(',').ToList().Contains(("'" + p.PayGradeStr + "'"))).ToList();
            }
            if (IsCPA != -1 && IsCPA != 2)
            {
                newList = newList.Where(p => p.IsCPA == IsCPA).ToList();
            }
            if (IsTest != -1 && IsTest != 2)
            {
                newList = newList.Where(p => p.IsTest == IsTest).ToList();
            }
            if (IsMUst != -1)
            {
                newList = newList.Where(p => p.IsMust == IsMUst).ToList();
            }
            #endregion

            //s.Stop();
            // var a3 = s.ElapsedMilliseconds;
            var surveyScore = togethetDB.GetUserSurvey();
            var ListID = new List<int>();

            foreach (var item in newList)
            {
                if (!ListID.Contains(item.CourseId))
                {
                    ListID.Add(item.CourseId);

                    #region 评分

                    if (item.IsPing == 0)
                    {
                        item.Survey_AllScoreDouble = -1;
                        item.Survey_CourseScoreDouble = -1;
                        item.Survey_TeacherScoreDouble = -1;
                        item.surveyStr = "N/A";
                        item.surveyNumber = -1;
                    }
                    else
                    {
                        item.surveyStr = item.surveyNumber + "/" + item.NOSurvey;
                        item.Survey_CourseScoreDouble = -1;
                        item.Survey_TeacherScoreDouble = -1;
                        var list = item.SurveyPaperId.Split(';').ToList();
                        var sumdouble = 0.00;
                        var count = 0;
                        var newSList = surveyScore.Where(p => p.ObjectID == item.CourseId).ToList();
                        //计算平均分
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i] != null && list[i] != "")
                            {
                                var type = list[i].Split(',')[0].StringToInt32();
                                var paperID = list[i].Split(',')[1].StringToInt32();
                                var userScore = 0.00;
                                if (newList.Count() == 0)
                                {
                                    userScore = 0;
                                }
                                else
                                {
                                    var first = newSList.Where(p => p.ExampaperID == paperID).FirstOrDefault();
                                    if (first != null)
                                    {
                                        userScore = first.AveragePing;
                                    }
                                    else
                                    {
                                        userScore = 0;
                                    }
                                }
                                switch (type)
                                {
                                    case 0://课程
                                        count++;
                                        var courseScore = userScore;
                                        sumdouble = sumdouble + courseScore;
                                        item.Survey_CourseScoreDouble = courseScore;
                                        // item.Survey_TeacherScore = "N/A";
                                        break;
                                    case 2://讲师
                                        count++;
                                        var teacherScore = userScore;
                                        sumdouble = sumdouble + teacherScore;
                                        item.Survey_TeacherScoreDouble = teacherScore;
                                        // item.Survey_CourseScore = "N/A";
                                        break;
                                }
                            }
                        }
                        item.Survey_AllScoreDouble = count == 0 ? 0 : Math.Round(sumdouble / count, 2, MidpointRounding.AwayFromZero);
                    }

                    #endregion
                }
            }
            return newList;
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
        /// <returns></returns>
        public List<SingleTogtherSurvey> GetSingleTogtherSurvey(int courseID, string SurveyPaperId, int isPing, int isTest, string courseStarttime,
            string deptids = "", string sDate = "", string eDate = "")
        {
            //是否在课程结束时间之前,为 true的时候 考勤人数等都为N/A
            var IsIncourseDate = false;
            if (!string.IsNullOrEmpty(eDate))
            {
                if (Convert.ToDateTime(courseStarttime) > Convert.ToDateTime(eDate))
                {
                    IsIncourseDate = true;
                }
            }

            string dwhere = " 1=1";
            string where = " 1=1";
            var timewhere = " 1=1";
            var surveywhere = " 1=1 and ObjectID=" + courseID;
            if (!string.IsNullOrEmpty(deptids))
            {
                where += " AND syu.DeptId IN (" + deptids + ")";
                dwhere += " AND syu.DeptId IN (" + deptids + ")";
                surveywhere += " AND syu.DeptId IN (" + deptids + ")";
            }
            if (!string.IsNullOrEmpty(sDate))
            {
                timewhere += "AND ((SubmitTime>='" + sDate + "'";
                surveywhere += "AND ((SubmitTime>='" + sDate + "'";
            }
            if (!string.IsNullOrEmpty(eDate))
            {
                timewhere += "AND SubmitTime<='" + eDate + "') or SubmitTime IS NULL)";
                surveywhere += "AND SubmitTime<='" + eDate + "') or SubmitTime IS NULL)";
            }
            //计算总数
            var sumScore = togethetDB.GetUserSurvey(surveywhere);
            var userScore = togethetDB.GetSingleTogther(courseID);
            var Examlist = examDB.GetAllList<tbExamSendStudent>(Query.And(Query.In("SourceType", new BsonArray(new List<int>() { 1, 4 })), Query.EQ("RelationID", courseID), Query.EQ("DoExamStatus", 2)));
            var newTogtherList = new List<SingleTogtherSurvey>();

            Examlist = Examlist.Where(p => (string.IsNullOrEmpty(sDate) ? true : p.SubmitTime.ToLocalTime() >= Convert.ToDateTime(sDate))
                                                   && (string.IsNullOrEmpty(eDate) ? true : p.SubmitTime.ToLocalTime() <= Convert.ToDateTime(eDate))).ToList();

            #region 集中授课部分
            var list = togethetDB.GetSingleTogtherSurvey(courseID, where, dwhere, timewhere).Where(p => p.reallyJoin > 0);
            var SingleTogtherList = list.Select(p => new { p.DeptId, p.DeptName, p.HaveJoin, p.SurveyNumber, p.reallyJoin }).Distinct();

            ///赋值上每个人的属性
            foreach (var item in list)
            {
                var single = Examlist.Where(p => p.UserID == item.UserId);
                if (single.Any())
                {
                    var time = single.FirstOrDefault().AnswerTime;
                    item.IsTest = 1;
                    item.AnswerTime = string.IsNullOrEmpty(time) ? 0 : Math.Round(Convert.ToDouble(time) / 60, 0);
                    item.avgAnswerTimes = single.FirstOrDefault().TestTimes;
                    item.avgExamScore = single.FirstOrDefault().StudentAnswerList.Sum(p => p.GetScore);
                    item.IsPass = single.FirstOrDefault().Number;
                }
                else
                {
                    item.IsTest = 0;
                }
            }

            foreach (var DeptName in list.Select(p => p.DeptName).Distinct())
            {
                var singleList = list.Where(p => p.DeptName == DeptName);
                var deptIDList = singleList.Select(p => p.DeptId).Distinct();
                if (singleList.Any())
                {
                    // SingleTogtherList
                    var item = new SingleTogtherSurvey();
                    var single = SingleTogtherList.Where(p => deptIDList.Contains(p.DeptId));
                    item.DeptName = DeptName;
                    item.DeptIds = string.Join(",", deptIDList);
                    item.HaveJoin = single.Sum(p => p.reallyJoin);
                    item.SurveyNumber = single.Sum(p => p.SurveyNumber);
                    item.IsMain = 1;

                    #region 单门课程评估
                    if (isPing == 0 || IsIncourseDate)
                    {
                        item.HaveJoin = -1;
                        item.SurveyNumber = -1;
                        item.SurveyJoinRate = -1;
                        item.Survey_CourseScoreDouble = -1;
                        item.Survey_TeacherScoreDouble = -1;
                    }
                    else
                    {

                        item.SurveyJoinRate = item.HaveJoin == 0 ? 0 : Math.Round(Convert.ToDouble(item.SurveyNumber) / Convert.ToDouble(item.HaveJoin), 4, MidpointRounding.AwayFromZero);

                        item.Survey_CourseScoreDouble = -1;
                        item.Survey_TeacherScoreDouble = -1;
                        item.SumSurvey_CourseScore = -1;
                        item.SumSurvey_TeacherScore = -1;
                        var Surveylist = SurveyPaperId.Split(';').ToList();
                        var sumdouble = 0.00;
                        var count = 0;
                        var newList = userScore.ToList();

                        for (int i = 0; i < Surveylist.Count; i++)
                        {
                            if (Surveylist[i] != null && Surveylist[i] != "")
                            {
                                var type = Surveylist[i].Split(',')[0].StringToInt32();
                                var paperID = Surveylist[i].Split(',')[1].StringToInt32();
                                var uScore = 0.00;
                                var sScore = 0.00;
                                if (newList.Count() == 0)
                                {
                                    uScore = 0;
                                }
                                else
                                {
                                    var first = newList.Where(p => p.ExampaperID == paperID && deptIDList.Contains(p.DeptId)).FirstOrDefault();
                                    // questionCount = first.questionCount;
                                    if (first != null)
                                    {
                                        uScore = first.AveragePing;

                                    }
                                    else
                                    {
                                        uScore = 0;
                                    }
                                }
                                if (sumScore.Count() == 0)
                                {
                                    sScore = 0;
                                }
                                else
                                {
                                    var first = sumScore.Where(p => p.ExampaperID == paperID).FirstOrDefault();
                                    // questionCount = first.questionCount;
                                    if (first != null)
                                    {
                                        sScore = first.AveragePing;

                                    }
                                    else
                                    {
                                        sScore = 0;
                                    }
                                }
                                switch (type)
                                {
                                    case 0://课程
                                        count++;
                                        var courseScore = uScore;
                                        item.Survey_CourseScoreDouble = courseScore;
                                        // item.Survey_TeacherScore = "N/A";
                                        item.SumSurvey_CourseScore = sScore;

                                        break;
                                    case 2://讲师
                                        count++;
                                        var teacherScore = uScore;
                                        item.Survey_TeacherScoreDouble = teacherScore;
                                        item.SumSurvey_TeacherScore = sScore;
                                        break;
                                }
                            }
                        }

                    }
                    #endregion

                    #region 单门测试情况
                    if (isTest == 0 || IsIncourseDate)
                    {
                        item.ExamJoin = -1;
                        item.ExamHaveJoin = -1;
                        item.ExamJoinRate = -1;
                        item.ExamPassRate = -1;
                        item.AnswerTime = -1;
                        item.avgAnswerTimes = -1;
                        item.avgExamScore = -1;
                    }
                    else
                    {
                        //var userIDLis
                        item.ExamHaveJoin = single.Sum(p => p.reallyJoin);
                        item.ExamJoin = singleList.Where(p => p.IsTest == 1).Count();
                        item.ExmaPass = singleList.Where(p => p.IsPass == 1).Count();
                        item.AnswerTime = (ReportCommon.CalculateMedianDouble(singleList.Where(p => p.IsTest == 1).Select(p => p.AnswerTime).ToList()));
                        item.avgAnswerTimes = ReportCommon.CalculateMedianDouble(singleList.Where(p => p.IsTest == 1).Select(p => p.avgAnswerTimes).ToList());
                        item.avgExamScore = ReportCommon.CalculateMedianDouble(singleList.Where(p => p.IsTest == 1).Select(p => p.avgExamScore).ToList());
                        item.ExamPassRate = item.ExamJoin == 0 ? 0 : Math.Round(Convert.ToDouble(item.ExmaPass) / Convert.ToDouble(item.ExamJoin), 4, MidpointRounding.AwayFromZero);
                        item.ExamJoinRate = item.ExamHaveJoin == 0 ? 0 : Math.Round(Convert.ToDouble(item.ExamJoin) / Convert.ToDouble(item.ExamHaveJoin), 4, MidpointRounding.AwayFromZero);
                    }
                    #endregion

                    newTogtherList.Add(item);

                }
            }
            #endregion

            #region 视频转播部门
            var RTlist = togethetDB.GetSingleRTSurvey(courseID, where, dwhere).Where(p => p.reallyJoin > 0);
            var courseInfoList = RTlist.Select(p => new { p.DeptId, p.DeptName, p.HaveJoin, p.SurveyNumber, p.reallyJoin }).Distinct();
            Examlist = Examlist.Where(p => p.SourceType == 4).ToList();
            ///赋值上每个人的属性
            foreach (var item in RTlist)
            {
                var single = Examlist.Where(p => p.UserID == item.UserId);
                if (single.Any())
                {
                    var time = single.FirstOrDefault().AnswerTime;
                    item.IsTest = 1;
                    item.AnswerTime = string.IsNullOrEmpty(time) ? 0 : Math.Round(Convert.ToDouble(time) / 60, 0);
                    item.avgAnswerTimes = single.FirstOrDefault().TestTimes;
                    item.avgExamScore = single.FirstOrDefault().StudentAnswerList.Sum(p => p.GetScore);
                    item.IsPass = single.FirstOrDefault().Number;
                }
                else
                {
                    item.IsTest = 0;
                }
            }
            foreach (var DeptName in RTlist.Select(p => p.DeptName).Distinct())
            {
                var singleList = RTlist.Where(p => p.DeptName == DeptName);
                if (singleList.Any())
                {
                    var item = new SingleTogtherSurvey();
                    var DeptIdList = singleList.Select(p => p.DeptId);
                    var single = courseInfoList.Where(p => DeptIdList.Contains(p.DeptId));
                    item.DeptIds = string.Join(",", DeptIdList);
                    item.DeptName = DeptName;
                    item.HaveJoin = single.Sum(p => p.reallyJoin);
                    item.SurveyNumber = single.Sum(p => p.SurveyNumber);
                    item.IsMain = 0;
                    #region 单门课程评估
                    if (isPing == 0 || IsIncourseDate)
                    {

                        item.SurveyNumber = -1;
                        item.SurveyJoinRate = -1;
                        item.Survey_CourseScoreDouble = -1;
                        item.Survey_TeacherScoreDouble = -1;
                    }
                    else
                    {

                        item.SurveyJoinRate = item.HaveJoin == 0 ? 0 : Math.Round(Convert.ToDouble(item.SurveyNumber) / Convert.ToDouble(item.HaveJoin), 4, MidpointRounding.AwayFromZero);

                        item.Survey_CourseScoreDouble = -1;
                        item.Survey_TeacherScoreDouble = -1;
                        var Surveylist = SurveyPaperId.Split(';').ToList();
                        var sumdouble = 0.00;
                        var count = 0;
                        var newList = userScore.ToList();
                        for (int i = 0; i < Surveylist.Count; i++)
                        {
                            if (Surveylist[i] != null && Surveylist[i] != "")
                            {
                                var type = Surveylist[i].Split(',')[0].StringToInt32();
                                var paperID = Surveylist[i].Split(',')[1].StringToInt32();
                                var uScore = 0.00;
                                if (newList.Count() == 0)
                                {
                                    uScore = 0;
                                }
                                else
                                {
                                    var first = newList.Where(p => p.ExampaperID == paperID && DeptIdList.Contains(p.DeptId)).FirstOrDefault();
                                    if (first != null)
                                    {
                                        uScore = first.AveragePing;
                                    }
                                    else
                                    {
                                        uScore = 0;
                                    }
                                }
                                switch (type)
                                {
                                    case 0://课程
                                        count++;
                                        var courseScore = uScore;
                                        item.Survey_CourseScoreDouble = courseScore;
                                        // item.Survey_TeacherScore = "N/A";
                                        break;
                                    case 2://讲师
                                        count++;
                                        var teacherScore = uScore;
                                        item.Survey_TeacherScoreDouble = teacherScore;
                                        break;
                                }
                            }
                        }
                    }
                    #endregion

                    #region 单门测试情况
                    if (isTest == 0 || IsIncourseDate)
                    {
                        item.ExamJoin = -1;
                        item.ExamHaveJoin = -1;
                        item.ExamJoinRate = -1;
                        item.ExamPassRate = -1;
                        item.AnswerTime = -1;
                        item.avgAnswerTimes = -1;
                        item.avgExamScore = -1;
                    }
                    else
                    {
                        //var userIDLis
                        item.ExamHaveJoin = single.Sum(p => p.reallyJoin);
                        item.ExamJoin = singleList.Where(p => p.IsTest == 1).Count();
                        item.ExmaPass = singleList.Where(p => p.IsPass == 1).Count();
                        item.AnswerTime = (ReportCommon.CalculateMedianDouble(singleList.Where(p => p.IsTest == 1).Select(p => p.AnswerTime).ToList()));
                        item.avgAnswerTimes = ReportCommon.CalculateMedianDouble(singleList.Where(p => p.IsTest == 1).Select(p => p.avgAnswerTimes).ToList());
                        item.avgExamScore = ReportCommon.CalculateMedianDouble(singleList.Where(p => p.IsTest == 1).Select(p => p.avgExamScore).ToList());
                        item.ExamPassRate = item.ExamJoin == 0 ? 0 : Math.Round(Convert.ToDouble(item.ExmaPass) / Convert.ToDouble(item.ExamJoin), 4, MidpointRounding.AwayFromZero);
                        item.ExamJoinRate = item.ExamHaveJoin == 0 ? 0 : Math.Round(Convert.ToDouble(item.ExamJoin) / Convert.ToDouble(item.ExamHaveJoin), 4, MidpointRounding.AwayFromZero);
                    }
                    #endregion

                    newTogtherList.Add(item);
                }
            }

            #endregion

            return newTogtherList;
        }
        #endregion

        #region 缓存
        /// <summary>
        /// 每个部门分所应参加的人数
        /// </summary>
        /// <returns></returns>
        public List<fVedioJoinNumber> GetTogetherUser(int year=-1)
        {
            year = year == -1 ? DateTime.Now.Year : year;
            return togethetDB.GetTogetherUser(year);
        }

        /// <summary>
        /// 集中授课个部门参与考试的人数
        /// </summary>
        /// <returns></returns>
        public List<ReportTogether> GetCourseNumber()
        {
            var list = togethetDB.GetStudyUser();
            var passlist = examDB.GetAllList<tbExamSendStudent>(Query.In("SourceType", new BsonArray(new List<int>() { 1, 4 })), 1, 10, "UserID", "RelationID", "Number");

            var listID = new List<int>();
            var newlist = new List<ReportTogether>();
            foreach (var item in list)
            {
                var single = passlist.Where(p => p.RelationID == item.courseID && p.UserID == item.userID);
                if (single.Any())
                {
                    item.IsTest = 1;
                    item.IsPass = single.FirstOrDefault().Number;
                }
                else
                {
                    item.IsTest = 0;
                }
            }
            foreach (var item in list.Select(p => new { p.YearPlan, p.DeptID }).Distinct())
            {
                var model = new ReportTogether();
                var singleList = list.Where(p => p.DeptID == item.DeptID && p.YearPlan == item.YearPlan);
                model.YearPlan = item.YearPlan;
                model.DeptId = item.DeptID;
                model.ExamJoin = singleList.Where(p => p.IsTest == 1).Count();
                model.ExamPass = singleList.Where(p => p.IsPass == 1).Count();
                newlist.Add(model);
            }
            return newlist;
        }

        /// <summary>
        /// 缓存 课程的基本信息 实际参与人数人全部的信息
        /// </summary>
        /// <returns></returns>
        public List<ReportTogether> GetCacheCourseInfoList(string freeWhere=" 1=1")
        {
            return togethetDB.GetCourseInfoList(freeWhere: freeWhere).Where(p => p.Publishflag == 1).ToList();
        }
        #endregion
    }
}
