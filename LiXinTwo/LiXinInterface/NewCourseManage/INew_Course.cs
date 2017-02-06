using LiXinModels.NewCourseManage;
using System.Collections.Generic;

namespace LiXinInterface.NewCourseManage
{
    public interface INew_Course 
    {
        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<New_Course> GetNew_CourseList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "ORDER BY New_Course.Id DESC");
        /// <summary>
        /// 获取考勤列表(课程管理)
        /// </summary>
        /// <param name="way">课程类型</param>
        /// <param name="totalcount"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<New_Course> GetNewCourseList(int way, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string Order = "LastUpdateTime desc", string where = "1=1");


        /// <summary>
        /// 综合评估页面列表方法
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<New_Course> GetNewAllScoreManager(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                           int pageLength = int.MaxValue, string orderBy = "ORDER BY New_Course.Id DESC");

        /// <summary>
        /// 获取单个课程信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        New_Course GetSingleCourse(int courseID, string wherestr = "1=1");

        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="model"></param>
        void AddCourse(New_Course model);

        /// <summary>
        /// 修改课程
        /// </summary>
        /// <param name="model">课程信息</param>
        bool UpdateCourse(New_Course model);

        /// <summary>
        /// 验证课程编号是否重名
        /// </summary>
        /// <param name="courseCode"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool IsExistCourseCode(string courseCode, int Id = 0, int way = 1);


        /// <summary>
        /// 根据课程ID 找出教师 教室
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        New_Course GetCourseByCourseRoomRule(int courseID);

        /// <summary>
        /// 讲师端-获取学员对我的评价列表(add by yxt 2013-07-06)
        /// <param name="totalCount">总数</param>
        /// <param name="where">条件语句格式" and ..."</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// </summary>
        List<New_CourseRoomRule> GetPingByUserToTeacherList(out int totalCount, string where = "", int startIndex = 1,
                                                    int startLength = int.MaxValue, string orderBy = "");



        List<New_Course> GetStudyList(out int totalCount, int userid, string where = " 1=1", int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "ORDER BY New_Course.Id DESC");
    }

}
