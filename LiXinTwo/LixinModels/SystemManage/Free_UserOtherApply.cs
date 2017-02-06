using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;

namespace LiXinModels.SystemManage
{
    public class Free_UserOtherApply
    {
        public Free_UserOtherApply()
        {
            UserApplyFiles = new List<Free_UserApplyFiles>();
        }

        #region Model
        public string  Username { get; set; }

        /// <summary>
        /// ID
        /// </summary>		
        public int ID { get; set; }

      
        

        /// <summary>
        /// typefrom=0
        ///ApplyType=1 Free_OtherApplyConfig主键 
        ///
        ///typefrom=1 Co_Course主键
        /// </summary>		
        public int ApplyID { get; set; }

        /// <summary>
        /// 申请内容
        /// </summary>
        public string ApplyName { get; set; }

        /// <summary>
        /// Year
        /// </summary>		
        public int Year { get; set; }

        /// <summary>
        /// 申报数值
        /// </summary>		
        public decimal ConvertScore { get; set; }

        /// <summary>
        /// Memo
        /// </summary>		
        public string Memo { get; set; }

        /// <summary>
        /// ApplyTime
        /// </summary>		
        public DateTime ApplyTime { get; set; }

        /// <summary>
        /// ApplyUserID
        /// </summary>		
        public int ApplyUserID { get; set; }

        /// <summary>
        /// ApplyType  1 其他形式 2 免修 3 授课人 4 其他有组织形式（4 查询的时候使用）
        /// </summary>		
        public int ApplyType { get; set; }

        /// <summary>
        /// Status 0 未提交 1提交
        /// </summary>		
        public int Status { get; set; }

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
        public int IsDelete { get; set; }

        /// <summary>
        /// tScore
        /// </summary>		
        public decimal tScore { get; set; }

        /// <summary>
        /// CPAScore
        /// </summary>		
        public decimal CPAScore { get; set; }


        public decimal GettScore { get; set; }

        public decimal GetCPAScore { get; set; }

        public string ApplyName_New { get; set; }

        /// <summary>
        /// 部门/分所退回理由
        /// </summary>
        public string DepTrainReason { get; set; }

        /// <summary>
        /// 总所退回理由
        /// </summary>
        public string DepReason { get; set; }

        /// <summary>
        /// 0自己新增 1 导入 2 评估自动折算  3授课自动折算 4 免修自动折算
        /// </summary>
        public int typeForm { get; set; }

        /// <summary>
        /// 创建者 TypeForm=1 CreateUserID>0 成功导入的学时
        /// </summary>
        public int CreateUserID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }


        public string CreateTimeStr
        {
            get
            {
                return this.CreateTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        /// <summary>
        /// Memo
        /// </summary>		
        public string MemoStr
        {
            get
            {
                return string.IsNullOrEmpty(Memo) ? "" : Memo.Replace("\n","");
            }
        }

        public int fromID { get; set; }


        public string Realname { get; set; }
        #endregion

        #region Extend


        public string Teacher { get; set; }
        /// <summary>
        /// 可折算学时 0 CPA学时 1 所内学时 2所有
        /// </summary>	
        public int ConvertType { get; set; }
        /// <summary>
        /// 1 自行申报合计 2 自动折算合计
        /// </summary>
        public int isout { get; set; }

        public string ApplyTimeStr
        {
            get
            {
                return this.ApplyTime == DateTime.MinValue ? "--" : this.ApplyTime.ToString("yyyy-MM-dd HH:mm");
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
                if (typeForm == 0)
                {
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
                }
                else
                {
                    result = "N/A";
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
                if (typeForm == 0)
                {
                    if ((this.ApproveStatus == 2 && this.DepApproveStatus == 0)||this.ApproveStatus==0)
                    {
                        result = "N/A";
                    }
                    else
                    {
                        result = ((Enums.FreeApprove)this.DepApproveStatus).ToString();
                    }
                }
                else
                {
                    result = "审批通过";
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

        public string CreateUserName { get; set; }

        public string typeFormStr
        {
            get
            {
                var result = "";
                switch (this.typeForm)
                {
                    case 0://自己新增
                        result = "自主申请";
                        break;
                    case 1://导入
                        result = "管理员导入";
                        break;
                    case 2://评估自动折算
                        result = "评估自动折算";
                        break;
                    case 3://讲师授课
                        result = "授课自动折算";
                        break;
                    case 4://免修自动折算
                        result = "免修自动折算";
                        break;

                }
                return result;
            }
        }

        public string DeptName { get; set; }

        public string TrainGrade { get; set; }

        public string CPANo { get; set; }

        /// <summary>
        /// 是否CPA
        /// </summary>
        public string CPA { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { get; set; }

        /// <summary>
        /// Free_ApplyConfig的ApplyType
        /// </summary>
        public int configtype { get; set; }

        public DateTime JoinDate { get; set; }

        public DateTime CPACreateDate { get; set; }

        /// <summary>
        /// 申请的所内免修数
        /// </summary>
        public int tCount { get; set; }

        /// <summary>
        /// 申请的CPA免修数
        /// </summary>
        public int CPACount { get; set; }
        #endregion


    }
}
