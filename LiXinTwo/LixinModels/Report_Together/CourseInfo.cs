using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_Together
{
    public class CourseInfo
    {
        //CourseId, haveApply, reallyApply, reallyJoin, HaveJoin
        /// <summary>
        /// 课程ID
        /// </summary>
        public int CourseId { get; set; }

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
    }
}
