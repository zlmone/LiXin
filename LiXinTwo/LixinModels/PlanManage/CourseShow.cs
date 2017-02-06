using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    [Serializable]
    public class CourseShow
    {
        #region model
        /// <summary>
        /// 课程表主键
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName
        {
            get;
            set;
        }

        /// <summary>
        /// 课程编号
        /// </summary>
        public string Code
        {
            get;
            set;
        }


        /// <summary>
        /// Year
        /// </summary>
        public int Year
        {
            get;
            set;
        }
        /// <summary>
        /// Month
        /// </summary>
        public string Month
        {
            get;
            set;
        }
        /// <summary>
        /// Day
        /// </summary>
        public int Day
        {
            get;
            set;
        }
        /// <summary>
        /// 开放级别
        /// </summary>
        public string OpenLevel
        {
            get;
            set;
        }


        /// <summary>
        /// Way
        /// </summary>
        public int Way
        {
            get;
            set;
        }
        /// <summary>
        /// Teacher
        /// </summary>
        public string Teacher
        {
            get;
            set;
        }

        /// <summary>
        /// 是否必须
        /// </summary>
        public int IsMust
        {
            get;
            set;
        }

        public int IsCPA
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 薪酬级别
        /// </summary>
        private string _PayGrade;
        public string PayGrade
        {
            get
            {
                return string.IsNullOrEmpty(this._PayGrade) ? "--" : _PayGrade;
            }
            set
            {
                _PayGrade = value;
            }
        }



        /// <summary>
        /// 讲师姓名
        /// </summary>
        private string _Realname;
        public string Realname
        {
            get
            {
                return string.IsNullOrEmpty(this._Realname) ? "--" : _Realname;
            }
            set
            {
                _Realname = value;
            }
        }


        /// <summary>
        ///  是否框架体系的课程
        /// </summary>
        public int IsSystem
        {
            get;
            set;
        }

        public DateTime PreCourseTime
        {
            get;
            set;
        }

        /// <summary>
        /// 年度计划的课程ID
        /// </summary>
        public string SurveyPaperId
        {
            get;
            set;
        }

        public decimal CourseLength
        {
            get;
            set;
        }

        public string RoomName
        {
            get;
            set;
        }
        #endregion

        public decimal sumLength { get; set; }

        public string RoomNamestr
        {
            get
            {
                if (string.IsNullOrEmpty(this.RoomName))
                {
                    return "--";
                }
                return this.RoomName;
            }
        }


        public string IsCPAstr
        {
            get
            {
                if (this.IsCPA == 0)
                {
                    return "否";
                }
                return "是";
            }
        }

        /// <summary>
        /// 框架类别 级别1>级别2......
        /// </summary>
        private string _SystemTree;
        public string SystemTree
        {
            get
            {
                return string.IsNullOrEmpty(this._SystemTree) ? "--" : _SystemTree;
            }
            set
            {
                _SystemTree = value;
            }
        }

        /// <summary>
        /// 预计开课时间
        /// </summary>
        public string OpenTime
        {
            get
            {
                return (this.Month == null ? "" : this.Month.Replace("-", "年")) + "月" + ((Enums.Day)this.Day).ToString();
            }
        }


        /// <summary>
        /// 授课形式
        /// </summary>
        public string WayStr
        {
            get
            {
                return ((Enums.Way)this.Way).ToString();
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
        /// 是否必修
        /// </summary>
        public string IsCPAStr
        {
            get
            {
                return ((Enums.IsCPA)this.IsCPA).ToString();
            }
        }

        /// <summary>
        /// 框架内外
        /// </summary>
        public string IsSystemStr
        {
            get
            {
                return this.IsSystem > 0 ? "内" : "外";
            }
        }

        public int totalcount
        {
            get;
            set;
        }

        /// <summary>
        /// 0未变  1删除  2新增
        /// </summary>
        public string DeleteOrNew
        {
            get;
            set;
        }


        public string PreCourseTimeStr
        {
            get
            {
                return this.PreCourseTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        /// <summary>
        /// 开放级别的更改
        /// </summary>
        public string OpenLevelUpdate
        {
            get;
            set;
        }

        /// <summary>
        ///授课讲师的更改
        /// </summary>
        public string teacherUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// IsYearPlan
        /// </summary>
        public int IsYearPlan { get; set; }
    }
}
