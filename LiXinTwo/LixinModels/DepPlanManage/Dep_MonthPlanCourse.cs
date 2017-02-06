using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepPlanManage
{
    public class Dep_MonthPlanCourse
    {
        #region Model
        /// <summary>
        /// MonthId
        /// </summary>		
        public int MonthId { get; set; }

        /// <summary>
        /// CourseId
        /// </summary>		
        public int CourseId { get; set; }

        public int IsSystem { get; set; }
        #endregion
    }
}
