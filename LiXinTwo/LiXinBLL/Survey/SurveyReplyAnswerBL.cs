using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.Survey;
using LiXinInterface.Survey;
using LiXinModels.Survey;

namespace LiXinBLL.Survey
{
    public class SurveyReplyAnswerBL : ISurveyReplyAnswer
    {
        protected SurveyReplyAnswerDB Sdb;
        public SurveyReplyAnswerBL()
        {
            Sdb = new SurveyReplyAnswerDB();
        }



        public Survey_ReplyAnswer GetSurvey_QuestionByQuestionID(int AnswerId)
        {
            return Sdb.GetSurvey_QuestionByQuestionID(AnswerId);
        }

        public bool InsertSurvey_ReplyAnswer(Survey_ReplyAnswer model)
        {
            return Sdb.InsertSurvey_ReplyAnswer(model);
        }


        public List<Survey_ReplyAnswer> GetList(string where = " 1=1")
        {
            return Sdb.GetList(where);
        }

        public bool GetStatus(int CourseId, int UserId, int status, int ExampaperID)
        {
            return Sdb.GetStatus(CourseId, UserId, status, ExampaperID);
        }

        public bool UpdateStatus(int CourseId, int UserId, int status,int ExampaperID)
        {
            return Sdb.UpdateStatus(CourseId, UserId, status, ExampaperID);
        }

        public List<Survey_ReplyAnswer> GetSurvey_ReplyAnswerListAndUser(string where = " 1=1")
        {
            return Sdb.GetSurvey_ReplyAnswerListAndUser(where);
        }




        public int GetSurvey_ReplyAnswerCountBYbjectID(int courseid, int ExampaperID, int QuestionId)
        {
            return Sdb.GetSurvey_ReplyAnswerCountBYbjectID(courseid, ExampaperID, QuestionId);
        }

        public int GetSingleSurvey_ReplyAnswerCount(int courseid, int ExampaperID, int QuestionId, int AnswerId, int UserId)
        {
            return Sdb.GetSingleSurvey_ReplyAnswerCount(courseid, ExampaperID, QuestionId, AnswerId, UserId);
        }


        public int GetCount(int CourseId, int UserId)
        {
            return Sdb.GetCount(CourseId, UserId);
        }


        public bool DeleteByCourseIDAndUserId(int CourseID, int UserId, int ExampaperID)
        {
            return Sdb.DeleteByCourseIDAndUserId(CourseID, UserId, ExampaperID);
        }

        /// <summary>
        /// 更新回答，调查专用
        /// </summary>
        public void UpdateReaplyAnswer(Survey_ReplyAnswer model)
        {
            Sdb.UpdateReaplyAnswer(model);
        }

        /// <summary>
        /// 查询调查中此人的所以答案
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetAnswerBySurvey(int surveyID, int paperID, int userID)
        {
            string where = string.Format("  ObjectID={0} AND ExampaperID={1} AND UserID={2} AND SourceFrom=1", surveyID, paperID, userID);
            return Sdb.GetList(where);
        }
    }
}
