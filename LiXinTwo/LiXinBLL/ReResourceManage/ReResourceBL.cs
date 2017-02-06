using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.ReResourceManage;
using LiXinInterface.ReResourceManage;
using LixinModels.ReResourceManage;

namespace LiXinBLL.ReResourceManage

{
    public class ReResourceBL : IReResource
    {
        private readonly ReResourceDB _reResourceDB = new ReResourceDB();

        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Re_Resource GetModel(int id)
        {
            return _reResourceDB.GetModel(id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">要添加的实体对象</param>
        public int AddModel(Re_Resource model)
        {
            _reResourceDB.AddModel(model);
            return model.ResourceID;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateModel(Re_Resource model)
        {
            return _reResourceDB.UpdateModel(model);
        }


        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="ids">ID集合格式为: 1,2,3,4</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBatchModel(string ids)
        {
            return _reResourceDB.DeleteBatchModel(ids);
        }


        /// <summary>
        /// 获取知识资源列表
        /// <param name="totalCount">总数</param>
        /// <param name="where">条件语句格式" and ..."</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// </summary>
        public List<Re_Resource> GetResourceList(out int totalCount, string where = "", int startIndex = 1, int startLength = int.MaxValue, string orderBy = "")
        {
            var list = _reResourceDB.GetResourceList(where, startIndex, startLength, orderBy);
            totalCount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 根据资源名称判断资源是否存在
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <param name="sufFix">后缀名</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="statu">资源状态0-正常，1-删除</param>
        /// <returns></returns>
        public bool Exists(string resourceName, string sufFix, int resourceId = 0, int statu = 0)
        {
            return _reResourceDB.Exists(resourceName, sufFix, resourceId, statu);
        }
        
        
    }
}