using System.Collections.Generic;
using LiXinModels.CourseLearn;
using System;
using LiXinModels.CourseManage;
using LiXinModels.User;
using LiXinModels;
using LiXinModels.Report_zVedio;

namespace LiXinInterface.CourseLearn
{
    public interface ICourseOrder
    {
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Update(Cl_CourseOrder model);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        void Add(Cl_CourseOrder model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);


        /// <summary>
        /// 得到一个model
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Cl_CourseOrder Get(int Id);

        Cl_TimeOutOrder GetOneTimeOutOrder(int id);

        /// <summary>
        /// 逾时审核
        /// </summary>
        /// <param name="model"></param>
        void UpdateTimeOutOrderStatus(Cl_TimeOutOrder model);

        /// <summary>
        /// 根据条件获取列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        List<Cl_CourseOrder> GetList(string strWhere = " 1 = 1 ");

        List<Cl_CourseOrder> GetList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY id DESC");

        /// <summary>
        /// 请假
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool UpdateLeave(int id, string memo, string leaderID, double timespan, string hrId, string name);

        /// <summary>
        /// 退订
        /// 0：退订成功（根本没有预订）
        /// 1：退订成功
        /// 2：退订失败，次数超了
        /// 3：退订失败
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int UpdateStatus(int id, int userId, DateTime start, DateTime end, int unsubscribeCount);

        /// <summary>
        /// 退订
        /// 0：退订成功（根本没有预订）
        /// 1：退订成功
        /// 2：退订失败，次数超了
        /// 3：退订失败
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int UpdateStatus(out int num, int id, int userId, int year, int unsubscribeCount);

        /// <summary>
        /// 0:不可报名
        /// 1:可直接报名
        /// 2:可报名，需排队 
        /// 3:报名失败
        /// 4:不可报名，排队已关闭
        /// 5:不可报名，报名已关闭
        /// </summary>
        int GetCanSignup(out int num, int courseId, int userId, double timespan);

        /// <summary>
        /// 课程【特殊人群/指定】报名
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="courseStartTime">课程开始时间</param>
        /// <param name="courseEndTime">课程结束时间</param>
        /// <param name="isAppoint">1：部门指定；2：总所指定</param>
        /// <param name="userIds">学员Id</param>
        /// <param name="timespan">排队状态更改为正常预定状态的时间点</param>
        void AddSpecialCrowdUserToCourse(int courseId, string courseName, DateTime courseStartTime, DateTime courseEndTime, int isAppoint, string userIds, double timespan);

        /// <summary>
        /// 0:不可报名
        /// 1:可直接报名
        /// 2:不可报名，人数已满
        /// 3:报名失败
        /// </summary>
        int GetCanSignupCPA(int courseId, int userId);

        /// <summary>
        /// 提交逾时申请
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="applymemo"></param>
        /// <returns></returns>
        int SubmitTimeOutApply(string ids, string applymemo);

        /// <summary>
        /// 逾时申请审批
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="applymemo"></param>
        /// <returns></returns>
        int SubmitTimeOutApproval(string ids, string approvalmemo, int approval);

        /// <summary>
        /// 通过Id获取要逾时申请的明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Cl_CourseOrder GetTimeOutApplyDetail(int id);

        /// <summary>
        /// 我的请假申请
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Cl_CourseOrder> GetMyLeaveList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_CourseOrder.id DESC");

        /// <summary>
        /// 查询CPA课程中学员的预订情况
        /// </summary>
        /// <returns></returns>
        List<LiXinModels.User.Sys_User> GetCPACourseSubscribeUserList(out int totalCount, int courseId,
                                                                             string where = " 1 = 1 ",
                                                                             int startIndex = 0,
                                                                             int pageLength = int.MaxValue,
                                                                             string orderBy =
                                                                                 "ORDER BY su.userId DESC");

        /// <summary>
        /// 我的补预订申请
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Cl_CourseOrder> GetMyComplementResList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                     int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_MakeUpOrder.id DESC");
        /// <summary>
        /// 我的逾时申请
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Cl_CourseOrder> GetMyTimeOutList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                     int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_TimeOutOrder.id DESC");

