using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.MyApproval;
using LiXinInterface.MyApproval;
using LiXinModels;
using LiXinModels.SystemManage;
using LiXinModels.MyApproval;
using LiXinModels.User;

namespace LiXinBLL.MyApproval
{
    public class FreeBL : IFree
    {
        public FreeDB FreeDB;
        public FreeBL()
        {
            FreeDB = new FreeDB();
        }

        #region 授权审批人
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertFree_DepApprove(Free_DepApprove model)
        {
            FreeDB.InsertFree_DepApprove(model);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateFree_DepApprove(Free_DepApprove model)
        {
            FreeDB.UpdateFree_DepApprove(model);
        }

        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        public void DeleteFree_DepApprove(string IDlist)
        {
            FreeDB.DeleteFree_DepApprove(IDlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<Free_DepApprove> GetModel(string where = "1=1")
        {
            return FreeDB.GetModel(where);
        }


        /// <summary>
        /// 授权审批人
        /// </summary>
        /// <returns></returns>
        public List<ShowDepApprove> GetDepApproveList(out int totalcount, string LeaderID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue)
        {
            var list = FreeDB.GetDepApproveList(LeaderID, where, startIndex, startLength);
            totalcount = list.Count() == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }
        #endregion

        #region 其他形式审批
        /// <summary>
        /// 其他形式审批
        /// </summary>
        /// <returns></returns>
        public List<ShowFreeDetails> GetOtherApproveList(out int totalCount, int userID, string ManageDeptIDs, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyTime desc")
        {
            var list = FreeDB.GetOtherApproveList(userID, ManageDeptIDs, where, startIndex, startLength, order);
            totalCount = list.Count() == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        /// <summary>
        /// 选择申请年度
        /// </summary>
        /// <returns></returns>
        public List<int> GetYearList(string where = " 1=1")
        {
            return FreeDB.GetYearList(where);
        }

        /// <summary>
        /// 选择申请年度
        /// </summary>
        /// <returns></returns>
        public List<int> GetOrgYearList(string where = " 1=1")
        {
            return FreeDB.GetOrgYearList(where);
        }
        #endregion

        #region 审批流程线
        /// <summary>
        /// 发起人信息
        /// </summary>
        /// <param name="ApplyID"></param>
        /// <returns></returns>
        public ShowFreeDetails GetSingleByID(int ApproveID, int type = 1)
        {
            var model = FreeDB.GetSingleByID(ApproveID, type);
            return model == null ? new ShowFreeDetails() : model;
        }

        /// <summary>
        /// 根据申请查询其审批流程
        /// </summary>
        /// <param name="ApproveID"></param>
        /// <returns></returns>
        public List<Free_Apporve> GetApproveListByID(int ApproveID, int ApproveType = 1, int type = 1)
        {
            return FreeDB.GetApproveListByID(ApproveID, ApproveType, type);
        }

        /// <summary>
        /// 证明资料
        /// </summary>
        /// <param name="ApproveID"></param>
        /// <returns></returns>
        public List<Free_UserApplyFiles> GetApplyFile(int ApproveID, int type = 0)
        {
            return FreeDB.GetApplyFile(ApproveID, type);
        }

        /// <summary>
        /// 是否被拒绝过 ,有就查出最后一次拒绝的批次
        /// </summary>
        /// <returns></returns>
        public Free_Apporve GetMaxDepApproveBatch(int ApproveID, string where = " 1=1")
        {
            return FreeDB.GetMaxDepApproveBatch(ApproveID, where);
        }
        #endregion

        #region 新增审批信息
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertFree_Apporve(Free_Apporve model)
        {
            FreeDB.InsertFree_Apporve(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateFree_Apporve(Free_Apporve model)
        {
            FreeDB.UpdateFree_Apporve(model);
        }

        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        public void DeleteFree_Apporve(string IDlist)
        {
            FreeDB.DeleteFree_Apporve(IDlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<Free_Apporve> GetFreeApporve(string where = "1=1")
        {
            return FreeDB.GetFreeApporve(where);
        }

        /// <summary>
        /// 更新个人申请的状态
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="where"></param>
        public void UpdateUserApply(int ID, string where = "1=1")
        {
            FreeDB.UpdateUserApply(ID, where);
        }
        #endregion

        #region 免修审批
        /// <summary>
        /// 其他形式审批
        /// </summary>
        /// <returns></returns>
        public List<ShowFreeDetails> GetFreeApproveList(out int totalCount, int userID, string ManageDeptIDs, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyTime desc")
        {
            var list = FreeDB.GetFreeApproveList(userID, ManageDeptIDs, where, startIndex, startLength, order);
            totalCount = list.Count() == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        /// <summary>
        /// 获取免修发起人信息
        /// </summary>
        /// <param name="ApplyID"></param>
        /// <returns></returns>
        public ShowFreeDetails GetFreeSingleByID(int ApproveID)
        {
            return FreeDB.GetFreeSingleByID(ApproveID);
        }
        #endregion

        #region  总所其他形式审批
        /// <summary>
        /// 总所其他形式审批
        /// </summary>
        /// <returns></returns>
        public List<ShowFreeDetails> GetDepOtherApproveList(out int totalCount, int userID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue,
           string order = "ApplyTime desc")
        {
            var list = FreeDB.GetDepOtherApproveList(userID, where, startIndex, startLength, order);
            totalCount = list.Count() == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }
        #endregion

        #region 总所免修审批
        /// <summary>
        /// 总所免修审批
        /// </summary>
        /// <returns></returns>
        public List<ShowFreeDetails> GetDepFreeApproveList(out int totalCount, int userID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyTime desc")
        {
            var list = FreeDB.GetDepFreeApproveList(userID, where, startIndex, startLength, order);
            totalCount = list.Count() == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }


        #endregion

        #region  其他有组织形式审批
        /// <summary>
        /// 其他有组织形式审批 分所 
        /// </summary>
        /// <returns></returns>
        public List<ShowFreeDetails> GetFreeOrgList(out int totalCount, int userID, string ManageDeptIDs, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyDateTime desc")
        {
            var list = FreeDB.GetFreeOrgList(userID, ManageDeptIDs, where, startIndex, startLength, order);
            totalCount = list.Count() == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        /// <summary>
        /// 获取免修发起人信息
        /// </summary>
        /// <param name="ApplyID"></param>
        /// <returns></returns>
        public ShowFreeDetails GetOrgSingleByID(int ApproveID)
        {
            return FreeDB.GetOrgSingleByID(ApproveID);
        }

        /// <summary>
        ///  其他有组织形式审批 总所
        /// </summary>
        /// <returns></returns>
        public List<ShowFreeDetails> GetDepFreeOrgList(out int totalCount, int userID, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyDateTime desc")
        {
            var list = FreeDB.GetDepFreeOrgList(userID, where, startIndex, startLength, order);
            totalCount = list.Count() == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }
        #endregion


        /// <summary>
        /// 获取当前人员所在部门的负责人或者当前人员的上级
        /// </summary>
        /// <param name="DeptID"></param>
        /// <param name="JobNum"></param>
        /// <returns></returns>
        public List<Sys_User> GetManageUserByDeptID(int DeptID, string JobNum)
        {
            return FreeDB.GetManageUserByDeptID(DeptID, JobNum);
        }

        /// <summary>
        /// 获取所有培训教育部的人，用来发送邮件
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetManageUser()
        {
            return FreeDB.GetManageUser();
        }

        /// <summary>
        /// 当前申请的免修是否在范围之内
        /// </summary>
        /// <param name="year"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int IfExistFreeItem(int year, int ID, int ApplyUserID = -1)
        {
            return FreeDB.IfExistFreeItem(year, ID, ApplyUserID);
        }

        /// <summary>
        /// 获取各部门的授权审批信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<Free_DepApprove> GetDeptApproveUserList(string where = "1=1", int startIndex = 1, int startLength = int.MaxValue)
        {
            return FreeDB.GetDeptApproveUserList(where, startIndex, startLength);
        }


        /// <summary>
        /// 当前申请的免修
        /// </summary>
        /// <param name="year"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetFreeItemByUser(int year, int ApplyUserID = -1)
        {
            var list = FreeDB.GetFreeItemByUser(year, ApplyUserID);
            foreach (var item in list)
            {
                var single = list.Where(p => p.ApplyUserID == item.ApplyUserID);
                item.tCount = single.Count(p => p.tScore > 0);
                item.CPACount = single.Count(p => p.CPAScore > 0);
            }
            return list;
        }
    }
}
