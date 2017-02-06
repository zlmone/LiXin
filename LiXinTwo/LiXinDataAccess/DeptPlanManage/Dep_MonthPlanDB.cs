using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.DepPlanManage;
using LiXinModels.DeptCourseManage;
using LiXinModels.DepCourseManage;
using System.Collections.Generic;
using System.Data;

namespace LiXinDataAccess.DeptPlanManage
{
    public class Dep_MonthPlanDB : BaseRepository
    {
        #region 新增改查

        /// <summary>
        /// 新增一条数据月度大纲
        /// </summary>     
        public void InsertDep_MonthPlan(Dep_MonthPlan model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Dep_MonthPlan(Year,Month,LastUpdateTime,UserId,PublishFlag,IsDelete,DeptId)
	                     values( @Year,@Month,@LastUpdateTime,@UserId,@PublishFlag,@IsDelete,@DeptId);SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.Year,
                    model.Month,
                    model.LastUpdateTime,
                    model.UserId,
                    model.PublishFlag,
                    model.IsDelete,
                    model.DeptId
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.ID);
            }
        }


        /// <summary>
        /// 新增一条数据月度大纲内的课程
        /// </summary>     
        public void InsertDep_MonthPlanCourse(Dep_MonthPlanCourse model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Dep_MonthPlanCourse(MonthId,CourseId,IsSystem)
	                     values( @MonthId,@CourseId,@IsSystem);";

                var param = new
                {
                    model.MonthId,
                    model.CourseId,
                    model.IsSystem
                };
                conn.Execute(strSql, param);
            }
        }

        /// <summary>
        /// 插入课程
        /// </summary>
        /// <param name="model"></param>
        public void InsertDept_Course(Dep_Course model)
        {
            using (var conn = OpenConnection())
            {
                const string sql = @"INSERT INTO dbo.Dep_Course
	                                (
                                    Code,
	                                CourseName,
                                    Year,
                                    Month,
                                    YearPlan,
	                                PreCourseTime,
                                    Day,
	                                OpenLevel,
                                    CourseLength,
                                    IsCPA,
	                                IsMust,
	                                Teacher,
	                                Publishflag,
	                                LastUpdateTime,
                                    SurveyPaperId,
                                    CourseFrom,
	                                IsDelete,
                                    RoomId,
                                    IsSystem,
                                    IsYearPlan,
                                    DeptId
	                                )
                                VALUES 
	                                (
                                    @code,
	                                @coursename,
                                    @Year,
                                    @Month,
                                    @YearPlan,
	                                @preCourseTime,
                                    @day,
	                                @openlevel,
                                    @courselength,
                                    @iscpa,
	                                @ismust,
	                                @teacher,
	                                @publishflag,
	                                getdate(),
                                    @SurveyPaperId,
                                    @courseFrom,
	                                0,
                                    @roomId,
                                    @isSystem,
                                    @isYearPlan,
                                    @DeptId
	                                );SELECT @@IDENTITY as ID ";
                var param = new
                {
                    Year = model.Year,
                    Month = model.Month,
                    YearPlan = model.Year,
                    code = model.Code,
                    coursename = model.CourseName,
                    preCourseTime = model.PreCourseTime,
                    day = model.Day,
                    openlevel = model.OpenLevel,
                    courselength = model.CourseLength,
                    iscpa = model.IsCPA,
                    ismust = model.IsMust,
                    teacher = model.Teacher,
                    SurveyPaperId = model.SurveyPaperId,
                    courseFrom = model.CourseFrom,
                    publishflag = model.Publishflag,
                    roomId = model.RoomId,
                    isSystem = model.IsSystem,
                    isYearPlan = model.IsYearPlan,
                    DeptId=model.DeptId 
                };
                dynamic list = conn.Query<dynamic>(sql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.ID);
            }
        }


        /// <summary>
        /// 删除计划中的课程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseID"></param>
        public void DeleteMonPlanCourse(int id, string courseID)
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"
DELETE Dep_MonthPlanCourse
WHERE  MonthId={0}
AND CourseId IN ({1})", id, courseID);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="courseIDs"></param>
        public void DeleteCourse(string courseIDs)
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"UPDATE Dep_Course
SET IsDelete=1
WHERE Id in ({0})", courseIDs);
                conn.Execute(sql);
            }

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        public void UpdateMonthPlan(int id, string where)
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"UPDATE Dep_MonthPlan
                            SET {1}
                            WHERE Id={0}", id, where);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 获得单个的月度大纲
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dep_MonthPlan Get(int id)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM Dep_MonthPlan
WHERE Id={0}", id);
                return conn.Query<Dep_MonthPlan>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 删除月度计划以及下面的课程
        /// </summary>
        /// <param name="courseID"></param>
        public void DeleteMonPlanAndCourse(int MonthId)
        {
            using(var conn=OpenConnection())
            {
                var sql = string.Format(@"BEGIN TRAN
 UPDATE Dep_MonthPlan
 SET IsDelete=1
 WHERE Id= {0}
   	if @@error<>0
	begin rollback tran
	end
	ELSE
	    DELETE FROM Dep_MonthPlanCourse
	    WHERE MonthId={0}
	begin commit tran
end", MonthId);
                conn.Execute(sql);

            }
        }


        /// <summary>
        /// 获取年份
        /// </summary>
        /// <returns></returns>
        public List<int> GetYearOfPlan(int deptid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT Year FROM Dep_YearPlan
WHERE PublishFlag=1 and deptid="+deptid;
                return conn.Query<int>(sql).ToList( );
            }
        }

        #endregion

        #region 月度大纲

        /// <summary>
        /// 获取计划的基本信息
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Dep_MonthPlan> GetMonthPlan(int DeptID=0,int startIndex = 1, int startLength = int.MaxValue, string where = "1=1",string deptwhere=" 1=1", string jsRenderSortField = "  month asc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH deptIDList AS 
  (
     SELECT DeptId,year  FROM Dep_YearPlan
       WHERE Id IN (SELECT YearId FROM Dep_LinkDepart WHERE (DeptId={2} or 1={2}) and ApprovalFlag=1)
      AND DeptId NOT IN  (SELECT ID AS DeptId FROM  f_GetDeptChildByDeptID({2}))
  ) 

  SELECT * FROM (
 SELECT row_number()OVER(ORDER BY {1}) number,count(*)OVER(PARTITION BY null) totalcount,* FROM (
SELECT Id,Year,Month,LastUpdateTime,tm.UserId,PublishFlag,tm.IsDelete,su.Realname,sd.DeptName, tm.DeptId,
courseCount=(SELECT count(1) FROM Dep_MonthPlanCourse
WHERE MonthId=Id) 
FROM dbo.Dep_MonthPlan tm
left JOIN Sys_User su ON tm.UserId=su.UserId
LEFT JOIN Sys_Department sd ON sd.DepartmentId=tm.DeptId
where tm.IsDelete=0   and   ((tm.PublishFlag=1 AND tm.DeptId!={2})  OR (tm.DeptId={2}))
AND  ( tm.DeptId in (SELECT ID AS DeptId FROM  f_GetDeptChildByDeptID({2}))
OR tm.Id IN   ( SELECT dmp.Id FROM Dep_MonthPlan     dmp
      INNER JOIN deptIDList  dl ON dmp.Year=dl.Year AND dmp.DeptId=dl.DeptId)) And {0}
 )r)t
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", where, jsRenderSortField, DeptID,deptwhere);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Dep_MonthPlan>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 获取计划的基本信息
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Dep_MonthPlan> GetMonthPlanForMaoJiaYuan(int DeptID = 0, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string deptwhere = " 1=1", string beiwhere="1=1", string jsRenderSortField = "  month asc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH deptIDList AS 
  (
     SELECT ID AS DeptId FROM  f_GetDeptChildByDeptID({2}) where {3}
     UNION
     SELECT DeptId  FROM Dep_YearPlan
       WHERE Id IN (SELECT YearId FROM Dep_LinkDepart WHERE (DeptId={2} or 1={2}))
      AND DeptId NOT IN  (SELECT ID AS DeptId FROM  f_GetDeptChildByDeptID({2}))
  )

  SELECT * FROM (
 SELECT row_number()OVER(ORDER BY {1}) number,count(*)OVER(PARTITION BY null) totalcount,* FROM (
SELECT Id,Year,Month,LastUpdateTime,tm.UserId,PublishFlag,tm.IsDelete,su.Realname,sd.DeptName, tm.DeptId,
courseCount=(SELECT count(1) FROM Dep_MonthPlanCourse
WHERE MonthId=Id) 
FROM dbo.Dep_MonthPlan tm
left JOIN Sys_User su ON tm.UserId=su.UserId
LEFT JOIN Sys_Department sd ON sd.DepartmentId=tm.DeptId
where tm.IsDelete=0   and  ( ((tm.PublishFlag=1 AND tm.DeptId!={2})  OR (tm.DeptId={2}))
AND  tm.DeptId in (SELECT DeptId FROM  deptIDList) And {0}) or {4}
 )r)t
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", where, jsRenderSortField, DeptID, deptwhere, beiwhere);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Dep_MonthPlan>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 是否存在此月度计划，
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>true存在，false不存在</returns>
        public bool IsExistMonplan(int year, string month, int id, int deptID=0)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT COUNT(*) as count FROM Dep_MonthPlan
WHERE Year={0} AND Month='{1}' and IsDelete=0 AND Id<>{2} and ({3}=0 or DeptId={3})", year, month, id, deptID);
                dynamic list = conn.Query<dynamic>(sql).FirstOrDefault();
                return decimal.ToInt32(list.count) > 0;
            }
        }
        #endregion

        #region 月度分解

        /// <summary>
        /// 获取年度计划内的课程详情
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<LiXinModels.CourseShow> GetYearCourseDetails(int startIndex = 0, int startLength = int.MaxValue, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT * FROM (
SELECT row_number()OVER(ORDER BY ty.year asc) number,count(*)OVER(PARTITION BY null) totalcount, cc.Code,cc.Id,CourseName,cc.Year,cc.Month,cc.Day,OpenLevel,Way,Teacher,IsMust,tp.IsSystem,su.UserId,su.Realname,su.PayGrade,dbo.GetSystemTree(tp.IsSystem)SystemTree 
,cc.IsCPA,cc.CourseLength
FROM Dep_YearPlan ty left JOIN Dep_YearPlanCourse tp  ON tp.YearId=ty.Id
INNER JOIN Dep_Course cc ON tp.CourseId=cc.Id
left JOIN Sys_User su ON su.UserId=cc.Teacher
WHERE  cc.IsDelete=0  AND ty.PublishFlag=1 and ty.IsDelete=0 
and {0}
) result
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<LiXinModels.CourseShow>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取月度计划内的课程详情
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<LiXinModels.CourseShow> GetMonthCourseDetails(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " cc.month desc")
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT * FROM (
SELECT row_number()OVER(ORDER BY {1}) number,count(*)OVER(PARTITION BY null) totalcount, cc.Id,CourseName,cc.PreCourseTime,SurveyPaperId,cc.month,day,OpenLevel,Way,Teacher,IsMust,tp.IsSystem,su.Realname,su.PayGrade,dbo.GetSystemTree(tp.IsSystem) SystemTree 
,cc.IsCPA,cc.CourseLength,scr.RoomName
FROM Dep_MonthPlan ty left JOIN Dep_MonthPlanCourse tp  ON tp.MonthId=ty.Id
INNER JOIN Dep_Course cc ON tp.CourseId=cc.Id
left JOIN Sys_User su ON su.UserId=cc.Teacher
LEFT JOIN Dep_ClassRoom scr ON scr.Id=cc.RoomId
WHERE cc.IsDelete=0  AND ty.IsDelete=0 
and {0}
) result
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", where, jsRenderSortField);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<LiXinModels.CourseShow>(sql, param).ToList();
            }
        }
        #endregion

        #region 月度差异对比

        /// <summary>
        /// 差异对比的头部信息 年度预计课程数，实际课程数等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dep_MonthPlan GetPlanCourseCount(int id,int deptID, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT *,
yearCourseCount=(SELECT count(*) FROM Dep_YearPlanCourse WHERE YearId IN (SELECT id FROM Dep_YearPlan WHERE Year=tm.Year AND IsDelete=0 AND DeptId={2}) AND DeptId={2}),
monthCourseCount=(SELECT count(*) FROM Dep_MonthPlanCourse WHERE MonthId={0})
 FROM Dep_MonthPlan tm
 WHERE tm.Id={0} and {1}", id, where, deptID);
                return conn.Query<Dep_MonthPlan>(sql).ToList().FirstOrDefault();
            }
        }
        #endregion
    }
}
