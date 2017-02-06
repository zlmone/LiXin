using System;
using System.Collections.Generic;
using System.Linq;
using LiXinModels;
using Retech.Core;
using Retech.Data;
using LiXinModels.SystemManage;
using LiXinModels.MyApproval;
using LiXinModels.User;

namespace LiXinDataAccess.MyApproval
{
    public class FreeDB : BaseRepository
    {
        #region 授权审批人
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertFree_DepApprove(Free_DepApprove model)
        {

            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Free_DepApprove(UserID,CreateUserID,CreateTime,ManageDeptIDs,LeardID)
	                     values( @UserID,@CreateUserID,@CreateTime,@ManageDeptIDs,@LeardID);SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.UserID,
                    model.CreateUserID,
                    model.CreateTime,
                    model.ManageDeptIDs,
                    model.LeardID
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.ID = decimal.ToInt32(model.ID);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateFree_DepApprove(Free_DepApprove model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update Free_DepApprove set 		                     		                     
		                       UserID = @UserID , 		                     
		                       CreateUserID = @CreateUserID , 		                     
		                       CreateTime = @CreateTime , 		                     
		                       ManageDeptIDs = @ManageDeptIDs , 		                     
		                       LeardID = @LeardID   
		                   where ID=@ID";

                var param = new
                {
                    model.ID,
                    model.UserID,
                    model.CreateUserID,
                    model.CreateTime,
                    model.ManageDeptIDs,
                    model.LeardID
                };
                conn.Execute(strSql, param);
            }

        }


        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        public void DeleteFree_DepApprove(string IDlist)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"delete from Free_DepApprove
                        WHERE   ID in (" + IDlist + ")";
                conn.Execute(sql);
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<Free_DepApprove> GetModel(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select * from Free_DepApprove  
			                   where {0}", where);

                return conn.Query<Free_DepApprove>(sql).ToList();
            }

        }

