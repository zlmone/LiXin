using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.DepLeaveApproval;
using LiXinModels.DepCourseLearn;

namespace LiXinControllers.DepLeaveApproval
{
    public class DepLeaveApprovalController:BaseController
    {
        protected IDepLeaveApproval Approval;
        public DepLeaveApprovalController(IDepLeaveApproval approval)
        {
            Approval = approval;
        }

        #region 页面呈现

        /// <summary>
        /// 请假审批管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DepLeaveApprovalManage()
        {
            return View();
        }

        /// <summary>
        /// 请假详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DepLeaveDetail(int id)
        {
            var config = Approval.GetConfig(CurrentUser.DeptId, 7);
            //var hour = config == null ? 999 : Convert.ToDecimal(config.ConfigValue);
            var model = Approval.GetDepLeaveApprovalInfor(id);
            return View(model);
        }

        /// <summary>
        /// 我的请假申请页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult MyLeaveApprovalList()
        {
            return View();
        }

        /// <summary>
        /// 我的请假申请页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult MyLeaveInfor(int id)
        {
            var config = Approval.GetConfig(CurrentUser.DeptId, 7);
            //var hour = config == null ? 999 : Convert.ToDecimal(config.ConfigValue);
            var model = Approval.GetDepLeaveApprovalInfor(id);
            return View(model);
        }

        #endregion


        #region 方法

