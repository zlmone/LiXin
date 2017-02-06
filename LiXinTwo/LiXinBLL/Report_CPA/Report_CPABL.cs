using LiXinDataAccess.Report_CPA;
using LiXinModels;
using LiXinModels.Report_CPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinCommon;
using LiXinInterface.Report_CPA;
using LiXinModels.Examination.DBModel;
using MongoDB.Driver.Builders;
using LiXinDataAccess.Examination;
using MongoDB.Bson;
using System.Diagnostics;
using System.Threading.Tasks;
using LiXinDataAccess.Report_All;
using LiXinDataAccess.SystemManage;
using LiXinBLL.SystemManage;

namespace LiXinBLL.Report_CPA
{
    public class Report_CPABL : IReport_CPA
    {
        private ReportCPADB ReportCPADB;
        private ExaminationDB examDB;
        private static Report_AllDB reportAll;
        public Report_CPABL()
        {
            ReportCPADB = new ReportCPADB();
            examDB = new ExaminationDB();
            reportAll = new Report_AllDB();
        }

        #region CPA报表
        /// <summary>
        /// 获得执业CPA继续教育学时统计
        /// </summary>
        /// <param name="CPAConfig">CPA周期时间</param>
        /// <param name="deptMaxScore">部门最大获取学时</param>
        /// <returns></returns>
        public List<ReportCPA> GetCPAList(int year, int OtherID, int FreeID, Sys_ParamConfig CPAConfig, int deptMaxScore, string where = "1=1", string timeWhere = " 1=1")
        {
            var list = CPAStartAndEnd(CPAConfig, year);
            var listYear = list.Select(p => p.Year).Distinct();
            var yearStr = string.Join(",", listYear);
            var scoreList = ReportCPADB.GetCPAScoreList(year, list[0].ToString(), list[1].ToString(), where, timeWhere, deptMaxScore, yearStr);
            var courseList = ReportCPADB.GetCourseIDInYear(timeWhere);
            var query = Query.And(Query.In("SourceType", new BsonArray(new List<int>() { 1, 2, 5 })), Query.EQ("Number", 1));

            var passlist = examDB.GetAllList<tbExamSendStudent>(query, 1, 10, "UserID", "RelationID");

            #region 其他形式、免修

            //自动折算
            var freeConfigList = new Sys_ParamConfigBL().GetList(" ConfigType = 63").Where(p => listYear.Contains(p.LastUpdateTime.Year)).ToList();

            foreach (var user in scoreList)
            {
                foreach (var freeconfig in freeConfigList)
                {
                    if (!(freeconfig == null || freeconfig.ConfigValue == ""))
                    {
                        var configvalue = freeconfig.ConfigValue.Split(';');
                        var CPADate = freeconfig.LastUpdateTime.Year + "-" + configvalue[1].Split(',')[0];
                        var cpascore = Convert.ToDecimal(configvalue[1].Split(',')[1]);
                        if (user.CPACreateDate >= Convert.ToDateTime(CPADate))
                        {
                            user.SumZhouqiScore = user.SumZhouqiScore + cpascore;
                            if (freeconfig.LastUpdateTime.Year == year)
                            {
                                user.FreeScore = cpascore;
                            }
                            user.FreeOut = 1;
                        }
                    }
                }
            }
            if (FreeID == 0)
            {
                scoreList = scoreList.Where(p => p.FreeOut == 1).ToList();
            }

            string freeWhere = "1=1";
            if (OtherID != -1)
            {
                freeWhere += " and ((ApplyType=1 or ApplyType=3) ";
                if (OtherID == 0)
                {
                    freeWhere += " and typeForm=2";
                }
                else if (OtherID == 999)
                {
                    freeWhere += " and typeForm=3";
                }
                else
                {
                    freeWhere += "and ApplyID=" + OtherID;
                }

                freeWhere += ")";
            }
            if (FreeID != -1)
            {
                if (FreeID > 0)
                {
                    if (OtherID != -1)
                    {
                        freeWhere += " or";
                    }
                    else
                    {
                        freeWhere += " and ";
                    }
                    freeWhere += " (ApplyType=2 and ApplyID=" + FreeID + ")";
                }
            }


            //其他形式、免修
            var FreeList = ReportCPADB.GetFreeUserApply(string.Format(" And Year in ({0}) and {1}", string.Join(",", listYear), freeWhere));

            //其他有组织形式
            var OrgList = ReportCPADB.GetOrgList(string.Format(" And Year in ({0})", string.Join(",", listYear)));

            var other = FreeList.Where(p => p.ApplyType == 1 || p.ApplyType == 3);
            var free = FreeList.Where(p => p.ApplyType == 2);

            #endregion

            foreach (var user in scoreList)
            {

                var pass = passlist.Where(p => p.UserID == user.UserId && courseList.Contains(p.RelationID));

                user.dScore = user.dScore > deptMaxScore && deptMaxScore != -1 ? deptMaxScore : user.dScore;
                user.passNumber = pass.Count() > 0 ? pass.Count() : 0;

                #region 其他形式、免修
                var otherAll = other.Where(p => p.ApplyUserID == user.UserId);
                var freeAll = free.Where(p => p.ApplyUserID == user.UserId);
                var orgAll = OrgList.Where(p => p.ApplyUserID == user.UserId);

                //查询年度的数据
                var otherSingle = otherAll.Where(p => p.Year == year);
                var freeSingle = freeAll.Where(p => p.Year == year);
                var orgSingle = orgAll.Where(p => p.Year == year);

                user.OtherProject = string.Join(";", otherSingle.Select(p => p.ApplyContent));
                user.OtherScore = otherSingle.Sum(p => p.GetCPAScore);
                user.FreeProject = (user.FreeOut == 1 ? "自动折算;" : "") + string.Join(";", freeSingle.Select(p => p.ApplyContent));
                user.FreeScore = user.FreeScore + freeSingle.Sum(p => p.GetCPAScore);
                user.OtherProjectScore = orgSingle.Sum(p => p.GetCPAScore);
                user.SumZhouqiScore = user.SumZhouqiScore + otherAll.Sum(p => p.GetCPAScore) + freeAll.Sum(p => p.GetCPAScore) + user.OtherProjectScore;
                #endregion

            }
            if (OtherID != -1)
            {
                scoreList = scoreList.Where(p => p.OtherScore > 0).ToList();
            }
            if (FreeID > 0)
            {
                scoreList = scoreList.Where(p => p.FreeScore > 0).ToList();
            }
            return scoreList;
        }



