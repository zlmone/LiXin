using System;

namespace LiXinModels.User
{
    /// <summary>
    ///     Sys_Department:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Sys_Department
    {
        #region Model

        private int _departmentid;
        private string _deptcode;
        private string _deptname;
        private int? _isdelete = 0;
        private int _parentdeptid;
        private string _remark;
        private int? _typenum = 0;

        /// <summary>
        /// </summary>
        public int DepartmentId
        {
            set { _departmentid = value; }
            get { return _departmentid; }
        }

        /// <summary>
        /// </summary>
        public string DeptCode
        {
            set { _deptcode = value; }
            get { return _deptcode; }
        }

        /// <summary>
        /// </summary>
        public string DeptName
        {
            set { _deptname = value; }
            get { return _deptname; }
        }

        /// <summary>
        /// </summary>
        public int ParentDeptId
        {
            set { _parentdeptid = value; }
            get { return _parentdeptid; }
        }

        /// <summary>
        /// </summary>
        public int? IsDelete
        {
            set { _isdelete = value; }
            get { return _isdelete; }
        }

        /// <summary>
        /// </summary>
        public int? TypeNum
        {
            set { _typenum = value; }
            get { return _typenum; }
        }

        /// <summary>
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        public int DeptGrade { get; set; }

        public int OldDeptId { get; set; }

        #endregion Model

        public string parentDeptName { get; set; }

        public string DeptNameShow
        {
            get
            {
                //return DeptCode + "[" + DeptName + "]";
                return DeptName;
            }
        }

        /// <summary>
        /// 部门总人数
        /// </summary>
        public int AllCount { get; set; }

        /// <summary>
        /// 部门报名人数
        /// </summary>
        public int SubscribeCount { get; set; }

        /// <summary>
        /// 报名人数所占百分比
        /// </summary>
        public int Percent
        {
            get
            {
                if (AllCount == 0)
                    return 0;
                return SubscribeCount * 100 / AllCount;
            }
        }

        public int IsSubmit
        {
            get;
            set;
        }

        public string IsSubmitStr
        {
            get
            {
                return this.IsSubmit > 0 ? "已提交" : "未提交";
            }
        }

        /// <summary>
        /// 部门的路径
        /// </summary>
        public string DeptPath
        {
            get;
            set;
        }
    }
}