using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DeptCourseManage;
using LiXinInterface.DeptCourseManage;
using LiXinModels.DeptCourseManage;

namespace LiXinBLL.DeptCourseManage
{
    public class DeptCourseBL : IDeptCourse
    {
        private DeptCourseDB deptDb;
        public DeptCourseBL()
        {
            deptDb = new DeptCourseDB();
        }

        /// <summary>
        /// 获取课程相关信息和用户所在分组的部门编号
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="Id">课程ID</param>
        /// <returns></returns>
        public Dept_Course GetCo_Course(int Id, int userId, int deptId=0)
        {
            return deptDb.GetCo_Course(Id, userId, deptId);
        }

        /// <summary>
        /// 获取课程相关信息及分页
        /// </summary>
        /// <param name="currentDeptId">用户部门ID</param>
        /// <returns></returns>
        public List<Dept_Course> GetCourseTogetherList(out int totalCount, int currentDeptId, string where = " 1 = 1 ", int startIndex = 0,
                                     int pageLength = int.MaxValue, string orderBy = "ORDER BY Co_Course.Id DESC")
        {
            return deptDb.GetCourseTogetherList(out totalCount, currentDeptId, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 向表格DepTran_CourseOpen中插入开放的信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="deptId">用户部门ID</param>
        /// <returns></returns>
        public bool IsOrderInsert(string courseId, int deptId,int num)
        {
            return deptDb.IsOrderInsert(courseId, deptId,num);
        }

        /// <summary>
        /// 判断表格DepTran_CourseOpen中是否有该课程的开放信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="deptId">用户部门ID</param>
        /// <returns></returns>
        public bool IsOrderExist(string courseId, int deptId)
        {
            return deptDb.IsOrderExist(courseId, deptId);
        }

        /// <summary>
        /// 向表格DepTran_CourseOpen中更新不开放的信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="deptId">用户部门ID</param>
        /// <returns></returns>
        public bool DeleteOrder(string courseId, int deptId)
        {
            return deptDb.DeleteOrder(courseId, deptId);
        }

        /// <summary>
        /// 向表格DepTran_CourseOpen中更新开放的信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="deptId">用户部门ID</param>
        /// <returns></returns>
        public bool UpdataOrder(string courseId, int deptId)
        {
            return deptDb.UpdataOrder(courseId, deptId);
        }

        /// <summary>
        /// 向表格DepTran_CourseOpen中更新开放的信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="deptId">用户部门ID</param>
        /// <param name="num">报名人数</param>
        /// <returns></returns>
        public bool UpdataOrder(string courseId, int deptId, int num)
        {
            return deptDb.UpdataOrder(courseId, deptId, num);
        }

        /// <summary>
        /// 向表格DepTran_CourseOpen中插入不开放的信息
        /// </summary>
        /// <param name="courseId">课程ID</param>
        /// <param name="deptId">用户部门ID</param>
        /// <param name="num">允许人数 </param>
        /// <returns></returns>
        public bool IsDeleteOrder(string courseId, int deptId,int num)
        {
            return deptDb.IsDeleteOrder(courseId, deptId,num);
        }

        /// <summary>
        /// 获得DepTran_DepartUser表格中用户的部门信息
        /// </summary>
        /// <param name="userId">用户的ID</param>
        /// <returns></returns>
        public Dept_Course GetDeptId(int userId)
        {
            return deptDb.GetDeptId(userId);
        }

        /// <summary>
        /// 转播课程管理（最高权限的人）
        /// </summary>
        /// <returns></returns>
        public List<Dept_Course> GetDeptAllCourseManage(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = "  StartTime desc")
        {
            var list = deptDb.GetDeptAllCourseManage(startIndex, startLength, where, jsRenderSortField);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

    }
}
