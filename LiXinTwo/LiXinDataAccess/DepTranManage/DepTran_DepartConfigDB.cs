using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.SystemManage;
using LiXinModels.DepTranManage;


namespace LiXinDataAccess.DepTranManage
{
    public class DepTran_DepartConfigDB : BaseRepository
    {
        #region 部门设置
        /// <summary>
        /// 判断 部门组信息 是否存在
        /// </summary>
        /// <param name="departName">部门组名</param>
        /// <param name="id">部门组ID</param>
        /// <returns></returns>
        public bool Exists(string departName, int id)
        {
            using (var conn = OpenConnection())
            {
                var sqlwhere = "select count(1) from DepTran_DepartLeaderConfig WHERE IsDelete = 0 and DepartSetName = @departName  ";
                if (id > 0)
                    sqlwhere += " and Id <> @Id";
                var param = new {departName, id };
                var count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count < 1;
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        public void AddDepartSetting(DepTranDepartSetting model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO DepTran_DepartLeaderConfig(DepartSetName,MainUserIDs,Memo,CreateTime,IsDelete) VALUES (@DepartSetName,@MainUserIDs,@Memo,@CreateTime,@IsDelete)SELECT @@IDENTITY AS ID";
                var param = new
                {
                    model.DepartSetName,
                    model.MainUserIDs,
                    model.Memo,
                    model.CreateTime,
                    model.IsDelete
                };
                var id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateDepartSetting(DepTranDepartSetting model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update DepTran_DepartLeaderConfig SET DepartSetName = @DepartSetName,MainUserIDs = @MainUserIDs,Memo=@Memo,CreateTime = @CreateTime,IsDelete = @IsDelete WHERE id=@id";
                var param = new
                {
                    model.DepartSetName,
                    model.MainUserIDs,
                    model.Memo,
                    model.CreateTime,
                    model.IsDelete,
                    model.Id
                };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">群组ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteDepartSetting(string ids)
        {
            using (var conn = OpenConnection())
            {
                var sqlwhere = @"update DepTran_DepartLeaderConfig set IsDelete = 1 where id in(" + ids + ")";
                var result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        public DepTranDepartSetting GetDepartSetting(int id)
        {
            using (var conn = OpenConnection())
            {
                const string sqlstr = @"SELECT sl.* FROM DepTran_DepartLeaderConfig sl WHERE id=@id";
                return conn.Query<DepTranDepartSetting>(sqlstr, new { id }).FirstOrDefault();
            }
        }
        /// <summary>
        /// 根据UserID获得数据
        /// </summary>
        public List<DepTranDepartSetting> GetDepartSettingByUserId(int userID)
        {
            using (var conn = OpenConnection())
            {
                string sqlstr = @"SELECT * FROM (SELECT *,dbo.CGF_FN_Search(MainUserIDs,'"+userID+"',',') AS match FROM DepTran_DepartLeaderConfig WHERE IsDelete=0) AS temp WHERE temp.match>0";
                return conn.Query<DepTranDepartSetting>(sqlstr).ToList();
            }
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<DepTranDepartSetting> GetDepartSettingList(string where = " 1 = 1 ")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"select * from DepTran_DepartLeaderConfig where " + where;
                return conn.Query<DepTranDepartSetting>(sql).ToList();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<DepTranDepartSetting> GetAllDepartSettingList(int startIndex = 1, int startLength = int.MaxValue, string where = " 1 = 1 ", string jsRenderSortField = "ORDER BY t0.Id")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"
SELECT * FROM (
    SELECT count(*)OVER(PARTITION BY null) AS totalcount, 
    row_number() OVER("+jsRenderSortField+@") AS rowNum, --ORDER BY t0.Id
    t0.*,
    dbo.f_GetTeacherName(t0.MainUserIDs) as Realname,
    (select count(*) from DepTran_DepartUser where DepTran_DepartUser.DepartSetId=t0.Id) AS UNumber 
    FROM DepTran_DepartLeaderConfig AS t0
    WHERE t0.IsDelete=0 and " + where + @"
    ) as result 
WHERE result.rowNum BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex";
                var param = new
                {
                    startIndex, startLength
                };
                return conn.Query<DepTranDepartSetting>(sql, param).ToList();
            }
        }
  
#endregion

        #region 部门设置人员相关

        /// <summary>
        /// 批量增加
        /// </summary>
        /// <param name="departSetID">部门ID</param>
        /// <param name="userIDs">用户ID集合</param>
        public bool AddDepartSettingUser(int departSetID, string userIDs)
        {
            using (var conn = OpenConnection())
            {
                var sqlwhere = "";
                var userlist = userIDs.Split(',');
                for (var i = 0; i < userlist.Count(); i++)
                {
                    sqlwhere += "INSERT INTO DepTran_DepartUser(DepartSetId,UserId) VALUES (" + departSetID + "," + Convert.ToInt32(userlist[i]) + ")";
                }
                var result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="departSetID">部门设置ID</param>
        /// <param name="userIDs">用户ID集合</param>
        /// <returns>操作状态</returns>
        public bool DeleteDepartSettingUser(int departSetID, string userIDs)
        {
            using (var conn = OpenConnection())
            {
                var sqlwhere = @"DELETE FROM DepTran_DepartUser where DepartSetId=@DepartSetId and UserId in (" + userIDs + ")";
                var param = new { DepartSetId = departSetID };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="departSetID">部门设置ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteDepartSettingAllUser(int departSetID)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere = @"DELETE FROM DepTran_DepartUser where DepartSetId=@DepartSetId";
                var param = new { DepartSetId = departSetID };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Sys_GroupUser> GetDepartSettingUserList(int departSetID, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT * FROM (SELECT count(*)OVER(PARTITION BY null) AS totalcount, row_number() OVER(ORDER BY team.GroupId) AS rowNum,team.*,t2.DeptName FROM (SELECT t0.DepartSetId as GroupId,t0.UserId,t1.JobNum,t1.Realname,t1.Sex,t1.TrainGrade,t1.PayGrade,t1.GroupMobileNum,t1.Telephone,t1.MobileNum,t1.Email,t1.DeptId,t1.DeptPath FROM DepTran_DepartUser AS t0,Sys_User AS t1 WHERE t0.UserId=t1.UserId AND t0.DepartSetId=@DepartSetId) AS team LEFT JOIN Sys_Department AS t2 on team.DeptId=t2.DepartmentId WHERE " + where + " ) AS result WHERE  result.rowNum BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex, startLength,
                    DepartSetId = departSetID
                };
                return conn.Query<Sys_GroupUser>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<int> GetDepartSettingUserID(int departSetID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT UserId FROM dbo.DepTran_DepartUser {0} ", departSetID == 0 ? "" : "WHERE DepartSetId=@DepartSetId");
                return conn.Query<int>(sql, new { DepartSetId = departSetID }).ToList();
            }
        }

        /// <summary>
        /// 根据人员ID查找部门ID
        /// </summary>
        public int GetDepartSettingDepartSetID(int UserId)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT DepartSetId FROM dbo.DepTran_DepartUser {0} ", UserId == 0 ? "" : "WHERE UserId=@UserId");
                return conn.Query<int>(sql, new { UserId = UserId }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Sys_GroupUser> GetUser(string userIDs, int startIndex = 1, int startLength = int.MaxValue)
        {
            using (var conn = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT count(*)OVER(PARTITION BY null) AS totalcount, row_number() OVER(ORDER BY team.UserId) AS rowNum,team.*,t0.DeptName FROM (SELECT UserId,JobNum,Realname,Sex,TrainGrade,PayGrade,GroupMobileNum,MobileNum,Telephone,Email,DeptId,DeptPath FROM Sys_User WHERE UserId in (" + userIDs + ")) AS team LEFT JOIN Sys_Department AS t0 on team.DeptId=t0.DepartmentId ) AS result WHERE  result.rowNum BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex";
                var param = new
                {
                    startIndex, startLength
                };
                return conn.Query<Sys_GroupUser>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Sys_GroupUser> GetDepartCourseLimitNumber(int userID,int courseid)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"select b.UserId,a.LimitNumber from DepTran_CourseOpen a,DepTran_DepartUser b
where a.DepartId=b.DepartSetId and a.OpenFlag=1 and b.UserId=@userID  and a.courseid=@courseid";
                var param = new
                {
                    userID,
                    courseid
                };
                return conn.Query<Sys_GroupUser>(sql, param).ToList();
            }
        }

        #endregion
    }
}
