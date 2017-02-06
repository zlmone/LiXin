using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    public class Sys_LeaderConfig
    {
        #region model
        /// <summary>
        /// Id
        /// </summary>		
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// GroupName
        /// </summary>		
        public string GroupName
        {
            get;
            set;
        }
        /// <summary>
        /// UserId
        /// </summary>		
        public int UserId
        {
            get;
            set;
        }
        /// <summary>
        /// CreateTime
        /// </summary>		
        public DateTime CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// LastUpdateTime
        /// </summary>		
        public DateTime LastUpdateTime
        {
            get;
            set;
        }
        /// <summary>
        /// Memo
        /// </summary>		
        public string Memo
        {
            get;
            set;
        }
        /// <summary>
        /// IsDelete
        /// </summary>		
        public int IsDelete
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 用户数量
        /// </summary>
        public int UNumber { get; set; }

        public string CreateTimeStr
        {
            get { return CreateTime.ToString("yyyy-MM-dd HH:mm"); }
        }

        public string LastUpdateTimeStr
        {
            get { return LastUpdateTime.ToString("yyyy-MM-dd HH:mm"); }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Realname { get; set; }

        public int totalcount { get; set; }
    }
}
