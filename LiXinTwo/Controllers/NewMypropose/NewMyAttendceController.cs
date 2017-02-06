using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinModels.NewCourseManage;


namespace LiXinControllers
{
    public partial class NewMyproposeController : BaseController
    {
        /// <summary>
        /// 我的考勤呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult MyNewAttendceList()
        {
            return View();
        }

        /// <summary>
        /// 获取我的考勤信息
        /// </summary>
        /// <param name="CourseName"></param>
        /// <param name="status"></param>
        /// <param name="Way"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="teachername"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public JsonResult MyAttendceList(string CourseName,int status, int Way, int pageSize = 10, int pageIndex = 1
            , string teachername = "", string starttime = "", string endtime = "", string jsRenderSortField = "ncod.CourseId desc")
        {

            string sql = " 1=1";
            #region 查询条件

            if (!string.IsNullOrEmpty(CourseName))
            {
                sql += " and nc.CourseName like '%" + CourseName.ReplaceSql() + "%'";
            }

            if (Way > -1)
            {
                sql += string.Format(" AND ncod.Type={0}", Way);
            }

            if (!string.IsNullOrEmpty(teachername))
            {
                sql += " and su.Realname like '%" + teachername.ReplaceSql() + "%'";
            }


            if (!string.IsNullOrEmpty(starttime))
            {
                sql += string.Format(" AND ncrr.StartTime>='{0}'", starttime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                sql += string.Format(" AND ncrr.EndTime<='{0}'", Convert.ToDateTime(endtime).AddDays(1).AddSeconds(-1));
            }

            #endregion
            int totalCount = 0;
            int uid = CurrentUser == null ? 0 : CurrentUser.UserId;
            var list = IAttendce.GetMyNewAttendList(uid, out totalCount, 1, int.MaxValue, jsRenderSortField, sql);
            switch (status)
            {
                case 0:
                    list = list.Where(p => p.AttStatusStr == "正常").ToList();
                    break;
                case 1:
                    list = list.Where(p => p.AttStatusStr == "缺勤").ToList();
                    break;
                case 2:
                    list = list.Where(p => p.AttStatusStr == "迟到").ToList();
                    break;
                case 3:
                    list = list.Where(p => p.AttStatusStr == "早退").ToList();
                    break;
                case 4:
                    list = list.Where(p => p.AttStatusStr == "迟到并早退").ToList();
                    break;
                case 5:
                    list = list.Where(p => p.AttStatusStr == "考勤未上传").ToList();
                    break;
            }
            List<New_CourseOrderDetail> list1 = list.Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();
            return Json(new
            {
                result = 1,
                dataList = list1,
                recordCount = list.Count
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
