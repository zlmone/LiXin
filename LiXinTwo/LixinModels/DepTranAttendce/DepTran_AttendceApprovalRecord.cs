using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepTranAttendce
{
    ///<summary>
    ///DepTran_AttendceApprovalRecord
    ///</summary>
    public partial class DepTran_AttendceApprovalRecord
    {
        #region Model
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// CourseId
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// DeptId
        /// </summary>
        public int DeptId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// ApprovalTime
        /// </summary>
        public DateTime ApprovalTime { get; set; }
        /// <summary>
        /// ApprovalFlag
        /// </summary>
        public int ApprovalFlag { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public int SubmitUserId { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime { get; set; }

        public string Reason { get; set; }

        #endregion Model

        /// <summary>
        /// 审批人
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public string ApprovalTimeStr
        {
            get
            {
                return ApprovalTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        /// <summary>
        /// 提交人
        /// </summary>
        public string SubmitRealname { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public string SubmitTimeStr
        {
            get
            {
                return SubmitTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public int totalcount { get; set; }
    }
}
