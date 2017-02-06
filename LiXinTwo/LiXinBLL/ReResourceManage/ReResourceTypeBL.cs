using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.ReResourceManage;
using LiXinInterface.ReResourceManage;
using LixinModels.ReResourceManage;

namespace LiXinBLL.ReResourceManage

{
    public class ReResourceTypeBL : IReResourceType
    {
        private readonly ReResourceTypeDB _reResourceTypeDB = new ReResourceTypeDB();

        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Re_ResourceType GetModel(int id)
        {
            return _reResourceTypeDB.GetModel(id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">要添加的实体对象</param>
        public int AddModel(Re_ResourceType model)
        {
            _reResourceTypeDB.AddModel(model);
            return model.ResourceTypeID;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateModel(Re_ResourceType model)
        {
            return _reResourceTypeDB.UpdateModel(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteModel(int id)
        {
            return _reResourceTypeDB.DeleteModel(id);
        }

        /// <summary>
        /// 判断类别名称是否存在
        /// </summary>
        /// <param name="sortName">类别名称</param>
        /// <param name="id">分类ID</param>
        /// <param name="parentId">分类父级ID</param>
        /// <returns></returns>
        public bool IsExist(string sortName,  int id = 0, int parentId = 0)
        {
            return _reResourceTypeDB.IsExist(sortName,id,parentId);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="strWhere">条件语句</param>
        /// <returns></returns>
        public List<Re_ResourceType> GetResourceTypeList(string strWhere = "1=1")
        {
            return _reResourceTypeDB.GetResourceTypeList(strWhere);
        }

        /// <summary>
        /// 判断类别下是否存在子节点或资源
        /// </summary>
        /// <param name="typeId">类别ID</param>
        /// <returns>true:存在</returns>
        public bool IsExistsChildNodeOrResource(int typeId)
        {
            return _reResourceTypeDB.IsExistsChildNodeOrResource(typeId);
        }
    }
}