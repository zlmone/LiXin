using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.DepCourseManage
{
    public class Dep_Coursepaper
    {
        public int Id { get; set; }

        public int PaperId { get; set; }

        public int CourseId { get; set; }

        public int Length { get; set; }

        public int Hour { get; set; }

        public int TotalScore { get; set; }

        public int LevelScore { get; set; }

        public int TestTimes { get; set; }

        public int IsMain { get; set; }
    }
}
