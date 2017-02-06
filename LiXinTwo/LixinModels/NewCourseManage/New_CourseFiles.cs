using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.NewCourseManage
{
    ///<summary>
    ///New_CourseFiles
    ///</summary>
    public partial class New_CourseFiles
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
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// RealName
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// LoadTimes
        /// </summary>
        public int LoadTimes { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// PackId
        /// </summary>
        public int PackId { get; set; }
        /// <summary>
        /// ResourceSize
        /// </summary>
        public int ResourceSize { get; set; }
        /// <summary>
        /// IsDelete
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 0:管理员；>0:讲师
        /// </summary>
        public int Flag { set; get; }

        #endregion Model

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

        /// <summary>
        /// 次数
        /// </summary>
        public int LearnTimes { get; set; }

        public int CourseResourceId { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        public decimal Progress { get; set; }

        public int VideoManageIsDelete { get; set; }
    }
}
