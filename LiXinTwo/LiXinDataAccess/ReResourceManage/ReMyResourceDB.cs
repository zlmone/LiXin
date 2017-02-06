using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Data;
using LixinModels.ReResourceManage;
using Retech.Core;
using Retech.Data;
namespace LiXinDataAccess.ReResourceManage
{
	public class ReMyResourceDB : BaseRepository
	{
	
	    /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Re_MyResource GetModel(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from Re_MyResource where  Id=@Id ";
                return conn.Query<Re_MyResource>(sqlstr, new { Id = id }).FirstOrDefault();
            }
        }
		
		/// <summary>
        /// 增加一条数据
        /// </summary>
         /// <param name="model">要添加的实体对象</param>
       public void AddModel(Re_MyResource model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @" INSERT INTO dbo.Re_MyResource (UserId, ResourceId, OpenTime, Score, Satisfaction, Practical, Attention
, IsCollection, CollectionTime, IsDownload, DownloadTime, Status, FavoriteId)
VALUES (@UserId, @ResourceId, @OpenTime, @Score, @Satisfaction, @Practical, @Attention
, @IsCollection, @CollectionTime, @IsDownload, @DownloadTime, @Status, @FavoriteId)
SELECT @@IDENTITY AS ID ";
                var param = new
                                {
                                    model.UserId,
                                    model.ResourceId,
                                    model.OpenTime,
                                    model.Score,
                                    model.Satisfaction,
                                    model.Practical,
                                    model.Attention,
                                    model.IsCollection,
                                    model.CollectionTime,
                                    model.IsDownload,
                                    model.DownloadTime,
                                    model.Status,
                                    model.FavoriteId
                                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }
        
         /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateModel(Re_MyResource model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @" UPDATE dbo.Re_MyResource
SET UserId = @UserId,
	ResourceId = @ResourceId,
	OpenTime = @OpenTime,
	Score = @Score,
	Satisfaction = @Satisfaction,
	Practical = @Practical,
	Attention = @Attention,
	IsCollection = @IsCollection,
	CollectionTime = @CollectionTime,
	IsDownload = @IsDownload,
	DownloadTime = @DownloadTime,
	Status = @Status,
	FavoriteId = @FavoriteId
WHERE Id = @Id ";
                var param = new
                                {
                                    model.Id,
                                    model.UserId,
                                    model.ResourceId,
                                    model.OpenTime,
                                    model.Score,
                                    model.Satisfaction,
                                    model.Practical,
                                    model.Attention,
                                    model.IsCollection,
                                    model.CollectionTime,
                                    model.IsDownload,
                                    model.DownloadTime,
                                    model.Status,
                                    model.FavoriteId
                                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="ids">ID集合格式为: 1,2,3,4</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBatchModel(string ids)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string delSql = string.Format(@" UPDATE dbo.Re_MyResource set Status = 1 where Id in ({0}) ", ids);
                int result = conn.Execute(delSql);
                return result > 0;
            }
        }


        /// <summary>
        /// 获取所有资源
        /// </summary>
        /// <returns></returns>
        public Re_Resource GetAllResource()
        {
            using (IDbConnection connection = OpenConnection())
            {
                string strSql = "select * from Re_Resource where Status=0";
                return connection.Query<Re_Resource>(strSql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取所有资源列表
        /// </summary>
        /// <returns></returns>
        public List<Re_Resource> GetResourceList(out int totalCount, string where, string orderBy, int startIndex = 1, int startLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>(string.Format(@"select count(1) from Re_Resource where {0} and Re_Resource.Status=0 ", where)).First();
                string strSql =
                    string.Format(
                        @"select top {0} * from (select row_number() over( {2} ) as rowNum,* from Re_Resource where {1} and Re_Resource.Status=0) result where rowNum BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex ",
                        startLength, where, orderBy);

                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return connection.Query<Re_Resource>(strSql, param).ToList();
            }
        }

        /// <summary>
        /// 根据ID获得资源
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Re_Resource GetResource(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlStr = "select rrs.* , rrt.TypeName  from Re_Resource rrs left join Re_ResourceType rrt on rrs.ResourceTypeID=rrt.ResourceTypeID where  ResourceID=@Id";
                var param = new
                    {
                        Id=Id
                    };
                return connection.Query<Re_Resource>(sqlStr, param).FirstOrDefault();
            }
        } 
        
        /// <summary>
        /// 获得用户的该资源下的信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public Re_MyResource GetMyResource(int userId, int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlStr = "select * from Re_MyResource where UserId=@userId and ResourceId=@courseId";
                var param = new
                    {
                        userId = userId,
                        courseId = courseId
                    };
                return connection.Query<Re_MyResource>(sqlStr, param).FirstOrDefault();
            }
        }  
        
        /// <summary>
        /// 插入该用户在该资源下的默认信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <param name="timeNow"></param>
        public void insertResource(int userId, int courseId, DateTime timeNow)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string sqlwhere = @" INSERT INTO dbo.Re_MyResource (UserId, ResourceId, OpenTime, Score, Satisfaction, Practical, Attention
