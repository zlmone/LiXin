using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Core;
using Retech.Data;
using LiXinModels.CourseLearn;
using LiXinModels.User;

namespace LiXinDataAccess
{
    public class CourseOrderDB : BaseRepository
    {
        /// <summary>
        ///     增加一条数据
        /// </summary>
        /// <param name="model"></param>
        public void Add(Cl_CourseOrder model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Cl_CourseOrder( CourseId,UserId,OrderTime,OrderStatus,IsLeave,ApprovalUser,ApprovalMemo,ApprovalFlag,ApprovalDateTime,ApprovalLimitTime,OrderTimes,LearnStatus,GetScore,PassStatus,IsAppoint,OrderEndTime,CourseStartTime,CourseEndTime,CourseName,FtriggerFlag,AttScore,AttFlag) values (@CourseId,@UserId,@OrderTime,@OrderStatus,@IsLeave,@ApprovalUser,@ApprovalMemo,@ApprovalFlag,@ApprovalDateTime,@ApprovalLimitTime,@OrderTimes,@LearnStatus,@GetScore,@PassStatus,@IsAppoint,@OrderEndTime,@CourseStartTime,@CourseEndTime,@CourseName,@FtriggerFlag,@AttScore,@AttFlag)";
                var param = new
                    {
                        model.CourseId,
                        model.UserId,
                        model.OrderTime,
                        model.OrderStatus,
                        model.IsLeave,
                        model.ApprovalUser,
                        model.ApprovalMemo,
                        model.ApprovalFlag,
                        model.ApprovalDateTime,
                        model.ApprovalLimitTime,
                        model.OrderTimes,
                        model.LearnStatus,
                        model.GetScore,
                        model.PassStatus,
                        model.IsAppoint,
                        model.OrderEndTime,
                        model.CourseStartTime,
                        model.CourseEndTime,
                        model.CourseName,
                        model.FtriggerFlag,
                        model.AttScore,
                        model.AttFlag
                    };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.UserId = decimal.ToInt32(id);
            }
        }

