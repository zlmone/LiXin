using LiXinModels.Report_CPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinInterface.Report_CPA;
using LiXinInterface.Report_AllData;
using System.Diagnostics;
using System.Data;

namespace LiXinControllers
{
    public partial class Report_zAttendceController : BaseController
    {
        private IReport_CPA CPABL;
        private IReport_AllData re_alldata;
        private static List<CPACompelete> sumList = new List<CPACompelete>();
        private static List<CPACompelete> fengList = new List<CPACompelete>();
        private static List<CPACompelete> List = new List<CPACompelete>();
        private static bool IsLast = false;

        public Report_zAttendceController(IReport_CPA _CPABL, IReport_AllData _re_alldata)
        {
            CPABL = _CPABL;
            re_alldata = _re_alldata;
        }


        #region 执业CPA继续教育目标完成情况
        /// <summary>
        /// 执业CPA继续教育目标完成情况
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserCpaScoreList(int type = 1, string deptids = "", string openLevel = "", string payGrade = "", string CPARelationship = "", int start = -1, string jsRenderSortField = " zhouqiNoComplete desc")
        {
            try
            {

                if (type == 0)
                {
                    if (Session["CPACompelte0"] != null)
                    {
                        Session.Remove("CPACompelte0");
                    }
                    Session["CPACompelte0"] = deptids + "㉿" + payGrade + "㉿" + openLevel + "㉿" + start + "㉿" + CPARelationship;
                }
                else
                {
                    if (Session["CPACompelte1"] != null)
                    {
                        Session.Remove("CPACompelte1");
                    }
                    Session["CPACompelte1"] = deptids + "㉿" + payGrade + "㉿" + openLevel + "㉿" + start + "㉿" + CPARelationship;
                }

                var where = "1=1";
                var timewhere = " 1=1";
                if (start != -1)
                {
                    timewhere += " and YearPlan=" + start;
                }
                if (!string.IsNullOrEmpty(deptids))
                {
                    where += " and DeptId in (" + deptids + ")";
                }
                if (!string.IsNullOrEmpty(openLevel))
                {
                    where += " AND TrainGrade IN (SELECT ID FROM dbo.F_SplitIDs('" + openLevel + "'))";
                }
                if (!string.IsNullOrEmpty(payGrade))
                {
                    where += " AND PayGrade IN (" + payGrade + ")";
                }
                //为分所 加上限制条件
                if (type == 1)
                {
                    where += " and DeptId in (" + string.Join(",", GetAllSubLeardDepartments()) + ")";
                }
                if (!string.IsNullOrEmpty(CPARelationship))
                {
                    where += " AND CPARelationship IN (" + CPARelationship.TrimStart(',').TrimEnd(',').Replace("'无'", "''") + ")";
                }



                //var IsLast = false;
                //部门自学最大学时
                var DeptMaxScoreConfig = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 14).ConfigValue;
                var deptMaxScore = DeptMaxScoreConfig.Split(';')[2].Split(',')[0].StringToInt32();
                //CPA年度考核目标学时
                var CpayearScore = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 16).ConfigValue.StringToInt32();
                //CPA培训周期考核目标学时
                var CpazhouqiScore = AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 17).ConfigValue.StringToInt32();

                var freeconfig = AllSystemConfigs.Where(p => p.ConfigType == 63).ToList();
                List = CPABL.GetUserCpaScore(out IsLast, AllSystemConfigs.FirstOrDefault(p => p.ConfigType == 8), deptMaxScore, CpayearScore, CpazhouqiScore,start,freeconfig, where, timewhere);

                //全所合计
                var all = GetObjectForCPA(List, 0, IsLast);

                //总所列表
                sumList = List.Where(p => p.IsMain == 1).ToList().SortListByField(jsRenderSortField);

                //分所列表
                fengList = List.Where(p => p.IsMain == 0).ToList().SortListByField(jsRenderSortField);

                //总所合计
                var sum = GetObjectForCPA(sumList, 1, IsLast);

                //分所合计
                var feng = GetObjectForCPA(fengList, 2, IsLast);

