using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.PlanManage;

namespace LiXinDataAccess.PlanManage
{
    public class Tr_YearPlanDB : BaseRepository
    {
        /// <summary>
        /// 判断 年度计划 是否存在
        /// </summary>
        /// <param name="YearName">年度计划名陈</param>
        /// <returns></returns>
        public bool Exists(int YearName)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select count(1) from Tr_YearPlan WHERE IsDelete = 0 and Year = @YearName";
                var param = new { YearName = YearName };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count < 1;
            }
        }
        
        /// <summary>
        /// 获取计划的基本信息
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Tr_YearPlan> GetYearPlanList(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (SELECT row_number()OVER(ORDER BY Year asc) number,count(*)OVER(PARTITION BY null) totalcount,t0.Id,t0.Year,t0.LastUpdateTime,t0.UserId,t0.PublishFlag,t0.IsDelete,t1.Realname,courseCount=(SELECT count(1) FROM Tr_YearPlanCourse WHERE YearId=Id) FROM Tr_YearPlan AS t0,Sys_User AS t1 where t0.UserID=t1.UserId and " + where + " ) result WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex");
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Tr_YearPlan>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        public void AddYearPlan(Tr_YearPlan model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO Tr_YearPlan(Year,UserID,LastUpdateTime,PublishFlag,IsDelete) VALUES (@Year,@UserID,@LastUpdateTime,@PublishFlag,@IsDelete)SELECT @@IDENTITY AS Id";
                var param = new
                {
                    model.Year,
                    model.UserID,
                    model.LastUpdateTime,
                    model.PublishFlag,
                    model.IsDelete
                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateYearPlan(Tr_YearPlan model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Tr_YearPlan SET Year = @Year,UserID = @UserID,LastUpdateTime=@LastUpdateTime,PublishFlag = @PublishFlag,IsDelete = @IsDelete WHERE Id=@Id";
                var param = new
                {
                    model.Year,
                    model.UserID,
                    model.LastUpdateTime,
                    model.PublishFlag,
                    model.IsDelete,
                    model.Id
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteYearPlan(string Ids)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"update Tr_YearPlan set IsDelete = 1 where Id in (" + Ids + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        public Tr_YearPlan GetYearPlan(int Id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select t0.Id,t0.Year,t0.LastUpdateTime,t0.UserId,t0.PublishFlag,t1.Realname,courseCount=(SELECT count(1) FROM Tr_YearPlanCourse WHERE YearId=Id) from Tr_YearPlan AS t0,Sys_User AS t1 where t0.UserID=t1.UserId and t0.Id=@Id and t0.IsDelete=0";
                return conn.Query<Tr_YearPlan>(sqlstr, new { Id = Id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        public Tr_YearPlan GetYearPlanByYear(int name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select t0.Id,t0.Year,t0.LastUpdateTime,t0.UserId,t0.PublishFlag from Tr_YearPlan AS t0 where t0.Year=@Year and t0.IsDelete=0";
                return conn.Query<Tr_YearPlan>(sqlstr, new { Year = name }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Tr_YearPlan> GetYearList(string where = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select * from Tr_YearPlan where " + where;
                return conn.Query<Tr_YearPlan>(sql).ToList();
            }
        }

        /// <summary>
        /// 获得最大年度
        /// </summary>
        public List<int> GetYear()
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "SELECT Year FROM Tr_YearPlan WHERE IsDelete=0";
                return conn.Query<int>(sqlstr).ToList();
            }
        }


        /// <summary>
        /// 获得所有年度
        /// </summary>
        public List<int> GetALLDepYear()
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = @"SELECT DISTINCT Year FROM(
SELECT Year FROM Tr_YearPlan WHERE IsDelete=0
UNION
SELECT Year FROM Dep_YearPlan WHERE IsDelete=0
) t";
                return conn.Query<int>(sqlstr).ToList();
            }
        }

    }
}
