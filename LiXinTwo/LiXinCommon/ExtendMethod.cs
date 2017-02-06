using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        /// 转换成整型
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns></returns>
        public static double StringToDouble(this object obj)
        {
            int result;
            return obj == null ? 0.00 : Convert.ToDouble(obj.ToString());
        }


        /// <summary>
        ///     转换成整型为null返回0
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns></returns>
        public static int StringToInt32Zero(this object obj)
        {
            int result;
            return obj == null ? 0 : (int.TryParse(obj.ToString(), out result) ? result : -1);
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
            str = str ?? "";
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

        /// <summary>
        /// 动态列的排序 
        /// </summary>
        /// <param name="sortList">要排序的List</param>
        /// <param name="jsRenderSortField">排序字段 形如 ID desc  其中  desc 或者 asc 全部为小写或者大写</param>
        /// <returns></returns>
        public static List<T> SortListByField<T>(this List<T> sortList, string jsRenderSortField)
        {
            //jsRenderSortField = jsRenderSortField.ToLower();

            if (jsRenderSortField.Contains("DESC") || jsRenderSortField.Contains("desc"))
            {
                var Filed = jsRenderSortField.Replace("DESC", "").Replace("desc", "").Trim();
                PropertyInfo pi = typeof(T).GetProperty(Filed);
                sortList = sortList.OrderByDescending(p => GetValue(p, Filed)).ToList();
            }
            else
            {
                var Filed = jsRenderSortField.Replace("asc", "").Replace("ASC", "").Trim();
                sortList = sortList.OrderBy(p => GetValue(p, Filed)).ToList();
            }
            return sortList;
        }

        /// <summary>
        /// 动态获取如model对象中的XX对象的XX属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private static object GetValue<T>(T model, string field)
        {
            return model.GetType().GetProperty(field).GetValue(model, null);
        }

        /// <summary>
        /// 将年份转换为大写
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string GetCapitalByYear(this string year)
        {

            var Convert = "";
            var capital = "零一二三四五六七八九";
            var number = "0123456789";
            var strlen = year.Length;
            for (int i = 0; i < strlen; i++)
            {
                var str = year.ToString()[i];

                Convert += capital[number.IndexOf(str)].ToString();
            }
            return Convert;

        }
    }
}
