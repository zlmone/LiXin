using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinInterface.MyApproval;
using LiXinInterface.SystemManage;
using LiXinModels;
using LiXinModels.CourseLearn;
using System.Text.RegularExpressions;

namespace LiXinControllers
{
    /// <summary>
    /// 我的审批记录控制类
    /// </summary>
    public class MyApprovalController : BaseController
    {
        protected IMyApproval Approval;
        protected ISys_TrainGrade TrainGrade;
        protected ICo_Course CourseBL;
        protected ISys_Leader LeaderBL;
        protected ICl_CpaLearnStatus CpaLearn;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="approval"></param>
        public MyApprovalController(IMyApproval approval, ISys_TrainGrade trainGrade, ICo_Course courseBL, ISys_Leader _leaderBL, ICl_CpaLearnStatus cpaLearn)
        {
            Approval = approval;
            TrainGrade = trainGrade;
            CourseBL = courseBL;
            LeaderBL = _leaderBL;
            CpaLearn = cpaLearn;
        }

        #region 页面呈现

        /// <summary>
        /// 我的审批主页
        /// </summary>
        /// <returns></returns>
        public ActionResult MyApprovalMain()
        {
            return View();
        }

        /// <summary>
        /// 我的审批记录详情
        /// </summary>
        /// <returns></returns>
        public ActionResult MyApprovalLeaveInfor(int id)
        {
            var infor = Approval.GetMyApprovalLeaveInfor(id);
            infor.ApprovalName = CurrentUser.Realname;
            return View(infor);
        }


        /// <summary>
        /// 我的补预定审批记录详情
        /// </summary>
        /// <returns></returns>
        public ActionResult MyApprovalMakeUpInfor(int id)
        {
            var infor = Approval.GetMyApprovalMakeUpInfor(id);
            infor.ApprovalName = CurrentUser.Realname;
            return View(infor);
        }

        /// <summary>
        /// 我的逾时审批记录详情
        /// </summary>
        /// <returns></returns>
        public ActionResult MyApprovalOutTimeInfor(int id)
        {
            var infor = Approval.GetMyApprovalOutTimeInfor(id);
            infor.ApprovalName = CurrentUser.Realname;
            return View(infor);
        }

        /// <summary>
        /// 我的逾时审批记录详情(请假)
        /// </summary>
        /// <returns></returns>
        public ActionResult MyApprovalOutTimeLeaveInfor(int id)
        {
            var infor = Approval.GetMyApprovalOutTimeLeaveInfor(id);
            infor.ApprovalName = CurrentUser.Realname;
            return View(infor);
        }

        /// <summary>
        /// 我的违纪情况审批
        /// </summary>
        /// <returns></returns>
        public ActionResult MyApprovalPrincipleInfor(int id)
        {
            var infor = Approval.GetMyApprovalPrincipleInfor(id);
            infor.ApprovalName = CurrentUser.Realname;
            return View(infor);
        }

        /// <summary>
        /// 我的违纪情况查询
        /// </summary>
        /// <returns></returns>
        public ActionResult MyPrincipleList()
        {
            return View();
        }

        /// <summary>
        /// 我的违纪情况详情
        /// </summary>
        /// <returns></returns>
        public ActionResult MyPrincipleInfor(int id)
        {
            var infor = Approval.GetMyPrincipleInfor(id);
            return View(infor);
        }

        /// <summary>
        /// 违纪申辩
        /// </summary>
        /// <returns></returns>
        public ActionResult EditExplain()
        {
            return View();
        }
        /// <summary>
        /// 违纪审核管理
        /// </summary>
        /// <returns></returns>
        public ActionResult PrincipleManage()
        {
            return View();
        }

        /// <summary>
        /// 审批申辩
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PrincipleApproval(int id)
        {
            ViewBag.Attendce = Approval.GetPrinReason(id);
            return View();
        }

        /// <summary>
        /// 审批申辩详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PrincipleApprovalInfor(int id)
        {
            var infor = Approval.GetPrincipleInfor(id);
            infor.ApprovalName = CurrentUser.Realname;
            return View(infor);
        }

        /// <summary>
        /// 我的违纪情况申请记录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MyPrincipleAppRecord()
        {
            return View();
        }

