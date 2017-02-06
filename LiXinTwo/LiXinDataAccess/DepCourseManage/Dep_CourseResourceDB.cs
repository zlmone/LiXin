using LiXinModels.DepCourseManage;
using Retech.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Data;
using System.Data;

namespace LiXinDataAccess.DepCourseManage
{
    public class Dep_CourseResourceDB : BaseRepository
    {
        /// <summary>
        /// 根据课程编号 获取课程关联的资源及附件
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Dep_CourseResource> GetCourseResourceList(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string selectSql = "select * from Dep_CourseResource where IsDelete=0 AND courseId=" + courseId;
                return connection.Query<Dep_CourseResource>(selectSql).ToList();
            }

        }

        public bool DeleteCourseResource(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return
                    connection.Execute("Update   Dep_CourseResource set isDelete=1 where Id=" + courseId) > 0;
            }
        }



        /// <summary>
        /// 获得课程相关资源和附件列表
        /// </summary>
        /// <param name="CourseId">课程ID</param>
        /// <returns></returns>
        public List<Dep_CourseResource> GetList(int CourseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "select * from Dep_CourseResource where IsDelete=0 AND CourseId=" + CourseId;
                return connection.Query<Dep_CourseResource>(sql).ToList();
            }
        }


        /// <summary>
        /// 新增课程关联资源
        /// </summary>
        /// <param name="model"></param>
        public bool AddCourseResource(Dep_CourseResource model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Dep_CourseResource (CourseId,RealName,ResourceName,ResourceSize,ResourceType,LastUpdateTime,IsDelete,PackId,Flag)
values (@CourseId,@RealName,@ResourceName,@ResourceSize,@ResourceType,@LastUpdateTime,@IsDelete,@PackId,@Flag)";
                var param = new
                {
                    model.CourseId,
                    model.RealName,
                    model.ResourceName,
                    model.ResourceSize,
                    model.ResourceType,
                    model.Flag,
                    LastUpdateTime = DateTime.Now,
                    IsDelete = 0,
                    model.PackId
                };
                return conn.Execute(sqlwhere, param) > 0;
            }
        }
    }
}
