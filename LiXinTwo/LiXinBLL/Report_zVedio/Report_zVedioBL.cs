using LiXinDataAccess;
using LiXinInterface.Report_Vedio;
using LiXinModels.Report_zVedio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;
using System.Diagnostics;
using LiXinDataAccess.Examination;
using LiXinModels.Examination.DBModel;
using Retech.Core.Cache;
using Retech.Cache;
using LiXinBLL.Cache;
using MongoDB.Driver.Builders;
using LiXinModels.Report_Vedio;
using LiXinDataAccess.Report_fVedio;

namespace LiXinBLL.Report_Vedio
{
    public class Report_VedioBL : ReportCache, IReport_Vedio
    {
        private VedioCourseLearnDB vedioDB;
        private ExaminationDB examDB;
        protected ReportDB ReportDB;
        // protected ICacheManager cacheManager = new MemoryCacheManager();
        public Report_VedioBL()
        {
            vedioDB = new VedioCourseLearnDB();
            examDB = new ExaminationDB();
            ReportDB = new ReportDB();
        }

        #region 汇总
        /// <summary>
        /// 总所——视频课程汇总统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<CourseVedioLearn> GetVedioLearn(string where = " 1=1")
        {

            //Stopwatch s = new Stopwatch();
            //s.Start();
            var baseList = vedioDB.GetVedioLearn(where);

            var studentList = examDB.GetAllList<tbExamSendStudent>(Query.And(Query.EQ("SourceType", 2)), 1, 10, "UserID", "RelationID");

            var checkUser = GetCheckList.Select(p => p.UserId).ToList();

           // var UserSurveyList = vedioDB.GetUserSurvey();

            var learnIDList = ReportDB.GetCourseLearn();

            studentList = studentList.Where(p => checkUser.Contains(p.UserID)).ToList(); 
            foreach (var item in baseList)
            {
                var listNumber = Vedio_NumberList.Where(p => p.CourseID == item.CourseId).ToList();
                item.Number = Vedio_NumberList == null ? 0 : listNumber.Count > 0 ? listNumber.FirstOrDefault().SumNumber : 0;
                var userIDList = learnIDList.Where(p => p.CourseID == item.CourseId).Select(p => p.UserID).ToList();
                if (item.IsTest == 0)
                {
                    item.PassRateDouble = -1;
                }
                else
                {
                    var newlist = studentList.Where(p => p.RelationID == item.CourseId && userIDList.Contains(p.UserID));
                    var userCount = newlist.Count();
                    var pass = item.passNumber;
                    item.PassRateDouble = item.IsTest == 0 ? -1 : (userCount == 0 ? 0 : Math.Round(Convert.ToDouble(pass) / Convert.ToDouble(userCount), 4, MidpointRounding.AwayFromZero));
                }
                if (item.IsPing == 0)
                {
                    item.Survey_AllScoreDouble = -1;
                    item.Survey_CourseScoreDouble = -1;
                    item.Survey_TeacherScoreDouble = -1;
                }
                else
                {
                    item.Survey_CourseScoreDouble = -1;
                    item.Survey_TeacherScoreDouble = -1;
                    var list = item.SurveyPaperId.Split(';').ToList();
                    var sumdouble = 0.00;
                    var count = 0;
                    var newList = UserSurveyList.ToList();

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i] != null && list[i] != "")
                        {
                            var type = list[i].Split(',')[0].StringToInt32();
                            var paperID = list[i].Split(',')[1].StringToInt32();
                             var userScore= 0.00;
                            if(newList.Count() == 0)
                            {
                                userScore=0;
                            }
                            else
                            {
                                var first = newList.Where(p => p.ExampaperID == paperID && p.ObjectID == item.CourseId).FirstOrDefault();
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

            }
            //s.Stop();
            //var a1 = s.ElapsedMilliseconds;
            return baseList;
        }
        #endregion

        #region 单个
        /// <summary>
        /// 视频课程单门课程统计
        /// </summary>
        /// <returns></returns>
        public List<VedioLearnSingle> GetVedioLearnSingle(string where = " 1=1")
        {
            //var vedioNumber = cacheManager.Get("Vedio_Number", 1440, () => { return (new Report_VedioBL()).GetCacheVedioSumNumber(); });

            var list = vedioDB.GetVedioLearnSingle(where);

            foreach (var item in list)
            {
                if (Vedio_NumberList == null)
                {
                    item.sumNumber = 0;
                }
                else
                {
                    var listnumber = Vedio_NumberList.Where(p => p.CourseID == item.CourseId).ToList();
                    item.sumNumber = listnumber.Count > 0 ? listnumber.FirstOrDefault().SumNumber : 0;
                }

            }
            return list;
        }
        #endregion

        #region 参与情况

        /// <summary>
        /// 参与情况
        /// </summary>
        /// <param name="ListDeptId"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<CourseJoin> GetCourseJoinList(int courseID, string where = "1=1", int type = 0, string deptIDs = "", string querytime = " 1=1")
        {
            var list = new List<CourseJoin>();
            if (type == 0)
            {
                list = vedioDB.GetCourseJoinList(courseID, where, querytime);
            }
            else
            {
                list = vedioDB.GetCourseDeptIDJoinList(courseID, deptIDs, where, querytime);
            }

            var TimeDeptList = vedioDB.GetVedioTimeByDept(courseID);
            var numberList = FVedio_NumberList.Where(p => p.courseID == courseID);
            var newvedioList = new List<CourseJoin>();
            var listStr = new List<string>();
            foreach (var item in list)
            {
                if (Vedio_NumberList == null)
                {
                    item.SumNumber = 0;
                }
                else
                {
                    var listnumber = numberList.Where(p => p.DeptID == item.DeptID).ToList();
                    if (listnumber.Count > 0)
                    {
                        item.SumNumber = listnumber.Count > 0 ? listnumber.FirstOrDefault().count : 0;
                    }
                }
            }
            foreach (var item in list)
            {
                if (!listStr.Contains(item.DeptPath))
                {
                    listStr.Add(item.DeptPath);
                    var newList = TimeDeptList.Where(p => p.DeptPath == item.DeptPath).ToList();
                    var depList = list.Where(p => p.DeptPath == item.DeptPath);
                    // item.allDeptPath=
                    item.DeptIDs = string.Join(",", depList.Select(p => p.DeptID));
                    item.JoinNumber = depList.Sum(p => p.JoinNumber);
                    item.SumNumber = depList.Sum(p => p.SumNumber);
                    item.JoinRateDouble = item.SumNumber == 0 ? 0.00 : Math.Round(Convert.ToDouble(item.JoinNumber) / Convert.ToDouble(item.SumNumber), 4, MidpointRounding.AwayFromZero);
                    item.LearnNumber = depList.Sum(p => p.LearnNumber);
                    item.AvgLookTime = item.JoinNumber == 0 ? -1 : ReportCommon.CalculateMedianDouble(newList.Select(p => Math.Round(p.VedioTime / 60, 0)).ToList());
                    item.LookNumbr = item.JoinNumber == 0 ? -1 : ReportCommon.CalculateMedianDouble(newList.Select(p => p.LearnTimes).ToList());
                    newvedioList.Add(item);
                }
            }
            return newvedioList;
        }
        #endregion

        #region 公共缓存
        /// <summary>
        /// 课程应学习的总人数
        /// </summary>
        /// <returns></returns>
        public List<VedioNumber> GetCacheVedioSumNumber()
        {
         //   return vedioDB.GetCacheVedioSumNumber();
            var list = ReportDB.GetVedioJoinNumber();
            var listnumber = new List<VedioNumber>();
            if (list != null)
            {

                foreach (var item in list)
                {
                    if (listnumber.Count(p => p.CourseID == item.courseID) == 0)
                    {
                        var number = new VedioNumber();
                        number.CourseID = item.courseID;
                        number.SumNumber = list.Where(p => p.courseID == item.courseID).Sum(p => p.count);
                        listnumber.Add(number);
                    }


                }
                return listnumber;
            }
            return new List<VedioNumber>();
        }

        /// <summary>
        /// 用于纳入考核范围内的人员
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetCheckUserList()
        {
            return vedioDB.GetCheckUserList();
        }

        /// <summary>
        /// 课程评价
        /// </summary>
        /// <returns></returns>
        public List<UserSurvey> GetUserSurvey()
        {
            return vedioDB.GetUserSurvey();
        }

        /// <summary>
        /// 获取视频课程的应该参与人员对应的部门
        /// </summary>
        /// <returns></returns>
        public List<fVedioJoinNumber> GetfVedioJoinNumber()
        {
            return ReportDB.GetVedioJoinNumber();

        }

        /// <summary>
        /// 课程考试缓存
        /// </summary>
        /// <returns></returns>
        public List<tbExamSendStudent> GetExamList()
        {
            return examDB.GetAllList<tbExamSendStudent>(Query.And(Query.EQ("SourceType", 2)));
        }
        #endregion




    }
}
