using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepCourseLearn
{
    ///<summary>
    ///Dep_CpaLearnStatus
    ///</summary>
    public partial class Dep_CpaLearnStatus
    {
        #region Model
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// CourseID
        /// </summary>
        public int CourseID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// GetLength
        /// </summary>
        public decimal GetLength { get; set; }
        /// <summary>
        /// LastUpdateTime
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 0：折算CPA；1：未知
        /// </summary>
        public int CpaFlag { get; set; }

        /// <summary>
        /// DepartSetId
        /// </summary>
        public int DepartSetId { get; set; }

        #endregion Model
    }
}
