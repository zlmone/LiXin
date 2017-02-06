using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseLearn;
using Retech.Core;
using System.Data;
using Retech.Data;

namespace LiXinDataAccess.CourseLearn
{
    public class Cl_CourseAdviceDB : BaseRepository
    {
        public List<Cl_CourseAdvice> GetList(string strWhere = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {                
                if (strWhere == "")
                {
                    strWhere = "1=1";
                }
                string selectSql = @"SELECT tab.*,sys_user.Realname as userrealname,sys_user.photourl as userphotourl FROM (select Cl_CourseAdvice.*,sys_user.Realname AS TeacherName from Cl_CourseAdvice left join Co_Course on Co_Course.Id = Cl_CourseAdvice.CourseId left join sys_user on  sys_user.userID = Co_Course.Teacher where " + strWhere + " and Cl_CourseAdvice.IsDelete = 0) as tab left join sys_user on  sys_user.userID = tab.UserId";
                return connection.Query<Cl_CourseAdvice>(selectSql).ToList();
            }
        }


        public Cl_CourseAdvice Get(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = "select * from Cl_CourseAdvice where id=@id";
                return connection.Query<Cl_CourseAdvice>(sqlstr, new { id = Id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取会员对课程是否提交
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int GetCl_CourseAdviceCount(int CourseId,int UserId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = "select count(1) from Cl_CourseAdvice where id=" + CourseId + " and UserId="+UserId;
                return connection.Query<int>(sqlstr).FirstOrDefault();
            }
        }

        /// <summary>
        /// 添加课前建议
        /// </summary>
        /// <param name="model"></param>
        public void SubmitCl_CourseAdvice(Cl_CourseAdvice model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "insert into Cl_CourseAdvice (CourseId,UserId,AdviceContent,AdviceTime,VisibleFlag,IsDelete) values(@CourseId,@UserId,@AdviceContent,@AdviceTime,@VisibleFlag,@IsDelete)";
                var param = new
                    {
                        model.CourseId,
                        model.UserId,
                        model.AdviceContent,
                        model.AdviceTime,
                        model.VisibleFlag,
                        model.IsDelete

                    };
                decimal id = connection.Query<decimal>(sql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);

            }
        }

        /// <summary>
        /// 回复课前建议
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ReplyCl_CourseAdvice(Cl_CourseAdvice model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "update Cl_CourseAdvice set AnserUserId=@AnserUserId,AnserContent=@AnserContent,AnserTime=@AnserTime,VisibleFlag=@VisibleFlag where CourseId=@CourseId and Id=@Id ";
                var param = new
                    {
                        model.AnserUserId,
                        model.AnserContent,
                        model.AnserTime,
                        model.CourseId,
                        model.VisibleFlag,
                        model.Id
                    };

                int result = connection.Execute(sql, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除课前建议
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"update Cl_CourseAdvice set IsDelete=1 where id = {0}", id);
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }


    }
}
