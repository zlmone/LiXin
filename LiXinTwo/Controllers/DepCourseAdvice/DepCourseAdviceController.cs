using LiXinCommon;
using LiXinInterface.DepCourseLearn;
using LiXinInterface.DepCourseManage;
using LiXinInterface.DeptSurvey;
using LiXinModels.DepCourseLearn;
using LiXinModels.DepCourseManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;


namespace LiXinControllers.DepCourseAdvice
{
    public partial class DepCourseAdviceController : BaseController
    {


        protected IDep_Course _IDep_CourseBL;
        protected IDeptSurveyExampaper _IDepSurveyQuestionBL;
        protected IDepSurveyReplyAnswer _IDepSurveyReplyAnswerBL;
        protected IDep_CourseOrder _IDep_CourseOrderBL;


        protected IDep_Attendce depAttBL;
        protected IDepCourseAdvice ClCourseAdviceBL;

        public DepCourseAdviceController(IDep_Attendce _depAttBL, IDepCourseAdvice _ClCourseAdviceBL, IDep_Course IDep_CourseBL, IDeptSurveyExampaper IDepSurveyQuestionBL,
            IDepSurveyReplyAnswer IDepSurveyReplyAnswerBL, IDep_CourseOrder IDep_CourseOrderBL)
        {

            _IDep_CourseBL = IDep_CourseBL;
            _IDepSurveyQuestionBL = IDepSurveyQuestionBL;
            _IDepSurveyReplyAnswerBL = IDepSurveyReplyAnswerBL;
            _IDep_CourseOrderBL = IDep_CourseOrderBL;

            depAttBL = _depAttBL;
            ClCourseAdviceBL = _ClCourseAdviceBL;
        }



