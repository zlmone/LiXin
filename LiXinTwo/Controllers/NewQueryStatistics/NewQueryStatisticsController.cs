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
using System.Data;
using LiXinInterface.User;

namespace LiXinControllers
{
    public partial class NewQueryStatisticsController : BaseController
    {
        public IStatistics statisticsBL;
        public IUserStatistics iUserBL;
        public INewUser nuserBL;
        public IUser userBL;
        public NewQueryStatisticsController(IStatistics _statisticsBL, IUserStatistics _iUserBL, INewUser _nuserBL, IUser _userBL)
        {
            statisticsBL = _statisticsBL;
            iUserBL = _iUserBL;
            nuserBL = _nuserBL;
            userBL = _userBL;
        }

        #region 呈现
        public ActionResult StudentQueryStatistics(int UserID, int index, string name, double sum)
        {
            ViewBag.UserID = UserID;
            ViewBag.index = index;
            ViewBag.name = name;
            ViewBag.sum = sum;
            var model = nuserBL.GetModel(" userID=" + UserID);
            ViewBag.Exam = model == null ? "0/0" : model.Score + "/" + model.SumScore;

            return View();
        }

        public ActionResult StudentQueryStatisticsList()
        {
            var yearList = nuserBL.GetNewYear();
            yearList.Add(yearList.Last() + 1);
            ViewBag.yearList = yearList;
            ViewBag.nowyear = DateTime.Now.Year;
            return View();
        }

        public ActionResult TeacherQueryStatistics(int teacherID)
        {
            ViewBag.teacherID = teacherID;
            return View();
        }

        public ActionResult TeacherQueryStatisticsList()
        {
            return View();
        }

        public ActionResult ScoreMemo(int id, int paperId, int courseID, int userID, int coPaperID, int IsGroupTeach)
        {
            ViewBag.IsGroupTeach = IsGroupTeach;
            ViewBag.Id = id;
            var score = 0;
            var sumScore = 0;
            if (paperId > 0)
            {
                iUserBL.GetCourseExamScore(courseID, paperId, userID, coPaperID, out score, out sumScore);
            }
            ViewBag.Score = score;
            ViewBag.sumScore = sumScore;
            var list = iUserBL.GetAttendceList(type: 1, where: " ncod.courseID=" + courseID + " and ncod.userID=" + userID);
            ViewBag.AttendStr = list.Count() == 0 ? "" : list.FirstOrDefault().AttendStr;
            return View();
        }

        public ActionResult ScoreDetails(int type, int userID)
        {
            var thead = "";
            switch (type)
            {
                case 0:
                    thead = "集中授课现场评分";
                    break;
                case 1:
                    thead = "分组带教现场评分";
                    break;
                case 2:
                    thead = "考勤得分";
                    break;
                case 3:
                    thead = "评估奖励";
                    break;
                case 4:
                    thead = "混合课程考试得分";
                    break;
                case 6:
                    thead = "视频课程考试得分";
                    break;
            }
            ViewBag.thead = thead;
            ViewBag.type = type;
            ViewBag.userID = userID;
            var model = nuserBL.GetModel(" userID=" + userID);
            var userModel = userBL.Get(userID);
            ViewBag.Exam = model == null ? "0/0" : model.Score + "/" + model.SumScore;
            ViewBag.Score = model == null ? 0 : model.Score;
            ViewBag.SumScore = model == null ? 0 : model.SumScore;
            ViewBag.realname = userModel.Realname;
            return View();
        }
        #endregion

