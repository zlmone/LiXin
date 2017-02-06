using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LiXinModels.CourseManage;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.CourseManage
{
    public class Co_VideoResourceDB : BaseRepository
    {
        public Co_VideoResource GetCo_VideoResourceeResource(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sqlwhere = "select * from Co_VideoResource where Id=@Id";
                var param = new
                {
                    Id
                };
                return connection.Query<Co_VideoResource>(sqlwhere, param).FirstOrDefault();

            }
        }


        public List<Co_VideoResource> GetList(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                string sql = "select * from Co_VideoResource where Id=" + Id;
                return connection.Query<Co_VideoResource>(sql).ToList();
            }
        }


        public bool AddCo_VideoResource(Co_VideoResource model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Co_VideoResource (Id,TopContent,LeftContent,RightContent,BottomContent,LastUpdateTime,VideoId)
values (@Id,@TopContent,@LeftContent,@RightContent,@BottomContent,@LastUpdateTime,@VideoId)";
                var param = new
                {
                    model.Id,
                    model.TopContent,
                    model.LeftContent,
                    model.RightContent,
                    model.BottomContent,
                    LastUpdateTime=DateTime.Now,
                    model.VideoId
                };
                return conn.Execute(sqlwhere, param) > 0;
            }
        }


        public bool DeleteCo_VideoResource(int Id)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return
                    connection.Execute("Update   Co_VideoResource  where Id=" + Id) > 0;
            }
        }

    }
}
