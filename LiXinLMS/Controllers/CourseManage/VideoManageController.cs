using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using LiXinModels.CourseManage;
using LiXinCommon;
using SharpZipLib;
namespace LiXinControllers
{
    public partial class CourseManageController : BaseController
    {



        public ActionResult EditVideo(int? Id)
        {
            ViewBag.Id = 0;
            if (Id.HasValue)
            {
                int t = 0;
                Cl_VideoManage video = _videoManageBL.GetCl_VideoManageList(out t, "Id=" + Id.Value, 0, 1, "ORDER BY Cl_VideoManage.Id,Cl_VideoManage.LastUpdateTime DESC").FirstOrDefault();
                ViewBag.Id = Id.Value;
                ViewBag.Name = video.Name;
            }
            return View();
        }

        public ContentResult SubmitVideo(HttpPostedFileBase FileData, string folder, string videoName)
        {
            string filename = "";
            string resultName = "";
            if (null != FileData)
            {
                try
                {
                    filename = Path.GetFileName(FileData.FileName); //获得文件名
                    string fullPathname = Path.Combine(folder, filename);
                    //文件后缀名
                    string suffix = FileData.FileName.Substring(FileData.FileName.LastIndexOf(".") + 1).ToLower();
                    resultName = Guid.NewGuid() + "." + suffix;
                    if (saveFile(FileData, folder, resultName))
                    {
                        //解压操作  然后获得解压后的文件名 保存在数据库中
                        int i = resultName.LastIndexOf(".");

                        GzipHelper.UnZip(UFCOVideoADDR, UFCOVideoZIP,
                                                          UFCOVideoUSR, UFCOVideoPwd,
                                                          HttpContext.Server.MapPath(folder + resultName),
                                                          UFCOVideoZIP + @"\" + resultName.Substring(0, i), "", true);
                        Cl_VideoManage model = new Cl_VideoManage()
                                                   {
                                                       LastUpdateTime = DateTime.Now,
                                                       Name = videoName.Replace("%2B","+" ),//.Replace("%3C","<").Replace("%3E",">").Replace("%2F","/"),
                                                       Path =
                                                           resultName.Substring(0, i) +
                                                           @"/Untitled/Untitled_media/Untitled.wmv",
                                                       Size = FileData.ContentLength / 1024,
                                                       IsDelete = 0
                                                   };
                        int status = NetworkConnection.Connect(UFCOVideoADDR, UFCOVideoZIP,
                                                               string.Format(@"{0}", UFCOVideoUSR), UFCOVideoPwd);
                        if (status == (int)ERROR_ID.ERROR_SUCCESS || status == 1202)
                        {
                            string aa =
                                ProcessRequest(UFCOVideoZIP + resultName.Substring(0, i) + @"/Untitled/Untitled.html");

                            var lefttop = getmodelList(aa, "<div id=\"indexlinks\" (.+?)></div>", 1);
                            var leftbot = getmodelList(aa,
                                                       "<table width=\"202\" height=\"200\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-bottom:1px solid #669966\">(.+?)</table>",
                                                       1);
                            model.BottomContent = "";
                            model.TopContent = "";

                            if (lefttop.Count != 0)
                            {
                                model.LeftContent = lefttop[0].ToString();
                            }
                            else
                            {
                                model.LeftContent = "";
                            }
                            if (leftbot.Count != 0)
                            {
                                model.RightContent = leftbot[0].ToString();
                            }
                            else
                            {
                                model.RightContent = "";
                            }
                            _videoManageBL.Add(model);
                        }
                        NetworkConnection.Disconnect(UFCOVideoZIP);
                        return Content(model.Id.ToString() + status.ToString());
                    }
                    else
                    {
                        return Content("-1");
                    }
                }
                catch (Exception ex)
                {
                    return Content(ex.Message+"--------"+ex.Source+"-----------"+ex.StackTrace);
                }
            }
            return Content("-1");
        }

        public ArrayList getmodelList(String fStr, String rStr, int Group)
        {//<!-- start -->(.+?)<!-- finish -->
            try
            {
                ArrayList al = new ArrayList();
                MatchCollection Mc = chkstrs(fStr, rStr);
                foreach (Match m in Mc)
                {
                    al.Add(tostr(m.Groups[Group]));
                }
                return al;
            }
            catch (Exception e)
            {
                return new ArrayList();
            }

        }

        public String tostr(Object x)
        {
            try
            {
                return x.ToString();
            }
            catch (Exception e)
            {
                return "";
            }

        }

        public string ProcessRequest(string path)
        {
            //ExistsFile(Server.MapPath("test/weather.txt"));//检查文件是否存在
            //读取文件
            StreamReader rd = new StreamReader(path, System.Text.Encoding.Default);
            try
            {
                string input = rd.ReadToEnd();
                rd.Close();
                return input;
            }
            catch
            {
                return "";
            }


        }

        public MatchCollection chkstrs(String inStr, String RegStr)
        {
            try
            {
                Regex r = new Regex(RegStr, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                return r.Matches(inStr);
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public String httpreq(String url)
        {
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(url);
            rq.Method = "GET";
            rq.ContentType = "application/x-www-form-urlencoded";
            Stream receiveStream = (rq.GetResponse() as HttpWebResponse).GetResponseStream();
            Encoding respseCode = Encoding.Default;
            try
            {
                respseCode = Encoding.GetEncoding("UTF-8");
            }
            catch
            {
                respseCode = Encoding.Default;
            }
            StreamReader sr = new StreamReader(receiveStream, respseCode);
            return sr.ReadToEnd();
        }

        public JsonResult SubmitModify(int id, int flag = 1, string Name = "")
        {
            int t = 0;
            Cl_VideoManage video = _videoManageBL.GetCl_VideoManageList(out t, "Id=" + id, 0, 1, "ORDER BY Cl_VideoManage.Id,Cl_VideoManage.LastUpdateTime ASC").FirstOrDefault();
            if (flag == 1)
            {
                video.IsDelete = 1;

            }
            else
            {
                video.Name = Name;

            }
            if (_videoManageBL.Modify(video))
            {
                return Json(new
             {
                 result = 1,
                 content = flag == 0 ? "操作成功！" : "删除成功！"
             }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
             {
                 result = 1,
                 content = "操作失败！"
             }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult VideoManageList(int type = 0, int showManage = 1, int showAddButton = 1, int showSureButton = 0, int showCheckbox = 0, string videoIds = "")
        {
            ViewBag.type = type;
            ViewBag.showManage = showManage;
            ViewBag.showAddButton = showAddButton;
            ViewBag.showSureButton = showSureButton;
            ViewBag.showCheckbox = showCheckbox;
            ViewBag.videoIds = videoIds;
            return View();
        }


        public JsonResult GetVideoList(string Name = "", int pageSize = 20, int pageIndex = 1)
        {
            int totalCount = 0;
            string strWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(Name))
            {
                strWhere += " AND Name like '%" + Name.ReplaceSql() + "%' ";
            }
            int totalcount = 0;
            var list = _videoManageBL.GetCl_VideoManageList(out totalcount, strWhere, (pageIndex - 1) * pageSize, pageSize, "ORDER BY Cl_VideoManage.Id DESC");


            int n = 0;
            var itemArray = new object[list.Count()];
            foreach (var item in list)
            {
                var temp = new
                {
                    Id = item.Id,
                    Name = item.Name.HtmlXssEncode(),
                    Path = item.Path
                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalcount
            }, JsonRequestBehavior.AllowGet);
        }


    }
}
