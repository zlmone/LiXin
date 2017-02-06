using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepAttendce
{
    ///<summary>
    ///Dep_CourseAttFile
    ///</summary>
    public partial class Dep_CourseAttFile
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
        /// Type
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// RealName
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// LoadTimes
        /// </summary>
        public int LoadTimes { get; set; }
        /// <summary>
        /// FileSize
        /// </summary>
        public int FileSize { get; set; }
        /// <summary>
        /// IsDelete
        /// </summary>
        public int IsDelete { get; set; }
        #endregion Model

        public string CreateTimeStr
        {
            get
            {
                return this.CreateTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
    }
}
