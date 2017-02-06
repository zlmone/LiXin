using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;
using MongoDB.Driver.Builders;
namespace LiXinDataAccess.SystemManage
{
   public class Sys_LogTrainDB:BaseMethod
    {
       public List<Sys_LogTrain> GetAllList()
       {
           return GetAllList<Sys_LogTrain>(Query.And(new[]
                {
                    Query.EQ("IsDelete", 0)
                }));
       }
    }
}
