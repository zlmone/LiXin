using LiXinModels.Report_zVedio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.Report_Vedio
{
    public interface IReport_Vedio
    {
        /// <summary>
        /// 总所——视频课程汇总统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<CourseVedioLearn> GetVedioLearn(string where = " 1=1");

        /// <summary>
        /// 视频课程单门课程统计
        /// </summary>
        /// <returns></returns>
        List<VedioLearnSingle> GetVedioLearnSingle(string where = " 1=1");

        /// <summary>
        /// 参与情况
        /// </summary>
        /// <param name="ListDeptId"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        List<CourseJoin> GetCourseJoinList(int courseID,string where="1=1",int type=0,string deptIDs="",string querytime=" 1=1" );


    }
}
