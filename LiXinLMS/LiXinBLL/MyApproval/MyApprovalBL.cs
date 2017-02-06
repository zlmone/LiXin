using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess;
using LiXinDataAccess.CourseLearn;
using LiXinDataAccess.MyApproval;
using LiXinInterface.MyApproval;
using LiXinModels;
using LiXinModels.CourseLearn;

namespace LiXinBLL.MyApproval
{
    public class MyApprovalBL : IMyApproval
    {
        protected static MyApprovalDB ApprovalDb;
        private static CourseOrderDB CoDB;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MyApprovalBL()
        {
            ApprovalDb = new MyApprovalDB();
            CoDB = new CourseOrderDB();
        }
        /// <summary>
        /// 获取我审批的请假记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<BaseApprovalInfor> GetMyApprovalLeavePaging(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by LeaveTime desc ")
        {
            return ApprovalDb.GetMyApprovalLeavePaging(out totalCount, where, startIndex, pageLength, orderBy);
        }
        /// <summary>
        /// 获取单个请假审批详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetMyApprovalLeaveInfor(int id)
        {
            return ApprovalDb.GetMyApprovalLeaveInfor(id);
        }

        /// <summary>
        /// 获取单个补预定审批详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetMyApprovalMakeUpInfor(int id)
        {
            return ApprovalDb.GetMyApprovalMakeUpInfor(id);
        }

        /// <summary>
        /// 获取单个逾时补预定审批详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetMyApprovalOutTimeInfor(int id)
        {
            return ApprovalDb.GetMyApprovalOutTimeInfor(id);
        }

        /// <summary>
        /// 获取单个逾时补预定审批详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetMyApprovalOutTimeLeaveInfor(int id)
        {
            return ApprovalDb.GetMyApprovalOutTimeLeaveInfor(id);
        }

