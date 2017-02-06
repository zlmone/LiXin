using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace LiXinCommon
{
    /// <summary>
    ///     Encoding ��ժҪ˵��
    /// </summary>
    public class BaseEncode
    {
        /// <summary>
        ///     ���ַ���ʹ��base64�㷨����
        /// </summary>
        /// <param name="sourceString">�����ܵ��ַ���</param>
        /// <param name="ens">System.Text.Encoding �����紴�����ı��뼯����System.Text.Encoding.GetEncoding(54936)</param>
        /// <returns>�������ı��ַ���</returns>
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
        ///     ���ַ���ʹ��base64�㷨����
        /// </summary>
        /// <param name="sourceString">�����ܵ��ַ���</param>
        /// <returns>�������ı��ַ���</returns>
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
        ///     ��base64������ַ����л�ԭ�ַ�����֧������
        /// </summary>
        /// <param name="base64String">base64���ܺ���ַ���</param>
        /// <param name="ens">System.Text.Encoding �����紴�����ı��뼯����System.Text.Encoding.GetEncoding(54936)</param>
        /// <returns>��ԭ����ı��ַ���</returns>
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
        ///     ��base64������ַ����л�ԭ�ַ�����֧������
        /// </summary>
        /// <param name="base64String">base64���ܺ���ַ���</param>
        /// <returns>��ԭ����ı��ַ���</returns>
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
        ///     ���������͵��ļ�����base64����
        /// </summary>
        /// <param name="fileName">�ļ���·�����ļ���</param>
        /// <returns>���ļ�����base64�������ַ���</returns>
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
        ///     �Ѿ���base64������ַ�������Ϊ�ļ�
        /// </summary>
        /// <param name="base64String">��base64�������ַ���</param>
        /// <param name="fileName">�����ļ���·�����ļ���</param>
        /// <returns>�����ļ��Ƿ�ɹ�</returns>
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
        ///     �������ַһȡ���ļ���ת��Ϊbase64����
        /// </summary>
        /// <param name="url">�ļ���url��ַ,һ�����Ե�url��ַ</param>
        /// <param name="objWebClient">System.Net.WebClient ����</param>
        /// <returns></returns>
        public static string EncodingFileFromUrl(string url, WebClient objWebClient)
        {
            return Convert.ToBase64String(objWebClient.DownloadData(url));
        }

        /// <summary>
        ///     �������ַһȡ���ļ���ת��Ϊbase64����
        /// </summary>
        /// <param name="url">�ļ���url��ַ,һ�����Ե�url��ַ</param>
        /// <returns>���ļ�ת�����base64�ַ���</returns>
        public static string EncodingFileFromUrl(string url)
        {
            return EncodingFileFromUrl(url, new WebClient());
        }

        /// <summary>
        ///     MD5����
        /// </summary>
        /// <param name="convertString">�����ܵ��ַ�</param>
        /// <returns>���ؼ��ܺ��ַ�</returns>
        public static string GetMd5Str(string convertString)
        {
            if (string.IsNullOrEmpty(convertString))
            {
                return string.Empty;
            }
            return FormsAuthentication.HashPasswordForStoringInConfigFile(convertString, "md5");
        }


        /// <summary>
        ///     �ж��Ƿ�������
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
        /// �ַ����˱��뺯��,��ʵ�ʱ���У�������˺�Ӧ��ʹ�ò�������sql���
        /// </summary>
        /// <param name="value">�����ַ���</param>
        /// <returns></returns>
        public static string StringEnCodeStr(string value)
        {
            if (value.Trim().Length == 0)
                return string.Empty;
            value = value.Replace(((char)59).ToString(), "&#59;");//;
            value = value.Replace(((char)32).ToString(), "&#32;");//�ո�
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
        /// ת��˫����
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StrEnCodeStr(string value){
            return value.Replace("\"", "\"");
        }
        

    }
}