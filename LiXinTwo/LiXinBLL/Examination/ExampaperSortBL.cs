using System.Collections.Generic;
using LiXinDataAccess.Examination;
using LiXinInterface.Examination;
using LiXinModels.Examination.DBModel;
using MongoDB.Driver.Builders;

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
        /// <param name="sort"></param>
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
        public int ModifyByID(tbExampaperSort sort)
        {
            eDB.Modify(sort);
            return sort._id;
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

        /// <summary>
        /// 判断是否已经存在此分类（部门分所开放至全所使用add by yxt）
        /// </summary>
        /// <param name="title">试卷分类标题</param>
        /// <param name="deptId">所属部门ID</param>
        ///  <param name="fatherId">父级ID</param>
        /// <returns></returns>
        public bool IsExsitCoExampaperSortName(string title, int deptId, int fatherId)
        {
            return eDB.IsExsitName<tbExampaperSort>(Query.And(new[]
                                                                  {
                                                                      Query.EQ("Title", title),
                                                                      Query.EQ("DeptId", deptId),
                                                                      Query.EQ("FatherID",fatherId),
                                                                      Query.EQ("Status", 0)
                                                                  }));
        }
    }
}