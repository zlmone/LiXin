using LiXinDataAccess.SystemManage;
using LiXinInterface.SystemManage;
using LiXinModels.MyApproval;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.SystemManage
{
    public class Free_UserOtherApplyBL : IFree_UserOtherApply
    {
        protected Free_UserOtherApplyDB fREE_USEROTHERAPPLYDB;
        public Free_UserOtherApplyBL(Free_UserOtherApplyDB _fREE_USEROTHERAPPLYDB)
        {
            fREE_USEROTHERAPPLYDB = _fREE_USEROTHERAPPLYDB;
        }

        #region 其他形式

        public bool AddFree_UserOtherApply(Free_UserOtherApply model)
        {
            return fREE_USEROTHERAPPLYDB.AddFree_UserOtherApply(model);
        }

        public bool UpdateFree_UserOtherApply(Free_UserOtherApply model)
        {
            return fREE_USEROTHERAPPLYDB.UpdateFree_UserOtherApply(model);
        }

        public bool DeleteFree_UserOtherApply(string id)
        {
            return fREE_USEROTHERAPPLYDB.DeleteFree_UserOtherApply(id);
        }

        public List<Free_UserOtherApply> GetFree_UserOtherApplyList(string where = " 1 = 1 ")
        {
            return fREE_USEROTHERAPPLYDB.GetFree_UserOtherApplyList(where);
        }


        public List<Free_UserOtherApply> GetFree_UserOtherApplyList_New(string where = " 1 = 1 ")
        {
            return fREE_USEROTHERAPPLYDB.GetFree_UserOtherApplyList_New(where);
        }

        public List<Free_UserOtherApply> GetFree_UserOtherApplyList_New_Batch(string where = " 1 = 1 ")
        {
            return fREE_USEROTHERAPPLYDB.GetFree_UserOtherApplyList_New_Batch(where);
        }

        public Free_UserOtherApply GetFree_UserOtherApplyById(int id)
        {
            return fREE_USEROTHERAPPLYDB.GetFree_UserOtherApplyById(id);
        }

        public bool UpdateFree_UserOtherApply_Status(int id, string where = " ,ApproveStatus=0,DepApproveStatus=0")
        {
            return fREE_USEROTHERAPPLYDB.UpdateFree_UserOtherApply_Status(id, where);
        }

        /// <summary>
        /// 获取已经最大的学时
        /// </summary>
        /// <returns></returns>
        public decimal GetMaxScore(int userID, int year)
        {
            return fREE_USEROTHERAPPLYDB.GetMaxScore(userID, year);
        }

        /// <summary>
        /// 自动折算
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetOutUserApply(string where = " 1 = 1 ")
        {
            return fREE_USEROTHERAPPLYDB.GetOutUserApply(where);
        }

        /// <summary>
        /// 授课讲师自动折算的学时
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetTeacherScore(string where = " 1=1")
        {
            return fREE_USEROTHERAPPLYDB.GetTeacherScore(where);
        }

        /// <summary>
        /// 获取当前人员年度内申请的所有学时
        /// </summary>
        /// <param name="ApplyUserID"></param>
        /// <param name="ApplyID"></param>
        /// <param name="ApplyType"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetMaxScore(string where = " 1=1")
        {
            return fREE_USEROTHERAPPLYDB.GetMaxScore(where);
        }

        /// <summary>
        /// 获取当前人员年度内申请的所有学时
        /// </summary>
        public List<Free_UserOtherApply> GetMaxScore(string userIDs, int ApplyID, int Year)
        {
            return fREE_USEROTHERAPPLYDB.GetMaxScore(userIDs, ApplyID, Year);
        }


        /// <summary>
        /// 获取当然授课人员以及申请的以及获得的
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public dynamic GetScoreByteacher(int userID, int year, string where = " 1=1")
        {
            return fREE_USEROTHERAPPLYDB.GetScoreByteacher(userID, year, where);
        }

        /// <summary>
        /// 授课人的自动折算
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetTeacherScoreList(string where = " 1=1")
        {
            return fREE_USEROTHERAPPLYDB.GetTeacherScoreList(where);
        }

        /// <summary>
        /// 获取人员获取的
        /// </summary>
        /// <param name="applyUserIDs"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetScoreByUser(string applyUserIDs, int year)
        {
            return fREE_USEROTHERAPPLYDB.GetScoreByUser(applyUserIDs, year);
        }

        /// <summary>
        /// 评估,授课人的自动评估查看
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ShowFreeDetails GetSurveyOut(int ID)
        {
            return fREE_USEROTHERAPPLYDB.GetSurveyOut(ID);
        }
        #endregion

        #region 免修
        /// <summary>
        /// 获取我申请的免修
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetFreeApply(string where = " 1=1")
        {
            return fREE_USEROTHERAPPLYDB.GetFreeApply(where);
        }
        #endregion

        #region 批量免修

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ApplyUserID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public void DeleteUserApply(string ApplyUserID, int ID)
        {
            fREE_USEROTHERAPPLYDB.DeleteUserApply(ApplyUserID, ID);
        }

        /// <summary>
        /// 查询批量免修
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<ShowFreeDetails> GetFreeDetailsList(out int totalCount, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ID desc")
        {
            var list = fREE_USEROTHERAPPLYDB.GetFreeDetailsList(where, startIndex, startLength, order);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }
        #endregion

        #region 公共

        public bool DeleteFree_UserOtherApplyByPeopleAndId(string ids, int id)
        {
            return fREE_USEROTHERAPPLYDB.DeleteFree_UserOtherApplyByPeopleAndId(ids, id);
        }

        public List<Free_UserOtherApply> FindFree_UserOtherApplyByFrom(int id)
        {
            return fREE_USEROTHERAPPLYDB.FindFree_UserOtherApplyByFrom(id);
        }

        public bool DeleteFree_UserApplyFilesById(string ids)
        {
            return fREE_USEROTHERAPPLYDB.DeleteFree_UserApplyFilesById(ids);
        }

        #endregion



        #region 用于个人中心的查询

        /// <summary>
        /// 获取我的当年度免修的学时
        /// </summary>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetAllMyScore(string year, int userID)
        {
            return fREE_USEROTHERAPPLYDB.GetAllMyScore(year, userID);
        }

        /// <summary>
        /// 其他形式
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<FreeUseScore> GetUserFreeOtherList(string where = "1=1", string outwhere = "1=1", string order = " typeForm,ApplyID,ApplyTime asc")
        {
            return fREE_USEROTHERAPPLYDB.GetUserFreeOtherList(where, outwhere, order);
        }

        /// <summary>
        /// 免修
        /// </summary>
        /// <returns></returns>
        public List<FreeUseScore> GetUserFreeList(string where = "1=1", string order = "ApplyID desc")
        {
            return fREE_USEROTHERAPPLYDB.GetUserFreeList(where, order);
        }

        /// <summary>
        /// 其他有组织形式
        /// </summary>
        /// <returns></returns>
        public List<FreeUseScore> GetUserFreeOrgList(string where = "1=1", string order = "ApplyID desc")
        {
            return fREE_USEROTHERAPPLYDB.GetUserFreeOrgList(where, order);
        }


     
        #endregion

    }
}
