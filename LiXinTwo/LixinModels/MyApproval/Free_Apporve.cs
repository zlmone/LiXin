using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.MyApproval
{
    public class Free_Apporve
    {
        #region Model
        /// <summary>
        /// ID
        /// </summary>		
        public int ID { get; set; }

        /// <summary>
        /// ApproveType=1或者2 Free_UserOtherApply 主键，
        ///ApproveType=3 Free_UserApplyOtherForm主键
        /// </summary>		
        public int userApplyID { get; set; }

        ///// <summary>
        ///// 部门/分所负责人审批 状态 0 未审批 1 通过  2拒绝  
        ///// </summary>		
        //public int DepTrainApprove { get; set; }

        /// <summary>
        /// 总所负责人审批 状态 0 未审批 1 通过  2拒绝  
        /// </summary>		
        public int DepApprove { get; set; }

        /// <summary>
        /// 部门/分所退回理由
        /// </summary>		
      //  public string DepTrainReason { get; set; }

        /// <summary>
        /// 总所退回理由
        /// </summary>		
        public string DepReason { get; set; }

        /// <summary>
        /// 批次
        /// </summary>		
        public int CheackBatch { get; set; }

        /// <summary>
        /// 1 分所 2 总所
        /// </summary>		
        public int type { get; set; }

        /// <summary>
        /// ApproveUserID
        /// </summary>		
        public int ApproveUserID { get; set; }

        /// <summary>
        /// ApproveTime
        /// </summary>		
        public DateTime ApproveTime { get; set; }

        /// <summary>
        /// 1 其他形式 2 免修 3其他形式项目 
        /// </summary>		
        public int ApproveType { get; set; }

        /// <summary>
        /// 证明资料是否被分所查看 1 查看 0没有查看
        /// </summary>
        public int LookFile { get; set; }

        /// <summary>
        /// 证明资料是否被总所查看 1 查看 0没有查看
        /// </summary>
      //  public int DepLookFile { get; set; }
        #endregion

        ///// <summary>
        ///// 分所审批状态  0 未审批 1 通过  2拒绝  
        ///// </summary>
        //public string DepTrainApproveStr
        //{
        //    get
        //    {
        //        return ((Enums.FreeApprove)this.DepTrainApprove).ToString();
        //    }
        //}
        /// <summary>
        /// 总所审批状态 0 未审批 1 通过  2拒绝  
        /// </summary>
        public string DepApproveStr
        {
            get
            {
                return ((Enums.FreeApprove)this.DepApprove).ToString();
            }
        }

        public string Realname { get; set; }

        public string ApproveTimeStr
        {
            get { return this.ApproveTime.ToString("yyyy-MM-dd HH:mm"); }
        }
    }
}
