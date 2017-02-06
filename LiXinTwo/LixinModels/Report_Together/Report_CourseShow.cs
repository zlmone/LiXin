using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;

namespace LiXinModels.Report_Together
{
    public class Report_CourseShow
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
		/// 课程学时
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
		public decimal PreAdviceConfigTime { get; set; }
		/// <summary>
		/// AfterEvlutionConfigTime
		/// </summary>
        public decimal AfterEvlutionConfigTime { get; set; }
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

		#endregion

#region == extend ==
        /// <summary>
        /// 是否有在线测试
        /// </summary>
        public string IsTestStr
        {
            get
            {
                return IsTest == 0 ? "N/A" : "是";
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
        /// 讲师名称
        /// </summary>
        public string TeacherName { get; set; }
#endregion 

        
        

		public string RoomName { get; set; }

		public int totalcount { get; set; }

		public string DeptName { get; set; }
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
		/// 课程类型
		/// </summary>
		public string WayStr
		{
			get
			{
				return ((Enums.Way)this.Way).ToString();
			}
		}

		public int CpaFlag { get; set; }

		public int CPAForm { get; set; }

		public int Length { get; set; }

		public int CourseId { get; set; }

		public DateTime CourseStartTime { get; set; }

		public DateTime CourseEndTime { get; set; }

		public string Realname { get; set; }

		public int PassStatus { get; set; }

		
		public int UserID { get; set; }

		public int DepCourseId { get; set; }

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

        public int openNumber { get; set; }
	}
}
