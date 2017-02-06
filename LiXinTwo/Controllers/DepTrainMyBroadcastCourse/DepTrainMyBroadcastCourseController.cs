using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.AttendceManage;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinInterface.Examination;
using LiXinInterface.Survey;
using LiXinInterface.User;
using LiXinModels.CourseLearn;
using LiXinModels.Examination.DBModel;
using LiXinModels.Survey;
using LiXinModels.CourseManage;
using LiXinModels.User;
using Retech.LearningAPI.Core;
using System.Text.RegularExpressions;

namespace LiXinControllers
{
    public partial class DepTrainMyBroadcastCourseController : BaseController
    {
        #region == 接口实现 ==
        protected ICo_Course CoCourseBL;
        protected ICourseOrder CourseOrderBL;
        protected ICl_CourseAdvice ClCourseAdviceBL;
        protected ICl_Attendce ClAttendceBL;
        protected ISurveyExampaper ISurveyExampaperBL;
        protected ISurveyQuestion ISurveyQuestionBL;
        protected ISurveyQuestionAnswer ISurveyQuestionAnswerBL;
        protected ISurveyReplyAnswer ISurveyReplyAnswerBL;
        protected ICo_CourseResource ICoCourseResourceBL;
        protected ICo_CoursePaper ICo_CoursePaperBL;

        protected ICl_LearnVideoInfor IClLearnVideoInforBL;
        protected ICl_CpaLearnStatus ICpaLearnStatusBL;
        protected IExamination IExaminationrBL;
        protected IExampaper IExampaperBL;
        protected IUser IUserBL;
        protected IAttendce IAttendceBL;

        public DepTrainMyBroadcastCourseController(ICo_Course _CoCourseBL, ICourseOrder _CourseOrderBL,
                                  ICl_CourseAdvice _ClCourseAdviceBL, ICl_Attendce _ClAttendceBL,
                                  ISurveyExampaper _ISurveyExampaperBL, ISurveyQuestion _ISurveyQuestionBL,
                                  ISurveyQuestionAnswer _ISurveyQuestionAnswerBL,
                                  ISurveyReplyAnswer _ISurveyReplyAnswerBL, ICo_CourseResource _ICoCourseResourceBL,
                                  ICo_CoursePaper _ICo_CoursePaperBL, IExamination _IExaminationrBL,
                                  IExampaper _IExampaperBL, ICl_CpaLearnStatus _ICpaLearnStatusBL,
                                  ICl_LearnVideoInfor _IClLearnVideoInforBL, IUser _IUserBL,
            IAttendce _IAttendceBL)
        {
            CoCourseBL = _CoCourseBL;
            CourseOrderBL = _CourseOrderBL;
            ClCourseAdviceBL = _ClCourseAdviceBL;
            ClAttendceBL = _ClAttendceBL;
            ISurveyExampaperBL = _ISurveyExampaperBL;
            ISurveyQuestionBL = _ISurveyQuestionBL;
            ISurveyQuestionAnswerBL = _ISurveyQuestionAnswerBL;
            ISurveyReplyAnswerBL = _ISurveyReplyAnswerBL;
            ICoCourseResourceBL = _ICoCourseResourceBL;
            ICo_CoursePaperBL = _ICo_CoursePaperBL;
            IExaminationrBL = _IExaminationrBL;
            IExampaperBL = _IExampaperBL;
            ICpaLearnStatusBL = _ICpaLearnStatusBL;
            IClLearnVideoInforBL = _IClLearnVideoInforBL;
            IUserBL = _IUserBL;
            IAttendceBL = _IAttendceBL;
        }
        #endregion

        
        #region 集中课程视频转播呈现

        /// <summary>
        /// 我的视频转播课程
        /// </summary>
        /// <returns></returns>
        public ActionResult MyEverythingCourse(string tp="0")
        {
            ViewBag.tp = tp;
            return View();
        }

