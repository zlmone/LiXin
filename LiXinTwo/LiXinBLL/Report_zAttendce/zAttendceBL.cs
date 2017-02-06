using LiXinDataAccess;
using LiXinInterface.Report_zAttendce;
using LiXinModels.Report_zAttendce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;
using System.Diagnostics;
using Retech.Core.Cache;
using Retech.Cache;
using LiXinBLL.Cache;
using MongoDB.Driver.Builders;
using LiXinModels.CourseLearn;
using LiXinModels;

namespace LiXinBLL.Report_zAttendce
{
    public class zAttendceBL : IzAttendce
    {
        private zAttendceDB zaDb;

        public zAttendceBL()
        {
            zaDb = new zAttendceDB();
        }

        /// <summary>
        /// 根据条件获得所有的缺勤情况
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<zAttendce> GetAllAttendce(string where, string whereDate = "", string whereDateT = "",
                                              string whereDateL = "", string deptName = "", int startIndex = 0, int pageLength = int.MaxValue,
                                              string jsRenderSortField = "", string whereDeptID = "")
        {
            var list = zaDb.GetAllAttendce(where, whereDate, whereDateT, whereDateL, startIndex, pageLength,
                                       jsRenderSortField, deptName, whereDeptID);

            var newlist = new List<zAttendce>();
            foreach (var item in list)
            {
                if (newlist.Count(p => p.DeptName == item.fenDeptName) == 0)
                {
                    var singleList = list.Where(p => p.fenDeptName == item.fenDeptName);
                    var model = new zAttendce();
                    model.ZongIS = item.IsZong;

                    model.DeptName = item.fenDeptName;
                    model.AttSum2 = singleList.Sum(p => p.AttSum);
                    model.LateSum2 = singleList.Sum(p => p.LateSum); ;
                    model.EarlySum2 = singleList.Sum(p => p.EarlySum); ;
                    model.LateAndEarlySum2 = singleList.Sum(p => p.LateAndEarlySum);
                    model.deptIDs = string.Join(",", singleList.Select(p => p.DepartmentId).Distinct());
                    newlist.Add(model);


                }
            }
            return newlist;
        }

        /// <summary>
        /// 查找所有分所
        /// </summary>
        /// <returns></returns>
        public List<zAttendce> GetFengDepartment(string whereDeptID = "")
        {
            return zaDb.GetFengDepartment(whereDeptID);
        }

        /// <summary>
        /// 每个人的考勤状态
        /// </summary>
        /// <returns></returns>
        public List<Cl_CourseOrder> GetUserAttend(int YearPlan)
        {
            return zaDb.GetUserAttend(YearPlan);
        }

        
    }
}
