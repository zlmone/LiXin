using System.Collections.Generic;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.DepAttendce;

namespace LiXinDataAccess.DepAttendce
{
    public class Dep_CourseAttFileDB : BaseRepository
    {
        #region 增改(基本信息)
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertDep_CourseAttFile(Dep_CourseAttFile model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Dep_CourseAttFile(CourseId,DepartId,Type,Name,RealName,CreateTime,LoadTimes,FileSize,IsDelete) VALUES (@CourseId,@DepartId,@Type,@Name,@RealName,@CreateTime,@LoadTimes,@FileSize,@IsDelete);SELECT @@IDENTITY as Id ";

                var param = new
                {
                    model.CourseId,
                    model.DepartId,
                    model.Type,
                    model.Name,
                    model.RealName,
                    model.CreateTime,
                    model.LoadTimes,
                    model.FileSize,
                    model.IsDelete
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.Id);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateDep_CourseAttFile(Dep_CourseAttFile model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update Dep_CourseAttFile set CourseId = @CourseId,DepartId = @DepartId,Type = @Type,Name = @Name,RealName = @RealName,CreateTime = @CreateTime,LoadTimes = @LoadTimes,FileSize = @FileSize,IsDelete = @IsDelete where Id=@Id";

                var param = new
                {
                    model.CourseId,
                    model.DepartId,
                    model.Type,
                    model.Name,
                    model.RealName,
                    model.CreateTime,
                    model.LoadTimes,
                    model.FileSize,
                    model.IsDelete,
                    model.Id
                };
                int result = conn.Execute(strSql, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public Dep_CourseAttFile GetDep_CourseAttFile(int courseID, int departID, string where = " 1=1")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from Dep_CourseAttFile where CourseId=@CourseId and DepartId=@DepartId and " + where);
                var param = new { CourseId = courseID, DepartId = departID };
                return connection.Query<Dep_CourseAttFile>(query, param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<Dep_CourseAttFile> GetDep_CourseAttFileDownload(int courseID, int departID, string where = " 1=1")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from Dep_CourseAttFile where CourseId=@CourseId and DepartId=@DepartId and " + where);
                var param = new { CourseId = courseID, DepartId = departID };
                return connection.Query<Dep_CourseAttFile>(query, param).ToList();
            }
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="id"></param>
        public void DeleteFile(int id)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"DELETE FROM dbo.Dep_CourseAttFile
                           WHERE Id = {0}", id);
                conn.Execute(sql);
            }
        }
        #endregion
    }
}