                return Json(new
                {
                    result = 1,
                    all = all,
                    sum = sum,
                    feng = feng,
                    sumList = sumList,
                    fengList = fengList.SortListByField(jsRenderSortField),
                    IsLast = IsLast
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<CPACompelete>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 取出执业CPA继续教育目标完成情况的集合 全部 总所 分所
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        public dynamic GetObjectForCPA(List<CPACompelete> List, int type = 0, bool IsLast = false)
        {
            var title = "";

            switch (type)
            {
                case 0:
                    title = "全所合计({0})个";
                    break;
                case 1:
                    title = "总所合计({0})个";
                    break;
                case 2:
                    title = "分所合计({0})个";
                    break;
            }
            if (List.Count > 0)
            {
                var allSum = new
                {
                    allNumer = string.Format(title, List.Count()),
                    alljoinCheckNumber = List.Sum(p => p.joinCheckNumber),
                    allfreeCheckNumber = List.Sum(p => p.freeCheckNumber),
                    allyearComplete = List.Sum(p => p.yearComplete),
                    allyearNoComplete = List.Sum(p => p.yearNoComplete),
                    allzhouqiComplete = List.Sum(p => p.zhouqiComplete),
                    allzhouqiNoComplete = List.Sum(p => p.zhouqiNoComplete),
                    allJoinDaodeNumber = List.Sum(p => p.JoinDaodeNumber),
                    allNoJoinDaodeNumber = List.Sum(p => p.NoJoinDaodeNumber),
                    type = type,
                    IsLast = IsLast ? 1 : 0
                };
                return allSum;
            }
            else
            {

                var allSum = new
                {
                    allNumer = "全所合计0个",
                    alljoinCheckNumber = 0,
                    allfreeCheckNumber = 0,
                    allyearComplete = 0,
                    allyearNoComplete = 0,
                    allzhouqiComplete = 0,
                    allzhouqiNoComplete = 0,
                    allJoinDaodeNumber = 0,
                    allNoJoinDaodeNumber = 0,
                    IsLast = IsLast ? 1 : 0
                };

                return allSum;
            }
        }

        /// <summary>
        /// 导出执业CPA继续教育目标完成情况
        /// </summary>
        public void outCPAScore()
        {
            var list = List;
            var all = GetObjectForCPA(List, 0, IsLast);

            //总所合计
            var sum = GetObjectForCPA(sumList, 1, IsLast);

            //分所合计
            var feng = GetObjectForCPA(fengList, 2, IsLast);

            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("对象名称", typeof(string));
            outTable.Columns.Add("参与考核人数", typeof(string));
            outTable.Columns.Add("免修人数", typeof(string));
            outTable.Columns.Add("年度目标学时完成人数", typeof(string));
            outTable.Columns.Add("年度目标学时未完成人数", typeof(string));
            if (IsLast)
            {
                outTable.Columns.Add("本培训周期学时目标完成人数", typeof(string));
                outTable.Columns.Add("本培训周期学时目标未完成人数", typeof(string));
            }
            outTable.Columns.Add("职业道德课程完成人数", typeof(string));
            outTable.Columns.Add("职业道德课程未完成人数", typeof(string));
            var index = 1;
            if (IsLast)
            {
                outTable.Rows.Add("", all.allNumer, all.alljoinCheckNumber, all.allfreeCheckNumber, all.allyearComplete, all.allyearNoComplete, all.allzhouqiComplete, all.allzhouqiNoComplete, all.allJoinDaodeNumber, all.allNoJoinDaodeNumber);
            }
            else
            {
                outTable.Rows.Add("", all.allNumer, all.alljoinCheckNumber, all.allfreeCheckNumber, all.allyearComplete, all.allyearNoComplete, all.allJoinDaodeNumber, all.allNoJoinDaodeNumber);
            }

            //  outTable.Rows.Add("", all.allNumer, all.allJoinRate, all.allJoinNumber, all.allSumNumber, all.allLearnNumber, all.allSumLookTime, all.allAvgLookTime, all.allLookNumbr);
            if (sumList.Count > 0)
            {
                if (IsLast)
                {
                    outTable.Rows.Add("", sum.allNumer, sum.alljoinCheckNumber, sum.allfreeCheckNumber, sum.allyearComplete, sum.allyearNoComplete, sum.allzhouqiComplete, sum.allzhouqiNoComplete, sum.allJoinDaodeNumber, sum.allNoJoinDaodeNumber);
                }
                else
                {
                    outTable.Rows.Add("", sum.allNumer, sum.alljoinCheckNumber, sum.allfreeCheckNumber, sum.allyearComplete, sum.allyearNoComplete, sum.allJoinDaodeNumber, sum.allNoJoinDaodeNumber);
                }
                foreach (var v in sumList)
                {
                    // outTable.Rows.Add(index, v.DeptName, v.JoinRate, v.JoinNumber, v.SumNumber, v.LearnNumber, v.SumLookTime, v.AvgLookTime, v.LookNumbr);
                    if (IsLast)
                    {
                        outTable.Rows.Add(index, v.deptName, v.joinCheckNumber, v.freeCheckNumber, v.yearComplete, v.yearNoComplete, v.zhouqiComplete, v.zhouqiNoComplete, v.JoinDaodeNumber, v.NoJoinDaodeNumber);
                    }
                    else
                    {
                        outTable.Rows.Add(index, v.deptName, v.joinCheckNumber, v.freeCheckNumber, v.yearComplete, v.yearNoComplete, v.JoinDaodeNumber, v.NoJoinDaodeNumber);
                    }
                    index++;
                }
            }
            if (fengList.Count > 0)
            {
                if (IsLast)
                {
                    outTable.Rows.Add("", feng.allNumer, feng.alljoinCheckNumber, feng.allfreeCheckNumber, feng.allyearComplete, feng.allyearNoComplete, feng.allzhouqiComplete, feng.allzhouqiNoComplete, feng.allJoinDaodeNumber, feng.allNoJoinDaodeNumber);
                }
                else
                {
                    outTable.Rows.Add("", feng.allNumer, feng.alljoinCheckNumber, feng.allfreeCheckNumber, feng.allyearComplete, feng.allyearNoComplete, feng.allJoinDaodeNumber, feng.allNoJoinDaodeNumber);
                }
                foreach (var v in fengList)
                {
                    // outTable.Rows.Add(index, v.DeptName, v.JoinRate, v.JoinNumber, v.SumNumber, v.LearnNumber, v.SumLookTime, v.AvgLookTime, v.LookNumbr);
                    if (IsLast)
                    {
                        outTable.Rows.Add(index, v.deptName, v.joinCheckNumber, v.freeCheckNumber, v.yearComplete, v.yearNoComplete, v.zhouqiComplete, v.zhouqiNoComplete, v.JoinDaodeNumber, v.NoJoinDaodeNumber);
                    }
                    else
                    {
                        outTable.Rows.Add(index, v.deptName, v.joinCheckNumber, v.freeCheckNumber, v.yearComplete, v.yearNoComplete, v.JoinDaodeNumber, v.NoJoinDaodeNumber);
                    }
                    index++;
                }
            }
            new Spreadsheet().Template("执业CPA继续教育目标完成情况", null, outTable, HttpContext, "执业CPA继续教育目标完成情况", "执业CPA继续教育目标完成情况");
        }
        #endregion
    }
}
