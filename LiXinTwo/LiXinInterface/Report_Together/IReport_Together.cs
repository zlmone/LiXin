using LiXinModels.Report_Together;
using LiXinModels.Report_zVedio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.Report_Together
{
    public interface IReport_Together
    {
        /// <summary>
        /// 根据ID获取课程信息
        /// </summary>
        /// <returns></returns>
        Report_CourseShow GetTogetherCourseById(int courseId);

        /// <summary>
        /// 获得员工单门课程报名、参与情况列表
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <param name="where">条件语句‘and ....’</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序：默认按Id倒序排序</param>
        /// <returns></returns>
        List<Report_UserLearnTogetherCourseShow> GetSingleTogetherCourseUsersList(out int totalCount, out decimal allGetScore, out decimal aLLGetExamScore, string realName = "",
                                                                                  int isDoEaxm = -1, int isCPA = -1,
                                                                                  int LeaveType = -1,
                                                                                  int isTeacher = -1, int orderType = -1,
                                                                                  int isSurveyReplyAnswer = -1,
                                                                                  int isCourseAdvice = -1,
                                                                                  int attStatus = -1,
                                                                                  int isResourceWrite = -1, string tranGrade = "", string deptList = ""
                                                                                  , int courseId = 0, string StartTime = "",
                                                                                  string EndTime = "", int startIndex = 1,
                                                                                  int pageLength = int.MaxValue
                                                                                  ,
                                                                                  string orderBy =
                                                                                      " ORDER BY DeptPath,GetScore ");
    }
}
