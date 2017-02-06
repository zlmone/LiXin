using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.NewQueryStatistics;
using LiXinDataAccess.NewQueryStatistics;
using LiXinInterface;

namespace LiXinBLL
{
    public class StatisticsBL : IStatistics
    {
        public static TeacherDB teacherDB;
        public StatisticsBL()
        {
            teacherDB = new TeacherDB();
        }

        /// <summary>
        /// 讲师评价
        /// </summary>
        /// <returns></returns>
        public List<TeacherStatistics> GetTeacherStatisticsList(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  UserId desc")
        {

            var list = teacherDB.GetTeacherStatisticsList(startIndex, startLength, where, jsRenderSortField);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

         /// <summary>
        /// 讲师评价详情
        /// </summary>
        /// <returns></returns>
        public List<CourseStatistics> GetCourseStatistics(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  Id desc")
        {
            var list = teacherDB.GetCourseStatistics(startIndex, startLength, where, jsRenderSortField);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }
    }
}
