using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
     [Serializable]
    public class Free_OtherFroms
    {
        public int Id { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 申请项目名称
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// 适用的申请 0:CPA,1:所内,2：2者皆有
        /// </summary>
        public int FromType { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime FromTime { get; set; }

        /// <summary>
        /// 是否删除 0:没有删除,1:已删除
        /// </summary>
        public int IsDelete { get; set; }

        public string FromTypeStr{
            get {
                if (FromType == 0)
                {
                    return "CPA";
                }
                else if (FromType == 1)
                {
                    return "所内";
                }
                else
                {
                    return "CPA/所内";
                }
            }
      }
    }
}
