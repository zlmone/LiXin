using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepCourseManage
{
    public class Report_DepCourse
    {
        public int attID { get; set; }

        public int Id { get; set; }
        /// <summary>
        /// 所有的考勤记录
        /// </summary>
        public int sumNum { get; set; }

        public int CourseId { get; set; }
        /// <summary>
        /// 0 总所  1分所
        /// </summary>
        public int IsMain { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int DeptId { get; set; }

        public string DeptIds { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Dep_DeptName { get; set; }

        /// <summary>
        /// 部门名称 第二级
        /// </summary>
        public string Dep_TwoDeptName { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 预定时间
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 请假时间
        /// </summary>
        public DateTime LeaveTime { get; set; }

        /// <summary>
        /// 考勤方式 0：不考勤；1：只上课考勤；2：只下课考勤；3：上、下课都考勤
        /// </summary>
        public int AttFlag { get; set; }

        /// <summary>
        /// 退订时间
        /// </summary>
        public DateTime ApprovalDateTime { get; set; }

        /// <summary>
        /// 请假审批截止时间
        /// </summary>
        public DateTime ApprovalLimitTime { get; set; }

        /// <summary>
        /// 是否完成目标学时
        /// </summary>
        public string IsCompleteStr { get; set; }

        /// <summary>
        /// 考勤数学时
        /// </summary>
        public decimal IsComplete_length { get; set; }

        /// <summary>
        /// 计划上报类型
        /// </summary>
        public string PlanStr { get; set; }
        /// <summary>
        /// 计划时间
        /// </summary>
        public string PlanTime { get; set; }

        public int PlanYear { get; set; }

        /// <summary>
        /// 计划开设课程数
        /// </summary>
        public int Dep_Course_Commencement { get; set; }

        /// <summary>
        /// 计划开设课程总学时
        /// </summary>
        public decimal Dep_Course_Commencement_length { get; set; }

        /// <summary>
        /// 实际开课总数量
        /// </summary>
        public int ActualNum { get; set; }

        /// <summary>
        /// 计划外课程数
        /// </summary>
        public int IsYearPlanWai { get; set; }


        /// <summary>
        /// 计划外课程总学时
        /// </summary>
        public decimal IsYearPlanWai_length { get; set; }

        /// <summary>
        /// 计划内课程总学时
        /// </summary>
        public decimal IsYearPlanNei_length { get; set; }

        /// <summary>
        /// 计划内外课程总学时总和  排序使用
        /// </summary>
        public decimal IsYearPlan_length
        {
            get
            {
                return this.IsYearPlanWai_length + this.IsYearPlanNei_length + this.IsLinkYearPlanNei_length + this.IsLinkYearPlanWai_length;
            }
        }

        /// <summary>
        /// 计划内课程总学时
        /// </summary>
        public int IsYearPlanNei { get; set; }

        /// <summary>
        /// 框架外课程数
        /// </summary>
        public int IsSystemNei { get; set; }

        /// <summary>
        /// 框架外课程总学时
        /// </summary>
        public decimal IsSystemNei_length { get; set; }

        /// <summary>
        /// 框架外课程数
        /// </summary>
        public int IsSystemWai { get; set; }

        /// <summary>
        /// 框架外课程总学时
        /// </summary>
        public decimal IsSystemWai_length { get; set; }

        /// <summary>
        /// 总和排序使用
        /// </summary>
        public decimal IsSystem_length
        {
            get
            {
                return IsSystemNei_length + IsSystemWai_length + this.IsLinkSystemNei_length + this.IsLinkSystemWai_length;
            }
        }

        /// <summary>
        /// 开放至全所课程数
        /// </summary>
        public int IsOpenSub { get; set; }

        /// <summary>
        /// 开放至全所课程总学时
        /// </summary>
        public decimal IsOpenSub_length { get; set; }

        /// <summary>
        /// 已上传考勤课程数
        /// </summary>
        public int UploadAttend_Num { get; set; }

        /// <summary>
        /// 审批通过课程数
        /// </summary>
        public int ApprovalPass { get; set; }

        /// <summary>
        /// 审批未通过课程数
        /// </summary>
        public int ApprovalNoPass { get; set; }

        /// <summary>
        /// 讲义上传课程数（看课程上传资源 以课程为单位 一个课程下上传10个资源也算1个）
        /// </summary>
        public int UploadRes_Num { get; set; }

        /// <summary>
        /// 人均获取学时
        /// </summary>
        public decimal Avg_GetLength { get; set; }


        /// <summary>
        /// 人均获取学时
        /// </summary>
        public string Avg_GetLengthStr
        {
            get
            {
                return this.Avg_GetLength.ToString("0.00");
            }
        }

        /// <summary>
        /// 平均出勤率
        /// </summary>
        public double Avg_Cq { get; set; }

        /// <summary>
        /// 平均迟到率
        /// </summary>
        public double Avg_Cd { get; set; }

        /// <summary>
        /// 平均缺勤率
        /// </summary>
        public double Avg_Qq { get; set; }

        public string  Username { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 是否CPA
        /// </summary>
        public string IsCPA { get; set; }

        /// <summary>
        /// 是否CPA
        /// </summary>
        public string IsCPAStr { get; set; }

        /// <summary>
        /// 参与课程数
        /// </summary>
        public int CanYu { get; set; }

        /// <summary>
        /// 已获取部门学时
        /// </summary>
        public decimal GetLength { get; set; }

        /// <summary>
        /// 请假次数
        /// </summary>
        public int Qj_Num { get; set; }

        public int Td_Num { get; set; }

        /// <summary>
        /// 缺勤次数
        /// </summary>
        public int Qq_Num { get; set; }

        public int Cd_Num { get; set; }

        public int Zt_Num { get; set; }

        public int Cd_Zt_Num { get; set; }


        /// <summary>
        /// 课前建议次数
        /// </summary>
        public int Advice_Num { get; set; }

        /// <summary>
        /// 课后评估次数
        /// </summary>
        public int AfterAnswer_Num { get; set; }

        /// <summary>
        /// 在线测试通过次数
        /// </summary>
        public int OnlineTest_Num { get; set; }

        /// <summary>
        /// 需求调研次数
        /// </summary>
        public int Research_Num { get; set; }


        public int IsDelete { get; set; }

        public int PublishFlag { get; set; }

        /// <summary>
        /// 薪酬级别
        /// </summary>
        public string PayGrade { get; set; }

        /// <summary>
        /// 培训级别
        /// </summary>
        public string TrainGrade { get; set; }

        /// <summary>
        /// 总学时
        /// </summary>
        public decimal All_CourseLength { get; set; }

        /// <summary>
        /// 每个学员所有讲师评估的中位数
        /// </summary>
        public double Dep_Survey_ReplyAnswer { get; set; }

        /// <summary>
        /// 讲师上课次数
        /// </summary>
        public int courseTeacherCount { get; set; }

        public string TeacherId { get; set; }

        /// <summary>
        /// 被联合的部门ID
        /// </summary>
        public int LinkDepart_Deptid { get; set; }

        public decimal AttScore { get; set; }

        public decimal EvlutionScore { get; set; }

        public decimal ExamScore { get; set; }

        /// <summary>
        /// 总学时 考勤+评估+考试
        /// </summary>
        public decimal AllScore
        {
            get
            {
                return AttScore + EvlutionScore + ExamScore;
            }
        }

        public int Att_Status { get; set; }

        //0：待考勤；1：正常;2:缺勤；3：迟到；4：早退；5：迟到且早退
        public string Att_StatusStr
        {
            get
            {
                string str = "";
                switch (Att_Status)
                {
                    case 1: str = "正常"; break;
                    case 2: str = "缺勤"; break;
                    case 3: str = "迟到"; break;
                    case 4: str = "早退"; break;
                    case 5: str = "迟到且早退"; break;
                }
                return str;
            }
        }

        public int OrderStatus { get; set; }

        /// <summary>
        /// 请假人数
        /// </summary>
        public int IsLeave { get; set; }

        /// <summary>
        /// 请假人数
        /// </summary>
        public string IsLeaveStr
        {
            get
            {
                return this.IsLeave == -1 ? "N/A" : this.IsLeave.ToString();
            }
        }

        public int ApprovalFlag { get; set; }

        /// <summary>
        /// 报名人数：只要点预定就算
        /// </summary>
        public int Enter_OrderNum { get; set; }

        /// <summary>
        /// 实际参与人数：预定成功且有考勤记录
        /// </summary>
        public int Actual_OrderNum { get; set; }

        /// <summary>
        /// 中位数 课程报名率
        /// </summary>
        public double ZWS_Enter_Pation_OrderNum
        {
            get
            {
                if (Pation_OrderNum != 0)
                {
                    return Math.Round((Convert.ToDouble(Enter_OrderNum) / Convert.ToDouble(Pation_OrderNum)), 4) * 100;
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// 课程参与率
        /// </summary>
        public double ZWS_Enter_Actual_OrderNum
        {
            get
            {
                return Enter_OrderNum == 0 ? 0 : Math.Round((Convert.ToDouble(Actual_OrderNum) / Convert.ToDouble(Enter_OrderNum)), 4) * 100;
            }
        }




        /// <summary>
        /// 应参加人数：一门课程对该部门应开放人数
        /// </summary>
        public int Pation_OrderNum { get; set; }

        /// <summary>
        /// 退订人数
        /// </summary>
        public int Un_OrderNum { get; set; }

        /// <summary>
        /// 补预定人数
        /// </summary>
        public int Bu_OrderNum { get; set; }

        public string Bu_OrderNumStr
        {
            get
            {
                return this.Bu_OrderNum == -1 ? "N/A" : this.Bu_OrderNum.ToString();
            }
        }

        /// <summary>
        /// 考勤开始时间
        /// </summary>
        public DateTime Attend_StartTime { get; set; }

        /// <summary>
        /// 考勤结束时间
        /// </summary>
        public DateTime Attend_EndTime { get; set; }

        /// <summary>
        /// 不预定 1：审批通过 0：没有
        /// </summary>
        public int Bu_ApprovalFlag { get; set; }

        /// <summary>
        /// 月 格式10
        /// </summary>
        public int month { get; set; }

        public decimal CourseLength { get; set; }

        /// <summary>
        /// 月 格式2014-10
        /// </summary>
        public string monthstr { get; set; }

        /// <summary>
        /// 转播所获得的分数 （未判断考勤）
        /// </summary>
        public decimal GetSumScore { get; set; }


        /// <summary>
        /// 转播所获得的分数 （真实的）
        /// </summary>
        public decimal GetRTScore
        {
            get
            {
                if (this.ApprovalFlag == 1)
                {
                    return this.GetSumScore < 0 ? 0 : GetSumScore;
                }
                return 0;
            }
        }


        public string AvgStr
        {
            get
            {
                return this.Avg_Cq.ToString("p") + "/<br>" + this.Avg_Cd.ToString("p") + "/<br>" + this.Avg_Qq.ToString("p");
            }
        }

        /// <summary>
        /// 每门课程的人数
        /// </summary>
        public int usercount { get; set; }

        /// <summary>
        /// 学时平均数
        /// </summary>
        public decimal Avg_Score
        {
            get
            {
                return usercount == 0 ? 0 : Math.Round(this.Avg_GetLength / Convert.ToDecimal(usercount), 2, MidpointRounding.AwayFromZero);
            }

        }

        /// <summary>
        ///是否有免修  0 没有免修 1 免修
        /// </summary>
        public int IsFree { get; set; }

        #region 主部门的学时

        public bool IsLink { get; set; }
        /// <summary>
        /// 计划外课程数
        /// </summary>
        public int IsLinkYearPlanWai { get; set; }


        /// <summary>
        /// 计划外课程总学时
        /// </summary>
        public decimal IsLinkYearPlanWai_length { get; set; }

        /// <summary>
        /// 计划内课程总学时
        /// </summary>
        public int IsLinkYearPlanNei { get; set; }

        /// <summary>
        /// 计划内课程总学时
        /// </summary>
        public decimal IsLinkYearPlanNei_length { get; set; }


        /// <summary>
        /// 框架外课程数
        /// </summary>
        public int IsLinkSystemNei { get; set; }

        /// <summary>
        /// 框架外课程总学时
        /// </summary>
        public decimal IsLinkSystemNei_length { get; set; }

        /// <summary>
        /// 框架外课程数
        /// </summary>
        public int IsLinkSystemWai { get; set; }

        /// <summary>
        /// 框架外课程总学时
        /// </summary>
        public decimal IsLinkSystemWai_length { get; set; }

        /// <summary>
        /// 计划外课程数
        /// </summary>
        public int SumYearPlanWai
        {
            get
            {
                return this.IsYearPlanWai + this.IsLinkYearPlanWai;
            }
        }


        /// <summary>
        /// 计划外课程总学时
        /// </summary>
        public decimal SumYearPlanWai_length
        {
            get
            {
                return this.IsYearPlanWai_length + this.IsLinkYearPlanWai_length;
            }
        }

        /// <summary>
        /// 计划内课程总学时
        /// </summary>
        public int SumYearPlanNei
        {
            get
            {
                return this.IsYearPlanNei + this.IsLinkYearPlanNei;
            }
        }

        /// <summary>
        /// 计划内课程总学时
        /// </summary>
        public decimal SumYearPlanNei_length
        {
            get
            {
                return this.IsYearPlanNei_length + this.IsLinkYearPlanNei_length;
            }
        }


        /// <summary>
        /// 框架外课程数
        /// </summary>
        public int SumSystemNei
        {
            get
            {
                return this.IsSystemNei + this.IsLinkSystemNei;
            }
        }

        /// <summary>
        /// 框架外课程总学时
        /// </summary>
        public decimal SumSystemNei_length
        {
            get
            {
                return this.IsSystemNei_length + this.IsLinkSystemNei_length;
            }
        }

        /// <summary>
        /// 框架外课程数
        /// </summary>
        public int SumSystemWai
        {
            get
            {
                return this.IsSystemWai + this.IsLinkSystemWai;
            }
        }


        /// <summary>
        /// 框架外课程总学时
        /// </summary>
        public decimal SumSystemWai_length
        {
            get
            {
                return this.IsSystemWai_length + this.IsLinkSystemWai_length;
            }
        }
        #endregion

    }
}
