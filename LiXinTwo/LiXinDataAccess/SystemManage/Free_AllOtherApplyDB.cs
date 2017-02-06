using Retech.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Data;
using LiXinModels.SystemManage;
using System.Data;
using LiXinModels;

namespace LiXinDataAccess.SystemManage
{
    public class Free_AllOtherApplyDB : BaseRepository
    {
        public void AddFree_AllOtherApply(Free_AllOtherApply model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO Free_AllOtherApply(UserIDs,UserApplyID) VALUES (@UserIDs,@UserApplyID)SELECT @@IDENTITY AS ID";
                var param = new
                {
                    model.UserApplyID,
                    model.UserIDs

                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.ID = decimal.ToInt32(id);
            }
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateFree_AllOtherApply(Free_AllOtherApply model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Free_AllOtherApply SET UserIDs = @UserIDs WHERE  UserApplyID = @UserApplyID";
                var param = new
                {
                    model.UserIDs,
                    model.UserApplyID,
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
        public bool DeleteOtherFroms(string id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"Delete Free_AllOtherApply where id in(" + id + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public Free_AllOtherApply GetOtherFromsById(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select * from Free_AllOtherApply where  UserApplyID=" + id;
                return conn.Query<Free_AllOtherApply>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得批量添加的人员数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<dynamic> GetUserIDByOtherFroms(int id)
        {
            using (var conn=OpenConnection())
            {
                var sql =string.Format(@"
 DECLARE @userIDs NVARCHAR(max)
 SELECT @userIDs=userIDs from Free_AllOtherApply where  UserApplyID={0}
SELECT UserId,CPA FROM Sys_User
WHERE UserId IN (SELECT ID FROM dbo.F_SplitIDs(@userIDs))", id);
                return conn.Query<dynamic>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取批量的所有人员数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ShowFreeUserDetails> GetUserDetails(int id)
        {
            using (var conn = OpenConnection())
            {
                var sql =string.Format(@" DECLARE @userIDs NVARCHAR(max)
 SELECT @userIDs=userIDs from Free_AllOtherApply where  UserApplyID={0}
 
SELECT  UserId,Realname,DeptName,TrainGrade,CPA,CPANo,tScore, CPAScore,GettScore, GetCPAScore,syu.Username FROM Sys_User   syu
LEFT JOIN  Free_UserOtherApply  fuo ON syu.UserId=fuo.ApplyUserID AND fromID={0} AND fuo.IsDelete=0
WHERE UserId IN (SELECT ID FROM dbo.F_SplitIDs(@userIDs))", id);
                return conn.Query<ShowFreeUserDetails>(sql).ToList();
            }
        }
        

    }
}
