using System.Collections.Generic;
using System.Linq;
using LiXinDataAccess.Examination;
using LiXinInterface.Examination;
using LiXinModels.Examination.DBModel;
using MongoDB.Driver.Builders;

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
        /// 获取问题分类字典
        /// </summary>
        /// <param name="deptId">部门ID集合</param>
        /// <returns></returns>
        public Dictionary<int, tbQuestionSort> GetAllQuestionSortDictionary(List<int> deptId=null)
        {
            return sortDB.GetAllQuestionSortDictionary(deptId);
        }

        /// <summary>
        ///     获取所有的试题库
        /// </summary>
        /// <param name="deptId">部门ID集合</param>
        /// <returns></returns>
        public List<tbQuestionSort> GetAllQuestionSortList(List<int> deptId=null)
        {
            return sortDB.GetAllQuestionSortList(deptId);
        }


        /// <summary>
        ///     修改试题库
        /// </summary>
        /// <param name="qSort"></param>
        /// <returns></returns>
        public int ModifyByID(tbQuestionSort qSort)
        {
            sortDB.Modify(qSort);
            return qSort._id;
        }

        /// <summary>
        ///     插入试题库
        /// </summary>
        /// <param name="questionSort"></param>
        /// <returns></returns>
        public int Insert(tbQuestionSort questionSort)
        {
            return sortDB.Insert(questionSort);
        }

        /// <summary>
        ///     删除题库
        /// </summary>
        /// <param name="questionSortID"></param>
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
            return sortDB.GetAllQuestionSortList().Count(p => p.FatherID == pId
                                                              && p.Status == 0
                                                              && (
                                                                     (tId == 0 && p.Title == name) ||
                                                                     (tId > 0 && p._id != tId && p.Title == name))) <= 0;
        }

        /// <summary>
        /// 判断是否已经存在此分类（部门分所开放至全所使用add by yxt）
        /// </summary>
        /// <param name="title">问题分类标题</param>
        /// <param name="deptId">所属部门ID</param>
        ///  <param name="fatherId">父级ID</param>
        /// <returns></returns>
        public bool IsExsitCoQuestionSortName(string title, int deptId,int fatherId)
        {
            return sortDB.IsExsitName<tbQuestionSort>(Query.And(new[]
                                                                  {
                                                                      Query.EQ("Title", title),
                                                                      Query.EQ("DeptId", deptId),
                                                                      Query.EQ("FatherID",fatherId),
                                                                      Query.EQ("Status", 0)
                                                                  }));
        }
    }
}