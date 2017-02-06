using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinInterface;
using LiXinModels;
using LiXinModels.User;
using LiXinDataAccess;
using LiXinDataAccess.User;
using LiXinBLL.SystemManage;
using LiXinCommon;

namespace LiXinBLL
{
    public class Sys_TrainGradeBL : ISys_TrainGrade
    {
        private readonly UserDB _userDB;
        private readonly Sys_TrainGradeDB trainDB;
        private Sys_ParamConfigBL configBL;
        public Sys_TrainGradeBL()
        {
            trainDB = new Sys_TrainGradeDB();
            _userDB = new UserDB();
            configBL = new Sys_ParamConfigBL();
        }

        #region 培训级别维护
        /// <summary>
        /// 获取培训级别基本详情
        /// </summary>
        /// <param name="realName">人员姓名</param>
        /// <param name="JobNum">人员编号</param>
        /// <param name="payGrade">薪酬级别</param>
        public List<Sys_TrainGrade> GetUserTrain(out int totalCount, int userID,string deptIDs="" , int pageIndex = 1, int pageSize = int.MaxValue, string realName = "", string JobNum = "", string payGrade = "", string deptName = "", string ordersql = "ORDER BY su.DeptPath asc,su.TrainGrade asc")
        {
            string where = "";
            string where1 = "";
            if (!string.IsNullOrEmpty(realName))
            {
                where += string.Format(" and  Realname LIKE '%{0}%'", realName);
            }
            if (!string.IsNullOrEmpty(JobNum))
            {
                where += string.Format(" and  JobNum LIKE '%{0}%'", JobNum);
            }
            if (!string.IsNullOrEmpty(payGrade))
            {
                where += string.Format(" and  payGrade LIKE '%{0}%'", payGrade);
            }
            if (!string.IsNullOrEmpty(deptName))
            {
                where += string.Format(" and  DeptPath LIKE '%{0}%'", deptName);
            }
            if (!string.IsNullOrEmpty(deptIDs))
            {

            }
            where1 = " (su.LeaderID=(SELECT JobNum FROM Sys_User WHERE UserId=@userID)  OR UserId=@userID)"; 
            
            where += " and UserType in ('在职','见习','特批','聘用')";
            var list = trainDB.GetUserTrain(DateTime.Now.Year, userID, where, pageIndex, pageSize, ordersql, where1);
            totalCount = list.Count == 0 ? 0 : list[0].totalCount;
            return list;
        }

        /// <summary>
        /// 新增维护信息
        /// </summary>
        /// <param name="model"></param>
        public void InsertTrainGrade(Sys_TrainGrade model)
        {
            trainDB.InsertTrainGrade(model);
        }

        /// <summary>
        /// 获取批量修改的人员信息
        /// </summary>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        public List<Sys_User> GetUpdateUser(string userIDs)
        {
            return trainDB.GetUpdateUser(userIDs);
        }

        /// <summary>
        /// 更新用户表中的培训级别
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="Grade"></param>
        public void UpdateTrainGrade(int userID, string Grade)
        {
            trainDB.UpdateTrainGrade(userID, Grade);
        }

        /// <summary>
        /// 是否是培训负责人
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="trainMaster"></param>
        /// <param name="selectDept"> </param>
        public void UpdateTrainMaster(int userID, int trainMaster, string selectDept)
        {
            if (trainMaster == 0)
            {
                trainDB.UpdateTrainMaster(userID, string.Format(" TrainMaster=0,ManageDeparts='{0}' ", selectDept));
            }
            else
            {

                trainDB.UpdateTrainMaster(userID, string.Format(" TrainMaster=1,ManageDeparts='{0}' ", selectDept));
            }
        }
        public void UpdateAllTrainMaster(int userID)
        {
            trainDB.UpdateAllTrainMaster(userID);
        }
        #endregion

