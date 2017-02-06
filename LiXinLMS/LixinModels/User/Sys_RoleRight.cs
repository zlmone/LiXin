using System;

namespace LiXinModels.User
{
    /// <summary>
    ///     Sys_RoleRight:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Sys_RoleRight
    {
        #region Model

        private int _rightid;
        private int _roleid;

        /// <summary>
        /// </summary>
        public int RoleId
        {
            set { _roleid = value; }
            get { return _roleid; }
        }

        /// <summary>
        /// </summary>
        public int RightId
        {
            set { _rightid = value; }
            get { return _rightid; }
        }

        #endregion Model
    }
}