using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LiXinInterface.CourseLearn;
using LiXinInterface.CourseManage;
using LiXinModels.CourseManage;

namespace LiXinControllers
{
    public class VideoController : BaseController
    {
        private ICl_VideoManage iClVideoManageBL;

        private ICl_LearnVideoInfor clLearnVideoInforBL;

        public VideoController(ICl_VideoManage _iClVideoManageBL, ICl_LearnVideoInfor _clLearnVideoInforBL)
        {
            iClVideoManageBL = _iClVideoManageBL;
            clLearnVideoInforBL = _clLearnVideoInforBL;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">学习记录learnID</param>
        /// <param name="coursecourseid">课程ID</param>
        /// <param name="ResourseId">资源ID</param>
        /// <returns></returns>
        public ActionResult VideoShow(int id, int LearnId = 0, int ResourseId = 0)
        {
            var t = 0;
            var video = iClVideoManageBL.GetCl_VideoManageList(out t, "Id=" + id, 0, 1, "ORDER BY Cl_VideoManage.Id,Cl_VideoManage.LastUpdateTime DESC").FirstOrDefault();

            ViewBag.url = Url.Content("/VedioPlay/" + video.Path);

            var model = clLearnVideoInforBL.Get(LearnId, ResourseId);

            ViewBag.Progress = model.Progress;


            ViewBag.video = video;
            return View();
        }

    }
}
