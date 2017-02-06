using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.CourseLearn
{
    /// <summary>
    /// Cl_CpaLearnStatus:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public  class Cl_CpaLearnStatus
    {
        public Cl_CpaLearnStatus()
        { }
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

        /// <summary>
        /// 2013-11-21 添加 记录观看视频总时长
        /// </summary>
        public decimal VedioTime { get; set; }
        #endregion Model

        #region 连表查询字段
        /// <summary>
        /// 学员所在组织结构
        /// </summary>
        public string DeptName { get; set; }

        //学员真实姓名
        public string Realname { get; set; }

        public string MobileNum { get; set; }

        //学员CPA 会计师编码
        public string CPANO { get; set; }
        #endregion
    }
}
