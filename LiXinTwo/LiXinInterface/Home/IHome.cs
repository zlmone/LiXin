using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.Home
{
    public interface IHome
    {
        /// <summary>
        /// 获取我考试通过的次数（部门/分所）
        /// </summary>
        int GetMyPassExamCount(int userId, int year = -1);
    }
}
