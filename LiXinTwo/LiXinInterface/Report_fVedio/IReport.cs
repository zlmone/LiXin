using LiXinModels.Report_zVedio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.Report_fVedio
{
    public interface IReport
    {

        /// <summary>
        /// 总所——视频课程汇总统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<CourseVedioLearn> GetVedioLearn(List<int> ListDeptId, string where = " 1=1");

        /// <summary>
        /// 视频课程单门课程统计
        /// </summary>
        /// <returns></returns>
        List<VedioLearnSingle> GetVedioLearnSingle(List<int> ListDeptId, string where = " 1=1");
    }
}
