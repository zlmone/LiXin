using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.CourseLearn
{
    public class Cl_LearnVideoInfor
    {
        public int Id { get; set; }

        public int LearnId { get; set; }

        public decimal Progress { get; set; }

        public int ResourseId { get; set; }

        public int AttendId { get; set; }

        public int LearnTimes { get; set; }
    }
}
