using LiXinModels.Report_Vedio;
using Retech.Core;
using System;
using System.Collections.Generic;
using System.Data;
using Retech.Data;
using System.Linq;
using System.Text;

namespace LiXinDataAccess.Report_fVedio
{
    public class Report_fOnLineTestDB : BaseRepository
    {
        /// <summary>
        /// 在线测试情况
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Report_OnLineTest> GetOnLineTest(int courseid, string where)
        {
            using (IDbConnection connection = OpenConnection())
            {
                #region 由于需求理解差异，进行更改 保留原方法 以防止错误
                
//                string querystr = string.Format(@" 
//     DECLARE  @IsOpenSub INT
//SET  @IsOpenSub=0
//SELECT @IsOpenSub=IsOpenSub FROM Co_Course WHERE Id={1}
//             
//                   
//    select  (select dbo.GetDeptTwoPathByDeptID(Sys_Department.DepartmentId)) as DeptName,
//		    (
//			    select count(1) from Cl_CpaLearnStatus 
//				    left join sys_user on Cl_CpaLearnStatus.Userid=sys_user.Userid				
//			    where Cl_CpaLearnStatus.courseid={1} and sys_user.Deptid =Sys_Department.DepartmentId
//			    and Cl_CpaLearnStatus.IsPass=1 and Cl_CpaLearnStatus.CpaFlag=0
//		    ) as IsPassNum,
//			(
//				select dbo.f_GetVedioUserList({1},Sys_Department.DepartmentId)
//			) as userlist,
//            (select istest from co_course where id={1} ) as istest,
//            Sys_Department.DepartmentId as deptid
//     from Sys_Department where {0}
//  ", where, courseid);

                #endregion

                var querystr = string.Format(@"DECLARE  @Istest INT 
SET  @Istest=0
SELECT @Istest=Istest FROM Co_Course WHERE Id={0}

SELECT ccs.UserID UserId,syu.DeptId,@Istest istest,dbo.GetDeptPathByDeptID(syu.DeptId) as DeptName
,dbo.GetDeptTwoPathByDeptID(syu.DeptId) as DeptTwoName,ccs.IsPass
FROM Cl_CpaLearnStatus     ccs
left join sys_user syu on ccs.UserID=syu.userid
AND syu.IsDelete=0 AND syu.Status=0
WHERE ccs.courseid ={0} and CpaFlag=0 and {1}", courseid, where);
                //var parameters = new
                //{
                //    pageCount = pageLength,
                //    pageIndex = startIndex / pageLength
                //};
                return connection.Query<Report_OnLineTest>(querystr).ToList();


            }
        }


        /// <summary>
        /// 参与情况
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Report_OnLineTest> GetCanYu(int courseid, string where)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string querystr = string.Format(@"                    
    select  (select dbo.GetDeptPathByDeptID(Sys_Department.DepartmentId)) as DeptName,
		    (
			    select count(1) from Cl_CpaLearnStatus 
				    left join sys_user on Cl_CpaLearnStatus.Userid=sys_user.Userid				
			    where Cl_CpaLearnStatus.courseid={1} and sys_user.Deptid =Sys_Department.DepartmentId
                AND  Cl_CpaLearnStatus.CpaFlag=0
		    ) as YiCanJia,
		    (
				select sum(VedioTime) from Cl_CpaLearnStatus
				left join sys_user on Cl_CpaLearnStatus.Userid=sys_user.Userid				
			    where Cl_CpaLearnStatus.courseid={1} and sys_user.Deptid =Sys_Department.DepartmentId
		    ) as allVedioTime,
		    (
			    select sum(LearnTimes) from Cl_LearnVideoInfor 					
					where LearnId in(select id from Cl_CpaLearnStatus
						left join sys_user on Cl_CpaLearnStatus.Userid=sys_user.Userid	
					 where courseid={1} and sys_user.Deptid =Sys_Department.DepartmentId
					)
		    ) as LearnTimes,
			--(
			--	select dbo.f_GetVedioUserList({1},Sys_Department.DepartmentId)
			--) as userlist,
            --(select istest from co_course where id={1} ) as istest,
            Sys_Department.DepartmentId as deptid
     from Sys_Department where {0}
  ", where, courseid);

                //var parameters = new
                //{
                //    pageCount = pageLength,
                //    pageIndex = startIndex / pageLength
                //};
                return connection.Query<Report_OnLineTest>(querystr).ToList();
                
            }
        }


