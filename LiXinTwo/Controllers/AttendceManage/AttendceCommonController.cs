using LiXinInterface.AttendceManage;
using LiXinInterface.CourseManage;
using LiXinInterface.SystemManage;
using LiXinInterface.User;

namespace LiXinControllers
{
    public partial class AttendceManageController : BaseController
    {
        public IAttendce IAtt;
        public ICo_Course Icourse;
        protected ISys_Leader leaderBL;
        protected IUser userBL;
        protected IFreeConfig freeConfigBL;
        protected IFree_UserOtherApply userApplyBL;
        public AttendceManageController(IAttendce _IAtt, ICo_Course _Icourse, ISys_Leader _leaderBL, IUser _userBL, IFreeConfig _freeConfigBL, IFree_UserOtherApply _userApplyBL)
        {
            IAtt = _IAtt;
            Icourse = _Icourse;
            leaderBL = _leaderBL;
            userBL = _userBL;
            freeConfigBL = _freeConfigBL;
            userApplyBL = _userApplyBL;
        }
    }
}
