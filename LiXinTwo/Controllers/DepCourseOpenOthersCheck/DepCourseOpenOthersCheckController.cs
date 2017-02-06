using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LiXinInterface;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinInterface.DepCourseLearn;
using LiXinInterface.DepCourseManage;
using LiXinInterface.DeptPlanManage;
using LiXinInterface.DeptSurvey;
using LiXinInterface.Examination;
using LiXinInterface.Survey;
using LiXinInterface.User;
using LiXinModels.CourseManage;
using System.Text.RegularExpressions;
using LiXinModels.DepCourseLearn;
using LiXinModels.DepCourseManage;
using LiXinModels.DepPlanManage;
using LiXinModels.Examination.DBModel;
using LiXinModels.Survey;
using LiXinModels.User;
using LiXinControllers.Filter;

namespace LiXinControllers
{
    public partial class DepCourseOpenOthersCheckController : BaseController
    {
        #region == 接口实现 ==
        protected ICo_Course CoCourseBl;
        protected ICo_CoursePaper CoCoursePaperBl;

        protected IDep_Course DepCourseBl;
        protected IDep_CourseOrder DepCourseOrderBl;
        protected IDepCourseAdvice DepCourseAdviceBl;
        protected IDep_CourseResource DepCourseResourceBl;
        protected IDep_CoursePaper DepCoursePaperBl;

        protected IExampaper ExampaperBl;
        protected IExampaperSort ExampaperSortBl;
        protected IQuestion QuestionBl;
        protected IQuestionSort QuestionSortBl;
        protected IKnowledgeKey KnowledgeKeyBl;

        protected IDeptSurveyExampaper DepSurveyExampaperBl;
        protected IDepSurveyReplyAnswer DepSurveyReplyAnswerBl;
        protected IUser IUserBL;

        protected ICo_CourseResource CoCourseResource;
        protected ISurveyExampaper CoSurveyExampaperBl;

        protected IDep_YearPlan DepYearPlanBl;
        protected ISys_TrainGrade SysTrainGradeBl;
        protected IDepartment DepartmentBl;
        protected IDep_YearPlan IyearPlan;
        public DepCourseOpenOthersCheckController(
            ICo_Course coCourseBl
            , ICo_CoursePaper coCoursePaperBl

            , IDep_Course depCourseBl
            , IDep_CourseOrder depCourseOrderBl
            , IDepCourseAdvice depCourseAdviceBl
            , IDep_CourseResource depCourseResourceBl
            , IDep_CoursePaper depCoursePaperBl

            , IExampaper exampaperBl
            , IExampaperSort exampaperSortBl
            , IQuestion questionBl
            , IQuestionSort questionSortBl
            , IKnowledgeKey knowledgeKeyBl

            , IDeptSurveyExampaper depSurveyExampaperBl
            , IDepSurveyReplyAnswer depSurveyReplyAnswerBl
            , IUser _IUserBL

            , ICo_CourseResource coCourseResource
            , ISurveyExampaper coSurveyExampaperBl

            , IDep_YearPlan depYearPlanBl
            , ISys_TrainGrade sysTrainGradeBl
            , IDepartment departmentBl
            , IDep_YearPlan _IyearPlan)
        {
            CoCourseBl = coCourseBl;
            CoCoursePaperBl = coCoursePaperBl;

            DepCourseBl = depCourseBl;
            DepCourseOrderBl = depCourseOrderBl;
            DepCourseAdviceBl = depCourseAdviceBl;
            DepCourseResourceBl = depCourseResourceBl;
            DepCoursePaperBl = depCoursePaperBl;
            ExampaperBl = exampaperBl;
            ExampaperSortBl = exampaperSortBl;
            QuestionBl = questionBl;
            QuestionSortBl = questionSortBl;
            KnowledgeKeyBl = knowledgeKeyBl;
            DepSurveyExampaperBl = depSurveyExampaperBl;
            DepSurveyReplyAnswerBl = depSurveyReplyAnswerBl;
            IUserBL = _IUserBL;

            CoCourseResource = coCourseResource;
            CoSurveyExampaperBl = coSurveyExampaperBl;

            DepYearPlanBl = depYearPlanBl;
            SysTrainGradeBl = sysTrainGradeBl;
            DepartmentBl = departmentBl;
            IyearPlan = _IyearPlan;
        }
        #endregion

        #region == 开放至全所审批 ==
        #region 部门开放至全所审批页面呈现

