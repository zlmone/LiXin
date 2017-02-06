using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinInterface.User;
using LiXinModels.User;
using LiXinLanguage;

namespace LiXinControllers.User
{
    public class UserCommonController : BaseController
    {
        protected IDepartment deptBL;
        protected IPost postBL;
        protected IRight rightBL;
        protected IRole roleBL;
        protected IUser userBL;

        private List<int> searchDeptIds = new List<int>();

        private List<Sys_Department> SearchDepartments = new List<Sys_Department>();


        public UserCommonController(IUser _userBL, IRole _roleBL, IPost _postBL, IRight _rightBL, IDepartment _deptBL)
        {
            userBL = _userBL;
            roleBL = _roleBL;
            postBL = _postBL;
            deptBL = _deptBL;
            rightBL = _rightBL;

        }

        public ActionResult SelectUser(string type = "checkbox", int deptId = 0, int postId = 0, string model = "",
                                       int NotShowHasPost = 0, int NotShowHasDept = 0, int pageSize = 10)
        {
            ViewBag.type = type;
            ViewBag.pageSize = pageSize;
            ViewBag.deptId = deptId;
            ViewBag.postId = postId;
            ViewBag.model = model;
            ViewBag.NotShowHasDept = NotShowHasDept;
            ViewBag.NotShowHasPost = NotShowHasPost;

            ViewBag.TrainGrade = AllTrainGrade;
            return View();
        }


        public ActionResult SelectUserByDept(string type = "checkbox", int pageSize = 10)
        {
            ViewBag.type = type;
            ViewBag.pageSize = pageSize;
            ViewBag.TrainGrade = AllTrainGrade;
            return View();
        }

        public ActionResult SelectUserByPost(string type = "checkbox")
        {
            ViewBag.type = type;
            return View();
        }

        public ActionResult DeptTree(int flag = 0, string type = "", string clickFunction = "deptNodeClick(this)",
                                     int buttonShow = 0)
        {
            //System.Diagnostics.Stopwatch stop = new System.Diagnostics.Stopwatch();
            //stop.Start();
            ViewBag.html = new MvcHtmlString(GetDeptTreeStr(flag, type, clickFunction).ToString());
            ViewBag.flag = flag;
            ViewBag.type = type;
            ViewBag.clickFunction = clickFunction;
            ViewBag.buttonShow = buttonShow;
            //stop.Stop();
            //var a = stop.ElapsedMilliseconds;
            return View();
        }


        public ActionResult SelectTeacher(string type = "radio")
        {
            ViewBag.type = type;
            return View();
        }

        public JsonResult GetDeptTree(int flag = 0, string type = "", string clickFunction = "deptNodeClick(this)", string name = "")
        {
            //string html = GetDeptTreeStr(flag, type, clickFunction).ToString();
            string html = GetDeptTreeSearchStr(flag, type, clickFunction, name).ToString();
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public StringBuilder GetDeptTreeStr(int flag, string type, string clickFunction)
        {
            string prefix = "";
            if (type == "checkbox")
                prefix = "<input type='checkbox' class='fll' id='{0}' value='{4}' />";
            else if (type == "radio")
                prefix = "<input type='radio' class='fll' id='{0}' value='{4}' name='prd'/>";
            var treeStr = new StringBuilder();
            if (flag == 0)
                treeStr.AppendFormat(
                    "<ul id=\"navigation\" class=\"treeview w700\"><li id=\"0\"><span title='{2}' id='{1}' class=\"rootNote\">{2}</span><ul>",
                    0, "0_-1", CommonLanguage.ParentDept, "", "0_-1_" + CommonLanguage.ParentDept);
            else
                treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview w700\">");
            GetDeptTreeChild(AllDepartments.FindAll(p => p.ParentDeptId == 0), treeStr, clickFunction, prefix);
            if (flag == 0)
                treeStr.AppendLine("</ul></li></ul>");
            else
                treeStr.AppendLine("</ul>");
            return treeStr;
        }