        /// <summary>
        /// 获取请假审批列表
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="cname">课程名称</param>
        /// <param name="isMust">是否必修</param>
        /// <param name="appFlag">审批标志</param>
        /// <param name="starttime">请假开始时间</param>
        /// <param name="endtime">请假结束时间</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="jsRenderSortField">排序方式</param>
        /// <returns></returns>
        public JsonResult GetApprovalLeaveList(string name = "", string cname = "", int isMust = -1,int appFlag=-1, string starttime = "2000-1-1", string endtime = "2050-1-1", int pageIndex = 1, int pageSize = 20, string jsRenderSortField = " LeaveTime desc ")
        {
            //获取配置
            //var config = Approval.GetConfig(CurrentUser.DeptId, 7);
            //var hour = config == null ? 999 : Convert.ToDecimal(config.ConfigValue);

            var sqlWhere = "";
            if(!string.IsNullOrEmpty(name.Trim()))
            {
                sqlWhere += string.Format(" and u.RealName like '%{0}%' ",name.Trim().ReplaceSql());
            }
            if (!string.IsNullOrEmpty(cname.Trim()))
            {
                sqlWhere += string.Format(" and c.CourseName like '%{0}%' ", cname.Trim().ReplaceSql());
            }
            if (isMust>=0)
            {
                sqlWhere += string.Format(" and c.IsMust={0} ", isMust);
            }
            if (appFlag >= 0)
            {
                switch (appFlag)
                {
                    case 0://未审批
                        sqlWhere += string.Format(" and o.ApprovalFlag={0} and o.ApprovalLimitTime>getdate() ", appFlag);
                        break;
                    case 1://审批通过
                        sqlWhere += string.Format(" and o.ApprovalFlag={0} and  o.ApprovalLimitTime>=o.ApprovalTime ", appFlag);
                        break;
                    case 2://审批拒绝
                        sqlWhere += string.Format(" and (o.ApprovalFlag={0} or (o.ApprovalLimitTime<o.ApprovalTime) or (o.ApprovalFlag=0 and o.ApprovalLimitTime<getdate())) ", appFlag);
                        break;
                }
            }
            //var deptId = string.Join(",", GetAllSubDepartments().Select(p => p.DepartmentId));
            //if(!string.IsNullOrEmpty(deptId))
            //{
            sqlWhere += string.Format(" and o.ApprovalUser ='{0}' ", CurrentUser.JobNum);
            //}
            sqlWhere += string.Format(" and (o.LeaveTime between '{0}' and '{1}') ", string.IsNullOrEmpty(starttime) ? "2000-1-1" : starttime, string.IsNullOrEmpty(endtime) ? "2050-1-1" : endtime);
            var list = Approval.GetDepLeaveApprovalList(pageIndex, pageSize, ("o." + jsRenderSortField), sqlWhere);
            list.ForEach(p => { p.CourseName = p.CourseName.Replace("'", "’");p.RealName = p.RealName.Replace("'", "’");});
            return Json(new {result = 1, dataList = list, recordCount = list.Count > 0 ? list[0].Totalcount : 0},JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据ID获取当个请假信息
        /// </summary>
        /// <param name="id">请假记录ID</param>
        /// <param name="appFlag">审批结果</param>
        /// <param name="reason">审批理由</param>
        /// <param name="mtype">0:不通知;1:邮件;2:短信;3:邮件and短信</param>
        /// <param name="email">邮箱</param>
        /// <param name="phone">手机号</param>
        /// <param name="name">请假人</param>
        /// <param name="cname">课程名</param>
        /// <returns></returns>
        public JsonResult UpdateLeaveInfor(int id, int appFlag, string reason, int mtype=0,string email="",string phone="",string name="",string cname="")
        {
            try
            {
                Approval.UpdateLeaveInfor(id, appFlag, reason, DateTime.Now, CurrentUser.UserId);
                //发送信息
                var emailList = new List<KeyValuePair<string, string>>();
                var messlist = new List<KeyValuePair<string, string>>();
                var content = string.Format("{0}，您好！ 您对课程《{1}》的请假申请已经获得审批，审批结果为{2}，详情请登录培训系统。", name, cname, appFlag == 1 ? "审批通过" : "审批拒绝");
                if (!string.IsNullOrWhiteSpace(email))
                    emailList.Add(new KeyValuePair<string, string>(email, content));
                if (!string.IsNullOrWhiteSpace(phone))
                    messlist.Add(new KeyValuePair<string, string>(phone, content));
                if (mtype == 1)
                {
                    if (emailList.Count > 0)
                        SendEmail(emailList, "请假审批结果通知");
                }
                else if (mtype == 2)
                {
                    if (messlist.Count > 0)
                        SendMessage(messlist);
                }
                else if (mtype == 3)
                {
                    if (emailList.Count > 0)
                        SendEmail(emailList, "请假审批结果通知");
                    if (messlist.Count > 0)
                        SendMessage(messlist);
                }
                return Json(new {result = 1,message="成功"}, JsonRequestBehavior.AllowGet);
            }catch
            {
                return Json(new { result = 1 ,message="审批失败"}, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取我的请假审批列表
        /// </summary>
        /// <param name="cname">课程名称</param>
        /// <param name="isMust">是否必修</param>
        /// <param name="appFlag">审批标志</param>
        /// <param name="starttime">请假开始时间</param>
        /// <param name="endtime">请假结束时间</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="jsRenderSortField">排序方式</param>
        /// <returns></returns>
        public JsonResult GetMyLeaveApprovalList(string cname = "", int isMust = -1,int appFlag=-1, string starttime = "2000-1-1", string endtime = "2050-1-1", int pageIndex = 1, int pageSize = 20, string jsRenderSortField = " LeaveTime desc ")
        {
            var list = GetLeaveList(cname,isMust,appFlag,starttime,endtime,pageIndex,pageSize,jsRenderSortField);
            return Json(new { result = 1, dataList = list, recordCount = list.Count > 0 ? list[0].Totalcount : 0 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 首页我的请假
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMyLeaveList()
        {
            var list = GetLeaveList("", -1, -1, "", "", 1, 4);
            var newList = new List<object>();
            list.ForEach(p=> newList.Add(new
                                             {
                                                 name=p.CourseName,
                                                 date=p.LeaveTimeShow,
                                                 url=0,
                                                 flag=0,
                                                 status = p.LeaveStatus
                                             }));
            return Json(newList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取请假列表集合
        /// </summary>
        /// <returns></returns>
        protected List<DepLeaveInfor> GetLeaveList(string cname = "", int isMust = -1, int appFlag = -1, string starttime = "2000-1-1", string endtime = "2050-1-1", int pageIndex = 1, int pageSize = 20, string jsRenderSortField = " LeaveTime desc ")
        {
            //获取配置
            //var config = Approval.GetConfig(CurrentUser.DeptId, 7);
            //var hour = config == null ? 999 : Convert.ToDecimal(config.ConfigValue);

            var sqlWhere = string.Format(" and o.UserId={0} ", CurrentUser.UserId);
            if (!string.IsNullOrEmpty(cname.Trim()))
            {
                sqlWhere += string.Format(" and c.CourseName like '%{0}%' ", cname.Trim().ReplaceSql());
            }
            if (isMust >= 0)
            {
                sqlWhere += string.Format(" and c.IsMust={0} ", isMust);
            }
            if (appFlag >= 0)
            {
                switch (appFlag)
                {
                    case 0://未审批
                        sqlWhere += string.Format(" and o.ApprovalFlag={0} and o.ApprovalLimitTime>getdate() ", appFlag);
                        break;
                    case 1://审批通过
                        sqlWhere += string.Format(" and o.ApprovalFlag={0} and  o.ApprovalLimitTime>=o.ApprovalTime ", appFlag);
                        break;
                    case 2://审批拒绝
                        sqlWhere += string.Format(" and (o.ApprovalFlag={0} or (o.ApprovalLimitTime<o.ApprovalTime) or (o.ApprovalFlag=0 and o.ApprovalLimitTime<getdate())) ", appFlag);
                        break;
                }
            }
            sqlWhere += string.Format(" and (o.LeaveTime between '{0}' and '{1}') ", string.IsNullOrEmpty(starttime) ? "2000-1-1" : starttime, string.IsNullOrEmpty(endtime) ? "2050-1-1" : endtime);
            return Approval.GetDepLeaveApprovalList(pageIndex, pageSize, ("o." + jsRenderSortField), sqlWhere);
        }
        #endregion
    }
}
