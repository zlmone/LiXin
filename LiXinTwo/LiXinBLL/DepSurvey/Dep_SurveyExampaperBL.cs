using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DeptSurvey;
using LiXinModels.Survey;
using LiXinInterface.DeptSurvey;

namespace LiXinBLL.DeptSurvey
{
    public class Dep_SurveyExampaperBL : IDeptSurveyExampaper
    {
        protected SurveyExampaperDB Sdb;
        protected SurveyQuestionDB qdb;
        public Dep_SurveyExampaperBL()
        {
            Sdb = new SurveyExampaperDB();
            qdb = new SurveyQuestionDB();
        }

        #region 问卷管理
        /// <summary>
        /// 根据ID获取问卷分类
        /// </summary>
        /// <param name="sortID"></param>
        /// <returns></returns>
        public Survey_ExampaperCategory GetPaperSort(int sortID)
        {
            return Sdb.GetPaperSort(sortID);
        }

        /// <summary>
        /// 获取所有问卷分类
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<Survey_ExampaperCategory> GetPaperSortList(string where = " IsDelete=0 ", string orderBy = " order by CategoryId desc ")
        {
            return Sdb.GetPaperSortList(where, orderBy);
        }

        /// <summary>
        /// 增加问卷分类
        /// </summary>
        /// <param name="model">问卷分类实体</param>
        public bool AddPaperSort(Survey_ExampaperCategory model)
        {
            return Sdb.AddPaperSort(model);
        }

        /// <summary>
        /// 增加问卷分类(部门分所开放至全所使用)
        /// </summary>
        /// <param name="model">问卷分类实体</param>
        public int AddPaperSortModel(Survey_ExampaperCategory model)
        {
            Sdb.AddPaperSort(model);
            return model.CategoryId;
        }

        /// <summary>
        /// 修改问卷分类
        /// </summary>
        /// <param name="model">问卷分类</param>
        public bool EditPaperSort(Survey_ExampaperCategory model)
        {
            return Sdb.EditPaperSort(model);
        }

        /// <summary>
        /// 删除问卷分类
        /// </summary>
        /// <param name="id">问卷分类ID</param>
        /// <returns></returns>
        public bool DeletePaperSort(int id)
        {
            return Sdb.DeletePaperSort(id);
        }

        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="ids">问卷ID</param>
        /// <returns></returns>
        public bool DeleteServeyExamPaper(string ids)
        {
            return Sdb.DeleteServeyExamPaper(ids);
        }

        /// <summary>
        /// 判断问卷分类是否存在
        /// </summary>
        /// <param name="sortName">问卷分类名称</param>
        /// <param name="sortID">问卷分类ID</param>
        /// <param name="parentID">父级ID</param>
        public bool IsExsitPaperSort(string sortName, int sortID, int parentID,string where=" 1=1")
        {
            return Sdb.IsExsitPaperSort(sortName, sortID, parentID, where);
        }

        /// <summary>
        /// 根据ID获取问卷
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public Survey_Exampaper GetServeyExamPaper(int paperID)
        {
            return Sdb.GetServeyExamPaper(paperID);
        }

        /// <summary>
        /// 获取所有问卷
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<Survey_Exampaper> GetServeyExamPaperList(string where = " IsDelete=0 ", string orderBy = " order by ExampaperID desc ")
        {
            return Sdb.GetServeyExamPaperList(where, orderBy);
        }

