using LiXinInterface;
using LiXinInterface.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinCommon;
using LiXinModels.SystemManage;

namespace LiXinControllers
{
    public partial class Report_FreeController : BaseController
    {
        private static List<Free_UserOtherApply> FreeList;
        #region

        public ActionResult Report_OtherOrganization_NoCPA()
        {
            ViewBag.allTrainGrade = AllTrainGrade;
            ViewBag.yearList = freeBL.GetOrgYearList();
            return View();
        }

        public ActionResult Report_OtherOrganization_Main()
        {

            return View();
        }

        public ActionResult Report_Free()
        {
            return View();
        }

        public ActionResult Report_Free_CPA()
        {
            ViewBag.allTrainGrade = AllTrainGrade;
            ViewBag.yearList = freeBL.GetYearList();
            return View();
        }

        public ActionResult Report_Free_NoCPA()
        {
            ViewBag.allTrainGrade = AllTrainGrade;
            ViewBag.yearList = freeBL.GetYearList();
            return View();
        }

        public ActionResult Repoet_Free_Choose()
        {
            return View();
        }

        public ActionResult Report_OtherOrganization()
        {
            ViewBag.allTrainGrade = AllTrainGrade;
            ViewBag.yearList = freeBL.GetOrgYearList();
            return View();
        }

        public ActionResult Report_OtherOrganization_Choose()
        {
            return View();
        }

        #endregion

        #region 免修

