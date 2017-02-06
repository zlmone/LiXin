using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DepTranAttendce;
using LiXinInterface.DepTranManage;
using LiXinModels.DepTranAttendce;

namespace LiXinBLL.DepTranManage
{
    public class DepTranAttendceApprovalBL : IDepTranAttendceApproval
    {
        protected DepTranAttendceApprovalDB ApprovalDB=new DepTranAttendceApprovalDB();

        /// <summary>
        /// 获得审批的数据列表(有分页)
        /// </summary>
        /// <param name="startIndex">当前页</param>
        /// <param name="startLength">条数</param>
        /// <param name="order">排序方式</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public List<AttendceCourseModel> GetDepTranAttendceApprovalList(string whereFlag, int startIndex = 1, int startLength = int.MaxValue, string order = " a.SubmitTime desc", string where = "1=1")
        {
            return ApprovalDB.GetDepTranAttendceApprovalList(whereFlag,startIndex, startLength, order, where);
        }

        /// <summary>
        /// 获得考勤人员数据列表
        /// </summary>
        public List<AttendceCourseModel> GetDepTran_AttendceUserList(int courseId, string depIds, int startIndex = 1,
                                                                  int startLength = int.MaxValue, string Order = "asc",
                                                                  string where = "1=1")
        {
            return ApprovalDB.GetDepTran_AttendceUserList(courseId, depIds, startIndex, startLength, Order, where);
        }

        /// <summary>
        /// 跟新审批状态
        /// </summary>
        public bool DepTranAttendceApprovalUsers(string courseId, int userId, string reason, int approval)
        {
            return ApprovalDB.DepTranAttendceApprovalUsers(courseId, userId, reason, approval);
        }

        /// <summary>
        /// 获取人员数据列表
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="depatId"></param>
        /// <returns></returns>
        public List<AttendceCourseModel> GetUserList(int courseId, int depatId)
        {
            return ApprovalDB.GetUserList(courseId, depatId);
        }
    }
}
