﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;

namespace LiXinInterface.SystemManage
{
    public interface ISys_Group
    {
        /// <summary>
        /// 获得某人所在的群组
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="isdeleteflag">
        /// 是否验证群组已删除
        /// <para>1：验证</para>
        /// <para>0：不验证</para>
        /// </param>
        /// <returns></returns>
        List<Sys_GroupUser> GetGroupList(int userID, int isdeleteflag = 1);

        /// <summary>
        /// 添加新的群组
        /// </summary>
        int Insert(Sys_Group sort);

        /// <summary>
        /// 根据ID获取单个实体
        /// </summary>
        Sys_Group GetModel(int id);

        /// <summary>
        /// 根据ID删除单个实体(假删)
        /// </summary>
        bool DeleteModel(string ids);

        /// <summary>
        /// 修改群组
        /// </summary>
        bool UpdateByID(Sys_Group sort);

        /// <summary>
        /// 获取群组分类列表
        /// </summary>
        List<Sys_Group> GetAllList(out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string strWhere = "1=1");

        /// <summary>
        /// 验证重名
        /// </summary>
        bool Checkname(string groupName, int groupId);


        #region 群组关联表操作

        /// <summary>
        /// 获取群组用户信息
        /// </summary>
        List<Sys_GroupUser> GroupUserList(int groupId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1");
        /// <summary>
        /// 获取选择用户信息
        /// </summary>
        List<Sys_GroupUser> GetUser(string userIDs, out int totalcount, int startIndex = 1, int startLength = int.MaxValue);

        /// <summary>
        /// 批量添加取群组用户信息
        /// </summary>
        bool AddGroupUser(int groupid, string userIDs);

        /// <summary>
        /// 批量添加取群组用户信息
        /// </summary>
        bool DeleteGroupUser(int groupid, string userIDs);

        /// <summary>
        /// 根据群组ID删除全部用户信息
        /// </summary>
        /// <returns></returns>
        bool DeleteGroupAllUser(int groupid);
        /// <summary>
        /// 获取群组用户ID
        /// </summary>
        List<int> GroupUserID(int groupId);
        #endregion
    }
}
