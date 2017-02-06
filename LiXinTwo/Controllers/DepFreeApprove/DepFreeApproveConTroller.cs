using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;
using LiXinModels.MyApproval;
using System.Web.Mvc;
using LiXinInterface.MyApproval;
using LiXinCommon;
using LiXinInterface.SystemManage;
using LiXinBLL.User;
using LiXinModels;

namespace LiXinControllers
{
    public class DepFreeApproveConTroller : BaseController
    {
        private IFree freeBL;
        private IFree_UserApplyTeacherDetails teacherDetailsBL;
        private IFree_UserOtherApply iFree_UserOtherApply;
        private IFree_UserApplyOtherForm iFree_UserApplyOtherForm;
        public DepFreeApproveConTroller(IFree _freeBL, IFree_UserApplyTeacherDetails _teacherDetailsBL, IFree_UserOtherApply _iFree_UserOtherApply, IFree_UserApplyOtherForm _iFree_UserApplyOtherForm)
        {
            freeBL = _freeBL;
            teacherDetailsBL = _teacherDetailsBL;
            iFree_UserOtherApply = _iFree_UserOtherApply;
            iFree_UserApplyOtherForm = _iFree_UserApplyOtherForm;
        }

        public ActionResult FreeApprove()
        {
            return View();
        }

        #region 其他形式审批
        /// <summary>
        /// 其他形式审批
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_OtherApprove()
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.yearList = freeBL.GetYearList();
            ViewBag.IsTimeIn = FreeIsTimeIn(61, 0);
            return View();
        }


        /// <summary>
        /// 其他形式审批详情
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ApplyUserID"></param>
        /// <param name="type">1 其他形式 2 免修 3 授课人</param>
        /// <returns></returns>
        public ActionResult Free_OtherApproveDetails(int ID, int ApplyUserID, int ApproveStatus, int DepApproveStatus, int type = 1)
        {
            ViewBag.ID = ID;
            ViewBag.type = type;
            ViewBag.ApplyUserID = ApplyUserID;
            ViewBag.ApproveStatus = ApproveStatus;
            ViewBag.DepApproveStatus = DepApproveStatus;
            ViewBag.Free_ApporveID = 0;
            var ApproveUser = freeBL.GetSingleByID(ID, type);
            ApproveUser.ApplyFileList = freeBL.GetApplyFile(ID);
            if (type == 3)
            {
                if (ApproveUser.ApplyType == 3)
                {
                    ApproveUser.teacherDetails = teacherDetailsBL.GetFree_UserApplyTeacherDetailsList(" IsDelete=0 and userApplyID=" + ID);
                }
            }
            if (ApproveStatus == 1)
            {
                ///最后一次分所审批通过的状态
                var MaxBatchmodel = freeBL.GetMaxDepApproveBatch(ID, " type=1 and  ApproveType=1 and DepApprove=1");
                ViewBag.Free_ApporveID = MaxBatchmodel.ID;
                ViewBag.MaxBatch = MaxBatchmodel.CheackBatch;
            }

            return View(ApproveUser);
        }

