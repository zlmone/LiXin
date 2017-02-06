﻿using LiXinModels.DeptCourseManage;
using LiXinModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface
{
    public interface IMyScore
    {
        /// <summary>
        /// 获取我的学时及考勤获取
        /// </summary>
        /// <returns></returns>
        List<CourseShow> GetCourseShow(out int totalcount, int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue, string jsrendersortfield = " userID");

        /// <summary>
        /// 部门/分所员工学时及考勤获取
        /// </summary>
        /// <returns></returns>
        List<Sys_User> GetDeptUserScore(out int totalcount, int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = " Realname asc");

        /// <summary>
        /// 部门/分所下人员的获取学时详情
        /// </summary>
        /// <param name="mainUserID">部门/分所的领导人</param>
        /// <param name="userID">要查看的人员ID</param>
        /// <returns></returns>
        List<CourseShow> GetDepUserScoreDetails(out int totalcount, int mainUserID, int userID, string where = " 1=1", int startIndex = 1, int startLength = int.MaxValue);
    }
}