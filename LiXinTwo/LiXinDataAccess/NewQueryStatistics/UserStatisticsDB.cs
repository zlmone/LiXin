using LiXinModels.NewCourseManage;
using LiXinModels.NewQueryStatistics;
using LiXinModels.User;
using Retech.Core;
using Retech.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LiXinDataAccess.NewQueryStatistics
{
    public class UserStatisticsDB : BaseRepository
    {

        /// <summary>
        /// 获取得分，总数
        /// </summary>
        /// <returns></returns>
        public List<ShowScore> GetScoreList(string where = " 1=1", string StartTime="")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH together AS
(
	SELECT UserID,Count(CourseID) togetherCount,Sum(TogetherScore) togetherScore, 0 groupCount,0  groupScore FROM    view_CourseOrderInfo vc
	WHERE (IsGroupTeach=1 OR IsGroupTeach=3) and  1=1
	AND   CourseID IN (SELECT Id FROM New_Course WHERE StartTime>'{1}')
	GROUP BY UserID
 ),
 groups AS
 (
    SELECT UserID,0 togetherCount,0 togetherScore,Count(CourseID) groupCount,Sum(GroupScore) groupScore FROM    view_CourseOrderInfo vc
	WHERE (IsGroupTeach=2  OR  IsGroupTeach=3) 
    AND   CourseID IN (SELECT Id FROM New_Course WHERE StartTime>'{1}')
	GROUP BY UserID
 )

 
 SELECT   t.UserID, together.togetherCount ,together.togetherScore, isnull(groups. groupCount,0) groupCount,isnull(groups. groupScore,0) groupScore from
 (
     SELECT UserID FROM together
     UNION
     SELECT UserID FROM   groups
) t 
LEFT JOIN  together on t.UserID=together.UserID
LEFT JOIN  groups on t.UserID=groups.UserID
where {0}", where, StartTime == "" ? DateTime.Now.Year + "-01-01" : StartTime);
                return conn.Query<ShowScore>(sql).ToList();
            }
        }

        /// <summary>
        /// 课程，独立考试得分总分
        /// </summary>
        /// <returns></returns>
        public List<ShowScore> GetExamCourseScore(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@" WITH learnList AS
 (
       SELECT UserId, count(CourseId) Count, sum(CourseExamScore) Score,sum( CourseExamSumScore) sumScore,
examScore=(SELECT Score FROM New_UserExamScore WHERE UserID=nco.UserId),
examSumScore=(SELECT SumScore FROM New_UserExamScore WHERE UserID=nco.UserId)
 FROM New_CourseOrder  nco
where 1=1
GROUP BY UserId
 )

       SELECT * FROM(
       SELECT distinct UserId,0 count,0 Score,0 sumScore,
       examScore=(SELECT Score FROM New_UserExamScore WHERE UserID=New_CpaLearnStatus.UserId),
      examSumScore=(SELECT SumScore FROM New_UserExamScore WHERE UserID=New_CpaLearnStatus.UserId) 
      FROM   New_CpaLearnStatus
       WHERE UserID NOT IN (SELECT UserId FROM learnList)
        UNION 
        SELECT * FROM learnList
             ) t
             WHERE  {0}", where);
                return conn.Query<ShowScore>(sql).ToList();
            }
        }

        /// <summary>
        /// 获得评价的次数
        /// </summary>
        /// <returns></returns>
        public List<ShowScore> GetSurveyList(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT  UserID,Count(CourseRoomRuleId)   Count FROM
(
	SELECT distinct userID, CourseRoomRuleId FROM Survey_ReplyAnswer
	WHERE SourceFrom=0 AND CourseRoomRuleId IS NOT NULL AND CourseRoomRuleId!=0 and  {0}
)  r GROUP BY  UserID", where);
                return conn.Query<ShowScore>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取课程的基本信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetCourseList(int userID, int startIndex = 1, int startLength = int.MaxValue,string where="1=1")
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT * FROM (
SELECT row_number()OVER(ORDER BY nco.CourseId  desc) number,count(*)OVER(PARTITION BY null) totalcount, CourseName, StartTime, EndTime,
AttendCount=(SELECT count(*) FROM New_CourseOrderDetail  nco 
LEFT JOIN New_CourseRoomRule ncr  ON ncr.Id=nco.SubCourseID  
WHERE nco.CourseId=nc.Id AND nco.UserId=@userID AND ncr.IsAttFlag<3 AND nco.IsLeave=0),
HaveAttend=(SELECT COUNT(*) FROM New_Attendce  na
LEFT JOIN New_CourseRoomRule ncr ON ncr.Id=na.SubCourseID
WHERE na.CourseId=nc.Id AND na.UserId=@userID AND ncr.IsAttFlag<3),
nc.Id, ClassId, UserId, OrderTime, LearnStatus, TogetherScore, GroupScore, CourseExamScore, ExamScore, TogetherMemo, GroupMemo, Type, CourseExamSumScore, ExamSumScore
,nc.IsTest,  nc.Id CourseId, nc.Way,
IsReward=(SELECT COUNT(*) FROM Survey_ReplyAnswer WHERE SourceFrom=0 AND ObjectID=nc.Id 
AND CourseRoomRuleId IS NOT NULL  AND UserID=@userID)
,ncp.PaperId,ncp.Id CoPaperID,IsGroupTeach
FROM New_Course nc LEFT JOIN New_CourseOrder nco
ON nc.Id=nco.CourseId AND nco.UserId=@userID
LEFT JOIN New_CoursePaper ncp 
ON ncp.CourseId=nc.Id
WHERE nc.Id IN
(
 SELECT CourseId FROM New_CourseOrder
  WHERE UserId=@userID
  UNION
  SELECT  CourseId FROM   New_CpaLearnStatus
  WHERE UserId=@userID
)
and {0}
) t
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where);
                var param = new
                {
                    userID = userID,
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<New_CourseOrder>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取考勤的详情
        /// </summary>
        /// <returns></returns>
        public List<New_CourseOrderDetail> GetAttendceList(int startIndex = 1, int startLength = int.MaxValue, string where = " 1=1", string jsRenderSortField = " ncod.UserId")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT row_number()OVER(ORDER BY {0}  desc) number,count(*)OVER(PARTITION BY null) totalcount, ncod.CourseId,ncod.UserId, ncrr.StartTime AS CoStartTime,ncrr.EndTime AS CoEndTime,na.StartTime,na.EndTime,IsAttFlag ,
ncs.CourseName,na.Id attId,
(SELECT count(*) FROM New_Attendce WHERE SubCourseID=ncod.SubCourseID)  AS AttCount
FROM New_CourseOrderDetail AS ncod 
LEFT JOIN New_Attendce  AS na ON (na.CourseId=ncod.CourseId AND na.SubCourseID=ncod.SubCourseID AND na.UserId=ncod.UserId) 
LEFT JOIN New_CourseRoomRule AS ncrr ON ncrr.Id=ncod.SubCourseID
LEFT JOIN New_Course ncs on  ncs.Id=ncod.CourseId
where ncod.IsLeave=0  and  {1}
) result
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", jsRenderSortField, where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<New_CourseOrderDetail>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 取得所有有学习记录的学员，加上所有的新进员工（并集）
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetAllStudyUser(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT UserId, NumberID, Realname,InternDept,DeptName FROM Sys_User
WHERE UserId IN(
SELECT DISTINCT  UserId FROM New_CourseOrder
UNION 
SELECT UserId FROM Sys_User
WHERE IsNew=1 AND Status=0 AND IsDelete=0 
) and {0}", where);
                return conn.Query<Sys_User>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取学员所学的课程(包括视频)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetCourserIDList()
        {
            using (var conn = OpenConnection())
            {
                var sql = @" SELECT CourseId, UserId,way FROM New_CourseOrder nco
LEFT JOIN New_Course nc ON nco.CourseId=nc.Id
WHERE  nc.IsDelete=0 
 
  UNION
  SELECT  CourseId, UserID,way  FROM   New_CpaLearnStatus  ncs
    LEFT JOIN New_Course nc ON ncs.CourseId=nc.Id
WHERE  nc.IsDelete=0" ;
                return conn.Query<New_CourseOrder>(sql).ToList();
            }
        }


    }
}