        /// <summary>
        /// 授权审批人
        /// </summary>
        /// <returns></returns>
        public List<ShowDepApprove> GetDepApproveList(string LeaderID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select * from(SELECT count(1)OVER(PARTITION BY NULL) totalCount,* FROM(
SELECT  row_number() over(ORDER BY ssu.UserId ) as rowNum,ssu.UserId,Realname,Sex, TrainGrade, DeptId,dbo.GetDeptPathByDeptID(DeptId) DeptPath,
ManageDeptIDs, ManageDeptName= isnull(
(SELECT DeptName+','  FROM Sys_Department WHERE DepartmentId IN (SELECT ID FROM dbo.F_SplitIDs(ManageDeptIDs))
FOR XML path('')),'N/A'),fda.ID
FROM  Sys_User  ssu
LEFT JOIN Free_DepApprove  fda ON ssu.UserId=fda.UserID
WHERE Status=0 AND IsDelete=0 AND  LeaderID={0} and {1}
) t  
) r where rowNum between @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", LeaderID, where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<ShowDepApprove>(sql, param).ToList();
            }
        }
        #endregion

        #region 其他形式审批
        /// <summary>
        /// 其他形式审批
        /// </summary>
        /// <returns></returns>
        public List<ShowFreeDetails> GetOtherApproveList(int userID, string ManageDeptIDs, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue,
           string order = "ApplyTime desc")
        {
            using (var conn = OpenConnection())
            {

                var sql = string.Format(@"
DECLARE @ManageDeptIDs NVARCHAR(50) 
SELECT @ManageDeptIDs=ManageDeptIDs FROM Free_DepApprove
WHERE UserID={2};
WITH tbdeptID AS
(
    SELECT ID FROM dbo.F_SplitIDs('{3}')                               
	UNION
	SELECT ID FROM dbo.F_SplitIDs(@ManageDeptIDs)  
)
SELECT * FROM(
SELECT * FROM(
SELECT  row_number() over(ORDER BY  {0}) as rowNum,count(1)OVER(PARTITION BY NULL) totalCount,* FROM(
SELECT fuo.*,isCommit=(SELECT count(1) FROM Free_UserApplyFiles WHERE userApplyID=fuo.ID),
ssu.Realname,ssu.TrainGrade,ssu.CPA ,foa.ApplyContent,foa.ConvertType,ssu.DeptPath DeptName
FROM Free_UserOtherApply fuo
LEFT JOIN Sys_User ssu ON fuo.ApplyUserID=ssu.UserId
LEFT JOIN Free_OtherApplyConfig foa ON foa.ID=fuo.ApplyID
WHERE fuo.IsDelete=0 and  fuo.ApplyType in (1,3) and  ApplyUserID IN (SELECT UserId FROM Sys_User
WHERE IsDelete=0 AND Status=0 AND (DeptId IN (SELECT ID FROM tbdeptID)
OR userId IN (SELECT UserID FROM Sys_User WHERE LeaderID=(SELECT JobNum FROM Sys_User WHERE UserId={2})))
   AND fuo.Status=1 )
)t where  {1})  r
)result where rowNum between @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", order, where, userID, ManageDeptIDs);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<ShowFreeDetails>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 选择申请年度
        /// </summary>
        /// <returns></returns>
        public List<int> GetYearList(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT distinct Year FROM   Free_UserOtherApply
WHERE IsDelete=0 and" + where;
                return conn.Query<int>(sql).ToList();
            }
        }

        /// <summary>
        /// 选择申请年度
        /// </summary>
        /// <returns></returns>
        public List<int> GetOrgYearList(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT distinct Year FROM   Free_UserApplyOtherForm
WHERE IsDelete=0 and" + where;
                return conn.Query<int>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取其他形式发起人信息
        /// </summary>
        /// <param name="ApplyID"></param>
        /// <returns></returns>
        public ShowFreeDetails GetSingleByID(int ApproveID, int type = 1)
        {
            using (var conn = OpenConnection())
            {
                try
                {
                    var sql = @"SELECT fuo.*,syu.Realname,syu.CPANo,syu.TrainGrade,syu.Sex,syu.CPA,syu.CPANo,syu.DeptName,syu.MobileNum
,foa.ApplyContent,foa.ConvertBase,foa.ConvertBaseInfo,foa.ConvertBaseTo,foa.ConvertMax,foa.TrainGradeScore,syu.SubordinateSubstation
,foa.ConvertType
FROM Free_UserOtherApply fuo
LEFT JOIN Free_OtherApplyConfig foa ON fuo.ApplyID=foa.ID
LEFT JOIN Sys_User syu ON fuo.ApplyUserID=syu.UserId
WHERE fuo.ID=@ApproveID and  " + (type == -1 ? " 1=1" : "fuo.ApplyType = @ApplyType;");
                    var param = new { ApproveID = ApproveID, ApplyType = type };
                    return conn.Query<ShowFreeDetails>(sql, param).FirstOrDefault();
                }
                catch
                {
                    return new ShowFreeDetails();
                }

            }
        }
        #endregion

        #region 审批流程线


        /// <summary>
        /// 根据申请查询其审批流程
        /// </summary>
        /// <param name="ApproveID"></param>
        /// <returns></returns>
        public List<Free_Apporve> GetApproveListByID(int ApproveID, int ApproveType = 1, int type = 1)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT fa.*,syu.Realname FROM Free_Apporve fa
LEFT JOIN Sys_User syu ON syu.UserId=fa.ApproveUserID 
WHERE userApplyID=@ApproveID and ApproveType=@ApproveType and " + (type == -1 ? " 1=1 " : " fa.type=@type");
                var param = new { ApproveID = ApproveID, ApproveType = ApproveType, type = type };
                return conn.Query<Free_Apporve>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 证明资料
        /// </summary>
        /// <param name="ApproveID"></param>
        /// <returns></returns>
        public List<Free_UserApplyFiles> GetApplyFile(int ApproveID, int type = 0)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT * FROM Free_UserApplyFiles
WHERE  IsDelete=0 and userApplyID=@ApproveID and type=@type";
                var param = new { ApproveID = ApproveID, type = type };
                return conn.Query<Free_UserApplyFiles>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 有就查出最后一次拒绝或通过的状态
        /// </summary>
        /// <returns></returns>
        public Free_Apporve GetMaxDepApproveBatch(int ApproveID, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT TOP 1 * FROM Free_Apporve
WHERE userApplyID=@ApproveID   AND {0}
ORDER BY CheackBatch desc", where);
                var param = new { ApproveID = ApproveID };
                var model = conn.Query<Free_Apporve>(sql, param).FirstOrDefault();
                return model == null ? new Free_Apporve() : model;
            }
        }

        #endregion

        #region 新增审批信息
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertFree_Apporve(Free_Apporve model)
        {

            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Free_Apporve(LookFile,userApplyID,DepApprove,DepReason,CheackBatch,type,ApproveUserID,ApproveTime,ApproveType)
	                     values( @LookFile,@userApplyID,@DepApprove,@DepReason,@CheackBatch,@type,@ApproveUserID,@ApproveTime,@ApproveType);SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.LookFile,
                    model.userApplyID,
                    model.DepApprove,
                    model.DepReason,
                    model.CheackBatch,
                    model.type,
                    model.ApproveUserID,
                    model.ApproveTime,
                    model.ApproveType
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.ID = decimal.ToInt32(list.ID);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateFree_Apporve(Free_Apporve model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update Free_Apporve set 			                     
		                       ApproveType = @ApproveType , 		                     
		                       LookFile = @LookFile , 		                     
		                       userApplyID = @userApplyID , 	                     
		                       DepApprove = @DepApprove , 		                     
		                       DepReason = @DepReason , 		                     
		                       CheackBatch = @CheackBatch , 
                               ApproveUserID=@ApproveUserID,
                               ApproveTime=@ApproveTime,		                     
		                       type = @type 
		                   where ID=@ID";

                var param = new
                {
                    model.ID,
                    model.ApproveType,
                    model.LookFile,
                    model.userApplyID,
                    model.DepApprove,
                    model.DepReason,
                    model.CheackBatch,
                    model.ApproveUserID,
                    model.ApproveTime,
                    model.type
                };
                conn.Execute(strSql, param);
            }

        }


        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        public void DeleteFree_Apporve(string IDlist)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"Updatet  Free_Apporve
                         SET IsDelete=1
                        WHERE   ID in (" + IDlist + ")";
                conn.Execute(sql);
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<Free_Apporve> GetFreeApporve(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select * from Free_Apporve  
			                   where {0}", where);

                return conn.Query<Free_Apporve>(sql).ToList();
            }

        }

        /// <summary>
        /// 更新个人申请的状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="where"></param>
        public void UpdateUserApply(int ID, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"UPDATE Free_UserOtherApply
                            SET {0}
                            WHERE ID={1}", where, ID);
                conn.Execute(sql);
            }
        }
        #endregion

        #region 免修审批
        /// <summary>
        /// 其他形式审批
        /// </summary>
        /// <returns></returns>
        public List<ShowFreeDetails> GetFreeApproveList(int userID, string ManageDeptIDs, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyTime desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
DECLARE @ManageDeptIDs NVARCHAR(50) 
SELECT @ManageDeptIDs=ManageDeptIDs FROM Free_DepApprove
WHERE UserID={2};
WITH tbdeptID AS
(
    SELECT ID FROM dbo.F_SplitIDs('{3}')                              
	UNION 
	SELECT ID FROM dbo.F_SplitIDs(@ManageDeptIDs)  
)
SELECT * FROM(
SELECT * FROM(
SELECT row_number() over(ORDER BY  {0}) as rowNum,count(1)OVER(PARTITION BY NULL) totalCount,* FROM(
SELECT fuo.*,isCommit=(SELECT count(1) FROM Free_UserApplyFiles WHERE userApplyID=fuo.ID),
ssu.Realname,ssu.TrainGrade,ssu.CPA  ,foa.FreeName,foa.ApplyType ConvertType,ssu.DeptPath DeptName
FROM Free_UserOtherApply fuo
LEFT JOIN Sys_User ssu ON fuo.ApplyUserID=ssu.UserId
LEFT JOIN Free_ApplyConfig foa ON foa.ID=fuo.ApplyID
WHERE  fuo.ApplyType=2 and ApplyUserID IN (SELECT UserId FROM Sys_User
WHERE IsDelete=0 AND Status=0 AND (DeptId IN (SELECT ID FROM tbdeptID)
OR userId IN (SELECT UserID FROM Sys_User WHERE LeaderID=(SELECT JobNum FROM Sys_User WHERE UserId={2})))
   AND fuo.Status=1 )
)t where {1})r
)result where rowNum between @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", order, where, userID, ManageDeptIDs);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<ShowFreeDetails>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取免修发起人信息
        /// </summary>
        /// <param name="ApplyID"></param>
        /// <returns></returns>
        public ShowFreeDetails GetFreeSingleByID(int ApproveID)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT fuo.ID, ApplyID,fuo.Year,ApplyTime, ApplyUserID,fuo.ApplyType,ApproveStatus, DepApproveStatus,syu.Realname,syu.CPANo,syu.TrainGrade,syu.Sex,syu.CPA,syu.CPANo,syu.DeptName
,FreeName, fac.UploadTip, fac.ApplyType,fuo.tScore, fuo.CPAScore,fuo.Memo,syu.SubordinateSubstation,fromID,fuo.CreateTime
FROM Free_UserOtherApply fuo
LEFT JOIN Free_ApplyConfig fac ON fuo.ApplyID=fac.ID
LEFT JOIN Sys_User syu ON fuo.ApplyUserID=syu.UserId
WHERE fuo.ID=@ApproveID and fuo.ApplyType=2";
                var param = new { ApproveID = ApproveID };
                return conn.Query<ShowFreeDetails>(sql, param).FirstOrDefault();
            }
        }
        #endregion

        #region  总所其他形式审批
        /// <summary>
        /// 总所其他形式审批
        /// </summary>
        /// <returns></returns>
        public List<ShowFreeDetails> GetDepOtherApproveList(int userID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue,
           string order = "ApplyTime desc")
        {
            using (var conn = OpenConnection())
            {

                var sql = string.Format(@"
SELECT * FROM(
SELECT * FROM(
SELECT row_number() over(ORDER BY  {0}) as rowNum,count(1)OVER(PARTITION BY NULL) totalCount,* FROM(
SELECT fuo.*,isCommit=(SELECT count(1) FROM Free_UserApplyFiles WHERE userApplyID=fuo.ID),
ssu.Realname,ssu.TrainGrade,ssu.CPA  ,foa.ApplyContent,foa.ConvertType,ssu.DeptPath DeptName
FROM Free_UserOtherApply fuo
LEFT JOIN Sys_User ssu ON fuo.ApplyUserID=ssu.UserId
LEFT JOIN Free_OtherApplyConfig foa ON foa.ID=fuo.ApplyID
WHERE  fuo.ApplyType in (1,3) and  ApplyUserID IN (SELECT UserId FROM Sys_User
WHERE IsDelete=0 AND Status=0)
AND fuo.Status=1)t where  {1} )r
)result where rowNum between @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", order, where, userID);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<ShowFreeDetails>(sql, param).ToList();
            }
        }
        #endregion

        #region 总所免修审批
        /// <summary>
        /// 总所免修审批
        /// </summary>
        /// <returns></returns>
        public List<ShowFreeDetails> GetDepFreeApproveList(int userID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyTime desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
SELECT * FROM(
SELECT * FROM(
SELECT row_number() over(ORDER BY  {0}) as rowNum,count(1)OVER(PARTITION BY NULL) totalCount,* FROM(
SELECT fuo.*,isCommit=(SELECT count(1) FROM Free_UserApplyFiles WHERE userApplyID=fuo.ID),
ssu.Realname,ssu.TrainGrade,ssu.CPA  ,foa.FreeName,foa.ApplyType ConvertType,ssu.DeptPath DeptName
FROM Free_UserOtherApply fuo
LEFT JOIN Sys_User ssu ON fuo.ApplyUserID=ssu.UserId
LEFT JOIN Free_ApplyConfig foa ON foa.ID=fuo.ApplyID
WHERE  fuo.ApplyType=2 and ApplyUserID IN (SELECT UserId FROM Sys_User
WHERE IsDelete=0 AND Status=0)
AND fuo.Status=1 )t where {1})r 
)result where rowNum between @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", order, where, userID);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<ShowFreeDetails>(sql, param).ToList();
            }
        }
        #endregion

        #region  其他有组织形式审批
        /// <summary>
        ///  其他有组织形式审批 分所
        /// </summary>
        /// <returns></returns>
        public List<ShowFreeDetails> GetFreeOrgList(int userID, string ManageDeptIDs, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyDateTime desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
DECLARE @ManageDeptIDs NVARCHAR(50) 
SELECT @ManageDeptIDs=ManageDeptIDs FROM Free_DepApprove
WHERE UserID={2};
WITH tbdeptID AS
(
   SELECT ID FROM dbo.F_SplitIDs('{3}')                               
	UNION 
	SELECT ID FROM dbo.F_SplitIDs(@ManageDeptIDs)  
)
SELECT * FROM(
SELECT * FROM(
SELECT row_number() over(ORDER BY  {0}) as rowNum,count(1)OVER(PARTITION BY NULL) totalCount,* FROM(
SELECT fuo.*,isCommit=(SELECT count(1) FROM Free_UserApplyFiles WHERE userApplyID=fuo.ID),
ssu.Realname,ssu.TrainGrade,ssu.CPA,ssu.DeptPath DeptName
FROM Free_UserApplyOtherForm fuo
LEFT JOIN Sys_User ssu ON fuo.ApplyUserID=ssu.UserId
WHERE  ApplyUserID IN (SELECT UserId FROM Sys_User
WHERE IsDelete=0 AND Status=0 AND (DeptId IN (SELECT ID FROM tbdeptID)
OR userId IN (SELECT UserID FROM Sys_User WHERE LeaderID=(SELECT JobNum FROM Sys_User WHERE UserId={2})))
AND fuo.Status=1)
)t where {1})r
)result where rowNum between @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", order, where, userID, ManageDeptIDs);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<ShowFreeDetails>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取免修发起人信息
        /// </summary>
        /// <param name="ApplyID"></param>
        /// <returns></returns>
        public ShowFreeDetails GetOrgSingleByID(int ApproveID)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT fuo.*,syu.Realname,syu.CPANo,syu.TrainGrade,syu.Sex,syu.CPA,syu.CPANo,syu.DeptName,ApplyDateTime ApplyTime
,syu.SubordinateSubstation
FROM Free_UserApplyOtherForm fuo
LEFT JOIN Sys_User syu ON fuo.ApplyUserID=syu.UserId
WHERE fuo.ID=@ApproveID ";
                var param = new { ApproveID = ApproveID };
                return conn.Query<ShowFreeDetails>(sql, param).FirstOrDefault();
            }
        }

        /// <summary>
        ///  其他有组织形式审批 总所
        /// </summary>
        /// <returns></returns>
        public List<ShowFreeDetails> GetDepFreeOrgList(int userID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyDateTime desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
SELECT * FROM(
SELECT row_number() over(ORDER BY  {0}) as rowNum,count(1)OVER(PARTITION BY NULL) totalCount,* FROM(
SELECT fuo.*,isCommit=(SELECT count(1) FROM Free_UserApplyFiles WHERE userApplyID=fuo.ID),
GetCPAS=CASE WHEN (OtherFromID=1 OR ssu.CPA='否') THEN -1 ELSE  CPAScore END ,
ssu.Realname,ssu.TrainGrade,ssu.DeptPath DeptName
FROM Free_UserApplyOtherForm fuo
LEFT JOIN Sys_User ssu ON fuo.ApplyUserID=ssu.UserId
WHERE  ApplyUserID IN (SELECT UserId FROM Sys_User
WHERE IsDelete=0 AND Status=0 )
AND fuo.Status=1)t WHERE {1}
)r  where rowNum between @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", order, where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<ShowFreeDetails>(sql, param).ToList();
            }
        }
        #endregion

        /// <summary>
        /// 获取当前人员所在部门的负责人或者分配能管理当前人员的部门
        /// </summary>
        /// <param name="DeptID"></param>
        /// <param name="JobNum"></param>
        /// <returns></returns>
        public List<Sys_User> GetManageUserByDeptID(int DeptID, string JobNum)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT UserId, Email, Realname FROM Sys_User
