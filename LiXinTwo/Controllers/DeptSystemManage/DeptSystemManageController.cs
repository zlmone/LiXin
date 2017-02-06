using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;
using LiXinModels;
using System.Web.Mvc;
using LiXinInterface;
using LiXinInterface.User;

namespace LiXinControllers
{
    public partial class DeptSystemManageController : BaseController
    {
        protected IDept_Notes noteBL;
        protected IUser userBL;
        protected IDepartment deptBL;
        protected IDept_NoteSort noteSortBL;

        public DeptSystemManageController(IDept_Notes _noteBL,
            IUser _userBL,
            IDepartment _deptBL, IDept_NoteSort _noteSortBL)
        {
            noteBL = _noteBL;
           userBL = _userBL;
           deptBL = _deptBL;
           noteSortBL = _noteSortBL;
        }
    }
}
