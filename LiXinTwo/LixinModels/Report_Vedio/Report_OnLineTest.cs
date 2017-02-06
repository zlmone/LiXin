using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_Vedio
{
    /// <summary>
    /// 在线测试情况
    /// </summary>
    public class Report_OnLineTest
    {
        public string Username { get; set; }

        public string DeptName { get; set; }

        /// <summary>
        /// 参与率
        /// </summary>
        public string CanYuLv
        {
            get
            {
                return (Convert.ToDouble(YiCanJia) / Convert.ToDouble(YingCanJia)).ToString() + "%";
            }
        }

        /// <summary>
        /// 已参加人数
        /// </summary>
        public int YiCanJia { get; set; }

        /// <summary>
        /// 应参加人数
        /// </summary>
        public int YingCanJia { get; set; }

        /// <summary>
        /// 测试通过人数
        /// </summary>
        public int IsPassNum { get; set; }

        /// <summary>
        /// 参加过考试的人数
        /// </summary>
        public int OnLineTestNum { get; set; }

        /// <summary>
        /// 测试通过率
        /// </summary>
        public string CeShiPass
        {
            get
            {
                if (istest == 1)
                {
                    return (Convert.ToDouble(IsPassNum) / Convert.ToDouble(OnLineTestNum)).ToString("p") + "%";
                }
                else
                {
                    return "N/A";
                }
            }
        }

        /// <summary>
        /// 平均答题时间
        /// </summary>
        public string AvgAnswerTime { get; set; }

        public decimal AvgAnswerTimeMin
        {
            get
            {

                if (AvgAnswerTime != "N/A")
                {
                    return Math.Round(Convert.ToDecimal(AvgAnswerTime) / 60, 0);
                }

                else
                {
                    return 0;

                }
            }
        }
        public decimal AvgAnswerTimeMinDouble
        {
            get
            {
                return OnLineTestNum == 0 ? -1 : AvgAnswerTimeMin;
            }
        }


        public decimal AvgAnswerTimeStr
        {
            get
            {
                return Math.Round(Convert.ToDecimal(AvgAnswerTime), 1);
            }
        }

        /// <summary>
        /// 平均答题次数
        /// </summary>
        public string AvgAnswerNum { get; set; }

        public double AvgAnswerNumDouble
        {
            get
            {
                return OnLineTestNum == 0 ? -1 : Convert.ToDouble(AvgAnswerNum);
            }
        }

        /// <summary>
        /// 平均得分
        /// </summary>
        public string Avg { get; set; }


        public double AvgDouble
        {
            get
            {
                return OnLineTestNum == 0 ? -1 : Convert.ToDouble(Avg);
            }
        }


        /// <summary>
        /// 部门用户集合 1,2,3
        /// </summary>
        public string userlist { get; set; }


        /// <summary>
        /// 姓名
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 培训级别
        /// </summary>
        public string TrainGrade { get; set; }

        public string CPA { get; set; }

        public string CPANo { get; set; }

        /// <summary>
        /// 观看次数
        /// </summary>
        public int LearnTimes { get; set; }

        /// <summary>
        /// 获得的学时
        /// </summary>
        public decimal GetLength { get; set; }


        /// <summary>
        /// 考试状态
        /// </summary>
        public string ExamStatus { get; set; }

        /// <summary>
        /// 考试是否过
        /// </summary>
        public int IsPass { get; set; }

        /// <summary>
        /// 是否有考试
        /// </summary>
        public int istest { get; set; }


        public string StrExamStatus
        {
            get
            {
                //（0:未做 ;1:未提交（进行中）;2:已提交;3：已批阅）
                //<option value="1">未参与</option>
                //<option value="2">考试中</option>
                //<option value="6">已通过</option>
                //<option value="4">未通过</option>
                string a = "";
                //switch (ExamStatus)
                //{
                //    case 0: a = "未做"; break;
                //    case 1: a="未提交（进行中）";break;
                //    case 2: a = "已提交"; break;
                //    case 3: a = "已批阅"; break;
                //}
                if (istest == 1)
                {
                    if (IsPass == 0 && ExamStatus == "0")
                    {
                        a = "未参与";
                    }
                    if ((IsPass == 0 && ExamStatus == "1") || (IsPass == 1 && ExamStatus == "1"))
                    {
                        a = "考试中";
                    }
                    if (IsPass == 1 && ExamStatus == "2")
                    {
                        a = "已通过";
                    }
                    if (ExamStatus == "2" && IsPass == 0)
                    {
                        a = "未通过";
                    }
                }
                else
                {
                    a = "N/A";
                }
                return a;
            }
        }

        /// <summary>
        /// 考试次数
        /// </summary>
        public string ExamNum { get; set; }

        /// <summary>
        /// 考试成绩
        /// </summary>
        public string ExamScore { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        public decimal VedioTime { get; set; }

        public decimal VedioTimeByMin
        {
            get
            {
                //return Convert.ToInt32(VedioTime/60);

                return Math.Round(VedioTime / 60, 0);
            }
        }

        public int DeptId { get; set; }

        /// <summary>
        /// 观看视频总时间
        /// </summary>
        public decimal allVedioTime { get; set; }

        /// <summary>
        /// 平均观看时间
        /// </summary>
        public int AvgVedioTime
        {
            get
            {
                if (YiCanJia != 0)
                {
                    return Convert.ToInt32(Math.Round(allVedioTime / 60, 0) / YiCanJia);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 平均观看次数
        /// </summary>
        public int AvgLearnTimes
        {
            get
            {
                if (YiCanJia != 0)
                {
                    return LearnTimes / YiCanJia;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 月份
        /// </summary>
        public int monthstr { get; set; }

        // public int VedioTime { get; set; }
        /// <summary>
        /// 累计观看时间 分钟
        /// </summary>
        public double allVedioTimeByMin
        {
            get
            {
                return Math.Round(Convert.ToDouble(this.allVedioTime) / 60, 0);
            }
        }



        /// <summary>
        /// 参加率
        /// </summary>
        public decimal JoinPateDouble { get; set; }




        /// <summary>
        /// 参加率
        /// </summary>
        public string JoinPate
        {
            get
            {
                return JoinPateDouble.ToString("p");
            }
        }

        /// <summary>
        /// 测试通过率
        /// </summary>
        public decimal ExamPassPateDouble { get; set; }

        /// <summary>
        /// 测试通过率
        /// </summary>
        public string ExamPassPate
        {
            get
            {
                return ExamPassPateDouble.ToString("p");
            }
        }

        public string DeptIDs { get; set; }

        public string DeptTwoName { get; set; }

        public int IsFree { get; set; }

    }
}