        /// <summary>
        /// 逾时审核
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Cl_CourseOrder> GetTimeOutApplyApprovalList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_TimeOutOrder.id DESC");

        /// <summary>
        /// 逾时审核申请
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Cl_CourseOrder> GetTimeOutApplyList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_MakeUpOrder.id DESC");

        /// <summary>
        /// 逾时申请审核
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Cl_CourseOrder> GetTimeOutApprovalList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_MakeUpOrder.id DESC");

        /// <summary>
        /// 查询CPA课程的报名状态
        /// </summary>
        /// <returns></returns>
        List<LiXinModels.CourseManage.Co_Course> GetCpaCourseSubscribeList(out int totalCount,
                                                                                  string where = " 1 = 1 ",
                                                                                  int startIndex = 0,
                                                                                  int pageLength = int.MaxValue,
                                                                                  string orderBy =
                                                                                      "ORDER BY Co_Course.id DESC");




        List<LiXinModels.CourseManage.Co_Course> GetMyCourseList(out int totalCount, int userId, string where = " 1 = 1 ", string num = "1=1", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.id DESC");

        List<LiXinModels.CourseManage.Co_Course> GetMyCourseCPAList(out int totalCount, int userId, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.id DESC");

        List<LiXinModels.CourseManage.Co_Course> GetCourseSubscribeList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.id DESC");


        List<LiXinModels.CourseManage.Co_Course> GetCourseDeptSubscribeList(out int totalCount, string leaderId, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.id DESC");

        List<LiXinModels.CourseManage.Co_Course> GetMyCourseListCourse(out int totalCount, int userId,
                                                                       string where = " 1 = 1 ", int startIndex = 0,
                                                                       int pageLength = int.MaxValue,
                                                                       string orderBy = "ORDER BY Co_Course.id DESC");

        /// <summary>
        /// 我的课程表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<LiXinModels.CourseManage.Co_Course> GetMyCourseShedule(int userId, string where = " 1 = 1 ",
                                                                           string orderBy =
                                                                               " ORDER BY Co_Course.id DESC");

        /// <summary>
        /// 查询培训级别下CPA课程的报名情况
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <returns></returns>
        List<NameSubscribeCount> GetTrainGradeCPASubscribe(int courseID);
        bool UpdateDucueFlag(int id, int status);

        bool UpdateOrderFlag(int id, int status);

        LiXinModels.CourseManage.Co_Course GetCourseById(int courseId, int userId = 0);

        List<LiXinModels.User.Sys_Department> GetDeptSubscribe(int courseId);

        /// <summary>
        /// CPA课程报名培训级别明细
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        List<NameSubscribeCount> GetCpaTrainGradeSubscribe(int courseId);

        List<NameSubscribeCount> GetTrainGradeSubscribe(int courseId);

        List<NameSubscribeCount> GetSuccessTrainGradeSubscribe(int courseId);

        List<LiXinModels.User.Sys_User> GetCourseSubscribeUserList(out int totalCount, int courseId, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Sys_User.userId DESC");


        List<LiXinModels.CourseManage.Co_Course> GetListByTeacher(out int totalCount, int teacher,
                                                                         string where = " 1 = 1 ",
                                                                         int startIndex = 0,
                                                                         int pageLength = int.MaxValue,
                                                                         string orderBy = "ORDER BY Co_Course.id DESC");
        List<LiXinModels.CourseManage.Co_Course> GetListByAllTeacher(out int totalCount,
                                                                        string where = " 1 = 1 ", int startIndex = 0,
                                                                        int pageLength = int.MaxValue,
                                                                        string orderBy = "ORDER BY Co_Course.id DESC");

        /// <summary>
        /// 获取老师课程下的人员
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        List<Cl_CourseOrder> GetTeacherCourse(out int totalCount, int CourseId, int startIndex = 0, int pageLength = int.MaxValue);


        List<Cl_CourseOrder> GetTeacherCourseUserList(out int totalCount, int CourseId, int startIndex = 0, int pageLength = int.MaxValue);

        /// <summary>
        /// 获取讲师端下考试人员信息
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        List<Cl_CourseOrder> GetTeacherOnLineTest(out int totalCount, int CourseId, int TeacherId, int startIndex = 0,
                                                  int pageLength = int.MaxValue);

        List<Cl_CourseOrder> GetTeacherCourseAttendceList(out int totalCount, int CourseId, int startIndex = 0,
                                                          int pageLength = int.MaxValue);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="UserId">学员ID</param>
        /// <param name="GetScore">学时</param>
        /// <param name="PassStatus">0：学习记录；1：通过；2：不通过；</param>
        /// <param name="LearnStatus">0：未开始；1：进行中；2：已完成</param>
        /// <returns></returns>
        bool UpdateGetScore(int courseId, int UserId, decimal GetScore, int PassStatus, int LearnStatus);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <param name="userid">学员ID</param>
        /// <param name="PassStatus">0：学习记录；1：通过；2：不通过；</param>
        /// <returns></returns>
        bool UpdatePassStatus(int courseid, int userid, int PassStatus);

        /// <summary>
        /// 修改所有课程中的排队状态
        /// </summary>
        /// <param name="lastTime"></param>
        List<Cl_CourseOrder> UpdateCourseOrderStatus(DateTime lastTime);

        /// <summary>
        /// 修改本课程中的排队状态
        /// </summary>
        /// <param name="id"></param>
        List<Cl_CourseOrder> UpdateCourseOrderStatusByCourseId(int id);

        /// <summary>
        /// 当第5次的时候 修改课程状态 2:已完成
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="UserId"></param>
        /// <param name="LearnStatus"></param>
        /// <returns></returns>
        bool UpdateLearnStatus(int courseId, int UserId, int LearnStatus = 0);

        List<Cl_CourseOrder> GetMyTimeOutListLeave(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                   int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_TimeOutOrder.id DESC");

        /// <summary>
        /// 撤销部门指定人员
        /// </summary>
        /// <param name="sqlstr"></param>
        void DropAssignUser(string sqlstr);

        /// <summary>
        /// 获取集中授课中报名的人员
        /// </summary>
        List<Sys_User> GetCourseOrderName(out int totalCount, int courseID, string where = "1=1", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " su.UserId asc");


        /// <summary>
        /// 获得集中课程视频转播列表
        /// </summary>
        /// <param name="totalCount">总记录数</param>
        /// <param name="userId">当前用户ID</param>
        /// <param name="teacherName">讲师</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="isMust">是否必修:0-必修 1-选修</param>
        /// <param name="learnStatus">课程状态0-未开始，1-进行中,2-已结束</param>
        /// <param name="sort">所有课程类型1-内部培训 2-社会招聘 3-新进员工 4-实习生</param>
        /// <param name="subscribeType">预订状态 0-不可预订 1-未预订</param>
        /// <param name="startTime">开课开始时间-begin</param>
        /// <param name="endTime">开课开始时间-end</param>
        /// <param name="where">条件语句</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        List<Co_Course> GetMyBroadcastList(out int totalCount, int userId,
                                                  string teacherName, string courseName, int isMust, int learnStatus,
                                                  int sort,
                                                  int subscribeType, string startTime, string endTime,
                                                  string where = "", int startIndex = 1, int pageLength = int.MaxValue,
                                                  string orderBy = "");

        /// <summary>
        /// 根据用户ID 找出集中，视频，CPA课程获得学时
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        List<Cl_CourseOrder> GetCourseLengthByUserId(int userid);

        /// <summary>
        /// 根据用户ID 找出部门自学课程学时
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        List<Cl_CourseOrder> GetDepCourseLengthByUserId(int userid);


        /// <summary>
        /// 个人算学时-我的报表
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <returns></returns>
        string GetUserScoreToMyReport(int uid, int year, Sys_ParamConfig cpazq, bool flag = true);

        /// <summary>
        /// 判断是否学过CPA职业道德课程
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        int GetCpaWithIsMust(int userid, string year);


        /// <summary>
        /// 第2期-完成情况明细表
        /// </summary>
        /// <returns></returns>
        List<Cl_CourseOrder> GetCompletionDetial(string where = " 1=1", string timewhere = " 1=1");


        List<Cl_CourseOrder> GetDepartment(string name);

    
        List<Cl_CourseOrder> GetAttendByCompletionDetial();
    }
}