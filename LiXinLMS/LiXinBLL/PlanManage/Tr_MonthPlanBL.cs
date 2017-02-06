using System;
using System.Collections.Generic;
using System.Linq;
using LiXinBLL.SystemManage;
using LiXinCommon;
using LiXinDataAccess;
using LiXinInterface;
using LiXinModels;
using LiXinModels.CourseManage;
using LiXinModels.PlanManage;

namespace LiXinBLL
{
    public class Tr_MonthPlanBL : ITr_MonthPlan
    {
        private Tr_MonthPlanDB monthDB;
        private Sys_ParamConfigBL configBL;
        public Tr_MonthPlanBL()
        {
            monthDB = new Tr_MonthPlanDB();
            configBL = new Sys_ParamConfigBL();
        }

        #region 月度计划首页
        /// <summary>
        /// 是否存在此月度计划，
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>true存在，false不存在</returns>
        public bool IsExistMonplan(int year, string month, int id)
        {
            return monthDB.IsExistMonplan(year, month, id);
        }
        /// <summary>
        /// 获取计划的基本信息
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Tr_MonthPlan> GetMonthPlan(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, int year = -1, string month = "-1", int publish = -1, string query = "", string jsRenderSortField = "  month asc")
        {
            string where = "1=1";
            if (year != -1)
            {
                where += " and Year=" + year;
            }
            if (month != "-1")
            {
                where += " and Month='" + month + "'";
            }
            if (publish != -1)
            {
                where += " and PublishFlag=" + publish;
            }
            if (query != "")
            {
                where += query;
            }
            var list = monthDB.GetMonthPlan(startIndex, startLength, where, jsRenderSortField);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id"></param>
        public void Publish(int id)
        {
            monthDB.UpdateMonthPlan(id, "PublishFlag=1");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            monthDB.UpdateMonthPlan(id, " IsDelete=1");
        }

        /// <summary>
        /// 获取年份
        /// </summary>
        /// <returns></returns>
        public List<int> GetYearOfPlan()
        {
            return monthDB.GetYearOfPlan();
        }

        /// <summary>
        /// 专为框架内外准备的数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetYearCourse(int year, string courseID)
        {
            var list = monthDB.GetYearCourse(year, courseID);
            var dic = new Dictionary<int, int>();
            foreach (var item in list)
            {
                dic[item.CourseId] = item.IsSystem;
            }
            return dic;
        }


        public void PublishAndCourse(int id)
        {
            monthDB.publish(id);
        }
        #endregion

        #region 年度 月度 课程查询

        /// <summary>
        /// 获取年度计划内的课程详情
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<CourseShow> GetYearCourseDetails(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string openlevel = "", string month = "", int year = -1, string courseName = "")
        {
            string where = "1=1";
            if (!string.IsNullOrEmpty(openlevel))
            {
                // where += string.Format(" And cc.OpenLevel LIKE '%{0}%'", openlevel);
                where += string.Format(" And (SELECT count(*) FROM  dbo.F_SplitIDs(OpenLevel)  WHERE ID  IN (SELECT ID FROM dbo.F_SplitIDs('{0}')) )>0", openlevel);
            }
            if (!string.IsNullOrEmpty(month))
            {
                where += string.Format(" AND charindex( cc.Month,'{0}')>0", month);
            }
            if (!string.IsNullOrEmpty(courseName))
            {
                where += string.Format(" AND cc.CourseName LIKE '%{0}%'", courseName);
            }
            if (year != -1)
            {
                where += " AND ty.Year=" + year;
            }
            var list = monthDB.GetYearCourseDetails(startIndex, startLength, where);

            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 获取月度计划内的课程详情
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<CourseShow> GetMonthCourseDetails(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, int id = -1, string jsRenderSortField = " cc.month desc", string where = " 1=1")
        {
            //string where = "1=1";
            if (id != -1)
            {
                where += " AND ty.Id=" + id;
            }
            var list = monthDB.GetMonthCourseDetails(startIndex, startLength, where, jsRenderSortField);

            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 获得课程
        /// </summary>
        /// <param name="CourseID"></param>
        /// <returns></returns>
        public CourseShow GetSingleCourse(int CourseID)
        {
            var list = monthDB.GetSingleCourse(CourseID);
            return list.Count == 0 ? new CourseShow() : list[0];
        }
        #endregion

        #region  新增、更新 月度计划，计划与课程的关联
        /// <summary>
        /// 插入月度数据库
        /// </summary>
        /// <param name="model"></param>
        public void InsertTr_MonthPlan(Tr_MonthPlan model)
        {
            monthDB.InsertTr_MonthPlan(model);
        }

        /// <summary>
        /// 插入月度计划和课程的关联
        /// </summary>
        /// <param name="model"></param>
        public void InsertTr_MonthPlanCourse(Tr_MonthPlanCourse model)
        {
            monthDB.InsertTr_MonthPlanCourse(model);
        }

        /// <summary>
        /// 插入课程
        /// </summary>
        /// <param name="model"></param>
        public void InsertCo_Course(Co_Course model)
        {
            monthDB.InsertCo_Course(model);
        }

        /// <summary>
        /// 删除计划中的课程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseID"></param>
        public void DeleteMonPlanCourse(int id, string courseID)
        {
            monthDB.DeleteMonPlanCourse(id, courseID);
        }
        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="courseIDs"></param>
        public void DeleteCourse(string courseIDs)
        {
            monthDB.DeleteCourse(courseIDs);
        }

        public Tr_MonthPlan Get(int id)
        {
            return monthDB.Get(id);
        }
        #endregion

        #region 差异对比
        /// <summary>
        /// 月度差异对比
        /// </summary>
        public List<CourseShow> GetMonPlanCompare(int year, string month)
        {
            var courseList = new List<CourseShow>();
            var monthCourseID = new List<int>();
            try
            {   //年度课程 
                var yearCourse = monthDB.GetYearCourseDetails(1, int.MaxValue, "  ty.Year=" + year);

                //月度课程
                var monthCourse = monthDB.GetMonthCourseDetails(1, int.MaxValue, " ty.Year=" + year + " AND ty.Month='" + month + "'");

                //将未变和新增的筛选出来
                foreach (var item in monthCourse)
                {
                    if (item.SurveyPaperId == "0")
                    {
                        item.DeleteOrNew = "2";
                    }
                    else
                    {
                        var ycourse = yearCourse.Where(p => p.Id == item.SurveyPaperId.StringToInt32()).FirstOrDefault();
                        if (ycourse != null)
                        {
                            monthCourseID.Add(item.SurveyPaperId.StringToInt32());
                            item.DeleteOrNew = "0";
                            item.OpenLevelUpdate = ycourse.OpenLevel;
                            item.teacherUpdate = ycourse.Realname;
                        }
                        else
                        {
                            item.DeleteOrNew = "2";
                        }

                    }
                    courseList.Add(item);
                }

                foreach (var item in yearCourse)
                {
                    if (!monthCourseID.Contains(item.Id))
                    {
                        item.DeleteOrNew = "1";
                        courseList.Add(item);
                    }
                }
                return courseList;
            }
            catch (Exception)
            {

                throw;
            }

            return new List<CourseShow>();
        }

        /// <summary>
        /// 差异对比的头部信息 年度预计课程数，实际课程数等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tr_MonthPlan GetPlanCourseCount(int id)
        {
            var list = monthDB.GetPlanCourseCount(id);
            if (list.Count > 0)
            {
                return list[0];
            }
            return new Tr_MonthPlan();
        }
        #endregion

        #region 我的培训计划 月度
        /// <summary>
        /// 我的培训计划 月度
        /// </summary>
        /// <returns></returns>
        public List<CourseShow> GetMyMonthPlan(out int totalcount, int userID, int startIndex = 1, int startLength = int.MaxValue, string where = " 1=1", string jsRenderSortField = " PreCourseTime desc")
        {

            var list = monthDB.GetMyMonthPlan(userID, startIndex, startLength, where,jsRenderSortField);

            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }


        /// <summary>
        /// 我的培训计划 年度
        /// </summary>
        /// <returns></returns>
        public List<CourseShow> GetMyYearPlan(out int totalcount, int userID, int startIndex = 1, int startLength = int.MaxValue, string where = " 1=1 ", string jsRenderSortField = " Month desc")
        {

            var list = monthDB.GetMyYearPlan(userID, startIndex, startLength, where,jsRenderSortField);

            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        #endregion

        #region 公共
        /// <summary>
        /// 获取配置当中的年度设定
        /// </summary>
        /// <param name="year"></param>
        /// <returns>月份  形如 2013-5</returns>
        public List<string> GetMonthOfConfig(Sys_ParamConfig list, int year)
        {
            // var list = configBL.GetSys_ParamConfig(7);
            var ArrayValue = list.ConfigValue.Split(';');
            var listmonth = new List<string>();

            if (ArrayValue[0].StringToInt32() == 1)
            {
                var month = ArrayValue[1].StringToInt32();

                var maxmonth = ArrayValue[2].StringToInt32() == 2 ? 12 : ArrayValue[3].StringToInt32();
                //当年的月份
                for (int i = 0; i <= maxmonth - month; i++)
                {
                    var spilt = month + i >= 10 ? "-" : "-0";

                    listmonth.Add(year + spilt + (month + i));
                }

                //如果有次年的话
                if (ArrayValue[2].StringToInt32() == 2)
                {
                    var nextmonth = ArrayValue[3].StringToInt32();
                    for (int i = 1; i <= nextmonth; i++)
                    {
                        var spilt = i >= 10 ? "-" : "-0";
                        listmonth.Add((year + 1) + spilt + i);
                    }
                }
            }

            return listmonth;
        }

        /// <summary>
        /// 根据配置文件和当前时间，获取本年度的开始时间和结束时间
        /// 如（startTime = 2012-10-01 0:00:00 ; endTime = 2012-12-31 23:59:59 ）
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="yearConfig"></param>
        public void GetYearStartAndEnd(out DateTime startTime, out DateTime endTime, Sys_ParamConfig yearConfig, int year)
        {
            startTime = DateTime.Now;
            endTime = DateTime.Now;
            var ArrayValue = yearConfig.ConfigValue.Split(';');
            if (ArrayValue[0].StringToInt32() == 1)
            {
                var month1 = ArrayValue[1].StringToInt32();
                var month2 = ArrayValue[3].StringToInt32();
                var year1 = year;
                var year2 = year;
                if (ArrayValue[2].StringToInt32() == 2)
                    year2 = year1 + 1;
                startTime = DateTime.Parse(year1 + "-" + month1 + "-01 0:00:00");
                endTime = DateTime.Parse(year2 + "-" + month2 + "-01 0:00:00").AddMonths(1).AddSeconds(-1);
            }
        }

        /// <summary>
        /// 根据配置文件和当前时间，获取本年度的开始时间和结束时间
        /// 如（startTime = 2012-10-01 0:00:00 ; endTime = 2012-12-31 23:59:59 ）
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="yearConfig"></param>
        public void YearPlanStartAndEnd(out DateTime startTime, out DateTime endTime, Sys_ParamConfig yearConfig, int year)
        {
            startTime = DateTime.Now;
            endTime = DateTime.Now;
            var ArrayValue = yearConfig.ConfigValue.Split(';');
            if (ArrayValue[0].StringToInt32() == 1)
            {
                var month1 = ArrayValue[1].StringToInt32();
                var month2 = ArrayValue[3].StringToInt32();
                var year1 = year;
                var year2 = year;
                if (ArrayValue[2].StringToInt32() == 2)
                    year2 = year + 1;
                startTime = DateTime.Parse(year1 + "-" + month1 + "-01 0:00:00");
                endTime = DateTime.Parse(year2 + "-" + month2 + "-01 0:00:00").AddMonths(1).AddSeconds(-1);
            }
        }

        /// <summary>
        /// 根据配置文件和当前时间，获取CPA周期
        /// 如（2012年10月—2013年12月）
        /// </summary>
        /// <param name="yearConfig"></param>
        public List<string> CPAStartAndEnd(Sys_ParamConfig CPAConfig)
        {
            int startYear = CPAConfig.LastUpdateTime.Year;
            var ArrayValue = CPAConfig.ConfigValue.Split(';');
            int cpavalue = ArrayValue[0].StringToInt32();
            int endYear = startYear + cpavalue;
            int month1 = ArrayValue[1].StringToInt32();
            int month2 = ArrayValue[2].StringToInt32();
            DateTime startTime = DateTime.Parse(startYear + "-" + month1 + "-01 0:00:00");
            DateTime endTime = DateTime.Parse(endYear + "-" + month2 + "-01 0:00:00").AddMonths(1).AddSeconds(-1);
            List<string> cpaTime = new List<string>();
            if (DateTime.Now >= startTime && DateTime.Now <= endTime)
            {
                string linshi = startYear + "年" + month1 + "月—" + endYear + "年" + month2 + "月";
                cpaTime.Add(linshi);
            }
            else if (DateTime.Now > endTime)
            {
                int yearcha = DateTime.Now.Year - startYear;
                int ss = yearcha / cpavalue;
                for (int i = 0; i <= ss; i++)
                {
                    string linshi = (startYear + (i * cpavalue)) + "年" + month1 + "月—" + (endYear + (i * cpavalue)) + "年" + month2 + "月";
                    cpaTime.Add(linshi);
                }
            }
            return cpaTime;
        }
        #endregion
    }
}
