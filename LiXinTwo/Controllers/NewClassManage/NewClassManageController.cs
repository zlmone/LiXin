using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.NewClassManage;
using LiXinInterface.User;
using LiXinModels.User;
using LixinModels.NewClassManage;
using System.Linq;
using LiXinControllers.Filter;
using LiXinInterface;

namespace LiXinControllers.NewClassManage
{
    public class NewClassManageController : BaseController
    {
        #region  == 接口实例化 ==
        protected IDepartment DepartmentBl;
        protected IPost PostBl;
        protected IRight RightBl;
        protected IRole RoleBl;
        protected IUser UserBl;
        protected INewClass NewClassBl;
        protected INewGroup NewGroupBl;
        protected INewGroupUser NewGroupUserBl;
        protected INewUser nuserBL;
        public NewClassManageController(
            IDepartment departmentBl
            , IPost postBl
            , IRight rightBl
            , IRole roleBl
            , IUser userBl
            , INewClass newClassBl
            , INewGroup newGroupBl
            , INewGroupUser newGroupUserBl
            , INewUser _nuserBL
            )
        {
            DepartmentBl = departmentBl;
            PostBl = postBl;
            RightBl = rightBl;
            RoleBl = roleBl;
            UserBl = userBl;
            NewClassBl = newClassBl;
            NewGroupBl = newGroupBl;
            NewGroupUserBl = newGroupUserBl;
            nuserBL = _nuserBL;
        }
        #endregion

        #region == 界面呈现 ==
        /// <summary>
        /// 学员分班管理界面
        /// </summary>
        /// <returns></returns>
        public ActionResult NewClassManage()
        {
            var yearList = nuserBL.GetNewYear();
            yearList.Add(yearList.Last() + 1);
            ViewBag.yearList = yearList;
            ViewBag.nowyear = DateTime.Now.Year;
            return View();
        }
        /// <summary>
        /// 自动分班弹出页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AutoSplitClass()
        {
            ViewBag.NotSplitClassNum = NewGroupUserBl.GetCountList();
            return View();
        }

        /// <summary>
        /// 插班管理弹出页面
        /// </summary>
        /// <returns></returns>
        public ActionResult JoinClassManage(string userIds)
        {
            var classList = NewClassBl.GetList(string.Format(" and c.Year={0} ", System.DateTime.Now.Year));
            ViewBag.userIds = userIds;
            return View(classList);
        }

        /// <summary>
        /// 根据班级ID获得人员列表界面
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassPersonManage(int classId)
        {
            ViewBag.classId = classId;
            var classModel = NewClassBl.GetModel(classId) ?? new New_Class();
            ViewBag.className = classModel.ClassName;
            List<New_Group> groupList = NewGroupBl.GetList(string.Format(" and g.ClassId={0} ", classId)) ?? new List<New_Group>();
            return View(groupList);
        }

        /// <summary>
        /// 根据班级ID获得人员列表界面
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectNewPersonList(string type = "checkbox")
        {
            ViewBag.type = type;
            return View();
        }

        public ActionResult MyClassRoom()
        {
            // ViewBag.userID = userID;
            ViewBag.realName = CurrentUser.Realname;
            ViewBag.model = NewGroupUserBl.GetClassInfoByUserID(CurrentUser.UserId);
            return View();
        }
        #endregion

