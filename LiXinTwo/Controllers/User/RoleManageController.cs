using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LiXinCommon;
using LiXinLanguage;
using LiXinModels.User;
using System;
using LiXinControllers.Filter;

namespace LiXinControllers.User
{
    public partial class UserManageController
    {
        [SystemLog("角色管理", LogLevel.Info)]
        public ActionResult RoleManage()
        {
            return View();
        }

        public ActionResult RoleUser(int id)
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.id = id;
            return View();
        }

        public JsonResult GetRoleList(string roleName, int status, int pageSize = int.MaxValue, int pageIndex = 1, string jsRenderSortField = "Sys_Roles.RoleId desc")
        {
            int totalCount = 0;
            List<Sys_Roles> list = roleBL.GetList(out totalCount, roleName.Trim().ReplaceSql(), status,
                                                  (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);

            int n = 0;
            var itemArray = new object[list.Count()];
            foreach (var item in list)
            {
                var temp = new
                {
                    RoleId = item.RoleId,
                    IsDefault = item.IsDefault,
                    RoleName = item.RoleName.HtmlXssEncode(),
                    RoleDesc = item.RoleDesc.HtmlXssEncode(),
                    Status = item.Status == 1 ? "是" : "否",
                    Realname = item.Realname,
                    CreateTimeStr = item.CreateTimeStr,
                    UserCount = item.UserCount
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        [SystemLog("编辑角色", LogLevel.Info)]
        public ActionResult RoleEdit(int id = 0)
        {
            var model = new Sys_Roles { RoleId = 0, RoleDesc = "", RoleName = "" };
            if (id != 0)
                model = roleBL.Get(id);
            ViewBag.model = model;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult SaveRole(int roleId, int isDefault, string roleName, string roleDesc)
        {
            try
            {
                if (roleBL.Exists(roleName.Trim(), roleId))
                {
                    return Json(new { result = 0, content = "角色名称重复！" }, JsonRequestBehavior.AllowGet);
                }

                var model = new Sys_Roles { Status = 0, CreateTime = DateTime.Now };
                if (roleId != 0)
                    model = roleBL.Get(roleId);
                if (model.Status == 1 && isDefault == 1)
                {
                    return Json(new { result = 0, content = "保存失败！已冻结角色不可设为默认角色！" },
                                JsonRequestBehavior.AllowGet);
                }
                if (roleBL.GetAllRoles().Find(p => p.IsDefault == 1 && p.IsDelete == 0) == null)
                    isDefault = 1;

                model.IsDefault = isDefault;
                model.RoleName = roleName.Trim();
                model.RoleDesc = roleDesc.Trim();
                if (model.RoleId == 0)
                {
                    model.Creater = CurrentUser.UserId;
                    roleBL.Add(model);
                }
                else
                    roleBL.Update(model);
                return Json(new { result = 1, content = "保存成功" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, content = "保存失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        [SystemLog("删除角色", LogLevel.Info)]
        public JsonResult DeleteRole(string roleIds)
        {
            try
            {
                roleBL.Delete(roleIds);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ChangeDefault(int id)
        {
            try
            {
                Sys_Roles model = roleBL.Get(id);
                if (model.Status == 1)
                    return Json(2, JsonRequestBehavior.AllowGet);
                var list = rightBL.GetRightByRoleId(id);
                if (list.Count == 0)
                    return Json(3, JsonRequestBehavior.AllowGet);
                model.IsDefault = 1;
                roleBL.Update(model);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateStatus(int id)
        {
            try
            {
                Sys_Roles model = roleBL.Get(id);
                model.Status = (model.Status + 1) % 2;
                roleBL.Update(model);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveRoleRight(int roleId, string rights)
        {
            try
            {
                roleBL.AddRightToRole(roleId, rights);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AssignRole(int id = 0)
        {
            string roles = "";
            if (id != 0)
                roles = roleBL.GetListByUserId(id)
                              .Select(p => p.RoleId)
                              .Aggregate(",", (current, roleId) => current + (roleId + ","));
            ViewBag.roles = roles;
            return View();
        }
    }
}