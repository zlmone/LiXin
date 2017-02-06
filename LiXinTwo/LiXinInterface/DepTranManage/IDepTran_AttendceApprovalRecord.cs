using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.DepTranAttendce;

namespace LiXinInterface.DepTranManage
{
    public interface IDepTran_AttendceApprovalRecord
    {
        /// <summary>
        /// 新增审批记录
        /// </summary>
        int AddDepTranApprovalRecord(DepTran_AttendceApprovalRecord model);

        /// <summary>
        /// 修改审批记录
        /// </summary>
        bool UpdateDepTranApprovalRecord(DepTran_AttendceApprovalRecord model);

        /// <summary>
        /// 获取审批记录
        /// </summary>
        List<DepTran_AttendceApprovalRecord> GetDepTranApprovalRecordlist(int courseID, int deptId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue);

        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="departId"></param>
        /// <param name="userId"></param>
        /// <param name="ApprovalTime"></param>
        /// <param name="ApprovalFlag"></param>
        /// <param name="reason"></param>
        /// <param name="SubmitUserId"></param>
        /// <param name="SubmitTime"></param>
        /// <returns></returns>
        bool Insert(int courseId, int departId, int userId, DateTime ApprovalTime, int ApprovalFlag, string reason, int SubmitUserId, DateTime SubmitTime);
    }
}
