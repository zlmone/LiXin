using LiXinModels.NewCourseManage;
using LiXinModels.Survey;
using Retech.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Retech.Data;
using System.Text;

namespace LiXinDataAccess.NewCourseManage
{
    public class New_CourseRoomRuleDB : BaseRepository
    {

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">实体</param>
        public void AddNew_CourseRoomRule(New_CourseRoomRule model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @"INSERT INTO New_CourseRoomRule (CourseId,RoomId,ClassIDs,TeacherId,PingId,Rules,SeatDetail,Type,StartTime,EndTime,PersonCount,GroupName,IsAttFlag,AfterEvlutionConfigTime) VALUES (@CourseId,@RoomId,@ClassIDs,@TeacherId,@PingId,@Rules,@SeatDetail,@Type,@StartTime,@EndTime,@PersonCount,@GroupName,@IsAttFlag,@AfterEvlutionConfigTime)SELECT @@IDENTITY AS Id";
                var param = new
                                {
                                    model.CourseId,
                                    model.RoomId,
                                    model.ClassIDs,
                                    model.TeacherId,
                                    model.PingId,
                                    model.Rules,
                                    model.SeatDetail,
                                    model.Type,
                                    model.StartTime,
                                    model.EndTime,
                                    model.PersonCount,
                                    model.GroupName,
                                    model.IsAttFlag,
                                    model.AfterEvlutionConfigTime
                                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="model">实体</param>
        public void UpdateCourseRoomRule(New_CourseRoomRule model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"Update New_CourseRoomRule set CourseId=@CourseId,RoomId=@RoomId,ClassIDs=@ClassIDs,TeacherId=@TeacherId,PingId=@PingId,Rules=@Rules,SeatDetail=@SeatDetail,Type=@Type,StartTime=@StartTime,EndTime=@EndTime,PersonCount=@PersonCount,GroupName=@GroupName,IsAttFlag=@IsAttFlag,AfterEvlutionConfigTime=@AfterEvlutionConfigTime  where Id=@Id";
                var param = new
                                {
                                    model.Id,
                                    model.CourseId,
                                    model.RoomId,
                                    model.ClassIDs,
                                    model.TeacherId,
                                    model.PingId,
                                    model.Rules,
                                    model.SeatDetail,
                                    model.Type,
                                    model.StartTime,
                                    model.EndTime,
                                    model.PersonCount,
                                    model.GroupName,
                                    model.IsAttFlag,
                                    model.AfterEvlutionConfigTime
                                };
                conn.Execute(sqlwhere, param);
            }
        }

        /// <summary>
        /// 根据课程ID找出数据
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="sqlwhere">查询条件</param>
        /// <returns></returns>
        public List<New_CourseRoomRule> GetNew_CourseRoomRuleListByCourseId(int courseId, string sqlwhere = "")
        {
            using (var connection = OpenConnection())
            {
                var selectSql =
                    "select dbo.f_GetRoomNameByNew_CourseRoomRuleId(New_CourseRoomRule.Id) AS RoomName,dbo.f_GetTeacherName(New_CourseRoomRule.TeacherId) as teachername,* from New_CourseRoomRule where  courseId=" +
                    courseId + " and " + (string.IsNullOrEmpty(sqlwhere) ? "1=1" : sqlwhere);
                return connection.Query<New_CourseRoomRule>(selectSql).ToList();
            }
        }

        /// <summary>
        /// 查找分组教室（为查找我的座位）
        /// </summary>
        /// <param name="sqlwhere">查询条件</param>
        /// <returns></returns>
        public List<New_CourseRoomRule> GetRoomRuleForSeat(string sqlwhere = "")
        {
            using (var connection = OpenConnection())
            {
                var selectSql =
                    "select *,b.PublicFlag as PublicFlag,c.RoomName from New_CourseRoomRule a, New_Course b,New_ClassRoom c where b.IsDelete=0 and a.CourseId=b.Id and a.RoomId=c.Id and " + (string.IsNullOrEmpty(sqlwhere) ? "1=1" : sqlwhere);
                return connection.Query<New_CourseRoomRule>(selectSql).ToList();
            }
        }

        /// <summary>
        /// 讲师端：找出一门课程下所教的教室
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="sqlwhere"></param>
        /// <returns></returns>
        public List<New_CourseRoomRule> GetNew_CourseRoomRuleListByCourseIdForStudent(int courseId, string sqlwhere = "")
        {
            using (var connection = OpenConnection())
            {
                var selectSql = "select * from New_CourseRoomRule where  courseId=" + courseId + " and " +
                                (string.IsNullOrEmpty(sqlwhere) ? "1=1" : sqlwhere);
                return connection.Query<New_CourseRoomRule>(selectSql).ToList();
            }
        }





        /// <summary>
        /// 根据条件获取学员对讲师的评价信息详情页面（暂时废弃的代码）
        /// </summary>
        /// <param name="where">条件语句格式" and ..."</param>
        /// <param name="startIndex">起始页索引</param>
        /// <param name="startLength">每页记录数</param>
        /// <param name="orderBy">排序规则</param>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetEvaluateUserToTeacherById(string where = "", int startIndex = 1,
                                                                     int startLength = int.MaxValue,
                                                                     string orderBy = " ORDER BY u.UserId ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                if (string.IsNullOrWhiteSpace(orderBy))
                {
                    orderBy = " ORDER BY u.UserId ";
                }
                string sql =
                    string.Format(
                        @" 
SELECT * FROM 
(
    SELECT count(*) OVER(PARTITION BY null) AS totalCount
    , row_number() OVER({1}) AS rowNum
    , DISTINCT u.UserId,u.Realname,ra.CourseRoomRuleId,ra.ExampaperID
FROM Survey_ReplyAnswer ra 
JOIN New_CourseRoomRule crr ON crr.Id=ra.CourseRoomRuleId 
LEFT JOIN Sys_User u ON u.UserId=ra.UserID 
WHERE ra.SourceFrom=0 AND ra.Status=1 {0}
) AS result 
WHERE result.rowNum BETWEEN @startLength*(@startIndex-1)+1 AND @startLength*@startIndex ",
                        where, orderBy);
                var param = new
                                {
                                    startIndex = startIndex,
                                    startLength = startLength
                                };
                return conn.Query<Survey_ReplyAnswer>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 获取单个课程信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public New_CourseRoomRule GetNew_CourseRoomRule(int Id, string wherestr = "1=1")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from New_CourseRoomRule where Id=@Id and " + wherestr);
                var param = new {Id = Id};
                return connection.Query<New_CourseRoomRule>(query, param).FirstOrDefault();
            }
        }


