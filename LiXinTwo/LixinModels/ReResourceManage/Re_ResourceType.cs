using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace LixinModels.ReResourceManage
{
	 	//Re_ResourceType
		public class Re_ResourceType
	{
      	/// <summary>
        /// ResourceTypeID
        /// </summary>
		public int ResourceTypeID { get; set; }
		/// <summary>
        /// TypeName
        /// </summary>
		public string TypeName { get; set; }
		/// <summary>
        /// TypeDec
        /// </summary>
		public string TypeDec { get; set; }
		/// <summary>
        /// ParentID
        /// </summary>
		public int ParentID { get; set; }
		/// <summary>
        /// IsDelete
        /// </summary>
		public int IsDelete { get; set; }
		   
	}
}