        /// <summary>
        ///     更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool Update(Cl_CourseOrder model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @"Update  Cl_CourseOrder set CourseId=@CourseId,UserId=@UserId,OrderTime=@OrderTime,OrderStatus=@OrderStatus,IsLeave=@IsLeave,ApprovalUser=@ApprovalUser,ApprovalMemo=@ApprovalMemo,ApprovalFlag=@ApprovalFlag,ApprovalDateTime=@ApprovalDateTime,ApprovalLimitTime=@ApprovalLimitTime,OrderTimes=@OrderTimes,LearnStatus=@LearnStatus,GetScore=@GetScore,PassStatus=@PassStatus,OrderEndTime=@OrderEndTime,IsAppoint=@IsAppoint,CourseStartTime=@CourseStartTime,CourseEndTime=@CourseEndTime,CourseName=@CourseName,FtriggerFlag=@FtriggerFlag,requestid=@requestid,DropType=@DropType,DropReason=@DropReason,AttScore=@AttScore,AttFlag=@AttFlag where Id=@Id";
                var param = new
                    {
                        model.Id,
                        model.CourseId,
                        model.UserId,
                        model.OrderTime,
                        model.OrderStatus,
                        model.IsLeave,
                        model.ApprovalUser,
                        model.ApprovalMemo,
                        model.ApprovalFlag,
                        model.ApprovalDateTime,
                        model.ApprovalLimitTime,
                        model.OrderTimes,
                        model.LearnStatus,
                        model.GetScore,
                        model.PassStatus,
                        model.OrderEndTime,
                        model.IsAppoint,
                        model.CourseStartTime,
                        model.CourseEndTime,
                        model.CourseName,
                        model.FtriggerFlag,
                        model.requestid,
                        model.DropType,
                        model.DropReason,
                        model.AttScore,
                        model.AttFlag
                    };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        public bool Delete(int id)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"delete Cl_CourseOrder where id = {0}", id);
                int result = conn.Execute(sqlwhere);
                return result > 0;
            }
        }

        /// <summary>
        ///     根据ID获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Cl_CourseOrder Get(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = "select * from Cl_CourseOrder where id=@id";
                return connection.Query<Cl_CourseOrder>(sqlstr, new { Id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cl_TimeOutOrder GetOneTimeOutOrder(int id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = "select * from Cl_TimeOutOrder where id=@id";
                return connection.Query<Cl_TimeOutOrder>(sqlstr, new { id }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void UpdateTimeOutOrderStatus(Cl_TimeOutOrder model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"UPDATE dbo.Cl_TimeOutOrder
                                   SET ApprovalMemo	= @ApprovalMemo		
                                      ,ApprovalFlag	= @ApprovalFlag		
                                      ,ApprovalDateTime	= @ApprovalDateTime	
                                 WHERE id =@Id";
                var param = new
                    {
                        model.Id,
                        model.ApprovalFlag,
                        model.ApprovalMemo,
                        model.ApprovalDateTime
                    };
                conn.Execute(sqlwhere, param);
            }
        }

        /// <summary>
        /// 根据条件获取
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Cl_CourseOrder Getmodel(string where)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = "select * from Cl_CourseOrder ";
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
                return connection.Query<Cl_CourseOrder>(sqlstr).FirstOrDefault();
            }
        }
        /// <summary>
        /// 考勤管理（专用）
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Cl_CourseOrder GetmodelbyAtt(string where)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = "SELECT cco.*,cto.ApprovalFlag AS TimeOutLeaveApprovalFlag FROM Cl_CourseOrder AS cco LEFT JOIN Cl_TimeOutLeave AS cto ON(cco.CourseId=cto.CourseId AND cco.UserId=cto.UserId) ";
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
                return connection.Query<Cl_CourseOrder>(sqlstr).FirstOrDefault();
            }
        }
        /// <summary>
        ///     获得数据列表
        /// </summary>
        /// <param name="strWhere">获取数据的Sql Where条件（不带WHERE）</param>
        public List<Cl_CourseOrder> GetList(string strWhere = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = "select * from Cl_CourseOrder where " + strWhere +
                                  " order by Cl_CourseOrder.courseid,Cl_CourseOrder.approvaldatetime";
                return conn.Query<Cl_CourseOrder>(sqlwhere).ToList();
            }
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
        /// <param name="sqlstr">sql语句</param>
        public List<Cl_CourseOrder> GetAllList(string sqlstr = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                return conn.Query<Cl_CourseOrder>(sqlstr).ToList();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                            int pageLength = int.MaxValue, string orderBy = "ORDER BY id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Cl_CourseOrder where " + where)
                              .First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,Cl_CourseOrder.* from Cl_CourseOrder
    where " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<Cl_CourseOrder>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 请假
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateLeave(int id, string memo, string leaderID, double timespan, string hrId, string name)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"Update  Cl_CourseOrder 
                                    set IsLeave=1,Reson=@Reson,ApprovalUser=@ApprovalUser,ApprovalMemo='',ApprovalFlag=0
                                    ,ApprovalDateTime=getdate(),LeaveTime=getdate(),ApprovalLimitTime=@ApprovalLimitTime 
                                    ,LeaveUserID=@LeaveUserID,Name=@Name
                                    where Id = " + id;
                var param = new
                    {
                        Reson = memo,
                        ApprovalLimitTime = DateTime.Now.AddHours(timespan),
                        ApprovalUser = leaderID,
                        Name = name,
                        LeaveUserID = hrId
                    };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 退订
        /// 0：退订成功（根本没有预订）
        /// 1：退订成功
        /// 2：退订失败，次数超了
        /// 3：退订失败
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int UpdateStatus(int id, int userId, DateTime start, DateTime end, int unsubscribeCount)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"select 
                                (select count(0)+1 from cl_courseorder col 
                                left join sys_user on sys_user.userid = col.userid
                                where col.[OrderStatus]= 1 
                                and ( col.[IsLeave] = 0 or (col.[IsLeave] = 1 and col.[ApprovalFlag] <>1))
                                and  col.ordertime < cl_courseorder.ordertime and col.courseID= co_course.id
                                and sys_user.IsDelete = 0
                                ) as MyLocation
                                ,co_course.*,cl_courseorder.Userid
                                from co_course
                                left join cl_courseorder on co_course.id=cl_courseorder.courseid
                                where cl_courseorder.id = {0}", id);
                var model = conn.Query<LiXinModels.CourseManage.Co_Course>(sqlwhere).FirstOrDefault();
                if (model == null)
                    return 0;
                if (model.MyLocation <= model.NumberLimited)
                {
                    string sqlstr = @"select isnull(sum(OrderTimes),0) from Cl_CourseOrder
                                    left join Co_Course on Co_Course.id=Cl_CourseOrder.courseid
                                    where Co_Course.starttime > @start
                                    and Co_Course.endtime < @end 
                                    and Co_Course.IsDelete = 0 
                                    and Co_Course.Publishflag = 1
                                    and Cl_CourseOrder.UserId = @userId";
                    var cou = conn.Query<int>(sqlstr, new { userId, start, end }).FirstOrDefault();
                    if (cou >= unsubscribeCount)
                        return 2;
                }
                sqlwhere =
                    string.Format(
                        @"Update Cl_CourseOrder set OrderStatus=0,ApprovalDateTime=getdate(),IsLeave = 0,ApprovalLimitTime='2000-01-01',LeaveTime='2000-01-01',Reson='',Name='',LeaveUserID='' {1} where Id = {0}",
                        id, model.MyLocation <= model.NumberLimited ? ",OrderTimes=OrderTimes+1" : "");
                int result = conn.Execute(sqlwhere);
                return result > 0 ? 1 : 3;
            }
        }

        /// <summary>
        /// 退订
        /// 0：退订成功（根本没有预订）
        /// 1：退订成功
        /// 2：退订失败，次数超了
        /// 3：退订失败
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int UpdateStatus(out int num, int id, int userId, int year, int unsubscribeCount)
        {
            using (IDbConnection conn = OpenConnection())
            {
                num = -1;
                string sqlwhere = string.Format(@"select 
                               (
                                    select count(0)+1 from cl_courseorder col 
                                    left join sys_user on sys_user.userid = col.userid
                                    where col.[OrderStatus]= 1 
                                    and ( col.[IsLeave] = 0 or (col.[IsLeave] = 1 and col.[ApprovalFlag] <>1))
                                    and  col.ordertime < cl_courseorder.ordertime and col.courseID= co_course.id
                                    and sys_user.isdelete = 0
                                ) as MyLocation
                                ,co_course.*,cl_courseorder.Userid,cl_courseorder.OrderStatus
                                from co_course
                                left join cl_courseorder on co_course.id=cl_courseorder.courseid
                                where cl_courseorder.id = {0}", id);
                var model = conn.Query<LiXinModels.CourseManage.Co_Course>(sqlwhere).FirstOrDefault();
                if (model == null)
                    return 0;
                //排队中的，不看已退订次数
                //if (model.MyLocation <= model.NumberLimited)
                if (model.OrderStatus == 1)
                {
                    string sqlstr = @"select isnull(sum(OrderTimes),0) from Cl_CourseOrder
                                    left join Co_Course on Co_Course.id=Cl_CourseOrder.courseid
                                    where Co_Course.YearPlan =  @year
                                    and Co_Course.IsDelete = 0 
                                    and Co_Course.Publishflag = 1
                                    and Cl_CourseOrder.UserId = @userId";
                    var cou = conn.Query<int>(sqlstr, new { userId, year }).FirstOrDefault();
                    if (cou >= unsubscribeCount)
                        return 2;
                    num = unsubscribeCount - cou;
                    return 1;
                }
                //sqlwhere =
                //    string.Format(
                //        @"Update Cl_CourseOrder set OrderStatus=0,ApprovalDateTime=getdate()
                //            ,IsLeave = 0,ApprovalLimitTime='2000-01-01',LeaveTime='2000-01-01'
                //            ,Reson='',Name='',LeaveUserID='' {1} where Id = {0}",
                //        id, model.MyLocation <= model.NumberLimited ? ",OrderTimes=OrderTimes+1" : "");
                sqlwhere =
                    string.Format(
                        @"Update Cl_CourseOrder set OrderStatus=0,ApprovalDateTime=getdate()
                            ,IsLeave = 0,ApprovalLimitTime='2000-01-01',LeaveTime='2000-01-01'
                            ,Reson='',Name='',LeaveUserID='' {1} where Id = {0}",
                        id, model.OrderStatus == 1 ? ",OrderTimes=OrderTimes+1" : "");
                int result = conn.Execute(sqlwhere);
                if (result > 0)
                    num = num == -1 ? -1 : num - 1;
                return result > 0 ? 1 : 3;
            }
        }

        /// <summary>
        /// 0:不可报名
        /// 1:可直接报名
        /// 2:可报名，需排队 
        /// 4:不可报名，排队已关闭
        /// 5:不可报名，报名已关闭
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public int GetCanSignup(out int num, int courseid, double timespan)
        {
            using (IDbConnection conn = OpenConnection())
            {
                num = 0;
                string sqlwhere = string.Format(@"select 
                            *
                            ,(
                                select count(0) from Cl_CourseOrder 
                                left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                                where Cl_CourseOrder.courseid = Co_Course.id 
                                and Cl_CourseOrder.[OrderStatus] = 1
                                and ( Cl_CourseOrder.[IsLeave] = 0 
                                    or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                                    or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                                        and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime)
                                )
                                and Sys_user.IsDelete = 0
                                --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                            ) as HasEntered  
                            ,(
                                select count(0) from Cl_CourseOrder 
                                left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                                where Cl_CourseOrder.courseid = Co_Course.id 
                                and Cl_CourseOrder.[OrderStatus] = 2
                                and Sys_user.IsDelete = 0
                                and Cl_CourseOrder.OrderTime<=Cl_CourseOrder.OrderEndTime
                                --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                            ) as DeptHasEntered --这边作为排队人数使用  
                            ,(
                                select count(0) from Cl_CourseOrder 
                                left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                                where Cl_CourseOrder.courseid = Co_Course.id 
                                and Cl_CourseOrder.[OrderStatus] = 2
                                and Sys_user.IsDelete = 0
                                and Cl_CourseOrder.OrderTime>Cl_CourseOrder.OrderEndTime
                                --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                            ) as SuccessEntered --这边作为排队人数使用  
                            from Co_Course 
                            where Id = {0} and IsDelete = 0 
                            and (Co_Course.IsOrder = 1 or Co_Course.IsOrder = 3 ) 
                            and Co_Course.Publishflag = 1 and Co_Course.startTime >= '{1}'", courseid, DateTime.Now);
                var model = conn.Query<LiXinModels.CourseManage.Co_Course>(sqlwhere).FirstOrDefault();
                if (model == null)
                    return 0; //不可报名
                if (model.StopOrderFlag == 1)
                    return 5; //不可报名，报名已关闭
                //DeptHasEntered:这边作为排队时间更改前处于排队状态的人数
                //SuccessEntered:这边作为排队时间更改后处于排队状态的人数
                num = model.DeptHasEntered + model.SuccessEntered;
                if (model.NumberLimited > model.HasEntered)
                {
                    if (model.StartTime.AddHours(timespan * -1) > DateTime.Now)
                    {
                        if (model.DeptHasEntered > 0)
                        {
                            if (model.StopDucueFlag == 0)
                                return 2; //可报名，需排队 
                            else
                                return 4;//不可报名，排队已关闭
                        }
                    }
                    else
                    {
                        if (model.SuccessEntered <= 0 && model.NumberLimited <= (model.HasEntered + model.DeptHasEntered))
                        {
                            if (model.StopDucueFlag == 0)
                                return 2; //可报名，需排队 
                            else
                                return 4; //不可报名，排队已关闭
                        }
                    }
                    return 1;//可直接报名
                }
                else
                {
                    if (model.Way == 3)
                        return 4;
                    if (model.StopDucueFlag == 0)
                        return 2; //可报名，需排队 
                    else
                        return 4; //不可报名，排队已关闭
                }
            }
        }

        /// <summary>
        /// 0:不可报名
        /// 1:可直接报名
        /// 2:可报名，需排队 
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public int GetCanSignupSpecialCrowdUserToCourse(out int num, int courseid, double timespan)
        {
            using (IDbConnection conn = OpenConnection())
            {
                num = 0;
                string sqlwhere = string.Format(@"select 
                            *
                            ,(
                                select count(0) from Cl_CourseOrder 
                                left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                                where Cl_CourseOrder.courseid = Co_Course.id 
                                and Cl_CourseOrder.[OrderStatus] = 1
                                and ( Cl_CourseOrder.[IsLeave] = 0 
                                    or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                                    or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                                        and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime)
                                )
                                and Sys_user.IsDelete = 0
                                --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                            ) as HasEntered  
                            ,(
                                select count(0) from Cl_CourseOrder 
                                left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                                where Cl_CourseOrder.courseid = Co_Course.id 
                                and Cl_CourseOrder.[OrderStatus] = 2
                                and Sys_user.IsDelete = 0
                                and Cl_CourseOrder.OrderTime<=Cl_CourseOrder.OrderEndTime
                                --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                            ) as DeptHasEntered --这边作为排队人数使用   
                            ,(
                                select count(0) from Cl_CourseOrder 
                                left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                                where Cl_CourseOrder.courseid = Co_Course.id 
                                and Cl_CourseOrder.[OrderStatus] = 2
                                and Sys_user.IsDelete = 0
                                and Cl_CourseOrder.OrderTime>Cl_CourseOrder.OrderEndTime
                                --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                            ) as SuccessEntered --这边作为排队人数使用   
                            from Co_Course 
                            where Id = {0} and IsDelete = 0 
                            and Co_Course.Publishflag = 1 and Co_Course.startTime >= '{1}'", courseid, DateTime.Now);
                var model = conn.Query<LiXinModels.CourseManage.Co_Course>(sqlwhere).FirstOrDefault();
                if (model == null)
                    return 0; //不可报名
                //DeptHasEntered:这边作为排队时间更改前处于排队状态的人数
                //SuccessEntered:这边作为排队时间更改后处于排队状态的人数
                if (model.NumberLimited > model.HasEntered)
                {
                    if (model.StartTime.AddHours(timespan * -1) > DateTime.Now)
                    {
                        if (model.DeptHasEntered > 0)
                            return 2; //可报名，需排队 
                    }
                    else
                    {
                        if (model.SuccessEntered <= 0 && model.NumberLimited <= (model.HasEntered + model.DeptHasEntered))
                            return 2; //可报名，需排队 
                    }
                    return 1;//可直接报名
                }
                else
                    return 2; //可报名，需排队 
            }
        }

        /// <summary>
        /// 0:不可报名
        /// 1:可直接报名
        /// 2:不可报名，人数已满 
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public int GetCanSignupCPA(int courseid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"select *,(select count(0) from Cl_CpaLearnStatus 
                            left join Sys_user on  Cl_CpaLearnStatus.UserId = Sys_user.userid 
                            where Cl_CpaLearnStatus.courseid = Co_Course.id 
                            and Sys_user.IsDelete = 0
                            --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                            ) as HasEntered  
                            from Co_Course 
                            where Id = {0} and IsDelete = 0 
                            and StopOrderFlag = 0 and Co_Course.Publishflag = 1 and Co_Course.startTime >= '{1}'",
                                                courseid, DateTime.Now);
                var model = conn.Query<LiXinModels.CourseManage.Co_Course>(sqlwhere).FirstOrDefault();
                if (model == null)
                    return 0; //不可报名
                if (model.NumberLimited > model.HasEntered)
                    return 1; //可直接报名
                else
                    return 2; //不可报名，人数已满
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="applymemo"></param>
        /// <returns></returns>
        public int SubmitTimeOutApply(string ids, string applymemo)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"Update  Cl_MakeUpOrder 
                                    set IsTimeOut=1,TimeOutMemo=@Reson,TimeOutPassFlag=0,TimeOutDateTime=getdate(),TimeOutUserID=ApprovalUser
                                    where Id in (" + ids + ")";
                var param = new
                    {
                        Reson = applymemo
                    };
                return conn.Execute(sqlwhere, param);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="approvalmemo"></param>
        /// <param name="approval"></param>
        /// <returns></returns>
        public int SubmitTimeOutApproval(string ids, string approvalmemo, int approval)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"Update  Cl_MakeUpOrder 
                                    set TimeOutPassFlag=@approval,TimeOutApprovalDateTime=getdate(),TimeOutApprovalMemo=@Reson
                                    where Id in (" + ids + ")";
                if (approval == 1)
                {
                    sqlwhere +=
                        @"insert into Cl_TimeOutOrder( [CourseId],[UserId],[ApprovalUser],[OutUserID],[Name],[MakeUpTime],[CourseName],[OutTime],[CourseStartTime],[CourseEndTime],[AttStartTime],[AttEndTime],[ApprovalFlag]) 
                   select CourseId,UserId,ApprovalUser,LeaveUserID,Name,LeaveTime as MakeUpTime,CourseName,TimeOutDateTime as OutTime,CourseStartTime,CourseEndTime,AttStartTime,AttEndTime,0 from dbo.Cl_MakeUpOrder where id in (" +
                        ids + ") ";
                }
                var param = new
                    {
                        Reson = approvalmemo,
                        approval = approval
                    };
                return conn.Execute(sqlwhere, param);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cl_CourseOrder GetTimeOutApplyDetail(int id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string query = string.Format(@"
select  Sys_User.RealName
,Sys_User.TrainGrade
,Co_Course.CourseName as course_Name
,Co_Course.Code as Course_Code
,Co_Course.CourseLength
,Co_Course.starttime
,Co_Course.endtime
,Sys_Teacher.RealName as TeacherName
,Cl_MakeUpOrder.LeaveTime as TimeOutApplyTime
,Cl_MakeUpOrder.*
from dbo.Cl_MakeUpOrder
left join dbo.Co_Course on Cl_MakeUpOrder.courseid= Co_Course.id
left join dbo.Cl_CourseOrder on Cl_CourseOrder.courseId = Cl_MakeUpOrder.courseId and Cl_CourseOrder.UserId = Cl_MakeUpOrder.UserId
left join dbo.Sys_User on Sys_User.UserId = Cl_MakeUpOrder.UserId
left join dbo.Sys_User Sys_Teacher on Sys_Teacher.Userid = Co_Course.teacher 
    where Cl_MakeUpOrder.Id = {0}", id);
                return connection.Query<Cl_CourseOrder>(query).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取我的请假列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetMyLeaveList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                                   int pageLength = int.MaxValue,
                                                   string orderBy = "ORDER BY Cl_CourseOrder.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>(@"select count(0)
                                    from dbo.Cl_CourseOrder
                                    left join dbo.Co_Course on Cl_CourseOrder.courseid= Co_Course.id
                                    left join dbo.Sys_User on Sys_User.JobNum = Cl_CourseOrder.ApprovalUser
                                    left join dbo.Sys_User Sys_Teacher on Sys_Teacher.Userid = Co_Course.teacher 
                                    where " + where).First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum
,Co_Course.CourseName as course_Name
,Co_Course.Sort
,Co_Course.CourseLength
,Co_Course.IsMust
,Co_Course.starttime
,Co_Course.endtime
,Sys_ApprovalUser.realName as approvalrealNAME
,Sys_User.realName 
,Sys_Teacher.realname as teacherName
--,tt.CourseTimesOrder
--,tt.TotalCourseTimes
,Cl_CourseOrder.* 
from dbo.Cl_CourseOrder
left join dbo.Co_Course on Cl_CourseOrder.courseid= Co_Course.id
left join dbo.Sys_User Sys_ApprovalUser on Sys_ApprovalUser.JobNum = Cl_CourseOrder.ApprovalUser
left join dbo.Sys_User on Sys_User.userid = Cl_CourseOrder.userid
left join dbo.Sys_User Sys_Teacher on Sys_Teacher.Userid = Co_Course.teacher
--left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
--			count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
--    from Co_Course cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
--    on tt.id = Co_Course.id and tt.code = Co_Course.code
    where " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<Cl_CourseOrder>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 获取我的补预订列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetMyComplementResList(out int totalCount, string where = " 1 = 1 ",
                                                           int startIndex = 0,
                                                           int pageLength = int.MaxValue,
                                                           string orderBy = "ORDER BY Cl_MakeUpOrder.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>(@"select count(0)
                                    from dbo.Cl_MakeUpOrder
                                    left join dbo.Co_Course on Cl_MakeUpOrder.courseid= Co_Course.id
                                    where " + where).First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum
,dbo.F_IsAppStatus(Co_Course.Id,Cl_CourseOrder.UserId) as GetScoreFlag
,Cl_CourseOrder.GetScore
,Cl_CourseOrder.OrderStatus
,Cl_CourseOrder.MakeUpApprovalFlag
,Co_Course.CourseName as course_Name
,Co_Course.Sort
,Co_Course.CourseLength
,Co_Course.IsMust
,Co_Course.starttime
,Co_Course.endtime
,Sys_User.realName
,Sys_Teacher.realname as teacherName
--,tt.CourseTimesOrder
--,tt.TotalCourseTimes
,Cl_MakeUpOrder.*
from dbo.Cl_MakeUpOrder
left join dbo.Co_Course on Cl_MakeUpOrder.courseid= Co_Course.id
left join dbo.Cl_CourseOrder on Cl_CourseOrder.courseId = Cl_MakeUpOrder.courseId and Cl_CourseOrder.UserId = Cl_MakeUpOrder.UserId
left join dbo.Sys_User on Sys_User.JobNum = Cl_MakeUpOrder.ApprovalUser
left join dbo.Sys_User Sys_Teacher on Sys_Teacher.Userid = Co_Course.teacher
--left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
--			count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
--		from Co_Course  cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
--        on tt.id = Co_Course.id and tt.code = Co_Course.code
    where " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<Cl_CourseOrder>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 获取我的逾时申请列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetMyTimeOutList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                                     int pageLength = int.MaxValue,
                                                     string orderBy = "ORDER BY Cl_TimeOutOrder.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>(@"select count(0)
                                    from dbo.Cl_TimeOutOrder
                                    left join dbo.Co_Course on Cl_TimeOutOrder.courseid= Co_Course.id
                                    where " + where).First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum
,case when (dbo.F_IsAppStatus(Cl_CourseOrder.CourseId,Cl_CourseOrder.UserId)=1) then Cl_CourseOrder.GetScore else 0 end GetScore
,Cl_CourseOrder.OrderStatus
,Cl_CourseOrder.MakeUpApprovalFlag
,Co_Course.CourseName as course_Name
,Co_Course.CourseLength
,Co_Course.IsMust
,Co_Course.starttime
,Co_Course.endtime
,Sys_User.realName
,Sys_Teacher.realname as teacherName
,tt.CourseTimesOrder
,tt.TotalCourseTimes
,Cl_TimeOutOrder.*
from dbo.Cl_TimeOutOrder
left join dbo.Co_Course on Cl_TimeOutOrder.courseid= Co_Course.id
left join dbo.Cl_CourseOrder on Cl_CourseOrder.courseId = Cl_TimeOutOrder.courseId and Cl_CourseOrder.UserId = Cl_TimeOutOrder.UserId
left join dbo.Sys_User on Sys_User.JobNum = Cl_TimeOutOrder.ApprovalUser
left join dbo.Sys_User Sys_Teacher on Sys_Teacher.Userid = Co_Course.teacher
left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
		from Co_Course  cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
        on tt.id = Co_Course.id and tt.code = Co_Course.code
    where " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<Cl_CourseOrder>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 获取我的逾时申请列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetMyTimeOutListLeave(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                                     int pageLength = int.MaxValue,
                                                     string orderBy = "ORDER BY Cl_TimeOutOrder.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>(@"select count(0)
                                    from dbo.Cl_TimeOutLeave
                                    left join dbo.Co_Course on Cl_TimeOutLeave.courseid= Co_Course.id
                                    where " + where).First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum
,Cl_CourseOrder.GetScore
,Cl_CourseOrder.OrderStatus
,Cl_CourseOrder.MakeUpApprovalFlag
,Co_Course.CourseName as course_Name
,Co_Course.CourseLength
,Co_Course.IsMust
,Co_Course.starttime
,Co_Course.endtime
,Sys_User.realName
,Sys_Teacher.realname as teacherName
,tt.CourseTimesOrder
,tt.TotalCourseTimes
,Cl_TimeOutLeave.*
from dbo.Cl_TimeOutLeave
left join dbo.Co_Course on Cl_TimeOutLeave.courseid= Co_Course.id
left join dbo.Cl_CourseOrder on Cl_CourseOrder.courseId = Cl_TimeOutLeave.courseId and Cl_CourseOrder.UserId = Cl_TimeOutLeave.UserId
left join dbo.Sys_User on Sys_User.JobNum = Cl_TimeOutLeave.ApprovalUser
left join dbo.Sys_User Sys_Teacher on Sys_Teacher.Userid = Co_Course.teacher
left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
		from Co_Course  cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
        on tt.id = Co_Course.id and tt.code = Co_Course.code 
    where  Cl_TimeOutLeave.TrainAppFlag=1 And " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Cl_CourseOrder>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 逾时审核
        /// </summary>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetTimeOutApplyApprovalList(out int totalCount, string where = " 1 = 1 ",
                                                                int startIndex = 0,
                                                                int pageLength = int.MaxValue,
                                                                string orderBy = "ORDER BY Cl_TimeOutOrder.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>(@"select count(0)
                                    from dbo.Cl_TimeOutOrder
                                    left join dbo.Co_Course on Cl_TimeOutOrder.courseid= Co_Course.id
left join dbo.Cl_CourseOrder on Cl_CourseOrder.courseId = Cl_TimeOutOrder.courseId and Cl_CourseOrder.UserId = Cl_TimeOutOrder.UserId
left join dbo.Sys_User Sys_ApplyUser on Sys_ApplyUser.UserID = Cl_TimeOutOrder.UserId
                                    where " + where).First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum
,Cl_CourseOrder.GetScore
,Co_Course.CourseName as course_Name
,Co_Course.CourseLength
,Co_Course.IsMust
,Co_Course.starttime
,Co_Course.endtime
,Sys_User.realName
,Sys_ApplyUser.realName as ApplyUserRealName
,Cl_TimeOutOrder.*
from dbo.Cl_TimeOutOrder
left join dbo.Co_Course on Cl_TimeOutOrder.courseid= Co_Course.id
left join dbo.Cl_CourseOrder on Cl_CourseOrder.courseId = Cl_TimeOutOrder.courseId and Cl_CourseOrder.UserId = Cl_TimeOutOrder.UserId
left join dbo.Sys_User on Sys_User.JobNum = Cl_TimeOutOrder.ApprovalUser
left join dbo.Sys_User Sys_ApplyUser on Sys_ApplyUser.UserID = Cl_TimeOutOrder.UserId
    where " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<Cl_CourseOrder>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 逾时审核申请
        /// </summary>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetTimeOutApplyList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                                        int pageLength = int.MaxValue,
                                                        string orderBy = "ORDER BY Cl_MakeUpOrder.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>(@"select count(0)
                                    from dbo.Cl_MakeUpOrder
                                    left join dbo.Co_Course on Cl_MakeUpOrder.courseid= Co_Course.id
                                    left join dbo.Sys_User on Sys_User.UserId = Cl_MakeUpOrder.UserId
                                    where " + where).First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum
,Sys_User.RealName
,Sys_User.TrainGrade
,Co_Course.CourseName as course_Name
,Co_Course.CourseLength
,Co_Course.starttime
,Co_Course.endtime
,Cl_MakeUpOrder.LeaveTime as TimeOutApplyTime
,Cl_MakeUpOrder.*
from dbo.Cl_MakeUpOrder
left join dbo.Co_Course on Cl_MakeUpOrder.courseid= Co_Course.id
left join dbo.Cl_CourseOrder on Cl_CourseOrder.courseId = Cl_MakeUpOrder.courseId and Cl_CourseOrder.UserId = Cl_MakeUpOrder.UserId
left join dbo.Sys_User on Sys_User.UserId = Cl_MakeUpOrder.UserId
    where " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<Cl_CourseOrder>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 逾时申请审核
        /// </summary>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetTimeOutApprovalList(out int totalCount, string where = " 1 = 1 ",
                                                           int startIndex = 0,
                                                           int pageLength = int.MaxValue,
                                                           string orderBy = "ORDER BY Cl_MakeUpOrder.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>(@"select count(0)
                                    from dbo.Cl_MakeUpOrder
left join dbo.Co_Course on Cl_MakeUpOrder.courseid= Co_Course.id
left join dbo.Cl_CourseOrder on Cl_CourseOrder.courseId = Cl_MakeUpOrder.courseId and Cl_CourseOrder.UserId = Cl_MakeUpOrder.UserId
left join dbo.Sys_User on Sys_User.UserId = Cl_MakeUpOrder.UserId
left join dbo.Sys_User Sys_ApplyUser on Sys_ApplyUser.jobnum = Cl_MakeUpOrder.TimeOutUserID
                                    where " + where).First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum
,Sys_User.RealName
,Sys_User.TrainGrade
,Co_Course.CourseName as course_Name
,Co_Course.CourseLength
,Co_Course.starttime
,Co_Course.endtime
,Cl_MakeUpOrder.LeaveTime as TimeOutApplyTime
,Sys_ApplyUser.Realname as ApplyUserRealName
,Sys_ApplyUser.DeptName as ApplyUserDeptName
,Cl_MakeUpOrder.*
from dbo.Cl_MakeUpOrder
left join dbo.Co_Course on Cl_MakeUpOrder.courseid= Co_Course.id
left join dbo.Cl_CourseOrder on Cl_CourseOrder.courseId = Cl_MakeUpOrder.courseId and Cl_CourseOrder.UserId = Cl_MakeUpOrder.UserId
left join dbo.Sys_User on Sys_User.UserId = Cl_MakeUpOrder.UserId
left join dbo.Sys_User Sys_ApplyUser on Sys_ApplyUser.jobnum = Cl_MakeUpOrder.TimeOutUserID
    where " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<Cl_CourseOrder>(query, parameters).ToList();
            }
        }



        /// <summary>
        /// 待预订页面集中授课课程
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<LiXinModels.CourseManage.Co_Course> GetMyCourseList(out int totalCount, int userId,
                                                                        string where = " 1 = 1 ", string num = " 1=1", int startIndex = 0,
                                                                        int pageLength = int.MaxValue,
                                                                        string orderBy = "ORDER BY Co_Course.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>(
                        string.Format(
                            "select count(1) from Co_Course left join (select * from cl_courseorder where cl_courseorder.userid = {0} )  cl_courseorder on Co_Course.id=cl_courseorder.courseid where " +
                            where, userId))
                              .First();

                #region old

                //                string query = string.Format(@"
                //                select top {0} * from (
                //                    select  row_number() over( {1} ) as rowNum
                //                        ,(select count(0) from Cl_CourseOrder 
                //                            left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                //                            where Cl_CourseOrder.courseid = Co_Course.id and Cl_CourseOrder.[OrderStatus]= 1 
                //                            and ( Cl_CourseOrder.[IsLeave] = 0 
                //                                    or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                //                                    or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                //                                        and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])
                //                                )
                //                            and Sys_user.IsDelete = 0
                //                            --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                //                         ) as HasEntered
                //                        ,Co_Course.*
                //                        ,cl_courseorder.[Id] as courseOrderId
                //                        ,cl_courseorder.[UserId]
                //                        ,cl_courseorder.[OrderTime]
                //                        ,cl_courseorder.[OrderStatus] as MyStatus
                //                        ,cl_courseorder.[IsLeave] as MyLeave
                //                        ,cl_courseorder.[ApprovalFlag]
                //                        ,cl_courseorder.[ApprovalDateTime]
                //                        ,cl_courseorder.[ApprovalLimitTime]
                //                        ,cl_courseorder.OrderEndTime
                //                        ,cl_courseorder.IsAppoint
                //                        ,Sys_ClassRoom.RoomName as RoomName
                //                        ,Sys_User.RealName as TeacherName
                //                        ,(select count(0) from cl_courseorder col 
                //                                left join sys_user on sys_user.userid = col.userid
                //                                where col.OrderStatus= 1 
                //                                and ( col.IsLeave = 0 or (col.IsLeave = 1 and col.[ApprovalFlag] <>1))
                //                                and  col.ordertime < cl_courseorder.OrderTime and col.courseID= co_course.id 
                //                                and sys_user.isdelete = 0
                //                         )+1 as MyLocation
                //                        ,(select count(0) from cl_courseorder col 
                //                            left join sys_user on sys_user.userid = col.userid
                //                            where (col.OrderStatus = 1 or (col.OrderStatus = 0 and col.ApprovalDateTime > col.OrderEndTime ))
                //                            and (( col.IsLeave = 0 or (col.IsLeave = 1 and col.[ApprovalFlag] <>1)) 
                //                                or (col.IsLeave = 1 and col.[ApprovalFlag] =1  and col.ApprovalDateTime > col.OrderEndTime))
                //                            and  col.ordertime < cl_courseorder.OrderTime and col.courseID= co_course.id 
                //                            and sys_user.isdelete = 0
                //                        )+1 as MyOrderEndLocation
                //   from	Co_Course 
                //   left join (select * from cl_courseorder where cl_courseorder.userid = {2} )  cl_courseorder 
                //            on Co_Course.id=cl_courseorder.courseid 
                //   left join Sys_ClassRoom on Sys_ClassRoom.id = Co_Course.RoomId
                //   left join Sys_User on Sys_User.Userid = Co_Course.teacher
                //                        where " + where + @" 
                //                ) result 
                //                where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy, userId);

                #endregion

                string query = string.Format(@"
                   select top {0} * from (
                       select  row_number() over( {1} ) as rowNum
                           ,(select count(0) from Cl_CourseOrder 
                               left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                               where Cl_CourseOrder.courseid = Co_Course.id and Cl_CourseOrder.[OrderStatus] in (1,2,3) 
                               and ( Cl_CourseOrder.[IsLeave] = 0 
                                       or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                                       or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                                           and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])
                                   )
                               and Sys_user.IsDelete = 0
                            ) as HasEntered
                            ,(
                                select count(0) from Cl_CourseOrder 
                                left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                                where Cl_CourseOrder.courseid = Co_Course.id 
                                and Cl_CourseOrder.[OrderStatus] = 2
                                and Sys_user.IsDelete = 0
                                and Cl_CourseOrder.OrderTime<=Cl_CourseOrder.OrderEndTime
                                --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                            ) as DeptHasEntered --这边作为排队人数使用  
                            ,(
                                select count(0) from Cl_CourseOrder 
                                left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                                where Cl_CourseOrder.courseid = Co_Course.id 
                                and Cl_CourseOrder.[OrderStatus] = 2
                                and Sys_user.IsDelete = 0
                                and Cl_CourseOrder.OrderTime>Cl_CourseOrder.OrderEndTime
                                --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                            ) as SuccessEntered --这边作为排队人数使用 
                           ,Co_Course.*
                           ,sys_user.traingrade as traingrade
                           ,cl_courseorder.[Id] as courseOrderId
                           ,cl_courseorder.[UserId]
                           ,cl_courseorder.[OrderTime]
                           ,cl_courseorder.[OrderStatus] as MyStatus
                           ,cl_courseorder.[IsLeave] as MyLeave
                           ,cl_courseorder.[ApprovalFlag]
                           ,cl_courseorder.[ApprovalDateTime]
                           ,cl_courseorder.[ApprovalLimitTime]
                           ,cl_courseorder.OrderEndTime
                           ,cl_courseorder.IsAppoint
                           ,Sys_ClassRoom.RoomName as RoomName
                           ,Sys_User.RealName as TeacherName
                           ,tt.CourseTimesOrder
                           ,tt.TotalCourseTimes
                   from	Co_Course 
                   left join (select * from cl_courseorder where cl_courseorder.userid = {2} )  cl_courseorder 
                            on Co_Course.id=cl_courseorder.courseid 
                   left join Sys_ClassRoom on Sys_ClassRoom.id = Co_Course.RoomId
                   left join Sys_User on Sys_User.Userid = Co_Course.teacher
                   left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			                   count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
                       from Co_Course cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
                       on tt.id = Co_Course.id and tt.code = Co_Course.code
                                        where " + where + @" 
                                ) result 
                                where {3} and rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength,
                                             orderBy, userId, num);

                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<LiXinModels.CourseManage.Co_Course>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 待预订页面CPA课程
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<LiXinModels.CourseManage.Co_Course> GetMyCourseCPAList(out int totalCount, int userId,
                                                                           string where = " 1 = 1 ", int startIndex = 0,
                                                                           int pageLength = int.MaxValue,
                                                                           string orderBy = "ORDER BY Co_Course.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>(
                        string.Format(
                            "select count(1) from Co_Course left join (select * from Cl_CpaLearnStatus where Cl_CpaLearnStatus.userid = {0} )  Cl_CpaLearnStatus   on Co_Course.id=Cl_CpaLearnStatus.courseid  where " +
                            where, userId))
                              .First();
                string query = string.Format(@"
                select top {0} * from (
                    select  row_number() over( {1} ) as rowNum
                        ,isnull(Cl_CpaLearnStatus.id,0) as MyStatus
                        ,Co_Course.*
                    from	Co_Course 
                    left join (select * from Cl_CpaLearnStatus where Cl_CpaLearnStatus.userid = {2} )  Cl_CpaLearnStatus 
                            on Co_Course.id=Cl_CpaLearnStatus.courseid 
                    where " + where + @" 
                ) result 
                where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy,
                                             userId);

                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<LiXinModels.CourseManage.Co_Course>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 我的课程（学员）列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<LiXinModels.CourseManage.Co_Course> GetMyCourseListCourse(out int totalCount, int userId,
                                                                              string where = " 1 = 1 ",
                                                                              int startIndex = 0,
                                                                              int pageLength = int.MaxValue,
                                                                              string orderBy =
                                                                                  "ORDER BY Co_Course.id DESC")
        {

            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>(string.Format(@"
                SELECT count(1)  FROM
(
	SELECT 
        row_number() over( ORDER BY  IsAppoint desc,OrderTime desc) as rowNum
		,Cl_CourseOrder.ID
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
		,Sys_User.Realname as TeacherName
		,Sys_ClassRoom.RoomName as RoomName
		,Cl_CourseOrder.PassStatus as PassStatus
		,CASE WHEN (dbo.F_IsAppStatus(Co_Course.Id,{0})=1) then Cl_CourseOrder.GetScore  ELSE 0 END as GetScore
		,Cl_CourseOrder.CourseId as CourseId
		,Cl_CourseOrder.UserId
        --添加请假跟退订操作
        --start
        ,cl_courseorder.[Id] as courseOrderId
        ,cl_courseorder.[OrderTime]
        ,cl_courseorder.[OrderStatus] as MyStatus
        ,cl_courseorder.[IsLeave] as MyLeave
        ,cl_courseorder.[ApprovalFlag]
        ,cl_courseorder.[ApprovalDateTime]
        ,cl_courseorder.[ApprovalLimitTime]
        ,cl_courseorder.OrderEndTime
        ,cl_courseorder.IsAppoint
        ,Co_Course.YearPlan
        ,Co_Course.[IsLeave]
        --end
		,row_number() OVER(PARTITION BY Co_Course.Code ORDER BY StartTime asc) AS CourseTimesOrder
		,count(Code)over( PARTITION BY Co_Course.Code) AS TotalCourseTimes		    
	FROM Cl_CourseOrder left join Co_Course on Cl_CourseOrder.CourseId=Co_Course.ID
						left join Sys_User on Co_Course.Teacher=Sys_User.UserId
						left join Sys_ClassRoom on  Co_Course.RoomId=Sys_ClassRoom.id
	WHERE " + where + @" 
	and    Cl_CourseOrder.UserId={0} and Cl_CourseOrder.OrderStatus in(1,2,3)
) bb
", userId)).First();


                string query = string.Format(@"
select top({1}) * from(
                SELECT row_number() over( ORDER BY  IsAppoint desc,StartTime desc) as rowNum, * FROM
(
	SELECT 
		Cl_CourseOrder.ID
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
        ,Cl_CourseOrder.IsAppoint    
		,Sys_User.Realname as TeacherName
		,Sys_ClassRoom.RoomName as RoomName
		,Cl_CourseOrder.PassStatus as PassStatus
		,CASE WHEN (dbo.F_IsAppStatus(Co_Course.Id,Cl_CourseOrder.UserId)=1) THEN Cl_CourseOrder.GetScore ELSE 0 END  as GetScore
		,Cl_CourseOrder.CourseId as CourseId
        ,Cl_CourseOrder.OrderTime
        ,Cl_CourseOrder.OrderStatus
        ,Cl_CourseOrder.MakeUpApprovalFlag
		,Cl_CourseOrder.UserId
        --添加请假跟退订操作
        --start
        ,cl_courseorder.[Id] as courseOrderId
        --,cl_courseorder.[OrderTime]
        ,cl_courseorder.[OrderStatus] as MyStatus
        ,cl_courseorder.[IsLeave] as MyLeave
        ,cl_courseorder.[ApprovalFlag]
        ,cl_courseorder.[ApprovalDateTime]
        ,cl_courseorder.[ApprovalLimitTime]
        ,cl_courseorder.OrderEndTime
        --,cl_courseorder.IsAppoint
        ,Co_Course.YearPlan
        ,Co_Course.[IsLeave]
        --end
		--,row_number() OVER(PARTITION BY Co_Course.Code ORDER BY StartTime asc) AS CourseTimesOrder
		--,count(Code)over( PARTITION BY Co_Course.Code) AS TotalCourseTimes	
        ,tt.CourseTimesOrder
		,tt.TotalCourseTimes	    
	FROM Cl_CourseOrder left join Co_Course on Cl_CourseOrder.CourseId=Co_Course.ID
						left join Sys_User on Co_Course.Teacher=Sys_User.UserId
						left join Sys_ClassRoom on  Co_Course.RoomId=Sys_ClassRoom.id
                        left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime asc) as CourseTimesOrder,
			                   count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
                       from Co_Course cc where cc.isdelete = 0 and cc.coursefrom = 2 and cc.publishflag = 1) tt 
                       on tt.id = Co_Course.id and tt.code = Co_Course.code
	WHERE " + where + @"
	and Cl_CourseOrder.UserId={0} and Cl_CourseOrder.OrderStatus in(1,2,3)
) bb

)aa where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
--ORDER BY IsAppoint desc
--ORDER BY CourseId desc
", userId, pageLength);

                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<LiXinModels.CourseManage.Co_Course>(query, parameters).ToList();
            }

        }


        /// <summary>
        /// 我的课程表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<LiXinModels.CourseManage.Co_Course> GetMyCourseShedule(int userId, string where = " 1 = 1 ",
                                                                           string orderBy =
                                                                               " ORDER BY Co_Course.id DESC")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"SELECT a.ID
		,b.CourseName
		,b.StartTime
		,b.EndTime
		,a.CourseId as CourseId
		from Cl_CourseOrder as a,Co_Course as b
		where b.Way=1 and 
