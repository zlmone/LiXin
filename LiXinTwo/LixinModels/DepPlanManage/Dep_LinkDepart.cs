using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepPlanManage
{
    ///<summary>
    ///Dep_LinkDepart
    ///</summary>
    public partial class Dep_LinkDepart
    {
        #region Model
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// YearId
        /// </summary>
        public int YearId { get; set; }
        /// <summary>
        /// DeptId
        /// </summary>
        public int DeptId { get; set; }
        /// <summary>
        /// ApprovalFlag
        /// </summary>
        public int ApprovalFlag { get; set; }
        /// <summary>
        /// ApprovalTime
        /// </summary>
        public DateTime ApprovalTime { get; set; }
        /// <summary>
        /// Reason
        /// </summary>
        public string Reason { get; set; }
        #endregion Model

        public string DeptName { get; set; }
    }
}
