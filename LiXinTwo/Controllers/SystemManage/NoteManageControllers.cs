using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;
using System.Web.Mvc;
using LiXinModels;
using LiXinCommon;
using LiXinControllers.Filter;
using LiXinModels.SystemManage;
using LiXinModels.User;
using System.IO;
using System.Web;

namespace LiXinControllers
{
    public partial class SystemManageController
    {
        #region 呈现

        public ActionResult EditNote(int noteID)
        {
            ViewBag.noteID = noteID;

            ViewBag.NoteSortList = noteSortBL.GetSys_NoteSortList(" isDelete=0 AND Type=0 ");
            ViewBag.NoteResource = sys_noteresourceBL.GetNoteResourceNote("  NoteId=" + noteID);
            ViewBag.TrainGrade = AllTrainGrade;
            var sqlwhere = string.Format(@"  NoteId={0}", noteID);
            //  var list = noteBL.GetListNote(sqlwhere).FirstOrDefault();

            return View();
        }

        public ActionResult NoteManage()
        {

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noteID"></param>
        /// <param name="flag">0 前端 1 管理</param>
        /// <returns></returns>
        public ActionResult NoteDetail(int noteID, int flag = 0)
        {
            Session["moduleName"] = "Announcement";
            ViewBag.noteID = noteID;
            ViewBag.flag = flag;
            var sqlwhere = string.Format(@"  NoteId={0}", noteID);
            var model = noteBL.GetListNote(sqlwhere).FirstOrDefault();
            if (model != null)
            {
                ViewBag.NoteSort = noteSortBL.GetSys_NoteSortList(" isDelete=0 AND Type=0 AND  Id=" + model.SortID).FirstOrDefault();
                // ViewBag.NoteResource = 
            }
            return View();
        }

        public ActionResult NoteManageDetail(int noteID, int flag = 0)
        {
            ViewBag.noteID = noteID;
            ViewBag.flag = flag;
            var sqlwhere = string.Format(@"  NoteId={0}", noteID);
            var model = noteBL.GetListNote(sqlwhere).FirstOrDefault();
            if (model != null)
            {
                ViewBag.NoteSort = noteSortBL.GetSys_NoteSortList(" isDelete=0 AND Type=0 AND  Id=" + model.SortID).FirstOrDefault();
            }
            return View("NoteDetail");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noteID"></param>
        /// <param name="flag">0 前端 1 管理</param>
        /// <returns></returns>
        public ActionResult FAQDetail(int noteID, int flag = 0)
        {
            ViewBag.noteID = noteID;
            ViewBag.flag = 0;
            var sqlwhere = string.Format(@"  NoteId={0}", noteID);
            var model = noteBL.GetListNote(sqlwhere).FirstOrDefault();
            if (model != null)
            {
                ViewBag.NoteSort = noteSortBL.GetSys_NoteSortList(" isDelete=0 AND Type=1 AND  Id=" + model.SortID).FirstOrDefault();
            }
            noteBL.UpdateNumber(noteID);
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="noteID"></param>
        /// <param name="flag">0 前端 1 管理</param>
        /// <returns></returns>
        public ActionResult FAQDetailShow(int noteID, int flag = 1)
        {
            ViewBag.noteID = noteID;
            ViewBag.flag = 1;
            var sqlwhere = string.Format(@"  NoteId={0}", noteID);
            var model = noteBL.GetListNote(sqlwhere).FirstOrDefault();
            if (model != null)
            {
                ViewBag.NoteSort = noteSortBL.GetSys_NoteSortList(" isDelete=0 AND Type=1 AND  Id=" + model.SortID).FirstOrDefault();
            }
            return View("FAQDetail");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="sortName"></param>
        /// <param name="flag">0:新增 1: 修改</param>
        /// <returns></returns>
        public ActionResult EditNoteSort(int Id = 0, int flag = 0)
        {
            Sys_NoteSort noteSort = new Sys_NoteSort();
            if (Id != 0)
            {
                noteSort = noteSortBL.GetSys_NoteSort(Id);
            }
            if (flag == 1)//编辑  选中的接点
            {
                if (noteSort != null)
                {
                    if (noteSort.ParentID == 0)
                    {
                        ViewBag.ParentName = "所有类别";
                    }
                    else
                    {
                        var temp = noteSortBL.GetSys_NoteSort(noteSort.ParentID);
                        if (temp == null)
                        {
                            ViewBag.ParentName = "所有类别";
                        }
                        else
                        {
                            ViewBag.ParentName = temp.SortName;
                        }
                    }
                }
                return View(noteSort);
            }
            else//新增
            {
                var tempAdd = new Sys_NoteSort();
                tempAdd.ParentID = Id;

                var tempParent = noteSortBL.GetSys_NoteSort(noteSort.Id);
                if (tempParent == null)
                {
                    ViewBag.ParentName = "所有类别";
                }
                else
                {
                    ViewBag.ParentName = tempParent.SortName;
                }
                return View(tempAdd);
            }
        }

        public ActionResult Sys_NoteSortList(int showManage = 0)
        {
            ViewBag.showManage = showManage;
            return View();
        }

        public ActionResult NoteListShow()
        {
            return View();
        }


        //FAQ
        public ActionResult FAQ_EditNote(int noteID)
        {
            ViewBag.noteID = noteID;
            ViewBag.NoteSortList = noteSortBL.GetSys_NoteSortList(" isDelete=0 AND Type=1 ");
            ViewBag.NoteResource = sys_noteresourceBL.GetNoteResourceNote("  NoteId=" + noteID);

            return View();
        }

        public ActionResult FAQ_NoteManage()
        {

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="sortName"></param>
        /// <param name="flag">0:新增 1: 修改</param>
        /// <returns></returns>
        public ActionResult FAQ_EditNoteSort(int Id = 0, int flag = 0)
        {
            Sys_NoteSort noteSort = new Sys_NoteSort();
            if (Id != 0)
            {
                noteSort = noteSortBL.GetSys_NoteSort(Id);
            }
            if (flag == 1)//编辑  选中的接点
            {
                if (noteSort != null)
                {
                    if (noteSort.ParentID == 0)
                    {
                        ViewBag.ParentName = "所有类别";
                    }
                    else
                    {
                        var temp = noteSortBL.GetSys_NoteSort(noteSort.ParentID);
                        if (temp == null)
                        {
                            ViewBag.ParentName = "所有类别";
                        }
                        else
                        {
                            ViewBag.ParentName = temp.SortName;
                        }
                    }
                }
                return View(noteSort);
            }
            else//新增
            {
                var tempAdd = new Sys_NoteSort();
                tempAdd.ParentID = Id;

                var tempParent = noteSortBL.GetSys_NoteSort(noteSort.Id);
                if (tempParent == null)
                {
                    ViewBag.ParentName = "所有类别";
                }
                else
                {
                    ViewBag.ParentName = tempParent.SortName;
                }
                return View(tempAdd);
            }
        }

        public ActionResult FAQ_NoteSortList(int showManage = 0)
        {
            ViewBag.showManage = showManage;
            return View();
        }

        public ActionResult FAQ_NoteListShow()
        {
            return View();
        }

        public ActionResult NoteReport()
        {
            return View();
        }

        public ActionResult NoteReportDetails(int NoteID)
        {
            ViewBag.NoteID = NoteID;
            return View();
        }

        public ActionResult Pop_AddBackImage()
        {
            return View();
        }
        #endregion

        #region 方法


        #region 类别树
        public JsonResult GetAllSystemSortTree(int sortType = 0, int type = 0, int checkBoxFlag = 0, int radioFlag = 0, int arrow = 0)
        {
            List<Sys_NoteSort> Sys_NoteSortList = noteSortBL.GetSys_NoteSortList(" Type=" + sortType); // .GetCo_CourseSystemList();
            var treeStr = new StringBuilder();
            treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview w700\">");
            treeStr.Append("<li id=\"0\" class='pNote'>");
            if (type == 0)
            {
                treeStr.AppendFormat("<a id=\"sys_a_0\" title=\"{0}\" onclick=\"fnChooseNote(0,this);\">{0}{1}</a>", "所有类别", arrow == 1 ? "<i></i>" : "");
            }
            else
            {
                treeStr.AppendFormat("<a id=\"sys_a_0\" title=\"{0}\" onclick=\"fnPopChooseNote(0,this);\">{0}{1}</a>", "所有类别", arrow == 1 ? "<i></i>" : "");
            }
            AddChild(0, Sys_NoteSortList, treeStr, type, checkBoxFlag, radioFlag);
            treeStr.Append("</li>");
            treeStr.AppendLine("</ul>");
            return Json(new
            {
                popTreeHtml = treeStr.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        private void AddChild(int partentID, IEnumerable<Sys_NoteSort> courseSystemList, StringBuilder treeStr, int type, int checkBoxFlag, int radioFlag = 0, int arrow = 0)
        {
            var sublist = courseSystemList.Where(p => p.ParentID == partentID);
            if (sublist.Any())
            {
                treeStr.Append("<ul>");
                foreach (Sys_NoteSort noteSort in sublist)
                {
                    if (radioFlag == 0)
                    {
                        if (checkBoxFlag == 1)
                        {
                            treeStr.AppendFormat(
                                "<li id='{0}' class='pNote'><input name='chbCourseSystem' type='checkbox'  id='chb_{0}'  class='fll' value='{0}'/>",
                                noteSort.Id);
                        }
                        else
                        {
                            treeStr.AppendFormat("<li id='{0}' class='pNote'> ", noteSort.Id);
                        }
                    }
                    else
                    {
                        treeStr.AppendFormat(
                            "<li id='{0}' class='pNote'><input name='chbCourseSystem' type='radio'  id='chb_{0}'  class='fll' value='{0}'/>",
                            noteSort.Id);
                    }

                    if (type == 0)
                    {
                        treeStr.AppendFormat(
                            "<a href='#' id='li_NoteSort_{0}' title='{1}' onclick='fnChooseNote({0})'>{1}{2}</a>",
                            noteSort.Id, noteSort.SortName.HtmlXssEncode(), arrow == 1 ? "<i></i>" : "");
                    }
                    else
                    {
                        treeStr.AppendFormat(
                             "<a href='#' id='li_NoteSort_{0}' title='{1}' onclick='fnPopChooseNote({0})'>{1}{2}</a>",
                            noteSort.Id, noteSort.SortName.HtmlXssEncode(), arrow == 1 ? "<i></i>" : "");
                    }
                    treeStr.AppendLine();
                    AddChild(noteSort.Id, courseSystemList, treeStr, type, checkBoxFlag, radioFlag);
                    treeStr.AppendLine();
                    treeStr.Append("</li>");
                }
                treeStr.Append("</ul>");
            }
        }
        #endregion

        /// <summary>
        /// 获取最新公告
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLastNote()
        {
            string where = string.Format(@" ((OpenGroupFlag=0 AND OpenDepartFlag=0 and (charindex('{1}',sn.TrainGrade)>0))	
or
( OpenGroupFlag=1 AND charindex('{1}',sn.TrainGrade)>0 and (SELECT count(*) FROM dbo.F_SplitIDs(OpenGroup) WHERE ID IN (SELECT GroupId FROM Sys_GroupUser
WHERE UserId={0}))>0) OR (OpenDepartFlag=1  AND charindex('{1}',sn.TrainGrade)>0 AND (SELECT count(*) FROM dbo.F_SplitIDs(OpenDepart) WHERE ID IN (SELECT DeptId FROM Sys_User
WHERE UserId={0}))>0))", CurrentUser.UserId, CurrentUser.TrainGrade);
            var list = noteBL.GetLastNote(where);
            var newlist = new List<object>();
            list.ForEach(p => newlist.Add(new
                                             {
                                                 id = p.NoteId,
                                                 content = p.NoteTitle
                                             }));
            return Json(newlist, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 根据一个父级分类获取下面所有的子分类
        /// </summary>
        public void GetSortByFatherID(List<Sys_NoteSort> list, int sortID, ref List<int> sortlist)
        {
            var newlist = list.Where(p => p.ParentID == sortID);
            foreach (var item in newlist)
            {
                sortlist.Add(item.Id);
                GetSortByFatherID(list, item.Id, ref sortlist);
            }
        }

        /// <summary>
        /// 获取通知公告
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="show">0 列表查询 1 首页更多点击进入</param>
        /// <returns></returns>
        public JsonResult GetNoteList(int type = 0, int show = 0, int SortID = 0,int istopshow=-1, string title = "", string realName = "", int publish = -1, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalCount = 0;
                var where = string.Format(@" AND su.Realname LIKE '%{0}%' AND sn.NoteTitle LIKE '%{1}%'", realName.ReplaceSql(), title.ReplaceSql());
                if (publish != -1)
                {
                    where += " and sn.PublishFlag=" + publish;
                }

                if (SortID != 0)
                {
                    var sortList = new List<int> { SortID };
                    GetSortByFatherID(noteSortBL.GetSys_NoteSortList(), SortID, ref sortList);
                    where += " and  SortID in (" + string.Join(",", sortList) + ")";
                }
                where += " AND sn.type=" + type;
                if (type == 0 && show == 1)
                {
                    where += string.Format(@" and ((OpenGroupFlag=0 AND OpenDepartFlag=0 and (charindex('{1}',sn.TrainGrade)>0))	
or
( OpenGroupFlag=1 AND charindex('{1}',sn.TrainGrade)>0 and (SELECT count(*) FROM dbo.F_SplitIDs(OpenGroup) WHERE ID IN (SELECT GroupId FROM Sys_GroupUser
WHERE UserId={0}))>0) OR (OpenDepartFlag=1  AND charindex('{1}',sn.TrainGrade)>0 AND (SELECT count(*) FROM dbo.F_SplitIDs(OpenDepart) WHERE ID IN (SELECT DeptId FROM Sys_User
WHERE UserId={0}))>0)) ", CurrentUser.UserId, CurrentUser.TrainGrade);
                }

                if (istopshow != -1)
                { 
                    where += " and sn.isTopShow=" + istopshow;
                }
                
                var list = noteBL.GetListNote(out totalCount, pageIndex, pageSize, where);
                int n = 0;
                var itemArray = new object[list.Count()];
                foreach (var item in list)
                {
                    var temp = new
                    {
                        NoteId = item.NoteId,
                        CreateTimeStr = item.CreateTimeStr,
                        NoteContent = item.NoteContent.HtmlXssEncode(),
                        NoteTitle = item.NoteTitle.HtmlXssEncode(),
                        publishflag = item.publishflag,
                        NoHtmlNoteContent = item.NoHtmlNoteContent,
                        publishFlagStr = item.publishFlagStr,
                        publishtimeStr = item.publishtimeStr,
                        Realname = item.Realname,
                        SortID = item.SortID,
                        Type = item.Type,
                        UserId = item.UserId,
                        AdFlag = item.AdFlag,
                        AdFlagStr = item.AdFlag == 1 ? "是" : "否",
                        Num = item.Num,
                        isTopShow = item.isTopShow
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
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Sys_Notes>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 检查重名
        /// </summary>
        /// <param name="noteID"></param>
        /// <param name="NoteTitle"></param>  
        /// <returns></returns>
        public JsonResult checkNoteTitle(int noteID, string NoteTitle, int type = 0)
        {
            try
            {

                var sqlwhere = new StringBuilder();
                sqlwhere.AppendFormat(" NoteTitle='{0}' AND Type={1}", NoteTitle.ReplaceSingleSql(), type);

                if (noteID > 0)
                {
                    sqlwhere.AppendFormat(" and NoteId<>{0}", noteID);
                }

                return Json(!(noteBL.GetListNote(sqlwhere.ToString()).Count > 0), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 添加修改通知公告
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [Filter.SystemLog("添加修改通知公告", LogLevel.Info)]
        public JsonResult submitEditNote(string title, string content, int noteID, int SortID, int AdFlag = 0, int DepTrainFlag = 0,
            int type = 0, string deleteID = "", int OpenGroupFlag = 0, string OpenGroup = "", int OpenDepartFlag = 0, string OpenDepart = "", string ContentDesc = "", string TrainGrade = "", string BackUrlName = "0.jpg")
        {
            try
            {
                var model = new Sys_Notes()
                {
                    NoteTitle = title,
                    NoteContent = content,
                    UserId = CurrentUser.UserId,
                    publishflag = 0,
                    IsDelete = 0,
                    NoteId = noteID,
                    SortID = SortID,
                    Type = type,
                    AdFlag = AdFlag,
                    DepTrainFlag = DepTrainFlag,
                    OpenGroupFlag = OpenGroupFlag,
                    OpenGroup = OpenGroup,
                    OpenDepartFlag = OpenDepartFlag,
                    OpenDepart = OpenDepart,
                    ContentDesc = ContentDesc,
                    TrainGrade = TrainGrade,
                    BackUrlName = BackUrlName
                };

                if (noteID > 0)
                {
                    noteBL.updateNote(model);

                    //删除附件
                    if (deleteID != "")
                    {
                        sys_noteresourceBL.DeleteNoteResource(deleteID);
                    }
                }
                else
                {
                    noteBL.AddNote(model);
                }
                return Json(new
                {
                    result = 1,
                    Id = model.NoteId,
                    content = "添加成功"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "添加失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 添加修改通知公告
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        [ValidateInput(false)]
        [Filter.SystemLog("添加修改通知公告类别", LogLevel.Info)]
        public JsonResult submitEditNoteSort(int ParentID, string sortName, int Id, int type = 0)
        {
            try
            {
                Sys_NoteSort model = new Sys_NoteSort()
                {
                    Id = Id,
                    SortName = sortName,
                    Type = type,
                    ParentID = ParentID
                };

                if (Id > 0)
                {
                    noteSortBL.UpdateSys_NoteSort(model);
                }
                else
                {
                    noteSortBL.AddSys_NoteSort(model);
                }
                return Json(new
                {
                    result = 1,
                    content = "操作成功！"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "操作失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// 检查重名
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="SortName"></param>  
        /// <returns></returns>
        public JsonResult checkNoteSort(int Id, string SortName, int pID = 0, int type = 0)
        {
            try
            {
                return Json(!(noteSortBL.IsExist(SortName, type, Id, pID)), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [Filter.SystemLog("删除通知公告类别", LogLevel.Info)]
        public JsonResult DeleteNoteSort(int Id, int type = 0)
        {
            string strWhereSort = " ParentID=" + Id + " AND Type=" + type;

            if (noteSortBL.GetSys_NoteSortList(strWhereSort).Count > 0)
            {

                return Json(new
                {
                    result = 0,
                    content = "该类别下存在子节点,不能删除！"
                }, JsonRequestBehavior.AllowGet);

            }
            string strWhere = " SortID=" + Id + " AND Type=" + type;
            if (noteBL.GetListNote(strWhere).Count > 0)
            {
                if (type > 0)
                {
                    return Json(new
                    {
                        result = 0,
                        content = "该类别下存在关联的政策，不能删除！"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        result = 0,
                        content = "该类别下存在关联的公告，不能删除！"
                    }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new
            {
                result = 1,
                content = noteSortBL.DeleteSys_NoteSort(Id) ? "删除成功！" : "删除失败"
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="noteID"></param>
        /// <returns></returns>
        [Filter.SystemLog("发布通知公告", LogLevel.Info)]
        public JsonResult PublishNote(int noteID)
        {
            try
            {
                noteBL.PublishNote(noteID);
                return Json(new
              {
                  result = 1,
                  content = "发布成功"
              }, JsonRequestBehavior.AllowGet);
            }

            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "发布失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="noteID"></param>
        /// <returns></returns>
        [Filter.SystemLog("删除通知公告", LogLevel.Info)]
        public JsonResult DeleteNote(string noteID)
        {
            try
            {
                noteBL.DeleteNote(noteID);
                return Json(new
                {
                    result = 1,
                    content = "删除成功"
                }, JsonRequestBehavior.AllowGet);
            }

            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "删除失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }






        /// <summary>
        /// 获取单个的通知公告
        /// </summary>
        /// <param name="noteID"></param>
        /// <returns></returns>
        public JsonResult GetSingeNote(int noteID)
        {
            try
            {
                var sqlwhere = string.Format(@"  NoteId={0}", noteID);
                var list = noteBL.GetListNote(sqlwhere);
                var model = list.Count > 0 ? list[0] : new Sys_Notes();

                var NoteResource = sys_noteresourceBL.GetNoteResourceNote(" NoteId=" + noteID);
                string html = "";
                foreach (var item in NoteResource)
                {
                    html += "<p>";
                    html += "<span title='" + item.FileName + "'>" + item.FileName + "</span>&nbsp;&nbsp;&nbsp;&nbsp;";
                    html += "<a onclick=\"loadPinFile('" + Url.Content(UFCONoteResource + item.RealName) + "','" + item.FileName + "'," + item.Id + ")\"  class='icon idown' title='我要下载'></a>";
                    html += "</p>";
                }
                var Listgroup = new List<Sys_Group>();
                var listdept = new List<Sys_Department>();
                if (model.OpenGroupFlag == 1)//群组
                {
                    int total = 0;
                    string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + model.OpenGroup + "')) ";
                    Listgroup = groupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                }
                if (model.OpenDepartFlag == 1)//组织结构
                {
                    string strWhere = " DepartmentId in (select id from dbo.F_SplitIDs('" + model.OpenDepart + "')) ";
                    listdept = deptBL.GetAllList(strWhere);
                }
                return Json(new
                {
                    result = 1,
                    SortID = model.SortID,
                    content = model.NoteContent,
                    title = model.NoteTitle,
                    date = model.publishtimeStr,
                    AdFlag = model.AdFlag,
                    DepTrainFlag = model.DepTrainFlag,
                    Listgroup = Listgroup,
                    listdept = listdept,
                    dataList = model,
                    //OpenGroupFlag = model.OpenGroupFlag,
                    //OpenDepartFlag = model.OpenDepartFlag,
                    //OpenGroup = model.OpenGroup,
                    //OpenDepart = model.OpenDepart,
                    //ContentDesc = model.ContentDesc,
                    html = html

                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    content = "",
                    id = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNoteSortList(int type = 0)
        {
            List<Sys_NoteSort> list = noteSortBL.GetSys_NoteSortList(" isDelete=0 AND type=" + type);
            int n = 0;
            var itemArray = new object[list.Count()];
            foreach (var item in list)
            {
                var temp = new
                {
                    NoteId = item.Id,
                    ParentID = item.ParentID,
                    SortName = item.SortName.HtmlXssEncode(),
                    Type = item.Type
                };
                itemArray[n] = temp;
                n++;
            }
            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = list.Count()
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 是否首页显示
        /// </summary>
        /// <param name="NoteID"></param>
        /// <param name="AdFlag">0撤销 1显示></param>
        /// <returns></returns>
        public JsonResult UpdateNoteAdFlag(int NoteID, int AdFlag)
        {
            noteBL.UpdateAdFlag(NoteID, AdFlag);
            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public void DownFile(int show, string path, string name, int resID, int NoteID)
        {
            if (System.IO.File.Exists(Server.MapPath(path)))
            {
                if (show == 0)
                {
                    var model = new Sys_MyNoteDowmLoad()
                    {
                        UserID = CurrentUser.UserId,
                        ResourceID = resID,
                        NoteID = NoteID,
                        DownLoadTime = DateTime.Now
                    };
                    noteBL.InsertSys_MyNoteDowmLoad(model);
                }
                var filePath = Server.MapPath(path); //路径 
                //以字符流的形式下载文件
                var fs = new FileStream(filePath, FileMode.Open);
                var bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.ContentType = "application/octet-stream";
                //通知浏览器下载文件而不是打开             
                var downloadfilename = name;

                if (Request.UserAgent.ToLower().IndexOf("msie") > -1)
                {
                    downloadfilename = HttpUtility.UrlPathEncode(downloadfilename);
                }

                if (Request.UserAgent.ToLower().IndexOf("firefox") > -1)
                {
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + downloadfilename + "\"");
                }

                else
                {
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(name, Encoding.UTF8));
                }

                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
                //}
            }
            else
            {
                Response.Write("此附件已不存在");
            }
        }
        #endregion

        /// <summary>
        /// 关联资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Filter.SystemLog("提交 新增培训政策以及QA或者公告的附件 ", LogLevel.Info)]
        public JsonResult AddNoteResource(Sys_NoteResource model)
        {
            try
            {
                return Json(new
                {
                    result = sys_noteresourceBL.AddNoteResource(model) ? "1" : "0"
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = "0"
                }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 新增浏览记录
        /// </summary>
        /// <param name="noteID"></param>
        /// <returns></returns>
        public JsonResult AddLookRecord(int noteID)
        {

            var model = new Sys_MyNoteLook()
            {
                NoteID = noteID,
                UserID = CurrentUser.UserId,
                LookTime = DateTime.Now
            };
            noteBL.InsertSys_MyNoteLook(model);
            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 公告统计
        /// </summary>
        public JsonResult GetNoteReport(string NoteTitle, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " publishtime desc")
        {
            try
            {
                var where = "1=1";
                if (!string.IsNullOrEmpty(NoteTitle))
                {
                    where += " and NoteTitle like '%" + NoteTitle.ReplaceSql() + "%'";
                }
                int totalcount = 0;
                var list = noteBL.GetNoteReport(out totalcount, pageIndex, pageSize, where, jsRenderSortField);
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
                    dataList = new List<Sys_Notes>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 公告统计详情
        /// </summary>
        public JsonResult GetNoteReportDetails(int noteID, string Realname = "", int type = -1, string start = "", string end = "", int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                var where = "1=1";
                if (!string.IsNullOrEmpty(Realname))
                {
                    where += " and Realname like '%" + Realname.ReplaceSql() + "%'";
                }
                if (type != -1)
                {
                    where += " and type=" + type;
                }
                if (!string.IsNullOrEmpty(start))
                {
                    where += " and LookTime>='" + start + "'";
                }
                if (!string.IsNullOrEmpty(end))
                {
                    where += " and LookTime<='" + end + "'";
                }
                int totalcount = 0;
                var list = noteBL.GetNoteReportDetails(out totalcount, noteID, pageIndex, pageSize, where);
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
                    dataList = new List<Sys_Notes>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///  获取所有的背景图片
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public JsonResult GetImageList(int type = 0, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalcount = 0;
                var where = " 1=1";
                if (type == 1)
                {

                }
                var list = noteBL.GetImageList(out totalcount, pageIndex, pageSize);
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
                    dataList = new List<sys_NoteBackImage>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="RealName"></param>
        /// <returns></returns>
        public JsonResult AddNoteBackUrl(string FileName, string RealName)
        {
            try
            {
                var model = new sys_NoteBackImage()
                {
                    FileName = FileName,
                    RealName = RealName,
                    defalutType = 0,
                    CreateTime = DateTime.Now
                };
                noteBL.Insertsys_NoteBackImage(model);
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
        /// 设置背景图片的用途
        /// </summary>
        public JsonResult UpdateImageType(int id, int type)
        {
            try
            {
                noteBL.UpdateImageType(id, type);
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
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeteteImage(int id)
        {
            try
            {
                noteBL.DeteteImage(id);
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
        /// 是否置顶
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UpdateTopShow(int isTop, int NoteID)
        {
            try
            {
                noteBL.UpdateTopShow(isTop, NoteID);
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
    }
}

