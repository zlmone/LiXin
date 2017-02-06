using System;
using LiXinModels.CourseManage;
using System.Collections.Generic;
namespace LiXinModels
{
    /// <summary>
    /// Tr_MonthPlan:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Tr_MonthPlan
    {
        #region model
        /// <summary>
        /// Id
        /// </summary>		
        public int Id
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
        /// LastUpdateTime
        /// </summary>		
        public DateTime LastUpdateTime
        {
            get;
            set;
        }
        /// <summary>
        /// UserId
        /// </summary>		
        public int UserId
        {
            get;
            set;
        }
        /// <summary>
        /// PublishFlag
        /// </summary>		
        public int PublishFlag
        {
            get;
            set;
        }
        /// <summary>
        /// IsDelete
        /// </summary>		
        public int IsDelete
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 包含课程数
        /// </summary>
        public int courseCount
        {
            get;
            set;
        }

        public int totalcount
        {
            get;
            set;
        }

        public string PublishFlagStr
        {
            get
            {
                return ((Enums.PublishFlag)this.PublishFlag).ToString();
            }
        }

        public string RealName
        {
            get;
            set;
        }

        /// <summary>
        /// 年度预计课程数
        /// </summary>
        public int yearCourseCount
        {
            get;
            set;
        }

        /// <summary>
        /// 实际课程数
        /// </summary>
        public int monthCourseCount
        {
            get;
            set;
        }

        public List<Co_Course> newCourse
        {
            get;
            set;
        }

        public string AddCourse
        {
            get;
            set;
        }
    }


}

