using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.DepCourseManage;

namespace LiXinInterface.DepCourseManage
{
    public interface IDep_CourseDept
    {
        /// <summary>
        /// 新增考勤审核关联
        /// </summary>
        int AddCourseDept(Dep_CourseDept model);

        /// <summary>
        /// 修改考勤审核关联
        /// </summary>
        bool UpdateCourseDept(Dep_CourseDept model);

        /// <summary>
        /// 获取单条考勤审核关联信息
        /// </summary>
        Dep_CourseDept GetCourseDept(int courseID, int departId);

        /// <summary>
        /// 获得邮件短信发送相关信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        Dep_CourseDept CourseName(int courseId, int departId);

        /// <summary>
        /// 获得邮件短信被发送人的信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        Dep_CourseDept UserNmae(int courseId, int departId);

        /// <summary>
        /// 获取单条考勤信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        Dep_CourseDept GetCourseOpen(int courseID, int departId);

        /// <summary>
        /// 更新审核内容
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <param name="attFlag"></param>
        /// <param name="approvalFlag"></param>
        /// <returns></returns>
        bool UpDateDepTran_CourseOpen(int courseId, int departId, int attFlag, int approvalFlag);
    }
}
