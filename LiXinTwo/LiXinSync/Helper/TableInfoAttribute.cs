using System;

namespace LiXinSync
{
    /// <summary>
    /// 指示一个实体类对应的数据库表信息
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class)]
    public class TableInfoAttribute : Attribute
    {
        /// <summary>
        /// 对应的表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 主键字段名
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 获取类的TableInfoAttribute特性
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static TableInfoAttribute GetAttribute(Type t)
        {
            TableInfoAttribute tableattr = (TableInfoAttribute)Attribute.GetCustomAttribute(t, typeof(TableInfoAttribute));
            return tableattr;
        }
    }
}
