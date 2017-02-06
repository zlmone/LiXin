using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.SystemManage
{
   public  interface IFree_UserApplyFiles
    {

       bool AddFree_UserApplyFiles(Free_UserApplyFiles model);

       bool UpdateFree_UserApplyFiles(Free_UserApplyFiles model);

       bool DeleteFree_UserApplyFiles(string id);

       List<Free_UserApplyFiles> GetFree_UserApplyFilesList(string where = " 1 = 1 ");
    }
}
