using LiXinInterface.AttendceManage;
using LiXinInterface.CourseManage;
using LiXinInterface.SystemManage;

namespace LiXinControllers
{
    public partial class AttendceManageController : BaseController
    {
        public IAttendce IAtt;
        public ICo_Course Icourse;
        protected ISys_Leader leaderBL;
        public AttendceManageController(IAttendce _IAtt, ICo_Course _Icourse, ISys_Leader _leaderBL)
        {
            IAtt = _IAtt;
            Icourse = _Icourse;
            leaderBL = _leaderBL;
        }
    }
}
