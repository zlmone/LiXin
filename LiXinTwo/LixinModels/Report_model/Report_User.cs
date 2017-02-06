using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class Report_User
    {
        #region 学时扩展

        /// <summary>
        ///集中课程学时
        /// </summary>
        public decimal CoScore { get; set; }
        /// <summary>
        ///视频课程学时
        /// </summary>
        public decimal voScore { get; set; }
        /// <summary>
        ///CPA折算目标学时
        /// </summary>
        public decimal CPAToScore { get; set; }
        /// <summary>
        ///部门/分所学时
        /// </summary>
        public decimal DepScore { get; set; }

        /// <summary>
        ///所有课程实际学时
        /// </summary>
        public decimal SumScore
        {
            get
            {
                return CoScore + voScore + CPAToScore + DepScore;
            }
        }
        /// <summary>
        ///所有课程有效学时
        /// </summary>
        public decimal EffectScore { get; set; }
        /// <summary>
        ///CPA实际学时
        /// </summary>
        public decimal CPAScore { get; set; }
        /// <summary>
        ///CPA有效学时
        /// </summary>
        public decimal EffectCPAScore { get; set; }
        /// <summary>
        ///年度所内目标学时
        /// </summary>
        public decimal TargetScore { get; set; }

        /// <summary>
        ///年度CPA目标学时
        /// </summary>
        public decimal TargetCPAScore { get; set; }

        /// <summary>
        /// 测试通过次数
        /// </summary>
        public int passNumber { get; set; }


        #endregion

        /// <summary>
        /// 人员下面所有课程
        /// </summary>
        public List<Report_Course_show> CoList { get; set; }

        //su.UserId,su.CPA,su.SubordinateSubstation,su.DeptId,su.TrainGrade,su.PayGrade,su.CPAStatus
        public int UserId { get; set; }

        public string CPA { get; set; }

        public string SubordinateSubstation { get; set; }

        public string TrainGrade { get; set; }

        public int DeptId { get; set; }

        public string PayGrade { get; set; }

        public string CPAStatus { get; set; }

        public int typeForm { get; set; }

        public DateTime JoinDate { get; set; }

        public DateTime CPACreateDate { get; set; }
    }
}
