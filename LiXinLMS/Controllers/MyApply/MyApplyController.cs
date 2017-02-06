using System;
using System.Web.Mvc;
using LiXinCommon;
using LiXinModels;
using LiXinInterface.CourseLearn;
using LiXinInterface.MyApproval;
using LiXinModels.CourseLearn;
using System.Collections.Generic;
using LiXinBLL.SystemManage;

namespace LiXinControllers
{
    public class MyApplyController : BaseController
    {
        protected ICourseOrder courseOrderBL;
        protected IMyApproval myAppBL;
        public MyApplyController(ICourseOrder _courseOrderBL, IMyApproval _myAppBL)
        {
            courseOrderBL = _courseOrderBL;
            myAppBL = _myAppBL;
        }

        #region 我的申请

        public ActionResult MyApply()
        {
            return View();
        }

        /// <summary>
        /// 我的请假申请
        /// </summary>
        /// <returns></returns>
        public ActionResult MyLeaveApply()
        {
            return View();
        }

        /// <summary>
        /// 我的补预订申请
        /// </summary>
        /// <returns></returns>
        public ActionResult MyComplementResApply()
        {
            return View();
        }

        /// <summary>
        /// 我的逾时申请
        /// </summary>
        /// <returns></returns>
        public ActionResult MyTimeOutApply()
        {
            return View();
        }




