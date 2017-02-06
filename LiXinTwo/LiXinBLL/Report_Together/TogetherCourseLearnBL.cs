using LiXinDataAccess;
using LiXinDataAccess.CourseManage;
using LiXinInterface.Report_Together;
using LiXinInterface.Report_Vedio;
using LiXinModels.CourseManage;
using LiXinModels.Examination.DBModel;
using LiXinModels.Report_Together;
using LiXinModels.Report_zVedio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;
using System.Diagnostics;
using LiXinDataAccess.Examination;
using LiXinBLL.SystemManage;

namespace LiXinBLL.Report_Together
{
    public class TogetherCourseLearnBL : IReport_Together
    {
        private TogetherCourseLearnDB togetherCourseLearnDB;
        private ExaminationDB examDB;
        private Co_CourseDB courseDb;
        private Co_CoursePaperDB coursepaperDb;
        private readonly ExampaperDB _paperDB;
        private Report_FreeDB freeDB; 
        public TogetherCourseLearnBL()
        {
            togetherCourseLearnDB = new TogetherCourseLearnDB();
            examDB = new ExaminationDB();
            courseDb = new Co_CourseDB();
            coursepaperDb = new Co_CoursePaperDB();
            _paperDB = new ExampaperDB();
            freeDB = new Report_FreeDB();
        }

        #region
        /// <summary>
        /// 根据ID获取课程信息
        /// </summary>
        /// <returns></returns>
        public Report_CourseShow GetTogetherCourseById(int courseId)
        {
            return togetherCourseLearnDB.GetTogetherCourseById(courseId);
        }

