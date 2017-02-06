using System.Collections.Generic;
using LiXinModels.Examination.DBModel;

namespace LiXinInterface.Examination
{
    public interface IQuestion
    {
        /// <summary>
        ///     获取所有的试题
        /// </summary>
        /// <returns></returns>
        List<tbQuestion> GetQuestionList();

        /// <summary>
        ///     插入问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        int Insert(tbQuestion question);

        /// <summary>
        ///     删除题库
        /// </summary>
        /// <param name="questionID"></param>
        /// <returns></returns>
        bool Delete(string questionID);

        /// <summary>
        ///     修改试题
        /// </summary>
        /// <param name="question">修改内容的集合</param>
        /// <returns></returns>
        int ModifyByID(tbQuestion question);

        /// <summary>
        ///     根据ID获取试题
        /// </summary>
        /// <returns></returns>
        tbQuestion GetSingleByID(int id);

        /// <summary>
        ///     获取所有的试题
        /// </summary>
        /// <param name="qIDs">传入的问题字符串，以逗号隔开</param>
        /// <returns></returns>
        List<tbQuestion> GetQuestionList(string qIDs);


        /// <summary>
        ///     根据条件查询问题列表并且分页
        /// </summary>
        /// <param name="sortID">分类ID</param>
        /// <param name="content">题目</param>
        /// <param name="type">类型</param>
        /// <param name="level">难度</param>
        /// <param name="keyName">知识点</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="totalcount">总数</param>
        /// <param name="deptId">部门ID集合</param>
        /// <returns></returns>
        List<tbQuestion> GetListByQuery(ref int totalcount, int sortID, string content, string keyName, int pageSize,
                                        int pageIndex, int type, int level, List<int> deptId = null);
    }
}