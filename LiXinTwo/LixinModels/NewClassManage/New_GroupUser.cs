using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace LixinModels.NewClassManage
{
    //New_GroupUser
    public class New_GroupUser
    {
        #region == model==
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ClassId
        /// </summary>
        public int ClassId { get; set; }
        /// <summary>
        /// GroupId
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// NumberID
        /// </summary>
        public string NumberID { get; set; }
        #endregion

        #region == 扩展字段 ==
        public int totalcount { get; set; }

        public string GroupName { get; set; }

        public string ClassName { get; set; }
        #endregion

    }
}