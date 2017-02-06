using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinModels.Report_zVedio;
using System.Data;
using System.Diagnostics;
using LiXinModels;
using System.Text.RegularExpressions;

namespace LiXinControllers
{
    public partial class Report_fVedioController : BaseController
    {
        private static List<CourseJoin> CourseJoinList;
        private static ChartModel joinpie = new ChartModel();//学员人数分布
        private static ChartModel joinzong = new ChartModel();//各月份学习情况分布
        private static ChartModel joinfeng = new ChartModel();//分所员工学习分布
        private static ChartModel joinmonth = new ChartModel();//总所各部门学习人数分布

        #region  呈现
        /// <summary>
        /// 总所视频课程
        /// </summary>
        /// <returns></returns>
        public ActionResult FVedioLearnList(string cp, int tabIndex = 0)
        {
            ViewBag.zpageInex = 1;
            ViewBag.fpageIndex = 1;
            ViewBag.Attname = "请输入搜索内容";
            //汇总
            if (!string.IsNullOrEmpty(cp))
            {
                ViewBag.type = 0;
                if (tabIndex == 0)
                {
                    if (Session["fvedio_all"] != null)
                    {
                        string sess = Session["fvedio_all"].ToString();
                        string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                        ViewBag.zpageInex = att[0];
                        ViewBag.courseName = att[1] == "" ? "请输入搜索内容" : att[1]; ;
                        ViewBag.start = att[2];
                        ViewBag.end = att[3];
                    }

                }
                else
                {
                    //单个
                    if (Session["fvedio_single"] != null)
                    {
                        // pageIndex + "㉿" + courseName + "㉿" + startyear + "㉿" + endyear + "㉿" + teacher + "㉿" + isMust + "㉿" + isTest + "㉿" + IsCPA 
                        //+ "㉿" + StartTime + "㉿" + EndTime + "㉿" + OpenObject;
                        string sess = Session["fvedio_single"].ToString();
                        string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                        ViewBag.fpageIndex = att[0];
                        ViewBag.fcourseName = att[1] == "" ? "请输入搜索内容" : att[1]; ;
                        ViewBag.startyear = att[2];
                        ViewBag.endyear = att[3];
                        ViewBag.teacher = att[4] == "" ? "请输入搜索内容" : att[4];
                        ViewBag.isMust = att[5];
                        ViewBag.isTest = att[6];
                        ViewBag.IsCPA = att[7];
                        ViewBag.StartTime = att[8];
                        ViewBag.EndTime = att[9];
                        ViewBag.OpenObject = att[10] == "" ? "请输入搜索内容" : att[10];
                    }

                }
            }
            else
            {
                ViewBag.type = 1;
            }
            return View();
        }

        /// <summary>
        /// 视频课程详情
        /// </summary>
        /// <returns></returns>
        public ActionResult FVedioLearnDetail(int courseID, int type = 0)
        {
            ViewBag.courseID = courseID;
            ViewBag.type = type;
            return View();
        }

        public ActionResult FCourseJoinDetails(int courseID)
        {
            ViewBag.courseID = courseID;
            return View();
        }

        /// <summary>
        /// 课程基本信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public ActionResult FCourseBaseInfo(int courseID)
        {
            var model = vedioBL.GetVedioLearnSingle(" CourseID=" + courseID).FirstOrDefault();

            return View(model);
        }

        #endregion

