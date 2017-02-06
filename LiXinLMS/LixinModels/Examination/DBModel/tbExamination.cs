using System;
using System.Collections.Generic;

namespace LiXinModels.Examination.DBModel
{
    /// <summary>
    ///     Examination'Examination
    ///     MongoDB Document of Examination
    /// </summary>
    public class tbExamination
    {
        /// <summary>
        /// 考试ID
        /// </summary>
        public int _id { set; get; }

        /// <summary>
        /// 试卷ID
        /// </summary>
        public int PaperID { set; get; }

        /// <summary>
        /// 考试名称
        /// </summary>
        public string ExaminationTitle { set; get; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int UserID { set; get; }

        /// <summary>
        /// 考试开始时间
        /// </summary>
        public DateTime ExamBeginTime { set; get; }

        /// <summary>
        /// 考试结束时间
        /// </summary>
        public DateTime ExamEndTime { set; get; }

        /// <summary>
        /// 考试时长
        /// </summary>
        public int ExamLength { set; get; }

        /// <summary>
        /// 是否百分制(0：是；1：否)
        /// </summary>
        public int PercentFlag { set; get; }

        /// <summary>
        /// 试卷显示方式(0:整体显示;1:单题可逆；2：单题不可逆)
        /// </summary>
        public int ShowType { set; get; }

        /// <summary>
        /// 允许进入的次数
        /// </summary>
        public int TestTimes { set; get; }

        /// <summary>
        /// 是否随机(0:否;1:是;)
        /// </summary>
        public int RadomOrderFlag { set; get; }

        /// <summary>
        /// 是否发布（0:未发布; 1:已发布）
        /// </summary>
        public int PublishedFlag { set; get; }

        /// <summary>
        /// 及格分数
        /// </summary>
        public double PassScore { get; set; }

        /// <summary>
        /// 是否发送信息(0:站内信 1:Email 2:两种方式)
        /// </summary>
        public int SendMessageFlag { set; get; }

        /// <summary>
        /// 考试规则
        /// </summary>
        public string ExamRules { set; get; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { set; get; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 审批人
        /// </summary>
        public List<int> ApprovalUser { set; get; }

        /// <summary>
        /// 是否发布成绩 0：否,1：是
        /// </summary>
        public int PublishResult { get; set; }

        /// <summary>
        /// 状态 0:正常;1:删除
        /// </summary>
        public int Status { set; get; }
    }
}