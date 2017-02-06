using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.SystemManage;
using LiXinModels.SystemManage;
using LiXinInterface.SystemManage;
namespace LiXinBLL.SystemManage
{
    public class Sys_LogTrainBL : ISys_LogTrain
    {
      private readonly static Sys_LogTrainDB logTrainDB = new Sys_LogTrainDB();


      public int Add(Sys_LogTrain log)
      {
          return logTrainDB.Insert(log);
      }

      public List<Sys_LogTrain> GetAllList()
      {
          return logTrainDB.GetAllList();
      }
    }
}
