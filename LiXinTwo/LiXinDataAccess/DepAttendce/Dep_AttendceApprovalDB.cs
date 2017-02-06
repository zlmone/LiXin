using System;
using System.Collections.Generic;
using System.Data;
using LiXinModels.DepAttendce;
using LiXinModels.DepTranAttendce;
using Retech.Core;
using Retech.Data;
using System.Linq;
using System.Text;

namespace LiXinDataAccess.DepAttendce
{
    public class Dep_AttendceApprovalDB : BaseRepository
    {
        /// <summary>
        /// 获得审批的数据列表(有分页)
        /// </summary>
        /// <param name="startIndex">当前页</param>
        /// <param name="startLength">条数</param>
        /// <param name="order">排序方式</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<DepAttendceCourseModel> GetDepTranAttendceApprovalList(string whereFlag, int startIndex = 1, int startLength = int.MaxValue, string order = " a.SubmitTime desc", string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT * FROM (
	select * from (
		SELECT row_number()OVER(ORDER BY "+ order+ @") num,
		count(*)OVER(PARTITION BY null) Totalcount,
		c.Id as CourseID,
		a.DepartID as DepartID,
		c.CourseLength as CourseLength,
		c.CourseName as CourseName,
		c.IsMust as IsMust,
		c.StartTime,
		c.EndTime,
		r.RoomName,
		a.OpenFlag,
		d.DeptName as DeptName,
		a.SubmitTime,
		u.Realname as TeacherName,
        isnull(a.ApprovalFlag,0)as ApprovalFlag,
		a.AttFlag
		FROM Dep_CourseDept a 
		LEFT JOIN Dep_Course c ON a.CourseId=c.Id 
		LEFT JOIN Sys_User u ON c.Teacher=u.UserId
		LEFT JOIN Dep_ClassRoom r ON r.Id=c.RoomId
		left join Sys_Department d on d.DepartmentId=a.DepartID
		WHERE " + whereFlag +
	") as temp where " + where+@") result 
WHERE  num BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex";
                var param = new{startIndex, startLength};
                return conn.Query<DepAttendceCourseModel>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得群组人员数据列表
        /// </summary>
        public List<AttendceCourseModel> GetDepTran_AttendceUserList(int courseId, string depIds, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = @"SELECT * FROM ( 
select  row_number() over(ORDER BY dta.UserId DESC) as rowNum,su.Realname,su.Sex,su.Telephone,su.Email,dta.UserId,dta.CourseId,
dta.StartTime,dta.EndTime,dta.Status,dta.ApprovalFlag,cc.CourseLengthDistribute,cc.CourseLength
from Dep_Attendce as dta
left join Sys_User as su on su.UserId=dta.UserId
left join Dep_Course as cc on cc.Id=dta.CourseId
where dta.CourseId=@courseId and su.DepartId=@depIds and " + where;
                sql += " ) result WHERE  rowNum BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    courseId = courseId,
                    depIds=depIds
                };
                return connection.Query<AttendceCourseModel>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 跟新审批状态
        /// </summary>
        public bool DepTranAttendceApprovalUsers(string courseId, int userId, string reason, int approval)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(
                    "update Dep_Attendce set ApprovalFlag=@approval,Reason=@reason where CourseId=@courseId and UserId=@userId");
                var param = new
                    {
                        courseId=courseId,
                        userId=userId,
                        reason=reason,
                        approval=approval
                    };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 获取人员数据列表
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="depatId"></param>
        /// <returns></returns>
        public List<DepAttendceCourseModel> GetUserList(int courseId, int depatId)
       {
           using (IDbConnection connection = OpenConnection())
           {
               StringBuilder strSql = new StringBuilder();
               strSql.Append(" SELECT row_number()OVER(ORDER BY dtco.Id asc) num," +
                             " dtco.*,su.Realname,su.DeptName,isnull(dta.Status,0) as Status,dtcop.AttFlag,dtcop.ApprovalFlag as CourseApprovalFlag,su.MobileNum as Telephone,su.Email,su.Sex,su.CPA,su.SubordinateSubstation," +
                             " isnull(dc.CourseLengthDistribute,'0;0;0') as CourseLengthDistribute,isnull(dc.CourseLength,0) as CourseLength,isnull(dc.IsTest,0) as IsTest,isnull(dc.IsPing,0) as IsPing,dtco.IsLeave,dtco.ApprovalFlag   " +
                             " FROM Dep_CourseOrder AS dtco " +
                             " LEFT JOIN Dep_Attendce AS dta ON (dta.CourseId=dtco.CourseId AND dta.UserId=dtco.UserId) " +
                             " LEFT JOIN Dep_CourseDept AS dtcop ON (dtcop.CourseId=dtco.CourseId) " +
                             " LEFT JOIN Sys_User AS su ON (su.UserId=dtco.UserId and dtco.DepartSetId=su.DeptId) " +
                             " left join Dep_Course as dc on dc.Id=dta.CourseId" +
                             " WHERE dtco.CourseId=@courseId and dtco.OrderStatus>0 AND dtco.DepartSetId=@depatId and  1=1");
               var param = new
                   {
                       courseId = courseId,
                       depatId = depatId
                   };
               return connection.Query<DepAttendceCourseModel>(strSql.ToString(), param).ToList();
           }
       }
    }
}
