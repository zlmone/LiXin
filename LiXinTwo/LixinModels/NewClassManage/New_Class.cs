using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace LixinModels.NewClassManage
{
	 	//New_Class
		public class New_Class
	{
      	/// <summary>
        /// Id
        /// </summary>
		public int Id { get; set; }
		/// <summary>
        /// ClassName
        /// </summary>
		public string ClassName { get; set; }
		/// <summary>
        /// PersonCount
        /// </summary>
		public int PersonCount { get; set; }
		/// <summary>
        /// Year
        /// </summary>
		public int Year { get; set; }

        /// <summary>
        /// 班号 大写英文字母
        /// </summary>
        public string ClassNo { get; set; }
        /// <summary>
        /// 班号索引  英文字母所对应的序号如：A-0 , B-1... 
        /// </summary>
        public int ClassIndex { get; set; }
        /// <summary>
        /// 是否可删除:0：不可以1：可以
        /// </summary>
        public int IsDoDelete { get; set; }


        /// <summary>
        /// 实际班级人数
        /// </summary>
        public int NowPeopleCount { get; set; }
        /// <summary>
        /// 实际班级组数
        /// </summary>
        public int NowGroupCount { get; set; }

            /// <summary>
        /// 记录数
        /// </summary>
        public int totalCount { get; set; }

        /// <summary>
        /// 实际班级男生人数
        /// </summary>
        public int NowManCount { get; set; }
        /// <summary>
        /// 实际班级女生人数
        /// </summary>
        public int NowWomanCount { get; set; }
        /// <summary>
        /// 实际班级有实习经验人数
        /// </summary>
        public int NowInternExpCount { get; set; }
        /// <summary>
        /// 实际班级无实习经验人数
        /// </summary>
        public int NowNoInternExpCount 
        { 
            get { return NowPeopleCount - NowInternExpCount; }
        }

            

		   
	}
}