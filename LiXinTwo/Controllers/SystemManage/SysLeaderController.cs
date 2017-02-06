using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinControllers.Filter;
using LiXinModels.SystemManage;
using LiXinModels.User;
using LiXinCommon;
using System.Text.RegularExpressions;

namespace LiXinControllers
{
    public partial class SystemManageController : BaseController
    {
        #region 页面
        public ActionResult LeaderUserIndex()
        {
            return View();
        }

        public ActionResult LeaderUserEdit(int id)
        {
            var sort = id > 0 ? learderBL.GetModel(id) : new Sys_LeaderConfig();
            ViewBag.TrainGrade = AllTrainGrade;
            return View(sort);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 指定上级管理页面
        /// </summary>
        public JsonResult GetAllLearderList(string title, string username, int pageSize = 20, int pageIndex = 1)
        {
            try
            {
                int totalCount = 0;
                var where = string.Format(@" t1.Realname LIKE '%{0}%' AND t0.GroupName LIKE '%{1}%'", username.ReplaceSql(), title.ReplaceSql());
                var groupList = learderBL.GetAllList(out totalCount, pageIndex, pageSize, where);
                foreach (var item in groupList)
                {
                    item.GroupName = item.GroupName.HtmlXssEncode();
                    item.Realname = item.Realname.HtmlXssEncode();
                }
                return Json(new
                {
                    dataList = groupList,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<Sys_LeaderConfig>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 指定上级管理添加
        /// </summary>
        public JsonResult GetAllLeaderUserList(int id, int pageSize = 20, int pageIndex = 1)
        {
            try
            {
                int totalCount = 0;
                List<Sys_GroupUser> gUserList = learderBL.GroupUserList(id, out totalCount, pageIndex, pageSize);
                foreach (var item in gUserList)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.DeptName = item.DeptName.HtmlXssEncode();
                }
                return Json(new
                {
                    dataList = gUserList,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取选择用户信息
        /// </summary>
        public JsonResult GetleaderMessage(string userIDs, int pageSize = 20, int pageIndex = 1)
        {

            try
            {
                int totalCount = 0;
                List<Sys_GroupUser> UserList = learderBL.GetUser(userIDs, out totalCount, pageIndex, pageSize);
                foreach (var item in UserList)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
                }
                return Json(new
                {
                    dataList = UserList,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Filter.SystemLog("上级指定管理  上级指定编辑", LogLevel.Info)]
        public JsonResult Submitleader(int id)
        {
            var txtSortName = Request.Form["txtSortName"];
            var txtMemo = Request.Form["txtMemo"];
            var leaderID = Request.Form["leaderID"].StringToInt32();
            var AllUserID = Request.Form["haveUserID"];
            var re = groupBL.Checkname(txtSortName, id);
            if (re)
            {
                if (id == 0)
                {
                    //添加
                    int newid = learderBL.Insert(new Sys_LeaderConfig
                    {
                        GroupName = txtSortName,
                        UserId = leaderID,
                        Memo = txtMemo,
                        CreateTime = DateTime.Now,
                        LastUpdateTime = DateTime.Now,
                        IsDelete = 0
                    });
                    if (newid > 0)
                    {
                        var qun = learderBL.AddGroupUser(newid, AllUserID);
                        if (qun)
                            return Json(new
                            {
                                result = 1,
                                content = "保存成功"
                            }, JsonRequestBehavior.AllowGet);
                        else
                            return Json(new
                            {
                                result = 0,
                                content = "保存失败"
                            }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            result = 0,
                            content = "保存失败"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //修改
                    var sort = learderBL.GetModel(id);
                    sort.GroupName = txtSortName;
                    sort.Memo = txtMemo;
                    sort.LastUpdateTime = DateTime.Now;
                    sort.UserId = leaderID;
                    bool newid = learderBL.UpdateByID(sort);
                    if (newid)
                    {
                        learderBL.DeleteGroupAllUser(id);
                        var qun = learderBL.AddGroupUser(id, AllUserID);
                        if (qun)
                            return Json(new
                            {
                                result = 1,
                                content = "保存成功"
                            }, JsonRequestBehavior.AllowGet);
                        else
                            return Json(new
                            {
                                result = 0,
                                content = "保存失败"
                            }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            result = 0,
                            content = "保存失败"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json(new
                {
                    result = 0,
                    content = "保存失败，该群组已存在"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 检查重命名
        /// </summary>
        /// <param name="title"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CheckLeader(string title, int id)
        {
            var isValidate = learderBL.Checkname(title, id);
            return Json(isValidate, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有的用户用户（根据群组ID）
        /// </summary>
        public JsonResult GetAllUserByleader()
        {
            List<int> sulist = learderBL.GroupUserID(0);
            string alluser = string.Join(",", sulist);
            return Json(new { content = alluser }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取此负责人下面所有的用户
        /// </summary>
        public JsonResult GetUserByleader(int Id)
        {
            List<int> sulist = learderBL.GroupUserID(Id);
            string alluser = string.Join(",",sulist);
            
            return Json(new { content = alluser }, JsonRequestBehavior.AllowGet);
        }

        [Filter.SystemLog("上级指定管理 删除", LogLevel.Info)]
        public JsonResult Deleteleader(string ids)
        {
            bool result = learderBL.DeleteModel(ids);
            foreach (var str in ids.Split(','))
            {
                learderBL.DeleteGroupAllUser(str.StringToInt32());
            }
            if (result)
                return Json(new
                {
                    result = 1,
                    content = "删除成功"
                }, JsonRequestBehavior.AllowGet);
            else
                return Json(new
                {
                    result = 0,
                    content = "删除失败"
                }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
