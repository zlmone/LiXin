using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DotNet.Common.FileConvert
{
    public class Pdf2Swf : BaseConfig
    {
        /// <summary>
        /// pdf2swf.exe路径
        /// </summary>
        private static string Pdf2SwfPath
        {
            get
            {
                var path = "";
                if (ConfigSection != null)
                    path = ConfigSection.KeyValues["pdf2swfPath"].Value;
                return path == "" ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdf2swf.exe") : path;
            }
        }

        /// <summary>
        /// PDF转化为SWF
        /// </summary>
        /// <param name="sourcePath">源路径</param>
        /// <param name="targetPath">目标路径</param>
        public static bool PdfToSwf(string sourcePath, string targetPath)
        {
            var bigargu = "";
            if (!File.Exists(sourcePath))
            {
                throw new IOException(string.Format("文件<{0}>不存在！", sourcePath));
            }
            if (sourcePath.ToLower().Contains(".pptx"))
            {
                bigargu = "-s poly2bitmap";
            }

            bool isOK;
            try
            {
                //命令参数
                var argu = ConfigSection.KeyValues["pdf2swfArgu"].Value;
                var endargu=ConfigSection.KeyValues["faststartArgu"].Value;
                var pc = new Process
                {
                    StartInfo =
                    {
                        FileName = Pdf2SwfPath,
                        Arguments = string.Format("-t {0} -s  {1} -o {2} {3} {4}", sourcePath, argu, targetPath, endargu, bigargu),
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardInput = false,
                        RedirectStandardOutput = false,
                        RedirectStandardError = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                };
                pc.Start();
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
