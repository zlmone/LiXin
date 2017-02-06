using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.DepPlanManage;
using LiXinModels.DeptCourseManage;
using LiXinModels.DepCourseManage;
using LiXinModels;

namespace LiXinInterface.DeptPlanManage
{
    public interface IDep_MonthPlan
    {

        /// <summary>
        /// 新增一条数据月度大纲
        /// </summary>     
        void InsertDep_MonthPlan(Dep_MonthPlan model);

        /// <summary>
        /// 新增一条数据月度大纲内的课程
        /// </summary>     
        void InsertDep_MonthPlanCourse(Dep_MonthPlanCourse model);

        /// <summary>
        /// 插入课程
        /// </summary>
        /// <param name="model"></param>
        void InsertDept_Course(Dep_Course model);

        /// <summary>
        /// 删除计划中的课程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseID"></param>
        void DeleteMonPlanCourse(int id, string courseID);

        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="courseIDs"></param>
        void DeleteCourse(string courseIDs);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        void UpdateMonthPlan(int id, string where);

        /// <summary>
        /// 获得单个的月度大纲
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Dep_MonthPlan Get(int id);

        /// <summary>
        /// 是否存在此月度计划，
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>true存在，false不存在</returns>
        bool IsExistMonplan(int year, string month, int id, int deptID = 0);

        /// <summary>
        /// 获取配置当中的年度设定
        /// </summary>
        /// <param name="year"></param>
        /// <returns>月份  形如 2013-5</returns>
        List<string> GetMonthOfConfig(Sys_ParamConfig list, int year);

        /// <summary>
        /// 获取计划的基本信息
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Dep_MonthPlan> GetMonthPlan(out int totalCount, int DeptID=0, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1",string deptwhere=" 1=1", string jsRenderSortField = "  month asc");

        /// <summary>
        /// 2013-9-22添加  被联合部门显示联合部门下的月度
        /// </summary>
        /// <param name="DeptID"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <param name="deptwhere"></param>
        /// <param name="beiwhere"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        List<Dep_MonthPlan> GetMonthPlanForMaoJiaYuan(out int totalCount, int DeptID = 0, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string deptwhere = " 1=1", string beiwhere = "1=1", string jsRenderSortField = "  month asc");
        /// <summary>
        /// 获取年度计划内的课程详情
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<LiXinModels.CourseShow> GetYearCourseDetails(out int totalCount, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1");

        /// <summary>
        /// 获取月度计划内的课程详情
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<LiXinModels.CourseShow> GetMonthCourseDetails(out int totalCount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " cc.month desc");

        /// <summary>
        /// 差异对比的头部信息 年度预计课程数，实际课程数等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Dep_MonthPlan GetPlanCourseCount(int id,int deptID, string where = " 1=1");

        /// <summary>
        /// 月度差异对比
        /// </summary>
        List<LiXinModels.CourseShow> GetMonPlanCompare(int year, string month, int DeptID);


        /// <summary>
        /// 删除月度计划以及下面的课程
        /// </summary>
        /// <param name="courseID"></param>
        void DeleteMonPlanAndCourse(int MonthId);


        /// <summary>
        /// 获取年份
        /// </summary>
        /// <returns></returns>
        List<int> GetYearOfPlan(int deptid);

    }
}
