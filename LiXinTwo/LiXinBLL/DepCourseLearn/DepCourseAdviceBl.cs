using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface.DepCourseLearn;
using LiXinDataAccess.DepCourseLearn;
using LiXinModels.CourseManage;
using LiXinModels.DepCourseLearn;
using LiXinModels.DepCourseManage;

namespace LiXinBLL.DepCourseLearn
{
    public class DepCourseAdviceBl : IDepCourseAdvice
    {
        private DepCourseAdviceDB DepAdvice = new DepCourseAdviceDB();

        public void SubmitCl_CourseAdvice(Dep_CourseAdvice model)
        {
            DepAdvice.SubmitCl_CourseAdvice(model);
        }
        /// <summary>
        /// 获取课前建议信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<Dep_CourseAdvice> GetList(string strWhere = "1=1")
        {
            return DepAdvice.GetList(strWhere);
        }

        public List<Dep_CourseShow> GetMyproposeList(out int totalCount, int UserId, string where = " 1 = 1 ", string num = " 1=1", int startIndex = 0,
                                                    int pageLength = int.MaxValue)
        {
            return DepAdvice.GetMyproposeList(out totalCount, UserId, where, num, startIndex, pageLength);
        }

        public List<Dep_CourseShow> GetMyAfterCourseList(out int totalCount, int UserId, string TrainGrade = "", int DeptId = 0, string where = " 1 = 1 ", string ALLnum = "", int startIndex = 0,
                                                   int pageLength = int.MaxValue)
        {
            //return attendceDb.GetMyAfterCourseList(out totalCount,UserId,TrainGrade,DeptId, where,ALLnum, startIndex, pageLength);
            return DepAdvice.GetMyAfterCourseList(out totalCount, UserId, TrainGrade, DeptId, where, ALLnum, startIndex,
                                                   pageLength);
        }

        /// <summary>
        /// 获取我的部门学时
        /// </summary>
        /// <returns></returns>
        public decimal GetMyTotalScore(int userID, int year = -1)
        {
            return DepAdvice.GetMyTotalScore(userID, year);
        }

        /// <summary>
        /// 回复课前建议
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ReplyCl_CourseAdvice(Dep_CourseAdvice model)
        {
            return DepAdvice.ReplyCl_CourseAdvice(model);
        }
    }
}
