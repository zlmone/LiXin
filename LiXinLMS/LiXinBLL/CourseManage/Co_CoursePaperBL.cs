using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.CourseManage;
using LiXinInterface.CourseManage;
using LiXinModels.CourseManage;

namespace LiXinBLL.CourseManage
{
    public class Co_CoursePaperBL : ICo_CoursePaper
    {
        private Co_CoursePaperDB coursepaperDb;
         public Co_CoursePaperBL()
        {
            coursepaperDb = new Co_CoursePaperDB();
        }


        public bool AddCoursePaper(Co_CoursePaper model)
        {
            return coursepaperDb.AddCoursePaper(model);
        }

        public bool IsExistCourseMainPaper(int courseId)
        {
            return coursepaperDb.IsExistCourseMainPaper(courseId);
        }

        public bool UpdateCoursePaper(Co_CoursePaper model)
        {
            return coursepaperDb.UpdateCourseMainPaper(model);
        }


        public Co_CoursePaper GetCo_CourseMainPaper(int CourseId)
        {
            return coursepaperDb.GetCo_CourseMainPaper(CourseId);
        }

        public bool UpdateTestTimes(int CourseId)
        {
            return coursepaperDb.UpdateTestTimes(CourseId);
        }
    }
}
