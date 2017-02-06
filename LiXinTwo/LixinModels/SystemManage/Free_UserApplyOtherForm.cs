using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
   public class Free_UserApplyOtherForm
   {
       #region Model
       			
		/// <summary>
		/// ID
        /// </summary>		
        public int ID { get; set;}        
				
		/// <summary>
        /// 0 cpa 1 所内 2 全部
        /// </summary>		
        public int OtherFromID { get; set;}        
				
		/// <summary>
		/// Year
        /// </summary>		
        public int Year { get; set;}        
				
		/// <summary>
		/// CourseName
        /// </summary>		
        public string CourseName { get; set;}        
				
		/// <summary>
		/// Address
        /// </summary>		
        public string Address { get; set;}        
				
		/// <summary>
		/// StartTime
        /// </summary>		
        public DateTime StartTime { get; set;}        
				
		/// <summary>
		/// EndTime
        /// </summary>		
        public DateTime EndTime { get; set;}        
				
		/// <summary>
		/// TogetherScore
        /// </summary>		
        public decimal TogetherScore { get; set;}        
				
		/// <summary>
		/// CPAScore
        /// </summary>		
        public decimal CPAScore { get; set; }        
				
		/// <summary>
		/// ApplyDateTime
        /// </summary>		
        public DateTime ApplyDateTime { get; set;}        
				
		/// <summary>
		/// ApplyUserID
        /// </summary>		
        public int ApplyUserID { get; set;}        
				
		/// <summary>
		/// Status
        /// </summary>		
        public int Status { get; set;}        
				
		  /// <summary>
        /// 分所审批 0 未审批 1 通过  2拒绝  
        /// </summary>		
        public int ApproveStatus { get; set; }

        /// <summary>
        /// 总所负责人审批 状态 0 未审批 1 通过  2拒绝  
        /// </summary>
        public int DepApproveStatus { get; set; }   
				
		/// <summary>
		/// IsDelete
        /// </summary>		
        public int IsDelete { get; set;}        
				
		/// <summary>
		/// GettScore
        /// </summary>		
        public decimal GettScore { get; set;}        
				
		/// <summary>
		/// GetCPAScore
        /// </summary>		
        public decimal GetCPAScore { get; set;}        
				
		 /// <summary>
        /// 部门/分所退回理由
        /// </summary>
        public string DepTrainReason { get; set; }

        /// <summary>
        /// 总所退回理由
        /// </summary>
        public string DepReason { get; set; }
       #endregion

        /// <summary>
        /// 1 合计
        /// </summary>
        public int isout { get; set; }

        public string Username { get; set; }

        public string ApplyTimeStr
        {
            get
            {
                return this.ApplyDateTime.ToString("yyyy-MM-dd HH:mm");
            }
        }



        /// <summary>
        /// 分所审批状态  0 未审批 1 通过  2拒绝  
        /// </summary>
        public string ApproveStatusStr
        {
            get
            {

                var result = "";

                if (this.ApproveStatus == 2)
                {
                    result = "审批退回";
                }
                else if (this.ApproveStatus == 1 && (DepApproveStatus == 1 || DepApproveStatus == 0))
                {
                    result = "审批通过";
                }
                else if (this.ApproveStatus == 1 && DepApproveStatus == 2)
                {
                    result = "待部门负责人退回";
                }
                else if (this.ApproveStatus == 0)
                {
                    result = "待审批";
                }

                return result;

            }
        }

        /// <summary>
        /// 总所审批状态  0 未审批 1 通过  2拒绝  
        /// </summary>
        public string DepApproveStatusStr
        {

            get
            {
                var result = "";
                if ((this.ApproveStatus == 2 && this.DepApproveStatus == 0)||this.ApproveStatus==0)
                {
                    result = "N/A";
                }
                else
                {
                    result = ((Enums.FreeApprove)this.DepApproveStatus).ToString();
                }
                return result;
            }
        }

        public string SexStr
        {

            get
            {
                return ((Enums.FreeApprove)this.DepApproveStatus).ToString();
            }
        }

        public int totalCount { get; set; }

        public List<Free_UserApplyFiles> UserApplyFiles { get; set; }

        public string TrainTimeStr
        {
            get
            {
                return this.StartTime.ToString("yyyy-MM-dd HH:mm") + "—" +
                    this.EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string StartTimeStr
        {
            get
            {
                return this.StartTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
        public string EndTimeStr
        {
            get
            {
                return this.EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string DeptName { get; set; }

        public string TrainGrade { get; set; }

        public string CPANo { get; set; }

        /// <summary>
        /// 是否CPA
        /// </summary>
        public string CPA { get; set; }


        public string Realname { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { get; set; }
   }
}
