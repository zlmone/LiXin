using LiXinModels.CourseManage;
using LiXinModels.DepCourseManage;
using LiXinModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.DepCourseManage
{
    public interface IDep_Course
    {
        /// <summary>
        /// 指定课程负责人
        /// </summary>
        /// <param name="id">课程ID</param>
        /// <param name="uid">负责人ID</param>
        /// <returns></returns>
        bool ModifyCourseMaster(int id, string uid);
        Dep_Course GetCo_Course(int Id);

        Dep_Course GetCo_Course(int Id, int userid);

        void AddCourse(Dep_Course model);

        bool UpdateCourseTimesByCode(string code, int courseTimes);


        bool InsertOrUpdateCourseMainPaper(Dep_Coursepaper coursePaper);

        bool UpdateCourse(Dep_Course model);

        /// <summary>
        /// 获取集中课程列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Dep_Course> GetCourseTogetherList(out int totalCount, int way = 1, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Id DESC");

        bool ModifySingleCourse(int flag, int courseId);

        /// <summary>
        /// （教育培训部）获得部门开放至全所审批列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="teacherName">讲师名称</param>
        /// <param name="openUserName">提交人</param>
        /// <param name="isOpenOthers">审批状态</param>
        /// <param name="startTime">开课时间-开始</param>
        /// <param name="endTime">开课时间-结束</param>
        /// <param name="where">条件语句</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按审批状态排序</param>
        /// <returns></returns>
        List<Dep_Course> DepCourseOpenOthersCheck(out int totalCount, string courseName, string teacherName,
                                                  string openUserName,
                                                  int isOpenOthers, string startTime, string endTime, string where = "",
                                                  int startIndex = 1, int pageLength = int.MaxValue, string orderBy = "");

        /// <summary>
        /// 验证课程编号是否重复
        /// </summary>
        /// <param name="courseCode"></param>
        /// <param name="courseFrom"></param>
        /// <param name="Id"></param>
        /// <param name="way"></param>
        /// <returns></returns>
        bool IsExistCourseCode(string courseCode, int courseFrom = 2, int Id = 0, int way = 1);

        /// <summary>
        /// 根据ID更新审批信息
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateApproval(Dep_Course model);





        /// <summary>
        /// 获取课程列表 普通获取List方式
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Dep_Course> GetCourseCommonList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY dep_Course.Code,dep_Course.Id DESC");



        /// <summary>
        /// （教育培训部）获得部门开课跟踪列表
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <param name="deptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <param name="totalCount">记录总数</param>
        /// <param name="where">条件语句"  and ... "</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按Id倒序排序</param>
        /// <returns></returns>
        List<Dep_Course> DepOpenCourseFollowingList(out int totalCount, string deptName, int deptFlag = 0,
                                                    string where = "",
                                                    int startIndex = 1, int pageLength = int.MaxValue,
                                                    string orderBy = "");

        /// <summary>
        /// 实际开课详情-（教育培训部）获得部门开放至全所列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="isMust">选修\必修 0:必修；1：选修 ；-1：全部(复选框全不选)； -2：全部（复选框全选）</param>
        /// <param name="isYearPlan">计划内\外 0：否；1：是 ； -1：全部(复选框全不选) ；-2：全部（复选框全选）</param>
        /// <param name="isOpenOthers">审批状态1：待审批；2：审批通过；3：审批拒绝 ；-1：待审批+通过+拒绝</param>
        /// <param name="startTime">开课时间-开始</param>
        /// <param name="endTime">开课时间-结束</param>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门</param>
        /// <param name="where">条件语句</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按审批状态排序</param>
        /// <returns></returns>
        List<Dep_Course> DepCourseOpenOthersList(out int totalCount, string courseName, int isMust, int isYearPlan,
                                                 int isOpenOthers, string startTime, string endTime, int yearPlan,
                                                 string deptId, string where = "",
                                                 int startIndex = 1, int pageLength = int.MaxValue, string orderBy = "");

        /// <summary>
        /// 实际开课详情-（教育培训部）获得部门自学列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="isYearPlan">计划内\外 0：否；1：是 ； -1：全部(复选框全不选) ；-2：全部（复选框全选）</param>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门</param>
        /// <param name="where">条件语句</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        List<Dep_Course> DepCourseLearnSelfList(out int totalCount, string courseName, int isYearPlan,
                                                int yearPlan, string deptId, string where = "",
                                                int startIndex = 1, int pageLength = int.MaxValue, string orderBy = "", int type = 0, string dwhere = " 1=1", int isYearStatus = 0);

        /// <summary>
        /// 获取单个课程实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Dep_Course GetCo_CourseAllList(int Id);


        /// <summary>
        /// 根据条件查询课程信息
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        List<Dep_Course> GetCourseIndexList(out int totalcount, string where = " 1 = 1 ", int startIndex = 1, int startLength = int.MaxValue, string order = "Id asc");

        List<Dep_Course> GetCourseAvg(out int totalcount, int CourseID, int startIndex = 1, int startLength = int.MaxValue, string order = "tab.UserID asc");

        #region == 部门指定查询 ==

        /// <summary>
        /// （部门负责人）获得部门指定查询列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="teacherName">讲师名称</param>
        /// <param name="isOrder">课程性质2-指定 3-兼有 -1:指定+兼有</param>
        /// <param name="courseStatus">课程状态0-未开始 1-进行中 2-已结束 -1:全部</param>
        ///  <param name="deptId">创建部门ID</param>
        /// <param name="where">条件语句‘and ....’</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按Id倒序排序</param>
        /// <returns></returns>
        List<Dep_Course> DepSelfCourseAppointSearch(out int totalCount, string courseName, string teacherName,
                                                    int isOrder = -1
                                                    , int courseStatus = -1, int deptId = 0, string where = "",
                                                    int startIndex = 1, int pageLength = int.MaxValue,
                                                    string orderBy = "");

        #endregion


        Dep_Course GetVideoCo_CourseById(int Id);

        /// <summary>
        /// 我开放的课程指定查询
        /// </summary>
        /// <returns></returns>
        List<Co_Course> GetMyOpenCourse(out int totalcount, int deptID, string where = " 1 = 1 ", int startIndex = 1, int pageLength = int.MaxValue, string jsRenderSortField = "Co_Course.Id desc");

        /// <summary>
        /// 查询课程指定的人员
        /// </summary>
        /// <param name="courseID">开放后的课程ID</param>
        /// <param name="deptCourseID">开放前的课程ID</param>
        /// <returns></returns>
        List<Sys_User> GetCanOrderList(out int totalcount, int courseID, int deptCourseID, string where = "1=1", int startIndex = 1, int pageLength = int.MaxValue, string jsRenderSortField = " DeptName desc");

        /// <summary>
        /// 用来取消指定
        /// </summary>
        /// <returns></returns>
        List<Sys_User> GetDropList(out int totalcount, int courseID, int deptCourseID, string where = "1=1", int startIndex = 1, int pageLength = int.MaxValue, string jsRenderSortField="");

        /// <summary>
        /// 获取纳入考核范围的所有课程信息
        /// </summary>
        /// <returns></returns>
        List<Dep_Course> GetDep_CourseList(int year);
    }
}
