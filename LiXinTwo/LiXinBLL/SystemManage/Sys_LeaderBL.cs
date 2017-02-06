using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.SystemManage;
using LiXinModels.SystemManage;
using LiXinInterface.SystemManage;
using LiXinModels.User;
using LiXinDataAccess.User;

namespace LiXinBLL.SystemManage
{
    public class Sys_LeaderBL : ISys_Leader
    {
        private static Sys_LeaderDB groupDB;
        private static Sys_UserLinkLeaderDB groupUSERDB;
        private static UserDB userDB;

        public Sys_LeaderBL()
        {
            groupDB = new Sys_LeaderDB();
            groupUSERDB = new Sys_UserLinkLeaderDB();
            userDB = new UserDB();
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
        public int Insert(Sys_LeaderConfig sort)
        {
            groupDB.AddGroup(sort);
            return sort.Id;
        }

        /// <summary>
        /// 根据ID获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_LeaderConfig GetModel(int id)
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
        public bool UpdateByID(Sys_LeaderConfig sort)
        {
            return groupDB.UpdateGroup(sort);
        }


        /// <summary>
        /// 获取群组分类列表
        /// </summary>
        /// <returns></returns>
        public List<Sys_LeaderConfig> GetAllList(out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string strWhere = "1=1")
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

        /// <summary>
        /// 在走审批流程时，通过用户userId获取真正的审批人HRID
        /// <para>1.有指定则查找指定领导返回</para>
        /// <para>2.如没指定则查找本身的Leader返回</para>
        /// <para>3.以上都没有返回null</para>
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="flag">标志  0：按顺序操作；1：跳过第2步</param>
        /// <returns>
        /// 返回审批人HRID
        /// </returns>
        public Sys_User GetLeaderIdByUserId(int userId, int flag = 0)
        {
            var leaderList = groupDB.GetGroupList(string.Format(" id in (select groupid from Sys_UserLinkLeader where userid = {0})", userId));
            if (leaderList.Count > 0)
            {
                var leader = userDB.Get(leaderList[0].UserId);
                if (leader != null && leader.JobNum.Trim() != "")
                    return leader;
            }
            if (flag == 0)
            {
                var tmp = userDB.GetList(string.Format(" jobnum = (select LeaderID from Sys_User where userId = {0}) and jobnum <> ''", userId));
                if (tmp.Count > 0)
                    return tmp[0];
            }
            return null;
        }
    }
}
