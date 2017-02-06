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
    public class Co_CourseDB : BaseRepository
    {

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
                string sqlwhere = "select count(1) from Co_Course where IsDelete=0 AND way=@way AND  Code=@courseCode AND courseFrom=@courseFrom";
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
        /// 验证课程是否重名
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="courseFrom">课程来源 0：年度计划；1：月度计划；2：课程管理</param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExistCourseName(string courseName, int courseFrom = 2, int Id = 0)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = "select count(1) from Co_Course where  courseName=@courseName AND courseFrom=@courseFrom";
                if (Id > 0)
                    sqlwhere += " and Id <> " + Id;
                var param = new
                {
                    Id,
                    courseName,
                    courseFrom
                };
                int count = connection.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }

        public Co_Course GetCo_Course(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = @"select Co_Course.* ,Sys_User.Realname AS TeacherName,Sys_User.IsDelete AS TeacherIsDelete,Sys_User.IsTeacher AS TeacherIsTeacher,Sys_ClassRoom.RoomName
 from Co_Course LEFT JOIN Sys_User ON 
Co_Course.Teacher=Sys_User.UserId LEFT Join Sys_ClassRoom ON Sys_ClassRoom.Id=Co_Course.RoomId  where Co_Course.Id=@Id";
                var param = new
                {
                    Id
                };
                return connection.Query<Co_Course>(sqlwhere, param).FirstOrDefault();

            }
        }

        public Co_Course GetCo_Course(int Id, int userid)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = @"select Co_Course.* ,(select PassStatus from Cl_CourseOrder where Cl_CourseOrder.CourseId=@Id and UserId=@userid) as PassStatus,Sys_User.Realname AS TeacherName,Sys_User.IsDelete AS TeacherIsDelete,Sys_User.IsTeacher AS TeacherIsTeacher,Sys_ClassRoom.RoomName
 from Co_Course LEFT JOIN Sys_User ON 
