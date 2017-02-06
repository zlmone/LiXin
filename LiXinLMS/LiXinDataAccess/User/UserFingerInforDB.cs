using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.User;

namespace LiXinDataAccess.User
{
    public class UserFingerInforDB : BaseRepository
    {
        /// <summary>
        /// 获取指纹列表
        /// </summary>
        /// <returns></returns>
        public List<Sys_UserFinger> GetUserFingerList(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
                                SELECT  row_number()OVER(ORDER BY Id DESC) number,count(*)OVER(PARTITION BY null) totalcount,
                                * FROM (
                                   SELECT suf.Id,su.UserId, su.Realname,su.IsTeacher,su.Status,su.Username as UserName,su.TrainGrade,
                                dbo.GetDeptPathByDeptID(su.DeptId) deptname,
                                suf.FingerTemplate1,suf.FingerTemplate2 FROM Sys_User su LEFT JOIN Sys_UserFingerInfor suf
                                ON su.UserId=suf.UserId
                                ) T
                                where  IsTeacher<2 and {0}
                               ) result
                              WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_UserFinger>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取指纹信息（导出指纹不为空的所有人）
        /// </summary>
        /// <returns></returns>
        public List<Sys_UserFinger> GetUserFingerInfor(string sqlwhere = "")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT suf.Id,su.UserId, su.Realname,su.IsTeacher,su.Username as UserName,su.TrainGrade,dbo.GetDeptPathByDeptID(su.DeptId) deptname,
suf.FingerTemplate1,suf.FingerTemplate2 
FROM Sys_User su LEFT JOIN Sys_UserFingerInfor suf
ON su.UserId=suf.UserId 
where  su.IsTeacher<2 and su.IsDelete=0 and su.Status=0 and {0}", sqlwhere);
                return conn.Query<Sys_UserFinger>(sql).ToList();
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="finger"></param>
        public void Update(int id, string finger1, string finger2)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"UPDATE Sys_UserFingerInfor
SET FingerTemplate1='{0}',FingerTemplate2='{2}'
WHERE Id={1}", finger1, id, finger2);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="finger"></param>
        public void Insert(int userid, string finger1, string finger2)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@" 
DECLARE @name NVARCHAR(50)
SELECT @name=Realname FROM Sys_User WHERE UserId=@userid
INSERT INTO dbo.Sys_UserFingerInfor
	(
	UserId,
	UserHrId,
	Ssn,
	Name,
	FingerTemplate,
	FingerTemplate1,
	FingerTemplate2
	)
VALUES 
	(
	@userid,
	'',
	'',
	@name,
	NULL,
	@fingertemplate1,
	@fingertemplate2
	)");
                var param = new
                {
                    userid = userid,
                    fingertemplate1 = finger1,
                    fingertemplate2 = finger2
                };
                conn.Execute(sql, param);
            }
        }
    }
}
