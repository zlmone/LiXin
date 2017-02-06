using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using LiXinDataAccess.Examination;
using LiXinInterface.Examination;
using LiXinModels.Examination.DBModel;

namespace LiXinBLL.Examination
{
    public class ExampaperBL : IExampaper
    {
        private static ExampaperDB EDB;

        public ExampaperBL()
        {
            EDB = new ExampaperDB();
        }

        /// <summary>
        ///     添加试卷
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public int InsertExampaper(tbExampaper sort)
        {
            return EDB.Insert(sort);
        }

        /// <summary>
        ///     获取试卷列表
        /// </summary>
        /// <returns></returns>
        public List<tbExampaper> GetAllExampaperList()
        {
            return EDB.GetAllExampaperList();
        }

        /// <summary>
        ///     根据ID获取单个实体
        /// </summary>
        /// <returns></returns>
        public tbExampaper GetExampaper(int id)
        {
            return EDB.GetExampaper(id);
        }

        /// <summary>
        ///     根据ID删除单个实体
        /// </summary>
        /// <returns></returns>
        public bool DeleteExampaper(int id, bool isTrueDelete)
        {
            return EDB.DeleteExampaper(id, isTrueDelete);
        }

        /// <summary>
        ///     根据ID批量删除单个实体
        /// </summary>
        /// <returns></returns>
        public bool DeleteExampapers(string ids)
        {
            return EDB.DeleteExampapers(ids);
        }

        /// <summary>
        ///     根据ID修改单个实体
        /// </summary>
        /// <returns></returns>
        public bool UpdateExampaper(tbExampaper tbE)
        {
            return EDB.Modify(tbE);
        }

        /// <summary>
        ///     获取指定ID集合的考试
        /// </summary>
        /// <param name="paperIDs">考试ID集合</param>
        /// <param name="isContainDeleted">true:包含已被删除的；false:去除已被删除的</param>
        /// <returns></returns>
        public List<tbExampaper> GetAllExampaper(IEnumerable<int> paperIDs, bool isContainDeleted = false)
        {
            IMongoQuery query = Query.In("_id", new BsonArray(paperIDs));
            if (!isContainDeleted)
                Query.And(Query.EQ("Status", 0), query);
            return EDB.GetAllList<tbExampaper>(query);
        }

        /// <summary>
        ///     获取已删除的试卷列表
        /// </summary>
        /// <returns></returns>
        public List<tbExampaper> GetAllDeleteExampaperList()
        {
            return EDB.GetAllDeleteExampaperList();
        }
    }
}