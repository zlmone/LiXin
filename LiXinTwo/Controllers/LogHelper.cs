using System;
using System.IO;
using log4net;
using log4net.Config;
using System.Web.Mvc;
using LiXinBLL.SystemManage;
using System.Collections.Generic;
using LiXinModels.SystemManage;

namespace LiXinControllers
{
    public class LogHelper
    {
        public static readonly ILog Loginfo = LogManager.GetLogger("loginfo");

        public static readonly ILog Logerror = LogManager.GetLogger("logerror");

        public static void SetConfig()
        {
            DOMConfigurator.Configure();
        }

        public static void SetConfig(FileInfo configFile)
        {
            DOMConfigurator.Configure(configFile);
        }

        public static void WriteLog(string info)
        {
            if (Loginfo.IsInfoEnabled)
            {
                Loginfo.Info(info);
            }
        }

        public static void WriteLog(string info, Exception se)
        {
            if (Logerror.IsErrorEnabled)
            {

                Logerror.Error(info, se);
            }
        }

        /// <summary> 
        /// 写入异常日志Mongodb
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void WriteLog(LiXinModels.SystemManage.Sys_Log log)
        {
            if (Logerror.IsErrorEnabled)
            {
                new LiXinBLL.SystemManage.Sys_LogBL().Add(log);
            }
        }

        public static void WriteLog(string ActionDesc, ControllerContext filterContext)
        {
            var log = new Sys_Log();
            log.ClientIP = LiXinCommon.CodeHelper.GetClientIp(filterContext.HttpContext.Request);
            log.LogTime = DateTime.Now;
            log.LogType = 0;
            log.LogContent = ActionDesc;

            log.CurrentUserId = filterContext.HttpContext.Session["UserId"] == null ? 0 : Convert.ToInt32(filterContext.HttpContext.Session["UserId"]);

            new Sys_LogBL().Add(log);
        }

        /// <summary>
        /// 专门记录短信邮件
        /// </summary>
        /// <param name="listContent"></param>
        /// <param name="filterContext"></param>
        /// <param name="type">5短信，6邮件  </param>
        /// <param name="status">1成功 0失败</param>
        /// <param name="ctype">0正常发送 1截取逗号后面的（坑爹的日志）</param>
        public static void WriteLog(List<KeyValuePair<string, string>> listContent, ControllerContext filterContext, int type, int status, int ctype)
        {
            foreach (var item in listContent)
            {
                var log = new Sys_Log();
                log.ClientIP = LiXinCommon.CodeHelper.GetClientIp(filterContext.HttpContext.Request);
                log.LogTime = DateTime.Now;
                log.LogType = type;
                
                log.AcceptName = item.Value.Split('，')[0];
                log.LogContent = ctype == 0 ? item.Value : item.Value.Replace(log.AcceptName, "");
                log.Status = status;
                log.CurrentUserId = filterContext.HttpContext.Session["UserId"] == null ? 0 : Convert.ToInt32(filterContext.HttpContext.Session["UserId"]);
                new Sys_LogBL().Add(log);
            }

        }


    }
}