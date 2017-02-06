using LiXinModels;
using LiXinModels.DepCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.Report_Together
{
    public interface IReport_DepCourse
    {

        List<Report_DepCourse> GetReport_Complete(int ConfigValue, int year, string whereDeptID = "", string depwhere = " 1=1", int deptMaxScore = 0);


        List<Report_DepCourse> GetReport_Complete(int year, string deptid);

        /// <summary>
        /// 部门完成明细
        /// </summary>
        /// <returns></returns>
        List<Report_DepCourse> GetReport_CompleteDetail(string where, Sys_ParamConfig config, int year);

        /// <summary>
        /// 师资情况
        /// </summary>
        /// <returns></returns>
        List<Report_DepCourse> GetReport_Teachers(Sys_ParamConfig config, string where = "", int year = 2013);

        /// <summary>
        /// 查看薪酬级别
        /// </summary>
        /// <returns></returns>
        List<Report_DepCourse> GetPayGrade(string where);


        List<Report_DepCourse> GetSingleCoursePation(int courseid, string where, string dwhere = "1=1", string StartTime = "", string EndTime = "");



        List<Report_DepCourse> GetDep_Course_DeptNumFor_ShiJi(int yearid, string deptIDWhere = " 1=1");

        List<Report_DepCourse> GetDep_Course_DeptNumFor_YearPlan(int yearid, string deptIDWhere = " 1=1");

        /// <summary>
        /// 总所实际开设课程学时
        /// </summary>
        /// <param name="yearid"></param>
        /// <returns></returns>
        List<Report_DepCourse> GetDep_Course_DeptNumFor_ShiJi_Length(int yearid, string deptIDWhere = " 1=1");

        /// <summary>
        /// 总所计划开设课程学时数分布图
        /// </summary>
        /// <param name="yearid"></param>
        /// <returns></returns>
        List<Report_DepCourse> GetDep_Course_DeptNumFor_YearPlan_Length(int yearid, string deptIDWhere = " 1=1");

        /// <summary>
        /// 讲师上课的课程
        /// </summary>
        /// <returns></returns>
        List<Dep_Course> GetDepCourseSurvey(int teacher, string where = " 1=1");


    }
}