        /// <summary>
        /// 获取其他审批
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOtherApproveList(string realName = "", string ApplyContent = "", string Start_ApplyTime = "", string End_ApplyTime = "",
            string TrainGrade = "", int Year = -1, int isCommit = -1, int ApproveStatusDep = -1, int ApproveStatus = -1,
            int pageSize = 10, int pageIndex = 1, string jsRenderSortField = "ApplyTime desc")
        {
            try
            {
                int totalcount = 0;
                string where = " 1=1 and (typeForm=0)";

                #region 条件
                if (!string.IsNullOrEmpty(realName))
                {
                    where += " and Realname like '%" + realName.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(ApplyContent))
                {
                    where += " and ApplyContent like '%" + ApplyContent.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(Start_ApplyTime))
                {
                    where += " and ApplyTime >='" + Start_ApplyTime + "'";
                }
                if (!string.IsNullOrEmpty(End_ApplyTime))
                {
                    where += " and ApplyTime<='" + End_ApplyTime + "'";
                }
                if (!string.IsNullOrEmpty(TrainGrade))
                {
                    where += " and TrainGrade='" + TrainGrade + "'";
                }
                if (Year != -1)
                {
                    where += " and Year=" + Year;
                }
                if (isCommit != -1)
                {
                    if (isCommit == 1)
                    {
                        where += " and isCommit>0";
                    }
                    else
                    {
                        where += " and isCommit=" + isCommit;
                    }
                }
                if (ApproveStatusDep != -1)
                {
                    if (ApproveStatusDep == 3)
                    {
                        where += " and ApproveStatus=1 and DepApproveStatus=2";
                    }
                    else if (ApproveStatusDep == 1)
                    {
                        where += " and ApproveStatus=1 and (DepApproveStatus=0 or DepApproveStatus=1)";
                    }
                    else
                    {
                        where += " and ApproveStatus=" + ApproveStatusDep;
                    }
                }
                if (ApproveStatus != -1)//总所审批退回
                {
                    if (ApproveStatus == 0)
                    {
                        where += " and ApproveStatus=1 and  DepApproveStatus=0";
                    }
                    else
                    {
                        where += " and DepApproveStatus=" + ApproveStatus;
                    }

                }
                #endregion

                var list = freeBL.GetDepOtherApproveList(out totalcount, CurrentUser.UserId, where: where);
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                list = list.SortListByField(jsRenderSortField);
                return Json(new { result = 1, dataList = list, recordCount = totalcount }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, dataList = new List<ShowFreeDetails>(), recordCount = 0 }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 总所 下载或者点击查看按钮是第一次讲审批内容初始化
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveDepApprove(int ApproveID, int ID, int ApproveType, int MaxBatch, int DepApprove = 0, string DepReason = "", int type = 0)
        {
            try
            {
                var addID = 0;
                if (ID == 0)
                {
                    var model = new LiXinModels.MyApproval.Free_Apporve();
                    var where = string.Format(" type=2 and  userApplyID={0} AND CheackBatch={1}  AND ApproveType={2}"
                       , ApproveID, MaxBatch, ApproveType);
                    var list = freeBL.GetFreeApporve(where);
                    if (list.Count() == 0)
                    {
                        model.DepApprove = 0;
                        model.DepReason = "";
                        model.userApplyID = ApproveID;
                        model.CheackBatch = MaxBatch;
                        model.type = 2;
                        model.ApproveUserID = CurrentUser.UserId;
                        model.ApproveTime = DateTime.Now;
                        model.ApproveType = ApproveType;
                        model.LookFile = 1;
                        freeBL.InsertFree_Apporve(model);
                        addID = model.ID;
                    }
                    else
                    {
                        addID = list.FirstOrDefault().ID;
                    }
                }
                else
                {
                    var single = freeBL.GetFreeApporve(" ID=" + ID).FirstOrDefault();

                    single.DepApprove = DepApprove;
                    single.DepReason = DepReason;
                    single.LookFile = 1;
                    single.ApproveUserID = CurrentUser.UserId;
                    single.ApproveTime = DateTime.Now;
                    freeBL.UpdateFree_Apporve(single);
                    //其他有组织形式
                    if (ApproveType == 3)
                    {
                        var userApply = iFree_UserApplyOtherForm.GetFree_UserApplyOtherForm(" ID=" + ApproveID).FirstOrDefault();
                        var sql = "";
                        ///通过
                        if (DepApprove == 1)
                        {
                            var tscore = userApply.OtherFromID != 0 ? userApply.TogetherScore : 0;
                            var cscore = userApply.OtherFromID != 1 ? userApply.CPAScore : 0;
                            sql = ",GettScore=" + tscore + ",GetCPAScore=" + cscore;
                        }
                        iFree_UserApplyOtherForm.UpdateBywhere(ApproveID, " DepApproveStatus=" + DepApprove + ", DepReason='" + DepReason + "'" + sql);
                    }
                    else
                    {
                        var userApply = iFree_UserOtherApply.GetFree_UserOtherApplyById(single.userApplyID);
                        var sql = "";
                        ///通过
                        if (DepApprove == 1)
                        {
                            sql = ",GettScore=" + userApply.tScore + ",GetCPAScore=" + userApply.CPAScore;
                        }

                        freeBL.UpdateUserApply(single.userApplyID, " DepApproveStatus=" + DepApprove + ", DepReason='" + DepReason + "'" + sql);
                    }

                    #region 发邮件

                    var model = new ShowFreeDetails();
                    var name = "";
                    switch (type)
                    {
                        case 0:
                            model = freeBL.GetSingleByID(ApproveID, -1);
                            if (model.CPA == "是")
                            {
                                switch (model.ConvertType)
                                {
                                    case 1:
                                        name = "内部培训其他形式学时";
                                        break;
                                    case 0:
                                        name = "CPA继续教育其他形式学时";
                                        break;
                                    case 2:
                                        name = "内部培训/CPA继续教育其他形式学时";
                                        break;
                                }
                            }
                            else
                            {
                                name = "内部培训其他形式学时";
                            }

                            break;
                        case 1:
                            model = freeBL.GetFreeSingleByID(ApproveID);
                            if (model.CPA == "是")
                            {
                                switch (model.ApplyType)
                                {
                                    case 0:
                                        name = "内部培训免修学时";
                                        break;
                                    case 1:
                                        name = "CPA继续教育免修学时";
                                        break;
                                    case 2:
                                        name = "内部培训/CPA继续教育免修学时";
                                        break;
                                }
                            }
                            else
                            {
                                name = "内部培训免修学时";
                            }

                            break;
                        case 2:
                            model = freeBL.GetOrgSingleByID(ApproveID);
                            if (model.CPA == "是")
                            {
                                switch (model.OtherFromID)
                                {
                                    case 0:
                                        name = "CPA继续教育其他有组织形式学时";
                                        break;
                                    case 1:
                                        name = "内部培训其他有组织形式学时";
                                        break;
                                   
                                    case 2:
                                        name = "内部培训/CPA继续教育其他有组织形式学时";
                                        break;
                                }
                            }
                            else
                            {
                                name = "内部培训其他有组织形式学时";
                            }
                            break;
                    }

                    SendMailToApplyUser(model.ApplyUserID, model.ApplyTime, model.Year, name, DepApprove);
                    if (DepApprove == 2)
                    {
                        SendMailToManage(single.ApproveUserID, model.ApplyUserID, model.ApplyTime, model.Year, name);
                    }
                    #endregion
                }



                return Json(new { result = 1, Content = "审批成功", ID = addID }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 1, Content = "审批失败", ID = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 证明资料被查看过之后的方法，防止ID被刷新掉
        /// </summary>
        /// <param name="ApproveID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult GetNewApprove(int ApproveID, int ApproveType)
        {
            try
            {

                var list = freeBL.GetApproveListByID(ApproveID, ApproveType, 2).Where(p => p.DepApprove == 0);

                var ID = list.Count() == 0 ? 0 : list.FirstOrDefault().ID;
                return Json(new { result = 1, ID = ID }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, lookFile = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 免修审批
        /// <summary>
        /// 免修审批
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_FreeApprove()
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.yearList = freeBL.GetYearList();
            ViewBag.IsTimeIn = FreeIsTimeIn(61, 0);
            return View();
        }



        /// <summary>
        /// 其他形式审批详情
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ApplyUserID"></param>
        /// <param name="type">1 其他形式 2 免修 3 授课人</param>
        /// <returns></returns>
        public ActionResult Free_FreeApproveDetails(int ID, int ApplyUserID, int ApproveStatus, int DepApproveStatus)
        {
            ViewBag.ID = ID;

            ViewBag.ApplyUserID = ApplyUserID;
            ViewBag.ApproveStatus = ApproveStatus;
            ViewBag.DepApproveStatus = DepApproveStatus;

            var ApproveUser = freeBL.GetFreeSingleByID(ID);
            ApproveUser.ApplyFileList = freeBL.GetApplyFile(ID);


            if (ApproveStatus == 1)
            {
                ///最后一次分所审批通过的状态
                var MaxBatchmodel = freeBL.GetMaxDepApproveBatch(ID, " type=1 and  ApproveType=2 and DepApprove=1");
                ViewBag.MaxBatch = MaxBatchmodel.CheackBatch;
                ViewBag.Free_ApporveID = MaxBatchmodel.ID;
            }

            return View(ApproveUser);
        }

        /// <summary>
        /// 获取免修审批
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFreeApproveList(string realName = "", string ApplyContent = "", string Start_ApplyTime = "", string End_ApplyTime = "",
            string TrainGrade = "", int Year = -1, int isCommit = -1, int ApproveStatusDep = -1, int ApproveStatus = -1,
            int pageSize = 10, int pageIndex = 1, string jsRenderSortField = "ApplyTime desc")
        {
            try
            {
                int totalcount = 0;
                string where = " 1=1 and (typeForm=0 )";

                #region 条件
                if (!string.IsNullOrEmpty(realName))
                {
                    where += " and Realname like '%" + realName.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(ApplyContent))
                {
                    where += " and FreeName like '%" + ApplyContent.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(Start_ApplyTime))
                {
                    where += " and ApplyTime >='" + Start_ApplyTime + "'";
                }
                if (!string.IsNullOrEmpty(End_ApplyTime))
                {
                    where += " and ApplyTime<='" + End_ApplyTime + "'";
                }
                if (!string.IsNullOrEmpty(TrainGrade))
                {
                    where += " and TrainGrade='" + TrainGrade + "'";
                }
                if (Year != -1)
                {
                    where += " and Year=" + Year;
                }
                if (isCommit != -1)
                {
                    if (isCommit == 1)
                    {
                        where += " and isCommit>0";
                    }
                    else
                    {
                        where += " and isCommit=" + isCommit;
                    }
                }
                if (ApproveStatusDep != -1)
                {
                    if (ApproveStatusDep == 3)
                    {
                        where += " and ApproveStatus=1 and DepApproveStatus=2";
                    }
                    else if (ApproveStatusDep == 1)
                    {
                        where += " and ApproveStatus=1 and (DepApproveStatus=0 or DepApproveStatus=1)";
                    }
                    else
                    {
                        where += " and ApproveStatus=" + ApproveStatusDep;
                    }
                }
                if (ApproveStatus != -1)//总所审批退回
                {
                    if (ApproveStatus == 0)
                    {
                        where += " and ApproveStatus=1 and  DepApproveStatus=0";
                    }
                    else
                    {
                        where += " and DepApproveStatus=" + ApproveStatus;
                    }

                }
                #endregion

                var list = freeBL.GetDepFreeApproveList(out totalcount, CurrentUser.UserId, where, pageIndex, pageSize, jsRenderSortField);
                return Json(new { result = 1, dataList = list, recordCount = totalcount }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, dataList = new List<ShowFreeDetails>(), recordCount = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 其他有组织形式
        /// <summary>
        /// 其他有组织形式列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_OtherOrg()
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.yearList = freeBL.GetOrgYearList();
            ViewBag.IsTimeIn = FreeIsTimeIn(61, 2);
            return View();
        }


        /// <summary>
        /// 其他形式审批详情
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ApplyUserID"></param>
        /// <returns></returns>
        public ActionResult Free_OtherOrgDetails(int ID, int ApplyUserID, int ApproveStatus, int DepApproveStatus)
        {
            ViewBag.ID = ID;

            ViewBag.ApplyUserID = ApplyUserID;
            ViewBag.ApproveStatus = ApproveStatus;
            ViewBag.DepApproveStatus = DepApproveStatus;

            var ApproveUser = freeBL.GetOrgSingleByID(ID);
            ApproveUser.ApplyFileList = freeBL.GetApplyFile(ID, 1);

            if (ApproveStatus == 1)
            {
                ///最后一次分所审批通过的状态
                var MaxBatchmodel = freeBL.GetMaxDepApproveBatch(ID, " type=1 and ApproveType=3 and DepApprove=1");
                ViewBag.Free_ApporveID = MaxBatchmodel.ID;
                ViewBag.MaxBatch = MaxBatchmodel.CheackBatch;
            }

            return View(ApproveUser);
        }


        /// <summary>
        /// 获取其他有组织形式
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOrgApproveList(string realName = "", string ApplyContent = "", string Start_ApplyTime = "", string End_ApplyTime = "",
            string TrainGrade = "", int Year = -1, int isCommit = -1, int ApproveStatusDep = -1, int ApproveStatus = -1,
            int pageSize = 10, int pageIndex = 1, string jsRenderSortField = "ApplyDateTime desc")
        {
            try
            {
                int totalcount = 0;
                string where = " 1=1";

                #region 条件
                if (!string.IsNullOrEmpty(realName))
                {
                    where += " and Realname like '%" + realName.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(ApplyContent))
                {
                    where += " and CourseName like '%" + ApplyContent.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(Start_ApplyTime))
                {
                    where += " and ApplyDateTime >='" + Start_ApplyTime + "'";
                }
                if (!string.IsNullOrEmpty(End_ApplyTime))
                {
                    where += " and ApplyDateTime<='" + End_ApplyTime + "'";
                }
                if (!string.IsNullOrEmpty(TrainGrade))
                {
                    where += " and TrainGrade='" + TrainGrade + "'";
                }
                if (Year != -1)
                {
                    where += " and Year=" + Year;
                }
                if (isCommit != -1)
                {
                    if (isCommit == 1)
                    {
                        where += " and isCommit>0";
                    }
                    else
                    {
                        where += " and isCommit=" + isCommit;
                    }
                }
                if (ApproveStatusDep != -1)
                {
                    if (ApproveStatusDep == 3)
                    {
                        where += " and ApproveStatus=1 and DepApproveStatus=2";
                    }
                    else if (ApproveStatusDep == 1)
                    {
                        where += " and ApproveStatus=1 and (DepApproveStatus=0 or DepApproveStatus=1)";
                    }
                    else
                    {
                        where += " and ApproveStatus=" + ApproveStatusDep;
                    }
                }
                if (ApproveStatus != -1)//总所审批退回
                {
                    if (ApproveStatus == 0)
                    {
                        where += " and ApproveStatus=1 and  DepApproveStatus=0";
                    }
                    else
                    {
                        where += " and DepApproveStatus=" + ApproveStatus;
                    }

                }
                #endregion

                var list = freeBL.GetDepFreeOrgList(out totalcount, CurrentUser.UserId, where, pageIndex, pageSize, jsRenderSortField);
                return Json(new { result = 1, dataList = list, recordCount = totalcount }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, dataList = new List<ShowFreeDetails>(), recordCount = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 公共

        /// <summary>
        /// 发送邮件给人员
        /// </summary>
        /// <param name="applyTime"></param>
        /// <param name="year"></param>
        /// <param name="name"></param>
        /// <param name="type">1 通过 2不通过</param>
        public void SendMailToApplyUser(int userID, DateTime applytime, int year, string name, int type)
        {
            var model = (new UserBL()).Get(userID);
            var content = type == 1 ? GetFormworkContent(17) : GetFormworkContent(18);
            var emailList = new List<KeyValuePair<string, string>>();
            var c = string.Format(content, model.Realname, applytime.ToString("yyyy年MM月dd日"), year, name);
            emailList.Add(new KeyValuePair<string, string>(model.Email, c));
            if (emailList.Count > 0)
                SendEmail(emailList, type == 1 ? "您的申请通过了，请查看！" : "对不起，你的申请被退回了，请查看！");
        }

        /// <summary>
        /// 发送邮件给管理员
        /// </summary>
        public void SendMailToManage(int ApproveUserID, int userID, DateTime applyTime, int year, string name)
        {
            var content = GetFormworkContent(19);
            var userModel = (new UserBL()).Get(ApproveUserID);
            var userList = freeBL.GetManageUserByDeptID(userModel.DeptId, userModel.LeaderID);
            var user = (new UserBL()).Get(userID);
            var emailList = new List<KeyValuePair<string, string>>();
            foreach (var ApproveUser in userList)
            {

                var c = string.Format(content, ApproveUser.Realname, CurrentUser.Realname, DateTime.Now.ToString("yyyy年MM月dd日"), user.Realname,
                        year, name);
                emailList.Add(new KeyValuePair<string, string>(ApproveUser.Email, c));
            }

            if (emailList.Count > 0)
                SendEmail(emailList, "有个审批被退回了，请查看！");
        }

        /// <summary>
        /// 授权审批人
        /// </summary>
        /// <param name="createName"></param>
        /// <param name="Realname"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetDeptApproveUserList(string createName = "", string Realname = "", int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                var where = " 1=1";
                if (!string.IsNullOrEmpty(createName))
                {
                    where += " and createName like '%" + createName.ReplaceSingleSql() + "%'";
                }
                if (!string.IsNullOrEmpty(Realname))
                {
                    where += " and Realname like '%" + Realname.ReplaceSingleSql() + "%'";
                }
                var list = freeBL.GetDeptApproveUserList(where, pageIndex, pageSize);
                var totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
                return Json(new { result = 1, dataList = list, recordCount = totalCount }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, dataList = new List<Free_DepApprove>(), recordCount = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Free_ApproveUserManage()
        {
            return View();
        }

        #endregion

    }
}
