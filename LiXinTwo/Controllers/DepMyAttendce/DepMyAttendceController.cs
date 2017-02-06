using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.DepCourseManage;


namespace LiXinControllers.DepMyAttendce
{
    public class DepMyAttendceController : BaseController
    {
        private IDep_CourseOrder _IDep_courseOrder;

        public DepMyAttendceController(IDep_CourseOrder IDep_courseOrder)
        {
            _IDep_courseOrder = IDep_courseOrder;
        }

    }
}
