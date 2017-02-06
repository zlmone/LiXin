using System.Data;
using LiXinModels.DepCourseManage;
using LiXinModels.User;
using Retech.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Data;
using Retech.Core;
using LiXinModels.CourseLearn;
using LiXinModels.User;

namespace LiXinDataAccess.DepCourseManage
{
    public partial class Dep_CourseOrderDB : BaseRepository
    {
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertDep_CourseOrder(Dep_CourseOrder model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Dep_CourseOrder(CourseId,UserId,OrderTime,OrderStatus,LearnStatus,GetScore,PassStatus,AttScore,EvlutionScore,ExamScore,DepartSetId,IsLeave,OrderTimes,IsAppoint)
	                         values( @CourseId,@UserId,@OrderTime,@OrderStatus,@LearnStatus,@GetScore,@PassStatus,@AttScore,@EvlutionScore,@ExamScore,@DepartSetId,@IsLeave,@OrderTimes,@IsAppoint);SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.OrderTime,
                    model.OrderStatus,
                    model.LearnStatus,
                    model.GetScore,
                    model.PassStatus,
                    model.AttScore,
                    model.EvlutionScore,
                    model.ExamScore,
                    model.DepartSetId,
                    model.IsLeave,
                    model.OrderTimes,
                    model.IsAppoint
                    //model.Reason,
                    //model.LeaveTime,
                    //model.ApprovalFlag
                    //model.ApprovalTime,
                    //model.ApprovalUserId
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.ID);
            }
        }


        public bool UpdateDep_CourseOrder(Dep_CourseOrder model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update Dep_CourseOrder set CourseId = @CourseId,
                UserId = @UserId,OrderTime = @OrderTime,OrderStatus = @OrderStatus,LearnStatus = @LearnStatus,
        GetScore = @GetScore,PassStatus = @PassStatus,AttScore = @AttScore,EvlutionScore = @EvlutionScore,
        ExamScore = @ExamScore,DepartSetId = @DepartSetId,OrderTimes=@OrderTimes,IsLeave=@IsLeave,DropReason=@DropReason,DropType=@DropType,IsAppoint=@IsAppoint   where Id=@Id";

                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.OrderTime,
                    model.OrderStatus,
                    model.LearnStatus,
                    model.GetScore,
                    model.PassStatus,
                    model.AttScore,
                    model.EvlutionScore,
                    model.ExamScore,
                    model.DepartSetId,
                    model.OrderTimes,
                    model.IsLeave,
                    model.DropReason,
                    model.DropType,
                    model.IsAppoint,
                    //model.Reason,
                    //model.LeaveTime,
                    //model.ApprovalFlag,
                    //model.ApprovalTime,
                    //model.ApprovalUserId,                   
                    model.Id
                };
                int result = conn.Execute(strSql, param);
                return result > 0;
            }
        }


        /// <summary>
        /// 我的课程(讲师)详细下学员列表
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public List<Dep_CourseOrder> GetTeacherCourseUserList(out int totalCount, int CourseId, int startIndex = 0,
                                                             int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>(string.Format(@"
                SELECT count(1) FROM
(
	SELECT 
        row_number() over( ORDER BY cc.id DESC) as rowNum
		,dco.ID		
		,cc.NumberLimited as NumberLimited
		,cc.StartTime
		,cc.EndTime
		,u.Realname as Realname
		,u.Sex as Sex
		,u.DeptCode as DeptCode	
		,u.TrainGrade as TrainGrade
		,dco.IsAppoint
		,dco.PassStatus
		,dco.GetScore
		,dco.CourseId
		,dco.UserId		
        ,dco.OrderStatus
        ,u.DeptName    
	FROM Dep_CourseOrder dco
    left join Dep_Course cc on dco.CourseId=cc.ID
	left join Sys_User u on dco.UserId=u.UserId 
	left join Dep_ClassRoom dcr on  cc.RoomId=dcr.id 
	WHERE 1=1 
	       And (   
                  (dco.orderstatus = 1 and dco.isleave = 0 )
				   or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
                   or dco.orderstatus = 2 
                ) 
          and dco.CourseId={0}
) bb_yxt  ", CourseId)).First();


                string query = string.Format(@"
                SELECT top({1}) * FROM
(
	SELECT 
        row_number() over( ORDER BY cc.id DESC) as rowNum
		,dco.ID		
		,cc.NumberLimited as NumberLimited
		,cc.StartTime
		,cc.EndTime
		,u.Realname as Realname
		,u.Sex as Sex
		,u.DeptCode as DeptCode	
		,u.TrainGrade as TrainGrade
		,dco.IsAppoint
		,dco.PassStatus
		,dco.GetScore
		,dco.CourseId
		,dco.UserId		    
        ,dco.OrderStatus
        ,u.DeptName 
	FROM Dep_CourseOrder dco
    left join Dep_Course cc on dco.CourseId=cc.ID
	left join Sys_User u on dco.UserId=u.UserId 
	left join Dep_ClassRoom dcr on  cc.RoomId=dcr.id 
	WHERE 1=1 
	       And (   
                  (dco.orderstatus = 1 and dco.isleave = 0 )
				   or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
                   or dco.orderstatus = 2 
               ) 
          and dco.CourseId={0}
) bb_yxt
WHERE  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", CourseId, pageLength);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Dep_CourseOrder>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 我的课程(讲师)考勤学员列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Dep_CourseOrder> GetTeacherCourseAttendceList(out int totalCount, int CourseId, int startIndex = 0,
                                                                 int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>(string.Format(@"
                SELECT count(1) FROM
(
	SELECT 
        row_number() over( ORDER BY cc.id DESC) as rowNum
		,dco.ID		
		,cc.NumberLimited as NumberLimited
		,cc.StartTime
		,cc.EndTime
		,u.Realname as Realname
		,u.Sex as Sex
		,u.DeptCode as DeptCode	
		,u.TrainGrade as TrainGrade
        ,u.JobNum as JobNum
		,dco.IsAppoint
		,dco.PassStatus
		,dco.GetScore
		,dco.CourseId
		,dco.UserId
        ,dco.OrderStatus
        ,u.DeptName 
        ,datt.StartTime as AttendceStartTime
        ,datt.EndTime as AttendceEndTime		
        ,dtc.ApprovalFlag  	AttendceApprovalFlag	    
	FROM Dep_CourseOrder dco 
	LEFT JOIN Dep_Attendce dtc ON dtc.CourseId=dco.CourseId AND dco.UserId=dtc.UserId
    left join Dep_Course cc on dco.CourseId=cc.Id 
	left join Sys_User u on dco.UserId=u.UserId 
	left join Dep_Attendce datt on  datt.UserId=dco.UserId and datt.CourseId=dco.CourseId
	WHERE 1=1 
	       And (   
                  (dco.orderstatus = 1 and dco.isleave = 0 )
				   or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
                   or dco.orderstatus = 2 
                ) 
          and dco.CourseId={0}
) bb_yxt
  ", CourseId)).First();


                string query = string.Format(@"
                SELECT top({1}) * FROM
(
	SELECT 
        row_number() over( ORDER BY cc.id DESC) as rowNum
		,dco.ID		
		,cc.NumberLimited as NumberLimited
		,cc.StartTime
		,cc.EndTime
		,u.Realname as Realname
		,u.Sex as Sex
		,u.DeptCode as DeptCode	
		,u.TrainGrade as TrainGrade
        ,u.JobNum as JobNum
		,dco.IsAppoint
		,dco.PassStatus
		,dco.GetScore
		,dco.CourseId
		,dco.UserId
        ,dco.OrderStatus
        ,u.DeptName 
        ,datt.StartTime as AttendceStartTime
        ,datt.EndTime as AttendceEndTime	
        ,datt.Status as Status  		    
	  ,dtc.ApprovalFlag  	AttendceApprovalFlag	    
	FROM Dep_CourseOrder dco 
	LEFT JOIN Dep_Attendce dtc ON dtc.CourseId=dco.CourseId AND dco.UserId=dtc.UserId
    left join Dep_Course cc on dco.CourseId=cc.Id 
	left join Sys_User u on dco.UserId=u.UserId 
	left join Dep_Attendce datt on  datt.UserId=dco.UserId and datt.CourseId=dco.CourseId
	WHERE 1=1 
	       And (   
                  (dco.orderstatus = 1 and dco.isleave = 0 )
				   or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
                   or dco.orderstatus = 2 
                ) 
          and dco.CourseId={0}
) bb_yxt
WHERE rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", CourseId, pageLength);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Dep_CourseOrder>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 讲师端考试人员都绑定出来
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public List<Dep_CourseOrder> GetTeacherOnLineTest(out int totalCount, int CourseId, int startIndex = 0,
                                                                 int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string count = string.Format(@"
select count(1) from
(
select 
	row_number() over( ORDER BY dco.CourseId DESC) as rowNum,
	dco.CourseId,
	u.UserId,
	u.Realname ,
	u.DeptName ,
	u.JobNum,
	dcp.Id as CoPaperID,
	dcp.PaperId as PaperId,
	dcp.TestTimes,
	dcp.LevelScore
 from Dep_CourseOrder dco 
 left join Dep_Course cc on dco.CourseId=cc.Id
 left join Dep_Coursepaper dcp on dco.CourseId=dcp.CourseId
 left join Sys_User u on u.UserId=dco.UserId
  where (
            (dco.orderstatus = 1 and dco.isleave = 0 )
            or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
            or dco.orderstatus = 2 
        ) 
        and dco.CourseId={0}  and cc.IsTest=1 
    )bb", CourseId);

                totalCount = connection.Query<int>(count).First();

                string sql = string.Format(@"

select top({1}) * from
(
select 
	row_number() over( ORDER BY dco.CourseId DESC) as rowNum,
	dco.CourseId,
	u.UserId,
	u.Realname ,
	u.DeptName ,
	u.JobNum,
	dcp.Id as CoPaperID,
	dcp.PaperId as PaperId,
	dcp.TestTimes,
	dcp.LevelScore
 from Dep_CourseOrder dco 
 left join Dep_Course cc on dco.CourseId=cc.Id
 left join Dep_Coursepaper dcp on dco.CourseId=dcp.CourseId
 left join Sys_User u on u.UserId=dco.UserId
  where (
            (dco.orderstatus = 1 and dco.isleave = 0 )
            or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
            or dco.orderstatus = 2 
        ) 
        and dco.CourseId={0}  and cc.IsTest=1 
    )bb_yxt 
    where  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", CourseId, pageLength);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Dep_CourseOrder>(sql, parameters).ToList();


            }
        }


        /// <summary>
        /// 查询课程详情 update by yxt(报名人数只统计预订+指定，不统计不预定的)
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public LiXinModels.DepCourseManage.Dep_Course GetCourseById(int courseId, int userId = 0)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = @"select (
                                     select count(0) from Dep_CourseOrder 
                                    left join Sys_user on  Dep_CourseOrder.UserId = Sys_user.userid 
                                    where Dep_CourseOrder.courseid = Dep_Course.id 
                                    and Dep_CourseOrder.[OrderStatus]=1 
                                    and ( Dep_CourseOrder.[IsLeave] = 0 
                                            or (Dep_CourseOrder.[IsLeave] = 1 and Dep_CourseOrder.[ApprovalFlag] in (0,2))
                                        )
                                    and Sys_user.IsDelete = 0
                                    --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Dep_Course.OpenLevel))
                                ) as HasEntered,Dep_Course.*,Sys_User.RealName as TeacherName 
                                from Dep_Course 
                                left join Sys_User on Sys_User.UserId = Dep_Course.Teacher 
                                where id=@id";
                if (userId != 0)
                {
                    sqlstr = string.Format(@"select 
(
    select count(0) from Dep_CourseOrder 
    left join Sys_user on  Dep_CourseOrder.UserId = Sys_user.userid 
    where Dep_CourseOrder.courseid = Dep_Course.id 
    and Dep_CourseOrder.[OrderStatus]=1 
    and ( Dep_CourseOrder.[IsLeave] = 0 
        or (Dep_CourseOrder.[IsLeave] = 1  and Dep_CourseOrder.[ApprovalFlag] in (0,2))
    ) 
    and Sys_user.IsDelete = 0
) as HasEntered
,Dep_Course.*
,Sys_User.RealName as TeacherName 
,Dep_CourseOrder.[Id] as courseOrderId
,Dep_CourseOrder.[UserId]
,Dep_CourseOrder.[OrderTime]
,Dep_CourseOrder.[OrderStatus] as MyStatus
,Dep_CourseOrder.[IsLeave] as MyLeave
,Dep_CourseOrder.[ApprovalFlag]
,Dep_CourseOrder.IsAppoint as IsAppoint
,Dep_CourseOrder.GetScore
,Dep_ClassRoom.RoomName as RoomName
from Dep_Course 
left join Sys_User on Sys_User.UserId = Dep_Course.Teacher 
left join Dep_CourseOrder on Dep_CourseOrder.courseid = Dep_Course.id and Dep_CourseOrder.userid ={0}
left join Dep_ClassRoom on Dep_ClassRoom.Id = Dep_Course.RoomId
where Dep_Course.id=@id", userId);
                }
                return
                    connection.Query<LiXinModels.DepCourseManage.Dep_Course>(sqlstr, new { id = courseId }).FirstOrDefault();
            }
        }



        /// <summary>
        ///     根据ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Dep_CourseOrder Get(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = "select * from Dep_CourseOrder where id=@id";
                return connection.Query<Dep_CourseOrder>(sqlstr, new { Id }).FirstOrDefault();
            }
        }



        /// <summary>
        /// 部门分所-我的可预定页面
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userid"></param>
        /// <param name="DepartId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Dep_Course> GetDepCourseList(out int totalCount, int userid, int DepartId, string where = " 1=1", int startIndex = 0,
                                                                              int pageLength = int.MaxValue, string orderBy = "ORDER BY  aa.CourseId", string orderWhere = " (tt.courseid is null or tt.OrderStatus!=1)")
        {
            using (var connection = OpenConnection())
            {
                string sql = string.Format(@"
  select count(1) from 
 (
 SELECT row_number() over( ORDER BY  aa.CourseId) as rowNum,* FROM
	(
	 select 
		Sys_User.Realname as TeacherName,
		dep_ClassRoom.RoomName as RoomName,
		dep_Course.Id as CourseId,
		dep_Course.CourseName,
		dep_Course.Way,
		dep_Course.Sort,
		dep_Course.IsMust,
        dep_Course.AdFlag,
        dep_Course.IsTest,
		dep_Course.StartTime,
		dep_Course.EndTime,
		dep_Course.CourseLength,
        dep_Course.NumberLimited,
        dep_Course.StopOrderFlag,
        (select count(1) from Dep_CourseOrder where CourseId=Dep_Course.Id and (( OrderStatus=1 and IsLeave=1 and ApprovalFlag!=1) or (OrderStatus=1 and IsLeave=0)) ) as HasEntered
   from dep_course 
	left join Sys_User on Sys_User.UserId=Dep_Course.Teacher
	left join Dep_ClassRoom on Dep_ClassRoom.Id=Dep_Course.RoomId
left join (select Dep_courseorder.courseid,Dep_courseorder.OrderStatus from Dep_courseorder left join Dep_course on Dep_courseorder.courseid=dep_course.id
				where Dep_courseorder.userid={0}) as tt
				on tt.courseid=dep_course.id
        where dep_Course.IsDelete = 0  
    and dep_Course.Publishflag = 1 
    and dep_course.IsOrder!=2
and dep_Course.CourseFrom = 2 
and {3}
and dep_Course.Way = 1
and ((
        (
			(select count(*) from Sys_User where UserId={0} and TrainGrade in (select id from dbo.F_SplitIDs(dep_Course.OpenLevel)))>0  
			 
		) 
		and 
		(
			(
				
				(dep_Course.OpendepartObject = '' or dep_Course.OpendepartObject is null )
			) 
			or 
				(select count(*) from Sys_User where UserId={0} and deptid in (select id from dbo.F_SplitIDs(dep_Course.OpendepartObject)))>0  
		) 
    ) 
    )
		)aa 
		where {2}
	)bb
", userid, DepartId, where, orderWhere);

                totalCount = connection.Query<int>(sql).First();


                string strsql = string.Format(@"
  select top ({3}) * from 
 (
 SELECT row_number() over( {4}) as rowNum,* FROM
	(
	 select 
		Sys_User.Realname as TeacherName,
		dep_ClassRoom.RoomName as RoomName,
		dep_Course.Id as CourseId,
		dep_Course.CourseName,
		dep_Course.Way,
		dep_Course.Sort,
		dep_Course.IsMust,
dep_Course.AdFlag,
        dep_Course.IsTest,
        Dep_Course.CourseTimes,
		dep_Course.StartTime,
		dep_Course.EndTime,
		dep_Course.CourseLength,
        dep_Course.NumberLimited,
        dep_Course.StopOrderFlag,
        tt.[OrderStatus] as MyStatus,
        yy.CourseTimesOrder,
		yy.TotalCourseTimes,tt.courseId IsOrder,tt.OrderStatus isOrderStatus,
        (select count(1) from Dep_CourseOrder where CourseId=Dep_Course.Id and (( OrderStatus=1 and IsLeave=1 and ApprovalFlag!=1) or (OrderStatus=1 and IsLeave=0)) ) as HasEntered
   from dep_course 
	left join Sys_User on Sys_User.UserId=Dep_Course.Teacher
    left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			                   count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
                       from Dep_Course cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) yy 
                       on yy.id = Dep_Course.id and yy.code = Dep_Course.code
	left join Dep_ClassRoom on Dep_ClassRoom.Id=Dep_Course.RoomId
    left join (select Dep_courseorder.courseid,Dep_courseorder.OrderStatus from Dep_courseorder left join Dep_course on Dep_courseorder.courseid=dep_course.id
				where Dep_courseorder.userid={0}) as tt
				on tt.courseid=dep_course.id
        	where dep_Course.IsDelete = 0 
and dep_Course.Publishflag = 1 
and dep_course.IsOrder!=2
and dep_Course.CourseFrom = 2 
and {5}
and dep_Course.Way = 1
and ((
        (
			(select count(*) from Sys_User where UserId={0} and TrainGrade in (select id from dbo.F_SplitIDs(dep_Course.OpenLevel)))>0  
			 
		) 
		and 
		(
			(
				
				(dep_Course.OpendepartObject = '' or dep_Course.OpendepartObject is null )
			) 
			or 
				(select count(*) from Sys_User where UserId={0} and deptid in (select id from dbo.F_SplitIDs(dep_Course.OpendepartObject)))>0  
		) 
    ) 
    )

		)aa 
		where {2}
	)bb
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)

", userid, DepartId, where, pageLength, orderBy, orderWhere);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Dep_Course>(strsql, parameters).ToList();
            }
        }

        /// <summary>
        /// 部门分所-预定
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public int GetYuDing(int courseid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"
      select 
	(select count(1) from Dep_CourseOrder where CourseId={0}
	and (( OrderStatus=1 and IsLeave=1 and ApprovalFlag!=1) or (OrderStatus=1 and IsLeave=0))	
	) as DeptHasEntered, 
  * from Dep_Course where id={0}", courseid);
                var model = conn.Query<LiXinModels.DepCourseManage.Dep_Course>(sql).FirstOrDefault();

                if (model == null)
                    return 0;//不可报名
                if (model.DeptHasEntered >= model.NumberLimited)
                {
                    return 0;//不能报名
                }
                if (model.StartTime < DateTime.Now)
                {
                    return 0;//补课报名
                }
                //if (model.StopDucueFlag == 1)
                //{
                //    return 1;//已关闭排队
                //}
                if (model.StopOrderFlag == 1)
                {
                    return 2;//关闭报名
                }

                return 3;//可以报名
            }
        }


        /// <summary>
        /// 根据课程ID和用户ID找出信息
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Dep_CourseOrder GetDep_CourseOrderByCourseIdAndUserId(int CourseId, int UserId)
        {
            using (var conn = OpenConnection())
            {
                string sql = "select * from Dep_CourseOrder where CourseId=" + CourseId + " and UserId=" + UserId;
                return conn.Query<Dep_CourseOrder>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 部门分所-我的已预定页面
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="deptid"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Dep_Course> GetMyCourseOrderList(out int totalCount, int userId,
                                                                              string where = " 1 = 1 ",
                                                                              int startIndex = 0,
                                                                              int pageLength = int.MaxValue,
                                                                              string orderBy =
                                                                                  "ORDER BY Dep_Course.id DESC",int apptime=0)
        {

            using (IDbConnection connection = OpenConnection())
            {


                string sql = string.Format(@"
select count(1) from(
                SELECT row_number() over( ORDER BY  StartTime desc) as rowNum, * FROM
(
	SELECT 		
		Dep_Course.StartTime
			    
	FROM Dep_CourseOrder left join Dep_Course on Dep_CourseOrder.CourseId=Dep_Course.ID
						left join Sys_User on Dep_Course.Teacher=Sys_User.UserId                        
						left join Dep_ClassRoom on  Dep_Course.RoomId=Dep_ClassRoom.id
                        left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			                   count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
                       from Dep_Course cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
                       on tt.id = Dep_Course.id and tt.code = Dep_Course.code
	WHERE  {1} and Dep_Course.IsDelete=0
	and Dep_CourseOrder.UserId={0} and 
	(
		Dep_CourseOrder.OrderStatus in(1) or 
		(
			Dep_CourseOrder.IsLeave=1 and Dep_CourseOrder.ApprovalFlag in (0,2)
		)
	)
)bb
)aa

", userId, where);

                totalCount = connection.Query<int>(sql).First();


                string query = string.Format(@"
select top({1}) * from(
                SELECT row_number() over( ORDER BY StartTime desc) as rowNum, * FROM
(
	SELECT 
		Dep_CourseOrder.ID
		,Dep_Course.CourseName
		,Dep_Course.NumberLimited as NumberLimited
		,Dep_Course.CourseLength
        ,Dep_Course.Way
        ,Dep_Course.Sort
		,Dep_Course.IsMust
        ,Dep_Course.IsTest
        ,Dep_Course.IsLeave
		,Dep_Course.StartTime
		,Dep_Course.EndTime
        ,Dep_Course.CourseTimes   
        --,Dep_Course.IsAppoint    
		,Sys_User.Realname as TeacherName
		,Dep_ClassRoom.RoomName as RoomName
		,Dep_CourseOrder.PassStatus as PassStatus
		,CASE WHEN (dbo.F_NewIsAppStatus(Dep_Course.Id,Dep_CourseOrder.UserId,Dep_CourseOrder.OrderStatus)=1) THEN Dep_CourseOrder.GetScore ELSE 0 END  as GetScore
		,Dep_CourseOrder.CourseId as CourseId
        ,Dep_CourseOrder.OrderTime
        ,Dep_CourseOrder.OrderStatus
		,Dep_CourseOrder.UserId
        ,Dep_CourseOrder.[Id] as courseOrderId
        ,Dep_CourseOrder.[OrderStatus] as MyStatus      
        ,Dep_Course.YearPlan
        ,Dep_CourseOrder.[IsLeave] as MyLeave       
        ,Dep_CourseOrder.IsAppoint as IsAppoint
        ,Dep_CourseOrder.ApprovalFlag as ApprovalFlag
        ,tt.CourseTimesOrder
		,tt.TotalCourseTimes	  
        ,Dep_CourseOrder.AttScore as AttScore
        ,Dep_CourseOrder.EvlutionScore as EvlutionScore
        ,Dep_CourseOrder.ExamScore as ExamScore 
        ,Dep_Attendce.ApprovalFlag as attApprovalFlag
        ,Dep_CourseOrder.LeaveTime as LeaveTime
        ,Dep_CourseOrder.ApprovalLimitTime as AppTime
	FROM Dep_CourseOrder left join Dep_Course on Dep_CourseOrder.CourseId=Dep_Course.ID
						left join Sys_User on Dep_Course.Teacher=Sys_User.UserId                        
						left join Dep_ClassRoom on  Dep_Course.RoomId=Dep_ClassRoom.id
                        left join Dep_Attendce on Dep_Attendce.CourseId=Dep_CourseOrder.CourseId and 
									Dep_Attendce.UserId={0}
                        left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			                   count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
                       from Dep_Course cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
                       on tt.id = Dep_Course.id and tt.code = Dep_Course.code
	WHERE  {2} and Dep_Course.IsDelete=0
	and Dep_CourseOrder.UserId={0} and 
	(
		Dep_CourseOrder.OrderStatus in(1) or 
		(
			Dep_CourseOrder.IsLeave=1 and Dep_CourseOrder.ApprovalFlag in (0,2)
		)
	)
) bb

)aa where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", userId, pageLength, where, apptime);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Dep_Course>(query, parameters).ToList();

            }

        }

        /// <summary>
        /// 查看个人总退订次数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetAllCourseOrderTimes(int userid,int year=-1)
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"select isnull(sum(OrderTimes),0) from Dep_CourseOrder dco
LEFT JOIN Dep_Course dc ON dc.Id=dco.CourseId  
where UserId={0} {1}", userid, year == -1 ? "" : " AND dc.Year=" + year);
                return conn.Query<int>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 修改退订状态
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="OrderStatus"></param>
        /// <returns></returns>
        public bool UpdateOrderStatus(int courseid, int userid, int OrderStatus)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"
update Dep_CourseOrder set OrderStatus={0},OrderTimes=OrderTimes+1,IsLeave=0 where CourseId={1} and UserId={2} 
", OrderStatus, courseid, userid);

                return conn.Execute(sql) > 0;
            }
        }

        /// <summary>
        /// 请假
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateLeave(int id, string Reason, int courseid, int userid,Sys_User leader,double configHour)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format("update Dep_CourseOrder set IsLeave=1,ApprovalFlag=0,Reason='{0}',LeaveTime=getdate(),ApprovalUserId={3},ApprovalUser='{4}',ApprovalLimitTime=dateadd(minute,{5},getdate()),ApprovalTime=getdate() where CourseId={1} and UserId={2}", Reason, courseid, userid, leader.UserId, leader.JobNum, configHour);

                return conn.Execute(sql) > 0;
            }
        }

        /// <summary>
        /// 课程预定查询列表
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        public List<Dep_Course> GetCourseSubscribeList(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Id")
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"
 SELECT * FROM(
SELECT row_number()OVER(ORDER BY {0}) number,count(*)OVER(PARTITION BY null) totalcount,dc.* 
, (select count(0) from Dep_CourseOrder  dco
   WHERE dco.CourseId=dc.Id
   AND  ((dco.orderstatus = 1 and dco.isleave = 0 )
				   or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)))
  ) as HasEntered
