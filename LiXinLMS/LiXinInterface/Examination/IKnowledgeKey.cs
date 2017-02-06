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
        /// <returns></returns>
        List<tbKnowledgeKey> GetAllKnowledgeKeys(ref int totalCount, string keyName = "", int pageSize = 20,
                                                 int pageIndex = 1);

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
        /// <returns></returns>
        bool IsExsitSortName(string keyName, int id = 0);

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