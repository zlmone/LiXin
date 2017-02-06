using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LiXinCommon;
using LiXinControllers.Filter;
using LixinModels.ReResourceManage;

namespace LiXinControllers
{
    public partial class ReResourceManageController
    {
        #region == 页面呈现 ==
        #region  == 资源页面 ==
        /// <summary>
        /// 知识管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ReResourceManage()
        {
            return View();
        }
        /// <summary>
        /// 上传资源--添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ResourceUpload()
        {
            var typeID = Convert.ToInt32(Request.QueryString["typeid"] ?? "0");
            var rt = ReResourceTypeBl.GetModel(typeID);
            var typeName = "";
            if (rt != null)
            {
                typeName = rt.TypeName;
            }
            ViewBag.typeID = typeID;
            ViewBag.typeName = typeName;
            ViewBag.userID = CurrentUser.UserId;
            return View();
        }
        /// <summary>
        /// 上传资源--编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateResource(int resourceId=0)
        {
            Re_Resource r = ReResourceBl.GetModel(resourceId);
            Re_ResourceType rt = ReResourceTypeBl.GetModel(r.ResourceTypeID);
            ViewBag.TypeName = rt == null ? "无" : rt.TypeName;
            if (r.OpenFlag == 1)//群组
            {
                int total = 0;
                string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + r.OpenGroupObject + "')) ";
                ViewBag.Groups = SysGroupBl.GetAllList(out total, 1, int.MaxValue, strWhere);
            }
            if (r.OpenFlag == 2)//组织结构
            {
                string strWhere = " DepartmentId in (select id from dbo.F_SplitIDs('" + r.OpenDepartObject + "')) ";
                ViewBag.Departs = DepartmentBl.GetAllList(strWhere);
            }
            if (r.OpenFlag == 3)//群组+组织结构
            {
                int total = 0;
                string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + r.OpenGroupObject + "')) ";
                ViewBag.Groups = SysGroupBl.GetAllList(out total, 1, int.MaxValue, strWhere);
                string strWhereDept = " DepartmentId in (select id from dbo.F_SplitIDs('" + r.OpenDepartObject + "')) ";
                ViewBag.Departs = DepartmentBl.GetAllList(strWhereDept);
            }
            return View(r);
        }
        #endregion

        #region == 知识分类页面 ==
        /// <summary>
        /// 知识分类页面
        /// </summary>
        /// <param name="showManage"></param>
        /// <returns></returns>
        public ActionResult ReResourceTypeManage(int showManage = 0)
        {
            ViewBag.showManage = showManage;
            return View();
        }

        /// <summary>
        /// 新增修改分类页面
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="flag">0:新增 1: 修改</param>
        /// <returns></returns>
        public ActionResult EditNoteSort(int Id = 0, int flag = 0)
        {
            Re_ResourceType noteSort = new Re_ResourceType();
            if (Id != 0)
            {
                noteSort = ReResourceTypeBl.GetModel(Id);
            }
            if (flag == 1) //编辑  选中的接点
            {
                if (noteSort != null)
                {
                    if (noteSort.ParentID == 0)
                    {
                        ViewBag.ParentName = "所有类别";
                    }
                    else
                    {
                        var temp = ReResourceTypeBl.GetModel(noteSort.ParentID);
                        ViewBag.ParentName = temp == null ? "所有类别" : temp.TypeName;
                    }
                }
                return View(noteSort);
            }
            //新增
            var tempAdd = new Re_ResourceType();
            tempAdd.ParentID = Id;
            var tempParent = ReResourceTypeBl.GetModel(noteSort.ResourceTypeID);
            ViewBag.ParentName = tempParent == null ? "所有类别" : tempParent.TypeName;
            return View(tempAdd);
        }
        #endregion

        #endregion

        #region == 方法 ==

