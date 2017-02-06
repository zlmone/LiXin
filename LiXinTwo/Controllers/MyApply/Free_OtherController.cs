using LiXinControllers.Filter;
using LiXinInterface.SystemManage;
using LiXinModels.PlanManage;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using System.Configuration;
using System.Web;
using LiXinModels;
using LiXinModels.MyApproval;
using System.Data;

namespace LiXinControllers
{
    public partial class MyApplyController : BaseController
    {
        #region 其他形式

        public ActionResult Free_Main()
        {
            return View();
        }

        /// <summary>
        /// 其他形式
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_OtherFrom()
        {
            ViewBag.IsTimeIn = FreeIsTimeIn(60, 0);
            ViewBag.IsInTrainGrade = FreeIsInTrainGrade(0);
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;
            ViewBag.yearList = freeBL.GetYearList();
            return View();
        }
        /// <summary>
        /// 其他形式-添加1
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_OtherFromAdd()
        {
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;
            return View();
        }

        /// <summary>
        /// 其他形式-添加2
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_OtherFromAdd_Second(string ids, int UserOtherApply_Id = 0, int year = 0)
        {
            ViewBag.year = year == 0 ? DateTime.Now.Year : year;
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;
            var list = freeConfigBL.GetFreeOtherList_New(" ID IN(" + ids + ")").OrderByDescending(p => p.ApplyType).ToList();
            ViewBag.TrainGrade = CurrentUser.TrainGrade;
            ViewBag.list = list;
            ViewBag.UserOtherApply_Id = UserOtherApply_Id;
            var Scorelist = iFree_UserOtherApply.GetMaxScore(" ID!=" + UserOtherApply_Id + " and  ApplyUserID=" + CurrentUser.UserId + " and ApplyID in (" + ids.TrimStart(',').TrimEnd(',') + ") and Year=" + DateTime.Now.Year);

            //加载申请上传证明资料
            if (UserOtherApply_Id != 0)
            {
                List<Free_UserApplyFiles> UserApplyFiles = ifree_userapplyfiles.GetFree_UserApplyFilesList(" type=0 and  userApplyID in(" + UserOtherApply_Id + ")");
                ViewBag.UserApplyFiles = UserApplyFiles.ToList();

            }

            //var UserOtherApply = iFree_UserOtherApply.GetFree_UserOtherApplyList_New(ids);

            StringBuilder sb = new StringBuilder();

            Free_UserOtherApply UserOtherApply = new Free_UserOtherApply();
            var userGetScore = iFree_UserOtherApply.GetScoreByteacher(CurrentUser.UserId, DateTime.Now.Year, "typeForm=3");

            ViewBag.teacherscore = Convert.ToDecimal(userGetScore.tScore);
            ViewBag.teacherCPAScore = Convert.ToDecimal(userGetScore.CPAScore);
            #region 修改时候

            if (UserOtherApply_Id != 0)
            {


                UserOtherApply = iFree_UserOtherApply.GetFree_UserOtherApplyById(UserOtherApply_Id);

                ViewBag.ApplyType = UserOtherApply.ApplyType;
                ViewBag.year = UserOtherApply.Year;
                if (UserOtherApply != null && UserOtherApply.ApplyType == 3)
                {
                    var Detailslist = iFree_UserApplyTeacherDetails.GetFree_UserApplyTeacherDetailsList(" userApplyID in(" + UserOtherApply_Id + ") and isdelete=0");

                    var teacherconfig = list.Where(p => p.ApplyType == 0).FirstOrDefault();
                    //string TrainGradeScore = list.Where(p => p.ApplyType == 0).FirstOrDefault().TrainGradeScore;
                    var single = Scorelist.Where(p => p.ApplyID == ids.StringToInt32()).FirstOrDefault();

                    var CPA = CurrentUser.CPA == "是" ? 1 : 0;
                    var key = 1;
                    foreach (var item in Detailslist)
                    {
                        item.key = key;
                        item.ClassName = item.ClassName.HtmlXssEncode();
                        item.TrainGradeScore = Convert.ToDecimal(teacherconfig.TrainGradeScore);
                        item.ConvertMax = teacherconfig.ConvertMax;
                        item.CPA = CPA;
                        item.ConvertBaseTo = teacherconfig.ConvertBaseTo;
                        item.ApprovetScore = single == null ? 0 : single.tScore;
                        item.ApproveCPAScore = single == null ? 0 : single.CPAScore;
                        key++;
                    }

                    ViewBag.teacherlist = Newtonsoft.Json.JsonConvert.SerializeObject(Detailslist);

                }


                ViewBag.UserOtherApply = UserOtherApply;
            }

            #endregion

            ViewBag.Scorelist = Scorelist;
            return View(UserOtherApply);
        }


        /// <summary>
        /// 其他形式审批详情
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ApplyUserID"></param>
        /// <param name="type">1 其他形式 2 免修 3 授课人</param>
        /// <param name="typeForm">0自己新增 1 导入</param>
        /// <returns></returns>
        public ActionResult Free_OtherFromDetails(int ID, int type = 1, int typeForm = 0)
        {
            ViewBag.ID = ID;
            ViewBag.type = type;
            ViewBag.typeForm = typeForm;
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;
            ViewBag.TrainGrade = CurrentUser.TrainGrade;
            var ApproveUser = freeBL.GetSingleByID(ID, type);
            if (typeForm == 0)
            {
                ApproveUser.ApplyFileList = ifree_userapplyfiles.GetFree_UserApplyFilesList(" type=0 and  userApplyID in(" + ID + ")");
            }
            else
            {
                ApproveUser.ApplyFileList = ifree_userapplyfiles.GetFree_UserApplyFilesList(" type=2 and  userApplyID in(" + ApproveUser.fromID + ")");
            }
            if (type == 3)
            {
                if (ApproveUser.ApplyType == 3)
                {
                    if (typeForm == 0)
                    {
                        ApproveUser.teacherDetails = teacherDetailsBL.GetFree_UserApplyTeacherDetailsList(" IsDelete=0 and userApplyID=" + ID);
                    }
                    else
                    {
                        ApproveUser.teacherDetails = teacherDetailsBL.GetFree_UserApplyTeacherDetailsList(" IsDelete=0 and userApplyID=" + ApproveUser.fromID);
                    }
                }
            }

            return View(ApproveUser);
        }

        /// <summary>
        /// 其他形式审批详情
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ApplyUserID"></param>
        /// <param name="type"> 2 评估自动折算 3 授课自动折算</param>
        /// <param name="typeForm">0自己新增 1 导入</param>
        /// <returns></returns>
        public ActionResult Free_OtherFromOutDetails(int ID, int type = 1, int typeForm = 0)
        {
            ViewBag.ID = ID;
            ViewBag.type = type;
            ViewBag.typeForm = typeForm;
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;
            var ApproveUser = new ShowFreeDetails();

            ApproveUser = iFree_UserOtherApply.GetSurveyOut(ID);
            ApproveUser.ApplyFileList = ifree_userapplyfiles.GetFree_UserApplyFilesList(" type=2 and  userApplyID in(" + ID + ")");

            return View(ApproveUser);
        }

