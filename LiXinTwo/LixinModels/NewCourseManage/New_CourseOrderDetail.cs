using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewCourseManage
{
    ///<summary>
    ///New_CourseOrderDetail
    ///</summary>
    public partial class New_CourseOrderDetail
    {
        #region Model
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// CourseId
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// SubCourseID
        /// </summary>
        public int SubCourseID { get; set; }
        /// <summary>
        /// LearnTime
        /// </summary>
        public DateTime LearnTime { get; set; }
        /// <summary>
        /// LearnStatus
        /// </summary>
        public int LearnStatus { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 是否请假
        /// </summary>
        public int IsLeave { set; get; }
        /// <summary>
        /// 请假审批人
        /// </summary>
        public string ApprovalName { set; get; }
        /// <summary>
        /// 请假理由
        /// </summary>
        public string LeaveReason { set; get; }

        #endregion Model

        /// <summary>
        /// 应出勤人数
        /// </summary>
        public int CoCount { get; set; }
        /// <summary>
        /// 实际出勤人数
        /// </summary>
        public int AttCount { get; set; }
        /// <summary>
        /// 课程开始时间
        /// </summary>
        public DateTime CoStartTime { get; set; }
        /// <summary>
        /// 课程结束时间
        /// </summary>
        public DateTime CoEndTime { get; set; }

        /// <summary>
        /// 考勤开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 考勤结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string NumberID { get; set; }

        /// <summary>
        /// 班级Id
        /// </summary>
        public int ClassId { get; set; }
        /// <summary>
        /// 班级Name
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 组Name
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 组Id
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 学员姓名
        /// </summary>
        public string Realname { get; set; }


        public int totalcount { get; set; }

        /// <summary>
        /// 考勤ID
        /// </summary>
        public int attId { get; set; }

        /// <summary>
        /// 考勤状态
        /// </summary>
        public int AttStatus { get; set; }

        /// <summary>
        /// 是否考勤
        /// </summary>
        public int IsAttFlag { get; set; }

        /// <summary>
        /// 教室
        /// </summary>
        public string RoomName { get; set; }


        public string StartTimeStr
        {
            get
            {
                if (StartTime.Year == 1)
                {
                    return "——";
                }
                else if (StartTime.Year == 2050)
                {
                    return "——";
                }
                else
                {
                    return StartTime.ToString("yyyy-MM-dd HH:mm");
                }
            }
        }
        public string EndTimeStr
        {
            get
            {
                if (EndTime.Year == 1)
                {
                    return "——";
                }
                else if (EndTime.Year == 2000)
                {
                    return "——";
                }
                else
                {
                    return EndTime.ToString("yyyy-MM-dd HH:mm");
                }
            }
        }
        public string CoTimeStr
        {
            get
            {
                return CoStartTime.ToString("yyyy-MM-dd HH:mm") + "-" + CoEndTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
        public string CoStartTimeStr
        {
            get
            {
                if (CoStartTime.Year == 1)
                {
                    return "——";
                }
                else if (CoStartTime.Year == 2050)
                {
                    return "——";
                }
                else
                {
                    return CoStartTime.ToString("yyyy-MM-dd HH:mm");
                }
            }
        }
        public string CoEndTimeStr
        {
            get
            {
                if (CoEndTime.Year == 1)
                {
                    return "——";
                }
                else if (CoEndTime.Year == 2000)
                {
                    return "——";
                }
                else
                {
                    return CoEndTime.ToString("yyyy-MM-dd HH:mm");
                }
            }
        }
        public string ShortCourseTimeStr
        {
            get
            {
                return StartTime.ToString("HH:mm") + "--" + EndTime.ToString("HH:mm");
            }
        }


        public string ClassAndGroup
        {
            get
            {
                return ClassName + "/" + GroupName;
            }
        }
        public string AttStatusStr
        {
            get
            {
                string reult = "";
                switch (IsAttFlag)
                {
                    case 0://上课、下课都考勤
                        if (attId == 0)
                        {
                            reult = "考勤未上传";
                        }
                        else
                        {
                            if (StartTime.Year != 2050 && StartTime.Year != 1 && EndTime.Year != 2000 && EndTime.Year != 1)
                            {
                                if (Convert.ToDateTime(StartTime.ToString("yyyy-MM-dd HH:mm")) <= Convert.ToDateTime(CoStartTime.ToString("yyyy-MM-dd HH:mm")) && Convert.ToDateTime(EndTime.ToString("yyyy-MM-dd HH:mm")) >= Convert.ToDateTime(CoEndTime.ToString("yyyy-MM-dd HH:mm")))
                                {
                                    reult = "正常";
                                }
                                else if (Convert.ToDateTime(StartTime.ToString("yyyy-MM-dd HH:mm")) > Convert.ToDateTime(CoStartTime.ToString("yyyy-MM-dd HH:mm")) && Convert.ToDateTime(EndTime.ToString("yyyy-MM-dd HH:mm")) >= Convert.ToDateTime(CoEndTime.ToString("yyyy-MM-dd HH:mm")))
                                {
                                    reult = "迟到";
                                }
                                else if (Convert.ToDateTime(StartTime.ToString("yyyy-MM-dd HH:mm")) <= Convert.ToDateTime(CoStartTime.ToString("yyyy-MM-dd HH:mm")) && Convert.ToDateTime(EndTime.ToString("yyyy-MM-dd HH:mm")) < Convert.ToDateTime(CoEndTime.ToString("yyyy-MM-dd HH:mm")))
                                {
                                    reult = "早退";
                                }
                                else
                                {
                                    reult = "迟到并早退";
                                }
                            }
                            else if (StartTime.Year != 2050 && StartTime.Year != 1 && (EndTime.Year == 2000 || EndTime.Year == 1))
                            {
                                if (Convert.ToDateTime(StartTime.ToString("yyyy-MM-dd HH:mm")) <= Convert.ToDateTime(CoStartTime.ToString("yyyy-MM-dd HH:mm")))
                                {
                                    reult = "早退";
                                }
                                else
                                {
                                    reult = "迟到并早退";
                                }
                            }
                            else if ((StartTime.Year == 2050 || StartTime.Year == 1) && EndTime.Year != 2000 && EndTime.Year != 1)
                            {
                                if (Convert.ToDateTime(EndTime.ToString("yyyy-MM-dd HH:mm")) >= Convert.ToDateTime(CoEndTime.ToString("yyyy-MM-dd HH:mm")))
                                {
                                    reult = "迟到";
                                }
                                else
                                {
                                    reult = "迟到并早退";
                                }
                            }
                            else
                            {
                                reult = "缺勤";
                            }
                        }
                        break;
                    case 1://仅上课考勤
                        if (attId == 0)
                        {
                            reult = "考勤未上传";
                        }
                        else
                        {
                            if (StartTime.Year != 2050 && StartTime.Year != 1)
                            {
                                if (Convert.ToDateTime(StartTime.ToString("yyyy-MM-dd HH:mm")) <= Convert.ToDateTime(CoStartTime.ToString("yyyy-MM-dd HH:mm")))
                                {
                                    reult = "正常";
                                }
                                else
                                {
                                    reult = "迟到";
                                }
                            }
                            else
                            {
                                reult = "缺勤";
                            }
                        }
                        break;
                    case 2://仅下课考勤
                        if (attId == 0)
                        {
                            reult = "考勤未上传";
                        }
                        else
                        {
                            if (EndTime.Year != 2000 && EndTime.Year != 1)
                            {
                                if (Convert.ToDateTime(EndTime.ToString("yyyy-MM-dd HH:mm")) >= Convert.ToDateTime(CoEndTime.ToString("yyyy-MM-dd HH:mm")))
                                {
                                    reult = "正常";
                                }
                                else
                                {
                                    reult = "早退";
                                }
                            }
                            else
                            {
                                reult = "缺勤";
                            }
                        }
                        break;
                    case 3://不考勤  
                        reult = "正常";
                        break;
                }
                return reult;
            }
        }
        /// <summary>
        /// 讲师姓名
        /// </summary>
        public string teachername { get; set; }

        /// <summary>
        /// 座位坐标
        /// </summary>
        public string SeatDetail { get; set; }
        /// <summary>
        /// 详细座位名 几排几列
        /// </summary>
        public string SeatDetailName { get; set; }

        /// <summary>
        /// 考勤标示符，谨防有一天他抽风改中文名字。。
        /// 正常=0,缺勤 = 1, 早退 = 2,迟到 = 3,迟到并早退 = 4
        /// </summary>
        public int AttStatusFlag
        {
            get
            {
                return (int)((Enums.AttStatusFlag)Enum.Parse(typeof(Enums.AttStatusFlag), this.AttStatusStr));
            }
        }

        public string CourseName { get; set; }

      
    }
}