        #region == 方法 ==
        #region== 当前班组方法 ==
        /// <summary>
        /// 获取当前班组列表
        /// <param name="year">年限</param>
        /// </summary>
        [ValidateInput(false)]
        public JsonResult GetCurrentClassList(int year = -1, int pageSize = int.MaxValue, int pageIndex = 1)
        {
            year = year == -1 ? DateTime.Now.Year : year;
            try
            {
                int totalCount = 0;
                List<New_Class> list = NewClassBl.GetCurrentClassList(out totalCount, year.ToString(), pageIndex, pageSize);
                return Json(new
                {
                    dataList = list,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<New_Class>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 批量删除班级
        /// <param name="classIds">班级ID集合</param>
        /// </summary>
        [Filter.SystemLog("删除班级", LogLevel.Info)]
        public JsonResult BatchDeleteClass(string classIds)
        {
            try
            {
                NewClassBl.DeleteBatchModel(classIds);
                RefreshClass(); //删除班级刷新缓存
                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 结束分班
        /// </summary>
        public JsonResult OverSplitClass()
        {
            try
            {
                NewClassBl.UpdateClassToNotDelete();
                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region == 指定班级的人员调整方法 ==
        /// <summary>
        /// 根据班级ID获得人员列表界面列表
        /// <param name="realName">姓名</param>
        /// <param name="classId">班级ID</param>
        /// <param name="groupId">分组ID</param>
        /// </summary>
        [ValidateInput(false)]
        public JsonResult GetClassPersonManageList(string realName, int classId = 0, int groupId = -1, int pageSize = int.MaxValue, int pageIndex = 1)
        {
            try
            {
                string where = "";
                if (!string.IsNullOrWhiteSpace(realName))
                {
                    where += string.Format(" and u.Realname like '%{0}%' ", realName.ReplaceSql());
                }
                if (groupId != -1)
                {
                    where += string.Format(" and g.Id={0} ", groupId);
                }
                int totalCount = 0;
                List<Sys_User> list = NewClassBl.GetClassPersonsList(out totalCount, classId, where, pageIndex, pageSize);
                return Json(new
                {
                    dataList = list,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<Sys_User>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据班级Id和人员Id集合批量删除数据
        /// </summary>
        /// <param name="userIds">学员ID集合格式为: 1,2,3,4</param>
        /// <param name="classId">班级ID</param>
        /// <returns>成功返回True，失败返回False</returns>
        [Filter.SystemLog("班级人员调整 删除", LogLevel.Info)]
        public JsonResult DeleteBatchByUserAndClass(string userIds, int classId = 0)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userIds))
                {
                    return Json(new
                    {
                        result = 0
                    }, JsonRequestBehavior.AllowGet);
                }
                NewGroupUserBl.DeleteBatchByUserAndClass(userIds, classId);
                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 已分班人员批量调整班组
        /// </summary>
        /// <param name="userIds">要调整的人员ID集合</param>
        /// <param name="newClassId">新班级ID</param>
        /// <param name="newGroupId">新组ID</param>
        /// <param name="newClassNo">新班号</param>
        /// <param name="newGroupNo">新组号</param>
        /// <param name="oldClassId">老班Id</param>
        /// <returns></returns>
        [Filter.SystemLog("班级人员调整 调整班组", LogLevel.Info)]
        public JsonResult ChangeClassGroupByOldUser(string userIds, int newClassId, int newGroupId, string newClassNo, string newGroupNo, int oldClassId)
        {

            if (string.IsNullOrWhiteSpace(userIds))
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
            var classModel = NewClassBl.GetModel(oldClassId);
            //if (classModel.IsDoDelete == 0)//学号不可更改直接返回
            //{
            //    NewGroupUserBl.BatchChangeClassGroup(userIds, oldClassId, newClassId, newGroupId);//批量更改班组
            //    return Json(new
            //    {
            //        result = 1
            //    }, JsonRequestBehavior.AllowGet);
            //}
            //需要批量修改学号的学生
            var userIdList = userIds.Trim(',').Split(',').ToList();
            var userIdList1 = userIds.Trim(',').Split(',').ToList();
            //如果新的班组和老的班组相同则将不对学员学号进行更改
            if (newClassId == oldClassId)
            {
                foreach (var uid in userIdList1)
                {
                    var model = NewGroupUserBl.GetModelByClassAndUser(oldClassId, int.Parse(uid));
                    if (newGroupId == model.GroupId)
                    {
                        userIdList.Remove(uid);
                    }
                }
            }
            if (userIdList.Count == 0)
            {
                NewGroupUserBl.BatchChangeClassGroup(userIds, oldClassId, newClassId, newGroupId);//批量更改班组
                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }

            int currentNo = 0;
            string yearClassGroup = System.DateTime.Now.Year + newClassNo + newGroupNo;//要生成的学号前缀
            var numberList =
                NewGroupUserBl.GetNumberList(string.Format(" and u.NumberID like '%{0}%' ", yearClassGroup));
            foreach (var v in numberList)
            {
                if (v.Replace(yearClassGroup, "").Length == 4)
                {
                    currentNo = int.Parse(v.Replace(yearClassGroup, "")) + 1;
                    break;
                }
            }

            Dictionary<int, string> userNumber = new Dictionary<int, string>();
            for (int i = 0; i < userIdList.Count; i++)
            {
                int no = currentNo + i;
                userNumber.Add(int.Parse(userIdList[i]), System.DateTime.Now.Year + newClassNo + newGroupNo + no.ToString("0000"));
            }

            int isDoDelete = NewClassBl.GetModel(newClassId).IsDoDelete;//要插入的班级是否已经结束分班
            foreach (var un in userNumber)
            {
                Sys_User userModel = UserBl.Get(un.Key);
                //if (userModel.IsChangeNumberId != 1)
                //{
                //    userModel.NumberID = un.Value;
                //    userModel.IsChangeNumberId = 0;
                //    if (isDoDelete == 0)
                //    {
                //        userModel.IsChangeNumberId = 1;
                //    }
                //    UserBl.Update(userModel);
                //}
                userModel.NumberID = un.Value;
                userModel.IsChangeNumberId = 0;
                UserBl.Update(userModel);
                NewGroupUserBl.BatchChangeClassGroup(un.Key.ToString(), oldClassId, newClassId, newGroupId);//批量更改班组
            }

            return Json(new
            {
                result = 1
            }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region == 未分班人员方法 ==
        /// <summary>
        /// 获取当前未分班人员列表
        /// <param name="realName">姓名</param>
        /// </summary>
        [ValidateInput(false)]
        public JsonResult GetUserNotInNewClassList(string realName,int year=-1, int pageSize = int.MaxValue, int pageIndex = 1)
        {
            try
            {
                year = year == -1 ? DateTime.Now.Year : year;
                int totalCount = 0;    
                List<Sys_User> list = NewGroupUserBl.GetList(out totalCount, realName,year, pageIndex, pageSize);
                return Json(new
                {
                    dataList = list,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<Sys_User>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 自动分班
        /// <param name="splitSex">性别不同</param>
        /// <param name="splitSchool">毕业院校不同</param>
        /// <param name="splitMajor">毕业专业不同</param>
        /// <param name="splitFirm">事务所实习经验的有无</param>
        /// <param name="perClassPersonCount">每班人数</param>
        /// <param name="perGroupPersonCount">每组人数</param>
        /// </summary>
        public JsonResult SaveAutoSplitClass(bool splitSex, bool splitSchool, bool splitMajor, bool splitFirm
                                         , int perClassPersonCount, int perGroupPersonCount)
        {
            try
            {
                NewGroupUserBl.DoAutoSplitClass(splitSex, splitSchool, splitMajor, splitFirm
                                         , perClassPersonCount, perGroupPersonCount);
                RefreshClass(); //新增班级刷新缓存
                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据班级ID获取组
        /// </summary>
        /// <param name="classId">班级ID</param>
        /// <returns></returns>
        public JsonResult GetGroupByClass(string classId)
        {
            List<New_Group> groupList = NewGroupBl.GetList(string.Format(" and g.ClassId={0} ", classId)) ?? new List<New_Group>();
            return Json(groupList, JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 获取班级集合
        /// </summary>
        /// <param name="realName">班级名称</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页</param>
        /// <returns></returns>
        public JsonResult GetClassList(string realName = "", int pageSize = int.MaxValue, int pageIndex = 1)
        {
            var totalCount = 0;
            var list = NewClassBl.GetClassList(out totalCount,
                                               string.Format(" ClassName like '%{0}%' and Year={1} ", realName.ReplaceSql(), DateTime.Now.Year),
                                               (pageIndex - 1) * pageSize, pageSize);
            return Json(new
                            {
                                dataList = list,
                                recordCount = totalCount
                            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 未分班人员进行插班操作
        /// </summary>
        /// <param name="userIds">要插班的人员ID集合</param>
        /// <param name="classId">班级ID</param>
        /// <param name="groupId">组ID</param>
        /// <param name="classNo">班号</param>
        /// <param name="groupNo">组号</param>
        /// <param name="flag">0：未分班人员插班操作  1：指定班组进行新增人员操作</param>
        /// <returns></returns>
        [Filter.SystemLog("班级维护 插班操作", LogLevel.Info)]
        public JsonResult InsertClassByIsNew(string userIds, int classId, int groupId, string classNo, string groupNo, int flag = 0)
        {

            if (string.IsNullOrWhiteSpace(userIds))
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
            if (flag == 1)
            {
                classNo = NewClassBl.GetModel(classId).ClassNo;
                groupNo = NewGroupBl.GetModel(groupId).GroupNo;
            }

            var userIdList = userIds.Trim(',').Split(',').ToList();

            int currentNo = 0;
            string yearClassGroup = System.DateTime.Now.Year + classNo + groupNo;//要生成的学号前缀
            var numberList =
                NewGroupUserBl.GetNumberList(string.Format(" and u.NumberID like '%{0}%' ", yearClassGroup));
            foreach (var v in numberList)
            {
                if (v.Replace(yearClassGroup, "").Length == 4)
                {
                    currentNo = int.Parse(v.Replace(yearClassGroup, "")) + 1;
                    break;
                }
            }

            Dictionary<int, string> userNumber = new Dictionary<int, string>();
            for (int i = 0; i < userIdList.Count; i++)
            {
                int no = currentNo + i;
                userNumber.Add(int.Parse(userIdList[i]), System.DateTime.Now.Year + classNo + groupNo + no.ToString("0000"));
            }

            int isDoDelete = NewClassBl.GetModel(classId).IsDoDelete;//要插入的班级是否已经结束分班
            foreach (var un in userNumber)
            {
                Sys_User userModel = UserBl.Get(un.Key);
                userModel.NumberID = un.Value;
                userModel.IsChangeNumberId = 0;
                UserBl.Update(userModel);
                //Sys_User userModel = UserBl.Get(un.Key);
                //if (userModel.IsChangeNumberId != 1)
                //{
                //    userModel.NumberID = un.Value;
                //    userModel.IsChangeNumberId = 0;
                //    if (isDoDelete == 0)
                //    {
                //        userModel.IsChangeNumberId = 1;
                //    }
                //    UserBl.Update(userModel);
                //}
                NewGroupUserBl.BatchAddNewGroupUser(classId, groupId, userModel.UserId, un.Value);
            }

            return Json(new
            {
                result = 1
            }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 获取新进人数、已分班人数，已分班级数
        /// <param name="year">年限</param>
        /// </summary>
        public JsonResult GetClassAndPersonCount(int year = 0)
        {
            year = System.DateTime.Now.Year;
            var countDict = NewClassBl.GetClassAndPersonCount(year);
            int splitClassCount = countDict["splitClassCount"];//已分班级数
            int newUserCount = countDict["newUserCount"];//新进人数
            int splitUserCount = countDict["splitUserCount"];//已分班人数

            return Json(new
            {
                splitClassCount = splitClassCount,
                newUserCount = newUserCount,
                splitUserCount = splitUserCount
            }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 我的班组详情
        /// <summary>
        /// 获取当前班级下的所有人员
        /// </summary>
        /// <returns></returns>
        public JsonResult GetClassUserList(int classID, string realName, int pageSize = int.MaxValue, int pageIndex = 1)
        {
            try
            {
                string where = " Sys_User.IsDelete = 0";
                where += " and   Sys_User.UserId IN (SELECT UserId FROM New_GroupUser WHERE ClassId=" + classID + ")";

                if (!string.IsNullOrWhiteSpace(realName))
                    where += string.Format(" and Sys_User.Realname like '%{0}%' ", realName.ReplaceSql());
                int totalcount = 0;
                var list = UserBl.GetList(out totalcount, where, (pageIndex - 1) * pageSize, pageSize);
                return Json(new
                {
                    dataList = list,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<Sys_User>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        /// <summary>
        /// 生成班级名称
        /// <param name="SumCount">一共分几个班</param>
        /// </summary>
        public JsonResult GetCanClass(int SumCount)
        {
            try
            {
                var list = NewGroupUserBl.GetClassnoList(SumCount);
                // RefreshClass(); //新增班级刷新缓存
                return Json(new
                {
                    result = 1,
                    list = list
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    list = new List<string>()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 自动分班
        /// <param name="splitSex">性别不同</param>
        /// <param name="splitSchool">毕业院校不同</param>
        /// <param name="splitMajor">毕业专业不同</param>
        /// <param name="splitFirm">事务所实习经验的有无</param>
        /// <param name="classconfig">班级配置 班级人数，每组人数； </param>
        /// </summary>
        [Filter.SystemLog("自动分班", LogLevel.Info)]
        public JsonResult SaveAllAutoSplitClass(bool splitSex, bool splitSchool, bool splitMajor, bool splitFirm
                                         , string classconfig)
        {
            try
            {
                //NewGroupUserBl.DoAutoSplitClass(splitSex, splitSchool, splitMajor, splitFirm
                //                         , perClassPersonCount, perGroupPersonCount);
                foreach (var item in classconfig.Split(';'))
                {
                    NewGroupUserBl.DoAutoSingleSplitClass(splitSex, splitSchool, splitMajor, splitFirm,
                                         item.Split(',')[0].StringToInt32(), item.Split(',')[1].StringToInt32());
                }
                RefreshClass(); //新增班级刷新缓存
                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}