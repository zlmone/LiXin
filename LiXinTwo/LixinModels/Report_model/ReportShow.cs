using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_model
{
    public class ReportShow
    {
        public int DepId { get; set; }

        public string Title { get; set; }
        /// <summary>
        /// 参与考核人数
        /// </summary>
        public int numberSum { get; set; }

        /// <summary>
        /// 免修人数
        /// </summary>
        public int freeSum { get; set; }

        /// <summary>
        /// 所内培训目标完成人数
        /// </summary>
        public int goalYesSum { get; set; }

        /// <summary>
        /// 所内培训目标未完成人数
        /// </summary>
        public int goalNoSum
        {
            get
            {
                var number = numberSum - goalYesSum;
                return number < 0 ? 0 : number;
            }
        }

        /// <summary>
        /// 目标学时完成人数
        /// </summary>
        public int PeriodYesSum { get; set; }

        /// <summary>
        /// 目标学时未完成人数
        /// </summary>
        public int PeriodNoSum
        {
            get
            {
                var number = numberSum - PeriodYesSum;
                return number < 0 ? 0 : number;
            }
        }

        /// <summary>
        /// 在线测试通过人数
        /// </summary>
        public int PaperYseSum { get; set; }

        /// <summary>
        /// 在线测试未通过人数
        /// </summary>
        public int PaperNoSum
        {
            get
            {
                var number = numberSum - PaperYseSum;
                return number < 0 ? 0 : number;
            }
        }

        /// <summary>
        /// 所内培训目标完成率
        /// </summary>
        public double lengthRate
        {
            get
            {
                if (goalYesSum == 0)
                {
                    return 0;
                }
                else
                {
                    if (numberSum == 0)
                    {
                        return 100;
                    }
                    else
                    {
                        return Math.Round((Convert.ToDouble(goalYesSum) * 100 / Convert.ToDouble(numberSum)), 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
        }

        public int type { get; set; }

        public string DeptIDs { get; set; }
    }
}
