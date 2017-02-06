using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using LiXinCommon;
using LiXinInterface.ClassRoom;
using LiXinModels;

namespace LiXinControllers.ClassRoom
{
    public class ClassRoomController : BaseController
    {
        protected IClassRoom RoomBl;

        public ClassRoomController(IClassRoom roomBl)
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
            return
                View(Request.QueryString["id"] == "0"
                         ? new Sys_ClassRoom()
                         : RoomBl.GetRoom(Convert.ToInt32(Request.QueryString["id"])));
        }

        /// <summary>
        /// 教室列表呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassRoomManage()
        {
            return View();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取所有的教室
        /// </summary>
        /// <param name="roomName">关键字</param>
        /// <param name="pageSize">没有显示条数</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="jsRenderSortField">排序方式</param>
        /// <returns></returns>
        public JsonResult GetAllClassRoom(string roomName = "", int pageIndex = 1, int pageSize = 20, string jsRenderSortField = "LastUpdateTime desc")
        {
            int totalCount;
            var roomList = RoomBl.GetRoomListPaging(out totalCount, string.Format(" RoomName like '%{0}%' and IsDelete=0 ", CodeHelper.ReplaceSql(roomName)), (pageIndex - 1) * pageSize + 1, pageSize, string.Format(" order by {0}", jsRenderSortField));

            roomList.ForEach(p=>
                                 {
                                     p.RoomName = p.RoomName.HtmlXssEncode();
                                 });
            return Json(new { result = 1, dataList = roomList, recordCount = totalCount }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存教室
        /// </summary>
        /// <param name="form">表单数据</param>
        /// <param name="id">教室ID</param>
        /// <returns></returns>
        public JsonResult SubmitClassRoom(FormCollection form, int id)
        {
            var room = new Sys_ClassRoom
                {
                    Id = id,
                    Memo = form["roomMemo"],
                    RoomName = form["roomName"].Trim(),
                    RoomNumber = form["roomNumber"].StringToInt32(),
                    RowNumber = form["roomRow"].StringToInt32(),
                    ColumnNumber = form["roomCol"].StringToInt32(),
                    LastUpdateTime=DateTime.Now,
                    UserID=CurrentUser.UserId,
                    IsDelete = 0
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
        /// <param name="id">教室ID</param>
        /// <returns></returns>
        public JsonResult IsExsitRoomName(string roomName, int id = 0)
        {
            return Json(RoomBl.IsExsitRoom(roomName, id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除教室
        /// </summary>
        /// <param name="id">教室ID</param>
        /// <returns></returns>
        public JsonResult DeleteRoom(int id)
        {
            return
                Json(
                    RoomBl.DeleteRoom(id)
                        ? new {result = 1, content = "删除成功"}
                        : new { result = 0, content = "删除失败" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取教室
        /// </summary>
        /// <returns></returns>
        public JsonResult GetKnowledgeKey()
        {
            var newList = new List<object>();
            RoomBl.GetRoomList().ForEach(p => newList.Add(new
                {
                    id = p.Id,
                    name = p.RoomName
                }));
            return Json(newList, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
