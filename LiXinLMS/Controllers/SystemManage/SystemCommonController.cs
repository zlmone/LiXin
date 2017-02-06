using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;
using LiXinModels;
using System.Web.Mvc;
using LiXinInterface.SystemManage;
using LiXinInterface.User;

namespace LiXinControllers
{
    public partial class SystemManageController : BaseController
    {
        protected ISys_Notes noteBL;
        protected ISys_Log logBL;
        protected ISys_Group groupBL;
        protected IUser userBL;
        protected ISys_ParamConfig paramconfigBL;
        protected ISys_LogTrain logTrainBL;
        protected IDepartment deptBL;
        protected ISys_NoteSort noteSortBL;
        protected ISys_Leader learderBL;
        protected ISys_NoteResource sys_noteresourceBL;

        public SystemManageController(ISys_Notes _noteBL, ISys_Group _groupBL,ISys_Log _logBL,
            IUser _userBL,ISys_ParamConfig _paramconfigBL,ISys_LogTrain _logTrainBL,
            IDepartment _deptBL, ISys_NoteSort _noteSortBL, ISys_Leader _learderBL, ISys_NoteResource _sys_noteresourceBL)
        {
            noteBL = _noteBL;
            logBL=_logBL;
           groupBL = _groupBL;
           userBL = _userBL;
           paramconfigBL = _paramconfigBL;
           logTrainBL = _logTrainBL;
           deptBL = _deptBL;
           noteSortBL = _noteSortBL;
           learderBL = _learderBL;
           sys_noteresourceBL = _sys_noteresourceBL;
        }
    }
}
