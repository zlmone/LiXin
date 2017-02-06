using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    public class Sys_LogTrain
    {
        public int _id { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogContent { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime LogTime { get; set; }


        /// <summary>
        /// 培训日志类型 0：培训通知 1：请假记录 
        /// </summary>
        public int LogTrainType { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// 模块名次
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 请求的Url
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 通知的对象UserId
        /// </summary>
        public int NoticeUserId { get; set; }

        /// <summary>
        /// 通知的对象UserId
        /// </summary>
        public string NoticeUserName { get; set; }

        /// <summary>
        /// 通知对象的DeptId
        /// </summary>
        public int NoticeDeptId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int CurrentUserId { get; set; }

        /// <summary>
        /// 是否删除 0：正常 1：删除
        /// </summary>
        public int IsDelete { get; set; }
    }
}
