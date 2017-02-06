using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LiXinModels;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.ClassRoom
{
    public class ClassRoomDB : BaseRepository
    {
        /// <summary>
        /// 根据ID获取教室
        /// </summary>
        /// <param name="roomID"></param>
        /// <returns></returns>
        public Sys_ClassRoom GetRoom(int roomID)
        {
            using (var connection = OpenConnection())
            {
                const string sqlstr = "select * from Sys_ClassRoom where Id=@roomID";
                return connection.Query<Sys_ClassRoom>(sqlstr, new { roomID }).FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取所有教室列表
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<Sys_ClassRoom> GetRoomListPaging(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,int pageLength = int.MaxValue, string orderBy = " order by LastUpdateTime desc ")
        {
            using (var connection = OpenConnection())
            {
                totalCount = connection.Query<int>("select count(1) from Sys_ClassRoom where " + where).First();
                var query = string.Format(@"
select top {0} * from (
    select  row_number() over( {1} ) as rowNum,Sys_ClassRoom.* from Sys_ClassRoom
    where " + where + @" 
) result 
where rowNum between  @pageCount*@pageIndex+1 and @pageCount*(@pageIndex+1)", pageLength, orderBy);
                var parameters = new
                {
                    pageCount = pageLength,
                    pageIndex = startIndex / pageLength
                };
                return connection.Query<Sys_ClassRoom>(query, parameters).ToList();
            }
        }
        /// <summary>
        /// 获取所有教室列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<Sys_ClassRoom> GetRoomList(string where = " IsDelete=0 ", string orderBy = " order by LastUpdateTime desc ")
        {
            using (var connection = OpenConnection())
            {
                var query = string.Format(@"select Sys_ClassRoom.* from Sys_ClassRoom where {0} {1} ",where, orderBy);
                return connection.Query<Sys_ClassRoom>(query).ToList();
            }
        }

        /// <summary>
        /// 增加教室
        /// </summary>
        /// <param name="model">教室实体</param>
        public bool AddRoom(Sys_ClassRoom model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"insert into Sys_ClassRoom (RoomName,RoomNumber,ColumnNumber,RowNumber,Memo,UserID,LastUpdateTime,IsDelete) values (@RoomName,@RoomNumber,@ColumnNumber,@RowNumber,@Memo,@UserID,@LastUpdateTime,@IsDelete) SELECT @@Identity as Id";
                var param = new
                {
                    model.RoomName,
                    model.RoomNumber,
                    model.ColumnNumber,
                    model.RowNumber,
                    model.Memo,
                    model.UserID,
                    model.LastUpdateTime,
                    model.IsDelete,
                    model.Address
                };
                var id = conn.Query<decimal>(sqlwhere, param).FirstOrDefault();
                model.Id = decimal.ToInt32(id);
                return model.Id > 0;
            }
        }

        /// <summary>
        /// 修改教室
        /// </summary>
        /// <param name="model">教室实体</param>
        public bool EditRoom(Sys_ClassRoom model)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Sys_ClassRoom set RoomName=@RoomName,RoomNumber=@RoomNumber,ColumnNumber=@ColumnNumber,RowNumber=@RowNumber,Memo=@Memo,Address=@Address where Id=@Id";
                var param = new
                {
                    model.RoomName,
                    model.RoomNumber,
                    model.ColumnNumber,
                    model.RowNumber,
                    model.Memo,
                    model.Id,
                    model.Address
                };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 删除教室
        /// </summary>
        /// <param name="id">教室ID</param>
        /// <returns></returns>
        public bool DeleteRoom(int id)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"update Sys_ClassRoom set IsDelete=1 where Id=@id";
                var param = new
                {
                    id
                };
                var result = conn.Execute(sqlwhere, param);
                return result > 0;
            }
        }

        /// <summary>
        /// 判断教室是否存在
        /// </summary>
        /// <param name="roomName">教室名称</param>
        /// <param name="roomID">教室ID</param>
        public bool IsExsitRoom(string roomName,int roomID)
        {
            using (var conn = OpenConnection())
            {
                const string sqlwhere =
                    @"select count(*) from Sys_ClassRoom where RoomName=@roomName and Id<>@roomID and IsDelete=0";
                var param = new
                {
                    roomName,
                    roomID
                };
                var count = conn.Query<int>(sqlwhere, param).FirstOrDefault();
                return count == 0;
            }
        }
    }
}
