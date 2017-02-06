using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LiXinControllers.Filter;
using System.Data;
using System.Text.RegularExpressions;
using LiXinCommon;
using System.Web;
using LiXinModels.DepPlanManage;
using LiXinModels.DepCourseManage;
using System.Text;
using LiXinModels.User;
using LiXinModels;

namespace LiXinControllers.DeptPlanManage
{
    public partial class DeptPlanManageController : BaseController
    {
        #region 呈现页面

        /// <summary>
        ///  年计划列表呈现
        /// </summary>
        public ViewResult YearPlan()
        {
            int deptid = CurrentUser == null ? 0 : CurrentUser.DeptId;
            //List<Dep_YearPlan> itemArray = Iyear.GetAllYear(deptid);

            //2013-10-08 修改左侧组织结构名分 是分所的显示分所 总所则显示总所
            ViewBag.DepOrBranch = CurrentUser.SubordinateSubstation.Contains("上海");

            List<int> itemArray = Iyear.GetYear();
            ViewBag.did = deptid;
            ViewData["StrYear"] = itemArray;
            Sys_ParamConfig CPAConfig = AllSystemConfigs.Where(p => p.ConfigType == 49).FirstOrDefault();
            var ArrayValue = CPAConfig.ConfigValue.Split(';');
            DateTime YstartTime = DateTime.Parse(DateTime.Now.Year.ToString() + "-" + ArrayValue[1] + "-" + ArrayValue[2] + " 0:00:00");
            DateTime YendTime = new DateTime();
            if (ArrayValue[3] == "2")
            {
                YendTime = DateTime.Parse(DateTime.Now.AddYears(1).Year.ToString() + "-" + ArrayValue[4] + "-" + ArrayValue[5] + " 0:00:00").AddDays(1);
            }
            else
            {
                YendTime = DateTime.Parse(DateTime.Now.Year.ToString() + "-" + ArrayValue[4] + "-" + ArrayValue[5] + " 0:00:00").AddDays(1);
            }
            if (DateTime.Now >= YstartTime && DateTime.Now < YendTime)
            {
                ViewBag.eidt = 1;
            }
            else
            {
                ViewBag.eidt = 0;
            }
            return View();
        }

        /// <summary>
        ///  年计划编辑页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult YearPlanAdd(int did)
        {
            //int deptid = CurrentUser == null ? 0 : CurrentUser.DeptId;
            List<int> years = Iyear.GetAllYearID(did);
            ViewBag.year = DateTime.Now.Year;
            ViewBag.did = did;
            ViewData["Years"] = years;
            return View();
        }

        /// <summary>
        ///  年计划编辑页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult YearPlanEdit(int id, int did)
        {
            //int deptid = CurrentUser == null ? 0 : CurrentUser.DeptId;
            ViewBag.did = did;
            Dep_YearPlan itemArray = Iyear.GetYearModel(id);
            ViewBag.trainGrade = trainBL.GetAllTrainGrade();
            if (itemArray.IsOpen == 1)//组织结构
            {
                string strWhere = " DepartmentId in (SELECT DeptId FROM Dep_LinkDepart WHERE YearId=" + id + ") ";
                ViewBag.Departs = depBL.GetAllList(strWhere);
            }
            ViewData["Yearmodel"] = itemArray;
            return View();
        }

        /// <summary>
        ///  年计划列表呈现
        /// </summary>
        public ViewResult YearPlanDetail(int id, int did)
        {
            ViewBag.Flag = 1;//部门分所详情
            BaseYearPlanDetail(id, did);
            return View();
        }
        /// <summary>
        ///  年计划列表呈现
        /// </summary>
        public ViewResult ManageYearPlanDetail(int lai, int id, int did)
        {
            ViewBag.Flag = 2;//事务所计划详情
            BaseYearPlanDetail(id, did);
            ViewBag.lai = lai;
            return View("YearPlanDetail");
        }

        /// <summary>
        ///  年计划列表呈现
        /// </summary>
        public ViewResult AppYearPlanDetail(int id, int did)
        {
            ViewBag.Flag = 3;//接受或拒绝年度计划详情
            BaseYearPlanDetail(id, did);
            return View("YearPlanDetail");
        }

