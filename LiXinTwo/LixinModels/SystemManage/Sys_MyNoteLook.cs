using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class Sys_MyNoteLook
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
        /// UserID
        /// </summary>		
        public int UserID { get; set; }

        /// <summary>
        /// LookTime
        /// </summary>		
        public DateTime LookTime { get; set; }

        /// <summary>
        /// IsDelete
        /// </summary>		
        public int IsDelete { get; set; }
    }
}
