using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.DepTranManage;
using Retech.Core;
using Retech.Data;
using System.Data;
using LiXinModels.DepAttendce;
using LiXinModels.DepCourseManage;
using LiXinModels.User;

namespace LiXinDataAccess.DepAttendce
{
    public class Dep_AttendceDB : BaseRepository
    {
        #region 增改(基本信息)
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertDep_Attendce(Dep_Attendce model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Dep_Attendce(CourseId,UserId,StartTime,EndTime,Status,ApprovalFlag,Reason,DepartSetId) VALUES (@CourseId,@UserId,@StartTime,@EndTime,@Status,@ApprovalFlag,@Reason,@DepartSetId);SELECT @@IDENTITY as Id ";

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
        public bool UpdateDep_Attendce(Dep_Attendce model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update Dep_Attendce set CourseId = @CourseId,UserId = @UserId,StartTime = @StartTime,EndTime = @EndTime,Status = @Status,ApprovalFlag = @ApprovalFlag,Reason = @Reason,DepartSetId=@DepartSetId where Id=@Id";

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
        public Dep_Attendce GetDep_Attendce(int courseID, int userID)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from Dep_Attendce where CourseId=@CourseId and UserId=@UserId ");
                var param = new { CourseId = courseID, UserId = userID };
                return connection.Query<Dep_Attendce>(query, param).FirstOrDefault();
            }
        }

        #endregion

        /// <summary>
        /// 获得数据列表(有分页)
        /// </summary>
        public List<DepAttendceCourseModel> GetDep_AttendceList(string depId, int startIndex = 1, int startLength = int.MaxValue, string Order = "StartTime DESC", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql =string.Format(@"SELECT * FROM ( SELECT row_number()OVER(ORDER BY " + Order + @") num,
count(*)OVER(PARTITION BY null) totalcount,* FROM (SELECT cc.DeptId,cc.Id AS CourseID,CourseName, isnull(cc.Way,0) AS Way,CourseLength,Teacher,IsMust,StartTime,EndTime,su.Realname as TeacherName,sc.RoomName,
 isnull(dtd.AttFlag,0) AS AttFlag,isnull(dtd.ApprovalFlag,0) as ApprovalFlag 
 FROM Dep_Course cc 
 LEFT JOIN Sys_User su ON su.UserId=cc.Teacher 
 LEFT JOIN Dep_ClassRoom sc ON sc.Id=cc.RoomId 
 LEFT JOIN Dep_CourseDept dtd ON dtd.CourseId=cc.Id AND dtd.DepartId in ({0})  
 WHERE cc.IsDelete=0 AND cc.CourseFrom=2 and cc.Publishflag=1 AND cc.way in (1,3) and cc.IsOpenOthers IN (0,3) and cc.AttFlag=1  
 AND (cc.DeptId in ({0}) OR dbo.CGF_FN_Search(cc.OpenDepartObject,'" + depId.Split(',')[0] + @"',',')=1)) AS temp where " + where + @") AS result where num BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex", depId);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    DepartId =depId
                };
                return conn.Query<DepAttendceCourseModel>(sql,param).ToList();
            }
        }

        /// <summary>
        /// 获得数据列表(有分页) 课前建议方法
        /// </summary>
        public List<DepAttendceCourseModel> GetDep_AttendceListForAdvice(string depId, int startIndex = 1, int startLength = int.MaxValue, string Order = "StartTime DESC", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT * FROM ( SELECT row_number()OVER(ORDER BY " + Order + @") num,
count(*)OVER(PARTITION BY null) totalcount,* FROM (SELECT cc.DeptId,cc.Id AS CourseID,CourseName, isnull(cc.Way,0) AS Way,CourseLength,Teacher,IsMust,StartTime,EndTime,su.Realname as TeacherName,sc.RoomName,
 isnull(dtd.AttFlag,0) AS AttFlag,isnull(dtd.ApprovalFlag,0) as ApprovalFlag 
 FROM Dep_Course cc 
 LEFT JOIN Sys_User su ON su.UserId=cc.Teacher 
 LEFT JOIN Dep_ClassRoom sc ON sc.Id=cc.RoomId 
 LEFT JOIN Dep_CourseDept dtd ON dtd.CourseId=cc.Id AND dtd.DepartId in ({0})  
 WHERE cc.IsDelete=0 AND cc.CourseFrom=2 and cc.Publishflag=1 AND cc.way in (1,3) and cc.IsOpenOthers IN (0,3) 
 AND (cc.DeptId in ({0}) OR dbo.CGF_FN_Search(cc.OpenDepartObject,'" + depId.Split(',')[0] + @"',',')=1)) AS temp where " + where + @") AS result where num BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex", depId);
                //and cc.AttFlag=1  
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    DepartId = depId
                };
                return conn.Query<DepAttendceCourseModel>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        public List<Dep_CourseOrder> GetDep_AttendUserList(int courseId, int depId, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY dtco.Id " + Order + @") num,