        /// <summary>
        /// 获得员工单门课程报名、参与情况列表
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <param name="where">条件语句‘and ....’</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按Id倒序排序</param>
        /// <returns></returns>
        public List<Report_UserLearnTogetherCourseShow> GetSingleTogetherCourseUsersList(out int totalCount, out decimal allGetScore, out decimal aLLGetExamScore, string realName = "", int isDoEaxm = -1, int isCPA = -1, int LeaveType = -1,
                        int isTeacher = -1, int orderType = -1, int isSurveyReplyAnswer = -1, int isCourseAdvice = -1, int attStatus = -1, int isResourceWrite = -1
                        , string tranGrade = "", string deptList = "", int courseId = 0, string StartTime = "", string EndTime = "", int startIndex = 1, int pageLength = int.MaxValue
                        , string orderBy = " ORDER BY DeptPath,GetScore ")
        {
            string where = "";
            Co_Course courseModel = courseDb.GetCo_Course(courseId);

            #region 条件

            if (!string.IsNullOrWhiteSpace(realName))
            {
                where += string.Format(" and Realname like '%{0}%' ", realName.ReplaceSql());
            }
            if (isCPA != -1 && isCPA != -2)
            {
                where += string.Format(" and IsCPA={0} ", isCPA);
            }
            if (LeaveType != -1)
            {
                where += string.Format(" and LeaveType={0} ", LeaveType);
            }
            if (isTeacher != -1 && isTeacher != -2)
            {
                where += string.Format(" and IsTeacher={0} ", isTeacher);
            }
            if (orderType != -1)
            {
                where += string.Format(" and OrderType={0} ", orderType);
            }
            if (isSurveyReplyAnswer != -1 && isSurveyReplyAnswer != -2)
            {
                where += string.Format(" and IsSurveyReplyAnswer={0} ", isSurveyReplyAnswer);
            }
            if (isCourseAdvice != -1 && isCourseAdvice != -2)
            {
                where += string.Format(" and IsCourseAdvice={0} ", isCourseAdvice);
            }
            if (!string.IsNullOrWhiteSpace(tranGrade))
            { where += string.Format(" and ('{0}' like '%'+TrainGrade+'%' and (TrainGrade is not null and TrainGrade <> ''))", tranGrade.ReplaceSql()); }
            if (!string.IsNullOrWhiteSpace(deptList))
            {
                where += string.Format(" and DeptId in (select [Value] from SplitStringBySeparator('{0}', ',', 1)) ", deptList);
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                where += " and OrderTime>='" + StartTime + "'";
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                where += " and OrderTime<='" + EndTime + "'";
            }

            #endregion

            #region 其他形式、免修

            //自动折算
            var config = new Sys_ParamConfigBL().GetList(" ConfigType = 63").Where(p => p.LastUpdateTime.Year == courseModel.YearPlan).FirstOrDefault();
            var FreeList = freeDB.GetFreeUserList("  Year=" + courseModel.YearPlan);
            if (!(config == null || config.ConfigValue == ""))
            {
                var configvalue = config.ConfigValue.Split(';');
                var tDate = courseModel.YearPlan + "-" + configvalue[0].Split(',')[0];
                var CPADate = courseModel.YearPlan + "-" + configvalue[1].Split(',')[0];
                var userList = freeDB.GetUserByDate(tDate, CPADate);
                FreeList.AddRange(userList.Select(p => p.ApplyUserID).Distinct());
            }
            #endregion

            List<Report_UserLearnTogetherCourseShow> list = togetherCourseLearnDB.GetSingleTogetherCourseUsersList(courseId, courseModel.IsRT, where, 1, int.MaxValue,
                "ORDER BY DeptPath,GetScore");
            //考试相关
            if (courseModel.IsTest == 1)
            {
                Co_CoursePaper coCoursePaper = coursepaperDb.GetCo_CourseMainPaper(courseId);
                List<tbExamSendStudent> examTogetherList = examDB.GetExamSendStudentListBySQL2008(courseId, coCoursePaper.id,
                                                           coCoursePaper.PaperId, 1);//集中课程考试
                List<tbExamSendStudent> examShipinList = examDB.GetExamSendStudentListBySQL2008(courseId, coCoursePaper.id,
                                                           coCoursePaper.PaperId, 4);//视频转播课程考试
                //var exampaper = _paperDB.GetExampaper(coCoursePaper.PaperId);
                //int LevelScoreStr =
                //    Convert.ToInt32(coCoursePaper.LevelScore*0.01*(exampaper == null ? 0 : exampaper.ExampaperScore));
                foreach (var userLearnTogetherCourseShow in list)
                {
                    tbExamSendStudent tbExamSendStudentModel = null;
                    if (userLearnTogetherCourseShow.OrderType == 3)
                    {
                        tbExamSendStudentModel =
                            examShipinList.FirstOrDefault(p => p.UserID == userLearnTogetherCourseShow.UserId);
                    }
                    else
                    {
                        tbExamSendStudentModel =
                            examTogetherList.FirstOrDefault(p => p.UserID == userLearnTogetherCourseShow.UserId);
                    }
                    if (tbExamSendStudentModel != null)
                    {

                        userLearnTogetherCourseShow.IsDoExam = tbExamSendStudentModel.DoExamStatus == 0 ? 0 : 1;
                        userLearnTogetherCourseShow.GetExamScore =
                            tbExamSendStudentModel.StudentAnswerList.Sum(pa => pa.GetScore);
                    }
                    else
                    {
                        userLearnTogetherCourseShow.IsDoExam = 0;
                        userLearnTogetherCourseShow.GetExamScore = 0;
                    }
                }
            }
            var listNew = list.Where(p => true);
            if (isDoEaxm != -1 && isDoEaxm != -2)
            {
                listNew = listNew.Where(p => p.IsDoExam == isDoEaxm);
            }
            if (attStatus != -1)
            {
                listNew = listNew.Where(p => p.AttStatusStr != "N/A" && p.AttStatus == attStatus);
            }
            if (orderBy.ToLower().Contains("GetExamScore".ToLower()))//获得分数排序
            {
                if (orderBy.ToLower().Contains("desc".ToLower()))
                {
                    listNew = listNew.OrderByDescending(p => p.GetExamScore);
                }
                else
                {
                    listNew = listNew.OrderBy(p => p.GetExamScore);
                }
            }
            allGetScore = listNew.Where(p => p.GetScoredouble >= 0).Sum(p => p.GetScoredouble);
            var listTemp = listNew.Where(p => p.IsDoExam == 1);
            aLLGetExamScore = listTemp.Count() > 0 ? Convert.ToDecimal(ReportCommon.CalculateMedianDouble(listTemp.Select(p => p.GetExamScore).ToList())) : 0;

            totalCount = listNew.Count();


            foreach (var item in listNew)
            {
                item.IsFree = FreeList.Count(p => p == item.UserId) == 0 ? 0 : 1;
            }
            return listNew.ToList();

        }
        #endregion






    }
}
