using LiXinModels.DepCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.DepCourseManage
{
    public interface IDep_CoursePaper
    {
        Dep_Coursepaper GetCo_CourseMainPaper(int CourseId);
    }
}
