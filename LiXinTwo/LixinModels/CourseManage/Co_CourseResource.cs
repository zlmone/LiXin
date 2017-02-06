using System;
using System.Configuration;
namespace LiXinModels.CourseManage
{
    /// <summary>
    /// Co_CourseResource:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Co_CourseResource
    {
        public Co_CourseResource()
        { }
        #region Model
        private int _id;
        private int? _courseid;
        private string _resourcename = "";
        private string _realname = "";
        private int _resourcetype;
        private int? _resourcesize;
        private DateTime? _lastupdatetime;
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
        public string ResourceName
        {
            set { _resourcename = value; }
            get { return _resourcename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RealName
        {
            set { _realname = value; }
            get { return _realname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ResourceType
        {
            set { _resourcetype = value; }
            get { return _resourcetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ResourceSize
        {
            set { _resourcesize = value; }
            get { return _resourcesize; }
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
        /// 扩展名
        /// </summary>
        public string ExtendName
        {
            get
            {
                int indexOfType = RealName.LastIndexOf('.');
                return RealName.Substring(indexOfType + 1);
            }
        }
        #endregion Model

        public int PackId { get; set; }

        public int IsDelete { get; set; }

        public int Flag { get; set; }

        public string DownLoadUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["UFCOResource"] + RealName;
            }
        }

        /// <summary>
        /// 进度
        /// </summary>
        public decimal Progress { get; set; }

        /// <summary>
        /// 次数
        /// </summary>
        public int LearnTimes { get; set; }


        public int IsPing { get; set; }

        public int CourseResourceId { get; set; }

        public int VideoManageIsDelete { get; set; }


        //public int ResourceType { get; set; }

    }
}

