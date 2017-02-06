using System;
using System.Collections.Generic;
using System.Data;
using Retech.Core;
using Retech.Data;
using System.Linq;
using System.Text;
using LiXinModels.DepTranAttendce;

namespace LiXinDataAccess.DepTranAttendce
{
    public class DepTranAttendceApprovalDB : BaseRepository
    {
        /// <summary>
        /// 获得审批的数据列表(有分页)
        /// </summary>
        /// <param name="startIndex">当前页</param>
        /// <param name="startLength">条数</param>
        /// <param name="order">排序方式</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<AttendceCourseModel> GetDepTranAttendceApprovalList(string whereFlag, int startIndex = 1, int startLength = int.MaxValue, string order = " a.SubmitTime desc", string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT * FROM (
	select * from (
		SELECT row_number()OVER(ORDER BY "+ order+ @") num,
		count(*)OVER(PARTITION BY null) Totalcount,
		(select count(*) from DepTran_Attendce x where x.DepartSetId=a.DepartID and x.Status=0) as WaitApp,
		(select count(*) from DepTran_Attendce x where x.DepartSetId=a.DepartID and x.Status=1) as YesApp,
		(select count(*) from DepTran_Attendce x where x.DepartSetId=a.DepartID and x.Status=2) as NoApp,
		(select count(*) from DepTran_Attendce x where x.DepartSetId=a.DepartID and x.CourseId=c.Id) as AppCount,
		c.Id as CourseID,
		a.DepartID as DepartID,
		c.CourseLength as CourseLength,
		c.CourseName as CourseName,
		c.IsMust as IsMust,
		c.StartTime,
		c.EndTime,
		r.RoomName,
		a.OpenFlag,
		d.DepartSetName as DeptName,
		a.SubmitTime,
		u.Realname as TeacherName,
        isnull(a.ApprovalFlag,0)as ApprovalFlag,
		a.AttFlag
		FROM DepTran_CourseOpen a 
		LEFT JOIN Co_Course c ON a.CourseId=c.Id 
		LEFT JOIN Sys_User u ON c.Teacher=u.UserId
		LEFT JOIN Sys_ClassRoom r ON r.Id=c.RoomId
		LEFT JOIN DepTran_DepartLeaderConfig d ON a.DepartID=d.Id
		WHERE " + whereFlag +
	") as temp where " + where+@") result 
WHERE  num BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex";
                var param = new{startIndex, startLength};
                return conn.Query<AttendceCourseModel>(sql, param).ToList();
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
from DepTran_Attendce as dta
left join Sys_User as su on su.UserId=dta.UserId
left join DepTran_DepartUser as dtdu on dtdu.UserId=dta.UserId
left join Co_Course as cc on cc.Id=dta.CourseId
where dta.CourseId=@courseId and dtdu.DepartSetId=@depIds and " + where;
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
                    "update DepTran_Attendce set ApprovalFlag=@approval,Reason=@reason where CourseId=@courseId and UserId=@userId");
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
       public List<AttendceCourseModel> GetUserList(int courseId, int depatId)
       {
           using (IDbConnection connection = OpenConnection())
           {
               StringBuilder strSql = new StringBuilder();
               strSql.Append(" SELECT row_number()OVER(ORDER BY dtco.Id asc) num,count(*)OVER(PARTITION BY null) totalcount," +
                             " (SELECT count(*) FROM DepTran_CourseOrder WHERE CourseId=@courseId AND DepartSetId IN (" + depatId + ")) AS OrderCount," +
                             " (SELECT count(*) FROM DepTran_Attendce WHERE CourseId=@courseId and Status>0 AND DepartSetId IN (" + depatId + ")) AS AttCount," +
                             " dtco.*,su.Realname,su.DeptName,dtdlc.DepartSetName,dta.Status,dta.ApprovalFlag,dtcop.AttFlag,dtcop.OpenFlag," +
                             " su.MobileNum,su.Email,dta.Status, " +
                             " cc.CourseLengthDistribute,cc.IsTest,cc.IsPing,cc.CourseLength" +
                             " FROM DepTran_CourseOrder AS dtco " +
                             " LEFT JOIN DepTran_Attendce AS dta " +
                             " ON (dta.CourseId=dtco.CourseId AND dta.DepartSetId=dtco.DepartSetId AND dta.UserId=dtco.UserId)" +
                             " LEFT JOIN DepTran_DepartLeaderConfig AS dtdlc ON dtdlc.Id=dtco.DepartSetId " +
                             " LEFT JOIN DepTran_CourseOpen AS dtcop ON (dtcop.CourseId=dtco.CourseId AND dtcop.DepartId=dtco.DepartSetId) " +
                             " LEFT JOIN Sys_User AS su ON su.UserId=dtco.UserId " +
                             " left join Co_Course as cc on cc.Id=dtco.CourseId " +
                             " WHERE dtco.CourseId=@courseId and dtco.OrderStatus>0 AND dtco.DepartSetId IN (" + depatId + ") and  1=1 ");
               var param = new
                   {
                       courseId = courseId,
                       depatId = depatId
                   };
               return connection.Query<AttendceCourseModel>(strSql.ToString(), param).ToList();
           }
       }
    }
}
