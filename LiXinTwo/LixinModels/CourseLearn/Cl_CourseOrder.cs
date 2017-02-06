using System;
namespace LiXinModels.CourseLearn
{
    /// <summary>
    /// Cl_CourseOrder:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Cl_CourseOrder
    {
        public Cl_CourseOrder()
        { }
        #region Model
        private int _id;
        private int? _courseid;
        private int _userid;
        private DateTime? _ordertime = Convert.ToDateTime("2000-1-1");
        private int? _orderstatus = 0;
        private int? _isleave = 0;
        private string _approvaluser;
        private string _approvalmemo = "";
        private string _reson = "";
        private int? _approvalflag = 0;
        private DateTime? _approvaldatetime = Convert.ToDateTime("2000-1-1");
        private DateTime? _leavetime = Convert.ToDateTime("2000-1-1");
        private DateTime? _approvallimittime = Convert.ToDateTime("2000-1-1");
        private DateTime? _orderendtime = Convert.ToDateTime("2000-1-1");
        private int? _ordertimes = 0;
        private int? _learnstatus = 0;
        private decimal? _getscore = 0M;
        private int? _passstatus = 0;
        private int? _isappoint = 0;
        private decimal? _attscore = 0;
        private int? _attflag = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CourseId
        {
            set { _courseid = value; }
            get { return _courseid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? OrderTime
        {
            set { _ordertime = value; }
            get { return _ordertime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OrderStatus
        {
            set { _orderstatus = value; }
            get { return _orderstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsLeave
        {
            set { _isleave = value; }
            get { return _isleave; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Reson
        {
            set { _reson = value; }
            get { return _reson; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ApprovalUser
        {
            set { _approvaluser = value; }
            get { return _approvaluser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ApprovalMemo
        {
            set { _approvalmemo = value; }
            get { return _approvalmemo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ApprovalFlag
        {
            set { _approvalflag = value; }
            get { return _approvalflag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ApprovalDateTime
        {
            set { _approvaldatetime = value; }
            get { return _approvaldatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LeaveTime
        {
            set { _leavetime = value; }
            get { return _leavetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ApprovalLimitTime
        {
            set { _approvallimittime = value; }
            get { return _approvallimittime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? OrderTimes
        {
            set { _ordertimes = value; }
            get { return _ordertimes; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LearnStatus
        {
            set { _learnstatus = value; }
            get { return _learnstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? GetScore
        {
            set { _getscore = value; }
            get { return _getscore; }
        }

        /// <summary>
        /// 是否获得学分标志位（补预定情况用）0：审批失败等原因没有获得学分；1：正常得学分；2：审批通过但是超出补预定次数上线而未获得学分
        /// </summary>
        public int GetScoreFlag { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int? PassStatus
        {
            set { _passstatus = value; }
            get { return _passstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? OrderEndTime
        {
            set { _orderendtime = value; }
            get { return _orderendtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsAppoint
        {
            set { _isappoint = value; }
            get { return _isappoint; }
        }

        public string CourseName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CourseStartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CourseEndTime
        {
            get;
            set;
        }
        public int? requestid { get; set; }
        public int? FtriggerFlag { get; set; }
        /// <summary>
        /// 补预定情况专用，0：未处理；1：审核通过；2：审核拒绝
        /// </summary>
        public int MakeUpApprovalFlag { set; get; }

        /// <summary>
        /// 撤销类型
        /// </summary>
        public int DropType { set; get; }
        /// <summary>
        /// 撤销理由
        /// </summary>
        public string DropReason { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? AttScore
        {
            set { _attscore = value; }
            get { return _attscore; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? AttFlag 
        {
            set { _attflag = value; }
            get { return _attflag; }
        }
        #endregion Model

        /// <summary>
        /// 请假时间
        /// </summary>
        public string LeaveTimeStr
        {
            get
            {
                if (LeaveTime.HasValue)
                    return LeaveTime.Value.ToString("yyyy-MM-dd HH:mm");
                return "——";
            }
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        public string ApprovalStatusStr
        {
            get
            {
                if (this.ApprovalFlag == -1)
                {
                    return "待审批";
                }
                return ((Enums.ApprovalStatus)ApprovalFlag).ToString();
            }
        }


        /// <summary>
        /// 审批时间
        /// </summary>
        public string ApprovalDateTimeStr
        {
            get
            {
                if (ApprovalFlag != 0)
                    return ApprovalDateTime.Value.ToString("yyyy-MM-dd HH:mm");
                return "——";
            }
        }

        public string ApprovalLimitTimeStr
        {
            get
            {
                return ApprovalLimitTime.Value.ToString("yyyy-MM-dd HH:mm");
            }
        }
        //请假逾时申请OA审核状态位
        public int TimeOutLeaveApprovalFlag { get; set; }
        #region 课程相关信息
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TeacherName { get; set; }
        public string Course_Name { get; set; }
        public string Course_Code { get; set; }
        public decimal CourseLength { get; set; }
        public int IsMust { get; set; }
        public string IsMustStr { get { return ((Enums.IsMust)this.IsMust).ToString(); } }
        public string CourseTime
        {
            get
            {
                return StartTime.ToString("yyyy-MM-dd HH:mm") + " -- " + EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
        public Int64 CourseTimesOrder { get; set; }
        public int TotalCourseTimes { get; set; }
        public string ApprovalRealName { get; set; }

        public string DeptCode { get; set; }
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
        public int NumberLimited { get; set; }
        #endregion

        #region 补预订相关信息
        /// <summary>
        /// 逾时申请人
        /// </summary>
        public string ApplyUserRealName { get; set; }
        /// <summary>
        /// 逾时申请人部门
        /// </summary>
        public string ApplyUserDeptName { get; set; }
        /// <summary>
        /// 逾时申请时间
        /// </summary>
        public DateTime? TimeOutDateTime { get; set; }
        /// <summary>
        /// 是否逾时申请
        /// </summary>
        public int IsTimeOut
        {
            get;
            set;
        }
        /// <summary>
        /// 申请理由
        /// </summary>
        public string TimeOutMemo { get; set; }
        /// <summary>
        /// 逾时申请 审批状态
        /// </summary>
        public int? TimeOutPassFlag
        {
            get;
            set;
        }
        /// <summary>
        /// 逾时申请 审批时间
        /// </summary>
        public DateTime? TimeOutApprovalDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 补预订时间
        /// </summary>
        public DateTime? TimeOutApplyTime { get; set; }

        public DateTime? MakeUpTime { get; set; }
        #endregion

        private string _realname;
        public string realname
        {
            get { return _realname; }
            set { _realname = value; }
        }
        public string bumen { get; set; }
        public int replynum { get; set; }
        public int sex { get; set; }
        public string TrainGrade { get; set; }
        public string JobNum { get; set; }
        public int TestTimes { get; set; }
        public int LevelScore { get; set; }
        public string DeptName { get; set; }
        public string userMobileNum { get; set; }
        public string userEmail { get; set; }


        public int Mylocation { get; set; }
        public int Myorderendlocation { get; set; }


        /// <summary>
        /// 考试通过分数线(试卷总分*试卷百分比)
        /// </summary>
        public int LevelScoreStr { get; set; }

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

        public string Name { get; set; }
        public string LeaveUserID { get; set; }

        public int totalCount { get; set; }

        public int ApplyCount { get; set; }


        public int TrainAppFlag { get; set; }

        private int _sourceType = 1;

        public int SourceType
        {
            get { return _sourceType; }
            set { _sourceType = value; }
        }


        /// <summary>
        /// 审批状态
        /// </summary>
        public string TrainAppFlagStr
        {
            get
            {
                return ((Enums.ApprovalStatus)TrainAppFlag).ToString();
            }
        }

        /// <summary>
        /// 申请时间
        /// </summary>
        public string TimeOutApplyTimeStr
        {
            get
            {
                if (this.TimeOutApplyTime.HasValue)
                {
                    return this.TimeOutApplyTime.Value.ToString("yyyy-MM-dd HH:mm");
                }
                return "--";
            }
        }

        public string AppReason { get; set; }


        public DateTime? TrainAppDatetime { get; set; }


        public string TrainAppDatetimeStr
        {
            get
            {
                if (this.TrainAppDatetime.HasValue)
                {
                    return this.TrainAppDatetime.Value.ToString("yyyy-MM-dd HH:mm");
                }
                return "--";
            }
        }

        public int CpaLearnStatus_CpaFlag { get; set; }

        public decimal CpaLearnStatus_GetLength { get; set; }

        public decimal EvlutionScore { get; set; }

        public decimal ExamScore { get; set; }

        /// <summary>
        /// 所内完成状态
        /// </summary>
        public string ComPletionStatu { get; set; }

      


        /// <summary>
        /// 一期所内学时
        /// </summary>
        public decimal tscore { get; set; }

        public decimal dScore { get; set; }

        /// <summary>
        /// 已获取的学时
        /// </summary>
        //public decimal t_dScore { get {
        //    return tscore + dScore;
        //} }
        public decimal suonei_t_d_cpaScore
        {
            get;
            set;
        }


        public decimal cpa_t_d_cpaScore
        {
            get;
            set;
        }
        /// <summary>
        /// 未完成的学时 等于目标学时-已获取的学时
        /// </summary>
        public decimal nopass_t_dScore { get; set; }

        /// <summary>
        /// 已获取cpa学时
        /// </summary>
        public decimal cpaScore { get; set; }

        /// <summary>
        /// 未完成的CPA学时
        /// </summary>
        public decimal nppass_cpaScore { get; set; }

        /// <summary>
        /// 通过次数
        /// </summary>
        public int OnlineTestPass { get; set; }

        public string ismoral { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public int deptid { get; set; }

        /// <summary>
        /// 课程类型1：集中课程 2：部门课程
        /// </summary>
        public int course_type { get; set; }

        /// <summary>
        /// 考勤状态
        /// </summary>
        public int co_cd { get; set; }

        public int dep_cd { get; set; }

        public int co_qq { get; set; }

        public int dep_qq { get; set; }

        public int co_zt { get; set; }

        public int dep_zt { get; set; }


        public int co_cd_zt { get; set; }

        public int dep_cd_zt { get; set; }

        /// <summary>
        /// 全部的违纪次数
        /// </summary>
        public int sumZT
        {
            get
            {
                return co_cd + dep_cd + co_qq + dep_qq + co_zt + dep_zt + co_cd_zt + dep_cd_zt;
            }
        }

        /// <summary>
        /// 集中 部门考勤状态
        /// </summary>
        public int AttendStatus { get; set; }

        public string iscpa { get; set; }


        /// <summary>
        /// 已完成培训周期CPA
        /// </summary>
        public decimal cpazhouqi_length { get; set; }

        /// <summary>
        /// 未完成培训周期CPA
        /// </summary>
        public decimal nopass_cpazhouqi_length { get; set; }


        public string cpa_statu { get; set; }

        public string Username { get; set; }

        public string cpazhouqi_statu { get; set; }

        public int isFree { get; set; }

    }

    /// <summary>
    /// TrainGradeSubscribeCount:实体类
    /// 记录课程下，每个[培训级别 或 部门分所]下，已报名人数
    /// </summary>
    [Serializable]
    public class NameSubscribeCount
    {
        public int num { get; set; }
        public string Name { get; set; }
        public int AllCount { get; set; }
        public int SubscribeCount { get; set; }
        public int Percent
        {
            get
            {
                if (AllCount == 0)
                    return 0;
                return SubscribeCount * 100 / AllCount;
            }
        }




    }
}

