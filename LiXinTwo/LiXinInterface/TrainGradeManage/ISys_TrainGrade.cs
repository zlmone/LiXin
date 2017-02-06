using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.User;
using LiXinModels;

namespace LiXinInterface
{
    public interface ISys_TrainGrade
    {
        /// <summary>
        /// 获取培训级别基本详情
        /// </summary>
        /// <param name="realName">人员姓名</param>
        /// <param name="JobNum">人员编号</param>
        /// <param name="payGrade">薪酬级别</param>
        List<Sys_TrainGrade> GetUserTrain(out int totalCount, int userID,string deptIDs="" ,int pageIndex = 1, int pageSize = int.MaxValue, string realName = "", string JobNum = "", string payGrade = "", string deptName = "", string ordersql = "ORDER BY su.DeptPath asc,su.TrainGrade asc");

        /// <summary>
        /// 新增维护信息
        /// </summary>
        /// <param name="model"></param>
        void InsertTrainGrade(Sys_TrainGrade model);

        /// <summary>
        /// 获取所有的薪酬级别
        /// </summary>
        /// <returns></returns>
        List<string> GetAllPayGrade();

        /// <summary>
        /// 获取所有的培训级别
        /// </summary>
        /// <returns></returns>
        List<string> GetAllTrainGrade();

        /// <summary>
        /// 当前时间是否在，维护时间之内,返回维护时间
        /// </summary>
        /// <param name="now">传不带时间的。。嘎嘎</param>
        bool IsUpdateTrain(out string startTime, out string endTime, Sys_ParamConfig list, DateTime now);

        /// <summary>
        /// 更新用户表中的培训级别
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="Grade"></param>
        void UpdateTrainGrade(int userID, string Grade);

        /// <summary>
        /// 批量变更的时候维护
        /// </summary>
        /// <param name="id"></param>
        void UpdateTrainGradeBytrain(int id);

        /// <summary>
        /// 获取批量修改的人员信息
        /// </summary>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        List<Sys_User> GetUpdateUser(string userIDs);

        /// <summary>
        /// 培训级别变更
        /// </summary>
        List<Sys_TrainGrade> GetTarinUpdate(out int totalCount, string StartTime, string EndTime, string realName = "", string payGrade = "", string DeptName = "", int status = -1, int pageIndex = 1, int pageSize = int.MaxValue);

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Sys_TrainGrade GetSingle(int ID);

        /// <summary>
        /// 更新培训级别
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="newGrade"></param>
        void UpdateSys_TrainGrade(int ID, string newGrade);

        /// <summary>
        /// 变更详情
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        List<Sys_TrainGrade> GetUpdateLog(out int totalCount, int UserID, int startIndex = 1, int startLength = int.MaxValue);

        void InsertGrade(string grade);

        /// <summary>
        /// 更新培训级别
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="newGrade"></param>
        void AllUpdate(string IDs);

        /// <summary>
        /// 是否是培训负责人
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="trainMaster"></param>
        void UpdateTrainMaster(int userID, int trainMaster, string selectDept);

        void UpdateAllTrainMaster(int userID);

         /// <summary>
        /// 批量修改人员的级别
        /// </summary>
        /// <param name="userIDs"></param>
        /// <param name="Grade"></param>
        void UpdateTrainGradeByUserID(string userIDs, string Grade);

        /// <summary>
        /// 获取我的管理部门
        /// </summary>
        /// <param name="jobNum">用户的HRID</param>
        List<Sys_Department> GetMyDepartMents(string jobNum);
    }
}
