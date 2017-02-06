using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    public class Sys_LogOperate
    {
        public int LogId { get; set; }

        public string LogTitle { get; set; }

        public DateTime LogTime { get; set; }

        public int LogType { get; set; }

        public string ClientIP { get; set; }

        public string ModuleName { get; set; }

        public string Record { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int userId { get; set; }

        public int IsDelete { get; set; }
    }
}
