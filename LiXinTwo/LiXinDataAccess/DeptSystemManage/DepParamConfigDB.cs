using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LiXinModels;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.DeptSystemManage
{
    public class DepParamConfigDB : BaseRepository
    {
        /// <summary>
        /// 获得部门配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Sys_ParamConfig GetConfig(int id, int type)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"select * from Dep_ParamConfig where DeptId={0} and ConfigType={1}", id, type);
                return conn.Query<Sys_ParamConfig>(sql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 判断配置是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsExist(int id, int type)
        {
            using (var conn = OpenConnection())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@" select count(*) from Dep_ParamConfig where DeptId=@id and ConfigType=@type ");
                var param = new
                    {
                        id=id,
                        type=type
                    };
                int count = conn.Query<int>(strSql.ToString(),param).FirstOrDefault();
                return count > 0;
            }
        }

        /// <summary>
        /// 插入配置
        /// </summary>
        /// <param name="id">配置ID</param>
        /// <param name="type">分类</param>
        /// <param name="value">配置值</param>
        /// <param name="name">配置名称</param>
        /// <returns></returns>
        public bool InsertConfig(int id, int type, string value,string name)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"insert into Dep_ParamConfig (ConfigName,ConfigType,ConfigValue,LastUpdateTime,DeptId) values('{3}',{1},'{2}','{4}',{0})", id, type, value,name,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return conn.Execute(sql) > 0;
            }
        }

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool UpdateConfig(int id, int type, string value)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"update Dep_ParamConfig set ConfigValue='{2}',LastUpdateTime='{3}' where DeptId={0} and ConfigType={1}", id, type, value,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return conn.Execute(sql) > 0;
            }
        }
    }
}
