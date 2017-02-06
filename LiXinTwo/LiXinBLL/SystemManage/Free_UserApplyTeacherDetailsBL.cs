using LiXinDataAccess.SystemManage;
using LiXinInterface.SystemManage;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.SystemManage
{
    public class Free_UserApplyTeacherDetailsBL : IFree_UserApplyTeacherDetails
    {
        protected Free_UserApplyTeacherDetailsDB _free_UserApplyTeacherDetailsDB;

        public Free_UserApplyTeacherDetailsBL()
        {
            _free_UserApplyTeacherDetailsDB = new Free_UserApplyTeacherDetailsDB();
        }


        #region 个人申请的添加
        public bool AddFree_UserApplyTeacherDetails(Free_UserApplyTeacherDetails model)
        {
            return _free_UserApplyTeacherDetailsDB.AddFree_UserApplyTeacherDetails(model);
        }

        public bool UpdateFree_UserApplyTeacherDetails(Free_UserApplyTeacherDetails model)
        {
            return _free_UserApplyTeacherDetailsDB.UpdateFree_UserApplyTeacherDetails(model);
        }

        public bool DeleteFree_UserApplyTeacherDetails(string id)
        {
            return _free_UserApplyTeacherDetailsDB.DeleteFree_UserApplyTeacherDetails(id);
        }


        public List<Free_UserApplyTeacherDetails> GetFree_UserApplyTeacherDetailsList(string where = " 1 = 1 ")
        {
            return _free_UserApplyTeacherDetailsDB.GetFree_UserApplyTeacherDetailsList(where);
        }
        #endregion

        #region 批量

        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertFree_BatchTeacherDetails(Free_BatchTeacherDetails model)
        {
            _free_UserApplyTeacherDetailsDB.InsertFree_BatchTeacherDetails(model);
        }

        /// <summary>
        /// 得到一个对象实体 
        /// </summary>
        public List<Free_BatchTeacherDetails> GetFree_BatchTeacherDetails(string where = "1=1")
        {
            return _free_UserApplyTeacherDetailsDB.GetFree_BatchTeacherDetails(where);
        }

        /// <summary>
        /// 删除一条或者多条数据
        /// </summary>
        public void DeleteFree_BatchTeacherDetails(string IDlist)
        {
            _free_UserApplyTeacherDetailsDB.DeleteFree_BatchTeacherDetails(IDlist);
        }
        #endregion
    }
}
