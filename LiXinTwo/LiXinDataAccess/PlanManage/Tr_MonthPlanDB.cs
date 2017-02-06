using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.PlanManage;
using LiXinModels.CourseManage;

namespace LiXinDataAccess
{
    public class Tr_MonthPlanDB : BaseRepository
    {
        #region 月度计划首页

        /// <summary>
        /// 是否存在此月度计划，
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>true存在，false不存在</returns>
        public bool IsExistMonplan(int year, string month, int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT COUNT(*) as count FROM Tr_MonthPlan
WHERE Year={0} AND Month='{1}' and IsDelete=0 AND Id<>{2}", year, month, id);
                dynamic list = conn.Query<dynamic>(sql).FirstOrDefault();
                return decimal.ToInt32(list.count) > 0;
            }
        }

        /// <summary>
        /// 获取计划的基本信息
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Tr_MonthPlan> GetMonthPlan(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  month asc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT row_number()OVER(ORDER BY {1}) number,count(*)OVER(PARTITION BY null) totalcount,* FROM (
SELECT Id,Year,Month,LastUpdateTime,tm.UserId,PublishFlag,tm.IsDelete,su.Realname,
courseCount=(SELECT count(1) FROM Tr_MonthPlanCourse
WHERE MonthId=Id)
FROM dbo.Tr_MonthPlan tm
INNER JOIN Sys_User su ON tm.UserId=su.UserId
where tm.IsDelete=0 AND {0}) a) result
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where, jsRenderSortField);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Tr_MonthPlan>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        public void UpdateMonthPlan(int id, string where)
        {
            using (IDbConnection conn = OpenConnection())
            {
               string sql = string.Format(@"UPDATE Tr_MonthPlan
                            SET {1}
                            WHERE Id={0}", id, where);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 获取年份
        /// </summary>
        /// <returns></returns>
        public List<int> GetYearOfPlan()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sql = @"SELECT Year FROM Tr_YearPlan
WHERE PublishFlag=1";
                return conn.Query<int>(sql).ToList();
            }
        }


        /// <summary>
        /// 专为框架内外准备的数据
        /// </summary>
        /// <returns></returns>
        public List<Tr_YearPlanCourse> GetYearCourse(int year, string courseID)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT   * FROM Tr_YearPlanCourse
WHERE CourseId IN ({0}) AND 
YearId IN (SELECT Id FROM Tr_YearPlan WHERE  Year={1} AND IsDelete=0)", courseID, year);
                return conn.Query<Tr_YearPlanCourse>(sql).ToList();
            }
        }
        #endregion

        #region 年度 月度 课程查询

        /// <summary>
        /// 获取年度计划内的课程详情
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<CourseShow> GetYearCourseDetails(int startIndex = 0, int startLength = int.MaxValue, string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"
SELECT * FROM (
SELECT row_number()OVER(ORDER BY ty.year asc) number,count(*)OVER(PARTITION BY null) totalcount, cc.Code,cc.Id,CourseName,cc.Year,cc.Month,cc.Day,OpenLevel,Way,Teacher,IsMust,tp.IsSystem,su.UserId,su.Realname,su.PayGrade,dbo.GetSystemTree(tp.IsSystem)SystemTree 
,cc.IsCPA,cc.CourseLength
FROM Tr_YearPlan ty left JOIN Tr_YearPlanCourse tp  ON tp.YearId=ty.Id
INNER JOIN Co_Course cc ON tp.CourseId=cc.Id
left JOIN Sys_User su ON su.UserId=cc.Teacher
WHERE  cc.IsDelete=0  AND ty.PublishFlag=1 and {0}) result
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<CourseShow>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取月度计划内的课程详情
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<CourseShow> GetMonthCourseDetails(int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " cc.month desc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT * FROM (
SELECT row_number()OVER(ORDER BY {1}) number,count(*)OVER(PARTITION BY null) totalcount, cc.Id,CourseName,cc.PreCourseTime,SurveyPaperId,cc.month,day,OpenLevel,Way,Teacher,IsMust,tp.IsSystem,su.Realname,su.PayGrade,dbo.GetSystemTree(tp.IsSystem) SystemTree 
,cc.IsCPA,cc.CourseLength,scr.RoomName
FROM Tr_MonthPlan ty left JOIN Tr_MonthPlanCourse tp  ON tp.MonthId=ty.Id
INNER JOIN Co_Course cc ON tp.CourseId=cc.Id
left JOIN Sys_User su ON su.UserId=cc.Teacher
LEFT JOIN Sys_ClassRoom scr ON scr.Id=cc.RoomId
WHERE cc.IsDelete=0  AND ty.IsDelete=0 and {0}) result
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", where, jsRenderSortField);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<CourseShow>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 获得课程
        /// </summary>
        /// <param name="CourseID"></param>
        /// <returns></returns>
        public List<CourseShow> GetSingleCourse(int CourseID)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT  cc.Id,CourseName,cc.Year,cc.Month,cc.Day,OpenLevel,Way,Teacher,IsMust,su.Realname,su.PayGrade
FROM  Co_Course cc 
INNER JOIN Sys_User su ON su.UserId=cc.Teacher
WHERE  cc.IsDelete=0 AND su.IsDelete=0  AND cc.Id=@CourseID");
                var param = new
                {
                    CourseID = CourseID
                };
                return conn.Query<CourseShow>(sql, param).ToList();
            }
        }
        #endregion

        #region  新增、更新 月度计划，计划与课程的关联

        public Tr_MonthPlan Get(int id)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM Tr_MonthPlan
