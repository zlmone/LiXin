using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.PlanManage;
using LiXinModels.CourseManage;

namespace LiXinInterface.PlanManage
{
    public interface ITr_YearPlan
    {
        #region 年计划

        /// <summary>
        /// 添加年计划
        /// </summary>
        int InsertYear(Tr_YearPlan model);

        /// <summary>
        /// 根据ID获取单个实体
        /// </summary>
        Tr_YearPlan GetYearModel(int id);

        /// <summary>
        /// 根据year获取单个实体
        /// </summary>
        Tr_YearPlan GetYearPlanByYear(int year);

        /// <summary>
        /// 根据ID删除单个实体(假删)
        /// </summary>
        bool DeleteYearModel(string ids);

        /// <summary>
        /// 修改年计划
        /// </summary>
        bool UpdateYearByID(Tr_YearPlan model);

        /// <summary>
        /// 获取年计划列表(分页)
        /// </summary>
        List<Tr_YearPlan> GetAllYearList(out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string strWhere = "1=1");
        /// <summary>
        /// 获取年计划列表
        /// </summary>
        List<Tr_YearPlan> GetAllYear(string strWhere = "1=1");

        /// <summary>
        /// 验证重名
        /// </summary>
        bool Checkname(int year);

        /// <summary>
        /// 获得最大年度
        /// </summary>
        List<int> GetAllYear();

        #endregion

        #region 年计划课程

        /// <summary>
        /// 添加年计划课程
        /// </summary>
        bool InsertYearCourse(Tr_YearPlanCourse model);

        /// <summary>
        /// 根据ID删除指定课程
        /// </summary>
        bool DeleteYearCourse(int YearId, string CourseIds);

        /// <summary>
        /// 根据ID删除全部课程
        /// </summary>
        bool DeleteAllYearCourse(string YearIds);

        /// <summary>
        /// 获取年计划课程列表(无分页)
        /// </summary>
        List<Tr_YearPlanCourse> GetAllYearCourseList(int YearId,string Order="asc",string where = "1=1");

        /// <summary>
        /// 获取年计划课程列表(有分页)
        /// </summary>
        List<Tr_YearPlanCourse> GetAllYearCourseList(int YearId,out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order="asc",string where = "1=1");

        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <returns></returns>
        List<Tr_YearPlanCourse> GetCourseList(string ids);

        /// <summary>
        /// 根据ID获取课程
        /// </summary>
        Tr_YearPlanCourse GetCourse(int id);

        /// <summary>
        /// 根据ID删除全部课程
        /// </summary>
        bool DeleteCoursebyYear(int YearId, string delIds);

        /// <summary>
        /// 更新课程信息
        /// </summary>
        bool UpdateCo_Course(Co_Course model);

        /// <summary>
        /// 判断年度计划下面是否删除讲师
        /// </summary>
        bool IsYearUser(int YearId);
        #endregion
    }
}