WHERE (SELECT count(1) FROM dbo.F_SplitIDs(ManageDeparts) WHERE ID={0})>0  OR  JobNum='{1}'
UNION
SELECT Sys_User.UserId, Email, Realname FROM Free_DepApprove
LEFT JOIN   Sys_User   ON  Sys_User.UserId=Free_DepApprove.UserID
WHERE (SELECT count(1) FROM dbo.F_SplitIDs(ManageDeptIDs) WHERE ID={0})>0", DeptID, (string.IsNullOrEmpty(JobNum) ? "0" : JobNum));
                return conn.Query<Sys_User>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取所有培训教育部的人，用来发送邮件
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetManageUser()
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT UserId,Realname,Email FROM Sys_User
WHERE DeptId=92 AND Status=0 AND IsDelete=0";
                return conn.Query<Sys_User>(sql).ToList();
            }
        }

        /// <summary>
        /// 当前申请的免修是否在范围之内
        /// </summary>
        /// <param name="year"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int IfExistFreeItem(int year, int ID, int ApplyUserID = -1)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT count(1) FROM Free_UserOtherApply
WHERE IsDelete=0 and ApplyType=2 AND Year={0}  and {1}", year, (ApplyUserID == -1 ? "" : " ApplyUserID=" + ApplyUserID), ID);
                return conn.Query<int>(sql).FirstOrDefault();
            }
        }


        /// <summary>
        /// 当前申请的免修
        /// </summary>
        /// <param name="year"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetFreeItemByUser(int year, int ApplyUserID = -1)
        {
            using (var  conn=OpenConnection())
            {
                var sql = string.Format(@"
SELECT ApplyUserID,sum(tScore) tScore, sum(CPAScore) CPAScore FROM Free_UserOtherApply fuo
WHERE fuo.ApplyType=2 AND fuo.IsDelete=0 AND ApplyUserID>0
AND {1} AND fuo.Year={0}
GROUP BY ApplyUserID", year, (ApplyUserID == -1 ? " 1=1   " : " fuo.ApplyUserID=" + ApplyUserID));
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取各部门的授权审批信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<Free_DepApprove> GetDeptApproveUserList(string where = "1=1", int startIndex = 1, int startLength = int.MaxValue)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT count(1)OVER(PARTITION BY NULL) totalCount,* FROM(
SELECT row_number() over(ORDER BY  CreateTime) as rowNum,fd.*,syu.Realname,syu.Sex,syu.TrainGrade,syu.DeptName,syu.DeptPath,syu1.Realname createName,ManageDeptName=
(SELECT DeptName+',' FROM Sys_Department 
WHERE DepartmentId IN (SELECT ID FROM dbo.F_SplitIDs(fd.ManageDeptIDs)) FOR XML path(''))
FROM Free_DepApprove fd
LEFT JOIN Sys_User syu ON fd.UserID=syu.UserId
LEFT JOIN Sys_User syu1 ON fd.CreateUserID=syu1.UserId
) t where {0})r where rowNum between @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Free_DepApprove>(sql, param).ToList();
            }
        }
    }

}
