using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewCourseManage
{
    public class New_CpaLearnStatus
    {

        public New_CpaLearnStatus() { }
        #region Model
        private int _id;
        private int _courseid;
        private int? _userid;
        private int? _isattflag;
        private int? _ispass;
        private int? _progress;
        private int? _learntimes;
        private decimal? _getlength = 0M;
        private DateTime? _lastupdatetime;
        private int? _cpaflag = 0;
        private int? _gradestatus = 0;
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
        public int CourseID
        {
            set { _courseid = value; }
            get { return _courseid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsAttFlag
        {
            set { _isattflag = value; }
            get { return _isattflag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsPass
        {
            set { _ispass = value; }
            get { return _ispass; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Progress
        {
            set { _progress = value; }
            get { return _progress; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? LearnTimes
        {
            set { _learntimes = value; }
            get { return _learntimes; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? GetLength
        {
            set { _getlength = value; }
            get { return _getlength; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 0：视频课程；1：CPA课程；2：转换的CPA课程
        /// </summary>
        public int? CpaFlag
        {
            set { _cpaflag = value; }
            get { return _cpaflag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? GradeStatus
        {
            set { _gradestatus = value; }
            get { return _gradestatus; }
        }
        #endregion Model
    }
}
