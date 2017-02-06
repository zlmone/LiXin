using LiXinModels.DepCourseManage;
using Retech.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Retech.Data;
using LiXinModels.CourseManage;
using LiXinModels.User;
using LiXinModels.DepPlanManage;

namespace LiXinDataAccess.DepCourseManage
{
    public class Dep_CourseDB : BaseRepository
    {





        /// <summary>
        /// 查处纳入考核范围的所有课程
        /// </summary>
        /// <returns></returns>
        public List<Dep_Course> GetDep_CourseList(int year)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string query = string.Format(@"select *,
                    	(select isopen from Dep_YearPlan where IsDelete=0 and PublishFlag=1 and [year]={0} and deptid=dep_course.deptid) as Year_isopen,
                        (select count(1) from Dep_CourseResource where courseid=dep_course.id and Dep_CourseResource.IsDelete=0 and ResourceType=0) as ResourceCount                      
                from dep_course where deptid in(select deptid from View_CheckUser) and isdelete=0 and yearplan={0} --and CourseFrom=2", year);

                return connection.Query<Dep_Course>(query).ToList();
            }
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Dep_YearPlan> GetDepYearListByWhere(string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"SELECT IsOpen, DeptId,LastUpdateTime from Dep_YearPlan where IsDelete=0 and PublishFlag=1 
and  DeptId in(select DeptId from View_CheckUser) and {0}
UNION
SELECT 3, dld.DeptId,LastUpdateTime FROM Dep_LinkDepart  dld
LEFT JOIN Dep_YearPlan dyp ON dld.YearId=dyp.Id
WHERE dld.ApprovalFlag=1  and  dld.DeptId in(select DeptId from View_CheckUser)  and {0}", where);
                return conn.Query<Dep_YearPlan>(sql).ToList();
            }
        }


        public List<Dep_Course> GetYearCourse(int year)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string query = string.Format(@" select * from dep_course where id in(
            select courseid from dbo.Dep_YearPlanCourse where yearid in(
            select id from Dep_YearPlan where  publishflag=1)) and year={0}", year);

                return connection.Query<Dep_Course>(query).ToList();
            }



        }

        public Dep_Course GetCo_Course(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = @"select Dep_Course.* 
,Sys_User.Realname AS TeacherName,Sys_User.IsDelete AS TeacherIsDelete,Sys_User.IsTeacher AS TeacherIsTeacher,Dep_ClassRoom.RoomName
,Sys_Department.DeptName 

 from Dep_Course 
LEFT JOIN Sys_User ON Dep_Course.Teacher=Sys_User.UserId 
LEFT Join Dep_ClassRoom ON Dep_ClassRoom.Id=Dep_Course.RoomId 
LEFT Join Sys_Department ON Sys_Department.DepartmentId=Dep_Course.DeptId 
where Dep_Course.Id=@Id ";
                var param = new
                {
                    Id
                };
                return connection.Query<Dep_Course>(sqlwhere, param).FirstOrDefault();

            }
        }

        public Dep_Course GetCo_Course(int Id, int userid)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = @"select Dep_Course.* ,(select PassStatus from Dep_CourseOrder where Dep_CourseOrder.CourseId=@Id and UserId=@userid) as PassStatus,Sys_User.Realname AS TeacherName,Sys_User.IsDelete AS TeacherIsDelete,Sys_User.IsTeacher AS TeacherIsTeacher,Dep_ClassRoom.RoomName
 from Dep_Course LEFT JOIN Sys_User ON 