        /// <summary>
        /// 获取单个课程信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public New_CourseRoomRule Get(string wherestr = "1=1")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from New_CourseRoomRule where " + wherestr);
                //var param = new { Id = Id };
                return connection.Query<New_CourseRoomRule>(query).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取单个课程信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public New_CourseRoomRule GetNewCourseRoomRule(int id)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select a.*,c.CourseName,r.RoomName,u.Realname as teachername from  
New_CourseRoomRule a
left join New_Course c on a.CourseId=c.Id
left join New_ClassRoom r on a.RoomId=r.Id
left join Sys_User u on a.TeacherId=u.UserId
where a.id=@id");
                var param = new {id };
                return connection.Query<New_CourseRoomRule>(query, param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据课程ID删除相关的集中授课和分组带教
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <param name="type">类型：-1:都删；0：集中；1：分组带教</param>
        /// <returns></returns>
        public bool DeleteCourseRoomRule(int courseID, int type = -1)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"delete from New_CourseRoomRule where CourseId=@courseID {0}",
                                          type == -1 ? "" : (" and Type=" + type));
                var param = new {courseID};
                connection.Execute(query, param);
                return true;
            }
        }


        /// <summary>
        /// 根据ID获取学员对我的评价详情信息
        /// </summary>
        /// <param name="courseRoomRuleId">新员工课程教室分配规则ID</param>
        /// <returns></returns>
        public New_CourseRoomRule GetCourseRoomRuleEvaluate(int courseRoomRuleId)
        {
            using (var connection = OpenConnection())
            {
                var query =
                    string.Format(
                        @" SELECT crr.Id,crr.CourseId,c.CourseName,crr.Type,crr.StartTime,crr.EndTime ,cr.RoomName,u.Realname as teachername 
       ,crr.PingId
       ,isnull((select  round(AVG(CONVERT(decimal,sra.SubjectiveAnswer)),2)  
        from Survey_ReplyAnswer sra
        JOIN Survey_Question sq ON sq.ObjectType=1 and sq.QuestionID=sra.QuestionID AND sq.QuestionType=3 --星评题
        where  sra.SourceFrom=0 and sra.ObjectID=crr.CourseId and sra.ExampaperID=crr.PingId 
               AND sra.QuestionID IN (SELECT q.QuestionID FROM Survey_Question q WHERE q.ExampaperID=crr.PingId AND q.Status=0) 
               AND isnumeric(sra.SubjectiveAnswer)=1 --判断是否为数字 
               and sra.Status=1 AND sra.CourseRoomRuleId=crr.Id),0 ) AS CourseTeacherAvg --讲师平均分
       ,isnull((select  round(AVG(CONVERT(decimal,sra.SubjectiveAnswer)),2)  
        from Survey_ReplyAnswer sra
        JOIN Survey_Question sq ON sq.ObjectType=0 and sq.QuestionID=sra.QuestionID AND sq.QuestionType=3 --星评题
        where  sra.SourceFrom=0 and sra.ObjectID=crr.CourseId and sra.ExampaperID=crr.PingId 
               AND sra.QuestionID IN (SELECT q.QuestionID FROM Survey_Question q WHERE q.ExampaperID=crr.PingId AND q.Status=0) 
               AND isnumeric(sra.SubjectiveAnswer)=1 --判断是否为数字 
               and sra.Status=1 AND sra.CourseRoomRuleId=crr.Id),0 ) AS CourseAvg --课程平均分
FROM New_CourseRoomRule crr
LEFT JOIN New_Course c ON c.Id=crr.CourseId 
LEFT JOIN New_ClassRoom cr ON cr.Id=crr.RoomId 
left join Sys_User u on crr.TeacherId=u.UserId 
WHERE crr.Id={0} ",
                        courseRoomRuleId);
                return connection.Query<New_CourseRoomRule>(query).FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据ID获取学员对我的评价详情信息
        /// </summary>
        /// <param name="userIds">用户ID集合</param>
        /// <returns></returns>
        public List<UserSeatModel> GetUserSeatDetail(string userIds)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select u.UserId,u.RealName as UserName,u.DeptName,u.NumberID,u.Sex,c.ClassNo,g.GroupNo
from Sys_User u
left join New_GroupUser gu on gu.UserId=u.UserId
left join New_Class c on c.Id=gu.ClassId
left join New_Group g on g.Id=gu.GroupId
where u.UserId in ({0})",userIds);
                return connection.Query<UserSeatModel>(query).ToList();
            }
        }
    }
}
