using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using LiXinCommon;
using LiXinDataAccess.Examination;
using LiXinInterface.Examination;
using LiXinModels.Examination.DBModel;

namespace LiXinBLL.Examination
{
    public class KnowledgeKeyBL : IKnowledgeKey
    {
        protected KnowledgeKeyDB Kkdb;
        protected QuestionDB Qdb;

        public KnowledgeKeyBL()
        {
            Kkdb = new KnowledgeKeyDB();
            Qdb = new QuestionDB();
        }

        /// <summary>
        ///     获取所有的知识点
        /// </summary>
        /// <param name="totalCount">总条数</param>
        /// <param name="keyName">关键字</param>
        /// <param name="pageSize">没有显示条数</param>
        /// <param name="pageIndex">页数</param>
        /// <returns></returns>
        public List<tbKnowledgeKey> GetAllKnowledgeKeys(ref int totalCount, string keyName = "", int pageSize = 20,
                                                        int pageIndex = 1)
        {
            return Kkdb.GetAllList<tbKnowledgeKey>(ref totalCount, Query.Matches("KeyName", keyName.FuzzyQuery()),
                                                   new SortByDocument("_id", -1), pageIndex, pageSize);
        }

        /// <summary>
        ///     添加新的知识点
        /// </summary>
        /// <param name="key">实体</param>
        /// <returns></returns>
        public int InsertKey(tbKnowledgeKey key)
        {
            return Kkdb.Insert(key);
        }

        /// <summary>
        ///     添加新的知识点
        /// </summary>
        /// <param name="key">实体</param>
        /// <returns></returns>
        public int ModifyKey(tbKnowledgeKey key)
        {
            return Kkdb.Modify(key) ? key._id : 0;
        }

        /// <summary>
        ///     判断是否已经存在此知识点
        /// </summary>
        /// <param name="keyName">知识点名称</param>
        /// <returns></returns>
        public bool IsExsitSortName(string keyName, int id = 0)
        {
            return Kkdb.IsExsitName<tbKnowledgeKey>("KeyName", keyName, id);
        }

        /// <summary>
        ///     获取单个知识点
        /// </summary>
        /// <param name="id">知识点ID</param>
        /// <returns></returns>
        public tbKnowledgeKey GetSingleByID(int id)
        {
            return Kkdb.GetSingleByID<tbKnowledgeKey>(id);
        }

        /// <summary>
        ///     删除知识点
        /// </summary>
        /// <param name="id">知识点ID</param>
        /// <returns></returns>
        public bool DeleteKey(int id)
        {
            return Qdb.GetAllList<tbQuestion>(false).Count(p => p.QuestionKey == id) == 0 &&
                   Qdb.Delete<tbKnowledgeKey>(id, false);
        }

        /// <summary>
        ///     获取知识点
        /// </summary>
        /// <param name="isContainDeleted">true:包含已被删除的；false:去除已被删除的</param>
        /// <returns></returns>
        public List<tbKnowledgeKey> GetKnowledgeKey(bool isContainDeleted)
        {
            return Kkdb.GetAllList<tbKnowledgeKey>(false);
        }

        /// <summary>
        ///     获取知识点的字典分类
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, tbKnowledgeKey> GetAllQuestionSortDictionary()
        {
            return Kkdb.GetAllQuestionSortDictionary();
        }
    }
}