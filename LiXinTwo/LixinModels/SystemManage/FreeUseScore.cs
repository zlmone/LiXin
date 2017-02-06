using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    public class FreeUseScore
    {
        /// <summary>
        /// 申请名称ID
        /// </summary>
        public int ApplyID { get; set; }

        /// <summary>
        /// 申请内容
        /// </summary>
        public string ApplyContent { get; set; }

        /// <summary>
        /// 申请类型
        /// </summary>
        public int ApplyType { get; set; }

        public int ApplyUserID { get; set; }

        public int Year { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }

        /// <summary>
        /// 实际获得的所内学时
        /// </summary>
        public decimal GettScore { get; set; }

        /// <summary>
        /// 实际获得的CPA学时
        /// </summary>
        public decimal GetCPAScore { get; set; }

        public int totalCount { get; set; }

        public int typeForm { get; set; }

        public string ApplyTimeStr
        {
            get
            {
                return this.typeForm == 4 ? "--" : this.ApplyTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        /// <summary>
        /// 申请类型
        /// </summary>
        public string typeFormStr
        {
            get
            {
                var result = "";
                switch (this.typeForm)
                {
                    case 0://自己新增
                        result = "自主申请";
                        break;
                    case 1://导入
                        result = "管理员导入";
                        break;
                    case 2://评估自动折算
                        result = "评估自动折算";
                        break;
                    case 3://讲师授课
                        result = "授课自动折算";
                        break;
                    case 4://免修自动折算
                        result = "免修自动折算";
                        break;

                }
                return result;
            }
        }


        public DateTime StartTime { get; set; }

        public DateTime  EndTime { get; set; }

    }
}
