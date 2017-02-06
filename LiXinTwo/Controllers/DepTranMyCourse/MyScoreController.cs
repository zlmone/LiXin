using LiXinModels.DeptCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;

namespace LiXinControllers
{
    public partial class DepTranMyCourseController
    {    
        #region 呈现

        public ActionResult MyDepTranScore()
        {
            ViewBag.listYear = IDepMyScore.GetDepCourseYear(CurrentUser.UserId);
            return View();
        }

        public ActionResult DepUserScore()
        {
            ViewBag.trainGrade = AllTrainGrade;
            return View();
        }

        public ActionResult DepUserScoreDetails(int userID)
        {
            ViewBag.userID = userID;
            return View();
        }


        #endregion

        #region 我的学时

        /// <summary>
        /// 获取我的学时及考勤获取(视频转播)
        /// </summary>
        /// <param name="type">0 当前人员ID  1 传将来的人员ID</param>
        /// <param name="courseName"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="year">年 </param>
        /// /// <param name="flag">percenter:来自个人中心 </param>
        /// <returns></returns>
        public JsonResult GetCourseShowList(string courseName = "", string starttime = "", string endtime = "", int year = 0, 
            int attdence = -1, int Approval = -1, int pageSize = 10, int pageIndex = 1, string flag = "", string jsrendersortfield = " StartTime desc")
        {
            try
            {
                int totalcount = 0;

                #region 条件

                var where = new StringBuilder();
                where.Append(" 1=1");
                if (year > 0)
                {
                    where.AppendFormat(" AND cc.YearPlan ={0} ", year);
                }
                if (flag == "percenter")
                {
                    where.AppendFormat(" AND dao.ApprovalFlag =1 and (dco.AttScore>0 or dco.EvlutionScore>0 or dco.ExamScore>0) ");
                }
                if (!string.IsNullOrEmpty(courseName))
                {
                    where.AppendFormat(" AND cc.CourseName like '%{0}%' ", courseName.ReplaceSql());
                }
                if (!string.IsNullOrEmpty(starttime))
                {
                    where.AppendFormat(" AND cc.StartTime  >='{0}' ", starttime);
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    where.AppendFormat(" AND cc.EndTime <= '{0}' ", endtime);
                }
                if (attdence != -1)
                {
                    if (attdence == 0)
                    {
                        where.AppendFormat(" And (dao.Status=0 or dao.Status is null) ");
                    }
                    else
                    {
                        where.AppendFormat(" And dao.Status=" + attdence);
                    }
                }
                switch (Approval)
                {
                    //未提交
                    case 0:
                        where.AppendFormat(" And ((OpenApprovalFlag=0 OR OpenApprovalFlag IS null) AND (t.AttFlag=0 OR t. AttFlag IS NULL))");
                        break;
                    //未审批
                    case 1:
                        where.AppendFormat(" And (OpenApprovalFlag=0 AND t.AttFlag=1)");
                        break;
                    //已通过
                    case 2:
                        where.AppendFormat(" And (OpenApprovalFlag=1 AND t.AttFlag=1)");
                        break;
                    //已拒绝
                    case 3:
                        where.AppendFormat(" And (OpenApprovalFlag=1 AND t.AttFlag=0)");
                        break;

                }

                #endregion

                var list = scoreBL.GetCourseShow(out totalcount, CurrentUser.UserId, where.ToString(), pageIndex, pageSize, jsrendersortfield);
                //个人中心时呈现得分
                if (flag == "percenter")
                {
                    list.ForEach(p =>
                    {
                        p.CourseType = 1;
                        //计算考试成绩 
                        if (p.CoPaperID > 0 && p.PaperId > 0)
                        {
                            var exam = ExamTest.GetExamSendStudentBySql2008(p.CourseID, CurrentUser.UserId, p.CoPaperID, p.PaperId, 4);
                            p.GetexamScore = exam == null ? 0 : exam.StudentAnswerList.Sum(pa => pa.GetScore);
                            var paper = IExampaperBL.GetExampaper(p.PaperId);
                            p.ExamScore = paper.ExampaperScore;
                        }
                        else
                        {
                            p.GetexamScore = 0;
                            p.ExamScore = 0;
                        }
                    });
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
                    dataList = new List<CourseShow>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 部门/分所中员工的学时
        /// <summary>
        /// 部门/分所中员工的学时
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllUserList(string realName = "", int sex = -1, string TrainGrade = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " Realname asc")
        {
            try
            {
                int totalcount = 0;
                #region 条件
                string where = " 1=1";
                if (!string.IsNullOrEmpty(TrainGrade))
                {
                    // where += string.Format(" And cc.OpenLevel LIKE '%{0}%'", openlevel);
                    where += string.Format(" And (SELECT count(*) FROM  dbo.F_SplitIDs(TrainGrade)  WHERE ID  IN (SELECT ID FROM dbo.F_SplitIDs('{0}')) )>0", TrainGrade);
                }
                if (!string.IsNullOrEmpty(realName))
                {
                    where += string.Format(@" and Realname LIKE '%{0}%'", realName.ReplaceSql());
                }
                if (sex != -1)
                {
                    where += " AND sex=" + sex;
                }
                where += " And UserType<>'离职人员'";
                #endregion
                var list = scoreBL.GetDeptUserScore(out totalcount, CurrentUser.UserId, where, pageIndex, pageSize, jsRenderSortField);
                var showlist = new List<object>();
                list.ForEach(p => showlist.Add(new
                {
                    UserId = p.UserId,
                    Realname = p.Realname,
                    SexStr = p.SexStr,
                    MobileNum = p.MobileNum,
                    Emailstr = p.Emailstr,
                    TrainGrade = p.TrainGrade,
                    CPA = p.CPA,
                    GetScore = p.GetScore
                }));
                return Json(new
                {
                    dataList = showlist,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<CourseShow>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 部门/分所下人员的获取学时详情
        /// </summary>
        public JsonResult GetDepUserScoreDetails(int userID, string courseName = "", string starttime = "", string endtime = "", int attdence = -1, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalcount = 0;

                #region 条件

                StringBuilder where = new StringBuilder();
                where.Append(" 1=1");
                //  where.AppendFormat(" AND dao.ApprovalFlag =1 and (dco.AttScore>0 or dco.EvlutionScore>0 or dco.ExamScore>0) ");
                if (!string.IsNullOrEmpty(courseName))
                {
                    where.AppendFormat(" AND CourseName like '%{0}%' ", courseName.ReplaceSql());
                }
                if (!string.IsNullOrEmpty(starttime))
                {
                    where.AppendFormat(" AND cc.StartTime  >='{0}' ", starttime);
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    where.AppendFormat(" AND cc.EndTime <= '{0}' ", endtime);
                }
                if (attdence != -1)
                {
                    if (attdence == 0)
                    {
                        where.AppendFormat(" And (dao.Status=0 or dao.Status is null) ");
                    }
                    else
                    {
                        where.AppendFormat(" And dao.Status=" + attdence);
                    }
                }

                #endregion

                var list = scoreBL.GetDepUserScoreDetails(out totalcount, CurrentUser.UserId, userID, where.ToString(), pageIndex, pageSize);
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
                    dataList = new List<CourseShow>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

    }
}
