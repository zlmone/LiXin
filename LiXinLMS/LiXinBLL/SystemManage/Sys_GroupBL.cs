using System.Collections.Generic;
using LiXinDataAccess.SystemManage;
using LiXinModels.SystemManage;
using LiXinInterface.SystemManage;

namespace LiXinBLL.SystemManage
{
    public class Sys_GroupBL : ISys_Group
    {
        private static Sys_GroupDB groupDB;
        private static Sys_GroupUserDB groupUSERDB;

        public Sys_GroupBL()
        {
            groupDB = new Sys_GroupDB();
            groupUSERDB = new Sys_GroupUserDB();
        }

        #region 群组操作


        /// <summary>
        /// 获得某人所在的群组
        /// </summary>
        public List<Sys_GroupUser> GetGroupList(int userID, int isdeleteflag = 1)
        {
            return groupUSERDB.GetGroupList(userID, isdeleteflag);
        }

        /// <summary>
        /// 添加新的群组
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public int Insert(Sys_Group sort)
        {
            groupDB.AddGroup(sort);
            return sort.GroupId;
        }

        /// <summary>
        /// 根据ID获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_Group GetModel(int id)
        {
            return groupDB.GetGroup(id);
        }

        /// <summary>
        /// 根据ID删除单个实体(假删)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteModel(string ids)
        {
            return groupDB.DeleteGroup(ids);
        }

        /// <summary>
        /// 修改群组
        /// </summary>
        /// <returns></returns>
        public bool UpdateByID(Sys_Group sort)
        {
            return groupDB.UpdateGroup(sort);
        }


        /// <summary>
        /// 获取群组分类列表
        /// </summary>
        /// <returns></returns>
        public List<Sys_Group> GetAllList(out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string strWhere = "1=1")
        {
            var list = groupDB.GetAllGroupList(startIndex, startLength, strWhere);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 验证重名
        /// </summary>
        /// <returns></returns>
        public bool Checkname(string groupName, int groupId)
        {
            return groupDB.Exists(groupName, groupId);
        }
        #endregion

        #region 群组关联表操作

        /// <summary>
        /// 获取群组用户信息
        /// </summary>
        /// <returns></returns>
        public List<Sys_GroupUser> GroupUserList(int groupId, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1")
        {
            var list = groupUSERDB.GetGroupUserList(groupId, startIndex, startLength, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 获取选择用户信息
        /// </summary>
        /// <returns></returns>
        public List<Sys_GroupUser> GetUser(string userIDs, out int totalcount, int startIndex = 0, int startLength = int.MaxValue)
        {
            var list = groupUSERDB.GetUser(userIDs, startIndex, startLength);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 批量添加群组用户信息
        /// </summary>
        /// <returns></returns>
        public bool AddGroupUser(int groupid, string userIDs)
        {
            return groupUSERDB.AddGroupUser(groupid, userIDs);
        }

        /// <summary>
        /// 批量删除群组用户信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteGroupUser(int groupid, string userIDs)
        {
            return groupUSERDB.DeleteGroupUser(groupid, userIDs);
        }

        /// <summary>
        /// 根据群组ID删除全部用户信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteGroupAllUser(int groupid)
        {
            return groupUSERDB.DeleteGroupALLUser(groupid);
        }

        /// <summary>
        /// 获取群组用户ID
        /// </summary>
        /// <returns></returns>
        public List<int> GroupUserID(int groupId)
        {
            var list = groupUSERDB.GetGroupUserID(groupId);
            return list;
        }
        #endregion
    }
}
