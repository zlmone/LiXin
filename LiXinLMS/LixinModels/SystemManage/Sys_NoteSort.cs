using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
   public  class Sys_NoteSort
    {
        public int Id { get; set; }

        public string SortName { get; set; }


       /// <summary>
        /// 0: 通知公告 ；1：政策
       /// </summary>
        public int Type { get; set; }

        private int parentID = 0;
       /// <summary>
       /// 默认为0.  暂不做处理
       /// </summary>
        public int ParentID { get { return parentID==0 ? 0 : parentID; } set { parentID = value; } }



        public int IsDelete { get; set; }
    }
}
