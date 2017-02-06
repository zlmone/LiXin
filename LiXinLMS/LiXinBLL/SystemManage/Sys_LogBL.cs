using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.SystemManage;
using LiXinModels.SystemManage;
using LiXinInterface.SystemManage;
namespace LiXinBLL.SystemManage
{
  public  class Sys_LogBL:ISys_Log
    {
      private readonly static Sys_LogDB logDB = new Sys_LogDB();


      public int Add(Sys_Log log)
      {
        return logDB.Insert(log);
      }

      public List<Sys_Log> GetAllList()
      {
          return logDB.GetAllList();
      }
    }
}
