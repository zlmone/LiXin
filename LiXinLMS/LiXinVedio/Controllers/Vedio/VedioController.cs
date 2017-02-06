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


namespace Controllers
{
    public class VedioController : Controller
    {
        protected Cl_CpaLearnStatusBL ICpaLearnStatusBL = new Cl_CpaLearnStatusBL();
        protected Co_CourseBL CoCourseBL=new Co_CourseBL();
        protected Cl_LearnVideoInforBL IClLearnVideoInforBL=new Cl_LearnVideoInforBL();
        protected Co_CourseResourceBL ICoCourseResourceBL=new Co_CourseResourceBL();
        private Cl_VideoManageBL iClVideoManageBL=new Cl_VideoManageBL();
        
        //VideoShow?id=12&LearnId=11&ResourseId=35
        public ActionResult Vedio(int id=0, int LearnId = 0, int ResourseId = 0)
        {
            var t = 0;
            var video = iClVideoManageBL.GetCl_VideoManageList(out t, "Id=" + id, 0, 1, "ORDER BY Cl_VideoManage.Id,Cl_VideoManage.LastUpdateTime DESC").FirstOrDefault();

            if (video != null)
            { 
                ViewBag.url = Url.Content("/UploadFiles/UFCOVideo/" + video.Path);

                var model = IClLearnVideoInforBL.Get(LearnId, ResourseId);

                ViewBag.Progress = model.Progress;
                ViewBag.video = video;
            }

            
            return View();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LearnVideoInforId"></param>
        /// <param name="allallduration">视频总时长</param>
        /// <param name="Co_CourseResourceId"></param>
        /// <param name="tt">视频当前播放进度</param>
        /// <returns></returns>
        public JsonResult FUpdateProgress(int LearnId, string allallduration, string tt, int ResourseId,int UserId)
        {
            double Progress = Convert.ToDouble(tt) / Convert.ToDouble(allallduration) * 100;

            var CpaLearnStatus = ICpaLearnStatusBL.GetCl_CpaLearnStatusByLearnId(LearnId, UserId);
            var course = CoCourseBL.GetCo_Course(CpaLearnStatus.CourseID);

            //var Sys_ParamConfig = AllSystemConfigs.Find(p => p.ConfigType == 25);//读取参数配置 观看课程时长达到视频总时长的

            var learn = IClLearnVideoInforBL.Get(LearnId, ResourseId);
            if (learn.Progress < Convert.ToDecimal(Progress))
            { 
                IClLearnVideoInforBL.UpdateProgress(Convert.ToDecimal(Progress), LearnId, ResourseId);
            }

            var courseResource = ICoCourseResourceBL.GetVedioList(CpaLearnStatus.CourseID,UserId);
            //var hege = courseResource.Where(p => p.Progress >= Convert.ToDecimal(Sys_ParamConfig.ConfigValue)).ToList();
            var hege = courseResource.Where(p => p.Progress >= Convert.ToDecimal(course.ReturnTimes)).ToList();
            if (courseResource.Count == hege.Count)
            {
                if (course.IsPing == 0 && course.IsTest == 0)
                {
                    if (CpaLearnStatus.Progress == 0)
                    {
                        ICpaLearnStatusBL.UpdateGetCl_CpaLearnStatusByCourseId(CpaLearnStatus.CourseID,UserId, course.CourseLength, 1);

                        if (course.IsCPA == 1)
                        {
                            Cl_CpaLearnStatus cls = new Cl_CpaLearnStatus();
                            cls.CourseID = course.Id;
                            cls.UserID = UserId;
                            cls.IsAttFlag = 0;
                            cls.Progress = 0;
                            cls.LearnTimes = 0;
                            if (course.IsMust == 1)
                            {
                                cls.GetLength = course.CourseLength*Convert.ToDecimal(0.5);
                            }
                            if (course.IsMust == 0)
                            {
                                cls.GetLength = course.CourseLength;
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


    }
}
