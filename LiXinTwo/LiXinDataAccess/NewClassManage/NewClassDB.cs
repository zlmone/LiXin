using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LiXinModels.User;
using LixinModels.NewClassManage;
using Retech.Core;
using Retech.Data;
namespace LiXinDataAccess.NewClassManage
{
    public class NewClassDB : BaseRepository
	{
	
	    /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public New_Class GetModel(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from New_Class where  Id=@Id ";
                return conn.Query<New_Class>(sqlstr, new { Id = id }).FirstOrDefault();
            }
        }
		
		/// <summary>
        /// 增加一条数据
        /// </summary>
         /// <param name="model">要添加的实体对象</param>
       public void AddModel(New_Class model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO dbo.New_Class (ClassName, PersonCount, Year, ClassNo, ClassIndex, IsDoDelete)
VALUES (@ClassName, @PersonCount, @Year, @ClassNo, @ClassIndex, @IsDoDelete)
SELECT @@IDENTITY AS ID ";
                var param = new
                                {
                                    model.ClassName,
                                    model.PersonCount,
                                    model.Year,
                                    model.ClassNo,
                                    model.ClassIndex,
                                    model.IsDoDelete
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
        public bool UpdateModel(New_Class model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @" UPDATE dbo.New_Class
SET ClassName = @ClassName,
	PersonCount = @PersonCount,
	Year = @Year,
     ClassNo=@ClassNo,
    ClassIndex=@ClassIndex,
    IsDoDelete=@IsDoDelete 
WHERE Id = @id ";
                var param = new
                                {
                                    model.Id,
                                    model.ClassName,
                                    model.PersonCount,
                                    model.Year,
                                    model.ClassNo,
                                    model.ClassIndex,
                                    model.IsDoDelete
                                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }
        
        
        /// <summary>
        /// 批量删除班级连带删除组和人数据
        /// </summary>
        /// <param name="ids">ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBatchModel(string ids)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string proSql = string.Format(@" exec proc_DeleteByNewClassId '{0}' ", ids);
                return conn.Execute(proSql) > 0;
            }
        }

        /// <summary>
        /// 点击结束分班将班级状态改为不可删除， 并将班级下的员工学号设为固定不可更改
        /// </summary>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateClassToNotDelete()
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string proSql = @"
update Sys_User set IsChangeNumberId=1 where (IsChangeNumberId is null or IsChangeNumberId<>1) and  UserId in (select distinct UserId from New_GroupUser) 
update New_Class set IsDoDelete=0 where IsDoDelete=1 ";
                return conn.Execute(proSql) >= 0;
            }
        }

        /// <summary>
        /// 根据年限获得班级列表
        /// </summary>
        public List<New_Class> GetList(string where = "")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql =string.Format(@" 
    select c.* from dbo.New_Class c 
where 1=1 {0} ",where);
                return conn.Query<New_Class>(sql).ToList();
            }
        }

        
        
        /// <summary>
        /// 获取班级信息集合
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">当前页</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序方式</param>
        /// <returns></returns>
        public List<New_Class> GetClassList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Id asc")
        {
            using (var connection = OpenConnection())
            {
                totalCount =
                   connection.Query<int>("select count(1) from New_Class where " + where).First();

                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,New_Class.*
    ,(select count(*) from New_GroupUser ngu 
          join Sys_User u on u.UserId=ngu.UserId and u.IsDelete=0 and (u.Status=0 or u.Status=1)
          where ngu.ClassId=New_Class.Id) as NowPeopleCount
 from New_Class 
    where {2} 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy, where);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<New_Class>(query, parameters).ToList();

            }
        }

        /// <summary>
        /// 获取班级信息集合
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<New_Class> GetClassList(string where = " 1 = 1 ")
        {
            using (var connection = OpenConnection())
            {

                var query = string.Format(@"select * from New_Class where {0}",  where);
                return connection.Query<New_Class>(query).ToList();

            }
        }

        /// <summary>
        /// 根据年限获得将要插入班级的班号索引
        /// </summary>
        public int GetCurrentIndex(int year)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@" 
    SELECT isnull(max(ClassIndex),-1)+1 