a.CourseID=b.Id and a.UserId={0} and 
b.IsDelete=0 and 
(
a.OrderStatus=3 or (a.OrderStatus=1 and a.IsLeave=0) or 
(a.OrderStatus=1 and a.IsLeave=1 and (a.ApprovalFlag=0 or a.ApprovalFlag=2)) or 
(a.OrderStatus=1 and a.IsLeave=1 and a.ApprovalFlag=1 and a.ApprovalDateTime > a.ApprovalLimitTime)
) and {1}", userId, where);
                return connection.Query<LiXinModels.CourseManage.Co_Course>(query).ToList();
            }

        }

        /// <summary>
        /// 查询课程的报名状态
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<LiXinModels.CourseManage.Co_Course> GetCourseSubscribeList(out int totalCount,
                                                                               string where = " 1 = 1 ",
                                                                               int startIndex = 0,
                                                                               int pageLength = int.MaxValue,
                                                                               string orderBy =
                                                                                   "ORDER BY Co_Course.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>(
                        string.Format(
                            "select count(1) from Co_Course left join Sys_User on Sys_User.Userid = Co_Course.teacher where " +
                            where))
                              .First();
                string query = string.Format(@"
                select top {0} * from (
                    select  row_number() over( {1} ) as rowNum,
                        (select count(0) from Cl_CourseOrder 
                            left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                            where Cl_CourseOrder.courseid = Co_Course.id and Cl_CourseOrder.[OrderStatus] in (1,2,3) 
                            and ( Cl_CourseOrder.[IsLeave] = 0 
                                    or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                                    or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                                        and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])
                                )
                            and Sys_user.isDelete = 0
                            --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) 
                        ) as HasEntered
                        ,(select count(0) from Cl_CourseOrder 
                            left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                            where Cl_CourseOrder.courseid = Co_Course.id and Cl_CourseOrder.[OrderStatus] =1
                            and ( Cl_CourseOrder.[IsLeave] = 0 
                                    or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                                    or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                                        and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])
                                )
                            and Sys_user.isDelete = 0
                            --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) 
                        ) as SuccessEntered
                        ,Co_Course.*
                        ,Sys_User.RealName as TeacherName
                from Co_Course
                left join Sys_User on Sys_User.Userid = Co_Course.teacher
                where " + where + @" 
                ) result 
                where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<LiXinModels.CourseManage.Co_Course>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 查询CPA课程的报名状态
        /// </summary>
        /// <returns></returns>
        public List<LiXinModels.CourseManage.Co_Course> GetCpaCourseSubscribeList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.id DESC")
        {
            using (var connection = OpenConnection())
            {
                totalCount = connection.Query<int>(string.Format("select count(1) from Co_Course where " + where)).First();
                var query = string.Format(@"
                select top {0} * from (
select row_number() over({1}) as rowNum,
Co_Course.Id,
Co_Course.CourseName,
Co_Course.OpenLevel,
Co_Course.CpaTeacher,
Co_Course.StartTime,
Co_Course.EndTime,
Co_Course.NumberLimited,
(SELECT count(*) FROM Cl_CpaLearnStatus WHERE Cl_CpaLearnStatus.CourseID=Co_Course.id) AS HasEntered
from Co_Course
where " + where + @" 
) temp
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<LiXinModels.CourseManage.Co_Course>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 查询课程的报名状态【部门负责人】
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="deptId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<LiXinModels.CourseManage.Co_Course> GetCourseDeptSubscribeList(out int totalCount, string leaderId,
                                                                                   string where = " 1 = 1 ",
                                                                                   int startIndex = 0,
                                                                                   int pageLength = int.MaxValue,
                                                                                   string orderBy =
                                                                                       "ORDER BY Co_Course.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>(
                        string.Format(
                            "select count(1) from Co_Course left join Sys_User on Sys_User.Userid = Co_Course.teacher where " +
                            where))
                              .First();
                string query = string.Format(@"
                select top {0} * from (
                    select  row_number() over( order by IsOrder desc,StartTime DESC ) as rowNum,
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
                            and Sys_user.isDelete = 0
                            and Sys_User.leaderId = '{2}'
                            --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) 
                        ) as DeptHasEntered,
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
                            and Sys_User.leaderId = '{2}'
                        ) as AssignUserCount
                        ,Co_Course.*
                        ,Sys_User.RealName as TeacherName
                from Co_Course
                left join Sys_User on Sys_User.Userid = Co_Course.teacher
                where " + where + @" 
                ) result 
                where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy,
                                             leaderId);
                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<LiXinModels.CourseManage.Co_Course>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 修改是否允许排队
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateDucueFlag(int id, int status)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"Update  Co_Course set StopDucueFlag=@DucueFlag where Id = " + id;
                var param = new
                    {
                        DucueFlag = status
                    };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 修改是否允许报名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateOrderFlag(int id, int status)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = @"Update Co_Course set StopOrderFlag=@OrderFlag where Id = " + id;
                var param = new
                    {
                        OrderFlag = status
                    };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 查询课程详情
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public LiXinModels.CourseManage.Co_Course GetCourseById(int courseId, int userId = 0)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = @"select (
                                     select count(0) from Cl_CourseOrder 
                                    left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                                    where Cl_CourseOrder.courseid = Co_Course.id 
                                    and Cl_CourseOrder.[OrderStatus] in (1,2,3)
                                    and ( Cl_CourseOrder.[IsLeave] = 0 
                                            or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                                            or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                                                and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])
                                        )
                                    and Sys_user.IsDelete = 0
                                    --and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                                ) as HasEntered,Co_Course.*,Sys_User.RealName as TeacherName 
                                from Co_Course 
                                left join Sys_User on Sys_User.UserId = Co_Course.Teacher 
                                where id=@id";
                if (userId != 0)
                {
                    #region old

                    //                    sqlstr = string.Format(@"select (select count(0) from Cl_CourseOrder 
                    //left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                    //where Cl_CourseOrder.courseid = Co_Course.id 
                    //and Cl_CourseOrder.[OrderStatus] in (1,2)
                    //and ( Cl_CourseOrder.[IsLeave] = 0 
                    //        or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                    //        or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                    //                and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])
                    //    ) 
                    //and Sys_user.IsDelete = 0
                    //--and  Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                    //) as HasEntered
                    //,Co_Course.*
                    //,Sys_User.RealName as TeacherName 
                    //,cl_courseorder.[Id] as courseOrderId
                    //,cl_courseorder.[UserId]
                    //,cl_courseorder.[OrderTime]
                    //,cl_courseorder.[OrderStatus] as MyStatus
                    //,cl_courseorder.[IsLeave] as MyLeave
                    //,cl_courseorder.[ApprovalFlag]
                    //,cl_courseorder.[ApprovalDateTime]
                    //,cl_courseorder.[ApprovalLimitTime]
                    //,cl_courseorder.OrderEndTime
                    //,cl_courseorder.IsAppoint
                    //,cl_courseorder.GetScore
                    //,(select count(0) from cl_courseorder col 
                    //left join sys_user on sys_user.userid = col.userid
                    //where col.OrderStatus= 1 
                    //        and ( col.IsLeave = 0 or (col.IsLeave = 1 and col.[ApprovalFlag] <>1))
                    //        and  col.ordertime < cl_courseorder.OrderTime and col.courseID= co_course.id 
                    //and sys_user.isdelete = 0
                    //)+1 as MyLocation
                    //,(select count(0) from cl_courseorder col 
                    //left join sys_user on sys_user.userid = col.userid
                    //where (col.OrderStatus = 1 or (col.OrderStatus = 0 and col.ApprovalDateTime > col.OrderEndTime ))
                    //and (( col.IsLeave = 0 or (col.IsLeave = 1 and col.[ApprovalFlag] <>1)) 
                    //    or (col.IsLeave = 1 and col.[ApprovalFlag] =1  and col.ApprovalDateTime > col.OrderEndTime))
                    //and  col.ordertime < cl_courseorder.OrderTime and col.courseID= co_course.id 
                    //and sys_user.isdelete = 0
                    //)+1 as MyOrderEndLocation
                    //,Sys_ClassRoom.RoomName
                    //from Co_Course 
                    //left join Sys_User on Sys_User.UserId = Co_Course.Teacher 
                    //left join cl_courseorder on cl_courseorder.courseid = Co_Course.id and cl_courseorder.userid ={0}
                    //left join Sys_ClassRoom on Sys_ClassRoom.id = Co_Course.RoomId
                    //where Co_Course.id=@id", userId);

                    #endregion

                    sqlstr = string.Format(@"select 
(
    select count(0) from Cl_CourseOrder 
    left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
    where Cl_CourseOrder.courseid = Co_Course.id 
    and Cl_CourseOrder.[OrderStatus] in (1,2,3)
    and ( Cl_CourseOrder.[IsLeave] = 0 
        or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
        or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])
    ) 
    and Sys_user.IsDelete = 0
) as HasEntered
,Co_Course.*
,Sys_User.RealName as TeacherName 
,cl_courseorder.[Id] as courseOrderId
,cl_courseorder.[UserId]
,cl_courseorder.[OrderTime]
,cl_courseorder.[OrderStatus] as MyStatus
,cl_courseorder.[IsLeave] as MyLeave
,cl_courseorder.[ApprovalFlag]
,cl_courseorder.[ApprovalDateTime]
,cl_courseorder.[ApprovalLimitTime]
,cl_courseorder.OrderEndTime
,cl_courseorder.IsAppoint
,cl_courseorder.GetScore
,Sys_ClassRoom.RoomName
from Co_Course 
left join Sys_User on Sys_User.UserId = Co_Course.Teacher 
left join cl_courseorder on cl_courseorder.courseid = Co_Course.id and cl_courseorder.userid ={0}
left join Sys_ClassRoom on Sys_ClassRoom.id = Co_Course.RoomId
where Co_Course.id=@id", userId);
                }
                return
                    connection.Query<LiXinModels.CourseManage.Co_Course>(sqlstr, new { id = courseId }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 查询部门的报名情况
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<LiXinModels.User.Sys_Department> GetDeptSubscribe(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = @"select Sys_Department.* 
,(select count(0) from Sys_User 
	where Sys_User.IsDelete = 0 
    and Sys_User.deptid = Sys_Department.departmentid
    and Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) 
    and (Sys_User.UserId in (select UserId from Sys_GroupUser where GroupId in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
         or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null )
    and Sys_User.[Status]=0
) as AllCount
,(select count(0) from Cl_CourseOrder 
    left join Sys_User on Cl_CourseOrder.userid = Sys_User.userid 
    where Sys_User.IsDelete = 0 and Sys_User.deptid =  Sys_Department.departmentid 
    and Cl_CourseOrder.courseid = Co_Course.id and Cl_CourseOrder.orderstatus in (1,2,3) 
    and (Cl_CourseOrder.IsLeave = 0 
        or (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1)
        or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime]))
    and Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) 
    and (Sys_User.UserId in (select UserId from Sys_GroupUser where GroupId in (select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)))
         or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null )
) as SubscribeCount
from Sys_Department 
left join Co_Course on (Sys_Department.departmentid in (
    select id from dbo.F_SplitIDs(Co_Course.OpenDepartObject)) 
    or Co_Course.OpenDepartObject = '' or Co_Course.OpenDepartObject is null)
