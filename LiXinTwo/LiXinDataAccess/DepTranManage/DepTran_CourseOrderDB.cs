using LiXinModels.DepTranManage;
using Retech.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Data;
using System.Data;
using LiXinModels.CourseManage;
using LiXinModels.User;

namespace LiXinDataAccess.DepTranManage
{
    public class DepTran_CourseOrderDB : BaseRepository
    {

        /// <summary>
        /// 添加预定信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddDepTran_CourseOrder(DepTran_CourseOrder model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into DepTran_CourseOrder (CourseId,UserId,OrderTime,OrderStatus,LearnStatus,GetScore,IsAppoint,PassStatus,AttScore,EvlutionScore,ExamScore,DepartSetId,OrderTimes)
values (@CourseId,@UserId,@OrderTime,@OrderStatus,@LearnStatus,@GetScore,@IsAppoint,@PassStatus,@AttScore,@EvlutionScore,@ExamScore,@DepartSetId,@OrderTimes)";
                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.OrderTime,
                    model.OrderStatus,
                    model.LearnStatus,
                    model.GetScore,
                    model.IsAppoint,
                    model.PassStatus,
                    model.AttScore,
                    model.EvlutionScore,
                    model.ExamScore,
                    model.DepartSetId,
                    model.OrderTimes
                };
                return conn.Execute(sqlwhere, param) > 0;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateDepTran_CourseOrder(DepTran_CourseOrder model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update DepTran_CourseOrder set CourseId = @CourseId,UserId = @UserId,OrderTime = @OrderTime,OrderStatus = @OrderStatus,LearnStatus = @LearnStatus,GetScore = @GetScore,IsAppoint = @IsAppoint,PassStatus = @PassStatus,AttScore = @AttScore,EvlutionScore = @EvlutionScore,ExamScore = @ExamScore,DepartSetId = @DepartSetId,OrderTimes=@OrderTimes where Id=@Id";

                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.OrderTime,
                    model.OrderStatus,
                    model.LearnStatus,
                    model.GetScore,
                    model.IsAppoint,
                    model.PassStatus,
                    model.AttScore,
                    model.EvlutionScore,
                    model.ExamScore,
                    model.DepartSetId,
                    model.OrderTimes,
                    model.Id
                };
                int result = conn.Execute(strSql, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据课程ID和用户ID找出信息
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DepTran_CourseOrder GetCo_CourseMainPaperByCourseIdAndUserId(int CourseId, int UserId)
        {
            using (var conn = OpenConnection())
            {
                string sql = "select * from DepTran_CourseOrder where CourseId=" + CourseId + " and UserId=" + UserId;
                return conn.Query<DepTran_CourseOrder>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 查看个人总退订次数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetAllCourseOrderTimes(int userid)
        {
            using (var conn = OpenConnection())
            {
                string sql = "select isnull(sum(OrderTimes),0) from DepTran_CourseOrder where UserId=" + userid;
                return conn.Query<int>(sql).FirstOrDefault();
            }
        }

        public List<DepTran_CourseOrder> GetList(string strWhere = " 1 = 1 ")
        {
            using (var conn = OpenConnection())
            {
                string sqlwhere = "select * from DepTran_CourseOrder where " + strWhere +
                                  " order by DepTran_CourseOrder.CourseId,DepTran_CourseOrder.OrderTime";
                return conn.Query<DepTran_CourseOrder>(sqlwhere).ToList();
            }
        }

        /// <summary>
        ///     根据ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DepTran_CourseOrder Get(int Id)
        {
            using (var connection = OpenConnection())
            {
                string sqlstr = "select * from DepTran_CourseOrder where id=@id";
                return connection.Query<DepTran_CourseOrder>(sqlstr, new { Id }).FirstOrDefault();
            }
        }

        public bool Delete(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"delete DepTran_CourseOrder where id = {0}", id);
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }


        /// <summary>
        /// 老方法
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Co_Course> GetCourseList(int userid, string where = " 1=1")
        {
            using (var connection = OpenConnection())
            {
                string sqlstr = string.Format(@"

select 
Sys_User.Realname as TeacherName,
	Sys_ClassRoom.RoomName as RoomName,
    Co_Course.Id as CourseId,
 * from Co_Course 
 left join  DepTran_CourseOrder 
  on DepTran_CourseOrder.CourseId=Co_Course.Id and DepTran_CourseOrder.UserId={1} 
 left join Sys_User on Sys_User.UserId=Co_Course.Teacher
 left join Sys_ClassRoom on Sys_ClassRoom.Id=Co_Course.RoomId


where {0}
", where, userid);
                return connection.Query<Co_Course>(sqlstr).ToList();
            }
        }

        /// <summary>
        /// 可预定课程
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userid"></param>
        /// <param name="DepartId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Co_Course> GetNewCourseList(out int totalCount, int userid, int DepartId, string where = " 1=1", int startIndex = 0,
                                                                              int pageLength = int.MaxValue)
        {
            using (var connection = OpenConnection())
            {
                string sql = string.Format(@"
 select count(1) from 
 (
 SELECT * FROM
	(
	 select 
		Sys_User.Realname as TeacherName,
		Sys_ClassRoom.RoomName as RoomName,
		Co_Course.Id as CourseId,
		Co_Course.CourseName,
		Co_Course.Way,
		Co_Course.Sort,
		Co_Course.IsMust,
Co_Course.AdFlag,
        Co_Course.IsTest,
		Co_Course.StartTime,
		Co_Course.EndTime,
		Co_Course.CourseLength,
        Co_Course.StopOrderFlag
		 from Co_Course 
			left join Sys_User on Sys_User.UserId=Co_Course.Teacher
			left join Sys_ClassRoom on Sys_ClassRoom.Id=Co_Course.RoomId
				where Co_Course.id in
				(
				select DepTran_CourseOpen.CourseId from DepTran_CourseOpen 
				left join
					(select DepTran_CourseOrder.CourseId,DepTran_CourseOrder.OrderStatus,DepTran_CourseOpen.DepartId,DepTran_CourseOpen.OpenFlag from DepTran_CourseOrder
						left join  DepTran_CourseOpen on DepTran_CourseOpen.CourseId=DepTran_CourseOrder.CourseId
						where  DepTran_CourseOrder.UserId={0} and DepTran_CourseOpen.DepartId={1} ) tt
						on tt.CourseId=DepTran_CourseOpen.CourseId
						where  (tt.CourseId is null or tt.OrderStatus!=1) and  DepTran_CourseOpen.DepartId={1} and DepTran_CourseOpen.OpenFlag=1
				)  and Co_Course.StartTime>getdate() and co_course.isdelete=0
		)aa 
		where {2}
	)bb
", userid, DepartId, where);

                totalCount = connection.Query<int>(sql).First();


                string sqlstr = string.Format(@"
    select top ({3}) * from 
 (
 SELECT row_number() over( ORDER BY  aa.CourseId) as rowNum,* FROM
	(
	 select 
		Sys_User.Realname as TeacherName,
		Sys_ClassRoom.RoomName as RoomName,
		Co_Course.Id as CourseId,
		Co_Course.CourseName,
		Co_Course.Way,
		Co_Course.Sort,
		Co_Course.IsMust,
        Co_Course.IsTest,
Co_Course.AdFlag,
		Co_Course.StartTime,
		Co_Course.EndTime,
		Co_Course.CourseLength,
        --Co_Course.NumberLimited,
        Co_Course.StopOrderFlag,
    Co_Course.CourseTimes,
        tt.CourseTimesOrder,
        DepTran_CourseOpen.LimitNumber as NumberLimited,
		(select count(1) from DepTran_CourseOrder where CourseId=Co_Course.Id and OrderStatus=1 ) as HasEntered
		 from Co_Course 
			left join Sys_User on Sys_User.UserId=Co_Course.Teacher
			left join Sys_ClassRoom on Sys_ClassRoom.Id=Co_Course.RoomId
            left join DepTran_CourseOpen on DepTran_CourseOpen.CourseId=Co_Course.id  and DepTran_CourseOpen.DepartId={1}
            left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			                   count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
                       from Co_Course cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1 ) tt 
                       on tt.id = Co_Course.id and tt.code = Co_Course.code
				where Co_Course.id in
				(
				select DepTran_CourseOpen.CourseId from DepTran_CourseOpen 
				left join
					(select DepTran_CourseOrder.CourseId,DepTran_CourseOrder.OrderStatus,DepTran_CourseOpen.DepartId,DepTran_CourseOpen.OpenFlag from DepTran_CourseOrder
						left join  DepTran_CourseOpen on DepTran_CourseOpen.CourseId=DepTran_CourseOrder.CourseId
						where  DepTran_CourseOrder.UserId={0} and DepTran_CourseOpen.DepartId={1} ) tt
						on tt.CourseId=DepTran_CourseOpen.CourseId
						where  (tt.CourseId is null or tt.OrderStatus!=1) and DepTran_CourseOpen.DepartId={1} and DepTran_CourseOpen.OpenFlag=1	
				) and Co_Course.StartTime>getdate() and co_course.isdelete=0
		)aa 
		where {2}
	)bb
    where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", userid, DepartId, where, pageLength);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Co_Course>(sqlstr, parameters).ToList();

            }
        }



        public int GetYuDing(int courseid,int deptid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"
      select 
	(select count(0) from DepTran_CourseOrder where CourseId={0}
	and DepTran_CourseOrder.OrderStatus=1 	
	) as DeptHasEntered, 
    DepTran_CourseOpen.LimitNumber as DepTranLimitNumber,
  * from Co_Course
left join DepTran_CourseOpen on Co_Course.Id=DepTran_CourseOpen.CourseId
								  and DepTran_CourseOpen.DepartId={1}
where Co_Course.id={0}", courseid, deptid);
                var model = conn.Query<LiXinModels.CourseManage.Co_Course>(sql).FirstOrDefault();

                if (model == null)
                    return 0;//不可报名
                if (model.DeptHasEntered >= model.DepTranLimitNumber)
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
                //if (model.StopOrderFlag == 1)
                //{
                //    return 2;//关闭报名
                //}

                return 3;//可以报名
            }
        }


        public bool UpdateOrderStatus(int courseid, int userid, int OrderStatus)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"
update DepTran_CourseOrder set OrderStatus={0},OrderTimes=OrderTimes+1,OrderTime=getdate() where CourseId={1} and UserId={2} 
", OrderStatus, courseid, userid);

                return conn.Execute(sql) > 0;
            }
        }

