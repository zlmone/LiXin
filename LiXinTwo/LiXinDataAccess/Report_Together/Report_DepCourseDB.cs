using LiXinModels.DepCourseManage;
using Retech.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Retech.Data;
using System.Data;
using LiXinModels.Report_Together;
using LiXinModels.Report_Vedio;
using LiXinModels.Report_zVedio;


namespace LiXinDataAccess.Report_Together
{
    public class Report_DepCourseDB : BaseRepository
    {
        /// <summary>
        /// 查找所有分所
        /// </summary>
        /// <returns></returns>
        public List<Report_DepCourse> GetReport_Complete(int year, string deptid)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string sql = string.Format(@"select dbo.GetDeptPathByDeptID(sd.DepartmentId) as Dep_DeptName
    ,dbo.GetDeptTwoPathByDeptID(sd.DepartmentId) Dep_TwoDeptName,sd.DeptName,
	View_CheckUser.deptid,Dep_YearPlan.IsDelete as IsDelete,Dep_YearPlan.PublishFlag as PublishFlag,
	Dep_YearPlan.year, Dep_YearPlan.isopen,
	(select deptid from  Dep_YearPlan where id=Dep_LinkDepart.yearid and approvalflag=1   and year={0}) as LinkDepart_Deptid
	  from View_CheckUser
	left join Dep_YearPlan on  Dep_YearPlan.deptid=View_CheckUser.deptid and Dep_YearPlan.IsDelete=0 and Dep_YearPlan.year={0}
	left join Dep_LinkDepart on Dep_LinkDepart.deptid=View_CheckUser.deptid and Dep_LinkDepart.ApprovalFlag=1
	LEFT JOIN Sys_Department sd ON sd.DepartmentId= View_CheckUser.DeptId
	left join  Sys_Department on Sys_Department.DepartmentId=Dep_YearPlan.DeptId and Dep_YearPlan.year={0} {1} 
 group by View_CheckUser.deptid,Dep_YearPlan.IsDelete,Dep_YearPlan.PublishFlag,Dep_YearPlan.year,
 Dep_YearPlan.isopen,Dep_LinkDepart.yearid,Dep_LinkDepart.approvalflag ,sd.DeptName,sd.DepartmentId  ", year, deptid);

                return connection.Query<Report_DepCourse>(sql).ToList();
            }
        }




        public List<Report_DepCourse> GetCheckUserList(string where)
        {
            using (var conn = OpenConnection())
            {
                var sql = @" 
	select dep_courseorder.userid,sys_user.Username,sys_user.realname as RealName,sys_user.deptpath as Dep_DeptName,sys_user.cpa as IsCPAStr,dep_course.yearplan as yearplan,sys_user.deptid from dep_courseorder 
		left join sys_user on sys_user.userid=dep_courseorder.userid
        left join dep_course on dep_course.id=dep_courseorder.courseid
	 where  dep_courseorder.userid in (select userid from View_CheckUser) " + where + @"
	  group by dep_courseorder.userid,sys_user.Username,sys_user.realname,sys_user.deptpath,sys_user.cpa,dep_course.yearplan,sys_user.deptid ";
                return conn.Query<Report_DepCourse>(sql).ToList();
            }
        }


        public List<Report_DepCourse> GetReport_Teachers(string where = "")
        {
            using (var conn = OpenConnection())
            {
                var sql = @" 
	select count(teacher) as courseTeacherCount,
		sum(CourseLength) as All_CourseLength,
		dep_Course.Teacher as TeacherId,
		Sys_User.realname as RealName,
		sys_user.DeptName as Dep_DeptName,
		sys_user.PayGrade as PayGrade,
		sys_user.TrainGrade as TrainGrade,
        dep_Course.yearplan as yearplan,
        sys_user.deptid as deptid,
        sys_User.IsTeacher as IsTeacher,
		sys_user.CPA as IsCPA from dep_Course
	left join sys_user on sys_user.userid=dep_Course.Teacher
  where dep_Course.isdelete=0 and dep_Course.Publishflag=1  and way=1
   and starttime<getdate() and CourseFrom=2 " + where + @"
	  group by teacher,Sys_User.realname,sys_user.DeptName,sys_user.PayGrade,sys_user.TrainGrade,sys_user.CPA
	  ,dep_Course.yearplan,sys_user.deptid,sys_User.IsTeacher";
                return conn.Query<Report_DepCourse>(sql).ToList();
            }
        }


