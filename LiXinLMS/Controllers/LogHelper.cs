using System;
using System.IO;
using log4net;
using log4net.Config;

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
    }
}