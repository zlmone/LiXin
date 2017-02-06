using System;

namespace LiXinModels.CourseLearn
{
    [Serializable]
    public class Cl_MakeUpOrder
    {
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
        /// <summary>
        /// ApprovalLimitTime
        /// </summary>
        public DateTime ApprovalLimitTime { get; set; }
        /// <summary>
        /// IsTimeOut
        /// </summary>
        public int IsTimeOut { get; set; }
        /// <summary>
        /// TimeOutMemo
        /// </summary>
        public string TimeOutMemo { get; set; }
        /// <summary>
        /// TimeOutUserID
        /// </summary>
        public string TimeOutUserID { get; set; }
        /// <summary>
        /// TimeOutPassFlag
        /// </summary>
        public int TimeOutPassFlag { get; set; }
        /// <summary>
        /// TimeOutApprovalMemo
        /// </summary>
        public string TimeOutApprovalMemo { get; set; }
        /// <summary>
        /// TimeOutDateTime
        /// </summary>
        public DateTime TimeOutDateTime { get; set; }
        /// <summary>
        /// TimeOutApprovalDateTime
        /// </summary>
        public DateTime TimeOutApprovalDateTime { get; set; }
        /// <summary>
        /// LeaveUserID
        /// </summary>
        public string LeaveUserID { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// CourseName
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// LeaveTime
        /// </summary>
        public DateTime LeaveTime { get; set; }
        /// <summary>
        /// CourseStartTime
        /// </summary>
        public DateTime CourseStartTime { get; set; }
        /// <summary>
        /// CourseEndTime
        /// </summary>
        public DateTime CourseEndTime { get; set; }
        /// <summary>
        /// AttStartTime
        /// </summary>
        public DateTime AttStartTime { get; set; }
        /// <summary>
        /// AttEndTime
        /// </summary>
        public DateTime AttEndTime { get; set; }
        /// <summary>
        /// requestid
        /// </summary>
        public int requestid { get; set; }
        /// <summary>
        /// FtriggerFlag
        /// </summary>
        public int FtriggerFlag { get; set; }
    }
}