        public List<Report_DepCourse> GetPayGrade(string where)
        {
            using (var conn = OpenConnection())
            {
                var sql = @" 
	select
		sys_user.PayGrade as PayGrade
		 from dep_Course
	left join sys_user on sys_user.userid=dep_Course.Teacher
  where dep_Course.isdelete=0 and dep_Course.Publishflag=1
   and starttime<getdate() and CourseFrom=2 " + where + @"
	  group by sys_user.PayGrade";
                return conn.Query<Report_DepCourse>(sql).ToList();
            }
        }

        /// <summary>
        /// 所有人员学过的课程所获得的学时
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetAvgCourseLength(int year, int deptMaxScore = 0,string userIDs="")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"DECLARE @deptMaxScore INT ={0}
SELECT  deptid,sum(Avg_GetLength) Avg_GetLength,count(userid) usercount   FROM(
SELECT     userid,deptid,
CASE WHEN Avg_GetLength>@deptMaxScore and @deptMaxScore!=-1 THEN @deptMaxScore ELSE Avg_GetLength END Avg_GetLength FROM (
SELECT  userid,deptid,sum(Avg_GetLength)   Avg_GetLength FROM(
 select DISTINCT (attscore+EvlutionScore+ExamScore) as Avg_GetLength,Dep_courseorder.CourseId,
	Dep_courseorder.userid as userid,
	--Dep_CourseDept.DepartId as deptid,
    sys_user.deptid as deptid
	 from Dep_courseorder 
	left join dep_course on Dep_courseorder.courseid=dep_course.id
	left join Dep_CourseDept on Dep_CourseDept.courseid=Dep_courseorder.courseid
    left join sys_user on sys_user.userid=Dep_courseorder.userid
 where Dep_courseorder.userid in (select userid from View_CheckUser)
  and dep_course.isdelete=0	and dep_course.yearplan={1} and {2}
) t GROUP BY  userid,deptid
) r )result  GROUP BY  deptid
", deptMaxScore, year, userIDs == "" ? " 1=1" : " sys_user.UserId NOT IN (" + userIDs + ")");
                return conn.Query<Report_DepCourse>(sql).ToList();
            }
        }




        /// <summary>
        /// 统计一门课程下所有部门
        /// </summary>
        /// <param name="coureid"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetSingleCoursePation(int coureid, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = @" 
select Cl_CourseOrder.courseid,
	  --dep_courseorder.userid,
	  count(Cl_CourseOrder.userid) as usernum,
	  sys_user.deptid,
      dbo.GetDeptTwoPathByDeptID(sys_user.deptid) DeptName, 
	  cc.StartTime,
	   cc.EndTime
 from Cl_CourseOrder 
	left join sys_user on Cl_CourseOrder.userid=sys_user.userid
	left join co_course cc on Cl_CourseOrder.courseid=cc.id
 where Cl_CourseOrder.userid IN (SELECT UserId FROM View_CheckUser) 
AND sys_user.SubordinateSubstation='上海总部'
 and courseid=" + coureid + @" and " + where + @"
 group by sys_user.deptid,courseid,deptpath,cc.StartTime,cc.EndTime
";
                return conn.Query<Report_DepCourse>(sql).ToList();
            }
        }


        /// <summary>
        /// 根据课程ID查出学时和考勤记录，预定状态
        /// </summary>
        /// <param name="coureid"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetSingleCourseAndAttend(int coureid)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"  
 select Cl_CourseOrder.userid as UserId,
		Cl_CourseOrder.GetScore as AttScore,		
		--isnull(Cl_Attendce.Status,0) as Att_Status,
		Cl_CourseOrder.OrderStatus as OrderStatus,
		Cl_CourseOrder.IsLeave as IsLeave,
		Cl_CourseOrder.ApprovalFlag as ApprovalFlag,
        sys_user.deptid as DeptId,
        Cl_Attendce.Id attID,
        Cl_Attendce.starttime as Attend_StartTime,
        Cl_Attendce.endtime as Attend_EndTime,
        (select dbo.F_NewIsAppStatus(Cl_CourseOrder.courseid,sys_user.userid,Cl_CourseOrder.OrderStatus)) as Bu_ApprovalFlag
        , OrderTime
        , LeaveTime
        , Cl_CourseOrder.ApprovalDateTime
        ,Cl_CourseOrder.ApprovalLimitTime
        ,cc.AttFlag
  from Cl_CourseOrder
    LEFT JOIN Co_Course cc ON cc.Id=Cl_CourseOrder.CourseId
	left join Cl_Attendce on Cl_CourseOrder.userid=Cl_Attendce.userid and Cl_CourseOrder.courseid=Cl_Attendce.courseid
    left join sys_user on sys_user.userid=Cl_CourseOrder.userid
  where  Cl_CourseOrder.UserId IN (SELECT UserId FROM View_CheckUser) AND Cl_CourseOrder.courseid=" + coureid;
                return conn.Query<Report_DepCourse>(sql).ToList();
            }


        }

        /// <summary>
        /// 查处单门课程 所有部门可报名人数
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetSingleCourse_OrderNum(int courseid)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"  
 SELECT Deptid as DeptId,
        count(UserId) as Pation_OrderNum
