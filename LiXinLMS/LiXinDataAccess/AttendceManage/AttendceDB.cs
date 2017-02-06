using System.Collections.Generic;
using System.Linq;
using Retech.Core;
using Retech.Data;
using System.Data;
using LiXinModels.CourseManage;
using LiXinModels.CourseLearn;
using LiXinModels;

namespace LiXinDataAccess.AttendceManage
{
    public class AttendceDB : BaseRepository
    {
        /// <summary>
        /// 获得数据列表(有分页)
        /// </summary>
        public List<Co_Course> GetAttendceList(int way, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY StartTime " + Order + ") num,count(*)OVER(PARTITION BY null) totalcount,count(cm.CourseId) AS midNum,cc.Id,CourseName,CourseLength,Teacher,AttFlag,AttStatus,IsMust,StartTime,EndTime,su.Realname as TeacherName,sc.RoomName FROM Co_Course cc INNER JOIN Sys_User su ON su.UserId=cc.Teacher LEFT JOIN Sys_ClassRoom sc ON sc.Id=cc.RoomId LEFT JOIN Cl_MidAttendce as cm ON cm.CourseId=cc.Id WHERE cc.IsDelete=0 and cc.AttFlag>0 AND cc.CourseFrom=2 and cc.Publishflag=1 AND cc.Way=@Way and " + where + " GROUP BY cc.Id,cc.Id,CourseName,CourseLength,Teacher,AttFlag,AttStatus,IsMust,StartTime,EndTime,su.Realname,sc.RoomName ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    Way = way
                };
                return conn.Query<Co_Course>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        public List<Cl_Attendce> GetAttendUserList(int CourseId,string Order = "asc", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT cc.NumberLimited,cl.UserId,cl.CourseId,cl.IsLeave,cl.LearnStatus,cl.ApprovalFlag,cl.ApprovalDateTime,cc.StartTime AS courseStart,cc.EndTime AS courseEnd,cc.AttFlag,cc.AttStatus,cl.ApprovalLimitTime,cl.OrderTime,cl.OrderEndTime,ca.StartTime,ca.EndTime,su.Realname,su.CPA,sd.DeptName,cl.OrderStatus,isnull(cm.Id,0) AS MakeOrederId FROM Cl_CourseOrder AS cl left JOIN Sys_User AS su ON su.UserId=cl.UserId LEFT JOIN Sys_Department AS sd ON su.DeptId=sd.DepartmentId LEFT JOIN  Cl_Attendce AS ca ON (ca.UserId=cl.UserId AND cl.CourseId=ca.CourseId) LEFT JOIN Cl_MakeUpOrder AS cm ON (cm.UserId=cl.UserId AND cl.CourseId=cm.CourseId) LEFT JOIN Co_Course AS cc ON cc.Id=cl.CourseId where cl.Id NOT IN  (SELECT cco.Id FROM Cl_CourseOrder AS cco LEFT JOIN Cl_TimeOutLeave AS cto ON(cco.CourseId=cto.CourseId AND cco.UserId=cto.UserId) where ((cco.ApprovalFlag=1 AND cco.ApprovalDateTime<=cco.ApprovalLimitTime and cco.IsLeave=1 AND cco.OrderStatus=1 ) OR cco.OrderStatus IN (0,2) OR (cco.OrderStatus=1 AND cto.ApprovalFlag=1)) AND cco.CourseId=@CourseId) AND cl.CourseId=@CourseId and " + where;
                var param = new
                {
                    CourseId = CourseId
                };
                return conn.Query<Cl_Attendce>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取人员ID
        /// </summary>
        public int GetUserID(string username,string deptname)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT UserId FROM dbo.Sys_User AS su LEFT JOIN Sys_Department AS sd ON su.DeptId=sd.DepartmentId 
WHERE su.Realname='" + username + "' AND sd.DeptName='" + deptname + "'";
                return conn.Query<int>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取补充信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public Co_Course GetJobID(int courseid, int userid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT su.JobNum,su.Realname AS TeacherName,su.LeaderID,cc.CourseName,cc.CourseLength,ca.StartTime AS attStartTime,ca.EndTime AS attEndTime,cc.StartTime,cc.EndTime FROM Cl_Attendce AS ca LEFT JOIN Co_Course AS cc ON ca.CourseId=cc.Id LEFT JOIN Sys_User AS su ON su.UserId=ca.UserId WHERE ca.UserId=@UserId AND ca.CourseId=@CourseId";
                var param = new
                {
                    UserId = userid,
                    CourseId = courseid
                };
                return conn.Query<Co_Course>(sql, param).FirstOrDefault();
            }
        }
        /// <summary>
        /// 获取课程时长，开始时间，结束时间，用户上级jobNum
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Co_Course GetCoAndUser(int courseid, int userid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT CourseLength,IsMust,Way,AttFlag,AttStatus,CourseLengthDistribute,IsCPA,StartTime,EndTime,IsTest,IsPing,(SELECT LeaderID FROM Sys_User WHERE UserId=@UserId) AS LeaderID FROM Co_Course WHERE Id=@CourseId";
                var param = new
                {
                    UserId = userid,
                    CourseId = courseid
                };
                return conn.Query<Co_Course>(sql, param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 判断是否第一次学习课程
        /// </summary>
        /// <param name="CourseId">课程</param>
        /// <param name="UserId">用户</param>
        /// <returns></returns>
        public bool ExistAtts(int CourseId, int UserId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select count(1) FROM Cl_Attendce WHERE CourseId IN (SELECT cc.id FROM dbo.Co_Course AS cc LEFT JOIN Co_Course AS ccc ON cc.Code=ccc.Code WHERE cc.IsDelete=0 AND cc.Publishflag=1 AND ccc.Id=@CourseId) AND UserId=@UserId AND CourseId!=@CourseId";
                var param = new { CourseId = CourseId, UserId = UserId };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count < 1;
            }
        }

        /// <summary>
        /// 更新课程表AttStatus
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public bool UpdateAttStatus(int CourseId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Co_Course SET AttStatus = 1 WHERE Id=@CourseId";
                var param = new
                {
                    CourseId = CourseId
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }

        }

        /// <summary>
        /// 获得消息发送人员信息
        /// </summary>
        public List<Message> GetUserinfo(int CourseId, string Users)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT tab.*,su.Realname AS Leadname,su.MobileNum,su.Email,cc.CourseName FROM (SELECT UserId,Realname,LeaderID FROM Sys_User WHERE UserId IN (" + Users + ")) AS tab LEFT JOIN Sys_User AS su ON su.JobNum=tab.LeaderID LEFT JOIN Co_Course AS cc ON cc.Id=@CourseId ";
                var param = new
                {
                    CourseId = CourseId
                };
                return conn.Query<Message>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取课程考勤状态
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public int GetAttStatus(int CourseId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT AttStatus FROM dbo.Co_Course WHERE Id=" + CourseId;
                return conn.Query<int>(sql).FirstOrDefault();
            }
        }
    }
}
