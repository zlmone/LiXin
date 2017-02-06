using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class Sys_ClassRoomResource
    {
        /// <summary>
        /// 教室资源表自增id
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// 资源所属教室ID
        /// </summary>
        public int ClassRoomID { get; set; }

        /// <summary>
        /// 资源上传后保存的新文件名
        /// </summary>
        public string RealName { set; get; }

        /// <summary>
        /// 资源上传前原始的文件名
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { set; get; }

        /// <summary>
        /// 创建时间呈现
        /// </summary>
        public string CreateDateShow
        {
            get { return CreateDate.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        /// <summary>
        /// 删除标志位
        /// </summary>
        public int IsDelete { set; get; }
    }
}
