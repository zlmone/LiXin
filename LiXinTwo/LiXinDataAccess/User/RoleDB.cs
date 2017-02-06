using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.User;

namespace LiXinDataAccess.User
{
    /// <summary>
    ///     角色管理数据访问类
    /// </summary>
    public class RoleDB : BaseRepository
    {
        /// <summary>
        ///     根据角色名判断角色是否存在
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool Exists(string roleName, int roleId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select count(*) from Sys_Roles WHERE RoleName=@rolename and IsDelete = 0";
                if (roleId > 0)
                    sqlwhere += " and roleId <> " + roleId;
                var param = new
                    {
                        rolename = roleName
                    };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }

        /// <summary>
        ///     增加角色
        ///     zhangjf20120820
        /// </summary>
        /// <param name="model">角色实体</param>
        /// <param name="rightIDs">权限集合，用'，'隔开</param>
        public void Add(Sys_Roles model, string rightIDs)
        {
            using (IDbConnection conn = OpenConnection())
            {
                #region //操作sql

                const string sqlwhere = @"DECLARE @roleId int;DECLARE @RightId int;--获取单个的id
                DECLARE @nextId int;--单个的id的下标
                SET @nextId=1;
                DECLARE @LengthId int;--Id的集合中共有多少个id

                BEGIN TRAN
                if(@ID=0)
                begin
                --如果设置为默认权限，遍历其他的默认开始---
                 if (@IsDefault=1)
                 begin
                 update Sys_Roles set IsDefault=0
                 end
                --如果设置为默认权限，遍历其他的默认结束---
                --插入角色开始
                 insert into Sys_Roles(RoleName,RoleDesc,IsDefault,Creater,[Status]) values(@RoleName,@RoleDesc,@IsDefault,@Creater,@Status)
                                SELECT @roleId=@@Identity
                --插入角色结束
                 --设置权限开始
                        SELECT @LengthId=dbo.GetSplitLength(@rightIDs,',');
                        while @nextId<=@LengthId
                          begin
                           --当前职位id
			                SET @RightId = left(dbo.GetSplitOfIndex(@rightIDs,',',@nextId), 100);           			  
                              if(@RightId<>'' and @RightId<>'0')
			                   begin
                            INSERT INTO [Sys_RoleRight]
                           ([RoleId],[RightId])
			                   VALUES(@roleId,@RightId)
			                   end
                            SET @nextId=@nextId+1;
                          end  
                 --设置权限结束
                end
                else
                begin
                 
                 SET @nextId=@ID
                --如果设置为默认权限，遍历其他的默认开始---
                 if (@IsDefault=1)
                 begin
                 update Sys_Roles set IsDefault=0
                 end
                --如果设置为默认权限，遍历其他的默认结束---
                --插入角色开始
                 update Sys_Roles set RoleName=@RoleName,RoleDesc=@RoleDesc,IsDefault=@IsDefault,Creater=@Creater,[Status]=@Status
                    where RoleId=@ID
                --插入角色结束
                  --删除角色权限
                  delete [Sys_RoleRight] where [RoleId]=@ID
                 --设置权限开始
                        SELECT @LengthId=dbo.GetSplitLength(@rightIDs,',');
                        while @nextId<=@LengthId 
                          begin
                           --当前职位id
			                SET @RightId = left(dbo.GetSplitOfIndex(@rightIDs,',',@nextId), 100);           			  
                              if(@RightId<>'' and @RightId<>'0')
			                   begin
                            INSERT INTO [Sys_RoleRight]
                           ([RoleId],[RightId])
			                   VALUES(@ID,@RightId)
			                   end
                            SET @nextId=@nextId+1;
                          end  
                 --设置权限结束
                end
                -----------
                IF @@error=0
                BEGIN
                  COMMIT TRAN 
                   SELECT @roleId as ID     
                END
                ELSE
                BEGIN
                  ROLLBACK TRAN
                 SELECT 0 as ID     
               END
            ";

                #endregion

                var param = new
                    {
                        ID = model.RoleId,
                        rightIDs,
                        model.RoleName,
                        model.RoleDesc,
                        model.IsDefault,
                        model.Creater,
                        model.Status
                    };

                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.RoleId = decimal.ToInt32(id);
            }
        }

        /// <summary>
        ///     增加角色
        /// </summary>
        /// <param name="model">角色实体</param>
        public void Add(Sys_Roles model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"
if (@IsDefault=1)
   begin
     update Sys_Roles set IsDefault=0
   end
insert into Sys_Roles(RoleName,RoleDesc,IsDefault,Creater,[Status]) values(@RoleName,@RoleDesc,@IsDefault,@Creater,@Status)
                                SELECT @@Identity as Id";
                var param = new
                    {
                        ID = model.RoleId,
                        model.RoleName,
                        model.RoleDesc,
                        model.IsDefault,
                        model.Creater,
                        model.Status
                    };

                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.RoleId = decimal.ToInt32(id);
            }
        }

