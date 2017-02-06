using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.User;
using LiXinLanguage;
using LiXinModels.User;

namespace LiXinControllers
{
    public class UserFingerInforController : BaseController
    {
        protected IUserFingerInfor ufBL;
        public UserFingerInforController(IUserFingerInfor _ufBL)
        {
            ufBL = _ufBL;
        }
        #region 呈现

        public ActionResult FingerInforManage()
        {
            return View();
        }

        public ActionResult EditFingerInfor(int id, string realname)
        {
            ViewBag.id = id;
            ViewBag.realname = realname;
            return View();
        }

        #endregion

        #region 方法
        /// <summary>
        /// 获取计划的基本信息
        /// </summary>
        public JsonResult GetUserFingerList(string realName, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalCount = 0;

                var fingerList = ufBL.GetUserFingerList(out totalCount, pageIndex, pageSize, " Status=0 and Realname like '%" + realName.ReplaceSql() + "%'");

                return Json(new
                {
                    result = 1,
                    dataList = fingerList,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Sys_UserFinger>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 保存指纹
        /// </summary>
        /// <param name="finger"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult SaveFinger()
        {
            try
            {
                var finger1 = Request.Form["finger1"];
                var finger2 = Request.Form["finger2"];
                var id = Request.Form["id"].StringToInt32();
                var userID = Request.Form["userID"].StringToInt32();
                if (id > 0)
                {
                    ufBL.Update(id, finger1,finger2);
                }
                else
                {
                    ufBL.Insert(userID, finger1, finger2);
                }
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
                    content = "更新失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导出人员的指纹信息
        /// </summary>
        public void ExportUserFingerInfor(string realName="")
        {
            var fingerList = ufBL.GetUserFingerInfor(string.Format(" su.Realname like '%{0}%' ",realName.ReplaceSql()));
            var dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("UserID", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("登录名", typeof(string));
            dt.Columns.Add("部门/分所", typeof(string));
            dt.Columns.Add("培训级别", typeof(string));
            dt.Columns.Add("指纹特征码一", typeof(string));
            dt.Columns.Add("指纹特征码二", typeof(string));
            var count = 1;
            foreach (var item in fingerList)
            {
                dt.Rows.Add(count++, item.UserId, item.Realname, item.UserName, item.deptname, string.IsNullOrEmpty(item.TrainGrade) ? "" : item.TrainGrade, string.IsNullOrEmpty(item.FingerTemplate1) ? "" : item.FingerTemplate1, string.IsNullOrEmpty(item.FingerTemplate2) ? "" : item.FingerTemplate2);
            }
            var dtList = new List<DataTable> {dt};
            var export = new Spreadsheet();
            export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, "人员指纹信息");
        }

        #endregion
    }
}
