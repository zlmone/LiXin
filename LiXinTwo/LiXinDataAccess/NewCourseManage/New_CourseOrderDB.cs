using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Retech.Data;
using Retech.Core;

namespace LiXinDataAccess.NewCourseManage
{
    public class New_CourseOrderDB : BaseRepository
    {

        #region 增改查(基本信息)
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertNew_CourseOrder(New_CourseOrder model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO New_CourseOrder(CourseExamScore,ExamScore,TogetherMemo,GroupMemo,Type,CourseExamSumScore,ExamSumScore,CourseId,ClassId,UserId,OrderTime,LearnStatus,TogetherScore,GroupScore)
	                     values( @CourseExamScore,@ExamScore,@TogetherMemo,@GroupMemo,@Type,@CourseExamSumScore,@ExamSumScore,@CourseId,@ClassId,@UserId,@OrderTime,@LearnStatus,@TogetherScore,@GroupScore);SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.CourseExamScore,
                    model.ExamScore,
                    model.TogetherMemo,
                    model.GroupMemo,
                    model.Type,
                    model.CourseExamSumScore,
                    model.ExamSumScore,
                    model.CourseId,
                    model.ClassId,
                    model.UserId,
                    model.OrderTime,
                    model.LearnStatus,
                    model.TogetherScore,
                    model.GroupScore
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.ID);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateNew_CourseOrder(New_CourseOrder model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update New_CourseOrder set 		                     		                     
		                       CourseExamScore = @CourseExamScore , 		                     
		                       ExamScore = @ExamScore , 		                     
		                       TogetherMemo = @TogetherMemo , 		                     
		                       GroupMemo = @GroupMemo , 		                     	                     
		                       TogetherScore = @TogetherScore , 		                     
		                       GroupScore = @GroupScore,
                               CourseExamSumScore=@CourseExamSumScore,
                               ExamSumScore=@ExamSumScore   
		                   where Id=@id";

                var param = new
                {
                    model.Id,
                    model.CourseExamScore,
                    model.ExamScore,
                    model.TogetherMemo,
                    model.GroupMemo,
                    model.TogetherScore,
                    model.GroupScore,
                    model.CourseExamSumScore,
                    model.ExamSumScore
                };
                conn.Execute(strSql, param);
            }
        }

