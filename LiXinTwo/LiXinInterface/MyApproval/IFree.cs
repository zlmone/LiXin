using LiXinModels;
using LiXinModels.MyApproval;
using LiXinModels.SystemManage;
using LiXinModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.MyApproval
{
    public interface IFree
    {
        #region 授权审批人
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        void InsertFree_DepApprove(Free_DepApprove model);


        /// <summary>
        /// 更新一条数据
        /// </summary>
        void UpdateFree_DepApprove(Free_DepApprove model);


        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        void DeleteFree_DepApprove(string IDlist);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        List<Free_DepApprove> GetModel(string where = "1==1");


        /// <summary>
        /// 授权审批人
        /// </summary>
        /// <returns></returns>
        List<ShowDepApprove> GetDepApproveList(out int totalcount, string LeaderID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue);
        #endregion

        #region 其他形式审批
        /// <summary>
        /// 其他形式审批
        /// </summary>
        /// <returns></returns>
        List<ShowFreeDetails> GetOtherApproveList(out int totalCount, int userID, string ManageDeptIDs, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyTime desc");

        /// <summary>
        /// 选择申请年度
        /// </summary>
        /// <returns></returns>
        List<int> GetYearList(string where = " 1=1");


        /// <summary>
        /// 选择申请年度
        /// </summary>
        /// <returns></returns>
        List<int> GetOrgYearList(string where = " 1=1");
        #endregion

        #region 审批流程线
        /// <summary>
        /// 发起人信息
        /// </summary>
        /// <param name="ApplyID"></param>
        /// <returns></returns>
        ShowFreeDetails GetSingleByID(int ApproveID, int type = 1);

        /// <summary>
        /// 根据申请查询其审批流程
        /// </summary>
        /// <param name="ApproveID"></param>
        /// <returns></returns>
        List<Free_Apporve> GetApproveListByID(int ApproveID, int ApproveType = 1, int type = 1);

        /// <summary>
        /// 证明资料
        /// </summary>
        /// <param name="ApproveID"></param>
        /// <returns></returns>
        List<Free_UserApplyFiles> GetApplyFile(int ApproveID, int type = 0);

        /// <summary>
        /// 是否被拒绝过 ,有就查出最后一次拒绝的批次
        /// </summary>
        /// <returns></returns>
        Free_Apporve GetMaxDepApproveBatch(int ApproveID, string where = " 1=1");
        #endregion

        #region 新增审批信息
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        void InsertFree_Apporve(Free_Apporve model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        void UpdateFree_Apporve(Free_Apporve model);

        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        void DeleteFree_Apporve(string IDlist);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        List<Free_Apporve> GetFreeApporve(string where = "1=1");

        /// <summary>
        /// 更新个人申请的状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="where"></param>
        void UpdateUserApply(int ID, string where = "1=1");
        #endregion

        #region 免修审批
        /// <summary>
        /// 其他形式审批
        /// </summary>
        /// <returns></returns>
        List<ShowFreeDetails> GetFreeApproveList(out int totalCount, int userID, string ManageDeptIDs, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyTime desc");

        /// <summary>
        /// 获取免修发起人信息
        /// </summary>
        /// <param name="ApplyID"></param>
        /// <returns></returns>
        ShowFreeDetails GetFreeSingleByID(int ApproveID);
        #endregion

        #region  总所其他形式审批
        /// <summary>
        /// 总所其他形式审批
        /// </summary>
        /// <returns></returns>
        List<ShowFreeDetails> GetDepOtherApproveList(out int totalCount, int userID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue,
           string order = "ApplyTime desc");
        #endregion

        #region 总所免修审批
        /// <summary>
        /// 总所免修审批
        /// </summary>
        /// <returns></returns>
        List<ShowFreeDetails> GetDepFreeApproveList(out int totalCount, int userID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyTime desc");
        #endregion

        #region 其他有组织形式审批
        /// <summary>
        /// 其他有组织形式审批 分所  
        /// </summary>
        /// <returns></returns>
        List<ShowFreeDetails> GetFreeOrgList(out int totalCount, int userID, string ManageDeptIDs, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyDateTime desc");

        /// <summary>
        /// 获取免修发起人信息
        /// </summary>
        /// <param name="ApplyID"></param>
        /// <returns></returns>
        ShowFreeDetails GetOrgSingleByID(int ApproveID);

        /// <summary>
        ///  其他有组织形式审批 总所
        /// </summary>
        /// <returns></returns>
        List<ShowFreeDetails> GetDepFreeOrgList(out int totalCount, int userID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyDateTime desc");

        #endregion

        /// <summary>
        /// 获取当前人员所在部门的负责人或者当前人员的上级
        /// </summary>
        /// <param name="DeptID"></param>
        /// <param name="JobNum"></param>
        /// <returns></returns>
        List<Sys_User> GetManageUserByDeptID(int DeptID, string JobNum);

        /// <summary>
        /// 获取所有培训教育部的人，用来发送邮件
        /// </summary>
        /// <returns></returns>
        List<Sys_User> GetManageUser();

        /// <summary>
        /// 当前申请的免修是否在范围之内
        /// </summary>
        /// <param name="year"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        int IfExistFreeItem(int year, int ID, int ApplyUserI = -1);

        /// <summary>
        /// 获取各部门的授权审批信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        List<Free_DepApprove> GetDeptApproveUserList(string where = "1=1", int startIndex = 1, int startLength = int.MaxValue);

        /// <summary>
        /// 当前申请的免修
        /// </summary>
        /// <param name="year"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        List<Free_UserOtherApply> GetFreeItemByUser(int year, int ApplyUserID = -1);

    }
}
