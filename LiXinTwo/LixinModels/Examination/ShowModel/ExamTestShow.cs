using System;

namespace LiXinModels.Examination.ShowModel
{
    public class ExamTestShow
    {
        /// <summary>
        ///     考试ID
        /// </summary>
        public int ExamID { set; get; }

        /// <summary>
        ///     考试参与者ID
        /// </summary>
        public int ExamUserID { set; get; }

        /// <summary>
        ///     考试名称
        /// </summary>
        public string ExamTitle { set; get; }

        /// <summary>
        ///     考试开始时间
        /// </summary>
        public DateTime StartTime { set; get; }

        /// <summary>
        ///     考试开始时间(显示)
        /// </summary>
        public string ExamTime
        {
            get { return StartTime.ToString("yyyy-MM-dd HH:mm") + "至" + EndTime.ToString("yyyy-MM-dd HH:mm"); }
        }

        /// <summary>
        ///     考试结束时间
        /// </summary>
        public DateTime EndTime { set; get; }

        /// <summary>
        ///     考试时长
        /// </summary>
        public int ExamLength { set; get; }

        /// <summary>
        ///     考试分数
        /// </summary>
        public string ExamScore { set; get; }

        /// <summary>
        ///     测试次数
        /// </summary>
        public string ExamTestTimes { set; get; }

        /// <summary>
        ///     所得成绩
        /// </summary>
        public string GetScore { set; get; }

        /// <summary>
        ///     是否通过（0：否；1：是）
        /// </summary>
        public int PassFlag { set; get; }

        /// <summary>
        ///     名次
        /// </summary>
        public string ScoreOrder { set; get; }

        /// <summary>
        ///     状态标志（0：未作，1：暂存：2：提交）
        /// </summary>
        public int DoExamStatus { set; get; }

        /// <summary>
        ///     试卷ID
        /// </summary>
        public int ExampaperID { set; get; }

        /// <summary>
        ///     试卷sortID
        /// </summary>
        public int ExampaperSortID { set; get; }


        //额外字段
        public int pFlag { set; get; } //参数
        //查询的数据
        public int examStatus
        {
            get
            {
                if (StartTime >= DateTime.Now)
                    return 0;
                if (StartTime <= DateTime.Now && EndTime >= DateTime.Now)
                    return 1;
                if (EndTime < DateTime.Now && DoExamStatus >= 2 &&
                    (pFlag == 0 || (pFlag == 1 ? PassFlag == 1 : PassFlag == 0)))
                    return 2;
                if (EndTime < DateTime.Now && DoExamStatus < 2)
                    return 3;
                return 3;
            }
        }

        public string Status
        {
            get { return PassFlag == 1 ? "通过" : (PassFlag == 0 ? "未通过" : "--"); }
        }
    }
}