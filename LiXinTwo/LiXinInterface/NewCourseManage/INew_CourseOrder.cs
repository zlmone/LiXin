using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.NewCourseManage
{
    public interface INew_CourseOrder
    {
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        void InsertNew_CourseOrder(New_CourseOrder model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        void UpdateNew_CourseOrder(New_CourseOrder model);

        /// <summary>
        /// 已存在的课程学习记录
        /// </summary>
        /// <param name="sqlStr">删除条件</param>
        void DeleteCourseOrder(string sqlStr = "1=2");

        /// <summary>
        /// 获取用户学习课程列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="where"></param>
        /// <param name="strwhere"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<New_CourseOrder> GetNew_CourseOrderListByStudent(int userid, string where = " 1 = 1 ", string strwhere = " 1=1", int startIndex = 0,
                            int pageLength = int.MaxValue, string orderBy = "ORDER BY New_CourseOrder.Id DESC");



        //List<New_CourseOrder> GetNew_CourseOrderListByTeacher(out int totalCount, int userid, string where = " 1 = 1 ", int startIndex = 0,
        //                     int pageLength = int.MaxValue, string orderBy = "ORDER BY New_CourseOrder.Id DESC");
        /// <summary>
        /// 讲师端课程列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="where"></param>
        /// <param name="strwhere"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<New_CourseOrder> GetNew_CourseOrderListByTeacher(int userid, string where = " 1 = 1 ", string strwhere = " 1=1", int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "ORDER BY New_CourseOrder.Id DESC");
        /// <summary>
        /// 获取此课程下面学员的评价详情
        /// </summary>
        /// <returns></returns>
        List<New_CourseOrder> GetUserScoreList(out int totalcount, int courseID, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  su.UserId desc");


        /// <summary>
        /// 获取学员所在课程中的信息
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<New_CourseOrder> GetCourseIdAndType(int courseid, int userid);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<New_CourseOrder> Get(string where = "1=1");

        /// <summary>
        /// 批量打分
        /// </summary>
        void UpdateAllScore(int TogetherScore, int GroupScore, string ids);

        /// <summary>
        /// 一键打分
        /// </summary>
        void UpdateAllScore(int courseID, int IsGroupTeach);
        /// <summary>
        /// 根据课程ID和用户ID 修改学习状态
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="LearnStatus">0：未开始；1：进行中；2：已完成</param>
        void UpdateLearnStatus(int courseid, int userid, int LearnStatus);


        /// <summary>
        /// 教师端考试列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId"></param>
        /// <param name="TeacherId"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<New_CourseOrder> GetTeacherOnLineTest(out int totalCount, int CourseId, int TeacherId, int startIndex = 0,
                                                                int pageLength = int.MaxValue);


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<New_CourseOrder> GetList(string where = " 1=1");

        /// <summary>
        /// 讲师点击日期获取当日课程信息
        /// </summary>
        /// <param name="teacherid"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<New_CourseOrder> GetTeachertoLearnDetal(int teacherid, string where = " 1=1");

        /// <summary>
        /// 学员点击日期获取当日课程信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<New_CourseOrder> GetStudentLearnDetal(int userid, string where = " 1=1");

        /// <summary>
        /// 获取课前建议列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="coursename"></param>
        /// <param name="userid"></param>
        /// <param name="where"></param>
        /// <param name="strwhere"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<New_CourseOrder> GetGetMyproposeList(out int totalCount, int userid, string where = " 1=1", string strwhere = " 1=1", string jsRenderSortField = "", int startIndex = 0, int pageLength = int.MaxValue);

        /// <summary>
        /// 获取我的评估列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userid"></param>
        /// <param name="where"></param>
        /// <param name="strwhere"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<New_CourseOrder> GetAfterCourseList(out int totalCount, int userid, string where, string strwhere, string jsRenderSortField, int startIndex = 0, int pageLength = int.MaxValue);

    }
}
