using LiXinModels.NewCourseManage;
using Retech.Core;
using System.Collections.Generic;
using System.Data;
using Retech.Data;
using System.Linq;
using System.Text;

namespace LiXinDataAccess.NewCourseManage
{
    public class New_CourseDB : BaseRepository
    {

        /// <summary>
        /// 验证课程编号是否重名
        /// </summary>
        /// <param name="courseCode"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExistCourseCode(string courseCode, int Id = 0,int way = 1)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = "select count(1) from New_Course where IsDelete=0 AND Way=@way AND  Code=@courseCode";
                if (Id > 0)
                    sqlwhere += " and Id <> " + Id;
                var param = new
                {
                    way = way,
                    courseCode = courseCode
                };
                int count = connection.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }
        
        
        /// <summary>
        /// 获取课程信息集合
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<New_Course> GetNew_CourseList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "ORDER BY New_Course.Id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                   connection.Query<int>("select count(1) from New_Course where " + where).First();

                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,New_Course.*
 from New_Course 
    where {2} 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy,where);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<New_Course>(query, parameters).ToList();
                            
            }
        }

        /// <summary>
        /// 获取课程列表(课程管理)
        /// </summary>
        /// <param name="way">课程类型</param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="Order"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_Course> GetCourseList(int way, int startIndex = 1, int startLength = int.MaxValue, string Order = "LastUpdateTime desc", string where = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = @"SELECT * FROM (SELECT row_number()OVER(ORDER BY " + Order + ") number,count(*)OVER(PARTITION BY null) totalcount,*,dbo.f_GetCourseTeacherName(Id,'0,1') AS teacher,dbo.f_GetClassName(Classes) AS classnames,dbo.f_GetRoomName(Id,'0,1') AS roomnames FROM New_Course WHERE Way=@Way AND " + where + " ) result WHERE  number BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex";
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    Way = way
                };
                return connection.Query<New_Course>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取单个课程信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public New_Course GetSingleCourse(int courseID,string wherestr="1=1")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select nc.*,dbo.f_GetCourseTeacherName(nc.Id,'0,1') AS teacher,dbo.f_GetClassName(nc.Classes) as classnames,dbo.f_GetRoomName(nc.Id,'0,1') AS roomnames from New_Course nc where nc.Id=@Id and " + wherestr);
                var param = new { Id = courseID };
                return connection.Query<New_Course>(query, param).FirstOrDefault();

            }
        }

        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="model"></param>
        public void AddCourse(New_Course model)
        {
            using (var connection = OpenConnection())
            {

                var strSql = new StringBuilder();
                strSql.Append("insert into New_Course (");
                strSql.Append(@"CourseName,Code,IsGroupTeach,Teachers,Classes,StartTime,EndTime,IsPingTeacher,ScoreDistribute,Memo,IsPingCourse,GStartTime,GType,GGroupNumber,GGroupPersonCount,GGroupRule,IsDelete,Way,LastUpdateTime,PublicFlag,IsTest,VideoLowLength)");
                strSql.Append(" values (@CourseName,@Code,@IsGroupTeach,@Teachers,@Classes,@StartTime,@EndTime,@IsPingTeacher,@ScoreDistribute,@Memo,@IsPingCourse,@GStartTime,@GType,@GGroupNumber,@GGroupPersonCount,@GGroupRule,@IsDelete,@Way,@LastUpdateTime,@PublicFlag,@IsTest,@VideoLowLength)");
                strSql.Append(";select @@IDENTITY");
                var param = new
                {
                    model.CourseName,
                    model.Code,
                    model.IsGroupTeach,
                    model.Teachers,
                    model.Classes,
                    model.StartTime,
                    model.EndTime,
                    model.IsPingTeacher,
                    model.ScoreDistribute,
                    model.Memo,
                    model.IsPingCourse,
                    model.GStartTime,
                    model.GType,
                    model.GGroupNumber,
                    model.GGroupPersonCount,
                    model.GGroupRule,
                    model.IsDelete,
                    model.Way,
                    model.LastUpdateTime,
                    model.PublicFlag,
                    model.IsTest,
                    model.VideoLowLength
                };

                var id =
                    connection.Query<decimal>(strSql.ToString(), param)
                              .FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 修改课程
        /// </summary>
        /// <param name="model">课程信息</param>
        public bool UpdateCourse(New_Course model)
        {
            using (var connection = OpenConnection())
            {

                var strSql = new StringBuilder();
                strSql.Append("update New_Course set ");
                strSql.Append(@"CourseName=@CourseName,Code=@Code,IsGroupTeach=@IsGroupTeach,Teachers=@Teachers,Classes=@Classes,StartTime=@StartTime,EndTime=@EndTime,IsPingTeacher=@IsPingTeacher,ScoreDistribute=@ScoreDistribute,Memo=@Memo,IsPingCourse=@IsPingCourse,GStartTime=@GStartTime,GType=@GType,GGroupNumber=@GGroupNumber,GGroupPersonCount=@GGroupPersonCount,GGroupRule=@GGroupRule,Way=@Way,LastUpdateTime=@LastUpdateTime,PublicFlag=@PublicFlag,IsDelete=@IsDelete,IsTest=@IsTest,VideoLowLength=@VideoLowLength where Id=@Id");
                var param = new
                {
                    model.Id,
                    model.CourseName,
                    model.Code,
                    model.IsGroupTeach,
                    model.Teachers,
                    model.Classes,
                    model.StartTime,
                    model.EndTime,
                    model.IsPingTeacher,
                    model.ScoreDistribute,
                    model.Memo,
                    model.IsPingCourse,
                    model.GStartTime,
                    model.GType,
                    model.GGroupNumber,
                    model.GGroupPersonCount,
                    model.GGroupRule,
                    model.Way,
                    model.LastUpdateTime,
                    model.PublicFlag,
                    model.IsDelete,
                    model.IsTest,
                    model.VideoLowLength
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 综合评分管理页面列表专用
        /// </summary>
        /// <returns></returns>
        public List<New_Course> GetNewAllScoreManager( string where = " 1 = 1 ",  int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "ORDER BY New_Course.Id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                //totalCount =
                //   connection.Query<int>("select count(1) from New_Course where " + where).First();

                string query = string.Format(@"
 select  * from (
	select count(*) OVER(PARTITION BY null) AS totalCount,row_number() over( {0} ) as rowNum,
	New_Course.Id,
	New_Course.CourseName,
	New_Course.StartTime,
	New_Course.EndTime,
	dbo.f_GetClassName(New_Course.Classes) as classnames,
    dbo.f_GetCourseTeacherName(New_Course.Id,'0,1') as teacher,
	New_Course.IsGroupTeach,     
	dbo.f_GetRoomName(New_Course.Id,'0,1') as roomnames,
    (SELECT COUNT(UserId) FROM New_CourseOrder
WHERE CourseId=New_Course.Id) as UserCount
   from New_Course 
   where PublicFlag=1  and {1}
  ) dd
where  rowNum between  @startLength*(@startIndex-1)+1 AND @startLength*@startIndex ", orderBy, where);
                var parameters = new
                {
                    startLength = pageLength,
                    startIndex = startIndex
                };
                return connection.Query<New_Course>(query, parameters).ToList();

            }
        }


        /// <summary>
        /// 根据课程ID 找出教室 讲师
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public New_Course GetCourseByCourseRoomRule(int courseID)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select 
    New_Course.IsTest,
	New_Course.Id,
	New_Course.CourseName,
    New_Course.Code,
	dbo.f_GetClassName(New_Course.Classes) as classnames,
	New_Course.StartTime,
	New_Course.EndTime,
	dbo.f_GetCourseTeacherName(New_Course.Id,'0,1') as Teachers,
	dbo.f_GetRoomName(New_Course.Id,'0,1') as roomnames
   from New_Course 
     where New_Course.Id=@Id ");
                var param = new { Id = courseID };
                return connection.Query<New_Course>(query, param).FirstOrDefault();

            }
        }


        /// <summary>
        /// 讲师端-获取学员对我的评价列表(add by yxt 2013-07-06)
        /// <param name="where">条件语句格式" and ..."</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// </summary>
        public List<New_CourseRoomRule> GetPingByUserToTeacherList(string where = "", int startIndex = 1, int startLength = int.MaxValue, string orderBy = "ORDER BY StartTime desc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = "ORDER BY StartTime desc";
                }
                string sql = string.Format(@" 
SELECT * FROM 
(
    SELECT count(*) OVER(PARTITION BY null) AS totalCount
    , row_number() OVER({1}) AS rowNum ,yxt_t1.* 
    from (
            select yxt_t0.*,((CourseTeacherAvg + CourseAvg)/2) as TotalCourseAvgStr --综合平均分 
            from (
                    select crr.Id,c.CourseName,crr.Type,crr.StartTime,crr.EndTime ,crr.TeacherId,u.Realname as teachername,crr.PersonCount
                           ,isnull((select  round(AVG(CONVERT(decimal,sra.SubjectiveAnswer)),2)  
                            from Survey_ReplyAnswer sra
                            JOIN Survey_Question sq ON sq.ObjectType=1 and sq.QuestionID=sra.QuestionID AND sq.QuestionType=3 --星评题
                            where  sra.SourceFrom=0 and sra.ObjectID=crr.CourseId and sra.ExampaperID=crr.PingId 
                                   AND sra.QuestionID IN (SELECT q.QuestionID FROM Survey_Question q WHERE q.ExampaperID=crr.PingId AND q.Status=0) 
                                   AND isnumeric(sra.SubjectiveAnswer)=1 --判断是否为数字
                                   and sra.Status=1 AND sra.CourseRoomRuleId=crr.Id),0 ) AS CourseTeacherAvg --讲师平均分
                          ,isnull((select  round(AVG(CONVERT(decimal,sra.SubjectiveAnswer)),2)  
                            from Survey_ReplyAnswer sra
                            JOIN Survey_Question sq ON sq.ObjectType=0 and  sq.QuestionID=sra.QuestionID AND sq.QuestionType=3 --星评题
                            where  sra.SourceFrom=0 and sra.ObjectID=crr.CourseId and sra.ExampaperID=crr.PingId 
                                   AND sra.QuestionID IN (SELECT q.QuestionID FROM Survey_Question q WHERE q.ExampaperID=crr.PingId AND q.Status=0) 
                                   AND isnumeric(sra.SubjectiveAnswer)=1 --判断是否为数字
                                   and sra.Status=1 AND sra.CourseRoomRuleId=crr.Id),0 ) AS CourseAvg --课程平均分
                         ,( 
                             SELECT COUNT(*) FROM
                                (
                                    SELECT DISTINCT sra.UserID
                                    FROM Survey_ReplyAnswer sra 
                                    WHERE sra.SourceFrom=0 
                                          AND sra.ObjectID=crr.CourseId 
                                          AND sra.ExampaperID=crr.PingId 
                                          AND sra.CourseRoomRuleId=crr.Id 
                                          AND sra.Status=1 
                                          AND sra.QuestionID IN (SELECT q.QuestionID FROM Survey_Question q WHERE q.ExampaperID=crr.PingId AND q.Status=0) 
                                ) AS ra_user
                           ) as HasPingUserCount --已评人数
                    FROM New_CourseRoomRule crr
                    JOIN New_Course c ON c.Id=crr.CourseId and c.IsDelete=0 and c.PublicFlag=1 
                    left join Sys_User u on crr.TeacherId=u.UserId 
                    where 1=1 and crr.PingId>0 and crr.PingId is not null  {0} 
                ) as yxt_t0 
          ) as yxt_t1 
) AS result 
WHERE result.rowNum BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex ", where, orderBy);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<New_CourseRoomRule>(sql, param).ToList();
            }
        }

        public List<New_Course> GetStudyList(out int totalCount,int userid,string where=" 1=1" ,int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "ORDER BY New_Course.Id DESC")
        {
            using (IDbConnection conn = OpenConnection())
            {
                totalCount =
                  conn.Query<int>("select count(1) from New_course where charindex(convert(varchar(20),(select ClassId from New_GroupUser where userid=" + userid + ")),Classes  )>0 and way=2 and PublicFlag=1 and IsDelete=0 and " + where).First();

                string query = string.Format(@"
 select top {0} * from (
	select  row_number() over( {1} ) as rowNum,
dbo.f_GetClassName(New_Course.Classes) as classnames,
	  * from New_course where charindex(
	convert(varchar(20),(select ClassId from New_GroupUser where userid={3})),Classes
  )>0 and way=2 and PublicFlag=1 and IsDelete=0 and {2}
  ) dd
where  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy, where,userid);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return conn.Query<New_Course>(query, parameters).ToList();
            }
        }


   


    }
}
