using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinModels.NewQueryStatistics;
using LiXinBLL;
using LiXinInterface;
using LiXinCommon;
using LiXinModels.User;
using LiXinModels.NewCourseManage;

namespace LiXinControllers
{
    public partial class NewQueryStatisticsController
    {
        #region 呈现
        public ActionResult MyQueryStatistics()
        {
            int newCount = 0;
            var SumScore = 0.00;
            ViewBag.UserId = CurrentUser.UserId;
            var showUser = GetMyStatistics();
            var newUserList = nuserBL.GetNewUserList(out newCount);
            //将配置中的分数取出来
            var course = AllSystemConfigs.Find(p => p.ConfigType == 34).ConfigValue.Split(';');
            if (course[0].Count() > 0)
            {
                SumScore =Convert.ToDouble(course[0].Split(',')[0]);
            }
            //考勤百分比
            var perAtt = 0.00;
            var pertogether =  0.00;
            var perGroup =  0.00;
            var perExam = 0.00;
            var vedio = 0.00;
            var single = 0.00;
            if (course[0].Count() > 0)
            {
                var scoreStr=course[1].Split(',');
                var sumscore = course[0].Split(',')[0].StringToDouble();
                perAtt = (scoreStr[0].StringToDouble() / sumscore)*100;
                pertogether = (scoreStr[1].StringToDouble() / sumscore)* 100;
                perGroup = (scoreStr[2].StringToDouble() / sumscore) * 100;
                perExam = (scoreStr[3].StringToDouble() / sumscore) *100;
                vedio = course[2].Split(',')[1].StringToDouble();
                single = course[2].Split(',')[2].StringToDouble();
            }
            ViewBag.SumScore = SumScore;
            ViewBag.newCount = newCount;
            ViewBag.perStr = string.Format(@"考勤:{0}%，集中授课现场评分:{1}%，分组带教现场评分:{2}%，考试成绩:{3}%（视频考试成绩：{4}%，独立考试成绩：{5}%）；
", perAtt.StringToInt32(), pertogether.StringToInt32(), perGroup.StringToInt32(), perExam.StringToInt32(), vedio, single);
            return View(showUser);
        }

        #endregion

        #region 学员综合评分
        /// <summary>
        /// 我的综合评分
        /// </summary>
        /// <returns></returns>
        public ShowScore GetMyStatistics()
        {
         
            var showList = GetShowScoreList();
           
            var newShowList = showList.Where(p => p.UserID == CurrentUser.UserId).ToList();
            var showUser = newShowList.Count > 0
                               ? newShowList[0]
                               : new ShowScore();
            return showUser;
        }
        #endregion
    }
}
