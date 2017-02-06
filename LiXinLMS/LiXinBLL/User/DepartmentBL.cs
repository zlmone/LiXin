using System.Collections.Generic;
using LiXinDataAccess.User;
using LiXinInterface.User;
using LiXinModels.User;

namespace LiXinBLL.User
{
    public class DepartmentBL : IDepartment
    {
        private readonly DepartmentDB _deptDB = new DepartmentDB();

        /// <summary>
        ///     获取所有部门
        /// </summary>
        /// <returns></returns>
        public List<Sys_Department> GetAllList(string strWhere = " 1 = 1 ")
        {
            return _deptDB.GetList(strWhere);
        }

        /// <summary>
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public Sys_Department Get(int deptId)
        {
            return _deptDB.Get(deptId);
        }

        /// <summary>
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public bool Update(Sys_Department department)
        {
            return _deptDB.Update(department);
        }

        /// <summary>
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool DeleteParentId(int parentId)
        {
            return _deptDB.DeleteParentDeptId(parentId);
        }

        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(Sys_Department model)
        {
            _deptDB.Add(model);
            return model.DepartmentId;
        }

        /// <summary>
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public bool Delete(int deptId)
        {
            return _deptDB.Delete(deptId);
        }

        /// <summary>
        /// </summary>
        /// <param name="DeptCode"></param>
        /// <param name="DeptName"></param>
        /// <param name="DeptId"></param>
        /// <returns></returns>
        public bool Exists(string DeptCode, string DeptName, int DeptId)
        {
            return _deptDB.Exists(DeptCode, DeptName, DeptId);
        }

        /// <summary>
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="parentId"></param>
        public void ChangeDeptParentId(int deptId, int parentId)
        {
            Sys_Department model = _deptDB.Get(deptId);
            model.ParentDeptId = parentId;
            Update(model);
        }

        /// <summary>
        /// </summary>
        /// <param name="DeptName"></param>
        /// <returns></returns>
        public List<Sys_Department> GetListByDeptName(string DeptName)
        {
            string sqlwhere = "  DeptName like '%" + DeptName + "%' ";
            return _deptDB.GetList(sqlwhere);
        }

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="deptId"></param>
        public void AddUserToDept(string userIds, int deptId)
        {
            _deptDB.AddUserToDept(userIds, deptId);
        }

        /// <summary>
        /// </summary>
        /// <param name="userIds"></param>
        public void DeleteUserDept(string userIds)
        {
            _deptDB.DeleteUserDept(userIds);
        }

        /// <summary>
        ///     根据部门上一级ID查询部门List
        /// </summary>
        /// <param name="parentDeptId">部门ID</param>
        /// <returns>部门List</returns>
        public List<Sys_Department> GetDepartmentsByParentDeptID(int parentDeptId)
        {
            string sqlwhere = " and parentDeptId = " + parentDeptId;
            return _deptDB.GetList(sqlwhere);
        }
    }
}