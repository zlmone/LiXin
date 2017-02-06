using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseManage;
using LiXinModels.DepCourseLearn;
using LiXinModels.DepCourseManage;

namespace LiXinInterface.DepCourseLearn
{
   public interface IDepCourseAdvice
    {
        /// <summary>
        /// 添加课前建议
        /// </summary>
        /// <param name="model"></param>
        void SubmitCl_CourseAdvice(Dep_CourseAdvice model);

       /// <summary>
       /// 获取课前建议信息
       /// </summary>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       List<Dep_CourseAdvice> GetList(string strWhere = "1=1");

       /// <summary>
       /// 我的课前建议列表
       /// </summary>
       /// <param name="UserId"></param>
       /// <param name="where"></param>
       /// <param name="startIndex"></param>
       /// <param name="pageLength"></param>
       /// <returns></returns>
       List<Dep_CourseShow> GetMyproposeList(out int totalCount, int UserId, string where = " 1 = 1 ", string num = "1=1", int startIndex = 0,
                                            int pageLength = int.MaxValue);

       List<Dep_CourseShow> GetMyAfterCourseList(out int totalCount, int UserId, string TrainGrade = "", int DeptId = 0,
                                                string where = " 1 = 1 ", string ALLnum = "", int startIndex = 0,
                                                int pageLength = int.MaxValue);

       /// <summary>
       /// 获取我的部门学时
       /// </summary>
       /// <returns></returns>
       decimal GetMyTotalScore(int userID, int year = -1);

       /// <summary>
        /// 回复课前建议
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       bool ReplyCl_CourseAdvice(Dep_CourseAdvice model);
    }
}
