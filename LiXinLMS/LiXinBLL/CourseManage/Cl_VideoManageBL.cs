using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.CourseManage;
using LiXinInterface.CourseManage;
using LiXinModels.CourseManage;
namespace LiXinBLL.CourseManage
{
    public class Cl_VideoManageBL : ICl_VideoManage
    {
        private readonly Cl_VideoManageDB _videoManageDAL = new Cl_VideoManageDB();

        public int Add(Cl_VideoManage model)
        {
            return _videoManageDAL.Add(model);
        }

        public bool Modify(Cl_VideoManage model)
        {
            return _videoManageDAL.Modify(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Cl_VideoManage> GetCl_VideoManageList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                              int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_VideoManage.Id DESC")
        {
            return _videoManageDAL.GetCl_VideoManageList(out totalCount,where,startIndex,pageLength,orderBy); 
        }
        
    }
}
