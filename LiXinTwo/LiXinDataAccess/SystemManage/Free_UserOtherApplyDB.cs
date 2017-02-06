using LiXinModels.SystemManage;
using Retech.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Retech.Data;
using LiXinModels.User;
using LiXinModels.MyApproval;

namespace LiXinDataAccess.SystemManage
{
    public class Free_UserOtherApplyDB : BaseRepository
    {
        #region 基础方法
        public bool AddFree_UserOtherApply(Free_UserOtherApply model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO Free_UserOtherApply(ApplyID,Year,ConvertScore,Memo,ApplyTime,ApplyUserID,ApplyType,Status,ApproveStatus,IsDelete,tScore,CPAScore,ApplyName,DepApproveStatus,GetCPAScore,GettScore,DepTrainReason,DepReason,typeForm,CreateUserID,CreateTime,fromID) 
VALUES (@ApplyID,@Year,@ConvertScore,@Memo,@ApplyTime,@ApplyUserID,@ApplyType,@Status,@ApproveStatus,@IsDelete,@tScore,@CPAScore,@ApplyName,@DepApproveStatus,@GetCPAScore,@GettScore,'','',@typeForm,@CreateUserID,@CreateTime,@fromID)SELECT @@IDENTITY AS ID";
                var param = new
                {
                    //model.ID,
                    model.ApplyID,
                    model.Year,
                    model.ConvertScore,
                    model.Memo,
                    model.ApplyTime,
                    model.ApplyUserID,
                    model.ApplyType,
                    model.Status,
                    model.ApproveStatus,
                    model.IsDelete,
                    model.tScore,
                    model.CPAScore,
                    model.ApplyName,
                    model.DepApproveStatus,
                    model.GetCPAScore,
                    model.GettScore,
                    model.typeForm,
                    model.CreateUserID,
                    CreateTime = DateTime.Now,
                    model.fromID
                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.ID = decimal.ToInt32(id);

                return model.ID > 0 ? true : false;

            }
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateFree_UserOtherApply(Free_UserOtherApply model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Free_UserOtherApply SET ApplyID = @ApplyID,Year = @Year,ConvertScore=@ConvertScore,Memo=@Memo,ApplyTime=@ApplyTime,ApplyUserID=@ApplyUserID,ApplyType=@ApplyType,Status=@Status,ApproveStatus=@ApproveStatus,IsDelete=@IsDelete,tScore=@tScore,CPAScore=@CPAScore,ApplyName=@ApplyName,DepApproveStatus=@DepApproveStatus,GetCPAScore=@GetCPAScore,GettScore=@GettScore,typeForm=@typeForm,CreateUserID=@CreateUserID,fromID=@fromID WHERE Id=@ID";
                var param = new
                {
                    model.ID,
                    model.ApplyID,
                    model.Year,
                    model.ConvertScore,
                    model.Memo,
                    model.ApplyTime,
                    model.ApplyUserID,
                    model.ApplyType,
                    model.Status,
                    model.ApproveStatus,
                    model.IsDelete,
                    model.tScore,
                    model.CPAScore,
                    model.ApplyName,
                    model.DepApproveStatus,
                    model.GetCPAScore,
                    model.GettScore,
                    model.typeForm,
                    model.CreateUserID,
                    model.fromID
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="groupIds">群组ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteFree_UserOtherApply(string id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"update Free_UserOtherApply set IsDelete = 1 where id in(" + id + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }


        public bool UpdateFree_UserOtherApply_Status(int id, string where = " ,ApproveStatus=0,DepApproveStatus=0")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "Update Free_UserOtherApply SET ApplyTime=getdate(),Status=1 " + where + " WHERE Id=" + id;

                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }
        #endregion

        #region 其他形式

        public List<Free_UserOtherApply> GetFree_UserOtherApplyList(string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select * from Free_UserOtherApply where " + where;
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }


        public List<Free_UserOtherApply> GetFree_UserOtherApplyList_New(string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"
SELECT Free_UserOtherApply.*,Free_OtherApplyConfig.ApplyContent AS ApplyName_New,Free_OtherApplyConfig.ConvertType AS ConvertType
 from Free_UserOtherApply 
     	LEFT JOIN Free_OtherApplyConfig ON Free_UserOtherApply.ApplyID=Free_OtherApplyConfig.ID
 where " + where;
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }

        public List<Free_UserOtherApply> GetFree_UserOtherApplyList_New_Batch(string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"
SELECT Free_UserOtherApply.*,Free_OtherApplyConfig.ApplyContent AS ApplyName_New,Free_OtherApplyConfig.ConvertType AS ConvertType,
Sys_User.Realname AS Realname,
Free_OtherApplyConfig.ConvertMax,
Free_OtherApplyConfig.TrainGradeScore,
Free_OtherApplyConfig.ConvertType
 from Free_UserOtherApply 
     	LEFT JOIN Free_OtherApplyConfig ON Free_UserOtherApply.ApplyID=Free_OtherApplyConfig.ID
        LEFT JOIN Sys_User ON Free_UserOtherApply.CreateUserID=Sys_User.UserId
 where " + where;
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }

        public Free_UserOtherApply GetFree_UserOtherApplyById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"
SELECT * from Free_UserOtherApply where id=" + id;
                return conn.Query<Free_UserOtherApply>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取已经最大的课后评估学时
        /// </summary>
        /// <returns></returns>
        public decimal GetMaxScore(int userID, int year)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT isnull(sum(tScore),0) Score FROM  Free_UserOtherApply
WHERE typeForm=2 and ApplyUserID=" + userID + " and Year=" + year;
                return conn.Query<decimal>(sql).ToList().FirstOrDefault();
            }
        }