        public ActionResult AfterCourse(int courseid, int userid, string backurl)
        {
            var course = _IDep_CourseBL.GetCo_Course(courseid);
            ViewBag.course = course;
            ViewBag.userid = userid;

            var CourseOrder = _IDep_CourseOrderBL.Get(courseid);
            ViewBag.CourseOrder = CourseOrder;

            ViewBag.backurl = backurl;

            //var courselist = _IDep_CourseBL.GetVideoCo_CourseById(courseid);

            if (!string.IsNullOrEmpty(course.SurveyPaperId) && course.IsPing == 1)
            {
                var arr = course.SurveyPaperId.Split(';');
                if (arr[0] != "")
                {
                    var examPaper = _IDepSurveyQuestionBL.GetSurveyExampaper(Convert.ToInt32(arr[0].Split(',')[1]));
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
                    var TeacherexamPaper = _IDepSurveyQuestionBL.GetSurveyExampaper(Convert.ToInt32(arr[1].Split(',')[1]));
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
                ViewBag.SurveyPaperId = course.SurveyPaperId;
            }
            ViewBag.show = 0;
            return View();
        }

        public ActionResult FAfterCourse(int courseid, int userid, string backurl)
        {
            var course = _IDep_CourseBL.GetCo_Course(courseid);
            ViewBag.course = course;
            ViewBag.userid = userid;

            var CourseOrder = _IDep_CourseOrderBL.Get(courseid);
            ViewBag.CourseOrder = CourseOrder;

            ViewBag.backurl = backurl;

            //var courselist = _IDep_CourseBL.GetVideoCo_CourseById(courseid);

            if (!string.IsNullOrEmpty(course.SurveyPaperId) && course.IsPing == 1)
            {
                var arr = course.SurveyPaperId.Split(';');
                if (arr[0] != "")
                {
                    var examPaper = _IDepSurveyQuestionBL.GetSurveyExampaper(Convert.ToInt32(arr[0].Split(',')[1]));
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
                    var TeacherexamPaper = _IDepSurveyQuestionBL.GetSurveyExampaper(Convert.ToInt32(arr[1].Split(',')[1]));
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
                ViewBag.SurveyPaperId = course.SurveyPaperId;
            }
            ViewBag.show = 1;
            return View("AfterCourse");
        }

        public ActionResult AfterCourseMain(int id, string backURL = "")
        {
            var course = _IDep_CourseBL.GetCo_Course(id);
            ViewBag.show = 0;
            ViewBag.course = course;
            ViewBag.backURL = backURL;
            if (course.SurveyPaperId != "")
            {
                if (course.SurveyPaperId.Split(';')[0] != "")
                {
                    int shijuanid = Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]);

                    List<AfterCourceDetail> acdlist = null;

                    var SurveyQuestionList = _IDepSurveyQuestionBL.GetSurvey_QuestionByExampaperID(shijuanid);

                    acdlist = SurveyQuestionList.Select(c => new AfterCourceDetail
                    {
                        sq = c,
                        num = _IDepSurveyQuestionBL.GetSurvey_QuestionAvg(id, c.QuestionID, c.ExampaperID, c.QuestionType),
                        srlist = _IDepSurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + id + " and ExampaperID=" + c.ExampaperID + " and QuestionID=" + c.QuestionID + " and SourceFrom=0 and Dep_Survey_ReplyAnswer.Status=1 ")

                    }).ToList();

                    ViewBag.acdlist = acdlist;
                    if ((acdlist.Where(p => p.sq.QuestionType == 3)).Count() != 0)
                    {
                        ViewBag.courseAvg = (acdlist.Sum(p => p.num) / (acdlist.Where(p => p.sq.QuestionType == 3)).Count()).ToString("f2");
                    }
                    else
                    {
                        ViewBag.courseAvg = 0;
                    }
                    // var a = acdlist.Sum(p => p.num);
                    //var count = acdlist.Where(p => p.sq.QuestionType == 3).Count();
                    //ViewBag.courseAvg = (a/count).ToString("f2");
                }
                if (course.SurveyPaperId.Split(';')[1] != "")
                {
                    int shijuanid = Convert.ToInt32(course.SurveyPaperId.Split(';')[1].Split(',')[1]);

                    List<AfterCourceDetail> acdlist = null;

                    var SurveyQuestionList = _IDepSurveyQuestionBL.GetSurvey_QuestionByExampaperID(shijuanid);


                    acdlist = SurveyQuestionList.Select(c => new AfterCourceDetail
                    {
                        sq = c,
                        num = _IDepSurveyQuestionBL.GetSurvey_QuestionAvg(id, c.QuestionID, c.ExampaperID, c.QuestionType),
                        srlist = _IDepSurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + id + " and ExampaperID=" + c.ExampaperID + " and QuestionID=" + c.QuestionID + " and SourceFrom=0 and Dep_Survey_ReplyAnswer.Status=1 ")


                    }).ToList();

                    ViewBag.teacheracdlist = acdlist;
                    if ((acdlist.Where(p => p.sq.QuestionType == 3).Count()) != 0)
                    {
                        ViewBag.teacherAvg = (acdlist.Sum(p => p.num) / (acdlist.Where(p => p.sq.QuestionType == 3)).Count()).ToString("f2");
                    }
                    else
                    {
                        ViewBag.teacherAvg = 0;
                    }
                }

            }
            return View();
        }

        public ActionResult FAfterCourseMain(int id, string backURL = "")
        {
            var course = _IDep_CourseBL.GetCo_Course(id);
            ViewBag.show = 1;
            ViewBag.course = course;
            ViewBag.backURL = backURL;
            if (course.SurveyPaperId != "")
            {
                if (course.SurveyPaperId.Split(';')[0] != "")
                {
                    int shijuanid = Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]);

                    List<AfterCourceDetail> acdlist = null;

                    var SurveyQuestionList = _IDepSurveyQuestionBL.GetSurvey_QuestionByExampaperID(shijuanid);

                    acdlist = SurveyQuestionList.Select(c => new AfterCourceDetail
                    {
                        sq = c,
                        num = _IDepSurveyQuestionBL.GetSurvey_QuestionAvg(id, c.QuestionID, c.ExampaperID, c.QuestionType),
                        srlist = _IDepSurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + id + " and ExampaperID=" + c.ExampaperID + " and QuestionID=" + c.QuestionID + " and SourceFrom=0 and Dep_Survey_ReplyAnswer.Status=1 ")

                    }).ToList();

                    ViewBag.acdlist = acdlist;
                    if ((acdlist.Where(p => p.sq.QuestionType == 3)).Count() != 0)
                    {
                        ViewBag.courseAvg = (acdlist.Sum(p => p.num) / (acdlist.Where(p => p.sq.QuestionType == 3)).Count()).ToString("f2");
                    }
                    else
                    {
                        ViewBag.courseAvg = 0;
                    }
                    // var a = acdlist.Sum(p => p.num);
                    //var count = acdlist.Where(p => p.sq.QuestionType == 3).Count();
                    //ViewBag.courseAvg = (a/count).ToString("f2");
                }
                if (course.SurveyPaperId.Split(';')[1] != "")
                {
                    int shijuanid = Convert.ToInt32(course.SurveyPaperId.Split(';')[1].Split(',')[1]);

                    List<AfterCourceDetail> acdlist = null;

                    var SurveyQuestionList = _IDepSurveyQuestionBL.GetSurvey_QuestionByExampaperID(shijuanid);


                    acdlist = SurveyQuestionList.Select(c => new AfterCourceDetail
                    {
                        sq = c,
                        num = _IDepSurveyQuestionBL.GetSurvey_QuestionAvg(id, c.QuestionID, c.ExampaperID, c.QuestionType),
                        srlist = _IDepSurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + id + " and ExampaperID=" + c.ExampaperID + " and QuestionID=" + c.QuestionID + " and SourceFrom=0 and Dep_Survey_ReplyAnswer.Status=1 ")


                    }).ToList();

                    ViewBag.teacheracdlist = acdlist;
                    if ((acdlist.Where(p => p.sq.QuestionType == 3).Count()) != 0)
                    {
                        ViewBag.teacherAvg = (acdlist.Sum(p => p.num) / (acdlist.Where(p => p.sq.QuestionType == 3)).Count()).ToString("f2");
                    }
                    else
                    {
                        ViewBag.teacherAvg = 0;
                    }
                }

            }
            return View("AfterCourseMain");
        }

        public ActionResult CourseAppDetial()
        {
            ViewBag.show = 0;
            return View();
        }

        public ActionResult FCourseAppDetial()
        {
            ViewBag.show = 1;
            return View("CourseAppDetial");
        }

        public ActionResult CourseIndex(string p)
        {
            ViewBag.Departments = GetAllSubDepartments();
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

        #region 倒出execl
        /// <summary>
        /// 导出execl
        /// </summary>
        /// <param name="id"></param>
        public void traceExecl(int id)
        {
            var course = _IDep_CourseBL.GetCo_Course(id);

            var dt = new DataTable();
            var dtTeacher = new DataTable();
            var courseAvg = "0";
            var teacherAvg = "0";

            if (course.SurveyPaperId != "")
            {
                if (course.SurveyPaperId.Split(';')[0] != "")
                {
                    int shijuanid = Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]);

                    List<AfterCourceDetail> acdlist = null;

                    var SurveyQuestionList = _IDepSurveyQuestionBL.GetSurvey_QuestionByExampaperID(shijuanid);

                    acdlist = SurveyQuestionList.Select(c => new AfterCourceDetail
                    {
                        sq = c,
                        num = _IDepSurveyQuestionBL.GetSurvey_QuestionAvg(id, c.QuestionID, c.ExampaperID, c.QuestionType),
                        srlist = _IDepSurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + id + " and ExampaperID=" + c.ExampaperID + " and QuestionID=" + c.QuestionID + " and SourceFrom=0 and Dep_Survey_ReplyAnswer.Status=1 ")

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
                    var teacherSurveyQuestionList = _IDepSurveyQuestionBL.GetSurvey_QuestionByExampaperID(teachershijuanid);

                    Teacheracdlist = teacherSurveyQuestionList.Select(c => new AfterCourceDetail
                    {
                        sq = c,
                        num = _IDepSurveyQuestionBL.GetSurvey_QuestionAvg(id, c.QuestionID, c.ExampaperID, c.QuestionType),
                        srlist = _IDepSurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + id + " and ExampaperID=" + c.ExampaperID + " and QuestionID=" + c.QuestionID + " and SourceFrom=0 and Dep_Survey_ReplyAnswer.Status=1 ")
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

        #region 评估列表
        /// <summary>
        /// 评估列表
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="IsMust"></param>
        /// <param name="Sort"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="jsRenderSortField"></param>
        /// <returns></returns>
        public JsonResult GetCourseIndex(string courseName, string StartTime, string EndTime, string IsMust, string Sort = "(1,3)", int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Id asc", int depId = 0)
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
                strWhere.AppendFormat(" IsPing=1 AND Publishflag=1 AND IsDelete=0 and DeptId={0}", depId);
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
                if (!string.IsNullOrEmpty(Sort))
                {
                    strWhere.AppendFormat(" AND Way in {0}", "(1,3)");
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
                List<Dep_Course> listCourse = _IDep_CourseBL.GetCourseIndexList(out totalcount, strWhere.ToString(), pageIndex, pageSize, jsRenderSortField);
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
        #endregion

        #region
        public JsonResult GetCourseAvg(int CourseID, int pageSize = 20, int pageIndex = 1)
        {
            try
            {
                int totalcount = 0;
                List<Dep_Course> listCourse = _IDep_CourseBL.GetCourseAvg(out totalcount, CourseID, pageIndex, pageSize, "tab.UserID asc");
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



        #region 页面显示
        public ActionResult DepCourseAdviceList()
        {
            ViewBag.Departments = GetAllSubDepartments();
            return View();
        }

        public ActionResult DepCourseAdviceDetail()
        {
            string id = Request.QueryString["id"];
            ViewBag.Id = id;
            return View();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <param name="CourseName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="IsMust"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetAllList(string CourseName, string StartTime, string EndTime, string IsMust, string jsrendersortfield, int pageSize = 20, int pageIndex = 1, int depId = 0)
        {
            int totalCount = 0;
            string where = string.Format(" 1=1 ");
            if (!string.IsNullOrEmpty(CourseName))
            {
                where += string.Format(" AND temp.CourseName like '%{0}%' ", CourseName.ReplaceSql());
            }
            if (!string.IsNullOrEmpty(IsMust))
            {
                if (!IsMust.Contains(","))
                {
                    where += string.Format(" AND temp.IsMust={0}", Convert.ToInt32(IsMust));
                }
            }
            if (!string.IsNullOrEmpty(StartTime))
            {
                where += string.Format(" and CONVERT(datetime,temp.StartTime,23) >= '{0}'", StartTime);
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                where += string.Format(" and CONVERT(datetime,temp.EndTime,23) <= '{0}'", EndTime);
            }
            var deptattendceList = depAttBL.GetDepAttendceListForAdvice(depId.ToString(), out totalCount, pageIndex, pageSize, jsrendersortfield, where);
            foreach (var item in deptattendceList)
            {
                item.CourseName = item.CourseName.HtmlXssEncode();
                item.RoomName = item.RoomName.HtmlXssEncode();
            }
            return Json(new
            {
                dataList = deptattendceList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OnLoadCl_CourseAdvice(int id)
        {
            List<Dep_CourseAdvice> clCourseAdvicesList = ClCourseAdviceBL.GetList(" Dep_CourseAdvice.CourseId=" + id);
            string html = "";
            foreach (var clCourseAdvice in clCourseAdvicesList)
            {
                html += "<div id=" + clCourseAdvice.Id + " class='list'>";
                html += "<div class='list-head'><span>" + clCourseAdvice.userRealname
                        + "</span><img src='" + Url.Content(clCourseAdvice.userPhotoUrlStr) + "' title='" + clCourseAdvice.userRealname + "' /></div>";
                html += "   <div class='list-info'><i></i>";
                html += "       <div class='list-type'><strong>课前建议：</strong>" + clCourseAdvice.AdviceTime + "";

                //if (clCourseAdvice.AnserContent == "")
                //{
                //    html += "       <a class='fr btn bt-co p_Reply' onclick='FReply(this)'  type='1'>反馈</a>";
                //}
                html += "</div>";
                html += "       <div class='list-con'>" + clCourseAdvice.AdviceContent + "</div>";

                if (clCourseAdvice.AnserContent != "")
                {
                    html += "<div class='list-te'>";
                    html += "<p><strong class='c_col'>讲师<span>" + clCourseAdvice.TeacherName + "</span>的反馈内容：</strong>" + clCourseAdvice.AnserContent + "</p>";
                    html += "<p class='time'>时间:" + clCourseAdvice.AnserTime + "</p>";
                    html += "</div>";
                }
                else
                {
                    html += "<div style='display:none'  id='Reply_div' class='pro-back'><table class='tab-Form'>";
                    html += "   <tr><td class='Tit'>请选择分享范围：</td><td><div class='sel'><input type='radio' name='tt' value='1' /><label>仅提出人</label><input type='radio' name='tt' value='2' /><label>全员</label></div></td></tr>";
                    html += "   <tr><td class='Tit'></td><td><input type=\"text\" id=\"txt_AnswerContent\" name=\"content\"  class='all80' maxlength='200'/></td></tr>";
                    html += "   <tr><td class='Tit'></td><td><input type=\"button\" value='提交'  class='btn btn-co' onclick='FAnswer(this)'/></td></tr>";
                    html += "</table></div>";
                }

                html += "   </div></div>";
            }

            return Json(new
            {
                content = html

            }, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}
