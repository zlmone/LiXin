using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using LiXinModels.Examination.DBModel;

namespace LiXinDataAccess.Examination
{
    public class ExampaperDB : BaseMethod
    {
        //public ExampaperDB()
        //{  
        //}
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <summary>
        ///     获取试卷列表
        /// </summary>
        /// <returns></returns>
        public List<tbExampaper> GetAllExampaperList()
        {
            try
            {
                MongoCollection<tbExampaper> coll = BaseDB.CreateInstance().GetCollection<tbExampaper>("tbExampaper");
                return coll.Find(Query.EQ("Status", 0)).ToList();
            }
            catch
            {
                return new List<tbExampaper>();
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     获取已删除的试卷列表
        /// </summary>
        /// <returns></returns>
        public List<tbExampaper> GetAllDeleteExampaperList()
        {
            try
            {
                MongoCollection<tbExampaper> coll = BaseDB.CreateInstance().GetCollection<tbExampaper>("tbExampaper");
                return coll.Find(Query.EQ("Status", 1)).ToList();
            }
            catch
            {
                return new List<tbExampaper>();
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     根据ID获取单个实体
        /// </summary>
        /// <returns></returns>
        public tbExampaper GetExampaper(int id)
        {
            try
            {
                MongoCollection<tbExampaper> coll = BaseDB.CreateInstance().GetCollection<tbExampaper>("tbExampaper");
                return coll.FindOne(Query.EQ("_id", id));
            }
            catch
            {
                return new tbExampaper();
            }
            finally
            {
                BaseDB.MongoService.Disconnect();
            }
        }

        /// <summary>
        ///     根据ID删除单个实体
        /// </summary>
        /// <returns></returns>
        public bool DeleteExampaper(int id, bool isTrueDelete)
        {
            try
            {
                MongoCollection<tbExampaper> coll = BaseDB.CreateInstance().GetCollection<tbExampaper>("tbExampaper");
                if (isTrueDelete)
                    coll.Remove(Query.EQ("_id", id));
                else
                    coll.Update(Query.EQ("_id", id), Update.Set("Status", "1"));
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
        ///     根据ID批量删除单个实体
        /// </summary>
        /// <returns></returns>
        public bool DeleteExampapers(string idlist)
        {
            try
            {
                MongoCollection<tbExampaper> coll = BaseDB.CreateInstance().GetCollection<tbExampaper>("tbExampaper");
                //BsonArray b = new BsonArray();
                string[] ids = idlist.Split(',');
                for (int i = 0; i < ids.Length - 1; i++)
                {
                    //b.Add(ids[i].StringToInt32());
                    coll.Update(Query.EQ("_id", Convert.ToInt32(ids[i])), Update.Set("Status", 1));
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
        ///     根据ID修改单个实体
        /// </summary>
        /// <returns></returns>
        public bool UpdateExampaper(tbExampaper tbE)
        {
            try
            {
                MongoCollection<tbExampaper> coll = BaseDB.CreateInstance().GetCollection<tbExampaper>("tbExampaper");
                tbExampaper exam = coll.FindOne(Query.EQ("_id", tbE._id));
                tbE.UserID = exam.UserID;
                coll.Remove(Query.EQ("_id", tbE._id));
                coll.Insert(tbE);
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
    }
}