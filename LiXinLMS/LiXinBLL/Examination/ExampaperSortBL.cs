using System.Collections.Generic;
using LiXinDataAccess.Examination;
using LiXinInterface.Examination;
using LiXinModels.Examination.DBModel;

namespace LiXinBLL.Examination
{
    public class ExampaperSortBL : IExampaperSort
    {
        private static ExampaperSortDB eDB;

        public ExampaperSortBL()
        {
            eDB = new ExampaperSortDB();
        }

        /// <summary>
        ///     添加新的问题分类
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public int Insert(tbExampaperSort sort)
        {
            return eDB.Insert(sort);
        }

        /// <summary>
        ///     根据ID获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tbExampaperSort GetSingleByID(int id)
        {
            return eDB.GetSingleByID<tbExampaperSort>(id);
        }

        /// <summary>
        ///     根据ID删除单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isTrueDelete">是否真删</param>
        /// <returns></returns>
        public bool DeleteSingleByID(int id, bool isTrueDelete)
        {
            return eDB.Delete<tbExampaperSort>(id, isTrueDelete);
        }

        /// <summary>
        ///     根据ID修改分类
        /// </summary>
        /// <returns></returns>
        public int ModifyByID(int id, tbExampaperSort sort)
        {
            return eDB.ModifyByID(id, sort);
        }

        /// <summary>
        ///     获取问题分类列表
        /// </summary>
        /// <returns></returns>
        public List<tbExampaperSort> GetAllExampaperSortList()
        {
            return eDB.GetAllList<tbExampaperSort>(false);
        }

        /// <summary>
        ///     获取问题分类字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, tbExampaperSort> GetAllExampaperSortDictionary()
        {
            return eDB.GetAllExampaperSortDictionary();
        }
    }
}