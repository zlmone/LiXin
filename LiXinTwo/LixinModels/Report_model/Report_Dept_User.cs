using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.User;

namespace LiXinModels
{
    public class Report_Dept_User
    {
        #region 构造函数
        public Report_Dept_User()
        {
            CoUserList = new List<Sys_User>();
            freeNumberList = new List<Report_User>();
        }
        #endregion

        #region model

        public int DepartmentId { get; set; }

        public string DeptName { get; set; }

        public string DeptPath { get; set; }
        /// <summary>
        /// 所有子节点的ID
        /// </summary>
        public string childID { get; set; }
        /// <summary>
        /// 课程来源 0：部门 1：分所
        /// </summary>
        public int deptFrom { get; set; }

        /// <summary>
        /// 部门下面的人员
        /// </summary>
        public List<Sys_User> CoUserList { get; set; }

        /// <summary>
        /// 参与考核人数
        /// </summary>
        public int numberSum
        {
            get
            {
                return CoUserList.Count(p => freeNumberList.Count(k => k.UserId == p.UserId) == 0);
            }
        }
        /// <summary>
        /// 免修人数
        /// </summary>
        public int freeSum
        {
            get
            {
                return freeNumberList.Select(p => p.UserId).Distinct().Count();
            }
        }

        /// <summary>
        /// 所内培训目标完成人数
        /// </summary>
        public int goalYesSum
        {
            get
            {
                return CoUserList.Where(p => freeNumberList.Count(k => k.UserId == p.UserId) == 0 && 
                                        p.passNumber >= CompleteExamTimes && p.EffectScore >= p.TargetScore).ToList().Count;
            }
        }

        /// <summary>
        /// 所内培训目标未完成人数
        /// </summary>
        public int goalNoSum
        {
            get
            {
                return numberSum - goalYesSum;
            }
        }

        /// <summary>
        /// 目标学时完成人数
        /// </summary>
        public int PeriodYesSum
        {
            get
            {
                return CoUserList.Where(p => freeNumberList.Count(k => k.UserId == p.UserId) == 0 && 
                                        p.EffectScore >= p.TargetScore).ToList().Count;
            }
        }

        /// <summary>
        /// 目标学时未完成人数
        /// </summary>
        public int PeriodNoSum
        {
            get
            {
                return numberSum - PeriodYesSum;
            }
        }

        /// <summary>
        /// 在线测试通过人数
        /// </summary>
        public int PaperYseSum
        {
            get
            {
                return CoUserList.Where(p => freeNumberList.Count(k => k.UserId == p.UserId) == 0 && 
                                        p.passNumber >= CompleteExamTimes).ToList().Count;
            }
        }

        /// <summary>
        /// 在线测试未通过人数
        /// </summary>
        public int PaperNoSum
        {
            get
            {
                var number = numberSum - PaperYseSum;
                return number < 0 ? 0 : number;
            }
        }

        /// <summary>
        /// 所内培训目标完成率
        /// </summary>
        public double lengthRate
        {
            get
            {
                if (goalYesSum == 0)
                {
                    return 0;
                }
                else
                {
                    if (numberSum == 0)
                    {
                        return 100;
                    }
                    else
                    {
                        return Math.Round((Convert.ToDouble(goalYesSum) * 100 / Convert.ToDouble(numberSum)), 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
        }

        /// <summary>
        ///在线测试通过次数
        /// </summary>
        public int CompleteExamTimes { get; set; }

        //免修人数列表
        public List<Report_User> freeNumberList { get; set; }

        #endregion
    }

    public class Report_Course_show
    {
        #region 属性
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 课程Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// CourseName
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// YearPlan
        /// </summary>
        public int YearPlan { get; set; }
        /// <summary>
        /// 0:必修；1：选修；
        /// </summary>
        public int IsMust { get; set; }
        /// <summary>
        /// 1:自学课程；2：开放至全所；3：过时的 
        /// </summary>
        public int Way { get; set; }
        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 课程学时 
        /// </summary>
        public decimal CourseLength { get; set; }
        /// <summary>
        /// 是否可折算CPA
        /// </summary>
        public int IsCPA { get; set; }

        /// <summary>
        /// 是否在线测试  0：否；1：是 
        /// </summary>
        public int IsTest { get; set; }
        /// <summary>
        /// 是否课后评估 0：否；1：是 
        /// </summary>
        public int IsPing { get; set; }
        /// <summary>
        /// GetScore
        /// </summary>
        public decimal GetScore { get; set; }

        /// <summary>
        /// PassStatus
        /// </summary>
        public int PassStatus { get; set; }

        /// <summary>
        /// AttScore
        /// </summary>
        public decimal AttScore { get; set; }
        /// <summary>
        /// EvlutionScore
        /// </summary>
        public decimal EvlutionScore { get; set; }
        /// <summary>
        /// ExamScore
        /// </summary>
        public decimal ExamScore { get; set; }
        /// <summary>
        /// CPAGetLength
        /// </summary>
        public decimal CPAGetLength { get; set; }
        /// <summary>
        /// CPAIsPass
        /// </summary>
        public int CPAIsPass { get; set; }
        /// <summary>
        /// CpaFlag
        /// </summary>
        public int CpaFlag { get; set; }
        /// <summary>
        /// 课程来源 0：一期 1：二期
        /// </summary>
        public int courseFrom { get; set; }
        #endregion
    }
}