        /// <summary>
        /// 课程违纪查看（培训负责人）
        /// </summary>
        /// <returns></returns>
        public ActionResult PrincipleTrainManage(string p)
        {
            //Session["copage"] = pageIndex + "㉿" + courseName + "㉿" + realName + "㉿" + dpname + "㉿" + grade + "㉿" + aat + "㉿" + status + "㉿" + attstatus;

            ViewBag.page = 1;
            ViewBag.cscourseName = "请输入搜索课程";
            ViewBag.csrealName = "请输入搜索姓名";
            ViewBag.csdpname = "请输入搜索部门/分所";
            ViewBag.csgrade = -1;
            ViewBag.aat = -1;
            ViewBag.status = -1;
            ViewBag.attstatus = -1;
            


            if (p != null && p != "" && p == "1")
            {
                if (Session["copage"] != null)
                {
                    string sess = Session["copage"].ToString();
                    string[] cs = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = cs[0];
                    ViewBag.cscourseName = cs[1] == "" ? "请输入搜索课程" : cs[1];
                    ViewBag.csrealName = cs[2] == "" ? "请输入搜索姓名" : cs[2];
                    ViewBag.csdpname = cs[3] == "" ? "请输入搜索部门/分所" : cs[3];
                    ViewBag.csgrade = cs[4];
                    ViewBag.aat = cs[5];
                    ViewBag.status = cs[6];
                    ViewBag.attstatus = cs[7];

                }
            }

            return View();
        }
        /// <summary>
        /// 违纪情况详情
        /// </summary>
        /// <returns></returns>
        public ActionResult PrincipleTrainInfor(int id)
        {
            var infor = Approval.GetPrincipleInfor(id);
            return View(infor);
        }

        /// <summary>
        /// 我的逾时审批（补预定）
        /// </summary>
        /// <returns></returns>
        public ActionResult MyApprovalOrder()
        {
            return View();
        }


        #endregion

        #region 数据加载

