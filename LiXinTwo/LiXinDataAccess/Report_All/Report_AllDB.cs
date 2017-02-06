using System.Collections.Generic;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.User;
using LiXinModels.SystemManage;

namespace LiXinDataAccess.Report_All
{
    public class Report_AllDB : BaseRepository
    {
        /// <summary>
        /// 获取部门/分所的基本信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Report_Dept_User> GetReport_DeptList(string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (SELECT DepartmentId,DeptName,dbo.GetDeptPathByDeptID(DepartmentId) AS DeptPath,
dbo.GetDeptIdPathByDeptID(DepartmentId) as childID,deptFrom=0 
FROM sys_department WHERE IsDelete=0 AND ParentDeptId IN (
SELECT DepartmentId FROM sys_department WHERE IsDelete=0 AND ParentDeptId IN(
SELECT DepartmentId FROM sys_department WHERE IsDelete=0 AND ParentDeptId=1) AND DeptName LIKE '%上海%') 
UNION 
SELECT DepartmentId,DeptName,dbo.GetDeptPathByDeptID(DepartmentId) AS DeptPath,
dbo.GetDeptIdPathByDeptID(DepartmentId) as childID,deptFrom=1 
FROM sys_department WHERE IsDelete=0 AND ParentDeptId IN(
SELECT DepartmentId FROM sys_department WHERE IsDelete=0 AND ParentDeptId=1) AND DeptName NOT LIKE '%上海%'
UNION 
SELECT DepartmentId,DeptName,dbo.GetDeptPathByDeptID(DepartmentId) AS DeptPath,
cast(DepartmentId AS NVARCHAR) as childID,deptFrom=0 
FROM sys_department WHERE DepartmentId IN (SELECT dtemp.DepartmentId FROM (
SELECT sd.DepartmentId,count(*) AS userCount    
FROM sys_department AS sd INNER JOIN Sys_User AS su ON su.DeptId=sd.DepartmentId
WHERE sd.DepartmentId IN (SELECT DepartmentId FROM sys_department WHERE IsDelete=0 AND ParentDeptId IN(
SELECT DepartmentId FROM sys_department WHERE IsDelete=0 AND ParentDeptId=1) AND DeptName LIKE '%上海%') 
group by sd.DepartmentId,sd.DeptName
) AS dtemp WHERE dtemp.userCount>0)
UNION 
SELECT DepartmentId,DeptName,dbo.GetDeptPathByDeptID(DepartmentId) AS DeptPath,
cast(DepartmentId AS NVARCHAR) as childID,deptFrom=1 
FROM sys_department WHERE DepartmentId IN (SELECT dtemp.DepartmentId FROM (
SELECT sd.DepartmentId,count(*) AS userCount    
FROM sys_department AS sd INNER JOIN Sys_User AS su ON su.DeptId=sd.DepartmentId
WHERE sd.DepartmentId IN (
SELECT DepartmentId FROM sys_department WHERE IsDelete=0 AND ParentDeptId=1) AND sd.DeptName NOT LIKE '%上海%'  
group by sd.DepartmentId,sd.DeptName
) AS dtemp WHERE dtemp.userCount>0)) AS temp WHERE " + where);
                return conn.Query<Report_Dept_User>(sql).ToList();
            }
        }

        /// <summary>
        /// 纳入考核范围的所有人员
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Sys_User> GetReport_UserList(string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT su.UserId,su.CPA,su.SubordinateSubstation,su.DeptId,su.TrainGrade,su.PayGrade,su.CPAStatus FROM View_CheckUser AS vcu 
LEFT JOIN Sys_User AS su ON su.UserId=vcu.UserId WHERE su.IsDelete=0 and su.status=0 and " + where);
                return conn.Query<Sys_User>(sql).ToList();
            }
        }

        /// <summary>
        /// 纳入考核范围的所有人员
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Sys_User> GetReport_AllUserList(string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT su.UserId,su.CPA,su.SubordinateSubstation,su.DeptId,su.TrainGrade,su.PayGrade,su.CPAStatus FROM Sys_User AS su 
WHERE su.IsDelete=0 AND su.Status=0 AND su.UserType IN ('在职','见习','特批','聘用')
AND su.IsTeacher<2");
                return conn.Query<Sys_User>(sql).ToList();
            }
        }


        /// <summary>
        /// 获取课程的基本信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Report_Course_show> GetReport_CourseList(string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT cco.UserId,cc.Id,cc.CourseName,YearPlan,IsMust,Way,StartTime,EndTime,CourseLength,IsCPA,IsTest,IsPing,
