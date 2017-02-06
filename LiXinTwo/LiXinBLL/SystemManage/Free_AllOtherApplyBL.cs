using LiXinDataAccess.SystemManage;
using LiXinInterface.SystemManage;
using LiXinModels;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.SystemManage
{
    public class Free_AllOtherApplyBL : IFree_AllOtherApply
    {
        private Free_AllOtherApplyDB db;

        public Free_AllOtherApplyBL(Free_AllOtherApplyDB _db)
        {
            db = _db;
        }


        public void AddFree_AllOtherApply(Free_AllOtherApply model)
        {
            db.AddFree_AllOtherApply(model);
        }


        public bool UpdateFree_AllOtherApply(Free_AllOtherApply model)
        {
            return db.UpdateFree_AllOtherApply(model);
        }


        public bool DeleteOtherFroms(string id)
        {
            return db.DeleteOtherFroms(id);
        }

        public Free_AllOtherApply GetOtherFromsById(int id)
        {
            return db.GetOtherFromsById(id);
        }

         /// <summary>
        /// 获得批量添加的人员数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<dynamic> GetUserIDByOtherFroms(int id)
        {
            return db.GetUserIDByOtherFroms(id);
        }

        /// <summary>
        /// 获取批量的所有人员数据
        /// </summary>
        public List<ShowFreeUserDetails> GetUserDetails(int id)
        {
            return db.GetUserDetails(id);
        }
    }

}
