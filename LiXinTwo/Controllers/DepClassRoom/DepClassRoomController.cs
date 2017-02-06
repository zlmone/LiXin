using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using LiXinCommon;
using LiXinInterface.ClassRoom;
using LiXinModels;
using LiXinModels.User;
using LiXinControllers.Filter;

namespace LiXinControllers.DepClassRoom
{
    public class DepClassRoomController : BaseController
    {
        protected IDepClassRoom RoomBl;

        public DepClassRoomController(IDepClassRoom roomBl)
        {
            RoomBl = roomBl;
        }

        #region 页面呈现

        /// <summary>
        /// 编辑教室页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassRoomEdit()
        {
            return View(Request.QueryString["id"] == "0" ? new Dep_ClassRoom() : RoomBl.GetRoom(Convert.ToInt32(Request.QueryString["id"])));
        }

        /// <summary>
        /// 教室列表呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassRoomManage()
        {
            ViewBag.Departments = GetAllSubDepartments();
            return View();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取所有的教室
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <param name="roomName">关键字</param>
        /// <param name="pageSize">没有显示条数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="jsRenderSortField">排序方式</param>
        /// <returns></returns>
        public JsonResult GetAllClassRoom(int deptId = 0, string roomName = "", int pageIndex = 1, int pageSize = 20, string jsRenderSortField = "LastUpdateTime desc")
        {
            var deptStr = string.Join(",", GetAllSubDepartments().Select(p => p.DepartmentId));
            int totalCount;
            var roomList = RoomBl.GetRoomListPaging(out totalCount,
                                                    string.Format(
                                                        " RoomName like '%{0}%' and IsDelete=0 and DeptId = {1} ",
                                                        roomName.ReplaceSql(), deptId), (pageIndex - 1)*pageSize + 1,
                                                    pageSize, string.Format(" order by {0}", jsRenderSortField));

            roomList.ForEach(p =>
                                 {
                                     p.RoomName = p.RoomName.HtmlXssEncode();
                                 });
            return Json(new {result = 1, dataList = roomList, recordCount = totalCount}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存教室
        /// </summary>
        /// <param name="form">表单数据</param>
        /// <param name="id">教室ID</param>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        [Filter.SystemLog("保存教室", LogLevel.Info)]
        public JsonResult SubmitClassRoom(FormCollection form, int id,int deptId)
        {
            var room = new Dep_ClassRoom
                {
                    Id = id,
                    Memo = form["roomMemo"],
                    RoomName = form["roomName"].Trim(),
                    RoomNumber = form["roomNumber"].StringToInt32(),
                    RowNumber = form["roomRow"].StringToInt32(),
                    ColumnNumber = form["roomCol"].StringToInt32(),
                    LastUpdateTime=DateTime.Now,
                    UserID=CurrentUser.UserId,
                    IsDelete = 0,
                    Address="",
                    DeptId = deptId
                };
            var flag = id == 0 ? RoomBl.AddRoom(room) : RoomBl.EditRoom(room);
            return
                Json(flag
                         ? new { result = 1, content = "成功" }
                         : new { result = 0, content = "添加失败" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断是否已经存在此教室
        /// </summary>
        /// <param name="roomName">教室名称</param>
        /// <param name="deptId">部门ID </param>
        /// <param name="id">教室ID</param>
        /// <returns></returns>
        public JsonResult IsExsitRoomName(string roomName, int deptId, int id = 0)
        {
            return Json(RoomBl.IsExsitRoom(roomName, id, string.Format(" and DeptId = {0} ", deptId)), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除教室
        /// </summary>
        /// <param name="id">教室ID</param>
        /// <returns></returns>
        [Filter.SystemLog("删除教室", LogLevel.Info)]
        public JsonResult DeleteRoom(int id)
        {
            return
                Json(
                    RoomBl.DeleteRoom(id)
                        ? new {result = 1, content = "删除成功"}
                        : new { result = 0, content = "删除失败" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
