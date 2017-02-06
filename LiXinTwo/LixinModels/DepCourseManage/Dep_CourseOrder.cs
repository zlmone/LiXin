using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;

namespace LiXinModels.DepCourseManage
{
    ///<summary>
    ///Dep_CourseOrder
    ///</summary>
    public partial class Dep_CourseOrder
    {
        #region Model
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
        /// OrderTime
        /// </summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// OrderStatus
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// LearnStatus
        /// </summary>
        public int LearnStatus { get; set; }
        /// <summary>
        /// GetScore
        /// </summary>
        public int GetScore { get; set; }
        /// <summary>
        /// PassStatus
        /// </summary>
        public string PassStatus { get; set; }
        /// <summary>
        /// AttScore
        /// </summary>
        public decimal AttScore { get; set; }
        /// <summary>
        /// EvlutionScore
        /// </summary>
        public decimal EvlutionScore { get; set; }
        /// <summary>
        /// ExamScore
        /// </summary>
        public decimal ExamScore { get; set; }
        /// <summary>
        /// 退订次数
        /// </summary>
        public int OrderTimes { get; set; }
        /// <summary>
        /// DepartSetId
        /// </summary>
        public int DepartSetId { get; set; }

        public int IsLeave { get; set; }

        public string Reason { get; set; }

        public DateTime LeaveTime { get; set; }

        public int ApprovalFlag { get; set; }

        public DateTime ApprovalTime { get; set; }

        public int ApprovalUserId { get; set; }

        public int IsAppoint { get; set; }

        /// <summary>
        /// 撤销理由
        /// </summary>
        public string DropReason { get; set; }
        /// <summary>
        /// 撤销类型
        /// </summary>
        public int DropType { get; set; }
        /// <summary>
        /// 审批人HRID
        /// </summary>
        public string ApprovalUser { set; get; }
        /// <summary>
        /// 审批截止时间
        /// </summary>
        public DateTime ApprovalLimitTime { set; get; }

        #endregion Model

        /// <summary>
        /// 学员姓名
        /// </summary>
        public string Realname { get; set; }

        public int Sex { get; set; }
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
        public string DeptName { get; set; }
        public string DeptCode { get; set; }
        
        public string TrainGrade { get; set; }

        public decimal AllScore
        {
            get
            {
                return AttScore + EvlutionScore + ExamScore;
            }
        }

        public DateTime? AttendceStartTime { get; set; }
        public DateTime? AttendceEndTime { get; set; }
        public string AttendceStartTimeStr
        {
            get
            {
                if (AttendceStartTime.HasValue)
                {
                    if (Convert.ToDateTime(AttendceStartTime).Year == 2050)
                    {
                        return "——";
                    }
                    else
                    {
                        return AttendceStartTime.Value.ToString("yyyy-MM-dd HH:mm");
                    }
                }
                else
                {
                    return "——";
                }
            }
        }
        public string AttendceEndTimeStr
        {
            get
            {
                if (AttendceEndTime.HasValue)
                {
                    if (Convert.ToDateTime(AttendceEndTime).Year == 2000)
                    {
                        return "——";
                    }
                    else
                    {
                        return AttendceEndTime.Value.ToString("yyyy-MM-dd HH:mm");
                    }
                }
                else
                {
                    return "——";
                }
            }
        }

        #region 考勤相关
        /// <summary>
        /// 课程开始时间
        /// </summary>
        public DateTime StartTime { set; get; }
        /// <summary>
        /// 课程结束时间
        /// </summary>
        public DateTime EndTime { set; get; }

