using LiXinModels.Report_Vedio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.Report_fVedio
{
    public interface IReport_OnLineTest
    {

        List<Report_OnLineTest> GetOnLineTest(int courseid, string where);


        List<Report_OnLineTest> GetOnLineTestDetail(out int totalCount, int courseid, string where = "1=1", int startIndex = 0, int pageLength = int.MaxValue, string jsRenderSortField = " UserId desc");

        List<Report_OnLineTest> GetCanYu(int courseid, string where);

        /// <summary>
        /// 根据月份查询分布图
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        List<Report_OnLineTest> GetMonthByCourseId(int courseid);

        List<Report_OnLineTest> GetMonthDeptByCourseId(int courseid, string deptIDs);
    }
}
