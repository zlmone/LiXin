using LiXinModels;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface
{
    public interface IReport_Free
    {

        /// <summary>
        /// 获取授课人
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<ReportFreeDetails> GetCPATeacherList(string where = "1=1", int cpa = -1);

        /// <summary>
        /// 获取其他形式
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<ReportFreeDetails> GetOtherList(int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " GetCPAScore asc");

        /// <summary>
        /// 获取符合时间的人员
        /// </summary>
        /// <param name="JoinDate"></param>
        /// <param name="CPACreateDate"></param>
        /// <returns></returns>
        List<Free_UserOtherApply> GetUserByDate(string JoinDate, string CPACreateDate, string where = "1=1");

         /// <summary>
        /// 所有免修的人员ID
        /// </summary>
        /// <returns></returns>
        List<int> GetFreeUserList(string where = "1=1");
    }
}
