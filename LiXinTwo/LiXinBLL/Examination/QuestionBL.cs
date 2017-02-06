using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using LiXinDataAccess.Examination;
using LiXinInterface.Examination;
using LiXinModels.Examination.DBModel;

namespace LiXinBLL.Examination
{
    public class QuestionBL : IQuestion
    {
        public static QuestionDB Qdb;

        public KnowledgeKeyDB kdb;

        public QuestionSortDB sortdb;

        public QuestionBL()
        {
            Qdb = new QuestionDB();
            kdb = new KnowledgeKeyDB();
            sortdb = new QuestionSortDB();
        }

        /// <summary>
        ///     获取所有的试题
        /// </summary>
        /// <returns></returns>
        public List<tbQuestion> GetQuestionList()
        {
            return Qdb.GetQuestionList();
        }


        /// <summary>
        ///     获取所有的试题
        /// </summary>
        /// <param name="qIDs">传入的问题字符串，以逗号隔开</param>
        /// <returns></returns>
        public List<tbQuestion> GetQuestionList(string qIDs)
        {
            var listID = new List<int>();
            if (!qIDs.Contains(","))
            {
                listID.Add(Convert.ToInt32(qIDs));
            }
            else
            {
                foreach (string item in qIDs.Split(','))
                {
                    listID.Add(Convert.ToInt32(item));
                }
            }
            return Qdb.GetAllList<tbQuestion>(Query.In("_id", new BsonArray(listID)));
        }

        /// <summary>
        ///     插入问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public int Insert(tbQuestion question)
        {
            return Qdb.Insert(question);
        }


        /// <summary>
        ///     删除题库
        /// </summary>
        /// <param name="questionID"></param>
        /// <returns></returns>
        public bool Delete(string questionID)
        {
            try
            {
                foreach (string item in questionID.Split(','))
                {
                    Qdb.Delete<tbQuestion>(Convert.ToInt32(item), false);
                }
                return true;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        ///     修改试题
        /// </summary>
        /// <param name="question">修改内容的集合</param>
        /// <returns></returns>
        public int ModifyByID(tbQuestion question)
        {
            Qdb.Modify(question);
            return question._id;
        }

        /// <summary>
        ///     根据ID获取试题
        /// </summary>
        /// <returns></returns>
        public tbQuestion GetSingleByID(int id)
        {
            return Qdb.GetSingleByID<tbQuestion>(id);
        }

        /// <summary>
        ///     根据条件查询问题列表并且分页
        /// </summary>
        /// <param name="sortID">分类ID</param>
        /// <param name="content">题目</param>
        /// <param name="type">类型</param>
        /// <param name="level">难度</param>
        /// <param name="keyName">知识点</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="deptId">部门ID集合</param>
        /// <param name="totalcount">总数</param>
        /// <returns></returns>
        public List<tbQuestion> GetListByQuery(ref int totalcount, int sortID, string content, string keyName,
                                               int pageSize, int pageIndex, int type, int level, List<int> deptId = null)
        {
            var listkey = new List<int>();
            var sotrlist = new List<int>();
            if (!string.IsNullOrEmpty(keyName))
            {
                var keyList =
                    kdb.GetAllList<tbKnowledgeKey>(false).Where(p => p.KeyName.Contains(keyName)).ToList();
                listkey = keyList.Select(p => p._id).ToList();
                if (listkey.Count == 0)
                {
                    return new List<tbQuestion>();
                }
            }
            if (sortID != 0)
            {
                sotrlist.Add(sortID);
                var list = sortdb.GetAllQuestionSortList();
                GetSortByFatherID(list, sortID, ref sotrlist);
            }

            return Qdb.GetListByQuery(ref totalcount, sotrlist, content, listkey, pageSize, pageIndex, type, level,deptId);
        }

        /// <summary>
        ///     根据一个父题库获取下面所有的子题库
        /// </summary>
        public void GetSortByFatherID(List<tbQuestionSort> list, int sortID, ref List<int> sortlist)
        {
            IEnumerable<tbQuestionSort> newlist = list.Where(p => p.FatherID == sortID);
            if (newlist.Count() > 0)
            {
                foreach (tbQuestionSort item in newlist)
                {
                    sortlist.Add(item._id);
                    GetSortByFatherID(list, item._id, ref sortlist);
                }
            }
        }
    }
}