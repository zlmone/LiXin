using System.Collections.Generic;
using LiXinModels.Examination.DBModel;

namespace LiXinInterface.Examination
{
    public interface IExampaperSort
    {
        /// <summary>
        ///     添加新的试卷分类
        /// </summary>
        int Insert(tbExampaperSort sort);

        /// <summary>
        ///     根据ID获取单个实体
        /// </summary>
        tbExampaperSort GetSingleByID(int id);

        /// <summary>
        ///     根据ID删除单个实体
        /// </summary>
        bool DeleteSingleByID(int id, bool isTrueDelete);

        /// <summary>
        ///     根据ID修改分类
        /// </summary>
        int ModifyByID(tbExampaperSort sort);

        /// <summary>
        ///     获取试卷分类列表
        /// </summary>
        List<tbExampaperSort> GetAllExampaperSortList();

        /// <summary>
        ///     获取试卷分类字典
        /// </summary>
        Dictionary<int, tbExampaperSort> GetAllExampaperSortDictionary();

        /// <summary>
        /// 判断是否已经存在此分类（部门分所开放至全所使用add by yxt）
        /// </summary>
        /// <param name="title">试卷分类标题</param>
        /// <param name="deptId">所属部门ID</param>
        ///  <param name="fatherId">父级ID</param>
        /// <returns></returns>
        bool IsExsitCoExampaperSortName(string title, int deptId, int fatherId);
    }
}