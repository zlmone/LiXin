using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using LiXinModels;
using System.Data;
using LiXinModels.CourseManage;
namespace LiXinDataAccess
{
   public class Co_CourseSystemDB:BaseRepository
    {

       /// <summary>
       /// 判断体系名称是否存在
       /// </summary>
       /// <param name="courseSystemName"></param>
       /// <param name="Id"></param>
       /// <returns></returns>
       public bool IsExist(string courseSystemName, int ParentID, int Id = 0)
       {
           using (IDbConnection conn = OpenConnection())
           {
               string sqlwhere =
                   "select count(*) from Co_CourseSystem WHERE courseSystemName=@courseSystemName and ParentID=@ParentID and IsDelete = 0";
               if (Id > 0)
                   sqlwhere += " and Id <> " + Id;
               var param = new
               {
                   courseSystemName,
                   ParentID = ParentID
               };
               int count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
               return count > 0;
           }
       }

        /// <summary>
        ///     增加一条数据
        /// </summary>
       public void AddCourseSystem(Co_CourseSystem model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string strSql =
    @"insert into Co_CourseSystem(CourseSystemName,ParentID,Memo,IsDelete) values(@CourseSystemName,@ParentID,@Memo,@IsDelete)
                SELECT @@Identity AS ID
";
                var param = new
                {
                    model.CourseSystemName,
                    ParentID = model.ParentID,
                    model.Memo,
                    model.IsDelete
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
       public bool UpdateCourseSystem(Co_CourseSystem model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string updateSql =
                      @"update Co_CourseSystem set CourseSystemName=@CourseSystemName,ParentID=@ParentID,Memo=@Memo,IsDelete=@IsDelete where Id=@Id ";
                var param = new
                {
                    model.Id,
                    model.CourseSystemName,
                    ParentID = model.ParentID,
                    model.Memo,
                    model.IsDelete
                };
                return connection.Execute(updateSql, param) > 0;
            }
        }

        /// <summary>
        ///     删除一条数据
        /// </summary>
       public bool DeleteCourseSystem(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string deleteSql = "update Co_CourseSystem set IsDelete = 1  WHERE Id=@Id";
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
       public Co_CourseSystem GetCourseSystem(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = "select * from Co_CourseSystem where IsDelete = 0 and Id=@Id";
                var param = new
                {
                    Id
                };
                return connection.Query<Co_CourseSystem>(sqlwhere,param).FirstOrDefault();

            }
        }

        /// <summary>
        ///     获得数据列表
        /// </summary>
       public List<Co_CourseSystem> GetCo_CourseSystemList(string strWhere = "1=1")
        {
            using (IDbConnection connection = OpenConnection())
            {
                if (strWhere == "")
                {
                    strWhere = "1=1";
                }
                string selectSql = "select * from Co_CourseSystem where " + strWhere + " and Co_CourseSystem.IsDelete = 0 ";
                return connection.Query<Co_CourseSystem>(selectSql).ToList();
            }
        }





    }
}
