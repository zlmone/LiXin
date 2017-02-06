using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;
using LiXinModels.DepAttendce;
using Retech.Core;
using Retech.Data;
using System.Data;
using LiXinModels.DepTranAttendce;
using LiXinModels.DepTranManage;

namespace LiXinDataAccess.DepTranAttendce
{
    public class DepTran_AttendceDB : BaseRepository
    {
        #region 增改(基本信息)
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertDepTran_Attendce(DepTran_Attendce model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO DepTran_Attendce(CourseId,UserId,StartTime,EndTime,Status,ApprovalFlag,Reason,DepartSetId) VALUES (@CourseId,@UserId,@StartTime,@EndTime,@Status,@ApprovalFlag,@Reason,@DepartSetId);SELECT @@IDENTITY as Id ";

                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.StartTime,
                    model.EndTime,
                    model.Status,
                    model.ApprovalFlag,
                    model.Reason,
                    model.DepartSetId
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.Id);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateDepTran_Attendce(DepTran_Attendce model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update DepTran_Attendce set CourseId = @CourseId,UserId = @UserId,StartTime = @StartTime,EndTime = @EndTime,Status = @Status,ApprovalFlag = @ApprovalFlag,Reason = @Reason,DepartSetId=@DepartSetId where Id=@Id";

                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.StartTime,
                    model.EndTime,
                    model.Status,
                    model.ApprovalFlag,
                    model.Reason,
                    model.DepartSetId,
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
        public DepTran_Attendce GetDepTran_Attendce(int courseID, int userID)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from DepTran_Attendce where CourseId=@CourseId and UserId=@UserId ");
                var param = new { CourseId = courseID, UserId = userID };
                return connection.Query<DepTran_Attendce>(query, param).FirstOrDefault();
            }
        }

        #endregion