        /// <summary>
        /// 根据课程ID和用户ID 修改学习状态
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="LearnStatus">0：未开始；1：进行中；2：已完成</param>
        public void UpdateLearnStatus(int courseid, int userid, int LearnStatus)
        {
            using (var conn = OpenConnection())
            {
                string sql = "update New_CourseOrder set LearnStatus=" + LearnStatus + " where CourseId=" + courseid + " and UserId=" + userid;
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 删除已存在的课程学习记录
        /// </summary>
        /// <param name="sqlStr">删除条件</param>
        public void DeleteCourseOrder(string sqlStr = "1=2")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format("delete New_CourseOrder where {0}", sqlStr);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 获取此课程下面学员的评价详情
        /// </summary>
        /// <returns></returns>
        public List<New_CourseOrder> GetUserScoreList(int courseID, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  su.UserId desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
	SELECT  row_number()OVER(ORDER BY {0}) number,count(*)OVER(PARTITION BY null) totalcount,nco.Id,nco.UserId,NumberID,Realname, Sex,su.MobileNum,dbo.f_GetClassName(ClassId) ClassName,
	TogetherScore, GroupScore, CourseExamScore,  ExamScore, isnull(TogetherMemo,'') TogetherMemo,isnull(GroupMemo,'') GroupMemo,CourseExamSumScore, ExamSumScore,nco.CourseId
	FROM Sys_User  su
    RIGHT JOIN New_CourseOrder nco ON su.UserId=nco.UserId
    where CourseId=@courseID and {1}
) result
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", jsRenderSortField, where);
                var param = new
                {
                    courseID = courseID,
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<New_CourseOrder>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 批量打分
        /// </summary>
        public void UpdateAllScore(int TogetherScore, int GroupScore, string ids)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
UPDATE New_CourseOrder
SET TogetherScore={0}, GroupScore={1}
WHERE Id in ({2})
", TogetherScore, GroupScore, ids);
                conn.Execute(sql);
            }
        }

        /// <summary>
        /// 一键打分
        /// </summary>
        public void UpdateAllScore(int courseID, int IsGroupTeach)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@" UPDATE   New_CourseOrder    
SET TogetherScore={2}, GroupScore={1} 
WHERE Id IN (
SELECT nco.Id FROM Sys_User    a 
        LEFT JOIN New_CourseOrder nco ON a.UserId=nco.UserId
        WHERE nco.CourseId={0})
", courseID, IsGroupTeach == 1 ? 0 : 3, IsGroupTeach == 2 ? 0 : 3);
                conn.Execute(sql);
            }
        }


        #endregion


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_CourseOrder> Get(string where = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "select * from New_CourseOrder where " + where;
                return connection.Query<New_CourseOrder>(sql).ToList();
            }
        }




        /// <summary>
        /// 学员学习课程列表
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetNew_CourseOrderListByStudent(int userid, string where = " 1 = 1 ", string strwhere = " 1=1", int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "order by New_Course.StartTime")
        {
            using (IDbConnection connection = OpenConnection())
            {
                #region 旧
                //                totalCount =
                //                     connection.Query<int>("select count(1) from New_CourseOrder where UserId= " + userid + " and" + where).First();
                //                string query = string.Format(@"
                //                   select top {0} * from (
                //	select  row_number() over( {1} ) as rowNum,
                //	New_Course.Id as CourseId,
                //	New_Course.Code,
                //	New_Course.CourseName as CourseName,
                //	dbo.f_GetTeacherName(New_Course.Teachers) as teachername,
                //	dbo.f_GetClassName(New_Course.Classes) as classname,
                //	New_Course.StartTime as StartTime,
                //	New_Course.EndTime as EndTime,
                //	--dbo.f_GetRoomName(New_CourseRoomRule.RoomId,0) as roomname,
                //	New_Course.IsGroupTeach as IsGroupTeach
                // from New_CourseOrder left join New_Course
                // on New_CourseOrder.CourseId=New_Course.Id
                // --left join New_CourseRoomRule on New_Course.Id=New_CourseRoomRule.CourseId
                // where UserId={2} and {3}
                // ) dd
                //   where  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)  ", pageLength, orderBy, userid, where);
                //                var parameters = new
                //               {
                //                   pageCount = pageLength,
                //                   pageIndex = startIndex / pageLength
                //               };
                //                return connection.Query<New_CourseOrder>(query, parameters).ToList();

                #endregion
                string query = string.Format(@"
                   select * from (
	select  row_number() over( {1} ) as rowNum,
    dbo.f_GetRoomNameByCourseIdAndUseriId(New_Course.id,{2})  as roomname,
    dbo.f_GetNewMyCourseTeacherName(New_Course.Id ,{2}) as teachername,
	dbo.f_GetClassName(New_course.Classes) as  ClassName,
	*
    from New_Course where Id in(  select distinct(courseid) from New_CourseOrder    where  UserId={2} ) and PublicFlag=1 and IsDelete=0 and {3} 
 ) dd {4}
  -- where  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)  ", pageLength, orderBy, userid, where, strwhere);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                //return connection.Query<New_CourseOrder>(query, parameters).ToList();
                return connection.Query<New_CourseOrder>(query).ToList();

            }
        }

        /// <summary>
        /// 讲师列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userid"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        //public List<New_CourseOrder> GetNew_CourseOrderListByTeacher(out int totalCount, int userid, string where = " 1 = 1 ", int startIndex = 0,
        //int pageLength = int.MaxValue, string orderBy = "ORDER BY New_CourseOrder.Id DESC")
        public List<New_CourseOrder> GetNew_CourseOrderListByTeacher(int userid, string where = " 1 = 1 ", string strwhere = " 1=1", int startIndex = 0,
                             int pageLength = int.MaxValue, string orderBy = "ORDER BY New_CourseOrder.Id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                //totalCount = connection.Query<int>("  select count(1) from New_Course where Id in(  select distinct(courseid) from New_CourseRoomRule    where  teacherid="+userid+") and "+where).First();


                string query = string.Format(@"
                   select * from (
	select  row_number() over( {1} ) as rowNum,
    dbo.f_GetRoomNameTest(New_Course.id,{2})  as roomname,
	dbo.f_GetClassName(New_course.Classes) as  ClassName,
	*
    from New_Course where Id in(  select distinct(courseid) from New_CourseRoomRule    where  teacherid={2} and PublicFlag=1 and IsDelete=0)and {3} 
 ) dd {4}
  -- where  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)  ", pageLength, orderBy, userid, where, strwhere);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                //return connection.Query<New_CourseOrder>(query, parameters).ToList();
                return connection.Query<New_CourseOrder>(query).ToList();
            }
        }

        /// <summary>
        /// 找出学员记录 在集中和分组带教中的信息
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetCourseIdAndType(int courseid, int userid)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string query = string.Format(@" select 
	dbo.f_GetRoomNameByNew_CourseRoomRuleId(New_CourseOrderDetail.SubCourseID) as roomname,
	dbo.f_GetClassName(New_CourseRoomRule.RoomId) as classname,
	dbo.f_GetTeacherName(New_CourseRoomRule.TeacherId) as teachername,
	New_Course.StartTime,
	New_Course.EndTime
    --New_CourseOrder.Type
  from dbo.New_CourseOrderDetail left join New_CourseRoomRule
  on New_CourseOrderDetail.SubCourseID=New_CourseRoomRule.Id
  left join New_Course on New_CourseOrderDetail.CourseId=New_Course.Id
    where New_CourseOrderDetail.Userid={0} and New_CourseOrderDetail.CourseId={1} 
", userid, courseid);

                return connection.Query<New_CourseOrder>(query).ToList();
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetList(string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT su.LoginId,nc.* FROM New_CourseOrder nc
LEFT JOIN Sys_User su ON nc.UserId=su.UserId where {0}", where);
                return conn.Query<New_CourseOrder>(sql).ToList();
            }
        }

        public List<New_CourseOrder> GetTeacherOnLineTest(out int totalCount, int CourseId, int TeacherId, int startIndex = 0,
                                                                 int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string count = string.Format(@"select Userid from New_CourseOrderDetail where New_CourseOrderDetail.CourseId={0} and SubCourseID in(
  select Id from New_CourseRoomRule where courseid={0} and TeacherId={1})  group by Userid", CourseId, TeacherId);

                totalCount = connection.Query<int>(count).Count();

                string sql = string.Format(@"
 select top({2}) * from 
  (
select 
	row_number() over( ORDER BY bb.CourseId DESC) as rowNum,*
 from
(
select
	--row_number() over( ORDER BY New_CourseOrderDetail.CourseId DESC) as rowNum,
    distinct
    Sys_User.Realname as Realname,
	Sys_User.NumberID as UserNum,
	New_CourseOrderDetail.UserId,
	New_CourseOrderDetail.CourseId,
	New_CoursePaper.Id as CoPaperID,
	New_CoursePaper.PaperId as PaperId,
	New_CoursePaper.TestTimes,
	New_CoursePaper.LevelScore
   from New_CourseOrderDetail
  left join  New_CoursePaper on New_CoursePaper.CourseId=New_CourseOrderDetail.CourseId
   left join Sys_User on Sys_User.userid=New_CourseOrderDetail.UserId
  where New_CourseOrderDetail.CourseId={0} and SubCourseID in(
  select Id from New_CourseRoomRule where courseid={0} and TeacherId={1})
)bb
) cc
    where  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", CourseId, TeacherId, pageLength);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<New_CourseOrder>(sql, parameters).ToList();
            }
        }

        /// <summary>
        /// 讲师点击日期获取当日所有课程
        /// </summary>
        /// <param name="teacherid"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetTeachertoLearnDetal(int teacherid, string where = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = string.Format(@"select 
    New_CourseRoomRule.id as SubCourseID,
	New_Course.CourseName,
	New_CourseRoomRule.CourseId,
	New_CourseRoomRule.RoomId,
	New_CourseRoomRule.StartTime,
	New_CourseRoomRule.EndTime,
    New_CourseRoomRule.[TYPE],
    New_CourseRoomRule.PersonCount,
	New_ClassRoom.RoomName,
    dbo.f_GetTeacherName(New_CourseRoomRule.TeacherId) as teachername,
	dbo.f_GetClassName(New_Course.Classes) classname,
	New_ClassRoom.Address as classroomAddress
 from New_CourseRoomRule