Dep_Course.Teacher=Sys_User.UserId LEFT Join Dep_ClassRoom ON Dep_ClassRoom.Id=Dep_Course.RoomId  where Dep_Course.Id=@Id";
                var param = new
                {
                    Id,
                    userid
                };
                return connection.Query<Dep_Course>(sqlwhere, param).FirstOrDefault();

            }
        }


        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="model"></param>
        public void AddCourse(Dep_Course model)
        {
            using (IDbConnection connection = OpenConnection())
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into [Dep_Course] (");
                strSql.Append(@"CourseName,Code,Year,Month,Day,OpenLevel,IsMust,Way,Teacher,StartTime,EndTime,Sort,CourseLength,RoomId,NumberLimited,IsCPA,AdFlag,IsOrder,IsLeave,IsRT,OpenFlag,OpenGroupObject,OpenDepartObject,IsTest,IsPing,SurveyPaperId,Memo,CourseFrom,StopOrderFlag,StopDucueFlag,ReturnTimes,AfterOrderTimes,AttFlag,PreAdviceConfigTime,AfterEvlutionConfigTime,PreCourseTime,Publishflag,CourseTimes,LastUpdateTime,IsDelete,TrainDays,CourseAddress,CourseObjectMemo,OpenPerson,CpaTeacher,YearPlan,IsSystem,IsYearPlan,CourseLengthDistribute,IsOpenSub,courseProvide,StudentRequirement,remark,IsOpenOthers,DeptId,OpenUserId,AttUserId)");
                strSql.Append(" values (");
                strSql.Append(@"@CourseName,@Code,@Year,@Month,@Day,@OpenLevel,@IsMust,@Way,@Teacher,@StartTime,@EndTime,@Sort,@CourseLength,@RoomId,@NumberLimited,@IsCPA,@AdFlag,@IsOrder,@IsLeave,@IsRT,@OpenFlag,@OpenGroupObject,@OpenDepartObject,@IsTest,@IsPing,@SurveyPaperId,@Memo,@CourseFrom,@StopOrderFlag,@StopDucueFlag,@ReturnTimes,@AfterOrderTimes,@AttFlag,@PreAdviceConfigTime,@AfterEvlutionConfigTime,@PreCourseTime,@Publishflag,@CourseTimes,@LastUpdateTime,@IsDelete,@TrainDays,@CourseAddress,@CourseObjectMemo,@OpenPerson,@CpaTeacher,@YearPlan,@IsSystem,@IsYearPlan,@CourseLengthDistribute,@IsOpenSub,@courseProvide,@StudentRequirement,@remark,@IsOpenOthers,@DeptId,@OpenUserId,@AttUserId)");
                strSql.Append(";select @@IDENTITY");
                var param = new
                {
                    model.CourseName,
                    model.Code,
                    model.Year,
                    model.Month,
                    model.Day,
                    model.OpenLevel,
                    model.IsMust,
                    model.Way,
                    model.Teacher,
                    model.StartTime,
                    model.EndTime,
                    model.Sort,
                    model.CourseLength,
                    model.RoomId,
                    model.NumberLimited,
                    model.IsCPA,
                    model.AdFlag,
                    model.IsOrder,
                    model.IsLeave,
                    model.OpenFlag,
                    model.IsRT,
                    model.OpenGroupObject,
                    model.OpenDepartObject,
                    model.IsTest,
                    model.IsPing,
                    model.SurveyPaperId,
                    model.Memo,
                    model.CourseFrom,
                    model.StopOrderFlag,
                    model.StopDucueFlag,
                    model.ReturnTimes,
                    model.AfterOrderTimes,
                    model.PreAdviceConfigTime,
                    model.AfterEvlutionConfigTime,
                    model.PreCourseTime,
                    model.Publishflag,
                    model.CourseTimes,
                    model.LastUpdateTime,
                    model.IsDelete,
                    model.TrainDays,
                    model.CourseAddress,
                    model.CourseObjectMemo,
                    model.OpenPerson,
                    model.CpaTeacher,
                    model.YearPlan,
                    model.AttFlag,
                    model.IsSystem,
                    model.IsYearPlan,
                    model.CourseLengthDistribute,
                    model.IsOpenSub,
                    model.courseProvide,
                    model.StudentRequirement,
                    model.remark,
                    model.IsOpenOthers,
                    model.DeptId,
                    model.OpenUserId,
                    model.AttUserId
                };

                decimal id =
                    connection.Query<decimal>(strSql.ToString(), param)
                              .FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 根据课程编号更新  同一课次中的课程的课次数 2013-3-21 9:33:42
        /// </summary>
        /// <param name="code"></param>
        /// <param name="courseTimes"></param>
        /// <returns></returns>
        public bool UpdateCourseTimesByCode(string code, int courseTimes)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"update [Dep_Course] set CourseTimes=@courseTimes
             where Code=@code and coursefrom=2 and way=1 ");
                var param = new
                {
                    code,
                    courseTimes
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }


        public bool UpdateCourse(Dep_Course model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"update [Dep_Course] set CourseName=@CourseName,Code=@Code,Year=@Year,Month=@Month,Day=@Day,
            OpenLevel=@OpenLevel,
            IsMust=@IsMust,
            Way=@Way,
            Teacher=@Teacher,
            StartTime=@StartTime,
            EndTime=@EndTime,
            Sort=@Sort,
            CourseLength=@CourseLength,
            RoomId=@RoomId,
            NumberLimited=@NumberLimited,
            IsCPA=@IsCPA,
            AdFlag=@AdFlag,
            IsOrder=@IsOrder,
            IsLeave=@IsLeave,
            IsRT=@IsRT,
            OpenFlag=@OpenFlag,
            OpenGroupObject=@OpenGroupObject,
            OpenDepartObject=@OpenDepartObject,
            IsTest=@IsTest,
            IsPing=@IsPing,
            SurveyPaperId=@SurveyPaperId,
            Memo=@Memo,
            CourseFrom=@CourseFrom,
            StopOrderFlag=@StopOrderFlag,
            StopDucueFlag=@StopDucueFlag,
            ReturnTimes=@ReturnTimes,
            AfterOrderTimes=@AfterOrderTimes,
           PreAdviceConfigTime=@PreAdviceConfigTime,
            AfterEvlutionConfigTime=@AfterEvlutionConfigTime,
            PreCourseTime=@PreCourseTime,
            Publishflag=@Publishflag,
            LastUpdateTime=@LastUpdateTime,
            IsDelete=@IsDelete,
            YearPlan =@YearPlan,
            IsSystem=@IsSystem,
            IsYearPlan=@IsYearPlan,
            CourseLengthDistribute=@CourseLengthDistribute,
            IsOpenSub=@IsOpenSub,
 TrainDays=@TrainDays,CourseAddress=@CourseAddress,CourseObjectMemo=@CourseObjectMemo,OpenPerson=@OpenPerson,CpaTeacher=@CpaTeacher,CourseTimes=@CourseTimes,AttFlag=@AttFlag 
,courseProvide = @courseprovide,StudentRequirement = @StudentRequirement,remark = @remark,DeptId=@DeptId,IsOpenOthers=@IsOpenOthers
             where Id=@Id ");
                var param = new
                {
                    model.Id,
                    model.CourseName,
                    model.Code,
                    model.Year,
                    model.Month,
                    model.Day,
                    model.OpenLevel,
                    model.IsMust,
                    model.Way,
                    model.Teacher,
                    model.StartTime,
                    model.EndTime,
                    model.Sort,
                    model.CourseLength,
                    model.RoomId,
                    model.NumberLimited,
                    model.IsCPA,
                    model.AdFlag,
                    model.IsOrder,
                    model.IsLeave,
                    model.IsRT,
                    model.OpenFlag,
                    model.OpenGroupObject,
                    model.OpenDepartObject,
                    model.IsTest,
                    model.IsPing,
                    model.SurveyPaperId,
                    model.Memo,
                    model.CourseFrom,
                    model.StopOrderFlag,
                    model.StopDucueFlag,
                    model.ReturnTimes,
                    model.AfterOrderTimes,
                    model.PreAdviceConfigTime,
                    model.AfterEvlutionConfigTime,
                    model.PreCourseTime,
                    model.Publishflag,
                    model.LastUpdateTime,
                    model.IsDelete,
                    model.YearPlan,
                    model.TrainDays,
                    model.CourseAddress,
                    model.CourseObjectMemo,
                    model.OpenPerson,
                    model.CpaTeacher,
                    model.CourseTimes,
                    model.AttFlag,
                    model.IsSystem,
                    model.IsYearPlan,
                    model.CourseLengthDistribute,
                    model.IsOpenSub,
                    model.courseProvide,
                    model.StudentRequirement,
                    model.remark,
                    model.DeptId,
                    model.IsOpenOthers
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }

        }



        /// <summary>
        /// 获取课程列表 集中授课使用  包含课次数据
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Dep_Course> GetCourseTogetherList(out int totalCount, int way = 1, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Dep_Course.Code,Dep_Course.Id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Dep_Course where " + where + @" AND Dep_Course.IsDelete=0 ")
                              .First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,Dep_Course.*,Sys_User.Realname AS TeacherName,Sys_User.IsDelete AS TeacherIsDelete,Sys_User.IsTeacher AS TeacherIsTeacher, Dep_ClassRoom.RoomName,CourseTimesOrder,TotalCourseTimes
--,
--row_number() OVER(PARTITION BY Co_Course.Code ORDER BY StartTime,Co_Course.Id asc) AS CourseTimesOrder,
--count(Code)over( PARTITION BY Co_Course.Code) AS TotalCourseTimes
 from Dep_Course LEFT JOIN Sys_User ON 
Dep_Course.Teacher=Sys_User.UserId LEFT Join Dep_ClassRoom ON Dep_ClassRoom.Id=Dep_Course.RoomId 
left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime,cc.Id asc) as CourseTimesOrder,
			count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
    from Dep_Course cc where cc.isdelete = 0 and cc.coursefrom = 2  ) tt 
    on tt.id = Dep_Course.id and tt.code = Dep_Course.code 
    where " + where + @" AND Dep_Course.IsDelete=0  --AND way={2}
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy, way);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Dep_Course>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 修改课程单一状态
        /// </summary>
        /// <param name="flag">0:删除 1:发布</param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public bool ModifySingleCourse(int flag, int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                if (flag == 0)
                {
                    strSql.AppendFormat(@"update [Dep_Course] set
            LastUpdateTime=@LastUpdateTime,OpenSubmitTime=@OpenSubmitTime,   {0}
             where Id=@Id ", "IsDelete=1");
                }
                if (flag == 1)
                {
                    strSql.AppendFormat(@"update [Dep_Course] set
            LastUpdateTime=@LastUpdateTime,OpenSubmitTime=@OpenSubmitTime,  {0}
             where Id=@Id ", " Publishflag=1 ");
                }

                var param = new
                {
                    Id = courseId,
                    LastUpdateTime = DateTime.Now,
                    OpenSubmitTime = DateTime.Now
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }

        }

        /// <summary>
        /// 指定课程负责人
        /// </summary>
        /// <param name="id">课程ID</param>
        /// <param name="uid">负责人ID</param>
        /// <returns></returns>
        public bool ModifyCourseMaster(int id, string uid)
        {
            using (var connection = OpenConnection())
            {
                var sql = string.Format("update Dep_Course set AttUserId='{0}' where Id={1}", uid, id);
                return connection.Execute(sql) > 0;
            }
        }


        /// <summary>
        /// 验证课程编号是否重名
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="courseFrom">课程来源 0：年度计划；1：月度计划；2：课程管理</param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExistCourseCode(string courseCode, int courseFrom = 2, int Id = 0, int way = 1)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = "select count(1) from Dep_Course where IsDelete=0 AND  Code=@courseCode AND courseFrom=@courseFrom"; //way=@way AND 
                if (Id > 0)
                    sqlwhere += " and Id <> " + Id;
                var param = new
                {
                    way = way,
                    courseCode = courseCode,
                    courseFrom = courseFrom
                };
                int count = connection.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }



        /// <summary>
        /// 获取课程列表 普通获取List方式
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Dep_Course> GetCourseCommonList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Dep_Course.Code,Dep_Course.Id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Dep_Course where " + where + @" AND Dep_Course.IsDelete=0 ")
                              .First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,Dep_Course.*,Sys_User.Realname AS TeacherName,Dep_ClassRoom.RoomName