        /// <summary>
        /// 年度计划详情
        /// </summary>
        private void BaseYearPlanDetail(int id, int did)
        {
            var itemArray = Iyear.GetYearModel(id);
            ViewBag.trainGrade = trainBL.GetAllTrainGrade();
            if (itemArray.IsOpen == 1)//组织结构
            {
                var strWhere = " DepartmentId in (SELECT DeptId FROM Dep_LinkDepart WHERE ApprovalFlag=1 and YearId=" + id + ") ";
                ViewBag.OKDeparts = depBL.GetAllList(strWhere);
                var strWhere1 = " DepartmentId in (SELECT DeptId FROM Dep_LinkDepart WHERE ApprovalFlag=2 and YearId=" + id + ") ";
                ViewBag.NODeparts = depBL.GetAllList(strWhere1);
                var strWhere2 = " DepartmentId in (SELECT DeptId FROM Dep_LinkDepart WHERE ApprovalFlag=0 and YearId=" + id + ") ";
                ViewBag.UnDeparts = depBL.GetAllList(strWhere2);
            }
            if (itemArray.IsOpen == 0)
            {
                itemArray.OpenType = 0;
                itemArray.OpenTypeStr = "单体上报";
            }
            else if (itemArray.IsOpen == 1 && itemArray.DeptId == did)
            {
                itemArray.OpenType = 1;
                itemArray.OpenTypeStr = "联合上报";
            }
            else
            {
                itemArray.OpenType = 2;
                itemArray.OpenTypeStr = "被联合上报";
            }
            ViewData["Yearmodel"] = itemArray;
        }

