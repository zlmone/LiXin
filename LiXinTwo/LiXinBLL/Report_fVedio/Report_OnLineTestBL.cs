using LiXinBLL.Report_Vedio;
using LiXinBLL.SystemManage;
using LiXinCommon;
using LiXinDataAccess;
using LiXinDataAccess.CourseManage;
using LiXinDataAccess.Examination;
using LiXinDataAccess.Report_fVedio;
using LiXinDataAccess.User;
using LiXinInterface.Report_fVedio;
using LiXinModels.CourseManage;
using LiXinModels.Examination.DBModel;
using LiXinModels.Report_Vedio;
using LiXinModels.User;
using MongoDB.Driver.Builders;
using Retech.Cache;
using Retech.Core.Cache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LiXinBLL.Report_fVedio
{
    public class Report_OnLineTestBL : IReport_OnLineTest
    {


        private static readonly Report_fOnLineTestDB fonlineDB = new Report_fOnLineTestDB();
        private readonly ExaminationDB _ExamDB = new ExaminationDB();
        private static readonly UserDB _userDB = new UserDB();
        private static readonly Co_CourseDB courseDb = new Co_CourseDB();
        private static readonly Report_FreeDB freeDB = new Report_FreeDB(); 
        protected ICacheManager cacheManager = new MemoryCacheManager();
      


        public List<Report_OnLineTest> GetOnLineTest(int courseid, string where)
        {


            var fvedioNumber = cacheManager.Get("fVedio_JoinNumber", 1440, () => { return (new Report_VedioBL()).GetfVedioJoinNumber(); });
            //以部门为单位
            List<Report_OnLineTest> list = fonlineDB.GetOnLineTest(courseid, where);

            //以学习课程的UserId为单位
            var examlist = _ExamDB.GetListByrelationId(courseid);

            var checkUser = (new Report_VedioBL()).GetCheckUserList().Select(p => p.UserId).ToList();
            examlist = examlist.Where(p => checkUser.Contains(p.UserID)).ToList();

            var listID = new List<int>();
            foreach (var item in list)
            {
                //list[i].AvgAnswerNum = examlist.Where(p => list[i].userlist.Contains(p.UserID.ToString()));
                //var AvgAnswerNum=examlist.Where(p => list[i].userlist.Contains(p.UserID.ToString())).Select(p=>p.TestTimes).ToList();
                if (!listID.Contains(item.DeptId))
                {
                    listID.Add(item.DeptId);
                    if (item.istest == 1)
                    {
                        if (fvedioNumber.Where(p => p.DeptID == item.DeptId && p.courseID == courseid).Count() != 0 && fvedioNumber != null)
                        {
                            item.YingCanJia = fvedioNumber.Where(p => p.DeptID == item.DeptId && p.courseID == courseid).FirstOrDefault().count;
                        }
                        else
                        {
                            item.YingCanJia = 0;

                        }
                    }
                }
            }

            #region 新方法

            var liststr = new List<string>();
            var newVedioList = new List<Report_OnLineTest>();
            var listIDs = new List<int>();
            foreach (var item in list)
            {
                if (!liststr.Contains(item.DeptTwoName))
                {
                    liststr.Add(item.DeptTwoName);
                    // var newvedio = new Report_OnLineTest();
                    var newlist = list.Where(p => p.DeptTwoName == item.DeptTwoName).ToList();
                    var sss = newlist.Count();
                    var listuserID = newlist.Select(p => p.UserId);
                    var newexamlist = examlist.Where(p => listuserID.Contains(p.UserID)).ToList();
                    listIDs.AddRange(listuserID);
                    item.DeptName = item.DeptName;
                    item.DeptIDs = string.Join(",", newlist.Select(p => p.DeptId).Distinct());
                    item.YingCanJia = newlist.Sum(p => p.YingCanJia);

                    //  item.OnLineTestNum = newlist.Sum(p => p.OnLineTestNum);
                    // item.IsPassNum = newlist.Select(p=>p.DeptTwoName,) .Sum(p => p.IsPassNum);

                    item.IsPassNum = newlist.Count(p => p.IsPass == 1);
                    //item.IsPassNum = newexamlist.Count(p => p.Number == 1 && checkUser.Contains(p.UserID));
                    item.OnLineTestNum = newexamlist.Count();
                    item.AvgAnswerNum = ReportCommon.CalculateMedian(newexamlist.Where(p => p.DoExamStatus > 0).Select(p => p.TestTimes).ToList()).ToString();

                    item.AvgAnswerTime = ReportCommon.CalculateMedian(newexamlist.Where(p => p.DoExamStatus > 0).Select(p => Convert.ToDouble(p.AnswerTime)).ToList()).ToString();

                    item.Avg = ReportCommon.CalculateMedian(newexamlist.Where(p => p.DoExamStatus > 0).Select(p => p.StudentAnswerList.Sum(a => a.GetScore)).ToList()).ToString();

                    item.JoinPateDouble = item.YingCanJia == 0 ? 0 : Math.Round(Convert.ToDecimal(item.OnLineTestNum) / Convert.ToDecimal(item.YingCanJia), 4, MidpointRounding.AwayFromZero);
                    item.ExamPassPateDouble = item.OnLineTestNum == 0 ? 0 : Math.Round((Convert.ToDecimal(item.IsPassNum) / Convert.ToDecimal(item.OnLineTestNum)), 4, MidpointRounding.AwayFromZero);
                    //    item.
                    newVedioList.Add(item);
                }
            }
            #endregion
            var a11 = "";
            foreach (var item in examlist.Select(p => p.UserID))
            {
                if (!listIDs.Contains(item.StringToInt32()))
                {
                    a11 += item;
                }
            }
            
            return newVedioList.Where(p => p.OnLineTestNum > 0).ToList();

        }

        public List<Report_OnLineTest> GetOnLineTestDetail(out int totalCount, int courseid, string where = "1=1", int startIndex = 0, int pageLength = int.MaxValue, string jsRenderSortField = " UserId desc")
        {
            List<Report_OnLineTest> list = fonlineDB.GetOnLineTestDetail(courseid, where, startIndex, pageLength, jsRenderSortField);
            totalCount = list.Count;
            //var examstudentList = _ExamDB.GetAllList<tbExamSendStudent>();

            var query = Query.And(new[] { Query.EQ("RelationID", courseid), Query.EQ("SourceType", 2) });
            var studentList = _ExamDB.GetAllList<tbExamSendStudent>(query);

          
            #region 其他形式、免修
            Co_Course courseModel = courseDb.GetCo_Course(courseid);
            //自动折算
            var config = new Sys_ParamConfigBL().GetList(" ConfigType = 63").Where(p => p.LastUpdateTime.Year == courseModel.YearPlan).FirstOrDefault();
            var FreeList = freeDB.GetFreeUserList("  Year=" + courseModel.YearPlan);
            if (!(config == null || config.ConfigValue == ""))
            {
                var configvalue = config.ConfigValue.Split(';');
                var tDate = courseModel.YearPlan + "-" + configvalue[0].Split(',')[0];
                var CPADate = courseModel.YearPlan + "-" + configvalue[1].Split(',')[0];
                var userList = freeDB.GetUserByDate(tDate, CPADate);
                FreeList.AddRange(userList.Select(p => p.ApplyUserID).Distinct());
            }
            #endregion
            foreach (var item in list)
            {
                //var examstudent = _ExamDB.GetExamSendStudentWithByCourseIdAndUserId(courseid, item.UserId, 2);
                if (item.istest == 0)
                {
                    item.ExamStatus = "1";
                    item.ExamScore = "N/A";
                    item.ExamNum = "N/A";
                }
                else
                {
                    var examstudent = studentList.Where(p => p.UserID == item.UserId).ToList().FirstOrDefault();
                    if (examstudent != null)
                    {
                        if (examstudent.DoExamStatus != 0)
                        {
                            item.ExamStatus = examstudent.DoExamStatus.ToString();
                            item.ExamScore = examstudent.StudentAnswerList.Sum(p => p.GetScore).ToString();
                            item.ExamNum = examstudent.TestTimes.ToString();
                        }
                        else
                        {
                            item.ExamStatus = "0";
                            item.ExamScore = "N/A";
                            item.ExamNum = "N/A";
                            // item.GetLength = 99999;
                        }
                        //item.IsPass = examstudent.IsPass;
                    }
                    else
                    {
                        item.ExamStatus = "0";
                        item.ExamScore = "N/A";
                        item.ExamNum = "N/A";
                        item.GetLength = 99999;
                        //item.IsPass = 0;
                    }
                    examstudent = null;
                }

                item.IsFree = FreeList.Count(p => p == item.UserId) == 0 ? 0 : 1;
            }

            return list;

        }


        /// <summary>
        /// 参与情况
        /// </summary>
        /// <param name="courseid"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<Report_OnLineTest> GetCanYu(int courseid, string where)
        {
            var fvedioNumber = cacheManager.Get("fVedio_JoinNumber", 1440, () => { return (new Report_VedioBL()).GetfVedioJoinNumber(); });

            var list = fonlineDB.GetCanYu(courseid, where);

            foreach (var item in list)
            {

                if (fvedioNumber == null)
                {
                    item.YingCanJia = 0;
                    item.LearnTimes = -1;
                    // item.allVedioTime = -1;

                }
                else
                {
                    var listnumber = fvedioNumber.Where(p => p.DeptID == item.DeptId && p.courseID == courseid);
                    item.YingCanJia = listnumber.Count() > 0 ? listnumber.FirstOrDefault().count : 0;

                }

            }

            return list;
        }


        public List<Report_OnLineTest> GetMonthDeptByCourseId(int courseid, string deptIDs)
        {
            return fonlineDB.GetMonthDeptByCourseId(courseid, deptIDs);
        }

        public List<Report_OnLineTest> GetMonthByCourseId(int courseid)
        {
            return fonlineDB.GetMonthByCourseId(courseid);
        }


    }
}
