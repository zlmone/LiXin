using LiXinDataAccess.NewCourseManage;
using LiXinInterface.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.NewCourseManage;

namespace LiXinBLL.NewCourseManage
{
    public class New_CoursePaperBL : INew_CoursePaper
    {
        protected New_CoursePaperDB CoursePaperDB = new New_CoursePaperDB();

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        public void AddCoursePaper(New_CoursePaper model)
        {
             CoursePaperDB.AddCoursePaper(model);
        }

        /// <summary>
        /// 根据课程获取关联的试卷
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <returns>执行结果</returns>
        public List<New_CoursePaper> GetCoursePaper(int courseID)
        {
            return CoursePaperDB.GetCoursePaper(courseID);
        }

        /// <summary>
        /// 根据课程ID删除课程试卷
        /// </summary>
        /// <param name="courseID">课程ID</param>
        public bool DeleteCoursePaper(int courseID)
        {
            return CoursePaperDB.DeleteCoursePaper(courseID);
        }

        /// <summary>
        /// 获取单个考试信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public New_CoursePaper GetSingleCoursePaper(int courseID)
        {
            return CoursePaperDB.GetSingleCoursePaper(courseID);
        }
    }
}
