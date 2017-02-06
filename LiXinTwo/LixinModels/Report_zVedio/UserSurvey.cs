using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels.Report_zVedio
{
    public class UserSurvey
    {
        /// <summary>
        /// CourseID
        /// </summary>
        public int ObjectID { get; set; }

        public int UserID { get; set; }
        public string UserIDs { get; set; }

        /// <summary>
        /// 调查问卷的ID
        /// </summary>
        public int ExampaperID { get; set; }

        /// <summary>
        /// 评价的分数
        /// </summary>
        public Decimal PingSum { get; set; }

        /// <summary>
        /// 每个人每次回答的时候星评题的数量
        /// </summary>
        public int questionCount { get; set; }

        /// <summary>
        /// 每个人回答的次数
        /// </summary>
        public int userNumber { get; set; }

        public int DeptId { get; set; }
        /// <summary>
        /// 平均分
        /// </summary>
        public double AveragePing
        {
            get
            {
                return questionCount == 0 ? 0 : Math.Round(Convert.ToDouble(PingSum) / Convert.ToDouble(questionCount), 2, MidpointRounding.AwayFromZero);
            }
        }

    }
}
