using System;
namespace LiXinModels.PlanManage
{
	/// <summary>
	/// Tr_MonthPlanCourse:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Tr_MonthPlanCourse
	{
		public Tr_MonthPlanCourse()
		{}
		#region Model
		private int? _monthid;
		private int? _courseid;
		private int? _issystem;
		/// <summary>
		/// 
		/// </summary>
		public int? MonthId
		{
			set{ _monthid=value;}
			get{return _monthid;}
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
		public int? IsSystem
		{
			set{ _issystem=value;}
			get{return _issystem;}
		}
		#endregion Model

	}
}

