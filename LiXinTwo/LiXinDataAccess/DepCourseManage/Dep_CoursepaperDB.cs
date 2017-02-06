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
    public class Dep_CoursepaperDB : BaseRepository
    {
        /// <summary>
        /// 获取课程关联的 主试卷
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public Dep_Coursepaper GetCo_CourseMainPaper(int CourseId)
        {
            using (var conn = OpenConnection())
            {
                string sql = "select * from Dep_Coursepaper where IsMain=0 AND CourseId=" + CourseId;
                return conn.Query<Dep_Coursepaper>(sql).FirstOrDefault();
            }
        }


        public bool IsExistCourseMainPaper(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return
                    connection.Query<int>("select count(1) from Dep_Coursepaper where IsMain=0 AND courseId=" + courseId)
                              .First() > 0;
            }
        }


        public bool UpdateCourseMainPaper(Dep_Coursepaper model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Dep_Coursepaper set PaperId=@PaperId,Length=@Length,
               Hour=@Hour,LevelScore=@LevelScore,TestTimes=@TestTimes where IsMain=0 AND CourseId=@CourseId";

                var param = new
                {
                    model.PaperId,
                    model.Length,
                    model.Hour,
                    model.LevelScore,
                    model.TestTimes,
                    model.CourseId
                };
                return conn.Execute(sqlwhere, param) > 0;
            }
        }


        public bool AddCoursePaper(Dep_Coursepaper model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Dep_Coursepaper (PaperId,CourseId,Length,Hour,LevelScore,TestTimes,IsMain)
values (@PaperId,@CourseId,@Length,@Hour,@LevelScore,@TestTimes,@IsMain)";
                var param = new
                {
                    model.PaperId,
                    model.CourseId,
                    model.Length,
                    model.Hour,
                    model.LevelScore,
                    model.TestTimes,
                    model.IsMain
                };
                return conn.Execute(sqlwhere, param) > 0;
            }
        }
    }
}