        /// <summary>
        /// 获取我审批的补预定记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<BaseApprovalInfor> GetMyApprovalMakeUpPaging(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by LeaveTime desc ")
        {
            return ApprovalDb.GetMyApprovalMakeUpPaging(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 获取我审批的逾时申请记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<BaseApprovalInfor> GetMyApprovalOutTimePaging(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by a.OutTime desc ")
        {
            return ApprovalDb.GetMyApprovalOutTimePaging(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 获取需要我审批的课程违纪记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<PrincipleInfor> GetMyApprovalPrinciplePaging(out int totalCount, string where = " and 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by a.AppDateTime desc ")
        {
            return ApprovalDb.GetMyApprovalPrinciplePaging(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 获取单个课程违纪详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetMyApprovalPrincipleInfor(int id)
        {
            return ApprovalDb.GetMyApprovalPrincipleInfor(id);
        }

        /// <summary>
        /// 获取我的违纪情况列表
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<PrincipleInfor> GetMyPrinciplePaging(out int totalCount, string where = " and 1 = 1 ",
                                                          int startIndex = 0, int pageLength = int.MaxValue,
                                                          string orderBy = " order by b.StartTime desc ")
        {
            return ApprovalDb.GetMyPrinciplePaging(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 获取我的违纪情况详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetMyPrincipleInfor(int id)
        {
            return ApprovalDb.GetMyPrincipleInfor(id);
        }

        /// <summary>
        /// 申辩/审批违纪
        /// </summary>
        /// <param name="sqlstr">sql</param>
        /// <returns></returns>
        public int ExplainPrin(string sqlstr)
        {
            return ApprovalDb.ExplainPrin(sqlstr);
        }

        /// <summary>
        /// 获取需要我审批的课程违纪记录
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<PrincipleInfor> GetPrincipleManagePaging(out int totalCount, string where = " and 1 = 1 ",
                                                             int startIndex = 0, int pageLength = int.MaxValue,
                                                             string orderBy = " order by a.AppDateTime desc ")
        {
            return ApprovalDb.GetPrincipleManagePaging(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 获取申辩信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AttendceInfor GetPrinReason(int id)
        {
            return ApprovalDb.GetPrinReason(id);
        }

        /// <summary>
        /// 获取审批的课程违纪详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApprovalUserInfor GetPrincipleInfor(int id)
        {
            return ApprovalDb.GetPrincipleInfor(id);
        }

        /// <summary>
        /// 我的违纪情况申请记录列表
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<PrincipleInfor> GetMyPrincipleRecordPaging(out int totalCount, string where = " and 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by b.StartTime desc ")
        {
            return ApprovalDb.GetMyPrincipleRecordPaging(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 获取课程违纪记录（培训管理员）
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<PrincipleInfor> GetPrincipleTrainManagePaging(out int totalCount, string where = " and 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by b.StartTime desc ")
        {
            return ApprovalDb.GetPrincipleTrainManagePaging(out totalCount, where, startIndex, pageLength, orderBy);
        }

        /// <summary>
        /// 获取单个课程预定信息
        /// </summary>
        /// <param name="sqlwhere">查询条件</param>
        /// <returns></returns>
        public Cl_CourseOrder GetCourseOrder(string sqlwhere)
        {
            return CoDB.Getmodel(sqlwhere);
        }

        /// <summary>
        /// 更新预定信息
        /// </summary>
        /// <param name="sqlstr">sql语句</param>
        /// <returns></returns>
        public bool ChangeCourseOrderInfor(string sqlstr)
        {
            return CoDB.ChangeCourseOrderInfor(sqlstr);
        }

        #region 补预定流程的处理

        /// <summary>
        /// 处理补预定流程的审批结果
        /// </summary>
        public void DealMakeUpCourseOrder(int times=0)
        {

            var makeupDb = new Cl_MakeUpOrderDB();
            var cpaDB = new Cl_CpaLearnStatusDB();
            //获取未审批的补预定数据
            var corderList = CoDB.GetAllList(string.Format("select CourseId,UserId,GetScore from Cl_CourseOrder where MakeUpApprovalFlag=0 and OrderStatus=3 order by Cl_CourseOrder.courseid,Cl_CourseOrder.approvaldatetime"));
            for (var i = 0; i < corderList.Count; i++)
            {
                var userID = Convert.ToInt32(corderList[i].UserId);
                var courseID = Convert.ToInt32(corderList[i].CourseId);
                var cmake = makeupDb.GetModel(string.Format(" where UserId={0} and CourseId={1}", userID, courseID));
                if (cmake != null)
                {
                    if (cmake.ApprovalFlag == 0 || (cmake.ApprovalFlag != 0 && cmake.ApprovalDateTime > cmake.ApprovalLimitTime))
                    {
                        //进入逾时补预定
                        var ctime = ApprovalDb.GetTimeOutOrder(string.Format(" where UserId={0} and CourseId={1}", userID, courseID));
                        if (ctime != null && ctime.ApprovalFlag != 0)
                        {
                            //判断是否达到补预定的上限
                            var havetime = CoDB.JudgeMakeUpTimes(userID);
                            if (havetime < times)
                            {
                                //正常审批
                                CoDB.ChangeCourseOrderInfor(
                                    string.Format(
                                        "update Cl_CourseOrder set GetScore={0}, MakeUpApprovalFlag={1} where UserId={2} and CourseId={3}",
                                        cmake.ApprovalFlag == 1 ? corderList[i].GetScore : 0M, cmake.ApprovalFlag,
                                        userID, courseID));
                                if (cmake.ApprovalFlag == 2)
                                {
                                    cpaDB.UpdateCPALearnStatus(
                                        string.Format(
                                            " update Cl_CpaLearnStatus set GetLength={0} where CourseID={1} and UserID={2} and CpaFlag=2",
                                            0M, courseID, userID));
                                }
                            }
                            else
                            {
                                //正常审批
                                CoDB.ChangeCourseOrderInfor(
                                    string.Format(
                                        "update Cl_CourseOrder set GetScore={0}, MakeUpApprovalFlag={1} where UserId={2} and CourseId={3}",
                                        0M, cmake.ApprovalFlag, userID, courseID));
                                cpaDB.UpdateCPALearnStatus(
                                    string.Format(
                                        " update Cl_CpaLearnStatus set GetLength={0} where CourseID={1} and UserID={2} and CpaFlag=2",
                                        0M, courseID, userID));

                            }
                        }
                    }
                    else
                    {
                        //判断是否达到补预定的上限
                        var havetime = CoDB.JudgeMakeUpTimes(userID);
                        if (havetime < times)
                        {
                            //正常审批
                            CoDB.ChangeCourseOrderInfor(
                                string.Format(
                                    "update Cl_CourseOrder set GetScore={0}, MakeUpApprovalFlag={1} where UserId={2} and CourseId={3}",
                                    cmake.ApprovalFlag == 1 ? corderList[i].GetScore : 0M, cmake.ApprovalFlag,
                                    userID,
                                    courseID));
                            if (cmake.ApprovalFlag == 2)
                            {
                                cpaDB.UpdateCPALearnStatus(
                                    string.Format(
                                        " update Cl_CpaLearnStatus set GetLength={0} where CourseID={1} and UserID={2} and CpaFlag=2",
                                        0M, courseID, userID));
                            }
                        }
                        else
                        {
                            //正常审批
                            CoDB.ChangeCourseOrderInfor(
                                string.Format(
                                    "update Cl_CourseOrder set GetScore={0}, MakeUpApprovalFlag={1} where UserId={2} and CourseId={3}",
                                    0M, cmake.ApprovalFlag, userID, courseID));
                            cpaDB.UpdateCPALearnStatus(
                                string.Format(
                                    " update Cl_CpaLearnStatus set GetLength={0} where CourseID={1} and UserID={2} and CpaFlag=2",
                                    0M, courseID, userID));

                        }
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 我的请假逾时申请
        /// </summary>
        public List<Cl_CourseOrder> GetMyTimeOutOrder(out int totalCount, int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Id desc")
        {
            var list = ApprovalDb.GetMyTimeOutOrder(startIndex, startLength, where, jsRenderSortField);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }


        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertCl_TimeOutLeave(string Memo, int id, string LeaderID)
        {
            ApprovalDb.InsertCl_TimeOutLeave(Memo, id, LeaderID);
        }


        /// <summary>
        /// 请假逾时申请审核
        /// </summary>
        public List<Cl_CourseOrder> GetTimeOutLeave(out int totalCount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " Id desc")
        {
            var list = ApprovalDb.GetTimeOutLeave(startIndex, startLength, where, jsRenderSortField);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        /// <summary>
        /// 请假逾时申请审核动作
        /// </summary>
        public void UpdateTimeOutLeave(string ids, string approvalmemo, int approval)
        {
            ApprovalDb.UpdateTimeOutLeave(ids, approvalmemo, approval);
        }

        /// <summary>
        /// 请假申请审核
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cl_TimeOutLeave GetSingleTimeOutLeave(int id)
        {
            return ApprovalDb.GetSingleTimeOutLeave(id);
        }


        /// <summary>
        /// 获取我审批的逾时申请记录（请假）
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<BaseApprovalInfor> GetMyApprovalOutTimePagingLeave(out int totalCount, string where = " 1 = 1 ", int startIndex = 0, int pageLength = int.MaxValue, string orderBy = " order by a.OutTime desc ")
        {
            return ApprovalDb.GetMyApprovalOutTimePagingLeave(out totalCount, where, startIndex, pageLength, orderBy);
        }
    }
}