        private void GetDeptTreeChild(List<Sys_Department> deptList, StringBuilder html,
                                      string clickFunction = "deptNodeClick(this)", string prefix = "")
        {
            if (deptList.Count > 0)
            {
                foreach (Sys_Department v in deptList)
                {
                    //string dept = v.DeptCode + "[" + v.DeptName + "]";
                    string dept = v.DeptName;
                    html.AppendFormat(
                        "<li class='pNote'>" + prefix +
                        "<a title='{2}' style='cursor:pointer;' id='{1}' {3} >{2}</a>", v.DepartmentId,
                        v.DepartmentId + "_" + v.ParentDeptId, dept,
                        (string.IsNullOrWhiteSpace(clickFunction) ? "" : "onclick=\"" + clickFunction + "\""),
                        v.DepartmentId + "_" + v.ParentDeptId + "_" + dept);
                    if (AllDepartments.Exists(p => p.ParentDeptId == v.DepartmentId))
                    {
                        html.AppendLine("<ul>");
                        GetDeptTreeChild(AllDepartments.FindAll(p => p.ParentDeptId == v.DepartmentId), html,
                                         clickFunction, prefix);
                        html.AppendLine("</ul>");
                    }
                    html.AppendLine("</li>");
                }
            }
        }

        public ActionResult PostTree(int flag = 0, string type = "", string clickFunction = "postNodeClick(this)",
                                     int buttonShow = 0)
        {
            ViewBag.html = new MvcHtmlString(GetPostTreeStr(flag, type, clickFunction).ToString());
            ViewBag.buttonShow = buttonShow;
            return View();
        }

        public JsonResult GetPostTree(int flag = 0, string type = "", string clickFunction = "postNodeClick(this)")
        {
            return Json(GetPostTreeStr(flag, type, clickFunction).ToString(), JsonRequestBehavior.AllowGet);
        }

        public StringBuilder GetPostTreeStr(int flag, string type, string clickFunction)
        {
            string prefix = "";
            if (type == "checkbox")
                prefix = "<input type='checkbox' class='fll' id='{0}' value='{4}' />";
            else if (type == "radio")
                prefix = "<input type='radio' class='fll' id='{0}' value='{4}' name='prd'/>";
            var treeStr = new StringBuilder();
            if (flag == 0)
                treeStr.AppendFormat(
                    "<ul id=\"navigation\" class=\"treeview\"><li id=\"0\"><span title='{2}' id='{1}' class=\"rootNote\">{2}</span><ul>",
                    0, "0_-1", CommonLanguage.ParentPost, "", "0_-1_" + CommonLanguage.ParentPost);
            else
                treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview\">");
            GetPostTreeChild(AllPosts.FindAll(p => p.ParentId == 0), treeStr, clickFunction, prefix);
            if (flag == 0)
                treeStr.AppendLine("</ul></li></ul>");
            else
                treeStr.AppendLine("</ul>");
            return treeStr;
        }

        private void GetPostTreeChild(List<Sys_Post> postList, StringBuilder html,
                                      string clickFunction = "postNodeClick(this)", string prefix = "")
        {
            if (postList.Count > 0)
            {
                foreach (Sys_Post v in postList)
                {
                    //string post = v.PostCode + "[" + v.PostName + "]";
                    string post = v.PostName;
                    html.AppendFormat(
                        "<li class='treeChild'>" + prefix +
                        "<a title='{2}' style='cursor:pointer;' class='pNote' id='{1}' {3} >{2}</a>", v.PostId,
                        v.PostId + "_" + v.ParentId, post,
                        (string.IsNullOrWhiteSpace(clickFunction) ? "" : "onclick=\"" + clickFunction + "\""),
                        v.PostId + "_" + v.ParentId + "_" + post);
                    if (AllPosts.Exists(p => p.ParentId == v.PostId))
                    {
                        html.AppendLine("<ul>");
                        GetPostTreeChild(AllPosts.FindAll(p => p.ParentId == v.PostId), html, clickFunction, prefix);
                        html.AppendLine("</ul>");
                    }
                    html.AppendLine("</li>");
                }
            }
        }

