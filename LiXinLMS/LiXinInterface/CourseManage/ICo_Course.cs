using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseManage;

namespace LiXinInterface.CourseManage
{
    public interface ICo_Course
    {
        /// <summary>
        /// 获取单个课程实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Co_Course GetCo_Course(int Id);
        /// <summary>
        /// 获取单个课程 并找出课程状态
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        Co_Course GetCo_Course(int Id, int userid);

        /// <summary>
        /// 验证课程编号是否重名
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="courseFrom">课程来源 0：年度计划；1：月度计划；2：课程管理</param>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool IsExistCourseCode(string courseCode, int courseFrom = 2, int Id = 0, int way = 1);

        /// <summary>
        /// 验证课程是否重名
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="courseFrom">课程来源 0：年度计划；1：月度计划；2：课程管理</param>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool IsExistCourseName(string courseName, int courseFrom = 2, int Id = 0);

        bool UpdateCourse(Co_Course model);

        /// <summary>
        /// 根据课程编号更新  同一课次中的课程的课次数 2013-3-21 9:33:42
        /// </summary>
        /// <param name="code"></param>
        /// <param name="courseTimes"></param>
        /// <returns></returns>
        bool UpdateCourseTimesByCode(string code, int courseTimes);
        /// <summary>
        /// 修改课程单一状态
        /// </summary>
        /// <param name="flag">0:删除 1:发布</param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        bool ModifySingleCourse(int flag, int courseId);

        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="model"></param>
        void AddCourse(Co_Course model);


        /// <summary>
        /// 获取课程列表 CPA课程 并读取是否已经录入成绩完毕
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Co_Course> GetCourseCPAListImportScore(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                              int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC");

        /// <summary>
        /// 获取课程列表 CPA课程
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Co_Course> GetCourseCPAList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                              int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC");

        /// <summary>
        /// 获取课程列表 视频课程和 CPA课程使用
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Co_Course> GetCourseVideoList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Id DESC");

        /// <summary>
        ///  获取课程列表 集中授课使用  包含课次数据
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Co_Course> GetCourseTogetherList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                              int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Id DESC");

        /// <summary>
        /// 获取课程列表 普通获取List方式
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Co_Course> GetCourseCommonList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                              int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC");

        /// <summary>
        /// 课程关联试卷 如果存在那么更新 否则 新增
        /// </summary>
        /// <param name="coursePaper"></param>
        /// <returns></returns>
        bool InsertOrUpdateCourseMainPaper(Co_CoursePaper coursePaper);

        /// <summary>
        /// 查看视频课程
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Co_Course> GetVideoCourseList(out int totalCount, string where = " 1 = 1 ", string TrainGrade = "",int Userid=0,int DeptId=0, string IsOpenSub="",int startIndex = 0,
                                           int pageLength = int.MaxValue,
                                           string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC");

        /// <summary>
        /// CPA课程
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Co_Course> GetCPACourseList(out int totalCount, string where = " 1 = 1 ", int userid = 0, int startIndex = 0,
                                         int pageLength = int.MaxValue,
                                         string orderBy = "");

        Co_Course GetVideoCo_CourseById(int Id);

        List<Co_Course> GetCourseIndexList(out int totalcount, string where = " 1 = 1 ", int startIndex = 1, int startLength = int.MaxValue, string order = "Id asc");

        List<Co_CourseShow> GetCourseAvg(out int totalcount, int CourseID, int startIndex = 1, int startLength = int.MaxValue, string order = "tab.UserID asc");
    }
}
