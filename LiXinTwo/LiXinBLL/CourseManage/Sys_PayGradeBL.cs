using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess;
using LiXinInterface.CourseManage;
using LiXinModels.CourseManage;

namespace LiXinBLL.CourseManage
{
    public class Sys_PayGradeBL : ISys_PayGrade
    {
        private readonly Sys_PayGradeDB payGradeDB = new Sys_PayGradeDB();

        /// <summary>
        /// 添加薪酬级别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddSys_PayGrade(Sys_PayGrade model)
        {
            return payGradeDB.AddSys_PayGrade(model);
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        public List<Sys_PayGrade> GetSys_PayGradeList(string strWhere = "1=1")
        {
            return payGradeDB.GetSys_PayGradeList(strWhere);
        }

        /// <summary>
        ///  ADD薪酬级别关联体系
        /// </summary>
        /// <returns></returns>
        public int AddSys_SortLinkGrade(Sys_SortLinkGrade model)
        {
            return payGradeDB.AddSys_SortLinkGrade(model);
        }

        /// <summary>
        ///     获得薪酬级别 关联课程体系 数据列表
        /// </summary>
        public List<Sys_SortLinkGrade> GetSys_SortLinkGradeList(string strWhere = "1=1")
        {
            return payGradeDB.GetSys_SortLinkGradeList(strWhere);
        }

        /// <summary>
        /// 删除 课程体系关联 薪酬级别
        /// </summary>
        /// <param name="sortId"></param>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public bool DeleteSys_SortLinkGrade(int sortId, int gradeId, int courseId)
        {
            return payGradeDB.DeleteSys_SortLinkGrade(sortId, gradeId, courseId);
        }


        /// <summary>
        /// 删除 课程体系关联 薪酬级别 +关联课程
        /// </summary>
        /// <returns></returns>
        public bool DeleteSys_SortLinkGrade(int courseId)
        {
            return payGradeDB.DeleteSys_SortLinkGrade(courseId);
        }
        ////////////////////////////胜任力课程 分割线



        /// <summary>
        ///  ADD  课程Id关联  薪酬级别+课程体系 关联表主键
        /// </summary>
        /// <returns></returns>
        public int AddSys_SortGradeLinkCourse(Sys_SortGradeLinkCourse model)
        {
            return payGradeDB.AddSys_SortGradeLinkCourse(model);
        }

        /// <summary>
        /// 删除 胜任力课程关联 薪酬级别+课程体系 关联表主键
        /// </summary>
        /// <param name="sortId"></param>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public bool DeleteSys_SortGradeLinkCourse(int courseId, int SortGradeID)
        {
            return payGradeDB.DeleteSys_SortGradeLinkCourse(courseId, SortGradeID);
        }


        /// <summary>
        ///  ADD  胜任力课程
        /// </summary>
        /// <returns></returns>
        public int AddSys_SortGradeCourse(Sys_SortGradeCourse model)
        {
            return payGradeDB.AddSys_SortGradeCourse(model);
        }

        /// <summary>
        ///  Update  胜任力课程
        /// </summary>
        /// <returns></returns>
        public bool UpdateSys_SortGradeCourse(Sys_SortGradeCourse model)
        {
            return payGradeDB.UpdateSys_SortGradeCourse(model);
        }

        /// <summary>
        ///     获得胜任力课程 数据列表
        /// </summary>
        public List<Sys_SortGradeCourse> GetSys_SortGradeCourseList(string strWhere = "1=1")
        {
            return payGradeDB.GetSys_SortGradeCourseList(strWhere);
        }

        /// <summary>
        ///  Delete  胜任力课程
        /// </summary>
        /// <returns></returns>
        public bool DeleteSys_SortGradeCourse(int Id)
        {
            return payGradeDB.DeleteSys_SortGradeCourse(Id);
        }

        /// <summary>
        /// 判断体系下是否有课程
        /// </summary>
        /// <param name="sortId"></param>
        /// <returns></returns>
        public int CourseSystemContainCourseNum(int sortId)
        {
            return payGradeDB.CourseSystemContainCourseNum(sortId);
        }

        public int GetPayGradeByUserId(int userid)
        {
            return payGradeDB.GetPayGradeByUserId(userid);
        }

    }
}