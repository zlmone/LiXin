using LiXinBLL.Report_Vedio;
using LiXinDataAccess;
using LiXinDataAccess.Examination;
using LiXinDataAccess.Report_fVedio;
using LiXinInterface.Report_fVedio;
using LiXinModels.Examination.DBModel;
using LiXinModels.Report_Vedio;
using LiXinModels.Report_zVedio;
using Retech.Cache;
using Retech.Core.Cache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LiXinCommon;
using LiXinBLL.Cache;
using MongoDB.Driver.Builders;

namespace LiXinBLL.Report_fVedio
{
    public class ReportBL : ReportCache, IReport
    {
        protected ReportDB ReportDB;
        private ExaminationDB examDB;
      //  protected ICacheManager cacheManager = new MemoryCacheManager();
        public ReportBL()
        {
            ReportDB = new ReportDB();
            examDB = new ExaminationDB();
        }

        #region 汇总
        /// <summary>
        /// 总所——视频课程汇总统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<CourseVedioLearn> GetVedioLearn(List<int> ListDeptId, string where = " 1=1")
        {
            //纳入考核范围内的人
            var checkUser = GetCheckList.Select(p => p.UserId).ToList();

            
            var deptIDs = string.Join(",", ListDeptId);
            var ListUserDept = ReportDB.GetUserDept(deptIDs);
            var baseList = ReportDB.GetVedioLearn(deptIDs,where);
           var learnIDList = ReportDB.GetCourseLearn();

            //人员考试
            var studentList = examDB.GetAllList<tbExamSendStudent>(Query.And(Query.EQ("SourceType", 2)), 1, 10, "UserID", "RelationID");
            studentList = studentList.Where(p => checkUser.Contains(p.UserID)).ToList();
            foreach (var item in baseList)
            {
                var listNumber = Vedio_NumberList.Where(p => p.CourseID == item.CourseId).ToList();
                var listfNumber = FVedio_NumberList.Where(p => p.courseID == item.CourseId && ListDeptId.Contains(p.DeptID)).ToList();
                item.Number = Vedio_NumberList == null ? 0 : listNumber.Count > 0 ? listNumber.FirstOrDefault().SumNumber : 0;
                item.fNumber = FVedio_NumberList == null ? 0 : listfNumber.Count > 0 ? listfNumber.Sum(p => p.count) : 0;

               var userIDList = learnIDList.Where(p => p.CourseID == item.CourseId).Select(p => p.UserID).ToList();

                //计算通过率
                if (item.IsTest == 0)
                {
                    item.PassRateDouble = -1;
                    item.fPassRateDouble = -1;
                }
                else
                {
                    var newlist = studentList.Where(p => p.RelationID == item.CourseId && userIDList.Contains(p.UserID));
                    var userCount = newlist.Count();
                    var pass = item.passNumber;
                    item.PassRateDouble = item.IsTest == 0 ? -1 : (userCount == 0 ? 0 : Math.Round(Convert.ToDouble(pass) / Convert.ToDouble(userCount), 4, MidpointRounding.AwayFromZero));

                    var fnewlist = newlist.Where(p => ListUserDept.Contains(p.UserID));
                    var fuserCount =fnewlist.Count();
                    var fpass = item.fPassNumber;
                    item.fPassRateDouble = item.IsTest == 0 ? -1 : (fuserCount == 0 ? 0 : Math.Round(Convert.ToDouble(fpass) / Convert.ToDouble(fuserCount), 4, MidpointRounding.AwayFromZero));
                }
                //计算课后评估
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
                    var newList = UserSurveyList;
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
            return baseList;
        }
        #endregion

        #region 单个
        /// <summary>
        /// 视频课程单门课程统计
        /// </summary>
        /// <returns></returns>
        public List<VedioLearnSingle> GetVedioLearnSingle(List<int> ListDeptId, string where = " 1=1")
        {
          //  var vedioNumber = cacheManager.Get("fVedio_JoinNumber", 1440, () => { return (new ReportBL()).GetfVedioJoinNumber(); });
            // var vedioAllNumber = cacheManager.Get<List<VedioNumber>>("Vedio_Number");

            var DeptIds = string.Join(",", ListDeptId);

            var list = ReportDB.GetVedioLearnSingle(DeptIds, where);

            foreach (var item in list)
            {
                if (FVedio_NumberList == null)
                {
                    item.sumNumber = 0;
                }
                else
                {
                    var listfNumber = FVedio_NumberList.Where(p => p.courseID == item.CourseId && ListDeptId.Contains(p.DeptID)).ToList();
                    item.sumNumber = listfNumber.Count > 0 ? listfNumber.Sum(p => p.count) : 0;
                }
            }


            return list;
        }



        #endregion

        #region 缓存
        ///// <summary>
        ///// 获取视频课程的应该参与人员对应的部门
        ///// </summary>
        ///// <returns></returns>
        //public List<fVedioJoinNumber> GetfVedioJoinNumber()
        //{
        //    return ReportDB.GetVedioJoinNumber();
        //}

        #endregion

    }
}
