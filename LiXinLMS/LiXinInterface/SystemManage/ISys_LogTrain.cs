using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;

namespace LiXinInterface.SystemManage
{
   public interface ISys_LogTrain
    {
       int Add(Sys_LogTrain log);
       List<Sys_LogTrain> GetAllList();
    }
}
