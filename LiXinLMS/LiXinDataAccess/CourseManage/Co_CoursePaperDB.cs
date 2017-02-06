using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using System.Data;
using LiXinModels.CourseManage;
namespace LiXinDataAccess.CourseManage
{
    public class Co_CoursePaperDB : BaseRepository
    {
        /// <summary>
        /// 新增课程关联考试
        /// </summary>
        /// <param name="model"></param>
        public bool AddCoursePaper(Co_CoursePaper model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Co_CoursePaper (PaperId,CourseId,Length,Hour,LevelScore,TestTimes,IsMain)
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

        /// <summary>
        /// 课程是否已经关联  主试卷
        /// </summary>
        /// <param name="courseId"></param>
        public bool IsExistCourseMainPaper(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return
                    connection.Query<int>("select count(1) from Co_CoursePaper where IsMain=0 AND courseId=" + courseId)
                              .First() > 0;
            }
        }

        /// <summary>
        /// 修改课程关联的 主试卷
        /// </summary>
        /// <param name="model"></param>
        public bool UpdateCourseMainPaper(Co_CoursePaper model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Co_CoursePaper set PaperId=@PaperId,Length=@Length,
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


        /// <summary>
        /// 获取课程关联的 主试卷
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public Co_CoursePaper GetCo_CourseMainPaper(int CourseId)
        {
            using (var conn = OpenConnection())
            {
                string sql = "select * from Co_CoursePaper where IsMain=0 AND CourseId="+CourseId;
                return conn.Query<Co_CoursePaper>(sql).FirstOrDefault();
            }
        }


        /// <summary>
        /// 修改课程下考试次数
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public bool UpdateTestTimes(int CourseId)
        {
            using (var conn = OpenConnection())
            {
                string sql = "update Co_Coursepaper  set TestTimes=TestTimes+1 where CourseId="+CourseId;

                return conn.Execute(sql) > 0;

            }
        }

    }
}
