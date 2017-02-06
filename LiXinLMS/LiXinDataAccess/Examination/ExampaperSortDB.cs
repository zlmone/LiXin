using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using LiXinModels.Examination.DBModel;

namespace LiXinDataAccess.Examination
{
    public class ExampaperSortDB : BaseMethod
    {
        /// <summary>
        ///     根据ID修改分类
        /// </summary>
        /// <returns></returns>
        public int ModifyByID(int id, tbExampaperSort sort)
        {
            try
            {
                MongoCollection<tbExampaperSort> coll =
                    BaseDB.CreateInstance().GetCollection<tbExampaperSort>("tbExampaperSort");
                var b = new UpdateDocument
                    {
                        {"Description", sort.Description},
                        {"Title", sort.Title},
                        {"FatherID", sort.FatherID},
                        {"Status", sort.Status}
                    };
                coll.Update(Query.EQ("_id", id), b);
                return id;
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

        /// <summary>
        ///     获取试卷分类字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, tbExampaperSort> GetAllExampaperSortDictionary()
        {
            return GetAllList<tbExampaperSort>(false).ToDictionary(p => p._id, p => p);
        }
    }
}