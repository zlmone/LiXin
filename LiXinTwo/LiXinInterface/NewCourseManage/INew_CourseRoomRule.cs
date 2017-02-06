using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Survey;

namespace LiXinInterface.NewCourseManage
{
    public interface INew_CourseRoomRule
    {
        /// <summary>
        /// 获取单个课程信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        New_CourseRoomRule GetNewCourseRoomRule(int id);

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="model"></param>
        void AddNew_CourseRoomRule(New_CourseRoomRule model);

        /// <summary>
        /// 根据课程ID找出数据
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        List<New_CourseRoomRule> GetNew_CourseRoomRuleListByCourseId(int courseId, string sqlwhere = "");

        /// <summary>
        /// 根据条件获取学员对讲师的评价信息详情页面（暂时废弃的代码）
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">条件语句格式" and ..."</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// <returns></returns>
        List<Survey_ReplyAnswer> GetEvaluateUserToTeacherById(out int totalCount, string where = "", int startIndex = 1,
                                                              int startLength = int.MaxValue, string orderBy = "");
        New_CourseRoomRule Get(string wherestr = "1=1");
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        New_CourseRoomRule GetNew_CourseRoomRule(int Id, string wherestr = "1=1");


        /// <summary>
        /// 根据ID获取学员对我的评价详情信息
        /// </summary>
        /// <param name="courseRoomRuleId">新员工课程教室分配规则ID</param>
        /// <returns></returns>
        New_CourseRoomRule GetCourseRoomRuleEvaluate(int courseRoomRuleId);


        /// <summary>
        /// 根据课程ID删除相关的集中授课和分组带教
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <param name="type">类型：-1:都删；0：集中；1：分组带教</param>
        /// <returns></returns>
        bool DeleteCourseRoomRule(int courseID, int type = -1);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="model">实体</param>
        void UpdateCourseRoomRule(New_CourseRoomRule model);

        /// <summary>
        /// 查找分组教室（为查找我的座位）
        /// </summary>
        /// <param name="sqlwhere">查询条件</param>
        /// <returns></returns>
        List<New_CourseRoomRule> GetRoomRuleForSeat(string sqlwhere = "");

        /// <summary>
        /// 根据ID获取学员对我的评价详情信息
        /// </summary>
        /// <param name="userIds">用户ID集合</param>
        /// <returns></returns>
        List<UserSeatModel> GetUserSeatDetail(string userIds);
    }
}
