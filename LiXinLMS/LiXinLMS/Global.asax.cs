using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Retech.DIUnity.DependencyResolver;
using LiXinCommon;
namespace LiXinLMS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Login", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DependencyResolver.SetResolver(new DependencyResolverFactory().CreateDependencyResolver());

            LiXinControllers.GlobalThreading.Instance().Start();
            LiXinControllers.LogHelper.SetConfig();
         
        }

        /// <summary>
        ///     捕获整个解决方案下的所有异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Exception objExp = HttpContext.Current.Server.GetLastError();
                string requestUrl = "";
                int indexMark = HttpContext.Current.Request.RawUrl.IndexOf("/", 1);
                if (indexMark > 0)
                {
                    requestUrl = HttpContext.Current.Request.RawUrl.Substring(1, indexMark - 1);
                }

                var log = new LiXinModels.SystemManage.Sys_Log()
                {
                    ClientIP = LiXinCommon.CodeHelper.GetClientIp(HttpContext.Current.Request),
                    LogTime = DateTime.Now,
                    LogType = 2,
                    LogContent = HttpContext.Current.Server.GetLastError().Message,
                    ModuleName = requestUrl,
                    RequestUrl = HttpContext.Current.Request.RawUrl,
                    CurrentUserId = Session["UserId"] == null ? 0 :
                    Convert.ToInt32(Session["UserId"])
                };
                LiXinControllers.LogHelper.WriteLog(log);
            }
            catch (Exception ex)
            {
                LiXinControllers.LogHelper.WriteLog("异常信息:" + ex.Message + "时间" + DateTime.Now);

            }
        }
    }
}