        /// <summary>
        /// 获得数据列表(有分页)
        /// </summary>
        public List<AttendceCourseModel> GetDepTran_AttendceList(string depId, int startIndex = 1, int startLength = int.MaxValue, string Order = "StartTime DESC", string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY " + Order + ") num,count(*)OVER(PARTITION BY null) totalcount,*  FROM ( SELECT cc.Id AS CourseID,CourseName,CourseLength,Teacher,IsMust,StartTime,EndTime,IsRT,su.Realname as TeacherName,sc.RoomName,isnull(dtco.AttFlag,0) AS AttFlag,isnull(dtco.OpenFlag,0) AS OpenFlag,isnull(dtco.ApprovalFlag,0) as ApprovalFlag FROM Co_Course cc LEFT JOIN Sys_User su ON su.UserId=cc.Teacher LEFT JOIN Sys_ClassRoom sc ON sc.Id=cc.RoomId LEFT JOIN DepTran_CourseOpen dtco ON dtco.CourseId=cc.Id AND dtco.DepartId in (@DepartId) WHERE cc.IsDelete=0 and cc.AttFlag>0 AND cc.CourseFrom=2 and cc.Publishflag=1 AND cc.Way=1 AND cc.IsRT=1) AS temp where " + where + " ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    DepartId = string.IsNullOrEmpty(depId)?"0":depId
                };
                return conn.Query<AttendceCourseModel>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        public List<DepTran_CourseOrder> GetDepTran_AttendUserList(int courseId, int depId, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY dtco.Id " + Order + ") num,count(*)OVER(PARTITION BY null) totalcount,(SELECT count(*) FROM DepTran_CourseOrder WHERE CourseId=@CourseId and OrderStatus=1 AND DepartSetId IN (" + depId + ")) AS OrderCount,(SELECT count(*) FROM DepTran_Attendce WHERE CourseId=@CourseId and Status>0 AND DepartSetId IN (" + depId + ")) AS AttCount,dtco.*,su.Realname,su.Sex,su.DeptName,dtdlc.DepartSetName,dta.Status,dtcop.ApprovalFlag,dtcop.AttFlag,dtcop.OpenFlag,su.MobileNum,su.Email FROM DepTran_CourseOrder AS dtco LEFT JOIN DepTran_Attendce AS dta ON (dta.CourseId=dtco.CourseId AND dta.DepartSetId=dtco.DepartSetId AND dta.UserId=dtco.UserId) LEFT JOIN DepTran_DepartLeaderConfig AS dtdlc ON dtdlc.Id=dtco.DepartSetId LEFT JOIN DepTran_CourseOpen AS dtcop ON (dtcop.CourseId=dtco.CourseId AND dtcop.DepartId=dtco.DepartSetId) LEFT JOIN Sys_User AS su ON su.UserId=dtco.UserId WHERE dtco.CourseId=@CourseId and dtco.OrderStatus in (1,2) AND dtco.DepartSetId IN (" + depId + ") and " + where + " ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    CourseId = courseId
                };
                return conn.Query<DepTran_CourseOrder>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得群组人员数据列表
        /// </summary>
        public List<DepTranDepartSetting> GetDepartUserList(int courseId, int depId, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY dtdu.DepartSetId) num,count(*)OVER(PARTITION BY null) totalcount,dtdu.*,dtdlc.DepartSetName,su.Realname,su.Sex,su.DeptName FROM DepTran_DepartUser AS dtdu LEFT JOIN DepTran_DepartLeaderConfig AS dtdlc ON dtdlc.Id=dtdu.DepartSetId LEFT JOIN Sys_User AS su ON su.UserId=dtdu.UserId WHERE dtdu.UserId NOT IN (SELECT UserId FROM DepTran_CourseOrder WHERE CourseId=@CourseId and OrderStatus in (1,2) AND DepartSetId IN (" + depId + ")) AND dtdu.DepartSetId IN (" + depId + ") AND dtdu.DepartSetId NOT IN (SELECT DepartId FROM DepTran_CourseOpen WHERE CourseId=@CourseId AND DepartId IN (" + depId + ") AND AttFlag=1) and " + where + " ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    CourseId = courseId
                };
                return conn.Query<DepTranDepartSetting>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 验证人员是否满足导入条件
        /// </summary>
        public int IsExistAttendce(int courseId, int userid, int depId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = @"SELECT DepartSetId FROM DepTran_DepartUser WHERE UserId=@UserId AND DepartSetId IN (" + depId + ") AND DepartSetId NOT IN (SELECT DepartId FROM DepTran_CourseOpen WHERE CourseId=@CourseId AND DepartId IN (" + depId + ") AND AttFlag=1)";
                var param = new
                {
                    UserId = userid,
                    CourseId = courseId
                };
                return connection.Query<int>(sql, param).FirstOrDefault();
            }
        }


        /// <summary>
        /// 获得差异数据列表(有分页)
        /// </summary>
        public List<AttendceCourseModel> GetDifferenceList(int depId, int startIndex = 1, int startLength = int.MaxValue, string Order = "cc.StartTime DESC", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY " + Order + ") num,count(*)OVER(PARTITION BY null) totalcount,cc.Id AS CourseID,CourseName,IsMust,StartTime,EndTime,IsOpenSub,ddlc.DepartSetName,(SELECT COUNT(*) FROM DepTran_CourseOrder WHERE CourseId=cc.Id and OrderStatus in (1,2) AND DepartSetId=@DepartId) AS CoCount,(SELECT COUNT(*) FROM DepTran_Attendce WHERE CourseId=cc.Id AND Status>0 AND DepartSetId=@DepartId) AS AttCount,(SELECT COUNT(*) FROM DepTran_Attendce WHERE CourseId=cc.Id AND Status=1 AND DepartSetId=@DepartId) AS AttOkCount,(SELECT COUNT(*) FROM DepTran_Attendce WHERE CourseId=cc.Id AND Status=2 AND DepartSetId=@DepartId) AS AttNOCount,(SELECT COUNT(*) FROM DepTran_Attendce WHERE CourseId=cc.Id AND Status=3 AND DepartSetId=@DepartId) AS chidaoCount,(SELECT COUNT(*) FROM DepTran_Attendce WHERE CourseId=cc.Id AND Status=4 AND DepartSetId=@DepartId) AS zaotuiCount,(SELECT COUNT(*) FROM DepTran_Attendce WHERE CourseId=cc.Id AND Status=5 AND DepartSetId =@DepartId) AS chizaoCount FROM Co_Course cc LEFT JOIN DepTran_DepartLeaderConfig ddlc ON ddlc.Id=@DepartId WHERE cc.IsDelete=0 and cc.AttFlag>0 AND cc.CourseFrom=2 and cc.Publishflag=1 AND cc.Way=1 AND cc.IsRT=1 and " + where + " ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    DepartId = depId
                };
                return conn.Query<AttendceCourseModel>(sql, param).ToList();
            }
        }

        public List<Dep_Attendce> GetOneUserList(int userId, int departId, int startIndex = 1, int startLength = int.MaxValue,
                                                     string where = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(
                    "SELECT * FROM ( " +
                    " select row_number()OVER(ORDER BY dco.CourseId  DESC) num,count(*)OVER(PARTITION BY null) totalcount," +
                    " dc.CourseName,dc.CourseLength,dc.IsMust,dc.StartTime,dc.EndTime,dcr.RoomName," +
                    " dc.IsYearPlan,isnull(dco.AttScore,0) AttScore," +
                    " isnull(dco.EvlutionScore,0) EvlutionScore,isnull(dco.ExamScore,0) ExamScore,da.Status," +
                    " su.Realname " +
                    " from Dep_CourseOrder dco" +
                    " left join Dep_Course dc on dc.Id=dco.CourseId" +
                    " left join Dep_ClassRoom dcr on dcr.Id=dc.RoomId" +
                    " left join Dep_Attendce da on da.UserId=dco.UserId and da.CourseId=dco.CourseId" +
                    " left join Sys_User su ON dc.Teacher=su.UserId" +
                    " where " +
                    " ( " +
                    " ((select count(*) from Dep_Course where OpenFlag=3)>0 and " +
                    " (select count(*) from Sys_GroupUser where UserId=@userId " +
                    " and groupid in (select id from dbo.F_SplitIDs(dc.OpenGroupObject)))>0 " +
                    " or" +
                    " (select count(*) from Sys_User where DeptId=@departId and deptid in " +
                    " (select id from dbo.F_SplitIDs(dc.OpenDepartObject)))>0) " +
                    " or" +
                    " ((select count(*) from Dep_Course where OpenFlag=1)>0 " +
                    " and" +
                    " (select count(*) from Sys_GroupUser where UserId=@userId and groupid in " +
                    " (select id from dbo.F_SplitIDs(dc.OpenGroupObject)))>0) " +
                    " or" +
                    " ((select count(*) from Dep_Course where OpenFlag=2)>0 " +
                    " and" +
                    " (select count(*) from Sys_User where DeptId=@departId and deptid in " +
                    " (select id from dbo.F_SplitIDs(dc.OpenDepartObject)))>0) " +
                    " ) and " + where +
                    " ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex");
                var param = new
                    {
                        startIndex = startIndex,
                        startLength = startLength,
                        userId = userId,
                        departId = departId,
                        where = where
                    };
                return connection.Query<Dep_Attendce>(sql.ToString(), param).ToList();
            }
        }

        /// <summary>
        /// 获得消息发送人员信息
        /// </summary>
        public List<Message> GetUserinfo(int CourseId, string Users)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT tab.*,su.Realname AS Leadname,su.MobileNum,su.Email,cc.CourseName FROM (SELECT UserId,Realname,LeaderID FROM Sys_User WHERE UserId IN (" + Users + ")) AS tab LEFT JOIN Sys_User AS su ON su.JobNum=tab.LeaderID LEFT JOIN Dep_Course AS cc ON cc.Id=@CourseId ";
                var param = new
                {
                    CourseId = CourseId
                };
                return conn.Query<Message>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取人的考勤详情
        /// </summary>
        /// <returns></returns>
        public List<DepTran_CourseOrder> GetAttendceUserList(int courseId, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT row_number()OVER(ORDER BY su.UserId  DESC) num,count(*)OVER(PARTITION BY null) totalcount,da.*,Realname,DeptName FROM   DepTran_Attendce   da
LEFT JOIN Sys_User su ON su.UserId=da.UserId
WHERE CourseId={0}) r
WHERE  num BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex", courseId);
                var param = new
                   {
                       startIndex = startIndex,
                       startLength = startLength
                   };
                return conn.Query<DepTran_CourseOrder>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 设置ApprovalFlag为1
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public bool UpdatePersonalAttendce(int courseId, int userId, int departId)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"update DepTran_Attendce set ApprovalFlag = 1 where CourseId={0} and UserId={1} and DepartSetId={2}",courseId,userId,departId);
                return conn.Execute(sql) > 0;
            }
        }


        /// <summary>
        /// 验证考勤状态为待考勤的人员个数
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public int ValidaDepAttStatus(int courseid, int deptid)
        {
            using (var conn = OpenConnection())
            {
                string sqlstr = @"SELECT COUNT(*) FROM (
SELECT dco.Id,dco.UserId,suser.DeptId,isnull(datt.Status,0) AS Status FROM DepTran_CourseOrder AS dco 
LEFT JOIN DepTran_Attendce AS datt ON (dco.CourseId=datt.CourseId AND dco.UserId=datt.UserId) 
LEFT JOIN Sys_User AS suser ON dco.UserId=suser.UserId
WHERE dco.CourseId=@CourseId AND suser.DeptId=@DeptId ) AS temp WHERE temp.Status=0";

                var param = new
                {
                    CourseId = courseid,
                    DeptId = deptid
                };
                return conn.Query<int>(sqlstr, param).FirstOrDefault();
            }
        }
    }
}