left join New_Course on New_Course.Id=New_CourseRoomRule.CourseId
left join New_ClassRoom on New_CourseRoomRule.RoomId=New_ClassRoom.Id
where Teacherid={0} and {1} ", teacherid, where);

                return connection.Query<New_CourseOrder>(sql).ToList();
            }
        }


        /// <summary>
        /// 学员点击日期获取当日所有课程
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetStudentLearnDetal(int userid, string where = " 1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = string.Format(@"
select 
	New_CourseRoomRule.StartTime,
	New_CourseRoomRule.EndTime,
	New_CourseRoomRule.TeacherId,
	New_CourseRoomRule.[TYPE],
	New_Course.Way,
    dbo.f_GetRoomNameByNew_CourseRoomRuleId(New_CourseRoomRule.Id) as roomname,
	New_Course.CourseName,
	New_ClassRoom.Address as classroomAddress,
    New_Course.IsGroupTeach as IsGroupTeach,
    dbo.f_GetTeacherName(New_CourseRoomRule.TeacherId) as teachername
 from dbo.New_CourseOrderDetail
left join New_CourseRoomRule on New_CourseOrderDetail.SubCourseID=New_CourseRoomRule.Id
left join New_Course on New_Course.Id=New_CourseOrderDetail.CourseId
left join New_ClassRoom on New_CourseRoomRule.RoomId=New_ClassRoom.Id
where New_CourseOrderDetail.UserId={0} and {1} and New_CourseOrderDetail.IsLeave=0", userid, where);

                return connection.Query<New_CourseOrder>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取课前建议列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="coursename"></param>
        /// <param name="userid"></param>
        /// <param name="where"></param>
        /// <param name="strwhere"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetGetMyproposeList(out int totalCount, int userid, string where = " 1=1", string strwhere = " 1=1", string jsRenderSortField = "", int startIndex = 0, int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string strsql = string.Format(@"
select count(1)  from
(
	select *,row_number() over(" + jsRenderSortField + @") as rowNum from
	(
		select * from
		(
			select
			 New_CourseOrder.Id
			,New_CourseOrder.CourseId 
			,New_Course.CourseName
			,New_Course.Way			
			,New_Course.IsTest
			,New_Course.StartTime 
			,New_Course.EndTime 
			,New_CourseOrder.UserId            
			--,New_CourseAdvice.CourseId
			,(select count(1) from New_CourseAdvice where New_CourseAdvice.CourseId=New_CourseOrder.CourseId and New_CourseAdvice.UserId=New_CourseOrder.UserId and New_CourseAdvice.IsDelete=0) as AdviceCount
			from New_CourseOrder left join New_Course on New_CourseOrder.CourseId=New_Course.Id
				where " + where + @" and  New_CourseOrder.UserId={1}
		
		)bb
	)dd where " + strwhere + @"
)aa 

", pageLength, userid);

                totalCount = connection.Query<int>(strsql).First();


                string sql = string.Format(@"
select top ({0}) * from
(
	select *,row_number() over(" + jsRenderSortField + @") as rowNum from
	(
		select * from
		(
			select
			 New_CourseOrder.Id
			,New_CourseOrder.CourseId 
			,New_Course.CourseName
			,New_Course.Way			
			,New_Course.IsTest
			,New_Course.StartTime 
			,New_Course.EndTime 
			,New_CourseOrder.UserId
            ,New_Course.IsGroupTeach
            ,dbo.f_GetCourseTeacherName(New_Course.Id,'0,1') as teachername	
			--,New_CourseAdvice.CourseId
			,(select count(1) from New_CourseAdvice where New_CourseAdvice.CourseId=New_CourseOrder.CourseId and New_CourseAdvice.UserId=New_CourseOrder.UserId and New_CourseAdvice.IsDelete=0) as AdviceCount
			from New_CourseOrder left join New_Course on New_CourseOrder.CourseId=New_Course.Id
				where " + where + @" and  New_CourseOrder.UserId={1}
		
		)bb
	)dd where " + strwhere + @"
)aa WHERE  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", pageLength, userid);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<New_CourseOrder>(sql, parameters).ToList();
            }
        }

        /// <summary>
        /// 获取我的评估列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userid"></param>
        /// <param name="starttime"></param>
        /// <param name="where"></param>
        /// <param name="strwhere"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<New_CourseOrder> GetAfterCourseList(out int totalCount, int userid, string where, string strwhere, string jsRenderSortField = "", int startIndex = 0, int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string strsql = string.Format(@"
 select count(1) from