from dbo.F_GetTogtherSigle(" + courseid + @")
                WHERE UserId IN (SELECT UserId FROM View_CheckUser) 
                GROUP BY Deptid";
                return conn.Query<Report_DepCourse>(sql).ToList();
            }
        }

        /// <summary>
        /// 转播课程
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="deptwhere"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetRTCourse(int courseID, string deptwhere = " 1=1", string where = " 1=1", string timewhere = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
SELECT dcor.UserId,dcor.OrderStatus,dcor.DepartSetId,syu.DeptId, dbo.GetDeptTwoPathByDeptID(syu.DeptId) DeptName,
 GetSumScore=AttScore+EvlutionScore+ExamScore,dco.ApprovalFlag,cc.SurveyPaperId,
 dcor.CourseId,t1.Enter_OrderNum,t2.Actual_OrderNum, 
 Pation_OrderNum=(SELECT count(1) FROM DepTran_DepartUser WHERE DepartSetId=dcor.DepartSetId) ,OrderTime
 FROM DepTran_CourseOrder  dcor
 LEFT JOIN   DepTran_CourseOpen dco ON dco.CourseId=dcor.CourseId AND dcor.DepartSetId=dco.DepartId
 LEFT JOIN Co_Course cc ON cc.Id=dcor.CourseId
 LEFT JOIN Sys_User syu ON syu.UserId=dcor.UserId
 LEFT JOIN 
(
  --实际报名人数
  SELECT syu.DeptId,count(dc.UserId) Enter_OrderNum FROM DepTran_CourseOrder dc
  LEFT JOIN Sys_User syu ON syu.UserId=dc.UserId
  WHERE   OrderStatus in (1,2) and dc.CourseId={0} and  {1} and {2}
  GROUP BY syu.DeptId  
) t1 ON   t1.DeptId=syu.DeptId
LEFT JOIN 
(
  --实际参与课程的人数     
  SELECT syu.DeptId,count(da.UserId) Actual_OrderNum  FROM DepTran_Attendce da
  LEFT JOIN Sys_User syu ON syu.UserId=da.UserId
  WHERE da.Status<>0 AND da.Status<>2  AND da.CourseId={0} and {1}
  AND  da.UserId IN (SELECT UserId FROM View_CheckUser)
  GROUP BY syu.DeptId  
) t2 ON  t2.DeptId=syu.DeptId
WHERE  dcor.CourseId={0} and {1}", courseID, deptwhere, timewhere);
                return conn.Query<Report_DepCourse>(sql).ToList();
            }
        }

        /// <summary>
        /// 转播的考勤
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<Report_DepCourse> GetRTAttend(int courseID)
        {
            using (var conn = OpenConnection())
            {
                var sql = @"SELECT CourseId,UserId,Status Att_Status FROM DepTran_Attendce
WHERE ApprovalFlag=1 and CourseId=" + courseID;
                return conn.Query<Report_DepCourse>(sql).ToList();
            }
        }


        #region 2014-1-2 毛佳源添加 统计每个月开课部门的数量
        /// <summary>
        /// 实际开课数据
        /// </summary>
        /// <returns></returns>
        public List<Report_DepCourse> GetDep_Course_DeptNumFor_ShiJi(int yearid, string deptIDWhere = " 1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string query = string.Format(@"     select * from ( 
             select  (select dbo.GetDeptPathByDeptID(DepartmentId)) as deptname,
                month(starttime) as month,dep_course.CourseLength as CourseLength,dep_course.deptid as DeptId,2 as code
                from dep_course
					left join  Sys_Department on Sys_Department.DepartmentId=dep_course.DeptId   
                 where deptid in(select deptid from View_CheckUser) and dep_course.isdelete=0 and yearplan=" + yearid + @"
                and CourseFrom=2 --and month(StartTime) =9 -- group by DeptId,dep_course.id,dep_course.CourseLength
                  )t where deptname like '%上海%' and deptid in(SELECT deptid FROM View_CheckUser) and {0}", deptIDWhere);

                return connection.Query<Report_DepCourse>(query).ToList();
            }
        }

        /// <summary>
        /// 年度计划开课
        /// </summary>
        /// <returns></returns>
        public List<Report_DepCourse> GetDep_Course_DeptNumFor_YearPlan(int yearid, string deptIDWhere = " 1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string query = string.Format(@"

	WITH mao as
			(
			SELECT Dep_YearPlan.Id AS YearId ,Dep_YearPlan.DeptId AS MainDeptid,Dep_LinkDepart.DeptId AS ChildDeptid  FROM Dep_YearPlan 
			  LEFT JOIN Dep_LinkDepart ON Dep_YearPlan.Id=Dep_LinkDepart.YearId
			  	WHERE Dep_YearPlan.IsOpen=1 AND Dep_YearPlan.PublishFlag=1 and Dep_YearPlan.IsDelete=0
			  		AND Dep_LinkDepart.ApprovalFlag=1
			  		)
		   				  		
		   SELECT * FROM(
			SELECT		   
			 Dep_Course.Month AS monthstr,
			mao.MainDeptid AS deptid,
			(select dbo.GetDeptPathByDeptID(mao.ChildDeptid)) AS DeptName,
			mao.ChildDeptid FROM  mao
				LEFT JOIN Dep_YearPlanCourse ON mao.YearId=Dep_YearPlanCourse.YearId
				LEFT JOIN Dep_Course ON Dep_Course.Id=Dep_YearPlanCourse.CourseId
				LEFT JOIN Sys_Department ON Sys_Department.DepartmentId=mao.ChildDeptid
                WHERE Dep_Course.YearPlan=" + yearid + @"
			UNION 
			
			SELECT 			
			Dep_Course.Month AS monthstr,			
			Dep_Course.DeptId AS deptid,
			(select dbo.GetDeptPathByDeptID(Sys_Department.DepartmentId)) AS DeptName,
			0			
			FROM Dep_Course
				LEFT JOIN Sys_Department ON Sys_Department.DepartmentId=Dep_Course.DeptId
				WHERE 
             id in (
            select courseid from dbo.Dep_YearPlanCourse
            where yearid in (
            select id from Dep_YearPlan where publishflag=1 and IsDelete=0)
            and Dep_Course.YearPlan=" + yearid + @"
             group by courseid)
               group by  Dep_Course.Month,deptid,DeptName,DepartmentId
               )t WHERE deptname LIKE '%上海%' and deptid in(SELECT deptid FROM View_CheckUser)
               and {0}", deptIDWhere);

                return connection.Query<Report_DepCourse>(query).ToList();
            }
        }


        public List<Report_DepCourse> GetDep_Course_DeptNumFor_YearPlan_Length(int yearid, string deptIDWhere = " 1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string query = string.Format(@"

	WITH mao as
			(
			SELECT Dep_YearPlan.Id AS YearId ,Dep_YearPlan.DeptId AS MainDeptid,Dep_LinkDepart.DeptId AS ChildDeptid  FROM Dep_YearPlan 
			  LEFT JOIN Dep_LinkDepart ON Dep_YearPlan.Id=Dep_LinkDepart.YearId
			  	WHERE Dep_YearPlan.IsOpen=1 AND Dep_YearPlan.PublishFlag=1 and Dep_YearPlan.IsDelete=0
			  		AND Dep_LinkDepart.ApprovalFlag=1
			  		)
		   				  	  
		   SELECT a.CourseLength,Sys_Department.DeptName FROM (	
		   SELECT sum(CourseLength) AS CourseLength,deptid FROM(
			SELECT			
			--mao.MainDeptid AS deptid,
			(select dbo.GetDeptPathByDeptID(mao.ChildDeptid)) AS DeptName,
			mao.ChildDeptid AS deptid,
			sum(Dep_Course.CourseLength) AS CourseLength
			 FROM  mao
				LEFT JOIN Dep_YearPlanCourse ON mao.YearId=Dep_YearPlanCourse.YearId
				LEFT JOIN Dep_Course ON Dep_Course.Id=Dep_YearPlanCourse.CourseId
				LEFT JOIN Sys_Department ON Sys_Department.DepartmentId=mao.ChildDeptid
                WHERE Dep_Course.YearPlan=" + yearid + @"
				group by  Dep_Course.Month,deptid,DeptName,DepartmentId,MainDeptid,ChildDeptid
			UNION 
			
			SELECT 	   	
			(select dbo.GetDeptPathByDeptID(Sys_Department.DepartmentId)) AS DeptName,	
			Dep_Course.DeptId AS deptid,		
			sum(Dep_Course.CourseLength) AS CourseLength
				FROM Dep_Course
					LEFT JOIN Sys_Department ON Sys_Department.DepartmentId=Dep_Course.DeptId
					WHERE 
		             id in (
				            select courseid from dbo.Dep_YearPlanCourse
				            where yearid in (
				            select id from Dep_YearPlan where publishflag=1 and IsDelete=0)
				             group by courseid
		             		)
                       and Dep_Course.YearPlan=" + yearid + @"
               				group by deptid,DeptName,DepartmentId
               
               )t WHERE --month LIKE '%1%' AND
                deptname LIKE '%上海%'
                  
                GROUP BY DeptId--,DeptName,ChildDeptid,CourseLength
                ) a 
                LEFT JOIN Sys_Department
                 ON Sys_Department.DepartmentId=a.deptid
                    where deptid in(SELECT deptid FROM View_CheckUser) and {0}
", deptIDWhere);

                return connection.Query<Report_DepCourse>(query).ToList();
            }
        }



        public List<Report_DepCourse> GetDep_Course_DeptNumFor_ShiJi_Length(int yearid, string deptIDWhere = " 1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string query = string.Format(@"     select * from ( 
             select (select dbo.GetDeptPathByDeptID(DeptId)) as deptname,
                sum(dep_course.CourseLength) as CourseLength,
                dep_course.deptid as DeptId            
                from dep_course
					left join  Sys_Department on Sys_Department.DepartmentId=dep_course.DeptId   
                 where deptid in(select deptid from View_CheckUser) and dep_course.isdelete=0 and yearplan=" + yearid + @"
                and CourseFrom=2 AND dep_course.IsDelete=0
                AND Publishflag=1 AND (Way=1 OR Way =3)  group by DeptId
                  )t where deptname like '%上海%' and deptid in(SELECT deptid FROM View_CheckUser) and {0}", deptIDWhere);

                return connection.Query<Report_DepCourse>(query).ToList();
            }
        }

        #endregion



        /// <summary>
        /// 每个部门分所应参加的人数
        /// </summary>
        /// <returns></returns>
        public List<fVedioJoinNumber> GetDepCourseUser()
        {
            using (var conn = OpenConnection())
            {
                var sql = @"EXEC PROC_GetDepCourseUser";
                return conn.Query<fVedioJoinNumber>(sql).ToList();
            }
        }

        #region 讲师课程评估
        /// <summary>
        /// 讲师上课的课程
        /// </summary>
        /// <param name="teacher"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Dep_Course> GetDepCourseTeacher(int teacher, string where = " 1=1")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT Id, CourseName, IsMust, StartTime, EndTime, SurveyPaperId,IsPing
