using LiXinInterface.User;
using LiXinControllers;
using LiXinInterface;
using LiXinInterface.PlanManage;
using LiXinInterface.AttendceManage;
using LiXinInterface.CourseLearn;
using LiXinInterface.SystemManage;

namespace LiXinControllers.User
{
    public partial class UserManageController : BaseController
    {
        private readonly object lockobj = new object();

        protected IDepartment deptBL;
        protected IPost postBL;
        protected IRight rightBL;
        protected IRole roleBL;
        protected IUser userBL;
        protected ITr_MonthPlan Imonth;
        protected ITr_YearPlan Iyear;
        protected ICl_Attendce iattendce;
        public IFree_UserOtherApply userApplyBL;
        public UserManageController(IUser _userBL, IRole _roleBL, IPost _postBL, IRight _rightBL, IDepartment _deptBL, ITr_MonthPlan _Imonth, ITr_YearPlan _Iyear, ICl_Attendce _iattendce,IFree_UserOtherApply _userApplyBL)
        {
            userBL = _userBL;
            roleBL = _roleBL;
            postBL = _postBL;
            deptBL = _deptBL;
            rightBL = _rightBL;
            Imonth = _Imonth;
            Iyear = _Iyear;
            iattendce = _iattendce;
            userApplyBL = _userApplyBL;
        }
    }
}