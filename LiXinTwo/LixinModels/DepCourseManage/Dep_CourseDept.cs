using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepCourseManage
{
    ///<summary>
    ///Dep_CourseDept
    ///</summary>
    public partial class Dep_CourseDept
    {
        #region Model
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// CourseId
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// DepartId
        /// </summary>
        public int DepartId { get; set; }
        /// <summary>
        /// AttFlag
        /// </summary>
        public int AttFlag { get; set; }
        /// <summary>
        /// SubmitTime
        /// </summary>
        public DateTime SubmitTime { get; set; }
        /// <summary>
        /// OpenFlag
        /// </summary>
        public int OpenFlag { get; set; }
        /// <summary>
        /// ApprovalFlag
        /// </summary>
        public int ApprovalFlag { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        #endregion Model

        #region 短信邮件信息
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 真实课程名
        /// </summary>
        public string CourseName { get; set; }

        public string MobileNum { get; set; }

        public string Email { get; set; }

        #endregion
    }
}
