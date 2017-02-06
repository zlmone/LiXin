using LiXinDataAccess.NewCourseManage;
using LiXinInterface.NewCourseManage;
using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinBLL.NewCourseManage
{
    public class New_CpaLearnStatusBL : INew_CpaLearnStatus
    {
        private New_CpaLearnStatusDB _cpaLearnStatusDb = new New_CpaLearnStatusDB();

        public void SubscribeCPALearnStatus(New_CpaLearnStatus model)
        {
            _cpaLearnStatusDb.SubscribeCPALearnStatus(model);
        }


        public bool UpdateCPALearnStatus(New_CpaLearnStatus model)
        {
            return _cpaLearnStatusDb.UpdateCPALearnStatus(model);
        }

        public New_CpaLearnStatus GetCl_CpaLearnStatusByCourseId(int CourseId, int UserId, string whereStr = " 1 = 1 ")
        {
            return _cpaLearnStatusDb.GetCl_CpaLearnStatusByCourseId(CourseId, UserId, whereStr);
        }

        public New_CpaLearnStatus GetNew_CpaLearnStatusByLearnId(int LearnId, int UserId)
        {
            return _cpaLearnStatusDb.GetNew_CpaLearnStatusByLearnId(LearnId, UserId);
        }


    }
}
