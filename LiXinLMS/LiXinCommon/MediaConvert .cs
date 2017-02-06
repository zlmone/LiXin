using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Web;

namespace LiXinCommon
{
    public class MediaConvert
    {
        public string  mediapath
        {
            get;
            set;
        }

        public string  flvpath
        {
            get;
            set;
        }
        public MediaConvert()
        {
        }
        public MediaConvert(string mpath,string fpath)
        {
            mediapath = mpath;
            flvpath = fpath;
        }
        /// <summary>  
        /// 转换软件所在的路径  
        /// </summary>  
        public string FfmpegPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FfmpegPath"]);

        public string FilePath = HttpContext.Current.Server.MapPath("../ClientBin/UploadFile/");

       /// <summary>
        /// 将视频文件转换为Flv格式  
       /// </summary>
       /// <param name="mediapath">源文件</param>
       /// <param name="flvpath">转换后</param>
       /// <returns></returns>
        public void ConvertMedia()
        {
            try
            {
                var aa = FfmpegPath;
                const string argu = " -y -ab 128 -acodec libmp3lame -ar 22050 -qscale 6 -r 15 -s 640x480 ";
                var pc = new Process
                {
                    StartInfo =
                    {
                        FileName = FfmpegPath,
                        Arguments = string.Format("-i {0}{1}{2}", FilePath + mediapath, argu, FilePath + flvpath),
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                };
                pc.Start();
                pc.BeginErrorReadLine();
                bool isOK = pc.WaitForExit(1000 * 100);
                pc.Close();
                pc.Dispose();
               
            }
            catch
            {
                
            }

        }
    }
}
