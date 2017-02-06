using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class PrincipleInfor
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 学员ID
        /// </summary>
        public int UserID { set; get; }
        /// <summary>
        /// 学员姓名
        /// </summary>
        public string RealName { set; get; }
        /// <summary>
        /// 课程ID
        /// </summary>
        public int CourseID { set; get; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { set; get; }
        /// <summary>
        /// 考勤方式（0：不考勤；1：上课考勤；2：下课考勤；3：上、下课都考勤）
        /// </summary>
        public int AttFlag { set; get; }

        /// <summary>
        /// 培训等级
        /// </summary>
        public string TrainGrade { set; get; }
        /// <summary>
        /// 所在部门
        /// </summary>
        public string DepartName { set; get; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int Status { set; get; }
        /// <summary>
        /// 是否申辩（0：未提出申请；1：提出申请 ）
        /// </summary>
        public int IsApp { set; get; }
        /// <summary>
        /// 出勤状态（0：正常出勤；1：缺勤）
        /// </summary>
        public int AttStatus { set; get; }

        /// <summary>
        /// 课程学时
        /// </summary>
        public decimal CourseLength { set; get; }
        /// <summary>
        /// 扣除学时
        /// </summary>
        public decimal LessLength { set; get; }
        /// <summary>
        /// 课程开始时间
        /// </summary>
        public DateTime CourseStartTime { set; get; }
        /// <summary>
        /// 课程结束时间
        /// </summary>
        public DateTime CourseEndTime { set; get; }
        /// <summary>
        /// 上课考勤时间
        /// </summary>
        public DateTime AttStartTime { set; get; }
        /// <summary>
        /// 下课考勤时间
        /// </summary>
        public DateTime AttEndTime { set; get; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime AppDateTime { set; get; }

        #region 扩展字段

        /// <summary>
        /// 课程时间呈现
        /// </summary>
        public string CourseStartTimeShow { get { return CourseStartTime.ToString("yyyy-MM-dd HH:mm"); } }
        /// <summary>
        /// 课程时间呈现
        /// </summary>
        public string CourseEndTimeShow { get { return CourseEndTime.ToString("yyyy-MM-dd HH:mm"); } }

        /// <summary>
        /// 课程时间呈现
        /// </summary>
        public string CourseTimeShow
        {
            get
            {
                return CourseStartTime.ToString("yyyy-MM-dd HH:mm") + " -- " + this.CourseEndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        /// <summary>
        /// 上课考勤时间呈现
        /// </summary>
        public string AttStartTimeShow { get { return AttStartTime == Convert.ToDateTime("2000-1-1") ? "——" : AttStartTime.ToString("yyyy-MM-dd HH:mm"); } }
        /// <summary>
        /// 下课考勤时间呈现
        /// </summary>
        public string AttEndTimeShow { get { return AttEndTime == Convert.ToDateTime("2000-1-1") ? "——" : AttEndTime.ToString("yyyy-MM-dd HH:mm"); } }
        /// <summary>
        /// 申辩时间呈现
        /// </summary>
        public string AppDateTimeShow { get { return AppDateTime.ToString("yyyy-MM-dd HH:mm"); } }
        /// <summary>
        /// 审批状态呈现
        /// </summary>
        public string StatusShow
        {
            get
            {
                if (IsApp == 1)
                {
                    switch (Status)
                    {
                        case 0:
                            return "未审批";
                        case 1:
                            return "审批通过";
                        case 2:
                            return "审批拒绝";
                        default:
                            return "未审批";
                    }
                }
                else
                {
                    return "未提交";
                }
            }
        }
        /// <summary>
        /// 是否已申辩呈现
        /// </summary>
        public string IsAppShow { get { return IsApp == 0 ? "否" : "是"; } }
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
                    if (Convert.ToDateTime(AttStartTime.ToString("g")) > Convert.ToDateTime(CourseStartTime.ToString("g")))
                    {
                        return "迟到";//" + (AttStartTime.Subtract(CourseStartTime).TotalMinutes).ToString("0.00") + "(min)";
                    }
                    if (Convert.ToDateTime(AttStartTime.ToString("g")) <= Convert.ToDateTime(CourseStartTime.ToString("g")))
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
                    if (Convert.ToDateTime(AttEndTime.ToString("g")) < Convert.ToDateTime(CourseEndTime.ToString("g")))
                    {
                        return "早退";//(" + (AttEndTime.Subtract(CourseEndTime).TotalMinutes * -1).ToString("0.00") + "min)";
                    }
                    if (Convert.ToDateTime(AttEndTime.ToString("g")) >= Convert.ToDateTime(CourseEndTime.ToString("g")))
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
                        if (Convert.ToDateTime(AttEndTime.ToString("g")) >= Convert.ToDateTime(CourseEndTime.ToString("g")))
                        {
                            return "迟到";
                        }
                        return "迟到且早退";//<span class='f11'>" + (AttEndTime.Subtract(CourseEndTime).TotalMinutes * -1).ToString("0.00") + "(min)</span>";
                    }
                    if (AttStartTime != Convert.ToDateTime("2050-1-1") &&
                        AttEndTime == Convert.ToDateTime("2000-1-1"))
                    {
                        if (Convert.ToDateTime(AttStartTime.ToString("g")) <= Convert.ToDateTime(CourseStartTime.ToString("g")))
                        {
                            return "早退";
                        }
                        return "迟到且早退";//<span class='f11'>" +(AttStartTime.Subtract(CourseStartTime).TotalMinutes).ToString("0.00") + "(min)</span>/早退";
                    }
                    if (AttStartTime != Convert.ToDateTime("2050-1-1") &&
                        AttEndTime != Convert.ToDateTime("2000-1-1"))
                    {
                        if (Convert.ToDateTime(AttStartTime.ToString("g")) <= Convert.ToDateTime(CourseStartTime.ToString("g")) && Convert.ToDateTime(AttEndTime.ToString("g")) >= Convert.ToDateTime(CourseEndTime.ToString("g")))
                        {
                            return "正常";
                        }
                        if (Convert.ToDateTime(AttStartTime.ToString("g")) > Convert.ToDateTime(CourseStartTime.ToString("g")) && Convert.ToDateTime(AttEndTime.ToString("g")) >= Convert.ToDateTime(CourseEndTime.ToString("g")))
                        {
                            return "迟到";//" + (AttStartTime.Subtract(CourseStartTime).TotalMinutes).ToString("0.00") +"(min)";
                        }
                        if (Convert.ToDateTime(AttStartTime.ToString("g")) > Convert.ToDateTime(CourseStartTime.ToString("g")) && Convert.ToDateTime(AttEndTime.ToString("g")) < Convert.ToDateTime(CourseEndTime.ToString("g")))
                        {
                            return "迟到且早退";//<span class='f11'>" +(AttStartTime.Subtract(CourseStartTime).TotalMinutes).ToString("0.00") +"(min)</span>/早退<span class='f11'>" +(AttEndTime.Subtract(CourseEndTime).TotalMinutes * -1).ToString("0.00") + "(min)</span>";
                        }
                        if (Convert.ToDateTime(AttStartTime.ToString("g")) <= Convert.ToDateTime(CourseStartTime.ToString("g")) && Convert.ToDateTime(AttEndTime.ToString("g")) < Convert.ToDateTime(CourseEndTime.ToString("g")))
                        {
                            return "早退";//(" + (AttEndTime.Subtract(CourseEndTime).TotalMinutes * -1).ToString("0.00") + "min)";
                        }
                    }
                }
                return "";
            }
        }

        #endregion
    }
}
