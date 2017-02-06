using System.Collections.Generic;
using LiXinModels.Examination.DBModel;

namespace LiXinModels.Examination.ShowModel
{
    public class MExampaperShow
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public MExampaperShow()
        {
            QuestionList = new List<ReExampaperQuestion>();
            QuestionRule = new List<ReRuleQuestion>();
        }

        #region Property

        /// <summary>
        ///     ID
        /// </summary>
        public int id { set; get; }

        /// <summary>
        ///     Exampaper SortID
        /// </summary>
        public string ExampaperTitle { set; get; }

        /// <summary>
        ///     Exampaper Type
        /// </summary>
        public string ExamType { set; get; }

        /// <summary>
        ///     Exampaper Score
        /// </summary>
        public int ExampaperScore { set; get; }

        /// <summary>
        ///     Exampaper QuestionTypeList
        /// </summary>
        public string QuestionTypeList { set; get; }

        /// <summary>
        ///     Exampaper SortID
        /// </summary>
        public int ExamSortID { set; get; }

        /// <summary>
        ///     Exampaper SortTitle
        /// </summary>
        public string ExamSortTitle { set; get; }

        /// <summary>
        ///     Exampaper Distribution
        /// </summary>
        public string Distribution { set; get; }

        /// <summary>
        ///     Exampaper Description
        /// </summary>
        public string Description { set; get; }


        /// <summary>
        ///     Last Update Time
        /// </summary>
        public string LastUpdateTime { set; get; }

        /// <summary>
        ///     Exampaper Question
        /// </summary>
        public List<ReExampaperQuestion> QuestionList { set; get; }

        /// <summary>
        ///     Rule Question
        /// </summary>
        public List<ReRuleQuestion> QuestionRule { set; get; }

        /// <summary>
        ///     创建者
        /// </summary>
        public string Creater { get; set; }

        #endregion
    }
}