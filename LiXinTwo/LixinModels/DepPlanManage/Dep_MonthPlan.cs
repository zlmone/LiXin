using LiXinModels.CourseManage;
using LiXinModels.DepCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepPlanManage
{
    [Serializable]
    public class Dep_MonthPlan
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

        public int DeptId { get; set; }
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

        public List<Dep_Course> newCourse
        {
            get;
            set;
        }

        public string AddCourse
        {
            get;
            set;
        }

        /// <summary>
        /// 1 我的  0不是我的
        /// </summary>
        public int isMy { get; set; }

        public string DeptName { get; set; }
    }
}
