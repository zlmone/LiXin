using LiXinModels.NewCourseManage;
using LiXinModels.NewQueryStatistics;
using LiXinModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface
{
    public interface IUserStatistics
    {
        /// <summary>
        /// 获取得分，总数
        /// </summary>
        /// <param name="tScore">配置中的集中授课总分</param>
        /// <param name="gScore">配置中的分组带教总分</param>
        /// <param name="tbaseScore">集中授课基准分数</param>
        /// <param name="gbaseScore">分组基准分数</param>
        /// <returns></returns>
        List<ShowScore> GetScoreList(int tScore, int gScore, int tbaseScore = 5, int gbaseScore = 5, string where = " 1=1", string StartTime = "");


        /// <summary>
        /// 课程，独立考试得分总分
        /// </summary>
        /// <param name="perCScore">混合课程考试的百分比</param>
        /// <param name="perVScore">视频课程考试的百分比</param>
        /// <param name="perEScore">独立考试的百分比</param>
        /// <param name="pScore">配置中的考试应得分数</param>
        /// <returns></returns>
        List<ShowScore> GetExamCourseScore(int perCScore, int perVScore, int perEScore, int pScore, string where = " 1=1");

        /// <summary>
        /// 获得评价的次数
        /// </summary>
        /// <param name="value">配置中的奖励</param>
        /// <returns></returns>
        List<ShowScore> GetSurveyList(string value, string SurveySingle, int RewardScore, int useType = 0, string where = " 1=1");


        /// <summary>
        /// 获取课程的基本信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<New_CourseOrder> GetCourseList(out int totalcount, int userID, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1");

        /// <summary>
        /// 获得课程内考试的信息
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <param name="paperID">试卷ID</param>
        /// <param name="userID">人员ID</param>
        /// <param name="coPaperID">课程关联试卷表主键ID</param>
        /// <param name="score">输出:得分</param>
        /// <param name="sumscore">输出：总分</param>
        void GetCourseExamScore(int courseID, int paperID, int userID, int coPaperID, out int score, out int sumscore);

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
        List<ShowScore> GetAttendceList(string configValue = "", string configSingleValue = "", int useType = 0, int AttendScore = 0, int type = 0, string where = " 1=1");

        /// <summary>
        /// 获得考勤的详情
        /// </summary>
        /// <param name="userID"></param>
        List<New_CourseOrderDetail> GetAttendceList(int userID, int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " ncod.UserId");

        /// <summary>
        /// 取得所有有学习记录的学员，加上所有的新进员工（并集）
        /// </summary>
        /// <returns></returns>
        List<Sys_User> GetAllStudyUser(string where = " 1=1");

        /// <summary>
        /// 获取新进员工的考试信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<New_CourseOrder> GetListByUserID();
    }
}
