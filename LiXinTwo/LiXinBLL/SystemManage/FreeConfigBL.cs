using LiXinDataAccess.SystemManage;
using LiXinInterface.SystemManage;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.SystemManage
{
    public class FreeConfigBL : IFreeConfig
    {
        private FreeConfigDB freeDB;
        public FreeConfigBL()
        {
            freeDB = new FreeConfigDB();
        }

        #region 其他形式-其他项目
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertFree_OtherApplyConfig(Free_OtherApplyConfig model)
        {
            freeDB.InsertFree_OtherApplyConfig(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateFree_OtherApplyConfig(Free_OtherApplyConfig model)
        {
            freeDB.UpdateFree_OtherApplyConfig(model);
        }

        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        public void DeleteFree_OtherApplyConfig(string IDlist)
        {
            freeDB.DeleteFree_OtherApplyConfig(IDlist);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Free_OtherApplyConfig GetModel(string where = "1=1")
        {
            return freeDB.GetModel(where);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<Free_OtherApplyConfig> GetFreeOtherList(out int totalCount, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue)
        {
            var list = freeDB.GetFreeOtherList(where, startIndex, startLength);
            totalCount = list.Count() == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }
        #endregion


        public List<Free_OtherApplyConfig> GetFreeOtherList_New(string where = "1=1")
        {
            var list = freeDB.GetFreeOtherList_New(where);
           
            return list;
        }


        #region 免修配置-免修项目
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertFree_ApplyConfig(Free_ApplyConfig model)
        {
            freeDB.InsertFree_ApplyConfig(model);
        }

         /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateFree_ApplyConfig(Free_ApplyConfig model)
        {
            freeDB.UpdateFree_ApplyConfig(model);
        }

        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        public void DeleteFree_ApplyConfig(string IDlist)
        {
            freeDB.DeleteFree_ApplyConfig(IDlist);
        }

         /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Free_ApplyConfig GetFree_ApplyConfig(string where = "1=1")
        {
           return  freeDB.GetFree_ApplyConfig(where);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<Free_ApplyConfig> GetFreeApplyList(out int totalCount, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue)
        {
            var list = freeDB.GetFreeApplyList(where, startIndex, startLength);
            totalCount = list.Count() == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        public List<Free_UserOtherApply> GetReport_Free_CPA(string where)
        {
            var list= freeDB.GetReport_Free_CPA(where);
            //Free_UserOtherApply model = null;
            //var freeConfig = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == DateTime.Now.Year).FirstOrDefault();

            return list;
        }
        #endregion

    }
}
