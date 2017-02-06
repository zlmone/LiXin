using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinModels;
using LiXinInterface.MyApproval;
using LiXinModels.SystemManage;
using LiXinModels.MyApproval;
using System.IO;
using System.Web;
using Novacode;
using System.Drawing;
using System.Configuration;
using LiXinBLL.User;

namespace LiXinControllers
{
    public partial class MyApprovalController : BaseController
    {
        #region 授权审批人
        public ActionResult Free_ApproveUserManage()
        {
            return View();
        }

        public JsonResult GetDepApproveList(string realName = "", string DeptName = "", int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalcount = 0;
                string where = "1=1";
                if (!string.IsNullOrEmpty(realName))
                {
                    where += " and Realname like '%" + realName.ReplaceSql() + "%'";
                }
                if (!string.IsNullOrEmpty(DeptName))
                {
                    where += " and DeptPath like '%" + DeptName.ReplaceSql() + "%'";
                }

                var List = freeBL.GetDepApproveList(out totalcount, CurrentUser.JobNum, where, pageIndex, pageSize);
                List.ForEach(p => p.ManageDeptName = p.ManageDeptName.TrimEnd(','));
                return Json(new { result = 1, dataList = List, recordCount = totalcount }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, dataList = new List<ShowDepApprove>(), recordCount = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 添加审批人
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="IsManage"></param>
        /// <param name="selectDept"></param>
        /// <returns></returns>
        public JsonResult AddApproveUser(int userID, int IsManage, string selectDept)
        {
            try
            {

                var single = freeBL.GetModel(" UserID=" + userID);
                var model = new Free_DepApprove();
                model.UserID = userID;
                model.ManageDeptIDs = selectDept;
                model.CreateUserID = CurrentUser.UserId;
                model.CreateTime = DateTime.Now;
                model.LeardID = CurrentUser.JobNum;
                freeBL.InsertFree_DepApprove(model);


                return Json(new { result = 1, Content = "指定成功" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, Content = "指定失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除审批人
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public JsonResult DeleteApprove(int ID)
        {
            try
            {
                freeBL.DeleteFree_DepApprove(ID.ToString());
                return Json(new { result = 1, Content = "撤销指定成功" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, Content = "撤销指定失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 其他形式审批
        public ActionResult Free_Approve()
        {
            return View();
        }

        /// <summary>
        /// 其他形式审批
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_OtherApprove()
        {
            ViewBag.TrainGrade = AllTrainGrade;
            ViewBag.yearList = freeBL.GetYearList(" Status=1");
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

            var ApproveUser = freeBL.GetSingleByID(ID, type);
            ApproveUser.ApplyFileList = freeBL.GetApplyFile(ID);
            if (type == 3)
            {
                if (ApproveUser.ApplyType == 3)
                {
                    ApproveUser.teacherDetails = teacherDetailsBL.GetFree_UserApplyTeacherDetailsList(" IsDelete=0 and userApplyID=" + ID);
                }
            }


            ///最后一次拒绝的状态，如果没有则返回一个空的对象，此方法用来进行批次的增加
            var MaxBatchmodel = freeBL.GetMaxDepApproveBatch(ID, " type=1 and  ApproveType=1 and DepApprove=2");
            ViewBag.MaxBatch = MaxBatchmodel.ID == 0 ? 0 : MaxBatchmodel.CheackBatch;
            // ViewBag.DepReason = MaxBatchmodel.DepReason;
            return View(ApproveUser);
        }

        /// <summary>
        /// 审批流程图
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="type">1 其他形式 2 免修 3其他形式项目</param>
        /// <returns></returns>
        public ActionResult Free_ApproveHead(int ID, string Realname, string ApplyTimeStr, int type = 1)
        {
            ViewBag.Realname = Realname;
            ViewBag.ApplyTimeStr = ApplyTimeStr;
            ViewBag.ApproveList = freeBL.GetApproveListByID(ID, type, -1);
            return View();
        }

        /// <summary>
        /// 审批弹出框
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_ApprovePop()
        {
            return View();
        }

        /// <summary>
        /// 退回弹出框
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_ApproveRejectPop()
        {
            return View();
        }

        /// <summary>
        /// 其他形式申请导出Word
        /// </summary>
        /// <param name="type">1 其他形式  3 授课人</param>
        /// <returns></returns>
        public ActionResult Free_OtherWord(int ID, int type)
        {
            ViewBag.ID = ID;
            ViewBag.type = type;
            return View();
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
                string where = " 1=1  and (typeForm=0 )";

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
                        where += " and ApproveStatus=1 and DepApproveStatus!=2";
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
                var ManageDeparts = CurrentUser.TrainMaster == 1 ? CurrentUser.ManageDeparts : "";

                var list = freeBL.GetOtherApproveList(out totalcount, CurrentUser.UserId, ManageDeparts, where, pageIndex, pageSize, jsRenderSortField);
                return Json(new { result = 1, dataList = list, recordCount = totalcount }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, dataList = new List<ShowFreeDetails>(), recordCount = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 分所 下载或者点击查看按钮是第一次讲审批内容初始化
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveDepTrainApprove(int ID, int MaxBatch, int ApproveID, int ApproveType, int DepTrainApprove = 0, string DepTrainReason = "", int type = 0)
        {
            try
            {
                var model = new LiXinModels.MyApproval.Free_Apporve();
                if (ID == 0)
                {

                    var where = string.Format(" type=1 and  userApplyID={0} AND CheackBatch={1}  AND ApproveType={2}"
                         , ApproveID, MaxBatch + 1, ApproveType);
                    var list = freeBL.GetFreeApporve(where);
                    if (list.Count() == 0)
                    {
                        model.DepApprove = 0;
                        model.DepReason = "";
                        model.userApplyID = ApproveID;
                        model.CheackBatch = MaxBatch + 1;
                        model.type = 1;
                        model.ApproveUserID = CurrentUser.UserId;
                        model.ApproveTime = DateTime.Now;
                        model.ApproveType = ApproveType;
                        model.LookFile = 1;
                        freeBL.InsertFree_Apporve(model);
                    }
                    else
                    {
                        model.ID = list.FirstOrDefault().ID;
                    }

                }
                else
                {
                    var updateID = 0;
                    var single = freeBL.GetFreeApporve(" ID=" + ID).FirstOrDefault();
                    single.DepApprove = DepTrainApprove;
                    single.DepReason = DepTrainReason;
                    single.ApproveUserID = CurrentUser.UserId;
                    single.ApproveTime = DateTime.Now;
                    freeBL.UpdateFree_Apporve(single);
                    updateID = single.userApplyID;

                    ///其他有组织形式
                    if (ApproveType == 3)
                    {
                        iFree_UserApplyOtherForm.UpdateBywhere(updateID, " ApproveStatus=" + DepTrainApprove + ", DepTrainReason='" + DepTrainReason + "'");
                    }
                    else
                    {
                        freeBL.UpdateUserApply(updateID, " ApproveStatus=" + DepTrainApprove + ", DepTrainReason='" + DepTrainReason + "'");
                    }

                    #region 发邮件

                    var approveModel = new ShowFreeDetails();
                    var name = "";
                    switch (type)
                    {
                        case 0:
                            approveModel = freeBL.GetSingleByID(ApproveID, -1);
                            if (approveModel.CPA == "是")
                            {
                                switch (approveModel.ConvertType)
                                {
                                    case 0:
                                        name = "内部培训其他形式学时";
                                        break;
                                    case 1:
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
                            approveModel = freeBL.GetFreeSingleByID(ApproveID);
                            if (approveModel.CPA == "是")
                            {
                                switch (approveModel.ApplyType)
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
                            approveModel = freeBL.GetOrgSingleByID(ApproveID);
                            if (approveModel.CPA == "是")
                            {
                                switch (approveModel.OtherFromID)
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
                    SendMailToApplyUser(approveModel.ApplyUserID, approveModel.ApplyTime, approveModel.Year, name, DepTrainApprove);
                    if (DepTrainApprove == 1)
                    {
                        SendMailToManage(approveModel.ApplyUserID, approveModel.ApplyTime, approveModel.Year, name);
                    }
                    #endregion
                }
                return Json(new { result = 1, Content = "审批成功", ID = model.ID }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, Content = "审批失败", ID = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 证明资料被查看过之后的方法，防止ID被刷新掉
        /// </summary>
        /// <param name="ApproveID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult GetNewApprove(int ApproveID, int type)
        {
            try
            {
                var list = freeBL.GetApproveListByID(ApproveID, type).Where(p => p.DepApprove == 0);

                var ID = list.Count() == 0 ? 0 : list.FirstOrDefault().ID;
                return Json(new { result = 1, ID = ID }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, ID = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 部门负责人退回 需要重新一条数据，因为人可能会不一样
        /// </summary>
        /// <param name="type">1 其他形式 2 免修  3 其他有组织形式</param>
        /// <returns></returns>
        public JsonResult ApproveReject(int ApproveID, int type = 1)
        {
            try
            {

                // 最后一次通过的状态
                var model = freeBL.GetMaxDepApproveBatch(ApproveID, " type=1 and ApproveType=" + type + " and DepApprove=1");
                model.DepApprove = 2;

                model.ApproveUserID = CurrentUser.UserId;
                model.ApproveTime = DateTime.Now;
                freeBL.InsertFree_Apporve(model);
                if (type == 3)
                {
                    iFree_UserApplyOtherForm.UpdateBywhere(ApproveID, " ApproveStatus=2");
                }
                else
                {
                    freeBL.UpdateUserApply(ApproveID, " ApproveStatus=2");
                }

                #region 发邮件
                var approveModel = new ShowFreeDetails();
                var name = "";
                switch (type)
                {
                    case 1:
                        approveModel = freeBL.GetSingleByID(ApproveID, -1);

                        switch (approveModel.ConvertType)
                        {
                            case 0:
                                name = "内部培训其他形式学时";
                                break;
                            case 1:
                                name = "CPA继续教育其他形式学时";
                                break;
                            case 2:
                                name = "内部培训/CPA继续教育其他形式学时";
                                break;
                        }

                        break;
                    case 2:
                        approveModel = freeBL.GetFreeSingleByID(ApproveID);

                        switch (approveModel.ApplyType)
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

                        break;
                    case 3:
                        approveModel = freeBL.GetOrgSingleByID(ApproveID);

                        switch (approveModel.OtherFromID)
                        {
                            case 0:
                                name = "内部培训其他有组织形式学时";
                                break;
                            case 1:
                                name = "CPA继续教育其他有组织形式学时";
                                break;
                            case 2:
                                name = "内部培训/CPA继续教育其他有组织形式学时";
                                break;
                        }

                        break;
                }
                SendMailToApplyUser(approveModel.ApplyUserID, approveModel.ApplyTime, approveModel.Year, name, 2);

                #endregion

                return Json(new { result = 1, Content = "退回成功" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, Content = "退回失败" }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 其他形式导出Word
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="type">1 其他形式  3 授课人</param>
        /// <param name="CPA">0 非CPA 1 CPA</param>
        /// <returns></returns>
        public void OtherToWord(int ID, int type, int CPA = 0)
        {
            var ApproveUser = freeBL.GetSingleByID(ID, type);
            if (type == 3)
            {
                if (ApproveUser.ApplyType == 3)
                {
                    ApproveUser.teacherDetails = teacherDetailsBL.GetFree_UserApplyTeacherDetailsList(" IsDelete=0 and userApplyID=" + ID);
                }
            }
            int totalcount = 0;
            var otherList = FreeConfigBL.GetFreeOtherList(out totalcount, " ApplyType=1 and Year="+ApproveUser.Year);

            GenerateWord word = new GenerateWord();

            #region 方法比较挫，后人有机会再改吧
            var css = @"
                        table { border-collapse: collapse; border-spacing: 0;}
                        #otherWord td { border: 1px solid; }

                        #otherWord { text-align: center; height: 620px; width: 627px; font-family:Arial,FangSong_GB2312; font-size: 14px}
                        
                        .w18 td { width: 185px; height: 56px; }

                        .w30 td { width: 288px; height: 100px; }

                        #joinInfo td { border: 0 none; }";
            #endregion
          
            var name = "其他形式培训学时申请表-" + ApproveUser.Realname + (ApproveUser.CPA == "是" ? ApproveUser.CPANo : "") + ".doc";
            word.ExportGenerateWord(HttpContext, css, GetOtherApproveTable(ApproveUser, otherList, CPA), name);

        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="ApproveUser"></param>
        /// <param name="CPA">0 非CPA 1 CPA</param>
        /// <returns></returns>
        public string GetOtherApproveTable(ShowFreeDetails ApproveUser, List<Free_OtherApplyConfig> otherList, int CPA = 0)
        {
            var Content = "";
            var CPAContent = "";
            var Memo = "";
            var name = "";
            var table = new StringBuilder();

            var ApproveReson = ApproveUser.ApproveStatus == 1 ? "同意" : ApproveUser.DepTrainReason;
            var DepApproveReson = ApproveUser.DepApproveStatus == 1 ? "同意" : ApproveUser.DepReason;

            var otherItemStr = new StringBuilder();

            for (int i = 0; i < otherList.Count(); i++)
            {
                otherItemStr.Append("(" + (i + 1) + ") " + otherList[i].ApplyContent + "<br/>");
            }
            if (ApproveUser.ApplyType == 1 || ApproveUser.ApplyType == 3)
            {
                if (CPA == 1)
                {
                    name = "注册会计师继续教育学时确认申请表";
                }
                else
                {
                    name = "我所内部培训－其他形式培训学时申请表";
                }
            }
            if (ApproveUser.ApplyType == 3)
            {

                int i = 1;
                foreach (var item in ApproveUser.teacherDetails)
                {
                    Content += string.Format(@"{0}、{1} 所内学时：{2} {3} 情况：{4}<br/>",
                        i, item.ClassName, item.tScore, (CPA == 1 ? "CPA学时：" + item.CPAScore : ""), ApproveUser.Memo);
                    CPAContent += string.Format(@"{0}、{1} 所内学时：{2} {3} <br/>",
                        i, item.ClassName, item.tScore, (CPA == 1 ? "CPA学时：" + item.CPAScore : ""));
                    Memo += string.Format(@"{0}、情况：{1} ", i, ApproveUser.Memo);
                    i++;
                }
            }
            else
            {
                Content = "1、" + ApproveUser.ApplyContent + " 所内学时：" + ApproveUser.tScore + "情况：" + ApproveUser.Memo;
                CPAContent = "1、" + ApproveUser.ApplyContent + (CPA == 1 ? "CPA学时：" + ApproveUser.CPAScore : "");
                Memo += string.Format(@"1、情况：{0} ", ApproveUser.Memo);
            }
            if (CPA == 1)
            {

                #region CPA

                table.AppendFormat(@"<div style='text-align: center'>
    <h3>{12}</h3>
</div>
<table border=0 cellspacing=0 cellpadding=0 width=634
 style='width:475.5pt;border-collapse:collapse;mso-yfti-tbllook:1184;font-family:Arial,FangSong_GB2312' id='otherWord'>
    <tr class='w18'>
        <td>姓名</td>
        <td>{0}</td>
        <td>性别</td>
        <td>{1}</td>
        <td>CPA证书号码</td>
        <td>{2}</td>
    </tr>
    <tr class='w18'  style='text-align: left;'>
         <td>所在会计师事务所</td>
        <td colspan='5'>立信会计师事务所（特殊普通合伙）<br/>{3}</td>
    </tr>
    <tr class='w18'  style='text-align: left;'>
        <td>申请确认的学时数</td>
        <td colspan='5'>{4}</td>
    </tr>
    <tr style='height: 56px'  style='text-align: left;'>
        <td>参加继续教育的形式</td>
        <td colspan='5'>{5}</td>
    </tr>
    <tr>
        <td colspan='6' style='border-bottom: 0 none'>
            <div style='float: left; padding-right: 10px;text-align: left;text-align: left;'>参加其他形式培训情况说明:</div>
        </td>
    </tr>
    <tr style='height: 200px'>
        <td colspan='6' style='border-top: 0 none; border-bottom: 0 none;vertical-align: top;text-align: left;'>{6}
        </td>
    </tr>
    <tr>
        <td colspan='6' style='border-top: 0 none'>
            <div style='float: right; padding-right: 10px;text-align: right;'>
                <div>申请人：{7}</div>
                {8}
            </div>

        </td>
    </tr>
    <tr class='w30'>
        <td colspan='3'>所在会计师事务所意见:<br/>{9}</td>
        <td colspan='3'>地方协会意见:<br/>{10}</td>
    </tr>
    <tr>
        <td colspan='6' style='border: 0 none;'><div style='text-align: left;'>
        说明：1.本申请表中，经教育培训部确认的学时数可计入当年内部培训年度目标学时。<br/>
             2.“其他形式培训”包括：<br/>
               {11}<br/>
             3.申请人提交本申请表时，请一并报送相关证明材料
        </div>
        </td>
    </tr>
</table>", ApproveUser.Realname, ApproveUser.SexStr, ApproveUser.CPANo, (ApproveUser.SubordinateSubstation.Contains("上海") ? "" : ApproveUser.DeptName),
         ApproveUser.CPAScore, CPAContent, Memo, ApproveUser.Realname, ApproveUser.ApplyTime.ToString("yyyy年MM月dd日"), ApproveReson, "", otherItemStr.ToString(), name);

                #endregion
            }
            else
            {
                #region NoCPA

                table.AppendFormat(@"<div style='text-align: center'>
    <h3>{13}</h3>
</div>
<table border=0 cellspacing=0 cellpadding=0 width=634
 style='width:475.5pt;border-collapse:collapse;mso-yfti-tbllook:1184;font-family:Arial,FangSong_GB2312' id='otherWord'>
    <tr class='w18'>
        <td>姓名</td>
        <td>{0}</td>
        <td>性别</td>
        <td>{1}</td>
    </tr>
    <tr class='w18'>
        <td>所在部门</td>
        <td>{2}</td>
        <td>培训级别</td>
        <td>{3}</td>
    </tr>
    <tr class='w18'>
        <td>申请年度</td>
        <td>{4}</td>
        <td>联系电话</td>
        <td>{5}</td>
    </tr>
    <tr style='height: 56px'  style='text-align: left;'>
        <td>申请确认的学时数</td>
        <td colspan='3'>{6}</td>
    </tr>
    <tr>
        <td colspan='4' style='border-bottom: 0 none'>
            <div style='float: left; padding-right: 10px;text-align: left;text-align: left;'>参加其他形式培训情况说明:</div>
        </td>
    </tr>
    <tr style='height: 200px'>
        <td colspan='4' style='border-top: 0 none; border-bottom: 0 none;vertical-align: top;text-align: left;'>{7}
        </td>
    </tr>
    <tr>
        <td colspan='4' style='border-top: 0 none'>
            <div style='float: right; padding-right: 10px;text-align: right;'>
                <div>申请人：{8}</div>
                {9}
            </div>

        </td>
    </tr>
    <tr  class='w30'>
        <td colspan='2'>部门负责人意见:<br/>{10}</td>
        <td colspan='2'>教育培训部意见:<br/>{11}</td>
    </tr>
    <tr>
        <td colspan='4' style='border: 0 none;'><div style='text-align: left;'>
            说明：1.本申请表中，经教育培训部确认的学时数可计入当年内部培训年度目标学时。<br/>
                 2.“其他形式培训”包括：<br/>
                    {12}<br/>
                 3.申请人提交本申请表时，请一并报送相关证明材料
        </div>
        </td>
    </tr>
</table>", ApproveUser.Realname, ApproveUser.SexStr, ApproveUser.DeptName, ApproveUser.TrainGrade, ApproveUser.Year.ToString().GetCapitalByYear() + "年", ApproveUser.MobileNum
             , ApproveUser.tScore, Content, ApproveUser.Realname, ApproveUser.ApplyTime.ToString("yyyy年MM月dd日"), ApproveReson, DepApproveReson, otherItemStr.ToString(), name);
                #endregion
            }

            return table.ToString();

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
            ViewBag.IsTimeIn = FreeIsTimeIn(61, 1);
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


            ///最后一次拒绝的状态，如果没有则返回一个空的对象，此方法用来进行批次的增加
            var MaxBatchmodel = freeBL.GetMaxDepApproveBatch(ID, "  type=1 and  ApproveType=2 and DepApprove=2");
            ViewBag.MaxBatch = MaxBatchmodel.CheackBatch;
            ViewBag.DepReason = MaxBatchmodel.DepReason;
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
                        where += " and ApproveStatus=1 and DepApproveStatus!=2";
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

                var list = freeBL.GetFreeApproveList(out totalcount, CurrentUser.UserId, CurrentUser.ManageDeparts, where, pageIndex, pageSize, jsRenderSortField);
                return Json(new { result = 1, dataList = list, recordCount = totalcount }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, dataList = new List<ShowFreeDetails>(), recordCount = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 其他形式导出Word
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="type">1 其他形式  3 授课人</param>
        /// <param name="CPA">0 非CPA 1 CPA</param>
        /// <returns></returns>
        public void FreeToWord(int ID, int type, int CPA = 0)
        {
            var ApproveUser = freeBL.GetFreeSingleByID(ID);
            if (type == 3)
            {
                if (ApproveUser.ApplyType == 3)
                {
                    ApproveUser.teacherDetails = teacherDetailsBL.GetFree_UserApplyTeacherDetailsList(" userApplyID=" + ID);
                }
            }
            int totalcount = 0;
            var otherList = FreeConfigBL.GetFreeApplyList(out totalcount ," Year="+ApproveUser.Year);

            GenerateWord word = new GenerateWord();
            var css = @"table { border-collapse: collapse; border-spacing: 0; }
                        #otherWord td { border: 1px solid; }

                        #otherWord { text-align: center; height: 620px; width: 627px; font-family:Arial,FangSong_GB2312; font-size: 14px}
                        
                        .w18 td { width: 182px; height: 56px; }

                        .w30 td { width: 288px; height: 100px; }

                        #joinInfo td { border: 0 none; }";
            var name = "免修申请表-" + ApproveUser.Realname + (ApproveUser.CPA == "是" ? ApproveUser.CPANo : "") + ".doc";
            word.ExportGenerateWord(HttpContext, css, GetFreeApproveTable(ApproveUser, otherList, CPA), name);

        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="ApproveUser"></param>
        /// <param name="CPA">0 非CPA 1 CPA</param>
        /// <returns></returns>
        public string GetFreeApproveTable(ShowFreeDetails ApproveUser, List<Free_ApplyConfig> otherList, int CPA = 0)
        {
            var Content = "";
            var CPAContent = "";
            var Memo = "";
            var name = "";
            var table = new StringBuilder();

            var ApproveReson = ApproveUser.ApproveStatus == 1 ? "同意" : ApproveUser.DepTrainReason;
            var DepApproveReson = ApproveUser.DepApproveStatus == 1 ? "同意" : ApproveUser.DepReason;

            var otherItemStr = new StringBuilder();

            for (int i = 0; i < otherList.Count(); i++)
            {
                otherItemStr.Append(i + 1 + "、" + otherList[i].FreeName + "<br/>");
            }
            if (ApproveUser.ApplyType == 2)
            {
                if (CPA == 1)
                {
                    name = "注册会计师继续教育免修学时申请表";
                }
                else
                {
                    name = "免修所内学时申请表";
                }
            }

            Content = "1、" + ApproveUser.FreeName + "<br/>2、情况：" + ApproveUser.Memo;


            if (CPA == 1)
            {

                #region CPA

                table.AppendFormat(@"<div style='text-align: center'>
    <h3>{11}</h3>
</div>
<table border=0 cellspacing=0 cellpadding=0 width=634
 style='width:475.5pt;border-collapse:collapse;mso-yfti-tbllook:1184;font-family:Arial,FangSong_GB2312' id='otherWord'>
    <tr class='w18'>
        <td>姓名</td>
        <td>{0}</td>
        <td>性别</td>
        <td>{1}</td>
        <td>CPA证书号码</td>
        <td>{2}</td>
    </tr>
    <tr class='w18'  style='text-align: left;'>
         <td>所在会计师事务所</td>
        <td colspan='5'>立信会计师事务所（特殊普通合伙）<br/>{3}</td>
    </tr>
    <tr class='w18'  style='text-align: left;'>
        <td>申请免修的学时数</td>
        <td colspan='5'>{4}</td>
    </tr>
   
    <tr>
        <td colspan='6' style='border-bottom: 0 none'>
            <div style='float: left; padding-right: 10px;text-align: left;text-align: left;'>参加其他形式培训情况说明:</div>
        </td>
    </tr>
    <tr style='height: 200px'>
        <td colspan='6' style='border-top: 0 none; border-bottom: 0 none;vertical-align: top;text-align: left;'>{5}
        </td>
    </tr>
    <tr>
        <td colspan='6' style='border-top: 0 none'>
            <div style='float: right; padding-right: 10px;text-align: right;'>
                <div>申请人：{6}</div>
                {7}
            </div>

        </td>
    </tr>
   <tr  class='w30'>
        <td colspan='3'>所在会计师事务所意见:<br/>{8}</td>
        <td colspan='3'>地方协会意见:<br/>{9}</td>
    </tr>
    <tr>
        <td colspan='6' style='border: 0 none;'><div style='text-align: left;'>
            说明：1.本申请表中，经地方协会确认的学时数可免当年继续教育学时。<br/>
                 2.可免修情况包括：<br/>
                   {10}<br/>
                 3.申请人提交本申请表时，请一并报送相关证明材料
        </div>
        </td>
    </tr>
</table>", ApproveUser.Realname, ApproveUser.SexStr, ApproveUser.CPANo, (ApproveUser.SubordinateSubstation.Contains("上海") ? "" : ApproveUser.DeptName),
         ApproveUser.CPAScore, Content, ApproveUser.Realname, ApproveUser.ApplyTime.ToString("yyyy年MM月dd日"), ApproveReson, "", otherItemStr.ToString(), name);

                #endregion
            }
            else
            {
                #region NoCPA

                table.AppendFormat(@"<div style='text-align: center'>
    <h3>{11}</h3>
</div>
<table border=0 cellspacing=0 cellpadding=0 width=634
 style='width:475.5pt;border-collapse:collapse;mso-yfti-tbllook:1184;font-family:Arial,FangSong_GB2312' id='otherWord'>
    <tr class='w18'>
        <td>姓名</td>
        <td>{0}</td>
        <td>性别</td>
        <td>{1}</td>
    </tr>
    <tr class='w18'>
        <td>所在部门</td>
        <td>{2}</td>
        <td>培训级别</td>
        <td>{3}</td>
    </tr>
    
    <tr style='height: 56px'  style='text-align: left;'>
        <td>申请免修的学时数</td>
        <td colspan='3'>{4}</td>
    </tr>
    <tr>
        <td colspan='4' style='border-bottom: 0 none'>
            <div style='float: left; padding-right: 10px;text-align: left;text-align: left;'>参加其他形式培训情况说明:</div>
        </td>
    </tr>
    <tr style='height: 200px'>
        <td colspan='4' style='border-top: 0 none; border-bottom: 0 none;vertical-align: top;text-align: left;'>{5}
        </td>
    </tr>
    <tr>
        <td colspan='4' style='border-top: 0 none'>
            <div style='float: right; padding-right: 10px;text-align: right;'>
                <div>申请人：{6}</div>
                {7}
            </div>

        </td>
    </tr>
    <tr  class='w30'>
        <td colspan='2'>部门负责人意见:<br/>{8}</td>
        <td colspan='2'>教育培训部意见:<br/>{9}</td>
    </tr>
    <tr>
        <td colspan='4' style='border: 0 none;'><div style='text-align: left;'>
            说明：1.本申请表中，经地方协会确认的学时数可免当年继续教育学时。<br/>
                 2.可免修情况包括：<br/>
                   {10}<br/>
                 3.申请人提交本申请表时，请一并报送相关证明材料
        </div>
        </td>
    </tr>
</table>", ApproveUser.Realname, ApproveUser.SexStr, ApproveUser.DeptName, ApproveUser.TrainGrade, ApproveUser.tScore, Content,
         ApproveUser.Realname, ApproveUser.ApplyTime.ToString("yyyy年MM月dd日"), ApproveReson, DepApproveReson, otherItemStr.ToString(), name);
                #endregion
            }

            return table.ToString();

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


            ///最后一次拒绝的状态，如果没有则返回一个空的对象，此方法用来进行批次的增加
            var MaxBatchmodel = freeBL.GetMaxDepApproveBatch(ID, " type=1 and  ApproveType=3 and DepApprove=2");
            ViewBag.MaxBatch = MaxBatchmodel.CheackBatch;
            ViewBag.DepReason = MaxBatchmodel.DepReason;
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
                        where += " and ApproveStatus=1 and DepApproveStatus!=2";
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

                var list = freeBL.GetFreeOrgList(out totalcount, CurrentUser.UserId, CurrentUser.ManageDeparts, where, pageIndex, pageSize, jsRenderSortField);
                return Json(new { result = 1, dataList = list, recordCount = totalcount }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, dataList = new List<ShowFreeDetails>(), recordCount = 0 }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 其他形式导出Word
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="CPA">0 非CPA 1 CPA</param>
        /// <returns></returns>
        public void OrgToWord(int ID, int type, int CPA = 0)
        {
            var ApproveUser = freeBL.GetOrgSingleByID(ID);


            GenerateWord word = new GenerateWord();
            var css = @"table { border-collapse: collapse; border-spacing: 0; }
                        #otherWord td { border: 1px solid; }

                        #otherWord { text-align: center; height: 620px; width: 627px; font-family:Arial,FangSong_GB2312; font-size: 14px}
                        
                        .w18 td { width: 182px; height: 56px; }

                        .w30 td { width: 288px; height: 100px; }

                        #joinInfo td { border: 0 none; }";
            var name = "其他有组织形式申请表-" + ApproveUser.Realname + (ApproveUser.CPA == "是" ? ApproveUser.CPANo : "") + ".doc";
            word.ExportGenerateWord(HttpContext, css, GetOrgApproveTable(ApproveUser, CPA), name);

        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="ApproveUser"></param>
        /// <param name="CPA">0 非CPA 1 CPA</param>
        /// <returns></returns>
        public string GetOrgApproveTable(ShowFreeDetails ApproveUser, int CPA = 0)
        {
            var Content = "";
            var CPAContent = "";
            var Memo = "";
            var name = "";
            var table = new StringBuilder();

            var ApproveReson = ApproveUser.ApproveStatus == 1 ? "同意" : ApproveUser.DepTrainReason;
            var DepApproveReson = ApproveUser.DepApproveStatus == 1 ? "同意" : ApproveUser.DepReason;

            if (CPA == 1)
            {
                name = "注册会计师继续教育其他有组织形式学时申请表";
            }
            else
            {
                name = "其他有组织形式所内学时申请表";
            }


            Content = "1、课程名称:" + ApproveUser.CourseName + " 培训地点：" + ApproveUser.Address
                      + " 培训时间：" + ApproveUser.StartTimeStr + "-" + ApproveUser.EndTimeStr
                      + (CPA == 1 && ApproveUser.OtherFromID != 1 ? (" 申请CPA学时：" + ApproveUser.CPAScore) : "") + (ApproveUser.OtherFromID != 0 ? " 申请所内学时：" + ApproveUser.TogetherScore : "");


            if (CPA == 1)
            {

                #region CPA

                table.AppendFormat(@"<div style='text-align: center'>
    <h3>{10}</h3>
</div>
<table border=0 cellspacing=0 cellpadding=0 width=634
 style='width:475.5pt;border-collapse:collapse;mso-yfti-tbllook:1184;font-family:Arial,FangSong_GB2312' id='otherWord'>
    <tr class='w18'>
        <td>姓名</td>
        <td>{0}</td>
        <td>性别</td>
        <td>{1}</td>
        <td>CPA证书号码</td>
        <td>{2}</td>
    </tr>
    <tr class='w18'  style='text-align: left;'>
         <td>所在会计师事务所</td>
        <td colspan='5'>立信会计师事务所（特殊普通合伙）<br/>{3}</td>
    </tr>
    <tr class='w18'  style='text-align: left;'>
        <td>申请其他有组织形式的学时数</td>
        <td colspan='5'>{4}</td>
    </tr>
   
    <tr>
        <td colspan='6' style='border-bottom: 0 none'>
            <div style='float: left; padding-right: 10px;text-align: left;text-align: left;'>参加其他有组织形式培训情况说明:</div>
        </td>
    </tr>
    <tr style='height: 200px'>
        <td colspan='6' style='border-top: 0 none; border-bottom: 0 none;vertical-align: top;text-align: left;'>{5}
        </td>
    </tr>
    <tr>
        <td colspan='6' style='border-top: 0 none'>
            <div style='float: right; padding-right: 10px;text-align: right;'>
                <div>申请人：{6}</div>
                {7}
            </div>

        </td>
    </tr>
   <tr class='w30'>
        <td colspan='3'>所在会计师事务所意见:<br/>{8}</td>
        <td colspan='3'>地方协会意见:<br/>{9}</td>
    </tr>
    <tr>
        <td colspan='6' style='border: 0 none;'><div style='text-align: left;'>
            说明：<p>1.申请人提交本申请表时，请一并报送相关证明材料</p>
        </div>
        </td>
    </tr>
</table>", ApproveUser.Realname, ApproveUser.SexStr, ApproveUser.CPANo, (ApproveUser.SubordinateSubstation.Contains("上海") ? "" : ApproveUser.DeptName),
         ApproveUser.CPAScore, Content, ApproveUser.Realname, ApproveUser.ApplyTime.ToString("yyyy年MM月dd日"), ApproveReson, "", name);

                #endregion
            }
            else
            {
                #region NoCPA

                table.AppendFormat(@"<div style='text-align: center'>
    <h3>{10}</h3>
</div>
<table border=0 cellspacing=0 cellpadding=0 width=634
 style='width:475.5pt;border-collapse:collapse;mso-yfti-tbllook:1184;font-family:Arial,FangSong_GB2312' id='otherWord'>
    <tr class='w18'>
        <td>姓名</td>
        <td>{0}</td>
        <td>性别</td>
        <td>{1}</td>
    </tr>
    <tr class='w18'>
        <td>所在部门</td>
        <td>{2}</td>
        <td>培训级别</td>
        <td>{3}</td>
    </tr>
    
    <tr style='height: 56px'  style='text-align: left;'>
        <td>申请其他有组织形式的学时数</td>
        <td colspan='3'>{4}</td>
    </tr>
    <tr>
        <td colspan='4' style='border-bottom: 0 none'>
            <div style='float: left; padding-right: 10px;text-align: left;text-align: left;'>参加其他有组织形式培训情况说明:</div>
        </td>
    </tr>
    <tr style='height: 200px'>
        <td colspan='4' style='border-top: 0 none; border-bottom: 0 none;vertical-align: top;text-align: left;'>{5}
        </td>
    </tr>
    <tr>
        <td colspan='4' style='border-top: 0 none'>
            <div style='float: right; padding-right: 10px;text-align: right;'>
                <div>申请人：{6}</div>
                {7}
            </div>
        </td>
    </tr>
    <tr  class='w30'>
        <td colspan='2'>部门负责人意见:<br/>{8}</td>
        <td colspan='2'>教育培训部意见:<br/>{9}</td>
    </tr>
    <tr>
        <td colspan='4' style='border: 0 none;'><div style='text-align: left;'>
            说明：<p>1.申请人提交本申请表时，请一并报送相关证明材料</p>
        </div>
        </td>
    </tr>
</table>", ApproveUser.Realname, ApproveUser.SexStr, ApproveUser.DeptName, ApproveUser.TrainGrade, ApproveUser.TogetherScore, Content,
         ApproveUser.Realname, ApproveUser.ApplyTime.ToString("yyyy年MM月dd日"), ApproveReson, DepApproveReson, name);
                #endregion
            }

            return table.ToString();

        }
        #endregion

        #region 公共

        /// <summary>
        /// 文件查看部分，其他也可以调用滴哦
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_FilesView(string ResourceName, string name, string realName = "")
        {
            var type = 0;
            var swf = new List<string> { "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "pdf" };
            ViewBag.Url = ConfigurationManager.AppSettings["FreeUplpadConvertUrl"] + "/" + name;
            ViewBag.ResourceName = ResourceName.Trim();
            ViewBag.name = name;
            ViewBag.realName = realName;
            //文件后缀名
            string suffix = ResourceName.Substring(ResourceName.LastIndexOf(".") + 1).ToLower();
            if (swf.Contains(suffix))
            {
                type = 1;
            }
            else
            {
                ViewBag.Url = ConfigurationManager.AppSettings["FreeUplpadUrl"] + "/" + name;
            }
            ViewBag.type = type;

            ViewBag.flag =System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(ViewBag.Url));
            return View();
        }


        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="applyTime"></param>
        /// <param name="year"></param>
        /// <param name="name"></param>
        /// <param name="type">1 通过 2不通过</param>
        public void SendMailToApplyUser(int userID, DateTime applyTime, int year, string name, int type)
        {
            var content = type == 1 ? GetFormworkContent(14) : GetFormworkContent(16);
            var model = (new UserBL()).Get(userID);
            var emailList = new List<KeyValuePair<string, string>>();
            var c = string.Format(content, model.Realname, applyTime.ToString("yyyy年MM月dd日"), year, name, CurrentUser.Realname);
            emailList.Add(new KeyValuePair<string, string>(model.Email, c));
            if (emailList.Count > 0)
                SendEmail(emailList, type == 1 ? "您的申请通过了，请查看！" : "对不起，你的申请被退回了，请查看！");
        }

        public void SendMailToManage(int userID, DateTime applyTime, int year, string name)
        {

            var content = GetFormworkContent(15);
            var user = (new UserBL()).Get(userID);
            var model = freeBL.GetManageUser();
            var emailList = new List<KeyValuePair<string, string>>();
            var c = string.Format(content, CurrentUser.DeptName, CurrentUser.Realname, DateTime.Now.ToString("yyyy年MM月dd日"), user.Realname,
                year, name);
            foreach (var item in model)
            {
                emailList.Add(new KeyValuePair<string, string>(item.Email, c));
            }
            // 
            if (emailList.Count > 0)
                SendEmail(emailList, "您有一个需要处理的审批，请查看！");

        }

        #endregion
    }
}
