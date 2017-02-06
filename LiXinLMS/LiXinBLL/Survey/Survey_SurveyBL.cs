using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.Survey;
using LiXinInterface.Survey;
using LiXinModels.Survey;
using LiXinModels.User;

namespace LiXinBLL.Survey
{
    public class Survey_SurveyBL : ISurvey_Survey
    {
        protected SurveyManageDB Sdb;
        private Survey_SurveyDB surveyDB;
        private Survey_ReplyAnswer answerDB;
        private SurveyQuestionAnswerDB qaDB;
        public Survey_SurveyBL()
        {
            surveyDB = new Survey_SurveyDB();
            answerDB = new Survey_ReplyAnswer();
            Sdb = new SurveyManageDB();
            qaDB = new SurveyQuestionAnswerDB();
        }

        #region 调查管理
        /// <summary>
        /// 查询调查管理的基本信息
        /// </summary>
        public List<Survey_Survey> GetSurveyList(out int totalCount, string Name = "", string startTime = "", String endTime = "", int status = -1, int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " PublishTime desc")
        {
            string where = " 1=1";
            if (!string.IsNullOrEmpty(Name))
            {
                where += string.Format(" AND Name LIKE '%{0}%'", Name);
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                where += string.Format(" AND PublishTime>='{0}'", startTime);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                where += string.Format(" AND PublishTime<='{0}'", Convert.ToDateTime(endTime).AddDays(1).AddSeconds(-1));
            }
            if (status != -1)
            {
                where += " AND PublishFlag=" + status;
            }

            var list = surveyDB.GetSurveyList(startIndex, startLength, where, jsRenderSortField);
            totalCount = list.Count == 0 ? 0 : list[0].totalCount;
            return list;
        }

        /// <summary>
        /// 单个查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Survey_Survey Get(int id)
        {
            return surveyDB.Get(id);
        }

