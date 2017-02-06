using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace LiXinDataAccess
{
    public class BaseMethod
    {
        /// <summary>
        ///     Insert New Document
        /// </summary>
        /// <typeparam name="T">Entity "T"</typeparam>
        /// <param name="o"></param>
        /// <returns>newdocument'id</returns>
        public int Insert<T>(T o)
        {
            try
            {
                Type t = typeof (T);
                MongoCollection<BsonDocument> coll = BaseDB.CreateInstance().GetCollection(t.Name);
                int seq = BaseDB.GetSeqence("Seqence" + t.Name);
                o.GetType().GetProperty("_id").SetValue(o, seq, null);
                coll.Insert(o);
                return seq;
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
        ///     Insert New Document Collectionss
        /// </summary>
        /// <typeparam name="T">Entity Collections "T"</typeparam>
        /// <param name="os"></param>
        /// <returns></returns>
        public bool Insert<T>(IEnumerable<T> os)
        {
            try
            {
                Type t = typeof (T);
                MongoCollection<BsonDocument> coll = BaseDB.CreateInstance().GetCollection(t.Name);

                foreach (T o in os)
                {
                    int seq = BaseDB.GetSeqence("Seqence" + t.Name);
                    o.GetType().GetProperty("_id").SetValue(o, seq, null);
                    coll.Insert(o);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     Delete Entity By ID
        /// </summary>
        /// <typeparam name="T">Entity"T"</typeparam>
        /// <param name="id">ID</param>
        /// <param name="isTrueDelete">true:delete true，false:delete false</param>
        /// <returns></returns>
        public bool Delete<T>(int id, bool isTrueDelete)
        {
            try
            {
                MongoCollection<T> coll = BaseDB.CreateInstance().GetCollection<T>(typeof (T).Name);
                if (isTrueDelete)
                    coll.Remove(Query.EQ("_id", id));
                else
                    coll.Update(Query.EQ("_id", id), Update.Set("Status", 1), UpdateFlags.Multi, SafeMode.True);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     Delete Entity By QueryConditions
        /// </summary>
        /// <typeparam name="T">Entity"T"</typeparam>
        /// <param name="query">QueryConditions</param>
        /// <param name="isTrueDelete">true:delete true，false:delete false</param>
        /// <returns></returns>
        public bool Delete<T>(IMongoQuery query, bool isTrueDelete)
        {
            try
            {
                MongoCollection<T> coll = BaseDB.CreateInstance().GetCollection<T>(typeof (T).Name);
                if (isTrueDelete)
                    coll.Remove(query);
                else
                    coll.Update(query, Update.Set("Status", 1), UpdateFlags.Multi, SafeMode.True);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     Get Single Entity By ID
        /// </summary>
        /// <typeparam name="T">Entity"T"</typeparam>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public T GetSingleByID<T>(int id)
        {
            try
            {
                var coll = BaseDB.CreateInstance().GetCollection<T>(typeof (T).Name);
                return coll.FindOne(Query.EQ("_id", id));
            }
            catch
            {
                return (T) typeof (T).Assembly.CreateInstance(typeof (T).Name);
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     Modify Entity
        /// </summary>
        /// <returns></returns>
        public bool Modify<T>(T o)
        {
            try
            {
                var coll = BaseDB.CreateInstance().GetCollection<T>(typeof (T).Name);
                coll.Save(o);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     Judge Entity Is Exsist
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="fildName">FildName</param>
        /// <param name="fildValue">FildValue</param>
        /// <returns></returns>
        public bool IsExsitName<T>(string fildName, string fildValue, int id = 0)
        {
            try
            {
                MongoCollection<T> coll = BaseDB.CreateInstance().GetCollection<T>(typeof (T).Name);
                if (id == 0)
                    return coll.Count(Query.And(Query.EQ(fildName, fildValue), Query.EQ("Status", 0))) <= 0;
                else
                    return
                        coll.Count(Query.And(Query.EQ(fildName, fildValue), Query.NE("_id", id), Query.EQ("Status", 0))) <=
                        0;
            }
            catch
            {
                return false;
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     Judge Entity Is Exsist
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="query">QueryConditions</param>
        /// <returns></returns>
        public bool IsExsitName<T>(IMongoQuery query)
        {
            try
            {
                MongoCollection<T> coll = BaseDB.CreateInstance().GetCollection<T>(typeof (T).Name);
                return coll.Count(query) <= 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     Get Entity List
        /// </summary>
        /// <param name="isContainDeleted">true:包含已被删除的；false:去除已被删除的</param>
        /// <returns></returns>
        public List<T> GetAllList<T>(bool isContainDeleted = false)
        {
            try
            {
                MongoCollection<T> coll = BaseDB.CreateInstance().GetCollection<T>(typeof (T).Name);
                return isContainDeleted ? coll.FindAll().ToList() : coll.Find(Query.EQ("Status", 0)).ToList();
            }
            catch
            {
                return new List<T>();
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     Get Entity List
        /// </summary>
        /// <param name="query">query</param>
        /// <returns></returns>
        public List<T> GetAllList<T>(IMongoQuery query = null)
        {
            try
            {
                MongoCollection<T> coll = BaseDB.CreateInstance().GetCollection<T>(typeof (T).Name);
                return query == null ? coll.FindAll().ToList() : coll.Find(query).ToList();
            }
            catch
            {
                return new List<T>();
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     Get Entity List(paging、sorting)
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="totalCount">data'count</param>
        /// <param name="query">QueryConditions</param>
        /// <param name="sort">SortConditions</param>
        /// <param name="pageIndex">PageIndex</param>
        /// <param name="pageSize">PageSize</param>
        /// <returns></returns>
        public List<T> GetAllList<T>(ref int totalCount, IMongoQuery query = null, IMongoSortBy sort = null,
                                     int pageIndex = 0, int pageSize = 0)
        {
            try
            {
                MongoCollection<T> collection = BaseDB.CreateInstance().GetCollection<T>(typeof (T).Name);

                sort = sort ?? new SortByDocument();
                pageSize = (pageSize == 0) ? 1 : pageSize;
                totalCount =
                    (int)
                    (query == null
                         ? collection.Find(Query.EQ("Status", 0)).Count()
                         : collection.Find(Query.And(query, Query.EQ("Status", 0))).Count());
                if (pageIndex < 1)
                    return ((query == null) ? collection.FindAll() : collection.Find(query)).SetSortOrder(sort).ToList();
                else
                    return
                        ((query == null)
                             ? collection.Find(Query.EQ("Status", 0))
                             : collection.Find(Query.And(query, Query.EQ("Status", 0)))).SetSortOrder(sort).SetSkip(
                                 (pageIndex - 1)*pageSize).SetLimit(pageSize).ToList();
            }
            catch
            {
                return new List<T>();
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }
		
    }
}