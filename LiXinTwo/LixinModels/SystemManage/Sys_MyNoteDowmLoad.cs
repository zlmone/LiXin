using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class Sys_MyNoteDowmLoad
    {
        /// <summary>
        /// ID
        /// </summary>		
        public int ID { get; set; }

        /// <summary>
        /// NoteID
        /// </summary>		
        public int NoteID { get; set; }

        /// <summary>
        /// ResourceID
        /// </summary>		
        public int ResourceID { get; set; }

        /// <summary>
        /// UserID
        /// </summary>		
        public int UserID { get; set; }

        /// <summary>
        /// DownLoadTime
        /// </summary>		
        public DateTime DownLoadTime { get; set; }

        /// <summary>
        /// IsDelete
        /// </summary>		
        public int IsDelete { get; set; } 
    }
}
