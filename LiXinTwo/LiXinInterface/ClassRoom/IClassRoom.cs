using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;

namespace LiXinInterface.ClassRoom
{
    public interface IClassRoom
    {
        /// <summary>
        /// 根据ID获取教室
        /// </summary>
        /// <param name="roomID">教室ID</param>
        /// <returns></returns>
        Sys_ClassRoom GetRoom(int roomID);
        /// <summary>
        /// 获取所有教室列表
        /// </summary>
        /// <param name="totalCount">总数</param>
        /// <param name="where">查询条件</param>
        /// <param name="startIndex">开始页数</param>
        /// <param name="pageLength">每页条数</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<Sys_ClassRoom> GetRoomListPaging(out int totalCount, string where = " 1 = 1 ", int startIndex = 0,
                                                     int pageLength = int.MaxValue,
                                                     string orderBy = " order by LastUpdateTime desc ");

        /// <summary>
        /// 获取所有教室列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<Sys_ClassRoom> GetRoomList(string where = " IsDelete=0 ",
                                               string orderBy = " order by LastUpdateTime desc ");

        /// <summary>
        /// 增加教室
        /// </summary>
        /// <param name="model">教室实体</param>
        bool AddRoom(Sys_ClassRoom model);

        /// <summary>
        /// 修改教室
        /// </summary>
        /// <param name="model">教室实体</param>
        bool EditRoom(Sys_ClassRoom model);

        /// <summary>
        /// 删除教室
        /// </summary>
        /// <param name="id">教室ID</param>
        /// <returns></returns>
        bool DeleteRoom(int id);
        /// <summary>
        /// 判断教室是否存在
        /// </summary>
        /// <param name="roomName">教室名称</param>
        /// <param name="roomID">教室ID</param>
        bool IsExsitRoom(string roomName, int roomID);
    }
}
