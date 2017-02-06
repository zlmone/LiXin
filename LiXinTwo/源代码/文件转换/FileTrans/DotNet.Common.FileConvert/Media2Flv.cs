using System;
using System.Diagnostics;
using System.IO;

namespace DotNet.Common.FileConvert
{
    public class Media2Flv : BaseConfig
    {
        /// <summary>
        /// ffmpeg.exe路径
        /// </summary>
        private static string FfmpegPath
        {
            get
            {
                var path = "";
                if (ConfigSection != null)
                    path = ConfigSection.KeyValues["ffmpegPath"].Value;
                return path == "" ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe") : path;
            }
        }

        /// <summary>
        /// qt-faststart.exe路径
        /// </summary>
        private static string FaststartPath
        {
            get
            {
                var path = "";
                if (ConfigSection != null)
                    path = ConfigSection.KeyValues["faststartPath"].Value;
                return path == "" ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "qt-faststart.exe") : path;
            }
        }

        /// <summary>
        /// 多媒体转换
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <param name="isMP4">是否为MP4，如果为MP4进行faststart转换</param>
        public static bool MediaConvert(string sourcePath, string targetPath, bool isMP4 = false)
        {
            if (!File.Exists(sourcePath))
            {
                throw new IOException(string.Format("文件<{0}>不存在！", sourcePath));
            }

            var tempTargetPath = targetPath;
            if (isMP4)
            {
                tempTargetPath = targetPath + ".mp4";
            }

            bool isOK;
            try
            {
                //命令参数
                var argu = ConfigSection.KeyValues["ffmpegArgu"].Value;
                var pc = new Process
                             {
                                 StartInfo =
                                     {
                                         FileName = FfmpegPath,
                                         Arguments = string.Format("-threads 4 -i {0} {1} {2}", sourcePath, argu, tempTargetPath),
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
                isOK = pc.WaitForExit(TimeOut);
                pc.Close();
                pc.Dispose();

                if (isMP4 && isOK)
                {
                    MP4Faststart(tempTargetPath, targetPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return isOK;
        }

        /// <summary>
        /// 处理MP4,使其边下载边播放
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        public static bool MP4Faststart(string sourcePath, string targetPath)
        {
            if (!File.Exists(sourcePath))
            {
                throw new IOException(string.Format("文件<{0}>不存在！", sourcePath));
            }

            bool isOK;
            try
            {
                //命令参数
                var argu = ConfigSection.KeyValues["faststartArgu"].Value;
                var pc = new Process
                {
                    StartInfo =
                    {
                        FileName = FaststartPath,
                        Arguments = string.Format("{0} {1} {2}", sourcePath, argu, targetPath),
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
                isOK = pc.WaitForExit(TimeOut);
                pc.Close();
                pc.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            return isOK;
        }
    }
}
