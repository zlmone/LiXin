using System.Collections.Generic;
using LixinModels.ReResourceManage;

namespace LiXinInterface.ReResourceManage 

{
    public interface IReResourceType
    {
        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         Re_ResourceType GetModel(int id);

        /// <summary>
        /// 增加一条数据
        /// </summary>
         /// <param name="model">要添加的实体对象</param>
         int AddModel(Re_ResourceType model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
         bool UpdateModel(Re_ResourceType model);

         /// <summary>
         /// 删除一条数据
         /// </summary>
         /// <param name="id">ID</param>
         /// <returns>成功返回True，失败返回False</returns>
         bool DeleteModel(int id);

         /// <summary>
         /// 判断类别名称是否存在
         /// </summary>
         /// <param name="sortName">类别名称</param>
         /// <param name="id">分类ID</param>
         /// <param name="parentId">分类父级ID</param>
         /// <returns></returns>
         bool IsExist(string sortName, int id = 0, int parentId = 0);

        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere">条件语句</param>
        /// <returns></returns>
        List<Re_ResourceType> GetResourceTypeList(string strWhere = "1=1");

        /// <summary>
        /// 判断类别下是否存在子节点或资源
        /// </summary>
        /// <param name="typeId">类别ID</param>
        /// <returns>true:存在</returns>
        bool IsExistsChildNodeOrResource(int typeId);
    }
}