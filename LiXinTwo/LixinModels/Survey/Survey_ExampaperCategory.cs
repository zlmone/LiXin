using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Survey
{
    public class Survey_ExampaperCategory
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId { set; get; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { set; get; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public int ParentId { set; get; }
        /// <summary>
        /// 状态（0：正常；1：删除）
        /// </summary>
        public int IsDelete { set; get; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptID { get; set; }
    }
}