        /// <summary>
        ///     修改角色信息
        /// </summary>
        /// <param name="model">角色实体</param>
        /// <returns>执行结果</returns>
        public bool Update(Sys_Roles model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"
                if (@IsDefault=1)
   begin
     update Sys_Roles set IsDefault=0
   end
                update Sys_Roles set RoleName=@RoleName,RoleDesc=@RoleDesc,IsDefault=@IsDefault,Status=@Status where RoleId=@RoleId";
                var param = new
                    {
                        model.RoleId,
                        model.RoleName,
                        model.RoleDesc,
                        model.IsDefault,
                        model.Status
                    };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        ///     删除角色状态
        /// </summary>
        /// <param name="roleIds">角色ID</param>
        /// <returns>操作状态</returns>
        public bool Delete(string roleIds)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"update Sys_Roles set IsDelete=1 where RoleID in ({0})
                                            delete Sys_UserRole where RoleID in ({0})
                                            delete Sys_RoleRight where RoleID in ({0})
                                        ", roleIds);
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        public Sys_Roles Get(int roleId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select * from Sys_Roles where RoleId=@RoleId";
                var param = new
                    {
                        RoleId = roleId
                    };
                return conn.Query<Sys_Roles>(sqlwhere, param).FirstOrDefault();
            }
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        public List<Sys_Roles> GetList(string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select Sys_Roles.*,Sys_User.Realname from Sys_Roles
                                left join Sys_User on Sys_User.UserId=Sys_Roles.Creater where " + where;
                return conn.Query<Sys_Roles>(sql).ToList();
            }
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        public List<Sys_Roles> GetListByUserId(int userId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sql = @"select Sys_Roles.* from Sys_Roles
where (RoleId in (select RoleId from Sys_UserRole where UserId = @UserId) or Isdefault=1) and IsDelete=0";
                return conn.Query<Sys_Roles>(sql, new { UserId = userId }).ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool SetDefaultRole(int roleId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sql = @"update Sys_Roles set IsDefault = 0
                                    update Sys_Roles set IsDefault = 1 where RoleId=@RoleId";
                int result = conn.Execute(sql, new { RoleId = roleId });
                return result > 0;
            }
        }

        /// <summary>
        ///     更改角色状态
        /// </summary>
        /// <param name="rolesID">角色ID</param>
        /// <param name="status">
        ///     状态【0：正常
        ///     <para>1：冻结】</para>
        /// </param>
        /// <returns>操作状态</returns>
        public bool UpdateStatus(int rolesID, int status)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"update Roles set Status=@Status where RoleID=@RoleID";
                var param = new
                    {
                        Status = status,
                        RoleID = rolesID
                    };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        ///     增加角色的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="rightId"></param>
        public void AddRightToRole(int roleId, string rightIds)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"
                        DECLARE @RightId int;--获取单个的id
                        DECLARE @nextId int;--单个的id的下标
                        SET @nextId=1;
                        DECLARE @LengthId int;--Id的集合中共有多少个id

                        delete from Sys_RoleRight where RoleId=@roleId         

                        SELECT @LengthId=dbo.GetSplitLength(@rightIDs,',');
                        while @nextId<=@LengthId
                          begin
                           --当前职位id
			                SET @RightId = left(dbo.GetSplitOfIndex(@rightIDs,',',@nextId), 100);           			  
                              if(@RightId<>'' and @RightId<>'0')
			                   begin
                            INSERT INTO [Sys_RoleRight]
                           ([RoleId],[RightId])
			                   VALUES(@roleId,@RightId)
			                   end
                            SET @nextId=@nextId+1;
                          end  ";
                var param = new
                    {
                        roleId,
                        rightIDs = rightIds
                    };
                conn.Execute(sqlwhere, param);
            }
        }

        /// <summary>
        ///     删除指定角色权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="rightId">权限ID</param>
        /// <returns>删除结果</returns>
        public void DeleteRoleRight(int roleId, int rightId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string delSql = "delete from Sys_RoleRight where RoleId=@RoleId and RightId=@RightId";
                connection.Execute(delSql, new { RoleId = roleId, RightId = rightId });
            }
        }

        /// <summary>
        ///     获取角色信息
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Sys_Roles> GetList(out int totalCount, string where = "1=1", int startIndex = 0,
                                       int pageLength = int.MaxValue, string orderBy = "ORDER BY RoleId DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>("select count(1) from Sys_Roles where " + where).First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {2} ) as rowNum
,Sys_Roles.*
,Sys_User.Realname
,(case Sys_Roles.isdefault when 1 then (select count(0) from Sys_User where isdelete = 0 and isteacher < 2)
when 0 then (select count(0) from Sys_UserRole where Sys_UserRole.roleId = Sys_Roles.roleid) end ) as UserCount
from Sys_Roles
left join Sys_User on Sys_User.UserId=Sys_Roles.Creater  
where {1}
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1) ", pageLength, where, orderBy);

                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };

                return connection.Query<Sys_Roles>(query, parameters).ToList();
            }
        }

       
    }
}