,0 as SuccessEntered,su.Realname TeacherName
FROM Dep_Course dc
LEFT JOIN Sys_User su ON su.UserId=dc.Teacher
WHERE dc.IsDelete=0 AND dc.Publishflag=1 and dc.CourseFrom=2 and dc.Way = 1   
and {1})t
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", jsRenderSortField, where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Dep_Course>(sql, param).ToList();
            }
        }


        /// <summary>
        /// 修改是否允许报名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status">1关闭  0开启</param>
        /// <returns></returns>
        public bool UpdateOrderFlag(int id, int status)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"Update Dep_Course set StopOrderFlag=@OrderFlag where Id = " + id;
                var param = new
                {
                    OrderFlag = status
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 获取报名成功的人员，根据培训级别分
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<NameSubscribeCount> GetSuccessTrainGradeSubscribe(int deptID, int courseID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"DECLARE @OpenLevel NVARCHAR(500),@OpenDepartObject NVARCHAR(500),@OpenPerson NVARCHAR(500)
SELECT @OpenLevel=isnull(OpenLevel,''),@OpenDepartObject=isnull(OpenDepartObject,''),@OpenPerson=isnull(OpenPerson,'') FROM Dep_Course
WHERE Id={0}

   SELECT    TrainGrade Name,count(UserId) SubscribeCount FROM(
SELECT dc.UserId,su.TrainGrade FROM Dep_CourseOrder  dc
LEFT JOIN  Sys_User su    ON dc.UserId=su.UserId
WHERE CourseId={0} AND su.IsDelete=0 AND ((dc.orderstatus = 1 and dc.isleave = 0 )
				   or (dc.orderstatus = 1 and dc.isleave = 1 and dc.approvalflag in (0,2))) 
)t GROUP BY   TrainGrade", courseID);
                return conn.Query<NameSubscribeCount>(sql).ToList();
            }
        }

        /// <summary>
        /// 课程预定查询中能管理的所有人员
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetCourseSubscribeUserList(int courseID, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " UserId")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM( 
 SELECT row_number()OVER(ORDER BY {2} ) number,count(*)OVER(PARTITION BY null) totalCount,* FROM(
SELECT su.*,dc.OrderStatus,dc.OrderTime,isnull(dc.IsAppoint,-1) IsAppoint,dc.OrderTimes,isleave,approvalflag FROM  Sys_User su
LEFT JOIN (SELECT * from  Dep_CourseOrder  WHERE CourseId={0} ) dc   ON dc.UserId=su.UserId
 WHERE  su.IsDelete=0  
)t where  {1} )result
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", courseID, where, jsRenderSortField);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };

                return conn.Query<Sys_User>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 插入考勤分数(带部门)
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="departId"></param>
        /// <param name="attScore"></param>
        /// <returns></returns>
        public bool ExistDeptTran_courseOrder(int courseId, int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select count(1) from Dep_CourseOrder where courseId=@courseId and userId=@userId");
                var param = new
                {
                    courseId = courseId,
                    userId = userId
                };
                int count = connection.Query<int>(strSql.ToString(), param).FirstOrDefault();
                return count > 0;
            }
        }

        /// <summary>
        /// 判断是否存在该科目该学员的考勤记录
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool InsertDeptTran_courseOrder(int courseId, int userId, int departId, decimal attScore, decimal EvlutionScore = 0, decimal ExamScore = 0)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO Dep_CourseOrder(CourseId,UserId,OrderTime,OrderStatus,LearnStatus,GetScore,IsAppoint,PassStatus ,AttScore,DepartSetId,OrderTimes)" +
                              " VALUES " +
                              " (@courseId,@userId,1991-1-1,0,0,0,0,0,@attScore,@departId,0)");
                var param = new
                {
                    courseId = courseId,
                    userId = userId,
                    departId = departId,
                    attScore = attScore,
                    EvlutionScore = EvlutionScore,
                    ExamScore = ExamScore
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 更新考勤分数
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="attScore"></param>
        /// <returns></returns>
        public bool DepTran_CourseOrderUsers(int courseId, int userId, decimal attScore, int type, decimal EvlutionScore = 0, decimal ExamScore = 0)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                if (type == 1)
                {
                    strSql.Append(
                        "update Dep_CourseOrder set AttScore=@attScore where CourseId=@courseId and UserId=@userId");
                }
                else
                {
                    strSql.Append(
                        "update Dep_CourseOrder set AttScore=@attScore,EvlutionScore=@EvlutionScore,ExamScore=@ExamScore where CourseId=@courseId and UserId=@userId");
                }
                
                var param = new
                {
                    courseId = courseId,
                    userId = userId,
                    attScore = attScore,
                     EvlutionScore = EvlutionScore,
                    ExamScore = ExamScore
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        #region == 部门课程指定查询 ==
        /// <summary>
        /// 查询课程中学员的预订情况
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="courseId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Sys_User> GetDepCourseSubscribeUserList(out int totalCount, int courseId,
                                                                          string where = " 1 = 1 ", int startIndex = 0,
                                                                          int pageLength = int.MaxValue,
                                                                          string orderBy =
                                                                              " ORDER BY u.userId DESC ")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>(string.Format(@" select count(1)
from sys_user u 
left join (select * from Dep_CourseOrder where Dep_CourseOrder.courseId = {0} )  dco on dco.userid = u.userid 
left join Dep_Course cc on cc.Id = {0} 
where " + where, courseId)).First();
                string query = string.Format(@"
                select top {0} * from (
                    select  row_number() over( {1} ) as rowNum
                        ,dco.OrderStatus AS IsApply
,dco.IsAppoint
,dco.orderstatus as CourseOrder
,dco.isleave as CourseLeave
,dco.ApprovalTime as CourseApprovalDateTime
,dco.ApprovalFlag as CourseLeaveApprovalFlag
,dco.OrderTime as OrderTime
,u.* 
from sys_user u 
left join (select * from Dep_CourseOrder where Dep_CourseOrder.courseId = {2} )  dco on dco.userid = u.userid 
left join Dep_Course cc on cc.Id = {2} 
                        where " + where + @" 
                ) result 
                where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy,
                                             courseId);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Sys_User>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 0:不可指定
        /// 1:可直接指定
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public int GetDepCanSignupSpecialCrowdUserToCourse(int courseid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"select 
                            cc.* 
                            ,(select count(*) from Dep_CourseOrder dco 
                                where dco.courseid = cc.id 
				                       And (   
						                      (dco.orderstatus = 1 and dco.isleave = 0 )
						                       or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
						                    ) 
                            ) as HasEntered --已报名人员总数 
                            from Dep_Course cc 
                            where cc.Id = {0} and cc.IsDelete = 0 
                                and cc.Publishflag = 1 
                                and cc.CourseFrom=2 --课程管理 
		                        and cc.IsOpenOthers=0 --部门自学 
		                        and cc.IsOrder in (2,3) --指定，兼有
                                and cc.StartTime > '{1}' ", courseid, DateTime.Now);
                var model = conn.Query<Dep_Course>(sqlwhere).FirstOrDefault();
                if (model == null)
                    return 0; //不可报名
                if (model.NumberLimited > model.HasEntered)
                {
                    return 1;//可直接指定
                }
                return 0; //不可报名
            }
        }

        /// <summary>
        /// 撤销部门指定人员
        /// </summary>
        /// <param name="sqlstr"></param>
        public void DepDropAssignUser(string sqlstr)
        {
            using (var connection = OpenConnection())
            {
                connection.Execute(sqlstr);
            }
        }
        #endregion



        /// <summary>
        /// 根据课程ID和用户ID找出信息
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Dep_CourseOrder GetCo_CourseMainPaperByCourseIdAndUserId(int CourseId, int UserId)
        {
            using (var conn = OpenConnection())
            {
                string sql = "select * from Dep_CourseOrder where CourseId=" + CourseId + " and UserId=" + UserId;
                return conn.Query<Dep_CourseOrder>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 修改 条件自己加
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool UpdateWhere(string where)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "update Dep_CourseOrder " + where;
                return connection.Execute(sql) > 0;
            }
        }


        /// <summary>
        /// 部门报名明细
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Sys_Department> GetDeptSubscribe(int courseId)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"DECLARE @OpenLevel NVARCHAR(500),@OpenDepartObject NVARCHAR(500)
SELECT @OpenLevel=isnull(OpenLevel,''),@OpenDepartObject=isnull(OpenDepartObject,'') FROM Dep_Course
WHERE Id={0}


SELECT *,
(SELECT count(1) FROM Sys_User su
WHERE su.DeptId=Sys_Department.DepartmentId AND
 (su.TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))  OR @OpenLevel='')
) AllCount,
(SELECT COUNT(1) FROM Dep_CourseOrder dc
 LEFT JOIN Sys_User su ON su.UserId=dc.UserId
 WHERE su.DeptId= Sys_Department.DepartmentId AND dc.CourseId={0}  AND  ((dc.orderstatus = 1 and dc.isleave = 0 )
				   or (dc.orderstatus = 1 and dc.isleave = 1 and dc.approvalflag in (0,2)))  and 
 (su.TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))  OR @OpenLevel='')
)   SubscribeCount
 FROM Sys_Department
