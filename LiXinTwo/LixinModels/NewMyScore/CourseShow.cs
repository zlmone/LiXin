using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;

namespace LiXinModels.NewMyScore
{
    /// <summary>
    /// 别再给我添加些乱七八糟的属性了。。。这个是我专用的骚年们 --by 神仙
    /// </summary>
    public class CourseShow
    {
        #region Model


        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 学时
        /// </summary>
        public int CourseLength { get; set; }

        /// <summary>
        /// 是否必修 0:必修；1：选修
        /// </summary>
        public int IsMust { get; set; }


        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Teacher { get; set; }

        /// <summary>
        /// 授课地点
        /// </summary>
        public string RoomName { get; set; }

        public int IsSystem { get; set; }

        /// <summary>
        /// 考勤得分
        /// </summary>		
        public decimal AttScore { get; set; }

        /// <summary>
        /// 课后评估得分
        /// </summary>		
        public decimal EvlutionScore { get; set; }

        /// <summary>
        /// 课后测试得分
        /// </summary>		
        public decimal ExamScore { get; set; }



        #endregion

        #region 扩展

        public string CourseNameStr
        {
            get
            {
                return CourseName.HtmlXssEncode();
            }
        }
        /// <summary>
        /// 获得的学时总和
        /// </summary>
        public decimal GetSumScore { get; set; }

        /// <summary>
        /// 考勤是否正常
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 考勤状态
        /// </summary>
        public string AttdenceFlag
        {
            get
            {
                var flag = "";
                switch (Status)
                {
                    case 0:
                        flag = "正常";
                        break;
                    case 1:
                        flag = "缺勤";
                        break;
                }
                return flag;
            }
        }

        /// <summary>
        /// 授课时间
        /// </summary>
        public string CourseTime
        {
            get
            {
                return StartTime.ToString("yyyy-MM-dd HH:mm") + " -- " + EndTime.ToString("yyyy-MM-dd HH:mm");
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
        /// 讲师
        /// </summary>
        public string TeacherStr
        {
            get
            {
                return string.IsNullOrEmpty(Teacher) ? "——" : Teacher;
            }
        }


        /// <summary>
        /// 框架内外
        /// </summary>
        public string IsSystemStr
        {
            get
            {
                return ((Enums.IsSystem)this.IsSystem).ToString();
            }
        }
        #endregion


    }
}