        /// <summary>
        /// 获取我的请假审批列表
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="realName">姓名</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="isApp">是否已审批</param>
        /// <param name="jsRenderSortField">排序字段</param>
        /// <param name="type"> 1:请假；2：补预定；3：补预定逾时审批，4请假逾时</param>
        /// <returns></returns>
        public JsonResult GetMyApprovalLeaveList(string courseName = "", string realName = "", int pageIndex = 1, int pageSize = 20, int isApp = 0, string jsRenderSortField = "l desc", int type = 1)
        {
            int totalCount;
            List<BaseApprovalInfor> list;
            switch (type)
            {
                case 2:
                    {
                        var sqlwhere = string.Format("{0} {1} {2} {3} ", courseName.Trim() == "" ? "" : " and c.CourseName like '%" + courseName.Trim().ReplaceSql() + "%' ", realName.Trim() == "" ? "" : " and b.Realname like '%" + realName.Trim().ReplaceSql() + "%' ", isApp == -1 ? "" : (isApp == 0 ? " and a.ApprovalFlag=0 " : " and a.ApprovalFlag<>0 "), " and a.ApprovalUser='" + CurrentUser.JobNum + "' ");
                        var sqlorderby = string.Format(" order by a.{0} {1} ", jsRenderSortField.Split(' ')[0] == "l" ? "LeaveTime" : "ApprovalDateTime", jsRenderSortField.Split(' ')[1]);
                        list = Approval.GetMyApprovalMakeUpPaging(out totalCount, sqlwhere, (pageIndex - 1) * pageSize, pageSize, sqlorderby);
                    }
                    break;
                case 3:
                    {
                        var sqlwhere = string.Format("{0} {1} {2} {3} ", courseName.Trim() == "" ? "" : " and c.CourseName like '%" + courseName.Trim().ReplaceSql() + "%' ", realName.Trim() == "" ? "" : " and b.Realname like '%" + realName.Trim().ReplaceSql() + "%' ", isApp == -1 ? "" : (isApp == 0 ? " and a.ApprovalFlag=0 " : " and a.ApprovalFlag<>0 "), " and a.ApprovalUser='" + CurrentUser.JobNum + "' ");
                        var sqlorderby = string.Format(" order by a.{0} {1} ", jsRenderSortField.Split(' ')[0] == "l" ? "OutTime" : "ApprovalDateTime", jsRenderSortField.Split(' ')[1]);
                        list = Approval.GetMyApprovalOutTimePaging(out totalCount, sqlwhere, (pageIndex - 1) * pageSize, pageSize, sqlorderby);
                    }
                    break;
                case 4:
                    {
                        var sqlwhere = string.Format("{0} {1} {2} {3} ", courseName.Trim() == "" ? "" : " and c.CourseName like '%" + courseName.Trim().ReplaceSql() + "%' ", realName.Trim() == "" ? "" : " and b.Realname like '%" + realName.Trim().ReplaceSql() + "%' ", isApp == -1 ? "" : (isApp == 0 ? " and a.ApprovalFlag=0 " : " and a.ApprovalFlag<>0 "), " and a.ApprovalUser='" + CurrentUser.JobNum + "' ");
                        var sqlorderby = string.Format(" order by a.{0} {1} ", jsRenderSortField.Split(' ')[0] == "l" ? "OutTime" : "ApprovalDateTime", jsRenderSortField.Split(' ')[1]);
                        list = Approval.GetMyApprovalOutTimePagingLeave(out totalCount, sqlwhere, (pageIndex - 1) * pageSize, pageSize, sqlorderby);
                    }
                    break;
                default:
                    {
                        var sqlwhere = string.Format("{0} {1} {2} {3} ", courseName.Trim() == "" ? "" : " and c.CourseName like '%" + courseName.Trim().ReplaceSql() + "%' ", realName.Trim() == "" ? "" : " and b.Realname like '%" + realName.Trim().ReplaceSql() + "%' ", isApp == -1 ? "" : (isApp == 0 ? " and a.ApprovalFlag=0 " : " and a.ApprovalFlag<>0 "), " and a.ApprovalUser='" + CurrentUser.JobNum + "' ");
                        var sqlorderby = string.Format(" order by a.{0} {1} ", jsRenderSortField.Split(' ')[0] == "l" ? "LeaveTime" : "ApprovalDateTime", jsRenderSortField.Split(' ')[1]);
                        list = Approval.GetMyApprovalLeavePaging(out totalCount, sqlwhere, (pageIndex - 1) * pageSize, pageSize, sqlorderby);
                    }
                    break;
            }
            var newList = new List<object>();
            list.ForEach(p => newList.Add(new
                                             {
                                                 p.ID,
                                                 p.RealName,
                                                 p.CourseCode,
                                                 CourseName = p.CourseName.HtmlXssEncode(),
                                                 p.CourseLength,
                                                 p.SubmitTimeShow,
                                                 p.IsApproval,
                                                 p.ApprovalTimeShow,
                                                 p.ApprovalResult,
                                                 p.TypeFlag
                                             }));

            return Json(new
            {
                result = 1,
                dataList = newList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取需要违纪学时审批记录列表
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="realName">姓名</param>
        /// <param name="endTime">课程结束时间</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="jsRenderSortField">排序字段</param>
        /// <param name="trainGrade">人员培训级别</param>
        /// <param name="status">审批状态</param>
        /// <param name="startTime">课程开始时间</param>
        /// <returns></returns>
        public JsonResult GetMyApprovalPrincipleList(string courseName = "", string realName = "", string trainGrade = "", int status = -1, string startTime = "2000-1-1", string endTime = "2050-1-1", int pageIndex = 1, int pageSize = 20, string jsRenderSortField = "StartTime desc")
        {
            int totalCount;
            List<PrincipleInfor> list;

            var str = "";
            if (Convert.ToDateTime(startTime) != Convert.ToDateTime("2000-1-1") && Convert.ToDateTime(endTime) != Convert.ToDateTime("2050-1-1"))
            {
                str = string.Format(" and (b.StartTime between '{0}' and '{1}') ", Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (Convert.ToDateTime(startTime) != Convert.ToDateTime("2000-1-1") && Convert.ToDateTime(endTime) == Convert.ToDateTime("2050-1-1"))
            {
                str = string.Format(" and b.StartTime >= '{0}' ", Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (Convert.ToDateTime(startTime) == Convert.ToDateTime("2000-1-1") && Convert.ToDateTime(endTime) != Convert.ToDateTime("2050-1-1"))
            {
                str = string.Format(" and b.StartTime <= '{0}' ", Convert.ToDateTime(endTime).ToString("yyyy-MM-dd HH:mm:ss"));
            }

            var sqlwhere = string.Format("{0} {1} {2} {3} {4} {5} ",
                                         courseName.Trim() == ""
                                             ? ""
                                             : " and b.CourseName like '%" + courseName.Trim().ReplaceSql() + "%' ",
                                         realName.Trim() == ""
                                             ? ""
                                             : " and c.Realname like '%" + realName.Trim().ReplaceSql() + "%' ",
                                         status == -1 ? "" : " and a.ApprovalFlag= " + status + " ",
                                         " and a.ApprovalUser='" + CurrentUser.JobNum + "' ",
                                         trainGrade == "-1" ? "" : (" and c.TrainGrade='" + trainGrade + "'"),
                                         str);
            var sqlorderby = string.Format(" order by b.{0} ", jsRenderSortField);
            list = Approval.GetMyApprovalPrinciplePaging(out totalCount, sqlwhere, pageIndex, pageSize, sqlorderby);

            var newList = new List<object>();
            list.ForEach(p => newList.Add(new
            {
                p.ID,
                p.UserID,
                p.RealName,
                CourseName = p.CourseName.HtmlXssEncode(),
                p.CourseID,
                p.CourseLength,
                p.TrainGrade,
                p.LessLength,
                p.DepartName,
                p.CourseStartTimeShow,
                p.CourseEndTimeShow,
                p.StatusShow
            }));

            return Json(new
            {
                result = 1,
                dataList = newList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取我的违纪情况列表
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="endTime">课程结束时间</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="jsRenderSortField">排序字段</param>
        /// <param name="aast">申辩状态</param>
        /// <param name="startTime">课程开始时间</param>
        /// <param name="ast">出勤情况</param>
        /// <param name="aat">是否已申辩</param>
        /// <returns></returns>
        public JsonResult GetMyPrincipleList(string courseName = "", int ast = -1, int aat = -1, int aast = -1, string startTime = "2000-1-1", string endTime = "2050-1-1", int pageIndex = 1, int pageSize = 20, string jsRenderSortField = "StartTime desc")
        {
            int totalCount;
            //时间条件
            var str = "";
            if (Convert.ToDateTime(startTime) != Convert.ToDateTime("2000-1-1") && Convert.ToDateTime(endTime) != Convert.ToDateTime("2050-1-1"))
            {
                str = string.Format(" and (b.StartTime between '{0}' and '{1}') ", Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(endTime).AddDays(1).AddMilliseconds(-1));
            }
            else if (Convert.ToDateTime(startTime) != Convert.ToDateTime("2000-1-1") && Convert.ToDateTime(endTime) == Convert.ToDateTime("2050-1-1"))
            {
                str = string.Format(" and b.StartTime >= '{0}' ", Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (Convert.ToDateTime(startTime) == Convert.ToDateTime("2000-1-1") && Convert.ToDateTime(endTime) != Convert.ToDateTime("2050-1-1"))
            {
                str = string.Format(" and b.StartTime <= '{0}' ", Convert.ToDateTime(endTime).AddDays(1).AddMilliseconds(-1));
            }
            //出勤情况
            var aststr = "";
            if (ast == 2)//缺勤
            {
                aststr += @" and
                (
                   (b.AttFlag=3 and ((a.StartTime is null and a.EndTime is null) or (a.StartTime='2050-1-1' and a.EndTime='2000-1-1'))) or
                   (b.AttFlag=1 and (a.StartTime is null or a.StartTime='2050-1-1')) or 
                   (b.AttFlag=2 and (a.EndTime is null or a.EndTime='2000-1-1'))
                ) ";
            }
            else if (ast == 0)//迟到
            {
                aststr += @" and
                (
                   (b.AttFlag=3 and a.StartTime>b.StartTime and a.EndTime>=b.EndTime) or
                   (b.AttFlag=1 and a.StartTime>b.StartTime)
                ) ";
            }
            else if (ast == 1)//早退
            {
                aststr += @" and
                (
                   (b.AttFlag=3 and a.EndTime<b.EndTime and a.StartTime<=b.StartTime) or 
                   (b.AttFlag=2 and a.EndTime<b.EndTime)
                ) ";
            }
            else if (ast == 3)//迟到且早退
            {
                aststr += aststr += @" and
                (
                   b.AttFlag=3 and a.StartTime>b.StartTime and a.EndTime<b.EndTime
                ) "; 
            }
            var sqlwhere = string.Format("{0} {1} {2} {3} {4} {5} ",
                                         courseName.Trim() == ""
                                             ? ""
                                             : " and b.CourseName like '%" + CodeHelper.ReplaceSql(courseName.Trim()) + "%' ",
                                         " and a.UserId='" + CurrentUser.UserId + "' ",
                                         str,
                                         aststr,
                                         aat == -1 ? "" : (string.Format(" and a.IsApp={0}", aat)),
                                         aast == -1 ? "" : (string.Format(" and a.ApprovalFlag={0}", aast))
                                         );
            var sqlorderby = string.Format(" order by b.{0} ", jsRenderSortField);
            List<PrincipleInfor> list = Approval.GetMyPrinciplePaging(out totalCount, sqlwhere, pageIndex, pageSize, sqlorderby);

            var newList = new List<object>();
            list.ForEach(p => newList.Add(new
            {
                p.ID,
                CourseName = p.CourseName.HtmlXssEncode(),
                p.CourseLength,
                p.CourseTimeShow,
                p.AttStatusShow,
                p.LessLength,
                p.IsAppShow,
                p.StatusShow,
                p.IsApp
            }));

            return Json(new
            {
                result = 1,
                dataList = newList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取违纪情况管理列表(部门负责人管理)
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="endTime">课程结束时间</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="jsRenderSortField">排序字段</param>
        /// <param name="attstatus">出勤情况</param>
        /// <param name="startTime">课程开始时间</param>
        /// <param name="realName">人员姓名查询</param>
        /// <param name="grade">培训级别</param>
        /// <param name="status">审批状态</param>
        /// <returns></returns>
        public JsonResult GetPrincipleManageList(string courseName = "", string realName = "", string grade = "-1", int status = -1, int attstatus = -1, string startTime = "2000-1-1", string endTime = "2050-1-1", int pageIndex = 1, int pageSize = 20, string jsRenderSortField = "a.AppDateTime desc")
        {
            int totalCount;
            List<PrincipleInfor> list;
            //时间条件
            var str = "";
            if (Convert.ToDateTime(startTime) != Convert.ToDateTime("2000-1-1") && Convert.ToDateTime(endTime) != Convert.ToDateTime("2050-1-1"))
            {
                str = string.Format(" and (a.AppDateTime between '{0}' and '{1}') ", Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(endTime).AddDays(1).AddMilliseconds(-1));
            }
            else if (Convert.ToDateTime(startTime) != Convert.ToDateTime("2000-1-1") && Convert.ToDateTime(endTime) == Convert.ToDateTime("2050-1-1"))
            {
                str = string.Format(" and a.AppDateTime >= '{0}' ", Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (Convert.ToDateTime(startTime) == Convert.ToDateTime("2000-1-1") && Convert.ToDateTime(endTime) != Convert.ToDateTime("2050-1-1"))
            {
                str = string.Format(" and a.AppDateTime <= '{0}' ", Convert.ToDateTime(endTime).AddDays(1).AddMilliseconds(-1));
            }

            //出勤情况
            var aststr = "";
            switch (attstatus)
            {
                case 2:
                    aststr += @" and
                (
                   (b.AttFlag=3 and ((a.StartTime is null and a.EndTime is null) or (a.StartTime='2050-1-1' and a.EndTime='2000-1-1'))) or
                   (b.AttFlag=1 and (a.StartTime is null or a.StartTime='2050-1-1')) or 
                   (b.AttFlag=2 and (a.EndTime is null or a.EndTime='2000-1-1'))
                ) ";
                    break;
                case 0:
                    aststr += @" and
                (
                   (b.AttFlag=3 and a.StartTime>b.StartTime and a.EndTime>=b.EndTime) or
                   (b.AttFlag=1 and a.StartTime>b.StartTime)
                ) ";
                    break;
                case 1:
                    aststr += @" and
                (
                   (b.AttFlag=3 and a.EndTime<b.EndTime and a.StartTime<=b.StartTime) or 
                   (b.AttFlag=2 and a.EndTime<b.EndTime)
                ) ";
                    break;
                case 3:
                    aststr += aststr += @" and
                (
                   b.AttFlag=3 and a.StartTime>b.StartTime and a.EndTime<b.EndTime
                ) ";
                    break;
            }
            var sqlwhere = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                                         courseName.Trim() == ""
                                             ? ""
                                             : " and b.CourseName like '%" + CodeHelper.ReplaceSql(courseName.Trim()) + "%' ",
                                         " and a.ApprovalUser='" + CurrentUser.JobNum + "' ",
                                         str,
                                         aststr,
                                         grade == "-1" ? "" : (string.Format(" and c.TrainGrade='{0}'", grade)),
                                         status == -1 ? "" : (string.Format(" and a.ApprovalFlag={0}", status)),
                                         realName.Trim() == ""
                                             ? ""
                                             : " and c.Realname like '%" + CodeHelper.ReplaceSql(realName.Trim()) + "%' "
                                         );
            var sqlorderby = string.Format(" order by {0} ", jsRenderSortField);
            list = Approval.GetPrincipleManagePaging(out totalCount, sqlwhere, pageIndex, pageSize, sqlorderby);

            var newList = new List<object>();
            list.ForEach(p => newList.Add(new
            {
                p.ID,
                p.UserID,
                p.CourseID,
                CourseName = p.CourseName.HtmlXssEncode(),
                p.RealName,
                p.TrainGrade,
                p.AppDateTimeShow,
                p.CourseLength,
                p.CourseTimeShow,
                p.AttStatusShow,
                p.LessLength,
                p.StatusShow,
                p.Status
            }));

            return Json(new
            {
                result = 1,
                dataList = newList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 我的违纪情况申请记录列表
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="endTime">课程结束时间</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页条数</param>    
        /// <param name="jsRenderSortField">排序字段</param>
        /// <param name="startTime">课程开始时间</param>
        /// <param name="ast">出勤情况</param>
        /// <param name="aast">审批状态</param>
        /// <returns></returns>
        public JsonResult GetMyPrincipleRecordPaging(string courseName = "", int ast = -1, int aast = -1, string startTime = "2000-1-1", string endTime = "2050-1-1", int pageIndex = 1, int pageSize = 20, string jsRenderSortField = "StartTime desc")
        {
            int totalCount;
            List<PrincipleInfor> list;
            //时间条件
            var str = "";
            if (Convert.ToDateTime(startTime) != Convert.ToDateTime("2000-1-1") && Convert.ToDateTime(endTime) != Convert.ToDateTime("2050-1-1"))
            {
                str = string.Format(" and (b.StartTime between '{0}' and '{1}') ", Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToDateTime(endTime).AddDays(1).AddSeconds(-1));
            }
            else if (Convert.ToDateTime(startTime) != Convert.ToDateTime("2000-1-1") && Convert.ToDateTime(endTime) == Convert.ToDateTime("2050-1-1"))
            {
                str = string.Format(" and b.StartTime >= '{0}' ", Convert.ToDateTime(startTime).ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (Convert.ToDateTime(startTime) == Convert.ToDateTime("2000-1-1") && Convert.ToDateTime(endTime) != Convert.ToDateTime("2050-1-1"))
            {
                str = string.Format(" and b.StartTime <= '{0}' ", Convert.ToDateTime(endTime).AddDays(1).AddSeconds(-1));
            }
            //出勤情况
            var aststr = "";
            if (ast == 0)
                aststr = string.Format(" and a.StartTime>b.StartTime");
            else if (ast == 1)
                aststr = string.Format(" and a.EndTime<b.EndTime");
            else if (ast == 2)
                aststr = string.Format(" and a.AttStatus=1");
            var sqlwhere = string.Format("{0} {1} {2} {3} {4} ",
                                         courseName.Trim() == ""
                                             ? ""
                                             : " and b.CourseName like '%" + CodeHelper.ReplaceSql(courseName.Trim()) + "%' ",
                                         " and a.UserId='" + CurrentUser.UserId + "' ",
                                         str,
                                         aststr,
                                         aast == -1 ? "" : (string.Format(" and a.ApprovalFlag={0}", aast))
                                         );
            var sqlorderby = string.Format(" order by b.{0} ", jsRenderSortField);
            list = Approval.GetMyPrinciplePaging(out totalCount, sqlwhere, pageIndex, pageSize, sqlorderby);

            var newList = new List<object>();
            list.ForEach(p => newList.Add(new
            {
                p.ID,
                CourseName = p.CourseName.HtmlXssEncode(),
                p.CourseLength,
                p.CourseTimeShow,
                p.AttStatusShow,
                p.LessLength,
                p.IsAppShow,
                p.StatusShow,
                p.IsApp
            }));

            return Json(new
            {
                result = 1,
                dataList = newList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取违纪情况管理列表(培训负责人管理)
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="jsRenderSortField">排序字段</param>
        /// <param name="attstatus">出勤情况</param>
        /// <param name="realName">人员姓名查询</param>
        /// <param name="dpname">部门/分所</param>
        /// <param name="grade">培训级别</param>
        /// <param name="aat">申辩状态</param>
        /// <param name="status">审批状态</param>
        /// <returns></returns>
        public JsonResult GetPrincipleTrainManageList(string courseName = "", string realName = "", string dpname = "", string grade = "-1", int aat = -1, int status = -1, int attstatus = -1, int pageIndex = 1, int pageSize = 20, string jsRenderSortField = "b.StartTime desc")
        {
            if (Session["copage"] != null)
            {
                Session.Remove("copage");
            }
            Session["copage"] = pageIndex + "㉿" + courseName + "㉿" + realName + "㉿" + dpname + "㉿" + grade + "㉿" + aat + "㉿" + status + "㉿" + attstatus;

            int totalCount;
            List<PrincipleInfor> list;
            //出勤情况
            var aststr = "";
            switch (attstatus)
            {
                case 2:
                    aststr += @" and
                (
                   (b.AttFlag=3 and ((a.StartTime is null and a.EndTime is null) or (a.StartTime='2050-1-1' and a.EndTime='2000-1-1'))) or
                   (b.AttFlag=1 and (a.StartTime is null or a.StartTime='2050-1-1')) or 
                   (b.AttFlag=2 and (a.EndTime is null or a.EndTime='2000-1-1'))
                ) ";
                    break;
                case 0:
                    aststr += @" and
                (
                   (b.AttFlag=3 and a.StartTime>b.StartTime and a.EndTime>=b.EndTime) or
                   (b.AttFlag=1 and a.StartTime>b.StartTime)
                ) ";
                    break;
                case 1:
                    aststr += @" and
                (
                   (b.AttFlag=3 and a.EndTime<b.EndTime and a.StartTime<=b.StartTime) or 
                   (b.AttFlag=2 and a.EndTime<b.EndTime)
                ) ";
                    break;
                case 3:
                    aststr += aststr += @" and
                (
                   b.AttFlag=3 and a.StartTime>b.StartTime and a.EndTime<b.EndTime
                ) ";
                    break;
            }

            var sqlwhere = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                                         courseName.Trim() == ""
                                             ? ""
                                             : " and b.CourseName like '%" + CodeHelper.ReplaceSql(courseName.Trim()) + "%' ",
                                         dpname.Trim() == ""
                                             ? ""
                                             : " and c.DeptName like '%" + dpname.Trim() + "%' ",
                                         aststr,
                                         grade == "-1" ? "" : (string.Format(" and c.TrainGrade='{0}'", grade)),
                                         status == -1 ? "" : (string.Format(" and a.ApprovalFlag={0}", status)),
                                         realName.Trim() == ""
                                             ? ""
                                             : " and c.Realname like '%" + CodeHelper.ReplaceSql(realName.Trim()) + "%' ",
                                         aat == -1 ? "" : (string.Format(" and a.IsApp={0}", aat))
                );
            var sqlorderby = string.Format(" order by {0} ", jsRenderSortField);
            list = Approval.GetPrincipleTrainManagePaging(out totalCount, sqlwhere, pageIndex, pageSize, sqlorderby);

            var newList = new List<object>();
            list.ForEach(p => newList.Add(new
            {
                p.ID,
                CourseName = p.CourseName.HtmlXssEncode(),
                p.DepartName,
                p.RealName,
                p.TrainGrade,
                p.AppDateTimeShow,
                p.CourseLength,
                p.CourseTimeShow,
                p.AttStatusShow,
                p.LessLength,
                p.StatusShow,
                p.IsAppShow,
                p.Status
            }));

            return Json(new
            {
                result = 1,
                dataList = newList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取所有的培训级别
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTrainGrade()
        {
            var list = TrainGrade.GetAllTrainGrade();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 违纪申辩
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memo">理由</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult ExplainPrin(int id, string expalnPrin)
        {
            var files = Request.Files;
            var message = "";
            var savename = "";
            var realname = "";
            if (files.Count > 0)
            {
                var exp = Path.GetExtension(files[0].FileName);
                if (files[0].ContentLength/(1024*1024) < 5)
                {
                    realname = files[0].FileName;
                    savename = DateTime.Now.ToString("yyyymmddhhmmss") + (new Random().Next(10, 99).ToString()) + exp;
                    files[0].SaveAs(Server.MapPath(pathurl + savename));
                }
                else
                {
                    message = "文件大小不可以超过5M";
                }
            }
            if (message != "")
            {
                return Json(new
                                {
                                    result = 0,
                                    message
                                }, "text/html", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var leader = LeaderBL.GetLeaderIdByUserId(CurrentUser.UserId);
                var str =
                    string.Format(
                        "update Cl_Attendce set ApprovalUser='{0}',Reason='{1}',ApprovalFlag=0,IsApp=1,AppDateTime='{2}',FileName='{4}',FileRealName='{5}' where Id={3}",
                        leader == null ? CurrentUser.LeaderID : leader.JobNum, expalnPrin.ReplaceSql(),
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id, savename,
                        realname);
                return
                    Json(
                        Approval.ExplainPrin(str) > 0
                            ? new
                                  {
                                      result = 1,
                                      message = ""
                                  }
                            : new
                                  {
                                      result = 0,
                                      message = "审批失败"
                                  }, "text/html",
                        JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 审批违纪申辩
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="memo">理由</param>
        /// <param name="result">审批结果</param>
        /// <param name="userID">用户ID</param>
        /// <param name="courseID">课程ID</param>
        /// <param name="lesslength">扣除学时</param>
        /// <returns></returns>
        public JsonResult ApprovalPrinApp(int id, string memo, int result, int userID, int courseID, decimal lesslength)
        {
            var str = string.Format(
                "update Cl_Attendce set ApprovalMemo='{0}',ApprovalFlag={1},ApprovalDateTime='{2}' where Id={3}", memo.ReplaceSql(),
                result, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), id);
            if (Approval.ExplainPrin(str) > 0)
            {
                //如果审批通过，把扣除的学时还回去
                var whereStr = string.Format(" where CourseId={0} and UserId={1}", courseID, userID);
                var corder = Approval.GetCourseOrder(whereStr);
                if (corder != null)
                {
                    //1、如果是补预定情况，只要不是补预定审批拒绝，审批通过就归还学时
                    //2、非补预定情况，审批通过就归还学时
                    if ((corder.OrderStatus == 3 && corder.MakeUpApprovalFlag != 2 && result == 1) || (corder.OrderStatus != 3 && result == 1))
                    {
                        //修改集中授课学时
                        Approval.ChangeCourseOrderInfor(string.Format("update Cl_CourseOrder set GetScore={0},AttScore={3} where UserId={1} and CourseId={2} ", (corder.GetScore + Math.Abs(lesslength)), userID, courseID, corder.AttFlag == 0 ? corder.AttScore + Math.Abs(lesslength) : corder.AttScore));
                        //如果转换CPA学时，归还CPA学时
                        var co = CourseBL.GetCo_Course(courseID);
                        if(co.IsCPA==1)
                        {
                            var cpalearn = CpaLearn.GetCl_CpaLearnStatusByCourseId(courseID, userID);
                            if(cpalearn!=null && cpalearn.Id>0)
                            {
                                //修改
                                cpalearn.GetLength = cpalearn.GetLength + Math.Abs(lesslength/2);
                                CpaLearn.UpdateCPALearnStatusByModel(cpalearn);
                            }
                        }
                    }
                }

                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = 0
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下载违纪附件
        /// </summary>
        public void LoadPrincipleFile(string path, string name)
        {
            if (System.IO.File.Exists(Server.MapPath(pathurl + path)))
            {
                var filePath = Server.MapPath(pathurl + path); //路径 
                //以字符流的形式下载文件
                var fs = new FileStream(filePath, FileMode.Open);
                var bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.ContentType = "application/octet-stream";
                //通知浏览器下载文件而不是打开
                Response.AddHeader("Content-Disposition",
                                   "attachment;  filename=" + HttpUtility.UrlEncode(name, Encoding.UTF8));
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            else
            {
                Response.Write("此附件已不存在");
            }
        }

        #endregion
    }
}
