using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;

namespace LiXinInterface.SystemManage
{
    public interface IFree_UserApplyOtherForm
    {
        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        void DeleteFree_UserApplyOtherForm(string IDlist);

        /// <summary>
        /// 获取其他有组织形式
        /// </summary>
        /// <returns></returns>
        List<Free_UserApplyOtherForm> GetFree_UserApplyOtherForm(string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyDateTime desc");

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        List<Free_UserApplyOtherForm> GetFree_UserApplyOtherForm(string where = "1=1");

        /// <summary>
        /// 新增一条数据
        /// </summary>  
        void InsertFree_UserApplyOtherForm(Free_UserApplyOtherForm model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        void UpdateFree_UserApplyOtherForm(Free_UserApplyOtherForm model);

        /// <summary>
        /// 获取其他人审批通过的项目
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<Free_UserApplyOtherForm> GetOtherPassForm(int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue);

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int UpdateBywhere(int id, string where = "1=1");


        List<Free_UserApplyOtherForm> GetReport_UserApplyOtherForm(string where);

        /// <summary>
        /// 课程名称是否重复
        /// </summary>
        /// <param name="FreeName"></param>
        /// <returns></returns>
        int GetExistFreeName(string FreeName, int ID = 0);
    }
}
