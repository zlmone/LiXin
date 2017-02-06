using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LiXinModels;
using Retech.Core;
using Retech.Data;
using LiXinModels.DeptCourseManage;

namespace LiXinDataAccess.DeptCourseManage
{
    public class DeptCourseDB : BaseRepository
    {
        /// <summary>
        /// 获取课程相关信息和用户所在分组的部门编号
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="Id">课程ID</param>
        /// <returns></returns>
        public Dept_Course GetCo_Course(int Id, int userId, int deptId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = string.Format(@"select DepartSetId,Co_Course.* ,Sys_User.Realname AS TeacherName,Sys_User.IsDelete AS TeacherIsDelete,Sys_User.IsTeacher AS TeacherIsTeacher,Sys_ClassRoom.RoomName,New_ClassRoom.RoomName as ClassroomAddress,isnull(dtco.OpenFlag ,0) as IsOpenOrder,dtco.LimitNumber as DeptNumberLimited
 from Co_Course  left join dbo.DepTran_CourseOpen dtco on  dtco.CourseId=Co_Course.Id and  {0} 
left join (select dtdu.DepartSetId from DepTran_DepartUser as dtdu 
left join Sys_User as su on dtdu.UserId=su.UserId where dtdu.UserId=@userId) ss on 1=1
left join New_ClassRoom on New_ClassRoom.Id=Co_Course.RoomId LEFT JOIN Sys_User ON 
Co_Course.Teacher=Sys_User.UserId LEFT Join Sys_ClassRoom ON Sys_ClassRoom.Id=Co_Course.RoomId  where Co_Course.Id=@Id", (deptId == 0 ? " 1=1" : "dtco.DepartId=" + deptId));
                var param = new
                {
                    Id = Id,
                    userId = userId
                };
                return connection.Query<Dept_Course>(sqlwhere, param).FirstOrDefault();

            }
        }

