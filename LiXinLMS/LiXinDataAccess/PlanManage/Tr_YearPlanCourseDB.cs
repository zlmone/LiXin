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

namespace LiXinDataAccess.PlanManage
{
    public class Tr_YearPlanCourseDB : BaseRepository
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        public bool AddYearCourse(Tr_YearPlanCourse model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO Tr_YearPlanCourse (YearId,CourseId,IsSystem) VALUES (@YearId,@CourseId,@IsSystem)";
                var param = new
                {
                    YearId=model.YearId,
                    CourseId=model.CourseId,
                    IsSystem=model.IsSystem
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="YearId">年计划ID</param>
        /// <param name="CourseIds">课程ID集合</param>
        /// <returns>操作状态</returns>
        public bool DeleteYearCourse(int YearId, string CourseIds)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"DELETE FROM Tr_YearPlanCourse where YearId=@YearId and CourseId in (" + CourseIds + ")";
                var param = new { YearId = YearId };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="YearId">年计划ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteAllYearCourse(string YearIds)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"DELETE FROM Tr_YearPlanCourse where YearId in (" + YearIds + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        /// 获得数据列表(无分页)
        /// </summary>
        public List<Tr_YearPlanCourse> GetYearCourseList(int YearId, string Order="asc",string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                //string sql = @"SELECT row_number()OVER(ORDER BY Month " + Order + ",day " + Order + ") number,cc.Id as CourseId,CourseName,Code,Year,Month,Day,OpenLevel,Way,Teacher,IsMust,IsSystem,su.Realname,su.PayGrade,su.IsDelete,dbo.GetSystemTree(tp.IsSystem) SystemTree FROM Tr_YearPlanCourse tp LEFT JOIN Co_Course cc ON tp.CourseId=cc.Id LEFT JOIN Sys_User su ON su.UserId=cc.Teacher LEFT JOIN Co_SystemLinkCourse csc ON csc.CourseId=cc.Id WHERE  cc.IsDelete=0 AND tp.YearId=@YearId and " + where;
                string sql = @"SELECT row_number()OVER(ORDER BY Month " + Order + ",day " + Order + ") number,cc.Id as CourseId,CourseName,Code,Year,Month,Day,OpenLevel,Way,Teacher,IsCPA,IsMust,cc.IsSystem,CourseLength,su.Realname,su.PayGrade,su.IsDelete FROM Tr_YearPlanCourse tp LEFT JOIN Co_Course cc ON tp.CourseId=cc.Id LEFT JOIN Sys_User su ON su.UserId=cc.Teacher LEFT JOIN Co_SystemLinkCourse csc ON csc.CourseId=cc.Id WHERE  cc.IsDelete=0 AND tp.YearId=@YearId and " + where;
                var param = new
                {
                    YearId = YearId
                };
                return conn.Query<Tr_YearPlanCourse>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得数据列表(有分页)
        /// </summary>
        public List<Tr_YearPlanCourse> GetYearCourseList(int YearId,int startIndex = 1, int startLength = int.MaxValue,string Order="asc",string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                //string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY Month " + Order + ",Day " + Order + ") number,count(*)OVER(PARTITION BY null) totalcount, cc.Id,CourseName,Code,Year,Month,Day,OpenLevel,Way,Teacher,IsMust,IsSystem,su.Realname,su.PayGrade,su.IsDelete,dbo.GetSystemTree(csc.SystemId) SystemTree FROM Tr_YearPlanCourse tp LEFT JOIN Co_Course cc ON tp.CourseId=cc.Id LEFT JOIN Sys_User su ON su.UserId=cc.Teacher LEFT JOIN Co_SystemLinkCourse csc ON csc.CourseId=cc.Id WHERE  cc.IsDelete=0 AND tp.YearId=@YearId and " + where + ") result WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY Month " + Order + ",Day " + Order + ") number,count(*)OVER(PARTITION BY null) totalcount, cc.Id,CourseName,Code,Year,Month,Day,OpenLevel,Way,Teacher,IsCPA,IsMust,cc.IsSystem,CourseLength,su.Realname,su.PayGrade,su.IsDelete FROM Tr_YearPlanCourse tp LEFT JOIN Co_Course cc ON tp.CourseId=cc.Id LEFT JOIN Sys_User su ON su.UserId=cc.Teacher LEFT JOIN Co_SystemLinkCourse csc ON csc.CourseId=cc.Id WHERE  cc.IsDelete=0 AND tp.YearId=@YearId and " + where + ") result WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    YearId = YearId
                };
                return conn.Query<Tr_YearPlanCourse>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得所有课程ID
        /// </summary>
        /// <param name="YearId">年计划ID</param>
        /// <returns>操作状态</returns>
        public List<int> GetCourseID(int YearId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT CourseId FROM dbo.Tr_YearPlanCourse WHERE YearId=@YearId";
                return conn.Query<int>(sql, new { YearId = YearId }).ToList();
            }
        }

        /// <summary>
        /// 批量查询课程
        /// </summary>
        /// <param name="ids">课程ID</param>
        /// <returns>操作状态</returns>
        public List<Tr_YearPlanCourse> GetCourseList(string ids)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = @"SELECT cc.Id,CourseName,Code,Month,Day,OpenLevel,Way,Teacher,IsCPA,IsMust,CourseLength,su.Realname,su.PayGrade,dbo.GetSystemTree(csc.SystemId) SystemTree FROM Co_Course cc INNER JOIN  Sys_User su ON su.UserId=cc.Teacher LEFT JOIN Co_SystemLinkCourse csc ON csc.CourseId=cc.Id WHERE  cc.IsDelete=0 AND cc.Id IN(" + ids + ")";
                return connection.Query<Tr_YearPlanCourse>(sql).ToList();
            }
        }