        /// <summary>
        /// 执业CPA继续教育学时明细
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ReportCPADetails> GetCPADetailsList(int year, int deptMaxScore, string where = " 1=1", string yearwhere = " 1=1")
        {
            var cpaDetails = ReportCPADB.GetCPADetailsList(where, yearwhere);

            #region 其他有组织形式

            var freeconfig = new Sys_ParamConfigBL().GetList(" ConfigType = 63 AND datename(year,LastUpdateTime)=" + year).FirstOrDefault();

            //其他形式、免修
            var FreeList = ReportCPADB.GetFreeUserApplyDetails(string.Format(" And fuo.Year in ({0})", year));

            //其他有组织形式
            var OrgList = ReportCPADB.GetOrgList(string.Format(" And Year in ({0})", year));

            var other = FreeList.Where(p => p.ApplyType == 1 || p.ApplyType == 3);
            var free = FreeList.Where(p => p.ApplyType == 2);

            #endregion

            var ReportDetailsList = new List<ReportCPADetails>();
            var listID = new List<int>();
            //去重之后的list
            var List = cpaDetails.Select(p => p.UserId).Distinct().ToList();
            for (int i = 0; i < List.Count(); i++)
            {
                var userID = Convert.ToInt32(List[i]);
                var itemList = cpaDetails.Where(p => p.UserId == userID);
                //其他形式、免修
                var otherAll = other.Where(p => p.ApplyUserID == userID);
                var freeAll = free.Where(p => p.ApplyUserID == userID);
                var orgAll = OrgList.Where(p => p.ApplyUserID == userID);


                var item = itemList.FirstOrDefault();
                var model = new ReportCPADetails();

                model.DeptName = item.DeptName;
                model.Realname = item.Realname;
                model.CPANo = item.CPANo;
                model.Username = item.Username;
                var listDetails = new List<CPADetails>();
                bool flag = true;

                #region 所内课程、注协课程

                foreach (var cpa in itemList.OrderBy(p => p.Way))
                {

                    ///部门自学
                    if (cpa.Way == 3)
                    {
                        if (flag)
                        {
                            var details = new CPADetails();
                            var dscore = itemList.Where(p => p.Way == 3).Sum(p => p.dScore);
                            details.courseType = cpa.courseType;
                            details.Way = cpa.Way;
                            details.CourseName = "";
                            details.StartTime = DateTime.MinValue;
                            details.EndTime = DateTime.MinValue;
                            // details.singleScore =  dscore <= deptMaxScore ? dscore : deptMaxScore;
                            details.singleScore = dscore > deptMaxScore && deptMaxScore != -1 ? deptMaxScore : dscore;
                            flag = false;
                            listDetails.Add(details);
                        }
                    }
                    else
                    {
                        var details = new CPADetails();
                        details.courseType = cpa.courseType;
                        details.Way = cpa.Way;
                        details.CourseName = string.IsNullOrEmpty(cpa.CourseName) ? "" : cpa.CourseName;
                        details.StartTime = cpa.StartTime;
                        details.EndTime = cpa.EndTime;
                        details.singleScore = cpa.SumScore;
                        listDetails.Add(details);
                    }

                    model.SumScore = cpa.SumScore;
                }

                #endregion

                #region 其他有组织形式
                foreach (var otherModel in otherAll)
                {
                    var details = new CPADetails();
                    details.courseType = 3;
                    //details.typeForm = otherModel.typeForm;
                    details.Way = -1;
                    details.CourseName = otherModel.ApplyContent;
                    details.StartTime = otherModel.ApplyTime;
                    details.EndTime = DateTime.MinValue;
                    details.singleScore = otherModel.GetCPAScore;
                    listDetails.Add(details);
                }

                foreach (var freeModel in freeAll)
                {
                    var details = new CPADetails();
                    details.courseType = 4;
                    // details.typeForm = freeModel.typeForm;
                    details.Way = -1;
                    details.CourseName = freeModel.ApplyContent;
                    details.StartTime = freeModel.ApplyTime;
                    details.EndTime = DateTime.MinValue;
                    details.singleScore = freeModel.GetCPAScore;
                    listDetails.Add(details);
                }

                foreach (var orgModel in orgAll)
                {
                    var details = new CPADetails();
                    details.courseType = 5;
                    // details.typeForm = orgModel.typeForm;
                    details.Way = -1;
                    details.CourseName = orgModel.ApplyContent;
                    details.StartTime = orgModel.StartTime;
                    details.EndTime = orgModel.EndTime;
                    details.singleScore = orgModel.GetCPAScore;
                    listDetails.Add(details);
                }
                if (!(freeconfig == null || freeconfig.ConfigValue == ""))
                {
                    var configvalue = freeconfig.ConfigValue.Split(';');
                    var CPADate = freeconfig.LastUpdateTime.Year + "-" + configvalue[1].Split(',')[0];
                    var cpascore = Convert.ToDecimal(configvalue[1].Split(',')[1]);
                    if (item.CPACreateDate >= Convert.ToDateTime(CPADate))
                    {
                        var details = new CPADetails();
                        details.courseType = 4;
                        // details.typeForm = freeModel.typeForm;
                        details.Way = -1;
                        details.CourseName = "自动折算";
                        //details.StartTime = freeModel.ApplyTime;
                        //details.EndTime = DateTime.MinValue;
                        details.singleScore = cpascore;
                        listDetails.Add(details);
                    }
                }
                #endregion

                model.CPADetailsList = listDetails;
                model.rowspan = listDetails.Count;
                model.SumScore = listDetails.Sum(p => p.singleScore);
                ReportDetailsList.Add(model);
            }

            return ReportDetailsList;
        }

