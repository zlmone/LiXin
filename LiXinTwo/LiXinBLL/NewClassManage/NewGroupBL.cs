using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.NewClassManage;
using LiXinInterface.NewClassManage;
using LixinModels.NewClassManage;

namespace LiXinBLL.NewClassManage
{
    public class NewGroupBL : INewGroup
    {
        private readonly static NewGroupDB newGroupDB = new NewGroupDB();

        /// <summary>
        /// 根据ID获取model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public New_Group GetModel(int id)
        {
            return newGroupDB.GetModel(id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">要添加的实体对象</param>
        public int AddModel(New_Group model)
        {
            newGroupDB.AddModel(model);
            return model.Id;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">要更新的实体对象</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateModel(New_Group model)
        {
            return newGroupDB.UpdateModel(model);
        }

        /// <summary>
        /// 根据条件获得组列表
        /// </summary>
        public List<New_Group> GetList(string where = "")
        {
            return newGroupDB.GetList(where);
        }
    }
}
