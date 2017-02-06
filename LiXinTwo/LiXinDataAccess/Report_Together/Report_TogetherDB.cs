using Retech.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.Report_Together;
using LiXinModels.Report_zVedio;
using LiXinModels.Report_Vedio;

namespace LiXinDataAccess
{
    public class Report_TogetherDB : BaseRepository
    {

        #region 所有课程的参与、贡献度情况
        /// <summary>
        /// 所有课程的应参加人数等属性
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IEnumerable<ReportTogether> GetTogetherList(string where = " 1=1", string freeWhere=" 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@" WITH tbreally AS
 (
   --实际报名人数
  	SELECT CourseId,count(cco.UserId) reallyApply,syu.DeptId FROM Cl_CourseOrder cco
	LEFT JOIN Sys_User syu ON cco.UserId=syu.UserId 
	WHERE cco.[OrderStatus] in (1,3)
    and ( cco.[IsLeave] = 0 
        or (cco.[IsLeave] = 1 and cco.[ApprovalFlag] <>1)
        or (cco.[IsLeave] = 1 and cco.[ApprovalFlag] = 1 
        and cco.[ApprovalDateTime] > cco.[ApprovalLimitTime])
      )
    AND cco.UserId IN (SELECT distinct UserId FROM View_CheckUser) 
    AND CourseId IN (SELECT Id FROM Co_Course WHERE Way=1 AND CourseFrom=2  AND Publishflag=1 AND IsDelete=0)
    and {1}
    GROUP BY   CourseId,syu.DeptId
 ),tbAttendce AS
 (
   --课程有考勤的人数
    SELECT CourseId,reallyJoin=Count(DISTINCT cac.UserId),syu.DeptId FROM  Cl_Attendce  cac
	LEFT JOIN Co_Course cc ON cac.CourseId=cc.Id
	 LEFT JOIN Sys_User syu ON cac.UserId=syu.UserId 
	WHERE Way=1 AND CourseFrom=2 
	AND Publishflag=1 AND cc.IsDelete=0
	AND cac.UserId IN (SELECT distinct UserId FROM View_CheckUser)
    AND ((AttFlag=1 and cac.StartTime!='2050-1-1') 
	      or (AttFlag=2 and cac.EndTime!='2000-1-1') 
	      or (AttFlag=3 and (cac.StartTime!='2050-1-1' or cac.EndTime!='2000-1-1')))
    and {1}
    GROUP BY   CourseId, DeptId
 ),tbSurvey AS
 (
 SELECT CourseId,DeptId,count(DISTINCT UserID)surveyNumber FROM (
     SELECT distinct objectID CourseId,syu.DeptId,sr.UserID FROM   Survey_ReplyAnswer  sr  
		LEFT JOIN Sys_User syu ON sr.UserID=syu.UserId
		WHERE SourceFrom=0 AND sr.Status=1 
		AND sr.ObjectID IN (SELECT Id  FROM Co_Course
		WHERE Way=1  AND CourseFrom=2)
		AND sr.UserID IN (SELECT UserId FROM View_CheckUser)  AND CourseRoomRuleId=0  
        and {1}
 ) t	GROUP BY CourseId,DeptId
 )
         
	 SELECT distinct cc.YearPlan, cco.CourseId, syu.DeptId,syu.DeptPath,dbo.GetDeptTwoPathByDeptID(syu.DeptId) DeptName,
	 isnull(tbreally.reallyApply,0)  reallyApply,isnull(tbAttendce.reallyJoin,0) reallyJoin,cc.IsPing,cc.IsTest,
	 tbSurvey.surveyNumber,cc.IsDelete
	FROM Cl_CourseOrder cco
	LEFT JOIN Co_Course cc ON cc.Id=cco.CourseId
	LEFT JOIN Sys_User syu ON syu.UserId=cco.UserId
	LEFT JOIN tbreally ON tbreally.CourseId=cco.CourseId AND tbreally.DeptId=syu.DeptId  --实际报名人数
	LEFT JOIN tbAttendce  ON tbAttendce.CourseId=cco.CourseId AND tbAttendce.DeptId=syu.DeptId --实际参加人数
	LEFT JOIN tbSurvey  ON tbSurvey.CourseId=cco.CourseId AND tbSurvey.DeptId=syu.DeptId  --每个部门每门课程的评估参与人数
	WHERE  syu.SubordinateSubstation='上海总部'
	AND cco.UserId IN (SELECT distinct UserId FROM View_CheckUser)
	--AND cc.IsDelete=0 
    and {0}
   ", where,freeWhere);
                return conn.Query<ReportTogether>(sql);
            }
        }


