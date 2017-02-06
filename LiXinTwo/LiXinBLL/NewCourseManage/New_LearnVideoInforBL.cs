using LiXinDataAccess.NewCourseManage;
using LiXinInterface.NewCourseManage;
using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.NewCourseManage
{
    public class New_LearnVideoInforBL : INew_LearnVideoInfor
    {
        private New_LearnVideoInforDB newLearnVideoInforDb;

        public New_LearnVideoInforBL()
        {
            newLearnVideoInforDb = new New_LearnVideoInforDB();
        }


        public void SubscribeVedio(New_LearnVideoInfor model)
        {
            newLearnVideoInforDb.SubscribeVedio(model);
        }


        /// <summary>
        /// 根据学习视频ID和对应视频资源ID 查找是否已学过
        /// </summary>
        /// <param name="LearnId"></param>
        /// <param name="ResourseId"></param>
        /// <returns></returns>
        public bool IsImport(int LearnId, int ResourseId)
        {
            return newLearnVideoInforDb.IsImport(LearnId,ResourseId);
        }

        public bool UpdateProgress(decimal Progress, int LearnId, int ResourseId)
        {
            return newLearnVideoInforDb.UpdateProgress(Progress, LearnId, ResourseId);
        }

        public bool UpdateLearnTimes(int LearnId, int ResourseId)
        {
            return newLearnVideoInforDb.UpdateLearnTimes(LearnId, ResourseId);
        }

        public List<New_LearnVideoInfor> GetCl_LearnVideoInforList(int LearnId)
        {
            return newLearnVideoInforDb.GetCl_LearnVideoInforList(LearnId);
        }

        public New_LearnVideoInfor Get(int LearnId, int ResourseId)
        {
            return newLearnVideoInforDb.Get(LearnId, ResourseId);
        }
    }
}
