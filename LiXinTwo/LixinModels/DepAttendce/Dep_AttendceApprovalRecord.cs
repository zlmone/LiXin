using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepAttendce
{
    ///<summary>
    ///Dep_AttendceApprovalRecord
    ///</summary>
    public partial class Dep_AttendceApprovalRecord
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
        /// 1:自学课程；2：开放至全所；3：过时的
        /// </summary>
        public int Way { get; set; }

        /// <summary>
        /// DeptId
        /// </summary>
        public int DeptId { get; set; }

        public string  DeptName { get; set; }
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
        /// Reason
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// SubmitUserId
        /// </summary>
        public int SubmitUserId { get; set; }
        /// <summary>
        /// SubmitTime
        /// </summary>
        public DateTime SubmitTime { get; set; }
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

        public decimal CourseLength { get; set; }

        public int yearplan { get; set; }

        public int AttFlag { get; set; }

        public int Status { get; set; }
    }
}