,Sys_User.MobileNum as TeacherMobileNum,Sys_User.email as TeacherEmail
 from Dep_Course LEFT JOIN Sys_User ON 
Dep_Course.Teacher=Sys_User.UserId LEFT Join Dep_ClassRoom ON Dep_ClassRoom.Id=Dep_Course.RoomId 
    where " + where + @" AND Dep_Course.IsDelete=0 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Dep_Course>(query, parameters).ToList();
            }
        }

        public Dep_Course GetVideoCo_CourseById(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = @"select Dep_Course.* ,Cl_CpaLearnStatus.[IsAttFlag] as [Status],Sys_User.Realname AS TeacherName,Co_Coursepaper.TestTimes
 from Dep_Course LEFT JOIN Cl_CpaLearnStatus ON 
Dep_Course.id=Cl_CpaLearnStatus.CourseID LEFT JOIN Sys_User ON 
Sys_User.UserId=Dep_Course.Teacher LEFT JOIN Co_Coursepaper ON Co_Coursepaper.CourseId=Dep_Course.Id where Dep_Course.Id=@Id";
                var param = new
                {
                    Id
                };
                return connection.Query<Dep_Course>(sqlwhere, param).FirstOrDefault();

            }
        }
        #region == 部门开放至全所审批 ==
        /// <summary>
        /// （教育培训部）获得部门开放至全所审批列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="where">条件语句</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按审批状态排序</param>
        /// <returns></returns>
        public List<Dep_Course> DepCourseOpenOthersCheck(out int totalCount, string where = "",
                                                   int startIndex = 1, int pageLength = int.MaxValue,
                                                    string orderBy = " ORDER BY IsOpenOthers asc ")
        {

            using (IDbConnection connection = OpenConnection())
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = " ORDER BY IsOpenOthers asc ";
                }
                string totalCountSql =
                string.Format(@"
                SELECT count(1)  FROM
(
	SELECT row_number() over( {1}) as rowNum,* 
FROM
(
	SELECT cc.Id,cc.CourseName
		,cc.NumberLimited as NumberLimited
		,cc.CourseLength
        ,cc.Way
        ,cc.Sort
		,cc.IsMust
        ,cc.IsTest
		,cc.StartTime
		,cc.EndTime
        ,cc.CourseTimes  
		,u.Realname as TeacherName
		,scr.RoomName as RoomName
        ,cc.YearPlan
        ,cc.IsYearPlan
        ,cc.[IsLeave]
        ,tt.CourseTimesOrder
		,tt.TotalCourseTimes
        ,cc.OpenUserId
        ,u1.Realname as OpenUserName	 --提交人
        ,cc.DeptId 
        ,dept.DeptName    --提交部门
        ,cc.IsOpenOthers
        ,cc.ApprovalUserId 
        ,u2.Realname as ApprovalUserName --审批人
        ,cc.ApprovalTime 
	FROM  Dep_Course cc
	left join Sys_User u on cc.Teacher=u.UserId --关联讲师
	left join Dep_ClassRoom scr on  cc.RoomId=scr.id --关联教室
    left join (select id,code, row_number() OVER(PARTITION BY cc1.Code ORDER BY cc1.StartTime asc) as CourseTimesOrder,
			                   count(cc1.Code)over( PARTITION BY cc1.Code) as TotalCourseTimes
                       from Dep_Course cc1 where cc1.isdelete = 0 and cc1.coursefrom = 2 and cc1.publishflag = 1) tt 
                       on tt.id = cc.id and tt.code = cc.code 
    left join Sys_User u1 on cc.OpenUserId=u1.UserId --关联提交人
    left join Sys_Department dept on cc.DeptId=dept.DepartmentId --关联提交部门 
    left join Sys_User u2 on cc.ApprovalUserId=u2.UserId --关联审批人 
	WHERE  cc.IsDelete=0 
           and cc.Publishflag=1 
           --and cc.Way=1 
           and cc.CourseFrom=2 
           and cc.IsOpenOthers<>0  
           {0}
	       
) bb_yxt 
 ) aa_yxt
", where, orderBy);
                totalCount = connection.Query<int>(totalCountSql).First();


                string query = string.Format(@"
select top({0}) * from(
SELECT row_number() over({2}) as rowNum,* 
FROM
(
	SELECT cc.Id,cc.CourseName
		,cc.NumberLimited as NumberLimited
		,cc.CourseLength
        ,cc.Way
        ,cc.Sort
		,cc.IsMust
        ,cc.IsTest
		,cc.StartTime
		,cc.EndTime
        ,cc.CourseTimes  
		,u.Realname as TeacherName
		,scr.RoomName as RoomName
        ,cc.IsYearPlan
        ,cc.YearPlan
        ,cc.[IsLeave]
        ,tt.CourseTimesOrder
		,tt.TotalCourseTimes
        ,cc.OpenUserId
        ,u1.Realname as OpenUserName	 --提交人
        ,cc.DeptId 
        ,dept.DeptName    --提交部门
        ,cc.IsOpenOthers
        ,cc.ApprovalUserId 
        ,u2.Realname as ApprovalUserName --审批人
        ,cc.ApprovalTime 
	FROM  Dep_Course cc
	left join Sys_User u on cc.Teacher=u.UserId --关联讲师
	left join Dep_ClassRoom scr on  cc.RoomId=scr.id --关联教室
    left join (select id,code, row_number() OVER(PARTITION BY cc1.Code ORDER BY cc1.StartTime asc) as CourseTimesOrder,
			                   count(cc1.Code)over( PARTITION BY cc1.Code) as TotalCourseTimes
                       from Dep_Course cc1 where cc1.isdelete = 0 and cc1.coursefrom = 2 and cc1.publishflag = 1) tt 
                       on tt.id = cc.id and tt.code = cc.code 
    left join Sys_User u1 on cc.OpenUserId=u1.UserId --关联提交人
    left join Sys_Department dept on cc.DeptId=dept.DepartmentId --关联提交部门 
    left join Sys_User u2 on cc.ApprovalUserId=u2.UserId --关联审批人 
	WHERE  cc.IsDelete=0 
           and cc.Publishflag=1 
           --and cc.Way=1 
           and cc.CourseFrom=2 
           and cc.IsOpenOthers<>0  
           {1}
	       
) bb_yxt

)aa_yxt where rowNum between   @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", pageLength, where, orderBy);

                var parameters = new
                {
                    startIndex = startIndex,
                    startLength = pageLength
                };
                return connection.Query<Dep_Course>(query, parameters).ToList();
            }

        }

        /// <summary>
        /// 根据ID更新审批信息
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateApproval(Dep_Course model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @" UPDATE dbo.Dep_Course
SET IsOpenOthers=@IsOpenOthers,  
    ApprovalUserId = @ApprovalUserId,
	ApprovalTime = @ApprovalTime,
	OpenCourseId = @OpenCourseId,
     OpenReason=@OpenReason
WHERE Id = @Id ";
                var param = new
                {
                    model.Id,
                    model.IsOpenOthers,
                    model.ApprovalUserId,
                    model.ApprovalTime,
                    model.OpenCourseId,
                    model.OpenReason
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }
        #endregion


        #region == 部门/分所自学 ==
        /// <summary>
        /// （教育培训部）获得部门自学列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="where">条件语句</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按Id倒序排序</param>
        /// <returns></returns>
        public List<Dep_Course> DepCourseLearnSelfList(out int totalCount, string where = "",
                                                   int startIndex = 1, int pageLength = int.MaxValue,
                                                    string orderBy = " ORDER BY Id desc ")
        {

            using (IDbConnection connection = OpenConnection())
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = " ORDER BY Id desc ";
                }
                string totalCountSql =
                string.Format(@"
                SELECT count(1)  FROM
(
	SELECT row_number() over( {1}) as rowNum,* 
FROM
(
	SELECT cc.Id,cc.CourseName
		,cc.NumberLimited as NumberLimited
		,cc.CourseLength 
        ,cc.Way
        ,cc.Sort
		,cc.IsMust
        ,cc.IsTest
		,cc.StartTime
		,cc.EndTime
        ,cc.CourseTimes  
		,u.Realname as TeacherName
		,scr.RoomName as RoomName
        ,cc.YearPlan
        ,cc.IsYearPlan
        ,cc.[IsLeave]
        ,tt.CourseTimesOrder
		,tt.TotalCourseTimes
        ,cc.OpenUserId
        ,u1.Realname as OpenUserName	 --提交人
        ,cc.DeptId 
        ,dept.DeptName    --提交部门
        ,cc.IsOpenOthers 
        ,cc.DeptId    LinkDeptId
	FROM  Dep_Course cc
	left join Sys_User u on cc.Teacher=u.UserId --关联讲师
	left join Dep_ClassRoom scr on  cc.RoomId=scr.id --关联教室
    left join (select id,code, row_number() OVER(PARTITION BY cc1.Code ORDER BY cc1.StartTime asc) as CourseTimesOrder,
			                   count(cc1.Code)over( PARTITION BY cc1.Code) as TotalCourseTimes
                       from Dep_Course cc1 where cc1.isdelete = 0 and cc1.coursefrom = 2 and cc1.publishflag = 1) tt 
                       on tt.id = cc.id and tt.code = cc.code 
    left join Sys_User u1 on cc.OpenUserId=u1.UserId --关联提交人
    left join Sys_Department dept on cc.DeptId=dept.DepartmentId --关联提交部门 
	WHERE  cc.IsDelete=0 --未删除
           and cc.Publishflag=1 --已发布
           --and cc.Way=1 --集中课程
           and cc.CourseFrom=2 --来源：课程管理
           and cc.IsOpenOthers=0  --部门自学 
           {0}
	       
) bb_yxt 
 ) aa_yxt
", where, orderBy);
                totalCount = connection.Query<int>(totalCountSql).First();


                string query = string.Format(@"
select top({0}) * from(
SELECT row_number() over({2}) as rowNum,* 
FROM
(
	SELECT cc.Id,cc.CourseName
		,cc.NumberLimited as NumberLimited
		,cc.CourseLength 
        ,cc.Way
        ,cc.Sort
		,cc.IsMust
        ,cc.IsTest
        ,cc.IsPing
		,cc.StartTime
		,cc.EndTime
        ,cc.CourseTimes  
		,u.Realname as TeacherName
		,scr.RoomName as RoomName
        ,cc.YearPlan
        ,cc.IsYearPlan
        ,cc.[IsLeave]
        ,tt.CourseTimesOrder
		,tt.TotalCourseTimes
        ,cc.OpenUserId
        ,u1.Realname as OpenUserName	 --提交人
        ,cc.DeptId 
        ,dept.DeptName    --提交部门
        ,cc.IsOpenOthers
        ,(select count(*) from Dep_CourseOrder dco 
            where dco.courseid = cc.id 
                   and dco.IsAppoint=0 --预订 
				   And (   
						  (dco.orderstatus = 1 and dco.isleave = 0 )
						   or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
						) 
          ) as YuDingUserCount --预订人数
        ,(select count(*) from Dep_CourseOrder dco 
            where dco.courseid = cc.id 
                   and dco.IsAppoint=1 --指定
				   And (   
						  (dco.orderstatus = 1 and dco.isleave = 0 )
						   or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
						) 
          ) as ZhiDingUserCount --指定人数
          ,(
			   select count(*) 
			   from (
					   select distinct UserId from dbo.Dep_Attendce da
					   where da.CourseId=cc.Id and da.Status<>0 and da.Status<>2 
					  ) as uu
             ) as ChuQinUserCount --出勤人数
           ,( 
              case when cc.IsPing=1 and  cc.SurveyPaperId is not null and ltrim(rtrim(cc.SurveyPaperId))<>'' 
				 then (
						 SELECT COUNT(*) FROM
							(
								SELECT DISTINCT dsra.UserID
								FROM Dep_Survey_ReplyAnswer dsra 
								WHERE dsra.SourceFrom=0 --课后评估 
									  AND dsra.ObjectID=cc.Id 
									  AND dsra.ExampaperID in (
																select [Value]=( select top 1 split2.[Value] 
																				 from [dbo].[SplitStringBySeparator](split1.[Value], ',', 1) as split2 
																				 order by split2.[Value] desc )
																from [dbo].[SplitStringBySeparator](cc.SurveyPaperId, ';', 1) as split1
															   ) 
									  AND dsra.Status=1 --已提交 
									  AND dsra.QuestionID IN (
                                                               SELECT q.QuestionID 
															   FROM Dep_Survey_Question q 
															   WHERE q.ExampaperID in (
																						  select [Value]=( select top 1 split2.[Value] 
																										   from [dbo].[SplitStringBySeparator](split1.[Value], ',', 1) as split2 
																										   order by split2.[Value] desc )
																						  from [dbo].[SplitStringBySeparator](cc.SurveyPaperId, ';', 1) as split1
																					   ) 
																	  AND q.Status=0
															  ) 
							) AS dsra_user 
					   )
				 else 0
			   end
             ) as HasPingUserCount --课后评估人数(已评人数) 
             ,(
               case when cc.IsTest=1 
                    then dcp.PaperId 
                    else 0 
                end 
               ) as OnlineExampaperId --在线测试试卷ID 
             ,isnull((
                select top 1 (case when ApprovalFlag=1  and AttFlag=1   
                            then 1 
                            when ApprovalFlag=1   and (AttFlag=0 OR  AttFlag IS NULL)   
                            then 2 
                            when (AttFlag=0 OR  AttFlag IS NULL) and (ApprovalFlag=0 OR  ApprovalFlag IS NULL)
                            then 3
                            else 0
                       end  ) as DeptApprovalFlag0
                from  Dep_CourseDept where CourseId=cc.Id and DepartId=cc.DeptId 
               ),3) as DeptApprovalFlag --部门考勤状态 
,cc.DeptId    LinkDeptId
	FROM  Dep_Course cc
	left join Sys_User u on cc.Teacher=u.UserId --关联讲师
	left join Dep_ClassRoom scr on  cc.RoomId=scr.id --关联教室
    left join (select id,code, row_number() OVER(PARTITION BY cc1.Code ORDER BY cc1.StartTime asc) as CourseTimesOrder,
			                   count(cc1.Code)over( PARTITION BY cc1.Code) as TotalCourseTimes
                       from Dep_Course cc1 where cc1.isdelete = 0 and cc1.coursefrom = 2 and cc1.publishflag = 1) tt 
                       on tt.id = cc.id and tt.code = cc.code 
    left join Sys_User u1 on cc.OpenUserId=u1.UserId --关联提交人
    left join Sys_Department dept on cc.DeptId=dept.DepartmentId --关联提交部门 
    left join Dep_Coursepaper dcp on dcp.CourseId=cc.Id 
	WHERE  cc.IsDelete=0 --未删除
           and cc.Publishflag=1 --已发布
           --and cc.Way=1 --集中课程
           and cc.CourseFrom=2 --来源：课程管理
           and cc.IsOpenOthers=0  --部门自学  
           {1}
	       
) bb_yxt

)aa_yxt where rowNum between   @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", pageLength, where, orderBy);

                var parameters = new
                {
                    startIndex = startIndex,
                    startLength = pageLength
                };
                return connection.Query<Dep_Course>(query, parameters).ToList();
            }


        }

        /// <summary>
        /// （教育培训部）获得部门自学列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="where">条件语句</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按Id倒序排序</param>
        /// <returns></returns>
        public List<Dep_Course> DepCourseLearnSelfAndLinkList(string where = "",
                                                   int startIndex = 1, int pageLength = int.MaxValue,
                                                    string orderBy = " ORDER BY Id desc ")
        {
            using (var conn = OpenConnection())
            {
                var sql = @"";
                return conn.Query<Dep_Course>(sql).ToList();
            }
        }
        #region == 预订指定人数统计暂时废弃的代码 ==
        //,(
        //    case when (cc.IsOrder=1 or cc.IsOrder=3) and (cc.OpenFlag=2 or cc.OpenFlag=3) 
        //         then (
        //                select count(*) 
        //                from (
        //                       select distinct UserId from Sys_User u 
        //                       where  u.IsDelete=0 and (u.Status=0 or u.Status=1) 
        //                              and u.DeptId in (select ID from dbo.F_SplitIDs(cc.OpenDepartObject))
        //                      ) as uu
        //               )
        //         else 0
        //    end
        //  ) as YuDingUserCount --预订人数
        // ,(
        //    case when (cc.IsOrder=2 or cc.IsOrder=3) 
        //         then (
        //                select count(*) 
        //                from (
        //                       select distinct  ID from dbo.F_SplitIDs(cc.OpenPerson)
        //                      ) as uu
        //               )
        //         else 0
        //    end
        //  ) as ZhiDingUserCount --指定人数
        #endregion
        #endregion

        #region == 部门开课跟踪 ==
        /// <summary>
        /// （教育培训部）获得部门开课跟踪列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="where">条件语句"  and ... "</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按Id倒序排序</param>
        /// <returns></returns>
        public List<Dep_Course> DepOpenCourseFollowingList(out int totalCount, string where = "",
                                                   int startIndex = 1, int pageLength = int.MaxValue,
                                                    string orderBy = " ORDER BY YearPlan asc ")
        {

            using (IDbConnection connection = OpenConnection())
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = " ORDER BY YearPlan asc  ";
                }

                string withTablesSql = string.Format(@" 
   --使用CTE公用表表达式 Dep_Course_A,Dep_Course_B
   with Dep_YearPlanCourse_A as (
           select CourseId from Dep_YearPlanCourse 
           where YearId in (select Id from Dep_YearPlan where IsDelete=0 and PublishFlag=1)
        ),--查找正常的年度计划课程ID
        Dep_Course_A as (   
			select cc.Id
			   ,cc.YearPlan
			   ,cc.DeptId 
			   ,dept.DeptName    --提交部门 
			   ,cc.CourseFrom 
               ,cc.IsYearPlan
			   ,isnull((select dcd.ApprovalFlag
				 from Dep_CourseDept dcd 
				 where dcd.CourseId=cc.Id and dcd.DepartId=cc.DeptId
				 ),0) as ApprovalFlag --是否考勤0-否，非0-是 
			   ,cc.CourseLength 
			FROM  Dep_Course cc
			left join Sys_Department dept on cc.DeptId=dept.DepartmentId --关联提交部门 
			WHERE  cc.IsDelete=0 --未删除
				   and (
						 (cc.CourseFrom=2 and cc.Publishflag=1 ) 
						 or ( cc.CourseFrom=0 and cc.Id in (select * from Dep_YearPlanCourse_A) ) 
						)
		 ),
		 Dep_Course_B as (
			 select distinct cc.YearPlan
							,cc.DeptId 
							,dept.DeptName    --提交部门 
                           
			 from Dep_Course cc
			 left join Sys_Department dept on cc.DeptId=dept.DepartmentId --关联提交部门 
			 WHERE  cc.IsDelete=0 --未删除
				   and (
						 (cc.CourseFrom=2 and cc.Publishflag=1 ) 
						 or ( cc.CourseFrom=0 and cc.Id in (select * from Dep_YearPlanCourse_A) ) 
						)
		 )  ");
                string totalCountSql =
                string.Format(@" {2} 
                SELECT count(1)  FROM
(
	SELECT row_number() over( {1}) as rowNum,* 
FROM
(
	select Dep_Course_B.YearPlan
		   ,Dep_Course_B.DeptId 
		   ,Dep_Course_B.DeptName    --提交部门 
           ,(select count(*) from Dep_Course_A where CourseFrom=0 and YearPlan=Dep_Course_B.YearPlan and DeptId=Dep_Course_B.DeptId
             ) as PlanOpenCourseCount --计划开课数
           ,(select count(*) from Dep_Course_A where CourseFrom=2 and YearPlan=Dep_Course_B.YearPlan and DeptId=Dep_Course_B.DeptId
             ) as NowOpenCourseCount --实际开课数
           ,(select count(*) from Dep_Course_A where CourseFrom=2 and YearPlan=Dep_Course_B.YearPlan and DeptId=Dep_Course_B.DeptId and ApprovalFlag<>0 
             ) as HasAttendceCourseCount --已考勤课程数
           ,isnull((select sum(CourseLength) from Dep_Course_A where CourseFrom=0 and YearPlan=Dep_Course_B.YearPlan and DeptId=Dep_Course_B.DeptId  
             ),0) as PlanCourseLengthSum --计划课程总学时
           ,isnull((select sum(CourseLength) from Dep_Course_A where CourseFrom=2 and YearPlan=Dep_Course_B.YearPlan and DeptId=Dep_Course_B.DeptId  
             ),0) as NowCourseLengthSum --实际课程总学时
    from Dep_Course_B  
    where 1=1  
          {0}
) bb_yxt 
 ) aa_yxt
", where, orderBy, withTablesSql);
                totalCount = connection.Query<int>(totalCountSql).First();


                string query = string.Format(@" {3} 
select top({0}) * from(
SELECT row_number() over({2}) as rowNum,* 
FROM
(
	select Dep_Course_B.YearPlan
		   ,Dep_Course_B.DeptId 
		   ,Dep_Course_B.DeptName    --提交部门 
           ,(select count(*) from Dep_Course_A where CourseFrom=0 and YearPlan=Dep_Course_B.YearPlan and DeptId=Dep_Course_B.DeptId
             ) as PlanOpenCourseCount --计划开课数
           ,(select count(*) from Dep_Course_A where CourseFrom=2 and YearPlan=Dep_Course_B.YearPlan and DeptId=Dep_Course_B.DeptId
             ) as NowOpenCourseCount --实际开课数
           ,(select count(*) from Dep_Course_A where CourseFrom=2 and YearPlan=Dep_Course_B.YearPlan and DeptId=Dep_Course_B.DeptId and ApprovalFlag<>0 
             ) as HasAttendceCourseCount --已考勤课程数
           ,isnull((select sum(CourseLength) from Dep_Course_A where CourseFrom=0 and YearPlan=Dep_Course_B.YearPlan and DeptId=Dep_Course_B.DeptId  
             ),0.0) as PlanCourseLengthSum --计划课程总学时
           ,isnull((select sum(CourseLength) from Dep_Course_A where CourseFrom=2 and YearPlan=Dep_Course_B.YearPlan and DeptId=Dep_Course_B.DeptId  
             ),0.0) as NowCourseLengthSum --实际课程总学时
            ,(select count(*) from Dep_Course_A where CourseFrom=2 and YearPlan=Dep_Course_B.YearPlan and DeptId=Dep_Course_B.DeptId
				and Dep_Course_A.IsYearPlan=1
             ) as NowOpenCourseCountnei
              ,(select count(*) from Dep_Course_A where CourseFrom=2 and YearPlan=Dep_Course_B.YearPlan and DeptId=Dep_Course_B.DeptId
				and Dep_Course_A.IsYearPlan=0
             ) as NowOpenCourseCountwai
    from Dep_Course_B  
    where 1=1 
          {1} 
) bb_yxt

)aa_yxt where rowNum between   @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", pageLength, where, orderBy, withTablesSql);

                var parameters = new
                {
                    startIndex = startIndex,
                    startLength = pageLength
                };
                return connection.Query<Dep_Course>(query, parameters).ToList();
            }

        }
        #endregion

        public Dep_Course GetCo_CourseAll(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = @"select Dep_Course.* 
,Sys_User.Realname AS TeacherName
,Sys_User.IsDelete AS TeacherIsDelete
,Sys_User.IsTeacher AS TeacherIsTeacher
,Dep_ClassRoom.RoomName
,Sys_Department.DeptName 
 from Dep_Course 
LEFT JOIN Sys_User ON Dep_Course.Teacher=Sys_User.UserId 
LEFT Join Dep_ClassRoom ON Dep_ClassRoom.Id=Dep_Course.RoomId  
LEFT Join Sys_Department ON Sys_Department.DepartmentId=Dep_Course.DeptId  
where Dep_Course.Id=@Id ";
                var param = new
                {
                    Id
                };
                return connection.Query<Dep_Course>(sqlwhere, param).FirstOrDefault();

            }
        }



        /// <summary>
        /// 有评价的课程
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Dep_Course> GetNoCPACourse(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string order = "Id asc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (SELECT row_number()OVER(ORDER BY " + order + ") numbe,count(*)OVER(PARTITION BY null) totalcount,* FROM Dep_Course WHERE " + where + " ) result WHERE  numbe BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex");
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Dep_Course>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 课程评价详细
        /// </summary>
        /// <param name="CourseID">课程ID</param>
        /// <param name="CoexmaID">课程评价试卷ID</param>
        /// <param name="TeexmaID">讲师评价试卷ID</param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<Dep_Course> GetCourseAvg(int CourseID, int CoexmaID, int TeexmaID, int startIndex = 1, int startLength = int.MaxValue, string order = "tab.UserID asc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (SELECT row_number()OVER(ORDER BY " + order + ") numbe,count(*)OVER(PARTITION BY null) totalcount,tab.UserID,t1.CoAvg,t2.TeAvg,su.Realname,sd.DeptName FROM (SELECT distinct sr.UserID FROM Dep_Survey_ReplyAnswer AS sr LEFT JOIN Dep_Survey_Question AS sq ON sq.QuestionID=sr.QuestionID WHERE sr.SourceFrom=0 AND sq.QuestionType=3 AND sr.Status=1 AND sr.ObjectID=@CourseID) AS tab LEFT join (SELECT sr.UserID,avg(Case when (isnumeric(sr.SubjectiveAnswer)=0 or sq.QuestionType<>3) then 0 Else Cast(sr.SubjectiveAnswer as decimal(10,2)) End) AS CoAvg FROM Dep_Survey_ReplyAnswer AS sr LEFT JOIN Dep_Survey_Question AS sq ON sq.QuestionID=sr.QuestionID WHERE sr.SourceFrom=0 AND sq.QuestionType=3 AND sr.Status=1 AND sr.ObjectID=@CourseID AND sr.ExampaperID=@CoexmaID GROUP BY sr.UserID ) AS t1 ON tab.UserID=t1.UserID left JOIN (SELECT sr.UserID,avg(Case when isnumeric(sr.SubjectiveAnswer)=0 then 0 Else Cast(sr.SubjectiveAnswer as decimal(10,2)) End) AS TeAvg FROM Dep_Survey_ReplyAnswer AS sr LEFT JOIN Dep_Survey_Question AS sq ON sq.QuestionID=sr.QuestionID WHERE sr.SourceFrom=0 AND sq.QuestionType=3 AND sr.Status=1 AND sr.ObjectID=@CourseID AND sr.ExampaperID=@TeexmaID GROUP BY sr.UserID ) AS t2 ON tab.UserID=t2.UserID LEFT JOIN Sys_User AS su ON su.UserId=tab.UserID LEFT JOIN Sys_Department AS sd ON su.DeptId=sd.DepartmentId) result WHERE  numbe BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex");
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    CourseID = CourseID,
                    CoexmaID = CoexmaID,
                    TeexmaID = TeexmaID
                };
                return conn.Query<Dep_Course>(sql, param).ToList();
            }
        }

        #region == 部门指定查询 ==
        /// <summary>
        /// （部门负责人）获得部门指定查询列表
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <param name="where">条件语句‘and ....’</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按Id倒序排序</param>
        /// <returns></returns>
        public List<Dep_Course> DepSelfCourseAppointSearch(out int totalCount, string where = "",
                                                   int startIndex = 1, int pageLength = int.MaxValue,
                                                    string orderBy = " ORDER BY Id desc ")
        {

            using (IDbConnection connection = OpenConnection())
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = " ORDER BY Id desc ";
                }
                string totalCountSql =
                string.Format(@"
                SELECT count(1)  FROM
(
	SELECT row_number() over( {1}) as rowNum,* 
FROM
(
	select  cc.*
        ,u.RealName as TeacherName    
        ,(select count(*) from Dep_CourseOrder dco 
            where dco.courseid = cc.id 
				   And (   
						  (dco.orderstatus = 1 and dco.isleave = 0 )
						   or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
						) 
        ) as HasEntered --已报名人员总数
        ,(select count(*) from Dep_CourseOrder dco 
            where dco.courseid = cc.id 
                   and dco.IsAppoint=1 --指定
				   And (   
						  (dco.orderstatus = 1 and dco.isleave = 0 )
						   or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
						) 
          ) as AssignUserCount --指定报名人员
    from Dep_Course cc
    left join Sys_User u on u.Userid = cc.teacher 
    where cc.IsDelete=0 --正常
		    and cc.Publishflag=1 --已发布
		    --and cc.Way=1 --集中课程 
		    and cc.CourseFrom=2 --课程管理 
		    and cc.IsOpenOthers=0 --部门自学 
		    and cc.IsOrder in (2,3) --指定，兼有
           {0}
) bb_yxt 
 ) aa_yxt
", where, orderBy);
                totalCount = connection.Query<int>(totalCountSql).First();


                string query = string.Format(@"
select top({0}) * from(
SELECT row_number() over({2}) as rowNum,* 
FROM
(
	select  cc.*
        ,u.RealName as TeacherName    
        ,(select count(*) from Dep_CourseOrder dco 
            where dco.courseid = cc.id 
				   And (   
						  (dco.orderstatus = 1 and dco.isleave = 0 )
						   or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
						) 
        ) as HasEntered --已报名人员总数
        ,(select count(*) from Dep_CourseOrder dco 
            where dco.courseid = cc.id 
                   and dco.IsAppoint=1 --指定
				   And (   
						  (dco.orderstatus = 1 and dco.isleave = 0 )
						   or (dco.orderstatus = 1 and dco.isleave = 1 and dco.approvalflag in (0,2)) 
						) 
          ) as AssignUserCount --指定报名人员
    from Dep_Course cc
    left join Sys_User u on u.Userid = cc.teacher 
    where cc.IsDelete=0 --正常
		    and cc.Publishflag=1 --已发布
		    --and cc.Way=1 --集中课程 
		    and cc.CourseFrom=2 --课程管理 
		    and cc.IsOpenOthers=0 --部门自学 
		    and cc.IsOrder in (2,3) --指定，兼有
           {1}
	       
) bb_yxt

)aa_yxt where rowNum between   @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", pageLength, where, orderBy);

                var parameters = new
                {
                    startIndex = startIndex,
                    startLength = pageLength
                };
                return connection.Query<Dep_Course>(query, parameters).ToList();
            }
        }
        #endregion

        #region 我开放的课程指定查询
        /// <summary>
        /// 我开放的课程指定查询
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Co_Course> GetMyOpenCourse(int deptID, string where = " 1 = 1 ", int startIndex = 1, int pageLength = int.MaxValue, string jsRenderSortField = "Co_Course.Id desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
select  * from (
     select  row_number() over( order by {2} ) as rowNum, count(1) OVER( PARTITION BY NULL) totalcount,
            (select count(0) from Cl_CourseOrder 
                left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                where Cl_CourseOrder.courseid = Co_Course.id and Cl_CourseOrder.[OrderStatus] in (1,2) 
                and ( Cl_CourseOrder.[IsLeave] = 0 
                        or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                        or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                            and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])
                    )
                and Sys_user.isDelete = 0
                --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) 
            ) as HasEntered,
            (select count(0) from Cl_CourseOrder 
                left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                where Cl_CourseOrder.courseid = Co_Course.id and Cl_CourseOrder.[OrderStatus] in (1,2)
                and ( Cl_CourseOrder.[IsLeave] = 0 
                        or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                        or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                            and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])
                    )
                and Cl_CourseOrder.IsAppoint=1
                and Sys_user.isDelete = 0
            ) as AssignUserCount
            ,Co_Course.*
            ,Sys_User.RealName as TeacherName ,dc.Id deptCourseID
    from Co_Course
    left join Sys_User on Sys_User.Userid = Co_Course.teacher
    LEFT JOIN Dep_Course dc ON dc.OpenCourseId=Co_Course.Id
    where  Co_Course.IsDelete = 0 and Co_Course.Publishflag = 1 
        and dc.IsOpenOthers=2 AND dc.CourseFrom=2 AND dc.way=2  AND dc.IsDelete=0
	AND dc.DeptId={0} and {1}
                                             
) result 
 where rowNum between   @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", deptID, where, jsRenderSortField);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = pageLength
                };
                return conn.Query<Co_Course>(sql, param).ToList();
            }

        }

        /// <summary>
        /// 查询课程指定的人员
        /// </summary>
        /// <param name="courseID">开放后的课程ID</param>
        /// <param name="deptCourseID">开放前的课程ID</param>
        /// <returns></returns>
        public List<Sys_User> GetCanOrderList(int courseID, int deptCourseID, string where = "1=1", int startIndex = 1, int pageLength = int.MaxValue, string jsRenderSortField = " DeptName desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
 
 DECLARE @year INT,@deptID INT,@yearID int  
SELECT @year=YearPlan,@deptID=DeptId FROM Dep_Course
WHERE Id={0};

WITH haveUserID AS
(
  SELECT Cl_CourseOrder.UserId from Cl_CourseOrder 
                left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                where Cl_CourseOrder.courseid = {1} and Cl_CourseOrder.[OrderStatus] in (1,2)
                and ( Cl_CourseOrder.[IsLeave] = 0 
                        or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                        or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                            and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])
                    )
                and Cl_CourseOrder.IsAppoint=1
                and Sys_user.isDelete = 0
)

 SELECT * FROM (                         
SELECT row_number() over( order by {3} ) as rowNum, count(1) OVER( PARTITION BY NULL) totalcount,UserId, Realname,DeptPath,TrainGrade,MobileNum,Email 
FROM Sys_User
WHERE (DeptId IN (
(SELECT ID FROM dbo.F_SplitIDs(dbo.F_GetDeptIDByCourse({0}))))
)   AND IsDelete=0  AND IsTeacher<2 AND Status = 0
AND   Sys_User.UserId NOT IN (SELECT UserId FROM haveUserID)
and {2})t 
where rowNum between   @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", deptCourseID, courseID, where, jsRenderSortField);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = pageLength
                };
                return conn.Query<Sys_User>(sql, param).ToList();

            }
        }

        /// <summary>
        /// 用来取消指定
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetDropList(int courseID, int deptCourseID, string where = "1=1", int startIndex = 1, int pageLength = int.MaxValue, string jsRenderSortField = "")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"DECLARE @year INT,@deptID INT,@yearID int  
SELECT @year=YearPlan,@deptID=DeptId FROM Dep_Course
WHERE Id={0};

