using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using System.Data;
using LiXinModels.DepCourseManage;

namespace LiXinDataAccess.DepCourseManage
{
    public class Dep_CourseDeptDB : BaseRepository
    {
        #region 增改(基本信息)
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertDep_CourseDept(Dep_CourseDept model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Dep_CourseDept(CourseId,DepartId,AttFlag,SubmitTime,OpenFlag,ApprovalFlag,UserId) VALUES (@CourseId,@DepartId,@AttFlag,@SubmitTime,@OpenFlag,@ApprovalFlag,@UserId);SELECT @@IDENTITY as Id ";

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
        public bool UpdateDep_CourseDept(Dep_CourseDept model)
        {
            using (var conn = OpenConnection())
            {
                const string strSql = @"update Dep_CourseDept set CourseId = @CourseId,DepartId = @DepartId,AttFlag = @AttFlag,OpenFlag = @OpenFlag,SubmitTime = @SubmitTime,ApprovalFlag=@ApprovalFlag,UserId=@UserId where Id=@Id";

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
        /// <param name="courseID"></param>
        /// <returns></returns>
        public Dep_CourseDept GetDep_CourseDept(int courseID, int departId)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from Dep_CourseDept where CourseId=@CourseId and DepartId=@DepartId ");
                var param = new { CourseId = courseID, DepartId = departId };
                return connection.Query<Dep_CourseDept>(query, param).FirstOrDefault();
            }
        }

        #endregion

        #region 短信获取信息
        /// <summary>
        /// 获得邮件短信发送相关信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public Dep_CourseDept CourseName(int courseId, int departId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(
                    "select cc.CourseName from Dep_CourseDept dtc left join Dep_Course cc on cc.Id=dtc.CourseId where dtc.CourseId=@courseId and dtc.DepartId=@departId");
                var param = new
                {
                    courseId = courseId,
                    departId = departId
                };
                return connection.Query<Dep_CourseDept>(strSql.ToString(), param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获得邮件短信被发送人的信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public Dep_CourseDept UserNmae(int courseId, int departId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(
                    "select dtc.UserId ,su.* from Dep_CourseDept dtc left join Sys_User su on su.UserId=dtc.UserId where CourseId=@courseId and DepartId=@departId");
                var param = new
                {
                    courseId = courseId,
                    departId = departId
                };
                return connection.Query<Dep_CourseDept>(strSql.ToString(), param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        public Dep_CourseDept GetDepTran_CourseOpen(int courseID, int departId)
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select * from Dep_CourseDept where CourseId=@CourseId and DepartId=@DepartId ");
                var param = new { CourseId = courseID, DepartId = departId };
                return connection.Query<Dep_CourseDept>(query, param).FirstOrDefault();
            }
        }

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
                strSql.Append("update Dep_CourseDept set AttFlag=@attFlag, ApprovalFlag=@approvalFlag " +
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
        #endregion
    }
}