        #region 讲师评价
        /// <summary>
        /// 讲师评价
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTeacherStatisticsList(string realName = "", int isTeacher = -1, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " UserId desc")
        {
            try
            {
                int totalcount = 0;
                string where = " 1=1";

                #region 条件
                if (!string.IsNullOrEmpty(realName))
                {
                    where += string.Format(@" and Realname LIKE '%{0}%'", realName.ReplaceSql());
                }
                if (isTeacher != -1)
                {
                    where += " and IsTeacher=" + isTeacher;
                }


                #endregion

                var list = statisticsBL.GetTeacherStatisticsList(out totalcount, pageIndex, pageSize, where, jsRenderSortField);

                foreach (var item in list)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
                }
                return Json(new
                {
                    dataList = list,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<TeacherStatistics>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        public JsonResult GetCourseStatistics(int teacherID, int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " Id desc")
        {
            try
            {
                int totalcount = 0;
                var where = " 1=1";
                if (teacherID != -1)
                {
                    where += " and (SELECT COUNT(*) FROM (SELECT ID FROM dbo.F_SplitIDs(Teachers))t WHERE   t.ID=" + teacherID + ")>0";
                }
                var list = statisticsBL.GetCourseStatistics(out totalcount, pageIndex, pageSize, where, jsRenderSortField);
                return Json(new
                {
                    dataList = list,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<CourseStatistics>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 学员综合评分
        /// <summary>
        /// 学员综合评分
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserStatistics(int Year=-1,string Realname = "", string NumberID = "", int pageSize = 10, int pageIndex = 1)
        {
            try
            {

                var ShowList = GetShowScoreList(Realname, NumberID, Year);

                int totalcount = ShowList.Count;

                ShowList = ShowList.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

                return Json(new
                {
                    dataList = ShowList,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<ShowScore>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public void OutScore(string Realname = "", string NumberID = "")
        {
            var List = GetShowScoreList(Realname, NumberID);

            DataTable outTable = new DataTable();
            outTable.Columns.Add("排名", typeof(string));
            outTable.Columns.Add("学号", typeof(string));
            outTable.Columns.Add("姓名", typeof(string));
            outTable.Columns.Add("所在部门", typeof(string));
            outTable.Columns.Add("考勤得分", typeof(string));
            outTable.Columns.Add("集中授课现场评分", typeof(string));
            outTable.Columns.Add("分组带教现场评分", typeof(string));
            outTable.Columns.Add("混合考试成绩", typeof(string));
            outTable.Columns.Add("视频考试成绩", typeof(string));
            outTable.Columns.Add("独立考试成绩", typeof(string));
            outTable.Columns.Add("奖励", typeof(string));
            outTable.Columns.Add("综合评分", typeof(string));
            int num = 0;
            foreach (var v in List)
            {
                outTable.Rows.Add(v.number, v.NumberID, v.Realname, v.DeptName, v.SAttendScore, v.StogetherSumScore, v.SgroupSumScore
                    , v.cSumScore, v.vSumScore, v.eSumScore, v.SRewardScore, v.SSumScore);
            }
            new Spreadsheet().Template("学员综合评分", null, outTable, HttpContext, "学员综合评分", "学员综合评分");
        }
        #endregion

        #region 综合评分详情（课程维度）
        /// <summary>
        /// 综合评分详情（课程维度）
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPersonalStatistics(int userID, int Way = -1, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalcount = 0;
                var where = " 1=1";
                if (Way != -1)
                {
                    where += " And nc.Way=" + Way;
                }
                var list = iUserBL.GetCourseList(out totalcount, userID, pageIndex, pageSize, where);

                var scorelist = iUserBL.GetListByUserID();

                //考试的分数
                foreach (var item in list)
                {
                    var score = scorelist.Find(p => p.CourseId == item.CourseId && p.UserId == userID);
                    item.CourseExamScore = score == null ? 0 : score.CourseExamScore;
                    item.CourseExamSumScore = score == null ? 0 : score.CourseExamSumScore;
                }

                return Json(new
                {
                    dataList = list,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<New_CourseOrder>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 穿透
        /// <summary>
        /// 穿透的时候用的。集中等的详情
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public JsonResult GetScoreDetails(int userID, int type, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalcount = 0;
                var courseList = iUserBL.GetCourseList(out totalcount, userID);

                //独立考试的答案
                var scorelist = iUserBL.GetListByUserID();
                var objList = new List<object>();
                switch (type)
                {
                    case 0://集中
                        courseList = courseList.Where(p => p.TogetherScore != 0 && (p.IsGroupTeach == 1 || p.IsGroupTeach == 3)).OrderByDescending(p => p.TogetherScore).ToList();
                        break;
                    case 1://分组
                        courseList = courseList.Where(p => p.GroupScore != 0 && (p.IsGroupTeach == 2 || p.IsGroupTeach == 3)).OrderByDescending(p => p.GroupScore).ToList();
                        break;
                    case 3://奖励
                        courseList = courseList.Where(p => p.IsReward > 0 && p.Way == 1).ToList();
                        break;
                    case 4://混合课程考试

                        foreach (var item in courseList)
                        {
                            var score = scorelist.Find(p => p.CourseId == item.CourseId && p.UserId == userID);
                            item.CourseExamScore = score == null ? 0 : score.CourseExamScore;
                            item.CourseExamSumScore = score == null ? 0 : score.CourseExamSumScore;
                        }
                        courseList = courseList.Where(p => p.CourseExamSumScore != 0 && p.Way == 1).ToList();
                        break;
                    case 6://视频课程考试

                        foreach (var item in courseList)
                        {
                            var score = scorelist.Find(p => p.CourseId == item.CourseId && p.UserId == userID);
                            item.CourseExamScore = score == null ? 0 : score.CourseExamScore;
                            item.CourseExamSumScore = score == null ? 0 : score.CourseExamSumScore;
                        }
                        courseList = courseList.Where(p => p.CourseExamSumScore != 0 && p.Way == 2).ToList();
                        break;
                }
                int count = courseList.Count;

                courseList = courseList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new
                {
                    dataList = courseList,
                    recordCount = count
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<New_CourseOrder>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 穿透的时候用的,考勤的详情
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public JsonResult GetAttedceDetails(int userID, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                var list = iUserBL.GetAttendceList(userID, pageIndex, pageSize);
                return Json(new
                {
                    dataList = list,
                    recordCount = list.Count() == 0 ? 0 : list.FirstOrDefault().totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<New_CourseOrderDetail>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


    }
}
