using System;

namespace LiXinModels.User
{
    /// <summary>
    ///     Sys_Roles:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Sys_Roles
    {
        #region Model

        private int _creater;
        private DateTime _createtime = DateTime.Now;
        private int _isdefault;
        private int _isdelete;
        private string _roledesc;
        private int _roleid;
        private string _rolename;
        private int _status;

        /// <summary>
        /// </summary>
        public int RoleId
        {
            set { _roleid = value; }
            get { return _roleid; }
        }

        /// <summary>
        /// </summary>
        public string RoleName
        {
            set { _rolename = value; }
            get { return _rolename; }
        }

        /// <summary>
        /// </summary>
        public string RoleDesc
        {
            set { _roledesc = value; }
            get { return _roledesc; }
        }

        /// <summary>
        ///     是否默认角色
        ///     1：是
        ///     0：否
        /// </summary>
        public int IsDefault
        {
            set { _isdefault = value; }
            get { return _isdefault; }
        }

        /// <summary>
        /// </summary>
        public int Creater
        {
            set { _creater = value; }
            get { return _creater; }
        }

        /// <summary>
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        /// <summary>
        ///     是否冻结
        ///     1：已冻结
        ///     0：未冻结
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }

        /// <summary>
        /// </summary>
        public int IsDelete
        {
            set { _isdelete = value; }
            get { return _isdelete; }
        }

        #endregion Model

        public int UserCount { get; set; }

        public string Realname { get; set; }

        public string CreateTimeStr
        {
            get { return CreateTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }
    }
}