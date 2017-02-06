using LiXinModels.NewQueryStatistics;
using Retech.Core;
using Retech.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LiXinDataAccess.NewQueryStatistics
{
    public class TeacherDB : BaseRepository
    {
        /// <summary>
        /// 讲师评价
        /// </summary>
        /// <returns></returns>
        public List<TeacherStatistics> GetTeacherStatisticsList(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  UserId desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH courseInfo AS 
(
	SELECT Id, StartTime, EndTime,Teachers FROM New_Course
	WHERE  IsDelete=0 AND  PublicFlag=1
)
SELECT * FROM (
SELECT row_number()OVER(ORDER BY  {0}) number,count(*)OVER(PARTITION BY null) totalcount, UserId,Realname, Sex,IsTeacher,
courseCount=(SELECT COUNT(*) FROM courseInfo where (SELECT COUNT(*) FROM (SELECT ID FROM dbo.F_SplitIDs(Teachers))t WHERE   t.ID=UserId)>0),
endCourseCount=(SELECT COUNT(*) FROM courseInfo where EndTime<getdate() AND (SELECT COUNT(*) FROM (SELECT ID FROM dbo.F_SplitIDs(Teachers))t WHERE   t.ID=UserId)>0),
SumScore=0
 FROM Sys_User
WHERE IsTeacher>0  AND Status=0 AND IsDelete=0 and  {1}
) r
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", jsRenderSortField, where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<TeacherStatistics>(sql, param).ToList();
            }

        }

        /// <summary>
        /// 讲师评价详情
        /// </summary>
        /// <returns></returns>
        public List<CourseStatistics> GetCourseStatistics(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  Id desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT row_number()OVER(ORDER BY {0}) number,count(*)OVER(PARTITION BY null) totalcount, Id,CourseName, StartTime, EndTime,
NumberofCourse=0,
NumberofStatistics=0,
Sumscore=0
FROM New_Course
WHERE PublicFlag=1 and {1}) t
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex                                         
", jsRenderSortField, where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<CourseStatistics>(sql, param).ToList();

            }
        }


    }
}
