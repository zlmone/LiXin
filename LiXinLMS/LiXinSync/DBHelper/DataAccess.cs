using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LiXinSync
{
    internal abstract class DataAccess
    {
        /// <summary>
        /// 通过sql语句获取DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        internal abstract DataSet GetDataSet(string sql);
        /// <summary>
        /// 通过sql语句获取DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        internal DataTable GetDataTable(string sql)
        {
            return GetDataSet(sql).Tables[0];
        }
        /// <summary>
        /// 执行SQL语句，返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        internal abstract int ExecuteSql(string sql);

        /// <summary>
        /// 执行SQL查询，并将查询返回的结果集中第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        internal abstract object ExecuteScalar(string sql);

        // 通过DataTable获取对象List 
        protected List<T> GetModelListByDataTable<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            Type t = typeof(T);
            System.Reflection.PropertyInfo[] ps = t.GetProperties();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                T obj = Activator.CreateInstance<T>();
                foreach (System.Reflection.PropertyInfo p in ps)
                {
                    //判断Table中是否存在Dept属性的列
                    //并且Table中的值不为空
                    if (dt.Rows[i][p.Name] == null || dt.Rows[i][p.Name] == DBNull.Value)
                        continue;

                    if (dt.Columns.Contains(p.Name))
                    {
                        p.SetValue(obj, dt.Rows[i][p.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        //通过SQL语句获取对象列表
        protected List<T> GetModelListBySql<T>(string sql)
        {
            return GetModelListByDataTable<T>(GetDataSet(sql).Tables[0]);
        }        
    }
}
