using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class ApprovalUserInfor
    {
        #region 人员信息
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { set; get; }
        /// <summary>
        /// 课程ID
        /// </summary>
        public int CourseID { set; get; }
        /// <summary>
        /// 学员姓名
        /// </summary>
        public string RealName { set; get; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { set; get; }

        /// <summary>
        /// 培训级别
        /// </summary>
        public string TrainGrade { set; get; }
        /// <summary>
        /// 薪酬级别
        /// </summary>
        public string PayGrade { set; get; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime OrderTime { set; get; }

        /// <summary>
        /// 申请理由
        /// </summary>
        public string Reason { set; get; }
        /// <summary>
        /// 上课考勤时间
        /// </summary>
        public DateTime AttStartTime { set; get; }
        /// <summary>
        /// 下课考勤时间
        /// </summary>
        public DateTime AttEndTime { set; get; }
        /// <summary>
        /// 扣除学时
        /// </summary>
        public decimal LessLength { set; get; }
        /// <summary>
        /// 考勤状态（0：正常考勤；1：缺勤） 
        /// </summary>
        public int AttStatus { set; get; }
        /// <summary>
        /// 是否已申辩
        /// </summary>
        public int IsApp { set; get; }
        /// <summary>
        /// 申辩附件名称
        /// </summary>
        public string FileName { set; get; }
        /// <summary>
        /// 申辩附件名称
        /// </summary>
        public string FileRealName { set; get; }

        public string DeptName { get; set; }

        #endregion

        #region 课程信息

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { set; get; }

        /// <summary>
        /// 课程编号
        /// </summary>
        public string CourseCode { set; get; }

        /// <summary>
        /// 课程学时
        /// </summary>
        public decimal CourseLength { set; get; }

        /// <summary>
        /// 授课讲师
        /// </summary>
        public string Teacher { set; get; }

        /// <summary>
        /// 课程开始时间
        /// </summary>
        public DateTime StartTime { set; get; }

        /// <summary>
        /// 课程开始时间
        /// </summary>
        public DateTime EndTime { set; get; }

        /// <summary>
        /// 考勤方式（1：上课考勤；2：下课考勤；3：上、下课考勤）
        /// </summary>
        public int AttFlag { set; get; }

        /// <summary>
        /// 补预定申请时间
        /// </summary>
        public DateTime MakeUpTime { set; get; }

        #endregion

        #region 审批信息

        /// <summary>
        /// 审批人
        /// </summary>
        public string ApprovalName { set; get; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime ApprovalDateTime { set; get; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int ApprovalFlag { set; get; }
        /// <summary>
        /// 拒绝理由
        /// </summary>
        public string RefuseReason { set; get; }
        /// <summary>
        /// 审批类型（1：请假；2：补预定；3：逾时审批；4:违纪审批）
        /// </summary>
        public int TypeFlag { set; get; }
        /// <summary>
        /// 审批截止时间
        /// </summary>
        public DateTime ApprovalLimitTime { set; get; }

        #endregion 

        #region 扩展字段

        /// <summary>
        /// 申请时间显示
        /// </summary>
        public string OrderTimeShow { get { return OrderTime.ToString("yyyy-MM-dd HH:mm"); } }

        /// <summary>
        /// 审批时间显示
        /// </summary>
        public string ApprovalTimeShow
        {
            get 
            { 
                if(ApprovalFlag == 0)
                { 
                    return "——";
                }else{
                    return ApprovalDateTime.ToString("yyyy-MM-dd HH:mm"); 
                }
            }
        }

        public string ApprovalResult
        {
            get
            {
                if (ApprovalFlag != 0 && (TypeFlag == 3 || TypeFlag == 4))
                {
                    return ApprovalFlag == 1 ? "审批通过" : "审批拒绝";
                }
                if (ApprovalFlag != 0 && (TypeFlag != 3 && TypeFlag != 4))
                {
                    return ApprovalDateTime <= ApprovalLimitTime ? (ApprovalFlag == 1 ? "审批通过" : "审批拒绝") : "过期审批";
                }
                return "未审批";
            }
        }
        /// <summary>
        /// 课程开始时间呈现
        /// </summary>
        public string StartTimeShow { get { return StartTime.ToString("yyyy-MM-dd HH:mm"); } }
        /// <summary>
        /// 课程结束时间呈现
        /// </summary>
        public string EndTimeShow { get { return EndTime.ToString("yyyy-MM-dd HH:mm"); } }
        /// <summary>
        /// 上课考勤时间
        /// </summary>
        public string AttStartTimeShow 
        { 
            get 
            {
                if (AttStartTime == Convert.ToDateTime("2050-1-1"))
                {
                    return "——";
                }
                else
                {
                    return AttStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            } 
        }
        /// <summary>
        /// 下课考勤时间
        /// </summary>
        public string AttEndTimeShow 
        { 
            get 
            { 
                if (AttEndTime == Convert.ToDateTime("2000-1-1"))
                {
                    return "——";
                }
                else
                {
                    return AttEndTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            } 
        }
        /// <summary>
        /// 补预定申请时间
        /// </summary>
        public string MakeUpTimeShow { get { return MakeUpTime.ToString("yyyy-MM-dd HH:mm"); } }
        /// <summary>
        /// 出勤情况呈现
        /// </summary>
        public string AttStatusShow
        {
            get
            {
                if (AttFlag == 0)
                {
                    return "正常";
                }
                else if (AttFlag == 1)
                {
                    if (AttStartTime == DateTime.MinValue || AttStartTime == Convert.ToDateTime("2050-1-1"))
                    {
                        return "缺勤";
                    }
                    if (AttStartTime > StartTime)
                    {
                        return "迟到";
                    }
                    if (AttStartTime <= StartTime)
                    {
                        return "正常";
                    }
                }
                else if (AttFlag == 2)
                {
                    if (AttEndTime == DateTime.MinValue || AttEndTime == Convert.ToDateTime("2000-1-1"))
                    {
                        return "缺勤";
                    }
                    if (AttEndTime < EndTime)
                    {
                        return "早退";
                    }
                    if (AttEndTime >= EndTime)
                    {
                        return "正常";
                    }
                }
                else if (AttFlag == 3)
                {
                    if ((AttStartTime == DateTime.MinValue && AttEndTime == DateTime.MinValue) || (AttStartTime == Convert.ToDateTime("2050-1-1") &&
                        AttEndTime == Convert.ToDateTime("2000-1-1")))
                    {
                        return "缺勤";
                    }
                    if (AttStartTime == Convert.ToDateTime("2050-1-1") &&
                        AttEndTime != Convert.ToDateTime("2000-1-1"))
                    {
                        if (AttEndTime >= EndTime)
                        {
                            return "迟到";
                        }
                        return "迟到且早退";
                    }
                    if (AttStartTime != Convert.ToDateTime("2050-1-1") &&
                        AttEndTime == Convert.ToDateTime("2000-1-1"))
                    {
                        if (AttStartTime <= StartTime)
                        {
                            return "早退";
                        }
                        return "迟到且早退";
                    }
                    if (AttStartTime != Convert.ToDateTime("2050-1-1") &&
                        AttEndTime != Convert.ToDateTime("2000-1-1"))
                    {
                        if (AttStartTime <= StartTime && AttEndTime >= EndTime)
                        {
                            return "正常";
                        }
                        if (AttStartTime > StartTime && AttEndTime > EndTime)
                        {
                            return "迟到";
                        }
                        if (AttStartTime > StartTime && AttEndTime < EndTime)
                        {
                            return "迟到且早退";
                        }
                        if (AttStartTime <= StartTime && AttEndTime < EndTime)
                        {
                            return "早退";
                        }
                    }
                }
                return "";
            }
        }
        #endregion
    }
}