Co_Course.Teacher=Sys_User.UserId LEFT Join Sys_ClassRoom ON Sys_ClassRoom.Id=Co_Course.RoomId  where Co_Course.Id=@Id";
                var param = new
                {
                    Id,
                    userid
                };
                return connection.Query<Co_Course>(sqlwhere, param).FirstOrDefault();

            }
        }


        public bool UpdateCourse(Co_Course model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"update [Co_Course] set CourseName=@CourseName,Code=@Code,Year=@Year,Month=@Month,Day=@Day,
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
,courseProvide = @courseprovide,studyRequirement = @studyrequirement,remark = @remark
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
                    model.studyRequirement,
                    model.remark
                };
                return connection.Execute(strSql.ToString(), param) > 0;
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
                strSql.Append(@"update [Co_Course] set CourseTimes=@courseTimes
             where Code=@code and coursefrom=2 and way=1 ");
                var param = new
                {
                    code,
                    courseTimes
                };
                return connection.Execute(strSql.ToString(), param) > 0;
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
                    strSql.AppendFormat(@"update [Co_Course] set
            LastUpdateTime=@LastUpdateTime,  {0}
             where Id=@Id ", "IsDelete=1");
                }
                if (flag == 1)
                {
                    strSql.AppendFormat(@"update [Co_Course] set
            LastUpdateTime=@LastUpdateTime,  {0}
             where Id=@Id ", " Publishflag=1 ");
                }

                var param = new
                {
                    Id = courseId,
                    LastUpdateTime = DateTime.Now
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }

        }

        /// <summary>
        /// 新增课程
        /// </summary>
        /// <param name="model"></param>
        public void AddCourse(Co_Course model)
        {
            using (IDbConnection connection = OpenConnection())
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into [Co_Course] (");
                strSql.Append(@"CourseName,Code,Year,Month,Day,OpenLevel,IsMust,Way,Teacher,StartTime,EndTime,Sort,CourseLength,RoomId,NumberLimited,IsCPA,AdFlag,IsOrder,IsLeave,IsRT,OpenFlag,OpenGroupObject,OpenDepartObject,IsTest,IsPing,SurveyPaperId,Memo,CourseFrom,StopOrderFlag,StopDucueFlag,ReturnTimes,AfterOrderTimes,AttFlag,PreAdviceConfigTime,AfterEvlutionConfigTime,PreCourseTime,Publishflag,CourseTimes,LastUpdateTime,IsDelete,TrainDays,CourseAddress,CourseObjectMemo,OpenPerson,CpaTeacher,YearPlan,IsSystem,IsYearPlan,CourseLengthDistribute,IsOpenSub,courseProvide,studyRequirement,remark)");
                strSql.Append(" values (");
                strSql.Append(@"@CourseName,@Code,@Year,@Month,@Day,@OpenLevel,@IsMust,@Way,@Teacher,@StartTime,@EndTime,@Sort,@CourseLength,@RoomId,@NumberLimited,@IsCPA,@AdFlag,@IsOrder,@IsLeave,@IsRT,@OpenFlag,@OpenGroupObject,@OpenDepartObject,@IsTest,@IsPing,@SurveyPaperId,@Memo,@CourseFrom,@StopOrderFlag,@StopDucueFlag,@ReturnTimes,@AfterOrderTimes,@AttFlag,@PreAdviceConfigTime,@AfterEvlutionConfigTime,@PreCourseTime,@Publishflag,@CourseTimes,@LastUpdateTime,@IsDelete,@TrainDays,@CourseAddress,@CourseObjectMemo,@OpenPerson,@CpaTeacher,@YearPlan,@IsSystem,@IsYearPlan,@CourseLengthDistribute,@IsOpenSub,@courseProvide,@studyRequirement,@remark)");
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
                    model.studyRequirement,
                    model.remark
                };

                decimal id =
                    connection.Query<decimal>(strSql.ToString(), param)
                              .FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }


        /// <summary>
        /// 获取课程列表 CPA课程 并读取是否已经录入成绩完毕
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Co_Course> GetCourseCPAListImportScore(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                              int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Co_Course where " + where + @" AND Co_Course.IsDelete=0 ")
                              .First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,Co_Course.Id,Co_Course.CourseName,Co_Course.CourseLength,Co_Course.StartTime,Co_Course.EndTime,Co_Course.CpaTeacher,Co_Course.OpenLevel,Co_Course.TrainDays,Co_Course.Publishflag,Co_Course.CourseObjectMemo,CourseAddress,sum(GradeStatus) AS CPAUNImportCount
 from  Co_Course LEFT JOIN Cl_CpaLearnStatus ON Co_Course.Id=
Cl_CpaLearnStatus.CourseID where " + where + @" AND Co_Course.IsDelete=0                           
GROUP BY Co_Course.Id,Co_Course.CourseName,Co_Course.CourseLength,Co_Course.StartTime,Co_Course.EndTime,Co_Course.CpaTeacher,Co_Course.OpenLevel,Co_Course.TrainDays,Co_Course.Publishflag,Co_Course.CourseObjectMemo,CourseAddress

) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Co_Course>(query, parameters).ToList();
            }
        }



        /// <summary>
        /// 获取课程列表 CPA课程
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Co_Course> GetCourseCPAList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                              int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Co_Course where " + where + @" AND Co_Course.IsDelete=0 ")
                              .First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,Co_Course.*
 from Co_Course 
    where " + where + @" AND Co_Course.IsDelete=0 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Co_Course>(query, parameters).ToList();
            }
        }



        /// <summary>
        /// 获取课程列表 视频课程
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Co_Course> GetCourseVideoList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                              int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Co_Course where " + where + @" AND Co_Course.IsDelete=0 ")
                              .First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,Co_Course.*,Sys_User.Realname AS TeacherName,Sys_User.IsDelete AS TeacherIsDelete,Sys_User.IsTeacher AS TeacherIsTeacher
 from Co_Course LEFT JOIN Sys_User ON 
