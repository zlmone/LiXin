using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseManage;

namespace LiXinInterface.CourseManage
{
    public interface ICl_VideoManage
    {
        int Add(Cl_VideoManage model);

        bool Modify(Cl_VideoManage model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        List<Cl_VideoManage> GetCl_VideoManageList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                              int pageLength = int.MaxValue, string orderBy =  "ORDER BY Cl_VideoManage.Id DESC");
    }
}