        /// <summary>
        /// 课程列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MyBroadcastList(string p)
        {
            ViewBag.page = 1;
            ViewBag.Attname = "请输入搜索内容";
            ViewBag.Atttea = "请输入搜索内容";
            ViewBag.Attmust = -1;
            ViewBag.Attstate = -1;
            ViewBag.Attsort = -1;
            ViewBag.AttType = -1;
            if (!string.IsNullOrWhiteSpace(p) && p == "1")
            {
                if (Session["clpage_MyBroadcastList"] != null)
                {
                    string sess = Session["clpage_MyBroadcastList"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.page = att[0];
                    ViewBag.Atttea = att[1] == "" ? "请输入搜索内容" : att[1];
                    ViewBag.Attname = att[2] == "" ? "请输入搜索内容" : att[2];
                    ViewBag.Attmust = att[3];
                    ViewBag.Attstate = att[4];
                    ViewBag.Attsort = att[5];
                    ViewBag.AttType = att[6];
                    ViewBag.AttstartTime = att[7];
                    ViewBag.AttendTime = att[8];
                }
            }
            return View();
        }

        /// <summary>
        /// 详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MyBroadcastCourse(int id)
        {
            var courseModel = CourseOrderBL.GetCourseById(id, CurrentUser.UserId);
            ViewBag.Course = courseModel;
            return View();
        }

        /// <summary>
        /// 课程详情
        /// </summary>
        /// <returns></returns>
        public ActionResult BroadcastCourseMain(int id)
        {
            var courseModel = CoCourseBL.GetCo_Course(id);
            ViewBag.Course = courseModel;
            var courseResourse = ICoCourseResourceBL.GetList(id);
            ViewBag.CourseResourse = courseResourse;
            return View();
        }

        /// <summary>
        /// 课后评估
        /// </summary>
        /// <returns></returns>
        /// 
        /// 
        public ActionResult BroadcastAfterCourse(int id, string backurl)
        {
            ViewBag.backurl = backurl;
            ViewBag.id = id;
            return View();
        }

        /// <summary>
        /// 新的课后页面方法 只有星级题目 原AfterCourseList暂时不用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BroadcastAfterCourseNewList(int id, string backurl)
        {
            ViewBag.backurl = backurl;
            var courselist = CoCourseBL.GetVideoCo_CourseById(id);
            if (!string.IsNullOrEmpty(courselist.SurveyPaperId) && courselist.IsPing == 1)
            {
                var arr = courselist.SurveyPaperId.Split(';');
                if (arr[0] != "")
                {
                    var examPaper = ISurveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[0].Split(',')[1]));
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
                    var TeacherexamPaper = ISurveyExampaperBL.GetSurveyExampaper(Convert.ToInt32(arr[1].Split(',')[1]));
                    if (TeacherexamPaper != null)
                    {
                        ViewBag.TeacherexamPaper = TeacherexamPaper;
                    }
                    else
                    {
                        ViewBag.TeacherexamPaper = null;
                    }
                }
                ViewBag.id = id;
                ViewBag.SurveyPaperId = courselist.SurveyPaperId;
            }
            return View();
        }

        /// <summary>
        /// 在线测试
        /// </summary>
        /// <returns></returns>
        public ActionResult BroadcastOnlineTest(int id)
        {
            Co_CoursePaper CoCoursePaper = ICo_CoursePaperBL.GetCo_CourseMainPaper(id);
            var course = CoCourseBL.GetCo_Course(id);

            if (CoCoursePaper != null && course.IsTest == 1)
            {
                var exampapger = IExampaperBL.GetExampaper(CoCoursePaper.PaperId);
                ViewBag.exampapger = exampapger;
            }
            ViewBag.Co_CoursePaper = CoCoursePaper;
            ViewBag.courseid = id;
            ViewBag.course = course;
            return View();
        }
        #endregion

        #region 集中课程视频转播列表
        /// <summary>
        /// 获得集中课程视频转播列表
        /// </summary>
        /// <param name="teacherName">讲师</param>
        /// <param name="courseName">课程名称</param>
        /// <param name="isMust">是否必修:0-必修 1-选修</param>
        /// <param name="learnStatus">课程状态0-未开始，1-进行中,2-已结束</param>
        /// <param name="sort">所有课程类型1-内部培训 2-社会招聘 3-新进员工 4-实习生</param>
        /// <param name="subscribeType">预订状态 0-不可预订 1-未预订</param>
        /// <param name="startTime">开课开始时间-begin</param>
        /// <param name="endTime">开课开始时间-end</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult GetMyBroadcastList(string teacherName, string courseName, int isMust, int learnStatus, int sort,
                                int subscribeType, string startTime, string endTime, int pageSize = int.MaxValue, int pageIndex = 1)
        {
            if (Session["clpage_MyBroadcastList"] != null)
            {
                Session.Remove("clpage_MyBroadcastList");
            }
            Session["clpage_MyBroadcastList"] = pageIndex + "㉿" + teacherName + "㉿" + courseName + "㉿" + isMust + "㉿" + learnStatus + "㉿" + sort + "㉿" + subscribeType + "㉿" + startTime + "㉿" + endTime;
            var totalCount = 0;
            var courseorder = CourseOrderBL.GetMyBroadcastList(out totalCount, CurrentUser.UserId,teacherName,courseName,isMust,learnStatus,
                sort,subscribeType,startTime,endTime,"",pageIndex, pageSize);

            return Json(new
            {
                result = 1,
                dataList = courseorder,
                recordCount = totalCount
            }, JsonRequestBehavior.AllowGet);
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
        /// <param name="extendname">扩展名</param>
        public void LoadPrincipleFile(string path, string name, string extendname)
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
                int indexOfType = name.LastIndexOf('.');
                if(indexOfType>=0)
                {
                    if(!extendname.Equals(name.Substring(indexOfType + 1)))
                    {
                        name = name + "." + extendname;
                    }
                }
                else
                {
                    name = name + "." + extendname;
                }
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
    }

}