        #region 培训级别变更
        /// <summary>
        /// 培训级别变更
        /// </summary>
        public List<Sys_TrainGrade> GetTarinUpdate(out int totalCount, string StartTime, string EndTime, string realName = "", string payGrade = "", string DeptName = "", int status = -1, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            string where = " ID IS NOT NULL";
            if (!string.IsNullOrEmpty(realName))
            {
                where += string.Format(" and  Realname LIKE '%{0}%'", realName);
            }
            if (!string.IsNullOrEmpty(DeptName))
            {
                where += string.Format(" and  DeptName LIKE '%{0}%'", DeptName);
            }
            if (!string.IsNullOrEmpty(payGrade))
            {
                where += string.Format(" and  payGrade LIKE '%{0}%'", payGrade);
            }
            if (status != -1)
            {
                where += " and st.Status=" + status;
            }
            //var list = trainDB.GetUserTrain(where, pageIndex, pageSize);
            var yearFirst = Convert.ToDateTime(DateTime.Now.Year + "-01" + "-01");
            var YearEnd = Convert.ToDateTime(DateTime.Now.Year + "-12" + "-31 23:59:59");
            var list = trainDB.GetTarinUpdate(yearFirst.ToString(), YearEnd.ToString(), StartTime, EndTime, where, pageIndex, pageSize);
            totalCount = list.Count == 0 ? 0 : list[0].totalCount;
            return list;
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Sys_TrainGrade GetSingle(int ID)
        {
            var list = trainDB.GetSingle(ID);
            return list.Count > 0 ? list[0] : new Sys_TrainGrade();
        }

        /// <summary>
        /// 更新培训级别
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="newGrade"></param>
        public void UpdateSys_TrainGrade(int ID, string newGrade)
        {
            trainDB.UpdateSys_TrainGrade(ID, newGrade);
        }

        /// <summary>
        /// 更新培训级别
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="newGrade"></param>
        public void AllUpdate(string IDs)
        {
            trainDB.AllUpdate(IDs);
        }
        /// <summary>
        /// 批量变更的时候维护
        /// </summary>
        /// <param name="id"></param>
        public void UpdateTrainGradeBytrain(int id)
        {
            trainDB.UpdateTrainGradeBytrain(id);
        }
        #endregion

        #region 详情
        /// <summary>
        /// 变更详情
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <returns></returns>
        public List<Sys_TrainGrade> GetUpdateLog(out int totalCount, int UserID, int startIndex = 1, int startLength = int.MaxValue)
        {
            var list = trainDB.GetUpdateLog(UserID, startIndex, startLength);
            totalCount = list.Count == 0 ? 0 : list[0].totalCount;
            return list;
        }
        #endregion

        #region 公共
        /// <summary>
        /// 获取所有的薪酬级别
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllPayGrade()
        {
            return trainDB.GetAllPayGrade();
        }

        /// <summary>
        /// 获取所有的培训级别
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllTrainGrade()
        {
            return trainDB.GetAllTrainGrade();
        }

        /// <summary>
        /// 当前时间是否在，培训级别维护时间之内,返回维护时间
        /// </summary>
        /// <param name="now">传不带时间的。。嘎嘎</param>
        public bool IsUpdateTrain(out string startTime, out string endTime, Sys_ParamConfig list, DateTime now)
        {
            try
            {
                var strlist = new List<string>();
                var ArrayValue = list.ConfigValue.Split(';');
                //var startTime = "";
                //var endTime = "";
                var nowyear = DateTime.Now.Year;
                startTime = nowyear + "-" + ArrayValue[1] + "-" + ArrayValue[2];
                endTime = (ArrayValue[3] == "1" ? nowyear + "-" : nowyear + 1 + "-") + ArrayValue[4] + "-" + ArrayValue[5];
                var end = Convert.ToDateTime(endTime).AddDays(1).AddSeconds(-1);
                endTime = end.ToString();
                if (now >= Convert.ToDateTime(startTime) && now <= end)
                {
                    return true;
                }
                return false;

            }
            catch
            {
                startTime = "";
                endTime = "";
                return true;
            }

        }


        /// <summary>
        /// 批量修改人员的级别
        /// </summary>
        /// <param name="userIDs"></param>
        /// <param name="Grade"></param>
        public void UpdateTrainGradeByUserID(string userIDs, string Grade)
        {
            trainDB.UpdateTrainGradeByUserID(userIDs, Grade);
        }
        #endregion

        public void InsertGrade(string grade)
        {
            trainDB.InsertGrade(grade);
        }

        /// <summary>
        /// 获取我的管理部门
        /// </summary>
        /// <param name="jobNum">用户的HRID</param>
        public List<Sys_Department> GetMyDepartMents(string jobNum)
        {
            return trainDB.GetMyDepartMents(jobNum);
        }
    }
}
