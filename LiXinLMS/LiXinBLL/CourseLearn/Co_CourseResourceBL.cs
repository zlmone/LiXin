using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.CourseManage;
using LiXinInterface.CourseLearn;
using LiXinModels.CourseManage;

namespace LiXinBLL.CourseLearn
{
    public class Co_CourseResourceBL : ICo_CourseResource
    {
        private  Co_CourseResourceDB coCourseResourceDb;
        public Co_CourseResourceBL()
        {
            coCourseResourceDb = new Co_CourseResourceDB();
        }


        public List<Co_CourseResource> GetList(int CourseId)
        {
            return coCourseResourceDb.GetList(CourseId);
        }

        public Co_CourseResource GetCo_CourseResource(int Id)
        {
            return coCourseResourceDb.GetCo_CourseResource(Id);
        }


        /// <summary>
        /// 根据课程编号 获取课程关联的资源及附件
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Co_CourseResource> GetCourseResourceList(int courseId)
        {
            return coCourseResourceDb.GetCourseResourceList(courseId);
        }

        /// <summary>
        /// 新增课程关联资源
        /// </summary>
        /// <param name="model"></param>
        public bool AddCourseResource(Co_CourseResource model)
        {
           return coCourseResourceDb.AddCourseResource(model);
        }

        /// <summary>
        /// 删除课程已经关联资源
        /// </summary>
        /// <param name="courseId"></param>
        public bool DeleteCourseResource(int courseId)
        {
            return coCourseResourceDb.DeleteCourseResource(courseId);
        }

        public List<Co_CourseResource> GetVedioList(int courseid,int Userid)
        {
            return coCourseResourceDb.GetVedioList(courseid, Userid);
        }

        public Co_CourseResource GetCo_CourseResourceByCourseId(int Id)
        {
            return coCourseResourceDb.GetCo_CourseResourceByCourseId(Id);
        }
    }
}
