using System;
using System.Collections.Generic;
using System.Linq;
using LiXinModels;
using LiXinModels.CourseLearn;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.MyApproval
{
    public class MyApprovalDB : BaseRepository
    {
        #region 我的请假审批记录

        /// <summary>
        /// 获取我审批的请假记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<BaseApprovalInfor> GetMyApprovalLeavePaging(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by a.LeaveTime desc ")
        {
            using (var connection = OpenConnection())
            {
                totalCount = connection.Query<int>("select count(1) from Cl_CourseOrder as a,Sys_User as b,Co_Course as c where a.CourseId=c.Id and a.UserId=b.UserId and a.IsLeave=1  " + where).First();
                var query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,a.Id as ID,b.Realname as RealName,a.LeaveTime as SubmitTime,c.Code as CourseCode,c.CourseName as CourseName,c.CourseLength as CourseLength,a.ApprovalFlag as ApprovalFlag,a.ApprovalDateTime as ApprovalTime,a.ApprovalLimitTime as ApprovalLimitTime,1 as TypeFlag   from  Cl_CourseOrder as a,Sys_User as b,Co_Course as c 
    where a.CourseId=c.Id and a.UserId=b.UserId and a.IsLeave=1 " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<BaseApprovalInfor>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 获取单个请假审批详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetMyApprovalLeaveInfor(int id)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select b.Realname as RealName,1 as TypeFlag,
b.Sex,b.TrainGrade,b.PayGrade,a.LeaveTime as OrderTime,a.Reson as Reason,
c.CourseName,c.Code as CourseCode,c.CourseLength,
(select Realname from Sys_User where UserId=c.Teacher) as Teacher,
c.StartTime,c.EndTime,a.ApprovalDateTime,a.ApprovalFlag,a.ApprovalMemo as RefuseReason,a.ApprovalLimitTime
from Cl_CourseOrder as a,Sys_User as b,Co_Course as c
where a.CourseId=c.Id and a.UserId=b.UserId and a.Id=@id");
                var parameters = new
                {
                    id
                };
                return connection.Query<ApprovalUserInfor>(query, parameters).FirstOrDefault();
            }
        }

        #endregion

        #region 我的补预定审批记录

        /// <summary>
        /// 获取我审批的补预定记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<BaseApprovalInfor> GetMyApprovalMakeUpPaging(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by a.LeaveTime desc ")
        {
            using (var connection = OpenConnection())
            {
                totalCount = connection.Query<int>("select count(1) from Cl_MakeUpOrder as a,Sys_User as b,Co_Course as c where a.CourseId=c.Id and a.UserId=b.UserId " + where).First();
                var query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,a.Id as ID,b.Realname as RealName,a.LeaveTime as SubmitTime,c.Code as CourseCode,c.CourseName as CourseName,c.CourseLength as CourseLength,a.ApprovalFlag as ApprovalFlag,a.ApprovalDateTime as ApprovalTime,a.ApprovalLimitTime as ApprovalLimitTime,2 as TypeFlag   from  Cl_MakeUpOrder as a,Sys_User as b,Co_Course as c 
    where a.CourseId=c.Id and a.UserId=b.UserId " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<BaseApprovalInfor>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 获取单个补预定审批详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetMyApprovalMakeUpInfor(int id)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select b.Realname as RealName,2 as TypeFlag,
b.Sex,b.TrainGrade,b.PayGrade,a.LeaveTime as OrderTime,a.AttStartTime,a.AttEndTime,
c.CourseName,c.Code as CourseCode,c.CourseLength,
(select Realname from Sys_User where UserId=c.Teacher) as Teacher,
c.StartTime,c.EndTime,a.ApprovalDateTime,a.ApprovalFlag,a.ApprovalMemo as RefuseReason,a.ApprovalLimitTime
from Cl_MakeUpOrder as a,Sys_User as b,Co_Course as c
where a.CourseId=c.Id and a.UserId=b.UserId and a.Id=@id");
                var parameters = new
                {
                    id
                };
                return connection.Query<ApprovalUserInfor>(query, parameters).FirstOrDefault();
            }
        }

        #endregion

        #region 我的逾时审批记录

        /// <summary>
        /// 获取我审批的逾时申请记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<BaseApprovalInfor> GetMyApprovalOutTimePaging(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by a.OutTime desc ")
        {
            using (var connection = OpenConnection())
            {
                totalCount = connection.Query<int>("select count(1) from Cl_TimeOutOrder as a,Sys_User as b,Co_Course as c where a.CourseId=c.Id and a.UserId=b.UserId " + where).First();
                var query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,a.Id as ID,b.Realname as RealName,a.OutTime as SubmitTime,c.Code as CourseCode,c.CourseName as CourseName,c.CourseLength as CourseLength,a.ApprovalFlag as ApprovalFlag,a.ApprovalDateTime as ApprovalTime,3 as TypeFlag from  Cl_TimeOutOrder as a,Sys_User as b,Co_Course as c 
    where a.CourseId=c.Id and a.UserId=b.UserId " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<BaseApprovalInfor>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 获取单个逾时补预定审批详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetMyApprovalOutTimeInfor(int id)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select b.Realname as RealName,3 as TypeFlag,
b.Sex,b.TrainGrade,b.PayGrade,a.OutTime as OrderTime,a.AttStartTime,a.AttEndTime,
c.CourseName,c.Code as CourseCode,c.CourseLength,
(select Realname from Sys_User where UserId=c.Teacher) as Teacher,
c.StartTime,c.EndTime,a.ApprovalDateTime,a.ApprovalFlag,a.ApprovalMemo as RefuseReason,
a.MakeUpTime
from Cl_TimeOutOrder as a,Sys_User as b,Co_Course as c
where a.CourseId=c.Id and a.UserId=b.UserId and a.Id=@id");
                var parameters = new
                {
                    id
                };
                return connection.Query<ApprovalUserInfor>(query, parameters).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取单个逾时补预定审批详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetMyApprovalOutTimeLeaveInfor(int id)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select b.Realname as RealName,3 as TypeFlag,
b.Sex,b.TrainGrade,b.PayGrade,a.OutTime as OrderTime,
c.CourseName,c.Code as CourseCode,c.CourseLength,
(select Realname from Sys_User where UserId=c.Teacher) as Teacher,
c.StartTime,c.EndTime,a.ApprovalDateTime,a.ApprovalFlag,a.ApprovalMemo as RefuseReason,
a.MakeUpTime
from Cl_TimeOutLeave as a,Sys_User as b,Co_Course as c
where a.CourseId=c.Id and a.UserId=b.UserId and a.Id=@id");
                var parameters = new
                {
                    id
                };
                return connection.Query<ApprovalUserInfor>(query, parameters).FirstOrDefault();
            }
        }
        #endregion

        #region 我的违纪情况审批记录

        /// <summary>
        /// 获取需要我审批的课程违纪记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<PrincipleInfor> GetMyApprovalPrinciplePaging(out int totalCount, string where = " and 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by a.AppDateTime desc ")
        {
            using (var connection = OpenConnection())
            {
                totalCount = connection.Query<int>("select count(1) from Cl_Attendce as a,Co_Course as b,Sys_User as c where a.CourseId=b.Id and a.UserId=c.UserId and a.LessLength<0 and a.IsApp=1 " + where).First();
                var query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,a.Id as ID,a.UserId as UserID,c.Realname as RealName,c.DeptName as DepartName,
a.CourseId as CourseID,b.CourseName as CourseName,b.AttFlag,
c.TrainGrade,b.CourseLength,a.LessLength,b.StartTime as CourseStartTime,
b.EndTime as CourseEndTime,a.StartTime as AttStartTime,a.EndTime as AttEndTime,a.IsApp,
a.ApprovalFlag as Status,a.AppDateTime from Cl_Attendce as a,Co_Course as b,Sys_User as c
where a.CourseId=b.Id
and a.UserId=c.UserId
and a.LessLength<0
and a.IsApp=1 " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<PrincipleInfor>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 获取单个课程违纪详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetMyApprovalPrincipleInfor(int id)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select b.Realname as RealName,4 as TypeFlag,
b.Sex,b.TrainGrade,b.PayGrade,a.AppDateTime as OrderTime,a.Reason as Reason,c.AttFlag,
c.CourseName,c.Code as CourseCode,c.CourseLength,a.LessLength,a.StartTime as AttStartTime,a.EndTime as AttEndTime,
(select Realname from Sys_User where UserId=c.Teacher) as Teacher,
c.StartTime,c.EndTime,a.ApprovalDateTime,a.ApprovalFlag,a.ApprovalMemo as RefuseReason
from Cl_Attendce as a,Sys_User as b,Co_Course as c
where a.CourseId=c.Id and a.UserId=b.UserId and a.Id=@id");
                var parameters = new
                {
                    id
                };
                return connection.Query<ApprovalUserInfor>(query, parameters).FirstOrDefault();
            }
        }

        #endregion

        #region 我的违纪情况管理

        /// <summary>
        /// 获取我的违纪情况列表
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<PrincipleInfor> GetMyPrinciplePaging(out int totalCount, string where = " and 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by b.StartTime desc ")
        {
            using (var connection = OpenConnection())
            {
                totalCount = connection.Query<int>("select count(1) from Cl_Attendce as a,Co_Course as b,Sys_User as c where a.CourseId=b.Id and a.UserId=c.UserId " +
                    "and (select count(*) from cl_courseorder where userid=a.userid and courseid=a.courseid and (isleave=0 or (isleave=1 and approvalFlag<>1) or (isleave=1 and approvalFlag=1 and approvaldatetime>ApprovalLimitTime)))>0 " +
                                                   "and a.LessLength<0 " + where).First();
                var query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,a.Id as ID,a.UserId as UserID,c.Realname as RealName,c.DeptName as DepartName,
a.CourseId as CourseID,b.CourseName as CourseName,b.AttFlag,
c.TrainGrade,b.CourseLength,a.LessLength,b.StartTime as CourseStartTime,
b.EndTime as CourseEndTime,a.StartTime as AttStartTime,a.EndTime as AttEndTime,
a.ApprovalFlag as Status,a.AppDateTime,a.IsApp
from Cl_Attendce as a,Co_Course as b,Sys_User as c
where a.CourseId=b.Id
and a.UserId=c.UserId
and (select count(*) from cl_courseorder where userid=a.userid and courseid=a.courseid and (isleave=0 or (isleave=1 and approvalFlag<>1) or (isleave=1 and approvalFlag=1 and approvaldatetime>ApprovalLimitTime)))>0
and a.LessLength<0 " + where + @" 
) result 
where rowNum between  @pageLength*(@startIndex-1)+1  AND @pageLength*@startIndex", pageLength, orderBy);
                var parameters = new
                {
                    pageLength = pageLength,
                    startIndex = startIndex
                };
                return connection.Query<PrincipleInfor>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 获取我的违纪情况详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetMyPrincipleInfor(int id)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select a.Id as ID,c.CourseName,c.Code as CourseCode,c.CourseLength,a.LessLength,a.StartTime as AttStartTime,a.EndTime as AttEndTime,4 as TypeFlag,
(select Realname from Sys_User where UserId=c.Teacher) as Teacher,c.AttFlag,
c.StartTime,c.EndTime,a.ApprovalDateTime,a.ApprovalFlag,a.ApprovalMemo as RefuseReason,a.IsApp,
(select Realname from Sys_User where JobNum=b.LeaderID) as ApprovalName
from Cl_Attendce as a,Sys_User as b,Co_Course as c
where a.CourseId=c.Id and a.UserId=b.UserId and a.Id=@id");
                var parameters = new
                {
                    id
                };
                return connection.Query<ApprovalUserInfor>(query, parameters).FirstOrDefault();
            }
        }

        /// <summary>
        /// 申辩/审批违纪
        /// </summary>
        /// <param name="sqlstr">sql</param>
        /// <returns></returns>
        public int ExplainPrin(string sqlstr)
        {
            using (var connection = OpenConnection())
            {
                return connection.Execute(sqlstr);
            }
        }

        #endregion

        #region 违纪情况审批管理

        /// <summary>
        /// 获取需要我审批的课程违纪记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<PrincipleInfor> GetPrincipleManagePaging(out int totalCount, string where = " and 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by a.AppDateTime desc ")
        {
            using (var connection = OpenConnection())
            {
                totalCount = connection.Query<int>("select count(1) from Cl_Attendce as a,Co_Course as b,Sys_User as c where a.CourseId=b.Id and a.UserId=c.UserId and a.LessLength<0 and a.IsApp=1 " + where).First();
                var query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,a.Id as ID,a.UserId as UserID,c.Realname as RealName,c.DeptName as DepartName,
a.CourseId as CourseID,b.CourseName as CourseName,b.AttFlag,
c.TrainGrade,b.CourseLength,a.LessLength,b.StartTime as CourseStartTime,
b.EndTime as CourseEndTime,a.StartTime as AttStartTime,a.EndTime as AttEndTime,a.IsApp,
a.ApprovalFlag as Status,a.AppDateTime from Cl_Attendce as a,Co_Course as b,Sys_User as c
where a.CourseId=b.Id
and a.UserId=c.UserId
and a.LessLength<0
and a.IsApp=1 " + where + @" 
) result 
where rowNum between  @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", pageLength, orderBy);
                var parameters = new
                {
                    startLength = pageLength,
                    startIndex = startIndex
                };
                return connection.Query<PrincipleInfor>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 获取审批的课程违纪详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetPrincipleInfor(int id)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select a.Id as ID,
a.CourseId as CourseID,
a.UserId as UserID,
c.CourseName,
c.AttFlag,
b.Realname,
b.TrainGrade,
b.DeptName,
b.Sex,a.FileName,a.FileRealName,
b.PayGrade,
c.Code as CourseCode,
c.CourseLength,
a.LessLength,
a.StartTime as AttStartTime,
a.Reason,
a.EndTime as AttEndTime,
(select Realname from Sys_User where UserId=c.Teacher) as Teacher,
(select Realname from sys_User where JobNum=a.ApprovalUser) as ApprovalName,
c.StartTime,
c.EndTime,
a.ApprovalDateTime,
a.ApprovalFlag,
4 as TypeFlag,
a.ApprovalMemo as RefuseReason,a.IsApp
from Cl_Attendce as a,Sys_User as b,Co_Course as c
where a.CourseId=c.Id and a.UserId=b.UserId and a.Id=@id");
                var parameters = new
                {
                    id
                };
                return connection.Query<ApprovalUserInfor>(query, parameters).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取申辩信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AttendceInfor GetPrinReason(int id)
        {
            using (var connection = OpenConnection())
            {
                const string sqlstr = "select Reason,LessLength from Cl_Attendce where Id=@id";
                return connection.Query<AttendceInfor>(sqlstr, new
                {
                    id
                }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取课程违纪记录（培训管理员）
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<PrincipleInfor> GetPrincipleTrainManagePaging(out int totalCount, string where = " and 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by b.StartTime desc ")
        {
            using (var connection = OpenConnection())
            {
                totalCount = connection.Query<int>("select count(1) from Cl_Attendce as a,Co_Course as b,Sys_User as c where a.CourseId=b.Id and a.UserId=c.UserId and a.LessLength<0  " + where).First();
                var query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,a.Id as ID,a.UserId as UserID,c.Realname as RealName,c.DeptName as DepartName,
a.CourseId as CourseID,b.CourseName as CourseName,a.IsApp,b.AttFlag,
c.TrainGrade,b.CourseLength,a.LessLength,b.StartTime as CourseStartTime,
b.EndTime as CourseEndTime,a.StartTime as AttStartTime,a.EndTime as AttEndTime,(select Realname from Sys_User where JobNum=c.LeaderID) as ApprovalName,
a.ApprovalFlag as Status,a.AppDateTime from Cl_Attendce as a,Co_Course as b,Sys_User as c
where a.CourseId=b.Id
and a.UserId=c.UserId
and a.LessLength<0 " + where + @" 
) result 
where rowNum between @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", pageLength, orderBy);
                var parameters = new
                {
                    startLength = pageLength,
                    startIndex = startIndex
                };
                return connection.Query<PrincipleInfor>(query, parameters).ToList();
            }
        }

        #endregion

        #region 我的违纪情况申请记录

        /// <summary>
        /// 我的违纪情况申请记录列表
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<PrincipleInfor> GetMyPrincipleRecordPaging(out int totalCount, string where = " and 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by b.StartTime desc ")
        {
            using (var connection = OpenConnection())
            {
                totalCount = connection.Query<int>("select count(1) from Cl_Attendce as a,Co_Course as b,Sys_User as c where a.CourseId=b.Id and a.UserId=c.UserId and a.LessLength<0  and a.IsApp=1 " + where).First();
                var query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,a.Id as ID,a.UserId as UserID,c.Realname as RealName,c.DeptName as DepartName,
a.CourseId as CourseID,b.CourseName as CourseName,b.AttFlag,
c.TrainGrade,b.CourseLength,a.LessLength,b.StartTime as CourseStartTime,
b.EndTime as CourseEndTime,a.StartTime as AttStartTime,a.EndTime as AttEndTime,
a.ApprovalFlag as Status,a.AppDateTime,a.IsApp
from Cl_Attendce as a,Co_Course as b,Sys_User as c
where a.CourseId=b.Id
and a.UserId=c.UserId
and a.LessLength<0 and a.IsApp=1 " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<PrincipleInfor>(query, parameters).ToList();
            }
        }

        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Cl_TimeOutOrder GetTimeOutOrder(string where)
        {
            using (var connection = OpenConnection())
            {
                string sqlstr = "select * from Cl_TimeOutOrder ";
                if (!string.IsNullOrEmpty(where))
                {
                    if (!where.TrimStart().StartsWith("WHERE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sqlstr += " WHERE " + where;
                    }
                    else
                    {
                        sqlstr += where;
                    }
                }
                return connection.Query<Cl_TimeOutOrder>(sqlstr).FirstOrDefault();
            }
        }

        #region 我的请假逾时申请
        /// <summary>
        /// 我的请假逾时申请
        /// </summary>
        public List<Cl_CourseOrder> GetMyTimeOutOrder(int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Id desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT *, row_number()OVER(ORDER BY {0} ) number,count(*)OVER(PARTITION BY null) totalcount FROM(
SELECT cc.CourseName course_Name ,su.Realname,
su.TrainGrade,
dbo.GetRealNameByUserID(cc.Teacher) TeacherName
,cc.CourseLength
,cc.Code Course_Code
,cc.starttime
,cc.endtime
,TrainAppDatetime
,cco.Id,cco.LeaveTime
,cco.ApprovalUser
,(SELECT COUNT(*) FROM Cl_TimeOutLeave WHERE courseId = cco.courseId and UserId = cco.UserId) ApplyCount
,Cl_TimeOutLeave.TrainAppFlag 
FROM Cl_CourseOrder cco
LEFT JOIN Co_Course cc ON cco.CourseId=cc.Id
LEFT JOIN Sys_User su ON cco.UserId=su.UserId
LEFT JOIN Cl_TimeOutLeave on cco.courseId = Cl_TimeOutLeave.courseId and cco.UserId = Cl_TimeOutLeave.UserId
WHERE OrderStatus=1 AND cco.IsLeave=1  AND 
((cco.ApprovalFlag=0 AND cco.ApprovalLimitTime<getdate())
  OR (cco.ApprovalFlag>0 AND cco.ApprovalDateTime>cco.ApprovalLimitTime))
) result
where  {1}
)a WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", jsRenderSortField, where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Cl_CourseOrder>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertCl_TimeOutLeave(string Memo, int id, string LeaderID)
        {
            using (var conn = OpenConnection())
            {
                string strSql = string.Format(@"
INSERT INTO Cl_TimeOutLeave(CourseId,UserId,OutUserID,Name,MakeUpTime,CourseName,CourseStartTime,CourseEndTime,AppReason,OutTime,ApprovalFlag,TrainAppFlag,ApprovalUser)

SELECT cco.CourseId,cco.UserId,su.JobNum,su.Realname,cco.LeaveTime,cc.CourseName,cc.StartTime,cc.EndTime,
'{0}' AppReason ,getdate() OutTime,-1 ApprovalFlag,0 TrainAppFlag,'{2}' ApprovalUser
FROM Cl_CourseOrder cco 
LEFT JOIN Sys_User su ON cco.UserId=su.UserId
LEFT JOIN Co_Course cc ON cc.Id=cco.CourseId
WHERE cco.Id={1}", Memo, id, LeaderID);
                conn.Execute(strSql);

            }
        }

        /// <summary>
        /// 请假申请审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cl_TimeOutLeave GetSingleTimeOutLeave(int id)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM  Cl_TimeOutLeave
WHERE Id={0}", id);
                return conn.Query<Cl_TimeOutLeave>(sql).FirstOrDefault();
            }
        }
        #endregion

        #region 请假逾时申请审核
        /// <summary>
        /// 请假逾时申请审核
        /// </summary>
        public List<Cl_CourseOrder> GetTimeOutLeave(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Id desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT *,row_number()OVER(ORDER BY {0}) number,count(*)OVER(PARTITION BY null) totalcount FROM(
SELECT  su.Realname,su.TrainGrade,cc.CourseName Course_Name,cc.CourseLength,
dbo.GetRealNameByUserID(cc.Teacher) TeacherName,
Name ApplyUserRealName,
cc.Code Course_Code,
OutTime TimeOutApplyTime,	
TrainAppFlag,
AppReason,
cc.StartTime,
cc.EndTime,
Sys_ApplyUser.DeptName ApplyUserDeptName,
MakeUpTime LeaveTime,
TrainAppDatetime,
cto.Id FROM Cl_TimeOutLeave cto
LEFT JOIN Sys_User su ON cto.UserId=su.UserId
LEFT JOIN Co_Course cc ON cto.CourseId=cc.Id
left join dbo.Sys_User Sys_ApplyUser on Sys_ApplyUser.jobnum = cto.OutUserID
) a
WHERE {1}
) result WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", jsRenderSortField, where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Cl_CourseOrder>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 请假逾时申请审核动作
        /// </summary>
        public void UpdateTimeOutLeave(string ids, string approvalmemo, int approval)
        {

            var ApprovalFlag = approval == 2 ? -1 : 0;
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"  UPDATE Cl_TimeOutLeave
SET TrainAppFlag={0}, ApprovalReason='{1}', TrainAppDatetime=getdate(),ApprovalFlag={3}
WHERE Id in ({2})", approval, approvalmemo, ids, ApprovalFlag);
                conn.Execute(sql);
            }

        }
        #endregion

        #region 逾时审批请假
        /// <summary>
        /// 获取我审批的逾时申请记录（请假）
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<BaseApprovalInfor> GetMyApprovalOutTimePagingLeave(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by a.OutTime desc ")
        {
            using (var connection = OpenConnection())
            {
                totalCount = connection.Query<int>("select count(1) from Cl_TimeOutLeave as a,Sys_User as b,Co_Course as c where a.CourseId=c.Id and a.UserId=b.UserId " + where).First();
                var query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,a.Id as ID,b.Realname as RealName,a.OutTime as SubmitTime,c.Code as CourseCode,c.CourseName as CourseName,c.CourseLength as CourseLength,a.ApprovalFlag  as ApprovalFlag,a.ApprovalDateTime as ApprovalTime,4 as TypeFlag 
from  Cl_TimeOutLeave as a,Sys_User as b,Co_Course as c 
    where a.CourseId=c.Id and a.UserId=b.UserId and a.TrainAppFlag=1 " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<BaseApprovalInfor>(query, parameters).ToList();
            }
        }
        #endregion
    }
}
