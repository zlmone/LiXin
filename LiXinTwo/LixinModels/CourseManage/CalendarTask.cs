using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.CourseManage
{
    public class CalendarTask
    {
        /// <summary>
        ///     日期的背景(0:其它月，1：当前月；2：有培训；3：当前天)
        /// </summary>
        public int Bg { set; get; }

        /// <summary>
        ///     本日任务安排数
        /// </summary>
        public double TaskTotal { set; get; }

        /// <summary>
        ///     年
        /// </summary>
        public int Year { set; get; }

        /// <summary>
        ///     月
        /// </summary>
        public int Month { set; get; }

        /// <summary>
        ///     号
        /// </summary>
        public int Day { set; get; }
        /// <summary>
        /// 当日上午的课程
        /// </summary>
        public string MoringStr { set; get; }
        /// <summary>
        /// 当日下午的课程
        /// </summary>
        public string AfterStr { set; get; }
        /// <summary>
        /// 1：视频转播；2：部门分所
        /// </summary>
        public int MFlag { set; get; }
        /// <summary>
        /// 1：视频转播；2：部门分所
        /// </summary>
        public int AFlag { set; get; }
    }
}