        /// <summary>
        /// 插入调查
        /// </summary>
        /// <param name="model"></param>
        public void InsertSurvey_Survey(Survey_Survey model)
        {
            surveyDB.InsertSurvey_Survey(model);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        public void UpdateSurvey_Survey(Survey_Survey model)
        {
            surveyDB.UpdateSurvey_Survey(model);
        }


        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id"></param>
        /// <param name="where"></param>
        public void Publish(int id)
        {
            surveyDB.PublishOrDelete(id, " PublishFlag=1,PublishTime ='" + DateTime.Now.Date + "'");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            surveyDB.PublishOrDelete(id, " IsDelete=1");
        }

        #endregion

        #region 参与需求调查
        /// <summary>
        ///  参与需求调查首页
        /// </summary>
        /// <returns></returns>
        public List<Survey_Survey> GetDoSurveyList(int userID, string Name = "", string startTime = "", string endTime = "", int IsAccessed = -1)
        {
            string where = " 1=1";
            if (!string.IsNullOrEmpty(Name))
            {
                where += " AND  Name LIKE '%" + Name + "%' ";
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                where += "  and StartTime>='" + startTime + "'";
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                where += "  and EndTime<='" + endTime + "'";
            }
            if (IsAccessed != -1)
            {
                where += " AND  IsAccessed=" + IsAccessed;
            }
            return surveyDB.GetDoSurveyList(userID, where);
        }

        /// <summary>
        /// 获取部门负责人所在的部门ID
        /// </summary>
        /// <returns></returns>
        public int GetDeptID(int userID)
        {
            return surveyDB.GetDeptID(userID);
        }
        #endregion

        #region 问卷审核(部门负责人)
        /// <summary>
        /// 问卷审核首页
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Survey_Survey> GetSurveyCheckForDept(int userID)
        {
            return surveyDB.GetSurveyCheckForDept(userID);
        }

        /// <summary>
        /// 获取问题基本情况
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public List<Survey_Question> GetQuestionBySurvey(int surveyID, int paperID, int userID)
        {
            return surveyDB.GetQuestionBySurvey(surveyID, paperID, userID);
        }

        /// <summary>
        ///  获取回答的基本情况
        /// </summary>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetReplayAnswer(int surveyID, int paperID, int questionID, int userID, string where = "1=1")
        {
            return surveyDB.GetReplayAnswer(surveyID, paperID, questionID, userID, where);
        }

        /// <summary>
        /// 部门负责人的采纳
        /// </summary>
        public void DeptAccessed(int AnswerId)
        {
            surveyDB.DeptAccessed(" IsDeptAccessed =1", AnswerId);
        }

        /// <summary>
        /// 部门负责人的推荐
        /// </summary>
        public void DeptForOffice(int AnswerId)
        {
            surveyDB.DeptAccessed(" IsOfficeAccessed = 1", AnswerId);
        }

        /// <summary>
        /// 这个部门负责人下面所有的人员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Sys_User> GetDeptUser(int surveyID, int paperID, int userID, string name = "", string grade = "")
        {
            var where = " 1=1";
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND Realname LIKE '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(grade))
            {
                where += " AND TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs('" + grade + "'))";
            }
            return surveyDB.GetDeptUser(surveyID, paperID, userID, where);
        }


        /// <summary>
        /// 根据问卷ID，查询出问卷的基本信息
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public List<Survey_Question> GetStuQuestion(int surveyID, int paperID, int userID, string where = " 1=1")
        {

            var questionlist = surveyDB.GetSurvey_QuestionForStu(surveyID, paperID, userID, where);
            var answerList = Sdb.GetQuestionAnswerByID(paperID);
            foreach (var item in questionlist)
            {
                item.Answers = answerList.Where(p => p.QuestionID == item.QuestionID).ToList();
            }

            return questionlist;
        }

        /// <summary>
        /// 学员的答案
        /// </summary>
        public List<Survey_ReplyAnswer> GetAnswerForStu(int surveyID, int paperID, int userID, string where = " 1=1")
        {
            return surveyDB.GetAnswerForStu(surveyID, paperID, userID, where);
        }

        /// <summary>
        /// 维护部门状态表
        /// </summary>
        /// <param name="depID"></param>
        /// <param name="surveyID"></param>
        public void InsertSurvey_DeptSurvey(int depID, int surveyID)
        {
            surveyDB.InsertSurvey_DeptSurvey(depID, surveyID);
        }

        /// <summary>
        /// 星评题的分数
        /// </summary>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetXinPAnswers(int QuestionID, int paperID, int surveyID, int userID)
        {
            return surveyDB.GetXinPAnswers(QuestionID, paperID, surveyID, userID);

        }

        /// <summary>
        /// 星评题的分数,汇总
        /// </summary>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetXinPAnswersOffice(int QuestionID, int paperID, int surveyID)
        {
            return surveyDB.GetXinPAnswersOffice(QuestionID, paperID, surveyID);
        }

        #endregion

        #region 问卷审核(事务所)
        /// <summary>
        /// 问卷审核首页(事务所)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Survey_Survey> GetSurveyCheckForOffice()
        {
            return surveyDB.GetSurveyCheckForOffice();
        }

        /// <summary>
        /// 事务所的采纳
        /// </summary>
        public void OfficeAccessed(int AnswerId)
        {
            surveyDB.DeptAccessed(" IsOfficeAccessed = 2", AnswerId);
        }

        /// <summary>
        /// 树
        /// </summary>
        /// <returns></returns>
        public List<Sys_Department> GetDeptForDepartment(int surveyID, int paperID)
        {
            return surveyDB.GetDeptForDepartment(surveyID, paperID);
        }

        /// <summary>
        /// 获取问题
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        public List<Survey_Question> GetQuestionSurveyForOffice(int surveyID, int paperID, int DeptID)
        {
            return surveyDB.GetQuestionSurveyForOffice(surveyID, paperID, DeptID);
        }

        /// <summary>
        ///  获取回答的基本情况
        /// </summary>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetOfficeReplayAnswer(int surveyID, int paperID, int questionID, int userID, int deptID, string where = "1=1")
        {
            return surveyDB.GetOfficeReplayAnswer(surveyID, paperID, questionID, userID, deptID, where);
        }

        /// <summary>
        /// 这个部门负责人下面所有的人员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Sys_User> GetStudentForOffice(int surveyID, int paperID, string name = "", string grade = "", int deptID = 0)
        {
            var where = " 1=1";
            if (!string.IsNullOrEmpty(name))
            {
                where += " AND Realname LIKE '%" + name + "%'";
            }
            if (!string.IsNullOrEmpty(grade))
            {
                where += " AND TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs('" + grade + "'))";
            }
            return surveyDB.GetStudentForOffice(surveyID, paperID, deptID, where);
        }
        #endregion


    }
}
