using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;
using LiXinModels.PlanManage;
using LiXinModels.CourseManage;

namespace LiXinInterface
{
    public interface ITr_MonthPlan
    {

        /// <summary>
        /// 是否存在此月度计划，
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>true存在，false不存在</returns>
        bool IsExistMonplan(int year, string month, int id);

        /// <summary>
        /// 获取计划的基本信息
        /// </summary>
        List<Tr_MonthPlan> GetMonthPlan(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, int year = -1, string month = "-1", int publish = -1, string query = "", string jsRenderSortField = "  month asc");

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id"></param>
        void Publish(int id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// 获取年份
        /// </summary>
        /// <returns></returns>
        List<int> GetYearOfPlan();

        /// <summary>
        /// 获取年度计划内的课程详情
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<CourseShow> GetYearCourseDetails(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string openlevel = "", string month = "", int year = -1, string courseName = "");

        /// <summary>
        /// 获取月度计划内的课程详情
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<CourseShow> GetMonthCourseDetails(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, int id = -1, string jsRenderSortField = " cc.month desc",string where=" 1=1");

        /// <summary>
        /// 插入月度数据库
        /// </summary>
        /// <param name="model"></param>
        void InsertTr_MonthPlan(Tr_MonthPlan model);

        /// <summary>
        /// 插入月度计划和课程的关联
        /// </summary>
        /// <param name="model"></param>
        void InsertTr_MonthPlanCourse(Tr_MonthPlanCourse model);


        /// <summary>
        /// 插入课程
        /// </summary>
        /// <param name="model"></param>
        void InsertCo_Course(Co_Course model);


        /// <summary>
        /// 删除计划中的课程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseID"></param>
        void DeleteMonPlanCourse(int id, string courseID);


        /// <summary>
        /// 月度差异对比
        /// </summary>
        List<CourseShow> GetMonPlanCompare(int year, string month);

        /// <summary>
        /// 差异对比的头部信息 年度预计课程数，实际课程数等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Tr_MonthPlan GetPlanCourseCount(int id);

        /// <summary>
        /// 专为框架内外准备的数据
        /// </summary>
        /// <returns></returns>
        Dictionary<int, int> GetYearCourse(int year, string courseID);

        /// <summary>
        /// 获取配置当中的年度设定
        /// </summary>
        /// <param name="year"></param>
        /// <returns>月份  形如 2013-5</returns>
        List<string> GetMonthOfConfig(Sys_ParamConfig list, int year);

        /// <summary>
        /// 获得课程
        /// </summary>
        /// <param name="CourseID"></param>
        /// <returns></returns>
        CourseShow GetSingleCourse(int CourseID);

        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="courseIDs"></param>
        void DeleteCourse(string courseIDs);

        /// <summary>
        /// 我的培训计划 月度
        /// </summary>
        /// <returns></returns>
        List<CourseShow> GetMyMonthPlan(out int totalcount, int userID, int startIndex = 1, int startLength = int.MaxValue, string where = " 1=1", string jsRenderSortField = " PreCourseTime desc");

        /// <summary>
        /// 我的培训计划 年度
        /// </summary>
        /// <returns></returns>
        List<CourseShow> GetMyYearPlan(out int totalcount, int userID, int startIndex = 1, int startLength = int.MaxValue, string where = " 1=1 ", string jsRenderSortField = " Month desc");


        /// <summary>
        /// 根据配置文件和当前时间，获取本年度的开始时间和结束时间
        /// 如（startTime = 2012-10-01 0:00:00 ; endTime = 2012-12-31 23:59:59 ）
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="yearConfig"></param>
        void GetYearStartAndEnd(out DateTime startTime, out DateTime endTime, Sys_ParamConfig yearConfig, int year);

        /// <summary>
        /// 根据配置文件和当前时间，获取本年度的开始时间和结束时间
        /// 如（startTime = 2012-10-01 0:00:00 ; endTime = 2012-12-31 23:59:59 ）
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="yearConfig"></param>
        void YearPlanStartAndEnd(out DateTime startTime, out DateTime endTime, Sys_ParamConfig yearConfig, int year);

        /// <summary>
        /// 根据配置文件和当前时间，获取CPA周期
        /// 如（2012年10月—2013年12月）
        /// </summary>
        List<string> CPAStartAndEnd(Sys_ParamConfig CPAConfig);

        Tr_MonthPlan Get(int id);

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id"></param>
        void PublishAndCourse(int id);
    }
}
