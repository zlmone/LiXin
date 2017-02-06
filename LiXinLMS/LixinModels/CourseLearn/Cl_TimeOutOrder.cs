using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.CourseLearn
{
    public class Cl_TimeOutOrder
    {
        #region Model
        /// <summary>
        /// 审批标志位（0：未审批；1：通过；2：拒绝）
        /// </summary>
        public int? ApprovalFlag { set { _approvalflag = value; } get { return _approvalflag; } }
        private int _id;
        private int? _courseid;
        private int? _userid;
        private string _approvaluser;
        private string _outuserid;
        private string _name;
        private string _approvalreason;
        private DateTime? _makeuptime;
        private string _coursename;
        private DateTime? _outtime;
        private DateTime? _coursestarttime;
        private DateTime? _courseendtime;
        private DateTime? _attstarttime;
        private DateTime? _attendtime;
        private string _approvalmemo;
        private int? _approvalflag;
        private DateTime? _approvaldatetime;
        private int? _requestid;
        private int? _ftriggerflag = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CourseId
        {
            set { _courseid = value; }
            get { return _courseid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ApprovalUser
        {
            set { _approvaluser = value; }
            get { return _approvaluser; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OutUserID
        {
            set { _outuserid = value; }
            get { return _outuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ApprovalReason
        {
            set { _approvalreason = value; }
            get { return _approvalreason; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? MakeUpTime
        {
            set { _makeuptime = value; }
            get { return _makeuptime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CourseName
        {
            set { _coursename = value; }
            get { return _coursename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? OutTime
        {
            set { _outtime = value; }
            get { return _outtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CourseStartTime
        {
            set { _coursestarttime = value; }
            get { return _coursestarttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CourseEndTime
        {
            set { _courseendtime = value; }
            get { return _courseendtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AttStartTime
        {
            set { _attstarttime = value; }
            get { return _attstarttime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AttEndTime
        {
            set { _attendtime = value; }
            get { return _attendtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ApprovalMemo
        {
            set { _approvalmemo = value; }
            get { return _approvalmemo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ApprovalDateTime
        {
            set { _approvaldatetime = value; }
            get { return _approvaldatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? requestid
        {
            set { _requestid = value; }
            get { return _requestid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? FtriggerFlag
        {
            set { _ftriggerflag = value; }
            get { return _ftriggerflag; }
        }

        /// <summary>
        /// 1 请假   0其他   用来判断逾时申请的来源
        /// </summary>
        public int Typefrom { get; set; }
        #endregion Model

    }
}