WHERE (DepartmentId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenDepartObject)) OR  @OpenDepartObject='')", courseId);
                return conn.Query<Sys_Department>(sql).ToList();
            }
        }

        /// <summary>
        /// 培训级别报名明细
        /// </summary>
        /// <param name="courseId"></param>
        public List<NameSubscribeCount> GetTrainGradeSubscribe(int courseId)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"DECLARE @OpenLevel NVARCHAR(500),@OpenDepartObject NVARCHAR(500)
SELECT @OpenLevel=isnull(OpenLevel,''),@OpenDepartObject=isnull(OpenDepartObject,'') FROM Dep_Course
WHERE Id={0}


 SELECT DISTINCT su.TrainGrade Name, 
 (SELECT COUNT(*) FROM Sys_User
 WHERE (DeptId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenDepartObject)) OR  @OpenDepartObject='')
 AND TrainGrade=su.TrainGrade) AllCount,
 (SELECT COUNT(1) FROM Dep_CourseOrder dc
 LEFT JOIN Sys_User su1 ON su1.UserId=dc.UserId
 WHERE (DeptId IN (SELECT ID FROM dbo.F_SplitIDs(@OpenDepartObject)) OR  @OpenDepartObject='')
 AND dc.CourseId={0} AND  ((dc.orderstatus = 1 and dc.isleave = 0 )
				   or (dc.orderstatus = 1 and dc.isleave = 1 and dc.approvalflag in (0,2)))  AND  su1.TrainGrade=su.TrainGrade) SubscribeCount
  FROM Sys_User  su
