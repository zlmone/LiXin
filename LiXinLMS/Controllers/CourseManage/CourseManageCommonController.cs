using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;
using LiXinModels;
using System.Web.Mvc;
using LiXinInterface.CourseManage;
using LiXinInterface.User;
using LiXinInterface.ClassRoom;
using LiXinInterface.CourseLearn;
using LiXinInterface.Examination;
using LiXinInterface.Survey;
using LiXinInterface.SystemManage;
using LiXinInterface.AttendceManage;
using LiXinInterface.PlanManage;
namespace LiXinControllers
{
    public partial class CourseManageController : BaseController
    {

        /// <summary>
        /// 胜任力课程体系顶部 常量
        /// </summary>
        protected  const string TopCourseSystemName="胜任力课程体系";

        protected ICo_CourseSystem _courseSysBL;
        protected ICo_SystemLinkCourse _SysLinkCourseBL;
        protected ICo_Course _courseBL;
        protected IClassRoom _classRoomBL;
        protected ICo_CourseResource _courseResourceBL;
        protected ICo_CoursePaper _coursePaperBL;
        protected IExampaper _exampaperBL;
        protected ISurveyExampaper _surveyExampaperBL;
        protected ISys_Group _sys_GroupBL;
        protected IDepartment _deptBL;
        protected IUser _userBL;
        protected ICourseOrder _courseOrderBL;
        protected ITr_YearPlan _trYearPlanBL;
        protected ISys_TrainGrade _sys_TrainBL;
        protected ICl_CpaLearnStatus _cpaLearnStatusBL;
        protected ISys_PayGrade _sysPayGradeBL;
        protected ISurveyQuestion _SurveyQuestionBL;
        protected ISurveyReplyAnswer _ISurveyReplyAnswerBL;
        protected ICl_VideoManage _videoManageBL;
        protected ICo_VideoResource _CoVideoResourceBL;


        public CourseManageController(ICo_CourseSystem courseSysBL, ICo_SystemLinkCourse sysLinkCourseBL, ICo_Course courseBL,
            IClassRoom classRoomBL, ICo_CourseResource courseResourceBL,ICo_CoursePaper coursePaperBL,IExampaper exampaperBL,
            ISurveyExampaper surveyExampaperBL, ISys_Group sys_GroupBL, IDepartment deptBL, IUser userBL, ICourseOrder courseOrderBL,
            ITr_YearPlan trYearPlanBL,ISys_TrainGrade sys_TrainBL,ISurveyReplyAnswer ISurveyReplyAnswerBL,ICo_VideoResource CoVideoResourceBL,
            ICl_CpaLearnStatus cpaLearnStatusBL, ISys_PayGrade sysPayGradeBL, ISurveyQuestion SurveyQuestionBL,ICl_VideoManage videoManageBL)
        {
            _courseSysBL = courseSysBL;
            _SysLinkCourseBL = sysLinkCourseBL;
            _courseBL = courseBL;
            _classRoomBL = classRoomBL;
            _courseResourceBL = courseResourceBL;
            _coursePaperBL = coursePaperBL;
            _exampaperBL = exampaperBL;
            _surveyExampaperBL = surveyExampaperBL;
            _sys_GroupBL = sys_GroupBL;
            _deptBL = deptBL;
            _userBL = userBL;
            _courseOrderBL = courseOrderBL;
            _trYearPlanBL = trYearPlanBL;
            _sys_TrainBL = sys_TrainBL;
            _cpaLearnStatusBL = cpaLearnStatusBL;
            _sysPayGradeBL = sysPayGradeBL;
            _SurveyQuestionBL = SurveyQuestionBL;
            _ISurveyReplyAnswerBL = ISurveyReplyAnswerBL;
            _videoManageBL = videoManageBL;
            _CoVideoResourceBL = CoVideoResourceBL;
        }

        /// </summary>
        /// <param name="PopCourseType">当前页面 0: 弹出层页面引用 ，1：引用母版页</param>
        /// 
        /// 课程体系树
        /// <param name="type">课程体系树        0: 弹出层页面引用 ，1：引用母版页</param>
        /// <param name="checkBoxFlag">0：木有复选框，1：前置复选框</param>
        ///<param name="showButton">0：木有确定按钮，1：显示确定按钮</param>
        ///
        /// 课程列表
        /// <param name="showSearch">显示搜索：0 不显示 1：显示</param>
        /// <param name="showOperation">显示操作列：0不显示：1：显示</param>
        /// <param name="showCourseButton">显示确认按钮：0不显示：1：显示</param>
        /// <param name="showCourseCheckbox">显示复选框按钮：0不显示：1：显示</param>   
        /// <param name="typeCourse">课程列表       显示 0:弹出层页面引用 ， 1：引用母版页</param>  
        /// <returns></returns>
        public ActionResult PopCourseCommonList(int PopCourseType=0,int type = 0, int checkBoxFlag = 0, int showButton = 0, int showSearch = 0, int showOperation = 0, int showCourseButton = 0, int showCourseCheckbox = 0, int typeCourse = 0)
        {
            ViewBag.PopCourseType = PopCourseType;
            ViewBag.Type = type;
            ViewBag.checkBoxFlag = checkBoxFlag;
            ViewBag.showButton = showButton;

            ViewBag.showSearch = showSearch;
            ViewBag.showOperation = showOperation;
            ViewBag.showCourseButton = showCourseButton;
            ViewBag.showCourseCheckbox = showCourseCheckbox;
            ViewBag.typeCourse = typeCourse;
            return View();
        }
    }
}
