using System.Collections.Generic;
using System.Linq;
using Retech.Core;
using Retech.Data;
using System.Data;
using LiXinModels.DepPlanManage;

namespace LiXinDataAccess.DeptPlanManage
{
    public class Dep_LinkDepartDB : BaseRepository
    {
        #region 增改(基本信息)
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertDep_LinkDepart(Dep_LinkDepart model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Dep_LinkDepart(YearId,DeptId,ApprovalFlag,ApprovalTime,Reason) VALUES (@YearId,@DeptId,@ApprovalFlag,@ApprovalTime,@Reason);SELECT @@IDENTITY as Id ";

                var param = new
                {
                    model.YearId,
                    model.DeptId,
                    model.ApprovalFlag,
                    model.ApprovalTime,
                    model.Reason
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.Id);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateDep_LinkDepart(Dep_LinkDepart model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update Dep_LinkDepart set YearId = @YearId,DeptId = @DeptId,ApprovalFlag = @ApprovalFlag,ApprovalTime = @ApprovalTime,Reason = @Reason where Id=@Id";

                var param = new
                {
                    model.YearId,
                    model.DeptId,
                    model.ApprovalFlag,
                    model.ApprovalTime,
                    model.Reason,
                    model.Id
                };
                int result = conn.Execute(strSql, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public Dep_LinkDepart GetDep_LinkDepart(int yearID, int deptID)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from Dep_LinkDepart where YearId=@YearId and DeptId=@DeptId ");
                var param = new { YearId = yearID, DeptId = deptID };
                return connection.Query<Dep_LinkDepart>(query, param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<Dep_LinkDepart> GetDep_LinkDepart(int deptID)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from Dep_LinkDepart where  DeptId=@DeptId ");
                var param = new { DeptId = deptID };
                return connection.Query<Dep_LinkDepart>(query, param).ToList();
            }
        }

        /// <summary>
        /// 根据年度ID全部删除
        /// </summary>
        /// <param name="YearIds">年计划ID</param>
        /// <returns>操作状态</returns>
        public bool DeleteAllDepLinkbyYear(string YearIds)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"DELETE FROM Dep_LinkDepart where YearId in (" + YearIds + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }
        #endregion

        /// <summary>
        /// 根据YearId 找出所有上报部门 
        /// </summary>
        /// <param name="yearId"></param>
        /// <returns></returns>
        public List<Dep_LinkDepart> GetDep_LinkDepartByYear(int yearId, int deptid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = "select * from Dep_LinkDepart left join Sys_Department on Dep_LinkDepart.DeptId=Sys_Department.DepartmentId where Dep_LinkDepart.YearId=(select Id from Dep_YearPlan where [Year]=" + yearId + " and deptid=" + deptid + ")";
                return conn.Query<Dep_LinkDepart>(sql).ToList();

            }
        }
    }
}
