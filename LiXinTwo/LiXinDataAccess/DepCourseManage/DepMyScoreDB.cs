using LiXinModels.DeptCourseManage;
using LiXinModels.User;
using Retech.Core;
using Retech.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace LiXinDataAccess.DepCourseManage
{
    public class DepMyScoreDB : BaseRepository
    {
        /// <summary>
        /// 获取我的学时及考勤获取
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<CourseShow> GetCourseShow(int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue, string jsrendersortfield = " userID asc,EndTime desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
   WITH info AS 
   (
   SELECT * FROM (
 SELECT DISTINCT cc.Id,cc.Id as CourseID,p.Id as CoPaperID,p.PaperId as PaperId, CourseName, IsMust, dbo.GetRealNameByUserID(Teacher) Teacher, cc.StartTime, cc.EndTime, CourseLength,
 RoomName,
 GetSumScore=AttScore+EvlutionScore+ExamScore,
 dao.Status,dco.userID,dao.ApprovalFlag,att.OpenApprovalFlag,att.AttFlag,cc.YearPlan
FROM Dep_CourseOrder dco
LEFT JOIN Dep_Course cc  ON dco.CourseId=cc.Id 
LEFT JOIN Dep_ClassRoom scr  ON cc.RoomId=scr.Id
LEFT JOIN Dep_Attendce dao ON dao.CourseId=cc.Id AND dao.UserId=dco.UserId 
LEFT JOIN Dep_Coursepaper p on cc.Id=p.CourseId
LEFT JOIN 
    (  SELECT ApprovalFlag  OpenApprovalFlag, AttFlag,CourseId FROM Dep_CourseDept WHERE DepartId=
     ( SELECT DeptId  FROM Sys_User WHERE UserId=@userID) 
    )  att  ON att.CourseId= cc.Id
WHERE dco.UserId=@userID  and cc.Id IN (
SELECT CourseId FROM Dep_CourseOrder
WHERE UserId=@userID
) 
 UNION 
 SELECT DepCourseId AS Id,DepCourseId AS  CourseID,0,0,CourseName,IsMust,teacherName as  Teacher,StartTime,EndTime,CourseLength,
 RoomName,GetScore AS GetSumScore,99,UserId,-1,-1,-1,YearPlan FROM View_DepCourseLearn WHERE UserId=@userID
 ) r WHERE  {0} 
  )
  
  select  * from (
 SELECT row_number()OVER(ORDER BY  {1}) number,count(*)OVER(PARTITION BY null) totalcount,*,sum(GetSumScore)OVER(PARTITION BY null) percenterSumScore
 ,mustScore=(SELECT  sum(GetSumScore)  FROM info WHERE IsMust=0),
 selectScore=(SELECT  sum(GetSumScore)  FROM info WHERE IsMust=1)
  from  info
 ) result
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where, jsrendersortfield);
                var param = new
                {
                    userID = userID,
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<CourseShow>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 部门/分所员工学时及考勤获取
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<Sys_User> GetDeptUserScore(int userID, string where = " 1=1", int year = -1, int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " Realname asc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@" DECLARE  @ManageDeparts NVARCHAR(200)
SELECT  @ManageDeparts=CASE WHEN TrainMaster=1 then ManageDeparts ELSE CAST(DeptId AS NVARCHAR(200)) END FROM Sys_User
WHERE UserId={0}

SELECT * FROM( 
SELECT row_number()OVER(ORDER BY {2}) number,count(*)OVER(PARTITION BY null) totalCount,* FROM(
SELECT   UserId, Realname, Sex, MobileNum, Email, TrainGrade,CPA
, GetScore=(SELECT sum(GetScore) FROM(
 SELECT isnull(sum(AttScore+EvlutionScore+ExamScore),0) GetScore FROM Dep_CourseOrder  dco
LEFT JOIN Dep_Attendce dao ON dao.CourseId=dco.CourseId AND dao.UserId=dco.UserId 
LEFT JOIN Dep_Course cc ON cc.Id=dco.CourseId
WHERE dco.DepartSetId IN (SELECT ID  FROM dbo.F_SplitIDs(@ManageDeparts))  AND dco.userID=Sys_User.UserId and OrderStatus>0
 AND dao.ApprovalFlag =1 {3}
 UNION
SELECT GetScore FROM View_DepCourseLearn  WHERE UserId= Sys_User.UserId {3})t)    
 FROM Sys_User
WHERE DeptId  IN (SELECT ID  FROM dbo.f_GetDeptChildByDeptID(@ManageDeparts))
AND IsDelete=0 AND   Status=0 And {1}
 )t ) result
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", 
 userID, where, jsRenderSortField, (year==-1?"":"AND YearPlan="+year));
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_User>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 部门/分所下人员的获取学时详情
        /// </summary>
        /// <param name="mainUserID">部门/分所的领导人</param>
        /// <param name="userID">要查看的人员ID</param>
        /// <returns></returns>
        public List<CourseShow> GetDepUserScoreDetails(int mainUserID, int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH departIDs AS
(
   SELECT Id FROM DepTran_DepartLeaderConfig
   WHERE ((SELECT count(*) FROM dbo.F_SplitIDs(MainUserIDs) WHERE ID={0})>0)
)
select * from (
 SELECT row_number()OVER(ORDER BY dco.userID) number,count(*)OVER(PARTITION BY null) totalcount,cc.Id, CourseName, IsMust, Teacher, cc.StartTime, cc.EndTime, CourseLength,
 RoomName,
 GetSumScore=AttScore+EvlutionScore+ExamScore,
 dao.Status
FROM DepTran_CourseOrder dco
LEFT JOIN Co_Course cc  ON dco.CourseId=cc.Id 
LEFT JOIN Sys_ClassRoom scr  ON cc.RoomId=scr.Id
LEFT JOIN DepTran_Attendce dao ON dao.CourseId=cc.Id AND dao.UserId=dco.UserId 
WHERE dco.UserId={1}  and cc.Id IN (
SELECT CourseId FROM DepTran_CourseOrder
WHERE UserId={1})
AND dco.DepartSetId IN (SELECT Id FROM departIDs)
and {2}
 ) result
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", mainUserID, userID, where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<CourseShow>(sql, param).ToList();

            }
        }

        /// <summary>
        /// 当前部门所管理的所有部门的年度
        /// </summary>
        /// <param name="deptID"></param>
        /// <returns></returns>
        public List<int> GetDepCourseYear(int UserId)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
SELECT distinct YearPlan FROM Dep_Course
WHERE IsDelete=0 AND Publishflag=1", UserId);
                return conn.Query<int>(sql).ToList();
            }
        }
    }
}
