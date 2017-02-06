using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewCourseManage
{
    ///<summary>
    ///New_MidAttendce
    ///</summary>
    public partial class New_MidAttendce
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
        /// SubCourseId
        /// </summary>
        public string SubCourseId { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Time
        /// </summary>
        public int Time { get; set; }
        #endregion Model
    }
}
