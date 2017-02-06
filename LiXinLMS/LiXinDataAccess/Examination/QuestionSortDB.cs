using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using LiXinModels.Examination.DBModel;

namespace LiXinDataAccess.Examination
{
    public class QuestionSortDB : BaseMethod
    {
        /// <summary>
        ///     获取问题分类字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, tbQuestionSort> GetAllQuestionSortDictionary()
        {
            return GetAllList<tbQuestionSort>(false).ToDictionary(p => p._id, p => p);
        }

        /// <summary>
        ///     获取所有的试题库
        /// </summary>
        /// <returns></returns>
        public List<tbQuestionSort> GetAllQuestionSortList()
        {
            try
            {
                MongoCollection<tbQuestionSort> coll =
                    BaseDB.CreateInstance().GetCollection<tbQuestionSort>("tbQuestionSort");
                return coll.Find(Query.EQ("Status", 0)).ToList();
            }
            catch
            {
                return new List<tbQuestionSort>();
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     修改试题库
        /// </summary>
        /// <param name="SortID"></param>
        /// <param name="qSort"></param>
        /// <returns></returns>
        public int ModifyByID(int SortID, tbQuestionSort qSort)
        {
            try
            {
                MongoCollection<tbQuestionSort> coll =
                    BaseDB.CreateInstance().GetCollection<tbQuestionSort>("tbQuestionSort");
                var update = new UpdateDocument
                    {
                        {"FatherID", qSort.FatherID},
                        {"Title", qSort.Title},
                        {"Description", qSort.Description},
                        {"Status", qSort.Status}
                    };
                coll.Update(Query.EQ("_id", SortID),update);
                return SortID;
            }
            catch
            {
                return 0;
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }
    }
}