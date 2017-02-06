using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.SystemManage
{
    public class Sys_MessageDetails
    {
        public int _id { get; set; }

        //日志主键，主要用作查看详情
        public int Logid { get; set; }

        //发送对象的姓名
        public string UserName { get; set; }

        //发送内容
        public string Content { get; set; }

        //1 成功 0 失败（仅指发送到第三方成功）
        public int Status { get; set; }

        //发送时间
        public DateTime SendTime { get; set; }

    }
}
