using System;
using System.Collections.Generic;
using System.Linq;
using LiXinModels;
using LiXinDataAccess.Report_All;
using LiXinDataAccess.User;
using LiXinModels.User;
using Retech.Cache;
using Retech.Core.Cache;
using System.Text.RegularExpressions;
using LiXinDataAccess.Examination;
using LiXinModels.Examination.DBModel;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using LiXinInterface.Report_AllData;
using LiXinBLL.SystemManage;
using LiXinModels.SystemManage;

namespace LiXinBLL.Report_AllData
{
    public class Report_AllData : IReport_AllData
    {
        private static Report_AllDB reportAll;
        private static UserDB userDB;
        private static ExaminationDB examDB;

        public Report_AllData()
        {
            reportAll = new Report_AllDB();
            userDB = new UserDB();
            examDB = new ExaminationDB();
        }

        /// <summary>
        /// 总表报数据处理
        /// </summary>
        /// <param name="sysConfig14">学时上限配置</param>
        /// <param name="sysConfig13">培训级别目标学时配置</param>
        /// <param name="sysConfig16">CPA目标学时配置</param>
        /// <param name="sysConfig27">在线测试通过次数</param>
        /// <param name="year">年度</param>
        /// <param name="naru">1 纳入 0 不纳入</param>
        /// <returns></returns>
        public List<Report_Dept_User> GetAllCourseReport(Sys_ParamConfig sysConfig14, Sys_ParamConfig sysConfig13, Sys_ParamConfig sysConfig16, Sys_ParamConfig sysConfig27, int year, int naru = 1)
        {
            var ReportData = new List<Report_Dept_User>();
            var freeconfig = new Sys_ParamConfigBL().GetList(" ConfigType = 63 AND datename(year,LastUpdateTime)=" + year).FirstOrDefault();
            try
            {
                #region 加载数据库数据
                List<Report_Dept_User> Report_CourseList = reportAll.GetReport_DeptList();//所有部门/分所
                var userlist = reportAll.GetReport_AllUserList();//所有的人员
                var user_courselist = reportAll.GetReport_CourseList().Where(p => p.YearPlan == year);//所有学习课程信息

                var query = Query.And(Query.In("SourceType", new BsonArray(new List<int>() { 1, 2 })), Query.EQ("Number", 1));
                var passlist = examDB.GetAllList<tbExamSendStudent>(query, 1, 10, "UserID", "RelationID");//所有通过考试的信息

                #region 其他形式、免修

                var tDate = "";
                var CPADate = "";
                decimal tscore = 0;
                decimal cpascore = 0;
                //所有免修的人员（包括自动折算的，typeForm==4）
                var freeList = new List<Report_User>();

                if (!(freeconfig == null || freeconfig.ConfigValue == ""))
                {
                    var configvalue = freeconfig.ConfigValue.Split(';');
                    tDate = year + "-" + configvalue[0].Split(',')[0];
                    CPADate = year + "-" + configvalue[1].Split(',')[0];
                    tscore = Convert.ToDecimal(configvalue[0].Split(',')[1]);
                    cpascore = Convert.ToDecimal(configvalue[1].Split(',')[1]);
                    freeList = reportAll.GetFreePassUserID(tDate, CPADate, " Year=" + year);
                }
               

                var freeScoreList = reportAll.GetFreeOtherScore(year.ToString());

                if (!(freeconfig == null || freeconfig.ConfigValue == ""))
                {
                    foreach (var item in freeList.Where(p => p.typeForm == 4))
                    {
                        var model = new Free_UserOtherApply();
                        model.ApplyUserID = item.UserId;
                        if (item.JoinDate >= Convert.ToDateTime(tDate))
                        {
                            model.GettScore = tscore;
                        }
                        if (item.CPACreateDate >= Convert.ToDateTime(CPADate) && item.CPA == "是")
                        {
                            model.GetCPAScore = cpascore;
                        }
                        freeScoreList.Add(model);
                    }
                }

                #endregion

                //整理数据
                foreach (var rc in Report_CourseList)
                {
                    rc.CompleteExamTimes = Convert.ToInt32(sysConfig27.ConfigValue);

                    #region 部门下面所有人员
                    var dept_userlist = userlist.Where(p => ("," + rc.childID + ",").IndexOf("," + p.DeptId + ",") >= 0).ToList();

                    if (dept_userlist.Count > 0)
                    {
                        foreach (var du in dept_userlist)
                        {
                            du.CoList = user_courselist.Where(u => u.UserId == du.UserId && u.YearPlan == year).ToList();
                            var feescore = freeScoreList.Where(p => p.ApplyUserID == du.UserId);
                            if (du.CoList.Count > 0)
                            {
                                #region 各部分实际学时计算

                                //集中
                                decimal sumT1 = 0;
                                decimal sumT2 = 0;
                                decimal sumT = 0;
                                List<Report_Course_show> tempT1 = du.CoList.Where(p => p.IsMust == 0 && p.courseFrom == 0 && p.Way == 1).ToList();//必修
                                sumT1 = Convert.ToDecimal(tempT1.Sum(p => p.GetScore));
                                List<Report_Course_show> tempT2 = du.CoList.Where(p => p.IsMust == 1 && p.courseFrom == 0 && p.Way == 1).ToList();//选修
                                sumT2 = Convert.ToDecimal(tempT2.Sum(p => p.GetScore));
                                du.CoScore = sumT1 + sumT2;

                                //视频
                                decimal sumV1 = 0;
                                decimal sumV2 = 0;
                                decimal sumV = 0;
                                List<Report_Course_show> tempV1 = du.CoList.Where(p => p.IsMust == 0 && p.courseFrom == 0 && p.CpaFlag == 0 && p.CPAIsPass == 1).ToList();//必修
                                sumV1 = Convert.ToDecimal(tempV1.Sum(p => p.CPAGetLength));
                                List<Report_Course_show> tempV2 = du.CoList.Where(p => p.IsMust == 1 && p.courseFrom == 0 && p.CpaFlag == 0 && p.CPAIsPass == 1).ToList();//选修
                                sumV2 = Convert.ToDecimal(tempV2.Sum(p => p.CPAGetLength));
                                du.voScore = sumV1 + sumV2;

                                //部门自学
                                decimal sumD1 = 0;
                                decimal sumD2 = 0;
                                decimal sumD = 0;
                                List<Report_Course_show> tempD1 = du.CoList.Where(p => p.IsMust == 0 && p.courseFrom == 1).ToList();//必修
                                sumD1 = Convert.ToDecimal(tempD1.Sum(p => (p.AttScore + p.EvlutionScore + p.ExamScore)));
                                List<Report_Course_show> tempD2 = du.CoList.Where(p => p.IsMust == 1 && p.courseFrom == 1).ToList();//选修
                                sumD2 = Convert.ToDecimal(tempD2.Sum(p => (p.AttScore + p.EvlutionScore + p.ExamScore)));
                                du.DepScore = sumD1 + sumD2;

                                //从CPA课程折算过来的学时(一期)
                                decimal sumCPA = 0;
                                List<Report_Course_show> tempCPA = du.CoList.Where(p => p.courseFrom == 0 && p.CpaFlag == 1).ToList();
                                sumCPA = Convert.ToDecimal(tempCPA.Sum(p => p.CPAGetLength));
                                du.CPAToScore = sumCPA;
                                #endregion

                                #region 有效学时计算

                                string mianstr = sysConfig14.ConfigValue;
                                string[] CoType = Regex.Split(mianstr, ";", RegexOptions.IgnoreCase);
                                //集中计算
                                if (!string.IsNullOrEmpty(CoType[0]))
                                {
                                    string[] ToType = Regex.Split(CoType[0], ",", RegexOptions.IgnoreCase);
                                    if (sumT1 > Convert.ToDecimal(ToType[1]) && Convert.ToInt32(ToType[1]) != -1)
                                    {
                                        sumT1 = Convert.ToDecimal(ToType[1]);
                                    }
                                    if (sumT2 > Convert.ToDecimal(ToType[2]) && Convert.ToInt32(ToType[2]) != -1)
                                    {
                                        sumT2 = Convert.ToDecimal(ToType[2]);
                                    }
                                    sumT = sumT1 + sumT2;
                                    if (sumT > Convert.ToDecimal(ToType[0]) && Convert.ToInt32(ToType[0]) != -1)
                                    {
                                        sumT = Convert.ToDecimal(ToType[0]);
                                    }
                                }
                                else
                                {
                                    sumT = sumT1 + sumT2;
                                }

                                //视频计算
                                if (!string.IsNullOrEmpty(CoType[1]))
                                {
                                    string[] VoType = Regex.Split(CoType[1], ",", RegexOptions.IgnoreCase);
                                    if (sumV1 > Convert.ToDecimal(VoType[1]) && Convert.ToInt32(VoType[1]) != -1)
                                    {
                                        sumV1 = Convert.ToDecimal(VoType[1]);
                                    }
                                    if (sumV2 > Convert.ToDecimal(VoType[2]) && Convert.ToInt32(VoType[2]) != -1)
                                    {
                                        sumV2 = Convert.ToDecimal(VoType[2]);
                                    }
                                    sumV = sumV1 + sumV2;
                                    if (sumV > Convert.ToDecimal(VoType[0]) && Convert.ToInt32(VoType[0]) != -1)
                                    {
                                        sumV = Convert.ToDecimal(VoType[0]);
                                    }
                                }
                                else
                                {
                                    sumV = sumV1 + sumV2;
                                }

                                //分所计算
                                if (!du.SubordinateSubstation.Contains("上海"))
                                {
                                    if (CoType.Length >= 4)
                                    {
                                        if (!string.IsNullOrEmpty(CoType[3]))
                                        {
                                            string[] DoType = Regex.Split(CoType[3], ",", RegexOptions.IgnoreCase);
                                            if (sumD1 > Convert.ToDecimal(DoType[1]) && Convert.ToInt32(DoType[1]) != -1)
                                            {
                                                sumD1 = Convert.ToDecimal(DoType[1]);
                                            }
                                            if (sumD2 > Convert.ToDecimal(DoType[2]) && Convert.ToInt32(DoType[2]) != -1)
                                            {
                                                sumD2 = Convert.ToDecimal(DoType[2]);
                                            }
                                            sumD = sumD1 + sumD2;
                                            if (sumD > Convert.ToDecimal(DoType[0]) && Convert.ToInt32(DoType[0]) != -1)
                                            {
                                                sumD = Convert.ToDecimal(DoType[0]);
                                            }
                                        }
                                        else
                                        {
                                            sumD = sumD1 + sumD2;
                                        }
                                    }
                                }
                                else//部门
                                {
                                    if (CoType.Length >= 3)
                                    {
                                        if (!string.IsNullOrEmpty(CoType[2]))
                                        {
                                            string[] DotType = Regex.Split(CoType[2], ",", RegexOptions.IgnoreCase);
                                            if (sumD1 > Convert.ToDecimal(DotType[1]) && Convert.ToInt32(DotType[1]) != -1)
                                            {
                                                sumD1 = Convert.ToDecimal(DotType[1]);
                                            }
                                            if (sumD2 > Convert.ToDecimal(DotType[2]) && Convert.ToInt32(DotType[2]) != -1)
                                            {
                                                sumD2 = Convert.ToDecimal(DotType[2]);
                                            }
                                            sumD = sumD1 + sumD2;
                                            if (sumD > Convert.ToDecimal(DotType[0]) && Convert.ToInt32(DotType[0]) != -1)
                                            {
                                                sumD = Convert.ToDecimal(DotType[0]);
                                            }
                                        }
                                        else
                                        {
                                            sumD = sumD1 + sumD2;
                                        }
                                    }
                                }
                                du.EffectScore = sumT + sumV + sumD + sumCPA;
                                #endregion

                                #region 在线测试次数
                                var pass = passlist.Where(p => p.UserID == du.UserId && du.CoList.Select(pp => pp.Id).Contains(p.RelationID));
                                du.passNumber = pass.Count() > 0 ? pass.Count() : 0;

                                #endregion

                                #region CPA实际学时计算
                                decimal sum1CPA = 0;
                                decimal sum2CPA = 0;
                                //一期CPA学时（即一期全部CPA学时+折算CPA学时）
                                List<Report_Course_show> temp1CPA = du.CoList.Where(p => p.courseFrom == 0 && p.CpaFlag != 0).ToList();
                                sum1CPA = Convert.ToDecimal(temp1CPA.Sum(p => p.CPAGetLength));
                                //二期CPA学时
                                List<Report_Course_show> temp2CPA = du.CoList.Where(p => p.courseFrom == 1 && p.CpaFlag == 0).ToList();
                                sum2CPA = Convert.ToDecimal(temp2CPA.Sum(p => p.CPAGetLength));
                                du.CPAScore = sum1CPA + sum2CPA;
                                #endregion

                                #region CPA有效学时计算
                                //部门学时上限
                                int depValue = Convert.ToInt32(mianstr.Split(';')[2].Split(',')[0]);
                                //分所学时上限
                                int deptValue = Convert.ToInt32(mianstr.Split(';')[3].Split(',')[0]);

                                if (du.SubordinateSubstation.Contains("上海") && depValue != -1 && sum2CPA > depValue)
                                {
                                    du.EffectCPAScore = sum1CPA + depValue;
                                }
                                else if (!du.SubordinateSubstation.Contains("上海") && deptValue != -1 && sum2CPA > deptValue)
                                {
                                    du.EffectCPAScore = sum1CPA + deptValue;
                                }
                                else
                                {
                                    du.EffectCPAScore = sum1CPA + sum2CPA;
                                }
                                #endregion

                                #region 其他形式、免修学时
                                du.EffectScore += feescore.Sum(p => p.GettScore);
                                du.EffectCPAScore += feescore.Sum(p => p.GetCPAScore);
                                du.FreeCPAScore = feescore.Sum(p => p.GetCPAScore);
                                du.IsFree = feescore.Any() ? 1 : 0;
                                #endregion

                            }
                            else
                            {
                                du.CoScore = 0;
                                du.voScore = 0;
                                du.DepScore = 0;
                                du.CPAToScore = 0;
                                du.passNumber = 0;
                                du.CPAScore = 0;
                                du.EffectScore += feescore.Sum(p => p.GettScore);
                                du.EffectCPAScore += feescore.Sum(p => p.GetCPAScore);
                                du.FreeCPAScore = feescore.Sum(p => p.GetCPAScore);
                            }
                            //年度所内目标学时
                            string config13 = sysConfig13.ConfigValue + ";";
                            string substr = "(?<=" + du.TrainGrade + "-).*?(?=;)";
                            string result13 = Regex.Match(config13, substr).Value == "" ? "0" : Regex.Match(config13, substr).Value;
                            du.TargetScore = Convert.ToDecimal(result13);
                            //年度CPA目标学时
                            string result16 = sysConfig16.ConfigValue == "" ? "0" : sysConfig16.ConfigValue;
                            du.TargetCPAScore = Convert.ToDecimal(result16);
                        }
                        rc.CoUserList = dept_userlist;

                    }
                    else
                    {
                        rc.CoUserList = new List<Sys_User>();
                    }
                    #endregion

                    rc.freeNumberList = freeList.Where(p => rc.childID.Split(',').ToList().Contains(p.DeptId.ToString())).ToList();
                }
                ReportData = Report_CourseList;
                #endregion
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                var Report_Dept_User = new Report_Dept_User();
                Report_Dept_User.DeptName = a;
                ReportData.Add(Report_Dept_User);
            }
            return ReportData;
        }
    }
}