        #region == 知识分类方法 ==
        /// <summary>
        /// 检查重名
        /// </summary>
        /// <param name="sortName">类别名称</param>
        /// <param name="id">分类ID</param>
        /// <param name="parentId">分类父级ID</param>
        /// <returns></returns>
        public JsonResult CheckNoteSort(int id, string sortName, int parentId = 0)
        {
            try
            {
                return Json(!(ReResourceTypeBl.IsExist(sortName, id, parentId)), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 添加修改知识分类
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [Filter.SystemLog("添加修改知识类别", LogLevel.Info)]
        public JsonResult submitEditNoteSort(Re_ResourceType model)
        {
            try
            {
                model.IsDelete = 0;
                if (model.ResourceTypeID> 0)
                {
                    ReResourceTypeBl.UpdateModel(model);
                }
                else
                {
                    ReResourceTypeBl.AddModel(model);
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
        /// 删除知识类别
        /// </summary>
        /// <param name="typeId">类别ID</param>
        /// <returns></returns>
        [Filter.SystemLog("删除知识类别", LogLevel.Info)]
        public JsonResult DeleteNoteSort(int typeId)
        {
            if (ReResourceTypeBl.IsExistsChildNodeOrResource(typeId))
            {
                return Json(new
                {
                    result = 0,
                    content = "该类别下存在子节点或关联的资源，不能删除！"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = 1,
                content = ReResourceTypeBl.DeleteModel(typeId) ? "删除成功！" : "删除失败！"
            }, JsonRequestBehavior.AllowGet);
        }

        #region ==  获取类别树  ==
        public JsonResult GetAllResourceTypeTree( int type = 0, int checkBoxFlag = 0, int radioFlag = 0, int arrow = 0)
        {
            List<Re_ResourceType> resourceTypeList = ReResourceTypeBl.GetResourceTypeList(); 
            var treeStr = new StringBuilder();
            treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview w700\">");
            treeStr.Append("<li id=\"0\" class='pNote'>");
            treeStr.AppendFormat("<a id=\"sys_a_0\" title=\"{0}\" onclick=\"fnChooseNote(0,this);\">{0}{1}</a>", "所有类别", arrow == 1 ? "<i></i>" : "");
            AddChild(0, resourceTypeList, treeStr, checkBoxFlag, radioFlag, arrow);
            treeStr.Append("</li>");
            treeStr.AppendLine("</ul>");
            return Json(new
            {
                popTreeHtml = treeStr.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        private void AddChild(int partentID, IEnumerable<Re_ResourceType> resourceTypeList, StringBuilder treeStr,  int checkBoxFlag, int radioFlag = 0, int arrow = 0)
        {
            var sublist = resourceTypeList.Where(p => p.ParentID == partentID);
            if (sublist.Any())
            {
                treeStr.Append("<ul>");
                foreach (Re_ResourceType noteSort in sublist)
                {
                    if (radioFlag == 0)
                    {
                        if (checkBoxFlag == 1)
                        {
                            treeStr.AppendFormat(
                                "<li id='{0}' class='pNote'><input name='chbCourseSystem' type='checkbox'  id='chb_{0}'  class='fll' value='{0}'/>",
                                noteSort.ResourceTypeID);
                        }
                        else
                        {
                            treeStr.AppendFormat("<li id='{0}' class='pNote'> ", noteSort.ResourceTypeID);
                        }
                    }
                    else
                    {
                        treeStr.AppendFormat(
                            "<li id='{0}' class='pNote'><input name='chbCourseSystem' type='radio'  id='chb_{0}'  class='fll' value='{0}'/>",
                            noteSort.ResourceTypeID);
                    }
                    treeStr.AppendFormat(
                            "<a href='#' id='li_NoteSort_{0}' title='{1}' onclick='fnChooseNote({0},{3})'>{1}{2}</a>",
                            noteSort.ResourceTypeID, noteSort.TypeName.HtmlXssEncode(), arrow == 1 ? "<i></i>" : "",noteSort.ParentID);
                   
                    
                    treeStr.AppendLine();
                    AddChild(noteSort.ResourceTypeID, resourceTypeList, treeStr,  checkBoxFlag, radioFlag);
                    treeStr.AppendLine();
                    treeStr.Append("</li>");
                }
                treeStr.Append("</ul>");
            }
        }
        #endregion
       
        #endregion

        #region == 资源方法 ==
        /// <summary>
        /// 获取知识资源列表
        /// <param name="resourceName">资源名称</param>
        /// <param name="startTime">更新时间-开始</param>
        /// <param name="endTime">更新时间-结束</param>
        /// <param name="resourceTypeId">资源分类</param>
        /// </summary>
        [ValidateInput(false)]
        public JsonResult GetResourceList(string resourceName, string startTime, string endTime,int resourceTypeId=0,
                int pageSize = int.MaxValue, int pageIndex = 1, string jsRenderSortField = " LastTime desc ")
        {
            try
            {
                string where = "";
                if(resourceTypeId!=0)
                {
                    where += string.Format(@" and  rr.ResourceTypeID in (SELECT rrt.ResourceTypeID FROM GetChildResourceTypeIds({0}) rrt) ", resourceTypeId);
                }
                if (!string.IsNullOrWhiteSpace(resourceName))
                {
                    where += string.Format(" and rr.ResourceName like '%{0}%' ", resourceName.Trim().ReplaceSql());
                }
                if (!string.IsNullOrWhiteSpace(startTime))
                {
                    where += string.Format(" and rr.LastTime>='{0}' ", startTime);
                }
                if (!string.IsNullOrWhiteSpace(endTime))
                {
                    where += string.Format(" and rr.LastTime<='{0}' ", Convert.ToDateTime(endTime).AddDays(1).AddSeconds(-1));
                }
                int totalCount = 0;
                List<Re_Resource> list = ReResourceBl.GetResourceList(out totalCount, where, pageIndex, pageSize, " order by " + jsRenderSortField);
                return Json(new
                {
                    dataList = list,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<Re_Resource>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 修改资源
        /// </summary>
        /// <param name="newResource">资源Model</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult UpdateResourceInfo(Re_Resource newResource)
        {
            Re_Resource r = ReResourceBl.GetModel(newResource.ResourceID);

            if (ReResourceBl.Exists(newResource.ResourceName.Trim(), newResource.Suffix, newResource.ResourceID))
            {
                return Json(new
                                {
                                    result = 0,
                                    content = "资源名已存在！"
                                }, JsonRequestBehavior.AllowGet);
            }

            r.ResourceName = newResource.ResourceName;
            r.ResourceTypeID = newResource.ResourceTypeID;
            r.LastTime = DateTime.Now;
            if (newResource.ThumbnailURL != "")
            {
                r.ThumbnailURL = newResource.ThumbnailURL;
            }
            r.ResourceDesc = newResource.ResourceDesc;
            r.OpenFlag = newResource.OpenFlag;
            r.OpenGroupObject = newResource.OpenGroupObject;
            r.OpenDepartObject = newResource.OpenDepartObject;
            if (ReResourceBl.UpdateModel(r))
                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            return Json(new
            {
                result = 0,
                content = "修改失败！"
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///根据资源ID删除
        /// </summary>
        /// <param name="resourIds">资源ID</param>
        /// <returns></returns>
        public JsonResult DeleteResource(string resourIds)
        {
            bool isDeleted = ReResourceBl.DeleteBatchModel(resourIds);
            return isDeleted
                       ? Json(new
                       {
                           result = 1,
                           content = "删除成功！"
                       }, JsonRequestBehavior.AllowGet)
                       : Json(new
                       {
                           result = 0,
                           content = "删除失败！"
                       }, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region == 公用方法 ==
        #region == 递归资源树类别 ==
        /// <summary>
        /// 资源类别树
        /// </summary>
        /// <returns></returns>
        public ActionResult ResourceTypeTreeView()
        {
            Response.Expires = 0;
            ViewBag.html = GetTreeView();
            return View();
        }


        public MvcHtmlString GetTreeView()
        {
            var html = new StringBuilder();
            var rtList = ReResourceTypeBl.GetResourceTypeList();
            html.AppendLine("<ul id=\"navigation\" class=\"treeview w700 \">" );
            html.Append("<li>");
            html.AppendFormat("<a  style=\"cursor: default;\">所有类别</a>");
            GetResourceTypeTreeView(0, rtList, html);
            html.Append("</li>");
            html.AppendLine("</ul>");

            return new MvcHtmlString(html.ToString());
        }

        /// <summary>
        /// 递归获取资源类别树
        /// </summary>
        /// <param name="rtList"></param>
        /// <param name="html"></param>
        private static void GetResourceTypeTreeView(int parentID, IEnumerable<Re_ResourceType> rtList, StringBuilder html)
        {
            var tempList = rtList.Where(p => p.ParentID == parentID);
            if (tempList.Any())
            {
                html.Append("<ul>");
                foreach (var v in tempList)
                {
                    html.AppendFormat("<li><a class='RTNode tal' id='{0}' title='{1}' onclick='RTNodeClick(this)' >{1}</a>", v.ResourceTypeID + "_" + v.ParentID , v.TypeName);

                    GetResourceTypeTreeView(v.ResourceTypeID, rtList, html);

                    html.Append("</li>");
                }
                html.Append("</ul>");
            }
        }
        #endregion

        #region == 文件上传 ==
        /// <summary>
        /// 判断资源是否存在
        /// </summary>
        /// <returns></returns>
        public JsonResult isExistsORisnull()
        {

            string rName = Request.QueryString["name"];
            string suffix = Request.QueryString["suffix"];
            int filesize = Convert.ToInt32(Request.QueryString["size"]);
            switch (suffix)
            {
                case "txt":
                    if (filesize < 1)
                    {
                        return Json(new
                        {
                            result = 1,
                            content = rName + "." + suffix + "文件为空！"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    break;
                case "doc":
                    if (filesize < 11546)
                    {
                        return Json(new
                        {
                            result = 1,
                            content = rName + "." + suffix + "文件为空！"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    break;
                case "docx":
                    if (filesize < 11546)
                    {
                        return Json(new
                        {
                            result = 1,
                            content = rName + "." + suffix + "文件为空！"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    break;
                case "xls":
                    if (filesize < 9892)
                    {
                        return Json(new
                        {
                            result = 1,
                            content = rName + "." + suffix + "文件为空！"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    break;
                case "xlsx":
                    if (filesize < 9892)
                    {
                        return Json(new
                        {
                            result = 1,
                            content = rName + "." + suffix + "文件为空！"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    break;
                case "ppt":
                    if (filesize < 31183)
                    {
                        return Json(new
                        {
                            result = 1,
                            content = rName + "." + suffix + "文件为空！"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    break;
                case "pptx":
                    if (filesize < 31156)
                    {
                        return Json(new
                        {
                            result = 1,
                            content = rName + "." + suffix + "文件为空！"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    break;

            }
            if (rName.Length > 30)
            {
                return Json(new
                {
                    result = 1,
                    content = "资源名称长度不能大于30个字符！"
                }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(rName))
            {
                return Json(new
                {
                    result = 1,
                    content = "请输入资源名称！"
                }, JsonRequestBehavior.AllowGet);
            }
            
            bool flag = ReResourceBl.Exists(rName, suffix);
            if (flag)
            {
                return Json(new
                {
                    result = 1,
                    content = rName + "." + suffix + "已存在！"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = 0,
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 文件上传调用的方法
        /// </summary>
        /// <param name="FileData"></param>
        /// <param name="folder"></param>
        /// <param name="detailFlag">detailFlag : 0 :返回上传后的文件名 1: 返回 "原始文件名,Guid文件名,文件大小"</param>
        /// <returns></returns>
        public ContentResult UploadResourceFileAction(HttpPostedFileBase FileData, string folder,
                            int OpenFlag,string OpenGroupObject,string OpenDepartObject, int typeID=0, string Rdec="", 
                            int userID=0, string ImageName="", int detailFlag = 0)
        {
            string filename = "";
            string resultName = "";
            if (null != FileData)
            {
                try
                {
                    string uploadFile = Server.MapPath(ReResourceOldFile) ;
                    string convertFile =Server.MapPath(ReResourceConvertFile);
                    if (!Directory.Exists(uploadFile))
                    {
                        Directory.CreateDirectory(uploadFile);
                    }
                    if (!Directory.Exists(convertFile))
                    {
                        Directory.CreateDirectory(convertFile);
                    }
                    //filename = Path.GetFileName(FileData.FileName); //获得文件名
                    //string fullPathname = Path.Combine(folder, filename);
                    //文件后缀名
                    string suffix = FileData.FileName.Substring(FileData.FileName.LastIndexOf(".") + 1).ToLower();
                    resultName = Guid.NewGuid() + "." + suffix;
                    //为实体类的字段赋值
                    var myResource = new Re_Resource
                    {
                        ResourceName = FileData.FileName.Substring(0, FileData.FileName.LastIndexOf(".")),
                        ResourceDesc = Rdec,
                        ResourceTypeID =typeID,
                        Suffix = suffix,
                        Tag = "",
                        URL = resultName,
                        isOpen = 1,
                        isDownload = 1,
                        UpTime = DateTime.Now,
                        UpUserID = userID,
                        LastTime = DateTime.Now,
                        OpenNum = 0,
                        DownNum = 0,
                        CollectionNum = 0,
                        Score = 0,
                        ScoreNum = 0,
                        Satisfaction = 0,
                        SatisfactionNum = 0,
                        Practical = 0,
                        PracticalNum = 0,
                        Attention = 0,
                        AttentionNum = 0,
                        IsRecommend = 1,
                        Status = 0,
                        OpenFlag = OpenFlag,
                        OpenGroupObject = OpenGroupObject,
                        OpenDepartObject = OpenDepartObject
                    };
                    var flv = new List<string> { "avi", "rmvb", "wmv" };
                    var swf = new List<string> { "doc", "docx", "ppt", "pptx", "xls", "xlsx", "pdf", "txt" };

                    //判断文件后缀名
                    if (flv.Contains(suffix))
                    {
                        myResource.ResultURL =  resultName + ".flv";
                        myResource.ThumbnailURL = "";
                        if (ImageName != "empty")
                        {
                            myResource.ThumbnailURL = ImageName;
                        }
                        myResource.Format = 1;
                    }
                    else if (swf.Contains(suffix))
                    {
                        myResource.ResultURL = resultName + ".swf";
                        myResource.ThumbnailURL = ""; 
                        if (ImageName != "empty")
                        {
                            myResource.ThumbnailURL = ImageName;
                        }
                        myResource.Format = 0;
                    }
                    else
                    {
                        resultName = "请选择指定类型的资源！";
                        return Content(resultName);
                    }

                    //文件另存为
                    saveFile(FileData, folder, resultName);
                    //更新数据库
                    var isUpload = ReResourceBl.AddModel(myResource);
                    //加入消息队列
                    RabbitSet(myResource.URL);
                    //MSMQManager.SendMessage(new FileMSMQ
                    //{
                    //    fileName = resultName,
                    //    oldName = FileData.FileName
                    //});
                    
                }
                catch (Exception ex)
                {
                    resultName = ex.ToString();
                }
            }
            if (detailFlag == 1)
            {
                resultName = FileData.FileName + "|" + resultName + "|" + FileData.ContentLength / 1024;
            }
            return Content(resultName);
        }
        //保存文件
        public bool saveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
        {
            bool result = false;
            string a = HttpContext.Server.MapPath(filepath);
            if (!Directory.Exists(a))
            {
                Directory.CreateDirectory(a);
            }
            try
            {
                postedFile.SaveAs((a + "\\" + saveName));
                result = true;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            return result;
        }
        #endregion

        #region == RabbitMQ配置 ==
        /// <summary>
        /// 文件转换配置
        /// </summary>
        /// <param name="filename"></param>
        public void RabbitSet(string filename)
        {
            #region 文件转换配置
            string fileHead = ConfigurationManager.AppSettings["ReResourceOldFile"];
            string fileHeadConvert = ConfigurationManager.AppSettings["ReResourceConvertFile"];

            var model = new RabbitMQModel.FileModel
            {
                SourcePath = Server.MapPath(fileHead + filename),
                TargetPath = Server.MapPath(fileHeadConvert + filename + GetNewExtName(filename)),
            };
            //RabbitMQClient.RabbitMQHelper.Instance.SendMessage(model);
            new RabbitMQClient.RabbitMQHelper(Resource_serverAddress, Resource_userName, Resource_password, Resource_virtualHost,
                               Resource_queue).SendMessage(model);

            #endregion
        }
        /// <summary>
        /// 获得转换文件的播放类型
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string GetNewExtName(string filename)
        {
            string newName = ".swf";
            if (!string.IsNullOrEmpty(filename))
            {
                string fileExt = System.IO.Path.GetExtension(filename);
                if (fileExt != null && (fileExt.Contains("rmvb") || fileExt.Contains("avi") || fileExt.Contains("wmv")))
                {
                    newName = ".flv";
                }
            }
            return newName;
        }
        #endregion
        #endregion
        #endregion


    }

    
}

