using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;
using MongoDB.Driver.Builders;
namespace LiXinDataAccess.SystemManage
{
   public class Sys_LogDB:BaseMethod
    {
       public List<Sys_Log> GetAllList()
       {
           return GetAllList<Sys_Log>(Query.And(new[]
                {
                    Query.EQ("IsDelete", 0)
                }));
       }
    }
}
