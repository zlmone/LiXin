using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.User;
using LiXinModels.User;
using LiXinInterface.User;
namespace LiXinBLL
{
    public class UserFingerInforBL : IUserFingerInfor
    {
        protected UserFingerInforDB ufDB;
        public UserFingerInforBL()
        {
            ufDB = new UserFingerInforDB();
        }

        /// <summary>
        /// 获取指纹列表
        /// </summary>
        /// <returns></returns>
        public List<Sys_UserFinger> GetUserFingerList(out int totalCount, int startIndex = 1, int startLength = int.MaxValue, string where = "1=1")
        {
            var list = ufDB.GetUserFingerList(startIndex, startLength, where);
            totalCount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

        /// <summary>
        /// 获取指纹信息（导出指纹不为空的所有人）
        /// </summary>
        /// <returns></returns>
        public List<Sys_UserFinger> GetUserFingerInfor(string sqlwhere="")
        {
            return ufDB.GetUserFingerInfor(sqlwhere);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="finger"></param>
        public void Update(int id, string finger1, string finger2)
        {
            ufDB.Update(id, finger1, finger2);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="finger"></param>
        public void Insert(int userid, string finger1, string finger2)
        {
            ufDB.Insert(userid, finger1, finger2);
        }
    }
}