        public JsonResult Free_OtherFromAddList(string othername, int year = 0, int pageSize = 10, int pageIndex = 1, int type = 0)
        {
            int litme = 0;
            string sql = " year=" + (year == 0 ? DateTime.Now.Year : year) + (type == 1 ? "" : (CurrentUser.CPA == "是" ? "" : " and ConvertType!=0"));
            if (!string.IsNullOrEmpty(othername))
            {
                sql += "AND ApplyContent LIKE '%" + othername.ReplaceSql() + "%'";
            }

            var list = freeConfigBL.GetFreeOtherList(out litme, sql, pageIndex, pageSize);

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


        /// <summary>
        /// 查询我申请的其他形式
        /// </summary>
        /// <param name="approveType">Tab页切换 0全部 1待审批 2审批通过 3审批拒绝</param>
        /// <returns></returns>
        public JsonResult GetFree_UserOtherApplyList(string othername, int bm_type, int px_type, string starttime, string endtime,
            int year, int cpa, int approveType = 0, string jsRenderSortField = "ApplyTime desc", int search = 0)
        {
            #region 条件
            var flag = false;
            var teacherWhere = " 1=1";
            var surveyWhere = " and 1=1";
            string sql = " Free_UserOtherApply.isdelete=0 and (typeForm=0 OR (typeForm=1 AND CreateUserID=0 and Status=1))  AND Free_UserOtherApply.ApplyType IN(1,3)  AND Free_UserOtherApply.ApplyUserID=" + CurrentUser.UserId;
            if (approveType > 0)
            {
                flag = true;
                switch (approveType)
                {
                    case 1://待审批
                        sql += " and (ApproveStatus=0 or (ApproveStatus=1 and DepApproveStatus=0))";
                        break;
                    case 2://审批通过
                        sql += " and ApproveStatus=1 and DepApproveStatus=1";
                        break;
                    case 3://审批拒绝
                        sql += @" and (ApproveStatus=2 or DepApproveStatus=2)";
                        break;
                }
            }
            if (!string.IsNullOrEmpty(othername))
            {
                flag = true;
                sql += " and Free_OtherApplyConfig.ApplyContent like '%" + othername + "%'";
                teacherWhere += " and CourseName   like '%" + othername + "%'";
                surveyWhere += " and cc.CourseName   like '%" + othername + "%'";
            }

            if (bm_type != 0)
            {
                flag = true;
                if (bm_type == 1)
                {
                    sql += " and Free_UserOtherApply.ApproveStatus=0";
                }
                else if (bm_type == 2)
                { sql += " and Free_UserOtherApply.ApproveStatus=1 and Free_UserOtherApply.DepApproveStatus!=2"; }
                else if (bm_type == 3)
                { sql += " and Free_UserOtherApply.ApproveStatus=2"; }
                else if (bm_type == 4)
                {
                    sql += " and (Free_UserOtherApply.ApproveStatus=1 and Free_UserOtherApply.DepApproveStatus=2 )";
                }
            }


            if (px_type != 0)
            {
                flag = true;
                if (px_type == 1)
                {
                    sql += " and (Free_UserOtherApply.DepApproveStatus=0 and Free_UserOtherApply.ApproveStatus=1)";
                }
                else if (px_type == 2)
                { sql += " and Free_UserOtherApply.DepApproveStatus=1"; }
                else if (px_type == 3)
                { sql += " and Free_UserOtherApply.DepApproveStatus=2"; }

            }

            if (!string.IsNullOrEmpty(starttime))
            {
                flag = true;
                sql += " and Free_UserOtherApply.ApplyTime>='" + starttime + "'";
                teacherWhere += " and EndTime>='" + starttime + "'";
                surveyWhere += " and ApplyTime>='" + starttime + "'";
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                flag = true;
                sql += " and Free_UserOtherApply.ApplyTime<='" + endtime + "'";
                teacherWhere += " and EndTime<='" + endtime + "'";
                surveyWhere += " and ApplyTime<='" + endtime + "'";
            }

            if (year != 0)
            {
                //flag = true;
                sql += " and Free_UserOtherApply.Year=" + year;
                surveyWhere += " and fuo.Year=" + year;
                teacherWhere += " and fuo.Year=" + year;
            }

            if (cpa != 2)
            {
                flag = true;
                sql += " and Free_OtherApplyConfig.ConvertType in (" + cpa + ",2)";
            }

            #endregion

            //自行申报
            var MyApplyList = iFree_UserOtherApply.GetFree_UserOtherApplyList_New(sql).SortListByField(jsRenderSortField);

            MyApplyList.ForEach(p => { p.isout = 1; });

            var OutApplyList = new List<Free_UserOtherApply>();
            var config = new Free_OtherApplyConfig();
            var teacherList = new List<Free_UserOtherApply>();
            if (search == 0 || !flag)
            {
                //自动折算
                OutApplyList = iFree_UserOtherApply.GetOutUserApply(surveyWhere + " and fuo.ApplyUserID=" + CurrentUser.UserId);
                OutApplyList.ForEach(p => { p.isout = 2; });

                //授课学时
                teacherList = iFree_UserOtherApply.GetTeacherScoreList(teacherWhere + " AND fuo.ApplyUserID=" + CurrentUser.UserId);
                teacherList.ForEach(p =>
                {
                    p.CPAScore = p.ConvertType == 1 ? 0 : p.CPAScore;
                    p.GetCPAScore = p.ConvertType == 1 ? 0 : p.GetCPAScore;
                });
            }
            //添加到末尾
            OutApplyList.AddRange(teacherList);
            MyApplyList.AddRange(OutApplyList);
            return Json(new
            {
                MyApplyList = MyApplyList.Where(p => p.isout == 1),
                allApplyCount = GetMyApply(0, MyApplyList),
                MyApplyCount = GetMyApply(1, MyApplyList.Where(p => p.isout == 1).ToList()),
                outApplyCount = GetMyApply(2, OutApplyList),
                OutApplyList = OutApplyList.Where(p => p.isout == 2)
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取合计
        /// </summary>
        /// <param name="type">0 合计 1 自行申报合计 2 自动折算合计</param>
        /// <param name="MyApplyList"></param>
        /// <returns></returns>
        public dynamic GetMyApply(int type, List<Free_UserOtherApply> MyApplyList)
        {
            var ApplyName = "";
            switch (type)
            {
                case 0:
                    ApplyName = "合计";
                    break;
                case 1:
                    ApplyName = "自行申报合计";
                    break;
                case 2:
                    ApplyName = "自动折算合计";
                    break;
            }
            var MyApplyCount = new
            {
                ApplyName = ApplyName,
                tScore = MyApplyList.Sum(p => p.tScore),
                CPAScore = MyApplyList.Sum(p => p.CPAScore),
                ApplyTimeStr = "N/A",
                ApproveStatusStr = "N/A",
                DepApproveStatusStr = "N/A",
                GettScore = MyApplyList.Sum(p => p.GettScore),
                GetCPAScore = MyApplyList.Sum(p => p.GetCPAScore),
                isout = type
            };
            return MyApplyCount;
        }

        /// <summary>
        /// 删除申请内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult fdeleteUserOtherApply(string id)
        {
            if (iFree_UserOtherApply.DeleteFree_UserOtherApply(id))
            {
                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 发布  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">1 免修 0 其他</param>
        /// <returns></returns>
        public JsonResult fSubmitUpdateStatus(int id, int type = 0)
        {

            var model = new ShowFreeDetails();
            var name = "";
            if (type == 0)
            {
                model = freeBL.GetSingleByID(id, -1);
                if (CurrentUser.CPA == "是")
                {
                    switch (model.ConvertType)
                    {
                        case 0:
                            name = "CPA继续教育其他形式学时";
                            break;
                        case 1:
                            name = "内部培训其他形式学时";
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
            }
            else
            {
                model = freeBL.GetFreeSingleByID(id);
                if (CurrentUser.CPA == "是")
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
            }

            if (iFree_UserOtherApply.UpdateFree_UserOtherApply_Status(id))
            {
                SendApplyMail(type, model.ApplyTime, model.Year, name);
                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 关联资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Filter.SystemLog("提交申请内容", LogLevel.Info)]
        public ContentResult SubmitAddResource(int userApplyID, string ResourceName, string RealName, int type = 0)
        {
            var model = new Free_UserApplyFiles();
            model.userApplyID = userApplyID;
            model.IsDelete = 0;
            model.ResourceName = ResourceName;
            model.RealName = RealName;
            model.type = type;
            //文件后缀名
            string suffix = ResourceName.Substring(ResourceName.LastIndexOf(".") + 1).ToLower();

            var swf = new List<string> { "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "pdf" };
            model.ConvertName = RealName;
            if (swf.Contains(suffix))
            {
                model.ConvertName = RealName + ".swf";
            }
            //保存
            var list = ifree_userapplyfiles.AddFree_UserApplyFiles(model);

            if (swf.Contains(suffix))
            {
                //加入消息队列
                RabbitSet(RealName);

            }
            return Content(list ? "1" : "0");

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="type">0 未提交 1 提交</param>
        /// <returns></returns>
        public JsonResult SubmitOtherFrom(string t, int type = 0, int UserOtherApply_Id = 0)
        {
            Free_UserOtherApply model;
            int result = 0;
            if (t != "")
            {
                //♣♥
                if (UserOtherApply_Id == 0)
                {
                    string[] arr = t.Split('♣');
                    model = new Free_UserOtherApply();
                    model.Year = Convert.ToInt32(arr[0]);
                    model.ConvertScore = Convert.ToDecimal(arr[1]);
                    model.Memo = arr[4];
                    model.ApplyTime = DateTime.Now;
                    model.ApplyUserID = CurrentUser.UserId;
                    model.ApplyType = 1;///
                    model.Status = type;//
                    model.ApproveStatus = 0;
                    model.IsDelete = 0;
                    model.tScore = Convert.ToDecimal(arr[2]);
                    model.CPAScore = Convert.ToDecimal(arr[3]);
                    model.ApplyName = arr[5];
                    model.ApplyID = Convert.ToInt32(arr[6]);
                    model.typeForm = 0;
                    model.CreateUserID = 0;

                    result = 1;
                    //if (iFree_UserOtherApply.AddFree_UserOtherApply(model))
                    //{
                    //    result = 1;
                    //}
                    //else
                    //{
                    //    result = 0;
                    //}
                }
                else
                {
                    string[] arr = t.Split('♣');
                    model = new Free_UserOtherApply();

                    model.Year = Convert.ToInt32(arr[0]);
                    model.ConvertScore = Convert.ToDecimal(arr[1]);
                    model.Memo = arr[4];
                    model.ApplyTime = DateTime.Now;
                    model.ApplyUserID = CurrentUser.UserId;
                    model.ApplyType = 1;///
                    model.Status = type;//
                    model.ApproveStatus = 0;
                    model.IsDelete = 0;
                    model.tScore = Convert.ToDecimal(arr[2]);
                    model.CPAScore = Convert.ToDecimal(arr[3]);
                    model.ApplyName = arr[5];
                    model.ApplyID = Convert.ToInt32(arr[6]);
                    model.ID = UserOtherApply_Id;
                    model.typeForm = 0;
                    model.CreateUserID = 0;
                    if (iFree_UserOtherApply.UpdateFree_UserOtherApply(model))
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            return Json(new
            {
                result = result
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="content"></param>
        /// <param name="type">0 未提交 1 提交</param>
        /// <returns></returns>
        public JsonResult SubmitOtherFromTeacher(int id, string t, string content, int type = 0, int UserOtherApply_Id = 0)
        {
            Free_UserOtherApply model;
            int UserOtherApply_Id_int = 0;

            if (t != "" && content != "")
            {
                if (UserOtherApply_Id == 0)
                {
                    string[] arr = t.Split('♣');
                    model = new Free_UserOtherApply();
                    model.ApplyName = arr[0];
                    model.Year = Convert.ToInt32(arr[1]);
                    model.Memo = arr[2];
                    model.ApplyTime = DateTime.Now;
                    model.ApplyUserID = CurrentUser.UserId;
                    model.ApplyType = 3;
                    model.ApproveStatus = 0;//
                    model.IsDelete = 0;
                    model.ConvertScore = 0;
                    model.Status = type;
                    model.ApplyID = id;

                    string[] array = content.Split('◆');
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
                    model.tScore = tScore;
                    model.CPAScore = CPAScore;

                    Free_UserApplyTeacherDetails details;
                    if (iFree_UserOtherApply.AddFree_UserOtherApply(model))
                    {

                        details = new Free_UserApplyTeacherDetails();
                        details.userApplyID = model.ID;
                        UserOtherApply_Id_int = model.ID;

                        //iFree_UserOtherApply.AddFree_UserOtherApply(model);
                        //iFree_UserApplyTeacherDetails.AddFree_UserApplyTeacherDetails(details);


                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i] != "")
                            {
                                details.ClassName = array[i].Split('●')[0];
                                details.ConvertScore = Convert.ToDecimal(array[i].Split('●')[1]);
                                details.IsDelete = 0;
                                details.tScore = Convert.ToDecimal(array[i].Split('●')[2]);
                                details.tScore = Convert.ToDecimal(array[i].Split('●')[3]);

                                iFree_UserApplyTeacherDetails.AddFree_UserApplyTeacherDetails(details);
                            }
                        }
                    }
                }
                else
                {
                    string[] arr = t.Split('♣');
                    model = new Free_UserOtherApply();
                    model.ID = UserOtherApply_Id;
                    model.ApplyName = arr[0];
                    model.Year = Convert.ToInt32(arr[1]);
                    model.Memo = arr[2];
                    model.ApplyTime = DateTime.Now;
                    model.ApplyUserID = CurrentUser.UserId;
                    model.ApplyType = 3;
                    model.ApproveStatus = 0;//
                    model.IsDelete = 0;
                    model.ConvertScore = 0;
                    model.Status = type;
                    model.ApplyID = id;

                    string[] array = content.Split('◆');
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
                    model.tScore = tScore;
                    model.CPAScore = CPAScore;

                    Free_UserApplyTeacherDetails details;
                    if (iFree_UserOtherApply.UpdateFree_UserOtherApply(model))
                    {

                        details = new Free_UserApplyTeacherDetails();
                        details.userApplyID = UserOtherApply_Id;

                        //删除原来的
                        iFree_UserApplyTeacherDetails.DeleteFree_UserApplyTeacherDetails(UserOtherApply_Id.ToString());

                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i] != "")
                            {
                                details.ClassName = array[i].Split('●')[0];
                                details.ConvertScore = Convert.ToDecimal(array[i].Split('●')[1]);
                                details.IsDelete = 0;
                                details.tScore = Convert.ToDecimal(array[i].Split('●')[2]);
                                details.tScore = Convert.ToDecimal(array[i].Split('●')[3]);

                                iFree_UserApplyTeacherDetails.AddFree_UserApplyTeacherDetails(details);
                            }
                        }
                    }
                }

            }

            return Json(new
            {
                result = 1,
                UserOtherApply_Id = UserOtherApply_Id
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other_content"></param>
        /// <param name="teacher_content"></param>
        /// <param name="type"></param>
        /// <param name="UserOtherApply_Id"></param>
        /// <returns></returns>
        public JsonResult SubimtOther_New(string other_content, string teacher_content, int type = 0, int UserOtherApply_Id = 0, string removefileID = "")
        {
            int result = 0;
            int userApplyID = 0;
            int other_teacher = 0; //1：其他形式  2：讲师
            var publish = type;
            ////添加
            if (UserOtherApply_Id == 0)
            {
                //请他形式添加
                #region
                if (other_content != "")
                {
                    string[] arr = other_content.Split('♣');
                    Free_UserOtherApply model = new Free_UserOtherApply();
                    model.Year = Convert.ToInt32(arr[0]);
                    model.ConvertScore = Convert.ToDecimal(arr[1]);
                    model.Memo = arr[4];
                    model.ApplyTime = DateTime.Now;
                    model.ApplyUserID = CurrentUser.UserId;
                    model.ApplyType = 1;///
                    model.Status = type;//
                    model.ApproveStatus = 0;
                    model.IsDelete = 0;
                    model.tScore = Convert.ToDecimal(arr[2]);
                    model.CPAScore = Convert.ToDecimal(arr[3]);
                    model.ApplyName = arr[5];
                    model.ApplyID = Convert.ToInt32(arr[6]);
                    model.typeForm = 0;
                    model.CreateUserID = 0;
                    model.CreateTime = DateTime.MinValue;

                    if (iFree_UserOtherApply.AddFree_UserOtherApply(model))
                    {
                        userApplyID = model.ID;
                        other_teacher = 1;
                        result = 1;
                    }
                    else
                    {
                        other_teacher = 1;
                        result = 0;
                    }

                }
                #endregion

                //讲师添加
                #region
                if (teacher_content != "")
                {
                    Free_UserOtherApply model = new Free_UserOtherApply();

                    string[] arr = teacher_content.Split('♣');
                    model = new Free_UserOtherApply();
                    model.ApplyName = arr[0];
                    model.Year = Convert.ToInt32(arr[1]);
                    model.Memo = arr[2];
                    model.ApplyTime = DateTime.Now;
                    model.ApplyUserID = CurrentUser.UserId;
                    model.ApplyType = 3;
                    model.ApproveStatus = 0;//
                    model.IsDelete = 0;
                    model.ConvertScore = 0;
                    model.Status = type;

                    model.ApplyID = Convert.ToInt32(arr[3]);
                    model.typeForm = 0;
                    model.CreateUserID = 0;
                    model.CreateTime = DateTime.MinValue;
                    string[] array = arr[4].Split('◆');
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
                    model.tScore = tScore;
                    model.CPAScore = CPAScore;

                    Free_UserApplyTeacherDetails details;
                    if (iFree_UserOtherApply.AddFree_UserOtherApply(model))
                    {

                        details = new Free_UserApplyTeacherDetails();
                        details.userApplyID = model.ID;
                        //UserOtherApply_Id_int = model.ID;
                        //iFree_UserOtherApply.AddFree_UserOtherApply(model);
                        //iFree_UserApplyTeacherDetails.AddFree_UserApplyTeacherDetails(details);

                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i] != "")
                            {
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
                }
                #endregion


            }
            else
            {
                //删除证明资料
                if (removefileID != "")
                {
                    ifree_userapplyfiles.DeleteFree_UserApplyFiles(removefileID.ToString().TrimEnd(','));
                }
                #region 其他形式

                if (other_content != "")
                {
                    string[] arr = other_content.Split('♣');
                    Free_UserOtherApply model = new Free_UserOtherApply();

                    model.Year = Convert.ToInt32(arr[0]);
                    model.ConvertScore = Convert.ToDecimal(arr[1]);
                    model.Memo = arr[4];
                    model.ApplyTime = DateTime.Now;
                    model.ApplyUserID = CurrentUser.UserId;
                    model.ApplyType = 1;///
                    model.Status = type;//
                    model.ApproveStatus = 0;
                    model.DepApproveStatus = 0;
                    model.IsDelete = 0;
                    model.tScore = Convert.ToDecimal(arr[2]);
                    model.CPAScore = Convert.ToDecimal(arr[3]);
                    model.ApplyName = arr[5];
                    model.ApplyID = Convert.ToInt32(arr[6]);
                    model.ID = UserOtherApply_Id;
                    model.typeForm = 0;
                    model.CreateUserID = 0;

                    if (iFree_UserOtherApply.UpdateFree_UserOtherApply(model))
                    {
                        result = 1;
                        other_teacher = 1;
                        userApplyID = model.ID;
                    }
                    else
                    {
                        other_teacher = 1;
                        result = 0;
                    }
                }

                #endregion

                #region 授课学时

                if (teacher_content != "")
                {
                    Free_UserOtherApply model = new Free_UserOtherApply();

                    string[] arr = teacher_content.Split('♣');
                    model = new Free_UserOtherApply();
                    model.ID = UserOtherApply_Id;
                    model.ApplyName = arr[0];
                    model.Year = Convert.ToInt32(arr[1]);
                    model.Memo = arr[2];
                    model.ApplyTime = DateTime.Now;
                    model.ApplyUserID = CurrentUser.UserId;
                    model.ApplyType = 3;
                    model.ApproveStatus = 0;
                    model.DepApproveStatus = 0;
                    model.IsDelete = 0;
                    model.ConvertScore = 0;
                    model.Status = type;
                    model.ApplyID = Convert.ToInt32(arr[3]);
                    model.typeForm = 0;
                    model.CreateUserID = 0;
                    model.CreateTime = DateTime.MinValue;
                    string[] array = arr[4].Split('◆');
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
                    model.tScore = tScore;
                    model.CPAScore = CPAScore;

                    Free_UserApplyTeacherDetails details;
                    if (iFree_UserOtherApply.UpdateFree_UserOtherApply(model))
                    {
                        details = new Free_UserApplyTeacherDetails();
                        details.userApplyID = UserOtherApply_Id;

                        //删除原来的
                        iFree_UserApplyTeacherDetails.DeleteFree_UserApplyTeacherDetails(UserOtherApply_Id.ToString());

                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i] != "")
                            {
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

                #endregion

                }
            }

            #region 发布，发邮件
            if (publish == 1)
            {
                var infoModel = freeBL.GetSingleByID(userApplyID, -1);
                var name = "";
                if (CurrentUser.CPA == "是")
                {
                    switch (infoModel.ConvertType)
                    {
                        case 0:
                            name = "CPA继续教育其他形式学时";
                            break;
                        case 1:
                            name = "内部培训其他形式学时";
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
                SendApplyMail(type, infoModel.ApplyTime, infoModel.Year, name);
            }

            #endregion
            return Json(new
            {
                result = result,
                userApplyID = userApplyID,
                other_teacher = other_teacher
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取当前人员年度内申请的所有学时
        /// </summary>
        /// <returns></returns>
        public JsonResult GetScoreByUser(int year, int ID)
        {
            try
            {
                var Scorelist = iFree_UserOtherApply.GetMaxScore("  ApplyUserID=" + CurrentUser.UserId + " and ApplyID =" + ID
                                                                 + " and Year=" + year).FirstOrDefault();
                Scorelist = Scorelist == null ? new Free_UserOtherApply() : Scorelist;
                return Json(new { result = 1, dataList = Scorelist }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, dataList = new Free_UserOtherApply() }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 免修
        /// <summary>
        /// 免修
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_Exemption()
        {
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;
            ViewBag.yearList = freeBL.GetYearList();
            ViewBag.IsTimeIn = FreeIsTimeIn(60, 1);
            ViewBag.IsInTrainGrade = FreeIsInTrainGrade(1);
            return View();
        }

        /// <summary>
        /// 新增免修 第一步
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_ExemptionAdd()
        {
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;

            return View();
        }

        /// <summary>
        /// 新增免修 第一步
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_ExemptionDeptAdd()
        {
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;
            return View("Free_ExemptionAdd");
        }


        /// <summary>
        /// 新增免修 第二步
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_ExemptionAdd_Second(int ID, int UserOtherApply_Id = 0, int year = 0)
        {
            ViewBag.year = year == 0 ? DateTime.Now.Year : year;
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;
            ViewBag.ID = ID;
            ViewBag.UserOtherApply_Id = UserOtherApply_Id;
            ViewBag.model = freeConfigBL.GetFree_ApplyConfig(" ID=" + ID);
            var userApply = new Free_UserOtherApply();
            //加载申请上传证明资料
            if (UserOtherApply_Id != 0)
            {

                userApply = iFree_UserOtherApply.GetFree_UserOtherApplyList(" ID=" + UserOtherApply_Id).FirstOrDefault();
                var UserApplyFiles = ifree_userapplyfiles.GetFree_UserApplyFilesList("  type=0 and userApplyID in(" + UserOtherApply_Id + ")");

                userApply.UserApplyFiles = UserApplyFiles;

                ViewBag.year = userApply.Year;
            }

            return View(userApply);
        }

        /// <summary>
        /// 免修详情
        /// </summary>
        /// <param name="typeForm">0自己新增 1 导入</param>
        /// <returns></returns>
        public ActionResult Free_ExemptionDetails(int ID, int typeForm)
        {
            ViewBag.ID = ID;
            ViewBag.typeForm = typeForm;
            var ApproveUser = freeBL.GetFreeSingleByID(ID);
            if (typeForm == 0)
            {
                ApproveUser.ApplyFileList = ifree_userapplyfiles.GetFree_UserApplyFilesList(" type=0 and  userApplyID in(" + ID + ")");
            }
            else
            {
                ApproveUser.ApplyFileList = freeBL.GetApplyFile(ApproveUser.fromID);
            }
            return View(ApproveUser);
        }

        /// <summary>
        /// 查询我申请的免修
        /// </summary>
        /// <param name="approveType">Tab页切换 0全部 1待审批 2审批通过 3审批拒绝</param>
        /// <returns></returns>
        public JsonResult GetFree_UserApplyList(string othername, int bm_type, int px_type, string starttime, string endtime,
            int year, int cpa, int approveType = 0, string jsRenderSortField = "ApplyTime desc", int search = 0)
        {
            try
            {
                #region 条件
                var flag = false;
                var listYear = new List<int>();
                string sql = " foa.isdelete=0  and (typeForm=0 OR (typeForm=1 AND CreateUserID=0 and Status=1)) AND foa.ApplyType=2   AND foa.ApplyUserID=" + CurrentUser.UserId;
                if (approveType > 0)
                {
                    switch (approveType)
                    {
                        case 1://待审批
                            sql += " and (ApproveStatus=0 or (ApproveStatus=1 and DepApproveStatus=0))";
                            break;
                        case 2://审批通过
                            sql += " and ApproveStatus=1 and DepApproveStatus=1";
                            break;
                        case 3://审批拒绝
                            sql += " and (ApproveStatus=2 or DepApproveStatus=2)";
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(othername))
                {
                    flag = true;
                    sql += " and fac.FreeName like '%" + othername.ReplaceSql() + "%'";
                }

                if (bm_type != 0)
                {
                    flag = true;
                    if (bm_type == 1)
                    {
                        sql += " and foa.ApproveStatus=0";
                    }
                    else if (bm_type == 2)
                    { sql += " and foa.ApproveStatus=1 and Free_UserOtherApply.DepApproveStatus!=2"; }
                    else if (bm_type == 3)
                    { sql += " and foa.ApproveStatus=2"; }
                    else if (bm_type == 4)
                    {
                        sql += " and (foa.ApproveStatus=1 and foa.DepApproveStatus=2 )";
                    }
                }


                if (px_type != 0)
                {
                    flag = true;
                    if (px_type == 1)
                    {
                        sql += " and (foa.DepApproveStatus=0 and foa.ApproveStatus=1)";
                    }
                    else if (px_type == 2)
                    {
                        sql += " and foa.DepApproveStatus=1";
                    }
                    else if (px_type == 3)
                    {
                        sql += " and foa.DepApproveStatus=2";
                    }

                }

                if (!string.IsNullOrEmpty(starttime))
                {
                    flag = true;
                    sql += " and foa.ApplyTime>='" + starttime + "'";
                }

                if (!string.IsNullOrEmpty(endtime))
                {
                    flag = true;
                    sql += " and foa.ApplyTime<='" + endtime + "'";
                }

                if (year != 0)
                {
                    listYear.Add(year);
                    sql += " and foa.Year=" + year;
                }
                else
                {
                    listYear = freeBL.GetYearList();
                }

                if (cpa != 2)
                {
                    flag = true;
                    sql += " and fac.ApplyType in (" + cpa + ",2)";
                }

                #endregion


                int i = 1;
                var joinflag = true;
                var CPACreateDateflag = true;
                ///自行申报
                var MyApplyList = iFree_UserOtherApply.GetFreeApply(sql).SortListByField(jsRenderSortField);
                MyApplyList.ForEach(p => { p.isout = 1; });

                #region 自动折算
                Free_UserOtherApply model = null;
                var freeConfigList = AllSystemConfigs.Where(p => p.ConfigType == 63 && listYear.Contains(p.LastUpdateTime.Year));

                var outList = new List<Free_UserOtherApply>();
                if (search == 0 || !flag)
                {
                    foreach (var freeConfig in freeConfigList)
                    {
                        if (!(freeConfig == null || freeConfig.ConfigValue == ""))
                        {
                            var yearInt = freeConfig.LastUpdateTime.Year;

                            model = new Free_UserOtherApply();
                            var configvalue = freeConfig.ConfigValue.Split(';');
                            var tDate = yearInt + "-" + configvalue[0].Split(',')[0];
                            var tScore = configvalue[0].Split(',')[1];
                            var CPADate = yearInt + "-" + configvalue[1].Split(',')[0];
                            var CPAScore = configvalue[1].Split(',')[1];

                            model.ApplyName_New = "免修折算";
                            model.typeForm = 4;
                            model.configtype = 2;
                            if (CurrentUser.JoinDate != null)
                            {
                                if (CurrentUser.JoinDate >= Convert.ToDateTime(tDate))
                                {
                                    model.tScore = Convert.ToDecimal(tScore);
                                    model.GettScore = model.tScore;
                                }
                                else
                                {
                                    joinflag = false;
                                }
                            }
                            else
                            {
                                joinflag = false;
                            }
                            if (CurrentUser.CPACreateDate != null)
                            {
                                if (CurrentUser.CPACreateDate >= Convert.ToDateTime(CPADate))
                                {
                                    model.CPAScore = Convert.ToDecimal(CPAScore);
                                    model.GetCPAScore = model.CPAScore;
                                }
                                else
                                {
                                    CPACreateDateflag = false;
                                }
                            }
                            else
                            {
                                CPACreateDateflag = false;
                            }
                            model.ApplyTime = DateTime.MinValue;
                            model.isout = 2;
                            model.DepApproveStatus = 1;
                            // model.ApplyTime = CurrentUser.JoinDate.Value;
                        }

                        if (model != null)
                        {
                            if (CPACreateDateflag || joinflag)
                            {
                                MyApplyList.Add(model);
                            }
                        }
                    }
                    outList = MyApplyList.Where(P => P.isout == 2).ToList();
                }
                #endregion

                return Json(new
                {
                    MyApplyList = MyApplyList.Where(P => P.isout == 1),
                    allApplyCount = GetMyFreeApply(0, MyApplyList),
                    MyApplyCount = GetMyFreeApply(1, MyApplyList.Where(P => P.isout == 1).ToList()),
                    outApplyCount = GetMyFreeApply(2, outList),
                    outList = outList
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                  {
                      MyApplyList = new List<Free_UserOtherApply>()
                  }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取合计
        /// </summary>
        /// <param name="type">0 合计 1 自行申报合计 2 自动折算合计</param>
        /// <param name="MyApplyList"></param>
        /// <returns></returns>
        public dynamic GetMyFreeApply(int type, List<Free_UserOtherApply> MyApplyList)
        {
            var ApplyName = "";
            switch (type)
            {
                case 0:
                    ApplyName = "合计";
                    break;
                case 1:
                    ApplyName = "自行申报合计";
                    break;
                case 2:
                    ApplyName = "自动折算合计";
                    break;
            }
            var MyApplyCount = new
            {
                ApplyName = ApplyName,
                tScore = MyApplyList.Sum(p => p.tScore),
                CPAScore = MyApplyList.Sum(p => p.CPAScore),
                GettScore = MyApplyList.Sum(p => p.GettScore),
                GetCPAScore = MyApplyList.Sum(p => p.GetCPAScore),
                ApplyTimeStr = "N/A",
                ApproveStatusStr = "N/A",
                DepApproveStatusStr = "N/A",
                isout = type
            };
            return MyApplyCount;
        }

        /// <summary>
        /// 获取免修
        /// </summary>
        /// <param name="ApplyContent"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetFreeList(string FreeName, int year, int tApplyType = -1, int cpaApplyType = -1, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalCount;
                string where = " 1=1 and year=" + year;
                if (!string.IsNullOrEmpty(FreeName))
                {
                    where += " and FreeName like '%" + FreeName.ReplaceSql() + "%'";
                }
                if (tApplyType != -1)
                {
                    if (tApplyType == 1)
                    {
                        where += " and (ApplyType=0 or ApplyType=2)";
                    }
                    else
                    {
                        where += " and (ApplyType=1)";
                    }
                }
                if (cpaApplyType != -1)
                {
                    if (cpaApplyType == 1)
                    {
                        where += " and (ApplyType=1 or ApplyType=2)";
                    }
                    else
                    {
                        where += " and (ApplyType=0)";
                    }
                }
                var list = freeConfigBL.GetFreeApplyList(out totalCount, where, pageIndex, pageSize);
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
                    dataList = new List<Free_OtherApplyConfig>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存我的免修申请
        /// </summary>
        /// <returns></returns>
        public JsonResult SubimtFree(Free_UserOtherApply model, int status, int year, string removefileID = "")
        {


            var ID = 0;
            if (model.ID > 0)
            {
                //删除证明资料
                if (removefileID != "")
                {
                    ifree_userapplyfiles.DeleteFree_UserApplyFiles(removefileID.ToString().TrimEnd(','));
                }
                ID = model.ID;
                var single = iFree_UserOtherApply.GetFree_UserOtherApplyList(" ID=" + ID).FirstOrDefault();
                single.Year = year;
                single.Status = status;
                single.ApplyTime = DateTime.Now;
                single.Memo = model.Memo;
                single.ApproveStatus = 0;
                single.DepApproveStatus = 0;
                single.DepReason = "";
                single.DepTrainReason = "";
                iFree_UserOtherApply.UpdateFree_UserOtherApply(single);

            }
            else
            {
                model.Status = status;
                model.ApplyTime = DateTime.Now;
                model.ApplyType = 2;
                model.ApplyUserID = CurrentUser.UserId;
                model.ConvertScore = 0;
                model.ApproveStatus = 0;
                model.DepApproveStatus = 0;
                model.DepReason = "";
                model.DepTrainReason = "";
                model.GetCPAScore = 0;
                model.GettScore = 0;
                model.IsDelete = 0;
                model.typeForm = 0;
                iFree_UserOtherApply.AddFree_UserOtherApply(model);
                ID = model.ID;

            }
            if (status == 1)
            {
                #region 发布，发邮件
                var infomodel = freeBL.GetFreeSingleByID(ID);
                var name = "";
                if (CurrentUser.CPA == "是")
                {
                    switch (infomodel.ApplyType)
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

                SendApplyMail(1, infomodel.ApplyTime, infomodel.Year, name);
                #endregion
            }
            return Json(new { result = 1, ID = ID }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///  当前申请的免修是否在范围之内
        /// </summary>
        /// <param name="year"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult IfExistFreeItem(int year, int ID, int UserOtherApply_Id = 0)
        {
            try
            {
                var tCount = 0;
                var CPACount = 0;

                var freeConfig = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == year).FirstOrDefault();

                if (!(freeConfig == null || freeConfig.ConfigValue == ""))
                {
                    var configvalue = freeConfig.ConfigValue.Split(';');
                    var tDate = year + "-" + configvalue[0].Split(',')[0];
                    var CPADate = year + "-" + configvalue[1].Split(',')[0];
                    if (CurrentUser.JoinDate >= Convert.ToDateTime(tDate))
                    {
                        tCount++;
                    }
                    if (CurrentUser.CPACreateDate >= Convert.ToDateTime(CPADate))
                    {
                        CPACount++;
                    }
                }
                var single = freeBL.GetFreeItemByUser(year, CurrentUser.UserId).FirstOrDefault();

                tCount = tCount + (single == null ? 0 : single.tCount);
                CPACount = CPACount + (single == null ? 0 : single.CPACount);

                return Json(new { tCount = tCount, CPACount = CPACount }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { sumCount = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 其他有组织形式
        /// <summary>
        /// 其他有组织形式
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_OtherOrganization()
        {
            ViewBag.IsTimeIn = FreeIsTimeIn(60, 2);
            ViewBag.IsInTrainGrade = FreeIsInTrainGrade(2);
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;
            ViewBag.yearList = freeBL.GetOrgYearList();
            return View();
        }

        /// <summary>
        /// 添加其他有组织形式 第一步
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_OtherOrganizationAdd()
        {
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;


            return View();
        }

        /// <summary>
        /// 添加其他有组织形式 第二步
        /// </summary>
        /// <param name="type">0 cpa 1 所内 2 全部</param>
        /// <returns></returns>
        public ActionResult Free_OtherOrganizationAdd_Second(int ID, int type, int year, int ApprlyID = 1)
        {
            ViewBag.ID = ID;
            ViewBag.ApprlyID = ApprlyID;
            ViewBag.type = type;
            ViewBag.year = year;

            var model = AllSystemConfigs.Where(p => p.ConfigType == 65 && p.LastUpdateTime.Year == year).FirstOrDefault();

            ViewBag.CPAScore = (model == null || model.ConfigValue == "") ? "0" : model.ConfigValue.Split(',')[0];//.StringToInt32();
            ViewBag.tScore = (model == null || model.ConfigValue == "") ? "0" : model.ConfigValue.Split(',')[1];//.StringToInt32();

            return View();
        }



        /// <summary>
        /// 获取其他人通过的课程
        /// </summary>
        /// <returns></returns>
        public ActionResult User_OtherPassForm()
        {
            return View();
        }

        /// <summary>
        /// 其他有组织形式的编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_OtherOrganizationEdit(int ID, int year)
        {
            ViewBag.ID = ID;
            ViewBag.year = year;
            var model = free_UserApplyOtherBL.GetFree_UserApplyOtherForm(" ID=" + ID).FirstOrDefault();

            var single = AllSystemConfigs.Where(p => p.ConfigType == 65 && p.LastUpdateTime.Year == year).FirstOrDefault();
            ViewBag.CPAScore = model == null ? 0 : single.ConfigValue.Split(',')[0].StringToDouble();
            ViewBag.tScore = model == null ? 0 : single.ConfigValue.Split(',')[1].StringToDouble();


            List<Free_UserApplyFiles> UserApplyFiles = ifree_userapplyfiles.GetFree_UserApplyFilesList(" type=1 and userApplyID in(" + ID + ")");
            ViewBag.UserApplyFiles = UserApplyFiles.ToList();

            return View(model);
        }

        public ActionResult Free_OtherOrganizationDetails(int ID)
        {
            ViewBag.ID = ID;

            var ApproveUser = freeBL.GetOrgSingleByID(ID);
            ApproveUser.ApplyFileList = freeBL.GetApplyFile(ID, 1);
            return View(ApproveUser);
        }



        /// <summary>
        /// 查询我申请的免修
        /// </summary>
        /// <param name="approveType">Tab页切换 0全部 1待审批 2审批通过 3审批拒绝</param>
        /// <returns></returns>
        public JsonResult GetFree_UserOrgApplyList(string othername, int bm_type, int px_type, string starttime, string endtime,
            int year, int cpa, int approveType = 0, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = "ApplyDateTime desc")
        {
            try
            {
                string sql = " isdelete=0 AND ApplyUserID=" + CurrentUser.UserId;
                if (approveType > 0)
                {
                    switch (approveType)
                    {
                        case 1://待审批
                            sql += " and (ApproveStatus=0 or (ApproveStatus=1 and DepApproveStatus=0))";
                            break;
                        case 2://审批通过
                            sql += " and ApproveStatus=1 and DepApproveStatus=1";
                            break;
                        case 3://审批拒绝
                            sql += " and (ApproveStatus=2 or DepApproveStatus=2)";
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(othername))
                {
                    sql += " and CourseName like '%" + othername.ReplaceSql() + "%'";
                }

                if (bm_type != 0)
                {
                    if (bm_type == 1)
                    {
                        sql += " and ApproveStatus=0";
                    }
                    else if (bm_type == 2)
                    { sql += " and ApproveStatus=1 and DepApproveStatus!=2"; }
                    else if (bm_type == 3)
                    { sql += " and ApproveStatus=2"; }
                    else if (bm_type == 4)
                    {
                        sql += " and ApproveStatus=1 and DepApproveStatus=2";
                    }
                }


                if (px_type != 0)
                {
                    if (px_type == 1)
                    {
                        sql += " and DepApproveStatus=0 and ApproveStatus=1";
                    }
                    else if (px_type == 2)
                    { sql += " and DepApproveStatus=1"; }
                    else if (px_type == 3)
                    { sql += " and DepApproveStatus=2"; }

                }

                if (!string.IsNullOrEmpty(starttime))
                {
                    sql += " and ApplyDateTime>='" + starttime + "'";
                }

                if (!string.IsNullOrEmpty(endtime))
                {
                    sql += " and ApplyDateTime<='" + endtime + "'";
                }

                if (year != 0)
                {
                    sql += " and Year=" + year;
                }

                if (cpa != 2)
                {
                    sql += " and OtherFromID in (" + cpa + ",2)";
                }


                ///自行申报
                var MyApplyList = free_UserApplyOtherBL.GetFree_UserApplyOtherForm(sql, order: jsRenderSortField);
                foreach (var item in MyApplyList)
                {
                    if (item.OtherFromID == 1)
                    {
                        item.CPAScore = 0;
                        item.GetCPAScore = 0;
                    }
                    if (item.OtherFromID == 0)
                    {
                        item.TogetherScore = 0;
                        item.GettScore = 0;
                    }
                }

                int totalCount = MyApplyList.Count();

                var sumT = MyApplyList.Sum(p => p.TogetherScore);
                var sumCPA = MyApplyList.Sum(p => p.CPAScore);
                var getT = MyApplyList.Sum(p => p.GettScore);
                var getCPA = MyApplyList.Sum(p => p.GetCPAScore);

                MyApplyList = MyApplyList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new
                {
                    dataList = MyApplyList,
                    recordCount = totalCount,
                    sumT = sumT,
                    sumCPA = sumCPA,
                    getT = getT,
                    getCPA = getCPA
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                  {
                      dataList = new List<Free_UserApplyOtherForm>(),
                      recordCount = 0
                  }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///其他有组织形式的配置
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOthreOrgList(int year, int fromType, int pageSize = 10, int pageIndex = 1)
        {
            //获取配置
            var model = AllSystemConfigs.Where(p => p.ConfigType == 65 && p.LastUpdateTime.Year == year).FirstOrDefault();
            var CPAScore = (model == null || model.ConfigValue == "") ? 0 : model.ConfigValue.Split(',')[0].StringToDouble();
            var tScore = (model == null || model.ConfigValue == "") ? 0 : model.ConfigValue.Split(',')[1].StringToDouble();


            string where = " 1=1 and IsDelete=0";
            if (year != -1)
            {
                where += " and Year=" + year;
            }
            if (fromType != -1)
            {
                where += " and  FromType in(" + fromType + ",2)";
            }
            var list = fFree_otherfrom.GetOtherFromsList(where);
            var totalcount = list.Count();
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                dataList = list,
                recordCount = totalcount,
                CPAScore = CPAScore,
                tScore = tScore
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取其他人审批通过的项目
        /// </summary>
        public JsonResult GetOtherPassForm(string courseName, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                var where = "1=1";
                if (!string.IsNullOrEmpty(courseName))
                {
                    where += " and CourseName like '%" + courseName.ReplaceSql() + "%'";
                }

                //if (CurrentUser.CPA == "是")
                //{
                //    where += " and OtherFromID!=1";
                //}
                //else
                //{
                //    where += " and OtherFromID!=0";
                //}

                var list = free_UserApplyOtherBL.GetOtherPassForm(CurrentUser.UserId, where, pageIndex, pageSize);
                var totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Free_UserApplyOtherForm>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        //保存课程
        public JsonResult AddOrg(int status, int type, int year)
        {
            var OrgArray = Request.Form["OrgArray"];
            dynamic OrgList = Newtonsoft.Json.JsonConvert.DeserializeObject(OrgArray);
            var listID = new List<int>();
            var ID = 0;
            for (int i = 0; i < OrgList.list.Count; i++)
            {
                var model = new Free_UserApplyOtherForm();
                model.OtherFromID = type;
                model.Year = year;
                model.CourseName = OrgList.list[i].CourseName;
                model.Address = OrgList.list[i].Address;
                model.StartTime = OrgList.list[i].StartTime;
                model.EndTime = OrgList.list[i].EndTime;
                model.TogetherScore = OrgList.list[i].TogetherScore;
                model.CPAScore = OrgList.list[i].CPAScore;
                model.ApplyDateTime = DateTime.Now;
                model.ApplyUserID = CurrentUser.UserId;
                model.Status = status;
                model.ApproveStatus = 0;
                model.DepApproveStatus = 0;
                model.IsDelete = 0;
                model.GettScore = 0;
                model.GetCPAScore = 0;
                model.DepTrainReason = "";
                model.DepReason = "";
                free_UserApplyOtherBL.InsertFree_UserApplyOtherForm(model);
                ID = model.ID;
                listID.Add(model.ID);
            }

            if (status == 1)
            {
                var model = freeBL.GetOrgSingleByID(ID);
                var name = "";
                if (CurrentUser.CPA == "是")
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
                SendApplyMail(2, model.ApplyDateTime, model.Year, name);
            }
            return Json(new { result = 1, listID = (listID.Count == 0 ? "" : string.Join(",", listID)) }, JsonRequestBehavior.AllowGet);
        }

        public ContentResult SubmitAddOrgResource(string userApplyID, string ResourceName, string RealName)
        {
            var flag = false;
            foreach (var ID in userApplyID.Split(','))
            {
                var model = new Free_UserApplyFiles();
                model.userApplyID = ID.StringToInt32();
                model.IsDelete = 0;
                model.ResourceName = ResourceName;
                model.RealName = RealName;
                model.type = 1;
                //文件后缀名
                string suffix = ResourceName.Substring(ResourceName.LastIndexOf(".") + 1).ToLower();
                var swf = new List<string> { "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "pdf" };
                model.ConvertName = RealName;
                if (swf.Contains(suffix))
                {
                    flag = true;
                    model.ConvertName = RealName + ".swf";
                }
                //保存
                var list = ifree_userapplyfiles.AddFree_UserApplyFiles(model);

            }
            if (flag)
            {
                //加入消息队列
                RabbitSet(RealName);
            }
            return Content("1");
        }

        public JsonResult fSubmitOrgUpdateStatus(int id)
        {
            try
            {
                if (free_UserApplyOtherBL.UpdateBywhere(id, " ApproveStatus=0,DepApproveStatus=0,ApplyDateTime=getdate(),Status=1") > 0)
                {
                    var model = freeBL.GetOrgSingleByID(id);
                    var name = "";
                    if (CurrentUser.CPA == "是")
                    {
                        switch (model.OtherFromID)
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

                    }
                    else
                    {
                        name = "内部培训其他有组织形式学时";
                    }
                    SendApplyMail(2, model.ApplyDateTime, model.Year, name);
                }
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

        public JsonResult fdeleteOrgUserOtherApply(int id)
        {
            try
            {
                free_UserApplyOtherBL.UpdateBywhere(id, " IsDelete=1");

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
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="deleteids">修改时 删除的上传资料</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult UpdateOrg(int ID, string deleteids, int type, Free_UserApplyOtherForm model)
        {
            try
            {
                // model = free_UserApplyOtherBL.GetFree_UserApplyOtherForm(" ID" + ID).FirstOrDefault();
                model.Status = type;
                model.ApplyDateTime = DateTime.Now;
                model.ID = ID;
                free_UserApplyOtherBL.UpdateFree_UserApplyOtherForm(model);

                if (deleteids != "")
                {
                    iFree_UserOtherApply.DeleteFree_UserApplyFilesById(deleteids);
                }

                if (model.Status == 1)
                {
                    var infomodel = freeBL.GetOrgSingleByID(ID);
                    var name = "";
                    if (CurrentUser.CPA == "是")
                    {
                        switch (infomodel.OtherFromID)
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
                    }
                    else
                    {
                        name = "内部培训其他有组织形式学时";
                    }
                    SendApplyMail(2, infomodel.ApplyDateTime, infomodel.Year, name);
                }

                return Json(new { result = 1, Content = "保存成功" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, Content = "保存失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  课程名称是否重复
        /// </summary>
        /// <param name="FreeName"></param>
        /// <returns></returns>
        public JsonResult GetExistFreeName(string FreeName, int ID = 0)
        {
            try
            {
                var nameArray = Request.Form["nameArray"];
                var count = free_UserApplyOtherBL.GetExistFreeName(FreeName.ReplaceSql(), ID);
                return Json(new { result = 1, count = count }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, count = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region RabbitMQ配置
        /// <summary>
        /// 文件转换配置
        /// </summary>
        /// <param name="filename"></param>
        public void RabbitSet(string filename)
        {
            #region 文件转换配置
            string fileHead = ConfigurationManager.AppSettings["FreeUplpadUrl"];
            string fileHeadConvert = ConfigurationManager.AppSettings["FreeUplpadConvertUrl"];

            var model = new RabbitMQModel.FileModel
            {
                SourcePath = Server.MapPath(fileHead + "/" + filename),
                TargetPath = Server.MapPath(fileHeadConvert + "/" + filename + ".swf"),
            };
            new RabbitMQClient.RabbitMQHelper(Resource_serverAddress, Resource_userName, Resource_password, Resource_virtualHost,
                               Resource_queue).SendMessage(model);

            #endregion
        }


        #endregion

        #region 批量免修
        public ActionResult Free_BatchFree()
        {
            return View();
        }

        /// <summary>
        /// 新增免修 第二步
        /// </summary>
        /// <returns></returns>
        public ActionResult Free_BatchFree_Second(int ID, int UserOtherApply_Id = 0, int year = 0)
        {
            ViewBag.year = year == 0 ? DateTime.Now.Year : year;
            ViewBag.cpa = CurrentUser.CPA == "是" ? 1 : 0;
            ViewBag.ID = ID;
            ViewBag.UserOtherApply_Id = UserOtherApply_Id;
            ViewBag.model = freeConfigBL.GetFree_ApplyConfig(" ID=" + ID);
            Free_UserOtherApply userApply = new Free_UserOtherApply();
            ViewBag.Status = 0;
            //加载申请上传证明资料
            if (UserOtherApply_Id != 0)
            {

                userApply = iFree_UserOtherApply.GetFree_UserOtherApplyList(" ID=" + UserOtherApply_Id + " and ApplyType=2 AND typeForm=1   AND CreateUserID>0").FirstOrDefault();
                //userApply = iFree_UserOtherApply.GetFree_UserOtherApplyList(" ID=" + UserOtherApply_Id + "  AND typeForm=1   AND CreateUserID>0").FirstOrDefault();
                var UserApplyFiles = ifree_userapplyfiles.GetFree_UserApplyFilesList(" type=2 and  userApplyID in(" + UserOtherApply_Id + ")");

                userApply.UserApplyFiles = UserApplyFiles;

                ///判断是否发布 已经发布 只能修改人员 其他都不能改，没发布 修改都能改
                var model = iFree_AllOtherApplyBL.GetOtherFromsById(UserOtherApply_Id);
                ViewBag.Status = userApply.Status;

                ViewBag.OpenPerson = model.UserIDs;

                ViewBag.PersonList = userBL.GetList(" UserId in (select id from dbo.F_SplitIDs('" + model.UserIDs + "'))");

                ViewBag.year = userApply.Year;

            }

            return View(userApply);
        }

        public ActionResult Free_BatchFreeDetails(int ID)
        {
            ViewBag.ID = ID;
            var ApproveUser = freeBL.GetFreeSingleByID(ID);
            ApproveUser.ApplyFileList = ifree_userapplyfiles.GetFree_UserApplyFilesList(" type=2 and  userApplyID in(" + ID + ")");


            var model = iFree_AllOtherApplyBL.GetOtherFromsById(ID);
            ViewBag.OpenPerson = model.UserIDs;

            ViewBag.PersonList = userBL.GetList(" UserId in (select id from dbo.F_SplitIDs('" + model.UserIDs + "'))");

            ViewBag.ConvertType = freeConfigBL.GetFree_ApplyConfig(" ID=" + ApproveUser.ApplyID).ApplyType;

            ViewBag.model = freeConfigBL.GetFree_ApplyConfig(" ID=" + ApproveUser.ApplyID);
            return View(ApproveUser);
        }

        /// <summary>
        /// 批量免修
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllFreeDetailsList(string FreeName, int ApplyType, string starttime, string endtime, int pageSize = 10,
            int pageIndex = 1, string jsRenderSortField = "ApplyTime desc")
        {
            try
            {
                string where = " 1=1 and fua.IsDelete=0 ";
                if (!string.IsNullOrEmpty(FreeName))
                {
                    where += " and fac.FreeName like '%" + FreeName.ReplaceSingleSql() + "%'";
                }
                if (ApplyType != 2)
                {
                    where += " and fac.ApplyType in (" + ApplyType + ",2)";
                }
                if (!string.IsNullOrEmpty(starttime))
                {
                    where += " and fua.ApplyTime>='" + starttime + "'";
                }

                if (!string.IsNullOrEmpty(endtime))
                {
                    where += " and fua.ApplyTime<='" + endtime + "'";
                }

                var totalCount = 0;
                var list = iFree_UserOtherApply.GetFreeDetailsList(out totalCount, where, pageIndex, pageSize, jsRenderSortField);
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
                    dataList = new List<ShowFreeDetails>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveBatchFree(int ConvetType, int ID, int UserOtherApplyId, int Year, string userIDs, string Memo, decimal tScore, decimal CPAScore,
            string removefileID = "", string removeUserIDs = "", string haveAddUser = "", int type = 0)
        {
            try
            {
                ///有删除，就先删除
                if (removeUserIDs != "")
                {
                    iFree_UserOtherApply.DeleteUserApply(removeUserIDs, UserOtherApplyId);
                }

                #region 免修
                //免修自动折算
                var userList = new List<Free_UserOtherApply>();
                var freeConfig = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == Year).FirstOrDefault();
                if (!(freeConfig == null || freeConfig.ConfigValue == ""))
                {
                    var configvalue = freeConfig.ConfigValue.Split(';');
                    var tDate = Year + "-" + configvalue[0].Split(',')[0];
                    var CPADate = Year + "-" + configvalue[1].Split(',')[0];

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
                //员工获取的学时
                tScore = ConvetType != 1 ? tScore : 0;
                CPAScore = ConvetType != 0 ? CPAScore : 0;
                userList.AddRange(freeBL.GetFreeItemByUser(Year, -1).ToList());
                #endregion

                var haveUserIDs = userIDs.TrimStart(',').TrimEnd(',') + "," + haveAddUser.TrimStart(',').TrimEnd(',');
                var newUserIDs = "";
                var AddID = 0;
                if (type == 0)
                {
                    foreach (var item in haveUserIDs.TrimStart(',').TrimEnd(',').Split(','))
                    {
                        if (userList.Count(p => p.ApplyUserID == item.StringToInt32()) == 0)
                        {
                            newUserIDs = newUserIDs + "," + item.StringToInt32();
                        }
                    }
                }
                else
                {
                    newUserIDs = haveUserIDs;
                }


                //修改
                if (UserOtherApplyId > 0)
                {
                    #region 更新

                    //删除证明资料
                    if (removefileID != "")
                    {
                        ifree_userapplyfiles.DeleteFree_UserApplyFiles(removefileID.ToString().TrimEnd(','));
                    }


                    var singlemodel = iFree_UserOtherApply.GetFree_UserOtherApplyList(" ID=" + UserOtherApplyId + " and ApplyType=2 AND typeForm=1   AND CreateUserID>0").FirstOrDefault();
                    singlemodel.tScore = tScore;
                    singlemodel.CPAScore = CPAScore;
                    singlemodel.Memo = Memo;
                    singlemodel.Year = Year;
                    //single.Status = type;
                    AddID = singlemodel.ID;
                    if (iFree_UserOtherApply.UpdateFree_UserOtherApply(singlemodel))
                    {
                        Free_AllOtherApply apply = new Free_AllOtherApply();

                        apply.UserIDs = newUserIDs;
                        apply.UserApplyID = UserOtherApplyId;
                        iFree_AllOtherApplyBL.UpdateFree_AllOtherApply(apply);
                    }
                    if (singlemodel.Status == 1)
                    {
                        userIDs = userIDs.TrimStart(',').TrimEnd(',');

                        if (!string.IsNullOrEmpty(userIDs))
                        {
                            var GetUserList = userBL.GetList(" UserId in (" + userIDs + ")");
                            foreach (var userID in userIDs.Split(','))
                            {
                                var userModel = GetUserList.Where(p => p.UserId == userID.StringToInt32()).FirstOrDefault();
                                var singleModel = userList.Where(p => p.ApplyUserID == userID.StringToInt32());
                                var tCount = singleModel.Sum(p => p.tCount);
                                var CPACount = singleModel.Sum(p => p.CPACount);
                                //只能申请2次免修项目，所内一次 cpa一次
                                var flag = false;
                                switch (ConvetType)
                                {
                                    case 0:
                                        if (tCount > 0)
                                        {
                                            flag = true;
                                        }
                                        break;
                                    case 1:
                                        if (CPACount > 0 && userModel.CPA == "是")
                                        {
                                            flag = true;
                                        }
                                        break;
                                    case 2:
                                        if ((CPACount > 0 && userModel.CPA == "是") || tCount > 0)
                                        {
                                            flag = true;
                                        }
                                        break;
                                }

                                if (!flag)
                                {

                                    var single = new Free_UserOtherApply();
                                    single.ApplyID = ID;
                                    single.ApplyName = "";
                                    single.Year = Year;
                                    single.tScore = tScore;
                                    single.CPAScore = userModel.CPA == "是" ? CPAScore : 0;
                                    single.Memo = Memo;
                                    single.Status = 1;
                                    single.ApplyTime = DateTime.Now;
                                    single.ApplyType = 2;
                                    single.ApplyUserID = userID.StringToInt32();
                                    single.ConvertScore = 0;
                                    single.ApproveStatus = 1;
                                    single.DepApproveStatus = 1;
                                    single.DepReason = "";
                                    single.DepTrainReason = "";
                                    single.GetCPAScore = single.CPAScore;
                                    single.GettScore = single.tScore;
                                    single.IsDelete = 0;
                                    single.typeForm = 1;
                                    single.CreateUserID = 0;
                                    single.CreateTime = DateTime.Now;
                                    single.fromID = AddID;
                                    iFree_UserOtherApply.AddFree_UserOtherApply(single);
                                }
                            }
                        }
                    }
                    AddID = UserOtherApplyId;//记录修改时 又上传了附件 保存ID

                    #endregion
                }
                else
                {
                    #region 新增
                    //保存批量信息
                    var model = new Free_UserOtherApply();
                    model.ApplyID = ID;
                    model.ApplyName = "";
                    model.tScore = tScore;
                    model.CPAScore = CPAScore;
                    model.Year = Year;
                    model.Memo = Memo;
                    model.Status = type;
                    model.ApplyTime = DateTime.Now;
                    model.ApplyType = 2;
                    model.ApplyUserID = 0;
                    model.ConvertScore = 0;
                    model.ApproveStatus = 1;
                    model.DepApproveStatus = 1;
                    model.DepReason = "";
                    model.DepTrainReason = "";
                    model.GetCPAScore = 0;
                    model.GettScore = 0;
                    model.IsDelete = 0;
                    model.typeForm = 1;
                    model.CreateUserID = CurrentUser.UserId;
                    model.CreateTime = DateTime.Now;

                    if (iFree_UserOtherApply.AddFree_UserOtherApply(model))
                    {
                        AddID = model.ID;
                        Free_AllOtherApply all = new Free_AllOtherApply();
                        all.UserApplyID = AddID;
                        all.UserIDs = haveUserIDs;
                        iFree_AllOtherApplyBL.AddFree_AllOtherApply(all);
                    }
                    #endregion
                }
                //保存人员信息

                return Json(new { result = 1, ID = AddID, Content = "保存成功" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, ID = 0, Content = "保存失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导出人员名单
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="CreateTimeStr"></param>
        /// <param name="type">0批量其他形式 1免修</param>
        public void ExportUser(int ID, string CreateTimeStr, int type, int ConvertType)
        {

            var PersonList = iFree_AllOtherApplyBL.GetUserDetails(ID);

            var dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("登录名", typeof(string));
            dt.Columns.Add("部门", typeof(string));
            dt.Columns.Add("培训级别", typeof(string));
            dt.Columns.Add("是否CPA", typeof(string));
            dt.Columns.Add("CPA编号", typeof(string));
            dt.Columns.Add("获得所内学时", typeof(string));
            dt.Columns.Add("获得CPA学时", typeof(string));

            var count = 1;
            foreach (var item in PersonList)
            {
                dt.Rows.Add(count++, item.Realname, item.Username, item.DeptName, item.TrainGrade, item.CPA, (item.CPA == "是" ? ("'" + item.CPANO) : "N/A")
                    , item.GettScore, (item.CPA == "是" && ConvertType != 0 ? item.GetCPAScore.ToString() : "N/A"));

            }
            var dtList = new List<DataTable>();
            dtList.Add(dt);
            var export = new Spreadsheet();

            var name = (type == 0 ? "批量导入其他形式（人员名单）" : "批量导入免修（人员名单）");
            new Spreadsheet().Template(name + "——" + CreateTimeStr, null, dt, HttpContext, name, name);

        }

        /// <summary>
        /// 导出申请表格
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="type">0批量其他形式 1免修</param>
        /// <returns></returns>
        public void ExportUserAllForFree(int ID, string CreateTimeStr, int type)
        {
            GenerateWord word = new GenerateWord();
            var name = type == 0 ? "批量导入其他形式申请表格" : "批量导入免修申请表格";

            var css = @"table { border-collapse: collapse; border-spacing: 0;height: 700px }
                        #otherWord td { border: 1px solid; }

                        #otherWord { text-align: center; height: 620px; width: 627px; font-family:Arial,FangSong_GB2312; font-size: 14px}
                        
                        .w18 td { width: 182px; height: 56px; }

                        .w30 { width: 288px; height: 130px; }

                        #joinInfo td { border: 0 none; }";
            if (type == 0)
            {
                word.ExportGenerateWord(HttpContext, css, GetOtherTable(ID, name), name + ".doc");
            }
            else
            {
                word.ExportGenerateWord(HttpContext, css, GetFreeTable(ID, name), name + ".doc");
            }

        }

        /// <summary>
        /// table
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetFreeTable(int ID, string name)
        {
            var Content = "";
            var CPAContent = "";
            var Memo = "";
            var model = iFree_AllOtherApplyBL.GetOtherFromsById(ID);
            var PersonList = userBL.GetList(" UserId in (select id from dbo.F_SplitIDs('" + model.UserIDs + "'))");
            var ApproveUser = freeBL.GetFreeSingleByID(ID);

            var table = new StringBuilder();
            table.AppendFormat(@"<div style='text-align: center'><h3>{0}</h3></div>", name);
            Content = "1、" + ApproveUser.FreeName + " 所内学时：" + ApproveUser.tScore + (ApproveUser.CPA == "是" ? "CPA学时：" + ApproveUser.CPAScore : "") + "情况：" + ApproveUser.Memo;
            CPAContent = "1、" + ApproveUser.FreeName + " 所内学时：" + ApproveUser.tScore + (ApproveUser.CPA == "是" ? "CPA学时：" + ApproveUser.CPAScore : "");
            Memo += string.Format(@"1、情况：{0} ", ApproveUser.Memo);

            #region table内容

            var CPATable = @"<div style='{9}'>
<table border=0 cellspacing=0 cellpadding=0 width=634 height=700
 style='width:475.5pt;height:700pt;border-collapse:collapse;mso-yfti-tbllook:1184;' id='otherWord'>
    <tr class='w18'>
        <td>姓名</td>
        <td>{0}</td>
        <td>性别</td>
        <td>{1}</td>
        <td>CPA证书号码</td>
        <td>{2}</td>
    </tr>
    <tr class='w18' style='text-align: left;'>
         <td>所在会计师事务所</td>
        <td colspan='5'>立信会计师事务所（特殊普通合伙）<br/>{3}</td>
    </tr>
    <tr class='w18' style='text-align: left;'>
        <td>申请确认的学时数</td>
        <td colspan='5'>{4}</td>
    </tr>
    <tr style='height: 56px;text-align: left;'>
        <td>参加继续教育的形式</td>
        <td colspan='5'>{5}</td>
    </tr>
    <tr>
        <td colspan='6' style='border-bottom: 0 none'>
            <div style='float: left; padding-right: 10px;text-align: left;text-align: left;'>参加其他形式培训情况说明:</div>
        </td>
    </tr>
    <tr style='height: 400pt'>
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
</table></div><br>";

            var NOCPATable = @"<div style='{10}'>
<table border=0 cellspacing=0 cellpadding=0 width=634 height=700
 style='width:475.5pt;height:700pt;border-collapse:collapse;mso-yfti-tbllook:1184;' id='otherWord'>
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
    <tr style='height: 56px;text-align: left;'>
        <td>申请确认的学时数</td>
        <td colspan='3'>{6}</td>
    </tr>
    <tr>
        <td colspan='4' style='border-bottom: 0 none'>
            <div style='float: left; padding-right: 10px;text-align: left;text-align: left;'>参加其他形式培训情况说明:</div>
        </td>
    </tr>
    <tr style='height: 400pt'>
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
</table></div><br/>";
            #endregion
            var index = 0;
            foreach (var item in PersonList)
            {
                var style = index == 0 ? "" : "margin-top: 50px;";
                if (item.CPA == "是")
                {
                    table.AppendFormat(CPATable, item.Realname, item.SexStr, item.CPANo, (item.SubordinateSubstation.Contains("上海") ? "" : item.DeptName),
                             ApproveUser.CPAScore, CPAContent, Memo, item.Realname, ApproveUser.ApplyTime.ToString("yyyy年MM月dd日"), style);
                }
                else
                {
                    table.AppendFormat(NOCPATable, item.Realname, item.SexStr, item.DeptName, item.TrainGrade, ApproveUser.Year.ToString().GetCapitalByYear() + "年", item.MobileNum
           , ApproveUser.tScore, Content, item.Realname, ApproveUser.ApplyTime.ToString("yyyy年MM月dd日"), style);
                }
                index++;
            }

            return table.ToString();
        }

        /// <summary>
        /// table
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetOtherTable(int ID, string name)
        {
            var Content = "";
            var CPAContent = "";
            var Memo = "";
            var table = new StringBuilder();


            var model = iFree_AllOtherApplyBL.GetOtherFromsById(ID);
            var PersonList = userBL.GetList(" UserId in (select id from dbo.F_SplitIDs('" + model.UserIDs + "'))");
            var ApproveUser = freeBL.GetSingleByID(ID, -1);


            var otherItemStr = new StringBuilder();

            table.AppendFormat(@"<div style='text-align: center'><h3>{0}</h3></div>", name);

            if (ApproveUser.ApplyType == 3)
            {
                ApproveUser.teacherDetails = teacherDetailsBL.GetFree_UserApplyTeacherDetailsList("  userApplyID=" + ID + " and isdelete=0");

                int i = 1;
                foreach (var item in ApproveUser.teacherDetails)
                {
                    Content += string.Format(@"{0}、{1} 所内学时：{2} {3} 情况：{4}<br/>",
                        i, item.ClassName, item.tScore, (ApproveUser.CPA == "是" ? "CPA学时：" + item.CPAScore : ""), ApproveUser.Memo);
                    CPAContent += string.Format(@"{0}、{1} 所内学时：{2} {3} <br/>",
                        i, item.ClassName, item.tScore, (ApproveUser.CPA == "是" ? "CPA学时：" + item.CPAScore : ""));
                    Memo += string.Format(@"{0}、情况：{1} ", i, ApproveUser.Memo);
                    i++;
                }
            }
            else
            {
                Content = "1、" + ApproveUser.ApplyContent + " 所内学时：" + ApproveUser.tScore + (ApproveUser.CPA == "是" ? "CPA学时：" + ApproveUser.CPAScore : "") + "情况：" + ApproveUser.Memo;
                CPAContent = "1、" + ApproveUser.ApplyContent + " 所内学时：" + ApproveUser.tScore + (ApproveUser.CPA == "是" ? "CPA学时：" + ApproveUser.CPAScore : "");
                Memo += string.Format(@"1、情况：{0} ", ApproveUser.Memo);
            }

            #region table

            var CPATable = @"<div style='{9}'>
<table border=0 cellspacing=0 cellpadding=0 width=634
 style='width:475.5pt;border-collapse:collapse;mso-yfti-tbllook:1184;' id='otherWord'>
    <tr class='w18'>
        <td>姓名</td>
        <td>{0}</td>
        <td>性别</td>
        <td>{1}</td>
        <td>CPA证书号码</td>
        <td>{2}</td>
    </tr>
    <tr class='w18' style='text-align: left;'>
         <td>所在会计师事务所</td>
        <td colspan='5'>立信会计师事务所（特殊普通合伙）<br/>{3}</td>
    </tr>
    <tr class='w18' style='text-align: left;'>
        <td>申请确认的学时数</td>
        <td colspan='5'>{4}</td>
    </tr>
    <tr style='height: 56pt;text-align: left;'>
        <td>参加继续教育的形式</td>
        <td colspan='5'>{5}</td>
    </tr>
    <tr>
        <td colspan='6' style='border-bottom: 0 none'>
            <div style='float: left; padding-right: 10px;text-align: left;text-align: left;'>参加其他形式培训情况说明:</div>
        </td>
    </tr>
    <tr style='height:400pt'>
        <td colspan='6' style='border-top: 0 none; border-bottom: 0 none;vertical-align: top;text-align: left;'>{6}
        </td>
    </tr>
    <tr  >
        <td colspan='6' style='border-top: 0 none'>
            <div style='float: right; padding-right: 10px;text-align: right;'>
                <div>申请人：{7}</div>
                {8}
            </div>

        </td>
    </tr>
</table></div><br/>";
            var NOCPATable = @"<div  style='{10}'>
<table border=0 cellspacing=0 cellpadding=0 width=634 height=750
 style='width:475.5pt;border-collapse:collapse;mso-yfti-tbllook:1184;' id='otherWord'>
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
    <tr style='height: 56pt;text-align: left;'>
        <td>申请确认的学时数</td>
        <td colspan='3'>{6}</td>
    </tr>
    <tr>
        <td colspan='4' style='border-bottom: 0 none'>
            <div style='float: left; padding-right: 10px;text-align: left;text-align: left;'>参加其他形式培训情况说明:</div>
        </td>
    </tr>
    <tr style='height:400pt'>
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
</table><div><br/>";
            #endregion

            int index = 0;
            foreach (var item in PersonList)
            {
                var style = index == 0 ? "" : "margin-top: 50px;";
                if (item.CPA == "是")
                {
                    table.AppendFormat(CPATable, item.Realname, item.SexStr, item.CPANo, (item.SubordinateSubstation.Contains("上海") ? "" : item.DeptName),
         ApproveUser.CPAScore, CPAContent, Memo, item.Realname, ApproveUser.ApplyTime.ToString("yyyy年MM月dd日"), style);
                }
                else
                {
                    table.AppendFormat(NOCPATable, item.Realname, item.SexStr, item.DeptName, item.TrainGrade, ApproveUser.Year.ToString().GetCapitalByYear() + "年", item.MobileNum
             , ApproveUser.tScore, Content, item.Realname, ApproveUser.ApplyTime.ToString("yyyy年MM月dd日"), style);
                }
                index++;
            }
            return table.ToString();
        }
        #endregion

        #region 公共

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="type">0其他形式 1免修 2有组织形式</param>
        /// <param name="applyTime"></param>
        /// <param name="year"></param>
        /// <param name="name"></param>
        public void SendApplyMail(int type, DateTime applyTime, int year, string name)
        {
            var model = AllSystemConfigs.Where(p => p.ConfigType == 60).FirstOrDefault();
            var flag = 0;
            var now = DateTime.Now.ToString("g");
            var endTime = model != null ? model.ConfigValue.Split(';')[type].Split(',')[1] : "";

            var content = GetFormworkContent(12);
            var userList = freeBL.GetManageUserByDeptID(CurrentUser.DeptId, CurrentUser.LeaderID);

            var emailList = new List<KeyValuePair<string, string>>();
            if (userList.Count > 0)
            {
                foreach (var item in userList)
                {
                    var c = string.Format(content, item.Realname, CurrentUser.Realname, applyTime.ToString("yyyy年MM月dd日"), year, name,
                        endTime.StringToDate(0).ToString("yyyy年MM月dd日"));
                    emailList.Add(new KeyValuePair<string, string>(item.Email, c));
                }
            }
            else//没有部门负责人，没有上级
            {

            }
            if (emailList.Count > 0)
                SendEmail(emailList, "您的部门有人申请学时，快来审批吧！");

        }


        public ActionResult Free_ApproveHead()
        {
            ViewBag.Realname = CurrentUser.Realname;
            return View();
        }
        #endregion

        #region 用于个人中心的查询
        /// <summary>
        /// 其他形式
        /// </summary>
        /// <param name="type">0 所内 1 CPA</param>
        /// <returns></returns>
        public JsonResult GetUserFreeOtherList(int type, string name = "", int year = -1, string zhouqi = "", int typeForm = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " typeForm,ApplyID,ApplyTime asc")
        {
            try
            {
                var isZhouqi = false;//是否是周期查询
                var listYear = new List<int>();
                #region 周期
                if (!string.IsNullOrEmpty(zhouqi))
                {
                    isZhouqi = true;
                    var ArrayValue = zhouqi.Split('—');
                    DateTime startTime = DateTime.Parse(ArrayValue[0].Replace("年", "-").Replace("月", "-") + "01 0:00:00");
                    DateTime endTime = DateTime.Parse(ArrayValue[1].Replace("年", "-").Replace("月", "-") + "01 0:00:00").AddMonths(1).AddSeconds(-1);
                    for (int i = 0; i <= endTime.Year - startTime.Year; i++)
                    {
                        listYear.Add(startTime.Year + i);
                    }
                }
                #endregion
                string where = "1=1 and ApplyUserID=" + CurrentUser.UserId;
                string outwhere = where;
                if (year != -1)
                {
                    where += " and fuo.Year=" + year;
                    outwhere += " and fuo.Year=" + year;
                }
                if (isZhouqi)
                {
                    where += string.Format(" and fuo.Year in ({0})", string.Join(",", listYear));
                    outwhere += string.Format(" and fuo.Year in ({0})", string.Join(",", listYear));
                }
                if (type == 0)
                {
                    where += " AND foa.ConvertType!=0";
                }
                else
                {
                    where += " AND foa.ConvertType!=1";
                    outwhere += " and typeForm!=2";
                }

                if (!string.IsNullOrEmpty(name))
                {
                    where += " and ApplyContent like '%" + name.ReplaceSql() + "%'";
                    outwhere += " and CourseName like '%" + name.ReplaceSql() + "%'";
                }

                var list = iFree_UserOtherApply.GetUserFreeOtherList(where, outwhere, jsRenderSortField);

                if (typeForm != -1)
                {
                    list = list.Where(p => p.typeForm == typeForm).ToList();
                }
                var totalcount = list.Count;
                decimal sum = 0;
                if (type == 0)
                {
                    sum = list.Sum(p => p.GettScore);
                    list = list.Where(p => p.GettScore > 0).ToList();
                }
                else
                {
                    sum = list.Sum(p => p.GetCPAScore);
                    list = list.Where(p => p.GetCPAScore > 0).ToList();
                }
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new { result = 1, dataList = list, recordCount = totalcount, sum = sum }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, dataList = new List<FreeUseScore>(), recordCount = 0 }, JsonRequestBehavior.AllowGet);
            }


        }

        /// <summary>
        /// 免修
        /// </summary>
        /// <param name="type">	0 所内免修 1 CPA免修</param>
        /// <returns></returns>
        public JsonResult GetUserFreeList(int type, string name = "", int year = -1, string zhouqi = "", int typeForm = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " typeForm,ApplyID,ApplyTime asc")
        {
            try
            {
                var isZhouqi = false;//是否是周期查询
                var listYear = new List<int>();
                #region 周期
                if (!string.IsNullOrEmpty(zhouqi))
                {
                    isZhouqi = true;
                    var ArrayValue = zhouqi.Split('—');
                    DateTime startTime = DateTime.Parse(ArrayValue[0].Replace("年", "-").Replace("月", "-") + "01 0:00:00");
                    DateTime endTime = DateTime.Parse(ArrayValue[1].Replace("年", "-").Replace("月", "-") + "01 0:00:00").AddMonths(1).AddSeconds(-1);
                    for (int i = 0; i <= endTime.Year - startTime.Year; i++)
                    {
                        listYear.Add(startTime.Year + i);
                    }
                }
                #endregion

                string where = "1=1 and ApplyUserID=" + CurrentUser.UserId;

                if (year != -1)
                {
                    listYear.Add(year);
                    where += " and fuo.Year=" + year;
                }
                if (isZhouqi)
                {
                    where += string.Format(" and fuo.Year in ({0})", string.Join(",", listYear));
                }
                if (type == 0)
                {
                    where += " AND foa.ApplyType!=1";
                }
                else
                {
                    where += " AND foa.ApplyType!=0";
                }

                var list = iFree_UserOtherApply.GetUserFreeList(where, order: jsRenderSortField);

                #region 自动折算
                foreach (var item in listYear)
                {
                    var nowyear = year == -1 ? DateTime.Now.Year : year;
                    var model = new FreeUseScore();
                    var freeConfig = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == item).FirstOrDefault();

                    if (!(freeConfig == null || freeConfig.ConfigValue == ""))
                    {
                        var configvalue = freeConfig.ConfigValue.Split(';');
                        var tDate = nowyear + "-" + configvalue[0].Split(',')[0];
                        var tScore = configvalue[0].Split(',')[1];
                        var CPADate = nowyear + "-" + configvalue[1].Split(',')[0];
                        var CPAScore = configvalue[1].Split(',')[1];

                        model.ApplyContent = "免修折算";
                        model.typeForm = 4;
                        model.ApplyTime = DateTime.Now;
                        if (CurrentUser.JoinDate != null)
                        {
                            if (CurrentUser.JoinDate >= Convert.ToDateTime(tDate))
                            {
                                model.GettScore = Convert.ToDecimal(tScore);
                                if (type == 0)
                                {
                                    list.Add(model);
                                }
                            }
                        }
                        if (CurrentUser.CPACreateDate != null)
                        {
                            if (CurrentUser.CPACreateDate >= Convert.ToDateTime(CPADate))
                            {
                                model.GetCPAScore = Convert.ToDecimal(CPAScore);
                                if (type == 1)
                                {
                                    list.Add(model);
                                }
                            }

                        }
                    }
                }
                #endregion

                if (typeForm != -1)
                {
                    list = list.Where(p => p.typeForm == typeForm).ToList();
                }
                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(p => p.ApplyContent.Contains(name.ReplaceSql())).ToList();
                }
                var totalcount = list.Count;
                decimal sum = 0;
                if (type == 0)
                {
                    sum = list.Sum(p => p.GettScore);
                    list = list.Where(p => p.GettScore > 0).ToList();
                }
                else
                {
                    sum = list.Sum(p => p.GetCPAScore);
                    list = list.Where(p => p.GetCPAScore > 0).ToList();
                }
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new { result = 1, dataList = list, recordCount = totalcount, sum = sum }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, dataList = new List<FreeUseScore>(), recordCount = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 其他形式有组织形式
        /// </summary>
        /// <param name="type">	0 所内 1 CPA</param>
        /// <returns></returns>
        public JsonResult GetUserFreeOrgList(int type, string name = "", int year = -1, string zhouqi = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " typeForm,ApplyID,ApplyTime asc")
        {
            try
            {
                var isZhouqi = false;//是否是周期查询
                var listYear = new List<int>();

                #region 周期
                if (!string.IsNullOrEmpty(zhouqi))
                {
                    isZhouqi = true;
                    var ArrayValue = zhouqi.Split('—');
                    DateTime startTime = DateTime.Parse(ArrayValue[0].Replace("年", "-").Replace("月", "-") + "01 0:00:00");
                    DateTime endTime = DateTime.Parse(ArrayValue[1].Replace("年", "-").Replace("月", "-") + "01 0:00:00").AddMonths(1).AddSeconds(-1);
                    for (int i = 0; i <= endTime.Year - startTime.Year; i++)
                    {
                        listYear.Add(startTime.Year + i);
                    }
                }
                #endregion

                string where = "1=1 and ApplyUserID=" + CurrentUser.UserId;
                if (year != -1)
                {
                    where += " and fuo.Year=" + year;
                }
                if (isZhouqi)
                {
                    where += string.Format(" and fuo.Year in ({0})", string.Join(",", listYear));
                }
                if (type == 0)
                {
                    where += " AND fuo.OtherFromID!=0";
                }
                else
                {
                    where += " AND fuo.OtherFromID!=1";
                }
                if (!string.IsNullOrEmpty(name))
                {
                    where += " and CourseName like '%" + name.ReplaceSql() + "%'";
                }
                var list = iFree_UserOtherApply.GetUserFreeOrgList(where, order: jsRenderSortField);
                var totalcount = list.Count;
                decimal sum = 0;
                if (type == 0)
                {
                    sum = list.Sum(p => p.GettScore);
                    list = list.Where(p => p.GettScore > 0).ToList();
                }
                else
                {
                    sum = list.Sum(p => p.GetCPAScore);
                    list = list.Where(p => p.GetCPAScore > 0).ToList();
                }
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new { result = 1, dataList = list, recordCount = totalcount, sum = sum }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, dataList = new List<FreeUseScore>(), recordCount = 0 }, JsonRequestBehavior.AllowGet);
            }


        }
        #endregion
    }
}
