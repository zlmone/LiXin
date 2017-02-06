using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinSync
{
    public class TrainUserFinger : BaseModel
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { set; get; }
        /// <summary>
        /// 指纹1
        /// </summary>
        public string FingerPrint1 { set; get; }
        /// <summary>
        /// 指纹2
        /// </summary>
        public string FingerPrint2 { set; get; }
    }
}
