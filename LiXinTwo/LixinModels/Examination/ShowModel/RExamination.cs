using System;
using System.Collections.Generic;

namespace LiXinModels.Examination.ShowModel
{
    public class RExamination
    {
        #region 公用

        #region model

        /// <summary>
        ///     ExaminationID
        /// </summary>
        public int _id { set; get; }

        /// <summary>
        ///     ExampaperID
        /// </summary>
        public int PaperID { set; get; }

        /// <summary>
        ///     Examination Title
        /// </summary>
        public string ExaminationTitle { set; get; }

        /// <summary>
        ///     CreaterID
        /// </summary>
        public int UserID { set; get; }

        /// <summary>
        ///     Examination StartTime
        /// </summary>
        public DateTime ExamBeginTime { set; get; }

        /// <summary>
        ///     Examination EndTime
        /// </summary>
        public DateTime ExamEndTime { set; get; }

        /// <summary>
        ///     Examination Length
        /// </summary>
        public int ExamLength { set; get; }

        /// <summary>
        ///     是否百分制(0：是；1：否)
        /// </summary>
        public int PercentFlag { set; get; }

        /// <summary>
        ///     Show Type(0:all;1:single（rollback）；2：single（no rollback）)
        /// </summary>
        public int ShowType { set; get; }

        /// <summary>
        ///     allow times
        /// </summary>
        public int TestTimes { set; get; }

        /// <summary>
        ///     Radom Order Flag(0:no random;1:random;)
        /// </summary>
        public int RadomOrderFlag { set; get; }

        /// <summary>
        ///     Published Flag（0:	no publish; 1:publish）
        /// </summary>
        public int PublishedFlag { set; get; }

        /// <summary>
        ///     及格分数
        /// </summary>
        public double PassScore { get; set; }

        /// <summary>
        ///     SendMessage Flag(0:neibu 1:Email 2:all)
        /// </summary>
        public int SendMessageFlag { set; get; }

        /// <summary>
        ///     Examination Rules
        /// </summary>
        public string ExamRules { set; get; }

        /// <summary>
        ///     LastUpdateTime
        /// </summary>
        public DateTime LastUpdateTime { set; get; }

        /// <summary>
        ///     Create Time
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        ///     Examination ApprovalUser
        /// </summary>
        public List<int> ApprovalUser { set; get; }

        /// <summary>
        ///     Status
        ///     0:zhengchang;1:deleted
        /// </summary>
        public int Status { set; get; }

        #endregion

        public List<RExamSendStudent> tbExamSendStudents { get; set; }

        //public tbExampaper tbExampaper { get; set; }

        /// <summary>
        ///     试卷总分
        /// </summary>
        public int ExamPaperTotalScore { get; set; }

        #endregion

        #region 参与率和通过率

        /// <summary>
        ///     安排人数
        /// </summary>
        public int TotalPerson { get; set; }

        /// <summary>
        ///     参与人数
        /// </summary>
        public int JoinPerson { get; set; }

        /// <summary>
        ///     参与率
        /// </summary>
        public int JoinRate { get; set; }

        /// <summary>
        ///     通过人数
        /// </summary>
        public int PassPerson { get; set; }

        /// <summary>
        ///     通过率
        /// </summary>
        public int PassRate { get; set; }

        #endregion

        #region 题型正确率

        /// <summary>
        ///     若没有此题型的题目，则为-1
        /// </summary>
        public Dictionary<string, int> QuesTypeCorrectRate { get; set; }

        /// <summary>
        ///     题型最高正确率
        /// </summary>
        public int QuesTypeCorrectMaxRate { get; set; }

        /// <summary>
        ///     题型最低正确率
        /// </summary>
        public int QuesTypeCorrectMinRate { get; set; }

        /// <summary>
        ///     题型平均正确率
        /// </summary>
        public int QuesTypeCorrectAverageRate { get; set; }

        /// <summary>
        ///     题型最高正确率 题型名
        /// </summary>
        public string QuesTypeCorrectMaxRateName { get; set; }

        /// <summary>
        ///     题型最低正确率 题型名
        /// </summary>
        public string QuesTypeCorrectMinRateName { get; set; }

        #endregion

        #region 题库正确率

        public Dictionary<string, int> QuesSortCorrectRate { get; set; }

        public int QuesSortCorrectMaxRate { get; set; }

        public int QuesSortCorrectMinRate { get; set; }

        public int QuesSortCorrectAverageRate { get; set; }

        public string QuesSortCorrectMaxRateName { get; set; }

        public string QuesSortCorrectMinRateName { get; set; }

        #endregion

        #region 成绩分布

        public Dictionary<string, int> RecordsDistribution { get; set; }

        #endregion
    }
}