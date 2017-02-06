using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LiXinModels.DeptCourseManage;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.DeptCourseManage
{
    public class DeptCourseResourceDB : BaseRepository
    {
        /// <summary>
        /// 获得课程相关资源和附件
        /// </summary>
        /// <param name="Id">课程ID</param>
        /// <returns></returns>
        public Dept_CourseResource GetCo_CourseResource(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = "select * from Co_CourseResource where Id=@Id";
                var param = new
                {
                    Id
                };
                return connection.Query<Dept_CourseResource>(sqlwhere, param).FirstOrDefault();

            }
        }

        /// <summary>
        /// 获得课程相关资源和附件列表
        /// </summary>
        /// <param name="CourseId">课程ID</param>
        /// <returns></returns>
        public List<Dept_CourseResource> GetList(int CourseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "select * from Co_CourseResource where IsDelete=0 and ResourceType=1 AND CourseId=" + CourseId;
                return connection.Query<Dept_CourseResource>(sql).ToList();
            }
        }

        /// <summary>
        /// 根据课程编号 获取课程关联的资源及附件
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Dept_CourseResource> GetCourseResourceList(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string selectSql = "select * from Co_CourseResource where IsDelete=0 and ResourceType=0 AND courseId=" + courseId;
                return connection.Query<Dept_CourseResource>(selectSql).ToList();
            }

        }

        /// <summary>
        /// 新增课程关联资源
        /// </summary>
        /// <param name="model"></param>
        public bool AddCourseResource(Dept_CourseResource model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Co_CourseResource (CourseId,RealName,ResourceName,ResourceSize,ResourceType,LastUpdateTime,IsDelete,PackId,Flag)
values (@CourseId,@RealName,@ResourceName,@ResourceSize,@ResourceType,@LastUpdateTime,@IsDelete,@PackId,@Flag)";
                var param = new
                {
                    model.CourseId,
                    model.RealName,
                    model.ResourceName,
                    model.ResourceSize,
                    model.ResourceType,
                    model.Flag,
                    LastUpdateTime=DateTime.Now,
                    IsDelete=0,
                    model.PackId
                };
                return conn.Execute(sqlwhere, param) > 0;
            }
        }

        /// <summary>
        /// 删除课程已经关联资源
        /// </summary>
        /// <param name="courseId"></param>
        public bool DeleteCourseResource(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return
                    connection.Execute("Update   Co_CourseResource set isDelete=1 where Id=" + courseId)> 0;
            }
        }


        /// <summary>
        /// 查找课程下的资源ID 和对应学习的状态
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public List<Dept_CourseResource> GetVedioList(int courseid, int Userid)
        {
            using (IDbConnection connection = OpenConnection())
            {
                //string selectSql = " select * from Co_CourseResource left join Cl_LearnVideoInfor on Cl_LearnVideoInfor.ResourseId=Co_CourseResource.Id  where Co_CourseResource.CourseId=" + courseid + " and Co_CourseResource.IsDelete=0";
                //string selectSql = " select * from Co_CourseResource  where Co_CourseResource.CourseId=" + courseid + " and Co_CourseResource.IsDelete=0";
                string query = string.Format(@"
select 
	Co_CourseResource.ResourceName,
	Co_CourseResource.RealName,
    Co_CourseResource.CourseID,
	Cl_LearnVideoInfor.Progress,
	Cl_LearnVideoInfor.LearnTimes,
Cl_VideoManage.IsDelete as VideoManageIsDelete,
Co_Course.id,
Co_CourseResource.ResourceType,
Co_CourseResource.Id as CourseResourceId,
	Co_Course.IsPing
    from Co_CourseResource  
   left join Cl_CpaLearnStatus on Cl_CpaLearnStatus.CourseID=Co_CourseResource.CourseId and  Cl_CpaLearnStatus.UserID={1} and Cl_CpaLearnStatus.CpaFlag=0  
   left join Cl_LearnVideoInfor on Cl_CpaLearnStatus.Id=Cl_LearnVideoInfor.LearnId and Co_CourseResource.id=[Cl_LearnVideoInfor].[ResourseId]
   left join Co_Course on Co_Course.Id=Co_CourseResource.CourseId  
   left join Cl_VideoManage on Co_CourseResource.PackId=Cl_VideoManage.Id
   where Co_CourseResource.CourseId={0} 
   and Co_CourseResource.IsDelete=0   and Co_CourseResource.ResourceType in(3,4)
", courseid, Userid);

                return connection.Query<Dept_CourseResource>(query).ToList();
            }
        }
    }
}