count(*)OVER(PARTITION BY null) totalcount,
(SELECT count(*) FROM Dep_CourseOrder WHERE CourseId=@CourseId AND OrderStatus=1 AND DepartSetId=@DepartId) AS OrderCount,
(SELECT count(*) FROM Dep_Attendce WHERE CourseId=@CourseId and Status>0 AND DepartSetId=@DepartId) AS AttCount,
dtco.*,su.Realname,su.DeptName,dta.Status,dtcop.AttFlag,dtcop.ApprovalFlag as CourseApprovalFlag,su.MobileNum as Telephone,su.Email,su.Sex 
FROM Dep_CourseOrder AS dtco 
LEFT JOIN Dep_Attendce AS dta ON (dta.CourseId=dtco.CourseId AND dta.UserId=dtco.UserId) 
LEFT JOIN Dep_CourseDept AS dtcop on (dtcop.CourseId=dtco.CourseId AND dtco.DepartSetId=dtcop.DepartId)  
LEFT JOIN Sys_User AS su ON su.UserId=dtco.UserId 
WHERE dtco.CourseId=@CourseId and dtco.OrderStatus>0 AND dtco.DepartSetId=@DepartId and (dtco.IsLeave!=1 or 
(dtco.IsLeave=1 and dtco.ApprovalFlag!=1)) and " + where + @" ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    CourseId = courseId,
                    DepartId = depId
                };
                return conn.Query<Dep_CourseOrder>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 获得考勤人员数据列表 倒出execl专用方法 2013-10-21 毛佳源添加
        /// </summary>
        public List<Dep_CourseOrder> GetDep_AttendUserListForExport(int courseId, int depId, string Order = "asc", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY dtco.Id " + Order + @") num,
count(*)OVER(PARTITION BY null) totalcount,
(SELECT count(*) FROM Dep_CourseOrder WHERE CourseId=@CourseId AND OrderStatus=1 AND DepartSetId=@DepartId) AS OrderCount,
(SELECT count(*) FROM Dep_Attendce WHERE CourseId=@CourseId and Status>0 AND DepartSetId=@DepartId) AS AttCount,
dtco.*,su.Realname,su.DeptName,su.TrainGrade,su.CPA,su.CPANo,dta.Status,dtcop.AttFlag,dtcop.ApprovalFlag as CourseApprovalFlag,su.MobileNum as Telephone,su.Email,su.Sex
FROM Dep_CourseOrder AS dtco 
LEFT JOIN Dep_Attendce AS dta ON (dta.CourseId=dtco.CourseId AND dta.UserId=dtco.UserId) 
LEFT JOIN Dep_CourseDept AS dtcop on (dtcop.CourseId=dtco.CourseId AND dtco.DepartSetId=dtcop.DepartId)  
LEFT JOIN Sys_User AS su ON su.UserId=dtco.UserId 
WHERE dtco.CourseId=@CourseId and dtco.OrderStatus>0 AND dtco.DepartSetId=@DepartId and (dtco.IsLeave!=1 or 
(dtco.IsLeave=1 and dtco.ApprovalFlag!=1)) and " + where + @" ) result ";
                var param = new
                {                   
                    CourseId = courseId,
                    DepartId = depId
                };
                return conn.Query<Dep_CourseOrder>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得群组人员数据列表
        /// </summary>
        public List<Sys_User> GetDeptUserList(int courseId, int depId, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY UserId) num,count(*)OVER(PARTITION BY null) totalcount,* 