, IsCollection, CollectionTime, IsDownload, DownloadTime, Status, FavoriteId)
VALUES (@userId, @courseId, @timeNow, -1, -1, -1, -1
, 0, @timeNow,0, @timeNow, 0, 0)
SELECT @@IDENTITY AS ID ";
                var param = new
                {
                    userId=userId,
                    courseId = courseId,
                    timeNow=timeNow
                };
                decimal id = connection.Query<decimal>(sqlwhere, param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 判断该用户该资源下是否有信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Re_MyResource> IsExist(int userId, int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlStr = "select * from Re_MyResource where UserId=@userId and ResourceId=@courseId";
                var param = new
                    {
                        userId = userId,
                        courseId = courseId
                    };
                return connection.Query<Re_MyResource>(sqlStr, param).ToList();
            }
        }

        /// <summary>
        /// 评分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <param name="type"></param>
        /// <param name="man"></param>
        /// <param name="score"></param>
        /// <param name="manL"></param>
        /// <param name="scoreL"></param>
        /// <returns></returns>
        public bool MyTotal(int userId, int courseId, int type, int man, double score, int manL, double scoreL)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string strSql = "update Re_Resource set Score=@score , ScoreNum=@man , ";
                switch (type)
                {
                    case 0:
                        strSql += " Satisfaction=@scoreL , SatisfactionNum=@manL ";
                        break;
                    case 1:
                        strSql += " Practical=@scoreL , PracticalNum=@manL";
                        break;
                    case 2:
                        strSql += " Attention=@scoreL , AttentionNum=@manL ";
                        break;
                }
                strSql += " where ResourceID=@courseId  ";
                var param = new
                    {
                        userId = userId,
                        courseId = courseId,
                        man = man,
                        score = score,
                        manL = manL,
                        scoreL = scoreL
                    };
                return connection.Execute(strSql, param) > 0;
            }
        }

        /// <summary>
        /// 评分—更新个人信息表信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <param name="type"></param>
        /// <param name="man"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool MyScoreTo(int userId, int courseId, int type, int man, double score)
        {
             using (IDbConnection connection = OpenConnection())
             {
                 string sqlStr = "";
                 switch (type)
                 {
                     case 0://满意度
                         sqlStr = "update Re_MyResource set Satisfaction=@score  ";
                         break;
                     case 1://实用度
                         sqlStr = "update Re_MyResource set Practical=@score  ";
                         break;
                     case 2://关注度
                         sqlStr = "update Re_MyResource set Attention=@score  ";
                         break;
                 }
                 sqlStr += " where ResourceID=@courseId  and UserId=@userId";
                 var param = new
                 {
                     userId = userId,
                     courseId = courseId,
                     man = man,
                     score = score,
                 };
                 return connection.Execute(sqlStr, param) > 0;
             }
        }

        /// <summary>
        /// 获得阅读人数
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Re_MyResource> GetNum(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlStr = "select * from Re_MyResource where  ResourceId=@courseId";
                var param = new
                    {
                        courseId=courseId
                    };
                return connection.Query<Re_MyResource>(sqlStr, param).ToList();
            }
        } 

        /// <summary>
        /// 更新阅读人数
        /// </summary>
        /// <param name="num"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public bool ReaderNum(int num,int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlStr = "update Re_Resource set OpenNum =@num where ResourceID=@courseId";
                var param = new
                    {
                        num = num,
                        courseId = courseId
                    };
                return connection.Execute(sqlStr, param) > 0;
            }
        }

	}
}