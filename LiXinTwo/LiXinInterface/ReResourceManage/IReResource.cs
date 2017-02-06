using System.Collections.Generic;
using LixinModels.ReResourceManage;

namespace LiXinInterface.ReResourceManage 

{
    public interface IReResource
    {
        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         Re_Resource GetModel(int id);

        /// <summary>
        /// 增加一条数据
        /// </summary>
         /// <param name="model">要添加的实体对象</param>
         int AddModel(Re_Resource model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
         bool UpdateModel(Re_Resource model);

         
         /// <summary>
         /// 批量删除数据
         /// </summary>
         /// <param name="ids">ID集合格式为: 1,2,3,4</param>
         /// <returns>成功返回True，失败返回False</returns>
         bool DeleteBatchModel(string ids);

        

        /// <summary>
        /// 获取知识资源列表
        /// <param name="totalCount">总数</param>
        /// <param name="where">条件语句格式" and ..."</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// </summary>
        List<Re_Resource> GetResourceList(out int totalCount, string where = "", int startIndex = 1,
                                          int startLength = int.MaxValue, string orderBy = "");

        /// <summary>
        /// 根据资源名称判断资源是否存在
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <param name="sufFix">后缀名</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="statu">资源状态0-正常，1-删除</param>
        /// <returns></returns>
        bool Exists(string resourceName, string sufFix, int resourceId = 0, int statu = 0);
    }
}