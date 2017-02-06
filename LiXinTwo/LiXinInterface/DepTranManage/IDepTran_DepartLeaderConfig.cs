using LiXinModels.DepTranManage;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.User;

namespace LiXinInterface.DepTranManage
{
    public interface IDepTran_DepartLeaderConfig
    {
        /// <summary>
        /// 获得数据列表
        /// </summary>
        List<Sys_GroupUser> GetDepartCourseLimitNumber(int userID,int courseid);
        /// <summary>
        /// 添加新的部门设置
        /// </summary>
        /// <param name="departSetting"></param>
        /// <returns></returns>
        int Insert(DepTranDepartSetting departSetting);

        /// <summary>
        /// 根据ID获取单个实体
        /// </summary>
        DepTranDepartSetting GetModel(int id);

        /// <summary>
        /// 根据ID删除单个实体(假删)
        /// </summary>
        bool DeleteModel(string ids);

        /// <summary>
        /// 修改群组
        /// </summary>
        bool UpdateByID(DepTranDepartSetting departSetting);

        /// <summary>
        /// 获取群组分类列表
        /// </summary>
        List<DepTranDepartSetting> GetAllList(out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string strWhere = "1=1", string jsRenderSortField = "ORDER BY t0.Id");

        /// <summary>
        /// 验证重名
        /// </summary>
        bool Checkname(string departName, int departId);


        #region 群组关联表操作

        /// <summary>
        /// 获取群组用户信息
        /// </summary>
        List<Sys_GroupUser> DepartSettingUserList(int departId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1");
        /// <summary>
        /// 获取选择用户信息
        /// </summary>
        List<Sys_GroupUser> GetUser(string userIDs, out int totalcount, int startIndex = 1, int startLength = int.MaxValue);

        /// <summary>
        /// 批量添加部门用户信息
        /// </summary>
        bool AddDepartSettingUser(int departId, string userIDs);

        /// <summary>
        /// 批量添加部门用户信息
        /// </summary>
        bool DeleteDepartSettingUser(int departId, string userIDs);

        /// <summary>
        /// 根据群组ID删除全部用户信息
        /// </summary>
        /// <returns></returns>
        bool DeleteDepartSettingAllUser(int departId);
        /// <summary>
        /// 获取群组用户ID
        /// </summary>
        List<int> GetDepartSettingUserID(int departSetID);


        int GetDepartSettingDepartSetID(int UserId);
        #endregion

        #region 考勤操作
        /// <summary>
        /// 根据UserID获得数据
        /// </summary>
        List<DepTranDepartSetting> GetDepartSettingByUserId(int UserID);

        #endregion
    }
}