FROM Sys_User WHERE UserId NOT IN 
(SELECT UserId FROM Dep_CourseOrder WHERE CourseId=@CourseId and OrderStatus>0 AND DepartSetId=@DepartId) 
AND DeptId=@DepartId and " + where + @" ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    CourseId = courseId,
                    DepartId = depId
                };
                return conn.Query<Sys_User>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 验证人员是否满足导入条件
        /// </summary>
        public int IsExistAttendce(int userid,int depId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = @"SELECT count(*) FROM Sys_User WHERE UserId=@UserId AND DeptId=@DeptId AND IsDelete=0";
                var param = new
                {
                    UserId = userid,
                    DeptId = depId
                };
                return connection.Query<int>(sql, param).FirstOrDefault();
            }
        }


        /// <summary>
        /// 获得差异数据列表(部门)
        /// </summary>
        public List<DepAttendceCourseModel> GetDifferenceList(int depId, int startIndex = 1, int startLength = int.MaxValue, string Order = "cc.StartTime DESC", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY " + Order + @") num,
count(*)OVER(PARTITION BY null) totalcount,
cc.Id AS CourseID,CourseName,IsMust,StartTime,EndTime,sd.DeptName AS DepartSetName,sd.DeptCode,
(SELECT COUNT(*) FROM Dep_CourseOrder WHERE CourseId=cc.Id and OrderStatus>0 AND (IsLeave!=1 or 
(IsLeave=1 and ApprovalFlag!=1)) AND DepartSetId=@DepartId) AS CoCount,
(SELECT COUNT(*) FROM Dep_Attendce WHERE CourseId=cc.Id AND Status>0 AND DepartSetId=@DepartId) AS AttCount,
(SELECT COUNT(*) FROM Dep_Attendce WHERE CourseId=cc.Id AND Status=1 AND DepartSetId=@DepartId) AS AttOkCount,
(SELECT COUNT(*) FROM Dep_Attendce WHERE CourseId=cc.Id AND Status=2 AND DepartSetId=@DepartId) AS AttNOCount,
(SELECT COUNT(*) FROM Dep_Attendce WHERE CourseId=cc.Id AND Status=3 AND DepartSetId=@DepartId) AS chidaoCount,
(SELECT COUNT(*) FROM Dep_Attendce WHERE CourseId=cc.Id AND Status=4 AND DepartSetId=@DepartId) AS zaotuiCount,
(SELECT COUNT(*) FROM Dep_Attendce WHERE CourseId=cc.Id AND Status=5 AND DepartSetId=@DepartId) AS chizaoCount 
FROM Dep_Course cc LEFT JOIN Sys_Department AS sd ON sd.DepartmentId=@DepartId 
WHERE cc.IsDelete=0 AND cc.CourseFrom=2 and cc.Publishflag=1 and cc.AttFlag=1 AND cc.IsOpenOthers IN (0,3) AND cc.way in (1,3)  
AND (cc.DeptId=@DepartId OR dbo.CGF_FN_Search(cc.OpenDepartObject,'" + depId + @"',',')=1) and " + where + @" ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    DepartId = depId
                };
                return conn.Query<DepAttendceCourseModel>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得差异数据列表(主办人)
        /// </summary>
        public List<DepAttendceCourseModel> GetDifferenceListByCo(int cId, int startIndex = 1, int startLength = int.MaxValue, string Order = "cc.StartTime DESC", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY " + Order + @") num,
