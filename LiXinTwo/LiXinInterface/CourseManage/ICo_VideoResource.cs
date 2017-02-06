using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseManage;

namespace LiXinInterface.CourseManage
{
    public interface ICo_VideoResource
    {
        /// <summary>
        /// 查找单个实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Co_VideoResource GetCo_VideoResourceeResource(int Id);

        /// <summary>
        /// 查找集合
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        List<Co_VideoResource> GetList(int Id);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddCo_VideoResource(Co_VideoResource model);

        /// <summary>
        /// 删除一个事体 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DeleteCo_VideoResource(int Id);

    }
}
