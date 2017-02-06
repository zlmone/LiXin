using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.NewQueryStatistics;
using LiXinInterface;
using LiXinDataAccess.NewQueryStatistics;
using LiXinModels.NewCourseManage;
using LiXinDataAccess.Examination;
using LiXinCommon;
using System.Threading.Tasks;
using LiXinModels.User;
using LiXinModels.Examination.DBModel;
using LiXinDataAccess.NewCourseManage;

namespace LiXinBLL
{
    public class UserStatisticsBL : IUserStatistics
    {

        private UserStatisticsDB userSDB;

        private readonly ExampaperDB paperDB;

        private readonly ExaminationDB ExamDB;

        private readonly New_CourseDB CourseDB;
        public UserStatisticsBL()
        {
            userSDB = new UserStatisticsDB();
            ExamDB = new ExaminationDB();
            paperDB = new ExampaperDB();
            CourseDB = new New_CourseDB();
        }

        #region 学员综合评定

        /// <summary>
        /// 获取每个人的总评分
        /// </summary>
        /// <param name="tScore">配置中的集中授课总分</param>
        /// <param name="gScore">配置中的分组带教总分</param>
        /// <param name="tbaseScore">集中授课基准分数</param>
        /// <param name="gbaseScore">分组基准分数</param>
        /// <returns></returns>
        public List<ShowScore> GetScoreList(int tScore, int gScore, int tbaseScore = 5, int gbaseScore = 5, string where = " 1=1", string StartTime = "")
        {
            var list = userSDB.GetScoreList(where, StartTime);
            for (int i = 0; i < list.Count; i++)
            {
                var tsum = list[i].togetherCount * tbaseScore == 0 ? 0.00 :
                    Math.Round((Convert.ToDouble(list[i].togetherScore) / (list[i].togetherCount * tbaseScore)) * tScore, 2, MidpointRounding.AwayFromZero);
                var gsum = list[i].groupScore * list[i].groupCount == 0 ? 0.00 :
                Math.Round((Convert.ToDouble(list[i].groupScore) / (list[i].groupCount * gbaseScore)) * gScore, 2, MidpointRounding.AwayFromZero);
                list[i].StogetherSumScore = tsum > tScore ? tScore : tsum;
                list[i].SgroupSumScore = gsum > gScore ? gScore : gsum;
                //list[i].SAttendScore = gsum < gScore ? Math.Round(Convert.ToDouble(gScore), 2, MidpointRounding.AwayFromZero) : gsum;
            }
            return list;
        }


        /// <summary>
        /// 课程，独立考试得分总分
        /// </summary>
        /// <param name="perCScore">混合课程考试的百分比</param>
        /// <param name="perVScore">视频课程考试的百分比</param>
        /// <param name="perEScore">独立考试的百分比</param>
        /// <param name="pScore">配置中的考试应得分数</param>
        /// <returns></returns>
        public List<ShowScore> GetExamCourseScore(int perCScore, int perVScore, int perEScore, int pScore, string where = " 1=1")
        {
            //获取
            var list = userSDB.GetExamCourseScore(where);
            var courselist = GetCourseByUserID();
            var eSumScore = 0.00;//独立考试算出来的分数
            var cSumScore = 0.00;//课程考试算出来的分数
            var vSumScore = 0.00;//视频考试算出来的分数
            for (int i = 0; i < list.Count; i++)
            {
                var userID = list[i].UserID;
                var course = courselist.Where(p => p.UserId == userID).ToList();

                //混合课程考试的得分和总分
                var cscore = course.Where(p => p.Way == 1).Sum(p => p.CourseExamScore);
                var csumScore = course.Where(p => p.Way == 1).Sum(p => p.CourseExamSumScore);

                //视频
                var vscore = course.Where(p => p.Way == 2).Sum(p => p.CourseExamScore);
                var vsumScore = course.Where(p => p.Way == 2).Sum(p => p.CourseExamSumScore);

                eSumScore = list[i].examSumScore == 0 ? 0.00 : ((Convert.ToDouble(list[i].examScore) / Convert.ToDouble(list[i].examSumScore))) * (perEScore / 100.00) * pScore;
                cSumScore = csumScore == 0 ? 0.00 : ((Convert.ToDouble(cscore) / Convert.ToDouble(csumScore))) * (perCScore / 100.00) * pScore;
                vSumScore = vsumScore == 0 ? 0.00 : ((Convert.ToDouble(vscore) / Convert.ToDouble(vsumScore))) * (perVScore / 100.00) * pScore;

                list[i].cSumScore = Math.Round(cSumScore, 2, MidpointRounding.AwayFromZero);
                list[i].eSumScore = Math.Round(eSumScore, 2, MidpointRounding.AwayFromZero);
                list[i].vSumScore = Math.Round(vSumScore, 2, MidpointRounding.AwayFromZero);

                var SExamScore = eSumScore + cSumScore + vSumScore > pScore ? pScore : eSumScore + cSumScore + vSumScore;
                list[i].SExamScore = Math.Round(SExamScore, 2, MidpointRounding.AwayFromZero);
            }
            return list;

        }

