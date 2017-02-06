using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinBLL.DepTranManage;
using LiXinInterface.DepTranManage;
using LiXinModels.DepAttendce;

namespace LiXinControllers
{
    public partial class DepCourseCourseLearnController
    {
        protected IDepTran_Attendce DTAttendceBl = new DepTran_AttendceBL();
        #region 呈现

        public ActionResult DeptAttdenceAndTime()
        {
            var userId = CurrentUser.UserId;
            ViewBag.userId = userId;
            var depId = CurrentUser.DeptId;
            ViewBag.depId = depId;
            return View();
        }

        #endregion

        #region 方法

        public JsonResult GetAttendceList(int userId, int deptId, string courseName, string startTime,
                                          string endTime, int status, int pageSize = 20, int pageIndex = 1)
        {
            string whereSql = " dco.UserId=" + userId;
            if (!string.IsNullOrEmpty(courseName))
            {
                whereSql += " and dc.CourseName like '%" + courseName + "%' ";
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                whereSql += " and dc.StartTime='" + startTime + "'";
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                whereSql += " and dc.StartTime='" + endTime + "'";
            }
            if (status != -1)
            {
                whereSql += " and da.Status=" + status;
            }

            int totalCount = 0;
            List<Dep_Attendce> list = new List<Dep_Attendce>();
            list = DTAttendceBl.GetOneUserList(out totalCount, CurrentUser.UserId, CurrentUser.DeptId, pageIndex, pageSize, whereSql);
            int n = 0;
            var itemArray = new object[list.Count()];
            foreach (var item in list)
            {
                var temp = new
                {
                    CourseName = item.CourseName,
                    CourseLength = item.CourseLength,
                    IsMust = item.IsMust,
                    StartTime = item.StartTime.ToString("yyyy-MM-dd HH:mm"),
                    EndTime = item.EndTime.ToString("yyyy-MM-dd HH:mm"),
                    RoomName = item.RoomName,
                    IsYearPlan = item.IsYearPlan,
                    total = item.total,
                    Status = item.Status,
                    Realname = item.Realname
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
    }
}
