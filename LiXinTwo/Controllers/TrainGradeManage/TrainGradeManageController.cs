using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.User;
using LiXinLanguage;
using LiXinModels.User;
using LiXinInterface;
using LiXinModels;
using LiXinControllers.Filter;

namespace LiXinControllers
{
    public class TrainGradeManageController : BaseController
    {
        private LiXinSync.SyncInfo syncUserInfo = new LiXinSync.SyncInfo();
        protected ISys_TrainGrade trainBL;
        protected IUser userBL;
        public TrainGradeManageController(ISys_TrainGrade _trainBL, IUser _userBL)
        {
            trainBL = _trainBL;
            userBL = _userBL;
        }
        #region 呈现
        public ActionResult TrainGrade()
        {
            ViewBag.payGrade = trainBL.GetAllPayGrade();
            return View();
        }

        public ActionResult AddTrainGrade()
        {
            ViewBag.trainGrade = trainBL.GetAllTrainGrade();
            return View();
        }

        public ActionResult AllAddTrainGrade(string userIDs)
        {
            ViewBag.userIDs = userIDs;
            return View();
        }

        public ActionResult GradeManage()
        {
            ViewBag.payGrade = trainBL.GetAllPayGrade();
            return View();
        }

        public ActionResult GradeUpdate(int ID)
        {
            ViewBag.ID = ID;
            ViewBag.trainGrade = trainBL.GetAllTrainGrade();
            ViewData["model"] = trainBL.GetSingle(ID);
            return View();
        }

        public ActionResult TrainGradeDetail(int userID)
        {
            ViewBag.userID = userID;
            ViewData["user"] = userBL.Get(userID);
            return View();
        }
        #endregion

