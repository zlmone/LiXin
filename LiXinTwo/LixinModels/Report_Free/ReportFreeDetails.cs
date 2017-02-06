using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class ReportFreeDetails
    {
        public ReportFreeDetails()
        {
            FreeDetailsList = new List<FreeDetailsList>();
        }
        public int totalcount { get; set; }

        public int ID { get; set; }

        public int UserId { get; set; }

        public string  Username { get; set; }

        public string Realname { get; set; }

        public int DeptId { get; set; }

        public string DeptName { get; set; }

        public string TrainGrade { get; set; }

        public string CPANo { get; set; }

        public string CPA { get; set; }

        public string ClassName { get; set; }

        public decimal  ConvertScore { get; set; }

        public decimal GettScore { get; set; }

        public decimal GetCPAScore { get; set; }
        

        /// <summary>
        /// 0自己新增 1 导入 2 评估自动折算 3 授课自动折算 4 免修自动折算
        /// </summary>
        public int typeForm { get; set; }

        public List<FreeDetailsList> FreeDetailsList { get; set; }

        public int rowspan
        {
            get
            {
                return FreeDetailsList.Count();
            }
        }

        /// <summary>
        /// 合计学时 所内
        /// </summary>
        public decimal totaltScore
        {
            get
            {
                return FreeDetailsList.Sum(p => p.GettScore);
            }
        }

        /// <summary>
        /// 合计学时 CPA
        /// </summary>
        public decimal totalCPAScore
        {
            get
            {
                return FreeDetailsList.Sum(p => p.GetCPAScore);
            }
        }

        public string ClassNameStr
        {
            get
            {
                return typeForm == 2 ? "课后评估奖励学时" : ClassName;
            }
        }

    }

    public class FreeDetailsList
    {
        public string ClassName { get; set; }

        public decimal ConvertScore { get; set; }

        public decimal GettScore { get; set; }

        public decimal GetCPAScore { get; set; }

        /// <summary>
        /// 0自己新增 1 导入 2 评估自动折算 3 授课自动折算 4 免修自动折算
        /// </summary>
        public int typeForm { get; set; }
    }
}
