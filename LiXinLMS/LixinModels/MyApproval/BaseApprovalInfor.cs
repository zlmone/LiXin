using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class BaseApprovalInfor
    {
        /// <summary>
        /// 审批数据主键
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { set; get; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime SubmitTime { set; get; }

        /// <summary>
        /// 课程编号
        /// </summary>
        public string CourseCode { set; get; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { set; get; }

        /// <summary>
        /// 课程学时
        /// </summary>
        public decimal CourseLength { set; get; }

        /// <summary>
        /// 审批结果（0：未审批；1：通过；2：拒绝）
        /// </summary>
        public int ApprovalFlag { set; get; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime ApprovalTime { set; get; }

        /// <summary>
        /// 审批截止时间
        /// </summary>
        public DateTime ApprovalLimitTime { set; get; }

        /// <summary>
        /// 1:请假；2：补预定；3：逾时申请
        /// </summary>
        public int TypeFlag { set; get; }

        #region 扩展字段

        /// <summary>
        /// 申请时间显示
        /// </summary>
        public string SubmitTimeShow { get { return SubmitTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

        /// <summary>
        /// 是否已审批显示((ApprovalFlag != 0 && TypeFlag != 3 && ApprovalTime<=ApprovalLimitTime) || TypeFlag == 3)
        /// </summary>
        public string IsApproval
        {
            get { return ApprovalFlag == 0 ? "否" : "是"; }
        }
        /// <summary>
        /// 审批时间显示
        /// </summary>
        public string ApprovalTimeShow
        {
            get { return ApprovalFlag == 0 ? "--" : ApprovalTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public string ApprovalResult
        {
            get
            {
                return ApprovalFlag == 0 ? "未审批" : (((ApprovalFlag != 0 && (TypeFlag != 3 && TypeFlag != 4) && ApprovalTime <= ApprovalLimitTime) || TypeFlag == 3 || TypeFlag == 4) ? (ApprovalFlag == 1 ? "审批通过" : "审批拒绝") : "过期审批");
            }
        }

        #endregion
    }
}
