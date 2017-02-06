using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;
using LiXinInterface.NewClassManage;
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
using LiXinInterface.NewCourseManage;
namespace LiXinControllers
{
    public partial class NewCourseManageController : BaseController
    {

        protected INew_Course _courseBL;
        protected INewClass _newclass;
        protected INew_CourseRoomRule _courseRoomRule;
        protected INewGroupUser _newGroupUser;
        protected INew_CoursePaper _newCoursePaper;
        protected INew_CourseFiles _newCourseFiles;
        protected INewClassRoom _newClassRoom;
        protected INew_CourseFiles _courseResourceBL;
        protected IExampaper _exampaperBL;
        protected ISurveyExampaper _surveyExampaperBL;
        protected IUser _userBL;
        protected INewGroup _newGroup;
        protected INew_CourseOrder _newCourseOrder;
        protected INew_CourseOrderDetail _courseOrderDetail;


        public NewCourseManageController(INew_Course courseBL,INewClass newclass, INew_CourseFiles courseResourceBL,
 INew_CoursePaper newCoursePaper, INew_CourseFiles newCourseFiles, INew_CourseRoomRule courseRoomRule, INewClassRoom newClassRoom, INewGroupUser newGroupUser, IExampaper exampaperBL, ISurveyExampaper surveyExampaperBL, IUser userBL, INewGroup newGroup, INew_CourseOrder newCourseOrder, INew_CourseOrderDetail courseOrderDetail)
        {
            _courseBL = courseBL;
            _newclass = newclass;
            _newClassRoom = newClassRoom;
            _newCoursePaper = newCoursePaper;
            _courseRoomRule = courseRoomRule;
            _newGroupUser = newGroupUser;
            _courseResourceBL = courseResourceBL;
            _newCourseFiles = newCourseFiles;
            _exampaperBL = exampaperBL;
            _surveyExampaperBL = surveyExampaperBL;
            _userBL = userBL;
            _newGroup = newGroup;
            _newCourseOrder = newCourseOrder;
            _courseOrderDetail = courseOrderDetail;
        }
    }
}
