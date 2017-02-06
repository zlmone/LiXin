using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;

namespace LiXinModels.Report_Together
{
    public class Report_UserLearnTogetherCourseShow
    {
        public int UserId { get; set; }

        public int DeptId { get; set; }
        public string DeptPath { get; set; }

        public string username { get; set; }

        public string Realname { get; set; }
        /// <summary>
        /// 培训级别
        /// </summary>
        public string TrainGrade { get; set; }
        /// <summary>
        /// 是否CPA 0-否 1-是
        /// </summary>
        public int IsCPA { get; set; }
        /// <summary>
        /// 是否CPA 0-否 1-是
        /// </summary>
        public string IsCPAStr { get { return IsCPA == 0 ? "否" : "是"; } }
        /// <summary>
        /// --报名类型 0-预订 1-指定 2-补预订 3-视频转播
        /// </summary>
        public int OrderType { get; set; }
        /// <summary>
        /// --报名类型 0-预订 1-指定 2-补预订 3-视频转播
        /// </summary>
        public string OrderTypeStr
        {
            get
            {
                string str = "";
                switch (OrderType)
                {
                    case 0:
                        str = "预订";
                        break;
                    case 1:
                        str = "指定";
                        break;
                    case 2:
                        str = "补预订";
                        break;
                    case 3:
                        str = "视频转播";
                        break;
                }
                return str;
            }
        }
        /// <summary>
        /// 请假类型 0-退订 1-请假 2-N/A
        /// </summary>
        public int LeaveType { get; set; }
        /// <summary>
        /// 请假类型 0-退订 1-请假 2-N/A
        /// </summary>
        public string LeaveTypeStr
        {
            get
            {
                string str = "";
                switch (LeaveType)
                {
                    case 0:
                        str = "退订";
                        break;
                    case 1:
                        str = "请假";
                        break;
                    case 2:
                        str = "N/A";
                        break;
                }
                return str;
            }
        }
        /// <summary>
        /// 视频转播时为99,非视频转播时：0：不考勤；1：只上课考勤；2：只下课考勤；3：上、下课都考勤
        /// </summary>
        public int AttFlag { get; set; }
        /// <summary>
        /// 课程和考勤时间
        /// </summary>
        public DateTime CourseStartTime { get; set; }
        public DateTime CourseEndTime { get; set; }
        public DateTime AttendceStartTime { get; set; }
        public DateTime AttendceEndTime { get; set; }
        /// <summary>
        /// 视频转播考勤状态 0：待考勤；1：正常;2:缺勤；3：迟到；4：早退；5：迟到且早退
        /// </summary>
        public int DepTranAttStatus { get; set; }

        /// <summary>
        /// 获得学时
        /// </summary>
        public decimal GetScore { get; set; }

        public decimal GetScoredouble
        {
            get
            {
                return (this.LeaveType == 0 || this.LeaveType == 1) ? -1 : GetScore < 0 ? 0 : GetScore;
            }
        }


        public string GetScoreStr
        {
            get
            {
                return this.GetScoredouble == -1 ? "N/A" : this.GetScoredouble.ToString();
            }
        }

        /// <summary>
        /// 课前建议  0-否 1-是
        /// </summary>
        public int IsCourseAdvice { get; set; }
        /// <summary>
        /// 课前建议  0-否 1-是
        /// </summary>
        public string IsCourseAdviceStr
        {
            get
            {
                if (this.LeaveType == 0 || this.LeaveType == 1)
                {
                    return "N/A";
                }
                return IsCourseAdvice == 0 ? "否" : "是";
            }
        }
        /// <summary>
        /// 课后评估  0-否 1-是
        /// </summary>
        public int IsSurveyReplyAnswer { get; set; }
        /// <summary>
        /// 课后评估  0-否 1-是
        /// </summary>
        public string IsSurveyReplyAnswerStr
        {
            get
            {
                if (this.LeaveType == 0 || this.LeaveType == 1)
                {
                    return "N/A";
                }
                return IsSurveyReplyAnswer == 0 ? "否" : "是";
            }
        }
        /// <summary>
        /// 是否担任讲师 =1时 是 ，!=1时 否
        /// 是否为讲师
        /// 0：不是
        /// 1：是（内部）
        /// 2：是（外部）
        /// </summary>
        public int IsTeacher { get; set; }
        /// <summary>
        /// 是否参与讲义编写 0-否 1-是 2-N/A
        /// </summary>
        public int IsResourceWrite { get; set; }

        /// <summary>
        /// 是否参与测试 0-否 1-是 2-N/A 
        /// </summary>
        public int IsDoExam { get; set; }
        /// <summary>
        /// 是否参与测试 0-否 1-是
        /// </summary>
        public string IsDoExamStr
        {
            get
            {
                if (this.LeaveType == 0 || this.LeaveType == 1)
                {
                    return "N/A";
                }
                return IsDoExam == 0 ? "否" : "是";
            }
        }
        public decimal GetExamScore { get; set; }
        /// <summary>
        /// 测试分数
        /// </summary>
        public string GetExamScoreStr { get { return IsDoExam == 0 ? "N/A" : GetExamScore.ToString("0.00"); } }
        public int totalCount { get; set; }

