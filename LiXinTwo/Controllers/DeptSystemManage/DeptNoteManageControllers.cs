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

namespace LiXinControllers
{
    public partial class DeptSystemManageController
    {

        #region 呈现

        public ActionResult EditNote(int noteID)
        {
            int departId = Convert.ToInt32(Request.QueryString["departId"]);
            ViewBag.noteID = noteID;
            ViewBag.departId = departId;
            ViewBag.NoteSortList = noteSortBL.GetSys_NoteSortList(" isDelete=0 AND Type=0 AND DeptId=" + departId);
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
            //ViewBag.departId = departId;
            ViewBag.flag = flag;
            var sqlwhere = string.Format(@"  NoteId={0}", noteID);
            var model = noteBL.GetListNote(sqlwhere).FirstOrDefault();
            if (model != null)
            {
                ViewBag.NoteSort = noteSortBL.GetSys_NoteSortList(" isDelete=0 AND Type=0 AND  Id=" + model.SortID).FirstOrDefault();
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
        /// <param name="Id"></param>
        /// <param name="sortName"></param>
        /// <param name="flag">0:新增 1: 修改</param>
        /// <returns></returns>
        public ActionResult EditNoteSort(int Id = 0, int flag = 0)
        {
            int departId = Convert.ToInt32(Request.QueryString["departId"]);
            Sys_NoteSort noteSort = new Sys_NoteSort();
            noteSort.DeptId = departId;
            if (Id != 0)
            {
                noteSort = noteSortBL.GetSys_NoteSort(departId,Id);
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
                        var temp = noteSortBL.GetSys_NoteSort(departId,noteSort.ParentID);
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
                tempAdd.DeptId = departId;
                var tempParent = noteSortBL.GetSys_NoteSort(departId,noteSort.Id);
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
            List<Sys_Department> departMent = new List<Sys_Department>();
            departMent = GetAllSubDepartments("");
            ViewBag.departMent = departMent;
            ViewBag.showManage = showManage;
            return View();
        }

        public ActionResult NoteListShow()
        {
            return View();
        }

        #endregion

        #region 方法


        #region 类别树
        public JsonResult GetAllSystemSortTree(int sortType = 0, int type = 0, int checkBoxFlag = 0, int radioFlag = 0, int arrow = 0)
        {
            int departId = Convert.ToInt32(Request.QueryString["departId"]);
            List<Sys_NoteSort> Sys_NoteSortList = noteSortBL.GetSys_NoteSortList(" Dep_NoteSort.DeptId="+departId+" and Type=" + sortType); // .GetCo_CourseSystemList();
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
            var list = noteBL.GetLastNote();
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
        /// <returns></returns>
        public JsonResult GetNoteList(int type = 0, int SortID = 0, string title = "", string realName = "", int publish = -1, int pageSize = 10, int pageIndex = 1)
        {
            int departId = Convert.ToInt32(Request.QueryString["departId"]);
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
                where += " AND sn.DeptId=" + departId;
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
                        Num=item.Num,
                        DeptId=item.DeptId
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
        public JsonResult checkNoteTitle(int departId, int noteID, string NoteTitle, int type = 0)
        {
            try
            {

                var sqlwhere = new StringBuilder();
                sqlwhere.AppendFormat(" NoteTitle='{0}' AND Type={1} AND DeptId={2}", NoteTitle.ReplaceSingleSql(), type,
                                      departId);

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
        public JsonResult submitEditNote(int departId, string title, string content, int noteID, int SortID,int AdFlag=0, int type = 0,string deleteID="")
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
                    AdFlag=AdFlag,
                    DeptId = departId
                };

                if (noteID > 0)
                {
                    noteBL.updateNote(model);

                    
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
            int departId = Convert.ToInt32(Request.QueryString["departId"]);
            try
            {
                Sys_NoteSort model = new Sys_NoteSort()
                {
                    Id = Id,
                    SortName = sortName,
                    Type = type,
                    ParentID = ParentID,
                    DeptId=departId
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
        public JsonResult checkNoteSort(int departId, int Id, string SortName, int pID = 0, int type = 0)
        {
            //try
            //{
                return Json(!(noteSortBL.IsExist(departId, SortName, type, Id, pID)), JsonRequestBehavior.AllowGet);
            //}
            //catch
            //{
            //    return Json(true, JsonRequestBehavior.AllowGet);
            //}
        }

        [Filter.SystemLog("删除通知公告类别", LogLevel.Info)]
        public JsonResult DeleteNoteSort(int Id, int type = 0)
        {
            int departId = Convert.ToInt32(Request.QueryString["departId"]);
            string strWhereSort = " ParentID=" + Id + " AND Type=" + type + "AND DeptId=" + departId;
            
            if (noteSortBL.GetSys_NoteSortList(strWhereSort).Count > 0)
            {

                return Json(new
                {
                    result = 0,
                    content = "该类别下存在子节点,不能删除！"
                }, JsonRequestBehavior.AllowGet);

            }
            string strWhere = " SortID=" + Id + " AND Type=" + type + "AND DeptId=" + departId;
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
        public JsonResult PublishNote(int noteID,int departId)
        {
            try
            {
                noteBL.PublishNote(noteID,departId);
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
        public JsonResult DeleteNote(string noteID, int departId)
        {
            try
            {
                noteBL.DeleteNote(noteID,departId);
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
                var sqlwhere = string.Format(@"  NoteId={0} ", noteID);
                var list = noteBL.GetListNote(sqlwhere);
                var model = list.Count > 0 ? list[0] : new Sys_Notes();

                //var NoteResource = noteBL.GetNoteResourceNote(" NoteId=" + noteID);
                string html = "";
                //foreach (var item in NoteResource)
                //{
                //    html += "<p>";
                //    html += "<span title='" + item.FileName + "'>" + item.FileName + "</span>&nbsp;&nbsp;&nbsp;&nbsp;";
                //    html += "<a href='" + Url.Content(UFCONoteResource + item.RealName) + "' target='_Blank' class='icon idown' title='我要下载'></a>";
                //    html += "</p>";
                //}

                return Json(new
                {
                    result = 1,
                    SortID = model.SortID,
                    content = model.NoteContent,
                    title = model.NoteTitle,
                    date = model.publishtimeStr,
                    AdFlag = model.AdFlag,
                    DepTrainFlag = model.DepTrainFlag,
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
        #endregion

   
    }
}