        /// <summary>
        /// 课程列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DepCourseOpenOthersCheck(string p)
        {
            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.Atttea = "请输入搜索内容";
            ViewBag.Attopenusername = "请输入搜索内容";
            ViewBag.Attisopenothers = -1;
            if (!string.IsNullOrWhiteSpace(p) && p == "1")
            {
                if (Session["clpage_DepCourseOpenOthersCheck"] != null)
                {
                    string sess = Session["clpage_DepCourseOpenOthersCheck"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Attname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.Atttea = att[2] == "" ? "请输入搜索内容" : att[2];
                    ViewBag.Attopenusername = att[3] == "" ? "请输入搜索内容" : att[3];
                    ViewBag.Attisopenothers = att[4];
                    ViewBag.AttstartTime = att[5];
                    ViewBag.AttendTime = att[6];
                }
            }
            return View();
        }

        /// <summary>
        /// 审核弹出窗体
        /// </summary>
        /// <returns></returns>
        public ActionResult DoCheck(int depCourseId, int type = 0)
        {
            ViewBag.depCourseId = depCourseId;
            var depCourse = new Dep_Course
                              {
                                  Id = depCourseId,
                                  IsOpenOthers = 2,
                                  IsSendMessage = 0,
                              };
            ViewBag.Type = type;
            return View(depCourse);
        }


        /// <summary>
        /// 集中课程详情页面
        /// </summary>
        /// <param name="depCourseId">课程ID</param>
        /// <param name="backFrom">返回页面0-开放至全所审批页面；1-课程详情（部门/分所开放至全所）</param>
        /// <param name="isDeptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <returns></returns>
        public ActionResult DepCourseTogetherDetial(int depCourseId, int backFrom = 0, int isDeptFlag = 0)
        {
            var course = DepCoDetail(depCourseId, backFrom, isDeptFlag);
            return View(course);
        }

        /// <summary>
        /// 集中课程详情页面
        /// </summary>
        /// <param name="depCourseId">课程ID</param>
        /// <param name="backFrom">返回页面0-开放至全所审批页面；1-课程详情（部门/分所开放至全所）</param>
        /// <param name="isDeptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <returns></returns>
        public ActionResult OpenOthersDepCourseTogetherDetial(int depCourseId, int backFrom = 0, int isDeptFlag = 0)
        {
            var course = DepCoDetail(depCourseId, backFrom, isDeptFlag);
            return View("DepCourseTogetherDetial", course);
        }

        /// <summary>
        /// 公共课程详情方法
        /// </summary>
        private Dep_Course DepCoDetail(int depCourseId, int backFrom = 0, int isDeptFlag = 0)
        {
            ViewBag.backFrom = backFrom;
            var course = DepCourseBl.GetCo_CourseAllList(depCourseId);
            if (course == null || course.IsDelete == 1)
            {
                course = new Dep_Course();
                ViewBag.MsgTips = "该课程不存在或已被删除！";
            }
            else
            {
                var deptList = DepartmentBl.GetTreeDeptParent(course.DeptId.ToString());
                if (deptList.Count == 0)
                {
                    isDeptFlag = 0;
                }
                else
                {
                    var deptList1 = deptList.Where(p => p.DeptName.Contains("上海")).ToList();
                    isDeptFlag = deptList1.Count > 0 ? 0 : 1;
                }

                var listResource = DepCourseResourceBl.GetCourseResourceList(depCourseId);
                ViewBag.CourseResourceList = listResource.Where(c => c.ResourceType == 0);//资源
                ViewBag.CourseAttachList = listResource.Where(c => c.ResourceType == 1);//附件
                if (course.IsTest == 1)
                {
                    ViewBag.CoursePaper = DepCoursePaperBl.GetCo_CourseMainPaper(depCourseId);
                    if (ViewBag.CoursePaper == null)
                    {
                        ViewBag.CoursePaper = new Dep_Coursepaper();
                        course.IsTest = 0;
                    }
                    else
                    {
                        tbExampaper exampaper = ExampaperBl.GetExampaper(ViewBag.CoursePaper.PaperId);
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
                            course.AfterCourseAssess = DepSurveyExampaperBl.GetSurveyExampaper(Convert.ToInt32(arr[0].Split(',')[1]));
                        if (!string.IsNullOrWhiteSpace(arr[1]))
                            course.AfterCourseTeacherExam =
                                DepSurveyExampaperBl.GetSurveyExampaper(Convert.ToInt32(arr[1].Split(',')[1]));
                    }
                }
            }

            ViewBag.isDeptFlag = isDeptFlag;
            return course;
        }
        #endregion

        #region 部门开放至全所审批列表
        /// <summary>
        /// 获得部门开放至全所审批列表
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="teacherName">讲师名称</param>
        /// <param name="openUserName">提交人</param>
        /// <param name="isOpenOthers">审批状态1-待审核，2-审核通过，3-审核拒绝</param>
        /// <param name="startTime">开课时间-开始</param>
        /// <param name="endTime">开课时间-结束</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult GetDepCourseOpenOthersCheckList(string courseName, string teacherName, string openUserName,
                                                         int isOpenOthers, string startTime, string endTime, string jsRenderSortField = "Id desc", int pageSize = int.MaxValue, int pageIndex = 1)
        {
            if (Session["clpage_DepCourseOpenOthersCheck"] != null)
            {
                Session.Remove("clpage_DepCourseOpenOthersCheck");
            }
            Session["clpage_DepCourseOpenOthersCheck"] = pageIndex + "㉿" + courseName + "㉿" + teacherName + "㉿" + openUserName + "㉿" + isOpenOthers + "㉿" + startTime + "㉿" + endTime;
            var totalCount = 0;
            var openCourseList = DepCourseBl.DepCourseOpenOthersCheck(out totalCount, courseName, teacherName, openUserName, isOpenOthers, startTime, endTime, "", pageIndex, pageSize, "order by " + jsRenderSortField);
            return Json(new
            {
                result = 1,
                dataList = openCourseList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 执行审批保存操作
        /// </summary>
        [ValidateInput(false)]
        [Filter.SystemLog("部门课程开放至全所审批", LogLevel.Info)]
        public JsonResult SaveDoCheck(Dep_Course depCourse)
        {
            try
            {
                depCourse.ApprovalUserId = CurrentUser.UserId;
                depCourse.ApprovalTime = System.DateTime.Now;
                depCourse.OpenCourseId = 0;
                #region == 审批通过IsOpenOthers=2 ==
                if (depCourse.IsOpenOthers == 2)//审批通过
                {
                    //复制部门课程到全所课程=>复制Dep_Course到Co_Course
                    var depCourseTemp = DepCourseBl.GetCo_CourseAllList(depCourse.Id);
                    var coCourseTemp = new Co_Course
                                           {
                                               CourseName = depCourseTemp.CourseName + "（部门分享）",
                                               Code = depCourseTemp.Code,
                                               Year = depCourseTemp.Year,
                                               Month = depCourseTemp.Month,
                                               Day = depCourseTemp.Day,
                                               OpenLevel = depCourseTemp.OpenLevel,
                                               IsMust = depCourseTemp.IsMust,
                                               Way = 1,//depCourseTemp.Way,
                                               Teacher = depCourseTemp.Teacher,
                                               StartTime = depCourseTemp.StartTime,
                                               EndTime = depCourseTemp.EndTime,
                                               Sort = "0",
                                               CourseLength = depCourseTemp.CourseLength,
                                               RoomId = 0,// depCourseTemp.RoomId,
                                               NumberLimited = depCourseTemp.NumberLimited,
                                               IsCPA = depCourseTemp.IsCPA,
                                               AdFlag = depCourseTemp.AdFlag,
                                               IsOrder = depCourseTemp.IsOrder,
                                               IsLeave = depCourseTemp.IsLeave,
                                               OpenFlag = depCourseTemp.OpenFlag,
                                               IsRT = depCourseTemp.IsRT,
                                               OpenGroupObject = depCourseTemp.OpenGroupObject,
                                               OpenDepartObject = depCourseTemp.OpenDepartObject,
                                               IsTest = depCourseTemp.IsTest,
                                               IsPing = depCourseTemp.IsPing,
                                               SurveyPaperId = depCourseTemp.SurveyPaperId,
                                               Memo = depCourseTemp.Memo,
                                               CourseFrom = 2,// depCourseTemp.CourseFrom,
                                               StopOrderFlag = depCourseTemp.StopOrderFlag,
                                               StopDucueFlag = depCourseTemp.StopDucueFlag,
                                               ReturnTimes = depCourseTemp.ReturnTimes,
                                               AfterOrderTimes = depCourseTemp.AfterOrderTimes,
                                               PreAdviceConfigTime = depCourseTemp.PreAdviceConfigTime,
                                               AfterEvlutionConfigTime = depCourseTemp.AfterEvlutionConfigTime,
                                               PreCourseTime = depCourseTemp.PreCourseTime,
                                               Publishflag = 0,//未发布 depCourseTemp.Publishflag,
                                               CourseTimes = depCourseTemp.CourseTimes,
                                               LastUpdateTime = depCourseTemp.LastUpdateTime,
                                               IsDelete = depCourseTemp.IsDelete,
                                               TrainDays = depCourseTemp.TrainDays,
                                               CourseAddress = depCourseTemp.CourseAddress,
                                               CourseObjectMemo = depCourseTemp.CourseObjectMemo,
                                               OpenPerson = depCourseTemp.OpenPerson,
                                               CpaTeacher = depCourseTemp.CpaTeacher,
                                               YearPlan = depCourseTemp.YearPlan,
                                               AttFlag = depCourseTemp.AttFlag,
                                               IsSystem = depCourseTemp.IsSystem,
                                               IsYearPlan = depCourseTemp.IsYearPlan,
                                               CourseLengthDistribute = depCourseTemp.CourseLengthDistribute,
                                               IsOpenSub = depCourseTemp.IsOpenSub,
                                               courseProvide = depCourseTemp.courseProvide,
                                               studyRequirement = depCourseTemp.StudentRequirement,
                                               remark = depCourseTemp.remark,
                                               DepCourseId = depCourse.Id //添加部门分所课程ID
                                           };
                    depCourse.OpenCourseId = CoCourseBl.AddOpenCourse(coCourseTemp);
                    if (depCourse.OpenCourseId <= 0)
                    {
                        return Json(new
                        {
                            result = 0
                        }, JsonRequestBehavior.AllowGet);
                    }
                    #region == 复制课程相关资源：1-附件；2-课后评估(课程、讲师)；3-在线考试 ==
                    //1-附件
                    CopyCourseResource(depCourse.Id, depCourse.OpenCourseId);
                    //2-课后评估(课程、讲师)
                    CopySurveyPaper(depCourse.Id, depCourse.OpenCourseId);
                    //3-在线考试
                    CopyOnlineTest(depCourse.Id, depCourse.OpenCourseId);
                    #endregion

                }
                #endregion
                DepCourseBl.UpdateApproval(depCourse);//修改审批状态

                if (depCourse.IsSendMessage != 0)
                {
                    SendMessage(depCourse);//发送消息
                }

                return Json(new
                {
                    result = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #region == 复制课程相关资源：1-附件；2-课后评估(课程、讲师)；3-在线考试 ==
        /// <summary>
        /// 1-复制部门课程附件到全所课程附件=>复制Dep_CourseResource到Co_CourseResource
        /// </summary>
        /// <param name="depCourseId">部门分所课程ID</param>
        /// <param name="coCourseId">一期全所课程ID</param>
        public void CopyCourseResource(int depCourseId, int coCourseId)
        {
            var depCourseResourseList = DepCourseResourceBl.GetCourseResourceList(depCourseId);
            depCourseResourseList.ForEach(p =>
                                              {
                                                  var coCourseResource = new Co_CourseResource
                                                  {
                                                      CourseId = coCourseId,
                                                      ResourceName = p.ResourceName,
                                                      RealName = p.RealName,
                                                      ResourceType = p.ResourceType,
                                                      ResourceSize = p.ResourceSize,
                                                      LastUpdateTime = DateTime.Now,
                                                      PackId = p.PackId,
                                                      Flag = p.Flag,
                                                      IsDelete = 0
                                                  };
                                                  CoCourseResource.AddCourseResource(coCourseResource);
                                              });
        }

        /// <summary>
        /// 2-复制部门课后评估(课程、讲师)到全所课后评估(课程、讲师)
        /// </summary>
        /// <param name="depCourseId">部门分所课程ID</param>
        /// <param name="coCourseId">一期全所课程ID</param>
        public void CopySurveyPaper(int depCourseId, int coCourseId)
        {
            var depCourseModel = DepCourseBl.GetCo_Course(depCourseId);

            if (depCourseModel.IsPing == 1 && !string.IsNullOrWhiteSpace(depCourseModel.SurveyPaperId))
            {
                string newCoSurveyPaperId = "";
                if (!string.IsNullOrWhiteSpace(depCourseModel.SurveyPaperId.Split(';')[0]))//复制课程评估
                {
                    newCoSurveyPaperId = depCourseModel.SurveyPaperId.Split(';')[0].Split(',')[0];
                    int khPingId = CopyToCoSurveyPaper(
                        Convert.ToInt32(depCourseModel.SurveyPaperId.Split(';')[0].Split(',')[1]));
                    newCoSurveyPaperId += "," + khPingId + ";";
                }
                if (!string.IsNullOrWhiteSpace(depCourseModel.SurveyPaperId.Split(';')[1]))//复制讲师评估
                {
                    if (!newCoSurveyPaperId.Contains(';'))
                    {
                        newCoSurveyPaperId += ";";
                    }
                    newCoSurveyPaperId += depCourseModel.SurveyPaperId.Split(';')[1].Split(',')[0];
                    int jsPingId = CopyToCoSurveyPaper(
                        Convert.ToInt32(depCourseModel.SurveyPaperId.Split(';')[1].Split(',')[1]));
                    newCoSurveyPaperId += "," + jsPingId;
                }
                var coCourseModel = CoCourseBl.GetCo_Course(coCourseId);
                coCourseModel.SurveyPaperId = newCoSurveyPaperId;
                CoCourseBl.UpdateCourse(coCourseModel);//修改课后评估问卷
            }
        }

        /// <summary>
        /// 2.1-评估执行插入操作
        /// </summary>
        /// <param name="depPaperId">部门分所问卷ID</param>
        public int CopyToCoSurveyPaper(int depPaperId)
        {
            int coPaperId = 0;
            var depSurveyExampaperModel = DepSurveyExampaperBl.GetServeyExamPaper(depPaperId); //获取部门分所试卷
            if (depSurveyExampaperModel == null) { return coPaperId; }
            var depExampaperCategoryModel = DepSurveyExampaperBl.GetPaperSort(depSurveyExampaperModel.SortID);
            //获取部门分所试卷分类
            if (depExampaperCategoryModel != null && depSurveyExampaperModel.ExampaperID > 0 && depExampaperCategoryModel.CategoryId > 0)
            {
                var coExampaperCategoryTemp = new Survey_ExampaperCategory
                                                  {
                                                      CategoryId = 0,
                                                      CategoryName = depExampaperCategoryModel.CategoryName + "(部门分享)",
                                                      ParentId = 0,
                                                      IsDelete = 0
                                                  };
                if (!CoSurveyExampaperBl.IsExsitPaperSort(coExampaperCategoryTemp.CategoryName, 0, 0))//问卷分类存在
                {
                    coExampaperCategoryTemp.CategoryId = CoSurveyExampaperBl.GetPaperSortByName(coExampaperCategoryTemp.CategoryName, 0);
                }
                else
                {
                    coExampaperCategoryTemp.CategoryId = CoSurveyExampaperBl.AddPaperSortModel(coExampaperCategoryTemp);
                }
                //copy到全所问卷分类
                if (coExampaperCategoryTemp.CategoryId > 0)
                {
                    var coSurveyExampaper = new Survey_Exampaper
                                                {
                                                    ExampaperID = 0,
                                                    SortID = coExampaperCategoryTemp.CategoryId,
                                                    ExamTitle = depSurveyExampaperModel.ExamTitle + "(部门分享)",
                                                    ExamType = depSurveyExampaperModel.ExamType,
                                                    UserID = depSurveyExampaperModel.UserID,
                                                    LastUpdateTime = DateTime.Now,
                                                    ExamDescription = depSurveyExampaperModel.ExamDescription,
                                                    IsDelete = 0,
                                                    ExampaperUsage = depSurveyExampaperModel.ExampaperUsage,
                                                    Questions = new List<Survey_Question>()
                                                }; //全所试卷

                    var depSurveyQuestionList =
                        DepSurveyExampaperBl.GetSurvey_QuestionByExampaperID(depSurveyExampaperModel.ExampaperID);
                    //获取部门分所问题列表
                    foreach (var depSurveyQuestion in depSurveyQuestionList)
                    {
                        var coQuestion = new Survey_Question
                                             {
                                                 QuestionType = depSurveyQuestion.QuestionType,
                                                 QuestionContent = depSurveyQuestion.QuestionContent,
                                                 QuestionOrder = depSurveyQuestion.QuestionOrder,
                                                 UpdateTime = DateTime.Now,
                                                 UserID = depSurveyQuestion.UserID,
                                                 Status = 0,
                                                 LinkSortPayGrade = depSurveyQuestion.LinkSortPayGrade,
                                                 ObjectType = 0,
                                                 Answers = new List<Survey_QuestionAnswer>()
                                             }; //全所试卷问题
                        var depSurveyQuestionAnswerList =
                            DepSurveyExampaperBl.GetSurvey_QuestionAnswerByQuestionID(depSurveyQuestion.QuestionID);
                        //获取部门分所问题选项列表
                        foreach (var depSurveyQuestionAnswer in depSurveyQuestionAnswerList)
                        {
                            var a = new Survey_QuestionAnswer
                                        {
                                            AnswerContent = depSurveyQuestionAnswer.AnswerContent,
                                            Score = depSurveyQuestionAnswer.Score,
                                            Status = 0,
                                            ShowOrder = depSurveyQuestionAnswer.ShowOrder
                                        }; //全所试卷问题选项
                            coQuestion.Answers.Add(a);
                        }
                        coSurveyExampaper.Questions.Add(coQuestion);
                    }
                    coPaperId = CoSurveyExampaperBl.AddCoExampaper(coSurveyExampaper); //copy到全所问卷(包括问题和选项)

                }
            }
            return coPaperId;
        }

        /// <summary>
        /// 3-复制部门在线考试到全所在线考试
        /// </summary>
        /// <param name="depCourseId">部门分所课程ID</param>
        /// <param name="coCourseId">一期全所课程ID</param>
        public void CopyOnlineTest(int depCourseId, int coCourseId)
        {
            var depCourseModel = DepCourseBl.GetCo_Course(depCourseId);
            if (depCourseModel.IsTest == 1)
            {
                var depCoursepaperModel = DepCoursePaperBl.GetCo_CourseMainPaper(depCourseId);//获取部门分所课程关联试卷表
                tbExampaper depExampaper = ExampaperBl.GetExampaper(depCoursepaperModel.PaperId);//获取部门分所试卷
                if (depExampaper._id <= 0)
                {
                    return;
                }
                #region == 复制考试信息 ==
                //1、遍历试卷问题ID,获取题目
                foreach (ReExampaperQuestion depQuestion in depExampaper.QuestionList)//遍历试卷试题
                {
                    tbQuestion depTbQuestion = QuestionBl.GetSingleByID(depQuestion.Qid);//获取试题信息
                    var coTbQuestion = depTbQuestion;//全所问题model
                    #region == 复制知识点 ==
                    tbKnowledgeKey depTbKnowledgeKey = KnowledgeKeyBl.GetSingleByID(depTbQuestion.QuestionKey);//获取试题知识点信息
                    var coTbKnowledgeKey = new tbKnowledgeKey
                                               {
                                                   _id = 0,
                                                   DeptId = 0,
                                                   KeyDescription = depTbKnowledgeKey.KeyDescription,
                                                   KeyName = depTbKnowledgeKey.KeyName + "(部门分享)",
                                                   Number = depTbKnowledgeKey.Number,
                                                   Status = 0
                                               };//全所知识点
                    if (!KnowledgeKeyBl.IsExsitCoSortName(coTbKnowledgeKey.KeyName, 0))//已经存在
                    {
                        coTbQuestion.QuestionKey = KnowledgeKeyBl.GetKnowledgeKey(false)
                                                                  .Find(p => p.KeyName.Equals(coTbKnowledgeKey.KeyName))
                                                                  ._id;
                    }
                    else
                    {
                        coTbQuestion.QuestionKey = KnowledgeKeyBl.InsertKey(coTbKnowledgeKey);//copy到全所知识点
                    }
                    #endregion

                    #region == 复制试题分类 ==
                    tbQuestionSort depTbQuestionSort = QuestionSortBl.GetSingleByID(depTbQuestion.QuestionSortID);//获取试题分类信息
                    var coTbQuestionSort = new tbQuestionSort
                    {
                        _id = 0,
                        DeptId = 0,
                        FatherID = 0,
                        Title = depTbQuestionSort.Title + "(部门分享)",
                        Description = depTbQuestionSort.Description,
                        Status = 0
                    };//全所试题分类
                    if (!QuestionSortBl.IsExsitCoQuestionSortName(coTbQuestionSort.Title, 0, 0))//已经存在
                    {
                        coTbQuestion.QuestionSortID = QuestionSortBl.GetAllQuestionSortList()
                                                                  .Find(p => p.DeptId == 0 && p.FatherID == 0 && p.Title.Equals(coTbQuestionSort.Title))
                                                                  ._id;
                    }
                    else
                    {
                        coTbQuestion.QuestionSortID = QuestionSortBl.Insert(coTbQuestionSort);//copy到全所问题分类
                    }
                    #endregion

                    coTbQuestion.DeptId = 0;
                    coTbQuestion._id = 0;
                    depQuestion.Qid = QuestionBl.Insert(coTbQuestion);//copy到全所问题表
                }
                //2、遍历试卷问题规则
                foreach (ReRuleQuestion depRuleQuestion in depExampaper.QuestionRule)//遍历试卷规则
                {
                    #region == 复制试题分类 ==
                    tbQuestionSort depTbQuestionSort = QuestionSortBl.GetSingleByID(depRuleQuestion.QSort);//获取试题分类信息
                    var coTbQuestionSort = new tbQuestionSort
                    {
                        _id = 0,
                        DeptId = 0,
                        FatherID = 0,
                        Title = depTbQuestionSort.Title + "(部门分享)",
                        Description = depTbQuestionSort.Description,
                        Status = 0
                    };//全所试题分类
                    if (!QuestionSortBl.IsExsitCoQuestionSortName(coTbQuestionSort.Title, 0, 0))//已经存在
                    {
                        depRuleQuestion.QSort = QuestionSortBl.GetAllQuestionSortList()
                                                                  .Find(p => p.DeptId == 0 && p.FatherID == 0 && p.Title.Equals(coTbQuestionSort.Title))
                                                                  ._id;
                    }
                    else
                    {
                        depRuleQuestion.QSort = QuestionSortBl.Insert(coTbQuestionSort);//copy到全所问题分类
                    }
                    #endregion
                }

                //3、复制试卷分类
                #region == 复制试卷分类 ==
                tbExampaperSort depTbExampaperSort = ExampaperSortBl.GetSingleByID(depExampaper.ExamSortID);//获取试卷分类信息
                var coTbExampaperSort = new tbExampaperSort
                {
                    _id = 0,
                    DeptId = 0,
                    FatherID = 0,
                    Title = depTbExampaperSort.Title + "(部门分享)",
                    Description = depTbExampaperSort.Description,
                    Status = 0
                };//全所试卷分类
                if (!ExampaperSortBl.IsExsitCoExampaperSortName(coTbExampaperSort.Title, 0, 0))//已经存在
                {
                    depExampaper.ExamSortID = ExampaperSortBl.GetAllExampaperSortList()
                                                              .Find(p => p.DeptId == 0 && p.FatherID == 0 && p.Title.Equals(coTbExampaperSort.Title))
                                                              ._id;
                }
                else
                {
                    depExampaper.ExamSortID = ExampaperSortBl.Insert(coTbExampaperSort);//copy到全所试卷分类
                }
                #endregion

                //4、复制试卷
                depExampaper._id = 0;
                depExampaper.DeptId = 0;
                depExampaper.ExampaperTitle += "(部门分享)";
                depExampaper.Status = 0;
                int copaperId = ExampaperBl.InsertExampaper(depExampaper);//copy到全所试卷表
                if (copaperId <= 0)
                {
                    return;
                }
                //5、复制课程关联试卷表
                var coCoursepaperModel = new Co_CoursePaper
                                              {
                                                  id = 0,
                                                  PaperId = copaperId,
                                                  CourseId = coCourseId,
                                                  Length = depCoursepaperModel.Length,
                                                  Hour = depCoursepaperModel.Hour,
                                                  TotalScore = depCoursepaperModel.TotalScore,
                                                  LevelScore = depCoursepaperModel.LevelScore,
                                                  TestTimes = depCoursepaperModel.TestTimes,
                                                  IsMain = depCoursepaperModel.IsMain
                                              };
                CoCoursePaperBl.AddCoursePaper(coCoursepaperModel);//copy到全所课程关联试卷表

                #endregion
            }
        }
        #endregion

        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendMessage(Dep_Course depCourse)
        {
            try
            {
                var emailList = new List<KeyValuePair<string, string>>();
                var messlist = new List<KeyValuePair<string, string>>();

                var depCourseTemp = DepCourseBl.GetCo_CourseAllList(depCourse.Id);
                string flagStr = depCourse.IsOpenOthers == 2 ? "审批通过" : "审批拒绝";
                Sys_User usermodel = IUserBL.Get(depCourseTemp.OpenUserId);
                if (usermodel == null)
                {
                    return;
                }
                string Content = usermodel.Realname
                                 + "，您好！ 您于" + depCourseTemp.OpenSubmitTime.ToString("yyyy-MM-dd HH:mm")
                                 + "提交开放至全所的课程《" + depCourseTemp.CourseName + "》，已于"
                                 + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "被" + flagStr + "！【此消息无需回复】";
                if (!string.IsNullOrWhiteSpace(usermodel.Email))
                {
                    emailList.Add(new KeyValuePair<string, string>(usermodel.Email, Content));
                }
                if (!string.IsNullOrWhiteSpace(usermodel.MobileNum))
                {
                    messlist.Add(new KeyValuePair<string, string>(usermodel.MobileNum, Content));
                }


                if (depCourse.IsSendMessage == 1 || depCourse.IsSendMessage == -1)
                {
                    if (emailList.Count > 0)
                    {
                        SendEmail(emailList, "部门课程开放至全所反馈");
                    }
                }
                if (depCourse.IsSendMessage == 2 || depCourse.IsSendMessage == -1)
                {
                    if (messlist.Count > 0)
                    {
                        SendMessage(messlist);
                    }
                }
            }
            catch
            {

            }
        }

        #endregion

        #endregion

        #region == 部门开课跟踪 ==
        /// <summary>
        /// （教育培训部）部门开课跟踪列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DepOpenCourseFollowing(string p)
        {
            ViewBag.page = 1;
            ViewBag.Attdeptname = "请输入搜索内容";
            ViewBag.Attdeptflag = 0;
            if (!string.IsNullOrWhiteSpace(p) && p == "1")
            {
                if (Session["clpage_DepOpenCourseFollowing"] != null)
                {
                    string sess = Session["clpage_DepOpenCourseFollowing"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Attdeptname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.Attdeptflag = att[2];
                }
            }
            return View();
        }
        /// <summary>
        /// （教育培训部）获得部门开课跟踪列表
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <param name="deptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult GetDepOpenCourseFollowingList(string deptName, int deptFlag = 0,
                                                     int pageSize = int.MaxValue, int pageIndex = 1, string jsRenderSortField = " YearPlan asc ")
        {
            if (Session["clpage_DepOpenCourseFollowing"] != null)
            {
                Session.Remove("clpage_DepOpenCourseFollowing");
            }
            Session["clpage_DepOpenCourseFollowing"] = pageIndex + "㉿" + deptName + "㉿" + deptFlag;
            var totalCount = 0;
            var openCourseList = DepCourseBl.DepOpenCourseFollowingList(out totalCount, deptName, deptFlag, "",
                                                                        pageIndex, pageSize,
                                                                        " order by " + jsRenderSortField);
            return Json(new
            {
                result = 1,
                dataList = openCourseList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region == 实际开课详情-部门\分所开放至全所 ==
        /// <summary>
        /// 课程列表
        /// </summary>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门</param>
        /// <param name="isDeptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <returns></returns>
        public ActionResult DepCourseOpenOthersList(string p, int yearPlan = 0, string deptId = "", int isDeptFlag = 0)
        {
            ViewBag.isDeptFlag = isDeptFlag;

            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.Attismust = -1;
            ViewBag.Attisyearplan = -1;
            ViewBag.Attisopenothers = -1;
            ViewBag.Attyearplan = yearPlan;
            ViewBag.Attdeptid = deptId;
            if (!string.IsNullOrWhiteSpace(p) && p == "1")
            {
                if (Session["clpage_DepCourseOpenOthersList"] != null)
                {
                    string sess = Session["clpage_DepCourseOpenOthersList"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Attname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.Attismust = att[2];
                    ViewBag.Attisyearplan = att[3];
                    ViewBag.Attisopenothers = att[4];
                    ViewBag.AttstartTime = att[5];
                    ViewBag.AttendTime = att[6];
                    ViewBag.Attyearplan = att[7];
                    ViewBag.Attdeptid = att[8];
                }
            }
            ViewBag.show = 0;
            return View();
        }

        /// <summary>
        /// 课程列表
        /// </summary>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门</param>
        /// <param name="isDeptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <returns></returns>
        public ActionResult FDepCourseOpenOthersList(string p, int yearPlan = 0, string deptId = "", int isDeptFlag = 0)
        {
            ViewBag.isDeptFlag = isDeptFlag;

            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.Attismust = -1;
            ViewBag.Attisyearplan = -1;
            ViewBag.Attisopenothers = -1;
            ViewBag.Attyearplan = yearPlan;
            ViewBag.Attdeptid = deptId;
            if (!string.IsNullOrWhiteSpace(p) && p == "1")
            {
                if (Session["clpage_DepCourseOpenOthersList"] != null)
                {
                    string sess = Session["clpage_DepCourseOpenOthersList"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Attname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.Attismust = att[2];
                    ViewBag.Attisyearplan = att[3];
                    ViewBag.Attisopenothers = att[4];
                    ViewBag.AttstartTime = att[5];
                    ViewBag.AttendTime = att[6];
                    ViewBag.Attyearplan = att[7];
                    ViewBag.Attdeptid = att[8];
                }
            }
            ViewBag.show = 1;
            return View("DepCourseOpenOthersList");
        }
        /// <summary>
        /// 开课详情-获得部门开放至全所列表
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="isMust">选修\必修 0:必修；1：选修 ；-1：全部(复选框全不选)； -2：全部（复选框全选）</param>
        /// <param name="isYearPlan">计划内\外 0：否；1：是 ； -1：全部(复选框全不选) ；-2：全部（复选框全选）</param>
        /// <param name="isOpenOthers">审批状态1：待审批；2：审批通过；3：审批拒绝 ；-1：待审批+通过+拒绝</param>
        /// <param name="startTime">开课时间-开始</param>
        /// <param name="endTime">开课时间-结束</param>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult GetDepCourseOpenOthersList(string courseName, int isMust, int isYearPlan,
                                                     int isOpenOthers, string startTime, string endTime, int yearPlan, string deptId,
                                                     int pageSize = int.MaxValue, int pageIndex = 1, string jsRenderSortField = " IsOpenOthers asc ")
        {
            if (Session["clpage_DepCourseOpenOthersList"] != null)
            {
                Session.Remove("clpage_DepCourseOpenOthersList");
            }
            Session["clpage_DepCourseOpenOthersList"] = pageIndex + "㉿" + courseName + "㉿" + isMust + "㉿" + isYearPlan + "㉿" + isOpenOthers +
                                "㉿" + startTime + "㉿" + endTime + "㉿" + yearPlan + "㉿" + deptId;
            var totalCount = 0;
            var openCourseList = DepCourseBl.DepCourseOpenOthersList(out totalCount, courseName, isMust, isYearPlan,
                                                                     isOpenOthers, startTime, endTime, yearPlan, deptId, "",
                                                                     pageIndex, pageSize,
                                                                     " order by " + jsRenderSortField);
            return Json(new
            {
                result = 1,
                dataList = openCourseList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region == 实际开课详情-部门\分所自学 ==
        /// <summary>
        /// （实际开课详情）课程列表
        /// </summary>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门</param>
        /// <param name="isDeptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <returns></returns>
        public ActionResult DepCourseLearnSelfList(string p, int yearPlan = 0, string deptId = "", int isDeptFlag = 0, int type = 0, int back = 0, int isYearStatus = 0)
        {
            ViewBag.isDeptFlag = isDeptFlag;

            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.Attisyearplan = -1;
            ViewBag.Attyearplan = yearPlan;
            ViewBag.Attdeptid = deptId;
            ViewBag.type = type;
            ViewBag.isYearStatus = isYearStatus;
            if (!string.IsNullOrWhiteSpace(p) && p == "1")
            {
                if (Session["clpage_DepCourseLearnSelfList"] != null)
                {
                    string sess = Session["clpage_DepCourseLearnSelfList"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Attname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.Attisyearplan = att[2];
                    ViewBag.Attyearplan = att[3];
                    ViewBag.Attdeptid = att[4];
                }
            }
            ViewBag.show = 0;
            ViewBag.back = back;
            return View();
        }

        /// <summary>
        /// （实际开课详情）课程列表
        /// </summary>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门</param>
        /// <param name="isDeptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <returns></returns>
        public ActionResult FDepCourseLearnSelfList(string p, int yearPlan = 0, string deptId = "", int isDeptFlag = 0, int type = 0, int isYearStatus = 0)
        {
            ViewBag.isDeptFlag = isDeptFlag;

            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.Attisyearplan = -1;
            ViewBag.Attyearplan = yearPlan;
            ViewBag.Attdeptid = deptId;
            ViewBag.type = type;
            ViewBag.isYearStatus = isYearStatus;
            if (!string.IsNullOrWhiteSpace(p) && p == "1")
            {
                if (Session["clpage_DepCourseLearnSelfList"] != null)
                {
                    string sess = Session["clpage_DepCourseLearnSelfList"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Attname = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.Attisyearplan = att[2];
                    ViewBag.Attyearplan = att[3];
                    ViewBag.Attdeptid = att[4];
                }
            }
            ViewBag.show = 1;
            return View("DepCourseLearnSelfList");
        }
        /// <summary>
        /// （实际开课详情）-获得部门自学列表
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="isYearPlan">计划内\外 0：否；1：是 ； -1：全部(复选框全不选) ；-2：全部（复选框全选）</param>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="type">0 正常查询 1查询我联合或者联合我的</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult GetDepCourseLearnSelfList(string courseName, int isYearPlan, int yearPlan, string deptId,
                                                     int pageSize = int.MaxValue, int pageIndex = 1, string jsRenderSortField = " Id asc ", int type = 0, int CourseType = -1, int isYearStatus = 0)
        {
            if (Session["clpage_DepCourseLearnSelfList"] != null)
            {
                Session.Remove("clpage_DepCourseLearnSelfList");
            }
            Session["clpage_DepCourseLearnSelfList"] = pageIndex + "㉿" + courseName + "㉿" + isYearPlan + "㉿" + yearPlan + "㉿" + deptId;
            var totalCount = 0;
            string dwhere = " 1=1";
            if (CourseType == 0)
            {
                dwhere += " and cc.DeptId = " + deptId;
            }
            if (CourseType == 1)
            {
                dwhere += " and cc.DeptId != " + deptId;
            }
            var openCourseList = DepCourseBl.DepCourseLearnSelfList(out totalCount, courseName, isYearPlan, yearPlan, deptId, "",
                                                                     pageIndex, pageSize, " order by " + jsRenderSortField, type, dwhere, isYearStatus);
            if (!string.IsNullOrEmpty(deptId))
            {
                openCourseList.ForEach(p =>
                {
                    if (p.LinkDeptId == Convert.ToInt32(deptId))
                    {
                        p.CourseTypeFrom = "本部门的课程";
                    }
                    else
                    {
                        p.CourseTypeFrom = "联合的课程";
                    }
                });
            }
            
            return Json(new
            {
                result = 1,
                dataList = openCourseList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region == 部门分所自学课程详情页面 ==
        #region == 页面呈现 ==
        /// <summary>
        /// 课程详情主页
        /// </summary>
        /// <param name="depCourseId">课程ID</param>
        /// <param name="tp">tab页序号</param>
        /// <param name="isDeptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <returns></returns>
        public ActionResult DepSelfCourse(int depCourseId, int tp = 0, int isDeptFlag = 0)
        {
            ViewBag.isDeptFlag = isDeptFlag;
            var depCourse = DepCourseBl.GetCo_CourseAllList(depCourseId);
            ViewBag.Course = depCourse;
            ViewBag.tp = tp;
            ViewBag.show = 0;
            return View();
        }

        /// <summary>
        /// 课程详情主页
        /// </summary>
        /// <param name="depCourseId">课程ID</param>
        /// <param name="tp">tab页序号</param>
        /// <param name="isDeptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <returns></returns>
        public ActionResult FDepSelfCourse(int depCourseId, int tp = 0, int isDeptFlag = 0)
        {
            ViewBag.isDeptFlag = isDeptFlag;
            var depCourse = DepCourseBl.GetCo_CourseAllList(depCourseId);
            ViewBag.Course = depCourse;
            ViewBag.tp = tp;
            ViewBag.show = 1;
            return View("DepSelfCourse");
        }
        /// <summary>
        /// 课程详情tab0
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <param name="isDeptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <returns></returns>
        public ActionResult CourseMain(int courseid, int isDeptFlag = 0)
        {
            ViewBag.isDeptFlag = isDeptFlag;
            var depCourse = DepCourseBl.GetCo_CourseAllList(courseid);
            ViewBag.Course = depCourse;
            var depCourseResourse = DepCourseResourceBl.GetCourseResourceList(courseid);
            ViewBag.CourseResourse = depCourseResourse;
            return View();
        }
        /// <summary>
        /// 课前建议
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <returns></returns>
        public ActionResult ClassPro(int courseid)
        {
            ViewBag.courseid = courseid;
            return View();
        }
        /// <summary>
        /// 考勤详情
        /// </summary>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public ActionResult Attendance(int courseid)
        {
            ViewBag.courseid = courseid;
            return View();
        }
        /// <summary>
        /// 课后评估
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <returns></returns>
        public ActionResult AfterCourseNew(int courseid)
        {
            var course = DepCourseBl.GetCo_CourseAllList(courseid);
            if (!string.IsNullOrEmpty(course.SurveyPaperId))
            {
                if (course.SurveyPaperId.Split(';')[0] != "")
                {
                    var afterquestion = DepSurveyExampaperBl.GetSurvey_QuestionByExampaperID(Convert.ToInt32(course.SurveyPaperId.Split(';')[0].Split(',')[1]));
                    ViewBag.afterquestion = afterquestion;
                    //加载第一个问题的答案
                    var firsthtml = "";
                    if (afterquestion.Count != 0)
                    {
                        firsthtml = FindAnswerString(courseid, afterquestion[0].ExampaperID, afterquestion[0].QuestionType, afterquestion[0].QuestionID);
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
                    var afterteacherquestion = DepSurveyExampaperBl.GetSurvey_QuestionByExampaperID(Convert.ToInt32(course.SurveyPaperId.Split(';')[1].Split(',')[1]));
                    ViewBag.afterteacherquestion = afterteacherquestion;
                    var firstafterteacherhtml = "";
                    if (afterteacherquestion.Count != 0)
                    {
                        firstafterteacherhtml = FindAnswerString(courseid, afterteacherquestion[0].ExampaperID,
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

            ViewBag.id = courseid;
            ViewBag.SurveyPaperId = course.SurveyPaperId;
            return View();
        }

        /// <summary>
        /// 在线测试
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <returns></returns>
        public ActionResult OnlineTest(int courseid)
        {
            ViewBag.Id = courseid;
            return View();
        }


        #endregion

        #region == 方法 ==
        /// <summary>
        /// 查看课程详情信息下学员列表
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetTeacherCourseUserList(int courseid, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            int Count = 0;
            var list = DepCourseOrderBl.GetTeacherCourseUserList(out Count, courseid, (pageIndex - 1) * pageSize + 1, pageSize);
            return Json(new
            {
                recordCount = Count,
                dataList = list

            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取课前建议列表
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <returns></returns>
        public JsonResult OnLoadDepCourseAdvice(int courseid)
        {
            List<Dep_CourseAdvice> clCourseAdvicesList = DepCourseAdviceBl.GetList(" Dep_CourseAdvice.CourseId=" + courseid + " ");
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

        /// <summary>
        /// 我的课程(讲师) 下考勤学员列表
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult FUserrAttendceList(int courseId, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            int Count = 0;
            var teacherlist = DepCourseOrderBl.GetTeacherCourseAttendceList(out Count, courseId, (pageIndex - 1) * pageSize + 1, pageSize);

            return Json(new
            {
                dataList = teacherlist,
                recordCount = Count

            }, JsonRequestBehavior.AllowGet);
        }
        #region 点击问题把答案绑定出来
        /// <summary>
        /// 点击问题把答案绑定出来
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <param name="ExampaperID">试卷ID</param>
        /// <param name="questiontype">问题类型</param>
        /// <param name="QuestionID">问题ID</param>
        /// <returns></returns>
        public JsonResult FindAnswer(int courseid, int ExampaperID, int questiontype, int QuestionID)
        {
            string html = "";
            html = FindAnswerString(courseid, ExampaperID, questiontype, QuestionID);
            return Json(new
            {
                List = html
            }, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 显示课后评估回答信息
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <param name="ExampaperID">试卷ID</param>
        /// <param name="questiontype">问题类型</param>
        /// <param name="QuestionID">问题ID</param>
        /// <returns></returns>
        public string FindAnswerString(int courseid, int ExampaperID, int questiontype, int QuestionID)
        {
            var list = DepSurveyReplyAnswerBl.GetSurvey_ReplyAnswerListAndUser(" ObjectID=" + courseid + " and ExampaperID=" + ExampaperID + " and QuestionID=" + QuestionID + " and Dep_Survey_ReplyAnswer.Status=1 ");
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
        #endregion

        /// <summary>
        /// 获取学员考试信息
        /// </summary>
        /// <param name="courseid">课程ID</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetTeacherOnLineTest(int courseid, int pageSize = 10, int pageIndex = 1)
        {

            var litme = 0;
            var courseorder = DepCourseOrderBl.GetTeacherOnLineTest(out litme, courseid,
                                                                  (pageIndex - 1) * pageSize + 1, pageSize);
            return Json(new
            {
                result = 1,
                dataList = courseorder,
                recordCount = litme
            }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #endregion

        #region == 计划开课详情（单体/联合上报） ==
        /// <summary>
        /// 课程列表
        /// </summary>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门</param>
        /// <param name="isOpen">上报方式：0-单体 1-联合</param>
        /// <param name="isDeptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <returns></returns>
        public ActionResult DepPlanOpenYearCourse(string p, int yearPlan = 0, string deptId = "", int isOpen = 0, int isDeptFlag = 0)
        {
            ViewBag.isDeptFlag = isDeptFlag;
            var depYearPlanList =
                DepYearPlanBl.GetDepYearListByWhere(
                    string.Format(@"  PublishFlag=1 and Year={0} and DeptId in ({1}) and IsOpen={2} ", yearPlan, deptId,
                                  isOpen));
            int id = depYearPlanList.Count > 0 ? depYearPlanList[0].Id : 0;
            Dep_YearPlan itemArray = DepYearPlanBl.GetYearModel(id);
            ViewBag.trainGrade = SysTrainGradeBl.GetAllTrainGrade();//获取培训级别
            if (itemArray != null && isOpen == 1)//组织结构
            {
                string strWhere = " DepartmentId in (SELECT DeptId FROM Dep_LinkDepart WHERE ApprovalFlag=1 and YearId=" + id + ") ";
                ViewBag.OKDeparts = DepartmentBl.GetAllList(strWhere);
                string strWhere1 = " DepartmentId in (SELECT DeptId FROM Dep_LinkDepart WHERE ApprovalFlag=2 and YearId=" + id + ") ";
                ViewBag.NODeparts = DepartmentBl.GetAllList(strWhere1);
                string strWhere2 = " DepartmentId in (SELECT DeptId FROM Dep_LinkDepart WHERE ApprovalFlag=0 and YearId=" + id + ") ";
                ViewBag.UnDeparts = DepartmentBl.GetAllList(strWhere2);
            }

            ViewBag.Yearmodel = itemArray;
            ViewBag.yearPlan = yearPlan;
            ViewBag.deptId = deptId;
            ViewBag.isOpen = isOpen;
            ViewBag.show = 0;
            return View();
        }
        /// <summary>
        /// 课程列表
        /// </summary>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门</param>
        /// <param name="isOpen">上报方式：0-单体 1-联合</param>
        /// <param name="isDeptFlag">是否部门 0:部门；1：分所 默认为0</param>
        /// <returns></returns>
        public ActionResult FDepPlanOpenYearCourse(string p, int yearPlan = 0, string deptId = "", int isOpen = 0, int isDeptFlag = 0)
        {
            ViewBag.isDeptFlag = isDeptFlag;
            var depYearPlanList =
                DepYearPlanBl.GetDepYearListByWhere(
                    string.Format(@"  PublishFlag=1 and Year={0} and DeptId in ({1}) and IsOpen={2} ", yearPlan, deptId,
                                  isOpen));
            int id = depYearPlanList.Count > 0 ? depYearPlanList[0].Id : 0;
            Dep_YearPlan itemArray = DepYearPlanBl.GetYearModel(id);
            ViewBag.trainGrade = SysTrainGradeBl.GetAllTrainGrade();//获取培训级别
            if (itemArray != null && isOpen == 1)//组织结构
            {
                string strWhere = " DepartmentId in (SELECT DeptId FROM Dep_LinkDepart WHERE ApprovalFlag=1 and YearId=" + id + ") ";
                ViewBag.OKDeparts = DepartmentBl.GetAllList(strWhere);
                string strWhere1 = " DepartmentId in (SELECT DeptId FROM Dep_LinkDepart WHERE ApprovalFlag=2 and YearId=" + id + ") ";
                ViewBag.NODeparts = DepartmentBl.GetAllList(strWhere1);
                string strWhere2 = " DepartmentId in (SELECT DeptId FROM Dep_LinkDepart WHERE ApprovalFlag=0 and YearId=" + id + ") ";
                ViewBag.UnDeparts = DepartmentBl.GetAllList(strWhere2);
            }

            ViewBag.Yearmodel = itemArray;
            ViewBag.yearPlan = yearPlan;
            ViewBag.deptId = deptId;
            ViewBag.isOpen = isOpen;
            ViewBag.show = 1;
            return View("DepPlanOpenYearCourse");
        }
        /// <summary>
        /// 计划开课详情-获得部门上报列表
        /// </summary>
        /// <param name="courseName">课程名称</param>
        /// <param name="teacherName">讲师名称</param>
        /// <param name="openLevel">开放级别</param>
        /// <param name="isMust">选修\必修 0:必修；1：选修 ；-1：全部(复选框全不选)； -2：全部（复选框全选）</param>
        /// <param name="isSystem">框架内/外 0:外；1：内 ；-1：全部(复选框全不选)； -2：全部（复选框全选）</param>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门ID</param>
        /// <param name="isOpen">单体/联合上报 0：单体 1：联合 </param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="jsRenderSortField">jsrender排序</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult GetDepPlanOpenYearCourseList(string courseName, string teacherName, string openLevel,
                                                   int isMust, int isSystem, int yearPlan = 0, string deptId = "", int isOpen = 0,
                                                     int pageSize = int.MaxValue, int pageIndex = 1, string jsRenderSortField = " Month_Day desc ")
        {
            var totalCount = 0;

            jsRenderSortField = (jsRenderSortField.ToLower().Contains("desc")
                                    ? "  ORDER BY Month desc,Day desc  "
                                    : "  ORDER BY Month asc,Day asc  ");

            var openCourseList = DepYearPlanBl.GetDepPlanOpenYearCourseList(out totalCount, courseName, teacherName,
                                                                            openLevel, isMust, isSystem, yearPlan,
                                                                            deptId, isOpen, "", pageIndex, pageSize,
                                                                            jsRenderSortField);
            return Json(new
            {
                result = 1,
                dataList = openCourseList,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出年度计划课程信息
        /// </summary>
        /// <param name="yearPlanId">年度计划ID</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="teacherName">讲师名称</param>
        /// <param name="openLevel">开放级别</param>
        /// <param name="isMust">选修\必修 0:必修；1：选修 ；-1：全部(复选框全不选)； -2：全部（复选框全选）</param>
        /// <param name="isSystem">框架内/外 0:外；1：内 ；-1：全部(复选框全不选)； -2：全部（复选框全选）</param>
        /// <param name="yearPlan">计划年度</param>
        /// <param name="deptId">创建部门ID</param>
        /// <param name="isOpen">单体/联合上报 0：单体 1：联合 </param>
        public void ExportDepPlanOpenYearCourseList(int yearPlanId, string courseName, string teacherName, string openLevel,
                                                   int isMust, int isSystem, int yearPlan = 0, string deptId = "", int isOpen = 0)
        {
            Dep_YearPlan year = DepYearPlanBl.GetYearModel(yearPlanId) ?? new Dep_YearPlan();
            var totalCount = 0;
            List<Dep_YearPlanCourse> yearList = DepYearPlanBl.GetDepPlanOpenYearCourseList(out totalCount, courseName, teacherName,
                                                                            openLevel, isMust, isSystem, yearPlan,
                                                                            deptId, isOpen);
            DataTable outTable = new DataTable();
            outTable.Columns.Add("课程名称", typeof(string));
            outTable.Columns.Add("学时", typeof(string));
            outTable.Columns.Add("预计开课时间", typeof(string));
            outTable.Columns.Add("开放级别", typeof(string));
            outTable.Columns.Add("授课讲师", typeof(string));
            //outTable.Columns.Add("讲师薪酬", typeof(string));
            outTable.Columns.Add("必修/选修", typeof(string));
            //outTable.Columns.Add("是否折算CPA学时", typeof(string));
            outTable.Columns.Add("框架内/外", typeof(string));
            foreach (Dep_YearPlanCourse v in yearList)
            {
                outTable.Rows.Add(v.CourseName, v.CourseLength, v.OpenTime, v.OpenLevel, v.Realname, v.IsMustStr, v.IsSystemStr);
                //outTable.Rows.Add(v.CourseName, v.CourseLength, v.OpenTime, v.OpenLevel, v.Realname, v.IsMustStr, v.IsCPAStr, v.IsSystemStr);
            }
            new Spreadsheet().Template(year.Year + "年度计划课程安排【" + year.DeptName + "】", null, outTable, HttpContext, "年度计划课程", "年度计划");
        }
        #endregion

        /// <summary>
        ///  年度计划跟踪呈现
        /// </summary>
        public ViewResult DepYearPlanTrack(string tp, int backtype = 0)
        {
            List<int> itemArray = IyearPlan.GetYear();
            ViewData["StrYear"] = itemArray;
            ViewBag.backtype = backtype;
            ViewBag.page = 1;
            if (tp != null && tp != "" && tp == "1")
            {
                string sess = Session["Tracktpage"].ToString();
                string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                ViewBag.startTime = att[0];
                ViewBag.endTime = att[1];
                ViewBag.deptName = att[2];
                ViewBag.year = att[3];
                ViewBag.isopen = att[4];
                ViewBag.page = att[5];
            }
            return View();
        }
    }

}