        /// <summary>
        /// 学习人员明细
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="courseid"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        public List<Report_OnLineTest> GetOnLineTestDetail(int courseid,string where="1=1", int startIndex = 0, int pageLength = int.MaxValue, string jsRenderSortField="")
        {
            using (IDbConnection connection = OpenConnection())
            {
                #region 旧
//                string strsql = string.Format(@"
//select count(1)
//		 from Cl_CpaLearnStatus
//	left join Sys_User on Cl_CpaLearnStatus.userid =Sys_User.userid
// where   Cl_CpaLearnStatus.CpaFlag=0 AND courseid={0} and {1}
//", courseid, where);

//                totalCount = connection.Query<int>(strsql).First();

//                string querystr = string.Format(@"   
//select top {2} * from (    
//    select  row_number() over( {1} ) as rowNum,             
//     Sys_User.DeptPath as DeptName,
//        Sys_User.Userid as UserId,
//        Sys_User.DeptId as DeptId,
//		Sys_User.Realname as Realname,
//		Sys_User.TrainGrade as TrainGrade,
//		Sys_User.CPA as CPA,
//		sys_user.CPANo as CPANo,
//        co_course.istest as istest,
//		Cl_LearnVideoInfor.LearnTimes as LearnTimes,
//		Cl_CpaLearnStatus.GetLength as GetLength,
//        Cl_CpaLearnStatus.VedioTime as VedioTime,
//        Cl_CpaLearnStatus.IsPass as IsPass
//		 from Cl_CpaLearnStatus
//	left join Sys_User on Cl_CpaLearnStatus.userid =Sys_User.userid
//    left join Cl_LearnVideoInfor on Cl_CpaLearnStatus.Id=Cl_LearnVideoInfor.Learnid
//    left join co_course on co_course.id=Cl_CpaLearnStatus.courseid
// where  Cl_CpaLearnStatus.CpaFlag=0 AND courseid={0} and {3} )
// result where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
//  ", courseid, jsRenderSortField, pageLength, where);
                #endregion

                var querystr = string.Format(@"SELECT * FROM (
select  DISTINCT       
Sys_User.Username,     
Sys_User.DeptPath as DeptName,
        Sys_User.Userid as UserId,
        Sys_User.DeptId as DeptId,
		Sys_User.Realname as Realname,
		Sys_User.TrainGrade as TrainGrade,
		Sys_User.CPA as CPA,
		sys_user.CPANo as CPANo,
        co_course.istest as istest,
		sum(Cl_LearnVideoInfor.LearnTimes) OVER(PARTITION BY Sys_User.Userid) as LearnTimes,
		Cl_CpaLearnStatus.GetLength as GetLength,
        Cl_CpaLearnStatus.VedioTime as VedioTime,
        Cl_CpaLearnStatus.IsPass as IsPass,
        Cl_CpaLearnStatus.LastUpdateTime
		 from Cl_CpaLearnStatus
	left join Sys_User on Cl_CpaLearnStatus.userid =Sys_User.userid
    left join Cl_LearnVideoInfor on Cl_CpaLearnStatus.Id=Cl_LearnVideoInfor.Learnid
    left join co_course on co_course.id=Cl_CpaLearnStatus.courseid
 where  Cl_CpaLearnStatus.CpaFlag=0 AND courseid={0}
  AND Sys_User.isDelete=0 AND status=0   
 -- 
 ) t WHERE   {1}
 {2} 
  ", courseid, where, jsRenderSortField);
                //var parameters = new
                //{
                //    pageCount = pageLength,
                //    pageIndex = startIndex / pageLength
                //};
                return connection.Query<Report_OnLineTest>(querystr).ToList();


            }
        }

        /// <summary>
        /// 各月份学习情况分布图
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public List<Report_OnLineTest> GetMonthByCourseId(int courseid)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string querystr = string.Format(@"  
  select datepart(month,LastUpdateTime) as monthstr,
  (select dbo.GetDeptPathByDeptID(sys_user.deptid)) as DeptName,sys_user.deptid DeptId,sum(clv.LearnTimes)    LearnTimes
   from Cl_CpaLearnStatus 
	left join sys_user on Cl_CpaLearnStatus.userid=sys_user.userid
	LEFT JOIN Cl_LearnVideoInfor clv ON clv.LearnId= Cl_CpaLearnStatus.Id
  where courseid={0}  AND CpaFlag=0 and
  sys_user.deptid IN (SELECT DeptId FROM View_CheckUser)
   GROUP BY  LastUpdateTime ,deptid
  ", courseid);

                return connection.Query<Report_OnLineTest>(querystr).ToList();
            }
        }

        /// <summary>
        /// 各月份学习情况分布图
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public List<Report_OnLineTest> GetMonthDeptByCourseId(int courseid, string deptIDs = "")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string querystr = string.Format(@"  
    
  select datepart(month,LastUpdateTime) as monthstr,
  (select dbo.GetDeptPathByDeptID(sys_user.deptid)) as DeptName,sys_user.deptid DeptId,sum(clv.LearnTimes)    LearnTimes
   from Cl_CpaLearnStatus 
	left join sys_user on Cl_CpaLearnStatus.userid=sys_user.userid
	LEFT JOIN Cl_LearnVideoInfor clv ON clv.LearnId= Cl_CpaLearnStatus.Id
  where courseid={0}  AND CpaFlag=0 and
  sys_user.deptid IN (SELECT DeptId FROM View_CheckUser)
  and sys_user.deptid IN ({1}) 
   GROUP BY  LastUpdateTime ,deptid
  ", courseid, deptIDs);

                return connection.Query<Report_OnLineTest>(querystr).ToList();
            }
        }
    }
}
