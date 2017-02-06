using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Retech.Cache;
using Retech.Core.Cache;
using LiXinBLL.User;
using LiXinModels.User;
using LiXinCommon.Configs;

namespace LiXinControllers.Filter
{
    /// <summary>
    ///     访问限制（通过用户权限判断）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BaseAccessAttribute : ActionFilterAttribute
    {
        protected ICacheManager cacheManager = new MemoryCacheManager();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MethodInfo[] methods =
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.GetMethods();

            Type returnType =
                methods.First(
                    p =>
                    p.Name.Equals(filterContext.ActionDescriptor.ActionName, StringComparison.CurrentCultureIgnoreCase))
                       .ReturnType;
            if (returnType == typeof(JsonResult))
            {
                base.OnActionExecuting(filterContext);
                return;
            }
            string errorString;
            bool hasRight = HasRight(filterContext, out errorString);
            if (hasRight)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                string backUrl;
                if (filterContext.HttpContext.Request.UrlReferrer != null)
                {
                    backUrl = filterContext.HttpContext.Request.UrlReferrer.PathAndQuery.TrimStart('/');
                    filterContext.HttpContext.Session["errorString"] = errorString;
                    filterContext.Result =
                        new RedirectResult("/Common/RightError?backUrl=" +
                                           filterContext.HttpContext.Server.UrlEncode(backUrl));
                }
                else
                {
                    filterContext.HttpContext.Session["errorString"] = errorString;
                    filterContext.Result = new RedirectResult("/Common/RightError?backUrl=");
                }
            }
        }

        protected virtual bool HasRight(ActionExecutingContext filterContext, out string message)
        {
            //判断有没有权限
            var filterContextInfo = new FilterContextInfo(filterContext);

            string url = filterContextInfo.controllerName + "/" + filterContextInfo.actionName;
            bool hasRight = false;

            //使用配置文件的方式去掉权限系统不管理的Url
            List<string> exUrls = cacheManager.Get("ExcludeUrls", () =>
                {
                    var urls = (UrlExcludeConfigurationSection)ConfigurationManager.GetSection("excludeUrls");
                    return (from RetechUrlConfigurationElement urlElement in urls.ExcludeUrls
                            select urlElement.Url).ToList();
                });
            if (exUrls != null && exUrls.Exists(p => p.Equals(url, StringComparison.CurrentCultureIgnoreCase)))
            {
                hasRight = true;
            }
            else
            {
                List<Sys_Right> allRights = cacheManager.Get("all_rights", () => { return new RightBL().GetAllRights(); });
                if (allRights != null)
                {
                    Sys_Right right = allRights.Find(p => p.Path.ToUpper() == url.ToUpper());
                    if (right != null)
                    {
                        var userRights = filterContext.HttpContext.Session["myRights"] as List<Sys_Right>;
                        if (userRights != null)
                        {
                            if (userRights.Exists(p => p.RightId == right.RightId))
                            {
                                hasRight = true;
                            }
                        }
                    }
                    else
                    {
                        hasRight = true;
                    }
                }
            }
            if (filterContext.HttpContext.Session["myRights"] == null)
            {
                message = "未登录";
            }
            else if (!hasRight)
            {
                message = "无权限";
            }
            else
            {
                message = "";
            }
            return hasRight;
        }
    }
}