using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepPlanManage
{
    ///<summary>
    ///Dep_YearPlan
    ///</summary>
    public partial class Dep_YearPlan
    {
        #region Model
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// LastUpdateTime
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// Year
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// PublishFlag
        /// </summary>
        public int PublishFlag { get; set; }
        /// <summary>
        /// IsDelete
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// IsOpen
        /// </summary>
        public int IsOpen { get; set; }
        /// <summary>
        /// DeptId
        /// </summary>
        public int DeptId { get; set; }
        #endregion Model

        /// <summary>
        /// 包含课程数
        /// </summary>
        public int courseCount { get; set; }

        /// <summary>
        /// 所有课程学时
        /// </summary>
        public decimal courseLength { get; set; }
        /// <summary>
        /// 联合数量
        /// </summary>
        public int linkCount { get; set; }

        /// <summary>
        /// 联合成功数量
        /// </summary>
        public int linkYesCount { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 部门名称集合
        /// </summary>
        public string DeptNames { get; set; }

        public string CreateTimeStr
        {
            get { return LastUpdateTime.ToString("yyyy-MM-dd HH:mm"); }
        }

        /// <summary>
        /// 联合部门ID
        /// </summary>
        public int linkdepID { get; set; }

        /// <summary>
        /// 联合部门名称
        /// </summary>
        public string linkDeptName { get; set; }

        /// <summary>
        /// 编辑标志
        /// </summary>
        public int EditFlag { get; set; }

        public int eidt { get; set; }

        public int OpenType { get; set; }
        /// <summary>
        /// 上报类型
        /// </summary>
        public string OpenTypeStr{get;set;}

        /// <summary>
        /// 联合上报部门ID集合
        /// </summary>
        public string DepIds { get; set; }

        /// <summary>
        /// 所有部门ID集合(年度)
        /// </summary>
        public string AllDepIds { get; set; }

        public int ApprovalFlag { get; set; }

        public int totalcount { get; set; }

        public int iscount { get; set; }
    }
}
