using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using LiXinInterface.CourseManage;
using LiXinModels.CourseManage;
using LiXinCommon;
using LiXinInterface.Survey;
using LiXinModels.Survey;
using System.Linq;
using LiXinModels.User;
using System.Data;
using LiXinModels;
using System.Text.RegularExpressions;

namespace LiXinControllers
{
    public partial class CourseManageController : BaseController
    {
        #region 呈现
        public ActionResult CourseIndex(string p)
        {
            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.AttSort = "1";
            if (p != null && p != "" && p == "1")
            {
                if (Session["cipage"] != null)
                {
                    string sess = Session["cipage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Attname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.AttstartTime = att[2];
                    ViewBag.AttendTime = att[3];
                    ViewBag.Attmust = att[4];
                    ViewBag.AttSort = att[5];
                }
            }
            return View();
        }

        public ActionResult CourseAppDetial()
        {
            return View();
        }

        /// <summary>
        /// AfterCourseMain课后评估页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AfterCourseMain(int id)
        {
            var course = _courseBL.GetCo_Course(id);
            ViewBag.course = course;

            if (course.SurveyPaperId != "")
            {
                if (course.SurveyPaperId.Split(';')[0] != "")
                {
                    int shijuanid = Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]);

                    List<AfterCourceDetail> acdlist = null;

                    var SurveyQuestionList = _SurveyQuestionBL.GetSurvey_QuestionByExampaperID(shijuanid);

                    acdlist = SurveyQuestionList.Select(c => new AfterCourceDetail
                    {
                        sq = c,
                        num = _SurveyQuestionBL.GetSurvey_QuestionAvg(id, c.QuestionID, c.ExampaperID, c.QuestionType),
                        srlist = _ISurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + id + " and ExampaperID=" + c.ExampaperID + " and QuestionID=" + c.QuestionID + " and SourceFrom=0 and Survey_ReplyAnswer.Status=1 ")

                    }).ToList();

                    ViewBag.acdlist = acdlist;
                    ViewBag.courseAvg = (acdlist.Sum(p => p.num) / (acdlist.Where(p => p.sq.QuestionType == 3)).Count()).ToString("f2");
                    // var a = acdlist.Sum(p => p.num);
                    //var count = acdlist.Where(p => p.sq.QuestionType == 3).Count();
                    //ViewBag.courseAvg = (a/count).ToString("f2");
                }
                if (course.SurveyPaperId.Split(';')[1] != "")
                {
                    int shijuanid = Convert.ToInt32(course.SurveyPaperId.Split(';')[1].Split(',')[1]);

                    List<AfterCourceDetail> acdlist = null;

                    var SurveyQuestionList = _SurveyQuestionBL.GetSurvey_QuestionByExampaperID(shijuanid);


                    acdlist = SurveyQuestionList.Select(c => new AfterCourceDetail
                    {
                        sq = c,
                        num = _SurveyQuestionBL.GetSurvey_QuestionAvg(id, c.QuestionID, c.ExampaperID, c.QuestionType),
                        srlist = _ISurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + id + " and ExampaperID=" + c.ExampaperID + " and QuestionID=" + c.QuestionID + " and SourceFrom=0 and Survey_ReplyAnswer.Status=1 ")


                    }).ToList();

