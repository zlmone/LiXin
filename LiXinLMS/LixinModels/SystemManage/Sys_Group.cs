using System;
using LiXinCommon;

namespace LiXinModels.SystemManage
{
    [Serializable]
    public class Sys_Group
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 群组名称 
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 创建者ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 群组名称 
        /// </summary>
        public string Memo { get; set; }
        
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 是否删除 0：未删除；1：删除；
        /// </summary>
        public int IsDelete { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Realname { get; set; }

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

        public int totalcount
        {
            get;
            set;
        }
    }
}
