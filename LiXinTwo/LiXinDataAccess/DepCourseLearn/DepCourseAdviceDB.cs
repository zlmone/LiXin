using System.Data;
using LiXinModels.CourseManage;
using LiXinModels.DepCourseManage;
using Retech.Core;
using Retech.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.DepCourseLearn;

namespace LiXinDataAccess.DepCourseLearn
{
    public class DepCourseAdviceDB : BaseRepository
    {

        /// <summary>
        /// 添加课前建议
        /// </summary>
        /// <param name="model"></param>
        public void SubmitCl_CourseAdvice(Dep_CourseAdvice model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "insert into Dep_CourseAdvice (CourseId,UserId,AdviceContent,AdviceTime,VisibleFlag,IsDelete) values(@CourseId,@UserId,@AdviceContent,@AdviceTime,@VisibleFlag,@IsDelete)";
                var param = new
                {
                    model.CourseId,
                    model.UserId,
                    model.AdviceContent,
                    model.AdviceTime,
                    model.VisibleFlag,
                    model.IsDelete

                };
                decimal id = connection.Query<decimal>(sql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);

            }
        }
        /// <summary>
        /// 获取课前建议信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<Dep_CourseAdvice> GetList(string strWhere = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                if (strWhere == "")
                {
                    strWhere = "1=1";
                }
                string selectSql =string.Format(@"SELECT tab.*,sys_user.Realname as userrealname,sys_user.photourl as userphotourl 
FROM (select Dep_CourseAdvice.*,sys_user.Realname AS TeacherName 
      from Dep_CourseAdvice 
      left join Dep_Course on Dep_Course.Id = Dep_CourseAdvice.CourseId 
      left join sys_user on  sys_user.userID = Dep_Course.Teacher 
      where {0} and Dep_CourseAdvice.IsDelete = 0
      ) as tab 
left join sys_user on  sys_user.userID = tab.UserId ", strWhere);
                return connection.Query<Dep_CourseAdvice>(selectSql).ToList();
            }
        }

        /// <summary>
        /// 我的课前建议
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Dep_CourseShow> GetMyproposeList(out int totalCount, int UserId, string where = " 1 = 1 ", string num = " 1=1", int startIndex = 0, int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string sql = string.Format(@"
select count(1) from (
select * from 
( 
	SELECT * FROM
	(
	SELECT        
		Dep_CourseOrder.Id 
		,Dep_Course.CourseName
        ,Dep_Course.Way
        ,Dep_Course.CourseLength
		,Dep_Course.IsTest
        ,Dep_Course.IsMust
        ,Dep_Course.StartTime 
        ,Dep_Course.EndTime 
		,Dep_Course.NumberLimited as NumberLimited			
		,Dep_CourseOrder.PassStatus		
		,Dep_CourseOrder.CourseId as CourseId
		,Dep_CourseOrder.UserId		
        ,(select count(1) from Dep_CourseAdvice where Dep_CourseAdvice.CourseId=Dep_CourseOrder.CourseId and Dep_CourseAdvice.UserId=Dep_CourseOrder.UserId  and  Dep_CourseAdvice.IsDelete=0) as AdviceCount
		,Dep_Course.StartTime as CourseStartTime
		,Dep_Course.EndTime as CourseEndTime	
        ,Sys_User.Realname as Realname		    
	FROM Dep_CourseOrder left join Dep_Course on Dep_CourseOrder.CourseId=Dep_Course.ID
						left join Sys_User on Dep_Course.Teacher=Sys_User.UserId			
						left join dbo.Dep_Coursepaper on Dep_Course.id=Dep_Coursepaper.CourseId								
	WHERE " + where + @"  and Dep_Course.isdelete=0
	 AND   ((Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 0 )
            or (Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 1 and Dep_CourseOrder.approvalflag in (0,2)) 
            or (Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 1 and Dep_CourseOrder.approvalflag = 1 ) 
            or Dep_CourseOrder.orderstatus = 3) and Dep_CourseOrder.UserId={0}
) bb   
)DD where " + num + @"
)AA 
 
", UserId);


                totalCount = connection.Query<int>(sql).First();

                string query = string.Format(@"


select top({0}) * from (
select *,row_number() over( ORDER BY DD.id DESC) as rowNum from 
( 
	SELECT * FROM
	(
	SELECT   
       
		Dep_CourseOrder.Id 
		,Dep_Course.CourseName
        ,Dep_Course.Way
        ,Dep_Course.CourseLength
		,Dep_Course.IsTest
        ,Dep_Course.IsMust
        ,Dep_Course.StartTime 
        ,Dep_Course.EndTime 
		,Dep_Course.NumberLimited as NumberLimited			
		,Dep_CourseOrder.PassStatus		
		,Dep_CourseOrder.CourseId as CourseId
		,Dep_CourseOrder.UserId		
        ,(select count(1) from Dep_CourseAdvice where Dep_CourseAdvice.CourseId=Dep_CourseOrder.CourseId and Dep_CourseAdvice.UserId=Dep_CourseOrder.UserId  and  Dep_CourseAdvice.IsDelete=0) as AdviceCount
		,Dep_Course.StartTime as CourseStartTime
		,Dep_Course.EndTime as CourseEndTime	
        ,Sys_User.Realname as Realname		    
	FROM Dep_CourseOrder left join Dep_Course on Dep_CourseOrder.CourseId=Dep_Course.ID
						left join Sys_User on Dep_Course.Teacher=Sys_User.UserId			
						left join dbo.Dep_Coursepaper on Dep_Course.id=Dep_Coursepaper.CourseId			
	WHERE " + where + @" and Dep_Course.isdelete=0
	AND   ((Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 0 )
            or (Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 1 and Dep_CourseOrder.approvalflag in (0,2)) 
            or (Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 1 and Dep_CourseOrder.approvalflag = 1 ) 
            or Dep_CourseOrder.orderstatus = 3) and Dep_CourseOrder.UserId={1}
) bb
)DD where " + num + @"
)AA WHERE  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", pageLength, UserId);



                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Dep_CourseShow>(query, parameters).ToList();


            }
        }


        /// <summary>
        /// 我的课后评估
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <returns></returns>
        public List<Dep_CourseShow> GetMyAfterCourseList(out int totalCount, int UserId, string TrainGrade = "", int DeptId = 0, string where = " 1 = 1 ", string ALLnum = "", int startIndex = 0,
                                                    int pageLength = int.MaxValue)
        {
            using (IDbConnection connection = OpenConnection())
            {

                string querystr = string.Format(@"
select count(1) from (
 SELECT * FROM      
(
      select * from
(
select 
Dep_Course.ID
		,Dep_Course.CourseName
        ,Dep_Course.Way  
        ,Dep_Course.StartTime
        ,Dep_Course.EndTime
        ,Dep_Course.NumberLimited as NumberLimited
        ,Dep_CourseOrder.GetScore as GetScore
        ,Dep_CourseOrder.PassStatus
        ,(select count(1) from Dep_Survey_ReplyAnswer where [ObjectID]=Dep_Course.ID and [UserID]={0} and [Status]=1) as ALLnum
 from dbo.Dep_Course left join dbo.Dep_CourseOrder on Dep_Course.Id=Dep_CourseOrder.CourseId
  where  {3} and Dep_CourseOrder.UserId={0} and IsPing=1 and isdelete=0 and (
   (

   ((Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 0 )
            or (Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 1 and Dep_CourseOrder.approvalflag in (0,2)) 
            or (Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 1 and Dep_CourseOrder.approvalflag = 1) 
            or Dep_CourseOrder.orderstatus = 3) and Dep_CourseOrder.UserId={0} 
   )
   or
   (	
	(Dep_Course.IsDelete=0 and Dep_Course.Publishflag=1 and Dep_Course.CourseFrom=2 )
		and (
		
			(Dep_Course.OpenFlag=0 and charindex('{1}',Dep_Course.OpenLevel)>0)		
			or   (
					Dep_Course.OpenFlag=1 and
					charindex('{1}',Dep_Course.OpenLevel)>0 and
			     	'{0}' in(select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(Dep_Course.OpenGroupObject)))
				  )
		   or     (
					Dep_Course.OpenFlag=2 and 
					 charindex('{1}',Dep_Course.OpenLevel)>0 and charindex(('{2}'),(Dep_Course.OpenDepartObject))>0
				  )
		   or
				(
					charindex('{1}',Dep_Course.OpenLevel)>0 and Dep_Course.OpenFlag=3 and(
					'{0}' in(select UserId from Sys_GroupUser  where GroupId  in (select id from dbo.F_SplitIDs(Dep_Course.OpenGroupObject)))
				 	or charindex(('{2}'),(Dep_Course.OpenDepartObject))>0)
				) 
			)
   )
)
)bb )DD where {4}
)aa
", UserId, TrainGrade, DeptId, where, ALLnum);
                totalCount = connection.Query<int>(querystr).First();


                string query = string.Format(@"
select top({3})  * from (
 SELECT *, row_number() over( ORDER BY DD.id desc) as rowNum
FROM      
( 
select  * from
( 
select 
Dep_Course.ID
		,Dep_Course.CourseName
        ,Sys_User.Realname as TeacherName
        ,Dep_Course.Way 
        ,Dep_Course.Sort
        ,Dep_Course.CourseLength
        ,Dep_Course.StartTime
        ,Dep_Course.EndTime
        ,Dep_Course.NumberLimited as NumberLimited
        ,Dep_CourseOrder.GetScore as GetScore
        ,Dep_CourseOrder.PassStatus
        ,(select count(1) from Dep_Survey_ReplyAnswer where [ObjectID]=Dep_Course.ID and [UserID]={0} and [Status]=1) as ALLnum
 from dbo.Dep_Course left join dbo.Dep_CourseOrder on Dep_Course.Id=Dep_CourseOrder.CourseId
                    left join Sys_User on Sys_User.UserId=Dep_Course.Teacher
  where  {4} and Dep_CourseOrder.UserId={0} and IsPing=1 and Dep_Course.isdelete=0 and (
   (

  ((Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 0 )
            or (Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 1 and Dep_CourseOrder.approvalflag in (0,2)) 
            or (Dep_CourseOrder.orderstatus = 1 and Dep_CourseOrder.isleave = 1 and Dep_CourseOrder.approvalflag = 1) 
            or Dep_CourseOrder.orderstatus = 3) and Dep_CourseOrder.UserId={0} 
   )
   or
   (	
	(Dep_Course.IsDelete=0 and Dep_Course.Publishflag=1 and Dep_Course.CourseFrom=2 )
		and (
		
			(Dep_Course.OpenFlag=0 and charindex('{1}',Dep_Course.OpenLevel)>0)		
			or   (
					Dep_Course.OpenFlag=1 and
					charindex('{1}',Dep_Course.OpenLevel)>0 and
			     	'{0}' in(select UserId from Sys_GroupUser  where GroupId in (select id from dbo.F_SplitIDs(Dep_Course.OpenGroupObject)))
				  )
		   or     (
					Dep_Course.OpenFlag=2 and 
					 charindex('{1}',Dep_Course.OpenLevel)>0 and charindex(('{2}'),(Dep_Course.OpenDepartObject))>0
				  )
		   or
				(
					charindex('{1}',Dep_Course.OpenLevel)>0 and Dep_Course.OpenFlag=3 and(
					'{0}' in(select UserId from Sys_GroupUser  where GroupId  in (select id from dbo.F_SplitIDs(Dep_Course.OpenGroupObject)))
				 	or charindex(('{2}'),(Dep_Course.OpenDepartObject))>0)
				) 		)	
   )
)
)bb 
)DD where {5}
)aa
where  rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)
", UserId, TrainGrade, DeptId, pageLength, where, ALLnum);

                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Dep_CourseShow>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 获取我的部门学时
        /// </summary>
        /// <returns></returns>
        public decimal GetMyTotalScore(int userID,int year=-1)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select 
case when(select count(*) from Dep_CourseOrder where UserId={0})>0 OR (SELECT COUNT(*) FROM Cl_CourseOrder WHERE UserId={0})>0 
then (
  SELECT sum(Score)  FROM(
 select isnull(sum(AttScore+ExamScore+EvlutionScore),0) Score from Dep_CourseOrder
  left join Dep_Attendce on Dep_Attendce.CourseId=Dep_CourseOrder.courseid and Dep_Attendce.UserId={0}
LEFT JOIN Dep_Course cc ON cc.Id=Dep_CourseOrder.CourseId
 where Dep_CourseOrder.UserId={0} and Dep_Attendce.ApprovalFlag=1 {1}
    UNION
    SELECT sum(GetScore)  Score FROM View_DepCourseLearn WHERE UserId={0}  {1}
    ) t)
else 0
end", userID, (year == -1 ? "" : "AND YearPlan=" + year));
                var score= conn.Query<decimal>(sql);
                return score == null ? 0 : score.FirstOrDefault();
            }
        }

        /// <summary>
        /// 回复课前建议
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ReplyCl_CourseAdvice(Dep_CourseAdvice model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "update Dep_CourseAdvice set AnserUserId=@AnserUserId,AnserContent=@AnserContent,AnserTime=@AnserTime,VisibleFlag=@VisibleFlag where CourseId=@CourseId and Id=@Id ";
                var param = new
                {
                    model.AnserUserId,
                    model.AnserContent,
                    model.AnserTime,
                    model.CourseId,
                    model.VisibleFlag,
                    model.Id
                };

                int result = connection.Execute(sql, param);
                return result > 0;
            }
        }
    }
}
