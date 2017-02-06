using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinModels;
using Newtonsoft.Json;
using LiXinModels.CourseManage;
using LiXinCommon;
using LiXinModels.PlanManage;
using System.Data;
using LiXinInterface;

namespace LiXinControllers
{
    public partial class MyPlanController
    {
        #region 呈现
        public ActionResult MyPlanList()
        {
            ViewBag.year = monthBL.GetYearOfPlan();
            return View();
        }
        #endregion

        #region 我的培训计划
        /// <summary>
        /// 月度计划
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyMonthPlan(int year = -1, int pop_Way = -1, string month = "-1", int pageSize = 10, int pageIndex = 1, string courseName = "", string teachername = "", int isMaster = -1, string jsRenderSortField = " PreCourseTime desc")
        {
            try
            {
                int totalCount = 0;
                #region 查询条件
                string where = " 1=1 ";
                if (month != "-1")
                {
                    where += string.Format(" AND charindex( Month,'{0}')>0", month);
                }
                if (year != -1)
                {
                    where += " AND Year=" + year;
                }
                if (pop_Way != -1)
                {
                    where += " and Way=" + pop_Way;
                }
                if (isMaster != -1)
                {
                    where += " and IsMust=" + isMaster;
                }
                if (courseName != "")
                {
                    where += " and courseName like '%" + courseName + "%'";
                }
                if (teachername != "")
                {
                    where += " and Realname like '%" + teachername + "%'";
                }
                #endregion

                var monthList = monthBL.GetMyMonthPlan(out totalCount, CurrentUser.UserId, pageIndex, pageSize, where, jsRenderSortField);
                foreach (var item in monthList)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.Realname = item.Realname.HtmlXssEncode();
                }
                return Json(new
                {
                    result = 1,
                    dataList = monthList,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Tr_MonthPlan>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 年度计划
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyYearPlan(int year = -1, int pop_Way = -1, int pageSize = 10, int pageIndex = 1, string courseName = "", string teachername = "", int isMaster = -1, string jsRenderSortField = " Month desc")
        {
            try
            {
                int totalCount = 0;
                #region 查询条件
                string where = " 1=1 ";
               
                if (year != -1)
                {
                    where += " AND Year=" + year;
                }
                if (pop_Way != -1)
                {
                    where += " and Way=" + pop_Way;
                }
                if (isMaster != -1)
                {
                    where += " and IsMust=" + isMaster;
                }
                if (courseName != "")
                {
                    where += " and courseName like '%" + courseName + "%'";
                }
                if (teachername != "")
                {
                    where += " and Realname like '%" + teachername + "%'";
                }
                #endregion
                var monthList = monthBL.GetMyYearPlan(out totalCount, CurrentUser.UserId, pageIndex, pageSize, where, jsRenderSortField);
                foreach (var item in monthList)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.Realname = item.Realname.HtmlXssEncode();
                }
                return Json(new
                {
                    result = 1,
                    dataList = monthList,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<CourseShow>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}
