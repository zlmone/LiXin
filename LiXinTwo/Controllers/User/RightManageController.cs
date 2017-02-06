using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinLanguage;
using LiXinModels.User;
using LiXinControllers.Filter;

namespace LiXinControllers.User
{
    public partial class UserManageController
    {
        private List<Sys_Right> roleRights = new List<Sys_Right>();

        [SystemLog("权限管理", LogLevel.Info)]
        public ActionResult RightManage()
        {
            ViewBag.treeString = GetRightTreeString();
            return View();
        }

        private string GetRightTreeString()
        {
            var treeStr = new StringBuilder("<table class=\"tab-List treeTable\" id=\"rightTreeTable\">");
            treeStr.AppendFormat(
                @"<thead><tr><th style='text-align:left;'>{0}</th><th class='Raster_28' style='text-align:left;'>{1}</th><th class='Raster_12' style='text-align:left;'>{2}</th><th class='tc Raster_6'>{3}</th><th class='tc Raster_6'>{4}</th><th class='tc Raster_6'>{5}</th></tr></thead><tbody>", "权限名称", "路径", "描述", "显示序号", "所属模块","操作");
            AppendTreeRow(0, treeStr, 0);
            treeStr.AppendFormat("</tbody></table>");
            return treeStr.ToString();
        }

        private void AppendTreeRow(int parentId, StringBuilder treeStr, int level)
        {
            string className = string.Empty;

            if (level > 0)
            {
                className = string.Format("class=\"child-of-note-{0}\"", GetParents(parentId));
            }
            level++;
            IOrderedEnumerable<Sys_Right> list = AllRights.Where(p => p.ParentId == parentId).OrderBy(p => p.ShowOrder);
            foreach (Sys_Right right in list)
            {
                string rightname = CodeHelper.GetNavigateString(right.RightName);
                //var rightname = right.RightName;
                treeStr.AppendLine();
                treeStr.AppendFormat("<tr id=\"note-{0}\" {1}>", GetParents(right.RightId), className);
                treeStr.AppendLine();
                treeStr.AppendFormat(
                    "<td style=\"padding-left:{5}px;\" class='c33' >{0}</td><td style='text-align:left;'>{1}</td><td style='text-align:left;'>{2}</td><td class='tc'>{3}</td><td class='tl'>{6}</td><td>{4}</td></tr>",
                    string.IsNullOrWhiteSpace(rightname) ? right.RightName.HtmlXssEncode() : rightname,
                    right.Path.HtmlXssEncode(),
                    right.RightDesc.HtmlXssEncode(),
                    right.ShowOrder,
                    right.Path == "Home/Index" ? "" : string.Format("<a onclick=\"Update({0})\" class=\"icon iedit\" title='编辑'>编辑</a><a title='删除' onclick=\"Delete({0})\" class=\"icon idel\">删除</a>", right.RightId),
                    19 * level,
                    right.ModuleName);
                AppendTreeRow(right.RightId, treeStr, level);
            }
        }

        private string GetParents(int rightId)
        {
            var list = new List<int> { rightId };
            Sys_Right right = AllRights.Find(p => p.RightId == rightId);

            while (right.ParentId != 0)
            {
                int parentId = right.ParentId;
                list.Add(right.ParentId);
                right = AllRights.Find(p => p.RightId == parentId);
            }
            string result = string.Empty;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                result += list[i] + "-";
            }
            if (result.EndsWith("-"))
                result = result.Substring(0, result.LastIndexOf('-'));
            return result;
        }

