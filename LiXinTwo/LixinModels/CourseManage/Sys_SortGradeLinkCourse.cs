using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.CourseManage
{
   public class Sys_SortGradeLinkCourse
    {
        #region Model
        private int _id;
        private int? _sortgradeid;
        private int? _courseid;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? SortGradeID
        {
            set { _sortgradeid = value; }
            get { return _sortgradeid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CourseID
        {
            set { _courseid = value; }
            get { return _courseid; }
        }
        #endregion Model
    }
}
