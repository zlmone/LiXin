using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DepTranAttendce;
using LiXinModels.DepTranAttendce;
using LiXinInterface.DepTranManage;

namespace LiXinBLL.DepTranManage
{
    public class DepTran_AttendceApprovalRecordBL : IDepTran_AttendceApprovalRecord
    {
        private static DepTran_AttendceApprovalRecordDB dep_attendceRecordbl;
        public DepTran_AttendceApprovalRecordBL()
        {
            dep_attendceRecordbl = new DepTran_AttendceApprovalRecordDB();
        }

        /// <summary>
        /// 新增审批记录
        /// </summary>
        public int AddDepTranApprovalRecord(DepTran_AttendceApprovalRecord model)
        {
            dep_attendceRecordbl.InsertDepTran_AttendceApprovalRecord(model);
            return model.Id;
        }

        /// <summary>
        /// 修改审批记录
        /// </summary>
        public bool UpdateDepTranApprovalRecord(DepTran_AttendceApprovalRecord model)
        {
            return dep_attendceRecordbl.UpdateDepTran_AttendceApprovalRecord(model);
        }

        /// <summary>
        /// 获取审批记录
        /// </summary>
        /// <returns></returns>
        public List<DepTran_AttendceApprovalRecord> GetDepTranApprovalRecordlist(int courseID, int deptId, out int totalcount, int startIndex = 1, int startLength = int.MaxValue)
        {
            var list = dep_attendceRecordbl.GetDepTran_AttendceApprovalRecord(courseID, deptId, startIndex, startLength);
            totalcount = list.Count > 0 ? list[0].totalcount : 0;
            return list;
        }

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
        public bool Insert(int courseId, int departId, int userId, DateTime ApprovalTime, int ApprovalFlag,
                           string reason, int SubmitUserId, DateTime SubmitTime)
        {
            return dep_attendceRecordbl.Insert(courseId, departId, userId, ApprovalTime, ApprovalFlag, reason,
                                               SubmitUserId, SubmitTime);
        }
    }
}
