using System.Collections.Generic;
using System.Linq;
using Retech.Core;
using Retech.Data;
using System.Data;
using LiXinModels.DepCourseLearn;

namespace LiXinDataAccess.DepCourseLearn
{
    public class Dep_CpaLearnStatusDB : BaseRepository
    {
        /// <summary>
        /// 新增一条数据
        /// </summary>
        public void InsertDepCPALearn(Dep_CpaLearnStatus model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"insert into Dep_CpaLearnStatus( CourseID,UserID,GetLength,LastUpdateTime,CpaFlag,DepartSetId) values (@CourseId,@UserId,@GetLength,getdate(),@CpaFlag,@DepartSetId)";
                var param = new
                {
                    model.CourseID,
                    model.UserID,
                    model.GetLength,
                    model.CpaFlag,
                    model.DepartSetId
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
        public bool UpdateDepCPALearn(Dep_CpaLearnStatus model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Dep_CpaLearnStatus SET CourseID = @CourseID,
	UserID = @UserID,
	GetLength = @GetLength,
	LastUpdateTime = getdate(),
    DepartSetId=@DepartSetId,
	CpaFlag = @CpaFlag WHERE Id=@Id";
                var param = new
                {
                    model.CourseID,
                    model.UserID,
                    model.GetLength,
                    model.DepartSetId,
                    model.CpaFlag,
                    model.Id
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sql">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateDepCPALearn(string sql)
        {
            using (var conn = OpenConnection())
            {
                var result = conn.Execute(sql);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据课程和人员来查找数据
        /// </summary>
        /// <param name="CourseId">课程ID</param>
        /// <param name="UserId">用户ID</param>  
        /// <returns></returns>
        public Dep_CpaLearnStatus GetDepCPALearnByCourseId(int CourseId, int UserId, string whereStr = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from Dep_CpaLearnStatus where CourseID=" + CourseId + " and UserID=" + UserId + " and " + whereStr;
                return conn.Query<Dep_CpaLearnStatus>(sqlstr).FirstOrDefault();
            }
        }

        

        /// <summary>
        /// 获取查询集合
        /// </summary>
        /// <param name="wherestr">查询条件</param>
        public List<Dep_CpaLearnStatus> GetDepCPALearn(string wherestr = "1=1")
        {
            using (var conn = OpenConnection())
            {
                var sqlwhere = string.Format("select * from Dep_CpaLearnStatus where "+wherestr);
                return conn.Query<Dep_CpaLearnStatus>(sqlwhere).ToList();
            }
        }

    }
}
