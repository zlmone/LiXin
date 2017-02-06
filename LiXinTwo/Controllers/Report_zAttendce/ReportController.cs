using LiXinBLL.Report_CPA;
using LiXinBLL.User;
using LiXinCommon;
using LiXinInterface.Report_CPA;
using LiXinInterface.Report_zAttendce;
using LiXinInterface.User;
using LiXinModels;
using LiXinModels.Report_Vedio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace LiXinControllers
{
    public partial class Report_zAttendceController : BaseController
    {
        protected IUser IUserBL = new UserBL();
        protected IReport_CPA Report_CPABL = new Report_CPABL();

        /// <summary>
        /// 全年出勤情况
        /// </summary>
        /// <returns></returns>
        public ActionResult ReZAttendceManageList(int flg = 0)
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
                if (Session["ReZAttendce0"] != null)
                {
                    string sess = Session["ReZAttendce0"].ToString();
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
        /// 全年出勤情况 分所
        /// </summary>
        /// <returns></returns>
        public ActionResult ReFAttendceManageList(int flg = 0)
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
                if (Session["ReZAttendce1"] != null)
                {
                    string sess = Session["ReZAttendce1"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.Deptids = att[0];
                    ViewBag.PayGrade = att[1];
                    ViewBag.TrainGrade = att[2];
                    ViewBag.year = att[3] == "" ? DateTime.Now.Year.ToString() : att[3];
                    ViewBag.isCPA = att[4];
                }
            }
            return View("ReZAttendceManageList");
        }

        /// <summary>
        /// 完成情况明细表
        /// </summary>
        /// <param name="ReportType">0:总所 1：部门/分所</param>
        /// <param name="Deptid">部门ID</param>
        /// <returns></returns>
        public ActionResult CompletionDetial(string Deptid = "", int ReportType = 0)
        {
            List<string> allTrainGrade = AllTrainGrade;
            ViewBag.allTrainGrade = allTrainGrade;
            ViewBag.ReportType = ReportType;
            ViewBag.Deptid = Deptid;

            var cpa = Report_CPABL.CPAStartAndEnd(AllSystemConfigs.Find(p => p.ConfigType == 8));

            int a = 0;
            if (DateTime.Now.Year == cpa[1].Year)
            {
                a = 1;
            }
            else
            {
                a = 0;
            }
            ViewBag.cpa = a;

            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReportType">0:总所 1：部门/分所</param>
        /// <param name="Deptid">部门ID</param>
        /// <returns></returns>
        public ActionResult FCompletionDetial(string Deptid = "", int ReportType = 1)
        {
            List<string> allTrainGrade = AllTrainGrade;
            ViewBag.allTrainGrade = allTrainGrade;
            ViewBag.ReportType = ReportType;
            ViewBag.Deptid = Deptid;
            var cpa = Report_CPABL.CPAStartAndEnd(AllSystemConfigs.Find(p => p.ConfigType == 8));

            int a = 0;
            if (DateTime.Now.Year == cpa[1].Year)
            {
                a = 1;
            }
            else
            {
                a = 0;
            }
            ViewBag.cpa = a;
            return View("CompletionDetial");
        }

        public ActionResult SelectDept(string naru, int ReportType, int Deptid = 0)
        {
            ViewBag.Deptid = Deptid;
            ViewBag.naru = naru;
            ViewBag.ReportType = ReportType;
            return View();
        }


        /// <summary>
        /// 培训目标完成情况
        /// </summary>
        /// <returns></returns>
        public ActionResult CPACompelte(int flg = 0)
        {
            ViewBag.allTrainGrade = AllTrainGrade;
            ViewBag.type = 0;

            //返回记住查询条件
            ViewBag.Deptids = "";
            ViewBag.PayGrade = "";
            ViewBag.TrainGrade = "";
            ViewBag.year = DateTime.Now.Year.ToString();
            ViewBag.CPARelationship = "";

            if (flg == 1)
            {
                if (Session["CPACompelte0"] != null)
                {
                    string sess = Session["CPACompelte0"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.Deptids = att[0];
                    ViewBag.PayGrade = att[1];
                    ViewBag.TrainGrade = att[2];
                    ViewBag.year = att[3] == "" ? DateTime.Now.Year.ToString() : att[3];
                    ViewBag.CPARelationship = att[4];
                }
            }
            return View();
        }

        /// <summary>
        /// 培训目标完成情况 分所  
        /// </summary>
        /// <returns></returns>
        public ActionResult FCPACompelte(int flg = 0)
        {
            ViewBag.allTrainGrade = AllTrainGrade;
            ViewBag.type = 1;

            //返回记住查询条件
            ViewBag.Deptids = "";
            ViewBag.PayGrade = "";
            ViewBag.TrainGrade = "";
            ViewBag.year = DateTime.Now.Year.ToString();
            ViewBag.CPARelationship = "";

            if (flg == 1)
            {
                if (Session["CPACompelte1"] != null)
                {
                    string sess = Session["CPACompelte1"].ToString();
                    string[] att = Regex.Split(sess, "㉿", RegexOptions.IgnoreCase);
                    ViewBag.Deptids = att[0];
                    ViewBag.PayGrade = att[1];
                    ViewBag.TrainGrade = att[2];
                    ViewBag.year = att[3] == "" ? DateTime.Now.Year.ToString() : att[3];
                    ViewBag.CPARelationship = att[4];
                }
            }
            return View("CPACompelte");
        }


        /// <summary>
        /// 培训目标
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportAllScore(int type = 0)
        {
            ViewBag.type = type;
            return View();
        }


        /// <summary>
        /// 培训目标 分所
        /// </summary>
        /// <returns></returns>
        public ActionResult FReportAllScore(int type = 0)
        {
            ViewBag.type = type;
            return View();
        }


        public ActionResult CompletionDetialByUser(int userid, string url)
        {
            var user = IUserBL.Get(userid);
            ViewBag.User = user;
            ViewBag.url = url;
            ViewBag.CPA = user.CPAStatus;
            ViewBag.type = 0;
            return View();
        }

        public ActionResult FCompletionDetialByUser(int userid, string url)
        {
            var user = IUserBL.Get(userid);
            ViewBag.User = user;
            ViewBag.url = url;
            ViewBag.CPA = user.CPAStatus;
            ViewBag.type = 1;
            return View("CompletionDetialByUser");
        }

    }
}