        #endregion

        #region 执业CPA继续教育目标完成情况
        /// <summary>
        /// 学员学时获取
        /// </summary>
        /// <param name="CPAConfig">CPA周期时间</param>
        /// <param name="deptMaxScore">部门最大获取学时</param>
        /// <param name="CPAYearSCore">CPA年度考核目标学时</param>
        /// <param name="CPAzhouqiScore">CPA培训周期考核目标学时</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<CPACompelete> GetUserCpaScore(out bool IsLast, Sys_ParamConfig CPAConfig, int deptMaxScore, int CPAYearSCore, int CPAzhouqiScore, int year, List<Sys_ParamConfig> freeconfig, string where = "1=1", string timeWhere = "1=1")
        {
            var searchyear = new List<int>();
            //周期
            var list = CPAStartAndEnd(CPAConfig, year);
            //周期的年
            var yearList = list.Select(p => p.Year);

            //当前时间是否为周期的最后一年
            IsLast = yearList.LastOrDefault() == year;

            var yearstr = IsLast ? string.Join(",", yearList) : year.ToString();

            searchyear = IsLast ? yearList.ToList() : new List<int>() { year };

            var flag = IsLast;

            // 周期内 每个学员获得的学时以及对应的年
            var zhouqiScore = ReportCPADB.GetCPAzhouqiUser(list[0].ToString(), list[1].ToString(), string.Join(",", yearList));

            //学员学时获取
            var userscore = ReportCPADB.GetUserCpaScore(where, timeWhere);

            #region 其他形式、免修

            var freeScoreList = reportAll.GetFreeOtherScore(yearstr);


            #endregion

            #region 判断今年年度目标学时完成
            //判断年度是否
            foreach (var item in userscore)
            // Parallel.ForEach(userscore,item=>
            {
                var isfree = false;
                decimal sumScore = 0;
                item.dScore = item.dScore > deptMaxScore && deptMaxScore != -1 ? deptMaxScore : item.dScore;

                var freeCPAScore = freeScoreList.Where(p => p.ApplyUserID == item.UserId && p.Year == year).Sum(p => p.GetCPAScore);

                var fconfig = freeconfig.Where(p => p.LastUpdateTime.Year == year).FirstOrDefault();

                if (!(fconfig == null || fconfig.ConfigValue == ""))
                {
                    var configvalue = fconfig.ConfigValue.Split(';');
                    var CPADate = year + "-" + configvalue[1].Split(',')[0];
                    var cpascore = Convert.ToDecimal(configvalue[1].Split(',')[1]);
                    if (item.CPACreateDate >= Convert.ToDateTime(CPADate) && item.CPA == "是")
                    {
                        isfree = true;
                        sumScore = sumScore + cpascore;
                    }
                }
                if (isfree || freeScoreList.Count(p => p.ApplyUserID == item.UserId && p.ApplyType == 2) > 0)
                {
                    item.IsFree = 1;
                }
                sumScore = sumScore + freeCPAScore + item.tScore + item.dScore;
                if (sumScore >= CPAYearSCore)
                {
                    item.IsyearComplete = 1;
                }
                else
                {
                    item.IsyearComplete = 0;
                }
            }

            #endregion

            #region 培训周期学时目标完成人数
            foreach (var item in userscore)
            {

                //最后一年才显示 所以周期最后一年才进行计算
                var listScore = zhouqiScore.Where(p => p.UserId == item.UserId);
                int count = 0;
                decimal sumscore = 0;
                var flagyear = false;
                var falgzhouqi = false;
                foreach (var model in listScore)
                {
                    decimal sumScore = 0;

                    var freeCPAScore = freeScoreList.Where(p => p.ApplyUserID == item.UserId && p.Year == model.YearPlan).Sum(p => p.GetCPAScore);

                    foreach (var intyear in searchyear)
                    {
                        var fconfig = freeconfig.Where(p => p.LastUpdateTime.Year == model.YearPlan).FirstOrDefault();

                        if (!(fconfig == null || fconfig.ConfigValue == ""))
                        {
                            var configvalue = fconfig.ConfigValue.Split(';');
                            var CPADate = model.YearPlan + "-" + configvalue[1].Split(',')[0];

                            var cpascore = Convert.ToDecimal(configvalue[1].Split(',')[1]);
                            if (Convert.ToDateTime(CPADate) >= item.JoinDate && item.CPA == "是")
                            {
                                sumScore = sumScore + cpascore;
                            }
                        }

                    }

                    sumScore = sumScore + freeCPAScore + item.tScore + item.dScore;
                    model.dScore = model.dScore > deptMaxScore && deptMaxScore != -1 ? deptMaxScore : model.dScore;
                    sumscore = sumscore + model.tScore + model.dScore;
                    if (model.tScore + model.dScore + sumScore >= CPAYearSCore)
                    {
                        count++;
                    }
                }
                //每年都通过
                if (count == list.Count())
                {
                    flagyear = true;
                }
                if (sumscore >= CPAzhouqiScore)
                {
                    falgzhouqi = true;
                }
                if (flagyear && falgzhouqi)
                {
                    item.IsZhouqiComplete = 1;
                }
                else
                {
                    item.IsZhouqiComplete = 0;
                }
            }
            #endregion

            //组装数据
            var cpaList = new List<CPACompelete>();

            foreach (var item in userscore)
            {
                if (cpaList.Count(p => p.deptName == item.deptName) == 0)
                {
                    var cpascore = new CPACompelete();
                    var singleMondel = userscore.Where(p => p.deptName == item.deptName).ToList();
                    var userIDs = singleMondel.Select(p => p.UserId).Distinct();
                    cpascore.deptName = item.deptName;
                    cpascore.IsMain = item.DeptPath.Contains("上海") ? 1 : 0; ;
                    cpascore.DeptIDs = string.Join(",", singleMondel.Select(p => p.DeptId).Distinct());
                    cpascore.joinCheckNumber = singleMondel.Where(p => p.IsFree == 0).Select(p => p.UserId).Distinct().Count();
                    cpascore.freeCheckNumber = singleMondel.Count(p => p.IsFree == 1);//免修 后期需要加上
                    cpascore.yearComplete = singleMondel.Count(p => p.IsFree == 0 && p.IsyearComplete == 1);
                    cpascore.zhouqiComplete = flag ? singleMondel.Count(p => p.IsFree == 0 && p.IsZhouqiComplete == 1) : 0;
                    cpascore.JoinDaodeNumber = zhouqiScore.Where(p => userIDs.Contains(p.UserId) && p.isJoinDaode > 0).Select(p => p.UserId).Distinct().Count();
                    cpascore.IsLast = flag ? 1 : 0;
                    cpaList.Add(cpascore);
                }
            }

            return cpaList;

        }
        #endregion

