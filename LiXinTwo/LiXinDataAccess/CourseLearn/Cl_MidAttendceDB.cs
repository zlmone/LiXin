using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.CourseLearn;
using System;

namespace LiXinDataAccess.CourseLearn
{
    public class Cl_MidAttendceDB : BaseRepository
    {
        ///<summary>
        /// 获取记录数
        /// </summary>
        public int Cl_MidAttendceCount(string strWhere)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select count(1) from Cl_MidAttendce";
                if (!string.IsNullOrEmpty(strWhere))
                {
                    if (!strWhere.TrimStart().StartsWith("WHERE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sqlwhere = " WHERE " + strWhere;
                    }
                }
                else
                {
                    strWhere = string.Empty;
                }
                int count = conn.Query<int>(sqlwhere).First();
                return count;
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(Cl_MidAttendce model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO Cl_MidAttendce (CourseId,UserId,CreateTime,Time) VALUES (@courseid,@userid,@createtime,@time) SELECT @@IDENTITY AS Id";
                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.CreateTime,
                    model.Time
                };

                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Cl_MidAttendce model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"UPDATE Cl_MidAttendce SET CourseId = @courseid,UserId = @userid,CreateTime = @createtime,Time = @time WHERE Id=@Id";
                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.CreateTime,
                    model.Time,
                    model.Id

                };
                return conn.Execute(sqlwhere, param) > 0;

            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string deleteSql = "DELETE FROM Cl_MidAttendce WHERE Id = @Id";
                var param = new
                {
                    Id
                };
                return conn.Execute(deleteSql, param) > 0;
            }
        }

        /// <summary>
        /// 删除符合条件的记录
        /// </summary>
        public int Delete(string deleteWhere)
        {
            using (IDbConnection conn = OpenConnection())
            {
                if (!string.IsNullOrEmpty(deleteWhere))
                {
                    if (!deleteWhere.TrimStart().StartsWith("WHERE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        deleteWhere = " WHERE " + deleteWhere;
                    }
                }
                string deleteSql = "DELETE FROM Cl_MidAttendce " + deleteWhere;
                return conn.Execute(deleteSql);
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Cl_MidAttendce GetModel(string where)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from Cl_MidAttendce ";
                if (!string.IsNullOrEmpty(where))
                {
                    if (!where.TrimStart().StartsWith("WHERE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sqlstr += " WHERE " + where;
                    }
                    else
                    {
                        sqlstr += where;
                    }
                }
                return conn.Query<Cl_MidAttendce>(sqlstr).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Cl_MidAttendce> GetList(string strWhere = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select * from Cl_MidAttendce where " + strWhere;
                return conn.Query<Cl_MidAttendce>(sql).ToList();
            }
        }
    }
}
