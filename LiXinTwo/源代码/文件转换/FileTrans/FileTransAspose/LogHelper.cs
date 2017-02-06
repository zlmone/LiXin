using System;
using log4net;

namespace FileTransAspose
{
    class LogHelper
    {
        private static readonly ILog Loginfo = LogManager.GetLogger("loginfo");   //选择<logger name="loginfo">的配置 

        private static readonly ILog Logerror = LogManager.GetLogger("logerror");   //选择<logger name="logerror">的配置 

        public static void WriteLog(string info)
        {
            if (Loginfo.IsInfoEnabled)
            {
                Loginfo.Info(info);
            }
        }

        public static void WriteLog(string info, Exception e)
        {
            if (Logerror.IsErrorEnabled)
            {
                Logerror.Error(info, e);
            }
        }
    }
}
