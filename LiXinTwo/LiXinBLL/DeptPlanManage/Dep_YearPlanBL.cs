using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;
using LiXinDataAccess.DeptPlanManage;
using LiXinModels.DepPlanManage;
using LiXinModels.DepCourseManage;
using LiXinInterface.DeptPlanManage;
using LiXinModels.User;

namespace LiXinBLL.DeptPlanManage
{
    public class Dep_YearPlanBL : IDep_YearPlan
    {
        private static Dep_YearPlanDB YearDB;
        private static Dep_YearPlanCourseDB YearCourseDB;
        private static Dep_LinkDepartDB LinkDepartDB;

        public Dep_YearPlanBL()
        {
            YearDB = new Dep_YearPlanDB();
            YearCourseDB = new Dep_YearPlanCourseDB();
            LinkDepartDB = new Dep_LinkDepartDB();
        }

        /// <summary>
        /// 根据部门ID和年 查处联合和同意被联合的所有部门
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Sys_Department> GetALLLinkDepart(int deptid, int year)
        {
            return YearDB.GetALLLinkDepart(deptid, year);
        }

        #region 年计划


        /// <summary>
        /// 获得已经存在的部门ID
        /// </summary>
        public string GetYearDepids(int year)
        {
            return YearDB.GetYearDepIDs(year);
        }

        /// <summary>
        /// 添加新的年计划
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public int InsertYear(Dep_YearPlan model)
        {
            YearDB.AddDepYearPlan(model);
            return model.Id;
        }

        /// <summary>
        /// 根据ID获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dep_YearPlan GetYearModel(int id)
        {
            return YearDB.GetDepYearPlan(id);
        }
        /// <summary>
        /// 根据year获取单个实体
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public Dep_YearPlan GetYearPlanByYear(int year)
        {
            return YearDB.GetDepYearPlanByYear(year);
        }
        /// <summary>
        /// 根据ID删除单个实体(假删)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteYearModel(string ids)
        {
            var bol = YearDB.DeleteDepYearPlan(ids);
            if (bol)
            {
                YearCourseDB.DeleteAllDepYearCourse(ids);
            }
            return bol;
        }

        /// <summary>
        /// 修改年计划
        /// </summary>
        /// <returns></returns>
        public bool UpdateYearByID(Dep_YearPlan model)
        {
            return YearDB.UpdateDepYearPlan(model);
        }

