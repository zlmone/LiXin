using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Retech.Core;
using Retech.Data;

namespace LiXinDataAccess.Home
{
    public class HomeDB: BaseRepository
    {
        /// <summary>
        /// 获取我考试通过的次数（部门/分所）
        /// </summary>
        public int GetMyPassExamCount(int userId,int year=-1)
        {
            using (var conn = OpenConnection())
            {
                var sql = string.Format(@"SELECT (DepTran+Dep)
FROM
(
SELECT
(SELECT count(*) FROM DepTran_CourseOrder  dc
 LEFT JOIN Dep_Course cc   ON dc.CourseId=cc.Id
WHERE UserId={0} AND PassStatus=1 {1}) AS DepTran,
(SELECT count(*) FROM Dep_CourseOrder dc
LEFT JOIN Dep_Course cc   ON dc.CourseId=cc.Id
 WHERE UserId={0} AND PassStatus=1 {1}
 AND OrderStatus=1 AND (dc.IsLeave=0 OR (dc.IsLeave=1 AND dc.ApprovalFlag<>1))) AS Dep
) AS aa", userId, year == -1 ? "" : " AND cc.YearPlan=" + year);
                return conn.Query<int>(sql).FirstOrDefault();
            }
        }
    }
}
