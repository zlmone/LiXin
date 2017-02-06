using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiXinBLL.CourseLearn;
using LiXinBLL.CourseManage;
using LiXinInterface;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinModels.CourseLearn;
using LiXinBLL.NewCourseManage;
using LiXinModels.NewCourseManage;
using LiXinBLL.SystemManage;


namespace Controllers
{
    public class VedioController : Controller
    {
        protected Cl_CpaLearnStatusBL ICpaLearnStatusBL = new Cl_CpaLearnStatusBL();
        protected Co_CourseBL CoCourseBL = new Co_CourseBL();
        protected Cl_LearnVideoInforBL IClLearnVideoInforBL = new Cl_LearnVideoInforBL();
        protected Co_CourseResourceBL ICoCourseResourceBL = new Co_CourseResourceBL();
        private Cl_VideoManageBL iClVideoManageBL = new Cl_VideoManageBL();


        protected New_CourseBL INew_Course = new New_CourseBL();
        protected New_CpaLearnStatusBL INew_CpaLearnStatus = new New_CpaLearnStatusBL();
        protected New_LearnVideoInforBL INew_LearnVideoInfor = new New_LearnVideoInforBL();
        protected Sys_ParamConfigBL ISys_ParamConfig = new Sys_ParamConfigBL();

        //public static decimal allvediotime = 0;
        //VideoShow?id=12&LearnId=11&ResourseId=35
        public ActionResult Vedio(int id = 0, int LearnId = 0, int ResourseId = 0, int UserId = 0, int type = 0)
        {
            //type:0是第一期视频课程 1：新进员工自学课程
            //记录上一次播放的总时间
            //allvediotime = ICpaLearnStatusBL.GetCl_CpaLearnStatusByLearnId(LearnId, UserId).VedioTime;

            var t = 0;
            var video = iClVideoManageBL.GetCl_VideoManageList(out t, "Id=" + id, 0, 1, "ORDER BY Cl_VideoManage.Id,Cl_VideoManage.LastUpdateTime DESC").FirstOrDefault();

            if (video != null)
            {
                if (type == 0)
                {
                    ViewBag.url = Url.Content("/UploadFiles/UFCOVideo/" + video.Path);

                    var model = IClLearnVideoInforBL.Get(LearnId, ResourseId);

                    ViewBag.Progress = model.Progress;
                    ViewBag.video = video;
                }
                else
                {
                    ViewBag.url = Url.Content("/UploadFiles/UFCOVideo/" + video.Path);

                    var model = INew_LearnVideoInfor.Get(LearnId, ResourseId);

                    ViewBag.Progress = model == null ? 0 : model.Progress;
                    ViewBag.video = video;
                }
            }

            return View();

        }
        #region
        /// <summary>
        /// 第一期视频方法
        /// </summary>
        /// <param name="LearnVideoInforId"></param>
        /// <param name="allallduration">视频总时长</param>
        /// <param name="Co_CourseResourceId"></param>
        /// <param name="tt">视频当前播放进度</param>
        /// <param name="learnLength">在线观看时间（分钟）</param>
        /// <returns></returns>
        public JsonResult FUpdateProgress(int LearnId, string allallduration, string tt, int ResourseId, int UserId, string learnLength = "0")
        {
            double Progress = Convert.ToDouble(tt) / Convert.ToDouble(allallduration) * 100;

            decimal allvediotime = 0;
            var clearn=ICpaLearnStatusBL.GetCl_CpaLearnStatusByLearnId(LearnId, UserId);
            if (clearn != null)
                allvediotime = clearn.VedioTime;

            //记录观看视频的总长度
            ICpaLearnStatusBL.UpdateVedioTime(LearnId, (allvediotime + Convert.ToDecimal(tt)));

            var CpaLearnStatus = ICpaLearnStatusBL.GetCl_CpaLearnStatusByLearnId(LearnId, UserId);
            var course = CoCourseBL.GetCo_Course(CpaLearnStatus.CourseID);

            //var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 25);//读取参数配置 观看课程时长达到视频总时长的

            var learn = IClLearnVideoInforBL.Get(LearnId, ResourseId);
            if (learn.Progress < Convert.ToDecimal(Progress))
            {
                IClLearnVideoInforBL.UpdateProgress(Convert.ToDecimal(Progress), LearnId, ResourseId, learnLength);
            }

            var courseResource = ICoCourseResourceBL.GetVedioList(CpaLearnStatus.CourseID, UserId);
            //var hege = courseResource.Where(p => p.Progress >= Convert.ToDecimal(Sys_ParamConfig.ConfigValue)).ToList();
            var hege = courseResource.Where(p => p.Progress >= Convert.ToDecimal(course.ReturnTimes)).ToList();
            if (courseResource.Count == hege.Count)
            {
                if (course.IsPing == 0 && course.IsTest == 0)
                {
                    if (CpaLearnStatus.Progress == 0)
                    {
                        decimal zhesuan = Convert.ToDecimal(ISys_ParamConfig.GetSys_ParamConfig(53).ConfigValue);
                        decimal courslength = course.CourseLength * (zhesuan / 100);

                        ICpaLearnStatusBL.UpdateGetCl_CpaLearnStatusByCourseId(CpaLearnStatus.CourseID, UserId, courslength, 1);

                        if (course.IsCPA == 1)
                        {
                            Cl_CpaLearnStatus cls = new Cl_CpaLearnStatus();
                            cls.CourseID = course.Id;
                            cls.UserID = UserId;
                            cls.IsAttFlag = 0;
                            cls.Progress = 0;
                            cls.LearnTimes = 0;
                            //cls.GetLength = course.CourseLength;
                            if (course.IsMust == 1)
                            {
                                cls.GetLength = courslength * Convert.ToDecimal(0.5);
                            }
                            if (course.IsMust == 0)
                            {
                                cls.GetLength = courslength;
                            }
                            cls.CpaFlag = 2;
                            cls.GradeStatus = 1;
                            cls.IsPass = 1;

                            ICpaLearnStatusBL.SubscribeCPALearnStatus(cls);
                        }
                    }

                }
            }


            return Json(new
            {
                result = 1
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 新进员工自学课程学习视频
        /// </summary>
        /// <param name="LearnId"></param>
        /// <param name="allallduration"></param>
        /// <param name="tt"></param>
        /// <param name="ResourseId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public JsonResult FNewUpdateProgress(int LearnId, string allallduration, string tt, int ResourseId, int UserId)
        {
            double Progress = Convert.ToDouble(tt) / Convert.ToDouble(allallduration) * 100;



            var CpaLearnStatus = INew_CpaLearnStatus.GetNew_CpaLearnStatusByLearnId(LearnId, UserId);
            var course = INew_Course.GetSingleCourse(CpaLearnStatus.CourseID);

            //var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 25);//读取参数配置 观看课程时长达到视频总时长的

            var learn = INew_LearnVideoInfor.Get(LearnId, ResourseId);
            if (learn.Progress < Convert.ToDecimal(Progress))
            {
                INew_LearnVideoInfor.UpdateProgress(Convert.ToDecimal(Progress), LearnId, ResourseId);
            }

            return Json(new
            {
                result = 1
            }, JsonRequestBehavior.AllowGet);
        }


    }
}
