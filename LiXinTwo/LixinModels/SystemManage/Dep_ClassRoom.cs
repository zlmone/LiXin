using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class Dep_ClassRoom
    {
        /// <summary>
        /// 教室ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { get; set; }
        /// <summary>
        /// 教室名称
        /// </summary>
        public string RoomName { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { set; get; }
        /// <summary>
        /// 教室容量
        /// </summary>
        public int RoomNumber { set; get; }

        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnNumber { set; get; }
        /// <summary>
        /// 行数
        /// </summary>
        public int RowNumber { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int UserID { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime LastUpdateTime { set; get; }

        /// <summary>
        /// 创建时间呈现
        /// </summary>
        public string LastUpdateTimeShow
        {
            get { return LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }
        /// <summary>
        /// 教室地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 删除标志位
        /// </summary>
        public int IsDelete { set; get; }
    }
}