        /// <summary>
        /// 已预定页面
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Co_Course> GetMyCourseOrderList(out int totalCount, int userId, int deptid = 0,
                                                                              string where = " 1 = 1 ",
                                                                              int startIndex = 0,
                                                                              int pageLength = int.MaxValue,
                                                                              string orderBy =
                                                                                  "ORDER BY Co_Course.id DESC")
        {

            using (IDbConnection connection = OpenConnection())
            {



                string sql = string.Format(@"
select count(1) from(
                SELECT row_number() over( ORDER BY  IsAppoint desc,StartTime desc) as rowNum, * FROM
(
	SELECT 
		DepTran_CourseOrder.ID
		,Co_Course.CourseName
		,Co_Course.NumberLimited as NumberLimited
		,Co_Course.CourseLength
        ,Co_Course.Way
        ,Co_Course.Sort
		,Co_Course.IsMust
        ,Co_Course.IsTest
		,Co_Course.StartTime
		,Co_Course.EndTime
        ,Co_Course.CourseTimes   
        ,DepTran_CourseOrder.IsAppoint    
		,Sys_User.Realname as TeacherName
		,Sys_ClassRoom.RoomName as RoomName
		,DepTran_CourseOrder.PassStatus as PassStatus
		,CASE WHEN (dbo.F_NewIsAppStatus(Co_Course.Id,DepTran_CourseOrder.UserId,DepTran_CourseOrder.OrderStatus)=1) THEN DepTran_CourseOrder.GetScore ELSE 0 END  as GetScore
		,DepTran_CourseOrder.CourseId as CourseId
        ,DepTran_CourseOrder.OrderTime
        ,DepTran_CourseOrder.OrderStatus
		,DepTran_CourseOrder.UserId
        ,DepTran_CourseOrder.[Id] as courseOrderId
        ,DepTran_CourseOrder.[OrderStatus] as MyStatus      
        ,Co_Course.YearPlan
        ,Co_Course.[IsLeave]        
        ,tt.CourseTimesOrder
		,tt.TotalCourseTimes	    
	FROM DepTran_CourseOrder left join Co_Course on DepTran_CourseOrder.CourseId=Co_Course.ID
						left join Sys_User on Co_Course.Teacher=Sys_User.UserId
                        left join DepTran_CourseOpen on DepTran_CourseOrder.CourseId=DepTran_CourseOpen.CourseId
								  and DepTran_CourseOpen.DepartId={1}
						left join Sys_ClassRoom on  Co_Course.RoomId=Sys_ClassRoom.id
                        left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			                   count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
                       from Co_Course cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
                       on tt.id = Co_Course.id and tt.code = Co_Course.code
	WHERE  " + where + @"
	and DepTran_CourseOrder.UserId={0} and DepTran_CourseOrder.OrderStatus in(1) and DepTran_CourseOpen.OpenFlag=1 and Co_Course.isdelete=0
) bb

)aa

", userId, deptid);

                totalCount = connection.Query<int>(sql).First();

                string query = string.Format(@"
select top({1}) * from(
                SELECT row_number() over( ORDER BY  IsAppoint desc,StartTime desc) as rowNum, * FROM
(
	SELECT 
		DepTran_CourseOrder.ID
		,Co_Course.CourseName
		,Co_Course.NumberLimited as NumberLimited
		,Co_Course.CourseLength
        ,Co_Course.Way
        ,Co_Course.Sort
		,Co_Course.IsMust
        ,Co_Course.IsTest
		,Co_Course.StartTime
		,Co_Course.EndTime
        ,Co_Course.CourseTimes   
        ,DepTran_CourseOrder.IsAppoint    
		,Sys_User.Realname as TeacherName
		,Sys_ClassRoom.RoomName as RoomName
		,DepTran_CourseOrder.PassStatus as PassStatus
		,CASE WHEN (dbo.F_NewIsAppStatus(Co_Course.Id,DepTran_CourseOrder.UserId,DepTran_CourseOrder.OrderStatus)=1) THEN DepTran_CourseOrder.GetScore ELSE 0 END  as GetScore
		,DepTran_CourseOrder.CourseId as CourseId
        ,DepTran_CourseOrder.OrderTime
        ,DepTran_CourseOrder.OrderStatus
		,DepTran_CourseOrder.UserId
        ,DepTran_CourseOrder.[Id] as courseOrderId
        ,DepTran_CourseOrder.[OrderStatus] as MyStatus      
        ,Co_Course.YearPlan
        ,Co_Course.[IsLeave]        
        ,tt.CourseTimesOrder
		,tt.TotalCourseTimes	  
        ,DepTran_CourseOpen.ApprovalFlag as DepTranCourseOpenApprovalFlag
        ,DepTran_CourseOrder.AttScore as AttScore
        ,DepTran_CourseOrder.EvlutionScore as EvlutionScore
        ,DepTran_CourseOrder.ExamScore as ExamScore 
	FROM DepTran_CourseOrder left join Co_Course on DepTran_CourseOrder.CourseId=Co_Course.ID
						left join Sys_User on Co_Course.Teacher=Sys_User.UserId
                        left join DepTran_CourseOpen on DepTran_CourseOrder.CourseId=DepTran_CourseOpen.CourseId
								  and DepTran_CourseOpen.DepartId={2}
						left join Sys_ClassRoom on  Co_Course.RoomId=Sys_ClassRoom.id
                        left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			                   count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
                       from Co_Course cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
                       on tt.id = Co_Course.id and tt.code = Co_Course.code
	WHERE  " + where + @"
	and DepTran_CourseOrder.UserId={0} and DepTran_CourseOrder.OrderStatus in(1) and DepTran_CourseOpen.OpenFlag=1 and Co_Course.isdelete=0
) bb

)aa where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", userId, pageLength, deptid);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<LiXinModels.CourseManage.Co_Course>(query, parameters).ToList();
            }
        }


        public Co_Course GetCourseById(int courseId, int userId = 0)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = string.Format(@" 
  select 
Co_Course.*
,Sys_User.RealName as TeacherName 
,DepTran_CourseOrder.[Id] as courseOrderId
,DepTran_CourseOrder.[UserId]
,DepTran_CourseOrder.[OrderTime]
,DepTran_CourseOrder.[OrderStatus] as MyStatus
,DepTran_CourseOrder.IsAppoint
,DepTran_CourseOrder.GetScore
,Sys_ClassRoom.RoomName
from Co_Course 
left join Sys_User on Sys_User.UserId = Co_Course.Teacher 
left join DepTran_CourseOrder on DepTran_CourseOrder.courseid = Co_Course.id and DepTran_CourseOrder.userid ={0}
left join Sys_ClassRoom on Sys_ClassRoom.id = Co_Course.RoomId
where Co_Course.id={1}", userId, courseId);

                return
                   connection.Query<LiXinModels.CourseManage.Co_Course>(sql).FirstOrDefault();
            }
        }


        /// <summary>
        /// 添加评估得分
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="EvlutionScore"></param>
        /// <returns></returns>
        public bool UpdateDepTran_CourseOrderEvlutionScore(int courseid, int userid, int EvlutionScore)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = string.Format(@"
update DepTran_CourseOrder set EvlutionScore={0} where CourseId={1} and UserId={2} 
", EvlutionScore, courseid, userid);
                return connection.Execute(sql) > 0;
            }
        }