WITH haveUserID AS
(
  SELECT UserId FROM Sys_User
WHERE (DeptId IN (
(SELECT ID FROM dbo.F_SplitIDs(dbo.F_GetDeptIDByCourse({0}))))
)   AND IsDelete=0  AND IsTeacher<2 AND Status = 0
)

SELECT * FROM (                         
SELECT row_number() over( order by {3} ) as rowNum, count(1) OVER( PARTITION BY NULL) totalcount,cco.UserId, Realname,DeptPath,TrainGrade,MobileNum,Email 
 from Cl_CourseOrder      cco
                left join Sys_user on  cco.UserId = Sys_user.userid 
                where cco.courseid = {1} and cco.[OrderStatus] in (1,2)
                and ( cco.[IsLeave] = 0 
                        or (cco.[IsLeave] = 1 and cco.[ApprovalFlag] <>1)
                        or (cco.[IsLeave] = 1 and cco.[ApprovalFlag] = 1 
                            and cco.[ApprovalDateTime] > cco.[ApprovalLimitTime])
                    )
                and cco.IsAppoint=1
                and Sys_user.isDelete = 0
                AND cco.UserId IN (SELECT UserId FROM haveUserID)
                And {2}
   )t  where rowNum between   @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", deptCourseID, courseID, where, jsRenderSortField);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = pageLength
                };
                return conn.Query<Sys_User>(sql, param).ToList();

            }
        }
        #endregion
    }
}
