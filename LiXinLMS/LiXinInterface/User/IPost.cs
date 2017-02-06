using System.Collections.Generic;
using LiXinModels.User;

namespace LiXinInterface.User
{
    public interface IPost
    {
        /// <summary>
        /// </summary>
        /// <param name="postCode"></param>
        /// <param name="postName"></param>
        /// <param name="postId"></param>
        bool Exists(string postCode, string postName, int postId);

        /// <summary>
        ///     增加一条数据
        /// </summary>
        void Add(Sys_Post model);

        /// <summary>
        ///     更新一条数据
        /// </summary>
        bool Update(Sys_Post model);

        /// <summary>
        ///     删除一条数据
        /// </summary>
        bool Delete(int postId);

        /// <summary>
        ///     得到一个对象实体
        /// </summary>
        Sys_Post Get(int postId);

        /// <summary>
        ///     获得数据列表
        /// </summary>
        List<Sys_Post> GetAllPost();

        /// <summary>
        ///     获得数据列表
        /// </summary>
        List<Sys_Post> GetList();

        /// <summary>
        ///     更改上级
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        bool ChangeParent(int postId, int parentId);

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="deptId"></param>
        void AddUserToPost(string userIds, int postId);

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        void DeleteUserPost(string userIds);
    }
}