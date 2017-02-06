using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.Report_zAttendce;
using LiXinModels.CourseLearn;

namespace LiXinDataAccess
{
    public class zAttendceDB : BaseRepository
    {
        /// <summary>
        /// 根据条件获得所有的缺勤情况
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<zAttendce> GetAllAttendce(string where, string whereDate = "", string whereDateT = "",
                                              string whereDateL = "", int startIndex = 0, int pageLength = int.MaxValue,
                                              string jsRenderSortField = "", string deptName = "", string whereDeptID = "")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string[] temp = new string[16];
                if (whereDateT != "")
                {
                    var a = whereDateT.Split(';');
                    for (int i = 0; i < a.Count(); i++)
                    {
                        temp[i] = a[i];
                    }
                }
                if (whereDateL != "")
                {
                    var a = whereDateT.Split(';');
                    for (int i = 0; i < a.Count(); i++)
                    {
                        temp[i + 8] = a[i];
                    }
                }
                string sql = string.Format(@"

WITH depAtt AS
(
	SELECT UserId, ApprovalFlag, Status
	FROM  Dep_Attendce da
	LEFT JOIN Dep_Course co ON co.Id=da.CourseId
	WHERE  ApprovalFlag=1 {1}
),deptAtt AS
(
  	SELECT UserId, ApprovalFlag, Status
	FROM  DepTran_Attendce da
	LEFT JOIN Co_Course co ON co.Id=da.CourseId
	WHERE  ApprovalFlag=1 {1}
)

select * from
(
select row_number()over(ORDER BY a1.DepartmentId asc) as rowNum,count(a1.DepartmentId)OVER(PARTITION BY null) as totalcount,
a1.DepartmentId,a1.DeptName,a1.Att,a2.Late,a3.Early,a4.LateAndEarly
,a5.Att as Att2,a5.Late as Late2,a5.Early as Early2,a5.LateAndEarly as LateAndEarly2
,a5.AttT as Att3,a5.LateT as Late3,a5.EarlyT as Early3,a5.LateAndEarlyT as LateAndEarly3
,dbo.GetDeptPathByDeptID(a1.DepartmentId)as DeptNames,dbo.GetDeptTwoPathByDeptID(a1.DepartmentId) fenDeptName 
from (
select sd.DepartmentId,sd.DeptName
,count(DISTINCT temp1.UserId) as Att
from Sys_Department sd
left join (
select ca.UserId,co.AttFlag,co.StartTime,co.EndTime
,su.DeptId
from Cl_Attendce ca 
left join Co_Course co on ca.CourseId=co.Id {1}
left join Sys_User su on su.UserId=ca.UserId 
left join Sys_Department sd on sd.DepartmentId=su.DeptId
where    ((co.AttFlag=1 and ca.StartTime='2050-1-1') 
      or (co.AttFlag=2 and ca.EndTime='2000-1-1') 
      or (co.AttFlag=3 and ca.StartTime='2050-1-1' and ca.EndTime='2000-1-1')) {0}

) as temp1 on temp1.DeptId=sd.DepartmentId
group by sd.DepartmentId,sd.DeptName

) as a1 -- 一期缺勤
--一期迟到
left join (
select sd.DepartmentId,sd.DeptName
,count(DISTINCT temp1.UserId) as Late
from Sys_Department sd
left join (
select ca.UserId,co.AttFlag,co.StartTime,co.EndTime
,su.DeptId
from Cl_Attendce ca 
left join Co_Course co on ca.CourseId=co.Id {1}
left join Sys_User su on su.UserId=ca.UserId 
left join Sys_Department sd on sd.DepartmentId=su.DeptId
where    ((co.AttFlag=1 AND ca.StartTime!='2050-1-1' and ca.StartTime>dateadd(n,1,co.StartTime))
      or (co.AttFlag=3 and ca.StartTime='2050-1-1' and ca.EndTime>co.EndTime)
      or (co.AttFlag=3 and ca.StartTime>dateadd(n,1,co.StartTime) and ca.EndTime>co.EndTime)) {0}
) as temp1 on temp1.DeptId=sd.DepartmentId
group by sd.DepartmentId,sd.DeptName
)  as a2 on a2.DepartmentId=a1.DepartmentId
--一期早退
left join (
select sd.DepartmentId,sd.DeptName
,count(DISTINCT temp1.UserId) as Early
from Sys_Department sd
left join (
select ca.UserId,co.AttFlag,co.StartTime,co.EndTime
,su.DeptId
from Cl_Attendce ca 
left join Co_Course co on ca.CourseId=co.Id {1}
left join Sys_User su on su.UserId=ca.UserId 
left join Sys_Department sd on sd.DepartmentId=su.DeptId
where    ((co.AttFlag=2 and ca.EndTime<co.EndTime)
      or (co.AttFlag=3 and ca.StartTime<dateadd(n,1,co.StartTime) and ca.EndTime<co.EndTime)
      or (co.AttFlag=3 and ca.StartTime<dateadd(n,1,co.StartTime) and ca.EndTime='2000-1-1')) {0}
) as temp1 on temp1.DeptId=sd.DepartmentId
group by sd.DepartmentId,sd.DeptName
)  as a3 on a3.DepartmentId=a1.DepartmentId
--一期迟到且早退
left join (
select sd.DepartmentId,sd.DeptName
,count(DISTINCT temp1.UserId) as LateAndEarly
from Sys_Department sd
left join (
select ca.UserId,co.AttFlag,co.StartTime,co.EndTime
,su.DeptId
from Cl_Attendce ca 
left join Co_Course co on ca.CourseId=co.Id {1}
left join Sys_User su on su.UserId=ca.UserId 
left join Sys_Department sd on sd.DepartmentId=su.DeptId
where    ((co.AttFlag=3 and ca.StartTime='2050-1-1' and ca.EndTime<co.EndTime)
      or (co.AttFlag=3 and ca.StartTime>dateadd(n,1,co.StartTime) and ca.EndTime<co.EndTime)
      or (co.AttFlag=3 and ca.StartTime>dateadd(n,1,co.StartTime) and ca.EndTime='2000-1-1')) {0}
) as temp1 on temp1.DeptId=sd.DepartmentId
group by sd.DepartmentId,sd.DeptName
)  as a4 on a4.DepartmentId=a1.DepartmentId
--二期各种状态
left join(
select sd.DepartmentId,sd.DeptName
,count(da1.Status) as Att,count(da2.Status) as Late
,count(da3.Status) as Early,count(da4.Status) as LateAndEarly
,count(dta1.Status) as AttT,count(dta2.Status) as LateT
,count(dta3.Status) as EarlyT,count(dta4.Status) as LateAndEarlyT
from Sys_Department sd
left join Sys_User su on su.DeptId=sd.DepartmentId 
left join depAtt da1 on da1.UserId=su.UserId  and da1.Status=2 {2} {10}
left join depAtt da2 on da2.UserId=su.UserId  and da2.Status=3 {3} {11}
left join depAtt da3 on da3.UserId=su.UserId  and da3.Status=4 {4} {12}
left join depAtt da4 on da4.UserId=su.UserId  and da4.Status=5 {5} {13}
left join deptAtt dta1 on dta1.UserId=su.UserId  and dta1.Status=2 {6} {14}
left join deptAtt dta2 on dta2.UserId=su.UserId  and dta2.Status=3 {7} {15}
left join deptAtt dta3 on dta3.UserId=su.UserId  and dta3.Status=4 {8} {16}
left join deptAtt dta4 on dta4.UserId=su.UserId  and dta4.Status=5 {9} {17}
where 1=1 {0}
group by sd.DepartmentId,sd.DeptName
) as a5 on a5.DepartmentId=a1.DepartmentId
where a1.DepartmentId>1 and a1.DepartmentId in (SELECT DeptId FROM dbo.View_CheckUser) and
 {18} and
{19}
) result
where rowNum between  @pageLength*@startIndex+1 and @pageLength*(@startIndex+1)"
                                           , where, whereDate, temp[0], temp[1], temp[2], temp[3], temp[4], temp[5],
                                           temp[6], temp[7], temp[8], temp[9], temp[10], temp[11], temp[12], temp[13],
                                           temp[14], temp[15], deptName, whereDeptID);
                var param = new
                    {
                        pageLength = pageLength,
                        startIndex = startIndex
                    };
                return connection.Query<zAttendce>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 查找所有分所
        /// </summary>
        /// <returns></returns>
        public List<zAttendce> GetFengDepartment(string whereDeptID = "")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = @" select dbo.GetDeptTwoPathByDeptID(DepartmentId) as DeptName,DepartmentId  from Sys_Department where dbo.GetDeptTwoPathByDeptID(DepartmentId) like '%分所%'
                AND DepartmentId IN (SELECT DeptId FROM View_CheckUser)  and " + whereDeptID;
                return connection.Query<zAttendce>(sql).ToList();
            }
        }

        /// <summary>
        /// 每个人的考勤状态
        /// </summary>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetUserAttend(int YearPlan)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
