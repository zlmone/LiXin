using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.User;

namespace LiXinDataAccess.User
{
    public class RightDB : BaseRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool Exists(string rightName, int rightId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select count(*) from Sys_Right WHERE rightName=@rightName ";
                if (rightId > 0)
                    sqlwhere += " and rightId <> " + rightId;
                var param = new
                    {
                        rightName
                    };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }

        /// <summary>
        ///     插入权限
        /// </summary>
        /// <returns>rightId</returns>
        public void Add(Sys_Right model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Sys_Right(RightName,RightType,Path,RightDesc,ParentId,ShowOrder) values(@RightName,@RightType,@Path,@RightDesc,@ParentId,@ShowOrder)
                    SELECT @@Identity AS ID
";
                var param = new
                    {
                        model.RightName,
                        model.RightType,
                        model.Path,
                        model.RightDesc,
                        model.ParentId,
                        model.ShowOrder
                    };
                dynamic id = connection.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.RightId = Convert.ToInt32(id);
            }
        }

        /// <summary>
        ///     修改权限
        /// </summary>
        /// <returns></returns>
        public bool Update(Sys_Right model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string sqlwhere =
                    "update Sys_Right set RightName=@RightName,RightType=@RightType,Path=@Path,RightDesc=@RightDesc,ParentId=@ParentId,ShowOrder=@ShowOrder,ModuleName=@moduleName where RightId=@RightId";
                var param = new
                    {
                        model.RightName,
                        model.RightType,
                        model.Path,
                        model.RightDesc,
                        model.ParentId,
                        model.ShowOrder,
                        model.RightId,
                        moduleName=model.ModuleName
                    };
                int result = connection.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        ///     删除权限
        /// </summary>
        /// <returns></returns>
        public bool Delete(int rightId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = @"delete from Sys_Right where RightId=@RightId
                    delete from Sys_RoleRight where rightID = @RightId
                ";
                var param = new
                    {
                        RightId = rightId
                    };
                int result = connection.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="rightId"></param>
        /// <returns></returns>
        public Sys_Right Get(int rightId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string sqlwhere = "select * from Sys_Right where RightId=@RightId";
                var param = new
                    {
                        RightId = rightId
                    };
                return connection.Query<Sys_Right>(sqlwhere, param).FirstOrDefault();
            }
        }

        /// <summary>
        ///     获取系统中的权限
        /// </summary>
        /// <returns></returns>
        public List<Sys_Right> GetList(string strWhere = " 1 = 1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "select * from Sys_Right where " + strWhere;
                return connection.Query<Sys_Right>(sql).ToList();
            }
        }

        /// <summary>
        ///     通过userId查询用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Sys_Right> GetRightByUserId(int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string sqlwhere = @"select Sys_Right.* from Sys_Right 
                                            left join Sys_RoleRight on Sys_Right.RightId=Sys_RoleRight.RightId
                                            left join Sys_Roles on Sys_Roles.RoleId= Sys_RoleRight.RoleId
                                            left join Sys_UserRole on Sys_UserRole.RoleId=Sys_RoleRight.RoleId
                                            where Sys_UserRole.UserId=@userId and Sys_Roles.status = 0
                                            union 
                                            select Sys_Right.* from Sys_Right 
                                            left join Sys_RoleRight on Sys_Right.RightId=Sys_RoleRight.RightId
                                            left join Sys_Roles on Sys_Roles.RoleId=Sys_RoleRight.RoleId
                                            where Isdefault=1";
                var param = new {userId};
                return connection.Query<Sys_Right>(sqlwhere, param).ToList();
            }
        }

        /// <summary>
        ///     通过roleId查询用户权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<Sys_Right> GetRightByRoleId(int roleId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string sqlwhere = @"select Sys_Right.* from Sys_Right 
                                            left join Sys_RoleRight on Sys_Right.RightId=Sys_RoleRight.RightId
                                            where Sys_RoleRight.RoleId=@roleId";
                var param = new {roleId};
                return connection.Query<Sys_Right>(sqlwhere, param).ToList();
            }
        }
    }
}