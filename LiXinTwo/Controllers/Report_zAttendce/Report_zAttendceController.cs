using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinBLL.Report_zAttendce;
using LiXinBLL.User;
using LiXinCommon;
using System.Data;
using LiXinInterface.Report_zAttendce;
using LiXinModels;
using System.Diagnostics;
using System.Text.RegularExpressions;
using LiXinModels.Report_zAttendce;
using LiXinInterface.User;
using LiXinInterface.CourseLearn;
using LiXinBLL.CourseLearn;
using LiXinBLL.Examination;
using LiXinInterface.Examination;
using LiXinModels.User;
using LiXinModels.Examination.DBModel;
using Retech.Core.Cache;
using Retech.Cache;
using LiXinInterface.Report_CPA;
using LiXinBLL.Report_CPA;
using LiXinModels.CourseLearn;
using LiXinModels.Report_zVedio;



namespace LiXinControllers
{
    public partial class Report_zAttendceController : BaseController
    {
        private IzAttendce zaDb = new zAttendceBL();
        private ICourseOrder icourseorder = new CourseOrderBL();
        private IExamination iExamination = new ExaminationBL();
        private static List<zAttendce> attendALL = new List<zAttendce>();
        private static List<zAttendce> attendSum = new List<zAttendce>();
        private static List<zAttendce> attendFen = new List<zAttendce>();
        private static List<Cl_CourseOrder> orderList = new List<Cl_CourseOrder>();


