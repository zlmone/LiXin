using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.NewCourseManage;
using Retech.Core;
using System.Data;
using Retech.Data;

namespace LiXinDataAccess.NewCourseManage
{
    public class New_CourseFilesDB : BaseRepository
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        public void AddCourseFiles(New_CourseFiles model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO New_CourseFiles (CourseId,Name,RealName,CreateDate,LoadTimes,Type,PackId,ResourceSize,IsDelete,Flag) VALUES (@CourseId,@Name,@RealName,@CreateDate,@LoadTimes,@Type,@PackId,@ResourceSize,@IsDelete,@Flag)SELECT @@IDENTITY AS Id";
                var param = new
                {
                    model.CourseId,
                    model.Name,
                    model.RealName,
                    model.CreateDate,
                    model.LoadTimes,
                    model.Type,
                    model.PackId,
                    model.ResourceSize,
                    model.IsDelete,
                    model.Flag
                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 获取单个信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public New_CourseFiles GetSingleCourseFile(int courseID, string wherestr = "1=1")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from New_CourseFiles where CourseId =@CourseId and " + wherestr);
                var param = new { CourseId = courseID };
                return connection.Query<New_CourseFiles>(query, param).FirstOrDefault();

            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateCourseFiles(New_CourseFiles model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update New_CourseFiles SET CourseId = @CourseId,Name = @Name,RealName = @RealName,CreateDate = @CreateDate,LoadTimes = @LoadTimes,Type = @Type,PackId = @PackId,ResourceSize = @ResourceSize,IsDelete = @IsDelete WHERE Id=@Id";
                var param = new
                {
                    model.CourseId,
                    model.Name,
                    model.RealName,
                    model.CreateDate,
                    model.LoadTimes,
                    model.Type,
                    model.PackId,
                    model.ResourceSize,
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
        public bool DeleteCourseFiles(string Ids)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"update New_CourseFiles set IsDelete = 1 where Id in (" + Ids + ")";
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据课程ID找出资源
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<New_CourseFiles> GetCourseResourceList(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string selectSql = "select * from New_CourseFiles where IsDelete=0 AND courseId=" + courseId;
                return connection.Query<New_CourseFiles>(selectSql).ToList();
            }

        }


        public List<New_CourseFiles> GetVedioList(int courseid, int Userid)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string query = string.Format(@"
select 
	New_CourseFiles.Name,
	New_CourseFiles.RealName,
    New_CourseFiles.CourseId,
	New_LearnVideoInfor.Progress,
	New_LearnVideoInfor.LearnTimes,
Cl_VideoManage.IsDelete as VideoManageIsDelete,
New_Course.Id,
New_CourseFiles.Type,
New_CourseFiles.Id as CourseResourceId
	--New_Course.IsPing
    from New_CourseFiles  
   left join New_CpaLearnStatus on New_CpaLearnStatus.CourseID=New_CourseFiles.CourseId and  New_CpaLearnStatus.UserID={1} and New_CpaLearnStatus.CpaFlag=0  
   left join New_LearnVideoInfor on New_CpaLearnStatus.Id=New_LearnVideoInfor.LearnId and New_CourseFiles.id=[New_LearnVideoInfor].[ResourseId]
   left join New_Course on New_Course.Id=New_CourseFiles.CourseId  
   left join Cl_VideoManage on New_CourseFiles.PackId=Cl_VideoManage.Id
   where New_CourseFiles.CourseId={0} 
   and New_CourseFiles.IsDelete=0   and New_CourseFiles.Type in(3,4)
", courseid, Userid);

                return connection.Query<New_CourseFiles>(query).ToList();
            }
        }

        /// <summary>
        /// 修改资源下载次数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateLoadTimes(int id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = " update New_CourseFiles set LoadTimes=LoadTimes+1 where Id=" + id;
                int result = connection.Execute(sql);
                return result > 0;
            }
        }
    }
}
