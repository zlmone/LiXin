using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinModels
{
    /// <summary>
    /// 参数配置表
    /// </summary>
    public class Sys_ParamConfig
    {
        private int _configId;
        /// <summary>
        /// 配置ID
        /// </summary>
        public int ConfigId
        {
            get { return _configId; }
            set { _configId = value; }
        }
        private string _configName;
        /// <summary>
        /// 配置名称
        /// </summary>
        public string ConfigName
        {
            get { return _configName; }
            set { _configName = value; }
        }
        private int _configType;
        /// <summary>
        /// 配置类型
        /// </summary>
        public int ConfigType
        {
            get { return _configType; }
            set { _configType = value; }
        }
        private string _configValue;

        /// <summary>
        /// 配置值
        /// </summary>
        public string ConfigValue
        {
            get { return _configValue; }
            set { _configValue = value; }
        }

        /// <summary>
        /// LastUpdateTime
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 0 不累加计算  1累加计算
        /// </summary>
        public int userType { get; set; }
    }

    [Serializable]
    public class TempConfig
    {
        /// <summary>
        /// 类型（0：迟到；1：早退）
        /// </summary>
        public int Type { set; get; }
        /// <summary>
        /// 起始分钟
        /// </summary>
        public int StartMinite { set; get; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public int EndMinite { set; get; }
        /// <summary>
        /// 扣除学时百分比
        /// </summary>
        public int Percent { set; get; }
    }
}