        #region 汇总
        /// <summary>
        /// 视频课程汇总统计
        /// </summary>
        /// <returns></returns>
        public JsonResult GetVedioList(string courseName = "", string start = "", string end = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " ReallyNumber desc")
        {
            try
            {
                if (Session["fvedio_all"] != null)
                {
                    Session.Remove("fvedio_all");
                }
                Session["fvedio_all"] = pageIndex + "㉿" + courseName + "㉿" + start + "㉿" + end;
                var where = " 1=1";
                if (!string.IsNullOrEmpty(start))
                {
                    where += " and YearPlan>=" + start;
                }
                if (!string.IsNullOrEmpty(end))
                {
                    where += " and YearPlan<=" + end;
                }
                if (!string.IsNullOrEmpty(courseName))
                {
                    where += " and CourseName like '%" + courseName.ReplaceSql() + "%'";
                }
                var list = ReportBL.GetVedioLearn(GetAllSubLeardDepartments(), where);

                list = list.SortListByField(jsRenderSortField);
                var totalcount = list.Count;
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<CourseVedioLearn>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导出视频课程汇总统计(总所)
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void OutVedioList(string courseName = "", string start = "", string end = "")
        {
            var where = " 1=1";
            if (!string.IsNullOrEmpty(start))
            {
                where += " and YearPlan>=" + start;
            }
            if (!string.IsNullOrEmpty(end))
            {
                where += " and YearPlan<=" + end;
            }
            if (!string.IsNullOrEmpty(courseName))
            {
                where += " and CourseName like '%" + courseName.ReplaceSql() + "%'";
            }
            var list = ReportBL.GetVedioLearn(GetAllSubLeardDepartments(), where);
            list = list.SortListByField(" ReallyNumber desc");
            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("课程名称", typeof(string));
            outTable.Columns.Add("全所参与学习人数", typeof(string));
            outTable.Columns.Add("部门或分所参与学习人数", typeof(string));
            outTable.Columns.Add("全所开放总人数", typeof(string));
            outTable.Columns.Add("部门或分所开放总人数", typeof(string));
            outTable.Columns.Add("全所学习人次", typeof(string));
            outTable.Columns.Add("部门或分所学习人次", typeof(string));
            outTable.Columns.Add("全所参与率", typeof(string));
            outTable.Columns.Add("部门或分所参与率", typeof(string));
            outTable.Columns.Add("全所在线测试通过率", typeof(string));
            outTable.Columns.Add("部门或分所在线测试通过率", typeof(string));
            outTable.Columns.Add("课后评估综合平均分", typeof(string));
            outTable.Columns.Add("课程评估平均分", typeof(string));
            outTable.Columns.Add("教师评估平均分", typeof(string));
            var index = 1;
            foreach (var v in list)
            {
                outTable.Rows.Add(index, v.CourseName, v.LearnNumber, v.fLearnNumber, v.Number, v.fNumber,
                     v.ReallyNumber, v.fReallyNumber, v.JoinRate, v.fJoinRate, v.PassRate, v.fPassRate, v.Survey_AllScore, v.Survey_CourseScore, v.Survey_TeacherScore);
                index++;
            }
            new Spreadsheet().Template("视频课程汇总统计(分所)", null, outTable, HttpContext, "视频课程汇总统计(分所)", "视频课程汇总统计(分所)");
        }
        #endregion

        #region 单个
        /// <summary>
        /// 视频课程单门课程统计
        /// </summary>
        /// <param name="startyear">开始年度</param>
        /// <param name="endyear">结束年度</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="teacher">讲师</param>
        /// <param name="isMust">是否必修</param>
        /// <param name="isTest">是否有在线测试</param>
        /// <param name="IsCPA">是否折算CPA</param>
        /// <param name="StartTime">开课时间 开始</param>
        /// <param name="EndTime">开课时间 结束</param>
        /// <param name="OpenObject">开放对象</param>
        /// <returns></returns>
        public JsonResult GetVedioLearnSingle(string startyear = "", string endyear = "", string courseName = "", string teacher = "", int isMust = -1,
          int isTest = -1, int IsCPA = -1, string StartTime = "", string EndTime = "", string OpenObject = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " StartTime desc")
        {
            try
            {
                if (Session["fvedio_single"] != null)
                {
                    Session.Remove("fvedio_single");
                }
                Session["fvedio_single"] = pageIndex + "㉿" + courseName + "㉿" + startyear + "㉿" + endyear + "㉿" + teacher + "㉿" + isMust
                    + "㉿" + isTest + "㉿" + IsCPA + "㉿" + StartTime + "㉿" + EndTime + "㉿" + OpenObject;

                string where = " 1=1";
                #region 条件
                if (!string.IsNullOrEmpty(startyear))
                {
                    where += " And YearPlan>=" + startyear;
                }
                if (!string.IsNullOrEmpty(endyear))
                {
                    where += " And YearPlan<=" + endyear;
                }
                if (!string.IsNullOrEmpty(courseName))
                {
                    where += " And CourseName like '%" + courseName.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(teacher))
                {
                    where += " And teacher like '%" + teacher.ReplaceSql() + "%'";
                }
                if (isMust != -1)
                {
                    where += " And isMust=" + isMust;
                }
                if (isTest != -1)
                {
                    where += " And isTest=" + isTest;
                }
                if (IsCPA != -1)
                {
                    where += " And IsCPA=" + IsCPA;
                }
                if (!string.IsNullOrEmpty(StartTime))
                {
                    where += " And  StartTime<=" + StartTime;
                }
                if (!string.IsNullOrEmpty(EndTime))
                {
                    where += " And EndTime>=" + EndTime;
                }
                if (!string.IsNullOrEmpty(OpenObject))
                {
                    where += " And openObject like '%" + OpenObject.ReplaceSql() + "%'";
                }
                #endregion
                int totalcount = 0;
                // var aa = cacheManager.Get<dynamic>("Vedio_Number");

                var list = ReportBL.GetVedioLearnSingle(GetAllSubLeardDepartments(), where);

                totalcount = list.Count;
                list = list.SortListByField(jsRenderSortField);

                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<VedioLearnSingle>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///导出视频课程单门课程统计
        /// </summary>
        public void OutVedioSingleList(string startyear = "", string endyear = "", string courseName = "", string teacher = "", int isMust = -1,
          int isTest = -1, int IsCPA = -1, string StartTime = "", string EndTime = "", string OpenObject = "")
        {
            string where = " 1=1";
            #region 条件
            if (!string.IsNullOrEmpty(startyear))
            {
                where += " And YearPlan>=" + startyear;
            }
            if (!string.IsNullOrEmpty(endyear))
            {
                where += " And YearPlan<=" + endyear;
            }
            if (!string.IsNullOrEmpty(courseName))
            {
                where += " And CourseName like '%" + courseName.ReplaceSql() + "%'";
            }
            if (!string.IsNullOrEmpty(teacher))
            {
                where += " And teacher like '%" + teacher.ReplaceSql() + "%'";
            }
            if (isMust != -1)
            {
                where += " And isMust=" + isMust;
            }
            if (isTest != -1)
            {
                where += " And isTest=" + isTest;
            }
            if (IsCPA != -1)
            {
                where += " And IsCPA=" + IsCPA;
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                where += " And  StartTime>=" + StartTime;
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                where += " And EndTime<=" + EndTime;
            }
            if (!string.IsNullOrEmpty(OpenObject))
            {
                where += " And openObject like '" + OpenObject.ReplaceSql() + "'";
            }
            #endregion
            // var aa = cacheManager.Get<dynamic>("Vedio_Number");
            var list = ReportBL.GetVedioLearnSingle(GetAllSubLeardDepartments(), where);
            list = list.SortListByField(" StartTime desc");
            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("课程名称", typeof(string));
            outTable.Columns.Add("开放时间", typeof(string));
            outTable.Columns.Add("讲师", typeof(string));
            outTable.Columns.Add("学时", typeof(string));
            outTable.Columns.Add("选/必修", typeof(string));
            outTable.Columns.Add("开放对象", typeof(string));
            outTable.Columns.Add("是否折算CPA学时", typeof(string));
            outTable.Columns.Add("是否有在线测试", typeof(string));
            outTable.Columns.Add("全所/部门或分所已参加人数", typeof(string));
            outTable.Columns.Add("开放总人数", typeof(string));
            var index = 1;
            foreach (var v in list)
            {
                outTable.Rows.Add(index, v.CourseName, v.CourseTime, v.Teacher, v.CourseLength, v.IsMustStr, v.openObjectStr,
                    v.IsCPAStr, v.IsTestStr,
                    "'" + v.fNumber, v.sumNumber);
                index++;
            }
            new Spreadsheet().Template("视频课程单门课程统计(总所)", null, outTable, HttpContext, "视频课程单门课程统计(总所)", "视频课程单门课程统计(总所)");
        }
        #endregion




        //刷新缓存
        public void Refresh()
        {
            RefreshfVedioJoinNumber();
            RefreshVedioSumNumber();
            RefreshfVedioSurvey();
            RefreshGetExamList();
        }




    }
}
