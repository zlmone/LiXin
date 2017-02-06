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

namespace LiXinControllers
{
    public partial class ParamConfigController : BaseController
    {
        protected  object obj=new object();

        protected ISys_ParamConfig paramconfigBL;
        protected ISys_TrainGrade ISysTrainGradeBL;


        protected IExamTest ExamTestBL;
        protected ExaminationBL ExamDB;

        protected ICl_CpaLearnStatus ICpaLearnStatusBL ;


        public ParamConfigController(ISys_ParamConfig _paramconfigBL, ISys_TrainGrade _ISysTrainGradeBL, IExamTest _ExamTestBL, ExaminationBL _ExamDB, ICl_CpaLearnStatus _ICpaLearnStatusBL)
        {
            paramconfigBL = _paramconfigBL;
            ISysTrainGradeBL = _ISysTrainGradeBL;
            ExamTestBL = _ExamTestBL;
            ExamDB = _ExamDB;
            ICpaLearnStatusBL = _ICpaLearnStatusBL;
        }

        #region 呈现页面        

        public ActionResult ParamConfig()
        {
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
          // ICpaLearnStatusBL.FInsertNewListCpa();
            

            //var aa = httpreq("http://video.esnai.net/lixin/2012/20120705s/index.asp?username=uuu");


            //var student = ExamDB.GetExamSendStudentListWithCourseId(4).Where(p => p.UserID >= 5853).OrderBy(p=>p.UserID);

            //var dt = new DataTable();
            //dt.Columns.Add("序号", typeof(string));
            //dt.Columns.Add("ID", typeof(string));

            //foreach (var item in student)
            //{
            //    dt.Rows.Add(item._id,item.UserID);
            //}

            //var dtList = new List<DataTable>();
            //string strFileName = "课程报名情况";
            //dtList.Add(dt);
            //var export = new Spreadsheet();
            //export.ExportChart(new List<LiXinModels.ChartModel>(), dtList, HttpContext, strFileName);

            //int a = student.Count();

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
            StreamReader sr = new StreamReader(receiveStream,respseCode);
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
            if (model.ConfigValue!= "")
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
                    chidao +="</tr>";
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

        #endregion
        
        public JsonResult fUpdateParamConfig(int configType,string configValue)
        {
            
            if (paramconfigBL.UpadteSys_ParamConfig(configType, configValue))
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



        public void testEmai(){
            LiXinBLL.Email em = new LiXinBLL.Email();            
           
            EmailInforMation model = new EmailInforMation();
            model.Context = "啊啊啊";

            //model.UserName = "LMS@retechcorp.com";
            //model.PassWord = "lmsretech654321";
            //model.From = "retechcorp@retechcorp.com";
            //model.Host = "mail.retechcorp.com";

            model.UserName = "307959185@qq.com";
            model.PassWord = "xxxxxxxx";
            model.From = "307959185@qq.com";
            model.Host = "smtp.qq.com";

            List<string> a = new List<string>();
            a.Add("maojiayuan@126.com");
            model.ToList =a ;
            em.SendEmail(model,0);
        }

    }
}
