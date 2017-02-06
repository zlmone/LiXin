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
namespace LiXinDataAccess.SystemManage
{
   public class Sys_NoteSortDB:BaseRepository
    {
       /// <summary>
       /// 判断类别名称是否存在
       /// </summary>
       /// <param name="courseSystemName"></param>
       /// <param name="Id"></param>
       /// <returns></returns>
       public bool IsExist(string sortName,int type=0, int Id = 0 , int ParentID=0)
       {
           using (IDbConnection conn = OpenConnection())
           {
               string sqlwhere =
                   "select count(*) from Sys_NoteSort WHERE SortName=@SortName and ParentID=@ParentID and type=@type and IsDelete = 0";
               if (Id > 0)
                   sqlwhere += " and Id <> " + Id;
               var param = new
               {
                   sortName,
                   ParentID = ParentID,
                   type = type
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
    @"insert into Sys_NoteSort(SortName,ParentID,Type,IsDelete) values(@SortName,@ParentID,@Type,0)
                SELECT @@Identity AS ID
";
                var param = new
                {
                    model.SortName,
                    model.ParentID,
                    model.Type
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
                      @"update Sys_NoteSort set SortName=@SortName,ParentID=@ParentID,IsDelete=@IsDelete where Id=@Id ";
                var param = new
                {
                    model.Id,
                    model.SortName,
                    model.ParentID,
                    model.IsDelete
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
                string deleteSql = "update Sys_NoteSort set IsDelete = 1  WHERE Id=@Id";
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
       public Sys_NoteSort GetSys_NoteSort(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = "select * from Sys_NoteSort where IsDelete = 0 and Id=@Id";
                var param = new
                {
                    Id
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
               string selectSql = "select * from Sys_NoteSort where " + strWhere + " and Sys_NoteSort.IsDelete = 0 ";
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
               var sql=@"SELECT count(*) count FROM Sys_NoteSort
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
