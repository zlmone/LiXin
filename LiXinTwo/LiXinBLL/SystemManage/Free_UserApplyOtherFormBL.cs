using LiXinDataAccess.SystemManage;
using LiXinInterface.SystemManage;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.SystemManage
{
    public class Free_UserApplyOtherFormBL : IFree_UserApplyOtherForm
    {
        private readonly Free_UserApplyOtherFormDB dal;
        public Free_UserApplyOtherFormBL()
        {
            dal = new Free_UserApplyOtherFormDB();
        }

        #region 基础方法
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertFree_UserApplyOtherForm(Free_UserApplyOtherForm model)
        {
            dal.InsertFree_UserApplyOtherForm(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateFree_UserApplyOtherForm(Free_UserApplyOtherForm model)
        {
            dal.UpdateFree_UserApplyOtherForm(model);
        }


        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        public void DeleteFree_UserApplyOtherForm(string IDlist)
        {
            dal.DeleteFree_UserApplyOtherForm(IDlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<Free_UserApplyOtherForm> GetFree_UserApplyOtherForm(string where = "1=1")
        {
            return dal.GetFree_UserApplyOtherForm(where);
        }
        #endregion

        /// <summary>
        /// 获取其他有组织形式
        /// </summary>
        /// <returns></returns>
        public List<Free_UserApplyOtherForm> GetFree_UserApplyOtherForm( string where = "1=1", int startIndex = 1, int startLength = int.MaxValue, string order = "ApplyDateTime desc")
        {
            return dal.GetFree_UserApplyOtherForm(where, startIndex, startLength);
        }

        /// <summary>
        /// 获取其他人审批通过的项目
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Free_UserApplyOtherForm> GetOtherPassForm(int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue)
        {
            return dal.GetOtherPassForm(userID, where, startIndex, startLength);
        }


        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int UpdateBywhere(int id, string where = "1=1")
        {
           return dal.UpdateBywhere(id, where);
        }

        public List<Free_UserApplyOtherForm> GetReport_UserApplyOtherForm(string where)
        {
            return dal.GetReport_UserApplyOtherForm(where);
        }

        /// <summary>
        /// 课程名称是否重复
        /// </summary>
        /// <param name="FreeName"></param>
        /// <returns></returns>
        public int GetExistFreeName(string FreeName, int ID = 0)
        {
            return dal.GetExistFreeName(FreeName, ID);
        }
    }
}