        #region 公共
        /// <summary>
        /// 根据配置文件和当前时间，获取CPA周期
        /// 如（2012年10月—2013年12月）
        /// </summary>
        /// <param name="yearConfig"></param>
        public List<DateTime> CPAStartAndEnd(Sys_ParamConfig CPAConfig, int year = -1)
        {
            var listYear = new List<int>();
            year = year == -1 ? DateTime.Now.Year : year;
            var now = year == -1 ? DateTime.Now : Convert.ToDateTime(year + "-01-01 0:00:00");
            int startYear = CPAConfig.LastUpdateTime.Year;
            var ArrayValue = CPAConfig.ConfigValue.Split(';');
            int cpavalue = ArrayValue[0].StringToInt32();
            int endYear = startYear + cpavalue;
            int month1 = ArrayValue[1].StringToInt32();
            int month2 = ArrayValue[2].StringToInt32();
            DateTime startTime = DateTime.Parse(startYear + "-" + month1 + "-01 0:00:00");
            DateTime endTime = DateTime.Parse(endYear + "-" + month2 + "-01 0:00:00").AddMonths(1).AddSeconds(-1);
            var cpaTime = new List<DateTime>();
            if (now >= startTime && now <= endTime)
            {
                //string linshi = startYear + "-" + month1 + "月—" + endYear + "年" + month2 + "月";

                cpaTime.Add(DateTime.Parse(startYear + "-" + month1 + "-01 0:00:00"));
                cpaTime.Add(Convert.ToDateTime(endYear + "-" + month2 + "-01 0:00:00").AddMonths(1).AddSeconds(-1));
            }
            else if (now > endTime)
            {

                cpaTime.Add(DateTime.Parse(year + "-" + month1 + "-01 0:00:00"));
                cpaTime.Add(Convert.ToDateTime(year + cpavalue + "-" + month2 + "-01 0:00:00").AddMonths(1).AddSeconds(-1));

            }
            return cpaTime;
        }

