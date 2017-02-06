using System;

namespace LiXinModels.User
{
    /// <summary>
    ///     Sys_Post:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Sys_Post
    {
        #region Model

        private int? _isdelete = 0;
        private int? _parentid = 0;
        private int? _postLevel;
        private string _postcode;
        private int _postid;
        private string _postname;
        private string _remark;

        /// <summary>
        /// </summary>
        public int PostId
        {
            set { _postid = value; }
            get { return _postid; }
        }

        /// <summary>
        /// </summary>
        public int? ParentId
        {
            set { _parentid = value; }
            get { return _parentid; }
        }

        /// <summary>
        /// </summary>
        public string PostCode
        {
            set { _postcode = value; }
            get { return _postcode; }
        }

        /// <summary>
        /// </summary>
        public string PostName
        {
            set { _postname = value; }
            get { return _postname; }
        }

        /// <summary>
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        public int? PostLevel
        {
            set { _postLevel = value; }
            get { return _postLevel; }
        }

        /// <summary>
        /// </summary>
        public int? IsDelete
        {
            set { _isdelete = value; }
            get { return _isdelete; }
        }

        #endregion Model

        public string parentPostName { get; set; }

        public string PostNameShow
        {
            get
            {
                //return PostCode + "[" + PostName + "]";
                return PostName;
            }
        }
    }
}