        /// <summary>
        /// 获得评价的次数
        /// </summary>
        /// <param name="value">范围的奖励</param>
        /// <param name="SurveySingle">累加的奖励</param>
        /// <returns></returns>
        public List<ShowScore> GetSurveyList(string value, string SurveySingle, int RewardScore, int useType = 0, string where = " 1=1")
        {
            var list = userSDB.GetSurveyList(where);
            if (!string.IsNullOrEmpty(value))
            {
                var Sum = 0.00;
                var configArry = value.Split(';');
                for (int i = 0; i < list.Count; i++)
                {
                    if (useType == 0)
                    {
                        for (int j = 0; j < configArry.Length; j++)
                        {
                            var config = configArry[j].Split(',');
                            if (Convert.ToInt32(config[0]) <= list[i].Count && (config[1] == "-1" ? 1 == 1 : list[i].Count <= Convert.ToInt32(config[1])))
                            {
                                Sum = Convert.ToDouble(config[2]);
                            }
                        }
                        list[i].SRewardScore = Sum > RewardScore ? RewardScore : Math.Round(Sum, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        var Single = SurveySingle == "" ? 0.00 : Convert.ToDouble(SurveySingle);
                        Sum = list[i].Count * Single;
                        list[i].SRewardScore = Math.Round(Sum, 2, MidpointRounding.AwayFromZero);

                    }
                }

            }
            else
            {
                list.ForEach(p => p.SRewardScore = 0);
            }
            return list;
        }

        /// <summary>
        /// 获取考勤的详情
        /// </summary>
        /// <param name="configValue">考勤配置的字符串详情</param>
        /// <param name="configSingleValue">累加计算的配置</param>
        /// <param name="useType">0 范围计算  1累加计算</param>
        /// <param name="AttendScore">配置中考勤的总分</param>
        /// <param name="type">0正常算分  1算好次数之后，直接返回</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ShowScore> GetAttendceList(string configValue = "", string configSingleValue = "", int useType = 0, int AttendScore = 0, int type = 0, string where = " 1=1")
        {
            var showScore = new List<ShowScore>();
            var ConfigList = new List<AttendceConfig>();
            var Attendlist = userSDB.GetAttendceList(where: where);
            var config = configValue == "" ? null : configValue.Split(';');
            var configSingle = configSingleValue == "" ? null : configSingleValue.Split(',');
            //将配置放入List
            if (config != null)
            {
                Parallel.For(0, config.Count(), i =>
                {
                    var array = config[i].Split(',');
                    var AttendceConfig = new AttendceConfig();
                    AttendceConfig.type = array[0].StringToInt32();
                    AttendceConfig.min = array[1].StringToInt32();
                    AttendceConfig.max = array[2].StringToInt32();
                    AttendceConfig.score = array[3].StringToInt32();
                    ConfigList.Add(AttendceConfig);
                });
            }

            foreach (var item in Attendlist)
            {
                var model = new ShowScore();
                var attend = Attendlist.Where(p => p.UserId == item.UserId);
                if (showScore.Count(p => p.UserID == item.UserId) == 0)
                {
                    var sum = 0;
                    model.SAttendScore = 0;

                    model.UserID = item.UserId;
                    model.Chidao = attend.Count(p => p.AttStatusFlag == 3);
                    model.Zaotui = attend.Count(p => p.AttStatusFlag == 2);
                    model.queqin = attend.Count(p => p.AttStatusFlag == 1 && p.AttCount != 0);
                    model.ChidaoZaotui = attend.Count(p => p.AttStatusFlag == 4);

                    model.AttCount = attend.Count(p => p.AttStatusFlag == 5);

                    if (type == 0)
                    {
                        //范围计算
                        if (useType == 0)
                        {
                            #region 范围计算
                            foreach (var attendC in ConfigList)
                            {
                                switch (attendC.type)
                                {
                                    case 1://早退
                                        if (model.Zaotui != 0 && attendC.min <= model.Zaotui && (attendC.max == -1 ? 1 == 1 : model.Zaotui <= attendC.max))
                                        {
                                            sum += attendC.score;
                                        }
                                        break;
                                    case 0://迟到
                                        if (model.Chidao != 0 && attendC.min <= model.Chidao && (attendC.max == -1 ? 1 == 1 : model.Chidao <= attendC.max))
                                        {
                                            sum += attendC.score;
                                        }
                                        break;
                                    case 2://缺勤
                                        if (model.queqin != 0 && attendC.min <= model.queqin && (attendC.max == -1 ? 1 == 1 : model.queqin <= attendC.max))
                                        {
                                            sum += attendC.score;
                                        }
                                        break;
                                    case 3://迟到并早退
                                        if (model.ChidaoZaotui != 0 && attendC.min <= model.ChidaoZaotui && (attendC.max == -1 ? 1 == 1 : model.ChidaoZaotui <= attendC.max))
                                        {
                                            sum += attendC.score;
                                        }
                                        break;
                                }
                            }
                            #endregion
                        }
                        //累加计算
                        else
                        {
                            sum += configSingle == null ? 0 : model.Chidao * configSingle[0].StringToInt32() + model.Zaotui * configSingle[1].StringToInt32() +
                                model.queqin * configSingle[2].StringToInt32() + model.ChidaoZaotui * configSingle[3].StringToInt32();
                        }
                        model.SAttendScore = sum > AttendScore ? 0 : AttendScore - sum;

                    }
                    showScore.Add(model);
                }
            }
            return showScore;
        }
        #endregion

        #region 个人综合评定
        /// <summary>
        /// 获取课程的基本信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetCourseList(out int totalcount, int userID, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            var list = userSDB.GetCourseList(userID, startIndex, startLength, where);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

        /// <summary>
        /// 获得课程内考试的信息
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <param name="paperID">试卷ID</param>
        /// <param name="userID">人员ID</param>
        /// <param name="coPaperID">课程关联试卷表主键ID</param>
        /// <param name="score">输出:得分</param>
        /// <param name="sumscore">输出：总分</param>
        public void GetCourseExamScore(int courseID, int paperID, int userID, int coPaperID, out int score, out int sumscore)
        {
            var exam = ExamDB.GetListByUserID(userID, courseID);
            var exampaper = paperDB.GetExampaper(paperID);
            score = exam == null ? 0 : exam.StudentAnswerList.Sum(pa => pa.GetScore);
            sumscore = exam == null ? 0 : (exampaper == null ? 0 : exampaper.ExampaperScore);
        }

        /// <summary>
        /// 获得考勤的详情
        /// </summary>
        /// <param name="userID"></param>
        public List<New_CourseOrderDetail> GetAttendceList(int userID, int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " ncod.UserId")
        {
            return userSDB.GetAttendceList(startIndex, startLength, " ncod.UserId=" + userID, jsRenderSortField);
        }
        #endregion

        #region 公共

        /// <summary>
        /// 取得所有有学习记录的学员，加上所有的新进员工（并集）
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetAllStudyUser(string where = " 1=1")
        {
            return userSDB.GetAllStudyUser(where);
        }

        /// <summary>
        /// 获取新进员工的考试信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetListByUserID()
        {
            var examlist = ExamDB.GetListByUserID();
            var paperlist = paperDB.GetAllExampaperList();
            var courselist = new List<New_CourseOrder>();
            foreach (var item in examlist)
            {
                var model = new New_CourseOrder();
                model.CourseId = item.RelationID;
                model.UserId = item.UserID;
                model.CourseExamScore = item.StudentAnswerList.Sum(pa => pa.GetScore);
                var paper = paperlist.Find(p => p._id == item.ExamPaperID);
                model.CourseExamSumScore = paper == null ? 0 : paper.ExampaperScore;
                courselist.Add(model);
            }
            return courselist;
        }
        /// <summary>
        /// 获取新进员工的学习的课程考试信息（在上一步的基础上进行筛选）
        /// </summary>
        /// <returns></returns>
        public List<New_CourseOrder> GetCourseByUserID()
        {
            int totalCount = 0;
            var examlist = ExamDB.GetListByUserID();
            var paperlist = paperDB.GetAllExampaperList();
            var courseIDlist = userSDB.GetCourserIDList();
            // var NewcourseList = CourseDB.GetNew_CourseList(out totalCount);
            var courselist = new List<New_CourseOrder>();
            foreach (var item in examlist)
            {
                var single = courseIDlist.Where(p => p.UserId == item.UserID && p.CourseId == item.RelationID);
                var model = new New_CourseOrder();
                //if (courseIDlist.Count(p => p.UserId == item.UserID && p.CourseId == item.RelationID) > 0)
                //{
                if (single.Count() > 0)
                {
                    model.CourseId = item.RelationID;
                    model.UserId = item.UserID;
                    model.Way = single == null ? 0 : single.FirstOrDefault().Way;
                    model.CourseExamScore = item.StudentAnswerList.Sum(pa => pa.GetScore);
                    var paper = paperlist.Find(p => p._id == item.ExamPaperID);
                    model.CourseExamSumScore = paper == null ? 0 : paper.ExampaperScore;
                    courselist.Add(model);
                }
            }
            return courselist;
        }




        #endregion
    }
}
