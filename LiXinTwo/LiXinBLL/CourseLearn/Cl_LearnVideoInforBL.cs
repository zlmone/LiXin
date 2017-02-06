using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiXinDataAccess.CourseLearn;
using LiXinInterface.CourseLearn;
using LiXinModels.CourseLearn;

namespace LiXinBLL.CourseLearn
{
    public class Cl_LearnVideoInforBL : ICl_LearnVideoInfor
    {
        private Cl_LearnVideoInforDB clLearnVideoInforDb;

        public Cl_LearnVideoInforBL()
        {
            clLearnVideoInforDb=new Cl_LearnVideoInforDB();
        }

        public void SubscribeVedio(Cl_LearnVideoInfor model)
        {
             clLearnVideoInforDb.SubscribeVedio(model);
        }

        public bool IsImport(int LearnId, int ResourseId)
        {
            return clLearnVideoInforDb.IsImport(LearnId, ResourseId);
        }


        public bool UpdateProgress(decimal Progress, int LearnId, int ResourseId, string learnLength = "0")
        {
            return clLearnVideoInforDb.UpdateProgress(Progress, LearnId, ResourseId, learnLength);
        }

        public bool UpdateLearnTimes(int LearnId, int ResourseId)
        {
            return clLearnVideoInforDb.UpdateLearnTimes(LearnId, ResourseId);
        }

        public List<Cl_LearnVideoInfor> GetCl_LearnVideoInforList(int LearnId)
        {
            return clLearnVideoInforDb.GetCl_LearnVideoInforList(LearnId);
        }

        public bool UpdateCl_LearnVideoInforForProgress(int learnid)
        {
            return clLearnVideoInforDb.UpdateCl_LearnVideoInforForProgress(learnid);
        }

        public Cl_LearnVideoInfor Get(int LearnId, int ResourseId)
        {
            return clLearnVideoInforDb.Get(LearnId, ResourseId);
        }
    }
}
