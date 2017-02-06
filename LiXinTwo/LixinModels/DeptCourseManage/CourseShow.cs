using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;

namespace LiXinModels.DeptCourseManage
{
    /// <summary>
    /// 颤抖吧，尔等凡人们
    /// </summary>
    public class CourseShow
    {
        #region Model

        /// <summary>
        /// IsYearPlan
        /// </summary>
        public int IsYearPlan { get; set; }


        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 学时
        /// </summary>
        public decimal CourseLength { get; set; }

        /// <summary>
        /// 是否必修 0:必修；1：选修
        /// </summary>
        public int IsMust { get; set; }


        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Teacher { get; set; }

        /// <summary>
        /// 授课地点
        /// </summary>
        public string RoomName { get; set; }

        public int IsSystem { get; set; }




        #endregion

        #region 扩展

        public string CourseNameStr
        {
            get
            {
                return CourseName.HtmlXssEncode();
            }
        }
        /// <summary>
        /// 获得的学时总和
        /// </summary>
        public decimal GetSumScore { get; set; }

        /// <summary>
        /// 考勤是否正常
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 考勤状态
        /// </summary>
        public string AttdenceFlag
        {
            get
            {
                return ((Enums.deptAttStatus)this.Status).ToString().Replace("待考勤", "待上传");
            }
        }

        /// <summary>
        /// 考勤状态
        /// </summary>
        public string AttdenceSingleFlag
        {
            get
            {
                return ((Enums.deptAttStatus)this.Status).ToString();
            }
        }

        /// <summary>
        /// 授课时间
        /// </summary>
        public string CourseTime
        {
            get
            {
                return StartTime.ToString("yyyy-MM-dd HH:mm") + " -- " + EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

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
                return string.IsNullOrEmpty(Teacher) ? "——" : Teacher;
            }
        }


        /// <summary>
        /// 框架内外
        /// </summary>
        public string IsSystemStr
        {
            get
            {
                return ((Enums.IsSystem)this.IsSystem).ToString();
            }
        }

        /// <summary>
        /// 计划内外
        /// </summary>
        public string IsPlanStr
        {
            get
            {
                if (IsYearPlan > 0)
                {
                    return "内";
                }
                else
                {
                    return "外";
                }
            }
        }
        /// <summary>
        /// 1:视频课程；2：部门自学
        /// </summary>
        public int CourseType { set; get; }
        /// <summary>
        /// 课程试卷关联表ID
        /// </summary>
        public int CoPaperID { set; get; }
        /// <summary>
        /// 课程ID
        /// </summary>
        public int CourseID { set; get; }
        /// <summary>
        /// 试卷ID
        /// </summary>
        public int PaperId { set; get; }
        /// <summary>
        /// 得分
        /// </summary>
        public decimal GetexamScore { set; get; }
        /// <summary>
        /// 总分
        /// </summary>
        public decimal ExamScore { set; get; }
        public int totalcount { get; set; }

        public int ApprovalFlag { get; set; }

        public decimal SumScore
        {
            get
            {
                if (this.ApprovalFlag == 1 || this.ApprovalFlag == -1)
                {
                    return GetSumScore < 0 ? 0 : GetSumScore;
                }
                return 0;
            }
        }

        /// <summary>
        /// 审批状态 0：未审批；1：已通过
        /// </summary>
        public int OpenApprovalFlag { get; set; }

        /// <summary>
        /// 0：未提交；1：已提交
        /// </summary>
        public int AttFlag { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public string ApprovalFlagStr
        {
            get
            {
                if (AttFlag == 0)
                {
                    if (OpenApprovalFlag == 0)
                        return "未提交";
                    else
                        return "已拒绝";
                }
                else if (AttFlag == -1)
                {
                    return "无";
                }
                else
                {
                    if (OpenApprovalFlag == 0)
                        return "未审批";
                    else
                        return "已通过";
                }
            }
        }


        public decimal percenterSumScore { get; set; }

        public decimal mustScore { get; set; }

        public decimal  selectScore { get; set; }
        #endregion


    }
}
