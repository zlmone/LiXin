using System;
using System.Collections.Generic;
using LiXinModels.Examination.DBModel;

namespace LiXinModels.Examination.ShowModel
{
    public class MQuestionShow
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public MQuestionShow()
        {
            FileUpload = new List<FileUpload>();
            QuestionAnswer = new List<QuestionAnswer>();
        }

        #region Property

        /// <summary>
        ///     ID
        /// </summary>
        public int id { set; get; }

        /// <summary>
        ///     Question SortID
        /// </summary>
        public int QuestionSortID { set; get; }

        /// <summary>
        ///     Question Content
        /// </summary>
        public string QuestionContent { set; get; }


        /// <summary>
        ///     Question Key
        /// </summary>
        public string QuestionKey { set; get; }

        /// <summary>
        ///     Question Answer Keys
        /// </summary>
        public string QuestionAnswerKeys { set; get; }

        /// <summary>
        ///     Question Analysis
        /// </summary>
        public string QuestionAnalysis { set; get; }


        /// <summary>
        ///     Last Update Time
        /// </summary>
        public DateTime LastUpdateTime { set; get; }

        /// <summary>
        ///     Status
        ///     0:zhengchang;1:deleted
        /// </summary>
        public int Status { set; get; }

        /// <summary>
        ///     File Attaches
        /// </summary>
        public List<FileUpload> FileUpload { set; get; }

        /// <summary>
        ///     Question Answer
        /// </summary>
        public List<QuestionAnswer> QuestionAnswer { set; get; }

        /// <summary>
        ///     创建者
        /// </summary>
        public string Creater { get; set; }

        /// <summary>
        ///     创建之间，转换为本地时间
        /// </summary>
        public string CreatLocalTime { get; set; }

        /// <summary>
        ///     问题类型
        /// </summary>
        public int QuestionType { get; set; }

        /// <summary>
        ///     问题类型
        /// </summary>
        public int QuestionLevel { get; set; }


        /// <summary>
        ///     问题类型
        /// </summary>
        public string QuestionTypeStr { get; set; }

        /// <summary>
        ///     难度
        /// </summary>
        public string QuestionLevelStr { get; set; }

        /// <summary>
        ///     调用次数
        /// </summary>
        public int VoidTimes { get; set; }


        public string SortName { get; set; }

        #endregion
    }
}