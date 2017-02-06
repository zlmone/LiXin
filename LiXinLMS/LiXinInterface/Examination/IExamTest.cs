using System.Collections.Generic;
using LiXinModels.Examination.DBModel;
using LiXinModels.Examination.ShowModel;

namespace LiXinInterface.Examination
{
    public interface IExamTest
    {
        /// <summary>
        ///     获取试题集合
        /// </summary>
        /// <param name="quIDlist">试题ID集合</param>
        /// <returns></returns>
        List<tbQuestion> GetQuestionList(IEnumerable<int> quIDlist);

        /// <summary>
        ///     获取单个考试人员答案
        /// </summary>
        /// <param name="euID">考试人员ID</param>
        /// <returns></returns>
        tbExamSendStudent GetExamUser(int euID);

        /// <summary>
        ///     保存单个考试人员答案
        /// </summary>
        /// <param name="eu">考试人员信息</param>
        /// <returns></returns>
        bool SaveExamUser(tbExamSendStudent eu);

        /// <summary>
        ///     获取我的考试
        /// </summary>
        /// <param name="userID">UserID</param>
        /// <returns></returns>
        List<tbExamSendStudent> GetMyExamList(int userID);

        /// <summary>
        ///     获取考试下的参考人员
        /// </summary>
        /// <param name="examIDs">考试ID集合</param>
        /// <returns></returns>
        List<tbExamSendStudent> GetExamSendStudentList(IEnumerable<int> examIDs);

        /// <summary>
        ///     获取考试的基本信息
        /// </summary>
        /// <param name="examUserID">考试人员ID</param>
        /// <param name="type">0:独立的考试</param>
        /// <returns></returns>
        ExamBaseInforShow GetExamBaseInforShow(int examUserID, int type = 0);

        /// <summary>
        ///     获取考试的基本信息
        /// </summary>
        /// <param name="examUserID">考试人员ID</param>
        /// <param name="type">0:独立的考试</param>
        /// <returns></returns>
        ExampaperShow GetExampaperBaseInforShow(int examUserID, int type = 0);

        /// <summary>
        ///     获取我的考试试题
        /// </summary>
        /// <returns></returns>
        List<tbQuestion> GetMyExaminationQuestionList(int euID);
    }
}