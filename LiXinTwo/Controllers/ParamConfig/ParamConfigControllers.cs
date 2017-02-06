using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LiXinInterface;
using LiXinInterface.SystemManage;
using LiXinModels;
using LiXinInterface.Examination;
using LiXinBLL.Examination;
using System.Data;
using LiXinInterface.CourseLearn;
using LiXinLanguage;
using LiXinModels.User;
using LiXinModels.SystemManage;

namespace LiXinControllers
{
    public partial class ParamConfigController : BaseController
    {
        protected object obj = new object();

        protected ISys_ParamConfig paramconfigBL;
        protected ISys_TrainGrade ISysTrainGradeBL;


        protected IExamTest ExamTestBL;
        protected ExaminationBL ExamDB;

        protected ICl_CpaLearnStatus ICpaLearnStatusBL;


        protected ISys_Notes noteBL;

        protected IFreeConfig freeBL;
        private List<int> searchDeptIds = new List<int>();

        private List<Sys_Department> SearchDepartments = new List<Sys_Department>();

        private List<Sys_Department> SearchcheckDepartments = new List<Sys_Department>();

        private IFree_OtherFroms fFree_otherfrom;
 

        public ParamConfigController(ISys_ParamConfig _paramconfigBL, ISys_TrainGrade _ISysTrainGradeBL,
            IExamTest _ExamTestBL, ExaminationBL _ExamDB, ICl_CpaLearnStatus _ICpaLearnStatusBL, ISys_Notes _noteBL, IFree_OtherFroms fFree_otherfromBL, IFreeConfig _freeBL)
        {
            paramconfigBL = _paramconfigBL;
            ISysTrainGradeBL = _ISysTrainGradeBL;
            ExamTestBL = _ExamTestBL;
            ExamDB = _ExamDB;
            ICpaLearnStatusBL = _ICpaLearnStatusBL;
            noteBL = _noteBL;
            fFree_otherfrom = fFree_otherfromBL;

            freeBL = _freeBL;


        }

        #region 呈现页面

        public ActionResult ParamConfig(int id = 0, int fromId = 0)
        {
            ViewBag.id = id;
            ViewBag.fromId = fromId;
            return View();
        }

        public ActionResult EmailConfig()
        {
            //Sys_ParamConfig model = paramconfigBL.GetSys_ParamConfig(1);
            var model = AllSystemConfigs.Find(p => p.ConfigType == 1);
            ViewBag.model = model;
            return View();
        }

        public ActionResult test()
        {
            return View();
        }
        public String httpreq(String url)
        {
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(url);
            rq.Method = "GET";
            rq.ContentType = "application/x-www-form-urlencoded";
            Stream receiveStream = (rq.GetResponse() as HttpWebResponse).GetResponseStream();
            Encoding respseCode = Encoding.Default;
            try
            {
                respseCode = Encoding.GetEncoding("UTF-8");
            }
            catch
            {
                respseCode = Encoding.Default;
            }
            StreamReader sr = new StreamReader(receiveStream, respseCode);
            return sr.ReadToEnd();
        }

        public ActionResult ParamConfigLeft()
        {
            return View();
        }