,dbo.f_GetDepCourseOpenObject(Id) OpenObject FROM Dep_Course
WHERE  Publishflag=1 AND  way=1 AND IsDelete=0 AND Teacher='{0}' and {1}", teacher, where);
                return conn.Query<Dep_Course>(sql).ToList();
            }
        }

        /// <summary>
        /// 讲师课程评估
        /// </summary>
        /// <returns></returns>
        public List<UserSurvey> GetDepSurvey()
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"WITH info AS 
(
  SELECT  ObjectID,ExampaperID,QuestionID,round(avg(CONVERT(decimal,SubjectiveAnswer)),2) avg FROM Dep_Survey_ReplyAnswer
  WHERE  SourceFrom=0 AND Status=1 and QuestionID IN (SELECT QuestionID FROM Dep_Survey_Question WHERE QuestionType=3)
  and UserID IN (SELECT UserID FROM dbo.View_CheckUser) 
  AND SubjectiveAnswer IN ('0','1','2','3','4','5')
  AND  ObjectID IN (SELECT Id FROM Dep_Course WHERE Way=1 AND CourseFrom=2) 
  GROUP BY   ObjectID,ExampaperID,QuestionID
)
SELECT ObjectID,ExampaperID,count(DISTINCT QuestionID) questionCount,sum(avg) PingSum
FROM info
GROUP BY ObjectID,ExampaperID");
                return conn.Query<UserSurvey>(sql).ToList();
            }
        }
        #endregion

    }
}
