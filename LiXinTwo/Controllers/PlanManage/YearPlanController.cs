using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LiXinControllers.Filter;
using LiXinModels.PlanManage;
using System.Data;
using LiXinModels.CourseManage;
using System.Text.RegularExpressions;
using LiXinCommon;
using System.Text;
using System.Web;
using LiXinModels;
namespace LiXinControllers
{
    public partial class PlanManageController : BaseController
    {
        #region 呈现页面

        /// <summary>
        ///  年计划列表呈现
        /// </summary>
        public ViewResult YearPlan()
        {
            List<Tr_YearPlan> itemArray = Iyear.GetAllYear("IsDelete=0 ORDER BY Year asc");
            ViewData["StrYear"] = itemArray;
            return View();
        }

        /// <summary>
        ///  年计划编辑页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult YearPlanAdd()
        {
            List<int> years = Iyear.GetAllYear();
            ViewBag.year = DateTime.Now.Year;
            ViewData["Years"] = years;
            return View();
        }

        /// <summary>
        ///  年计划编辑页面呈现
        /// </summary>
        /// <returns></returns>
        public ActionResult YearPlanEdit(int id)
        {
            Tr_YearPlan itemArray = Iyear.GetYearModel(id);
            ViewBag.trainGrade = trainBL.GetAllTrainGrade();
            ViewData["Yearmodel"] = itemArray;
            return View();
        }

        /// <summary>
        ///  年计划列表呈现
        /// </summary>
        public ViewResult YearPlanDetail(int id)
        {
            Tr_YearPlan itemArray = Iyear.GetYearModel(id);
            ViewBag.trainGrade = trainBL.GetAllTrainGrade();
            ViewData["Yearmodel"] = itemArray;
            return View();
        }

