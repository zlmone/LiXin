using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace LiXinControllers.Filter
{
    public class SystemLogAttribute : ActionFilterAttribute
    {
        private string _actionDesc = "";

        private LogLevel _logLevel = LogLevel.Warn;
        //日志记录等级

        public SystemLogAttribute()
        {
        }

        /// <summary>
        ///     记录日志
        /// </summary>
        /// <param name="actionDesc">动作描述</param>
        /// <param name="level">动作等级</param>
        public SystemLogAttribute(string actionDesc, LogLevel level)
        {
            _actionDesc = actionDesc;
            _logLevel = level;
        }

        /// <summary>
        ///     动作描述
        /// </summary>
        public string ActionDesc
        {
            get { return _actionDesc; }
            set { _actionDesc = value; }
        }

        public LogLevel LogLevel
        {
            get { return _logLevel; }
            set { _logLevel = value; }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
//var xx=filterContext..ActionDescriptor.GetParameters(); filterContext.RequestContext.

                MethodInfo[] methods =
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.GetMethods();

                Type returnType =
                    methods.First(
                        p =>
                        p.Name.Equals(filterContext.ActionDescriptor.ActionName,
                                      StringComparison.CurrentCultureIgnoreCase))
                           .ReturnType;

                //if (returnType != typeof (JsonResult))
                //{
                //    base.OnActionExecuted(filterContext);
                //    return;
                //}

                var log = new LiXinModels.SystemManage.Sys_Log();
                log.ClientIP = LiXinCommon.CodeHelper.GetClientIp(filterContext.HttpContext.Request);
                log.LogTime = DateTime.Now;
                log.LogType = 0;
                log.LogContent = ActionDesc;
                log.ModuleName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                log.RequestUrl = filterContext.HttpContext.Request.RawUrl;

                log.CurrentUserId = filterContext.HttpContext.Session["UserId"] == null ? 0 : Convert.ToInt32(filterContext.HttpContext.Session["UserId"]);
                new LiXinBLL.SystemManage.Sys_LogBL().Add(log);
            }
            catch
            {
            }
            finally
            {
                base.OnActionExecuted(filterContext);
            }
        }
    }

    public enum LogLevel
    {
        /// <summary>
        ///     调试信息
        /// </summary>
        Debug = 0,

        /// <summary>
        ///     一般信息
        /// </summary>
        Info = 1,

        /// <summary>
        ///     警告信息
        /// </summary>
        Warn = 2,

        /// <summary>
        ///     一般错误
        /// </summary>
        Error = 3,

        /// <summary>
        ///     致命错误
        /// </summary>
        Fatal = 4
    }

    //public enum LogType
    //{
    //    /// <summary>
    //    ///    新增
    //    /// </summary>
    //    Create = 0,

    //    /// <summary>
    //    ///    删除
    //    /// </summary>
    //    Delete = 1,

    //    /// <summary>
    //    ///     修改
    //    /// </summary>
    //    Update= 2,

    //    /// <summary>
    //    ///     查询
    //    /// </summary>
    //    Read = 3
    //}

}