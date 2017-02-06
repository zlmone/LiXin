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
    public class Free_UserApplyFilesDB : BaseRepository
    {
        public bool AddFree_UserApplyFiles(Free_UserApplyFiles model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO Free_UserApplyFiles(userApplyID,ResourceName,RealName,IsDelete,ConvertName,type) 
VALUES (@userApplyID,@ResourceName,@RealName,@IsDelete,@ConvertName,@type)SELECT @@IDENTITY AS ID";
                var param = new
                {
                    //model.ID,
                    model.userApplyID,
                    model.ResourceName,
                    model.RealName,
                    model.IsDelete,
                    model.ConvertName,
                    model.type
                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.ID = decimal.ToInt32(id);

                return model.ID > 0 ? true : false;

            }
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateFree_UserApplyFiles(Free_UserApplyFiles model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Free_UserApplyFiles SET userApplyID = @userApplyID,ResourceName = @ResourceName,RealName=@RealName,IsDelete=@IsDelete WHERE Id=@ID";
                var param = new
                {
                    model.ID,
                    model.userApplyID,
                    model.ResourceName,
                    model.RealName,
                    model.IsDelete
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
        public bool DeleteFree_UserApplyFiles(string id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"update Free_UserApplyFiles set IsDelete = 1 where id in(" + id + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Free_UserApplyFiles> GetFree_UserApplyFilesList(string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select * from Free_UserApplyFiles where IsDelete = 0 and " + where;
                return conn.Query<Free_UserApplyFiles>(sql).ToList();
            }
        }



    }
}
