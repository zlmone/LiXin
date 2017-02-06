using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.NewCourseManage
{
    public interface INew_CourseFiles
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        void AddCourseFiles(New_CourseFiles model);

        /// <summary>
        /// 获取单个信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        New_CourseFiles GetSingleCourseFile(int courseID, string wherestr = "1=1");

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        bool UpdateCourseFiles(New_CourseFiles model);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">ID</param>
        /// <returns>操作状态</returns>
        bool DeleteCourseFiles(string Ids);

         /// <summary>
        /// 根据课程ID找出资源
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        List<New_CourseFiles> GetCourseResourceList(int courseId);


        List<New_CourseFiles> GetVedioList(int courseid, int Userid);


        bool UpdateLoadTimes(int id);
    }
}
