using LiXinInterface.DeptPlanManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;
using LiXinInterface.User;
using LiXinInterface.ClassRoom;
using LiXinInterface.DepCourseManage;


namespace LiXinControllers.DeptPlanManage
{
    public partial class DeptPlanManageController : BaseController
    {
        protected IDep_MonthPlan monthBL;
        protected IDep_YearPlan Iyear;
        protected ISys_TrainGrade trainBL;
        protected IDepartment depBL;
        protected IDepClassRoom roomBL;
        protected IUser userBL;
        protected IDep_Course Idep_courseBL;

        public DeptPlanManageController(IDep_MonthPlan _monthBL, IDep_YearPlan _Iyear, ISys_TrainGrade _trainBL,
            IDepartment _depBL, IDepClassRoom _roomBL, IUser _userBL, IDep_Course _Idep_courseBL)
        {
            monthBL = _monthBL;
            Iyear = _Iyear;
            trainBL = _trainBL;
            depBL = _depBL;
            roomBL = _roomBL;
            userBL = _userBL;
            Idep_courseBL = _Idep_courseBL;
        }
    }
}
