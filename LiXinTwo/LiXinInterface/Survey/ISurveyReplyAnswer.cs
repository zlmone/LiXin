using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Survey;

namespace LiXinInterface.Survey
{
    public interface ISurveyReplyAnswer
    {
        bool InsertSurvey_ReplyAnswer(Survey_ReplyAnswer model);

        Survey_ReplyAnswer GetSurvey_QuestionByQuestionID(int AnswerId);

        List<Survey_ReplyAnswer> GetList(string where = " 1=1");


        bool GetStatus(int CourseId, int UserId, int status, int ExampaperID);



        bool UpdateStatus(int CourseId, int UserId, int status, int ExampaperID);

        /// <summary>
        /// 查处回答人数
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="ExampaperID"></param>
        /// <returns></returns>
        int GetSurvey_ReplyAnswerCountBYbjectID(int courseid, int ExampaperID, int QuestionId);


        int GetSingleSurvey_ReplyAnswerCount(int courseid, int ExampaperID, int QuestionId, int AnswerId, int UserId);

        /// <summary>
        /// 查询是否已回答 0:没回答  大于0:已回答
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        int GetCount(int CourseId, int UserId);

        /// <summary>
        /// 点击保存按钮课后评估后删除
        /// </summary>
        /// <param name="CourseID"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        bool DeleteByCourseIDAndUserId(int CourseID, int UserId, int ExampaperID);

        /// <summary>
        /// 更新回答，调查专用
        /// </summary>
        void UpdateReaplyAnswer(Survey_ReplyAnswer model);

        /// <summary>
        /// 查询调查中此人的所以答案
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<Survey_ReplyAnswer> GetAnswerBySurvey(int surveyID, int paperID, int userID);


        List<Survey_ReplyAnswer> GetSurvey_ReplyAnswerListAndUser(string where = " 1=1");
    }
}
