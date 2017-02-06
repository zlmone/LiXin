using System;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace LiXinDataAccess
{
    public class BaseDB
    {
        //连接配置
        private static string _mongodbConnectionString = "";
        //数据库名称
        private static string _mongodbName = "";
        //连接对象
        public static MongoServer MongoService { set; get; }

        private static readonly Object ThisLock = new Object();

        /// <summary>
        ///     实例化对象
        /// </summary>
        public static MongoDatabase CreateInstance()
        {
            if (MongoService == null)
            {
                _mongodbConnectionString = ConfigurationManager.AppSettings["MongodbConnectionString"];
                _mongodbName = ConfigurationManager.AppSettings["MongodbName"];
                MongoServerSettings set=new MongoServerSettings();
                MongoService = MongoServer.Create(_mongodbConnectionString);
            }
            try
            {
                if (!MongoService.DatabaseExists(_mongodbName))
                {
                    MongoService.CreateDatabaseSettings(_mongodbName);
                }
                return MongoService.GetDatabase(_mongodbName);
            }
            finally
            {
                MongoService.Disconnect();
            }
        }

        //获取Seqence
        public static int GetSeqence(string collectionIndexName)
        {
            try
            {
                lock (ThisLock)
                {
                    var db = CreateInstance();
                    //判断是否存在
                    if (!db.CollectionExists(collectionIndexName))
                    {
                        db.CreateCollection(collectionIndexName);
                        db[collectionIndexName].Insert(new {index = 0});
                    }
                    //返回Seqence
                    int index = db[collectionIndexName].FindOne()["index"].ToInt32();
                    db[collectionIndexName].Update(Query.EQ("index", index), Update.Set("index", index + 1));
                    return index + 1;
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                MongoService.Disconnect();
            }
        }
    }
}