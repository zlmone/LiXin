using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseManage;

namespace LiXinInterface.CourseManage
{
    public interface ISys_PayGrade
    {
        /// <summary>
        /// 添加薪酬级别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddSys_PayGrade(Sys_PayGrade model);

        /// <summary>
        ///     获得数据列表
        /// </summary>
        List<Sys_PayGrade> GetSys_PayGradeList(string strWhere = "1=1");

        /// <summary>
        ///  ADD薪酬级别关联体系
        /// </summary>
        /// <returns></returns>
        int AddSys_SortLinkGrade(Sys_SortLinkGrade model);

        /// <summary>
        ///     获得薪酬级别 关联课程体系 数据列表
        /// </summary>
        List<Sys_SortLinkGrade> GetSys_SortLinkGradeList(string strWhere = "1=1");

        /// <summary>
        /// 删除 课程体系关联 薪酬级别
        /// </summary>
        /// <param name="sortId"></param>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        bool DeleteSys_SortLinkGrade(int sortId, int gradeId, int courseId);

        /// <summary>
        /// 删除 课程体系关联 薪酬级别 +关联课程
        /// </summary>
        /// <returns></returns>
        bool DeleteSys_SortLinkGrade(int courseId);

        ////////////////////////////胜任力课程 分割线



        /// <summary>
        ///  ADD  课程Id关联  薪酬级别+课程体系 关联表主键
        /// </summary>
        /// <returns></returns>
        int AddSys_SortGradeLinkCourse(Sys_SortGradeLinkCourse model);

        /// <summary>
        /// 删除 胜任力课程关联 薪酬级别+课程体系 关联表主键
        /// </summary>
        /// <param name="sortId"></param>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        bool DeleteSys_SortGradeLinkCourse(int courseId, int SortGradeID);


        /// <summary>
        ///  ADD  胜任力课程
        /// </summary>
        /// <returns></returns>
        int AddSys_SortGradeCourse(Sys_SortGradeCourse model);
        /// <summary>
        ///  Update  胜任力课程
        /// </summary>
        /// <returns></returns>
        bool UpdateSys_SortGradeCourse(Sys_SortGradeCourse model);
        /// <summary>
        ///     获得胜任力课程 数据列表
        /// </summary>
        List<Sys_SortGradeCourse> GetSys_SortGradeCourseList(string strWhere = "1=1");

        /// <summary>
        ///  Delete  胜任力课程
        /// </summary>
        /// <returns></returns>
        bool DeleteSys_SortGradeCourse(int Id);
        /// <summary>
        /// 判断体系下是否有课程
        /// </summary>
        /// <param name="sortId"></param>
        /// <returns></returns>
        int CourseSystemContainCourseNum(int sortId);


        /// <summary>
        /// 根据用户ID找到对应薪酬级别ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        int GetPayGradeByUserId(int userid);
    }
}