case WHEN (dbo.F_NewIsAppStatus(cc.Id,cco.UserId,cco.OrderStatus)=1)   THEN cco.GetScore  ELSE  0 END AS GetScore,
PassStatus,cast('0' as decimal(18,2)) AS AttScore,cast('0' as decimal(18,2)) AS EvlutionScore,
cast('0' as decimal(18,2)) AS ExamScore,ccs.GetLength AS CPAGetLength,
ccs.IsPass AS CPAIsPass,ccs.CpaFlag,courseFrom=0 
FROM Cl_CourseOrder AS cco 
LEFT JOIN Co_Course AS cc ON cc.Id=cco.CourseId 
LEFT JOIN Cl_CpaLearnStatus AS ccs ON (ccs.CourseID=cco.CourseId AND ccs.UserID=cco.UserId)
UNION 
SELECT ccs.UserId,cc.Id,cc.CourseName,YearPlan,IsMust,Way,StartTime,EndTime,CourseLength,IsCPA,IsTest,IsPing,
GetScore=0,PassStatus=0,cast('0' as decimal(18,2)) AS AttScore,cast('0' as decimal(18,2)) AS EvlutionScore,
cast('0' as decimal(18,2)) AS ExamScore,ccs.GetLength AS CPAGetLength,
ccs.IsPass AS CPAIsPass,ccs.CpaFlag,courseFrom=0 
FROM Cl_CpaLearnStatus AS ccs 
LEFT JOIN Co_Course AS cc ON cc.Id=ccs.CourseId 
WHERE ccs.CpaFlag IN (0,1)
UNION 
SELECT dco.UserId,dc.Id,dc.CourseName,YearPlan,IsMust,Way,StartTime,EndTime,CourseLength,IsCPA,IsTest,IsPing,
cast(dco.GetScore as decimal(18,2)) AS GetScore,cast(PassStatus as INT) AS PassStatus,AttScore,EvlutionScore,ExamScore,
dcs.GetLength AS CPAGetLength,
CPAIsPass=1,dcs.CpaFlag,courseFrom=1 
FROM Dep_CourseOrder AS dco 
LEFT JOIN Sys_User AS su ON dco.UserId=su.UserId  
LEFT JOIN Dep_Course AS dc ON dc.Id=dco.CourseId 
LEFT JOIN Dep_CpaLearnStatus AS dcs ON (dcs.CourseID=dc.Id AND dcs.UserID=dco.UserId)
LEFT JOIN Dep_CourseDept AS dcd ON (dcd.CourseId=dco.CourseId AND dcd.DepartId=su.DeptId) 
WHERE dcd.AttFlag=1 AND dcd.ApprovalFlag=1
) AS temp WHERE " + where);
                return conn.Query<Report_Course_show>(sql).ToList();
            }
        }


        /// <summary>
        /// 免修的人数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Report_User> GetFreePassUserID(string JoinDate, string CPACreateDate, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var dateWhere = " 1=1";
                if (!string.IsNullOrEmpty(JoinDate))
                {
                    dateWhere = string.Format(" JoinDate>='{0}' OR CPACreateDate>='{1}'", JoinDate, CPACreateDate);
                }

                var sql = string.Format(@"SELECT DISTINCT  ApplyUserID UserId, DeptId,typeForm,JoinDate,CPACreateDate,TrainGrade,CPA,PayGrade FROM(
SELECT  ApplyUserID,syu.DeptId,0 typeForm,JoinDate,CPACreateDate,syu.TrainGrade,syu.CPA,syu.PayGrade  FROM Free_UserOtherApply fuo
LEFT JOIN Sys_User syu ON fuo.ApplyUserID=syu.UserId
WHERE fuo.ApproveStatus=1 AND fuo.DepApproveStatus=1 AND  ApplyType=2 AND fuo.IsDelete=0 AND ApplyUserID>0 AND fuo.Status=1
AND UserType IN ('在职','见习','特批','聘用')
and {1}
UNION
SELECT UserId ApplyUserID, DeptId,4 typeForm,JoinDate,CPACreateDate,TrainGrade,CPA,PayGrade  FROM Sys_User
WHERE ({0})
AND IsDelete=0 AND Status=0 AND IsTeacher<2
AND UserType IN ('在职','见习','特批','聘用')
) t", dateWhere, where);
                return conn.Query<Report_User>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取所有人员的其他形式、免修的学时数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetFreeOtherScore(string year, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT ApplyUserID,Year,sum(GettScore) GettScore,sum(GetCPAScore) GetCPAScore,syu.DeptId,ApplyType FROM (
	SELECT sum(GettScore) GettScore, sum(GetCPAScore) GetCPAScore, ApplyUserID,fuo.Year,fuo.ApplyType FROM Free_UserOtherApply fuo
	WHERE fuo.IsDelete=0 and Status=1 AND ApproveStatus=1 AND DepApproveStatus=1 AND CreateUserID=0 
	GROUP BY ApplyUserID,Year,typeForm,ApplyType 
	UNION
    SELECT   sum(GettScore) GettScore, sum(GetCPAScore) GetCPAScore , ApplyUserID,Year,4 FROM (
	SELECT GettScore=CASE WHEN OtherFromID=0 THEN 0 ELSE GettScore  END ,GetCPAScore=CASE WHEN OtherFromID=1 THEN 0 ELSE GetCPAScore  END, ApplyUserID,Year  FROM Free_UserApplyOtherForm
	WHERE IsDelete=0 and  Status=1 AND ApproveStatus=1 AND DepApproveStatus=1 ) r
	GROUP BY ApplyUserID,Year
) t
LEFT JOIN Sys_User syu ON syu.UserId=t.ApplyUserID
where  Year in ({0}) and {1}
GROUP BY   ApplyUserID,Year,DeptId,ApplyType

", year, where);
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }

    }
}