FROM New_Class 
WHERE  Year={0} ", year);
                return conn.Query<int>(sql).FirstOrDefault();
            }
        }


        /// <summary>
        /// 当前班组列表
        /// <param name="where">条件语句格式" and ..."</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// </summary>
        public List<New_Class> GetCurrentClassList(string where = "", int startIndex = 1, int startLength = int.MaxValue, string orderBy = " ORDER BY c.ClassIndex  ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = " ORDER BY c.ClassIndex ";
                }
                string sql = string.Format(@" 
SELECT * FROM 
(
    SELECT count(*)OVER(PARTITION BY null) AS totalCount
    , row_number() OVER({1}) AS rowNum
    ,c.*
       ,(SELECT COUNT(*) FROM New_GroupUser gu
                         join Sys_User u on u.UserId=gu.UserId and u.IsDelete=0 and (u.Status=0 or u.Status=1)
        WHERE gu.ClassId=c.Id) AS NowPeopleCount 
       ,(SELECT COUNT(*) FROM New_Group g WHERE g.ClassId=c.Id) AS  NowGroupCount
       ,(SELECT COUNT(*) FROM  Sys_User u1 where u1.IsDelete=0 and (u1.Status=0 or u1.Status=1) and u1.Sex=0 and u1.UserId in (SELECT UserId FROM New_GroupUser gu WHERE gu.ClassId=c.Id) ) AS NowManCount 
       ,(SELECT COUNT(*) FROM  Sys_User u1 where u1.IsDelete=0 and (u1.Status=0 or u1.Status=1) and  u1.Sex=1 and u1.UserId in (SELECT UserId FROM New_GroupUser gu WHERE gu.ClassId=c.Id) ) AS NowWomanCount
       ,(SELECT COUNT(*) FROM  Sys_User u1 where u1.IsDelete=0 and (u1.Status=0 or u1.Status=1) and  u1.IsInternExp=1 and u1.UserId in (SELECT UserId FROM New_GroupUser gu WHERE gu.ClassId=c.Id) ) AS NowInternExpCount
 FROM New_Class c 
 WHERE 1=1  {0}
) AS result 
WHERE result.rowNum BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex ", where, orderBy);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<New_Class>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 获得指定班级人员列表
        /// <param name="where">条件语句格式" and ..."</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// </summary>
        public List<Sys_User> GetClassPersonsList(int classId,string where = "", int startIndex = 1, int startLength = int.MaxValue, string orderBy = " ORDER BY u.NumberID ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = " ORDER BY u.NumberID ";
                }
                string sql = string.Format(@" 
SELECT * FROM 
(
    SELECT count(*)OVER(PARTITION BY null) AS totalCount
    , row_number() OVER({1}) AS rowNum
    ,u.*,g.GroupName,gu.GroupId,gu.ClassId
    FROM Sys_User u 
    left join New_GroupUser gu on gu.UserId=u.UserId 
    left join New_Group g on g.Id=gu.GroupId 
    WHERE  (u.Status=0 or u.Status=1) and u.IsDelete=0 
          AND u.UserId  IN (SELECT DISTINCT ngu.UserId FROM New_GroupUser ngu where ngu.ClassId={2})
          {0}
) AS result 
WHERE result.rowNum BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex ", where, orderBy, classId);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_User>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 根据年限获得已分班、未分班人数，已分班级数
        /// </summary>
        public Dictionary<string, int> GetClassAndPersonCount(int year)
        {
            Dictionary<string,int> countDict = new Dictionary<string, int>();
            countDict.Add("splitClassCount",0);//已分班级数
            countDict.Add("newUserCount", 0);//新进员工数
            countDict.Add("splitUserCount", 0);//已分班人数
            using (IDbConnection conn = OpenConnection())
            {
                string splitClassCountSql = string.Format(@" 
    SELECT count(*) FROM New_Class
WHERE Year={0} ", year);
                countDict["splitClassCount"] = conn.Query<int>(splitClassCountSql).FirstOrDefault();

                string newUserCountSql = string.Format(@" 
    SELECT count(*) FROM Sys_User u
WHERE u.IsNew=1 and u.Status=0 and u.IsDelete=0  ");
                countDict["newUserCount"] = conn.Query<int>(newUserCountSql).FirstOrDefault();

                string splitUserCountSql = string.Format(@" 
    SELECT count(*) FROM New_GroupUser gu 
join Sys_User u on u.UserId=gu.UserId and u.IsDelete=0 and (u.Status=0 or u.Status=1)
JOIN New_Class c ON gu.ClassId=c.Id AND c.Year={0}  ", year);
                countDict["splitUserCount"] = conn.Query<int>(splitUserCountSql).FirstOrDefault();
            }

            return countDict;
        }
	}
}