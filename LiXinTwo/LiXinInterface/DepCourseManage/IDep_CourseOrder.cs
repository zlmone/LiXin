using LiXinModels.CourseLearn;
using LiXinModels.DepCourseManage;
using LiXinModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.User;

namespace LiXinInterface.DepCourseManage
{
    public partial interface IDep_CourseOrder
    {
        /// <summary>
        /// 新增一条数据
        /// </summary>
        /// <param name="model"></param>
        void InsertDep_CourseOrder(Dep_CourseOrder model);

        /// <summary>
        /// 我的课程(讲师)详细下学员列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId">课程ID</param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<Dep_CourseOrder> GetTeacherCourseUserList(out int totalCount, int CourseId, int startIndex = 0,
                                                              int pageLength = int.MaxValue);

        /// <summary>
        /// 我的课程(讲师)考勤学员列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<Dep_CourseOrder> GetTeacherCourseAttendceList(out int totalCount, int CourseId, int startIndex = 0,
                                                           int pageLength = int.MaxValue);

        /// <summary>
        /// 我的课程讲师端学员端列表(加入考试成绩)
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<Dep_CourseOrder> GetTeacherOnLineTest(out int totalCount, int CourseId, int startIndex = 0,
                                                   int pageLength = int.MaxValue);

        LiXinModels.DepCourseManage.Dep_Course GetCourseById(int courseId, int userId = 0);


        /// <summary>
        /// 得到一个model
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Dep_CourseOrder Get(int Id);



        /// <summary>
        /// 我的可预定页面
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userid"></param>
        /// <param name="DepartId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<Dep_Course> GetDepCourseList(out int totalCount, int userid, int DepartId, string where = " 1=1", int startIndex = 0,
                                                                              int pageLength = int.MaxValue, string orderBy = "ORDER BY  aa.CourseId", string orderWhere = " (tt.courseid is null or tt.OrderStatus!=1)");

        int GetYuDing(int courseid);

        /// <summary>
        /// 根据课程ID和用户ID 找出预定表信息
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Dep_CourseOrder GetDep_CourseOrderByCourseIdAndUserId(int CourseId, int UserId);

        bool UpdateDep_CourseOrder(Dep_CourseOrder model);


        /// <summary>
        /// 部门分所 已预定页面列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Dep_Course> GetMyCourseOrderList(out int totalCount, int userId,
                                                                                string where = " 1 = 1 ",
                                                                                int startIndex = 0,
                                                                                int pageLength = int.MaxValue,
                                                                                string orderBy =
                                                                                    "ORDER BY Dep_Course.id DESC", int apptime = 0);


        /// <summary>
        /// 查看个人总退订次数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        int GetAllCourseOrderTimes(int userid, int year = -1);


        /// <summary>
        /// 修改退订状态
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="OrderStatus"></param>
        /// <returns></returns>
        bool UpdateOrderStatus(int courseid, int userid, int OrderStatus);

        /// <summary>
        /// 请假功能
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Reason"></param>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        bool UpdateLeave(int id, string Reason, int courseid, int userid, Sys_User leader, double configHour);

        /// <summary>
        /// 课程预定查询列表
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        List<Dep_Course> GetCourseSubscribeList(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Id");

        /// <summary>
        /// 添加指定人员 
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="isAppoint"></param>
        /// <param name="userIds"></param>
        void AddSpecialCrowdUserToCourse(int courseId, int isAppoint, string userIds);


        /// <summary>
        /// 修改是否允许报名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status">1关闭  0开启</param>
        /// <returns></returns>
        bool UpdateOrderFlag(int id, int status);

        /// <summary>
        /// 获取报名成功的人员，根据培训级别分
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        List<NameSubscribeCount> GetSuccessTrainGradeSubscribe(int deptID, int courseID);

        /// <summary>
        /// 课程预定查询中能管理的所有人员
        /// </summary>
        /// <returns></returns>
        List<Sys_User> GetCourseSubscribeUserList(out int totalcount, int courseID, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " UserId");

        /// <summary>
        /// 判断是否存在该科目该学员的考勤记录
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool ExistDeptTran_courseOrder(int courseId, int userId);

        /// <summary>
        /// 插入考勤分数(带部门)
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="departId"></param>
        /// <param name="attScore"></param>
        /// <returns></returns>
        bool InsertDeptTran_courseOrder(int courseId, int userId, int departId, decimal attScore, decimal EvlutionScore = 0, decimal ExamScore = 0);

        /// <summary>
        /// 更新考勤分数
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="attScore"></param>
        /// <returns></returns>
        bool DepTran_CourseOrderUsers(int courseId, int userId, decimal attScore, int type, decimal EvlutionScore = 0, decimal ExamScore = 0);

        /// <summary>
        /// 获取预定人员
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="courseID"></param>
        /// <param name="where"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        List<Sys_User> GetDepCourseOrderName(out int totalCount, int courseID, string where = "1=1", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " su.UserId asc");


        #region == 部门课程指定查询 ==
        /// <summary>
        /// 查询课程中学员的预订情况
        /// </summary>
        List<Sys_User> GetDepCourseSubscribeUserList(out int totalCount, int courseId, string where = " 1 = 1 ",
                                                  int startIndex = 0, int pageLength = int.MaxValue,
                                                  string orderBy = " ORDER BY u.userId DESC ");

        /// <summary>
        /// 课程指定查询-添加课程【指定人员】
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <param name="isAppoint">0：自主报名 1：指定</param>
        /// <param name="userIds">学员Id</param>
        /// <param name="newAddUserIdList">新添加成功的人员id列表</param>
        void AddDepSpecialCrowdUserToCourse(int courseId, int isAppoint, string userIds, out List<string> newAddUserIdList);

        /// <summary>
        /// 撤销部门指定人员
        /// </summary>
        /// <param name="sqlstr"></param>
        void DepDropAssignUser(string sqlstr);
        #endregion


        Dep_CourseOrder GetCo_CourseMainPaperByCourseIdAndUserId(int CourseId, int UserId);

        /// <summary>
        /// 修改 条件自己加
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool UpdateWhere(string where);


        /// <summary>
        /// 部门报名明细
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        List<Sys_Department> GetDeptSubscribe(int courseId);

        /// <summary>
        /// 培训级别报名明细
        /// </summary>
        /// <param name="courseId"></param>
        List<NameSubscribeCount> GetTrainGradeSubscribe(int courseId);


        /// <summary>
        /// 学员端-我的考勤及学时
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Dep_CourseOrder> GetMyAttendceList(out int totalCount, int userId,
                                                                             string where = " 1 = 1 ",
                                                                             int startIndex = 0,
                                                                             int pageLength = int.MaxValue,
                                                                             string orderBy =
                                                                                 "ORDER BY Dep_Course.id DESC");


        /// <summary>
        /// 学员端-我的考试
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Dep_CourseOrder> GetMyExamList(out int totalCount, int userId,
                                                                             string where = " 1 = 1 ",
                                                                             int startIndex = 0,
                                                                             int pageLength = int.MaxValue,
                                                                             string orderBy =
                                                                                 "ORDER BY Dep_Course.id DESC");

        /// <summary>
        /// 查询我的授课的课程下报名成功的人员信息
        /// </summary>
        /// <returns></returns>
        List<Sys_User> GetCourseUserListByTeacher(out int totalcount, int courseID, int startIndex = 1, int startLength = int.MaxValue);

        /// <summary>
        /// 我的课程讲师端学员端列表(加入考试成绩)
        /// </summary>
        List<Dep_CourseOrder> GetCourseTeacherOnLineTest(out int totalCount, int CourseId, int startIndex = 1, int pageLength = int.MaxValue);

        /// <summary>
        /// 取消课程发布
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        bool CancelCoursePub(int courseid);

        /// <summary>
        /// 删除发布全所后预定的人员
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        void DeleteZhiDingUser(int courseid);
    }
}
