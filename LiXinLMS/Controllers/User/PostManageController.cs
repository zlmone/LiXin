using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LiXinLanguage;
using LiXinModels.User;
using LiXinCommon;

namespace LiXinControllers.User
{
    public partial class UserManageController
    {
        public ActionResult PostManage()
        {
            return View();
        }

        public ActionResult PostEdit(int id = 0, int parentId = 0)
        {
            string childs = "";
            Sys_Post model = AllPosts.Find(p => p.PostId == id);
            if (model == null)
                model = new Sys_Post {PostId = id, ParentId = parentId, PostCode = "", PostName = "", Remark = ""};
            else
            {
                var list = new List<int>();
                GetChildPostIds(model.PostId, list);
                childs = list.Aggregate("", (current, postId) => current + (postId + ","));
            }
            if (model.ParentId == 0)
                model.parentPostName = CommonLanguage.ParentPost;
            else
                model.parentPostName = AllPosts.Find(p => p.PostId == model.ParentId).PostNameShow;
            ViewBag.model = model;
            ViewBag.childs = childs;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult SavePost(int postId, string postCode, string postName, string postDesc, int postLevel,
                                   int parentId)
        {
            try
            {
                if (postBL.Exists(postCode, postName, postId))
                {
                    return Json(new {result = 0, content = "岗位代码或岗位名称重复！"}, JsonRequestBehavior.AllowGet);
                }

                Sys_Post model = AllPosts.Find(p => p.PostId == postId);
                if (model == null)
                    model = new Sys_Post();
                model.PostId = postId;
                model.PostCode = postCode;
                model.PostName = postName;
                model.Remark = postDesc;
                if (postLevel == -1)
                    model.PostLevel = null;
                else
                    model.PostLevel = postLevel;
                model.ParentId = parentId;
                if (model.PostId == 0)
                {
                    postBL.Add(model);
                    AllPosts.Add(model);
                }
                else
                {
                    postBL.Update(model);
                }
                lock (lockobj)
                {
                    RefreshPostCache();
                }
                return Json(new {result = 1, content = "保存成功"}, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new {result = 0, content = "保存失败"}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeletePost(int postId)
        {
            try
            {
                DeletePostChild(postId);
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

        private void DeletePostChild(int postId)
        {
            postBL.Delete(postId);
            AllPosts.Remove(AllPosts.Find(p => p.PostId == postId));
            if (AllPosts.FindAll(p => p.ParentId == postId).Count > 0)
            {
                foreach (Sys_Post item in AllPosts.FindAll(p => p.ParentId == postId))
                {
                    DeletePostChild(item.PostId);
                }
            }
        }

        public JsonResult AddUserToPost(string ids, int postId)
        {
            try
            {
                postBL.AddUserToPost(ids, postId);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteUserPost(string ids)
        {
            try
            {
                postBL.DeleteUserPost(ids);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ChangeUserPost(string ids, int postId)
        {
            try
            {
                postBL.AddUserToPost(ids, postId);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        private void GetChildPostIds(int parentId, List<int> list)
        {
            foreach (Sys_Post post in AllPosts.Where(p => p.ParentId == parentId))
            {
                list.Add(post.PostId);
                GetChildPostIds(post.PostId, list);
            }
        }
    }
}