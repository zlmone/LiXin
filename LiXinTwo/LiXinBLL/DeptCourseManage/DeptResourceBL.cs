using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DeptCourseManage;
using LiXinInterface.DeptCourseManage;
using LiXinModels.DeptCourseManage;

namespace LiXinBLL.DeptCourseManage
{
    public class DeptResourceBL : IDeptCourseResource
    {
        private  DeptCourseResourceDB coCourseResourceDb;
        public DeptResourceBL()
        {
            coCourseResourceDb = new DeptCourseResourceDB();
        }

        /// <summary>
        /// 获取课程下所有资源
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public List<Dept_CourseResource> GetList(int CourseId)
        {
            return coCourseResourceDb.GetList(CourseId);
        }

        /// <summary>
        /// 获得课程相关资源和附件
        /// </summary>
        /// <param name="Id">课程ID</param>
        /// <returns></returns>
        public Dept_CourseResource GetCo_CourseResource(int Id)
        {
            return coCourseResourceDb.GetCo_CourseResource(Id);
        }

        /// <summary>
        /// 根据课程编号 获取课程关联的资源及附件
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Dept_CourseResource> GetCourseResourceList(int courseId)
        {
            return coCourseResourceDb.GetCourseResourceList(courseId);
        }

        /// <summary>
        /// 新增课程关联资源
        /// </summary>
        /// <param name="model"></param>
        public bool AddCourseResource(Dept_CourseResource model)
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

        /// <summary>
        /// 查找视频课程下的视频信息和进度条 
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public List<Dept_CourseResource> GetVedioList(int courseid, int Userid)
        {
            return coCourseResourceDb.GetVedioList(courseid, Userid);
        }
    }
}
