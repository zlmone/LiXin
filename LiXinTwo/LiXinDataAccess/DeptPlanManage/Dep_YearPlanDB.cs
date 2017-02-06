using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.DepPlanManage;
using LiXinModels.User;

namespace LiXinDataAccess.DeptPlanManage
{
    public class Dep_YearPlanDB : BaseRepository
    {
        /// <summary>
        /// 判断 年度计划 是否存在
        /// </summary>
        /// <param name="YearName">年度计划名陈</param>
        /// <returns></returns>
        public bool Exists(int YearName)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select count(1) from Dep_YearPlan WHERE IsDelete = 0 and Year = @YearName";
                var param = new { YearName = YearName };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count < 1;
            }
        }

        /// <summary>
        /// 获得已经存在的部门ID
        /// </summary>
        public string GetYearDepIDs(int year)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "SELECT dbo.f_GetYearDeptIDs(" + year + ")";
                return conn.Query<string>(sqlstr).FirstOrDefault();
            }
        }


        /// <summary>
        /// 获取计划的列表
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Dep_YearPlan> GetDepYearPlanList(string deptIDs, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (SELECT row_number()OVER(ORDER BY temp.Year asc) number,count(*)OVER(PARTITION BY null) totalcount,temp.*,sdt.DeptName AS linkDeptName FROM (SELECT t0.*,t1.Realname,t2.DeptName,'0' AS linkdepID,
(SELECT sum(dc.CourseLength) FROM Dep_YearPlanCourse AS dypc LEFT JOIN Dep_Course AS dc ON dc.Id=dypc.CourseId WHERE dypc.YearId=t0.Id) as courseLength,
(SELECT count(1) FROM Dep_YearPlanCourse WHERE YearId=t0.Id) AS courseCount,
(SELECT count(1) FROM Dep_LinkDepart WHERE YearId=t0.Id) AS linkCount,
(SELECT count(1) FROM Dep_LinkDepart WHERE YearId=t0.Id AND ApprovalFlag=1) AS linkYesCount  
FROM Dep_YearPlan AS t0 
LEFT JOIN Sys_User AS t1 on t0.UserID=t1.UserId 
LEFT JOIN Sys_Department AS t2 ON t2.DepartmentId=t0.DeptId 
WHERE t0.DeptId IN (" + deptIDs + @") 
UNION 
SELECT t0.*,t1.Realname,t2.DeptName,t3.DeptId AS linkdepID,
(SELECT sum(dc.CourseLength) FROM Dep_YearPlanCourse AS dypc LEFT JOIN Dep_Course AS dc ON dc.Id=dypc.CourseId WHERE dypc.YearId=t0.Id) as courseLength,
(SELECT count(1) FROM Dep_YearPlanCourse WHERE YearId=t0.Id) AS courseCount,
(SELECT count(1) FROM Dep_LinkDepart WHERE YearId=t0.Id) AS linkCount,
(SELECT count(1) FROM Dep_LinkDepart WHERE YearId=t0.Id AND ApprovalFlag=1) AS linkYesCount  
FROM Dep_LinkDepart AS t3 
LEFT JOIN Dep_YearPlan AS t0 ON t0.Id=t3.YearId 
LEFT JOIN Sys_User AS t1 on t0.UserID=t1.UserId 
LEFT JOIN Sys_Department AS t2 ON t2.DepartmentId=t0.DeptId 
WHERE t3.DeptId IN (" + deptIDs + @") AND t3.ApprovalFlag IN (0,1) and t0.PublishFlag=1 and t0.IsDelete=0 
) AS temp LEFT JOIN Sys_Department AS sdt ON sdt.DepartmentId=temp.linkdepID where " + where + @" ) result WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex");
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Dep_YearPlan>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        public void AddDepYearPlan(Dep_YearPlan model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO Dep_YearPlan(LastUpdateTime,Year,UserID,PublishFlag,IsDelete,IsOpen,DeptId) VALUES (@LastUpdateTime,@Year,@UserID,@PublishFlag,@IsDelete,@IsOpen,@DeptId)SELECT @@IDENTITY AS Id";
                var param = new
                {
                    model.LastUpdateTime,
                    model.Year,
                    model.UserID,
                    model.PublishFlag,
                    model.IsDelete,
                    model.IsOpen,
                    model.DeptId
                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateDepYearPlan(Dep_YearPlan model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Dep_YearPlan SET LastUpdateTime = @LastUpdateTime,Year = @Year,UserID = @UserID,PublishFlag = @PublishFlag,IsDelete = @IsDelete,IsOpen = @IsOpen,DeptId = @DeptId WHERE Id=@Id";
                var param = new
                {
                    model.LastUpdateTime,
                    model.Year,
                    model.UserID,
                    model.PublishFlag,
                    model.IsDelete,
                    model.IsOpen,
                    model.DeptId,
                    model.Id
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteDepYearPlan(string Ids)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"update Dep_YearPlan set IsDelete = 1 where Id in (" + Ids + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        public Dep_YearPlan GetDepYearPlan(int Id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select t0.*,t1.Realname,dbo.f_GetLinkDeptIDs(" + Id + ") AS DepIds,dbo.f_GetYearDeptIDs(t0.Year) as AllDepIds,(SELECT count(1) FROM Dep_YearPlanCourse WHERE YearId=t0.Id) as courseCount,(SELECT count(1) FROM Dep_LinkDepart WHERE YearId=t0.Id) AS linkCount,(SELECT count(1) FROM Dep_LinkDepart WHERE YearId=t0.Id AND ApprovalFlag=1) AS linkYesCount,t2.DeptName  from Dep_YearPlan AS t0,Sys_User AS t1,Sys_Department as t2 where t0.UserID=t1.UserId and t0.DeptId=t2.DepartmentId and t0.Id=@Id and t0.IsDelete=0";
                return conn.Query<Dep_YearPlan>(sqlstr, new { Id = Id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        public Dep_YearPlan GetDepYearPlanByYear(int name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select t0.* from Dep_YearPlan AS t0 where t0.Year=@Year and t0.IsDelete=0";
                return conn.Query<Dep_YearPlan>(sqlstr, new { Year = name }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Dep_YearPlan> GetDepYearList(int deptid, string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select * from Dep_YearPlan where IsDelete=0 AND (DeptId=@DeptId OR (Id IN (SELECT YearId FROM Dep_LinkDepart WHERE DeptId=@DeptId AND ApprovalFlag IN (1)))) and " + where + " ORDER BY Year asc";
                return conn.Query<Dep_YearPlan>(sql, new { DeptId = deptid }).ToList();
            }
        }

        /// <summary>
        /// 获得已经存在的年度
        /// </summary>
        public List<int> GetDepYear(int deptid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "SELECT Year FROM Dep_YearPlan WHERE IsDelete=0 and PublishFlag=1 AND (DeptId=@DeptId OR (Id IN (SELECT YearId FROM Dep_LinkDepart WHERE DeptId=@DeptId AND ApprovalFlag IN (1))))";
                return conn.Query<int>(sqlstr, new { DeptId = deptid }).ToList();
            }
        }
        /// <summary>
        /// 获得已经存在的年度
        /// </summary>
        public List<int> GetYear(string where = " 1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "SELECT DISTINCT Year FROM Dep_YearPlan WHERE IsDelete=0 and " + where;
                return conn.Query<int>(sqlstr).ToList();
            }
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Dep_YearPlan> GetDepYearListByWhere(string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"select * from Dep_YearPlan where IsDelete=0 and PublishFlag=1 and " + where);
                return conn.Query<Dep_YearPlan>(sql).ToList();
            }
        }

        /// <summary>
        /// 获得被联合上报部门为0
        /// </summary>
        /// <returns>执行结果</returns>
        public List<Dep_YearPlan> GetDepLink_ApprovalFlagList(int year, int deptid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"SELECT * FROM Dep_YearPlan WHERE Id IN (SELECT YearId FROM Dep_LinkDepart WHERE DeptId=@DeptId AND ApprovalFlag=0) AND IsDelete=0 AND PublishFlag=1 AND Year=@Year";
                var param = new
                {
                    DeptId = deptid,
                    Year = year
                };
                return conn.Query<Dep_YearPlan>(sqlwhere, param).ToList();
            }
        }

        /// <summary>
        /// 获得所有被联合上报部门为0
        /// </summary>
        /// <returns>执行结果</returns>
        public List<Dep_YearPlan> GetDepLink_ApprovalFlagList(int deptid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"SELECT * FROM Dep_YearPlan WHERE Id IN (SELECT YearId FROM Dep_LinkDepart WHERE DeptId=@DeptId AND ApprovalFlag=0) AND IsDelete=0 AND PublishFlag=1";
                var param = new
                {
                    DeptId = deptid
                };
                return conn.Query<Dep_YearPlan>(sqlwhere, param).ToList();
            }
        }

        /// <summary>
        /// 修改其他被联合上报部门为拒绝
        /// </summary>
        /// <returns>执行结果</returns>
        public bool UpdateDepLink_ApprovalFlag(int year, int deptid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"UPDATE Dep_LinkDepart SET ApprovalFlag =2,ApprovalTime =getdate() WHERE YearId IN (
SELECT Id FROM Dep_YearPlan WHERE Id IN (SELECT YearId FROM Dep_LinkDepart WHERE DeptId=@DeptId AND ApprovalFlag=0) 
AND IsDelete=0 AND PublishFlag=1 AND Year=@Year) AND DeptId=@DeptId AND ApprovalFlag=0";
                var param = new
                {
                    DeptId = deptid,
                    Year = year
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 获取审批计划的列表
        /// </summary>
        /// <returns></returns>
        public List<Dep_YearPlan> GetLinkYearPlanList(string deptIDs, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (SELECT row_number()OVER(ORDER BY t0.Year asc) number,count(*)OVER(PARTITION BY null) totalcount,
t0.*,t1.Realname,t2.DeptName,t3.DeptId AS linkdepID,t3.ApprovalFlag,
(SELECT sum(dc.CourseLength) FROM Dep_YearPlanCourse AS dypc LEFT JOIN Dep_Course AS dc ON dc.Id=dypc.CourseId WHERE dypc.YearId=t0.Id) as courseLength,
(SELECT count(1) FROM Dep_YearPlanCourse WHERE YearId=t0.Id) AS courseCount,
(SELECT count(1) FROM Dep_LinkDepart WHERE YearId=t0.Id) AS linkCount,
(SELECT count(1) FROM Dep_LinkDepart WHERE YearId=t0.Id AND ApprovalFlag=1) AS linkYesCount 
FROM Dep_LinkDepart AS t3 
LEFT JOIN Dep_YearPlan AS t0 ON t0.Id=t3.YearId 
LEFT JOIN Sys_User AS t1 on t0.UserID=t1.UserId 
LEFT JOIN Sys_Department AS t2 ON t2.DepartmentId=t0.DeptId 
WHERE t3.DeptId IN (" + deptIDs + @") and t0.PublishFlag=1 and " + where + @" ) result WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex");
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Dep_YearPlan>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 年度计划跟踪-部门/分所(已上报)
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="deptype">"in": 部门,"not in":分所</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Dep_YearPlan> GetYearPlanTrackList(int startIndex = 1, int startLength = int.MaxValue, string deptype = "in", string where = "1=1", string jsRenderSortField = "tempTa.Year asc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (SELECT row_number()OVER(" + jsRenderSortField + @") number,count(*)OVER(PARTITION BY null) totalcount,tempTa.* FROM (
SELECT t0.*,t1.Realname,t2.DeptName,
dbo.f_GetLinkDeptIDs(t0.Id) AS AllDepIds,
dbo.f_GetLinkDeptNameByYearID(t0.Id) AS DeptNames,
(SELECT sum(dc.CourseLength) FROM Dep_YearPlanCourse AS dypc 
LEFT JOIN Dep_Course AS dc ON dc.Id=dypc.CourseId WHERE dypc.YearId=t0.Id) as courseLength,
(SELECT count(1) FROM Dep_YearPlanCourse WHERE YearId=t0.Id) AS courseCount,
(SELECT count(1) FROM Dep_LinkDepart WHERE YearId=t0.Id) AS linkCount,
(SELECT count(1) FROM Dep_LinkDepart WHERE YearId=t0.Id AND ApprovalFlag=1) AS linkYesCount 
FROM Dep_YearPlan AS t0 
LEFT JOIN Sys_User AS t1 on t0.UserID=t1.UserId 
LEFT JOIN Sys_Department AS t2 ON t2.DepartmentId=t0.DeptId 
WHERE t0.PublishFlag=1 AND t0.IsDelete=0 AND t0.DeptId " + deptype + @" (SELECT DISTINCT temp.DepartmentId FROM (
SELECT DepartmentId FROM Sys_Department WHERE IsDelete=0 AND ParentDeptId IN 
(SELECT DepartmentId FROM Sys_Department WHERE IsDelete=0 AND DeptName like '%上海%') 
UNION SELECT DepartmentId FROM Sys_Department WHERE IsDelete=0 AND DeptName like '%上海%') AS temp)) AS tempTa where " + where + @" ) result WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex");
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Dep_YearPlan>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 年度计划跟踪-部门/分所(未上报)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptype">0: 部门,1:分所</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Sys_Department> GetNoTrackList(int year, int startIndex = 1, int startLength = int.MaxValue, int deptype = 0, string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (SELECT row_number()OVER(ORDER BY tempTa.DepartmentId asc) number,count(*)OVER(PARTITION BY null) totalcount,tempTa.*,sd.DeptName,dbo.GetDeptPathByDeptID(tempTa.DepartmentId) AS DeptPath FROM (
SELECT DISTINCT temp.DepartmentId FROM (
SELECT DepartmentId FROM Sys_Department WHERE IsDelete=0 AND ParentDeptId IN 
(SELECT DepartmentId FROM Sys_Department WHERE IsDelete=0 AND DeptName like '%上海%') 
UNION SELECT DepartmentId FROM Sys_Department WHERE IsDelete=0 AND DeptName like '%上海%') AS temp) AS tempTa 
LEFT JOIN Sys_Department AS sd ON sd.DepartmentId=tempTa.DepartmentId 
WHERE tempTa.DepartmentId NOT IN(SELECT DeptId FROM Dep_YearPlan WHERE IsDelete=0 AND Year=@Year) AND 
tempTa.DepartmentId NOT IN(SELECT DeptId FROM Dep_LinkDepart WHERE YearId IN 
(SELECT Id FROM Dep_YearPlan WHERE IsDelete=0 AND Year=@Year) AND ApprovalFlag IN(0,1)) and " + where + @" ) result WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex");
                if (deptype == 1)
                {
                    sql = string.Format(@"SELECT * FROM (SELECT row_number()OVER(ORDER BY sd.DepartmentId asc) number,count(*)OVER(PARTITION BY null) totalcount,sd.DepartmentId,sd.DeptName,dbo.GetDeptPathByDeptID(sd.DepartmentId) AS DeptPath FROM Sys_Department AS sd WHERE sd.DepartmentId NOT in (
SELECT DISTINCT temp.DepartmentId FROM (
SELECT DepartmentId FROM Sys_Department WHERE IsDelete=0 AND ParentDeptId IN 
(SELECT DepartmentId FROM Sys_Department WHERE IsDelete=0 AND DeptName like '%上海%') 
UNION SELECT DepartmentId FROM Sys_Department WHERE IsDelete=0 AND DeptName like '%上海%') AS temp) 
AND sd.IsDelete=0 and sd.DepartmentId NOT IN(SELECT DeptId FROM Dep_YearPlan WHERE IsDelete=0 AND Year=@Year) AND 
sd.DepartmentId NOT IN(SELECT DeptId FROM Dep_LinkDepart WHERE YearId IN 
(SELECT Id FROM Dep_YearPlan WHERE IsDelete=0 AND Year=@Year) AND ApprovalFlag IN(0,1)) and " + where + @" ) result WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex");
                }
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    Year = year
                };
                return conn.Query<Sys_Department>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 根据部门ID和年 查处联合和同意被联合的所有部门
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Sys_Department> GetALLLinkDepart(int deptid, int year)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"select * from Sys_Department where DepartmentId={0} or 
DepartmentId in(
select deptid from Dep_YearPlan where id in(
select yearid from Dep_LinkDepart where deptid={0} and ApprovalFlag=1) and [year]={1} and IsDelete=0 and PublishFlag=1 
)
or DepartmentId in(
select deptid from Dep_LinkDepart
where yearid in(
select id from Dep_YearPlan where deptid={0} and [year]={1} and IsDelete=0 and PublishFlag=1  ) and ApprovalFlag=1
)", deptid, year);

                return conn.Query<Sys_Department>(sql).ToList();
            }
        }

        /// <summary>
        /// 发布年度计划 查看是否有计划被发布
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetIsOrNoLinkDepart(int deptid, int year)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@" select YearId from Dep_LinkDepart where DeptId={0} and ApprovalFlag=1 AND YearId 
IN (SELECT Id FROM Dep_YearPlan WHERE Year={1} AND PublishFlag=1 AND IsDelete=0) UNION SELECT Id AS YearId FROM Dep_YearPlan WHERE Year={1} AND PublishFlag=1 AND IsDelete=0 AND DeptId={0}", deptid, year);

                return conn.Query<int>(sql).ToList();
            }
        }

        /// <summary>
        /// A部门被联合后 在点发布 此时审批就不能在联合
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dep_YearPlan GetPublicIsOrNoLinkDepart(int deptid, int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"  select * from dbo.Dep_YearPlan where deptid={0} and 
  [year]=(select [year] from dbo.Dep_YearPlan where id={1})
  and publishflag=1 and isdelete=0", deptid, id);

                return conn.Query<Dep_YearPlan>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 发布年度计划修改联合上报部门为拒绝(联合发布)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateLinkandYearPlan(int year, int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"UPDATE Dep_LinkDepart SET ApprovalFlag=2,ApprovalTime=getdate() 
WHERE DeptId IN (SELECT DeptId FROM Dep_YearPlan WHERE Year=@Year AND DeptId IN (
SELECT DeptId FROM Dep_LinkDepart WHERE YearId=@YearId) AND PublishFlag=1 AND IsDelete=0 
UNION 
SELECT dld.DeptId FROM Dep_LinkDepart AS dld 
LEFT JOIN Dep_YearPlan AS dyp ON dld.YearId=dyp.Id 
WHERE dyp.Year=@Year AND dyp.PublishFlag=1 AND dyp.IsDelete=0 AND dld.ApprovalFlag=1 
AND dld.DeptId IN (SELECT DeptId FROM Dep_LinkDepart WHERE YearId=@YearId)
) AND YearId=@YearId";
                var param = new
                {
                    Year = year,
                    YearId = id
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 发布年度计划修改联合上报部门为拒绝(单体发布)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateLinkDepart(int year, int dpetid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"UPDATE Dep_LinkDepart SET ApprovalFlag=2,ApprovalTime=getdate() 
WHERE DeptId=@DeptId AND ApprovalFlag=0 AND YearId IN 
(SELECT Id FROM Dep_YearPlan WHERE Year=@Year AND PublishFlag=1 AND IsDelete=0 AND IsOpen=1)";
                var param = new
                {
                    Year = year,
                    DeptId = dpetid
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 被联合的部门其主部门的名称
        /// </summary>
        /// <returns></returns>
        public List<Sys_Department> GetLinkDept()
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = @"SELECT dld.DeptId DepartmentId,dyp.DeptId LinkDeptId,sd.DeptName FROM Dep_LinkDepart  dld
LEFT JOIN Dep_YearPlan dyp ON dld.YearId=dyp.Id
LEFT JOIN Sys_Department sd ON sd.DepartmentId=dyp.DeptId
WHERE ApprovalFlag=1";
                return conn.Query<Sys_Department>(sql).ToList();
            }
        }
    }
}