        /// <summary>
        /// 根据配置文件和当前时间，获取CPA周期
        /// 如（2012年10月—2013年12月）
        /// </summary>
        /// <param name="yearConfig"></param>
        public List<string> CPAStartAndEndStr(Sys_ParamConfig CPAConfig)
        {
            var listYear = new List<int>();
            int startYear = CPAConfig.LastUpdateTime.Year;
            var ArrayValue = CPAConfig.ConfigValue.Split(';');
            int cpavalue = ArrayValue[0].StringToInt32();
            int endYear = startYear + cpavalue;
            int month1 = ArrayValue[1].StringToInt32();
            int month2 = ArrayValue[2].StringToInt32();
            DateTime startTime = DateTime.Parse(startYear + "-" + month1 + "-01 0:00:00");
            DateTime endTime = DateTime.Parse(endYear + "-" + month2 + "-01 0:00:00").AddMonths(1).AddSeconds(-1);
            var cpaTime = new List<string>();
            if (DateTime.Now >= startTime && DateTime.Now <= endTime)
            {
                string linshi = startYear + "年" + "—" + endYear + "年";
                cpaTime.Add(linshi);
            }
            else if (DateTime.Now > endTime)
            {
                int yearcha = DateTime.Now.Year - startYear;
                int ss = yearcha / cpavalue;
                for (int i = 1; i <= ss; i++)
                {
                    var syear = listYear.Count == 0 ? startYear : listYear.Last() + 1;
                    var eyear = syear + cpavalue;
                    listYear.Add(eyear);
                    string linshi = syear + "年" + "—" + eyear + "年";
                    cpaTime.Add(linshi);
                }
            }
            return cpaTime;
        }



