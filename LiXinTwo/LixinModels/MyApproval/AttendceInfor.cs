using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class AttendceInfor
    {
        /// <summary>
        /// 扣除学时
        /// </summary>
        public decimal LessLength { set; get; }
        /// <summary>
        /// 申请理由
        /// </summary>
        public string Reason { set; get; }
    }
}
