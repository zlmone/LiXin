using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.NewClassManage;
using LiXinInterface.User;
using LiXinModels.NewCourseManage;
using LiXinModels.Survey;
using LiXinModels.User;
using LixinModels.NewClassManage;
using System.Linq;
using System.Text.RegularExpressions;

namespace LiXinControllers.NewMyEvaluate
{
    public partial class NewMyEvaluateController
    {

        #region == 界面呈现 ==

        public ActionResult NewMyEvaluateMain(int courseRoomRuleId,string keywords="")
        {
            List<AfterCourceDetail> teacheracdlist;
            List<AfterCourceDetail> courseacdlist;
            var courseRoomRuleDetail = GetNewEvaluateUserToTeacherDetail(courseRoomRuleId, out teacheracdlist, out courseacdlist, keywords.ReplaceSql());
            ViewBag.teacheracdlist = teacheracdlist;
            ViewBag.courseacdlist = courseacdlist;
            return View(courseRoomRuleDetail);
        }



        /// <summary>
        /// 管理员端-所有学员对讲师的评价页面
        /// </summary>
        /// <returns></returns>
        public ActionResult NewEvaluateAllUserToTeacher(string tp)
        {
            ViewBag.nowTime = DateTime.Now.Year + "-01-01";
            ViewBag.page = 1;
            ViewBag.Attname = "请输入课程名称";
            ViewBag.AttSort = "-1";
            ViewBag.AttlistFlag = 1;
         //   Session["netpage"] = pageIndex + "㉿" + courseName + "㉿" + startTime + "㉿" + endTime + "㉿" + type + "㉿" + listFlag + "㉿" + jsRenderSortField;

            if (tp != null && tp != "" && tp == "1")
            {
                if (Session["Newnetpage"] != null)
                {
                    string sess = Session["Newnetpage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Attname = att[1] == "" ? "请输入课程名称" : att[1];
                    ViewBag.AttstartTime = att[2];
                    ViewBag.AttendTime = att[3];
                    ViewBag.Atttype = att[4];
                    ViewBag.AttlistFlag = att[5];
                    ViewBag.AttSort = att[6];
                }
            }
            return View();
        }

        /// <summary>
        /// 管理员端-学员对讲师的评价详情页面
        /// </summary>
        /// <param name="courseRoomRuleId">新员工课程教室分配规则ID</param>
        /// <returns></returns>
        public ActionResult NewEvaluateAllUserToTeacherDetail(int courseRoomRuleId)
        {
            //List<AfterCourceDetail> teacheracdlist;
            //List<AfterCourceDetail> courseacdlist;
            //var courseRoomRuleDetail = GetNewEvaluateUserToTeacherDetail(courseRoomRuleId, out teacheracdlist, out courseacdlist);
            //ViewBag.teacheracdlist = teacheracdlist;
            //ViewBag.courseacdlist = courseacdlist;
            var courseRoomRuleDetail = NewCourseRoomRuleBl.GetCourseRoomRuleEvaluate(courseRoomRuleId);
            return View(courseRoomRuleDetail);
        }


        /// <summary>
        /// 讲师端-学员对讲师的评价页面
        /// </summary>
        /// <returns></returns>
        public ActionResult NewEvaluateUserToTeacher()
        {
            return View();
        }

        /// <summary>
        /// 讲师端-学员对讲师的评价详情页面
        /// </summary>
        /// <param name="courseRoomRuleId">新员工课程教室分配规则ID</param>
        /// <returns></returns>
        public ActionResult NewEvaluateUserToTeacherDetail(int courseRoomRuleId)
        {
            
            //var list=NewCourseRoomRuleBl.GetNew_CourseRoomRuleListByCourseId(courseId, string.Format(" TeacherId={0} ", CurrentUser.UserId));
            List<AfterCourceDetail> teacheracdlist;
            List<AfterCourceDetail> courseacdlist;
            var courseRoomRuleDetail = GetNewEvaluateUserToTeacherDetail(courseRoomRuleId, out teacheracdlist, out courseacdlist);
            ViewBag.teacheracdlist = teacheracdlist;
            ViewBag.courseacdlist = courseacdlist;
            return View(courseRoomRuleDetail);
        }

        /// <summary>
        /// 获取学员对讲师的评价详情
        /// </summary>
        /// <param name="courseRoomRuleId">新员工课程教室分配规则ID</param>
        /// <param name="teacheracdlist">讲师评估列表</param>
        /// <param name="courseacdlist">课程评估列表</param>
        /// <returns></returns>
        public New_CourseRoomRule GetNewEvaluateUserToTeacherDetail(int courseRoomRuleId,
                out List<AfterCourceDetail> teacheracdlist, out List<AfterCourceDetail> courseacdlist, string keywords="")
        {
            var courseRoomRuleDetail = NewCourseRoomRuleBl.GetCourseRoomRuleEvaluate(courseRoomRuleId);
            teacheracdlist = new List<AfterCourceDetail>();
            courseacdlist = new List<AfterCourceDetail>();
            if (courseRoomRuleDetail.PingId > 0)
            {
                List<AfterCourceDetail> acdlist = null;
                var surveyQuestionList = SurveyQuestionBl.GetSurvey_QuestionByExampaperID(courseRoomRuleDetail.PingId);
                acdlist = surveyQuestionList.Select(c => new AfterCourceDetail
                {
                    sq = c,
                    num = SurveyQuestionBl.GetNewSurveyQuestionAvg(courseRoomRuleDetail.CourseId, c.QuestionID, c.ExampaperID, courseRoomRuleDetail.Id, c.QuestionType),
                    
                    srlist = SurveyReplyAnswerBl.GetSurvey_ReplyAnswerListAndUser(
                        string.Format(@" ObjectID={0} and ExampaperID={1} and QuestionID={2} and CourseRoomRuleId={3} 
                                       and SourceFrom=0 and Survey_ReplyAnswer.Status=1 and (SubjectiveAnswer like '%" + keywords + "%' or QuestionReply like '%" + keywords + "%')"
                                       , courseRoomRuleDetail.CourseId, c.ExampaperID, c.QuestionID, courseRoomRuleDetail.Id))
                 
                }).ToList();
                teacheracdlist = acdlist.Where(p => p.sq.ObjectType == 1).ToList();//讲师评估列表
                courseacdlist = acdlist.Where(p => p.sq.ObjectType == 0).ToList();//课程评估列表
            }
            return courseRoomRuleDetail;
        }

        #endregion

        #region == 方法 ==

        /// <summary>
        /// 讲师端-学员对讲师的评价列表
        /// <param name="courseName">课程名称</param>
        /// <param name="startTime">课程开始时间-开始</param>
        /// <param name="endTime">课程开始时间-结束</param>
        /// <param name="type">课程形式：0-集中课程 1-分组带教 -1-全部</param>
        /// <param name="listFlag">列表标识：0-只显示我个人的评价列表； 1-显示所有讲师的评价列表</param>
        /// </summary>
        [ValidateInput(false)]
        public JsonResult GetNewEvaluateUserToTeacherList(string courseName, string startTime, string endTime, int type = -1, int listFlag = 0,
                int pageSize = int.MaxValue, int pageIndex = 1, string jsRenderSortField = " StartTime desc ")
        {
            try
            {
                if (Session["Newnetpage"] != null && listFlag == 1)
                {
                    Session.Remove("Newnetpage");
                }
                Session["Newnetpage"] = pageIndex + "㉿" + courseName + "㉿" + startTime + "㉿" + endTime + "㉿" + type + "㉿" + listFlag + "㉿" + jsRenderSortField;

                string where = "";
                if (listFlag == 0)
                {
                    where += string.Format("  AND crr.TeacherId={0} ", CurrentUser.UserId);
                }

                if (!string.IsNullOrWhiteSpace(courseName))
                {
                    where += string.Format(" and c.CourseName like '%{0}%' ", courseName.Trim().ReplaceSql());
                }
                if (type != -1)
                {
                    where += string.Format(" and crr.Type={0} ", type);
                }
                if (!string.IsNullOrWhiteSpace(startTime))
                {
                    where += string.Format(" and crr.StartTime>='{0}' ", startTime);
                }
                if (!string.IsNullOrWhiteSpace(endTime))
                {
                    where += string.Format(" and crr.StartTime<='{0}' ", Convert.ToDateTime(endTime).AddDays(1).AddSeconds(-1));
                }
                int totalCount = 0;
                List<New_CourseRoomRule> list = NewCourseBl.GetPingByUserToTeacherList(out totalCount, where, pageIndex, pageSize, " order by " + jsRenderSortField);
                return Json(new
                {
                    dataList = list,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<New_CourseRoomRule>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 讲师端-学员对讲师的评价详情页面列表(暂时废弃的代码)
        ///  <param name="exampaperId">评估ID</param>
        /// <param name="courseRoomRuleId">课程教室分配规则</param>
        /// </summary>
        [ValidateInput(false)]
        public JsonResult GetNewEvaluateUserToTeacherDetailList(int exampaperId, int courseRoomRuleId, int pageSize = int.MaxValue, int pageIndex = 1)
        {
            try
            {
                string where = string.Format(@" AND ra.ExampaperID={0}  AND ra.CourseRoomRuleId={1} AND crr.TeacherId={2}   "
                                     , exampaperId, courseRoomRuleId, CurrentUser.UserId);
                int totalCount = 0;
                List<Survey_ReplyAnswer> list = NewCourseRoomRuleBl.GetEvaluateUserToTeacherById(out totalCount, where, pageIndex, pageSize);
                return Json(new
                {
                    dataList = list,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new List<New_Course>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region == 导出Excel ==

        /// <summary>
        /// 导出execl
        /// </summary>
        /// <param name="courseRoomRuleId">新员工课程教室分配规则ID</param>
        public void ExportNewEvaluateUserToTeacherDetail(int courseRoomRuleId)
        {
            var dt = new DataTable();
            var dtTeacher = new DataTable();
            List<AfterCourceDetail> teacheracdlist;
            List<AfterCourceDetail> courseacdlist;
            var courseRoomRuleDetail = GetNewEvaluateUserToTeacherDetail(courseRoomRuleId, out teacheracdlist,
                                                                         out courseacdlist);

            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("评价内容 ", typeof(string));
            dt.Columns.Add("平均分 ", typeof(string));
            dt.Columns.Add("评语 ", typeof(string));
            int num = 1;

            if (courseacdlist != null) //课程评价列表
            {
                foreach (var item in courseacdlist.OrderByDescending(p => p.sq.QuestionType).ToList())
                {
                    if (item.sq.QuestionType == 3)
                    {
                        if (item.srlist.Count == 0)
                        {
                            dt.Rows.Add(Convert.ToString(num), item.sq.QuestionContent,
                                                    item.num.ToString("f2"), "--");
                            num++;
                        }
                        else
                        {
                            int i = 1;
                            var zifuchuanList = new List<string>();
                            foreach (var t in item.srlist)
                            {
                                if (!string.IsNullOrWhiteSpace(t.QuestionReply))
                                {
                                    string zifuchuan = i + "." + t.QuestionReply + "-" + t.Realname + " (" + t.DeptName + ")";
                                    zifuchuanList.Add(zifuchuan);
                                    i++;
                                }
                            }
                            if (zifuchuanList.Count == 0)
                            {
                                dt.Rows.Add(Convert.ToString(num), item.sq.QuestionContent,
                                            item.num.ToString("f2"), "--");
                                num++;
                            }
                            else
                            {
                                int no = 1;
                                foreach (var zfc in zifuchuanList)
                                {
                                    if (no == 1)
                                    {
                                        dt.Rows.Add(Convert.ToString(num), item.sq.QuestionContent,
                                                    item.num.ToString("f2"), zfc);
                                        num++;
                                    }
                                    else
                                    {
                                        dt.Rows.Add("", "", "", zfc);
                                    }
                                    no++;
                                }
                            }
                        }
                    }
                    else if (item.sq.QuestionType == 2)
                    {
                        if (item.srlist.Count == 0)
                        {
                            dt.Rows.Add(Convert.ToString(num), item.sq.QuestionContent,
                                                    "--", "--");
                            num++;
                        }
                        else
                        {
                            int i = 1;
                            var zifuchuanList = new List<string>();
                            foreach (var t in item.srlist)
                            {
                                if (!string.IsNullOrWhiteSpace(t.SubjectiveAnswer))
                                {
                                    string zifuchuan = i + "." + t.SubjectiveAnswer + "-" + t.Realname + " (" + t.DeptName + ")";
                                    zifuchuanList.Add(zifuchuan);
                                    i++;
                                }
                            }
                            if (zifuchuanList.Count == 0)
                            {
                                dt.Rows.Add(Convert.ToString(num), item.sq.QuestionContent,
                                             "--", "--");
                                num++;
                            }
                            else
                            {
                                int no = 1;
                                foreach (var zfc in zifuchuanList)
                                {
                                    if (no == 1)
                                    {
                                        dt.Rows.Add(Convert.ToString(num), item.sq.QuestionContent,
                                                     "--", zfc);
                                        num++;
                                    }
                                    else
                                    {
                                        dt.Rows.Add("", "", "", zfc);
                                    }
                                    no++;
                                }
                            }
                        }
                    }
                }
            }

            /*------------------------------------------------------------------------------------------------*/

            dtTeacher.Columns.Add("序号", typeof(string));
            dtTeacher.Columns.Add("评价内容 ", typeof(string));
            dtTeacher.Columns.Add("平均分 ", typeof(string));
            dtTeacher.Columns.Add("评语 ", typeof(string));
            num = 1;
            if (teacheracdlist != null)//讲师评价列表
            {
                foreach (var item in teacheracdlist.OrderByDescending(p => p.sq.QuestionType).ToList())
                {
                    if (item.sq.QuestionType == 3)
                    {
                        if (item.srlist.Count == 0)
                        {
                            dtTeacher.Rows.Add(Convert.ToString(num), item.sq.QuestionContent,
                                                    item.num.ToString("f2"), "--");
                            num++;
                        }
                        else
                        {
                            int i = 1;
                            var zifuchuanList = new List<string>();
                            foreach (var t in item.srlist)
                            {
                                if (!string.IsNullOrWhiteSpace(t.QuestionReply))
                                {
                                    string zifuchuan = i + "." + t.QuestionReply + "-" + t.Realname + " (" + t.DeptName + ")";
                                    zifuchuanList.Add(zifuchuan);
                                    i++;
                                }
                            }
                            if (zifuchuanList.Count == 0)
                            {
                                dtTeacher.Rows.Add(Convert.ToString(num), item.sq.QuestionContent,
                                            item.num.ToString("f2"), "--");
                                num++;
                            }
                            else
                            {
                                int no = 1;
                                foreach (var zfc in zifuchuanList)
                                {
                                    if (no == 1)
                                    {
                                        dtTeacher.Rows.Add(Convert.ToString(num), item.sq.QuestionContent,
                                                    item.num.ToString("f2"), zfc);
                                        num++;
                                    }
                                    else
                                    {
                                        dtTeacher.Rows.Add("", "", "", zfc);
                                    }
                                    no++;
                                }
                            }
                        }
                    }
                    else if (item.sq.QuestionType == 2)
                    {
                        if (item.srlist.Count == 0)
                        {
                            dtTeacher.Rows.Add(Convert.ToString(num), item.sq.QuestionContent,
                                                    "--", "--");
                            num++;
                        }
                        else
                        {
                            int i = 1;
                            var zifuchuanList = new List<string>();
                            foreach (var t in item.srlist)
                            {
                                if (!string.IsNullOrWhiteSpace(t.SubjectiveAnswer))
                                {
                                    string zifuchuan = i + "." + t.SubjectiveAnswer + "-" + t.Realname + " (" + t.DeptName + ")";
                                    zifuchuanList.Add(zifuchuan);
                                    i++;
                                }
                            }
                            if (zifuchuanList.Count == 0)
                            {
                                dtTeacher.Rows.Add(Convert.ToString(num), item.sq.QuestionContent,
                                             "--", "--");
                                num++;
                            }
                            else
                            {
                                int no = 1;
                                foreach (var zfc in zifuchuanList)
                                {
                                    if (no == 1)
                                    {
                                        dtTeacher.Rows.Add(Convert.ToString(num), item.sq.QuestionContent,
                                                     "--", zfc);
                                        num++;
                                    }
                                    else
                                    {
                                        dtTeacher.Rows.Add("", "", "", zfc);
                                    }
                                    no++;
                                }
                            }
                        }
                    }

                }
            }
            var first = new DataTable();
            first.Columns.Add("课程名称：");
            first.Columns.Add(courseRoomRuleDetail.CourseName + "(" + courseRoomRuleDetail.TypeStr + ")",
                              typeof(string));

            var second = new DataTable();
            second.Columns.Add("开始时间：" + courseRoomRuleDetail.StartTimeStr, typeof(string));
            second.Columns.Add("结束时间：" + courseRoomRuleDetail.EndTimeStr, typeof(string));
            second.Columns.Add("讲师：" + courseRoomRuleDetail.teachername, typeof(string));
            second.Columns.Add("教室：" + courseRoomRuleDetail.RoomName, typeof(string));

            var third = new DataTable();
            third.Columns.Add("课程平均分：" + courseRoomRuleDetail.CourseAvgStr, typeof(string));
            third.Columns.Add("讲师平均分：" + courseRoomRuleDetail.CourseTeacherAvgStr, typeof(string));
            third.Columns.Add("课程评估综合平均分：" + courseRoomRuleDetail.TotalCourseAvgStr, typeof(string));

            var fourth = new DataTable();
            fourth.Columns.Add("对《" + courseRoomRuleDetail.CourseName + "》课程的评价");


            var fifth = new DataTable();
            fifth.Columns.Add("对" + courseRoomRuleDetail.teachername + "讲师的评价", typeof(string));

            var dtList = new List<DataTable>();
            const string strFileName = "课程评价汇总";
            dtList.Add(first);
            dtList.Add(second);
            dtList.Add(third);
            dtList.Add(fourth);
            dtList.Add(dt);
            dtList.Add(fifth);
            dtList.Add(dtTeacher);
            var export = new Spreadsheet();
            export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, strFileName);

        }

        #endregion
    }
}