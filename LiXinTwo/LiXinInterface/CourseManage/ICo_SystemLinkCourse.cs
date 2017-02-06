using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;

namespace LiXinInterface.CourseManage
{
    public interface ICo_SystemLinkCourse
    {
       List<Co_SystemLinkCourse> GetSysLinkCourseListBySystemId(int SystemId);

       /// <summary>
       /// 根据SystemId  返回CourseId
       /// </summary>
       /// <param name="SystemId"></param>
       /// <returns></returns>
       List<int> GetCourseIdListBySystemId(int SystemId);
    }
}
