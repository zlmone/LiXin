using LiXinModels.Survey;
using LiXinModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.DeptSurvey
{
    public interface IDepSurvey_Survey
    {
        /// <summary>
        /// 查询调查管理的基本信息
        /// </summary>
        List<Survey_Survey> GetSurveyList(out int totalCount, int deptID, string Name = "", string startTime = "", String endTime = "", int status = -1, int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " PublishTime desc");

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
        /// 获取部门负责人所在的部门ID
        /// </summary>
        /// <returns></returns>
        int GetDeptID(int userID);

        /// <summary>
        /// 问卷汇总
        /// </summary>
        /// <param name="userID">发布人ID</param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <param name="jsRenderSortField">排序</param>
        List<Survey_Survey> GetSurveyReport(out int totalCount,int DeptID, int userID, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Id ");


        /// <summary>
        /// 树 当有联合的组织结构时候，查询出所有的，当没有就查自己本身
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <param name="deptID"></param>
        /// <param name="OpenDepartFlag">0 没有  1有</param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Sys_Department> GetDeptForDepartment(int surveyID, int paperID, int deptID, int OpenDepartFlag);

        /// <summary>
        /// 根据部门获取问题
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        List<Survey_Question> GetQuestionSurveyByDeptID(int surveyID, int paperID, int DeptID);

        /// <summary>
        /// 获取答案详情
        /// </summary>
        /// <returns></returns>
        List<Survey_ReplyAnswer> GetOfficeReplayAnswer(int surveyID, int paperID, int questionID, int userID, int deptID, string where = "1=1");

        /// <summary>
        /// 星评题的分数,汇总
        /// </summary>
        /// <returns></returns>
        List<Survey_ReplyAnswer> GetXinPAnswersOffice(int QuestionID, int paperID, int surveyID, int DeptID);

        /// <summary>
        /// 这个部门负责人下面所有的人员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<Sys_User> GetStudentByDept(int surveyID, int paperID, int deptID, int OpenDepartFlag, string name = "", string grade = "");

        /// <summary>
        /// 学员的答案
        /// </summary>
        List<Survey_ReplyAnswer> GetAnswerForStu(int surveyID, int paperID, int userID);

        /// <summary>
        /// 根据问卷ID，查询出问卷的基本信息
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        List<Survey_Question> GetStuQuestion(int surveyID, int paperID, int userID, string where = " 1=1");
    }
}
