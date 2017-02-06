using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Survey;

namespace LiXinInterface.Survey
{
    public interface ISurveyQuestionAnswer
    {
        List<Survey_QuestionAnswer> GetSurvey_QuestionAnswerByQuestionID(int QuestionID);
    }
}
