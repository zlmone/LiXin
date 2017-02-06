using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LixinModels.ReResourceManage;
using Retech.Core;
using Retech.Data;
namespace LiXinDataAccess.ReResourceManage
{
    public class ReResourceDB : BaseRepository
    {

        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Re_Resource GetModel(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from Re_Resource where  ResourceID=@ResourceID ";
                return conn.Query<Re_Resource>(sqlstr, new { ResourceID = id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">要添加的实体对象</param>
        public void AddModel(Re_Resource model)
        {
            model.ResourceName = model.ResourceName.Trim();
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @" INSERT INTO dbo.Re_Resource (ResourceName, ResourceDesc, ResourceTypeID, Format, Suffix, Tag, URL
, ThumbnailURL, ResultURL, isOpen, isDownload, UpTime, UpUserID, LastTime, OpenNum, DownNum
, CollectionNum, Score, ScoreNum, Satisfaction, SatisfactionNum, Practical, PracticalNum
, Attention, AttentionNum, IsRecommend, Status, OpenFlag, OpenGroupObject, OpenDepartObject)
VALUES (@ResourceName, @ResourceDesc, @ResourceTypeID, @Format, @Suffix, @Tag, @URL
, @ThumbnailURL, @ResultURL, @isOpen, @isDownload, @UpTime, @UpUserID, @LastTime, @OpenNum, @DownNum
, @CollectionNum, @Score, @ScoreNum, @Satisfaction, @SatisfactionNum, @Practical, @PracticalNum
, @Attention, @AttentionNum, @IsRecommend, @Status, @OpenFlag, @OpenGroupObject, @OpenDepartObject)
SELECT @@IDENTITY AS ID ";
                var param = new
                                {
                                    model.ResourceName,
                                    model.ResourceDesc,
                                    model.ResourceTypeID,
                                    model.Format,
                                    model.Suffix,
                                    model.Tag,
                                    model.URL,
                                    model.ThumbnailURL,
                                    model.ResultURL,
                                    model.isOpen,
                                    model.isDownload,
                                    model.UpTime,
                                    model.UpUserID,
                                    model.LastTime,
                                    model.OpenNum,
                                    model.DownNum,
                                    model.CollectionNum,
                                    model.Score,
                                    model.ScoreNum,
                                    model.Satisfaction,
                                    model.SatisfactionNum,
                                    model.Practical,
                                    model.PracticalNum,
                                    model.Attention,
                                    model.AttentionNum,
                                    model.IsRecommend,
                                    model.Status,
                                    model.OpenFlag,
                                    OpenGroupObject = string.IsNullOrEmpty(model.OpenGroupObject) ? "" : model.OpenGroupObject,
                                    OpenDepartObject = string.IsNullOrEmpty(model.OpenDepartObject)? "" : model.OpenDepartObject
                                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.ResourceID = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateModel(Re_Resource model)
        {
            model.ResourceName = model.ResourceName.Trim();
            model.ResourceDesc = model.ResourceDesc ?? "";
            model.OpenGroupObject = model.OpenGroupObject ?? "";
            model.OpenDepartObject = model.OpenDepartObject ?? "";
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @" UPDATE dbo.Re_Resource
SET ResourceName = @ResourceName,
	ResourceDesc = @ResourceDesc,
	ResourceTypeID = @ResourceTypeID,
	Format = @Format,
	Suffix = @Suffix,
	Tag = @Tag,
	URL = @URL,
	ThumbnailURL = @ThumbnailURL,
	ResultURL = @ResultURL,
	isOpen = @isOpen,
	isDownload = @isDownload,
	UpTime = @UpTime,
	UpUserID = @UpUserID,
	LastTime = @LastTime,
	OpenNum = @OpenNum,
	DownNum = @DownNum,
	CollectionNum = @CollectionNum,
	Score = @Score,
	ScoreNum = @ScoreNum,
	Satisfaction = @Satisfaction,
	SatisfactionNum = @SatisfactionNum,
	Practical = @Practical,
	PracticalNum = @PracticalNum,
	Attention = @Attention,
	AttentionNum = @AttentionNum,
	IsRecommend = @IsRecommend,
	Status = @Status,
	OpenFlag = @OpenFlag,
	OpenGroupObject = @OpenGroupObject,
	OpenDepartObject = @OpenDepartObject
WHERE ResourceID = @ResourceID ";
                var param = new
                                {
                                    model.ResourceID,
                                    model.ResourceName,
                                    model.ResourceDesc,
                                    model.ResourceTypeID,
                                    model.Format,
                                    model.Suffix,
                                    model.Tag,
                                    model.URL,
                                    model.ThumbnailURL,
                                    model.ResultURL,
                                    model.isOpen,
                                    model.isDownload,
                                    model.UpTime,
                                    model.UpUserID,
                                    model.LastTime,
                                    model.OpenNum,
                                    model.DownNum,
                                    model.CollectionNum,
                                    model.Score,
                                    model.ScoreNum,
                                    model.Satisfaction,
                                    model.SatisfactionNum,
                                    model.Practical,
                                    model.PracticalNum,
                                    model.Attention,
                                    model.AttentionNum,
                                    model.IsRecommend,
                                    model.Status,
                                    model.OpenFlag,
                                    model.OpenGroupObject,
                                    model.OpenDepartObject
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
                string delSql = string.Format(@" UPDATE dbo.Re_Resource set Status = 1 where ResourceID in ({0}) ", ids);
                int result = conn.Execute(delSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 获取所有资源
        /// </summary>
        /// <returns></returns>
        public List<Re_Resource> GetAllResource()
        {
            using (IDbConnection connection = OpenConnection())
            {
                string strSql = "select * from Re_Resource where Status=0";
                return connection.Query<Re_Resource>(strSql).ToList();
            }
        }

        /// <summary>
        /// 获取知识资源列表
        /// <param name="where">条件语句格式" and ..."</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// </summary>
        public List<Re_Resource> GetResourceList(string where = "", int startIndex = 1, int startLength = int.MaxValue, string orderBy = "ORDER BY LastTime desc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = "ORDER BY LastTime desc";
                }
                string sql = string.Format(@" 
SELECT * FROM 
(
    SELECT count(*) OVER(PARTITION BY null) AS totalCount
    , row_number() OVER({1}) AS rowNum ,yxt_t1.* 
    from (
            select yxt_t0.* 
            from (
                    SELECT  rr.*,u.Realname as UpUserName 
                    FROM Re_Resource rr 
                    left join Sys_User u on rr.UpUserID=u.UserId 
                    where rr.Status=0    {0} 
                ) as yxt_t0 
          ) as yxt_t1 
) AS result 
WHERE result.rowNum BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex ", where, orderBy);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Re_Resource>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 根据资源名称判断资源是否存在
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <param name="sufFix">后缀名</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="statu">资源状态0-正常，1-删除</param>
        /// <returns></returns>
        public bool Exists(string resourceName, string sufFix, int resourceId = 0, int statu = 0)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sql =
                    " SELECT COUNT(1) FROM Re_Resource WHERE ResourceName=@ResourceName and Suffix=@Suffix and Status=@Status AND ResourceID<>@ResourceID ";
                var param = new
                {
                    ResourceName = resourceName,
                    Suffix = sufFix,
                    ResourceID = resourceId,
                    Status = statu
                };
                int count = conn.Query<int>(sql, param).First();
                return count > 0;
            }
        }

    }
}