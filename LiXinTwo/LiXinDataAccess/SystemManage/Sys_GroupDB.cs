using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.SystemManage;
using LiXinModels.User;

namespace LiXinDataAccess.SystemManage
{
    public class Sys_GroupDB: BaseRepository
    {
        /// <summary>
        /// 判断 群组信息 是否存在
        /// </summary>
        /// <param name="groupName">群组名</param>
        /// <param name="groupId">群组ID</param>
        /// <returns></returns>
        public bool Exists(string groupName, int groupId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select count(1) from Sys_Group WHERE IsDelete = 0 and GroupName = @GroupName  ";
                if (groupId > 0)
                    sqlwhere += " and GroupId <> @GroupId";
                var param = new { GroupName = groupName, GroupId = groupId };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count < 1;
            }
        }
        
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        public void AddGroup(Sys_Group model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO Sys_Group(GroupName,UserId,Memo,CreateTime,LastUpdateTime,IsDelete) VALUES (@GroupName,@UserId,@Memo,@CreateTime,@LastUpdateTime,@IsDelete)SELECT @@IDENTITY AS ID";
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
                model.GroupId = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateGroup(Sys_Group model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Sys_Group SET GroupName = @GroupName,UserId = @UserId,Memo=@Memo,CreateTime = @CreateTime,LastUpdateTime=@LastUpdateTime,IsDelete = @IsDelete WHERE GroupId=@GroupId";
                var param = new
                {
                    model.GroupName,
                    model.UserId,
                    model.Memo,
                    model.CreateTime,
                    model.LastUpdateTime,
                    model.IsDelete,
                    model.GroupId
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
        public bool DeleteGroup(string groupId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"update Sys_Group set IsDelete = 1 where GroupId in(" + groupId + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        public Sys_Group GetGroup(int groupId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from Sys_Group where GroupId=@GroupId";
                return conn.Query<Sys_Group>(sqlstr, new { GroupId=groupId }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Sys_Group> GetGroupList(string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select * from Sys_Group where " + where;
                return conn.Query<Sys_Group>(sql).ToList();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Sys_Group> GetAllGroupList(int startIndex = 1, int startLength = int.MaxValue, string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT count(*)OVER(PARTITION BY null) AS totalcount, row_number() OVER(ORDER BY t0.GroupId) AS rowNum,t0.*,t1.Realname,(select count(*) from Sys_GroupUser where Sys_GroupUser.GroupId=t0.GroupId) AS UNumber FROM Sys_Group AS t0,Sys_User AS t1 WHERE t0.UserId=t1.UserId AND t0.IsDelete=0 and " + where + ") as result WHERE result.rowNum BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_Group>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 根据组把组成员都列出来
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<Sys_User> GetGroupUserList(string groupids,string ids="0")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string query = string.Format(@" select UserId,Realname from sys_User where userid in(
  select Sys_GroupUser.UserId from Sys_Group left join 
  Sys_GroupUser on Sys_Group.GroupId=Sys_GroupUser.GroupId 
	where dbo.Sys_Group.GroupId in ({0})) and UserId not in({1})", groupids, ids);

                return conn.Query<Sys_User>(query).ToList();
            }
        }
    }
}
