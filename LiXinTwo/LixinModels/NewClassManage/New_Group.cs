using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace LixinModels.NewClassManage
{
	 	//New_Group
		public class New_Group
	{
      	/// <summary>
        /// Id
        /// </summary>
		public int Id { get; set; }
		/// <summary>
        /// ClassId
        /// </summary>
		public int ClassId { get; set; }
		/// <summary>
        /// GroupName
        /// </summary>
		public string GroupName { get; set; }
		/// <summary>
        /// PersonCount
        /// </summary>
		public int PersonCount { get; set; }

        /// <summary>
        /// 组号 小写英文字母
        /// </summary>
        public string GroupNo { get; set; }
        /// <summary>
        /// 组号索引 英文字母对应数字如a-0,b-1,... 
        /// </summary>
        public int GroupIndex { get; set; }
		   
	}
}