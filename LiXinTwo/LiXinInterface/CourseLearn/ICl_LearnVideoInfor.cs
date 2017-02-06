using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinModels.CourseLearn;

namespace LiXinInterface.CourseLearn
{
    public interface ICl_LearnVideoInfor
    {
         void SubscribeVedio(Cl_LearnVideoInfor model);

         bool IsImport(int LearnId, int ResourseId);

        /// <summary>
        /// 修改播放进度
        /// </summary>
        /// <param name="Progress"></param>
        /// <param name="LearnId"></param>
        /// <param name="ResourseId"></param>
        /// <returns></returns>
         bool UpdateProgress(decimal Progress, int LearnId, int ResourseId, string learnLength = "0");


        /// <summary>
        /// 修改播放次数
        /// </summary>
        /// <param name="LearnTimes"></param>
        /// <param name="LearnId"></param>
        /// <param name="ResourseId"></param>
        /// <returns></returns>
        bool UpdateLearnTimes(int LearnId, int ResourseId);


        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="LearnId"></param>
        /// <returns></returns>
        List<Cl_LearnVideoInfor> GetCl_LearnVideoInforList(int LearnId);


        /// <summary>
        /// 视频进度清0.0
        /// </summary>
        /// <param name="learnid"></param>
        /// <returns></returns>
        bool UpdateCl_LearnVideoInforForProgress(int learnid);

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="LearnId"></param>
        /// <returns></returns>
        Cl_LearnVideoInfor Get(int LearnId, int ResourseId);
    }
}
