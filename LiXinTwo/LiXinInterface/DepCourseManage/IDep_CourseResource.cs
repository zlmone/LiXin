using LiXinModels.DepCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.DepCourseManage
{
    public interface IDep_CourseResource
    {
        List<Dep_CourseResource> GetCourseResourceList(int courseId);

        bool DeleteCourseResource(int courseId);

        List<Dep_CourseResource> GetList(int CourseId);

        bool AddCourseResource(Dep_CourseResource model);
    }
}
