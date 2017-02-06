using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;
using LiXinInterface.PlanManage;
using LiXinInterface.CourseManage;
using LiXinSync;
using System.Web.Mvc;
using LiXinInterface.CourseLearn;
using LiXinInterface.ClassRoom;
namespace LiXinControllers
{
    public partial class PlanManageController : BaseController
    {
        protected ITr_MonthPlan Imonth;
        protected ITr_YearPlan Iyear;
        protected ICo_Course Icourse;
        protected ISys_TrainGrade trainBL;
        protected ICourseOrder courseOrderBL;
        protected IClassRoom classRoomBL;
        public PlanManageController(ITr_MonthPlan _Imonth, ITr_YearPlan _Iyear, ICo_Course _Icourse, ISys_TrainGrade _trainBL, ICourseOrder _courseOrderBL,
        IClassRoom _classRoomBL)
        {
            Imonth = _Imonth;
            Iyear = _Iyear;
            Icourse = _Icourse;
            trainBL = _trainBL;
            courseOrderBL = _courseOrderBL;
            classRoomBL = _classRoomBL;
        }


        #region 主页

        public ActionResult Index()
        {
            int totalCount = 0;

            //待开课课程
            string waitCoursewhere = string.Format("  Co_Course.IsDelete = 0 and CourseFrom=2 and Co_Course.way=1 and Co_Course.PublishFlag = 1 and Co_Course.StartTime  >='{0}' ", DateTime.Now);
            var waitCourseList = Icourse.GetCourseCommonList(out totalCount, waitCoursewhere, 0, 5, "ORDER BY Co_Course.StartTime");

            //待审批逾时申请
            string timeoutApprovalwhere = string.Format(@" Co_Course.IsDelete = 0 and Co_Course.publishflag =1 and Sys_User.IsDelete = 0 and Cl_MakeUpOrder.IsTimeOut = 1 and Cl_MakeUpOrder.TimeOutPassFlag = 0");
            var timeoutApprovalList = courseOrderBL.GetTimeOutApprovalList(out totalCount, timeoutApprovalwhere, 1, 5, "order by Cl_MakeUpOrder.TimeOutDateTime desc");

            //课程预约情况
            string courseOrderListwhere = string.Format(@" Co_Course.IsDelete = 0 and Co_Course.Publishflag = 1 and Co_Course.CourseFrom=2 and Co_Course.Way = 1  ");
            var courseOrderList = courseOrderBL.GetCourseSubscribeList(out totalCount, courseOrderListwhere, 1, 6, "order by Co_Course.Id desc");

            ViewBag.waitCourseList = waitCourseList;
            ViewBag.timeoutApprovalList = timeoutApprovalList;
            ViewBag.courseOrderList = courseOrderList;
            ViewBag.UserRights = UserRights;
           
            return View();
        }

        #endregion
    }
}
