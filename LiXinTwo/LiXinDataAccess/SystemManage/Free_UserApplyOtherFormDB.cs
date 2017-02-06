using Retech.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;
using System.Data;
using Retech.Data;

namespace LiXinDataAccess.SystemManage
{
    public class Free_UserApplyOtherFormDB : BaseRepository
    {
        #region 基础方法
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertFree_UserApplyOtherForm(Free_UserApplyOtherForm model)
        {

            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Free_UserApplyOtherForm(ApplyDateTime,ApplyUserID,Status,ApproveStatus,DepApproveStatus,IsDelete,GettScore,GetCPAScore,DepTrainReason,DepReason,OtherFromID,Year,CourseName,Address,StartTime,EndTime,TogetherScore,CPAScore)
	                     values( @ApplyDateTime,@ApplyUserID,@Status,@ApproveStatus,@DepApproveStatus,@IsDelete,@GettScore,@GetCPAScore,@DepTrainReason,@DepReason,@OtherFromID,@Year,@CourseName,@Address,@StartTime,@EndTime,@TogetherScore,@CPAScore);SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.ApplyDateTime,
                    model.ApplyUserID,
                    model.Status,
                    model.ApproveStatus,
                    model.DepApproveStatus,
                    model.IsDelete,
                    model.GettScore,
                    model.GetCPAScore,
                    model.DepTrainReason,
                    model.DepReason,
                    model.OtherFromID,
                    model.Year,
                    model.CourseName,
                    model.Address,
                    model.StartTime,
                    model.EndTime,
                    model.TogetherScore,
                    model.CPAScore
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.ID = decimal.ToInt32(list.ID);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateFree_UserApplyOtherForm(Free_UserApplyOtherForm model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update Free_UserApplyOtherForm set 		                     		                     		                     
		                       Status = @Status , 		                     
		                       ApproveStatus = 0 , 		                     
		                       DepApproveStatus =0 , 		                     
		                       IsDelete = 0 , 		                     
		                       GettScore = 0 , 		                     
		                       GetCPAScore =0 , 		                     
		                       DepTrainReason ='' , 		                     
		                       DepReason = '' ,		                     
		                       CourseName = @CourseName , 		                     
		                       Address = @Address , 		                     
		                       StartTime = @StartTime , 		                     
		                       EndTime = @EndTime , 		                     
		                       TogetherScore = @TogetherScore , 		                     
		                       CPAScore = @CPAScore,
                               ApplyDateTime=@ApplyDateTime   
		                   where ID=@ID";

                var param = new
                {
                    model.Status,
                    model.CourseName,
                    model.Address,
                    model.StartTime,
                    model.EndTime,
                    model.TogetherScore,
                    model.CPAScore,
                    model.ApplyDateTime,
                    model.ID
                };
                conn.Execute(strSql, param);
            }

        }


        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        public void DeleteFree_UserApplyOtherForm(string IDlist)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"Updatet  Free_UserApplyOtherForm
                         SET IsDelete=1
                        WHERE   ID in (" + IDlist + ")";
                conn.Execute(sql);
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<Free_UserApplyOtherForm> GetFree_UserApplyOtherForm(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select * from Free_UserApplyOtherForm  
			                   where {0}", where);

                return conn.Query<Free_UserApplyOtherForm>(sql).ToList();
            }

        }
        #endregion

        /// <summary>
        /// 获取其他有组织形式
        /// </summary>
        /// <returns></returns>
        public List<Free_UserApplyOtherForm> GetFree_UserApplyOtherForm(string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyDateTime desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT *
FROM Free_UserApplyOtherForm
WHERE {0} ", where);

                return conn.Query<Free_UserApplyOtherForm>(sql).ToList();
            }
        }



        /// <summary>
        /// 获取其他人审批通过的项目
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Free_UserApplyOtherForm> GetOtherPassForm(int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT  row_number()OVER(ORDER BY ID) number,count(1)OVER(PARTITION BY null) totalCount , ID,CourseName, Address, StartTime, EndTime, TogetherScore, CPAScore,OtherFromID FROM Free_UserApplyOtherForm
WHERE ApproveStatus=1 AND DepApproveStatus=1 
and {0} ) t where number between @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Free_UserApplyOtherForm>(sql, param).ToList();
            }
        }




        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int UpdateBywhere(int id, string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"UPDATE Free_UserApplyOtherForm
SET {0}
WHERE ID={1}", where, id);
                return conn.Execute(sql);
            }
        }


        /// <summary>
        /// 获得其他有组织形式
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Free_UserApplyOtherForm> GetReport_UserApplyOtherForm(string where)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
SELECT Username,Sys_User.Realname as Realname,Sys_User.TrainGrade as TrainGrade,Sys_User.DeptName as DeptName ,
Sys_User.CPA AS CPA,Sys_User.CPANo as CPANo,
Sys_User.DeptId as DeptId,
Free_UserApplyOtherForm.ID,
Free_UserApplyOtherForm.OtherFromID,
Free_UserApplyOtherForm.GettScore,
Free_UserApplyOtherForm.GetCPAScore,
Free_UserApplyOtherForm.CourseName
 FROM Free_UserApplyOtherForm 
   LEFT JOIN Sys_User ON Free_UserApplyOtherForm.ApplyUserID=Sys_User.UserId
   	LEFT JOIN Free_OtherFroms ON Free_UserApplyOtherForm.OtherFromID=Free_OtherFroms.ID
   	 WHERE Free_UserApplyOtherForm.Status=1 AND Free_UserApplyOtherForm.IsDelete=0 AND Free_UserApplyOtherForm.DepApproveStatus=1 and {0}", where);

                return conn.Query<Free_UserApplyOtherForm>(sql).ToList();
            }
        }

        /// <summary>
        /// 课程名称是否重复
        /// </summary>
        /// <param name="FreeName"></param>
        /// <returns></returns>
        public int GetExistFreeName(string FreeName, int ID = 0)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT count(1) count FROM Free_UserApplyOtherForm
WHERE CourseName='{0}' AND IsDelete=0 and {1}", FreeName, (ID == 0 ? " 1=1 " : " ID!=" + ID));
                return conn.Query<int>(sql).FirstOrDefault();
            }
        }

    }
}
