﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.Survey;

namespace LiXinInterface.DeptSurvey
{
    public interface IDeptSurveyExampaper
    {
        /// <summary>
        /// 根据ID获取问卷分类
        /// </summary>
        /// <param name="sortID"></param>
        /// <returns></returns>
        Survey_ExampaperCategory GetPaperSort(int sortID);

        /// <summary>
        /// 获取所有问卷分类
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<Survey_ExampaperCategory> GetPaperSortList(string where = " IsDelete=0 ",
                                                        string orderBy = " order by CategoryId desc ");

        /// <summary>
        /// 增加问卷分类
        /// </summary>
        /// <param name="model">问卷分类实体</param>
        bool AddPaperSort(Survey_ExampaperCategory model);

        /// <summary>
        /// 增加问卷分类(部门分所开放至全所使用)
        /// </summary>
        /// <param name="model">问卷分类实体</param>
        int AddPaperSortModel(Survey_ExampaperCategory model);

        /// <summary>
        /// 修改问卷分类
        /// </summary>
        /// <param name="model">问卷分类</param>
        bool EditPaperSort(Survey_ExampaperCategory model);

        /// <summary>
        /// 删除问卷分类
        /// </summary>
        /// <param name="id">问卷分类ID</param>
        /// <returns></returns>
        bool DeletePaperSort(int id);

        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="ids">问卷ID</param>
        /// <returns></returns>
        bool DeleteServeyExamPaper(string ids);

        /// <summary>
        /// 判断问卷分类是否存在
        /// </summary>
        /// <param name="sortName">问卷分类名称</param>
        /// <param name="sortID">问卷分类ID</param>
        /// <param name="parentID">父级ID</param>
        bool IsExsitPaperSort(string sortName, int sortID, int parentID, string where = " 1=1");

        /// <summary>
        /// 根据ID获取问卷
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        Survey_Exampaper GetServeyExamPaper(int paperID);

        /// <summary>
        /// 获取所有问卷
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<Survey_Exampaper> GetServeyExamPaperList(string where = " IsDelete=0 ",
                                                             string orderBy = " order by ExampaperID desc ");

        /// <summary>
        /// 获取所有问卷(分页)
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<Survey_Exampaper> GetServeyExamPaperListPaging(out int totalCount, string where = " IsDelete=0 ",
                                                                   int startIndex = 0, int pageLength = int.MaxValue,
                                                                   string orderBy = "  LastUpdateTime desc ");

        /// <summary>
        /// 增加问卷
        /// </summary>
        /// <param name="model">问卷实体</param>
        bool AddServeyExamPaper(Survey_Exampaper model);

        /// <summary>
        /// 修改问卷
        /// </summary>
        /// <param name="model">问卷</param>
        bool EditServeyExamPaper(Survey_Exampaper model);

        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="id">问卷ID</param>
        /// <returns></returns>
        bool DeleteServeyExamPaper(int id);

        /// <summary>
        /// 判断问卷是否存在
        /// </summary>
        /// <param name="paperName">问卷名称</param>
        /// <param name="paperID">问卷ID</param>
        /// <param name="sortID">分类ID</param>
        bool IsExsitServeyExamPaper(string paperName, int paperID, int sortID);

        /// <summary>
        /// 添加调查问卷
        /// </summary>
        /// <param name="exampaper"></param>
        void AddExampaper(Survey_Exampaper exampaper);

        /// <summary>
        /// 根据问卷ID，查询出问卷的基本信息
        /// </summary>
        /// <param name="paperID"></param>
        /// <returns></returns>
        Survey_Exampaper GetSurveyExampaper(int paperID);

        /// <summary>
        /// 获取问题的所有答案
        /// </summary>
        /// <param name="QuestionID"></param>
        /// <returns></returns>
        List<Survey_QuestionAnswer> GetSurvey_QuestionAnswerByQuestionID(int QuestionID);

        /// <summary>
        /// 获取试卷的问题信息 add by yxt on 2013-08-07
        /// </summary>
        /// <param name="ExampaperID">试卷ID</param>
        /// <returns></returns>
        List<Survey_Question> GetSurvey_QuestionByExampaperID(int ExampaperID);

        /// <summary>
        /// 获取调查问卷列表
        /// </summary>
        /// <param name="ExampaperID"></param>
        /// <returns></returns>
        List<Survey_QuestionAnswer> GetSurvey_QuestionAndAnswerByExampaperID(int ExampaperID);


        /// <summary>
        /// 查看平均分
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="questionID"></param>
        /// <param name="exampaperID"></param>
        /// <returns></returns>
        decimal GetSurvey_QuestionAvg(int courseID, int questionID, int exampaperID, int questionType);
    }
}
