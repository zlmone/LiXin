using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using LiXinBLL.CourseManage;
using LiXinModels.CourseManage;
using SharpZipLib;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

namespace LiXinVedio.Controllers
{
    public class CourseManageController : Controller
    {
        private readonly Cl_VideoManageBL _videoManageBL = new Cl_VideoManageBL();

        public ContentResult SubmitVideo(HttpPostedFileBase FileData, string videoName, string folder = "~/UploadFiles/UFCOVideoZip/")
        {
            if (null != FileData)
            {
                try
                {
                    //文件后缀名
                    var suffix = FileData.FileName.Substring(FileData.FileName.LastIndexOf(".") + 1).ToLower();
                    var resultName = Guid.NewGuid() + "." + suffix;
                    saveFile(FileData, resultName, folder);
                    //解压操作  然后获得解压后的文件名 保存在数据库中
                    var i = resultName.LastIndexOf(".");
                    UnZip(HttpContext.Server.MapPath(folder + resultName), HttpContext.Server.MapPath(@"~/UploadFiles/UFCOVideo/") + resultName.Substring(0, i) + "/", true);
                    var count = 0;
                    //获取子文件夹
                    var dirname = "/Untitled";
                    var di = new DirectoryInfo(HttpContext.Server.MapPath(@"~/UploadFiles/UFCOVideo/") + resultName.Substring(0, i));
                    var dirarr = di.GetDirectories();//获取子文件夹列表
                    if (dirarr.Length > 0)
                    {
                        dirname = "/" + dirarr[0].Name;
                        count++;
                    }

                    //获取html页面
                    var wenjianname = "Untitled.html";
                    var di1 = new DirectoryInfo(HttpContext.Server.MapPath(@"~/UploadFiles/UFCOVideo/") + resultName.Substring(0, i) + dirname);
                    var wenjlist = di1.GetFiles();
                    foreach (var obj in wenjlist.Where(obj => obj.FullName.IndexOf(".html")>=0))
                    {
                        wenjianname = obj.Name;
                        count++;
                        break;
                    }

                    //获取第二层文件夹名称
                    var di2 = new DirectoryInfo(HttpContext.Server.MapPath(@"~/UploadFiles/UFCOVideo/") + resultName.Substring(0, i) + "/" + dirname);
                    var dirname1 = "/Untitled_media";
                    var dirarr1 = di2.GetDirectories();//获取子文件夹列表
                    if (dirarr1.Length > 0)
                    {
                        dirname1 = "/" + dirarr1[0].Name;
                        count++;
                    }

                    //获取视频名字
                    var ship = "Untitled.wmv";
                    var di3 = new DirectoryInfo(HttpContext.Server.MapPath(@"~/UploadFiles/UFCOVideo/") + resultName.Substring(0, i) + dirname + dirname1);
                    var shiplist = di3.GetFiles();
                    foreach (var obj in shiplist.Where(obj => obj.FullName.IndexOf(".wmv") >= 0))
                    {
                        ship = obj.Name;
                        count++;
                        break;
                    }

                    if (count == 4)
                    {
                        var model = new Cl_VideoManage()
                                        {
                                            LastUpdateTime = DateTime.Now,
                                            Name = videoName.Replace("%3C", "<").Replace("%3E", ">"),
                                            Path = resultName.Substring(0, i) + dirname + dirname1 + "/" + ship,
                                            Size = FileData.ContentLength/1024,
                                            IsDelete = 0
                                        };
                        var aa =
                            ProcessRequest(HttpContext.Server.MapPath(@"~/UploadFiles/UFCOVideo/") +
                                           resultName.Substring(0, i) + dirname + @"/" + wenjianname);
                        var lefttop = getmodelList(aa, "<div class=\"contentWrapper_left\">(.+?)</div>", 1);
                        model.BottomContent = "";
                        model.TopContent = "";
                        model.LeftContent = lefttop.Count != 0 ? lefttop[0].ToString() : "";
                        model.RightContent = "../../UploadFiles/UFCOVideo/" + resultName.Substring(0, i) + dirname +
                                             dirname1 + "/images/photo.gif";
                        _videoManageBL.Add(model);
                        return Content(model.Id.ToString());
                    }
                    return Content("-1");
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
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

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="zipUrl">压缩文件的路径</param>
        /// <param name="documentUrl">解压后文档的路径</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>
        public static void UnZip(string zipUrl, string documentUrl, bool overWrite)
        {
            using (var s = new ZipInputStream(System.IO.File.OpenRead(zipUrl)))
            {
                s.Password = "";
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    var directoryName = "";
                    var pathToZip = "";
                    pathToZip = theEntry.Name;
                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";
                    var fileName = Path.GetFileName(pathToZip);
                    Directory.CreateDirectory(documentUrl + directoryName);

                    if (fileName != "")
                    {
                        if ((System.IO.File.Exists(documentUrl + directoryName + fileName) && overWrite) || (!System.IO.File.Exists(documentUrl + directoryName + fileName)))
                        {
                            
                            using (FileStream streamWriter = System.IO.File.Create(documentUrl + directoryName + fileName))
                            {
                                var data = new byte[2048];
                                while (true)
                                {
                                    var size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }

                s.Close();
            }
        }

        public bool saveFile(HttpPostedFileBase postedFile, string saveName,
                             string filepath = "~/UploadFiles/UFCOVideoZip")
        {
            var result = false;
            var a = HttpContext.Server.MapPath(filepath);
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

        public string ProcessRequest(string path)
        {
            //ExistsFile(Server.MapPath("test/weather.txt"));//检查文件是否存在
            //读取文件
            var rd = new StreamReader(path, System.Text.Encoding.Default);
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
    }
}
