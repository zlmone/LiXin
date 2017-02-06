using System.Collections.Generic;
using LiXinModels.Examination.DBModel;

namespace LiXinInterface.Examination
{
    public interface IQuestionSort
    {
        /// <summary>
        ///     获取问题分类字典
        /// </summary>
        /// <returns></returns>
        Dictionary<int, tbQuestionSort> GetAllQuestionSortDictionary();

        /// <summary>
        ///     获取所有的试题库
        /// </summary>
        /// <returns></returns>
        List<tbQuestionSort> GetAllQuestionSortList();

        /// <summary>
        ///     修改试题库
        /// </summary>
        /// <param name="SortID"></param>
        /// <param name="qSort"></param>
        /// <returns></returns>
        int ModifyByID(int SortID, tbQuestionSort qSort);

        /// <summary>
        ///     插入问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        int Insert(tbQuestionSort questionSort);

        /// <summary>
        ///     删除题库
        /// </summary>
        /// <param name="questionID"></param>
        /// <returns></returns>
        bool Delete(int questionSortID);


        /// <summary>
        ///     获取单个的题库
        /// </summary>
        /// <param name="sortID"></param>
        /// <returns></returns>
        tbQuestionSort GetSingleByID(int sortID);

        /// <summary>
        ///     验证是否已存在此库名（同级下是否存在）
        /// </summary>
        /// <param name="pId">父级ID</param>
        /// <param name="tId">待修改的ID</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        bool IsExsitName(int pId, int tId, string name);
    }
}