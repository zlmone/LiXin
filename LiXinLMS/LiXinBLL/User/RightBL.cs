using System.Collections.Generic;
using LiXinDataAccess.User;
using LiXinInterface.User;
using LiXinModels.User;

namespace LiXinBLL.User
{
    public class RightBL : IRight
    {
        private readonly RightDB _rightDB = new RightDB();

        /// <summary>
        /// </summary>
        /// <param name="rightName"></param>
        /// <param name="rightId"></param>
        /// <returns></returns>
        public bool Exists(string rightName, int rightId)
        {
            return _rightDB.Exists(rightName, rightId);
        }

        /// <summary>
        ///     插入权限
        /// </summary>
        /// <returns></returns>
        public void Add(Sys_Right model)
        {
            _rightDB.Add(model);
        }

        /// <summary>
        ///     修改权限
        /// </summary>
        /// <returns></returns>
        public bool Update(Sys_Right model)
        {
            return _rightDB.Update(model);
        }

        /// <summary>
        ///     删除权限
        /// </summary>
        /// <returns></returns>
        public bool Delete(int rightId)
        {
            return _rightDB.Delete(rightId);
        }

        /// <summary>
        ///     通过userId查询用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Sys_Right> GetRightByUserId(int userId)
        {
            return _rightDB.GetRightByUserId(userId);
        }

        /// <summary>
        ///     通过postId查询用户权限
        /// </summary>
        /// <returns></returns>
        public List<Sys_Right> GetRightByRoleId(int roleId)
        {
            return _rightDB.GetRightByRoleId(roleId);
        }

        /// <summary>
        ///     根据id获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_Right Get(int rightId)
        {
            return _rightDB.Get(rightId);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public List<Sys_Right> GetAllRights()
        {
            return _rightDB.GetList();
        }

        /// <summary>
        ///     根据ID获取所有的子权限-
        /// </summary>
        /// <param name="iParentId"></param>
        /// <returns></returns>
        public List<Sys_Right> GetChlidRight(int iParentId)
        {
            string strCondition = " 1 = 1";
            if (iParentId >= 0)
            {
                strCondition = " ParentId=" + iParentId;
            }
            return _rightDB.GetList(strCondition);
        }
    }
}