Co_Course.Teacher=Sys_User.UserId 
    where " + where + @" AND Co_Course.IsDelete=0 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Co_Course>(query, parameters).ToList();
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
        public List<Co_Course> GetCourseTogetherList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Co_Course where " + where + @" AND Co_Course.IsDelete=0 ")
                              .First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,Co_Course.*,Sys_User.Realname AS TeacherName,Sys_User.IsDelete AS TeacherIsDelete,Sys_User.IsTeacher AS TeacherIsTeacher, Sys_ClassRoom.RoomName,CourseTimesOrder,TotalCourseTimes
--,
--row_number() OVER(PARTITION BY Co_Course.Code ORDER BY StartTime,Co_Course.Id asc) AS CourseTimesOrder,
--count(Code)over( PARTITION BY Co_Course.Code) AS TotalCourseTimes
 from Co_Course LEFT JOIN Sys_User ON 
Co_Course.Teacher=Sys_User.UserId LEFT Join Sys_ClassRoom ON Sys_ClassRoom.Id=Co_Course.RoomId 
left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime,cc.Id asc) as CourseTimesOrder,
			count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
    from Co_Course cc where cc.isdelete = 0 and cc.coursefrom = 2  AND way=1 ) tt 
    on tt.id = Co_Course.id and tt.code = Co_Course.code 
    where " + where + @" AND Co_Course.IsDelete=0 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Co_Course>(query, parameters).ToList();
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
        public List<Co_Course> GetCourseCommonList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                      int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Co_Course where " + where + @" AND Co_Course.IsDelete=0 ")
                              .First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,Co_Course.*,Sys_User.Realname AS TeacherName,Sys_ClassRoom.RoomName
,Sys_User.MobileNum as TeacherMobileNum,Sys_User.email as TeacherEmail
 from Co_Course LEFT JOIN Sys_User ON 
