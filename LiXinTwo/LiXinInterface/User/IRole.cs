using System.Collections.Generic;
using LiXinModels.User;

namespace LiXinInterface.User
{
    public interface IRole
    {
        /// <summary>
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="rightId"></param>
        void AddRightToRole(int roleId, string rightIds);

        /// <summary>
        /// </summary>
        /// <param name="rolesId"></param>
        /// <returns></returns>
        bool Delete(string roleIds);

        /// <summary>
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="rightId"></param>
        void DeleteRoleRight(int roleId, int rightId);

        /// <summary>
        ///     根据条件查询角色信息(条件要需where关键字)
        /// </summary>
        /// <param name="sqlCondition">查询条件</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="iDisplayStart">开始数</param>
        /// <param name="iDisplayLength">每页显示数</param>
        /// <returns>返回对应角色区间List</returns>
        List<Sys_Roles> GetRightsByCondition(string strWhere, out int totalCount, int iDisplayStart, int iDisplayLength);

        /// <summary>
        /// </summary>
        /// <param name="rolesId"></param>
        /// <returns></returns>
        Sys_Roles Get(int rolesId);

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Sys_Roles> GetListByUserId(int userId);

        /// <summary>
        ///     增加角色
        ///     zhangjf20120820
        /// </summary>
        /// <param name="model">角色实体</param>
        /// <param name="rightIDs">权限集合，用'，'隔开</param>
        void Add(Sys_Roles roles, string rightIDs);

        /// <summary>
        ///     增加角色
        /// </summary>
        /// <param name="model">角色实体</param>
        void Add(Sys_Roles roles);

        /// <summary>
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool Update(Sys_Roles roles);

        /// <summary>
        ///     判断角色名是否存在
        /// </summary>
        /// <param name="role">角色model</param>
        /// <returns>True:存在 False:不存在</returns>
        bool Exists(string roleName, int roleId);

        /// <summary>
        ///     获取所有角色信息
        /// </summary>
        /// <returns></returns>
        List<Sys_Roles> GetAllRoles();

        /// <summary>
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        bool SetDefaultRole(int roleId);

        /// <summary>
        ///     设置角色冻结/解冻
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        bool SetStatus(int roleId);

        /// <summary>
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="strWhere"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Sys_Roles> GetList(out int totalCount, string strWhere, int startIndex, int pageLength, string orderBy);

        /// <summary>
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="roleName"></param>
        /// <param name="status"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        List<Sys_Roles> GetList(out int totalCount, string roleName, int status, int startIndex, int pageLength, string orderBy);
    }
}