where Sys_Department.IsDelete = 0 and Co_Course.id=  @courseId order by SubscribeCount desc";

                //                string sqlstr = @"select * from (
                //                                    select Sys_Department.* 
                //                                    ,(select count(0) from Sys_User where Sys_User.IsDelete = 0 
                //                                        and Sys_User.deptid = Sys_Department.departmentid
                //                                        and Sys_user.Traingrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) 
                //                                    ) as AllCount
                //                                    ,(select count(0) from Cl_CourseOrder 
                //                                        left join Sys_User on Cl_CourseOrder.userid = Sys_User.userid 
                //                                        where Sys_User.IsDelete = 0 and Sys_User.deptid =  Sys_Department.departmentid 
                //                                        and Cl_CourseOrder.courseid = Co_Course.id and Cl_CourseOrder.orderstatus = 1 
                //                                        and (Cl_CourseOrder.IsLeave = 0 or 
                //                                            (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1))
                //                                    ) as SubscribeCount
                //                                    ,Co_Course.OpenDepartObject
                //                                    from Sys_Department
                //                                    left join Co_Course on  Co_Course.id = @courseId
                //                                    where Sys_Department.IsDelete = 0 and Co_Course.id = @courseId
                //                                ) as tt
                //                                where (tt.departmentid in (select id from dbo.F_SplitIDs(tt.OpenDepartObject)) 
                //                                    or tt.OpenDepartObject = '' or tt.OpenDepartObject is null)
                //                                    or tt.SubscribeCount > 0";
                return connection.Query<LiXinModels.User.Sys_Department>(sqlstr, new { courseId }).ToList();
            }
        }

        /// <summary>
        /// 查询CPA课程培训级别下的报名情况
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<NameSubscribeCount> GetCpaTrainGradeSubscribe(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string sqlstr = @"select TrainGrade as Name,
       COUNT(1) AS AllCount, 
       isnull(TT.SubscribeCount,0) as SubscribeCount 
FROM sys_user su left join 
	(
	   SELECT TrainGrade as Name,COUNT(1) AS SubscribeCount 
	   FROM (
               select sys_user.TrainGrade
               from sys_user left join Cl_CpaLearnStatus on Cl_CpaLearnStatus.UserID=sys_user.userid
                             left join Co_Course on Co_Course.id = Cl_CpaLearnStatus.courseid
               where sys_user.IsDelete = 0 and Cl_CpaLearnStatus.courseid = @courseId
                     and (
							(
								select count(*) 
								from Sys_GroupUser 
								where UserId=sys_user.Userid and groupid in 
								     (
								         select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)
								      )
						     )>0 
						     or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null
						  )
            ) T  GROUP BY TrainGrade
     ) TT on TT.Name = su.TrainGrade 
  left join Co_Course on Co_Course.id = @courseId
     where su.isdelete = 0 
     and (
			(
				select count(*) 
				from Sys_GroupUser 
				where UserId=su.Userid and groupid in 
				     (
				         select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject)
				      )
		     )>0 
		     or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null
		  )     
     group by TrainGrade,SubscribeCount";
                var list = connection.Query<NameSubscribeCount>(sqlstr, new { courseId }).ToList();
                var openlevel =
                    connection.Query<string>("select OpenLevel from Co_Course where id = @courseId", new { courseId })
                              .FirstOrDefault()
                              .Split(',');

                var dataList = new List<NameSubscribeCount>();
                for (int i = 0; i < openlevel.Length; i++)
                {
                    var tmp = list.Where(p => p.Name == openlevel[i]).FirstOrDefault();
                    if (tmp != null)
                        dataList.Add(tmp);
                    else
                        dataList.Add(new NameSubscribeCount
                        {
                            Name = openlevel[i],
                            AllCount = 0,
                            SubscribeCount = 0
                        });
                }
                return dataList;
            }
        }

        /// <summary>
        /// 查询培训级别下的报名情况
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<NameSubscribeCount> GetTrainGradeSubscribe(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = @"select TrainGrade as Name,COUNT(1) AS AllCount, 
                        isnull(TT.SubscribeCount,0) as SubscribeCount 
                        FROM 
                        sys_user su
                        left join (
	                        SELECT TrainGrade as Name,COUNT(1) AS SubscribeCount FROM (
                                select sys_user.TrainGrade
                                from sys_user 
                                left join cl_courseorder on cl_courseorder.userid=sys_user.userid
                                left join Co_Course on Co_Course.id = cl_courseorder.courseid
                                where sys_user.IsDelete = 0 and cl_courseorder.courseid = @courseId
                                        and Cl_CourseOrder.orderstatus in (1,2,3) --1,2,3  现在取报名成功的
                                        and (Cl_CourseOrder.IsLeave = 0 or 
                                            (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1)
                                            or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                                                and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime]))
                                        and ((sys_user.deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject))
                                            or Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null )
                                        and ((select count(*) from Sys_GroupUser 
                                                where UserId=sys_user.Userid and groupid in (
                                                    select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject))
                                              )>0 or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null))
                            ) T  GROUP BY TrainGrade
                        ) TT on TT.Name = su.TrainGrade