WHERE (su.TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs(@OpenLevel))  OR @OpenLevel='')", courseId);
                return conn.Query<NameSubscribeCount>(sql).ToList();
            }
        }



        /// <summary>
        /// 我的考勤及学时页面列表
        /// </summary>
        /// <returns></returns>
        public List<Dep_CourseOrder> GetMyAttendceList(out int totalCount, int userId,
                                                                              string where = " 1 = 1 ",
                                                                              int startIndex = 0,
                                                                              int pageLength = int.MaxValue,
                                                                              string orderBy =
                                                                                  "ORDER BY Dep_Course.id DESC")
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"
SELECT count(1) FROM
(
  select row_number() over( ORDER BY Dep_Course.StartTime ASC) as rowNum
		,Dep_CourseOrder.ID
		,Dep_Course.CourseName as CourseName
        ,Dep_Course.YearPlan
        ,Dep_Course.Way
        ,Dep_Course.Sort
        ,Dep_Course.StartTime 
        ,Dep_Course.EndTime 
        ,Dep_Course.CourseLength
		,Dep_Course.IsTest
        ,Dep_Course.IsMust
        ,Dep_Course.AttFlag as AttFlag
		,Dep_Course.NumberLimited as NumberLimited			
		,Dep_CourseOrder.PassStatus		
		,Dep_CourseOrder.CourseId as CourseId
		,Dep_CourseOrder.UserId
		,Dep_CourseOrder.AttScore
		,Dep_CourseOrder.EvlutionScore
		,Dep_CourseOrder.ExamScore
		,Dep_Attendce.StartTime as AttendceStartTime
		,Dep_Attendce.EndTime as AttendceEndTime		
        ,Sys_User.Realname as Realname	
   from dep_courseorder 
	left join dep_course on dep_courseorder.courseid=dep_course.id
	left join Sys_User on Dep_Course.Teacher=Sys_User.UserId
    left join Dep_Attendce on Dep_Attendce.CourseId=Dep_CourseOrder.CourseId 
      where {1} and  (
				(Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 0) 
				 or
				 (Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 1 and Dep_CourseOrder.approvalflag in (0,2))
				
			)
			and Dep_CourseOrder.UserId={0} 
		) bb	


