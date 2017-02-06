using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.CourseManage;
namespace LiXinDataAccess
{
    public class Sys_PayGradeDB : BaseRepository
    {
        /// <summary>
        /// 添加薪酬级别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddSys_PayGrade(Sys_PayGrade model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string strSql =
    @"insert into Sys_PayGrade(GradeName) values(@GradeName)
                SELECT @@Identity AS ID";
                var param = new
                {
                    GradeName = model.GradeName
                };
                decimal id =
                    connection.Query<decimal>(strSql, param)
                              .FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
            return model.Id;
        }

        /// <summary>
        ///     获得 薪酬级别数据列表
        /// </summary>
        public List<Sys_PayGrade> GetSys_PayGradeList(string strWhere = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                if (strWhere == "")
                {
                    strWhere = "1=1";
                }
                string selectSql = "select * from Sys_PayGrade where " + strWhere;
                return connection.Query<Sys_PayGrade>(selectSql).ToList();
            }
        }

        /// <summary>
        ///  ADD薪酬级别关联体系 并关联课程
        /// </summary>
        /// <returns></returns>
        public int AddSys_SortLinkGrade(Sys_SortLinkGrade model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string strSql =
    @"insert into Sys_SortLinkGrade(GradeID,SortID,CourseId) values(@GradeID,@SortID,@CourseId)
                SELECT @@Identity AS ID";
                var param = new
                {
                    GradeID = model.GradeID,
                    SortID = model.SortID,
                    CourseId = model.CourseId
                };
                decimal id =
                    connection.Query<decimal>(strSql, param)
                              .FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
            return model.Id;
        }

        /// <summary>
        ///     获得薪酬级别 关联课程体系 并关联课程 数据列表
        /// </summary>
        public List<Sys_SortLinkGrade> GetSys_SortLinkGradeList(string strWhere = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                if (strWhere == "")
                {
                    strWhere = "1=1";
                }
                string selectSql = @"select * from Sys_SortLinkGrade  Left join Sys_SortGradeCourse
                on  Sys_SortGradeCourse.Id=Sys_SortLinkGrade.CourseId AND Sys_SortGradeCourse.IsDelete=0 where " + strWhere;
                return connection.Query<Sys_SortLinkGrade>(selectSql).ToList();
            }
        }

        /// <summary>
        /// 判断体系下是否有课程
        /// </summary>
        /// <param name="sortId"></param>
        /// <returns></returns>
        public int CourseSystemContainCourseNum(int sortId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string selectSql = string.Format(@"SELECT COUNT(*) FROM dbo.Sys_SortGradeCourse
WHERE IsDelete=0 AND id IN (SELECT courseId FROM dbo.Sys_SortLinkGrade WHERE SortID={0})", sortId);
                return connection.Query<int>(selectSql).FirstOrDefault();
            }
        }


        /// <summary>
        /// 删除 课程体系关联 薪酬级别 +关联课程
        /// </summary>
        /// <param name="sortId"></param>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public bool DeleteSys_SortLinkGrade(int sortId, int gradeId, int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string deleteSql = "DELETE Sys_SortLinkGrade where GradeID =@GradeID AND SortID=@SortID AND CourseId=@CourseId";
                var param = new
                {
                    SortID = sortId,
                    GradeID = gradeId,
                    CourseId = courseId
                };
                return connection.Execute(deleteSql, param) > 0;
            }
        }

        /// <summary>
        /// 删除 课程体系关联 薪酬级别 +关联课程
        /// </summary>
        /// <returns></returns>
        public bool DeleteSys_SortLinkGrade(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string deleteSql = "DELETE Sys_SortLinkGrade where CourseId=@CourseId";
                var param = new
                {
                    CourseId = courseId
                };
                return connection.Execute(deleteSql, param) > 0;
            }
        }






        ////////////////////////////胜任力课程 分割线


        #region 过时的 方法
        /// <summary>
        ///  ADD  课程Id关联  薪酬级别+课程体系 关联表主键
        /// </summary>
        /// <returns></returns>
        public int AddSys_SortGradeLinkCourse(Sys_SortGradeLinkCourse model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string strSql =
    @"insert into Sys_SortGradeLinkCourse(CourseID,SortGradeID) values(@CourseID,@SortGradeID)
                SELECT @@Identity AS ID";
                var param = new
                {
                    CourseID = model.CourseID,
                    SortGradeID = model.SortGradeID
                };
                decimal id =
                    connection.Query<decimal>(strSql, param)
                              .FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
            return model.Id;
        }

        /// <summary>
        /// 删除 胜任力课程关联 薪酬级别+课程体系 关联表主键
        /// </summary>
        /// <param name="sortId"></param>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public bool DeleteSys_SortGradeLinkCourse(int courseId, int SortGradeID)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string deleteSql = "DELETE Sys_SortGradeLinkCourse where courseId =@courseId AND SortGradeID=@SortGradeID";
                var param = new
                {
                    courseId = courseId,
                    SortGradeID = SortGradeID
                };
                return connection.Execute(deleteSql, param) > 0;
            }
        }

        #endregion



        /// <summary>
        ///  ADD  胜任力课程
        /// </summary>
        /// <returns></returns>
        public int AddSys_SortGradeCourse(Sys_SortGradeCourse model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string strSql =
    @"insert into Sys_SortGradeCourse(Name,OpenLeavel,Type,IsMust,IsDelete) values(@Name,@OpenLeavel,@Type,@IsMust,0)
                SELECT @@Identity AS ID";
                var param = new
                {
                    Name = model.Name,
                    OpenLeavel = model.OpenLeavel,
                    Type = model.Type,
                    IsMust = model.IsMust
                };
                decimal id =
                    connection.Query<decimal>(strSql, param)
                              .FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
            return model.Id;
        }

        /// <summary>
        ///  Update  胜任力课程
        /// </summary>
        /// <returns></returns>
        public bool UpdateSys_SortGradeCourse(Sys_SortGradeCourse model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Sys_SortGradeCourse set Name=@Name,OpenLeavel=@OpenLeavel,
               Type=@Type,IsMust=@IsMust,IsDelete=@IsDelete where Id=@CourseId";

                var param = new
                {
                    CourseId = model.Id,
                    Name = model.Name,
                    OpenLeavel = model.OpenLeavel,
                    Type = model.Type,
                    IsMust = model.IsMust,
                    IsDelete = model.IsDelete
                };
                return conn.Execute(sqlwhere, param) > 0;
            }
        }

        /// <summary>
        ///  Delete  胜任力课程
        /// </summary>
        /// <returns></returns>
        public bool DeleteSys_SortGradeCourse(int Id)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Sys_SortGradeCourse set IsDelete=1 where Id=@Id";

                var param = new
                {
                    Id
                };
                return conn.Execute(sqlwhere, param) > 0;
            }
        }

        /// <summary>
        ///     获得胜任力课程 数据列表
        /// </summary>
        public List<Sys_SortGradeCourse> GetSys_SortGradeCourseList(string strWhere = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                if (strWhere == "")
                {
                    strWhere = "1=1";
                }
                string selectSql = "select * from Sys_SortGradeCourse where " + strWhere + " AND IsDelete=0 ";
                return connection.Query<Sys_SortGradeCourse>(selectSql).ToList();
            }
        }

        /// <summary>
        /// 根据人员薪酬找到薪酬级别ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetPayGradeByUserId(int userid)
        {
            using (IDbConnection connection = OpenConnection())
            {
                
                string selectSql = "   select Sys_PayGrade.Id from [Sys_User] left join Sys_PayGrade on  [Sys_User].[PayGrade]=Sys_PayGrade.GradeName where [Sys_User].UserId='"+userid+"'";
                return connection.Query<int>(selectSql).FirstOrDefault();
            }
        }
        
    }
}
