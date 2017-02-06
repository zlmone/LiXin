using LiXinModels.CourseManage;
using LiXinModels.DepTranManage;
using LiXinModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.DepTranManage
{
    public interface IDepTran_CourseOrder
    {
        /// <summary>
        /// 根据条件查出信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Co_Course> GetCourseList(int userid, string where = " 1=1");


        bool AddDepTran_CourseOrder(DepTran_CourseOrder model);

        bool UpdateDepTran_CourseOrder(DepTran_CourseOrder model);

        DepTran_CourseOrder GetCo_CourseMainPaperByCourseIdAndUserId(int CourseId, int UserId);

        /// <summary>
        /// 查看个人所有总退订次数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        int GetAllCourseOrderTimes(int userid);


        DepTran_CourseOrder Get(int Id);

        bool Delete(int id);

        /// <summary>
        /// 预定方法
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        int GetYuDing(int courseid,int deptid);

        /// <summary>
        /// 修改预定状态
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="OrderStatus"></param>
        /// <returns></returns>
        bool UpdateOrderStatus(int courseid, int userid, int OrderStatus);



        /// <summary>
        /// 绑定我的预定课程信息
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Co_Course> GetMyCourseOrderList(out int totalCount, int userId, int deptid = 0,
                                                                             string where = " 1 = 1 ",
                                                                             int startIndex = 0,
                                                                             int pageLength = int.MaxValue,
                                                                             string orderBy =
                                                                                 "ORDER BY Co_Course.id DESC");

        /// <summary>
        /// 根据课程ID和用户ID 找到课程信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Co_Course GetCourseById(int courseId, int userId = 0);

        /// <summary>
        /// 显示可预定课程
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userid"></param>
        /// <param name="DepartId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<Co_Course> GetNewCourseList(out int totalCount, int userid, int DepartId, string where = " 1=1", int startIndex = 0,
                                                                              int pageLength = int.MaxValue);
        /// <summary>
        /// 修改评估分数
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="EvlutionScore"></param>
        /// <returns></returns>
        bool UpdateDepTran_CourseOrderEvlutionScore(int courseid, int userid, int EvlutionScore);

        /// <summary>
        /// 修改考试分数
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="ExamScore"></param>
        /// <returns></returns>
        bool UpdateDepTran_CourseOrderExamScore(int courseid, int userid, int ExamScore);

        /// <summary>
        /// 修改考勤分数
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="AttScore"></param>
        /// <returns></returns>
        bool UpdateDepTran_CourseOrderAttScore(int courseid, int userid, int AttScore);

        /// <summary>
        /// 修改 条件自己加
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool UpdateWhere(string where);

        /// <summary>
        /// 更新考勤分数
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="attScore"></param>
        /// <returns></returns>
        bool DepTran_CourseOrderUsers(int courseId, int userId, decimal attScore);

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
        bool InsertDeptTran_courseOrder(int courseId, int userId, int departId, decimal attScore);


        List<DepTran_CourseOrder> SetUserOrder(out int totalCount, int courseId, int departId);

        bool SetOrder(int courseId, int departId, int userId);

        List<DepTran_CourseOrder> GetAllOrder(int courseId, int departId);

        /// <summary>
        /// 查询报名人员
        /// </summary>
        /// <returns></returns>
        List<Sys_User> GetCourseUserList(out int totalcount, int id, int startIndex = 1, int startLength = int.MaxValue, string where = " 1=1");

        /// <summary>
        /// 获取相关考试人员的信息
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<DepTran_CourseOrder> GetOnLineTest(out int totalCount, int CourseId, int startIndex = 0,
                                                               int pageLength = int.MaxValue);

        /// <summary>
        /// 找出视频转播的开课总人数
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        Co_Course GetCo_CourseLimitNumber(int courseid, int deptid);
    }
}