WITH depAtt AS
(
	SELECT UserId, ApprovalFlag, Status
	FROM  Dep_Attendce da
	LEFT JOIN Dep_Course dc ON dc.Id=da.CourseId
	WHERE  ApprovalFlag=1 AND dc.YearPlan={0}
),deptAtt AS
(
  	SELECT UserId, ApprovalFlag, Status
	FROM  DepTran_Attendce da
	LEFT JOIN Co_Course cc ON cc.Id=da.CourseId
	WHERE  ApprovalFlag=1 AND cc.YearPlan={0}
)

SELECT t.UserId, sum(co_qq)+sum(dept_qq) co_qq,sum(co_cd)+sum(dept_cd) co_cd,sum(co_zt)+sum(dept_zt) co_zt,sum(co_cd_zt)+sum(dept_cd_zt) co_cd_zt,
sum(dep_qq) dep_qq,
sum(dep_cd) dep_cd,sum(dep_zt) dep_zt,sum(dep_cd_zt) dep_cd_zt FROM (
--缺勤
select ca.UserId,co.AttFlag,1 co_qq ,0 co_cd,0 co_zt,0 co_cd_zt,0 dep_qq,0 dep_cd,0 as dep_zt,0 dep_cd_zt,0 dept_qq,0 dept_cd,0 as dept_zt,0 dept_cd_zt
from Cl_Attendce ca 
left join Co_Course co on ca.CourseId=co.Id  and co.YearPlan={0}
left join Sys_User su on su.UserId=ca.UserId 
left join Sys_Department sd on sd.DepartmentId=su.DeptId
where    ((co.AttFlag=1 and ca.StartTime='2050-1-1') 
      or (co.AttFlag=2 and ca.EndTime='2000-1-1') 
      or (co.AttFlag=3 and ca.StartTime='2050-1-1' and ca.EndTime='2000-1-1')) 
UNION 
 --迟到
select ca.UserId,co.AttFlag,0 co_qq ,1 co_cd,0 co_zt,0 co_cd_zt,0 dep_qq,0 dep_cd,0 as dep_zt,0 dep_cd_zt,0 dept_qq,0 dept_cd,0 as dept_zt,0 dept_cd_zt
from Cl_Attendce ca 
left join Co_Course co on ca.CourseId=co.Id  and co.YearPlan={0}
left join Sys_User su on su.UserId=ca.UserId 
left join Sys_Department sd on sd.DepartmentId=su.DeptId
where    ((co.AttFlag=1 AND ca.StartTime!='2050-1-1' and ca.StartTime>dateadd(n,1,co.StartTime))
      or (co.AttFlag=3 and ca.StartTime='2050-1-1' and ca.EndTime>co.EndTime)
      or (co.AttFlag=3 and ca.StartTime>dateadd(n,1,co.StartTime) and ca.EndTime>co.EndTime)) 
UNION  all  
--早退
select ca.UserId,co.AttFlag,0 co_qq ,0 co_cd,1 co_zt,0 co_cd_zt,0 dep_qq,0 dep_cd,0 as dep_zt,0 dep_cd_zt,0 dept_qq,0 dept_cd,0 as dept_zt,0 dept_cd_zt
from Cl_Attendce ca 
left join Co_Course co on ca.CourseId=co.Id  and co.YearPlan={0}
left join Sys_User su on su.UserId=ca.UserId 
left join Sys_Department sd on sd.DepartmentId=su.DeptId
where    ((co.AttFlag=2 and ca.EndTime<co.EndTime)
      or (co.AttFlag=3 and ca.StartTime<dateadd(n,1,co.StartTime) and ca.EndTime<co.EndTime)
      or (co.AttFlag=3 and ca.StartTime<dateadd(n,1,co.StartTime) and ca.EndTime='2000-1-1')) 
UNION  all
--迟到并早退
select ca.UserId,co.AttFlag,0 co_qq ,0 co_cd,0 co_zt,1 co_cd_zt,0 dep_qq,0 dep_cd,0 as dep_zt,0 dep_cd_zt,0 dept_qq,0 dept_cd,0 as dept_zt,0 dept_cd_zt
from Cl_Attendce ca 
left join Co_Course co on ca.CourseId=co.Id  and co.YearPlan={0}
left join Sys_User su on su.UserId=ca.UserId 
left join Sys_Department sd on sd.DepartmentId=su.DeptId
where    ((co.AttFlag=3 and ca.StartTime='2050-1-1' and ca.EndTime<co.EndTime and ca.EndTime!='2000-1-1')
      or (co.AttFlag=3 and ca.StartTime>dateadd(n,1,co.StartTime) and ca.EndTime<co.EndTime AND  ca.StartTime!='2050-1-1' and ca.EndTime!='2000-1-1')
      or (co.AttFlag=3 and ca.StartTime>dateadd(n,1,co.StartTime) and ca.EndTime='2000-1-1' AND ca.StartTime!='2050-1-1'))
UNION  all
select su.userID,1 AttFlag,0 co_qq ,0 co_cd,0 co_zt,0 co_cd_zt
,count(da1.Status) as dep_qq,count(da2.Status) as dep_cd
,count(da3.Status) as dep_zt,count(da4.Status) as dep_cd_zt
,count(dta1.Status) as dept_qq,count(dta2.Status) as dept_cd
,count(dta3.Status) as dept_zt,count(dta4.Status) as dept_cd_zt
from  Sys_User su 
left join depAtt da1 on da1.UserId=su.UserId and da1.Status=2  
left join depAtt da2 on da2.UserId=su.UserId  and da2.Status=3  
left join depAtt da3 on da3.UserId=su.UserId  and da3.Status=4  
left join depAtt da4 on da4.UserId=su.UserId  and da4.Status=5 
left join deptAtt dta1 on dta1.UserId=su.UserId  and dta1.Status=2  
left join deptAtt dta2 on dta2.UserId=su.UserId  and dta2.Status=3  
left join deptAtt dta3 on dta3.UserId=su.UserId  and dta3.Status=4  
left join deptAtt dta4 on dta4.UserId=su.UserId  and dta4.Status=5  
GROUP BY su.userID  
 ) t 
 LEFT JOIN Sys_User syu ON t.UserId=syu.UserId
 WHERE syu.UserId IN (SELECT UserId FROM View_CheckUser) 
 GROUP BY t.UserId
 ", YearPlan);
                return conn.Query<Cl_CourseOrder>(sql).ToList();
            }
        }



        public List<Cl_CourseOrder> GetUserAttendByCourseid(int Courseid)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"