        /// <summary>
        /// 获取课程相关信息及分页
        /// </summary>
        /// <param name="currentDeptId">用户部门ID</param>
        /// <returns></returns>
        public List<Dept_Course> GetCourseTogetherList(out int totalCount, int currentDeptId, string where = " 1 = 1 ", int startIndex = 0,
                                       int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Code,Co_Course.Id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>(string.Format(@"select count(1) from Co_Course 
 left join dbo.DepTran_CourseOpen dtco on  dtco.CourseId=Co_Course.Id and  dtco.DepartId={0}
    where " + where + @" AND Co_Course.IsDelete=0 AND Co_Course.IsRT=1 AND Co_Course.Publishflag=1 ", currentDeptId))
                              .First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,Co_Course.*,Sys_User.Realname AS TeacherName,Sys_User.IsDelete AS TeacherIsDelete,Sys_User.IsTeacher AS TeacherIsTeacher, Sys_ClassRoom.RoomName,CourseTimesOrder,TotalCourseTimes,Sys_ClassRoom.RoomName as ClassroomAddress
--,
--row_number() OVER(PARTITION BY Co_Course.Code ORDER BY StartTime,Co_Course.Id asc) AS CourseTimesOrder,
--count(Code)over( PARTITION BY Co_Course.Code) AS TotalCourseTimes
,isnull(dtco.OpenFlag ,0) as IsOpenOrder,dtco.LimitNumber
 from Co_Course left join dbo.DepTran_CourseOpen dtco on  dtco.CourseId=Co_Course.Id and  dtco.DepartId={2}
left join New_ClassRoom on New_ClassRoom.Id=Co_Course.RoomId
LEFT JOIN Sys_User ON Co_Course.Teacher=Sys_User.UserId LEFT Join Sys_ClassRoom ON Sys_ClassRoom.Id=Co_Course.RoomId 
left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime,cc.Id asc) as CourseTimesOrder,
			count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
    from Co_Course cc where cc.isdelete = 0 and cc.coursefrom = 2  AND way=1 ) tt 
    on tt.id = Co_Course.id and tt.code = Co_Course.code 
    where " + where + @" AND Co_Course.IsDelete=0 AND Co_Course.IsRT=1 AND Co_Course.Publishflag=1 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy, currentDeptId);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Dept_Course>(query, parameters).ToList();
            }
        }

        /// <summary>
        /// 向表格DepTran_CourseOpen中插入开放的信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="deptId">用户部门ID</param>
        /// <param name="num">允许人数 </param>
        /// <returns></returns>
        public bool IsOrderInsert(string courseId, int deptId,int num)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into DepTran_CourseOpen (CourseId,DepartId,AttFlag,OpenFlag,ApprovalFlag,LimitNumber) values(@courseId,@deptId,0,1,0,@Num)");
                var param = new
                {
                    courseId = courseId,
                    deptId = deptId,
                    Num=num
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 判断表格DepTran_CourseOpen中是否有该课程的开放信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="deptId">用户部门ID</param>
        /// <returns></returns>
        public bool IsOrderExist(string courseId, int deptId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere =
                    "select count(1) from dbo.DepTran_CourseOpen where CourseId=@courseId and DepartId=@deptId";
                var param = new
                    {
                        courseId = courseId,
                        deptId = deptId
                    };
                int count = connection.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }

        /// <summary>
        /// 向表格DepTran_CourseOpen中更新不开放的信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="deptId">用户部门ID</param>
        /// <returns></returns>
        public bool DeleteOrder(string courseId, int deptId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" update DepTran_CourseOpen set OpenFlag=0 where CourseId=@courseId and DepartId=@deptId");
                var param = new
                {
                    courseId = courseId,
                    deptId = deptId

                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 向表格DepTran_CourseOpen中更新开放的信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="deptId">用户部门ID</param>
        /// <param name="num">报名人数</param>
        /// <returns></returns>
        public bool UpdataOrder(string courseId, int deptId, int num)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var strSql = new StringBuilder();
                strSql.Append("update DepTran_CourseOpen set LimitNumber=@Num where CourseId=@courseId and DepartId=@deptId");
                var param = new
                    {
                        courseId, deptId,
                        Num = num
                    };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 向表格DepTran_CourseOpen中更新开放的信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="deptId">用户部门ID</param>
        /// <param name="num">报名人数</param>
        /// <returns></returns>
        public bool UpdataOrder(string courseId, int deptId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update DepTran_CourseOpen set OpenFlag=1 where CourseId=@courseId and DepartId=@deptId");
                var param = new
                {
                    courseId, deptId
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 向表格DepTran_CourseOpen中插入不开放的信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="deptId">用户部门ID</param>
        /// <param name="num">允许人数</param>
        /// <returns></returns>
        public bool IsDeleteOrder(string courseId, int deptId,int num)
        {
            using (var connection = OpenConnection())
            {
                var strSql = new StringBuilder();
                strSql.Append("insert into DepTran_CourseOpen (CourseId,DepartId,AttFlag,OpenFlag,LimitNumber,ApprovalFlag) values(@courseId,@deptId,0,0,@Num,0)");
                var param = new
                    {
                        courseId, deptId,Num=num
                    };
                return connection.Execute(strSql.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 获得DepTran_DepartUser表格中用户的部门信息
        /// </summary>
        /// <param name="userId">用户的ID</param>
        /// <returns></returns>
        public Dept_Course GetDeptId(int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere =
                    "select dtdu.DepartSetId from DepTran_DepartUser as dtdu left join Sys_User as su on dtdu.UserId=su.UserId where dtdu.UserId=@userId";
                var param = new
                    {
                        userId = userId
                    };
                return connection.Query<Dept_Course>(sqlwhere, param).FirstOrDefault();
            }
        }


        #region 转播课程管理（最高权限的人）
        /// <summary>
        /// 转播课程管理（最高权限的人）
        /// </summary>
        /// <returns></returns>
        public List<Dept_Course> GetDeptAllCourseManage(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  StartTime desc")
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select * from(
select  row_number() over(ORDER BY {0}) as num,count(*)OVER(PARTITION BY null) totalcount,
Co_Course.*,Sys_User.Realname AS TeacherName,Sys_User.IsDelete AS TeacherIsDelete,
Sys_User.IsTeacher AS TeacherIsTeacher, Sys_ClassRoom.RoomName,CourseTimesOrder,
TotalCourseTimes,Sys_ClassRoom.RoomName as ClassroomAddress
from Co_Course
left join New_ClassRoom on New_ClassRoom.Id=Co_Course.RoomId
LEFT JOIN Sys_User ON Co_Course.Teacher=Sys_User.UserId LEFT Join Sys_ClassRoom ON Sys_ClassRoom.Id=Co_Course.RoomId 
left join (select id,code, row_number() OVER(PARTITION BY cc.Code ORDER BY cc.StartTime,cc.Id asc) as CourseTimesOrder,
			count(cc.Code)over( PARTITION BY cc.Code) as TotalCourseTimes
    from Co_Course cc where cc.isdelete = 0 and cc.coursefrom = 2  AND way=1 ) tt 
    on tt.id = Co_Course.id and tt.code = Co_Course.code 
    where Co_Course.IsDelete=0 AND Co_Course.IsRT=1 AND Co_Course.Publishflag=1  AND {1}
)result 
WHERE num BETWEEN @startLength*(@startIndex-1)+1  AND @startLength*@startIndex", jsRenderSortField, where);
                var param = new
                {
                    startIndex = startIndex,
                    startLength = startLength
                };
                return conn.Query<Dept_Course>(sql, param).ToList();
            }
        }
        #endregion
    }
}