        public Tr_YearPlanCourse GetCo_Course(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = "select cc.Id,cc.Code,cc.CourseName,cc.Year,cc.Month,cc.Day,cc.OpenLevel,cc.Way,cc.Teacher,cc.IsCPA,cc.IsMust,cc.CourseLength,su.Realname,su.PayGrade from Co_Course as cc LEFT JOIN Sys_User as su ON cc.Teacher=su.UserId where cc.Id=@Id ";
                var param = new
                {
                    Id
                };
                return connection.Query<Tr_YearPlanCourse>(sqlwhere, param).FirstOrDefault();

            }
        }

        /// <summary>
        /// 删除关联表and课程表里面的课程
        /// </summary>
        /// <param name="YearId">年计划ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteCoursByYear(int YearId,string delid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"UPDATE dbo.Co_Course SET IsDelete=1 WHERE Id IN (" + delid + ") DELETE FROM dbo.Tr_YearPlanCourse WHERE YearId=@YearId AND CourseId IN (" + delid + ")";
                var param = new
                {
                    YearId = YearId
                };
                int result = conn.Execute(sqlwhere,param);
                return result > 0;
            }
        }

        /// <summary>
        /// 判断年度计划下面人员是否删除
        /// </summary>
        /// <param name="Yearid"></param>
        /// <returns></returns>
        public int UserIsdel(int Yearid)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = @"SELECT count(1) FROM Tr_YearPlanCourse AS ty LEFT JOIN Co_Course AS cc ON cc.Id=ty.CourseId LEFT JOIN Sys_User AS su ON su.UserId=cc.Teacher WHERE cc.Teacher!='' AND ((su.IsDelete=0 AND su.IsTeacher=0) OR su.IsDelete=1) and ty.YearId=" + Yearid;
                int count = connection.Query<int>(sql).FirstOrDefault();
                return count;
            }
        }

        /// <summary>
        /// 插入课程
        /// </summary>
        /// <param name="model"></param>
        public bool UpdateCo_Course(Co_Course model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sql = @"Update dbo.Co_Course SET Code=@Code,CourseName=@CourseName,Year=@Year,Month=@Month,YearPlan=@YearPlan,PreCourseTime=@PreCourseTime,Day=@Day,OpenLevel=@OpenLevel,IsCPA=@IsCPA,IsMust=@IsMust,IsSystem=@IsSystem,IsYearPlan=@IsYearPlan,CourseLength=@CourseLength,Way=@Way,Teacher=@Teacher,Publishflag=@Publishflag,LastUpdateTime=@LastUpdateTime,SurveyPaperId=@SurveyPaperId,CourseFrom=@CourseFrom,IsDelete=@IsDelete WHERE Id=@Id";
                var param = new
                {
                    Year = model.Year,
                    Month = model.Month,
                    YearPlan = model.Year,
                    Code = model.Code,
                    CourseName = model.CourseName,
                    PreCourseTime = model.PreCourseTime,
                    Day = model.Day,
                    OpenLevel = model.OpenLevel,
                    IsCPA = model.IsCPA,
                    IsMust = model.IsMust,
                    CourseLength=model.CourseLength,
                    Way = model.Way,
                    Teacher = model.Teacher,
                    SurveyPaperId = model.SurveyPaperId,
                    CourseFrom = model.CourseFrom,
                    Publishflag=0,
                    LastUpdateTime=model.LastUpdateTime,
                    IsDelete=0,
                    IsSystem=model.IsSystem,
                    IsYearPlan=model.IsYearPlan,
                    Id = model.Id
                };
                int result = conn.Execute(sql, param);
                return result > 0;
            }
        }
    }
}