(
	select *	
	 from
	(
		select 
		New_Course.CourseName,
		New_CourseRoomRule.CourseId,
		New_CourseRoomRule.StartTime,
		New_CourseRoomRule.EndTime,
		New_CourseRoomRule.PingId,
		New_CourseOrderDetail.Type,
		New_CourseRoomRule.Id,
		dbo.f_GetTeacherName(New_CourseRoomRule.TeacherId) as teachername,
		(select count(1) from Survey_ReplyAnswer where Survey_ReplyAnswer.ExampaperID=New_CourseRoomRule.PingId 
		and  Survey_ReplyAnswer.UserID={0} and Survey_ReplyAnswer.CourseRoomRuleId=New_CourseOrderDetail.SubCourseID) as cc
		 from New_CourseOrderDetail left join
		New_CourseRoomRule on New_CourseOrderDetail.SubCourseID=New_CourseRoomRule.Id
		left join New_Course on New_CourseOrderDetail.CourseId=New_Course.Id
		where New_CourseOrderDetail.UserId={0} and New_CourseRoomRule.PingId>0
 )aa where " + where + @"
)bb where " + strwhere + @" 
", userid);
                totalCount = connection.Query<int>(strsql).First();


                string sql = string.Format(@"

select top({1}) * from 
(
    select *,row_number() over(" + jsRenderSortField + @") as rowNum from
(
	select *	
	 from
	(
		select 
		New_Course.CourseName,
		New_CourseRoomRule.CourseId,
		New_CourseRoomRule.StartTime,
		New_CourseRoomRule.EndTime,
		New_CourseRoomRule.PingId,
		New_CourseOrderDetail.Type,
		New_CourseRoomRule.Id,
		dbo.f_GetTeacherName(New_CourseRoomRule.TeacherId) as teachername,
		(select count(1) from Survey_ReplyAnswer where Survey_ReplyAnswer.ExampaperID=New_CourseRoomRule.PingId 
		and  Survey_ReplyAnswer.UserID={0} and Survey_ReplyAnswer.CourseRoomRuleId=New_CourseOrderDetail.SubCourseID) as cc
		 from New_CourseOrderDetail left join
		New_CourseRoomRule on New_CourseOrderDetail.SubCourseID=New_CourseRoomRule.Id
		left join New_Course on New_CourseOrderDetail.CourseId=New_Course.Id
		where New_CourseOrderDetail.UserId={0} and New_CourseRoomRule.PingId>0
 )aa where " + where + @"
)bb where " + strwhere + @"
)cc  where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)

", userid, pageLength);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<New_CourseOrder>(sql, parameters).ToList();
            }
        }





    }
}
