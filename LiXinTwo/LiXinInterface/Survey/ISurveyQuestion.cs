using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseManage;
using LiXinModels.Survey;

namespace LiXinInterface.Survey
{
    public interface ISurveyQuestion
    {
        /// <summary>
        /// 根据所属问卷把问卷都找出来
        /// </summary>
        /// <param name="ExampaperID"></param>
        /// <returns></returns>
        List<Survey_Question> GetSurvey_QuestionByExampaperID(int ExampaperID);

        /// <summary>
        /// 根据人员查出回答的答案
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="paperID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<Survey_Question> GetSurvey_QuestionByUserID(int courseID, int paperID, int userID);

        /// <summary>
        /// 查找单个信息
        /// </summary>
        /// <param name="QuestionID"></param>
        /// <returns></returns>
        Survey_Question GetSurvey_QuestionByQuestionID(int QuestionID);


        /// <summary>
        /// 获取调查问卷列表
        /// </summary>
        /// <param name="ExampaperID"></param>
        /// <returns></returns>
        List<Survey_QuestionAndAnswer> GetSurvey_QuestionAndAnswerByExampaperID(int ExampaperID);

        /// <summary>
        /// 算平均分
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="questionID"></param>
        /// <param name="exampaperID"></param>
        /// <returns></returns>
        //decimal GetSurvey_QuestionAvg(int courseID, int questionID, int exampaperID);
        decimal GetSurvey_QuestionAvg(int courseID, int questionID, int exampaperID, int questionType);

        /// <summary>
        /// 查看平均分(新进员工对讲师评价使用)
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <param name="questionId">问题ID</param>
        /// <param name="exampaperId">问卷ID</param>
        /// <param name="courseRoomRuleId">新员工课程教室分配规则ID</param>
        /// <param name="questionType">问题类型 0：单选题；1：多选题；2：主观题; 3:星评题</param>
        /// <returns></returns>
        decimal GetNewSurveyQuestionAvg(int courseId, int questionId, int exampaperId, int courseRoomRuleId,
                                        int questionType);
    }
}
