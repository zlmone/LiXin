using System.Collections.Generic;
using LiXinModels.User;

namespace LiXinInterface.User
{
    public interface IRight
    {
        /// <summary>
        /// </summary>
        /// <param name="rightName"></param>
        /// <param name="rightId"></param>
        /// <returns></returns>
        bool Exists(string rightName, int rightId);

        /// <summary>
        ///     插入权限
        /// </summary>
        /// <returns></returns>
        void Add(Sys_Right model);

        /// <summary>
        ///     修改权限
        /// </summary>
        /// <returns></returns>
        bool Update(Sys_Right model);

        /// <summary>
        ///     删除权限
        /// </summary>
        /// <returns></returns>
        bool Delete(int rightId);

        /// <summary>
        ///     通过userId查询用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Sys_Right> GetRightByUserId(int userId);

        /// <summary>
        ///     通过postId查询用户权限
        /// </summary>
        /// <returns></returns>
        List<Sys_Right> GetRightByRoleId(int roleId);

        /// <summary>
        ///     根据id获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Sys_Right Get(int rightId);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        List<Sys_Right> GetAllRights();

        /// <summary>
        ///     根据ID获取所有的子权限-
        /// </summary>
        /// <param name="iParentId"></param>
        /// <returns></returns>
        List<Sys_Right> GetChlidRight(int iParentId);
    }
}