using LiXinModels.NewCourseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiXinInterface.NewCourseManage
{
    public interface INew_CpaLearnStatus
    {
        void SubscribeCPALearnStatus(New_CpaLearnStatus model);


        bool UpdateCPALearnStatus(New_CpaLearnStatus model);

        New_CpaLearnStatus GetCl_CpaLearnStatusByCourseId(int CourseId, int UserId, string whereStr = " 1 = 1 ");

        New_CpaLearnStatus GetNew_CpaLearnStatusByLearnId(int LearnId, int UserId);
       
    }
}
