using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace LiXinCommon
{
    /// <summary>
    ///     Encoding 的摘要说明
    /// </summary>
    public class BaseEncode
    {
        /// <summary>
        ///     将字符串使用base64算法加密
        /// </summary>
        /// <param name="sourceString">待加密的字符串</param>
        /// <param name="ens">System.Text.Encoding 对象，如创建中文编码集对象：System.Text.Encoding.GetEncoding(54936)</param>
        /// <returns>加码后的文本字符串</returns>
        public static string EncodingForString(string sourceString, Encoding ens)
        {
            try
            {
                return Convert.ToBase64String(ens.GetBytes(sourceString));
            }
            catch
            {
                return sourceString;
            }
        }

        /// <summary>
        ///     将字符串使用base64算法加密
        /// </summary>
        /// <param name="sourceString">待加密的字符串</param>
        /// <returns>加码后的文本字符串</returns>
        public static string EncodingForString(string sourceString)
        {
            try
            {
                return EncodingForString(sourceString, Encoding.Default);
            }
            catch
            {
                return sourceString;
            }
        }

        /// <summary>
        ///     从base64编码的字符串中还原字符串，支持中文
        /// </summary>
        /// <param name="base64String">base64加密后的字符串</param>
        /// <param name="ens">System.Text.Encoding 对象，如创建中文编码集对象：System.Text.Encoding.GetEncoding(54936)</param>
        /// <returns>还原后的文本字符串</returns>
        public static string DecodingForString(string base64String, Encoding ens)
        {
            try
            {
                return ens.GetString(Convert.FromBase64String(base64String));
            }
            catch
            {
                return base64String;
            }
        }

        /// <summary>
        ///     从base64编码的字符串中还原字符串，支持中文
        /// </summary>
        /// <param name="base64String">base64加密后的字符串</param>
        /// <returns>还原后的文本字符串</returns>
        public static string DecodingForString(string base64String)
        {
            try
            {
                return DecodingForString(base64String, Encoding.GetEncoding(54936));
            }
            catch
            {
                return base64String;
            }
        }

        /// <summary>
        ///     对任意类型的文件进行base64加码
        /// </summary>
        /// <param name="fileName">文件的路径和文件名</param>
        /// <returns>对文件进行base64编码后的字符串</returns>
        public static string EncodingForFile(string fileName)
        {
            FileStream fs = File.OpenRead(fileName);
            var br = new BinaryReader(fs);
            /*System.Byte[] b=new System.Byte[fs.Length]; 
            fs.Read(b,0,Convert.ToInt32(fs.Length));*/
            string base64String = Convert.ToBase64String(br.ReadBytes((int) fs.Length));
            br.Close();
            fs.Close();
            return base64String;
        }


        /// <summary>
        ///     把经过base64编码的字符串保存为文件
        /// </summary>
        /// <param name="base64String">经base64加码后的字符串</param>
        /// <param name="fileName">保存文件的路径和文件名</param>
        /// <returns>保存文件是否成功</returns>
        public static bool SaveDecodingToFile(string base64String, string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Create);
            var bw = new BinaryWriter(fs);
            bw.Write(Convert.FromBase64String(base64String));
            bw.Close();
            fs.Close();
            return true;
        }

        /// <summary>
        ///     从网络地址一取得文件并转化为base64编码
        /// </summary>
        /// <param name="url">文件的url地址,一个绝对的url地址</param>
        /// <param name="objWebClient">System.Net.WebClient 对象</param>
        /// <returns></returns>
        public static string EncodingFileFromUrl(string url, WebClient objWebClient)
        {
            return Convert.ToBase64String(objWebClient.DownloadData(url));
        }

        /// <summary>
        ///     从网络地址一取得文件并转化为base64编码
        /// </summary>
        /// <param name="url">文件的url地址,一个绝对的url地址</param>
        /// <returns>将文件转化后的base64字符串</returns>
        public static string EncodingFileFromUrl(string url)
        {
            return EncodingFileFromUrl(url, new WebClient());
        }

        /// <summary>
        ///     MD5加密
        /// </summary>
        /// <param name="convertString">待加密的字符</param>
        /// <returns>返回加密后字符</returns>
        public static string GetMd5Str(string convertString)
        {
            if (string.IsNullOrEmpty(convertString))
            {
                return string.Empty;
            }
            return FormsAuthentication.HashPasswordForStoringInConfigFile(convertString, "md5");
        }


        /// <summary>
        ///     判断是否有中文
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CheckEn(string name)
        {
            if (name.Length < Encoding.Default.GetByteCount(name))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 字符过滤编码函数,在实际编程中，建议过滤后还应该使用参数构造sql语句
        /// </summary>
        /// <param name="value">输入字符串</param>
        /// <returns></returns>
        public static string StringEnCodeStr(string value)
        {
            if (value.Trim().Length == 0)
                return string.Empty;
            value = value.Replace(((char)59).ToString(), "&#59;");//;
            value = value.Replace(((char)32).ToString(), "&#32;");//空格
            value = value.Replace(((char)37).ToString(), "&#37;");//%
            value = value.Replace(((char)39).ToString(), "&#39;");//'
            value = value.Replace(((char)44).ToString(), "&#44;");//,
            value = value.Replace(((char)60).ToString(), "&#60;");//<
            value = value.Replace(((char)62).ToString(), "&#62;");//>
            value = value.Replace(((char)92).ToString(), "&#92;");//\\
            value = value.Replace(((char)94).ToString(), "&#94;");//^
            value = value.Replace(((char)45).ToString() + ((char)45).ToString(), "&#45;&#45;");//--
            value = Regex.Replace(value, "<a href=", "<a target=_blank href=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regex = new Regex("<a href=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            value = regex.Replace(value, "<a target=_blank href=");
            return value;
        }
        /// <summary>
        /// 转换双引号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StrEnCodeStr(string value){
            return value.Replace("\"", "\"");
        }
        

    }
}