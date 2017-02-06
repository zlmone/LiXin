using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseLearn;
using System.Data;
using LiXinModels.CourseManage;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.CourseLearn
{
    public class Cl_AttendceDB : BaseRepository
    {
        #region 点开
        
        public Cl_Attendce GetCl_Attendce(int CourseId, int UserId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = "select * from Cl_Attendce where CourseId=@CourseId and UserId=@UserId";
                return connection.Query<Cl_Attendce>(sqlstr, new { CourseId = CourseId, UserId = UserId }).FirstOrDefault();
            }
        }

        public List<Cl_Attendce> GetList(string where = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = "select * from Cl_Attendce where "+where;
                return connection.Query<Cl_Attendce>(sqlstr).ToList();
            }
        }

        /// <summary>
        /// 我的课程(讲师) 下考勤学员列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Cl_Attendce> GetListByTeacher(out int totalCount, string CourseId, int startIndex = 0, int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {
                #region 暂无用处
                string sql = "";
                string sqlstr = "";
                //sqlstr += " select Cl_Attendce.CourseId,Cl_Attendce.StartTime,Cl_Attendce.EndTime,Sys_User.JobNum,Sys_User.Realname,Sys_User.DeptName,Cl_CourseOrder.OrderTime,";
                //sqlstr += "( select  row_number() over( order by Cl_CourseOrder.OrderTime desc  ) )as 'num'";
                sqlstr += "  select count(1)";
                sqlstr += "  from Cl_CourseOrder left join Cl_Attendce on Cl_CourseOrder.Userid=Cl_Attendce.Userid  left join Sys_User on Cl_CourseOrder.Userid =Sys_User.UserId";
                sqlstr += "  where Cl_CourseOrder.CourseId= " + CourseId;
                sqlstr += "  and Cl_CourseOrder.IsLeave=0 and (Cl_CourseOrder.OrderStatus=1 or Cl_CourseOrder.OrderStatus=3) ";
                sqlstr += "  and (Cl_CourseOrder.OrderTime<OrderEndTime or getdate()>OrderEndTime) ";
                sqlstr += "  and charindex((select TrainGrade from Sys_User ";
                sqlstr += "  where UserId=Cl_CourseOrder.UserId),(select OpenLevel from Co_Course where Co_Course.Id=Cl_CourseOrder.CourseId))>0 ";
                //sqlstr += "  order by Cl_CourseOrder.OrderTime desc";

                totalCount =
                   connection.Query<int>(sqlstr).First();

                sql += " select top(" + pageLength + ") * from";
                sql += "( ";
                sql += " select Cl_Attendce.CourseId,Cl_Attendce.StartTime,Cl_Attendce.EndTime,    Cl_Attendce.OnlineStartTime,Cl_Attendce.OnlineEndTime,Sys_User.JobNum,Sys_User.Realname,Sys_User.DeptName,Cl_CourseOrder.OrderTime,";
                sql += "( row_number() over( order by Cl_CourseOrder.OrderTime desc  ) )as 'num'";
                sql += "  from Cl_CourseOrder left join Cl_Attendce on Cl_CourseOrder.Userid=Cl_Attendce.Userid  left join Sys_User on Cl_CourseOrder.Userid =Sys_User.UserId";
                sql += "  where Cl_CourseOrder.CourseId= " + CourseId;
                sql += "  and Cl_CourseOrder.IsLeave=0 and (Cl_CourseOrder.OrderStatus=1 or Cl_CourseOrder.OrderStatus=3) ";
                sql += "  and (Cl_CourseOrder.OrderTime<OrderEndTime or getdate()>OrderEndTime) ";
                sql += "  and charindex((select TrainGrade from Sys_User ";
                sql += "  where UserId=Cl_CourseOrder.UserId),(select OpenLevel from Co_Course where Co_Course.Id=Cl_CourseOrder.CourseId))>0 ";
                //sql += "  order by Cl_CourseOrder.OrderTime desc";
                sql += ")result where  num between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)";
                #endregion




                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex/pageLength
                    };

                    return connection.Query<Cl_Attendce>(sql, parameters).ToList();

            }
        }

        /// <summary>
        /// 判断 考勤 是否存在
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public bool Exists(int courseId, int userid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select count(1) from Cl_Attendce WHERE CourseId = @CourseId and UserId = @UserId";
                var param = new { CourseId = courseId, UserId = userid };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count < 1;
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        public void AddAttendce(Cl_Attendce model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"INSERT INTO dbo.Cl_Attendce (CourseId,UserId,StartTime,EndTime,OnlineStartTime,OnlineEndTime,LessLength,ApprovalUser,ApprovalMemo,ApprovalFlag,ApprovalDateTime,AttStatus) VALUES (@courseid,@userid,@starttime,@endtime,@onlinestarttime,@onlineendtime,@lesslength,@approvaluser,@approvalmemo,@approvalflag,@approvaldatetime,@AttStatus)";
                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.StartTime,
                    model.EndTime,
                    model.OnlineStartTime,
                    model.OnlineEndTime,
                    model.LessLength,
                    model.ApprovalUser,
                    model.ApprovalMemo,
                    model.ApprovalFlag,
                    model.ApprovalDateTime,
                    model.AttStatus
                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateAttendce(Cl_Attendce model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Cl_Attendce SET CourseId = @courseid,UserId = @userid,StartTime = @starttime,EndTime = @endtime,LessLength = @lesslength,ApprovalUser = @approvaluser,ApprovalMemo = @approvalmemo,ApprovalFlag = @approvalflag,ApprovalDateTime = @approvaldatetime,AttStatus=@AttStatus WHERE Id=@id";
                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.StartTime,
                    model.EndTime,
                    model.LessLength,
                    model.ApprovalUser,
                    model.ApprovalMemo,
                    model.ApprovalFlag,
                    model.ApprovalDateTime,
                    model.AttStatus,
                    model.Id
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }
        #endregion

        //#region 我的考试,我的评估,我的考勤方法

        /// <summary>
        /// 我的考试
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="UserId"></param>
        /// <param name="TrainGrade"></param>
        /// <param name="DeptId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyExamList(out int totalCount, int UserId = 0, string TrainGrade = "", int DeptId = 0, string where = " 1 = 1 ", int startIndex = 0,
                                     int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string querystr = string.Format(@"
      select count(1) from
(
select row_number() over( ORDER BY Co_Course.id desc) as rowNum,
Co_Course.ID
		,Co_Course.CourseName
        ,Co_Course.Way
        ,Co_Course.StartTime
        ,Co_Course.EndTime
        ,Co_Course.DepCourseId as DepCourseId
        ,Co_Course.NumberLimited as NumberLimited
        ,Cl_CourseOrder.GetScore as GetScore
        ,Co_CoursePaper.PaperId as PaperId
		,Co_CoursePaper.TestTimes as ExamALLTestTimes
        ,Co_CoursePaper.[Length] as Length	
        ,Cl_CourseOrder.PassStatus
        ,Cl_CpaLearnStatus.IsPass as IsPass
        ,Co_CoursePaper.LevelScore
 from dbo.Co_Course left join dbo.Cl_CourseOrder on Co_Course.Id=Cl_CourseOrder.CourseId
                    left join dbo.Co_CoursePaper on Co_Course.id=Co_CoursePaper.CourseId
                    left join Cl_CpaLearnStatus on Co_Course.Id=Cl_CpaLearnStatus.CourseID and Cl_CpaLearnStatus.UserID={0}  and CpaFlag=0
  where {3} and IsTest=1 and Co_Course.IsDelete=0 and (
   (

    ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
                and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
            or Cl_CourseOrder.orderstatus = 3) and Cl_CourseOrder.UserId={0} and IsTest=1 
   )
   or
   (	
	(Co_Course.Way=2 and IsTest=1 and Co_Course.IsDelete=0 and Co_Course.Publishflag=1 and Co_Course.CourseFrom=2 )
		and (
		
			(Co_Course.OpenFlag=0 and charindex('{1}',Co_Course.OpenLevel)>0)		
			or   (
					Co_Course.OpenFlag=1 and
					charindex('{1}',Co_Course.OpenLevel)>0 and
			     	'{0}' in(select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
				  )
		   or     (
					Co_Course.OpenFlag=2 and 
					 charindex('{1}',Co_Course.OpenLevel)>0 and charindex(('{2}'),(Co_Course.OpenDepartObject))>0
				  )
		   or
				(
					charindex('{1}',Co_Course.OpenLevel)>0 and Co_Course.OpenFlag=3 and(
					'{0}' in(select UserId from Sys_GroupUser  where GroupId  in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
				 	or charindex(('{2}'),(Co_Course.OpenDepartObject))>0)
				) 
			)
   )
)
)bb 
", UserId, TrainGrade, DeptId, where);
                totalCount = connection.Query<int>(querystr).First();

                string query = string.Format(@"
select top({3}) * from
(
select row_number() over( ORDER BY Co_Course.id desc) as rowNum,
Co_Course.ID
		,Co_Course.CourseName
        ,Co_Course.Way
        ,Co_Course.Sort
        ,Co_Course.StartTime
        ,Co_Course.EndTime
        ,Co_Course.DepCourseId as DepCourseId
        ,Co_Course.NumberLimited as NumberLimited
        ,Cl_CourseOrder.GetScore as GetScore
        ,Co_CoursePaper.PaperId as PaperId
		,Co_CoursePaper.TestTimes as ExamALLTestTimes
        ,Co_CoursePaper.[Length] as Length	
        ,Cl_CourseOrder.PassStatus
        ,Cl_CpaLearnStatus.IsPass as IsPass
        --,(select IsPass from Cl_CpaLearnStatus where Cl_CpaLearnStatus.CourseID=Co_Course.ID and Cl_CpaLearnStatus.UserID={0}  and CpaFlag=0) as IsPass
        ,Co_CoursePaper.LevelScore
        ,Co_CoursePaper.Id as CoPaperID
 from dbo.Co_Course left join dbo.Cl_CourseOrder on Co_Course.Id=Cl_CourseOrder.CourseId
                    left join dbo.Co_CoursePaper on Co_Course.id=Co_CoursePaper.CourseId
                    left join Cl_CpaLearnStatus on Co_Course.Id=Cl_CpaLearnStatus.CourseID and Cl_CpaLearnStatus.UserID={0}  and CpaFlag=0
  where {4} and IsTest=1 and Co_Course.IsDelete=0 and (
   (

   ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
                and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
            or Cl_CourseOrder.orderstatus = 3) and Cl_CourseOrder.UserId={0} and IsTest=1
   )
   or
   (	
	(Co_Course.Way=2 and IsTest=1 and Co_Course.IsDelete=0 and Co_Course.Publishflag=1 and Co_Course.CourseFrom=2 )
		and
		(
			(Co_Course.OpenFlag=0 and charindex('{1}',Co_Course.OpenLevel)>0)		
			or   (
					Co_Course.OpenFlag=1 and
					charindex('{1}',Co_Course.OpenLevel)>0 and
			     	'{0}' in(select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
				  )
		   or     (
					Co_Course.OpenFlag=2 and 
					 charindex('{1}',Co_Course.OpenLevel)>0 and charindex(('{2}'),(Co_Course.OpenDepartObject))>0
				  )
		   or
				(
					charindex('{1}',Co_Course.OpenLevel)>0 and Co_Course.OpenFlag=3 and(
					'{0}' in(select UserId from Sys_GroupUser  where GroupId  in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
				 	or charindex(('{2}'),(Co_Course.OpenDepartObject))>0)
				) )
			
   )
)
)bb 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", UserId, TrainGrade, DeptId, pageLength, where);


           
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                
                return connection.Query<Co_CourseShow>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 我的考勤
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="falg">0:列表；1：首页</param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyAttendcsList(out int totalCount, int UserId, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue,int falg=0)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql =
                    string.Format(
                        @"
SELECT count(1) FROM
(
	SELECT 
        row_number() over( ORDER BY Co_Course.id DESC) as rowNum
		,Cl_CourseOrder.ID
		,Co_Course.CourseName
        ,Co_Course.YearPlan
        ,Co_Course.Way
        ,Co_Course.StartTime 
        ,Co_Course.EndTime 
        ,Co_Course.CourseLength
		,Co_Course.IsTest
        ,Co_Course.IsMust 
        ,Co_Course.DepCourseId as DepCourseId
		,Co_Course.NumberLimited as NumberLimited			
		,Cl_CourseOrder.PassStatus		
		,Cl_CourseOrder.CourseId as CourseId
		,Cl_CourseOrder.UserId
		,Cl_Attendce.StartTime as AttendceStartTime
		,Cl_Attendce.EndTime as AttendceEndTime		
        ,Sys_User.Realname as Realname		    
	FROM Cl_CourseOrder left join Co_Course on Cl_CourseOrder.CourseId=Co_Course.ID
						left join Sys_User on Co_Course.Teacher=Sys_User.UserId									
						left join Cl_Attendce on Cl_Attendce.CourseId=Cl_CourseOrder.CourseId 
						and Cl_Attendce.UserId=Cl_CourseOrder.UserId
	WHERE " + (falg == 0 ? "" : ("Co_Course.EndTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and ")) + where +
                        @"AND   ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
                and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
            or Cl_CourseOrder.orderstatus = 3) and Cl_CourseOrder.UserId={0} and Co_Course.IsDelete=0
) bb 
",
                        UserId);

                totalCount = connection.Query<int>(sql).First();

                string query = string.Format(@"
                SELECT top({0}) * FROM
(
	SELECT 
        row_number() over( ORDER BY Co_Course.StartTime ASC) as rowNum
		,Cl_CourseOrder.ID
		,Co_Course.CourseName
        ,Co_Course.YearPlan
        ,Co_Course.Way
        ,Co_Course.Sort
        ,Co_Course.StartTime 
        ,Co_Course.EndTime 
        ,Co_Course.CourseLength
		,Co_Course.IsTest
        ,Co_Course.IsMust
        ,Co_Course.DepCourseId as DepCourseId
        ,Co_Course.AttFlag as AttFlag
		,Co_Course.NumberLimited as NumberLimited			
		,Cl_CourseOrder.PassStatus		
		,Cl_CourseOrder.CourseId as CourseId
		,Cl_CourseOrder.UserId
        ,Cl_Attendce.Id as AttendceId
		,Cl_Attendce.StartTime as AttendceStartTime
		,Cl_Attendce.EndTime as AttendceEndTime	
        ,Cl_Attendce.IsApp	as IsApp
        ,Cl_Attendce.ApprovalFlag as AttStatus
        ,Cl_Attendce.LessLength as LessLength
        ,Sys_User.Realname as Realname		    
        ,(select count(1) from Cl_MidAttendce where UserId=Cl_Attendce.UserId and Cl_MidAttendce.CourseId=Cl_CourseOrder.CourseId) as MidAttendceCount
	FROM Cl_CourseOrder left join Co_Course on Cl_CourseOrder.CourseId=Co_Course.ID
						left join Sys_User on Co_Course.Teacher=Sys_User.UserId
						left join Cl_Attendce on Cl_Attendce.CourseId=Cl_CourseOrder.CourseId 
						and Cl_Attendce.UserId=Cl_CourseOrder.UserId
	WHERE " + (falg == 0 ? "" : ("Co_Course.EndTime<'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' and ")) + where + @"
	 AND   ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
                and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
            or Cl_CourseOrder.orderstatus = 3) and Cl_CourseOrder.UserId={1} and Co_Course.IsDelete=0
) bb
WHERE  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", pageLength, UserId);
                
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Co_CourseShow>(query, parameters).ToList();
            }
        }


        /// <summary>
        /// 我的课前建议
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyproposeList(out int totalCount,int UserId, string where = " 1 = 1 ",string num=" 1=1", int startIndex = 0, int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string sql = string.Format(@"
select count(1) from (
select * from 
( 
	SELECT * FROM
	(
	SELECT        
		Cl_CourseOrder.ID
		,Co_Course.CourseName
        ,Co_Course.Way
        ,Co_Course.CourseLength
		,Co_Course.IsTest
        ,Co_Course.IsMust
        ,Co_Course.StartTime 
        ,Co_Course.EndTime 
        ,Co_Course.DepCourseId as DepCourseId
		,Co_Course.NumberLimited as NumberLimited			
		,Cl_CourseOrder.PassStatus		
		,Cl_CourseOrder.CourseId as CourseId
		,Cl_CourseOrder.UserId		
        ,(select count(1) from Cl_CourseAdvice where Cl_CourseAdvice.CourseId=Cl_CourseOrder.CourseId and Cl_CourseAdvice.UserId=Cl_CourseOrder.UserId  and  Cl_CourseAdvice.IsDelete=0) as AdviceCount
		,Co_Course.StartTime as CourseStartTime
		,Co_Course.EndTime as CourseEndTime	
        ,Sys_User.Realname as Realname		    
	FROM Cl_CourseOrder left join Co_Course on Cl_CourseOrder.CourseId=Co_Course.ID
						left join Sys_User on Co_Course.Teacher=Sys_User.UserId			
						left join dbo.Co_CoursePaper on Co_Course.id=Co_CoursePaper.CourseId							
	WHERE " + where + @"  
	 AND   ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
                and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
            or Cl_CourseOrder.orderstatus = 3) and Cl_CourseOrder.UserId={0} and Co_Course.IsDelete=0
) bb   
)DD where " + num + @"
)AA 
 
", UserId);


                totalCount = connection.Query<int>(sql).First();

                string query = string.Format(@"


select top({0}) * from (
select *,row_number() over( ORDER BY DD.id DESC) as rowNum from 
( 
	SELECT * FROM
	(
	SELECT   
       
		Cl_CourseOrder.ID
		,Co_Course.CourseName
        ,Co_Course.Way
        ,Co_Course.CourseLength
		,Co_Course.IsTest
        ,Co_Course.IsMust
        ,Co_Course.StartTime 
        ,Co_Course.EndTime 
        ,Co_Course.DepCourseId as DepCourseId
		,Co_Course.NumberLimited as NumberLimited			
		,Cl_CourseOrder.PassStatus		
		,Cl_CourseOrder.CourseId as CourseId
		,Cl_CourseOrder.UserId		
        ,(select count(1) from Cl_CourseAdvice where Cl_CourseAdvice.CourseId=Cl_CourseOrder.CourseId and Cl_CourseAdvice.UserId=Cl_CourseOrder.UserId  and  Cl_CourseAdvice.IsDelete=0) as AdviceCount
		,Co_Course.StartTime as CourseStartTime
		,Co_Course.EndTime as CourseEndTime	
        ,Sys_User.Realname as Realname		    
	FROM Cl_CourseOrder left join Co_Course on Cl_CourseOrder.CourseId=Co_Course.ID
						left join Sys_User on Co_Course.Teacher=Sys_User.UserId			
						left join dbo.Co_CoursePaper on Co_Course.id=Co_CoursePaper.CourseId							
	WHERE " + where + @"
	 AND   ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
                and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
            or Cl_CourseOrder.orderstatus = 3) and Cl_CourseOrder.UserId={1} and Co_Course.IsDelete=0
) bb
)DD where " + num + @"
)AA WHERE  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", pageLength, UserId);



                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Co_CourseShow>(query, parameters).ToList();


            }
        }

        /// <summary>
        /// 我的课后评估
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Co_CourseShow> GetMyAfterCourseList(out int totalCount,int UserId,string TrainGrade="",int DeptId=0 ,string where = " 1 = 1 ", string ALLnum="", int startIndex = 0,
                                                    int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string querystr = string.Format(@"
select count(1) from (
 SELECT * FROM      
(
      select * from
(
select 
Co_Course.ID
		,Co_Course.CourseName
        ,Co_Course.Way
        ,Co_Course.StartTime
        ,Co_Course.EndTime
        ,Co_Course.NumberLimited as NumberLimited
        ,Cl_CourseOrder.GetScore as GetScore
        ,Cl_CourseOrder.PassStatus
        ,(select count(1) from Survey_ReplyAnswer where [ObjectID]=Co_Course.ID and [UserID]={0} and [Status]=1) as ALLnum
 from dbo.Co_Course left join dbo.Cl_CourseOrder on Co_Course.Id=Cl_CourseOrder.CourseId
                    --left join Survey_ReplyAnswer on Survey_ReplyAnswer.ObjectID=Cl_CourseOrder.CourseId
                    --and Survey_ReplyAnswer.UserId=Cl_CourseOrder.UserId and Survey_ReplyAnswer.[Status]=0
  where {3} and IsPing=1 and Co_Course.IsDelete=0  and (
   (

   ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
                and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
            or Cl_CourseOrder.orderstatus = 3) and Cl_CourseOrder.UserId={0} 
   )
   or
   (	
	(Co_Course.Way=2  and Co_Course.IsDelete=0 and Co_Course.Publishflag=1 and Co_Course.CourseFrom=2 )
		and (
		
			(Co_Course.OpenFlag=0 and charindex('{1}',Co_Course.OpenLevel)>0)		
			or   (
					Co_Course.OpenFlag=1 and
					charindex('{1}',Co_Course.OpenLevel)>0 and
			     	'{0}' in(select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
				  )
		   or     (
					Co_Course.OpenFlag=2 and 
					 charindex('{1}',Co_Course.OpenLevel)>0 and charindex(('{2}'),(Co_Course.OpenDepartObject))>0
				  )
		   or
				(
					charindex('{1}',Co_Course.OpenLevel)>0 and Co_Course.OpenFlag=3 and(
					'{0}' in(select UserId from Sys_GroupUser  where GroupId  in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
				 	or charindex(('{2}'),(Co_Course.OpenDepartObject))>0)
				) 
			)
   )
)
)bb )DD where {4}
)aa
", UserId, TrainGrade, DeptId, where,ALLnum);
                totalCount = connection.Query<int>(querystr).First();


                string query = string.Format(@"
select top({3})  * from (
 SELECT *, row_number() over( ORDER BY DD.id desc) as rowNum
FROM      
(
select  * from
(
select 
Co_Course.ID
		,Co_Course.CourseName
        ,Sys_User.Realname as TeacherName
        ,Co_Course.Way
        ,Co_Course.Sort
        ,Co_Course.CourseLength
        ,Co_Course.StartTime
        ,Co_Course.EndTime
        ,Co_Course.DepCourseId as DepCourseId
        ,Co_Course.NumberLimited as NumberLimited
        ,Cl_CourseOrder.GetScore as GetScore
        ,Cl_CourseOrder.PassStatus
        ,(select count(1) from Survey_ReplyAnswer where [ObjectID]=Co_Course.ID and [UserID]={0} and [Status]=1) as ALLnum
        --,Survey_ReplyAnswer.SubjectiveAnswer as SubjectiveAnswer
        --,Survey_ReplyAnswer.ObjectiveAnswer as ObjectiveAnswer,

 from dbo.Co_Course left join dbo.Cl_CourseOrder on Co_Course.Id=Cl_CourseOrder.CourseId
                    left join Sys_User on Sys_User.UserId=Co_Course.Teacher
                    --and Survey_ReplyAnswer.UserId=Cl_CourseOrder.UserId and Survey_ReplyAnswer.[Status]=0
  where {4} and IsPing=1 and Co_Course.IsDelete=0 and (
   (

  ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
                and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
            or Cl_CourseOrder.orderstatus = 3) and Cl_CourseOrder.UserId={0} 
   )
   or
   (	
	(Co_Course.Way=2  and Co_Course.IsDelete=0 and Co_Course.Publishflag=1 and Co_Course.CourseFrom=2 )
		and (
		
			(Co_Course.OpenFlag=0 and charindex('{1}',Co_Course.OpenLevel)>0)		
			or   (
					Co_Course.OpenFlag=1 and
					charindex('{1}',Co_Course.OpenLevel)>0 and
			     	'{0}' in(select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
				  )
		   or     (
					Co_Course.OpenFlag=2 and 
					 charindex('{1}',Co_Course.OpenLevel)>0 and charindex(('{2}'),(Co_Course.OpenDepartObject))>0
				  )
		   or
				(
					charindex('{1}',Co_Course.OpenLevel)>0 and Co_Course.OpenFlag=3 and(
					'{0}' in(select UserId from Sys_GroupUser  where GroupId  in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
				 	or charindex(('{2}'),(Co_Course.OpenDepartObject))>0)
				) 		)	
   )
)
)bb 
)DD where {5}
)aa
where  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", UserId, TrainGrade, DeptId, pageLength, where, ALLnum);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Co_CourseShow>(query, parameters).ToList();
            }
        }


        /// <summary>
        /// 获取人员通过考试的个数
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="UserId"></param>
        /// <param name="TrainGrade"></param>
        /// <param name="DeptId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public int GetMyExamListPassCount(int UserId = 0, string TrainGrade = "", int DeptId = 0, string where = " 1 = 1 ")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string querystr = string.Format(@"
      select count(1) from
(
select row_number() over( ORDER BY Co_Course.id desc) as rowNum,
Co_Course.ID
		,Co_Course.CourseName
        ,Co_Course.Way
        ,Co_Course.StartTime
        ,Co_Course.EndTime
        ,Co_Course.NumberLimited as NumberLimited
        ,Cl_CourseOrder.GetScore as GetScore
        ,Co_CoursePaper.PaperId as PaperId
		,Co_CoursePaper.TestTimes as ExamALLTestTimes
        ,Co_CoursePaper.[Length] as Length	
        ,Cl_CourseOrder.PassStatus
        ,Cl_CpaLearnStatus.IsPass as IsPass
        ,Co_CoursePaper.LevelScore
 from dbo.Co_Course left join dbo.Cl_CourseOrder on Co_Course.Id=Cl_CourseOrder.CourseId
                    left join dbo.Co_CoursePaper on Co_Course.id=Co_CoursePaper.CourseId
                    left join Cl_CpaLearnStatus on Co_Course.Id=Cl_CpaLearnStatus.CourseID and Cl_CpaLearnStatus.UserID={0}  and CpaFlag=0
  where {3} and IsTest=1 and Co_Course.IsDelete=0 and (
   (

    ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
            or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
                and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
            or Cl_CourseOrder.orderstatus = 3) and Cl_CourseOrder.UserId={0} and IsTest=1
   )
   or
   (	
	(Co_Course.Way=2 and IsTest=1 and Co_Course.IsDelete=0 and Co_Course.Publishflag=1 and Co_Course.CourseFrom=2 )
		and (
		
			(Co_Course.OpenFlag=0 and charindex('{1}',Co_Course.OpenLevel)>0)		
			or   (
					Co_Course.OpenFlag=1 and
					charindex('{1}',Co_Course.OpenLevel)>0 and
			     	'{0}' in(select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
				  )
		   or     (
					Co_Course.OpenFlag=2 and 
					 charindex('{1}',Co_Course.OpenLevel)>0 and charindex(('{2}'),(Co_Course.OpenDepartObject))>0
				  )
		   or
				(
					charindex('{1}',Co_Course.OpenLevel)>0 and Co_Course.OpenFlag=3 and(
					'{0}' in(select UserId from Sys_GroupUser  where GroupId  in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
				 	or charindex(('{2}'),(Co_Course.OpenDepartObject))>0)
				) 
			)
   )
)
)bb 
", UserId, TrainGrade, DeptId, where);
                return  connection.Query<int>(querystr).First();


            
               
            }
        }

    }
}
