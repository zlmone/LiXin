using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.User;

namespace LiXinDataAccess
{
    public class Sys_TrainGradeDB : BaseRepository
    {
        #region 培训级别维护
        /// <summary>
        /// 获取基本信息
        /// </summary>
        /// <param name="Yearfirst">今年的开始时间</param>
        /// <param name="YearEnd">今年的结束时间</param>
        /// <param name="StartTime">限定的开始时间</param>
        /// <param name="EndTime">限定的结束时间</param>
        /// <returns></returns>
        public List<Sys_TrainGrade> GetUserTrain(int year, int userID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string ordersql = "ORDER BY su.DeptPath asc,su.TrainGrade asc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"
WITH TrainG AS
(
   SELECT userID,Status,SubmitFlag FROM Sys_TrainGrade 
   WHERE  year(UpdateTime)=@year
)

SELECT * FROM (
SELECT row_number()OVER({1}) number,count(*)OVER(PARTITION BY null) totalcount,
su.UserId,su.Realname,su.Sex,su.JobNum,su.DeptName,su.PayGrade,su.TrainGrade,TrainMaster,su.DeptPath
,IsApply=(SELECT  count(*) from TrainG  WHERE  UserID=su.UserId AND SubmitFlag=1)--是否可以再次提交申请
,UpdateCount=(SELECT COUNT(*) FROM TrainG WHERE UserID=su.UserId AND SubmitFlag=0)--期限内是否有维护的
,ApprovalStatus=(SELECT  count(*) from TrainG  WHERE  UserID=su.UserId AND SubmitFlag=1 AND  Status=1)--是否提交
FROM Sys_User su
WHERE IsDelete=0 AND IsTeacher<2 and  1=1
AND su.LeaderID=(SELECT JobNum FROM Sys_User WHERE UserId=@userID)
and su.UserId!=@userID {0}
) result
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where, ordersql);
                var param = new
               {
                   startIndex = startIndex,
                   startLength = startLength,
                   year = year,
                   userID = userID
               };
                return conn.Query<Sys_TrainGrade>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 新增维护信息
        /// </summary>
        /// <param name="model"></param>
        public void InsertTrainGrade(Sys_TrainGrade model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sql = @"INSERT INTO dbo.Sys_TrainGrade
	                            (
	                            UserID,
	                            CreateUserID,
	                            OldGrade,
	                            NewGrade,
	                            Reason,
	                            UpdateTime,
	                            Status,
                                SubmitFlag
	                            )
                            VALUES 
	                            (
	                            @userid,
	                            @createuserid,
	                            @oldgrade,
	                            @newgrade,
	                            @reason,
	                            getdate(),
	                            @status,
                                @submitFlag
	                            );SELECT @@IDENTITY AS ID";

                var param = new
                {
                    userid = model.UserID,
                    createuserid = model.CreateUserID,
                    oldgrade = model.OldGrade,
                    newgrade = model.NewGrade,
                    reason = model.Reason,
                    status = model.Status,
                    submitFlag = model.SubmitFlag
                };

                dynamic list = conn.Query<dynamic>(sql, param).FirstOrDefault();
                model.ID = decimal.ToInt32(list.ID);
            }

        }

        /// <summary>
        /// 获取批量修改的人员信息
        /// </summary>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        public List<Sys_User> GetUpdateUser(string userIDs)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT UserId,trainGrade, Realname FROM Sys_User
WHERE UserId IN({0})", userIDs);
                return conn.Query<Sys_User>(sql).ToList();
            }
        }

        public void UpdateTrainGrade(int userID, string Grade)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"UPDATE Sys_User
