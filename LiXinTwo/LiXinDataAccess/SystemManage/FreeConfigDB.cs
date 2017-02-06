using LiXinModels.SystemManage;
using Retech.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.SystemManage
{
    public class FreeConfigDB : BaseRepository
    {
        #region 其他形式-其他项目

        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertFree_OtherApplyConfig(Free_OtherApplyConfig model)
        {

            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Free_OtherApplyConfig(Memo,year,CreateTime,IsDelete,ApplyType,ApplyContent,ConvertType,ConvertBase,ConvertBaseInfo,ConvertBaseTo,UploadTip,ConvertMax,TrainGradeScore,MemoTip)
	                     values( @Memo,@year,@CreateTime,@IsDelete,@ApplyType,@ApplyContent,@ConvertType,@ConvertBase,@ConvertBaseInfo,@ConvertBaseTo,@UploadTip,@ConvertMax,@TrainGradeScore,@MemoTip);SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.Memo,
                    model.year,
                    model.CreateTime,
                    model.IsDelete,
                    model.ApplyType,
                    model.ApplyContent,
                    model.ConvertType,
                    model.ConvertBase,
                    model.ConvertBaseInfo,
                    model.ConvertBaseTo,
                    model.UploadTip,
                    model.ConvertMax,
                    model.TrainGradeScore,
                    model.MemoTip
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.ID = decimal.ToInt32(model.ID);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateFree_OtherApplyConfig(Free_OtherApplyConfig model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update Free_OtherApplyConfig set 		                     		                     
		                       Memo = @Memo , 		                     
		                       year = @year , 		     		                     
		                       ApplyContent = @ApplyContent , 		                     
		                       ConvertType = @ConvertType , 		                     
		                       ConvertBase = @ConvertBase , 		                     
		                       ConvertBaseInfo = @ConvertBaseInfo , 		                     
		                       ConvertBaseTo = @ConvertBaseTo , 		                     
		                       UploadTip = @UploadTip , 		                     
		                       ConvertMax = @ConvertMax , 		                     
		                       TrainGradeScore = @TrainGradeScore,
                               MemoTip=@MemoTip   
		                   where ID=@ID";

                var param = new
                {
                    model.ID,
                    model.Memo,
                    model.year,
                    model.ApplyContent,
                    model.ConvertType,
                    model.ConvertBase,
                    model.ConvertBaseInfo,
                    model.ConvertBaseTo,
                    model.UploadTip,
                    model.ConvertMax,
                    model.TrainGradeScore,
                    model.MemoTip
                };
                conn.Execute(strSql, param);
            }

        }


        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        public void DeleteFree_OtherApplyConfig(string IDlist)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"update Free_OtherApplyConfig
                         SET IsDelete=1
                        WHERE   ID in (" + IDlist + ")";
                conn.Execute(sql);
            }
        }



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Free_OtherApplyConfig GetModel(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select *  from Free_OtherApplyConfig  
			                   where {0}", where);


                return conn.Query<Free_OtherApplyConfig>(sql).FirstOrDefault();
            }

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<Free_OtherApplyConfig> GetFreeOtherList(string where = "1=1", int startIndex = 1, int startLength = int.MaxValue)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT row_number()OVER(ORDER BY  CASE WHEN ApplyType=2 THEN -1 ELSE ApplyType END asc, CreateTime desc) number,count(*)OVER(PARTITION BY null) totalcount,*
FROM Free_OtherApplyConfig
WHERE IsDelete=0 and {0}
)t WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Free_OtherApplyConfig>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<Free_OtherApplyConfig> GetFreeOtherList_New(string where = "1=1")
        {
            try
            {
                using (var conn = OpenConnection())
                {
                    var sql = string.Format(@"SELECT * FROM (
SELECT row_number()OVER(ORDER BY CreateTime desc) number,count(*)OVER(PARTITION BY null) totalcount,*
FROM Free_OtherApplyConfig
WHERE 1=1 and {0}
)t ORDER BY ID", where);//IsDelete=0 如果配置删除了（假删） 在修改已删除配置的申请内容 无法找到 把isdelete去掉

                    return conn.Query<Free_OtherApplyConfig>(sql).ToList();
                }
            }
            catch 
            {

                return new List<Free_OtherApplyConfig>();
            }
           
        }
        #endregion

        #region 免修配置-免修项目
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertFree_ApplyConfig(Free_ApplyConfig model)
        {

            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Free_ApplyConfig(IsDelete,FreeName,UploadTip,ApplyType,CPAFreeScore,TogetherFreeScore,Memo,year,CreateTime,MemoTip)
	                     values( @IsDelete,@FreeName,@UploadTip,@ApplyType,@CPAFreeScore,@TogetherFreeScore,@Memo,@year,@CreateTime,@MemoTip);
                         SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.IsDelete,
                    model.FreeName,
                    model.UploadTip,
                    model.ApplyType,
                    model.CPAFreeScore,
                    model.TogetherFreeScore,
                    model.Memo,
                    model.year,
                    model.CreateTime,
                    model.MemoTip
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.ID = decimal.ToInt32(model.ID);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateFree_ApplyConfig(Free_ApplyConfig model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update Free_ApplyConfig set 		                     		                     
		                       IsDelete = @IsDelete , 		                     
		                       FreeName = @FreeName , 		                     
		                       UploadTip = @UploadTip , 		                     
		                       ApplyType = @ApplyType , 		                     
		                       CPAFreeScore = @CPAFreeScore , 		                     
		                       TogetherFreeScore = @TogetherFreeScore , 		                     
		                       Memo = @Memo , 		                     
		                       year = @year ,
                               MemoTip=@MemoTip  
		                   where ID=@ID";

                var param = new
                {
                    model.ID,
                    model.IsDelete,
                    model.FreeName,
                    model.UploadTip,
                    model.ApplyType,
                    model.CPAFreeScore,
                    model.TogetherFreeScore,
                    model.Memo,
                    model.year,
                    model.MemoTip
                };
                conn.Execute(strSql, param);
            }

        }

        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        public void DeleteFree_ApplyConfig(string IDlist)
		{
		    using (var conn = OpenConnection())
            {
                var str = @"Update Free_ApplyConfig
                         SET IsDelete=1
                        WHERE   ID in (" + IDlist + ")";
                conn.Execute(str);    
            }
		}


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Free_ApplyConfig GetFree_ApplyConfig(string where = "1=1")
        {
            using (var conn = OpenConnection())
            {
               var sql =string.Format( @"select *  from Free_ApplyConfig  
			                    where {0}", where);

                return conn.Query<Free_ApplyConfig>(sql).FirstOrDefault();
            }

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<Free_ApplyConfig> GetFreeApplyList(string where = "1=1", int startIndex = 1, int startLength = int.MaxValue)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT row_number()OVER(ORDER BY CreateTime desc) number,count(*)OVER(PARTITION BY null) totalcount,*
FROM Free_ApplyConfig
WHERE IsDelete=0  and {0}
)t WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Free_ApplyConfig>(sql, param).ToList();
            }
        }


        public List<Free_UserOtherApply> GetReport_Free_CPA(string where)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT Sys_User.Realname as Realname,Username,Sys_User.TrainGrade as TrainGrade,Sys_User.DeptName as DeptName ,
