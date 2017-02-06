using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.CourseManage
{
    public class Cl_VideoManage
    {
        public Cl_VideoManage()
        { }
        #region Model
        private int _id;
        private string _name = "";
        private string _path = "";
        private DateTime? _lastupdatetime = Convert.ToDateTime("2000 - 1 - 1");
        private int? _size = 0;
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
        public string Path
        {
            set { _path = value; }
            get { return _path; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastUpdateTime
        {
            set { _lastupdatetime = value; }
            get { return _lastupdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Size
        {
            set { _size = value; }
            get { return _size; }
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

        public string TopContent { get; set; }

        public string LeftContent { get; set; }

        public string RightContent { get; set; }

        public string BottomContent { get; set; }
    }
}
