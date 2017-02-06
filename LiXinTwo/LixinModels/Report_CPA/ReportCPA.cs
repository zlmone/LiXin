using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_CPA
{
    public class ReportCPA
    {
        #region model
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Realname { get; set; }

        public string DeptName { get; set; }

        /// <summary>
        /// CPA编号
        /// </summary>
        public string CPANo { get; set; }

        public string CPA { get; set; }


        public DateTime JoinDate { get; set; }

        public DateTime CPACreateDate { get; set; }
        /// <summary>
        /// 所内折算CPA学时
        /// </summary>
        public decimal sumZheScore
        {
            get
            {
                return tScore + dScore;
            }
        }

        /// <summary>
        /// 注协课程CPA学时
        /// </summary>
        public decimal sumCPAScore { get; set; }

        /// <summary>
        /// 是否参加职业道德培训
        /// </summary>
        public int isJoinDaode { get; set; }

        private decimal _SumScore;
        /// <summary>
        /// CPA合计学时
        /// </summary>
        public decimal SumScore
        {
            get
            {
                return CPA == "否" ? 0 : tScore + dScore + sumCPAScore + OtherScore + OtherProjectScore + FreeScore;
            }
            set
            {
                _SumScore = value;
            }

        }

        /// <summary>
        /// 其他培训项目
        /// </summary>
        public string OtherProject { get; set; }

        /// <summary>
        /// 其他培训学时
        /// </summary>
        public decimal OtherScore { get; set; }

        /// <summary>
        /// 可免培训项目
        /// </summary>
        public string FreeProject { get; set; }

        /// <summary>
        /// 可免培训学时
        /// </summary>
        public decimal FreeScore { get; set; }

        /// <summary>
        /// 其他有组织形式学时
        /// </summary>
        public decimal OtherProjectScore { get; set; }

        public int YearPlan { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        /// <summary>
        /// 集中授课折算成的cpa
        /// </summary>
        public decimal tScore { get; set; }

        /// <summary>
        /// 部门自学折算成的cpa
        /// </summary>
        public decimal dScore { get; set; }

        /// <summary>
        /// 导入考勤获得的cpa学时
        /// </summary>
        public decimal cpaScore { get; set; }

        /// <summary>
        /// 考试通过次数
        /// </summary>
        public int passNumber { get; set; }

        /// <summary>
        /// 培训周期学时合计
        /// </summary>
        public decimal SumZhouqiScore { get; set; }

        // DeptId, DeptPath, deptName
        public int DeptId { get; set; }

        /// <summary>
        /// 完全路径
        /// </summary>
        public string DeptPath { get; set; }

        /// <summary>
        /// 仅显示到部门
        /// </summary>
        public string deptName { get; set; }


        /// <summary>
        /// 周期内的人数
        /// </summary>
        public int zhouqiNumber { get; set; }

        /// <summary>
        /// 年度目标学时是否完成
        /// </summary>
        public int IsyearComplete { get; set; }

        /// <summary>
        /// 周期学时目标是否完成
        /// </summary>
        public int IsZhouqiComplete { get; set; }

        //合计
        public decimal sumAllScore { get; set; }

        //public DateTime CPACreateDate { get; set; }

        //public DateTime  JoinDate { get; set; }

        //自动折算
        public int FreeOut { get; set; }

        public int IsFree { get; set; }
        #endregion

        #region 扩展

        public string isJoinDaodeStr
        {
            get
            {
                return this.isJoinDaode > 0 ? "是" : "否";
            }
        }

        //courseType, Way, CourseName, CourseLength

        /// <summary>
        /// 课程类型  0 所内  1 注协课程
        /// </summary>
        public int courseType { get; set; }

        /// <summary>
        /// 培训形式 1 集中授课  2 视频课程  3部门自学  -1 N/A
        /// </summary>
        public int Way { get; set; }

        public string CourseName { get; set; }

        /// <summary>
        /// 课程的学时
        /// </summary>
        public decimal CourseLength { get; set; }

        //其他项目
        public string OtherProjectStr
        {
            get
            {
                return string.IsNullOrEmpty(this.OtherProject) ? "--" : this.OtherProject;
            }
        }

        //免修
        public string FreeProjectStr
        {
            get
            {
                return string.IsNullOrEmpty(this.FreeProject) ? "--" : this.FreeProject;
            }
        }

        //其他项目
        public string OtherScoreStr
        {
            get
            {
                return string.IsNullOrEmpty(this.OtherProject) ? "--" : this.OtherScore.ToString();
            }
        }

        //免修
        public string FreeScoreStr
        {
            get
            {
                return string.IsNullOrEmpty(this.FreeProject) ? "--" : this.FreeScore.ToString();
            }
        }
        #endregion
    }


}
