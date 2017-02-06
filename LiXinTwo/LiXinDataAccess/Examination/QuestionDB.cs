using System.Collections.Generic;
using System.Linq;
using LiXinCommon;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using LiXinModels.Examination.DBModel;

namespace LiXinDataAccess.Examination
{
    public class QuestionDB : BaseMethod
    {
        /// <summary>
        ///     获取所有的试题
        /// </summary>
        /// <returns></returns>
        public List<tbQuestion> GetQuestionList()
        {
            try
            {
                var coll = BaseDB.CreateInstance().GetCollection<tbQuestion>("tbQuestion");
                return coll.Find(Query.EQ("Status", 0)).ToList();
            }
            catch
            {
                return new List<tbQuestion>();
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     根据条件查询问题列表并且分页
        /// </summary>
        /// <param name="sortID">分类ID</param>
        /// <param name="content">题目</param>
        /// <param name="type">类型</param>
        /// <param name="level">难度</param>
        /// <param name="key">知识点</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="totalcount">总数</param>
        /// <param name="deptId">部门ID集合</param>
        /// <returns></returns>
        public List<tbQuestion> GetListByQuery(ref int totalcount, List<int> sortID, string content, List<int> key, int pageSize, int pageIndex, int type, int level,List<int> deptId=null )
        {
            var querylist = new List<IMongoQuery>();

            #region 当查询内容含有特殊字符时模糊查询有问题

            if (content != "")
            {
                querylist.Add(Query.Matches("QuestionContent", new BsonRegularExpression("" + content.FuzzyQuery() + "")));
            }

            #endregion

            if (key.Count != 0)
            {
                querylist.Add(Query.In("QuestionKey", new BsonArray(key)));
            }
            if (type != 0)
            {
                querylist.Add(Query.EQ("QuestionType", type));
            }
            if (level != 0)
            {
                querylist.Add(Query.EQ("QuestionLevel", level));
            }
            if (sortID.Count != 0)
            {
                querylist.Add(Query.In("QuestionSortID", new BsonArray(sortID)));
            }
            if(deptId!=null)
            {
                querylist.Add(Query.In("DeptId", new BsonArray(deptId)));
            }

            var qu = querylist.Count == 0 ? null : Query.And(querylist);
            return GetAllList<tbQuestion>(ref totalcount, qu, new SortByDocument("_id", -1), pageIndex, pageSize);
        }
    }
}