using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.CourseLearn;
using System;

namespace LiXinDataAccess.CourseLearn
{
    public class Cl_MakeUpOrderDB: BaseRepository
    {
        ///<summary>
        /// 获取记录数
        /// </summary>
        public int Cl_MakeUpOrderCount(string strWhere="1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                //string sqlwhere = "select count(1) from Cl_MakeUpOrder ";
                //if (!string.IsNullOrEmpty(strWhere))
                //{
                //    if (!strWhere.TrimStart().StartsWith("WHERE", StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        sqlwhere = " WHERE " + strWhere;
                //    }else
                //    {
                //        sqlwhere+=strWhere;
                //    }
                //}
                string sqlwhere = string.Format(@"SELECT COUNT(*) FROM View_OrderTimes WHERE {0} ",strWhere);
                int count = conn.Query<int>(sqlwhere).First();
                return count;
            }
        }

        /// <summary>
        /// 增加一条数据(补预订专用)
        /// </summary>
        public void Add(Cl_MakeUpOrder model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO Cl_MakeUpOrder (CourseId,UserId,ApprovalUser,ApprovalMemo,ApprovalFlag,ApprovalDateTime,ApprovalLimitTime,IsTimeOut,LeaveUserID,Name,CourseName,LeaveTime,CourseStartTime,CourseEndTime,AttStartTime,AttEndTime) VALUES (@CourseId,@UserId,@ApprovalUser,@ApprovalMemo,@ApprovalFlag,@ApprovalDateTime,@ApprovalLimitTime,@IsTimeOut,@LeaveUserID,@Name,@CourseName,@LeaveTime,@CourseStartTime,@CourseEndTime,@AttStartTime,@AttEndTime) SELECT @@IDENTITY AS Id";
                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.ApprovalUser,
                    model.ApprovalMemo,
                    model.ApprovalFlag,
                    model.ApprovalDateTime,
                    model.ApprovalLimitTime,
                    model.IsTimeOut,
                    model.LeaveUserID,
                    model.Name,
                    model.CourseName,
                    model.LeaveTime,
                    model.CourseStartTime,
                    model.CourseEndTime,
                    model.AttStartTime,
                    model.AttEndTime
                };

                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }
        /// <summary>
        /// 更新一条数据(补预订专用)
        /// </summary>
        public bool Update(Cl_MakeUpOrder model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"UPDATE Cl_MakeUpOrder SET CourseId=@CourseId,UserId=@UserId,ApprovalUser=@ApprovalUser,ApprovalMemo=@ApprovalMemo,ApprovalFlag=@ApprovalFlag,ApprovalDateTime=@ApprovalDateTime,ApprovalLimitTime=@ApprovalLimitTime,IsTimeOut=@IsTimeOut,LeaveUserID=@LeaveUserID,Name=@Name,CourseName=@CourseName,LeaveTime=@LeaveTime,CourseStartTime=@CourseStartTime,CourseEndTime=@CourseEndTime,AttStartTime=@AttStartTime,AttEndTime=@AttEndTime WHERE Id=@Id";
                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.ApprovalUser,
                    model.ApprovalMemo,
                    model.ApprovalFlag,
                    model.ApprovalDateTime,
                    model.ApprovalLimitTime,
                    model.IsTimeOut,
                    model.LeaveUserID,
                    model.Name,
                    model.CourseName,
                    model.LeaveTime,
                    model.CourseStartTime,
                    model.CourseEndTime,
                    model.AttStartTime,
                    model.AttEndTime
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
                string deleteSql = "DELETE FROM Cl_MakeUpOrder WHERE Id = @Id";
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
                string deleteSql = "DELETE FROM Cl_MakeUpOrder " + deleteWhere;
                return conn.Execute(deleteSql);
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Cl_MakeUpOrder GetModel(string where)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from Cl_MakeUpOrder ";
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
                return conn.Query<Cl_MakeUpOrder>(sqlstr).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Cl_MakeUpOrder> GetList(string strWhere = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"select * from Cl_MakeUpOrder where " + strWhere;
                return conn.Query<Cl_MakeUpOrder>(sql).ToList();
            }
        }

    }
}
