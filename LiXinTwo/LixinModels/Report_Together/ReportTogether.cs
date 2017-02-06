using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_Together
{
    public class ReportTogether
    {
        #region model

        public int YearPlan { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        public int CourseId { get; set; }

        public string OpenLevel { get; set; }

        /// <summary>
        /// 指定/预定 0:无；1：预定；2：指定；3：两者都有
        /// </summary>
        public int IsOrder { get; set; }

        /// <summary>
        /// 实际报名人数
        /// </summary>
        public int reallyApply { get; set; }

        /// <summary>
        ///应报名人数
        /// </summary>
        public int haveApply { get; set; }

        /// <summary>
        /// 实际参与课程的人数
        /// </summary>
        public int reallyJoin { get; set; }

        /// <summary>
        /// 应参加课程的人数
        /// </summary>
        public int HaveJoin { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 获取的学时
        /// </summary>
        public Decimal GetScore { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptIds { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptPath { get; set; }

        /// <summary>
        /// 0 总所  1部门
        /// </summary>
        public int IsMain { get; set; }

        /// <summary>
        /// 保存的调查ID 如：0：课程；2：讲师，如0,ID;2,ID
        /// </summary>
        public string SurveyPaperId { get; set; }

        /// <summary>
        /// 参与课后评估的人数
        /// </summary>
        public int surveyNumber { get; set; }

        /// <summary>
        /// 参与课后评估的人数
        /// </summary>
        public int SumsurveyNumber { get; set; }

        /// <summary>
        /// 测试通过人数
        /// </summary>
        public int ExamPass { get; set; }

        /// <summary>
        ///  测试参与人数
        /// </summary>
        public int ExamJoin { get; set; }

        /// <summary>
        ///  测试参与人数
        /// </summary>
        public int SumExamJoin { get; set; }

        /// <summary>
        /// 视频得分
        /// </summary>
        public decimal GetSumScore { get; set; }

        /// <summary>
        ///审批
        /// </summary>
        public int ApprovalFlag { get; set; }


        /// <summary>
        /// 部门群组ID
        /// </summary>
        public int DepartSetId { get; set; }

        public int IsPing { get; set; }

        /// <summary>
        /// 课程信息基本
        /// </summary>
        public List<CourseInfo> CourseInfoList { get; set; }

        /// <summary>
        /// 学员获取学分的情况
        /// </summary>
        public List<decimal> UserScoreList { get; set; }

        // IsMust, StartTime, EndTime, Teacher, Realname, PayGrade
        public string courseName { get; set; }

        public int IsMust { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Teacher { get; set; }

        public string Realname { get; set; }

        public string PayGrade { get; set; }

        /// <summary>
        /// 是否可折算CPA学时 0：否；1：是
        /// </summary>
        public int IsCPA { get; set; }

        /// <summary>
        /// 是否在线测试 0：否；1：是
        /// </summary>
        public int IsTest { get; set; }

        public int IsDelete { get; set; }
        public int Publishflag { get; set; }

     
        #endregion

        #region Extend


        /// <summary>
        /// 课程报名率
        /// </summary>
        public double ApplyRate
        {
            get
            {
                //if (IsOrder == 1 || IsOrder == 3)
                //{
                return haveApply == 0 ? 0 : Math.Round(Convert.ToDouble(reallyApply) / Convert.ToDouble(haveApply), 4, MidpointRounding.AwayFromZero);
                //}
                //return -1;
            }
        }

        /// <summary>
        /// 课程报名率
        /// </summary>
        public string ApplyRateStr
        {
            get
            {
                return ApplyRate == -1 ? "N/A" : ApplyRate.ToString("p");
            }
        }

        /// <summary>
        /// 课程参与率
        /// </summary>
        public double JoinRate
        {
            get
            {

                return reallyApply == 0 ? 0 : Math.Round(Convert.ToDouble(reallyJoin) / Convert.ToDouble(reallyApply), 4, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// 课程参与率
        /// </summary>
        public string JoinRateStr
        {
            get
            {
                return JoinRate.ToString("p");
            }
        }



        /// <summary>
        /// 课程评估参与率
        /// </summary>
        public double SurveyRate
        {
            get
            {
                return this.SumsurveyNumber == 0 ? 0 : Math.Round(Convert.ToDouble(surveyNumber) / Convert.ToDouble(SumsurveyNumber), 4, MidpointRounding.AwayFromZero);
            }
        }


        /// <summary>
        /// 课程评估参与率
        /// </summary>
        public string SurveyRateStr
        {
            get
            {
                return SurveyRate.ToString("p");
            }
        }

        /// <summary>
        /// 测试参与率
        /// </summary>
        public double ExamJoinRate
        {
            get
            {
                return this.SumExamJoin == 0 ? 0 : Math.Round(Convert.ToDouble(ExamJoin) / Convert.ToDouble(SumExamJoin), 4, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// 测试参与率
        /// </summary>
        public string ExamJoinRateStr
        {
            get
            {
                return ExamJoinRate.ToString("p");
            }
        }

        /// <summary>
        /// 测试通过率
        /// </summary>
        public double ExamPassRate
        {
            get
            {
                return this.ExamJoin == 0 ? 0 : Math.Round(Convert.ToDouble(ExamPass) / Convert.ToDouble(ExamJoin), 4, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// 测试通过率
        /// </summary>
        public string ExamPassRateStr
        {
            get
            {
                return ExamPassRate.ToString("p");
            }
        }

        /// <summary>
        /// 通过转播所获得的学分
        /// </summary>
        public decimal GetRtScore
        {
            get
            {
                if (this.ApprovalFlag == 1)
                {
                    return GetSumScore < 0 ? 0 : GetSumScore;
                }
                return 0;
            }

        }

        public string GetScoreStr
        {
            get
            {
                return this.GetScore.ToString("0.00");
            }
        }
        #endregion

        #region 课程扩展

        public string surveyStr { get; set; }
        /// <summary>
        /// 讲师姓名
        /// </summary>
        public string teacher
        {
            get
            {
                return string.IsNullOrEmpty(Realname) ? "--" : Realname;
            }
        }

        /// <summary>
        /// 薪酬级别
        /// </summary>
        public string PayGradeStr
        {
            get
            {
                return string.IsNullOrEmpty(PayGrade) ? "--" : PayGrade;
            }
        }


        /// <summary>
        /// 课后评估综合的分数集合
        /// </summary>
        public double Survey_AllScoreDouble { get; set; }

        /// <summary>
        /// 课程评估平均分
        /// </summary>
        public double Survey_CourseScoreDouble { get; set; }

        /// <summary>
        /// 课程评估平均分
        /// </summary>
        public double Survey_TeacherScoreDouble { get; set; }

        /// <summary>
        /// 课后评估综合的分数集合
        /// </summary>
        public string Survey_AllScore
        {
            get
            {
                return this.Survey_AllScoreDouble == -1 ? "N/A" : this.Survey_AllScoreDouble.ToString("0.00");
            }
        }

        /// <summary>
        /// 课程评估平均分
        /// </summary>
        public string Survey_CourseScore
        {
            get
            {
                return this.Survey_CourseScoreDouble == -1 ? "N/A" : this.Survey_CourseScoreDouble.ToString("0.00");
            }
        }

        /// <summary>
        /// 课程评估平均分
        /// </summary>
        public string Survey_TeacherScore
        {
            get
            {
                return this.Survey_TeacherScoreDouble == -1 ? "N/A" : this.Survey_TeacherScoreDouble.ToString("0.00");
            }
        }

        /// <summary>
        /// 未参与评估的人数
        /// </summary>
        public int NOSurvey
        {
            get
            {
                return surveyNumber == -1 ? -1 : (reallyJoin == 0 ? 0 : reallyJoin - surveyNumber);
            }
        }

        /// <summary>
        /// 是否必修
        /// </summary>
        public string IsMustStr
        {
            get
            {
                return ((Enums.IsMust)this.IsMust).ToString();
            }
        }

        /// <summary>
        /// 开课时间
        /// </summary>
        public string CourseTime
        {
            get
            {
                return StartTime.ToString("yyyy-MM-dd HH:mm") + " -- " + EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
        #endregion


    }

}
