using LiXinModels;
using LiXinModels.Report_Together;
using LiXinModels.Report_Vedio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.Report_Together
{
    public interface IReportTogether
    {

        /// <summary>
        /// 所有课程的参与、贡献度情况
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<ReportTogether> GetTogetherList(int year, Sys_ParamConfig config, string deptIDs = "");

        /// <summary>
        /// 每个部门分所应参加的人数
        /// </summary>
        /// <returns></returns>
        List<fVedioJoinNumber> GetTogetherUser(int year = -1);

        /// <summary>
        /// 所有课程的参与、贡献度情况
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<ReportTogether> GetCourseInfoList(int year, Sys_ParamConfig config, List<string> allTrainGrade, string courseName = "", string teacherName = "", int IsMUst = -1, int IsCPA = -1
            , int IsTest = -1, string payGrade = "", string openLevel = "", int IsDelete = 0, int LevelType = 0);

          /// <summary>
        /// 员工单门课程评估、测试情况表
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="SurveyPaperId"></param>
        /// <param name="isPing"></param>
        /// <param name="isTest"></param>
        /// <returns></returns>
        List<SingleTogtherSurvey> GetSingleTogtherSurvey(int courseID, string SurveyPaperId, int isPing, int isTest, string courseStarttime,
            string deptids = "", string sDate = "", string eDate = "");
    }
}
