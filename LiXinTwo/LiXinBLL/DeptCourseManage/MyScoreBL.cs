using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess;
using LiXinInterface;
using LiXinModels.DeptCourseManage;
using LiXinModels.User;

namespace LiXinBLL
{
    public class MyScoreBL : IMyScore
    {
        private static MyScoreDB scoreDB;
        public MyScoreBL()
        {
            scoreDB = new MyScoreDB();
        }

        /// <summary>
        /// 获取我的学时及考勤获取
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<CourseShow> GetCourseShow(out int totalcount, int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue, string jsrendersortfield = " userID")
        {

            var list = scoreDB.GetCourseShow(userID, where, startIndex, startLength,jsrendersortfield);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }

        /// <summary>
        /// 部门/分所员工学时及考勤获取
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<Sys_User> GetDeptUserScore(out int totalcount, int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " Realname asc")
        {
            var list = scoreDB.GetDeptUserScore(userID, where, startIndex, startLength, jsRenderSortField);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        /// <summary>
        /// 部门/分所下人员的获取学时详情
        /// </summary>
        /// <param name="mainUserID">部门/分所的领导人</param>
        /// <param name="userID">要查看的人员ID</param>
        /// <returns></returns>
        public List<CourseShow> GetDepUserScoreDetails(out int totalcount, int mainUserID, int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue)
        {
            var list = scoreDB.GetDepUserScoreDetails(mainUserID,userID, where, startIndex, startLength);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalcount;
            return list;
        }
    }
}
