using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    public class Sys_NoteResource
    {

        public int Id { get; set; }

        public int NoteId { get; set; }

        public string RealName {get;set;}

        public string FileName{get;set;}

          public DateTime  CreateDate{get;set;}

          public int IsDelete { get; set; }

    }
}
