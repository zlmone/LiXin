using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.User;

namespace LiXinInterface.User
{
    public interface IUserFingerInfor
    {
        /// <summary>
        /// 获取指纹列表
        /// </summary>
        /// <returns></returns>
        List<Sys_UserFinger> GetUserFingerList(out int totalCount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1");

        /// <summary>
        /// 获取指纹信息（导出指纹不为空的所有人）
        /// </summary>
        /// <returns></returns>
        List<Sys_UserFinger> GetUserFingerInfor(string sqlwhere = "");

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="finger"></param>
        void Update(int id, string finger1, string finger2);

         /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="finger"></param>
        void Insert(int userid, string finger1, string finger2);
    }
}