count(*)OVER(PARTITION BY null) totalcount,
cc.Id AS CourseID,CourseName,IsMust,StartTime,EndTime,sd.DeptName AS DepartSetName,sd.DeptCode,
(SELECT COUNT(*) FROM Dep_CourseOrder WHERE CourseId=cc.Id and OrderStatus>0 AND (IsLeave!=1 or 
(IsLeave=1 and ApprovalFlag!=1)) AND DepartSetId=sd.DepartmentId) AS CoCount,
(SELECT COUNT(*) FROM Dep_Attendce WHERE CourseId=cc.Id AND Status>0 AND DepartSetId=sd.DepartmentId) AS AttCount,
(SELECT COUNT(*) FROM Dep_Attendce WHERE CourseId=cc.Id AND Status=1 AND DepartSetId=sd.DepartmentId) AS AttOkCount,
(SELECT COUNT(*) FROM Dep_Attendce WHERE CourseId=cc.Id AND Status=2 AND DepartSetId=sd.DepartmentId) AS AttNOCount,
(SELECT COUNT(*) FROM Dep_Attendce WHERE CourseId=cc.Id AND Status=3 AND DepartSetId=sd.DepartmentId) AS chidaoCount,
(SELECT COUNT(*) FROM Dep_Attendce WHERE CourseId=cc.Id AND Status=4 AND DepartSetId=sd.DepartmentId) AS zaotuiCount,
(SELECT COUNT(*) FROM Dep_Attendce WHERE CourseId=cc.Id AND Status=5 AND DepartSetId=sd.DepartmentId) AS chizaoCount 
 FROM Sys_Department AS sd,Dep_Course AS cc WHERE (sd.DepartmentId=cc.DeptId  
or dbo.CGF_FN_Search(cc.OpenDepartObject,sd.DepartmentId,',')=1) AND cc.Id=@CourseId and " + where + @" ) result WHERE  num BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    CourseId = cId
                };
                return conn.Query<DepAttendceCourseModel>(sql, param).ToList();
            }
        }

        public DepAttendceCourseModel GetCl_Attendce(int CourseId, int UserId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = "select * from Dep_Attendce where CourseId=@CourseId and UserId=@UserId";
                return connection.Query<DepAttendceCourseModel>(sqlstr, new { CourseId = CourseId, UserId = UserId }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 设置个人ApprovalFlag为1
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdatePersonalAttendce(int courseId, int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr =
                    "update Dep_Attendce set  ApprovalFlag = 1 where CourseId=@courseId and UserId=@userId";
                var param = new
                    {
                        courseId=courseId,
                        userId=userId
                    };
                return connection.Execute(sqlstr, param) > 0;
            }
        }


        /// <summary>
        /// 获得课程数据列表(有分页)
        /// </summary>
        public List<DepAttendceCourseModel> GetDep_AttendceListByUser(int userId, int startIndex = 1, int startLength = int.MaxValue, string Order = "StartTime DESC", string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {

                string sql = @"SELECT * FROM ( SELECT row_number()OVER(ORDER BY "+Order+ @") num,
count(*)OVER(PARTITION BY null) totalcount,cc.Id AS CourseID,CourseName,isnull(cc.Way,0) AS Way,CourseLength,Teacher,IsMust,StartTime,EndTime,su.Realname as TeacherName,sc.RoomName 
 FROM Dep_Course cc 
 LEFT JOIN Sys_User su ON su.UserId=cc.Teacher 
 LEFT JOIN Dep_ClassRoom sc ON sc.Id=cc.RoomId  
 WHERE cc.IsDelete=0 AND cc.CourseFrom=2 and cc.Publishflag=1 and cc.AttFlag=0 AND cc.way in (1,3) AND cc.IsOpenOthers IN (0,3) 
 AND dbo.CGF_FN_Search(cc.AttUserId,'" + userId + @"',',')=1 AND " + where + @") AS result 
where num BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<DepAttendceCourseModel>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 根据CourseID获得部门数据
        /// </summary>
        public List<Sys_Department> GetDepartByCId(int cID)
        {
            using (var conn = OpenConnection())
            {
                string sqlstr = @"SELECT * FROM Sys_Department WHERE DepartmentId=(SELECT DeptId FROM Dep_Course WHERE Id="+cID+@") 
or dbo.CGF_FN_Search((SELECT OpenDepartObject FROM Dep_Course WHERE Id="+cID+@"),DepartmentId,',')=1";
                return conn.Query<Sys_Department>(sqlstr).ToList();
            }
        }

        /// <summary>
        /// 验证考勤状态为待考勤的人员个数
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public int ValidaAttStatus(int courseid, int deptid)
        {
            using (var conn = OpenConnection())
            {
                string sqlstr = @"SELECT COUNT(*) FROM (
SELECT dco.Id,dco.UserId,suser.DeptId,isnull(datt.Status,0) AS Status FROM Dep_CourseOrder AS dco 
LEFT JOIN Dep_Attendce AS datt ON (dco.CourseId=datt.CourseId AND dco.UserId=datt.UserId) 
LEFT JOIN Sys_User AS suser ON dco.UserId=suser.UserId
WHERE dco.OrderStatus=1 and dco.CourseId=@CourseId AND suser.DeptId=@DeptId ) AS temp WHERE temp.Status=0";

                var param = new
                {
                    CourseId = courseid,
                    DeptId = deptid
                };
                return conn.Query<int>(sqlstr, param).FirstOrDefault();
            }
        }


        /// <summary>
        /// 删除考勤记录
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        public void DeleteAttendForCourseAndUser(int courseid,int userid)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string sql = "delete Dep_Attendce where CourseId=" + courseid+"and Userid="+userid;


                connection.Execute(sql);
            }
        }


        /// <summary>
        /// 删除预定记录
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        public void DeleteCourseOrderForCourseAndUser(int courseid, int userid)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string sql = "delete Dep_courseorder where CourseId=" + courseid + "and Userid=" + userid;


                connection.Execute(sql);
            }
        }
    }
}
