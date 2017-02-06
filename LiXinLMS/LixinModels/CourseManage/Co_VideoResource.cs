using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.CourseManage
{
    public class Co_VideoResource
    {
        public Co_VideoResource()
        {
        }

        public int Id { get; set; }

        public int VideoId { get; set; }

        public string TopContent { get; set; }

            public string LeftContent { get; set; }

           public string  RightContent { get; set; }

            public string BottomContent { get; set; }

            public DateTime LastUpdateTime { get; set; }
    }
}
