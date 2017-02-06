using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.DeptSystemManage;
using LiXinInterface.DeptSystemManage;
using LiXinModels;

namespace LiXinBLL.DeptSystemManage
{
    public class ParamConfigBl : IDep_ParamConfig
    {
        private DepParamConfigDB db = new DepParamConfigDB();

        /// <summary>
        /// 获得部门配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Sys_ParamConfig GetConfig(int id, int type)
        {

            return db.GetConfig(id, type);
        }

        /// <summary>
        /// 判断配置是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsExist(int id, int type)
        {
            return db.IsExist(id, type);
            ;
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
            return db.InsertConfig(id, type, value,name);
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
            return db.UpdateConfig(id, type, value);
        }
    }
}
