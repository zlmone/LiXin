using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.PlanManage;
using LiXinModels.PlanManage;
using LiXinInterface.PlanManage;
using LiXinModels.CourseManage;

namespace LiXinBLL.PlanManage
{
    public class Tr_YearPlanBL : ITr_YearPlan
    {
        private static Tr_YearPlanDB YearDB;
        private static Tr_YearPlanCourseDB YearCourseDB;

        public Tr_YearPlanBL()
        {
            YearDB = new Tr_YearPlanDB();
            YearCourseDB = new Tr_YearPlanCourseDB();
        }

        #region 年计划

        /// <summary>
        /// 添加新的年计划
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public int InsertYear(Tr_YearPlan model)
        {
            YearDB.AddYearPlan(model);
            return model.Id;
        }

        /// <summary>
        /// 根据ID获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tr_YearPlan GetYearModel(int id)
        {
            return YearDB.GetYearPlan(id);
        }
        /// <summary>
        /// 根据year获取单个实体
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public Tr_YearPlan GetYearPlanByYear(int year)
        {
            return YearDB.GetYearPlanByYear(year);
        }
        /// <summary>
        /// 根据ID删除单个实体(假删)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteYearModel(string ids)
        {
            var bol = YearDB.DeleteYearPlan(ids);
            if (bol)
            {
                YearCourseDB.DeleteAllYearCourse(ids);
            }
            return bol;
        }

        /// <summary>
        /// 修改年计划
        /// </summary>
        /// <returns></returns>
        public bool UpdateYearByID(Tr_YearPlan model)
        {
            return YearDB.UpdateYearPlan(model);
        }

        /// <summary>
        /// 获取年计划列表(分页)
        /// </summary>
        /// <returns></returns>
        public List<Tr_YearPlan> GetAllYearList(out int totalcount, int startIndex = 0, int startLength = int.MaxValue, string strWhere = "1=1")
        {
            var list = YearDB.GetYearPlanList(startIndex, startLength, strWhere);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 获取年计划列表
        /// </summary>
        /// <returns></returns>
        public List<Tr_YearPlan> GetAllYear(string strWhere = "1=1")
        {
            var list = YearDB.GetYearList(strWhere);
            return list;
        }

        /// <summary>
        /// 验证重名
        /// </summary>
        /// <returns></returns>
        public bool Checkname(int year)
        {
            return YearDB.Exists(year);
        }

        /// <summary>
        /// 获得所有年度
        /// </summary>
        public List<int> GetAllYear()
        {
            return YearDB.GetYear();
        }

        /// <summary>
        /// 获得最大年度
        /// </summary>
        public List<int> GetALLDepYear()
        {
            return YearDB.GetALLDepYear();
        }

        #endregion

        #region 年计划课程

        /// <summary>
        /// 添加年计划课程
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertYearCourse(Tr_YearPlanCourse model)
        {
            return YearCourseDB.AddYearCourse(model);

        }

        /// <summary>
        /// 根据ID删除指定课程
        /// </summary>
        /// <param name="YearId"></param>
        /// <param name="CourseIds"></param>
        /// <returns></returns>
        public bool DeleteYearCourse(int YearId, string CourseIds)
        {
            return YearCourseDB.DeleteYearCourse(YearId, CourseIds);
        }

        /// <summary>
        /// 根据ID删除全部课程
        /// </summary>
        /// <param name="YearId"></param>
        /// <returns></returns>
        public bool DeleteAllYearCourse(string YearIds)
        {
            return YearCourseDB.DeleteAllYearCourse(YearIds);
        }

        /// <summary>
        /// 获取年计划课程列表(无分页)
        /// </summary>
        /// <returns></returns>
        public List<Tr_YearPlanCourse> GetAllYearCourseList(int YearId, string Order = "asc", string where = "1=1")
        {
            var list = YearCourseDB.GetYearCourseList(YearId, Order, where);
            return list;
        }

        /// <summary>
        /// 获取年计划课程列表(有分页)
        /// </summary>
        /// <returns></returns>
        public List<Tr_YearPlanCourse> GetAllYearCourseList(int YearId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            var list = YearCourseDB.GetYearCourseList(YearId, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }
        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <returns></returns>
        public List<Tr_YearPlanCourse> GetCourseList(string ids)
        {
            return YearCourseDB.GetCourseList(ids);
        }

        /// <summary>
        /// 根据ID获取课程
        /// </summary>
        /// <returns></returns>
        public Tr_YearPlanCourse GetCourse(int id)
        {
            return YearCourseDB.GetCo_Course(id);
        }

        /// <summary>
        /// 根据ID删除全部课程
        /// </summary>
        /// <param name="YearId">年度ID</param>
        /// <returns></returns>
        public bool DeleteCoursebyYear(int YearId, string delIds)
        {
            return YearCourseDB.DeleteCoursByYear(YearId, delIds);
        }

        /// <summary>
        /// 更新课程信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCo_Course(Co_Course model)
        {
            return YearCourseDB.UpdateCo_Course(model);
        }
        /// <summary>
        /// 判断年度计划下面是否删除讲师
        /// </summary>
        /// <param name="YearId"></param>
        /// <returns></returns>
        public bool IsYearUser(int YearId)
        {
            int delcoun = YearCourseDB.UserIsdel(YearId);
            if (delcoun > 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
