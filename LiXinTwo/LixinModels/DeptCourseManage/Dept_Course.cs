﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Survey;
using LiXinCommon;

namespace LiXinModels.DeptCourseManage
{
    [Serializable]
    public partial class Dept_Course
    {
         #region model

        private string _coursename;


        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// CourseName
        /// </summary>
        public string CourseName
        {
            get { return _coursename; }
            set { _coursename = value; }
        }
        /// <summary>
        /// Code
        /// </summary>
        public string Code
        {
            get;
            set;
        }
        /// <summary>
        /// Year
        /// </summary>
        public int Year
        {
            get;
            set;
        }
        /// <summary>
        /// Month
        /// </summary>
        public string Month
        {
            get;
            set;
        }
        /// <summary>
        /// Day
        /// </summary>
        public int Day
        {
            get;
            set;
        }
        /// <summary>
        /// OpenLevel
        /// </summary>
        public string OpenLevel
        {
            get;
            set;
        }
        /// <summary>
        /// IsMust
        /// </summary>
        public int IsMust
        {
            get;
            set;
        }
        /// <summary>
        /// Way
        /// </summary>
        public int Way
        {
            get;
            set;
        }
        /// <summary>
        /// Teacher
        /// </summary>
        public string Teacher
        {
            get;
            set;
        }
        private DateTime _starttime;
        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime
        {
            get { return _starttime; }
            set { _starttime = value; }
        }
        private DateTime _endtime;
        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime
        {
            get { return _endtime; }
            set { _endtime = value; }
        }
        /// <summary>
        /// Sort
        /// </summary>
        public string Sort
        {
            get;
            set;
        }
        /// <summary>
        /// CourseLength
        /// </summary>
        public decimal CourseLength
        {
            get;
            set;
        }
        /// <summary>
        /// RoomId
        /// </summary>
        public int RoomId
        {
            get;
            set;
        }
        private int _numberlimited;
        /// <summary>
        /// NumberLimited
        /// </summary>
        public int NumberLimited
        {
            get
            {
                return _numberlimited;
            }
            set
            {
                _numberlimited = value;
            }
        }

        public int DeptNumberLimited { get; set; }

        public int LimitNumber { set; get; }

        /// <summary>
        /// IsCPA
        /// </summary>
        public int IsCPA
        {
            get;
            set;
        }
        /// <summary>
        /// IsOrder
        /// </summary>
        public int IsOrder
        {
            get;
            set;
        }
        /// <summary>
        /// IsLeave
        /// </summary>
        public int IsLeave
        {
            get;
            set;
        }
        /// <summary>
        /// IsRT
        /// </summary>
        public int IsRT
        {
            get;
            set;
        }
        /// <summary>
        /// OpenFlag
        /// </summary>
        public int OpenFlag
        {
            get;
            set;
        }
        /// <summary>
        /// OpenGroupObject
        /// </summary>
        public string OpenGroupObject
        {
            get;
            set;
        }
        /// <summary>
        /// OpenDepartObject
        /// </summary>
        public string OpenDepartObject
        {
            get;
            set;
        }
        /// <summary>
        /// IsTest
        /// </summary>
        public int IsTest
        {
            get;
            set;
        }
        /// <summary>
        /// IsPing
        /// </summary>
        public int IsPing
        {
            get;
            set;
        }
        /// <summary>
        /// SurveyPaperId
        /// </summary>
        public string SurveyPaperId
        {
            get;
            set;
        }
        /// <summary>
        /// Memo
        /// </summary>
        public string Memo
        {
            get;
            set;
        }
        /// <summary>
        /// CourseFrom
        /// </summary>
        public int CourseFrom
        {
            get;
            set;
        }
        /// <summary>
        /// StopOrderFlag
        /// </summary>
        public int StopOrderFlag
        {
            get;
            set;
        }
        /// <summary>
        /// StopDucueFlag
        /// </summary>
        public int StopDucueFlag
        {
            get;
            set;
        }
        /// <summary>
        /// ReturnTimes
        /// </summary>
        public int ReturnTimes
        {
            get;
            set;
        }
        /// <summary>
        /// AfterOrderTimes
        /// </summary>
        public int AfterOrderTimes
        {
            get;
            set;
        }
        /// <summary>
        /// 在课程开始前多少小时之前可以进行课前建议
        /// </summary>
        public decimal PreAdviceConfigTime { get; set; }

        /// <summary>
        /// 课后多少小时内可以进行评估
        /// </summary>
        public decimal AfterEvlutionConfigTime { get; set; }

        /// <summary>
        /// Publishflag
        /// </summary>
        public int Publishflag
        {
            get;
            set;
        }
        /// <summary>
        /// LastUpdateTime
        /// </summary>
        public DateTime LastUpdateTime
        {
            get;
            set;
        }
        /// <summary>
        /// IsDelete
        /// </summary>
        public int IsDelete
        {
            get;
            set;
        }

        public DateTime PreCourseTime
        {
            get;
            set;
        }

        /// <summary>
        /// 课程开设次数
        /// </summary>
        public int CourseTimes { get; set; }

