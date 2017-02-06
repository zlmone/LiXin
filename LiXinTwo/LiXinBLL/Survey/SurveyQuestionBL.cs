using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.Survey;
using LiXinInterface.Survey;
using LiXinModels.CourseManage;
using LiXinModels.Survey;

namespace LiXinBLL.Survey
{
    public class SurveyQuestionBL : ISurveyQuestion
    {
        protected SurveyQuestionDB Sdb;
        protected SurveyQuestionAnswerDB sqadb;

        protected SurveyManageDB SurveyExampaperdb;

        public SurveyQuestionBL()
        {
            Sdb = new SurveyQuestionDB();
            sqadb = new SurveyQuestionAnswerDB();
            SurveyExampaperdb = new SurveyManageDB();
        }

        public List<Survey_Question> GetSurvey_QuestionByExampaperID(int ExampaperID)
        {
            return Sdb.GetSurvey_QuestionByExampaperID(ExampaperID);
        }

        public List<Survey_Question> GetSurvey_QuestionByUserID(int courseID, int paperID, int userID)
        {
            return Sdb.GetSurvey_QuestionByUserID(courseID, paperID, userID);
        }
        public Survey_Question GetSurvey_QuestionByQuestionID(int QuestionID)
        {
            return Sdb.GetSurvey_QuestionByQuestionID(QuestionID);
        }

        public decimal GetSurvey_QuestionAvg(int courseID, int questionID, int exampaperID, int questionType)
        {
            return Sdb.GetSurvey_QuestionAvg(courseID, questionID, exampaperID, questionType);
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
        public decimal GetNewSurveyQuestionAvg(int courseId, int questionId, int exampaperId,int courseRoomRuleId, int questionType)
        {
            return Sdb.GetNewSurveyQuestionAvg(courseId, questionId, exampaperId, courseRoomRuleId, questionType);
        }

        /// <summary>
        /// 获取问答卷列表
        /// </summary>
        /// <param name="ExampaperID"></param>
        /// <returns></returns>
        public List<Survey_QuestionAndAnswer> GetSurvey_QuestionAndAnswerByExampaperID(int ExampaperID)
        {
            List<Survey_QuestionAndAnswer> qaaList = null;
            var t = SurveyExampaperdb.GetServeyExamPaper(ExampaperID);
            if (t != null)
            {

                var SurveyQuestionList = GetSurvey_QuestionByExampaperID(ExampaperID);

                qaaList = SurveyQuestionList.Select(c => new Survey_QuestionAndAnswer
                {
                    sq = c,
                    sqalist = sqadb.GetSurvey_QuestionAnswerByQuestionID(c.QuestionID).ToList()
                }).ToList();
                return qaaList;
            }
            return qaaList;
        }



    }
}
