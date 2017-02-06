using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DeptPlanManage;
using LiXinInterface.DeptPlanManage;
using LiXinModels.DepPlanManage;
using LiXinModels.DeptCourseManage;
using LiXinModels.DepCourseManage;
using LiXinModels;
using LiXinCommon;

namespace LiXinBLL.DeptPlanManage
{
    public class Dep_MonthPlanBL : IDep_MonthPlan
    {
        public Dep_MonthPlanDB monthDB;
        public Dep_MonthPlanBL()
        {
            monthDB = new Dep_MonthPlanDB();
        }

        #region 新增改查
        /// <summary>
        /// 新增一条数据月度大纲
        /// </summary>     
        public void InsertDep_MonthPlan(Dep_MonthPlan model)
        {
            monthDB.InsertDep_MonthPlan(model);
        }

        /// <summary>
        /// 新增一条数据月度大纲内的课程
        /// </summary>     
        public void InsertDep_MonthPlanCourse(Dep_MonthPlanCourse model)
        {
            monthDB.InsertDep_MonthPlanCourse(model);
        }

        /// <summary>
        /// 插入课程
        /// </summary>
        /// <param name="model"></param>
        public void InsertDept_Course(Dep_Course model)
        {
            monthDB.InsertDept_Course(model);
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


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        public void UpdateMonthPlan(int id, string where)
        {
            monthDB.UpdateMonthPlan(id, where);
        }

        /// <summary>
        /// 获得单个的月度大纲
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dep_MonthPlan Get(int id)
        {
            return monthDB.Get(id);
        }

        /// <summary>
        /// 删除月度计划以及下面的课程
        /// </summary>
        /// <param name="courseID"></param>
        public void DeleteMonPlanAndCourse(int MonthId)
        {
            monthDB.DeleteMonPlanAndCourse(MonthId);
        }
        #endregion

        #region 月度大纲

        /// <summary>
        /// 获取计划的基本信息
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Dep_MonthPlan> GetMonthPlan(out int totalCount, int DeptID = 0, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1",string deptwhere=" 1=1", string jsRenderSortField = "  month asc")
        {
            var list = monthDB.GetMonthPlan(DeptID, startIndex, startLength, where,deptwhere, jsRenderSortField);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }


        /// <summary>
        /// 获取计划的基本信息
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Dep_MonthPlan> GetMonthPlanForMaoJiaYuan(out int totalCount, int DeptID = 0, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string deptwhere = " 1=1", string beiwhere="1=1",string jsRenderSortField = "  month asc")
        {
            var list = monthDB.GetMonthPlanForMaoJiaYuan(DeptID, startIndex, startLength, where, deptwhere,beiwhere, jsRenderSortField);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

        /// <summary>
        /// 是否存在此月度计划，
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>true存在，false不存在</returns>
        public bool IsExistMonplan(int year, string month, int id, int deptID = 0)
        {
            return monthDB.IsExistMonplan(year, month, id, deptID);
        }
        #endregion

        #region 月度分解
        /// <summary>
        /// 获取年度计划内的课程详情
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<LiXinModels.CourseShow> GetYearCourseDetails(out int totalCount, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1")
        {
            var list = monthDB.GetYearCourseDetails(startIndex, startLength, where);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

        /// <summary>
        /// 获取月度计划内的课程详情
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<LiXinModels.CourseShow> GetMonthCourseDetails(out int totalCount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " cc.month desc")
        {
            var list = monthDB.GetMonthCourseDetails(startIndex, startLength, where, jsRenderSortField);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }
        #endregion

        #region 月度差异对比
        /// <summary>
        /// 差异对比的头部信息 年度预计课程数，实际课程数等
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dep_MonthPlan GetPlanCourseCount(int id, int deptID, string where = " 1=1")
        {
            return monthDB.GetPlanCourseCount(id, deptID, where);
        }

        /// <summary>
        /// 月度差异对比
        /// </summary>
        public List<LiXinModels.CourseShow> GetMonPlanCompare(int year, string month, int DeptID)
        {
            var courseList = new List<LiXinModels.CourseShow>();
            var monthCourseID = new List<int>();
            try
            {
                var where = string.Format(" ty.DeptId={1} and  ty.Year={0} ", year, DeptID);
                //年度课程 
                var yearCourse = monthDB.GetYearCourseDetails(1, int.MaxValue, where);

                var mwhere = string.Format(" ty.DeptId ={0}  And ty.Year={1} AND ty.Month='{2}' ", DeptID, year, month);
                //月度课程
                var monthCourse = monthDB.GetMonthCourseDetails(1, int.MaxValue, mwhere);

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

            return new List<LiXinModels.CourseShow>();
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
        #endregion


        /// <summary>
        /// 获取年份
        /// </summary>
        /// <returns></returns>
        public List<int> GetYearOfPlan(int deptid)
        {
            return monthDB.GetYearOfPlan(deptid);
        }
    }
}
