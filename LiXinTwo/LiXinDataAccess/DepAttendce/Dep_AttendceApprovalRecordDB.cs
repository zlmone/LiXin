using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels.DepAttendce;

namespace LiXinDataAccess.DepAttendce
{
    public class Dep_AttendceApprovalRecordDB : BaseRepository
    {
        #region 增改(基本信息)
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertDep_AttendceApprovalRecord(Dep_AttendceApprovalRecord model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Dep_AttendceApprovalRecord(CourseId,DeptId,UserId,ApprovalTime,ApprovalFlag,SubmitUserId,SubmitTime) VALUES (@CourseId,@DeptId,@UserId,@ApprovalTime,@ApprovalFlag,@SubmitUserId,@SubmitTime);SELECT @@IDENTITY as Id ";

                var param = new
                {
                    model.CourseId,
                    model.DeptId,
                    model.UserId,
                    model.ApprovalTime,
                    model.ApprovalFlag,
                    model.SubmitUserId,
                    model.SubmitTime
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.Id);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateDep_AttendceApprovalRecord(Dep_AttendceApprovalRecord model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update Dep_AttendceApprovalRecord set CourseId = @CourseId,DeptId = @DeptId,UserId = @UserId,ApprovalTime = @ApprovalTime,ApprovalFlag = @ApprovalFlag,SubmitUserId=@SubmitUserId,SubmitTime=@SubmitTime where Id=@Id";

                var param = new
                {
                    model.CourseId,
                    model.DeptId,
                    model.UserId,
                    model.ApprovalTime,
                    model.ApprovalFlag,
                    model.SubmitUserId,
                    model.SubmitTime,
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
        public List<Dep_AttendceApprovalRecord> GetDep_AttendceApprovalRecord(int courseID, int depID, int startIndex = 1, int startLength = int.MaxValue)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"SELECT * FROM (select row_number()OVER(ORDER BY dtaar.ApprovalTime DESC) num,count(*)OVER(PARTITION BY null) totalcount,dtaar.*,su.Realname,ssu.Realname AS SubmitRealname from Dep_AttendceApprovalRecord AS dtaar LEFT JOIN Sys_User AS su ON su.UserId=dtaar.UserId LEFT JOIN Sys_User AS ssu ON ssu.UserId=dtaar.SubmitUserId where dtaar.CourseId=@CourseId and dtaar.DeptId=@DeptId ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex");
                var param = new
                {
                    CourseId = courseID,
                    DeptId = depID,
                    startIndex = startIndex,
                    startLength = startLength
                };
                return connection.Query<Dep_AttendceApprovalRecord>(query, param).ToList();
            }
        }


        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<Dep_AttendceApprovalRecord> GetDep_AttendceApprovalRecordList()
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"SELECT * FROM Dep_AttendceApprovalRecord");

                return connection.Query<Dep_AttendceApprovalRecord>(query).ToList();
            }
        }


        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<Dep_AttendceApprovalRecord> GetDep_AttendceApprovalRecordListNew(int year)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"
SELECT Dep_CourseDept.*,dep_course.CourseLength as CourseLength,Dep_CourseDept.DepartId as DeptId,dep_course.yearplan as yearplan FROM Dep_CourseDept
left join dep_course on dep_course.id=Dep_CourseDept.courseid
WHERE YearPlan={0}
",year);

                return connection.Query<Dep_AttendceApprovalRecord>(query).ToList();
            }
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<Dep_AttendceApprovalRecord> GetDep_AttendceApprovalRecordListNewForLast()
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"
SELECT * FROM Dep_Attendce WHERE ApprovalFlag=1 --AND Status in(2,3)
");

                return connection.Query<Dep_AttendceApprovalRecord>(query).ToList();
            }
        }


        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <param name="userId"></param>
        /// <param name="ApprovalTime"></param>
        /// <param name="ApprovalFlag"></param>
        /// <param name="reason"></param>
        /// <param name="SubmitUserId"></param>
        /// <param name="SubmitTime"></param>
        /// <returns></returns>
        public bool Insert(int courseId, int departId, int userId, DateTime ApprovalTime, int ApprovalFlag, string reason, int SubmitUserId, DateTime SubmitTime)
        {
            using (var connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(
                    "INSERT INTO Dep_AttendceApprovalRecord(CourseId,DeptId,UserId,ApprovalTime,ApprovalFlag,Reason,SubmitUserId,SubmitTime) VALUES (@courseId,@departId,@userId,@ApprovalTime,@ApprovalFlag,@reason,@SubmitUserId,@SubmitTime)");
                var param = new
                {
                    courseId = courseId,
                    departId = departId,
                    userId = userId,
                    ApprovalTime = ApprovalTime,
                    ApprovalFlag = ApprovalFlag,
                    reason = reason,
                    SubmitTime = SubmitTime,
                    SubmitUserId = SubmitUserId
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 所有通过审批的
        /// </summary>
        /// <returns></returns>
        public List<Dep_AttendceApprovalRecord> GetAttendce(int yearid, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT CourseId, da.UserId,da.Status,syu.DeptId,Dep_Course.Way FROM Dep_Attendce   da
LEFT JOIN Sys_User syu ON syu.UserId=da.UserId
LEFT JOIN Dep_Course ON Dep_Course.Id=da.CourseId
WHERE ApprovalFlag=1 and Dep_Course.YearPlan=" + yearid + @"
AND da.UserId IN (SELECT userId FROM View_CheckUser) and
" + where;
                return conn.Query<Dep_AttendceApprovalRecord>(sql).ToList();
            }
        }
        #endregion
    }
}
