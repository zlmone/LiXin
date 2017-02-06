using System.Web.Mvc;

namespace LiXinControllers.Filter
{
    public class FilterContextInfo
    {
        public FilterContextInfo(ActionExecutingContext filterContext)
        {
            #region 获取链接中的字符

            // 获取域名
            domainName = filterContext.HttpContext.Request.Url.Authority;
            //获取模块名称
            //module = filterContext.HttpContext.Request.Url.Segments[1].Replace('/', ' ').Trim();
            //获取 controllerName 名称
            controllerName = filterContext.RouteData.Values["controller"].ToString();
            //获取ACTION 名称
            actionName = filterContext.RouteData.Values["action"].ToString();

            #endregion
        }

        /// <summary>
        ///     获取域名
        /// </summary>
        public string domainName { get; set; }

        /// <summary>
        ///     获取模块名称
        /// </summary>
        public string module { get; set; }

        /// <summary>
        ///     获取 controllerName 名称
        /// </summary>
        public string controllerName { get; set; }

        /// <summary>
        ///     获取ACTION 名称
        /// </summary>
        public string actionName { get; set; }
    }
}