left join Co_Course on Co_Course.id = @courseId
where su.isdelete = 0  and ((su.deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject))
                    or Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null )
                and ((select count(*) from Sys_GroupUser 
                        where UserId=su.Userid and groupid in (
                            select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject))
                      )>0 or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null))
                        group by TrainGrade,SubscribeCount";
                var list = connection.Query<NameSubscribeCount>(sqlstr, new { courseId }).ToList();
                var openlevel =
                    connection.Query<string>("select OpenLevel from Co_Course where id = @courseId", new { courseId })
                              .FirstOrDefault()
                              .Split(',');

                var dataList = new List<NameSubscribeCount>();
                for (int i = 0; i < openlevel.Length; i++)
                {
                    var tmp = list.Where(p => p.Name == openlevel[i]).FirstOrDefault();
                    if (tmp != null)
                        dataList.Add(tmp);
                    else
                        dataList.Add(new NameSubscribeCount
                            {
                                Name = openlevel[i],
                                AllCount = 0,
                                SubscribeCount = 0
                            });
                }
                return dataList;

                //for (int i = 0; i < openlevel.Length; i++)
                //{
                //    var tmp = list.Where(p => p.Name == openlevel[i]).FirstOrDefault();
                //    if (tmp == null)
                //        list.Add(new NameSubscribeCount
                //        {
                //            Name = openlevel[i],
                //            AllCount = 0,
                //            SubscribeCount = 0
                //        });
                //}
                //return list.OrderByDescending(p => p.Name).ToList();
            }
        }

        /// <summary>
        /// 获取报名成功的人员
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<NameSubscribeCount> GetSuccessTrainGradeSubscribe(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlstr = @"select TrainGrade as Name,COUNT(1) AS AllCount, 
                        isnull(TT.SubscribeCount,0) as SubscribeCount 
                        FROM 
                        sys_user su
                        left join (
	                        SELECT TrainGrade as Name,COUNT(1) AS SubscribeCount FROM (
                                select sys_user.TrainGrade
                                from sys_user 
                                left join cl_courseorder on cl_courseorder.userid=sys_user.userid
                                left join Co_Course on Co_Course.id = cl_courseorder.courseid
                                where sys_user.IsDelete = 0 and cl_courseorder.courseid = @courseId
                                        and Cl_CourseOrder.orderstatus in (1,2,3) --现在取报名成功的
                                        and (Cl_CourseOrder.IsLeave = 0 
                                            or (Cl_CourseOrder.IsLeave = 1 and Cl_CourseOrder.ApprovalFlag <>1)
                                            or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                                                and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime]))
                                        and ((sys_user.deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject))
                                            or Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null )
                                        and ((select count(*) from Sys_GroupUser 
                                                where UserId=sys_user.Userid and groupid in (
                                                    select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject))
                                              )>0 or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null))
                            ) T  GROUP BY TrainGrade
                        ) TT on TT.Name = su.TrainGrade
