using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;

namespace LiXinInterface.SystemManage
{
   public interface ISys_Log
    {
       int Add(Sys_Log log);
       List<Sys_Log> GetAllList();
    }
}
