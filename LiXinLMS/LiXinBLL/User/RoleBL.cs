using System.Collections.Generic;
using LiXinDataAccess.User;
using LiXinInterface.User;
using LiXinModels.User;

namespace LiXinBLL.User
{
    public class RoleBL : IRole
    {
        private readonly RoleDB _roleDB = new RoleDB();

        /// <summary>
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="rightId"></param>
        public void AddRightToRole(int roleId, string rightIds)
        {
            _roleDB.AddRightToRole(roleId, rightIds);
        }

        /// <summary>
        /// </summary>
        /// <param name="rolesId"></param>
        /// <returns></returns>
        public bool Delete(string roleIds)
        {
            return _roleDB.Delete(roleIds);
        }

        /// <summary>
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="rightId"></param>
        public void DeleteRoleRight(int roleId, int rightId)
        {
            _roleDB.DeleteRoleRight(roleId, rightId);
        }

        /// <summary>
        ///     根据条件查询角色信息(条件要需where关键字)
        /// </summary>
        /// <param name="sqlCondition">查询条件</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="iDisplayStart">开始数</param>
        /// <param name="iDisplayLength">每页显示数</param>
        /// <returns>返回对应角色区间List</returns>
        public List<Sys_Roles> GetRightsByCondition(string strWhere, out int totalCount, int iDisplayStart,
                                                    int iDisplayLength)
        {
            return _roleDB.GetList(out totalCount, strWhere, iDisplayStart, iDisplayLength);
        }

        /// <summary>
        /// </summary>
        /// <param name="rolesId"></param>
        /// <returns></returns>
        public Sys_Roles Get(int rolesId)
        {
            return _roleDB.Get(rolesId);
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Sys_Roles> GetListByUserId(int userId)
        {
            return _roleDB.GetListByUserId(userId);
        }

        /// <summary>
        ///     增加角色
        ///     zhangjf20120820
        /// </summary>
        /// <param name="model">角色实体</param>
        /// <param name="rightIDs">权限集合，用'，'隔开</param>
        public void Add(Sys_Roles roles, string rightIDs)
        {
            _roleDB.Add(roles, rightIDs);
        }

        /// <summary>
        ///     增加角色
        /// </summary>
        /// <param name="model">角色实体</param>
        public void Add(Sys_Roles roles)
        {
            _roleDB.Add(roles);
        }

        /// <summary>
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public bool Update(Sys_Roles roles)
        {
            return _roleDB.Update(roles);
        }

        /// <summary>
        ///     判断角色名是否存在
        /// </summary>
        /// <param name="role">角色model</param>
        /// <returns>True:存在 False:不存在</returns>
        public bool Exists(string roleName, int roleId)
        {
            return _roleDB.Exists(roleName, roleId);
        }

        /// <summary>
        ///     获取所有角色信息
        /// </summary>
        /// <returns></returns>
        public List<Sys_Roles> GetAllRoles()
        {
            return _roleDB.GetList();
        }

        /// <summary>
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool SetDefaultRole(int roleId)
        {
            return _roleDB.SetDefaultRole(roleId);
        }

        /// <summary>
        ///     设置角色冻结/解冻
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool SetStatus(int roleId)
        {
            Sys_Roles role = _roleDB.Get(roleId);
            if (role.Status == 0)
                role.Status = 1;
            else
                role.Status = 0;
            return _roleDB.Update(role);
        }

        /// <summary>
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="strWhere"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Sys_Roles> GetList(out int totalCount, string strWhere, int startIndex, int pageLength,
                                       string orderBy)
        {
            return _roleDB.GetList(out totalCount, strWhere, startIndex, pageLength, orderBy);
        }


        public List<Sys_Roles> GetList(out int totalCount, string roleName, int status, int startIndex, int pageLength, string orderBy = "ORDER BY RoleId DESC")
        {
            string strWhere = " Sys_Roles.IsDelete = 0";
            if (roleName != "")
                strWhere += string.Format(" and Sys_Roles.roleName like '%{0}%'", roleName);
            if (status != 99)
                strWhere += " and Sys_Roles.status = " + status;
            return _roleDB.GetList(out totalCount, strWhere, startIndex, pageLength, orderBy);
        }
    }
}