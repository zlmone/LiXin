using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.User;

namespace LiXinDataAccess.User
{
    /// <summary>
    ///     Post信息(区域信息)
    /// </summary>
    public class PostDB : BaseRepository
    {
        /// <summary>
        ///     判断 部门信息(区域信息) 是否存在
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public bool Exists(string PostCode, string PostName, int PostId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere =
                    "select count(*) from Sys_Post WHERE (PostCode=@PostCode or PostName=@PostName) and IsDelete = 0";
                if (PostId > 0)
                    sqlwhere += " and PostId <> " + PostId;
                var param = new
                    {
                        PostCode,
                        PostName
                    };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }

        /// <summary>
        ///     增加一条数据
        /// </summary>
        /// <param name="model"></param>
        public void Add(Sys_Post model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Sys_Post(PostCode,PostName,ParentId,Remark,PostLevel) values(@PostCode,@PostName,@ParentId,@Remark,@PostLevel)
                SELECT @@Identity AS ID
";
                var param = new
                    {
                        model.PostCode,
                        model.PostName,
                        model.ParentId,
                        model.Remark,
                        model.PostLevel
                    };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.PostId = decimal.ToInt32(id);
            }
        }

        /// <summary>
        ///     更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool Update(Sys_Post model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Sys_Post set PostCode=@PostCode,PostName=@PostName,ParentId=@ParentId,Remark=@Remark,PostLevel=@PostLevel where PostId=@PostId

UPDATE Sys_User SET PostId=@PostId,PostCode=@PostCode,PostName=@PostName,PostLevel=@PostLevel WHERE PostId=@PostId
";
                var param = new
                    {
                        model.PostCode,
                        model.PostName,
                        model.ParentId,
                        model.Remark,
                        model.PostLevel,
                        model.PostId
                    };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        ///     删除一条数据
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public bool Delete(int postId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"update Sys_Post set IsDelete = 1 WHERE PostId = @PostId
                    update Sys_User set PostId = -1 ,PostCode = '',PostName='',PostLevel=NULL where PostId = @PostId
                ";
                var param = new
                    {
                        PostId = postId
                    };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        ///     删除父级关系
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool DeleteParentPostId(int parentId)
        {
            const string sqlwhere = "UPDATE Sys_Post SET ParentId=-1 WHERE ParentId=@parentId";
            using (IDbConnection connection = OpenConnection())
            {
                var param = new { parentId };
                return connection.Execute(sqlwhere, param) > 0;
            }
        }

        /// <summary>
        ///     得到一个对象实体
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public Sys_Post Get(int postId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format("select * from Sys_Post where IsDelete = 0 and PostId={0}", postId);
                return conn.Query<Sys_Post>(sqlwhere).FirstOrDefault();
            }
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        public List<Sys_Post> GetList(string strWhere = " 1 = 1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select * from Sys_Post where " + strWhere + " and Sys_Post.IsDelete = 0 ";
                return conn.Query<Sys_Post>(sqlwhere).ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Sys_Post> GetList(out int totalCount, string where = " 1 = 1", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY PostId DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Sys_Post where " + where + " and Sys_Post.IsDelete = 0")
                              .First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,* from Sys_Post
    where " + where + @" and Sys_Post.IsDelete = 0
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<Sys_Post>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="postId"></param>
        public void AddUserToPost(string userIds, int postId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql =
                    "UPDATE a SET a.PostId=@PostId,a.PostCode=b.PostCode,a.PostName=b.PostName,a.PostLevel=b.PostLevel FROM Sys_User a,Sys_Post b WHERE a.UserId in (" +
                    userIds + ") AND b.PostId=@PostId";
                var param = new { PostId = postId };
                connection.Execute(sql, param);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="deptId"></param>
        public void DeleteUserPost(string userIds)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "UPDATE Sys_User SET PostId=-1,PostCode='',PostName='',PostLevel=NULL WHERE  UserId in (" + userIds + ")";
                connection.Execute(sql);
            }
        }
    }
}