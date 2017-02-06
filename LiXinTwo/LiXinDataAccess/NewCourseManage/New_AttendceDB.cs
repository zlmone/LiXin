using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Retech.Data;
using Retech.Core;

namespace LiXinDataAccess.NewCourseManage
{
    public class New_AttendceDB : BaseRepository
    {
        #region 增改(基本信息)
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertNew_Attendce(New_Attendce model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO New_Attendce(CourseId,UserId,SubCourseID,StartTime,EndTime,AttStatus) VALUES (@CourseId,@UserId,@SubCourseID,@StartTime,@EndTime,@AttStatus);SELECT @@IDENTITY as Id ";

                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.SubCourseID,
                    model.StartTime,
                    model.EndTime,
                    model.AttStatus
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.Id);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateNew_Attendce(New_Attendce model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update New_Attendce set CourseId = @courseid,UserId = @userid,SubCourseID = @subcourseid,StartTime = @starttime,EndTime = @endtime,AttStatus = @attstatus where Id=@Id";

                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.SubCourseID,
                    model.StartTime,
                    model.EndTime,
                    model.AttStatus,
                    model.Id
                };
                int result = conn.Execute(strSql, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public New_Attendce GetNew_Attendce(int courseID, int userID, int subCourseID)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from New_Attendce where CourseId=@CourseId and UserId=@UserId and SubCourseID=@SubCourseID");
                var param = new { CourseId = courseID, UserId = userID, SubCourseID = subCourseID };
                return connection.Query<New_Attendce>(query, param).FirstOrDefault();

            }
        }

        #endregion

        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        public List<New_CourseOrderDetail> GetNewAttendUserList(int courseId, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY ncod.Id " + Order + ") num,count(*)OVER(PARTITION BY null) totalcount,(SELECT count(*) FROM New_CourseOrderDetail WHERE CourseId=@CourseId) AS CoCount,(SELECT count(*) FROM New_Attendce WHERE CourseId=@CourseId) AS AttCount,ncod.*,su.Realname,ncrr.TeacherId,ncrr.StartTime AS CoStartTime,ncrr.EndTime AS CoEndTime,na.StartTime,na.EndTime,na.AttStatus,isnull(na.Id,0) as attId,ngu.ClassId,ngu.GroupId,su.NumberID,nc.ClassName,ng.GroupName,ncr.RoomName,ncrr.IsAttFlag FROM New_CourseOrderDetail AS ncod LEFT JOIN New_Attendce  AS na ON (na.CourseId=ncod.CourseId AND na.SubCourseID=ncod.SubCourseID AND na.UserId=ncod.UserId) LEFT JOIN New_CourseRoomRule AS ncrr ON ncrr.Id=ncod.SubCourseID LEFT JOIN New_GroupUser AS ngu ON ngu.UserId=ncod.UserId LEFT JOIN New_Class AS nc ON nc.Id=ngu.ClassId LEFT JOIN New_Group AS ng ON (ng.Id=ngu.GroupId AND ngu.ClassId=ngu.ClassId) LEFT JOIN Sys_User AS su ON su.UserId=ncod.UserId LEFT JOIN New_ClassRoom AS ncr ON ncr.Id=ncrr.RoomId WHERE ncod.IsLeave=0 AND ncrr.IsAttFlag!=3 and ncod.CourseId=@CourseId and " + where + " ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    CourseId = courseId
                };
                return conn.Query<New_CourseOrderDetail>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 扩展GetNewAttendUserList方法 增加讲师ID 姓名
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_CourseOrderDetail> GetNewAttendList(int courseId, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY ncod.Id " + Order + ") num,count(*)OVER(PARTITION BY null) totalcount,(SELECT count(*) FROM New_CourseOrderDetail WHERE CourseId=@CourseId) AS CoCount,(SELECT count(*) FROM New_Attendce WHERE CourseId=@CourseId) AS AttCount,ncod.*,su.Realname,ncrr.TeacherId,te.Realname as teachername,ncrr.StartTime AS CoStartTime,ncrr.EndTime AS CoEndTime,na.StartTime,na.EndTime,na.AttStatus,isnull(na.Id,0) as attId,ngu.ClassId,ngu.GroupId,ngu.NumberID,nc.ClassName,ng.GroupName,ncr.RoomName,ncrr.IsAttFlag FROM New_CourseOrderDetail AS ncod LEFT JOIN New_Attendce  AS na ON (na.CourseId=ncod.CourseId AND na.SubCourseID=ncod.SubCourseID AND na.UserId=ncod.UserId) LEFT JOIN New_CourseRoomRule AS ncrr ON ncrr.Id=ncod.SubCourseID LEFT JOIN New_GroupUser AS ngu ON ngu.UserId=ncod.UserId LEFT JOIN New_Class AS nc ON nc.Id=ngu.ClassId LEFT JOIN New_Group AS ng ON (ng.Id=ngu.GroupId AND ngu.ClassId=ngu.ClassId) LEFT JOIN Sys_User AS su ON su.UserId=ncod.UserId LEFT JOIN New_ClassRoom AS ncr ON ncr.Id=ncrr.RoomId  left join Sys_User as te on te.UserId=ncrr.TeacherId WHERE ncod.CourseId=@CourseId and " + where + " ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    CourseId = courseId
                };
                return conn.Query<New_CourseOrderDetail>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取考勤列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_Attendce> GetList(string where="1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var query = string.Format(@"select * from New_Attendce where "+where);

                return conn.Query<New_Attendce>(query).ToList();
            }
        }

        /// <summary>
        /// 获得我的考勤数据列表
        /// </summary>
        public List<New_CourseOrderDetail> GetMyNewAttendList(int userID, int startIndex = 1, int startLength = int.MaxValue, string Order = "desc", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY " + Order + ") num,count(*)OVER(PARTITION BY null) totalcount, ncod.CourseId,ncod.UserId,ncod.SubCourseID,ncod.Type,nc.CourseName,nc.IsGroupTeach,ncr.RoomName,su.Realname AS teachername,ncrr.GroupName,ncrr.IsAttFlag,ncrr.StartTime AS CoStartTime,ncrr.EndTime AS CoEndTime,na.StartTime,na.EndTime,na.AttStatus,isnull(na.Id,0) as attId  FROM New_CourseOrderDetail AS ncod LEFT JOIN New_Course AS nc ON nc.Id=ncod.CourseId LEFT JOIN New_CourseRoomRule AS ncrr ON (ncod.SubCourseID=ncrr.Id AND ncrr.CourseId=ncod.CourseId) LEFT JOIN New_ClassRoom AS ncr ON ncr.Id=ncrr.RoomId LEFT JOIN Sys_User as su ON su.UserId=ncrr.TeacherId LEFT JOIN New_Attendce AS na ON (na.UserId= ncod.UserId AND na.CourseId=ncod.CourseId AND ncod.SubCourseID=na.SubCourseID) WHERE ncod.UserId=@UserId and " + where + " ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    UserId = userID
                };
                return conn.Query<New_CourseOrderDetail>(sql, param).ToList();
            }
        }


    }
}
