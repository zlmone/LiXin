using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DeptSurvey;
using LiXinModels.Survey;
using LiXinModels.User;
using LiXinInterface.DeptSurvey;

namespace LiXinBLL.DeptSurvey
{
    public class Dep_Survey_SurveyBL : IDepSurvey_Survey
    {
        private Survey_SurveyDB surveyDB;
        protected SurveyExampaperDB Sdb;
        public Dep_Survey_SurveyBL()
        {
            surveyDB = new Survey_SurveyDB();
            Sdb = new SurveyExampaperDB();
        }

        #region 调查管理
        /// <summary>
        /// 查询调查管理的基本信息
        /// </summary>
        public List<Survey_Survey> GetSurveyList(out int totalCount, int deptID, string Name = "", string startTime = "", String endTime = "", int status = -1, int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " PublishTime desc")
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
            where += string.Format(" And DeptID in (SELECT ID FROM dbo.f_GetDeptChildByDeptID({0}))", deptID);

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

        #region 问卷汇总
        /// <summary>
        /// 问卷汇总
        /// </summary>
        /// <param name="userID">发布人ID</param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <param name="jsRenderSortField">排序</param>
        public List<Survey_Survey> GetSurveyReport(out int totalCount,int DeptID, int userID, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Id ")
        {
            var list = surveyDB.GetSurveyReport(userID,DeptID, startIndex, startLength, where, jsRenderSortField);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        /// <summary>
        /// 树 当有联合的组织结构时候，查询出所有的，当没有就查自己本身
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <param name="deptID"></param>
        /// <param name="OpenDepartFlag">0 没有  1有</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Sys_Department> GetDeptForDepartment(int surveyID, int paperID, int deptID, int OpenDepartFlag)
        {
            var where = " sd.DepartmentId=" + deptID;
            if (OpenDepartFlag == 1)
            {
                where += string.Format(" or sd.DepartmentId IN (SELECT Id FROM dbo.F_SplitIDs(@OpenDepart))", surveyID);
            }
            return surveyDB.GetDeptForDepartment(surveyID, paperID, where);
        }

        /// <summary>
        /// 根据部门获取问题
        /// </summary>
        /// <param name="surveyID"></param>
        /// <param name="paperID"></param>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        public List<Survey_Question> GetQuestionSurveyByDeptID(int surveyID, int paperID, int DeptID)
        {
            return surveyDB.GetQuestionSurveyByDeptID(surveyID, paperID, DeptID);
        }

        /// <summary>
        /// 获取答案详情
        /// </summary>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetOfficeReplayAnswer(int surveyID, int paperID, int questionID, int userID, int deptID, string where = "1=1")
        {
            return surveyDB.GetOfficeReplayAnswer(surveyID, paperID, questionID, userID, deptID, where);
        }

        /// <summary>
        /// 星评题的分数,汇总
        /// </summary>
        /// <returns></returns>
        public List<Survey_ReplyAnswer> GetXinPAnswersOffice(int QuestionID, int paperID, int surveyID,int DeptID)
        {
            return surveyDB.GetXinPAnswersOffice(QuestionID, paperID, surveyID, DeptID);
        }

        /// <summary>
        /// 这个部门负责人下面所有的人员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Sys_User> GetStudentByDept(int surveyID, int paperID, int deptID, int OpenDepartFlag, string name = "", string grade = "")
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
            if (OpenDepartFlag == 1)
            {
                where += " and DeptId IN (SELECT Id FROM dbo.F_SplitIDs(@OpenDepart))";
            }

            return surveyDB.GetStudentByDept(surveyID, paperID, deptID, where);
        }

        /// <summary>
        /// 学员的答案
        /// </summary>
        public List<Survey_ReplyAnswer> GetAnswerForStu(int surveyID, int paperID, int userID)
        {
            return surveyDB.GetAnswerForStu(surveyID, paperID, userID);
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
        #endregion
    }
}
