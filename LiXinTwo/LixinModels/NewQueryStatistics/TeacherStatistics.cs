using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewQueryStatistics
{
    /// <summary>
    /// 讲师评价
    /// </summary>
    public class TeacherStatistics
    {
        public int userID { get; set; }

        public string Realname { get; set; }

        public int Sex { get; set; }

        /// <summary>
        /// 0正常 1：是（内部） 2：是（外部）
        /// </summary>
        public int IsTeacher { get; set; }

        /// <summary>
        /// 课程总数
        /// </summary>
        public int courseCount { get; set; }

        /// <summary>
        /// 已结束课程
        /// </summary>
        public int endCourseCount { get; set; }
        
        /// <summary>
        /// 应评
        /// </summary>
        public int ShouldCount { get; set; }
        /// <summary>
        /// 已评
        /// </summary>
        public int HasCount { get; set; }

        /// <summary>
        /// 综合平均分
        /// </summary>
        public int SumScore { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int totalcount { get; set; }

        public string teacherType
        {
            get
            {
                return ((Enums.IsTeacher)this.IsTeacher).ToString();
            }
        }

        public string SexStr
        {
            get
            {
                if (Sex == 0)
                    return "男";
                else if (Sex == 1)
                    return "女";
                else
                    return "保密";
            }
        }
    }
}
