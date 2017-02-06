using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DeptCourseManage;
using LiXinInterface.DeptCourseManage;
using LiXinModels.DeptCourseManage;

namespace LiXinBLL.DeptCourseManage
{
    public class DeptCoursePaperBL : IDeptCoursePaper
    {
        private DeptCoursePaperDB coursepaperDb;
         public DeptCoursePaperBL()
        {
            coursepaperDb = new DeptCoursePaperDB();
        }

         /// <summary>
         /// 新增课程关联考试
         /// </summary>
         /// <param name="model"></param>
        public bool AddCoursePaper(Dept_CoursePaper model)
        {
            return coursepaperDb.AddCoursePaper(model);
        }

        /// <summary>
        /// 课程是否已经关联  主试卷
        /// </summary>
        /// <param name="courseId"></param>
        public bool IsExistCourseMainPaper(int courseId)
        {
            return coursepaperDb.IsExistCourseMainPaper(courseId);
        }

        /// <summary>
        /// 修改课程关联的 主试卷
        /// </summary>
        /// <param name="model"></param>
        public bool UpdateCoursePaper(Dept_CoursePaper model)
        {
            return coursepaperDb.UpdateCourseMainPaper(model);
        }


        /// <summary>
        /// 获取课程关联的 主试卷
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public Dept_CoursePaper GetCo_CourseMainPaper(int CourseId)
        {
            return coursepaperDb.GetCo_CourseMainPaper(CourseId);
        }

        /// <summary>
        /// 修改课程下考试次数
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public bool UpdateTestTimes(int CourseId)
        {
            return coursepaperDb.UpdateTestTimes(CourseId);
        }
    }
}
