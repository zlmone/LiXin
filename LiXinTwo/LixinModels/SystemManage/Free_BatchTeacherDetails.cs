using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    public class Free_BatchTeacherDetails
    {
        /// <summary>
        /// ID
        /// </summary>		
        public int ID { get; set; }

        /// <summary>
        /// userApplyID
        /// </summary>		
        public int userApplyID { get; set; }

        /// <summary>
        /// ClassName
        /// </summary>		
        public string ClassName { get; set; }

        /// <summary>
        /// ConvertScore
        /// </summary>		
        public decimal ConvertScore { get; set; }

        /// <summary>
        /// IsDelete
        /// </summary>		
        public int IsDelete { get; set; }

        /// <summary>
        /// tScore
        /// </summary>		
        public decimal tScore { get; set; }

        /// <summary>
        /// CPAScore
        /// </summary>		
        public decimal CPAScore { get; set; } 
    }
}
