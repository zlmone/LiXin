using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    public class Free_OtherApplyConfig
    {
        #region Model

        /// <summary>
        /// ID
        /// </summary>		
        public int ID { get; set; }

        /// <summary>
        /// ApplyContent
        /// </summary>		
        public string ApplyContent { get; set; }

        /// <summary>
        /// 可折算学时 0 CPA学时 1 所内学时 2所有
        /// </summary>		
        public int ConvertType { get; set; }

        /// <summary>
        /// ConvertBase
        /// </summary>		
        public decimal ConvertBase { get; set; }

        /// <summary>
        /// ConvertBaseInfo
        /// </summary>		
        public string ConvertBaseInfo { get; set; }

        /// <summary>
        /// ConvertBaseTo
        /// </summary>		
        public decimal ConvertBaseTo { get; set; }

        /// <summary>
        /// UploadTip
        /// </summary>		
        public string UploadTip { get; set; }

        /// <summary>
        /// 可折CPA学时年度的最高限制
        /// </summary>		
        public decimal ConvertMax { get; set; }

        /// <summary>
        /// 可折内部培训学时年度的最高限制 ApplyType=1 形如 A-1,B-2  ApplyType=0 直接一个数值
        /// </summary>		
        public string TrainGradeScore { get; set; }

        /// <summary>
        /// Memo
        /// </summary>		
        public string Memo { get; set; }

        /// <summary>
        /// MemoTip
        /// </summary>		
        public string MemoTip { get; set; }


        /// <summary>
        /// year
        /// </summary>		
        public int year { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public int IsDelete { get; set; }

        /// <summary>
        /// 0授课人 1 其他项目 2课后评估
        /// </summary>
        public int ApplyType { get; set; }
        #endregion

        public int totalCount { get; set; }

        public string JiBie { get; set; }

        public Dictionary<string, decimal> TrainGradeScoreList
        {
            get
            {
                if (string.IsNullOrEmpty(this.TrainGradeScore) || this.ApplyType == 0||this.ApplyType==2)
                {
                    return new Dictionary<string, decimal>();
                }
                else
                {
                    var valueList = this.TrainGradeScore.Split(',');
                    var dicList = new Dictionary<string, decimal>();
                    foreach (var item in valueList)
                    {
                        dicList[item.Split('-')[0]] = Convert.ToDecimal(item.Split('-')[1]);
                    }
                    return dicList;
                }

            }
        }

        public string TrainGradeScoreStr
        {
            get
            {
                if (this.ApplyType == 1)
                {
                    if (ConvertType == 1 || ConvertType == 2)
                        return string.Join("/", TrainGradeScoreList.Values.ToList());
                    else
                        return "N/A";
                }
                else
                {
                    // return "按实际授课、主持或演讲时间的" + this.ConvertBaseTo + "倍折算学时";
                    return TrainGradeScore;
                }
            }
        }

        public string ConvertMaxStr
        {
            get
            {
                if (ConvertType == 0 || ConvertType == 2)
                    return ConvertMax.ToString();
                else
                    return "N/A";
            }
        }

        public string UploadTipStr
        {
            get
            {
                return string.IsNullOrEmpty(this.UploadTip) ? "" : this.UploadTip.Trim();
            }
        }

    }
}
