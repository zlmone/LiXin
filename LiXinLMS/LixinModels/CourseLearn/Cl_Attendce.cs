using System;
namespace LiXinModels.CourseLearn
{
    /// <summary>
    /// Cl_Attendce:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Cl_Attendce
    {
        public Cl_Attendce(){ }
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
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// OnlineStartTime
        /// </summary>
        public DateTime OnlineStartTime { get; set; }
        /// <summary>
        /// OnlineEndTime
        /// </summary>
        public DateTime OnlineEndTime { get; set; }
        /// <summary>
        /// LessLength
        /// </summary>
        public decimal LessLength { get; set; }
        /// <summary>
        /// ApprovalUser
        /// </summary>
        public string ApprovalUser { get; set; }
        /// <summary>
        /// ApprovalMemo
        /// </summary>
        public string ApprovalMemo { get; set; }
        /// <summary>
        /// ApprovalFlag
        /// </summary>
        public int ApprovalFlag { get; set; }
        /// <summary>
        /// ApprovalDateTime
        /// </summary>
        public DateTime ApprovalDateTime { get; set; }
        #endregion Model

        public string Realname { get; set; }

        public string DeptName { get; set; }
        /// <summary>
        /// CPA标志
        /// </summary>
        public string CPA { get; set; }

        /// <summary>
        /// 预定状态
        /// </summary>
        public int OrderStatus { get; set; }

        public int MakeOrederId { get; set; }

        public string JobNum { get; set; }

        public int NumberLimited { get; set; }
        public int IsLeave { get; set; }
        public int LearnStatus { get; set; }
        //public int ApprovalFlag { get; set; }
        //public DateTime ApprovalDateTime { get; set; }
        public DateTime ApprovalLimitTime { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime OrderEndTime { get; set; }
        public DateTime courseStart { get; set; }
        public DateTime courseEnd { get; set; }
        public string StartTimeM
        {
            get
            {
                if (StartTime.Year == 1)
                {
                    return "";
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

        public string EndTimeM
        {
            get
            {
                if (EndTime.Year == 1)
                {
                    return "";
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

        public int totalcount
        {
            get;
            set;
        }

        public Int64 num
        {
            get;
            set;
        }

        /// <summary>
        /// 是否预定
        /// </summary>
        public string OrderStr
        {
            get
            {
                if (OrderStatus == 3)
                {
                    return "否";
                }
                else
                {
                    return "是";
                }
            }
        }

        /// <summary>
        /// 实到人数
        /// </summary>
        public int actual{ get; set; }

        /// <summary>
        /// 已报名人数
        /// </summary>
        public int Ordersum{ get; set; }

        /// <summary>
        /// 补预定人数
        /// </summary>
        public int OrderOut { get; set; }

        /// <summary>
        /// 迟到人数
        /// </summary>
        public int agosum { get; set; }

        /// <summary>
        /// 早退人数
        /// </summary>
        public int lastsum { get; set; }

        /// <summary>
        /// 迟到率
        /// </summary>
        public string agoRate
        {
            get
            {

                if (actual == 0)
                {
                    return "0%";
                }
                else
                {
                    return (agosum * 100 / actual).ToString() + "%";
                }
            }
        }

        /// <summary>
        /// 早退率
        /// </summary>
        public string lastRate
        {
            get
            {
                if (actual == 0)
                {
                    return "0%";
                }
                else
                {
                    return (lastsum * 100 / actual).ToString() + "%";
                }
            }
        }

        /// <summary>
        /// 考勤方式 
        /// </summary>
        public int AttFlag { get; set; }

        /// <summary>
        /// 考勤状态 
        /// </summary>
        public int AttStatus { get; set; }
    }
}

