using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewCourseManage
{
    ///<summary>
    ///New_CoursePaper
    ///</summary>
    public partial class New_CoursePaper
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
        /// PaperId
        /// </summary>
        public int PaperId { get; set; }
        /// <summary>
        /// Length
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// Hour
        /// </summary>
        public int Hour { get; set; }
        /// <summary>
        /// TotalScore
        /// </summary>
        public int TotalScore { get; set; }
        /// <summary>
        /// LevelScore
        /// </summary>
        public int LevelScore { get; set; }
        /// <summary>
        /// TestTimes
        /// </summary>
        public int TestTimes { get; set; }
        #endregion Model
    }
}
