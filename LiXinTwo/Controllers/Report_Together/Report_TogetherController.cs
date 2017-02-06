using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinInterface.Report_Together;
using LiXinInterface.Report_Vedio;
using LiXinModels.Report_Together;
using LiXinModels.Report_zVedio;
using LiXinCommon;
using System.Data;
using LiXinInterface.Report_fVedio;
using LiXinModels;
using System.Text.RegularExpressions;
using LiXinModels.DepCourseManage;
using LiXinBLL;
using LiXinInterface;

namespace LiXinControllers
{
    public partial class Report_TogetherController : BaseController
    {
        protected IReport_Together TogetherCourseLearnBL;
        protected IReportTogether TogetherBL;
        private IReport_DepCourse iReport_DepCourse;
        private IReport_Free FreeBL;
        private static List<Report_DepCourse> SingleCoursePationList = new List<Report_DepCourse>();
        private static List<Report_UserLearnTogetherCourseShow> UserLearnTogetherList = new List<Report_UserLearnTogetherCourseShow>();

        private static decimal allGetScore = 0;
        private static decimal aLLGetExamScore = 0;
        private static bool IsIn;
        public Report_TogetherController(IReport_Together togetherCourseLearnBL, IReportTogether _TogetherBL, IReport_DepCourse _iReport_DepCourse, IReport_Free _FreeBL)
        {
            TogetherCourseLearnBL = togetherCourseLearnBL;
            TogetherBL = _TogetherBL;
            iReport_DepCourse = _iReport_DepCourse;
            FreeBL = _FreeBL;
        }

        #region  呈现

        /// <summary>
        /// 集中授课课程基本信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public ActionResult TogetherCourseBaseInfo(int courseId = 0)
        {
            var model = TogetherCourseLearnBL.GetTogetherCourseById(courseId);
            if (model.IsOrder == 2)
            {
                var list = AllTrainGrade.Where(p => string.IsNullOrEmpty(model.OpenLevel) ? true : !model.OpenLevel.Contains(p)).ToList();
                var OpenLevel = list.Count() == 0 ? "" : string.Join(",", list);
                model.OpenLevel = OpenLevel;
            }
            return View(model);
        }

        #endregion

        #region 员工单门课程报名、参与情况
        /// <summary>
        /// 获得员工单门课程报名、参与情况页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SingleTogetherCourseUsersDetail(int courseId = 0, string deptIds = "", int index = -1)
        {
            ViewBag.allTrainGrade = AllTrainGrade;
            ViewBag.courseId = courseId;
            ViewBag.deptIds = deptIds;
            ViewBag.index = index;
            return View();
        }
      
