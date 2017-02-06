using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;
using System.Web.Mvc;
using LiXinModels.User;
using LiXinModels;

namespace LiXinControllers
{
    public partial class MyApplyController : BaseController
    {
        #region

        public ActionResult Free_SendMessage(int id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Free_BatchOtherFromAdd()
        {
            return View();
        }

        /// <summary>
        /// 第2部添加
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserOtherApply_Id"></param>
        /// <returns></returns>
        public ActionResult Free_BatchOtherFromAdd_Second(int id, int UserOtherApply_Id = 0, int year = 0)
        {
            year = year == 0 ? DateTime.Now.Year : year;
            ViewBag.year = year;
            var t = freeConfigBL.GetModel(" id=" + id + " and isDelete=0");
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;
            ViewBag.TrainGrade = CurrentUser.TrainGrade;
            ViewBag.id = id;
            ViewBag.UserOtherApply_Id = UserOtherApply_Id;
            ViewBag.Status = 0;
            Free_UserOtherApply UserOtherApply = new Free_UserOtherApply();
            if (UserOtherApply_Id != 0)
            {

                //查找批量人员
                Free_AllOtherApply model = iFree_AllOtherApplyBL.GetOtherFromsById(UserOtherApply_Id);

                if (model != null)
                {
                    ViewBag.PersonList = userBL.GetList(" UserId in (select id from dbo.F_SplitIDs('" + model.UserIDs + "'))");
                }

                UserOtherApply = iFree_UserOtherApply.GetFree_UserOtherApplyById(UserOtherApply_Id);

                ViewBag.year = UserOtherApply.Year;

                ViewBag.ApplyType = UserOtherApply.ApplyType;
                //ViewBag.UserOtherApply=UserOtherApply;
                //判断是否发布 已经发布 只能修改人员 其他都不能改，没发布 修改都能改
                ViewBag.Status = UserOtherApply.Status;

                ViewBag.OpenPerson = model.UserIDs;

                //加载申请上传证明资料
                List<Free_UserApplyFiles> UserApplyFiles = ifree_userapplyfiles.GetFree_UserApplyFilesList(" type=2 and userApplyID in(" + UserOtherApply_Id + ")");
                ViewBag.UserApplyFiles = UserApplyFiles.ToList();

                StringBuilder sb = null;
                if (UserOtherApply != null && UserOtherApply.ApplyType == 3)
                {
                    //sb = new StringBuilder();
                    List<Free_UserApplyTeacherDetails> Detailslist = iFree_UserApplyTeacherDetails.GetFree_UserApplyTeacherDetailsList(" userApplyID in(" + UserOtherApply_Id + ") and isdelete=0");
                    //var teacherconfig = t.Where(p => p.ApplyType == 0).FirstOrDefault();
                    var ConvertMax = t.ConvertMax;
                    string TrainGradeScore = t.TrainGradeScore;
                    var CPA = CurrentUser.CPA == "是" ? 1 : 0;
                    var key = 1;
                    foreach (var item in Detailslist)
                    {
                        item.key = key;
                        item.ClassName = item.ClassName.HtmlXssEncode();
                        item.TrainGradeScore = Convert.ToDecimal(TrainGradeScore);
                        item.ConvertMax = ConvertMax;
                        item.CPA = CPA;
                        item.ConvertBaseTo = t.ConvertBaseTo;
                        key++;
                    }

                    ViewBag.teacherlist = Newtonsoft.Json.JsonConvert.SerializeObject(Detailslist);
                }
            }

            ViewBag.freeConfig = t;

            return View(UserOtherApply);
        }

        public ActionResult Free_BatchOtherFrom()
        {
            return View();
        }

        public ActionResult Free_BatchOtherFromDetails(int ID, int type = 1)
        {
            ViewBag.ID = ID;
            ViewBag.type = type;

            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;
            ViewBag.TrainGrade = CurrentUser.TrainGrade;

            var ApproveUser = freeBL.GetSingleByID(ID, type);
            ApproveUser.ApplyFileList = ifree_userapplyfiles.GetFree_UserApplyFilesList(" type=2 and userApplyID in(" + ApproveUser.ID + ")");

            ViewBag.ConvertType = freeConfigBL.GetFreeOtherList_New(" ID=" + ApproveUser.ApplyID).FirstOrDefault().ConvertType;
            if (type == 3)
            {
                if (ApproveUser.ApplyType == 3)
                {
                    ApproveUser.teacherDetails = teacherDetailsBL.GetFree_UserApplyTeacherDetailsList("  userApplyID=" + ID + " and isdelete=0");
                }
            }
            return View(ApproveUser);
        }
        #endregion

        #region 批量列表
        public JsonResult GetFree_BatchUserOtherApplyList(string othername, int type, string starttime, string endtime, string jsRenderSortField = "ApplyTime desc", int pageSize = 10, int pageIndex = 1)
        {
            int limit = 0;
            string sql = " Free_UserOtherApply.typeForm=1 and Free_UserOtherApply.ApplyType in (1,3) AND Free_UserOtherApply.CreateUserID!=0 AND Free_UserOtherApply.IsDelete=0 ";

            if (!string.IsNullOrEmpty(othername))
            {
                sql += " and Free_OtherApplyConfig.ApplyContent like '%" + othername + "%'";
            }
            if (type != -1)
            {
                sql += " and Free_OtherApplyConfig.ConvertType in (" + type + ")";

                //if (type == 1)
                //{
                //    sql += " and Free_OtherApplyConfig.TrainGradeScore!=''";
                //}
                //else if (type == 2)
                //{
                //    sql += " and Free_OtherApplyConfig.ConvertMax!=0";
                //}
                //else if (type == 3)
                //{
                //    sql += " and (Free_OtherApplyConfig.TrainGradeScore!='' and Free_OtherApplyConfig.ConvertMax!=0)";
                //}
            }
            if (!string.IsNullOrEmpty(starttime))
            {
                sql += " and Free_UserOtherApply.ApplyTime>='" + starttime + "'";
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                sql += " and Free_UserOtherApply.ApplyTime<='" + endtime + "'";
            }

            //var list = iFree_UserOtherApply.GetFree_UserOtherApplyList_New(sql).SortListByField(jsRenderSortField);

            var list = iFree_UserOtherApply.GetFree_UserOtherApplyList_New_Batch(sql).SortListByField(jsRenderSortField);

            limit = list.Count();

            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                dataList = list,
                recordCount = limit
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 第一步添加
        public JsonResult Free_BatchOtherFromAddList(string othername, int year, int pageSize = 10, int pageIndex = 1)
        {
            int litme = 0;
            string sql = " year=" + year;
            if (!string.IsNullOrEmpty(othername))
            {
                sql += "AND ApplyContent LIKE '%" + othername + "%'";
            }

            var list = freeConfigBL.GetFreeOtherList(out litme, sql, pageIndex, pageSize).OrderBy(p => p.ApplyType);

            foreach (var item in list)
            {
                item.JiBie = item.ConvertType == 0 ? "N/A" : (item.TrainGradeScoreList.Keys.Contains(CurrentUser.TrainGrade) ? item.TrainGradeScoreList[CurrentUser.TrainGrade].ToString() : "N/A");
            }

            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = litme
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  第二部添加
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConvetType">1所内 0CPA 2所有</param>
        public JsonResult SubmitBatchOtherFrom_New(int ConvetType, decimal ConvertBaseTo, string ApplyID, int year, string other_content, string teacher_content, int type = 0, int UserOtherApply_Id = 0,
            string removefileID = "", string deleteids = "", string addids = "", string removeUserIDs = "")
        {

            int result = 0;
            int userApplyID = 0;
            int other_teacher = 0; //1：其他形式  2：讲师
            var listID = new List<int>();
            if (UserOtherApply_Id == 0)
            {
                #region 其他形式添加
                //请他形式添加
                if (other_content != "")
                {
                    string[] arr = other_content.Split('♣');
                    Free_UserOtherApply model = model = new Free_UserOtherApply();

                    model.Year = Convert.ToInt32(arr[2]);
                    model.ConvertScore = Convert.ToDecimal(arr[3]);
                    model.Memo = arr[6];
                    model.ApplyTime = DateTime.Now;
                    model.ApplyUserID = 0;// Convert.ToInt32(idlist[i]);
                    model.ApplyType = 1;///
                    model.Status = type;//



                    model.ApproveStatus = 0;
                    model.IsDelete = 0;
                    model.tScore = Convert.ToDecimal(arr[4]);
                    model.CPAScore = Convert.ToDecimal(arr[5]);
                    //model.GettScore = Convert.ToDecimal(arr[4]);
                    //model.GetCPAScore = Convert.ToDecimal(arr[5]);
                    model.ApplyName = arr[1];
                    model.ApplyID = Convert.ToInt32(arr[7]);
                    model.typeForm = 1;
                    model.CreateUserID = CurrentUser.UserId;
                    model.CreateTime = DateTime.Now;
                    model.fromID = 0;

                    if (iFree_UserOtherApply.AddFree_UserOtherApply(model))
                    {
                        userApplyID = model.ID;
                        Free_AllOtherApply all = new Free_AllOtherApply();
                        all.UserApplyID = model.ID;
                        all.UserIDs = arr[0];
                        iFree_AllOtherApplyBL.AddFree_AllOtherApply(all);
                    }

                    int fromID = model.ID;
                }
                #endregion

                #region 讲师添加
                //讲师添加
                if (teacher_content != "")
                {
                    string[] arr = teacher_content.Split('♣');

                    string[] array = arr[5].Split('◆');
                    decimal tScore = 0;
                    decimal CPAScore = 0;
                    for (int i = 0; i < array.Length; i++)
                    {
                        //算出总的所内学时和CPA学时
                        if (array[i] != "")
                        {
                            tScore += Convert.ToDecimal(array[i].Split('●')[2]);
                            CPAScore += Convert.ToDecimal(array[i].Split('●')[3]);
                        }
                    }
                    string[] idlist = arr[0].Split(',');
                    Free_UserOtherApply model = new Free_UserOtherApply();

                    model = new Free_UserOtherApply();
                    model.ApplyName = arr[1];
                    model.Year = Convert.ToInt32(arr[2]);
                    model.Memo = arr[3];
                    model.ApplyTime = DateTime.Now;
                    model.ApplyUserID = 0;// Convert.ToInt32(idlist[i]);
                    model.ApplyType = 3;
                    model.ApproveStatus = 0;//
                    model.IsDelete = 0;
                    model.ConvertScore = ConvertBaseTo;
                    model.Status = type;
                    model.ApplyID = Convert.ToInt32(arr[4]);
                    model.CreateTime = DateTime.Now;
                    model.CreateUserID = CurrentUser.UserId;
                    model.typeForm = 1;
                    model.GettScore = tScore;
                    model.GetCPAScore = CPAScore;
                    model.tScore = tScore;
                    model.CPAScore = CPAScore;
                    model.fromID = 0;

                    Free_UserApplyTeacherDetails details;
                    Free_AllOtherApply apply;
                    if (iFree_UserOtherApply.AddFree_UserOtherApply(model))
                    {
                        apply = new Free_AllOtherApply();

                        apply.UserIDs = arr[0];
                        apply.UserApplyID = model.ID;
                        //添加批量人员
                        iFree_AllOtherApplyBL.AddFree_AllOtherApply(apply);

                        details = new Free_UserApplyTeacherDetails();
                        details.userApplyID = model.ID;

                        for (int z = 0; z < array.Length; z++)
                        {
                            if (array[z] != "")
                            {
                                details.ClassName = array[z].Split('●')[0];
                                details.ConvertScore = Convert.ToDecimal(array[z].Split('●')[1]);
                                details.IsDelete = 0;
                                details.tScore = Convert.ToDecimal(array[z].Split('●')[2]);
                                details.CPAScore = Convert.ToDecimal(array[z].Split('●')[3]);
                                details.userApplyID = apply.UserApplyID;
                                iFree_UserApplyTeacherDetails.AddFree_UserApplyTeacherDetails(details);
                            }
                        }
                        result = 1;
                        other_teacher = 2;
                        userApplyID = model.ID;
                    }

                    int fromID = model.ID;

                }
                #endregion
            }
            else
            {
               // var userList = iFree_UserOtherApply.GetFree_UserOtherApplyList("  typeForm=1 AND ApplyType=1");
                var dict = new Dictionary<int, decimal>();
                var dicCPA = new Dictionary<int, decimal>();
                if ((type == 1 && addids != "" && other_content != "") || (teacher_content != "" && addids != ""))
                {
                    GetUserScoreByApplyID(out dict, out dicCPA, ConvertBaseTo, ApplyID.StringToInt32(), addids, year);
                }
                userApplyID = UserOtherApply_Id;
                //删除证明资料
                if (removefileID != "")
                {
                    ifree_userapplyfiles.DeleteFree_UserApplyFiles(removefileID.ToString().TrimEnd(','));
                }
                if (removeUserIDs != "")
                {
                    iFree_UserOtherApply.DeleteUserApply(removeUserIDs, UserOtherApply_Id);
                }

                #region 其他形式修改
                if (other_content != "")
                {
                    string[] arr = other_content.Split('♣');
                    Free_UserOtherApply model = new Free_UserOtherApply();

                    model.Year = Convert.ToInt32(arr[2]);
                    model.ConvertScore = Convert.ToDecimal(arr[3]);
                    model.Memo = arr[6];
                    model.ApplyTime = DateTime.Now;
                    model.ApplyUserID = 0;//Convert.ToInt32(idlist[i]);
                    model.ApplyType = 1;///
                    model.Status = type;//
                    model.ApproveStatus = 0;
                    model.IsDelete = 0;
                    model.tScore = Convert.ToDecimal(arr[4]);
                    model.CPAScore = Convert.ToDecimal(arr[5]);
                    model.ApplyName = arr[1];
                    model.ApplyID = Convert.ToInt32(arr[7]);
                    model.typeForm = 1;
                    model.CreateUserID = CurrentUser.UserId;
                    model.CreateTime = DateTime.Now;
                    model.fromID = 0;
                    model.ID = UserOtherApply_Id;

                    //iFree_UserOtherApply.AddFree_UserOtherApply(model);
                    if (iFree_UserOtherApply.UpdateFree_UserOtherApply(model))
                    {
                        Free_AllOtherApply apply = new Free_AllOtherApply();
                        apply.UserIDs = arr[0];
                        apply.UserApplyID = model.ID;
                        iFree_AllOtherApplyBL.UpdateFree_AllOtherApply(apply);
                    }

                    int fromID = model.ID;

                    //发布的时候添加人员
                    if (type == 1)
                    {
                        if (deleteids != "")
                        {
                            iFree_UserOtherApply.DeleteFree_UserOtherApplyByPeopleAndId(deleteids, UserOtherApply_Id);
                        }
                        if (addids != "")
                        {
                            addids = addids.TrimStart(',').TrimEnd(',');
                            for (int i = 0; i < addids.Split(',').Length; i++)
                            {
                                if (addids.Split(',')[i] != "")
                                {
                                    var userID = Convert.ToInt32(addids.Split(',')[i]);

                                    //if (listID.Count(p=>p==userID) == 0)
                                    //{
                                    //    listID.Add(userID);
                                        model.Status = 1;
                                        model.ApplyUserID = userID;
                                        model.fromID = fromID;
                                        model.CreateUserID = 0;
                                        model.typeForm = 1;
                                        model.DepApproveStatus = 1;
                                        model.ApproveStatus = 1;

                                        model.tScore = ConvetType != 0 ? (dict.Keys.Contains(model.ApplyUserID) ? dict[model.ApplyUserID] : 0) : 0;
                                        model.CPAScore = ConvetType != 1 ? (dicCPA.Keys.Contains(model.ApplyUserID) ? dicCPA[model.ApplyUserID] : 0) : 0;
                                        model.GettScore = model.tScore;
                                        model.GetCPAScore = model.CPAScore;
                                        if (!(model.tScore == 0 && model.CPAScore == 0))
                                        {
                                            iFree_UserOtherApply.AddFree_UserOtherApply(model);
                                        }
                                    //}
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 讲师修改
                if (teacher_content != "")
                {
                    var ID = 0;
                    string[] arr = teacher_content.Split('♣');

                    string[] array = arr[5].Split('◆');
                    decimal tScore = 0;
                    decimal CPAScore = 0;
                    for (int i = 0; i < array.Length; i++)
                    {
                        //算出总的所内学时和CPA学时
                        if (array[i] != "")
                        {
                            tScore += Convert.ToDecimal(array[i].Split('●')[2]);
                            CPAScore += Convert.ToDecimal(array[i].Split('●')[3]);
                        }
                    }

                    string[] idlist = arr[0].Split(',');
                    Free_UserOtherApply model = new Free_UserOtherApply();

                    model.ApplyName = arr[1];
                    model.Year = Convert.ToInt32(arr[2]);
                    model.Memo = arr[3];
                    model.ApplyTime = DateTime.Now;
                    model.ApplyUserID = 0;// Convert.ToInt32(idlist[i]);
                    model.ApplyType = 3;
                    model.ApproveStatus = 0;//
                    model.IsDelete = 0;
                    model.ConvertScore = ConvertBaseTo;
                    model.Status = type;
                    model.ApplyID = Convert.ToInt32(arr[4]);
                    model.CreateTime = DateTime.Now;
                    model.CreateUserID = CurrentUser.UserId;
                    model.typeForm = 1;
                    model.fromID = 0;
                    model.GettScore = tScore;
                    model.GetCPAScore = CPAScore;
                    model.tScore = tScore;
                    model.CPAScore = CPAScore;
                    model.ID = UserOtherApply_Id;

                    Free_UserApplyTeacherDetails details;
                    if (iFree_UserOtherApply.UpdateFree_UserOtherApply(model))
                    {
                        Free_AllOtherApply apply = new Free_AllOtherApply();
                        apply.UserIDs = arr[0];
                        apply.UserApplyID = model.ID;
                        //添加批量人员
                        iFree_AllOtherApplyBL.UpdateFree_AllOtherApply(apply);

                        ID = iFree_AllOtherApplyBL.GetOtherFromsById(model.ID).ID;

                        details = new Free_UserApplyTeacherDetails();
                        details.userApplyID = UserOtherApply_Id;

                        //删除原来的
                        iFree_UserApplyTeacherDetails.DeleteFree_UserApplyTeacherDetails(UserOtherApply_Id.ToString());

                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i] != "")
                            {
                                details.userApplyID = model.ID;
                                details.ClassName = array[i].Split('●')[0];
                                details.ConvertScore = Convert.ToDecimal(array[i].Split('●')[1]);
                                details.IsDelete = 0;
                                details.tScore = Convert.ToDecimal(array[i].Split('●')[2]);
                                details.CPAScore = Convert.ToDecimal(array[i].Split('●')[3]);
                                iFree_UserApplyTeacherDetails.AddFree_UserApplyTeacherDetails(details);
                            }
                        }
                        result = 1;
                        other_teacher = 2;
                        userApplyID = model.ID;
                    }

                    //发布的时候添加人员
                    if (type == 1)
                    {
                        int fromID = model.ID;

                        if (deleteids != "")
                        {
                            iFree_UserOtherApply.DeleteFree_UserOtherApplyByPeopleAndId(deleteids, UserOtherApply_Id);
                        }
                        if (addids != "")
                        {
                            addids = addids.TrimStart(',').TrimEnd(',');
                            //var BatchDetailsList = iFree_UserApplyTeacherDetails.GetFree_BatchTeacherDetails(" userApplyID=" + ID);
                            for (int i = 0; i < addids.Split(',').Length; i++)
                            {
                                if (addids.Split(',')[i] != "")
                                {
                                    var userID = Convert.ToInt32(addids.Split(',')[i]);
                                    //if (listID.Count(p => p == userID) == 0)
                                    //{
                                    //    listID.Add(userID);
                                        model.Status = 1;
                                        model.ApplyUserID = userID;
                                        model.fromID = fromID;
                                        model.CreateUserID = 0;
                                        model.typeForm = 1;
                                        model.DepApproveStatus = 1;
                                        model.ApproveStatus = 1;
                                        model.tScore = dict.Keys.Contains(model.ApplyUserID) ? dict[model.ApplyUserID] : 0;
                                        model.CPAScore = dicCPA.Keys.Contains(model.ApplyUserID) ? dicCPA[model.ApplyUserID] : 0;

                                        model.GettScore = model.tScore;
                                        model.GetCPAScore = model.CPAScore;
                                        if (!(model.tScore == 0 && model.CPAScore == 0))
                                        {
                                            iFree_UserOtherApply.AddFree_UserOtherApply(model);
                                        }
                                    //}
                                }
                            }
                            //发送邮件
                            // FunApplyCpaSendMessage(UserOtherApply_Id, addids);
                        }
                    }
                }
                #endregion
            }

            return Json(new { result = 1, userApplyID = userApplyID }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 列表方法

        #region 删除方法
        /// <summary>
        /// 删除申请内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteFree_UserOtherApply(int id)
        {
            int result = 0;
            if (iFree_UserOtherApply.DeleteFree_UserOtherApply(id.ToString()))
            {
                result = 1;
            }
            else
            {
                result = 0;
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 发布方法
        public JsonResult PublicFree_UserOtherApply(int id)
        {
            int result = 0;
            var userList = new List<Free_UserOtherApply>();
            if (iFree_UserOtherApply.UpdateFree_UserOtherApply_Status(id, ",ApproveStatus=1,DepApproveStatus=1"))
            {
                var ApplyList = iFree_AllOtherApplyBL.GetUserIDByOtherFroms(id);
                Free_UserOtherApply UserOtherApply = iFree_UserOtherApply.GetFree_UserOtherApplyById(id);

                var conveType = 0;//类型
                int fromID = UserOtherApply.ID;

                var ID = iFree_AllOtherApplyBL.GetOtherFromsById(id).ID;
                var BatchDetailsList = iFree_UserApplyTeacherDetails.GetFree_UserApplyTeacherDetailsList(" userApplyID=" + id);

                var dict = new Dictionary<int, decimal>();
                var dicCPA = new Dictionary<int, decimal>();

                if (UserOtherApply.ApplyType != 2)
                {
                    var config = freeConfigBL.GetFreeOtherList_New(" ID=" + UserOtherApply.ApplyID).FirstOrDefault();
                    conveType = config.ConvertType == 2 ? 2 : (config.ConvertType == 1 ? 0 : 1);
                    var userIDs = string.Join(",", ApplyList.Select(p => p.UserId));
                    if (UserOtherApply.ApplyType == 1)
                    {
                        if (userIDs != "")
                        {
                            GetUserScoreByApplyID(out dict, out dicCPA, UserOtherApply.ConvertScore, UserOtherApply.ApplyID, userIDs, UserOtherApply.Year);
                        }
                    }
                    else
                    {
                        var ConvertScore = BatchDetailsList.Sum(p => p.ConvertScore);
                        if (userIDs != "")
                        {
                            GetUserScoreByApplyID(out dict, out dicCPA, ConvertScore, UserOtherApply.ApplyID, userIDs, UserOtherApply.Year);
                        }
                    }
                }
                else
                {
                    //免修自动折算
                    var year = UserOtherApply.Year;
                    var freeConfig = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == year).FirstOrDefault();
                    if (!(freeConfig == null || freeConfig.ConfigValue == ""))
                    {
                        var configvalue = freeConfig.ConfigValue.Split(';');
                        var tDate = year + "-" + configvalue[0].Split(',')[0];
                        var CPADate = year + "-" + configvalue[1].Split(',')[0];

                        userList = reportFreeBL.GetUserByDate(tDate, CPADate).ToList();
                        foreach (var item in userList)
                        {
                            if (item.JoinDate >= Convert.ToDateTime(tDate))
                            {
                                item.tCount = 1;
                            }
                            if (item.CPA == "是" && item.CPACreateDate >= Convert.ToDateTime(CPADate))
                            {
                                item.CPACount = 1;
                            }
                        }
                    }

                    userList.AddRange(freeBL.GetFreeItemByUser(year, -1).ToList());

                    var config = freeConfigBL.GetFree_ApplyConfig(" ID=" + UserOtherApply.ApplyID);
                    conveType = config.ApplyType;
                }

                foreach (var item in ApplyList)
                {
                    int userID = item.UserId;
                    //只能申请2次免修项目，所内一次 cpa一次
                    var flag = false;
                    if (UserOtherApply.ApplyType != 2)
                    {
                        var singleModel = userList.Where(p => p.ApplyUserID == userID);
                        var tCount = singleModel.Sum(p => p.tCount);
                        var CPACount = singleModel.Sum(p => p.CPACount);
                      
                        switch (conveType)
                        {
                            case 0:
                                if (tCount > 0)
                                {
                                    flag = true;
                                }
                                break;
                            case 1:
                                if (CPACount > 0 && item.CPA == "是")
                                {
                                    flag = true;
                                }
                                break;
                            case 2:
                                if ((CPACount > 0 && item.CPA == "是") || tCount > 0)
                                {
                                    flag = true;
                                }
                                break;
                        }
                    }
                    if (!flag || UserOtherApply.ApplyType != 2)
                    {
                        var CPAScore = conveType != 0 ? (item.CPA == "否" ? 0 : (dicCPA.Keys.Contains(userID) ? dicCPA[userID] : 0)) : 0;
                        var tscore = conveType != 1 ? (dict.Keys.Contains(userID) ? dict[userID] : 0) : 0;
                        UserOtherApply.Status = 1;
                        UserOtherApply.ApplyUserID = userID;
                        UserOtherApply.fromID = fromID;
                        UserOtherApply.CreateUserID = 0;
                        UserOtherApply.typeForm = 1;
                        UserOtherApply.DepApproveStatus = 1;
                        UserOtherApply.ApproveStatus = 1;
                        UserOtherApply.ApplyName = "";
                        if (UserOtherApply.ApplyType == 2)
                        {
                            UserOtherApply.GetCPAScore = conveType != 0 ? (item.CPA == "否" ? 0 : UserOtherApply.CPAScore) : 0; ;
                            UserOtherApply.GettScore = conveType != 1 ? UserOtherApply.tScore : 0;
                        }
                        else
                        {
                            UserOtherApply.tScore = tscore;
                            UserOtherApply.CPAScore = CPAScore;
                            UserOtherApply.GettScore = tscore;
                            UserOtherApply.GetCPAScore = CPAScore;
                        }
                        if (!(UserOtherApply.tScore == 0 && UserOtherApply.CPAScore == 0))
                        {
                            iFree_UserOtherApply.AddFree_UserOtherApply(UserOtherApply);
                        }

                    }
                }
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 发送邮件查询
        public JsonResult GetUserIds_Free_AllOtherApply(int ids, string realName = "", string deptName = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " Sys_User.UserId asc")
        {
            try
            {
                var model = iFree_AllOtherApplyBL.GetOtherFromsById(Convert.ToInt32(ids));

                string where = " UserId in (select id from dbo.F_SplitIDs('" + model.UserIDs + "'))";

                if (!string.IsNullOrWhiteSpace(deptName))
                    where += string.Format(" and Sys_User.DeptPath like '%{0}%'", deptName.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(realName))
                    where += string.Format(" and Sys_User.RealName like '%{0}%'", realName.ReplaceSql());

                var list = userBL.GetList(where).SortListByField(jsRenderSortField);
                int totalCount = list.Count();
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
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
                    dataList = new List<Sys_User>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        public JsonResult GetApplyUsers(int id, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "OrderTime desc")
        {

            var list = iFree_AllOtherApplyBL.GetUserDetails(id);
            // var list = userBL.GetList(" UserId in (select id from dbo.F_SplitIDs('" + model.UserIDs + "'))");

            int totalCount = list.Count();

            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 公共
        /// <summary>
        /// 根据人员所申请的内容,判断其所能申请的学时
        /// </summary>
        /// <param name="ApplyID"></param>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        public void GetUserScoreByApplyID(out Dictionary<int, decimal> dictScore, out Dictionary<int, decimal> dicCPAScore,
            decimal ConvertBaseTo, int ApplyID, string userIDs, int year)
        {

            dictScore = new Dictionary<int, decimal>();
            dicCPAScore = new Dictionary<int, decimal>();
            //获取配置
            var config = freeConfigBL.GetFreeOtherList_New(" ID=" + ApplyID).FirstOrDefault();

            ///所有人员所获得的学时
            var addUserIDs = userIDs.TrimStart(',').TrimEnd(',');

            var Scorelist = iFree_UserOtherApply.GetMaxScore(addUserIDs, ApplyID, year);

            //授课讲师获得的学时
            var teacherList = iFree_UserOtherApply.GetScoreByUser(addUserIDs, year);

            #region 数值赋给字典

            decimal score = 0;
            if (config.ApplyType == 1)
            {
                score = Math.Round(ConvertBaseTo * (config.ConvertBaseTo / config.ConvertBase), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                score = Math.Round(ConvertBaseTo * config.ConvertBaseTo, 2, MidpointRounding.AwayFromZero);
            }

            decimal Maxtscore = 0;
            decimal MaxCPAscore = 0;
            foreach (var item in Scorelist)
            {
                var single = Scorelist.Where(p => p.ApplyUserID == item.ApplyUserID).FirstOrDefault();

                //var teacherSingle = teacherList.Where(p => p.ApplyUserID == item.ApplyUserID).FirstOrDefault();
                if (config.ApplyType == 1)
                {
                    Maxtscore = config.TrainGradeScoreList.Keys.Contains(item.TrainGrade) ? config.TrainGradeScoreList[item.TrainGrade] : 0;
                    MaxCPAscore = config.ConvertMax;
                }
                else
                {
                    single = teacherList.Where(p => Convert.ToInt32(p.ApplyUserID) == item.ApplyUserID).FirstOrDefault();
                    Maxtscore = Convert.ToDecimal(config.TrainGradeScore);
                    MaxCPAscore = config.ConvertMax;
                }

                if (single != null)
                {
                    var tscore = Convert.ToDecimal(single.tScore);
                    var CPAscore = Convert.ToDecimal(single.CPAScore);
                    var gettScore = Maxtscore - tscore < 0 ? 0 : Maxtscore - tscore;
                    var getCPAScore = MaxCPAscore - CPAscore < 0 ? 0 : MaxCPAscore - CPAscore;
                    dictScore[item.ApplyUserID] = config.ConvertType == 0 ? 0 : (score > gettScore ? gettScore : score);
                    dicCPAScore[item.ApplyUserID] = item.CPA == "否" ? 0 : (score > getCPAScore ? getCPAScore : score);
                }
                else
                {
                    dictScore[item.ApplyUserID] = score;
                    dicCPAScore[item.ApplyUserID] = item.CPA == "否" ? 0 : score;
                }
                //DicScoreList[item.ApplyUserID] =
            }

            #endregion
        }
        #endregion

    }
}
