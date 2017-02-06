using System;
using System.Collections.Generic;

namespace LiXinModels.Examination.DBModel
{
    /// <summary>
    ///     Examination'Question
    ///     MongoDB Document of Question
    /// </summary>
    public class tbQuestion
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public tbQuestion()
        {
            FileUpload = new List<FileUpload>();
            QuestionAnswer = new List<QuestionAnswer>();
        }

        #region Property

        /// <summary>
        ///     ID
        /// </summary>
        public int _id { set; get; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { set; get; }

        /// <summary>
        ///     Question SortID
        /// </summary>
        public int QuestionSortID { set; get; }

        /// <summary>
        ///     Question Content
        /// </summary>
        public string QuestionContent { set; get; }

        /// <summary>
        ///     Question Type
        ///     1wendati:;2:danxuan;3:duoxuan;4:panduan;5:tiankong;6:duomeiti
        /// </summary>
        public int QuestionType { set; get; }

        /// <summary>
        ///     Question Level
        ///     1:easy;2:common;3:hard
        /// </summary>
        public int QuestionLevel { set; get; }

        /// <summary>
        ///     Question Key
        /// </summary>
        public int QuestionKey { set; get; }

        /// <summary>
        ///     Question Answer Keys
        /// </summary>
        public string QuestionAnswerKeys { set; get; }

        /// <summary>
        ///     Question Analysis
        /// </summary>
        public string QuestionAnalysis { set; get; }

        /// <summary>
        ///     Create userID
        /// </summary>
        public int UserID { set; get; }

        /// <summary>
        ///     Create Time
        /// </summary>
        public DateTime CreateTime { set; get; }

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

        #endregion
    }

    /// <summary>
    ///     Question Answer
    /// </summary>
    public class QuestionAnswer
    {
        /// <summary>
        ///     Order(1、2、3、4.......)
        /// </summary>
        public int Order { set; get; }

        /// <summary>
        ///     Answer Content
        /// </summary>
        public string Answer { set; get; }

        /// <summary>
        ///     Is Right Answer(0:Wrong;1:Right)
        /// </summary>
        public int AnswerFlag { set; get; }

        /// <summary>
        ///     Answer Type (Usered in type of duomeiti)（0:wenda;1:danxuan;2:duoxuan;）
        /// </summary>
        public int AnswerType { set; get; }
    }

    /// <summary>
    ///     File Attaches
    /// </summary>
    public class FileUpload
    {
        public FileUpload()
        {
            FileType = -1;
            FileName = "";
        }

        /// <summary>
        ///     Type 0：picture，1：yinpin，2：vidio
        /// </summary>
        public int FileType { set; get; }

        /// <summary>
        ///     Name
        /// </summary>
        public string FileName { set; get; }

        /// <summary>
        ///     RealName
        /// </summary>
        public string RealName { get; set; }

        
    }
}