using LiXinModels;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.SystemManage
{
    public interface IFree_AllOtherApply
    {
        void AddFree_AllOtherApply(Free_AllOtherApply model);

        bool UpdateFree_AllOtherApply(Free_AllOtherApply model);

        bool DeleteOtherFroms(string id);

        Free_AllOtherApply GetOtherFromsById(int id);

        
         /// <summary>
        /// 获得批量添加的人员数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<dynamic> GetUserIDByOtherFroms(int id);

          /// <summary>
        /// 获取批量的所有人员数据
        /// </summary>
        List<ShowFreeUserDetails> GetUserDetails(int id);
    }
}
