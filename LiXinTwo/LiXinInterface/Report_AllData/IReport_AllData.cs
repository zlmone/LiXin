using LiXinModels;
using System.Collections.Generic;

namespace LiXinInterface.Report_AllData
{
    public interface IReport_AllData
    {
              /// <summary>
        /// 总表报数据处理
        /// </summary>
        /// <param name="sysConfig14">学时上限配置</param>
        /// <param name="sysConfig13">培训级别目标学时配置</param>
        /// <param name="sysConfig16">CPA目标学时配置</param>
        /// <param name="sysConfig27">在线测试通过次数</param>
        /// <param name="year">年度</param>
        /// <param name="naru">1 纳入 0 不纳入</param>
        /// <returns></returns>
        List<Report_Dept_User> GetAllCourseReport(Sys_ParamConfig sysConfig14, Sys_ParamConfig sysConfig13, Sys_ParamConfig sysConfig16, Sys_ParamConfig sysConfig27, int year, int naru = 1);
    }
}
