using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.SystemManage;

namespace LiXinInterface.SystemManage
{
    public interface IUpdateRecord
    {
        /// <summary>
        /// 添加更新记录
        /// </summary>
        /// <param name="record">实体</param>
        /// <returns></returns>
       int InsertKey(UpdateRecord record);

        /// <summary>
        /// 修改添加更新记录
        /// </summary>
        /// <param name="record">实体</param>
        /// <returns></returns>
        int ModifyKey(UpdateRecord record);

        /// <summary>
        /// 获取用户的更新记录
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        UpdateRecord GetUserRecord(int userID);
    }
}
