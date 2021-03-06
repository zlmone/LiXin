﻿using System;
using System.Collections.Generic;

namespace LiXinModels.Examination.DBModel
{
    /// <summary>
    ///     Examination User
    /// </summary>
    public class tbExamSendStudent
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public tbExamSendStudent()
        {
            StudentAnswerList = new List<ReStudentExamAnswer>();
        }

        /// <summary>
        ///     reID
        /// </summary>
        public int _id { set; get; }

        /// <summary>
        ///     RelationID Such as:
        ///     Examination:ExaminationID;
        /// SourceType=4 or SourceType=5:课程ID
        /// </summary>
        public int RelationID { set; get; }

        /// <summary>
        /// 关联试卷表ID-CoursepaperId
        /// </summary>
        public int CoursePaperId { set; get; }

        /// <summary>
        ///     SourceType
        ///     （0:Examination;1:课程考试）
        ///     0：原有的考试
        ///     1：集中课程的考试
        ///     2：视频课程的考试
        ///     3：新进员工的考试
        ///     4：课程转播的考试
        ///     5：部门分所的考试
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
        ///     （0:未做 ;1:未提交（进行中）;2:已提交;3：已批阅）
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
        ///     考试通过的基线（全部换算成标准分）
        /// </summary>
        public int PaperScore { set; get; }

        /// <summary>
        /// 记录是否获得学时  0:没有  1;记录   当为1的时候第2 3 4次考试就不在获得学时
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 学员答题时间 1分22,2.33,4.55
        /// </summary>
        public string AnswerTime { get; set; }

        /// <summary>
        ///     User Answer
        /// </summary>
        public List<ReStudentExamAnswer> StudentAnswerList { set; get; }
    }

    /// <summary>
    ///     User Answer
    /// </summary>
    public class ReStudentExamAnswer
    {
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
        ///     题序号
        /// </summary>
        public int Order { set; get; }

        /// <summary>
        ///     试题分值
        /// </summary>
        public int Score { set; get; }

        /// <summary>
        ///     本题所得分值
        /// </summary>
        public int GetScore { set; get; }

        /// <summary>
        ///     Evlution
        /// </summary>
        public string Evlution { set; get; }
    }
}