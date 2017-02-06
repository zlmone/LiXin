using LiXinModels;
using LiXinModels.Report_CPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.Report_CPA
{
    public interface IReport_CPA
    {
        /// <summary>
        /// 获得执业CPA继续教育学时统计
        /// </summary>
        /// <returns></returns>
        List<ReportCPA> GetCPAList(int year,int OtherID, int FreeID,Sys_ParamConfig CPAConfig, int deptMaxScore, string where = "1=1", string timeWhere = " 1=1");

        /// <summary>
        /// 根据配置文件和当前时间，获取CPA周期
        /// 如（2012年10月—2013年12月）
        /// </summary>
        /// <param name="yearConfig"></param>
        List<DateTime> CPAStartAndEnd(Sys_ParamConfig CPAConfig,int year=-1);

        /// <summary>
        /// 执业CPA继续教育学时统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<ReportCPADetails> GetCPADetailsList(int year, int deptMaxScore, string where = " 1=1", string yearwhere=" 1=1");

        /// <summary>
        /// 学员学时获取
        /// </summary>
        /// <param name="CPAConfig">CPA周期时间</param>
        /// <param name="deptMaxScore">部门最大获取学时</param>
        /// <param name="CPAYearSCore">CPA年度考核目标学时</param>
        /// <param name="CPAzhouqiScore">CPA培训周期考核目标学时</param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<CPACompelete> GetUserCpaScore(out bool IsLast, Sys_ParamConfig CPAConfig, int deptMaxScore, int CPAYearSCore, int CPAzhouqiScore, int year,List<Sys_ParamConfig> freeconfig, string where = "1=1", string timeWhere = "1=1");

        /// <summary>
        /// 周期学时
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        List<dynamic> GetzhouqiAllScore(Sys_ParamConfig CPAConfig,int year, int score = -1);

        List<string> CPAStartAndEndStr(Sys_ParamConfig CPAConfig);

        /// <summary>
        /// CPA关系所在地
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<string> GetCPARelationship(string where = "1=1");



        /// <summary>
        /// 周期内完成道德培训的人员ID
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        List<int> GetzhouqiIsDaode(Sys_ParamConfig CPAConfig, int year);
    }
}
