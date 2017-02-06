using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_zVedio
{
    public class CourseJoin
    {
        #region model

        public int DeptID { get; set; }

        public string DeptName { get; set; }

        public string DeptPath { get; set; }

        /// <summary>
        /// 已参加人数
        /// </summary>
        public int JoinNumber { get; set; }

        /// <summary>
        /// 应参加人数
        /// </summary>
        public int SumNumber { get; set; }


        /// <summary>
        /// 人次
        /// </summary>
        public int LearnNumber { get; set; }

        /// <summary>
        /// 观看时间
        /// </summary>
        public decimal vedioTime { get; set; }


        public string  DeptIDs { get; set; }
        #endregion

        #region 扩展

        /// <summary>
        /// 累计观看时间（分钟）
        /// </summary>
        public decimal SumLookTime
        {
            get
            {
				return JoinNumber == 0 ? -1 : Math.Round(this.vedioTime / 60, 0);
            }
        }


        /// <summary>
        /// 平均观看时间（分钟）
        /// </summary>
        public double AvgLookTime { get; set; }


        /// <summary>
        /// 参加率 保留2位小数
        /// </summary>
        public string JoinRate
        {
            get
            {
				return  JoinRateDouble.ToString("p");
            }
        }


        /// <summary>
        /// 参加率 保留2位小数
        /// </summary>
        public double JoinRateDouble{get;set;}
        //{
        //    get
        //    {
        //        return this.SumNumber == 0 ? 0.00 : Math.Round(Convert.ToDouble(JoinNumber) / Convert.ToDouble(SumNumber) , 4, MidpointRounding.AwayFromZero);
        //    }
        //}



        /// <summary>
        /// 在线测试通过率 保留2位小数
        /// </summary>
        public string PassRate
        {
            get
            {
                return PassRateDouble == -1 ? "N/A" : PassRateDouble + "%";
            }
        }


        /// <summary>
        /// 在线测试通过率 保留2位小数
        /// </summary>
        public double PassRateDouble
        {
            get;

            set;
        }



        /// <summary>
        /// 人均观看次数
        /// </summary>
        public double LookNumbr { get; set; }

        /// <summary>
        /// 是否是总所 1是 0否
        /// </summary>
        public int IsMain
        {
            get
            {
                return this.allDeptPath.ToString().Contains("上海") ? 1 : 0;
            }
        }


        public string  allDeptPath { get; set; }
		/// <summary>
		/// 累计观看时间（分钟）
		/// </summary>
		public string SumLookTimeStr
		{
			get
			{
				return this.SumLookTime == -1 ? "N/A" : this.SumLookTime.ToString();
			}
		}


		/// <summary>
		/// 平均观看时间（分钟）
		/// </summary>
		public string AvgLookTimeStr
		{
			get
			{
				return this.AvgLookTime == -1 ? "N/A" : this.AvgLookTime.ToString();
			}
		}

		/// <summary>
		/// 人均观看次数
		/// </summary>
		public string LookNumbrStr
		{
			get
			{
				return this.LookNumbr == -1 ? "N/A" : this.LookNumbr.ToString();
			}
		}
        #endregion
    }
}
