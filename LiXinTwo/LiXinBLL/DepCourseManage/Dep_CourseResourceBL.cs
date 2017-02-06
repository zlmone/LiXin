using LiXinDataAccess.DepCourseManage;
using LiXinInterface.DepCourseManage;
using LiXinModels.DepCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.DepCourseManage
{
    public class Dep_CourseResourceBL : IDep_CourseResource
    {
        private Dep_CourseResourceDB dep_courseResourceDb;

        public Dep_CourseResourceBL(Dep_CourseResourceDB _dep_courseResourceDb)
        {
            dep_courseResourceDb = _dep_courseResourceDb;
        }

        public List<Dep_CourseResource> GetCourseResourceList(int courseId)
        {
            return dep_courseResourceDb.GetCourseResourceList(courseId);
        }

        public bool DeleteCourseResource(int courseId)
        {
            return dep_courseResourceDb.DeleteCourseResource(courseId);
        }

        public List<Dep_CourseResource> GetList(int CourseId)
        {
            return dep_courseResourceDb.GetList(CourseId);
        }
         /// <summary>
        /// 新增课程关联资源
        /// </summary>
        /// <param name="model"></param>
        public bool AddCourseResource(Dep_CourseResource model)
        {
            return dep_courseResourceDb.AddCourseResource(model);
        }
    }
}