SELECT t.courseid,t.UserId,t.deptid, sum(co_qq)+sum(dept_qq) co_qq,sum(co_cd)+sum(dept_cd) co_cd,sum(co_zt)+sum(dept_zt) co_zt,sum(co_cd_zt)+sum(dept_cd_zt) co_cd_zt,
sum(dep_qq) dep_qq,
sum(dep_cd) dep_cd,sum(dep_zt) dep_zt,sum(dep_cd_zt) dep_cd_zt FROM (
--缺勤
select ca.courseid,ca.UserId,su.deptid,co.AttFlag,1 co_qq ,0 co_cd,0 co_zt,0 co_cd_zt,0 dep_qq,0 dep_cd,0 as dep_zt,0 dep_cd_zt,0 dept_qq,0 dept_cd,0 as dept_zt,0 dept_cd_zt
from Cl_Attendce ca 
left join Co_Course co on ca.CourseId=co.Id -- and co.YearPlan=2013
left join Sys_User su on su.UserId=ca.UserId 
left join Sys_Department sd on sd.DepartmentId=su.DeptId
where    ((co.AttFlag=1 and ca.StartTime='2050-1-1') 
      or (co.AttFlag=2 and ca.EndTime='2000-1-1') 
      or (co.AttFlag=3 and ca.StartTime='2050-1-1' and ca.EndTime='2000-1-1')) --and ca.courseid=467
UNION 
SELECT  Id,UserId, DeptId,1,1 co_qq ,0 co_cd,0 co_zt,0 co_cd_zt,0 dep_qq,0 dep_cd,0 as dep_zt,0 dep_cd_zt,0 dept_qq,0 dept_cd,0 as dept_zt,0 dept_cd_zt
FROM View_TogetherAtt
UNION all

 --迟到
select ca.courseid,ca.UserId,su.deptid,co.AttFlag,0 co_qq ,1 co_cd,0 co_zt,0 co_cd_zt,0 dep_qq,0 dep_cd,0 as dep_zt,0 dep_cd_zt,0 dept_qq,0 dept_cd,0 as dept_zt,0 dept_cd_zt
from Cl_Attendce ca 
left join Co_Course co on ca.CourseId=co.Id  --and co.YearPlan=2013
left join Sys_User su on su.UserId=ca.UserId 
left join Sys_Department sd on sd.DepartmentId=su.DeptId
where    ((co.AttFlag=1 AND ca.StartTime!='2050-1-1' and ca.StartTime>dateadd(n,1,co.StartTime))
      or (co.AttFlag=3 and ca.StartTime='2050-1-1' and ca.EndTime>co.EndTime)
      or (co.AttFlag=3 and ca.StartTime>dateadd(n,1,co.StartTime) and ca.EndTime>co.EndTime)) --and ca.courseid=467
UNION  all  
--早退
select ca.courseid,ca.UserId,su.deptid,co.AttFlag,0 co_qq ,0 co_cd,1 co_zt,0 co_cd_zt,0 dep_qq,0 dep_cd,0 as dep_zt,0 dep_cd_zt,0 dept_qq,0 dept_cd,0 as dept_zt,0 dept_cd_zt
from Cl_Attendce ca 
left join Co_Course co on ca.CourseId=co.Id  --and co.YearPlan=2013
left join Sys_User su on su.UserId=ca.UserId 
left join Sys_Department sd on sd.DepartmentId=su.DeptId
where    ((co.AttFlag=2 and ca.EndTime<co.EndTime)
      or (co.AttFlag=3 and ca.StartTime<dateadd(n,1,co.StartTime) and ca.EndTime<co.EndTime)
      or (co.AttFlag=3 and ca.StartTime<dateadd(n,1,co.StartTime) and ca.EndTime='2000-1-1')) --and ca.courseid=467
UNION  all
--迟到并早退
select ca.courseid,ca.UserId,su.deptid,co.AttFlag,0 co_qq ,0 co_cd,0 co_zt,1 co_cd_zt,0 dep_qq,0 dep_cd,0 as dep_zt,0 dep_cd_zt,0 dept_qq,0 dept_cd,0 as dept_zt,0 dept_cd_zt
from Cl_Attendce ca 
left join Co_Course co on ca.CourseId=co.Id  --and co.YearPlan=2013
left join Sys_User su on su.UserId=ca.UserId 
left join Sys_Department sd on sd.DepartmentId=su.DeptId
where    ((co.AttFlag=3 and ca.StartTime='2050-1-1' and ca.EndTime<co.EndTime)
      or (co.AttFlag=3 and ca.StartTime>dateadd(n,1,co.StartTime) and ca.EndTime<co.EndTime)
      or (co.AttFlag=3 and ca.StartTime>dateadd(n,1,co.StartTime) and ca.EndTime='2000-1-1')) --and ca.courseid={0}
  GROUP BY su.userID ,ca.CourseId,ca.userid,co.attflag,su.deptid
 ) t 
 LEFT JOIN Sys_User syu ON t.UserId=syu.UserId
 WHERE syu.UserId IN (SELECT UserId FROM View_CheckUser) and courseid={0}
 GROUP BY t.UserId,t.courseid,t.deptid
 ", Courseid);
                return conn.Query<Cl_CourseOrder>(sql).ToList();
            }
        }


       
    }
}
