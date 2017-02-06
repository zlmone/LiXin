using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_CPA
{
    public class CPACompelete
    {
        /// <summary>
        /// 仅显示到部门
        /// </summary>
        public string deptName { get; set; }

        /// <summary>
        /// 参加职业道德的人数
        /// </summary>
        public int JoinDaodeNumber { get; set; }

        /// <summary>
        /// 部门ID集合
        /// </summary>
        public string DeptIDs { get; set; }

        /// <summary>
        /// 是否是总部 1 是 0 否
        /// </summary>
        public int IsMain { get; set; }

        /// <summary>
        /// 参与考核的人数
        /// </summary>
        public int joinCheckNumber { get; set; }

        /// <summary>
        /// 免修人数
        /// </summary>
        public int freeCheckNumber { get; set; }

        /// <summary>
        /// 年度目标学时完成人数
        /// </summary>
        public int yearComplete { get; set; }

        //是否是最后一年
        public int IsLast { get; set; }
        /// <summary>
        /// 年度目标学时未完成人数
        /// </summary>
        public int yearNoComplete
        {
            get
            {
                var number = joinCheckNumber  - yearComplete;
                return number < 0 ? 0 : number;
            }
        }


        /// <summary>
        /// 本培训周期学时目标完成人数
        /// </summary>
        public int zhouqiComplete { get; set; }

        /// <summary>
        /// 本培训周期学时目标未完成人数
        /// </summary>
        public int zhouqiNoComplete
        {
            get
            {
                var number = joinCheckNumber - zhouqiComplete;
                return number < 0 ? 0 : number;
            }
        }


        /// <summary>
        /// 未参加职业道德的人数
        /// </summary>
        public int NoJoinDaodeNumber
        {
            get
            {
                return joinCheckNumber - JoinDaodeNumber;
            }
        }

    }
}
