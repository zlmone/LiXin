using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DepTranManage;
using LiXinDataAccess.SystemManage;
using LiXinInterface.DepTranManage;
using LiXinModels.DepTranManage;
using LiXinModels.SystemManage;
using LiXinModels.User;
using LiXinDataAccess.User;

namespace LiXinBLL.DepTranManage
{
    public class DepTran_DepartLeaderConfigBL : IDepTran_DepartLeaderConfig
    {
        private static DepTran_DepartConfigDB groupDB;
        private static UserDB userDB;

        public DepTran_DepartLeaderConfigBL()
        {
            groupDB = new DepTran_DepartConfigDB();
            userDB = new UserDB();
        }

        #region 群组操作

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Sys_GroupUser> GetDepartCourseLimitNumber(int userID,int courseid)
        {
            return groupDB.GetDepartCourseLimitNumber(userID, courseid);
        }

        /// <summary>
        /// 添加新的部门设置
        /// </summary>
        /// <param name="departSetting"></param>
        /// <returns></returns>
        public int Insert(DepTranDepartSetting departSetting)
        {
            groupDB.AddDepartSetting(departSetting);
            return departSetting.Id;
        }

        /// <summary>
        /// 根据ID获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DepTranDepartSetting GetModel(int id)
        {
            return groupDB.GetDepartSetting(id);
        }

        /// <summary>
        /// 根据ID删除单个实体(假删)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteModel(string ids)
        {
            return groupDB.DeleteDepartSetting(ids);
        }

        /// <summary>
        /// 修改群组
        /// </summary>
        /// <returns></returns>
        public bool UpdateByID(DepTranDepartSetting departSetting)
        {
            return groupDB.UpdateDepartSetting(departSetting);
        }


        /// <summary>
        /// 获取群组分类列表
        /// </summary>
        /// <returns></returns>
        public List<DepTranDepartSetting> GetAllList(out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string strWhere = "1=1", string jsRenderSortField = "ORDER BY t0.Id")
        {
            var list = groupDB.GetAllDepartSettingList(startIndex, startLength, strWhere, jsRenderSortField);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 验证重名
        /// </summary>
        /// <returns></returns>
        public bool Checkname(string departName, int departId)
        {
            return groupDB.Exists(departName, departId);
        }
        #endregion

        #region 群组关联表操作

        /// <summary>
        /// 获取群组用户信息
        /// </summary>
        /// <returns></returns>
        public List<Sys_GroupUser> DepartSettingUserList(int departId, out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1")
        {
            var list = groupDB.GetDepartSettingUserList(departId, startIndex, startLength, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 获取选择用户信息
        /// </summary>
        /// <returns></returns>
        public List<Sys_GroupUser> GetUser(string userIDs, out int totalcount, int startIndex = 0, int startLength = int.MaxValue)
        {
            var list = groupDB.GetUser(userIDs, startIndex, startLength);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 批量添加部门用户信息
        /// </summary>
        /// <returns></returns>
        public bool AddDepartSettingUser(int departId, string userIDs)
        {
            return groupDB.AddDepartSettingUser(departId, userIDs);
        }

        /// <summary>
        /// 批量删除群组用户信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteDepartSettingUser(int departId, string userIDs)
        {
            return groupDB.DeleteDepartSettingUser(departId, userIDs);
        }

        /// <summary>
        /// 根据群组ID删除全部用户信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteDepartSettingAllUser(int departId)
        {
            return groupDB.DeleteDepartSettingAllUser(departId);
        }

        /// <summary>
        /// 获取群组用户ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetDepartSettingUserID(int departSetID)
        {
            var list = groupDB.GetDepartSettingUserID(departSetID);
            return list;
        }

        /// <summary>
        /// 根据人员ID查找部门ID
        /// </summary>
        public int GetDepartSettingDepartSetID(int UserId)
        {
            var list = groupDB.GetDepartSettingDepartSetID(UserId);
            return list; 
        }
        #endregion

        #region 考勤操作
        /// <summary>
        /// 根据UserID获得数据
        /// </summary>
        public List<DepTranDepartSetting> GetDepartSettingByUserId(int UserID)
        {
            return groupDB.GetDepartSettingByUserId(UserID);
        }

        #endregion
    }
}
