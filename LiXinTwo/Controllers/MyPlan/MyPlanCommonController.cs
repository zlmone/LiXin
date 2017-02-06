using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;

namespace LiXinControllers
{
    public partial class MyPlanController : BaseController
    {
        private ITr_MonthPlan monthBL;
        public MyPlanController(ITr_MonthPlan _monthBL)
        {
            monthBL = _monthBL;
        }
    }
}
