using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewQueryStatistics
{
    public class CourseStatistics
    {
        public string CourseName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        /// <summary>
        /// 上课人数
        /// </summary>
        public int NumberofCourse { get; set; }

        /// <summary>
        /// 评价人数
        /// </summary>
        public int NumberofStatistics { get; set; }

        /// <summary>
        /// 总得分
        /// </summary>
        public int Sumscore { get; set; }

        /// <summary>
        /// 课程时间段
        /// </summary>
        public string CourseTime
        {
            get
            {
                return this.StartTime.ToString("yyyy-MM-dd HH:mm") + "~" + this.EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public int totalcount { get; set; }
    }
}
