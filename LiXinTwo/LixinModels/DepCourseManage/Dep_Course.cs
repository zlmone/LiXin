using LiXinModels.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;


namespace LiXinModels.DepCourseManage
{
    public class Dep_Course
    {
        #region model
        /// <summary>
        /// Id
        /// </summary>		
        public int Id { get; set; }

        /// <summary>
        /// CourseName
        /// </summary>		
        public string CourseName { get; set; }

        /// <summary>
        /// Code
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        /// YearPlan
        /// </summary>		
        public int YearPlan { get; set; }

        /// <summary>
        /// Year
        /// </summary>		
        public int Year { get; set; }

        /// <summary>
        /// Month
        /// </summary>		
        public string Month { get; set; }

        /// <summary>
        /// Day
        /// </summary>		
        public int Day { get; set; }

        /// <summary>
        /// NextStartTime
        /// </summary>		
        public DateTime NextStartTime { get; set; }

        /// <summary>
        /// OpenLevel
        /// </summary>		
        public string OpenLevel { get; set; }

        /// <summary>
        /// IsMust
        /// </summary>		
        public int IsMust { get; set; }

        /// <summary>
        /// Way
        /// </summary>		
        public int Way { get; set; }

        /// <summary>
        /// Teacher
        /// </summary>		
        public string Teacher { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>		
        public DateTime StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>		
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Sort
        /// </summary>		
        public string Sort { get; set; }

        /// <summary>
        /// CourseLength
        /// </summary>		
        public decimal CourseLength { get; set; }

        /// <summary>
        /// RoomId
        /// </summary>		
        public int RoomId { get; set; }

        /// <summary>
        /// NumberLimited
        /// </summary>		
        public int NumberLimited { get; set; }

        /// <summary>
        /// IsCPA
        /// </summary>		
        public int IsCPA { get; set; }

        /// <summary>
        /// IsOrder
        /// </summary>		
        public int IsOrder { get; set; }

        /// <summary>
        /// IsLeave
        /// </summary>		
        public int IsLeave { get; set; }

        /// <summary>
        /// IsRT
        /// </summary>		
        public int IsRT { get; set; }

        /// <summary>
        /// AttFlag
        /// </summary>		
        public int AttFlag { get; set; }

        /// <summary>
        /// AttStatus
        /// </summary>		
        public int AttStatus { get; set; }

        /// <summary>
        /// OpenFlag
        /// </summary>		
        public int OpenFlag { get; set; }

        /// <summary>
        /// OpenGroupObject
        /// </summary>		
        public string OpenGroupObject { get; set; }

        /// <summary>
        /// OpenDepartObject
        /// </summary>		
        public string OpenDepartObject { get; set; }

        /// <summary>
        /// OpenPerson
        /// </summary>		
        public string OpenPerson { get; set; }

        /// <summary>
        /// IsTest
        /// </summary>		
        public int IsTest { get; set; }

        /// <summary>
        /// IsPing
        /// </summary>		
        public int IsPing { get; set; }

        /// <summary>
        /// SurveyPaperId
        /// </summary>		
        public string SurveyPaperId { get; set; }

        /// <summary>
        /// Memo
        /// </summary>		
        public string Memo { get; set; }

        /// <summary>
        /// CourseFrom
        /// </summary>		
        public int CourseFrom { get; set; }

        /// <summary>
        /// StopOrderFlag
        /// </summary>		
        public int StopOrderFlag { get; set; }

        /// <summary>
        /// StopDucueFlag
        /// </summary>		
        public int StopDucueFlag { get; set; }

        /// <summary>
        /// ReturnTimes
        /// </summary>		
        public int ReturnTimes { get; set; }

        /// <summary>
        /// AfterOrderTimes
        /// </summary>		
        public int AfterOrderTimes { get; set; }

        /// <summary>
        /// Publishflag
        /// </summary>		
        public int Publishflag { get; set; }

        /// <summary>
        /// LastUpdateTime
        /// </summary>		
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// PreAdviceConfigTime
        /// </summary>		
        public decimal PreAdviceConfigTime { get; set; }

        /// <summary>
        /// AfterEvlutionConfigTime
        /// </summary>		
        public decimal AfterEvlutionConfigTime { get; set; }

        /// <summary>
        /// CpaTeacher
        /// </summary>		
        public string CpaTeacher { get; set; }

        /// <summary>
        /// PreCourseTime
        /// </summary>		
        public DateTime PreCourseTime { get; set; }

        /// <summary>
        /// CourseTimes
        /// </summary>		
        public int CourseTimes { get; set; }

        /// <summary>
        /// TrainDays
        /// </summary>		
        public int TrainDays { get; set; }

        /// <summary>
        /// CourseAddress
        /// </summary>		
        public string CourseAddress { get; set; }

        /// <summary>
        /// CourseObjectMemo
        /// </summary>		
        public string CourseObjectMemo { get; set; }

        /// <summary>
        /// IsDelete
        /// </summary>		
        public int IsDelete { get; set; }

        /// <summary>
        /// CourseLengthDistribute
        /// </summary>		
        public string CourseLengthDistribute { get; set; }

        /// <summary>
        /// IsSystem
        /// </summary>		
        public int IsSystem { get; set; }

        /// <summary>
        /// IsYearPlan
        /// </summary>		
        public int IsYearPlan { get; set; }

        /// <summary>
        /// IsOpenSub
        /// </summary>		
        public int IsOpenSub { get; set; }

        /// <summary>
        /// courseProvide
        /// </summary>		
        public string courseProvide { get; set; }

        /// <summary>
        /// StudentRequirement
        /// </summary>		
        public string StudentRequirement { get; set; }

        /// <summary>
        /// remark
        /// </summary>		
        public string remark { get; set; }

        /// <summary>
        /// AdFlag
        /// </summary>		
        public int AdFlag { get; set; }

        /// <summary>
        /// IsOpenOthers
        /// </summary>		
        public int IsOpenOthers { get; set; }

        /// <summary>
        /// DeptId
        /// </summary>		
        public int DeptId { get; set; }

        /// <summary>
        /// OpenUserId
        /// </summary>		
        public int OpenUserId { get; set; }

        /// <summary>
        /// OpenSubmitTime
        /// </summary>		
        public DateTime OpenSubmitTime { get; set; }

        /// <summary>
        /// ApprovalUserId
        /// </summary>		
        public int ApprovalUserId { get; set; }

        /// <summary>
        /// ApprovalTime
        /// </summary>		
        public DateTime ApprovalTime { get; set; }

        /// <summary>
        /// OpenCourseId
        /// </summary>		
        public int OpenCourseId { get; set; }

        /// <summary>
        /// OpenReason
        /// </summary>		
        public string OpenReason { get; set; }
        /// <summary>
        /// 课程负责人
        /// </summary>
        public string AttUserId { set; get; }
        #endregion

        /// <summary>
        /// 开放对象
        /// </summary>
        public string OpenObject { get; set; }

        public int DeptHasEntered { get; set; }

        /// <summary>
        /// 该用户是否还具有 讲师身份 0 不具有  1 内部 2 外聘
        /// </summary>
        public int TeacherIsTeacher { get; set; }

        /// <summary>
        /// 讲师是否被删除
        /// </summary>
        public int TeacherIsDelete { get; set; }

        public string TeacherName
        {
            get;
            set;
        }
        /// <summary>
        /// 考勤
        /// </summary>
        public string courseattend { get; set; }
        /// <summary>
        /// 在线测试
        /// </summary>
        public string courseonlinetest { get; set; }
        /// <summary>
        /// 课后评估
        /// </summary>
        public string courseafter { get; set; }

        public string Realname { get; set; }

        public string PayGrade { get; set; }

        /// <summary>
        /// 1：内部培训；2：社会招聘；3：新进员工；4：实习生
        /// </summary>
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

        public string CourseStatus
        {
            get
            {
                if (DateTime.Now > StartTime && DateTime.Now < EndTime)
                    return "进行中";
                else if (DateTime.Now < StartTime)
                    return "预订中";
                else if (DateTime.Now > EndTime)
                {
                    return "已结束";
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 预订、指定
        /// </summary>
        public string IsOrderStr
        {
            get
            {
                return ((Enums.CourseIsOrder)this.IsOrder).ToString();
            }
        }

        public string RoomName
        {
            get;
            set;
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

        public string CourseNameStr
        {
            get
            {
                return CourseName.HtmlXssEncode();
            }
        }


        /// <summary>
        /// 框架内外
        /// </summary>
        public string IsSystemStr
        {
            get
            {
                if (IsSystem > 0)
                {
                    return "内";
                }
                else
                {
                    return "外";
                }
            }
        }

        public decimal AttScore { get; set; }
        public decimal EvlutionScore { get; set; }
        public decimal ExamScore { get; set; }

        public decimal AllScore
        {
            get
            {
                return AttScore + EvlutionScore + ExamScore;
            }
        }


        public int CourseId { get; set; }
        /// <summary>
        /// 已报名总人数（自主+指定）
        /// </summary>
        public int HasEntered
        {
            get;
            set;
        }
        /// <summary>
        /// 指定报名人数
        /// </summary>
        public int AssignUserCount { set; get; }

        /// <summary>
        /// 是否可以预订
        /// </summary>
        public string OrderStr
        {
            get
            {
                //if (IsOrder == 1 || IsOrder == 3)
                //    return "是";
                //else
                //    return "否";
                if (IsOrder == 1)
                    return "预订";
                else if (IsOrder == 2)
                    return "指定";
                else if (IsOrder == 3)
                    return "兼有";
                else
                    return "无";

            }
        }

        public int DeptCourseStatus
        {
            get
            {
                if (StartTime <= DateTime.Now)
                    return 3;
                return 1;
            }
        }

        /// <summary>
        /// 同一课程编号下的课次 顺序
        /// </summary>
        public Int64 CourseTimesOrder { get; set; }
        /// <summary>
        /// 同一课程编号下 已经开设的课程总数
        /// </summary>
        public int TotalCourseTimes { get; set; }

        /// <summary>
        /// 课程状态
        /// </summary>   CPA 课程中引用 , 修改请告知Richter
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

        public string txt_year_ids
        {
            get;
            set;
        }


        #region 考勤状态

        public DateTime AttendceStartTime { get; set; }
        public DateTime AttendceEndTime { get; set; }

        public string Status
        {
            get
            {
                if (AttFlag == 0)
                {
                    return "正常";
                }
                else if (AttFlag == 1)
                {
                    if (AttendceStartTime == DateTime.MinValue || AttendceStartTime == Convert.ToDateTime("2050-1-1"))
                    {
                        if (StartTime > DateTime.Now)
                        {
                            return "课程未开始";
                        }
                        else if (StartTime < DateTime.Now)
                        {
                            if (EndTime > DateTime.Now)
                            {
                                return "等待考勤上传";
                            }
                            else
                            {
                                return "缺勤";
                            }
                        }
                        //return StartTime > DateTime.Now ? "课程未开始" : "等待考勤上传";
                    }
                    if (AttendceStartTime > StartTime)
                    {
                        return "迟到" + (AttendceStartTime.Subtract(StartTime).TotalMinutes).ToString("0.00") + "(min)";
                    }
                    if (AttendceStartTime <= StartTime)
                    {
                        return "正常";
                    }
                }
                else if (AttFlag == 2)
                {
                    if (AttendceEndTime == DateTime.MinValue || AttendceEndTime == Convert.ToDateTime("2000-1-1"))
                    {
                        //return StartTime > DateTime.Now ? "课程未开始" : "等待考勤上传";
                        if (StartTime > DateTime.Now)
                        {
                            return "课程未开始";
                        }
                        else if (StartTime < DateTime.Now)
                        {
                            if (EndTime > DateTime.Now)
                            {
                                return "等待考勤上传";
                            }
                            else
                            {
                                return "缺勤";
                            }
                        }
                    }
                    if (AttendceEndTime < EndTime)
                    {
                        return "早退" + (AttendceEndTime.Subtract(EndTime).TotalMinutes * -1).ToString("0.00") + "(min)";
                    }
                    if (AttendceEndTime >= EndTime)
                    {
                        return "正常";
                    }
                }
                else if (AttFlag == 3)
                {
                    if (AttendceStartTime == DateTime.MinValue && AttendceEndTime == DateTime.MinValue)
                    {
                        // return StartTime > DateTime.Now ? "课程未开始" : "等待考勤上传";
                        if (StartTime > DateTime.Now)
                        {
                            return "课程未开始";
                        }
                        else if (StartTime < DateTime.Now)
                        {
                            if (EndTime > DateTime.Now)
                            {
                                return "等待考勤上传";
                            }
                            else
                            {
                                return "缺勤";
                            }
                        }
                    }
                    if (AttendceStartTime == Convert.ToDateTime("2050-1-1") &&
                        AttendceEndTime == Convert.ToDateTime("2000-1-1"))
                    {
                        //return StartTime > DateTime.Now ? "课程未开始" : "等待考勤上传";
                        if (StartTime > DateTime.Now)
                        {
                            return "课程未开始";
                        }
                        else if (StartTime < DateTime.Now)
                        {
                            if (EndTime > DateTime.Now)
                            {
                                return "等待考勤上传";
                            }
                            else
                            {
                                return "缺勤";
                            }
                        }
                    }
                    if (AttendceStartTime == Convert.ToDateTime("2050-1-1") &&
                        AttendceEndTime != Convert.ToDateTime("2000-1-1"))
                    {
                        if (AttendceEndTime >= EndTime)
                        {
                            return "迟到";
                        }
                        else
                        {
                            return "迟到/早退<span class='f11'>" +
                                   (AttendceEndTime.Subtract(EndTime).TotalMinutes * -1).ToString("0.00") + "(min)</span>";
                        }

                    }
                    if (AttendceStartTime != Convert.ToDateTime("2050-1-1") &&
                        AttendceEndTime == Convert.ToDateTime("2000-1-1"))
                    {
                        if (AttendceStartTime <= StartTime)
                        {
                            return "早退";
                        }
                        else
                        {
                            return "迟到<span class='f11'>" +
                                   (AttendceStartTime.Subtract(StartTime).TotalMinutes).ToString("0.00") +
                                   "(min)</span>/早退";
                        }
                    }
                    if (AttendceStartTime != Convert.ToDateTime("2050-1-1") &&
                        AttendceEndTime != Convert.ToDateTime("2000-1-1"))
                    {
                        if (AttendceStartTime <= StartTime && AttendceEndTime >= EndTime)
                        {
                            return "正常";
                        }
                        else if (AttendceStartTime > StartTime && AttendceEndTime > EndTime)
                        {
                            return "迟到" + (AttendceStartTime.Subtract(StartTime).TotalMinutes).ToString("0.00") +
                                   "(min)";
                        }
                        else if (AttendceStartTime > StartTime && AttendceEndTime < EndTime)
                        {
                            return "迟到<span class='f11'>" +
                                   (AttendceStartTime.Subtract(StartTime).TotalMinutes).ToString("0.00") +
                                   "(min)</span>/早退<span class='f11'>" +
                                   (AttendceEndTime.Subtract(EndTime).TotalMinutes * -1).ToString("0.00") + "(min)</span>";
                            //<span class='f11'>(" + AttendceStartTime.Subtract(StartTime).TotalMinutes +"min)</span>
                        }
                        else if (AttendceStartTime <= StartTime && AttendceEndTime < EndTime)
                        {
                            return "早退" + (AttendceEndTime.Subtract(EndTime).TotalMinutes * -1).ToString("0.00") + "(min)";
                        }
                    }
                }

                return "";
            }
        }
        #endregion

        /// <summary>
        /// 课后评估，问题与选项
        /// </summary>
        public Survey_Exampaper AfterCourseAssess { get; set; }

        /// <summary>
        /// 课后评估【对讲师的评估】，问题与选项
        /// </summary>
        public Survey_Exampaper AfterCourseTeacherExam { get; set; }

        public double Survey_TeacherScoreDouble { get; set; }

        /// <summary>
        /// 课程评估平均分
        /// </summary>
        public string Survey_TeacherScore
        {
            get
            {
                return this.Survey_TeacherScoreDouble == -1 ? "N/A" : this.Survey_TeacherScoreDouble.ToString("0.00");
            }
        }

        public string CourseTypeFrom { get; set; }
        public int LinkDeptId { get; set; }
        public int totalcount { get; set; }

        #region == 部门开放至全所审批 ==

        /// <summary>
        /// 是否发送消息：0：不发送，1-发送邮件，2-发送短信，-1：发送邮件+短信
        /// </summary>
        public int IsSendMessage { get; set; }

        /// <summary>
        /// 是否需请假 0-否 1-是
        /// </summary>
        public string IsLeaveStr
        {
            get
            {
                return ((Enums.IsLeave)this.IsLeave).ToString();
            }
        }
        /// <summary>
        /// 是否折算CPA 0-否 1-是
        /// </summary>
        public string IsCPAStr
        {
            get
            {
                return ((Enums.IsCPA)this.IsCPA).ToString();
            }
        }

        public string IsMustStr
        {
            get { return IsMust == 0 ? "必修" : "选修"; }
        }
        /// <summary>
        /// 授课时间
        /// </summary>
        public string TeacherCourseTime
        {
            get
            {
                return ("0001-01-01 00:00".Equals(StartTime.ToString("yyyy-MM-dd HH:mm")) ? "" : StartTime.ToString("yyyy-MM-dd HH:mm"))
                    + " -- "
                    + ("0001-01-01 00:00".Equals(EndTime.ToString("yyyy-MM-dd HH:mm")) ? "" : EndTime.ToString("yyyy-MM-dd HH:mm"));
            }
        }
        public string StatusShow
        {
            get
            {
                if (DateTime.Now >= StartTime && DateTime.Now <= EndTime)
                    return "进行中";
                else if (DateTime.Now < StartTime)
                    return "未开始";
                else
                    return "已结束";
            }
        }
        /// <summary>
        /// 开课时间
        /// </summary>
        public string StartTimeStr
        {
            get
            {
                return StartTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTimeStr
        {
            get
            {
                return EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
        /// <summary>
        /// 提交人
        /// </summary>
        public string OpenUserName
        {
            get;
            set;
        }
        /// <summary>
        /// 提交部门
        /// </summary>
        public string DeptName
        {
            get;
            set;
        }
        /// <summary>
        /// 审批人
        /// </summary>
        public string ApprovalUserName
        {
            get;
            set;
        }
        /// <summary>
        /// 审批人显示
        /// </summary>
        public string ApprovalUserNameStr
        {
            get { return string.IsNullOrWhiteSpace(ApprovalUserName) ? "--" : ApprovalUserName; }
        }
        /// <summary>
        /// 审批时间
        /// </summary>
        public string ApprovalTimeStr
        {
            get
            {
                if ("0001-01-01".Equals(ApprovalTime.ToString("yyyy-MM-dd")))
                {
                    return "--";
                }
                return ApprovalTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
        /// <summary>
        /// 审批状态 0:未开放；1：待审批；2：审批通过；3：审批拒绝
        /// </summary>
        public string IsOpenOthersStr
        {
            get
            {
                return ((Enums.OpenApprovalStatus)IsOpenOthers).ToString();
            }
        }
        /// <summary>
        /// 开课方式 0:部门/分所自学；非0：开放至全所
        /// </summary>
        public string IsOpenOthersShow
        {
            get
            {
                return IsOpenOthers == 0 ? "部门/分所自学" : "开放至全所";
            }
        }

        /// <summary>
        /// 是否部门 0:部门；1：分所 默认为0
        /// </summary>
        public int IsDeptFlag
        {
            get { return string.IsNullOrWhiteSpace(DeptName) ? 0 : DeptName.Contains("上海") ? 0 : 1; }
        }

        /// <summary>
        /// 是否可审批 0-不可以；1-可以
        /// </summary>
        public int IsCheck
        {
            get
            {
                return IsOpenOthers == 1 ? 1 : 0;
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

        public int attApprovalFlag { get; set; }



        private int? _mystatus;

        /// <summary>
        /// 我对于该课程的状态
        /// 4: 无状态[还没预订] null 【还可报名】
        /// 1：预订
        /// 0：退订
        /// 3：补预订
        /// 2：排队中
        /// 5：请假中
        /// 6：请假成功
        /// 7：请假失败
        /// </summary>
        public int? MyStatus
        {
            set
            {
                _mystatus = value;
            }
            get
            {
                if (_mystatus.HasValue)
                {
                    if (_mystatus == 1)
                    {
                        if (_myleave == 1)
                        {
                            //if (_approvalflag > 0)
                            //{
                            //    return 5;
                            //}
                            if (_approvalflag == 0)
                            {
                                return 5;
                            }
                            else if (_approvalflag == 1)
                            {
                                return 6;
                            }
                            else if (_approvalflag == 2)
                            {
                                return 7;
                            }
                            return 5;
                        }
                    }
                    if (_mystatus == 0 && StartTime < DateTime.Now)
                        return 8;
                    return _mystatus;
                }
                if (StartTime >= DateTime.Now && StopOrderFlag == 0)
                    return 4;
                if (StartTime >= DateTime.Now && StopOrderFlag == 1)
                    return 9;
                return 8;
            }
        }

        public string MyStatusStr
        {
            get
            {
                var str = "预订成功";
                if (IsAppoint == 1)
                    str = "指定";
                else if (IsAppoint == 2)
                    str = "考勤指定";
                switch (MyStatus)
                {
                    case 0:
                        //return "退订成功";
                        return "待预订";
                    case 1:
                        return str;
                    case 2:
                        return str + "[排队中]";
                    case 3:
                        return "补预订";

                    case 4:
                        return "待预订";
                    case 5:
                        return str + "[请假中]";
                    case 6:
                        return "请假成功";
                    case 7:
                        return str + "[请假失败]";
                    case 8:
                        return "已过期";
                    case 9:
                        return "关闭预订";
                    default:
                        return "";
                }
            }
        }

        public int IsAppoint { get; set; }

        /// <summary>
        /// 我对于该课程的操作状态
        /// 0：可预订、不可退订、不可请假
        /// 1：不可预订、可退订、可请假
        /// 2：不可预订、可退订、不可请假
        /// 3：不可预订、不可退订、不可请假
        /// 4：不可预订、不可退订、可请假
        /// </summary>
        public int MyDoStatus
        {
            get
            {
                if (StartTime <= DateTime.Now)
                    return 3;
                else
                {
                    switch (MyStatus.Value)
                    {
                        case 0:
                            ////退订时间在排队递补时间之后的，不可再预订————暂时先这么做吧
                            ////add by guohl
                            ////2013-03-27
                            //if (_approvaldatetime > _orderendtime)
                            //    return 3;
                            return 0;
                        case 1:
                            if (IsAppoint > 0)
                                return 4;
                            if (IsLeave == 1)
                                return 4;
                            return 1;
                        case 2:
                            //update by guohl 
                            //2013-04-25
                            //只要在排队中，都可退订
                            //if (IsAppoint > 0)
                            //    return 3;
                            //if (IsLeave == 1)
                            //    return 3;
                            return 2;
                        case 3:
                            return 3;
                        case 4:
                            return 0;
                        case 5:
                            if (IsAppoint > 0)
                                return 3;
                            if (IsLeave == 1)
                                return 3;
                            return 2;
                        case 6:
                            return 3;
                        case 7:
                            if (IsAppoint > 0)
                                return 4;
                            if (IsLeave == 1)
                                return 4;
                            return 1;
                        default:
                            break;
                    }
                }
                return 3;
            }
        }

        //private DateTime _starttime;

        public int? courseOrderId
        {
            get;
            set;
        }

        public int? UserId
        {
            get;
            set;
        }

        public decimal CoAvg { get; set; }
        public decimal TeAvg { get; set; }

        private DateTime? _ordertime;
        public DateTime? OrderTime
        {
            set { _ordertime = value; }
            get { return _ordertime; }
        }
        private int? _myleave;
        /// <summary>
        /// 是否请假
        /// 1：已请
        /// 0：未请
        /// </summary>
        public int? MyLeave
        {
            set
            {
                _myleave = value;
            }
            get
            {
                return _myleave;
            }
        }

        /// <summary>
        /// 请假审批过期时间
        /// </summary>
        public DateTime AppTime { get; set; }

        public DateTime LeaveTime { get; set; }

        public string DefaultApp
        {
            get
            {
                var str = "预订成功";
                if (IsAppoint == 1)
                    str = "指定";
                if (AppTime < DateTime.Now && ApprovalFlag == 0 && MyLeave == 1)
                {
                    str += "[请假失败]";
                }
                else
                {
                    str = "";
                }
                return str;

            }
        }

        private int? _approvalflag;
        public int OrderStatus { get; set; }

        /// <summary>
        /// 计划内的课程数量
        /// </summary>
        public int NowOpenCourseCountnei { get; set; }

        /// <summary>
        /// 计划外的课程数量
        /// </summary>
        public int NowOpenCourseCountwai { get; set; }
        /// <summary>
        /// 请假审批状态
        /// 0：未审批
        /// 1：审批通过
        /// 2：审批拒绝
        /// </summary>
        public int? ApprovalFlag
        {
            set
            {
                _approvalflag = value;
            }
            get
            {
                return _approvalflag;
            }
        }
        /// <summary>
        /// 否联合上报 0：否；1：是
        /// </summary>
        public int Year_isopen { get; set; }

        public int ResourceCount { get; set; }

        public int AttCount { get; set; }
        #endregion

        #region == 部门自学 ==
        /// <summary>
        /// 预订人数
        /// </summary>
        public int YuDingUserCount { get; set; }
        /// <summary>
        /// 指定人数
        /// </summary>
        public int ZhiDingUserCount { get; set; }
        /// <summary>
        /// 出勤人数
        /// </summary>
        public int ChuQinUserCount { get; set; }
        /// <summary>
        /// 课后评估人数(已评人数)
        /// </summary>
        public int HasPingUserCount { get; set; }
        /// <summary>
        /// 在线测试人数(已考人数)
        /// </summary>
        public int OnlineTestUserCount { get; set; }
        /// <summary>
        /// 在线测试试卷ID
        /// </summary>
        public int OnlineExampaperId { get; set; }
        /// <summary>
        /// 部门考勤状态
        /// </summary>
        public int DeptApprovalFlag { get; set; }
        /// <summary>
        /// 部门考勤状态显示
        /// </summary>
        public string DeptApprovalFlagStr
        {
            get
            {
                var str = "";
                switch (this.DeptApprovalFlag)
                {
                    case 0:
                        str = "未审批";
                        break;
                    case 1:
                        str = "审批通过";
                        break;
                    case 2:
                        str = "审批退回";
                        break;
                    case 3:
                        str = "未提交";
                        break;
                }
                return str;
            }
        }




        public int isOrderStatus { get; set; }
        #endregion

        #region == 部门开课跟踪 ==
        /// <summary>
        /// 计划开课数
        /// </summary>
        public int PlanOpenCourseCount { get; set; }
        /// <summary>
        /// 实际开课数
        /// </summary>
        public int NowOpenCourseCount { get; set; }
        /// <summary>
        /// 已考勤课程数
        /// </summary>
        public int HasAttendceCourseCount { get; set; }
        /// <summary>
        /// 计划课程总学时
        /// </summary>
        public decimal PlanCourseLengthSum { get; set; }
        /// <summary>
        /// 实际课程总学时
        /// </summary>
        public decimal NowCourseLengthSum { get; set; }
        #endregion

    }
}
