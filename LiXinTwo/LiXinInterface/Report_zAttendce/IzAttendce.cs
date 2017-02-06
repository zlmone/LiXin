using LiXinModels.CourseLearn;
using LiXinModels.Report_zAttendce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.Report_zAttendce
{
    public interface IzAttendce
    {
        /// <summary>
        /// 根据条件获得所有的缺勤情况
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<zAttendce> GetAllAttendce(string where = "", string whereDate = "", string whereDateT = "",
                                       string whereDateL = "", string deptName = "", int startIndex = 0, int pageLength = int.MaxValue,
                                       string jsRenderSortField = "", string whereDeptID = "");

        /// <summary>
        /// 查找所有分所
        /// </summary>
        /// <returns></returns>
        List<zAttendce> GetFengDepartment(string whereDeptID = "");

        /// <summary>
        /// 每个人的考勤状态
        /// </summary>
        /// <returns></returns>
        List<Cl_CourseOrder> GetUserAttend(int YearPlan);

    }
}
