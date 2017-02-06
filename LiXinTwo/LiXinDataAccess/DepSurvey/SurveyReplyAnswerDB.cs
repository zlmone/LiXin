using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Survey;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.DeptSurvey
{
    public class SurveyReplyAnswerDB : BaseRepository
    {
        public Survey_ReplyAnswer GetSurvey_QuestionByQuestionID(int AnswerId)
        {
            using (var connection = OpenConnection())
            {
                const string sqlstr = "select * from Dep_Survey_ReplyAnswer where AnswerId=@AnswerId";
                return connection.Query<Survey_ReplyAnswer>(sqlstr, new
                {
                    AnswerId
                }).FirstOrDefault();
            }
        }

        public bool InsertSurvey_ReplyAnswer(Survey_ReplyAnswer model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"INSERT INTO dbo.Dep_Survey_ReplyAnswer
	                (
	                SourceFrom,
	                ObjectID,
	                ExampaperID,
	                QuestionID,
	                SubjectiveAnswer,
	                ObjectiveAnswer,
	                UserID,
	                Status,
	                QuestionReply,
	                DeptID,
	                IsDeptAccessed,
	                IsOfficeAccessed,
	                IsMaster
	                )
                VALUES 
	                (
	                @sourcefrom,
	                @objectid,
	                @exampaperid,
	                @questionid,
	                @subjectiveanswer,
	                @objectiveanswer,
	                @userid,
	                @status,
	                @questionreply,
	                @deptid,
	                @isdeptaccessed,
	                @isofficeaccessed,
	                @ismaster
	                ) SELECT @@Identity as Id";
                var param = new
                    {
                        model.SourceFrom,
                        model.ObjectID,
                        model.ExampaperID,
                        model.QuestionID,
                        model.SubjectiveAnswer,
                        model.ObjectiveAnswer,
                        model.Status,
                        model.UserID,
                        model.QuestionReply,
                        model.DeptID,
                        model.IsDeptAccessed,
                        model.IsOfficeAccessed,
                        model.IsMaster
                    };
                var id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.AnswerId = decimal.ToInt32(id);
                return model.AnswerId > 0;
            }
        }

        public List<Survey_ReplyAnswer> GetList(string where = " 1=1")
        {
            using (var connection = OpenConnection())
            {
                string sqlstr = "select * from Dep_Survey_ReplyAnswer where " + where;
                return connection.Query<Survey_ReplyAnswer>(sqlstr).ToList();
            }
        }
        /// <summary>
        /// 查找带人员信息的
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetSurvey_ReplyAnswerListAndUser(string where = " 1=1")
        {
            using (var connection = OpenConnection())
            {
                string sqlstr = @"  select Sys_User.Realname,Sys_User.DeptName,* 
from Dep_Survey_ReplyAnswer 
left join Sys_user on Dep_Survey_ReplyAnswer.UserId=Sys_User.UserId 
where " + where;
                return connection.Query<Survey_ReplyAnswer>(sqlstr).ToList();
            }
        }

        public bool GetStatus(int CourseId, int UserId, int status, int ExampaperID)
        {
            using (var connection = OpenConnection())
            {
                string sql = "select count(1) from Dep_Survey_ReplyAnswer where UserID=" + UserId + " and ObjectID=" +
                             CourseId + " and [Status]=" + status + " and ExampaperID=" + ExampaperID;
                int result = connection.Query<int>(sql).FirstOrDefault();
                return result > 0;

            }
        }

        public bool UpdateStatus(int CourseId, int UserId, int status, int ExampaperID)
        {
            using (var connection = OpenConnection())
            {
                string sql = " update Dep_Survey_ReplyAnswer set [Status]=" + status + " where  UserID=" + UserId + " and ObjectID=" + CourseId + " and ExampaperID=" + ExampaperID;
                int result = connection.Query<int>(sql).FirstOrDefault();
                return result > 0;
            }
        }

        public int GetSurvey_ReplyAnswerCountBYbjectID(int courseid, int ExampaperID, int QuestionId)
        {
            using (var connection = OpenConnection())
            {
                string sql = "select count(1) from Dep_Survey_ReplyAnswer where  ObjectID=" + courseid + " and ExampaperID=" +
                             ExampaperID + " and QuestionID=" + QuestionId;
                int result = connection.Query<int>(sql).FirstOrDefault();
                return result;
            }
        }



        public int GetSingleSurvey_ReplyAnswerCount(int courseid, int ExampaperID, int QuestionId, int AnswerId, int UserId)
        {
            using (var connection = OpenConnection())
            {
                string sql = "";
                if (UserId == 0)
                {
                    sql = "  select count(1) from Dep_Survey_ReplyAnswer where  ObjectID=" + courseid + " and ExampaperID=" + ExampaperID + " and ObjectiveAnswer  like '%" + AnswerId + "%' and QuestionId=" + QuestionId;
                }
                else
                {
                    sql = "  select count(1) from Dep_Survey_ReplyAnswer where  ObjectID=" + courseid + " and ExampaperID=" + ExampaperID + " and ObjectiveAnswer like '%" + AnswerId + "%' and QuestionId=" + QuestionId + " and UserID=" + UserId;
                }
                int result = connection.Query<int>(sql).FirstOrDefault();
                return result;
            }
        }


        public int GetCount(int CourseId, int UserId)
        {
            using (var connection = OpenConnection())
            {
                string sql = "";
                sql += " select* from ";
                sql += " Co_Course left ";
                sql += " join Dep_Survey_ReplyAnswer ";
                sql += " on";
                sql += " Co_Course.Id = Dep_Survey_ReplyAnswer.ObjectID";
                sql += " where";
                sql += " Dep_Survey_ReplyAnswer.SourceFrom = 0";
                sql += " and";
                sql += " Dep_Survey_ReplyAnswer.UserID = " + UserId + "";
                sql += " and";
                sql += " Dep_Survey_ReplyAnswer.ObjectID = " + CourseId + "";

                return connection.Query<int>(sql).FirstOrDefault();

            }
        }

        public bool DeleteByCourseIDAndUserId(int CourseID, int UserId, int ExampaperID)
        {
            using (var connection = OpenConnection())
            {
                string sql = " delete Dep_Survey_ReplyAnswer where ObjectID=" + CourseID + " and UserID=" + UserId + " and ExampaperID=" + ExampaperID + " and SourceFrom=0 and [Status]=0";

                var result = connection.Execute(sql);
                return result > 0;

            }
        }

        /// <summary>
        /// 更新回答，调查专用
        /// </summary>
        public void UpdateReaplyAnswer(Survey_ReplyAnswer model)
        {
            using (var conn = OpenConnection())
            {
                const string sql = @"UPDATE dbo.Dep_Survey_ReplyAnswer
                SET 
	                SubjectiveAnswer = @subjectiveanswer,
	                ObjectiveAnswer = @objectiveanswer,
 	                QuestionReply = @questionreply
                WHERE 	UserID = @userid
                AND    ExampaperID = @exampaperid
                AND    ObjectID = @objectid
                AND   QuestionID=@questionid
                and IsMaster=@IsMaster
                and DeptID=@deptID";
                var param = new
                {

                    subjectiveanswer = model.SubjectiveAnswer,
                    objectiveanswer = model.ObjectiveAnswer,
                    questionreply = model.QuestionReply,
                    userid = model.UserID,
                    exampaperid = model.ExampaperID,
                    objectid = model.ObjectID,
                    questionid = model.QuestionID,
                    IsMaster = model.IsMaster,
                    deptID =model.DeptID
                };
                conn.Execute(sql, param);

            }
        }


    }
}
