using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_zVedio
{
    public class VedioLearnSingle
    {
        #region model
        /// <summary>
        /// 课程ID3.
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 讲师名称
        /// </summary>
        public string Teacher { get; set; }

        /// <summary>
        /// 学时
        /// </summary>
        public decimal CourseLength { get; set; }

        /// <summary>
        /// 是否必修 0:必修；1：选修
        /// </summary>
        public int IsMust { get; set; }

        /// <summary>
        /// 是否在线测试 0：否；1：是
        /// </summary>
        public int IsTest { get; set; }

        /// <summary>
        /// 是否折算CPA学时
        /// </summary>
        public int IsCPA { get; set; }

        /// <summary>
        /// 已参加人数
        /// </summary>
        public int LearnNumber { get; set; }

        /// <summary>
        /// 开放总人数
        /// </summary>
        public int sumNumber { get; set; }

        /// <summary>
        /// 开放对象
        /// </summary>
        public string openObject { get; set; }

        /// <summary>
        /// 总部
        /// </summary>
        public int totalcount { get; set; }
        #endregion

        #region extend

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
        /// 是否有评估
        /// </summary>
        public string IsTestStr
        {
            get
            {
                return this.IsTest == 0 ? "N/A" : "是";
            }
        }

        /// <summary>
        /// 是否折算CPA学时
        /// </summary>
        public string IsCPAStr
        {
            get
            {
                return this.IsCPA == 0 ? "N/A" : "是";
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


        public string openObjectStr
        {
            get
            {
                return string.IsNullOrEmpty(this.openObject) ? "N/A" : this.openObject;
            }
        }

        #endregion

        #region 分所使用
        /// <summary>
        /// 分所参加的人数
        /// </summary>
        public int fLearNNumber { get; set; }

        /// <summary>
        /// 分所参加的人数
        /// </summary>
        public int fSumNNumber { get; set; }

        public string fNumber
        {
            get
            {
                return LearnNumber + "/" + fLearNNumber;
            }
        
        }
        #endregion
    }
}
