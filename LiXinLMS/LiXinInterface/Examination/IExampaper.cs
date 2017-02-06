using System.Collections.Generic;
using LiXinModels.Examination.DBModel;

namespace LiXinInterface.Examination
{
    public interface IExampaper
    {
        /// <summary>
        ///     添加试卷
        /// </summary>
        int InsertExampaper(tbExampaper sort);

        /// <summary>
        ///     获取试卷列表
        /// </summary>
        List<tbExampaper> GetAllExampaperList();

        /// <summary>
        ///     根据ID获取单个实体
        /// </summary>
        tbExampaper GetExampaper(int id);

        /// <summary>
        ///     根据ID删除单个实体
        /// </summary>
        bool DeleteExampaper(int id, bool isTrueDelete);

        /// <summary>
        ///     根据ID批量删除单个实体
        /// </summary>
        bool DeleteExampapers(string ids);

        /// <summary>
        ///     根据ID修改单个实体
        /// </summary>
        bool UpdateExampaper(tbExampaper tbE);

        /// <summary>
        ///     获取指定ID集合的考试
        /// </summary>
        /// <param name="paperIDs">考试ID集合</param>
        /// <param name="isContainDeleted">true:包含已被删除的；false:去除已被删除的</param>
        /// <returns></returns>
        List<tbExampaper> GetAllExampaper(IEnumerable<int> paperIDs, bool isContainDeleted = false);


    }
}