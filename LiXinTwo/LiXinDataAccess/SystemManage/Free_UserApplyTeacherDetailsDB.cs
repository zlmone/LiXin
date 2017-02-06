using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Data;
using Retech.Core;
using System.Data;
using LiXinModels;

namespace LiXinDataAccess.SystemManage
{
    public class Free_UserApplyTeacherDetailsDB : BaseRepository
    {

        #region 个人申请的添加
        
        public bool AddFree_UserApplyTeacherDetails(Free_UserApplyTeacherDetails model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO Free_UserApplyTeacherDetails(userApplyID,ClassName,ConvertScore,IsDelete,tScore,CPAScore) VALUES (@userApplyID,@ClassName,@ConvertScore,@IsDelete,@tScore,@CPAScore)SELECT @@IDENTITY AS ID";
                var param = new
                {
                    //model.ID,
                    model.userApplyID,
                    model.ClassName,
                    model.ConvertScore,
                    model.IsDelete,
                    model.tScore,
                    model.CPAScore
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
        public bool UpdateFree_UserApplyTeacherDetails(Free_UserApplyTeacherDetails model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Free_UserApplyTeacherDetails SET userApplyID = @userApplyID,ClassName = @ClassName,ConvertScore=@ConvertScore,IsDelete=@IsDelete,tScore=@tScore,CPAScore=@CPAScore WHERE Id=@ID";
                var param = new
                {
                    model.ID,
                    model.userApplyID,
                    model.ClassName,
                    model.ConvertScore,
                    model.IsDelete,
                    model.tScore,
                    model.CPAScore
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
        public bool DeleteFree_UserApplyTeacherDetails(string id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"update Free_UserApplyTeacherDetails set IsDelete = 1 where userApplyID in(" + id + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }


        public List<Free_UserApplyTeacherDetails> GetFree_UserApplyTeacherDetailsList(string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select * from Free_UserApplyTeacherDetails where " + where;
                return conn.Query<Free_UserApplyTeacherDetails>(sql).ToList();
            }
        }

        #endregion

        #region 批量

        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertFree_BatchTeacherDetails(Free_BatchTeacherDetails model)
        {

            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Free_BatchTeacherDetails(userApplyID,ClassName,ConvertScore,IsDelete,tScore,CPAScore)
	                     values( @userApplyID,@ClassName,@ConvertScore,@IsDelete,@tScore,@CPAScore);SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.userApplyID,
                    model.ClassName,
                    model.ConvertScore,
                    model.IsDelete,
                    model.tScore,
                    model.CPAScore
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.ID = decimal.ToInt32(list.ID);
            }
        }

        /// <summary>
        /// 得到一个对象实体 
        /// </summary>
        public List<Free_BatchTeacherDetails> GetFree_BatchTeacherDetails(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select * from Free_BatchTeacherDetails  
			                   where {0}", where);

                return conn.Query<Free_BatchTeacherDetails>(sql).ToList();
            }

        }

        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        public void DeleteFree_BatchTeacherDetails(string IDlist)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"DELETE   Free_BatchTeacherDetails
                        WHERE   ID in (" + IDlist + ")";
                conn.Execute(sql);
            }
        }
        #endregion
    }
}
