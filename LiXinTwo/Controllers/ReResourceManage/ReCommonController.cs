using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;
using LiXinInterface.SystemManage;
using LiXinModels;
using System.Web.Mvc;
using LiXinInterface.ReResourceManage;
using LiXinInterface.User;

namespace LiXinControllers
{
    public partial class ReResourceManageController : BaseController
    {
        protected ISys_Group SysGroupBl;
        protected IDepartment DepartmentBl;
        protected IPost PostBl;
        protected IRight RightBl;
        protected IRole RoleBl;
        protected IUser UserBl;
        protected IReResourceType ReResourceTypeBl;
        protected IReResource ReResourceBl;
        protected IReMyResource ReMyResourceBl;
        public ReResourceManageController(
            ISys_Group sysGroupBl
            ,IDepartment departmentBl
            , IPost postBl
            , IRight rightBl
            , IRole roleBl
            , IUser userBl
            , IReResourceType reResourceTypeBl
            , IReResource reResourceBl
            , IReMyResource reMyResourceBl
            )
        {
            SysGroupBl = sysGroupBl;
            DepartmentBl = departmentBl;
            PostBl = postBl;
            RightBl = rightBl;
            RoleBl = roleBl;
            UserBl = userBl;
            ReResourceTypeBl = reResourceTypeBl;
            ReResourceBl = reResourceBl;
            ReMyResourceBl = reMyResourceBl;
        }
    }
}