WHERE Id={0}", id);
                return conn.Query<Tr_MonthPlan>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 插入月度数据库
        /// </summary>
        /// <param name="model"></param>
        public void InsertTr_MonthPlan(Tr_MonthPlan model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sql = @"INSERT INTO dbo.Tr_MonthPlan
	                            (
	                            Year,
	                            Month,
	                            LastUpdateTime,
	                            UserId,
	                            PublishFlag,
	                            IsDelete
	                            )
                            VALUES 
	                            (
	                            @year,
	                            @month,
	                            getdate(),
	                            @userid,
	                            0,
	                            0
	                            );SELECT @@IDENTITY as ID ";
                var param = new
               {
                   year = model.Year,
                   month = model.Month,
                   userid = model.UserId
               };
                dynamic list = conn.Query<dynamic>(sql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.ID);
            }
        }

        /// <summary>
        /// 插入月度计划和课程的关联
        /// </summary>
        /// <param name="model"></param>
        public void InsertTr_MonthPlanCourse(Tr_MonthPlanCourse model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sql = @"INSERT INTO dbo.Tr_MonthPlanCourse
	                                (
	                                MonthId,
	                                CourseId,
	                                IsSystem
	                                )
                                VALUES 
	                                (
	                                @monthid,
	                                @courseid,
	                                @issystem
	                                );SELECT @@IDENTITY as ID ";
                var param = new
                {
                    monthid = model.MonthId,
                    courseid = model.CourseId,
                    issystem = model.IsSystem
                };
                conn.Execute(sql, param);
            }
        }

        /// <summary>
        /// 插入课程
        /// </summary>
        /// <param name="model"></param>
        public void InsertCo_Course(Co_Course model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sql = @"INSERT INTO dbo.Co_Course
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
	                                Way,
	                                Teacher,
	                                Publishflag,
	                                LastUpdateTime,
                                    SurveyPaperId,
                                    CourseFrom,
	                                IsDelete,
                                    RoomId,
                                    IsSystem,
                                    IsYearPlan
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
	                                @way,
	                                @teacher,
	                                @publishflag,
	                                getdate(),
                                    @SurveyPaperId,
                                    @courseFrom,
	                                0,
                                    @roomId,
                                    @isSystem,
                                    @isYearPlan
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
                  way = model.Way,
                  teacher = model.Teacher,
                  SurveyPaperId = model.SurveyPaperId,
                  courseFrom = model.CourseFrom,
                  publishflag = model.Publishflag,
                  roomId = model.RoomId,
                  isSystem=model.IsSystem,
                  isYearPlan=model.IsYearPlan
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
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"
DELETE Tr_MonthPlanCourse
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
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"UPDATE Co_Course
SET IsDelete=1
WHERE Id in ({0})", courseIDs);
                conn.Execute(sql);
            }

        }

        public void publish(int id)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"UPDATE Tr_MonthPlan