left join Co_Course on Co_Course.id = @courseId
where su.isdelete = 0  and ((su.deptid in (select id from dbo.F_SplitIDs(Co_Course.OpendepartObject))
                    or Co_Course.OpendepartObject = '' or Co_Course.OpendepartObject is null )
                and ((select count(*) from Sys_GroupUser 
                        where UserId=su.Userid and groupid in (
                            select id from dbo.F_SplitIDs(Co_Course.OpenGroupObject))
                      )>0 or Co_Course.OpenGroupObject = '' or Co_Course.OpenGroupObject is null))
                        group by TrainGrade,SubscribeCount";
                var list = connection.Query<NameSubscribeCount>(sqlstr, new { courseId }).ToList();

                //var openlevel = connection.Query<string>("select OpenLevel from Co_Course where id = @courseId", new { courseId }).FirstOrDefault().Split(',');

                //var dataList = new List<NameSubscribeCount>();
                //for (int i = 0; i < openlevel.Length; i++)
                //{
                //    var tmp = list.Where(p => p.Name == openlevel[i]).FirstOrDefault();
                //    if (tmp != null)
                //        dataList.Add(tmp);
                //    else
                //        dataList.Add(new NameSubscribeCount
                //        {
                //            Name = openlevel[i],
                //            AllCount = 0,
                //            SubscribeCount = 0
                //        });
                //}

                //return dataList;

                return list.Where(p => p.SubscribeCount > 0).ToList();
            }
        }

        /// <summary>
        /// 查询培训级别下CPA课程的报名情况
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <returns></returns>
        public List<NameSubscribeCount> GetTrainGradeCPASubscribe(int courseID)
        {
            using (var connection = OpenConnection())
            {
                const string sqlstr = @"SELECT b.TrainGrade AS Name FROM Cl_CpaLearnStatus AS a,Sys_User AS b WHERE CourseID=@courseID AND a.UserID=b.UserId";
                var list = connection.Query<NameSubscribeCount>(sqlstr, new { courseID = courseID }).ToList();
                var firstOrDefault = connection.Query<string>("select OpenLevel from Co_Course where id = @courseId", new { courseId = courseID }).FirstOrDefault();
                if (firstOrDefault != null)
                {
                    var openlevel = firstOrDefault.Split(',');
                    var dataList = new List<NameSubscribeCount>();
                    foreach (var grade in firstOrDefault.Split(','))
                    {
                        if (!string.IsNullOrEmpty(grade))
                        {
                            dataList.Add(new NameSubscribeCount
                                             {
                                                 Name = grade,
                                                 SubscribeCount = list.Count(p => p.Name == grade)
                                             });
                        }
                    }
                    return dataList;
                }
                return new List<NameSubscribeCount>();
            }
        }


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
        public List<LiXinModels.User.Sys_User> GetCourseSubscribeUserList(out int totalCount, int courseId,
                                                                          string where = " 1 = 1 ", int startIndex = 0,
                                                                          int pageLength = int.MaxValue,
                                                                          string orderBy =
                                                                              "ORDER BY Sys_User.userId DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>(string.Format(@"select count(1)
from sys_user
left join (select * from cl_courseorder where Cl_CourseOrder.courseId = {0} )  cl_courseorder on Cl_CourseOrder.userid = sys_user.userid
left join co_course on co_course.Id = {0} -- sys_user.TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel)) 
where " + where, courseId)).First();
                string query = string.Format(@"
                select top {0} * from (
                    select  row_number() over( {1} ) as rowNum
                        ,cl_courseorder.orderstatus AS IsApply
,cl_courseorder.IsAppoint
,cl_courseorder.orderstatus as CourseOrder
,cl_courseorder.isleave as CourseLeave
,cl_courseorder.ApprovalLimitTime as CourseApprovalLimitTime
,cl_courseorder.ApprovalDateTime as CourseApprovalDateTime
,cl_courseorder.ApprovalFlag as CourseLeaveApprovalFlag
,cl_courseorder.OrderTime as OrderTime
,sys_user.* 
from sys_user
left join (select * from cl_courseorder where Cl_CourseOrder.courseId = {2} )  cl_courseorder on Cl_CourseOrder.userid = sys_user.userid
left join co_course on co_course.Id = {2} --sys_user.TrainGrade in (select id from dbo.F_SplitIDs(Co_Course.OpenLevel))
                        where " + where + @" 
                ) result 
                where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy,
                                             courseId);

                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<LiXinModels.User.Sys_User>(query, parameters).ToList();
            }
        }


        /// <summary>
        /// 查询CPA课程中学员的预订情况
        /// </summary>
        /// <returns></returns>
        public List<LiXinModels.User.Sys_User> GetCPACourseSubscribeUserList(out int totalCount, int courseId,
                                                                          string where = " 1 = 1 ", int startIndex = 0,
                                                                          int pageLength = int.MaxValue,
                                                                          string orderBy =
                                                                              "ORDER BY Sys_User.userId DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>(string.Format(@"select count(1) from Cl_CpaLearnStatus,Sys_User where Sys_User.UserId= Cl_CpaLearnStatus.UserID and Cl_CpaLearnStatus.CourseID = {0} and {1}", courseId, where)).First();
                var query = string.Format(@"
                select top {0} * from (
                    select  row_number() over( {1} ) as rowNum,Sys_User.Realname,Sys_User.UserId,Sys_User.TrainGrade,Sys_User.DeptName,Sys_User.DeptPath,Sys_User.Sex,Cl_CpaLearnStatus.LastUpdateTime as OrderTime
                    from Cl_CpaLearnStatus,Sys_User
                    where Sys_User.UserId= Cl_CpaLearnStatus.UserID and Sys_User.IsDelete = 0 AND Cl_CpaLearnStatus.CourseID = {2} and " + where + @" 
                ) result 
                where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy, courseId);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<LiXinModels.User.Sys_User>(query, parameters).ToList();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="teacher"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<LiXinModels.CourseManage.Co_Course> GetListByTeacher(out int totalCount, int teacher,
                                                                         string where = " 1 = 1 ", int startIndex = 0,
                                                                         int pageLength = int.MaxValue,
                                                                         string orderBy = "ORDER BY Co_Course.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "";

                totalCount =
                    connection.Query<int>("select count(1) from Co_Course where Teacher=" + teacher + " " + where +
                                          " and CourseFrom=2 and way=1 and Publishflag=1").First();

                sql += "  select top(" + pageLength + ") * from (";
                sql += "  select  row_number() over( order by Id desc  ) as rowNum,";
                sql += "  Co_Course.OpenLevel,Co_Course.Id,CourseName,Way,StartTime,EndTime,";
                sql += @"  (select count(0) from Cl_CourseOrder 
                               left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                               where Cl_CourseOrder.courseid = Co_Course.id and Cl_CourseOrder.[OrderStatus] in (1,3) 
                               and ( Cl_CourseOrder.[IsLeave] = 0 
                                       or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                                       or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                                           and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])
                                   )
                               and Sys_user.IsDelete = 0) as 'number',";
                sql += "  NumberLimited,Publishflag,Sort";
                sql += "  from Co_Course where way=1 and Publishflag=1 and Teacher=" + teacher + " " + where;
                sql += "  )result where  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)";

                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };

                return connection.Query<LiXinModels.CourseManage.Co_Course>(sql, parameters).ToList();
            }
        }



        public List<LiXinModels.CourseManage.Co_Course> GetListByAllTeacher(out int totalCount,
                                                                        string where = " 1 = 1 ", int startIndex = 0,
                                                                        int pageLength = int.MaxValue,
                                                                        string orderBy = "ORDER BY Co_Course.id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "";

                totalCount =
                    connection.Query<int>("select count(1) from Co_Course where 1=1" + where +
                                          " and CourseFrom=2 and way=1 and Publishflag=1").First();

                sql += "  select top(" + pageLength + ") * from (";
                sql += "  select  row_number() over( order by Id desc  ) as rowNum,";
                sql += "  Co_Course.OpenLevel,Co_Course.Id,CourseName,Way,StartTime,EndTime,";
                sql += @"  (select count(0) from Cl_CourseOrder 
                               left join Sys_user on  Cl_CourseOrder.UserId = Sys_user.userid 
                               where Cl_CourseOrder.courseid = Co_Course.id and Cl_CourseOrder.[OrderStatus] in (1,3) 
                               and ( Cl_CourseOrder.[IsLeave] = 0 
                                       or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] <>1)
                                       or (Cl_CourseOrder.[IsLeave] = 1 and Cl_CourseOrder.[ApprovalFlag] = 1 
                                           and Cl_CourseOrder.[ApprovalDateTime] > Cl_CourseOrder.[ApprovalLimitTime])
                                   )
                               and Sys_user.IsDelete = 0) as 'number',";
                sql += "  NumberLimited,Publishflag,Sort";
                sql += "  from Co_Course where way=1 and Publishflag=1 " + where;
                sql += "  )result where  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)";

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };

                return connection.Query<LiXinModels.CourseManage.Co_Course>(sql, parameters).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetTeacherCourse(out int totalCount, int CourseId, int startIndex = 0,
                                                     int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "";
                string strsql = "";

                strsql += "select  count(1)";
                strsql += " from Cl_CourseOrder left join Sys_User on Cl_CourseOrder.UserId=Sys_User.UserId ";
                strsql += " where Cl_CourseOrder.CourseId=" + CourseId + "";
                strsql +=
                    " and Cl_CourseOrder.IsLeave=0 and (Cl_CourseOrder.OrderStatus=1 or Cl_CourseOrder.OrderStatus=3) ";
                strsql += " and (Cl_CourseOrder.OrderTime<OrderEndTime or getdate()>OrderEndTime)";
                strsql +=
                    "and charindex((select TrainGrade from Sys_User where UserId=Cl_CourseOrder.UserId),(select OpenLevel from Co_Course where Co_Course.Id=Cl_CourseOrder.CourseId))>0";

                totalCount = connection.Query<int>(strsql).First();

                //sql += "  select ";
                sql += " select top(" + pageLength + ") * from";
                sql += "( ";
                sql += "select  row_number() over( order by OrderTime desc  ) as rowNum,";
                sql += "Sys_User.Realname,Sys_User.DeptName,";
                sql +=
                    "(select count(1) from Survey_ReplyAnswer where Survey_ReplyAnswer.ObjectID=Cl_CourseOrder.CourseId) as 'replynum',";
                sql += "Cl_CourseOrder.CourseId,Sys_User.UserId";
                sql += " from Cl_CourseOrder left join Sys_User on Cl_CourseOrder.UserId=Sys_User.UserId ";
                sql += " where Cl_CourseOrder.CourseId=" + CourseId + "";
                sql +=
                    " and Cl_CourseOrder.IsLeave=0 and (Cl_CourseOrder.OrderStatus=1 or Cl_CourseOrder.OrderStatus=3) ";
                sql += " and (Cl_CourseOrder.OrderTime<OrderEndTime or getdate()>OrderEndTime)";
                sql +=
                    "and charindex((select TrainGrade from Sys_User where UserId=Cl_CourseOrder.UserId),(select OpenLevel from Co_Course where Co_Course.Id=Cl_CourseOrder.CourseId))>0";
                //sql += "order by Cl_CourseOrder.OrderTime desc";
                sql += ")result where  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)";

                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };

                return connection.Query<Cl_CourseOrder>(sql, parameters).ToList();

            }
        }

        /// <summary>
        /// 我的课程(讲师)详细下学员列表
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetTeacherCourseUserList(out int totalCount, int CourseId, int startIndex = 0,
                                                             int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>(string.Format(@"
                SELECT count(1) FROM
(
	SELECT 
        row_number() over( ORDER BY Co_Course.id DESC) as rowNum
		,Cl_CourseOrder.ID		
		,Co_Course.NumberLimited as NumberLimited
		,Co_Course.StartTime
		,Co_Course.EndTime
		,Sys_User.Realname as Realname
		,Sys_User.Sex as Sex
		,Sys_User.DeptCode as DeptCode	
		,Sys_User.TrainGrade as TrainGrade
		,Cl_CourseOrder.IsAppoint
		,Cl_CourseOrder.PassStatus
		,Cl_CourseOrder.GetScore
		,Cl_CourseOrder.CourseId
		,Cl_CourseOrder.UserId		    
	FROM Cl_CourseOrder left join Co_Course on Cl_CourseOrder.CourseId=Co_Course.ID
						left join Sys_User on Cl_CourseOrder.UserId=Sys_User.UserId
						left join Sys_ClassRoom on  Co_Course.RoomId=Sys_ClassRoom.id
	WHERE 1=1
	 And ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
        and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
    or Cl_CourseOrder.orderstatus = 3)  and Cl_CourseOrder.CourseId={0}
) bb  ", CourseId)).First();


                string query = string.Format(@"
                SELECT top({1}) * FROM
(
	SELECT 
        row_number() over( ORDER BY Co_Course.id DESC) as rowNum
		,Cl_CourseOrder.ID		
		,Co_Course.NumberLimited as NumberLimited
		,Co_Course.StartTime
		,Co_Course.EndTime
		,Sys_User.Realname as Realname
		,Sys_User.Sex as Sex
		,Sys_User.DeptCode as DeptCode	
		,Sys_User.TrainGrade as TrainGrade
        ,Sys_User.JobNum as JobNum
		,Cl_CourseOrder.IsAppoint
		,Cl_CourseOrder.PassStatus
		,Cl_CourseOrder.GetScore
		,Cl_CourseOrder.CourseId
		,Cl_CourseOrder.UserId
        ,Cl_CourseOrder.OrderStatus
        ,Sys_User.DeptName as bumen		    
	FROM Cl_CourseOrder left join Co_Course on Cl_CourseOrder.CourseId=Co_Course.ID
						left join Sys_User on Cl_CourseOrder.UserId=Sys_User.UserId
						left join Sys_ClassRoom on  Co_Course.RoomId=Sys_ClassRoom.id
	WHERE 1=1
	And ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
        and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
    or Cl_CourseOrder.orderstatus = 3)  and Cl_CourseOrder.CourseId={0}
) bb
WHERE  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", CourseId, pageLength);

                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<Cl_CourseOrder>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 我的课程(讲师)考情学员列表
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="CourseId"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetTeacherCourseAttendceList(out int totalCount, int CourseId, int startIndex = 0,
                                                                 int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount = connection.Query<int>(string.Format(@"
                SELECT count(1) FROM
(
	SELECT 
        row_number() over( ORDER BY Co_Course.id DESC) as rowNum
		,Cl_CourseOrder.ID		
		,Co_Course.NumberLimited as NumberLimited
		,Co_Course.StartTime
		,Co_Course.EndTime
		,Sys_User.Realname as Realname
		,Sys_User.Sex as Sex
		,Sys_User.DeptName as DeptCode	
		,Sys_User.TrainGrade as TrainGrade
		,Cl_CourseOrder.IsAppoint
		,Cl_CourseOrder.PassStatus
		,Cl_CourseOrder.GetScore
		,Cl_CourseOrder.CourseId
		,Cl_CourseOrder.UserId
			    
	FROM Cl_CourseOrder left join Co_Course on Cl_CourseOrder.CourseId=Co_Course.ID
						left join Sys_User on Cl_CourseOrder.UserId=Sys_User.UserId
						left join Cl_Attendce on  Cl_Attendce.UserId=Cl_CourseOrder.UserId and Cl_Attendce.CourseId=Cl_CourseOrder.CourseId
	WHERE 1=1
	 AND   ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
        and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
    or Cl_CourseOrder.orderstatus = 3)  and Cl_CourseOrder.CourseId={0}
) bb
  ", CourseId)).First();


                string query = string.Format(@"
                SELECT top({1}) * FROM
(
	SELECT 
        row_number() over( ORDER BY Co_Course.id DESC) as rowNum
		,Cl_CourseOrder.ID		
		,Co_Course.NumberLimited as NumberLimited
		,Co_Course.StartTime
		,Co_Course.EndTime
		,Sys_User.Realname as Realname
		,Sys_User.Sex as Sex
		,Sys_User.DeptName as DeptCode	
		,Sys_User.TrainGrade as TrainGrade
        ,Sys_User.JobNum as JobNum
		,Cl_CourseOrder.IsAppoint
		,Cl_CourseOrder.PassStatus
		,Cl_CourseOrder.GetScore
		,Cl_CourseOrder.CourseId
		,Cl_CourseOrder.UserId
        ,Cl_CourseOrder.OrderStatus
        ,Sys_User.DeptName as bumen
        ,Cl_Attendce.StartTime as AttendceStartTime
        ,Cl_Attendce.EndTime as AttendceEndTime		    
	FROM Cl_CourseOrder left join Co_Course on Cl_CourseOrder.CourseId=Co_Course.ID
						left join Sys_User on Cl_CourseOrder.UserId=Sys_User.UserId
						left join Cl_Attendce on  Cl_Attendce.UserId=Cl_CourseOrder.UserId and Cl_Attendce.CourseId=Cl_CourseOrder.CourseId
	WHERE 1=1
	 And ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
        and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
    or Cl_CourseOrder.orderstatus = 3)  and Cl_CourseOrder.CourseId={0}
) bb
WHERE rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", CourseId, pageLength);

                var parameters = new
                    {
                        pageCount = pageLength,
                        pageIndex = startIndex / pageLength
                    };
                return connection.Query<Cl_CourseOrder>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 讲师端考试人员都绑定出来
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetTeacherOnLineTest(out int totalCount, int CourseId, int TeacherId, int startIndex = 0,
                                                                 int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string count = string.Format(@"
select count(1) from
(
select 
	row_number() over( ORDER BY Cl_CourseOrder.CourseId DESC) as rowNum,
	Cl_CourseOrder.CourseId,
	Sys_User.UserId,
	Sys_User.Realname,
	Sys_User.DeptName,
	Sys_User.JobNum,
	Co_CoursePaper.Id,
	Co_CoursePaper.PaperId,
	Co_CoursePaper.TestTimes,
	Co_CoursePaper.LevelScore
 from Cl_CourseOrder 
 left join Co_Course on Cl_CourseOrder.CourseId=Co_Course.Id
 left join Co_CoursePaper on Cl_CourseOrder.CourseId=Co_CoursePaper.CourseId
 left join Sys_User on Sys_User.UserId=Cl_CourseOrder.UserId
  where ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
        and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
    or Cl_CourseOrder.orderstatus = 3) and Cl_CourseOrder.CourseId={0} {1} and Co_Course.IsTest=1
    )bb", CourseId, TeacherId == 0 ? " and 1=1 " : string.Format(" and Co_Course.Teacher={0} ", TeacherId));

                totalCount = connection.Query<int>(count).First();

                string sql = string.Format(@"

select top({2}) * from
(
select 
	row_number() over( ORDER BY Cl_CourseOrder.CourseId DESC) as rowNum,
	Cl_CourseOrder.CourseId,
	Sys_User.UserId,
	Sys_User.Realname as realname,
	Sys_User.DeptName as DeptName,
	Sys_User.JobNum,
	Co_CoursePaper.Id as CoPaperID,
	Co_CoursePaper.PaperId as PaperId,
	Co_CoursePaper.TestTimes,
	Co_CoursePaper.LevelScore
 from Cl_CourseOrder 
 left join Co_Course on Cl_CourseOrder.CourseId=Co_Course.Id
 left join Co_CoursePaper on Cl_CourseOrder.CourseId=Co_CoursePaper.CourseId
 left join Sys_User on Sys_User.UserId=Cl_CourseOrder.UserId
  where ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
    or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
        and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
    or Cl_CourseOrder.orderstatus = 3) and Cl_CourseOrder.CourseId={0} {1} and Co_Course.IsTest=1
    )bb
    where  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", CourseId, TeacherId == 0 ? " and 1=1 " : string.Format(" and Co_Course.Teacher={0} ", TeacherId), pageLength);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Cl_CourseOrder>(sql, parameters).ToList();


            }
        }





        /// <summary>
        /// 修改学时
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="UserId"></param>
        /// <param name="GetScore"></param>
        /// <param name="PassStatus">0:考勤已经有学时,1:直接修改满分情况</param>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool UpdateGetScore(int courseId, int UserId, decimal GetScore, int PassStatus, int LearnStatus = 0)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "";
                //if (num == 0)
                //{
                //    sql = "update Cl_CourseOrder set GetScore=GetScore+" + GetScore + ", LearnStatus=" + LearnStatus + ", PassStatus=" + PassStatus + " where CourseId=" + courseId + " and UserId=" + UserId;
                //}
                //else
                //{
                sql = "update Cl_CourseOrder set GetScore=" + GetScore + " , LearnStatus=" + LearnStatus + " , PassStatus=" + PassStatus + " where CourseId=" + courseId + " and UserId=" + UserId;
                //}
                return connection.Query<int>(sql).FirstOrDefault() > 0;

            }
        }

        /// <summary>
        /// 当第5次的时候学习状态 2：已完成 
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="UserId"></param>
        /// <param name="LearnStatus"></param>
        /// <returns></returns>
        public bool UpdateLearnStatus(int courseId, int UserId, int LearnStatus = 0)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "update Cl_CourseOrder set LearnStatus=" + LearnStatus + " where CourseId=" + courseId + " and UserId=" + UserId;
                return connection.Query<int>(sql).FirstOrDefault() > 0;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="PassStatus"></param>
        /// <returns></returns>
        public bool UpdatePassStatus(int courseid, int userid, int PassStatus)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "update Cl_CourseOrder set PassStatus=" + PassStatus + " where CourseId=" + courseid + " and UserId=" + userid;

                int result = connection.Execute(sql);
                return result > 0;
            }
        }
        /// <summary>
        /// 更新预定信息
        /// </summary>
        /// <param name="sqlstr">sql语句</param>
        /// <returns></returns>
        public bool ChangeCourseOrderInfor(string sqlstr)
        {
            using (var connection = OpenConnection())
            {
                var result = connection.Execute(sqlstr);
                return result > 0;
            }
        }



        #region 排队递补

        /// <summary>
        /// 获取需要修改排队状态的那些课程的报名情况
        /// </summary>
        /// <param name="lastTime"></param>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetUpdateCourseOrderStatus(DateTime lastTime, string strWhere = " 1 = 1 ", string orderby = "order by Cl_CourseOrder.courseid asc")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"select  co_course.NumberLimited