        /// <summary>
        /// 评估自动折算
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetOutUserApply(string where = " 1 = 1 ")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT fuo.ID, ApplyID,cc.CourseName ApplyName_New, tScore, GettScore,1 ConvertType,ApplyTime,2 typeForm,1 DepApproveStatus FROM Free_UserOtherApply fuo
LEFT JOIN Co_Course cc ON cc.Id=fuo.ApplyID
WHERE typeForm=2  " + where;
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }

        /// <summary>
        /// 授课讲师自动折算的学时
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetTeacherScore(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT cc.Id,cc.CourseName ApplyName_New, CourseLength ConvertScore,EndTime ApplyTime,Teacher,3 typeForm FROM Co_Course cc
WHERE EndTime<getdate() 
AND Publishflag=1  AND Way=1  AND cc.IsDelete=0 AND CourseFrom=2 " + where;
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取当前人员年度内申请的所有学时
        /// </summary>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetMaxScore(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT ApplyID,ApplyUserID,syu.TrainGrade,syu.CPA, Year,sum(tScore) tScore, sum(CPAScore) CPAScore
 FROM Free_UserOtherApply fuo 
LEFT JOIN Sys_User syu ON syu.UserId=fuo.ApplyUserID
WHERE fuo.IsDelete=0 AND ((ApproveStatus!=2 AND DepApproveStatus!=0) OR (ApproveStatus!=2 AND DepApproveStatus!=2)) 
and ApplyType in (1,3) and  {0}
GROUP  BY  ApplyID,ApplyUserID,Year,TrainGrade,syu.CPA                                    
 ", where);
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取当前人员年度内申请的所有学时
        /// </summary>
        public List<Free_UserOtherApply> GetMaxScore(string userIDs, int ApplyID, int Year)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT  syu.UserId ApplyUserID, syu.TrainGrade,syu.CPA, Year,sum(tScore) tScore, sum(CPAScore) CPAScore FROM Sys_User syu
LEFT JOIN
(
	SELECT * from Free_UserOtherApply where ApplyID={1} and Year={2}
	AND IsDelete=0 AND ((ApproveStatus!=2 AND DepApproveStatus!=0) OR (ApproveStatus!=2 AND DepApproveStatus!=2)) 
	and ApplyType in (1,3) AND ApplyUserID IN ({0}) 
) t   ON syu.UserId=t.ApplyUserID
WHERE   syu.UserId IN ({0})
GROUP BY syu.UserId, syu.TrainGrade,syu.CPA, Year", userIDs, ApplyID, Year);
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }

