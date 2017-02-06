using System;
using System.Reflection;

namespace LiXinSync
{
    /// <summary>
    /// 字段映射信息
    /// <para>在实体属性上定义映射关系</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DataFieldAttribute : Attribute
    {
        /// <summary>
        /// 字段名称
        /// <para>映射到数据库字段的名称</para>
        /// </summary>
        public string FieldName { get; set; }

        private bool _identity = false;

        /// <summary>
        /// 是否自增字段
        /// <para>默认为false</para>
        /// </summary>
        public bool Identity
        {
            get { return _identity; }
            set { _identity = value; }
        }

        /// <summary>
        /// 获取属性的DataField特性
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static DataFieldAttribute GetAttribute(MemberInfo memberInfo)
        {
            DataFieldAttribute fieldattr = (DataFieldAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(DataFieldAttribute));
            return fieldattr;
        }


    }
}
