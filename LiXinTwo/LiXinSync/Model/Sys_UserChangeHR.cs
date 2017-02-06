using System;

namespace LiXinSync.User
{
    /// <summary>
    ///     hr的User变更视图
    /// </summary>
    [Serializable]
    public class Sys_UserChangeHR : BaseModel
    {
        public string tId { get; set; }

        public string colName { get; set; }

        public string tableName { get; set; }

        public DateTime makeDate { get; set; }

        public string modifyType { get; set; }
    }
}