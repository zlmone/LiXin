using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseManage;

namespace LiXinInterface.CourseManage
{
    public interface ICo_CoursePaper
    {
        bool AddCoursePaper(Co_CoursePaper model);

        bool IsExistCourseMainPaper(int courseId);

        bool UpdateCoursePaper(Co_CoursePaper model);

        Co_CoursePaper GetCo_CourseMainPaper(int CourseId);

        /// <summary>
        /// 修改次数
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        bool UpdateTestTimes(int CourseId);
    }
}