", userId, where);

                totalCount = conn.Query<int>(sql).First();


                string sqlstr = string.Format(@"
SELECT top({2}) * FROM
(
  select row_number() over( ORDER BY Dep_Course.StartTime ASC) as rowNum
		,Dep_CourseOrder.ID
		,Dep_Course.CourseName  as CourseName
        ,Dep_Course.YearPlan
        ,Dep_Course.Way
        ,Dep_Course.Sort
        ,Dep_Course.StartTime as StartTime 
        ,Dep_Course.EndTime  as EndTime
        ,Dep_Course.CourseLength
		,Dep_Course.IsTest
        ,Dep_Course.IsMust
        ,Dep_Course.AttFlag as AttFlag
		,Dep_Course.NumberLimited as NumberLimited			
		,Dep_CourseOrder.PassStatus		
		,Dep_CourseOrder.CourseId as CourseId
		,Dep_CourseOrder.UserId
		,Dep_CourseOrder.AttScore
		,Dep_CourseOrder.EvlutionScore
		,Dep_CourseOrder.ExamScore
		,Dep_Attendce.StartTime as AttendceStartTime
		,Dep_Attendce.EndTime as AttendceEndTime		
        ,Sys_User.Realname as Realname	
   from dep_courseorder 
	left join dep_course on dep_courseorder.courseid=dep_course.id
	left join Sys_User on Dep_Course.Teacher=Sys_User.UserId
    left join Dep_Attendce on Dep_Attendce.CourseId=Dep_CourseOrder.CourseId 
      where {1} and  (
				(Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 0) 
				 or
				 (Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 1 and Dep_CourseOrder.approvalflag in (0,2))
				
			)
			and Dep_CourseOrder.UserId={0} 
		) bb	
WHERE  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", userId, where, pageLength);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return conn.Query<Dep_CourseOrder>(sqlstr, parameters).ToList();

            }
        }

        /// <summary>
        /// 学员端-我的考试
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Dep_CourseOrder> GetMyExamList(out int totalCount, int userId,
                                                                              string where = " 1 = 1 ",
                                                                              int startIndex = 0,
                                                                              int pageLength = int.MaxValue,
                                                                              string orderBy =
                                                                                  "ORDER BY Dep_Course.id DESC")
        {
            using (var conn = OpenConnection())
            {
                string sql = string.Format(@"
select count(1) from
(
select row_number() over( ORDER BY Dep_Course.id desc) as rowNum,
		Dep_Course.ID
		,Dep_Course.CourseName as CourseName
        ,Dep_Course.Sort as Sort
        ,Dep_Course.Way
        ,Dep_Course.StartTime
        ,Dep_Course.EndTime
        ,Dep_Course.NumberLimited as NumberLimited
        ,Dep_CourseOrder.GetScore as GetScore
        ,Dep_CoursePaper.PaperId as PaperId
		,Dep_CoursePaper.TestTimes as ExamALLTestTimes
        ,Dep_CoursePaper.[Length] as Length	
        ,Dep_CourseOrder.PassStatus
        --,Dep_CpaLearnStatus.IsPass as IsPass
        ,Dep_CoursePaper.LevelScore
 from dbo.Dep_Course left join dbo.Dep_CourseOrder on Dep_Course.Id=Dep_CourseOrder.CourseId
                    left join dbo.Dep_Coursepaper on Dep_Course.id=Dep_Coursepaper.CourseId
                    where {1} and IsTest=1 
                    and  (
						(Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 0) 
						 or
						 (Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 1 and Dep_CourseOrder.approvalflag in (0,2))
						
					)
					and Dep_CourseOrder.UserId={0}
                    
)bb 	

", userId, where);

                totalCount = conn.Query<int>(sql).First();


                string sqlstr = string.Format(@"
SELECT top({2}) * FROM
(
select row_number() over( ORDER BY Dep_Course.id desc) as rowNum,
		Dep_Course.ID
		,Dep_Course.CourseName as CourseName
        ,Dep_Course.Sort as Sort
        ,Dep_Course.Way
        ,Dep_Course.StartTime
        ,Dep_Course.EndTime
        ,Dep_Course.NumberLimited as NumberLimited
        ,Dep_CourseOrder.GetScore as GetScore
        ,Dep_CoursePaper.Id as CoPaperID
        ,Dep_CoursePaper.PaperId as PaperId
		,Dep_CoursePaper.TestTimes as ExamALLTestTimes
        ,Dep_CoursePaper.[Length] as Length	
        ,Dep_CourseOrder.PassStatus
        --,Dep_CpaLearnStatus.IsPass as IsPass
        ,Dep_CoursePaper.LevelScore
 from dbo.Dep_Course left join dbo.Dep_CourseOrder on Dep_Course.Id=Dep_CourseOrder.CourseId
                    left join dbo.Dep_Coursepaper on Dep_Course.id=Dep_Coursepaper.CourseId
                    where {1} and IsTest=1 
                    and  (
						(Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 0) 
						 or
						 (Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 1 and Dep_CourseOrder.approvalflag in (0,2))
						
					)
					and Dep_CourseOrder.UserId={0}
		) bb	
WHERE  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", userId, where, pageLength);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return conn.Query<Dep_CourseOrder>(sqlstr, parameters).ToList();

            }
        }


        /// <summary>
        /// 获取集中授课中报名的人员
        /// </summary>
        public List<Sys_User> GetDepCourseOrderName(int courseID, string where = "1=1", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " su.UserId asc")
        {
            using (var connection = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT row_number()OVER(ORDER BY {2}) number,count(*)OVER(PARTITION BY null) totalcount,
OrderTime,su.* FROM Dep_CourseOrder cco LEFT JOIN Sys_User su ON cco.UserId=su.UserId
WHERE OrderStatus>0 AND CourseId={0} and {1}) r
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", courseID, where, jsRenderSortField);
                var param = new
                {
                    startIndex = pageIndex,
                    startLength = pageSize
                };
                return connection.Query<Sys_User>(sql, param).ToList();

            }
        }

        #region 我的授课课程
        /// <summary>
        /// 查询我的授课的课程下报名成功的人员信息
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetCourseUserListByTeacher(int courseID, int startIndex = 1, int startLength = int.MaxValue)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (
SELECT row_number()OVER(ORDER BY su.UserId DESC ) number,count(*)OVER(PARTITION BY null) totalCount,su.*,IsAppoint FROM Sys_User  su
INNER JOIN (
SELECT UserId,IsAppoint from  Dep_CourseOrder  WHERE CourseId={0} 
AND  ((orderstatus = 1 and isleave = 0 )
  or (orderstatus = 1 and isleave = 1 and approvalflag in (0,2)))
  )t ON t.UserId=su. UserId
  )r
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", courseID);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_User>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 我的课程讲师端学员端列表(加入考试成绩)
        /// </summary>
        public List<Dep_CourseOrder> GetCourseTeacherOnLineTest(int courseID, int startIndex = 1, int startLength = int.MaxValue)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@" SELECT * FROM (
select 
	row_number() over( ORDER BY dco.CourseId DESC) as num,
	count(*)OVER(PARTITION BY null) totalcount,
	dco.CourseId,
	Sys_User.UserId,
	Sys_User.Realname,
	Sys_User.DeptName as DeptName,
	Sys_User.JobNum,
	Dep_CoursePaper.Id as CoPaperID,
	Dep_CoursePaper.PaperId as PaperId,
	Dep_CoursePaper.TestTimes,
	Dep_CoursePaper.LevelScore
 from Dep_CourseOrder dco 
 left join Dep_Course on dco.CourseId=Dep_Course.Id
 left join Dep_CoursePaper on dco.CourseId=Dep_CoursePaper.CourseId
 left join Sys_User on Sys_User.UserId=dco.UserId
  where ((dco.orderstatus = 1 and dco.isleave = 0 )
				   or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)))   and dco.CourseId={0}
     )t 
