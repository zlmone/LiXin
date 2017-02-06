using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LiXinModels;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.ClassRoom
{
    public class ClassRoomResourceDB : BaseRepository
    {
        /// <summary>
        /// 获取教室资源列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<Sys_ClassRoomResource> GetRoomResourceList(string where = " IsDelete=0 ", string orderBy = " order by CreateDate desc ")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select Sys_ClassRoomResource.* from Sys_ClassRoomResource {0} {1} ", where, orderBy);
                return connection.Query<Sys_ClassRoomResource>(query).ToList();
            }
        }

        /// <summary>
        /// 新增教室资源
        /// </summary>
        /// <param name="model">教室资源实体</param>
        public bool AddRoomResource(Sys_ClassRoomResource model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Sys_ClassRoomResource (ClassRoomID,RealName,Name,CreateDate,IsDelete) 
                      values (@ClassRoomID,@RealName,@Name,@CreateDate,@IsDelete) 
                      SELECT @@Identity as Id";
                var param = new
                {
                    model.ClassRoomID,
                    model.RealName,
                    model.Name,
                    model.CreateDate,
                    model.IsDelete
                };
                var id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
                return model.Id > 0;
            }
        }

        /// <summary>
        /// 删除指定教室下资源信息
        /// </summary>
        /// <param name="id">教室ID</param>
        /// <returns></returns>
        public bool DeleteRoomResourcesByRoomID(int id)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Sys_ClassRoomResource set IsDelete=1 where ClassRoomID=@id";
                var param = new
                {
                    id
                };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据教室资源关联表主键删除资源
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        public bool DeleteRoomResourcesByID(int id)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Sys_ClassRoomResource set IsDelete=1 where Id=@id";
                var param = new
                {
                    id
                };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }
    }
}
