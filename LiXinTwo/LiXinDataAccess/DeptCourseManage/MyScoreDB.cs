using LiXinModels.DeptCourseManage;
using LiXinModels.User;
using Retech.Core;
using Retech.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace LiXinDataAccess
{
    public class MyScoreDB : BaseRepository
    {
        /// <summary>
        /// 获取我的学时及考勤获取
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<CourseShow> GetCourseShow(int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue, string jsrendersortfield = " userID")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select * from (
 SELECT row_number()OVER(ORDER BY {1} ) number,count(*)OVER(PARTITION BY null) totalcount, * FROM(
 SELECT DISTINCT cc.Id,cc.Id as CourseID,p.Id as CoPaperID,p.PaperId as PaperId, CourseName, IsMust, dbo.GetRealNameByUserID(Teacher) Teacher, cc.StartTime, cc.EndTime, CourseLength,
 RoomName,
 GetSumScore=AttScore+EvlutionScore+ExamScore,
 dao.Status,IsYearPlan,dco.userID,dao.ApprovalFlag, isnull(t.OpenApprovalFlag,0) OpenApprovalFlag,isnull(t.AttFlag,0) AttFlag
FROM DepTran_CourseOrder dco
LEFT JOIN Co_Course cc  ON dco.CourseId=cc.Id 
LEFT JOIN Sys_ClassRoom scr  ON cc.RoomId=scr.Id
LEFT JOIN DepTran_Attendce dao ON dao.CourseId=cc.Id AND dao.UserId=dco.UserId 
LEFT JOIN Co_CoursePaper p on cc.Id=p.CourseId
LEFT JOIN 
    (  SELECT ApprovalFlag  OpenApprovalFlag, AttFlag,CourseId FROM DepTran_CourseOpen WHERE DepartId=( SELECT DepartSetId  FROM DepTran_DepartUser WHERE UserId=@userID) 
    )  t   ON t.CourseId= cc.Id
WHERE dco.UserId=@userID  and cc.Id IN (
SELECT CourseId FROM DepTran_CourseOrder
WHERE UserId=@userID
and OrderStatus>0)  and {0}
 ) result
 )t
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
        public List<Sys_User> GetDeptUserScore(int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " Realname asc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
WITH departIDs AS
(
   SELECT Id FROM DepTran_DepartLeaderConfig
   WHERE ( ( SELECT count(*) FROM dbo.F_SplitIDs(MainUserIDs)    WHERE ID={0})>0)
)

SELECT * FROM(
SELECT row_number()OVER(ORDER BY {2} ) number,count(*)OVER(PARTITION BY null) totalCount,* FROM(
SELECT   UserId, Realname, Sex, MobileNum, Email, TrainGrade,CPA, 
GetScore=(SELECT isnull(sum(AttScore+EvlutionScore+ExamScore),0) FROM DepTran_CourseOrder  dco
LEFT JOIN DepTran_Attendce dao ON dao.CourseId=dco.CourseId AND dao.UserId=dco.UserId 
WHERE dco.DepartSetId IN(SELECT Id FROM departIDs )  AND dco.userID=Sys_User.UserId and OrderStatus>0
 AND ApprovalFlag =1 and (AttScore>0 or EvlutionScore>0 or ExamScore>0) 
)   
 FROM Sys_User
WHERE UserId IN (
SELECT DISTINCT UserId  FROM DepTran_DepartUser
WHERE DepartSetId IN(SELECT Id FROM departIDs))
 AND IsDelete=0 AND   Status=0  AND {1}
)result  
) t  
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", userID, where, jsRenderSortField);
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
 SELECT row_number()OVER(ORDER BY dco.userID) number,count(*)OVER(PARTITION BY null) totalcount,cc.Id, CourseName, IsMust, cc.StartTime, cc.EndTime, CourseLength,
 RoomName,
 dbo.f_GetTeacherName(cc.Teacher) Teacher,
 GetSumScore=CASE WHEN  dao.ApprovalFlag=1 THEN AttScore+EvlutionScore+ExamScore ELSE 0 END,
 dao.Status,dao.ApprovalFlag
FROM DepTran_CourseOrder dco
LEFT JOIN Co_Course cc  ON dco.CourseId=cc.Id 
LEFT JOIN Sys_ClassRoom scr  ON cc.RoomId=scr.Id
LEFT JOIN DepTran_Attendce dao ON dao.CourseId=cc.Id AND dao.UserId=dco.UserId 
WHERE dco.UserId={1}  and cc.Id IN (
SELECT CourseId FROM DepTran_CourseOrder
WHERE UserId={1} and OrderStatus>0)
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
    }
}