Co_Course.Teacher=Sys_User.UserId LEFT Join Sys_ClassRoom ON Sys_ClassRoom.Id=Co_Course.RoomId 
    where " + where + @" AND Co_Course.IsDelete=0 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Co_Course>(query, parameters).ToList();
            }
        }


        /// <summary>
        /// CPA课程
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Co_Course> GetCPACourseList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int userid = 0,
                                      int pageLength = int.MaxValue, string orderBy = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Cl_CpaLearnStatus left join Co_Course on Cl_CpaLearnStatus.CourseID=Co_Course.id where  UserID=" + userid + where + " and CpaFlag=1 ").First();
                ;

                string query = string.Format(@" 
        select top ({0}) * from (
                select   row_number() over( ORDER BY Co_Course.Id DESC ) as rowNum,
              Co_Course.Id ,
            Co_Course.CourseName,
                 Cl_CpaLearnStatus.id as CpaLearnStatus,
              Co_Course.CourseLength,
              Co_Course.OpenLevel,
              Co_Course.CpaTeacher,
              Co_Course.NumberLimited,
              Co_Course.StartTime,
              Co_Course.EndTime,
              Co_Course.TrainDays,
              Co_Course.CourseAddress,
              Cl_CpaLearnStatus.GetLength
               from Cl_CpaLearnStatus 
                 left join Co_Course on Cl_CpaLearnStatus.CourseID=Co_Course.id 
                 where UserID={1} and CpaFlag=1  {2}
                )bb where   rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, userid, where);



                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Co_Course>(query, parameters).ToList();
            }
        }

        public Co_Course GetVideoCo_CourseById(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = @"select Co_Course.* ,Cl_CpaLearnStatus.[IsAttFlag] as [Status],Sys_User.Realname AS TeacherName,Co_Coursepaper.TestTimes
 from Co_Course LEFT JOIN Cl_CpaLearnStatus ON 
Co_Course.id=Cl_CpaLearnStatus.CourseID LEFT JOIN Sys_User ON 
Sys_User.UserId=Co_Course.Teacher LEFT JOIN Co_Coursepaper ON Co_Coursepaper.CourseId=Co_Course.Id where Co_Course.Id=@Id";
                var param = new
                {
                    Id
                };
                return connection.Query<Co_Course>(sqlwhere, param).FirstOrDefault();

            }
        }

        public List<Co_Course> GetVideoCourseList(out int totalCount, string where = " 1 = 1 ", string TrainGrade = "", int Userid = 0, int DeptId = 0, string IsOpenSub = "", int startIndex = 0,
                                                  int pageLength = int.MaxValue,
                                                  string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC")
        {

            using (IDbConnection connection = OpenConnection())
            {

                string strsql = string.Format(@"
                select count(1) from (
  select  row_number() over( ORDER BY Co_Course.Code,Co_Course.Id DESC ) as rowNum,
   Co_Course.Id, Co_Course.CourseName,
    Co_Course.CourseLength,
    Co_Course.Way,
    Co_Course.Publishflag,
     Co_Course.IsMust,
     Co_Course.OpenLevel,
     Co_Course.IsDelete,
     Co_Course.CourseFrom,
    Co_Course.IsTest,
    Co_Course.IsPing,
Sys_User.Realname AS TeacherName,
     Co_Course.OpenFlag,
     Co_Course.OpenDepartObject,
      Co_Course.StartTime,
       Co_Course.EndTime,       
       Co_Course.OpenGroupObject,   
        Cl_CpaLearnStatus.IsPass as  [Status],
       Cl_CpaLearnStatus.GetLength as GetLength,     
     (select top(1) ResourceType from Co_CourseResource where Co_CourseResource.CourseId=Co_Course.id and IsDelete=0) as  ResourceType
     --(select IsPass from Cl_CpaLearnStatus where Cl_CpaLearnStatus.CourseID=Co_Course.Id and Cl_CpaLearnStatus.UserID={0} and CpaFlag=0) as [Status],
    --(select GetLength from Cl_CpaLearnStatus where Cl_CpaLearnStatus.CourseID=Co_Course.Id and Cl_CpaLearnStatus.UserID={0} and CpaFlag=0) as GetLength
       from Co_Course  left join Cl_CpaLearnStatus on Cl_CpaLearnStatus.CourseID=Co_Course.Id and Cl_CpaLearnStatus.UserID={0} and CpaFlag=0 LEFT JOIN Sys_User ON Sys_User.UserId=Co_Course.Teacher 
        where  {3} and
			Co_Course.Way=2 and
			Co_Course.IsDelete=0 and
			Co_Course.Publishflag=1 and Co_Course.CourseFrom=2 
			and	( (Co_Course.OpenFlag=0 and charindex('{1}',Co_Course.OpenLevel)>0)			
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
				)  {4}) 

          ) result  
              ", Userid, TrainGrade, DeptId, where, IsOpenSub);

                totalCount = connection.Query<int>(strsql)
                              .First();

                string querystr = string.Format(@"

         
	select top ({3}) * from
	(
         select  row_number() over( ORDER BY result.Id DESC ) as rowNum,* from (
  select 
   Co_Course.Id, Co_Course.CourseName,
    Co_Course.CourseLength,
    Co_Course.Way,
    Co_Course.Publishflag,
     Co_Course.IsMust,
     Co_Course.OpenLevel,
     Co_Course.IsDelete,
     Co_Course.CourseFrom,
    Co_Course.IsTest,
    Co_Course.IsPing,
Sys_User.Realname AS TeacherName,
     Co_Course.OpenFlag,
    Co_Course.IsOpenSub,
     Co_Course.OpenDepartObject,
      Co_Course.StartTime,
       Co_Course.EndTime,       
       Co_Course.OpenGroupObject, 
    Cl_CpaLearnStatus.IsPass as  [Status],
       Cl_CpaLearnStatus.GetLength as GetLength,        
     (select top(1) ResourceType from Co_CourseResource where Co_CourseResource.CourseId=Co_Course.id and IsDelete=0) as  ResourceType
      --(select IsPass from Cl_CpaLearnStatus where Cl_CpaLearnStatus.CourseID=Co_Course.Id and Cl_CpaLearnStatus.UserID={0} and CpaFlag=0) as [Status],
--(select GetLength from Cl_CpaLearnStatus where Cl_CpaLearnStatus.CourseID=Co_Course.Id and Cl_CpaLearnStatus.UserID={0} and CpaFlag=0) as GetLength
       from Co_Course left join Cl_CpaLearnStatus on Cl_CpaLearnStatus.CourseID=Co_Course.Id and Cl_CpaLearnStatus.UserID={0} and CpaFlag=0 
LEFT JOIN Sys_User ON Sys_User.UserId=Co_Course.Teacher 
        where   {4} and
			Co_Course.Way=2 and
			Co_Course.IsDelete=0 and
			Co_Course.Publishflag=1 and Co_Course.CourseFrom=2 
			and	( (Co_Course.OpenFlag=0 and charindex('{1}',Co_Course.OpenLevel)>0)			
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
				) {5} ) 
          ) result where {4}
)aa
where   rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", Userid, TrainGrade, DeptId, pageLength, where, IsOpenSub);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Co_Course>(querystr, parameters).ToList();


            }
        }


        /// <summary>
        /// 有评价的课程
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Co_Course> GetNoCPACourse(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string order = "Id asc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (SELECT row_number()OVER(ORDER BY " + order + ") numbe,count(*)OVER(PARTITION BY null) totalcount,* FROM Co_Course WHERE " + where + " ) result WHERE  numbe BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex");
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Co_Course>(sql, param).ToList();
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
        public List<Co_CourseShow> GetCourseAvg(int CourseID, int CoexmaID, int TeexmaID, int startIndex = 1, int startLength = int.MaxValue, string order = "tab.UserID asc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM (SELECT row_number()OVER(ORDER BY " + order + ") numbe,count(*)OVER(PARTITION BY null) totalcount,tab.UserID,t1.CoAvg,t2.TeAvg,su.Realname,sd.DeptName FROM (SELECT distinct sr.UserID FROM Survey_ReplyAnswer AS sr LEFT JOIN Survey_Question AS sq ON sq.QuestionID=sr.QuestionID WHERE sr.SourceFrom=0 AND sq.QuestionType=3 AND sr.Status=1 AND sr.ObjectID=@CourseID) AS tab LEFT join (SELECT sr.UserID,avg(Case when (isnumeric(sr.SubjectiveAnswer)=0 or sq.QuestionType<>3) then 0 Else Cast(sr.SubjectiveAnswer as decimal(10,2)) End) AS CoAvg FROM Survey_ReplyAnswer AS sr LEFT JOIN Survey_Question AS sq ON sq.QuestionID=sr.QuestionID WHERE sr.SourceFrom=0 AND sq.QuestionType=3 AND sr.Status=1 AND sr.ObjectID=@CourseID AND sr.ExampaperID=@CoexmaID GROUP BY sr.UserID ) AS t1 ON tab.UserID=t1.UserID left JOIN (SELECT sr.UserID,avg(Case when isnumeric(sr.SubjectiveAnswer)=0 then 0 Else Cast(sr.SubjectiveAnswer as decimal(10,2)) End) AS TeAvg FROM Survey_ReplyAnswer AS sr LEFT JOIN Survey_Question AS sq ON sq.QuestionID=sr.QuestionID WHERE sr.SourceFrom=0 AND sq.QuestionType=3 AND sr.Status=1 AND sr.ObjectID=@CourseID AND sr.ExampaperID=@TeexmaID GROUP BY sr.UserID ) AS t2 ON tab.UserID=t2.UserID LEFT JOIN Sys_User AS su ON su.UserId=tab.UserID LEFT JOIN Sys_Department AS sd ON su.DeptId=sd.DepartmentId) result WHERE  numbe BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex");
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength,
                    CourseID = CourseID,
                    CoexmaID = CoexmaID,
                    TeexmaID = TeexmaID
                };
                return conn.Query<Co_CourseShow>(sql, param).ToList();
            }
        }
    }
}
