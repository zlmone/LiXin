using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels;

namespace LiXinInterface.ClassRoom
{
    public interface IClassRoomResource
    {
        /// <summary>
        /// 获取教室资源列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        List<Sys_ClassRoomResource> GetRoomResourceList(string where = " IsDelete=0 ", string orderBy = " order by CreateDate desc ");
        

        /// <summary>
        /// 新增教室资源
        /// </summary>
        /// <param name="model">教室资源实体</param>
        bool AddRoomResource(Sys_ClassRoomResource model);

        /// <summary>
        /// 删除指定教室下资源信息
        /// </summary>
        /// <param name="id">教室ID</param>
        /// <returns></returns>
        bool DeleteRoomResourcesByRoomID(int id);

        /// <summary>
        /// 根据教室资源关联表主键删除资源
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        bool DeleteRoomResourcesByID(int id);
    }
}
