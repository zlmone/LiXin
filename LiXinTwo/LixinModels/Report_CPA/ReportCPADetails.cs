using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_CPA
{
    public class ReportCPADetails
    {
        public ReportCPADetails()
        {
            CPADetailsList = new List<CPADetails>();
        }
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Realname { get; set; }

        public string DeptName { get; set; }

        /// <summary>
        /// CPA编号
        /// </summary>
        public string CPANo { get; set; }


        /// <summary>
        /// 学时明细
        /// </summary>
        public List<CPADetails> CPADetailsList { get; set; }

        /// <summary>
        /// 合计学时
        /// </summary>
        public decimal SumScore { get; set; }

        /// <summary>
        /// 需要合并的行数
        /// </summary>
        public int rowspan { get; set; }


    }


    /// <summary>
    /// 学时明细
    /// </summary>
    public class CPADetails
    {
        /// <summary>
        /// 课程类型  0 所内  2 注协课程 3 其他形式 4 免修 5 其他有组织形式
        /// </summary>
        public int courseType { get; set; }

        /// <summary>
        /// 培训形式 1 集中授课  2 视频课程  3部门自学  -1 N/A  
        /// </summary>
        public int Way { get; set; }

        /// <summary>
        /// 0自己新增 1 导入 2 评估自动折算 3 授课自动折算 4 免修自动折算
        /// </summary>
        public int typeForm { get; set; }

        public string CourseName { get; set; }


        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        /// <summary>
        /// 合计学时
        /// </summary>
        public decimal singleScore { get; set; }

        /// <summary>
        /// 课程类型  0 所内  2 注协课程
        /// </summary>
        public string courseTypeStr
        {
            get
            {
                return ((Enums.courseType)this.courseType).ToString();
            }
        }

        /// <summary>
        /// 培训形式 1 集中授课  2 视频课程  3部门自学  -1 N/A
        /// </summary>
        public string WayStr
        {
            get
            {
                var result = "";
                switch (Way)
                {
                    case 1:
                        result = "集中授课";
                        break;
                    case 2:
                        result = "视频课程";
                        break;
                    case 3:
                        result = "部门自学";
                        break;
                    case -1:
                        result = "N/A";
                        break;
                }

                return result;
            }
        }


        public string StartTimeStr
        {
            get
            {
                return this.StartTime == DateTime.MinValue ? "" : this.StartTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string EndTimeStr
        {
            get
            {
                return this.EndTime == DateTime.MinValue ? "" : this.EndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

    }

}
