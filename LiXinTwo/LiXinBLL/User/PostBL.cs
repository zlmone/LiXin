using System.Collections.Generic;
using LiXinDataAccess.User;
using LiXinInterface.User;
using LiXinModels.User;

namespace LiXinBLL.User
{
    //按树形结构记录所有岗位
    public class PostBL : IPost
    {
        //private readonly PostDB _postDB = new PostDB();
        private PostDB _postDB = new PostDB();

        /// <summary>
        /// </summary>
        /// <param name="postCode"></param>
        /// <param name="postName"></param>
        /// <param name="postId"></param>
        public bool Exists(string postCode, string postName, int postId)
        {
            return _postDB.Exists(postCode, postName, postId);
        }

        /// <summary>
        ///     增加一条数据
        /// </summary>
        public void Add(Sys_Post model)
        {
            _postDB.Add(model);
        }

        /// <summary>
        ///     更新一条数据
        /// </summary>
        public bool Update(Sys_Post model)
        {
            return _postDB.Update(model);
        }

        /// <summary>
        ///     删除一条数据
        /// </summary>
        public bool Delete(int postId)
        {
            return _postDB.Delete(postId);
        }

        /// <summary>
        ///     得到一个对象实体
        /// </summary>
        public Sys_Post Get(int postId)
        {
            return _postDB.Get(postId);
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        public List<Sys_Post> GetAllPost()
        {
            return _postDB.GetList();
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        public List<Sys_Post> GetList()
        {
            return _postDB.GetList();
        }

        /// <summary>
        ///     更改上级
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool ChangeParent(int postId, int parentId)
        {
            Sys_Post post = _postDB.Get(postId);
            post.ParentId = parentId;
            return _postDB.Update(post);
        }

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="deptId"></param>
        public void AddUserToPost(string userIds, int postId)
        {
            _postDB.AddUserToPost(userIds, postId);
        }

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        public void DeleteUserPost(string userIds)
        {
            _postDB.DeleteUserPost(userIds);
        }
    }
}