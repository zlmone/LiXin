using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.SystemManage
{
    public interface IFree_OtherFroms
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        void AddOtherFroms(Free_OtherFroms model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateOtherFroms(Free_OtherFroms model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteOtherFroms(string id);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<Free_OtherFroms> GetOtherFromsList(string where = " 1 = 1 ");


        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Free_OtherFroms GetOtherFromsById(int id);

    }
}
