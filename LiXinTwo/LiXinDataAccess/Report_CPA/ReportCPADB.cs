using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.Report_CPA;
using LiXinModels.SystemManage;

namespace LiXinDataAccess.Report_CPA
{
    public class ReportCPADB : BaseRepository
    {
        #region CPA报表
        /// <summary>
        /// 每个人获得的cpa学时 详细 年度 时间等
        /// </summary>
        /// <returns></returns>
        public List<ReportCPA> GetCPAScoreList(int year,string starttime = "", string endtime = "", string where = " 1=1", string timeWhere = " 1=1", int deptMaxScore = -1,string yearStr="")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
DECLARE @maxscore INT={4};
WITH cpazhouqiscore AS 
(
 SELECT   UserID,sum(SumZhouqiScore) SumZhouqiScore FROM(
  SELECT UserID,SumZhouqiScore=sum(tScore)+
  CASE WHEN sum(dScore)>@maxscore and @maxscore<>-1 THEN @maxscore ELSE sum(dScore) END
  +sum(cpaScore) FROM view_CPAScore
  WHERE StartTime>='{0}' AND EndTime<='{1}'
  and UserID in (SELECT UserId FROM Sys_User
  WHERE CPA='是' AND IsDelete=0 AND Status=0)  AND YearPlan IN ({5})
  GROUP BY  UserID,YearPlan
  )t  GROUP BY  UserID
),cpascore AS
(
   SELECT UserID,sum(tScore) tScore,sum(dScore) dScore,sum(cpaScore) cpaScore FROM view_CPAScore
   where {3}
   and UserID in (SELECT UserId FROM Sys_User
   WHERE CPA='是' AND IsDelete=0 AND Status=0) AND YearPlan IN ({5})
   GROUP BY UserID
)
SELECT * FROM (
SELECT syu.UserId, Realname, DeptName, CPANo,syu.Username,
isJoinDaode=(SELECT count(1) FROM Cl_CpaLearnStatus ccs
   INNER JOIN Co_Course cc  ON ccs.CourseID=cc.Id
   WHERE UserID=syu.UserId AND cc.Way=3 AND cc.IsMust=1  AND ccs.GradeStatus=1 AND YearPlan IN ({5}))
,SumZhouqiScore,tScore,dScore,cpaScore sumCPAScore,syu.CPACreateDate
 FROM Sys_User syu
 LEFT JOIN cpazhouqiscore ON syu.UserId=cpazhouqiscore.UserID
 LEFT JOIN cpascore ON syu.UserId=cpascore.UserID
WHERE CPARelationship='立信上海总所' AND CPA='是'
AND syu.IsDelete=0 AND syu.Status=0
 ) t where {2}", starttime, endtime, where, timeWhere, deptMaxScore, yearStr, year);
                return conn.Query<ReportCPA>(sql).ToList();
            }
        }


        /// <summary>
        /// 查询出此年度内的课程
        /// </summary>
        /// <returns></returns>
        public List<int> GetCourseIDInYear(string timewhere = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT Id FROM Co_Course
WHERE {0} AND CourseFrom=2", timewhere);
                return conn.Query<int>(sql).ToList();
            }
        }
        /// <summary>
        /// 查询出此年度内的课程
        /// </summary>
        /// <returns></returns>
        public List<int> GetCourseIDInYearForDep(string timewhere = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT Id FROM dep_Course
WHERE {0} AND CourseFrom=2", timewhere);
                return conn.Query<int>(sql).ToList();
            }
        }



        /// <summary>
        /// 执业CPA继续教育学时明细
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportCPA> GetCPADetailsList(string where = " 1=1", string yearwhere = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT  DeptName,syu.UserId, Realname, CPANo,courseType,
Way,CourseName,YearPlan,StartTime,EndTime,
tScore,dScore,cpaScore sumCPAScore,syu.Username,syu.CPACreateDate
from Sys_User syu
left JOIN view_CPAScore vcs ON vcs.UserID=syu.UserID and {0}
WHERE  CPARelationship='立信上海总所' AND CPA='是' 
AND syu.IsDelete=0 AND syu.Status=0  and {1}", yearwhere,where);
                return conn.Query<ReportCPA>(sql).ToList();
            }
        }
        #endregion

        #region 执业CPA继续教育目标完成情况
        /// <summary>
        /// 周期内 每个学员获得的学时以及对应的年
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<ReportCPA> GetCPAzhouqiUser(string startTime, string endTime, string yearstr)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT  YearPlan,vcs.UserID, sum(tScore) tScore, sum(dScore) dScore,syu.CPA,
IsJoinDaode=(select count(1)  from  Cl_CpaLearnStatus
	left join Co_Course on Cl_CpaLearnStatus.CourseID=Co_Course.id 
   where CpaFlag=1 and Co_Course.way=3 and ismust=1 and userid=vcs.UserID
   AND YearPlan IN ({2}))
