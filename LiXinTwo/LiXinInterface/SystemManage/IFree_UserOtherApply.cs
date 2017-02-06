using LiXinModels.MyApproval;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.SystemManage
{
    public interface IFree_UserOtherApply
    {
        #region 其他形式

        bool AddFree_UserOtherApply(Free_UserOtherApply model);


        bool UpdateFree_UserOtherApply(Free_UserOtherApply model);


        bool DeleteFree_UserOtherApply(string id);

        List<Free_UserOtherApply> GetFree_UserOtherApplyList(string where = " 1 = 1 ");

        List<Free_UserOtherApply> GetFree_UserOtherApplyList_New(string where = " 1 = 1 ");

        List<Free_UserOtherApply> GetFree_UserOtherApplyList_New_Batch(string where = " 1 = 1 ");

        Free_UserOtherApply GetFree_UserOtherApplyById(int id);

        bool UpdateFree_UserOtherApply_Status(int id, string where = " ,ApproveStatus=0,DepApproveStatus=0");

        /// 获取已经最大的学时
        /// </summary>
        /// <returns></returns>
        decimal GetMaxScore(int userID, int year);


        /// <summary>
        /// 获取当前人员年度内申请的所有学时
        /// </summary>
        List<Free_UserOtherApply> GetMaxScore(string userIDs, int ApplyID, int Year);

        /// <summary>
        /// 自动折算
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Free_UserOtherApply> GetOutUserApply(string where = " 1 = 1 ");

        /// <summary>
        /// 授课讲师自动折算的学时
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Free_UserOtherApply> GetTeacherScore(string where = " 1=1");

        /// <summary>
        /// 获取当前人员年度内申请的所有学时
        /// </summary>
        /// <param name="ApplyUserID"></param>
        /// <param name="ApplyID"></param>
        /// <param name="ApplyType"></param>
        /// <returns></returns>
        List<Free_UserOtherApply> GetMaxScore(string where = " 1=1");


        /// <summary>
        /// 获取当然授课人员以及申请的以及获得的
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        dynamic GetScoreByteacher(int userID, int year, string where = " 1=1");


        /// <summary>
        /// 授课人的自动折算
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Free_UserOtherApply> GetTeacherScoreList(string where = " 1=1");

        /// <summary>
        /// 获取人员获取的
        /// </summary>
        /// <param name="applyUserIDs"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        List<Free_UserOtherApply> GetScoreByUser(string applyUserIDs, int year);


        /// <summary>
        /// 评估,授课人的自动评估查看
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        ShowFreeDetails GetSurveyOut(int ID);
        #endregion

        #region 免修
        /// <summary>
        /// 获取我申请的免修
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Free_UserOtherApply> GetFreeApply(string where = " 1=1");

        /// <summary>
        /// 获取我的当年度免修的学时
        /// </summary>
        /// <returns></returns>
        List<Free_UserOtherApply> GetAllMyScore(string year, int userID);
        #endregion

        #region 批量免修

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ApplyUserID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        void DeleteUserApply(string ApplyUserID, int ID);

        /// <summary>
        /// 查询批量免修
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        List<ShowFreeDetails> GetFreeDetailsList(out int totalCount, string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ID desc");
        #endregion

        #region 公共

        bool DeleteFree_UserOtherApplyByPeopleAndId(string ids, int id);

        List<Free_UserOtherApply> FindFree_UserOtherApplyByFrom(int id);

        bool DeleteFree_UserApplyFilesById(string ids);

        #endregion



        #region 用于个人中心的查询
        /// <summary>
        /// 其他形式
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        List<FreeUseScore> GetUserFreeOtherList(string where = "1=1", string outwhere = "1=1", string order = " typeForm,ApplyID,ApplyTime asc");

        /// <summary>
        /// 免修
        /// </summary>
        /// <returns></returns>
        List<FreeUseScore> GetUserFreeList(string where = "1=1", string order = "ApplyID desc");

        /// <summary>
        /// 其他有组织形式
        /// </summary>
        /// <returns></returns>
        List<FreeUseScore> GetUserFreeOrgList(string where = "1=1", string order = "ApplyID desc");

        #endregion

    }
}
