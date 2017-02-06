using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.User;

namespace LiXinInterface.NewCourseManage
{
    public interface INew_CourseOrderDetail
    {
        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="model"></param>
        void InsertNew_CourseOrderDetail(New_CourseOrderDetail model);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateNew_CourseOrderDetail(New_CourseOrderDetail model);

        /// <summary>
        /// 删除已存在的课程学习记录
        /// </summary>
        /// <param name="sqlStr">删除条件</param>
        void DeleteCourseOrder(string sqlStr = "1=2");
        /// <summary>
        /// 查询单个事体
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        New_CourseOrderDetail GetSingleNew_CourseOrderDetail(int courseid, int userid);


        List<New_CourseOrderDetail> GetNew_CourseOrderDetailSeatDetail(string courseid, int userid);

        /// <summary>
        /// 获取子教室安排的学员
        /// </summary>
        /// <param name="totalCount">总条数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">起始页</param>
        /// <param name="pageLength">每页条数</param>
        /// <returns></returns>
        List<Sys_User> GetCourseOrderDetailSeatUserDetail(out int totalCount, string where = " 1 = 1 ",
                                                                 int startIndex = 0,
                                                                 int pageLength = int.MaxValue);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<New_CourseOrderDetail> GetSearchResult(string where = "1=1");

        /// <summary>
        /// 查看学员对于一门课程是否有多个课程
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        List<New_CourseOrderDetail> GetCourseOrderDetailIsSingleOrMore(int courseid, int userid);
    }
}