        /// <summary> 
        /// 获取当然授课人员以及申请的以及获得的
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public dynamic GetScoreByteacher(int userID, int year, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT sum(tScore) tScore, sum(CPAScore) CPAScore,
CPA=(SELECT CPA FROM Sys_User WHERE UserId={0}) 
FROM Free_UserOtherApply
WHERE ApplyType=3 AND {2}
AND ApplyUserID={0} and Year={1}", userID, year, where);
                return conn.Query<dynamic>(sql).ToList().FirstOrDefault();
            }
        }

        /// <summary>
        /// 授课人的自动折算
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetTeacherScoreList(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT fuo.*,cc.CourseName ApplyName_New,2 isout,
GetCPAScore= CASE WHEN IsCPA=1 THEN GetCPAScore ELSE 0  END, 
ConvertType= CASE WHEN IsCPA=1 THEN 2 ELSE 1  END FROM Free_UserOtherApply fuo
LEFT JOIN Co_Course cc ON fuo.ApplyID=cc.Id
WHERE fuo.typeForm=3 AND fuo.IsDelete=0 and " + where;
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取人员获取的
        /// </summary>
        /// <param name="applyUserIDs"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetScoreByUser(string applyUserIDs, int year)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT fuo.ApplyUserID,sum(tScore) tScore, sum(CPAScore) CPAScore
FROM Free_UserOtherApply fuo
WHERE ApplyType=3 
AND ApplyUserID in ({0}) and Year={1}
and (typeForm=0 OR (typeForm=1 AND CreateUserID=0) OR typeForm=3)
GROUP BY fuo.ApplyUserID", applyUserIDs, year);
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }

        /// <summary>
        /// 评估,授课人的自动评估查看
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ShowFreeDetails GetSurveyOut(int ID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
SELECT  fuo.*,cc.CourseName ApplyContent,syu.Realname,syu.CPANo,syu.TrainGrade,syu.Sex,syu.CPA,syu.CPANo,syu.DeptName,
syu.MobileNum,syu.SubordinateSubstation,cc.StartTime,cc.EndTime 
FROM Free_UserOtherApply fuo
LEFT JOIN Co_Course cc ON fuo.ApplyID=cc.Id 
LEFT JOIN Sys_User syu ON syu.UserId=fuo.ApplyUserID
WHERE fuo.ID={0}", ID);
                return conn.Query<ShowFreeDetails>(sql).ToList().FirstOrDefault();
            }
        }
        #endregion

        #region 免修
        /// <summary>
        /// 获取我申请的免修
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetFreeApply(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT foa.*,fac.FreeName ApplyName_New,fac.ApplyType AS configtype FROM Free_UserOtherApply foa
LEFT JOIN Free_ApplyConfig fac ON foa.ApplyID=fac.ID
WHERE foa.ApplyType=2 and {0}", where);
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }
        #endregion

        #region 批量免修
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ApplyUserID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public void DeleteUserApply(string ApplyUserID, int ID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"update Free_UserOtherApply
SET IsDelete=1 
WHERE ApplyUserID in ({0}) AND fromID={1}", ApplyUserID, ID);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 查询批量免修
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<ShowFreeDetails> GetFreeDetailsList(string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ID desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT row_number()OVER(ORDER BY {0}) number,count(*)OVER(PARTITION BY null) totalCount,fua.ID,
ApplyID, ApplyName, fua.Year,fua.Memo,tScore, CPAScore ,fua.CreateUserID,fua.CreateTime,syu.Realname CreateUserName
,fac.FreeName,fac.ApplyType,fua.Status FROM Free_UserOtherApply  fua
LEFT JOIN Free_ApplyConfig fac ON fua.ApplyID=fac.ID 
LEFT JOIN Sys_User syu ON syu.UserId=fua.CreateUserID
WHERE fua.ApplyType=2 AND typeForm=1   AND CreateUserID>0 and  {1}
)t WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", order, where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<ShowFreeDetails>(sql, param).ToList();
            }
        }


        #endregion

        #region 公共

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="groupIds">群组ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteFree_UserOtherApplyByPeopleAndId(string ids, int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"update Free_UserOtherApply set IsDelete = 1 where ApplyUserID in(" + id + ") and Id=" + id;
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="groupIds">群组ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteFree_UserApplyFilesById(string ids)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"update Free_UserApplyFiles set IsDelete = 1 where Id in (" + ids + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }


        public List<Free_UserOtherApply> FindFree_UserOtherApplyByFrom(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"select * from Free_UserOtherApply where fromId=" + id;
                return conn.Query<Free_UserOtherApply>(sqlwhere).ToList();
            }
        }


        /// <summary>
        /// 获取我的当年度免修的学时
        /// </summary>
        /// <param name="year"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetAllMyScore(string year, int userID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT ApplyUserID,typeForm,Year,ApplyType,sum(GettScore) GettScore,sum(GetCPAScore) GetCPAScore FROM (
	SELECT sum(GettScore) GettScore, sum(GetCPAScore) GetCPAScore, ApplyUserID,fuo.Year,typeForm,ApplyType FROM Free_UserOtherApply fuo
	WHERE fuo.IsDelete=0 and Status=1 AND ApproveStatus=1 AND DepApproveStatus=1 AND CreateUserID=0 
	GROUP BY ApplyUserID,Year,typeForm,ApplyType 
	UNION
    SELECT   sum(GettScore) GettScore, sum(GetCPAScore) GetCPAScore , ApplyUserID,Year,5,4 FROM (
	SELECT GettScore=CASE WHEN OtherFromID=0 THEN 0 ELSE GettScore  END ,GetCPAScore=CASE WHEN OtherFromID=1 THEN 0 ELSE GetCPAScore  END, ApplyUserID,Year  FROM Free_UserApplyOtherForm
	WHERE IsDelete=0 and  Status=1 AND ApproveStatus=1 AND DepApproveStatus=1 ) r
	GROUP BY ApplyUserID,Year
) t
where  ApplyUserID={0} and Year in ({1})
GROUP BY   ApplyUserID,typeForm,Year ,ApplyType", userID, year);
                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }
        #endregion

        #region 用于个人中心的查询
        /// <summary>
        /// 其他形式
        /// </summary>
        /// <returns></returns>
        public List<FreeUseScore> GetUserFreeOtherList(string where = "1=1", string outwhere = "1=1", string order = " typeForm,ApplyID,ApplyTime asc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT fuo.ApplyID,foa.ApplyContent,fuo.ApplyTime,fuo.GettScore,fuo.GetCPAScore,typeForm 
FROM Free_UserOtherApply  fuo
LEFT JOIN Free_OtherApplyConfig foa ON fuo.ApplyID=foa.ID
WHERE CreateUserID=0 AND (fuo.ApplyType=1 or fuo.ApplyType=3) 
AND fuo.Status=1 AND fuo.ApproveStatus=1 AND fuo.DepApproveStatus=1
AND fuo.IsDelete=0  and {0}
UNION
SELECT fuo.ApplyID,cc.CourseName ApplyContent,fuo.ApplyTime,fuo.GettScore,fuo.GetCPAScore,typeForm 
FROM Free_UserOtherApply  fuo
LEFT JOIN Co_Course cc ON cc.Id=fuo.ApplyID
WHERE CreateUserID=0 AND (fuo.ApplyType=1 or fuo.ApplyType=3) 
AND fuo.Status=1 AND fuo.ApproveStatus=1 AND fuo.DepApproveStatus=1 AND (typeForm=2 OR typeForm=3)
AND fuo.IsDelete=0  and {1}

)t order by {2} ", where, outwhere, order);
                return conn.Query<FreeUseScore>(sql).ToList();
            }
        }

        /// <summary>
        /// 免修
        /// </summary>
        /// <returns></returns>
        public List<FreeUseScore> GetUserFreeList(string where = "1=1", string order = "ApplyID desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT count(*)OVER(PARTITION BY null) totalCount,fuo.ApplyID,foa.FreeName ApplyContent,fuo.ApplyTime
,fuo.GettScore,fuo.GetCPAScore,typeForm FROM Free_UserOtherApply  fuo
LEFT JOIN Free_ApplyConfig foa ON fuo.ApplyID=foa.ID
WHERE CreateUserID=0 AND fuo.ApplyType=2
AND fuo.Status=1 AND fuo.ApproveStatus=1 AND fuo.DepApproveStatus=1
AND fuo.IsDelete=0  AND {1}
 
)t  order by {0}", order, where);
                return conn.Query<FreeUseScore>(sql).ToList();
            }
        }

        /// <summary>
        /// 其他有组织形式
        /// </summary>
        /// <returns></returns>
        public List<FreeUseScore> GetUserFreeOrgList(string where = "1=1", string order = "ApplyID desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT count(*)OVER(PARTITION BY null) totalCount,fuo.OtherFromID ApplyID,fuo.CourseName ApplyContent,fuo.ApplyDateTime ApplyTime
,fuo.GettScore,fuo.GetCPAScore,0 typeForm FROM Free_UserApplyOtherForm  fuo
LEFT JOIN Free_OtherFroms foa ON fuo.OtherFromID=foa.ID
WHERE  fuo.Status=1 AND fuo.ApproveStatus=1 AND fuo.DepApproveStatus=1
AND fuo.IsDelete=0  AND 1=1   
AND {1}
 
)t  order by {0}", order, where);
                return conn.Query<FreeUseScore>(sql).ToList();
            }
        }

     

        #endregion

       
    }
}
