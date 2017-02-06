using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LiXinCommon
{
    public static class ExtendMethod
    {
        

        /// <summary>
        ///     字符串长度截取
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static string ToLimitString(this string str, int length)
        {
            return str.Length > length ? (str.Substring(0, length) + "…") : str;
        }

        /// <summary>
        ///     转换成整型
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns></returns>
        public static int StringToInt32(this object obj)
        {
            int result;
            return obj == null ? -1 : (int.TryParse(obj.ToString(), out result) ? result : -1);
        }


        /// <summary>
        ///     转换成日期
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="flag">当日期为空或错误时，0:返回最小时间；1：返回最大时间；2：返回当前时间</param>
        /// <returns></returns>
        public static DateTime StringToDate(this object obj, int flag)
        {
            DateTime date;
            if (obj == null || !DateTime.TryParse(obj.ToString(), out date))
            {
                switch (flag)
                {
                    case 0:
                        date = DateTime.MinValue;
                        break;
                    case 1:
                        date = DateTime.MaxValue;
                        break;
                    default:
                        date = DateTime.Now;
                        break;
                }
            }
            return date;
        }

        /// <summary>
        ///     随机从集合中取出指定个数的元素
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">集合</param>
        /// <param name="count">所取个数</param>
        /// <returns></returns>
        public static List<T> RandomGetSome<T>(this List<T> obj, int count)
        {
            if (obj.Count <= count)
                return obj;
            var ran = new Random();
            var result = new List<T>();
            int length = obj.Count - 1;
            for (int i = 0; i < count; i++)
            {
                int pos = ran.Next(i, length);
                result.Add(obj[pos]);
                T temp = obj[i];
                obj[i] = obj[pos];
                obj[pos] = temp;
            }
            return result;
        }

        /// <summary>
        ///     随机从数组中取出指定个数的元素
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="obj">数组</param>
        /// <param name="count">所取个数</param>
        /// <returns></returns>
        public static T[] RandomGetSome<T>(this T[] obj, int count)
        {
            if (obj.Length <= count)
                return obj;
            var ran = new Random();
            var result = new T[count];
            int length = obj.Length;
            for (int i = 0; i < count; i++)
            {
                int pos = ran.Next(i, length);
                result[i] = obj[pos];
                T temp = obj[i];
                obj[i] = obj[pos];
                obj[pos] = temp;
            }
            return result;
        }

        /// <summary>
        ///     有的表格里面要另外加ovh等等的，套div
        /// </summary>
        /// <param name="strcontent">你懂的</param>
        /// <returns></returns>
        public static string StringOvh(this string strcontent)
        {
            return String.Format("<div class='ovh' title='{0}'>{0}</div>", strcontent);
        }

        /// <summary>
        ///     去除字符串中的html代码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string NoHtml(this string str)
        {
            return Regex.Replace(str, @"<[^>]*>", "");
        }

        /// <summary>
        ///     集合里的元素随机排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">集合</param>
        /// <returns></returns>
        public static List<T> RandomListOrder<T>(this List<T> obj)
        {
            var r = new Random();
            if (obj.Count < 2)
                return obj;
            var newobj = new List<T>();
            for (int i = obj.Count - 1; i >= 0; i--)
            {
                int index = r.Next(0, obj.Count - 1);
                newobj.Add(obj[index]);
                obj.Remove(obj[index]);
            }
            return newobj;
        }

        /// <summary>
        ///     去重标签
        /// </summary>
        /// <param name="split">分隔符</param>
        /// <param name="inputStr">输入的标签</param>
        /// <returns></returns>
        public static string BuildTags(string split, string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
                return "";
            return string.Join(split,
                               inputStr.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries).ToList().Distinct());
        }

        /// <summary>
        ///     MongoDB模糊查询条件解析
        /// </summary>
        /// <returns></returns>
        public static string FuzzyQuery(this string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
                return "";
            string newStr = inputStr;
            if (inputStr.Contains(@"\"))
            {
                newStr = newStr.Replace(@"\", @"\\");
            }
            if (inputStr.Contains("("))
            {
                newStr = newStr.Replace("(", @"\(");
            }
            if (inputStr.Contains(")"))
            {
                newStr = newStr.Replace(")", @"\)");
            }
            if (inputStr.Contains("["))
            {
                newStr = newStr.Replace("[", @"\[");
            }
            if (inputStr.Contains("{"))
            {
                newStr = newStr.Replace("{", @"\{");
            }
            if (inputStr.Contains("}"))
            {
                newStr = newStr.Replace("}", @"\}");
            }
            if (inputStr.Contains("^"))
            {
                newStr = newStr.Replace("^", @"\^");
            }
            if (inputStr.Contains("*"))
            {
                newStr = newStr.Replace("*", @"\*");
            }
            if (inputStr.Contains("$"))
            {
                newStr = newStr.Replace("$", @"\$");
            }
            if (inputStr.Contains("."))
            {
                newStr = newStr.Replace(".", @"\.");
            }
            if (inputStr.Contains("|"))
            {
                newStr = newStr.Replace("|", @"\|");
            }
            if (inputStr.Contains("?"))
            {
                newStr = newStr.Replace("?", @"\?");
            }
            if (inputStr.Contains("+"))
            {
                newStr = newStr.Replace("+", @"\+");
            }
            return newStr;
        }

        /// <summary>
        /// html编码
        /// </summary>
        /// <returns></returns>
        public static string HtmlXssEncode(this string inputStr)
        {
            return Microsoft.Security.Application.Encoder.HtmlEncode(inputStr);
        }

    
    }
}
