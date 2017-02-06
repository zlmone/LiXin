using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DepLeaveApproval;
using LiXinInterface.DepLeaveApproval;
using LiXinModels;
using LiXinModels.DepCourseLearn;

namespace LiXinBLL.DepLeaveApproval
{
    public class DepLeaveApprovalBL : IDepLeaveApproval
    {
        protected DepLeaveApprovalDB ApprovalDB = new DepLeaveApprovalDB();
        /// <summary>
        /// 获取要审批的请假列表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageCount">每页条数</param>
        /// <param name="order">排序方式</param>
        /// <param name="where">查询条件</param>
        /// <param name="hour">配置多少小时候审批默认拒绝</param>
        /// <returns></returns>
        public List<DepLeaveInfor> GetDepLeaveApprovalList(int pageIndex = 1, int pageCount = int.MaxValue, string order = " o.LeaveTime desc ", string where = " and 1=1 ",int hour=999)
        {
            return ApprovalDB.GetDepLeaveApprovalList(pageIndex, pageCount, order, where,hour);
        }

        /// <summary>
        /// 根据ID获取当个请假信息
        /// </summary>
        /// <param name="id">当前页</param>
        /// <param name="hour">配置多少小时候审批默认拒绝</param>
        /// <returns></returns>
        public DepLeaveInfor GetDepLeaveApprovalInfor(int id,int hour=999)
        {
            return ApprovalDB.GetDepLeaveApprovalInfor(id,hour);
        }

        /// <summary>
        /// 根据ID获取当个请假信息
        /// </summary>
        /// <param name="id">请假记录ID</param>
        /// <param name="appFlag">审批结果</param>
        /// <param name="reason">审批理由</param>
        /// <param name="appTime">审批时间</param>
        /// <param name="appUserId">审批人ID</param>
        /// <returns></returns>
        public bool UpdateLeaveInfor(int id, int appFlag, string reason, DateTime appTime, int appUserId)
        {
            return ApprovalDB.UpdateLeaveInfor(id, appFlag, reason, appTime, appUserId);
        }

        /// <summary>
        /// 获取相关配置
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <param name="key">配置类型</param>
        /// <returns></returns>
        public Sys_ParamConfig GetConfig(int deptId,int key)
        {
            return ApprovalDB.GetConfig(deptId, key);
        }
    }
}