        public JsonResult GetUserIds(int deptId = 0, int postId = 0)
        {
            int totalCount = 0;
            string userIds = "0";
            var list = userBL.GetList(out totalCount, deptId.ToString(), postId.ToString(), "", "", "", "", "", 99, 99);
            if (list.Count > 0)
            {
                IEnumerable<int> listIds = list.Select(p => p.UserId);
                userIds = listIds.Aggregate("", (current, id) => current + (id + ","));
                userIds = userIds.TrimEnd(',');
            }
            return Json(userIds, JsonRequestBehavior.AllowGet);
        }

        #region 查询组织结构

        public StringBuilder GetDeptTreeSearchStr(int flag, string type, string clickFunction, string name)
        {
            var treeStr = new StringBuilder();

            if (string.IsNullOrWhiteSpace(name))
            {
                searchDeptIds = AllDepartments.Select(p => p.DepartmentId).ToList();
                SearchDepartments = AllDepartments;
            }
            else
            {
                searchDeptIds = AllDepartments.Where(p => p.DeptName.Contains(name)).Select(p => p.DepartmentId).ToList();
                List<int> listIds = new List<int>();
                for (int i = 0; i < searchDeptIds.Count; i++)
                {
                    GetParentDeptId(searchDeptIds[i], listIds);
                }
                if (listIds.Count == 0)
                    return treeStr;
                SearchDepartments = AllDepartments.Where(p => listIds.Contains(p.DepartmentId)).ToList();
            }

            string prefix = "";
            if (type == "checkbox")
                prefix = "<input type='checkbox' class='fll'  id='{0}' value='{4}' />";
            else if (type == "radio")
                prefix = "<input type='radio' class='fll' id='{0}' value='{4}' name='prd'/>";

            if (flag == 0)
                treeStr.AppendFormat(
                    "<ul id=\"navigation\" class=\"treeview w700\"><li id=\"0\"><span title='{2}' id='{1}' class=\"rootNote\">{2}</span><ul>",
                    0, "0_-1", CommonLanguage.ParentDept, "", "0_-1_" + CommonLanguage.ParentDept);
            else
                treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview w700\">");
            GetDeptTreeSearchChild(SearchDepartments.FindAll(p => p.ParentDeptId == 0), treeStr, clickFunction, prefix);
            if (flag == 0)
                treeStr.AppendLine("</ul></li></ul>");
            else
                treeStr.AppendLine("</ul>");
            return treeStr;
        }

        private void GetParentDeptId(int deptId, List<int> list)
        {
            if (deptId > 0)
            {
                list.Add(deptId);
                var dept = AllDepartments.Find(p => p.DepartmentId == deptId);
                GetParentDeptId(dept.ParentDeptId, list);
            }
        }

        private void GetDeptTreeSearchChild(List<Sys_Department> deptList, StringBuilder html,
                                      string clickFunction = "deptNodeClick(this)", string prefix = "")
        {
            if (deptList.Count > 0)
            {
                foreach (Sys_Department v in deptList)
                {
                    html.AppendFormat(
                        "<li class='pNote'>" + prefix +
                        "<a title='{2}' id='{1}' {3} {5}>{2}</a>",
                        v.DepartmentId,
                        v.DepartmentId + "_" + v.ParentDeptId,
                        v.DeptName,
                        ((string.IsNullOrWhiteSpace(clickFunction) || (!searchDeptIds.Contains(v.DepartmentId))) ? "" : "onclick=\"" + clickFunction + "\""),
                        v.DepartmentId + "_" + v.ParentDeptId + "_" + v.DeptName,
                        searchDeptIds.Contains(v.DepartmentId) ? "style='cursor:pointer;'" : "style='cursor:default;'");
                    if (SearchDepartments.Exists(p => p.ParentDeptId == v.DepartmentId))
                    {
                        html.AppendLine("<ul>");
                        GetDeptTreeSearchChild(SearchDepartments.FindAll(p => p.ParentDeptId == v.DepartmentId), html,
                                         clickFunction, prefix);
                        html.AppendLine("</ul>");
                    }
                    html.AppendLine("</li>");
                }
            }
        }

        #endregion
    }
}