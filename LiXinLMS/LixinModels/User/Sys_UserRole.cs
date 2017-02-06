using System;

namespace LiXinModels.User
{
    /// <summary>
    ///     Sys_UserRole:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Sys_UserRole
    {
        #region Model

        private int _roleid;
        private int _userid;

        /// <summary>
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }

        /// <summary>
        /// </summary>
        public int RoleId
        {
            set { _roleid = value; }
            get { return _roleid; }
        }

        #endregion Model
    }
}