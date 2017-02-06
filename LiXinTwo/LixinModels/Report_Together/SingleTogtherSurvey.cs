using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_Together
{
    public class SingleTogtherSurvey
    {

        public int type { get; set; }

        public int UserId { get; set; }

        public int DeptId { get; set; }

        public string DeptIds { get; set; }

        public string DeptName { get; set; }

        /// <summary>
        /// 应参加评估人数
        /// </summary>
        public int HaveJoin { get; set; }



        /// <summary>
        /// 实际参加
        /// </summary>
        public int reallyJoin { get; set; }

        /// <summary>
        /// 实际参加评估人数
        /// </summary>
        public int SurveyNumber { get; set; }

        /// <summary>
        /// 课程评估参与率
        /// </summary>
        public double SurveyJoinRate { get; set; }

        /// <summary>
        /// 课程评估平均分
        /// </summary>
        public double Survey_CourseScoreDouble { get; set; }

        /// <summary>
        /// 课程评估平均分
        /// </summary>
        public double Survey_TeacherScoreDouble { get; set; }


        /// <summary>
        /// 应参加评估人数
        /// </summary>
        public string HaveJoinStr
        {
            get
            {
                return this.HaveJoin == -1 ? "N/A" : this.HaveJoin.ToString();
            }
        }


        /// <summary>
        /// 实际参加评估人数
        /// </summary>
        public string SurveyNumberStr
        {
            get
            {
                return this.SurveyNumber == -1 ? "N/A" : this.SurveyNumber.ToString();
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
        /// 课程评估平均分
        /// </summary>
        public double SumSurvey_CourseScore { get; set; }

        /// <summary>
        /// 课程评估平均分
        /// </summary>
        public double SumSurvey_TeacherScore { get; set; }

        /// <summary>
        /// 课程评估参与率
        /// </summary>
        public string SurveyJoinRateStr
        {
            get
            {
                return this.SurveyJoinRate == -1 ? "N/A" : this.SurveyJoinRate.ToString("P");
            }
        }

        /// <summary>
        /// 进入考试的次数
        /// </summary>
        public int TestTimes { get; set; }
        /// <summary>
        /// 为1 参加了考试 0 没有
        /// </summary>
        public int IsTest { get; set; }

        /// <summary>
        /// 为1 通过考试 0 没有
        /// </summary>
        public int IsPass { get; set; }

        /// <summary>
        /// 测试应参与人数
        /// </summary>
        public int ExamHaveJoin { get; set; }

        /// <summary>
        /// 测试实际参与人数
        /// </summary>
        public int ExamJoin { get; set; }

        /// <summary>
        /// 测试的通过人数
        /// </summary>
        public int ExmaPass { get; set; }

        /// <summary>
        /// 测试的参与率
        /// </summary>
        public double ExamJoinRate { get; set; }

        /// <summary>
        /// 学员答题时间
        /// </summary>
        public double AnswerTime { get; set; }

        /// <summary>
        /// 平均答题次数
        /// </summary>
        public double avgAnswerTimes { get; set; }


        /// <summary>
        ///  平均得分
        /// </summary>
        public double avgExamScore { get; set; }

        /// <summary>
        /// 测试的通过率
        /// </summary>
        public double ExamPassRate { get; set; }

        public int IsMain { get; set; }


        /// <summary>
        /// 测试应参与人数
        /// </summary>
        public string ExamHaveJoinStr
        {
            get
            {
                return this.ExamHaveJoin == -1 ? "N/A" : this.ExamHaveJoin.ToString();
            }
        }

        /// <summary>
        /// 测试实际参与人数
        /// </summary>
        public string ExamJoinStr
        {
            get
            {
                return this.ExamJoin == -1 ? "N/A" : this.ExamJoin.ToString();
            }
        }

        /// <summary>
        /// 测试的参与率
        /// </summary>
        public string ExamJoinRateStr
        {
            get
            {
                return this.ExamJoinRate == -1 ? "N/A" : this.ExamJoinRate.ToString("P");
            }
        }


        /// <summary>
        /// 测试的通过率
        /// </summary>
        public string ExamPassRateStr
        {
            get
            {
                return this.ExamJoinRate == -1 ? "N/A" : this.ExamPassRate.ToString("P");
            }
        }

        /// <summary>
        /// 学员答题时间
        /// </summary>
        public string AnswerTimeStr
        {
            get
            {
                return this.AnswerTime == -1 ? "N/A" : this.AnswerTime.ToString("0.00");
            }
        }
        /// <summary>
        /// 平均答题次数
        /// </summary>
        public string avgAnswerTimesStr
        {
            get
            {
                return this.avgAnswerTimes == -1 ? "N/A" : this.avgAnswerTimes.ToString("0.00");
            }
        }


        /// <summary>
        ///  平均得分
        /// </summary>
        public string avgExamScoreStr
        {
            get
            {
                return this.avgExamScore == -1 ? "N/A" : this.avgExamScore.ToString();
            }
        }


    }
}
