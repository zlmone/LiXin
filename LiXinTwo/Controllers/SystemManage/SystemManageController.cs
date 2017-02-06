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
        #region 日志
        public ActionResult SystemLogMain()
        {
            return View();
        }

        [Filter.SystemLog("日志列表", LogLevel.Info)]
        public ActionResult SystemLogList(int? Id)
        {
            return View();
        }

        [Filter.SystemLog("培训日志列表", LogLevel.Info)]
        public ActionResult SystemLogTrainList()
        {
            ViewBag.DeptList = deptBL.GetAllList();
            return View();
        }


        public JsonResult GetSysLogList(string logContent, string userName, int logType, string logTimeStart, string logTimeEnd, string acceptName = "", int status = -1, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = " LogTime desc")
        {
            List<Sys_User> listUser = userBL.GetList();
            listUser.Add(new Sys_User { UserId = 0, Realname = "超级管理员" });
            listUser.Add(new Sys_User { UserId = -1, Realname = "系统操作" });//-1 为系统操作同步
            long totalcount = 0;
            int sortType = -1;
            var tempUser = new List<int>();
            bool flag = true;
            if (jsRenderSortField.ToLower().Contains("desc"))
            {
                sortType = -1;
                //listLog = listLog.OrderByDescending(p => p._id).ToList();
            }
            else
            {
                sortType = 1;
                //listLog = listLog.OrderBy(p => p._id).ToList();
            }

            if (!string.IsNullOrEmpty(userName))
            {
                var tempUserList = listUser.Where(p => p.Realname.ToLower().Contains(userName.ToLower())).ToList();
                tempUser = tempUserList.Select(p => p.UserId).ToList();
                if (tempUserList.Count == 0)
                {
                    flag = false;
                }
            }
            var listLog = new List<Sys_Log>();
            if (flag)
            {
                listLog = logBL.GetAllList(out totalcount, -1, pageSize, pageIndex, sortType, tempUser, acceptName, logContent, logTimeStart, logTimeEnd, logType, status);
            }
           

         

            foreach (var item in listLog)
            {
                var user = listUser.Where(u => u.UserId == item.CurrentUserId).FirstOrDefault();
                item.UserName = user == null ? "该用户已删除" : user.Realname;
            }

            return Json(new
            {
                result = 1,
                dataList = listLog,
                recordCount = totalcount
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取培训日志记录
        /// </summary>
        /// <param name="logTrainContent"></param>
        /// <param name="TrainUserName"></param>
        /// <param name="logTrainType"></param>
        /// <param name="logTimeTrainStart"></param>
        /// <param name="logTrainTimeEnd"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetSysLogTrainList(string logTrainContent, string TrainUserName, int logTrainType, string logTimeTrainStart, string logTrainTimeEnd, int pageSize = 20, int pageIndex = 1)
        {
            //List<Sys_User> listUser = userBL.GetList();
            List<Sys_LogTrain> listLogTrain = logTrainBL.GetAllList().Where(l => l.LogContent.Contains(logTrainContent) && (l.NoticeUserName.Contains(TrainUserName))).ToList();
            if (!string.IsNullOrEmpty(logTimeTrainStart))
            {
                listLogTrain = listLogTrain.Where(l => l.LogTime.ToLocalTime() > logTimeTrainStart.StringToDate(0)).ToList();
            }
            if (!string.IsNullOrEmpty(logTrainTimeEnd))
            {
                listLogTrain = listLogTrain.Where(l => l.LogTime.ToLocalTime() < logTrainTimeEnd.StringToDate(1)).ToList();
            }
            if (logTrainType != -1)
            {
                listLogTrain = listLogTrain.Where(l => l.LogTrainType == logTrainType).ToList();
            }
            if (!string.IsNullOrEmpty(TrainUserName))
            {
                //listUser = listUser.Where(p => p.Realname.ToLower().Contains(TrainUserName.ToLower())).ToList();
            }

            //var tempUser = new int[listUser.Count];
            int n = 0;
            //for (int r = 0; r < listUser.Count; r++)
            //{
            //    tempUser[r] = listUser[r].UserId;
            //}
            //listLogTrain = listLogTrain.Where(p => tempUser.Contains(p.CurrentUserId)).ToList();
            int totalCount = listLogTrain.Count();
            listLogTrain = listLogTrain.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var itemArray = new object[listLogTrain.Count()];
            foreach (var item in listLogTrain)
            {
                //   var user = listUser.Where(u => u.UserId == item.NoticeUserId).FirstOrDefault();

                var temp = new
                {
                    logTrainContent = item.LogContent,
                    LogTrainType = typeof(LiXinModels.Enums.LogTrainType).GetEnumName(item.LogTrainType),
                    //TrainUserName = user == null ? "该用户已删除" : user.Realname,
                    TrainUserName = item.NoticeUserName,
                    LogTimeStr = item.LogTime.ToLocalTime().ToString()
                };
                itemArray[n] = temp;
                n++;
            }
            return Json(new
            {
                result = 1,
                dataList = itemArray.ToList(),
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 群组呈现页面

        /// <summary>
        ///  群组列表呈现
        /// </summary>
        public ViewResult GroupIndex()
        {
            return View();
        }

        /// <summary>
        ///  创建群组的页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupEdit(int id)
        {

            Sys_Group sort = id > 0 ? groupBL.GetModel(id) : new Sys_Group();
            ViewBag.TrainGrade = AllTrainGrade;
            ViewData["model"] = sort;
            return View();
        }

        /// <summary>
        ///  群组发送消息的页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult GroupMessage(int id)
        {

            Sys_Group sort = id > 0 ? groupBL.GetModel(id) : new Sys_Group();
            ViewBag.TrainGrade = AllTrainGrade;
            ViewData["model"] = sort;
            return View();
        }

        /// <summary>
        ///  发送消息呈现
        /// </summary>
        public ActionResult MessageEdit()
        {
            return View();
        }

        #endregion

        #region 群组操作

        /// <returns></returns>
        /// <summary>
        /// 获取所有群组列表
        /// </summary>
        public JsonResult GetAllGroupList(string title, string username, int pageSize = 20, int pageIndex = 1)
        {

            try
            {
                int totalCount = 0;
                var where = string.Format(@" t1.Realname LIKE '%{0}%' AND t0.GroupName LIKE '%{1}%'", username.ReplaceSql(), title.ReplaceSql());
                List<Sys_Group> groupList = groupBL.GetAllList(out totalCount, pageIndex, pageSize, where);
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
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOpenGroupUserList(string groupids, string ids = "0")
        {
           // ids=ids.TrimStart(',').TrimEnd(',');
            ///所有人员所获得的学时
            var addUserIDs = "";
            foreach (var item in ids.TrimStart(',').TrimEnd(',').Split(','))
            {
                if (item.ToString() != "")
                {
                    addUserIDs = addUserIDs == "" ? item.ToString() : addUserIDs + "," + item.ToString();
                }
            }
            if (addUserIDs == "")
            {
                addUserIDs = "0";
            }
            else
            {
                if (addUserIDs.Substring(0, 1) == ",")
                {
                    addUserIDs = addUserIDs.Substring(1);
                }
            }
            var list = groupBL.GetGroupUserList(groupids, addUserIDs);

            return Json(new
            {
                dataList = list
            }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 保存群组
        /// </summary>
        /// <returns></returns>
        //[Filter.SystemLog("保存群组", LogLevel.Info)]
        public JsonResult SubmitGroup(int id, string txtSortName, string txtMemo, string AllUserID)
        {
            var re = groupBL.Checkname(txtSortName, id);
            if (re)
            {
                if (id == 0)
                {
                    //添加
                    int newid = groupBL.Insert(new Sys_Group
                    {
                        GroupName = txtSortName,
                        UserId = CurrentUser == null ? 0 : CurrentUser.UserId,
                        Memo = txtMemo,
                        CreateTime = DateTime.Now,
                        LastUpdateTime = DateTime.Now,
                        IsDelete = 0
                    });
                    if (newid > 0)
                    {
                        var qun = groupBL.AddGroupUser(newid, AllUserID);
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
                    Sys_Group sort = groupBL.GetModel(id);
                    sort.GroupName = txtSortName;
                    sort.Memo = txtMemo;
                    sort.LastUpdateTime = DateTime.Now;
                    bool newid = groupBL.UpdateByID(sort);
                    if (newid)
                    {
                        groupBL.DeleteGroupAllUser(id);
                        var qun = groupBL.AddGroupUser(id, AllUserID);
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
        /// 根据ID删除群组
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("删除群组", LogLevel.Info)]
        public JsonResult DeleteGroup(string ids)
        {
            bool result = groupBL.DeleteModel(ids);
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

        /// <summary>
        /// 获取用户（根据群组ID）
        /// </summary>
        public JsonResult GetUserByGid(int id)
        {
            List<int> sulist = groupBL.GroupUserID(id);
            string alluser = "";
            foreach (int su in sulist)
            {
                alluser += su + ",";
            }
            if (sulist.Count > 0)
            {
                alluser = alluser.Remove(alluser.LastIndexOf(","), 1);
            }
            return Json(new { content = alluser }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证群组
        /// </summary>
        public JsonResult Checkgroup(string title, int id)
        {
            var isValidate = groupBL.Checkname(title, id);
            return Json(isValidate, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取选择用户信息
        /// </summary>
        public JsonResult GetUserMessage(string userIDs, int pageSize = 20, int pageIndex = 1)
        {

            try
            {
                int totalCount = 0;
                List<Sys_GroupUser> UserList = groupBL.GetUser(userIDs, out totalCount, pageIndex, pageSize);
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
        /// 获取所有用户列表
        /// </summary>
        ///public JsonResult GetAllUserList(int id, string Uname, string Uemail, string Udept, int Usex, string UTG, int pageSize = 20, int pageIndex = 1)
        public JsonResult GetAllUserList(int id, int pageSize = 20, int pageIndex = 1)
        {
            try
            {
                //string where = " 1=1 ";
                //if (!string.IsNullOrWhiteSpace(Udept))
                //    where += string.Format(" and team.DeptPath like '%{0}%' ", Udept.ReplaceSql());
                //if (!string.IsNullOrWhiteSpace(Uemail))
                //    where += string.Format(" and team.Email like '%{0}%' ", Uemail.ReplaceSql());
                //if (!string.IsNullOrWhiteSpace(Uname))
                //    where += string.Format(" and team.Realname like '%{0}%' ", Uname.ReplaceSql());
                //if (Usex != 99)
                //    where += " and team.Sex = " + Usex;
                //if (!string.IsNullOrWhiteSpace(UTG) && UTG != "99")
                //    where += string.Format(" and ('{0}' like '%'+ team.TrainGrade +'%' and (team.TrainGrade is not null and team.TrainGrade <> '')) ", UTG);
                int totalCount = 0;
                List<Sys_GroupUser> gUserList = groupBL.GroupUserList(id, out totalCount, pageIndex, pageSize);
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
        ///  批量删除用户
        /// </summary>
        [Filter.SystemLog("删除群组用户", LogLevel.Info)]
        public JsonResult DeleteUser(int id, string userIDs)
        {
            var qun = groupBL.DeleteGroupUser(id, userIDs);
            if (qun)
                return Json(new { result = 1, content = "删除成功" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { result = 0, content = "删除失败" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 群组复制
        /// </summary>
        public JsonResult GroupCopy(string seluser)
        {
            if (Session["CopyUser"] != null)
            {
                Session.Remove("CopyUser");
                Session["CopyUser"] = seluser;
            }
            else
            {
                Session["CopyUser"] = seluser;
            }
            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 群组粘贴
        /// </summary>
        public JsonResult GroupPast(string users)
        {
            string seluser = null;
            if (Session["CopyUser"] != null)
            {
                if (users != "" && users != null)
                {
                    seluser = users + "," + Session["CopyUser"].ToString();
                }
                else
                {
                    seluser = Session["CopyUser"].ToString();
                }
                Session.Remove("CopyUser");
                string[] userList = Regex.Split(seluser, ",", RegexOptions.IgnoreCase);
                userList = GetString(userList);
                string Newuesr = "";
                for (int i = 0; i < userList.Length; i++)
                {
                    Newuesr += userList[i] + ",";
                }
                Newuesr = Newuesr.Remove(Newuesr.LastIndexOf(","), 1);
                return Json(new { result = 1, content = Newuesr }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = 0, content = "您没有选择复制内容" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取所有用户列表(查询)
        /// </summary>
        public JsonResult GetGroupUserList(int id, string Uname, string Uemail, string Udept, int Usex, string UTG)
        {
            try
            {
                string where = " 1=1 ";
                if (!string.IsNullOrWhiteSpace(Udept))
                    where += string.Format(" and team.DeptPath like '%{0}%' ", Udept.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(Uemail))
                    where += string.Format(" and team.Email like '%{0}%' ", Uemail.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(Uname))
                    where += string.Format(" and team.Realname like '%{0}%' ", Uname.ReplaceSql());
                if (Usex != 99)
                    where += " and team.Sex = " + Usex;
                if (!string.IsNullOrWhiteSpace(UTG) && UTG != "99")
                    where += string.Format(" and ('{0}' like '%'+ team.TrainGrade +'%' and (team.TrainGrade is not null and team.TrainGrade <> '')) ", UTG);
                int totalCount = 0;
                List<Sys_GroupUser> gUserList = groupBL.GroupUserList(id, out totalCount, 1, int.MaxValue, where);
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
        /// 发送消息
        /// </summary>
        public JsonResult SendMessageByGroup(string Uids, string Title, string Content, int Must)
        {
            try
            {
                List<Sys_User> userlist = userBL.GetList(string.Format(" UserId in ({0})", Uids));
                List<string> telephone = new List<string>();
                List<string> EmailAddress = new List<string>();

                var emailList = new List<KeyValuePair<string, string>>();
                var messlist = new List<KeyValuePair<string, string>>();
                foreach (Sys_User u in userlist)
                {
                    if (!string.IsNullOrWhiteSpace(u.Email))
                    {
                        // Content = u.Realname + "," + Content;
                        emailList.Add(new KeyValuePair<string, string>(u.Email, u.Realname + "，" + Content));
                    }
                    if (!string.IsNullOrWhiteSpace(u.MobileNum))
                    {
                        // Content = u.Realname + "," + Content;
                        messlist.Add(new KeyValuePair<string, string>(u.MobileNum, u.Realname + "，" + Content));
                    }
                }
                if (Must == 1)
                {
                    if (emailList.Count > 0)
                        //SendEmail(EmailAddress, Title, Content);
                        SendEmail(emailList, Title, type: 1);
                }
                else if (Must == 2)
                {
                    if (messlist.Count > 0)
                        SendMessage(messlist, 1);
                }
                else
                {
                    if (emailList.Count > 0)
                        SendEmail(emailList, Title, type: 1);
                    if (messlist.Count > 0)
                        SendMessage(messlist, 1);
                }
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        //数组去重
        public static string[] GetString(string[] values)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < values.Length; i++)//遍历数组成员
            {
                if (list.IndexOf(values[i].ToLower()) == -1)
                    //对每个成员做一次新数组查询如果没有相等的则加到新数组
                    list.Add(values[i]);
            }       
            return list.ToArray();
        }
    }
}
