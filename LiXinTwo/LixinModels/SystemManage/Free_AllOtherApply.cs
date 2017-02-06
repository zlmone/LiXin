using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    /// <summary>
    /// 批量导入其他形式
    /// </summary>
    public class Free_AllOtherApply
    {
        public int ID { get; set; }

        /// <summary>
        /// 人员ID集合
        /// </summary>
        public string UserIDs { get; set; }

        /// <summary>
        /// 申请表ID
        /// </summary>
        public int UserApplyID { get; set; }
    }
}
