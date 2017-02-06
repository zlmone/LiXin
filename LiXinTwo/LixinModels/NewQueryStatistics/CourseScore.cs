using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewQueryStatistics
{
    public class CourseScore
    {
        public string CourseName { get; set; }


        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }


        public string TimeStr
        {
            get
            {
                return this.StartTime.ToString("yyyy-MM-dd HH:mm") + "——" + this.EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        /// <summary>
        /// 实际考勤
        /// </summary>
        public int HaveAttend { get; set; }


        /// <summary>
        /// 应考勤
        /// </summary>
        public int AttendCount { get; set; }


        /// <summary>
        /// 分组总和 
        /// </summary>
        public double GroupScore { get; set; }
        /// <summary>
        /// 集中总和 
        /// </summary>
        public double TogetherScore { get; set; }

        /// <summary>
        /// 课程考试
        /// </summary>
        public double CourseExamScore { get; set; }


        /// <summary>
        /// 奖励总和
        /// </summary>
        public double IsReward { get; set; }

        public string RewardStr
        {
            get
            {
                return this.IsReward > 0 ? "课程已评估" : "课程未评估";
            }
        }
    }
}
