using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.Survey;
using LiXinInterface.Survey;
using LiXinModels.Survey;

namespace LiXinBLL.Survey
{
    public class SurveyQuestionAnswerBL : ISurveyQuestionAnswer
    {
        protected SurveyQuestionAnswerDB Sdb;
         public SurveyQuestionAnswerBL()
        {
            Sdb = new SurveyQuestionAnswerDB();
        }


        public List<Survey_QuestionAnswer> GetSurvey_QuestionAnswerByQuestionID(int QuestionID)
        {
            return Sdb.GetSurvey_QuestionAnswerByQuestionID(QuestionID);
        }
    }
}