        /// <summary>
        /// 动态的取得年
        /// </summary>
        /// <returns></returns>
        public List<int> GetCourseYear()
        {
            return ReportCPADB.GetCourseYear();
        }
        /// <summary>
        ///查询执业CPA的职业CPA
        /// </summary>
        /// <returns></returns>
        public List<string> GetPayGrade(string where = "1=1", int type = 0)
        {
            return ReportCPADB.GetPayGrade(where, type);
        }

        /// <summary>
        /// 周期学时
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public List<dynamic> GetzhouqiAllScore(Sys_ParamConfig CPAConfig, int year, int score = -1)
        {
            var list = CPAStartAndEnd(CPAConfig, year);
            var flag = false;
            //当前时间是否为周期的最后一年
            if (year == list[1].Year)
            {
                flag = true;
            }
            var yearStr = string.Join(",", list.Select(p => p.Year));
            return ReportCPADB.GetzhouqiAllScore(yearStr, list[0].ToString(), list[1].ToString(), score);
        }

        /// <summary>
        /// CPA关系所在地
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<string> GetCPARelationship(string where = "1=1")
        {
            return ReportCPADB.GetCPARelationship(where);
        }

        /// <summary>
        /// 周期内完成道德培训的人员ID
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<int> GetzhouqiIsDaode(Sys_ParamConfig CPAConfig, int year)
        {
            var list = CPAStartAndEnd(CPAConfig, year);
            return ReportCPADB.GetzhouqiIsDaode(list[0].ToString(), list[1].ToString());
        }
        #endregion

    }
}
