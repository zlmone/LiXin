using LiXinControllers.Filter;
using LiXinInterface.ClassRoom;
using LiXinInterface.DepCourseManage;
using LiXinInterface.DeptSurvey;
using LiXinInterface.Examination;
using LiXinInterface.SystemManage;
using LiXinInterface.User;
using LiXinModels.DepCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using System.Text.RegularExpressions;
using LiXinInterface;
using LiXinInterface.DeptPlanManage;
using LiXinModels.User;

namespace LiXinControllers
{
    public class DepCourseManageController :BaseController
    {
        protected IDep_Course Idep_course;
        protected IDep_CourseResource Idep_courseresource;
        protected IDep_CoursePaper Idep_coursepaper;
        protected IDeptSurveyExampaper IdepsurveyExampaperBL;
        protected ISys_Group _sys_GroupBL;
        protected IDepartment _deptBL;
        protected IUser _userBL;
        protected IDepClassRoom _classRoomBL;
        protected IDep_CourseOrder _det_courseorder;

        protected ISys_TrainGrade _sys_TrainBL;

        protected IDep_YearPlan Idep_YearPlan;

        protected IDep_MonthPlan Imonth;

        protected IExampaper exampaperBL;


        protected IDepartment IDepartmentBL;
        
        public DepCourseManageController(IDep_Course _Idep_course, IExampaper _exampaperBL,
            IDep_CourseResource _Idep_courseresource, IDep_CoursePaper _Idep_coursepaper,
            IDeptSurveyExampaper _IdepsurveyExampaperBL, ISys_Group sys_GroupBL, IDepartment deptBL, IUser userBL, IDepClassRoom classRoomBL,
            ISys_TrainGrade sys_TrainBL, IDep_YearPlan _Idep_YearPlan, IDepartment _IDepartmentBL, IDep_CourseOrder det_courseorder)
        {
            Idep_course = _Idep_course;
            exampaperBL = _exampaperBL;
            Idep_courseresource = _Idep_courseresource;
            Idep_coursepaper = _Idep_coursepaper;
            IdepsurveyExampaperBL = _IdepsurveyExampaperBL;
            _sys_GroupBL = sys_GroupBL;
            _deptBL = deptBL;
            _userBL = userBL;
            exampaperBL = _exampaperBL;
            _sys_TrainBL = sys_TrainBL;
            _classRoomBL = classRoomBL;
            Idep_YearPlan = _Idep_YearPlan;
            IDepartmentBL = _IDepartmentBL;
            _det_courseorder = det_courseorder;
            
        }


        #region 页面呈现
        /// <summary>
        /// 标签列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DepManageList(int tp=0)
        {
            //根据人员SubordinateSubstation字段区分是 开设人员是分所还是部门 true为总所 false是分所
            ViewBag.DepOrBranch = CurrentUser.SubordinateSubstation.Contains("上海");

            ViewBag.tp = tp;
            
            return View();
        }


        /// <summary>
        /// 开放至全所列表
        /// </summary>
        /// <returns></returns>
        public ActionResult IncurredCourseManageList()
        {
            ViewBag.DepOrBranch = CurrentUser.SubordinateSubstation.Contains("上海");
            //ViewBag.DeptList = IDepartmentBL.GetTreeDeptDown(CurrentUser.DeptId.ToString());
            return View();
        }

        /// <summary>
        /// 部门自学
        /// </summary>
        /// <returns></returns>
        public ActionResult DepCourseManageList()
        {
            ViewBag.DepOrBranch = CurrentUser.SubordinateSubstation.Contains("上海");

            ViewBag.DeptidList = GetAllSubDepartments();
            //ViewBag.DeptList = IDepartmentBL.GetTreeDeptDown(CurrentUser.DeptId.ToString());
            return View();
        }

        public ActionResult DeptPersn()
        {
           // ViewBag.deptId = deptId;
            ViewBag.TrainGrade = AllTrainGrade;
            ////ViewBag.DeptList = IDepartmentBL.GetTreeDeptDown(CurrentUser.DeptId.ToString());
            //ViewBag.DeptList = IDepartmentBL.GetAllList(" DepartmentId in("+deptId+")");
            return View();
        }
        

