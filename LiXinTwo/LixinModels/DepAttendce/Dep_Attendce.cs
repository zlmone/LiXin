using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepAttendce
{
    ///<summary>
    ///Dep_Attendce
    ///</summary>
    public partial class Dep_Attendce
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
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// ApprovalFlag
        /// </summary>
        public int ApprovalFlag { get; set; }
        /// <summary>
        /// Reason
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// DepartSetId
        /// </summary>
        public int DepartSetId { get; set; }

        public string CourseName { get; set; }

        public decimal CourseLength { get; set; }

        public int IsMust { get; set; }

        public string RoomName { get; set; }

        public int IsYearPlan { get; set; }

        public decimal AttScore { get; set; }

        public decimal EvlutionScore { get; set; }

        public decimal ExamScore { get; set; }

        public decimal total
        {
            get { return (AttScore + EvlutionScore + ExamScore); }
        }

        public string Realname { get; set; }

        public string DeptName { get; set; }

        public string TrainGrade { get; set; }

        

        public int OrderStatus { get;set; }

   
      

        #endregion Model

        public string StatusStr
        {
            get
            {
                return ((Enums.deptAttStatus)this.Status).ToString();
            }
        }

        public int totalcount { get; set; }
    }
}
