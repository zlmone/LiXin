using System.Collections.Generic;
using System.Linq;
using LiXinDataAccess.Examination;
using LiXinInterface.Examination;
using LiXinModels.Examination.DBModel;

namespace LiXinBLL.Examination
{
    public class QuestionSortBL : IQuestionSort
    {
        private readonly QuestionSortDB sortDB;

        public QuestionSortBL()
        {
            sortDB = new QuestionSortDB();
        }

        /// <summary>
        ///     获取问题分类字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, tbQuestionSort> GetAllQuestionSortDictionary()
        {
            return sortDB.GetAllQuestionSortDictionary();
        }

        /// <summary>
        ///     获取所有的试题库
        /// </summary>
        /// <returns></returns>
        public List<tbQuestionSort> GetAllQuestionSortList()
        {
            return sortDB.GetAllQuestionSortList();
        }


        /// <summary>
        ///     修改试题库
        /// </summary>
        /// <param name="SortID"></param>
        /// <param name="qSort"></param>
        /// <returns></returns>
        public int ModifyByID(int SortID, tbQuestionSort qSort)
        {
            return sortDB.ModifyByID(SortID, qSort);
        }

        /// <summary>
        ///     插入问题
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public int Insert(tbQuestionSort questionSort)
        {
            return sortDB.Insert(questionSort);
        }

        /// <summary>
        ///     删除题库
        /// </summary>
        /// <param name="questionID"></param>
        /// <returns></returns>
        public bool Delete(int questionSortID)
        {
            return sortDB.Delete<tbQuestionSort>(questionSortID, false);
        }

        /// <summary>
        ///     获取单个的题库
        /// </summary>
        /// <param name="sortID"></param>
        /// <returns></returns>
        public tbQuestionSort GetSingleByID(int sortID)
        {
            return sortDB.GetSingleByID<tbQuestionSort>(sortID);
        }

        /// <summary>
        ///     验证是否已存在此库名（同级下是否存在）
        /// </summary>
        /// <param name="pId">父级ID</param>
        /// <param name="tId">待修改的ID</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public bool IsExsitName(int pId, int tId, string name)
        {
            return sortDB.GetAllQuestionSortList().Where(p => p.FatherID == pId
                                                              && p.Status == 0
                                                              && (
                                                                     (tId == 0 && p.Title == name) ||
                                                                     (tId > 0 && p._id != tId && p.Title == name))
                       ).Count() > 0
                       ? false
                       : true;
        }
    }
}