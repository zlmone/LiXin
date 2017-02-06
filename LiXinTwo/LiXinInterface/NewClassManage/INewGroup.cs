using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;
using LixinModels.NewClassManage;

namespace LiXinInterface.NewClassManage
{
    public interface INewGroup
    {
        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        New_Group GetModel(int id);

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">要添加的实体对象</param>
        int AddModel(New_Group model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateModel(New_Group model);

        /// <summary>
        /// 根据条件获得组列表
        /// </summary>
        List<New_Group> GetList(string where = "");
    }
}
