using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using LiXinBLL.NewClassManage;
using LiXinCommon;
using LiXinInterface.ClassRoom;
using LiXinBLL.ClassRoom;
using LiXinInterface.NewClassManage;
using LiXinModels;
using LiXinControllers.Filter;

namespace LiXinControllers.NewClassRoom
{
    public class NewClassRoomController : BaseController
    {
        protected INewClassRoom RoomBL;
        protected IClassRoomResource RoomResourceBL;
        protected INewClass Newclass;
        protected INewGroupUser NewGroupUser;

        public NewClassRoomController(INewClassRoom roomBl, IClassRoomResource roomResourceBL, INewClass newclass, INewGroupUser newGroupUser)
        {
            RoomBL = roomBl;
            RoomResourceBL = roomResourceBL;
            Newclass = newclass;
            NewGroupUser = newGroupUser;
        }

        #region 页面呈现

        /// <summary>
        /// 获取当前所选班级的人数
        /// </summary>
        /// <param name="cId"></param>
        /// <returns></returns>
        public JsonResult GetClassPersonCount(string cId)
        {
            var personCount = string.IsNullOrEmpty(cId) ? 0 : NewGroupUser.GetList(string.Format(" ClassId in (select id from dbo.F_SplitIDs('{0}'))", cId)).Count;
            return Json(personCount, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 教室详情页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassRoomDetial(int id = 0)
        {
            var cn = new New_ClassRoom();
            var resources = new List<Sys_ClassRoomResource>();
            if (id != 0)
            {
                cn = RoomBL.GetRoom(Convert.ToInt32(Request.QueryString["id"]));
                resources = RoomResourceBL.GetRoomResourceList(string.Format(" WHERE ClassRoomID={0} AND IsDelete=0 ", id));
            }
            ViewBag.ClassRoomResources = resources;
            return View(cn);
        }

        /// <summary>
        /// 编辑教室页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassRoomEdit(int id = 0)
        {
            var cn = new New_ClassRoom();
            var resources = new List<Sys_ClassRoomResource>();
            if (id != 0)
            {
                cn = RoomBL.GetRoom(Convert.ToInt32(Request.QueryString["id"]));
                resources = RoomResourceBL.GetRoomResourceList(string.Format(" WHERE ClassRoomID={0} AND IsDelete=0 ", id));
            }
            //课程已选班级
            ViewBag.ClassList = Newclass.GetClassList(string.Format(" Id in ({0}) ", string.IsNullOrEmpty(cn.Classes) ? "0" : cn.Classes));
            ViewBag.PersonCount = string.IsNullOrEmpty(cn.Classes) ? 0 : NewGroupUser.GetList(string.Format(" ClassId in (select id from dbo.F_SplitIDs('{0}'))", cn.Classes)).Count;
            ViewBag.ClassRoomResources = resources;
            return View(cn);
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
        /// 关联教室资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ContentResult SubmitAddRoomResource(int roomId, string RealName, string resourceName)
        {
            var result = RoomResourceBL.AddRoomResource(new Sys_ClassRoomResource
            {
                ClassRoomID = roomId,
                RealName = RealName,
                Name = resourceName,
                CreateDate = DateTime.Now,
                IsDelete = 0
            }) ? "1" : "0";
            return Content(result);
        }


        /// <summary>
        /// 获取所有的教室
        /// </summary>
        /// <param name="roomName">关键字</param>
        /// <param name="pageSize">没有显示条数</param>
        /// <param name="num">人数范围</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="jsRenderSortField">排序方式</param>
        /// <returns></returns>
        public JsonResult GetAllClassRoom(string num = "", string roomName = "", int pageIndex = 1, int pageSize = 20, string jsRenderSortField = "LastUpdateTime desc", int rtype = -1)
        {
            string tempNumber = "";
            int totalCount;
            switch (num)
            {
                case "-1":
                    break;
                case "0":
                    tempNumber = " and RoomNumber >=0 and RoomNumber<=30 ";
                    break;
                case "1":
                    tempNumber = " and RoomNumber >30 and RoomNumber<=60 ";
                    break;
                case "2":
                    tempNumber = " and RoomNumber >60 and RoomNumber<=90 ";
                    break;
                case "3":
                    tempNumber = " and RoomNumber >90 and RoomNumber<=120 ";
                    break;
                case "4":
                    tempNumber = "and RoomNumber >120";
                    break;
            }
            var sqlwhere = " 1=1 ";
            if (rtype > -1)
            {
                if (rtype == 0)
                    sqlwhere += string.Format(" and SeatType=0 ");
                else if (rtype == 1)
                    sqlwhere += string.Format(" and SeatType=1 ");
                else
                    sqlwhere += string.Format(" and SeatType=2 ");
            }

            var roomList = RoomBL.GetRoomListPaging(out totalCount, string.Format(" {1} and RoomName like '%{0}%' and IsDelete=0 " + tempNumber, CodeHelper.ReplaceSql(roomName), sqlwhere), (pageIndex - 1) * pageSize + 1, pageSize, string.Format(" order by {0}", jsRenderSortField));

            roomList.ForEach(p =>
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
        [Filter.SystemLog("新员工 编辑教室", LogLevel.Info)]
        public JsonResult SubmitClassRoom(FormCollection form, int id, string hidDelResourceIds = "")
        {
            bool flag = false;
            try
            {
                var room = new New_ClassRoom
                {
                    Id = id,
                    Memo = form["roomMemo"],
                    RoomName = form["roomName"].Trim(),
                    Classes = form["Classes"],
                    PrePersonCount = form["PrePersonCount"].StringToInt32(),
                    RoomNumber = form["roomNumber"].StringToInt32(),
                    SeatType = form["SeatType"].StringToInt32(),
                    RowNumber = form["roomRow"].StringToInt32(),
                    ColumnNumber = form["roomCol"].StringToInt32(),
                    LastUpdateTime = DateTime.Now,
                    UserID = CurrentUser.UserId,
                    IsDelete = 0,
                    Address = form["Address"],
                    DisSeat = form["roomSeat"].Trim()
                };

                if (id == 0)
                {
                    flag = RoomBL.AddRoom(room);
                }
                else
                {
                    RoomBL.EditRoom(room);
                    if (!string.IsNullOrEmpty(hidDelResourceIds))
                    {
                        foreach (string rid in hidDelResourceIds.Split(','))
                        {
                            RoomResourceBL.DeleteRoomResourcesByID(rid.StringToInt32());
                        }
                    }
                    flag = true;
                }
                return Json(flag ? new { result = 1, content = "成功", classRoomID = room.Id } : new { result = 0, content = "添加失败", classRoomID = room.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                flag = false;
                throw;
            }
        }

        /// <summary>
        /// 判断是否已经存在此教室
        /// </summary>
        /// <param name="roomName">教室名称</param>
        /// <param name="id">教室ID</param>
        /// <returns></returns>
        public JsonResult IsExsitRoomName(string roomName, int id = 0)
        {
            return Json(RoomBL.IsExsitRoom(roomName, id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除教室
        /// </summary>
        /// <param name="id">教室ID</param>
        /// <returns></returns>
        [Filter.SystemLog("新员工 删除教室", LogLevel.Info)]
        public JsonResult DeleteRoom(int id)
        {
            return
                Json(
                    RoomBL.DeleteRoom(id)
                        ? new { result = 1, content = "删除成功" }
                        : new { result = 0, content = "删除失败" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取教室
        /// </summary>
        /// <returns></returns>
        public JsonResult GetKnowledgeKey()
        {
            var newList = new List<object>();
            RoomBL.GetRoomList().ForEach(p => newList.Add(new
                {
                    id = p.Id,
                    name = p.RoomName
                }));
            return Json(newList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region == 附件下载 ==
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">附件的真实文件名</param>
        /// <returns></returns>
        public JsonResult IsExistFile(string path)
        {
            if (!System.IO.File.Exists(Server.MapPath(NewClassroomResource + path)))
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = 1
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="path">附件的真实文件名</param>
        /// <param name="name">附件名</param>
        public void LoadPrincipleFile(string path, string name)
        {
            if (System.IO.File.Exists(Server.MapPath(NewClassroomResource + path)))
            {
                var filePath = Server.MapPath(NewClassroomResource + path); //路径 
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
                Response.Write("此文件已不存在");
            }
        }
        #endregion
    }
}