,Cl_CourseOrder.id
,Cl_CourseOrder.courseid
,Cl_CourseOrder.userid
,Cl_CourseOrder.ordertime
,Cl_CourseOrder.orderendtime
,Cl_CourseOrder.approvaldatetime
,Cl_CourseOrder.isleave
,Cl_CourseOrder.approvalflag
,Cl_CourseOrder.ApprovalLimitTime
,Cl_CourseOrder.orderstatus
,Sys_user.mobilenum as userMobileNum
,Sys_user.email as userEmail
,Sys_user.realname
,co_course.coursename as course_name
from dbo.Cl_CourseOrder
left join (select distinct courseid from Cl_CourseOrder where approvaldatetime > '{0}') col 
        on Cl_CourseOrder.courseid = col.courseid
left join co_course on col.courseid = co_course.id
left join Sys_user on Sys_user.userId = Cl_CourseOrder.UserId
where " + strWhere + orderby, lastTime);
                return conn.Query<Cl_CourseOrder>(sqlwhere).ToList();
            }
        }

        public List<Cl_CourseOrder> GetUpdateCourseOrderStatusByCourseId(int courseId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"select  co_course.NumberLimited
,Cl_CourseOrder.id
,Cl_CourseOrder.courseid
,Cl_CourseOrder.userid
,Cl_CourseOrder.ordertime
,Cl_CourseOrder.orderendtime
,Cl_CourseOrder.approvaldatetime
,Cl_CourseOrder.isleave
,Cl_CourseOrder.approvalflag
,Cl_CourseOrder.ApprovalLimitTime
,Cl_CourseOrder.orderstatus
,Sys_user.mobilenum as userMobileNum
,Sys_user.email as userEmail
,Sys_user.realname
,co_course.coursename as course_name
from dbo.Cl_CourseOrder
left join co_course on Cl_CourseOrder.courseid = co_course.id
left join Sys_user on Sys_user.userId = Cl_CourseOrder.UserId
where co_course.isdelete = 0
    --and ((Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 0 )
	--or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag in (0,2)) 
    --or (Cl_CourseOrder.orderstatus = 1 and Cl_CourseOrder.isleave = 1 and Cl_CourseOrder.approvalflag = 1 
    --    and Cl_CourseOrder.approvaldatetime > Cl_CourseOrder.ApprovalLimitTime) 
	--or Cl_CourseOrder.orderstatus = 2 or Cl_CourseOrder.orderstatus = 3)
