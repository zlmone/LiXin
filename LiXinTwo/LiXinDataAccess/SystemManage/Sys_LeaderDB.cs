using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.SystemManage;


namespace LiXinDataAccess.SystemManage
{
    public class Sys_LeaderDB : BaseRepository
    {
        /// <summary>
        /// 判断 群组信息 是否存在
        /// </summary>
        /// <param name="groupName">群组名</param>
        /// <param name="id">群组ID</param>
        /// <returns></returns>
        public bool Exists(string groupName, int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select count(1) from Sys_LeaderConfig WHERE IsDelete = 0 and GroupName = @GroupName  ";
                if (id > 0)
                    sqlwhere += " and Id <> @Id";
                var param = new { GroupName = groupName, id = id };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count < 1;
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        public void AddGroup(Sys_LeaderConfig model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO Sys_LeaderConfig(GroupName,UserId,Memo,CreateTime,LastUpdateTime,IsDelete) VALUES (@GroupName,@UserId,@Memo,@CreateTime,@LastUpdateTime,@IsDelete)SELECT @@IDENTITY AS ID";
                var param = new
                {
                    model.GroupName,
                    model.UserId,
                    model.Memo,
                    model.CreateTime,
                    model.LastUpdateTime,
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
        public bool UpdateGroup(Sys_LeaderConfig model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Sys_LeaderConfig SET GroupName = @GroupName,UserId = @UserId,Memo=@Memo,CreateTime = @CreateTime,LastUpdateTime=@LastUpdateTime,IsDelete = @IsDelete WHERE id=@id";
                var param = new
                {
                    model.GroupName,
                    model.UserId,
                    model.Memo,
                    model.CreateTime,
                    model.LastUpdateTime,
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
        /// <param name="ids">群组ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteGroup(string id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"update Sys_LeaderConfig set IsDelete = 1 where id in(" + id + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        public Sys_LeaderConfig GetGroup(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = @"SELECT sl.*,su.Realname FROM Sys_LeaderConfig sl,Sys_User su
                                  WHERE sl.UserId=su.UserId and id=@id";
                return conn.Query<Sys_LeaderConfig>(sqlstr, new { id = id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Sys_LeaderConfig> GetGroupList(string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select * from Sys_LeaderConfig where " + where;
                return conn.Query<Sys_LeaderConfig>(sql).ToList();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Sys_LeaderConfig> GetAllGroupList(int startIndex = 1, int startLength = int.MaxValue, string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT count(*)OVER(PARTITION BY null) AS totalcount, row_number() OVER(ORDER BY t0.Id) AS rowNum,t0.*,t1.Realname,(select count(*) from Sys_UserLinkLeader where Sys_UserLinkLeader.GroupId=t0.Id) AS UNumber FROM Sys_LeaderConfig AS t0,Sys_User AS t1 WHERE t0.UserId=t1.UserId AND t0.IsDelete=0 and " + where + ") as result WHERE result.rowNum BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_LeaderConfig>(sql, param).ToList();
            }
        }


        
    }
}
