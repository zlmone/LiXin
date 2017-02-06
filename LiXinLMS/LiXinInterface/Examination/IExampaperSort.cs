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
        int ModifyByID(int id, tbExampaperSort sort);

        /// <summary>
        ///     获取试卷分类列表
        /// </summary>
        List<tbExampaperSort> GetAllExampaperSortList();

        /// <summary>
        ///     获取试卷分类字典
        /// </summary>
        Dictionary<int, tbExampaperSort> GetAllExampaperSortDictionary();
    }
}