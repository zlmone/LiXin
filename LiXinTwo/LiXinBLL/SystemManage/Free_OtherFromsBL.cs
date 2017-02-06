using LiXinDataAccess.SystemManage;
using LiXinInterface.SystemManage;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.SystemManage
{
    public class Free_OtherFromsBL : IFree_OtherFroms
    {
        private Free_OtherFromsDB _free_OtherFromsDB;

        public Free_OtherFromsBL()
        {
            _free_OtherFromsDB = new Free_OtherFromsDB();
        }

        public void AddOtherFroms(Free_OtherFroms model)
        {
            _free_OtherFromsDB.AddOtherFroms(model);
        }

        public bool UpdateOtherFroms(Free_OtherFroms model)
        {
           return  _free_OtherFromsDB.UpdateOtherFroms(model);
        }

        public bool DeleteOtherFroms(string id)
        {
            return _free_OtherFromsDB.DeleteOtherFroms(id);
        }

        public List<Free_OtherFroms> GetOtherFromsList(string where = " 1 = 1 ")
        {
            return _free_OtherFromsDB.GetOtherFromsList(where);
        }

        public Free_OtherFroms GetOtherFromsById(int id)
        {
            return _free_OtherFromsDB.GetOtherFromsById(id);
        }
    }
}
