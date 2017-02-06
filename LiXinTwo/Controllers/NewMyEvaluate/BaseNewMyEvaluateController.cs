using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;
using LiXinInterface.NewClassManage;
using LiXinInterface.NewCourseManage;
using LiXinInterface.Survey;
using LiXinInterface.User;
using LiXinModels;
using System.Web.Mvc;
namespace LiXinControllers.NewMyEvaluate
{
    public partial class NewMyEvaluateController : BaseController
    {
        #region  == 接口实例化 ==
        protected INew_Course NewCourseBl;
        protected INew_CourseRoomRule NewCourseRoomRuleBl;
        protected IUser UserBl;
        protected ISurveyQuestion SurveyQuestionBl;
        protected ISurveyReplyAnswer SurveyReplyAnswerBl;
        
        public NewMyEvaluateController(
            INew_Course newCourseBl
            , INew_CourseRoomRule newCourseRoomRuleBl
            ,IUser userBl
            , ISurveyQuestion surveyQuestionBl
            , ISurveyReplyAnswer surveyReplyAnswerBl)
        {
            NewCourseBl = newCourseBl;
            NewCourseRoomRuleBl = newCourseRoomRuleBl;
            UserBl = userBl;
            SurveyQuestionBl = surveyQuestionBl;
            SurveyReplyAnswerBl = surveyReplyAnswerBl;
        }
        #endregion
    }
}
