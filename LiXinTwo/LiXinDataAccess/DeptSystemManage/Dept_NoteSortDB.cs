using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.CourseManage;
using LiXinModels.SystemManage;
namespace LiXinDataAccess
{
   public class Dept_NoteSortDB:BaseRepository
    {
       /// <summary>
       /// 判断类别名称是否存在
       /// </summary>
       /// <param name="courseSystemName"></param>
       /// <param name="Id"></param>
       /// <returns></returns>
       public bool IsExist(int departId, string sortName, int type = 0, int Id = 0, int ParentID = 0)
       {
           using (IDbConnection conn = OpenConnection())
           {
               string sqlwhere =
                   "select count(*) from Dep_NoteSort WHERE SortName=@SortName and ParentID=@ParentID and type=@type and IsDelete = 0 and DeptId=@departId";
               if (Id > 0)
                   sqlwhere += " and Id <> " + Id;
               var param = new
               {
                   sortName,
                   ParentID = ParentID,
                   type = type,
                   departId=departId
               };
               int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
               return count > 0;
           }
       }

        /// <summary>
        ///     增加一条数据
        /// </summary>
       public void AddSys_NoteSort(Sys_NoteSort model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string strSql =
    @"insert into Dep_NoteSort(SortName,ParentID,Type,IsDelete,DeptId) values(@SortName,@ParentID,@Type,0,@DeptId)
                SELECT @@Identity AS ID
";
                var param = new
                {
                    model.SortName,
                    model.ParentID,
                    model.Type,
                    model.DeptId
                };

                decimal id =
                    connection.Query<decimal>(strSql, param)
                              .FirstOrDefault();
                model.Id = decimal.ToInt32(id);
            }
        }

        /// <summary>
        ///     更新一条数据
        /// </summary>
       public bool UpdateSys_NoteSort(Sys_NoteSort model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string updateSql =
                      @"update Dep_NoteSort set SortName=@SortName,ParentID=@ParentID,IsDelete=@IsDelete where Id=@Id and DeptId=@DeptId";
                var param = new
                {
                    model.Id,
                    model.SortName,
                    model.ParentID,
                    model.IsDelete,
                    model.DeptId
                };
                return connection.Execute(updateSql, param) > 0;
            }
        }

        /// <summary>
        ///     删除一条数据
        /// </summary>
       public bool DeleteSys_NoteSort(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string deleteSql = "update Dep_NoteSort set IsDelete = 1  WHERE Id=@Id";
                var param = new
                {
                    Id
                };
                return connection.Execute(deleteSql, param) > 0;
            }
        }

        /// <summary>
        ///     得到一个对象实体
        /// </summary>
       public Sys_NoteSort GetSys_NoteSort(int departId, int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = "select * from Dep_NoteSort where IsDelete = 0 and Id=@Id and DeptId=@departId";
                var param = new
                {
                    Id,
                    departId
                };
                return connection.Query<Sys_NoteSort>(sqlwhere, param).FirstOrDefault();

            }
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
       public List<Sys_NoteSort> GetSys_NoteSortList(string strWhere = "1=1")
       {
           using (IDbConnection connection = OpenConnection())
           {
               string selectSql = "select * from Dep_NoteSort where " + strWhere + " and Dep_NoteSort.IsDelete = 0 ";
               return connection.Query<Sys_NoteSort>(selectSql).ToList();
           }
       }

       /// <summary>
       /// 是否有下级
       /// </summary>
       /// <param name="ParentID"></param>
       /// <returns></returns>
       public int IsHaveChild(int parentID)
       {
           using (var conn = OpenConnection())
           {
               var sql = @"SELECT count(*) count FROM Dep_NoteSort
WHERE ParentID=@parentID";
               var param = new
               {
                   parentID = parentID
               };
               var list = conn.Query<dynamic>(sql, param).FirstOrDefault();
               return decimal.ToInt32(list.count);

           }
       }
    }
}
