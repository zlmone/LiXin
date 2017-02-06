using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.Report_zVedio;

namespace LiXinDataAccess
{
    public class VedioCourseLearnDB : BaseRepository
    {
        #region 汇总

        /// <summary>
        /// 总所——视频课程汇总统计（仅统计到 在线测试通过率）
        /// </summary>
        public List<CourseVedioLearn> GetVedioLearn(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT vvc.* FROM View_VedioCourseLearn    vvc
 where  {0}", where);
                return conn.Query<CourseVedioLearn>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取人员对课程的评价
        /// </summary>
        /// <returns></returns>
        public List<UserSurvey> GetUserSurvey()
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH info AS 
(
  SELECT  ObjectID,ExampaperID,QuestionID,round(avg(CONVERT(decimal,SubjectiveAnswer)),2) avg FROM Survey_ReplyAnswer
  WHERE  SourceFrom=0 AND Status=1 and QuestionID IN (SELECT QuestionID FROM Survey_Question WHERE QuestionType=3)
  and UserID IN (SELECT UserID FROM dbo.View_CheckUser) 
  AND SubjectiveAnswer IN ('0','1','2','3','4','5') AND CourseRoomRuleId=0   
  GROUP BY   ObjectID,ExampaperID,QuestionID
)
SELECT ObjectID,ExampaperID,count(DISTINCT QuestionID) questionCount,sum(avg) PingSum
FROM info
GROUP BY ObjectID,ExampaperID");
                return conn.Query<UserSurvey>(sql).ToList();
            }
        }


        #endregion

        #region 单个
        /// <summary>
        /// 视频课程单门课程统计
        /// </summary>
        /// <returns></returns>
        public List<VedioLearnSingle> GetVedioLearnSingle( string where = " 1=1", string jsRenderSortField = " StartTime desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT
vvc.CourseId,cc.CourseName,cc.StartTime,cc.EndTime,su.Realname Teacher,cc.CourseLength,cc.IsMust,cc.IsTest,cc.IsCPA,vvc.LearnNumber
,cc.Year,cc.YearPlan,dbo.f_GetVdeioOpenObject(cc.Id) openObject
 FROM View_VedioCourseLearn  vvc
 LEFT JOIN Co_Course cc ON vvc.CourseId= cc.Id
 LEFT JOIN Sys_User su ON su.UserId=cc.Teacher
where  cc.IsDelete=0 AND cc.Publishflag=1 and   1=1 ) t WHERE   {0}
", where);
              
                return conn.Query<VedioLearnSingle>(sql).ToList();
            }
        }
        #endregion

        #region 公共
        /// <summary>
        /// 课程应学习的总人数
        /// </summary>
        /// <returns></returns>
        public List<VedioNumber> GetCacheVedioSumNumber()
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT Id CourseID,dbo.f_GetVedioNumber(Id) SumNumber FROM Co_Course
WHERE Way=2 AND CourseFrom=2 AND IsDelete=0 AND Publishflag=1";
                return conn.Query<VedioNumber>(sql).ToList();
            }
        }
        
        /// <summary>
        /// 用于纳入考核范围内的人员
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetCheckUserList()
        {
            using (var conn = OpenConnection())
            {
                var sql = @" SELECT * from View_CheckUser";
                return conn.Query<dynamic>(sql).ToList();
            }
        }

        #endregion

        #region 参与情况
        /// <summary>
        /// 参与情况
        /// </summary>
        /// <param name="deptIds"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<CourseJoin> GetCourseJoinList( int courseID,string where="1=1",string querytime=" 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH tblearn AS
(
  SELECT ccs.UserID,syu.DeptId,sum(clv.LearnTimes) LearnTimes,sum(ccs.VedioTime) VedioTime FROM Cl_CpaLearnStatus  ccs
LEFT JOIN Cl_LearnVideoInfor clv   ON ccs.Id=clv.LearnId
LEFT JOIN Sys_User syu ON syu.UserId=ccs.UserID
WHERE syu.UserId IN (SELECT distinct UserId FROM View_CheckUser) 
and  syu.Status=0 AND syu.IsDelete=0  AND CourseID={0}  and {2}
group BY ccs.UserID,syu.DeptId,ccs.VedioTime
)

SELECT DepartmentId DeptID,DeptName,dbo.GetDeptTwoPathByDeptID(DepartmentId)  DeptPath,dbo.GetDeptPathByDeptID(DepartmentId)  allDeptPath,
sum(vvu.LearnTimes) LearnNumber,
count(DISTINCT UserID) JoinNumber,sum(VedioTime) vedioTime FROM Sys_Department   sd
inner JOIN  tblearn  vvu ON vvu.DeptId=sd.DepartmentId
WHERE  DepartmentId IN (SELECT distinct DeptId FROM View_CheckUser) 
and {1}
 GROUP BY DepartmentId,DeptName

", courseID, where, querytime);
                return conn.Query<CourseJoin>(sql).ToList();
            }
        }

        /// <summary>
        /// 参与情况 分所
        /// </summary>
        /// <param name="deptIds"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<CourseJoin> GetCourseDeptIDJoinList(int courseID, string deptIDs, string where = "1=1", string querytime = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH tblearn AS
(
  SELECT ccs.UserID,syu.DeptId,sum(clv.LearnTimes) LearnTimes,sum(ccs.VedioTime) VedioTime FROM Cl_CpaLearnStatus  ccs
LEFT JOIN Cl_LearnVideoInfor clv   ON ccs.Id=clv.LearnId
LEFT JOIN Sys_User syu ON syu.UserId=ccs.UserID
WHERE syu.UserId IN (SELECT distinct UserId FROM View_CheckUser) 
and CourseID={0}  and {3}
group BY ccs.UserID,syu.DeptId,ccs.VedioTime
)


SELECT DepartmentId DeptID,DeptName,dbo.GetDeptTwoPathByDeptID(DepartmentId)  DeptPath,dbo.GetDeptPathByDeptID(DepartmentId)  allDeptPath,sum(vvu.LearnTimes) LearnNumber,
count(DISTINCT UserID) JoinNumber,sum(VedioTime) vedioTime FROM Sys_Department   sd
inner JOIN  tblearn  vvu ON vvu.DeptId=sd.DepartmentId
WHERE  DepartmentId IN (SELECT distinct DeptId FROM View_CheckUser)  and  {1}
AND DepartmentId in ({2})
 GROUP BY DepartmentId,DeptName", courseID, where, deptIDs, querytime);
                return conn.Query<CourseJoin>(sql).ToList();
            }
        }

        /// <summary>
        /// 每个部门的每个人观看的时长
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<dynamic> GetVedioTimeByDept(int courseID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"  SELECT CourseID, UserID, DeptId,DeptPath, sum(LearnTimes) LearnTimes ,isnull(sum(VedioTime),0) VedioTime FROM (
 SELECT ccs.CourseID,ccs.UserID,syu.DeptId,syu.DeptPath, clv.LearnTimes,ccs.VedioTime,CpaFlag FROM  Cl_CpaLearnStatus  ccs 
LEFT JOIN Cl_LearnVideoInfor clv ON ccs.Id=clv.LearnId
LEFT JOIN Sys_User syu ON syu.UserId=ccs.UserID
WHERE ccs.CourseID={0}  AND   CpaFlag=0
 AND  ccs.UserID IN (SELECT UserId FROM dbo.View_CheckUser)
  ) t
  GROUP BY CourseID, UserID, DeptId,DeptPath", courseID);
                return conn.Query<dynamic>(sql).ToList();
            }
        }
        #endregion
      


    }
}
