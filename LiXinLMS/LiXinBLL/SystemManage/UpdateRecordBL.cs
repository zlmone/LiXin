using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess;
using LiXinInterface.SystemManage;
using LiXinModels.SystemManage;
using MongoDB.Driver;

namespace LiXinBLL.SystemManage
{
    public class UpdateRecordBL : IUpdateRecord
    {
        protected BaseMethod BaseM;
        public UpdateRecordBL()
        {
            BaseM = new BaseMethod();
        }

        /// <summary>
        /// 添加更新记录
        /// </summary>
        /// <param name="record">实体</param>
        /// <returns></returns>
        public int InsertKey(UpdateRecord record)
        {
            return BaseM.Insert(record);
        }

        /// <summary>
        /// 修改添加更新记录
        /// </summary>
        /// <param name="record">实体</param>
        /// <returns></returns>
        public int ModifyKey(UpdateRecord record)
        {
            return BaseM.Modify(record) ? record._id : 0;
        }

        /// <summary>
        /// 获取用户的更新记录
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public UpdateRecord GetUserRecord(int userID)
        {
            return BaseM.GetAllList<UpdateRecord>(new QueryDocument("UserID", userID)).FirstOrDefault();
        }
    }
}
