using System;
using System.Collections.Generic;
using System.Linq;
using LiXinModels;
using LiXinModels.DepCourseLearn;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.DepLeaveApproval
{
    public class DepLeaveApprovalDB: BaseRepository
    {
        /// <summary>
        /// 获取要审批的请假列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageCount">每页条数</param>
        /// <param name="order">排序方式</param>
        /// <param name="where">查询条件</param>
        /// <param name="hour">配置多少小时候审批默认拒绝</param>
        /// <returns></returns>
        public List<DepLeaveInfor> GetDepLeaveApprovalList(int pageIndex = 1, int pageCount = int.MaxValue, string order = " o.LeaveTime desc", string where = " and 1=1",int hour=999)
        {
            using (var conn = OpenConnection())
            {
                var query =
                    string.Format(
                        @"
select * 
from (
		SELECT row_number()OVER(ORDER BY {0}) num,
		count(*)OVER(PARTITION BY null) Totalcount,
		o.Id,
		o.UserId as UserID,
		u.RealName,
		u.Sex,
        u.email as Email,
        u.Mobilenum as Phone,
		u.TrainGrade,
		u.PayGrade,
        (select DeptName from Sys_Department where DepartmentId=u.DeptId) as DeptName,
		o.LeaveTime,
		o.Reason,
        c.IsMust,
		c.CourseName,
		c.Code as CourseCode,
		c.CourseLength,
		dbo.f_GetTeacherName(c.Teacher) as Teacher,
        --(case when dateadd(minute,{2},o.leavetime)>getdate() then 0 else 1 end) as LimitFlag,
        o.ApprovalLimitTime,
		c.StartTime,
		c.EndTime,
		(select Realname from Sys_User where UserId=o.ApprovalUserId) as AppRealName,
		o.AppReason,
		o.ApprovalFlag as AppFlag,
		o.ApprovalTime as AppTime
		from Dep_CourseOrder o
		left join Dep_Course c on o.CourseId=c.Id
		left join Sys_User u on o.UserId=u.UserId
		where o.IsLeave=1 {1}
	 ) temp
where num between  @pageCount*(@pageIndex-1)+1 and @pageCount*@pageIndex", order, where,hour);
                var param = new { pageIndex, pageCount };
                return conn.Query<DepLeaveInfor>(query, param).ToList();
            }
        }

        /// <summary>
        /// 根据ID获取当个请假信息
        /// </summary>
        /// <param name="id">当前页</param>
        /// <param name="hour">配置多少小时候审批默认拒绝</param>
        /// <returns></returns>
        public DepLeaveInfor GetDepLeaveApprovalInfor(int id,int hour=999)
        {
            using (var conn = OpenConnection())
            {
                var query =
                    string.Format(
                        @"
select o.Id,
		o.UserId as UserID,
		u.RealName,
		u.Sex,
        u.email as Email,
        u.Mobilenum as Phone,
		u.TrainGrade,
		u.PayGrade,
		o.LeaveTime,
		o.Reason,
        c.IsMust,
(select DeptName from Sys_Department where DepartmentId=u.DeptId) as DeptName,
		c.CourseName,
		c.Code as CourseCode,
		c.CourseLength,
		dbo.f_GetTeacherName(c.Teacher) as Teacher,
        --(case when dateadd(minute,{1},o.leavetime)>getdate() then 0 else 1 end) as LimitFlag,
        o.ApprovalLimitTime,
		c.StartTime,
		c.EndTime,
		(select Realname from Sys_User where UserId=o.ApprovalUserId) as AppRealName,
		o.AppReason,
		o.ApprovalFlag as AppFlag,
		o.ApprovalTime as AppTime
		from Dep_CourseOrder o
		left join Dep_Course c on o.CourseId=c.Id
		left join Sys_User u on o.UserId=u.UserId
		where o.IsLeave=1 and o.Id={0}", id,hour);
                return conn.Query<DepLeaveInfor>(query).FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据ID获取当个请假信息
        /// </summary>
        /// <param name="id">请假记录ID</param>
        /// <param name="appFlag">审批结果</param>
        /// <param name="reason">审批理由</param>
        /// <param name="appTime">审批时间</param>
        /// <param name="appUserId">审批人ID</param>
        /// <returns></returns>
        public bool UpdateLeaveInfor(int id,int appFlag,string reason,DateTime appTime,int appUserId)
        {
            using (var conn = OpenConnection())
            {
                var query = string.Format(@"update Dep_CourseOrder set ApprovalFlag={0},AppReason='{1}',ApprovalTime='{2}',ApprovalUserId={3}{4} where Id=@id ", appFlag, reason, appTime.ToString("yyyy-MM-dd HH:mm:ss"), appUserId, appFlag == 1 ? ",GetScore=0,AttScore=0,EvlutionScore=0,ExamScore=0" : "");
                return conn.Execute(query, new {id})>0;
            }
        }

        /// <summary>
        /// 获取相关配置
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <param name="key">配置类型</param>
        /// <returns></returns>
        public Sys_ParamConfig GetConfig(int deptId,int key)
        {
            using (var conn = OpenConnection())
            {
                var query = string.Format("select * from Dep_ParamConfig where DeptId={0} and ConfigType={1} ", deptId,key);
                return conn.Query<Sys_ParamConfig>(query).FirstOrDefault();
            }
        }
    }
}
