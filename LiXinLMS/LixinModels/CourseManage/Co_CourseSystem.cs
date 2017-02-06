using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.CourseManage
{
  public class Co_CourseSystem
    {
        public int Id { get; set; }

      /// <summary>
      /// 课程体系名称
      /// </summary>
        public string CourseSystemName { get; set; }

      /// <summary>
      /// 父级ID
      /// </summary>
        public int ParentID { get; set; }

      /// <summary>
      /// 描述
      /// </summary>
        public string Memo { get; set; }

        private int isDelete = 0;
      /// <summary>
        /// 0：正常：1删除
      /// </summary>
        public int IsDelete { get{ return isDelete == 0 ? 0 : isDelete;} set{ isDelete=value; }}
    }
}
