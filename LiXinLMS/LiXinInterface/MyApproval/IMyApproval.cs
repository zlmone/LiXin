using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;
using LiXinModels.CourseLearn;

namespace LiXinInterface.MyApproval
{
    /// <summary>
    /// 我的审批记录接口
    /// </summary>
    public interface IMyApproval
    {
        /// <summary>
        /// 获取我审批的请假记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<BaseApprovalInfor> GetMyApprovalLeavePaging(out int totalCount, string where = " 1 = 1 ",
                                                         int startIndex = 0, int pageLength = int.MaxValue,
                                                         string orderBy = " order by LeaveTime desc ");

        /// <summary>
        /// 获取单个请假审批详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ApprovalUserInfor GetMyApprovalLeaveInfor(int id);

        /// <summary>
        /// 获取我审批的补预定记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<BaseApprovalInfor> GetMyApprovalMakeUpPaging(out int totalCount, string where = " 1 = 1 ",
                                                                 int startIndex = 0, int pageLength = int.MaxValue,
                                                                 string orderBy = " order by LeaveTime desc ");

        /// <summary>
        /// 获取单个补预定审批详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ApprovalUserInfor GetMyApprovalMakeUpInfor(int id);

        /// <summary>
        /// 获取我审批的逾时申请记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<BaseApprovalInfor> GetMyApprovalOutTimePaging(out int totalCount, string where = " 1 = 1 ",
                                                                  int startIndex = 0, int pageLength = int.MaxValue,
                                                                  string orderBy = " order by a.OutTime desc ");


        /// <summary>
        /// 获取单个逾时补预定审批详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ApprovalUserInfor GetMyApprovalOutTimeInfor(int id);

        /// <summary>
        /// 获取单个逾时补预定审批详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ApprovalUserInfor GetMyApprovalOutTimeLeaveInfor(int id);

        /// <summary>
        /// 获取需要我审批的课程违纪记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<PrincipleInfor> GetMyApprovalPrinciplePaging(out int totalCount, string where = " and 1 = 1 ",
                                                                 int startIndex = 0, int pageLength = int.MaxValue,
                                                                 string orderBy = " order by a.AppDateTime desc ");

        /// <summary>
        /// 获取单个课程违纪详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ApprovalUserInfor GetMyApprovalPrincipleInfor(int id);

        /// <summary>
        /// 获取我的违纪情况列表
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<PrincipleInfor> GetMyPrinciplePaging(out int totalCount, string where = " and 1 = 1 ",
                                                         int startIndex = 0, int pageLength = int.MaxValue,
                                                         string orderBy = " order by b.StartTime desc ");

        /// <summary>
        /// 获取我的违纪情况详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ApprovalUserInfor GetMyPrincipleInfor(int id);

        /// <summary>
        /// 申辩/审批违纪
        /// </summary>
        /// <param name="sqlstr">sql</param>
        /// <returns></returns>
        int ExplainPrin(string sqlstr);

        /// <summary>
        /// 获取需要我审批的课程违纪记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<PrincipleInfor> GetPrincipleManagePaging(out int totalCount, string where = " and 1 = 1 ",
                                                             int startIndex = 0, int pageLength = int.MaxValue,
                                                             string orderBy = " order by a.AppDateTime desc ");

        /// <summary>
        /// 获取申辩信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AttendceInfor GetPrinReason(int id);

        /// <summary>
        /// 获取审批的课程违纪详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ApprovalUserInfor GetPrincipleInfor(int id);

        /// <summary>
        /// 我的违纪情况申请记录列表
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<PrincipleInfor> GetMyPrincipleRecordPaging(out int totalCount, string where = " and 1 = 1 ",
                                                               int startIndex = 0, int pageLength = int.MaxValue,
                                                               string orderBy = " order by b.StartTime desc ");

        /// <summary>
        /// 获取课程违纪记录（培训管理员）
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<PrincipleInfor> GetPrincipleTrainManagePaging(out int totalCount, string where = " and 1 = 1 ",
                                                                  int startIndex = 0, int pageLength = int.MaxValue,
                                                                  string orderBy = " order by b.StartTime desc ");

        /// <summary>
        /// 获取单个课程预定信息
        /// </summary>
        /// <param name="sqlwhere">查询条件</param>
        /// <returns></returns>
        Cl_CourseOrder GetCourseOrder(string sqlwhere);

        /// <summary>
        /// 更新预定信息
        /// </summary>
        /// <param name="sqlstr">sql语句</param>
        /// <returns></returns>
        bool ChangeCourseOrderInfor(string sqlstr);

        /// <summary>
        /// 我的请假逾时申请
        /// </summary>
        List<Cl_CourseOrder> GetMyTimeOutOrder(out int totalCount, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Id desc");


        /// <summary>
        /// 新增一条数据
        /// </summary>     
        void InsertCl_TimeOutLeave(string Memo, int id, string LeaderID);

        /// <summary>
        /// 请假逾时申请审核
        /// </summary>
        List<Cl_CourseOrder> GetTimeOutLeave(out int totalCount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Id desc");

        /// <summary>
        /// 请假逾时申请审核动作
        /// </summary>
        void UpdateTimeOutLeave(string ids, string approvalmemo, int approval);

        /// <summary>
        /// 请假申请审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Cl_TimeOutLeave GetSingleTimeOutLeave(int id);

        /// <summary>
        /// 获取我审批的逾时申请记录（请假）
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<BaseApprovalInfor> GetMyApprovalOutTimePagingLeave(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by a.OutTime desc ");
    }
}
