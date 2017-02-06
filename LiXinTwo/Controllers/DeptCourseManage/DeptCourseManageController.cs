using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinBLL.CourseManage;
using LiXinBLL.DepTranManage;
using LiXinBLL.Examination;
using LiXinBLL.Survey;
using LiXinInterface.DepTranManage;
using LiXinInterface.Examination;
using LiXinInterface.Survey;
using LiXinCommon;
using System.Web.Mvc;
using System.Web;
using LiXinBLL.DeptCourseManage;
using LiXinInterface.DeptCourseManage;
using LiXinModels.DepTranManage;
using LiXinModels.DeptCourseManage;
using LiXinControllers.Filter;
using LiXinInterface.CourseManage;
using LiXinModels.CourseLearn;
using LiXinInterface.CourseLearn;
using LiXinBLL.CourseLearn;
using LiXinModels;
using LiXinModels.User;
using System.Data;


namespace LiXinControllers.DeptCourseManage
{
    public partial class DeptCourseManageController : BaseController
    {
        protected readonly IDeptCourse courseBl = new DeptCourseBL();
        protected readonly IDeptCourseResource courseResourceBl = new DeptResourceBL();
        readonly IDeptCoursePaper coursePaperBl = new DeptCoursePaperBL();
        protected IExampaper IExampaperBL = new ExampaperBL();
        protected ICo_Course CoCourseBL = new Co_CourseBL();
        protected ISurveyExampaper _surveyExampaperBL = new SurveyExampaperBL();
        protected IDepTran_DepartLeaderConfig depLConfigBL = new DepTran_DepartLeaderConfigBL();
        protected IDepTran_CourseOrder orderBl = new DepTran_CourseOrderBL();
        protected IDepTran_Attendce DAttendce = new DepTran_AttendceBL();
        protected ISurveyQuestion ISurveyQuestionBL = new SurveyQuestionBL();
        protected ISurveyReplyAnswer ISurveyReplyAnswerBL = new SurveyReplyAnswerBL();
        protected ICl_CourseAdvice ClCourseAdviceBL = new Cl_CourseAdviceBL();
        protected IDepTran_CourseOpen iDepTran_CourseOpen = new DepTran_CourseOpenBL();

        #region 页面呈现

        /// <summary>
        /// 课程预定查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CourseSubscribe()
        {
            return View();
        }

        public ActionResult CourseSubscribeDetail(int courseid, int DepartId)
        {
            ViewBag.model = CoCourseBL.GetCo_Course(courseid);
            ViewBag.DepartId = DepartId;
            return View();
        }

