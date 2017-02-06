using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    [Serializable]
    public class Free_UserApplyFiles
    {
        public Free_UserApplyFiles() { }

        public int ID { get; set; }

        /// <summary>
        /// Free_UserOtherApply 主键
        /// </summary>
        public int userApplyID { get; set; }

        /// <summary>
        /// 真实名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 存储名称
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 删除标志位
        /// </summary>
        public int IsDelete { get; set; }

        /// <summary>
        /// 转换之后的名称
        /// </summary>
        public string ConvertName { get; set; }

        /// <summary>
        /// 0其他形式 免修  1 其他有组织形式  
        /// </summary>
        public int type { get; set; }

    }
}
