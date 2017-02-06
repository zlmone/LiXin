using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepCourseManage
{
    public class Dep_CourseResource
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string ResourceName { get; set; }

        public string RealName { get; set; }

        public int ResourceType { get; set; }

        public int ResourceSize { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int PackId { get; set; }

        public int Flag { get; set; }

        public int IsDelete { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string ExtendName
        {
            get
            {
                int indexOfType = RealName.LastIndexOf('.');
                return RealName.Substring(indexOfType + 1);
            }
        }
    }
}
