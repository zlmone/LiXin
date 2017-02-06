using System;

namespace LiXinModels.User
{
    /// <summary>
    ///     Sys_Right:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Sys_Right
    {
        #region Model

        private int _parentid;
        private string _path;
        private string _rightdesc;
        private int _rightid;
        private string _rightname;
        private int _righttype;
        private int? _showorder;
        private string _moduleName;

        /// <summary>
        /// </summary>
        public int RightId
        {
            set { _rightid = value; }
            get { return _rightid; }
        }

        /// <summary>
        /// </summary>
        public string RightName
        {
            set { _rightname = value; }
            get { return _rightname; }
        }

        /// <summary>
        ///     0：菜单权限；1：字段权限；2：操作权限
        /// </summary>
        public int RightType
        {
            set { _righttype = value; }
            get { return _righttype; }
        }

        /// <summary>
        /// </summary>
        public string RightDesc
        {
            set { _rightdesc = value; }
            get { return _rightdesc; }
        }

        /// <summary>
        /// </summary>
        public string Path
        {
            set { _path = value; }
            get { return _path; }
        }

        /// <summary>
        /// </summary>
        public int ParentId
        {
            set { _parentid = value; }
            get { return _parentid; }
        }

        /// <summary>
        /// </summary>
        public int? ShowOrder
        {
            set { _showorder = value; }
            get { return _showorder; }
        }
        /// <summary>
        /// 所属模块名称
        /// </summary>
        public string ModuleName{set { _moduleName = value; }get { return _moduleName; }}

        #endregion Model

    }
}