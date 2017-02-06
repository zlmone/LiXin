using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinModels.SystemManage;
using LiXinCommon;
using LiXinModels;

namespace LiXinControllers
{
    public partial class ParamConfigController : BaseController
    {
        #region 其他形式申请项目配置-其他项目
        /// <summary>
        /// 其他形式申请项目配置
        /// </summary>
        /// <param name="manType">大Tab页</param>
        /// <param name="partType">小Tab页</param>
        /// <returns></returns>
        public ActionResult FreeApplyConfig(int manType = 0, int partType = 0)
        {
            ViewBag.manType = manType;
            ViewBag.partType = partType;
            ViewBag.listYear = AllSystemConfigs.Where(p => p.LastUpdateTime.Year > 1905)
                                           .Select(p => p.LastUpdateTime.Year).Distinct().OrderBy(p => p).ToList();
            return View();
        }

        /// <summary>
        /// 其他形式申请项目配置
        /// </summary>
        /// <returns></returns>
        public ActionResult FreeOtherApply(int year)
        {
            ViewBag.year = year;

            return View();
        }

        /// <summary>
        /// 其他形式申请项目----授课人
        /// </summary>
        /// <returns></returns>
        public ActionResult FreeOtherTeacher(int year)
        {
            ViewBag.year = year;
            var single = freeBL.GetModel(" ApplyType=0  and year=" + year);
            ViewBag.IsEdit = DateTime.Now.Year <= year ? 1 : 0;
            return View(single == null ? new Free_OtherApplyConfig() : single);
        }


        public ActionResult FreeOtherSurvey(int year)
        {
            ViewBag.year = year;
            var model = AllSystemConfigs.Where(p => p.ConfigType == 64 && p.LastUpdateTime.Year == year).FirstOrDefault();
            model = model == null ? new Sys_ParamConfig() : model;
            ViewBag.IsEdit = DateTime.Now.Year <= year ? 1 : 0;
            return View(model);
        }


        /// <summary>
        /// 其他形式申请项目----其他项目
        /// </summary>
        /// <returns></returns>
        public ActionResult FreeOther(int year)
        {
            ViewBag.year = year;
            ViewBag.IsEdit = DateTime.Now.Year <= year ? 1 : 0;
            return View();
        }

        /// <summary>
        /// 其他形式申请项目----增加其他项目
        /// </summary>
        /// <returns></returns>
        public ActionResult AddFreeOther(int id, int year)
        {
            var single = id == 0 ? new Free_OtherApplyConfig() : freeBL.GetModel(" ID=" + id + " and year=" + year);
            ViewBag.year = year;
            ViewBag.id = id;
            var model = AllSystemConfigs.Where(p => p.ConfigType == 62).FirstOrDefault();
            ViewBag.TrainGrade =string.IsNullOrEmpty(model.ConfigValue) ? new List<string>() : model.ConfigValue.Split(';')[0].Split(',').ToList();
            return View(single);
        }

