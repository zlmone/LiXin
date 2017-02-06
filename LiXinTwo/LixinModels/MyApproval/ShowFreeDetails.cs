using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.MyApproval
{
    public class ShowFreeDetails
    {

        public ShowFreeDetails()
        {
            ApplyFileList = new List<Free_UserApplyFiles>();
            teacherDetails = new List<Free_UserApplyTeacherDetails>();
        }


        /// <summary>
        /// ID
        /// </summary>		
        public int ID { get; set; }

        /// <summary>
        /// ApplyID
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
        /// ApplyTime
        /// </summary>		
        public DateTime ApplyTime { get; set; }

        /// <summary>
        /// ApplyUserID
        /// </summary>		
        public int ApplyUserID { get; set; }

        /// <summary>
        /// 其他形式：1 其他形式 2 免修 3 授课人
        /// 免修适用的申请: 0 所内免修 1 CPA免修 2所有
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
        /// 部门/分所退回理由
        /// </summary>
        public string DepTrainReason { get; set; }

        /// <summary>
        /// 总所退回理由
        /// </summary>
        public string DepReason { get; set; }

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

        public decimal GetCPAS { get; set; }

        /// <summary>
        /// 是否已经提交证明资料 1是 0否
        /// </summary>
        public int isCommit { get; set; }

        public string SubordinateSubstation { get; set; }

        #region 其他形式
        /// <summary>
        /// 申请内容
        /// </summary>
        public string ApplyContent { get; set; }

        /// <summary>
        /// 免修项目
        /// </summary>


        public string Realname { get; set; }

        public string DeptName { get; set; }

        public string TrainGrade { get; set; }

        public string MobileNum { get; set; }

        public int totalCount { get; set; }

        /// <summary>
        /// 0 所内免修 1 CPA免修
        /// </summary>
        public int ConvertType { get; set; }

        /// <summary>
        /// 证明资料
        /// </summary>
        public List<Free_UserApplyFiles> ApplyFileList { get; set; }

        public string CPANo { get; set; }

        public string CPA { get; set; }

        public int Sex { get; set; }


        /// <summary>
        /// ConvertBase
        /// </summary>		
        public decimal ConvertBase { get; set; }

        /// <summary>
        /// ConvertBaseInfo
        /// </summary>		
        public string ConvertBaseInfo { get; set; }

        /// <summary>
        /// ConvertBaseTo
        /// </summary>		
        public decimal ConvertBaseTo { get; set; }


        /// <summary>
        /// 可折CPA学时年度的最高限制
        /// </summary>		
        public decimal ConvertMax { get; set; }

        /// <summary>
        /// 可折内部培训学时年度的最高限制 其他形式 形如 A-1,B-2  授课人 直接一个数值
        /// </summary>		
        public string TrainGradeScore { get; set; }

        /// <summary>
        /// 申请人的可折算内部学时限制
        /// </summary>
        public string ApproveTScore
        {
            get
            {
                var result = "N/A";
                if (this.ApplyType == 1)
                {
                    if (string.IsNullOrEmpty(this.TrainGradeScore))
                    {
                        result = "N/A";
                    }
                    else
                    {
                        var valueList = this.TrainGradeScore.Split(',');
                        var dicList = new Dictionary<string, int>();
                        foreach (var item in valueList)
                        {
                            if (item.Split('-')[0] == this.TrainGrade)
                            {
                                result = item.Split('-')[1];
                            }
                        }
                    }
                }
                else if (this.ApplyType == 3)
                {
                    result = TrainGradeScore;
                }
                return result;
            }
        }

        public List<Free_UserApplyTeacherDetails> teacherDetails { get; set; }


        public Dictionary<string, decimal> TrainGradeScoreList
        {
            get
            {
                if (string.IsNullOrEmpty(this.TrainGradeScore) || this.ApplyType == 0)
                {
                    return new Dictionary<string, decimal>();
                }
                else
                {
                    var valueList = this.TrainGradeScore.Split(',');
                    var dicList = new Dictionary<string, decimal>();
                    foreach (var item in valueList)
                    {
                        dicList[item.Split('-')[0]] = Convert.ToDecimal(item.Split('-')[1]);
                    }
                    return dicList;
                }

            }
        }

        #endregion

        #region 免修
        public string FreeName { get; set; }

        /// <summary>
        /// CPA免修学时
        /// </summary>		
        public int CPAFreeScore { get; set; }

        /// <summary>
        /// 所内免修学时
        /// </summary>		
        public int TogetherFreeScore { get; set; }

        /// <summary>
        /// Memo
        /// </summary>		
        public string Memo { get; set; }

        /// <summary>
        /// Memo
        /// </summary>		
        public string MemoStr
        {
            get
            {
                return string.IsNullOrEmpty(Memo) ? "" : Memo.Replace("\n", "");
            }
        }

        /// <summary>
        /// 免修申请类型
        /// </summary>
        public string FreeApproveTypeStr
        {
            get
            {
                var result = "";
                switch (this.ApplyType)
                {
                    case 0:
                        result = "所内免修";
                        break;
                    case 1:
                        result = "CPA免修";
                        break;
                    case 2:
                        result = "所内免修、CPA免修";
                        break;
                }
                return result;
            }
        }


        #endregion

        #region 其他有组织形式


        /// <summary>
        /// 0 cpa 1 所内 2 全部
        /// </summary>	
        public int OtherFromID { get; set; }

        public string CourseName { get; set; }

        public string Address { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime ApplyDateTime { get; set; }

        public decimal TogetherScore { get; set; }

        public string ApplyDateTimeStr
        {
            get
            {
                return this.ApplyDateTime.ToString("yyyy-MM-dd");
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
        #endregion

        #region Extend


        public string ApplyTimeStr
        {
            get
            {
                return this.ApplyTime.ToString("yyyy-MM-dd HH:mm");
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
                    if (this.ApproveStatus == 1 && DepApproveStatus == 2)
                    {
                        result = "待部门负责人退回";
                    }
                    else
                    {
                        result = ((Enums.FreeApprove)this.ApproveStatus).ToString();
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
                    if ((this.ApproveStatus == 2 && this.DepApproveStatus == 0) || this.ApproveStatus==0)
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
                    result = ((Enums.FreeApprove)this.DepApproveStatus).ToString();
                }
                return result;
            }
        }

        public string SexStr
        {
            get
            {
                if (Sex == 0)
                    return "男";
                else if (Sex == 1)
                    return "女";
                else
                    return "保密";
            }
        }

        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }

        public string CreateTimeStr
        {
            get
            {
                return this.CreateTime.ToString();
            }
        }

        public string CreateUserName { get; set; }

        /// <summary>
        /// 0自己新增 1 导入 2 评估自动折算
        /// </summary>
        public int typeForm { get; set; }

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

        /// <summary>
        /// 批量发布的时候，Free_UserOtherApply的主键，用来查询资料 默认0
        /// </summary>
        public int fromID { get; set; }
        #endregion


        public string TrainGradeScoreStr
        {
            get
            {
                if (this.ApplyType == 1)
                {
                    if (ConvertType == 1 || ConvertType == 2)
                        return string.Join("/", TrainGradeScoreList.Values.ToList());
                    else
                        return "N/A";
                }
                else
                {
                    // return "按实际授课、主持或演讲时间的" + this.ConvertBaseTo + "倍折算学时";
                    return TrainGradeScore;
                }
            }
        }

        public string ConvertMaxStr
        {
            get
            {
                if (ConvertType == 0 || ConvertType == 2)
                    return ConvertMax.ToString();
                else
                    return "N/A";
            }
        }


        public decimal CPAScoreStr
        {
            get
            {
                if (ConvertType == 1 || CPA == "否")
                {
                    return -1;
                }
                else
                {
                    return this.CPAScore;
                }
            }
        }

        public decimal tScoreStr
        {
            get
            {
                if (ConvertType == 0)
                {
                    return -1;
                }
                else
                {
                    return this.tScore;
                }
            }
        }
    }
}