        #region 富二代
        public JsonResult GetList(string deptids, string cpa, string level, string jsRenderSortField = " sumSum desc",
                                  string startTime = "", string PayGrade = "", int type = 0)
        {
            try
            {

                if (type == 0)
                {
                    if (Session["ReZAttendce0"] != null)
                    {
                        Session.Remove("ReZAttendce0");
                    }
                    Session["ReZAttendce0"] = deptids + "㉿" + PayGrade + "㉿" + level + "㉿" + startTime + "㉿" + cpa;
                }
                else
                {
                    if (Session["ReZAttendce1"] != null)
                    {
                        Session.Remove("ReZAttendce1");
                    }
                    Session["ReZAttendce1"] = deptids + "㉿" + PayGrade + "㉿" + level + "㉿" + startTime + "㉿" + cpa;
                }

                string where = "   AND su.IsDelete=0 AND su.Status=0 and su.UserType IN ('在职','见习','特批','聘用') ";
                string whereDate = "";
                string whereDateT = "";
                string whereDateL = "";
                string whereDeptName = " 1=1 ";
                string whereDeptID = " 1=1";
                string fenDeptID = " 1=1";
                if (level != "")
                {
                    //" AND TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs('" + openLevel + "'))";
                    where += " and su.TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs('" + level + "'))";
                }

                if (cpa != "2")
                {
                    if (cpa == "1")
                    {
                        where += " and CPA = '是'";
                    }
                    if (cpa == "0")
                    {
                        where += " and CPA = '否'";
                    }
                }
                if (startTime != "")
                {
                    // whereDate += " and Year(co.EndTime)=" + startTime;
                    //whereDateT +=
                    //    string.Format(
                    //        "and Year(da1.StartTime)>={0};and Year(da2.StartTime)>={0};and Year(da3.StartTime)>={0};and Year(da4.StartTime)>={0};and Year(dta1.StartTime)>={0};and Year(dta2.StartTime)>={0}; and Year(dta3.StartTime)>={0};and Year(dta4.StartTime)>={0}",
                    //        startTime);
                    whereDate += " and co.YearPlan=" + startTime;
                }

                if (!string.IsNullOrEmpty(PayGrade))
                {
                    where += " AND PayGrade IN (" + PayGrade + ")";

                }
                if (!string.IsNullOrEmpty(deptids))
                {
                    whereDeptName += " and a1.DepartmentId in (" + deptids + ")";
                }
                //分所
                if (type == 1)
                {
                    whereDeptID += " and a1.DepartmentId in (" + string.Join(",", GetAllSubLeardDepartments()) + ")";
                    fenDeptID += " and DepartmentId in (" + string.Join(",", GetAllSubLeardDepartments()) + ")";
                }
                attendALL = zaDb.GetAllAttendce(where, whereDate, whereDateT, whereDateL, whereDeptName, whereDeptID: whereDeptID);




                //全所合计
                var all = GetData(attendALL, 3);

                //总所列表
                attendSum = attendALL.Where(p => p.ZongIS == 1).ToList().SortListByField(jsRenderSortField);
                //分所列表
                attendFen = attendALL.Where(p => p.ZongIS == 0).ToList().SortListByField(jsRenderSortField);

                //总所合计
                var sum = GetData(attendSum, 1);

                //分所合计
                var feng = GetData(attendFen, 0);


                return Json(new
                    {
                        result = 1,
                        all = all,
                        sum = sum,
                        feng = feng,
                        sumList = attendSum,
                        fengList = attendFen
                    }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">所有的</param>
        /// <param name="type"></param>
        /// <param name="feng">分所</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public dynamic GetData(List<zAttendce> list, int type)
        {
            //获得总所
            var title = "";
            switch (type)
            {
                case 0:
                    title = "分所合计({0})个";
                    break;
                case 1:
                    title = "总所合计({0})个";
                    break;
                case 3:
                    title = "全所合计";
                    break;
            }

            if (list.Count > 0)
            {
                var attSum = list.Sum(p => p.AttSum2);
                var lateSum = list.Sum(p => p.LateSum2);
                var earlySum = list.Sum(p => p.EarlySum2);
                var lateAndEarly = list.Sum(p => p.LateAndEarlySum2);
                var allsum = new
                {
                    Att = attSum,
                    Late = lateSum,
                    Early = earlySum,
                    LateAndEarly = lateAndEarly,
                    type = type,
                    Title = string.Format(title, list.Count)
                };


                return allsum;
            }
            else
            {

                var allsum = new
                {
                    Att = 0,
                    Late = 0,
                    Early = 0,
                    LateAndEarly = 0,
                    type = type,
                    Title = "全所合计"
                };
                return allsum;
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="deptName"></param>
        /// <param name="cpa"></param>
        /// <param name="level"></param>
        /// <param name="jsRenderSortField"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public void OutAttendce(string deptName, int cpa, string level, string jsRenderSortField = "", string startTime = "",
                                string endTime = "", int type = 0)
        {
            //全所合计
            var all = GetData(attendALL, 3);


            //总所合计
            var sum = GetData(attendSum, 1);

            //分所合计
            var feng = GetData(attendFen, 0);

            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("对象名称", typeof(string));
            outTable.Columns.Add("缺勤人次", typeof(string));
            outTable.Columns.Add("迟到人次", typeof(string));
            outTable.Columns.Add("早退人次", typeof(string));
            outTable.Columns.Add("迟到并早退人次", typeof(string));

            outTable.Rows.Add("", all.Title, all.Att, all.Late, all.Early, all.LateAndEarly);
            if (attendSum.Count() > 0)
            {
                var index = 1;
                outTable.Rows.Add("", sum.Title, sum.Att, sum.Late, sum.Early, sum.LateAndEarly);
                foreach (var v in attendSum)
                {
                    outTable.Rows.Add(index, v.DeptName, v.AttSum2, v.LateSum2, v.EarlySum2, v.LateAndEarlySum2);
                    index++;
                }
            }
            if (attendFen.Count() > 0)
            {
                outTable.Rows.Add("", feng.Title, feng.Att, feng.Late, feng.Early, feng.LateAndEarly);
                var index = 1;
                foreach (var v in attendFen)
                {
                    outTable.Rows.Add(index, v.DeptName, v.AttSum2, v.LateSum2, v.EarlySum2, v.LateAndEarlySum2);
                    index++;
                }
            }
            var title = type == 0 ? "(总所)" : "(分所)";
            new Spreadsheet().Template("全年出勤情况" + title, null, outTable, HttpContext, "全年出勤情况" + title, "全年出勤情况" + title);
        }
        #endregion

        #region 完成情况明细表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReportType">ReportType  0:总所 1：部门/分所</param>
        /// <param name="Deptid">部门ID</param>
        /// <param name="deptlist"></param>
        /// <param name="year"></param>
        /// <param name="realname"></param>
        /// <param name="iscpa"></param>
        /// <param name="ispeixun"></param>
        /// <param name="iszhiye"></param>
        /// <param name="TrainGrade"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetCompletionDetial(int ReportType, string Deptid, string naru, string deptlist, string year, string realname, string iscpa, string ispeixun, string iszhiye, string TrainGrade, int pageIndex, int pageSize = 10, string PayGrade = "", string cpaRelationship = "", string jsRenderSortField = "", int isFree = -1)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            //if (ReportType == 0)
            //{
            //    if (Session["CompletionDetial0"] != null)
            //    {
            //        Session.Remove("CompletionDetial0");
            //    }
            //    Session["CompletionDetial0"] = Deptid + "㉿" + naru + "㉿" + deptlist + "㉿" + year + "㉿" + realname + "㉿" + iscpa + "㉿" + ispeixun + "㉿" + iszhiye + "㉿" + TrainGrade + "㉿" + pageIndex + "㉿" + PayGrade + "㉿" + cpaRelationship;
            //}
            //else
            //{

            //}

            #region 条件
            string sql = " 1=1";
            string timewhere = " 1=1";
            var narusql = "";
            if (naru == "1")
            {
                narusql += " and sys_user.UserId in (select UserId from View_CheckUser) ";
            }
            else if (naru == "0")
            {
                narusql += " and sys_user.UserId not in (select UserId from View_CheckUser) ";
            }
            if (ReportType == 0)
            {
                sql += narusql;
            }
            else
            {
                sql += " and DeptId in (" + string.Join(",", GetAllSubLeardDepartments()) + ") " + narusql;
            }

            if (!string.IsNullOrEmpty(Deptid))
            {
                sql += " and DeptId in (" + Deptid + ")";
            }

            if (!string.IsNullOrEmpty(deptlist))
            {
                if (deptlist.Substring(0, 1) == ",")
                {
                    deptlist = deptlist.Substring(1);
                }
                sql += " and deptid in (" + deptlist + ")";
            }
            if (!string.IsNullOrEmpty(year))
            {
                timewhere += " and yearplan=" + year;
            }

            if (!string.IsNullOrEmpty(realname))
            {
                sql += " and realname like '%" + realname + "%'";
            }

            if (!string.IsNullOrEmpty(iscpa))
            {

                if (iscpa == "1")
                    sql += " and sys_user.CPA='是'";
                else
                {
                    sql += " and sys_user.CPA='否'";
                }
            }


            //if (!string.IsNullOrEmpty(iszhiye))
            //{
            //    if (iszhiye == "1")
            //    {
            //        sql += " and ismust=" + iszhiye;
            //    }
            //    else
            //    {
            //        sql += " and ((ismust=0 OR ismust IS NULL) and cpa='是')";
            //    }

            //}



            if (!string.IsNullOrEmpty(TrainGrade))
            {
                sql += " and TrainGrade in (" + TrainGrade + ")";
            }

            if (!string.IsNullOrEmpty(PayGrade))
            {
                sql += " AND PayGrade IN (" + PayGrade + ")";
            }
            if (!string.IsNullOrEmpty(cpaRelationship))
            {
                sql += " AND CPARelationship IN (" + cpaRelationship + ")";
            }

            var list = icourseorder.GetCompletionDetial(sql, timewhere);
            #endregion

            #region 考勤次数的计算

            var attendlist = zaDb.GetUserAttend(year.StringToInt32());
            foreach (var v in list)
            {
                var ll = attendlist.Where(p => p.UserId == v.UserId).ToList();
                if (ll.Count > 0)
                {
                    var attend = ll.FirstOrDefault();
                    v.co_cd = attend.co_cd;
                    v.co_zt = attend.co_zt;
                    v.co_qq = attend.co_qq;
                    v.co_cd_zt = attend.co_cd_zt;


                    v.dep_cd = attend.dep_cd;
                    v.dep_zt = attend.dep_zt;
                    v.dep_qq = attend.dep_qq;
                    v.dep_cd_zt = attend.dep_cd_zt;
                }
            }
            #endregion
            ///获取考勤

            //s.Stop();
            //var aa = s.ElapsedMilliseconds;
            var level = AllSystemConfigs.Find(p => p.ConfigType == 13).ConfigValue;

            var cpa = AllSystemConfigs.Find(p => p.ConfigType == 16).ConfigValue;

            var cpa_zhouqi = AllSystemConfigs.Find(p => p.ConfigType == 17).ConfigValue;

            //  AllSystemConfigs.Find(p => p.ConfigType == 8)

            //部门上线
            decimal bumenlength = Convert.ToInt32(AllSystemConfigs.Find(p => p.ConfigType == 14).ConfigValue.Split(';')[2].Split(',')[0]);

            var zhouqidate = AllSystemConfigs.Find(p => p.ConfigType == 8);

            int yearplan = string.IsNullOrEmpty(year) ? DateTime.Now.Year : Convert.ToInt32(year);

            var cpazhouqiCPA = CPABL.GetzhouqiAllScore(zhouqidate, yearplan, Convert.ToInt32(bumenlength));

        

            Sys_ParamConfig configType14 = AllSystemConfigs.Where(p => p.ConfigType == 14).FirstOrDefault();
            Sys_ParamConfig configType13 = AllSystemConfigs.Where(p => p.ConfigType == 13).FirstOrDefault();
            Sys_ParamConfig configType16 = AllSystemConfigs.Where(p => p.ConfigType == 16).FirstOrDefault();
            Sys_ParamConfig configType27 = AllSystemConfigs.Where(p => p.ConfigType == 27).FirstOrDefault();
            var onlinetest_num = configType27.ConfigValue;
            //var ReportallData = cacheManager.Get<List<Report_Dept_User>>(("courseReport" + yearplan), () => { return null; });
            //ReportallData = ReportallData == null ? re_alldata.GetAllCourseReport(configType14, configType13, configType16, yearplan) : ReportallData;
            var ReportallData = cacheManager.Get<List<Report_Dept_User>>(("courseReport" + yearplan), 1440, () => { return re_alldata.GetAllCourseReport(configType14, configType13, configType16, configType27, yearplan, 0); });

            //if (isFree != -1)
            //{
            //    foreach (var item in ReportallData)
            //    {
            //        item.CoUserList = item.CoUserList.Where(p => p.IsFree == isFree).ToList();
            //    }
            //}
            var passDaodeList = CPABL.GetzhouqiIsDaode(zhouqidate, yearplan);
            //s.Reset();
            //s.Start();
            foreach (Cl_CourseOrder item in list)
            {

                var a = level.Split(';').Where(p => p.Split('-')[0] == item.TrainGrade).ToList();
                if (a.Count() != 0)
                {

                    var bb = ReportallData.Where(p => p.childID.Split(',').Contains(item.deptid.ToString())).ToList();

                    var cc = bb.Select(p => p.CoUserList.Where(q => q.UserId == item.UserId)).Sum(p => p.FirstOrDefault() == null ? 0 : bb.Select(ap => ap.CoUserList.Where(q => q.UserId == item.UserId)).Sum(ap => ap.FirstOrDefault().EffectScore));

                    var freeCPAScore = bb.Select(p => p.CoUserList.Where(q => q.UserId == item.UserId)).Sum(p => p.FirstOrDefault() == null ? 0 : bb.Select(ap => ap.CoUserList.Where(q => q.UserId == item.UserId)).Sum(ap => ap.FirstOrDefault().FreeCPAScore));
                   
                    item.suonei_t_d_cpaScore = cc;

                    item.OnlineTestPass = bb.Select(p => p.CoUserList.Where(q => q.UserId == item.UserId)).Sum(p => p.FirstOrDefault() == null ? 0 : bb.Select(ap => ap.CoUserList.Where(aq => aq.UserId == item.UserId)).Sum(ap => ap.FirstOrDefault().passNumber));

                    //cpa培训周期学时
                    var cpaList = cpazhouqiCPA.Where(p => p.UserID == item.UserId).FirstOrDefault();
                    //  item.cpazhouqi_length = bb.Select(p => p.CoUserList.Where(q => q.UserId == item.UserId)).Sum(p => p.FirstOrDefault() == null ? 0 : bb.Select(ap => ap.CoUserList.Where(aq => aq.UserId == item.UserId)).Sum(ap => ap.FirstOrDefault().EffectCPAScore));
                    item.cpazhouqi_length = cpaList == null ? freeCPAScore : cpaList.SumZhouqiScore + freeCPAScore;
                    //未完成CPA培训周期学时
                    item.nopass_cpazhouqi_length = (Convert.ToDecimal(cpa_zhouqi) - item.cpazhouqi_length) >= 0 ? (Convert.ToDecimal(cpa_zhouqi) - item.cpazhouqi_length) : 0;

                    item.nopass_t_dScore = (Convert.ToDecimal(a[0].Split('-')[1]) - item.suonei_t_d_cpaScore) >= 0 ? Convert.ToDecimal(a[0].Split('-')[1]) - item.suonei_t_d_cpaScore : 0;

                    item.ComPletionStatu = (item.suonei_t_d_cpaScore >= Convert.ToDecimal(a[0].Split('-')[1]) && item.OnlineTestPass >= Convert.ToInt32(onlinetest_num)) ? "完成" : "未完成";

                    item.cpa_t_d_cpaScore = item.tscore + (item.dScore > bumenlength && bumenlength != -1 ? bumenlength : item.dScore) + item.cpaScore + freeCPAScore;

                    item.nppass_cpaScore = (Convert.ToDecimal(cpa) - item.cpa_t_d_cpaScore) >= 0 ? (Convert.ToDecimal(cpa) - item.cpa_t_d_cpaScore) : 0;

                    if (item.iscpa == "是")
                    {
                        item.cpa_statu = item.cpa_t_d_cpaScore + "/" + item.nppass_cpaScore;
                        item.cpazhouqi_statu = item.cpazhouqi_length + "/" + item.nopass_cpazhouqi_length;
                    }
                    else
                    {
                        item.nppass_cpaScore = -1;
                        item.cpa_statu = "N/A"; 
                        item.cpazhouqi_statu = "N/A";
                    }

                    item.ismoral = passDaodeList.Count(p => p.StringToInt32() == item.UserId) > 0 ? "是" : "否";// item.IsMust == 1 ? "是" : "否";
                    item.isFree = bb.Count(p => p.freeNumberList.Count(q => q.UserId == item.UserId) > 0) > 0 ? 1 : 0;

                }
                ////item.nopass_t_dScore=
            }
            //s.Stop();
            //var aa3 = s.ElapsedMilliseconds;


            //if (!string.IsNullOrEmpty(ispeixun))
            //{
            //    list = list.Where(p => p.ComPletionStatu == ispeixun).ToList();
            //}
            if (ispeixun == "1")
            {
                list = list.Where(p => p.ComPletionStatu == "完成").ToList();
            }
            else if (ispeixun == "0")
            {
                list = list.Where(p => p.ComPletionStatu != "完成").ToList();
            }
            if (!string.IsNullOrEmpty(iszhiye))
            {
                if (iszhiye == "1")
                {
                    list = list.Where(p => p.ismoral == "是").ToList();
                }
                else
                {
                    list = list.Where(p => p.ismoral != "是").ToList();
                }

            }
            if (isFree != -1)
            {
                list = list.Where(p => p.isFree == isFree).ToList();
            }


            if (!string.IsNullOrEmpty(jsRenderSortField))
            {
                list = list.SortListByField(jsRenderSortField);
            }
            else
            {
                list = list.OrderByDescending(p => p.nppass_cpaScore)
                    .ThenByDescending(p => p.nopass_t_dScore)
                    .ThenByDescending(p => p.OnlineTestPass)
                    .ThenByDescending(p => p.sumZT)
                    .ToList();
            }

            orderList = list;

            int limit = list.Count(); ;
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                result = 1,
                dataList = list,
                recordCount = limit
            }, JsonRequestBehavior.AllowGet);
        }


        //导出execl
        public void GetCompletionDetialForReport(int ReportType, string Deptid, string deptlist, string year, string realname, string iscpa, string ispeixun, string iszhiye, string TrainGrade)
        {



            //if (!string.IsNullOrEmpty(ispeixun))
            //{
            //    orderList = orderList.Where(p => p.ComPletionStatu == ispeixun).ToList();
            //}

            ////int limit = list.Count(); ;
            ////list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();


            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("部门/分所", typeof(string));
            outTable.Columns.Add("登录名", typeof(string));
            outTable.Columns.Add("员工姓名", typeof(string));
            outTable.Columns.Add("培训级别", typeof(string));
            outTable.Columns.Add("培训目标是否完成（所内）", typeof(string));
            outTable.Columns.Add("已获取（所内）", typeof(string));
            outTable.Columns.Add("未完成目标学时（所内）", typeof(string));
            outTable.Columns.Add("已通过在线测试次数（所内）", typeof(string));
            outTable.Columns.Add("已完成（CPA）", typeof(string));
            outTable.Columns.Add("未完成（CPA）", typeof(string));

            var cpa_zhouqi = Report_CPABL.CPAStartAndEnd(AllSystemConfigs.Find(p => p.ConfigType == 8));
            if (DateTime.Now.Year == cpa_zhouqi[1].Year)
            {
                outTable.Columns.Add("本培训周期已完成（CPA）", typeof(string));
                outTable.Columns.Add("本培训周期未完成目标学时（CPA）", typeof(string));
              
            }

            outTable.Columns.Add("是否已完成职业道德（CPA）", typeof(string));
            outTable.Columns.Add("集中授课迟到次数", typeof(string));
            outTable.Columns.Add("集中授课缺勤次数", typeof(string));
            outTable.Columns.Add("集中授课早退次数", typeof(string));
            outTable.Columns.Add("集中授课迟到并早退次数", typeof(string));
            outTable.Columns.Add("部门学习迟到次数", typeof(string));
            outTable.Columns.Add("部门学习缺勤次数", typeof(string));
            outTable.Columns.Add("部门学习早退次数", typeof(string));
            outTable.Columns.Add("部门学习迟到并早退次数", typeof(string));
            var index = 1;

            foreach (var v in orderList)
            {
                if (DateTime.Now.Year == cpa_zhouqi[1].Year)
                {
                    outTable.Rows.Add(index,
                        v.DeptName,
                        v.Username,
                        v.realname,
                        v.TrainGrade,
                        v.ComPletionStatu,
                        v.suonei_t_d_cpaScore,
                        v.nopass_t_dScore,
                        v.OnlineTestPass,
                       v.iscpa == "否" ? "N/A" : (v.cpa_t_d_cpaScore).ToString(),
                        v.iscpa == "否" ? "N/A" : v.nppass_cpaScore.ToString(),
                         v.iscpa == "否" ? "N/A" : v.cpazhouqi_length.ToString(),
                         v.iscpa == "否" ? "N/A" : v.nopass_cpazhouqi_length.ToString(),

                        iscpa == "否" ? "N/A" : v.ismoral,
                        "'" + v.co_cd,
                        "'" + v.co_qq,
                        "'" + v.co_zt,
                        "'" + v.co_cd_zt,
                          "'" + v.dep_cd,
                        "'" + v.dep_qq,
                        "'" + v.dep_zt,
                        "'" + v.dep_cd_zt);
                }
                else
                {
                    outTable.Rows.Add(index,
                        v.DeptName,
                        v.Username,
                        v.realname,
                        v.TrainGrade,
                        v.ComPletionStatu,
                        v.suonei_t_d_cpaScore,
                        v.nopass_t_dScore,
                        v.OnlineTestPass,
                        v.iscpa == "否" ? "N/A" : (v.cpa_t_d_cpaScore).ToString(),
                        v.iscpa == "否" ? "N/A" : v.nppass_cpaScore.ToString(),
                        v.iscpa == "否" ? "N/A" : v.ismoral,
                         "'" + v.co_cd,
                        "'" + v.co_qq,
                        "'" + v.co_zt,
                        "'" + v.co_cd_zt,
                          "'" + v.dep_cd,
                        "'" + v.dep_qq,
                        "'" + v.dep_zt,
                        "'" + v.dep_cd_zt);
                }
                index++;
            }
            new Spreadsheet().Template("培训目标-完成情况明细表", null, outTable, HttpContext, "培训目标-完成情况明细表");

        }

        #endregion

        #region 选择部门
        /// <summary>
        /// 选择部门
        /// </summary>
        /// <param name="deptname"></param>
        /// <returns></returns>
        public JsonResult GetSelectDept(int deptid, string deptname, string naru, int ReportType, int cpa = -1, int pageIndex = 1, int pageSize = 15)
        {

            //var list = icourseorder.GetDepartment(deptname);

            string sql = " 1=1";


            if (ReportType == 0)
            {
                if (!string.IsNullOrEmpty(naru))
                {
                    sql += " and deptid in (select deptid from View_CheckUser) ";
                }
                if (cpa != -1)
                {
                    sql += " AND  sys_user.CPA='是'";
                }
            }
            else
            {

                if (!string.IsNullOrEmpty(naru))
                {
                    sql += " and DeptId in (" + string.Join(",", GetAllSubLeardDepartments()) + ") and deptid in (select deptid from View_CheckUser)";
                }
                else
                {
                    sql += " and DeptId in (" + string.Join(",", GetAllSubLeardDepartments()) + ")";
                }
                if (cpa != -1)
                {
                    sql += " AND   sys_user.CPA='是'";
                }

            }


            if (deptid != 0)
            {
                sql += " and deptid=" + deptid;
            }

            if (!string.IsNullOrEmpty(deptname))
            {
                sql += " and deptpath like '%" + deptname + "%'";
            }


            var list = icourseorder.GetDepartment(sql);

            string deptodlist = "";
            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    deptodlist += item.deptid + ",";
                }
            }
            int limit = list.Count();
            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                dataList = list,
                recordCount = limit,
                deptodlist = deptodlist != "" ? deptodlist.Substring(0, deptodlist.LastIndexOf(',')) : ""
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}