        /// <summary>
        /// 培训对象
        /// </summary>
        public string CourseObjectMemo { get; set; }

        /// <summary>
        /// 授课地点
        /// </summary>
        public string CourseAddress { get; set; }

        /// <summary>
        /// 培训天数
        /// </summary>
        public int TrainDays { get; set; }

        /// <summary>
        /// IsSystem
        /// </summary>
        public int IsSystem { get; set; }
        /// <summary>
        /// IsYearPlan
        /// </summary>
        public int IsYearPlan { get; set; }

        /// <summary>
        /// 课程提供/组织方
        /// </summary>
        public string courseProvide { get; set; }

        /// <summary>
        /// 学习要求
        /// </summary>
        public string studyRequirement { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        #endregion

        /// <summary>
        /// 所属年度计划
        /// </summary>
        public int YearPlan { get; set; }

        /// <summary>
        /// 指定的特殊人群
        /// </summary>
        public string OpenPerson { get; set; }

        /// <summary>
        /// CPA 讲师
        /// </summary>
        public string CpaTeacher { get; set; }

        /// <summary>
        /// 同一课程编号下的课次 顺序
        /// </summary>
        public Int64 CourseTimesOrder { get; set; }

        /// <summary>
        /// 考勤方式（0：不考勤；1：仅上课考勤；2：仅下课考勤；3：上、下课都考勤）
        /// </summary>
        public int AttFlag { set; get; }

        /// <summary>
        /// 考勤状态（0：未计算获得学时；1：已计算获得学时）
        /// </summary>
        public int AttStatus { set; get; }

        /// <summary>
        /// 是否投放到首页（0：否；1：是）
        /// </summary>
        public int AdFlag { set; get; }

        /// <summary>
        /// 同一课程编号下 已经开设的课程总数
        /// </summary>
        public int TotalCourseTimes { get; set; }

        #region 学员报名

        public string CourseNameStr
        {
            get
            {
                return CourseName.HtmlXssEncode();
            }
        }

        /// <summary>
        /// 已报名人数
        /// </summary>
        public int HasEntered
        {
            get;
            set;
        }

        /// <summary>
        /// 成功报名人数
        /// </summary>
        public int SuccessEntered
        {
            get;
            set;
        }

        /// <summary>
        /// 部门已报名人数
        /// </summary>
        public int DeptHasEntered { get; set; }


        /// <summary>
        /// 部门指定人数
        /// </summary>
        public int AssignUserCount { set; get; }


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
                            if (_approvalflag > 0)
                            {
                                if (_approvaldatetime > _approvallimittime)
                                    return 5;
                                return 5 + _approvalflag;
                            }
                            return 5;
                        }
                    }
                    if (_mystatus == 0 && _starttime < DateTime.Now)
                        return 8;
                    return _mystatus;
                }
                if (_starttime >= DateTime.Now && StopOrderFlag == 0)
                    return 4;
                if (_starttime >= DateTime.Now && StopOrderFlag == 1)
                    return 9;
                return 8;
            }
        }

        public string MyStatusStr
        {
            get
            {
                var str = "预订成功";
                if (_isappoint == 1)
                    str = "部门指定";
                else if (_isappoint == 2)
                    str = "总所指定";
                switch (MyStatus)
                {
                    case 0:
                        return "退订成功";
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

        private int? _approvalflag;
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

        private DateTime? _approvaldatetime;
        public DateTime? ApprovalDateTime
        {
            set
            {
                _approvaldatetime = value;
            }
            get
            {
                return _approvaldatetime;
            }
        }
        private DateTime? _approvallimittime;
        public DateTime? ApprovalLimitTime
        {
            set
            {
                _approvallimittime = value;
            }
            get
            {
                return _approvallimittime;
            }
        }

        private int? _mylocation;
        /// <summary>
        /// 我的位置
        /// </summary>
        public int? MyLocation
        {
            set
            {
                _mylocation = value;
            }
            get
            {
                if (UserId.HasValue)
                    return _mylocation;
                return 0;
            }
        }

        private int? _myorderendlocation;
        /// <summary>
        /// 在排队替补结束时间前我的位置
        /// </summary>
        public int? MyOrderEndLocation
        {
            set
            {
                _myorderendlocation = value;
            }
            get
            {
                if (UserId.HasValue)
                    return _myorderendlocation;
                return 0;
            }
        }

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
                if (_starttime <= DateTime.Now)
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

        private DateTime _orderendtime;
        /// <summary>
        /// 排队结束时间
        /// </summary>
        public DateTime OrderEndTime
        {
            set { _orderendtime = value; }
            get { return _orderendtime; }
        }

        private int _isappoint;
        /// <summary>
        /// 是否指定
        /// </summary>
        public int IsAppoint
        {
            set { _isappoint = value; }
            get { return _isappoint; }
        }

        #endregion

        /// <summary>
        /// 是否可以预订
        /// </summary>
        public string OrderStr
        {
            get
            {
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
        /// <summary>
        /// 是否开放部门分所
        /// </summary>
        public int IsOpenSub { get; set; }

        public int CourseId { get; set; }

        public int midNum { get; set; }

        #region 补预定

        public string JobNum { get; set; }
        public string LeaderID { get; set; }

        public DateTime attStartTime { get; set; }

        public DateTime attEndTime { get; set; }
        #endregion

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
        /// 授课时间
        /// </summary>
        public string TeacherCourseTime
        {
            get
            {
                return StartTime.ToString("yyyy-MM-dd HH:mm") + " -- " + EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string TeacherName
        {
            get;
            set;
        }


        private string _roomname;
        public string RoomName
        {
            get { return _roomname; }
            set { _roomname = value; }
        }

        public int IsAttFlag { get; set; }

        public int IsPass { get; set; }

        public int number { get; set; }

        public int ResourceType { get; set; }

        /// <summary>
        /// 0：课程资源；1：课程附件；3：scorm;4;视频 
        /// </summary>
        public string ResourceTypeName
        {
            get
            {
                switch (ResourceType)
                {
                    case 0:
                        return "课程资源";
                    case 1:
                        return "课程附件";
                    case 3:
                        return "scorm";
                    case 4:
                        return "视频";
                }
                return "";
            }
        }

        public int Status
        {
            get
            {
                if (DateTime.Now < StartTime)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

        public decimal GetScore { get; set; }

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

        /// <summary>
        /// 课后评估，问题与选项
        /// </summary>
        public Survey_Exampaper AfterCourseAssess { get; set; }

        /// <summary>
        /// 课后评估【对讲师的评估】，问题与选项
        /// </summary>
        public Survey_Exampaper AfterCourseTeacherExam { get; set; }

        /// <summary>
        /// 课程资源
        /// </summary>
        public List<Dept_CourseResource> CourseResourselist { get; set; }

        /// <summary>
        /// 在线测试
        /// </summary>
        public Dept_CoursePaper CoursePaper { get; set; }


        public int totalcount
        {
            get;
            set;
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
        public int PassStatus { get; set; }

        /// <summary>
        /// 讲师是否被删除
        /// </summary>
        public int TeacherIsDelete { get; set; }

        /// <summary>
        /// 该用户是否还具有 讲师身份 0 不具有  1 内部 2 外聘
        /// </summary>
        public int TeacherIsTeacher { get; set; }

        /// <summary>
        /// CPA成绩是否导入完毕 0 已经完成, 大于0 未完成(还存在未导入成绩的学员)
        /// </summary>
        public int CPAUNImportCount { get; set; }

        public decimal GetLength { get; set; }


        public int OrderStatus { get; set; }

        public int MakeUpApprovalFlag { get; set; }

        public string TeacherMobileNum { get; set; }
        public string TeacherEmail { get; set; }

        //课程独立的配置学时 考勤;考试;评估  用分号隔开
        public string CourseLengthDistribute { get; set; }

        public string courseattend { get; set; }

        public string courseonlinetest { get; set; }

        public string courseafter { get; set; }

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
        /// 允许考试次数
        /// </summary>
        public int TestTimes
        {
            get;
            set;
        }
        /// <summary>
        /// 是否有评估and测试
        /// </summary>
        public string IsPingTest
        {
            get
            {
                if (IsTest == 1 && IsPing==0)
                    return "在线测试";
                else if (IsTest == 0 && IsPing == 1)
                    return "课后评估";
                else if (IsTest == 1 && IsPing == 1)
                    return "（测试，评估）兼有";
                else
                    return "无";

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
        /// 课程大纲
        /// </summary>
        public string MemoStr
        {
            get
            {
                if (string.IsNullOrEmpty(Memo))
                {
                    return "暂无内容";
                }
                else
                {
                    return Memo;
                }
            }
        }

        /// <summary>
        /// 区分分所是否可以学
        /// </summary>
        public int IsOpenSubInt { get; set; }

        public int CpaLearnStatus { get; set; }

        public int inopenlevel { get; set; }

        //用户级别
        public string traingrade { get; set; }

        public int noopenlevelandfensuo { get; set; }

        public int IsMain { get; set; }

        public string courseProvideStr
        {
            get
            {
                return string.IsNullOrEmpty(this.courseProvide) ? "" : this.courseProvide;
            }
        }

        public string IsOrNoOrLine
        {
            get;
            set;
        }


        #region 部门分所

        //是否开放
        public int openflag { get; set; }
        //审批状态
        public int attendStatus { get; set; }

        #endregion

        /// <summary>
        /// 是否开放预订
        /// </summary>
        public int IsOpenOrder
        {
            get;
            set;
        }
        /// <summary>
        /// 是否开放预订显示
        /// </summary>
        public string IsOpenOrderStr
        {
            get
            {
                return IsOpenOrder > 0 ? "是" : "否";
            }
        }

        public string ClassroomAddress { get; set; }

        public int DepartSetId { get; set; }

    }





    /// <summary>
    /// 课后评估，问题与选项
    /// </summary>
    [Serializable]
    public class Survey_QuestionAndAnswer
    {
        public Survey_Question sq { get; set; }

        public List<Survey_QuestionAnswer> sqalist { get; set; }
    }

}
