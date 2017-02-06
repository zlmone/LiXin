using System.Collections.Generic;
using LiXinModels.Examination.DBModel;

namespace LiXinInterface.Examination
{
    public interface IKnowledgeKey
    {
        /// <summary>
        ///     获取所有的知识点
        /// </summary>
        /// <param name="totalCount">总条数</param>
        /// <param name="keyName">关键字</param>
        /// <param name="pageSize">没有显示条数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="deptList">部门ID</param>
        /// <returns></returns>
        List<tbKnowledgeKey> GetAllKnowledgeKeys(ref int totalCount, string keyName = "", int pageSize = 20, int pageIndex = 1, IEnumerable<int> deptList = null);

        /// <summary>
        ///     添加新的知识点
        /// </summary>
        /// <param name="key">实体</param>
        /// <returns></returns>
        int InsertKey(tbKnowledgeKey key);

        /// <summary>
        ///     添加新的知识点
        /// </summary>
        /// <param name="key">实体</param>
        /// <returns></returns>
        int ModifyKey(tbKnowledgeKey key);

        /// <summary>
        ///     获取单个知识点
        /// </summary>
        /// <param name="id">知识点ID</param>
        /// <returns></returns>
        tbKnowledgeKey GetSingleByID(int id);

        /// <summary>
        ///     判断是否已经存在此知识点
        /// </summary>
        /// <param name="keyName">知识点名称</param>
        /// <param name="id">知识点ID</param>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        bool IsExsitSortName(string keyName, int id = 0, int deptId = 0);

        /// <summary>
        /// 判断是否已经存在此知识点（部门分所开放至全所使用add by yxt）
        /// </summary>
        /// <param name="keyName">知识点名称</param>
        ///  <param name="deptId">所属部门ID</param>
        /// <returns></returns>
        bool IsExsitCoSortName(string keyName, int deptId);

        /// <summary>
        ///     删除知识点
        /// </summary>
        /// <param name="id">知识点ID</param>
        /// <returns></returns>
        bool DeleteKey(int id);

        /// <summary>
        ///     获取知识点
        /// </summary>
        /// <param name="isContainDeleted">true:包含已被删除的；false:去除已被删除的</param>
        /// <returns></returns>
        List<tbKnowledgeKey> GetKnowledgeKey(bool isContainDeleted);

        /// <summary>
        ///     获取知识点的字典分类
        /// </summary>
        /// <returns></returns>
        Dictionary<int, tbKnowledgeKey> GetAllQuestionSortDictionary();
    }
}