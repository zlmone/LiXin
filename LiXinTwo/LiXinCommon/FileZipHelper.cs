using System;
using System.Collections.Generic;
using System.IO;

namespace LiXinCommon
{
    public class FileZipHelper
    {
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="zipFile"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string PackFiles(string zipFile, string[] files)
        {
            var fs = new List<KeyValuePair<string, string>>();
            foreach (var s in files)
            {
                var index = s.LastIndexOf('\\');
                fs.Add(new KeyValuePair<string, string>(s, s.Substring(index)));

            }
            return PackFiles(zipFile, fs);

        }

        public static Stream PackFileToStream(List<KeyValuePair<string, string>> files)
        {
            try
            {
                var stream = new MemoryStream();
                ICSharpCode.SharpZipLib.Zip.ZipFile zip = ICSharpCode.SharpZipLib.Zip.ZipFile.Create(stream);
                zip.BeginUpdate();
                foreach (var file in files)
                {
                    zip.Add(file.Key,file.Value);
                }
                zip.CommitUpdate();
                zip.Close();
                return stream;
            }
            catch (Exception)
            {
                return null;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFile"></param>
        /// <param name="files">Key:文件全路径，Value:压缩文件里的文件名</param>
        /// <returns></returns>
        public static string PackFiles(string zipFile, List<KeyValuePair<string, string>> files)
        {
            try
            {
                ICSharpCode.SharpZipLib.Zip.ZipFile zip = ICSharpCode.SharpZipLib.Zip.ZipFile.Create(zipFile);
                zip.BeginUpdate();
                foreach (var s in files)
                {
                    zip.Add(s.Key, s.Value);
                }
                zip.CommitUpdate();
                zip.Close();
                return zipFile;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}