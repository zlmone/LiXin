using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.CourseManage
{
  public  class Sys_SortGradeCourse
    {
        #region Model
        private int _id;
        private string _name = "";
        private string _openleavel = "";
        private int? _type = 0;
        private int? _ismust = 0;
        private int? _isdelete = 0;
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OpenLeavel
        {
            set { _openleavel = value; }
            get { return _openleavel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsMust
        {
            set { _ismust = value; }
            get { return _ismust; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? IsDelete
        {
            set { _isdelete = value; }
            get { return _isdelete; }
        }
        #endregion Model

        public int sortid { get; set; }
    }
}
