using LiXinInterface.Report_CPA;
using LiXinInterface.SystemManage;
using LiXinModels.Report_CPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinControllers
{
    public partial class Report_CPAController:BaseController
    {
        private IReport_CPA CPABL;
        protected IFreeConfig freeConfigBL;
        public Report_CPAController(IReport_CPA _CPABL, IFreeConfig _freeConfigBL)
        {
            CPABL = _CPABL;
            freeConfigBL = _freeConfigBL;
        }
    }
}
