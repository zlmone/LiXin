using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Survey;

namespace LiXinModels.NewCourseManage
{
    ///<summary>
    ///New_Course
    ///</summary>
    public partial class New_Course
    {
        #region Model
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// CourseName
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// IsGroupTeach
        /// </summary>
        public int IsGroupTeach { get; set; }
        /// <summary>
        /// Teachers
        /// </summary>
        public string Teachers { get; set; }
        /// <summary>
        /// Classes
        /// </summary>
        public string Classes { get; set; }
        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// IsPingTeacher
        /// </summary>
        public string IsPingTeacher { get; set; }
        /// <summary>
        /// ScoreDistribute
        /// </summary>
        public string ScoreDistribute { get; set; }
        /// <summary>
        /// Memo
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// IsPingCourse
        /// </summary>
        public int IsPingCourse { get; set; }
        /// <summary>
        /// GStartTime
        /// </summary>
        public string GStartTime { get; set; }
        /// <summary>
        /// GType
        /// </summary>
        public int GType { get; set; }
        /// <summary>
        /// GGroupNumber
        /// </summary>
        public int GGroupNumber { get; set; }
        /// <summary>
        /// GGroupPersonCount
        /// </summary>
        public int GGroupPersonCount { get; set; }
        /// <summary>
        /// GGroupRule
        /// </summary>
        public string GGroupRule { get; set; }
        /// <summary>
        /// 删除标志（0：否；1：是）
        /// </summary>
        public int IsDelete { set; get; }
        /// <summary>
        /// 课程类型（1：集中；2：自学）
        /// </summary>
        public int Way { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime LastUpdateTime { set; get; }
        /// <summary>
        /// 发布标志位
        /// </summary>
        public int PublicFlag { set; get; }
        /// <summary>
        /// 在线测试（0：否；0：是）
        /// </summary>
        public int IsTest { set; get; }
        /// <summary>
        /// 视频观看最低时限
        /// </summary>
        public int VideoLowLength { set; get; }
        
        #endregion Model

        /// <summary>
        /// 讲师集合
        /// </summary>
        public string teacher { get; set; }

        /// <summary>
        /// 班级集合
        /// </summary>
        public string classnames { get; set; }

        /// <summary>
        /// 教室集合
        /// </summary>
        public string roomnames { get; set; }
        //课程状态
        public string StatusShow
        {
            get
            {
                if (PublicFlag == 1)
                {
                    if (DateTime.Now >= StartTime && DateTime.Now <= EndTime)
                        return "进行中";
                    else if (DateTime.Now < StartTime)
                        return "未开始";
                    else
                        return "已结束";
                }else
                {
                    return "未发布";
                }
            }
        }

        public int totalcount
        {
            get;
            set;
        }

        public string StartTimeStr
        {
            get { return StartTime == Convert.ToDateTime("2000-1-1") ? "——" : StartTime.ToString("yyyy-MM-dd HH:mm"); } 
        }

        public string EndTimeStr
        {
            get { return EndTime == Convert.ToDateTime("2050-1-1") ? "——" : EndTime.ToString("yyyy-MM-dd HH:mm"); }
        }

        /// <summary>
        /// 课程总人数
        /// </summary>
        public int UserCount { get; set; }

        /// <summary>
        /// 课后评估，问题与选项
        /// </summary>
        public Survey_Exampaper AfterCourseAssess { get; set; }

     

        #region == 学员对讲师评价使用 add by yxt 2013-07-06==
        /// <summary>
        /// --上课人数
        /// </summary>
        public int LearnCoursePersonCount  { get; set; }

        /// <summary>
        /// --已评价人数
        /// </summary>
        public int HadPingPersonCount  { get; set; }

        /// <summary>
        /// 课程时间显示
        /// </summary>
        public string CourseTimeStr
        {
            get
            {
                return StartTimeStr+"--"+EndTimeStr;
            }
        }
        #endregion
    }
}
