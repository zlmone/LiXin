<%@ WebHandler Language="C#" Class="imageManager" %>
/**
 * Created by visual studio2010
 * User: xuheng
 * Date: 12-3-7
 * Time: 下午16:29
 * To change this template use File | Settings | File Templates.
 */

using System;
using System.IO;
using System.Web;
using System.Web.SessionState;

public class imageManager : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string fileurl = "../../upload/uploadimages";

        //TODO:上传文件
        if (context.Session["UserID"] != null)
        {
            fileurl += "/" + context.Session["UserID"];
        }
        else
        {
            fileurl += "/" + "Test";
        }
        string path = context.Server.MapPath(fileurl); //最好使用缩略图地址，否则当网速慢时可能会造成严重的延时
        string[] filetype = {".gif", ".png", ".jpg", ".jpeg", ".bmp"}; //文件允许格式

        string action = context.Server.HtmlEncode(context.Request["action"]);

        if (action == "get")
        {
            String str = String.Empty;
            var info = new DirectoryInfo(path);

            string serverFileUrl = "server/upload/uploadimages/";
            if (context.Session["userID"] != null)
            {
                serverFileUrl += context.Session["userID"] + "/";
            }
            else
            {
                serverFileUrl += "Test" + "/";
            }

            //目录验证
            if (info.Exists)
            {
                foreach (FileInfo fi in info.GetFiles())
                {
                    if (Array.IndexOf(filetype, fi.Extension) != -1)
                    {
                        str += serverFileUrl + fi.Name + "ue_separate_ue";
                    }
                }
            }
            context.Response.Write(str);
        }
    }


    public bool IsReusable
    {
        get { return false; }
    }
}