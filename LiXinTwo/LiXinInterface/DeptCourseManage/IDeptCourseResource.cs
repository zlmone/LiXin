using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.DeptCourseManage;

namespace LiXinInterface.DeptCourseManage
{
    public interface IDeptCourseResource
    {
        /// <summary>
        /// 获得课程相关资源和附件
        /// </summary>
        /// <param name="Id">课程ID</param>
        /// <returns></returns>
        Dept_CourseResource GetCo_CourseResource(int Id);

        /// <summary>
        /// 根据课程编号 获取课程关联的资源及附件
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        List<Dept_CourseResource> GetCourseResourceList(int courseId);

        /// <summary>
        /// 新增课程关联资源
        /// </summary>
        /// <param name="model"></param>
        bool AddCourseResource(Dept_CourseResource model);

        /// <summary>
        /// 删除课程已经关联资源
        /// </summary>
        /// <param name="courseId"></param>
        bool DeleteCourseResource(int courseId);

        /// <summary>
        /// 获取课程下所有资源
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        List<Dept_CourseResource> GetList(int CourseId);


        /// <summary>
        /// 查找视频课程下的视频信息和进度条 
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        List<Dept_CourseResource> GetVedioList(int courseid, int userid);
    }
}
