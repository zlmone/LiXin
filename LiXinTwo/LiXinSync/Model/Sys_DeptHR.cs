using System;

namespace LiXinSync.User
{
    /// <summary>
    ///     hr的Dept视图
    /// </summary>
    [Serializable]
    public class Sys_DeptHR : BaseModel
    {
        #region Model
        /// <summary>
        /// </summary>
        public int dept_id
        {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        public string DEPT_CODE
        {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        public string CONTEnT
        {
            get;
            set;
        }

        /// <summary>
        /// </summary>
        public string PDEPT_CODE
        {
            get;
            set;
        }

        public int Grade { get; set; }

        #endregion Model
    }
}