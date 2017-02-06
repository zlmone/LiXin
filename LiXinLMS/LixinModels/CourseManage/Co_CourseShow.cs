using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;

namespace LiXinModels.CourseManage
{
    public class Co_CourseShow
    {
        #region model
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// YearPlan
        /// </summary>
        public int YearPlan { get; set; }
        /// <summary>
        /// CourseName
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
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
        /// NextStartTime
        /// </summary>
        public DateTime NextStartTime { get; set; }
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
        public int PreAdviceConfigTime { get; set; }
        /// <summary>
        /// AfterEvlutionConfigTime
        /// </summary>
        public int AfterEvlutionConfigTime { get; set; }
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
        /// OpenPerson
        /// </summary>
        public string OpenPerson { get; set; }
        /// <summary>
        /// CpaTeacher
        /// </summary>
        public string CpaTeacher { get; set; }

        public string SortStr
        {
            get
            {
                var s = "";
                if (!string.IsNullOrEmpty(Sort))
                {
                    if (Sort.Contains("1")) s += ((Enums.Sort)1).ToString() + "，";
                    if (Sort.Contains("2")) s += ((Enums.Sort)2).ToString() + "，";
                    if (Sort.Contains("3")) s += ((Enums.Sort)3).ToString() + "，";
                    if (Sort.Contains("4")) s += ((Enums.Sort)4).ToString() + "，";
                }
                if (s.Length > 0)
                    return s.TrimEnd('，');
                return s;
            }
        }

        #endregion

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

        public string TeacherName{get;set;}

        public string RoomName{get;set;}

        public int totalcount{get;set;}

        public decimal GetScore { get; set; }
        public string DeptName { get; set; }
        public decimal CoAvg { get; set; }
        public decimal TeAvg { get; set; }
        /// <summary>
        /// 课程关联试卷表主键ID
        /// </summary>
        public int CoPaperID { get; set; }
        /// <summary>
        /// 试卷ID
        /// </summary>
        public int PaperId { get; set; }

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
        /// 考试取得的总分
        /// </summary>
        public decimal GetexamScore { get; set; }
        /// <summary>
        /// 试卷总分
        /// </summary>
        public int ExamScore { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        public string WayStr
        {
            get
            {
                return ((Enums.Way)this.Way).ToString();
            }
        }

        /// <summary>
        /// 所有课程学时
        /// </summary>
        public decimal allsum { get; set; }
        /// <summary>
        /// 有效学时
        /// </summary>
        public decimal CoLength { get; set; }
        /// <summary>
        /// 所有获得学时
        /// </summary>
        public decimal passsum { get; set; }
        /// <summary>
        /// 道德学时
        /// </summary>
        public decimal daode { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        public string lengthRate
        {
            get
            {
                if (CoLength == 0)
                {
                    return "0%";
                }
                else
                {
                    return (Math.Round((CoLength * 100 / allsum), 2, MidpointRounding.AwayFromZero)).ToString() + "%";
                }
            }
        }

        public int CpaFlag { get; set; }

        /// <summary>
        /// 学时类型
        /// </summary>
        public string CpaFlagStr
        {
            get
            {
                return ((Enums.CpaFlag)this.CpaFlag).ToString();
            }
        }
        /// <summary>
        /// cpa学时
        /// </summary>
        public decimal GetLength { get; set; }

        
        
        /// <summary>
        /// 考试总次数
        /// </summary>
        public int ExamALLTestTimes { get; set; }

        /// <summary>
        /// 考试次数
        /// </summary>
        public int ExamTestTimes { get; set; }

        /// <summary>
        /// 通过线分数
        /// </summary>
        public int LevelScore { get; set; }

        public int LevelScoreStr { get; set; }

        /// <summary>
        /// 试卷总分
        /// </summary>
        public int ExampaperScore { get; set; }

        public int Length { get; set; }

        public int CourseId { get; set; }

        public DateTime CourseStartTime { get; set; }

        public DateTime CourseEndTime { get; set; }

        public string Realname { get; set; }

        public int PassStatus { get; set; }

        /// <summary>
        /// 视频通过状态
        /// </summary>
        public int IsPass { get; set; }

        public string CourseNameStr
        {
            get
            {
                return CourseName.HtmlXssEncode();
            }
        }

        #region 我的课前建议字段

        public string AdviceContent { get; set; }

        public DateTime AdviceTime { get; set; }

        public string AdviceTimeStr {
            get
            {
               return AdviceTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public int AdviceCount { get; set; }

        public int ALLnum { get; set; }

        /// <summary>
        /// 一次/多次考勤
        /// </summary>
        public int MidAttendceCount { get; set; }


        #endregion

        #region 我的课后评估字段

        public string SubjectiveAnswer { get; set; }

        public string ObjectiveAnswer { get; set; }

        #endregion

        //2个考勤时间
        public DateTime AttendceStartTime { get; set; }
        public DateTime AttendceEndTime { get; set; }

        public string AttendceTimeStr
        {
            get
            {
                string t = "";
                if (AttendceStartTime != DateTime.MinValue)
                {
                    t += AttendceStartTime.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    t += " --";
                }
                if (AttendceEndTime != DateTime.MinValue)
                {
                    t += " -- " + AttendceEndTime.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    t += " -- --";
                }
                return t;
                //return AttendceStartTime.ToString("yyyy-MM-dd HH:mm") + " -- " + AttendceEndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string StartTimeM
        {
            get
            {
                if (AttendceStartTime.Year == 1)
                {
                    return "";
                }
                else if (AttendceStartTime.Year == 2050)
                {
                    return "——";
                }
                else
                {
                    return AttendceStartTime.ToString("yyyy-MM-dd HH:mm");
                }
            }
        }

        public string EndTimeM
        {
            get
            {
                if (AttendceEndTime.Year == 1)
                {
                    return "";
                }
                else if (AttendceEndTime.Year == 2000)
                {
                    return "——";
                }
                else
                {
                    return AttendceEndTime.ToString("yyyy-MM-dd HH:mm");
                }
            }
        }
        
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
                        return "早退" + (AttendceEndTime.Subtract(EndTime).TotalMinutes*-1).ToString("0.00") + "(min)";
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
                                   (AttendceEndTime.Subtract(EndTime).TotalMinutes*-1).ToString("0.00") + "(min)</span>";
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
                                   (AttendceEndTime.Subtract(EndTime).TotalMinutes*-1).ToString("0.00") + "(min)</span>";
                            //<span class='f11'>(" + AttendceStartTime.Subtract(StartTime).TotalMinutes +"min)</span>
                        }
                        else if (AttendceStartTime <= StartTime && AttendceEndTime < EndTime)
                        {
                            return "早退" + (AttendceEndTime.Subtract(EndTime).TotalMinutes*-1).ToString("0.00") + "(min)";
                        }
                    }
                }               

                return "";
            }
        }

        public int UserID { get; set; }

        public string TeacherNamestr
        {
            get
            {
                return string.IsNullOrEmpty(this.TeacherName) ? "--" : this.TeacherName;
            }
        }

        public int AttFlag { get; set; }

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
    }
}
