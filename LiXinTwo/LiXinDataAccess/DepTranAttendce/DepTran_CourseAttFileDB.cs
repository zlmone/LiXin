using System.Collections.Generic;
using System.Linq;
using Retech.Core;
using Retech.Data;
using System.Data;
using LiXinModels.DepTranAttendce;

namespace LiXinDataAccess.DepTranAttendce
{
    public class DepTran_CourseAttFileDB : BaseRepository
    {
        #region 增改(基本信息)
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertDepTran_CourseAttFile(DepTran_CourseAttFile model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO DepTran_CourseAttFile(CourseId,DepartId,Type,Name,RealName,CreateTime,LoadTimes,FileSize,IsDelete) VALUES (@CourseId,@DepartId,@Type,@Name,@RealName,@CreateTime,@LoadTimes,@FileSize,@IsDelete);SELECT @@IDENTITY as Id ";

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
        public bool UpdateDepTran_CourseAttFile(DepTran_CourseAttFile model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update DepTran_CourseAttFile set CourseId = @CourseId,DepartId = @DepartId,Type = @Type,Name = @Name,RealName = @RealName,CreateTime = @CreateTime,LoadTimes = @LoadTimes,FileSize = @FileSize,IsDelete = @IsDelete where Id=@Id";

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
        public DepTran_CourseAttFile GetDepTran_CourseAttFile(int courseID,int departID,string where = " 1=1")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from DepTran_CourseAttFile where CourseId=@CourseId and DepartId=@DepartId and " + where);
                var param = new { CourseId = courseID, DepartId = departID };
                return connection.Query<DepTran_CourseAttFile>(query, param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<DepTran_CourseAttFile> GetDepTran_CourseAttFileDownload(int courseID, int departID, string where = " 1=1")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from DepTran_CourseAttFile where CourseId=@CourseId and DepartId=@DepartId and " + where);
                var param = new { CourseId = courseID, DepartId = departID };
                return connection.Query<DepTran_CourseAttFile>(query, param).ToList();
            }
        }
        #endregion
    }
}
