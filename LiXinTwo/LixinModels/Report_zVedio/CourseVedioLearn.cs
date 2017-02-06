using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_zVedio
{
    public class CourseVedioLearn
    {
        #region Model
        /// <summary>
        /// 课程ID
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 关联试卷表ID-CoursepaperId
        /// </summary>
        public int CoursePaperId { get; set; }

        /// <summary>
        /// 是否有在线测试  0：否；1：是
        /// </summary>
        public int IsTest { get; set; }

        /// <summary>
        /// 是否有课后评估
        /// </summary>
        public int IsPing { get; set; }

        /// <summary>
        /// （如：0：课程；2：讲师，如0,ID;2,ID）；
        /// </summary>
        public string SurveyPaperId { get; set; }

        /// <summary>
        /// 所属年度
        /// </summary>
        public int YearPlan { get; set; }

        /// <summary>
        /// 参与学习人数(点击进入观看过课程的人数)
        /// </summary>
        public int LearnNumber { get; set; }

        /// <summary>
        /// 视频课程的参与人次
        /// </summary>
        public int ReallyNumber { get; set; }

        /// <summary>
        /// 应参加人数
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 通过人数
        /// </summary>
        public int passNumber { get; set; }


        public int IsOpenSub { get; set; }

        #endregion

        #region Extend
        /// <summary>
        /// 参加率 保留2位小数
        /// </summary>
        public string JoinRate
        {
            get
            {
                return  JoinRateDouble.ToString("p");
            }
        }


        /// <summary>
        /// 参加率 保留2位小数
        /// </summary>
        public double JoinRateDouble
        {
            get
            {
                return this.Number == 0 ? 0.00 : Math.Round(Convert.ToDouble(LearnNumber) / Convert.ToDouble(Number), 4, MidpointRounding.AwayFromZero) ;
            }
        }



        /// <summary>
        /// 在线测试通过率 保留2位小数
        /// </summary>
        public string PassRate
        {
            get
            {
                return PassRateDouble == -1 ? "N/A" : PassRateDouble .ToString("p");
            }
        }


        /// <summary>
        /// 在线测试通过率 保留2位小数
        /// </summary>
        public double PassRateDouble
        {
            get;

            set;
        }



        /// <summary>
        /// 课后评估综合的分数集合
        /// </summary>
        public string Survey_AllScore
        {
            get
            {
                return this.Survey_AllScoreDouble == -1 ? "N/A" : this.Survey_AllScoreDouble.ToString();
            }
        }

        /// <summary>
        /// 课程评估平均分
        /// </summary>
        public string Survey_CourseScore
        {
            get
            {
                return this.Survey_CourseScoreDouble == -1 ? "N/A" : this.Survey_CourseScoreDouble.ToString();
            }
        }

        /// <summary>
        /// 课程评估平均分
        /// </summary>
        public string Survey_TeacherScore
        {
            get
            {
                return this.Survey_TeacherScoreDouble == -1 ? "N/A" : this.Survey_TeacherScoreDouble.ToString();
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

        #endregion

        #region 分所
        /// <summary>
        /// 参与学习人数(点击进入观看过课程的人数) 分所
        /// </summary>
        public int fLearnNumber { get; set; }

        /// <summary>
        /// 通过人数 分所
        /// </summary>
        public int fPassNumber { get; set; }

        /// <summary>
        /// 视频课程的参与人次 分所
        /// </summary>
        public int fReallyNumber { get; set; }

        /// <summary>
        /// 应该参加的人数 分所
        /// </summary>
        public int fNumber { get; set; }

       

        /// <summary>
        /// 在线测试通过率 保留2位小数
        /// </summary>
        public string fPassRate
        {
            get
            {
                return fPassRateDouble == -1 ? "N/A" :  fPassRateDouble.ToString("p");
            }
        }


        /// <summary>
        /// 在线测试通过率 保留2位小数
        /// </summary>
        public double fPassRateDouble
        {
            get;

            set;
        }


        /// <summary>
        /// 参加率 保留2位小数
        /// </summary>
        public string fJoinRate
        {
            get
            {
                return fJoinRateDouble.ToString("p");
            }
        }


        /// <summary>
        /// 参加率 保留2位小数
        /// </summary>
        public double fJoinRateDouble
        {
            get
            {
                return this.fNumber == 0 ? 0.00 : Math.Round(Convert.ToDouble(fLearnNumber) / Convert.ToDouble(fNumber), 4, MidpointRounding.AwayFromZero);
            }
        }
        #endregion

    }
}
