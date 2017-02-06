using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepCourseLearn
{
    public class DepLeaveInfor
    {
        /// <summary>
        /// 请假ID
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 请假人
        /// </summary>
        public int UserID { set; get; }
        /// <summary>
        /// 请假人姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { set; get; }
        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { set; get; }
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
        /// 请假时间
        /// </summary>
        public DateTime LeaveTime { set; get; }
        /// <summary>
        /// 请假理由
        /// </summary>
        public string Reason { set; get; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { set; get; }
        /// <summary>
        /// 课程编号
        /// </summary>
        public string CourseCode { set; get; }
        /// <summary>
        /// 必/选修
        /// </summary>
        public int IsMust { set; get; }
        /// <summary>
        /// 课程学时
        /// </summary>
        public decimal CourseLength { set; get; }
        /// <summary>
        /// 讲师
        /// </summary>
        public string Teacher { set; get; }
        /// <summary>
        /// 课程开始时间
        /// </summary>
        public DateTime StartTime { set; get; }
        /// <summary>
        /// 课程结束时间
        /// </summary>b
        public DateTime EndTime { set; get; }
        /// <summary>
        /// 审批人姓名
        /// </summary>
        public string AppRealName { set; get; }
        /// <summary>
        /// 审批理由
        /// </summary>
        public string AppReason { set; get; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int AppFlag { set; get; }
        /// <summary>
        /// 审批状态 0:未过期；1：过期
        /// </summary>
        public int LimitFlag { set; get; }
        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime AppTime { set; get; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { set; get; }
        /// <summary>
        /// 审批截止时间
        /// </summary>
        public DateTime ApprovalLimitTime { set; get; }


        #region 扩展字段

        /// <summary>
        /// 总条数
        /// </summary>
        public int Totalcount { set; get; }
        /// <summary>
        /// 请假时间呈现
        /// </summary>
        public string LeaveTimeShow { get { return LeaveTime.ToString("yyyy-MM-dd HH:mm:ss"); } }
        /// <summary>
        /// 课程开始时间呈现
        /// </summary>
        public string StartTimeShow { get { return StartTime.ToString("yyyy-MM-dd HH:mm:ss"); } }
        /// <summary>
        /// 课程结束时间呈现
        /// </summary>
        public string EndTimeShow { get { return EndTime.ToString("yyyy-MM-dd HH:mm:ss"); } }
        /// <summary>
        /// 审批时间
        /// </summary>
        public string AppTimeShow { get { return AppTime.ToString("yyyy-MM-dd HH:mm:ss"); } }
        /// <summary>
        /// 审批状态
        /// </summary>
        public string LeaveStatus
        {
            get
            {
                if(AppFlag==0)
                {
                    return ApprovalLimitTime < DateTime.Now ? "审批拒绝" : "未审批";
                }
                return AppTime > ApprovalLimitTime ? "审批拒绝" : (AppFlag == 1 ? "审批通过" : "审批拒绝");
            }
        }

        #endregion
    }
}
 