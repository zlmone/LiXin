using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinModels.CourseManage;
using LiXinCommon;
using System.Text.RegularExpressions;
using LiXinModels.NewCourseManage;
using LixinModels.NewClassManage;
using LiXinControllers.Filter;

namespace LiXinControllers
{
    public partial class NewCourseManageController : BaseController
    {
        #region 呈现页面
        /// <summary>
        /// 课程管理
        /// </summary>
        /// <param name="way">1:集中授课 2 视频授课</param>
        /// <returns></returns>
        public ActionResult NewCourseManageList(string p, int way = 1)
        {
           
            ViewBag.Way = way;
            if (p != null && p != "" && p == "1")
            {
                ViewBag.page = 1;
            }
            else
            {
                ViewBag.page = 0;
            }
            return View();
        }

        public ActionResult NewCourseTogetherList(string tp)
        {
            ViewBag.nowTime = DateTime.Now.Year + "-01-01 00:00";
            ViewBag.page = 1;
            ViewBag.cname = "请输入搜索内容";
            ViewBag.isGroupt = "-1";
            ViewBag.state = "0";
            if (tp != null && tp != "" && tp == "1")
            {
                if (Session["newcopage"] != null)
                {
                    string sess = Session["newcopage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.way = att[0];
                    ViewBag.cname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.cstartTime = att[2];
                    ViewBag.cendTime = att[3];
                    ViewBag.isGroupt = att[4];
                    ViewBag.state = att[5];
                    ViewBag.page = att[6];
                }
            }
            return View();
        }

        public ActionResult NewCourseVideoList(string vp)
        {
            ViewBag.nowTime = DateTime.Now.Year + "-01-01 00:00";
            ViewBag.page = 1;
            ViewBag.cname = "请输入搜索内容";
            ViewBag.state = "0";
            if (vp != null && vp != "" && vp == "1")
            {
                if (Session["newcopage"] != null)
                {
                    string sess = Session["newcopage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.way = att[0];
                    ViewBag.cname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.cstartTime = att[2];
                    ViewBag.cendTime = att[3];
                    ViewBag.isGroupt = att[4];
                    ViewBag.state = att[5];
                    ViewBag.page = att[6];
                }
            }
            return View();
        }

        public ActionResult EditNewCourseVideo(int? Id)
        {
            New_Course course = new New_Course();
            ViewBag.ClassList = new List<New_Class>();
            var coursePaper = new New_CoursePaper
            {
                Hour = Convert.ToInt32(AllSystemConfigs.Find(s => s.ConfigType == 26).ConfigValue.Split(';')[0]),//课程结束后 多久时间内 允许考试
                Length = Convert.ToInt32(AllSystemConfigs.Find(s => s.ConfigType == 26).ConfigValue.Split(';')[1]),//考试时长
                TestTimes = Convert.ToInt32(AllSystemConfigs.Find(s => s.ConfigType == 27).ConfigValue) //测试次数
            };
            if (Id.HasValue)
            {
                course = _courseBL.GetSingleCourse(Id.Value);
                if (course == null)
                {
                    course = new New_Course();
                    course.VideoLowLength = Convert.ToInt32(AllSystemConfigs.Find(s => s.ConfigType == 25).ConfigValue);
                }
                else
                {
                    //班级
                    if (!string.IsNullOrEmpty(course.Classes))
                    {
                        ViewBag.ClassList = _newclass.GetClassList(string.Format(" Id in (select id from dbo.F_SplitIDs('" + course.Classes + "'))"));
                    }
                    List<New_CourseFiles> listResource = _courseResourceBL.GetCourseResourceList(Id.Value);
                    ViewBag.SCROMResource = listResource.Where(c => c.Type == 3).FirstOrDefault();//SCROM 课件
                    ViewBag.CourseAttachList = listResource.Where(c => c.Type == 1);//附件
                    ViewBag.CourseVideoList = listResource.Where(c => c.Type == 4);//视频
                    ViewBag.allID = string.Join(",", listResource.Where(c => c.Type == 4).Select(p => p.PackId));
                    //在线测试
                    if (course.IsTest > 0)
                    {
                        var coPaperList = _newCoursePaper.GetCoursePaper(Id.Value);
                        if (coPaperList.Count > 0)
                        {
                            coursePaper = coPaperList.FirstOrDefault();
                            LiXinModels.Examination.DBModel.tbExampaper exampaper = _exampaperBL.GetExampaper(coursePaper.PaperId);
                            //TODO: 保存 试卷名称 以及试卷总分
                            if (exampaper != null && exampaper.Status != 1)
                            {
                                ViewBag.PaperName = exampaper.ExampaperTitle;
                                ViewBag.PaperTotalScore = exampaper.ExampaperScore;
                            }
                        }
                    }
                    //课后评估
                    if (course.IsPingCourse > 0)
                    {
                        LiXinModels.Survey.Survey_Exampaper surveyExamPaper = _surveyExampaperBL.GetServeyExamPaper(Convert.ToInt32(course.IsPingCourse));
                        if (surveyExamPaper != null || surveyExamPaper.IsDelete != 1)
                        {
                            ViewBag.SurveyExampaperId0 = surveyExamPaper.ExampaperID;
                            ViewBag.SurveyExampaperName0 = surveyExamPaper.ExamTitle;
                        }
                    }
                }
            }
            else
            {
                course.VideoLowLength = Convert.ToInt32(AllSystemConfigs.Find(s => s.ConfigType == 25).ConfigValue);
            }
            ViewBag.CoursePaper = coursePaper;
            return View(course);
        }

        public ActionResult NewCourseVideoDetial(int Id)
        {
            var course = _courseBL.GetSingleCourse(Id);
            if (course == null || course.IsDelete == 1)
            {
                course = new New_Course();
                ViewBag.MsgTips = "该课程不存在或已被删除！";
            }
            else
            {
                var listResource = _courseResourceBL.GetCourseResourceList(Id);
                ViewBag.SCROMResource = listResource.FirstOrDefault(c => c.Type == 3);//SCROM 课件

                ViewBag.CourseAttachList = listResource.Where(c => c.Type == 1).ToList();//附件

                ViewBag.CourseVideoList = listResource.Where(c => c.Type == 4);//视频

                if (course.IsTest == 1)
                {
                    var coPaperList = _newCoursePaper.GetCoursePaper(Id);
                    ViewBag.CoursePaper = coPaperList.FirstOrDefault();
                    if (ViewBag.CoursePaper == null)
                    {
                        ViewBag.CoursePaper = new New_CoursePaper();
                        course.IsTest = 0;
                    }
                    else
                    {
                        LiXinModels.Examination.DBModel.tbExampaper exampaper = _exampaperBL.GetExampaper(ViewBag.CoursePaper.PaperId);
                        //TODO: 保存 试卷名称 以及试卷总分
                        if (exampaper != null && exampaper.Status != 1)
                        {
                            ViewBag.PaperName = exampaper.ExampaperTitle;
                            ViewBag.PaperTotalScore = exampaper.ExampaperScore;
                        }
                        else
                        {
                            course.IsTest = 0;
                        }
                    }
                }

                if (course.IsPingCourse > 0)
                {
                    course.AfterCourseAssess = _surveyExampaperBL.GetSurveyExampaper(course.IsPingCourse);
                }

            }

            return View(course);
        }

        #endregion

        #region 课程列表操作

        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <param name="way"> 1：集中；2：自学 </param>
        /// <param name="courseName">课程名称</param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="IsGroupTeach">是否分组</param>
        /// <param name="state">课程状态</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetNewCourseList(int way, string courseName, string StartTime, string EndTime, int IsGroupTeach, int state, int pageSize = 20, int pageIndex = 1, int pub = 0, string jsRenderSortField = "LastUpdateTime desc")
        {
            if (Session["newcopage"] != null)
            {
                Session.Remove("newcopage");
            }
            Session["newcopage"] = way + "㉿" + courseName + "㉿" + StartTime + "㉿" + EndTime + "㉿" + IsGroupTeach + "㉿" + state + "㉿" + pageIndex;

            int totalCount = 0;
            var strWhere = new StringBuilder();
            strWhere.Append(" IsDelete=0 ");
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
            if (IsGroupTeach > -1)
            {
                strWhere.AppendFormat(" AND IsGroupTeach = {0}", IsGroupTeach);
            }
            if (pub > 0)
            {
                strWhere.AppendFormat(" AND PublicFlag = {0}", pub);
            }
            switch (state)
            {
                case 0:
                    break;
                case 1:
                    strWhere.AppendFormat(" and PublicFlag=1 and StartTime > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    break;
                case 2:
                    strWhere.AppendFormat(" and PublicFlag=1 and StartTime <= '{0}' and EndTime >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    break;
                case 3:
                    strWhere.AppendFormat(" and PublicFlag=1 and EndTime < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    break;
                case 4:
                    strWhere.AppendFormat(" and PublicFlag=0 ");
                    break;
            }

            var listCourse = _courseBL.GetNewCourseList(way, out totalCount, pageIndex, pageSize, jsRenderSortField, strWhere.ToString());
            foreach (var item in listCourse)
            {
                item.CourseName = item.CourseName.HtmlXssEncode();
                item.teacher = item.teacher.HtmlXssEncode();
                item.roomnames = item.roomnames.HtmlXssEncode();
            }
            return Json(new
            {
                dataList = listCourse,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 发布-删除课程
        /// </summary>
        /// <param name="flag">0:删除 1:发布</param>
        /// <param name="courseId"></param>
        /// <param name="way"> 1：集中；2：自学</param>
        /// <returns></returns>
        public JsonResult UpdateNewCourse(int flag, int courseId, int way = 0)
        {
            int page = 1;
            string sess = "";

            if (Session["newcopage"] != null)
            {
                sess = Session["newcopage"].ToString();
            }
            if (sess != "")
            {
                string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                page = Convert.ToInt32(att[6]);
            }

            var strWhere = new StringBuilder();
            strWhere.Append(" 1=1 ");
            if (way > 0)
            {
                strWhere.AppendFormat(" AND Way = {0}", way);
            }
            New_Course course = _courseBL.GetSingleCourse(courseId, strWhere.ToString());
            if (flag == 1)
            {
                course.PublicFlag = 1;
                if (way == 1)
                {
                    var ruleList = _courseRoomRule.GetNew_CourseRoomRuleListByCourseId(courseId);
                    //混合培训
                    if (course.IsGroupTeach == 0 || ruleList.Count == 0)
                    {
                        return Json(new { result = 0, content = "课程里面没有开设集中授课也没有开设分组带教！", indexpage = page },
                                    JsonRequestBehavior.AllowGet);
                    }
                    if ((course.IsGroupTeach == 1 || course.IsGroupTeach == 3) && ruleList.Count(p => p.Type == 0) == 0)
                    {
                        return Json(new { result = 0, content = "课程里面没有开设集中授课！", indexpage = page },
                                    JsonRequestBehavior.AllowGet);
                    }
                    if ((course.IsGroupTeach == 2 || course.IsGroupTeach == 3) && ruleList.Count(p => p.Type == 1) == 0)
                    {
                        return Json(new { result = 0, content = "课程里面没有开设分组带教！", indexpage = page },
                                    JsonRequestBehavior.AllowGet);
                    }
                    //初始化学员数据
                    //删除已存在数据
                    _newCourseOrder.DeleteCourseOrder(string.Format(" CourseId={0} ", course.Id));
                    _courseOrderDetail.DeleteCourseOrder(string.Format(" CourseId ={0} ", course.Id));
                    var list = _courseRoomRule.GetNew_CourseRoomRuleListByCourseId(course.Id);
                    var havedcourseorder = new List<int>();
                    list.ForEach(p =>
                                     {
                                         var detailarr = p.SeatDetail.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                         var subuserid = new List<int>();
                                         for (var i = 0; i < detailarr.Length; i++)
                                         {
                                             var subdetail = detailarr[i].Split(',');
                                             var userId = 0;
                                             if (subdetail.Length == 5)
                                                 userId = Convert.ToInt32(subdetail[2]);
                                             if (subdetail.Length == 6)
                                                 userId = Convert.ToInt32(subdetail[3]);
                                             if (userId > 0)
                                             {
                                                 if (!havedcourseorder.Contains(userId))
                                                 {
                                                     havedcourseorder.Add(userId);
                                                     var groupuser = _newGroupUser.GetModelByUserID(userId);
                                                     _newCourseOrder.InsertNew_CourseOrder(new New_CourseOrder { CourseId = course.Id, ClassId = groupuser == null ? 0 : groupuser.ClassId, UserId = userId, OrderTime = DateTime.Now, LearnStatus = 0, TogetherScore = 0, GroupScore = 0, CourseExamScore = 0, ExamScore = 0, TogetherMemo = "", GroupMemo = "", Type = 0, CourseExamSumScore = 0, ExamSumScore = 0 });
                                                 }
                                                 if (!subuserid.Contains(userId))
                                                 {
                                                     subuserid.Add(userId);
                                                     _courseOrderDetail.InsertNew_CourseOrderDetail(new New_CourseOrderDetail { CourseId = course.Id, UserId = userId, SubCourseID = p.Id, LearnTime = DateTime.Now, LearnStatus = 0, Type = p.Type, IsLeave = 0, ApprovalName = "", LeaveReason = "" });
                                                 }
                                             }
                                         }
                                     });

                }
                LogHelper.WriteLog("新员工 发布课程", this.ControllerContext);
                return _courseBL.UpdateCourse(course) ? Json(new { result = 1, content = "发布成功", indexpage = page }, JsonRequestBehavior.AllowGet) : Json(new { result = 0, content = "发布失败" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                course.IsDelete = 1;
                bool result = _courseBL.UpdateCourse(course);
                LogHelper.WriteLog("新员工 删除课程", this.ControllerContext);
                if (result)
                    return Json(new
                    {
                        result = 1,
                        content = "删除成功",
                        indexpage = page
                    }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new
                    {
                        result = 0,
                        content = "删除失败"
                    }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 自学课程编辑操作

        /// <summary>
        /// 验证课程编号重名
        /// </summary>
        public JsonResult CheckNewCourseCode(string CourseCode, int Id, int way = 1)
        {
            return Json(!_courseBL.IsExistCourseCode(CourseCode.ReplaceSingleSql(), Id, way), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断时间有效性
        /// </summary>
        /// <param name="starttime"></param>
        /// <returns></returns>
        public JsonResult CheckJudgeTime(string starttime)
        {
            try
            {
                var time = Convert.ToDateTime(starttime);
                return Json(time < DateTime.Now ? false : true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 编辑自学课程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="groupteach">是否分组带教</param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="teacher"></param>
        /// <param name="classes">班级ID集合</param>
        /// <param name="surveyPaper">问卷ID</param>
        /// <param name="test">在线测试集合</param>
        /// <param name="memo">备注</param>
        /// <param name="way">课程类型</param>
        /// <param name="ReType">资源类型</param>
        /// <param name="ScormData">Scorm资源数据</param>
        /// <param name="VideoData">视频资源数据</param>
        /// <param name="RemoveReIds">需要删除的资源数据</param>
        /// <param name="lowlength">视频观看最低时限</param>
        /// <returns></returns>
        [Filter.SystemLog("新员工 编辑视频课程", LogLevel.Info)]
        public JsonResult SubmitCourseVideo(int id, string name, string code, int groupteach, string starttime, string endtime, string teacher, string classes, string surveyPaper, string test, int way, int ReType, string ScormData, string VideoData, string RemoveReIds, int oldReType, int lowlength)
        {
            try
            {
                var testarr = test.Split(',');
                var course = new New_Course();
                if (id > 0)
                {
                    course = _courseBL.GetSingleCourse(id);
                }
                if (!string.IsNullOrEmpty(testarr[0]))
                {
                    course.IsTest = 1;
                }
                else
                {
                    course.IsTest = 0;
                }
                course.Classes = classes;
                course.Code = code;
                course.CourseName = name;
                course.EndTime = Convert.ToDateTime(endtime);
                course.StartTime = Convert.ToDateTime(starttime);
                course.IsGroupTeach = groupteach;
                course.Teachers = teacher;
                course.VideoLowLength = lowlength;
                if (way == 1)
                {
                    course.IsPingTeacher = string.IsNullOrEmpty(surveyPaper) ? "" : surveyPaper;
                    course.IsPingCourse = 0;
                }
                else
                {
                    course.IsPingTeacher = "";
                    course.IsPingCourse = Convert.ToInt32(string.IsNullOrEmpty(surveyPaper) ? "0" : surveyPaper);
                }

                if (id > 0)
                {
                    _courseBL.UpdateCourse(course);
                }
                else
                {
                    course.GGroupNumber = 0;
                    course.GGroupPersonCount = 0;
                    course.GGroupRule = "";
                    course.GStartTime = "";
                    course.GType = 0;
                    course.Way = way;
                    course.LastUpdateTime = DateTime.Now;
                    course.PublicFlag = 0;
                    course.IsDelete = 0;
                    course.ScoreDistribute = "";
                    _courseBL.AddCourse(course);
                }

                //删除课程附件
                if (!string.IsNullOrEmpty(RemoveReIds))
                    _courseResourceBL.DeleteCourseFiles(RemoveReIds);

                //修改在线测试
                if (id > 0)
                {
                    _newCoursePaper.DeleteCoursePaper(course.Id);
                }
                if (!string.IsNullOrEmpty(testarr[0]))
                {
                    var coursepaper = new New_CoursePaper
                    {
                        CourseId = course.Id,
                        PaperId = Convert.ToInt32(testarr.Length > 0 ? testarr[0] : "0"),
                        LevelScore = Convert.ToInt32(testarr.Length > 1 ? testarr[2] : "0"),
                        Length = Convert.ToInt32(testarr.Length > 1 ? testarr[1] : "0"),
                        Hour = 0,
                        TestTimes = Convert.ToInt32(testarr.Length > 1 ? testarr[3] : "0")
                    };
                    _newCoursePaper.AddCoursePaper(coursepaper);
                }

                //课件资源
                if (ReType == 1)
                {
                    if (oldReType == 0 && !string.IsNullOrEmpty(ScormData))
                    {
                        var scormStr = ScormData.Split('|');
                        var Remodel = new New_CourseFiles
                        {
                            CourseId = course.Id,
                            Name = scormStr[1],
                            RealName = scormStr[2],
                            CreateDate = DateTime.Now,
                            LoadTimes = 0,
                            Type = 3,
                            PackId = Convert.ToInt32(scormStr.Length > 0 ? scormStr[0] : "0"),
                            ResourceSize = Convert.ToInt32(scormStr.Length > 0 ? scormStr[3] : "0"),
                            IsDelete = 0
                        };
                        _newCourseFiles.AddCourseFiles(Remodel);
                    }
                    else if (oldReType == 1 && !string.IsNullOrEmpty(ScormData))
                    {
                        New_CourseFiles Remodel = _newCourseFiles.GetSingleCourseFile(course.Id, " Type=3");
                        var scormStr = ScormData.Split('|');
                        Remodel.Name = scormStr[1];
                        Remodel.RealName = scormStr[2];
                        Remodel.CreateDate = DateTime.Now;
                        Remodel.LoadTimes = 0;
                        Remodel.PackId = Convert.ToInt32(scormStr.Length > 0 ? scormStr[0] : "0");
                        Remodel.ResourceSize = Convert.ToInt32(scormStr.Length > 0 ? scormStr[3] : "0");
                        _newCourseFiles.UpdateCourseFiles(Remodel);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(VideoData))
                    {
                        VideoData = VideoData.Remove(VideoData.LastIndexOf(";"), 1);
                        var videoStrList = VideoData.Split(';');
                        if (videoStrList.Length > 0)
                        {
                            for (int i = 0; i < videoStrList.Length; i++)
                            {
                                var videoStr = videoStrList[i].Split('|');
                                var Remodel = new New_CourseFiles
                                {
                                    CourseId = course.Id,
                                    Name = videoStr[1],
                                    RealName = videoStr[2],
                                    CreateDate = DateTime.Now,
                                    LoadTimes = 0,
                                    Type = 4,
                                    PackId = 0,
                                    ResourceSize = 0,
                                    IsDelete = 0
                                };
                                _newCourseFiles.AddCourseFiles(Remodel);
                            }
                        }
                    }
                }

                return Json(new { result = 1, id = course.Id }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, id = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 添加附件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ContentResult AddVideoCoursefile(New_CourseFiles model)
        {
            model.CreateDate = DateTime.Now;
            model.IsDelete = 0;
            model.LoadTimes = 0;
            model.PackId = 0;
            _newCourseFiles.AddCourseFiles(model);
            return Content(model.Id > 0 ? "1" : "0");
        }
        #endregion
    }
}
