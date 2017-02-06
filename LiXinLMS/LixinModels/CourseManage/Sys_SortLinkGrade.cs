using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.CourseManage
{
   public class Sys_SortLinkGrade
    {
        #region Model
        private int _id;
        private int? _sortid;
        private int? _gradeid;
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
        public int? SortID
        {
            set { _sortid = value; }
            get { return _sortid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? GradeID
        {
            set { _gradeid = value; }
            get { return _gradeid; }
        }

        public int CourseId { get; set; }
        #endregion Model
    }
}