        /// <summary>
        /// 获得员工单门课程报名、参与情况列表
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns></returns>
        public JsonResult GetSingleTogetherCourseUsersList(string realName = "", int isDoEaxm = -1, int isCPA = -1, int leaveType = -1,
                        int isTeacher = -1, int orderType = -1, int isSurveyReplyAnswer = -1, int isCourseAdvice = -1, int attStatus = -1, int isResourceWrite = -1, string tranGrade = "", string deptList = ""
                        , int courseId = 0, string StartTime = "", string EndTime = "",int isFree=-1, int pageSize = int.MaxValue, int pageIndex = 1, string jsRenderSortField = " DeptPath,GetScore "
                        )
        {
            try
            {
                int totalCount = 0;
                allGetScore = 0;
                aLLGetExamScore = 0;
                UserLearnTogetherList = TogetherCourseLearnBL.GetSingleTogetherCourseUsersList(out totalCount, out allGetScore, out aLLGetExamScore, realName, isDoEaxm, isCPA, leaveType,
                         isTeacher, orderType, isSurveyReplyAnswer, isCourseAdvice, attStatus, isResourceWrite, tranGrade, deptList
                        , courseId, StartTime, EndTime, pageIndex, pageSize, " order by " + jsRenderSortField);
                
                if (isFree != -1)
                {
                    UserLearnTogetherList = UserLearnTogetherList.Where(p=>p.IsFree==isFree).ToList();
                }
                if (!jsRenderSortField.Contains(","))
                {
                    UserLearnTogetherList = UserLearnTogetherList.SortListByField(jsRenderSortField);
                }
                else
                {
                    UserLearnTogetherList = UserLearnTogetherList.OrderBy(p => p.DeptId).ThenBy(p => p.GetScoredouble).ToList();
                }

                var scoreList = UserLearnTogetherList.Where(p => p.GetScoredouble >= 0).Select(p => p.GetScoredouble);
                var list = UserLearnTogetherList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                allGetScore = totalCount == 0 ? 0 : Convert.ToDecimal(ReportCommon.CalculateMedianDouble(scoreList.ToList()));
                return Json(new
                {
                    dataList = list,
                    recordCount = totalCount,
                    allGetScore = allGetScore.ToString("0.00"),
                    aLLGetExamScore = aLLGetExamScore.ToString("0.00")
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<Report_UserLearnTogetherCourseShow>(),
                    recordCount = 0,
                    allGetScore = 0,
                    aLLGetExamScore = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导出员工单门课程报名、参与情况列表
        /// </summary>
        /// <param name="courseId">课程Id</param>
        /// <returns></returns>
        public void ExportSingleTogetherCourseUsersList(string realName = "", int isDoEaxm = -1, int isCPA = -1, int leaveType = -1,
                        int isTeacher = -1, int orderType = -1, int isSurveyReplyAnswer = -1, int isCourseAdvice = -1, int attStatus = -1, int isResourceWrite = -1, string tranGrade = "", string deptList = ""
                        , int courseId = 0, int pageSize = int.MaxValue, int pageIndex = 1, string jsRenderSortField = " DeptPath,GetScore "
                        )
        {


            var list = UserLearnTogetherList;
            var dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("部门/分所", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("登录名", typeof(string));
            dt.Columns.Add("培训级别", typeof(string));
            dt.Columns.Add("是否CPA", typeof(string));
            dt.Columns.Add("报名类型", typeof(string));
            dt.Columns.Add("请假类型", typeof(string));
            dt.Columns.Add("考勤类型", typeof(string));
            dt.Columns.Add("获得学时", typeof(string));
            dt.Columns.Add("是否提交课前建议", typeof(string));
            dt.Columns.Add("是否参与课后评估", typeof(string));
            dt.Columns.Add("是否参与测试", typeof(string));
            dt.Columns.Add("测试分数", typeof(string));

            dt.Rows.Add("合计", "--", "--", "--", "--", "--", "--", "--", "--", allGetScore + "", "--", "--", "--",
                        aLLGetExamScore + "");
            var count = 1;
            foreach (Report_UserLearnTogetherCourseShow item in list)
            {
                dt.Rows.Add(count++, item.DeptPath, item.Realname, item.username, item.TrainGrade, item.IsCPAStr, item.OrderTypeStr, item.LeaveTypeStr, item.AttStatusStr,
                            item.GetScoreStr, item.IsCourseAdviceStr, item.IsSurveyReplyAnswerStr, item.IsDoExamStr, item.GetExamScoreStr);
            }
            var dtList = new List<DataTable>();
            dtList.Add(dt);
            var export = new Spreadsheet();
            new Spreadsheet().ExportChart(new List<ChartModel>(), dtList, HttpContext, "员工单门课程报名、参与情况明细表");
        }
        #endregion

        #region 单门课程参与情况
        public ActionResult SingleCoursePation(int courseid, int cp = 0)
        {
            //  Session["together_CoursePation"] = deptid + "㉿" + StartTime + "㉿" + EndTime;
            ViewBag.deptid = "";
            ViewBag.StartTime = "";
            ViewBag.EndTime = "";
            ViewBag.cp = cp;
            if (cp == 1)
            {
                if (Session["together_CoursePation"] != null)
                {
                    string sess = Session["together_CoursePation"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.deptid = att[0];
                    ViewBag.StartTime = att[1];
                    ViewBag.EndTime = att[2];
                }
            }
            return View();
        }


        /// <summary>
        /// 单门课程参与情况
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="deptid"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jsRenderSortField"></param>
        /// <param name="type">0 显示的时候使用  1导出的时候使用</param>
        /// <returns></returns>
        public JsonResult GetSingleCoursePation(int courseid, string deptid, string StartTime, string EndTime, string jsRenderSortField, int type, string courseStarttime)
        {
            if (Session["together_CoursePation"] != null)
            {
                Session.Remove("together_CoursePation");
            }
            Session["together_CoursePation"] = deptid + "㉿" + StartTime + "㉿" + EndTime;

            string sql = " 1=1";
            var dwhere = " 1=1";

            if (!string.IsNullOrEmpty(deptid))
            {
                sql += " and sys_user.deptid in(" + deptid + ")";
                dwhere += " and syu.deptid in(" + deptid + ")";
            }

            //是否在课程结束时间之前,为 true的时候 考勤人数等都为N/A
            IsIn = false;
            if (!string.IsNullOrEmpty(EndTime))
            {
                if (Convert.ToDateTime(courseStarttime) > Convert.ToDateTime(EndTime))
                {
                    IsIn = true;
                }
            }

            SingleCoursePationList = iReport_DepCourse.GetSingleCoursePation(courseid, sql, dwhere, StartTime, EndTime);

            SingleCoursePationList = SingleCoursePationList.SortListByField(jsRenderSortField);

            var sb = GetSingleCoursePationHtml(SingleCoursePationList, showhide: type, IsIn: IsIn);

            return Json(new
            {
                dataList = sb.ToString()

            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 返回拼接的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="type">0 显示的时候使用  1导出的时候使用</param>
        /// <param name="showhide">0第一次查询 默认收缩 1排序查询等第二次操作</param>
        /// <param name="IsIn">是否在课程结束时间之前,为 true的时候 考勤人数等都为N/A,false 正常显示</param>
        /// <returns></returns>
        public StringBuilder GetSingleCoursePationHtml(List<Report_DepCourse> list, int type = 0, int showhide = 0, bool IsIn = false)
        {

            StringBuilder sb = new StringBuilder();

            var zongsuo = list.Where(p => p.IsMain == 0).ToList();


            var fensuo = list.Where(p => p.IsMain == 1).ToList();

            if (list.Count() != 0)
            {
                sb.Append("<tr>");
                sb.Append("<td></td>");
                sb.Append("<td style='text-align:left'>全所合计（" + list.Count() + "个）</td>");
                if (zongsuo.Count() != 0 && fensuo.Count() != 0)
                {
                    sb.Append("<td>" + ((ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.Avg_GetLength).ToList(), 2) + ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.Avg_GetLength).ToList(), 2)) / 2).ToString("0.00") + "</td>");
                }
                if (zongsuo.Count() != 0 && fensuo.Count() == 0)
                {
                    sb.Append("<td>" + ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.Avg_GetLength).ToList(), 2).ToString("0.00") + "</td>");
                }
                if (zongsuo.Count() == 0 && fensuo.Count() != 0)
                {
                    sb.Append("<td>" + ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.Avg_GetLength).ToList(), 2).ToString("0.00") + "</td>");
                }
                sb.Append("<td>" + list.Sum(p => p.Pation_OrderNum) + "</td>");
                sb.Append("<td>" + list.Sum(p => p.Enter_OrderNum) + "</td>");
                sb.Append("<td>" + list.Where(p => p.IsLeave >= 0).Sum(p => p.IsLeave) + "</td>");
                sb.Append("<td>" + list.Sum(p => p.Un_OrderNum) + "</td>");

                if (zongsuo.Count() != 0 && fensuo.Count() != 0)
                {
                    sb.Append("<td>" + ((ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.ZWS_Enter_Pation_OrderNum).ToList(), 2) + ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.ZWS_Enter_Pation_OrderNum).ToList(), 2)) / 2).ToString("0.00") + "%</td>");
                }
                if (zongsuo.Count() != 0 && fensuo.Count() == 0)
                {
                    sb.Append("<td>" + ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.ZWS_Enter_Pation_OrderNum).ToList(), 2).ToString("0.00") + "%</td>");
                }
                if (zongsuo.Count() == 0 && fensuo.Count() != 0)
                {
                    sb.Append("<td>" + ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.ZWS_Enter_Pation_OrderNum).ToList(), 2).ToString("0.00") + "%</td>");
                }



                if (IsIn)
                {
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                }
                else
                {
                    sb.Append("<td>" + list.Sum(p => p.Actual_OrderNum) + "</td>");
                    if (zongsuo.Count() != 0 && fensuo.Count() != 0)
                    {
                        sb.Append("<td>" + ((ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.ZWS_Enter_Actual_OrderNum).ToList(), 2) + ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.ZWS_Enter_Actual_OrderNum).ToList(), 2)) / 2).ToString("0.00") + "%</td>");
                    }
                    if (zongsuo.Count() != 0 && fensuo.Count() == 0)
                    {
                        sb.Append("<td>" + ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.ZWS_Enter_Actual_OrderNum).ToList(), 2).ToString("0.00") + "%</td>");
                    }
                    if (zongsuo.Count() == 0 && fensuo.Count() != 0)
                    {
                        sb.Append("<td>" + ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.ZWS_Enter_Actual_OrderNum).ToList(), 2).ToString("0.00") + "%</td>");
                    }
                    sb.Append("<td>" + list.Where(p => p.Bu_OrderNum >= 0).Sum(p => p.Bu_OrderNum) + "</td>");
                    sb.Append("<td>" + list.Sum(p => p.Cd_Num) + "</td>");
                    sb.Append("<td>" + list.Sum(p => p.Qq_Num) + "</td>");
                    sb.Append("<td>" + list.Sum(p => p.Zt_Num) + "</td>");
                    sb.Append("<td>" + list.Sum(p => p.Cd_Zt_Num) + "</td>");
                }

                sb.Append("</tr>");
            }
            else
            {
                sb.Append("<tr><td></td><td>全所合计0个</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0%</td><td>0</td><td>0</td><td>0%</td><td>0</td><td>0</td><td>0</td><td>0</td><td>0</td></tr>");
                sb.Append("<tr class='odd'><td colspan='15'><div class='tc c38 line_h30'>暂无数据</div></td></tr>");
            }

            #region 总所

            if (zongsuo.Count() != 0)
            {
                sb.Append("<tr>");
                sb.Append("<td></td>");
                sb.Append("<td style='text-align:left'>总所合计（" + zongsuo.Count() + "个）</td>");
                sb.Append("<td>" + ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.Avg_GetLength).ToList(), 2).ToString("0.00") + "</td>");
                sb.Append("<td>" + zongsuo.Sum(p => p.Pation_OrderNum) + "</td>");
                sb.Append("<td>" + zongsuo.Sum(p => p.Enter_OrderNum) + "</td>");
                sb.Append("<td>" + zongsuo.Where(p => p.IsLeave >= 0).Sum(p => p.IsLeave) + "</td>");
                sb.Append("<td>" + zongsuo.Sum(p => p.Un_OrderNum) + "</td>");
                sb.Append("<td>" + ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.ZWS_Enter_Pation_OrderNum).ToList(), 2).ToString("0.00") + "%</td>");
                if (IsIn)
                {
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                }
                else
                {
                    sb.Append("<td>" + zongsuo.Sum(p => p.Actual_OrderNum) + "</td>");
                    sb.Append("<td>" + ReportCommon.CalculateMedianDouble(zongsuo.Select(p => p.ZWS_Enter_Actual_OrderNum).ToList(), 2).ToString("0.00") + "%</td>");
                    sb.Append("<td>" + zongsuo.Where(p => p.Bu_OrderNum >= 0).Sum(p => p.Bu_OrderNum) + "</td>");
                    sb.Append("<td>" + zongsuo.Sum(p => p.Cd_Num) + "</td>");
                    sb.Append("<td>" + zongsuo.Sum(p => p.Qq_Num) + "</td>");
                    sb.Append("<td>" + zongsuo.Sum(p => p.Zt_Num) + "</td>");
                    sb.Append("<td>" + zongsuo.Sum(p => p.Cd_Zt_Num) + "</td>");
                }
                if (type == 0)
                {
                    if (showhide == 0)
                    {
                        sb.Append("<td><a class='buttonhide btnshow'  onclick='showHide(1,this)'><a></td>");
                    }
                    else
                    {
                        sb.Append("<td><a class='buttonshow btnshow'  onclick='showHide(1,this)'><a></td>");
                    }
                }
                sb.Append("</tr>");
            }

            int zongsuonum = 1;
            foreach (var item in zongsuo)
            {
                if (type == 0)
                {
                    if (showhide == 0)
                    {
                        sb.Append("<tr  class='report_1' style='display:none'>");
                    }
                    else
                    {
                        sb.Append("<tr  class='report_1'>");
                    }
                }
                else
                {
                    sb.Append("<tr>");
                }
                sb.Append("<td>" + zongsuonum + "</td>");
                sb.Append("<td style='text-align:left'><a onclick=\"viewDetails('" + item.DeptIds + "',0)\" style=\"cursor:pointer\">" + item.DeptName + "</a></td>");
                sb.Append("<td>" + item.Avg_GetLength.ToString("0.00") + "</td>");
                sb.Append("<td>" + item.Pation_OrderNum + "</td>");
                sb.Append("<td>" + item.Enter_OrderNum + "</td>");
                sb.Append("<td>" + item.IsLeaveStr + "</td>");
                sb.Append("<td>" + item.Un_OrderNum + "</td>");
                if (item.Pation_OrderNum != 0)
                {
                    sb.Append("<td>" + item.ZWS_Enter_Pation_OrderNum.ToString("0.00") + "%</td>");
                }
                else
                {
                    sb.Append("<td>" + 0 + "%</td>");
                }
                if (IsIn)
                {
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                }
                else
                {
                    sb.Append("<td>" + item.Actual_OrderNum + "</td>");
                    sb.Append("<td>" + (item.Enter_OrderNum == 0 ? 0 : Math.Round(Convert.ToDouble(Convert.ToDouble(item.Actual_OrderNum) / Convert.ToDouble(item.Enter_OrderNum)), 4) * 100).ToString("0.00") + "%</td>");
                    sb.Append("<td>" + item.Bu_OrderNumStr + "</td>");
                    sb.Append("<td>" + item.Cd_Num + "</td>");
                    sb.Append("<td>" + item.Qq_Num + "</td>");
                    sb.Append("<td>" + item.Zt_Num + "</td>");
                    sb.Append("<td>" + item.Cd_Zt_Num + "</td>");
                }
                sb.Append("</tr>");
                zongsuonum++;
            }

            #endregion

            #region 分所
            // var listDeptName = new List<string>();
            if (fensuo.Count() != 0)
            {
                sb.Append("<tr>");
                sb.Append("<td></td>");
                sb.Append("<td style='text-align:left'>分所合计（" + fensuo.Count() + "个）</td>");
                sb.Append("<td>" + ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.Avg_GetLength).ToList(), 2).ToString("0.00") + "</td>");
                sb.Append("<td>" + fensuo.Sum(p => p.Pation_OrderNum) + "</td>");
                sb.Append("<td>" + fensuo.Sum(p => p.Enter_OrderNum) + "</td>");
                sb.Append("<td>" + fensuo.Where(p => p.IsLeave >= 0).Sum(p => p.IsLeave) + "</td>");
                sb.Append("<td>" + fensuo.Sum(p => p.Un_OrderNum) + "</td>");
                sb.Append("<td>" + ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.ZWS_Enter_Pation_OrderNum).ToList(), 2).ToString("0.00") + "%</td>");
                if (IsIn)
                {
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                }
                else
                {
                    sb.Append("<td>" + fensuo.Sum(p => p.Actual_OrderNum) + "</td>");
                    sb.Append("<td>" + ReportCommon.CalculateMedianDouble(fensuo.Select(p => p.ZWS_Enter_Actual_OrderNum).ToList(), 2).ToString("0.00") + "%</td>");
                    sb.Append("<td>" + fensuo.Where(p => p.Bu_OrderNum >= 0).Sum(p => p.Bu_OrderNum) + "</td>");
                    sb.Append("<td>" + fensuo.Sum(p => p.Cd_Num) + "</td>");
                    sb.Append("<td>" + fensuo.Sum(p => p.Qq_Num) + "</td>");
                    sb.Append("<td>" + fensuo.Sum(p => p.Zt_Num) + "</td>");
                    sb.Append("<td>" + fensuo.Sum(p => p.Cd_Zt_Num) + "</td>");
                }
                if (type == 0)
                {
                    if (showhide == 0)
                    {
                        sb.Append("<td><a class='buttonhide btnshow'  onclick='showHide(2,this)'><a></td>");
                    }
                    else
                    {
                        sb.Append("<td><a class='buttonshow btnshow'  onclick='showHide(2,this)'><a></td>");
                    }
                }
                sb.Append("</tr>");
            }

            int fensuonum = 1;
            //第二级
            foreach (var item in fensuo)
            {
                //if (!listDeptName.Contains(item.DeptName))
                //{
                //    listDeptName.Add(item.DeptName);

                //}
                if (type == 0)
                {
                    if (showhide == 0)
                    {
                        sb.Append("<tr  class='report_0' style='display:none'>");
                    }
                    else
                    {
                        sb.Append("<tr  class='report_0'>");
                    }
                }
                else
                {
                    sb.Append("<tr>");
                }
                sb.Append("<td>" + fensuonum + "</td>");
                sb.Append("<td style='text-align:left'><a onclick=\"viewDetails('" + item.DeptIds + "',0)\" style=\"cursor:pointer\">" + item.DeptName + "</a></td>");
                sb.Append("<td>" + item.Avg_GetLength.ToString("0.00") + "</td>");
                sb.Append("<td>" + item.Pation_OrderNum + "</td>");
                sb.Append("<td>" + item.Enter_OrderNum + "</td>");
                sb.Append("<td>" + item.IsLeaveStr + "</td>");
                sb.Append("<td>" + item.Un_OrderNum + "</td>");
                if (item.Pation_OrderNum != 0)
                {
                    sb.Append("<td>" + item.ZWS_Enter_Pation_OrderNum + "%</td>");
                }
                else
                {
                    sb.Append("<td>" + 0 + "%</td>");
                }
                if (IsIn)
                {
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                    sb.Append("<td>N/A</td>");
                }
                else
                {
                    sb.Append("<td>" + item.Actual_OrderNum + "</td>");
                    sb.Append("<td>" + (item.Enter_OrderNum == 0 ? 0 : Math.Round(Convert.ToDouble(Convert.ToDouble(item.Actual_OrderNum) / Convert.ToDouble(item.Enter_OrderNum)), 4) * 100).ToString("0.00") + "%</td>");
                    sb.Append("<td>" + item.Bu_OrderNumStr + "</td>");
                    sb.Append("<td>" + item.Cd_Num + "</td>");
                    sb.Append("<td>" + item.Qq_Num + "</td>");
                    sb.Append("<td>" + item.Zt_Num + "</td>");
                    sb.Append("<td>" + item.Cd_Zt_Num + "</td>");
                }
                sb.Append("</tr>");
                fensuonum++;
            }
            #endregion

            return sb;
        }

        /// <summary>
        /// 导出
        /// </summary>
        public void OutSingleCoursePation()
        {

            StringBuilder html = new StringBuilder();
            html.Append(@"
<table><tr ><td colspan='15' style='text-align:center'>各部门/分所单门课程报名、参与情况表<td></tr> <table>
<table  class='tb' cellspacing='0' cellpadding='0' border='0'><thead>
              <tr>
                <th colspan='3'></th>
                <th colspan='5' style='background-color: gray'>报名情况</th>
                <th colspan='2'>参与情况</th>
                <th colspan='5' style='background-color: gray'>考勤</th>
            </tr>
            <tr>
                <th>序号</th>
                <th>部门/分所</th>
                <th>平均获取学时</th>
                <th>开放人数</th
                <th>报名人数</th>
                <th>请假人数</th>
                <th>退订人数</th>
                <th>课程报名率</th>
                <th>实际参与人数</th>
                <th >课程参与率</th>
                <th>补预订成功人数</th>
                <th>迟到人数</th>
                <th>缺勤人数</th>
                <th>早退人数</th>
                <th>迟到并早退人数</th>
            </tr>
            </thead>
            <tbody>");
            html.Append(GetSingleCoursePationHtml(SingleCoursePationList, 1, IsIn: IsIn));
            html.Append("</tbody></table>");

            new Spreadsheet().ExportExcel_Html(html.ToString(), "SingleCoursePation", HttpContext);

        }
        #endregion


    }
}