                    ViewBag.teacheracdlist = acdlist;
                    ViewBag.teacherAvg = (acdlist.Sum(p => p.num) / (acdlist.Where(p => p.sq.QuestionType == 3)).Count()).ToString("f2");
                }

            }


            return View();
        }

        public JsonResult FBindSurvey_ReplyAnswer(int Courseid, int userid, string SurveyPaperId)
        {
            string html = "";
            string teacherhtml = "";
            int status = 0;
            int teacherstatus = 0;
            if (SurveyPaperId != "")
            {
                if (SurveyPaperId.Split(';')[0] != "")
                {
                    var ReplyAnswer =
               _ISurveyReplyAnswerBL.GetList(" ObjectID=" + Courseid + " and ExampaperID=" + SurveyPaperId.Split(';')[0].Split(',')[1] +
                                            " and UserID=" + userid + " and Status=1");
                    if (ReplyAnswer.Count != 0)
                    {

                        foreach (Survey_ReplyAnswer surveyReplyAnswer in ReplyAnswer)
                        {
                            //html += surveyReplyAnswer.SubjectiveAnswer + surveyReplyAnswer.ObjectiveAnswer + ";";
                            html += surveyReplyAnswer.QuestionID + "♣" + surveyReplyAnswer.SubjectiveAnswer + "♣" +
                                    surveyReplyAnswer.QuestionReply + "♥";
                            status = surveyReplyAnswer.Status;
                        }

                        html = html.Substring(0, html.LastIndexOf('♥'));
                        //return Json(new { result = 1, content = html, status = status }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (SurveyPaperId.Split(';')[1] != "")//绑定对讲师的评估
                {
                    var ReplyAnswer =
              _ISurveyReplyAnswerBL.GetList(" ObjectID=" + Courseid + " and ExampaperID=" + SurveyPaperId.Split(';')[1].Split(',')[1] +
                                           " and UserID=" + userid + " and Status=1");
                    if (ReplyAnswer.Count != 0)
                    {

                        foreach (Survey_ReplyAnswer surveyReplyAnswer in ReplyAnswer)
                        {
                            //html += surveyReplyAnswer.SubjectiveAnswer + surveyReplyAnswer.ObjectiveAnswer + ";";
                            teacherhtml += surveyReplyAnswer.QuestionID + "♣" + surveyReplyAnswer.SubjectiveAnswer + "♣" +
                                    surveyReplyAnswer.QuestionReply + "♥";
                            teacherstatus = surveyReplyAnswer.Status;
                        }

                        teacherhtml = teacherhtml.Substring(0, teacherhtml.LastIndexOf('♥'));
                        //return Json(new { result = 1, content = html, status = status }, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            return Json(new { result = 1, content = html, teachercontent = teacherhtml, status = status, teacherstatus = teacherstatus }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 倒出execl
        /// <summary>
        /// 导出execl
        /// </summary>
        /// <param name="id"></param>
        public void traceExecl(int id)
        {
            var course = _courseBL.GetCo_Course(id);

            var dt = new DataTable();
            var dtTeacher = new DataTable();
            var courseAvg="0";
            var teacherAvg="0";

            if (course.SurveyPaperId != "")
            {
                if (course.SurveyPaperId.Split(';')[0] != "")
                {
                    int shijuanid = Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]);

                    List<AfterCourceDetail> acdlist = null;

                    var SurveyQuestionList = _SurveyQuestionBL.GetSurvey_QuestionByExampaperID(shijuanid);

                    acdlist = SurveyQuestionList.Select(c => new AfterCourceDetail
                    {
                        sq = c,
                        num = _SurveyQuestionBL.GetSurvey_QuestionAvg(id, c.QuestionID, c.ExampaperID, c.QuestionType),
                        srlist = _ISurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + id + " and ExampaperID=" + c.ExampaperID + " and QuestionID=" + c.QuestionID + " and SourceFrom=0 and Survey_ReplyAnswer.Status=1 ")

                    }).ToList();

                    courseAvg = (acdlist.Sum(p => p.num) / (acdlist.Where(p => p.sq.QuestionType == 3)).Count()).ToString("f2");


                    dt.Columns.Add("序号", typeof(int));
                    dt.Columns.Add("评价内容 ", typeof(string));
                    dt.Columns.Add("平均分 ", typeof(string));
                    dt.Columns.Add("评语 ", typeof(string));
                    int num = 1;
                    string zifuchuan = "";
                    if (acdlist != null)
                    {
                        //var list = acdlist.OrderBy(p => p.sq.QuestionOrder).ToList();
                        foreach (var item in acdlist.OrderByDescending(p => p.sq.QuestionType).ToList())
                        {
                            if (item.sq.QuestionType == 3)
                            {
                                int i = 1;
                                if (item.srlist.Count != 0)
                                {
                                    foreach (var t in item.srlist)
                                    {
                                        zifuchuan += i++ + "." + "([" + t.DeptName + "]" + t.Realname + ")" + t.QuestionReply + ",";
                                    }
                                }
                                dt.Rows.Add(num++, item.sq.QuestionContent, item.num.ToString("f2"), zifuchuan == "" ? zifuchuan : zifuchuan.Substring(0, zifuchuan.LastIndexOf(',')));
                                zifuchuan = "";
                                i = 1;
                            }
                            else if (item.sq.QuestionType == 2)
                            {
                                int i = 1;
                                if (item.srlist.Count != 0)
                                {
                                    foreach (var t in item.srlist)
                                    {
                                        zifuchuan += i++ + "." + "([" + t.DeptName + "]" + t.Realname + ")" + t.SubjectiveAnswer + ",";
                                    }
                                }
                                dt.Rows.Add(num++, item.sq.QuestionContent, "--", zifuchuan == "" ? zifuchuan : zifuchuan.Substring(0, zifuchuan.LastIndexOf(',')));
                                zifuchuan = "";
                                i = 1;
                            }
                        }
                    }
                }
                /*------------------------------------------------------------------------------------------------*/

                if (course.SurveyPaperId.Split(';')[1] != "")
                {
                    int teachershijuanid = Convert.ToInt32(course.SurveyPaperId.Split(';')[1].Split(',')[1]);
                    List<AfterCourceDetail> Teacheracdlist = null;
                    var teacherSurveyQuestionList = _SurveyQuestionBL.GetSurvey_QuestionByExampaperID(teachershijuanid);

                    Teacheracdlist = teacherSurveyQuestionList.Select(c => new AfterCourceDetail
                    {
                        sq = c,
                        num = _SurveyQuestionBL.GetSurvey_QuestionAvg(id, c.QuestionID, c.ExampaperID, c.QuestionType),
                        srlist = _ISurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + id + " and ExampaperID=" + c.ExampaperID + " and QuestionID=" + c.QuestionID + " and SourceFrom=0 and Survey_ReplyAnswer.Status=1 ")
                    }).ToList();

                    teacherAvg = (Teacheracdlist.Sum(p => p.num) / (Teacheracdlist.Where(p => p.sq.QuestionType == 3)).Count()).ToString("f2");

                    dtTeacher.Columns.Add("序号", typeof(int));
                    dtTeacher.Columns.Add("评价内容 ", typeof(string));
                    dtTeacher.Columns.Add("平均分 ", typeof(string));
                    dtTeacher.Columns.Add("评语 ", typeof(string));
                    int teachernum = 1;
                    string teacherzifuchuan = "";

                    if (Teacheracdlist != null)
                    {
                        foreach (var item in Teacheracdlist.OrderByDescending(p => p.sq.QuestionType).ToList())
                        {
                            if (item.sq.QuestionType == 3)
                            {
                                int i = 1;
                                if (item.srlist.Count != 0)
                                {
                                    foreach (var t in item.srlist)
                                    {
                                        teacherzifuchuan += i++ + "." + "([" + t.DeptName + "]" + t.Realname + ")" + t.QuestionReply + ",";
                                    }
                                }
                                dtTeacher.Rows.Add(teachernum++, item.sq.QuestionContent, item.num.ToString("f2"), teacherzifuchuan == "" ? teacherzifuchuan : teacherzifuchuan.Substring(0, teacherzifuchuan.LastIndexOf(',')));
                                teacherzifuchuan = "";
                                i = 1;
                            }
                            else if (item.sq.QuestionType == 2)
                            {
                                int i = 1;
                                if (item.srlist.Count != 0)
                                {
                                    foreach (var t in item.srlist)
                                    {
                                        teacherzifuchuan += i++ + "." + "([" + t.DeptName + "]" + t.Realname + ")" + t.SubjectiveAnswer + ",";
                                    }
                                }
                                dtTeacher.Rows.Add(teachernum++, item.sq.QuestionContent, "--", teacherzifuchuan == "" ? teacherzifuchuan : teacherzifuchuan.Substring(0, teacherzifuchuan.LastIndexOf(',')));
                                teacherzifuchuan = "";
                                i = 1;
                            }

                        }
                    }
                }
                var first = new DataTable();
                first.Columns.Add("对《" + course.CourseName + "》的评价汇总");
                first.Columns.Add(" 课程平均分:" + courseAvg + " 讲师平均分:" + teacherAvg + "课程评估总平均分:" + ((Convert.ToDouble(courseAvg) + Convert.ToDouble(teacherAvg)) / 2).ToString("n2"), typeof(string));

                var second = new DataTable();
                second.Columns.Add("对" + course.TeacherName + "讲师的评估", typeof(string));

                var dtList = new List<DataTable>();
                string strFileName = "课程评估汇总";
                dtList.Add(first);
                dtList.Add(dt);
                dtList.Add(second);
                dtList.Add(dtTeacher);
                var export = new Spreadsheet();

                export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, strFileName);
            }
        }
        #endregion

        #region 客户端评估详情列表

        public ActionResult AfterCourse(int courseid, int userid, string backurl)
        {
            var course = _courseBL.GetCo_Course(courseid);
            ViewBag.course = course;
            ViewBag.userid = userid;

            var CourseOrder = _courseOrderBL.Get(courseid);
            ViewBag.CourseOrder = CourseOrder;

            ViewBag.backurl = backurl;

            var courselist = _courseBL.GetVideoCo_CourseById(courseid);

            if (!string.IsNullOrEmpty(courselist.SurveyPaperId) && course.IsPing == 1)
            {
                var arr = courselist.SurveyPaperId.Split(';');
                if (arr[0] != "")
                {
                    var examPaper = _surveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[0].Split(',')[1]));
                    if (examPaper != null)
                    {
                        ViewBag.examPaper = examPaper;
                    }
                    else
                    {
                        ViewBag.examPaper = null;
                    }
                }
                if (arr[1] != "")
                {
                    var TeacherexamPaper = _surveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[1].Split(',')[1]));
                    if (TeacherexamPaper != null)
                    {
                        ViewBag.TeacherexamPaper = TeacherexamPaper;
                    }
                    else
                    {
                        ViewBag.TeacherexamPaper = null;
                    }
                }
                ViewBag.id = courseid;
                ViewBag.SurveyPaperId = courselist.SurveyPaperId;
            }

            return View();
        }




        #endregion

        #region 操作

        public JsonResult GetCourseIndex(string courseName, string StartTime, string EndTime, string IsMust, int Sort = 1, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id asc")
        {
            try
            {
                if (Session["cipage"] != null)
                {
                    Session.Remove("cipage");
                }
                Session["cipage"] = pageIndex + "㉿" + courseName + "㉿" + StartTime + "㉿" + EndTime + "㉿" + IsMust + "㉿" + Sort;

                int totalcount = 0;
                StringBuilder strWhere = new StringBuilder();
                strWhere.Append(" IsPing=1 AND Publishflag=1 AND IsDelete=0 ");
                if (!string.IsNullOrEmpty(courseName))
                {
                    strWhere.AppendFormat(" AND CourseName like '%{0}%' ", courseName.ReplaceSql());
                }
                if (!string.IsNullOrEmpty(StartTime))
                {
                    strWhere.AppendFormat(" AND StartTime  >='{0}' ", StartTime);
                }
                if (!string.IsNullOrEmpty(EndTime))
                {
                    strWhere.AppendFormat(" AND EndTime <= '{0}' ", EndTime);
                }
                if (Sort > 0)
                {
                    strWhere.AppendFormat(" AND Way ={0}", Sort);
                }
                else
                {
                    strWhere.AppendFormat(" AND Way !=3");
                }
                if (!string.IsNullOrEmpty(IsMust))
                {
                    if (!IsMust.Contains(","))
                    {
                        strWhere.AppendFormat(" AND IsMust={0}", Convert.ToInt32(IsMust));
                    }
                }
                List<Co_Course> listCourse = _courseBL.GetCourseIndexList(out totalcount, strWhere.ToString(), pageIndex, pageSize, jsRenderSortField);
                foreach (var item in listCourse)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                }
                return Json(new
                {
                    dataList = listCourse,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCourseAvg(int CourseID, int pageSize = 20, int pageIndex = 1)
        {
            try
            {
                int totalcount = 0;
                List<Co_CourseShow> listCourse = _courseBL.GetCourseAvg(out totalcount, CourseID, pageIndex, pageSize, "tab.UserID asc");
                listCourse.ForEach(p =>
                                       {
                                           if (string.IsNullOrEmpty(p.TeacherName))
                                           {
                                               p.TeacherName = "";
                                           }
                                           if (p.SurveyPaperId != null && p.SurveyPaperId != "")
                                           {
                                               var strarr = p.SurveyPaperId.Split(';');
                                               if (strarr.Length == 2)
                                               {
                                                   if (strarr[0] == "")
                                                       p.CoAvg = -1;
                                                   if (strarr[1] == "")
                                                       p.TeacherName = "";
                                               }
                                           }
                                           else
                                           {
                                               p.CoAvg = -1;
                                               p.TeacherName = "";
                                           }
                                       });


                return Json(new
                {
                    dataList = listCourse,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }

    public class AfterCourceDetail
    {
        public Survey_Question sq;

        public decimal num;

        public List<Survey_ReplyAnswer> srlist;
    }
}