        /// <summary>
        /// 获取所有问卷(分页)
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<Survey_Exampaper> GetServeyExamPaperListPaging(out int totalCount, string where = " IsDelete=0 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " LastUpdateTime desc ")
        {
            return Sdb.GetServeyExamPaperListPaging(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 增加问卷
        /// </summary>
        /// <param name="model">问卷实体</param>
        public bool AddServeyExamPaper(Survey_Exampaper model)
        {
            return Sdb.AddServeyExamPaper(model);
        }

        /// <summary>
        /// 修改问卷
        /// </summary>
        /// <param name="model">问卷</param>
        public bool EditServeyExamPaper(Survey_Exampaper model)
        {
            return Sdb.EditServeyExamPaper(model);
        }

        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="id">问卷ID</param>
        /// <returns></returns>
        public bool DeleteServeyExamPaper(int id)
        {
            return Sdb.DeleteServeyExamPaper(id);
        }

        /// <summary>
        /// 判断问卷是否存在
        /// </summary>
        /// <param name="paperName">问卷名称</param>
        /// <param name="paperID">问卷ID</param>
        /// <param name="sortID">分类ID</param>
        public bool IsExsitServeyExamPaper(string paperName, int paperID, int sortID)
        {
            return Sdb.IsExsitServeyExamPaper(paperName, paperID, sortID);
        }

        /// <summary>
        /// 添加调查问卷
        /// </summary>
        /// <param name="exampaper"></param>
        public void AddExampaper(Survey_Exampaper exampaper)
        {
            //修改的时候，先全部删除，再添加
            if (exampaper.ExampaperID > 0)
            {
                Sdb.DeleteByExampaperId(exampaper.ExampaperID);
                Sdb.DeleteAnswerById(exampaper.ExampaperID);
                Sdb.EditServeyExamPaper(exampaper);
            }
            else
            {
                Sdb.AddServeyExamPaper(exampaper);
            }


            foreach (var question in exampaper.Questions)
            {
                question.ExampaperID = exampaper.ExampaperID;
                qdb.Add(question);
                foreach (var answer in question.Answers)
                {
                    answer.QuestionID = question.QuestionID;
                    qdb.Add(answer);
                }
            }
            //exampaper = exampaperRepository.GetModel(exampaper.ExampaperID);
        }

        /// <summary>
        /// 根据问卷ID，查询出问卷的基本信息
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        public Survey_Exampaper GetSurveyExampaper(int paperID)
        {
            var SurveyList = Sdb.GetSurveyByID(paperID);
            var questionlist = Sdb.GetQuestionByID(paperID);
            var answerList = Sdb.GetQuestionAnswerByID(paperID);
            foreach (var item in questionlist)
            {
                item.Answers = answerList.Where(p => p.QuestionID == item.QuestionID).ToList();
            }

            if (SurveyList.Count > 0)
            {
                var examPaper = SurveyList[0];
                examPaper.Questions = questionlist;
                return examPaper;
            }
            return new Survey_Exampaper();
        }
        #endregion


        /// <summary>
        /// 获取问题的所有答案
        /// </summary>
        /// <param name="QuestionID"></param>
        /// <returns></returns>
        public List<Survey_QuestionAnswer> GetSurvey_QuestionAnswerByQuestionID(int QuestionID)
        {
            return qdb.GetSurvey_QuestionAnswerByQuestionID(QuestionID);
        }


        /// <summary>
        /// 获取试卷的问题信息 add by yxt on 2013-08-07
        /// </summary>
        /// <param name="ExampaperID">试卷ID</param>
        /// <returns></returns>
        public List<Survey_Question> GetSurvey_QuestionByExampaperID(int ExampaperID)
        {
            return qdb.GetSurvey_QuestionByExampaperID(ExampaperID);
        }


        /// <summary>
        /// 获取问答卷列表
        /// </summary>
        /// <param name="ExampaperID"></param>
        /// <returns></returns>
        public List<Survey_QuestionAnswer> GetSurvey_QuestionAndAnswerByExampaperID(int ExampaperID)
        {
            List<Survey_QuestionAnswer> qaaList = null;
            var t = Sdb.GetServeyExamPaper(ExampaperID);
            if (t != null)
            {

                var SurveyQuestionList = GetSurvey_QuestionByExampaperID(ExampaperID);

                qaaList = SurveyQuestionList.Select(c => new Survey_QuestionAnswer
                {
                    sq = c,
                    sqalist = qdb.GetSurvey_QuestionAnswerByQuestionID(c.QuestionID).ToList()
                }).ToList();
                return qaaList;
            }
            return qaaList;
        }


        public decimal GetSurvey_QuestionAvg(int courseID, int questionID, int exampaperID, int questionType)
        {
            return qdb.GetSurvey_QuestionAvg(courseID, questionID, exampaperID, questionType);
        }



    }
}
