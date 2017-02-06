using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;
using LiXinInterface.CourseManage;
using LiXinDataAccess;
using LiXinDataAccess.CourseManage;
namespace LiXinBLL.CourseManage
{
    public class Co_SystemLinkCourseBL : ICo_SystemLinkCourse
    {
        private readonly Co_SystemLinkCourseDB _sysLinkCourseDB = new Co_SystemLinkCourseDB();

        public List<Co_SystemLinkCourse> GetSysLinkCourseListBySystemId(int SystemId)
        {
            return _sysLinkCourseDB.GetSysLinkCourseListBySystemId(SystemId);
        }

       /// <summary>
       /// 根据SystemId  返回CourseId
       /// </summary>
       /// <param name="SystemId"></param>
       /// <returns></returns>
        public List<int> GetCourseIdListBySystemId(int SystemId)
        {
            return _sysLinkCourseDB.GetCourseIdListBySystemId(SystemId);
        }
    }
}