        /// <summary>
        /// 添加考试得分
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="EvlutionScore"></param>
        /// <returns></returns>
        public bool UpdateDepTran_CourseOrderExamScore(int courseid, int userid, int ExamScore)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = string.Format(@"
update DepTran_CourseOrder set ExamScore={0} where CourseId={1} and UserId={2} 
", ExamScore, courseid, userid);

                return connection.Execute(sql) > 0;
            }
        }


        /// <summary>
        /// 添加考勤等分
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="EvlutionScore"></param>
        /// <returns></returns>
        public bool UpdateDepTran_CourseOrderAttScore(int courseid, int userid, int AttScore)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = string.Format(@"
update DepTran_CourseOrder set AttScore={0} where CourseId={1} and UserId={2} 
", AttScore, courseid, userid);

                return connection.Execute(sql) > 0;
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
                string sql = "update DepTran_CourseOrder " + where;
                return connection.Execute(sql) > 0;
            }
        }

        /// <summary>
        /// 更新考勤分数
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="attScore"></param>
        /// <returns></returns>
        public bool DepTran_CourseOrderUsers(int courseId, int userId, decimal attScore)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(
                    "update DepTran_CourseOrder set AttScore=@attScore where CourseId=@courseId and UserId=@userId");
                var param = new
                {
                    courseId = courseId,
                    userId = userId,
                    attScore = attScore
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 判断是否存在该科目该学员的考勤记录
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool InsertDeptTran_courseOrder(int courseId, int userId, int departId, decimal attScore)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO DepTran_CourseOrder(CourseId,UserId,OrderTime,OrderStatus,LearnStatus,GetScore,IsAppoint,PassStatus ,AttScore,EvlutionScore,ExamScore,DepartSetId,OrderTimes)" +
                              " VALUES " +
                              " (@courseId,@userId,1991-1-1,0,0,0,0,0,@attScore,0,0,@departId,0)");
                var param = new
                    {
                        courseId = courseId,
                        userId = userId,
                        departId = departId,
                        attScore = attScore
                    };
                return connection.Execute(strSql.ToString(), param) > 0;
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
                strSql.Append("select count(1) from DepTran_CourseOrder where courseId=@courseId and userId=@userId");
                var param = new
                    {
                        courseId = courseId,
                        userId = userId
                    };
                int count = connection.Query<int>(strSql.ToString(), param).FirstOrDefault();
                return count > 0;
            }
        }

        public List<DepTran_CourseOrder> SetUserOrder(out int totalCount, int courseId, int departId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>(
                        string.Format(@"select count(1) from DepTran_CourseOrder where CourseId={0} and DepartSetId={1} ",
                                      courseId, departId)).First();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from DepTran_CourseOrder where CourseId=@courseId and DepartSetId=@departId");
                var param = new
                    {
                        departId = departId,
                        courseId = courseId
                    };
                return connection.Query<DepTran_CourseOrder>(strSql.ToString(), param).ToList();
            }
        }

        public bool SetOrder(int courseId, int departId, int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder sqlStr = new StringBuilder();
                sqlStr.Append(
                    "update DepTran_CourseOrder set OrderStatus=0 where CourseId=@courseId and UserId=@userId and DepartSetId=@departId");
                var param = new
                {
                    departId = departId,
                    courseId = courseId,
                    userId = userId
                };
                return connection.Execute(sqlStr.ToString(), param) > 0;
            }
        }


        /// <summary>
        /// 讲师授课 学员集合
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<DepTran_CourseOrder> GetTeacherMyCourseOrderList(int userId, int courseid)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string query = "";
                if (userId != 0)
                {
                    query = string.Format(@"

	SELECT 
		DepTran_CourseOrder.ID
		,Co_Course.Teacher
		,Co_Course.CourseName		
        ,Co_Course.Way
        ,Co_Course.Sort
		,Co_Course.IsMust 
        ,Co_Course.CourseTimes   
        ,DepTran_CourseOrder.IsAppoint  
		,Sys_ClassRoom.RoomName as RoomName
		,DepTran_CourseOrder.PassStatus as PassStatus
		,CASE WHEN (dbo.F_NewIsAppStatus(Co_Course.Id,DepTran_CourseOrder.UserId,DepTran_CourseOrder.OrderStatus)=1) THEN DepTran_CourseOrder.GetScore ELSE 0 END  as GetScore
		,DepTran_CourseOrder.CourseId as CourseId
        ,DepTran_CourseOrder.OrderTime
        ,DepTran_CourseOrder.OrderStatus
		,DepTran_CourseOrder.UserId
        ,DepTran_CourseOrder.[Id] as courseOrderId
        ,DepTran_CourseOrder.[OrderStatus] as MyStatus
		,Sys_User.UserId
		,Sys_User.Realname
		,Sys_User.DeptName
		,Sys_User.JobNum
        ,Co_CoursePaper.Id as CoPaperID
		,Co_CoursePaper.PaperId
		,Co_CoursePaper.TestTimes
		,Co_CoursePaper.LevelScore
	FROM DepTran_CourseOrder left join Co_Course on DepTran_CourseOrder.CourseId=Co_Course.ID
						left join Co_CoursePaper on DepTran_CourseOrder.CourseId=Co_CoursePaper.CourseId
						left join Sys_User on DepTran_CourseOrder.UserId=Sys_User.UserId
                        left join DepTran_CourseOpen on DepTran_CourseOrder.CourseId=DepTran_CourseOpen.CourseId
								  and DepTran_CourseOpen.DepartId=(SELECT DepartSetId FROM dbo.DepTran_DepartUser where UserId={0})
                                  and DepTran_CourseOpen.OpenFlag=1 
						left join Sys_ClassRoom on  Co_Course.RoomId=Sys_ClassRoom.id
                        left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			                   count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
                       from Co_Course cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
                       on tt.id = Co_Course.id and tt.code = Co_Course.code
	WHERE  1=1
	and Co_Course.Teacher={0} and DepTran_CourseOrder.OrderStatus in(1,2)  and Co_Course.isTest=1 and Co_Course.IsDelete=0
    and DepTran_CourseOrder.courseid={1}
", userId, courseid);
                }
                else
                {
                     query = string.Format(@"

	SELECT 
		DepTran_CourseOrder.ID
		,Co_Course.Teacher
		,Co_Course.CourseName		
        ,Co_Course.Way
        ,Co_Course.Sort
		,Co_Course.IsMust 
        ,Co_Course.CourseTimes   
        ,DepTran_CourseOrder.IsAppoint  
		,Sys_ClassRoom.RoomName as RoomName
		,DepTran_CourseOrder.PassStatus as PassStatus
		,CASE WHEN (dbo.F_NewIsAppStatus(Co_Course.Id,DepTran_CourseOrder.UserId,DepTran_CourseOrder.OrderStatus)=1) THEN DepTran_CourseOrder.GetScore ELSE 0 END  as GetScore
		,DepTran_CourseOrder.CourseId as CourseId
        ,DepTran_CourseOrder.OrderTime
        ,DepTran_CourseOrder.OrderStatus
		,DepTran_CourseOrder.UserId
        ,DepTran_CourseOrder.[Id] as courseOrderId
        ,DepTran_CourseOrder.[OrderStatus] as MyStatus
		,Sys_User.UserId
		,Sys_User.Realname
		,Sys_User.DeptName
		,Sys_User.JobNum
        ,Co_CoursePaper.Id as CoPaperID
		,Co_CoursePaper.PaperId
		,Co_CoursePaper.TestTimes
		,Co_CoursePaper.LevelScore
	FROM DepTran_CourseOrder left join Co_Course on DepTran_CourseOrder.CourseId=Co_Course.ID
						left join Co_CoursePaper on DepTran_CourseOrder.CourseId=Co_CoursePaper.CourseId
						left join Sys_User on DepTran_CourseOrder.UserId=Sys_User.UserId
                        --left join DepTran_CourseOpen on DepTran_CourseOrder.CourseId=DepTran_CourseOpen.CourseId
						--		  and DepTran_CourseOpen.DepartId=(SELECT DepartSetId FROM dbo.DepTran_DepartUser where UserId={0})
                        --          and DepTran_CourseOpen.OpenFlag=1 
						left join Sys_ClassRoom on  Co_Course.RoomId=Sys_ClassRoom.id
                        left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			                   count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
                       from Co_Course cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
                       on tt.id = Co_Course.id and tt.code = Co_Course.code
	WHERE  1=1
	and 1=1 and DepTran_CourseOrder.OrderStatus in(1,2)  and Co_Course.isTest=1 and Co_Course.IsDelete=0
    and DepTran_CourseOrder.courseid={0}
",  courseid);
                }
                return connection.Query<DepTran_CourseOrder>(query).ToList();
            }
        }


        /// <summary>
        /// 查询报名人员
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="id"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Sys_User> GetCourseUserList(int id, int startIndex = 1, int startLength = int.MaxValue, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT row_number()OVER(ORDER BY Realname) number,count(*)OVER(PARTITION BY null) totalCount,su.*,ddc.DepartSetName,ddc.Id DepartSetID FROM Sys_User su
LEFT JOIN DepTran_DepartUser ddu ON ddu.UserId=su.UserId
LEFT JOIN DepTran_DepartLeaderConfig ddc ON ddc.Id=ddu.DepartSetId
WHERE su.UserId IN ( SELECT UserId FROM DepTran_CourseOrder WHERE CourseId={0})  
and {1}              
) t 
WHERE  number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", id,where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Sys_User>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获得相关考试人员的信息
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public List<DepTran_CourseOrder> GetMyCourseOrderList(int courseid)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string query = string.Format(@"

	SELECT DISTINCT
		DepTran_CourseOrder.ID
		,Co_Course.Teacher
		,Co_Course.CourseName		
        ,Co_Course.Way
        ,Co_Course.Sort
		,Co_Course.IsMust 
        ,Co_Course.CourseTimes   
        ,DepTran_CourseOrder.IsAppoint  
		,Sys_ClassRoom.RoomName as RoomName
		,DepTran_CourseOrder.PassStatus as PassStatus
		,CASE WHEN (dbo.F_NewIsAppStatus(Co_Course.Id,DepTran_CourseOrder.UserId,DepTran_CourseOrder.OrderStatus)=1) THEN DepTran_CourseOrder.GetScore ELSE 0 END  as GetScore
		,DepTran_CourseOrder.CourseId as CourseId
        ,DepTran_CourseOrder.OrderTime
        ,DepTran_CourseOrder.OrderStatus
		,DepTran_CourseOrder.UserId
        ,DepTran_CourseOrder.[Id] as courseOrderId
        ,DepTran_CourseOrder.[OrderStatus] as MyStatus
		,Sys_User.UserId
		,Sys_User.Realname
		,Sys_User.DeptName
		,Sys_User.JobNum
        ,Co_CoursePaper.Id as CoPaperID
		,Co_CoursePaper.PaperId
		,Co_CoursePaper.TestTimes
		,Co_CoursePaper.LevelScore
	FROM DepTran_CourseOrder left join Co_Course on DepTran_CourseOrder.CourseId=Co_Course.ID
						left join Co_CoursePaper on DepTran_CourseOrder.CourseId=Co_CoursePaper.CourseId
						left join Sys_User on DepTran_CourseOrder.UserId=Sys_User.UserId
                        left join DepTran_CourseOpen on DepTran_CourseOrder.CourseId=DepTran_CourseOpen.CourseId
						left join Sys_ClassRoom on  Co_Course.RoomId=Sys_ClassRoom.id
                        left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			                   count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
                       from Co_Course cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
                       on tt.id = Co_Course.id and tt.code = Co_Course.code
	WHERE  1=1
	and  DepTran_CourseOrder.OrderStatus in(1,2) and DepTran_CourseOpen.OpenFlag=1 
    and DepTran_CourseOrder.courseid={0}
", courseid);
                return connection.Query<DepTran_CourseOrder>(query).ToList();
            }
        }
        /// <summary>
        /// 讲师端考试人员都绑定出来
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public List<DepTran_CourseOrder> GetOnLineTest(int CourseId, int startIndex = 1,
                                                                 int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string sql = string.Format(@"
select  * from
(
select 
	row_number() over( ORDER BY DepTran_CourseOrder.CourseId DESC) as rowNum,count(*)OVER(PARTITION BY NULL)  totalcount ,
	DepTran_CourseOrder.CourseId,
	Sys_User.UserId,
	Sys_User.Realname as realname,
	Sys_User.DeptName as DeptName,
	Sys_User.JobNum,
	Co_CoursePaper.Id as CoPaperID,
	Co_CoursePaper.PaperId as PaperId,
	Co_CoursePaper.TestTimes,
	Co_CoursePaper.LevelScore
 from DepTran_CourseOrder 
 left join Co_Course on DepTran_CourseOrder.CourseId=Co_Course.Id
 left join Co_CoursePaper on DepTran_CourseOrder.CourseId=Co_CoursePaper.CourseId
 left join Sys_User on Sys_User.UserId=DepTran_CourseOrder.UserId
  where  DepTran_CourseOrder.CourseId={0}
    )bb
    where rowNum BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", CourseId);

                var param = new
                {
                    startIndex = startIndex,
                    startLength = pageLength
                };
                return connection.Query<DepTran_CourseOrder>(sql, param).ToList();


            }
        }

        public List<DepTran_CourseOrder> GetAllOrder(int courseId, int departId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = string.Format(@"select * from DepTran_CourseOrder where CourseId={0} and DepartSetId={1}",
                                           courseId, departId);
                return connection.Query<DepTran_CourseOrder>(sql).ToList();
            }
        }


        /// <summary>
        /// 该方法找出视频转播的开课人数
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public Co_Course GetCo_CourseLimitNumber(int courseid, int deptid)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = string.Format(@"  select Co_Course.*,DepTran_CourseOpen.LimitNumber as DepTranLimitNumber,Sys_User.Realname AS TeacherName,Sys_User.IsDelete AS TeacherIsDelete,Sys_User.IsTeacher AS TeacherIsTeacher,Sys_ClassRoom.RoomName from co_course 
	left join DepTran_CourseOpen on DepTran_CourseOpen.CourseId=co_course.id
	LEFT JOIN Sys_User ON 
Co_Course.Teacher=Sys_User.UserId LEFT Join Sys_ClassRoom ON Sys_ClassRoom.Id=Co_Course.RoomId 
	  where co_course.id={0} and DepTran_CourseOpen.DepartId={1}",
                                           courseid, deptid);
                return connection.Query<Co_Course>(sql).FirstOrDefault();
            }
        }
    }
}
