using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.NewCourseManage
{
    public interface INew_LearnVideoInfor
    {
        void SubscribeVedio(New_LearnVideoInfor model);

        bool IsImport(int LearnId, int ResourseId);

        bool UpdateProgress(decimal Progress, int LearnId, int ResourseId);

        bool UpdateLearnTimes(int LearnId, int ResourseId);

        List<New_LearnVideoInfor> GetCl_LearnVideoInforList(int LearnId);

        New_LearnVideoInfor Get(int LearnId, int ResourseId);

    }
}
