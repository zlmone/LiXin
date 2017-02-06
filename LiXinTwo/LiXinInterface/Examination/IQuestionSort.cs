using System.Collections.Generic;
using LiXinModels.Examination.DBModel;

namespace LiXinInterface.Examination
{
    public interface IQuestionSort
    {
        /// <summary>
        ///     获取问题分类字典
        /// </summary>
        /// <param name="deptId">部门ID集合</param>
        /// <returns></returns>
        Dictionary<int, tbQuestionSort> GetAllQuestionSortDictionary(List<int> deptId=null);

        /// <summary>
        ///     获取所有的试题库
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        List<tbQuestionSort> GetAllQuestionSortList(List<int> deptId=null);

        /// <summary>
        ///     修改试题库
        /// </summary>
        /// <param name="qSort"></param>
        /// <returns></returns>
        int ModifyByID(tbQuestionSort qSort);

        /// <summary>
        ///     插入问题
        /// </summary>
        /// <param name="questionSort"></param>
        /// <returns></returns>
        int Insert(tbQuestionSort questionSort);

        /// <summary>
        ///     删除题库
        /// </summary>
        /// <param name="questionSortID"></param>
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

        /// <summary>
        /// 判断是否已经存在此分类（部门分所开放至全所使用add by yxt）
        /// </summary>
        /// <param name="title">问题分类标题</param>
        /// <param name="deptId">所属部门ID</param>
        ///  <param name="fatherId">父级ID</param>
        /// <returns></returns>
        bool IsExsitCoQuestionSortName(string title, int deptId, int fatherId);
    }
}