        /// <summary>
        /// 获取年计划列表(分页)
        /// </summary>
        /// <returns></returns>
        public List<Dep_YearPlan> GetAllYearList(string deptIDs, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string strWhere = "1=1")
        {
            var list = YearDB.GetDepYearPlanList(deptIDs, startIndex, startLength, strWhere);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 获取年计划列表
        /// </summary>
        /// <returns></returns>
        public List<Dep_YearPlan> GetAllYear(int depID, string strWhere = "1=1")
        {
            var list = YearDB.GetDepYearList(depID, strWhere);
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
        /// 获得已经存在的年度
        /// </summary>
        public List<int> GetAllYearID(int depID)
        {
            return YearDB.GetDepYear(depID);
        }

        /// <summary>
        /// 获得已经存在的年度
        /// </summary>
        public List<int> GetYear(string where = " 1=1")
        {
            return YearDB.GetYear(where);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Dep_YearPlan> GetDepYearListByWhere(string where = "1=1")
        {
            return YearDB.GetDepYearListByWhere(where);
        }

        /// <summary>
        /// 获得被联合上报部门为0
        /// </summary>
        public List<Dep_YearPlan> GetDepLink_ApprovalFlagList(int year, int deptid)
        {
            return YearDB.GetDepLink_ApprovalFlagList(year, deptid);
        }

        /// <summary>
        ///  获得所有被联合上报部门为0
        /// </summary>
        public List<Dep_YearPlan> GetDepLink_ApprovalFlagList(int deptid)
        {
            return YearDB.GetDepLink_ApprovalFlagList(deptid);
        }

        /// <summary>
        /// 修改其他被联合上报部门为拒绝
        /// </summary>
        /// <returns></returns>
        public bool UpdateDepLink_ApprovalFlag(int year, int deptid)
        {
            return YearDB.UpdateDepLink_ApprovalFlag(year, deptid);
        }

        /// <summary>
        /// 获取计划审批的列表
        /// </summary>
        public List<Dep_YearPlan> GetLinkYearPlanList(string deptIDs, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string strWhere = "1=1")
        {
            var list = YearDB.GetLinkYearPlanList(deptIDs, startIndex, startLength, strWhere);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 年度计划跟踪-部门/分所(已上报)
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="deptype">"in": 部门,"not in":分所</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Dep_YearPlan> GetYearPlanTrackList(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string deptype = "in", string strWhere = "1=1", string jsRenderSortField = " tempTa.Year asc")
        {
            var list = YearDB.GetYearPlanTrackList(startIndex, startLength, deptype, strWhere, jsRenderSortField);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        /// <summary>
        /// 年度计划跟踪-部门/分所(未上报)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptype">0: 部门,1:分所</param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<Sys_Department> GetNoTrackList(int year, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, int deptype = 0, string strWhere = "1=1")
        {
            var list = YearDB.GetNoTrackList(year, startIndex, startLength, deptype, strWhere);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

        #endregion

        #region 联合上报

        /// <summary>
        /// 添加
        /// </summary>
        public int InsertDepLink(Dep_LinkDepart model)
        {
            LinkDepartDB.InsertDep_LinkDepart(model);
            return model.Id;
        }

        /// <summary>
        /// 获取单条信息
        /// </summary>
        public Dep_LinkDepart GetDepLink(int yearID, int deptID)
        {
            return LinkDepartDB.GetDep_LinkDepart(yearID, deptID);
        }

        
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<Dep_LinkDepart> GetDepLink(int deptID)
        {
            return LinkDepartDB.GetDep_LinkDepart(deptID);
        }

        /// <summary>
        /// 修改
        /// </summary>
        public bool UpdateDepLink(Dep_LinkDepart model)
        {
            return LinkDepartDB.UpdateDep_LinkDepart(model);
        }

        /// <summary>
        /// 根据年度ID全部删除
        /// </summary>
        public bool DeleteAllDepLinkbyYear(string YearIds)
        {
            return LinkDepartDB.DeleteAllDepLinkbyYear(YearIds);
        }
        #endregion

        public List<Dep_LinkDepart> GetDep_LinkDepartByYear(int yearId, int deptid)
        {

            return LinkDepartDB.GetDep_LinkDepartByYear(yearId, deptid);
        }


        /// <summary>
        /// 发布年度计划 查看是否有计划被发布
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetIsOrNoLinkDepart(int deptid, int year)
        {
            return YearDB.GetIsOrNoLinkDepart(deptid, year);
        }

        /// <summary>
        /// A部门被联合后 在点发布 此时审批就不能在联合
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dep_YearPlan GetPublicIsOrNoLinkDepart(int deptid, int id)
        {
            return YearDB.GetPublicIsOrNoLinkDepart(deptid, id);
        }

        /// <summary>
        /// 发布年度计划修改联合上报部门为拒绝(联合发布)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateLinkandYearPlan(int year, int id)
        {
            return YearDB.UpdateLinkandYearPlan(year, id);
        }

        /// <summary>
        /// 发布年度计划修改联合上报部门为拒绝(单体发布)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateLinkDepart(int year, int dpetid)
        {
            return YearDB.UpdateLinkDepart(year, dpetid);
        }

        #region 年计划课程

        /// <summary>
        /// 添加年计划课程
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertYearCourse(Dep_YearPlanCourse model)
        {
            return YearCourseDB.AddDepYearCourse(model);

        }

        /// <summary>
        /// 根据ID删除指定课程
        /// </summary>
        /// <param name="YearId"></param>
        /// <param name="CourseIds"></param>
        /// <returns></returns>
        public bool DeleteYearCourse(int YearId, string CourseIds)
        {
            return YearCourseDB.DeleteDepYearCourse(YearId, CourseIds);
        }

        /// <summary>
        /// 根据ID删除全部课程
        /// </summary>
        /// <param name="YearId"></param>
        /// <returns></returns>
        public bool DeleteAllYearCourse(string YearIds)
        {
            return YearCourseDB.DeleteAllDepYearCourse(YearIds);
        }

        /// <summary>
        /// 获取年计划课程列表(无分页)
        /// </summary>
        /// <returns></returns>
        public List<Dep_YearPlanCourse> GetAllYearCourseList(int YearId, string Order = "asc", string where = "1=1")
        {
            var list = YearCourseDB.GetDepYearCourseList(YearId, Order, where);
            return list;
        }

        /// <summary>
        /// 获取年计划课程列表(有分页)
        /// </summary>
        /// <returns></returns>
        public List<Dep_YearPlanCourse> GetAllYearCourseList(int YearId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string Order = "asc", string where = "1=1")
        {
            var list = YearCourseDB.GetDepYearCourseList(YearId, startIndex, startLength, Order, where);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }
        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <returns></returns>
        public List<Dep_YearPlanCourse> GetCourseList(string ids)
        {
            return YearCourseDB.GetDepCourseList(ids);
        }

        /// <summary>
        /// 根据ID获取课程
        /// </summary>
        /// <returns></returns>
        public Dep_YearPlanCourse GetCourse(int id)
        {
            return YearCourseDB.GetDepCo_Course(id);
        }

        /// <summary>
        /// 根据ID删除全部课程
        /// </summary>
        /// <param name="YearId">年度ID</param>
        /// <returns></returns>
        public bool DeleteCoursebyYear(int YearId, string delIds)
        {
            return YearCourseDB.DeleteDepCoursByYear(YearId, delIds);
        }

        /// <summary>
        /// 更新课程信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCo_Course(Dep_Course model)
        {
            return YearCourseDB.UpdateDepCo_Course(model);
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




        #region == 部门开课跟踪 add by yxt ==
        /// <summary>
        /// 获取计划开课详情列表
        /// </summary>
        /// <param name="totalcount">总记录数</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="teacherName">讲师名称</param>
        /// <param name="openLevel">开放级别</param>
        /// <param name="isMust">选修\必修 0:必修；1：选修 ；-1：全部(复选框全不选)； -2：全部（复选框全选）</param>
        /// <param name="isSystem">框架内/外 0:外；1：内 ；-1：全部(复选框全不选)； -2：全部（复选框全选）</param>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门ID</param>
        /// <param name="isOpen">单体/联合上报 0：单体 1：联合 </param>
        /// <param name="where">条件</param>
        /// <param name="startIndex">页所引</param>
        /// <param name="pageLength">每页大小</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public List<Dep_YearPlanCourse> GetDepPlanOpenYearCourseList(out int totalcount, string courseName, string teacherName, string openLevel,
                                                   int isMust, int isSystem, int yearPlan = 0, string deptId = "", int isOpen = 0, string where = "",
                                                   int startIndex = 1, int pageLength = int.MaxValue,
                                                    string orderBy = "")
        {
            where +=
                string.Format(
                    @" and tp.YearId in (select Id from Dep_YearPlan where IsDelete=0 and PublishFlag=1 and  Year={0} and DeptId in({1}) and IsOpen={2}) ",
                    yearPlan, deptId, isOpen);
            if (!string.IsNullOrWhiteSpace(courseName))
            {
                where += string.Format(" and cc.CourseName like '%{0}%' ", courseName.Trim().ReplaceSql());
            }
            if (!string.IsNullOrWhiteSpace(teacherName))
            {
                where += string.Format(" and su.Realname like '%{0}%' ", teacherName.Trim().ReplaceSql());
            }
            if (!string.IsNullOrEmpty(openLevel))
            {
                where +=
                    string.Format(
                        " And (SELECT count(*) FROM  dbo.F_SplitIDs(OpenLevel)  WHERE ID  IN (SELECT ID FROM dbo.F_SplitIDs('{0}')) )>0 ",
                        openLevel);
            }
            if (isMust != -1 && isMust != -2)
            {
                where += string.Format(" and cc.IsMust={0} ", isMust);
            }
            if (isSystem != -1 && isSystem != -2)
            {
                where += (isSystem == 0 ? " and tp.IsSystem=0 " : " and tp.IsSystem>0 ");
            }
            var list = YearCourseDB.GetDepPlanOpenYearCourseList(where, startIndex, pageLength, orderBy);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }
        #endregion
    }
}
