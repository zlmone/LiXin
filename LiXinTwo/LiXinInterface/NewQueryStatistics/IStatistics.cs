using LiXinModels.NewQueryStatistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface
{
    public interface IStatistics
    {
        /// <summary>
        /// 讲师评价
        /// </summary>
        /// <returns></returns>
        List<TeacherStatistics> GetTeacherStatisticsList(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  UserId desc");

        /// <summary>
        /// 讲师评价详情
        /// </summary>
        /// <returns></returns>
        List<CourseStatistics> GetCourseStatistics(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  Id desc");
    }
}
