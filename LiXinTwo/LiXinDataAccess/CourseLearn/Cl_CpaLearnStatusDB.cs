using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using System.Data;
using LiXinModels.CourseManage;
using LiXinModels.CourseLearn;
namespace LiXinDataAccess.CourseLearn
{
    public class Cl_CpaLearnStatusDB : BaseRepository
    {
        public void SubscribeCPALearnStatus(Cl_CpaLearnStatus model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Cl_CpaLearnStatus( CourseId,UserId,IsAttFlag,IsPass,Progress,LearnTimes,GetLength,LastUpdateTime,CpaFlag,GradeStatus) 
                      values (@CourseId,@UserId,@IsAttFlag,@IsPass,@Progress,@LearnTimes,@GetLength,getdate(),@CpaFlag,@GradeStatus);SELECT @@IDENTITY AS ID";
                var param = new
                    {
                        model.CourseID,
                        model.UserID,
                        model.IsAttFlag,
                        model.IsPass,
                        model.Progress,
                        model.LearnTimes,
                        model.GetLength,
                        model.CpaFlag,
                        model.GradeStatus
                    };
                
                dynamic list = conn.Query<dynamic>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(list.ID);
                
                 
               
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateCPALearnStatus(Cl_CpaLearnStatus model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update Cl_CpaLearnStatus SET IsAttFlag=@IsAttFlag,IsPass = @IsPass,Progress = @Progress,LearnTimes = @LearnTimes,GetLength = @GetLength,LastUpdateTime = @LastUpdateTime,CpaFlag = @CpaFlag,GradeStatus = @GradeStatus WHERE Id=@Id";
                var param = new
                {
                    model.IsAttFlag,
                    model.IsPass,
                    model.Progress,
                    model.LearnTimes,
                    model.GetLength,
                    LastUpdateTime=DateTime.Now,
                    model.CpaFlag,
                    model.GradeStatus,
                    model.Id
                };
                int result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sql">实体</param>
        /// <returns>执行结果</returns>
        public bool UpdateCPALearnStatus(string sql)
        {
            using (var conn = OpenConnection())
            {
                var result = conn.Execute(sql);
                return result > 0;
            }
        }

        /// <summary>
        /// 根据课程和人员来查找数据
        /// </summary>
        /// <param name="CourseId">课程ID</param>
        /// <param name="UserId">用户ID</param>  
        /// <param name="CpaFlag">0：视频课程；1：CPA课程；2：转换的CPA课程</param>  
        /// <returns></returns>
        public Cl_CpaLearnStatus GetCl_CpaLearnStatusByCourseId(int CourseId, int UserId, string whereStr = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from Cl_CpaLearnStatus where CourseID=" + CourseId + " and UserID=" + UserId + " and " + whereStr;
                return conn.Query<Cl_CpaLearnStatus>(sqlstr).FirstOrDefault();
            }
        }


        public Cl_CpaLearnStatus GetCl_CpaLearnStatusByLearnId(int LearnId, int UserId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from Cl_CpaLearnStatus where Id=" + LearnId + " and UserID=" + UserId;
                return conn.Query<Cl_CpaLearnStatus>(sqlstr).FirstOrDefault();
            }
        }


