using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.DepPlanManage;
using LiXinModels.DepCourseManage;
using LiXinModels.User;

namespace LiXinInterface.DeptPlanManage
{
    public interface IDep_YearPlan
    {
        #region 年计划

        /// <summary>
        /// 获得已经存在的部门ID
        /// </summary>
        string GetYearDepids(int year);

        /// <summary>
        /// 添加新的年计划
        /// </summary>
        int InsertYear(Dep_YearPlan model);

        /// <summary>
        /// 根据ID获取单个实体
        /// </summary>
        Dep_YearPlan GetYearModel(int id);

        /// <summary>
        /// 发布年度计划 查看是否有计划被发布
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        List<int> GetIsOrNoLinkDepart(int deptid, int year);

        /// <summary>
        /// A部门被联合后 在点发布 此时审批就不能在联合
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Dep_YearPlan GetPublicIsOrNoLinkDepart(int deptid, int id);

        /// <summary>
        /// 根据year获取单个实体
        /// </summary>
        Dep_YearPlan GetYearPlanByYear(int year);

        /// <summary>
        /// 根据ID删除单个实体(假删)
        /// </summary>
        bool DeleteYearModel(string ids);

        /// <summary>
        /// 修改年计划
        /// </summary>
        bool UpdateYearByID(Dep_YearPlan model);

        /// <summary>
        /// 获取年计划列表(分页)
        /// </summary>
        List<Dep_YearPlan> GetAllYearList(string deptIDs, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string strWhere = "1=1");

        /// <summary>
        /// 获取年计划列表
        /// </summary>
        List<Dep_YearPlan> GetAllYear(int depID, string strWhere = "1=1");

        /// <summary>
        /// 验证重名
        /// </summary>
        /// <returns></returns>
        bool Checkname(int year);

        /// <summary>
        /// 获得已经存在的年度
        /// </summary>
        List<int> GetAllYearID(int depID);

        /// <summary>
        /// 获得已经存在的年度
        /// </summary>
        List<int> GetYear(string where = " 1=1");

        /// <summary>
        /// 获得数据列表
        /// </summary>
        List<Dep_YearPlan> GetDepYearListByWhere(string where = "1=1");

        /// <summary>
        /// 获得被联合上报部门为0
        /// </summary>
        List<Dep_YearPlan> GetDepLink_ApprovalFlagList(int year, int deptid);

        /// <summary>
        /// 获得所有被联合上报部门为0
        /// </summary>
        List<Dep_YearPlan> GetDepLink_ApprovalFlagList(int deptid);

        /// <summary>
        /// 修改其他被联合上报部门为拒绝
        /// </summary>
        /// <returns></returns>
        bool UpdateDepLink_ApprovalFlag(int year, int deptid);

        /// <summary>
        /// 获取计划审批的列表
        /// </summary>
        List<Dep_YearPlan> GetLinkYearPlanList(string deptIDs, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string strWhere = "1=1");

        /// <summary>
        /// 年度计划跟踪-部门/分所
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="deptype">"in": 部门,"not in":分所</param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Dep_YearPlan> GetYearPlanTrackList(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string deptype = "in", string strWhere = "1=1", string jsRenderSortField = "tempTa.Year asc");

        /// <summary>
        /// 年度计划跟踪-部门/分所(未上报)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptype">0: 部门,1:分所</param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        List<Sys_Department> GetNoTrackList(int year, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, int deptype = 0, string strWhere = "1=1");

        #endregion

        #region 联合上报

        /// <summary>
        /// 添加
        /// </summary>
        int InsertDepLink(Dep_LinkDepart model);

        /// <summary>
        /// 获取单条信息
        /// </summary>
        Dep_LinkDepart GetDepLink(int yearID, int deptID);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        List<Dep_LinkDepart> GetDepLink(int deptID);

        /// <summary>
        /// 修改
        /// </summary>
        bool UpdateDepLink(Dep_LinkDepart model);

        /// <summary>
        /// 根据年度ID全部删除
        /// </summary>
        bool DeleteAllDepLinkbyYear(string YearIds);

        /// <summary>
        /// 发布年度计划修改联合上报部门为拒绝(联合发布)
        /// </summary>
        bool UpdateLinkandYearPlan(int year, int id);

        /// <summary>
        /// 发布年度计划修改联合上报部门为拒绝(单体发布)
        /// </summary>
        bool UpdateLinkDepart(int year, int dpetid);

        #endregion

        #region 年计划课程

        /// <summary>
        /// 添加年计划课程
        /// </summary>
        bool InsertYearCourse(Dep_YearPlanCourse model);

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
        /// <returns></returns>
        List<Dep_YearPlanCourse> GetAllYearCourseList(int YearId, string Order = "asc", string where = "1=1");

        /// <summary>
        /// 获取年计划课程列表(有分页)
        /// </summary>
        List<Dep_YearPlanCourse> GetAllYearCourseList(int YearId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1");

        /// <summary>
        /// 获取课程列表
        /// </summary>
        List<Dep_YearPlanCourse> GetCourseList(string ids);

        /// <summary>
        /// 根据ID获取课程
        /// </summary>
        Dep_YearPlanCourse GetCourse(int id);

        /// <summary>
        /// 根据ID删除全部课程
        /// </summary>
        bool DeleteCoursebyYear(int YearId, string delIds);

        /// <summary>
        /// 更新课程信息
        /// </summary>
        bool UpdateCo_Course(Dep_Course model);

        /// <summary>
        /// 判断年度计划下面是否删除讲师
        /// </summary>
        bool IsYearUser(int YearId);

        #endregion

        /// <summary>
        /// 联合上报 根据yearId 找出所有部门
        /// </summary>
        /// <param name="yearId"></param>
        /// <returns></returns>
        List<Dep_LinkDepart> GetDep_LinkDepartByYear(int yearId, int deptid);


        List<Sys_Department> GetALLLinkDepart(int deptid, int year);

        #region == 部门开课跟踪 ==

        /// <summary>
        /// 获取计划开课详情列表
        /// </summary>
        /// <param name="totalcount">总记录数</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="teacherName">讲师名称</param>
        /// <param name="openLevel">开放级别</param>
        /// <param name="isMust">选修\必修 0:必修；1：选修 ；-1：全部(复选框全不选)； -2：全部（复选框全选）</param>
        /// <param name="isSystem">框架内/外 0:外；1：内 ；-1：全部(复选框全不选)； -2：全部（复选框全选）</param>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门ID</param>
        /// <param name="isOpen">单体/联合上报 0：单体 1：联合 </param>
        /// <param name="where">条件</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        List<Dep_YearPlanCourse> GetDepPlanOpenYearCourseList(out int totalcount, string courseName, string teacherName,
                                                              string openLevel,
                                                              int isMust, int isSystem, int yearPlan = 0, string deptId = "",
                                                              int isOpen = 0, string where = "",
                                                              int startIndex = 1, int pageLength = int.MaxValue,
                                                              string orderBy = "");
        #endregion
    }
}
