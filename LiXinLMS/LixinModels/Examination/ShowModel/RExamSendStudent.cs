using System;
using System.Collections.Generic;

namespace LiXinModels.Examination.ShowModel
{
    /// <summary>
    ///     Examination User
    /// </summary>
    public class RExamSendStudent
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public RExamSendStudent()
        {
            StudentAnswerList = new List<RReStudentExamAnswer>();
        }

        #region model

        /// <summary>
        ///     reID
        /// </summary>
        public int _id { set; get; }

        /// <summary>
        ///     RelationID Such as:
        ///     Examination:ExaminationID;
        /// </summary>
        public int RelationID { set; get; }

        /// <summary>
        ///     SourceType
        ///     （0:Examination）
        /// </summary>
        public int SourceType { set; get; }

        /// <summary>
        ///     ExamPaperID
        /// </summary>
        public int ExamPaperID { set; get; }

        /// <summary>
        ///     UserID
        /// </summary>
        public int UserID { set; get; }

        /// <summary>
        ///     DoExam Status
        ///     （0:no do ;1:no over;2:over;3;approvaled）
        /// </summary>
        public int DoExamStatus { set; get; }

        /// <summary>
        ///     LastUpdateTime
        /// </summary>
        public DateTime LastUpdateTime { set; get; }

        /// <summary>
        ///     TestTime
        /// </summary>
        public DateTime TestTime { set; get; }

        /// <summary>
        ///     SubmitTime
        /// </summary>
        public DateTime SubmitTime { set; get; }

        /// <summary>
        ///     TestedTimes
        /// </summary>
        public int TestTimes { set; get; }

        /// <summary>
        ///     IsPass
        ///     (0:no pass;1:pass)
        /// </summary>
        public int IsPass { set; get; }

        /// <summary>
        ///     Status
        ///     0:zhengchang;1:deleted
        /// </summary>
        public int Status { set; get; }

        /// <summary>
        ///     User Answer
        /// </summary>
        public List<RReStudentExamAnswer> StudentAnswerList { set; get; }

        #endregion

        /// <summary>
        ///     试卷得分
        /// </summary>
        public int GetTotalScore { get; set; }
    }

    /// <summary>
    ///     User Answer
    /// </summary>
    public class RReStudentExamAnswer
    {
        #region model

        /// <summary>
        ///     QuestionID
        /// </summary>
        public int Qid { set; get; }

        /// <summary>
        ///     Question Type
        /// </summary>
        public int QType { set; get; }

        /// <summary>
        ///     DoneFlag
        ///     (0:no;  1:done)
        /// </summary>
        public int DoneFlag { set; get; }

        /// <summary>
        ///     user'Answer
        /// </summary>
        public string Answer { set; get; }

        /// <summary>
        ///     GetScore
        /// </summary>
        public int GetScore { set; get; }

        /// <summary>
        ///     Evlution
        /// </summary>
        public string Evlution { set; get; }

        #endregion

        /// <summary>
        ///     题库Id
        /// </summary>
        public int QuestionSortID { get; set; }

        /// <summary>
        ///     题库名
        /// </summary>
        public string QuestionSortTitle { get; set; }

        //public LiXinModels.Examination.DBModel.tbQuestion tbQuestion { get; set; }

        //public LiXinModels.Examination.DBModel.tbQuestionSort tbQuestionSort { get; set; }
    }
}