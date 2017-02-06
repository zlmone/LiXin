using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Retech.Cache;
using LiXinCommon;
using LiXinInterface.User;
using LiXinModels.User;
using LiXinControllers;
using LiXinLanguage;
using LiXinControllers.Filter;

namespace LiXinControllers.User
{
    public partial class UserManageController
    {
        [SystemLog("部门管理", LogLevel.Info)]
        public ActionResult DeptManage()
        {
            return View();
        }

        [SystemLog("编辑部门", LogLevel.Info)]
        public ActionResult DeptEdit(int id = 0, int parentId = 0)
        {
            string childs = "";
            Sys_Department model = AllDepartments.Find(p => p.DepartmentId == id);
            if (model == null)
                model = new Sys_Department
                    {
                        DepartmentId = id,
                        ParentDeptId = parentId,
                        DeptCode = "",
                        DeptName = "",
                        Remark = ""
                    };
            else
            {
                var list = new List<int>();
                GetChildDeptIds(model.DepartmentId, list);
                childs = list.Aggregate("", (current, deptId) => current + (deptId + ","));
            }
            if (model.ParentDeptId == 0)
                model.parentDeptName = CommonLanguage.ParentDept;
            else
                model.parentDeptName = AllDepartments.Find(p => p.DepartmentId == model.ParentDeptId).DeptNameShow;
            ViewBag.model = model;
            ViewBag.childs = childs;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult SaveDept(int deptId, string deptCode, string deptName, string deptDesc, int parentId)
        {
            try
            {
                if (deptBL.Exists(deptCode, deptName, deptId))
                {
                    return Json(new { result = 0, content = "部门代码或部门名称重复！" }, JsonRequestBehavior.AllowGet);
                }

                Sys_Department model = AllDepartments.Find(p => p.DepartmentId == deptId);
                if (model == null)
                    model = new Sys_Department();
                model.DepartmentId = deptId;
                model.DeptCode = deptCode;
                model.DeptName = deptName;
                model.Remark = deptDesc;
                model.ParentDeptId = parentId;

                if (model.DepartmentId == 0)
                {
                    deptBL.Add(model);
                    AllDepartments.Add(model);
                }
                else
                {
                    deptBL.Update(model);
                }
                lock (lockobj)
                {
                    RefreshDepartmentCache();
                }
                return Json(new { result = 1, content = "保存成功" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, content = "保存失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        [SystemLog("删除部门", LogLevel.Info)]
        public JsonResult DeleteDept(int deptId)
        {
            try
            {
                DeleteDeptChild(deptId);
                lock (lockobj)
                {
                    RefreshDepartmentCache();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private void DeleteDeptChild(int deptId)
        {
            deptBL.Delete(deptId);
            AllDepartments.Remove(AllDepartments.Find(p => p.DepartmentId == deptId));
            if (AllDepartments.FindAll(p => p.ParentDeptId == deptId).Count > 0)
            {
                foreach (Sys_Department item in AllDepartments.FindAll(p => p.ParentDeptId == deptId))
                {
                    DeleteDeptChild(item.DepartmentId);
                }
            }
        }

        public JsonResult AddUserToDept(string ids, int deptId)
        {
            try
            {
                deptBL.AddUserToDept(ids, deptId);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteUserDept(string ids)
        {
            try
            {
                deptBL.DeleteUserDept(ids);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ChangeUserDept(string ids, int deptId)
        {
            try
            {
                deptBL.AddUserToDept(ids, deptId);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private void GetChildDeptIds(int parentId, List<int> list)
        {
            foreach (Sys_Department dept in AllDepartments.Where(p => p.ParentDeptId == parentId))
            {
                list.Add(dept.DepartmentId);
                GetChildDeptIds(dept.DepartmentId, list);
            }
        }
    }
}