        public JsonResult GetMyLeaveList(string course, int Approval, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {
            int totalCount = 0;
            string where = string.Format(@" Co_Course.IsDelete = 0 
                                            and Cl_CourseOrder.userid = {0}
                                            and Co_Course.publishflag =1
                                            and Cl_CourseOrder.isleave = 1
                                            ", CurrentUser.UserId);

            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
            if (Approval == 1)
                where += " and (Cl_CourseOrder.approvalflag = 0 or (Cl_CourseOrder.approvalflag > 0 and Cl_CourseOrder.ApprovalDateTime > Cl_CourseOrder.ApprovalLimitTime))";
            if (Approval == 2)
                where += " and Cl_CourseOrder.approvalflag > 0 and Cl_CourseOrder.ApprovalDateTime <= Cl_CourseOrder.ApprovalLimitTime";
            var list = courseOrderBL.GetMyLeaveList(out totalCount, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var appFlag = item.ApprovalFlag > 0 ? (item.ApprovalDateTime > item.ApprovalLimitTime ? 0 : item.ApprovalFlag) : 0;
                var temp = new
                {
                    Course_Name = item.Course_Name.HtmlXssEncode(),
                    CourseLength = item.CourseLength,
                    IsMustStr = item.IsMustStr,
                    CourseTime = item.CourseTime,
                    CourseTimesOrder = item.CourseTimesOrder,
                    TotalCourseTimes = item.TotalCourseTimes,
                    TeacherName = item.TeacherName.HtmlXssEncode(),
                    LeaveTimeStr = item.LeaveTimeStr,
                    ApprovalStatusStr = Approval == 0 ? (appFlag == 0 && item.ApprovalLimitTime < DateTime.Now ? "逾时审批" : ((Enums.ApprovalStatus)appFlag).ToString()) : ((Enums.ApprovalStatus)appFlag).ToString(),
                    ApprovalRealName = item.ApprovalRealName,
                    ApprovalDateTimeStr = appFlag > 0 ? item.ApprovalDateTime.Value.ToString("yyyy-MM-dd HH:mm") : "——",
                    ApprovalFlag = item.ApprovalFlag,
                    ApprovalMemo = item.ApprovalMemo.HtmlXssEncode(),
                    SortStr = item.SortStr,
                    CourseWay = "集中授课"
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyComplementResList(string course, int Approval, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {
            int totalCount = 0;
            string where = string.Format(@" Co_Course.IsDelete = 0 
                                            and Cl_MakeUpOrder.userid = {0}
                                            and Co_Course.publishflag =1
                                            ", CurrentUser.UserId);

            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
            if (Approval == 1)
                where += " and (Cl_MakeUpOrder.approvalflag = 0 or (Cl_MakeUpOrder.approvalflag > 0 and Cl_MakeUpOrder.ApprovalDateTime > Cl_MakeUpOrder.ApprovalLimitTime))";
            if (Approval == 2)
                where += " and Cl_MakeUpOrder.approvalflag > 0 and Cl_MakeUpOrder.ApprovalDateTime <= Cl_MakeUpOrder.ApprovalLimitTime";
            var list = courseOrderBL.GetMyComplementResList(out totalCount, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var appFlag = item.ApprovalFlag > 0 ? (item.ApprovalDateTime > item.ApprovalLimitTime ? 0 : item.ApprovalFlag) : 0;
                var temp = new
                {
                    Course_Name = item.Course_Name.HtmlXssEncode(),
                    OrderStatus = item.OrderStatus,
                    MakeUpApprovalFlag = item.MakeUpApprovalFlag,
                    CourseLength = item.CourseLength,
                    IsMustStr = item.IsMustStr,
                    CourseTime = item.CourseTime,
                    CourseTimesOrder = item.CourseTimesOrder,
                    TotalCourseTimes = item.TotalCourseTimes,
                    TeacherName = item.TeacherName.HtmlXssEncode(),
                    GetScore = item.GetScore,
                    GetScoreFlag=item.GetScoreFlag,
                    ApprovalFlag = appFlag,
                    ApprovalStatus = Approval == 1 ? (item.ApprovalLimitTime < DateTime.Now ? "逾时审批" : ((Enums.ApprovalStatus)appFlag).ToString()) : ((Enums.ApprovalStatus)appFlag).ToString(),
                    ApprovalRealName = item.realname,
                    ApprovalMemo = appFlag == 2 ? item.ApprovalMemo : "",
                    ApprovalDateTime = appFlag > 0 ? item.ApprovalDateTime.Value.ToString("yyyy-MM-dd HH:mm") : "——",
                    SortStr = item.SortStr,
                    CourseWay = "集中授课"
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyTimeOutList(string course, int Approval, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {
            int totalCount = 0;
            string where = string.Format(@" Co_Course.IsDelete = 0 
                                            and Cl_TimeOutOrder.userid = {0}
                                            and Co_Course.publishflag =1
                                            ", CurrentUser.UserId);

            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
            if (Approval == 1)
                where += " and Cl_TimeOutOrder.approvalflag = 0 ";
            if (Approval == 2)
                where += " and Cl_TimeOutOrder.approvalflag > 0 ";
            var list = courseOrderBL.GetMyTimeOutList(out totalCount, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    Course_Name = item.Course_Name.HtmlXssEncode(),
                    OrderStatus = item.OrderStatus,
                    MakeUpApprovalFlag = item.MakeUpApprovalFlag,
                    CourseLength = item.CourseLength,
                    IsMustStr = item.IsMustStr,
                    CourseTime = item.CourseTime,
                    CourseTimesOrder = item.CourseTimesOrder,
                    TotalCourseTimes = item.TotalCourseTimes,
                    TeacherName = item.TeacherName.HtmlXssEncode(),
                    GetScore = item.GetScore,
                    ApprovalFlag = item.ApprovalFlag,
                    ApprovalStatus = ((Enums.ApprovalStatus)item.ApprovalFlag).ToString(),
                    ApprovalRealName = item.realname,
                    ApprovalMemo = item.ApprovalFlag == 2 ? item.ApprovalMemo : "",
                    ApprovalDateTime = item.ApprovalFlag > 0 ? item.ApprovalDateTime.Value.ToString("yyyy-MM-dd HH:mm") : "——"
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyTimeOutLeaveList(string course, int Approval, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {
            int totalCount = 0;
            string where = string.Format(@" Co_Course.IsDelete = 0 
                                            and Cl_TimeOutLeave.userid = {0}
                                            and Co_Course.publishflag =1
                                            ", CurrentUser.UserId);

            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
            if (Approval == 1)
                where += " and Cl_TimeOutLeave.approvalflag = 0 ";
            if (Approval == 2)
                where += " and Cl_TimeOutLeave.approvalflag > 0 ";
            var list = courseOrderBL.GetMyTimeOutListLeave(out totalCount, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    Course_Name = item.Course_Name.HtmlXssEncode(),
                    OrderStatus = item.OrderStatus,
                    MakeUpApprovalFlag = item.MakeUpApprovalFlag,
                    CourseLength = item.CourseLength,
                    IsMustStr = item.IsMustStr,
                    CourseTime = item.CourseTime,
                    CourseTimesOrder = item.CourseTimesOrder,
                    TotalCourseTimes = item.TotalCourseTimes,
                    TeacherName = item.TeacherName.HtmlXssEncode(),
                    GetScore = item.GetScore,
                    ApprovalFlag = item.ApprovalFlag,
                    ApprovalStatus = ((Enums.ApprovalStatus)item.ApprovalFlag).ToString(),
                    ApprovalRealName = item.realname,
                    ApprovalMemo = item.ApprovalFlag == 2 ? item.ApprovalMemo : "",
                    ApprovalDateTime = item.ApprovalFlag > 0 ? item.ApprovalDateTime.Value.ToString("yyyy-MM-dd HH:mm") : "——"
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 逾时审核申请

        public ActionResult TimeOutApply()
        {
            return View();
        }

        public ActionResult TimeOutApplyDetail(int id)
        {
            ViewBag.model = courseOrderBL.GetTimeOutApplyDetail(id);
            return View();
        }

        public JsonResult GetTimeOutApplyList(string realname, int timeout, string traingrade, int approval = 99, string course = "", int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {
            int totalCount = 0;
            string where = string.Format(@" Co_Course.IsDelete = 0 
                                            and Co_Course.publishflag =1
                                            and Sys_User.IsDelete = 0
                                            and Cl_MakeUpOrder.ApprovalUser = '{0}'
                                            and (
                                                    (Cl_MakeUpOrder.approvalflag = 0 and Cl_MakeUpOrder.ApprovalLimitTime < '{1}' ) or 
                                                    (Cl_MakeUpOrder.approvalflag > 0 
                                                        and Cl_MakeUpOrder.ApprovalDateTime > Cl_MakeUpOrder.ApprovalLimitTime)
                                                )
                                            and (Cl_MakeUpOrder.IsTimeOut = {2} or {2} = 99 )
                                            {3}
                                            ", CurrentUser.JobNum, DateTime.Now, timeout,
                                             (approval >= 0 ? string.Format("and (Cl_MakeUpOrder.TimeOutPassFlag = {0} or {0} = 99 )", approval) : "and Cl_MakeUpOrder.IsTimeOut = 0"));

            if (!string.IsNullOrWhiteSpace(realname))
                where += string.Format(" and Sys_User.RealName like '%{0}%'", realname.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(traingrade))
                where += string.Format(" and ('{0}' like '%'+Sys_User.TrainGrade+'%'  and (sys_user.traingrade is not null and sys_user.traingrade <> ''))", traingrade);
            var list = courseOrderBL.GetTimeOutApplyList(out totalCount, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    Id = item.Id,
                    RealName = item.realname,
                    TrainGrade = item.TrainGrade,
                    TimeOutApplyTime = item.TimeOutApplyTime.HasValue ? item.TimeOutApplyTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                    Course_Name = item.Course_Name.HtmlXssEncode(),
                    CourseLength = item.CourseLength,
                    CourseTime = item.CourseTime,
                    IsTimeOut = item.IsTimeOut == 0 ? "否" : "是",
                    ApprovalStatus = item.IsTimeOut == 0 ? "——" : ((Enums.ApprovalStatus)item.TimeOutPassFlag).ToString(),
                    ApprovalLimitTime = item.ApprovalLimitTime.HasValue ? item.ApprovalLimitTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubmitTimeOutApply(string ids, string applymemo)
        {
            try
            {
                courseOrderBL.SubmitTimeOutApply(ids, applymemo);
                return Json(new
                {
                    result = 1,
                    content = "提交成功！"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "提交失败！"
                }, JsonRequestBehavior.AllowGet);
            }
        }



        #endregion

        #region 逾时申请审核

        public ActionResult TimeOutApproval()
        {
            return View();
        }

        public ActionResult TimeOutApprovalDetail(int id, int pageFlag = 0)
        {
            var backUrl = "/MyApply/TimeOutApproval";
            if (pageFlag == 1)
                backUrl = "/PlanManage/Index";
            ViewBag.model = courseOrderBL.GetTimeOutApplyDetail(id);
            ViewBag.backUrl = backUrl;
            return View();
        }

        public JsonResult GetTimeOutApprovalList(string realname, string traingrade, int appflag, string applyname, string applydept, int approval = 99, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {
            int totalCount = 0;
            string where = string.Format(@" Co_Course.IsDelete = 0 
                                            and Co_Course.publishflag =1
                                            and Sys_User.IsDelete = 0
                                            and Cl_MakeUpOrder.IsTimeOut = 1 
                                            and (Cl_MakeUpOrder.TimeOutPassFlag = {0} or {0} = 99 )
                                            {1}
                                            ", approval
                                             , appflag == 99 ? "" : (appflag == 0 ? "and Cl_MakeUpOrder.TimeOutPassFlag = 0" : "and Cl_MakeUpOrder.TimeOutPassFlag > 0"));

            if (!string.IsNullOrWhiteSpace(realname))
                where += string.Format(" and Sys_User.RealName like '%{0}%'", realname.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(applyname))
                where += string.Format(" and Sys_ApplyUser.RealName like '%{0}%'", applyname.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(applydept))
                where += string.Format(" and Sys_ApplyUser.deptname like '%{0}%'", applydept.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(traingrade))
                where += string.Format(" and ('{0}' like '%'+Sys_User.TrainGrade+'%'  and (sys_user.traingrade is not null and sys_user.traingrade <> ''))", traingrade);
            var list = courseOrderBL.GetTimeOutApprovalList(out totalCount, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    Id = item.Id,
                    RealName = item.realname,
                    TrainGrade = item.TrainGrade,
                    Course_Name = item.Course_Name.HtmlXssEncode(),
                    CourseLength = item.CourseLength,
                    CourseTime = item.CourseTime,
                    ApplyUserRealName = item.ApplyUserRealName,
                    TimeOutDateTime = item.TimeOutDateTime.Value.ToString("yyyy-MM-dd HH:mm"),
                    ApplyUserDeptName = item.ApplyUserDeptName,
                    ApprovalStatus = ((Enums.ApprovalStatus)item.TimeOutPassFlag).ToString(),
                    TimeOutPassFlag = item.TimeOutPassFlag
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTimeOutApplyMemo(int id)
        {
            return Json(courseOrderBL.GetTimeOutApplyDetail(id).TimeOutMemo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubmitTimeOutApproval(string ids, string approvalmemo, int approval)
        {
            try
            {
                courseOrderBL.SubmitTimeOutApproval(ids, approvalmemo, approval);
                return Json(new
                {
                    result = 1,
                    content = "审批成功！"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "审批失败！"
                }, JsonRequestBehavior.AllowGet);
            }
        }



        #endregion

        #region 请假审批

        public ActionResult LeaveApply()
        {
            return View();
        }

        public JsonResult GetLeaveApplyList(string course, string realname, int Approval, int pageSize = 20, int pageIndex = 1)
        {
            int totalCount = 0;
            string where = " Co_Course.IsDelete = 0 and Co_Course.publishflag =1 and Cl_CourseOrder.isleave = 1";

            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(realname))
                where += string.Format(" and Sys_User.realname like '%{0}%'", realname.ReplaceSql());
            if (Approval == 0)
                where += " and Cl_CourseOrder.approvalflag = 0 ";
            if (Approval == 2)
                where += " and Cl_CourseOrder.approvalflag > 0 ";
            var list = courseOrderBL.GetMyLeaveList(out totalCount, where, (pageIndex - 1) * pageSize, pageSize);
            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApplyLeave(int id, int flag)
        {
            try
            {
                var model = courseOrderBL.Get(id);
                model.ApprovalFlag = flag;
                model.ApprovalDateTime = DateTime.Now;
                model.ApprovalMemo = "已审批";
                courseOrderBL.Update(model);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 逾时审批

        public ActionResult TimeOutApplyApproval()
        {
            return View();
        }



        public JsonResult GetTimeOutApplyApprovalList(string course, string realname, int Approval, int pageSize = 20, int pageIndex = 1)
        {
            int totalCount = 0;
            string where = " Co_Course.IsDelete = 0 and Co_Course.publishflag =1 and Cl_CourseOrder.OrderStatus = 3";

            if (!string.IsNullOrWhiteSpace(course))
                where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(realname))
                where += string.Format(" and Sys_ApplyUser.realname like '%{0}%'", realname.ReplaceSql());
            if (Approval == 0)
                where += " and Cl_TimeOutOrder.approvalflag = 0 ";
            if (Approval == 2)
                where += " and Cl_TimeOutOrder.approvalflag > 0 ";
            var list = courseOrderBL.GetTimeOutApplyApprovalList(out totalCount, where, (pageIndex - 1) * pageSize, pageSize);
            int n = 0;
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    Id = item.Id,
                    realname = item.ApplyUserRealName,
                    ApprovalRealName = item.realname,
                    MakeUpTime = item.MakeUpTime.HasValue ? item.MakeUpTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                    Course_Name = item.Course_Name.HtmlXssEncode(),
                    CourseLength = item.CourseLength,
                    CourseTime = item.CourseTime,
                    ApprovalStatusStr = ((Enums.ApprovalStatus)item.ApprovalFlag).ToString(),
                    ApprovalFlag = item.ApprovalFlag,
                    ApprovalDateTime = (item.ApprovalDateTime.HasValue && item.ApprovalFlag > 0) ? item.ApprovalDateTime.Value.ToString("yyyy-MM-dd HH:mm") : "——",
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApprovalTimeOut(int id, int flag)
        {
            try
            {
                var model = courseOrderBL.GetOneTimeOutOrder(id);
                model.ApprovalFlag = flag;
                model.ApprovalDateTime = DateTime.Now;
                model.ApprovalMemo = "已审批";
                courseOrderBL.UpdateTimeOutOrderStatus(model);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


        #region 我的请假逾时申请

        public ActionResult MyTimeOutOrder()
        {
            return View();
        }

        public ActionResult TimeApply()
        {

            return View();
        }

        public ActionResult MyTimeOutApplyDetail(int id)
        {
            var where = " Id=" + id;


            int totalCount = 0;
            var list = myAppBL.GetMyTimeOutOrder(out totalCount, 1, int.MaxValue, where);
            var model = list.Count == 0 ? new Cl_CourseOrder() : list[0];
            ViewBag.model = model;
            return View(model);
        }

        public ActionResult MyTimeOutApprovalDetail(int id)
        {
            var where = " Id=" + id;

            int totalCount = 0;
            var list = myAppBL.GetTimeOutLeave(out totalCount, 1, int.MaxValue, where);
            var model = list.Count == 0 ? new Cl_CourseOrder() : list[0];
            ViewBag.model = model;
            // ViewBag.AppReason = AppReason;
            return View(model);
        }

        public JsonResult GetMyTimeOutOrder(int type = 99, string realname = "", string traingrade = "", int approval = 99, string course = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = "Id desc")
        {
            try
            {
                var where = " 1=1  And ApprovalUser=" + CurrentUser.JobNum;

                #region 条件
                if (type != 99)
                {
                    where += type == 1 ? " and ApplyCount>0" : " and ApplyCount=0";
                }

                if (!string.IsNullOrWhiteSpace(realname))
                    where += string.Format(" and RealName like '%{0}%'", realname.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(course))
                    where += string.Format(" and course_Name like '%{0}%'", course.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(traingrade))
                    where += string.Format(" and ('{0}' like '%'+TrainGrade+'%'  and (traingrade is not null and traingrade <> ''))", traingrade);
                if (approval != 99)
                {
                    where += approval == -1 ? " and TrainAppFlag IS NULL" : " and TrainAppFlag=" + approval;
                }

                #endregion
                int totalCount = 0;
                var list = myAppBL.GetMyTimeOutOrder(out totalCount, pageIndex, pageSize, where, jsRenderSortField);
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
                    dataList = new List<Cl_CourseOrder>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult InsertCl_TimeOutLeave(string applymemo, int id)
        {
            try
            {
                var leader = (new Sys_LeaderBL()).GetLeaderIdByUserId(CurrentUser.UserId);
                myAppBL.InsertCl_TimeOutLeave(applymemo, id, leader == null ? "0" : leader.LeaderID);
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

        #region 请假逾时申请审批
        public ActionResult MyTimeOutApproval()
        {
            return View();
        }

        public ActionResult TimeApproval()
        {
            return View();
        }

        public ActionResult MyTimeOutApplyOrder()
        {
            return View();
        }

        public ActionResult MyTimeOutApplyLeave()
        {
            return View();
        }

        /// <summary>
        /// 请假逾时申请审核
        /// </summary>
        public JsonResult GetTimeOutLeave(int type = 99, int approval = 99, string realname = "", string traingrade = "", string applyname = "", string applydept = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " Id desc")
        {
            try
            {
                var where = " 1=1";

                #region 条件
                if (type != 99)
                {
                    where += type == 1 ? " and TrainAppFlag>0" : " and TrainAppFlag=0";
                }
                if (!string.IsNullOrWhiteSpace(realname))
                    where += string.Format(" and RealName like '%{0}%'", realname.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(applyname))
                    where += string.Format(" and ApplyUserRealName like '%{0}%'", applyname.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(applydept))
                    where += string.Format(" and ApplyUserDeptName like '%{0}%'", applydept.ReplaceSql());
                if (approval != 99)
                {
                    where += " and TrainAppFlag=" + approval;
                }
                #endregion

                int totalCount = 0;
                var list = myAppBL.GetTimeOutLeave(out totalCount, pageIndex, pageSize, where, jsRenderSortField);
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
                    dataList = new List<Cl_CourseOrder>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SubmitMyTimeOutApproval(string ids, string approvalmemo, int approval)
        {
            try
            {
                myAppBL.UpdateTimeOutLeave(ids, approvalmemo, approval);
                return Json(new
                {
                    result = 1,
                    content = "审批成功！"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "审批失败！"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 请假申请审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetSingleTimeOutLeave(int id)
        {
            try
            {
                var model = myAppBL.GetSingleTimeOutLeave(id);
                return Json(model.AppReason, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        public JsonResult ApprovalTest()
        {
            new LiXinBLL.MyApproval.MyApprovalBL().DealMakeUpCourseOrder();
            return Json(new
            {
                result = "执行成功"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