        public string CourseTime
        {
            get
            {
                return StartTime.ToString("yyyy-MM-dd HH:mm") + " -- " + EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string Telephone { get; set; }
        /// <summary>
        /// 考勤状态(1：正常 2:缺勤 3:迟到 4:早退 5:迟到且早退)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 考勤结束标志 0：未提交；1：已提交 
        /// </summary>
        public int AttFlag { get; set; }
        /// <summary>
        /// 考勤审批状态(0-未审批，1-审批通过 2-审批拒绝)
        /// </summary>
        public int AttendceApprovalFlag { get; set; }
        /// <summary>
        /// 考勤审批状态()
        /// </summary>
        public string AttendceApprovalFlagStr
        {
            get
            {
                return AttendceApprovalFlag == 1
                           ? "考勤正常"
                           : (AttendceApprovalFlag == 2 ? "考勤不正常" : "待考勤");
            }
        }


  

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartID { set; get; }
        /// <summary>
        /// 课程学时分配
        /// </summary>
        public string CourseLengthDistribute { get; set; }

        /// <summary>
        /// 课程学时
        /// </summary>
        public decimal CourseLength { set; get; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobileNum { get; set; }
        #endregion
        public string Email { get; set; }
        #region == 考试相关 ==
        public int LevelScore { get; set; }
        /// <summary>
        /// 考试通过分数线(试卷总分*试卷百分比)
        /// </summary>
        public int LevelScoreStr { get; set; }

        /// <summary>
        /// 试卷总分
        /// </summary>
        public int ExampaperScore { get; set; }

        /// <summary>
        /// 课程关联试卷表主键ID
        /// </summary>
        public int CoPaperID { get; set; }
        /// <summary>
        /// 试卷ID
        /// </summary>
        public int PaperId { get; set; }
        /// <summary>
        /// 考试取得的总分
        /// </summary>
        public decimal GetexamScore { get; set; }


        public decimal tmpScore { get; set; }
        public decimal GetLength { get; set; }
        public int Way { get; set; }
        public int tbExamstudentId { get; set; }

        public int DoExamStatus { get; set; }

        
        /// <summary>
        /// 考试次数
        /// </summary>
        public int ExamTestTimes { get; set; }
        /// <summary>
        /// 允许次数
        /// </summary>
        public int TestTimes { get; set; }
        #endregion

        #region 扩展字段

        /// <summary>
        /// 分所名称
        /// </summary>
        public string DepartSetName { set; get; }


        /// <summary>
        /// 是否开放标志  0：不开放；1：开放 
        /// </summary>
        public int OpenFlag { get; set; }

        /// <summary>
        /// 报名人数
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 考勤人数
        /// </summary>
        public int AttCount { get; set; }


        /// <summary>
        /// 考勤状态
        /// </summary>
        public string StatusStr
        {
            get
            {
                return ((Enums.deptAttStatus)this.Status).ToString();
            }
        }
        #endregion
        public int totalcount { get; set; }

        public string Sort { get; set; }

        public string SortStr
        {
            get
            {
                var s = "";
                if (!string.IsNullOrEmpty(Sort))
                {
                    if (Sort.Contains("1"))
                        s += ((Enums.Sort)1).ToString() + "，";
                    if (Sort.Contains("2"))
                        s += ((Enums.Sort)2).ToString() + "，";
                    if (Sort.Contains("3"))
                        s += ((Enums.Sort)3).ToString() + "，";
                    if (Sort.Contains("4"))
                        s += ((Enums.Sort)4).ToString() + "，";
                }
                if (s.Length > 0)
                    return s.TrimEnd('，');
                return s;
            }
        }

        public string CourseName { get; set; }

        public int ExamALLTestTimes { get; set; }

        public int Length { get; set; }

        public string CourseNameStr
        {
            get
            {
                return CourseName.HtmlXssEncode();
            }
        }

        public string CPA { get; set; }

        public string CPANo { get; set; }

        /// <summary>
        /// 是否预定
        /// </summary>
        public string OrderStr
        {
            get
            {
                if (OrderStatus == 3)
                {
                    return "否";
                }
                else
                {
                    return "是";
                }
            }
        }

        public int CourseApprovalFlag { get; set; }

        /// <summary>
        /// 课后评估数
        /// </summary>
        public int AfterCount { get; set; }

        public int DeptSurveyCount { get; set; }

        public int AttendCount { get; set; }

        /// <summary>
        /// 每个学员所有讲师评估的中位数
        /// </summary>
        public int SubjectiveAnswer { get; set; }

        public int TeacherId { get; set; }

    }
}
