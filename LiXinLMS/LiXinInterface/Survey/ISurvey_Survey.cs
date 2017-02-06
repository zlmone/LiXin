using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Survey;
using LiXinModels.User;

namespace LiXinInterface.Survey
{
    public interface ISurvey_Survey
    {
        /// <summary>
        /// 查询调查管理的基本信息
        /// </summary>
        List<Survey_Survey> GetSurveyList(out int totalCount, string Name = "", string startTime = "", String endTime = "", int status = -1, int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " PublishTime desc");

        /// <summary>
        /// 单个查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Survey_Survey Get(int id);

        /// <summary>
        /// 插入调查
        /// </summary>
        /// <param name="model"></param>
        void InsertSurvey_Survey(Survey_Survey model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        void UpdateSurvey_Survey(Survey_Survey model);

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id"></param>
        /// <param name="where"></param>
        void Publish(int id);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        ///  参与需求调查首页
        /// </summary>
        /// <returns></returns>
        List<Survey_Survey> GetDoSurveyList(int userID, string Name = "", string startTime = "", string endTime = "", int IsAccessed = -1);

        /// <summary>
        /// 问卷审核首页
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<Survey_Survey> GetSurveyCheckForDept(int userID);

        /// <summary>
        /// 获取问题基本情况
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <returns></returns>
        List<Survey_Question> GetQuestionBySurvey(int surveyID, int paperID, int userID);

        /// <summary>
        ///  获取回答的基本情况
        /// </summary>
        /// <returns></returns>
        List<Survey_ReplyAnswer> GetReplayAnswer(int surveyID, int paperID, int questionID, int userID, string where = "1=1");


        /// <summary>
        /// 部门负责人的采纳
        /// </summary>
        void DeptAccessed(int AnswerId);


        /// <summary>
        /// 部门负责人的推荐
        /// </summary>
        void DeptForOffice(int AnswerId);

        /// <summary>
        /// 事务所的采纳
        /// </summary>
        void OfficeAccessed(int AnswerId);

        /// <summary>
        /// 这个部门负责人下面所有的人员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<Sys_User> GetDeptUser(int surveyID, int paperID, int userID, string name = "", string grade = "");

        /// <summary>
        /// 根据问卷ID，查询出问卷的基本信息
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        List<Survey_Question> GetStuQuestion(int surveyID, int paperID, int userID, string where = " 1=1");

        /// <summary>
        /// 学员的答案
        /// </summary>
        List<Survey_ReplyAnswer> GetAnswerForStu(int surveyID, int paperID, int userID, string where = " 1=1");

        /// <summary>
        /// 维护部门状态表
        /// </summary>
        /// <param name="depID"></param>
        /// <param name="surveyID"></param>
        void InsertSurvey_DeptSurvey(int depID, int surveyID);

        /// <summary>
        /// 问卷审核首页(事务所)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<Survey_Survey> GetSurveyCheckForOffice();

        /// <summary>
        /// 树
        /// </summary>
        /// <returns></returns>
        List<Sys_Department> GetDeptForDepartment(int surveyID, int paperID);


        /// <summary>
        /// 获取部门负责人所在的部门ID
        /// </summary>
        /// <returns></returns>
        int GetDeptID(int userID);

        /// <summary>
        /// 获取问题
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        List<Survey_Question> GetQuestionSurveyForOffice(int surveyID, int paperID, int DeptID);

        /// <summary>
        ///  获取回答的基本情况
        /// </summary>
        /// <returns></returns>
        List<Survey_ReplyAnswer> GetOfficeReplayAnswer(int surveyID, int paperID, int questionID, int userID, int deptID, string where = "1=1");

        /// <summary>
        /// 这个部门负责人下面所有的人员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<Sys_User> GetStudentForOffice(int surveyID, int paperID, string name = "", string grade = "", int deptID = 0);

        /// <summary>
        /// 星评题的分数
        /// </summary>
        /// <returns></returns>
        List<Survey_ReplyAnswer> GetXinPAnswers(int QuestionID, int paperID, int surveyID,int userID);

         /// <summary>
        /// 星评题的分数,汇总
        /// </summary>
        /// <returns></returns>
        List<Survey_ReplyAnswer> GetXinPAnswersOffice(int QuestionID, int paperID, int surveyID);

    }
}