set TrainGrade='{1}'
WHERE UserId={0}", userID, Grade);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 批量变更的时候维护
        /// </summary>
        /// <param name="id"></param>
        public void UpdateTrainGradeBytrain(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"DECLARE @userID INT,@taingrade NVARCHAR(50)
SELECT @userID=userID, @taingrade=NewGrade FROM Sys_TrainGrade
WHERE ID={0}

UPDATE Sys_User
SET TrainGrade= @taingrade
WHERE UserId=@userID", id);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 是否是培训负责人
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="query"></param>
        public void UpdateTrainMaster(int userID, string query)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"UPDATE Sys_User
                            SET {1}
                            WHERE UserId={0}", userID, query);
                conn.Execute(sql);
            }
        }

        public void UpdateAllTrainMaster(int userID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"UPDATE Sys_User
                                SET TrainMaster=0
                                WHERE LeaderID IN (SELECT JobNum FROM Sys_User WHERE UserId={0})
                                AND UserId!={0}", userID);
                conn.Execute(sql);
            }
        }
        #endregion

        #region 培训级别变更
        /// <summary>
        /// 培训级别变更
        /// </summary>
        /// <returns></returns>
        public List<Sys_TrainGrade> GetTarinUpdate(string Yearfirst, string YearEnd, string StartTime, string EndTime, string where, int startIndex = 1, int startLength = int.MaxValue)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT row_number()OVER(ORDER BY su.UserID asc) number,count(*)OVER(PARTITION BY null) totalcount
,su.UserId,su.Realname,su.Sex,su.JobNum,su.DeptName,su.PayGrade,su.TrainGrade
,st.ID,st.CreateUserID,st.OldGrade,st.NewGrade,st.Reason,st.UpdateTime,st.Status 
,dbo.GetRealNameByUserID(st.CreateUserID) AS CreateName
FROM  Sys_TrainGrade st
LEFT JOIN Sys_User su ON st.UserID=su.UserId
where ((UpdateTime<=@StartTime  AND @Yearfirst<=UpdateTime) or  (UpdateTime<=@YearEnd AND @EndTime<=UpdateTime))
AND su.IsDelete=0 AND su.IsTeacher<2 and {0}
and SubmitFlag=1
) result
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where);
                var param = new
                {
                    Yearfirst = Yearfirst,
                    YearEnd = YearEnd,
                    StartTime = StartTime,
                    EndTime = EndTime,
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_TrainGrade>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Sys_TrainGrade> GetSingle(int ID)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM Sys_TrainGrade
WHERE ID={0}", ID);
                return conn.Query<Sys_TrainGrade>(sql).ToList();
            }
        }

        /// <summary>
        /// 更新培训级别
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="newGrade"></param>
        public void UpdateSys_TrainGrade(int ID, string newGrade)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"UPDATE dbo.Sys_TrainGrade
SET NewGrade = @newgrade,UpdateTime = getdate(),	Status = 1
WHERE ID=@ID");
                var param = new
                {
                    newgrade = newGrade,
                    ID = ID
                };
                conn.Execute(sql, param);
            }
        }


        /// <summary>
        /// 更新培训级别
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="newGrade"></param>
        public void AllUpdate(string IDs)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"UPDATE Sys_TrainGrade
SET Status=1
WHERE ID IN ({0})", IDs);
                conn.Execute(sql);
            }
        }


        #endregion

        #region 详情
        /// <summary>
        /// 变更详情
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<Sys_TrainGrade> GetUpdateLog(int UserID, int startIndex = 1, int startLength = int.MaxValue)
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT row_number()OVER(ORDER BY UpdateTime asc) number,count(*)OVER(PARTITION BY null) totalcount,
*,dbo.GetRealNameByUserID(CreateUserID) AS CreateName FROM Sys_TrainGrade
WHERE UserID={0}
AND Status=1) result
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", UserID);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_TrainGrade>(sql, param).ToList();
            }
        }
        #endregion

        #region 公共

        /// <summary>
        /// 获取所有的薪酬级别
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllPayGrade()
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = @"SELECT distinct PayGrade FROM Sys_User
WHERE IsDelete=0 AND IsTeacher<2 AND PayGrade<>'--'";
                return conn.Query<string>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取所有的培训级别
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllTrainGrade()
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = @"SELECT Grade FROM dbo.TrainGrade order by Grade asc";
                return conn.Query<string>(sql).ToList();
            }
        }
        #endregion

        public void InsertGrade(string grade)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"IF(SELECT COUNT(*) FROM TrainGrade WHERE Grade=@grade)=0
                            BEGIN
                            INSERT INTO dbo.TrainGrade
	                            (
	                            Grade
	                            )
                            VALUES 
	                            (
	                            @grade
	                            )
                            END";
                var param = new
                {
                    grade = grade
                };
                conn.Execute(sql, param);
            }
        }

    }
}