--WHERE  num BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", courseID);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Dep_CourseOrder>(sql, param).ToList();
            }
        }
        #endregion


        /// <summary>
        /// 取消课程发布
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public bool CancelCoursePub(int courseid)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                
                    strSql.AppendFormat(@"update [Dep_Course] set
            LastUpdateTime=@LastUpdateTime,  {0}
             where Id=@Id ", " Publishflag=0 ");
              
                var param = new
                {
                    Id = courseid,
                    LastUpdateTime = DateTime.Now
                  
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 删除发布全所后预定的人员
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public void DeleteZhiDingUser(int courseid)
        {
            using (IDbConnection connection = OpenConnection())
            {
                //StringBuilder strSql = new StringBuilder();

                string sql = "delete dep_courseorder where CourseId="+courseid;


                connection.Execute(sql); 
            }
        }



        /// <summary>
        /// 获取纳入考核范围的所有学员预定信息
        /// </summary>
        /// <returns></returns>
        public List<Dep_CourseOrder> GetDep_CourseOrderList()
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@" select * from Dep_courseorder where userid in (select userid from View_CheckUser)");
               
                return conn.Query<Dep_CourseOrder>(sql).ToList();
            }
        }

        /// <summary>
        /// 部门/分所自学员工学习情况
        /// </summary>
        /// <returns></returns>
        public List<Dep_CourseOrder> GetReport_CompleteDetail(int year=2013)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@" 

 with asd as
 (
  select  objectid,Userid from Dep_Survey_ReplyAnswer where sourcefrom=1 group by objectid,Userid
 )