        #region 培训级别维护
        /// <summary>
        /// 获取培训级别基本详情
        /// </summary>
        public JsonResult GetUserTrain(string realName = "", string deptName = "", string JobNum = "", string payGrade = "", int pageIndex = 1, int pageSize = int.MaxValue, string jsRenderSortField = "")
        {
            try
            {
                var totalCount = 0;
                //获取培训负责人
                var trainUserID = CurrentUser.UserId;
                var deptIDs = "";
                if(CurrentUser.TrainMaster==1)
                {
                    deptIDs = CurrentUser.ManageDeparts;
                }

                string star;
                string end;
                var TrainGradeTime = trainBL.IsUpdateTrain(out star, out end, AllSystemConfigs.Where(p => p.ConfigType == 9).FirstOrDefault(), DateTime.Now.Date);
                var list = trainBL.GetUserTrain(out totalCount, trainUserID,deptIDs, pageIndex, pageSize, realName.ReplaceSql(), JobNum.ReplaceSql(), payGrade.ReplaceSql(), deptName, jsRenderSortField == "" ? "ORDER BY su.DeptPath asc,su.TrainGrade asc" : ("order by su." + jsRenderSortField));

                foreach (var item in list)
                {
                    item.isOut = item.UserID == CurrentUser.UserId ? 1 : 0;
                }
                list.ForEach(p => p.IsUpdate = TrainGradeTime);
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalCount,
                    isupdate = TrainGradeTime
                }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Sys_TrainGrade>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 新增维护信息
        /// </summary>
        public JsonResult InsertTrainGrade(int userID, string oldgrade, string newgrade, string reason = "", int status = 1)
        {
            try
            {
                var model = new Sys_TrainGrade()
                {
                    CreateUserID = CurrentUser.UserId,
                    UserID = userID,
                    OldGrade = oldgrade,
                    NewGrade = newgrade,
                    Reason = reason,
                    Status = status,
                    SubmitFlag = status == 0 ? 1 : 0//当为status的时候，即没有更新成功，就是提交申请的，此时，设为1
                };

                trainBL.InsertTrainGrade(model);
                if (status == 1)
                {
                    trainBL.UpdateTrainGrade(userID, newgrade);
                }
                return Json(new
                {
                    result = 1,
                    content = "维护级别成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "维护级别失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取批量修改的人员信息
        /// </summary>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        public JsonResult GetUpdateUser(string userIDs)
        {
            try
            {
                int totalcount = 0;
                var list = trainBL.GetUpdateUser(userIDs);
                var gradeList = trainBL.GetAllTrainGrade();
                list.ForEach(p => p.TraindGradeList = gradeList);
                return Json(new
                {
                    result = 0,
                    dataList = list
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Sys_User>()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 批量新增维护信息
        /// </summary>
        [Filter.SystemLog("批量新增维护信息", LogLevel.Info)]
        public JsonResult InsertAllTrainGrade()
        {
            try
            {
                var data = Request.Form["gradeList"];
                dynamic traindata = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
                for (int i = 0; i < traindata.Count; i++)
                {
                    var model = new Sys_TrainGrade()
                    {
                        UserID = traindata[i].userID,
                        CreateUserID = CurrentUser.UserId,
                        OldGrade = traindata[i].oldgrade,
                        NewGrade = traindata[i].newgrade,
                        Reason = traindata[i].reason,
                        Status = traindata[i].status,
                        SubmitFlag = traindata[i].status == 0 ? 1 : 0//当为status的时候，即没有更新成功，就是提交申请的，此时，设为1
                    };
                    trainBL.InsertTrainGrade(model);
                    if (model.Status == 1)
                    {
                        trainBL.UpdateTrainGrade(model.UserID, model.NewGrade);
                    }
                  
                }

                return Json(new
                {
                    result = 1,
                    content = "维护级别成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(new
                {
                    result = 0,
                    content = "维护级别失败"
                }, JsonRequestBehavior.AllowGet);
            }


        }

        /// <summary>
        /// 是否是培训负责人
        /// </summary>
        /// <param name="userID">被任命人ID</param>
        /// <param name="trainMaster">0：取消；1：任命</param>
        /// <param name="selectDept">所管理的部门集合</param>
        /// <returns></returns>
         [Filter.SystemLog("是否是培训负责人", LogLevel.Info)]
        public JsonResult UpdateTrainMaster(int userID, int trainMaster,string selectDept="")
        {
            try
            {
                trainBL.UpdateTrainMaster(userID, trainMaster,selectDept);
                return Json(new
                {
                    result = 1,
                    content = "成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(new
                {
                    result = 0,
                    content = "失败"
                }, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion

        #region 培训级别变更
        /// <summary>
        /// 培训级别变更
        /// </summary>
        public JsonResult GetTarinUpdate(string realName = "", string payGrade = "", string DeptName = "", int status = -1, int pageIndex = 1, int pageSize = int.MaxValue)
        {

            try
            {
                int totalCount = 0;
                string star;
                string end;
                var TrainGradeTime = trainBL.IsUpdateTrain(out star, out end, AllSystemConfigs.Where(p => p.ConfigType == 9).FirstOrDefault(), DateTime.Now.Date);
                var list = trainBL.GetTarinUpdate(out totalCount, star, end, realName.ReplaceSql(), payGrade.ReplaceSql(),
                   DeptName.ReplaceSql(), status, pageIndex, pageSize);
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Sys_TrainGrade>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 更新培训数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="newgrade"></param>
        /// <returns></returns>
         [Filter.SystemLog("更新培训数据", LogLevel.Info)]
        public JsonResult UpdateGrade(string newgrade, int ID, int userID)
        {
            try
            {
                trainBL.UpdateSys_TrainGrade(ID, newgrade);
                trainBL.UpdateTrainGrade(userID, newgrade);

                return Json(new
                {
                    result = 1,
                    content = "维护级别成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(new
               {
                   result = 0,
                   content = "维护级别失败"
               }, JsonRequestBehavior.AllowGet);
            }
        }


        [Filter.SystemLog("维护变更", LogLevel.Info)]
        public JsonResult AllUpdate(string tIDs)
        {
            try
            {
                trainBL.AllUpdate(tIDs);
                foreach (var id in tIDs.Split(','))
                {
                    trainBL.UpdateTrainGradeBytrain(id.StringToInt32());
                }
                return Json(new
                {
                    result = 1,
                    content = "维护变更成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "维护变更失败"
                }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region 详情
        /// <summary>
        /// 变更记录
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetUpdateLog(int userID, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            try
            {
                int totalCount = 0;
                var list = trainBL.GetUpdateLog(out totalCount, userID, pageIndex, pageSize);
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Sys_TrainGrade>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        /// <summary>
        /// 同步级别
        /// </summary>
        /// <returns></returns>
        public JsonResult TainGradeSync()
        {
            try
            {
                syncUserInfo.SyncGrade();
                RefreshTrainGrade();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取当前用户所在部门及其子部门
        /// </summary>
        /// <returns></returns>
        public JsonResult GetManageDepart()
        {
            var deptList = trainBL.GetMyDepartMents(CurrentUser.JobNum??"无");
            return Json(new{deptList}, JsonRequestBehavior.AllowGet);
        }

    }
}