        /// <summary>
        /// 详情页面
        /// </summary>
        /// <param name="id">课程编号</param>
        /// <returns></returns>
        public ActionResult DeptCourseDetail(int id, string status, int deptId = 0)
        {
            Dept_Course dc = new Dept_Course();
            List<Dept_CourseResource> CourseResource = new List<Dept_CourseResource>();
            List<Dept_CourseResource> courseResourceType = new List<Dept_CourseResource>();
            Dept_CoursePaper CoursePaper = new Dept_CoursePaper();
            dc = courseBl.GetCo_Course(id, CurrentUser.UserId, deptId);
            CourseResource = courseResourceBl.GetCourseResourceList(id);
            CoursePaper = coursePaperBl.GetCo_CourseMainPaper(id);
            if (CoursePaper != null)
            {
                var exampaper = DAttendce.GetExampaper(CoursePaper.PaperId);
                var exampapger = IExampaperBL.GetExampaper(CoursePaper.PaperId);
                ViewBag.exampapger = exampapger;
                ViewBag.exampaper = exampaper;
                ViewBag.name = exampapger.ExampaperTitle;
            }
            courseResourceType = courseResourceBl.GetList(id);

            if (dc.IsPing == 1)
            {
                if (!string.IsNullOrWhiteSpace(dc.SurveyPaperId))
                {
                    var arr = dc.SurveyPaperId.Split(';');
                    if (!string.IsNullOrWhiteSpace(arr[0]))
                        dc.AfterCourseAssess =
                            _surveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[0].Split(',')[1]));
                    if (!string.IsNullOrWhiteSpace(arr[1]))
                        dc.AfterCourseTeacherExam =
                            _surveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[1].Split(',')[1]));
                }
            }

            ViewBag.status = status;
            ViewBag.CourseResource = CourseResource;
            ViewBag.CoursePaper = CoursePaper;
            ViewBag.CourseAttachList = courseResourceType;
            ViewBag.flag = 0;
            ViewBag.deptId = deptId;
            dc.LimitNumber = dc.IsOpenOrder == 0 ? dc.LimitNumber : (dc.DeptNumberLimited == 0 ? dc.LimitNumber : dc.DeptNumberLimited);
            return View(dc);
        }

        /// <summary>
        /// 主页面显示
        /// </summary>
        /// <returns></returns>
        public ActionResult DeptCourseManage()
        {
            var dc = courseBl.GetDeptId(CurrentUser.UserId) ?? new Dept_Course();
            var uid = CurrentUser.UserId;
            var depConfiglist = depLConfigBL.GetDepartSettingByUserId(uid);
            ViewBag.did = depConfiglist.Count > 0 ? depConfiglist[0].Id : 0;
            return View(dc);
        }

        public ActionResult DeptAllCourseManage()
        {
            return View();
        }

        public ActionResult DeptAllCourseDetail(int id)
        {
            var Course = CoCourseBL.GetCo_Course(id);
            //课程关联的资源
            var CourseResourse = courseResourceBl.GetCourseResourceList(id);
            //课程下面的附件
            var courseResourceType = courseResourceBl.GetList(id);

            ViewBag.CourseResourse = CourseResourse;
            ViewBag.CourseAttachList = courseResourceType;
            ViewBag.id = id;
            ViewBag.Course = Course;
            return View();
        }


        public ActionResult DeptCourse(int id, int showFlag = 0)
        {
            var Course = CoCourseBL.GetCo_Course(id);
            //还需在获取讲师信息

            ViewBag.Course = Course;

            var CourseOrder = orderBl.Get(Course.Id);
            ViewBag.CourseOrder = CourseOrder;
            ViewBag.showFlag = showFlag;

            return View();
        }

        public ActionResult Attendance(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult ClassPro(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 在线测试
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineTest(int id)
        {
            ViewBag.Id = id;
            ViewBag.CoursePaper = coursePaperBl.GetCo_CourseMainPaper(id) == null ? 0 : 1;
            return View();
        }

        public ActionResult AfterCourseNew(int id)
        {
            var course = CoCourseBL.GetCo_Course(id);

            //if (course.SurveyPaperId != "" && course.SurveyPaperId !=null)
            if (!string.IsNullOrEmpty(course.SurveyPaperId))
            {
                if (course.SurveyPaperId.Split(';')[0] != "")
                {
                    var afterquestion = ISurveyQuestionBL.GetSurvey_QuestionByExampaperID(Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]));
                    //var afterquestion = ISurveyQuestionBL.GetSurvey_QuestionByUserID(id, Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]), CurrentUser.UserId);
                    ViewBag.afterquestion = afterquestion;

                    //加载第一个问题的答案
                    var firsthtml = "";
                    if (afterquestion.Count != 0)
                    {
                        firsthtml = FindAnswerString(id, afterquestion[0].ExampaperID, afterquestion[0].QuestionType, afterquestion[0].QuestionID);
                    }

                    ViewBag.firsthtml = firsthtml;

                }
                else
                {
                    ViewBag.afterquestion = null;
                    ViewBag.firsthtml = null;
                }
                if (course.SurveyPaperId.Split(';')[1] != "")
                {
                    var afterteacherquestion = ISurveyQuestionBL.GetSurvey_QuestionByExampaperID(Convert.ToInt32(course.SurveyPaperId.Split(';')[1].Split(',')[1]));
                    //var afterteacherquestion = ISurveyQuestionBL.GetSurvey_QuestionByUserID(id, Convert.ToInt32(course.SurveyPaperId.Split(';')[1].Split(',')[1]), CurrentUser.UserId);
                    ViewBag.afterteacherquestion = afterteacherquestion;
                    var firstafterteacherhtml = "";
                    if (afterteacherquestion.Count != 0)
                    {
                        firstafterteacherhtml = FindAnswerString(id, afterteacherquestion[0].ExampaperID,
                                                                    afterteacherquestion[0].QuestionType,
                                                                    afterteacherquestion[0].QuestionID);
                    }
                    ViewBag.firstafterteacherhtml = firstafterteacherhtml;

                }
                else
                {
                    ViewBag.afterteacherquestion = null;
                }
            }

            ViewBag.id = id;
            ViewBag.SurveyPaperId = course.SurveyPaperId;

            return View();
        }

        /// <summary>
        /// 开放预订
        /// </summary>
        /// <returns></returns>
        public ActionResult OpenOrder(int courseId, string StartTime, string EndTime, int departId, int num, int type = 1)
        {
            ViewBag.courseId = courseId;
            ViewBag.StartTime = StartTime;
            ViewBag.EndTime = EndTime;
            ViewBag.departId = departId;
            ViewBag.num = num;
            ViewBag.type = type;
            return View();
        }
        #endregion


        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <returns></returns>
       // [Filter.SystemLog("获取列表  集中授课", LogLevel.Info)]
        public JsonResult GetCourseTogetherList(string courseName, string StartTime, string EndTime, string IsMust, string IsOrder, string IsCPA, int departId, int Sort = -1, string isSys = "", string isYear = "", int pageSize = 20, int pageIndex = 1, int systemId = 0, int PublishFlag = -1, string jsRenderSortField = "Code desc")
        {
            //if (Session["ctpage"] != null)
            //{
            //    Session.Remove("ctpage");
            //}
            //Session["ctpage"] = pageIndex + "㉿" + courseName + "㉿" + StartTime + "㉿" + EndTime + "㉿" + IsMust + "㉿" +
            //                    IsCPA + "㉿" + Sort + "㉿" + isSys + "㉿" + isYear + "㉿" + IsOrder;

            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(string.Format("  CourseFrom=2 AND way=1 "));
            if (!string.IsNullOrEmpty(courseName))
            {
                strWhere.AppendFormat(" AND courseName like '%{0}%' ", courseName.ReplaceSql());
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
                strWhere.AppendFormat(" AND Sort Like '%{0}%'", Sort);
            }
            if (!string.IsNullOrEmpty(IsMust))
            {
                if (!IsMust.Contains(","))
                {
                    strWhere.AppendFormat(" AND IsMust={0}", Convert.ToInt32(IsMust));
                }
            }
            if (!string.IsNullOrEmpty(IsCPA))
            {
                if (!IsCPA.Contains(","))
                {
                    strWhere.AppendFormat(" AND IsCPA={0}", Convert.ToInt32(IsCPA));
                }
            }
            if (!string.IsNullOrEmpty(isYear))
            {
                if (!isYear.Contains(","))
                {
                    strWhere.AppendFormat(" and IsYearPlan ={0}", Convert.ToInt32(isYear));
                }
            }
            if (!string.IsNullOrEmpty(isSys))
            {
                if (!isSys.Contains(","))
                {
                    if (Convert.ToInt32(isSys) == 0)
                    {
                        strWhere.AppendFormat(" and IsSystem=0 ");
                    }
                    else
                    {
                        strWhere.AppendFormat(" and IsSystem>0 ");
                    }
                }
            }

            if (!string.IsNullOrEmpty((IsOrder)))
            {
                if (!IsOrder.Contains(","))
                {
                    if (IsOrder == "1")
                    {
                        strWhere.AppendFormat(" and isnull(dtco.OpenFlag,0)!=0 ");
                    }
                    else
                    {
                        strWhere.AppendFormat(" and isnull(dtco.OpenFlag,0)=0 ");
                    }
                }
            }

            if (PublishFlag != -1)
            {
                strWhere.AppendFormat(" AND PublishFlag={0} ", PublishFlag);
            }
            List<Dept_Course> listCourse = new List<Dept_Course>();

            int totalCount = 0;
            listCourse = courseBl.GetCourseTogetherList(out totalCount, departId, strWhere.ToString(), (pageIndex - 1) * pageSize + 1, pageSize, "ORDER BY Co_Course." + jsRenderSortField);
            int n = 0;
            var itemArray = new object[listCourse.Count()];
            foreach (var item in listCourse)
            {
                string tempTeacher = "";
                if (item.TeacherIsDelete == 1 || item.TeacherIsTeacher == 0)
                {
                    tempTeacher = "--";
                }
                else
                {
                    tempTeacher = item.TeacherName;
                }
                var temp = new
                {
                    Id = item.Id,
                    CourseName = item.CourseName.HtmlXssEncode(),
                    CourseLength = item.CourseLength,
                    IsMust = typeof(LiXinModels.Enums.IsMust).GetEnumName(item.IsMust),
                    StartTime = item.StartTime.ToString("yyyy-MM-dd HH:mm"),
                    EndTime = item.EndTime.ToString("yyyy-MM-dd HH:mm"),
                    SortStr = item.SortStr,
                    TeacherName = tempTeacher.HtmlXssEncode(),
                    RoomName = item.RoomName.HtmlXssEncode(),
                    Way = item.Way,
                    IsCPA = typeof(LiXinModels.Enums.IsCPA).GetEnumName(item.IsCPA),
                    Publishflag = item.Publishflag,
                    CourseTimes = item.CourseTimes,
                    CourseTimesOrder = item.CourseTimesOrder,
                    TotalCourseTimes = item.TotalCourseTimes,
                    IsSystem = item.IsSystemStr,
                    IsYearPlan = item.IsPlanStr,
                    CourseState = item.CourseState,
                    IsOpenOrderStr = item.IsOpenOrderStr,
                    DepartSetId = item.DepartSetId,
                    StatusShow = item.StatusShow,
                    ClassroomAddress = item.ClassroomAddress,
                    IsOpenOrder = item.IsOpenOrder,
                    NumberLimited = item.LimitNumber == 0 ? item.NumberLimited : item.LimitNumber
                };
                itemArray[n] = temp;
                n++;
            }
            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 开放课程预订
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public string GetOpenOrder(string courseId, DateTime StartTime, DateTime EndTime, int departId, int num)
        {
            var timeNow = DateTime.Now;
            if (timeNow >= StartTime)
            {
                return "3";
            }
            var isExist = courseBl.IsOrderExist(courseId, departId);
            bool isDone;
            if (isExist)
            {
                isDone = courseBl.UpdataOrder(courseId, departId);
                return isDone ? "1" : "2";
            }
            isDone = courseBl.IsOrderInsert(courseId, departId, num);
            return isDone ? "1" : "2";
        }

        /// <summary>
        /// 修改课程开放课程报名人数
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public string ModifyOpenOrder(string courseId, int departId, int num = 0)
        {
            var isExist = courseBl.IsOrderExist(courseId, departId);
            if (isExist)
            {
                return courseBl.UpdataOrder(courseId, departId, num) ? "1" : "2";
            }
            return "2";
        }

        /// <summary>
        /// 不开放课程预订
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="StartTime"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        public string DeleteOpenOrder(string courseId, DateTime StartTime, int departId, int num)
        {
            DateTime timeNow = System.DateTime.Now;
            var isExist = courseBl.IsOrderExist(courseId, departId);
            if (timeNow < StartTime)
            {
                if (isExist == true)
                {
                    var isDeleteOk = courseBl.DeleteOrder(courseId, departId);
                    if (isDeleteOk == true)
                    {
                        UserDisorder(courseId.StringToInt32(), departId);
                        return "1";
                    }
                    else
                    {
                        return "2";
                    }
                }
                else
                {
                    var isDeleteOk = courseBl.IsDeleteOrder(courseId, departId, num);
                    if (isDeleteOk == true)
                    {
                        UserDisorder(courseId.StringToInt32(), departId);
                        return "1";
                    }
                    else
                    {
                        return "2";
                    }
                }

            }
            else
            {
                return "3";
            }
        }

        public void UserDisorder(int courseId, int departId)
        {
            List<DepTran_CourseOrder> list = new List<DepTran_CourseOrder>();
            list = orderBl.GetAllOrder(courseId, departId);
            foreach (var item in list)
            {
                orderBl.SetOrder(courseId, departId, item.UserId);
            }
        }


        #region 转播课程预定查询列表
        public JsonResult GetCourseSubscribeList(string course, string teaName, int courseOrder, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "co_course.starttime desc")
        {
            try
            {
                //List<DepTranDepartSetting> list = depLConfigBL.GetDepartSettingByUserId(CurrentUser.UserId);

                int totalCount = 0;
                string where = " 1=1";
                if (!string.IsNullOrWhiteSpace(teaName))
                    where += string.Format(" and Sys_user.Realname  like '%{0}%'", teaName.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(course))
                    where += string.Format(" and Co_Course.CourseName like '%{0}%'", course.ReplaceSql());
                var list = iDepTran_CourseOpen.GetList(out totalCount, CurrentUser.UserId, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);

                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<DepTran_CourseOpen>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取报名成功的人员，根据培训级别分
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public JsonResult GetDrawSubscribe(int courseId, int DepartId)
        {
            var pie = new ChartModel();
            var column = new ChartModel();
            var pieList = iDepTran_CourseOpen.GetSuccessTrainGradeSubscribe(courseId, DepartId); //报名成功的 
            //var pieList = null;
            var pieSer = new List<pieSeries>();
            var columnSer = new List<Series>();

            #region 扇形图
            foreach (var item in pieList)
            {
                pieSer.Add(new pieSeries
                {
                    name = item.Name + " (" + item.SubscribeCount + "人)",
                    y = item.SubscribeCount
                });
            }
            pie.DivID = "showpie";
            pie.title = "培训级别报名率";
            pie.pieseries = pieSer;
            #endregion

            return Json(new
            {
                pie = pie,
                column = column
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 课程预定查询中能管理的所有人员
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCourseSubscribeUserList(int courseId,int DepartId, string realName = "", string deptName = "",int isorder=0, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = " UserId")
        {
            try
            {
                int totalcount = 0;
                var where = " 1=1";
                if (!string.IsNullOrWhiteSpace(deptName))
                    where += string.Format(" and deptName like '%{0}%'", deptName.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(realName))
                    where += string.Format(" and RealName like '%{0}%'", realName.ReplaceSql());
                if (isorder != 0)
                {
                    where += string.Format(" and OrderStatus = {0}", isorder);
                }
                var list = iDepTran_CourseOpen.GetCourseSubscribeUserList(out totalcount, DepartId, courseId, (pageIndex - 1) * pageSize, pageSize, where, jsRenderSortField);

                var newList = new List<object>();
                list.ForEach(p => newList.Add(new
                {
                    p.UserId,
                    p.Realname,
                    p.SexStr,
                    p.DeptName,
                    p.TrainGrade,
                    p.OrderStatusStr,
                    p.DepAppointStr,
                    p.OrderTimeStr,
                    p.OrderStatus
                }
                        ));
                return Json(new
                {
                    result = 1,
                    dataList = newList,
                    recordCount = totalcount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(new
                {
                    result = 0,
                    dataList = new List<Sys_User>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 导出课程预定查询中能管理的所有人员
        /// </summary>
        /// <returns></returns>
        public void ExportCourseSubscribeUserList(int courseId, string realName = "", string deptName = "", int DepartId=0,int isorder=0)
        {
            int totalcount = 0;
            var where = " 1=1";
            
            if (!string.IsNullOrWhiteSpace(deptName))
                where += string.Format(" and deptName like '%{0}%'", deptName.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(realName))
                where += string.Format(" and RealName like '%{0}%'", realName.ReplaceSql());
            if (isorder != 0)
            {
                where += string.Format(" and OrderStatus = {0}", isorder);
            }
            var list = iDepTran_CourseOpen.GetCourseSubscribeUserList(out totalcount,DepartId, courseId, where: where);
            var dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("部门/分所", typeof(string));
            dt.Columns.Add("培训级别", typeof(string));
            dt.Columns.Add("性别", typeof(string));
           
            dt.Columns.Add("报名时间", typeof(string));
            var count = 1;
            foreach (Sys_User item in list)
            {
                dt.Rows.Add(count++, item.UserId, item.Realname, item.DeptPath, item.TrainGrade, item.SexStr, item.OrderTimeStr);
            }
            var dtList = new List<DataTable>();
            string strFileName = "转播课程报名情况";
            dtList.Add(dt);
            var export = new Spreadsheet();
            export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, strFileName);
        }
        #endregion

        #region == 附件下载 ==
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">附件的真实文件名</param>
        /// <returns></returns>
        public JsonResult IsExistFile(string path)
        {
            if (!System.IO.File.Exists(Server.MapPath(UFCOResource + path)))
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = 1
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="path">附件的真实文件名</param>
        /// <param name="name">附件名</param>
        public void LoadPrincipleFile(string path, string name)
        {
            if (System.IO.File.Exists(Server.MapPath(UFCOResource + path)))
            {
                var filePath = Server.MapPath(UFCOResource + path); //路径 
                //以字符流的形式下载文件
                var fs = new FileStream(filePath, FileMode.Open);
                var bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.ContentType = "application/octet-stream";
                //通知浏览器下载文件而不是打开
                Response.AddHeader("Content-Disposition",
                                   "attachment;  filename=" + HttpUtility.UrlEncode(name, Encoding.UTF8));
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            else
            {
                Response.Write("此文件已不存在");
            }
        }
        #endregion

        #region 转播课程管理（最高权限的人）

        /// <summary>
        /// 转播课程管理（最高权限的人）
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDeptAllCourseManage(string courseName, string StartTime, string EndTime, string IsMust, string IsOrder, string IsCPA, int Sort = -1, string isSys = "", string isYear = "", int pageSize = 20, int pageIndex = 1, int systemId = 0, int PublishFlag = -1, string jsRenderSortField = " StartTime desc")
        {
            try
            {

                #region 条件
                StringBuilder strWhere = new StringBuilder();
                strWhere.Append(string.Format("  CourseFrom=2 AND way=1 "));
                if (!string.IsNullOrEmpty(courseName))
                {
                    strWhere.AppendFormat(" AND courseName like '%{0}%' ", courseName.ReplaceSql());
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
                    strWhere.AppendFormat(" AND Sort Like '%{0}%'", Sort);
                }
                if (!string.IsNullOrEmpty(IsMust))
                {
                    if (!IsMust.Contains(","))
                    {
                        strWhere.AppendFormat(" AND IsMust={0}", Convert.ToInt32(IsMust));
                    }
                }
                if (!string.IsNullOrEmpty(IsCPA))
                {
                    if (!IsCPA.Contains(","))
                    {
                        strWhere.AppendFormat(" AND IsCPA={0}", Convert.ToInt32(IsCPA));
                    }
                }
                if (!string.IsNullOrEmpty(isYear))
                {
                    if (!isYear.Contains(","))
                    {
                        strWhere.AppendFormat(" and IsYearPlan ={0}", Convert.ToInt32(isYear));
                    }
                }
                if (!string.IsNullOrEmpty(isSys))
                {
                    if (!isSys.Contains(","))
                    {
                        if (Convert.ToInt32(isSys) == 0)
                        {
                            strWhere.AppendFormat(" and IsSystem=0 ");
                        }
                        else
                        {
                            strWhere.AppendFormat(" and IsSystem>0 ");
                        }
                    }
                }

                if (!string.IsNullOrEmpty((IsOrder)))
                {
                    if (!IsOrder.Contains(","))
                    {
                        if (IsOrder == "1")
                        {
                            strWhere.AppendFormat(" and isnull(OpenFlag,0)!=0 ");
                        }
                        else
                        {
                            strWhere.AppendFormat(" and isnull(OpenFlag,0)=0 ");
                        }
                    }
                }

                if (PublishFlag != -1)
                {
                    strWhere.AppendFormat(" AND PublishFlag={0} ", PublishFlag);
                }
                #endregion

                List<Dept_Course> listCourse = new List<Dept_Course>();

                int totalCount = 0;
                listCourse = courseBl.GetDeptAllCourseManage(out totalCount, pageIndex, pageSize, strWhere.ToString(), jsRenderSortField);
                int n = 0;
                var itemArray = new object[listCourse.Count];
                foreach (var item in listCourse)
                {
                    string tempTeacher = "";
                    if (item.TeacherIsDelete == 1 || item.TeacherIsTeacher == 0)
                    {
                        tempTeacher = "--";
                    }
                    else
                    {
                        tempTeacher = item.TeacherName;
                    }
                    var temp = new
                    {
                        Id = item.Id,
                        CourseName = item.CourseName.HtmlXssEncode(),
                        CourseLength = item.CourseLength,
                        IsMust = typeof(LiXinModels.Enums.IsMust).GetEnumName(item.IsMust),
                        StartTime = item.StartTime.ToString("yyyy-MM-dd HH:mm"),
                        EndTime = item.EndTime.ToString("yyyy-MM-dd HH:mm"),
                        SortStr = item.SortStr,
                        TeacherName = tempTeacher.HtmlXssEncode(),
                        RoomName = item.RoomName.HtmlXssEncode(),
                        Way = item.Way,
                        IsCPA = typeof(LiXinModels.Enums.IsCPA).GetEnumName(item.IsCPA),
                        Publishflag = item.Publishflag,
                        CourseTimes = item.CourseTimes,
                        CourseTimesOrder = item.CourseTimesOrder,
                        TotalCourseTimes = item.TotalCourseTimes,
                        IsSystem = item.IsSystemStr,
                        IsYearPlan = item.IsPlanStr,
                        CourseState = item.CourseState,
                        IsOpenOrderStr = item.IsOpenOrderStr,
                        DepartSetId = item.DepartSetId,
                        StatusShow = item.StatusShow,
                        ClassroomAddress = item.ClassroomAddress
                    };
                    itemArray[n] = temp;
                    n++;
                }
                return Json(new
                {
                    result = 1,
                    dataList = itemArray,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取报名的人员名册
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetCourseUserList(int courseid, int id = 0, int pageIndex = 1, int pageSize = 10)
        {
            int Count = 0;
            var where = " 1=1";
            if (id != 0)
            {
                where = " ddc.id=" + id;
            }

            var list = orderBl.GetCourseUserList(out Count, courseid, where:where);

            var listID = new List<int>();
            var htmlStr = new StringBuilder();
            foreach (var item in list)
            {
                if (!listID.Contains(item.DepartSetID))
                {
                    listID.Add(item.DepartSetID);
                    if (item.DepartSetID > 0)
                    {
                        htmlStr.AppendFormat("<tr id='info'><td class='Tit'>{0}:</td><td><a style='color:red;cursor:pointer;text-decoration:underline' onclick='changeList({1})'>{2}</a></td></tr>", item.DepartSetName, item.DepartSetID, list.Count(p => p.DepartSetID == item.DepartSetID));
                    }
                }
            }

            list = list.Skip((pageIndex-1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                recordCount = Count,
                dataList = list,
                htmlStr = htmlStr.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 考勤详情
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetAttendceUserList(int courseId, int pageIndex = 1, int pageSize = 20)
        {
            int Count = 0;
            var list = DAttendce.GetAttendceUserList(out Count, courseId, pageIndex, pageSize);

            return Json(new
            {
                recordCount = Count,
                dataList = list

            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 并接字符串
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="ExampaperID"></param>
        /// <param name="questiontype"></param>
        /// <param name="QuestionID"></param>
        /// <returns></returns>
        public string FindAnswerString(int courseid, int ExampaperID, int questiontype, int QuestionID)
        {
            var list = ISurveyReplyAnswerBL.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + courseid + " and ExampaperID=" + ExampaperID + " and QuestionID=" + QuestionID + " and Survey_ReplyAnswer.Status=1");
            string html = "";
            int i = 1;
            foreach (var surveyReplyAnswer in list)
            {
                if (questiontype == 2)
                {
                    html += "<p>" + i + ".<strong class='c_col'>回答：</strong>" + surveyReplyAnswer.SubjectiveAnswer + "<span>（[" + surveyReplyAnswer.DeptName + "] " + surveyReplyAnswer.Realname + "）</span>" + "</p>";
                }
                else
                {
                    html += "<p>" + i + ".<strong>评分：" + surveyReplyAnswer.SubjectiveAnswer + "</strong><strong class='c_col'>理由：</strong>" + surveyReplyAnswer.QuestionReply + "<span>（[" + surveyReplyAnswer.DeptName + "] " + surveyReplyAnswer.Realname + "）</span>" + "</p>";
                }
                i++;
            }

            return html;

        }

        /// <summary>
        /// 获得相关考试人员的信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOnLineTest(int courseid, int pageSize = 10, int pageIndex = 1)
        {

            var Count = 0;
            var courseorder = orderBl.GetOnLineTest(out Count, courseid, pageIndex, pageSize);


            return Json(new
            {
                result = 1,
                dataList = courseorder,
                recordCount = Count
            }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 反馈
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult OnLoadCl_CourseAdvice(int id)
        {
            List<Cl_CourseAdvice> clCourseAdvicesList = ClCourseAdviceBL.GetList(" Cl_CourseAdvice.CourseId=" + id);
            string html = "";
            foreach (var clCourseAdvice in clCourseAdvicesList)
            {
                html += "<div id=" + clCourseAdvice.Id + " class='list'>";
                html += "<div class='list-head'><span>" + clCourseAdvice.userRealname
                        + "</span><img src='" + Url.Content(clCourseAdvice.userPhotoUrlStr) + "' title='" + clCourseAdvice.userRealname + "' /></div>";
                html += "   <div class='list-info'><i></i>";
                html += "       <div class='list-type'><strong>课前建议：</strong>" + clCourseAdvice.AdviceTime + "";


                html += "</div>";
                html += "       <div class='list-con'>" + clCourseAdvice.AdviceContent + "</div>";

                if (clCourseAdvice.AnserContent != "")
                {
                    html += "<div class='list-te'>";
                    html += "<p><strong class='c_col'>讲师<span>" + clCourseAdvice.TeacherName + "</span>的反馈内容：</strong>" + clCourseAdvice.AnserContent + "</p>";
                    html += "<p class='time'>时间:" + clCourseAdvice.AnserTime + "</p>";
                    html += "</div>";
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
