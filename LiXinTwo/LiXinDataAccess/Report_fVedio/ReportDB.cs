using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.Report_zVedio;
using LiXinModels.Report_Vedio;

namespace LiXinDataAccess.Report_fVedio
{
    public class ReportDB : BaseRepository
    {
        #region 汇总

        /// <summary>
        /// 总所——视频课程汇总统计（仅统计到 在线测试通过率）
        /// </summary>
        public List<CourseVedioLearn> GetVedioLearn(string deptIDs,string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT vvc.*,
fReallyNumber=sum(isnull(vvd.LearnTimes,0)),
fLearnNumber=count(DISTINCT vvd.UserID) ,
fPassNumber=(SELECT count(DISTINCT UserID) FROM Cl_CpaLearnStatus WHERE CourseID=vvc.CourseID AND IsPass=1
AND UserID IN (SELECT UserID FROM Sys_User WHERE DeptId IN ({1})  AND DeptId IN (SELECT DeptId FROM View_CheckUser)))
FROM View_VedioCourseLearn vvc
LEFT JOIN (SELECT * FROM View_VedioUserDept WHERE DeptId IN ({1})) vvd ON vvc.CourseId=vvd.Id
where  {0}
GROUP BY CourseId, CoursePaperId, Year, IsTest, IsPing, SurveyPaperId, CourseName, LearnNumber, ReallyNumber,YearPlan,passNumber,vvc.IsOpenSub", where, deptIDs);
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
                var sql = string.Format(@"SELECT ObjectID,ExampaperID,UserID, sum(CAST(SubjectiveAnswer AS INT)) PingSum,
    count(QuestionID) questionCount FROM(
   SELECT ObjectID, sr.UserID,sr.ExampaperID,SubjectiveAnswer, sr.QuestionID  
    FROM   Survey_ReplyAnswer  sr
WHERE SourceFrom=0 AND sr.Status=1  
--AND sr.QuestionID IN (SELECT QuestionID FROM Survey_Question WHERE QuestionType=3)      
AND  sr.UserID IN (SELECT UserID FROM View_CheckUser)  
and PATINDEX('%[^0-9]%',SubjectiveAnswer)=0                                                     
) r   GROUP BY  ObjectID, ExampaperID,UserID");
                return conn.Query<UserSurvey>(sql).ToList();
            }
        }

        /// <summary>
        /// 我管理部门的人员
        /// </summary>
        /// <param name="deptIDs"></param>
        /// <returns></returns>
        public List<int> GetUserDept(string deptIDs)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT UserId FROM Sys_User
WHERE DeptId IN ({0})  AND DeptId IN (SELECT DeptId FROM View_CheckUser)", deptIDs);
                return conn.Query<int>(sql).ToList();
            }
        }


        public List<dynamic> GetCourseLearn()
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT CourseID, UserID FROM Cl_CpaLearnStatus
WHERE CpaFlag=0";
                return conn.Query<dynamic>(sql).ToList();
            }
        }
        #endregion

        #region 单个
        /// <summary>
        /// 视频课程单门课程统计
        /// </summary>
        /// <returns></returns>
        public List<VedioLearnSingle> GetVedioLearnSingle(string DeptIds,string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@" WITH info AS
 (
   SELECT Id CourseID,dbo.f_GetVdeioOpenObject(Id)  openObject FROM Co_Course
 )
select * from (
SELECT
vvc.CourseId,cc.CourseName,cc.StartTime,cc.EndTime,su.Realname Teacher,cc.CourseLength,cc.IsMust,cc.IsTest,cc.IsCPA,vvc.LearnNumber
,cc.YearPlan,info.openObject
     , fLearNNumber =(SELECT count(1) FROM View_VedioUserDept WHERE DeptId IN ({1})  and Id=cc.Id)
 FROM View_VedioCourseLearn  vvc
 LEFT JOIN Co_Course cc ON vvc.CourseId= cc.Id
 LEFT JOIN Sys_User su ON su.UserId=cc.Teacher
 LEFT JOIN info ON info.CourseID= cc.Id
where  cc.IsDelete=0 AND cc.Publishflag=1 ) t where  {0}
", where, DeptIds);

                return conn.Query<VedioLearnSingle>(sql).ToList();
            }
        }
        #endregion

        #region 缓存
        /// <summary>
        /// 获取视频课程的应该参与人员对应的部门
        /// </summary>
        /// <returns></returns>
        public List<fVedioJoinNumber> GetVedioJoinNumber()
        {
            using (var conn = OpenConnection())
            {
                var sql = @"exec PROC_GetVedioUser";
                return conn.Query<fVedioJoinNumber>(sql).ToList();
            }
        }

        #endregion

       

    }
}