FROM VIEW_CpaUserScore vcs
LEFT JOIN Sys_User syu ON syu.UserId=vcs.UserID 
WHERE StartTime>='{0}' AND EndTime<='{1}'
and CPA='是' AND IsDelete=0 AND Status=0  AND UserType IN ('在职','见习','特批','聘用')
 GROUP BY  YearPlan,vcs.UserID,syu.CPA", startTime, endTime, yearstr);
                return conn.Query<ReportCPA>(sql).ToList();
            }
        }

        /// <summary>
        /// 学员学时获取
        /// </summary>
        /// <param name="where"></param>
        /// <param name="timewhere"></param>
        /// <returns></returns>
        public List<ReportCPA> GetUserCpaScore(string where = "1=1", string timewhere = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH cpascore AS
(
   SELECT UserID,sum(tScore) tScore,sum(dScore) dScore FROM VIEW_CpaUserScore
   WHERE {0}
   AND UserID IN  (SELECT UserId FROM Sys_User
  WHERE CPA='是' AND IsDelete=0 AND Status=0 )
   GROUP BY UserID
) 
select * from (
SELECT syu.UserId, Realname, TrainGrade,PayGrade,syu.DeptId,syu.DeptPath,dbo.GetDeptTwoPathByDeptID(DeptId) deptName
,tScore,dScore,syu.CPA,CPARelationship,syu.JoinDate,syu.CPACreateDate   FROM Sys_User  syu
LEFT JOIN cpascore  ON syu.UserId=cpascore.UserID
where CPA='是' AND IsDelete=0 AND Status=0  AND UserType IN ('在职','见习','特批','聘用') ) t where   {1}", timewhere, where);
                return conn.Query<ReportCPA>(sql).ToList();
            }
        }

        /// <summary>
        /// 周期学时
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public List<dynamic> GetzhouqiAllScore(string yearStr, string starttime = "", string endtime = "", int score = -1)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"DECLARE @maxscore INT={2};
 SELECT   UserID,sum(SumZhouqiScore) SumZhouqiScore FROM(
  SELECT UserID,SumZhouqiScore=sum(tScore)+
  CASE WHEN sum(dScore)>@maxscore and @maxscore<>-1 THEN @maxscore ELSE sum(dScore) END
  +sum(cpaScore) FROM view_CPAScore
  WHERE StartTime>='{0}' AND EndTime<='{1}'
  and UserID in (SELECT UserId FROM Sys_User
  WHERE CPA='是' AND IsDelete=0 AND Status=0 AND UserType IN ('在职','见习','特批','聘用'))
 AND YearPlan IN ({3})
  GROUP BY  UserID,YearPlan
 )t  GROUP BY  UserID", starttime, endtime, score, yearStr);
                return conn.Query<dynamic>(sql).ToList();
            }
        }
        #endregion

        #region 公共
        /// <summary>
        /// 动态的取得年
        /// </summary>
        /// <returns></returns>
        public List<int> GetCourseYear()
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT * FROM (
SELECT  YearPlan FROM Co_Course
WHERE IsDelete=0 AND Publishflag=1
UNION 
SELECT  YearPlan FROM Dep_Course
WHERE IsDelete=0 AND Publishflag=1
)t";
                return conn.Query<int>(sql).ToList();
            }
        }

        /// <summary>
        ///查询执业CPA的职业CPA
        /// </summary>
        /// <returns></returns>
        public List<string> GetPayGrade()
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT DISTINCT PayGrade FROM Sys_User
WHERE CPA='是' AND IsDelete=0 AND Status=0 
 and PayGrade<>''";
                return conn.Query<string>(sql).ToList();
            }
        }

        /// <summary>
        ///查询执业CPA的职业CPA
        /// </summary>
        /// <returns></returns>
        public List<string> GetPayGrade(string where = " 1=1", int type = 0)
        {
            using (var conn = OpenConnection())
            {
                var sql = "";
                if (type == 0)
                {

                    sql = @"SELECT DISTINCT PayGrade FROM Sys_User
WHERE CPA='是' AND IsDelete=0 AND Status=0  and PayGrade<>'' and " + where;
                }
                else if (type == 1)
                {
                    sql = @"SELECT DISTINCT PayGrade FROM Sys_User 
WHERE UserId IN (
SELECT distinct UserId FROM View_CheckUser 
) AND Status=0 AND IsDelete=0 and PayGrade<>'' and" + where;
                }
                else
                {
					sql = @"SELECT DISTINCT PayGrade FROM Sys_User 
WHERE IsTeacher>=0 and Status=0 AND IsDelete=0 and PayGrade<>''
AND UserId IN (SELECT distinct Teacher FROM Co_Course
UNION 
SELECT DISTINCT Teacher FROM Dep_Course) and" + where;
                }

                return conn.Query<string>(sql).ToList();
            }
        }


        /// <summary>
        /// CPA关系所在地
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<string> GetCPARelationship(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT CPARelationship FROM (
SELECT CASE WHEN  CPARelationship='立信上海总所' THEN 1
when CPARelationship='无' then 0  ELSE 2 END number,CPARelationship FROM (
SELECT DISTINCT CASE when CPARelationship='' THEN '无' ELSE CPARelationship end CPARelationship FROM Sys_User
WHERE CPA='是' ) t  ) r where {0} ORDER BY number ", where);
                return conn.Query<string>(sql).ToList();
            }
        }

        /// <summary>
        /// 周期内完成道德培训的人员ID
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<int> GetzhouqiIsDaode(string startDate, string endDate)
        {
            using (var conn = OpenConnection())
            {
                var str = string.Format(@"SELECT  vcs.UserID
FROM VIEW_CpaUserScore vcs
WHERE StartTime>='{0}' AND EndTime<='{1}'
AND IsMust=1", startDate, endDate);
                return conn.Query<int>(str).ToList();
            }
        }
        #endregion

        #region 用于报表的查询
        /// <summary>
        /// 人员获取的所有其他形式、免修----用于报表 执业CPA继续教育学时统计，针对性较强
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<FreeUseScore> GetFreeUserApply(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT ApplyUserID, ApplyID, ApplyContent,sum(GetCPAScore) GetCPAScore,ApplyType,Year,typeForm FROM (
SELECT fuo.ApplyUserID,fuo.ApplyID,
ApplyContent=
(CASE WHEN ApplyType=2 THEN (SELECT FreeName FROM Free_ApplyConfig WHERE ID=fuo.ApplyID)
ELSE (SELECT ApplyContent FROM Free_OtherApplyConfig WHERE ID=fuo.ApplyID) END
),fuo.GetCPAScore,fuo.ApplyType,Year,typeForm
FROM Free_UserOtherApply  fuo
WHERE CreateUserID=0 AND fuo.Status=1 AND fuo.ApproveStatus=1 AND fuo.DepApproveStatus=1
AND fuo.IsDelete=0  AND (typeForm=0 OR typeForm=1 OR typeForm=4)
UNION
SELECT fuo.ApplyUserID,fuo.ApplyID,cc.CourseName ApplyContent,fuo.GetCPAScore,fuo.ApplyType,fuo.Year,typeForm
FROM Free_UserOtherApply  fuo
LEFT JOIN Co_Course cc ON cc.Id=fuo.ApplyID
WHERE CreateUserID=0 
AND fuo.Status=1 AND fuo.ApproveStatus=1 AND fuo.DepApproveStatus=1 AND (typeForm=2 OR typeForm=3)
AND fuo.IsDelete=0 
)t
WHERE GetCPAScore>0  {0}
GROUP BY ApplyUserID, ApplyID, ApplyContent,ApplyType,Year,typeForm", where);
                return conn.Query<FreeUseScore>(sql).ToList();
            }
        }

        /// <summary>
        /// 人员获取的所有其他形式、免修----用于报表 执业CPA继续教育学时统计，针对性较强
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<FreeUseScore> GetFreeUserApplyDetails(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT fuo.ApplyUserID,fuo.ApplyID,
ApplyContent=
(CASE WHEN ApplyType=2 THEN (SELECT FreeName FROM Free_ApplyConfig WHERE ID=fuo.ApplyID)
ELSE (SELECT ApplyContent FROM Free_OtherApplyConfig WHERE ID=fuo.ApplyID) END
),fuo.GetCPAScore,fuo.ApplyType,Year,typeForm,ApplyTime
FROM Free_UserOtherApply  fuo
WHERE CreateUserID=0 AND fuo.Status=1 AND fuo.ApproveStatus=1 AND fuo.DepApproveStatus=1
AND fuo.IsDelete=0  AND (typeForm=0 OR typeForm=1 OR typeForm=4) AND GetCPAScore>0 {0}
UNION
SELECT fuo.ApplyUserID,fuo.ApplyID,cc.CourseName ApplyContent,fuo.GetCPAScore,fuo.ApplyType
,fuo.Year,typeForm,ApplyTime
FROM Free_UserOtherApply  fuo
LEFT JOIN Co_Course cc ON cc.Id=fuo.ApplyID
WHERE CreateUserID=0 
AND fuo.Status=1 AND fuo.ApproveStatus=1 AND fuo.DepApproveStatus=1 AND (typeForm=2 OR typeForm=3)
AND fuo.IsDelete=0  AND GetCPAScore>0 
 {0}", where);
                return conn.Query<FreeUseScore>(sql).ToList();
            }
        }

        /// <summary>
        /// 其他有组织形式
        /// </summary>
        /// <returns></returns>
        public List<FreeUseScore> GetOrgList(string where="1=1")
        {
            using (var conn=OpenConnection())
            {
                var sql = string.Format(@"SELECT ApplyUserID,fuo.CourseName  ApplyContent,GetCPAScore,StartTime, EndTime,fuo.Year   FROM Free_UserApplyOtherForm fuo
WHERE fuo.Status=1 AND fuo.ApproveStatus=1 AND fuo.DepApproveStatus=1
AND fuo.IsDelete=0 AND fuo.GetCPAScore>0 {0}

", where);
                return conn.Query<FreeUseScore>(sql).ToList();
            }
        }
        #endregion
    }
}