        public JsonResult RefreshCacheRight()
        {
            RefreshRightCache();
            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRightTable()
        {
            return Json(new
                {
                    tree = GetRightTreeString()
                }, JsonRequestBehavior.AllowGet);
        }

        [SystemLog("编辑权限", LogLevel.Info)]
        public ActionResult RightEdit(int id = 0)
        {
            Sys_Right right = AllRights.Find(p => p.RightId == id) ?? new Sys_Right
                {
                    RightId = 0,
                    Path = "",
                    RightDesc = "",
                    RightName = ""
                };

            ViewData["allrights"] = AllRights;
            return View(right);
        }

        [SystemLog("删除权限", LogLevel.Info)]
        public JsonResult DeleteRight(int id)
        {
            try
            {
                DeleteRightChild(id);
                lock (lockobj)
                {
                    RefreshRightCache();
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private void DeleteRightChild(int rightId)
        {
            rightBL.Delete(rightId);
            AllRights.Remove(AllRights.Find(p => p.RightId == rightId));
            if (AllRights.FindAll(p => p.ParentId == rightId).Count > 0)
            {
                foreach (Sys_Right item in AllRights.FindAll(p => p.ParentId == rightId))
                {
                    DeleteRightChild(item.RightId);
                }
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult SaveRight(int rightID, string rightName, string rightDesc, int rightType, string rightPath,
                                    int parentRight, int? showOrder, string moduleName)
        {
            try
            {
                if (rightBL.Exists(rightName, rightID))
                {
                    return Json(new
                        {
                            result = 0,
                            content = "权限名称重复！"
                        }, JsonRequestBehavior.AllowGet);
                }

                var right = AllRights.Find(p => p.RightId == rightID);

                if (right == null)
                {
                    //新增
                    right = new Sys_Right
                        {
                            RightName = rightName,
                            RightDesc = rightDesc,
                            RightType = rightType,
                            Path = rightPath,
                            ParentId = parentRight,
                            ShowOrder = showOrder,
                            ModuleName = moduleName
                        };
                    rightBL.Add(right);
                    lock (lockobj)
                    {
                        AllRights.Add(right);
                    }
                }
                else
                {
                    right.RightName = rightName;
                    right.RightDesc = rightDesc;
                    right.RightType = rightType;
                    right.Path = rightPath;
                    right.ShowOrder = showOrder;
                    right.ModuleName = moduleName;

                    //修改前的判断
                    if (right.RightId == parentRight)
                    {
                        return Json(new
                            {
                                result = 0,
                                content = "上级权限不能为本身！"
                            }, JsonRequestBehavior.AllowGet);
                    }
                    var childs = new List<int>();

                    GetChildRights(right.RightId, childs);

                    if (childs.IndexOf(parentRight) >= 0)
                    {
                        return Json(new
                            {
                                result = 0,
                                content = "上级权限也不能为本身的子权限！"
                            }, JsonRequestBehavior.AllowGet);
                    }

                    right.ParentId = parentRight;
                    rightBL.Update(right);
                    lock (lockobj)
                    {
                        AllRights.Remove(AllRights.Find(p => p.RightId == rightID));
                        AllRights.Add(right);
                    }
                }
                return Json(new
                    {
                        result = 1,
                        content = "保存成功"
                    }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        private void GetChildRights(int parentId, List<int> list)
        {
            foreach (Sys_Right allRight in AllRights.Where(p => p.ParentId == parentId))
            {
                list.Add(allRight.RightId);
                GetChildRights(allRight.RightId, list);
            }
        }

        private void GetChildRights(int parentId, List<Sys_Right> list)
        {
            foreach (Sys_Right allRight in AllRights.Where(p => p.ParentId == parentId))
            {
                list.Add(allRight);
                GetChildRights(allRight.RightId, list);
            }
        }

        public ActionResult AssignRight(int roleId, string type = "checkbox",
                                        string clickFunction = "rightNodeClick(this);", int buttonShow = 1)
        {
            ViewBag.roleId = roleId;
            roleRights = rightBL.GetRightByRoleId(roleId);
            ViewBag.html = new MvcHtmlString(GetRightTreeStr(type, clickFunction).ToString());
            ViewBag.buttonShow = buttonShow;
            return View();
        }

        public JsonResult GetRightTree(int roleId, string type = "checkbox",
                                       string clickFunction = "rightNodeClick(this);")
        {
            roleRights = rightBL.GetRightByRoleId(roleId);
            string html = GetRightTreeStr(type, clickFunction).ToString();
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public StringBuilder GetRightTreeStr(string type, string clickFunction)
        {
            string prefix = "";
            if (type == "checkbox")
                prefix = "<input type='checkbox' class='fll' id='{0}' value='{4}' pid='{6}' {5}/>";
            var treeStr = new StringBuilder("<ul id=\"navigation\" class=\"treeview\">");
            GetRightTreeChild(AllRights.FindAll(p => p.ParentId == 0), treeStr, clickFunction, prefix);
            treeStr.AppendLine("</ul>");
            return treeStr;
        }

        private void GetRightTreeChild(List<Sys_Right> rightList, StringBuilder html,
                                       string clickFunction = "rightNodeClick(this);", string prefix = "")
        {
            if (rightList.Count > 0)
            {
                foreach (Sys_Right v in rightList)
                {
                    string rightname = CodeHelper.GetNavigateString(v.RightName);
                    html.AppendFormat(
                        "<li class='treeChild'>" + prefix +
                        "<a title='{2}' style='cursor:pointer;' class='pNote' id='{1}' {3} >{2}<span>{7}</span></a>",
                        v.RightId,
                        v.RightId + "_" + v.ParentId,
                        string.IsNullOrWhiteSpace(rightname) ? v.RightName : rightname,
                        (string.IsNullOrWhiteSpace(clickFunction) ? "" : "onclick=\"" + clickFunction + "\""),
                        v.RightId + "_" + v.ParentId + "_" + (string.IsNullOrWhiteSpace(rightname) ? v.RightName : rightname),
                        (roleRights.Find(p => p.RightId == v.RightId) == null ? "" : "checked='checked'"),
                        v.ParentId,
                        GetModuleMemo(v.ModuleName)
                        );
                    if (AllRights.Exists(p => p.ParentId == v.RightId))
                    {
                        html.AppendLine("<ul>");
                        GetRightTreeChild(AllRights.FindAll(p => p.ParentId == v.RightId), html, clickFunction, prefix);
                        html.AppendLine("</ul>");
                    }
                    html.AppendLine("</li>");
                }
            }
        }

        /// <summary>
        /// 获取模块的描述
        /// </summary>
        /// <param name="mname"></param>
        /// <returns></returns>
        private string GetModuleMemo(string mname)
        {
            var modulememo = "";
            switch (mname)
            {
                case "MyTrain":
                    modulememo = "模块：总所学员端功能";
                    break;
                case "NewMyTrain":
                    modulememo = "模块：新进员工端功能";
                    break;
                case "MyDepTrain":
                    modulememo = "模块：部门/分所学员端功能";
                    break;
                case "DepManage":
                    modulememo = "模块：部门/分所管理端功能";
                    break;
                case "TrainManage":
                    modulememo = "模块：总所培训管理端功能";
                    break;
                case "Knowledge":
                    modulememo = "模块：知识中心";
                    break;
                case "SystemManage":
                    modulememo = "模块：系统管理";
                    break;
                case "Announcement":
                    modulememo = "模块：公告查看";
                    break;
                case "FAQ":
                    modulememo = "模块：政策查看";
                    break;
                case "Person":
                    modulememo = "模块：个人中心";
                    break;
                default:
                    modulememo = "";
                    break;
            }
            return modulememo;
        }
    }
}