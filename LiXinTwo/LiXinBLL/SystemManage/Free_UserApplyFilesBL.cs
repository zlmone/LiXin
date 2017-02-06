using LiXinDataAccess.SystemManage;
using LiXinInterface.SystemManage;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.SystemManage
{
    public class Free_UserApplyFilesBL : IFree_UserApplyFiles
    {
        protected Free_UserApplyFilesDB userapplyilesdb;

        public Free_UserApplyFilesBL(Free_UserApplyFilesDB _userapplyilesdb)
        {
            userapplyilesdb = _userapplyilesdb;
        }


        public bool AddFree_UserApplyFiles(Free_UserApplyFiles model)
        {
           return userapplyilesdb.AddFree_UserApplyFiles(model);
        }


        public bool UpdateFree_UserApplyFiles(Free_UserApplyFiles model)
        {
            return userapplyilesdb.UpdateFree_UserApplyFiles(model);
        }

        public bool DeleteFree_UserApplyFiles(string id)
        {
            return userapplyilesdb.DeleteFree_UserApplyFiles(id);
        }



        public List<Free_UserApplyFiles> GetFree_UserApplyFilesList(string where = " 1 = 1 ")
        {
            return userapplyilesdb.GetFree_UserApplyFilesList(where);
        }

    }
}
