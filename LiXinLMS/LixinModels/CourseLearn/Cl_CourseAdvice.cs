using System;
namespace LiXinModels.CourseLearn
{
    /// <summary>
    /// Cl_CourseAdvice:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Cl_CourseAdvice
    {
        public Cl_CourseAdvice()
        { }
        #region Model
        private int _id;
        private int? _courseid;
        private int? _userid;
        private string _advicecontent = "";
        private DateTime? _advicetime;
        private int? _anseruserid;
        private string _ansercontent = "";
        private DateTime? _ansertime;
        private int? _visibleflag;
        private int? _isdelete;
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
        public string AdviceContent
        {
            set { _advicecontent = value; }
            get { return _advicecontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AdviceTime
        {
            set { _advicetime = value; }
            get { return _advicetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? AnserUserId
        {
            set { _anseruserid = value; }
            get { return _anseruserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AnserContent
        {
            set { _ansercontent = value; }
            get { return _ansercontent; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AnserTime
        {
            set { _ansertime = value; }
            get { return _ansertime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? VisibleFlag
        {
            set { _visibleflag = value; }
            get { return _visibleflag; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsDelete
        {
            set { _isdelete = value; }
            get { return _isdelete; }
        }
        #endregion Model

        public string userRealname { get; set; }
        public string TeacherName { get; set; }
        
        public string userPhotoUrl { get; set; }

        public string userPhotoUrlStr
        {
            get
            {
                if (string.IsNullOrEmpty(userPhotoUrl))
                    return "~/Images/photo/default.jpg";
                return userPhotoUrl;
            }
        }

    }
}

