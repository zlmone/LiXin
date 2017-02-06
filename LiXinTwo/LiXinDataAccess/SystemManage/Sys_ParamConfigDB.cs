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

        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertSys_ParamConfig(Sys_ParamConfig model)
        {

            using (var conn = OpenConnection())
            {
                const string strSql = @"INSERT INTO Sys_ParamConfig(ConfigName,ConfigType,ConfigValue,LastUpdateTime,userType)
	                     values( @ConfigName,@ConfigType,@ConfigValue,@LastUpdateTime,@userType);SELECT @@IDENTITY as ID ";

                var param = new
                {
                    model.ConfigName,
                    model.ConfigType,
                    model.ConfigValue,
                    model.LastUpdateTime,
                    model.userType
                };
                dynamic list = conn.Query<dynamic>(strSql, param).FirstOrDefault();
                model.ConfigId = decimal.ToInt32(model.ConfigId);
            }
        }

        public bool UpadteSys_ParamConfig(int ConfigType, string ConfigValue, int useType = 0)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = "update Sys_ParamConfig set LastUpdateTime=getdate(),ConfigValue=@ConfigValue,userType = @usertype where ConfigType=@ConfigType ";

                var param = new
               {
                   ConfigType,
                   ConfigValue,
                   usertype = useType
               };

                int result = conn.Execute(sql, param);
                return result > 0;

            }
        }



        public Sys_ParamConfig GetSys_ParamConfig(int configType)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = "select * from Sys_ParamConfig where ConfigType=" + configType;


                return conn.Query<Sys_ParamConfig>(sql).FirstOrDefault();
            }
        }

        public List<Sys_ParamConfig> GetList(string where = "1=1")
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = "select * from Sys_ParamConfig where " + where;
                return conn.Query<Sys_ParamConfig>(sql).ToList();
            }

        }

        /// <summary>
        /// 更新用途
        /// </summary>
        /// <param name="type1">ConfigType</param>
        /// <param name="type2">ConfigType</param>
        public void updateUseType(int type1, int type2)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = string.Format(@"UPDATE dbo.Sys_ParamConfig
SET userType = 1
where ConfigType={0} or ConfigType={1}", type1, type2);
                conn.Execute(sql);
            }

        }

        public bool UpadteSys_ParamConfigByYear(int ConfigType, string ConfigValue,int year, int useType = 0)
        {
            using (IDbConnection conn = OpenConnection())
            {
                string sql = @"update Sys_ParamConfig set ConfigValue=@ConfigValue,userType = @usertype where ConfigType=@ConfigType
                and datepart(year,LastUpdateTime)=@year";

                var param = new
                {
                    ConfigType,
                    ConfigValue,
                    usertype = useType,
                    year = year
                };

                int result = conn.Execute(sql, param);
                return result > 0;

            }
        }



    }
}