select dep_courseorder.*,sys_user.realname,sys_user.CPA,
	(select count(1) from Dep_CourseAdvice where Dep_CourseAdvice.userid=dep_courseorder.userid and Dep_CourseAdvice.courseid=dep_courseorder.courseid) as AttendCount,
	(select count(1) from Dep_Survey_ReplyAnswer where Dep_Survey_ReplyAnswer.userid=dep_courseorder.userid and Dep_Survey_ReplyAnswer.ObjectID=dep_courseorder.courseid and SourceFrom=0
	 	group by Dep_Survey_ReplyAnswer.ObjectID
	) as AfterCount,
	(select count(1) from asd where userid=dep_courseorder.userid) as DeptSurveyCount,
    isnull(Dep_Attendce.status,0) as status,
	isnull(Dep_Attendce.ApprovalFlag,0) as AttendceApprovalFlag,
    dep_course.yearplan
 from dep_courseorder
	left join sys_user on sys_user.userid=dep_courseorder.userid
    left join Dep_Attendce on Dep_Attendce.userid=dep_courseorder.userid and Dep_Attendce.courseid=dep_courseorder.courseid
    left join dep_course on dep_course.id=dep_courseorder.courseid
 where dep_courseorder.userid in(select userid from View_CheckUser)  and dep_course.yearplan="+year+@"

