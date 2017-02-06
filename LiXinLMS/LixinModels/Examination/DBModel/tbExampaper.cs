using System;
using System.Collections.Generic;

namespace LiXinModels.Examination.DBModel
{
    /// <summary>
    ///     Examination'Exampaper
    ///     MongoDB Document of Exampaper
    /// </summary>
    public class tbExampaper
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public tbExampaper()
        {
            QuestionList = new List<ReExampaperQuestion>();
            QuestionRule = new List<ReRuleQuestion>();
        }

        /// <summary>
        ///     Exampaper ID
        /// </summary>
        public int _id { set; get; }

        /// <summary>
        ///     Exampaper Title
        /// </summary>
        public string ExampaperTitle { set; get; }

        /// <summary>
        ///     题型排序（2,3,1……）
        /// </summary>
        public string QuestionTypeOrder { set; get; }

        /// <summary>
        ///     Exampaper Type（0:no radom;1:radom）
        /// </summary>
        public int ExamType { set; get; }

        /// <summary>
        ///     Exampaper Score
        /// </summary>
        public int ExampaperScore { set; get; }

        /// <summary>
        ///     Creater ID
        /// </summary>
        public int UserID { set; get; }

        /// <summary>
        ///     LastUpdateTime
        /// </summary>
        public DateTime LastUpdateTime { set; get; }

        /// <summary>
        ///     Create Time
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        ///     Exampaper Description
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        ///     Exampaper Distribution
        /// </summary>
        public string Distribution { set; get; }

        /// <summary>
        ///     ExamSortID
        /// </summary>
        public int ExamSortID { set; get; }

        /// <summary>
        ///     Status
        ///     0:zhengchang;1:deleted
        /// </summary>
        public int Status { set; get; }

        /// <summary>
        ///     Exampaper re Question
        /// </summary>
        public List<ReExampaperQuestion> QuestionList { set; get; }

        /// <summary>
        ///     Exampaper Rule
        /// </summary>
        public List<ReRuleQuestion> QuestionRule { set; get; }
    }

    /// <summary>
    ///     Relation Exampaper Question
    /// </summary>
    public class ReExampaperQuestion
    {
        /// <summary>
        ///     QuestionID
        /// </summary>
        public int Qid { set; get; }

        /// <summary>
        ///     Question Order
        /// </summary>
        public int QOrder { set; get; }

        /// <summary>
        ///     Question Type
        /// </summary>
        public int QType { set; get; }

        /// <summary>
        ///     Question Score
        /// </summary>
        public int QScore { set; get; }
    }

    /// <summary>
    ///     Relation Rule Question
    /// </summary>
    public class ReRuleQuestion
    {
        /// <summary>
        ///     类型
        /// </summary>
        public int Qtype { set; get; }

        /// <summary>
        ///     题库
        /// </summary>
        public int QSort { set; get; }

        /// <summary>
        ///     难度及数量，如：1:3;2:2;3:5
        /// </summary>
        public string QLevelStr { set; get; }

        /// <summary>
        ///     分值
        /// </summary>
        public int QScore { set; get; }
    }
}