using LiXinDataAccess;
using LiXinDataAccess.SystemManage;
using LiXinInterface;
using LiXinModels;
using LiXinModels.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;

namespace LiXinBLL
{
    public class Report_FreeBL : IReport_Free
    {
        private Report_FreeDB FreeDB;
        private FreeConfigDB configDB;
        private Free_UserOtherApplyDB userDB;

        public Report_FreeBL()
        {
            FreeDB = new Report_FreeDB();
            configDB = new FreeConfigDB();
            userDB = new Free_UserOtherApplyDB();
        }

        /// <summary>
        /// 获取授课人
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportFreeDetails> GetCPATeacherList(string where = "1=1", int cpa = -1)
        {
            string teacherWhere = where;

            if (cpa == 0)
            {

                teacherWhere += " AND CPA='是'  AND IsCPA=1 ";
                where += " AND CPA='是'";
            }
            var list = FreeDB.GetCPATeacherList(where);

            //授课学时
            var newteacherList = FreeDB.GetTeacherScoreList(teacherWhere);

            list.AddRange(newteacherList);

            #region 合并
            var newList = new List<ReportFreeDetails>();
            foreach (var item in list)
            {
                if (!(newList.Any(p => p.UserId == item.UserId)))
                {
                    var detailsList = list.Where(p => p.UserId == item.UserId);
                    var model = new ReportFreeDetails();
                    model.UserId = item.UserId;
                    model.Realname = item.Realname;
                    model.Username = item.Username;
                    model.DeptName = item.DeptName;
                    model.TrainGrade = item.TrainGrade;
                    model.CPA = item.CPA;
                    model.CPANo = item.CPANo;
                    model.DeptId = item.DeptId;
                    var FreeDetailsList = new List<FreeDetailsList>();
                    foreach (var details in detailsList)
                    {
                        var model1 = new FreeDetailsList();
                        model1.ClassName = details.ClassName;
                        model1.ConvertScore = details.ConvertScore;
                        model1.GettScore = details.GettScore;
                        model1.GetCPAScore = details.GetCPAScore;
                        model1.typeForm = details.typeForm;
                        FreeDetailsList.Add(model1);
                    }
                    model.FreeDetailsList = FreeDetailsList;
                    newList.Add(model);
                }
            }
            #endregion

            return newList;
        }


        /// <summary>
        /// 获取其他形式
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="startLength"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportFreeDetails> GetOtherList(int startIndex = 0, int startLength = int.MaxValue, string where = "1=1", string jsRenderSortField = " GetCPAScore asc")
        {
            return FreeDB.GetOtherList(startIndex, startLength, where, jsRenderSortField);
        }

        /// <summary>
        /// 获取符合时间的人员
        /// </summary>
        /// <param name="JoinDate"></param>
        /// <param name="CPACreateDate"></param>
        /// <returns></returns>
        public List<Free_UserOtherApply> GetUserByDate(string JoinDate, string CPACreateDate, string where = "1=1")
        {
            return FreeDB.GetUserByDate(JoinDate, CPACreateDate, where);
        }

        /// <summary>
        /// 所有免修的人员ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetFreeUserList(string where = "1=1")
        {
            return FreeDB.GetFreeUserList(where);
        }

    }
}
