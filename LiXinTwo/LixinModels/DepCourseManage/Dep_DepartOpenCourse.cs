using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepCourseManage
{
    ///<summary>
    ///Dep_DepartOpenCourse
    ///</summary>
    public partial class Dep_DepartOpenCourse
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
        /// DeCourseId
        /// </summary>
        public int DeCourseId { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }
        #endregion Model
    }
}