        /// <summary>
        ///  新增课程呈现
        /// </summary>
        public ViewResult AddYearCourse(string type, int ccid, string str)
        {
            ViewBag.year = Request.QueryString["year"] ?? DateTime.Now.Year.ToString();
            ViewBag.month = Request.QueryString["month"] ?? DateTime.Now.Month.ToString();
            ViewBag.trainGrade = trainBL.GetAllTrainGrade();
            Dep_YearPlanCourse model = new Dep_YearPlanCourse();
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace(" ", "+");
                string strr = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(str));
                strr = System.Web.HttpUtility.HtmlDecode(strr);
                string[] coList = strr.Split('♣');
                model = new Dep_YearPlanCourse
                {
                    CourseId = Convert.ToInt32(coList[0]),
                    CourseName = coList[1],
                    Code = coList[2],
                    Year = Convert.ToInt32(coList[3]),
                    Month = coList[4],
                    Day = Convert.ToInt32(coList[5]),
                    OpenLevel = coList[6],
                    IsMust = Convert.ToInt32(coList[7]),
                    Teacher = coList[8],
                    Realname = coList[9],
                    PayGrade = coList[10],
                    IsSystem = Convert.ToInt32(coList[11]),
                    IsCPA = Convert.ToInt32(coList[12]),
                    CourseLength = Convert.ToDecimal(coList[13])
                };
            }
            else
            {
                model.CourseId = 0;
                model.Code = "";
                if (!string.IsNullOrEmpty(type) && ccid > 0)
                {
                    model.CourseName = type;
                    model.IsSystem = ccid;
                }
                else
                {
                    model.CourseName = "";
                    model.IsSystem = 0;
                }
            }
            ViewData["model"] = model;
            return View();
        }

        public ActionResult SelCompetencyCourse()
        {
            return View();
        }

        /// <summary>
        /// 接受或拒绝联合上报
        /// </summary>
        public ViewResult YearLinkPlan()
        {
            int deptid = CurrentUser == null ? 0 : CurrentUser.DeptId;
            //List<Dep_YearPlan> itemArray = Iyear.GetAllYear(deptid);
            List<int> itemArray = Iyear.GetYear();
            ViewBag.did = deptid;
            ViewData["StrYear"] = itemArray;
            ViewBag.DepartList = GetAllSubDepartments();
            return View();
        }

        public ViewResult YearPlanMessage(int did)
        {
            ViewBag.did = did;
            return View();
        }
        #endregion

        #region 操作

        /// <summary>
        /// 获取所有年计划列表
        /// </summary>
        public JsonResult GetAllYearList(int did, int id, int isopen, int IsAll, string startTime, string endTime, int pageSize = 20, int pageIndex = 1)
        {
            int eidt = 0;
            Sys_ParamConfig CPAConfig = AllSystemConfigs.Where(p => p.ConfigType == 49).FirstOrDefault();
            var ArrayValue = CPAConfig.ConfigValue.Split(';');
            DateTime YstartTime = DateTime.Parse(DateTime.Now.Year.ToString() + "-" + ArrayValue[1] + "-" + ArrayValue[2] + " 0:00:00");
            DateTime YendTime = new DateTime();
            if (ArrayValue[3] == "2")
            {
                YendTime = DateTime.Parse(DateTime.Now.AddYears(1).Year.ToString() + "-" + ArrayValue[4] + "-" + ArrayValue[5] + " 0:00:00").AddDays(1);
            }
            else
            {
                YendTime = DateTime.Parse(DateTime.Now.Year.ToString() + "-" + ArrayValue[4] + "-" + ArrayValue[5] + " 0:00:00").AddDays(1);
            }
            if (DateTime.Now >= YstartTime && DateTime.Now < YendTime)
            {
                eidt = 1;
            }
            DateTime s = DateTime.Parse("1990-01-01");
            DateTime e = DateTime.Now;
            if (!string.IsNullOrEmpty(startTime))
                s = DateTime.Parse(startTime);
            if (!string.IsNullOrEmpty(endTime))
                e = DateTime.Parse(endTime).AddDays(1);
            string where = " 1=1";
            try
            {
                int totalCount = 0;
                string deptStr = string.Format("SELECT ID FROM dbo.f_GetDeptChildByDeptID({0})", did);

                if (IsAll == 0)
                {
                    deptStr = Convert.ToString(did);
                }
                if (id > 0)
                {
                    where += string.Format(" and temp.Year={2} and temp.LastUpdateTime >= '{0}' and  temp.LastUpdateTime <= '{1}' and temp.IsDelete=0", s, e, id);
                }
                else
                {
                    where += string.Format(" and temp.LastUpdateTime >= '{0}' and  temp.LastUpdateTime <= '{1}' and temp.IsDelete=0", s, e);
                }
                switch (isopen)
                {
                    case 0:
                        break;
                    case 1:
                        where += string.Format(" and temp.IsOpen=0 ");
                        break;
                    case 2:
                        where += string.Format(" and temp.IsOpen=1 and temp.linkdepID=0");
                        break;
                    case 3:
                        where += string.Format(" and temp.IsOpen=1 and temp.linkdepID>0");
                        break;
                }
                List<Dep_YearPlan> yearList = Iyear.GetAllYearList(deptStr, out totalCount, pageIndex, pageSize, where);
                foreach (var item in yearList)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.DeptName = item.DeptName.HtmlXssEncode();
                    item.linkDeptName = string.IsNullOrEmpty(item.linkDeptName) == true ? "——" : item.linkDeptName.HtmlXssEncode();
                    if (item.IsOpen == 0)
                    {
                        item.OpenTypeStr = "单体上报";
                    }
                    else if (item.IsOpen == 1 && item.linkdepID == 0)
                    {
                        item.OpenTypeStr = "联合上报";
                    }
                    else
                    {
                        item.OpenTypeStr = "被联合上报";
                    }

                    if (item.DeptId == did)
                    {
                        item.EditFlag = 1;
                    }
                    else
                    {
                        item.EditFlag = 0;
                    }
                    item.eidt = eidt;
                }
                return Json(new
                {
                    dataList = yearList,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据ID删除年计划
        /// </summary>
        [Filter.SystemLog("根据ID删除年计划", LogLevel.Info)]
        public JsonResult DeleteYear(string ids)
        {
            bool result = Iyear.DeleteYearModel(ids);
            if (result)
                return Json(new
                {
                    result = 1,
                    content = "删除成功"
                }, JsonRequestBehavior.AllowGet);
            else
                return Json(new
                {
                    result = 0,
                    content = "删除失败"
                }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据ID发布年计划
        /// </summary>
        [Filter.SystemLog("根据ID发布年计划", LogLevel.Info)]
        public JsonResult PublishYear(int id, int did)
        {
            if (Iyear.IsYearUser(id))
            {
                Dep_YearPlan Ymodel = Iyear.GetYearModel(id);
                if (Iyear.GetIsOrNoLinkDepart(did, Ymodel.Year).Count == 0)
                {
                    //Dep_YearPlan Ymodel = Iyear.GetYearModel(id);
                    Ymodel.PublishFlag = 1;
                    bool result = Iyear.UpdateYearByID(Ymodel);
                    if (Ymodel.IsOpen == 1)
                    {
                        Iyear.UpdateLinkandYearPlan(Ymodel.Year, Ymodel.Id);

                    }
                    Iyear.UpdateLinkDepart(Ymodel.Year, did);
                    if (result)
                        return Json(new
                        {
                            result = 1,
                            content = "发布成功"
                        }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new
                        {
                            result = 0,
                            content = "发布失败"
                        }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        result = 0,
                        content = "已存在被联合年度，发布失败"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    result = 0,
                    content = "发布失败,一些课程不存在授课讲师"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存年计划
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("保存年度计划", LogLevel.Info)]
        public JsonResult SubmitYear(int id, int did, int selyear, string courlist, string DelID, int Isopen, string DepIDs)
        {
            if (!string.IsNullOrEmpty(DelID) && id > 0)
            {
                DelID = DelID.Remove(DelID.LastIndexOf(","), 1);
                Iyear.DeleteCoursebyYear(id, DelID);
            }
            if (!string.IsNullOrEmpty(courlist))
            {
                courlist = courlist.Remove(courlist.LastIndexOf(","), 1);
                string[] course = Regex.Split(courlist, ",", RegexOptions.IgnoreCase);
                int yearid = 0;
                int publishFlag = 0;
                try
                {
                    if (id > 0)
                    {
                        yearid = id;
                        Dep_YearPlan yearmodel = Iyear.GetYearModel(id);
                        yearmodel.LastUpdateTime = DateTime.Now;
                        yearmodel.IsOpen = Isopen;
                        Iyear.UpdateYearByID(yearmodel);
                        publishFlag = yearmodel.PublishFlag;
                        if (publishFlag == 0)
                        {
                            Iyear.DeleteAllDepLinkbyYear(Convert.ToString(id));
                        }
                    }
                    else
                    {
                        yearid = Iyear.InsertYear(new Dep_YearPlan
                        {
                            LastUpdateTime = DateTime.Now,
                            Year = selyear,
                            UserID = CurrentUser == null ? 0 : CurrentUser.UserId,
                            PublishFlag = 0,
                            IsDelete = 0,
                            IsOpen = Isopen,
                            DeptId = did
                        });
                    }
                    if (yearid > 0)
                    {
                        if (Isopen == 1)
                        {
                            if (publishFlag == 0)
                            {
                                if (!string.IsNullOrEmpty(DepIDs))
                                {
                                    string[] depidlist = DepIDs.Split(',');
                                    for (int j = 0; j < depidlist.Length; j++)
                                    {
                                        Iyear.InsertDepLink(new Dep_LinkDepart
                                        {
                                            YearId = yearid,
                                            DeptId = Convert.ToInt32(depidlist[j]),
                                            ApprovalFlag = 0,
                                            ApprovalTime = DateTime.Now,
                                            Reason = ""
                                        });
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < course.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(course[i]))
                            {
                                course[i] = course[i].Replace(" ", "+");
                                course[i] = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(course[i]));
                                course[i] = System.Web.HttpUtility.HtmlDecode(course[i]);
                                string[] coList = course[i].Split('♣');
                                Dep_Course model = new Dep_Course
                                {
                                    CourseName = HttpUtility.UrlDecode(coList[1]),
                                    Code = HttpUtility.UrlDecode(coList[2]),
                                    Year = Convert.ToInt32(coList[3]),
                                    Month = coList[4],
                                    PreCourseTime = DateTime.Now,
                                    Day = Convert.ToInt32(coList[5]),
                                    OpenLevel = coList[6],
                                    IsCPA = Convert.ToInt32(coList[12]),
                                    IsMust = Convert.ToInt32(coList[7]),
                                    CourseLength = Convert.ToDecimal(coList[13]),
                                    Teacher = coList[8],
                                    SurveyPaperId = "0",
                                    CourseFrom = 0,
                                    Publishflag = 0,
                                    RoomId = 0,
                                    IsSystem = Convert.ToInt32(coList[11]),
                                    IsYearPlan = 1,
                                    DeptId = did //创建部门(部门开课跟踪统计时会用到)add by yxt
                                };
                                if (Convert.ToInt32(coList[0]) < 1)
                                {
                                    monthBL.InsertDept_Course(model);
                                    Iyear.InsertYearCourse(new Dep_YearPlanCourse
                                    {
                                        YearId = yearid,
                                        CourseId = model.Id,
                                        IsSystem = Convert.ToInt32(coList[11])
                                    });
                                }
                                else
                                {
                                    model.Id = Convert.ToInt32(coList[0]);
                                    Iyear.UpdateCo_Course(model);
                                }
                            }
                        }
                        return Json(new
                        {
                            result = 1,
                            content = "保存成功"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            result = 0,
                            content = "保存失败"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch
                {
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(DelID) && id > 0)
                {
                    return Json(new
                    {
                        result = 1,
                        content = "保存成功"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        result = 0,
                        content = "保存失败"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 根据ID获取所有课程列表(无分页)
        /// </summary>
        public JsonResult GetUpdataYCList(int yid, string month, string openLevel)
        {
            string where = "1=1";
            try
            {
                if (!string.IsNullOrEmpty(openLevel))
                {
                    // where += string.Format(" And cc.OpenLevel LIKE '%{0}%'", openlevel);
                    where += string.Format(" And (SELECT count(*) FROM  dbo.F_SplitIDs(OpenLevel)  WHERE ID  IN (SELECT ID FROM dbo.F_SplitIDs('{0}')) )>0", openLevel);
                }
                if (!string.IsNullOrEmpty(month))
                {
                    where += " and ( ";
                    if (month.IndexOf(",") > -1)
                    {
                        string[] months = Regex.Split(month, ",", RegexOptions.IgnoreCase);
                        for (int i = 0; i < months.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(months[i]))
                            {
                                string mm = "";
                                if (months[i].Length > 1)
                                {
                                    mm = "-" + months[i];
                                }
                                else
                                {
                                    mm = "-0" + months[i];
                                }
                                if (i == (months.Length - 1))
                                {
                                    where += "charindex('" + mm + "',Month)>0 )";
                                }
                                else
                                {
                                    where += "charindex('" + mm + "',Month)>0 or ";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (month.Length > 1)
                        {
                            month = "-" + month;
                        }
                        else
                        {
                            month = "-0" + month;
                        }
                        where += "charindex('" + month + "',Month)>0 ) ";
                    }

                }
                List<Dep_YearPlanCourse> yearList = Iyear.GetAllYearCourseList(yid, "asc", where);
                foreach (var item in yearList)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                }
                return Json(new
                {
                    dataList = yearList,
                    recordCount = yearList.Count
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据ID获取所有课程列表(有分页)
        /// </summary>
        public JsonResult GetYearCourseList(int yid, string name, string Teaname, string openLevel, string isMust, string isSystem, string Order, int pageSize = 20, int pageIndex = 1)
        {
            string where = "1=1";
            try
            {
                int totalCount = 0;
                if (!string.IsNullOrEmpty(openLevel))
                {
                    // where += string.Format(" And cc.OpenLevel LIKE '%{0}%'", openlevel);
                    where += string.Format(" And (SELECT count(*) FROM  dbo.F_SplitIDs(OpenLevel)  WHERE ID  IN (SELECT ID FROM dbo.F_SplitIDs('{0}')) )>0", openLevel);
                }
                if (!string.IsNullOrEmpty(isMust))
                {
                    where += string.Format(" and cc.IsMust ={0}", Convert.ToInt32(isMust));
                }
                if (!string.IsNullOrEmpty(isSystem))
                {
                    if (Convert.ToInt32(isSystem) == 0)
                    {
                        where += string.Format(" and tp.IsSystem=0 ");
                    }
                    else
                    {
                        where += string.Format(" and tp.IsSystem>0 ");
                    }
                }
                if (!string.IsNullOrEmpty(name))
                {
                    where += string.Format(" and cc.CourseName LIKE '%{0}%'", name.ReplaceSql());
                }
                if (!string.IsNullOrEmpty(Teaname))
                {
                    where += string.Format(" and su.Realname LIKE '%{0}%'", Teaname.ReplaceSql());
                }
                List<Dep_YearPlanCourse> yearList = Iyear.GetAllYearCourseList(yid, out totalCount, pageIndex, pageSize, Order, where);
                foreach (var item in yearList)
                {
                    item.CourseName = item.CourseName.HtmlXssEncode();
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.PayGrade = item.PayGrade.HtmlXssEncode();
                }
                return Json(new
                {
                    dataList = yearList,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导出年度计划课程信息
        /// </summary>
        public void OutYearCourse(int id, string name, string Teaname, string isMust, string isSystem, string openLevel, string Order)
        {
            Dep_YearPlan year = Iyear.GetYearModel(id);
            string where = "1=1";
            if (!string.IsNullOrEmpty(openLevel))
            {
                where += string.Format(" And (SELECT count(*) FROM  dbo.F_SplitIDs(OpenLevel)  WHERE ID  IN (SELECT ID FROM dbo.F_SplitIDs('{0}')) )>0", openLevel);
            }
            if (!string.IsNullOrEmpty(isMust))
            {
                where += string.Format(" and cc.IsMust ={0}", Convert.ToInt32(isMust));
            }
            if (!string.IsNullOrEmpty(isSystem))
            {
                if (Convert.ToInt32(isSystem) == 0)
                {
                    where += string.Format(" and tp.IsSystem=0 ");
                }
                else
                {
                    where += string.Format(" and tp.IsSystem>0 ");
                }
            }
            if (!string.IsNullOrEmpty(Teaname))
            {
                where += string.Format(" and su.Realname LIKE '%{0}%'", Teaname.ReplaceSql());
            }
            if (!string.IsNullOrEmpty(name))
            {
                where += string.Format(" and cc.CourseName LIKE '%{0}%'", name.ReplaceSql());
            }
            List<Dep_YearPlanCourse> yearList = Iyear.GetAllYearCourseList(id, Order, where);
            DataTable outTable = new DataTable();
            outTable.Columns.Add("课程名称", typeof(string));
            outTable.Columns.Add("学时", typeof(string));
            outTable.Columns.Add("预计开课时间", typeof(string));
            outTable.Columns.Add("开放级别", typeof(string));
            outTable.Columns.Add("授课讲师", typeof(string));
            outTable.Columns.Add("必修/选修", typeof(string));
            //outTable.Columns.Add("是否折算CPA学时", typeof(string));
            outTable.Columns.Add("框架内/外", typeof(string));
            //outTable.Columns.Add("框架类别", typeof(string));
            //foreach (Tr_YearPlanCourse v in yearList)
            //{
            //    outTable.Rows.Add(v.CourseName, v.OpenTime, v.OpenLevel, v.WayStr, v.Realname,
            //                      v.PayGrade, v.IsMustStr, v.IsSystemStr, v.SystemTree);
            //}
            foreach (Dep_YearPlanCourse v in yearList)
            {
                //outTable.Rows.Add(v.CourseName,v.CourseLength, v.OpenTime, v.OpenLevel,v.Realname,v.PayGrade, v.IsMustStr,v.IsCPAStr, v.IsSystemStr);
                outTable.Rows.Add(v.CourseName, v.CourseLength, v.OpenTime, v.OpenLevel, v.Realname, v.IsMustStr, v.IsSystemStr);
            }
            new Spreadsheet().Template(year.Year + "年度计划课程安排", null, outTable, HttpContext, "年度计划课程", "年度计划");
        }

        /// <summary>
        /// 根据ID删除年计划
        /// </summary>
        public JsonResult DeleteYearCourse(int id, string ids)
        {
            bool result = Iyear.DeleteYearCourse(id, ids);
            if (result)
                return Json(new
                {
                    result = 1,
                    content = "删除成功"
                }, JsonRequestBehavior.AllowGet);
            else
                return Json(new
                {
                    result = 0,
                    content = "删除失败"
                }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据IDS查询课程信息
        /// </summary>
        public JsonResult GetCourses(string ids)
        {
            var result = Iyear.GetCourseList(ids);
            return Json(new
            {
                dataList = result,
                recordCount = result.Count
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证年度计划
        /// </summary>
        public JsonResult Checkdept(int year)
        {
            string depids = Iyear.GetYearDepids(year);
            return Json(depids, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有年计划列表
        /// </summary>
        public JsonResult GetLinkYearList(int id, string startTime, string endTime, string DeptID, int pageSize = 20, int pageIndex = 1)
        {
            DateTime s = DateTime.Parse("1990-01-01");
            DateTime e = DateTime.Now;
            if (!string.IsNullOrEmpty(startTime))
                s = DateTime.Parse(startTime);
            if (!string.IsNullOrEmpty(endTime))
                e = DateTime.Parse(endTime).AddDays(1);
            string where = " 1=1";
            string deptids = "";
            try
            {
                int totalCount = 0;

                if (id > 0)
                {
                    where += string.Format(" and t0.Year={2} and t0.LastUpdateTime >= '{0}' and  t0.LastUpdateTime <= '{1}' and t0.IsDelete=0", s, e, id);
                }
                else
                {
                    where += string.Format(" and t0.LastUpdateTime >= '{0}' and  t0.LastUpdateTime <= '{1}' and t0.IsDelete=0", s, e);
                }
                List<Dep_YearPlan> yearLinkList = Iyear.GetLinkYearPlanList(DeptID, out totalCount, pageIndex, pageSize, where);
                foreach (var item in yearLinkList)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
                    item.DeptName = item.DeptName.HtmlXssEncode();
                    item.linkDeptName = string.IsNullOrEmpty(item.linkDeptName) == true ? "——" : item.linkDeptName.HtmlXssEncode();
                    item.OpenTypeStr = "被联合上报";
                }
                return Json(new
                {
                    dataList = yearLinkList,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        [Filter.SystemLog("接受或拒绝联合 发送消息", LogLevel.Info)]
        public JsonResult SendMessageByYear(string Yids, int did, int flag, int Must)
        {
            try
            {
                string userName = CurrentUser == null ? "" : CurrentUser.Realname;
                //int deptid = CurrentUser == null ? 0 : CurrentUser.DeptId;
                List<Dep_YearPlan> yearlist = Iyear.GetDepYearListByWhere(string.Format(" Id in ({0})", Yids));
                if (yearlist.Count > 0)
                {
                    List<string> telephone = new List<string>();
                    List<string> EmailAddress = new List<string>();

                    var emailList = new List<KeyValuePair<string, string>>();
                    var messlist = new List<KeyValuePair<string, string>>();
                    var yearDepLinklist = Iyear.GetDepLink_ApprovalFlagList(did);
                    var userList = userBL.GetList();
                    var linkList = Iyear.GetDepLink(did);
                    var ids = "";
                    var reject = "";
                    var passList = new List<int>();
                    foreach (Dep_YearPlan u in yearlist)
                    {
                        string flagStr = flag == 1 ? "通过" : "拒绝";
                        Sys_User usermodel = userList.Where(p => p.UserId == u.UserID).FirstOrDefault();
                        passList.Add(u.UserID);
                        string Content = usermodel.Realname + "，您好! 您于" + u.LastUpdateTime.ToString("yyyy-MM-dd HH:mm") + "发起的" + u.Year + "年度计划联合上报," + userName + "已于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + flagStr + "了您的请求，详情请见培训系统";
                        if (!string.IsNullOrWhiteSpace(usermodel.Email) && (Must == 1 || Must == 3))
                            emailList.Add(new KeyValuePair<string, string>(usermodel.Email, Content.Replace("教育培训部", CurrentUser.DeptName)));
                        if (!string.IsNullOrWhiteSpace(usermodel.MobileNum) && (Must == 2 || Must == 3))
                            messlist.Add(new KeyValuePair<string, string>(usermodel.MobileNum, Content.Replace("教育培训部", CurrentUser.DeptName)));
                        Dep_LinkDepart linkmodel = linkList.Where(p => p.YearId == u.Id).FirstOrDefault();
                        if (linkmodel != null)
                        {
                            linkmodel.ApprovalFlag = flag;
                            linkmodel.ApprovalTime = DateTime.Now;
                            //  ids = ids == "" ? linkmodel.Id.ToString() : ids + "," + linkmodel.Id;
                            bool re = Iyear.UpdateDepLink(linkmodel);
                            if (re)
                            {
                                if (flag == 1)
                                {
                                    yearDepLinklist = yearDepLinklist.Where(p => p.Year == u.Year).ToList();
                                    if (yearDepLinklist.Count > 0)
                                    {
                                        foreach (Dep_YearPlan item in yearDepLinklist)
                                        {
                                            if (!passList.Contains(item.UserID))
                                            {
                                                Sys_User Dusermodel = userList.Where(p => p.UserId == item.UserID).FirstOrDefault();
                                                string DContent = Dusermodel.Realname + "，您好! 您于" + item.LastUpdateTime.ToString("yyyy-MM-dd HH:mm") + "发起的" + item.Year + "年度计划联合上报," + userName + "已于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "拒绝了您的请求，详情请见培训系统";
                                                if (!string.IsNullOrWhiteSpace(Dusermodel.Email) && (Must == 1 || Must == 3))
                                                    emailList.Add(new KeyValuePair<string, string>(Dusermodel.Email, DContent));
                                                if (!string.IsNullOrWhiteSpace(Dusermodel.MobileNum) && (Must == 2 || Must == 3))
                                                    messlist.Add(new KeyValuePair<string, string>(Dusermodel.MobileNum, DContent));
                                            }
                                        }
                                        Iyear.UpdateDepLink_ApprovalFlag(u.Year, did);
                                    }
                                }
                            }
                        }
                    }
                    if (Must == 1)
                    {
                        if (emailList.Count > 0)
                            SendEmail(emailList, "年度计划联合上报反馈");
                    }
                    else if (Must == 2)
                    {
                        if (messlist.Count > 0)
                            SendMessage(messlist);
                    }
                    else
                    {
                        if (emailList.Count > 0)
                            SendEmail(emailList, "年度计划联合上报反馈");
                        if (messlist.Count > 0)
                            SendMessage(messlist);
                    }
                    return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 年度计划跟踪-部门/分所(已上报)
        /// </summary>
        public JsonResult GetYearTrackList(int did, int id, int isopen, string deptName, string startTime, string endTime, int pageSize = 20, int pageIndex = 1, string jsRenderSortField = "tempTa.Year asc")
        {
            if (Session["Tracktpage"] != null)
            {
                Session.Remove("Tracktpage");
            }
            Session["Tracktpage"] = startTime + "㉿" + endTime + "㉿" + deptName + "㉿" + id + "㉿" + isopen + "㉿" + pageIndex;

            if (id > 0)
            {
                DateTime s = DateTime.Parse("1990-01-01");
                DateTime e = DateTime.Now;
                if (!string.IsNullOrEmpty(startTime))
                    s = DateTime.Parse(startTime);
                if (!string.IsNullOrEmpty(endTime))
                    e = DateTime.Parse(endTime).AddDays(1);
                string where = " 1=1";
                string deptype = "in";
                try
                {
                    int totalCount = 0;
                    if (did > 0)
                    {
                        deptype = "not in";
                    }
                    where += string.Format(" and tempTa.Year={2} and tempTa.LastUpdateTime >= '{0}' and  tempTa.LastUpdateTime <= '{1}' ", s, e, id);
                    switch (isopen)
                    {
                        case 0:
                            break;
                        case 1:
                            where += string.Format(" and tempTa.IsOpen=0 ");
                            break;
                        case 2:
                            where += string.Format(" and tempTa.IsOpen=1 ");
                            break;
                    }
                    if (!string.IsNullOrEmpty(deptName))
                    {
                        where += string.Format(" AND (tempTa.DeptName like '%{0}%' or tempTa.DeptNames like '%{0}%') ", deptName.ReplaceSql());
                    }
                    List<Dep_YearPlan> yearList = Iyear.GetYearPlanTrackList(out totalCount, pageIndex, pageSize, deptype, where, "order by " + jsRenderSortField);
                    foreach (var item in yearList)
                    {
                        item.Realname = item.Realname.HtmlXssEncode();
                        item.DeptName = item.DeptName.HtmlXssEncode();
                    }
                    return Json(new
                    {
                        dataList = yearList,
                        recordCount = totalCount
                    }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(new
                    {
                        dataList = new object[0],
                        recordCount = 0
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 年度计划跟踪-部门/分所(未上报)
        /// </summary>
        public JsonResult GetYearNoTrackList(int did, int id, string deptName, int pageSize = 20, int pageIndex = 1)
        {
            if (id > 0)
            {
                string where = " 1=1";
                try
                {
                    int totalCount = 0;
                    if (!string.IsNullOrEmpty(deptName))
                    {
                        where += string.Format(" AND sd.DeptName like '%{0}%'", deptName.ReplaceSql());
                    }
                    List<Sys_Department> deptList = Iyear.GetNoTrackList(id, out totalCount, pageIndex, pageSize, did, where);
                    foreach (var item in deptList)
                    {
                        item.DeptName = item.DeptName.HtmlXssEncode();
                    }
                    return Json(new
                    {
                        dataList = deptList,
                        recordCount = totalCount
                    }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(new
                    {
                        dataList = new object[0],
                        recordCount = 0
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    dataList = new object[0],
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 部门树

        /// <summary>
        /// 获取树形结构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDeptSort()
        {
            List<Sys_Department> deptSort = new List<Sys_Department>();
            var treeStr = new StringBuilder();
            if (CurrentUser.TrainMaster == 0)
            {
                int deptid = CurrentUser == null ? 0 : CurrentUser.DeptId;
                deptSort = depBL.GetAllList(" IsDelete=0 ").OrderBy(p => p.DepartmentId).ToList();
                Sys_Department dept = deptSort.Where(p => p.DepartmentId == deptid).First();
                treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview\">");
                treeStr.Append("<li id=\"tree0\" class='pNote'>");
                treeStr.Append("<ul>");
                treeStr.AppendFormat("<li id=\"tree{0}\" class='pNote'>", deptid);
                treeStr.AppendFormat("<a id=\"{0}\" dir='dir' title=\"{1}\" onclick=\"selectSort({0},this);\">{1}</a>", deptid, dept.DeptName);
                AddChild(deptid, deptSort, treeStr);
                treeStr.Append("</li>");
                treeStr.Append("</ul>");
                treeStr.Append("</li>");
                treeStr.AppendLine("</ul>");
            }
            else
            {
                deptSort = GetAllSubDepartments();
                treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview\">");
                treeStr.Append("<li id=\"tree0\" class='pNote'>");
                treeStr.Append("<ul>");
                foreach (Sys_Department dept in deptSort)
                {
                    treeStr.AppendFormat(
                        "<li id='tree{0}' class='pNote'><a id=\"{0}\" dir='dir' title=\"{1}\" onclick=\"selectSort({0},this);\">{1}</a>",
                        dept.DepartmentId, dept.DeptName);
                    treeStr.Append("</li>");
                }
                treeStr.Append("</ul>");
                treeStr.Append("</li>");
                treeStr.AppendLine("</ul>");
            }
            return Json(treeStr.ToString(), JsonRequestBehavior.AllowGet);
        }

        private void AddChild(int fSortID, IEnumerable<Sys_Department> sorts, StringBuilder treeStr)
        {
            if (sorts.Count(p => p.ParentDeptId == fSortID) > 0)
            {
                treeStr.Append("<ul>");
                foreach (Sys_Department sort in sorts.Where(p => p.ParentDeptId == fSortID))
                {
                    treeStr.AppendFormat(
                        "<li id='tree{0}' class='pNote'><a title=\"{1}\" onclick=\"selectSort({0},this);\" id=\"{0}\">{1}</a>",
                        sort.DepartmentId, sort.DeptName);
                    treeStr.AppendLine();
                    AddChild(sort.DepartmentId, sorts, treeStr);
                    treeStr.AppendLine();
                    treeStr.Append("</li>");
                }
                treeStr.Append("</ul>");
            }
        }

        #endregion
    }
}
