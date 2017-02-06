using System;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using LiXinLanguage;
using System.Security.Cryptography;

namespace LiXinCommon
{
    public static class CodeHelper
    {
        /// <summary>
        ///     获取字符串对应的日期
        /// </summary>
        /// <param name="strTime"></param>
        /// <returns></returns>
        public static DateTime? GetDateTime(this string strTime)
        {
            if (string.IsNullOrEmpty(strTime))
                return null;

            DateTime result;

            if (DateTime.TryParse(strTime, out result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        ///     根据字符串获取Int32
        ///     <para>如果转换失败，返回 Int.MinValue </para>
        /// </summary>
        /// <param name="strInt"></param>
        /// <returns></returns>
        public static Int32 GetInt32(this string strInt)
        {
            if (string.IsNullOrEmpty(strInt))
                return int.MinValue;
            int result;
            return Int32.TryParse(strInt, out result) ? result : int.MinValue;
        }


        /// <summary>
        ///     根据字符串获取Double
        ///     <para>如果转换失败，返回 Double.MinValue </para>
        /// </summary>
        /// <param name="strInt"></param>
        /// <returns></returns>
        public static Double GetDouble(this string strDouble)
        {
            if (string.IsNullOrEmpty(strDouble))
                return double.MinValue;
            double result;
            return Double.TryParse(strDouble, out result) ? result : double.MinValue;
        }

        /// <summary>
        ///     判断字符串是否是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsInt32(this string str)
        {
            int result = 0;
            return int.TryParse(str, out result);
        }

        /// <summary>
        ///     判断字符串是否日期格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDate(this string str)
        {
            DateTime result;
            return DateTime.TryParse(str, out result);
        }

        /// <summary>
        ///     生成随机验证码，返回图片格式
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Bitmap GetValidateCodeMap(out string code, int width = 60, int height = 18)
        {
            var basemap = new Bitmap(width, height);
            Graphics graph = Graphics.FromImage(basemap);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);
            var font = new Font(FontFamily.GenericSerif, height - 4, FontStyle.Bold, GraphicsUnit.Pixel);

            var random = new Random();
            char[] letters = "ABCDEFGHIJKLMNPQRSTUVWXYZ".ToCharArray();

            code = string.Empty;
            var colors = new[] { Color.Red, Color.Black, Color.Blue, Color.Green, Color.GreenYellow, Color.DarkRed };

            //添加随机的五个字母            
            for (int x = 0; x < 5; x++)
            {
                string letter = letters[random.Next(0, letters.Length - 1)].ToString(CultureInfo.InvariantCulture);
                code += letter;
                graph.DrawString(letter, font, new SolidBrush(colors[random.Next(0, 4)]), x * (height - 6),
                                 random.Next(0, 4));
            }
            //混淆背景            
            var linePen = new Pen(new SolidBrush(colors[random.Next(0, 4)]), 1);
            for (int x = 0; x < 2; x++)
            {
                graph.DrawLine(linePen,
                               new Point(random.Next(0, width - 1),
                                         random.Next(0, 17)),
                               new Point(random.Next(0, width - 1),
                                         random.Next(0, 17)));
            }
            return basemap;
        }

        /// <summary>
        ///     获取中括号里的内容
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetContentString(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (str.StartsWith("[") && str.EndsWith("]"))
            {
                return str.Remove(str.Length - 1).Remove(0, 1);
            }
            else
                return str;
        }

        public static string GetNavigateString(string filedName)
        {
            var temp = new ResourceManager("LiXinLanguage.NavigateMenuLanguage", typeof(NavigateMenuLanguage).Assembly);
            return temp.GetString(filedName);
        }

        public static string GetNavigateString(string filedName, CultureInfo cf)
        {
            var temp = new ResourceManager("LiXinLanguage.NavigateMenuLanguage", typeof(NavigateMenuLanguage).Assembly);
            return temp.GetString(filedName, cf);
        }

        /// <summary>
        ///     Base64加密
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string Base64Code(this string message)
        {
            byte[] bytes = Encoding.Default.GetBytes(message);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        ///     Base64解密
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string Base64Decode(this string message)
        {
            byte[] bytes = Convert.FromBase64String(message);
            return Encoding.Default.GetString(bytes);
        }

        /// <summary>
        ///     取得客户端真实IP。如果有代理则取第一个非内网地址
        /// </summary>
        public static string GetClientIp(HttpRequest request)
        {
            string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(result))
            {
                //可能有代理 
                if (result.IndexOf(".", StringComparison.Ordinal) == -1) //没有“.”肯定是非IPv4格式 
                    result = null;
                else
                {
                    if (result.IndexOf(",", StringComparison.Ordinal) != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。 
                        result = result.Replace(" ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        foreach (string t in temparyip)
                        {
                            if (IsIPAddress(t)
                                && t.Substring(0, 3) != "10."
                                && t.Substring(0, 7) != "192.168"
                                && t.Substring(0, 7) != "172.16.")
                            {
                                return t; //找到不是内网的地址 
                            }
                        }
                    }
                    else if (IsIPAddress(result)) //代理即是IP格式 
                        return result;
                    else
                        result = null; //代理中的内容 非IP，取IP 
                }
            }

            if (string.IsNullOrEmpty(result))
                result = request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(result))
                result = request.UserHostAddress;

            return result;
        }

        /// <summary>
        ///     取得客户端真实IP。如果有代理则取第一个非内网地址
        /// </summary>
        public static string GetClientIp(HttpRequestBase request)
        {
            string result = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(result))
            {
                //可能有代理 
                if (result.IndexOf(".", StringComparison.Ordinal) == -1) //没有“.”肯定是非IPv4格式 
                    result = null;
                else
                {
                    if (result.IndexOf(",", StringComparison.Ordinal) != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。 
                        result = result.Replace(" ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        foreach (string t in temparyip)
                        {
                            if (IsIPAddress(t)
                                && t.Substring(0, 3) != "10."
                                && t.Substring(0, 7) != "192.168"
                                && t.Substring(0, 7) != "172.16.")
                            {
                                return t; //找到不是内网的地址 
                            }
                        }
                    }
                    else if (IsIPAddress(result)) //代理即是IP格式 
                        return result;
                    else
                        result = null; //代理中的内容 非IP，取IP 
                }
            }

            if (string.IsNullOrEmpty(result))
                result = request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(result))
                result = request.UserHostAddress;

            return result;
        }


        private static bool IsIPAddress(string str1)
        {
            if (string.IsNullOrEmpty(str1) || str1.Length < 7 || str1.Length > 15)
                return false;

            const string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            var regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }

        /// <summary>
        ///     位移加密字符串
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static string Encrypt(this string strInput)
        {
            strInput = Convert.ToBase64String(Encoding.Default.GetBytes(strInput));
            string strFont, strEnd;
            string strOutput;
            char[] charFont;
            int i, len, intFont;

            len = strInput.Length;
            //Console.WriteLine(" strInput @#s length is :{0} \n", len);
            strFont = strInput.Remove(len - 1, 1);
            strEnd = strInput.Remove(0, len - 1);

            //Console.WriteLine(" strFont is : {0} \n" , strFont);                                 
            //Console.WriteLine(" strEnd is : {0} \n" , strEnd);                                 

            charFont = strFont.ToCharArray();
            for (i = 0; i < strFont.Length; i++)
            {
                intFont = charFont[i] + 3;
                //Console.WriteLine(" intFont is : {0} \n", intFont);                                 

                charFont[i] = Convert.ToChar(intFont);
                //Console.WriteLine("charFont[{0}] is : {1}\n", i, charFont[i]);                                 
            }
            strFont = ""; //let strFont  null
            for (i = 0; i < charFont.Length; i++)
            {
                strFont += charFont[i];
            }
            strOutput = strEnd + strFont;
            return strOutput;
        }

        /// <summary>
        ///模糊查询SQL关键字  替换SQL关键字
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string ReplaceSql(this string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return string.Empty;
            return sql.Replace("'", "''")
                      .Replace("[", "[[]")
                      .Replace("_", "[_]")
                      .Replace("%", "[%]")
                      .Replace("#", "[#]");
        }

        /// <summary>
        ///  精确查询 如验证重名时，需要进行的替换
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string ReplaceSingleSql(this string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return string.Empty;
            return sql.Replace("'", "''");
        }

        /// <summary>
        /// 字符串32位MD5加密
        /// </summary>
        /// <param name="ConvertString"></param>
        /// <returns></returns>
        public static string GetStrMd5(string ConvertString)
        {

            if (string.IsNullOrEmpty(ConvertString))
            {
                return string.Empty;
            }
            else
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)));
                return t2;
            }
        }

        /// <summary>
        /// html解码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string html)
        {
            StringBuilder sb = new StringBuilder(HttpUtility.HtmlDecode(html));
            return sb.ToString();
        }

        /// <summary>
        /// html遍码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string html)
        {
            StringBuilder sb = new StringBuilder(HttpUtility.HtmlEncode(html));
            return sb.ToString();
        }

       
    }
}