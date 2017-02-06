using System.Collections.Generic;
using System.Linq;
using LiXinModels.Examination.DBModel;

namespace LiXinDataAccess.Examination
{
    public class KnowledgeKeyDB : BaseMethod
    {
        /// <summary>
        ///     获取知识点的字典分类
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, tbKnowledgeKey> GetAllQuestionSortDictionary()
        {
            return GetAllList<tbKnowledgeKey>(false).ToDictionary(p => p._id, p => p);
        }
    }
}