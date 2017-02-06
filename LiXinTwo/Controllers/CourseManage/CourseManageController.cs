using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;
using LiXinModels;
using System.Web.Mvc;
using LiXinInterface.CourseManage;
using LiXinInterface.User;
using LiXinModels.CourseManage;
using LiXinCommon;
using System.IO;
using Retech.LearningAPI.Core;
using System.Web;
using LiXinModels.CourseLearn;
using System.Data;
using LiXinModels.User;
using LiXinControllers.Filter;
using System.Threading;
using System.Text.RegularExpressions;

namespace LiXinControllers
{
    public partial class CourseManageController : BaseController
    {

        #region 集中授课
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="TotalCourseTimes">TotalCourseTimes 不等于0 说明是课程拷贝,此时该值等于 已经开设的次数,则课程编号不能修改</param>
        /// <returns></returns>
        [Filter.SystemLog("编辑集中授课", LogLevel.Info)]
        public ActionResult EditCourseTogether(int? Id, int TotalCourseTimes = 0, int Publishflag = 0, int IsCopy = 0)
        {
            //必修学时
            ViewBag.MustCourseLength = AllSystemConfigs.Find(s => s.ConfigType == 18).ConfigValue.Split(';')[0];
            //选修学时
            ViewBag.ChoosableCourseLength = AllSystemConfigs.Find(s => s.ConfigType == 18).ConfigValue.Split(';')[1];

            //课程结束后 多久时间内 允许考试
            ViewBag.RefHour = AllSystemConfigs.Find(s => s.ConfigType == 26).ConfigValue.Split(';')[0];
            //考试时长
            ViewBag.RefLength = AllSystemConfigs.Find(s => s.ConfigType == 26).ConfigValue.Split(';')[1];
            //测试次数
            ViewBag.MaxTestTimes = AllSystemConfigs.Find(s => s.ConfigType == 27).ConfigValue;

            //课前建议时间外
            ViewBag.RefPreAdviceConfigTime = AllSystemConfigs.Find(s => s.ConfigType == 29).ConfigValue.Split(';')[0];
            //课后评估时间内
            ViewBag.RefAfterEvlutionConfigTime = AllSystemConfigs.Find(s => s.ConfigType == 29).ConfigValue.Split(';')[1];

            //ViewBag.xueshi = AllSystemConfigs.Find(p => p.ConfigType == 24);

            ViewBag.TotalCourseTimes = TotalCourseTimes;

            ViewBag.TokenPublishflag = Publishflag;//课程是否发布

            ViewBag.IsCopy = IsCopy;


            //培训级别
            ViewBag.trainGrade = _sys_TrainBL.GetAllTrainGrade();


            Co_Course course = new Co_Course();

            if (string.IsNullOrEmpty(course.CourseLengthDistribute))
            {
                course.CourseLengthDistribute = AllSystemConfigs.Find(p => p.ConfigType == 24).ConfigValue;
            }

            if (Id.HasValue)
            {
                course = _courseBL.GetCo_Course(Id.Value);

                if (course == null)
                {
                    course = new Co_Course();
                }
                else
                {
                    if (course.TeacherIsTeacher == 0 || course.TeacherIsDelete == 1)
                    {
                        course.Teacher = "";
                        course.TeacherName = "";
                    }
                    List<Co_CourseResource> listResource = _courseResourceBL.GetCourseResourceList(Id.Value);
                    ViewBag.CourseResourceList = listResource.Where(c => c.ResourceType == 0);//资源
                    ViewBag.CourseAttachList = listResource.Where(c => c.ResourceType == 1);//附件
                    if (course.IsTest == 1)
                    {
                        ViewBag.CoursePaper = _coursePaperBL.GetCo_CourseMainPaper(Id.Value);
                        if (ViewBag.CoursePaper == null)
                        {
                            ViewBag.CoursePaper = new Co_CoursePaper();
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
                    if (course.IsPing == 1)
                    {
                        string[] surveyArray = course.SurveyPaperId.Split(';');
                        bool flag = false;
                        foreach (var item in surveyArray)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {//时间问题，判断拙劣。不影响功能。
                                string[] itemArray = item.Split(',');
                                LiXinModels.Survey.Survey_Exampaper surveyExamPaper = _surveyExampaperBL.GetServeyExamPaper(Convert.ToInt32(itemArray[1]));
                                if (surveyExamPaper == null || surveyExamPaper.IsDelete == 1)
                                {
                                    if (itemArray[0] == "0")
                                    {
                                        ViewBag.SurveyExampaperName0 = "";
                                        //   
                                    }
                                    if (itemArray[0] == "2")
                                    {
                                        ViewBag.SurveyExampaperName2 = "";
                                    }
                                }
                                else
                                {
                                    flag = true;
                                    if (itemArray[0] == "0")
                                    {
                                        ViewBag.SurveyExampaperId0 = surveyExamPaper.ExampaperID;
                                        ViewBag.SurveyExampaperName0 = surveyExamPaper.ExamTitle;
                                    }
                                    if (itemArray[0] == "2")
                                    {
                                        ViewBag.SurveyExampaperId2 = surveyExamPaper.ExampaperID;
                                        ViewBag.SurveyExampaperName2 = surveyExamPaper.ExamTitle;
                                    }
                                }

                            }
                        }
                        if (!flag)
                        {
                            course.IsPing = 0;
                        }

                    }

                    if (course.OpenFlag == 1)//群组
                    {
                        int total = 0;
                        string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                        ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                    }
                    if (course.OpenFlag == 2)//组织结构
                    {
                        string strWhere = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                        ViewBag.Departs = _deptBL.GetAllList(strWhere);
                    }
                    if (course.OpenFlag == 3)//群组+组织结构
                    {
                        int total = 0;
                        string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                        ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                        string strWhereDept = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                        ViewBag.Departs = _deptBL.GetAllList(strWhereDept);
                    }
                    if (!string.IsNullOrEmpty(course.OpenPerson))
                    {
                        ViewBag.PersonList = _userBL.GetList(" UserId in (select id from dbo.F_SplitIDs('" + course.OpenPerson + "'))");
                    }
                }
            }
            ViewBag.ClassRoomList = _classRoomBL.GetRoomList();
            ViewBag.YearPlanList = _trYearPlanBL.GetAllYear(" PublishFlag=1 AND IsDelete=0 ").Select(t => t.Year);
            return View(course);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="showSearch">显示搜索：0 不显示 1：显示</param>
        /// <param name="showOperation">显示操作列：0不显示：1：显示</param>
        /// <param name="showCourseButton">显示确认按钮：0不显示：1：显示</param>
        /// <param name="showCourseCheckbox">显示复选框按钮：0不显示：1：显示</param>   
        /// <param name="typeCourse">课程列表公共显示 0:弹出层页面引用 ， 1：引用母版页</param>  
        /// <param name="showAddButton">课程开设按钮 0:不显示 ， 1：显示</param>     
        /// <returns></returns>
        // [Filter.SystemLog("公共课程列表", LogLevel.Info)]
        public ActionResult CourseCommonList(int showSearch = 0, int showOperation = 0, int showCourseButton = 0, int showCourseCheckbox = 0, int typeCourse = 0, int showAddButton = 0)
        {
            ViewBag.showSearch = showSearch;
            ViewBag.showOperation = showOperation;
            ViewBag.showCourseButton = showCourseButton;
            ViewBag.showCourseCheckbox = showCourseCheckbox;
            ViewBag.typeCourse = typeCourse;
            ViewBag.showAddButton = showAddButton;
            return View();
        }
        // [Filter.SystemLog("集中授课 课程列表", LogLevel.Info)]
        public ActionResult CourseTogetherList(string tp)
        {
            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.AttSort = "-1";
            if (tp != null && tp != "" && tp == "1")
            {
                if (Session["ctpage"] != null)
                {
                    string sess = Session["ctpage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Attname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.AttstartTime = att[2];
                    ViewBag.AttendTime = att[3];
                    ViewBag.Attmust = att[4];
                    ViewBag.Attcpa = att[5];
                    ViewBag.AttSort = att[6];
                    ViewBag.AttisSys = att[7];
                    ViewBag.AttisYear = att[8];
                }
            }
            return View();
        }
        // [Filter.SystemLog("视频授课 课程列表", LogLevel.Info)]
        public ActionResult CourseVideoList(string vp)
        {
            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.AttSort = "-1";
            if (vp != null && vp != "" && vp == "1")
            {
                if (Session["cvpage"] != null)
                {
                    string sess = Session["cvpage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Attname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.AttstartTime = att[2];
                    ViewBag.AttendTime = att[3];
                    ViewBag.Attmust = att[4];
                    ViewBag.Attcpa = att[5];
                    ViewBag.AttSort = att[6];
                    ViewBag.AttisSys = att[7];
                    ViewBag.AttisYear = att[8];
                }
            }
            return View();
        }


        [Filter.SystemLog("CPA 课程列表", LogLevel.Info)]
        public ActionResult CourseCPAList(string cp)
        {
            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            if (cp != null && cp != "" && cp == "1")
            {
                if (Session["ccpage"] != null)
                {
                    string sess = Session["ccpage"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Attname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.AttstartTime = att[2];
                    ViewBag.AttendTime = att[3];
                }
            }
            return View();
        }


        /// <summary>
        /// 课程管理
        /// </summary>
        /// <param name="way">1:集中授课 2 视频授课 3 CPA</param>
        /// <returns></returns>
        // [Filter.SystemLog("课程管理列表", LogLevel.Info)]
        public ActionResult CourseManageList(string p, int way = 1)
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


        // [Filter.SystemLog("CPA成绩详细", LogLevel.Info)]
        public ActionResult CourseCPADetailScoreList(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        // [Filter.SystemLog("CPA成绩导入", LogLevel.Info)]
        public ActionResult CourseCPAListImportScore()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids">已经选择的群组</param>
        /// <returns></returns>
        // [Filter.SystemLog("选择群组", LogLevel.Info)]
        public ActionResult CourseChooseGroup(string ids, int TokenPublishflag = 0, string tempGroupIds = "")
        {
            ViewBag.ChoosedIds = ids;
            ViewBag.TokenPublishflag = TokenPublishflag;
            ViewBag.tempGroupIds = tempGroupIds;
            return View();
        }

        // [Filter.SystemLog("选择调查问卷", LogLevel.Info)]
        public ActionResult CourseChooseSurvey()
        {
            return View();
        }

        /// <summary>
        /// 验证课程名称重名 需求暂时取消
        /// </summary>
        // [Filter.SystemLog("验证课程重名", LogLevel.Info)]
        public JsonResult CheckCourseName(string CourseName, int Id)
        {
            return Json(!_courseBL.IsExistCourseName(CourseName.ReplaceSingleSql(), 2, Id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证课程编号重名
        /// </summary>
        //[Filter.SystemLog("验证课程编号重名", LogLevel.Info)]
        public JsonResult CheckCourseCode(string CourseCode, int Id, int way = 1)
        {
            return Json(!_courseBL.IsExistCourseCode
(CourseCode.ReplaceSingleSql(), 2, Id, way), JsonRequestBehavior.AllowGet);
        }



        //[HttpPost]
        //hidResourceIds 删除资源IDS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        /// <param name="coursePaper"></param>
        /// <param name="hidResourceIds"></param>
        /// <param name="IsCopy">0; 新增课程或者修改课程 等于1 时 为 课程拷贝 </param>
        /// <returns></returns>
        [Filter.SystemLog("提交 编辑 集中授课课程", LogLevel.Info)]
        public JsonResult SubmitEditCourseTogether(Co_Course course, Co_CoursePaper coursePaper, string hidResourceIds, int IsCopy = 0)
        {
            if (course.Id == 0 || IsCopy == 1)
            {
                if (course.StartTime < DateTime.Now)
                {
                    return Json(new
                    {
                        result = 0,
                        content = "课程开始时间不能小于系统当前时间！"
                    }, JsonRequestBehavior.AllowGet);
                }
                course.Year = course.PreCourseTime.Year;
                course.Month = course.PreCourseTime.Year + "-" + (course.PreCourseTime.Month.ToString().Length > 1 ? course.PreCourseTime.Month.ToString() : "0" + course.PreCourseTime.Month.ToString());
                course.CourseFrom = 2;
                course.AfterOrderTimes = 0;
                course.IsDelete = 0;
                course.LastUpdateTime = DateTime.Now;
                course.Way = 1;
                course.AdFlag = course.IsChooseRoom == 2 ? 2 : course.AdFlag;
                course.CourseLengthDistribute = course.courseattend + ";" + course.courseonlinetest + ";" + course.courseafter;
                //添加
                _courseBL.AddCourse(course);
                if (IsCopy == 1)
                {
                    _courseBL.UpdateCourseTimesByCode(course.Code, course.CourseTimes);
                }
                if (course.Id > 0)
                {

                    //TODO 如果课次 大于1 那么复制第二次课次的时候 把 开课时间置空!!  
                    ///当前课次与上一次课次进行比较!   如果修改课次之后 那么课程如何更新!!! 课程资源 更新标准???
                    // 循环添加课程  根据当前的课程编号 查找课程资源 然后进行添加
                    ///关联考试
                    if (course.IsTest == 1)
                    {
                        coursePaper.CourseId = course.Id;
                        coursePaper.IsMain = 0;
                        _courseBL.InsertOrUpdateCourseMainPaper(coursePaper);
                    }
                    return Json(new
                    {
                        result = 1,
                        content = "保存成功！",
                        courseId = course.Id
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //修改
                Co_Course tempCourse = _courseBL.GetCo_Course(course.Id);
                if (tempCourse.Publishflag != 1 && course.StartTime < DateTime.Now)
                {
                    return Json(new
                    {
                        result = 0,
                        content = "课程开始时间不能小于系统当前时间！"
                    }, JsonRequestBehavior.AllowGet);
                }

                tempCourse.CourseName = course.CourseName;
                tempCourse.Code = course.Code;
                tempCourse.Year = course.Year;
                tempCourse.Month = course.Month;
                tempCourse.Day = course.Day;
                tempCourse.OpenLevel = course.OpenLevel;
                tempCourse.IsMust = course.IsMust;
                //tempCourse.Way = course.Way;
                tempCourse.Teacher = course.Teacher;
                tempCourse.StartTime = course.StartTime;
                tempCourse.EndTime = course.EndTime;
                tempCourse.Sort = course.Sort;
                tempCourse.CourseLength = course.CourseLength;
                tempCourse.RoomId = course.RoomId;
                tempCourse.NumberLimited = course.NumberLimited;
                tempCourse.IsCPA = course.IsCPA;
                tempCourse.AdFlag = course.AdFlag == 1 && course.IsChooseRoom == 2 ? 2 : course.AdFlag;
                tempCourse.IsOrder = course.IsOrder;
                tempCourse.IsLeave = course.IsLeave;
                tempCourse.OpenFlag = course.OpenFlag;
                tempCourse.OpenGroupObject = course.OpenGroupObject;
                tempCourse.OpenDepartObject = course.OpenDepartObject;
                tempCourse.IsTest = course.IsTest;
                tempCourse.IsPing = course.IsPing;
                tempCourse.SurveyPaperId = course.SurveyPaperId;
                tempCourse.Memo = course.Memo;
                tempCourse.AttFlag = course.AttFlag;
                tempCourse.IsOpenSub = course.IsOpenSub;

                tempCourse.PreCourseTime = course.PreCourseTime;
                tempCourse.PreAdviceConfigTime = course.PreAdviceConfigTime;
                tempCourse.AfterEvlutionConfigTime = course.AfterEvlutionConfigTime;
                tempCourse.OpenPerson = course.OpenPerson;
                tempCourse.CourseTimes = course.CourseTimes;
                tempCourse.YearPlan = course.YearPlan;
                tempCourse.IsYearPlan = course.IsYearPlan;
                tempCourse.IsSystem = course.IsSystem;
                tempCourse.LastUpdateTime = DateTime.Now;
                tempCourse.IsRT = course.IsRT;

                tempCourse.CourseLengthDistribute = course.courseattend + ";" + course.courseonlinetest + ";" + course.courseafter;
                //tempCourse.CourseFrom = course.CourseFrom;
                //tempCourse.StopOrderFlag = course.StopOrderFlag;
                //tempCourse.StopDucueFlag = course.StopDucueFlag;

                //参数配置
                //tempCourse.ReturnTimes = course.ReturnTimes;
                //tempCourse.AfterOrderTimes = course.AfterOrderTimes;
                //tempCourse.Publishflag = course.Publishflag;
                //tempCourse.LastUpdateTime = course.LastUpdateTime;
                //tempCourse.IsDelete = course.IsDelete;
                tempCourse.LastUpdateTime = DateTime.Now;
                bool newid = _courseBL.UpdateCourse(tempCourse);
                if (course.CourseTimes > 1)//课次大于1 那么存在已经 拷贝的课程
                {
                    _courseBL.UpdateCourseTimesByCode(course.Code, course.CourseTimes);

                }
                ///关联考试
                if (course.IsTest == 1)
                {
                    coursePaper.CourseId = course.Id;
                    coursePaper.IsMain = 0;
                    _courseBL.InsertOrUpdateCourseMainPaper(coursePaper);
                }
                //移除关联资源
                if (hidResourceIds.EndsWith(","))
                {
                    hidResourceIds = hidResourceIds.Substring(0, hidResourceIds.Length - 1);

                }
                DeleteCourseResource(hidResourceIds);
                if (newid)
                    return Json(new
                    {
                        result = 1,
                        content = "保存成功",
                        courseId = course.Id
                    }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 关联资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Filter.SystemLog("提交 新增课程资源", LogLevel.Info)]
        public ContentResult SubmitAddCourseResource(Co_CourseResource model)
        {
            var result = _courseResourceBL.AddCourseResource(model) ? "1" : "0";
            return Content(result);
        }

        /// <summary>
        /// 删除课程关联资源
        /// </summary>
        /// <param name="courseResourceIds">课程关联资源 courseResourceIds</param>
        [Filter.SystemLog("删除 课程关联资源", LogLevel.Info)]
        public void DeleteCourseResource(string courseResourceIds)
        {
            List<string> listStrCourseResource = courseResourceIds.Split(',').ToList();
            foreach (var item in listStrCourseResource)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    _courseResourceBL.DeleteCourseResource(int.Parse(item));
                }
            }
        }

        /// <summary>
        /// 修改课程关联资源 先删除全部
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="strCourseResource">课程关联资源拼接字符串: 形式: resourceName,realName,resourceSize;resourceName,realName.resourceSize</param>
        [Filter.SystemLog("修改课程关联资源 先删除全部", LogLevel.Info)]
        public void ModifyCourseResource(int courseId, string strCourseResource)
        {
            ///先删除
            _courseResourceBL.DeleteCourseResource(courseId);

            if (!string.IsNullOrEmpty(strCourseResource))
            {
                List<string> listStrCourseResource = strCourseResource.Split(';').ToList();
                foreach (var item in listStrCourseResource)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] a = item.Split('|');
                        int indexOfType = a[1].LastIndexOf('.');
                        Co_CourseResource _tempCourseResource =
                            new Co_CourseResource()
                            {
                                ResourceName = a[0],
                                RealName = a[1],
                                ResourceSize = int.Parse(a[2]),
                                ResourceType = 0,
                                CourseId = courseId,
                                LastUpdateTime = DateTime.Now
                            };
                        _courseResourceBL.AddCourseResource(_tempCourseResource);//循环插入
                    }
                }
            }
        }

        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="IsMust"></param>
        /// <param name="IsCPA"></param>
        /// <param name="openLevel"></param>
        /// <param name="way">授课方式 ：0:获取全部 1:集中；2视频；3：部门自学</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        //[Filter.SystemLog("获取列表  公共课程", LogLevel.Info)]
        public JsonResult GetCourseCommonList(string courseName, string StartTime, string EndTime, int IsMust = -1, int IsCPA = -1, int Sort = -1, int way = 0, int pageSize = 20, int pageIndex = 1, int systemId = 0, int PublishFlag = -1)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" CourseFrom=2 ");
            if (way != 0)
            {
                strWhere.AppendFormat(" AND way={0}", way);
            }
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
            if (IsMust != -1)
            {
                strWhere.AppendFormat(" AND IsMust={0}", IsMust);
            }
            if (IsCPA != -1)
            {
                strWhere.AppendFormat(" AND IsCPA={0}", IsCPA);
            }
            if (systemId != 0)
            {
                string strCourseIds = string.Join(",", _SysLinkCourseBL.GetCourseIdListBySystemId(systemId));
                if (!string.IsNullOrEmpty(strCourseIds))
                {
                    strWhere.AppendFormat(" AND Co_Course.Id in({0}) ", strCourseIds);
                }
                else
                {
                    strWhere.AppendFormat(" AND 1=2 ");//该体系下木有课程
                }
            }
            if (PublishFlag != -1)
            {
                strWhere.AppendFormat(" AND PublishFlag={0} ", PublishFlag);
            }
            List<Co_Course> listCourse = new List<Co_Course>();

            int totalCount = 0;
            listCourse = _courseBL.GetCourseCommonList(out totalCount, strWhere.ToString(), (pageIndex - 1) * pageSize, pageSize, "ORDER BY Co_Course.Code,Co_Course.Id DESC");
            int n = 0;
            var itemArray = new object[listCourse.Count()];
            foreach (var item in listCourse)
            {
                var temp = new
                {
                    Id = item.Id,
                    CourseName = item.CourseName.HtmlXssEncode(),
                    CourseLength = item.CourseLength,
                    IsMust = typeof(LiXinModels.Enums.IsMust).GetEnumName(item.IsMust),
                    StartTime = item.StartTime.ToString(),
                    EndTime = item.EndTime.ToString(),
                    SortStr = item.SortStr,
                    TeacherName = item.TeacherName.HtmlXssEncode(),
                    RoomName = item.RoomName.HtmlXssEncode(),
                    Way = item.Way,
                    IsCPA = typeof(LiXinModels.Enums.IsCPA).GetEnumName(item.IsCPA),
                    Publishflag = item.Publishflag,
                    CourseTimes = item.CourseTimes,
                    CourseTimesOrder = item.CourseTimesOrder,
                    TotalCourseTimes = item.TotalCourseTimes
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
        /// 获取课程列表
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="IsMust"></param>
        /// <param name="IsCPA"></param>
        /// <param name="openLevel"></param>
        /// <param name="way">授课方式 ：0:获取全部 1:集中；2视频；3：部门自学</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        // [Filter.SystemLog("获取列表  集中授课", LogLevel.Info)]
        public JsonResult GetCourseTogetherList(string courseName, string StartTime, string EndTime, string IsMust, string IsCPA, int Sort = -1, string isSys = "", string isYear = "", int pageSize = 20, int pageIndex = 1, int systemId = 0, int PublishFlag = -1, string jsRenderSortField = "Code desc")
        {
            if (Session["ctpage"] != null)
            {
                Session.Remove("ctpage");
            }
            Session["ctpage"] = pageIndex + "㉿" + courseName + "㉿" + StartTime + "㉿" + EndTime + "㉿" + IsMust + "㉿" + IsCPA + "㉿" + Sort + "㉿" + isSys + "㉿" + isYear;

            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" CourseFrom=2 AND way=1 ");
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
            if (systemId != 0)
            {
                string strCourseIds = string.Join(",", _SysLinkCourseBL.GetCourseIdListBySystemId(systemId));
                if (!string.IsNullOrEmpty(strCourseIds))
                {
                    strWhere.AppendFormat(" AND Co_Course.Id in({0}) ", strCourseIds);
                }
                else
                {
                    strWhere.AppendFormat(" AND 1=2 ");//该体系下木有课程
                }
            }
            if (PublishFlag != -1)
            {
                strWhere.AppendFormat(" AND PublishFlag={0} ", PublishFlag);
            }
            List<Co_Course> listCourse = new List<Co_Course>();

            int totalCount = 0;
            listCourse = _courseBL.GetCourseTogetherList(out totalCount, strWhere.ToString(), (pageIndex - 1) * pageSize, pageSize, "ORDER BY Co_Course." + jsRenderSortField);
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
                    CourseState = item.CourseState
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
        /// 获取课程列表
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="IsMust"></param>
        /// <param name="IsCPA"></param>
        /// <param name="openLevel"></param>
        /// <param name="way">授课方式 ：0:获取全部 1:集中；2视频；3：CPA</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        //[Filter.SystemLog("获取列表  视频授课", LogLevel.Info)]
        public JsonResult GetCourseVideoList(string courseName, string StartTime, string EndTime, string IsMust, string IsCPA, int Sort = -1, int way = 2, string isSys = "", string isYear = "", int pageSize = 20, int pageIndex = 1, int systemId = 0, int PublishFlag = -1, string jsRenderSortField = "Code desc")
        {
            if (Session["cvpage"] != null)
            {
                Session.Remove("cvpage");
            }
            Session["cvpage"] = pageIndex + "㉿" + courseName + "㉿" + StartTime + "㉿" + EndTime + "㉿" + IsMust + "㉿" + IsCPA + "㉿" + Sort + "㉿" + isSys + "㉿" + isYear;
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" CourseFrom=2 ");
            if (way != 0)
            {
                strWhere.AppendFormat(" AND way={0}", way);
            }
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
            if (systemId != 0)
            {
                string strCourseIds = string.Join(",", _SysLinkCourseBL.GetCourseIdListBySystemId(systemId));
                if (!string.IsNullOrEmpty(strCourseIds))
                {
                    strWhere.AppendFormat(" AND Co_Course.Id in({0}) ", strCourseIds);
                }
                else
                {
                    strWhere.AppendFormat(" AND 1=2 ");//该体系下木有课程
                }
            }
            if (PublishFlag != -1)
            {
                strWhere.AppendFormat(" AND PublishFlag={0} ", PublishFlag);
            }
            List<Co_Course> listCourse = new List<Co_Course>();

            int totalCount = 0;
            listCourse = _courseBL.GetCourseVideoList(out totalCount, strWhere.ToString(), (pageIndex - 1) * pageSize, pageSize, "ORDER BY " + jsRenderSortField);
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
                    IsSystem = item.IsSystemStr,
                    IsYearPlan = item.IsPlanStr,
                    Publishflag = item.Publishflag,
                    CourseTimes = item.CourseTimes,
                    CourseState = item.CourseState
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
        /// 获取课程列表
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="IsMust"></param>
        /// <param name="IsCPA"></param>
        /// <param name="openLevel"></param>
        /// <param name="way">授课方式 ：0:获取全部 1:集中；2视频；3：CPA</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        //[Filter.SystemLog("获取列表  CPA课程", LogLevel.Info)]
        public JsonResult GetCourseCPAList(string courseName, string StartTime, string EndTime, int way = 3, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "LastUpdateTime desc")
        {
            if (Session["ccpage"] != null)
            {
                Session.Remove("ccpage");
            }
            Session["ccpage"] = pageIndex + "㉿" + courseName + "㉿" + StartTime + "㉿" + EndTime;
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" CourseFrom=2  AND way=3 ");
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

            List<Co_Course> listCourse = new List<Co_Course>();

            int totalCount = 0;
            listCourse = _courseBL.GetCourseCPAList(out totalCount, strWhere.ToString(), (pageIndex - 1) * pageSize, pageSize, "ORDER BY " + jsRenderSortField);
            int n = 0;
            var itemArray = new object[listCourse.Count()];
            foreach (var item in listCourse)
            {
                var temp = new
                {
                    Id = item.Id,
                    CourseName = item.CourseName.HtmlXssEncode(),
                    CourseLength = item.CourseLength,
                    StartTime = item.StartTime.ToString("yyyy-MM-dd HH:mm"),
                    EndTime = item.EndTime.ToString("yyyy-MM-dd HH:mm"),
                    CpaTeacher = item.CpaTeacher.HtmlXssEncode(),
                    OpenLevel = item.OpenLevel,
                    TrainDays = item.TrainDays,
                    Publishflag = item.Publishflag,
                    CourseObjectMemo = item.CourseObjectMemo.HtmlXssEncode(),
                    CourseAddress = item.CourseAddress.HtmlXssEncode(),
                    Memo = item.Memo.HtmlXssEncode(),
                    CourseState = item.CourseState,
                    datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
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
        /// 获取课程列表
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="IsMust"></param>
        /// <param name="IsCPA"></param>
        /// <param name="openLevel"></param>
        /// <param name="way">授课方式 ：0:获取全部 1:集中；2视频；3：CPA</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        // [Filter.SystemLog("获取列表  CPA课程 已经发布 成绩导入页面需要", LogLevel.Info)]
        public JsonResult GetCourseCPAListImportScore(string courseName, string StartTime, string EndTime, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = " StartTime desc")
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" CourseFrom=2  AND way=3 AND Publishflag=1 AND EndTime<GetDate() ");
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

            List<Co_Course> listCourse = new List<Co_Course>();

            int totalCount = 0;
            listCourse = _courseBL.GetCourseCPAListImportScore(out totalCount, strWhere.ToString(), (pageIndex - 1) * pageSize, pageSize, "ORDER BY " + jsRenderSortField);
            int n = 0;
            var itemArray = new object[listCourse.Count()];
            foreach (var item in listCourse)
            {
                var temp = new
                {
                    Id = item.Id,
                    CourseName = item.CourseName.HtmlXssEncode(),
                    CourseLength = item.CourseLength,
                    StartTime = item.StartTime.ToString("yyyy-MM-dd HH:mm"),
                    EndTime = item.EndTime.ToString("yyyy-MM-dd HH:mm"),
                    CpaTeacher = item.CpaTeacher.HtmlXssEncode(),
                    OpenLevel = item.OpenLevel,
                    TrainDays = item.TrainDays,
                    Publishflag = item.Publishflag,
                    CourseObjectMemo = item.CourseObjectMemo.HtmlXssEncode(),
                    CourseAddress = item.CourseAddress.HtmlXssEncode(),
                    StateStr = item.StateStr,
                    ImportOverFlag = item.CPAUNImportCount == 0 ? "未导入" : "已导入"
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
        /// 获取某门课程的 成绩详情 未做分页 及 筛选处理
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetCPACourseScoreList(int courseId, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = " LastUpdateTime desc")
        {
            //int totalCount = 0;
            //string strWhere = " AND CourseId= "+courseId;
            //var list = _cpaLearnStatusBL.GetCPACourseScoreList(out totalCount, strWhere, (pageIndex - 1) * pageSize, pageSize, "ORDER BY Co_Course.Code,Co_Course.Id DESC");
            var list = _cpaLearnStatusBL.GetCPACourseScoreList(courseId, jsRenderSortField);
            int n = 0;
            int totalcount = list.Count;
            //list.AsQueryable().OrderBy(jsRenderSortField);
            list = list.Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();
            var itemArray = new object[list.Count];
            foreach (var item in list)
            {
                var temp = new
                {
                    Id = item.Id,
                    DeptName = item.DeptName.HtmlXssEncode(),
                    Realname = item.Realname,
                    CPANO = item.CPANO,
                    GetLength = item.GetLength

                };
                itemArray[n] = temp;
                n++;
            }

            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = totalcount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据月份加载课程
        /// </summary>
        /// <param name="monthstr"></param>
        /// <param name="way">1:集中；2视频；3：CPA</param>
        /// <returns></returns>
        //  [Filter.SystemLog("根据月份加载课程", LogLevel.Info)]
        public JsonResult GetMonthPlanCourse(string monthstr, int way = 1)
        {
            string strwhere = string.Format("  CourseFrom=1 AND Month='{0}' AND Way='{1}' ", monthstr, way);
            int total = 0;
            List<Co_Course> listCourse = _courseBL.GetCourseCommonList(out total, strwhere, 0, int.MaxValue);
            int n = 0;
            var itemArray = new object[listCourse.Count()];
            foreach (var item in listCourse)
            {
                var temp = new
                {
                    Id = item.Id,
                    Code = item.Code,
                    CourseName = item.CourseName,
                    OpenLevel = item.OpenLevel,
                    Year = item.Year,
                    Month = item.Month,
                    StartTime = item.Day,
                    EndTime = item.OpenLevel,
                    IsMust = item.IsMust,
                    TeacherName = item.TeacherName,
                    Teacher = item.Teacher,
                    PreCourseTime = item.PreCourseTime.ToString("yyyy-MM-dd HH:mm"),
                    CourseLength = item.CourseLength,
                    IsCPA = item.IsCPA,
                    RoomId = item.RoomId,
                    IsSystem = item.IsSystem,
                    IsYearPlan = item.IsYearPlan
                };
                itemArray[n] = temp;
                n++;
            }
            return Json(new
            {
                result = 1,
                dataList = itemArray,
                recordCount = total
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 视频授课

        /// <summary>
        /// 视频授课
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        //    [Filter.SystemLog("视频授课", LogLevel.Info)]
        public ActionResult EditCourseVideo(int? Id, int Publishflag = 0)
        {
            //必修学时
            ViewBag.MustCourseLength = AllSystemConfigs.Find(s => s.ConfigType == 18).ConfigValue.Split(';')[0];
            //选修学时
            ViewBag.ChoosableCourseLength = AllSystemConfigs.Find(s => s.ConfigType == 18).ConfigValue.Split(';')[1];

            //课程结束后 多久时间内 允许考试
            ViewBag.RefHour = AllSystemConfigs.Find(s => s.ConfigType == 26).ConfigValue.Split(';')[0];
            //考试时长
            ViewBag.RefLength = AllSystemConfigs.Find(s => s.ConfigType == 26).ConfigValue.Split(';')[1];
            //测试次数
            ViewBag.MaxTestTimes = AllSystemConfigs.Find(s => s.ConfigType == 27).ConfigValue;

            //课前建议时间外
            ViewBag.RefPreAdviceConfigTime = AllSystemConfigs.Find(s => s.ConfigType == 29).ConfigValue.Split(';')[0];
            //课后评估时间内
            ViewBag.RefAfterEvlutionConfigTime = AllSystemConfigs.Find(s => s.ConfigType == 29).ConfigValue.Split(';')[1];
            //培训级别
            ViewBag.trainGrade = _sys_TrainBL.GetAllTrainGrade();


            ViewBag.Publishflag = Publishflag;

            Co_Course course = new Co_Course();
            if (Id.HasValue)
            {
                course = _courseBL.GetCo_Course(Id.Value);
                if (course == null)
                {
                    course = new Co_Course();
                    course.ReturnTimes = Convert.ToInt32(AllSystemConfigs.Find(s => s.ConfigType == 25).ConfigValue);
                }
                else
                {
                    if (course.TeacherIsTeacher == 0 || course.TeacherIsDelete == 1)
                    {
                        course.Teacher = "";
                        course.TeacherName = "";
                    }
                    List<Co_CourseResource> listResource = _courseResourceBL.GetCourseResourceList(Id.Value);
                    ViewBag.SCROMResource = listResource.Where(c => c.ResourceType == 3).FirstOrDefault();//SCROM 课件
                    ViewBag.CourseAttachList = listResource.Where(c => c.ResourceType == 1);//附件
                    ViewBag.CourseVideoList = listResource.Where(c => c.ResourceType == 4);//视频
                    ViewBag.allID = string.Join(",", listResource.Where(c => c.ResourceType == 4).Select(p => p.PackId));
                    if (course.IsTest == 1)
                    {
                        ViewBag.CoursePaper = _coursePaperBL.GetCo_CourseMainPaper(Id.Value);
                        if (ViewBag.CoursePaper == null)
                        {
                            ViewBag.CoursePaper = new Co_CoursePaper();
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
                    if (course.IsPing == 1)
                    {
                        string[] surveyArray = course.SurveyPaperId.Split(';');
                        bool flag = false;
                        foreach (var item in surveyArray)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {//时间问题，判断拙劣。不影响功能。
                                string[] itemArray = item.Split(',');
                                LiXinModels.Survey.Survey_Exampaper surveyExamPaper = _surveyExampaperBL.GetServeyExamPaper(Convert.ToInt32(itemArray[1]));
                                if (surveyExamPaper == null || surveyExamPaper.IsDelete == 1)
                                {
                                    if (itemArray[0] == "0")
                                    {
                                        ViewBag.SurveyExampaperName0 = "";
                                        //   
                                    }
                                    if (itemArray[0] == "2")
                                    {
                                        ViewBag.SurveyExampaperName2 = "";
                                    }
                                }
                                else
                                {
                                    flag = true;
                                    if (itemArray[0] == "0")
                                    {
                                        ViewBag.SurveyExampaperId0 = surveyExamPaper.ExampaperID;
                                        ViewBag.SurveyExampaperName0 = surveyExamPaper.ExamTitle;
                                    }
                                    if (itemArray[0] == "2")
                                    {
                                        ViewBag.SurveyExampaperId2 = surveyExamPaper.ExampaperID;
                                        ViewBag.SurveyExampaperName2 = surveyExamPaper.ExamTitle;
                                    }
                                }

                            }
                        }
                        if (!flag)
                        {
                            course.IsPing = 0;
                        }
                    }

                    if (course.OpenFlag == 1)//群组
                    {
                        int total = 0;
                        string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                        ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                    }
                    if (course.OpenFlag == 2)//组织结构
                    {
                        string strWhere = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                        ViewBag.Departs = _deptBL.GetAllList(strWhere);
                    }
                    if (course.OpenFlag == 3)//群组+组织结构
                    {
                        int total = 0;
                        string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                        ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                        string strWhereDept = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                        ViewBag.Departs = _deptBL.GetAllList(strWhereDept);
                    }
                }
            }
            else
            {
                course.ReturnTimes = Convert.ToInt32(AllSystemConfigs.Find(s => s.ConfigType == 25).ConfigValue);
            }
            ViewBag.ClassRoomList = _classRoomBL.GetRoomList();
            ViewBag.YearPlanList = _trYearPlanBL.GetAllYear(" PublishFlag=1 AND IsDelete=0 ").Select(t => t.Year);
            return View(course);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        /// <param name="coursePaper"></param>
        /// <param name="strhidResourceIds">移除资源的ID编号</param>
        /// <param name="deleteType">移除类型 -1:新增操作,此时无意义  0:视频链接;   1: SCROM</param>
        /// <param name="StrResourceIds">上传资源的 字符穿</param>
        /// <returns></returns>
        [Filter.SystemLog("提交编辑  视频课程", LogLevel.Info)]
        public JsonResult SubmitEditCourseVideo(Co_Course course, Co_CoursePaper coursePaper, string StrResourceIds, string strhidResourceIds = "", int deleteType = -1)
        {
            if (course.Id == 0)
            {
                //List<Co_CourseResource> listResource = _courseResourceBL.GetCourseResourceList(course.Id);
                //ViewBag.CourseAttachList = listResource.Where(c => c.ResourceType == 1);//附件

                course.Year = course.PreCourseTime.Year;
                course.Month = course.PreCourseTime.Year + "-" + (course.PreCourseTime.Month.ToString().Length > 1 ? course.PreCourseTime.Month.ToString() : "0" + course.PreCourseTime.Month.ToString());
                course.CourseFrom = 2;
                course.AfterOrderTimes = 0;
                course.IsDelete = 0;
                course.Way = 2;
                course.LastUpdateTime = DateTime.Now;
                course.CourseLengthDistribute = "";
                //添加
                _courseBL.AddCourse(course);

                AddVideoCourseResource(course, StrResourceIds);

                if (course.Id > 0)
                {
                    ///关联考试
                    if (course.IsTest == 1)
                    {
                        coursePaper.CourseId = course.Id;
                        coursePaper.IsMain = 0;
                        _courseBL.InsertOrUpdateCourseMainPaper(coursePaper);
                    }
                    return Json(new
                    {
                        result = 1,
                        content = "保存成功！",
                        courseId = course.Id
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //修改
                Co_Course tempCourse = _courseBL.GetCo_Course(course.Id);
                tempCourse.CourseName = course.CourseName;
                tempCourse.ReturnTimes = course.ReturnTimes;
                tempCourse.Code = course.Code;
                tempCourse.Year = course.Year;
                tempCourse.Month = course.Month;
                tempCourse.Day = course.Day;
                tempCourse.OpenLevel = course.OpenLevel;
                tempCourse.IsMust = course.IsMust;
                tempCourse.IsOpenSub = course.IsOpenSub;
                //tempCourse.Way = course.Way;
                tempCourse.Teacher = course.Teacher;
                tempCourse.StartTime = course.StartTime;
                tempCourse.EndTime = course.EndTime;
                tempCourse.Sort = course.Sort;
                tempCourse.CourseLength = course.CourseLength;
                tempCourse.RoomId = course.RoomId;
                tempCourse.NumberLimited = course.NumberLimited;
                tempCourse.IsCPA = course.IsCPA;
                tempCourse.IsOrder = course.IsOrder;
                tempCourse.IsLeave = course.IsLeave;
                tempCourse.OpenFlag = course.OpenFlag;
                tempCourse.OpenGroupObject = course.OpenGroupObject;
                tempCourse.OpenDepartObject = course.OpenDepartObject;
                tempCourse.IsTest = course.IsTest;
                tempCourse.IsPing = course.IsPing;
                tempCourse.SurveyPaperId = course.SurveyPaperId;
                tempCourse.Memo = course.Memo;
                tempCourse.PreCourseTime = course.PreCourseTime;
                tempCourse.PreAdviceConfigTime = course.PreAdviceConfigTime;
                tempCourse.AfterEvlutionConfigTime = course.AfterEvlutionConfigTime;
                tempCourse.LastUpdateTime = DateTime.Now;
                tempCourse.IsSystem = course.IsSystem;
                // tempCourse.IsYearPlan = course.IsYearPlan;
                tempCourse.YearPlan = course.YearPlan;
                tempCourse.CourseLengthDistribute = "";
                bool newid = _courseBL.UpdateCourse(tempCourse);

                ///关联考试
                if (course.IsTest == 1)
                {
                    coursePaper.CourseId = course.Id;
                    coursePaper.IsMain = 0;
                    _courseBL.InsertOrUpdateCourseMainPaper(coursePaper);
                }
                //移除关联资源
                if (strhidResourceIds.EndsWith(","))
                {
                    strhidResourceIds = strhidResourceIds.Substring(0, strhidResourceIds.Length - 1);

                }
                DeleteCourseResource(strhidResourceIds);
                AddVideoCourseResource(course, StrResourceIds);

                if (newid)
                    return Json(new
                    {
                        result = 1,
                        content = "保存成功",
                        courseId = course.Id
                    }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }
        }
        ///添加 资源关联
        ///StrResourceIds packId | resourceName + "|" + realName + "|" + ResourceSize|ResourceType;
        [Filter.SystemLog("添加视频课程资源", LogLevel.Info)]
        private void AddVideoCourseResource(Co_Course course, string StrResourceIds)
        {

            if (!string.IsNullOrEmpty(StrResourceIds))
            {
                List<string> listStrCourseResource = StrResourceIds.Split(';').ToList();
                foreach (var item in listStrCourseResource)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] a = item.Split('|');
                        int indexOfType = a[1].LastIndexOf('.');
                        Co_CourseResource _tempCourseResource =
                            new Co_CourseResource()
                            {
                                PackId = (a[4] == "3" || a[4] == "4") ? int.Parse(a[0]) : 0,
                                ResourceName = a[1],
                                RealName = a[2],
                                ResourceSize = int.Parse(a[3]),
                                ResourceType = int.Parse(a[4]),
                                CourseId = course.Id,
                                IsDelete = 0,
                                LastUpdateTime = DateTime.Now
                            };
                        _courseResourceBL.AddCourseResource(_tempCourseResource);//循环插入
                    }
                }
            }
        }



        //文件上传
        //[Filter.SystemLog("SCROM 文件上传", LogLevel.Info)]
        public ContentResult UploadFileAction(HttpPostedFileBase FileData, string folder, int detailFlag = 0)
        {
            string filename = "";
            string resultName = "";
            int packId = 0;
            if (null != FileData)
            {
                try
                {
                    if (FileData.ContentLength > 1073741824)
                    {
                        resultName = "Scorm文件大小不能超过1G";
                    }
                    else
                    {
                        filename = Path.GetFileName(FileData.FileName); //获得文件名
                        string fullPathname = Path.Combine(folder, filename);
                        //文件后缀名
                        string suffix = FileData.FileName.Substring(FileData.FileName.LastIndexOf(".") + 1).ToLower();
                        resultName = Guid.NewGuid() + "." + suffix;

                        saveFile(FileData, folder, resultName);
                        packId = UpLoadScorm(HttpContext.Server.MapPath(folder), resultName);

                        if (packId == 0)
                        {
                            resultName = "ScormError";
                        }
                        else
                        {
                            if (detailFlag == 1)
                            {
                                resultName = FileData.FileName + "|" + resultName + "|" + FileData.ContentLength / 1024;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    resultName = "ScormError";
                }
            }
            return Content(packId + "|" + resultName);
        }

        public bool saveFile(HttpPostedFileBase postedFile, string filepath, string saveName)
        {
            bool result = false;
            string a = HttpContext.Server.MapPath(filepath);
            if (!Directory.Exists(a))
            {
                Directory.CreateDirectory(a);
            }
            try
            {
                postedFile.SaveAs((a + "\\" + saveName));
                result = true;
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
            return result;
        }

        public Stream FileToStream(string filepath)
        {
            Stream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return stream;
        }

        private int UpLoadScorm(string pathname, string filesname)
        {
            var scormLearn = new ScormLearn();
            int packID = scormLearn.StorageWareForPackId(filesname, FileToStream(pathname + filesname));
            return packID;
        }


        #endregion

        #region CPA 课程
        [Filter.SystemLog("编辑 CPA 课程", LogLevel.Info)]

        public ActionResult EditCourseCPA(int? Id, int Publishflag = 0)
        {
            //必修学时
            ViewBag.MustCourseLength = AllSystemConfigs.Find(s => s.ConfigType == 18).ConfigValue.Split(';')[0];
            //选修学时
            ViewBag.ChoosableCourseLength = AllSystemConfigs.Find(s => s.ConfigType == 18).ConfigValue.Split(';')[1];
            //培训级别
            ViewBag.trainGrade = _sys_TrainBL.GetAllTrainGrade();

            ViewBag.Publishflag = Publishflag;

            Co_Course course = new Co_Course();
            if (Id.HasValue)
            {
                course = _courseBL.GetCo_Course(Id.Value);
                if (course == null)
                {
                    course = new Co_Course();
                }
                else
                {
                    List<Co_CourseResource> listResource = _courseResourceBL.GetCourseResourceList(Id.Value);
                    ViewBag.CourseAttachList = listResource.Where(c => c.ResourceType == 1);//附件

                    if (course.OpenFlag == 1)//群组
                    {
                        int total = 0;
                        string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                        ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                    }
                    if (course.OpenFlag == 2)//组织结构
                    {
                        string strWhere = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                        ViewBag.Departs = _deptBL.GetAllList(strWhere);
                    }
                    if (course.OpenFlag == 3)//群组+组织结构
                    {
                        int total = 0;
                        string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                        ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                        string strWhereDept = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                        ViewBag.Departs = _deptBL.GetAllList(strWhereDept);
                    }
                }
            }
            ViewBag.YearPlanList = _trYearPlanBL.GetAllYear(" PublishFlag=1 AND IsDelete=0 ").Select(t => t.Year);
            return View(course);
        }

        [Filter.SystemLog("导入 CPA 课程成绩模版", LogLevel.Info)]
        public ActionResult ImportCourseCPATemplate(int Id)
        {
            ViewBag.CourseId = Id;
            return View();
        }

        /// <summary>
        /// 判断课程开始时间是否小于服务器当前时间,如果小于返回 0
        /// </summary>
        /// <param name="statrTime"></param>
        /// <returns></returns>
        public JsonResult JudgeStartTime(string statrTime)
        {
            if (DateTime.Parse(statrTime) < DateTime.Now)
            {
                var content = "课程开始时间不能小于系统当前时间！";
                return Json(new
                {
                    result = 0,
                    content = content
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);

            }

        }

        //[HttpPost]
        //hidResourceIds 删除资源IDS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="course"></param>
        /// <param name="coursePaper"></param>
        /// <param name="hidResourceIds"></param>
        /// <param name="IsCopy">0; 新增课程或者修改课程 不等于0 时 为 课程拷贝</param>
        /// <returns></returns>
        [Filter.SystemLog("提交编辑 CPA 课程", LogLevel.Info)]
        public JsonResult SubmitEditCourseCPA(Co_Course course, string hidResourceIds)
        {


            if (course.Id == 0)
            {
                course.Year = course.PreCourseTime.Year;
                course.Month = course.PreCourseTime.Year + "-" + (course.PreCourseTime.Month.ToString().Length > 1 ? course.PreCourseTime.Month.ToString() : "0" + course.PreCourseTime.Month.ToString());
                course.CourseFrom = 2;
                course.AfterOrderTimes = 0;
                course.IsDelete = 0;
                course.LastUpdateTime = DateTime.Now;
                course.PreCourseTime = DateTime.Now;
                course.Way = 3;
                course.IsOrder = 1;
                course.CourseLengthDistribute = "";
                //添加
                _courseBL.AddCourse(course);
                if (course.Id > 0)
                {
                    return Json(new
                    {
                        result = 1,
                        content = "保存成功！",
                        courseId = course.Id
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //修改
                Co_Course tempCourse = _courseBL.GetCo_Course(course.Id);
                tempCourse.CourseName = course.CourseName;
                tempCourse.Code = course.Code;
                tempCourse.Year = course.PreCourseTime.Year;
                tempCourse.Month = course.PreCourseTime.Year + "-" + (course.PreCourseTime.Month.ToString().Length > 1 ? course.PreCourseTime.Month.ToString() : "0" + course.PreCourseTime.Month.ToString());
                tempCourse.Day = course.Day;
                tempCourse.OpenLevel = course.OpenLevel;
                tempCourse.IsMust = course.IsMust;
                //tempCourse.Way = course.Way;
                tempCourse.Teacher = course.Teacher;
                tempCourse.StartTime = course.StartTime;
                tempCourse.EndTime = course.EndTime;
                tempCourse.CourseLength = course.CourseLength;
                tempCourse.NumberLimited = course.NumberLimited;
                tempCourse.OpenFlag = course.OpenFlag;
                tempCourse.OpenGroupObject = course.OpenGroupObject;
                tempCourse.Memo = course.Memo;
                tempCourse.PreCourseTime = course.PreCourseTime;
                tempCourse.PreAdviceConfigTime = course.PreAdviceConfigTime;
                tempCourse.AfterEvlutionConfigTime = course.AfterEvlutionConfigTime;

                tempCourse.CpaTeacher = course.CpaTeacher;
                tempCourse.TrainDays = course.TrainDays;
                tempCourse.CourseObjectMemo = course.CourseObjectMemo;
                tempCourse.CourseAddress = course.CourseAddress;
                tempCourse.PreCourseTime = DateTime.Now;
                tempCourse.LastUpdateTime = DateTime.Now;
                tempCourse.YearPlan = course.YearPlan;
                tempCourse.CourseLengthDistribute = "";

                tempCourse.courseProvide = course.courseProvide;
                tempCourse.studyRequirement = course.studyRequirement;
                tempCourse.remark = course.remark;

                bool newid = _courseBL.UpdateCourse(tempCourse);

                //移除关联资源
                if (!string.IsNullOrEmpty(hidResourceIds))
                {
                    if (hidResourceIds.EndsWith(","))
                    {
                        hidResourceIds = hidResourceIds.Substring(0, hidResourceIds.Length - 1);

                    }
                    DeleteCourseResource(hidResourceIds);
                }

                if (newid)
                    return Json(new
                    {
                        result = 1,
                        content = "保存成功",
                        courseId = course.Id
                    }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        ///  CPA 课程成绩模版
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("导入 CPA 课程成绩模版", LogLevel.Info)]
        public ContentResult SubmitImport(HttpPostedFileBase FileData, string folder, int courseId)
        {
            string filename = "";
            string resultName = "";
            string result = "0";
            if (null != FileData)
            {
                try
                {
                    filename = Path.GetFileName(FileData.FileName); //获得文件名
                    string fullPathname = Path.Combine(folder, filename);//文件后缀名
                    string suffix = FileData.FileName.Substring(FileData.FileName.LastIndexOf(".") + 1).ToLower();
                    resultName = Guid.NewGuid() + "." + suffix;
                    saveFile(FileData, folder, resultName);
                    List<DataTable> dtList = new Spreadsheet().LoadExcel(HttpContext.Server.MapPath(folder) + resultName);
                    if (dtList[0].Rows.Count > 0)
                    {
                        List<Sys_User> listAllUser = _userBL.GetList();
                        for (int i = 0; i < dtList[0].Rows.Count; i++)
                        {
                            Sys_User user = listAllUser.Where(u => u.CPANo == dtList[0].Rows[i]["注册会计师编号"].ToString()).FirstOrDefault();
                         
                            if (user != null)
                            {
                                Cl_CpaLearnStatus CpaLearnmodel = _cpaLearnStatusBL.GetCl_CpaLearnStatusByCourseId(courseId, user.UserId, "CpaFlag=1");
                                if (CpaLearnmodel != null)
                                {
                                    _cpaLearnStatusBL.UpdateCpaLearnStatus(courseId, user.UserId, Convert.ToInt32(dtList[0].Rows[i]["获取学时"].ToString()));
                                }
                                else
                                {
                                    CpaLearnmodel = new Cl_CpaLearnStatus();
                                    CpaLearnmodel.CourseID = courseId;
                                    CpaLearnmodel.UserID = user.UserId;
                                    CpaLearnmodel.GetLength = Convert.ToInt32(dtList[0].Rows[i]["获取学时"].ToString());
                                    CpaLearnmodel.LastUpdateTime = DateTime.Now;
                                    CpaLearnmodel.CpaFlag = 1;
                                    CpaLearnmodel.GradeStatus = 1;
                                    _cpaLearnStatusBL.SubscribeCPALearnStatus(CpaLearnmodel);
                                }
                            }

                            //IAtt.AddAttendce(id, Convert.ToInt32(dtList[0].Rows[i]["人员ID"].ToString()), dtList[0].Rows[i]["课程开始考勤时间"].ToString(), dtList[0].Rows[i]["课程结束考勤时间"].ToString());
                        }
                    }
                    result = "1";
                }
                catch (Exception ex)
                {
                    result = "0";
                }
            }
            return Content(result);
        }


        #endregion

        #region 发布/删除课程

        /// <summary>
        /// 修改课程单一状态
        /// </summary>
        /// <param name="flag">0:删除 1:发布</param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [Filter.SystemLog("修改课程单一状态 0:删除 1:发布", LogLevel.Info)]
        public JsonResult SubmitModifySingleCourse(int flag, string cl, int courseId, int way = 0)
        {
            int page = 1;
            string sess = "";
            if (cl == "t")
            {
                if (Session["ctpage"] != null)
                {
                    sess = Session["ctpage"].ToString();
                }
            }
            else if (cl == "t")
            {
                if (Session["cvpage"] != null)
                {
                    sess = Session["cvpage"].ToString();
                }
            }
            else if (cl == "c")
            {
                if (Session["ccpage"] != null)
                {
                    sess = Session["ccpage"].ToString();
                }
            }
            if (sess != "")
            {
                string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                page = Convert.ToInt32(att[0]);
            }
            if (flag == 1)
            {
                Co_Course course = _courseBL.GetCo_Course(courseId);
                Dictionary<int, string> dic = PublishValidate(course);
                if (dic.Count > 0)
                {
                    var dic_content = "课程信息有误，详细如下：<br/>";
                    foreach (var item in dic)
                    {
                        dic_content += item.Value + "<br/>";
                    }
                    return Json(new
                    {
                        result = 0,
                        content = dic_content
                    }, JsonRequestBehavior.AllowGet);

                }
            }
            if (_courseBL.ModifySingleCourse(flag, courseId))
            {
                if (way > 0)
                {
                    Co_Course course = _courseBL.GetCo_Course(courseId);
                    if (!string.IsNullOrEmpty(course.OpenPerson))
                    {
                        //课程特殊人群在课程发布时报名
                        _courseOrderBL.AddSpecialCrowdUserToCourse(courseId, course.CourseName, course.StartTime, course.EndTime, 2, course.OpenPerson, AllSystemConfigs.Find(p => p.ConfigType == 21).ConfigValue.GetDouble());

                        var content = GetFormworkContent(0);
                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            var userList = _userBL.GetList(" sys_user.userid in (select id from dbo.F_SplitIDs('" + course.OpenPerson + "')) and sys_user.isteacher< 2 and sys_user.isdelete = 0 and (sys_user.realname is not null or sys_user.realname <> '')");
                            var messList = new List<KeyValuePair<string, string>>();
                            var emailList = new List<KeyValuePair<string, string>>();
                            for (int i = 0; i < userList.Count; i++)
                            {
                                if (!string.IsNullOrWhiteSpace(userList[i].Realname))
                                {
                                    var c = string.Format(content,
                                                        userList[i].Realname,
                                                        DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                                                        course.CourseName,
                                                        course.StartTime.ToString("yyyy-MM-dd HH:mm"));
                                    if (!string.IsNullOrWhiteSpace(userList[i].MobileNum))
                                        messList.Add(new KeyValuePair<string, string>(userList[i].MobileNum, c));
                                    if (!string.IsNullOrWhiteSpace(userList[i].Email))
                                        emailList.Add(new KeyValuePair<string, string>(userList[i].Email, c));
                                }
                            }
                            if (messList.Count > 0)
                                SendMessage(messList);
                            if (emailList.Count > 0)
                                SendEmail(emailList, "有人给代报了课程，快来看看吧！");
                        }
                    }

                }
                return Json(new
                {
                    result = 1,
                    content = flag > 0 ? "发布成功！" : "删除成功！",
                    indexpage = page
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                result = 0,
                content = flag > 0 ? "课程开始时间不能小于系统当前时间！" : "删除失败！"
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 发布课程验证
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public Dictionary<int, string> PublishValidate(Co_Course course)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            //判断  1 开放级别 2 授课讲师 3 教室 4 群组 5 组织结构 6 在线测试试卷 7 课后问卷评估
            string[] levelArray = { };
            if (!string.IsNullOrEmpty(course.OpenLevel))
            {
                levelArray = course.OpenLevel.Split(',');
            }
            //else
            //{
            //    levelArray = course.OpenLevel;
            //}


            List<string> listGrade = _sys_TrainBL.GetAllTrainGrade();

            //判断培训级别
            int levelCounts = 0;
            foreach (var item in levelArray)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    if (listGrade.Contains(item))
                    {
                        levelCounts++;
                    }
                }
            }
            //if (levelCounts == 0)
            //{
            //    dic.Add(1, "培训级别无效");//
            //}
            if (course.Way == 1 || course.Way == 2)//集中授课  
            {


                if (course.Way == 1)
                {
                    var teacher = _userBL.Get(int.Parse(course.Teacher));
                    if (teacher == null || teacher.IsDelete == 1 || teacher.IsTeacher == 0)
                        dic.Add(2, "授课讲师无效");// //授课讲师 无效
                    var room = _classRoomBL.GetRoom(course.RoomId);
                    if (room == null || room.IsDelete == 1)
                        dic.Add(3, "所选教室无效");//教室 无效
                }

                if (course.OpenFlag == 1 || course.OpenFlag == 3)//包含群组
                {
                    int groupCount = 0;
                    string[] groupArray = course.OpenGroupObject.Split(',');
                    int total = 0;
                    var listGroup = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, " 1=1 ");
                    foreach (var item in groupArray)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            if (listGroup.Where(g => g.GroupId == int.Parse(item) && g.IsDelete == 0).FirstOrDefault() != null)
                            {
                                groupCount++;
                            }
                        }
                    }
                    if (groupCount == 0)
                        dic.Add(4, "群组无效");//教室 无效 return 4;// 群组 无效
                }

                if (course.OpenFlag == 2 || course.OpenFlag == 3)//包含组织结构
                {
                    int deptCount = 0;
                    if (!string.IsNullOrEmpty(course.OpenDepartObject))
                    {
                        string[] deptArray = course.OpenDepartObject.Split(',');
                        var listDept = _deptBL.GetAllList(" IsDelete=0 ");
                        foreach (var item in deptArray)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                if (listDept.Where(g => g.DepartmentId == int.Parse(item)).FirstOrDefault() != null)
                                {
                                    deptCount++;
                                }
                            }
                        }
                        if (deptCount == 0)
                            dic.Add(5, "组织结构无效");//组织结构 无效
                    }
                }

                if (course.IsTest == 1)
                {
                    var coursePaper = _coursePaperBL.GetCo_CourseMainPaper(course.Id);
                    var paper = _exampaperBL.GetExampaper(coursePaper.PaperId);
                    if (paper == null || paper.Status == 1)
                        dic.Add(5, "课程关联试卷无效");//课程关联试卷 无效
                }
                if (course.IsPing == 1)
                {

                    string[] surveyArray = course.SurveyPaperId.Split(';');
                    bool flag = false;
                    string strError = "";
                    foreach (var item in surveyArray)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {//时间问题，判断拙劣。不影响功能。
                            string[] itemArray = item.Split(',');
                            LiXinModels.Survey.Survey_Exampaper surveyExamPaper = _surveyExampaperBL.GetServeyExamPaper(Convert.ToInt32(itemArray[1]));
                            if (surveyExamPaper == null || surveyExamPaper.IsDelete == 1)
                            {
                                flag = true;
                                strError += itemArray[0] == "0" ? "课程问卷评估无效 " : "讲师问卷评估无效";//课程关联试卷 无效
                            }
                        }
                    }
                    if (flag)
                    {
                        dic.Add(7, strError);
                    }
                }
            }


            if (course.Way == 2)//视频课程  部分已经归纳值上方代码  如果其他十分特殊的可以单独处理
            {
            }
            if (course.Way == 3)//CPA课程  暂无特殊发布验证
            {

            }

            //课程属性验证全部通过 dic.Count  为0
            return dic;

        }
        #endregion

        /// <summary>
        /// 查询预定CPA课程人员
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns></returns>
        public JsonResult GetCpaNoStart(int CourseId)
        {
            int page = 1;

            string sess = "";


            if (Session["ccpage"] != null)
            {
                sess = Session["ccpage"].ToString();
            }

            if (sess != "")
            {
                string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                page = Convert.ToInt32(att[0]);
            }

            var num = _courseBL.ModifySingleCourse(0, CourseId) ? 1 : 0;
            //删除预定人员方法
            //_cpaLearnStatusBL.DeleteByCourseIdAndCpaFlag(CourseId, 1);

            return Json(new
            {
                result = num,
                page = page,
                content = num > 0 ? "删除成功！" : "删除失败！",

            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FunCourseCpaSendMessage(int courseId, string userIds, int cpaflag = 3)
        {
            Co_Course course = _courseBL.GetCo_Course(courseId);

            try
            {
                var content = GetFormworkContent(9);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var userList = _userBL.GetList(" sys_user.userid in (select id from dbo.F_SplitIDs('" + userIds + "')) and sys_user.isteacher< 2 and sys_user.isdelete = 0 and (sys_user.realname is not null or sys_user.realname <> '')");

                    var messList = new List<KeyValuePair<string, string>>();
                    var emailList = new List<KeyValuePair<string, string>>();

                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(userList[i].Realname))
                        {
                            var c = string.Format(content,
                                                userList[i].Realname,
                                                course.CourseName,
                                                DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                            if (!string.IsNullOrWhiteSpace(userList[i].MobileNum))
                                messList.Add(new KeyValuePair<string, string>(userList[i].MobileNum, c));
                            if (!string.IsNullOrWhiteSpace(userList[i].Email))
                                emailList.Add(new KeyValuePair<string, string>(userList[i].Email, c));
                        }
                    }
                    if (messList.Count > 0)
                        SendMessage(messList);
                    if (emailList.Count > 0)
                        SendEmail(emailList, "课程已删除，注意查看！");
                }
                if (cpaflag == 3)
                {
                    //删除预定人员
                    _cpaLearnStatusBL.DeleteByCourseIdAndCpaFlag(courseId, 1);
                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 删除已经预定CPA课程人员
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public JsonResult FdeleteCpalearnStatus(int courseid)
        {
            int num = _cpaLearnStatusBL.DeleteByCourseIdAndCpaFlag(courseid, 1) ? 1 : 0;
            return Json(num, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 给报名的人员发送短信
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="type">1 集中  2视频 3cpa </param>
        /// <returns></returns>
        public ActionResult CourseCPASendMessage(int courseid, int type = 3)
        {
            ViewBag.courseid = courseid;
            ViewBag.type = type;
            return View();
        }



        #region 详情页面 集中+ 视频 +CPA

        // [Filter.SystemLog("集中授课 课程详情", LogLevel.Info)]
        public ActionResult CourseTogetherDetial(int Id)
        {
            var course = _courseBL.GetCo_Course(Id);
            if (course == null || course.IsDelete == 1)
            {
                course = new Co_Course();
                ViewBag.MsgTips = "该课程不存在或已被删除！";
            }
            else
            {
                List<Co_CourseResource> listResource = _courseResourceBL.GetCourseResourceList(Id);
                ViewBag.CourseResourceList = listResource.Where(c => c.ResourceType == 0);//资源
                ViewBag.CourseAttachList = listResource.Where(c => c.ResourceType == 1);//附件
                if (course.IsTest == 1)
                {
                    ViewBag.CoursePaper = _coursePaperBL.GetCo_CourseMainPaper(Id);
                    if (ViewBag.CoursePaper == null)
                    {
                        ViewBag.CoursePaper = new Co_CoursePaper();
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
                if (course.IsPing == 1)
                {
                    if (!string.IsNullOrWhiteSpace(course.SurveyPaperId))
                    {
                        var arr = course.SurveyPaperId.Split(';');
                        if (!string.IsNullOrWhiteSpace(arr[0]))
                            course.AfterCourseAssess = _surveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[0].Split(',')[1]));
                        if (!string.IsNullOrWhiteSpace(arr[1]))
                            course.AfterCourseTeacherExam =
                                _surveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[1].Split(',')[1]));
                    }


                    //ViewBag.SurveyExampaperId0 = 0;
                    //ViewBag.SurveyExampaperId2 = 0;
                    //string[] surveyArray = course.SurveyPaperId.Split(';');
                    //bool flag = false;
                    //foreach (var item in surveyArray)
                    //{
                    //    if (!string.IsNullOrEmpty(item))
                    //    {//时间问题，判断拙劣。不影响功能。
                    //        string[] itemArray = item.Split(',');
                    //        LiXinModels.Survey.Survey_Exampaper surveyExamPaper = _surveyExampaperBL.GetServeyExamPaper(Convert.ToInt32(itemArray[1]));
                    //        if (surveyExamPaper == null || surveyExamPaper.IsDelete == 1)
                    //        {
                    //            if (itemArray[0] == "0")
                    //            {
                    //                ViewBag.SurveyExampaperName0 = "";
                    //                //   
                    //            }
                    //            if (itemArray[0] == "2")
                    //            {
                    //                ViewBag.SurveyExampaperName2 = "";
                    //            }
                    //        }
                    //        else
                    //        {
                    //            flag = true;
                    //            if (itemArray[0] == "0")
                    //            {
                    //                ViewBag.SurveyExampaperId0 = surveyExamPaper.ExampaperID;
                    //                ViewBag.SurveyExampaperName0 = surveyExamPaper.ExamTitle;
                    //            }
                    //            if (itemArray[0] == "2")
                    //            {
                    //                ViewBag.SurveyExampaperId2 = surveyExamPaper.ExampaperID;
                    //                ViewBag.SurveyExampaperName2 = surveyExamPaper.ExamTitle;
                    //            }
                    //        }

                    //    }
                    //}
                    //if (!flag)
                    //{
                    //    course.IsPing = 0;
                    //}
                }

            }
            return View(course);
        }

        /// <summary>
        /// 视频课程详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CourseVideoDetial(int Id)
        {
            var course = _courseBL.GetCo_Course(Id);
            if (course == null || course.IsDelete == 1)
            {
                course = new Co_Course();
                ViewBag.MsgTips = "该课程不存在或已被删除！";
            }
            else
            {
                List<Co_CourseResource> listResource = _courseResourceBL.GetCourseResourceList(Id);
                ViewBag.SCROMResource = listResource.Where(c => c.ResourceType == 3).FirstOrDefault();//SCROM 课件

                //List<Co_CourseResource> listResource = _courseResourceBL.GetCourseResourceList(Id);
                //ViewBag.CourseAttachList = listResource.Where(c => c.ResourceType == 1).FirstOrDefault();

                ViewBag.CourseAttachList = listResource.Where(c => c.ResourceType == 1).ToList();//附件


                //List<Co_CourseResource> aa = listResource.Where(c => c.ResourceType == 1).ToList();
                ViewBag.CourseVideoList = listResource.Where(c => c.ResourceType == 4);//视频
                if (course.IsTest == 1)
                {
                    ViewBag.CoursePaper = _coursePaperBL.GetCo_CourseMainPaper(Id);
                    if (ViewBag.CoursePaper == null)
                    {
                        ViewBag.CoursePaper = new Co_CoursePaper();
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
                if (course.IsPing == 1)
                {
                    if (!string.IsNullOrWhiteSpace(course.SurveyPaperId))
                    {
                        var arr = course.SurveyPaperId.Split(';');
                        if (!string.IsNullOrWhiteSpace(arr[0]))
                            course.AfterCourseAssess = _surveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[0].Split(',')[1]));
                        if (!string.IsNullOrWhiteSpace(arr[1]))
                            course.AfterCourseTeacherExam =
                                _surveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[1].Split(',')[1]));
                    }
                    //ViewBag.SurveyExampaperId0 = 0;
                    //ViewBag.SurveyExampaperId2 = 0;
                    //string[] surveyArray = course.SurveyPaperId.Split(';');
                    //bool flag = false;
                    //foreach (var item in surveyArray)
                    //{
                    //    if (!string.IsNullOrEmpty(item))
                    //    {//时间问题，判断拙劣。不影响功能。
                    //        string[] itemArray = item.Split(',');
                    //        LiXinModels.Survey.Survey_Exampaper surveyExamPaper = _surveyExampaperBL.GetServeyExamPaper(Convert.ToInt32(itemArray[1]));
                    //        if (surveyExamPaper == null || surveyExamPaper.IsDelete == 1)
                    //        {
                    //            if (itemArray[0] == "0")
                    //            {
                    //                ViewBag.SurveyExampaperName0 = "";
                    //                //   
                    //            }
                    //            if (itemArray[0] == "2")
                    //            {
                    //                ViewBag.SurveyExampaperName2 = "";
                    //            }
                    //        }
                    //        else
                    //        {
                    //            flag = true;
                    //            if (itemArray[0] == "0")
                    //            {
                    //                ViewBag.SurveyExampaperId0 = surveyExamPaper.ExampaperID;
                    //                ViewBag.SurveyExampaperName0 = surveyExamPaper.ExamTitle;
                    //            }
                    //            if (itemArray[0] == "2")
                    //            {
                    //                ViewBag.SurveyExampaperId2 = surveyExamPaper.ExampaperID;
                    //                ViewBag.SurveyExampaperName2 = surveyExamPaper.ExamTitle;
                    //            }
                    //        }

                    //    }
                    //}
                    //if (!flag)
                    //{
                    //    course.IsPing = 0;
                    //}
                }

            }
            return View(course);
        }

        /// <summary>
        /// CPA 课程详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CourseCPADetial(int Id)
        {
            Co_Course course = new Co_Course();
            course = _courseBL.GetCo_Course(Id);
            if (course == null)
            {
                course = new Co_Course();
            }
            else
            {
                List<Co_CourseResource> listResource = _courseResourceBL.GetCourseResourceList(Id);
                ViewBag.CourseAttachList = listResource;//.Where(c => c.ResourceType == 1);//附件

                if (course.OpenFlag == 1)//群组
                {
                    int total = 0;
                    string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                    ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                }
                if (course.OpenFlag == 2)//组织结构
                {
                    string strWhere = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                    ViewBag.Departs = _deptBL.GetAllList(strWhere);
                }
                if (course.OpenFlag == 3)//群组+组织结构
                {
                    int total = 0;
                    string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                    ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                    string strWhereDept = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                    ViewBag.Departs = _deptBL.GetAllList(strWhereDept);
                }
            }
            ViewBag.Flag = 3;
            return View(course);
        }

        /// <summary>
        /// CPA 课程详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CourseCPADetialIndex(int Id)
        {
            var course = new Co_Course();
            course = _courseBL.GetCo_Course(Id);
            if (course == null)
            {
                course = new Co_Course();
            }
            else
            {
                List<Co_CourseResource> listResource = _courseResourceBL.GetCourseResourceList(Id);
                ViewBag.CourseAttachList = listResource;//.Where(c => c.ResourceType == 1);//附件

                if (course.OpenFlag == 1)//群组
                {
                    int total = 0;
                    string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                    ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                }
                if (course.OpenFlag == 2)//组织结构
                {
                    string strWhere = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                    ViewBag.Departs = _deptBL.GetAllList(strWhere);
                }
                if (course.OpenFlag == 3)//群组+组织结构
                {
                    int total = 0;
                    string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                    ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                    string strWhereDept = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                    ViewBag.Departs = _deptBL.GetAllList(strWhereDept);
                }
            }
            //是否CPA
            var isbool = CurrentUser.CPA == "是";
            //课程是否未开始
            if (isbool)
            {
                isbool = course.StartTime > DateTime.Now;
            }

            //是否对本人所在群组开放和是否对本人培训级别进行开发
            if (isbool)
            {
                var list = _sys_GroupBL.GetGroupList(CurrentUser.UserId, 0).Select(p => p.GroupId).ToList();
                var ismygrade = string.IsNullOrEmpty(course.OpenLevel) || ("," + course.OpenLevel + ",").IndexOf("," + CurrentUser.TrainGrade + ",", System.StringComparison.Ordinal) >= 0;//是否对我的级别开放
                var aflag = string.IsNullOrEmpty(course.OpenGroupObject);
                var aflag1 = list.Any(i => ("," + course.OpenGroupObject + ",").IndexOf("," + i + ",", System.StringComparison.Ordinal) >= 0);//我是否在开放的群组

                if (!(ismygrade && (aflag || aflag1)))
                {

                    isbool = false;
                }
            }
            //是否已预订过
            if (isbool)
            {
                var list = _cpaLearnStatusBL.GetCpaCourse(string.Format(" CourseID={0} and UserID={1} ", Id, CurrentUser.UserId));
                isbool = list.Count == 0;
            }


            ViewBag.Predit = isbool;
            ViewBag.Flag = 1;
            return View("CourseCPADetial", course);
        }

        /// <summary>
        /// CPA 课程详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CourseCPADetialForLearn(int Id, int flag = 2)
        {
            var course = new Co_Course();
            course = _courseBL.GetCo_Course(Id);
            if (course == null)
            {
                course = new Co_Course();
            }
            else
            {
                List<Co_CourseResource> listResource = _courseResourceBL.GetCourseResourceList(Id);
                ViewBag.CourseAttachList = listResource;//.Where(c => c.ResourceType == 1);//附件

                if (course.OpenFlag == 1)//群组
                {
                    int total = 0;
                    string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                    ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                }
                if (course.OpenFlag == 2)//组织结构
                {
                    string strWhere = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                    ViewBag.Departs = _deptBL.GetAllList(strWhere);
                }
                if (course.OpenFlag == 3)//群组+组织结构
                {
                    int total = 0;
                    string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                    ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                    string strWhereDept = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                    ViewBag.Departs = _deptBL.GetAllList(strWhereDept);
                }
            }
            //是否CPA
            var isbool = CurrentUser.CPA == "是";
            //课程是否未开始
            if (isbool)
            {
                isbool = course.StartTime > DateTime.Now;
            }

            //是否对本人所在群组开放和是否对本人培训级别进行开发
            if (isbool)
            {
                var list = _sys_GroupBL.GetGroupList(CurrentUser.UserId, 0).Select(p => p.GroupId).ToList();
                var ismygrade = string.IsNullOrEmpty(course.OpenLevel) || ("," + course.OpenLevel + ",").IndexOf("," + CurrentUser.TrainGrade + ",", System.StringComparison.Ordinal) >= 0;//是否对我的级别开放
                var aflag = string.IsNullOrEmpty(course.OpenGroupObject);
                var aflag1 = list.Any(i => ("," + course.OpenGroupObject + ",").IndexOf("," + i + ",", System.StringComparison.Ordinal) >= 0);//我是否在开放的群组

                if (!(ismygrade && (aflag || aflag1)))
                {

                    isbool = false;
                }
            }
            //是否已预订过
            if (isbool)
            {
                var list = _cpaLearnStatusBL.GetCpaCourse(string.Format(" CourseID={0} and UserID={1} ", Id, CurrentUser.UserId));
                isbool = list.Count == 0;
            }


            ViewBag.Predit = isbool;
            ViewBag.Flag = flag;
            return View("CourseCPADetial", course);
        }


        /// <summary>
        /// CPA 课程详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult FromCourseCPAList(int Id, int flag = 4)
        {
            var course = new Co_Course();
            course = _courseBL.GetCo_Course(Id);
            if (course == null)
            {
                course = new Co_Course();
            }
            else
            {
                List<Co_CourseResource> listResource = _courseResourceBL.GetCourseResourceList(Id);
                ViewBag.CourseAttachList = listResource;//.Where(c => c.ResourceType == 1);//附件

                if (course.OpenFlag == 1)//群组
                {
                    int total = 0;
                    string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                    ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                }
                if (course.OpenFlag == 2)//组织结构
                {
                    string strWhere = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                    ViewBag.Departs = _deptBL.GetAllList(strWhere);
                }
                if (course.OpenFlag == 3)//群组+组织结构
                {
                    int total = 0;
                    string strWhere = " GroupId in (select id from dbo.F_SplitIDs('" + course.OpenGroupObject + "')) ";
                    ViewBag.Groups = _sys_GroupBL.GetAllList(out total, 1, int.MaxValue, strWhere);
                    string strWhereDept = " DepartmentId in (select id from dbo.F_SplitIDs('" + course.OpenDepartObject + "')) ";
                    ViewBag.Departs = _deptBL.GetAllList(strWhereDept);
                }
            }
            //是否CPA
            var isbool = CurrentUser.CPA == "是";
            //课程是否未开始
            if (isbool)
            {
                isbool = course.StartTime > DateTime.Now;
            }

            //是否对本人所在群组开放和是否对本人培训级别进行开发
            if (isbool)
            {
                var list = _sys_GroupBL.GetGroupList(CurrentUser.UserId, 0).Select(p => p.GroupId).ToList();
                var ismygrade = string.IsNullOrEmpty(course.OpenLevel) || ("," + course.OpenLevel + ",").IndexOf("," + CurrentUser.TrainGrade + ",", System.StringComparison.Ordinal) >= 0;//是否对我的级别开放
                var aflag = string.IsNullOrEmpty(course.OpenGroupObject);
                var aflag1 = list.Any(i => ("," + course.OpenGroupObject + ",").IndexOf("," + i + ",", System.StringComparison.Ordinal) >= 0);//我是否在开放的群组

                if (!(ismygrade && (aflag || aflag1)))
                {

                    isbool = false;
                }
            }
            //是否已预订过
            if (isbool)
            {
                var list = _cpaLearnStatusBL.GetCpaCourse(string.Format(" CourseID={0} and UserID={1} ", Id, CurrentUser.UserId));
                isbool = list.Count == 0;
            }


            ViewBag.Predit = isbool;
            ViewBag.Flag = flag;
            return View("CourseCPADetial", course);
        }
        #endregion






    }
}
