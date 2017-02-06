using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    public class UpdateRecord
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int _id { set; get; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { set; get; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { set; get; }
    }
}