SET PublishFlag=1
WHERE Id={0}

UPDATE Co_Course
SET Publishflag=1
WHERE Id IN (SELECT CourseId FROM Tr_MonthPlanCourse WHERE MonthId={0})", id);
                conn.Execute(sql);
            }
        }
        #endregion

        #region 月度差异对比

        /// <summary>
        /// 差异对比的头部信息 年度预计课程数，实际课程数等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Tr_MonthPlan> GetPlanCourseCount(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT *,
yearCourseCount=(SELECT count(*) FROM Tr_YearPlanCourse WHERE YearId IN (SELECT id FROM Tr_YearPlan WHERE Year=tm.Year) ),
monthCourseCount=(SELECT count(*) FROM Tr_MonthPlanCourse WHERE MonthId={0})
 FROM Tr_MonthPlan tm
 WHERE tm.Id={0}
", id);
                return conn.Query<Tr_MonthPlan>(sql).ToList();
            }
        }
        #endregion

        #region 我的培训计划 月度
        /// <summary>
        /// 我的培训计划 月度
        /// </summary>
        /// <returns></returns>
        public List<CourseShow> GetMyMonthPlan(int UserID, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " PreCourseTime desc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"DECLARE @trainGrade NVARCHAR(50)
SELECT @trainGrade=TrainGrade FROM Sys_User WHERE UserId={1}
SELECT * FROM(
SELECT row_number()OVER(ORDER BY {2}) number,count(*)OVER(PARTITION BY null) totalcount,sum(CourseLength)OVER(PARTITION BY NULL) sumLength, * from
(SELECT cc.Id,CourseName,cc.PreCourseTime,cc.month,day,OpenLevel,Way,Teacher,IsMust,IsCPA,tp.IsSystem,su.Realname,su.PayGrade,dbo.GetSystemTree(tp.IsSystem) SystemTree 
,ty.Year,cc.CourseLength,scr.RoomName

FROM Tr_MonthPlan ty left JOIN Tr_MonthPlanCourse tp  ON tp.MonthId=ty.Id
INNER JOIN Co_Course cc ON tp.CourseId=cc.Id
left JOIN Sys_User su ON su.UserId=cc.Teacher
LEFT JOIN Sys_ClassRoom scr ON scr.Id=cc.RoomId
WHERE cc.IsDelete=0 AND (su.IsDelete=0  OR su.IsDelete IS NULL)  AND ty.IsDelete=0
AND charindex(@trainGrade,cc.OpenLevel)>0 AND ty.PublishFlag=1
) a where {0}
) result
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where, UserID,jsRenderSortField);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<CourseShow>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 我的培训计划 年度
        /// </summary>
        /// <returns></returns>
        public List<CourseShow> GetMyYearPlan(int UserID, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Month desc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"DECLARE @trainGrade NVARCHAR(50)
SELECT @trainGrade=TrainGrade FROM Sys_User WHERE UserId={1}
SELECT * FROM(
SELECT row_number()OVER(ORDER BY {2}) number,count(*)OVER(PARTITION BY null) totalcount,sum(CourseLength)OVER(PARTITION BY NULL) sumLength, * from
(SELECT cc.Id,CourseName,cc.Year,cc.Month,cc.Day,OpenLevel,Way,Teacher,IsMust,IsCPA,tp.IsSystem,su.UserId,su.Realname,su.PayGrade,dbo.GetSystemTree(tp.IsSystem)SystemTree 
,ty.year tyyear,cc.CourseLength

FROM Tr_YearPlan ty left JOIN Tr_YearPlanCourse tp  ON tp.YearId=ty.Id
INNER JOIN Co_Course cc ON tp.CourseId=cc.Id
left JOIN Sys_User su ON su.UserId=cc.Teacher
WHERE  cc.IsDelete=0   AND ty.PublishFlag=1
AND charindex(@trainGrade,cc.OpenLevel)>0
)  a WHERE {0}
)result
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where, UserID, jsRenderSortField);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<CourseShow>(sql, param).ToList();
            }
        }
        #endregion

    }
}
