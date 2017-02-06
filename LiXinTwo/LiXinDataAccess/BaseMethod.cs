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
        /// ObjectId的键
        /// </summary>
        private readonly string OBJECTID_KEY = "_id";

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
                var t = typeof(T);
                var coll = BaseDB.CreateInstance().GetCollection(t.Name);
                var seq = BaseDB.GetSeqence("Seqence" + t.Name);
                o.GetType().GetProperty("_id").SetValue(o, seq, null);
                coll.Insert(o, SafeMode.True);
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
                var t = typeof(T);
                var coll = BaseDB.CreateInstance().GetCollection(t.Name);

                foreach (T o in os)
                {
                    var seq = BaseDB.GetSeqence("Seqence" + t.Name);
                    o.GetType().GetProperty("_id").SetValue(o, seq, null);
                    coll.Insert(o, SafeMode.True);
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
                var coll = BaseDB.CreateInstance().GetCollection<T>(typeof(T).Name);
                if (isTrueDelete)
                    coll.Remove(Query.EQ("_id", id), SafeMode.True);
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
                MongoCollection<T> coll = BaseDB.CreateInstance().GetCollection<T>(typeof(T).Name);
                if (isTrueDelete)
                    coll.Remove(query, SafeMode.True);
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
                var coll = BaseDB.CreateInstance().GetCollection<T>(typeof(T).Name);
                return coll.FindOne(Query.EQ("_id", id));
            }
            catch
            {
                return (T)typeof(T).Assembly.CreateInstance(typeof(T).Name);
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
                var coll = BaseDB.CreateInstance().GetCollection<T>(typeof(T).Name);
                coll.Save(o, SafeMode.True);
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
        /// <returns></returns>
        public bool IsExsitName<T>(Dictionary<string, object> queryWhere, int id = 0)
        {
            try
            {
                var mongoQueryWhere = new List<IMongoQuery>();
                foreach (var key in queryWhere)
                {
                    if (key.Value is string)
                    {
                        mongoQueryWhere.Add(Query.EQ(key.Key, key.Value.ToString()));
                    }
                    if (key.Value is int)
                    {
                        mongoQueryWhere.Add(Query.EQ(key.Key, Convert.ToInt32(key.Value)));
                    }
                }
                mongoQueryWhere.Add(Query.EQ("Status", 0));
                if (id > 0)
                {
                    mongoQueryWhere.Add(Query.NE("_id", id));
                }
                var coll = BaseDB.CreateInstance().GetCollection<T>(typeof(T).Name);
                return coll.Count(Query.And(mongoQueryWhere)) <= 0;
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
                var coll = BaseDB.CreateInstance().GetCollection<T>(typeof(T).Name);
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
                var coll = BaseDB.CreateInstance().GetCollection<T>(typeof(T).Name);
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
        /// <param name="field">要查询的字段 逗号</param>
        /// <returns></returns>
        public List<T> GetAllList<T>(IMongoQuery query = null, int pageSize = 10, int pageIndex = 1, params string[] fields)
        {
            try
            {
                var coll = BaseDB.CreateInstance().GetCollection<T>(typeof(T).Name);
                return query == null ? coll.FindAll().ToList() : (fields.Count() == 0 ? coll.Find(query).ToList() : coll.Find(query).SetFields(fields).ToList());
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
                var collection = BaseDB.CreateInstance().GetCollection<T>(typeof(T).Name);

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
                                 (pageIndex - 1) * pageSize).SetLimit(pageSize).ToList();
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


        #region 分页查询 指定索引最后项-PageSize模式

        /// <summary>
        /// 分页查询 指定索引最后项-PageSize模式 
        /// </summary>
        /// <typeparam name="T">所需查询的数据的实体类型</typeparam>
        /// <param name="query">查询的条件 没有可以为null</param>
        /// <param name="indexName">索引名称</param>
        /// <param name="lastKeyValue">最后索引的值</param>
        /// <param name="pageSize">分页的尺寸</param>
        /// <param name="sortType">排序类型 1升序 -1降序 仅仅针对该索引</param>
        /// <param name="collectionName">指定的集合名称</param>
        /// <returns>返回一个List列表数据</returns>
        public List<T> Find<T>(out long totalcount, IMongoQuery query, string indexName, object lastKeyValue, int pageSize,int pageindex, int sortType)
        {
            MongoCollection<T> mc = BaseDB.CreateInstance().GetCollection<T>(typeof(T).Name);
            MongoCursor<T> mongoCursor = null;
            query = this.InitQuery(query); 
            totalcount = mc.Find(query).Count();
            //判断升降序后进行查询 
            if (sortType > 0)
            {
                //升序
                if (lastKeyValue != null)
                {
                    //有上一个主键的值传进来时才添加上一个主键的值的条件
                    query = Query.And(query, Query.GT(indexName, BsonValue.Create(lastKeyValue)));
                }
                //先按条件查询 再排序 再取数
                mongoCursor = mc.Find(query).SetSortOrder(new SortByDocument(indexName, 1)).SetSkip((pageindex-1)*pageSize).SetLimit(pageSize);
            } 
            else
            {
                //降序
                if (lastKeyValue != null)
                {
                    query = Query.And(query, Query.LT(indexName, BsonValue.Create(lastKeyValue)));
                }
                mongoCursor = mc.Find(query).SetSortOrder(new SortByDocument(indexName, -1)).SetSkip((pageindex - 1) * pageSize).SetLimit(pageSize);
            }
            return mongoCursor.ToList<T>();
        }

       


        #endregion

        /// <summary>
        /// 初始化查询记录 主要当该查询条件为空时 会附加一个恒真的查询条件，防止空查询报错
        /// </summary>
        /// <param name="query">查询的条件</param>
        /// <returns></returns>
        private IMongoQuery InitQuery(IMongoQuery query)
        {
            if (query == null)
            {
                //当查询为空时 附加恒真的条件 类似SQL：1=1的语法
                query = Query.Exists(OBJECTID_KEY);
            }
            return query;
        }

        /// <summary>
        /// 初始化排序条件  主要当条件为空时 会默认以ObjectId递增的一个排序
        /// </summary>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        private SortByDocument InitSortBy(SortByDocument sortBy)
        {
            if (sortBy == null)
            {
                //默认ObjectId 递增
                sortBy = new SortByDocument(OBJECTID_KEY, 1);
            }
            return sortBy;
        }


    }
}