        /// <summary>
        ///  新增课程呈现
        /// </summary>
        public ViewResult AddYearCourse(string type,int ccid,string str)
        {
            ViewBag.year = Request.QueryString["year"] ?? DateTime.Now.Year.ToString();
            ViewBag.month = Request.QueryString["month"] ?? DateTime.Now.Month.ToString();
            ViewBag.trainGrade = trainBL.GetAllTrainGrade();
            Tr_YearPlanCourse model = new Tr_YearPlanCourse();
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace(" ","+");
                string strr = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(str));
                strr=System.Web.HttpUtility.HtmlDecode(strr);
                string[] coList = strr.Split('♣');
                model = new Tr_YearPlanCourse
                {
                    CourseId = Convert.ToInt32(coList[0]),
                    CourseName = coList[1],
                    Code = coList[2],
                    Year = Convert.ToInt32(coList[3]),
                    Month = coList[4],
                    Day = Convert.ToInt32(coList[5]),
                    OpenLevel = coList[6],
                    IsMust = Convert.ToInt32(coList[7]),
                    Way = Convert.ToInt32(coList[8]),
                    Teacher = coList[9],
                    Realname = coList[10],
                    PayGrade = coList[11],
                    IsSystem = Convert.ToInt32(coList[12]),
                    IsCPA = Convert.ToInt32(coList[13]),
                    CourseLength = Convert.ToDecimal(coList[14])
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

        #endregion

        #region 操作

        /// <returns></returns>
        /// <summary>
        /// 获取所有年计划列表
        /// </summary>
        public JsonResult GetAllYearList(int id,string startTime,string endTime,int pageSize = 20, int pageIndex = 1)
        {
            DateTime s = DateTime.Parse("1990-01-01");
            DateTime e = DateTime.Now;
            if (!string.IsNullOrEmpty(startTime))
                s = DateTime.Parse(startTime);
            if (!string.IsNullOrEmpty(endTime))
                e = DateTime.Parse(endTime).AddDays(1);
            string where = "";
            try
            {
                int totalCount = 0;
                if (id > 0)
                {
                    where = string.Format("t0.Id={2} and t0.LastUpdateTime >= '{0}' and  t0.LastUpdateTime <= '{1}' and t0.IsDelete=0", s, e, id);
                }
                else
                {
                    where = string.Format("t0.LastUpdateTime >= '{0}' and  t0.LastUpdateTime <= '{1}' and t0.IsDelete=0", s, e);
                }
                List<Tr_YearPlan> yearList = Iyear.GetAllYearList(out totalCount, pageIndex, pageSize, where);
                foreach (var item in yearList)
                {
                    item.Realname = item.Realname.HtmlXssEncode();
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
        /// 保存年计划
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("编辑年计划", LogLevel.Info)]
        public JsonResult SubmitYear(int id, int selyear, string courlist, string DelID)
        {
            if (!string.IsNullOrEmpty(DelID) && id>0)
            {
                DelID = DelID.Remove(DelID.LastIndexOf(","), 1);
                Iyear.DeleteCoursebyYear(id, DelID);
            }
            if (!string.IsNullOrEmpty(courlist))
            {
                courlist = courlist.Remove(courlist.LastIndexOf(","), 1);
                string[] course = Regex.Split(courlist, ",", RegexOptions.IgnoreCase);
                int yearid = 0;
                try
                {
                    if (id > 0)
                    {
                        yearid = id;
                    }
                    else
                    {
                        yearid = Iyear.InsertYear(new Tr_YearPlan
                        {
                            LastUpdateTime = DateTime.Now,
                            Year = selyear,
                            UserID = CurrentUser == null ? 0 : CurrentUser.UserId,
                            PublishFlag = 0,
                            IsDelete = 0
                        });
                    }
                    if (yearid > 0)
                    {
                        for (int i = 0; i < course.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(course[i]))
                            {
                                course[i] = course[i].Replace(" ", "+");
                                course[i] = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(course[i]));
                                course[i] = System.Web.HttpUtility.HtmlDecode(course[i]);
                                string[] coList = course[i].Split('♣');
                                Co_Course model = new Co_Course
                                {
                                    CourseName = HttpUtility.UrlDecode(coList[1]),
                                    Code = HttpUtility.UrlDecode(coList[2]),
                                    Year = Convert.ToInt32(coList[3]),
                                    Month = coList[4],
                                    PreCourseTime = DateTime.Now,
                                    Day = Convert.ToInt32(coList[5]),
                                    OpenLevel = coList[6],
                                    IsCPA = Convert.ToInt32(coList[13]),
                                    IsMust = Convert.ToInt32(coList[7]),
                                    CourseLength = Convert.ToDecimal(coList[14]),
                                    Way = Convert.ToInt32(coList[8]),
                                    Teacher = coList[9],
                                    SurveyPaperId = "0",
                                    CourseFrom = 0,
                                    IsSystem = Convert.ToInt32(coList[12]),
                                    IsYearPlan=1,
                                    LastUpdateTime = DateTime.Now
                                };
                                if (Convert.ToInt32(coList[0]) < 1)
                                {
                                    Imonth.InsertCo_Course(model);
                                    Iyear.InsertYearCourse(new Tr_YearPlanCourse
                                    {
                                        YearId = yearid,
                                        CourseId = model.Id,
                                        IsSystem = Convert.ToInt32(coList[12])
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
                if (!string.IsNullOrEmpty(DelID)&& id>0)
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
        /// 根据ID删除年计划
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("删除年计划", LogLevel.Info)]
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
        /// <returns></returns>
        [Filter.SystemLog("发布年计划", LogLevel.Info)]
        public JsonResult PublishYear(int id)
        {
            if (Iyear.IsYearUser(id))
            {
                Tr_YearPlan Ymodel = Iyear.GetYearModel(id);
                Ymodel.PublishFlag = 1;
                bool result = Iyear.UpdateYearByID(Ymodel);
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
                    content = "发布失败,一些课程不存在授课讲师"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <returns></returns>
        /// <summary>
        /// 根据ID获取所有课程列表(无分页)
        /// </summary>
        public JsonResult GetUpdataYCList(int yid,string month, string openLevel)
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
                List<Tr_YearPlanCourse> yearList = Iyear.GetAllYearCourseList(yid,"asc",where);
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

        /// <returns></returns>
        /// <summary>
        /// 根据ID获取所有课程列表(有分页)
        /// </summary>
        public JsonResult GetYearCourseList(int yid, string name, string Teaname,string Way, string openLevel, string isMust, string isSystem, string Order, int pageSize = 20, int pageIndex = 1)
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
                if (Way!="-1")
                {
                    where += string.Format(" and cc.Way={0}", Convert.ToInt32(Way));
                }
                List<Tr_YearPlanCourse> yearList = Iyear.GetAllYearCourseList(yid,out totalCount, pageIndex, pageSize,Order,where);
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
        public void OutYearCourse(int id, string name, string Way, string Teaname, string isMust, string isSystem, string openLevel, string Order)
        {
            Tr_YearPlan year = Iyear.GetYearModel(id);
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
            if (Way != "-1")
            {
                where += string.Format(" and cc.Way={0}", Convert.ToInt32(Way));
            }
            List<Tr_YearPlanCourse> yearList = Iyear.GetAllYearCourseList(id,Order,where);
            DataTable outTable = new DataTable();
            outTable.Columns.Add("课程名称", typeof(string));
            outTable.Columns.Add("学时", typeof(string));
            outTable.Columns.Add("预计开课时间", typeof(string));
            outTable.Columns.Add("开放级别", typeof(string));
            outTable.Columns.Add("培训形式", typeof(string));
            outTable.Columns.Add("授课讲师", typeof(string));
            outTable.Columns.Add("讲师薪酬", typeof(string));
            outTable.Columns.Add("必修/选修", typeof(string));
            outTable.Columns.Add("是否折算CPA学时", typeof(string));
            outTable.Columns.Add("框架内/外", typeof(string));
            //outTable.Columns.Add("框架类别", typeof(string));
            //foreach (Tr_YearPlanCourse v in yearList)
            //{
            //    outTable.Rows.Add(v.CourseName, v.OpenTime, v.OpenLevel, v.WayStr, v.Realname,
            //                      v.PayGrade, v.IsMustStr, v.IsSystemStr, v.SystemTree);
            //}
            foreach (Tr_YearPlanCourse v in yearList)
            {
                outTable.Rows.Add(v.CourseName,v.CourseLength, v.OpenTime, v.OpenLevel, v.WayStr, v.Realname,
                                  v.PayGrade, v.IsMustStr,v.IsCPAStr, v.IsSystemStr);
            }
            new Spreadsheet().Template(year.Year+"年度计划课程安排",null, outTable, HttpContext,"年度计划课程","年度计划");
        }

        /// <summary>
        /// 根据ID删除年计划
        /// </summary>
        /// <returns></returns>
        [Filter.SystemLog("删除年计划课程", LogLevel.Info)]
        public JsonResult DeleteYearCourse(int id, string ids)
        {
            bool result = Iyear.DeleteYearCourse(id,ids);
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
        /// <returns></returns>
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
        public JsonResult CheckYear(int etitle)
        {
            bool isValidate = Iyear.Checkname(etitle);
            return Json(isValidate, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
