using System;
using System.IO;
using log4net;
using log4net.Config;

namespace SendMessageOCX
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

        public static void ErrorLog(string info)
        {
            if (Logerror.IsErrorEnabled)
            {

                Logerror.Error(info);
            }
        }
    }
}