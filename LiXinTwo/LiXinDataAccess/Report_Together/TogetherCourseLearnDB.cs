using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Report_Together;
using Retech.Core;
using Retech.Data;
using System.Data;

namespace LiXinDataAccess
{
    public class TogetherCourseLearnDB : BaseRepository
    {
        #region 
        /// <summary>
        /// 根据ID获取课程信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public Report_CourseShow GetTogetherCourseById(int courseId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlWhere = @"  select Co_Course.* ,Sys_User.Realname AS TeacherName
        ,Sys_ClassRoom.RoomName ,openNumber=(SELECT count(UserId) FROM dbo.F_GetTogtherSigle(@Id)
WHERE UserId IN (SELECT UserId FROM View_CheckUser))
 from Co_Course 
 LEFT JOIN Sys_User ON Co_Course.Teacher=Sys_User.UserId 
 LEFT Join Sys_ClassRoom ON Sys_ClassRoom.Id=Co_Course.RoomId  
 where Co_Course.Id=@Id ";
                var param = new
                {
                    Id=courseId
                };
                return connection.Query<Report_CourseShow>(sqlWhere, param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得员工单门课程报名、参与情况列表
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <param name="isRt">是否视频转播0-否 1-是</param>
        /// <param name="where">条件语句‘and ....’</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按Id倒序排序</param>
        /// <returns></returns>
        public List<Report_UserLearnTogetherCourseShow> GetSingleTogetherCourseUsersList(int courseId=0,int isRt=0, string where = "",
                                                   int startIndex = 1, int pageLength = int.MaxValue,
                                                    string orderBy = " ORDER BY DeptPath,GetScore ")
        {

            using (IDbConnection connection = OpenConnection())
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = " ORDER BY DeptPath,GetScore ";
                }
                string unionQuery = isRt==0?"":
                string.Format(@"
                union 
--二期视频转播
select u.UserId,u.username,u.DeptId
       ,u.DeptPath,u.Realname,u.TrainGrade
       ,(case u.CPA when '是' then 1 
                   else 0
         end) as IsCPA --0-否 1-是 
       ,3 as OrderType --报名类型 0-预订 1-指定 2-补预订 3-视频转播
       ,(case when dtco.OrderStatus=0 then 0 
		   else 2  
		   end) as LeaveType --请假类型 0-退订 1-请假 2-N/A 
	   ,99 as AttFlag
       ,cc.StartTime as CourseStartTime,cc.EndTime as CourseEndTime
       ,'2050-1-1' as AttendceStartTime,'2000-1-1' as AttendceEndTime --考勤相关
       ,dtatt.Status as  DepTranAttStatus --0：待考勤；1：正常;2:缺勤；3：迟到；4：早退；5：迟到且早退
       , CASE WHEN dco.ApprovalFlag=1 THEN dtco.AttScore+dtco.EvlutionScore+dtco.ExamScore ELSE 0 END--获得学时
       ,(case when (select count(1) from Cl_CourseAdvice ca where ca.CourseId=dtco.CourseId and ca.UserId=dtco.UserId and ca.IsDelete=0)>0
             then 1 
             else 0
        end) as IsCourseAdvice --课前建议  0-否 1-是
        ,(case when (select count(1) from Survey_ReplyAnswer ra 
                            where ra.ObjectID=dtco.CourseId and ra.UserID=dtco.UserId
                                  and ra.SourceFrom=0 and ra.Status=1 )>0
             then 1 
             else 0 
        end) as IsSurveyReplyAnswer --课后评估  0-否 1-是
        ,(case u.IsTeacher  when 1 then 1 
              else 0 
          end) as IsTeacher  
        ,2 as IsResourceWrite --0-否 1-是 2-N/A 
        ,dtco.OrderTime
from DepTran_CourseOrder dtco 
LEFT JOIN   DepTran_CourseOpen dco ON dco.CourseId=dtco.CourseId AND dtco.DepartSetId=dco.DepartId
join Co_Course cc on cc.Id=dtco.CourseId 
LEFT JOIN Sys_User u ON dtco.UserId=u.UserId 
left join DepTran_Attendce dtatt on dtatt.CourseId=dtco.CourseId and dtatt.UserId=dtco.UserId 
where cc.Way=1 
      and dtco.UserId in (select UserId from View_CheckUser )
      and  dtco.CourseId={0} 

", courseId);
                
                string query = string.Format(@"
select top({0}) * from(
SELECT  count(*) OVER(PARTITION BY null) AS totalCount,row_number() over({4}) as rowNum,* 
FROM
(
	-- 一期集中授课
select u.UserId,u.username,u.DeptId
       ,u.DeptPath,u.Realname,u.TrainGrade
       ,(case u.CPA when '是' then 1 
                   else 0
         end) as IsCPA --0-否 1-是 
       ,(case when co.OrderStatus=3 then 2 --when cc.IsRT=1 then '视频转播' 
       when co.IsAppoint=0 then 0 
       else 1  
       end) as OrderType --报名类型 0-预订 1-指定 2-补预订
       ,(case when co.IsLeave=1  AND co.OrderStatus!=0 and co.[ApprovalFlag] = 1 
        and co.[ApprovalDateTime] <= co.[ApprovalLimitTime] then 1 
       when co.OrderStatus=0 then 0 
       else 2  
       end) as LeaveType --请假类型 0-退订 1-请假 2-N/A 
       ,cc.AttFlag as AttFlag
       ,cc.StartTime as CourseStartTime,cc.EndTime as CourseEndTime
       ,att.StartTime as AttendceStartTime,att.EndTime as AttendceEndTime --考勤相关
       ,99 as  DepTranAttStatus
       ,CASE when dbo.F_NewIsAppStatus({1},co.UserId,OrderStatus)=1 then co.GetScore ELSE 0  end GetScore --获得学时
       ,(case when (select count(1) from Cl_CourseAdvice ca where ca.CourseId=co.CourseId and ca.UserId=co.UserId and ca.IsDelete=0)>0
             then 1 
             else 0
        end) as IsCourseAdvice --课前建议  0-否 1-是
        ,(case when (select count(1) from Survey_ReplyAnswer ra 
                            where ra.ObjectID=co.CourseId and ra.UserID=co.UserId
                                  and ra.SourceFrom=0 and ra.Status=1 )>0
             then 1 
             else 0 
        end) as IsSurveyReplyAnswer --课后评估  0-否 1-是
        ,(case u.IsTeacher  when 1 then 1 
              else 0 
          end) as IsTeacher  
        ,2 as IsResourceWrite --0-否 1-是 2-N/A
        ,co.OrderTime
from Cl_CourseOrder co 
join Co_Course cc on cc.Id=co.CourseId 
LEFT JOIN Sys_User u ON co.UserId=u.UserId 
left join Cl_Attendce att on att.CourseId=co.CourseId and att.UserId=co.UserId 
where cc.Way=1 
      and co.OrderStatus!=2
      and co.UserId in (select UserId from View_CheckUser) --纳入考核
      and  co.CourseId={1} 
 {2}
	       
) bb_yxt
  where 1=1 
        {3} 
)aa_yxt where rowNum between   @startLength*(@startIndex-1)+1  AND @startLength*@startIndex
", pageLength,courseId,unionQuery, where, orderBy);

                var parameters = new
                {
                    startIndex = startIndex,
                    startLength = pageLength
                };
                return connection.Query<Report_UserLearnTogetherCourseShow>(query, parameters).ToList();
            }
        }
        #endregion

        

        
      


    }
}