        public ActionResult ClassTemplateConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 31);
            ViewBag.model = model;
            return View();
        }

        public ActionResult MessageConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 2);
            ViewBag.model = model;
            return View();
        }

        public ActionResult AfterEvaluationConfig()
        {
            return View();
        }

        public ActionResult DepartmentConfig()
        {
            return View();
        }

        /// <summary>
        /// 5:违纪学时
        /// </summary>
        /// <returns></returns>
        public ActionResult DisciplineHoursConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 5);
            ViewBag.model = model;
            string chidao = "";
            if (model.ConfigValue != "")
            {
                for (int i = 0; i < model.ConfigValue.Split(';').Length; i++)
                {

                    if (model.ConfigValue.Split(';')[i].Split(',')[0] == "0")
                    {
                        chidao += "<tr type='chidao'>";
                        chidao += "<td>迟到</td>";
                    }
                    else
                    {
                        chidao += "<tr type='zaotui'>";
                        chidao += "<td>早退</td>";
                    }
                    chidao += "<td>" + model.ConfigValue.Split(';')[i].Split(',')[1] + "</td>";
                    chidao += "<td>" + model.ConfigValue.Split(';')[i].Split(',')[2] + "</td>";
                    chidao += "<td>" + model.ConfigValue.Split(';')[i].Split(',')[3] + "</td>";
                    chidao += "<td><a class='icon idel' title='删除' onclick='fdelete(this)'></a></td>";
                    chidao += "</tr>";
                }

            }
            ViewBag.chidao = chidao;

            return View();
        }

        public ActionResult ExamNumberConfig()
        {
            return View();
        }

        public ActionResult TrainYearConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 7);
            ViewBag.model = model;
            return View();
        }

        public ActionResult CPATrainConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 8);
            ViewBag.model = model;
            return View();
        }

        public ActionResult TrainLevelAdjustTimeConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 9);
            ViewBag.model = model;
            return View();
        }

        public ActionResult AllDeptTrainSetTime()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 10);
            ViewBag.model = model;
            return View();
        }

        public ActionResult TrainReportingTimeConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 11);
            ViewBag.model = model;
            return View();
        }

        public ActionResult TrainOpenObjectConfig()
        {
            return View();
        }

        public ActionResult AlllevelTrainTargetTimeConfig()
        {
            var list = ISysTrainGradeBL.GetAllTrainGrade();
            var model = AllSystemConfigs.Find(p => p.ConfigType == 13);

            //list.Add(model);
            ViewBag.list = list;


            ViewBag.model = model;
            return View();
        }

        public ActionResult TrainTimeLimitConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 14);
            ViewBag.model = model;
            return View();
        }

        public ActionResult YearCompletionTimesConfig()
        {
            return View();
        }

        public ActionResult CPAYealTargetHoursConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 16);
            ViewBag.model = model;
            return View();
        }

        public ActionResult CPATargetTrainCycleTimeConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 17);
            ViewBag.model = model;
            return View();
        }

        public ActionResult CourseUpperLimitConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 18);
            ViewBag.model = model;
            return View();
        }

        public ActionResult YearCourseRescindNumberConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 19);
            ViewBag.model = model;
            return View();
        }

        public ActionResult YearCourseFillUpNumberConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 20);
            ViewBag.model = model;
            return View();
        }

        public ActionResult QueuechangeNormalNumberConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 21);
            ViewBag.model = model;
            return View();
        }

        public ActionResult LeaveTimeConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 22);
            ViewBag.model = model;
            return View();
        }

        public ActionResult FillUpTimeConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 23);
            ViewBag.model = model;
            return View();
        }

        public ActionResult ConcentrationTeachTimeConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 24);
            ViewBag.model = model;
            return View();
        }

        public ActionResult AfterVideoJoinTestConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 25);
            ViewBag.model = model;
            return View();
        }

        public ActionResult TestAnswerTimeConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 26);
            ViewBag.model = model;
            return View();
        }

        public ActionResult TestMaxNumberConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 27);
            ViewBag.model = model;
            return View();
        }

        public ActionResult DisciplineClassDeductionConfig()
        {
            return View();
        }

        public ActionResult ClassOpingTimeConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 29);
            ViewBag.model = model;
            return View();
        }

        public ActionResult EvaluationRewardTimeLimitConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 30);
            ViewBag.model = model;
            return View();
        }

        public ActionResult CompletionOnlineTestTimes()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 33);
            ViewBag.model = model;

            return View();
        }

        /// <summary>
        /// 综合评分
        /// </summary>
        /// <returns></returns>
        public ActionResult NewAllScoreConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 34);
            // ViewBag.model = model;
            return View(model);
        }

        public ActionResult NewAttendanceScoreConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 35);
            ViewBag.model = model;
            return View();
        }

        public ActionResult NewRewardScoreConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 36);
            ViewBag.model = model;
            return View();
        }

        public ActionResult DeptEvaluationConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 37);
            ViewBag.model = model;
            return View();
        }

        public ActionResult NewAttendanceSingle()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 38);
            ViewBag.model = model;
            return View();
        }

        public ActionResult IsUnionStudyConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 39);
            ViewBag.model = model;
            return View();
        }

        public ActionResult NewAttendanceScore()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 35);
            ViewBag.model = model;
            string chidao = "";
            if (model.ConfigValue != "")
            {
                for (int i = 0; i < model.ConfigValue.Split(';').Length; i++)
                {
                    var type = model.ConfigValue.Split(';')[i].Split(',')[0];
                    switch (type)
                    {
                        case "0":
                            chidao += "<tr type='chidao'>";
                            chidao += "<td>迟到</td>";
                            break;
                        case "1":
                            chidao += "<tr type='zaotui'>";
                            chidao += "<td>早退</td>";
                            break;
                        case "2":
                            chidao += "<tr type='queqin'>";
                            chidao += "<td>缺勤</td>";
                            break;
                        case "3":
                            chidao += "<tr type='chidao_zaotui'>";
                            chidao += "<td>迟到/早退</td>";
                            break;
                    }
                    chidao += "<td>" + model.ConfigValue.Split(';')[i].Split(',')[1] + "</td>";
                    chidao += "<td>" + model.ConfigValue.Split(';')[i].Split(',')[2] + "</td>";
                    chidao += "<td>" + model.ConfigValue.Split(';')[i].Split(',')[3] + "</td>";
                    chidao += "<td><a class='icon idel' title='删除' onclick='fdelete(this)'></a></td>";
                    chidao += "</tr>";
                }

            }
            ViewBag.chidao = chidao;
            return View();
        }

        public ActionResult NewRewardScoreSingle()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 40);
            ViewBag.model = model;
            return View();
        }

        public ActionResult NewRewardScore()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 36);
            ViewBag.model = model;
            string chidao = "";
            if (model.ConfigValue != "")
            {
                for (int i = 0; i < model.ConfigValue.Split(';').Length; i++)
                {
                    chidao += "<td>" + model.ConfigValue.Split(';')[i].Split(',')[0] + "</td>";
                    chidao += "<td>" + model.ConfigValue.Split(';')[i].Split(',')[1] + "</td>";
                    chidao += "<td>" + model.ConfigValue.Split(';')[i].Split(',')[2] + "</td>";
                    chidao += "<td><a class='icon idel' title='删除' onclick='fdelete(this)'></a></td>";
                    chidao += "</tr>";
                }

            }
            ViewBag.chidao = chidao;
            return View();
        }


        public ActionResult BuMenunsubscribe()
        {
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 41);
            return View();
        }

        public ActionResult TimeDistribution()
        {
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 42);
            return View();
        }

        /// <summary>
        /// 部门分所过期课程学时折算分布
        /// </summary>
        /// <returns></returns>
        public ActionResult TimeoutDistribution()
        {
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 50);
            return View();
        }

        public ActionResult AnnualPlansAndCurriculumConfiguration()
        {
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 43);
            return View();
        }

        public ActionResult AwardTime()
        {
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 44);
            return View();
        }

        public ActionResult MonthPlan()
        {
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 45);
            return View();
        }

        public ActionResult AttendceDeadline()
        {
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 46);
            return View();
        }

        /// <summary>
        /// 过时课程考勤
        /// </summary>
        /// <returns></returns>
        public ActionResult TimeOutAttendceDeadline()
        {
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 51);
            return View();
        }

        public ActionResult OrderTimesLimited()
        {
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 47);
            return View();
        }

        public ActionResult Depunsubscribe()
        {
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 48);
            return View();
        }

        public ActionResult YearPlanTimeSetting()
        {
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 49);
            return View();
        }

        public ActionResult VedioCourseZheSuan()
        {
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 53);
            return View();
        }
        /// <summary>
        /// 计划开设课程学时数图表区间配置
        /// </summary>
        /// <returns></returns>
        public ActionResult SectionConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 56);
            ViewBag.model = model;

            string chidao = "";
            if (model.ConfigValue != "")
            {

                for (int i = 0; i < model.ConfigValue.Split(',').Length; i++)
                {
                    chidao += "<tr><td>" + model.ConfigValue.Split(',')[i].Split(':')[0] + "</td>";
                    chidao += "<td>" + model.ConfigValue.Split(',')[i].Split(':')[1] + "</td>";
                    chidao += "<td><a class='icon idel' title='删除' onclick='fdelete(this)'></a></td>";
                    chidao += "</tr>";
                }

            }
            ViewBag.chidao = chidao;

            return View();
        }

        /// <summary>
        /// 纳入考核范围
        /// </summary>
        /// <returns></returns>
        public ActionResult InputDepCheck()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 54);
            ViewBag.html = new MvcHtmlString(GetDeptTreeSearchStr("deptNodeClick(this)", "").ToString());
            ViewBag.checkhtml = new MvcHtmlString(GetCheckDeptTreeSearchStr(model.ConfigValue, ""));
            return View(model);
        }
        public ActionResult DepIsCPA()
        {
            ViewBag.depcpa = AllSystemConfigs.Find(p => p.ConfigType == 55).ConfigValue;
            return View();
        }

        public ActionResult BackImageConfig()
        {
            return View();
        }

        public ActionResult BackImageUpload()
        {
            return View();
        }

        public ActionResult BackImageEdit(int id)
        {

            var model = noteBL.GetImageSingle(" ID=" + id).FirstOrDefault();

            ViewBag.id = id;
            return View(model);
        }
        #endregion

        public JsonResult fUpdateParamConfig(int configType, string configValue, int useType = 0)
        {
            //考勤累加扣分时候
            if (useType == 1 && configType == 38)
            {
                paramconfigBL.updateUseType(35, 38);
            }

            //评估累加扣分时候
            if (useType == 1 && configType == 40)
            {
                paramconfigBL.updateUseType(36, 40);
            }
            if (configType == 54)
            {
                RefreshCheckUser();
            }

            if (paramconfigBL.UpadteSys_ParamConfig(configType, configValue, useType))
            {
                lock (obj)
                {
                    RefreshSystemConfigsCache();
                }

                //AllSystemConfigs.Find(p => p.ConfigType == configType).ConfigValue = configValue;
                return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        #region 纳入考核范围 树结构
        /// <summary>
        /// 大树
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="type"></param>
        /// <param name="clickFunction"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult GetDeptTree(string clickFunction = "deptNodeClick(this)", string name = "")
        {
            //string html = GetDeptTreeStr(flag, type, clickFunction).ToString();
            string html = GetDeptTreeSearchStr(clickFunction, name).ToString();
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询树结构
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="type"></param>
        /// <param name="clickFunction"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public StringBuilder GetDeptTreeSearchStr(string clickFunction, string name)
        {
            var treeStr = new StringBuilder();

            if (string.IsNullOrWhiteSpace(name))
            {
                searchDeptIds = AllDepartments.Select(p => p.DepartmentId).ToList();
                SearchDepartments = AllDepartments;
            }
            else
            {
                searchDeptIds = AllDepartments.Where(p => p.DeptName.Contains(name)).Select(p => p.DepartmentId).ToList();
                List<int> listIds = new List<int>();
                for (int i = 0; i < searchDeptIds.Count; i++)
                {
                    GetParentDeptId(searchDeptIds[i], listIds);
                }
                if (listIds.Count == 0)
                    return treeStr;
                SearchDepartments = AllDepartments.Where(p => listIds.Contains(p.DepartmentId)).ToList();
            }

            string prefix = "";

            prefix = "<input type='checkbox' class='fll'  id='{0}' value='{4}' />";

            treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview w700\">");
            GetDeptTreeSearchChild(SearchDepartments.FindAll(p => p.ParentDeptId == 0), treeStr, clickFunction, prefix);

            treeStr.AppendLine("</ul>");
            return treeStr;
        }

        /// <summary>
        /// 查父级
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="list"></param>
        private void GetParentDeptId(int deptId, List<int> list)
        {
            if (deptId > 0)
            {
                list.Add(deptId);
                var dept = AllDepartments.Find(p => p.DepartmentId == deptId);
                GetParentDeptId(dept.ParentDeptId, list);
            }
        }

        /// <summary>
        /// 查询子级
        /// </summary>
        /// <param name="deptList"></param>
        /// <param name="html"></param>
        /// <param name="clickFunction"></param>
        /// <param name="prefix"></param>
        private void GetDeptTreeSearchChild(List<Sys_Department> deptList, StringBuilder html,
                                      string clickFunction = "deptNodeClick(this)", string prefix = "")
        {
            if (deptList.Count > 0)
            {
                foreach (Sys_Department v in deptList)
                {
                    html.AppendFormat(
                        "<li class='pNote'>" + (v.ParentDeptId < 2 ? "" : prefix) +
                        "<a title='{2}' id='{1}' {3} {5}>{2}</a>",
                        v.DepartmentId,
                        v.DepartmentId + "_" + v.ParentDeptId,
                        v.DeptName,
                        ((string.IsNullOrWhiteSpace(clickFunction) || (!searchDeptIds.Contains(v.DepartmentId))) ? "" : "onclick=\"" + clickFunction + "\""),
                        v.DepartmentId + "_" + v.ParentDeptId + "_" + v.DeptName,
                        searchDeptIds.Contains(v.DepartmentId) ? "style='cursor:pointer;'" : "style='cursor:default;'");
                    if (SearchDepartments.Exists(p => p.ParentDeptId == v.DepartmentId))
                    {
                        html.AppendLine("<ul>");
                        GetDeptTreeSearchChild(SearchDepartments.FindAll(p => p.ParentDeptId == v.DepartmentId), html,
                            clickFunction, prefix);
                        html.AppendLine("</ul>");
                    }
                    html.AppendLine("</li>");
                }
            }
        }

        /// <summary>
        /// 考核的
        /// </summary>
        /// <param name="clickFunction"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult GetCheckDeptTree(string name = "")
        {
            //string html = GetDeptTreeStr(flag, type, clickFunction).ToString();
            var model = AllSystemConfigs.Find(p => p.ConfigType == 54);
            if (model.ConfigValue == "")
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                string html = GetCheckDeptTreeSearchStr(model.ConfigValue, name).ToString();
                return Json(html, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 考核范围的列表
        /// </summary>
        /// <param name="deptIDs"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public String GetCheckDeptTreeSearchStr(string deptIDs, string name)
        {
            StringBuilder treeStr = new StringBuilder();
            var listID = deptIDs.Split(',').ToList();

            var checkDeptList = AllDepartments.Where(p => (listID.Contains(p.DepartmentId.ToString()) && p.DeptName.Contains(name))).ToList();
            SearchcheckDepartments = checkDeptList;
            var prefix = "<input type='checkbox' class='fll'  id='{0}' value='{4}' />";
            var clickFunction = "checkdeptNodeClick(this)";
            treeStr.AppendLine("<ul id=\"navigation\" class=\"treeview w700\"><li><ul>");
            if (!string.IsNullOrEmpty(deptIDs))
            {
                GetCheckChildDeptID(checkDeptList.OrderBy(p => p.IsMain).ToList(), new List<int>(), treeStr, clickFunction, prefix);
            }
            else
            {
                treeStr.AppendFormat("<li class='pNote'><a>暂无数据</a></li>");
            }
            treeStr.AppendLine("</ul></li></ul>");
            return treeStr.ToString();


        }

        public void GetCheckChildDeptID(List<Sys_Department> deptList, List<int> ListID, StringBuilder html,
                                      string clickFunction = "deptNodeClick(this)", string prefix = "")
        {
            foreach (var v in deptList)
            {
                if (!ListID.Contains(v.DepartmentId))
                {
                    ListID.Add(v.DepartmentId);

                    html.AppendFormat(
                       "<li class='pNote'>" + (v.ParentDeptId < 2 ? "" : prefix) +
                       "<a title='{2}' id='check_{1}' {3} {5}>{2}</a>",
                       v.DepartmentId,
                       v.DepartmentId + "_" + v.ParentDeptId,
                       v.DeptName,
                       ((string.IsNullOrWhiteSpace(clickFunction) || (!searchDeptIds.Contains(v.DepartmentId))) ? "" : "onclick=\"" + clickFunction + "\""),
                       v.DepartmentId + "_" + v.ParentDeptId + "_" + v.DeptName,
                       searchDeptIds.Contains(v.DepartmentId) ? "style='cursor:pointer;'" : "style='cursor:default;'");

                    if (SearchcheckDepartments.Count(p => p.ParentDeptId == v.DepartmentId) > 0)
                    {
                        html.AppendLine("<ul>");
                        GetCheckChildDeptID(SearchcheckDepartments.Where(p => p.ParentDeptId == v.DepartmentId).ToList(), ListID, html, clickFunction, prefix);
                        html.AppendLine("</ul>");
                    }
                    html.AppendLine("</li>");
                }
            }
        }
        #endregion



        #region 其他形式免修 ConfigType从60-62
        public ActionResult OtherApplyConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 60);
            ViewBag.model = model;
            return View();
        }

        public ActionResult OtherApprovalConfig()
        {
            var model = AllSystemConfigs.Find(p => p.ConfigType == 61);
            ViewBag.model = model;
            return View();
        }

        public ActionResult OtherlevelConfig()
        {
            ViewBag.trainGrade = ISysTrainGradeBL.GetAllTrainGrade();
            var model = AllSystemConfigs.Find(p => p.ConfigType == 62);
            ViewBag.model = model;
            return View();
        }

        public ActionResult OtherFromsProjectConfiguration(int year)
        {
            ViewBag.IsEdit = DateTime.Now.Year <=year ? 1 : 0;
            ViewBag.model = AllSystemConfigs.Find(p => p.ConfigType == 65 && p.LastUpdateTime.Year == year);
            ViewBag.year = year;
            return View();
        }

        public ActionResult OtherFromsProjectConfigurationAdd(int year,int id = 0)
        {

            //ViewBag.From = fFree_otherfrom.GetOtherFromsById(id);
            Free_OtherFroms model = new Free_OtherFroms();
            ViewBag.year = year;
            if (id != 0)
            {
                model = fFree_otherfrom.GetOtherFromsById(id);
            }

            return View(model);
        }

        public ActionResult OtherFromsProjectConfigurationConfig(int year)
        {
            ViewBag.trainGrade = AllTrainGrade;
            ViewBag.year = year;
            var model = AllSystemConfigs.Find(p => p.ConfigType == 65 && p.LastUpdateTime.Year == year);           
           return View(model);         
        }





        #endregion

        #region 添加其他形式
        public JsonResult GetAddOther(string name, int type,int year)
        {

            Free_OtherFroms model = new Free_OtherFroms();
            model.FromName = name;
            model.FromTime = DateTime.Now;
            model.FromType = type;
            model.IsDelete = 0;
            model.Year = year;

            fFree_otherfrom.AddOtherFroms(model);

            return Json(new
            {
                result = 0,

            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUpdateOther(int id, string name, int type, int year)
        {

            Free_OtherFroms model = new Free_OtherFroms();
            model.FromName = name;
            model.FromTime = DateTime.Now;
            model.FromType = type;
            model.IsDelete = 0;
            model.Year = year;
            model.Id = id;

            if (fFree_otherfrom.UpdateOtherFroms(model))
            {
                return Json(new
                {
                    result = 0,

                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    result = 1,

                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOthreFromList(string name,int year, int pageSize = 10, int pageIndex = 1)
        {
            string sql = " isdelete=0 and year=" + year;
            int limit = 0;
            if (!string.IsNullOrEmpty(name))
            {
                sql += " and fromname like '%" + name + "%'";
            }
            var list = fFree_otherfrom.GetOtherFromsList(sql);
            limit = list.Count();

            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return Json(new
            {
                dataList = list,
                recordCount = limit

            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult fDelete(string id)
        {
            var b = fFree_otherfrom.DeleteOtherFroms(id);
            return Json(new
           {
               result = 1

           }, JsonRequestBehavior.AllowGet);
        }
        #endregion



    }
}
