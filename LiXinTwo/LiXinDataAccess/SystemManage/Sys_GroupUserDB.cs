using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using System.Data;
using LiXinModels.SystemManage;

namespace LiXinDataAccess.SystemManage
{
    public class Sys_GroupUserDB : BaseRepository
    {

        /// <summary>
        /// 批量增加
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="userIDs">用户ID集合</param>
        /// <param name="model">实体</param>
        public bool AddGroupUser(int groupid, string userIDs)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "";
                string[] userlist = userIDs.Split(',');
                for (int i = 0; i < userlist.Count(); i++)
                {
                    sqlwhere += "INSERT INTO Sys_GroupUser(GroupId,UserId) VALUES (" + groupid + "," + Convert.ToInt32(userlist[i]) + ")";
                }
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <param name="userIDs">用户ID集合</param>
        /// <returns>操作状态</returns>
        public bool DeleteGroupUser(int groupId, string userIDs)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"DELETE FROM Sys_GroupUser where GroupId=@GroupId and UserId in (" + userIDs + ")";
                var param = new { GroupId = groupId };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="groupId">群组ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteGroupALLUser(int groupId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"DELETE FROM Sys_GroupUser where GroupId=@GroupId";
                var param = new { GroupId = groupId };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Sys_GroupUser> GetGroupUserList(int groupId, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT count(*)OVER(PARTITION BY null) AS totalcount, row_number() OVER(ORDER BY team.GroupId) AS rowNum,team.*,t2.DeptName FROM (SELECT t0.GroupId,t0.UserId,t1.JobNum,t1.Realname,t1.Sex,t1.TrainGrade,t1.PayGrade,t1.GroupMobileNum,t1.Telephone,t1.MobileNum,t1.Email,t1.DeptId,t1.DeptPath FROM Sys_GroupUser AS t0,Sys_User AS t1 WHERE t0.UserId=t1.UserId AND t0.GroupId=@GroupId) AS team LEFT JOIN Sys_Department AS t2 on team.DeptId=t2.DepartmentId WHERE " + where + " ) AS result WHERE  result.rowNum BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    GroupId = groupId
                };
                return conn.Query<Sys_GroupUser>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<int> GetGroupUserID(int groupId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT UserId FROM dbo.Sys_GroupUser WHERE GroupId=@GroupId";
                return conn.Query<int>(sql, new { GroupId = groupId }).ToList();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Sys_GroupUser> GetUser(string userIDs, int startIndex = 1, int startLength = int.MaxValue)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT count(*)OVER(PARTITION BY null) AS totalcount, row_number() OVER(ORDER BY team.UserId) AS rowNum,team.*,t0.DeptName FROM (SELECT UserId,JobNum,Realname,Sex,TrainGrade,PayGrade,GroupMobileNum,Telephone,Email,DeptId,DeptPath FROM Sys_User WHERE UserId in (" + userIDs + ")) AS team LEFT JOIN Sys_Department AS t0 on team.DeptId=t0.DepartmentId ) AS result WHERE  result.rowNum BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_GroupUser>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得某人所在的群组
        /// </summary>
        public List<Sys_GroupUser> GetGroupList(int userID, int isdeleteflag = 1)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format("select b.* from Sys_Group as a,Sys_GroupUser as b where  a.GroupId=b.GroupId and b.UserId={0} {1}", userID, (isdeleteflag == 1 ? " and a.IsDelete=0 " : " "));
                return conn.Query<Sys_GroupUser>(sql).ToList();
            }
        }
    }
}
