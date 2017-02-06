using LiXinModels;
using LiXinModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface
{
    public interface INewUser
    {
        /// <summary>
        /// 新进学员录入
        /// </summary>
        /// <param name="user"></param>
        void AddNewUser(Sys_User model);

        /// <summary>
        /// 新进学员更新
        /// </summary>
        /// <param name="user"></param>
        void UpdateNewUser(Sys_User model, string userID);

        /// <summary>
        /// 新进员工列表
        /// </summary>
        /// <returns></returns>
        List<Sys_User> GetNewUserList(out int totalcount, int startIndex = 1, int startLength = int.MaxValue, string jsRenderSortField = "  UserId desc",
         string where = " 1=1", int Year=-1);

        /// <summary>
        /// 新进学员更新
        /// </summary>
        /// <param name="user"></param>
        void UpdateNewUserNoTrain(Sys_User model, string userID);


        /// <summary>
        /// 新增一条数据
        /// </summary>     
        void InsertNew_UserExamScore(New_UserExamScore model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        void UpdateNew_UserExamScore(New_UserExamScore model);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        New_UserExamScore GetModel(string where = " 1=1");

        /// <summary>
        /// 根据语句 人员ID更新
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        void Update(int userId, string query);

        /// <summary>
        /// 没有就新增  有就更新
        /// </summary>
        /// <param name="model"></param>
        void UpdateOrInsert(string loginID, double score, double sumScore);

        /// <summary>
        /// 新员工的年度
        /// </summary>
        /// <returns></returns>
        List<int> GetNewYear();

    }

}
