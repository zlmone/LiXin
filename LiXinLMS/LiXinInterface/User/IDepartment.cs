using System.Collections.Generic;
using LiXinModels.User;

namespace LiXinInterface.User
{
    public interface IDepartment
    {
        List<Sys_Department> GetAllList(string strWhere = " 1 = 1 ");

        /// <summary>
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        Sys_Department Get(int deptId);

        /// <summary>
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        bool Update(Sys_Department department);

        /// <summary>
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        bool DeleteParentId(int parentId);

        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(Sys_Department model);

        /// <summary>
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        bool Delete(int deptId);

        /// <summary>
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <param name="DeptName"></param>
        /// <param name="DeptId"></param>
        /// <returns></returns>
        bool Exists(string DeptCode, string DeptName, int DeptId);

        /// <summary>
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="parentId"></param>
        void ChangeDeptParentId(int deptId, int parentId);

        /// <summary>
        /// </summary>
        /// <param name="DeptName"></param>
        /// <returns></returns>
        List<Sys_Department> GetListByDeptName(string DeptName);

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="deptId"></param>
        void AddUserToDept(string userIds, int deptId);

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        void DeleteUserDept(string userIds);

        /// <summary>
        ///     根据部门上一级ID查询部门List
        /// </summary>
        /// <param name="parentDeptId">部门ID</param>
        /// <returns>部门List</returns>
        List<Sys_Department> GetDepartmentsByParentDeptID(int parentDeptId);
    }
}