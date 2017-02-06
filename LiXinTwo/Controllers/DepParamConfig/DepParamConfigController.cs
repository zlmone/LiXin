using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinInterface.DeptSystemManage;
using LiXinModels;
using LiXinModels.User;
using LiXinControllers.Filter;

namespace LiXinControllers.DepParamConfig
{
    public class DepParamConfigController : BaseController
    {
        protected IDep_ParamConfig DepParamCon;
        public DepParamConfigController(IDep_ParamConfig _DepParamCon)
        {
            DepParamCon = _DepParamCon;
        }

        #region 页面
        public ActionResult DepParamConfig()
        {
            List<Sys_Department> departMent = new List<Sys_Department>();
            departMent = GetAllSubDepartments("");
            ViewBag.departMent = departMent;
            return View();
        }

        public ActionResult TrainReportingTimeConfig()
        {
            int id = Convert.ToInt32(Request.QueryString["departId"]);
            int type = Convert.ToInt32(Request.QueryString["type"]);
            Sys_ParamConfig sp = DepParamCon.GetConfig(id, type);
            if (sp == null)
            {
                sp = new Sys_ParamConfig();
                sp.ConfigValue = "1;00;00;1;00;00";
            }
            return View(sp);
        }

        public ActionResult ClassOpingTimeConfig()
        {
            int id = Convert.ToInt32(Request.QueryString["departId"]);
            int type = Convert.ToInt32(Request.QueryString["type"]);
            Sys_ParamConfig sp = DepParamCon.GetConfig(id, type);
            if (sp == null)
            {
                sp = new Sys_ParamConfig();
                sp.ConfigValue = ";";
            }
            return View(sp);
        }

        /// <summary>
        /// 过时课程课后评估的开放时间
        /// </summary>
        /// <returns></returns>
        public ActionResult TimeOutClassOpingTimeConfig()
        {
            var id = Convert.ToInt32(Request.QueryString["departId"]);
            var type = Convert.ToInt32(Request.QueryString["type"]);
            var sp = DepParamCon.GetConfig(id, type) ?? new Sys_ParamConfig { ConfigValue = "" };
            return View(sp);
        }

        public ActionResult TestAnswerTimeConfig()
        {
            int id = Convert.ToInt32(Request.QueryString["departId"]);
            int type = Convert.ToInt32(Request.QueryString["type"]);
            Sys_ParamConfig sp = DepParamCon.GetConfig(id, type);
            if (sp == null)
            {
                sp = new Sys_ParamConfig();
                sp.ConfigValue = ";";
            }
            return View(sp);
        }

        /// <summary>
        /// 过时课程在线测试允许答题时间
        /// </summary>
        /// <returns></returns>
        public ActionResult TimeOutTestAnswerTimeConfig()
        {
            var id = Convert.ToInt32(Request.QueryString["departId"]);
            var type = Convert.ToInt32(Request.QueryString["type"]);
            var sp = DepParamCon.GetConfig(id, type) ?? new Sys_ParamConfig { ConfigValue = ";" };
            return View(sp);
        }

        public ActionResult TestMaxNumberConfig()
        {
            int id = Convert.ToInt32(Request.QueryString["departId"]);
            int type = Convert.ToInt32(Request.QueryString["type"]);
            Sys_ParamConfig sp = DepParamCon.GetConfig(id, type);
            if (sp == null)
            {
                sp = new Sys_ParamConfig();
                sp.ConfigValue = "";
            }
            return View(sp);
        }

        public ActionResult CompletionOnlineTestTimes()
        {
            int id = Convert.ToInt32(Request.QueryString["departId"]);
            int type = Convert.ToInt32(Request.QueryString["type"]);
            Sys_ParamConfig sp = DepParamCon.GetConfig(id, type);
            if (sp == null)
            {
                sp = new Sys_ParamConfig();
                sp.ConfigValue = "";
            }
            return View(sp);
        }

        public ActionResult YearCourseRescindNumberConfig()
        {
            int id = Convert.ToInt32(Request.QueryString["departId"]);
            int type = Convert.ToInt32(Request.QueryString["type"]);
            Sys_ParamConfig sp = DepParamCon.GetConfig(id, type);
            if (sp == null)
            {
                sp = new Sys_ParamConfig();
                sp.ConfigValue = "";
            }
            return View(sp);
        }

        public ActionResult AttendceTimeLimited()
        {
            int id = Convert.ToInt32(Request.QueryString["departId"]);
            int type = Convert.ToInt32(Request.QueryString["type"]);
            Sys_ParamConfig sp = DepParamCon.GetConfig(id, type);
            if (sp == null)
            {
                sp = new Sys_ParamConfig();
                sp.ConfigValue = "";
            }
            return View(sp);
        }
        #endregion

        #region 方法
        [Filter.SystemLog("修改参数配置", LogLevel.Info)]
        public JsonResult UpdateConfig(int deptID = 0)
        {
            var id = deptID == 0 ? CurrentUser.DeptId : deptID;
            var type = Convert.ToInt32(Request.QueryString["type"]);
            var value = Request.QueryString["value"];
            var exist = false;
            var update = false;
            exist = DepParamCon.IsExist(id, type);
            update = exist == false ? DepParamCon.InsertConfig(id, type, value, ((ConfigName)type).ToString()) : DepParamCon.UpdateConfig(id, type, value);
            return Json(update ? new { result = 1 } : new { result = 0 }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public enum ConfigName
    {
        培训需求上报时间设定 = 1,
        课前建议和课后评估的开放时间 = 2,
        在线测试允许答题时间 = 3,
        在线测试允许最大次数 = 4,
        全年在线测试完成次数 = 5,
        全年课程退订次数 = 6,
        请假审核时限 = 7,
        过时课程课后评估的开放时间 = 8,
        过时课程在线测试允许答题时间 = 9
    }
}
