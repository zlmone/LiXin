using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using LiXinModels.NewClassManage;
using LiXinModels.User;
using LixinModels.NewClassManage;
using Retech.Core;
using Retech.Data;
namespace LiXinDataAccess.NewClassManage
{
    public class NewGroupUserDB : BaseRepository
    {

        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public New_GroupUser GetModel(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from New_GroupUser where  Id=@Id ";
                return conn.Query<New_GroupUser>(sqlstr, new { Id = id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据ClassID和UserId获取model(主要用于获取人员所在组ID)
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="userId">人员ID</param>
        /// <returns></returns>
        public New_GroupUser GetModelByClassAndUser(int classId, int userId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from New_GroupUser where  ClassId=@ClassId and UserId=@UserId ";
                return conn.Query<New_GroupUser>(sqlstr, new { ClassId = classId, UserId = userId }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据UserId获取model(主要用于获取人员所在班ID)
        /// </summary>
        /// <param name="userId">人员ID</param>
        /// <returns></returns>
        public New_GroupUser GetModelByUserID(int userId)
        {
            using (var conn = OpenConnection())
            {
                const string sqlstr = "select * from New_GroupUser as a,New_Class as b where a.ClassId=b.Id and b.Year=@year and a.UserId=@UserId ";
                return conn.Query<New_GroupUser>(sqlstr, new { UserId = userId, year = DateTime.Now.Year }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">要添加的实体对象</param>
        public void AddModel(New_GroupUser model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @"INSERT INTO dbo.New_GroupUser (ClassId, GroupId, UserId, NumberID)
VALUES (@ClassId, @GroupId, @UserId, @NumberID)
SELECT @@IDENTITY AS ID ";
                var param = new
                                {
                                    model.ClassId,
                                    model.GroupId,
                                    model.UserId,
                                    model.NumberID
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
        public bool UpdateModel(New_GroupUser model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @" UPDATE dbo.New_GroupUser
SET ClassId = @ClassId,
	GroupId = @GroupId,
	UserId = @UserId,
	NumberID = @NumberID
WHERE Id = @id ";
                var param = new
                                {
                                    model.Id,
                                    model.ClassId,
                                    model.GroupId,
                                    model.UserId,
                                    model.NumberID
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
                string delSql = string.Format(@" delete from New_GroupUser where Id in ({0}) ", ids);
                int result = conn.Execute(delSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据班级Id和人员Id集合批量删除数据
        /// </summary>
        /// <param name="userIds">学员ID集合格式为: 1,2,3,4</param>
        /// <param name="classId">班级ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBatchByUserAndClass(string userIds, int classId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string delSql = string.Format(@" delete from New_GroupUser where UserId in ({0}) and ClassId={1} ",
                                              userIds, classId);
                int result = conn.Execute(delSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据班级Id和人员Id集合批量调整人员班组
        /// </summary>
        /// <param name="userIds">学员ID集合格式为: 1,2,3,4</param>
        /// <param name="classId">老班级ID</param>
        /// <param name="newClassId">新班级ID</param>
        /// <param name="newGroupId">新组ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool BatchChangeClassGroup(string userIds, int classId, int newClassId, int newGroupId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string delSql =
                    string.Format(
                        @" update New_GroupUser set ClassId={0},GroupId={1} where UserId in ({2}) and ClassId={3} "
                        , newClassId, newGroupId, userIds, classId);
                int result = conn.Execute(delSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据班级Id和人员Id集合批量调整人员班组，并且修改学号
        /// </summary>
        /// <param name="userIds">学员ID集合格式为: 1,2,3,4</param>
        /// <param name="classId">老班级ID</param>
        /// <param name="newClassId">新班级ID</param>
        /// <param name="newGroupId">新组ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool BatchChangeClassGroup(string userIds, int classId, int newClassId, int newGroupId,string NumberID)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string delSql =
                    string.Format(
                        @" update New_GroupUser set ClassId={0},GroupId={1},NumberID='{4}' where UserId in ({2}) and ClassId={3} "
                        , newClassId, newGroupId, userIds, classId, NumberID);
                int result = conn.Execute(delSql);
                return result > 0;
            }
        }

        /// <summary>
        /// 获得当前未分班人员列表
        /// <param name="where">条件语句格式" and ..."</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// </summary>
        public List<Sys_User> GetList(string where = "", int startIndex = 1, int startLength = int.MaxValue,
                                      string orderBy = " ORDER BY u.NumberID,u.Realname ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = " ORDER BY u.NumberID,u.Realname ";
                }
                string sql =
                    string.Format(
                        @" 
SELECT * FROM 
(
    SELECT count(*)OVER(PARTITION BY null) AS totalCount
    , row_number() OVER({1}) AS rowNum
    ,u.*
    FROM Sys_User u 
    WHERE  u.Status=0 and u.IsDelete=0 
          AND u.UserId NOT IN (SELECT DISTINCT ngu.UserId FROM New_GroupUser ngu)
          {0}
) AS result 
WHERE result.rowNum BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex ",
                        where, orderBy);
                var param = new
                                {
                                    startIndex = startIndex,
                                    startLength = startLength
                                };
                return conn.Query<Sys_User>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得当前未分班人员总数
        /// </summary>
        public int GetCountList()
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql =
                    @" 
    SELECT count(*)
    FROM Sys_User u 
    WHERE u.IsNew=1 and u.Status=0 and u.IsDelete=0 
          AND u.UserId NOT IN (SELECT DISTINCT ngu.UserId FROM New_GroupUser ngu)";
                return conn.Query<int>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据条件获取指定班组学员学号信息集合
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<string> GetNumberList(string where = "")
        {
            using (var connection = OpenConnection())
            {
                var query =
                    string.Format(@" SELECT u.NumberID FROM  Sys_User u  
WHERE 1=1 {0}
ORDER BY u.NumberID DESC ",
                                  where);
                return connection.Query<string>(query).ToList();

            }
        }

        /// <summary>
        /// 批量添加班组人员
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="groupId">组ID</param>
        /// <param name="userIds">人员ID集合：1,2,3</param>
        public void BatchAddNewGroupUser(int classId, int groupId, string userIds)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere =
                    @"
                        DECLARE @UserId int;--获取单个的id
                        DECLARE @nextId int;--单个的id的下标
                        SET @nextId=1;
                        DECLARE @LengthId int;--Id的集合中共有多少个id

                        SELECT @LengthId=dbo.GetSplitLength(@userIds,',');
                        while @nextId<=@LengthId
                          begin
                           --当前id
			                SET @UserId = left(dbo.GetSplitOfIndex(@userIds,',',@nextId), 100);           			  
                              if(@UserId<>'' and @UserId<>'0' and (select count(*) from New_GroupUser where UserId=@UserId)=0 )
			                   begin
                                INSERT INTO New_GroupUser 
                               (ClassId,GroupId,UserId)
			                       VALUES(@ClassId,@GroupId,@UserId)
			                   end
                            SET @nextId=@nextId+1;
                          end  ";
                var param = new
                                {
                                    ClassId = classId,
                                    GroupId = groupId,
                                    userIds = userIds
                                };
                conn.Execute(sqlwhere, param);
            }
        }

        /// <summary>
        /// 批量添加班组人员
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <param name="groupId">组ID</param>
        /// <param name="userIds">人员ID集合：1,2,3</param>
        public void BatchAddNewGroupUser(int classId, int groupId, int userId, string NumberID)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere =
                    @"if((select count(*) from New_GroupUser where UserId=@UserId)=0 )
			                   begin
                                INSERT INTO New_GroupUser 
                               (ClassId,GroupId,UserId,NumberID)
			                       VALUES(@ClassId,@GroupId,@UserId,@NumberID)
			                   end
                      ";
                var param = new
                {
                    ClassId = classId,
                    GroupId = groupId,
                    UserId = userId,
                    NumberID = NumberID
                };
                conn.Execute(sqlwhere, param);
            }
        }

        /// <summary>
        /// 查询指定班下的相关人员
        /// <param name="where">条件语句格式" and ..."</param>
        /// </summary>
        public List<New_GroupUser> GetList(string where = "")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format("SELECT New_GroupUser.* FROM New_GroupUser,New_Class, Sys_User WHERE NEW_GROUPUSER.ClassId=New_Class.Id AND sys_user.UserId=New_GroupUser.UserId AND sys_user.Status<2 AND {0}", where);
                return conn.Query<New_GroupUser>(sql).ToList();
            }
        }

        /// <summary>
        /// 获得当前未分班人员列表
        /// <param name="where">条件语句格式" and ..."</param>
        /// </summary>
        public List<ClassUser> GetClassUserList(string where = "", string ordersql = " order by u.UserId")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select  u.UserId,u.RealName,u.Sex,u.DeptId,u.DeptName,u.GraduateSchool,u.IsInternExp,u.IsOurIntern,u.InternDept,a.GroupId,a.NumberID,u.Major
from New_GroupUser a LEFT JOIN Sys_User u on a.UserId=u.UserId
where u.Status=0 and u.IsDelete=0 {0}", where);
                return conn.Query<ClassUser>(sql).ToList();
            }
        }

        /// <summary>
        /// 查询人所在的组和班级
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public New_GroupUser GetClassInfoByUserID(int userID)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT *,ng.GroupName,ncs.ClassName FROM New_GroupUser    ngu
LEFT JOIN New_Group ng on ngu.GroupId=ng.Id
LEFT JOIN New_Class ncs on ngu.ClassId=ncs.Id
WHERE UserId=" + userID;
                return conn.Query<New_GroupUser>(sql).FirstOrDefault();
            }
        }
    }
}