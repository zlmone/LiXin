using System;
namespace LiXinModels.PlanManage
{
	/// <summary>
	/// Tr_YearPlan:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Tr_YearPlan
	{
		#region Model
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// LastUpdateTime
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// Year
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// PublishFlag
        /// </summary>
        public int PublishFlag { get; set; }
        /// <summary>
        /// IsDelete
        /// </summary>
        public int IsDelete { get; set; }
		#endregion Model


        /// <summary>
        /// 包含课程数
        /// </summary>
        public int courseCount
        {
            get;
            set;
        }

        public int totalcount
        {
            get;
            set;
        }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string Realname { get; set; }

        public string CreateTimeStr
        {
            get { return LastUpdateTime.ToString("yyyy-MM-dd HH:mm"); }
        }
	}
}

