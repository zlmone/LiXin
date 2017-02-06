using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Retech.Core;
using Retech.Data;
using LiXinModels;

namespace LiXinDataAccess.SystemManage
{
    public class Sys_ParamConfigDB : BaseRepository
    {
        public bool UpadteSys_ParamConfig(int ConfigType, string ConfigValue)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = "update Sys_ParamConfig set ConfigValue=@ConfigValue where ConfigType=@ConfigType ";

                var param = new
               {
                   ConfigType,
                   ConfigValue
               };

                int result = conn.Execute(sql, param);
                return result > 0;
            
            }
        }



        public Sys_ParamConfig GetSys_ParamConfig(int configType)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = "select * from Sys_ParamConfig where ConfigType="+configType;


                return conn.Query<Sys_ParamConfig>(sql).FirstOrDefault();
            }
        }

        public List<Sys_ParamConfig> GetList()
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = "select * from Sys_ParamConfig ";
                return conn.Query<Sys_ParamConfig>(sql).ToList();
            }

        }

    }
}
