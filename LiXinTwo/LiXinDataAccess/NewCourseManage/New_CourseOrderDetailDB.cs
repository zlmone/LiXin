using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LiXinModels.User;
using Retech.Data;
using Retech.Core;

namespace LiXinDataAccess.NewCourseManage
{
    public class New_CourseOrderDetailDB : BaseRepository
    {
        #region 增改(基本信息)
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertNew_CourseOrderDetail(New_CourseOrderDetail model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO New_CourseOrderDetail(CourseId,UserId,SubCourseID,LearnTime,LearnStatus,Type,IsLeave,ApprovalName,LeaveReason) VALUES ( @CourseId,@UserId,@SubCourseID,@LearnTime,@LearnStatus,@Type,@IsLeave,@ApprovalName,@LeaveReason);SELECT @@IDENTITY as Id ";

                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.SubCourseID,
                    model.LearnTime,
                    model.LearnStatus,
                    model.Type,
                    model.IsLeave,
                    model.ApprovalName,
                    model.LeaveReason
                };
                var list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.Id);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateNew_CourseOrderDetail(New_CourseOrderDetail model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update New_CourseOrderDetail set CourseId = @CourseId,UserId = @UserId,SubCourseID = @SubCourseID,LearnTime = @LearnTime,LearnStatus = @LearnStatus,Type = @Type,IsLeave=@IsLeave,ApprovalName=@ApprovalName,LeaveReason=@LeaveReason where Id=@Id";

                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.SubCourseID,
                    model.LearnTime,
                    model.LearnStatus,
                    model.Type,
                    model.IsLeave,
                    model.Id,
                    model.ApprovalName,
                    model.LeaveReason
                };
                int result = conn.Execute(strSql, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除已存在的课程学习记录
        /// </summary>
        /// <param name="sqlStr">删除条件</param>
        public void DeleteCourseOrder(string sqlStr = "1=2")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format("delete New_CourseOrderDetail where {0}", sqlStr);
                conn.Execute(sql);
            }
        }

        #endregion

        /// <summary>
        /// 获取课程详情学习表
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public New_CourseOrderDetail GetSingleNew_CourseOrderDetail(int courseid, int userid)
        {         
          using (var conn = OpenConnection())
          {
              var sql = string.Format(@"select * from New_CourseOrderDetail where CourseId={0} and UserId={1}", courseid, userid);
              return conn.Query<New_CourseOrderDetail>(sql).FirstOrDefault();
          }           
        }

        /// <summary>
        /// 学员座位信息
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<New_CourseOrderDetail> GetNew_CourseOrderDetailSeatDetail(string courseid,int userid)
        {
            using (var conn = OpenConnection())
          {
              var sql = string.Format(@" select New_CourseRoomRule.Id,
		 New_CourseRoomRule.SeatDetail as SeatDetail,
		 New_CourseRoomRule.StartTime as StartTime,
		 New_CourseRoomRule.EndTime as EndTime,
		 dbo.f_GetTeacherName(TeacherId) as teachername,
         New_CourseOrderDetail.type,
        dbo.f_GetRoomNameByNew_CourseRoomRuleId(New_CourseRoomRule.id) as RoomName
   from New_CourseOrderDetail left join
   New_CourseRoomRule on New_CourseOrderDetail.SubCourseID=New_CourseRoomRule.Id
    where New_CourseOrderDetail.CourseId={0} and New_CourseOrderDetail.UserId={1} and New_CourseOrderDetail.IsLeave=0", courseid, userid);
            return conn.Query<New_CourseOrderDetail>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取子教室安排的学员
        /// </summary>
        /// <param name="totalCount">总条数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">起始页</param>
        /// <param name="pageLength">每页条数</param>
        /// <returns></returns>
        public List<Sys_User> GetCourseOrderDetailSeatUserDetail(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                             int pageLength = int.MaxValue)
        {
            using (var conn = OpenConnection())
            {
                totalCount =
                   conn.Query<int>(@"select count(1) from New_CourseOrderDetail as a,Sys_User b
where a.UserId=b.UserId and " + where).First();
                var sql = string.Format(@"select top {2} * from (
    select  row_number() over( {1} ) as rowNum, b.Realname,a.UserId,b.Sex,b.JoinDate,b.GraduateSchool,a.IsLeave as IsNew
from New_CourseOrderDetail as a,Sys_User b
where a.UserId=b.UserId
and {0}) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", where," order by b.UserId asc",pageLength);
                return conn.Query<Sys_User>(sql, new{pageCount = pageLength,pageIndex = startIndex / pageLength}).ToList();
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_CourseOrderDetail> GetSearchResult(string where = "1=1")
        {
            using (var connection = OpenConnection())
            {
                string sql = "select * from New_CourseOrderDetail where " + where;
                return connection.Query<New_CourseOrderDetail>(sql).ToList();
            }
        }

        /// <summary>
        ///  查看学员在一门课程中是否有多个课程
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<New_CourseOrderDetail> GetCourseOrderDetailIsSingleOrMore(int courseid, int userid)
        {
            using (var connection = OpenConnection())
            {
                string sql = " select * from New_CourseOrderDetail where CourseId=" + courseid + " and UserId="+userid;

                return connection.Query<New_CourseOrderDetail>(sql).ToList();
            }
        }





    }
}
