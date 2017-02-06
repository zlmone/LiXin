using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinControllers.Filter;
using LiXinInterface.DepTranManage;
using LiXinInterface.User;
using LiXinModels.DepTranManage;
using LiXinModels.SystemManage;
using LiXinModels.User;
using LiXinCommon;
using System.Text.RegularExpressions;

namespace LiXinControllers
{
    public class DepTranDepartSettingController : BaseController
    {
        protected IDepTran_DepartLeaderConfig DepartSetting;
        protected IUser UserBL;
        public DepTranDepartSettingController(IDepTran_DepartLeaderConfig departSetting, IUser userBL)
        {
            DepartSetting = departSetting;
            UserBL = userBL;
        }

        #region 页面
        public ActionResult DepartSettingManage()
        {
            return View();
        }

        public ActionResult DepartSettingEdit(int id)
        {
            var sort = new DepTranDepartSetting{DepartSetName="",MainUserIDs="",Realname="",Memo=""};
            var userList = new List<Sys_User>();
            if (id > 0)
            {
                sort = DepartSetting.GetModel(id);
                userList =
                    UserBL.GetList(string.Format(" UserId in ({0}) ", string.IsNullOrEmpty(sort.MainUserIDs) ? "0" : sort.MainUserIDs));
            }
            ViewBag.UserList = userList;
            ViewBag.TrainGrade = AllTrainGrade;
            return View(sort);
        }

        #endregion

        #region 方法
        /// <summary>
        /// 指定上级管理页面
        /// </summary>
        public JsonResult GetAllDepartSettingList(string title, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " CreateTime desc")
        {
            try
            {
                int totalCount;
                var where = string.Format(@" t0.DepartSetName LIKE '%{0}%'", title.ReplaceSql());
                var groupList = DepartSetting.GetAllList(out totalCount, pageIndex, pageSize, where, "order by " + jsRenderSortField);
                foreach (var item in groupList)
                {
                    item.DepartSetName = item.DepartSetName.HtmlXssEncode();
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
                    dataList = new List<DepTranDepartSetting>(),
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
                int totalCount;
                var gUserList = DepartSetting.DepartSettingUserList(id, out totalCount, pageIndex, pageSize);
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
                int totalCount;
                var userList = DepartSetting.GetUser(userIDs, out totalCount, pageIndex, pageSize);
                foreach (var item in userList)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
                }
                return Json(new
                {
                    dataList = userList,
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
        public JsonResult Submitleader(int id)
        {
            var txtSortName = Request.Form["txtSortName"];
            var txtMemo = Request.Form["txtMemo"];
            var leaderID = Request.Form["leaderID"];
            var allUserID = Request.Form["haveUserID"];
            var re = DepartSetting.Checkname(txtSortName, id);
            if (re)
            {
                if (id == 0)
                {
                    //添加
                    var newid = DepartSetting.Insert(new DepTranDepartSetting
                    {
                        DepartSetName = txtSortName,
                        MainUserIDs = leaderID,
                        Memo = txtMemo,
                        CreateTime = DateTime.Now,
                        IsDelete = 0
                    });
                    if (newid > 0)
                    {
                        var qun = DepartSetting.AddDepartSettingUser(newid, allUserID);
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
                    var sort = DepartSetting.GetModel(id);
                    sort.DepartSetName = txtSortName;
                    sort.Memo = txtMemo;
                    sort.MainUserIDs = leaderID;
                    var newid = DepartSetting.UpdateByID(sort);
                    if (newid)
                    {
                        DepartSetting.DeleteDepartSettingAllUser(id);
                        var qun = DepartSetting.AddDepartSettingUser(id, allUserID);
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
                    content = "保存失败，该部门群组已存在"
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
            var isValidate = DepartSetting.Checkname(title, id);
            return Json(isValidate, JsonRequestBehavior.AllowGet);
        }

        
        /// <summary>
        /// 获取此负责人下面所有的用户
        /// </summary>
        public JsonResult GetUserByleader(int Id)
        {
            List<int> sulist = DepartSetting.GetDepartSettingUserID(Id);
            string alluser = string.Join(",",sulist);
            
            return Json(new { content = alluser }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取
        /// </summary>
        public JsonResult GetAllUserByleader()
        {
            var sulist = DepartSetting.GetDepartSettingUserID(0);
            var alluser = string.Join(",", sulist);
            return Json(new { content = alluser }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult DeleteDepartSetting(string ids)
        {
            var result = DepartSetting.DeleteModel(ids);
            foreach (var str in ids.Split(','))
            {
                DepartSetting.DeleteDepartSettingAllUser(str.StringToInt32());
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