        /// <summary>
        /// 考勤状态0：待考勤或N/A；1：正常;2:缺勤；3：迟到；4：早退；5：迟到且早退
        /// </summary>
        public int AttStatus
        {
            get
            {
                if (AttFlag == 99)//视频转播
                {
                    return DepTranAttStatus;
                }
                if (AttFlag == 0)
                {
                    return 1;// "正常";
                }
                else if (AttFlag == 1)
                {
                    if (AttendceStartTime == DateTime.MinValue || AttendceStartTime == Convert.ToDateTime("2050-1-1"))
                    {
                        return 2;// "缺勤";
                    }
                    if (Convert.ToDateTime(AttendceStartTime.ToString("g")) > Convert.ToDateTime(CourseStartTime.ToString("g")))
                    {
                        return 3;// "迟到"
                    }
                    if (Convert.ToDateTime(AttendceStartTime.ToString("g")) <= Convert.ToDateTime(CourseStartTime.ToString("g")))
                    {
                        return 1;// "正常";
                    }
                }
                else if (AttFlag == 2)
                {
                    if (AttendceEndTime == DateTime.MinValue || AttendceEndTime == Convert.ToDateTime("2000-1-1"))
                    {
                        return 2;// "缺勤";
                    }
                    if (Convert.ToDateTime(AttendceEndTime.ToString("g")) < Convert.ToDateTime(CourseEndTime.ToString("g")))
                    {
                        return 4;// "早退";
                    }
                    if (Convert.ToDateTime(AttendceEndTime.ToString("g")) >= Convert.ToDateTime(CourseEndTime.ToString("g")))
                    {
                        return 1;// "正常";
                    }
                }
                else if (AttFlag == 3)
                {
                    if ((AttendceStartTime == DateTime.MinValue && AttendceEndTime == DateTime.MinValue) || (AttendceStartTime == Convert.ToDateTime("2050-1-1") &&
                        AttendceEndTime == Convert.ToDateTime("2000-1-1")))
                    {
                        return 2;// "缺勤";
                    }
                    if (AttendceStartTime == Convert.ToDateTime("2050-1-1") &&
                        AttendceEndTime != Convert.ToDateTime("2000-1-1"))
                    {
                        if (Convert.ToDateTime(AttendceEndTime.ToString("g")) >= Convert.ToDateTime(CourseEndTime.ToString("g")))
                        {
                            return 3;// "迟到";
                        }
                        return 5;// "迟到且早退";
                    }
                    if (AttendceStartTime != Convert.ToDateTime("2050-1-1") &&
                        AttendceEndTime == Convert.ToDateTime("2000-1-1"))
                    {
                        if (Convert.ToDateTime(AttendceStartTime.ToString("g")) <= Convert.ToDateTime(CourseStartTime.ToString("g")))
                        {
                            return 4;// "早退";
                        }
                        return 5;// "迟到且早退";
                    }
                    if (AttendceStartTime != Convert.ToDateTime("2050-1-1") &&
                        AttendceEndTime != Convert.ToDateTime("2000-1-1"))
                    {
                        if (Convert.ToDateTime(AttendceStartTime.ToString("g")) <= Convert.ToDateTime(CourseStartTime.ToString("g"))
                            && Convert.ToDateTime(AttendceEndTime.ToString("g")) >= Convert.ToDateTime(CourseEndTime.ToString("g")))
                        {
                            return 1;// "正常";
                        }
                        if (Convert.ToDateTime(AttendceStartTime.ToString("g")) > Convert.ToDateTime(CourseStartTime.ToString("g"))
                            && Convert.ToDateTime(AttendceEndTime.ToString("g")) > Convert.ToDateTime(CourseEndTime.ToString("g")))
                        {
                            return 3;// "迟到";
                        }
                        if (Convert.ToDateTime(AttendceStartTime.ToString("g")) > Convert.ToDateTime(CourseStartTime.ToString("g"))
                            && Convert.ToDateTime(AttendceEndTime.ToString("g")) < Convert.ToDateTime(CourseEndTime.ToString("g")))
                        {
                            return 5;// "迟到且早退";
                        }
                        if (Convert.ToDateTime(AttendceStartTime.ToString("g")) <= Convert.ToDateTime(CourseStartTime.ToString("g"))
                            && Convert.ToDateTime(AttendceEndTime.ToString("g")) < Convert.ToDateTime(CourseEndTime.ToString("g")))
                        {
                            return 4;// "早退";
                        }
                    }
                }
                return 0;// "正常";
            }
        }

        /// <summary>
        /// 考勤状态显示0：待考勤或N/A；1：正常;2:缺勤；3：迟到；4：早退；5：迟到且早退
        /// </summary>
        public string AttStatusStr
        {
            get
            {
                string str;
                if (this.LeaveType == 0 || this.LeaveType == 1)
                {
                    return "N/A";
                }
                switch (AttStatus)
                {
                    case 1:
                        str = "正常";
                        break;
                    case 2:
                        str = "缺勤";
                        break;
                    case 3:
                        str = "迟到";
                        break;
                    case 4:
                        str = "早退";
                        break;
                    case 5:
                        str = "迟到且早退";
                        break;
                    default:
                        str = "N/A";
                        break;
                }
                return str;
            }
        }

        public int IsFree { get; set; }
    }
}
