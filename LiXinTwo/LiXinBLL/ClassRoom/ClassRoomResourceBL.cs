using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.ClassRoom;
using LiXinInterface.ClassRoom;
using LiXinModels;

namespace LiXinBLL.ClassRoom
{
    public class ClassRoomResourceBL:IClassRoomResource
    {
        protected ClassRoomResourceDB roomDb;

        public ClassRoomResourceBL()
        {
            roomDb = new ClassRoomResourceDB();
        }

        /// <summary>
        /// 获取教室资源列表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <returns></returns>
        public List<Sys_ClassRoomResource> GetRoomResourceList(string where = " IsDelete=0 ", string orderBy = " order by CreateDate desc ")
        {
            return roomDb.GetRoomResourceList(where, orderBy);
        }

        /// <summary>
        /// 新增教室资源
        /// </summary>
        /// <param name="model">教室资源实体</param>
        public bool AddRoomResource(Sys_ClassRoomResource model)
        {
            roomDb.AddRoomResource(model);
            return model.Id > 0;
        }

        /// <summary>
        /// 删除指定教室下资源信息
        /// </summary>
        /// <param name="id">教室ID</param>
        /// <returns></returns>
        public bool DeleteRoomResourcesByRoomID(int id)
        {
            return roomDb.DeleteRoomResourcesByRoomID(id);
        }

        /// <summary>
        /// 根据教室资源关联表主键删除资源
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        public bool DeleteRoomResourcesByID(int id)
        {
            return roomDb.DeleteRoomResourcesByID(id);
        }
    }
}
