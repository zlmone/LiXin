using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepCourseManage
{
    public class Dep_CourseShow
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
                else
                {
                    s = "无";
                }
                if (s.Length > 0)
                    return s.TrimEnd('，');
                return s;
            }
        }

        public int AdviceCount { get; set; }
        public string TeacherName { get; set; }
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

        public int CourseId { get; set; }

        public string Realname { get; set; }


        #endregion
    }
}
