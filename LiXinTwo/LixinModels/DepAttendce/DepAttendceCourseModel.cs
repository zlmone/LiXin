using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepAttendce
{
    public class DepAttendceCourseModel
    {
        #region 基本字段
        /// <summary>
        /// 课程ID
        /// </summary>
        public int CourseID { set; get; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartID { set; get; }
        /// <summary>
        /// 课程类型
        /// </summary>
        public int Way { get; set; }
        /// <summary>
        /// 课程学时
        /// </summary>
        public decimal CourseLength { set; get; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { set; get; }
        /// <summary>
        /// 必修/选修
        /// </summary>
        public int IsMust { set; get; }
        /// <summary>
        /// 课程开始时间
        /// </summary>
        public DateTime StartTime { set; get; }
        /// <summary>
        /// 课程结束时间
        /// </summary>
        public DateTime EndTime { set; get; }
        /// <summary>
        /// 授课地点
        /// </summary>
        public string RoomName { set; get; }
        /// <summary>
        /// 是否开放预订
        /// </summary>
        public int IsOpen { set; get; }
        /// <summary>
        /// 提交部门
        /// </summary>
        public string DeptName { set; get; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime { set; get; }
        /// <summary>
        /// 讲师姓名
        /// </summary>
        public string TeacherName { set; get; }

        #endregion

        #region 扩展字段
        /// <summary>
        /// 课程开课时间
        /// </summary>
        public string CourseTimeStr{get { return StartTime.ToString("yyyy-MM-dd HH:mm") + " - " + EndTime.ToString("yyyy-MM-dd HH:mm"); }}
        /// <summary>
        /// 提交时间
        /// </summary>
        public string SubmitTimeStr { get { return SubmitTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

        /// <summary>
        /// 是否必修
        /// </summary>
        public string IsMustStr
        {
            get
            {
                return ((Enums.IsMust)this.IsMust).ToString();
            }
        }

        /// <summary>
        /// 讲师
        /// </summary>
        public string TeacherStr
        {
            get
            {
                if (string.IsNullOrEmpty(TeacherName))
                {
                    return "——";
                }
                else
                {
                    return TeacherName;
                }
            }
        }

        /// <summary>
        /// 总条数
        /// </summary>
        public int Totalcount { set; get; }

        public int AttFlag { get; set; }

        public int IsTest { get; set; }

        public int IsPing { get; set; }
        #endregion

        #region 考勤

        //是否开放
        public int OpenFlag { get; set; }

        /// <summary>
        ///审批状态
        /// </summary>
        public string attendStatusStr
        {
            get
            {
                if (AttFlag == 0 && ApprovalFlag == 0)
                {
                    return "未审批";
                }
                else if (AttFlag == 1 && ApprovalFlag == 1)
                {
                    return "审批通过";
                }
                else if (AttFlag == 1 && ApprovalFlag == 0)
                {
                    return "审批中";
                }
                else
                {
                    return "审批拒绝";
                }
            }
        }

        /// <summary>
        /// 课程状态
        /// </summary>
        public int CourseState
        {
            get
            {
                if (this.StartTime > DateTime.Now)
                    return 1;
                else if (this.StartTime <= DateTime.Now && this.EndTime >= DateTime.Now)
                    return 2;
                else
                    return 3;
            }
        }

        /// <summary>
        /// 课程状态
        /// </summary>
        public string StateStr
        {
            get
            {
                return ((Enums.State)this.CourseState).ToString();
            }
        }

        /// <summary>
        /// 群组ID集合
        /// </summary>
        public string DepartSetIds { set; get; }

        /// <summary>
        /// 报名人数
        /// </summary>
        public int CoCount { set; get; }

        /// <summary>
        /// 考勤人数
        /// </summary>
        public int AttCount { set; get; }

        /// <summary>
        /// 考勤正常人数
        /// </summary>
        public int AttOkCount { set; get; }

        /// <summary>
        /// 考勤缺勤人数
        /// </summary>
        public int AttNoCount { set; get; }

        /// <summary>
        /// 考勤迟到人数
        /// </summary>
        public int chidaoCount { set; get; }

        /// <summary>
        /// 考勤早退人数
        /// </summary>
        public int zaotuiCount { set; get; }

        /// <summary>
        /// 考勤迟到且早退人数
        /// </summary>
        public int chizaoCount { set; get; }

        /// <summary>
        /// 考勤待考勤人数
        /// </summary>
        public int NoAttCount
        {
            get
            {
                return CoCount - AttCount;
            }
        }

        public string DepartSetName { get; set; }

        public string DeptCode { get; set; }
        #endregion

        #region 考勤详情
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// CourseId
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// ApprovalFlag
        /// </summary>
        public int ApprovalFlag { get; set; }

        public string ApprovalFlagStr
        {
            get
            {
                if (AttFlag == 0)
                {
                    return "未审批";
                }
                else if (AttFlag == 1)
                {
                    return "审批通过";
                }
                else
                {
                    return "审批拒绝";
                }
            }
        }
        /// <summary>
        /// Reason
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// DepartSetId
        /// </summary>
        public int DepartSetId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// CPA
        /// </summary>
        public string CPA
        {
            get;
            set;
        }

        public int IsCPA
        {
            get
            {
                if (string.IsNullOrEmpty(CPA))
                {
                    return 0;
                }
                else
                {
                    if (CPA == "是")
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        /// <summary>
        /// 课程学时分配
        /// </summary>
        public string CourseLengthDistribute { get; set; }

        public string SubordinateSubstation { get; set; }

        /// <summary>
        /// 请假
        /// </summary>
        public int IsLeave { get; set; }

        #endregion

        public int totalcount{get;set;}
    }
}