Sys_User.CPA AS CPA,Sys_User.CPANo as CPANo,
Sys_User.DeptId as DeptId,
Free_UserOtherApply.ApplyID,
Free_ApplyConfig.ApplyType,
Free_ApplyConfig.FreeName as ApplyName_New,
Free_UserOtherApply.GettScore,Free_UserOtherApply.GetCPAScore,Free_UserOtherApply.typeForm
 FROM Free_UserOtherApply
   LEFT JOIN Sys_User ON Free_UserOtherApply.ApplyUserID=Sys_User.UserId
    LEFT JOIN Free_ApplyConfig ON Free_UserOtherApply.ApplyID=Free_ApplyConfig.ID
   	WHERE ApplyUserID>0 and Free_UserOtherApply.ApplyType=2 AND Free_UserOtherApply.Status=1 AND Free_UserOtherApply.IsDelete=0
			AND (Free_UserOtherApply.typeForm=0 OR Free_UserOtherApply.typeForm=1 OR Free_UserOtherApply.typeForm=4)			
			AND Free_UserOtherApply.CreateUserID =0 AND Free_UserOtherApply.DepApproveStatus=1 and {0}", where);

                return conn.Query<Free_UserOtherApply>(sql).ToList();
            }
        }
        #endregion
    }
}
