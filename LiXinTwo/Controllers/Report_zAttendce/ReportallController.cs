using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinInterface.Report_CPA;
using LiXinBLL.Report_CPA;
using LiXinModels.Report_CPA;
using LiXinCommon;
using System.Data;
using LiXinModels;
using Retech.Cache;
using Retech.Core.Cache;
using LiXinModels.Report_model;
using LiXinModels.User;
using System.Text.RegularExpressions;
using LiXinBLL.Report_Vedio;

namespace LiXinControllers
{
    public partial class Report_zAttendceController : BaseController
    {
        ICacheManager cacheManager = new MemoryCacheManager();

        private static List<List<ReportShow>> OuttempList;

        #region 呈现
        /// <summary>
        /// 所内培训目标完成情况
        /// </summary>
        /// <param name="flg">0 时候为新进  1 为返回的时候 需要记住</param>
        /// <returns></returns>
        public ActionResult ReportAllData(int flg = 0)
        {
            List<string> allTrainGrade = AllTrainGrade;
            ViewBag.allTrainGrade = allTrainGrade;
            ViewBag.type = 0;
            //返回记住查询条件
            ViewBag.Deptids = "";
            ViewBag.PayGrade = "";
            ViewBag.TrainGrade = "";
            ViewBag.year = DateTime.Now.Year.ToString();
            ViewBag.isCPA = "";
            if (flg == 1)
            {
                if (Session["ReportAllData0"] != null)
                {
                    string sess = Session["ReportAllData0"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.Deptids = att[0];
                    ViewBag.PayGrade = att[1];
                    ViewBag.TrainGrade = att[2];
                    ViewBag.year = att[3] == "" ? DateTime.Now.Year.ToString() : att[3];
                    ViewBag.isCPA = att[4];
                }
            }
            return View();
        }

        /// <summary>
        /// 所内培训目标完成情况
        /// </summary>
        /// <returns></returns>
        public ActionResult FReportAllData(int flg = 0)
        {
            List<string> allTrainGrade = AllTrainGrade;
            ViewBag.allTrainGrade = allTrainGrade;
            ViewBag.type = 1;

            //返回记住查询条件
            ViewBag.Deptids = "";
            ViewBag.PayGrade = "";
            ViewBag.TrainGrade = "";
            ViewBag.year = DateTime.Now.Year.ToString();
            ViewBag.isCPA = "";
            if (flg == 1)
            {
                if (Session["ReportAllData1"] != null)
                {
                    string sess = Session["ReportAllData1"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.Deptids = att[0];
                    ViewBag.PayGrade = att[1];
                    ViewBag.TrainGrade = att[2];
                    ViewBag.year = att[3] == "" ? DateTime.Now.Year.ToString() : att[3];
                    ViewBag.isCPA = att[4];
                }
            }
            return View("ReportAllData");
        }
        #endregion

        #region 培训目标完成情况

        public JsonResult GetReportAllList(string deptids, string year, string openLevel, string isCPA, string PayGrade, string jsRenderSortField = " DepartmentId desc", int type = 1)
        {
            try
            {
                if (type == 0)
                {
                    if (Session["ReportAllData0"] != null)
                    {
                        Session.Remove("ReportAllData0");
                    }
                    Session["ReportAllData0"] = deptids + "㉿" + PayGrade + "㉿" + openLevel + "㉿" + year + "㉿" + isCPA;
                }
                else
                {
                    if (Session["ReportAllData1"] != null)
                    {
                        Session.Remove("ReportAllData1");
                    }
                    Session["ReportAllData1"] = deptids + "㉿" + PayGrade + "㉿" + openLevel + "㉿" + year + "㉿" + isCPA;
                }


                int yearplan = string.IsNullOrEmpty(year) ? DateTime.Now.Year : Convert.ToInt32(year);
                Sys_ParamConfig configType14 = AllSystemConfigs.Where(p => p.ConfigType == 14).FirstOrDefault();
                Sys_ParamConfig configType13 = AllSystemConfigs.Where(p => p.ConfigType == 13).FirstOrDefault();
                Sys_ParamConfig configType16 = AllSystemConfigs.Where(p => p.ConfigType == 16).FirstOrDefault();
                Sys_ParamConfig configType27 = AllSystemConfigs.Where(p => p.ConfigType == 27).FirstOrDefault();

                //免修的自动折算日期
                var freeConfig = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == yearplan).FirstOrDefault();

                var ReportallData = cacheManager.Get<List<Report_Dept_User>>(("courseReport" + yearplan), () => { return null; });
                ReportallData = ReportallData == null ? cacheManager.Get(("courseReport" + yearplan), 1440, () =>
                {
                    return re_alldata.GetAllCourseReport(configType14, configType13, configType16, configType27, yearplan);
                }) : ReportallData;

                var checkUser = (new Report_VedioBL()).GetCheckUserList();

                var deptIDlist = checkUser.Select(p => Convert.ToString(p.DeptId)).Distinct().ToList();

                #region 数据筛选

                if (checkUser.Count > 0)
                {
                    List<Report_Dept_User> templist = new List<Report_Dept_User>();

                    //  ReportallData = ReportallData.Where(p => deptids.Split(',').ToList().Contains(p.DepartmentId.ToString())).ToList();
                    foreach (var tempdata in ReportallData)
                    {
                        Report_Dept_User tempdep = new Report_Dept_User();
                        List<Sys_User> tempUserList = new List<Sys_User>();
                        foreach (var temp in tempdata.CoUserList)
                        {
                            if (deptIDlist.Contains(temp.DeptId.ToString()))
                            {
                                tempUserList.Add(temp);
                            }
                        }
                        if (tempUserList.Count > 0)
                        {
                            tempdep.DepartmentId = tempdata.DepartmentId;
                            tempdep.DeptName = tempdata.DeptName;
                            tempdep.DeptPath = tempdata.DeptPath;
                            tempdep.childID = tempdata.childID;
                            tempdep.deptFrom = tempdata.deptFrom;
                            tempdep.CoUserList = tempUserList;
                            tempdep.freeNumberList = tempdata.freeNumberList.Where(p => deptIDlist.Contains(p.DeptId.ToString())).ToList();
                            tempdep.CompleteExamTimes = tempdata.CompleteExamTimes;
                            templist.Add(tempdep);
                        }
                    }
                    ReportallData = templist;
                }


                var OrderDeptIDList = new List<int>();

                if (!string.IsNullOrEmpty(deptids))
                {
                    List<Report_Dept_User> templist = new List<Report_Dept_User>();

                    //  ReportallData = ReportallData.Where(p => deptids.Split(',').ToList().Contains(p.DepartmentId.ToString())).ToList();
                    foreach (var tempdata in ReportallData)
                    {
                        Report_Dept_User tempdep = new Report_Dept_User();
                        List<Sys_User> tempUserList = new List<Sys_User>();
                        foreach (var temp in tempdata.CoUserList)
                        {
                            if (deptids.Split(',').ToList().Contains(temp.DeptId.ToString()))
                            {
                                tempUserList.Add(temp);
                            }
                        }
                        if (tempUserList.Count > 0)
                        {
                            tempdep.DepartmentId = tempdata.DepartmentId;
                            tempdep.DeptName = tempdata.DeptName;
                            tempdep.DeptPath = tempdata.DeptPath;
                            tempdep.childID = tempdata.childID;
                            tempdep.deptFrom = tempdata.deptFrom;
                            tempdep.CoUserList = tempUserList;
                            tempdep.freeNumberList = tempdata.freeNumberList.Where(p => deptids.Split(',').ToList().Contains(p.DeptId.ToString())).ToList();
                            tempdep.CompleteExamTimes = tempdata.CompleteExamTimes;
                            templist.Add(tempdep);
                        }
                    }
                    ReportallData = templist;
                }

                if (type == 1)
                {
                    OrderDeptIDList = GetAllSubLeardDepartments();
                    ReportallData = ReportallData.Where(p => OrderDeptIDList.Contains(p.DepartmentId)).ToList();
                }
                // ReportallData = ReportallData.SortListByField(jsRenderSortField);
                if (!string.IsNullOrEmpty(openLevel))
                {
                    List<Report_Dept_User> templist = new List<Report_Dept_User>();

                    foreach (var tempdata in ReportallData)
                    {
                        Report_Dept_User tempdep = new Report_Dept_User();
                        List<Sys_User> tempUserList = new List<Sys_User>();
                        foreach (var temp in tempdata.CoUserList)
                        {
                            if (openLevel.ToLower().Contains(temp.TrainGrade.ToLower()))
                            {
                                tempUserList.Add(temp);
                            }
                        }
                        if (tempUserList.Count > 0)
                        {
                            tempdep.DepartmentId = tempdata.DepartmentId;
                            tempdep.DeptName = tempdata.DeptName;
                            tempdep.DeptPath = tempdata.DeptPath;
                            tempdep.childID = tempdata.childID;
                            tempdep.deptFrom = tempdata.deptFrom;
                            tempdep.CoUserList = tempUserList;
                            tempdep.freeNumberList = tempdata.freeNumberList.Where(p => openLevel.ToLower().Contains(p.TrainGrade.ToLower())).ToList();
                            tempdep.CompleteExamTimes = tempdata.CompleteExamTimes;
                            templist.Add(tempdep);
                        }
                    }
                    ReportallData = templist;
                }

                if (!string.IsNullOrEmpty(isCPA) && isCPA != "2")
                {
                    List<Report_Dept_User> temp1list = new List<Report_Dept_User>();
                    foreach (var temp1data in ReportallData)
                    {
                        Report_Dept_User temp1dep = new Report_Dept_User();
                        List<Sys_User> temp1UserList = new List<Sys_User>();
                        var temp1freeUser = new List<Report_User>();
                        foreach (var temp1 in temp1data.CoUserList)
                        {

                            if (isCPA == "1")
                            {
                                if (temp1.CPA == "是")
                                {
                                    temp1UserList.Add(temp1);
                                }
                            }
                            else
                            {
                                if (temp1.CPA == "否")
                                {
                                    temp1UserList.Add(temp1);
                                }
                            }

                        }
                        foreach (var temp1 in temp1data.freeNumberList)
                        {

                            if (isCPA == "1")
                            {
                                if (temp1.CPA == "是")
                                {
                                    temp1freeUser.Add(temp1);
                                }
                            }
                            else
                            {
                                if (temp1.CPA == "否")
                                {
                                    temp1freeUser.Add(temp1);
                                }
                            }

                        }
                        if (temp1UserList.Count > 0)
                        {

                            temp1dep.DepartmentId = temp1data.DepartmentId;
                            temp1dep.DeptName = temp1data.DeptName;
                            temp1dep.DeptPath = temp1data.DeptPath;
                            temp1dep.childID = temp1data.childID;
                            temp1dep.deptFrom = temp1data.deptFrom;
                            temp1dep.CoUserList = temp1UserList;
                            temp1dep.freeNumberList = temp1freeUser;
                            temp1dep.CompleteExamTimes = temp1data.CompleteExamTimes;
                            temp1list.Add(temp1dep);
                        }
                    }
                    ReportallData = temp1list;
                }

                if (!string.IsNullOrEmpty(PayGrade))
                {
                    List<Report_Dept_User> temp2list = new List<Report_Dept_User>();
                    foreach (var temp2data in ReportallData)
                    {
                        Report_Dept_User temp2dep = new Report_Dept_User();
                        List<Sys_User> temp2UserList = new List<Sys_User>();
                        foreach (var temp2 in temp2data.CoUserList)
                        {
                            if (PayGrade.Split(',').ToList().Contains(temp2.PayGrade == null ? "" : "'" + temp2.PayGrade.ToLower() + "'"))
                            {
                                temp2UserList.Add(temp2);
                            }
                        }
                        if (temp2UserList.Count > 0)
                        {
                            temp2dep.DepartmentId = temp2data.DepartmentId;
                            temp2dep.DeptName = temp2data.DeptName;
                            temp2dep.DeptPath = temp2data.DeptPath;
                            temp2dep.childID = temp2data.childID;
                            temp2dep.deptFrom = temp2data.deptFrom;
                            temp2dep.CoUserList = temp2UserList;
                            temp2dep.freeNumberList = temp2data.freeNumberList.Where(p => PayGrade.Split(',').ToList().Contains(p.PayGrade == null ? "" : "'" + p.PayGrade.ToLower() + "'")).ToList();
                            temp2dep.CompleteExamTimes = temp2data.CompleteExamTimes;
                            temp2list.Add(temp2dep);
                        }
                    }
                    ReportallData = temp2list;
                }
                #endregion

                #region 显示数据组装
                List<List<ReportShow>> outlist = new List<List<ReportShow>>();
                //全所合计
                List<ReportShow> alltitlelist = new List<ReportShow>();
                var alltitle = new ReportShow()
                {
                    DepId = 0,
                    Title = "全所合计",
                    numberSum = ReportallData.Sum(p => p.numberSum),
                    freeSum = ReportallData.Sum(p => p.freeSum),
                    goalYesSum = ReportallData.Sum(p => p.goalYesSum),
                    PeriodYesSum = ReportallData.Sum(p => p.PeriodYesSum),
                    PaperYseSum = ReportallData.Sum(p => p.PaperYseSum),
                    type = 0
                };
                alltitlelist.Add(alltitle);
                outlist.Add(alltitlelist);

                //总所合计
                List<ReportShow> deptitlelist = new List<ReportShow>();
                var deptitle = new ReportShow()
                {
                    DepId = 0,
                    Title = "总所合计（" + ReportallData.Where(pp => pp.deptFrom == 0 && pp.CoUserList.Count > 0).Count() + "个）",
                    numberSum = ReportallData.Where(pp => pp.deptFrom == 0).Sum(p => p.numberSum),
                    freeSum = ReportallData.Where(pp => pp.deptFrom == 0).Sum(p => p.freeSum),
                    goalYesSum = ReportallData.Where(pp => pp.deptFrom == 0).Sum(p => p.goalYesSum),
                    PeriodYesSum = ReportallData.Where(pp => pp.deptFrom == 0).Sum(p => p.PeriodYesSum),
                    PaperYseSum = ReportallData.Where(pp => pp.deptFrom == 0).Sum(p => p.PaperYseSum),
                    type = 1
                };
                deptitlelist.Add(deptitle);
                outlist.Add(deptitlelist);

                List<ReportShow> depdatalist = new List<ReportShow>();
                var depRedatalist = ReportallData.Where(pp => pp.deptFrom == 0 && pp.CoUserList.Count > 0).ToList().SortListByField(jsRenderSortField);
                foreach (var dr in depRedatalist)
                {
                    var depRedata = new ReportShow()
                    {
                        DepId = dr.DepartmentId,
                        Title = dr.DeptName,
                        numberSum = dr.numberSum,
                        freeSum = dr.freeSum,
                        goalYesSum = dr.goalYesSum,
                        PeriodYesSum = dr.PeriodYesSum,
                        PaperYseSum = dr.PaperYseSum,
                        DeptIDs = dr.childID,
                        type = 1
                    };
                    depdatalist.Add(depRedata);
                }
                outlist.Add(depdatalist);
                //分所合计
                List<ReportShow> depttitlelist = new List<ReportShow>();
                var depttitle = new ReportShow()
                {
                    DepId = 0,
                    Title = "分所合计（" + ReportallData.Where(pp => pp.deptFrom == 1 && pp.CoUserList.Count > 0).Count() + "个）",
                    numberSum = ReportallData.Where(pp => pp.deptFrom == 1).Sum(p => p.numberSum),
                    freeSum = ReportallData.Where(pp => pp.deptFrom == 1).Sum(p => p.freeSum),
                    goalYesSum = ReportallData.Where(pp => pp.deptFrom == 1).Sum(p => p.goalYesSum),
                    PeriodYesSum = ReportallData.Where(pp => pp.deptFrom == 1).Sum(p => p.PeriodYesSum),
                    PaperYseSum = ReportallData.Where(pp => pp.deptFrom == 1).Sum(p => p.PaperYseSum),

                    type = 2
                };
                depttitlelist.Add(depttitle);
                outlist.Add(depttitlelist);
                List<ReportShow> deptdatalist = new List<ReportShow>();
                var deptRedatalist = ReportallData.Where(pp => pp.deptFrom == 1 && pp.CoUserList.Count > 0).ToList().SortListByField(jsRenderSortField);
                foreach (var dtr in deptRedatalist)
                {
                    var deptRedata = new ReportShow()
                    {
                        DepId = dtr.DepartmentId,
                        Title = dtr.DeptName,
                        numberSum = dtr.numberSum,
                        freeSum = dtr.freeSum,
                        goalYesSum = dtr.goalYesSum,
                        PeriodYesSum = dtr.PeriodYesSum,
                        PaperYseSum = dtr.PaperYseSum,
                        DeptIDs = dtr.childID,
                        type = 2
                    };
                    deptdatalist.Add(deptRedata);
                }
                outlist.Add(deptdatalist);
                OuttempList = outlist;
                #endregion

                return Json(new
                {
                    result = 1,
                    alldata = alltitlelist,
                    depdata = deptitlelist,
                    depdataList = depdatalist,
                    deptdata = depttitlelist,
                    deptdataList = deptdatalist
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    alldata = new List<ReportShow>(),
                    depdata = new List<ReportShow>(),
                    depdataList = new List<ReportShow>(),
                    deptdata = new List<ReportShow>(),
                    deptdataList = new List<ReportShow>()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 导出

        public void Out_ReportAll()
        {

            var outTable = new DataTable();
            outTable.Columns.Add("对象名称", typeof(string));
            outTable.Columns.Add("参与考核人数", typeof(string));
            outTable.Columns.Add("免修人数", typeof(string));
            outTable.Columns.Add("所内培训目标完成人数", typeof(string));
            outTable.Columns.Add("所内培训目标未完成人数", typeof(string));
            outTable.Columns.Add("所内培训目标完成率", typeof(string));
            outTable.Columns.Add("目标学时完成人数", typeof(string));
            outTable.Columns.Add("目标学时未完成人数", typeof(string));
            outTable.Columns.Add("在线测试通过3次人数", typeof(string));
            outTable.Columns.Add("未通过人数", typeof(string));

            foreach (var tempList in OuttempList)
            {
                foreach (var temp in tempList)
                {
                    outTable.Rows.Add(temp.Title, temp.numberSum, temp.freeSum, temp.goalYesSum, temp.goalNoSum, temp.lengthRate, temp.PeriodYesSum, temp.PeriodNoSum, temp.PaperYseSum, temp.PaperNoSum);
                }
            }
            List<SheetModel> sheets = new List<SheetModel>();
            List<DataModel> datamodels = new List<DataModel>();
            DataModel datamodel = new DataModel();
            datamodel.datatype = Datatype.DataTable;
            datamodel.Dataseries = outTable;
            datamodels.Add(datamodel);

            SheetModel sheetmodel = new SheetModel();
            sheetmodel.SheetName = "所内培训目标完成情况";
            sheetmodel.DataModels = datamodels;
            sheets.Add(sheetmodel);

            new Spreadsheet().ExportExcel(sheets, HttpContext, "所内培训目标完成情况");
        }
        #endregion
    }
}
