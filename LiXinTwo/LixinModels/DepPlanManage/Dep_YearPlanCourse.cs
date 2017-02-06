using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepPlanManage
{
    ///<summary>
    ///Dep_YearPlanCourse
    ///</summary>
    public partial class Dep_YearPlanCourse
    {
        #region Model
        /// <summary>
        /// YearId
        /// </summary>
        public int YearId { get; set; }
        /// <summary>
        /// CourseId
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// IsSystem
        /// </summary>
        public int IsSystem { get; set; }
        #endregion Model

        #region 课程model
        /// <summary>
        /// 课程表主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 课程代码
        /// </summary>
        public string Code
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
        /// CourseLength
        /// </summary>
        public decimal CourseLength
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

        /// <summary>
        /// 是否折算CPA
        /// </summary>
        public int IsCPA
        {
            get;
            set;
        }

        /// <summary>
        /// 薪酬级别
        /// </summary>
        public string PayGrade
        {
            get;
            set;
        }

        /// <summary>
        /// 讲师姓名
        /// </summary>
        public string Realname
        {
            get;
            set;
        }

        /// <summary>
        /// 框架类别 级别1>级别2......
        /// </summary>
        public string SystemTree
        {
            get;
            set;
        }

        /// <summary>
        /// 人员删除标识
        /// </summary>
        public int IsDelete { get; set; }
        #endregion Model

        public int totalcount { get; set; }

        /// <summary>
        /// IsYearPlan
        /// </summary>
        public int IsYearPlan { get; set; }

        /// <summary>
        /// 预计开课时间
        /// </summary>
        public string OpenTime
        {
            get
            {
                string yue = "";
                if (Month.IndexOf("-") > -1)
                {
                    yue = Month.Replace("-", "年");
                    if (yue.IndexOf("年0") > -1)
                    {
                        yue = yue.Replace("年0", "年");
                    }
                }
                return yue + "月" + ((Enums.Day)this.Day).ToString();
            }
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealnameStr
        {
            get
            {
                if (string.IsNullOrEmpty(Realname) || IsDelete == 1)
                {
                    return "";
                }
                else
                {
                    return Realname;
                }
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
        /// 是否折算CPA
        /// </summary>
        public string IsCPAStr
        {
            get
            {
                return ((Enums.IsCPA)this.IsCPA).ToString();
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
        /// 框架内外
        /// </summary>
        public string IsSystemStr
        {
            get
            {
                if (IsSystem > 0)
                {
                    return "内";
                }
                else
                {
                    return "外";
                }
            }
        }
        /// <summary>
        /// 薪酬
        /// </summary>
        public string PayGradeStr
        {
            get
            {
                if (string.IsNullOrEmpty(PayGrade) || IsDelete == 1)
                {
                    return "";
                }
                else
                {
                    return PayGrade;
                }
            }
        }

        /// <summary>
        /// 框架类别 级别1>级别2......
        /// </summary>
        public string SystemTreeStr
        {
            get
            {
                if (string.IsNullOrEmpty(SystemTree) || IsDelete == 1)
                {
                    return "";
                }
                else
                {
                    return SystemTree;
                }
            }
        }
    }
}
