using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using System.Data;
using LiXinModels.DepTranManage;
using LiXinModels.User;
using LiXinModels.CourseLearn;

namespace LiXinDataAccess.DepTranManage
{
    public class DepTran_CourseOpenDB : BaseRepository
    {
        #region 增改(基本信息)
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertDepTran_CourseOpen(DepTran_CourseOpen model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO DepTran_CourseOpen(CourseId,DepartId,AttFlag,OpenFlag,SubmitTime,ApprovalFlag,UserId)  VALUES (@CourseId,@DepartId,@AttFlag,@OpenFlag,@SubmitTime,@ApprovalFlag,@UserId );SELECT @@IDENTITY as Id ";

                var param = new
                {
                    model.CourseId,
                    model.DepartId,
                    model.AttFlag,
                    model.OpenFlag,
                    model.SubmitTime,
                    model.ApprovalFlag,
                    model.UserId
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.Id);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateDepTran_CourseOpen(DepTran_CourseOpen model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update DepTran_CourseOpen set CourseId = @CourseId,DepartId = @DepartId,AttFlag = @AttFlag,OpenFlag = @OpenFlag,SubmitTime = @SubmitTime,ApprovalFlag=@ApprovalFlag,UserId=@UserId where Id=@Id";

                var param = new
                {
                    model.CourseId,
                    model.DepartId,
                    model.AttFlag,
                    model.OpenFlag,
                    model.SubmitTime,
                    model.ApprovalFlag,
                    model.UserId,
                    model.Id
                };
                int result = conn.Execute(strSql, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        public DepTran_CourseOpen GetDepTran_CourseOpen(int courseID, int departId)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from DepTran_CourseOpen where CourseId=@CourseId and DepartId=@DepartId ");
                var param = new { CourseId = courseID, DepartId = departId };
                return connection.Query<DepTran_CourseOpen>(query, param).FirstOrDefault();
            }
        }
        #endregion

        /// <summary>
        /// 更新审核内容
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <param name="attFlag"></param>
        /// <param name="approvalFlag"></param>
        /// <returns></returns>
        public bool UpDateDepTran_CourseOpen(int courseId, int departId, int attFlag, int approvalFlag)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update DepTran_CourseOpen set AttFlag=@attFlag, ApprovalFlag=@approvalFlag " +
                              " where CourseId=@courseId and DepartId=@departId");
                var param = new
                    {
                        courseId = courseId,
                        departId = departId,
                        attFlag = attFlag,
                        approvalFlag = approvalFlag
                    };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 获得邮件短信发送相关信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public DepTran_CourseOpen CourseName(int courseId, int departId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(
                    "select cc.CourseName from DepTran_CourseOpen dtc left join Co_Course cc on cc.Id=dtc.CourseId where dtc.CourseId=@courseId and dtc.DepartId=@departId");
                var param = new
                    {
                        courseId = courseId,
                        departId = departId
                    };
                return connection.Query<DepTran_CourseOpen>(strSql.ToString(), param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得邮件短信被发送人的信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public DepTran_CourseOpen UserNmae(int courseId, int departId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(
                    "select dtc.UserId ,su.* from DepTran_CourseOpen dtc left join Sys_User su on su.UserId=dtc.UserId where CourseId=@courseId and DepartId=@departId");
                var param = new
                {
                    courseId = courseId,
                    departId = departId
                };
                return connection.Query<DepTran_CourseOpen>(strSql.ToString(), param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 视频转播课程预定查询 
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="coursename"></param>
        /// <param name="teachername"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        public List<DepTran_CourseOpen> GetList(out int totalCount,int deptid, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " Id")
        {
            using (IDbConnection connection = OpenConnection())
            {
                string count = string.Format(@" 
select count(1)  from(
              
  SELECT row_number() over( {1}) as rowNum, 
	Co_Course.id as CourseId,
	Co_Course.CourseName as CourseName,
	Co_Course.IsMust,
	Co_Course.StartTime as CourseStartTime,
	Co_Course.EndTime as CourseEndTime,
	Co_Course.CourseLength as CourseLength,
	Sys_ClassRoom.RoomName as RoomName ,
	Sys_user.realname as TeacherName,
    DepTran_DepartLeaderConfig.DepartSetName as DepartSetName
   from DepTran_CourseOpen 
  left join Co_Course on DepTran_CourseOpen.courseid=Co_Course.id
  left join Sys_user on Co_Course.Teacher=Sys_user.UserId  
  left join Sys_ClassRoom on Sys_ClassRoom.id=Co_Course.roomid
   left join DepTran_DepartLeaderConfig on DepTran_DepartLeaderConfig.Id=DepTran_CourseOpen.DepartId
  where {0} and DepTran_CourseOpen.OpenFlag=1 and Co_Course.isdelete=0
and DepTran_CourseOpen.DepartId in (
	SELECT id FROM DepTran_DepartLeaderConfig where charindex('{2}',MainUserIDs)>0
	
  )
) bb
", where, jsRenderSortField, deptid);
                totalCount = connection.Query<int>(count).First();

                string query = string.Format(@" 
select *  from(
              
  SELECT row_number() over( {1}) as rowNum, 
	Co_Course.id as CourseId,
    DepTran_CourseOpen.DepartId as DepartId,
	Co_Course.CourseName as CourseName,
	Co_Course.IsMust as IsMust,
	Co_Course.StartTime as CourseStartTime,
	Co_Course.EndTime as CourseEndTime,
	Co_Course.CourseLength as CourseLength,
	Sys_ClassRoom.RoomName as RoomName,
	Sys_user.realname as TeacherName,
    DepTran_DepartLeaderConfig.DepartSetName as DepartSetName
   from DepTran_CourseOpen 
  left join Co_Course on DepTran_CourseOpen.courseid=Co_Course.id
  left join Sys_user on Co_Course.Teacher=Sys_user.UserId  
  left join Sys_ClassRoom on Sys_ClassRoom.id=Co_Course.roomid
  left join DepTran_DepartLeaderConfig on DepTran_DepartLeaderConfig.Id=DepTran_CourseOpen.DepartId
  where {0} and DepTran_CourseOpen.OpenFlag=1 and Co_Course.isdelete=0
and DepTran_CourseOpen.DepartId in (
	SELECT id FROM DepTran_DepartLeaderConfig where charindex('{2}',MainUserIDs)>0
	
  )
) bb

 where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", where, jsRenderSortField, deptid);
                var parameters = new
                {
                    pageCount = startLength,
                    pageIndex = startIndex / startLength
                };
                return connection.Query<DepTran_CourseOpen>(query, parameters).ToList();



            }
        }

        public List<Sys_User> GetCourseSubscribeUserList(out int totalCount,int deptid, int courseID, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " UserId")
        {
            using (var conn = OpenConnection())
            {
                var query = string.Format(@" SELECT count(1) FROM( 
 SELECT 
 row_number()OVER(ORDER BY UserId desc ) number,
 count(*)OVER(PARTITION BY null) totalCount,

  * from
 (
	 select sys_user.*,dc.OrderStatus as orderstatus,dc.OrderTime from sys_user left join (select * from DepTran_CourseOrder where courseid={0} ) dc
		on dc.UserId=sys_user.UserId where dc.orderstatus in(1,2)
--and (select DepartSetId from DepTran_DepartUser where charindex(CONVERT(nvarchar(5),sys_user.UserId) ,UserId)>0)={3}
and DepartSetId={3}
 )bb where {1}
 )result
", courseID, where, jsRenderSortField, deptid);

                totalCount = conn.Query<int>(query).First();

                var sql = string.Format(@" SELECT * FROM( 
 SELECT 
 row_number()OVER(ORDER BY UserId desc ) number,
 count(*)OVER(PARTITION BY null) totalCount,

  * from
 (
	 select sys_user.*,dc.OrderStatus as orderstatus,dc.OrderTime from sys_user left join (select * from DepTran_CourseOrder where courseid={0} ) dc
		on dc.UserId=sys_user.UserId where dc.orderstatus in(1,2)
    --and (select DepartSetId from DepTran_DepartUser where charindex(CONVERT(nvarchar(5),sys_user.UserId) ,UserId)>0)={3}
 and DepartSetId={3}
 )bb where {1}
 )result
WHERE  number BETWEEN @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", courseID, where, jsRenderSortField, deptid);
                var param = new
                {
                    pageCount = startLength,
                    pageIndex = startIndex / startLength
                };

                return conn.Query<Sys_User>(sql, param).ToList();
            }
        }




        /// <summary>
        /// 获取报名成功的人员，根据培训级别分
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<NameSubscribeCount> GetSuccessTrainGradeSubscribe(int courseID,int deptid)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"
      SELECT    TrainGrade Name,count(UserId) SubscribeCount FROM(
SELECT dc.UserId,su.TrainGrade FROM DepTran_CourseOrder  dc
LEFT JOIN  Sys_User su    ON dc.UserId=su.UserId
WHERE CourseId={0} AND su.IsDelete=0 and dc.OrderStatus in(1,2)
--and  (select DepartSetId from DepTran_DepartUser where charindex(CONVERT(nvarchar(5),su.UserId) ,UserId)>0)={1}
and DepartSetId={1}
)t GROUP BY   TrainGrade", courseID, deptid);
                return conn.Query<NameSubscribeCount>(sql).ToList();
            }
        }
       
    }
}
