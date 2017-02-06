using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
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
        /// <param name="deptId">部门ID集合</param>
        /// <returns></returns>
        public Dictionary<int, tbQuestionSort> GetAllQuestionSortDictionary(List<int> deptId = null)
        {
            if (deptId == null)
                return GetAllList<tbQuestionSort>(false).ToDictionary(p => p._id, p => p);
            return GetAllList<tbQuestionSort>(false).Where(p => deptId.Contains(p.DeptId)).ToDictionary(p => p._id,p => p);
        }

        /// <summary>
        ///     获取所有的试题库
        /// </summary>
        /// <param name="deptId">部门ID集合</param>
        /// <returns></returns>
        public List<tbQuestionSort> GetAllQuestionSortList(List<int> deptId=null)
        {
            try
            {
                var coll = BaseDB.CreateInstance().GetCollection<tbQuestionSort>("tbQuestionSort");
                return deptId == null ? coll.Find(Query.EQ("Status", 0)).ToList() : coll.Find(Query.And(Query.In("DeptId", new BsonArray(deptId)),Query.EQ("Status", 0))).ToList();
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
        /// <param name="sortID"></param>
        /// <param name="qSort"></param>
        /// <returns></returns>
        public int ModifyByID(int sortID, tbQuestionSort qSort)
        {
            try
            {
                var coll = BaseDB.CreateInstance().GetCollection<tbQuestionSort>("tbQuestionSort");
                var update = new UpdateDocument
                    {
                        {"FatherID", qSort.FatherID},
                        {"Title", qSort.Title},
                        {"Description", qSort.Description},
                        {"Status", qSort.Status}
                    };
                coll.Update(Query.EQ("_id", sortID),update);
                return sortID;
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