        /// <summary>
        /// 员工学过的集中授课课程以及获取的学时
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IEnumerable<ReportTogether> GetUserScore(int year, string freeWhere = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT  UserId,sum(GetScore) GetScore, DeptId,dbo.GetDeptTwoPathByDeptID(DeptId) DeptName FROM(
SELECT cco.UserId,
case WHEN (dbo.F_NewIsAppStatus(cco.CourseId,cco.UserId,cco.OrderStatus)=1)  THEN cco.GetScore  ELSE  0 END AS GetScore,
syu.DeptId
FROM Cl_CourseOrder cco
LEFT JOIN Sys_User syu ON syu.UserId=cco.UserId
WHERE 1=1
AND cco.UserId IN (SELECT distinct UserId FROM View_CheckUser)   
AND cco.CourseId IN (SELECT Id FROM Co_Course WHERE YearPlan={0} and Way=1 AND CourseFrom=2 AND Publishflag=1)
and {1}
) t GROUP BY  UserId,DeptId  ", year, freeWhere);
                return conn.Query<ReportTogether>(sql);
            }
        }

        /// <summary>
        /// 视频转播的课程相关属性
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportTogether> GetDepTranCourse(string where = "1=1", string freeWhere = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
 SELECT dcor.UserId,dcor.DepartSetId,syu.DeptId, dbo.GetDeptTwoPathByDeptID(syu.DeptId) deptName,
 GetSumScore=AttScore+EvlutionScore+ExamScore,dco.ApprovalFlag,cc.SurveyPaperId,
 dcor.CourseId,haveApply=(SELECT count(1) FROM DepTran_DepartUser WHERE DepartSetId=dcor.DepartSetId),
 t1.reallyApply,t2.reallyJoin,t3.HaveJoin,isnull(t4.surveyNumber,0) surveyNumber,IsTest, IsPing
 FROM DepTran_CourseOrder  dcor
 LEFT JOIN   DepTran_CourseOpen dco ON dco.CourseId=dcor.CourseId AND dcor.DepartSetId=dco.DepartId
 LEFT JOIN Co_Course cc ON cc.Id=dcor.CourseId
 LEFT JOIN Sys_User syu ON syu.UserId=dcor.UserId
 LEFT JOIN 
(
  --实际报名人数
  SELECT CourseId,syu.DeptId,count(dc.UserId) reallyApply FROM DepTran_CourseOrder dc
  LEFT JOIN Sys_User syu ON syu.UserId=dc.UserId
  WHERE   dc.UserId IN (SELECT UserId FROM View_CheckUser) and OrderStatus in (1,2)
  and {1}
  GROUP BY CourseId,syu.DeptId  
) t1 ON t1.CourseId=dcor.CourseId AND  t1.DeptId=syu.DeptId
LEFT JOIN 
(
  --实际参与课程的人数     
  SELECT CourseId,syu.DeptId,count(da.UserId) reallyJoin  FROM DepTran_Attendce da
  LEFT JOIN Sys_User syu ON syu.UserId=da.UserId
  WHERE da.Status<>0 AND da.Status<>2
  AND  da.UserId IN (SELECT UserId FROM View_CheckUser)
   and {1}
  GROUP BY CourseId,syu.DeptId  
) t2 ON t2.CourseId=dcor.CourseId AND  t2.DeptId=syu.DeptId
LEFT JOIN
(
    --应参与课程的人数      
    SELECT CourseId,syu.DeptId,count(da.UserId) HaveJoin FROM DepTran_Attendce da
    LEFT JOIN Sys_User syu ON syu.UserId=da.UserId
    WHERE   da.UserId IN (SELECT UserId FROM View_CheckUser)
    and {1}
    GROUP by CourseId,syu.DeptId 
    
) t3 ON t3.CourseId=dcor.CourseId AND  t3.DeptId=syu.DeptId
LEFT JOIN
(
 SELECT CourseId,DeptId,count(UserID)surveyNumber FROM (
     SELECT distinct objectID CourseId,syu.DeptId,sr.UserID FROM   Survey_ReplyAnswer  sr  
		LEFT JOIN Sys_User syu ON sr.UserID=syu.UserId
		WHERE SourceFrom=0 AND sr.Status=1 
		AND sr.ObjectID IN (SELECT Id  FROM Co_Course
		WHERE Way=1  AND CourseFrom=2)
		AND sr.UserID IN (SELECT UserId FROM View_CheckUser)  AND CourseRoomRuleId=0  
        and {1}
 ) t	GROUP BY CourseId,DeptId
)  t4 ON t4.CourseId=dcor.CourseId AND  t4.DeptId=syu.DeptId
 WHERE  {0}", where, freeWhere);
                return conn.Query<ReportTogether>(sql).ToList();
            }
        }

        /// <summary>
        /// 视频转播的课程相关属性
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportTogether> GetDepTranUser(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
SELECT dc.CourseId,dc.DepartId, haveApply=(SELECT count(1) FROM DepTran_DepartUser WHERE DepartSetId=dc.DepartId)
,isnull(t1.reallyApply,0) reallyApply,
isnull(t2.reallyJoin,0) reallyJoin,isnull(t3.HaveJoin,0) HaveJoin
FROM DepTran_CourseOpen  dc
LEFT JOIN 
(
  --实际报名人数
  SELECT CourseId,count(UserId) reallyApply FROM DepTran_CourseOrder
  WHERE UserId IN (SELECT UserId FROM View_CheckUser)
  GROUP BY CourseId  
) t1 ON t1.CourseId=dc.CourseId
LEFT JOIN 
(
  --实际参与课程的人数     
  SELECT CourseId,count(UserId) reallyJoin  FROM DepTran_Attendce
  WHERE Status<>0 AND Status<>2
  AND  UserId IN (SELECT UserId FROM View_CheckUser)
  GROUP BY CourseId  
) t2 ON t2.CourseId=dc.CourseId
LEFT JOIN
(
    --应参与课程的人数      
    SELECT CourseId,count(UserId) HaveJoin FROM DepTran_Attendce
     WHERE UserId IN (SELECT UserId FROM View_CheckUser)
    GROUP by CourseId
) t3 ON t3.CourseId=dc.CourseId");
                return conn.Query<ReportTogether>(sql).ToList();
            }
        }

        /// <summary>
        /// 所有学习的人员所在的部门
        /// </summary>
        /// <returns></returns>
        public List<fVedioJoinNumber> GetStudyUser()
        {
            using (var conn = OpenConnection())
            {
                var sql = @"

SELECT DISTINCT cc.YearPlan, cc.Id courseID, cco.UserId, syu.DeptId
FROM Cl_CourseOrder cco
LEFT JOIN Co_Course cc on cc.Id=cco.CourseId
LEFT JOIN Sys_User syu ON syu.UserId=cco.UserId
WHERE  cco.UserId IN (SELECT distinct UserId FROM View_CheckUser)
UNION
SELECT distinct cc.YearPlan,cc.Id courseID, dcor.UserId,syu.DeptId
 FROM DepTran_CourseOrder  dcor
 LEFT JOIN Co_Course cc on cc.Id=dcor.CourseId
 LEFT JOIN   DepTran_CourseOpen dco ON dco.CourseId=dcor.CourseId AND dcor.DepartSetId=dco.DepartId
 LEFT JOIN Sys_User syu ON syu.UserId=dcor.UserId
  WHERE  dcor.UserId IN (SELECT distinct UserId FROM View_CheckUser)";
                return conn.Query<fVedioJoinNumber>(sql).ToList();
            }
        }

        /// <summary>
        /// 课后评估的人数
        /// </summary>
        /// <returns></returns>
        public List<ReportTogether> GetSurveyUserCount()
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT distinct objectID CourseId,syu.DeptId,count(DISTINCT sr.UserID) surveyNumber FROM   Survey_ReplyAnswer  sr  
LEFT JOIN Sys_User syu ON sr.UserID=syu.UserId
WHERE SourceFrom=0 AND sr.Status=1 
AND sr.ObjectID IN (SELECT Id  FROM Co_Course
WHERE Way=1  AND CourseFrom=2)
AND sr.UserID IN (SELECT UserId FROM View_CheckUser)  AND CourseRoomRuleId=0
GROUP BY objectID,syu.DeptId";
                return conn.Query<ReportTogether>(sql).ToList();
            }
        }
        #endregion

        #region 所有课程的综合统计
        /// <summary>
        /// 课程的基本信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportTogether> GetCourseInfoList(string where = " 1=1",string freeWhere=" 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH tbreally AS
 (
   --实际报名人数
  	SELECT CourseId,count(cco.UserId) reallyApply FROM Cl_CourseOrder cco
	WHERE cco.[OrderStatus] in (1,3)
    and ( cco.[IsLeave] = 0 
        or (cco.[IsLeave] = 1 and cco.[ApprovalFlag] <>1)
        or (cco.[IsLeave] = 1 and cco.[ApprovalFlag] = 1 
        and cco.[ApprovalDateTime] > cco.[ApprovalLimitTime])
      )
    AND cco.UserId IN (SELECT distinct UserId FROM View_CheckUser) 
    AND {1}
    GROUP BY   CourseId
 ),tbAttendce AS
 (
   --课程有考勤的人数
    SELECT cac.UserId UserID,CourseId,AttFlag,cac.StartTime,cac.EndTime FROM  Cl_Attendce  cac
	LEFT JOIN Co_Course cc ON cac.CourseId=cc.Id
	WHERE Way=1 AND CourseFrom=2 
	AND cac.UserId IN (SELECT distinct UserId FROM View_CheckUser)
    AND {1}
 ),tbSurvey AS
 (
 SELECT CourseId,count(DISTINCT UserID) surveyNumber FROM (
     SELECT  objectID CourseId,sr.UserID FROM   Survey_ReplyAnswer  sr  
		WHERE SourceFrom=0 AND sr.Status=1 
		AND  sr.UserID IN (SELECT UserId FROM View_CheckUser)  AND CourseRoomRuleId=0  
         AND {1}
 ) t	GROUP BY CourseId
 )
 SELECT * from(
SELECT DISTINCT cco.CourseId,cc.CourseName,cc.OpenLevel, YearPlan,cc.IsMust,cc.StartTime,cc.EndTime,cc.Teacher,syu.Realname,syu.PayGrade
,tbreally.reallyApply,t2.reallyJoin,t3.HaveJoin,tbSurvey.surveyNumber,cc.SurveyPaperId,cc.IsPing,cc.IsCPA,cc.IsTest,cc.IsDelete,cc.Publishflag
,cc.IsOrder 
FROM Cl_CourseOrder  cco
LEFT JOIN Co_Course cc  ON cco.CourseId=cc.Id
LEFT JOIN Sys_User syu ON syu.UserId=cc.Teacher
LEFT JOIN tbreally ON tbreally.CourseId=cco.CourseId   --实际报名人数
LEFT JOIN (SELECT CourseId,reallyJoin=Count(DISTINCT UserId) FROM tbAttendce 
    WHERE ((AttFlag=1 and StartTime!='2050-1-1') 
	      or (AttFlag=2 and EndTime!='2000-1-1') 
	      or (AttFlag=3 and (StartTime!='2050-1-1' or EndTime!='2000-1-1'))) 
	      GROUP BY CourseId) t2  ON t2.CourseId=cco.CourseId  --实际参加人数
LEFT JOIN (SELECT CourseId, Count(DISTINCT UserId) HaveJoin FROM tbAttendce GROUP BY CourseId) t3 
	   ON t3.CourseId=cco.CourseId --应该参加人数
	LEFT JOIN tbSurvey  ON tbSurvey.CourseId=cco.CourseId  --每个部门每门课程的评估参与人数	  
) t WHERE  {0}
", where,freeWhere);
                return conn.Query<ReportTogether>(sql).ToList();
            }
        }


        /// <summary>
        /// 获取人员对课程的评价
        /// </summary>
        /// <returns></returns>
        public List<UserSurvey> GetUserSurvey(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH info AS 
(
  SELECT  ObjectID,ExampaperID,QuestionID,round(avg(CONVERT(decimal,SubjectiveAnswer)),2) avg FROM Survey_ReplyAnswer sr
  LEFT JOIN Sys_User syu ON syu.UserId=sr.UserId
  WHERE  SourceFrom=0 AND sr.Status=1 and QuestionID IN (SELECT QuestionID FROM Survey_Question WHERE QuestionType=3)
  and sr.UserID IN (SELECT UserID FROM dbo.View_CheckUser) 
  AND SubjectiveAnswer IN ('0','1','2','3','4','5') AND CourseRoomRuleId=0
  AND  ObjectID IN (SELECT Id FROM Co_Course WHERE Way=1 AND CourseFrom=2)  
  and {0}
  GROUP BY   ObjectID,ExampaperID,QuestionID
)
SELECT ObjectID,ExampaperID,count(DISTINCT QuestionID) questionCount,sum(avg) PingSum
FROM info
GROUP BY ObjectID,ExampaperID", where);
                return conn.Query<UserSurvey>(sql).ToList();
            }
        }

        /// <summary>
        /// 课程的基本信息 转播课程
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportTogether> GetRTCourseInfoList(string where, string freeWhere=" 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
SELECT CourseId, CourseName, YearPlan, IsMust, StartTime, EndTime, Teacher, Realname, PayGrade,SurveyPaperId,OpenLevel,
sum(haveApply) haveApply,reallyApply, reallyJoin, HaveJoin, surveyNumber,IsOrder  FROM (
 SELECT distinct dcor.CourseId,cc.CourseName,YearPlan,cc.IsMust,cc.StartTime,cc.EndTime,cc.Teacher,syu.Realname,syu.PayGrade,
  cc.SurveyPaperId,cc.OpenLevel,
 haveApply=(SELECT count(1) FROM DepTran_DepartUser WHERE DepartSetId=dcor.DepartSetId),
 t1.reallyApply,t2.reallyJoin,t3.HaveJoin,isnull(t4.surveyNumber,0) surveyNumber,cc.IsOrder
 FROM DepTran_CourseOrder  dcor
 LEFT JOIN   DepTran_CourseOpen dco ON dco.CourseId=dcor.CourseId AND dcor.DepartSetId=dco.DepartId
 LEFT JOIN Co_Course cc ON cc.Id=dcor.CourseId
 LEFT JOIN Sys_User syu ON syu.UserId=cc.Teacher
 LEFT JOIN 
(
  --实际报名人数
  SELECT CourseId,count(dc.UserId) reallyApply FROM DepTran_CourseOrder dc
  WHERE  dc.UserID IN (SELECT UserId FROM View_CheckUser)  and OrderStatus in (1,2)
  and {1}
  GROUP BY CourseId
) t1 ON t1.CourseId=dcor.CourseId
LEFT JOIN 
(
  --实际参与课程的人数     
  SELECT CourseId,count(da.UserId) reallyJoin  FROM DepTran_Attendce da
  WHERE da.Status<>0 AND da.Status<>2  AND  da.UserID IN (SELECT UserId FROM View_CheckUser)
  and {1}
  GROUP BY CourseId
) t2 ON t2.CourseId=dcor.CourseId 
LEFT JOIN
(
    --应参与课程的人数      
    SELECT CourseId,count(da.UserId) HaveJoin FROM DepTran_Attendce da
    WHERE  da.UserID IN (SELECT UserId FROM View_CheckUser)
    and {1}
    GROUP by CourseId
    
) t3 ON t3.CourseId=dcor.CourseId
LEFT JOIN
(
 SELECT CourseId,count(DISTINCT UserID)surveyNumber FROM (
     SELECT distinct objectID CourseId,sr.UserID FROM   Survey_ReplyAnswer  sr  
		WHERE SourceFrom=0 AND sr.Status=1 
		AND sr.ObjectID IN (SELECT Id  FROM Co_Course
		WHERE Way=1  AND CourseFrom=2)
		AND sr.UserID IN (SELECT UserId FROM View_CheckUser)  AND CourseRoomRuleId=0  
        and {1}
 ) t	GROUP BY CourseId
)  t4 ON t4.CourseId=dcor.CourseId
 WHERE 1=1
 ) t 
 WHERE {0}
 GROUP BY CourseId, CourseName, YearPlan, IsMust, StartTime, EndTime, Teacher, Realname, PayGrade, SurveyPaperId,OpenLevel
, reallyApply, reallyJoin, HaveJoin, surveyNumber,IsOrder", where, freeWhere);
                return conn.Query<ReportTogether>(sql).ToList();
            }
        }


        #endregion

        #region 员工单门课程评估、测试情况表
        /// <summary>
        /// 员工单门课程评估
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<SingleTogtherSurvey> GetSingleTogtherSurvey(int courseID, string where = " 1=1", string dwhere = " 1=1", string timewhere = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"--应参加评估人数
WITH tbhave AS
(
    SELECT cac.UserId UserID,CourseId,AttFlag,cac.StartTime,cac.EndTime,syu.DeptId FROM  Cl_Attendce  cac
	LEFT JOIN Co_Course cc ON cac.CourseId=cc.Id
	 LEFT JOIN Sys_User syu ON cac.UserId=syu.UserId 
	WHERE Way=1 AND CourseFrom=2 
	AND cac.CourseId={0}  and  {1}
	AND cac.UserId IN (SELECT distinct UserId FROM View_CheckUser)
),tbSurvey AS
(
   SELECT syu.DeptId,count(DISTINCT sr.UserID) SurveyNumber FROM Survey_ReplyAnswer sr
   LEFT JOIN Sys_User syu ON syu.UserId=sr.UserID
   WHERE ObjectID={0} AND SourceFrom=0  AND sr.Status=1
   AND sr.UserId IN (SELECT UserId FROM View_CheckUser)  AND CourseRoomRuleId=0
   and  {1} and  {3}
   GROUP BY syu.DeptId
)
  SELECT cco.UserId,syu.DeptId,dbo.GetDeptTwoPathByDeptID(syu.DeptId) DeptName,t3.HaveJoin,t2.reallyJoin, tbSurvey.SurveyNumber FROM Cl_CourseOrder  cco
	LEFT JOIN Co_Course cc ON cc.Id=cco.CourseId
	LEFT JOIN Sys_User syu ON syu.UserId=cco.UserId
	LEFT JOIN (SELECT CourseId, Count(DISTINCT UserId) HaveJoin,DeptId FROM tbhave GROUP BY CourseId,DeptId) t3 
	ON t3.CourseId=cco.CourseId AND t3.DeptId=syu.DeptId --应该参加人数
	LEFT JOIN 
	(SELECT CourseId,reallyJoin=Count(DISTINCT UserId),DeptId FROM tbhave 
	 WHERE ((AttFlag=1 and StartTime!='2050-1-1') 
	      or (AttFlag=2 and EndTime!='2000-1-1') 
	      or (AttFlag=3 and (StartTime!='2050-1-1' or EndTime!='2000-1-1')))
	      GROUP BY CourseId,DeptId
	 )t2 ON t2.CourseId=cco.CourseId AND t2.DeptId=syu.DeptId --实际参加人数
	LEFT JOIN  tbSurvey ON tbSurvey.DeptId=syu.DeptId
	WHERE  cc.Publishflag=1 and   {2}
    AND cco.UserId IN (SELECT UserId FROM View_CheckUser)
	and cco.CourseId={0}  AND syu.SubordinateSubstation='上海总部'", courseID, dwhere, where, timewhere);
                return conn.Query<SingleTogtherSurvey>(sql).ToList();
            }
        }

        /// <summary>
        /// 员工单门课程评估 （转播）
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<SingleTogtherSurvey> GetSingleRTSurvey(int courseID, string where = " 1=1", string dwhere = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT dcor.UserId,dcor.DepartSetId,syu.DeptId, dbo.GetDeptTwoPathByDeptID(syu.DeptId) DeptName,
 cc.SurveyPaperId,t3.HaveJoin,isnull(t4.surveyNumber,0) SurveyNumber,t2.reallyJoin
 FROM DepTran_CourseOrder  dcor
 LEFT JOIN   DepTran_CourseOpen dco ON dco.CourseId=dcor.CourseId AND dcor.DepartSetId=dco.DepartId
 LEFT JOIN Co_Course cc ON cc.Id=dcor.CourseId
 LEFT JOIN Sys_User syu ON syu.UserId=dcor.UserId
 LEFT JOIN 
(
  --实际参与课程的人数     
  SELECT syu.DeptId,count(da.UserId) reallyJoin  FROM DepTran_Attendce da
  LEFT JOIN Sys_User syu ON syu.UserId=da.UserId
  WHERE da.Status<>0 AND da.Status<>2  AND da.CourseId={0} and {1}
  AND  da.UserId IN (SELECT UserId FROM View_CheckUser)
  GROUP BY syu.DeptId  
) t2 ON   t2.DeptId=syu.DeptId
LEFT JOIN
(
    --应参与课程的人数      
    SELECT syu.DeptId,count(da.UserId) HaveJoin FROM DepTran_Attendce da
    LEFT JOIN Sys_User syu ON syu.UserId=da.UserId
    WHERE da.CourseId={0} and {1}
     AND  da.UserId IN (SELECT UserId FROM View_CheckUser)
    GROUP by syu.DeptId 
    
) t3 ON   t3.DeptId=syu.DeptId
LEFT JOIN
(
 SELECT DeptId,count(DISTINCT UserID)surveyNumber FROM (
     SELECT distinct syu.DeptId,sr.UserID FROM   Survey_ReplyAnswer  sr  
		LEFT JOIN Sys_User syu ON sr.UserID=syu.UserId
		WHERE SourceFrom=0 AND sr.Status=1 
		AND sr.ObjectID={0} and {1}
		AND sr.UserID IN (SELECT UserId FROM View_CheckUser)  AND CourseRoomRuleId=0  
 ) t	GROUP BY DeptId
)  t4 ON t4.DeptId=syu.DeptId
 WHERE dcor.CourseId={0} and {2}", courseID, dwhere, where);
                return conn.Query<SingleTogtherSurvey>(sql).ToList();
            }
        }


        /// <summary>
        /// 每个部门针对每个问卷的评估分数
        /// </summary>
        /// <returns></returns>
        public List<UserSurvey> GetSingleTogther(int courseID, string dwhere = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH info AS 
(
  SELECT  ExampaperID,syu.DeptId,QuestionID,round(avg(CONVERT(decimal,SubjectiveAnswer)),2) avg 
  FROM Survey_ReplyAnswer sr
  LEFT JOIN Sys_User syu ON sr.UserID=syu.UserId
  WHERE  SourceFrom=0 AND sr.Status=1 and QuestionID IN (SELECT QuestionID FROM Survey_Question WHERE QuestionType=3)
  and sr.UserID IN (SELECT UserID FROM dbo.View_CheckUser) 
  AND SubjectiveAnswer IN ('0','1','2','3','4','5') AND CourseRoomRuleId=0
  AND  ObjectID={0}
  GROUP BY    ExampaperID,syu.DeptId,QuestionID
)
SELECT DeptId,ExampaperID,count(DISTINCT QuestionID) questionCount,sum(avg) PingSum
FROM info
where 1=1 and {1}
GROUP BY DeptId,ExampaperID", courseID, dwhere);
                return conn.Query<UserSurvey>(sql).ToList();
            }
        }


        #endregion

        #region 公共

        /// <summary>
        /// 每个部门分所应参加的人数
        /// </summary>
        /// <returns></returns>
        public List<fVedioJoinNumber> GetTogetherUser(int year)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"EXEC PROC_GetTogetUser @Year=" + year;
                return conn.Query<fVedioJoinNumber>(sql).ToList();
            }
        }






        #endregion
    }
}