");

                return conn.Query<Dep_CourseOrder>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取课程讲师平均分 在sql里已区分好一级 
        /// </summary>
        /// <returns></returns>
        public List<Dep_CourseOrder> GetAvgSubjectiveAnswer(int year=2013)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@" 
 with  mao as
 (
select SubjectiveAnswer,ObjectID as ObjectID,Dep_Survey_ReplyAnswer.UserId as UserId,
	ROW_NUMBER() OVER(PARTITION BY Dep_Survey_ReplyAnswer.ObjectID ORDER BY (SubjectiveAnswer)) AS RowNum,
	--COUNT(*) OVER(PARTITION BY Dep_Survey_ReplyAnswer.UserId) AS Cnt,
	COUNT(*) OVER(PARTITION BY Dep_Survey_ReplyAnswer.ObjectID) AS Cnt,
	Convert(int,Dep_course.Teacher) as TeacherId
 from Dep_Survey_ReplyAnswer 
	left join Dep_Survey_Question on Dep_Survey_ReplyAnswer.questionid=Dep_Survey_Question.questionId
	left join Dep_Survey_Exampaper on Dep_Survey_ReplyAnswer.ExampaperID=Dep_Survey_Exampaper.ExampaperID
	left join Dep_course on Dep_course.id=Dep_Survey_ReplyAnswer.ObjectID
	where Dep_Survey_Question.QuestionType=3 and Dep_Survey_Exampaper.ExamType=2 --and Dep_Survey_ReplyAnswer.userid=21
	and Dep_course.yearplan=" + year+ @" --order by ObjectID
)

select avg(Convert(int,SubjectiveAnswer)) as SubjectiveAnswer,ObjectID,TeacherId,userid from
(
	select SubjectiveAnswer,RowNum,Cnt,ObjectID,TeacherId,userid
	from mao o
	where RowNum in ((Cnt + 1) / 2,(Cnt + 2) / 2) 
)a 
group by ObjectID,TeacherId,userid
");

                return conn.Query<Dep_CourseOrder>(sql).ToList();
            }
        }


    }
}