        public JsonResult GetReport_Free_CPA(int year, string realname, string applyids, string cpanum, string TrainGradeids, string deptids, int typeForm, int cpa, int pageIndex = 1, int pageSize = 10, string jsRenderSortField = " ID desc")
        {
            string sql = " 1=1";
            string where = "1=1";
            var flag = false;
            var listYear = new List<int>();
            #region 条件
            if (year != 0)
            {
                listYear.Add(year);
                sql += " and Free_UserOtherApply.Year=" + year;
            }
            else
            {
                listYear = freeBL.GetYearList();
            }
            if (!string.IsNullOrEmpty(realname))
            {
                sql += " and Sys_User.Realname like '%" + realname + "%'";
                where += " and Sys_User.Realname like '%" + realname + "%'";
            }

            if (!string.IsNullOrEmpty(applyids))
            {
                var array = applyids.Split(',');
                if (array.Contains("0"))
                {
                    flag = false;
                    sql += " and Free_UserOtherApply.ApplyID in (" + applyids + ")";
                }
                else
                {
                    flag = true;
                    sql += " and Free_UserOtherApply.ApplyID in (" + applyids + ")";
                }
                
            }

            if (!string.IsNullOrEmpty(cpanum))
            {
                sql += " and Sys_User.CPANo like '%" + cpanum + "%'";
                where += " and Sys_User.CPANo like '%" + cpanum + "%'";
            }

            if (!string.IsNullOrEmpty(TrainGradeids))
            {
                sql += "AND charindex(Sys_User.TrainGrade,'" + TrainGradeids + "')>0";
                where += "AND charindex(Sys_User.TrainGrade,'" + TrainGradeids + "')>0";
            }

            if (!string.IsNullOrEmpty(deptids))
            {
                sql += " and Sys_User.DeptId in( " + deptids + ")";
                where += " and Sys_User.DeptId in( " + deptids + ")";
            }

            if (typeForm != -1)
            {
                sql += " and Free_UserOtherApply.typeForm=" + typeForm;
            }

            if (cpa == 0)
            {
                sql += " and Sys_User.CPA='是' and (Free_ApplyConfig.ApplyType=1 or Free_ApplyConfig.ApplyType=2)";
                where += "  and Sys_User.CPA='是' ";
            }
            else
            {
                sql += " and (Free_ApplyConfig.ApplyType=0 or Free_ApplyConfig.ApplyType=2)";
                where += "  and (Sys_User.CPA='否' or Sys_User.CPA='是')";

            }

            sql += " order by Free_UserOtherApply.typeForm desc,Sys_User.DeptId asc,Sys_User.TrainGrade asc";

            #endregion


            var list = freeConfigBL.GetReport_Free_CPA(sql);

            if (!flag)
            {
                #region 自动折算
                var freeConfigList = AllSystemConfigs.Where(p => p.ConfigType == 63 && listYear.Contains(p.LastUpdateTime.Year));

                foreach (var freeConfig in freeConfigList)
                {
                    if (!(freeConfig == null || freeConfig.ConfigValue == ""))
                    {
                        var yearInt = freeConfig.LastUpdateTime.Year;
                        var joinflag = true;
                        var CPACreateDateflag = true;
                        var configvalue = freeConfig.ConfigValue.Split(';');
                        var tDate = yearInt + "-" + configvalue[0].Split(',')[0];
                        var tScore = configvalue[0].Split(',')[1];
                        var CPADate = yearInt + "-" + configvalue[1].Split(',')[0];
                        var CPAScore = configvalue[1].Split(',')[1];
                        var userList = reportFreeBL.GetUserByDate(tDate, CPADate, where);
                        foreach (var item in userList)
                        {
                            joinflag = true;
                            CPACreateDateflag = true;
                            item.ApplyName_New = "自动折算";
                            item.typeForm = 4;
                            if (item.JoinDate != null)
                            {
                                if (item.JoinDate >= Convert.ToDateTime(tDate))
                                {
                                    item.GettScore = Convert.ToDecimal(tScore);
                                }
                                else
                                {
                                    joinflag = false;
                                }
                            }
                            else
                            {
                                joinflag = false;
                            }
                            if (item.CPACreateDate != null)
                            {
                                if (item.CPACreateDate >= Convert.ToDateTime(CPADate))
                                {
                                    item.GetCPAScore = Convert.ToDecimal(CPAScore);
                                }
                                else
                                {
                                    CPACreateDateflag = false;
                                }
                            }
                            else
                            {
                                CPACreateDateflag = false;
                            }
                            //if (CPACreateDateflag || joinflag)
                            //{
                            if (cpa == 1)//非CPA
                            {
                                if (joinflag)
                                {
                                    list.Add(item);
                                }
                            }

                            if (cpa == 0)//CPA
                            {
                                if (CPACreateDateflag)
                                {
                                    list.Add(item);
                                }
                            }

                            //}
                        }
                    }
                }
                #endregion
            }                   
            if (!jsRenderSortField.Contains(","))
            {
                list = list.SortListByField(jsRenderSortField);
            }
            else
            {
                //typeForm desc,Sys_User.DeptId asc,Sys_User.TrainGrade asc
                list = list.OrderByDescending(p => p.typeForm).ThenBy(p => p.DeptId).ThenBy(p => p.TrainGrade).ToList();
            }
            if (typeForm != -1)
            {
                list = list.Where(p => p.typeForm == typeForm).ToList();
            }
            FreeList = list;

            int limit = list.Count();

            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                dataList = list,
                recordCount = limit
                //deptodlist = deptodlist != "" ? deptodlist.Substring(0, deptodlist.LastIndexOf(',')) : ""
            }, JsonRequestBehavior.AllowGet);
        }


        public void GetReport_Free_CPA_Report(string realname, string applyids, string cpanum, string TrainGradeids, string deptids, int typeForm, int cpa, string jsRenderSortField = " ID desc")
        {

            var list = FreeList;

            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("姓名", typeof(string));
            outTable.Columns.Add("登录名", typeof(string));
            outTable.Columns.Add("部门", typeof(string));
            outTable.Columns.Add("培训级别", typeof(string));
            if (cpa == 0)
            {
                outTable.Columns.Add("CPA编号", typeof(string));
            }
            outTable.Columns.Add("免修项目", typeof(string));
            if (cpa == 0)
            {
                outTable.Columns.Add("CPA免修学时数", typeof(string));
            }
            else
            {
                outTable.Columns.Add("所内免修学时数", typeof(string));
            }

            var index = 1;

            if (cpa == 0)
            {
                foreach (var v in list)
                {
                    outTable.Rows.Add(index, v.Realname, v.Username, v.DeptName, v.TrainGrade, "'" + v.CPANo, v.ApplyName_New,
                         v.GetCPAScore);
                    index++;
                }
                new Spreadsheet().Template("我所员工免修申报统计一览表(CPA)", null, outTable, HttpContext, "我所员工免修申报统计一览表(CPA)", "我所员工免修申报统计一览表(CPA)");
            }
            else
            {
                foreach (var v in list)
                {
                    outTable.Rows.Add(index, v.Realname, v.Username, v.DeptName, v.TrainGrade, v.ApplyName_New,
                                           v.GettScore);
                    index++;
                }
                new Spreadsheet().Template("我所员工免修申报统计一览表(非CPA)", null, outTable, HttpContext, "我所员工免修申报统计一览表(非CPA)", "我所员工免修申报统计一览表(非CPA)");
            }

        }


        public JsonResult GetFree_ApplyConfig(string applyname, int pageIndex = 1, int pageSize = 15)
        {
            string sql = " 1=1";
            int limit = 0;

            if (!string.IsNullOrEmpty(applyname))
            {
                sql += " and Free_ApplyConfig.FreeName like '%" + applyname + "%'";
            }

            var list = freeConfigBL.GetFreeApplyList(out limit,where: sql);

            var model = new Free_ApplyConfig();
            model.FreeName = "自动折算";
            model.ID = 0;
            list.Insert(0, model);

            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                dataList = list,
                recordCount = limit
            }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region 其他形式

        public JsonResult GetFree_OtherFroms(string fromname, int pageIndex = 1, int pageSize = 15)
        {
            string sql = " IsDelete=0 ";
            int limit = 0;


            if (!string.IsNullOrEmpty(fromname))
            {
                sql += " and Free_OtherFroms.FromName like '%" + fromname + "%'";
            }

            var list = free_OtherFromsBL.GetOtherFromsList(sql);
            limit = list.Count();

            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                dataList = list,
                recordCount = limit
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReport_Free_OtherFroms(int year, string realname, string applyids, string cpanum, string TrainGradeids, string deptids, int cpa, int pageIndex = 1, int pageSize = 10, string jsRenderSortField = "")
        {
            string sql = " 1=1";

            if (year != 0)
            {
                sql += " and Free_UserApplyOtherForm.Year=" + year;
            }

            if (!string.IsNullOrEmpty(realname))
            {
                sql += " and Sys_User.Realname like '%" + realname + "%'";
            }

            if (!string.IsNullOrEmpty(applyids))
            {
                //sql += " and Free_UserApplyOtherForm.OtherFromID  in (" + applyids + ")";
                sql += " and Free_UserApplyOtherForm.CourseName  like '%" + applyids + "%'";
            }

            if (!string.IsNullOrEmpty(cpanum))
            {
                sql += " and Sys_User.CPANo like '%" + cpanum + "%'";
            }

            if (!string.IsNullOrEmpty(TrainGradeids))
            {
                //sql += " and Sys_User.TrainGrade in( " + TrainGradeids + ")";
                sql += "AND charindex(Sys_User.TrainGrade,'" + TrainGradeids + "')>0";
            }

            if (!string.IsNullOrEmpty(deptids))
            {
                sql += " and Sys_User.DeptId in( " + deptids + ")";
            }

            if (cpa == 0)
            {
                //sql += " and Sys_User.CPA='是' order by Free_UserApplyOtherForm.GetCPAScore,Sys_User.DeptId,Sys_User.TrainGrade desc";

                sql += " and (Free_UserApplyOtherForm.OtherFromID=0 or Free_UserApplyOtherForm.OtherFromID=2) order by Free_UserApplyOtherForm.GetCPAScore desc,Sys_User.DeptId desc,Sys_User.TrainGrade desc";
            }
            else
            {
                //sql += " and Sys_User.CPA='否' order by Free_UserApplyOtherForm.GettScore,Sys_User.DeptId,Sys_User.TrainGrade desc";
                sql += " and (Free_UserApplyOtherForm.OtherFromID=1 or Free_UserApplyOtherForm.OtherFromID=2) order by Free_UserApplyOtherForm.GettScore desc,Sys_User.DeptId asc,Sys_User.TrainGrade asc";

            }


            var list = free_UserApplyOtherFormBL.GetReport_UserApplyOtherForm(sql);

            if (jsRenderSortField != "")
            {
                list = list.SortListByField(jsRenderSortField);
            }

            int limit = list.Count();

            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                dataList = list,
                recordCount = limit
                //deptodlist = deptodlist != "" ? deptodlist.Substring(0, deptodlist.LastIndexOf(',')) : ""
            }, JsonRequestBehavior.AllowGet);
        }


        public void GetReport_Free_OtherFroms_Report(int year, string realname, string applyids, string cpanum, string TrainGradeids, string deptids, int cpa, int pageIndex = 1, int pageSize = 10, string jsRenderSortField = " ID desc")
        {
            string sql = " 1=1";

            if (year != 0)
            {
                sql += " and Free_UserApplyOtherForm.Year=" + year;
            }
            if (!string.IsNullOrEmpty(realname))
            {
                sql += " and Sys_User.Realname like '%" + realname + "%'";
            }

            if (!string.IsNullOrEmpty(applyids))
            {
                //sql += " and Free_UserApplyOtherForm.OtherFromID  in (" + applyids + ")";
                sql += " and Free_UserApplyOtherForm.CourseName  like '%" + applyids + "%'";
            }

            if (!string.IsNullOrEmpty(cpanum))
            {
                sql += " and Sys_User.CPANo like '%" + cpanum + "%'";
            }

            if (!string.IsNullOrEmpty(TrainGradeids))
            {
                //sql += " and Sys_User.TrainGrade in( " + TrainGradeids + ")";
                sql += "AND charindex(Sys_User.TrainGrade,'" + TrainGradeids + "')>0";
            }

            if (!string.IsNullOrEmpty(deptids))
            {
                sql += " and Sys_User.DeptId in( " + deptids + ")";
            }

            if (cpa == 0)
            {
                //sql += " and Sys_User.CPA='是' order by Free_UserApplyOtherForm.GetCPAScore,Sys_User.DeptId,Sys_User.TrainGrade desc";

                sql += " and (Free_UserApplyOtherForm.OtherFromID=0 or Free_UserApplyOtherForm.OtherFromID=2) order by Free_UserApplyOtherForm.GetCPAScore desc,Sys_User.DeptId desc,Sys_User.TrainGrade desc";
            }
            else
            {
                //sql += " and Sys_User.CPA='否' order by Free_UserApplyOtherForm.GettScore,Sys_User.DeptId,Sys_User.TrainGrade desc";
                sql += " and (Free_UserApplyOtherForm.OtherFromID=1 or Free_UserApplyOtherForm.OtherFromID=2) order by Free_UserApplyOtherForm.GettScore desc,Sys_User.DeptId desc,Sys_User.TrainGrade desc";

            }

            var list = free_UserApplyOtherFormBL.GetReport_UserApplyOtherForm(sql).SortListByField(jsRenderSortField);


            DataTable outTable = new DataTable();
            outTable.Columns.Add("序号", typeof(string));
            outTable.Columns.Add("姓名", typeof(string));
            outTable.Columns.Add("登录名", typeof(string));
            outTable.Columns.Add("部门", typeof(string));
            outTable.Columns.Add("培训级别", typeof(string));
            if (cpa == 0)
            {
                outTable.Columns.Add("CPA编号", typeof(string));
            }
            outTable.Columns.Add("申请课程名", typeof(string));
            if (cpa == 0)
            {
                outTable.Columns.Add("CPA学时", typeof(string));
            }
            else
            {
                outTable.Columns.Add("所内学时", typeof(string));
            }

            var index = 1;

            if (cpa == 0)
            {
                foreach (var v in list)
                {

                    outTable.Rows.Add(index, v.Realname, v.Username, v.DeptName, v.TrainGrade, "'" + v.CPANo, v.CourseName,
                         v.GetCPAScore);
                    index++;

                }
                new Spreadsheet().Template("其他有组织形式申报统计一览表(CPA)", null, outTable, HttpContext, "其他有组织形式申报统计一览表(CPA)", "其他有组织形式申报统计一览表(CPA)");
            }
            else
            {
                foreach (var v in list)
                {
                    outTable.Rows.Add(index, v.Realname, v.Username, v.DeptName, v.TrainGrade, v.CourseName,
                                           v.GettScore);
                    index++;
                }
                new Spreadsheet().Template("其他有组织形式申报统计一览表(非CPA)", null, outTable, HttpContext, "其他有组织形式申报统计一览表(非CPA)", "其他有组织形式申报统计一览表(非CPA)");
            }
        }

        #endregion

    }
}
