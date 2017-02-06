using LiXinModels.NewCourseManage;
using Retech.Core;
using System;
using System.Collections.Generic;
using System.Data;
using Retech.Data;
using System.Linq;
using System.Text;

namespace LiXinDataAccess.NewCourseManage
{
    public class New_CpaLearnStatusDB : BaseRepository
    {

        public void SubscribeCPALearnStatus(New_CpaLearnStatus model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into New_CpaLearnStatus( CourseId,UserId,IsAttFlag,IsPass,Progress,LearnTimes,GetLength,LastUpdateTime,CpaFlag,GradeStatus) 
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
        public bool UpdateCPALearnStatus(New_CpaLearnStatus model)
        {
            using (IDbConnection conn = OpenConnection())
            {
                const string sqlwhere = @"Update New_CpaLearnStatus SET IsAttFlag=@IsAttFlag,IsPass = @IsPass,Progress = @Progress,LearnTimes = @LearnTimes,GetLength = @GetLength,LastUpdateTime = @LastUpdateTime,CpaFlag = @CpaFlag,GradeStatus = @GradeStatus WHERE Id=@Id";
                var param = new
                {
                    model.IsAttFlag,
                    model.IsPass,
                    model.Progress,
                    model.LearnTimes,
                    model.GetLength,
                    LastUpdateTime = DateTime.Now,
                    model.CpaFlag,
                    model.GradeStatus,
                    model.Id
                };
                int result = conn.Execute(sqlwhere, param);
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
        public New_CpaLearnStatus GetCl_CpaLearnStatusByCourseId(int CourseId, int UserId, string whereStr = " 1 = 1 ")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from New_CpaLearnStatus where CourseID=" + CourseId + " and UserID=" + UserId + " and " + whereStr;
                return conn.Query<New_CpaLearnStatus>(sqlstr).FirstOrDefault();
            }
        }

        public New_CpaLearnStatus GetNew_CpaLearnStatusByLearnId(int LearnId, int UserId)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sqlstr = "select * from New_CpaLearnStatus where Id=" + LearnId + " and UserID=" + UserId;
                return conn.Query<New_CpaLearnStatus>(sqlstr).FirstOrDefault();
            }
        }


    }
}
