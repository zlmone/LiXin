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
    public class New_CoursePaperDB : BaseRepository
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        public void AddCoursePaper(New_CoursePaper model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO New_CoursePaper (CourseId,PaperId,Length,Hour,TotalScore,LevelScore,TestTimes) VALUES (@CourseId,@PaperId,@Length,@Hour,@TotalScore,@LevelScore,@TestTimes)SELECT @@IDENTITY AS Id";
                var param = new
                {
                    model.CourseId,
                    model.PaperId,
                    model.Length,
                    model.Hour,
                    model.TotalScore,
                    model.LevelScore,
                    model.TestTimes
                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 根据课程ID删除课程试卷
        /// </summary>
        /// <param name="courseID">课程ID</param>
        public bool DeleteCoursePaper(int courseID)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere = @"delete New_CoursePaper where CourseId=@courseID";
                var result = conn.Execute(sqlwhere, new { courseID });
                return result > 0;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateCoursePaper(New_CoursePaper model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update New_CoursePaper SET CourseId = @CourseId,PaperId = @PaperId,Length = @Length,Hour = @Hour,TotalScore = @TotalScore,LevelScore = @LevelScore,TestTimes = @TestTimes WHERE Id=@Id";
                var param = new
                {
                    model.CourseId,
                    model.PaperId,
                    model.Length,
                    model.Hour,
                    model.TotalScore,
                    model.LevelScore,
                    model.TestTimes,
                    model.Id
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据课程获取关联的试卷
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <returns>执行结果</returns>
        public List<New_CoursePaper> GetCoursePaper(int courseID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select * from New_CoursePaper where CourseId={0}", courseID);
                return conn.Query<New_CoursePaper>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取单个考试信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public New_CoursePaper GetSingleCoursePaper(int courseID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select * from New_CoursePaper where CourseId={0}", courseID);
                return conn.Query<New_CoursePaper>(sql).FirstOrDefault();
            }
        }

    }
}
