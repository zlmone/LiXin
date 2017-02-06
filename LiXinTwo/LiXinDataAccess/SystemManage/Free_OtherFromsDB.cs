using LiXinModels.SystemManage;
using Retech.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Retech.Data;

namespace LiXinDataAccess.SystemManage
{
    public class Free_OtherFromsDB : BaseRepository
    {
        public void AddOtherFroms(Free_OtherFroms model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO Free_OtherFroms(Year,FromName,FromType,FromTime,IsDelete) VALUES (@Year,@FromName,@FromType,@FromTime,@IsDelete)SELECT @@IDENTITY AS ID";
                var param = new
                {
                   model.Year,
                   model.FromName,
                   model.FromType,
                   model.FromTime,
                   model.IsDelete
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
        public bool UpdateOtherFroms(Free_OtherFroms model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Free_OtherFroms SET Year = @Year,FromName = @FromName,FromType=@FromType,FromTime = @FromTime,IsDelete=@IsDelete WHERE Id=@Id";
                var param = new
                {
                    model.Year,
                    model.FromName,
                    model.FromType,
                    model.FromTime,
                    model.IsDelete,
                    model.Id
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="groupIds">群组ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteOtherFroms(string id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"update Free_OtherFroms set IsDelete = 1 where id in(" + id + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Free_OtherFroms> GetOtherFromsList(string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select * from Free_OtherFroms where " + where;
                return conn.Query<Free_OtherFroms>(sql).ToList();
            }
        }



        /// <summary>
        /// 获得数据列表
        /// </summary>
        public Free_OtherFroms GetOtherFromsById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select * from Free_OtherFroms where  id=" + id;
                return conn.Query<Free_OtherFroms>(sql).FirstOrDefault();
            }
        }


    }
}
