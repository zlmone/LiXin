using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.SystemManage
{
    public interface IFreeConfig
    {
        #region 其他形式-其他项目
        /// <summary>
        /// 新增一条数据
        /// </summary> 
        void InsertFree_OtherApplyConfig(LiXinModels.SystemManage.Free_OtherApplyConfig model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        void UpdateFree_OtherApplyConfig(LiXinModels.SystemManage.Free_OtherApplyConfig model);

        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        void DeleteFree_OtherApplyConfig(string IDlist);

        /// <summary>
        /// 获取列表
        /// </summary>
        List<Free_OtherApplyConfig> GetFreeOtherList(out int totalCount, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue);

        /// <summary>
        /// 获取列表 不带分页
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Free_OtherApplyConfig> GetFreeOtherList_New(string where = "1=1");
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        Free_OtherApplyConfig GetModel(string where = "1=1");
        #endregion

        #region 免修配置-免修项目
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        void InsertFree_ApplyConfig(Free_ApplyConfig model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        void UpdateFree_ApplyConfig(Free_ApplyConfig model);

        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        void DeleteFree_ApplyConfig(string IDlist);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        Free_ApplyConfig GetFree_ApplyConfig(string where = "1=1");

         /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        List<Free_ApplyConfig> GetFreeApplyList(out int totalCount, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue);


        List<Free_UserOtherApply> GetReport_Free_CPA(string where);
  
        #endregion

    }
}
