using LiXinDataAccess;
using LiXinInterface;
using LiXinModels;
using LiXinModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL
{
    public class NewUserBL : INewUser
    {
        public static NewUserDB userDB;
        public NewUserBL()
        {
            if (userDB == null)
            {
                userDB = new NewUserDB();
            }
        }
        #region 增删改查
        /// <summary>
        /// 新进学员录入
        /// </summary>
        /// <param name="user"></param>
        public void AddNewUser(Sys_User model)
        {
            userDB.AddNewUser(model);
        }

        /// <summary>
        /// 新进学员更新
        /// </summary>
        /// <param name="user"></param>
        public void UpdateNewUser(Sys_User model, string userID)
        {
            userDB.UpdateNewUser(model, userID);
        }

        /// <summary>
        /// 新进员工列表
        /// </summary>
        /// <returns></returns>
        public List<Sys_User> GetNewUserList(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = "  UserId desc",
            string where = " 1=1", int Year = -1)
        {
            Year = Year == -1 ? DateTime.Now.Year : Year;
            if (Year == DateTime.Now.Year)
            {
                where += " and IsNew=1";
            }
            else
            {
                where += " and  IsHistoryNew=1  AND NewYear=" + Year;
            }
            var list = userDB.GetNewUserList(startIndex, startLength, where, jsRenderSortField);
            totalcount = list.Count == 0 ? 0 : list.FirstOrDefault().totalCount;
            return list;
        }

        /// <summary>
        /// 新进学员更新
        /// </summary>
        /// <param name="user"></param>
        public void UpdateNewUserNoTrain(Sys_User model, string userID)
        {
            userDB.UpdateNewUserNoTrain(model, userID);
        }

        /// <summary>
        /// 根据语句 人员ID更新
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        public void Update(int userId, string query)
        {
            userDB.Update(userId, query);
        }
        #endregion

        #region 录入独立考试的学分
        /// <summary>
        /// 新增一条数据
        /// </summary>     
        public void InsertNew_UserExamScore(New_UserExamScore model)
        {
            userDB.InsertNew_UserExamScore(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateNew_UserExamScore(New_UserExamScore model)
        {
            userDB.UpdateNew_UserExamScore(model);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public New_UserExamScore GetModel(string where = " 1=1")
        {
            return userDB.GetModel(where);
        }

        /// <summary>
        /// 没有就新增  有就更新
        /// </summary>
        /// <param name="model"></param>
        public void UpdateOrInsert(string loginID, double score, double sumScore)
        {
            userDB.UpdateOrInsert(loginID, (score < 0 ? 0.00 : score), sumScore < 0 ? 0.00 : sumScore);
        }
        #endregion

        /// <summary>
        /// 新员工的年度
        /// </summary>
        /// <returns></returns>
        public List<int> GetNewYear()
        {
            return userDB.GetNewYear();
        }
    }
}