and co_course.id = {0}
order by Cl_CourseOrder.courseid asc,Cl_CourseOrder.orderstatus asc,Cl_CourseOrder.ordertime asc
", courseId);
                return conn.Query<Cl_CourseOrder>(sqlwhere).ToList();
            }
        }

        /// <summary>
        /// 修改排队状态
        /// </summary>
        /// <param name="ids"></param>
        public void UpdateOrderStatus(string ids)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere = string.Format(@"update Cl_CourseOrder set orderstatus = 1 where id in ({0})", ids);
                conn.Query<int>(sqlwhere).FirstOrDefault();
            }
        }

        #endregion

        /// <summary>
        /// 撤销部门指定人员
        /// </summary>
        /// <param name="sqlstr"></param>
        public void DropAssignUser(string sqlstr)
        {
            using (var connection = OpenConnection())
            {
                connection.Execute(sqlstr);
            }
        }

        /// <summary>
        /// 查找某人补预定审批通过的次数
        /// </summary>
        /// <param name="userID">学员ID</param>
        /// <returns></returns>
        public int JudgeMakeUpTimes(int userID)
        {
            var sqlstr = string.Format("select count(*) from Cl_CourseOrder where OrderStatus=3 and UserId={0} and MakeUpApprovalFlag=1", userID);
            using (var connection = OpenConnection())
            {
                return connection.Query<int>(sqlstr).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取集中授课中报名的人员
        /// </summary>
        public List<Sys_User> GetCourseOrderName(int courseID, string where = "1=1", int pageSize = 10, int pageIndex = 1)
        {
            using (var connection = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM(
SELECT row_number()OVER(ORDER BY su.UserId  asc) number,count(*)OVER(PARTITION BY null) totalcount,
OrderTime,su.* FROM Cl_CourseOrder cco LEFT JOIN Sys_User su ON cco.UserId=su.UserId
WHERE OrderStatus>0 AND CourseId={0} and {1}) r
WHERE number BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", courseID, where);
                var param = new
                {
                    startIndex = pageIndex,
                    startLength = pageSize
                };
                return connection.Query<Sys_User>(sql, param).ToList();

            }
        }
    }

}