        /// <summary>
        /// 添加其他项目
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type">0 授课人 1 其他项目 2 课后评估 </param>
        /// <returns></returns>
        public JsonResult AddOther(Free_OtherApplyConfig model, int year, int type = 1)
        {
            try
            {
                model.year = year;
                model.IsDelete = 0;
                model.ApplyType = type;
                model.UploadTip = string.IsNullOrEmpty(model.UploadTip) ? "" : model.UploadTip;
                model.Memo = string.IsNullOrEmpty(model.Memo) ? "" : model.Memo;
                if (model.Memo != "")
                {
                    model.Memo = model.Memo.Trim();
                }
                if (type == 0)
                {
                    model.ConvertBaseInfo = "";
                    model.ConvertType = 2;
                }
                else
                {
                    model.ConvertBaseInfo = string.IsNullOrEmpty(model.ConvertBaseInfo) ? "" : model.ConvertBaseInfo;
                    //model.ConvertType = 1;
                }

                if (model.ID == 0)
                {
                    model.CreateTime = DateTime.Now;
                    model.IsDelete = 0;
                    model.ApplyType = type;
                    freeBL.InsertFree_OtherApplyConfig(model);
                }
                else
                {

                    model.year = year;
                    freeBL.UpdateFree_OtherApplyConfig(model);
                }
                return Json(new { result = 1, Content = "保存成功" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, Content = "保存失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取其他项目list
        /// </summary>
        /// <param name="ApplyContent"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetFreeOtherList(string ApplyContent, int year, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalCount;
                string where = " 1=1 and  IsDelete=0 and ApplyType=1 and year=" + year;
                if (!string.IsNullOrEmpty(ApplyContent))
                {
                    where += " and ApplyContent like '%" + ApplyContent.ReplaceSql() + "%'";
                }
                var list = freeBL.GetFreeOtherList(out totalCount, where, pageIndex, pageSize);
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Free_OtherApplyConfig>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteFreeOther(int id)
        {
            try
            {
                freeBL.DeleteFree_OtherApplyConfig(id.ToString());
                return Json(new { result = 1, Content = "保存成功" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, Content = "保存失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取单个实例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetSingle(int id, int year)
        {
            try
            {
                var model = freeBL.GetModel("ID=" + id + " and year=" + year);
                return Json(new { result = 1, model = model }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, model = new Free_OtherApplyConfig() }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion


        #region 免修项目配置
        public ActionResult FreeApply(int year)
        {
            ViewBag.year = year;
            return View();
        }

        /// <summary>
        /// 免修项目
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult FreeItem(int year)
        {
            ViewBag.year = year;
            ViewBag.IsEdit = DateTime.Now.Year <= year ? 1 : 0;
            return View();
        }

        /// <summary>
        /// 自动折算
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult FreeOut(int year)
        {
            ViewBag.IsEdit = DateTime.Now.Year <= year ? 1 : 0;
            ViewBag.year = year;
            var model = AllSystemConfigs.Where(p => p.ConfigType == 63 && p.LastUpdateTime.Year == year).FirstOrDefault();
            return View(model);
        }


        /// <summary>
        /// 其他形式申请项目----增加其他项目
        /// </summary>
        /// <returns></returns>
        public ActionResult AddFreeItem(int id, int year)
        {
            var single = id == 0 ? new Free_ApplyConfig() : freeBL.GetFree_ApplyConfig(" ID=" + id + " and year=" + year);
            ViewBag.year = year;
            ViewBag.id = id;
            return View(single);
        }

        /// <summary>
        /// 获取其他项目list
        /// </summary>
        /// <param name="ApplyContent"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public JsonResult GetFreeList(string FreeName, int year, int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                int totalCount;
                string where = " 1=1 and year=" + year;
                if (!string.IsNullOrEmpty(FreeName))
                {
                    where += " and FreeName like '%" + FreeName.ReplaceSql() + "%'";
                }
                var list = freeBL.GetFreeApplyList(out totalCount, where, pageIndex, pageSize);
                return Json(new
                {
                    result = 1,
                    dataList = list,
                    recordCount = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    result = 0,
                    dataList = new List<Free_OtherApplyConfig>(),
                    recordCount = 0
                }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteFreeItem(int id)
        {
            try
            {
                freeBL.DeleteFree_ApplyConfig(id.ToString());
                return Json(new { result = 1, Content = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, Content = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取单个实例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetFreeItem(int id, int year)
        {
            try
            {
                var model = freeBL.GetFree_ApplyConfig("ID=" + id + " and year=" + year);
                return Json(new { result = 1, model = model }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, model = new Free_OtherApplyConfig() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveItem(int year, Free_ApplyConfig model)
        {
            try
            {
                model.year = year;
                model.IsDelete = 0;
                if (model.ID == 0)
                {
                    model.CreateTime = DateTime.Now;
                    freeBL.InsertFree_ApplyConfig(model);
                }
                else
                {
                    freeBL.UpdateFree_ApplyConfig(model);
                }

                return Json(new { result = 1, Content = "保存成功" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, Content = "保存失败" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="configType"></param>
        /// <param name="configValue"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public JsonResult AddConfig(int configType, string configValue, int year)
        {
            try
            {

                var ConfigName = "";
                switch (configType)
                {
                    case 63:
                        ConfigName = "其他有组织形式折算学时限制";
                        break;
                    case 64:
                        ConfigName = "其他形式申请项目--课后评估";
                        break;
                    case 65:
                        ConfigName = "其他有组织形式折算学时限制";
                        break;
                }

                if (AllSystemConfigs.Any(p => p.ConfigType == 65 && p.LastUpdateTime.Year == year))
                {
                    paramconfigBL.UpadteSys_ParamConfigByYear(configType, configValue,year, 0);
                }
                else
                {
                    var model = new Sys_ParamConfig();
                    model.ConfigName = ConfigName;
                    model.ConfigType = configType;
                    model.ConfigValue = configValue;
                    model.userType = 0;
                    //是否为当年创建
                    model.LastUpdateTime = DateTime.Now.Year == year ? DateTime.Now : Convert.ToDateTime(year + "-01-01");
                    paramconfigBL.InsertSys_ParamConfig(model);
                }
                lock (obj)
                {
                    RefreshSystemConfigsCache();
                }

                return Json(new { result = 1, Content = "保存成功" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { result = 0, Content = "保存失败" }, JsonRequestBehavior.AllowGet);
            }


        }
    }
}
