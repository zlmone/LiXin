using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseManage;
using LiXinModels.Survey;
using Retech.Core;
using Retech.Data;
using System.Data;

namespace LiXinDataAccess.DeptSurvey
{
    public partial class SurveyQuestionDB : BaseRepository
    {
        public List<Survey_Question> GetSurvey_QuestionByExampaperID(int ExampaperID)
        {
            using (var connection = OpenConnection())
            {
                string sqlstr = "select * from Dep_Survey_Question where ExampaperID=" + ExampaperID + " and Status=0";
                return connection.Query<Survey_Question>(sqlstr).ToList();
            }
        }

        public List<Survey_Question> GetSurvey_QuestionByUserID(int courseID, int paperID, int userID)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT * FROM Dep_Survey_Question 
WHERE 	QuestionID IN(
SELECT distinct QuestionID FROM Dep_Survey_ReplyAnswer
WHERE ObjectID={0} AND ExampaperID={1}
AND SourceFrom=0
)", courseID, paperID);
                return conn.Query<Survey_Question>(sql).ToList();
            }
        }

        public Survey_Question GetSurvey_QuestionByQuestionID(int QuestionID)
        {
            using (var connection = OpenConnection())
            {
                const string sqlstr = "select * from Dep_Survey_Question where QuestionID=@QuestionID";
                return connection.Query<Survey_Question>(sqlstr, new
                {
                    QuestionID
                }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 查看平均分
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="questionID"></param>
        /// <param name="exampaperID"></param>
        /// <returns></returns>
        public decimal GetSurvey_QuestionAvg(int courseID, int questionID, int exampaperID, int questionType)
        {
            using (var connection = OpenConnection())
            {
                if (questionType == 3)
                {
                    string sql = "select  round(AVG(CONVERT(decimal,SubjectiveAnswer)),2) avg from Dep_Survey_ReplyAnswer where ObjectID=" + courseID + " and ExampaperID=" + exampaperID + " and QuestionID=" + questionID + " and SourceFrom=0 and Status=1";
                    var aa = connection.Query<dynamic>(sql).FirstOrDefault();
                    return Convert.ToDecimal(aa.avg);
                }
                else
                {
                    return 0;
                }

            }
        }

        /// <summary>
        /// 查看平均分(新进员工对讲师评价使用)
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <param name="questionId">问题ID</param>
        /// <param name="exampaperId">问卷ID</param>
        /// <param name="courseRoomRuleId">新员工课程教室分配规则ID</param>
        /// <param name="questionType">问题类型 0：单选题；1：多选题；2：主观题; 3:星评题</param>
        /// <returns></returns>
        public decimal GetNewSurveyQuestionAvg(int courseId, int questionId, int exampaperId, int courseRoomRuleId, int questionType)
        {
            using (var connection = OpenConnection())
            {
                if (questionType == 3)
                {
                    string sql = string.Format(@"select  round(AVG(CONVERT(decimal,SubjectiveAnswer)),2) avg 
from Dep_Survey_ReplyAnswer 
where ObjectID={0} and ExampaperID={1} and QuestionID={2} and CourseRoomRuleId={3} 
      and SourceFrom=0 and Status=1", courseId, exampaperId, questionId, courseRoomRuleId);
                    var aa = connection.Query<dynamic>(sql).FirstOrDefault();
                    return Convert.ToDecimal(aa.avg);
                }
                return 0;
            }
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(Survey_Question model)
        {
            using (var connection = OpenConnection())
            {
                const string sql = @"INSERT INTO dbo.Dep_Survey_Question
	                                (
	                                ExampaperID,
	                                QuestionType,
	                                QuestionContent,
	                                QuestionOrder,
	                                UpdateTime,
	                                userID,
	                                Status,
                                    LinkSortPayGrade
	                                )
                                VALUES 
	                                (
	                                @exampaperid,
	                                @questiontype,
	                                @questioncontent,
	                                @questionorder,
	                                getdate(),
	                                @userid,
	                                0,
                                    @linkSortPayGrade
	                                );SELECT @@IDENTITY ID";
                var param = new
                {
                    exampaperid = model.ExampaperID,
                    questiontype = model.QuestionType,
                    questioncontent = model.QuestionContent,
                    questionorder = model.QuestionOrder,
                    userid = model.UserID,
                    linkSortPayGrade = model.LinkSortPayGrade
                };
                dynamic item = connection.Query<dynamic>(sql, param).FirstOrDefault();
                model.QuestionID = decimal.ToInt32(item.ID);
            }
        }





    }
}
