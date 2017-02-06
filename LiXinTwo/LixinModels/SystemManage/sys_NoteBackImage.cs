using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    public class sys_NoteBackImage
    {
        public int ID { get; set; }

        /// <summary>
        /// 上传的图片保存的名称
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 上传的图片的真实姓名 
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 默认为背景  0 不默认 1 默认为公告的背景  2 默认为课程的背景 3 1和2的总和
        /// </summary>
        public int defalutType { get; set; }

        public int totalcount { get; set; }

        public DateTime CreateTime { get; set; }

        public string defalutTypeStr
        {
            get
            {
                var result = "";
                switch (defalutType)
                {
                    case 0:
                        result = "正常";
                        break;
                    case 1:
                        result = "公告的背景";
                        break;
                    case 2:
                        result = "课程的背景";
                        break;
                    case 3:
                        result = "公告/课程的背景";
                        break;
                }
                return result;
            }
        }
    }
}
