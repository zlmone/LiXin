using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.NewCourseManage;

namespace LiXinInterface.NewCourseManage
{
    public interface INew_CoursePaper
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        void AddCoursePaper(New_CoursePaper model);
        /// <summary>
        /// 根据课程获取关联的试卷
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <returns>执行结果</returns>
        List<New_CoursePaper> GetCoursePaper(int courseID);

        /// <summary>
        /// 根据课程ID删除课程试卷
        /// </summary>
        /// <param name="courseID">课程ID</param>
        bool DeleteCoursePaper(int courseID);

        /// <summary>
        /// 获取单个试卷信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        New_CoursePaper GetSingleCoursePaper(int courseID);
    }
}
