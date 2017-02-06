using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    public class Free_ApplyConfig
    {
        #region Model
        /// <summary>
        /// ID
        /// </summary>		
        public int ID { get; set; }

        /// <summary>
        /// FreeName
        /// </summary>		
        public string FreeName { get; set; }

        /// <summary>
        /// UploadTip
        /// </summary>		
        public string UploadTip { get; set; }

        /// <summary>
        /// 适用的申请 0 所内免修 1 CPA免修 2所有
        /// </summary>		
        public int ApplyType { get; set; }

        /// <summary>
        /// CPA免修学时
        /// </summary>		
        public decimal CPAFreeScore { get; set; }

        /// <summary>
        /// 所内免修学时
        /// </summary>		
        public decimal TogetherFreeScore { get; set; }

        /// <summary>
        /// Memo
        /// </summary>		
        public string Memo { get; set; }

        public string MemoTip { get; set; }

        public int year { get; set; }

        public DateTime CreateTime { get; set; }

        public int IsDelete { get; set; }
        #endregion

        /// <summary>
        /// 是否适用于申请所内免修
        /// </summary>
        public string TogetherFree
        {
            get
            {
                return (this.ApplyType == 0 || this.ApplyType == 2)? "是" : "否";
            }
        }

        /// <summary>
        /// 是否适用于申请CPA免修
        /// </summary>
        public string CPAFree
        {
            get
            {
                return (this.ApplyType == 1 || this.ApplyType == 2) ? "是" : "否";
            }
        }

        public int totalCount { get; set; }

        

    
    }
}