        public ActionResult CourseTogetherDetial(int Id,string tp)
        {
            ViewBag.tp = tp;
            var course = Idep_course.GetCo_CourseAllList(Id);
            if (course == null || course.IsDelete == 1)
            {
                course = new Dep_Course();
                ViewBag.MsgTips = "该课程不存在或已被删除！";
            }
            else
            {
                List<Dep_CourseResource> listResource = Idep_courseresource.GetCourseResourceList(Id);
                ViewBag.CourseResourceList = listResource.Where(c => c.ResourceType == 0);//资源
                ViewBag.CourseAttachList = listResource.Where(c => c.ResourceType == 1);//附件
                if (course.IsTest == 1)
                {
                    ViewBag.CoursePaper = Idep_coursepaper.GetCo_CourseMainPaper(Id);
                    if (ViewBag.CoursePaper == null)
                    {
                        ViewBag.CoursePaper = new Dep_Coursepaper();
                        course.IsTest = 0;
                    }
                    else
                    {
                        LiXinModels.Examination.DBModel.tbExampaper exampaper = exampaperBL.GetExampaper(ViewBag.CoursePaper.PaperId);
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
                            course.AfterCourseAssess = IdepsurveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[0].Split(',')[1]));
                        if (!string.IsNullOrWhiteSpace(arr[1]))
                            course.AfterCourseTeacherExam =
                                IdepsurveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[1].Split(',')[1]));
                    }                    
                }

            }
            return View(course);
        }
        #endregion

        //dep_course Way:1自学 2：开放至全所
        #region 修改集中课程
        /// <summary>
        /// 修改集中课程
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="TotalCourseTimes"></param>
        /// <param name="Publishflag"></param>
        /// <param name="IsCopy"></param>
        /// <returns></returns>
         [Filter.SystemLog("修改集中课程", LogLevel.Info)]
        public ActionResult EditDepCourseTogether(int? Id, int TotalCourseTimes = 0, int Publishflag = 0, int IsCopy = 0,int way=0)
        {
            //根据人员SubordinateSubstation字段区分是 开设人员是分所还是部门  有上海是属于部门  没有上海则是分所
            ViewBag.DepOrBranch = CurrentUser.SubordinateSubstation.Contains("上海");
            //ViewBag.deptID = CurrentUser.DeptId;
            if (way != 0)
            {
                ViewBag.Way = way;//用于区分返回值
            }
            ///默认加载第一个部门ID
            int deptid = GetAllSubDepartments()[0].DepartmentId;

            ViewBag.DeptidList = GetAllSubDepartments();

            ViewBag.Deptid = deptid;

            //ViewBag.DeptId = CurrentUser.DeptId;

            //必修学时
            ViewBag.MustCourseLength = AllSystemConfigs.Find(s => s.ConfigType == 18).ConfigValue.Split(';')[0];
            //选修学时
            ViewBag.ChoosableCourseLength = AllSystemConfigs.Find(s => s.ConfigType == 18).ConfigValue.Split(';')[1];

            //课程结束后 多久时间内 允许考试
            //ViewBag.RefHour = AllSystemConfigs.Find(s => s.ConfigType == 26).ConfigValue.Split(';')[0];
            //ViewBag.RefHour = DepConfig(CurrentUser.DeptId, 3)==""?"999":DepConfig(CurrentUser.DeptId, 3).Split(';')[0];
            ViewBag.RefHour = DepConfig(deptid, 3) == "" ? "999" : DepConfig(deptid, 3).Split(';')[0];

            
            //考试时长
            //ViewBag.RefLength = AllSystemConfigs.Find(s => s.ConfigType == 26).ConfigValue.Split(';')[1];
            //ViewBag.RefLength = DepConfig(CurrentUser.DeptId, 3)==""?"999":DepConfig(CurrentUser.DeptId, 3).Split(';')[1];
            ViewBag.RefLength = DepConfig(deptid, 3) == "" ? "999" : DepConfig(deptid, 3).Split(';')[1];

            //测试次数
            //ViewBag.MaxTestTimes = AllSystemConfigs.Find(s => s.ConfigType == 27).ConfigValue;
            //ViewBag.MaxTestTimes = DepConfig(CurrentUser.DeptId, 4);
            ViewBag.MaxTestTimes = DepConfig(deptid, 4);

            //课前建议时间外
            //ViewBag.RefPreAdviceConfigTime = AllSystemConfigs.Find(s => s.ConfigType == 29).ConfigValue.Split(';')[0];
            //ViewBag.RefPreAdviceConfigTime =  DepConfig(CurrentUser.DeptId, 2)==""?"0":DepConfig(CurrentUser.DeptId, 2).Split(';')[0];
            ViewBag.RefPreAdviceConfigTime = DepConfig(deptid, 2) == "" ? "0" : DepConfig(CurrentUser.DeptId, 2).Split(';')[0];

            //课后评估时间内
            //ViewBag.RefAfterEvlutionConfigTime = AllSystemConfigs.Find(s => s.ConfigType == 29).ConfigValue.Split(';')[1];
            //ViewBag.RefAfterEvlutionConfigTime = DepConfig(CurrentUser.DeptId, 2)==""?"0":DepConfig(CurrentUser.DeptId, 2).Split(';')[1];
            ViewBag.RefAfterEvlutionConfigTime = DepConfig(deptid, 2) == "" ? "0" : (DepConfig(CurrentUser.DeptId, 2) == "" ? "0" : DepConfig(CurrentUser.DeptId, 2).Split(';')[1]);
            //ViewBag.xueshi = AllSystemConfigs.Find(p => p.ConfigType == 24);

            ViewBag.TotalCourseTimes = TotalCourseTimes;

            ViewBag.TokenPublishflag = Publishflag;//课程是否发布

            ViewBag.IsCopy = IsCopy;

            //教室
            ViewBag.ClassRoomList = _classRoomBL.GetRoomList(" DeptId=" + deptid + " and IsDelete=0 ");

            //培训级别
            ViewBag.trainGrade = _sys_TrainBL.GetAllTrainGrade();


            Dep_Course course = new Dep_Course();

         

            if (string.IsNullOrEmpty(course.CourseLengthDistribute))
            {
                course.CourseLengthDistribute = AllSystemConfigs.Find(p => p.ConfigType == 42).ConfigValue;
            }

            if (Id.HasValue)
            {
                course = Idep_course.GetCo_CourseAllList(Id.Value);

                ViewBag.Deptid = course.DeptId;
                ViewBag.ClassRoomList = _classRoomBL.GetRoomList(" DeptId=" + course.DeptId + " and IsDelete=0 ");

                if (course == null)
                {
                    course = new Dep_Course();
                }
                else
                {
                    //2013-9-22 改为只要是本部门的人都能选择为讲师
                    //if (course.TeacherIsTeacher == 0 || course.TeacherIsDelete == 1)
                    //{
                    //    course.Teacher = "";
                    //    course.TeacherName = "";
                    //}
                    if (course.TeacherIsDelete == 1)
                    {
                        course.Teacher = "";
                        course.TeacherName = "";
                    }
                    List<Dep_CourseResource> listResource = Idep_courseresource.GetCourseResourceList(Id.Value);
                    ViewBag.CourseResourceList = listResource.Where(c => c.ResourceType == 0);//资源
                    ViewBag.CourseAttachList = listResource.Where(c => c.ResourceType == 1);//附件
                    if (course.IsTest == 1)
                    {
                        ViewBag.CoursePaper = Idep_coursepaper.GetCo_CourseMainPaper(Id.Value);
                        if (ViewBag.CoursePaper == null)
                        {
                            ViewBag.CoursePaper = new Dep_Coursepaper();
                            course.IsTest = 0;
                        }
                        else
                        {
                            LiXinModels.Examination.DBModel.tbExampaper exampaper = exampaperBL.GetExampaper(ViewBag.CoursePaper.PaperId);
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
                                LiXinModels.Survey.Survey_Exampaper surveyExamPaper = IdepsurveyExampaperBL.GetServeyExamPaper(Convert.ToInt32(itemArray[1]));
                                if (surveyExamPaper == null || surveyExamPaper.IsDelete == 1)
                                {
                                    if (itemArray[0] == "0")
                                    {
                                        ViewBag.SurveyExampaperName0 = "";
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
            //ViewBag.ClassRoomList = _classRoomBL.GetRoomList(" DeptId=" + CurrentUser.DeptId + " and IsDelete=0 ");
            
            //ViewBag.YearPlanList = Idep_YearPlan.GetAllYear(CurrentUser.DeptId," PublishFlag=1 AND IsDelete=0 ").Select(t => t.Year);
            return View(course);
            //return View();
        }
        #endregion

        #region 提交 编辑 集中授课课
        [Filter.SystemLog("提交 编辑 集中授课课程", LogLevel.Info)]
        public JsonResult SubmitEditCourseTogether(Dep_Course course, Dep_Coursepaper coursePaper, string hidResourceIds, int IsCopy = 0)
        {
            if (course.Id == 0 || IsCopy == 1)
            {
                int zhi =Convert.ToInt32(AllSystemConfigs.Find(s => s.ConfigType == 43).ConfigValue);

                if (zhi == 1)
                {
                    //var yearplan = Idep_YearPlan.GetAllYearID(CurrentUser.DeptId);
                    var yearplan = Idep_YearPlan.GetAllYearID(course.DeptId);
                    if (yearplan.Count == 0)
                    {
                        return Json(new
                        {
                            result = 0,
                            content = "本部门下没有年度计划，无法开课！"
                        }, JsonRequestBehavior.AllowGet);
                    }
                     
                }

                if (course.StartTime < DateTime.Now && (course.Way==1||course.Way==2 ))
                {
                    return Json(new
                    {
                        result = 0,
                        content = "课程开始时间不能小于系统当前时间！"
                    }, JsonRequestBehavior.AllowGet);
                }

                if (course.EndTime > DateTime.Now && (course.Way ==3))
                {
                    return Json(new
                    {
                        result = 0,
                        content = "课程结束时间不能大于系统当前时间！"
                    }, JsonRequestBehavior.AllowGet);
                }

                course.Year = course.PreCourseTime.Year;
                course.Month = course.PreCourseTime.Year +"-" + (course.PreCourseTime.Month.ToString().Length > 1 ? course.PreCourseTime.Month.ToString() : "0" + course.PreCourseTime.Month.ToString());
                course.CourseFrom = 2;
                course.AfterOrderTimes = 0;
                course.IsDelete = 0;
                course.LastUpdateTime = DateTime.Now;
                course.Way = course.Way;
                course.CourseLengthDistribute = course.courseattend + ";" + course.courseonlinetest + ";" + course.courseafter;
                course.OpenFlag = course.OpenFlag;

                if (!string.IsNullOrEmpty(course.OpenDepartObject))
                {
                    course.OpenDepartObject = course.OpenDepartObject;
                }
                if (!string.IsNullOrEmpty(course.txt_year_ids))
                {
                    if (!string.IsNullOrEmpty(course.OpenDepartObject))
                    {
                        course.OpenDepartObject += "," + course.txt_year_ids;
                    }
                    else
                    {
                        course.OpenDepartObject =  course.txt_year_ids;
                    }
                    
                }

                //course.DeptId = CurrentUser.DeptId;//添加部门ID
                course.DeptId = course.DeptId;//添加部门ID
                if (course.Way == 1 || course.Way == 3)//1：分所 3：过时
                {
                    course.IsOpenOthers = 0; //课程发布后 审批状态在开课后就不作修改了
                }
                if (course.Way == 2)//2：全所
                {
                    course.IsOpenOthers = 1;
                }

                course.OpenUserId = CurrentUser.UserId;
                course.Memo = course.Memo;
                course.AttUserId = CurrentUser.UserId.ToString();
                course.AttFlag = course.AttFlag;
                //添加
                Idep_course.AddCourse(course);
                if (IsCopy == 1)
                {
                    Idep_course.UpdateCourseTimesByCode(course.Code, course.CourseTimes);
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
                        Idep_course.InsertOrUpdateCourseMainPaper(coursePaper);
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
                Dep_Course tempCourse = Idep_course.GetCo_CourseAllList(course.Id);
                if (tempCourse.Publishflag != 1 && course.StartTime < DateTime.Now && (course.Way == 1 || course.Way == 2))
                {
                    return Json(new
                    {
                        result = 0,
                        content = "课程开始时间不能小于系统当前时间！"
                    }, JsonRequestBehavior.AllowGet);
                }


                if (course.EndTime > DateTime.Now && (course.Way == 3))
                {
                    return Json(new
                    {
                        result = 0,
                        content = "课程结束时间不能大于系统当前时间！"
                    }, JsonRequestBehavior.AllowGet);
                }


                //tempCourse.IsOpenOthers = course.IsOpenOthers;
                //tempCourse.DeptId = CurrentUser.DeptId; //课程部门ID根据人员部门ID 来区分
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
                tempCourse.AdFlag = course.AdFlag;
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

                //string[] coursearr = course.OpenPerson.Split(',');
                //string[] tempCoursearr = tempCourse.OpenPerson.Split(',');

                //var mao = course.OpenPerson.Where(t => !tempCourse.OpenPerson.Contains(t)).ToString();

                tempCourse.PreCourseTime = course.PreCourseTime;
                tempCourse.PreAdviceConfigTime = course.PreAdviceConfigTime;
                tempCourse.AfterEvlutionConfigTime = course.AfterEvlutionConfigTime;
                tempCourse.OpenPerson = course.OpenPerson;
                tempCourse.CourseTimes = course.CourseTimes;
                tempCourse.YearPlan = course.YearPlan;
                tempCourse.IsYearPlan = course.IsYearPlan;
                tempCourse.IsSystem = course.IsSystem;
                tempCourse.LastUpdateTime = DateTime.Now;

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
                bool newid = Idep_course.UpdateCourse(tempCourse);
                if (course.CourseTimes > 1)//课次大于1 那么存在已经 拷贝的课程
                {
                    Idep_course.UpdateCourseTimesByCode(course.Code, course.CourseTimes);

                }
                ///关联考试
                if (course.IsTest == 1)
                {
                    coursePaper.CourseId = course.Id;
                    coursePaper.IsMain = 0;
                    Idep_course.InsertOrUpdateCourseMainPaper(coursePaper);
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
        #endregion

        #region 删除课程关联资源
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
                    Idep_courseresource.DeleteCourseResource(int.Parse(item));
                }
            }
        }
        #endregion

        #region 集中课程列表
        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="IsMust"></param>
        /// <param name="IsCPA"></param>
        /// <param name="openLevel"></param>
        /// <param name="way">授课方式 way 1:自学 2：全所</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
       // [Filter.SystemLog("获取列表  集中授课", LogLevel.Info)]
        public JsonResult GetCourseTogetherList(int DeptId,int way,string courseName, string StartTime, string EndTime, string IsMust, string IsCPA,int IsOpenOthers, int Sort = -1, string isSys = "", string isYear = "", int pageSize = 20, int pageIndex = 1, int systemId = 0, int PublishFlag = -1, string jsRenderSortField = "Code desc")
        {
            //if (Session["ctpage"] != null)
            //{
            //    Session.Remove("ctpage");
            //}
            //Session["ctpage"] = pageIndex + "㉿" + courseName + "㉿" + StartTime + "㉿" + EndTime + "㉿" + IsMust + "㉿" + IsCPA + "㉿" + Sort + "㉿" + isSys + "㉿" + isYear;

            StringBuilder strWhere = new StringBuilder();
            //strWhere.Append(" CourseFrom=2 AND way=1 ");
            //int intDeptId = 0;

            //if (DeptId == 0)
            //{
            //    intDeptId = CurrentUser.DeptId;
            //}
            //else
            //{
            //    intDeptId = DeptId;
            //}

            


            if (DeptId == 0)
            {
                var deptlist = GetAllSubDepartments();
                string ids = "";
                foreach (var item in deptlist)
                {
                    ids += item.DepartmentId + ",";
                }
                strWhere.Append(" CourseFrom=2 and Dep_Course.DeptId in(" + ids.Substring(0,ids.LastIndexOf(','))+")");
            }
            else
            {
                strWhere.Append(" CourseFrom=2 and Dep_Course.DeptId=" + DeptId);
            }

            if (way == 1)
            {
                strWhere.Append(" and way in(1,3)");
            }
            else
            {
                strWhere.Append(" and way=2");
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
            if (IsOpenOthers > 0)
            {
                strWhere.AppendFormat(" AND IsOpenOthers= '{0}'", IsOpenOthers);
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
                //string strCourseIds = string.Join(",", _SysLinkCourseBL.GetCourseIdListBySystemId(systemId));
                //if (!string.IsNullOrEmpty(strCourseIds))
                //{
                //    strWhere.AppendFormat(" AND Co_Course.Id in({0}) ", strCourseIds);
                //}
                //else
                //{
                //    strWhere.AppendFormat(" AND 1=2 ");//该体系下木有课程
                //}
            }
            if (PublishFlag != -1)
            {
                strWhere.AppendFormat(" AND PublishFlag={0} ", PublishFlag);
            }
            List<Dep_Course> listCourse = new List<Dep_Course>();

            int totalCount = 0;
            listCourse = Idep_course.GetCourseTogetherList(out totalCount,way, strWhere.ToString(), (pageIndex - 1) * pageSize, pageSize, "ORDER BY Dep_Course." + jsRenderSortField);
            int n = 0;
            var itemArray = new object[listCourse.Count()];
            foreach (var item in listCourse)
            {

                string tempTeacher = "";
                tempTeacher = item.TeacherName;
                //if (item.TeacherIsDelete == 1 || item.TeacherIsTeacher == 0)
                //{
                //    tempTeacher = "--";
                //}
                //else
                //{
                //    tempTeacher = item.TeacherName;
                //}
                var temp = new
                {
                    item.Id,
                    CourseName = item.CourseName.HtmlXssEncode(), 
                    item.CourseLength,
                    IsMust = typeof(LiXinModels.Enums.IsMust).GetEnumName(item.IsMust),
                    StartTime = item.StartTime.ToString("yyyy-MM-dd HH:mm"),
                    EndTime = item.EndTime.ToString("yyyy-MM-dd HH:mm"),
                    item.SortStr,
                    TeacherName = tempTeacher.HtmlXssEncode(),
                    RoomName = item.RoomName.HtmlXssEncode(),
                    item.Way,
                    IsCPA = typeof(LiXinModels.Enums.IsCPA).GetEnumName(item.IsCPA),
                    item.Publishflag, 
                    item.CourseTimes, 
                    item.CourseTimesOrder, 
                    item.TotalCourseTimes,
                    IsSystem = item.IsSystemStr,
                    IsYearPlan = item.IsPlanStr, 
                    item.CourseState, 
                    item.IsOpenOthersStr, 
                    item.IsOpenOthers,
                    item.AttUserId,
                    item.AttFlag
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
        #endregion


        #region 上传资源
        /// <summary>
        /// 关联资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Filter.SystemLog("提交 新增课程资源", LogLevel.Info)]
        public ContentResult SubmitAddCourseResource(Dep_CourseResource model)
        {
            var result = Idep_courseresource.AddCourseResource(model) ? "1" : "0";
            return Content(result);
        }
        #endregion

        #region 修改课程单一状态
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
            //if (cl == "t")
            //{
            //    if (Session["ctpage"] != null)
            //    {
            //        sess = Session["ctpage"].ToString();
            //    }
            //}
            //else if (cl == "t")
            //{
            //    if (Session["cvpage"] != null)
            //    {
            //        sess = Session["cvpage"].ToString();
            //    }
            //}
            //else if (cl == "c")
            //{
            //    if (Session["ccpage"] != null)
            //    {
            //        sess = Session["ccpage"].ToString();
            //    }
            //}
            if (sess != "")
            {
                string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                page = Convert.ToInt32(att[0]);
            }
            if (flag == 1)
            {
                Dep_Course course = Idep_course.GetCo_CourseAllList(courseId);
                Dictionary<int, string> dic = PublishValidate(course);
                if (dic.Count > 0)
                {
                    var dic_content = "课程信息有误,详细如下:<br/>";
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
            if (Idep_course.ModifySingleCourse(flag, courseId))
            {
                if (way > 0)
                {
                    Dep_Course course = Idep_course.GetCo_CourseAllList(courseId);
                    if (!string.IsNullOrEmpty(course.OpenPerson))
                    {
                        //课程特殊人群在课程发布时报名
                        //_courseOrderBL.AddSpecialCrowdUserToCourse(courseId, course.CourseName, course.StartTime, course.EndTime, 2, course.OpenPerson, AllSystemConfigs.Find(p => p.ConfigType == 21).ConfigValue.GetDouble());

                        _det_courseorder.AddSpecialCrowdUserToCourse(courseId, 1, course.OpenPerson);

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
                                        messList.Add(new KeyValuePair<string, string>(userList[i].MobileNum, c.Replace("教育培训部",CurrentUser.DeptName)));
                                    if (!string.IsNullOrWhiteSpace(userList[i].Email))
                                        emailList.Add(new KeyValuePair<string, string>(userList[i].Email, c.Replace("教育培训部", CurrentUser.DeptName)));
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


        public JsonResult FCancelCourse(int courseid)
        {
            
            //_det_courseorder
            if (_det_courseorder.CancelCoursePub(courseid))
            {
                _det_courseorder.DeleteZhiDingUser(courseid);
                return Json(new
                {
                    result = 0,
                    content = "取消发布成功"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    result = 1,
                    content = "取消发布失败"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 验证课程编号重名
        /// <summary>
        /// 验证课程编号重名
        /// </summary>
        [Filter.SystemLog("验证课程编号重名", LogLevel.Info)]
        public JsonResult CheckCourseCode(string CourseCode, int Id, int way = 1)
        {
            return Json(!Idep_course.IsExistCourseCode
(CourseCode.ReplaceSingleSql(), 2, Id, way), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  发布课程验证
        /// <summary>
        /// 发布课程验证
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public Dictionary<int, string> PublishValidate(Dep_Course course)
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
                    //var teacher = _userBL.Get(int.Parse(course.Teacher));
                    //if (teacher == null || teacher.IsDelete == 1 || teacher.IsTeacher == 0)
                    //    dic.Add(2, "授课讲师无效");// //授课讲师 无效
                    var teacher = _userBL.Get(int.Parse(course.Teacher));
                    if (teacher == null || teacher.IsDelete == 1 )
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
                    var coursePaper = Idep_coursepaper.GetCo_CourseMainPaper(course.Id);
                    var paper = exampaperBL.GetExampaper(coursePaper.PaperId);
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
                            LiXinModels.Survey.Survey_Exampaper surveyExamPaper = IdepsurveyExampaperBL.GetServeyExamPaper(Convert.ToInt32(itemArray[1]));
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

        #region 发送消息
        public JsonResult FunCourseCpaSendMessage(int courseId, string userIds, int cpaflag = 3)
        {
            Dep_Course course = Idep_course.GetCo_CourseAllList(courseId);

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
                                messList.Add(new KeyValuePair<string, string>(userList[i].MobileNum, c.Replace("教育培训部", CurrentUser.DeptName)));
                            if (!string.IsNullOrWhiteSpace(userList[i].Email))
                                emailList.Add(new KeyValuePair<string, string>(userList[i].Email, c.Replace("教育培训部", CurrentUser.DeptName)));
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
                    //_cpaLearnStatusBL.DeleteByCourseIdAndCpaFlag(courseId, 1);
                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region 集中授课开设

        /// <summary>
        ///  集中授课开设 专属 ,内有自定义事件~~ (*^__^*) ……    调用 年月 部分视图页面  拷贝 YearMonthSelect 
        /// </summary>
        /// <param name="viewType"></param>
        /// <param name="dataType"></param>
        /// <param name="onclick"></param>
        /// <returns></returns>
        public ActionResult YearMonthSelectForCourse(int viewType = 1, int dataType = 0, string onclick = "")
        {
            if (dataType == 0)
            {
                ViewBag.year = DateTime.Now.Year;
            }
            else
            {
                ViewBag.year = DateTime.Now.Year;
                //ViewBag.year = Imonth.GetYearOfPlan();
            }
            ViewBag.viewType = viewType;
            ViewBag.dataType = dataType;
            ViewBag.onclick = onclick;
            return View();
        }
        #endregion

        #region 根据月份加载课程
        /// <summary>
        /// 根据月份加载课程
        /// </summary>
        /// <param name="monthstr"></param>
        /// <param name="way">1:集中；2视频；3：CPA</param>
        /// <returns></returns>
        [Filter.SystemLog("根据月份加载课程", LogLevel.Info)]
        public JsonResult GetMonthPlanCourse(string monthstr, int way = 1,int deptid=0)
        {

            string query = string.Format(@"
or
			Dep_Course.Id in( select CourseId from [Dep_MonthPlanCourse] where Monthid in (
				select Id from dbo.Dep_MonthPlan where id in(
					select id from dbo.Dep_MonthPlan tm where 
						tm.[year]=(
					select [year] from Dep_YearPlan where id=
					(
					 select yearid from Dep_LinkDepart where DeptId={0} and ApprovalFlag=1
					)
				 ) and tm.deptid=
					(
						 select deptid from Dep_YearPlan where id =
						 (
						 select yearid from Dep_LinkDepart where DeptId={0} and ApprovalFlag=1
						 )
					) and  [Month]='{1}'
				)

			
			)
			)", deptid, monthstr);
            string strwhere = string.Format("  CourseFrom=1 and Dep_Course.Id in( select CourseId from [Dep_MonthPlanCourse] where Monthid in (select Id from dbo.Dep_MonthPlan where DeptId=" + deptid + " and PublishFlag=1 and [Month]='{0}')) {2} ", monthstr, way, query);
            int total = 0;
            List<Dep_Course> listCourse = Idep_course.GetCourseCommonList(out total, strwhere, 0, int.MaxValue);
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

        /// <summary>
        /// 指定课程负责人
        /// </summary>
        /// <param name="id">课程ID </param>
        /// <param name="uid">负责人ID</param>
        /// <returns></returns>
        public JsonResult AssignMaster(int id,string uid)
        {
            //修改课程负责人
            return Idep_course.ModifyCourseMaster(id, uid)
                       ? Json(new {result = 1, message = "操作成功"}, JsonRequestBehavior.AllowGet)
                       : Json(new {result = 0, message = "操作失败"}, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// 获取集中授课中报名的人员
        /// </summary>
        public JsonResult GetDepCourseOrderName(int courseID, string realName = "", string deptName = "", int pageSize = 10, int pageIndex = 1, string jsRenderSortField = " su.UserId asc")
        {
            try
            {
                int totalCount = 0;
                string where = " 1=1";
                if (!string.IsNullOrWhiteSpace(deptName))
                    where += string.Format(" and su.DeptPath like '%{0}%'", deptName.ReplaceSql());
                if (!string.IsNullOrWhiteSpace(realName))
                    where += string.Format(" and su.RealName like '%{0}%'", realName.ReplaceSql());
                var list = _det_courseorder.GetDepCourseOrderName(out totalCount, courseID, where, pageSize, pageIndex, jsRenderSortField);
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
                    dataList = new List<Sys_User>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// 修改默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult GetParamConfig(int type)
        {
            string guoqifenbu = "";
            string ping = "";
            string test = "";

            if (type == 3)
            {
                guoqifenbu = AllSystemConfigs.Find(s => s.ConfigType == 50).ConfigValue; //30;20;50
                ping += DepConfig(CurrentUser.DeptId, 8);//4
                test += DepConfig(CurrentUser.DeptId, 9);//5;6

            }
            if (type == 1 || type == 2)
            {
                guoqifenbu = AllSystemConfigs.Find(s => s.ConfigType == 42).ConfigValue; //30;20;50
                //ping += AllSystemConfigs.Find(s => s.ConfigType == 29).ConfigValue.Split(';')[1];//4
                //test += AllSystemConfigs.Find(s => s.ConfigType == 26).ConfigValue;//5;6
                ping += DepConfig(CurrentUser.DeptId, 2);//1;2
                test += DepConfig(CurrentUser.DeptId, 3);//1;2

                
            }

            return Json(new
            {
                guoqifenbu = guoqifenbu,
                ping = ping,
                test = test,
                type = type
            }, JsonRequestBehavior.AllowGet);
 
        }


        /// <summary>
        /// 获取用户列表（立信新的）
        /// </summary>
        /// <param name="jobNum">工号</param>
        /// <param name="realName">姓名</param>
        /// <param name="dept">部门</param>
        /// <param name="sex">性别  【99：全部；0：男；1：女】</param>
        /// <param name="status">状态  【99：全部；0：正常；1：冻结】</param>
        /// <param name="trainGrade">培训级别</param>
        /// <param name="deptId">部门ID</param>
        /// <param name="flag">
        /// 0：本部门
        /// 1：包含下级部门
        /// </param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="IsNew">0所有人  1不是新进员工 2新进员工</param>
        /// <returns></returns>
        public JsonResult GetUserInfoList(string jobNum, string realName, string email, string deptName, int sex, int status, string trainGrade, int cpa = 99, string usertype = "99", int roleId = 0, string deptId="", int flag = 0, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "Sys_User.UserId desc", int IsNew = 0)
        {
            int totalCount = 0;
            var deptIds = deptId.ToString();
            if (flag == 1)//根据部门Id获取该部门及其子部门的ID的集合
            {
                //if (deptId != 0)
                //{
                //    var deptList = new List<int>();
                //    GetChildDeptIds(deptId, deptList);
                //    deptIds = deptList.Aggregate("", (current, item) => current + (item + ",")) + deptId;
                //}
            }
            string where = " Sys_User.IsDelete = 0 and Sys_User.IsTeacher < 2";
            if (deptIds != "0")
                where += string.Format(" and Sys_User.DeptId in ({0})", deptIds);
            if (!string.IsNullOrWhiteSpace(deptName))
                where += string.Format(" and Sys_User.DeptPath like '%{0}%' ", deptName.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(email))
                where += string.Format(" and Sys_User.Email like '%{0}%' ", email.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(jobNum))
                where += string.Format(" and Sys_User.JobNum like '%{0}%' ", jobNum.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(realName))
                where += string.Format(" and Sys_User.Realname like '%{0}%' ", realName.ReplaceSql());
            if (!string.IsNullOrWhiteSpace(trainGrade) && trainGrade != "99")
                where += string.Format(" and ('{0}' like '%'+ Sys_User.TrainGrade +'%' and (Sys_User.TrainGrade is not null and Sys_User.TrainGrade <> '')) ", trainGrade);
            if (cpa != 99)
                where += string.Format(" and Sys_User.CPA = '{0}' ", cpa == 1 ? "是" : "否");
            if (usertype != "99")
            {
                where += string.Format(" and Sys_User.UserType = '{0}' ", usertype.Trim());
            }
            if (sex != 99)
                where += " and Sys_User.Sex = " + sex;
            if (roleId != 0)
                where += string.Format(" and Sys_User.UserId in (select distinct userid from Sys_UserRole where roleid = {0}) ", roleId);
            if (status != 99)
            {
                if (status == 1)
                    where += string.Format(" and ((Sys_User.status = 1 and Sys_User.FreezeTime is  null) or (Sys_User.status = 1 and Sys_User.FreezeTime is not null and Sys_User.FreezeTime > '{0}') or Sys_User.status = 2) ", DateTime.Now);
                else if (status == 0)
                    where += string.Format(" and ((Sys_User.Status = 0) or (Sys_User.Status = 1 and Sys_User.FreezeTime is not null and Sys_User.FreezeTime < '{0}')) ", DateTime.Now);
            }
            if (IsNew != 0)
            {
                if (IsNew == 1)
                {
                    where += " and (Sys_User.IsNew=0 or Sys_User.IsNew is null)";
                }
                else
                {
                    where += " and Sys_User.IsNew=1";
                }
            }

            where += " and Sys_User.UserType in ('在职','聘用','特批','见习')";

            //var list = _userBL.GetList(out totalCount, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
            var list = _userBL.GetList(out totalCount, where, (pageIndex - 1) * pageSize, pageSize, "order by " + jsRenderSortField);
            

            int n = 0;
            var itemArray = new object[list.Count()];
            foreach (var item in list)
            {
                var temp = new
                {
                    JobNum = item.JobNum,
                    Realname = item.Realname,
                    SexStr = item.SexStr,
                    DeptName = item.DeptPath,
                    TrainGrade = item.TrainGrade,
                    Telephone = item.Telephone,
                    Email = item.Email,
                    RoleName = item.RoleName.HtmlXssEncode(),
                    StatusStr = item.StatusStr,
                    UserId = item.UserId,
                    DeptId = item.DeptId,
                    PayGrade = string.IsNullOrWhiteSpace(item.PayGrade) ? "--" : item.PayGrade,
                    CPA = item.CPA,
                    Status = item.Status
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
    }
}