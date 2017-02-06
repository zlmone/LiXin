using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LiXinSync
{
    public class BaseModel
    {
        /// <summary>
        /// 是否新Model
        /// <para>true 在数据库中没有这条记录，保存时将会用INSERT语句</para>
        /// <para>false 本条记录是在数据库中获取的，保存时将会用UPDATE语句</para>        
        /// </summary>
        public bool IsNew
        {
            get;
            internal set;
        }

        private Dictionary<string, object> _oldValues;
        //获取对象的新增SQL语句(只限于SqlServer数据库)
        public virtual string GenerateInsertSql()
        {
            Type thisType = this.GetType();
            PropertyInfo[] ps = thisType.GetProperties();
            StringBuilder insertSql = new StringBuilder();
            string tableName;
            TableInfoAttribute tabattr = TableInfoAttribute.GetAttribute(thisType);
            if (tabattr == null)
            {
                tableName = thisType.Name;
            }
            else
            {
                tableName = tabattr.TableName;
            }
            insertSql.AppendFormat("INSERT INTO [{0}] (", tableName);
            StringBuilder values = new StringBuilder();
            for (int i = 0, j = 0; i < ps.Length; i++)
            {
                DataFieldAttribute fieldattr = DataFieldAttribute.GetAttribute(ps[i]);
                //如果是自增长字段，跳过
                if (fieldattr != null && fieldattr.Identity)
                    continue;
                if (ps[i].Name == "IsNew")
                    continue;
                if (j != 0)
                {
                    insertSql.Append(",");
                    values.Append(",");
                }
                insertSql.AppendFormat("[{0}]", ps[i].Name);
                object value = ps[i].GetValue(this, null);
                if (value == null)
                    values.Append("NULL");
                else
                    values.AppendFormat("'{0}'", value.ToString().Replace("'", "''"));
                j++;
            }
            insertSql.Append(") VALUES(");
            insertSql.Append(values);
            insertSql.Append(")");
            return insertSql.ToString();
        }
        /// <summary>
        /// 生成对象的Update语句
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateUpdateSql()
        {
            Type thisType = this.GetType();
            PropertyInfo[] ps = thisType.GetProperties();
            TableInfoAttribute tableAttr = TableInfoAttribute.GetAttribute(thisType);
            if (tableAttr == null)
                throw new Exception("对象未设置数据库对应关系");
            string primaryKey = tableAttr.PrimaryKey;
            string tableName = tableAttr.TableName;
            StringBuilder updateSql = new StringBuilder();
            updateSql.AppendFormat("UPDATE {0} SET ", tableAttr.TableName);
            StringBuilder where = new StringBuilder("\n WHERE ");
            Dictionary<string, object> modifyValues = GetModifyValues();
            //如果未做修改，返回空字符串
            if (modifyValues.Count == 0)
                return string.Empty;
            int fieldCount = 0;
            foreach (string key in modifyValues.Keys)
            {
                PropertyInfo p = ps.First(a => a.Name == key);
                DataFieldAttribute fieldAttr = DataFieldAttribute.GetAttribute(p);
                //如果定义为自增长字段，则跳过
                if (fieldAttr != null && fieldAttr.Identity)
                    continue;
                //如果是主键，则跳过
                if (p.Name == primaryKey)
                    continue;
                object value = p.GetValue(this, null);

                if (value == null)
                {
                    updateSql.AppendFormat("[{0}]=NULL,", p.Name);
                }
                else
                {
                    string valueStr = value.ToString().Replace("'", "''");
                    updateSql.AppendFormat("[{0}]='{1}',", p.Name, valueStr);
                }
                fieldCount++;
            }

            if (fieldCount == 0)
                return string.Empty;

            //去掉最后一个逗号
            updateSql = updateSql.Remove(updateSql.Length - 1, 1);
            //添加WHERE条件
            PropertyInfo pk = ps.First(p => primaryKey == p.Name);
            where.AppendFormat("[{0}]='{1}'", primaryKey, _oldValues[primaryKey]);
            updateSql.Append(where);
            return updateSql.ToString();
        }

        private Dictionary<string, object> GetModifyValues()
        {
            Dictionary<string, object> modifyValues = new Dictionary<string, object>();
            Type t = this.GetType();
            PropertyInfo[] ps = t.GetProperties();
            for (int i = 0; i < ps.Length; i++)
            {
                //如果原始值与现有值不一致，则判定为修改
                if (ps[i].Name == "IsNew")
                    continue;
                if (!_oldValues.ContainsKey(ps[i].Name))
                {
                    continue;
                }
                object oldValue = _oldValues[ps[i].Name];
                object newValue = ps[i].GetValue(this, null);
                if (oldValue == DBNull.Value && newValue == null)
                {
                    continue;
                }
                if (!object.Equals(_oldValues[ps[i].Name], ps[i].GetValue(this, null)))
                {
                    modifyValues.Add(ps[i].Name, ps[i].GetValue(this, null));
                }
            }
            return modifyValues;
        }

        public BaseModel()
        {
            IsNew = true;
            _oldValues = new Dictionary<string, object>();
        }

        public int Save(string databasePath)
        {
            StringBuilder sql = new StringBuilder();
            Type t = this.GetType();
            TableInfoAttribute tableAttr = TableInfoAttribute.GetAttribute(t);
            PropertyInfo pk = t.GetProperty(tableAttr.PrimaryKey);
            DataFieldAttribute fieldAttr = DataFieldAttribute.GetAttribute(pk);
            DataAccess sda = new SqlDataAccess(databasePath);
            if (IsNew)
            {
                //调用新增方法保存
                //返回新增记录的主键
                sql.Append(GenerateInsertSql());
                sql.AppendLine();
                if (fieldAttr != null && fieldAttr.Identity)
                {
                    //主键是自增长类型的                    
                    sql.Append("SELECT @@Identity AS " + tableAttr.PrimaryKey);
                }
                else
                {
                    sql.AppendFormat("SELECT  {0} AS {1}", pk.GetValue(this, null), tableAttr.PrimaryKey);
                }
                DataSet ds = sda.GetDataSet(sql.ToString());
                this.IsNew = false;
                int primaryKey = int.Parse(ds.Tables[0].Rows[0][tableAttr.PrimaryKey].ToString());
                pk.SetValue(this, primaryKey, null);

                //新增成功后，需要把现有的属性放入_oldValues，以防止下次保存时，数据不能提交
                _oldValues = new Dictionary<string, object>();
                foreach (var item in t.GetProperties())
                {
                    if (item.Name == "IsNew")
                        continue;
                    _oldValues.Add(item.Name, item.GetValue(this, null));
                }
                return primaryKey;

            }
            else
            {
                object primaryKey = pk.GetValue(this, null);
                string updatesql = GenerateUpdateSql();
                if (!string.IsNullOrWhiteSpace(updatesql))
                {
                    sda.ExecuteSql(updatesql);
                    _oldValues = new Dictionary<string, object>();
                    foreach (var item in t.GetProperties())
                    {
                        if (item.Name == "IsNew")
                            continue;
                        _oldValues.Add(item.Name, item.GetValue(this, null));
                    }
                }

                return (int)primaryKey;
            }
        }

        // 通过DataTable获取对象List 
        internal static List<T> GetModelsByDataTable<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            Type t = typeof(T);
            System.Reflection.PropertyInfo[] ps = t.GetProperties();
            foreach (DataRow row in dt.Rows)
            {
                T obj = Activator.CreateInstance<T>();
                Dictionary<string, object> oldValues = new Dictionary<string, object>();
                for (int i = 0; i < ps.Length; i++)
                {
                    if (ps[i].Name == "IsNew")
                    {
                        ps[i].SetValue(obj, false, null);
                        continue;
                    }
                    if (!dt.Columns.Contains(ps[i].Name))
                        continue;
                    object value = row[ps[i].Name];
                    if (value != null && value != DBNull.Value)
                    {
                        Type colType = ps[i].PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {   //判断是否为 System.Nullable 类型
                            colType = colType.GetGenericArguments()[0];
                        }
                        value = Convert.ChangeType(value, colType);

                        ps[i].SetValue(obj, value, null);
                    }
                    oldValues.Add(ps[i].Name, value);
                }
                BaseModel model = obj as BaseModel;
                model._oldValues = oldValues;
                list.Add(obj);
            }
            return list;
        }

        internal static List<T> GetModelsBySql<T>(string sql, DataAccess da)
        {
            return GetModelsByDataTable<T>(da.GetDataTable(sql));
        }

        public override string ToString()
        {
            Type t = GetType();
            PropertyInfo[] ps = t.GetProperties();
            StringBuilder Desc = new StringBuilder();

            foreach (var item in ps)
            {
                Desc.AppendFormat("{0}:{1} \n", item.Name, item.GetValue(this, null));
            }
            return t.Name + "\n" + Desc.ToString();
        }

    }
}