        /// <summary>
        /// 根据课程和人员来查找数据
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool UpdateGetCl_CpaLearnStatusByCourseId(int CourseId, int UserId, decimal GetLength, int IsPass)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "update  Cl_CpaLearnStatus set GetLength=" + GetLength + ",IsPass=" + IsPass + " ,Progress=1 where CourseID=" + CourseId + " and UserID=" + UserId;
                return conn.Execute(sqlstr) > 0;
            }
        }


        public void SubscribeCPA(Cl_CpaLearnStatus model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"insert into Cl_CpaLearnStatus( CourseId,UserId,CpaFlag,LastUpdateTime) values (@CourseId,@UserId,1,getdate())";
                var param = new
                {
                    model.CourseID,
                    model.UserID,
                };
                decimal id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }

        /// <summary>
        /// 获取查询集合
        /// </summary>
        /// <param name="wherestr">查询条件</param>
        public List<Cl_CpaLearnStatus> GetCpaCourse(string wherestr="1=1")
        {
            using (var conn = OpenConnection())
            {
                var sqlwhere = string.Format("select * from Cl_CpaLearnStatus where {0}",wherestr);
                return conn.Query<Cl_CpaLearnStatus>(sqlwhere).ToList();
            }
        }

        /// <summary>
        /// 根据CPA课程ID 找出所预定的人员
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public List<Cl_CpaLearnStatus> GetCpaCourseByCourseId(int courseid)
        {
            using (var conn = OpenConnection())
            {
                var sqlwhere = string.Format(@"  
  select  Sys_User.Realname,Sys_User.MobileNum from Cl_CpaLearnStatus left join Sys_User on
      Cl_CpaLearnStatus.UserID=Sys_User.UserID where Cl_CpaLearnStatus.CourseID={0} and Cl_CpaLearnStatus.CpaFlag=1", courseid);
                return conn.Query<Cl_CpaLearnStatus>(sqlwhere).ToList();
            }
        }

        /// <summary>
        /// 根据课程ID和学员ID 修改是否在记录学时 0：可以改   1不能改
        /// </summary>
        /// <param name="num"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool UpdateProgress(int num,int userid)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "update  Cl_CpaLearnStatus set Progress=1  where CourseID=" + num + " and UserID=" + userid;
                return conn.Execute(sqlstr) > 0;
            }
        }

        /// <summary>
        /// 记录观看视频总时长
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="VedioTime"></param>
        /// <returns></returns>
        public bool UpdateVedioTime(int learnid,decimal VedioTime)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "update  Cl_CpaLearnStatus set VedioTime=" + VedioTime + "  where Id=" + learnid ;
                return conn.Execute(sqlstr) > 0;
            }
        }

        /// <summary>
        /// 记录观看视频总时长累加
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="userid"></param>
        /// <param name="VedioTime"></param>
        /// <returns></returns>
        public bool UpdateVedioScormTime(int learnid, decimal VedioTime)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "update  Cl_CpaLearnStatus set VedioTime=isnull(VedioTime,0)+" + VedioTime + "  where Id=" + learnid;
                return conn.Execute(sqlstr) > 0;
            }
        }
        #region CPA 成绩导入方法

        /// <summary>
        /// 某门课程某个学员的CPA 课程成绩是否已经导入  
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="getLength"></param>
        /// <returns></returns>
        public bool IsImport(int courseId, int userId, int getLength)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere =
                    "select count(*) from Cl_CpaLearnStatus WHERE GradeStatus=1  AND userId=@userId and courseId=@courseId ";

                var param = new
                {
                    userId,
                    courseId
                };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }

        /// <summary>
        ///  某门课程的CPA 课程成绩是否已经导入  完毕
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="getLength"></param>
        /// <returns></returns>
        public bool IsImportOver(int courseId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlwhere =
                    "select count(*) from Cl_CpaLearnStatus WHERE GradeStatus=0 AND  courseId=@courseId ";

                var param = new
                {
                    courseId
                };
                int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count > 0;
            }
        }

        /// <summary>
        /// 更新 学员获得的CPA学时
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <param name="getLength"></param>
        /// <returns></returns>
        public bool UpdateCpaLearnStatus(int courseId, int userId, int getLength)
        {
            using (IDbConnection connection = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update [Cl_CpaLearnStatus] set ");
                strSql.Append("GetLength=@GetLength, ");
                strSql.Append("LastUpdateTime=@LastUpdateTime,");
                strSql.Append("GradeStatus=1 ");
                strSql.Append(" where courseId=@courseId AND UserID=@UserID  ");

                var param = new
                {
                    courseId,
                    userId,
                    GetLength=getLength,
                    LastUpdateTime=DateTime.Now
                };
                return connection.Execute(strSql.ToString(), param) > 0;
            }

        }

        /// <summary>
        /// 根据课程编号 获取 该课程下学员所获得的成绩详情
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<Cl_CpaLearnStatus> GetCPACourseScoreList(int courseId, string orderStr = "GetLength desc")
        { 
               
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select  Cl_CpaLearnStatus.Id,Cl_CpaLearnStatus.GetLength,Sys_User.Realname,Sys_User.CPANo,
                                Sys_Department.DeptName
                                from Cl_CpaLearnStatus LEFT JOIN Sys_User 
                                ON Cl_CpaLearnStatus.UserID=Sys_User.UserId
                                ,Co_Course,Sys_Department
                                WHERE CourseID=Co_Course.Id  AND Sys_User.DeptId=Sys_Department.DepartmentId AND   CourseId={0} ORDER BY " + orderStr, courseId);
                return connection.Query<Cl_CpaLearnStatus>(query).ToList();
            }

        }

        /// <summary>
        /// 视频考试不过清楚学习记录
        /// </summary>
        /// <param name="learnId"></param>
        /// <returns></returns>
        public bool DeleteLearn(int learnId)
        {
            
                using (IDbConnection conn = OpenConnection())
                {
                    string sqlwhere = string.Format(@"delete Cl_CpaLearnStatus where Id = {0}", learnId);
                    int result = conn.Execute(sqlwhere);
                    string sql = string.Format(@"delete Cl_LearnVideoInfor where LearnId = {0}", learnId);
                    int r = conn.Execute(sql);
                    return result > 0 && r>0;
                }
            
        }

        /// <summary>
        /// 根据课程ID和课程类型 删除学习记录
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="cpaflag"></param>
        /// <returns></returns>
        public bool DeleteByCourseIdAndCpaFlag(int courseid, int cpaflag)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = " delete Cl_CpaLearnStatus where CourseID=" + courseid + " and CpaFlag="+cpaflag;
                int result = conn.Execute(sql);
                return result > 0 ; 
            }
        }


        #endregion


        #region 修复视频课程转入CPA课程方法

        public List<Cl_CpaLearnStatus> NewGetList(string sql)
        {          
             using (IDbConnection conn = OpenConnection())
             {
                 return conn.Query<Cl_CpaLearnStatus>(sql).ToList();
             }
              
        }

        #endregion
    }
}
