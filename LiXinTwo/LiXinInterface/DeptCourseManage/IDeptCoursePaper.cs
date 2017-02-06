using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.DeptCourseManage;

namespace LiXinInterface.DeptCourseManage
{
    public interface IDeptCoursePaper
    {
        /// <summary>
        /// 新增课程关联考试
        /// </summary>
        /// <param name="model"></param>
        bool AddCoursePaper(Dept_CoursePaper model);

        /// <summary>
        /// 课程是否已经关联  主试卷
        /// </summary>
        /// <param name="courseId"></param>
        bool IsExistCourseMainPaper(int courseId);

        /// <summary>
        /// 修改课程关联的 主试卷
        /// </summary>
        /// <param name="model"></param>
        bool UpdateCoursePaper(Dept_CoursePaper model);

        /// <summary>
        /// 获取课程关联的 主试卷
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        Dept_CoursePaper GetCo_CourseMainPaper(int CourseId);

        /// <summary>
        /// 修改次数
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        bool UpdateTestTimes(int CourseId);
    }
}
