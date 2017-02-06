using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;

namespace LiXinInterface.DeptSystemManage
{
    public interface IDep_ParamConfig
    {
        /// <summary>
        /// 获得部门配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Sys_ParamConfig GetConfig(int id, int type);

        /// <summary>
        /// 判断配置是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsExist(int id, int type);

        /// <summary>
        /// 插入配置
        /// </summary>
        /// <param name="id">配置ID</param>
        /// <param name="type">分类</param>
        /// <param name="value">配置值</param>
        /// <param name="name">配置名称</param>
        /// <returns></returns>
        bool InsertConfig(int id, int type, string value,string name);

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool UpdateConfig(int id, int type, string value);
    }
}
