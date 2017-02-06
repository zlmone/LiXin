using System.Collections.Generic;
using LiXinModels.Examination.ShowModel;

namespace LiXinInterface.Examination
{
    public interface IExamReport
    {
        /// <summary>
        ///     获得考试列表（报表模板）
        /// </summary>
        /// <param name="ExamName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        List<RExamination> GetCommonRExamination(string ExamName, string StartTime, string EndTime);

        /// <summary>
        ///     得到考试列表，填充内部参与率和通过率字段
        /// </summary>
        /// <param name="ExamName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        List<RExamination> GetJoinAndPassExamReport(string ExamName, string StartTime, string EndTime);

        /// <summary>
        ///     得到考试列表，填充内部题型正确率字段
        /// </summary>
        /// <param name="ExamName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        List<RExamination> GetQuestionCorrectReport(string ExamName, string StartTime, string EndTime);

        /// <summary>
        ///     得到考试列表，填充内部题库正确率字段
        /// </summary>
        /// <param name="ExamName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="QuesSortTitle"></param>
        /// <returns></returns>
        List<RExamination> GetQuestionSortCorrectReport(string ExamName, string StartTime, string EndTime,
                                                        out List<string> QuesSortTitle);

        /// <summary>
        ///     得到考试列表，填充内部成绩分布字段
        /// </summary>
        /// <param name="ExamName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="ScoreSize">成绩梯度的分值</param>
        /// <param name="ScoreCount">几个梯度</param>
        /// <returns></returns>
        List<RExamination> GetExaminationRecordsDistribution(string ExamName, string StartTime, string EndTime,
                                                             out List<string> distribution, int ScoreSize = 9,
                                                             int ScoreCount = 5);

        RExamination GetExaminationRecordsDistribution(int id, out List<string> distribution, int ScoreSize = 9,
                                                       int ScoreCount = 5);

        /// <summary>
        ///     成绩与排名统计
        /// </summary>
        /// <returns></returns>
        List<ExamGradeRank> GetGradeRank();
    }
}