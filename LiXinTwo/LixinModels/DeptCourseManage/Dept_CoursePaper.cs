using System;
namespace LiXinModels.DeptCourseManage
{
	/// <summary>
	/// Co_CoursePaper:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Dept_CoursePaper
	{
		public Dept_CoursePaper()
		{}
		#region Model

        public int id { get; set; }
	    private int _paperid;
		private int? _courseid;
		private int? _length;
		private decimal? _hour;
		private int? _totalscore;
		private int? _levelscore;
		private int? _testtimes;
		/// <summary>
		/// 
		/// </summary>
		public int PaperId
		{
			set{ _paperid=value;}
			get{return _paperid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CourseId
		{
			set{ _courseid=value;}
			get{return _courseid;}
		}
		/// <summary>
		/// 
		/// </summary>
        public int Length { get; set; }
		/// <summary>
		/// 
		/// </summary>
        public decimal? Hour
		{
			set{ _hour=value;}
			get{return _hour;}
		}
		/// <summary>
		/// 
		/// </summary>
        public int TotalScore { get; set; }
		/// <summary>
		/// 
		/// </summary>
        public int LevelScore { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? TestTimes
		{
			set{ _testtimes=value;}
			get{return _testtimes;}
		}
		#endregion Model

        /// <summary>
        /// 0 是 1 不是   主试卷
        /// </summary>
        public int IsMain { get; set; }

        /// <summary>
        /// 试卷名称
        /// </summary>
        public string PaperName { get; set; }

	}
}

