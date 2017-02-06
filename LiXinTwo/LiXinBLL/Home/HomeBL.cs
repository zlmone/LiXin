using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.Home;
using LiXinInterface.Home;

namespace LiXinBLL.Home
{
    public class HomeBL : IHome
    {
        protected HomeDB Hdb = new HomeDB();

        /// <summary>
        /// 获取我考试通过的次数（部门/分所）
        /// </summary>
        public int GetMyPassExamCount(int userId, int year = -1)
        {
            return Hdb.GetMyPassExamCount(userId, year);
        }
    }
}
