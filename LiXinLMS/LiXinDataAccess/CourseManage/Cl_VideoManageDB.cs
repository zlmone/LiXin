using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;
using System.Data;
using LiXinModels.CourseManage;
namespace LiXinDataAccess.CourseManage
{
    public class Cl_VideoManageDB : BaseRepository
    {

        public int Add(Cl_VideoManage model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string strSql =
    @"insert into Cl_VideoManage(Name,Path,LastUpdateTime,Size,IsDelete,TopContent,LeftContent,RightContent,BottomContent) values(@Name,@Path,@LastUpdateTime,@Size,@IsDelete,@TopContent,@LeftContent,@RightContent,@BottomContent)
                SELECT @@Identity AS ID
";
                var param = new
                {
                    model.Name,
                    model.Path,
                    model.LastUpdateTime,
                    model.Size,
                    model.IsDelete,
                    model.TopContent,
                    model.LeftContent,
                    model.RightContent,
                    model.BottomContent
                };

                decimal id =
                    connection.Query<decimal>(strSql, param)
                              .FirstOrDefault();
                model.Id = decimal.ToInt32(id);
                return model.Id;
            }
        }


        public bool Modify(Cl_VideoManage model)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string strSql =
    @"Update Cl_VideoManage set Name=@Name,Path=@Path,LastUpdateTime=@LastUpdateTime,Size=@Size,IsDelete=@IsDelete where Id=@Id
                SELECT @@Identity AS ID
";
                var param = new
                {
                    model.Name,
                    model.Path,
                    model.LastUpdateTime,
                    model.Size,
                    model.IsDelete,
                    model.Id
                };
                   return  connection.Execute(strSql, param)>0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageLength"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<Cl_VideoManage> GetCl_VideoManageList(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                              int pageLength = int.MaxValue, string orderBy = "ORDER BY Cl_VideoManage.Id DESC")
        {
            using (IDbConnection connection = OpenConnection())
            {
                totalCount =
                    connection.Query<int>("select count(1) from Cl_VideoManage where " + where + @" AND Cl_VideoManage.IsDelete=0 ")
                              .First();
                string query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,*
 from  Cl_VideoManage 
    where " + where + @" AND Cl_VideoManage.IsDelete=0                           
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Cl_VideoManage>(query, parameters).ToList();
            }
        }
        

    }
}
