using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Survey
{
    public class Survey_user
    {
       // UserId, Realname, Sex, DeptPath, TrainGrade, CPA
        public int UserId { get; set; }
        public string Realname { get; set; }
        public int Sex { get; set; }
        public string DeptPath { get; set; }
        public string TrainGrade { get; set; }
        public string CPA { get; set; }
        public int totalcount { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime { get; set; }

        public string SexStr
        {
            get
            {
                if (Sex == 0)
                    return "男";
                else if (Sex == 1)
                    return "女";
                else
                    return "保密";
            }
        }

        public string SubmitTimeStr
        {
            get
            {
                var result= this.SubmitTime.ToString("yyyy-MM-dd HH:mm");
                if (result == "0001-01-01 00:00")
                    return "--";
                return result;
            }
        }

        /// <summary>
        /// 开放人数
        /// </summary>
        public int ShouldNumber { get; set; }
    }
}
