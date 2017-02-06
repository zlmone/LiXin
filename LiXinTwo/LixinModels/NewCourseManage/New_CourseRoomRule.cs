using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewCourseManage
{
    ///<summary>
    ///New_CourseRoomRule
    ///</summary>
    public partial class New_CourseRoomRule
    {
        #region Model
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// CourseId
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// RoomId
        /// </summary>
        public int RoomId { get; set; }
        /// <summary>
        /// ClassIDs
        /// </summary>
        public string ClassIDs { get; set; }
        /// <summary>
        /// Rules
        /// </summary>
        public string Rules { get; set; }
        /// <summary>
        /// SeatDetail
        /// </summary>
        public string SeatDetail { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { set; get; }
        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { set; get; }

        public int TeacherId { get; set; }

        public int PingId { get; set; }
        /// <summary>
        /// 人数(应评人数)
        /// </summary>
        public int PersonCount { set; get; }
        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { set; get; }
        /// <summary>
        /// 是否考勤：（0：上课、下课都考勤1：仅上课考勤2：仅下课考勤3：不考勤）
        /// </summary>
        public int IsAttFlag { set; get; }

        /// <summary>
        /// 课后评估开放时间
        /// </summary>
        public decimal AfterEvlutionConfigTime { set; get; }

        #endregion Model

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 教室名称
        /// </summary>
        public string RoomName { get; set; }

        /// <summary>
        /// 讲师平均分=总分/(人数*问卷星评题题数)
        /// </summary>
        public decimal CourseTeacherAvg { get; set; }

        /// <summary>
        /// 课程平均分=总分/(人数*问卷星评题题数)
        /// </summary>
        public decimal CourseAvg { get; set; }

        /// <summary>
        /// 发布标志位
        /// </summary>
        public int PublicFlag { set; get; }

        /// <summary>
        /// 显示讲师平均分=总分/(人数*问卷星评题题数)
        /// </summary>
        public string CourseTeacherAvgStr { get { return CourseTeacherAvg.ToString("f2"); } }

        /// <summary>
        /// 显示课程平均分=总分/(人数*问卷星评题题数)
        /// </summary>
        public string CourseAvgStr { get { return CourseAvg.ToString("f2"); } }

        /// <summary>
        /// 总平均分=(显示讲师平均分+显示课程平均分)/2
        /// </summary>
        public string TotalCourseAvgStr { get { return ((CourseTeacherAvg + CourseAvg)/2).ToString("f2"); } }

        /// <summary>
        /// 已评人数
        /// </summary>
        public int HasPingUserCount { set; get; }

        public string StartTimeStr
        {
            get
            {
                return StartTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string EndTimeStr
        {
            get
            {
                return EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public string StatusStr
        {
            get
            {
                if (StartTime > DateTime.Now)
                    return "未开始";
                else if (StartTime <= DateTime.Now && EndTime >= DateTime.Now)
                    return "进行中";
                else
                    return "已结束";
            }
        }

        /// <summary>
        /// 课程时间显示
        /// </summary>
        public string CourseTimeStr
        {
            get
            {
                return StartTimeStr + "--" + EndTimeStr;
            }
        }

        public string ShortCourseTimeStr
        {
            get
            {
                return StartTime.ToString("MM-dd HH:mm") + "--" + EndTime.ToString("MM-dd HH:mm");
            }
        }
        

        /// <summary>
        /// Type
        /// </summary>
        public string TypeStr
        {
            get { return ((Enums.NewCourseType)Type).ToString(); }
        }
        public int totalcount
        {
            get;
            set;
        }

        public string teachername { get; set; }
    }
}
